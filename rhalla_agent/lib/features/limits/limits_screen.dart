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

/// «سقوف الرحالة» — حدود التحويل التي تمنحها الرحالة للوكالة. «عمولاتي» فُصلت
/// إلى شاشةٍ مستقلّة في التقارير (قرار المالك): السقفُ إعدادٌ، والعمولةُ تقرير.

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

class LimitsScreen extends ConsumerWidget {
  const LimitsScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final user = ref.watch(authControllerProvider).user;
    final ceilings = ref.watch(ceilingsProvider);
    final currency = user?.currencyCode ?? 'د.ل';

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'سقوف الرحالة',
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
              ],
            ),
          ),
        ],
      ),
    );
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
