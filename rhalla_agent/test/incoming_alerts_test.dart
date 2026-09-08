import 'package:dio/dio.dart';
import 'package:flutter/services.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/net/api_client.dart';
import 'package:rhalla_agent/core/storage/secure_store.dart';
import 'package:rhalla_agent/features/alerts/incoming_alerts.dart';

/// جرسُ الوارد والشريطُ المنسدل: **متى يُنبِّه ومتى يسكت**.
///
/// هذا هو الشرط الذي يُفقد التنبيهَ معناه إن انكسر. شريطٌ ينسدل عند كل فتحة
/// عن حوالاتٍ عمرها يومان يُعلِّم الوكيل تجاهلَه، فيضيع الوصولُ الحقيقيّ في
/// ما اعتاد ألّا ينظر إليه. ولا يظهر ذلك في `analyze` ولا في تشغيلٍ سريع —
/// إنما بعد أسبوعٍ من العمل، حين يكون الوكيل قد تعلّم.

class _FakeStore extends SecureStore {
  Set<int> seen = <int>{};

  // بلا رمزٍ يرفض [ApiClient] الطلب قبل أن يصل إلى المحوّل، فتُبتلع
  // النتيجةُ صامتةً ويبدو الجرس وكأنه لم يجد شيئاً.
  @override
  Future<String?> readToken() async => 'رمز';

  @override
  Future<Set<int>> readSeenIncoming() async => seen;

  @override
  Future<void> writeSeenIncoming(Set<int> ids) async => seen = ids;
}

class _StubAdapter implements HttpClientAdapter {
  /// المعرّفات التي «يعيدها الخادم» في النبضة القادمة.
  List<int> ids = const [];

  @override
  void close({bool force = false}) {}

  @override
  Future<ResponseBody> fetch(
    RequestOptions options,
    Stream<Uint8List>? requestStream,
    Future<void>? cancelFuture,
  ) async =>
      ResponseBody.fromString(
        '{"success":true,"message":"","key":"","data":{"ids":$ids}}',
        200,
        headers: {
          Headers.contentTypeHeader: [Headers.jsonContentType],
        },
      );
}

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  late _StubAdapter adapter;
  late _FakeStore store;
  late IncomingAlertsController c;
  late int rings;

  setUp(() {
    adapter = _StubAdapter();
    store = _FakeStore();
    rings = 0;

    // الرنّة تمرّ بقناة الجهاز؛ نعدّها هنا بدل تشغيل صوت.
    TestDefaultBinaryMessengerBinding.instance.defaultBinaryMessenger
        .setMockMethodCallHandler(
      const MethodChannel('com.rhalla.rhalla_agent/device'),
      (call) async {
        if (call.method == 'notificationSound') rings++;
        return null;
      },
    );

    // والاهتزاز نداءٌ على قناة النظام — يُبتلع بلا أن يُعَدّ.
    TestDefaultBinaryMessengerBinding.instance.defaultBinaryMessenger
        .setMockMethodCallHandler(SystemChannels.platform, (_) async => null);

    c = IncomingAlertsController(
      ApiClient(store)..raw.httpClientAdapter = adapter,
      store,
    );
  });

  tearDown(() => c.dispose());

  group('النبضة الأولى', () {
    test('لا تُنبِّه ولو كان الوارد كلُّه غيرَ مرئيّ', () async {
      adapter.ids = [11, 12, 13];
      await c.refresh();

      // العدّاد يقول ثلاثة — صامتاً.
      expect(c.state.unseen, 3);
      expect(c.state.ping, 0, reason: 'لا شريط منسدل على متراكمٍ سابق');
      expect(rings, 0, reason: 'ولا رنّة');
    });

    test('ولا تكتم ما يصل بعدها', () async {
      adapter.ids = [11];
      await c.refresh();
      expect(c.state.ping, 0);

      adapter.ids = [11, 12];
      await c.refresh();

      expect(c.state.unseen, 2);
      expect(c.state.ping, 1);
      expect(c.state.arrived, 1, reason: 'الجديدةُ وحدَها لا الإجمالي');
      expect(rings, 1);
    });
  });

  group('النبضة', () {
    test('تُرفع مرّةً واحدة لكل واردة مهما تكرّرت النبضات', () async {
      adapter.ids = [11];
      await c.refresh();

      adapter.ids = [11, 12];
      await c.refresh();
      final after = c.state.ping;

      await c.refresh();
      await c.refresh();

      expect(c.state.ping, after, reason: 'لا شريطَ كلَّ ثلاثين ثانية');
      expect(rings, 1);
    });

    test('تُرفع ولو بقي العدد كما هو', () async {
      adapter.ids = [11];
      await c.refresh();

      // واحدةٌ سُلِّمت وأخرى وصلت في النبضة ذاتها: العدد 1 قبلُ و1 بعدُ.
      adapter.ids = [12];
      await c.refresh();

      expect(c.state.unseen, 1);
      expect(c.state.ping, 1, reason: 'الوصولُ حدثٌ لا فرقٌ في عدد');
      expect(c.state.arrived, 1);
    });

    test('لا تُرفع حين يفتح الوكيل القائمة', () async {
      adapter.ids = [11];
      await c.refresh();
      adapter.ids = [11, 12];
      await c.refresh();

      final before = c.state.ping;
      await c.markAllSeen();

      expect(c.state.unseen, 0, reason: 'فتحُ القائمة يُصفّر العدّاد');
      expect(c.state.ping, before,
          reason: 'وفتحُها ليس وصولاً — فلا ينسدل شريطٌ بلا حوالة');
    });
  });

  test('الخروجُ ثم العودة يُعيد تثبيت الأساس', () async {
    adapter.ids = [11];
    await c.refresh();
    adapter.ids = [11, 12];
    await c.refresh();
    expect(rings, 1);

    c.reset();

    // ⚠ الخارجُ لم يفتح القائمة، فما رآه لم يُحفظ: العائدُ يجد الاثنتين
    // غيرَ مرئيّتين — ولا يُنبَّه إليهما، لأنهما ليستا وصولاً جديداً.
    await c.refresh();

    expect(c.state.unseen, 2);
    expect(c.state.ping, 0);
    expect(rings, 1, reason: 'ولا رنّةَ ثانية لما سبق أن رنّ له');
  });
}
