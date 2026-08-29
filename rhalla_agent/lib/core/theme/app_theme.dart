import 'package:flutter/material.dart';

import 'tokens.dart';

/// سلّم الخطوط.
///
/// القاعدة من نظام التصميم:
///   Kufi  = كل ما هو عرضي أو رقمي أو قابل للنقر
///   Plex  = كل ما هو شارح
///
/// الخطّان **مضمَّنان** في `assets/fonts` لا يُجلبان من الشبكة: الوكيل يعمل
/// في فروع قد تكون بلا اتصال، وخط يصل متأخراً يعني ومضة بخط بديل في كل شاشة.
class T {
  T._();

  static const kufiFamily = 'NotoKufiArabic';
  static const plexFamily = 'IBMPlexSansArabic';

  // ─── Noto Kufi Arabic ──────────────────────────────────────
  //
  // ملف **متغيّر** (fvar) بمحور wght واحد. `fontWeight` وحده لا يحرّك المحور
  // في الخطوط المتغيّرة — لذلك نمرّر `fontVariations` أيضاً، ونُبقي
  // `fontWeight` لأنه ما يستعمله المحرّك في القياس والتراجع.
  static TextStyle kufi(double size, FontWeight w,
          {Color? color, double? height, double? spacing}) =>
      TextStyle(
        fontFamily: kufiFamily,
        fontSize: size,
        fontWeight: w,
        fontVariations: [FontVariation('wght', w.value.toDouble())],
        color: color ?? R.ink,
        height: height,
        letterSpacing: spacing,
      );

  // ─── IBM Plex Sans Arabic ──────────────────────────────────
  //
  // ثلاث سماكات ثابتة مضمَّنة: 400 و500 و600. أي سماكة أخرى يقرّبها المحرّك
  // إلى أقربها — لا تُستعمل w700+ مع هذا الخط.
  static TextStyle plex(double size, FontWeight w,
          {Color? color, double? height, double? spacing}) =>
      TextStyle(
        fontFamily: plexFamily,
        fontSize: size,
        fontWeight: w,
        color: color ?? R.ink,
        height: height,
        letterSpacing: spacing,
      );

  // ─── أنماط مسمّاة ──────────────────────────────────────────
  static TextStyle get title => kufi(26, FontWeight.w700, height: 1.35);
  static TextStyle get titleSm => kufi(24, FontWeight.w700, height: 1.4);
  static TextStyle get appBarTitle => kufi(15, FontWeight.w600);
  static TextStyle get section => kufi(15, FontWeight.w600);
  static TextStyle get cta => kufi(15, FontWeight.w600, color: Colors.white);
  static TextStyle get amount => kufi(14, FontWeight.w700);
  static TextStyle get amountHero => kufi(40, FontWeight.w800);
  static TextStyle get amountFrac => kufi(22, FontWeight.w600, color: R.inkA(.42));

  static TextStyle get label => plex(11.5, FontWeight.w500, color: R.inkA(.55));
  static TextStyle get body => plex(13, FontWeight.w400, color: R.inkA(.58), height: 1.7);
  static TextStyle get name => plex(13.5, FontWeight.w600);
  static TextStyle get meta => plex(11, FontWeight.w400, color: R.inkA(.55));
  static TextStyle get value => plex(15, FontWeight.w600);
}

ThemeData buildTheme() {
  final base = ThemeData(
    useMaterial3: true,
    colorScheme: ColorScheme.fromSeed(
      seedColor: R.primary,
      primary: R.primary,
      surface: R.bgTop,
      error: R.error,
    ),
    scaffoldBackgroundColor: Colors.transparent,
    splashFactory: InkSparkle.splashFactory,
  );

  return base.copyWith(
    textTheme: base.textTheme.apply(
      bodyColor: R.ink,
      displayColor: R.ink,
      fontFamily: T.plexFamily,
    ),
    appBarTheme: const AppBarTheme(
      backgroundColor: Colors.transparent,
      elevation: 0,
      scrolledUnderElevation: 0,
    ),
  );
}
