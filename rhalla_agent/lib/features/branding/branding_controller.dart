import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme/tokens.dart';
import '../auth/auth_controller.dart';
import 'branding_repository.dart';

/// حالة هوية الشركة في الجلسة.
class BrandingState {
  const BrandingState({
    required this.branding,
    this.loading = false,
    this.applied = false,
    this.settled = false,
    this.epoch = 0,
  });

  final Branding branding;
  final bool loading;

  /// عدّاد يرتفع مع كل إسناد ألوان.
  ///
  /// يراقبه الراوتر فيُعاد بناء الشجرة كاملةً عند كل تغيّر هوية — وهو الطريق
  /// الوحيد لإعادة تلوين فروع الهيكل، لأنها محفوظة بـ `GlobalKey` فلا تُعاد
  /// بناءً بإعادة بناء ما فوقها. وبه أيضاً تُستدرَك هويةٌ وصلت متأخّرة بعد
  /// انتهاء المهلة، بدل أن تبقى الجلسة كلّها بألوان خاطئة.
  final int epoch;

  /// انتهت محاولة الجلب — بنجاحٍ أو بفشل.
  ///
  /// يقف عليها الراوتر: لا تُبنى شاشةٌ من شاشات ما بعد الدخول قبل أن تستقرّ
  /// الهوية. والسبب ليس أناقة — بل أن `go_router` يحفظ فروع الهيكل بـ
  /// `GlobalKey`، فالعناصر المبنيّة تُنقل ولا تُعاد بناءً. أي شاشةٍ بُنيت
  /// بألوان الرحالة قبل وصول ألوان الشركة تبقى بها إلى نهاية الجلسة —
  /// وهذا ما حدث فعلاً ببطاقة الإجراءات في الشاشة الرئيسية.
  final bool settled;

  /// هل أُسندت ألوان الشركة إلى [R] فعلاً؟
  ///
  /// تُقرأ في `main.dart` مفتاحاً لإعادة بناء الشجرة: `R` قيمٌ ساكنة، وتغييرها
  /// وحده لا يُخطر فلاتر بشيء. بغير هذا المفتاح تبقى الشاشة بلون الرحالة حتى
  /// أول تنقّل.
  final bool applied;

  BrandingState copyWith({
    Branding? branding,
    bool? loading,
    bool? applied,
    bool? settled,
    int? epoch,
  }) =>
      BrandingState(
        branding: branding ?? this.branding,
        loading: loading ?? this.loading,
        applied: applied ?? this.applied,
        settled: settled ?? this.settled,
        epoch: epoch ?? this.epoch,
      );
}

/// جالب الهوية ومُسنِدها.
///
/// ⚠ الإسناد إلى [R] عالميّ في الجلسة (انظر التعليق في `tokens.dart`)، ولذلك
/// **الخروج يجب أن يُعيد الهوية الرسمية**: شاشة الدخول هوية «شركة الرحالة»
/// بقرار المالك، فظهورها بلون الشركة السابقة خطأ ظاهر.
class BrandingController extends StateNotifier<BrandingState> {
  BrandingController(this._repo)
      : super(BrandingState(branding: Branding.fallback));

  final BrandingRepository _repo;

  int _epoch = 0;

  /// مهلة الانتظار قبل الدخول بهوية الرحالة الافتراضية.
  ///
  /// الراوتر يقف على [BrandingState.settled]، فلو تعلّق الطلب على مهلة
  /// الشبكة الكاملة (20 ثانية) بقي الوكيل أمام شاشة البداية.
  ///
  /// ثمانٍ لا ثلاث: المهلة الأولى كانت ثلاث ثوانٍ، ولحظةُ بطءٍ واحدة عند
  /// الإقلاع أسقطت هوية الشركة وأدخلت التطبيق بالأخضر. وثوانٍ إضافية أمام
  /// شاشة البداية أهون من جلسةٍ كاملة بألوان شركةٍ أخرى.
  ///
  /// وما بعد المهلة يدخل بالافتراضية ولا يبقى عالقاً — وإن وصلت الهوية
  /// متأخّرة رفعت [BrandingState.epoch] فأُعيد بناء الشجرة بألوانها.
  static const _gate = Duration(seconds: 8);

  Future<void> load() async {
    if (state.loading) return;
    state = state.copyWith(loading: true);

    // المهلة تفتح الباب ولا تُلغي الطلب.
    //
    // `timeout()` كانت ستُهمل الردّ الواصل بعدها، فتبقى الجلسة كلّها بألوان
    // خاطئة بسبب تأخّرٍ لثانية. هنا يتابع الطلب طريقه، وإن وصل متأخّراً
    // طُبّق ورفع `epoch` فأُعيد بناء الشجرة بألوانه.
    Future<void>.delayed(_gate, () {
      if (mounted && !state.settled) {
        state = state.copyWith(settled: true);
      }
    });

    try {
      final b = await _repo.load();
      _apply(b);
    } catch (_) {
      // فشل جلب الهوية لا يمنع دخول التطبيق: يُعرض بهوية الرحالة الرسمية.
      // ألوانٌ افتراضية أهون بكثير من شاشة خطأ تمنع الوكيل من عمله.
      if (!mounted) return;
      state = state.copyWith(loading: false, settled: true);
    }
  }

