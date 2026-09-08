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
import 'employee_session.dart';

/* ══════════════════════════════════════════════════════════════════════════
   الأرصدة — رصيدُ الوكيل والملخّصُ الماليّ
   ══════════════════════════════════════════════════════════════════════════

   ⚠ **قراءةٌ خالصة، ورقمٌ واحد لا رقمان**: الرصيد يُقرأ بمسار الوكيل نفسِه
   في الخادم، فما يراه الموظف هو ما يراه وكيلُه حرفاً بحرف. ولو حُسب هنا
   بطريقةٍ أخرى لصار للرصيد جوابان.
   ══════════════════════════════════════════════════════════════════════════ */

final _balanceProvider = FutureProvider.autoDispose<double?>((ref) async {
  final env = await ref.watch(apiClientProvider).get('/device/employee/balance');
  final rows = env.rows;
  if (rows.isEmpty) return null;
  return Fmt.num_(rows.first['Walet']);
});

final _summaryProvider =
    FutureProvider.autoDispose<Map<String, dynamic>>((ref) async {
  final env = await ref.watch(apiClientProvider).get('/device/employee/summary');
  return env.row ?? const {};
});

class EmployeeBalancesScreen extends ConsumerWidget {
  const EmployeeBalancesScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final p = ref.watch(employeeAuthProvider).profile;
    final canBalance = p?.can('VIEW_AGENT_TOTAL_BALANCE') ?? false;
    final canSummary = p?.can('VIEW_FINANCIAL_SUMMARY') ?? false;

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'الأرصدة', onBack: () => context.pop()),
          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 16, R.padScreen, 30),
              children: [
                if (canBalance) _AgentBalance(ref: ref),
                if (canBalance && canSummary) const SizedBox(height: R.gapCard),
                if (canSummary) _Summary(ref: ref),
                if (!canBalance && !canSummary)
                  Padding(
                    padding: const EdgeInsets.only(top: 60),
                    child: Column(
                      children: [
                        Icon(Icons.lock_outline_rounded,
                            size: 42, color: R.inkA(.24)),
                        const SizedBox(height: 14),
                        Text('لا أرصدة ممنوحة لك.',
                            textAlign: TextAlign.center,
                            style: T.plex(13, FontWeight.w500,
                                color: R.inkA(.5))),
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

class _AgentBalance extends StatelessWidget {
  const _AgentBalance({required this.ref});
  final WidgetRef ref;

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(_balanceProvider);

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Text('رصيد الوكيل', style: T.kufi(14, FontWeight.w700)),
          const SizedBox(height: 10),
          async.when(
            loading: () => const Center(
                child: SizedBox(
                    width: 22,
                    height: 22,
                    child: CircularProgressIndicator(strokeWidth: 2))),
            error: (e, _) => Text('تعذّر التحميل.',
                style: T.plex(12.5, FontWeight.w500, color: R.error)),
            data: (v) => v == null
                ? Text('لا رصيد.',
                    style: T.plex(12.5, FontWeight.w500, color: R.inkA(.5)))
                : Directionality(
                    textDirection: TextDirection.ltr,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Text('د.ل',
                            style: T.kufi(14, FontWeight.w700,
                                color: R.primaryDark)),
                        const SizedBox(width: 7),
                        Text(Fmt.money(v),
                            style: T.kufi(26, FontWeight.w800, color: R.ink)),
                      ],
                    ),
                  ),
          ),
        ],
      ),
    );
  }
}

class _Summary extends StatelessWidget {
  const _Summary({required this.ref});
  final WidgetRef ref;

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(_summaryProvider);

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Text('ملخّص يومي', style: T.kufi(14, FontWeight.w700)),
          const SizedBox(height: 12),
          async.when(
            loading: () => const Center(
                child: SizedBox(
                    width: 22,
                    height: 22,
                    child: CircularProgressIndicator(strokeWidth: 2))),
            error: (e, _) => Text('تعذّر التحميل.',
                style: T.plex(12.5, FontWeight.w500, color: R.error)),
            data: (d) => Column(
              children: [
                _Line('حوالات اليوم',
                    '${Fmt.count(int.tryParse('${d['today_count'] ?? 0}') ?? 0)} · ${Fmt.money(Fmt.num_(d['today_total']))}'),
                _Line('نقدٌ دخل خزينتي', Fmt.money(Fmt.num_(d['cashbox_in']))),
                _Line('نقدٌ خرج منها', Fmt.money(Fmt.num_(d['cashbox_out']))),
                _Line('صافي الخزينة', Fmt.money(Fmt.num_(d['cashbox_net'])),
                    bold: true),
                _Line('بانتظار التسليم',
                    '${Fmt.count(int.tryParse('${d['pending_count'] ?? 0}') ?? 0)} · ${Fmt.money(Fmt.num_(d['pending_total']))}'),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _Line extends StatelessWidget {
  const _Line(this.label, this.value, {this.bold = false});

  final String label;
  final String value;
  final bool bold;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.only(bottom: 8),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Text(label,
                style: T.plex(12.5, bold ? FontWeight.w700 : FontWeight.w500,
                    color: bold ? R.ink : R.inkA(.62))),
            Directionality(
              textDirection: TextDirection.ltr,
              child: Text(value,
                  style: T.kufi(bold ? 15 : 13.5,
                      bold ? FontWeight.w800 : FontWeight.w700)),
            ),
          ],
        ),
      );
}

/* ══════════════════════════════════════════════════════════════════════════
   المستفيدون المفضّلون
   ══════════════════════════════════════════════════════════════════════════

   ⚠ العرضُ والإدارةُ صلاحيتان لا واحدة: موظفٌ يرى مفضّلة وكيله ليختار منها
   بسرعة، وآخرُ يُضيف إليها ويحذف. والوكيل يقرّر أيّهما.
   ══════════════════════════════════════════════════════════════════════════ */

final _favoritesProvider =
    FutureProvider.autoDispose<List<Map<String, dynamic>>>((ref) async {
  final env = await ref
      .watch(apiClientProvider)
      .post('/device/employee/favorites', body: const {});
  return env.rows;
});

class EmployeeFavoritesScreen extends ConsumerStatefulWidget {
  const EmployeeFavoritesScreen({super.key});

