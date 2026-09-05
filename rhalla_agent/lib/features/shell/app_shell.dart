import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/keyboard.dart';
import '../chat/chat_unread.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import 'auto_refresh.dart';

class AppShell extends ConsumerWidget {
  const AppShell({super.key, required this.navigationShell});

  final StatefulNavigationShell navigationShell;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
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
              // شارة الدردشة تُقرأ هنا لا داخل الشريط: الشريط بلا حالة،
              // والقراءة في مكانٍ واحد تجعل موضع التبويب هو التغيير الوحيد
              // لو نُقل لاحقاً.
              chatUnread: ref.watch(chatUnreadProvider),
              // إغلاق اللوحة قبل تبديل التبويب: فروع الهيكل تبقى حيّة في
              // `go_router`، فحقلٌ مركَّز في تبويبٍ غادرَه الوكيل يُبقي اللوحة
              // مفتوحة فوق تبويبٍ آخر لا حقل فيه.
              onTap: (i) {
                hideKeyboard();
                navigationShell.goBranch(
                  i,
                  initialLocation: i == navigationShell.currentIndex,
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}

class _NavBar extends StatelessWidget {
  const _NavBar({
    required this.index,
    required this.onTap,
    this.chatUnread = 0,
  });

  final int index;
  final ValueChanged<int> onTap;
  final int chatUnread;

  /// موضع تبويب الدردشة — مذكورٌ مرّة، فنقلُه لا يتطلّب البحث عن رقمٍ عارٍ.
  static const _chatTab = 2;

  @override
  Widget build(BuildContext context) {
    // أربعة تبويبات — «الدردشة» بينها بأمر المالك (5 سبتمبر 2026)، وموضعها
    // بين التقارير والحساب كما طلب.
    final items = <_NavItem>[
      const _NavItem('الرئيسية', Icons.home_outlined, Icons.home_rounded),
      const _NavItem('التقارير', Icons.assessment_outlined, Icons.assessment_rounded),
      const _NavItem('الدردشة', Icons.chat_bubble_outline_rounded,
          Icons.chat_bubble_rounded),
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
                      // لا شارة على التبويب المفتوح: الوكيل ينظر إليه الآن،
                      // ورقمٌ فوق ما يقرؤه إلحاحٌ بلا معنى.
                      badge: i == _chatTab && i != index ? chatUnread : 0,
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
  const _Tab({
    required this.item,
    required this.active,
    required this.onTap,
    this.badge = 0,
  });

  final _NavItem item;
  final bool active;
  final VoidCallback onTap;

  /// عدد غير المقروء — صفرٌ يعني بلا شارة.
  final int badge;

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
              child: Stack(
                clipBehavior: Clip.none,
                children: [
                  Icon(active ? item.activeIcon : item.icon,
                      size: 21, color: color),

                  // شارة غير المقروء على أيقونة التبويب.
                  //
                  // على الأيقونة لا بجانب الاسم: التبويب النشط يتمدّد أفقياً
                  // (‏AnimatedContainer أعلاه)، فشارةٌ في الصفّ كانت تتحرّك
                  // مع كل تبديل تبويب.
                  if (badge > 0)
                    PositionedDirectional(
                      top: -5,
                      end: -8,
                      child: Container(
                        constraints:
                            const BoxConstraints(minWidth: 16, minHeight: 16),
                        padding: const EdgeInsets.symmetric(horizontal: 4),
                        alignment: Alignment.center,
                        decoration: BoxDecoration(
                          color: R.error,
                          borderRadius: BorderRadius.circular(99),
                          // حدٌّ بلون الشريط الزجاجي يفصل الرقم عن الأيقونة
                          // حين يعلوها.
                          border: Border.all(color: Colors.white, width: 1.5),
                        ),
                        child: Directionality(
                          // رقمٌ لاتيني في فقرة عربية — يُفرض اتجاهه.
                          textDirection: TextDirection.ltr,
                          child: Text(badge > 9 ? '9+' : '$badge',
                              style: T.plex(9.5, FontWeight.w700,
                                  color: Colors.white)),
                        ),
                      ),
                    ),
                ],
              ),
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
