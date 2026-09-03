import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'employees_repository.dart';

/// «الأجهزة المفعّلة» — ومنها يُلغى الجهاز.
///
/// ⚠ ما يجب أن يعرفه الوكيل قبل الضغط، ولذلك يُقال في ورقة التأكيد صراحةً:
/// إلغاء الجهاز **لا يرفع** حظر الدخول كمسؤول عنه. التصنيف الأمني في
/// الخادم دائم ولا يُمحى بإلغاء ولا بخروج ولا بحذف التطبيق.
class EmployeeDevicesScreen extends ConsumerWidget {
  const EmployeeDevicesScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(employeeDevicesProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'الأجهزة المفعّلة', onBack: () => context.pop()),
          Expanded(
            child: async.when(
              loading: () =>
                  Center(child: CircularProgressIndicator(color: R.primary)),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(employeeDevicesProvider),
              ),
              data: (rows) => RefreshIndicator(
                onRefresh: () async =>
                    ref.refresh(employeeDevicesProvider.future),
                color: R.primary,
                backgroundColor: Colors.white,
                child: rows.isEmpty
                    ? ListView(
                        physics: const AlwaysScrollableScrollPhysics(),
                        children: const [SizedBox(height: 80), _Empty()],
                      )
                    : ListView(
                        padding: const EdgeInsets.fromLTRB(
                            R.padScreen, 14, R.padScreen, 40),
                        physics: const AlwaysScrollableScrollPhysics(),
                        children: [
                          for (var i = 0; i < rows.length; i++) ...[
                            if (i > 0) const SizedBox(height: R.gapRow),
                            _DeviceCard(d: rows[i]),
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
}

class _DeviceCard extends ConsumerStatefulWidget {
  const _DeviceCard({required this.d});

  final EmployeeDevice d;

  @override
  ConsumerState<_DeviceCard> createState() => _DeviceCardState();
}

class _DeviceCardState extends ConsumerState<_DeviceCard> {
  bool _busy = false;

  @override
  Widget build(BuildContext context) {
    final d = widget.d;
    final tone = d.isActive ? R.primaryDark : R.inkA(.5);

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            children: [
              IconTile(
                size: 38,
                background: tone.withValues(alpha: .12),
                icon: Icon(Icons.phone_android_rounded, size: 19, color: tone),
              ),
              const SizedBox(width: 12),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(d.employeeName,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: T.kufi(14.5, FontWeight.w700)),
                    const SizedBox(height: 3),
                    Directionality(
                      textDirection: TextDirection.ltr,
                      child: Text(Fmt.phone(d.phone),
                          style: T.plex(12, FontWeight.w500,
                              color: R.inkA(.55))),
                    ),
                  ],
                ),
              ),
              Container(
                padding:
                    const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
                decoration: BoxDecoration(
                  color: tone.withValues(alpha: .10),
                  border: Border.all(color: tone.withValues(alpha: .28)),
                  borderRadius: BorderRadius.circular(99),
                ),
                child: Text(d.statusLabel,
                    style: T.plex(10.5, FontWeight.w600, color: tone)),
              ),
            ],
          ),
          const SizedBox(height: 10),
          Divider(color: R.inkA(.07), height: 1),
          const SizedBox(height: 10),

          if (d.pointOfSale.isNotEmpty) ...[
            _Line(icon: Icons.storefront_outlined, text: d.pointOfSale),
            const SizedBox(height: 6),
          ],
          _Line(
            icon: Icons.smartphone_rounded,
            text: [d.platform, d.model].where((s) => s.isNotEmpty).join(' · '),
            fallback: 'نوع الجهاز غير معروف',
          ),
          const SizedBox(height: 6),
          // معرّفٌ مختصر يميّز جهازين ولا يكشف المعرّف الحقيقي.
          _Line(icon: Icons.tag_rounded, text: 'المعرّف · ${d.deviceRef}'),
          if (d.activatedAt.isNotEmpty) ...[
            const SizedBox(height: 6),
            _Line(
              icon: Icons.event_available_rounded,
              text: 'فُعّل · ${Fmt.stampShort(d.activatedAt)}',
            ),
          ],
          if (d.lastActivityAt.isNotEmpty) ...[
            const SizedBox(height: 6),
            _Line(
              icon: Icons.schedule_rounded,
              text: 'آخر نشاط · ${Fmt.stampShort(d.lastActivityAt)}',
            ),
          ],

          if (d.isActive) ...[
            const SizedBox(height: 14),
            GlassButton(
              label: 'إلغاء الجهاز',
              onPressed: _busy ? null : _revoke,
            ),
          ],
        ],
      ),
    );
  }

  Future<void> _revoke() async {
    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _ConfirmRevoke(name: widget.d.employeeName),
    );
    if (ok != true || !mounted) return;

    setState(() => _busy = true);
    try {
      await ref.read(employeesRepositoryProvider).revokeDevice(widget.d.id);
      if (!mounted) return;
      ref.invalidate(employeeDevicesProvider);
      ref.invalidate(employeesProvider);
    } on ApiFailure catch (e) {
      _say(e.message);
    } catch (_) {
      _say('تعذّر الإلغاء — تحقّق من الاتصال.');
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

class _ConfirmRevoke extends StatelessWidget {
  const _ConfirmRevoke({required this.name});

  final String name;

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
              child: Text('إلغاء جهاز $name',
                  style: T.kufi(17, FontWeight.w700, color: R.error)),
            ),
            const SizedBox(height: 10),
            Text(
              'ستُغلق جلساته فوراً ويحتاج كوداً جديداً منك للعودة على أي جهاز.',
              textAlign: TextAlign.center,
              style:
                  T.plex(13, FontWeight.w500, color: R.inkA(.65), height: 1.7),
            ),
            const SizedBox(height: 16),
            const WarnBanner(
              text: 'إلغاء الجهاز لا يرفع عنه حظر الدخول كمسؤول — ذلك التصنيف '
                  'دائم في الخادم ولا يُمحى.',
            ),
            const SizedBox(height: 20),
            PrimaryButton(
              label: 'إلغاء الجهاز',
              onPressed: () => Navigator.of(context).pop(true),
            ),
            const SizedBox(height: 10),
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
              child: Text('تراجع',
                  style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
            ),
          ],
        ),
      );
}

class _Line extends StatelessWidget {
  const _Line({required this.icon, required this.text, this.fallback});

  final IconData icon;
  final String text;
  final String? fallback;

  @override
  Widget build(BuildContext context) {
    final shown = text.trim().isEmpty ? (fallback ?? '') : text;
    if (shown.isEmpty) return const SizedBox.shrink();

    return Row(
      children: [
        Icon(icon, size: 14, color: R.inkA(.45)),
        const SizedBox(width: 7),
        Expanded(
          child: Text(shown,
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
              style: T.plex(12, FontWeight.w400, color: R.inkA(.6))),
        ),
      ],
    );
  }
}

class _Empty extends StatelessWidget {
  const _Empty();

  @override
  Widget build(BuildContext context) => Column(
        children: [
          Icon(Icons.devices_outlined, size: 42, color: R.primaryA(.3)),
          const SizedBox(height: 16),
          Text('لا توجد أجهزة مفعّلة',
              style: T.kufi(15, FontWeight.w600, color: R.inkA(.6))),
          const SizedBox(height: 8),
          Text('يظهر الجهاز هنا بعد أن يُفعّل الموظف التطبيق بكوده.',
              textAlign: TextAlign.center,
              style: T.plex(12.5, FontWeight.w400,
                  color: R.inkA(.45), height: 1.8)),
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
