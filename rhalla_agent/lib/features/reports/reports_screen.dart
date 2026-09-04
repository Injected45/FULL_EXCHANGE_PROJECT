import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';

/// «التقارير» — تبويبٌ يجمع ما كان متفرّقاً في شاشة الحساب.
///
/// حلّ محلّ تبويب «الحوالات» (قرار المالك، 3 سبتمبر 2026): ذلك التبويب كان
/// يفتح «الحوالات الواردة» نفسها التي يفتحها زرّ «تسليم» في الرئيسية، فكان
/// أحدهما يكرّر الآخر ويأكل خانةً من أربع خانات في شريطٍ لا يتّسع لخامسة.
/// و«الحوالات الواردة» لم تُحذف — صارت تُفتح بالدفع من زرّ «تسليم».
///
/// وهذه الشاشة **دليلٌ لا تقرير**: لا تطلب من الخادم شيئاً ولا تحسب رقماً،
/// بل تفتح التقارير القائمة. فلا طلب شبكة جديد ولا حجم إضافي، والرقم يُعرض
/// في مكانٍ واحد لا مكانين يفترقان.
class ReportsScreen extends ConsumerWidget {
  const ReportsScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final user = ref.watch(authControllerProvider).user;

    return Screen(
      child: ListView(
        padding: const EdgeInsets.fromLTRB(0, 0, 0, 120),
        children: [
          const RhallaAppBar(title: 'التقارير'),
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 20, R.padScreen, 0),
            child: Column(
              children: [
                _Group(
                  title: 'حسابي',
                  children: [
                    _ReportRow(
                      icon: Icons.description_outlined,
                      label: 'كشف الحساب',
                      hint: 'كل حركة على حسابك بترتيبها',
                      onTap: () => context.push('/statement'),
                    ),
                    _ReportRow(
                      icon: Icons.speed_outlined,
                      label: 'السقوف والعمولات',
                      hint: 'ما تستطيع إرساله يومياً وعمولة كل شريحة',
                      onTap: () => context.push('/limits'),
                      last: true,
                    ),
                  ],
                ),
                const SizedBox(height: R.gapCard),
                // «الحوالات الواردة» ليست هنا (قرار المالك، 3 سبتمبر 2026):
                // لها زرّها الخاصّ في الشاشة الرئيسية، ومدخلان لشاشةٍ واحدة
                // تكرارٌ يوسّع الواجهة بلا أن يضيف إليها.
                //
                // متابعة الموظفين للحساب الرئيسي وحده — والخادم هو من يمنع
                // غيره (403)، لا إخفاء الصفّ.
                if (user?.isMainAgent == true)
                  _Group(
                    title: 'التشغيل',
                    children: [
                      _ReportRow(
                        icon: Icons.groups_2_outlined,
                        label: 'متابعة الموظفين ونقاط البيع',
                        hint: 'من سلّم · خزائن الموظفين · أداء كل نقطة',
                        onTap: () => context.push('/employees/reports'),
                        last: true,
                      ),
                    ],
                  ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

/// عنوانٌ فوق بطاقة — يفصل «حسابي» عن «التشغيل» بلا خطّ ولا إطار زائد.
class _Group extends StatelessWidget {
  const _Group({required this.title, required this.children});

  final String title;
  final List<Widget> children;

  @override
  Widget build(BuildContext context) => Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Padding(
            padding: const EdgeInsets.only(right: 6, bottom: 9),
            child: Text(title,
                style: T.plex(12.5, FontWeight.w600, color: R.inkA(.5))),
          ),
          GlassCard(
            padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 6),
            child: Column(children: children),
          ),
        ],
      );
}

/// صفّ تقرير — كصفوف شاشة الحساب، وتحته سطرٌ يقول ما يجده الوكيل بالداخل.
///
/// السطر التوضيحي ليس زينة: أسماء التقارير متقاربة، والوكيل الذي يدخل
/// تقريراً ليجد أنه ليس ما أراد يخرج ثم يجرّب غيره.
class _ReportRow extends StatelessWidget {
  const _ReportRow({
    required this.icon,
    required this.label,
    required this.hint,
    this.onTap,
    this.last = false,
  });

  final IconData icon;
  final String label;
  final String hint;
  final VoidCallback? onTap;
  final bool last;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onTap,
        child: Container(
          constraints: const BoxConstraints(minHeight: 44),
          padding: const EdgeInsets.symmetric(vertical: 13),
          decoration: last
              ? null
              : BoxDecoration(
                  border: Border(bottom: BorderSide(color: R.inkA(.07))),
                ),
          child: Row(
            children: [
              IconTile(icon: Icon(icon, size: 18, color: R.primaryGradEnd)),
              const SizedBox(width: 13),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(label, style: T.plex(13.5, FontWeight.w500)),
                    const SizedBox(height: 3),
                    Text(hint,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: T.plex(11.5, FontWeight.w400,
                            color: R.inkA(.45))),
                  ],
                ),
              ),
              const SizedBox(width: 8),
              Icon(Icons.chevron_left_rounded, size: 22, color: R.inkA(.4)),
            ],
          ),
        ),
      );
}
