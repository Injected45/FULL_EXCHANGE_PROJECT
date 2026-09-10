import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/keyboard.dart';
import '../chat/chat_unread.dart';
import 'auto_refresh.dart';
import 'nav_bar.dart';

class AppShell extends ConsumerWidget {
  const AppShell({super.key, required this.navigationShell});

  final StatefulNavigationShell navigationShell;

  /// موضع تبويب الدردشة — مذكورٌ مرّة، فنقلُه لا يتطلّب البحث عن رقمٍ عارٍ.
  static const _chatTab = 2;

  // أربعة تبويبات — «الدردشة» بينها بأمر المالك (5 سبتمبر 2026)، وموضعها
  // بين التقارير والحساب كما طلب.
  static const _items = <NavItem>[
    NavItem('الرئيسية', Icons.home_outlined, Icons.home_rounded),
    NavItem('التقارير', Icons.assessment_outlined, Icons.assessment_rounded),
    NavItem('الدردشة', Icons.chat_bubble_outline_rounded,
        Icons.chat_bubble_rounded),
    NavItem('الإعدادات', Icons.settings_outlined, Icons.settings_rounded),
  ];

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
            child: AppNavBar(
              items: _items,
              index: navigationShell.currentIndex,
              badgeAt: _chatTab,
              // شارة الدردشة تُقرأ هنا لا داخل الشريط: الشريط بلا حالة،
              // والقراءة في مكانٍ واحد تجعل موضع التبويب هو التغيير الوحيد
              // لو نُقل لاحقاً.
              badge: ref.watch(chatUnreadProvider),
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
