import 'dart:typed_data';

import 'package:dio/dio.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/net/api_client.dart';
import 'package:rhalla_agent/core/net/api_envelope.dart';
import 'package:rhalla_agent/core/storage/secure_store.dart';

/// الجلسة المنتهية (401) يجب أن تُخرج الوكيل، لا أن تتركه عالقاً
/// أمام خطأ على كل شاشة برمز ميت في التخزين.
///
/// الوصل نفسه في `authControllerProvider`؛ هذه الاختبارات تقفل الطرف
/// الذي يطلق الإشارة — [ApiClient.onUnauthorized].

class _FakeStore extends SecureStore {
  @override
  Future<String?> readToken() async => 'رمز-منتهٍ';
}

class _StubAdapter implements HttpClientAdapter {
  _StubAdapter(this.status);

  int status;
  String body = '{}';

  @override
  void close({bool force = false}) {}

  @override
  Future<ResponseBody> fetch(
    RequestOptions options,
    Stream<Uint8List>? requestStream,
    Future<void>? cancelFuture,
  ) async =>
      ResponseBody.fromString(
        body,
        status,
        headers: {
          Headers.contentTypeHeader: [Headers.jsonContentType],
        },
      );
}

void main() {
  late ApiClient api;
  late _StubAdapter adapter;
  late int calls;

  setUp(() {
    adapter = _StubAdapter(401);
    api = ApiClient(_FakeStore())..raw.httpClientAdapter = adapter;
    calls = 0;
    api.onUnauthorized = () => calls++;
  });

  test('401 يطلق الخروج', () async {
    await expectLater(api.post('/x'), throwsA(isA<ApiFailure>()));
    expect(calls, 1);
  });

  test('401 مهما قال الجسم — success:true لا يلغي انتهاء الجلسة', () async {
    adapter.body = '{"success":true,"message":"تم"}';
    await expectLater(api.post('/x'), throwsA(isA<ApiFailure>()));
    expect(calls, 1);
  });

  test('طلبات متوازية ترد 401 معاً تُخرج مرّة واحدة', () async {
    final rs = await Future.wait([
      api.post('/a').then<Object?>((_) => null).catchError((e) => e),
      api.post('/b').then<Object?>((_) => null).catchError((e) => e),
      api.post('/c').then<Object?>((_) => null).catchError((e) => e),
    ]);
    expect(rs.every((e) => e is ApiFailure), isTrue);
    expect(calls, 1, reason: 'خروج واحد لا ثلاثة');
  });

  test('الردّ الناجح لا يطلق الخروج', () async {
    adapter
      ..status = 200
      ..body = '{"success":true,"data":{"x":1}}';
    await api.post('/x');
    expect(calls, 0);
  });

  test('جلسة جديدة بعد خروج: 401 لاحق يطلق الخروج ثانيةً', () async {
    await expectLater(api.post('/x'), throwsA(isA<ApiFailure>()));
    expect(calls, 1);

    // نجاح يعيد ضبط الحارس — وإلا بقي أول خروج يبتلع كل ما بعده.
    adapter
      ..status = 200
      ..body = '{"success":true,"data":{}}';
    await api.post('/x');

    adapter
      ..status = 401
      ..body = '{}';
    await expectLater(api.post('/x'), throwsA(isA<ApiFailure>()));
    expect(calls, 2);
  });

  test('403 ليست انتهاء جلسة — تبويب نقاط البيع لغير الوكيل الرئيسي', () async {
    adapter
      ..status = 403
      ..body = '{"success":false,"message":"غير مصرّح"}';
    await expectLater(api.post('/x'), throwsA(isA<ApiFailure>()));
    expect(calls, 0, reason: '403 صلاحية لا جلسة — الخروج هنا خطأ');
  });
}
