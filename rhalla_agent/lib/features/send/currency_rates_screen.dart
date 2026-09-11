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

/// **أسعارُ العملات** — لوحةُ الأسعار المعتمدة، عرضاً فقط.
///
/// ══════════════════════════════════════════════════════════════════════════
///  أمرُ المالك (11 سبتمبر 2026)
/// ══════════════════════════════════════════════════════════════════════════
///
/// «شاشةٌ داخل الحوالات الخارجية باسم أسعار العملات … الدولة، اسم العملة،
///  سعر الصرف، رمز العملة … بشكلٍ احترافيّ منظّم يشبه لوحاتِ أسعار العملات في
///  تطبيقات المصارف … **Read Only بالكامل**».
///
/// ثمّ: «تبني شاشةَ عرض الأسعار على أعلى كفاءة … على درايةٍ بالأسعار وفروقات
///  العملات وطريقةِ الاحتساب للعملات الأقوى من العملة الليبية مثل الدولار
///  واليورو والدينار التونسي، والأضعف مثل الجنيه المصري والدينار السوداني».
///
/// ══════════════════════════════════════════════════════════════════════════
///  ⚠⚠ القرارُ الأوّل: الرقمُ المجرَّد ممنوع — كلُّ سعرٍ يُقرأ باتجاهه
/// ══════════════════════════════════════════════════════════════════════════
///
/// «5.5500» وحدَه لا معنى له: أهو جنيهاتٌ للدينار أم دنانيرُ للجنيه؟ والفرقُ
/// بينهما ثلاثون ضعفاً في جيب زبون. فيُكتب كما يُكتب في لوحات الصرف:
///
///     ‎1 د.ل = 5.5500 ج.م          ← الاتجاهُ المنفَّذ
///     ‎1 ج.م = 0.1802 د.ل          ← المقلوب، للتحقّق السريع
///
/// ⚠ **والاتجاهُ المعروض هو اتجاهُ التنفيذ حرفاً بحرف**، لا اجتهاداً في
/// العرض: المنظومةُ تحسب `المسلَّم = المبلغ بالدينار × السعر` في المحفّز وفي
/// `SalePrice_mo_Value` معاً. فأيُّ قلبٍ للاتجاه في الشاشة يعيد بالضبط العطبَ
/// الذي بلّغ عنه المالك — سعرٌ يُعرض وآخرُ يُنفَّذ.
///
/// ── وهنا موضعُ سؤال «الأقوى والأضعف» ────────────────────────────────────
///
/// المنظومةُ تحمل علماً اسمه `CurrencyPower` (تونس = 1، ومصر والسودان وتشاد = 0).
/// وهو يُعرض هنا شارةً **ولا يُحسب به**: لا المحفّزُ ولا الدالّةُ يقرآنه،
/// كلاهما يضرب ضرباً مهما كانت قيمتُه. فاستعمالُه لقلب اتجاه العرض كان
/// سيُظهر للوكيل رقماً لا يُنفَّذ.
///
/// وما يحلّ المسألةَ فعلاً هو **المقلوب**: العملةُ الأضعف يُقرأ اتجاهُها
/// المباشر بيسر (1 د.ل = 5.55 ج.م)، والأقوى يصير المقلوبُ هو المفهوم فيها —
/// وكلاهما معروضٌ، فلا يحتاج الوكيلُ إلى حسابٍ ذهنيّ أمام زبون.
///
/// ══════════════════════════════════════════════════════════════════════════
///  ⚠⚠ القرارُ الثاني: اللوحةُ تسرد الخدماتِ المعروضة، لا الأسعارَ الموجودة
/// ══════════════════════════════════════════════════════════════════════════
///
/// لوحةٌ تعرض ما له سعرٌ تُخفي أخطرَ سطرٍ فيها: خدمةً يراها الوكيل في شاشة
/// الإنشاء **ولا سعرَ لها**. يختارها أمام زبونٍ فتُردّ.
///
/// فالخادمُ يبدأ من الخدمات المعروضة ويصل السعرَ إليها، وتظهر هنا بحالتها:
/// «معتمد» · «لا سعر معتمد» · «سعرٌ مكرَّر». وهي **نفسُ** حالات الحارس الذي
/// يمنع التحويل في الخادم — فما تقوله الشاشةُ هو ما يفعله الخادم، لا وعدٌ
/// يخالفه.
///
/// ══════════════════════════════════════════════════════════════════════════
///  ⚠ ولا نسخةَ أسعارٍ ولا زرَّ تعديل
/// ══════════════════════════════════════════════════════════════════════════
///
/// المزوّدُ `autoDispose` يقرأ عند كلّ فتح، والخادمُ يقرأ جداولَ المنظومة
/// مباشرة. ولا `POST` ولا `PUT` ولا `DELETE` لهذه الشاشة في الخادم أصلاً.
class CurrencyRatesScreen extends ConsumerStatefulWidget {
  const CurrencyRatesScreen({super.key, this.mode = TransfersMode.agent});

