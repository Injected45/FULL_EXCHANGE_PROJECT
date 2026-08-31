import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import 'pos_repository.dart';

class PosScreen extends ConsumerWidget {
  const PosScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final user = ref.watch(authControllerProvider).user;
    final async = ref.watch(posListProvider);

    // الخادم يرد 403 لغير الوكيل الرئيسي — نمنع الوصول قبل الطلب.
    if (user != null && !user.isMainAgent) {
      return Screen(
        child: Column(
          children: [
            const RhallaAppBar(title: 'نقاط البيع'),
            Expanded(
              child: Center(
                child: Padding(
                  padding: const EdgeInsets.fromLTRB(40, 0, 40, 90),
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(Icons.lock_outline_rounded, size: 44, color: R.inkA(.3)),
                      const SizedBox(height: 20),
                      Text('هذه الشاشة للوكيل الرئيسي',
                          textAlign: TextAlign.center,
                          style: T.kufi(15, FontWeight.w600, height: 1.5)),
                      const SizedBox(height: 10),
                      Text('نقاط البيع تُدار من حساب الوكالة الرئيسي.',
                          textAlign: TextAlign.center,
                          style: T.plex(12.5, FontWeight.w400,
                              color: R.inkA(.55), height: 1.7)),
                    ],
                  ),
                ),
              ),
            ),
          ],
        ),
      );
    }

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'نقاط البيع',
            subtitle: '${user?.branchName ?? ''} · المخوّلون لديك',
            trailing: _AddButton(
              onTap: () => _openAdd(context, ref),
            ),
          ),
          Expanded(
            child: async.when(
              loading: () => const Center(
                child: CircularProgressIndicator(color: R.primary),
              ),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(posListProvider),
              ),
              data: (list) => RefreshIndicator(
                onRefresh: () async => ref.refresh(posListProvider.future),
                color: R.primary,
                backgroundColor: Colors.white,
                child: ListView(
                  padding:
                      const EdgeInsets.fromLTRB(R.padScreen, 20, R.padScreen, 120),
                  physics: const AlwaysScrollableScrollPhysics(),
                  children: [
                    _Summary(list: list),
                    const SizedBox(height: 16),
                    const WarnBanner(
                      text:
                          'تعديل أي نقطة بيع — حتى إيقافها أو تشغيلها — يُلغي تسجيلها ويُجبرها على التسجيل من جديد.',
                    ),
                    const SizedBox(height: 20),
                    if (list.isEmpty)
                      const _Empty()
                    else
                      for (var i = 0; i < list.length; i++) ...[
                        if (i > 0) const SizedBox(height: R.gapRow),
                        RiseIn.small(
                          delay: Duration(milliseconds: 40 * i),
                          child: _PosRow(
                            pos: list[i],
                            onTap: () => _openEdit(context, ref, list[i]),
                          ),
                        ),
                      ],
                  ],
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _openAdd(BuildContext context, WidgetRef ref) async {
    final added = await showModalBottomSheet<bool>(
      context: context,
      isScrollControlled: true,
      // useRootNavigator: شريط التبويب السفلي يُرسم داخل الهيكل، فورقة
      // تُفتح على مُلاحِق الهيكل يغطّي الشريطُ أسفلَها — ومنه زر الإلغاء.
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => const _AddPosSheet(),
    );
    if (added == true) ref.invalidate(posListProvider);
  }

  Future<void> _openEdit(
      BuildContext context, WidgetRef ref, PointOfSale pos) async {
    final saved = await showModalBottomSheet<bool>(
      context: context,
      isScrollControlled: true,
      // useRootNavigator: شريط التبويب السفلي يُرسم داخل الهيكل، فورقة
      // تُفتح على مُلاحِق الهيكل يغطّي الشريطُ أسفلَها — ومنه زر الإلغاء.
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _EditPosSheet(pos: pos),
    );
    if (saved == true) ref.invalidate(posListProvider);
  }
}

class _AddButton extends StatelessWidget {
  const _AddButton({required this.onTap});

  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => DecoratedBox(
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(R.rPill),
          boxShadow: R.shPill,
        ),
        child: Material(
          color: Colors.transparent,
          borderRadius: BorderRadius.circular(R.rPill),
          child: InkWell(
            borderRadius: BorderRadius.circular(R.rPill),
            onTap: onTap,
            child: Ink(
              height: 44,
              decoration: BoxDecoration(
                gradient: R.primaryGradient,
                borderRadius: BorderRadius.circular(R.rPill),
              ),
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 18),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    const Icon(Icons.add_rounded, size: 17, color: Colors.white),
                    const SizedBox(width: 8),
                    Text('إضافة',
                        style: T.kufi(12, FontWeight.w600, color: Colors.white)),
                  ],
                ),
              ),
            ),
          ),
        ),
      );
}

