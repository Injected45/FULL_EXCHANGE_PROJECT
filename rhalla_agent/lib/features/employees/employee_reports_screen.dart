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

/// «متابعة الموظفين» — لوحة الوكيل وتقرير نقاط البيع في شاشة واحدة.
///
/// ⚠ **أرقامٌ تشغيلية لا محاسبية.** ما هنا يجيب «كم سلّم هذا الموظف» و«كم
/// نقداً يُتوقّع في يده» — لا رصيد الوكيل لدى الرحالة، وله شاشته. والخلط
/// بين السؤالين هو ما يجعل تقريراً صحيحاً يبدو خاطئاً.
class EmployeeReportsScreen extends ConsumerStatefulWidget {
  const EmployeeReportsScreen({super.key});

  @override
  ConsumerState<EmployeeReportsScreen> createState() =>
      _EmployeeReportsScreenState();
}

class _EmployeeReportsScreenState extends ConsumerState<EmployeeReportsScreen> {
  static const _periods = {
    'today': 'اليوم',
    'yesterday': 'أمس',
    'week': 'هذا الأسبوع',
    'month': 'هذا الشهر',
  };

  String _period = 'today';

  Map<String, dynamic>? _dash;
  Map<String, dynamic>? _report;
  bool _loading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    setState(() { _loading = true; _error = null; });
    try {
      final api = ref.read(apiClientProvider);
      // نداءان متوازيان لا متتاليان: الشاشة تنتظر أبطأهما لا مجموعهما.
      final results = await Future.wait([
        api.get('/employees/dashboard'),
        api.get('/employees/reports/points-of-sale', query: {'period': _period}),
      ]);
      if (!mounted) return;
      setState(() {
        _dash = results[0].row ?? const {};
        _report = results[1].row ?? const {};
        _loading = false;
      });
    } on ApiFailure catch (e) {
      if (mounted) setState(() { _error = e.message; _loading = false; });
    } catch (_) {
      if (mounted) {
        setState(() { _error = 'تعذّر الاتصال بالخادم.'; _loading = false; });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final rows = ((_report?['rows'] as List?) ?? const [])
        .whereType<Map>()
        .map((e) => e.cast<String, dynamic>())
        .toList();

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'متابعة الموظفين', onBack: () => context.pop()),
          Expanded(
            child: _loading
                ? Center(child: CircularProgressIndicator(color: R.primary))
                : _error != null
                    ? _Failed(message: _error!, onRetry: _load)
                    : RefreshIndicator(
                        onRefresh: _load,
                        color: R.primary,
                        backgroundColor: Colors.white,
                        child: ListView(
                          padding: const EdgeInsets.fromLTRB(
                              R.padScreen, 12, R.padScreen, 40),
                          physics: const AlwaysScrollableScrollPhysics(),
                          children: [
                            if (_dash != null) _Dashboard(d: _dash!),
                            const SizedBox(height: 18),

                            Text('تقرير نقاط البيع', style: T.section),
                            const SizedBox(height: 10),
                            _PeriodBar(
                              periods: _periods,
                              current: _period,
                              onPick: (p) {
                                setState(() => _period = p);
                                _load();
                              },
                            ),
                            const SizedBox(height: 12),

                            if (rows.isEmpty)
                              const _NoEmployees()
                            else
                              for (final r in rows) ...[
                                _EmployeeRow(r: r),
                                const SizedBox(height: R.gapRow),
                              ],
                          ],
                        ),
                      ),
          ),
        ],
      ),
    );
  }
}

class _Dashboard extends StatelessWidget {
  const _Dashboard({required this.d});

  final Map<String, dynamic> d;

  int _i(String k) => int.tryParse('${d[k] ?? 0}') ?? 0;
  double _n(String k) => double.tryParse('${d[k] ?? 0}') ?? 0;