  /// بعد حفظٍ من شاشة الإعدادات — الخادم أعاد الهوية الجديدة كاملةً.
  void adopt(Branding b) => _apply(b);

  void _apply(Branding b) {
    R.applyBrand(
      primaryColor: b.colors.primary,
      secondaryColor: b.colors.secondary,
      backgroundColor: b.colors.background,
      textColor: b.colors.text,
      onPrimaryColor: b.colors.onPrimary,
    );
    _chrome(b.colors.background);
    _epoch++;
    if (!mounted) return;
    state = BrandingState(
      branding: b,
      loading: false,
      applied: true,
      settled: true,
      epoch: _epoch,
    );
  }

  /// شريط تنقّل النظام يتبع خلفية التطبيق.
  ///
  /// تُضبط في `main.dart` مرّة عند الإقلاع بلون الرحالة؛ بدون هذه الاستدعاءة
  /// يبقى شريطاً أخضرَ أسفل تطبيقٍ أزرق. وسطوع الأيقونات يُشتقّ من اللون
  /// نفسه لا يُفترض: ثيمٌ داكن بأيقونات داكنة يجعل الشريط فارغاً في العين.
  void _chrome(Color background) {
    SystemChrome.setSystemUIOverlayStyle(SystemUiOverlayStyle(
      statusBarColor: Colors.transparent,
      statusBarIconBrightness:
          background.computeLuminance() > .5 ? Brightness.dark : Brightness.light,
      statusBarBrightness:
          background.computeLuminance() > .5 ? Brightness.light : Brightness.dark,
      systemNavigationBarColor: background,
      systemNavigationBarIconBrightness:
          background.computeLuminance() > .5 ? Brightness.dark : Brightness.light,
    ));
  }

  /// عودة إلى هوية «شركة الرحالة» — تُستدعى عند الخروج.
  void reset() {
    R.resetBrand();
    _chrome(R.bgBottom);
    _epoch++;
    if (!mounted) return;
    state = BrandingState(
      branding: Branding.fallback,
      applied: false,
      epoch: _epoch,
    );
  }
}

final brandingControllerProvider =
    StateNotifierProvider<BrandingController, BrandingState>((ref) {
  final controller = BrandingController(ref.watch(brandingRepositoryProvider));

  // الخروج يمحو الهوية فوراً — شاشة الدخول هوية «الرحالة» بقرار المالك.
  //
  // الربط هنا لا في كل شاشة، لأن شاشةً واحدة تُنسى تكفي لتظهر ألوان شركةٍ
  // في شاشة الدخول.
  ref.listen<AuthState>(authControllerProvider, (prev, next) {
    if (next.status == AuthStatus.signedOut &&
        prev?.status == AuthStatus.signedIn) {
      controller.reset();
      // الدخول التالي يجب أن يجلب من جديد، لا أن يأخذ نتيجة محفوظة.
      ref.invalidate(brandingBootstrapProvider);
    }
  });

  return controller;
});

/// جلب الهوية بمجرّد أن تصير الجلسة داخلة.
///
/// يُراقَب من `main.dart` — أي دائماً — لا من الهيكل: الراوتر صار يحجز
/// شاشات ما بعد الدخول حتى تستقرّ الهوية، فلو كان الجالب داخل الهيكل لانتظر
/// كلٌّ منهما الآخر ولما فتح التطبيق أبداً.
///
/// وهو يحرس نفسه بحالة الجلسة: طلبٌ قبل استعادة الرمز يرجع 401، و401 تُخرج
/// الوكيل من التطبيق — فيصير جلبُ لونٍ سبباً في تسجيل خروج.
final AutoDisposeFutureProvider<void> brandingBootstrapProvider =
    FutureProvider.autoDispose<void>((ref) async {
  ref.keepAlive();

  // `watch` لا `read`: تغيّر حالة الجلسة يُعيد تشغيل هذا المزوّد، فيُجلب
  // عند الدخول ولو أُنشئ قبله.
  if (ref.watch(authControllerProvider).status != AuthStatus.signedIn) return;

  // ⚠ التأجيل ضروري لا تجميل. Riverpod يمنع مزوّداً من تعديل مزوّدٍ آخر
  // أثناء تهيئته:
  //
  //     Providers are not allowed to modify other providers during
  //     their initialization.
  //
  // و`load()` أوّل ما تفعله هو ضبط `loading = true` على مزوّد الهوية، فتقع
  // في هذا المنع. والاستثناء **لا يظهر على الشاشة** — يُبتلع في مستقبلٍ غير
  // مراقَب — فيبدو الأمر كأن الطلب لم يُرسَل قطّ، وهو ما كلّف تشخيصاً طويلاً.
  await Future<void>.delayed(Duration.zero);

  await ref.read(brandingControllerProvider.notifier).load();
});