class _Summary extends StatelessWidget {
  const _Summary({required this.list});

  final List<PointOfSale> list;

  @override
  Widget build(BuildContext context) {
    final active = list.where((p) => p.state == PosState.active).length;
    final waiting =
        list.where((p) => p.state == PosState.awaitingRegistration).length;
    final off = list.where((p) => p.state == PosState.suspended).length;

    Widget tile(String label, int n, Color c) => Expanded(
          child: GlassCard(
            padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 15),
            child: Column(
              children: [
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(Fmt.count(n),
                      style: T.kufi(22, FontWeight.w600, color: c)),
                ),
                const SizedBox(height: 9),
                Text(label,
                    textAlign: TextAlign.center,
                    style: T.plex(11, FontWeight.w400, color: R.inkA(.55))),
              ],
            ),
          ),
        );

    return Row(
      children: [
        tile('نشطة', active, R.primaryGradEnd),
        const SizedBox(width: R.gapRow),
        tile('بانتظار التسجيل', waiting, R.ink),
        const SizedBox(width: R.gapRow),
        tile('موقوفة', off, R.inkA(.5)),
      ],
    );
  }
}

class _PosRow extends StatelessWidget {
  const _PosRow({required this.pos, this.onTap});

  final PointOfSale pos;
  final VoidCallback? onTap;

  @override
  Widget build(BuildContext context) {
    final (label, fg, bg) = switch (pos.state) {
      PosState.active => ('نشطة', R.primaryDark, R.primaryA(.14)),
      PosState.awaitingRegistration =>
        ('بانتظار التسجيل', R.warnInk, R.warnBorder),
      PosState.suspended => ('موقوفة', R.inkA(.55), R.inkA(.07)),
    };
    final dimmed = pos.state == PosState.suspended;

    return GlassRow(
      onTap: onTap,
      children: [
        IconTile(
          letter: pos.initial,
          background: dimmed ? R.inkA(.07) : null,
          color: dimmed ? R.inkA(.5) : null,
        ),
        const SizedBox(width: 13),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                pos.name.isEmpty ? 'بلا اسم' : pos.name,
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
                style: dimmed
                    ? T.plex(13.5, FontWeight.w600, color: R.inkA(.55))
                    : T.name,
              ),
              const SizedBox(height: 7),
              Directionality(
                textDirection: TextDirection.ltr,
                child: Align(
                  alignment: AlignmentDirectional.centerStart,
                  child: Text('+218 ${Fmt.phone(pos.phone)}', style: T.meta),
                ),
              ),
            ],
          ),
        ),
        const SizedBox(width: 10),
        Container(
          padding: const EdgeInsets.symmetric(horizontal: 11, vertical: 6),
          decoration:
              BoxDecoration(color: bg, borderRadius: BorderRadius.circular(99)),
          child: Text(label, style: T.plex(11, FontWeight.w600, color: fg)),
        ),
        if (onTap != null) ...[
          const SizedBox(width: 6),
          Icon(Icons.chevron_left_rounded, size: 20, color: R.inkA(.32)),
        ],
      ],
    );
  }
}