  @override
  ConsumerState<EmployeeFavoritesScreen> createState() =>
      _EmployeeFavoritesScreenState();
}

class _EmployeeFavoritesScreenState
    extends ConsumerState<EmployeeFavoritesScreen> {
  int? _busy;

  @override
  Widget build(BuildContext context) {
    final p = ref.watch(employeeAuthProvider).profile;
    final canManage = p?.can('MANAGE_FAVORITES') ?? false;
    final async = ref.watch(_favoritesProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'المستفيدون', onBack: () => context.pop()),
          Expanded(
            child: RefreshIndicator(
              onRefresh: () async => ref.invalidate(_favoritesProvider),
              color: R.primary,
              backgroundColor: Colors.white,
              child: async.when(
                loading: () => const Center(child: CircularProgressIndicator()),
                error: (e, _) => _Empty(text: 'تعذّر التحميل.\n$e'),
                data: (rows) => rows.isEmpty
                    ? const _Empty(text: 'لا مستفيدين محفوظين.')
                    : ListView.separated(
                        padding: const EdgeInsets.fromLTRB(
                            R.padScreen, 14, R.padScreen, 30),
                        itemCount: rows.length,
                        separatorBuilder: (_, _) => const SizedBox(height: 8),
                        itemBuilder: (_, i) {
                          final m = rows[i];
                          final id = int.tryParse('${m['ID'] ?? m['id'] ?? 0}') ?? 0;
                          return _FavRow(
                            name: '${m['Name'] ?? m['name'] ?? ''}',
                            phone: '${m['Phone'] ?? m['phone'] ?? ''}',
                            busy: _busy == id,
                            onDelete: canManage && id > 0
                                ? () => _delete(id)
                                : null,
                          );
                        },
                      ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _delete(int id) async {
    setState(() => _busy = id);
    try {
      await ref
          .read(apiClientProvider)
          .post('/device/employee/favorites/delete', body: {'id': id});
      if (mounted) ref.invalidate(_favoritesProvider);
    } on ApiFailure catch (e) {
      if (mounted) _say(e.message);
    } catch (_) {
      if (mounted) _say('تعذّر الحذف — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busy = null);
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

class _FavRow extends StatelessWidget {
  const _FavRow({
    required this.name,
    required this.phone,
    required this.busy,
    required this.onDelete,
  });

  final String name;
  final String phone;
  final bool busy;
  final VoidCallback? onDelete;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(12),
        decoration: BoxDecoration(
          color: R.whiteA(.55),
          border: Border.all(color: R.inkA(.08)),
          borderRadius: BorderRadius.circular(R.rCard),
        ),
        child: Row(
          children: [
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(name.isEmpty ? '—' : name,
                      style: T.kufi(13.5, FontWeight.w700)),
                  if (phone.isNotEmpty) ...[
                    const SizedBox(height: 2),
                    Directionality(
                      textDirection: TextDirection.ltr,
                      child: Text(Fmt.phone(phone),
                          style: T.plex(11.5, FontWeight.w400,
                              color: R.inkA(.55))),
                    ),
                  ],
                ],
              ),
            ),
            if (onDelete != null)
              busy
                  ? const SizedBox(
                      width: 18,
                      height: 18,
                      child: CircularProgressIndicator(strokeWidth: 2))
                  : IconButton(
                      onPressed: onDelete,
                      icon: Icon(Icons.delete_outline_rounded,
                          size: 20, color: R.error),
                      constraints:
                          const BoxConstraints(minWidth: 40, minHeight: 40),
                    ),
          ],
        ),
      );
}

class _Empty extends StatelessWidget {
  const _Empty({required this.text});
  final String text;

  @override
  Widget build(BuildContext context) => ListView(
        padding: const EdgeInsets.fromLTRB(30, 70, 30, 30),
        children: [
          Icon(Icons.people_outline_rounded, size: 42, color: R.inkA(.24)),
          const SizedBox(height: 14),
          Text(text,
              textAlign: TextAlign.center,
              style: T.plex(13, FontWeight.w500,
                  color: R.inkA(.5), height: 1.8)),
        ],
      );
}
