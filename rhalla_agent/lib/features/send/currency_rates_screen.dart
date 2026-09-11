import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../transfers/agent_incoming_repository.dart';

/// **أسعارُ العملات** — شاشةُ عرضٍ فقط.
///
/// ══════════════════════════════════════════════════════════════════════════
///  أمرُ المالك (11 سبتمبر 2026)
/// ══════════════════════════════════════════════════════════════════════════
///
/// «شاشةٌ جديدة داخل قسم الحوالات الخارجية باسم أسعار العملات … تعرض لكلّ
///  عملة: الدولة، اسم العملة، سعر الصرف، رمز العملة … بشكلٍ احترافيّ منظّم
///  يشبه لوحاتِ أسعار العملات في تطبيقات المصارف … **Read Only بالكامل**، ولا
///  زرَّ تعديلٍ أو إضافةٍ أو حذف، ولا يُسمح للوكيل أو الموظف بتغيير الأسعار».
///
/// ── لا نسخةَ أسعارٍ في التطبيق ─────────────────────────────────────────
///
/// ⚠ لا تخزينَ ولا ذاكرةَ دائمة: المزوّدُ `autoDispose` يقرأ من الخادم عند كلّ
/// فتح، والخادمُ يقرأ من جداول المنظومة مباشرةً. فأيُّ تعديلٍ في المكتب
/// الخلفيّ يظهر هنا **عند أوّل فتح** — ولا يوجد رقمٌ مخزَّنٌ يتقادم.
///
/// ⚠ ولا زرَّ يكتب: لا نقطةَ كتابةٍ لهذه الشاشة في الخادم أصلاً.
///
/// ── ولماذا الخدمةُ سطرٌ مستقلّ ──────────────────────────────────────────
///
/// ⚠ لأنّ السعرَ يختلف بها **في القاعدة نفسِها**، وقيسَ: مصر ٥٫٥٥٠ لحوالات
/// البريد وفودافون والتسليم باليد، و٥٫٣٠٠ للحوالة البنكية وإنستا باي. وتونس
/// ٣٫٢٢٥ للتسليم باليد و٢٫٥٠٠ للبريد.
///
/// فسطرٌ واحدٌ لكلّ دولة كان سيعرض رقماً **خاطئاً لأربعة أخماس الخدمات** —
/// ويُسعَّر به زبون.
class CurrencyRatesScreen extends ConsumerWidget {
  const CurrencyRatesScreen({super.key, this.mode = TransfersMode.agent});

  /// أيُّ بابٍ يُقرأ منه — والقراءةُ في الخادم واحدةٌ للاثنين.
  final TransfersMode mode;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(currencyRatesProvider(mode));

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'أسعار العملات',
            subtitle: 'الأسعار المعتمدة — عرض فقط',
            onBack: () => Navigator.of(context).pop(),
          ),
          Expanded(
            child: RefreshIndicator(
              onRefresh: () async => ref.invalidate(currencyRatesProvider(mode)),
              color: R.primary,
              backgroundColor: Colors.white,
              child: async.when(
                loading: () =>
                    Center(child: CircularProgressIndicator(color: R.primary)),
                error: (e, _) => ListView(
                  physics: const AlwaysScrollableScrollPhysics(),
                  children: [
                  _Msg(
                    icon: Icons.wifi_off_rounded,
                    text: 'تعذّر تحميل الأسعار.\n$e',
                  ),
                ]),
                data: (groups) => groups.isEmpty
                    ? ListView(
                        physics: const AlwaysScrollableScrollPhysics(),
                        children: const [
                          _Msg(
                            icon: Icons.currency_exchange_rounded,
                            text: 'لا أسعارَ معتمدةً بعد.',
                          ),
                        ],
                      )
                    : ListView.separated(
                        padding: const EdgeInsets.fromLTRB(
                            R.padScreen, 14, R.padScreen, 110),
                        physics: const AlwaysScrollableScrollPhysics(),
                        itemCount: groups.length + 1,
                        separatorBuilder: (_, _) =>
                            const SizedBox(height: R.gapCard),
                        itemBuilder: (_, i) => i == groups.length
                            ? const _ReadOnlyNote()
                            : _CountryCard(g: groups[i]),
                      ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}

/* ───────────────── البطاقة ───────────────── */

/// بطاقةُ دولةٍ واحدة — ترويسةٌ فيها العملةُ ورمزُها، وتحتها سعرٌ لكلّ خدمة.
class _CountryCard extends StatelessWidget {
  const _CountryCard({required this.g});
  final RateGroup g;

  @override
  Widget build(BuildContext context) => GlassCard(
        padding: const EdgeInsets.fromLTRB(16, 14, 16, 14),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                IconTile(
                  size: 40,
                  background: R.primaryA(.12),
                  // ⚠ رمزُ العملة داخل الدائرة: هو أسرعُ ما تلتقطه العينُ في
                  // لوحةِ أسعار، وأقصرُ من اسم العملة فلا يُقصّ.
                  icon: Text(g.code.isEmpty ? '—' : g.code,
                      style: T.kufi(12.5, FontWeight.w800,
                          color: R.primaryDark)),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      // ١) الدولة  ٢) اسمُ العملة — بالترتيب الذي طلبه الأمر.
                      Text(g.country,
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style: T.kufi(15, FontWeight.w800)),
                      const SizedBox(height: 2),
                      Text(g.currency,
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style: T.plex(12, FontWeight.w400,
                              color: R.inkA(.58))),
                    ],
                  ),
                ),
              ],
            ),
            const SizedBox(height: 12),
            Divider(color: R.inkA(.08), height: 1),
            const SizedBox(height: 10),

            for (var i = 0; i < g.rows.length; i++) ...[
              if (i > 0) const SizedBox(height: 9),
              _RateRow(row: g.rows[i], code: g.code),
            ],
          ],
        ),
      );
}

