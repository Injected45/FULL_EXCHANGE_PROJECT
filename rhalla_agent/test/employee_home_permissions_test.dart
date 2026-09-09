import 'dart:convert';
import 'dart:typed_data';

import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:go_router/go_router.dart';
import 'package:rhalla_agent/core/net/api_client.dart';
import 'package:rhalla_agent/core/storage/secure_store.dart';
import 'package:rhalla_agent/features/employee_app/employee_home_screen.dart';

/*
 * ══════════════════════════════════════════════════════════════════════════
 *  شرطُ المالك (9 سبتمبر 2026): يفتح التطبيقُ بلا صلاحيةٍ واحدة،
 *  وكلُّ صلاحيةٍ يمنحها الوكيل **تظهر**.
 * ══════════════════════════════════════════════════════════════════════════
 *
 * والشقُّ الثاني هو الذي انكسر: «بلا صلاحيات» كانت تُحسب من خمسة مفاتيح
 * مكتوبةٍ بأسمائها، والشاشةُ تعرض ثلاثَ عشرةَ بلاطة. فموظفٌ مُنح «البحث برقم
 * الحوالة» وحدَها كان يرى لافتة «لم تُمنح صلاحيات بعد» **فوق بلاطةٍ تعمل**،
 * وموظفٌ مُنح «تسجيل التسليم» وحدَه كان يرى شاشةً فارغةً بلا لافتة.
 *
 * وهذا الاختبار يمرّ على **كلّ** مفتاحٍ يعرضه الكتالوج اليوم، واحداً واحداً.
 */

/// خادمٌ مزيّف يُجيب `employee/me` بالصلاحيات المطلوبة.
///
/// ⚠ **يُسلَك المسارُ الحقيقيّ كاملاً** — `_restore` تسأل الخادم ثم تبني
/// `EmployeeProfile.fromJson`. وحقنُ الحالة مباشرةً كان أسهل، لكنه يتخطّى
/// القراءةَ التي قد تكون هي الكاسرة.
class _StubAdapter implements HttpClientAdapter {
  _StubAdapter(this.permissions, this.shift);

  final List<String> permissions;
  final Map<String, dynamic>? shift;

  @override
  void close({bool force = false}) {}

  @override
  Future<ResponseBody> fetch(
      RequestOptions options, Stream<Uint8List>? _, Future<void>? _) async {
    final data = _profileJson(permissions, shift);
    return ResponseBody.fromString(
      jsonEncode({'success': true, 'message': '', 'key': '', 'data': data}),
      200,
      headers: {
        Headers.contentTypeHeader: [Headers.jsonContentType],
      },
    );
  }
}

/// ملفُّ الموظف بالشكل الذي يعيده `employee/me` ويقرؤه `fromJson`.
Map<String, dynamic> _profileJson(
        List<String> permissions, Map<String, dynamic>? shift) =>
    {
      'employee': {'id': 1, 'name': 'موظف اختبار', 'phone': '910000000'},
      'permissions': permissions,
      'points_of_sale': const [],
      'open_shift': ?shift,
    };

class _FakeStore extends SecureStore {
  _FakeStore(this.permissions, this.shift);

  final List<String> permissions;
  final Map<String, dynamic>? shift;

  /*
   * ⚠ **كلُّ قراءةٍ تُغطّى، لا قراءاتُ الموظف وحدَها.**
   *
   * شجرةُ التطبيق تُنشئ متحكّمَ جلسة الوكيل أيضاً، وهو يقرأ `onboarded`
   * من التخزين الآمن — وذلك نداءُ إضافةٍ حقيقيّة لا وجودَ لها في
   * الاختبار، فيُرمى استثناءٌ يُسقط الفحصَ لسببٍ لا علاقةَ له بالصلاحيات.
   * وتغطيةُ ما احتجناه وحدَه تُخفي ذلك حتى يطول الفحصُ فيظهر.
   */
  @override
  Future<String?> readToken() async => null;

  @override
  Future<bool> readOnboarded() async => true;

  @override
  Future<Map<String, dynamic>?> readUser() async => null;

  @override
  Future<Set<int>> readSeenIncoming() async => <int>{};

  @override
  Future<String> deviceId() async => 'جهاز-اختبار';

  @override
  Future<String?> readEmployeeToken() async => 'رمز';

  @override
  Future<void> writeEmployee(Map<String, dynamic> j) async {}

  /*
   * ⚠ **الجلسةُ المحفوظة تُعاد أيضاً** لا الشبكةُ وحدَها.
   *
   * `_restore` تسأل الخادم، فإن أخفق النداءُ لأيّ سبب — وفي الاختبار
   * تُخفق إضافاتُ الجهاز لغياب المنصّة — رجعت إلى المحفوظ. وتركُه
   * فارغاً يجعل الشاشةَ دوّامةَ تحميلٍ أبداً، فيمرّ كلُّ فحصٍ يتوقّع
   * الغياب **لأن الشاشة فارغة** لا لأن الصلاحية محجوبة.
   */
  @override
  Future<Map<String, dynamic>?> readEmployee() async =>
      _profileJson(permissions, shift);
}