/// تعديل نقطة بيع.
///
/// WARN: الخادم يعيد `Reg = 'NO'` في **كل** حفظ — ولو لم يتغيّر إلا `IsActive`
/// (depositController.php:349). أي أن تصحيح حرف في الاسم يُخرج نقطة البيع من
/// التطبيق ويُجبرها على إعادة التسجيل. لذلك: الحفظ خلف تأكيد ثانٍ، لا زر واحد.
class _EditPosSheet extends ConsumerStatefulWidget {
  const _EditPosSheet({required this.pos});

  final PointOfSale pos;

  @override
  ConsumerState<_EditPosSheet> createState() => _EditPosSheetState();
}

class _EditPosSheetState extends ConsumerState<_EditPosSheet> {
  late final _name = TextEditingController(text: widget.pos.name);
  late final _phone = TextEditingController(text: widget.pos.phone);
  late final _phoneFocus =
      AutoClearFocus(_phone, onChanged: () => setState(() {}));
  /// الاسم أيضاً يُفرَغ عند الدخول — قرار المالك: أي حقل يُنتقل إليه.
  late final _nameFocus =
      AutoClearFocus(_name, onChanged: () => setState(() {}));
  late bool _active = widget.pos.isActive;

  String? _error;
  bool _saving = false;
  bool _confirming = false;

  @override
  void dispose() {
    _phoneFocus.dispose();
    _nameFocus.dispose();
    _name.dispose();
    _phone.dispose();
    super.dispose();
  }

  bool get _changed =>
      _name.text.trim() != widget.pos.name ||
      Fmt.phoneForApi(_phone.text) != Fmt.phoneForApi(widget.pos.phone) ||
      _active != widget.pos.isActive;

  Future<void> _save() async {
    final name = _name.text.trim();
    final phone = Fmt.phoneForApi(_phone.text);

    if (name.length < 3) {
      setState(() => _error = 'أدخل اسم نقطة البيع.');
      return;
    }
    if (!Fmt.isValidLibyanPhone(phone)) {
      setState(() => _error = 'أدخل رقماً ليبياً من 9 أرقام يبدأ بـ 9.');
      return;
    }

    setState(() {
      _saving = true;
      _error = null;
    });

    try {
      await ref.read(posRepositoryProvider).update(
            id: widget.pos.id,
            name: name,
            phone: phone,
            isActive: _active,
          );
      if (mounted) Navigator.of(context).pop(true);
    } on ApiFailure catch (e) {
      if (mounted) {
        setState(() {
          _error = e.message;
          _saving = false;
          _confirming = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) => Container(
        padding: EdgeInsets.fromLTRB(
            22, 22, 22, 26 + MediaQuery.viewInsetsOf(context).bottom),
        decoration: BoxDecoration(
          color: R.whiteA(.94),
          borderRadius: const BorderRadius.vertical(top: Radius.circular(R.rNav)),
        ),
        child: Column(
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
            Text('تعديل نقطة البيع', style: T.kufi(17, FontWeight.w600)),
            const SizedBox(height: 8),
            Text('أُنشئت ${widget.pos.createdAt.split(" ").first}',
                style: T.label),
            const SizedBox(height: 20),
            _Field(
              label: 'اسم نقطة البيع',
              controller: _name,
              focusNode: _nameFocus,
              hint: 'مثال: نقطة بيع السوق القديم',
              onChanged: (_) => setState(() {}),
            ),
            const SizedBox(height: R.gapCard),
            _Field(
              label: 'رقم الهاتف',
              controller: _phone,
              focusNode: _phoneFocus,
              hint: '9XXXXXXXX',
              ltr: true,
              digitsOnly: true,
              maxLength: 9,
              onChanged: (_) => setState(() {}),
            ),
            const SizedBox(height: R.gapCard),
            _ActiveToggle(
              value: _active,
              onChanged: (v) => setState(() => _active = v),
            ),
            const SizedBox(height: 16),
            const WarnBanner(
              text:
                  'الحفظ يُلغي تسجيل نقطة البيع مهما كان التغيير — حتى الإيقاف '
                  'وحده. ستحتاج إلى إعادة التسجيل من جهازها برمز تحقّق جديد.',
            ),
            if (_error != null) ...[
              const SizedBox(height: 12),
              Text(_error!,
                  style:
                      T.plex(12, FontWeight.w500, color: R.error, height: 1.5)),
            ],
            const SizedBox(height: 20),
            if (!_confirming)
              PrimaryButton(
                label: 'حفظ التعديل',
                onPressed:
                    _changed ? () => setState(() => _confirming = true) : null,
              )
            else ...[
              Text('هل تؤكّد؟ ستخرج نقطة البيع فوراً.',
                  textAlign: TextAlign.center,
                  style: T.plex(12.5, FontWeight.w600, color: R.errorText)),
              const SizedBox(height: 12),
              PrimaryButton(
                label: 'نعم، احفظ وألغِ التسجيل',
                loading: _saving,
                onPressed: _saving ? null : _save,
              ),
            ],
            const SizedBox(height: 10),
            TextButton(
              onPressed: _saving
                  ? null
                  : () => _confirming
                      ? setState(() => _confirming = false)
                      : Navigator.of(context).pop(false),
              style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
              child: Text(_confirming ? 'تراجع' : 'إلغاء',
                  style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
            ),
          ],
        ),
      );
}

class _ActiveToggle extends StatelessWidget {
  const _ActiveToggle({required this.value, required this.onChanged});

  final bool value;
  final ValueChanged<bool> onChanged;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 6),
        decoration: BoxDecoration(
          color: R.inkA(.04),
          borderRadius: BorderRadius.circular(R.rRow),
        ),
        child: Row(
          children: [
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text('الحالة', style: T.label),
                  const SizedBox(height: 4),
                  Text(value ? 'نشطة' : 'موقوفة',
                      style: T.plex(13.5, FontWeight.w600)),
                ],
              ),
            ),
            Switch(
              value: value,
              onChanged: onChanged,
              activeThumbColor: Colors.white,
              activeTrackColor: R.primary,
            ),
          ],
        ),
      );
}

