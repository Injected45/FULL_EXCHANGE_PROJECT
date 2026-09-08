import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import 'approvals_repository.dart';

/// «سقف التحويل» — ورقةُ ضبط سياسة موظفٍ واحد.
///
/// ⚠ **حقلٌ فارغ = بلا سقف، وهو الحالُ الافتراضيّ لكل موظف.** والصفرُ ليس
/// بديلاً عنه: صفرٌ يعني «لا يُسمح له بحوالةٍ واحدة»، وهو قرارٌ آخر قد يريده
/// الوكيل فعلاً لموظفٍ يوقفه مؤقّتاً دون سحب صلاحيته.
///
/// ⚠ **ولا يمنع السقفُ حوالةً ولا يُلغيها.** ما يتجاوزه يذهب إلى الوكيل
/// طلبَ موافقة — يوافق فتُنفَّذ، أو يرفض فلا تُنفَّذ. والموظفُ لا يُعيد إدخال
/// شيء في الحالتين. وهذه الجملةُ مكتوبةٌ في الورقة نفسِها لأنّ وكيلاً يظنّ
/// السقفَ منعاً لن يضع سقفاً أبداً.
class EmployeeLimitsSheet extends ConsumerStatefulWidget {
  const EmployeeLimitsSheet({
    super.key,
    required this.employeeId,
    required this.employeeName,
  });

  final int employeeId;
  final String employeeName;

  @override
  ConsumerState<EmployeeLimitsSheet> createState() => _EmployeeLimitsSheetState();
}

class _EmployeeLimitsSheetState extends ConsumerState<EmployeeLimitsSheet> {
  final _per = TextEditingController();
  final _cum = TextEditingController();
  late final _perFocus = AutoClearFocus(_per, formatOnExit: true);
  late final _cumFocus = AutoClearFocus(_cum, formatOnExit: true);

  /// نافذةُ السقف التراكميّ — يختارها الوكيل، ولا تُخترع له.
  int? _cumHours;

  int _recipientMinutes = 60;
  int _ttlHours = 24;

  bool _loaded = false;
  bool _busy = false;
  String? _error;

  static const _windows = <int, String>{
    24: 'يوم واحد',
    72: 'ثلاثة أيام',
    168: 'أسبوع',
    720: 'شهر',
  };

  @override
  void dispose() {
    _perFocus.dispose();
    _cumFocus.dispose();
    _per.dispose();
    _cum.dispose();
    super.dispose();
  }

  void _fill(EmployeeLimits l) {
    if (_loaded) return;
    _loaded = true;
    if (l.perTransfer != null) _per.text = Fmt.money(l.perTransfer!);
    if (l.cumulative != null) _cum.text = Fmt.money(l.cumulative!);
    _cumHours = l.cumulativeHours;
    _recipientMinutes = l.recipientMinutes;
    _ttlHours = l.approvalTtlHours;
  }

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(employeeLimitsProvider(widget.employeeId));