/// الشاشةُ مبنيّةً لموظفٍ يملك هذه الصلاحيات وحدَها.
Widget _app(List<String> permissions, {Map<String, dynamic>? shift}) {
  final store = _FakeStore(permissions, shift);
  final api = ApiClient(store)
    ..raw.httpClientAdapter = _StubAdapter(permissions, shift);

  final router = GoRouter(
    routes: [
      GoRoute(path: '/', builder: (_, _) => const EmployeeHomeScreen()),
    ],
  );

  return ProviderScope(
    overrides: [
      apiClientProvider.overrideWithValue(api),
      secureStoreProvider.overrideWithValue(store),
    ],
    child: MaterialApp.router(
      locale: const Locale('ar'),
      routerConfig: router,
      builder: (context, child) => Directionality(
        textDirection: TextDirection.rtl,
        child: child ?? const SizedBox.shrink(),
      ),
    ),
  );
}

/// ما تعرضه الشاشة من بلاطات — نصوصُ العناوين.
Finder _tiles() => find.byType(InkWell);

/// يُركّب الشاشة ثم ينتظر استعادةَ الجلسة.
///
/// ⚠ **التركيبُ داخل `runAsync` لا قبلَه.** استعادةُ الجلسة تمرّ بـ dio،
/// وهو زمنٌ حقيقيّ لا نبضاتُ إطارات. ومهمّةٌ تبدأ في زمن الاختبار المزيّف
/// **لا تتقدّم** داخل `runAsync` بعدها — فتُقاس الشاشةُ وهي ما زالت
/// دوّامةَ تحميل، ويبدو كلُّ فحصٍ فاشلاً لسببٍ لا علاقةَ له بالصلاحيات.
Future<void> _mount(WidgetTester t, List<String> permissions,
    {Map<String, dynamic>? shift}) async {
  await t.pumpWidget(_app(permissions, shift: shift));

  /*
   * ⚠ **يُنتظر حتى تُبنى الشاشة فعلاً، لا مدّةً ثابتة.**
   *
   * مهلةٌ ثابتة تنجح منفردةً وتسقط تحت حمل الحزمة كاملة — واختبارٌ
   * متذبذب يُعلّم إعادةَ التشغيل بدل الفحص، فيضيع سقوطُه الحقيقيّ.
   * فيُستطلَع اختفاءُ دوّامة التحميل بحدٍّ أعلى للأمان.
   */
  for (var i = 0; i < 60; i++) {
    // الترويسةُ تُبنى أوّلَ ما يصل الملفّ — فاسمُ الموظف علامةُ الجاهزية.
    if (find.text('موظف اختبار').evaluate().isNotEmpty) break;
    await t.runAsync(() => Future<void>.delayed(
        const Duration(milliseconds: 30)));
    await t.pump();
  }
  await t.pump(const Duration(milliseconds: 50));
}

