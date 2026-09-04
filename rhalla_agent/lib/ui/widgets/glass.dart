import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import 'ambient.dart';
import '../../core/keyboard.dart';

/// بطاقة زجاجية — الأساس البصري لكل شيء في هذا التطبيق.
class GlassCard extends StatelessWidget {
  const GlassCard({
    super.key,
    required this.child,
    this.padding = const EdgeInsets.symmetric(horizontal: 18, vertical: 16),
    this.radius = R.rCard,
    this.large = false,
    this.sheen = false,
    this.margin,
    this.onTap,
  });

  final Widget child;
  final EdgeInsetsGeometry padding;
  final double radius;

  /// النسخة الكبيرة — blur وظل أقوى، تُستعمل للبطاقات البطلة.
  final bool large;

  /// لمعان يمرّ فوق البطاقة كل 7 ثوانٍ.
  final bool sheen;
  final EdgeInsetsGeometry? margin;
  final VoidCallback? onTap;

  @override
  Widget build(BuildContext context) {
    final blur = large ? R.blurGlassXl : R.blurGlass;
    final content = ClipRRect(
      borderRadius: BorderRadius.circular(radius),
      child: BackdropFilter(
        filter: ImageFilter.blur(sigmaX: blur, sigmaY: blur),
        child: DecoratedBox(
          decoration: BoxDecoration(
            gradient: R.glassGradient(from: large ? .85 : .82),
            border: Border.all(color: R.whiteA(large ? .92 : .9)),
            borderRadius: BorderRadius.circular(radius),
          ),
          child: Stack(
            children: [
              Padding(padding: padding, child: child),
              if (sheen) const Positioned.fill(child: Sheen()),
            ],
          ),
        ),
      ),
    );

    return Container(
      margin: margin,
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(radius),
        boxShadow: large ? R.shCardL : R.shCardS,
      ),
      child: onTap == null
          ? content
          : Material(
              color: Colors.transparent,
              borderRadius: BorderRadius.circular(radius),
              child: InkWell(
                borderRadius: BorderRadius.circular(radius),
                onTap: onTap,
                child: content,
              ),
            ),
    );
  }
}

/// صف قائمة — العملية، نقطة البيع، حركة الحساب.
/// إيحاء صفّ حركة الحساب.
///
/// قرار المالك: الحوالة الصادرة (‏−) حمراء بكاملها والواردة (‏+) خضراء،
/// ليُميّزها الوكيل بلمحة بدل قراءة الإشارة. اللون على الصفّ كلّه لا على
/// الرقم وحده — الرقم وحده يُقرأ متأخّراً في قائمة طويلة.
enum RowTone {
  none,
  credit,
  debit;

  /// مزيج لا استبدال: الزجاج الأبيض يبقى تحته، واللون غسالة فوقه — وإلا
  /// فقد الصفّ شفافيته وخرج عن نظام التصميم.
  Color get fill => switch (this) {
        RowTone.none => R.whiteA(.7),
        RowTone.credit => Color.alphaBlend(R.creditA(.13), R.whiteA(.72)),
        RowTone.debit => Color.alphaBlend(R.debitA(.12), R.whiteA(.72)),
      };

  Color get border => switch (this) {
        RowTone.none => R.whiteA(.9),
        RowTone.credit => R.creditA(.30),
        RowTone.debit => R.debitA(.28),
      };

  /// لون المبلغ والإشارة وبيان الحوالة.
  Color get ink => switch (this) {
        RowTone.none => R.ink,
        RowTone.credit => R.credit,
        RowTone.debit => R.error,
      };

  Color get tile => switch (this) {
        RowTone.none => R.primaryA(.13),
        RowTone.credit => R.creditA(.16),
        RowTone.debit => R.debitA(.14),
      };
}

class GlassRow extends StatelessWidget {
  const GlassRow({
    super.key,
    required this.children,
    this.onTap,
    this.tone = RowTone.none,
    this.dense = false,
    this.footer,
  });

  final List<Widget> children;
  final VoidCallback? onTap;
  final RowTone tone;

