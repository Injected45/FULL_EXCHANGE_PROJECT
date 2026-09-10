import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../branding/brand_mark.dart';
import '../branding/branding_controller.dart';
import '../chat/chat_screen.dart';
import 'employee_security_screen.dart';
import 'employee_session.dart';

/// واجهة الموظف — **مبنيّة من صلاحياته وحدها**.
///
/// ما لم يمنحه الوكيل لا يظهر أصلاً: لا زرّ ولا بطاقة ولا سطر. والإخفاء
/// تجميل لا حماية — كل نداء يرفضه الخادم أيضاً بـ 403 ويُسجّله أمنياً.
///
/// والصلاحيات تُقرأ من `employee/me` عند كل فتح، فسحبُها من الإدارة يظهر
/// هنا عند أول تحديث. لا تُخزَّن في الرمز.
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

    final canSeeIncoming = p.can('VIEW_INCOMING_TRANSFERS');
    final canCreate      = p.can('CREATE_TRANSFER');

    /*
     * ══════════════════════════════════════════════════════════════════════
     *  بلاطاتُ الشاشة — كلُّ واحدةٍ خلف صلاحيتها
     * ══════════════════════════════════════════════════════════════════════
     *
     * تُبنى قائمةً هنا لا مباشرةً في الشجرة، لسببٍ واحد: **حتى يُعرَف هل
     * تُوجد بلاطةٌ واحدة أصلاً**. انظر التعليق عند عرضها.
     *
     * ⚠ والإخفاءُ تجميلٌ لا حماية: الخادم يردّ 403 على كل نداء ويُسجّله
     * أمنياً. لكنّ بلاطةً تُفتح فتُخفق تُعلّم الموظف أنّ التطبيق معطوب.
     *
     * ⚠ ولا فواصلَ داخل القائمة: تُوضع عند العرض بين كل اثنتين. وفاصلٌ
     * مشروطٌ داخل بلاطةٍ — كما كان في «إنشاء حوالة» — يترك فراغاً معلّقاً
     * حين تكون تاليتُه ممنوعة.
     */
    final tiles = <Widget>[
      /*
       * إنشاءُ حوالة — أوّلاً لأنه العملُ الذي يقف له الزبون.
       *
       * ⚠ ويفتح **شاشة الوكيل نفسَها** (`/send/internal`): أمرُ المالك أن
       * الموظف واجهةٌ من وكيل لا كيانٌ ثانٍ، فنموذجُ الحوالة واحدٌ للاثنين.
       * والمستودعُ وحده يبدّل المسار إلى نظيره تحت `device/employee/`.
       */
      if (canCreate)
        _Tile(
          icon: Icons.north_east_rounded,
          title: 'إنشاء حوالة',
          subtitle: 'حوالة محلية باسم الوكيل',
          onTap: () => context.push('/send/internal'),
        ),

      /*
       * ⚠ بابُ الحوالات — وهو **تبويب الوكيل نفسُه** بعين الموظف: واردةٌ
       * بتبويباتها الثلاثة وبحثها وفاتورتها وتسليمها، وصادرةٌ بشرائح مراحلها.
       * أمرُ المالك (10 سبتمبر 2026)، والشرحُ في رأس `TransfersScreen`.
       *
       * فالعنوان «الحوالات» لا «الحوالات الواردة»: صار فيها الاتجاهان.
       */
      if (canSeeIncoming)
        _Tile(
          icon: Icons.swap_horiz_rounded,
          title: 'الحوالات',
          subtitle: p.can('DELIVER_TRANSFER')
              ? 'الواردة والصادرة · اعرض وسجّل التسليم'
              : 'الواردة والصادرة · عرض فقط',
          onTap: () => context.push('/employee/transfers'),
        ),

      /*
       * ⚠ وموظفٌ يُنشئ الحوالات بلا صلاحية عرض الواردة: بابُه إلى «صادرتي»
       * يبقى مفتوحاً — الشاشةُ نفسُها، وتفتح على القسم الذي يملكه.
       *
       * وبدونها كان من يُنشئ الحوالات لا يجد أين يراها: القائمةُ خلف بلاطةٍ
       * تشترط صلاحيةً أخرى لا علاقة لها بعمله.
       */
      if (!canSeeIncoming && p.can('VIEW_OWN_TRANSFERS'))
        _Tile(
          icon: Icons.north_east_rounded,
          title: 'حوالاتي الصادرة',
          subtitle: 'ما أنشأتَه، بحالته في المنظومة',
          onTap: () => context.push('/employee/transfers'),
        ),

      /*
       * ⚠ **بلاطةُ التسليم وحدَه** — لمن مُنح التسليم بلا العرض.
       *
       * وهي حالةٌ يقع فيها الوكيل: يمنح «تسجيل تسليم حوالة» ظنّاً أنها
       * تكفي، فلا يرى الموظفُ شيئاً — لأن بلاطة الوارد تشترط العرض.
       * فتُعرض هنا بلاطةٌ تقول ما ينقصه بدل شاشةٍ فارغة.
       */
      if (!canSeeIncoming && p.can('DELIVER_TRANSFER'))
        _Tile(
          icon: Icons.call_received_rounded,
          title: 'الحوالات الواردة',
          subtitle: 'تحتاج صلاحية «عرض الحوالات الواردة» — راجع وكيلك',
          onTap: () => context.push('/employee/transfers'),
        ),

      // ⚠ «طلباتي» تظهر لمن يُنشئ الحوالات وحدَه: من لا يُنشئ لا طلباتِ له.
      // ونتيجةُ الطلب تُقرأ هنا لا تُسأل من الوكيل — والموظفُ واقفٌ أمام
      // زبونٍ ينتظر.
      if (canCreate)
        _Tile(
          icon: Icons.fact_check_outlined,
          title: 'طلباتي',
          subtitle: 'الحوالات التي تنتظر موافقة الوكيل',
          onTap: () => context.push('/employee/approvals'),
        ),

      if (p.can('VIEW_POS_TRANSFERS'))
        _Tile(
          icon: Icons.storefront_outlined,
          title: 'حوالات نقطة بيعي',
          subtitle: 'عملي على نقطة البيع الحالية',
          onTap: () => context.push('/employee/pos-transfers'),
        ),

      if (p.can('SEARCH_TRANSFER'))
        _Tile(
          icon: Icons.search_rounded,
          title: 'بحث برقم الحوالة',
          subtitle: 'ابحث عن حوالة بعينها',
          onTap: () => context.push('/employee/search'),
        ),

      // ⚠ «التقارير» بوّابةٌ لها مفتاحها، وكلُّ تقريرٍ داخلها له مفتاحُه —
      // فوكيلٌ يُري موظّفَه تقريراً واحداً دون سائرها يستطيع ذلك.
      if (p.can('REPORTS_VIEW'))
        _Tile(
          icon: Icons.insert_chart_outlined_rounded,
          title: 'التقارير',
          subtitle: 'حوالات اليوم والمسلَّمة والخزينة',
          onTap: () => context.push('/employee/reports'),
        ),

      /*
       * ⚠ العنوانُ والوصفُ يتبعان ما مُنح فعلاً.
       *
       * «رصيد الوكيل والملخّص اليومي» كان يُعرض لمن مُنح الملخّصَ وحدَه —
       * فيفتح البلاطة يبحث عن رصيدٍ لن يجده، ثم يسأل وكيله عنه. والبلاطةُ
       * لا تَعِد بما لا تُعطي.
       */
      if (p.can('VIEW_AGENT_TOTAL_BALANCE') || p.can('VIEW_FINANCIAL_SUMMARY'))
        _Tile(
          icon: Icons.account_balance_wallet_outlined,
          title: p.can('VIEW_AGENT_TOTAL_BALANCE')
              ? 'الأرصدة'
              : 'الملخّص المالي',
          subtitle: p.can('VIEW_AGENT_TOTAL_BALANCE')
              ? (p.can('VIEW_FINANCIAL_SUMMARY')
                  ? 'رصيد الوكيل والملخّص اليومي'
                  : 'رصيد الوكيل')
              : 'ملخّص يومك',
          onTap: () => context.push('/employee/balances'),
        ),

      if (p.can('VIEW_FAVORITES'))
        _Tile(
          icon: Icons.people_outline_rounded,
          title: 'المستفيدون',
          subtitle: p.can('MANAGE_FAVORITES')
              ? 'المفضّلة — عرضٌ وإدارة'
              : 'المفضّلة — عرض فقط',
          onTap: () => context.push('/employee/favorites'),
        ),

      // كشفُ حوالاته — تحت صلاحية عرض حوالاته نفسِها.
      if (p.can('VIEW_OWN_TRANSFERS'))
        _Tile(
          icon: Icons.receipt_long_outlined,
          title: 'كشف حوالاتي',
          subtitle: 'ما قبضتُ وما سلَّمتُ والصافي',
          onTap: () => context.push('/employee/statement'),
        ),


      // مراسلة الوكيل — صلاحيةٌ تُمنح كسائرها، فلا تظهر لمن لم يمنحه وكيلُه
      // إيّاها. والخادم يرفضها كذلك: إخفاء البطاقة تجميل، والحارس في الوسيط.
      if (p.can('CHAT_WITH_AGENT'))
        _Tile(
          icon: Icons.chat_bubble_outline_rounded,
          title: 'مراسلة الوكيل',
          subtitle: 'اسأل أو أبلغ عن أمرٍ في العمل',
          onTap: () => Navigator.of(context, rootNavigator: true).push(
            MaterialPageRoute(
              builder: (_) => const ChatScreen(
                title: 'الوكيل',
                asEmployee: true,
              ),
            ),
          ),
        ),
    ];

    /*
     * «بلا صلاحيات» حالةٌ حقيقية لا خطأ: الموظف يُنشأ فارغاً ثم يُمنح.
     *
     * ⚠ وتُقاس على **ما تعرضه الشاشة فعلاً** لا على قائمة مفاتيح: بلاطةٌ
     * تُضاف غداً تَعُدّ نفسَها بدل أن تُنسى في قائمةٍ ثانية.
     *
     * ⚠ وسقطت منها الورديةُ والعهدة (10 سبتمبر 2026): لم يعد لهما وجود —
     * أمرُ المالك أنّ التطبيق «حوالةٌ استلمها أو حوالةٌ سلّمها فقط».
     */
    final nothingGranted = tiles.isEmpty;

    return Screen(
      child: RefreshIndicator(
        // ⚠ العهدة تُبطَل مع السحبة: الموظف يسحب بعد أن سلّم حوالة ليرى
        // أثرها، ورقمٌ قديمٌ تحت اسمه بعد سحبةٍ صريحة أسوأ من لا رقم.
        onRefresh: () async {
          await ref.read(employeeAuthProvider.notifier).refresh();
        },
        color: R.primary,
        backgroundColor: Colors.white,
        child: ListView(
          padding: EdgeInsets.zero,
          physics: const AlwaysScrollableScrollPhysics(),
          children: [
            _Header(profile: p),

            Padding(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 18, R.padScreen, 40),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  if (nothingGranted) const _NoPermissions(),

                  /*
                   * ⚠ **«بلا صلاحيات» تُشتقّ من البلاطات نفسِها — لا من قائمة.**
                   *
                   * كانت تُحسب من خمسة مفاتيح مكتوبةً بأسمائها، والشاشةُ تعرض
                   * ثلاثَ عشرةَ بلاطة. فنتج عن ذلك خللان يخالفان القاعدة:
                   *
                   *   • موظفٌ مُنح «البحث برقم الحوالة» وحدَها — أو المفضّلة أو
                   *     الدردشة أو التقارير — كان يرى لافتة «لم تُمنح أي صلاحية»
                   *     **فوق بلاطةٍ تعمل**. فيظنّ الوكيل أن المنح أخفق.
                   *
                   *   • وموظفٌ مُنح «تسجيل التسليم» بلا «عرض الحوالات الواردة»
                   *     كان يرى شاشةً فارغة بلا لافتةٍ تشرح — لأن بلاطة الوارد
                   *     تشترط العرض.
                   *
                   * فالبلاطاتُ تُبنى قائمةً أولاً، ثم تُسأل: أفارغةٌ هي؟ وبذلك
                   * تُحسب بلاطةٌ تُضاف غداً وحدَها، ولا تُنسى في قائمةٍ ثانية.
                   */

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

class _Header extends ConsumerWidget {
  const _Header({required this.profile});

  final EmployeeProfile profile;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final top = MediaQuery.paddingOf(context).top;
    final company = ref.watch(brandingControllerProvider).branding;

    return Container(
      padding: EdgeInsets.fromLTRB(R.padScreen, top + 16, R.padScreen, 30),
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
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  // ⚠ شعارُ الشركة كما في ترويسة الوكيل — أمرُ المالك
                  // (10 سبتمبر 2026) شمل التطبيقين: «في الواجهة الرئيسية
                  // لتطبيق الوكيل والموظف». والموظفُ يقرأ هوية وكيله من
                  // `device/employee/branding`، فهي التي تظهر له.
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
                            style: T.kufi(16, FontWeight.w700,
                                color: Colors.white)),
                        const SizedBox(height: 4),
                        // نقطة البيع الفعّالة تُعرض دائماً: كل عملية تُسجَّل
                        // عليها، فيجب أن يعرف الموظف أين يعمل الآن.
                        Text(
                          profile.posName.isEmpty
                              ? company.displayName
                              : profile.posName,
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style: T.plex(11.5, FontWeight.w400,
                              color: R.whiteA(.82)),
                        ),
                      ],
                    ),
                  ),
                  // زرُّ أمان الجهاز — افتراضيّ لكل موظف، لا صلاحية: يختار به
                  // حماية دخول جهازه (بصمة · نمط · بلا).
                  IconButton(
                    tooltip: 'الأمان',
                    onPressed: () => Navigator.of(context, rootNavigator: true)
                        .push(MaterialPageRoute(
                            builder: (_) => const EmployeeSecurityScreen())),
                    icon: const Icon(Icons.shield_outlined,
                        size: 22, color: Colors.white),
                    constraints:
                        const BoxConstraints(minWidth: 44, minHeight: 44),
                  ),
                ],
              ),
              // ⚠ العهدةُ في الأعلى، ظاهرةً — أمرُ المالك (10 سبتمبر 2026):
              // «ليرى الموظف من الواجهة كم في حوزته». وكانت سطراً صغيراً تحت
            ],
          ),
        ],
      ),
    );
  }
}


class _Tile extends StatelessWidget {
  const _Tile({
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
                      style: T.plex(11.5, FontWeight.w400,
                          color: R.inkA(.55))),
                ],
              ),
            ),
            Icon(Icons.chevron_left_rounded, size: 22, color: R.inkA(.4)),
          ],
        ),
      );
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
            Center(child: Text('تسجيل الخروج', style: T.kufi(17, FontWeight.w700))),
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