  /// أيُّ بابٍ يُقرأ منه — والقراءةُ في الخادم واحدةٌ للاثنين.
  final TransfersMode mode;

  @override
  ConsumerState<CurrencyRatesScreen> createState() =>
      _CurrencyRatesScreenState();
}

class _CurrencyRatesScreenState extends ConsumerState<CurrencyRatesScreen> {
  final _search = TextEditingController();
  String _q = '';

  @override
  void dispose() {
    _search.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(currencyRatesProvider(widget.mode));

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'أسعار العملات',
            subtitle: 'الأسعار المعتمدة للتحويل — عرض فقط',
            onBack: () => Navigator.of(context).pop(),
          ),
          Expanded(
            child: RefreshIndicator(
              onRefresh: () async =>
                  ref.invalidate(currencyRatesProvider(widget.mode)),
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
                  ],
                ),
                data: _board,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _board(List<RateGroup> all) {
    /*
     * البحثُ يشمل الدولة والعملة ورمزَها واسمَ الخدمة.
     *
     * ⚠ ويُبقي **الصفوفَ المطابقة** لا الدولةَ بكاملها: وكيلٌ يكتب «فودافون»
     * يريد سطرَ فودافون، لا مصرَ بخدماتها الخمس. والعكسُ صحيح: من كتب «مصر»
     * يريدها كلَّها.
     */
    final q = _q.trim();
    final groups = q.isEmpty
        ? all
        : all.map((g) => g.filtered(q)).whereType<RateGroup>().toList();

    return ListView(
      padding: const EdgeInsets.fromLTRB(R.padScreen, 12, R.padScreen, 110),
      physics: const AlwaysScrollableScrollPhysics(),
      children: [
        _SearchBox(
          controller: _search,
          onChanged: (v) => setState(() => _q = v),
        ),
        const SizedBox(height: R.gapCard),

        if (groups.isEmpty)
          _Msg(
            icon: q.isEmpty
                ? Icons.currency_exchange_rounded
                : Icons.search_off_rounded,
            text: q.isEmpty ? 'لا أسعارَ معتمدةً بعد.' : 'لا نتيجةَ لـ«$q».',
          )
        else
          for (final g in groups) ...[
            _CountryCard(g: g),
            const SizedBox(height: R.gapCard),
          ],

        if (groups.isNotEmpty) const _BoardNote(),
      ],
    );
  }
}

/* ───────────────── البحث ───────────────── */

class _SearchBox extends StatelessWidget {
  const _SearchBox({required this.controller, required this.onChanged});

  final TextEditingController controller;
  final ValueChanged<String> onChanged;

