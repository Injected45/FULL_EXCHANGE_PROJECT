import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'employee_header.dart';
import 'employee_session.dart';

/// التبويبُ الأوّل — **الحوالات**.
///
/// ══════════════════════════════════════════════════════════════════════════
///  خمسةُ مداخل، كما نصّ الأمر حرفاً بحرف
/// ══════════════════════════════════════════════════════════════════════════
///
/// ```
/// الحوالات
/// ├── حوالات محلية      ← إنشاء · حوالاتي · كشف حوالاتي
/// ├── حوالات خارجية     ← إنشاء · حوالاتي · كشف الحوالات
/// ├── طلباتي
/// ├── بحث برقم حوالة
/// └── نقطة بيعي
/// ```
///
/// ⚠ **هذه هي الشاشةُ الرئيسية القديمة، مُعادَ توزيعُها لا مُعادَ بناؤها.**
/// البلاطاتُ الثلاثَ عشرةَ التي كانت هنا لم تُحذف: ثلاثٌ منها صارت تبويباتٍ
/// قائمةً بذاتها (الخزينة · المراسلة · المستفيدون)، واثنتان انتقلتا إلى
/// التقارير، والباقي تحت هذا التبويب في موضعه من الشجرة أعلاه.
///
/// ⚠ **والخروجُ بقي حيث كان** — أسفلَ هذا التبويب: هو الموضعُ الذي تعلّمه
/// الموظف، ونقلُه إلى تبويبٍ آخر يجعله يبحث عنه.
///
/// ⚠ وكلُّ مدخلٍ خلف صلاحيته، والإخفاءُ تجميلٌ لا حماية: الخادمُ يردّ 403
/// على كل نداء ويُسجّله. لكنّ باباً يُفتح فيُخفق يُعلّم الموظف أن التطبيق
/// معطوب لا أنه غير مصرَّح.
class EmployeeHomeScreen extends ConsumerStatefulWidget {
  const EmployeeHomeScreen({super.key});

  @override
  ConsumerState<EmployeeHomeScreen> createState() => _EmployeeHomeScreenState();
}

