import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:local_auth/local_auth.dart';

import '../../core/net/api_client.dart' show secureStoreProvider;
import '../../core/storage/secure_store.dart';

/* ══════════════════════════════════════════════════════════════════════════
   قفلُ التطبيق بعد الخمول — Local Security Lock
   ══════════════════════════════════════════════════════════════════════════

   يقفل الواجهةَ إذا غاب التطبيق في الخلفية أكثر من خمس دقائق، ويُفتح
   بالبصمة أو بوجه المستخدم — وإلّا فبإعادة الدخول.

   ── ⚠ قفلٌ لا خروج ───────────────────────────────────────────────────────

   الجلسةُ تبقى كما هي: هذا سترٌ للواجهة لا إنهاءٌ للمصادقة. والفرقُ عمليّ —
   وكيلٌ يضع هاتفه دقائق ثم يعود لا يُطلب منه رمزُ تحقّقٍ جديد عبر واتساب،
   بل لمسةُ إصبع. أمّا انتهاءُ الجلسة فعلاً في الخادم فيسلك مسارَ الدخول
   الكامل كما كان — ولا يفتحه هذا القفل.

   ── ⚠ ولا يُقاس الخمولُ بمؤقّتٍ في الذاكرة ───────────────────────────────

   المؤقّتُ يتوقّف حين يوقف النظامُ التطبيق، فيعود المستخدم بعد ساعةٍ ولم
   يمرّ من عمر المؤقّت إلّا ثوانٍ. فيُسجَّل **وقتُ المغادرة** في التخزين
   الآمن ويُقاس الفارقُ عند العودة.

   ولذلك لا يُحتال عليه بإغلاق التطبيق: الوقتُ محفوظٌ لا في الذاكرة.

   ── ⚠ ولا تُخزَّن بياناتٌ حيويّة — أبداً ──────────────────────────────────

   `local_auth` واجهةٌ فوق `BiometricPrompt` و`LocalAuthentication`: يُسأل
   النظامُ فيُعيد نجاحاً أو فشلاً. لا بصمةَ ولا وجهَ يمرّ بالتطبيق ولا يُحفظ
   منه شيء — وهو شرطُ المتجرين معاً.
   ══════════════════════════════════════════════════════════════════════════ */

/// مدّةُ الخمول التي تُقفل بعدها الواجهة — أمرُ المالك (9 سبتمبر 2026).
const kIdleLock = Duration(minutes: 5);

/// حالةُ القفل.
enum LockPhase {
  /// لم يُقرأ بعد حالُ القفل.
  ///
  /// ⚠ **ولا تُستر بها الشاشة** — انظر [AppLockState.shouldHide].
  unknown,

  /// مفتوح.
  open,

  /// مقفلٌ ينتظر بصمةً أو بديلَها.
  locked,
}

/// ⚠ **`AppLockState` لا `LockState`**: فلاتر نفسُها تُصدّر `LockState`
/// من `shortcuts.dart`، فالاسمُ القصير يجعل كلَّ ملفٍّ يستورد `material`
/// و`app_lock` معاً يُخفق بغموضٍ لا علاقة له بالقفل.
@immutable
class AppLockState {
  const AppLockState({
    this.phase = LockPhase.unknown,
    this.biometricsAvailable = false,
    this.enabled = false,
    this.lastError,
  });

  final LockPhase phase;

  /// هل يملك الجهاز بصمةً أو وجهاً مُسجَّلاً فعلاً؟
  final bool biometricsAvailable;

  /// هل فعّل المستخدم الدخولَ السريع؟
  ///
  /// ⚠ **والقفلُ يعمل بلا هذا**: هو يخصّ **طريقةَ** الفتح لا وجودَه. القفلُ
  /// سياسةٌ أمنيّة، وجعلُه اختيارياً يجعل جهازاً منسيّاً على طاولةٍ مفتوحاً.
  final bool enabled;

  /// آخرُ سببِ فشلٍ يُعرض للمستخدم — لا رسالةُ استثناء.
  final String? lastError;

  bool get isLocked => phase == LockPhase.locked;