  @override
  Widget build(BuildContext context) => Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            children: [
              Expanded(child: _Stat(
                label: 'نقاط بيع نشطة',
                value: '${_i('active_points_of_sale')}',
                icon: Icons.storefront_outlined,
              )),
              const SizedBox(width: 10),
              Expanded(child: _Stat(
                label: 'موظفون نشطون',
                value: '${_i('active_employees')} / ${_i('total_employees')}',
                icon: Icons.groups_2_outlined,
              )),
            ],
          ),
          const SizedBox(height: 10),
          Row(
            children: [
              Expanded(child: _Stat(
                label: 'بانتظار التسليم',
                value: '${_i('pending_transfers')}',
                icon: Icons.schedule_rounded,
              )),
              const SizedBox(width: 10),
              Expanded(child: _Stat(
                label: 'سُلّمت اليوم',
                value: '${_i('delivered_today_count')}',
                icon: Icons.check_circle_outline_rounded,
              )),
            ],
          ),
          const SizedBox(height: 10),
          GlassCard(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                _MoneyRow(
                  label: 'قيمة ما سُلّم اليوم',
                  value: _n('delivered_today_total'),
                ),
                const SizedBox(height: 8),
                _MoneyRow(
                  label: 'النقد المتوقّع لدى الموظفين',
                  value: _n('expected_cash_total'),
                  strong: true,
                ),
                if (_i('differences_today') > 0) ...[
                  const SizedBox(height: 10),
                  Divider(color: R.inkA(.08), height: 1),
                  const SizedBox(height: 10),
                  // الفروق تُبرز لا تُدفن: هي ما يستحقّ نظر الوكيل أوّلاً.
                  Row(
                    children: [
                      Icon(Icons.warning_amber_rounded,
                          size: 16, color: R.warnIcon),
                      const SizedBox(width: 8),
                      Expanded(
                        child: Text(
                          '${_i('differences_today')} إقفال بفرق اليوم',
                          style: T.plex(12.5, FontWeight.w600,
                              color: R.warnIcon),
                        ),
                      ),
                      Directionality(
                        textDirection: TextDirection.ltr,
                        child: Text(Fmt.money(_n('differences_total')),
                            style: T.kufi(13, FontWeight.w700,
                                color: R.warnIcon)),
                      ),
                    ],
                  ),
                ],
              ],
            ),
          ),
        ],
      );
}

class _Stat extends StatelessWidget {
  const _Stat({required this.label, required this.value, required this.icon});

  final String label;
  final String value;
  final IconData icon;

  @override
  Widget build(BuildContext context) => GlassCard(
        padding: const EdgeInsets.fromLTRB(14, 14, 14, 14),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Icon(icon, size: 18, color: R.primaryDark),
            const SizedBox(height: 10),
            Directionality(
              textDirection: TextDirection.ltr,
              child: Text(value,
                  style: T.kufi(20, FontWeight.w800, color: R.primaryDark)),
            ),
            const SizedBox(height: 3),
            Text(label,
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
                style: T.plex(11, FontWeight.w500, color: R.inkA(.55))),
          ],
        ),
      );
}

class _MoneyRow extends StatelessWidget {
  const _MoneyRow({
    required this.label,
    required this.value,
    this.strong = false,
  });

  final String label;
  final double value;
  final bool strong;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          Expanded(
            child: Text(label,
                style: strong
                    ? T.kufi(13.5, FontWeight.w700)
                    : T.plex(12.5, FontWeight.w400, color: R.inkA(.6))),
          ),
          // رمز العملة يسار الرقم دائماً — قاعدة المشروع.
          Directionality(
            textDirection: TextDirection.ltr,
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Text('د.ل ',
                    style: T.plex(11, FontWeight.w500, color: R.inkA(.5))),
                Text(Fmt.money(value),
                    style: strong
                        ? T.kufi(16, FontWeight.w800, color: R.primaryDark)
                        : T.kufi(13, FontWeight.w600)),
              ],
            ),
          ),
        ],
      );
}

class _PeriodBar extends StatelessWidget {
  const _PeriodBar({
    required this.periods,
    required this.current,
    required this.onPick,
  });

  final Map<String, String> periods;
  final String current;
  final ValueChanged<String> onPick;

  @override
  Widget build(BuildContext context) => SingleChildScrollView(
        scrollDirection: Axis.horizontal,
        child: Row(
          children: [
            for (final e in periods.entries) ...[
              GestureDetector(
                onTap: () => onPick(e.key),
                child: AnimatedContainer(
                  duration: const Duration(milliseconds: 160),
                  padding:
                      const EdgeInsets.symmetric(horizontal: 16, vertical: 9),
                  decoration: BoxDecoration(
                    gradient: e.key == current ? R.primaryGradient : null,
                    color: e.key == current ? null : R.whiteA(.66),
                    border: Border.all(
                        color: e.key == current
                            ? Colors.transparent
                            : R.inkA(.08)),
                    borderRadius: BorderRadius.circular(R.rPill),
                  ),
                  child: Text(e.value,
                      style: T.plex(12, FontWeight.w600,
                          color: e.key == current
                              ? Colors.white
                              : R.inkA(.6))),
                ),
              ),
              const SizedBox(width: 8),
            ],
          ],
        ),
      );
}

