import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/keyboard.dart';
import '../chat/chat_unread.dart';
import '../shell/nav_bar.dart';

/// هيكلُ تطبيق الموظف — **خمسةُ تبويبات**.
///
/// ══════════════════════════════════════════════════════════════════════════
///  أمرُ إعادة الهيكلة (10 سبتمبر 2026) — `docs/employee-app-ui-restructure.md`
/// ══════════════════════════════════════════════════════════════════════════
///
/// كانت الواجهةُ شاشةً واحدةً تحمل ثلاثَ عشرةَ بلاطة، كلُّ بلاطةٍ بابٌ إلى
/// شاشةٍ مدفوعة. فأصبحت:
///
/// ```
/// ١. الحوالات   ← محلية · خارجية · طلباتي · بحث برقم حوالة · نقطة بيعي
/// ٢. التقارير   ← حوالات اليوم (الكل)
/// ٣. الخزينة    ← كشفُ الصادر · كشفُ الوارد المسلَّم
/// ٤. مراسلة الوكيل
/// ٥. المستفيدون
/// ```
///
/// ⚠ **إعادةُ توزيعٍ لا إنشاء.** لم تُبنَ شاشةٌ من الصفر ولم يُحذف حقلٌ ولا
/// كلمة: ما كان بلاطةً صار مدخلاً تحت تبويبه، وما ينقص جُلب من تطبيق الوكيل
/// بالمعامل لا بالنسخ.
///
/// ⚠ **والشريطُ هو شريطُ الوكيل نفسُه** (`AppNavBar`) لا نسخةٌ عنه — قاعدةُ
/// الأمر: «تطبيق الموظف صورةٌ معكوسة من تطبيق الوكيل، وأيُّ تعديلٍ هناك
/// يتبعه هنا». وشريطٌ منسوخٌ ينقض ذلك في اليوم الأوّل.
///
/// ⚠ **ولا تبويبَ يُخفى ولو خلا من صلاحية.** الخمسةُ ثابتة، ومن لم يُمنح
/// ما تحت أحدها يجد فيه سطراً يقول ما ينقصه ويحيله إلى وكيله — لا شاشةً
/// بيضاء ولا تبويباً يختفي فيظنّ الموظف أن التطبيق ناقص. والحارسُ في
/// الخادم على كلّ مسار كما كان: الإخفاءُ تجميلٌ والرفضُ حماية.
class EmployeeShell extends ConsumerStatefulWidget {
  const EmployeeShell({super.key, required this.navigationShell});

  final StatefulNavigationShell navigationShell;

  /// موضعُ تبويب المراسلة — مذكورٌ مرّة، فنقلُه لا يتطلّب البحث عن رقمٍ عارٍ.
  static const chatTab = 3;

  static const _items = <NavItem>[
    NavItem('الحوالات', Icons.swap_horiz_rounded, Icons.swap_horiz_rounded),
    NavItem('التقارير', Icons.assessment_outlined, Icons.assessment_rounded),
    NavItem('الخزينة', Icons.savings_outlined, Icons.savings_rounded),
    NavItem('مراسلة الوكيل', Icons.chat_bubble_outline_rounded,
        Icons.chat_bubble_rounded),
    NavItem('المستفيدون', Icons.people_outline_rounded, Icons.people_rounded),
  ];

  @override
  ConsumerState<EmployeeShell> createState() => _EmployeeShellState();
}

class _EmployeeShellState extends ConsumerState<EmployeeShell>
    with WidgetsBindingObserver {
  ChatUnreadController? _chat;

  /*
   * ⚠ شارةُ المراسلة تُشغَّل من الهيكل لا من شاشة المحادثات.
   *
   * شارةٌ لا تُحدَّث إلّا والموظف داخل شاشة المحادثة ليست شارة: هي موجودةٌ
   * ليعلم بالرسالة **وهو في تبويبٍ آخر**. والهيكلُ هو ما يعيش طوال الجلسة،
   * والشاشاتُ تحته تُبنى وتُهدَم — وهو المنطقُ نفسُه في `AutoRefresh` عند
   * الوكيل.
   *
   * ⚠ و`employeeMode` تُضبط هنا: نقطةُ الوكيل تردّ 403 على رمز الموظف،
   * فبدونها كانت الشارةُ تفشل صامتةً وتبقى صفراً إلى الأبد.
   */
  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addObserver(this);
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (!mounted) return;
      final c = ref.read(chatUnreadProvider.notifier);
      c.employeeMode = true;
      _chat = c;
      c.start();
    });
  }

  @override
  void dispose() {
    WidgetsBinding.instance.removeObserver(this);
    _chat?.stop();
    super.dispose();
  }

  // في الخلفية لا شاشة تُقرأ — نبضٌ عندها استنزافٌ للبطارية بلا فائدة.
  // وعند العودة نبضةٌ فورية لا انتظارَ دورةٍ كاملة.
  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    if (state == AppLifecycleState.resumed) {
      _chat?.start();
    } else {
      _chat?.stop();
    }
  }

  @override
  Widget build(BuildContext context) {
    final navigationShell = widget.navigationShell;

    return Scaffold(
      backgroundColor: Colors.transparent,
      body: Stack(
        children: [
          navigationShell,
          PositionedDirectional(
            start: 16,
            end: 16,
            bottom: 14,
            child: AppNavBar(
              items: EmployeeShell._items,
              index: navigationShell.currentIndex,
              badgeAt: EmployeeShell.chatTab,
              badge: ref.watch(chatUnreadProvider),
              // إغلاق اللوحة قبل تبديل التبويب: فروعُ الهيكل تبقى حيّة في
              // `go_router`، فحقلٌ مركَّزٌ في تبويبٍ غادره الموظف يُبقي
              // اللوحة مفتوحةً فوق تبويبٍ آخر لا حقلَ فيه.
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
