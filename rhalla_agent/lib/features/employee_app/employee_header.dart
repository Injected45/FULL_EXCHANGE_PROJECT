import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/glass.dart';
import '../branding/brand_mark.dart';
import '../branding/branding_controller.dart';
import 'employee_security_screen.dart';
import 'employee_session.dart';

/// ترويسةُ الموظف — اسمُه ونقطةُ بيعه وشعارُ شركته وزرّ أمان جهازه.
///
/// ══════════════════════════════════════════════════════════════════════════
///  لماذا خرجت من الشاشة الرئيسية إلى ملفٍّ مستقلّ
/// ══════════════════════════════════════════════════════════════════════════
///
/// كانت جزءاً من `employee_home_screen.dart` حين كانت الواجهةُ شاشةً واحدة.
/// وبعد إعادة الهيكلة (10 سبتمبر 2026) صارت خمسةَ تبويبات، والترويسةُ تعلو
/// كلَّ واحدٍ منها.
///
/// ⚠ ونسخُها في خمسِ شاشات كان يعني خمسَ ترويساتٍ تفترق: تُصحَّح نقطةُ البيع
/// في واحدةٍ وتبقى في أربع. فهي عنصرٌ واحدٌ يُبنى خمس مرّات.
///
/// ⚠ ولم يسقط منها حرف: الشعارُ في دائرته، والاسمُ، ونقطةُ البيع الفعّالة،
/// وزرُّ «الأمان» — كما كانت تماماً.
class EmployeeHeader extends ConsumerWidget {
  const EmployeeHeader({super.key, required this.profile, this.trailing});

  final EmployeeProfile profile;

  /// عنصرٌ إضافيّ يسار زرّ الأمان — تستعمله شاشةٌ تحتاج فعلاً في ترويستها.
  final Widget? trailing;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final top = MediaQuery.paddingOf(context).top;
    final company = ref.watch(brandingControllerProvider).branding;

    return Container(
      padding: EdgeInsets.fromLTRB(R.padScreen, top + 16, R.padScreen, 24),
      decoration: BoxDecoration(
        gradient: R.headerGradient,
        borderRadius:
            BorderRadius.vertical(bottom: Radius.circular(R.rHeaderBottom)),
      ),
      clipBehavior: Clip.antiAlias,
      child: Stack(
        clipBehavior: Clip.none,
        children: [
          const PositionedDirectional(
            top: -40, end: -30, child: BrandWatermark(size: 200),
          ),
          Row(
            children: [
              // ⚠ شعارُ الشركة كما في ترويسة الوكيل — أمرُ المالك
              // (10 سبتمبر 2026) شمل التطبيقين. والموظفُ يقرأ هوية وكيله
              // من `device/employee/branding`، فهي التي تظهر له.
              //
              // وبلا حرفٍ هنا: ترويسةُ الموظف لم تكن تعرض حرفاً أصلاً،
              // فيبقى الاحتياطيُّ أيقونتَه المعتادة.
              const BrandAvatar(initial: null),
              const SizedBox(width: 12),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(profile.name,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style:
                            T.kufi(16, FontWeight.w700, color: Colors.white)),
                    const SizedBox(height: 4),
                    // نقطة البيع الفعّالة تُعرض دائماً: كل عملية تُسجَّل
                    // عليها، فيجب أن يعرف الموظف أين يعمل الآن.
                    Text(
                      profile.posName.isEmpty
                          ? company.displayName
                          : profile.posName,
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      style:
                          T.plex(11.5, FontWeight.w400, color: R.whiteA(.82)),
                    ),
                  ],
                ),
              ),
              ?trailing,
              // زرُّ أمان الجهاز — افتراضيّ لكل موظف، لا صلاحية: يختار به
              // حماية دخول جهازه (بصمة · نمط · بلا).
              IconButton(
                tooltip: 'الأمان',
                onPressed: () => Navigator.of(context, rootNavigator: true)
                    .push(MaterialPageRoute(
                        builder: (_) => const EmployeeSecurityScreen())),
                icon: const Icon(Icons.shield_outlined,
                    size: 22, color: Colors.white),
                constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
              ),
            ],
          ),
        ],
      ),
    );
  }
}

/// بلاطةُ مدخلٍ إلى شاشة — عنصرُ القوائم في تبويبات الموظف كلِّها.
///
/// ⚠ واحدةٌ لكل التبويبات: بلاطتان بمظهرين تجعلان القسمين يبدوان من
/// تطبيقين. وهي البلاطةُ نفسُها التي كانت في الشاشة الرئيسية — لم يتغيّر
/// شكلُها، تغيّر موضعُها.
class EmployeeTile extends StatelessWidget {
  const EmployeeTile({
    super.key,
    required this.icon,
    required this.title,
    required this.subtitle,
    required this.onTap,
  });

