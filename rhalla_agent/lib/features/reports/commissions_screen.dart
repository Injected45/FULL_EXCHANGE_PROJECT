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

/// «عمولاتي» — كانت ضمن شاشة «السقوف والعمولات»، فُصلت إلى التقارير (قرار
/// المالك): السقوفُ إعدادٌ للوكالة، والعمولاتُ تقريرٌ ماليّ — موضعُها التقارير.
///
/// المحتوى كما هو حرفياً: البطاقةُ الإجمالية وصفوفُ العمولات، بلا نقصٍ.

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

/// ⚠ CommtionRetview_get يعيد **كل** العمولات بلا ترقيم (رُصد 1385 صفاً ·
/// ~8 ثوانٍ). لذلك شاشةٌ مستقلّة لها لا تُجمَّد بها شاشةٌ أخرى.
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

class CommissionsScreen extends ConsumerWidget {
  const CommissionsScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final user = ref.watch(authControllerProvider).user;
    final commissions = ref.watch(commissionsProvider);
    final currency = user?.currencyCode ?? 'د.ل';

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'عمولاتي',
            subtitle: user == null ? null : 'على حساب الوكالة ACC ${user.accId}',
            onBack: () => context.pop(),
          ),
          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 22, R.padScreen, 40),
              children: [
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