    return Container(
      padding: EdgeInsets.fromLTRB(
          22, 22, 22, 26 + MediaQuery.viewInsetsOf(context).bottom),
      decoration: BoxDecoration(
        color: R.whiteA(.96),
        borderRadius: const BorderRadius.vertical(top: Radius.circular(R.rNav)),
      ),
      child: SingleChildScrollView(
        child: async.when(
          loading: () => const Padding(
            padding: EdgeInsets.all(40),
            child: Center(child: CircularProgressIndicator()),
          ),
          error: (e, _) => Padding(
            padding: const EdgeInsets.all(30),
            child: Text('تعذّر تحميل السقف. $e',
                textAlign: TextAlign.center,
                style: T.kufi(13, FontWeight.w600, color: R.error)),
          ),
          data: (l) {
            _fill(l);
            return _form(l);
          },
        ),
      ),
    );
  }

  Widget _form(EmployeeLimits l) => Column(
        mainAxisSize: MainAxisSize.min,
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Center(
            child: Container(
              width: 44,
              height: 4,
              decoration: BoxDecoration(
                color: R.inkA(.16),
                borderRadius: BorderRadius.circular(99),
              ),
            ),
          ),
          const SizedBox(height: 20),
          Column(
            children: [
              Text('سقف التحويل',
                  textAlign: TextAlign.center,
                  style: T.kufi(16, FontWeight.w700)),
              const SizedBox(height: 4),
              Text(widget.employeeName,
                  textAlign: TextAlign.center,
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                  style: T.kufi(14, FontWeight.w600, color: R.inkA(.62))),
            ],
          ),
          const SizedBox(height: 14),

          // ⚠ الجملةُ التي تجعل الوكيل يضع سقفاً بدل أن يخافه.
          Container(
            padding: const EdgeInsets.all(12),
            decoration: BoxDecoration(
              color: R.primaryA(.06),
              border: Border.all(color: R.primaryA(.20)),
              borderRadius: BorderRadius.circular(R.rCard),
            ),
            child: Text(
              'ما يتجاوز السقف لا يُرفض ولا يُحذف — يصلك طلب موافقة، '
              'فإن وافقت نُفِّذت الحوالة، وإن رفضت لم تُنفَّذ. '
              'والموظف لا يُعيد إدخال شيء.',
              style: T.plex(12, FontWeight.w400, color: R.inkA(.68), height: 1.8),
            ),
          ),
          const SizedBox(height: 18),

          _MoneyField(
            label: 'سقف الحوالة الواحدة',
            hint: 'اتركه فارغاً — بلا سقف',
            controller: _per,
            focusNode: _perFocus,
          ),
          const SizedBox(height: R.gapCard),

          _MoneyField(
            label: 'السقف التراكمي (اختياري)',
            hint: 'اتركه فارغاً — بلا سقف تراكمي',
            controller: _cum,
            focusNode: _cumFocus,
          ),

          // ⚠ ولا تظهر النافذةُ إلّا مع سقفٍ تراكميّ: «لا يتجاوز 10,000»
          // بلا مدّة سؤالٌ بلا جواب، والخادمُ يرفضه.
          if (_cum.text.trim().isNotEmpty || _cumHours != null) ...[
            const SizedBox(height: 12),
            Text('تُحتسب خلال',
                style: T.plex(12, FontWeight.w600, color: R.inkA(.55))),
            const SizedBox(height: 8),
            Wrap(
              spacing: 8,
              runSpacing: 8,
              children: _windows.entries
                  .map((e) => _Chip(
                        label: e.value,
                        selected: _cumHours == e.key,
                        onTap: () => setState(() => _cumHours = e.key),
                      ))
                  .toList(),
            ),
          ],

          const SizedBox(height: 20),
          Divider(color: R.inkA(.08)),
          const SizedBox(height: 14),

          Text('الحماية من تقسيم الحوالة',
              style: T.kufi(13.5, FontWeight.w700)),
          const SizedBox(height: 6),
          Text(
            'إذا حوّل الموظف إلى نفس المستفيد أكثر من مرة خلال هذه المدة، '
            'يصلك طلب موافقة — حتى لو كان كل مبلغ تحت السقف.',
            style: T.plex(11.5, FontWeight.w400, color: R.inkA(.55), height: 1.7),
          ),
          const SizedBox(height: 10),
          Wrap(
            spacing: 8,
            runSpacing: 8,
            children: const <int, String>{
              0: 'بلا مراقبة',
              30: 'نصف ساعة',
              60: 'ساعة',
              180: 'ثلاث ساعات',
              1440: 'يوم',
            }
                .entries
                .map((e) => _Chip(
                      label: e.value,
                      selected: _recipientMinutes == e.key,
                      onTap: () => setState(() => _recipientMinutes = e.key),
                    ))
                .toList(),
          ),

          const SizedBox(height: 18),
          Text('مدة صلاحية طلب الموافقة',
              style: T.kufi(13.5, FontWeight.w700)),
          const SizedBox(height: 6),
          Text(
            'الطلب الذي لا تبتّ فيه خلال هذه المدة تنتهي صلاحيته ولا يُنفَّذ.',
            style: T.plex(11.5, FontWeight.w400, color: R.inkA(.55), height: 1.7),
          ),
          const SizedBox(height: 10),
          Wrap(
            spacing: 8,
            runSpacing: 8,
            children: const <int, String>{
              4: 'أربع ساعات',
              12: 'اثنتا عشرة ساعة',
              24: 'يوم',
              72: 'ثلاثة أيام',
            }
                .entries
                .map((e) => _Chip(
                      label: e.value,
                      selected: _ttlHours == e.key,
                      onTap: () => setState(() => _ttlHours = e.key),
                    ))
                .toList(),
          ),

          if (_error != null) ...[
            const SizedBox(height: 14),
            _ErrorLine(_error!),
          ],

          const SizedBox(height: 20),
          if (!l.canEdit)
            Text(
              'تعديل السقف من الحساب الرئيسي فقط.',
              textAlign: TextAlign.center,
              style: T.plex(12.5, FontWeight.w600, color: R.warnIcon),
            )
          else
            PrimaryButton(
              label: 'حفظ السقف',
              loading: _busy,
              onPressed: _busy ? null : _save,
            ),
          const SizedBox(height: 10),
          TextButton(
            onPressed: () => Navigator.of(context).pop(),
            style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
            child: Text('إغلاق',
                style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
          ),
        ],
      );

  Future<void> _save() async {
    // ⚠ يُقرأ عبر `Fmt.num_` لا من النصّ مباشرةً: حقلُ المال يحمل فواصل
    // آلافٍ يضعها المنسّق، وقراءتُه خاماً تُرسل رقماً غيرَ الذي كُتب.
    final perText = _per.text.trim();
    final cumText = _cum.text.trim();

    final per = perText.isEmpty ? null : Fmt.num_(perText);
    final cum = cumText.isEmpty ? null : Fmt.num_(cumText);

    if (cum != null && _cumHours == null) {
      setState(() => _error = 'اختر فترة احتساب السقف التراكمي.');
      return;
    }

    setState(() {
      _busy = true;
      _error = null;
    });

    try {
      await ref.read(approvalsRepositoryProvider).saveLimits(
            widget.employeeId,
            perTransfer: per,
            cumulative: cum,
            cumulativeHours: cum == null ? null : _cumHours,
            recipientMinutes: _recipientMinutes,
            approvalTtlHours: _ttlHours,
          );

      ref.invalidate(employeeLimitsProvider(widget.employeeId));
      if (!mounted) return;
      Navigator.of(context).pop(true);
    } on ApiFailure catch (e) {
      if (mounted) setState(() { _busy = false; _error = e.message; });
    } catch (_) {
      if (mounted) {
        setState(() {
          _busy = false;
          _error = 'تعذّر الحفظ — تحقّق من الاتصال.';
        });
      }
    }
  }
}

