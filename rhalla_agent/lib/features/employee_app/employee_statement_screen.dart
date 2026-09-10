import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../branding/branding_controller.dart';
import '../transfers/receipt.dart';

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
///
/// ══════════════════════════════════════════════════════════════════════════
///  وشاشةٌ واحدةٌ تُبنى ثلاث مرّات — أمرُ إعادة الهيكلة (10 سبتمبر 2026)
/// ══════════════════════════════════════════════════════════════════════════
///
/// الشجرةُ في الأمر تطلب ثلاثةَ كشوف: «كشف حوالاتي» تحت المحلّية، و«كشف
/// الحوالات» تحت الخارجية، و«حوالات اليوم (الكل)» في التقارير.
///
/// ⚠ **وهي كشفٌ واحدٌ بثلاثة نطاقات، لا ثلاثةُ كشوف.** ثلاثُ شاشاتٍ متشابهة
/// تفترق عند أوّل تعديل، ثمّ يختلف مجموعُ «الكل» عن مجموع شطريه — وهو أسوأُ
/// ما يقع في جرد. والخادمُ كذلك: استعلامٌ واحدٌ يأخذ القناة.
///
/// ⚠ والفلترُ كاملٌ كما نصّ الأمر: **اليوم · أسبوع · شهر · الكل · من–إلى**،
/// ومعه إصدارُ الكشف طباعةً ومشاركة.
enum StatementScope {
  /// الكلّ — محلّيةٌ وخارجية معاً. تبويبُ التقارير.
  all('الكل', null),

  /// المحلّية وحدها.
  local('محلية', 'LOCAL'),

  /// الخارجية وحدها.
  external('خارجية', 'EXTERNAL');

  const StatementScope(this.label, this.channel);

  final String label;

  /// ما يُرسل إلى الخادم. `null` يعني بلا ترشيح.
  final String? channel;
}

/// مدى الكشف — شريحةٌ جاهزة أو مدىً اختاره الموظف بتاريخين.
class StatementRange {
  const StatementRange({required this.days, this.from, this.to});

  /// عددُ الأيام، و**0 تعني الكلّ** — بلا حدٍّ زمنيّ.
  final int days;
  final DateTime? from;
  final DateTime? to;

  bool get isCustom => from != null || to != null;

  static const today = StatementRange(days: 1);
  static const week = StatementRange(days: 7);
  static const month = StatementRange(days: 30);
  static const all = StatementRange(days: 0);

  /// مفتاحُ المزوّد — نصٌّ لأنّ `family` يقارن بالتساوي، والأصنافُ بلا
  /// `==` تُنتج طلباً جديداً عند كل بناء.
  String get key => isCustom
      ? 'from:${_d(from)}|to:${_d(to)}'
      : 'days:$days';

  static String _d(DateTime? t) =>
      t == null ? '' : '${t.year}-${_two(t.month)}-${_two(t.day)}';

  static String _two(int n) => n < 10 ? '0$n' : '$n';

  String get label {
    if (isCustom) {
      final a = from == null ? '…' : _d(from);
      final b = to == null ? '…' : _d(to);
      return '$a — $b';
    }
    return switch (days) {
      1 => 'اليوم',
      7 => 'أسبوع',
      30 => 'شهر',
      _ => 'الكل',
    };
  }
}

/// مفتاحُ طلبِ كشف — النطاقُ والمدى معاً.
///
/// ⚠ صنفٌ بمساواةٍ بالقيمة: `FutureProvider.family` يقارن مفتاحَه بـ`==`،
/// وصنفٌ بلا `==` يُنتج مزوّداً جديداً عند كل بناءٍ للشاشة — أي طلبَ شبكةٍ
/// عند كل إطار.
class StatementQuery {
  const StatementQuery(this.scope, this.range);

  final StatementScope scope;
  final StatementRange range;

  @override
  bool operator ==(Object other) =>
      other is StatementQuery &&
      other.scope == scope &&
      other.range.key == range.key;

  @override
  int get hashCode => Object.hash(scope, range.key);
}

class EmployeeStatementScreen extends ConsumerStatefulWidget {
  const EmployeeStatementScreen({
    super.key,
    this.scope = StatementScope.local,
    this.title = 'كشف حوالاتي',
    this.asTab = false,
  });