  @override
  Widget build(BuildContext context) => Container(
        decoration: BoxDecoration(
          color: R.whiteA(.72),
          border: Border.all(color: R.whiteA(.9)),
          borderRadius: BorderRadius.circular(R.rRow),
        ),
        child: TextField(
          controller: controller,
          onChanged: onChanged,
          textInputAction: TextInputAction.search,
          style: T.plex(13.5, FontWeight.w500),
          decoration: InputDecoration(
            isDense: true,
            border: InputBorder.none,
            contentPadding:
                const EdgeInsets.symmetric(horizontal: 14, vertical: 14),
            hintText: 'ابحث بدولة أو عملة أو خدمة',
            hintStyle: T.plex(13, FontWeight.w400, color: R.inkA(.42)),
            prefixIcon: Icon(Icons.search_rounded, size: 19, color: R.inkA(.4)),
            suffixIcon: controller.text.isEmpty
                ? null
                : IconButton(
                    icon:
                        Icon(Icons.close_rounded, size: 17, color: R.inkA(.45)),
                    onPressed: () {
                      controller.clear();
                      onChanged('');
                    },
                  ),
          ),
        ),
      );
}

/* ───────────────── البطاقة ───────────────── */

/// بطاقةُ دولةٍ واحدة — ترويسةُ العملة، ثمّ سطرٌ لكلّ خدمة.
class _CountryCard extends StatelessWidget {
  const _CountryCard({required this.g});
  final RateGroup g;

  @override
  Widget build(BuildContext context) => GlassCard(
        padding: const EdgeInsets.fromLTRB(15, 14, 15, 12),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                IconTile(
                  size: 42,
                  background: R.primaryA(.11),
                  // رمزُ العملة داخل الدائرة: أسرعُ ما تلتقطه العينُ في لوحةِ
                  // أسعار، وأقصرُ من اسم العملة فلا يُقصّ.
                  icon: Padding(
                    padding: const EdgeInsets.symmetric(horizontal: 3),
                    child: FittedBox(
                      child: Text(
                        g.code.isEmpty ? '—' : g.code,
                        style:
                            T.kufi(12, FontWeight.w800, color: R.primaryDark),
                      ),
                    ),
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      // ١) الدولة  ٢) اسمُ العملة — بترتيب الأمر.
                      Text(g.country,
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style: T.kufi(15.5, FontWeight.w800)),
                      const SizedBox(height: 2),
                      Text(g.currency.isEmpty ? '—' : g.currency,
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style:
                              T.plex(12, FontWeight.w400, color: R.inkA(.58))),
                    ],
                  ),
                ),
                if (g.isStrong) ...[
                  const SizedBox(width: 8),
                  const _StrongBadge(),
                ],
              ],
            ),
            const SizedBox(height: 10),
            Divider(color: R.inkA(.08), height: 1),

            for (var i = 0; i < g.rows.length; i++) ...[
              if (i > 0) Divider(color: R.inkA(.05), height: 1),
              _RateRow(row: g.rows[i], code: g.code),
            ],
          ],
        ),
      );
}

/// شارةُ «عملةٌ أقوى من الدينار» — من عَلَم المنظومة `CurrencyPower`.
///
/// ⚠ تُعرض ولا يُحسب بها: انظر ترويسة [CurrencyRatesScreen].
class _StrongBadge extends StatelessWidget {
  const _StrongBadge();

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 9, vertical: 5),
        decoration: BoxDecoration(
          color: R.primaryA(.1),
          borderRadius: BorderRadius.circular(R.rPill),
        ),
        child: Text('أقوى من الدينار',
            style: T.plex(9.5, FontWeight.w700, color: R.primaryDark)),
      );
}

/// سطرُ خدمةٍ واحدة — اسمُها وحالتُها، والسعرُ باتجاهيه.
class _RateRow extends StatelessWidget {
  const _RateRow({required this.row, required this.code});

  final RateRow row;
  final String code;

