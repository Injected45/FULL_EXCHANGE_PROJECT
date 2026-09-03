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

/// بدء الوردية.
///
/// الرصيد الافتتاحي **تصريحٌ من الموظف** لا رقمٌ يُنقل من إقفال أمس: النقل
/// الآلي يُخفي أي فرقٍ حدث بين الورديتين، وهو بالضبط ما يُراد كشفه.
class StartShiftScreen extends ConsumerStatefulWidget {
  const StartShiftScreen({super.key});

  @override
  ConsumerState<StartShiftScreen> createState() => _StartShiftScreenState();
}

class _StartShiftScreenState extends ConsumerState<StartShiftScreen> {
  final _opening = TextEditingController();
  late final _focus = AutoClearFocus(_opening, formatOnExit: true);

  bool _busy = false;
  String? _error;

  @override
  void dispose() {
    _focus.dispose();
    _opening.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final profile = ref.watch(employeeAuthProvider).profile;

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'بدء وردية', onBack: () => context.pop()),
          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 18, R.padScreen, 40),
              children: [
                Text('كم النقد الذي في يدك الآن؟',
                    textAlign: TextAlign.center,
                    style: T.kufi(16, FontWeight.w700)),
                const SizedBox(height: 8),
                Text('عُدّ ما في الصندوق واكتبه. هذا الرقم أساس حساب '
                    'النقد المتوقّع عند الإقفال.',
                    textAlign: TextAlign.center,
                    style: T.plex(12.5, FontWeight.w400,
                        color: R.inkA(.55), height: 1.8)),
                const SizedBox(height: 22),

                _MoneyField(
                  label: 'الرصيد الافتتاحي',
                  controller: _opening,
                  focusNode: _focus,
                ),

                if (profile != null && profile.posName.isNotEmpty) ...[
                  const SizedBox(height: R.gapCard),
                  GlassCard(
                    child: Row(
                      children: [
                        Icon(Icons.storefront_outlined,
                            size: 17, color: R.inkA(.5)),
                        const SizedBox(width: 9),
                        Expanded(
                          child: Text('نقطة البيع · ${profile.posName}',
                              style: T.plex(12.5, FontWeight.w500,
                                  color: R.inkA(.6))),
                        ),
                      ],
                    ),
                  ),
                ],

                if (_error != null) ...[
                  const SizedBox(height: 14),
                  _ErrorBox(message: _error!),
                ],

                const SizedBox(height: 22),
                PrimaryButton(
                  label: 'بدء الوردية',
                  loading: _busy,
                  onPressed: _busy ? null : _start,
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _start() async {
    // `Fmt.num_` تزيل فواصل الآلاف — قراءة `text` مباشرةً تُرسل رقماً آخر.
    final opening = Fmt.num_(_opening.text);
    if (opening < 0) {
      setState(() => _error = 'الرصيد الافتتاحي لا يكون سالباً.');
      return;
    }

    setState(() { _busy = true; _error = null; });
    try {
      await ref.read(apiClientProvider).post(
        '/device/employee/shift/start',
        body: {'opening_cash': opening},
      );
      await ref.read(employeeAuthProvider.notifier).refresh();
      if (mounted) context.pop();
    } on ApiFailure catch (e) {
      if (mounted) setState(() { _busy = false; _error = e.message; });
    } catch (_) {
      if (mounted) {
        setState(() { _busy = false; _error = 'تعذّر الاتصال بالخادم.'; });
      }
    }
  }
}

/// إقفال الوردية.
///
/// المتوقّع يُعرض **قبل** أن يكتب الموظف الفعلي، لأنه معلومةٌ يحقّ له رؤيتها:
/// إخفاؤه لا يجعل العدّ أصدق، لكنه يجعل الموظف يجهل ما يُقارن به.
class CloseShiftScreen extends ConsumerStatefulWidget {
  const CloseShiftScreen({super.key});

  @override
  ConsumerState<CloseShiftScreen> createState() => _CloseShiftScreenState();
}

class _CloseShiftScreenState extends ConsumerState<CloseShiftScreen> {
  final _actual = TextEditingController();
  late final _focus = AutoClearFocus(_actual, formatOnExit: true);

  Map<String, dynamic>? _summary;
  bool _loading = true;
  bool _busy = false;
  String? _error;

  /// نتيجة الإقفال بعد نجاحه — تُعرض في الشاشة نفسها.
  Map<String, dynamic>? _result;

  @override
  void initState() {
    super.initState();
    _load();
  }

  @override
  void dispose() {
    _focus.dispose();
    _actual.dispose();
    super.dispose();
  }

  Future<void> _load() async {
    try {
      final env =
          await ref.read(apiClientProvider).get('/device/employee/cashbox');
      if (!mounted) return;
      setState(() {
        _summary = (env.row?['summary'] as Map?)?.cast<String, dynamic>();
        _loading = false;
      });
    } catch (_) {
      if (mounted) setState(() => _loading = false);
    }
  }

  double _n(String k) => double.tryParse('${_summary?[k] ?? 0}') ?? 0;

  @override
  Widget build(BuildContext context) {
    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'إقفال الوردية',
            onBack: () => context.pop(),
          ),
          Expanded(
            child: _loading
                ? Center(child: CircularProgressIndicator(color: R.primary))
                : ListView(
                    padding: const EdgeInsets.fromLTRB(
                        R.padScreen, 18, R.padScreen, 40),
                    children: _result != null
                        ? [_ResultCard(result: _result!)]
                        : _form(),
                  ),
          ),
        ],
      ),
    );
  }

  List<Widget> _form() => [
        GlassCard(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              _row('الرصيد الافتتاحي', _n('opening')),
              const SizedBox(height: 8),
              _row('النقد المستلم', _n('in'), tone: R.primaryDark),
              const SizedBox(height: 8),
              _row('النقد المسلَّم', _n('out'), tone: R.error),
              const SizedBox(height: 10),
              Divider(color: R.inkA(.08), height: 1),
              const SizedBox(height: 10),
              _row('النقد المتوقّع', _n('expected'), strong: true),
            ],
          ),
        ),
        const SizedBox(height: 16),
        Text('كم النقد الموجود فعلاً؟',
            textAlign: TextAlign.center, style: T.kufi(15, FontWeight.w700)),
        const SizedBox(height: 8),
        Text('عُدّ الصندوق واكتب ما وجدته. الفرق يُحفظ كما هو.',
            textAlign: TextAlign.center,
            style: T.plex(12.5, FontWeight.w400,
                color: R.inkA(.55), height: 1.8)),
        const SizedBox(height: 16),
        _MoneyField(
          label: 'النقد الفعلي',
          controller: _actual,
          focusNode: _focus,
        ),
        if (_error != null) ...[
          const SizedBox(height: 14),
          _ErrorBox(message: _error!),
        ],
        const SizedBox(height: 22),
        PrimaryButton(
          label: 'إقفال الوردية',
          loading: _busy,
          onPressed: _busy ? null : _close,
        ),
      ];

  Widget _row(String label, double value, {Color? tone, bool strong = false}) =>
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

  Future<void> _close() async {
    final actual = Fmt.num_(_actual.text);
    if (actual < 0) {
      setState(() => _error = 'المبلغ لا يكون سالباً.');
      return;
    }

    setState(() { _busy = true; _error = null; });
    try {
      final env = await ref.read(apiClientProvider).post(
        '/device/employee/shift/close',
        body: {'actual_cash': actual},
      );
      await ref.read(employeeAuthProvider.notifier).refresh();
      if (!mounted) return;
      setState(() { _busy = false; _result = env.row ?? const {}; });
    } on ApiFailure catch (e) {
      if (mounted) setState(() { _busy = false; _error = e.message; });
    } catch (_) {
      if (mounted) {
        setState(() { _busy = false; _error = 'تعذّر الاتصال بالخادم.'; });
      }
    }
  }
}

