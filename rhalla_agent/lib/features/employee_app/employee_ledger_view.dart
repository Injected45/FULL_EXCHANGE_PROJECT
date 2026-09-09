import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';

/* ══════════════════════════════════════════════════════════════════════════
   كشفُ حركة خزينة الموظف — للجرد
   ══════════════════════════════════════════════════════════════════════════

   حركاتُ هذا الموظف وحدَه، بالرصيد بعد كل حركة، وفلترةٍ بالتاريخ والنوع.

   ── أربعةُ قراراتٍ في العرض ليست ذوقاً ────────────────────────────────────

   1. **الأقدمُ أولاً — إلزاماً لا تفضيلاً.**
      عمودُ الرصيد تراكميّ، وقراءتُه من الأحدث إلى الأقدم تجعل كل سطرٍ يحمل
      رصيدَ ما بعده. والترتيبُ هنا هو ترتيبُ الحساب نفسِه.

   2. **الرصيدُ يُصفَّر عند رأس كل وردية ولا يتراكم.**
      الورديةُ دورةُ تصفية: تُقفل بعدّ النقد وتسليمِه، ثم تُفتح التالية
      بافتتاحيٍّ يُعلنه الموظف — لا بما انتهت إليه سابقتُها. فاستمرارُ
      الرصيد بينهما يزعم استمراريةً لا وجود لها. والقسمُ هنا يفصلها بصفٍّ
      افتتاحيٍّ ظاهر، وصفِّ إقفالٍ يقول المتوقَّع والمعدود والفرق.

   3. **المعكوسةُ تُعرض مشطوبةً ولا تُحسب.**
      التصحيحُ بعكسٍ لا بحذف، وكشفٌ يُخفي ما صُحّح لا يشهد على ما جرى.

   4. **الأرقامُ والأسماءُ تأتي من الخادم كما هي.**
      لا مجموعَ يُحسب هنا ولا نوعَ يُترجَم: نوعٌ يُضاف في الخادم غداً يظهر
      باسمه العربيّ بلا إصدارٍ جديد. ولو حُسب المجموعُ هنا لصار للرصيد
      جوابان — والجوابُ الآخر هو ما يُقفل به الوكيلُ الوردية.
   ══════════════════════════════════════════════════════════════════════════ */

/// مدى التاريخ المطلوب.
enum LedgerRange { day, week, month, all, custom }

@immutable
class LedgerQuery {
  const LedgerQuery({this.range = LedgerRange.month, this.type, this.custom});

  final LedgerRange range;

  /// `null` = كلُّ الأنواع.
  final String? type;

  final DateTimeRange? custom;

  LedgerQuery copyWith({
    LedgerRange? range,
    String? type,
    bool clearType = false,
    DateTimeRange? custom,
  }) =>
      LedgerQuery(
        range: range ?? this.range,
        type: clearType ? null : (type ?? this.type),
        custom: custom ?? this.custom,
      );

  /// معاملاتُ النداء — والخادمُ يقصّ، لا التطبيق.
  ///
  /// ⚠ القصُّ في الخادم لا هنا: الرصيدُ التراكميّ يُحسب من أول المدى، فقصُّ
  /// النتيجة بعد وصولها كان سيُظهر رصيداً يبدأ من منتصف الحساب.
  Map<String, dynamic> get params {
    final now = DateTime.now();
    final (DateTime? from, DateTime? to) = switch (range) {
      LedgerRange.day => (DateTime(now.year, now.month, now.day), now),
      LedgerRange.week => (now.subtract(const Duration(days: 6)), now),
      LedgerRange.month => (DateTime(now.year, now.month, 1), now),
      LedgerRange.all => (null, null),
      LedgerRange.custom => (custom?.start, custom?.end),
    };

    String d(DateTime v) => '${v.year.toString().padLeft(4, '0')}-'
        '${v.month.toString().padLeft(2, '0')}-'
        '${v.day.toString().padLeft(2, '0')}';

    return {
      if (from != null) 'from': d(from),
      if (to != null) 'to': d(to),
      if (type != null) 'type': type,
    };
  }

  @override
  bool operator ==(Object other) =>
      other is LedgerQuery &&
      other.range == range &&
      other.type == type &&
      other.custom == custom;