  final IconData icon;
  final String title;
  final String subtitle;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => GlassCard(
        onTap: onTap,
        child: Row(
          children: [
            IconTile(
              size: 38,
              background: R.primaryA(.12),
              icon: Icon(icon, size: 19, color: R.primaryDark),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(title, style: T.kufi(14.5, FontWeight.w700)),
                  const SizedBox(height: 3),
                  Text(subtitle,
                      style: T.plex(11.5, FontWeight.w400, color: R.inkA(.55))),
                ],
              ),
            ),
            Icon(Icons.chevron_left_rounded, size: 22, color: R.inkA(.4)),
          ],
        ),
      );
}

/// بابٌ موجودٌ ولم يُمنح — يُعرض مطفأً ومعه سببُ إطفائه.
///
/// ══════════════════════════════════════════════════════════════════════════
///  ⚠ لماذا يُعرض أصلاً بدل أن يُخفى
/// ══════════════════════════════════════════════════════════════════════════
///
/// بلاغُ المالك (11 سبتمبر 2026): «تبويب حوالات خارجية، أريد منك إنشاء حوالة
/// — لأنك نسيتَ تنفيذها في تطبيق الموظف».
///
/// **ولم تكن منسيّة**: البابُ مبنيٌّ ومساره في الخادم يعمل. لكنّ
/// `CREATE_EXTERNAL_TRANSFER` مفتاحٌ جديد يبدأ ممنوعاً على الجميع — ولم يُمنح
/// بعد — فكانت البلاطةُ **تُحذف من الشاشة بلا كلمة**. فقرأ المالكُ الصمتَ
/// «لم تُبنَ»، وهو أصدقُ ما يمكن أن يُقرأ من شاشةٍ لا تقول شيئاً.
///
/// ونصُّ أمر إعادة الهيكلة يمنع ذلك حرفياً: «ما لا يملكه الموظف لا يُعرض له،
/// **أو يُعرض ومعه سببُ غيابه بصراحة**». فالإخفاءُ الصامت خيارٌ صحيحٌ حين
/// تكون الميزةُ غريبةً عن عمل الموظف، وخاطئٌ حين يكون البابُ جزءاً معلوماً
/// من القسم الذي يقف فيه — عندها يبدو التطبيقُ ناقصاً لا مُقيَّداً.
///
/// ⚠ **ولا تُفتح بالنقر.** بابٌ يُفتح ثم يردّ 403 أسوأُ من بابٍ مغلق: الأوّل
/// عطبٌ في نظر المستخدم، والثاني قرارٌ مفهوم. فهي تقول ما ينقص وتقف.
class EmployeeLockedTile extends StatelessWidget {
  const EmployeeLockedTile({
    super.key,
    required this.icon,
    required this.title,
    required this.permissionLabel,
  });

  final IconData icon;
  final String title;

  /// اسمُ الصلاحية كما يراها الوكيل في شاشة المنح — كي يعرف ما يطلبه بالضبط.
  final String permissionLabel;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(14),
        decoration: BoxDecoration(
          color: R.whiteA(.42),
          border: Border.all(color: R.inkA(.09)),
          borderRadius: BorderRadius.circular(R.rCard),
        ),
        child: Row(
          children: [
            IconTile(
              size: 38,
              background: R.inkA(.06),
              icon: Icon(icon, size: 19, color: R.inkA(.38)),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(title,
                      style:
                          T.kufi(14.5, FontWeight.w700, color: R.inkA(.5))),
                  const SizedBox(height: 3),
                  Text(
                    'تحتاج صلاحية «$permissionLabel» — راجع وكيلك.',
                    style:
                        T.plex(11.5, FontWeight.w400, color: R.inkA(.45)),
                  ),
                ],
              ),
            ),
            Icon(Icons.lock_outline_rounded, size: 19, color: R.inkA(.3)),
          ],
        ),
      );
}

/// «لا شيء هنا» — رسالةٌ موحّدة لكل تبويبٍ فارغ أو ممنوع.
class EmployeeEmpty extends StatelessWidget {
  const EmployeeEmpty({super.key, required this.icon, required this.text});

  final IconData icon;
  final String text;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.fromLTRB(30, 50, 30, 30),
        child: Column(
          children: [
            Icon(icon, size: 42, color: R.inkA(.24)),
            const SizedBox(height: 14),
            Text(text,
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w500,
                    color: R.inkA(.5), height: 1.8)),
          ],
        ),
      );
}
