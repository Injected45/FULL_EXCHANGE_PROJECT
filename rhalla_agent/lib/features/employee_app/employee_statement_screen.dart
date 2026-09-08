import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';

/// «كشف حوالاتي» — ما أنشأه الموظف وما سلّمه، في كشفٍ واحد للجرد.
///
/// ══════════════════════════════════════════════════════════════════════════
///  لماذا كشفٌ واحد لا تقريران
/// ══════════════════════════════════════════════════════════════════════════
///
/// ⚠ **لأنّ النقدَ في يده واحد.** ما قبضه من مُرسلٍ وما دفعه لمستفيدٍ في
/// درجٍ واحد، فجردُهما في شاشتين يعني أن يجمع الموظفُ بنفسه ويطرح — وكلُّ
/// حسابٍ يدويّ في نهاية يومٍ طويل مصدرُ خطأ.
///
/// فالكشفُ يعرض الاثنين بترتيبٍ زمنيّ، ويُظهر ثلاثة أرقام: ما قبض، وما دفع،
/// والصافي بينهما.
///
/// ⚠ **والصافي حركةُ نقدٍ لا رصيدُ حساب**: كم دخل جيبَه وكم خرج منه. وهو لا
/// يُقرأ من دفتر المنظومة ولا يُكتب فيه — عهدةٌ تُجرد، لا حسابٌ يُرحَّل.
class EmployeeStatementScreen extends ConsumerStatefulWidget {
  const EmployeeStatementScreen({super.key});

  @override
  ConsumerState<EmployeeStatementScreen> createState() =>
      _EmployeeStatementScreenState();
}

class _EmployeeStatementScreenState
    extends ConsumerState<EmployeeStatementScreen> {
  int _days = 7;

  static const _windows = <int, String>{
    1: 'اليوم',
    7: 'أسبوع',
    30: 'شهر',
  };

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(employeeStatementProvider(_days));

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'كشف حوالاتي', onBack: () => context.pop()),

          Padding(
            padding: const EdgeInsets.symmetric(
                horizontal: R.padScreen, vertical: 8),
            child: Row(
              children: _windows.entries
                  .map((e) => Padding(
                        padding: const EdgeInsets.only(left: 8),
                        child: _Chip(
                          label: e.value,
                          selected: _days == e.key,
                          onTap: () => setState(() => _days = e.key),
                        ),
                      ))
                  .toList(),
            ),
          ),

          Expanded(
            child: RefreshIndicator(
              onRefresh: () async => ref.invalidate(employeeStatementProvider(_days)),
              color: R.primary,
              backgroundColor: Colors.white,
              child: async.when(
                loading: () => const Center(child: CircularProgressIndicator()),
                error: (e, _) => _Msg(
                  icon: Icons.wifi_off_rounded,
                  text: 'تعذّر تحميل الكشف.\n$e',
                ),
                data: (s) => ListView(
                  padding: const EdgeInsets.fromLTRB(
                      R.padScreen, 4, R.padScreen, 30),
                  children: [
                    _Totals(s: s),
                    const SizedBox(height: R.gapCard),
                    if (s.items.isEmpty)
                      const _Msg(
                        icon: Icons.receipt_long_rounded,
                        text: 'لا حوالات في هذه المدة.',
                      )
                    else
                      ...s.items.map((it) => Padding(
                            padding: const EdgeInsets.only(bottom: 8),
                            child: _Row(it: it),
                          )),
                  ],
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}

/* ───────────────── الطراز ───────────────── */

class StatementItem {
  const StatementItem({
    required this.transferNumber,
    required this.action,
    required this.amount,
    required this.at,
  });

  final String transferNumber;
  final String action;
  final double amount;
  final String? at;

  bool get isIn => action == 'CREATED';
}

class Statement {
  const Statement({
    required this.createdCount,
    required this.createdTotal,
    required this.deliveredCount,
    required this.deliveredTotal,
    required this.net,
    required this.items,
  });

  final int createdCount;
  final double createdTotal;
  final int deliveredCount;
  final double deliveredTotal;
  final double net;
  final List<StatementItem> items;

  static Statement fromJson(Map<String, dynamic> j) => Statement(
        createdCount: int.tryParse('${j['created_count'] ?? 0}') ?? 0,
        createdTotal: Fmt.num_(j['created_total']),
        deliveredCount: int.tryParse('${j['delivered_count'] ?? 0}') ?? 0,
        deliveredTotal: Fmt.num_(j['delivered_total']),
        net: Fmt.num_(j['net']),
        items: ((j['items'] as List?) ?? const [])
            .map((e) => (e as Map).cast<String, dynamic>())
            .map((m) => StatementItem(
                  transferNumber: '${m['transfer_number'] ?? ''}',
                  action: '${m['action'] ?? ''}',
                  amount: Fmt.num_(m['amount']),
                  at: '${m['at'] ?? ''}',
                ))
            .toList(),
      );
}

/// ⚠ باسمٍ صريح: `statementProvider` مأخوذٌ لكشف الوكيل في
/// `home_repository.dart`. واسمان متطابقان في ملفّين لا يتضاربان حتى
/// يُستورَد الملفّان معاً — ثمّ يتضاربان في يومٍ لا أحدَ يتوقّعه.
final employeeStatementProvider =
    FutureProvider.autoDispose.family<Statement, int>((ref, days) async {
  final env = await ref
      .watch(apiClientProvider)
      .get('/device/employee/statement', query: {'days': days});
  return Statement.fromJson(env.row ?? const {});
});

/* ───────────────── العناصر ───────────────── */

class _Totals extends StatelessWidget {
  const _Totals({required this.s});
  final Statement s;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          children: [
            Row(
              children: [
                Expanded(
                  child: _Cell(
                    label: 'قبضتُ',
                    sub: '${s.createdCount} حوالة',
                    value: s.createdTotal,
                    tone: R.primaryDark,
                  ),
                ),
                Container(width: 1, height: 46, color: R.inkA(.10)),
                Expanded(
                  child: _Cell(
                    label: 'سلَّمتُ',
                    sub: '${s.deliveredCount} حوالة',
                    value: s.deliveredTotal,
                    tone: R.error,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 12),
            Divider(color: R.inkA(.08), height: 1),
            const SizedBox(height: 12),

            // ⚠ الصافي بارزٌ: هو الرقمُ الذي يُقارَن بالنقد في الدرج.
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text('الصافي',
                        style: T.kufi(13.5, FontWeight.w800)),
                    Text('قبضتُ − سلَّمتُ',
                        style: T.plex(11, FontWeight.w400, color: R.inkA(.5))),
                  ],
                ),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Row(
                    children: [
                      Text('د.ل',
                          style: T.kufi(12.5, FontWeight.w700,
                              color: R.primaryDark)),
                      const SizedBox(width: 5),
                      Text(Fmt.money(s.net),
                          style: T.kufi(20, FontWeight.w800, color: R.ink)),
                    ],
                  ),
                ),
              ],
            ),
          ],
        ),
      );
}