  @override
  int get hashCode => Object.hash(range, type, custom);
}

/// الكشفُ كما يعيده الخادم — بلا حسابٍ محلّي.
@immutable
class Ledger {
  const Ledger({
    required this.opening,
    required this.cashIn,
    required this.cashOut,
    required this.net,
    required this.current,
    required this.hasShift,
    required this.count,
    required this.shiftCount,
    required this.partial,
    required this.truncated,
    required this.types,
    required this.rows,
  });

  final double opening;
  final double cashIn;
  final double cashOut;
  final double net;

  /// الرصيدُ الحالي = متوقَّعُ الوردية المفتوحة. `null` بلا وردية.
  final double? current;

  final bool hasShift;
  final int count;
  final int shiftCount;

  /// المجموعُ جزئيّ لأنّ نوعاً واحداً مفلتَر.
  final bool partial;

  final bool truncated;

  /// أسماءُ الأنواع بالعربية — من الخادم.
  final Map<String, String> types;

  final List<LedgerRow> rows;

  static Ledger fromJson(Map<String, dynamic> j) => Ledger(
        opening: Fmt.num_(j['opening']),
        cashIn: Fmt.num_(j['in']),
        cashOut: Fmt.num_(j['out']),
        net: Fmt.num_(j['net']),
        current: j['current'] == null ? null : Fmt.num_(j['current']),
        hasShift: j['has_shift'] == true,
        count: int.tryParse('${j['count'] ?? 0}') ?? 0,
        shiftCount: int.tryParse('${j['shift_count'] ?? 0}') ?? 0,
        partial: j['partial'] == true,
        truncated: j['truncated'] == true,
        types: ((j['types'] as Map?) ?? const {})
            .map((k, v) => MapEntry('$k', '$v')),
        rows: ((j['rows'] as List?) ?? const [])
            .whereType<Map>()
            .map((m) => LedgerRow.fromJson(m.cast<String, dynamic>()))
            .toList(),
      );
}

@immutable
class LedgerRow {
  const LedgerRow({
    required this.kind,
    required this.typeLabel,
    required this.at,
    required this.reference,
    required this.cashIn,
    required this.cashOut,
    required this.balance,
    required this.status,
    required this.note,
    required this.counted,
    required this.reversed,
    required this.isReversal,
    required this.outsideShift,
    required this.expected,
    required this.actual,
    required this.difference,
  });

  /// OPENING · MOVE · CLOSING
  final String kind;

  final String typeLabel;
  final String at;
  final String? reference;

  /// `null` يعني «لا قيمة في هذا العمود»، وهو غيرُ صفر.
  final double? cashIn;
  final double? cashOut;

  final double balance;
  final String status;
  final String? note;

  final bool counted;
  final bool reversed;
  final bool isReversal;
  final bool outsideShift;

  /// لصفّ الإقفال وحدَه.
  final double? expected;
  final double? actual;
  final double? difference;

  bool get isOpening => kind == 'OPENING';
  bool get isClosing => kind == 'CLOSING';

  static double? _opt(Object? v) => v == null ? null : Fmt.num_(v);

  static LedgerRow fromJson(Map<String, dynamic> j) => LedgerRow(
        kind: '${j['kind'] ?? 'MOVE'}',
        typeLabel: '${j['type_label'] ?? 'حركة'}',
        at: '${j['at'] ?? ''}',
        reference: (j['reference'] as Object?)?.toString(),
        cashIn: _opt(j['in']),
        cashOut: _opt(j['out']),
        balance: Fmt.num_(j['balance']),
        status: '${j['status'] ?? ''}',
        note: (j['note'] as Object?)?.toString(),
        counted: j['counted'] != false,
        reversed: j['reversed'] == true,
        isReversal: j['is_reversal'] == true,
        outsideShift: j['outside_shift'] == true,
        expected: _opt(j['expected']),
        actual: _opt(j['actual']),
        difference: _opt(j['difference']),
      );
}

final ledgerProvider =
    FutureProvider.autoDispose.family<Ledger, LedgerQuery>((ref, q) async {
  final env = await ref
      .watch(apiClientProvider)
      .get('/device/employee/cashbox/ledger', query: q.params);
  return Ledger.fromJson(env.row ?? const {});
});