void main() {
  const noPermsBanner = 'لم تُمنح صلاحيات بعد';

  /*
   * كلُّ مفتاحٍ في الكتالوج اليوم. مكتوبٌ هنا **للاختبار وحدَه** — والشاشةُ
   * نفسُها لا تعرف إلّا ما يصلها من الخادم.
   *
   * ⚠ ومصدرُ هذه القائمة `EmployeePermissions::CATALOG`؛ فمفتاحٌ يُضاف هناك
   * ولا يُضاف هنا يفوته هذا الاختبار — و`employee_permissions_matrix.php`
   * في الخادم هو ما يحرس الاكتمال (23 من 23).
   */
  const catalog = <String>[
    'VIEW_INCOMING_TRANSFERS',
    'DELIVER_TRANSFER',
    'CREATE_TRANSFER',
    'VIEW_OWN_TRANSFERS',
    'VIEW_POS_TRANSFERS',
    'SEARCH_TRANSFER',
    'VIEW_OWN_CASHBOX',
    'CASHBOX_ENTRY',
    'START_SHIFT',
    'CLOSE_SHIFT',
    'VIEW_AGENT_TOTAL_BALANCE',
    'VIEW_FINANCIAL_SUMMARY',
    'REPORTS_VIEW',
    'REPORT_DAILY_TRANSFERS',
    'REPORT_DELIVERED_TRANSFERS',
    'REPORT_PENDING_TRANSFERS',
    'REPORT_EMPLOYEE_CASHBOX',
    'REPORT_POINT_OF_SALE',
    'REPORT_AGENT_BALANCE',
    'REPORT_AUDIT',
    'VIEW_FAVORITES',
    'MANAGE_FAVORITES',
    'CHAT_WITH_AGENT',
  ];

  /*
   * مفاتيحُ **لا شاشةَ لها في الرئيسية عمداً** — وهي التقاريرُ المفردة
   * وإدارةُ المفضّلة وإقفالُ الوردية.
   *
   * ⚠ وليست خللاً: بابُها داخل بلاطةٍ أخرى. «تقرير حوالات اليوم» يُفتح من
   * بلاطة التقارير، و«إدارة المفضّلة» من بلاطة المستفيدين، و«إقفال وردية»
   * من بطاقة الوردية المفتوحة. ومن مُنحها وحدَها يرى اللافتة — وهو الصدق:
   * لا شيءَ يستطيع بدؤه من هنا.
   */
  const gatedInside = <String>{
    'REPORT_DAILY_TRANSFERS',
    'REPORT_DELIVERED_TRANSFERS',
    'REPORT_PENDING_TRANSFERS',
    'REPORT_EMPLOYEE_CASHBOX',
    'REPORT_POINT_OF_SALE',
    'REPORT_AGENT_BALANCE',
    'REPORT_AUDIT',
    'MANAGE_FAVORITES',
    'CLOSE_SHIFT',
  };

  testWidgets('بلا صلاحيةٍ واحدة: لافتةٌ ولا بلاطة', (tester) async {
    await _mount(tester, const []);


    expect(find.text(noPermsBanner), findsOneWidget);
    expect(find.byType(Card), findsNothing);
  });

  /*
   * ⚠ **دورةٌ واحدة على الكتالوج لا دورتان.**
   *
   * كلُّ تركيبٍ يبني الشاشة كاملةً وينتظر وصولَ الملفّ، وذلك ثوانٍ. ودورتان
   * تُضاعفان زمنَ الحزمة كلِّها بلا أن تفحصا شيئاً إضافيّاً — فالشرطان
   * يُقاسان على المشهد نفسِه.
   */
  testWidgets('⚠ كلُّ مفتاحٍ وحدَه: لا لافتةَ مع بلاطة، ولا صمتَ بلا سبب',
      (tester) async {
    for (final key in catalog) {
      await _mount(tester, [key]);

      final banner = find.text(noPermsBanner).evaluate().isNotEmpty;

      // زرُّ الخروج نفسُه `InkWell`؛ فما زاد عليه بلاطة.
      final hasTile = _tiles().evaluate().length > 1;

      expect(banner && hasTile, isFalse,
          reason: 'مع «$key»: اللافتةُ وبلاطةٌ معاً — تناقض');

      // ومفتاحٌ له بابٌ في الرئيسية يجب ألّا يقول «بلا صلاحيات».
      if (!gatedInside.contains(key)) {
        expect(banner, isFalse,
            reason: 'مُنح «$key» ومع ذلك تقول الشاشةُ إنه بلا صلاحيات');
      }
    }
  });
  testWidgets('⚠ «تسجيل التسليم» وحدَه لا يترك الشاشة فارغةً بلا شرح',
      (tester) async {
    // الحالةُ التي كانت تُنتج شاشةً بيضاء: بلاطةُ الوارد تشترط العرض.
    await _mount(tester, const ['DELIVER_TRANSFER']);

    expect(find.text(noPermsBanner), findsNothing);
    expect(find.text('الحوالات الواردة'), findsOneWidget);
    expect(find.textContaining('راجع وكيلك'), findsOneWidget,
        reason: 'يقول له ما ينقصه بدل أن يصمت');
  });

  testWidgets('⚠ و«تسجيل حركة خزينة» وحدَه كذلك', (tester) async {
    await _mount(tester, const ['CASHBOX_ENTRY']);

    expect(find.text(noPermsBanner), findsNothing);
    expect(find.text('خزينتي'), findsOneWidget);
  });

  testWidgets('و«بدء وردية» وحدَها عملٌ كسائره — لا لافتة', (tester) async {
    await _mount(tester, const ['START_SHIFT']);
    expect(find.text(noPermsBanner), findsNothing);
  });

  testWidgets('⚠ وتقريرٌ مفردٌ بلا بوّابته يقول الحقيقة: لا شيءَ هنا',
      (tester) async {
    // ليست خللاً — بابُه داخل بلاطة التقارير، وهي غيرُ ممنوحة.
    await _mount(tester, const ['REPORT_DAILY_TRANSFERS']);
    expect(find.text(noPermsBanner), findsOneWidget);
  });

  testWidgets('ومنحُ صلاحيتين يُظهر بابيهما معاً', (tester) async {
    await _mount(tester, const ['SEARCH_TRANSFER', 'CHAT_WITH_AGENT']);

    expect(find.text(noPermsBanner), findsNothing);
    expect(find.text('بحث برقم الحوالة'), findsOneWidget);
    expect(find.text('مراسلة الوكيل'), findsOneWidget);
  });

  testWidgets('⚠ ورصيدُ الوكيل لا يظهر بلا صلاحيته', (tester) async {
    await _mount(tester, const ['VIEW_FINANCIAL_SUMMARY']);

    // العنوانُ يتبع ما مُنح: ملخّصٌ لا «الأرصدة».
    expect(find.text('الملخّص المالي'), findsOneWidget);
    expect(find.textContaining('رصيد الوكيل'), findsNothing);
  });
}
