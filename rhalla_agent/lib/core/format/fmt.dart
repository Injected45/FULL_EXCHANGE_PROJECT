import 'package:intl/intl.dart';

/// التنسيق.
///
/// قرار المالك: **كل الأرقام غربية** — لا عربية-هندية في أي سياق.
/// لذلك نستعمل لوكال 'en' لا 'ar_LY'، لأن الأخير يُخرج ٠١٢٣.
class Fmt {
  Fmt._();

  static final _money3 = NumberFormat('#,##0.000', 'en');
  static final _int = NumberFormat('#,##0', 'en');
  static final _rate4 = NumberFormat('#,##0.0###', 'en');

  /// الدينار الليبي بثلاث خانات عشرية — 1 دينار = 1000 درهم.
  /// خانتان خطأ في هذه العملة.
  static String money(num? v) => _money3.format(v ?? 0);

  static String moneyWithSign(num v, {bool credit = false}) {
    final sign = credit ? '+ ' : '− ';
    return '$sign${_money3.format(v.abs())}';
  }

  static String count(num? v) => _int.format(v ?? 0);

  /// سعر الصرف — أربع خانات، فالفارق بين 5.7700 و5.8000 يغيّر ما يستلمه
  /// المستفيد. money() بثلاث خانات تكفي للمبالغ لا للأسعار.
  static String rate(num? v) => _rate4.format(v ?? 0);

  /// الخادم يعيد الأرقام أحياناً نصاً وأحياناً رقماً — حسب أي مسار
  /// أعادها (Eloquent يحوّل، أما DB::select الخام فلا).
  static double num_(dynamic v) {
    if (v == null) return 0;
    if (v is num) return v.toDouble();
    return double.tryParse(v.toString().replaceAll(',', '')) ?? 0;
  }

  /// عرض رقم الهاتف: 924458817 → «92 445 8817»
  static String phone(String digits) {
    final d = digits.replaceAll(RegExp(r'\D'), '');
    if (d.length < 9) return d;
    return '${d.substring(0, 2)} ${d.substring(2, 5)} ${d.substring(5, 9)}';
  }

  /// للإرسال إلى الخادم: 9 خانات تبدأ بـ 9، **بلا بادئة 218**.
  /// النقاط المختلفة تتوقّع صيغاً مختلفة، فنوحّد عند حدود التطبيق.
  static String phoneForApi(String input) {
    var d = input.replaceAll(RegExp(r'\D'), '');
    if (d.startsWith('218')) d = d.substring(3);
    if (d.startsWith('00218')) d = d.substring(5);
    if (d.startsWith('0')) d = d.substring(1);
    return d;
  }

  static bool isValidLibyanPhone(String input) {
    final d = phoneForApi(input);
    return d.length == 9 && d.startsWith('9');
  }
}
