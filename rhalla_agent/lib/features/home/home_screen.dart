import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/feature_flags.dart';
import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/glass.dart';
import '../branding/brand_mark.dart';
import '../auth/auth_controller.dart';
import '../transfers/agent_incoming_repository.dart';
import '../transfers/delivery_receipt_screen.dart';
import 'home_repository.dart';

class HomeScreen extends ConsumerWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final user = ref.watch(authControllerProvider).user;
    final snap = ref.watch(homeSnapshotProvider);

    return RefreshIndicator(
      onRefresh: () async => ref.refresh(homeSnapshotProvider.future),
      color: R.primary,
      backgroundColor: Colors.white,
      child: ListView(
        padding: EdgeInsets.zero,
        physics: const AlwaysScrollableScrollPhysics(),
        children: [
          _Header(
            name: user?.displayName ?? '',
            initial: user?.initial,
            currency: user?.currencyCode ?? 'د.ل',
            role: user?.isMainAgent == true ? 'وكيل رئيسي' : 'نقطة بيع',
            accId: user?.accId,
            balance: snap.valueOrNull?.balance,
            loading: snap.isLoading,
          ),

          Transform.translate(
            offset: const Offset(0, -28),
            child: const Padding(
              padding: EdgeInsets.symmetric(horizontal: R.padScreen),
              child: _ActionsCard(),
            ),
          ),

          if (kShowDailyLimit)
            Padding(
              padding: const EdgeInsets.fromLTRB(R.padScreen, 0, R.padScreen, 0),
              child: snap.when(
                loading: () => const _LimitSkeleton(),
                error: (e, _) => _ErrorCard(
                    message: '$e',
                    onRetry: () => ref.invalidate(homeSnapshotProvider)),
                data: (s) => _DailyLimit(
                  ceiling: s.limits.daily,
                  currency: user?.currencyCode ?? 'د.ل',
                ),
              ),
            ),

          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 22, R.padScreen, 120),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    Text('آخر العمليات', style: T.section),
                    const Spacer(),
                    // «عرض الكل» يفتح كشف الحساب — وهو الشاشة التي تعرض
                    // التاريخ كاملاً، فلا تكرار.
                    _SeeAllButton(onTap: () => context.push('/statement')),
                  ],
                ),
                const SizedBox(height: 12),
                snap.when(
                  loading: () => const _RowsSkeleton(),
                  error: (_, _) => const SizedBox.shrink(),
                  data: (s) => s.movements.isEmpty
                      ? const _EmptyMovements()
                      : Column(
                          children: [
                            for (var i = 0; i < s.movements.length; i++) ...[
                              if (i > 0) const SizedBox(height: R.gapRow),
                              RiseIn.small(
                                delay: Duration(milliseconds: 60 * i),
                                child: _MovementRow(
                                  m: s.movements[i],
                                  currency: user?.currencyCode ?? 'د.ل',
                                ),
                              ),
                            ],
                          ],
                        ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _Header extends StatelessWidget {
  const _Header({
    required this.name,
    required this.initial,
    required this.currency,
    required this.role,
    required this.accId,
    required this.balance,
    required this.loading,
  });

  final String name;
  final String? initial;
  final String currency;
  final String role;
  final int? accId;
  final double? balance;
  final bool loading;

  @override
  Widget build(BuildContext context) {
    final top = MediaQuery.paddingOf(context).top;

    return Container(
      padding: EdgeInsets.fromLTRB(R.padScreen, top + 16, R.padScreen, 44),
      decoration: BoxDecoration(
        gradient: R.headerGradient,
        borderRadius: BorderRadius.vertical(bottom: Radius.circular(R.rHeaderBottom)),
      ),
      clipBehavior: Clip.antiAlias,
      child: Stack(
        clipBehavior: Clip.none,
        children: [
          // علامة الشركة خلف الترويسة — شعارها إن رفعته، وإلا شعار الرحالة.
          // شفافيةٌ خفيفة على صورة الشركة أيضاً، وإلا صارت صورةً بارزة تنافس
          // بيانات الحساب فوقها بدل أن تكون خلفية.
          PositionedDirectional(
            top: -40,
            end: -30,
            child: const BrandWatermark(size: 220),
          ),
          Column(
            children: [
              RiseIn.small(
                child: Row(
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
                      // حرف من اسم حقيقي فقط — لا حرف من رقم هاتف.
                      child: initial == null
                          ? const Icon(Icons.person_outline_rounded,
                              size: 22, color: Colors.white)
                          : Text(
                              initial!,
                              style: T.kufi(16, FontWeight.w600, color: Colors.white),
                            ),
                    ),
                    const SizedBox(width: 12),
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(name,
                              maxLines: 1,
                              overflow: TextOverflow.ellipsis,
                              style: T.kufi(16, FontWeight.w600, color: Colors.white)),
                          if (kShowAgentRole) ...[
                            const SizedBox(height: 8),
                            Text(role,
                                style: T.plex(11.5, FontWeight.w400,
                                    color: R.whiteA(.82))),
                          ],
                        ],
                      ),
                    ),
                    Container(
                      width: 44,
                      height: 44,
                      alignment: Alignment.center,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        color: R.whiteA(.18),
                        border: Border.all(color: R.whiteA(.3)),
                      ),
                      child: const Icon(Icons.notifications_none_rounded,
                          size: 20, color: Colors.white),
                    ),
                  ],
                ),
              ),
              const SizedBox(height: 28),
              RiseIn(
                delay: const Duration(milliseconds: 120),
                child: Column(
                  children: [
                    Text('رصيد الوكالة',
                        style: T.plex(12, FontWeight.w400, color: R.whiteA(.82))),
                    const SizedBox(height: 13),
                    if (loading)
                      Container(
                        width: 180,
                        height: 40,
                        decoration: BoxDecoration(
                          color: R.whiteA(.18),
                          borderRadius: BorderRadius.circular(12),
                        ),
                      )
                    else
                      Directionality(
                        textDirection: TextDirection.ltr,
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.center,
                          crossAxisAlignment: CrossAxisAlignment.baseline,
                          textBaseline: TextBaseline.alphabetic,
                          children: [
                            // رمز العملة أولاً — قرار المالك: الرمز في أقصى
                            // اليسار ثم المبلغ إلى يمينه، في كل شاشة.
                            Text(currency,
                                style: T.plex(13, FontWeight.w500,
                                    color: R.whiteA(.78))),
                            const SizedBox(width: 8),
                            Text(
                              Fmt.money(balance ?? 0).split('.').first,
                              style: T.kufi(44, FontWeight.w800, color: Colors.white),
                            ),
                            const SizedBox(width: 8),
                            Text(
                              '.${Fmt.money(balance ?? 0).split('.').last}',
                              style: T.kufi(22, FontWeight.w600, color: R.whiteA(.82)),
                            ),
                          ],
                        ),
                      ),
                    if (kShowAccountBadge && accId != null) ...[
                      const SizedBox(height: 15),
                      Container(
                        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 7),
                        decoration: BoxDecoration(
                          color: R.whiteA(.18),
                          border: Border.all(color: R.whiteA(.3)),
                          borderRadius: BorderRadius.circular(99),
                        ),
                        child: Directionality(
                          textDirection: TextDirection.ltr,
                          child: Text('ACC $accId',
                              style: T.kufi(12, FontWeight.w600,
                                  color: Colors.white, spacing: .72)),
                        ),
                      ),
                    ],
                  ],
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}

class _ActionsCard extends StatelessWidget {
  const _ActionsCard();

  @override
  Widget build(BuildContext context) => RiseIn.small(
        delay: const Duration(milliseconds: 200),
        child: DecoratedBox(
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(R.rActions),
            boxShadow: R.shNav,
          ),
          child: ClipRRect(
            borderRadius: BorderRadius.circular(R.rActions),
            child: BackdropFilter(
              filter: ImageFilter.blur(sigmaX: R.blurGlassXl, sigmaY: R.blurGlassXl),
              child: Container(
                padding: const EdgeInsets.all(8),
                decoration: BoxDecoration(
                  color: R.whiteA(.8),
                  border: Border.all(color: R.whiteA(.92)),
                  borderRadius: BorderRadius.circular(R.rActions),
                ),
                child: Row(
                  children: [
                    Expanded(
                      child: _Action(Icons.arrow_upward_rounded, 'محلية',
                          primary: true,
                          onTap: () => context.push('/send/internal')),
                    ),
                    const SizedBox(width: 6),
                    Expanded(
                      child: _Action(Icons.public_rounded, 'خارجية',
                          onTap: () => context.push('/send/external')),
                    ),
                    const SizedBox(width: 6),
                    Expanded(
                      child: _Action(Icons.check_rounded, 'تسليم',
                          onTap: () => context.go('/transfers')),
                    ),
                    if (kShowAccountsTransfer) ...[
                      const SizedBox(width: 6),
                      Expanded(
                        child: _Action(Icons.swap_horiz_rounded, 'بين الحسابات',
                            onTap: () => context.push('/send/accounts')),
                      ),
                    ],
                  ],
                ),
              ),
            ),
          ),
        ),
      );
}

class _Action extends StatelessWidget {
  const _Action(this.icon, this.label, {this.primary = false, this.onTap});

  final IconData icon;
  final String label;
  final bool primary;
  final VoidCallback? onTap;

  @override
  Widget build(BuildContext context) => Material(
        color: primary ? Colors.transparent : R.primaryA(.13),
        borderRadius: BorderRadius.circular(19),
        child: Ink(
          height: 74,
          decoration: BoxDecoration(
            gradient: primary ? R.primaryGradient : null,
            borderRadius: BorderRadius.circular(19),
          ),
          child: InkWell(
            borderRadius: BorderRadius.circular(19),
            onTap: onTap,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(icon, size: 19, color: primary ? Colors.white : R.primaryGradEnd),
                const SizedBox(height: 8),
                Text(
                  label,
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: T.kufi(12, FontWeight.w600,
                      color: primary ? Colors.white : R.primaryDark),
                ),
              ],
            ),
          ),
        ),
      );
}