class _EmployeeHomeScreenState extends ConsumerState<EmployeeHomeScreen> {
  @override
  Widget build(BuildContext context) {
    final state = ref.watch(employeeAuthProvider);
    final p = state.profile;

    if (p == null) {
      return Screen(
        child: Center(child: CircularProgressIndicator(color: R.primary)),
      );
    }

    final canCreate   = p.can('CREATE_TRANSFER');
    final canOwn      = p.can('VIEW_OWN_TRANSFERS');
    final canIncoming = p.can('VIEW_INCOMING_TRANSFERS');
    final canDeliver  = p.can('DELIVER_TRANSFER');
    final canExternal = p.can('CREATE_EXTERNAL_TRANSFER');

    /*
     * ⚠ تُبنى قائمةً لا مباشرةً في الشجرة، حتى يُعرف **هل يوجد مدخلٌ واحد
     * أصلاً**. ومدخلٌ يُضاف غداً يَعُدّ نفسَه بدل أن يُنسى في قائمةٍ ثانية —
     * وهو العيبُ الذي كان في هذه الشاشة قبلاً وكشفه المالك.
     */
    final tiles = <Widget>[
      /*
       * الحوالةُ المحلّية أوّلاً: هي العملُ الذي يقف له الزبون. وتُفتح لمن
       * يملك أيَّ بابٍ تحتها — إنشاءً أو عرضاً أو تسليماً.
       */
      if (canCreate || canOwn || canIncoming || canDeliver)
        EmployeeTile(
          icon: Icons.swap_horiz_rounded,
          title: 'حوالات محلية',
          subtitle: 'إنشاء حوالة · حوالاتي · كشف حوالاتي',
          onTap: () => context.push('/employee/local'),
        ),

      /*
       * ⚠ الخارجيةُ تُفتح لمن يُنشئها **أو** لمن يرى حوالاته: موظفٌ سُحبت
       * منه صلاحيةُ الإنشاء يبقى مسؤولاً عن خارجيةٍ أنشأها بالأمس ويجب أن
       * يجدها. وما لا يملكه من الاثنين لا يُعرض له داخلها.
       */
      if (canExternal || canOwn)
        EmployeeTile(
          icon: Icons.public_rounded,
          title: 'حوالات خارجية',
          // ⚠ الثلاثةُ تُذكر دائماً: القسمُ يعرضها كلَّها الآن — ما مُنح
          // يُفتح، وما لم يُمنح يقول ما ينقصه. وسطرٌ يُخفي «إنشاء حوالة»
          // كان جزءاً من الصمت الذي قُرئ «لم تُبنَ».
          subtitle: 'إنشاء حوالة · حوالاتي · كشف الحوالات',
          onTap: () => context.push('/employee/external'),
        ),

      // ⚠ «طلباتي» لمن يُنشئ الحوالات وحدَه: من لا يُنشئ لا طلباتِ له.
      // ونتيجةُ الطلب تُقرأ هنا لا تُسأل من الوكيل — والموظفُ واقفٌ أمام
      // زبونٍ ينتظر.
      if (canCreate)
        EmployeeTile(
          icon: Icons.fact_check_outlined,
          title: 'طلباتي',
          subtitle: 'الحوالات التي تنتظر موافقة الوكيل',
          onTap: () => context.push('/employee/approvals'),
        ),

      if (p.can('SEARCH_TRANSFER'))
        EmployeeTile(
          icon: Icons.search_rounded,
          title: 'بحث برقم حوالة',
          subtitle: 'وارداً كان أو صادراً — والضغطةُ تفتح الفاتورة',
          onTap: () => context.push('/employee/search'),
        ),

      if (p.can('VIEW_POS_TRANSFERS'))
        EmployeeTile(
          icon: Icons.storefront_outlined,
          title: 'نقطة بيعي',
          subtitle: 'عملي على نقطة البيع الحالية',
          onTap: () => context.push('/employee/pos-transfers'),
        ),
    ];

    return Screen(
      child: RefreshIndicator(
        onRefresh: () async {
          await ref.read(employeeAuthProvider.notifier).refresh();
        },
        color: R.primary,
        backgroundColor: Colors.white,
        child: ListView(
          padding: EdgeInsets.zero,
          physics: const AlwaysScrollableScrollPhysics(),
          children: [
            EmployeeHeader(profile: p),

            Padding(
              // ⚠ 110 أسفلَ القائمة: الشريطُ السفليّ يعلو المحتوى، وبدونها
              // يبقى آخرُ عنصرٍ تحته فلا يُنقر.
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 18, R.padScreen, 110),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  if (tiles.isEmpty) const _NoPermissions(),

                  for (var i = 0; i < tiles.length; i++) ...[
                    if (i > 0) const SizedBox(height: R.gapRow),
                    tiles[i],
                  ],

                  const SizedBox(height: 24),
                  GlassButton(
                    label: 'تسجيل الخروج',
                    onPressed: () => _confirmSignOut(context),
                  ),
                  const SizedBox(height: 12),
                  Text(
                    'الخروج يُنهي التفعيل — ستحتاج كوداً جديداً من الإدارة للعودة.',
                    textAlign: TextAlign.center,
                    style: T.plex(11.5, FontWeight.w400,
                        color: R.inkA(.5), height: 1.7),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Future<void> _confirmSignOut(BuildContext context) async {
    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => const _ConfirmSignOut(),
    );
    if (ok != true || !mounted) return;
    await ref.read(employeeAuthProvider.notifier).signOut();
  }
}

class _NoPermissions extends StatelessWidget {
  const _NoPermissions();

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.symmetric(vertical: 40),
        child: Column(
          children: [
            Icon(Icons.lock_outline_rounded, size: 40, color: R.primaryA(.3)),
            const SizedBox(height: 16),
            Text('لم تُمنح صلاحيات بعد',
                style: T.kufi(15, FontWeight.w600, color: R.inkA(.6))),
            const SizedBox(height: 8),
            Text('راجع الوكيل ليمنحك ما تحتاجه من صلاحيات، '
                'ثم اسحب الشاشة لأسفل للتحديث.',
                textAlign: TextAlign.center,
                style: T.plex(12.5, FontWeight.w400,
                    color: R.inkA(.45), height: 1.8)),
          ],
        ),
      );
}

class _ConfirmSignOut extends StatelessWidget {
  const _ConfirmSignOut();

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(22, 22, 22, 26),
        decoration: BoxDecoration(
          color: R.whiteA(.94),
          borderRadius:
              const BorderRadius.vertical(top: Radius.circular(R.rNav)),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Center(
              child: Container(
                width: 44,
                height: 4,
                decoration: BoxDecoration(
                  color: R.inkA(.16),
                  borderRadius: BorderRadius.circular(99),
                ),
              ),
            ),
            const SizedBox(height: 20),
            Center(
                child:
                    Text('تسجيل الخروج', style: T.kufi(17, FontWeight.w700))),
            const SizedBox(height: 10),
            Text(
              'الخروج يُنهي تفعيل هذا الجهاز. للعودة ستحتاج كوداً جديداً من '
              'الإدارة ورمز تحقّق جديد.',
              textAlign: TextAlign.center,
              style:
                  T.plex(13, FontWeight.w500, color: R.inkA(.65), height: 1.7),
            ),
            const SizedBox(height: 20),
            PrimaryButton(
              label: 'تسجيل الخروج',
              onPressed: () => Navigator.of(context).pop(true),
            ),
            const SizedBox(height: 10),
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
              child: Text('إلغاء',
                  style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
            ),
          ],
        ),
      );
}
