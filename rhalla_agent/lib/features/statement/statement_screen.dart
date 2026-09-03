import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import '../home/home_repository.dart';

/// الكشف الكامل. الخادم يعيد **كل** التاريخ بلا ترقيم (رُصد 3345 صفاً)،
/// فنجلب مرة ونعرض دفعات.
final statementProvider = FutureProvider.autoDispose<List<Movement>>((ref) async {
  final api = ref.watch(apiClientProvider);
  try {
    final env = await api.get('/device/local/account/statment');
    return env.rows.map(Movement.fromJson).toList();
  } on ApiFailure catch (e) {
    if (e.isEmptyResult) return const [];
    rethrow;
  }
});

enum _Filter { all, credit, debit }

class StatementScreen extends ConsumerStatefulWidget {
  const StatementScreen({super.key});

  @override
  ConsumerState<StatementScreen> createState() => _StatementScreenState();
}

class _StatementScreenState extends ConsumerState<StatementScreen> {
  _Filter _filter = _Filter.all;
  static const _pageSize = 30;
  int _shown = _pageSize;

  @override
  Widget build(BuildContext context) {
    final user = ref.watch(authControllerProvider).user;
    final async = ref.watch(statementProvider);
    final currency = user?.currencyCode ?? 'د.ل';

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'كشف الحساب',
            subtitle: user == null ? null : 'ACC ${user.accId} · ${user.branchName ?? ''}',
            onBack: () => context.pop(),
          ),
          Expanded(
            child: async.when(
              loading: () =>
                  Center(child: CircularProgressIndicator(color: R.primary)),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(statementProvider),
              ),
              data: (all) {
                final rows = switch (_filter) {
                  _Filter.all => all,
                  _Filter.credit => all.where((m) => m.isCredit).toList(),
                  _Filter.debit => all.where((m) => !m.isCredit).toList(),
                };
                final visible = rows.take(_shown).toList();
                final groups = _groupByDate(visible);

                return ListView(
                  padding: const EdgeInsets.fromLTRB(
                      R.padScreen, 20, R.padScreen, 40),
                  children: [
                    _BalanceCard(
                      balance: all.isEmpty ? 0 : all.first.balance,
                      credits: all.where((m) => m.isCredit).fold<double>(
                          0, (s, m) => s + m.amount),
                      debits: all.where((m) => !m.isCredit).fold<double>(
                          0, (s, m) => s + m.amount),
                      currency: currency,
                    ),
                    const SizedBox(height: 18),
                    _Filters(
                      value: _filter,
                      onChanged: (f) => setState(() {
                        _filter = f;
                        _shown = _pageSize;
                      }),
                    ),
                    const SizedBox(height: 20),
                    if (rows.isEmpty)
                      const _Empty()
                    else ...[
                      for (final g in groups) ...[
                        Padding(
                          padding: const EdgeInsets.only(bottom: 10, top: 6),
                          child: Directionality(
                            textDirection: TextDirection.ltr,
                            child: Align(
                              alignment: AlignmentDirectional.centerStart,
                              child: Text(
                                g.key,
                                style: T.kufi(12, FontWeight.w600,
                                    color: R.inkA(.5), spacing: .6),
                              ),
                            ),
                          ),
                        ),
                        for (var i = 0; i < g.value.length; i++) ...[
                          if (i > 0) const SizedBox(height: R.gapRow),
                          _MovementRow(m: g.value[i], currency: currency),
                        ],
                        const SizedBox(height: 16),
                      ],
                      if (visible.length < rows.length)
                        GlassButton(
                          label:
                              'عرض المزيد · بقي ${Fmt.count(rows.length - visible.length)}',
                          onPressed: () => setState(() => _shown += _pageSize),
                        ),
                    ],
                  ],
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  /// التجميع بالتاريخ كما يعيده الخادم — `InsertDate` نص «YYYY-MM-DD».
  List<MapEntry<String, List<Movement>>> _groupByDate(List<Movement> rows) {
    final map = <String, List<Movement>>{};
    for (final m in rows) {
      final key = m.date.split(' ').first;
      map.putIfAbsent(key, () => []).add(m);
    }
    return map.entries.toList();
  }
}

class _BalanceCard extends StatelessWidget {
  const _BalanceCard({
    required this.balance,
    required this.credits,
    required this.debits,
    required this.currency,
  });

  final double balance;
  final double credits;
  final double debits;
  final String currency;

  @override
  Widget build(BuildContext context) => RiseIn(
        duration: const Duration(milliseconds: 500),
        child: GlassCard(
          large: true,
          sheen: true,
          padding: const EdgeInsets.fromLTRB(22, 20, 22, 20),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text('الرصيد الحالي', style: T.label),
              const SizedBox(height: 13),
              Directionality(
                textDirection: TextDirection.ltr,
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.baseline,
                  textBaseline: TextBaseline.alphabetic,
                  children: [
                    Text(currency,
                        style: T.plex(12, FontWeight.w400, color: R.inkA(.5))),
                    const Spacer(),
                    Text(Fmt.money(balance),
                        style: T.kufi(32, FontWeight.w800,
                            color: balance < 0 ? R.error : R.ink)),
                  ],
                ),
              ),
              const SizedBox(height: 16),
              Divider(color: R.inkA(.07), height: 1),
              const SizedBox(height: 14),
              Row(
                children: [
                  _Total(label: 'إجمالي الوارد', value: credits, credit: true),
                  const SizedBox(width: 24),
                  _Total(label: 'إجمالي الصادر', value: debits, credit: false),
                ],
              ),
            ],
          ),
        ),
      );
}