class _DailyLimit extends StatelessWidget {
  const _DailyLimit({required this.ceiling, required this.currency});

  /// السقف اليومي المسموح — لا المستهلك.
  ///
  /// جدول Daily_transfer_preparer_schedule_DEttelse يعيد سقوفاً
  /// (Daily / Weekly / monthly / Annual)، ولا يعيد المستهلك إطلاقاً.
  /// تسميته «المحوّل اليوم» تُضلّل الوكيل عن المبلغ المتاح له.
  final double ceiling;
  final String currency;

  @override
  Widget build(BuildContext context) {
    return RiseIn.small(
      delay: const Duration(milliseconds: 280),
      child: GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Text('السقف اليومي', style: T.label),
                const Spacer(),
                Text('التفاصيل',
                    style: T.plex(11.5, FontWeight.w500, color: R.primaryGradEnd)),
              ],
            ),
            const SizedBox(height: 12),
            Directionality(
              textDirection: TextDirection.ltr,
              child: Row(
                crossAxisAlignment: CrossAxisAlignment.baseline,
                textBaseline: TextBaseline.alphabetic,
                children: [
                  // الرمز ملاصق للرقم لا في الطرف المقابل، والاثنان يمينَ
                  // البطاقة كما في التصميم.
                  const Spacer(),
                  Text(currency,
                      style: T.plex(11, FontWeight.w400, color: R.inkA(.55))),
                  const SizedBox(width: 6),
                  Text(Fmt.money(ceiling), style: T.kufi(19, FontWeight.w700)),
                ],
              ),
            ),
            const SizedBox(height: 10),
            // لا شريط تقدّم: الخادم لا يعطي المستهلك، ورسم نسبة بلا بسط كذب.
            Text('المستهلك اليوم غير متاح من الخادم بعد', style: T.meta),
          ],
        ),
      ),
    );
  }
}

