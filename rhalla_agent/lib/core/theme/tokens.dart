import 'package:flutter/widgets.dart';

/// رموز نظام التصميم — مستخرجة من مشروع Claude Design.
/// المرجع: docs/design-system.md — لا تخترع قيمة ليست هنا.
class R {
  R._();

  // ─── العلامة ───────────────────────────────────────────────
  static const primary = Color(0xFF00B17A);
  static const primaryGradStart = Color(0xFF00C489);
  static const primaryGradEnd = Color(0xFF00875E);
  static const primaryDark = Color(0xFF00603F);
  static const credit = Color(0xFF00A570); // مبلغ وارد
  static const ink = Color(0xFF0A261E); // onSurface

  // ─── الخلفيات ──────────────────────────────────────────────
  static const bgTop = Color(0xFFF3FAF7);
  static const bgBottom = Color(0xFFEAF4F0);
  static const scrimBottom = Color(0xFFF1F8F5);

  // ─── الخطأ والتنبيه ────────────────────────────────────────
  static const error = Color(0xFFC43B2E);
  static const errorText = Color(0xFFA82E23);
  static const warnBg = Color(0x33FFD678); // rgba(255,214,120,.2)
  static const warnBorder = Color(0x80FFD678);
  static const warnInk = Color(0xFF6E5408);
  static const warnIcon = Color(0xFF8A6A0B);

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
  static const primaryGradient = LinearGradient(
    begin: Alignment(0.6, -1),
    end: Alignment(-0.6, 1),
    colors: [primaryGradStart, primaryGradEnd],
  );

  static const headerGradient = LinearGradient(
    begin: Alignment(0.5, -1),
    end: Alignment(-0.5, 1),
    colors: [primaryGradStart, Color(0xFF008A5D), Color(0xFF006B47)],
    stops: [0.0, 0.55, 1.0],
  );

  static LinearGradient glassGradient({double from = .82, double to = .55}) =>
      LinearGradient(
        begin: Alignment.topRight,
        end: Alignment.bottomLeft,
        colors: [whiteA(from), whiteA(to)],
      );

  static const screenBackground = LinearGradient(
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
