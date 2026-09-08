import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../branding/brand_mark.dart';
import '../branding/branding_controller.dart';
import '../chat/chat_screen.dart';
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
    final canCashbox     = p.can('VIEW_OWN_CASHBOX');
    final canStartShift  = p.can('START_SHIFT');
    final canCloseShift  = p.can('CLOSE_SHIFT');
    final hasShift       = p.openShift != null;

    // «بلا صلاحيات» حالةٌ حقيقية لا خطأ: الموظف يُنشأ فارغاً ثم يُمنح.
    final nothingGranted = !canSeeIncoming &&
        !canCreate &&
        !canCashbox &&
        !canStartShift &&
        !canCloseShift;

    return Screen(
      child: RefreshIndicator(
        // ⚠ العهدة تُبطَل مع السحبة: الموظف يسحب بعد أن سلّم حوالة ليرى
        // أثرها، ورقمٌ قديمٌ تحت اسمه بعد سحبةٍ صريحة أسوأ من لا رقم.
        onRefresh: () async {
          ref.invalidate(custodyProvider);
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
                  if (hasShift) ...[
                    _ShiftCard(
                      shift: p.openShift!,
                      canClose: canCloseShift,
                      onClose: () => context.push('/employee/shift/close'),
                    ),
                    const SizedBox(height: R.gapCard),
                  ] else if (canStartShift) ...[
                    _StartShiftCard(
                      onTap: () => context.push('/employee/shift/start'),
                    ),
                    const SizedBox(height: R.gapCard),
                  ],

                  if (nothingGranted) const _NoPermissions(),

                  /*
                   * إنشاءُ حوالة — أوّلاً لأنه العملُ الذي يقف له الزبون.
                   *
                   * ⚠ ويفتح **شاشة الوكيل نفسَها** (`/send/internal`): أمرُ
                   * المالك أن الموظف واجهةٌ من وكيل لا كيانٌ ثانٍ، فنموذجُ
                   * الحوالة واحدٌ للاثنين. والمستودعُ وحده يبدّل المسار إلى
                   * نظيره تحت `device/employee/`.
                   */
                  if (canCreate) ...[
                    _Tile(
                      icon: Icons.north_east_rounded,
                      title: 'إنشاء حوالة',
                      subtitle: 'حوالة محلية باسم الوكيل',
                      onTap: () => context.push('/send/internal'),
                    ),
                    if (canSeeIncoming) const SizedBox(height: R.gapRow),
                  ],

                  if (canSeeIncoming)
                    _Tile(
                      icon: Icons.call_received_rounded,
                      title: 'الحوالات الواردة',
                      subtitle: p.can('DELIVER_TRANSFER')
                          ? 'اعرض وسجّل التسليم'
                          : 'عرض فقط',
                      onTap: () => context.push('/employee/transfers'),
                    ),

                  // ⚠ «طلباتي» تظهر لمن يُنشئ الحوالات وحدَه: من لا
                  // يُنشئ لا طلباتِ له. ونتيجةُ الطلب تُقرأ هنا لا تُسأل
                  // من الوكيل — والموظفُ واقفٌ أمام زبونٍ ينتظر.
                  if (canCreate) ...[
                    const SizedBox(height: R.gapRow),
                    _Tile(
                      icon: Icons.fact_check_outlined,
                      title: 'طلباتي',
                      subtitle: 'الحوالات التي تنتظر موافقة الوكيل',
                      onTap: () => context.push('/employee/approvals'),
                    ),
                  ],

                  /*
                   * ⚠ كلُّ بلاطةٍ خلف صلاحيتها — ولا تظهر لمن لم
                   * يُمنحها. والإخفاءُ تجميل: الخادم يردّ 403 على كلّ
                   * نداء. لكنّ بلاطةً تُفتح فتُخفق تُعلّم الموظف أنّ
                   * التطبيق معطوب، فلا تُعرض أصلاً.
                   */
                  if (p.can('VIEW_POS_TRANSFERS')) ...[
                    const SizedBox(height: R.gapRow),
                    _Tile(
                      icon: Icons.storefront_outlined,
                      title: 'حوالات نقطة بيعي',
                      subtitle: 'عملي على نقطة البيع الحالية',
                      onTap: () => context.push('/employee/pos-transfers'),
                    ),
                  ],

                  if (p.can('SEARCH_TRANSFER')) ...[
                    const SizedBox(height: R.gapRow),
                    _Tile(
                      icon: Icons.search_rounded,
                      title: 'بحث برقم الحوالة',
                      subtitle: 'ابحث عن حوالة بعينها',
                      onTap: () => context.push('/employee/search'),
                    ),
                  ],

                  // ⚠ «التقارير» بوّابةٌ لها مفتاحها، وكلُّ تقريرٍ
                  // داخلها له مفتاحُه — فوكيلٌ يُري موظّفَه تقريراً
                  // واحداً دون سائرها يستطيع ذلك.
                  if (p.can('REPORTS_VIEW')) ...[
                    const SizedBox(height: R.gapRow),
                    _Tile(
                      icon: Icons.insert_chart_outlined_rounded,
                      title: 'التقارير',
                      subtitle: 'حوالات اليوم والمسلَّمة والخزينة',
                      onTap: () => context.push('/employee/reports'),
                    ),
                  ],

                  if (p.can('VIEW_AGENT_TOTAL_BALANCE') ||
                      p.can('VIEW_FINANCIAL_SUMMARY')) ...[
                    const SizedBox(height: R.gapRow),
                    _Tile(
                      icon: Icons.account_balance_wallet_outlined,
                      title: 'الأرصدة',
                      subtitle: 'رصيد الوكيل والملخّص اليومي',
                      onTap: () => context.push('/employee/balances'),
                    ),
                  ],

                  if (p.can('VIEW_FAVORITES')) ...[
                    const SizedBox(height: R.gapRow),
                    _Tile(
                      icon: Icons.people_outline_rounded,
                      title: 'المستفيدون',
                      subtitle: p.can('MANAGE_FAVORITES')
                          ? 'المفضّلة — عرضٌ وإدارة'
                          : 'المفضّلة — عرض فقط',
                      onTap: () => context.push('/employee/favorites'),
                    ),
                  ],

                  // كشفُ حوالاته — تحت صلاحية عرض حوالاته نفسِها.
                  if (p.can('VIEW_OWN_TRANSFERS')) ...[
                    const SizedBox(height: R.gapRow),
                    _Tile(
                      icon: Icons.receipt_long_outlined,
                      title: 'كشف حوالاتي',
                      subtitle: 'ما قبضتُ وما سلَّمتُ والصافي',
                      onTap: () => context.push('/employee/statement'),
                    ),
                  ],

                  if (canCashbox) ...[
                    const SizedBox(height: R.gapRow),
                    _Tile(
                      icon: Icons.savings_outlined,
                      title: 'خزينتي',
                      subtitle: hasShift
                          ? 'حركات الوردية والنقد المتوقّع'
                          : 'ابدأ وردية لتسجيل الحركات',
                      onTap: () => context.push('/employee/cashbox'),
                    ),
                  ],

                  // مراسلة الوكيل — صلاحيةٌ تُمنح كسائرها، فلا تظهر لمن لم
                  // يمنحه وكيلُه إيّاها. والخادم يرفضها كذلك: إخفاء البطاقة
                  // تجميل، والحارس في الوسيط.
                  if (p.can('CHAT_WITH_AGENT')) ...[
                    const SizedBox(height: R.gapRow),
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
                  Container(
                    width: 46,
                    height: 46,
                    alignment: Alignment.center,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: R.whiteA(.2),
                      border: Border.all(color: R.whiteA(.34)),
                    ),
                    child: Icon(Icons.person_outline_rounded,
                        size: 22, color: Colors.white),
                  ),
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
                        if (profile.can('VIEW_OWN_CASHBOX'))
                          const _CustodyLine(),
                      ],
                    ),
                  ),
                ],
              ),
            ],
          ),
        ],
      ),
    );
  }
}

