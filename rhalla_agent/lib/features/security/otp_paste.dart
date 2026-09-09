import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';

/* ══════════════════════════════════════════════════════════════════════════
   «لصق الرمز» — بديلُ AutoFill في شاشةٍ بلوحةِ أرقامٍ مخصّصة
   ══════════════════════════════════════════════════════════════════════════

   ── ⚠ لماذا لا AutoFill النظاميّ هنا ─────────────────────────────────────

   AutoFill لرموز التحقّق — `AutofillHints.oneTimeCode` على أندرويد،
   و`textContentType: .oneTimeCode` على iOS — **يُعرض فوق كيبورد النظام**،
   ويشترط حقلَ نصٍّ حقيقيّاً مركَّزاً.

   وشاشتا الرمز في هذا التطبيق (الوكيل والموظف) **لا تحويان حقلَ نصٍّ
   أصلاً**: خاناتٌ للعرض ولوحةُ أرقامٍ مرسومة، وذلك قرارُ تصميمٍ مكتوبٌ في
   شاشة الموظف حرفاً: «الشاشة لا تحوي `TextField` للرمز أصلاً، فلا كيبورد
   نظام يفتح لها».

   فإضافةُ حقلٍ خفيّ لاستدعاء AutoFill تفتح كيبورد النظام فوق اللوحة
   المرسومة — أي تنقض التصميم لتُظهر اقتراحاً قد لا يظهر أصلاً: اقتراحُ
   الرمز يعتمد على أن يقرأ النظامُ الرسالة، وهو يفعل ذلك لرسائل **SMS**
   لا لرسائل واتساب.

   ── ⚠ وما لا يجوز أصلاً ──────────────────────────────────────────────────

   قراءةُ محتوى واتساب من داخل تطبيقنا **مستحيلةٌ نظاميّاً وممنوعةٌ في
   المتجرين**: لا Accessibility Service ولا Notification Listener ولا
   `READ_SMS`. وأيُّها سببُ رفضٍ في Google Play ومراجعة Apple.

   ── فماذا يبقى ───────────────────────────────────────────────────────────

   ما يفعله المستخدم أصلاً: ينسخ الرمز من واتساب. فيُختصر الطريقُ إلى
   **لمسةٍ واحدة** بدل كتابةِ أربعة أرقام.

   ⚠ **والقراءةُ بلمسةٍ صريحة لا تلقائياً**: iOS يُظهر إذنَ لصقٍ من النظام،
   وأندرويد ١٢+ يُظهر تنبيهاً بأن التطبيق قرأ الحافظة. وقراءةٌ صامتةٌ عند
   فتح الشاشة تُنتج ذلك التنبيه بلا سببٍ يفهمه المستخدم — وهي سلوكٌ
   تتشدّد فيه مراجعةُ المتجرين.
   ══════════════════════════════════════════════════════════════════════════ */

/// يستخرج رمزاً من نصٍّ منسوخ، أو `null` إن لم يكن فيه رمز.
///
/// ⚠ **يقبل النصَّ كما نُسخ لا الرقمَ وحدَه**: المستخدم ينسخ من واتساب
/// «رمز التحقق الخاص بك هو: 4821» بسطرها، لا الأربعةَ أرقام مجرّدة —
/// والاشتراطُ على المجرّد يجعل الزرّ لا يعمل أبداً في يد أحد.
String? extractOtp(String? clipboard, {int length = 4}) {
  if (clipboard == null) return null;

  final text = clipboard.trim();
  if (text.isEmpty || text.length > 400) return null;

  /*
   * ⚠ يُطلب **بالضبط** `length` من الأرقام المتجاورة — لا أوّلُ أربعةٍ من
   * رقمٍ أطول.
   *
   * رسالةُ التحقّق تحمل الرمز وحدَه رقماً؛ لكنّ حافظةً فيها رقمُ هاتفٍ
   * (`0922015243`) كانت ستُعطي `0922` فيُرسَل رمزٌ خاطئ ويُستهلك من عدد
   * المحاولات. والحدودُ حول المجموعة هي ما يمنع ذلك.
   */
  final re = RegExp(r'(?<!\d)\d{' '$length' r'}(?!\d)');
  final matches = re.allMatches(text).toList();

  // ⚠ ولا يُخمَّن حين يحتمل النصُّ رمزين: صامتٌ خيرٌ من رمزٍ خاطئ.
  if (matches.length != 1) return null;

  return matches.first.group(0);
}

/// زرُّ «لصق الرمز» — يظهر تحت لوحة الأرقام.
class PasteOtpButton extends StatefulWidget {
  const PasteOtpButton({
    super.key,
    required this.onCode,
    this.length = 4,
    this.enabled = true,
  });

  /// يُنادى برمزٍ صالحٍ فقط — والشاشةُ هي من تتحقّق منه كعادتها.
  final ValueChanged<String> onCode;

  final int length;
  final bool enabled;

  @override
  State<PasteOtpButton> createState() => _PasteOtpButtonState();
}

class _PasteOtpButtonState extends State<PasteOtpButton> {
  bool _busy = false;
  String? _note;

  Future<void> _paste() async {
    if (_busy || !widget.enabled) return;
    setState(() { _busy = true; _note = null; });

    try {
      final data = await Clipboard.getData(Clipboard.kTextPlain);
      final code = extractOtp(data?.text, length: widget.length);

      if (!mounted) return;

      if (code == null) {
        /*
         * ⚠ يُقال السببُ صراحةً.
         *
         * زرٌّ يُضغط فلا يقع شيء يُقرأ «التطبيق معطوب». والحالةُ الغالبة أنّ
         * المستخدم لم ينسخ الرمز بعد — وهذا ما تقوله الجملة.
         */
        setState(() => _note = 'لا يوجد رمز في الحافظة — انسخه من الرسالة أولاً.');
        return;
      }

      widget.onCode(code);
    } catch (_) {
      // منصّةٌ ترفض الحافظة، أو المستخدم رفض إذن اللصق في iOS.
      if (mounted) setState(() => _note = 'تعذّرت قراءة الحافظة.');
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  @override
  Widget build(BuildContext context) => Column(
        children: [
          TextButton.icon(
            onPressed: widget.enabled && !_busy ? _paste : null,
            icon: Icon(Icons.content_paste_rounded,
                size: 17, color: R.primaryDark),
            label: Text('لصق الرمز المنسوخ',
                style: T.plex(13, FontWeight.w600, color: R.primaryDark)),
            style: TextButton.styleFrom(
              minimumSize: const Size(0, 44),
              padding: const EdgeInsets.symmetric(horizontal: 14),
            ),
          ),
          if (_note != null)
            Padding(
              padding: const EdgeInsets.only(top: 2),
              child: Text(_note!,
                  textAlign: TextAlign.center,
                  style: T.plex(11.5, FontWeight.w400, color: R.inkA(.55))),
            ),
        ],
      );
}
