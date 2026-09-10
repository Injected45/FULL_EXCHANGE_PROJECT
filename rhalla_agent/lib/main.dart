import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import 'core/theme/app_theme.dart';
import 'features/alerts/incoming_toast.dart';
import 'features/branding/branding_controller.dart';
import 'features/employee_app/employee_freeze_gate.dart';
import 'features/security/lock_gate.dart';
import 'router.dart';
import 'ui/widgets/ambient.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  SystemChrome.setSystemUIOverlayStyle(const SystemUiOverlayStyle(
    statusBarColor: Colors.transparent,
    statusBarIconBrightness: Brightness.dark,
    statusBarBrightness: Brightness.light,
    systemNavigationBarColor: Color(0xFFEAF4F0),
    systemNavigationBarIconBrightness: Brightness.dark,
  ));
  runApp(const ProviderScope(child: RhallaAgentApp()));
}

class RhallaAgentApp extends ConsumerWidget {
  const RhallaAgentApp({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final router = ref.watch(routerProvider);

    // هوية الشركة تُسنَد إلى `R` وهي قيمٌ ساكنة، فتغييرها لا يُخطر فلاتر
    // بشيء. مراقبة هذا المزوّد هي ما يعيد بناء الشجرة بالألوان الجديدة —
    // بدونها تبقى الواجهة بلون الرحالة حتى أول تنقّل.
    // ⚠ المراقبةُ هي الأثر لا القيمة: تغيّرُ رقم الهوية يُعيد بناء هذه الشجرة
    // فتُقرأ ألوانُ `R` الجديدة فيما فوق الـ`Navigator`. وما تحته يتكفّل به
    // `routerProvider` — انظر التعليق عند `KeyedSubtree` أدناه.
    //
    // و`select` على الرقم وحدَه: بقيّةُ حقول الهوية تتغيّر بلا أن تعني شيئاً
    // لهذه الشجرة، وإعادةُ بنائها لأجلها عملٌ بلا أثر.
    final _ = ref.watch(brandingControllerProvider.select((s) => s.epoch));

    // جالب الهوية يُراقَب من هنا لأن هذا الموضع حيٌّ دائماً: الراوتر يحجز
    // شاشات ما بعد الدخول حتى تستقرّ الهوية، فلو كان الجالب داخل تلك
    // الشاشات لانتظر كلٌّ منهما الآخر. وهو يحرس نفسه بحالة الجلسة.
    ref.watch(brandingBootstrapProvider);

    return MaterialApp.router(
      title: 'رحلة — الوكيل',
      debugShowCheckedModeBanner: false,
      theme: buildTheme(),
      routerConfig: router,

      // التطبيق عربي RTL بالكامل.
      locale: const Locale('ar'),
      supportedLocales: const [Locale('ar')],
      localizationsDelegates: const [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],

      builder: (context, child) {
        // الخلفية المتحركة تُبنى فوق الـ Navigator مرة واحدة،
        // فلا تعيد الحركة البدء مع كل انتقال بين الشاشات.
        /*
         * ⚠⚠ **ولا مفتاحَ هويةٍ هنا بعد اليوم.**
         *
         * كان `key: ValueKey(brandEpoch)` — يهدم الشجرة عند تغيّر الهوية
         * ليُعاد بناؤها بالألوان الجديدة. ولم يكن يبلغ ما وُضع له: فروعُ
         * `StatefulShellRoute` محفوظةٌ بـ`GlobalKey` تحت هذا الموضع، فتُنقَل
         * ولا يُعاد بناؤها — وهو ما رآه المالك: «أجزاءٌ تغيّرت وأجزاءٌ بقيت
         * بالثيم السابق» (10 سبتمبر 2026).
         *
         * والعلاجُ الصحيح صار في `routerProvider`: يُعاد بناء المُوجِّه نفسِه
         * عند تغيّر الهوية، فتُولَد الفروعُ بمفاتيح جديدة وتُبنى بألوانها.
         *
         * ⚠ وبقاؤه مع ذلك كان خطراً لا زيادةَ احتياط: هدمُ الشجرة وإنشاءُ
         * مُوجِّهٍ جديد في الإطار نفسِه يضع `_rootKey` — وهو `GlobalKey` واحد —
         * بين شجرتين حيّتين. وهذا سبيلُ «Duplicate GlobalKey».
         *
         * وما فوق الـ`Navigator` يُعاد بناؤه على أي حال: هذا الباني يُنادى مع
         * كل إعادة بناءٍ لـ`RhallaAgentApp`، وهي تراقب الهوية.
         */
        return KeyedSubtree(
          key: const ValueKey('app-root'),
          child: Directionality(
            textDirection: TextDirection.rtl,
            // شريطُ «لديك حوالة جديدة» فوق الـ Navigator للسبب نفسِه: الوكيل
            // قد يكون في أي شاشةٍ حين تصل واردة، وشريطٌ تحت الـ Navigator
            // تغطّيه أوّلُ شاشةٍ تُدفَع. وهو صامتٌ قبل الدخول لأن نبضة
            // الوارد لا تعمل إلا من هيكل الوكيل — لا لشرطٍ مكتوبٍ هنا.
            /*
             * ⚠ **بوّابةُ القفل تلفّ كلَّ شيء** — وفوقها لا شيء إلّا ما
             * يجب أن يُرى وهي مقفلة، ولا شيءَ يجب.
             *
             * وشريطُ «لديك حوالة جديدة» **داخلها**: إشعارٌ ينسدل فوق
             * شاشةِ قفلٍ يكشف أن حوالةً وصلت لمن يمسك الهاتف ولم يُثبت
             * هويّته بعد.
             */
            child: LockGate(
              child: Stack(
                children: [
                  AmbientBackground(child: child ?? const SizedBox.shrink()),
                  const IncomingToast(),
                  // شاشةُ تجميدٍ حين يُوقف الوكيلُ الموظف — فوق كلّ شيء،
                  // وتظهر في وضع الموظف وحده.
                  const EmployeeFreezeGate(),
                ],
              ),
            ),
          ),
        );
      },
    );
  }
}
