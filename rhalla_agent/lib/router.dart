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
import 'features/employees/approvals_screen.dart';
import 'features/employees/employees_screen.dart';
import 'features/employee_app/employee_activation_screen.dart';
import 'features/employee_app/employee_approvals_screen.dart';
import 'features/employee_app/employee_extras_screens.dart';
import 'features/employee_app/employee_lookup_screens.dart';
import 'features/employee_app/employee_reports_screen.dart';
import 'features/employee_app/employee_statement_screen.dart';
import 'features/employee_app/employee_home_screen.dart';
import 'features/employee_app/employee_session.dart';
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
import 'features/reports/commissions_screen.dart';
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

/// آخرُ موضعٍ استقرّ عليه المستخدم — يُستأنف منه مُوجِّهٌ أُعيد بناؤه.
///
/// ⚠ متغيّرٌ عامّ لا حالةُ مزوّد: هو **ما ينجو من إعادة البناء**، وحالةُ
/// المزوّد تُبنى معه من جديد فلا تحمل شيئاً. ولا يُخزَّن على القرص: هذا موضعُ
/// جلسةٍ لا تفضيلٌ يُستعاد بعد إغلاق التطبيق.
String _lastLocation = '/';

/// المساراتُ التي يشاركها الموظف الوكيلَ — إنشاءُ الحوالة وحده.
///
/// ⚠ قائمةٌ صريحةٌ لا بادئةٌ مفتوحة: `startsWith('/send')` كانت ستفتح معها
/// الحوالة الخارجية والتحويل بين الحسابات، ولم يُؤذَن بواحدٍ منهما.
///
/// ⚠ **ومشروطةٌ بالصلاحية لا بالدخول وحده.**
///
/// إخفاءُ البطاقة من الشاشة لا يُغلق المسار: `context.push('/send/internal')`
/// من أي موضع، أو رابطٌ عميق، أو زرُّ رجوعٍ إلى شاشةٍ بقيت في السجلّ بعد
/// سحب الصلاحية — كلُّها تبلغ الشاشة. ونصُّ البند صريح: «ولا يستطيع الوصول
/// إليه عن طريق API أو Deep Link أو أي وسيلة أخرى».
///
/// والخادمُ يبقى الحارسَ الأخير (403 عند أوّل نداء)، لكنّ شاشةً تُفتح ثم
/// تسقط عند الإرسال تُعلّم الموظف أن التطبيق معطوب لا أنه غير مصرَّح.
bool _sharedWithEmployee(String loc, bool canCreate) {
  final shared = loc == '/send/internal' ||
      loc == '/send/internal/review' ||
      loc == '/send/internal/done';

  return shared && canCreate;
}

/*
 * ══════════════════════════════════════════════════════════════════════════
 *  ⚠⚠ لا يُراقَب هنا إلّا ما يقرؤه `redirect` — وإلّا انهار المُوجِّه دورياً
 * ══════════════════════════════════════════════════════════════════════════
 *
 * هذا المزوّد **يبني `GoRouter` نفسَه**. وكلُّ إعادة بناءٍ له تُنشئ مُوجِّهاً
 * جديداً، وبه كومةَ تنقّلٍ جديدة تبدأ من `initialLocation` — فتُغلق كلُّ شاشةٍ
 * مدفوعة في وجه من يستعملها.
 *
 * ── العطبُ الذي كشفه المالك (10 سبتمبر 2026) ─────────────────────────────
 *
 * «في تطبيق الموظف الصفحات غير مستقرّة… أفتح إنشاء حوالة وأبدأ أكتب البيانات
 * تُقفل ولا تدعني أُكمل».
 *
 * والسبب أنّ السطر كان `ref.watch(employeeAuthProvider)` — الحالةَ كاملةً.
 * ونبضُ `me` كلّ اثنتي عشرة ثانية يُسند حالةً **جديدةً** في كل مرّة، ولو لم
 * يتغيّر فيها حرف. فيُعاد بناءُ المُوجِّه كلّ اثنتي عشرة ثانية، وتُغلق شاشةُ
 * إنشاء الحوالة بينما الموظف يكتب فيها بيانات زبونٍ واقفٍ أمامه.
 *
 * ── والعلاجُ طبقتان، وكلتاهما لازمة ──────────────────────────────────────
 *
 * ١) **`select` هنا**: لا يُراقَب إلّا ما يقرؤه `redirect` فعلاً — أربعُ قيمٍ
 *    بدائية. فتغيُّرُ أيّ شيءٍ آخر في الجلسة (`paused`، وردية، اسمُ نقطة بيع)
 *    لا يمسّ المُوجِّه أصلاً.
 *
 * ٢) **مساواةٌ بالقيمة في `EmployeeAuthState`** مع امتناعِ الإسناد حين لا
 *    تتغيّر: فنبضةٌ لم تُغيّر شيئاً لا تُوقظ أحداً — لا المُوجِّه ولا الشاشات.
 *
 * ⚠ وإعادةُ تقييم `redirect` **لا تحتاج إعادةَ بناءٍ للمُوجِّه**: يتكفّل بها
 * `refreshListenable` أدناه. فما فُقد بهذا التغيير: لا شيء.
 */
