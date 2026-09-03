import 'package:flutter/widgets.dart';

/// رموز نظام التصميم — مستخرجة من مشروع Claude Design.
/// المرجع: docs/design-system.md — لا تخترع قيمة ليست هنا.
class R {
  R._();

  // ─── العلامة ───────────────────────────────────────────────
  //
  // هذه القيم **تتغيّر** بهوية الشركة بعد الدخول (انظر `applyBrand`)، ولذلك
  // هي `static` قابلة للإسناد لا `static const`.
  //
  // ولمَ إسنادٌ عامّ لا موفّرٌ يُقرأ في كل شاشة؟ لأن كل شاشة في التطبيق تقرأ
  // `R.primary` مباشرةً، فتحويلها إلى موفّر يعني تعديل عشرات الملفات وفرصةً
  // لنسيان واحدة فتبقى بلون الرحالة داخل تطبيق شركة أخرى. والجلسة الواحدة
  // لا تحمل إلا شركةً واحدة، فلا تعارض.
  //
  // ⚠ ولذلك `resetBrand()` **واجبة** عند الخروج: بغيرها يظهر لون الشركة
  // السابقة في شاشة الدخول، وشاشة الدخول هوية «الرحالة» الرسمية بقرار المالك.
  static Color primary = _dPrimary;
  static Color primaryGradStart = _dGradStart;
  static Color primaryGradEnd = _dGradEnd;
  static Color primaryDark = _dPrimaryDark;
  static Color credit = _dCredit; // مبلغ وارد
  static Color ink = _dInk; // onSurface

  // ─── الخلفيات ──────────────────────────────────────────────
  static Color bgTop = _dBgTop;
  static Color bgBottom = _dBgBottom;
  static Color scrimBottom = _dScrim;

  // ─── هوية الرحالة الافتراضية ───────────────────────────────
  // تُحفظ منفصلةً لأن `resetBrand` تحتاج الأصل بعد أن دهسته هوية شركة.
  static const _dPrimary = Color(0xFF00B17A);
  static const _dGradStart = Color(0xFF00C489);
  static const _dGradEnd = Color(0xFF00875E);
  static const _dPrimaryDark = Color(0xFF00603F);
  static const _dCredit = Color(0xFF00A570);
  static const _dInk = Color(0xFF0A261E);
  static const _dBgTop = Color(0xFFF3FAF7);
  static const _dBgBottom = Color(0xFFEAF4F0);
  static const _dScrim = Color(0xFFF1F8F5);

  /// اللون المقروء فوق اللون الأساسي — يحسبه الخادم ويُسنده التطبيق.
  ///
  /// أبيض في كل ثيمات الرحالة، لكن شركةً بلونٍ فاتح تحتاج نصّاً داكناً وإلا
  /// اختفت كتابة كل زرّ رئيسي في التطبيق.
  static Color onPrimary = const Color(0xFFFFFFFF);

  /// هل الجلسة تعرض هوية شركة أم هوية الرحالة الرسمية؟
  static bool get isBranded => primary != _dPrimary;

  /// إسناد ألوان الشركة. تُستدعى مرّة عند الدخول ومرّة عند تغيير الهوية.
  ///
  /// التدرّجان يُشتقّان من اللون الأساسي والثانوي لا يُطلبان منفصلين: طلبُ
  /// أربعة ألوان من مدير فرعٍ ينتهي بتدرّجٍ لا ينسجم، واشتقاقُهما يُبقي
  /// شكل التطبيق كما صُمّم مهما كان لون الشركة.
  static void applyBrand({
    required Color primaryColor,
    required Color secondaryColor,
    required Color backgroundColor,
    required Color textColor,
    required Color onPrimaryColor,
  }) {
    primary = secondaryColor;
    primaryGradStart = secondaryColor;
    primaryGradEnd = primaryColor;
    primaryDark = primaryColor;
    credit = primaryColor;
    ink = textColor;
    onPrimary = onPrimaryColor;
    bgTop = backgroundColor;
    bgBottom = backgroundColor;
    scrimBottom = backgroundColor;
  }

  /// العودة إلى هوية «شركة الرحالة» — واجبة عند الخروج.
  static void resetBrand() {
    primary = _dPrimary;
    primaryGradStart = _dGradStart;
    primaryGradEnd = _dGradEnd;
    primaryDark = _dPrimaryDark;
    credit = _dCredit;
    ink = _dInk;
    onPrimary = const Color(0xFFFFFFFF);
    bgTop = _dBgTop;
    bgBottom = _dBgBottom;
    scrimBottom = _dScrim;
  }

  // ─── الخطأ والتنبيه ────────────────────────────────────────
  static const error = Color(0xFFC43B2E);
  static const errorText = Color(0xFFA82E23);
  static const warnBg = Color(0x33FFD678); // rgba(255,214,120,.2)
  static const warnBorder = Color(0x80FFD678);
  static const warnInk = Color(0xFF6E5408);
  static const warnIcon = Color(0xFF8A6A0B);