  @override
  Widget build(BuildContext context) {
    final priced = row.rate != null && row.status == 'PRICED';

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 11),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  row.service?.isNotEmpty == true
                      ? row.service!
                      : 'السعر المعتمد',
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                  style: T.plex(13, FontWeight.w600, color: R.inkA(.82)),
                ),
                const SizedBox(height: 5),
                if (priced)
                  _Freshness(day: row.pricedOn, stale: row.isStale)
                else
                  _StatusChip(status: row.status),
              ],
            ),
          ),
          const SizedBox(width: 10),
          if (priced)
            Column(
              crossAxisAlignment: CrossAxisAlignment.end,
              children: [
                /*
                 * ⚠ الاتجاهُ المنفَّذ — والسطرُ كلُّه بترتيبٍ لاتينيّ مفروض.
                 *
                 * «1 د.ل = 5.5500 ج.م» مقطعٌ مختلطُ الاتجاه داخل فقرةٍ عربية،
                 * وبلا `Directionality` تقلبه الفقرةُ فيُقرأ معكوساً — سعرٌ
                 * يُسعَّر به زبون.
                 *
                 * ⚠ و`tabularFigures`: اللوحةُ تُقرأ عمودياً، وأرقامٌ متفاوتةُ
                 * العرض تُزحزح الفواصلَ العشرية سطراً عن سطر فيصعب التقاطُ
                 * الفرق بين 5.3000 و5.5500 بنظرةٍ واحدة.
                 */
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Row(
                    crossAxisAlignment: CrossAxisAlignment.baseline,
                    textBaseline: TextBaseline.alphabetic,
                    children: [
                      Text('1 د.ل =',
                          style:
                              T.plex(10.5, FontWeight.w500, color: R.inkA(.45))),
                      const SizedBox(width: 5),
                      Text(Fmt.rate(row.rate!),
                          style: T.kufi(16.5, FontWeight.w800, color: R.ink)
                              .copyWith(fontFeatures: const [
                            FontFeature.tabularFigures()
                          ])),
                      const SizedBox(width: 4),
                      Text(code,
                          style:
                              T.plex(11, FontWeight.w600, color: R.inkA(.55))),
                    ],
                  ),
                ),
                const SizedBox(height: 3),
                /*
                 * المقلوب — للتحقّق السريع، وهو **الاتجاهُ المفهوم** حين تكون
                 * العملةُ أقوى من الدينار. انظر ترويسة الشاشة.
                 */
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(
                    '1 $code = ${inverseRate(row.rate!)} د.ل',
                    style: T.plex(10.5, FontWeight.w400, color: R.inkA(.45))
                        .copyWith(fontFeatures: const [
                      FontFeature.tabularFigures()
                    ]),
                  ),
                ),
              ],
            )
          else
            Padding(
              padding: const EdgeInsets.only(top: 2),
              child:
                  Text('—', style: T.kufi(16, FontWeight.w700, color: R.inkA(.22))),
            ),
        ],
      ),
    );
  }
}

/// مقلوبُ السعر بأربع خاناتٍ **معنوية**، لا بأربع خاناتٍ عشرية.
///
/// ⚠ السببُ عمليّ لا جماليّ: مقلوبُ 555.555 هو 0.0018، ومقلوبُ 3.225 هو
/// 0.3100775. وتثبيتُ أربعِ خاناتٍ عشرية يُفقِد الأوّلَ دقّتَه كلَّها ويُبقي
/// الثانيَ سليماً — فيُعرض رقمان بدقّتين مختلفتين في العمود نفسِه، وهو أسوأُ
/// من ألّا يُعرض المقلوبُ أصلاً.
///
/// ⚠ وتُقصّ الأصفارُ الزائدة: «0.1802» لا «0.180180».
///
/// عامّةٌ لا خاصّة كي يختبرها `test/` — الرقمُ هنا يُقرأ ويُسعَّر به.
String inverseRate(double rate) {
  if (rate <= 0) return '—';
  final v = 1 / rate;

  var decimals = 4;
  if (v < 1) {
    // عددُ الأصفار بين الفاصلة وأوّلِ رقمٍ دالّ، زائداً أربعاً.
    var probe = v;
    var lead = 0;
    while (probe < 0.1 && lead < 10) {
      probe *= 10;
      lead++;
    }
    decimals = lead + 4;
  }

  var s = v.toStringAsFixed(decimals);
  if (s.contains('.')) {
    s = s.replaceFirst(RegExp(r'0+$'), '');
    if (s.endsWith('.')) s = s.substring(0, s.length - 1);
  }
  return s;
}

