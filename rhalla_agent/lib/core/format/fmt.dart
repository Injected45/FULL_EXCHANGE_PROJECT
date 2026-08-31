// الاثنتان معاً: services فيها TextInputFormatter، وwidgets فيها FocusNode
// (لـ MoneyFieldFocus). ولا تُصدِّر إحداهما الأخرى.
import 'package:flutter/services.dart';
import 'package:flutter/widgets.dart';
import 'package:intl/intl.dart';

/// التنسيق.
///
/// قرار المالك: **كل الأرقام غربية** — لا عربية-هندية في أي سياق.
/// لذلك نستعمل لوكال 'en' لا 'ar_LY'، لأن الأخير يُخرج ٠١٢٣.
class Fmt {
  Fmt._();

  static final _money = NumberFormat('#,##0.00#', 'en');
  static final _int = NumberFormat('#,##0', 'en');
  static final _rate4 = NumberFormat('#,##0.0###', 'en');

  /// المبالغ بخانتين عشريتين، **وثالثة متى حملت قيمة** — قرار المالك
  /// (30 أغسطس 2026):
  ///
  ///     1000        ⇦  1,000.00
  ///     1000.325    ⇦  1,000.325
  ///     1000.05     ⇦  1,000.05
  ///
  /// النمط `#,##0.00#`: خانتان إلزاميتان وثالثة اختيارية. والغرض محاسبي لا
  /// جمالي — الدينار الليبي = 1000 درهم، فالخانة الثالثة **مال حقيقي**.
  /// قصْر العرض على خانتين كان يُخفي الدرهم ويفتح باب فروق في الجرد، فلا
  /// تُعِدها إلى `#,##0.00`. وثلاثُ خانات دائماً (`#,##0.000`) مرفوضة أيضاً:
  /// أُقرّ ألّا تظهر أصفار لا تضيف معنى.
  static String money(num? v) => _money.format(v ?? 0);

