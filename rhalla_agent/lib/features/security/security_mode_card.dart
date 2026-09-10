import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/glass.dart';
import 'app_lock.dart';

/// بطاقةُ اختيار حماية الدخول — بصمة/وجه · نمط الجهاز · بلا حماية.
///
/// ⚠ **زرٌّ افتراضيّ لا صلاحية**: يظهر للوكيل وللموظف سواء، ولا يحكمه الوكيل
/// في تطبيق الموظف — كلٌّ يؤمّن جهازه كما يشاء (أمر المالك). يُخزَّن الاختيار
/// على الجهاز، ويقود القفلَ التلقائيّ عبر `AppLockController`.
class SecurityModeCard extends ConsumerStatefulWidget {
  const SecurityModeCard({super.key});

  @override
  ConsumerState<SecurityModeCard> createState() => _SecurityModeCardState();
}

class _SecurityModeCardState extends ConsumerState<SecurityModeCard> {
  String? _mode; // null حتى يُقرأ الوضع الحاليّ

  @override
  void initState() {
    super.initState();
    _mode = ref.read(appLockProvider.notifier).mode;
  }

  Future<void> _select(String mode) async {
    setState(() => _mode = mode);
    await ref.read(appLockProvider.notifier).setSecurityMode(mode);
  }

  @override
  Widget build(BuildContext context) {
    final available = ref.watch(appLockProvider).biometricsAvailable;
    final current = _mode ?? ref.read(appLockProvider.notifier).mode;

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text('حماية الدخول', style: T.section),
          const SizedBox(height: 4),
          Text('اختر كيف يُفتح التطبيق على جهازك.',
              style: T.plex(12, FontWeight.w400, color: R.inkA(.55))),
          const SizedBox(height: 14),
          _Option(
            icon: Icons.face_rounded,
            title: 'بصمة الوجه أو الإصبع',
            subtitle: available
                ? 'يُفتح بحيويّتك المسجّلة في النظام.'
                : 'غير متاحة على هذا الجهاز — سجّل بصمةً في إعدادات النظام.',
            selected: current == 'biometric',
            enabled: available,
            onTap: available ? () => _select('biometric') : null,
          ),
          _Option(
            icon: Icons.pattern_rounded,
            title: 'نمط الجهاز أو رقمه السرّي',
            subtitle: 'يُفتح بقفل شاشة جهازك (نمط · رقم · كلمة مرور).',
            selected: current == 'device',
            enabled: true,
            onTap: () => _select('device'),
          ),
          _Option(
            icon: Icons.lock_open_rounded,
            title: 'بلا حماية دخول',
            subtitle: 'يُفتح مباشرة — أنت مسؤول عن تأمين جهازك.',
            selected: current == 'none',
            enabled: true,
            danger: true,
            onTap: () => _select('none'),
            last: true,
          ),
        ],
      ),
    );
  }
}

class _Option extends StatelessWidget {
  const _Option({
    required this.icon,
    required this.title,
    required this.subtitle,
    required this.selected,
    required this.enabled,
    this.onTap,
    this.danger = false,
    this.last = false,
  });

  final IconData icon;
  final String title;
  final String subtitle;
  final bool selected;
  final bool enabled;
  final VoidCallback? onTap;
  final bool danger;
  final bool last;

  @override
  Widget build(BuildContext context) {
    final tone = danger ? R.error : R.primaryDark;
    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(R.rCard),
      child: Container(
        padding: const EdgeInsets.symmetric(vertical: 12),
        decoration: last
            ? null
            : BoxDecoration(
                border: Border(bottom: BorderSide(color: R.inkA(.06)))),
        child: Opacity(
          opacity: enabled ? 1 : .5,
          child: Row(
            children: [
              Container(
                width: 38,
                height: 38,
                alignment: Alignment.center,
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  color: (selected ? tone : R.inkA(.4)).withValues(alpha: .12),
                ),
                child: Icon(icon,
                    size: 19, color: selected ? tone : R.inkA(.5)),
              ),
              const SizedBox(width: 12),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(title, style: T.kufi(13.5, FontWeight.w700)),
                    const SizedBox(height: 3),
                    Text(subtitle,
                        style: T.plex(11.5, FontWeight.w400,
                            color: R.inkA(.55), height: 1.5)),
                  ],
                ),
              ),
              const SizedBox(width: 8),
              Icon(
                selected
                    ? Icons.radio_button_checked_rounded
                    : Icons.radio_button_unchecked_rounded,
                size: 22,
                color: selected ? tone : R.inkA(.3),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
