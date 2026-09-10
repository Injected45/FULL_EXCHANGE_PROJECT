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
import 'employee_header.dart';
import 'employee_session.dart';

/* ══════════════════════════════════════════════════════════════════════════
   خزينتي — مالُ الحوالات، لا عهدةَ ولا وردية
   ══════════════════════════════════════════════════════════════════════════

   تصحيحُ المالك (10 سبتمبر 2026): «طلبتُ إيقاف خدمة العهدة … ولكن أنت أخفيتَ
   حتى الخزينة وهذا خطأ. أريد إعادة تفعيل خزينة الموظف بحيث يظهر فيها قيمةُ
   الحوالات الصادرة والواردة والرصيد … ليجرد الدرج ويطابق الماليةَ بالحركة».

   فالذي أُلغي إدخالُ اليد — قبضٌ وصرفٌ يدويّان، وعهدةٌ افتتاحية، ووردية تُفتح
   وتُقفل. والذي هنا **مرآةُ حوالاته**: ما دخل الدرجَ وما خرج منه والفرق.

   ⚠ ولا رقمَ يُحسب في الهاتف: الخادمُ يجمع، والشاشةُ تعرض. حسابٌ ثانٍ هنا
   يفترق عن الأوّل عند أوّل حالة، ثمّ لا يُعرف أيُّهما الصادق.
   ══════════════════════════════════════════════════════════════════════════ */

/// نافذةُ الجرد — والدرجُ يُجرد يومياً، فالافتراضيُّ يوم.
class _Window {
  const _Window(this.days, this.label);
  final int days;
  final String label;
}

const _windows = [
  _Window(1, 'اليوم'),
  _Window(7, 'أسبوع'),
  _Window(30, 'شهر'),
];

final _cashboxProvider = FutureProvider.autoDispose
    .family<Map<String, dynamic>, int>((ref, days) async {
  try {
    final env = await ref
        .watch(apiClientProvider)
        .get('/device/employee/cashbox', query: {'days': days});
    return env.row ?? const {};
  } on ApiFailure catch (e) {
    if (e.isEmptyResult) return const {};
    rethrow;
  }
});

/// أيُّ الكشفين معروضٌ الآن.
///
/// ⚠ نصُّ الأمر يفصلهما ولا يوحّدهما، والفرقُ بينهما مقصود:
///
///   • **الصادرة** تُعرض **بالكامل** أيّاً كانت حالتُها — مسلَّمةً أو غير
///     مسلَّمة: قيمتُها دخلت الدرجَ لحظةَ إنشائها، وما بعد ذلك شأنُ الفرع
///     المستقبِل لا شأنُ درجِ هذا الموظف.
///   • **الواردة** لا تُعرض إلّا **مسلَّمةً**: المالُ لا يخرج من الدرج إلّا
///     حين يُدفع فعلاً للمستفيد. وحوالةٌ واردةٌ لم تُسلَّم بعدُ لم تمسّ درجَه.
///
/// وسجلُّ النسبة يقول ذلك بنفسه: `CREATED` صادرةٌ بأيّ حال، و`DELIVERED`
/// لا تُكتب إلّا بعد تسليمٍ وقع. فالفرزُ قراءةٌ لا شرطٌ يُضاف.
enum _Ledger { outgoing, incoming }

class EmployeeCashboxScreen extends ConsumerStatefulWidget {
  const EmployeeCashboxScreen({super.key, this.asTab = false});

  /// تبويبٌ في الشريط السفليّ — فبلا زرّ رجوع، وبترويسة الموظف فوقه.
  final bool asTab;

  @override
  ConsumerState<EmployeeCashboxScreen> createState() =>
      _EmployeeCashboxScreenState();
}

class _EmployeeCashboxScreenState extends ConsumerState<EmployeeCashboxScreen> {
  int _days = 1;
  _Ledger _ledger = _Ledger.outgoing;

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(_cashboxProvider(_days));
    final p = ref.watch(employeeAuthProvider).profile;

