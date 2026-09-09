import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/net/api_client.dart';
import 'package:rhalla_agent/core/storage/secure_store.dart';
import 'package:rhalla_agent/features/alerts/incoming_alerts.dart';
import 'package:rhalla_agent/features/alerts/incoming_toast.dart';

/// الشريط المنسدل: ينسدل على الوصول، ويرتفع بعد خمس ثوانٍ، **ويبقى في
/// الشجرة** بعد أن يرتفع.
///
/// الأخيرةُ هي التي تستحق اختباراً: الشريط ثابتٌ فوق الـ Navigator يُطوى ولا
/// يُهدَم، وأيّ مكوّنٍ يشترط رفعَه من الشجرة بعد طيّه (`Dismissible` مثلاً)
/// يُسقط التطبيق كلَّه — لا الشريطَ وحدَه — وقد سبق أن كُتب به.

/// يقود الحالة يدوياً بدل الشبكة: المختبَر هنا العرضُ لا الجلب.
///
/// يرث المتحكّم الحقيقي لأن المزوّد يشترط نوعه — ولا يُستدعى `start`،
/// فلا مؤقّتَ ولا طلب.
class _Fake extends IncomingAlertsController {
  _Fake() : super(ApiClient(SecureStore()), SecureStore());

  void arrive(int n) =>
      state = IncomingAlerts(unseen: n, ping: state.ping + 1, arrived: n);

  /// فتحُ القائمة: يُصفّر العدّاد ويُبقي النبضة.
  void openedList() =>
      state = IncomingAlerts(ping: state.ping, arrived: state.arrived);
}

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  /*
   * ⚠ يُجاب نداءُ التخزين الآمن بلا شيء.
   *
   * `AuthController._bootstrap` يُنشأ تلقائياً في هذه الشجرة، ويلفّ
   * قراءاتِه بمهلةٍ حتى لا يتجمّد التطبيق على شاشة البداية إن تعلّق
   * التخزين (وقع على جهازٍ حقيقيّ، 9 سبتمبر 2026).
   *
   * وبلا مُجيبٍ للقناة لا يُجاب النداءُ في زمن الاختبار المزيّف أبداً،
   * فيبقى الحارسُ معلّقاً ويُسقط الفحصَ بـ«Pending timers» — لسببٍ لا
   * علاقةَ له بالشريط المنسدل. والإجابةُ بلا شيء تعني «لا جلسة»، وهي
   * الحالةُ الصحيحة لاختبارٍ لا يُسجّل دخولاً.
   */
  setUp(() {
    TestDefaultBinaryMessengerBinding.instance.defaultBinaryMessenger
        .setMockMethodCallHandler(
      const MethodChannel('plugins.it_nomads.com/flutter_secure_storage'),
      (_) async => null,
    );
  });

  late _Fake fake;

  Future<void> mount(WidgetTester tester) async {
    fake = _Fake();
    await tester.pumpWidget(
      ProviderScope(
        overrides: [incomingAlertsProvider.overrideWith((_) => fake)],
        child: const MaterialApp(
          locale: Locale('ar'),
          home: Directionality(
            textDirection: TextDirection.rtl,
            child: Stack(children: [SizedBox.expand(), IncomingToast()]),
          ),
        ),
      ),
    );

    /*
     * ⚠ يُصرَّف مؤقّتُ حارسِ الإقلاع.
     *
     * `AuthController._bootstrap` يلفّ قراءاتِه بمهلةٍ حتى لا يتجمّد
     * التطبيق على شاشة البداية إن تعلّق التخزين — وهو ما وقع على جهازٍ
     * حقيقيّ. ونداءُ الإضافة لا يُجاب في زمن الاختبار المزيّف، فيبقى
     * المؤقّت معلّقاً عند التفكيك ويُسقط الفحصَ بلا علاقةٍ بما يُفحص.
     */
    await tester.pump(const Duration(seconds: 9));
  }

  testWidgets('لا شيء قبل وصول شيء', (tester) async {
    await mount(tester);
    expect(find.textContaining('حوالة جديدة'), findsNothing);
  });

  testWidgets('ينسدل على الوصول ثم يرتفع بعد خمس ثوانٍ', (tester) async {
    await mount(tester);

    fake.arrive(1);
    await tester.pump();
    await tester.pump(const Duration(milliseconds: 300));

    expect(find.text('لديك حوالة جديدة'), findsOneWidget);

    // ينتظر الخمس ثم يكمل حركة الارتفاع.
    await tester.pump(const Duration(seconds: 5));
    await tester.pump(const Duration(milliseconds: 300));

    // ⚠ الودجتُ نفسُها باقية في الشجرة — ارتفعت ولم تُهدَم. ولو كانت
    // تشترط الهدم بعد الطيّ لسقط الاختبار هنا بتأكيدٍ لا بفارق نصّ.
    expect(find.byType(IncomingToast), findsOneWidget);

    // وبطاقتُه رُفعت، فلا شعارَ يُجلب ولا نصَّ يقرؤه قارئُ الشاشة.
    expect(find.textContaining('حوالة جديدة'), findsNothing);
  });

  testWidgets('العدد يُجمع في النصّ', (tester) async {
    await mount(tester);

    fake.arrive(3);
    await tester.pump();
    await tester.pump(const Duration(milliseconds: 300));

    expect(find.text('لديك 3 حوالات جديدة'), findsOneWidget);
  });

  testWidgets('فتحُ القائمة لا ينسدل به شريط', (tester) async {
    await mount(tester);

    fake.openedList();
    await tester.pump();
    await tester.pump(const Duration(milliseconds: 300));

    expect(find.textContaining('حوالة جديدة'), findsNothing);
  });
}
