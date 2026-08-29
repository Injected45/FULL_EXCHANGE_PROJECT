import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
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

          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 0, R.padScreen, 0),
            child: snap.when(
              loading: () => const _LimitSkeleton(),
              error: (e, _) => _ErrorCard(message: '$e', onRetry: () => ref.invalidate(homeSnapshotProvider)),
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
                Text('آخر العمليات', style: T.section),
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
                                child: _MovementRow(m: s.movements[i]),
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
      decoration: const BoxDecoration(
        gradient: R.headerGradient,
        borderRadius: BorderRadius.vertical(bottom: Radius.circular(R.rHeaderBottom)),
      ),
      clipBehavior: Clip.antiAlias,
      child: Stack(
        clipBehavior: Clip.none,
        children: [
          PositionedDirectional(
            top: -40,
            end: -30,
            child: RhallaLogo(size: 220, color: R.whiteA(.09)),
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
                          const SizedBox(height: 8),
                          Text(role,
                              style: T.plex(11.5, FontWeight.w400,
                                  color: R.whiteA(.82))),
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
                            Text(
                              Fmt.money(balance ?? 0).split('.').first,
                              style: T.kufi(44, FontWeight.w800, color: Colors.white),
                            ),
                            const SizedBox(width: 8),
                            Text(
                              '.${Fmt.money(balance ?? 0).split('.').last}',
                              style: T.kufi(22, FontWeight.w600, color: R.whiteA(.82)),
                            ),
                            const SizedBox(width: 8),
                            Text(currency,
                                style: T.plex(13, FontWeight.w500,
                                    color: R.whiteA(.78))),
                          ],
                        ),
                      ),
                    if (accId != null) ...[
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
                      child: _Action(Icons.arrow_upward_rounded, 'داخلية',
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
                    const SizedBox(width: 6),
                    Expanded(
                      child: _Action(Icons.swap_horiz_rounded, 'بين الحسابات',
                          onTap: () => context.push('/send/accounts')),
                    ),
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
                  Text(currency,
                      style: T.plex(11, FontWeight.w400, color: R.inkA(.55))),
                  const Spacer(),
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

class _MovementRow extends StatelessWidget {
  const _MovementRow({required this.m});

  final Movement m;

  @override
  Widget build(BuildContext context) => GlassRow(
        children: [
          IconTile(
            icon: Icon(
              m.isCredit ? Icons.arrow_downward_rounded : Icons.arrow_upward_rounded,
              size: 17,
              color: R.primaryGradEnd,
            ),
          ),
          const SizedBox(width: 13),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(m.title.isEmpty ? 'حركة حساب' : m.title,
                    maxLines: 1, overflow: TextOverflow.ellipsis, style: T.name),
                const SizedBox(height: 7),
                Text(m.date, style: T.meta),
              ],
            ),
          ),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Text(
              Fmt.moneyWithSign(m.amount, credit: m.isCredit),
              style: T.kufi(14, FontWeight.w700,
                  color: m.isCredit ? R.credit : R.ink),
            ),
          ),
        ],
      );
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