    return Screen(
      child: Column(
        children: [
          if (widget.asTab && p != null)
            EmployeeHeader(profile: p)
          else
            RhallaAppBar(title: 'خزينتي', onBack: () => context.pop()),
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 12, R.padScreen, 0),
            child: Row(
              children: [
                for (final w in _windows) ...[
                  if (w != _windows.first) const SizedBox(width: 8),
                  Expanded(
                    child: GestureDetector(
                      onTap: () => setState(() => _days = w.days),
                      child: AnimatedContainer(
                        duration: const Duration(milliseconds: 180),
                        padding: const EdgeInsets.symmetric(vertical: 11),
                        decoration: BoxDecoration(
                          gradient: w.days == _days ? R.primaryGradient : null,
                          color: w.days == _days ? null : R.whiteA(.66),
                          border: Border.all(
                              color: w.days == _days
                                  ? Colors.transparent
                                  : R.inkA(.08)),
                          borderRadius: BorderRadius.circular(R.rPill),
                        ),
                        child: Text(
                          w.label,
                          textAlign: TextAlign.center,
                          style: T.plex(12, FontWeight.w600,
                              color: w.days == _days
                                  ? Colors.white
                                  : R.inkA(.6)),
                        ),
                      ),
                    ),
                  ),
                ],
              ],
            ),
          ),
          Expanded(
            child: RefreshIndicator(
              onRefresh: () async => ref.invalidate(_cashboxProvider(_days)),
              color: R.primary,
              backgroundColor: Colors.white,
              child: async.when(
                loading: () =>
                    Center(child: CircularProgressIndicator(color: R.primary)),
                error: (e, _) => _Failed(
                  message: '$e',
                  onRetry: () => ref.invalidate(_cashboxProvider(_days)),
                ),
                data: (d) => _Body(
                  d: d,
                  ledger: _ledger,
                  onLedger: (l) => setState(() => _ledger = l),
                  bottomPad: widget.asTab ? 110 : 40,
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}

class _Body extends StatelessWidget {
  const _Body({
    required this.d,
    required this.ledger,
    required this.onLedger,
    required this.bottomPad,
  });

  final Map<String, dynamic> d;
  final _Ledger ledger;
  final ValueChanged<_Ledger> onLedger;
  final double bottomPad;

  @override
  Widget build(BuildContext context) {
    final all = ((d['items'] as List?) ?? const [])
        .whereType<Map>()
        .map((e) => e.cast<String, dynamic>())
        .toList();

    // ⚠ الفرزُ على `direction` كما كتبه الخادم، لا استنتاجاً من المبلغ أو
    // من الاسم: صفٌّ ينقصه حقلٌ كان سيُصنَّف بما وقع فيه صدفةً.
    final wanted = ledger == _Ledger.outgoing ? 'IN' : 'OUT';
    final items = all.where((m) => '${m['direction']}' == wanted).toList();

    final balance = Fmt.num_(d['balance']);
    // ⚠ سالبٌ = دفع من ماله أكثر ممّا قبض، فالوكيلُ مدينٌ له. والإشارةُ تقلب
    // الجملة لا الرقم: ناقصٌ عارٍ يُقرأ عجزاً وهو دائنٌ لا مدين.
    final owed = balance >= 0;

    return ListView(
      padding: EdgeInsets.fromLTRB(R.padScreen, 14, R.padScreen, bottomPad),
      physics: const AlwaysScrollableScrollPhysics(),
      children: [
        Container(
          padding: const EdgeInsets.fromLTRB(20, 18, 20, 18),
          decoration: BoxDecoration(
            gradient: R.primaryGradient,
            borderRadius: BorderRadius.circular(R.rActions),
            boxShadow: R.shNav,
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(owed ? 'المفترض في درجك' : 'مستحقٌّ لك',
                  style: T.plex(12, FontWeight.w500, color: R.whiteA(.86))),
              const SizedBox(height: 8),
              // نفسُ سُلَّم رصيد الوكيل: 15 · 30 · 19.
              Directionality(
                textDirection: TextDirection.ltr,
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.baseline,
                  textBaseline: TextBaseline.alphabetic,
                  children: [
                    Text('د.ل',
                        style:
                            T.plex(15, FontWeight.w600, color: R.whiteA(.86))),
                    const SizedBox(width: 7),
                    Text(Fmt.money(balance.abs()).split('.').first,
                        style: T.kufi(30, FontWeight.w700, color: Colors.white)),
                    const SizedBox(width: 2),
                    Text('.${Fmt.money(balance.abs()).split('.').last}',
                        style:
                            T.kufi(19, FontWeight.w600, color: R.whiteA(.88))),
                  ],
                ),
              ),
              const SizedBox(height: 10),
              /*
               * ⚠ طرفا المعادلة تحت الرقم — إجابةٌ على السؤال الذي يليه دائماً:
               * «من أين جاء هذا الرقم؟». وبها يُجرد الدرجُ بلا فتح كشفٍ كامل.
               */
              Directionality(
                textDirection: TextDirection.ltr,
                child: Text(
                  '${Fmt.money(Fmt.num_(d['in']))} '
                  '− ${Fmt.money(Fmt.num_(d['out']))}',
                  style: T.plex(11.5, FontWeight.w400, color: R.whiteA(.62)),
                ),
              ),
            ],
          ),
        ),
        const SizedBox(height: R.gapCard),

        Row(
          children: [
            Expanded(
              child: _Stat(
                label: 'حوالات صادرة',
                hint: 'قبضتَ قيمتها',
                value: Fmt.num_(d['in']),
                count: int.tryParse('${d['in_count'] ?? 0}') ?? 0,
                tone: R.primaryDark,
                icon: Icons.south_west_rounded,
              ),
            ),
            const SizedBox(width: 10),
            Expanded(
              child: _Stat(
                label: 'حوالات واردة',
                hint: 'سلَّمتَ قيمتها',
                value: Fmt.num_(d['out']),
                count: int.tryParse('${d['out_count'] ?? 0}') ?? 0,
                tone: R.error,
                icon: Icons.north_east_rounded,
              ),
            ),
          ],
        ),

        const SizedBox(height: R.gapCard),

        /*
         * ══════════════════════════════════════════════════════════════
         *  كشفان لا كشفٌ واحد — نصُّ الأمر (10 سبتمبر 2026)
         * ══════════════════════════════════════════════════════════════
         *
         * «الخزينة: كشفُ حركة الحوالات الصادرة بالكامل بغضّ النظر عن حالتها،
         *  وكشفُ حركة الحوالات الواردة المسلَّمة فقط».
         *
         * ⚠ والفرقُ بين السطرين مقصودٌ ولا يُوحَّد — انظر [_Ledger].
         */
        _LedgerSwitch(
          current: ledger,
          onPick: onLedger,
          outCount: int.tryParse('${d['in_count'] ?? 0}') ?? 0,
          inCount: int.tryParse('${d['out_count'] ?? 0}') ?? 0,
        ),
        const SizedBox(height: 8),
        Text(
          ledger == _Ledger.outgoing
              ? 'كلُّ ما أنشأتَه في هذه المدّة — مسلَّماً كان أو غيرَ مسلَّم.'
              : 'ما سلَّمتَه فعلاً في هذه المدّة — والوارد غيرُ المسلَّم لم '
                  'يمسّ درجَك بعد.',
          style: T.plex(11, FontWeight.w400, color: R.inkA(.5), height: 1.6),
        ),
        const SizedBox(height: 12),

        if (items.isEmpty)
          GlassCard(
            child: Column(
              children: [
                Icon(Icons.savings_outlined, size: 34, color: R.inkA(.28)),
                const SizedBox(height: 12),
                Text(
                    ledger == _Ledger.outgoing
                        ? 'لا حوالات صادرة في هذه المدّة.'
                        : 'لم تُسلِّم حوالةً واردة في هذه المدّة.',
                    textAlign: TextAlign.center,
                    style: T.plex(13, FontWeight.w500,
                        color: R.inkA(.6), height: 1.7)),
              ],
            ),
          )
        else
          for (var i = 0; i < items.length; i++) ...[
            if (i > 0) const SizedBox(height: R.gapRow),
            _MoveRow(m: items[i]),
          ],
      ],
    );
  }
}

/// مبدّلُ الكشفين — الصادرة · الواردة المسلَّمة.
///
/// ⚠ ويحمل عدَدَ كلٍّ منهما: الموظف يعرف قبل التبديل هل في الآخر شيءٌ
/// أصلاً، فلا يُبدّل إلى كشفٍ فارغ ثمّ يعود.
class _LedgerSwitch extends StatelessWidget {
  const _LedgerSwitch({
    required this.current,
    required this.onPick,
    required this.outCount,
    required this.inCount,
  });

  final _Ledger current;
  final ValueChanged<_Ledger> onPick;

  /// عددُ ما أنشأه — أي صفوفُ كشف «الصادرة».
  final int outCount;

  /// عددُ ما سلّمه — أي صفوفُ كشف «الواردة».
  final int inCount;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          Expanded(
            child: _LedgerTab(
              label: 'كشف الصادرة',
              count: outCount,
              active: current == _Ledger.outgoing,
              onTap: () => onPick(_Ledger.outgoing),
            ),
          ),
          const SizedBox(width: 8),
          Expanded(
            child: _LedgerTab(
              label: 'كشف الواردة',
              count: inCount,
              active: current == _Ledger.incoming,
              onTap: () => onPick(_Ledger.incoming),
            ),
          ),
        ],
      );
}

class _LedgerTab extends StatelessWidget {
  const _LedgerTab({
    required this.label,
    required this.count,
    required this.active,
    required this.onTap,
  });