class _ShiftCard extends StatelessWidget {
  const _ShiftCard({
    required this.shift,
    required this.canClose,
    required this.onClose,
  });

  final OpenShift shift;
  final bool canClose;
  final VoidCallback onClose;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Row(
              children: [
                IconTile(
                  size: 38,
                  background: R.primaryA(.12),
                  icon: Icon(Icons.play_circle_outline_rounded,
                      size: 19, color: R.primaryDark),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('وردية مفتوحة',
                          style: T.kufi(14.5, FontWeight.w700)),
                      const SizedBox(height: 3),
                      Text('بدأت · ${Fmt.stampShort(shift.startedAt)}',
                          style: T.plex(11.5, FontWeight.w400,
                              color: R.inkA(.55))),
                    ],
                  ),
                ),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Text('د.ل ',
                          style: T.plex(11, FontWeight.w500,
                              color: R.inkA(.5))),
                      Text(Fmt.money(shift.openingCash),
                          style: T.kufi(14, FontWeight.w700,
                              color: R.primaryDark)),
                    ],
                  ),
                ),
              ],
            ),
            if (canClose) ...[
              const SizedBox(height: 12),
              PrimaryButton(label: 'إقفال الوردية', onPressed: onClose),
            ],
          ],
        ),
      );
}

class _StartShiftCard extends StatelessWidget {
  const _StartShiftCard({required this.onTap});

  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => GlassCard(
        onTap: onTap,
        child: Row(
          children: [
            IconTile(
              size: 38,
              background: R.primaryA(.12),
              icon: Icon(Icons.play_arrow_rounded,
                  size: 20, color: R.primaryDark),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text('ابدأ وردية', style: T.kufi(14.5, FontWeight.w700)),
                  const SizedBox(height: 3),
                  Text('أعلن النقد الذي في يدك الآن',
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

/* ═══════════════════ خزينتي ═══════════════════ */

/// «خزينتي» — الحركات والمعادلة.
///
/// المعادلة تُعرض كاملةً لا نتيجتها وحدها: الموظف يرى من أين جاء الرقم،
/// فلا يفاجئه «المتوقّع» عند الإقفال.
class EmployeeCashboxScreen extends ConsumerStatefulWidget {
  const EmployeeCashboxScreen({super.key});

  @override
  ConsumerState<EmployeeCashboxScreen> createState() =>
      _EmployeeCashboxScreenState();
}

class _EmployeeCashboxScreenState extends ConsumerState<EmployeeCashboxScreen> {
  Map<String, dynamic>? _data;
  bool _loading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    setState(() { _loading = true; _error = null; });
    try {
      final env = await ref.read(apiClientProvider).get('/device/employee/cashbox');
      if (!mounted) return;
      setState(() { _data = env.row ?? const {}; _loading = false; });
    } on ApiFailure catch (e) {
      if (mounted) setState(() { _error = e.message; _loading = false; });
    } catch (_) {
      if (mounted) {
        setState(() { _error = 'تعذّر الاتصال بالخادم.'; _loading = false; });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final summary = (_data?['summary'] as Map?)?.cast<String, dynamic>();
    final entries = ((_data?['entries'] as List?) ?? const [])
        .whereType<Map>()
        .map((e) => e.cast<String, dynamic>())
        .toList();
    final canEntry = ref.watch(employeeAuthProvider).profile?.can('CASHBOX_ENTRY') ?? false;

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'خزينتي', onBack: () => context.pop()),
          Expanded(
            child: _loading
                ? Center(child: CircularProgressIndicator(color: R.primary))
                : _error != null
                    ? _Failed(message: _error!, onRetry: _load)
                    : RefreshIndicator(
                        onRefresh: _load,
                        color: R.primary,
                        backgroundColor: Colors.white,
                        child: ListView(
                          padding: const EdgeInsets.fromLTRB(
                              R.padScreen, 14, R.padScreen, 40),
                          physics: const AlwaysScrollableScrollPhysics(),
                          children: [
                            if (summary == null)
                              const _NoShift()
                            else ...[
                              _SummaryCard(summary: summary),
                              if (canEntry) ...[
                                const SizedBox(height: R.gapCard),
                                Row(
                                  children: [
                                    Expanded(
                                      child: GlassButton(
                                        label: 'نقد وارد',
                                        onPressed: () => _entry('IN'),
                                      ),
                                    ),
                                    const SizedBox(width: 10),
                                    Expanded(
                                      child: GlassButton(
                                        label: 'نقد مسلَّم',
                                        onPressed: () => _entry('OUT'),
                                      ),
                                    ),
                                  ],
                                ),
                              ],
                              const SizedBox(height: 18),
                              Text('حركات الوردية', style: T.section),
                              const SizedBox(height: 10),
                              if (entries.isEmpty)
                                Text('لا حركات بعد.',
                                    style: T.plex(12.5, FontWeight.w400,
                                        color: R.inkA(.5)))
                              else
                                for (final e in entries) ...[
                                  _EntryRow(entry: e),
                                  const SizedBox(height: R.gapRow),
                                ],
                            ],
                          ],
                        ),
                      ),
          ),
        ],
      ),
    );
  }

  Future<void> _entry(String direction) async {
    final amount = await showModalBottomSheet<double>(
      context: context,
      useRootNavigator: true,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _AmountSheet(
        title: direction == 'IN' ? 'نقد وارد' : 'نقد مسلَّم',
      ),
    );
    if (amount == null || !mounted) return;

    try {
      await ref.read(apiClientProvider).post(
        '/device/employee/cashbox/entry',
        body: {
          'amount': amount,
          'direction': direction,
          // مرجعٌ فريد للطلب: نقرتان أو إعادة إرسال لا تُنشئان حركتين.
          'client_ref': 'e-${DateTime.now().microsecondsSinceEpoch}',
        },
      );
      await _load();
    } on ApiFailure catch (e) {
      if (mounted) _say(e.message);
    } catch (_) {
      if (mounted) _say('تعذّر تسجيل الحركة.');
    }
  }

  void _say(String m) {
    ScaffoldMessenger.of(context)
      ..hideCurrentSnackBar()
      ..showSnackBar(SnackBar(
        content:
            Text(m, style: T.plex(13, FontWeight.w500, color: Colors.white)),
        backgroundColor: R.inkA(.92),
        behavior: SnackBarBehavior.floating,
      ));
  }
}

class _SummaryCard extends StatelessWidget {
  const _SummaryCard({required this.summary});

  final Map<String, dynamic> summary;

  double _n(String k) => double.tryParse('${summary[k] ?? 0}') ?? 0;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            _row('الرصيد الافتتاحي', _n('opening')),
            const SizedBox(height: 8),
            _row('إجمالي النقد المستلم', _n('in'), tone: R.primaryDark),
            const SizedBox(height: 8),
            _row('إجمالي النقد المسلَّم', _n('out'), tone: R.error),
            const SizedBox(height: 10),
            Divider(color: R.inkA(.08), height: 1),
            const SizedBox(height: 10),
            _row('النقد المتوقّع لديك', _n('expected'), strong: true),
          ],
        ),
      );