  /// صفّ مضغوط بارتفاع **ثابت**.
  ///
  /// قرار المالك: صفوف الحركات كانت تأخذ مساحة أكبر مما تستحقّه بياناتها.
  /// والارتفاع ثابت لا تابعٌ للمحتوى عمداً — فالقائمة تبقى موحّدة الإيقاع
  /// بدل أن يتغيّر حجم كل حاوية بطول ما فيها.
  static const denseHeight = 56.0;
  final bool dense;

  /// سطرٌ تابع أسفل الصفّ، **داخل الحاوية نفسها**.
  ///
  /// أُضيف لعرض عمولة الحوالة تحتها في وحدةٍ بصرية واحدة (قرار المالك،
  /// 3 سبتمبر 2026). وحين يكون `null` يبقى الصفّ كما كان حرفياً — بارتفاعه
  /// الثابت وحشوه — فلا يتغيّر شكل أي صفٍّ قائم.
  final Widget? footer;

  @override
  Widget build(BuildContext context) {
    final row = SizedBox(
      height: dense ? denseHeight : null,
      child: Row(children: children),
    );

    final body = ClipRRect(
      borderRadius: BorderRadius.circular(R.rRow),
      child: BackdropFilter(
        filter: ImageFilter.blur(sigmaX: R.blurRow, sigmaY: R.blurRow),
        child: Container(
          constraints: dense ? null : const BoxConstraints(minHeight: 44),
          padding: EdgeInsets.fromLTRB(
            dense ? 13 : 16,
            dense ? 0 : 14,
            dense ? 13 : 16,
            // الحشو السفلي يظهر مع السطر التابع فقط: بدونه يبقى الصفّ
            // المضغوط بارتفاعه الثابت كما صُمّم.
            dense ? (footer == null ? 0 : 10) : 14,
          ),
          decoration: BoxDecoration(
            color: tone.fill,
            border: Border.all(color: tone.border),
            borderRadius: BorderRadius.circular(R.rRow),
          ),
          child: footer == null
              ? row
              : Column(
                  mainAxisSize: MainAxisSize.min,
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [row, footer!],
                ),
        ),
      ),
    );

    return DecoratedBox(
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(R.rRow),
        boxShadow: R.shRow,
      ),
      child: onTap == null
          ? body
          : Material(
              color: Colors.transparent,
              borderRadius: BorderRadius.circular(R.rRow),
              child: InkWell(
                borderRadius: BorderRadius.circular(R.rRow),
                onTap: onTap,
                child: body,
              ),
            ),
    );
  }
}

/// مربّع الأيقونة داخل الصف.
class IconTile extends StatelessWidget {
  const IconTile({
    super.key,
    this.icon,
    this.letter,
    this.color,
    this.background,
    this.size = 40,
  });

  final Widget? icon;
  final String? letter;
  final Color? color;
  final Color? background;

  /// 40 في البطاقات، وأصغر في الصفوف المضغوطة.
  final double size;

  @override
  Widget build(BuildContext context) => Container(
        width: size,
        height: size,
        decoration: BoxDecoration(
          color: background ?? R.primaryA(.13),
          borderRadius: BorderRadius.circular(size >= 40 ? R.rTile : 10),
        ),
        alignment: Alignment.center,
        child: icon ??
            Text(
              letter ?? '',
              style: T.kufi(14, FontWeight.w600, color: color ?? R.primaryGradEnd),
            ),
      );
}

/// الشعار — يُرسم كقناع بلون واحد، تماماً كما في التصميم.
class RhallaLogo extends StatelessWidget {
  const RhallaLogo({super.key, required this.size, required this.color});

  final double size;
  final Color color;

  @override
  Widget build(BuildContext context) => SvgPicture.asset(
        'assets/brand/rhalla-logo.svg',
        width: size,
        height: size,
        colorFilter: ColorFilter.mode(color, BlendMode.srcIn),
      );
}

/// قرص الحالة — علامة صحّ خضراء أو ✕ حمراء، بهالةٍ حولها.
///
/// شكل واحد للحالتين عمداً: النجاح والفشل يقعان في المكان نفسه من الشاشة،
/// فيقرأ الوكيل النتيجة من اللون والرمز بلا أن يبحث عنها.
class StatusDisc extends StatelessWidget {
  const StatusDisc.success({super.key, this.size = 96})
      : _ok = true;

