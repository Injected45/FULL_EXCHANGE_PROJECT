import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import 'core/keyboard.dart';
import 'features/account/account_screen.dart';
import 'features/account/security_screen.dart';
import 'features/branding/branding_controller.dart';
import 'features/branding/branding_screen.dart';
import 'features/chat/chat_threads_screen.dart';
import 'features/employees/employee_devices_screen.dart';
import 'features/employees/employee_permissions_screen.dart';
import 'features/employees/employee_reports_screen.dart';
import 'features/employees/employees_repository.dart';
import 'features/employees/employees_screen.dart';
import 'features/employee_app/employee_activation_screen.dart';
import 'features/employee_app/employee_home_screen.dart';
import 'features/employee_app/employee_session.dart';
import 'features/employee_app/employee_shift_screens.dart';
import 'features/employee_app/employee_transfers_screen.dart';
import 'features/auth/auth_controller.dart';
import 'features/auth/onboarding_screen.dart';
import 'features/auth/otp_screen.dart';
import 'features/auth/phone_screen.dart';
import 'features/auth/splash_screen.dart';
import 'features/favorites/favorites_repository.dart';
import 'features/favorites/favorites_screen.dart';
import 'features/home/home_screen.dart';
import 'features/legal/terms_screen.dart';
import 'features/limits/limits_screen.dart';
import 'features/pos/pos_screen.dart';
import 'features/send/accounts_repository.dart';
import 'features/send/review_accounts_screen.dart';
import 'features/send/review_screen.dart';
import 'features/send/send_accounts_screen.dart';
import 'features/send/send_external_screen.dart';
import 'features/send/send_internal_screen.dart';
import 'features/send/send_repository.dart';
import 'features/send/success_screen.dart';
import 'features/shell/app_shell.dart';
import 'features/statement/statement_screen.dart';
import 'features/reports/reports_screen.dart';
import 'features/transfers/transfers_screen.dart';

final _rootKey = GlobalKey<NavigatorState>();