  final String label;
  final int count;
  final bool active;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => GestureDetector(
        onTap: onTap,
        child: AnimatedContainer(
          duration: const Duration(milliseconds: 180),
          padding: const EdgeInsets.symmetric(vertical: 11, horizontal: 10),
          decoration: BoxDecoration(
            color: active ? R.primaryA(.12) : R.whiteA(.66),
            border: Border.all(
                color: active ? R.primaryA(.45) : R.inkA(.08),
                width: active ? 1.4 : 1),
            borderRadius: BorderRadius.circular(R.rPill),
          ),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Flexible(
                child: Text(label,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: T.plex(12,
                        active ? FontWeight.w700 : FontWeight.w500,
                        color: active ? R.primaryDark : R.inkA(.6))),
              ),
              const SizedBox(width: 6),
              Directionality(
                // رقمٌ لاتينيّ في فقرةٍ عربية — يُفرض اتجاهه.
                textDirection: TextDirection.ltr,
                child: Text('$count',
                    style: T.plex(11, FontWeight.w700,
                        color: active ? R.primaryDark : R.inkA(.45))),
              ),
            ],
          ),
        ),
      );
}

class _Stat extends StatelessWidget {
  const _Stat({
    required this.label,
    required this.hint,
    required this.value,
    required this.count,
    required this.tone,
    required this.icon,
  });