  final StatementScope scope;
  final String title;

  /// تبويبٌ في الشريط السفليّ — فبلا زرّ رجوع، وبحشوةٍ سفليّةٍ تحت الشريط.
  final bool asTab;

  @override
  ConsumerState<EmployeeStatementScreen> createState() =>
      _EmployeeStatementScreenState();
}

class _EmployeeStatementScreenState
    extends ConsumerState<EmployeeStatementScreen> {
  StatementRange _range = StatementRange.week;

  static const _windows = <StatementRange>[
    StatementRange.today,
    StatementRange.week,
    StatementRange.month,
    StatementRange.all,
  ];

  @override
  Widget build(BuildContext context) {
    final query = StatementQuery(widget.scope, _range);
    final async = ref.watch(employeeStatementProvider(query));

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: widget.title,
            subtitle: widget.scope == StatementScope.all
                ? 'محلية وخارجية · مسلَّمة وغير مسلَّمة'
                : null,
            onBack: widget.asTab ? null : () => context.pop(),
            // ⚠ «إصدار الكشف» يظهر ومعه بياناتُه لا قبلها: زرٌّ يُضغط على
            // شاشةٍ لم تُحمَّل بعد يُخرج ورقةً بيضاء.
            trailing: async.valueOrNull != null
                ? _ExportButton(
                    onPrint: () => _issue(async.value!, share: false),
                    onShare: () => _issue(async.value!, share: true),
                  )
                : null,
          ),

          _Filters(
            windows: _windows,
            current: _range,
            onPick: (r) => setState(() => _range = r),
            onCustom: _pickRange,
          ),

          Expanded(
            child: RefreshIndicator(
              onRefresh: () async =>
                  ref.invalidate(employeeStatementProvider(query)),
              color: R.primary,
              backgroundColor: Colors.white,
              child: async.when(
                loading: () => const Center(child: CircularProgressIndicator()),
                error: (e, _) => ListView(children: [
                  _Msg(
                    icon: Icons.wifi_off_rounded,
                    text: 'تعذّر تحميل الكشف.\n$e',
                  ),
                ]),
                data: (s) => ListView(
                  padding: EdgeInsets.fromLTRB(
                      R.padScreen, 4, R.padScreen, widget.asTab ? 110 : 30),
                  physics: const AlwaysScrollableScrollPhysics(),
                  children: [
                    _Totals(s: s),
                    const SizedBox(height: R.gapCard),

                    /*
                     * ⚠ بلوغُ السقف يُقال صراحةً.
                     *
                     * كشفٌ مقصوصٌ صامتاً يُقرأ كشفاً تامّاً، فيُجرد الدرجُ
                     * على مجموعٍ ناقصٍ ولا يعرف الموظف لماذا لا يطابق.
                     */
                    if (s.truncated) ...[
                      _Notice(
                        text: 'عُرض أحدثُ ٥٠٠ حركة في هذه المدة. '
                            'اختر مدىً أضيق ليكون الكشف تامّاً.',
                      ),
                      const SizedBox(height: R.gapCard),
                    ],

                    if (s.items.isEmpty)
                      const _Msg(
                        icon: Icons.receipt_long_rounded,
                        text: 'لا حوالات في هذه المدة.',
                      )
                    else
                      ...s.items.map((it) => Padding(
                            padding: const EdgeInsets.only(bottom: 8),
                            child: _Row(
                              it: it,
                              // ⚠ وسمُ القناة يُعرض في كشف «الكل» وحدَه:
                              // في كشفٍ كلُّ صفوفه محلّية يكون الوسمُ ضجيجاً
                              // مكرّراً على كلّ سطر.
                              showChannel: widget.scope == StatementScope.all,
                            ),
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

  /// اختيارُ مدىً بتاريخين — «من – إلى».
  Future<void> _pickRange() async {
    final now = DateTime.now();
    final picked = await showDateRangePicker(
      context: context,
      // ⚠ سنتان إلى الوراء: أقدمُ ممّا يحتاجه جردٌ يوميّ، وأقربُ من «منذ
      // البداية» الذي يجعل التمرير في المُنتقي عملاً بذاته.
      firstDate: DateTime(now.year - 2),
      lastDate: now,
      currentDate: now,
      locale: const Locale('ar'),
      helpText: 'اختر مدى الكشف',
      saveText: 'تطبيق',
    );

    if (picked == null || !mounted) return;

    setState(() => _range = StatementRange(
          days: 0,
          from: picked.start,
          to: picked.end,
        ));
  }

  /*
   * ══════════════════════════════════════════════════════════════════════
   *  إصدارُ الكشف — تصويرٌ لا توليدُ PDF نصّيّ
   * ══════════════════════════════════════════════════════════════════════
   *
   * ⚠ القاعدةُ نفسُها التي تُطبع بها الفاتورة (انظر `receipt.dart`): توليدُ
   * PDF نصّيٍّ يتطلّب تشكيلَ الحروف العربية ووصلَها داخل مكتبة الـ PDF، وهو
   * مصدرُ أعطالٍ معروف — حروفٌ منفصلة أو معكوسة. والتصويرُ يلتقط ما يراه
   * الموظف حرفياً بخطّ التطبيق نفسِه: فما يُطبع هو ما يُرى.
   *
   * ⚠ والورقةُ تُبنى **خارج الشاشة**، غيرَ قابلةٍ للتمرير، بكل صفوف الكشف:
   * تصويرُ القائمة المعروضة كان سيلتقط ما يظهر منها فقط — أي كشفاً ناقصاً
   * يظنّه حاملُه تامّاً.
   */
  Future<void> _issue(Statement s, {required bool share}) async {
    final company =
        ref.read(brandingControllerProvider).branding.displayName;

    await showDialog<void>(
      context: context,
      barrierDismissible: false,
      builder: (_) => _StatementSheetDialog(
        statement: s,
        title: widget.title,
        rangeLabel: _range.label,
        company: company,
        share: share,
      ),
    );
  }
}

/* ───────────────── الفلاتر ───────────────── */

class _Filters extends StatelessWidget {
  const _Filters({
    required this.windows,
    required this.current,
    required this.onPick,
    required this.onCustom,
  });

  final List<StatementRange> windows;
  final StatementRange current;
  final ValueChanged<StatementRange> onPick;
  final VoidCallback onCustom;

  @override
  Widget build(BuildContext context) => SingleChildScrollView(
        scrollDirection: Axis.horizontal,
        padding:
            const EdgeInsets.symmetric(horizontal: R.padScreen, vertical: 8),
        child: Row(
          children: [
            for (final w in windows) ...[
              _Chip(
                label: w.label,
                selected: !current.isCustom && current.days == w.days,
                onTap: () => onPick(w),
              ),
              const SizedBox(width: 8),
            ],
            // «من – إلى» شريحةٌ في الصفّ نفسِه لا زرٌّ منفصل: هي خيارٌ خامسٌ
            // من الفلتر، لا فعلٌ من نوعٍ آخر.
            _Chip(
              label: current.isCustom ? current.label : 'من – إلى',
              selected: current.isCustom,
              icon: Icons.date_range_rounded,
              onTap: onCustom,
            ),
          ],
        ),
      );
}

/* ───────────────── الطراز ───────────────── */

class StatementItem {
  const StatementItem({
    required this.transferNumber,
    required this.action,
    required this.amount,
    required this.channel,
    required this.at,
  });

  final String transferNumber;
  final String action;
  final double amount;

  /// `LOCAL` أو `EXTERNAL` — كما كتبه سجلُّ النسبة.
  final String channel;
  final String? at;

  bool get isIn => action == 'CREATED';
  bool get isExternal => channel == 'EXTERNAL';
}

class Statement {
  const Statement({
    required this.createdCount,
    required this.createdTotal,
    required this.deliveredCount,
    required this.deliveredTotal,
    required this.net,
    required this.truncated,
    required this.items,
  });

  final int createdCount;
  final double createdTotal;
  final int deliveredCount;
  final double deliveredTotal;
  final double net;

  /// بلغ الكشفُ سقفَ العرض، فما فيه أحدثُ ٥٠٠ حركةٍ لا كلُّها.
  final bool truncated;

  final List<StatementItem> items;

  static Statement fromJson(Map<String, dynamic> j) => Statement(
        createdCount: int.tryParse('${j['created_count'] ?? 0}') ?? 0,
        createdTotal: Fmt.num_(j['created_total']),
        deliveredCount: int.tryParse('${j['delivered_count'] ?? 0}') ?? 0,
        deliveredTotal: Fmt.num_(j['delivered_total']),
        net: Fmt.num_(j['net']),
        truncated: j['truncated'] == true,
        items: ((j['items'] as List?) ?? const [])
            .map((e) => (e as Map).cast<String, dynamic>())
            .map((m) => StatementItem(
                  transferNumber: '${m['transfer_number'] ?? ''}',
                  action: '${m['action'] ?? ''}',
                  amount: Fmt.num_(m['amount']),
                  channel: '${m['channel'] ?? 'LOCAL'}',
                  at: '${m['at'] ?? ''}',
                ))
            .toList(),
      );
}

/// ⚠ باسمٍ صريح: `statementProvider` مأخوذٌ لكشف الوكيل في
/// `home_repository.dart`. واسمان متطابقان في ملفّين لا يتضاربان حتى
/// يُستورَد الملفّان معاً — ثمّ يتضاربان في يومٍ لا أحدَ يتوقّعه.
final employeeStatementProvider =
    FutureProvider.autoDispose.family<Statement, StatementQuery>(
        (ref, q) async {
  final r = q.range;

  final env = await ref.watch(apiClientProvider).get(
    '/device/employee/statement',
    query: {
      'days': r.days,
      if (q.scope.channel != null) 'channel': q.scope.channel,
      if (r.from != null) 'from': StatementRange._d(r.from),
      if (r.to != null) 'to': StatementRange._d(r.to),
    },
  );

  return Statement.fromJson(env.row ?? const {});
});

/* ───────────────── العناصر ───────────────── */

class _ExportButton extends StatelessWidget {
  const _ExportButton({required this.onPrint, required this.onShare});

  final VoidCallback onPrint;
  final VoidCallback onShare;

  @override
  Widget build(BuildContext context) {
    return PopupMenuButton<int>(
      tooltip: 'إصدار الكشف',
      icon: Icon(Icons.ios_share_rounded, size: 20, color: R.ink),
      onSelected: (v) => v == 0 ? onPrint() : onShare(),
      itemBuilder: (_) => [
        PopupMenuItem(
          value: 0,
          child: Row(children: [
            Icon(Icons.print_outlined, size: 18, color: R.ink),
            const SizedBox(width: 10),
            Text('طباعة الكشف', style: T.plex(13, FontWeight.w500)),
          ]),
        ),
        PopupMenuItem(
          value: 1,
          child: Row(children: [
            Icon(Icons.share_outlined, size: 18, color: R.ink),
            const SizedBox(width: 10),
            Text('مشاركة الكشف', style: T.plex(13, FontWeight.w500)),
          ]),
        ),
      ],
    );
  }
}

class _Notice extends StatelessWidget {
  const _Notice({required this.text});
  final String text;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(12),
        decoration: BoxDecoration(
          color: R.warnBg,
          border: Border.all(color: R.warnBorder),
          borderRadius: BorderRadius.circular(R.rCard),
        ),
        child: Row(
          children: [
            Icon(Icons.info_outline_rounded, size: 16, color: R.warnIcon),
            const SizedBox(width: 8),
            Expanded(
              child: Text(text,
                  style: T.plex(11.5, FontWeight.w500,
                      color: R.warnInk, height: 1.6)),
            ),
          ],
        ),
      );
}

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
                    Text('الصافي', style: T.kufi(13.5, FontWeight.w800)),
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
  const _Row({required this.it, this.showChannel = false});

  final StatementItem it;
  final bool showChannel;

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
              it.isIn ? Icons.south_west_rounded : Icons.north_east_rounded,
              size: 17,
              color: tone,
            ),
          ),
          const SizedBox(width: 10),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    Flexible(
                      child: Text(
                          it.isIn ? 'حوالة أنشأتُها' : 'حوالة سلَّمتُها',
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style: T.kufi(13, FontWeight.w700)),
                    ),
                    if (showChannel) ...[
                      const SizedBox(width: 6),
                      _ChannelTag(external: it.isExternal),
                    ],
                  ],
                ),
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

class _ChannelTag extends StatelessWidget {
  const _ChannelTag({required this.external});
  final bool external;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 6, vertical: 2),
        decoration: BoxDecoration(
          color: external ? R.warnBg : R.primaryA(.10),
          borderRadius: BorderRadius.circular(6),
        ),
        child: Text(external ? 'خارجية' : 'محلية',
            style: T.plex(9.5, FontWeight.w700,
                color: external ? R.warnInk : R.primaryDark)),
      );
}