/// نتيجة الإقفال — مطابق أو عجز أو زيادة.
///
/// اللون من ألوان الحالات الثابتة: الأخضر مطابق، والأحمر عجز، والبرتقالي
/// زيادة. الزيادة ليست خطأً لكنها ليست مطابقةً أيضاً.
class _ResultCard extends StatelessWidget {
  const _ResultCard({required this.result});

  final Map<String, dynamic> result;

  double _n(String k) => double.tryParse('${result[k] ?? 0}') ?? 0;

  @override
  Widget build(BuildContext context) {
    final kind = '${result['result'] ?? ''}';
    final label = '${result['label'] ?? ''}';
    final (tone, icon) = switch (kind) {
      'MATCH'    => (R.primaryDark, Icons.check_circle_rounded),
      'SHORTAGE' => (R.error, Icons.trending_down_rounded),
      _          => (R.warnIcon, Icons.trending_up_rounded),
    };

    return Column(
      children: [
        const SizedBox(height: 20),
        Icon(icon, size: 54, color: tone),
        const SizedBox(height: 16),
        Text(label, style: T.kufi(22, FontWeight.w800, color: tone)),
        const SizedBox(height: 24),
        GlassCard(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              _row('المتوقّع', _n('expected')),
              const SizedBox(height: 8),
              _row('الفعلي', _n('actual')),
              const SizedBox(height: 10),
              Divider(color: R.inkA(.08), height: 1),
              const SizedBox(height: 10),
              _row('الفرق', _n('difference'), tone: tone, strong: true),
            ],
          ),
        ),
        const SizedBox(height: 24),
        PrimaryButton(
          label: 'تمّ',
          onPressed: () => Navigator.of(context).pop(),
        ),
      ],
    );
  }

  Widget _row(String label, double value, {Color? tone, bool strong = false}) =>
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
                        ? T.kufi(17, FontWeight.w800, color: tone ?? R.ink)
                        : T.kufi(13.5, FontWeight.w600, color: tone ?? R.ink)),
              ],
            ),
          ),
        ],
      );
}