class _MovementRow extends ConsumerStatefulWidget {
  const _MovementRow({required this.m, required this.currency});

  final Movement m;
  final String currency;

  @override
  ConsumerState<_MovementRow> createState() => _MovementRowState();
}

class _MovementRowState extends ConsumerState<_MovementRow> {
  bool _opening = false;

  Movement get m => widget.m;
  String get currency => widget.currency;

  /// يفتح فاتورة الحوالة — الشاشة نفسها التي تفتحها قائمة «الحوالات
  /// الواردة»، لا نسخةً ثانية عنها.
  ///
  /// الحركة تحمل رقم الحوالة لا بياناتها، فتُجلب بالرقم. وما لا يُوجد —
  /// حوالة صادرة من حساب الوكيل مثلاً — لا فاتورة استلام له، ويُقال ذلك
  /// صراحةً بدل فتح شاشة فارغة.
  ///
  /// والفاتورة تُفتح كاملةً بزرّ التسليم (قرار المالك، 3 سبتمبر 2026): حوالةٌ
  /// «بانتظار التسليم» تُسلَّم من هنا كما تُسلَّم من «الحوالات الواردة»،
  /// و«تم التسليم» تُعرض بلا زرّ. الحالة هي التي تحكم لا الشاشة — والقرار
  /// كلّه في مكان واحد داخل الفاتورة.
  Future<void> _open() async {
    if (_opening || !m.isTransfer) return;
    setState(() => _opening = true);

    try {
      final t = await ref
          .read(agentIncomingRepositoryProvider)
          .findByCode(m.code);
      if (!mounted) return;

      if (t == null) {
        _say('لا توجد فاتورة استلام لهذه الحركة — الحوالة ليست واردة إليك.');
        return;
      }
      await Navigator.of(context, rootNavigator: true).push(
        MaterialPageRoute(
          builder: (_) => DeliveryReceiptScreen(transfer: t),
        ),
      );

      // تسليمٌ سُجِّل في الفاتورة يجب أن يظهر في الصفّ الذي فُتحت منه، وإلا
      // بقي الوسم «بانتظار التسليم» أمام وكيلٍ سلّم للتوّ — فيسلّم مرّتين.
      // الإبطال هنا لا في الفاتورة: الاتجاه features/home ← features/transfers
      // وليس العكس.
      if (mounted) ref.invalidate(homeSnapshotProvider);
    } catch (_) {
      if (mounted) _say('تعذّر فتح الحوالة — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _opening = false);
    }
  }

  void _say(String msg) => ScaffoldMessenger.of(context)
    ..hideCurrentSnackBar()
    ..showSnackBar(SnackBar(
      content:
          Text(msg, style: T.plex(13, FontWeight.w500, color: Colors.white)),
      backgroundColor: R.inkA(.9),
      behavior: SnackBarBehavior.floating,
      margin: const EdgeInsets.fromLTRB(16, 0, 16, 100),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
    ));

  /// وسم الحالة — من **دفتر تسليم الوكيل**، لا من حالة المنظومة.
  ///
  /// قرار المالك (2 سبتمبر 2026): ما يظهر هنا هو ما تقوله شاشة «الحوالات
  /// الواردة» — «بانتظار التسليم» / «تم التسليم» / «ملغاة» — لا حالة الحوالة
  /// بين الوكيل والرحالة.
  ///
  /// السببان مختلفان تماماً: «مسلمه» في المنظومة تعني أن الحوالة وصلت إلى
  /// الوكيل، بينما «تم التسليم» تعني أنه دفع المال للمستفيد. عرضُ الأولى
  /// مكانَ الثانية يجعل الوكيل يقرأ أنه سلّم مالاً لم يسلّمه — وهو أخطر خلطٍ
  /// ممكن في هذه الشاشة.
  ///
  /// وحركةٌ خارج ذلك الدفتر — حوالة صادرة، أو عمولة — لا حالة تسليم لها،
  /// فتُوسم باتجاهها كما كانت.
  String get _badge {
    if (m.isCommission) return 'عمولة';
    if (!m.isTransfer) return m.isCredit ? 'إيداع' : 'خصم';

    final badge = m.agentBadge;
    if (badge.isNotEmpty) return badge;

    return m.isCredit ? 'واردة' : 'صادرة';
  }

  /// الملغاة حمراء دائماً ولو كانت الحركة واردة.
  ///
  /// لونُ الاتجاه وحده يجعل حوالةً ملغاة تبدو خضراء عاديّة، والوكيل قد يدفع
  /// مالها. اللون هنا تحذير، لا زينة.
  RowTone get _tone {
    if (m.isTransfer && m.agentBadge == 'ملغاة') return RowTone.debit;
    return m.isCredit ? RowTone.credit : RowTone.debit;
  }

  @override
  Widget build(BuildContext context) {
    final tone = _tone;
    // العمولة ليست حركة اتجاه — لها رمزها الخاص كما في التصميم.
    final isFee = m.isCommission;

    return GlassRow(
      dense: true,
      tone: tone,
      onTap: m.isTransfer ? _open : null,
      children: [
        // الأيقونة وتحتها وسمُ الحالة — كتلة واحدة كما في التصميم.
        SizedBox(
          // 74 لا 54: «بانتظار التسليم» أطول من «مسلمه»، والوسم لا يُقصّ.
          width: 74,
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              IconTile(
                size: 32,
                background: tone.tile,
                icon: Icon(
                  isFee
                      ? Icons.savings_outlined
                      : m.isCredit
                          ? Icons.arrow_downward_rounded
                          : Icons.arrow_upward_rounded,
                  size: 15,
                  color: tone.ink,
                ),
              ),
              const SizedBox(height: 4),
              // سطران بلا قصّ: حذف كلمةٍ من حالة الحوالة ممنوع، وسطرٌ ثانٍ
              // أهون من «بانتظار الت…».
              Text(_badge,
                  maxLines: 2,
                  textAlign: TextAlign.center,
                  style: T.plex(9, FontWeight.w600,
                      color: tone.ink, height: 1.35)),
            ],
          ),
        ),
        const SizedBox(width: 11),
        Expanded(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(m.title.isEmpty ? 'حركة حساب' : Fmt.localName(m.title),
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: T.plex(12.5, FontWeight.w600, color: tone.ink)),
              if (m.isTransfer) ...[
                const SizedBox(height: 2),
                // الرقم كتلة LTR: رقمٌ داخل جملة عربية ينقلب ترتيبه.
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text('${m.code}  :رقم الحوالة',
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      textAlign: TextAlign.right,
                      style:
                          T.plex(10, FontWeight.w400, color: R.inkA(.55))),
                ),
              ],
              const SizedBox(height: 3),
              // التاريخ يبقى محايداً: تلوينه يُذهب التدرّج ويجعل الصفّ صاخباً.
              Row(
                children: [
                  Icon(Icons.calendar_today_rounded,
                      size: 10, color: R.inkA(.45)),
                  const SizedBox(width: 5),
                  Flexible(
                    child: Text(
                      Fmt.stampShort(m.time.isNotEmpty ? m.time : m.date),
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      style:
                          T.plex(10.5, FontWeight.w400, color: R.inkA(.5)),
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
        const SizedBox(width: 8),
        // المبلغ وتحته رمز العملة — والرمز يأتي من الخادم لا مكتوباً هنا.
        Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.end,
          children: [
            Directionality(
              textDirection: TextDirection.ltr,
              child: Text(
                Fmt.moneyWithSign(m.amount, credit: m.isCredit),
                style: T.kufi(13.5, FontWeight.w700, color: tone.ink),
              ),
            ),
            const SizedBox(height: 1),
            Text(currency,
                style: T.plex(9.5, FontWeight.w400, color: tone.ink)),
          ],
        ),
        // السهم يَعِد بفتح شيء، فلا يظهر إلا حيث يوجد ما يُفتح.
        if (m.isTransfer) ...[
          const SizedBox(width: 4),
          _opening
              ? SizedBox(
                  width: 16,
                  height: 16,
                  child: CircularProgressIndicator(
                      strokeWidth: 2,
                      valueColor: AlwaysStoppedAnimation(tone.ink)),
                )
              : Icon(Icons.expand_more_rounded,
                  size: 18, color: R.inkA(.4)),
        ],
      ],
    );
  }
}

class _EmptyMovements extends StatelessWidget {
  const _EmptyMovements();

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.symmetric(vertical: 40),
        child: Column(
          children: [
            RhallaLogo(size: 56, color: R.primaryA(.3)),
            const SizedBox(height: 18),
            Text('لا توجد حركة على الحساب بعد',
                style: T.kufi(15, FontWeight.w600, height: 1.5)),
          ],
        ),
      );
}

class _LimitSkeleton extends StatelessWidget {
  const _LimitSkeleton();