  Widget _row(String label, double value, {Color? tone, bool strong = false}) =>
      Row(
        children: [
          Expanded(
            child: Text(label,
                style: strong
                    ? T.kufi(14, FontWeight.w700)
                    : T.plex(12.5, FontWeight.w400, color: R.inkA(.6))),
          ),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Text('د.ل ',
                    style: T.plex(11, FontWeight.w500, color: R.inkA(.5))),
                Text(Fmt.money(value),
                    style: strong
                        ? T.kufi(17, FontWeight.w800, color: R.primaryDark)
                        : T.kufi(13.5, FontWeight.w600,
                            color: tone ?? R.ink)),
              ],
            ),
          ),
        ],
      );
}

class _EntryRow extends StatelessWidget {
  const _EntryRow({required this.entry});

  final Map<String, dynamic> entry;

  @override
  Widget build(BuildContext context) {
    final isIn = '${entry['direction']}' == 'IN';
    final reversed = entry['is_reversed'] == 1 || entry['is_reversed'] == true;
    final amount = double.tryParse('${entry['amount'] ?? 0}') ?? 0;
    final tone = isIn ? R.primaryDark : R.error;

    final label = switch ('${entry['transaction_type']}') {
      'TRANSFER_DELIVERY' => 'تسليم حوالة',
      'CASH_RECEIVED'     => 'نقد وارد',
      'CASH_HANDOVER'     => 'نقد مسلَّم',
      'REVERSAL'          => 'عكس حركة',
      'ADJUSTMENT'        => 'تسوية',
      _                   => 'حركة',
    };

    return GlassCard(
      child: Row(
        children: [
          IconTile(
            size: 32,
            background: tone.withValues(alpha: .12),
            icon: Icon(
                isIn ? Icons.arrow_downward_rounded : Icons.arrow_upward_rounded,
                size: 15, color: tone),
          ),
          const SizedBox(width: 11),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(label,
                    style: T.plex(12.5, FontWeight.w600,
                        color: reversed ? R.inkA(.45) : tone)),
                if ('${entry['reference_id'] ?? ''}'.isNotEmpty) ...[
                  const SizedBox(height: 2),
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Text('${entry['reference_id']}',
                        style: T.plex(10, FontWeight.w400,
                            color: R.inkA(.5))),
                  ),
                ],
                if (reversed) ...[
                  const SizedBox(height: 2),
                  Text('مُلغاة بحركة عكسية',
                      style: T.plex(10, FontWeight.w500, color: R.warnIcon)),
                ],
              ],
            ),
          ),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Text(
              Fmt.moneyWithSign(amount, credit: isIn),
              style: T.kufi(13, FontWeight.w700,
                  color: reversed ? R.inkA(.4) : tone),
            ),
          ),
        ],
      ),
    );
  }
}