/* ═══════════════════════════ العرض ═══════════════════════════ */

class EmployeeLedgerView extends ConsumerStatefulWidget {
  const EmployeeLedgerView({super.key});

  @override
  ConsumerState<EmployeeLedgerView> createState() => _EmployeeLedgerViewState();
}

class _EmployeeLedgerViewState extends ConsumerState<EmployeeLedgerView> {
  LedgerQuery _q = const LedgerQuery();

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(ledgerProvider(_q));

    return RefreshIndicator(
      onRefresh: () async => ref.invalidate(ledgerProvider(_q)),
      color: R.primary,
      backgroundColor: Colors.white,
      child: ListView(
        padding: const EdgeInsets.fromLTRB(R.padScreen, 14, R.padScreen, 40),
        physics: const AlwaysScrollableScrollPhysics(),
        children: [
          _Ranges(
            query: _q,
            types: async.valueOrNull?.types ?? const {},
            onChanged: (q) => setState(() => _q = q),
          ),
          const SizedBox(height: R.gapCard),
          switch (async) {
            AsyncData(:final value) => _Body(ledger: value),
            AsyncError(:final error) => _Failed(
                message: error is ApiFailure
                    ? error.message
                    : 'تعذّر الاتصال بالخادم.',
                onRetry: () => ref.invalidate(ledgerProvider(_q)),
              ),
            _ => Padding(
                padding: const EdgeInsets.only(top: 40),
                child:
                    Center(child: CircularProgressIndicator(color: R.primary)),
              ),
          },
        ],
      ),
    );
  }
}

class _Body extends StatelessWidget {
  const _Body({required this.ledger});

  final Ledger ledger;

  @override
  Widget build(BuildContext context) {
    if (ledger.rows.isEmpty) {
      return GlassCard(
        child: Column(
          children: [
            Icon(Icons.receipt_long_outlined, size: 34, color: R.inkA(.28)),
            const SizedBox(height: 10),
            Text('لا حركات في هذه الفترة.',
                style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
          ],
        ),
      );
    }

    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        _Totals(ledger: ledger),
        const SizedBox(height: R.gapCard),
        for (final r in ledger.rows) ...[
          _Row(row: r),
          const SizedBox(height: R.gapRow),
        ],
        if (ledger.truncated) ...[
          const SizedBox(height: 6),
          Text(
            'عُرضت أوّلُ ٥٠٠ حركة في هذه الفترة — ضيّق التاريخ لرؤية الباقي.',
            textAlign: TextAlign.center,
            style: T.plex(11.5, FontWeight.w400, color: R.inkA(.5)),
          ),
        ],
      ],
    );
  }
}

class _Totals extends StatelessWidget {
  const _Totals({required this.ledger});

  final Ledger ledger;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            _line('الرصيد الافتتاحي', ledger.opening),
            const SizedBox(height: 8),
            _line('إجمالي الداخل', ledger.cashIn, tone: R.primaryDark),
            const SizedBox(height: 8),
            _line('إجمالي الخارج', ledger.cashOut, tone: R.error),
            const SizedBox(height: 10),
            Divider(color: R.inkA(.08), height: 1),
            const SizedBox(height: 10),

            /*
             * ⚠ «الرصيد الحالي» هو متوقَّعُ الوردية المفتوحة — لا مجموعُ
             * المعروض. والفرقُ حقيقيّ: الكشفُ قد يُفلتَر بتاريخٍ لا يشمل
             * الوردية كلَّها، والمطلوبُ منه الآن لا يتغيّر بتغيّر الفلتر.
             */
            if (ledger.current != null)
              _line('الرصيد الحالي المطلوب منك', ledger.current!, strong: true)
            else
              Row(
                children: [
                  Icon(Icons.info_outline_rounded, size: 15, color: R.inkA(.45)),
                  const SizedBox(width: 6),
                  Expanded(
                    child: Text('لا وردية مفتوحة الآن.',
                        style: T.plex(12, FontWeight.w500, color: R.inkA(.55))),
                  ),
                ],
              ),

