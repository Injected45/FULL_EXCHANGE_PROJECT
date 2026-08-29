import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import 'core/theme/app_theme.dart';
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
        return Directionality(
          textDirection: TextDirection.rtl,
          child: AmbientBackground(child: child ?? const SizedBox.shrink()),
        );
      },
    );
  }
}