class _AmountSheet extends StatefulWidget {
  const _AmountSheet({required this.title});

  final String title;

  @override
  State<_AmountSheet> createState() => _AmountSheetState();
}

class _AmountSheetState extends State<_AmountSheet> {
  final _amount = TextEditingController();
  late final _focus = AutoClearFocus(_amount, formatOnExit: true);
  String? _error;

  @override
  void dispose() {
    _focus.dispose();
    _amount.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => Padding(
        padding: EdgeInsets.only(bottom: MediaQuery.viewInsetsOf(context).bottom),
        child: Container(
          padding: const EdgeInsets.fromLTRB(20, 14, 20, 24),
          decoration: BoxDecoration(
            color: R.whiteA(.96),
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
              const SizedBox(height: 16),
              Center(child: Text(widget.title, style: T.kufi(17, FontWeight.w700))),
              const SizedBox(height: 16),
              GlassCard(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text('المبلغ', style: T.label),
                    const SizedBox(height: 9),
                    Directionality(
                      textDirection: TextDirection.ltr,
                      child: TextField(
                        controller: _amount,
                        focusNode: _focus,
                        keyboardType: const TextInputType.numberWithOptions(
                            decimal: true),
                        inputFormatters: moneyInputFormatters,
                        style: T.kufi(18, FontWeight.w700),
                        decoration: InputDecoration(
                          isDense: true,
                          border: InputBorder.none,
                          prefixText: 'د.ل  ',
                          prefixStyle: T.plex(13, FontWeight.w500,
                              color: R.inkA(.5)),
                          hintText: '0.00',
                          hintStyle: T.plex(15, FontWeight.w400,
                              color: R.inkA(.35)),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              if (_error != null) ...[
                const SizedBox(height: 12),
                Text(_error!,
                    textAlign: TextAlign.center,
                    style: T.plex(12.5, FontWeight.w500, color: R.errorText)),
              ],
              const SizedBox(height: 18),
              PrimaryButton(label: 'تسجيل', onPressed: _submit),
              const SizedBox(height: 8),
              TextButton(
                onPressed: () => Navigator.of(context).pop(),
                style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
                child: Text('إلغاء',
                    style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
              ),
            ],
          ),
        ),
      );

  void _submit() {
    // `Fmt.num_` تزيل فواصل الآلاف — قراءة `text` مباشرةً تُرسل رقماً آخر.
    final v = Fmt.num_(_amount.text);
    if (v <= 0) {
      setState(() => _error = 'اكتب مبلغاً أكبر من صفر.');
      return;
    }
    Navigator.of(context).pop(v);
  }
}

class _NoShift extends StatelessWidget {
  const _NoShift();

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.symmetric(vertical: 50),
        child: Column(
          children: [
            Icon(Icons.schedule_rounded, size: 40, color: R.primaryA(.3)),
            const SizedBox(height: 16),
            Text('لا توجد وردية مفتوحة',
                style: T.kufi(15, FontWeight.w600, color: R.inkA(.6))),
            const SizedBox(height: 8),
            Text('ابدأ وردية من الشاشة الرئيسية لتسجيل حركات الخزينة.',
                textAlign: TextAlign.center,
                style: T.plex(12.5, FontWeight.w400,
                    color: R.inkA(.45), height: 1.8)),
          ],
        ),
      );
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.all(R.padScreen),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(Icons.error_outline_rounded, size: 38, color: R.error),
            const SizedBox(height: 14),
            Text(message,
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w500,
                    color: R.inkA(.65), height: 1.7)),
            const SizedBox(height: 18),
            GlassButton(label: 'إعادة المحاولة', onPressed: onRetry),
          ],
        ),
      );
}