  final String label;
  final String hint;
  final double value;
  final int count;
  final Color tone;
  final IconData icon;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(icon, size: 15, color: tone),
                const SizedBox(width: 6),
                Expanded(
                  child: Text(label,
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      style: T.kufi(12.5, FontWeight.w700)),
                ),
              ],
            ),
            const SizedBox(height: 3),
            Text(hint,
                style: T.plex(10.5, FontWeight.w400, color: R.inkA(.5))),
            const SizedBox(height: 9),
            Directionality(
              textDirection: TextDirection.ltr,
              child: Row(
                crossAxisAlignment: CrossAxisAlignment.baseline,
                textBaseline: TextBaseline.alphabetic,
                children: [
                  Text('د.ل ',
                      style:
                          T.plex(10.5, FontWeight.w500, color: R.inkA(.55))),
                  Flexible(
                    child: Text(Fmt.money(value),
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: T.kufi(15, FontWeight.w700, color: tone)),
                  ),
                ],
              ),
            ),
            const SizedBox(height: 4),
            Text('${Fmt.count(count)} حوالة',
                style: T.plex(10.5, FontWeight.w400, color: R.inkA(.5))),
          ],
        ),
      );
}

/// صفُّ حركة — اتجاهها من فعل الموظف لا من اتجاه الحوالة.
class _MoveRow extends StatelessWidget {
  const _MoveRow({required this.m});

  final Map<String, dynamic> m;

  @override
  Widget build(BuildContext context) {
    final isIn = '${m['direction'] ?? ''}' == 'IN';
    final tone = isIn ? R.primaryDark : R.error;

    return GlassRow(
      children: [
        IconTile(
          size: 34,
          background: tone.withValues(alpha: .12),
          icon: Icon(isIn ? Icons.south_west_rounded : Icons.north_east_rounded,
              size: 16, color: tone),
        ),
        const SizedBox(width: 11),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text('${m['label'] ?? ''}',
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: T.kufi(12.5, FontWeight.w700)),
              const SizedBox(height: 4),
              Directionality(
                // الرقمُ والتاريخ مقطعٌ لاتينيّ الاتجاه — وإلّا انقلب.
                textDirection: TextDirection.ltr,
                child: Align(
                  alignment: AlignmentDirectional.centerStart,
                  child: Text(
                    '${m['transfer_number'] ?? ''}'
                    '${(m['beneficiary'] ?? '').toString().isEmpty ? '' : ' · '}'
                    '${m['beneficiary'] ?? ''}',
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: T.meta,
                  ),
                ),
              ),
            ],
          ),
        ),
        const SizedBox(width: 8),
        Directionality(
          textDirection: TextDirection.ltr,
          child: Text('${isIn ? '+' : '−'} ${Fmt.money(Fmt.num_(m['amount']))}',
              style: T.kufi(13.5, FontWeight.w700, color: tone)),
        ),
      ],
    );
  }
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => ListView(
        physics: const AlwaysScrollableScrollPhysics(),
        padding: const EdgeInsets.fromLTRB(R.padScreen, 60, R.padScreen, 30),
        children: [
          Icon(Icons.wifi_off_rounded, size: 40, color: R.inkA(.3)),
          const SizedBox(height: 14),
          Text(message,
              textAlign: TextAlign.center,
              style: T.plex(13, FontWeight.w500,
                  color: R.inkA(.65), height: 1.7)),
          const SizedBox(height: 18),
          GlassButton(label: 'إعادة المحاولة', onPressed: onRetry),
        ],
      );
}