  @override
  Widget build(BuildContext context) => GlassCard(
        child: SizedBox(
          height: 52,
          child: Align(
            alignment: AlignmentDirectional.centerStart,
            child: Container(
              width: 140,
              height: 14,
              decoration: BoxDecoration(
                color: R.inkA(.08),
                borderRadius: BorderRadius.circular(9),
              ),
            ),
          ),
        ),
      );
}

class _RowsSkeleton extends StatelessWidget {
  const _RowsSkeleton();

  @override
  Widget build(BuildContext context) => Column(
        children: [
          for (var i = 0; i < 3; i++) ...[
            if (i > 0) const SizedBox(height: R.gapRow),
            GlassRow(children: [
              Container(
                width: 40,
                height: 40,
                decoration: BoxDecoration(
                  color: R.inkA(.06),
                  borderRadius: BorderRadius.circular(R.rTile),
                ),
              ),
              const SizedBox(width: 13),
              Expanded(
                child: Container(
                  height: 12,
                  decoration: BoxDecoration(
                    color: R.inkA(.06),
                    borderRadius: BorderRadius.circular(9),
                  ),
                ),
              ),
            ]),
          ],
        ],
      );
}

class _ErrorCard extends StatelessWidget {
  const _ErrorCard({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(Icons.error_outline_rounded, size: 18, color: R.error),
                const SizedBox(width: 10),
                Expanded(
                  child: Text(message,
                      style: T.plex(12, FontWeight.w500,
                          color: R.errorText, height: 1.5)),
                ),
              ],
            ),
            const SizedBox(height: 12),
            TextButton(
              onPressed: onRetry,
              style: TextButton.styleFrom(minimumSize: const Size(44, 44)),
              child: Text('إعادة المحاولة',
                  style: T.plex(12.5, FontWeight.w600, color: R.primaryGradEnd)),
            ),
          ],
        ),
      );
}

/// «عرض الكل ›» بجانب عنوان آخر العمليات.
///
/// السهم إلى اليسار — جهة المتابعة في واجهة عربية.
class _SeeAllButton extends StatelessWidget {
  const _SeeAllButton({required this.onTap});

  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => Material(
        color: R.primaryA(.08),
        borderRadius: BorderRadius.circular(R.rPill),
        child: InkWell(
          onTap: onTap,
          borderRadius: BorderRadius.circular(R.rPill),
          child: Padding(
            // 36 ارتفاعاً بالحشوة — أقلّ من 44 لأنه اختصارٌ مكرّر: الشاشة
            // نفسها في شريط التنقّل السفلي بهدف إصابة كامل.
            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Text('عرض الكل',
                    style: T.plex(11.5, FontWeight.w600, color: R.primaryDark)),
                const SizedBox(width: 4),
                Icon(Icons.chevron_left_rounded, size: 16, color: R.primaryDark),
              ],
            ),
          ),
        ),
      );
}
