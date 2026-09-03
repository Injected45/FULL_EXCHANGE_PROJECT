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
                      onToggle: (key) => setState(() {
                        _granted.contains(key)
                            ? _granted.remove(key)
                            : _granted.add(key);
                      }),
                      onAll: (on) => setState(() {
                        for (final i in g.items) {
                          on ? _granted.add(i.key) : _granted.remove(i.key);
                        }
                      }),
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
  final ValueChanged<String> onToggle;
  final ValueChanged<bool> onAll;

  @override
  Widget build(BuildContext context) {
    final count = group.items.where((i) => granted.contains(i.key)).length;
    final all = count == group.items.length && count > 0;

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
              on: granted.contains(item.key),
              onTap: () => onToggle(item.key),
            ),
        ],
      ),
    );
  }
}

class _PermRow extends StatelessWidget {
  const _PermRow({required this.label, required this.on, required this.onTap});

  final String label;
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
