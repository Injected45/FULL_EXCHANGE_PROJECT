import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import 'features/account/account_screen.dart';
import 'features/account/security_screen.dart';
import 'features/branding/branding_controller.dart';
import 'features/branding/branding_screen.dart';
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
import 'features/transfers/transfers_screen.dart';

final _rootKey = GlobalKey<NavigatorState>();

final routerProvider = Provider<GoRouter>((ref) {
  final auth = ref.watch(authControllerProvider);

  // هوية الشركة تُقرأ هنا لا لتُعرض، بل لتُؤخَّر شاشاتُ ما بعد الدخول حتى
  // تستقرّ — انظر التعليق على `BrandingState.settled`.
  final brandSettled =
      ref.watch(brandingControllerProvider.select((s) => s.settled));

  return GoRouter(
    navigatorKey: _rootKey,
    initialLocation: '/',
    refreshListenable: _AuthListenable(ref),
    redirect: (context, state) {
      final loc = state.matchedLocation;

      // لم تُقرأ الحالة من التخزين بعد.
      if (auth.status == AuthStatus.unknown) {
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

      // تبويب نقاط البيع للوكيل الرئيسي وحده؛ الخادم يرد 403 لغيره.
      if (loc == '/pos' && auth.user?.isMainAgent != true) return '/';

      return null;
    },
    routes: [
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
          StatefulShellBranch(routes: [
            GoRoute(path: '/transfers', builder: (_, _) => const TransfersScreen()),
          ]),
          StatefulShellBranch(routes: [
            GoRoute(path: '/pos', builder: (_, _) => const PosScreen()),
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
  }
}
