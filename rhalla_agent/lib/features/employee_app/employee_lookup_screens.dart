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
import '../transfers/agent_incoming_repository.dart';
import '../transfers/delivery_receipt_screen.dart';
import '../transfers/outgoing_receipt_screen.dart';

/* ══════════════════════════════════════════════════════════════════════════
   حوالاتُ نقطة البيع
   ══════════════════════════════════════════════════════════════════════════

   ⚠ **عملُه هو على نقطة بيعه، لا عملُ زملائه** — أمرُ المالك (8 سبتمبر
   2026): «الموظفون لا يرون جلسات بعضهم». والخادمُ يقيّدها بصاحبها، وهذه
   الشاشةُ تعرض ما يعود به.

   وتبقى مختلفةً عن «كشف حوالاتي»: هذه على **نقطة بيعه الحالية** وحدَها،
   وذاك على عمله أينما كان.
   ══════════════════════════════════════════════════════════════════════════ */

final _posTransfersProvider =
    FutureProvider.autoDispose<List<Map<String, dynamic>>>((ref) async {
  final env = await ref
      .watch(apiClientProvider)
      .get('/device/employee/transfers/point-of-sale',
          query: {'per_page': 50});

  final row = env.row;
  final items = (row?['items'] as List?) ?? const [];
  return items.map((e) => (e as Map).cast<String, dynamic>()).toList();
});