  /*
   * ⚠⚠ **يُستر المقفولُ وحدَه — لا المجهول.**
   *
   * كان يستر `unknown` أيضاً، منعاً لومضةِ أرصدةٍ في أوّل إطار. وكان
   * ذلك **عطلاً قاتلاً**: أخفقت قراءةُ التخزين الآمن على جهازٍ حقيقيّ
   * فماتت دالّةُ الإقلاع، فبقيت الحالةُ `unknown` — **فتجمّد التطبيق
   * على شاشةٍ ساترة ولم تظهر شاشةُ الدخول أبداً**. (بلاغُ المالك،
   * 9 سبتمبر 2026، على جهازٍ حقيقيّ.)
   *
   * والقاعدةُ التي تمنع تكرارَه: **القفلُ يفشل مفتوحاً لا مغلقاً**.
   * قفلٌ لا يعرف حالَه يجب أن يُفسح الطريق؛ فهو سترُ راحةٍ لا حارسُ
   * مصادقة — والمصادقةُ الحقيقية في الخادم وفي الجلسة، وهي قائمةٌ
   * سواءٌ استُر الشاشةُ أم لا.
   *
   * ⚠ ولا تضيع الومضةُ التي خِيف منها: الإقلاعُ **لا يقفل أصلاً**
   * (يمحو طابعَ المغادرة)، فلا شيءَ يُستر في أوّل إطار. والسترُ عند
   * العودة من الخلفية يقع مع `inactive` في `LockGate` قبل أن يلتقط
   * النظامُ معاينتَه.
   */
  bool get shouldHide => phase == LockPhase.locked;

  AppLockState copyWith({
    LockPhase? phase,
    bool? biometricsAvailable,
    bool? enabled,
    String? lastError,
    bool clearError = false,
  }) =>
      AppLockState(
        phase: phase ?? this.phase,
        biometricsAvailable: biometricsAvailable ?? this.biometricsAvailable,
        enabled: enabled ?? this.enabled,
        lastError: clearError ? null : (lastError ?? this.lastError),
      );
}

