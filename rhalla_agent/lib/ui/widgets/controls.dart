import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';

/// الزر الأساسي. حالتا «معطّل» و«تحميل» لم تكونا في التصميم — عُرّفتا هنا.
class PrimaryButton extends StatelessWidget {
  const PrimaryButton({
    super.key,
    required this.label,
    this.onPressed,
    this.loading = false,
    this.icon,
    this.height = 56,
  });

  final String label;
  final VoidCallback? onPressed;
  final bool loading;
  final Widget? icon;

  /// 56 في كل مكان. تُخفَّض في الشاشات المزدحمة وحدها — لا تنزل تحت 48،
  /// وهو الحدّ الذي يبقى دونه الزرّ صعب الإصابة بالإبهام.
  final double height;

  @override
  Widget build(BuildContext context) {
    final enabled = onPressed != null && !loading;

    return Opacity(
      opacity: enabled ? 1 : .45,
      child: DecoratedBox(
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(R.rPill),
          boxShadow: enabled ? R.shCta : null,
        ),
        child: Material(
          color: Colors.transparent,
          borderRadius: BorderRadius.circular(R.rPill),
          child: InkWell(
            borderRadius: BorderRadius.circular(R.rPill),
            onTap: enabled
                ? () {
                    HapticFeedback.lightImpact();
                    onPressed!();
                  }
                : null,
            child: Ink(
              height: height,
              decoration: BoxDecoration(
                gradient: R.primaryGradient,
                borderRadius: BorderRadius.circular(R.rPill),
              ),
              child: Center(
                child: loading
                    ? const SizedBox(
                        width: 22,
                        height: 22,
                        child: CircularProgressIndicator(
                          strokeWidth: 2.4,
                          valueColor: AlwaysStoppedAnimation(Colors.white),
                        ),
                      )
                    : Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          if (icon != null) ...[icon!, const SizedBox(width: 10)],
                          Text(label, style: T.cta),
                        ],
                      ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}

/// زر زجاجي ثانوي.
class GlassButton extends StatelessWidget {
  const GlassButton({
    super.key,
    required this.label,
    this.onPressed,
    this.height = 54,
  });

  final String label;
  final VoidCallback? onPressed;

  /// كما في [PrimaryButton]: تُخفَّض في الشاشات المزدحمة ولا تنزل تحت 48.
  final double height;

  @override
  Widget build(BuildContext context) => ClipRRect(
        borderRadius: BorderRadius.circular(R.rPill),
        child: BackdropFilter(
          filter: ImageFilter.blur(sigmaX: R.blurChip, sigmaY: R.blurChip),
          child: Material(
            color: R.whiteA(.75),
            child: InkWell(
              onTap: onPressed,
              child: Container(
                height: height,
                alignment: Alignment.center,
                decoration: BoxDecoration(
                  border: Border.all(color: R.whiteA(.9)),
                  borderRadius: BorderRadius.circular(R.rPill),
                ),
                child: Text(label, style: T.kufi(14, FontWeight.w600, color: R.primaryDark)),
              ),
            ),
          ),
        ),
      );
}

/// زر أيقونة دائري — الرجوع والمشاركة.
class CircleIconButton extends StatelessWidget {
  const CircleIconButton({super.key, required this.child, this.onPressed});

  final Widget child;
  final VoidCallback? onPressed;

  @override
  Widget build(BuildContext context) => ClipOval(
        child: BackdropFilter(
          filter: ImageFilter.blur(sigmaX: R.blurChip, sigmaY: R.blurChip),
          child: Material(
            color: R.whiteA(.6),
            shape: CircleBorder(side: BorderSide(color: R.whiteA(.85))),
            child: InkWell(
              onTap: onPressed,
              customBorder: const CircleBorder(),
              child: SizedBox(width: 44, height: 44, child: Center(child: child)),
            ),
          ),
        ),
      );
}

/// شريط التطبيق — عنوان وسطر فرعي وزر رجوع اختياري.
class RhallaAppBar extends StatelessWidget {
  const RhallaAppBar({
    super.key,
    required this.title,
    this.subtitle,
    this.onBack,
    this.trailing,
  });

  final String title;
  final String? subtitle;
  final VoidCallback? onBack;
  final Widget? trailing;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.fromLTRB(R.padScreen, 8, R.padScreen, 0),
        child: Row(
          children: [
            if (onBack != null) ...[
              CircleIconButton(
                onPressed: onBack,
                child: const Icon(Icons.arrow_back_ios_new, size: 16, color: R.ink),
              ),
              const SizedBox(width: 14),
            ],
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(title, style: T.appBarTitle),
                  if (subtitle != null) ...[
                    const SizedBox(height: 8),
                    Text(subtitle!, style: T.label),
                  ],
                ],
              ),
            ),
            if (trailing != null) ...[const SizedBox(width: 14), trailing!],
          ],
        ),
      );
}

/// لوحة الأرقام — LTR، ‏12 خانة، بلا فاصلة عشرية وبلا مفتاح تأكيد.
class NumericKeypad extends StatelessWidget {
  const NumericKeypad({super.key, required this.onDigit, required this.onDelete});