class _MoneyField extends StatelessWidget {
  const _MoneyField({
    required this.label,
    required this.hint,
    required this.controller,
    required this.focusNode,
  });

  final String label;
  final String hint;
  final TextEditingController controller;
  final FocusNode focusNode;

  @override
  Widget build(BuildContext context) => Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(label, style: T.plex(12.5, FontWeight.w600, color: R.inkA(.62))),
          const SizedBox(height: 6),
          TextField(
            controller: controller,
            focusNode: focusNode,
            keyboardType: const TextInputType.numberWithOptions(decimal: true),
            // ⚠ القائمةُ المشتركة لا قائمةٌ خاصّة: ترتيبُها صحّةٌ لا ذوق.
            inputFormatters: moneyInputFormatters,
            textDirection: TextDirection.ltr,
            style: T.kufi(15, FontWeight.w700),
            decoration: InputDecoration(
              hintText: hint,
              hintStyle: T.plex(12, FontWeight.w400, color: R.inkA(.32)),
              // العملةُ يسارَ الرقم — كسائر مبالغ التطبيق.
              prefixText: 'د.ل  ',
              prefixStyle: T.kufi(13, FontWeight.w700, color: R.primaryDark),
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
              contentPadding:
                  const EdgeInsets.symmetric(horizontal: 14, vertical: 14),
            ),
          ),
        ],
      );
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
          padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 9),
          decoration: BoxDecoration(
            color: selected ? R.primaryA(.12) : R.whiteA(.7),
            border: Border.all(
                color: selected ? R.primaryA(.45) : R.inkA(.12),
                width: selected ? 1.4 : 1),
            borderRadius: BorderRadius.circular(99),
          ),
          child: Text(label,
              style: T.plex(12, selected ? FontWeight.w700 : FontWeight.w500,
                  color: selected ? R.primaryDark : R.inkA(.6))),
        ),
      );
}

class _ErrorLine extends StatelessWidget {
  const _ErrorLine(this.message);
  final String message;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(12),
        decoration: BoxDecoration(
          color: R.error.withValues(alpha: .07),
          border: Border.all(color: R.error.withValues(alpha: .3)),
          borderRadius: BorderRadius.circular(R.rCard),
        ),
        child: Row(
          children: [
            Icon(Icons.error_outline_rounded, size: 17, color: R.error),
            const SizedBox(width: 8),
            Expanded(
              child: Text(message,
                  style: T.plex(12.5, FontWeight.w500, color: R.error, height: 1.6)),
            ),
          ],
        ),
      );
}
