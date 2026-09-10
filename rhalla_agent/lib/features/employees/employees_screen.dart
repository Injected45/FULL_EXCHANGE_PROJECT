import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';
import 'package:qr_flutter/qr_flutter.dart';
import 'package:share_plus/share_plus.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../chat/chat_repository.dart';
import '../chat/chat_screen.dart';
import 'approvals_badge.dart';
import 'employee_limits_sheet.dart';
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
                // ⚠ طلباتُ الموافقة أوّلَ الرأس: هي وحدها التي تنتظر
                // فعلاً من الوكيل، وبقيّةُ الأيقونات تصفّحٌ يحتمل.
                ApprovalsBadgeIcon(
                  onTap: () => context.push('/employees/approvals'),
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
                onRefresh: () => ref.refresh(employeesProvider.future).then((_) {}, onError: (_) {}),
                color: R.primary,
                backgroundColor: Colors.white,
                child: ListView(
                  padding: const EdgeInsets.fromLTRB(
                      R.padScreen, 14, R.padScreen, 120),
                  physics: const AlwaysScrollableScrollPhysics(),
                  children: [
                    if (rows.isNotEmpty) ...[
                      _PauseAllBar(allPaused: rows.first.allPaused),
                      const SizedBox(height: 14),
                    ],
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

/// شريطُ «إيقاف/تشغيل جميع الموظفين» — سيطرةٌ جماعية فورية عن بُعد.
///
/// مستقلٌّ عن الإيقاف الفرديّ: تشغيلُ الكلّ لا يُلغي إيقافَ موظفٍ أوقفتَه
/// وحده. ولا يمسّ مالاً — تجميدُ واجهةٍ فقط، بلا فقدان شيء.
class _PauseAllBar extends ConsumerStatefulWidget {
  const _PauseAllBar({required this.allPaused});

  final bool allPaused;

  @override
  ConsumerState<_PauseAllBar> createState() => _PauseAllBarState();
}

class _PauseAllBarState extends ConsumerState<_PauseAllBar> {
  bool _busy = false;

  Future<void> _toggle() async {
    final pause = !widget.allPaused;
    if (pause) {
      final ok = await showModalBottomSheet<bool>(
        context: context,
        useRootNavigator: true,
        backgroundColor: Colors.transparent,
        builder: (_) => const _ConfirmSheet(
          title: 'إيقاف جميع الموظفين',
          body: 'سيتجمّد كلّ موظفيك فوراً ويرون رسالة «تواصل مع الإدارة». '
              'لا يُفقد شيء، ويعودون فور التشغيل.',
          confirm: 'إيقاف الكل',
          danger: true,
        ),
      );
      if (ok != true || !mounted) return;
    }

    setState(() => _busy = true);
    try {
      await ref.read(employeesRepositoryProvider).setPausedAll(paused: pause);
      if (mounted) ref.invalidate(employeesProvider);
    } on ApiFailure catch (e) {
      _snack(e.message);
    } catch (_) {
      _snack('تعذّر تنفيذ العملية — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  void _snack(String m) {
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

  @override
  Widget build(BuildContext context) {
    final on = widget.allPaused;
    return GlassCard(
      child: Row(
        children: [
          IconTile(
            size: 38,
            background:
                on ? R.error.withValues(alpha: .12) : R.primaryA(.12),
            icon: Icon(
                on
                    ? Icons.pause_circle_filled_rounded
                    : Icons.groups_rounded,
                size: 19,
                color: on ? R.error : R.primaryDark),
          ),
          const SizedBox(width: 12),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(on ? 'كلّ الموظفين موقوفون' : 'إيقاف جميع الموظفين',
                    style: T.kufi(14, FontWeight.w700)),
                const SizedBox(height: 3),
                Text(
                    on
                        ? 'اضغط للتشغيل — يعودون فوراً'
                        : 'تجميدٌ جماعيّ فوريّ، بلا فقدان شيء',
                    style:
                        T.plex(11.5, FontWeight.w400, color: R.inkA(.55))),
              ],
            ),
          ),
          if (_busy)
            const SizedBox(
                width: 24,
                height: 24,
                child: CircularProgressIndicator(strokeWidth: 2.2))
          else
            Switch(value: on, onChanged: (_) => _toggle()),
        ],
      ),
    );
  }
}

class _EmployeeCard extends ConsumerStatefulWidget {
  const _EmployeeCard({required this.e});

  final Employee e;

  @override
  ConsumerState<_EmployeeCard> createState() => _EmployeeCardState();
}

class _EmployeeCardState extends ConsumerState<_EmployeeCard> {
  bool _busy = false;

  /// منطويةٌ افتراضياً — يظهر الاسم فقط، وتنسدل التفاصيل والأزرار بالضغط.
  bool _expanded = false;

  @override
  Widget build(BuildContext context) {
    final e = widget.e;
    final tone = _tone(e.status);

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          // الرأسُ القابل للطيّ: الاسمُ وحدَه، مع مؤشّرِ إيقافٍ إن وُجد.
          InkWell(
            onTap: () => setState(() => _expanded = !_expanded),
            borderRadius: BorderRadius.circular(R.rCard),
            child: Row(
              children: [
                IconTile(
                  size: 38,
                  background: tone.withValues(alpha: .12),
                  icon: Icon(Icons.badge_outlined, size: 19, color: tone),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Text(e.fullName,
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      style: T.kufi(14.5, FontWeight.w700)),
                ),
                if (e.frozen) ...[
                  Container(
                    padding:
                        const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
                    decoration: BoxDecoration(
                      color: R.error.withValues(alpha: .1),
                      borderRadius: BorderRadius.circular(99),
                    ),
                    child: Text('موقوف',
                        style:
                            T.plex(10.5, FontWeight.w700, color: R.error)),
                  ),
                  const SizedBox(width: 8),
                ],
                AnimatedRotation(
                  turns: _expanded ? -0.25 : 0,
                  duration: const Duration(milliseconds: 180),
                  child: Icon(Icons.expand_more_rounded,
                      size: 22, color: R.inkA(.45)),
                ),
              ],
            ),
          ),

          if (_expanded) ...[
            const SizedBox(height: 12),
            Row(
              children: [
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(Fmt.phone(e.phone),
                      style: T.plex(12.5, FontWeight.w500, color: R.inkA(.6))),
                ),
                const Spacer(),
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

            // إيقافٌ مؤقّت (تجميد ناعم) — يختلف عن «الإيقاف» أعلاه: لا يُغلق
            // الجلسة ولا يفقد شيئاً، ويعود فوراً. سيطرةٌ سريعة عن بُعد.
            const SizedBox(height: 8),
            _Action(
              label: e.paused ? 'تشغيل الموظف' : 'إيقاف مؤقّت',
              icon: e.paused
                  ? Icons.play_circle_outline_rounded
                  : Icons.pause_circle_outline_rounded,
              danger: !e.paused,
              onTap: _busy ? null : _togglePause,
            ),

            // مراسلة الموظّف — من هنا تبدأ المحادثة أوّل مرّة.
            //
            // شاشة الدردشة تعرض المحادثات **القائمة** وحدها، فبلا هذا الزرّ
            // لا سبيل إلى مراسلة موظّفٍ لم يبدأ هو. والزرّ يُنشئ المحادثة عند
            // الضغط لا قبله: محادثةٌ فارغة لكل موظّف تملأ القائمة بما لم يبدأ.
            const SizedBox(height: 8),
            Row(
              children: [
                Expanded(
                  child: _Action(
                    label: 'سقف التحويل',
                    icon: Icons.speed_rounded,
                    onTap: _busy ? null : _openLimits,
                  ),
                ),
                const SizedBox(width: 8),
                Expanded(
                  child: _Action(
                    label: 'مراسلة',
                    icon: Icons.chat_bubble_outline_rounded,
                    onTap: _busy ? null : _openChat,
                  ),
                ),
              ],
            ),

            /*
             * ⚠ الحذفُ وحدَه في سطره، وأحمر.
             *
             * وهو الفعلُ الوحيد هنا الذي لا يُتراجَع عنه من التطبيق،
             * فلا يجاور فعلاً عادياً كالمراسلة — ضغطةٌ في غير موضعها
             * تُخرج موظفاً من العمل.
             */
            const SizedBox(height: 8),
            _Action(
              label: 'حذف الموظف',
              icon: Icons.person_remove_outlined,
              danger: true,
              onTap: _busy ? null : _deleteEmployee,
            ),
          ], // نهاية الجزء المنسدل (if (_expanded))
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
      final issued =
          await ref.read(employeesRepositoryProvider).issueCode(widget.e.id);
      if (!mounted) return;
      await showModalBottomSheet<void>(
        context: context,
        useRootNavigator: true,
        backgroundColor: Colors.transparent,
        // ⚠ الورقة تحمل الرمزَ والعدّادَ وزرَّي التجديد والإلغاء، فقد
        // تطول على شاشةٍ قصيرة — و`isScrollControlled` هو ما يسمح لها
        // بتجاوز نصف الشاشة بدل أن تقتطع نفسها.
        isScrollControlled: true,
        builder: (_) => _CodeSheet(
          issued: issued,
          name: widget.e.fullName,
          phone: widget.e.phone,
          employeeId: widget.e.id,
        ),
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

  /// حذفُ الموظف — بتأكيدٍ يقول ما يبقى وما يزول.
  ///
  /// ⚠ ونصُّ التأكيد يذكر **بقاءَ حوالاته في السجلّ**: وكيلٌ يظنّ أن الحذف
  /// يمحو تاريخَ موظفه قد يمتنع عنه خوفاً، أو يفعله ظانّاً أنه يُخفي شيئاً.
  /// وكلاهما سوءُ فهمٍ يُصلحه سطرٌ واحد.
  Future<void> _deleteEmployee() async {
    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _ConfirmSheet(
        title: 'حذف ${widget.e.fullName}',
        body: 'الحذف متاح ما دام الموظف لم ينفّذ أي عملية مالية.\n\n'
            'ستُغلق جلساته وأجهزته وأكواده فوراً، ويتحرّر رقم هاتفه '
            'فتستطيع إضافته من جديد برقم صحيح.\n\n'
            'وإن كان قد أنشأ حوالة أو سجّل حركة خزينة فلن يُحذف — '
            'أوقفه بدلاً من ذلك.',
        confirm: 'حذف',
        danger: true,
      ),
    );
    if (ok != true || !mounted) return;

    setState(() => _busy = true);
    try {
      await ref.read(employeesRepositoryProvider).remove(widget.e.id);
      if (mounted) ref.invalidate(employeesProvider);
    } on ApiFailure catch (e) {
      _say(e.message);
    } catch (_) {
      _say('تعذّر الحذف — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }
  /// ورقةُ سقف التحويل — سياسةُ هذا الموظف وحدَه.
  Future<void> _openLimits() async {
    await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      // الورقةُ فيها حقولُ إدخال، فترتفع فوق لوحة المفاتيح.
      isScrollControlled: true,
      builder: (_) => EmployeeLimitsSheet(
        employeeId: widget.e.id,
        employeeName: widget.e.fullName,
      ),
    );
  }

  Future<void> _togglePause() async {
    final pause = !widget.e.paused;
    setState(() => _busy = true);
    try {
      await ref
          .read(employeesRepositoryProvider)
          .setPaused(id: widget.e.id, paused: pause);
      if (mounted) ref.invalidate(employeesProvider);
    } on ApiFailure catch (e) {
      _say(e.message);
    } catch (_) {
      _say('تعذّر تنفيذ العملية — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busy = false);
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

/// عرض كود التفعيل ورمز QR — **مرّة واحدة**.
///
/// الخادم يحفظ الاثنين مُجزَّأين (Hash / SHA-256)، فلا وجود لهما بعد إغلاق
/// هذه الورقة. والرمزُ يُقدَّم أولاً لأنه أسرعُ وأقلُّ خطأً، والكودُ باقٍ
/// كاملاً تحته لمن لا كاميرا لديه أو لمن يُملي الكود صوتاً على الهاتف.
class _CodeSheet extends ConsumerStatefulWidget {
  const _CodeSheet({
    required this.issued,
    required this.name,
    required this.phone,
    required this.employeeId,
  });

  final ActivationCode issued;
  final String name;

  /// يُعرض تحت الاسم: الوكيل قد يُصدر أكواداً لعدّة موظفين في جلسةٍ واحدة،
  /// وأسماءٌ متشابهة تجعل الرمزَ يُسلَّم إلى غير صاحبه.
  final String phone;

  final int employeeId;

  @override
  ConsumerState<_CodeSheet> createState() => _CodeSheetState();
}

class _CodeSheetState extends ConsumerState<_CodeSheet> {
  late ActivationCode _c = widget.issued;
  Timer? _tick;
  Duration _left = Duration.zero;
  bool _busy = false;
  bool _revoked = false;

  @override
  void initState() {
    super.initState();
    _restart();
  }

  /*
   * ⚠ العدّاد يُحسب من `expires_at` الآتي من الخادم، لا من عدٍّ تنازليّ
   * يبدأ من عشر دقائق.
   *
   * فالعدُّ المحليّ يتوقّف إن نام الجهاز أو خرج التطبيق إلى الخلفية، فيُظهر
   * وقتاً باقياً لرمزٍ مات — والوكيل يُصرّ على أنه صالح. والفرقُ يظهر عند
   * أوّل مكالمة يقول فيها الموظف «الرمز لا يعمل» والشاشةُ أمام الوكيل تقول
   * إن أمامه أربع دقائق.
   */
  void _restart() {
    _tick?.cancel();
    _recompute();
    _tick = Timer.periodic(const Duration(seconds: 1), (_) => _recompute());
  }

  void _recompute() {
    final end = _c.expiresAt;
    final left = end == null ? Duration.zero : end.difference(DateTime.now());
    final next = left.isNegative ? Duration.zero : left;
    if (next != _left && mounted) setState(() => _left = next);
    if (next == Duration.zero) _tick?.cancel();
  }

  @override
  void dispose() {
    _tick?.cancel();
    super.dispose();
  }

  bool get _dead => _revoked || _left == Duration.zero;

  String get _clock {
    final m = _left.inMinutes.toString().padLeft(2, '0');
    final s = (_left.inSeconds % 60).toString().padLeft(2, '0');
    return '$m:$s';
  }

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(22, 22, 22, 26),
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
              const SizedBox(height: 20),
              // العنوان والاسم في سطرين لا سطر واحد.
              //
              // اسم الموظف قد يكون ثلاثياً أو رباعياً، فيدفع «كود تفعيل» في
              // سطر واحد إلى الاقتطاع أو إلى لفٍّ يقطع العبارة في منتصفها.
              // وفصلُهما يجعل العنوان ثابتاً مهما طال الاسم.
              Column(
                children: [
                  Text('تفعيل الموظف',
                      textAlign: TextAlign.center,
                      style: T.kufi(16, FontWeight.w700)),
                  const SizedBox(height: 4),
                  Text(
                    widget.name,
                    textAlign: TextAlign.center,
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                    style: T.kufi(14, FontWeight.w600, color: R.inkA(.62)),
                  ),
                  if (widget.phone.isNotEmpty) ...[
                    const SizedBox(height: 2),
                    // رقمٌ داخل نصٍّ عربيّ ينقلب ترتيبُه — فسطرُه وحدَه وبـLTR.
                    Directionality(
                      textDirection: TextDirection.ltr,
                      child: Text(
                        Fmt.phone(widget.phone),
                        textAlign: TextAlign.center,
                        style: T.plex(12, FontWeight.w500, color: R.inkA(.5)),
                      ),
                    ),
                  ],
                ],
              ),
              if (_c.hasQr) ...[
                const SizedBox(height: 16),
                _qr(),
                const SizedBox(height: 12),
                _countdown(),
                const SizedBox(height: 8),
                Text(
                  _dead
                      ? 'أصدر رمزاً جديداً ليُمسح.'
                      : 'اطلب من الموظف فتح تطبيق الرحالة ⇦ الدخول كموظف ⇦ '
                          'مسح رمز التفعيل.',
                  textAlign: TextAlign.center,
                  style: T.plex(12, FontWeight.w400,
                      color: R.inkA(.55), height: 1.7),
                ),
              ],
              const SizedBox(height: 18),
              /*
               * ⚠ والكودُ يبقى كاملاً تحت الرمز، لا يُستبدل به.
               *
               * فالرمزُ يحتاج كاميرا تعمل وإذناً مُنِح وضوءاً كافياً، وأيُّها
               * تخلّف بقي الكودُ الطريقَ الوحيد. وحذفُه لأن الرمز أسرعُ
               * يُعطّل التفعيل كلَّه على هاتفٍ كاميرتُه معطوبة.
               */
              Text('أو أدخل الكود يدوياً',
                  textAlign: TextAlign.center,
                  style: T.plex(12, FontWeight.w600, color: R.inkA(.5))),
              const SizedBox(height: 8),
              Container(
                padding: const EdgeInsets.symmetric(vertical: 16),
                decoration: BoxDecoration(
                  color: _dead ? R.inkA(.05) : R.primaryA(.08),
                  border:
                      Border.all(color: _dead ? R.inkA(.14) : R.primaryA(.28)),
                  borderRadius: BorderRadius.circular(R.rCard),
                ),
                child: Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(
                    _c.code,
                    textAlign: TextAlign.center,
                    style: T.kufi(24, FontWeight.w800,
                        color: _dead ? R.inkA(.35) : R.primaryDark, spacing: 6),
                  ),
                ),
              ),
              const SizedBox(height: 14),
              /*
               * ⚠ المدّةُ تُقال هنا لا تُترك للمفاجأة.
               *
               * الكودُ يُحرق بعد عشر دقائق إن لم يُستعمل (أمر المالك،
               * 8 سبتمبر 2026: «يُمنع ترك صلاحية المفتاح مفتوحة»). ووكيلٌ
               * يُصدره ثم يُسلّمه غداً يجد موظّفَه يقول «الكود لا يعمل» —
               * فيظنّ التطبيق معطوباً، والسببُ قاعدةٌ لم تُقَل له.
               */
              WarnBanner(
                text: _dead
                    ? 'انتهت صلاحية هذا التفعيل. أصدر رمزاً جديداً.'
                    : 'انسخ الكود الآن — لن يظهر مرة أخرى. '
                        'وصلاحيته عشر دقائق فقط، فإن لم يُستعمل فيها '
                        'فأصدر غيره.',
              ),
              const SizedBox(height: 18),
              // النسخ والمشاركة فعلان نظيران، فهما في سطر واحد.
              //
              // والمشاركة ليست ترفاً: الكود يُسلَّم للموظف عبر واتساب غالباً،
              // والنسخُ يعني الخروج من التطبيق وفتح المحادثة ولصقَه — بينما
              // ورقة المشاركة تفتح البرامج المثبَّتة مباشرةً.
              //
              // ⚠ والمشاركةُ تُرسل الكودَ وحدَه لا الرمز: رمزُ QR صورةٌ تُمسح
              // من الشاشة أمام الموظف، وإرسالُه في محادثة يجعله ملفاً باقياً
              // في هاتفين ومعرض صورٍ وسحابةِ نسخٍ احتياطي.
              Row(
                children: [
                  Expanded(
                    child: PrimaryButton(
                      label: 'نسخ الكود',
                      icon: const Icon(Icons.copy_rounded,
                          size: 18, color: Colors.white),
                      onPressed: _dead
                          ? null
                          : () {
                              Clipboard.setData(ClipboardData(text: _c.code));
                              _say('نُسخ الكود');
                            },
                    ),
                  ),
                  const SizedBox(width: 10),
                  Expanded(
                    child: SecondaryButton(
                      label: 'مشاركة',
                      icon: Icon(Icons.share_rounded,
                          size: 18, color: R.primaryDark),
                      onPressed: _dead
                          ? null
                          : () => SharePlus.instance.share(
                                // الاسم مع الكود: الوكيل قد يُصدر أكواداً
                                // لعدّة موظفين في جلسة واحدة، ورسالةٌ بكودٍ
                                // مجرّد لا يُعرف صاحبها.
                                ShareParams(
                                  text: 'كود تفعيل ${widget.name} في تطبيق '
                                      'الموظف: ${_c.code}',
                                ),
                              ),
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 10),
              Row(
                children: [
                  Expanded(
                    child: SecondaryButton(
                      label: 'إصدار رمز جديد',
                      icon: Icon(Icons.refresh_rounded,
                          size: 18, color: R.primaryDark),
                      onPressed: _busy ? null : _renew,
                    ),
                  ),
                  const SizedBox(width: 10),
                  Expanded(
                    child: SecondaryButton(
                      label: 'إلغاء الرمز',
                      icon:
                          Icon(Icons.block_rounded, size: 18, color: R.error),
                      onPressed: (_busy || _revoked) ? null : _revoke,
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
        ),
      );

  Widget _qr() => Center(
        child: Container(
          padding: const EdgeInsets.all(14), // المنطقة الهادئة حول الرمز
          decoration: BoxDecoration(
            // ⚠ أبيض صريح لا لون الورقة: الماسحات تقرأ التباين، ورمزٌ على
            // خلفيةٍ رماديةٍ فاتحة يُقرأ على شاشةٍ ويُخفق على أخرى.
            color: Colors.white,
            borderRadius: BorderRadius.circular(R.rCard),
            border: Border.all(color: R.inkA(.10)),
          ),
          child: Stack(
            alignment: Alignment.center,
            children: [
              Opacity(
                opacity: _dead ? .12 : 1,
                child: QrImageView(
                  data: _c.qrToken,
                  version: QrVersions.auto,
                  size: 208,
                  padding: EdgeInsets.zero,
                  backgroundColor: Colors.white,
                  // ⚠ أعلى مستوى تصحيحٍ للأخطاء: الرمز يُمسح من شاشةِ هاتفٍ
                  // فيها انعكاسٌ وبصمات، لا من ورقةٍ مطبوعة.
                  errorCorrectionLevel: QrErrorCorrectLevel.H,
                  eyeStyle: QrEyeStyle(
                    eyeShape: QrEyeShape.square,
                    color: R.ink,
                  ),
                  dataModuleStyle: QrDataModuleStyle(
                    dataModuleShape: QrDataModuleShape.square,
                    color: R.ink,
                  ),
                ),
              ),
              if (_dead)
                Text(
                  _revoked ? 'أُلغي' : 'انتهى',
                  style: T.kufi(18, FontWeight.w800, color: R.error),
                ),
            ],
          ),
        ),
      );

  Widget _countdown() {
    final low = _left.inSeconds <= 60;
    final tone = _dead ? R.error : (low ? R.warnIcon : R.primaryDark);
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Icon(Icons.schedule_rounded, size: 15, color: tone),
        const SizedBox(width: 6),
        Text(_dead ? 'انتهت الصلاحية' : 'ينتهي خلال',
            style: T.plex(12, FontWeight.w500, color: tone)),
        if (!_dead) ...[
          const SizedBox(width: 6),
          // الوقت رقمٌ وترتيبُه من اليسار — كسائر أرقام التطبيق.
          Directionality(
            textDirection: TextDirection.ltr,
            child:
                Text(_clock, style: T.kufi(13, FontWeight.w800, color: tone)),
          ),
        ],
      ],
    );
  }

  Future<void> _renew() async {
    setState(() => _busy = true);
    try {
      final next = await ref
          .read(employeesRepositoryProvider)
          .issueCode(widget.employeeId);
      if (!mounted) return;
      setState(() {
        _c = next;
        _revoked = false;
        _busy = false;
      });
      _restart();
      ref.invalidate(employeesProvider);
    } on ApiFailure catch (e) {
      if (mounted) setState(() => _busy = false);
      _say(e.message);
    } catch (_) {
      if (mounted) setState(() => _busy = false);
      _say('تعذّر إصدار رمز جديد — تحقّق من الاتصال.');
    }
  }

  Future<void> _revoke() async {
    setState(() => _busy = true);
    try {
      await ref.read(employeesRepositoryProvider).revokeCode(widget.employeeId);
      if (!mounted) return;
      setState(() {
        _revoked = true;
        _busy = false;
      });
      _tick?.cancel();
      ref.invalidate(employeesProvider);
      _say('أُلغي التفعيل. الرمز والكود لم يعودا يعملان.');
    } on ApiFailure catch (e) {
      if (mounted) setState(() => _busy = false);
      _say(e.message);
    } catch (_) {
      if (mounted) setState(() => _busy = false);
      _say('تعذّر الإلغاء — تحقّق من الاتصال.');
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