  static String moneyWithSign(num v, {bool credit = false}) {
    final sign = credit ? '+ ' : '− ';
    return '$sign${_money.format(v.abs())}';
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

/// المرشِّحات القياسية لحقل مبلغ. **الترتيب جزء من الصحة، لا ذوق:**
///
/// 1. [WesternDigits] أولاً — قبل التصفية، وإلا حُذف الرقم الهندي قبل تحويله.
/// 2. التصفية تسمح بالفاصلة `,` — بدونها تُحذف فواصل الآلاف فور وضعها.
/// 3. [ThousandsGrouping] أخيراً — يضع الفواصل بعد أن استقرّ النص.
///
/// استعمِلها في كل حقل مبلغ بدل تكرار القائمة، فالترتيب يُنسى بسهولة.
final moneyInputFormatters = <TextInputFormatter>[
  const WesternDigits(),
  FilteringTextInputFormatter.allow(RegExp(r'[0-9.,]')),
  const ThousandsGrouping(),
];

/// يضع فواصل الآلاف أثناء الكتابة: `2500` ⇦ `2,500`
///
/// **القراءة آمنة:** كل مواضع قراءة المبلغ تمرّ بـ [Fmt.num_] وهي تزيل
/// الفواصل قبل التحويل، فلا يصل إلى الخادم نصٌّ فيه فاصلة.
class ThousandsGrouping extends TextInputFormatter {
  const ThousandsGrouping();

  static String group(String input) {
    // نُبقي أول نقطة فقط: «1.2.3» ليس مبلغاً، وكان يُقرأ صفراً بصمت.
    final cleaned = input.replaceAll(',', '');
    final dot = cleaned.indexOf('.');
    final intDigits =
        (dot < 0 ? cleaned : cleaned.substring(0, dot)).replaceAll('.', '');
    final frac = dot < 0 ? null : cleaned.substring(dot + 1).replaceAll('.', '');

    final b = StringBuffer();
    for (var i = 0; i < intDigits.length; i++) {
      if (i > 0 && (intDigits.length - i) % 3 == 0) b.write(',');
      b.write(intDigits[i]);
    }
    return frac == null ? b.toString() : '${b.toString()}.$frac';
  }

  @override
  TextEditingValue formatEditUpdate(
    TextEditingValue oldValue,
    TextEditingValue newValue,
  ) {
    final text = group(newValue.text);
    if (text == newValue.text) return newValue;

    // موضع المؤشّر يُحفظ بعدد المحارف **المهمّة** قبله لا بموضعه الحرفي،
    // وإلا قفز عند كل فاصلة تُضاف أو تُحذف.
    final caret = newValue.selection.baseOffset.clamp(0, newValue.text.length);
    var significant = 0;
    for (var i = 0; i < caret; i++) {
      if (newValue.text[i] != ',') significant++;
    }

    var offset = 0;
    if (significant > 0) {
      offset = text.length;
      var seen = 0;
      for (var i = 0; i < text.length; i++) {
        if (text[i] != ',') seen++;
        if (seen == significant) {
          offset = i + 1;
          break;
        }
      }
    }

    return TextEditingValue(
      text: text,
      selection: TextSelection.collapsed(offset: offset),
    );
  }
}

/// عقدة تركيز لحقل رقمي، تحمل قرارَي المالك (30 أغسطس 2026):
///
/// **١ — تُفرِغ الحقل بمجرّد دخول المؤشّر إليه.** لئلا يلتحم رقمٌ سابق بما
/// يكتبه الوكيل فيخرج مبلغ غير الذي قصده. وإن خرج المؤشّر ولم يُكتب شيء
/// **أُعيد ما كان**: التنظيف تهيئةٌ لأوّل رقم، لا حذفٌ لما أدخله الوكيل —
/// وبدون هذا الاسترجاع تكفي لمسةٌ عابرة على الحقل لتضيع الحوالة كلها.
///
/// **٢ — تُعيد تنسيق المبلغ حين يخرج المؤشّر** إذا كان [formatOnExit]:
/// `2500` ⇦ `2,500.00`. عند الخروج لا أثناء الكتابة، لأن فرض الخانتين مع
/// كل ضغطة يجعل إدخال الكسور مستحيلاً: يكتب `2` فتصير `2.00` ثم لا يجد
/// أين يضع النقطة.
///
/// [onChanged] ضروري لا تجميلي: تعيين `controller.text` برمجياً **لا**
/// يستدعي `onChanged` الخاصّ بالحقل، فبدونه يبقى سطر الإجمالي على قيمته
/// القديمة بينما الحقل يعرض غيرها.
class NumericFieldFocus extends FocusNode {
  NumericFieldFocus(
    this._controller, {
    VoidCallback? onChanged,
    bool formatOnExit = false,
  })  : _onChanged = onChanged,
        _formatOnExit = formatOnExit {
    addListener(_onFocusChange);
  }

  final TextEditingController _controller;
  final VoidCallback? _onChanged;
  final bool _formatOnExit;

  /// ما كان في الحقل لحظة دخول المؤشّر — يُعاد إن خرج بلا كتابة.
  String _before = '';

  void _onFocusChange() {
    if (hasFocus) {
      _before = _controller.text;
      if (_controller.text.isNotEmpty) {
        _controller.clear();
        _onChanged?.call();
      }
      return;
    }

    if (_controller.text.isEmpty && _before.isNotEmpty) {
      _controller.text = _before;
      _onChanged?.call();
    }
    _before = '';

    if (!_formatOnExit) return;
    final raw = _controller.text.trim();
    if (raw.isEmpty) return; // يبقى فارغاً ليظهر التلميح
    final formatted = Fmt.money(Fmt.num_(raw));
    if (formatted == _controller.text) return;
    _controller.text = formatted;
    _onChanged?.call();
  }

  @override
  void dispose() {
    removeListener(_onFocusChange);
    super.dispose();
  }
}

/// يحوّل ما تُدخِله لوحة المفاتيح العربية (٠١٢٣) إلى أرقام غربية (0123).
///
/// **يجب أن يسبق `FilteringTextInputFormatter` في `inputFormatters`** — فتلك
/// تسمح بـ `[0-9]` فقط، فتحذف الرقم الهندي قبل أن تصل إليه نوبتنا، فيبدو
/// للوكيل أن لوحة المفاتيح لا تكتب شيئاً.
///
/// التحويل واحد-بواحد في الطول عمداً، فلا يقفز مؤشّر الكتابة.
class WesternDigits extends TextInputFormatter {
  const WesternDigits();

  static const _arabicIndic = 0x0660; // ٠
  static const _extendedArabicIndic = 0x06F0; // ۰
  static const _arabicDecimalSeparator = 0x066B; // ٫

  static String normalize(String s) {
    final b = StringBuffer();
    for (final r in s.runes) {
      if (r >= _arabicIndic && r <= _arabicIndic + 9) {
        b.writeCharCode(0x30 + r - _arabicIndic);
      } else if (r >= _extendedArabicIndic && r <= _extendedArabicIndic + 9) {
        b.writeCharCode(0x30 + r - _extendedArabicIndic);
      } else if (r == _arabicDecimalSeparator) {
        b.write('.');
      } else {
        b.writeCharCode(r);
      }
    }
    return b.toString();
  }

  @override
  TextEditingValue formatEditUpdate(
    TextEditingValue oldValue,
    TextEditingValue newValue,
  ) {
    final text = normalize(newValue.text);
    if (text == newValue.text) return newValue;
    return TextEditingValue(
      text: text,
      selection: newValue.selection,
      composing: TextRange.empty,
    );
  }
}