class _MoneyField extends StatelessWidget {
  const _MoneyField({
    required this.label,
    required this.controller,
    required this.focusNode,
  });

  final String label;
  final TextEditingController controller;
  final FocusNode focusNode;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(label, style: T.label),
            const SizedBox(height: 9),
            // رمز العملة على اليسار دائماً — قاعدة المشروع، ولذلك `prefixText`.
            Directionality(
              textDirection: TextDirection.ltr,
              child: TextField(
                controller: controller,
                focusNode: focusNode,
                keyboardType:
                    const TextInputType.numberWithOptions(decimal: true),
                inputFormatters: moneyInputFormatters,
                style: T.kufi(20, FontWeight.w800),
                decoration: InputDecoration(
                  isDense: true,
                  border: InputBorder.none,
                  prefixText: 'د.ل  ',
                  prefixStyle:
                      T.plex(13, FontWeight.w500, color: R.inkA(.5)),
                  hintText: '0.00',
                  hintStyle:
                      T.plex(17, FontWeight.w400, color: R.inkA(.32)),
                ),
              ),
            ),
          ],
        ),
      );
}

class _ErrorBox extends StatelessWidget {
  const _ErrorBox({required this.message});

  final String message;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
        decoration: BoxDecoration(
          color: R.error.withValues(alpha: .07),
          border: Border.all(color: R.error.withValues(alpha: .26)),
          borderRadius: BorderRadius.circular(R.rRow),
        ),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Icon(Icons.error_outline_rounded, size: 17, color: R.errorText),
            const SizedBox(width: 9),
            Expanded(
              child: Text(message,
                  style: T.plex(12.5, FontWeight.w500,
                      color: R.errorText, height: 1.7)),
            ),
          ],
        ),
      );
}
