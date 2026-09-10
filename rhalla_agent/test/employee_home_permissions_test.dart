import 'dart:convert';
import 'dart:typed_data';

import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:go_router/go_router.dart';
import 'package:rhalla_agent/core/net/api_client.dart';
import 'package:rhalla_agent/core/storage/secure_store.dart';
import 'package:rhalla_agent/features/employee_app/employee_cashbox_screen.dart';
import 'package:rhalla_agent/features/employee_app/employee_extras_screens.dart';
import 'package:rhalla_agent/features/employee_app/employee_home_screen.dart';
import 'package:rhalla_agent/features/employee_app/employee_hubs.dart';
import 'package:rhalla_agent/features/employee_app/employee_reports_screen.dart';

/*
 * ══════════════════════════════════════════════════════════════════════════
 *  شرطُ المالك (9 سبتمبر 2026): يفتح التطبيقُ بلا صلاحيةٍ واحدة،
 *  وكلُّ صلاحيةٍ يمنحها الوكيل **تظهر**.
 * ══════════════════════════════════════════════════════════════════════════
 *
 * والشقُّ الثاني هو الذي انكسر أوّل مرّة: «بلا صلاحيات» كانت تُحسب من خمسة
 * مفاتيح مكتوبةٍ بأسمائها، والشاشةُ تعرض ثلاثَ عشرةَ بلاطة. فموظفٌ مُنح
 * «البحث برقم الحوالة» وحدَها كان يرى لافتة «لم تُمنح صلاحيات بعد» **فوق
 * بلاطةٍ تعمل**، وموظفٌ مُنح «تسجيل التسليم» وحدَه كان يرى شاشةً فارغةً بلا
 * لافتة.
 *
 * ── وما تغيّر بعد إعادة الهيكلة (10 سبتمبر 2026) ─────────────────────────
 *
 * ⚠ **الشاشةُ الواحدة صارت خمسةَ تبويبات**، فلم يعد «هل تظهر البلاطة في
 * الرئيسية؟» سؤالاً صحيحاً: «مراسلة الوكيل» تبويبٌ بذاته، و«المستفيدون»
 * كذلك، و«الأرصدة» نزلت إلى التقارير.
 *
 * فالسؤالُ الآن أدقّ: **لكلّ مفتاحٍ تبويبُه، ويُفحص هناك.** وجدولُ
 * [_where] أدناه هو ذلك الإسناد، وهو نفسُه توثيقٌ لمن يقرأ: أين يجد
 * الموظفُ ما مُنحه.
 *
 * ⚠ وتركيبُ التبويب المعنيّ وحدَه لا الخمسةِ معاً: كلُّ تركيبٍ ينتظر وصولَ
 * الملفّ من الشبكة المزيّفة، وخمسةٌ لكلّ مفتاحٍ تُضاعف زمنَ الحزمة خمسَ
 * مرّات بلا أن تفحص شيئاً إضافياً.
 */

/// خادمٌ مزيّف يُجيب `employee/me` بالصلاحيات المطلوبة.
///
/// ⚠ **يُسلَك المسارُ الحقيقيّ كاملاً** — `_restore` تسأل الخادم ثم تبني
/// `EmployeeProfile.fromJson`. وحقنُ الحالة مباشرةً كان أسهل، لكنه يتخطّى
/// القراءةَ التي قد تكون هي الكاسرة.
class _StubAdapter implements HttpClientAdapter {
  _StubAdapter(this.permissions);

  final List<String> permissions;

  @override
  void close({bool force = false}) {}

  @override
  Future<ResponseBody> fetch(
      RequestOptions options, Stream<Uint8List>? _, Future<void>? _) async {
    return ResponseBody.fromString(
      jsonEncode({
        'success': true,
        'message': '',
        'key': '',
        'data': _profileJson(permissions),
      }),
      200,
      headers: {
        Headers.contentTypeHeader: [Headers.jsonContentType],
      },
    );
  }
}

/// ملفُّ الموظف بالشكل الذي يعيده `employee/me` ويقرؤه `fromJson`.
Map<String, dynamic> _profileJson(List<String> permissions) => {
      'employee': {'id': 1, 'name': 'موظف اختبار', 'phone': '910000000'},
      'permissions': permissions,
      'points_of_sale': const [],
    };

class _FakeStore extends SecureStore {
  _FakeStore(this.permissions);

  final List<String> permissions;

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
      _profileJson(permissions);
}

