import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'employees_repository.dart';

/// منح صلاحيات موظف — **Default Deny**.
///
/// ما لا يُعلَّم هنا مرفوض. لا صلاحية ضمنية، ولا «منح القسم يمنح ما فيه»:
/// «فتح قسم التقارير» لا يُظهر تقريراً واحداً — لكل تقرير مفتاحه.
///
/// والكتالوج يأتي من الخادم لا من التطبيق: ميزةٌ تُضاف غداً تظهر هنا
/// **مرفوضةً للجميع** بلا تحديث للتطبيق ولا هجرة قاعدة بيانات. ولهذا لا
/// يوجد في هذا الملف اسم صلاحية واحد مكتوب.
class EmployeePermissionsScreen extends ConsumerStatefulWidget {
  const EmployeePermissionsScreen({super.key, required this.employee});

  final Employee employee;

  @override
  ConsumerState<EmployeePermissionsScreen> createState() =>
      _EmployeePermissionsScreenState();
}

class _EmployeePermissionsScreenState
    extends ConsumerState<EmployeePermissionsScreen> {
  late final Set<String> _granted = widget.employee.permissions.toSet();
  bool _busy = false;

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(permissionCatalogProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'صلاحيات الموظف', onBack: () => context.pop()),
          Expanded(
            child: async.when(
              loading: () => Center(
                child: CircularProgressIndicator(color: R.primary),
              ),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(permissionCatalogProvider),
              ),
              data: (groups) => ListView(
                padding: const EdgeInsets.fromLTRB(
                    R.padScreen, 14, R.padScreen, 40),
                children: [
                  GlassCard(
                    child: Row(
                      children: [
                        IconTile(
                          size: 38,
                          background: R.primaryA(.12),
                          icon: Icon(Icons.badge_outlined,
                              size: 19, color: R.primaryDark),
                        ),
                        const SizedBox(width: 12),
                        Expanded(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(widget.employee.fullName,
                                  maxLines: 1,
                                  overflow: TextOverflow.ellipsis,
                                  style: T.kufi(14.5, FontWeight.w700)),
                              const SizedBox(height: 3),
                              Text('${_granted.length} صلاحية ممنوحة',
                                  style: T.plex(12, FontWeight.w500,
                                      color: R.inkA(.55))),
                            ],
                          ),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(height: 12),
                  const WarnBanner(
                    text: 'ما لا تمنحه هنا يبقى ممنوعاً. والميزات التي تُضاف '
                        'مستقبلاً تكون مقفلة تلقائياً حتى تمنحها بنفسك.',
                  ),
                  const SizedBox(height: 14),

                  for (final g in groups) ...[
                    _GroupCard(
                      group: g,
                      granted: _granted,
                      onToggle: _toggle,
                      onAll: (on) => _all(g, on),
                    ),
                    const SizedBox(height: R.gapCard),
                  ],

                  const SizedBox(height: 8),
                  PrimaryButton(
                    label: 'حفظ الصلاحيات',
                    loading: _busy,
                    onPressed: _busy ? null : _save,
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  /*
   * ══════════════════════════════════════════════════════════════════════
   *  ⚠ الصلاحيةُ الحسّاسة لا تُمنح مع مجموعتها
   * ══════════════════════════════════════════════════════════════════════
   *
   * أمرُ المالك (9 سبتمبر 2026): رصيدُ الوكيل لا يظهر للموظف إلّا بصلاحيةٍ
   * يسمح بها الوكيل.
   *
   * ولم تكن المشكلةُ تسرُّباً — فُحص كلُّ مسارٍ للموظف بجلسةٍ مُنحت كلَّ شيءٍ
   * إلّا الماليّ، فردَّ كلُّ مسارِ رصيدٍ 403 ولا نقطةَ أخرى تحمل الرقم.
   * المشكلةُ أنّ **«تحديد الكل»** على مجموعة التقارير كان يمنح «تقرير رصيد
   * الوكيل» معها: الوكيل يضغطها ليُعطي تقاريرَ اليوم والمسلَّمة، فيُسلّم
   * رصيدَه في الضغطة نفسها بلا أن يلاحظ. وكذلك مجموعة الأرصدة.
   *
   * ── ثلاثةُ قراراتٍ فيها ────────────────────────────────────────────────
   *
   * 1. **«تحديد الكل» لا يمنحها، و«إلغاء الكل» يُلغيها.** غيرُ متناظرٍ عمداً:
   *    الخطرُ في المنح لا في السحب، وسحبٌ لا يكتمل يُبقي رصيداً مكشوفاً
   *    والوكيلُ يظنّ أنه أغلق الباب.
   *
   * 2. **التشغيلُ يسأل، والإطفاءُ لا يسأل.** سؤالٌ عند الإغلاق يجعل الوكيل
   *    يضغط «نعم» بلا قراءة، فيتعلّم تجاهلَ السؤال حين يهمّ.
   *
   * 3. **والسؤالُ يسمّي ما سيُكشَف** («سيرى الموظف رصيد وكالتك الكلّي») لا
   *    «هل أنت متأكد؟». والنصُّ من الخادم: لا اسمَ صلاحيةٍ ولا جملةَ تحذيرٍ
   *    مكتوبةٌ في هذا الملف.
   */
  Future<void> _toggle(PermissionItem item) async {
    final on = _granted.contains(item.key);

    if (on) {
      setState(() => _granted.remove(item.key));
      return;
    }

    if (item.sensitive) {
      final ok = await showModalBottomSheet<bool>(
        context: context,
        useRootNavigator: true,
        backgroundColor: Colors.transparent,
        builder: (_) => _ConfirmSensitive(item: item),
      );
      if (ok != true || !mounted) return;
    }

    setState(() => _granted.add(item.key));
  }

  void _all(PermissionGroup g, bool on) {
    setState(() {
      for (final i in g.items) {
        if (on) {
          // ⚠ الحسّاسةُ تُستثنى من المنح الجماعيّ — انظر أعلاه.
          if (!i.sensitive) _granted.add(i.key);
        } else {
          _granted.remove(i.key);
        }
      }
    });
  }

  Future<void> _save() async {
    setState(() => _busy = true);
    try {
      await ref.read(employeesRepositoryProvider).setPermissions(
            id: widget.employee.id,
            permissions: _granted.toList(),
          );
      if (!mounted) return;
      ref.invalidate(employeesProvider);
      context.pop();
    } on ApiFailure catch (e) {
      _say(e.message);
    } catch (_) {
      _say('تعذّر الحفظ — تحقّق من الاتصال.');
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

class _GroupCard extends StatelessWidget {
  const _GroupCard({
    required this.group,
    required this.granted,
    required this.onToggle,
    required this.onAll,
  });

  final PermissionGroup group;
  final Set<String> granted;
  final ValueChanged<PermissionItem> onToggle;
  final ValueChanged<bool> onAll;

  @override
  Widget build(BuildContext context) {
    final count = group.items.where((i) => granted.contains(i.key)).length;

    /*
     * ⚠ «الكل» يُقاس على ما **يجوز** منحُه جماعياً لا على المجموعة كلِّها.
     *
     * ولو قِيس على الكلّ لَما ظهرت «إلغاء الكل» أبداً في مجموعةٍ فيها
     * حسّاسة: الوكيل يضغط «تحديد الكل» فيُمنح كلُّ ما يجوز، ويبقى الزرُّ
     * يقول «تحديد الكل» — فيضغطه ثانيةً وثالثة ولا يتغيّر شيء، فيظنّ
     * الشاشةَ معطوبة.
     */
    final bulk = group.items.where((i) => !i.sensitive).toList();
    final all = bulk.isNotEmpty &&
        bulk.every((i) => granted.contains(i.key));

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            children: [
              Expanded(
                child: Text('${group.name}  ($count/${group.items.length})',
                    style: T.kufi(14, FontWeight.w700)),
              ),
              // «الكل» اختصارٌ للوكيل لا صلاحيةٌ في ذاتها: يعلّم البنود
              // واحداً واحداً، فما يُحفظ هو ما يُرى.
              GestureDetector(
                onTap: () => onAll(!all),
                child: Text(all ? 'إلغاء الكل' : 'تحديد الكل',
                    style: T.plex(11.5, FontWeight.w600, color: R.primaryDark)),
              ),
            ],
          ),
          const SizedBox(height: 10),
          Divider(color: R.inkA(.07), height: 1),
          for (final item in group.items)
            _PermRow(
              label: item.label,
              live: item.live,
              sensitive: item.sensitive,
              on: granted.contains(item.key),
              onTap: () => onToggle(item),
            ),
        ],
      ),
    );
  }
}

class _PermRow extends StatelessWidget {
  const _PermRow({
    required this.label,
    required this.live,
    required this.sensitive,
    required this.on,
    required this.onTap,
  });

  final String label;
  final bool live;
  final bool sensitive;
  final bool on;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onTap,
        child: Padding(
          padding: const EdgeInsets.symmetric(vertical: 11),
          child: Row(
            children: [
              AnimatedContainer(
                duration: const Duration(milliseconds: 150),
                width: 22,
                height: 22,
                alignment: Alignment.center,
                decoration: BoxDecoration(
                  color: on ? R.primary : Colors.transparent,
                  border: Border.all(
                    color: on ? R.primary : R.inkA(.22),
                    width: 1.6,
                  ),
                  borderRadius: BorderRadius.circular(7),
                ),
                child: on
                    ? const Icon(Icons.check_rounded,
                        size: 15, color: Colors.white)
                    : null,
              ),
              const SizedBox(width: 12),
              Expanded(
                child: Text(label,
                    style: T.plex(13, FontWeight.w500,
                        color: on ? R.ink : R.inkA(.62))),
              ),

              // ⚠ وسمٌ لا تعطيل: المنحُ المسبق يبقى ممكناً — تُمنح اليوم
              // فتعمل يوم تُوصَل الميزة، بلا أن يعود الوكيل إلى الشاشة.
              // والمقصود ألّا يظنّ أنه أعطى قدرةً تظهر الآن.
              if (!live)
                Container(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
                  decoration: BoxDecoration(
                    color: R.warnBg,
                    borderRadius: BorderRadius.circular(999),
                    border: Border.all(color: R.warnBorder),
                  ),
                  child: Text('لم تُفعَّل بعد',
                      style: T.plex(10.5, FontWeight.w600, color: R.warnInk)),
                ),

              /*
               * ⚠ وسمُ الحسّاسة — ليمنع سؤالاً لا جواباً له.
               *
               * وكيلٌ يضغط «تحديد الكل» ثم يرى بنداً غيرَ معلَّم يظنّ الشاشةَ
               * معطوبة. والوسمُ يقول له إنّ استثناءه مقصود، وإنّ منحَه
               * بيده إن أراد.
               */
              if (sensitive) ...[
                if (!live) const SizedBox(width: 6),
                Container(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
                  decoration: BoxDecoration(
                    color: R.error.withValues(alpha: .08),
                    borderRadius: BorderRadius.circular(999),
                    border: Border.all(color: R.error.withValues(alpha: .3)),
                  ),
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(Icons.lock_outline_rounded,
                          size: 11, color: R.errorText),
                      const SizedBox(width: 3),
                      Text('تُمنح يدوياً',
                          style: T.plex(10.5, FontWeight.w600,
                              color: R.errorText)),
                    ],
                  ),
                ),
              ],
            ],
          ),
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

/// سؤالُ التأكيد قبل منح صلاحيةٍ حسّاسة.
///
/// ⚠ **يسمّي ما سيُكشَف، ونصُّه من الخادم.** «هل أنت متأكد؟» يُقرأ روتيناً
/// ويُضغط بلا نظر؛ و«سيرى الموظف رصيد وكالتك الكلّي» يُقرأ قراراً. ولا جملةَ
/// تحذيرٍ ولا اسمَ صلاحيةٍ مكتوبٌ في هذا الملف — الكتالوج كلُّه من الخادم.
class _ConfirmSensitive extends StatelessWidget {
  const _ConfirmSensitive({required this.item});

  final PermissionItem item;

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
            Row(
              children: [
                Container(
                  width: 46,
                  height: 46,
                  alignment: Alignment.center,
                  decoration: BoxDecoration(
                    color: R.error.withValues(alpha: .08),
                    borderRadius: BorderRadius.circular(16),
                  ),
                  child: Icon(Icons.visibility_outlined,
                      size: 22, color: R.error),
                ),
                const SizedBox(width: 13),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(item.label, style: T.kufi(16, FontWeight.w700)),
                      const SizedBox(height: 4),
                      Text(
                        // الافتراضيُّ لخادمٍ لا يُرسل السبب — عامٌّ لكنه صادق.
                        item.why ?? 'صلاحية تكشف معلومة مالية عن وكالتك.',
                        style: T.plex(12.5, FontWeight.w400,
                            color: R.inkA(.6), height: 1.55),
                      ),
                    ],
                  ),
                ),
              ],
            ),
            const SizedBox(height: 18),
            const WarnBanner(
              text: 'هذه الصلاحية لا تُمنح مع «تحديد الكل» — تُمنح بيدك وحدك. '
                  'وتستطيع سحبها في أي وقت، فتُغلق فوراً.',
            ),
            const SizedBox(height: 18),
            PrimaryButton(
              label: 'أمنحها',
              onPressed: () => Navigator.of(context).pop(true),
            ),
            const SizedBox(height: 10),
            SecondaryButton(
              label: 'إلغاء',
              onPressed: () => Navigator.of(context).pop(false),
            ),
          ],
        ),
      );
}