class _Chip extends StatelessWidget {
  const _Chip({
    required this.label,
    required this.selected,
    required this.onTap,
    this.icon,
  });

  final String label;
  final bool selected;
  final VoidCallback onTap;
  final IconData? icon;

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
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              if (icon != null) ...[
                Icon(icon,
                    size: 14,
                    color: selected ? R.primaryDark : R.inkA(.55)),
                const SizedBox(width: 5),
              ],
              Text(label,
                  style: T.plex(
                      12.5, selected ? FontWeight.w700 : FontWeight.w500,
                      color: selected ? R.primaryDark : R.inkA(.6))),
            ],
          ),
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

/* ══════════════════════════════════════════════════════════════════════════
   ورقةُ الكشف — تُبنى لتُصوَّر، لا لتُقرأ على الشاشة
   ══════════════════════════════════════════════════════════════════════════ */

/// نافذةٌ تعرض الورقةَ كاملةً ثمّ تطبعها أو تشاركها.
///
/// ⚠ **تُعرض ولا تُخفى.** بناءُ الورقة خارج الشجرة (بـ`Offstage` أو ما
/// شابه) يعني `RepaintBoundary` بلا حجمٍ ولا رسم، فيخرج التصويرُ فارغاً —
/// وهي نتيجةٌ لا يظهر سببُها في أيّ سجلّ. فتُعرض معاينةً حقيقية: الموظف يرى
/// ما سيُطبع قبل أن يُطبع، وهو الأصحّ على أيّ حال.
class _StatementSheetDialog extends StatefulWidget {
  const _StatementSheetDialog({
    required this.statement,
    required this.title,
    required this.rangeLabel,
    required this.company,
    required this.share,
  });