/// تاريخُ آخر إدراجٍ للسعر — ومَن تقادم يُقال عنه.
class _Freshness extends StatelessWidget {
  const _Freshness({required this.day, required this.stale});

  final String? day;
  final bool stale;

  @override
  Widget build(BuildContext context) {
    if (day == null) return const SizedBox.shrink();

    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Icon(stale ? Icons.schedule_rounded : Icons.check_circle_outline_rounded,
            size: 12, color: stale ? R.warnIcon : R.inkA(.35)),
        const SizedBox(width: 4),
        Directionality(
          textDirection: TextDirection.ltr,
          child: Text(day!,
              style: T.plex(10.5, FontWeight.w500,
                  color: stale ? R.warnIcon : R.inkA(.45))),
        ),
        if (stale) ...[
          const SizedBox(width: 6),
          Text('سعر قديم',
              style: T.plex(10, FontWeight.w700, color: R.warnIcon)),
        ],
      ],
    );
  }
}

/// حالةُ خدمةٍ بلا سعرٍ قابلٍ للتنفيذ — بنصّ الخادم نفسِه.
class _StatusChip extends StatelessWidget {
  const _StatusChip({required this.status});
  final String status;

  @override
  Widget build(BuildContext context) {
    final text = switch (status) {
      'NO_PRICE' => 'لا سعر معتمد — غير متاحة حالياً',
      'AMBIGUOUS' => 'سعرٌ مكرَّر — لا تُنفَّذ حتى يُضبط',
      _ => 'غير متاح',
    };

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      decoration: BoxDecoration(
        color: R.error.withValues(alpha: .09),
        borderRadius: BorderRadius.circular(R.rPill),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(Icons.block_rounded, size: 11, color: R.errorText),
          const SizedBox(width: 4),
          Flexible(
            child: Text(text,
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
                style: T.plex(10, FontWeight.w700, color: R.errorText)),
          ),
        ],
      ),
    );
  }
}

/// تذييلُ اللوحة — يقول ما هي وما ليست.
class _BoardNote extends StatelessWidget {
  const _BoardNote();

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(13),
        decoration: BoxDecoration(
          color: R.whiteA(.5),
          border: Border.all(color: R.inkA(.08)),
          borderRadius: BorderRadius.circular(R.rCard),
        ),
        child: const Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _NoteLine(
              icon: Icons.lock_outline_rounded,
              text: 'الأسعار تُعتمد من إدارة الرحالة وتظهر هنا كما هي في '
                  'المنظومة. لا تُعدَّل من التطبيق.',
            ),
            SizedBox(height: 9),
            _NoteLine(
              icon: Icons.north_east_rounded,
              // ⚠ يُقال صراحةً إنّ هذا سعرُ التحويل الصادر: لوحةٌ لا تسمّي
              // اتجاهَ سعرها تُقرأ خطأً مرّةً واحدةً، ومرّةٌ واحدةٌ تكفي.
              text: 'السعر المعروض هو سعر التحويل الصادر من ليبيا، وهو نفسه '
                  'المطبَّق عند تنفيذ الحوالة.',
            ),
            SizedBox(height: 9),
            _NoteLine(
              icon: Icons.verified_outlined,
              // ⚠ التعهّدُ في الاتجاهين، وهو نصُّ أمر المالك: «السعرُ الظاهر
              // في الحوالة الخارجية يجب أن يكون ظاهراً في شاشة الأسعار».
              // فما ليس هنا لا يُنفَّذ، وما يُنفَّذ فهو هنا بسعره نفسِه.
              text: 'كل سعر معروض هنا قابل للتنفيذ، وكل حوالة خارجية تُنفَّذ '
                  'بسعرٍ معروضٍ في هذه الشاشة. وما لا سعر معتمد له لا يظهر '
                  'ولا يُنفَّذ.',
            ),
          ],
        ),
      );
}