            if (ledger.partial) ...[
              const SizedBox(height: 12),
              // ⚠ يقولها صراحةً: المجاميعُ أعلاه للنوع المفلتَر وحدَه، ومن
              // يقرأ الجزءَ كلاًّ يخرج برقمٍ ناقص في جرد.
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 8),
                decoration: BoxDecoration(
                  color: R.warnIcon.withValues(alpha: .1),
                  borderRadius: BorderRadius.circular(R.rTile),
                ),
                child: Row(
                  children: [
                    Icon(Icons.filter_alt_outlined, size: 14, color: R.warnIcon),
                    const SizedBox(width: 6),
                    Expanded(
                      child: Text(
                        'المجاميع للنوع المختار وحدَه — أزل الفلتر للجرد الكامل.',
                        style: T.plex(11, FontWeight.w500, color: R.inkA(.7)),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ],
        ),
      );

  Widget _line(String label, double value,
          {Color? tone, bool strong = false}) =>
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
                        : T.kufi(13.5, FontWeight.w600, color: tone ?? R.ink)),
              ],
            ),
          ),
        ],
      );
}

/// صفُّ الكشف — حركةً أو رأسَ ورديةٍ أو إقفالاً.
class _Row extends StatelessWidget {
  const _Row({required this.row});

  final LedgerRow row;

  @override
  Widget build(BuildContext context) {
    if (row.isOpening) return _shiftHead(context);
    if (row.isClosing) return _shiftFoot(context);
    return _move(context);
  }