final routerProvider = Provider<GoRouter>((ref) {
  final authStatus = ref.watch(authControllerProvider.select((s) => s.status));
  final onboarded = ref.watch(authControllerProvider.select((s) => s.onboarded));
  final isMainAgent =
      ref.watch(authControllerProvider.select((s) => s.user?.isMainAgent));

  // هوية الشركة تُقرأ هنا لا لتُعرض، بل لتُؤخَّر شاشاتُ ما بعد الدخول حتى
  // تستقرّ — انظر التعليق على `BrandingState.settled`.
  final brandSettled =
      ref.watch(brandingControllerProvider.select((s) => s.settled));

  /*
   * ══════════════════════════════════════════════════════════════════════
   *  ⚠⚠ وإعادةُ بناءِ المُوجِّه عند تغيّر الهوية — **مقصودةٌ هنا وحدها**
   * ══════════════════════════════════════════════════════════════════════
   *
   * بلاغُ المالك (10 سبتمبر 2026): «غيّرتُ الألوان، فتغيّرت أجزاءٌ من التطبيق
   * وأجزاءٌ بقيت بالثيم السابق».
   *
   * وهو عطبٌ موصوفٌ في هذا الملفّ منذ البداية: `go_router` يحفظ فروعَ
   * `StatefulShellRoute` بـ`GlobalKey`، فما بُني منها مرّةً **يُنقَل ولا يُعاد
   * بناؤه**. وألوانُ `R` قيمٌ ساكنة تُقرأ لحظةَ البناء — فالفرعُ الذي زاره
   * الوكيل قبل الحفظ يبقى بألوانه القديمة إلى نهاية الجلسة. و`KeyedSubtree`
   * في `main.dart` يُعيد بناء ما فوق الـ`Navigator` ولا يبلغ تلك الفروع.
   *
   * ومُوجِّهٌ جديد يعني فروعاً جديدة بمفاتيح جديدة — أي إعادةَ بناءٍ حقيقية
   * لكلّ شاشةٍ محفوظة. وهو **الطريقُ الوحيد** لإعادة تلوينها بلا خروجٍ ودخول.
   *
   * ⚠ ولا يتناقض مع سببِ `select` أعلاه: ذاك كان يقع **كلَّ اثنتي عشرة ثانية**
   * بلا سبب، وهذا يقع عند تغيّر الهوية فقط — دخولٌ، أو حفظٌ، أو استعادة. أي
   * مرّاتٍ معدودة في العمر، وبطلبٍ صريحٍ من المستخدم في كلّ مرّة.
   *
   * ⚠ والموضعُ يُحفظ ويُستأنف: بغيره يُقذف الوكيلُ إلى الرئيسية لحظةَ حفظه
   * الثيمَ من شاشة الإعدادات، فيقرأ الحفظَ الناجح خروجاً من الشاشة.
   */
  // ⚠ القيمةُ نفسُها لا تُقرأ — **المراقبةُ هي الأثر المقصود**: تغيُّرُها يُعيد
  // بناء هذا المزوّد، وإعادةُ بنائه هي إعادةُ بناء المُوجِّه. و`_` اسمٌ لا
  // يرتبط في دارت، فلا متغيّرَ مهملاً ولا سطرَ تجاهلٍ للمحلّل.
  final _ = ref.watch(brandingControllerProvider.select((s) => s.epoch));

  // وضع الموظف — مسارٌ مستقلّ تماماً عن مسار الوكيل.
  //
  // ⚠ الفصل بين السياقين شرط أمني (بند 22): جلسة موظف لا تُرقّى إلى مسؤول.
  // ولذلك لا تشارك الشاشتان تبويباً ولا هيكلاً، والراوتر يحسم أيّهما قبل كل
  // شيء آخر.
  final empStatus =
      ref.watch(employeeAuthProvider.select((s) => s.status));
  final empCanCreate = ref.watch(employeeAuthProvider
      .select((s) => s.profile?.can('CREATE_TRANSFER') ?? false));
  final employeeIn = empStatus == EmpSessionStatus.signedIn;

  return GoRouter(
    navigatorKey: _rootKey,
    // يُغلق لوحة المفاتيح عند كل انتقال — انظر [KeyboardDismisser].
    observers: [KeyboardDismisser()],
    // ⚠ الموضعُ المحفوظ لا الجذر: مُوجِّهٌ يُعاد بناؤه لتغيّر الهوية يجب أن
    // يُعيد الوكيلَ إلى حيث كان — لا إلى الرئيسية. و`_lastLocation` يُحدَّث
    // في `redirect` أدناه، وهو أوّلُ ما يُنادى عند كل تنقّل.
    //
    // ولا أثرَ له في الحالة العادية: يبدأ `/` ولا يتغيّر قبل أوّل تنقّل.
    initialLocation: _lastLocation,
    refreshListenable: _AuthListenable(ref),
    redirect: (context, state) {
      final loc = state.matchedLocation;
      final inEmployeeArea = loc.startsWith('/employee/');

      // ⚠ يُسجَّل قبل أيّ قرار: ما يُوجَّه عنه لا يُحفظ، وإنّما ما استقرّ
      // عليه المستخدم فعلاً — والتوجيهُ التالي يمرّ من هنا فيُصحّحه.
      _lastLocation = loc;

      /*
       * الموظف الداخل يبقى في مساره ولا يرى شاشة وكيل — إلّا ثلاثاً
       * يشاركها الوكيلَ عمداً.
       *
       * ⚠ أمرُ المالك (7 سبتمبر 2026): «الموظف ينفّذ الحوالة وكأنه الوكيل
       * — واجهةٌ من وكيل، لا مستقلٌّ استقلاليةً تامّة». فشاشةُ الإنشاء
       * ومراجعتُها ونجاحُها **هي هي**؛ ونسخةٌ ثانية منها للموظف كانت
       * ستفترق عن الأولى عند أوّل تعديل، فيرى الاثنان نموذجين مختلفين
       * لعمليةٍ واحدة.
       *
       * ⚠ وهو استثناءٌ مكتوبٌ لا ثغرة: الحارسُ في الخادم
       * (`employee:CREATE_TRANSFER`)، والمستودعُ يبدّل المسار إلى نظيره
       * تحت `device/employee/` لأن رمز الموظف لا يفتح مسارات الوكيل. فمن
       * بلغ هذه الشاشة بلا صلاحية يصطدم بـ403 عند أوّل نداء.
       */
      if (employeeIn) {
        return (inEmployeeArea && loc != '/employee/activate') ||
                _sharedWithEmployee(loc, empCanCreate)
            ? null
            : '/employee/home';
      }

      // شاشة التفعيل مفتوحة قبل الدخول — وهي المدخل الوحيد لمسار الموظف.
      if (loc == '/employee/activate') return null;

      // موظفٌ خرج أو أُلغي جهازه: لا يبقى في شاشات الموظف.
      if (inEmployeeArea) return '/phone';

      // لم تُقرأ الحالة من التخزين بعد.
      if (authStatus == AuthStatus.unknown ||
          empStatus == EmpSessionStatus.unknown) {
        return loc == '/splash' ? null : '/splash';
      }

      final signedIn = authStatus == AuthStatus.signedIn;
      final inAuthFlow = loc == '/phone' ||
          loc == '/otp' ||
          loc == '/onboarding' ||
          loc == '/splash';

      if (!signedIn) {
        if (!onboarded) {
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
      if (loc == '/pos' && isMainAgent != true) return '/';

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
        // قسمُ تقارير الموظف عن نفسِه — لا تقارير الوكيل عن موظفيه.
        path: '/employee/reports',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeOwnReportsScreen(),
      ),
      GoRoute(
        path: '/employee/balances',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeBalancesScreen(),
      ),
      GoRoute(
        path: '/employee/favorites',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeFavoritesScreen(),
      ),
      GoRoute(
        path: '/employee/pos-transfers',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeePosTransfersScreen(),
      ),
      GoRoute(
        path: '/employee/search',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeSearchScreen(),
      ),
      GoRoute(
        // كشفُ حوالاته — للجرد على نفسه.
        path: '/employee/statement',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeStatementScreen(),
      ),
      GoRoute(
        // «طلباتي» — نتيجةُ ما أرسله الموظف إلى وكيله.
        path: '/employee/approvals',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const EmployeeApprovalsScreen(),
      ),
      GoRoute(
        path: '/employee/transfers',
        parentNavigatorKey: _rootKey,
        // ⚠ شاشةُ الوكيل نفسُها بعين الموظف — أمرُ المالك (10 سبتمبر 2026):
        // «تبويب مخصّص للحوالات نفس تبويب الوكيل … ونفسها في كل شيء».
        // والشرحُ الكامل في رأس `TransfersScreen`.
        builder: (_, _) => const TransfersScreen(asEmployee: true),
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
        // «عمولاتي» — فُصلت من «السقوف والعمولات» إلى التقارير (قرار المالك).
        path: '/commissions',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const CommissionsScreen(),
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
        // طلباتُ الموافقة — خارج التبويبات: لا تُفتح إلّا من الجرس أو
        // من شاشة الموظفين، وهي شاشةُ قرارٍ لا شاشةَ تصفّح.
        path: '/employees/approvals',
        parentNavigatorKey: _rootKey,
        builder: (_, _) => const ApprovalsScreen(),
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