final routerProvider = Provider<GoRouter>((ref) {
  final auth = ref.watch(authControllerProvider);

  // هوية الشركة تُقرأ هنا لا لتُعرض، بل لتُؤخَّر شاشاتُ ما بعد الدخول حتى
  // تستقرّ — انظر التعليق على `BrandingState.settled`.
  final brandSettled =
      ref.watch(brandingControllerProvider.select((s) => s.settled));

  // وضع الموظف — مسارٌ مستقلّ تماماً عن مسار الوكيل.
  //
  // ⚠ الفصل بين السياقين شرط أمني (بند 22): جلسة موظف لا تُرقّى إلى مسؤول.
  // ولذلك لا تشارك الشاشتان تبويباً ولا هيكلاً، والراوتر يحسم أيّهما قبل كل
  // شيء آخر.
  final emp = ref.watch(employeeAuthProvider);
  final employeeIn = emp.status == EmpSessionStatus.signedIn;

  return GoRouter(
    navigatorKey: _rootKey,
    // يُغلق لوحة المفاتيح عند كل انتقال — انظر [KeyboardDismisser].
    observers: [KeyboardDismisser()],
    initialLocation: '/',
    refreshListenable: _AuthListenable(ref),
    redirect: (context, state) {
      final loc = state.matchedLocation;
      final inEmployeeArea = loc.startsWith('/employee/');

      // الموظف الداخل يبقى في مساره ولا يرى شاشة وكيل واحدة.
      if (employeeIn) {
        return inEmployeeArea && loc != '/employee/activate'
            ? null
            : '/employee/home';
      }

      // شاشة التفعيل مفتوحة قبل الدخول — وهي المدخل الوحيد لمسار الموظف.
      if (loc == '/employee/activate') return null;

      // موظفٌ خرج أو أُلغي جهازه: لا يبقى في شاشات الموظف.
      if (inEmployeeArea) return '/phone';

      // لم تُقرأ الحالة من التخزين بعد.
      if (auth.status == AuthStatus.unknown ||
          emp.status == EmpSessionStatus.unknown) {
        return loc == '/splash' ? null : '/splash';
      }

      final signedIn = auth.status == AuthStatus.signedIn;
      final inAuthFlow = loc == '/phone' ||
          loc == '/otp' ||
          loc == '/onboarding' ||
          loc == '/splash';

      if (!signedIn) {
        if (!auth.onboarded) {
          return loc == '/onboarding' ? null : '/onboarding';
        }
        return inAuthFlow && loc != '/splash' ? null : '/phone';
      }

      // مسجّل دخول، والهوية لم تستقرّ بعد ⇦ يبقى في شاشة البداية.
      //
      // ثانيةٌ أو ثانيتان هنا خيرٌ من شاشةٍ تُبنى بلون الرحالة ثم لا تتلوّن:
      // فروع الهيكل محفوظة بـ `GlobalKey` في `go_router`، فما بُني مرّة لا
      // يُعاد بناؤه. وللانتظار سقفٌ في `BrandingController._gate`.
      if (!brandSettled) return loc == '/splash' ? null : '/splash';

      // مسجّل دخول — لا يبقى في مسار المصادقة.
      if (inAuthFlow) return '/';

      // «نقاط البيع» للوكيل الرئيسي وحده؛ الخادم يرد 403 لغيره. صارت شاشةً
      // تُدفع لا تبويباً، والحارس باقٍ: الرابط قد يُفتح بلا مرور بالحساب.
      if (loc == '/pos' && auth.user?.isMainAgent != true) return '/';

      return null;
    },
    routes: [
      // ── مسار الموظف ────────────────────────────────────────────────
      GoRoute(
        path: '/employee/activate',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeActivationScreen(),
      ),
      GoRoute(
        path: '/employee/home',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeHomeScreen(),
      ),
      GoRoute(
        path: '/employee/transfers',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeTransfersScreen(),
      ),
      GoRoute(
        path: '/employee/cashbox',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeCashboxScreen(),
      ),
      GoRoute(
        path: '/employee/shift/start',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const StartShiftScreen(),
      ),
      GoRoute(
        path: '/employee/shift/close',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const CloseShiftScreen(),
      ),

      GoRoute(path: '/splash', builder: (_, _) => const SplashScreen()),
      GoRoute(path: '/onboarding', builder: (_, _) => const OnboardingScreen()),
      GoRoute(path: '/phone', builder: (_, _) => const PhoneScreen()),
      GoRoute(
        path: '/otp',
        builder: (_, s) => OtpScreen(phone: s.extra as String? ?? ''),
      ),

      // شاشات تُفتح فوق الهيكل، بزر رجوع.
      GoRoute(
        path: '/statement',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const StatementScreen(),
      ),
      // «الحوالات الواردة» كانت تبويباً في الشريط فصارت شاشةً تُدفع — من زرّ
      // «تسليم» في الرئيسية، ومن «التقارير». وكونُها مدفوعةً على الجذر يعني
      // أن الرجوع يعيد الوكيل إلى حيث كان بدل أن يقفز به إلى تبويب آخر.
      GoRoute(
        path: '/transfers',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const TransfersScreen(),
      ),

      // «نقاط البيع» خرجت من شريط التبويبات (قرار المالك، 3 سبتمبر 2026):
      // مدخلها في تبويب الحساب، وتبويبٌ ثانٍ لها تكرار. فصار الشريط ثلاثة.
      GoRoute(
        path: '/pos',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const PosScreen(),
      ),
      GoRoute(
        path: '/favorites',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const FavoritesScreen(),
      ),
      GoRoute(
        path: '/limits',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const LimitsScreen(),
      ),
      GoRoute(
        path: '/terms',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const TermsScreen(),
      ),
      GoRoute(
        path: '/security',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const SecurityScreen(),
      ),
      GoRoute(
        path: '/branding',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const BrandingScreen(),
      ),

      // إدارة الموظفين — للحساب الرئيسي، والخادم يرفض (403) لغيره.
      GoRoute(
        path: '/employees',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeesScreen(),
      ),
      GoRoute(
        path: '/employees/reports',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeReportsScreen(),
      ),
      GoRoute(
        path: '/employees/devices',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeDevicesScreen(),
      ),
      GoRoute(
        // الموظف يُمرَّر في `extra`: صلاحياته الحالية معروضة سلفاً في القائمة،
        // فجلبها مرّة ثانية طلبٌ بلا فائدة.
        path: '/employees/:id/permissions',
        parentNavigatorKey: _rootKey,
        builder: (_, s) =>
            EmployeePermissionsScreen(employee: s.extra as Employee),
      ),

      // مسار إنشاء الحوالة — فوق الهيكل، خارج التبويبات.
      // extra قد يحمل FavoriteCustomer حين يأتي الوكيل من المفضّلة،
      // فيُملأ اسم المستفيد وهاتفه سلفاً.
      GoRoute(
        path: '/send/internal',
        parentNavigatorKey: _rootKey,
        builder: (_, s) =>
            SendInternalScreen(prefill: s.extra as FavoriteCustomer?),
      ),
      GoRoute(
        path: '/send/internal/review',
        parentNavigatorKey: _rootKey,
        builder: (_, s) =>
            ReviewTransferScreen(draft: s.extra as TransferDraft),
      ),
      GoRoute(
        path: '/send/internal/done',
        parentNavigatorKey: _rootKey,
        builder: (_, s) =>
            TransferDoneScreen(transfer: s.extra as CreatedTransfer),
      ),

      GoRoute(
        path: '/send/external',
        parentNavigatorKey: _rootKey,
        builder: (_, s) =>
            SendExternalScreen(prefill: s.extra as FavoriteCustomer?),
      ),
      GoRoute(
        path: '/send/external/done',
        parentNavigatorKey: _rootKey,
        builder: (_, s) => ExternalDoneScreen(args: s.extra),
      ),

      GoRoute(
        path: '/send/accounts',
        parentNavigatorKey: _rootKey,
        builder: (_, s) =>
            SendAccountsScreen(prefill: s.extra as FavoriteCustomer?),
      ),
      GoRoute(
        path: '/send/accounts/review',
        parentNavigatorKey: _rootKey,
        builder: (_, s) =>
            ReviewAccountsScreen(draft: s.extra as AccountsDraft),
      ),
      GoRoute(
        path: '/send/accounts/done',
        parentNavigatorKey: _rootKey,
        builder: (_, s) =>
            AccountsDoneScreen(transfer: s.extra as AccountsTransfer),
      ),

      StatefulShellRoute.indexedStack(
        builder: (_, _, shell) => AppShell(navigationShell: shell),
        branches: [
          StatefulShellBranch(routes: [
            GoRoute(path: '/', builder: (_, _) => const HomeScreen()),
          ]),
          // «التقارير» حلّ محلّ «الحوالات» في الشريط (قرار المالك، 3 سبتمبر
          // 2026): التبويب القديم كان يفتح ما يفتحه زرّ «تسليم» في الرئيسية.
          // و«الحوالات الواردة» انتقلت إلى مسارٍ مدفوع أعلاه — لم تُحذف.
          StatefulShellBranch(routes: [
            GoRoute(path: '/reports', builder: (_, _) => const ReportsScreen()),
          ]),
          // «الدردشة» تبويبٌ كامل بين التقارير والحساب (أمر المالك، 5 سبتمبر
          // 2026): ستحمل أقساماً وتفاصيل داخلها، وشاشةٌ مدفوعة من زرٍّ في
          // الترويسة لا تتّسع لذلك — الرجوع منها يخرج من القسم كلّه.
          StatefulShellBranch(routes: [
            GoRoute(path: '/chat', builder: (_, _) => const ChatThreadsScreen()),
          ]),
          StatefulShellBranch(routes: [
            GoRoute(path: '/account', builder: (_, _) => const AccountScreen()),
          ]),
        ],
      ),
    ],
  );
});

class _AuthListenable extends ChangeNotifier {
  _AuthListenable(Ref ref) {
    ref.listen(authControllerProvider, (_, _) => notifyListeners());
    // جلسة الموظف تُحرّك الراوتر كما تُحرّكه جلسة الوكيل: بغير هذا يبقى
    // الموظف على شاشة التفعيل بعد نجاحها حتى ينقر شيئاً.
    ref.listen(employeeAuthProvider, (_, _) => notifyListeners());
  }
}