/// التبويبُ مبنيّاً لموظفٍ يملك هذه الصلاحيات وحدَها.
Widget _app(List<String> permissions, Widget Function() screen) {
  final store = _FakeStore(permissions);
  final api = ApiClient(store)
    ..raw.httpClientAdapter = _StubAdapter(permissions);

  final router = GoRouter(
    routes: [
      GoRoute(path: '/', builder: (_, _) => screen()),
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

/// ما تعرضه الشاشة من بلاطات — تُعدّ لا تُسمّى.
Finder _tiles() => find.byType(InkWell);

/// يُركّب الشاشة ثم ينتظر استعادةَ الجلسة.
///
/// ⚠ **التركيبُ داخل `runAsync` لا قبلَه.** استعادةُ الجلسة تمرّ بـ dio،
/// وهو زمنٌ حقيقيّ لا نبضاتُ إطارات. ومهمّةٌ تبدأ في زمن الاختبار المزيّف
/// **لا تتقدّم** داخل `runAsync` بعدها — فتُقاس الشاشةُ وهي ما زالت
/// دوّامةَ تحميل، ويبدو كلُّ فحصٍ فاشلاً لسببٍ لا علاقةَ له بالصلاحيات.
Future<void> _mount(WidgetTester t, List<String> permissions,
    {Widget Function()? screen}) async {
  await t.pumpWidget(_app(permissions, screen ?? EmployeeHomeScreen.new));

  /*
   * ⚠ **يُنتظر حتى تُبنى الشاشة فعلاً، لا مدّةً ثابتة.**
   *
   * مهلةٌ ثابتة تنجح منفردةً وتسقط تحت حمل الحزمة كاملة — واختبارٌ
   * متذبذب يُعلّم إعادةَ التشغيل بدل الفحص، فيضيع سقوطُه الحقيقيّ.
   * فيُستطلَع اكتمالُ البناء بحدٍّ أعلى للأمان.
   */
  for (var i = 0; i < 60; i++) {
    // الترويسةُ تُبنى أوّلَ ما يصل الملفّ — فاسمُ الموظف علامةُ الجاهزية.
    if (find.text('موظف اختبار').evaluate().isNotEmpty) break;
    await t.runAsync(() => Future<void>.delayed(
        const Duration(milliseconds: 30)));
    await t.pump();
  }
  await t.pump(const Duration(milliseconds: 50));

  /*
   * ⚠ يُصرَّف مؤقّتُ حارسِ الإقلاع.
   *
   * `AuthController._bootstrap` يلفّ قراءاتِه بمهلةٍ ثمانِ ثوانٍ حتى لا
   * يتجمّد التطبيق على شاشة البداية إن تعلّق التخزين. والمؤقّتُ في زمن
   * الاختبار المزيّف لا يمضي وحدَه، فيبقى معلّقاً عند التفكيك ويُسقط
   * الفحصَ بـ«Pending timers» — لسببٍ لا علاقةَ له بما يُفحص.
   */
  await t.pump(const Duration(seconds: 9));
}

void main() {
  const noPermsBanner = 'لم تُمنح صلاحيات بعد';

  /*
   * كلُّ مفتاحٍ في الكتالوج اليوم، ومعه **التبويبُ الذي يظهر فيه**.
   *
   * ⚠ ومصدرُ القائمة `EmployeePermissions::CATALOG`؛ فمفتاحٌ يُضاف هناك ولا
   * يُضاف هنا يفوته هذا الاختبار — و`employee_permissions_matrix.php` في
   * الخادم هو ما يحرس الاكتمال.
   *
   * ⚠ وهذا الجدولُ **وثيقةٌ أيضاً**: هو الجوابُ على «أين يجد الموظف ما
   * مُنحه؟» بعد أن صارت الواجهةُ خمسةَ تبويبات.
   */
  final where = <String, Widget Function()>{
    // ١ — الحوالات
    'VIEW_INCOMING_TRANSFERS': EmployeeHomeScreen.new,
    'DELIVER_TRANSFER': EmployeeHomeScreen.new,
    'CREATE_TRANSFER': EmployeeHomeScreen.new,
    'VIEW_OWN_TRANSFERS': EmployeeHomeScreen.new,
    'CREATE_EXTERNAL_TRANSFER': EmployeeHomeScreen.new,
    'VIEW_POS_TRANSFERS': EmployeeHomeScreen.new,
    'SEARCH_TRANSFER': EmployeeHomeScreen.new,

    // ٢ — التقارير
    'VIEW_AGENT_TOTAL_BALANCE': () =>
        const EmployeeOwnReportsScreen(asTab: true),
    'VIEW_FINANCIAL_SUMMARY': () => const EmployeeOwnReportsScreen(asTab: true),
    'REPORT_DAILY_TRANSFERS': () => const EmployeeOwnReportsScreen(asTab: true),
    'REPORT_DELIVERED_TRANSFERS': () =>
        const EmployeeOwnReportsScreen(asTab: true),
    'REPORT_PENDING_TRANSFERS': () =>
        const EmployeeOwnReportsScreen(asTab: true),
    'REPORT_POINT_OF_SALE': () => const EmployeeOwnReportsScreen(asTab: true),
    'REPORT_AGENT_BALANCE': () => const EmployeeOwnReportsScreen(asTab: true),
    'REPORT_AUDIT': () => const EmployeeOwnReportsScreen(asTab: true),

    // ٥ — المستفيدون
    'VIEW_FAVORITES': () => const EmployeeFavoritesScreen(asTab: true),
    'MANAGE_FAVORITES': () => const EmployeeFavoritesScreen(asTab: true),
  };

  /*
   * مفاتيحُ **لا بابَ مستقلّاً لها** — وهي حالةٌ صحيحةٌ لا خلل.
   *
   * `REPORTS_VIEW` تفتح القسمَ ولا تُظهر تقريراً واحداً، وقد صار القسمُ
   * تبويباً دائماً في الشريط — فلا بلاطةَ لها تُعدّ.
   *
   * `MANAGE_FAVORITES` بابُها «إضافة مستفيد» داخل تبويب المستفيدين، ولا
   * تُظهر القائمةَ نفسَها: من مُنحها بلا `VIEW_FAVORITES` يرى ما ينقصه.
   */
  const noOwnDoor = <String>{'REPORTS_VIEW'};

  /*
   * مفاتيحُ **بابُها هو التبويبُ نفسُه** لا بلاطةٌ فيه.
   *
   * `VIEW_FAVORITES` تفتح قائمةَ المستفيدين، والقائمةُ هي المحتوى: لا بلاطةَ
   * تُعدّ. فالفحصُ عليها أنّ سطرَ «ما ينقصك» اختفى — أي أنّ البابَ فُتح.
   *
   * ⚠ وعدُّ البلاطات هنا كان سيقول «لا بابَ له» عن تبويبٍ يعمل: الاختبارُ
   * يقيس ما يقدر عليه، ولا يدّعي ما لا يقيس.
   */
  const doorIsTheTab = <String>{'VIEW_FAVORITES'};

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
  testWidgets('⚠ كلُّ مفتاحٍ في تبويبه: بابٌ يظهر، ولا لافتةَ فوق بابٍ يعمل',
      (tester) async {
    /*
     * ⚠ خطُّ الأساس يُقاس ولا يُكتب رقماً.
     *
     * كان الشرطُ «أكثر من `InkWell` واحد» لأنّ زرَّ الخروج وحدَه في الرأس.
     * ثمّ أُضيف زرُّ «الأمان» إلى الرأس — وهو بلا صلاحية عمداً، يخصّ جهازَ
     * الموظف لا عملَه — فصار العددُ اثنين بلا بلاطةٍ واحدة، وفشل الفحصُ
     * مُبلّغاً عن تناقضٍ لا وجود له.
     *
     * فيُقاس أثاثُ كلّ شاشةٍ بتركيبها **بلا صلاحية واحدة**: ما زاد عليه
     * بلاطة. وزرٌّ يُضاف إلى الرأس غداً يدخل في القياس من تلقاء نفسه، بينما
     * تبقى البلاطةُ الحقيقية مكشوفةً كما كانت.
     *
     * ⚠ ولكلّ تبويبٍ أثاثُه: قياسُ الرئيسية وتطبيقُه على التقارير كان
     * سيَعُدّ فرقَ الأثاث بلاطةً — أو يبتلع بلاطةً حقيقية.
     */
    final chrome = <String, int>{};

    for (final entry in where.entries) {
      final key = entry.key;
      final screen = entry.value;
      final id = '$screen';

      if (!chrome.containsKey(id)) {
        await _mount(tester, const [], screen: screen);
        final base = _tiles().evaluate().length;

        // ⚠ وخطُّ الأساس نفسُه محدود: لو صارت الشاشةُ تبني بلاطاتٍ بلا
        // صلاحية لابتلعها القياسُ صامتاً ومرّ الفحصُ على العيب الذي كُتب له.
        expect(base, lessThan(4),
            reason: 'أثاثُ الرأس تضخّم — أعِد النظر في خطّ الأساس قبل تمديده');
        chrome[id] = base;
      }

      await _mount(tester, [key], screen: screen);

      final banner = find.text(noPermsBanner).evaluate().isNotEmpty;
      final hasTile = _tiles().evaluate().length > chrome[id]!;

      expect(banner && hasTile, isFalse,
          reason: 'مع «$key»: اللافتةُ وبلاطةٌ معاً — تناقض');

      if (doorIsTheTab.contains(key)) {
        // البابُ هو المحتوى: يكفي أن يختفي سطرُ «ما ينقصك».
        expect(find.textContaining('راجع وكيلك').evaluate(), isEmpty,
            reason: 'مُنح «$key» ومع ذلك يقول التبويبُ إنّ صلاحيةً تنقصه');
      } else if (!noOwnDoor.contains(key)) {
        expect(hasTile, isTrue,
            reason: 'مُنح «$key» ولا بابَ له في تبويبه');
        expect(banner, isFalse,
            reason: 'مُنح «$key» ومع ذلك تقول الشاشةُ إنه بلا صلاحيات');
      }
    }
  });

  testWidgets('⚠ «تسجيل التسليم» وحدَه لا يترك الشاشة فارغةً بلا شرح',
      (tester) async {
    // الحالةُ التي كانت تُنتج شاشةً بيضاء قبل أن تُصلَح.
    await _mount(tester, const ['DELIVER_TRANSFER']);

    expect(find.text(noPermsBanner), findsNothing);
    // ⚠ بعد إعادة الهيكلة صار بابُ الواردة داخل «حوالات محلية» — والتسليمُ
    // من أبوابها، فيُفتح له القسمُ كلُّه لا بلاطةٌ منفصلة.
    expect(find.text('حوالات محلية'), findsOneWidget);
  });

  testWidgets('⚠ وتقريرٌ مفردٌ يظهر في تبويب التقارير لا في الحوالات',
      (tester) async {
    // في تبويب الحوالات: لا شيءَ له هناك، واللافتةُ تقول الحقيقة.
    await _mount(tester, const ['REPORT_DAILY_TRANSFERS']);
    expect(find.text(noPermsBanner), findsOneWidget);

    // وفي تبويبه: بابُه ظاهر.
    await _mount(tester, const ['REPORT_DAILY_TRANSFERS'],
        screen: () => const EmployeeOwnReportsScreen(asTab: true));
    expect(find.text('حوالات اليوم'), findsOneWidget);
  });

  testWidgets('ومنحُ صلاحيتين يُظهر بابيهما معاً', (tester) async {
    await _mount(tester, const ['SEARCH_TRANSFER', 'CREATE_TRANSFER']);

    expect(find.text(noPermsBanner), findsNothing);
    expect(find.text('بحث برقم حوالة'), findsOneWidget);
    expect(find.text('حوالات محلية'), findsOneWidget);
    expect(find.text('طلباتي'), findsOneWidget);
  });

  testWidgets('⚠ ورصيدُ الوكيل لا يظهر بلا صلاحيته', (tester) async {
    await _mount(tester, const ['VIEW_FINANCIAL_SUMMARY'],
        screen: () => const EmployeeOwnReportsScreen(asTab: true));

    // العنوانُ يتبع ما مُنح: ملخّصٌ لا «الأرصدة».
    expect(find.text('الملخّص المالي'), findsOneWidget);
    expect(find.textContaining('رصيد الوكيل'), findsNothing);
  });

  /*
   * ══════════════════════════════════════════════════════════════════════
   *  ⚠ وتبويبٌ بلا صلاحيةٍ يقول ما ينقصه ولا يختفي
   * ══════════════════════════════════════════════════════════════════════
   *
   * التبويباتُ الخمسةُ ثابتةٌ في الشريط بقرار: تبويبٌ يختفي يجعل الموظف
   * يظنّ التطبيقَ ناقصاً ويسأل عن ميزةٍ يراها في هاتف زميله. وسطرٌ يقول
   * «تحتاج صلاحية …» يحيله إلى وكيله في الحال.
   */
  testWidgets('⚠ تبويبُ المستفيدين بلا صلاحيته يقول ما ينقص', (tester) async {
    await _mount(tester, const [],
        screen: () => const EmployeeFavoritesScreen(asTab: true));

    expect(find.textContaining('عرض المفضّلة'), findsOneWidget);
    expect(find.textContaining('راجع وكيلك'), findsOneWidget);
  });

  testWidgets('⚠ وتبويبُ المراسلة كذلك', (tester) async {
    await _mount(tester, const [], screen: EmployeeChatTab.new);

    expect(find.textContaining('مراسلة الوكيل'), findsOneWidget);
    expect(find.textContaining('راجع وكيلك'), findsOneWidget);
  });

  /*
   * ⚠ والخزينةُ تبويبٌ بكشفين لا كشفٍ واحد — نصُّ أمر إعادة الهيكلة.
   *
   * والفرقُ بينهما ليس تجميلاً: الصادرةُ كاملةٌ أيّاً كانت حالتُها،
   * والواردةُ مسلَّمةٌ فقط. فاختفاءُ أحد الزرّين يعني كشفاً لا يبلغه الموظف.
   */
  testWidgets('⚠ والخزينةُ تعرض كشفَيها معاً', (tester) async {
    await _mount(tester, const ['VIEW_OWN_TRANSFERS'],
        screen: () => const EmployeeCashboxScreen(asTab: true));

    expect(find.text('كشف الصادرة'), findsOneWidget);
    expect(find.text('كشف الواردة'), findsOneWidget);
  });
}