/* ══════════════════════════════════════════════════════════════════════════
   عهدة الموظف — سطرٌ تحت اسمه في الترويسة
   ══════════════════════════════════════════════════════════════════════════

   ⚠ **النقد في درج الموظف ليس ملكَه — هو عهدةٌ عند الوكيل.** فما يظهر
   التزامٌ عليه لا رصيدٌ له، والصياغة تقولها: «في عهدتك» لا «رصيدك».

       المتوقَّع = الافتتاحيّ + الداخل − الخارج

   ⚠ **والإشارة تقلب المعنى، فيتغيّر النصّ معها لا الرقمُ وحدَه**:

     • موجب ⇦ نقدٌ في يده يسلّمه للوكيل — عليه.
     • سالب ⇦ دفع أكثر ممّا قبض — له على الوكيل. وهي حالةٌ واقعية:
       موظفٌ بدأ ورديّته بلا نقد ثمّ سلّم حوالاتٍ من ماله.

   وعرض السالب برقمٍ ناقص وحدَه يجعل الموظف يقرأ عجزاً في عهدته وهو دائنٌ
   لوكيله — وهذا عكس الحقيقة تماماً.

   ⚠ **ولا يُعرض شيءٌ بلا وردية.** وصفرٌ حينئذٍ خطأ: «لا شيء عليك» غيرُ
   «لم تبدأ بعد»، والفرق بينهما يومُ عملٍ كامل.
   ══════════════════════════════════════════════════════════════════════════ */