class _Total extends StatelessWidget {
  const _Total({required this.label, required this.value, required this.credit});

  final String label;
  final double value;
  final bool credit;

  @override
  Widget build(BuildContext context) => Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(label, style: T.meta),
          const SizedBox(height: 8),
          Directionality(
            textDirection: TextDirection.ltr,
            // الصادر أحمر كصفوفه، وإلا ناقض الإجمالي ما فوقه.
            child: Text(Fmt.money(value),
                style: T.kufi(14, FontWeight.w700,
                    color: credit ? R.credit : R.error)),
          ),
        ],
      );
}

class _Filters extends StatelessWidget {
  const _Filters({required this.value, required this.onChanged});

  final _Filter value;
  final ValueChanged<_Filter> onChanged;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          for (final f in _Filter.values) ...[
            if (f != _Filter.all) const SizedBox(width: 8),
            _Chip(
              label: switch (f) {
                _Filter.all => 'الكل',
                _Filter.credit => 'وارد',
                _Filter.debit => 'صادر',
              },
              on: f == value,
              onTap: () => onChanged(f),
            ),
          ],
        ],
      );
}

class _Chip extends StatelessWidget {
  const _Chip({required this.label, required this.on, required this.onTap});

  final String label;
  final bool on;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => Material(
        color: on ? Colors.transparent : R.whiteA(.7),
        borderRadius: BorderRadius.circular(99),
        child: InkWell(
          borderRadius: BorderRadius.circular(99),
          onTap: onTap,
          child: Ink(
            height: 44,
            decoration: BoxDecoration(
              gradient: on ? R.primaryGradient : null,
              border: on ? null : Border.all(color: R.whiteA(.9)),
              borderRadius: BorderRadius.circular(99),
            ),
            child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 20),
              child: Center(
                child: Text(label,
                    style: T.kufi(12, FontWeight.w600,
                        color: on ? Colors.white : R.inkA(.6))),
              ),
            ),
          ),
        ),
      );
}

class _MovementRow extends StatelessWidget {
  const _MovementRow({required this.m, required this.currency});

  final Movement m;
  final String currency;

  @override
  Widget build(BuildContext context) {
    final tone = m.isCredit ? RowTone.credit : RowTone.debit;

    final meta = T.plex(10.5, FontWeight.w400, color: R.inkA(.5));

    return GlassRow(
      dense: true,
      tone: tone,
      children: [
        Expanded(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(m.title.isEmpty ? 'حركة حساب' : Fmt.localName(m.title),
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: T.plex(12.5, FontWeight.w600, color: tone.ink)),
              const SizedBox(height: 3),
              // الرصيد بعد الحركة يبقى محايداً: هو ليس صادراً ولا وارداً،
              // وتلوينه بلون الحركة يوهم أنه جزء منها.
              Row(
                children: [
                  Text('الرصيد', style: meta),
                  const SizedBox(width: 5),
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Text(Fmt.money(m.balance), style: meta),
                  ),
                ],
              ),
            ],
          ),
        ),
        const SizedBox(width: 10),
        Directionality(
          textDirection: TextDirection.ltr,
          child: Text(
            Fmt.moneyWithSign(m.amount, credit: m.isCredit),
            style: T.kufi(13.5, FontWeight.w700, color: tone.ink),
          ),
        ),
      ],
    );
  }
}

class _Empty extends StatelessWidget {
  const _Empty();

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.symmetric(vertical: 50),
        child: Column(
          children: [
            RhallaLogo(size: 56, color: R.primaryA(.3)),
            const SizedBox(height: 18),
            Text('لا حركة في هذا التصنيف',
                style: T.kufi(15, FontWeight.w600, height: 1.5)),
          ],
        ),
      );
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.all(R.padScreen),
          child: GlassCard(
            child: Column(
              mainAxisSize: MainAxisSize.min,
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
          ),
        ),
      );
}