/// سطرُ سعرٍ واحد — الخدمةُ يميناً والسعرُ يساراً.
class _RateRow extends StatelessWidget {
  const _RateRow({required this.row, required this.code});

  final RateRow row;
  final String code;

  @override
  Widget build(BuildContext context) => Row(
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          Icon(Icons.chevron_left_rounded, size: 16, color: R.inkA(.28)),
          const SizedBox(width: 4),
          Expanded(
            child: Text(
              row.service?.isNotEmpty == true ? row.service! : 'السعر المعتمد',
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
              style: T.plex(12.5, FontWeight.w500, color: R.inkA(.68)),
            ),
          ),
          const SizedBox(width: 10),
          /*
           * ٣) سعرُ الصرف  ٤) رمزُ العملة — وبهذا الترتيب.
           *
           * ⚠ باتجاهٍ لاتينيّ مفروض: الرقمُ مقطعٌ لاتينيّ في فقرةٍ عربية،
           * وبغيره تقلبه الفقرة فيُقرأ معكوساً — وهذا سعرٌ يُسعَّر به زبون.
           *
           * ⚠ و`Fmt.rate` بأربع خانات: السعرُ يحمل كسوراً تفرق في المبالغ
           * الكبيرة (٥٫٣ مقابل ٥٫٥٥)، وتقريبُه إلى خانتين يُخفي فرقاً حقيقياً.
           */
          Directionality(
            textDirection: TextDirection.ltr,
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.baseline,
              textBaseline: TextBaseline.alphabetic,
              children: [
                Text(row.rate == null ? '—' : Fmt.rate(row.rate!),
                    style: T.kufi(15.5, FontWeight.w800, color: R.ink)),
                const SizedBox(width: 5),
                Text(code,
                    style: T.plex(10.5, FontWeight.w500, color: R.inkA(.5))),
              ],
            ),
          ),
        ],
      );
}

/// سطرُ التذكير بأنّ الشاشة للعرض — يُقال صراحةً لا يُترك للاستنتاج.
class _ReadOnlyNote extends StatelessWidget {
  const _ReadOnlyNote();

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(13),
        decoration: BoxDecoration(
          color: R.whiteA(.5),
          border: Border.all(color: R.inkA(.08)),
          borderRadius: BorderRadius.circular(R.rCard),
        ),
        child: Row(
          children: [
            Icon(Icons.lock_outline_rounded, size: 16, color: R.inkA(.42)),
            const SizedBox(width: 9),
            Expanded(
              child: Text(
                'الأسعار تُعتمد من إدارة الرحالة، وتظهر هنا كما هي في المنظومة. '
                'لا تُعدَّل من التطبيق.',
                style: T.plex(11.5, FontWeight.w400,
                    color: R.inkA(.55), height: 1.7),
              ),
            ),
          ],
        ),
      );
}

class _Msg extends StatelessWidget {
  const _Msg({required this.icon, required this.text});
  final IconData icon;
  final String text;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.fromLTRB(30, 60, 30, 30),
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

/* ───────────────── الطراز والمزوّد ───────────────── */

/// سعرُ خدمةٍ واحدة داخل دولة.
class RateRow {
  const RateRow({required this.service, required this.rate});

  final String? service;
  final double? rate;
}

/// أسعارُ دولةٍ واحدة — عملتُها ورمزُها، وأسعارُها بحسب الخدمة.
class RateGroup {
  const RateGroup({
    required this.country,
    required this.currency,
    required this.code,
    required this.rows,
  });

  final String country;
  final String currency;
  final String code;
  final List<RateRow> rows;
}

/// أسعارُ العملات — **نقطةٌ واحدة بمسارين**، وقراءةٌ واحدة في الخادم.
///
/// ⚠ `autoDispose` عمداً: لا نسخةَ تبقى في الذاكرة بعد إغلاق الشاشة، فلا
/// يُعرض سعرٌ قديمٌ بعد تعديلٍ في المكتب الخلفيّ.
final currencyRatesProvider =
    FutureProvider.autoDispose.family<List<RateGroup>, TransfersMode>(
        (ref, mode) async {
  final path = mode.isEmployee
      ? '/device/employee/currency-rates'
      : '/device/currency-rates';

  try {
    final env = await ref.watch(apiClientProvider).get(path);
    final items = ((env.row?['items'] as List?) ?? const [])
        .whereType<Map>()
        .map((e) => e.cast<String, dynamic>())
        .toList();

    /*
     * التجميعُ بالدولة في الهاتف — والخادمُ يعيدها مرتّبةً بها أصلاً.
     *
     * ⚠ وهو **تجميعُ عرضٍ لا حساب**: لا رقمَ يُجمع ولا يُتوسَّط. كلُّ سعرٍ
     * يُعرض كما جاء.
     */
    final groups = <String, RateGroup>{};
    for (final m in items) {
      final key = '${m['country_id']}';
      final g = groups[key];
      final row = RateRow(
        service: (m['service'] as String?)?.trim(),
        rate: m['rate'] == null ? null : Fmt.num_(m['rate']),
      );

      if (g == null) {
        groups[key] = RateGroup(
          country: '${m['country'] ?? ''}'.trim(),
          currency: '${m['currency'] ?? ''}'.trim(),
          code: '${m['code'] ?? ''}'.trim(),
          rows: [row],
        );
      } else {
        g.rows.add(row);
      }
    }

    return groups.values.toList();
  } on ApiFailure catch (e) {
    if (e.isEmptyResult) return const [];
    rethrow;
  }
});