class _AddPosSheet extends ConsumerStatefulWidget {
  const _AddPosSheet();

  @override
  ConsumerState<_AddPosSheet> createState() => _AddPosSheetState();
}

class _AddPosSheetState extends ConsumerState<_AddPosSheet> {
  final _name = TextEditingController();
  final _phone = TextEditingController();
  late final _phoneFocus =
      AutoClearFocus(_phone, onChanged: () => setState(() {}));
  late final _nameFocus =
      AutoClearFocus(_name, onChanged: () => setState(() {}));
  String? _error;
  bool _saving = false;

  @override
  void dispose() {
    _phoneFocus.dispose();
    _nameFocus.dispose();
    _name.dispose();
    _phone.dispose();
    super.dispose();
  }

  Future<void> _save() async {
    final name = _name.text.trim();
    final phone = Fmt.phoneForApi(_phone.text);

    if (name.length < 3) {
      setState(() => _error = 'أدخل اسم نقطة البيع.');
      return;
    }
    if (!Fmt.isValidLibyanPhone(phone)) {
      setState(() => _error = 'أدخل رقماً ليبياً من 9 أرقام يبدأ بـ 9.');
      return;
    }

    setState(() {
      _saving = true;
      _error = null;
    });

    try {
      await ref.read(posRepositoryProvider).add(name: name, phone: phone);
      if (mounted) Navigator.of(context).pop(true);
    } on ApiFailure catch (e) {
      if (mounted) {
        setState(() {
          _error = e.message;
          _saving = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) => Container(
        padding: EdgeInsets.fromLTRB(
            22, 22, 22, 26 + MediaQuery.viewInsetsOf(context).bottom),
        decoration: BoxDecoration(
          color: R.whiteA(.94),
          borderRadius: const BorderRadius.vertical(top: Radius.circular(R.rNav)),
        ),
        child: Column(
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
            Text('نقطة بيع جديدة', style: T.kufi(17, FontWeight.w600)),
            const SizedBox(height: 8),
            Text('تُنشأ غير مسجّلة، وتُكمل تسجيلها بنفسها من التطبيق.',
                style: T.label),
            const SizedBox(height: 20),
            _Field(
              label: 'اسم نقطة البيع',
              controller: _name,
              focusNode: _nameFocus,
              hint: 'مثال: نقطة بيع السوق القديم',
            ),
            const SizedBox(height: R.gapCard),
            _Field(
              label: 'رقم الهاتف',
              controller: _phone,
              focusNode: _phoneFocus,
              hint: '9XXXXXXXX',
              ltr: true,
              digitsOnly: true,
              maxLength: 9,
            ),
            if (_error != null) ...[
              const SizedBox(height: 12),
              Text(_error!,
                  style: T.plex(12, FontWeight.w500, color: R.error, height: 1.5)),
            ],
            const SizedBox(height: 20),
            PrimaryButton(
              label: 'إضافة',
              loading: _saving,
              onPressed: _saving ? null : _save,
            ),
            const SizedBox(height: 10),
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
              child: Text('إلغاء',
                  style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
            ),
          ],
        ),
      );
}

class _Field extends StatelessWidget {
  const _Field({
    required this.label,
    required this.controller,
    required this.hint,
    this.focusNode,
    this.ltr = false,
    this.digitsOnly = false,
    this.maxLength,
    this.onChanged,
  });

  final String label;
  final TextEditingController controller;
  final String hint;

  /// AutoClearFocus كي يُفرَغ الحقل عند دخول المؤشّر.
  final FocusNode? focusNode;
  final bool ltr;
  final bool digitsOnly;
  final int? maxLength;
  final ValueChanged<String>? onChanged;

  @override
  Widget build(BuildContext context) {
    final field = TextField(
      controller: controller,
      focusNode: focusNode,
      onChanged: onChanged,
      keyboardType: digitsOnly ? TextInputType.number : TextInputType.text,
      // رقمي ⇦ أرقام فقط · نصّي ⇦ حروف ومسافات فقط. قرار المالك.
      inputFormatters: [
        if (digitsOnly) ...[
          const WesternDigits(),
          FilteringTextInputFormatter.digitsOnly,
        ] else
          ...lettersOnlyFormatters,
        if (maxLength != null) LengthLimitingTextInputFormatter(maxLength),
      ],
      style: ltr ? T.kufi(16, FontWeight.w600) : T.value,
      decoration: InputDecoration(
        isDense: true,
        border: InputBorder.none,
        counterText: '',
        hintText: hint,
        hintStyle: T.plex(13.5, FontWeight.w400, color: R.inkA(.42)),
      ),
    );

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(label, style: T.label),
          const SizedBox(height: 9),
          ltr
              ? Directionality(textDirection: TextDirection.ltr, child: field)
              : field,
        ],
      ),
    );
  }
}

class _Empty extends StatelessWidget {
  const _Empty();

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.symmetric(vertical: 50),
        child: Column(
          children: [
            RhallaLogo(size: 56, color: R.primaryA(.3)),
            const SizedBox(height: 18),
            Text('لا توجد نقاط بيع بعد',
                style: T.kufi(15, FontWeight.w600, height: 1.5)),
            const SizedBox(height: 10),
            Text('أضف مخوّلاً ليعمل نيابة عنك من فرعك.',
                textAlign: TextAlign.center,
                style: T.plex(12.5, FontWeight.w400,
                    color: R.inkA(.55), height: 1.7)),
          ],
        ),
      );
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(R.padScreen, 0, R.padScreen, 90),
          child: GlassCard(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(message,
                    style: T.plex(12.5, FontWeight.w500,
                        color: R.errorText, height: 1.6)),
                const SizedBox(height: 12),
                TextButton(
                  onPressed: onRetry,
                  style: TextButton.styleFrom(minimumSize: const Size(44, 44)),
                  child: Text('إعادة المحاولة',
                      style: T.plex(12.5, FontWeight.w600, color: R.primaryGradEnd)),
                ),
              ],
            ),
          ),
        ),
      );
}