  /* ── رأسُ الوردية ─────────────────────────────────────────────────── */
  Widget _shiftHead(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(12, 10, 12, 10),
        decoration: BoxDecoration(
          gradient: R.primaryGradient,
          borderRadius: BorderRadius.circular(R.rTile),
        ),
        child: Row(
          children: [
            Icon(Icons.play_circle_outline_rounded,
                size: 16, color: Colors.white),
            const SizedBox(width: 7),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(row.typeLabel,
                      style: T.kufi(12.5, FontWeight.w700, color: Colors.white)),
                  Text('${row.status} · ${Fmt.stamp(row.at)}',
                      style:
                          T.plex(10.5, FontWeight.w400, color: R.whiteA(.8))),
                ],
              ),
            ),
            _money(row.balance, Colors.white, bold: true),
          ],
        ),
      );

  /* ── إقفالُ الوردية ───────────────────────────────────────────────── */
  Widget _shiftFoot(BuildContext context) {
    final diff = row.difference ?? 0;
    final tone = diff.abs() < 0.0005
        ? R.primaryDark
        : (diff < 0 ? R.error : R.warnIcon);

    return Container(
      padding: const EdgeInsets.fromLTRB(12, 11, 12, 11),
      decoration: BoxDecoration(
        color: tone.withValues(alpha: .08),
        border: Border.all(color: tone.withValues(alpha: .3)),
        borderRadius: BorderRadius.circular(R.rTile),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            children: [
              Icon(Icons.lock_outline_rounded, size: 15, color: tone),
              const SizedBox(width: 7),
              Expanded(
                child: Text('${row.typeLabel} — ${row.status}',
                    style: T.kufi(12.5, FontWeight.w700, color: tone)),
              ),
              Text(Fmt.stamp(row.at),
                  style: T.plex(10.5, FontWeight.w400, color: R.inkA(.5))),
            ],
          ),
          const SizedBox(height: 8),
          // المتوقَّع والمعدود والفرق — كما حُفظت ليلةَ الإقفال لا كما تُحسب
          // اليوم. انظر خدمة الكشف.
          _pair('المتوقَّع', row.expected),
          _pair('المعدود فعلاً', row.actual),
          _pair('الفرق', row.difference, tone: tone),
          if ((row.note ?? '').isNotEmpty) ...[
            const SizedBox(height: 6),
            Text(row.note!,
                style: T.plex(11, FontWeight.w400, color: R.inkA(.6))),
          ],
        ],
      ),
    );
  }

  Widget _pair(String label, double? v, {Color? tone}) => Padding(
        padding: const EdgeInsets.only(top: 3),
        child: Row(
          children: [
            Expanded(
              child: Text(label,
                  style: T.plex(11.5, FontWeight.w400, color: R.inkA(.6))),
            ),
            _money(v ?? 0, tone ?? R.ink),
          ],
        ),
      );

  /* ── حركة ────────────────────────────────────────────────────────── */
  Widget _move(BuildContext context) {
    final isIn = row.cashIn != null;
    final tone = !row.counted
        ? R.inkA(.45)
        : (isIn ? R.primaryDark : R.error);

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            children: [
              Icon(
                isIn ? Icons.south_west_rounded : Icons.north_east_rounded,
                size: 17,
                color: tone,
              ),
              const SizedBox(width: 9),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      row.typeLabel,
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      // ⚠ المعكوسةُ مشطوبة: تُقرأ حركةً جرت ثم صُحّحت،
                      // لا حركةً قائمةً بلونٍ باهت. و`T.kufi` لا تأخذ
                      // `decoration` — تُضاف بعدها لا بتجاوز النظام.
                      style: T
                          .kufi(13.5, FontWeight.w700,
                              color: row.counted ? R.ink : R.inkA(.5))
                          .copyWith(
                              decoration: row.counted
                                  ? null
                                  : TextDecoration.lineThrough),
                    ),
                    const SizedBox(height: 2),
                    Text(Fmt.stamp(row.at),
                        style: T.plex(10.5, FontWeight.w400, color: R.inkA(.5))),
                  ],
                ),
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.end,
                children: [
                  _money(row.cashIn ?? row.cashOut ?? 0, tone, bold: true),
                  const SizedBox(height: 2),
                  // ⚠ الرصيدُ بعد الحركة — عمودُ الجرد. ويظهر لكل صفٍّ حتى
                  // المعكوسة، وهو فيها مساوٍ لما قبلها لأنها لا تحرّكه.
                  Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Text('الرصيد ',
                          style: T.plex(9.5, FontWeight.w400,
                              color: R.inkA(.45))),
                      _money(row.balance, R.inkA(.7)),
                    ],
                  ),
                ],
              ),
            ],
          ),

          if ((row.reference ?? '').isNotEmpty ||
              row.status.isNotEmpty ||
              row.outsideShift ||
              (row.note ?? '').isNotEmpty) ...[
            const SizedBox(height: 9),
            Divider(color: R.inkA(.06), height: 1),
            const SizedBox(height: 8),
            Wrap(
              spacing: 6,
              runSpacing: 6,
              children: [
                if ((row.reference ?? '').isNotEmpty)
                  _chip('المرجع: ${row.reference}', R.inkA(.55), ltr: true),
                if (row.status.isNotEmpty)
                  _chip(row.status, row.counted ? R.inkA(.55) : R.warnIcon),
                if (row.outsideShift)
                  // ⚠ لا تُخفى: مجموعُ الكشف يجب أن يساوي مجموعَ الجدول،
                  // لكنها لا تدخل في متوقَّع الوردية — فتُوسَم.
                  _chip('خارج وردية', R.warnIcon),
                if ((row.note ?? '').isNotEmpty)
                  _chip(row.note!, R.inkA(.55)),
              ],
            ),
          ],
        ],
      ),
    );
  }

  Widget _chip(String text, Color tone, {bool ltr = false}) {
    final label = Text(text,
        style: T.plex(10.5, FontWeight.w500, color: tone));

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      decoration: BoxDecoration(
        color: tone.withValues(alpha: .09),
        borderRadius: BorderRadius.circular(R.rTile),
      ),
      child: ltr
          ? Directionality(textDirection: TextDirection.ltr, child: label)
          : label,
    );
  }

  /// مبلغٌ بالرمز عن يساره — كسائر مبالغ التطبيق.
  Widget _money(double v, Color tone, {bool bold = false}) => Directionality(
        textDirection: TextDirection.ltr,
        child: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text('د.ل ',
                style: T.plex(9.5, FontWeight.w400,
                    color: tone.withValues(alpha: .7))),
            Text(Fmt.money(v),
                style: bold
                    ? T.kufi(14.5, FontWeight.w800, color: tone)
                    : T.kufi(11.5, FontWeight.w600, color: tone)),
          ],
        ),
      );
}