  const StatusDisc.failure({super.key, this.size = 96})
      : _ok = false;

  final double size;
  final bool _ok;

  @override
  Widget build(BuildContext context) {
    final tint = _ok ? R.primary : R.error;

    return SizedBox(
      width: size,
      height: size,
      child: Stack(
        alignment: Alignment.center,
        children: [
          // الهالة: قرص شفيف أوسع من الرمز يفصله عن الخلفية.
          Container(
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: tint.withValues(alpha: .10),
            ),
          ),
          Container(
            width: size * .70,
            height: size * .70,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: tint,
              boxShadow: [
                BoxShadow(
                  color: tint.withValues(alpha: .36),
                  blurRadius: 30,
                  offset: const Offset(0, 14),
                )
              ],
            ),
            child: Icon(
              _ok ? Icons.check_rounded : Icons.close_rounded,
              size: size * .34,
              color: Colors.white,
            ),
          ),
        ],
      ),
    );
  }
}

/// ترويسة العلامة — القرص الأخضر واسم الشركة تحته.
///
/// تُصدَّر شاشات الدخول بها بقرار المالك (2 سبتمبر 2026): الوكيل يرى اسم من
/// يتعامل معه قبل أن يُدخل رقمه.
///
/// الاسم **«شركة الرحالة للحوالات المالية»** لا «للصرافة». التغيير ليس
/// تجميلاً: صفة «الصرافة» تتعارض مع موافقات مصرف ليبيا المركزي وقد تُوقع
/// الشركة في مشكلة نظامية. فلا يُعاد اللفظ القديم إلى أي شاشة.
class BrandLockup extends StatelessWidget {
  const BrandLockup({super.key, this.logoSize = 64});

  final double logoSize;

  /// الاسم النظامي كاملاً. مكتوب هنا مرّة واحدة، فتغييره لاحقاً موضع واحد.
  static const companyName = 'شركة الرحالة للحوالات المالية';

  @override
  Widget build(BuildContext context) => Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          Container(
            width: logoSize,
            height: logoSize,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: R.whiteA(.75),
              border: Border.all(color: R.primaryA(.28), width: 1.5),
              boxShadow: [
                BoxShadow(
                  color: R.primaryA(.18),
                  blurRadius: 24,
                  offset: const Offset(0, 10),
                )
              ],
            ),
            alignment: Alignment.center,
            child: RhallaLogo(size: logoSize * .62, color: R.primary),
          ),
          const SizedBox(height: 10),
          Text(
            companyName,
            textAlign: TextAlign.center,
            style: T.kufi(13, FontWeight.w700, color: R.primaryDark),
          ),
        ],
      );
}

/// خلفية الشاشة — بلا كتل متحركة (تأتي من AmbientBackground فوق الـ Navigator).
class Screen extends StatelessWidget {
  const Screen({super.key, required this.child, this.bottomBar});

  final Widget child;
  final Widget? bottomBar;

  @override
  Widget build(BuildContext context) => Scaffold(
        backgroundColor: Colors.transparent,
        // لمسةٌ في الفراغ تُغلق لوحة المفاتيح (قرار المالك، 4 سبتمبر 2026).
        //
        // اللوحة كانت تبقى مفتوحة بعد مغادرة الحقل فتأكل نصف الشاشة: يكتب
        // الوكيل في البحث ثم يبدّل التبويب، فيختفي الحقل من الشجرة **ولا
        // يُفقد التركيز** — والنظام يُبقي اللوحة لأن لا أحد أخبره أن الحقل
        // انصرف.
        //
        // `translucent` لا `opaque`: الأزرار والحقول تلتقط لمساتها أولاً،
        // ولا يصل إلى هنا إلا ما لم يلتقطه أحد — أي اللمس في الفراغ.
        body: GestureDetector(
          behavior: HitTestBehavior.translucent,
          onTap: () => hideKeyboard(),
          child: Stack(
            children: [
              SafeArea(bottom: false, child: child),
              if (bottomBar != null)
                PositionedDirectional(
                  start: 16,
                  end: 16,
                  bottom: 14,
                  child: bottomBar!,
                ),
            ],
          ),
        ),
      );
}
