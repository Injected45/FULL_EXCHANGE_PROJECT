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
import '../auth/auth_controller.dart';
import '../home/home_repository.dart';

/// عمولة مُحصّلة.
/// أعمدة CommtionRetview_get كما رُصدت:
/// commion · InsertDate · AccIDFrom · AccBranchID · BName · STATUESSTRING
class Commission {
  const Commission({
    required this.amount,
    required this.date,
    required this.branchName,
    required this.status,
  });

  final double amount;
  final String date;
  final String branchName;
  final String status;

  factory Commission.fromJson(Map<String, dynamic> j) => Commission(
        // بهذا الإملاء — commion لا commission.
        amount: Fmt.num_(j['commion']),
        date: '${j['InsertDate'] ?? ''}'.trim(),
        branchName: '${j['BName'] ?? ''}'.trim(),
        status: '${j['STATUESSTRING'] ?? ''}'.trim(),
      );
}

/// مزوّدان منفصلان عمداً.
///
/// السقوف ترجع في أجزاء من الثانية، بينما CommtionRetview_get يعيد
/// **كل** العمولات بلا ترقيم (رُصد 1385 صفاً · ~8 ثوانٍ). انتظارهما معاً
/// يُجمّد الشاشة ثماني ثوانٍ على لا شيء.
final ceilingsProvider = FutureProvider.autoDispose<Limits>((ref) async {
  final api = ref.watch(apiClientProvider);
  try {
    final env = await api.post('/device/Daily_transfer', body: {});
    final row = env.row;
    return row == null ? const Limits() : Limits.fromJson(row);
  } on ApiFailure catch (e) {
    if (e.isEmptyResult) return const Limits();
    rethrow;
  }
});

final commissionsProvider =
    FutureProvider.autoDispose<List<Commission>>((ref) async {
  final api = ref.watch(apiClientProvider);
  try {
    final env = await api.post('/device/internal/CommtionRetview_get');
    return env.rows.map(Commission.fromJson).toList();
  } on ApiFailure catch (e) {
    if (e.isEmptyResult) return const [];
    rethrow;
  }
});

class LimitsScreen extends ConsumerWidget {
  const LimitsScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final user = ref.watch(authControllerProvider).user;
    final ceilings = ref.watch(ceilingsProvider);
    final commissions = ref.watch(commissionsProvider);
    final currency = user?.currencyCode ?? 'د.ل';

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'السقوف والعمولات',
            subtitle: user == null ? null : 'على حساب الوكالة ACC ${user.accId}',
            onBack: () => context.pop(),
          ),
          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 22, R.padScreen, 40),
              children: [
                Text('سقوف التحويل', style: T.section),
                const SizedBox(height: 12),
                ceilings.when(
                  loading: () => const _CeilingSkeleton(),
                  error: (e, _) => _Failed(
                    message: '$e',
                    onRetry: () => ref.invalidate(ceilingsProvider),
                  ),
                  data: (l) => Column(
                    children: [
                      _Ceiling(label: 'اليومي', value: l.daily, currency: currency),
                      const SizedBox(height: 10),
                      _Ceiling(label: 'الأسبوعي', value: l.weekly, currency: currency),
                      const SizedBox(height: 10),
                      _Ceiling(label: 'الشهري', value: l.monthly, currency: currency),
                      const SizedBox(height: 10),
                      _Ceiling(label: 'السنوي', value: l.annual, currency: currency),
                    ],
                  ),
                ),
                const SizedBox(height: 14),
                Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Icon(Icons.info_outline, size: 15, color: R.inkA(.45)),
                    const SizedBox(width: 9),
                    Expanded(
                      child: Text(
                        'هذه حدود مسموح بها، لا مبالغ محوّلة. '
                        'الخادم لا يوفّر المستهلك من كل سقف بعد.',
                        style: T.plex(11.5, FontWeight.w400,
                            color: R.inkA(.55), height: 1.6),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 26),
                Text('عمولاتي', style: T.section),
                const SizedBox(height: 12),
                commissions.when(
                  loading: () => const _CommissionsSkeleton(),
                  error: (e, _) => _Failed(
                    message: '$e',
                    onRetry: () => ref.invalidate(commissionsProvider),
                  ),
                  data: (all) {
                    final monthly = _thisMonth(all);
                    return Column(
                      children: [
                        _CommissionsCard(
                          total: monthly.fold<double>(0, (s, c) => s + c.amount),
                          count: monthly.length,
                          currency: currency,
                        ),
                        const SizedBox(height: 12),
                        if (all.isEmpty)
                          Padding(
                            padding: const EdgeInsets.symmetric(vertical: 30),
                            child: Text('لا عمولات مسجّلة بعد',
                                style: T.kufi(15, FontWeight.w600)),
                          )
                        else
                          for (var i = 0; i < all.take(15).length; i++) ...[
                            if (i > 0) const SizedBox(height: R.gapRow),
                            _CommissionRow(c: all[i]),
                          ],
                      ],
                    );
                  },
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  /// الخادم لا يجمع، فنجمع محلياً على شهر التاريخ الأحدث في القائمة.
  List<Commission> _thisMonth(List<Commission> all) {
    if (all.isEmpty) return const [];
    final newest = all.first.date;
    if (newest.length < 7) return all;
    final prefix = newest.substring(0, 7); // YYYY-MM
    return all.where((c) => c.date.startsWith(prefix)).toList();
  }
}

class _CeilingSkeleton extends StatelessWidget {
  const _CeilingSkeleton();

  @override
  Widget build(BuildContext context) => Column(
        children: [
          for (var i = 0; i < 4; i++) ...[
            if (i > 0) const SizedBox(height: 10),
            GlassCard(
              padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 16),
              child: SizedBox(
                height: 16,
                child: Align(
                  alignment: AlignmentDirectional.centerStart,
                  child: Container(
                    width: 90,
                    height: 12,
                    decoration: BoxDecoration(
                      color: R.inkA(.07),
                      borderRadius: BorderRadius.circular(9),
                    ),
                  ),
                ),
              ),
            ),
          ],
        ],
      );
}

class _CommissionsSkeleton extends StatelessWidget {
  const _CommissionsSkeleton();

  @override
  Widget build(BuildContext context) => Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Container(
            height: 148,
            decoration: BoxDecoration(
              gradient: R.primaryGradient,
              borderRadius: BorderRadius.circular(R.rActions),
              boxShadow: R.shNav,
            ),
            child: const Center(
              child: SizedBox(
                width: 24,
                height: 24,
                child: CircularProgressIndicator(
                  strokeWidth: 2.4,
                  valueColor: AlwaysStoppedAnimation(Colors.white),
                ),
              ),
            ),
          ),
          const SizedBox(height: 12),
          Text('العمولات تُجلب كاملة من الخادم — قد تستغرق ثوانٍ.',
              textAlign: TextAlign.center,
              style: T.plex(11.5, FontWeight.w400, color: R.inkA(.55))),
        ],
      );
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(message,
                style: T.plex(12.5, FontWeight.w500,
                    color: R.errorText, height: 1.6)),
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

