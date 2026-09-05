import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';
import 'package:share_plus/share_plus.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../chat/chat_repository.dart';
import '../chat/chat_screen.dart';
import 'employees_repository.dart';

/// «الموظفون» — إدارة من يعمل تحت الوكيل وما يُسمح له.
///
/// ⚠ لا شيء في هذه الشاشة يمسّ المال: لا رصيد، ولا حوالة، ولا قيد. إدارةُ
/// أشخاصٍ وصلاحيات فقط — وهذا هو الخطّ الأحمر الذي وضعه المالك.
///
/// والشاشة للحساب الرئيسي وحده، والخادم هو من يمنع (403) لا إخفاء الزرّ.
class EmployeesScreen extends ConsumerWidget {
  const EmployeesScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(employeesProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'الموظفون',
            onBack: () => context.pop(),
            trailing: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                IconButton(
                  tooltip: 'المتابعة والتقارير',
                  onPressed: () => context.push('/employees/reports'),
                  icon: Icon(Icons.insert_chart_outlined_rounded,
                      size: 22, color: R.primaryDark),
                  constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
                ),
                IconButton(
                  tooltip: 'الأجهزة المفعّلة',
                  onPressed: () => context.push('/employees/devices'),
                  icon: Icon(Icons.devices_outlined, size: 22, color: R.primaryDark),
                  constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
                ),
              ],
            ),
          ),
          Expanded(
            child: async.when(
              loading: () => const _Skeleton(),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(employeesProvider),
              ),
              data: (rows) => RefreshIndicator(
                onRefresh: () async => ref.refresh(employeesProvider.future),
                color: R.primary,
                backgroundColor: Colors.white,
                child: ListView(
                  padding: const EdgeInsets.fromLTRB(
                      R.padScreen, 14, R.padScreen, 120),
                  physics: const AlwaysScrollableScrollPhysics(),
                  children: [
                    _AddButton(onTap: () => _openAdd(context, ref)),
                    const SizedBox(height: 14),
                    if (rows.isEmpty)
                      const _Empty()
                    else
                      for (var i = 0; i < rows.length; i++) ...[
                        if (i > 0) const SizedBox(height: R.gapRow),
                        _EmployeeCard(e: rows[i]),
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
    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (_) => const _AddEmployeeSheet(),
    );
    if (ok == true) ref.invalidate(employeesProvider);
  }
}

class _AddButton extends StatelessWidget {
  const _AddButton({required this.onTap});

  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => GlassCard(
        onTap: onTap,
        child: Row(
          children: [
            IconTile(
              size: 38,
              background: R.primaryA(.12),
              icon: Icon(Icons.person_add_alt_1_rounded,
                  size: 19, color: R.primaryDark),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: Text('إضافة موظف', style: T.kufi(14.5, FontWeight.w700)),
            ),
            Icon(Icons.chevron_left_rounded, size: 22, color: R.inkA(.4)),
          ],
        ),
      );
}

class _EmployeeCard extends ConsumerStatefulWidget {
  const _EmployeeCard({required this.e});

  final Employee e;

  @override
  ConsumerState<_EmployeeCard> createState() => _EmployeeCardState();
}

class _EmployeeCardState extends ConsumerState<_EmployeeCard> {
  bool _busy = false;

  @override
  Widget build(BuildContext context) {
    final e = widget.e;
    final tone = _tone(e.status);

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            children: [
              IconTile(
                size: 38,
                background: tone.withValues(alpha: .12),
                icon: Icon(Icons.badge_outlined, size: 19, color: tone),
              ),
              const SizedBox(width: 12),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(e.fullName,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: T.kufi(14.5, FontWeight.w700)),
                    const SizedBox(height: 3),
                    Directionality(
                      textDirection: TextDirection.ltr,
                      child: Text(Fmt.phone(e.phone),
                          style: T.plex(12, FontWeight.w500,
                              color: R.inkA(.55))),
                    ),
                  ],
                ),
              ),
              _StatusPill(label: e.status.label, color: tone),
            ],
          ),
          const SizedBox(height: 10),
          Divider(color: R.inkA(.07), height: 1),
          const SizedBox(height: 10),

          _Line(icon: Icons.storefront_outlined, text: e.posLabel),
          const SizedBox(height: 6),
          _Line(
            icon: e.hasDevice
                ? Icons.phone_android_rounded
                : Icons.phonelink_erase_rounded,
            text: e.hasDevice
                ? 'جهاز مفعّل${e.deviceModel.isEmpty ? '' : ' · ${e.deviceModel}'}'
                : 'لا جهاز مربوط',
          ),
          const SizedBox(height: 6),
          _Line(
            icon: Icons.vpn_key_outlined,
            text: e.permissions.isEmpty
                ? 'بلا صلاحيات — لا يرى شيئاً'
                : '${e.permissions.length} صلاحية ممنوحة',
          ),
          if (e.lastActivityAt.isNotEmpty) ...[
            const SizedBox(height: 6),
            _Line(
              icon: Icons.schedule_rounded,
              text: 'آخر نشاط · ${Fmt.stampShort(e.lastActivityAt)}',
            ),
          ],

          const SizedBox(height: 12),
          Row(
            children: [
              Expanded(
                child: _Action(
                  label: 'كود تفعيل',
                  icon: Icons.qr_code_2_rounded,
                  filled: e.needsCode,
                  onTap: _busy ? null : _issueCode,
                ),
              ),
              const SizedBox(width: 8),
              Expanded(
                child: _Action(
                  label: 'الصلاحيات',
                  icon: Icons.tune_rounded,
                  onTap: _busy
                      ? null
                      : () => context.push('/employees/${e.id}/permissions',
                          extra: e),
                ),
              ),
              const SizedBox(width: 8),
              Expanded(
                child: _Action(
                  label: e.status == EmployeeStatus.suspended ? 'تفعيل' : 'إيقاف',
                  icon: e.status == EmployeeStatus.suspended
                      ? Icons.play_arrow_rounded
                      : Icons.pause_rounded,
                  danger: e.status != EmployeeStatus.suspended,
                  onTap: _busy ? null : _toggleSuspend,
                ),
              ),
            ],
          ),

          // مراسلة الموظّف — من هنا تبدأ المحادثة أوّل مرّة.
          //
          // شاشة الدردشة تعرض المحادثات **القائمة** وحدها، فبلا هذا الزرّ
          // لا سبيل إلى مراسلة موظّفٍ لم يبدأ هو. والزرّ يُنشئ المحادثة عند
          // الضغط لا قبله: محادثةٌ فارغة لكل موظّف تملأ القائمة بما لم يبدأ.
          const SizedBox(height: 8),
          _Action(
            label: 'مراسلة',
            icon: Icons.chat_bubble_outline_rounded,
            onTap: _busy ? null : _openChat,
          ),
        ],
      ),
    );
  }

  Color _tone(EmployeeStatus s) => switch (s) {
        EmployeeStatus.active => R.primaryDark,
        EmployeeStatus.compromised => R.error,
        EmployeeStatus.disabled => R.error,
        EmployeeStatus.suspended => R.warnIcon,
        _ => R.inkA(.55),
      };

  Future<void> _issueCode() async {
    setState(() => _busy = true);
    try {
      final code =
          await ref.read(employeesRepositoryProvider).issueCode(widget.e.id);
      if (!mounted) return;
      await showModalBottomSheet<void>(
        context: context,
        useRootNavigator: true,
        backgroundColor: Colors.transparent,
        builder: (_) => _CodeSheet(code: code, name: widget.e.fullName),
      );
      if (mounted) ref.invalidate(employeesProvider);
    } on ApiFailure catch (e) {
      _say(e.message);
    } catch (_) {
      _say('تعذّر إصدار الكود — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  /// يفتح محادثة الوكيل مع هذا الموظّف — ينشئها إن لم تكن.
  Future<void> _openChat() async {
    setState(() => _busy = true);
    try {
      final id = await ref
          .read(chatRepositoryProvider)
          .openEmployee(widget.e.id);
      if (!mounted) return;
      setState(() => _busy = false);
      if (id == null) return;

      await Navigator.of(context, rootNavigator: true).push(
        MaterialPageRoute(
          builder: (_) => ChatScreen(
            title: widget.e.fullName,
            threadId: id,
          ),
        ),
      );
      // قائمة المحادثات تحمل عدّادات — والعودة من محادثةٍ قُرئت تُبطلها.
      if (mounted) ref.invalidate(chatThreadsProvider);
    } catch (e) {
      if (!mounted) return;
      setState(() => _busy = false);
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('تعذّر فتح المحادثة. $e',
            style: T.kufi(13, FontWeight.w600))),
      );
    }
  }

  Future<void> _toggleSuspend() async {
    final suspend = widget.e.status != EmployeeStatus.suspended;

    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _ConfirmSheet(
        title: suspend ? 'إيقاف الموظف' : 'إعادة تفعيل الموظف',
        body: suspend
            ? 'ستُغلق جلساته فوراً ولن يستطيع تنفيذ أي عملية حتى تعيد تفعيله.'
            : 'سيعود الموظف إلى العمل بصلاحياته الحالية.',
        confirm: suspend ? 'إيقاف' : 'إعادة تفعيل',
        danger: suspend,
      ),
    );
    if (ok != true || !mounted) return;

    setState(() => _busy = true);
    try {
      await ref.read(employeesRepositoryProvider).setStatus(
            id: widget.e.id,
            status: suspend ? 'SUSPENDED' : 'ACTIVE',
          );
      if (mounted) ref.invalidate(employeesProvider);
    } on ApiFailure catch (e) {
      _say(e.message);
    } catch (_) {
      _say('تعذّر تنفيذ العملية — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busy = false);
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

/// عرض كود التفعيل — **مرّة واحدة**.
///
/// الخادم يحفظه مُجزّأً ولا يُعيده أبداً؛ فإن أُغلقت هذه الورقة قبل أن يُنسخ
/// الكود فلا سبيل إلا إصدار كودٍ جديد. ولذلك تقول ذلك صراحةً.
class _CodeSheet extends StatelessWidget {
  const _CodeSheet({required this.code, required this.name});

  final String code;
  final String name;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(22, 22, 22, 26),
        decoration: BoxDecoration(
          color: R.whiteA(.96),
          borderRadius:
              const BorderRadius.vertical(top: Radius.circular(R.rNav)),
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
            // العنوان والاسم في سطرين لا سطر واحد.
            //
            // اسم الموظف قد يكون ثلاثياً أو رباعياً، فيدفع «كود تفعيل» في سطر
            // واحد إلى الاقتطاع أو إلى لفٍّ يقطع العبارة في منتصفها. وفصلُهما
            // يجعل العنوان ثابتاً مهما طال الاسم، ويترك للاسم سطرين كاملين.
            Column(
              children: [
                Text('كود تفعيل الموظف',
                    textAlign: TextAlign.center,
                    style: T.kufi(16, FontWeight.w700)),
                const SizedBox(height: 4),
                Text(
                  name,
                  textAlign: TextAlign.center,
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                  style: T.kufi(14, FontWeight.w600, color: R.inkA(.62)),
                ),
              ],
            ),
            const SizedBox(height: 16),
            Container(
              padding: const EdgeInsets.symmetric(vertical: 18),
              decoration: BoxDecoration(
                color: R.primaryA(.08),
                border: Border.all(color: R.primaryA(.28)),
                borderRadius: BorderRadius.circular(R.rCard),
              ),
              child: Directionality(
                textDirection: TextDirection.ltr,
                child: Text(
                  code,
                  textAlign: TextAlign.center,
                  style: T.kufi(26, FontWeight.w800,
                      color: R.primaryDark, spacing: 6),
                ),
              ),
            ),
            const SizedBox(height: 14),
            const WarnBanner(
              text: 'انسخ الكود الآن — لن يظهر مرة أخرى. وإن ضاع فأصدر كوداً جديداً.',
            ),
            const SizedBox(height: 18),
            // النسخ والمشاركة فعلان نظيران، فهما في سطر واحد.
            //
            // والمشاركة ليست ترفاً: الكود يُسلَّم للموظف عبر واتساب غالباً،
            // والنسخُ يعني الخروج من التطبيق وفتح المحادثة ولصقَه — بينما
            // ورقة المشاركة تفتح البرامج المثبَّتة مباشرةً. والنسخ يبقى
            // كما هو لمن لا يريد المشاركة، ولمن لا يجد برنامجاً مناسباً.
            //
            // النسخ يبقى الأساسيّ (يمين السطر في واجهة عربية) لأن التحذير
            // فوقه يقول «انسخ الكود الآن» — وزرّان أخضران متجاوران يتنافسان.
            Row(
              children: [
                Expanded(
                  child: PrimaryButton(
                    label: 'نسخ الكود',
                    icon: const Icon(Icons.copy_rounded,
                        size: 18, color: Colors.white),
                    onPressed: () {
                      Clipboard.setData(ClipboardData(text: code));
                      ScaffoldMessenger.of(context)
                        ..hideCurrentSnackBar()
                        ..showSnackBar(SnackBar(
                          content: Text('نُسخ الكود',
                              style: T.plex(13, FontWeight.w500,
                                  color: Colors.white)),
                          backgroundColor: R.inkA(.92),
                          behavior: SnackBarBehavior.floating,
                        ));
                    },
                  ),
                ),
                const SizedBox(width: 10),
                Expanded(
                  child: SecondaryButton(
                    label: 'مشاركة',
                    icon: Icon(Icons.share_rounded,
                        size: 18, color: R.primaryDark),
                    onPressed: () => SharePlus.instance.share(
                      // الاسم مع الكود: الوكيل قد يُصدر أكواداً لعدّة موظفين
                      // في جلسة واحدة، ورسالةٌ بكودٍ مجرّد لا يُعرف صاحبها.
                      ShareParams(
                        text: 'كود تفعيل $name في تطبيق الموظف: $code',
                      ),
                    ),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 10),
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
              child: Text('تمّ',
                  style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
            ),
          ],
        ),
      );
}

/// إضافة موظف — الاسم والهاتف ونقاط البيع.
///
/// الصلاحيات لا تُمنح هنا عمداً: الموظف يُنشأ **بلا صلاحية واحدة**
/// (Default Deny)، ثم يمنحه الوكيل ما يريد من شاشة الصلاحيات. منحُها في
/// شاشة الإنشاء يُغري بمنح الكل بضغطة.
class _AddEmployeeSheet extends ConsumerStatefulWidget {
  const _AddEmployeeSheet();

  @override
  ConsumerState<_AddEmployeeSheet> createState() => _AddEmployeeSheetState();
}

class _AddEmployeeSheetState extends ConsumerState<_AddEmployeeSheet> {
  final _name = TextEditingController();
  final _phone = TextEditingController();
  late final _nameFocus = AutoClearFocus(_name);
  late final _phoneFocus = AutoClearFocus(_phone);

  final Set<int> _pos = {};
  bool _busy = false;
  String? _error;

  @override
  void dispose() {
    _nameFocus.dispose();
    _phoneFocus.dispose();
    _name.dispose();
    _phone.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final posAsync = ref.watch(employeePosProvider);

    return Padding(
      padding: EdgeInsets.only(bottom: MediaQuery.viewInsetsOf(context).bottom),
      child: Container(
        padding: const EdgeInsets.fromLTRB(20, 14, 20, 24),
        constraints: BoxConstraints(
          maxHeight: MediaQuery.sizeOf(context).height * .85,
        ),
        decoration: BoxDecoration(
          color: R.whiteA(.96),
          borderRadius:
              const BorderRadius.vertical(top: Radius.circular(R.rNav)),
        ),
        child: SingleChildScrollView(
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
              const SizedBox(height: 16),
              Center(child: Text('إضافة موظف', style: T.kufi(17, FontWeight.w700))),
              const SizedBox(height: 16),

              _TextField(
                label: 'اسم الموظف',
                controller: _name,
                focusNode: _nameFocus,
                hint: 'الاسم الكامل',
              ),
              const SizedBox(height: R.gapCard),
              _TextField(
                label: 'رقم الهاتف',
                controller: _phone,
                focusNode: _phoneFocus,
                hint: '9XXXXXXXX',
                digits: true,
              ),
              const SizedBox(height: R.gapCard),

              Text('نقاط البيع', style: T.label),
              const SizedBox(height: 8),
              posAsync.when(
                loading: () => Padding(
                  padding: const EdgeInsets.symmetric(vertical: 14),
                  child: Center(
                    child: SizedBox(
                      width: 22,
                      height: 22,
                      child: CircularProgressIndicator(
                          strokeWidth: 2, color: R.primary),
                    ),
                  ),
                ),
                error: (_, _) => Text('تعذّر جلب نقاط البيع.',
                    style: T.plex(12.5, FontWeight.w400, color: R.errorText)),
                data: (list) => list.isEmpty
                    ? Text('لا توجد نقاط بيع في فرعك.',
                        style:
                            T.plex(12.5, FontWeight.w400, color: R.inkA(.55)))
                    : Wrap(
                        spacing: 8,
                        runSpacing: 8,
                        children: [
                          for (final p in list)
                            _Chip(
                              label: p.name.isEmpty ? 'نقطة ${p.id}' : p.name,
                              on: _pos.contains(p.id),
                              onTap: () => setState(() {
                                _pos.contains(p.id)
                                    ? _pos.remove(p.id)
                                    : _pos.add(p.id);
                              }),
                            ),
                        ],
                      ),
              ),

              const SizedBox(height: 14),
              const WarnBanner(
                text: 'يُنشأ الموظف بلا أي صلاحية. امنحه ما يحتاجه من شاشة '
                    'الصلاحيات، ثم أصدر له كود تفعيل.',
              ),

              if (_error != null) ...[
                const SizedBox(height: 12),
                Text(_error!,
                    textAlign: TextAlign.center,
                    style: T.plex(12.5, FontWeight.w500, color: R.errorText)),
              ],

              const SizedBox(height: 18),
              PrimaryButton(
                label: 'حفظ',
                loading: _busy,
                onPressed: _busy ? null : _save,
              ),
              const SizedBox(height: 8),
              TextButton(
                onPressed: () => Navigator.of(context).pop(false),
                style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
                child: Text('إلغاء',
                    style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Future<void> _save() async {
    final name = _name.text.trim();
    final phone = Fmt.phoneForApi(_phone.text);

    if (name.isEmpty) {
      setState(() => _error = 'اكتب اسم الموظف.');
      return;
    }
    if (!RegExp(r'^9\d{8}$').hasMatch(phone)) {
      setState(() => _error = 'رقم الهاتف يجب أن يكون 9 أرقام يبدأ بـ 9.');
      return;
    }

    setState(() { _busy = true; _error = null; });
    try {
      await ref.read(employeesRepositoryProvider).create(
            fullName: name,
            phone: phone,
            pointsOfSale: _pos.toList(),
          );
      if (mounted) Navigator.of(context).pop(true);
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

/* ───────────────────────── عناصر مشتركة ───────────────────────── */

class _Line extends StatelessWidget {
  const _Line({required this.icon, required this.text});

  final IconData icon;
  final String text;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          Icon(icon, size: 14, color: R.inkA(.45)),
          const SizedBox(width: 7),
          Expanded(
            child: Text(text,
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
                style: T.plex(12, FontWeight.w400, color: R.inkA(.6))),
          ),
        ],
      );
}

class _StatusPill extends StatelessWidget {
  const _StatusPill({required this.label, required this.color});

  final String label;
  final Color color;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
        decoration: BoxDecoration(
          color: color.withValues(alpha: .10),
          border: Border.all(color: color.withValues(alpha: .28)),
          borderRadius: BorderRadius.circular(99),
        ),
        child: Text(label,
            style: T.plex(10.5, FontWeight.w600, color: color)),
      );
}

class _Action extends StatelessWidget {
  const _Action({
    required this.label,
    required this.icon,
    this.onTap,
    this.filled = false,
    this.danger = false,
  });

  final String label;
  final IconData icon;
  final VoidCallback? onTap;
  final bool filled;
  final bool danger;

  @override
  Widget build(BuildContext context) {
    final base = danger ? R.error : R.primaryDark;
    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(R.rTile),
      child: Opacity(
        opacity: onTap == null ? .45 : 1,
        child: Container(
          padding: const EdgeInsets.symmetric(vertical: 10),
          decoration: BoxDecoration(
            color: filled ? base : base.withValues(alpha: .08),
            border: Border.all(color: base.withValues(alpha: filled ? 1 : .20)),
            borderRadius: BorderRadius.circular(R.rTile),
          ),
          child: Column(
            children: [
              Icon(icon, size: 16, color: filled ? Colors.white : base),
              const SizedBox(height: 4),
              Text(label,
                  maxLines: 1,
                  style: T.plex(10.5, FontWeight.w600,
                      color: filled ? Colors.white : base)),
            ],
          ),
        ),
      ),
    );
  }
}

class _Chip extends StatelessWidget {
  const _Chip({required this.label, required this.on, required this.onTap});

  final String label;
  final bool on;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => GestureDetector(
        onTap: onTap,
        child: AnimatedContainer(
          duration: const Duration(milliseconds: 160),
          padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 9),
          decoration: BoxDecoration(
            color: on ? R.primaryA(.12) : R.whiteA(.7),
            border: Border.all(
              color: on ? R.primary : R.inkA(.10),
              width: on ? 1.6 : 1,
            ),
            borderRadius: BorderRadius.circular(99),
          ),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              if (on) ...[
                Icon(Icons.check_rounded, size: 14, color: R.primaryDark),
                const SizedBox(width: 5),
              ],
              Text(label,
                  style: T.plex(12, FontWeight.w600,
                      color: on ? R.primaryDark : R.inkA(.6))),
            ],
          ),
        ),
      );
}

class _TextField extends StatelessWidget {
  const _TextField({
    required this.label,
    required this.controller,
    required this.focusNode,
    required this.hint,
    this.digits = false,
  });

  final String label;
  final TextEditingController controller;
  final FocusNode focusNode;
  final String hint;
  final bool digits;

  @override
  Widget build(BuildContext context) {
    final field = TextField(
      controller: controller,
      focusNode: focusNode,
      keyboardType: digits ? TextInputType.number : TextInputType.text,
      // WesternDigits أولاً دائماً — الفلتر بعده يسمح بـ [0-9] فقط، فلو
      // جاء بعده لحذف الرقم العربي قبل أن يُحوَّل.
      inputFormatters: digits
          ? [
              const WesternDigits(),
              FilteringTextInputFormatter.digitsOnly,
              LengthLimitingTextInputFormatter(9),
            ]
          : [
              const WesternDigits(),
              ...lettersOnlyFormatters,
              LengthLimitingTextInputFormatter(200),
            ],
      style: digits ? T.kufi(16, FontWeight.w600) : T.value,
      decoration: InputDecoration(
        isDense: true,
        border: InputBorder.none,
        counterText: '',
        hintText: hint,
        hintStyle: T.plex(13, FontWeight.w400, color: R.inkA(.42)),
      ),
    );

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(label, style: T.label),
          const SizedBox(height: 9),
          digits
              ? Directionality(textDirection: TextDirection.ltr, child: field)
              : field,
        ],
      ),
    );
  }
}

class _ConfirmSheet extends StatelessWidget {
  const _ConfirmSheet({
    required this.title,
    required this.body,
    required this.confirm,
    this.danger = false,
  });

  final String title;
  final String body;
  final String confirm;
  final bool danger;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(22, 22, 22, 26),
        decoration: BoxDecoration(
          color: R.whiteA(.94),
          borderRadius:
              const BorderRadius.vertical(top: Radius.circular(R.rNav)),
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
            Center(
              child: Text(title,
                  style: T.kufi(17, FontWeight.w700,
                      color: danger ? R.error : R.ink)),
            ),
            const SizedBox(height: 10),
            Text(body,
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w500,
                    color: R.inkA(.65), height: 1.7)),
            const SizedBox(height: 20),
            PrimaryButton(
              label: confirm,
              onPressed: () => Navigator.of(context).pop(true),
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

class _Empty extends StatelessWidget {
  const _Empty();

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.symmetric(vertical: 46),
        child: Column(
          children: [
            Icon(Icons.badge_outlined, size: 42, color: R.primaryA(.3)),
            const SizedBox(height: 16),
            Text('لا يوجد موظفون بعد',
                style: T.kufi(15, FontWeight.w600, color: R.inkA(.6))),
            const SizedBox(height: 8),
            Text('أضف موظفاً، امنحه صلاحياته، ثم أصدر له كود تفعيل.',
                textAlign: TextAlign.center,
                style: T.plex(12.5, FontWeight.w400,
                    color: R.inkA(.45), height: 1.8)),
          ],
        ),
      );
}

class _Skeleton extends StatelessWidget {
  const _Skeleton();

  @override
  Widget build(BuildContext context) => ListView(
        padding: const EdgeInsets.fromLTRB(R.padScreen, 14, R.padScreen, 40),
        children: [
          for (var i = 0; i < 4; i++) ...[
            if (i > 0) const SizedBox(height: R.gapRow),
            GlassCard(
              child: SizedBox(
                height: 84,
                child: Center(
                  child: Container(
                    height: 12,
                    decoration: BoxDecoration(
                      color: R.inkA(.06),
                      borderRadius: BorderRadius.circular(99),
                    ),
                  ),
                ),
              ),
            ),
          ],
        ],
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
