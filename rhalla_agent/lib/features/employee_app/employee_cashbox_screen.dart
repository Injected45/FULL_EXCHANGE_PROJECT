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

class EmployeeCashboxScreen extends ConsumerStatefulWidget {
  const EmployeeCashboxScreen({super.key});

  @override
  ConsumerState<EmployeeCashboxScreen> createState() =>
      _EmployeeCashboxScreenState();
}

class _EmployeeCashboxScreenState extends ConsumerState<EmployeeCashboxScreen> {
  int _days = 1;

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(_cashboxProvider(_days));

    return Screen(
      child: Column(
        children: [
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
                data: (d) => _Body(d: d),
              ),
            ),
          ),
        ],
      ),
    );
  }
}

class _Body extends StatelessWidget {
  const _Body({required this.d});

  final Map<String, dynamic> d;

  @override
  Widget build(BuildContext context) {
    final items = ((d['items'] as List?) ?? const [])
        .whereType<Map>()
        .map((e) => e.cast<String, dynamic>())
        .toList();

    final balance = Fmt.num_(d['balance']);
    // ⚠ سالبٌ = دفع من ماله أكثر ممّا قبض، فالوكيلُ مدينٌ له. والإشارةُ تقلب
    // الجملة لا الرقم: ناقصٌ عارٍ يُقرأ عجزاً وهو دائنٌ لا مدين.
    final owed = balance >= 0;

    return ListView(
      padding: const EdgeInsets.fromLTRB(R.padScreen, 14, R.padScreen, 40),
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
        Row(
          children: [
            Text('الحركة', style: T.section),
            const SizedBox(width: 8),
            Text('${items.length}',
                style: T.plex(12, FontWeight.w700, color: R.inkA(.5))),
          ],
        ),
        const SizedBox(height: 10),

        if (items.isEmpty)
          GlassCard(
            child: Column(
              children: [
                Icon(Icons.savings_outlined, size: 34, color: R.inkA(.28)),
                const SizedBox(height: 12),
                Text('لا حركة في هذه المدّة.',
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