  /// نجمة المفضّلة — أصفر ذهبيّ بطلب المالك. أدكن من عائلة التنبيه
  /// (FFD678) لأن الأخير باهت على خلفية البطاقات الفاتحة.
  static const star = Color(0xFFFFC107);

  // ─── علم ليبيا ─────────────────────────────────────────────
  static const flagRed = Color(0xFFE70013);
  static const flagBlack = Color(0xFF000000);
  static const flagGreen = Color(0xFF239E46);

  // ─── سلّم الحبر ────────────────────────────────────────────
  static Color inkA(double a) => ink.withValues(alpha: a);
  static Color whiteA(double a) => const Color(0xFFFFFFFF).withValues(alpha: a);
  /// إيحاء حركة الحساب: الوارد أخضر والصادر أحمر.
  static Color creditA(double a) => credit.withValues(alpha: a);
  static Color debitA(double a) => error.withValues(alpha: a);

  static Color primaryA(double a) => primary.withValues(alpha: a);

  // ─── التدرجات ──────────────────────────────────────────────
  // التدرّجات صارت `get` لا `const` لأن ألوانها تتغيّر بهوية الشركة.
  static LinearGradient get primaryGradient => LinearGradient(
        begin: const Alignment(0.6, -1),
        end: const Alignment(-0.6, 1),
        colors: [primaryGradStart, primaryGradEnd],
      );

  /// تدرّج الترويسة ثلاثيّ الوقفات، ووقفته الوسطى تُحسب مزجاً بين الطرفين
  /// لا لوناً ثابتاً — لونٌ أخضر ثابت في المنتصف يشوّه ترويسة شركةٍ زرقاء.
  static LinearGradient get headerGradient => LinearGradient(
        begin: const Alignment(0.5, -1),
        end: const Alignment(-0.5, 1),
        // بألوان الرحالة يعطي هذا 00C489 → 008D60 → 00603F، وهو عملياً
        // التدرّج الأصلي (00C489 → 008A5D → 006B47) — فلم يتغيّر شكل الترويسة.
        colors: [
          primaryGradStart,
          Color.lerp(primaryGradStart, primaryDark, .55)!,
          primaryDark,
        ],
        stops: const [0.0, 0.55, 1.0],
      );

  static LinearGradient glassGradient({double from = .82, double to = .55}) =>
      LinearGradient(
        begin: Alignment.topRight,
        end: Alignment.bottomLeft,
        colors: [whiteA(from), whiteA(to)],
      );

  static LinearGradient get screenBackground => LinearGradient(
        begin: Alignment.topCenter,
        end: Alignment.bottomCenter,
        colors: [bgTop, bgBottom],
      );

  // ─── نصف القطر ─────────────────────────────────────────────
  static const rPill = 99.0;
  static const rCardXl = 26.0;
  static const rCard = 22.0;
  static const rRow = 20.0;
  static const rActions = 24.0;
  static const rNav = 28.0;
  static const rOtp = 18.0;
  static const rKey = 17.0;
  static const rTile = 13.0;
  static const rHeaderBottom = 38.0;

  // ─── الظلال ────────────────────────────────────────────────
  static List<BoxShadow> get shCta => [
        BoxShadow(color: primaryA(.32), blurRadius: 32, offset: const Offset(0, 16)),
      ];
  static List<BoxShadow> get shPill => [
        BoxShadow(color: primaryA(.34), blurRadius: 30, offset: const Offset(0, 14)),
      ];
  static List<BoxShadow> get shCardL => [
        BoxShadow(color: const Color(0xFF032D21).withValues(alpha: .12), blurRadius: 44, offset: const Offset(0, 20)),
      ];
  static List<BoxShadow> get shCardM => [
        BoxShadow(color: const Color(0xFF032D21).withValues(alpha: .10), blurRadius: 40, offset: const Offset(0, 18)),
      ];
  static List<BoxShadow> get shCardS => [
        BoxShadow(color: const Color(0xFF032D21).withValues(alpha: .06), blurRadius: 26, offset: const Offset(0, 10)),
      ];
  static List<BoxShadow> get shRow => [
        BoxShadow(color: const Color(0xFF032D21).withValues(alpha: .05), blurRadius: 16, offset: const Offset(0, 6)),
      ];
  static List<BoxShadow> get shNav => [
        BoxShadow(color: const Color(0xFF032D21).withValues(alpha: .12), blurRadius: 36, offset: const Offset(0, 16)),
      ];

  // ─── الزجاج ────────────────────────────────────────────────
  /// سيغما Flutter ≈ نصف قيمة blur في CSS.
  static const blurGlass = 9.0; // CSS blur(18px)
  static const blurGlassXl = 11.0; // CSS blur(22px)
  static const blurRow = 8.0; // CSS blur(16px)
  static const blurNav = 12.0; // CSS blur(24px)
  static const blurChip = 7.0; // CSS blur(14px)

  // ─── المسافات ──────────────────────────────────────────────
  static const gapRow = 9.0;
  static const gapCard = 12.0;
  static const padScreen = 20.0;
}