class AppLockController extends StateNotifier<AppLockState>
    with WidgetsBindingObserver {
  AppLockController(this._store, this._auth) : super(const AppLockState()) {
    WidgetsBinding.instance.addObserver(this);
    _boot();
  }

  final SecureStore _store;
  final LocalAuthentication _auth;

  /// وضعُ الحماية المختار: `biometric` · `device` · `none`. يُقرأ عند الإقلاع
  /// ويُحدَّث من الإعدادات. `none` يُعطّل القفلَ التلقائيّ، و`device` يسمح
  /// بنمط/رقم الجهاز بديلاً عن البصمة.
  String _mode = 'biometric';
  String get mode => _mode;

  Future<void> setSecurityMode(String mode) async {
    _mode = mode;
    await _store.writeSecurityMode(mode);
    if (mounted) state = state.copyWith(enabled: mode == 'biometric');
  }

  /// حارسُ «مرّةً واحدة» على نداء البصمة.
  ///
  /// ⚠ `authenticate` يُخفق إن نُودي وهو جارٍ، ولمستان على الزرّ تُنتجان
  /// نداءين — فيُرفض الثاني ويُعرض فشلٌ لم يقع.
  bool _authenticating = false;

  /* ═══════════════════════ الإقلاع والدورة ═══════════════════════ */

  Future<void> _boot() async {
    /*
     * ⚠⚠ **كلُّ ما هنا محروسٌ، والنهايةُ مفتوحةٌ مهما جرى.**
     *
     * `readBiometricUnlock` و`clearBackgroundedAt` تنفذان إلى التخزين
     * الآمن، وهو يُخفق على أجهزةٍ حقيقيّة (مفتاحٌ تالف، أو تخزينٌ لم
     * يُهيَّأ بعد على تثبيتٍ جديد). وكانتا بلا حارس، فماتت الدالّةُ
     * صامتةً وبقي التطبيق مستوراً إلى الأبد.
     *
     * والاستثناءُ هنا **لا يُعرض ولا يُسجَّل خطأً**: الإقلاعُ لا يقفل
     * أصلاً، فتعذُّرُ قراءةِ تفضيلٍ لا يعني للمستخدم شيئاً يفعله.
     */
    var available = false;
    var enabled = false;

    try {
      available = await _probeBiometrics();
      _mode = await _store.readSecurityMode();
      enabled = _mode == 'biometric';
    } catch (_) {
      // بلا بصمةٍ وبلا تفضيل — والقفلُ يبقى عاملاً بالوقت.
    }

    /*
     * ⚠ **الإقلاعُ ليس عودةً من الخلفية.**
     *
     * التطبيقُ يُقلع مفتوحاً: الراوتر هو من يقرّر أن يُدخله أو يُرسله إلى
     * شاشة الدخول. وقفلُه هنا يعني شاشةَ بصمةٍ فوق شاشةِ دخولٍ فارغة، فلا
     * يفهم المستخدم ما المطلوب منه.
     *
     * والطابعُ يُمحى عند الإقلاع حتى لا يُقفل بغيابٍ سبق إغلاق التطبيق.
     */
    try {
      await _store.clearBackgroundedAt();
    } catch (_) {
      /*
       * ⚠ وطابعٌ عالقٌ لا يُجمّد شيئاً: أسوأُ أثرِه قفلٌ عند العودة
       * الأولى من الخلفية، وله بصمةٌ أو خروجٌ يفتحه. أمّا موتُ الدالّة
       * هنا فكان يُجمّد التطبيق كلَّه.
       */
    }

    if (!mounted) return;
    state = state.copyWith(
      phase: LockPhase.open,
      biometricsAvailable: available,
      enabled: enabled,
    );
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    switch (state) {
      case AppLifecycleState.paused:
      case AppLifecycleState.detached:
      case AppLifecycleState.hidden:
        _onLeave();
      case AppLifecycleState.resumed:
        _onReturn();
      case AppLifecycleState.inactive:
        /*
         * ⚠ `inactive` **لا تُسجّل مغادرة**.
         *
         * تقع عند كل نافذةِ نظامٍ تعلو التطبيق: طلبُ إذن، ورقةُ مشاركة،
         * مكالمةٌ واردة، **ونافذةُ البصمة نفسُها**. وتسجيلُ المغادرة فيها
         * يجعل الفتحَ بالبصمة يُعيد القفلَ في اللحظة التي ينجح فيها.
         */
        break;
    }
  }

  Future<void> _onLeave() async {
    // ⚠ يُكتب مرّةً واحدة: `paused` ثم `detached` تقعان متتاليتين، والثانية
    // كانت ستؤخّر الطابع فتُقصّر مدّةَ الغياب المحسوبة.
    if (await _store.readBackgroundedAt() != null) return;
    await _store.writeBackgroundedAt(DateTime.now().toUtc());
  }

  Future<void> _onReturn() async {
    // ⚠ وقراءةٌ تُخفق تعني «لا قفل» لا «قفلٌ إلى الأبد» — يفشل مفتوحاً.
    DateTime? left;
    try {
      left = await _store.readBackgroundedAt();
    } catch (_) {
      return;
    }
    if (left == null) return;

    await _store.clearBackgroundedAt();
    final away = DateTime.now().toUtc().difference(left);

    if (away < kIdleLock) return;

    /*
     * ⚠ **ولا يُقفل ما لا جلسةَ خلفه.**
     *
     * وكيلٌ على شاشة الهاتف أو الرمز — أو موظفٌ على شاشة التفعيل — يترك
     * التطبيق ستّ دقائق ثم يعود، فيجد «التطبيق مقفل» فوق شاشةِ دخول.
     * فيضغط «فتح بالبصمة» ليصل إلى… شاشة الدخول نفسِها. ومن لا بصمةَ
     * لجهازه يجد مخرجَه الوحيد «الدخول بالتحقّق من جديد» — وهو أصلاً لم
     * يدخل.
     *
     * والقفلُ سترٌ لجلسةٍ قائمة؛ فحيث لا جلسة لا شيءَ يُستر.
     *
     * ⚠ ويُسأل التخزينُ لا حالةُ الشاشة: `readToken` تُعيد رمزَ الموظف أو
     * رمزَ الوكيل — أيَّهما كان فعّالاً — فيعمل الحارسُ للوضعين بلا أن
     * يستورد هذا الملفُّ شيئاً من `features`.
     */
    final token = await _store.readToken();
    if (token == null || token.isEmpty) return;

    // ⚠ «بلا حماية دخول»: المستخدم اختار ألّا يُقفَل جهازه. لا قفلَ إذاً —
    // وهذا اختيارُه لتأمين جهازه بنفسه (أمر المالك).
    if (_mode == 'none') return;

    /*
     * ⚠ ويُعاد سؤالُ النظام عن البصمة عند كل قفل لا عند الإقلاع وحدَه:
     * المستخدم قد يُسجّل بصمةً — أو يمحوها — والتطبيقُ في الخلفية.
     */
    final available = await _probeBiometrics();
    if (!mounted) return;

    state = state.copyWith(
      phase: LockPhase.locked,
      biometricsAvailable: available,
      clearError: true,
    );
  }

  /* ═══════════════════════ القفل والفتح ═══════════════════════ */

  /// قفلٌ فوريّ — لا ينتظر الخمس دقائق.
  ///
  /// ⚠ لحالاتٍ أمنيّة: تعطيلُ حساب، أو إلغاءُ جهاز، أو حدثٌ أمنيّ من الخادم.
  /// والسياسةُ القائمة تسري كما هي — هذا سترٌ إضافيّ لا بديلٌ عنها.
  Future<void> lockNow() async {
    if (state.phase == LockPhase.locked) return;

    // ⚠ الشرطُ نفسُه: لا قفلَ بلا جلسةٍ خلفه.
    final token = await _store.readToken();
    if (token == null || token.isEmpty) return;

    if (!mounted) return;
    state = state.copyWith(phase: LockPhase.locked, clearError: true);
  }

  /// يُفتح بعد نجاح المصادقة — يُنادى من شاشة القفل وحدَها.
  void markOpen() =>
      state = state.copyWith(phase: LockPhase.open, clearError: true);

  /// محاولةُ فتحٍ بالبصمة. تُعيد `true` عند النجاح.
  ///
  /// ⚠ **القرارُ للنظام لا للتطبيق**: `local_auth` تُعيد نجاحاً أو فشلاً من
  /// `BiometricPrompt`/`LocalAuthentication`، ولا يمرّ بالتطبيق شيءٌ حيويّ.
  Future<bool> unlockWithBiometrics() async {
    if (_authenticating) return false;
    _authenticating = true;

    try {
      final ok = await _auth.authenticate(
        localizedReason: 'أثبت هويتك لفتح التطبيق',
        options: AuthenticationOptions(
          // ⚠ `stickyAuth` حتى لا يُلغى الطلبُ إذا علت نافذةُ نظامٍ عليه.
          stickyAuth: true,
          // بصمةٌ فقط في وضع `biometric`؛ وفي وضع `device` نسمح بنمط/رقم
          // الجهاز بديلاً (اختيارُ المستخدم لتأمين جهازه).
          biometricOnly: _mode != 'device',
        ),
      );

      if (!mounted) return ok;

      if (ok) {
        state = state.copyWith(phase: LockPhase.open, clearError: true);
      } else {
        // إلغاءُ المستخدم ليس خطأً يُعرض — يبقى على شاشة القفل.
        state = state.copyWith(clearError: true);
      }
      return ok;
    } catch (e) {
      if (mounted) {
        state = state.copyWith(lastError: _reason(e));
      }
      return false;
    } finally {
      _authenticating = false;
    }
  }

  /// تفعيلُ الدخول السريع أو إلغاؤه — يخصّ طريقةَ الفتح لا وجودَ القفل.
  Future<void> setBiometricUnlock(bool on) async {
    await _store.writeBiometricUnlock(on);
    if (mounted) state = state.copyWith(enabled: on);
  }

  /* ═══════════════════════ أدواتٌ داخلية ═══════════════════════ */

  Future<bool> _probeBiometrics() async {
    try {
      /*
       * ⚠ **شرطان لا واحد**: جهازٌ يدعم العتاد، **و**بصمةٌ مُسجَّلة فعلاً.
       *
       * `isDeviceSupported` وحدَها تُعيد `true` على هاتفٍ فيه قارئٌ بلا بصمةٍ
       * مسجّلة — فيُعرض زرُّ بصمةٍ يُخفق دائماً، ولا يفهم المستخدم لماذا.
       */
      if (!await _auth.isDeviceSupported()) return false;
      if (!await _auth.canCheckBiometrics) return false;
      final kinds = await _auth.getAvailableBiometrics();
      return kinds.isNotEmpty;
    } catch (_) {
      // إضافةٌ غائبة أو منصّةٌ لا تجيب ⇦ لا بصمة. والبديلُ قائم.
      return false;
    }
  }

  /// سببٌ يفهمه المستخدم — لا اسمُ استثناء ولا أثرُ مكدّس.
  String _reason(Object e) {
    final s = e.toString();
    if (s.contains('NotAvailable')) {
      return 'المصادقة الحيوية غير متاحة على هذا الجهاز.';
    }
    if (s.contains('NotEnrolled')) {
      return 'لا توجد بصمة مسجَّلة في الجهاز — سجّلها من إعدادات الهاتف.';
    }
    if (s.contains('LockedOut') || s.contains('PermanentlyLockedOut')) {
      return 'أُقفلت المصادقة الحيوية بعد محاولات خاطئة — افتح الهاتف مرّة '
          'ثم أعد المحاولة.';
    }
    return 'تعذّرت المصادقة الحيوية.';
  }

  @override
  void dispose() {
    WidgetsBinding.instance.removeObserver(this);
    super.dispose();
  }
}

final localAuthProvider = Provider<LocalAuthentication>(
  (ref) => LocalAuthentication(),
);

final appLockProvider = StateNotifierProvider<AppLockController, AppLockState>(
  (ref) => AppLockController(
    ref.watch(secureStoreProvider),
    ref.watch(localAuthProvider),
  ),
);