class _Cell extends StatelessWidget {
  const _Cell({
    required this.label,
    required this.sub,
    required this.value,
    required this.tone,
  });

  final String label;
  final String sub;
  final double value;
  final Color tone;

  @override
  Widget build(BuildContext context) => Column(
        children: [
          Text(label, style: T.plex(12, FontWeight.w700, color: tone)),
          const SizedBox(height: 4),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Text(Fmt.money(value),
                style: T.kufi(16, FontWeight.w800, color: R.ink)),
          ),
          const SizedBox(height: 2),
          Text(sub, style: T.plex(10.5, FontWeight.w400, color: R.inkA(.45))),
        ],
      );
}

class _Row extends StatelessWidget {
  const _Row({required this.it});
  final StatementItem it;

  @override
  Widget build(BuildContext context) {
    final tone = it.isIn ? R.primaryDark : R.error;

    return Container(
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        color: R.whiteA(.55),
        border: Border.all(color: R.inkA(.08)),
        borderRadius: BorderRadius.circular(R.rCard),
      ),
      child: Row(
        children: [
          Container(
            width: 32,
            height: 32,
            decoration: BoxDecoration(
              color: tone.withValues(alpha: .10),
              shape: BoxShape.circle,
            ),
            child: Icon(
              it.isIn
                  ? Icons.south_west_rounded
                  : Icons.north_east_rounded,
              size: 17,
              color: tone,
            ),
          ),
          const SizedBox(width: 10),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(it.isIn ? 'حوالة أنشأتُها' : 'حوالة سلَّمتُها',
                    style: T.kufi(13, FontWeight.w700)),
                const SizedBox(height: 2),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(it.transferNumber,
                      style: T.plex(11, FontWeight.w400, color: R.inkA(.5))),
                ),
              ],
            ),
          ),
          Column(
            crossAxisAlignment: CrossAxisAlignment.end,
            children: [
              Directionality(
                textDirection: TextDirection.ltr,
                child: Text(
                  // ⚠ الإشارةُ تقول الاتجاه بلا قراءة: + دخل، − خرج.
                  '${it.isIn ? '+' : '−'} ${Fmt.money(it.amount)}',
                  style: T.kufi(14, FontWeight.w800, color: tone),
                ),
              ),
              const SizedBox(height: 2),
              Text(Fmt.stampShort(it.at),
                  style: T.plex(10.5, FontWeight.w400, color: R.inkA(.45))),
            ],
          ),
        ],
      ),
    );
  }
}

class _Chip extends StatelessWidget {
  const _Chip({required this.label, required this.selected, required this.onTap});

  final String label;
  final bool selected;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(99),
        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 15, vertical: 8),
          decoration: BoxDecoration(
            color: selected ? R.primaryA(.12) : R.whiteA(.55),
            border: Border.all(
                color: selected ? R.primaryA(.45) : R.inkA(.10),
                width: selected ? 1.4 : 1),
            borderRadius: BorderRadius.circular(99),
          ),
          child: Text(label,
              style: T.plex(12.5, selected ? FontWeight.w700 : FontWeight.w500,
                  color: selected ? R.primaryDark : R.inkA(.6))),
        ),
      );
}

class _Msg extends StatelessWidget {
  const _Msg({required this.icon, required this.text});
  final IconData icon;
  final String text;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.fromLTRB(30, 50, 30, 30),
        child: Column(
          children: [
            Icon(icon, size: 42, color: R.inkA(.24)),
            const SizedBox(height: 14),
            Text(text,
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w500,
                    color: R.inkA(.5), height: 1.8)),
          ],
        ),
      );
}