class _Ceiling extends StatelessWidget {
  const _Ceiling({
    required this.label,
    required this.value,
    required this.currency,
  });

  final String label;
  final double value;
  final String currency;

  @override
  Widget build(BuildContext context) => GlassCard(
        padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 16),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.baseline,
          textBaseline: TextBaseline.alphabetic,
          children: [
            Text(label, style: T.kufi(13, FontWeight.w600)),
            const Spacer(),
            Directionality(
              textDirection: TextDirection.ltr,
              child: Text(Fmt.money(value), style: T.kufi(15, FontWeight.w700)),
            ),
            const SizedBox(width: 6),
            Text(currency, style: T.meta),
          ],
        ),
      );
}

class _CommissionsCard extends StatelessWidget {
  const _CommissionsCard({
    required this.total,
    required this.count,
    required this.currency,
  });

  final double total;
  final int count;
  final String currency;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(22, 20, 22, 20),
        clipBehavior: Clip.antiAlias,
        decoration: BoxDecoration(
          gradient: R.primaryGradient,
          borderRadius: BorderRadius.circular(R.rActions),
          boxShadow: R.shNav,
        ),
        child: Stack(
          clipBehavior: Clip.none,
          children: [
            PositionedDirectional(
              top: -46,
              end: -36,
              child: RhallaLogo(size: 180, color: R.whiteA(.09)),
            ),
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text('إجمالي عمولات آخر شهر',
                    style: T.plex(11.5, FontWeight.w400, color: R.whiteA(.82))),
                const SizedBox(height: 13),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Row(
                    crossAxisAlignment: CrossAxisAlignment.baseline,
                    textBaseline: TextBaseline.alphabetic,
                    children: [
                      const Spacer(),
                      Text(currency,
                          style: T.plex(13, FontWeight.w500,
                              color: R.whiteA(.78))),
                      const SizedBox(width: 8),
                      Text(Fmt.money(total),
                          style: T.kufi(32, FontWeight.w800, color: Colors.white)),
                    ],
                  ),
                ),
                const SizedBox(height: 16),
                Text('${Fmt.count(count)} عملية',
                    style: T.plex(11, FontWeight.w400, color: R.whiteA(.78))),
              ],
            ),
          ],
        ),
      );
}

class _CommissionRow extends StatelessWidget {
  const _CommissionRow({required this.c});

  final Commission c;

  @override
  Widget build(BuildContext context) => GlassRow(
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(c.branchName.isEmpty ? 'عمولة تحويل' : c.branchName,
                    maxLines: 1, overflow: TextOverflow.ellipsis, style: T.name),
                const SizedBox(height: 7),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Align(
                    alignment: AlignmentDirectional.centerStart,
                    child: Text(c.date, style: T.meta),
                  ),
                ),
              ],
            ),
          ),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Text('+ ${Fmt.money(c.amount)}',
                style: T.kufi(14, FontWeight.w700, color: R.credit)),
          ),
        ],
      );
}
