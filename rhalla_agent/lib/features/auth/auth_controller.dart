import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/net/api_client.dart';
import '../../core/storage/secure_store.dart';
import 'auth_repository.dart';

enum AuthStatus { unknown, signedOut, signedIn }

class AuthState {
  const AuthState({required this.status, this.user, this.onboarded = false});

  final AuthStatus status;
  final AgentUser? user;
  final bool onboarded;

  AuthState copyWith({AuthStatus? status, AgentUser? user, bool? onboarded}) =>
      AuthState(
        status: status ?? this.status,
        user: user ?? this.user,
        onboarded: onboarded ?? this.onboarded,
      );

  static const initial = AuthState(status: AuthStatus.unknown);
}

class AuthController extends StateNotifier<AuthState> {
  AuthController(this._repo, this._store) : super(AuthState.initial) {
    _bootstrap();
  }

  final AuthRepository _repo;
  final SecureStore _store;

  /*
   * ══════════════════════════════════════════════════════════════════════
   *  ⚠⚠ **يُحسَم دائماً — ولو أخفق كلُّ شيء.**
   * ══════════════════════════════════════════════════════════════════════
   *
   * الراوتر يحبس المستخدم في `/splash` ما دامت الحالة `unknown`. فأيُّ
   * استثناءٍ هنا **يُجمّد التطبيق على شاشة البداية إلى الأبد** — لا شاشةَ
   * دخول ولا رسالةَ خطأ ولا مخرج.
   *
   * وقد وقع فعلاً على جهازٍ حقيقيّ (بلاغُ المالك بصورةٍ، 9 سبتمبر 2026):
   * التطبيق واقفٌ على الشعار و«وكالتك في جيبك» ولا يتقدّم.
   *
   * ⚠ والتخزينُ الآمن **يُخفق فعلاً**: مفتاحُ Android Keystore يضيع مع
   * إعادة التثبيت بينما يبقى ملفُّ التفضيلات المشفَّر (نسخُ النظام
   * الاحتياطيّ)، فتُخفق فكُّ الشفرة وتُرمى من `read` نفسِها.
   *
   * ولذلك ثلاثُ طبقات:
   *   ١) كلُّ قراءةٍ محروسةٌ على حدة — فشلُ إحداها لا يُسقط الأخرى.
   *   ٢) حارسٌ يلفّ الكلّ — استثناءٌ غيرُ متوقَّع ينتهي بـ«خارج».
   *   ٣) مهلةٌ قصوى — قراءةٌ **تتعلّق** ولا ترمي شيئاً لا تُبقيه معلّقاً.
   *
   * و«خارج» هو الفشلُ الصحيح: يرى شاشةَ الدخول ويُعيد التحقّق. أمّا
   * «غير معروف» فليس حالةً يعيش فيها المستخدم — هي لحظةُ قراءةٍ لا أكثر.
   */
  Future<void> _bootstrap() async {
    var onboarded = false;
    AgentUser? user;

    try {
      await Future(() async {
        try {
          onboarded = await _store.readOnboarded();
        } catch (_) {
          // ⚠ تعذّرت قراءةُ «هل مرّ بالتعريف؟» ⇦ يُعرض التعريف. مزعجٌ
          // مرّةً، وليس تجميداً.
        }

        try {
          user = await _repo.restore();
        } catch (_) {
          // رمزٌ لا يُقرأ = لا جلسة. يدخل من جديد.
        }
      }).timeout(const Duration(seconds: 8));
    } catch (_) {
      // ⚠ المهلة: قراءةٌ معلّقة لا تُبقي التطبيق على الشعار.
    }

    if (!mounted) return;
    state = AuthState(
      status: user == null ? AuthStatus.signedOut : AuthStatus.signedIn,
      user: user,
      onboarded: onboarded,
    );
  }

  Future<void> markOnboarded() async {
    await _store.setOnboarded();
    if (!mounted) return;
    state = state.copyWith(onboarded: true);
  }

  void adopt(AuthSession session) {
    state = AuthState(
      status: AuthStatus.signedIn,
      user: session.user,
      onboarded: true,
    );
  }

  Future<void> signOut() async {
    await _repo.signOut();
    if (!mounted) return;
    state = AuthState(status: AuthStatus.signedOut, onboarded: state.onboarded);
  }

  /// حذفُ الحساب من داخل التطبيق (شرطٌ في المتجرين لتطبيقٍ يُنشئ حساباً).
  /// يرمي [ApiFailure] عند فشل الخادم لتعرضه الشاشة؛ وعند النجاح يُخرج
  /// الوكيلَ كأيّ تسجيل خروج.
  Future<void> deleteAccount(String code) async {
    await _repo.deleteAccount(code);
    if (!mounted) return;
    state = AuthState(status: AuthStatus.signedOut, onboarded: state.onboarded);
  }

  /// إرسالُ رمز واتساب إلى هاتف الوكيل نفسِه قبل الحذف.
  Future<void> requestDeleteOtp() async {
    final phone = state.user?.phone;
    if (phone == null || phone.isEmpty) {
      throw ApiFailure('تعذّر معرفة رقم هاتفك. أعد تسجيل الدخول.');
    }
    await _repo.requestOtp(phone);
  }
}

final authControllerProvider =
    StateNotifierProvider<AuthController, AuthState>((ref) {
  final controller = AuthController(
    ref.watch(authRepositoryProvider),
    ref.watch(secureStoreProvider),
  );

  // جلسة منتهية (401) ⇒ خروج فوري. بدون هذا يبقى الرمز الميت في التخزين،
  // ويظل الراوتر يعدّ الوكيل داخل التطبيق فيرى خطأً على كل شاشة بلا مخرج.
  // الوصل هنا لا في core/net: الاتجاه features ← core، والراوتر يراقب هذا
  // المزوّد دائماً فلا يمكن أن يُنسى الوصل.
  ref.watch(apiClientProvider).onUnauthorized = () {
    controller.signOut();
  };

  return controller;
});