  final ValueChanged<String> onDigit;
  final VoidCallback onDelete;

  Widget _key(Widget child, VoidCallback onTap) => Material(
        color: R.whiteA(.55),
        borderRadius: BorderRadius.circular(R.rKey),
        child: InkWell(
          borderRadius: BorderRadius.circular(R.rKey),
          onTap: () {
            HapticFeedback.selectionClick();
            onTap();
          },
          child: Container(
            height: 52,
            alignment: Alignment.center,
            decoration: BoxDecoration(
              border: Border.all(color: R.whiteA(.8)),
              borderRadius: BorderRadius.circular(R.rKey),
            ),
            child: child,
          ),
        ),
      );

  @override
  Widget build(BuildContext context) {
    final digits = ['1', '2', '3', '4', '5', '6', '7', '8', '9'];

    return Directionality(
      textDirection: TextDirection.ltr,
      child: Padding(
        padding: const EdgeInsets.fromLTRB(14, 0, 14, 14),
        child: GridView.count(
          crossAxisCount: 3,
          shrinkWrap: true,
          physics: const NeverScrollableScrollPhysics(),
          mainAxisSpacing: 9,
          crossAxisSpacing: 9,
          childAspectRatio: 124 / 52,
          children: [
            for (final d in digits)
              _key(Text(d, style: T.kufi(22, FontWeight.w500)), () => onDigit(d)),
            const SizedBox.shrink(),
            _key(Text('0', style: T.kufi(22, FontWeight.w500)), () => onDigit('0')),
            Material(
              color: R.inkA(.05),
              borderRadius: BorderRadius.circular(R.rKey),
              child: InkWell(
                borderRadius: BorderRadius.circular(R.rKey),
                onTap: () {
                  HapticFeedback.selectionClick();
                  onDelete();
                },
                child: Container(
                  height: 52,
                  alignment: Alignment.center,
                  decoration: BoxDecoration(
                    border: Border.all(color: R.whiteA(.55)),
                    borderRadius: BorderRadius.circular(R.rKey),
                  ),
                  child: Icon(Icons.backspace_outlined, size: 21, color: R.inkA(.6)),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

/// يجعل شاشةً ترسم [NumericKeypad] تقبل أرقام كيبورد الجهاز أيضاً.
///
/// شاشتا الهاتف والرمز لا تحتويان `TextField` إطلاقاً: الرقم يُبنى حرفاً حرفاً
/// من نقرات اللوحة المرسومة. فلا شيء فيهما يستقبل ضغطة مفتاح، ولا ينفع معهما
/// ضبط أي تخطيط لوحة مفاتيح في النظام — وعلى المحاكي يعني ذلك أن كل رقم يحتاج
/// نقرة ماوس، فتصير كل تجربة دخول عملاً يدوياً بطيئاً.
///
/// يُركَّب على الحالة لا على شجرة الودجات: لا يوجد في هاتين الشاشتين ما يأخذ
/// التركيز، فالالتقاط على مستوى [HardwareKeyboard] يكفي ويجعل الإضافة ثلاثة
/// أسطر لا إعادةَ لفٍّ لكامل `build`.
///
/// المفاتيح تُحوَّل إلى نفس النداءات التي تستدعيها اللوحة المرسومة، فيبقى
/// مصدر الحقيقة واحداً ولا يتفرّع السلوك بين مدخلين.
mixin HardwareDigits<T extends StatefulWidget> on State<T> {
  void onHardwareDigit(String digit);
  void onHardwareDelete();

  /// ما يفعله Enter. الافتراضي لا شيء — شاشة الرمز تتحقّق وحدها عند اكتمال
  /// الخانات الأربع، فلا معنى لمفتاح تأكيد فيها.
  void onHardwareSubmit() {}

  @override
  void initState() {
    super.initState();
    HardwareKeyboard.instance.addHandler(_handleKey);
  }

  @override
  void dispose() {
    HardwareKeyboard.instance.removeHandler(_handleKey);
    super.dispose();
  }

  bool _handleKey(KeyEvent event) {
    if (!mounted) return false;

    // المستمع عام، والشاشة قد تكون تحت شاشة أخرى — شاشة الهاتف تبقى حيّة
    // تحت شاشة الرمز. بلا هذا الشرط تلتقط الشاشتان الضغطة نفسها.
    if (ModalRoute.of(context)?.isCurrent != true) return false;

    if (event is KeyUpEvent) return false;

    final key = event.logicalKey;

    // المسح وحده يقبل التكرار: إبقاء Backspace مضغوطاً يمسح الحقل تباعاً،
    // وهذا هو المقصود دائماً.
    if (key == LogicalKeyboardKey.backspace ||
        key == LogicalKeyboardKey.delete) {
      onHardwareDelete();
      return true;
    }

    // وما بعده لا يقبله. حقل النص العادي يكرّر الحرف عند الضغط المطوّل،
    // وهاتان ليستا حقلَي نص: مفتاح عالق أو ضغطة طويلة تضاعف رقماً داخل رقم
    // هاتف أو رمز تحقّق دون أن ينتبه الوكيل، ولا أحد يقصد كتابة «5555»
    // بإبقاء 5 مضغوطاً. والتكرار على Enter أسوأ — إرسال مكرّر.
    //
    // ليس افتراضاً: ضغطة واحدة أُبقيت نصف ثانية أنتجت ثلاثة أرقام في القياس.
    if (event is! KeyDownEvent) return false;

    if (key == LogicalKeyboardKey.enter ||
        key == LogicalKeyboardKey.numpadEnter) {
      onHardwareSubmit();
      return true;
    }

    final digit = _digitOf(event);
    if (digit == null) return false;
    onHardwareDigit(digit);
    return true;
  }

  /// يقرأ الحرف الناتج لا موضع المفتاح، فيعمل مع صفّ الأرقام ولوحة الأرقام
  /// معاً ومع أي تخطيط لوحة مفاتيح — بما فيه التخطيط العربي.
  ///
  /// ويحوّل الأرقام العربية-الهندية والفارسية إلى غربية، تماماً كما يفعل
  /// `WesternDigits` في الحقول العادية: كل رقم في هذا التطبيق غربي.
  static String? _digitOf(KeyEvent event) {
    final ch = event.character;
    if (ch == null || ch.length != 1) return null;

    final c = ch.codeUnitAt(0);
    if (c >= 0x0030 && c <= 0x0039) return ch; // 0-9
    if (c >= 0x0660 && c <= 0x0669) {
      return String.fromCharCode(c - 0x0660 + 0x30); // ٠-٩
    }
    if (c >= 0x06F0 && c <= 0x06F9) {
      return String.fromCharCode(c - 0x06F0 + 0x30); // ۰-۹
    }
    return null;
  }
}

/// علم ليبيا المبسّط.
class LibyaFlag extends StatelessWidget {
  const LibyaFlag({super.key});

  @override
  Widget build(BuildContext context) => Container(
        width: 24,
        height: 17,
        clipBehavior: Clip.antiAlias,
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(3),
          boxShadow: [BoxShadow(color: R.inkA(.08), spreadRadius: 1, blurRadius: 0)],
        ),
        child: Column(
          children: [
            const Expanded(child: ColoredBox(color: R.flagRed, child: SizedBox.expand())),
            Expanded(
              child: ColoredBox(
                color: R.flagBlack,
                child: Center(
                  child: Container(
                    width: 7,
                    height: 7,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      border: Border.all(color: Colors.white, width: 1.5),
                    ),
                  ),
                ),
              ),
            ),
            const Expanded(child: ColoredBox(color: R.flagGreen, child: SizedBox.expand())),
          ],
        ),
      );
}

/// مؤشّر وامض — يحاكي مؤشّر الإدخال في التصميم.
class BlinkingCaret extends StatefulWidget {
  const BlinkingCaret({super.key, this.height = 22});

  final double height;

  @override
  State<BlinkingCaret> createState() => _BlinkingCaretState();
}

class _BlinkingCaretState extends State<BlinkingCaret>
    with SingleTickerProviderStateMixin {
  late final _c = AnimationController(
    vsync: this,
    duration: const Duration(milliseconds: 500),
  )..repeat(reverse: true);

  @override
  void dispose() {
    _c.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => FadeTransition(
        opacity: _c,
        child: Container(
          width: 2,
          height: widget.height,
          decoration: BoxDecoration(
            color: R.primary,
            borderRadius: BorderRadius.circular(2),
          ),
        ),
      );
}

/// خانات رمز التحقّق — قيم الحالات مأخوذة من منطق مكوّن التصميم.
class OtpBoxes extends StatelessWidget {
  const OtpBoxes({super.key, required this.value, this.length = 6});

  final String value;
  final int length;

  @override
  Widget build(BuildContext context) => Directionality(
        textDirection: TextDirection.ltr,
        child: Row(
          children: [
            for (var i = 0; i < length; i++) ...[
              if (i > 0) const SizedBox(width: 9),
              Expanded(child: _cell(i)),
            ],
          ],
        ),
      );

  Widget _cell(int i) {
    final ch = i < value.length ? value[i] : '';
    final active = i == value.length;
    final filled = ch.isNotEmpty;

    return AnimatedContainer(
      duration: const Duration(milliseconds: 200),
      height: 50,
      alignment: Alignment.center,
      decoration: BoxDecoration(
        color: R.whiteA(filled ? .9 : .5),
        borderRadius: BorderRadius.circular(R.rOtp),
        border: Border.all(
          width: 1.5,
          color: active
              ? R.primary
              : filled
                  ? R.primaryA(.35)
                  : R.whiteA(.9),
        ),
        boxShadow: active
            ? [BoxShadow(color: R.primaryA(.13), blurRadius: 0, spreadRadius: 4)]
            : [
                BoxShadow(
                  color: const Color(0xFF032D21).withValues(alpha: .06),
                  blurRadius: 20,
                  offset: const Offset(0, 8),
                )
              ],
      ),
      child: Text(ch, style: T.kufi(24, FontWeight.w600)),
    );
  }
}

/// لافتة تنبيه كهرمانية.
class WarnBanner extends StatelessWidget {
  const WarnBanner({super.key, required this.text, this.icon});

  final String text;
  final IconData? icon;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 9),
        decoration: BoxDecoration(
          color: R.warnBg,
          border: Border.all(color: R.warnBorder),
          borderRadius: BorderRadius.circular(18),
        ),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Icon(icon ?? Icons.warning_amber_rounded, size: 17, color: R.warnIcon),
            const SizedBox(width: 11),
            Expanded(
              child: Text(
                text,
                style: T.plex(11.5, FontWeight.w500, color: R.warnInk, height: 1.65),
              ),
            ),
          ],
        ),
      );
}

/// صفّ «مفتاح · قيمة» في بطاقات المراجعة.
///
/// كان `_Kv` خاصاً بـ `review_screen.dart`. رُقّي هنا حين احتاجته مراجعة
/// التحويل بين الحسابات — نسخه كان سيجعل صفّين يتباعدان بصمت.
class KvRow extends StatelessWidget {
  const KvRow(this.k, this.v,
      {super.key, this.numeric = false, this.strong = false, this.sub});

  final String k;
  final String v;

  /// القيم الرقمية تُلَفّ بـ ltr؛ العربية لا — تنكسر لحظة اختلاطها برقم.
  final bool numeric;
  final bool strong;
  final String? sub;

  @override
  Widget build(BuildContext context) => Row(
        crossAxisAlignment:
            sub == null ? CrossAxisAlignment.center : CrossAxisAlignment.start,
        children: [
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(k,
                  style: T.plex(strong ? 12.5 : 12,
                      strong ? FontWeight.w500 : FontWeight.w400,
                      color: R.inkA(strong ? .6 : .55))),
              if (sub != null) ...[
                const SizedBox(height: 7),
                Text(sub!, style: T.meta),
              ],
            ],
          ),
          const Spacer(),
          numeric
              ? Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(v,
                      style: T.kufi(strong ? 17 : 14, FontWeight.w700)),
                )
              : Text(v, style: T.plex(13.5, FontWeight.w600)),
        ],
      );
}