  final Statement statement;
  final String title;
  final String rangeLabel;
  final String company;
  final bool share;

  @override
  State<_StatementSheetDialog> createState() => _StatementSheetDialogState();
}

class _StatementSheetDialogState extends State<_StatementSheetDialog>
    with ReceiptTools<_StatementSheetDialog> {
  @override
  Widget build(BuildContext context) {
    return Dialog.fullscreen(
      backgroundColor: R.bgTop,
      child: SafeArea(
        child: Column(
          children: [
            RhallaAppBar(
              title: 'معاينة الكشف',
              subtitle: widget.rangeLabel,
              onBack: () => Navigator.of(context).pop(),
            ),
            Expanded(
              child: SingleChildScrollView(
                padding: const EdgeInsets.all(R.padScreen),
                child: RepaintBoundary(
                  key: receiptKey,
                  child: _StatementSheet(
                    statement: widget.statement,
                    title: widget.title,
                    rangeLabel: widget.rangeLabel,
                    company: widget.company,
                  ),
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.fromLTRB(
                  R.padScreen, 8, R.padScreen, R.padScreen),
              child: PrimaryButton(
                label: widget.share ? 'مشاركة' : 'طباعة',
                loading: receiptBusy,
                onPressed: receiptBusy
                    ? null
                    : () async {
                        if (widget.share) {
                          await shareReceipt(
                            name: widget.title,
                            text: '${widget.title} — ${widget.rangeLabel}',
                          );
                        } else {
                          await printReceipt(name: widget.title);
                        }
                      },
              ),
            ),
          ],
        ),
      ),
    );
  }
}

/// الورقةُ نفسُها — بيضاءُ الأرضية، بكل صفوف الكشف بلا تمرير.
class _StatementSheet extends StatelessWidget {
  const _StatementSheet({
    required this.statement,
    required this.title,
    required this.rangeLabel,
    required this.company,
  });

  final Statement statement;
  final String title;
  final String rangeLabel;
  final String company;

  @override
  Widget build(BuildContext context) {
    final s = statement;

    return Container(
      color: Colors.white,
      padding: const EdgeInsets.all(18),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          // ⚠ الترويسةُ باسم شركة الوكيل لا باسم الرحالة — القاعدةُ نفسُها
          // التي تحكم الفاتورة: كلُّ ورقةٍ يخرج بها الموظف باسم شركته.
          Text(company,
              textAlign: TextAlign.center,
              style: T.kufi(15, FontWeight.w800, color: R.ink)),
          const SizedBox(height: 4),
          Text(title,
              textAlign: TextAlign.center,
              style: T.kufi(13, FontWeight.w700, color: R.primaryDark)),
          const SizedBox(height: 2),
          Text(rangeLabel,
              textAlign: TextAlign.center,
              style: T.plex(11, FontWeight.w500, color: R.inkA(.55))),
          const SizedBox(height: 12),
          Divider(color: R.inkA(.2), height: 1),
          const SizedBox(height: 12),

          Row(
            children: [
              Expanded(
                child: _SheetTotal(
                    label: 'قبضتُ',
                    count: s.createdCount,
                    value: s.createdTotal),
              ),
              Expanded(
                child: _SheetTotal(
                    label: 'سلَّمتُ',
                    count: s.deliveredCount,
                    value: s.deliveredTotal),
              ),
              Expanded(
                child: _SheetTotal(
                    label: 'الصافي', count: null, value: s.net),
              ),
            ],
          ),

          const SizedBox(height: 12),
          Divider(color: R.inkA(.2), height: 1),
          const SizedBox(height: 8),

          for (final it in s.items)
            Padding(
              padding: const EdgeInsets.symmetric(vertical: 5),
              child: Row(
                children: [
                  SizedBox(
                    width: 62,
                    child: Text(it.isIn ? 'أنشأتُها' : 'سلَّمتُها',
                        style: T.plex(10.5, FontWeight.w600,
                            color: it.isIn ? R.primaryDark : R.error)),
                  ),
                  Expanded(
                    child: Directionality(
                      textDirection: TextDirection.ltr,
                      child: Align(
                        alignment: AlignmentDirectional.centerStart,
                        child: Text(it.transferNumber,
                            style: T.plex(10.5, FontWeight.w400,
                                color: R.inkA(.75))),
                      ),
                    ),
                  ),
                  Text(Fmt.stampShort(it.at),
                      style: T.plex(9.5, FontWeight.w400, color: R.inkA(.5))),
                  const SizedBox(width: 10),
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Text(
                        '${it.isIn ? '+' : '−'} ${Fmt.money(it.amount)}',
                        style: T.plex(11, FontWeight.w700, color: R.ink)),
                  ),
                ],
              ),
            ),

          if (s.items.isEmpty)
            Padding(
              padding: const EdgeInsets.symmetric(vertical: 20),
              child: Text('لا حوالات في هذه المدة.',
                  textAlign: TextAlign.center,
                  style: T.plex(12, FontWeight.w500, color: R.inkA(.5))),
            ),

          const SizedBox(height: 10),
          Divider(color: R.inkA(.2), height: 1),
          const SizedBox(height: 6),
          Text(
            // ⚠ يقول ما هو: جردُ حركةِ نقدٍ لا كشفَ حسابٍ محاسبيّ. وورقةٌ
            // تُقرأ على غير ما هي تُبنى عليها قرارات.
            'كشفُ حركةِ حوالاتٍ للجرد — ليس كشفَ حساب.',
            textAlign: TextAlign.center,
            style: T.plex(9.5, FontWeight.w400, color: R.inkA(.45)),
          ),
        ],
      ),
    );
  }
}

class _SheetTotal extends StatelessWidget {
  const _SheetTotal({
    required this.label,
    required this.count,
    required this.value,
  });

  final String label;
  final int? count;
  final double value;

  @override
  Widget build(BuildContext context) => Column(
        children: [
          Text(label, style: T.plex(10.5, FontWeight.w700, color: R.inkA(.6))),
          const SizedBox(height: 3),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Text(Fmt.money(value),
                style: T.kufi(13, FontWeight.w800, color: R.ink)),
          ),
          if (count != null) ...[
            const SizedBox(height: 2),
            Text('$count حوالة',
                style: T.plex(9.5, FontWeight.w400, color: R.inkA(.45))),
          ],
        ],
      );
}