class EmployeePosTransfersScreen extends ConsumerWidget {
  const EmployeePosTransfersScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(_posTransfersProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'حوالات نقطة بيعي', onBack: () => context.pop()),
          Expanded(
            child: RefreshIndicator(
              onRefresh: () async => ref.invalidate(_posTransfersProvider),
              color: R.primary,
              backgroundColor: Colors.white,
              child: async.when(
                loading: () => const Center(child: CircularProgressIndicator()),
                error: (e, _) => _Msg(
                  icon: Icons.wifi_off_rounded,
                  text: 'تعذّر التحميل.\n$e',
                ),
                data: (rows) => rows.isEmpty
                    ? const _Msg(
                        icon: Icons.storefront_outlined,
                        text: 'لا حوالات على نقطة بيعك.\n\n'
                            'وإن لم تكن لك نقطة بيع مُسنَدة، '
                            'فلا شيء يُعرض هنا.',
                      )
                    : ListView.separated(
                        padding: const EdgeInsets.fromLTRB(
                            R.padScreen, 14, R.padScreen, 30),
                        itemCount: rows.length,
                        separatorBuilder: (_, _) => const SizedBox(height: 8),
                        itemBuilder: (_, i) => _TransferRow(m: rows[i]),
                      ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}

/* ══════════════════════════════════════════════════════════════════════════
   البحث برقم الحوالة
   ══════════════════════════════════════════════════════════════════════════ */

class EmployeeSearchScreen extends ConsumerStatefulWidget {
  const EmployeeSearchScreen({super.key});

  @override
  ConsumerState<EmployeeSearchScreen> createState() =>
      _EmployeeSearchScreenState();
}

class _EmployeeSearchScreenState extends ConsumerState<EmployeeSearchScreen> {
  final _code = TextEditingController();
  late final _focus = AutoClearFocus(_code);

  bool _busy = false;
  String? _error;

  /*
   * ⚠ قائمةٌ لا صفٌّ واحد — بلاغُ المالك (10 سبتمبر 2026): «يستدعيها ويعرضها
   * كتبويب في الشاشة، فإذا عرف أنها هي يضغط عليها لتُفتح مثل الحوالة الصادرة
   * بكافة بياناتها ويمكن طباعتها أو تصديرها».
   *
   * والبحثُ جزئيّ (`LIKE`) فقد يطابق أكثر من رقم؛ وعرضُ الأوّل وحدَه كان
   * يُخفي البقيّة بلا أن يقول إنه أخفى شيئاً.
   */
  List<Map<String, dynamic>> _results = const [];
  int? _opening;

  @override
  void dispose() {
    _focus.dispose();
    _code.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => Screen(
        child: Column(
          children: [
            RhallaAppBar(title: 'بحث برقم الحوالة', onBack: () => context.pop()),
            Expanded(
              child: ListView(
                padding:
                    const EdgeInsets.fromLTRB(R.padScreen, 16, R.padScreen, 30),
                children: [
                  TextField(
                    controller: _code,
                    focusNode: _focus,
                    textDirection: TextDirection.ltr,
                    style: T.kufi(15, FontWeight.w700),
                    decoration: InputDecoration(
                      hintText: 'رقم الحوالة',
                      hintStyle: T.plex(12.5, FontWeight.w400,
                          color: R.inkA(.35)),
                      filled: true,
                      fillColor: R.whiteA(.7),
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(R.rCard),
                        borderSide: BorderSide(color: R.inkA(.12)),
                      ),
                      enabledBorder: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(R.rCard),
                        borderSide: BorderSide(color: R.inkA(.12)),
                      ),
                      focusedBorder: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(R.rCard),
                        borderSide: BorderSide(color: R.primaryA(.5)),
                      ),
                      contentPadding: const EdgeInsets.symmetric(
                          horizontal: 14, vertical: 14),
                    ),
                    onSubmitted: (_) => _search(),
                  ),
                  const SizedBox(height: 12),
                  PrimaryButton(
                    label: 'بحث',
                    loading: _busy,
                    onPressed: _busy ? null : _search,
                  ),

                  if (_error != null) ...[
                    const SizedBox(height: 16),
                    Container(
                      padding: const EdgeInsets.all(12),
                      decoration: BoxDecoration(
                        color: R.error.withValues(alpha: .07),
                        border:
                            Border.all(color: R.error.withValues(alpha: .3)),
                        borderRadius: BorderRadius.circular(R.rCard),
                      ),
                      child: Text(_error!,
                          style: T.plex(12.5, FontWeight.w500,
                              color: R.error, height: 1.7)),
                    ),
                  ],

                  if (_results.isNotEmpty) ...[
                    const SizedBox(height: 18),
                    Row(
                      children: [
                        Text('النتائج', style: T.section),
                        const SizedBox(width: 8),
                        Text('${_results.length}',
                            style: T.plex(12, FontWeight.w700,
                                color: R.inkA(.5))),
                      ],
                    ),
                    const SizedBox(height: 10),
                    for (var i = 0; i < _results.length; i++) ...[
                      if (i > 0) const SizedBox(height: 8),
                      // ⚠ الصفُّ نفسُه، وقد صار يُضغط: الضغطةُ تفتح الفاتورة
                      // الكاملة — الواردة بفاتورتها والصادرة بفاتورتها.
                      Opacity(
                        opacity: _opening == null || _opening == i ? 1 : .5,
                        child: InkWell(
                          borderRadius: BorderRadius.circular(R.rCard),
                          onTap: _opening != null ? null : () => _open(i),
                          child: _TransferRow(m: _results[i], big: true),
                        ),
                      ),
                    ],
                  ],
                ],
              ),
            ),
          ],
        ),
      );

  Future<void> _search() async {
    final code = _code.text.trim();
    if (code.isEmpty) {
      setState(() => _error = 'اكتب رقم الحوالة.');
      return;
    }

    setState(() {
      _busy = true;
      _error = null;
      _results = const [];
    });

    try {
      final env = await ref
          .read(apiClientProvider)
          // ⚠ `q` هو ما يقرؤه المسار. كان يُرسَل `code`، فيصل المصطلحُ
          // فارغاً ويردّ الخادم «اكتب ثلاثة محارف» على بحثٍ مكتوب — وهو
          // سببُ أنّ الشاشة «لا تعرض». (الخادم صار يقبل الاثنين احتياطاً.)
          .get('/device/employee/transfers/search', query: {'q': code});

      if (!mounted) return;

      // الحمولةُ كائنٌ فيه `items`؛ وتُقرأ `rows` كذلك لردٍّ أقدم.
      final data = env.row;
      final list = (data?['items'] as List?) ?? const [];
      final rows = list.isNotEmpty
          ? list.whereType<Map>().map((e) => e.cast<String, dynamic>()).toList()
          : env.rows;

      setState(() {
        _busy = false;
        _results = rows;
        if (rows.isEmpty) _error = 'لا حوالة بهذا الرقم.';
      });
    } on ApiFailure catch (e) {
      if (mounted) setState(() { _busy = false; _error = e.message; });
    } catch (_) {
      if (mounted) {
        setState(() {
          _busy = false;
          _error = 'تعذّر البحث — تحقّق من الاتصال.';
        });
      }
    }
  }

  /*
   * ══════════════════════════════════════════════════════════════════════
   *  فتحُ الفاتورة — نفسُ فواتير شاشة الحوالات، لا نسخةٌ ثالثة
   * ══════════════════════════════════════════════════════════════════════
   *
   * أمرُ المالك: «تُفتح مثل الحوالة الصادرة بكافة بياناتها ويمكن طباعتها أو
   * تصديرها، كأني أعرضها من شاشة الحوالات». فهي **هي**: `DeliveryReceiptScreen`
   * للواردة و`OutgoingReceiptScreen` للصادرة — بطباعتهما ومشاركتهما.
   *
   * ⚠ ويُقرَأ `kind` من الخادم لا يُخمَّن من شكل الحقول: صفٌّ ناقصُ الحقول
   * كان سيُقرأ صادراً أو وارداً بحسب ما صادف أن يحمله.
   *
   * ⚠ وتُجلب الحوالةُ كاملةً قبل الفتح: صفُّ البحث موجزٌ (رقمٌ واسمٌ ومبلغ)،
   * والفاتورةُ تحتاج المرسِلَ والفرعَ وسببَ الإلغاء. والمستودعُ في **وضع
   * الموظف**، فمسارات الوكيل تردّ 403 على جلسته.
   */
  Future<void> _open(int i) async {
    final m = _results[i];
    final code = '${m['transfer_number'] ?? m['Code'] ?? m['code'] ?? ''}'.trim();
    if (code.isEmpty) return;

    setState(() => _opening = i);
    final repo = ref.read(transfersRepositoryForProvider(TransfersMode.employee));

    try {
      if ('${m['kind'] ?? ''}' == 'OUTGOING') {
        final t = await repo.findOutgoing(code);
        if (!mounted) return;
        if (t == null) {
          _say('تعذّر فتح فاتورة هذه الحوالة.');
          return;
        }
        await Navigator.of(context, rootNavigator: true).push(
          MaterialPageRoute(builder: (_) => OutgoingReceiptScreen(transfer: t)),
        );
        return;
      }

      final t = await repo.findByCode(code);
      if (!mounted) return;
      if (t == null) {
        _say('تعذّر فتح فاتورة هذه الحوالة.');
        return;
      }
      await Navigator.of(context, rootNavigator: true).push(
        MaterialPageRoute(
          builder: (_) => DeliveryReceiptScreen(
            transfer: t,
            mode: TransfersMode.employee,
          ),
        ),
      );
    } on ApiFailure catch (e) {
      _say(e.message);
    } catch (_) {
      _say('تعذّر فتح الفاتورة — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _opening = null);
    }
  }

  void _say(String m) {
    if (!mounted) return;
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

/* ───────────────── مشترك ───────────────── */

/// صفُّ حوالةٍ — يقرأ مفاتيحَ الخادم المختلفة بلا نموذجٍ ثالث.
///
/// ⚠ ومفاتيحُ الخادم غيرُ موحّدة بين المسارات (`Code` و`transfer_number`،
/// `OverallVal` و`amount`). فتُقرأ الاحتمالاتُ كلُّها بدل أن يُفرض توحيدٌ
/// على مساراتٍ قائمة يستعملها غيرُنا.
class _TransferRow extends StatelessWidget {
  const _TransferRow({required this.m, this.big = false});

  final Map<String, dynamic> m;
  final bool big;

  String get _code =>
      '${m['transfer_number'] ?? m['Code'] ?? m['code'] ?? ''}'.trim();

  double get _amount => Fmt.num_(
      m['amount'] ?? m['OverallVal'] ?? m['Amount'] ?? m['value']);

  String get _name => '${m['receiver_name'] ??
      m['beneficiary_name'] ??
      m['reviced_name'] ??
      m['RevicedName'] ??
      ''}'
      .trim();

  String get _action => '${m['action'] ?? ''}';

  @override
  Widget build(BuildContext context) {
    final delivered = _action == 'DELIVERED';
    final tone = delivered ? R.error : R.primaryDark;

    final body = Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Expanded(
              child: Text(_name.isEmpty ? 'حوالة' : _name,
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: T.kufi(big ? 15 : 13.5, FontWeight.w700)),
            ),
            if (_amount != 0)
              Directionality(
                textDirection: TextDirection.ltr,
                child: Text(Fmt.money(_amount),
                    style: T.kufi(big ? 18 : 14, FontWeight.w800, color: tone)),
              ),
          ],
        ),
        if (_code.isNotEmpty) ...[
          const SizedBox(height: 4),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Text(_code,
                style: T.plex(11.5, FontWeight.w400, color: R.inkA(.55))),
          ),
        ],
        if (_action.isNotEmpty) ...[
          const SizedBox(height: 3),
          Text(delivered ? 'حوالة سلَّمتُها' : 'حوالة أنشأتُها',
              style: T.plex(11.5, FontWeight.w600, color: tone)),
        ],
        if (m['at'] != null || m['occurred_at'] != null) ...[
          const SizedBox(height: 3),
          Text(Fmt.stampShort('${m['at'] ?? m['occurred_at']}'),
              style: T.plex(11, FontWeight.w400, color: R.inkA(.45))),
        ],
      ],
    );

    return big
        ? GlassCard(child: body)
        : Container(
            padding: const EdgeInsets.all(12),
            decoration: BoxDecoration(
              color: R.whiteA(.55),
              border: Border.all(color: R.inkA(.08)),
              borderRadius: BorderRadius.circular(R.rCard),
            ),
            child: body,
          );
  }
}

class _Msg extends StatelessWidget {
  const _Msg({required this.icon, required this.text});
  final IconData icon;
  final String text;

  @override
  Widget build(BuildContext context) => ListView(
        padding: const EdgeInsets.fromLTRB(30, 70, 30, 30),
        children: [
          Icon(icon, size: 42, color: R.inkA(.24)),
          const SizedBox(height: 14),
          Text(text,
              textAlign: TextAlign.center,
              style: T.plex(13, FontWeight.w500,
                  color: R.inkA(.5), height: 1.8)),
        ],
      );
}