/* ═══════════════════════════ الفلاتر ═══════════════════════════ */

class _Ranges extends StatelessWidget {
  const _Ranges({
    required this.query,
    required this.types,
    required this.onChanged,
  });

  final LedgerQuery query;
  final Map<String, String> types;
  final ValueChanged<LedgerQuery> onChanged;

  @override
  Widget build(BuildContext context) => Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          SingleChildScrollView(
            scrollDirection: Axis.horizontal,
            child: Row(
              children: [
                _pill(context, 'اليوم', LedgerRange.day),
                _pill(context, 'أسبوع', LedgerRange.week),
                _pill(context, 'هذا الشهر', LedgerRange.month),
                _pill(context, 'الكل', LedgerRange.all),
                _pill(context, _customLabel(), LedgerRange.custom,
                    icon: Icons.date_range_rounded),
              ],
            ),
          ),
          if (types.isNotEmpty) ...[
            const SizedBox(height: 8),
            SingleChildScrollView(
              scrollDirection: Axis.horizontal,
              child: Row(
                children: [
                  _type(context, 'كل الأنواع', null),
                  for (final e in types.entries)
                    // ⚠ الافتتاحيّ ليس حركةً تُفلتَر — هو رأسُ القسم.
                    if (e.key != 'OPENING') _type(context, e.value, e.key),
                ],
              ),
            ),
          ],
        ],
      );

  String _customLabel() {
    final c = query.custom;
    if (c == null) return 'من — إلى';
    String d(DateTime v) => '${v.day}/${v.month}';
    return '${d(c.start)} – ${d(c.end)}';
  }

  Widget _pill(BuildContext context, String label, LedgerRange r,
      {IconData? icon}) {
    final on = query.range == r;
    return Padding(
      padding: const EdgeInsetsDirectional.only(end: 7),
      child: GestureDetector(
        onTap: () async {
          if (r != LedgerRange.custom) {
            onChanged(query.copyWith(range: r));
            return;
          }
          final picked = await showDateRangePicker(
            context: context,
            firstDate: DateTime(2024),
            lastDate: DateTime.now(),
            initialDateRange: query.custom,
            locale: const Locale('ar'),
            helpText: 'اختر الفترة',
            saveText: 'تم',
          );
          if (picked != null) {
            onChanged(query.copyWith(range: LedgerRange.custom, custom: picked));
          }
        },
        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 13, vertical: 8),
          decoration: BoxDecoration(
            gradient: on ? R.primaryGradient : null,
            color: on ? null : R.whiteA(.66),
            border:
                Border.all(color: on ? Colors.transparent : R.inkA(.08)),
            borderRadius: BorderRadius.circular(R.rPill),
          ),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              if (icon != null) ...[
                Icon(icon,
                    size: 13, color: on ? Colors.white : R.inkA(.55)),
                const SizedBox(width: 5),
              ],
              Text(label,
                  style: T.plex(11.5, FontWeight.w600,
                      color: on ? Colors.white : R.inkA(.6))),
            ],
          ),
        ),
      ),
    );
  }

  Widget _type(BuildContext context, String label, String? key) {
    final on = query.type == key;
    return Padding(
      padding: const EdgeInsetsDirectional.only(end: 7),
      child: GestureDetector(
        onTap: () => onChanged(key == null
            ? query.copyWith(clearType: true)
            : query.copyWith(type: key)),
        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 11, vertical: 6),
          decoration: BoxDecoration(
            color: on ? R.primaryA(.14) : R.whiteA(.6),
            border: Border.all(
                color: on ? R.primaryA(.4) : R.inkA(.07)),
            borderRadius: BorderRadius.circular(R.rPill),
          ),
          child: Text(label,
              style: T.plex(11, FontWeight.w600,
                  color: on ? R.primaryDark : R.inkA(.6))),
        ),
      ),
    );
  }
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          children: [
            Icon(Icons.cloud_off_rounded, size: 34, color: R.error),
            const SizedBox(height: 10),
            Text(message,
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w500, color: R.inkA(.7))),
            const SizedBox(height: 12),
            SecondaryButton(
                label: 'إعادة المحاولة', height: 46, onPressed: onRetry),
          ],
        ),
      );
}
