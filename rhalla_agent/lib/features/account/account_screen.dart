import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';

class AccountScreen extends ConsumerWidget {
  const AccountScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final user = ref.watch(authControllerProvider).user;

    return Screen(
      child: ListView(
        padding: EdgeInsets.zero,
        children: [
          _ProfileHeader(
            name: user?.displayName ?? '',
            initial: user?.initial,
            role: user?.isMainAgent == true ? 'وكيل رئيسي' : 'نقطة بيع',
            accId: user?.accId,
          ),
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 20, R.padScreen, 120),
            child: Column(
              children: [
                GlassCard(
                  padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 6),
                  child: Column(
                    children: [
                      _Row(
                        icon: Icons.badge_outlined,
                        label: 'بياناتي',
                        value: user == null ? null : Fmt.phone(user.phone),
                      ),
                      if (user?.isMainAgent == true)
                        _Row(
                          icon: Icons.storefront_outlined,
                          label: 'نقاط البيع',
                          onTap: () => context.go('/pos'),
                        ),
                      _Row(
                        icon: Icons.star_outline_rounded,
                        label: 'المفضّلة',
                        onTap: () => context.push('/favorites'),
                      ),
                      _Row(
                        icon: Icons.speed_outlined,
                        label: 'السقوف والعمولات',
                        onTap: () => context.push('/limits'),
                      ),
                      _Row(
                        icon: Icons.description_outlined,
                        label: 'كشف الحساب',
                        onTap: () => context.push('/statement'),
                        last: true,
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: R.gapCard),
                GlassCard(
                  padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 6),
                  child: Column(
                    children: [
                      // «الإشعارات · مفعّلة» حُذف: لا شيء مُوصَّل — Pusher
                      // غير مربوط بعد — فكان الصف يدّعي ما لا يحدث.
                      _Row(
                        icon: Icons.shield_outlined,
                        label: 'الخصوصية والأمان',
                        onTap: () => context.push('/security'),
                        last: true,
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: R.gapCard),
                GlassCard(
                  padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 6),
                  child: Column(
                    children: [
                      _Row(
                        icon: Icons.article_outlined,
                        label: 'الشروط والأحكام',
                        onTap: () => context.push('/terms'),
                      ),
                      _Row(
                        icon: Icons.logout_rounded,
                        label: 'تسجيل الخروج',
                        onTap: () => _confirmSignOut(context, ref),
                        last: true,
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: R.gapCard),
                GlassCard(
                  padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 6),
                  child: _Row(
                    icon: Icons.delete_outline_rounded,
                    label: 'حذف الحساب',
                    danger: true,
                    last: true,
                    onTap: () => _confirmDelete(context),
                  ),
                ),
                const SizedBox(height: 20),
                Text('رحلة · الرحالة للصرافة', style: T.meta),
                const SizedBox(height: 6),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text('1.0.0', style: T.meta),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _confirmSignOut(BuildContext context, WidgetRef ref) async {
    final ok = await _ask(
      context,
      title: 'تسجيل الخروج',
      body: 'ستحتاج إلى رمز تحقّق جديد للدخول مرة أخرى.',
      confirm: 'تسجيل الخروج',
    );
    if (ok == true) await ref.read(authControllerProvider.notifier).signOut();
  }

  Future<void> _confirmDelete(BuildContext context) async {
    await _ask(
      context,
      title: 'حذف الحساب',
      // الخادم يحذف حذفاً ناعماً: Reg='NO' و deleted_at — لا يُزال الصف.
      body: 'سيتوقّف حسابك عن العمل ولن تستطيع الدخول. '
          'لا يمكن التراجع عن هذا من التطبيق — يحتاج مراجعة الفرع.',
      confirm: 'حذف الحساب',
      danger: true,
    );
  }

  Future<bool?> _ask(
    BuildContext context, {
    required String title,
    required String body,
    required String confirm,
    bool danger = false,
  }) =>
      showModalBottomSheet<bool>(
        context: context,
        backgroundColor: Colors.transparent,
        builder: (_) => Container(
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
              Text(title,
                  style: T.kufi(17, FontWeight.w600,
                      color: danger ? R.error : R.ink)),
              const SizedBox(height: 10),
              Text(body,
                  style: T.plex(12.5, FontWeight.w400,
                      color: R.inkA(.6), height: 1.7)),
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
        ),
      );
}

class _ProfileHeader extends StatelessWidget {
  const _ProfileHeader({
    required this.name,
    required this.initial,
    required this.role,
    required this.accId,
  });

  final String name;
  final String? initial;
  final String role;
  final int? accId;

  @override
  Widget build(BuildContext context) {
    final top = MediaQuery.paddingOf(context).top;

    return Container(
      padding: EdgeInsets.fromLTRB(R.padScreen, top + 16, R.padScreen, 44),
      decoration: const BoxDecoration(
        gradient: R.headerGradient,
        borderRadius:
            BorderRadius.vertical(bottom: Radius.circular(R.rHeaderBottom)),
      ),
      clipBehavior: Clip.antiAlias,
      child: Stack(
        clipBehavior: Clip.none,
        children: [
          PositionedDirectional(
            top: -40,
            end: -30,
            child: RhallaLogo(size: 220, color: R.whiteA(.09)),
          ),
          RiseIn.small(
            child: Row(
              children: [
                Container(
                  width: 46,
                  height: 46,
                  alignment: Alignment.center,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: R.whiteA(.2),
                    border: Border.all(color: R.whiteA(.34)),
                  ),
                  child: initial == null
                      ? const Icon(Icons.person_outline_rounded,
                          size: 22, color: Colors.white)
                      : Text(initial!,
                          style:
                              T.kufi(16, FontWeight.w600, color: Colors.white)),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(name,
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style:
                              T.kufi(16, FontWeight.w600, color: Colors.white)),
                      const SizedBox(height: 8),
                      Text(role,
                          style: T.plex(11.5, FontWeight.w400,
                              color: R.whiteA(.82))),
                      if (accId != null) ...[
                        const SizedBox(height: 11),
                        Container(
                          padding: const EdgeInsets.symmetric(
                              horizontal: 14, vertical: 7),
                          decoration: BoxDecoration(
                            color: R.whiteA(.18),
                            border: Border.all(color: R.whiteA(.3)),
                            borderRadius: BorderRadius.circular(99),
                          ),
                          child: Directionality(
                            textDirection: TextDirection.ltr,
                            child: Text('ACC $accId',
                                style: T.kufi(12, FontWeight.w600,
                                    color: Colors.white, spacing: .72)),
                          ),
                        ),
                      ],
                    ],
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _Row extends StatelessWidget {
  const _Row({
    required this.icon,
    required this.label,
    this.value,
    this.onTap,
    this.danger = false,
    this.last = false,
  });

  final IconData icon;
  final String label;
  final String? value;
  final VoidCallback? onTap;
  final bool danger;
  final bool last;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onTap,
        child: Container(
          constraints: const BoxConstraints(minHeight: 44),
          padding: const EdgeInsets.symmetric(vertical: 14),
          decoration: last
              ? null
              : BoxDecoration(
                  border: Border(bottom: BorderSide(color: R.inkA(.07))),
                ),
          child: Row(
            children: [
              IconTile(
                icon: Icon(icon,
                    size: 18, color: danger ? R.error : R.primaryGradEnd),
                background: danger ? R.error.withValues(alpha: .08) : null,
              ),
              const SizedBox(width: 13),
              Expanded(
                child: Text(label,
                    style: T.plex(13.5, FontWeight.w500,
                        color: danger ? R.error : R.ink)),
              ),
              if (value != null) ...[
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(value!, style: T.meta),
                ),
                const SizedBox(width: 8),
              ],
              if (!danger && onTap != null)
                Icon(Icons.arrow_forward_ios, size: 14, color: R.inkA(.4)),
            ],
          ),
        ),
      );
}
