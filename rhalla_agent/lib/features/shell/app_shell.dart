import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../auth/auth_controller.dart';
import 'auto_refresh.dart';

class AppShell extends ConsumerWidget {
  const AppShell({super.key, required this.navigationShell});

  final StatefulNavigationShell navigationShell;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    // تبويب نقاط البيع للوكيل الرئيسي فقط — نقطة البيع نفسها لا تراه،
    // والخادم يرد 403 على نقاطه أصلاً.
    final isMain = ref.watch(authControllerProvider).user?.isMainAgent ?? false;

    return Scaffold(
      backgroundColor: Colors.transparent,
      body: Stack(
        children: [
          AutoRefresh(
            tabIndex: navigationShell.currentIndex,
            child: navigationShell,
          ),
          PositionedDirectional(
            start: 16,
            end: 16,
            bottom: 14,
            child: _NavBar(
              index: navigationShell.currentIndex,
              showPos: isMain,
              onTap: (i) => navigationShell.goBranch(
                i,
                initialLocation: i == navigationShell.currentIndex,
              ),
            ),
          ),
        ],
      ),
    );
  }
}

class _NavBar extends StatelessWidget {
  const _NavBar({required this.index, required this.onTap, required this.showPos});

  final int index;
  final ValueChanged<int> onTap;
  final bool showPos;

  @override
  Widget build(BuildContext context) {
    final items = <_NavItem>[
      const _NavItem('الرئيسية', Icons.home_outlined, Icons.home_rounded),
      const _NavItem('الحوالات', Icons.swap_horiz_rounded, Icons.swap_horiz_rounded),
      if (showPos) const _NavItem('نقاط البيع', Icons.storefront_outlined, Icons.storefront_rounded),
      const _NavItem('الحساب', Icons.person_outline_rounded, Icons.person_rounded),
    ];

    return DecoratedBox(
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(R.rNav),
        boxShadow: R.shNav,
      ),
      child: ClipRRect(
        borderRadius: BorderRadius.circular(R.rNav),
        child: BackdropFilter(
          filter: ImageFilter.blur(sigmaX: R.blurNav, sigmaY: R.blurNav),
          child: Container(
            height: 72,
            padding: const EdgeInsets.symmetric(horizontal: 8),
            decoration: BoxDecoration(
              color: R.whiteA(.8),
              border: Border.all(color: R.whiteA(.92)),
              borderRadius: BorderRadius.circular(R.rNav),
            ),
            child: Row(
              children: [
                for (var i = 0; i < items.length; i++)
                  Expanded(
                    child: _Tab(
                      item: items[i],
                      active: i == index,
                      onTap: () => onTap(i),
                    ),
                  ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

class _NavItem {
  const _NavItem(this.label, this.icon, this.activeIcon);
  final String label;
  final IconData icon;
  final IconData activeIcon;
}

class _Tab extends StatelessWidget {
  const _Tab({required this.item, required this.active, required this.onTap});

  final _NavItem item;
  final bool active;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    // .5 بدل .42 — الأخيرة تسقط تحت 2.5:1 على الزجاج، وهي التنقّل الرئيسي.
    final color = active ? R.primaryGradEnd : R.inkA(.5);

    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(R.rNav),
      child: SizedBox(
        height: 72,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            AnimatedContainer(
              duration: const Duration(milliseconds: 220),
              padding: EdgeInsets.symmetric(horizontal: active ? 16 : 0, vertical: 5),
              decoration: BoxDecoration(
                color: active ? R.primaryA(.14) : Colors.transparent,
                borderRadius: BorderRadius.circular(99),
              ),
              child: Icon(active ? item.activeIcon : item.icon, size: 21, color: color),
            ),
            const SizedBox(height: 6),
            Text(
              item.label,
              style: T.plex(11, active ? FontWeight.w600 : FontWeight.w500, color: color),
            ),
          ],
        ),
      ),
    );
  }
}