class _CustodyLine extends ConsumerWidget {
  const _CustodyLine();

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(custodyProvider);

    return async.maybeWhen(
      data: (c) {
        if (!c.hasShift) return const SizedBox.shrink();

        final owed = c.expected >= 0;

        return Padding(
          padding: const EdgeInsets.only(top: 6),
          child: Row(
            children: [
              Icon(
                owed
                    ? Icons.account_balance_wallet_rounded
                    : Icons.south_west_rounded,
                size: 13,
                color: R.whiteA(.9),
              ),
              const SizedBox(width: 5),
              Text(owed ? 'في عهدتك · ' : 'مستحقٌّ لك · ',
                  style: T.plex(11.5, FontWeight.w600, color: R.whiteA(.9))),
              // رقمٌ لاتينيّ الاتجاه كسائر مبالغ التطبيق، والرمز عن يساره،
              // والقيمة مطلقة لأن الإشارة قيلت بالنصّ.
              Directionality(
                textDirection: TextDirection.ltr,
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Text('${c.currency} ',
                        style: T.plex(10.5, FontWeight.w500,
                            color: R.whiteA(.82))),
                    Text(Fmt.money(c.expected.abs()),
                        style: T.kufi(12.5, FontWeight.w800,
                            color: Colors.white)),
                  ],
                ),
              ),
            ],
          ),
        );
      },
      // لا هيكلَ تحميلٍ ولا رسالةَ خطأ: سطرٌ ثانويّ تحت الاسم، ووميضُه في كل
      // فتحةٍ يزاحم ما فوقه. يظهر حين يصل الرقم، ويصمت حين لا يصل.
      orElse: () => const SizedBox.shrink(),
    );
  }
}

/// عهدة الموظف كما يحسبها الخادم — لا يُحسب هنا شيء.
class Custody {
  const Custody({
    required this.hasShift,
    required this.opening,
    required this.inAmount,
    required this.outAmount,
    required this.expected,
    required this.currency,
  });

  final bool hasShift;
  final double opening;
  final double inAmount;
  final double outAmount;

  /// ⚠ قد تكون سالبة — انظر التعليق أعلاه.
  final double expected;

  final String currency;

  static Custody fromJson(Map<String, dynamic> j) => Custody(
        hasShift: j['has_shift'] == true,
        opening: Fmt.num_(j['opening']),
        inAmount: Fmt.num_(j['in']),
        outAmount: Fmt.num_(j['out']),
        expected: Fmt.num_(j['expected']),
        currency: (j['currencyCode'] ?? 'د.ل').toString(),
      );
}

/// ⚠ مسارٌ خفيف مستقلٌّ عن شاشة الخزينة: يُسأل مع كل فتحةٍ للشاشة الرئيسية،
/// وجلبُ قائمة الحركات كلِّها من أجل رقمٍ واحد إسرافٌ يتكرّر كل مرة.
final custodyProvider = FutureProvider.autoDispose<Custody>((ref) async {
  final env = await ref.watch(apiClientProvider).get('/device/employee/custody');
  return Custody.fromJson(env.row ?? const {});
});