class _NoteLine extends StatelessWidget {
  const _NoteLine({required this.icon, required this.text});
  final IconData icon;
  final String text;

  @override
  Widget build(BuildContext context) => Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(icon, size: 14, color: R.inkA(.38)),
          const SizedBox(width: 9),
          Expanded(
            child: Text(text,
                style: T.plex(11.5, FontWeight.w400,
                    color: R.inkA(.55), height: 1.65)),
          ),
        ],
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

/* ───────────────── الطراز ───────────────── */

/// سعرُ خدمةٍ واحدة داخل دولة — أو غيابُه، ومعه سببُه.
class RateRow {
  const RateRow({
    required this.service,
    required this.rate,
    required this.status,
    required this.pricedOn,
  });

  final String? service;

  /// `null` حين لا سعرَ معتمداً — والسببُ في [status].
  final double? rate;

  /// `PRICED` · `NO_PRICE` · `AMBIGUOUS` — بنصّ الخادم، لا باستنتاجٍ من الرقم.
  ///
  /// ⚠ ولا يُشتقّ من `rate == null`: الغيابُ والتكرارُ كلاهما يعطي `null`،
  /// وسببُهما مختلف — أوّلُهما نقصٌ يُسعَّر، وثانيهما تضاربٌ يُضبط. والوكيلُ
  /// ينقل السببَ إلى الشركة، فسببٌ خاطئ يرسله إلى البابِ الخطأ.
  final String status;

  /// تاريخُ آخر إدراجٍ للسعر (YYYY-MM-DD).
  final String? pricedOn;

  /// هل يدخل هذا السطرُ اللوحةَ أصلاً؟
  ///
  /// ⚠ **الشرطان معاً، ولا يُشتقّ أحدُهما من الآخر.** خادمٌ يعيد `PRICED`
  /// بسعرٍ فارغ، أو سعراً مع حالةٍ مانعة — كلاهما تناقضٌ، والتناقضُ في لوحةِ
  /// أسعارٍ يُحسم بالإخفاء لا بالترجيح. والصفرُ ليس سعراً: به يستلم المستفيد
  /// لا شيء بينما الهامشُ يبدو سليماً.
  bool get isDisplayable => status == 'PRICED' && (rate ?? 0) > 0;

  /*
   * ⚠ مئةٌ وثمانون يوماً، لا ثلاثون.
   *
   * هذه لوحةُ شركةِ حوالات لا شاشةُ تداولٍ لحظيّ: السعرَ تضعه الإدارة ويُثبَّت
   * شهوراً. فعتبةٌ قصيرة كانت ستُشعل كلَّ سطرٍ في اللوحة — أحدثُ الأسعار اليوم
   * عمرُها أربعةٌ وسبعون يوماً — وتحذيرٌ يعمّ الجميعَ لا يقرؤه أحد.
   *
   * وبهذه العتبة يُضيء ما يستحقّ وحدَه: سعرُ «حوالات البريد» لمصر المُدرَج في
   * 2024-12-28، أي منذ أكثرَ من سنةٍ ونصف، وما زالت تُسعَّر به حوالاتٌ اليوم.
   */
  static const staleDays = 180;