class _EmployeeRow extends StatelessWidget {
  const _EmployeeRow({required this.r});

  final Map<String, dynamic> r;

  int _i(String k) => int.tryParse('${r[k] ?? 0}') ?? 0;
  double _n(String k) => double.tryParse('${r[k] ?? 0}') ?? 0;

  @override
  Widget build(BuildContext context) {
    final hasShift = r['has_open_shift'] == true;

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            children: [
              IconTile(
                size: 34,
                background: R.primaryA(.12),
                icon: Icon(Icons.badge_outlined, size: 17, color: R.primaryDark),
              ),
              const SizedBox(width: 11),
              Expanded(
                child: Text('${r['full_name'] ?? ''}',
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: T.kufi(14, FontWeight.w700)),
              ),
              if (hasShift)
                Container(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 9, vertical: 4),
                  decoration: BoxDecoration(
                    color: R.primaryA(.10),
                    border: Border.all(color: R.primaryA(.28)),
                    borderRadius: BorderRadius.circular(99),
                  ),
                  child: Text('وردية مفتوحة',
                      style: T.plex(10, FontWeight.w600, color: R.primaryDark)),
                ),
            ],
          ),
          const SizedBox(height: 10),
          Divider(color: R.inkA(.07), height: 1),
          const SizedBox(height: 10),

          _line('حوالات سلّمها', '${_i('delivered_count')}'),
          const SizedBox(height: 6),
          _money('قيمة ما سلّمه', _n('delivered_total')),
          const SizedBox(height: 6),
          _money('نقد مستلم', _n('cash_in'), tone: R.primaryDark),
          const SizedBox(height: 6),
          _money('نقد مسلَّم', _n('cash_out'), tone: R.error),
          const SizedBox(height: 8),
          Divider(color: R.inkA(.07), height: 1),
          const SizedBox(height: 8),

          // بلا وردية لا افتتاحيّ، ورقمُ «متوقّع» بلا افتتاحيّ يضلّل — فيُقال
          // ذلك صراحةً بدل عرض صفرٍ يبدو حقيقة.
          if (hasShift)
            _money('النقد المتوقّع لديه', _n('expected_cash'), strong: true)
          else
            Row(
              children: [
                Icon(Icons.info_outline_rounded, size: 14, color: R.inkA(.45)),
                const SizedBox(width: 7),
                Expanded(
                  child: Text('لا وردية مفتوحة — لا يُحسب نقدٌ متوقّع',
                      style: T.plex(11.5, FontWeight.w400,
                          color: R.inkA(.5))),
                ),
              ],
            ),
        ],
      ),
    );
  }

  Widget _line(String label, String value) => Row(
        children: [
          Expanded(
            child: Text(label,
                style: T.plex(12, FontWeight.w400, color: R.inkA(.6))),
          ),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Text(value, style: T.kufi(13, FontWeight.w700)),
          ),
        ],
      );

  Widget _money(String label, double v, {Color? tone, bool strong = false}) =>
      Row(
        children: [
          Expanded(
            child: Text(label,
                style: strong
                    ? T.kufi(13, FontWeight.w700)
                    : T.plex(12, FontWeight.w400, color: R.inkA(.6))),
          ),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Text('د.ل ',
                    style: T.plex(10.5, FontWeight.w500, color: R.inkA(.5))),
                Text(Fmt.money(v),
                    style: strong
                        ? T.kufi(15, FontWeight.w800, color: R.primaryDark)
                        : T.kufi(12.5, FontWeight.w600, color: tone ?? R.ink)),
              ],
            ),
          ),
        ],
      );
}

class _NoEmployees extends StatelessWidget {
  const _NoEmployees();

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.symmetric(vertical: 40),
        child: Column(
          children: [
            Icon(Icons.insert_chart_outlined_rounded,
                size: 40, color: R.primaryA(.3)),
            const SizedBox(height: 14),
            Text('لا بيانات في هذه الفترة',
                style: T.kufi(15, FontWeight.w600, color: R.inkA(.6))),
          ],
        ),
      );
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.all(R.padScreen),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(Icons.error_outline_rounded, size: 38, color: R.error),
            const SizedBox(height: 14),
            Text(message,
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w500,
                    color: R.inkA(.65), height: 1.7)),
            const SizedBox(height: 18),
            GlassButton(label: 'إعادة المحاولة', onPressed: onRetry),
          ],
        ),
      );
}