  bool get isStale {
    final d = pricedOn;
    if (d == null) return false;
    final parsed = DateTime.tryParse(d);
    if (parsed == null) return false;
    return DateTime.now().difference(parsed).inDays > staleDays;
  }

  bool matches(String lowerQuery) =>
      (service ?? '').toLowerCase().contains(lowerQuery);
}

/// أسعارُ دولةٍ واحدة — عملتُها ورمزُها، وسطرٌ لكلّ خدمة.
class RateGroup {
  RateGroup({
    required this.country,
    required this.currency,
    required this.code,
    required this.isStrong,
    required this.rows,
  });

  final String country;
  final String currency;
  final String code;

  /// عَلَمُ `CurrencyPower` — يُعرض شارةً ولا يُحسب به.
  final bool isStrong;

  final List<RateRow> rows;

  /// المجموعةُ مرشَّحةً ببحث — أو `null` إن لم يبقَ فيها شيء.
  RateGroup? filtered(String q) {
    final t = q.toLowerCase();

    final headHit = country.toLowerCase().contains(t) ||
        currency.toLowerCase().contains(t) ||
        code.toLowerCase().contains(t);

    if (headHit) return this;

    final kept = rows.where((r) => r.matches(t)).toList();
    if (kept.isEmpty) return null;

    return RateGroup(
      country: country,
      currency: currency,
      code: code,
      isStrong: isStrong,
      rows: kept,
    );
  }
}

/// أسعارُ العملات — **نقطةٌ واحدة بمسارين**، وقراءةٌ واحدة في الخادم.
///
/// ⚠ `autoDispose` عمداً: لا نسخةَ تبقى بعد إغلاق الشاشة، فلا يُعرض سعرٌ
/// قديمٌ بعد تعديلٍ في المكتب الخلفيّ.
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
     * ⚠ وهو **تجميعُ عرضٍ لا حساب**: لا رقمَ يُجمع ولا يُتوسَّط ولا يُقرَّب.
     * كلُّ سعرٍ يُعرض كما جاء من القاعدة.
     */
    final groups = <String, RateGroup>{};
    for (final m in items) {
      final key = '${m['country_id']}';
      final row = RateRow(
        service: (m['service'] as String?)?.trim(),
        rate: m['rate'] == null ? null : Fmt.num_(m['rate']),
        status: '${m['status'] ?? 'PRICED'}',
        pricedOn: (m['priced_on'] as String?)?.trim(),
      );

      /*
       * ⚠⚠ لا يدخل اللوحةَ إلّا سعرٌ قابلٌ للتنفيذ — أمرُ المالك 11 سبتمبر 2026.
       *
       * «أيُّ عملةٍ بدون سعرٍ امنع ظهورَها في الشاشة … والسعرُ الظاهر في
       *  الحوالة الخارجية يجب أن يكون ظاهراً في شاشة الأسعار».
       *
       * ⚠ والخادمُ يُرشّح أصلاً بالحارس نفسِه، وهذا **ترشيحٌ ثانٍ مقصود**:
       * الهاتفُ يُحدَّث قبل الخادم وبعده، وتطبيقٌ جديدٌ أمام خادمٍ قديمٍ كان
       * سيعرض سطراً بلا سعر. وصفٌّ بلا رقمٍ على لوحةِ أسعارٍ يُقرأ «متاحٌ
       * وسأسأل عن سعره» — فيَعِد الوكيلُ زبونَه بما لا يُنفَّذ.
       *
       * ⚠ وهو حذفُ **صفٍّ لا حقل**: لا يُخفى شيءٌ من سطرٍ معروض.
       */
      if (!row.isDisplayable) continue;

      final g = groups[key];
      if (g == null) {
        groups[key] = RateGroup(
          country: '${m['country'] ?? ''}'.trim(),
          currency: '${m['currency'] ?? ''}'.trim(),
          code: '${m['code'] ?? ''}'.trim(),
          isStrong: '${m['currency_power'] ?? ''}' == '1',
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
