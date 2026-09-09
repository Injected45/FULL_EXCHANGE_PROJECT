import 'dart:async';

import 'package:dio/dio.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/net/api_client.dart';
import 'package:rhalla_agent/core/storage/secure_store.dart';
import 'package:rhalla_agent/features/auth/auth_controller.dart';
import 'package:rhalla_agent/features/auth/auth_repository.dart';
import 'package:rhalla_agent/features/employee_app/employee_session.dart';

/*
 * ══════════════════════════════════════════════════════════════════════════
 *  ⚠⚠ الإقلاعُ يُحسَم دائماً — ولو أخفق التخزينُ أو تعلّق
 * ══════════════════════════════════════════════════════════════════════════
 *
 * الراوتر يحبس المستخدم في `/splash` ما دامت حالةُ الوكيل **أو** حالةُ الموظف
 * `unknown`. فاستثناءٌ واحدٌ في الإقلاع **يُجمّد التطبيق على شاشة البداية إلى
 * الأبد**: لا شاشةَ دخول، ولا رسالةَ خطأ، ولا مخرج إلّا حذفُ التطبيق.
 *
 * وقع مرّتين على أجهزةٍ حقيقيّة في 9 سبتمبر 2026 — وبلّغ عنه المالك بصورةٍ
 * للتطبيق واقفاً على الشعار. والسببُ في الحالتين واحد: قراءةٌ من التخزين
 * الآمن خارج أيّ حارس.
 *
 * ⚠ والتخزينُ الآمن يُخفق فعلاً: مفتاحُ Android Keystore يضيع مع إعادة
 * التثبيت بينما يبقى ملفُّ التفضيلات المشفَّر (نسخُ النظام الاحتياطيّ)،
 * فتُخفق فكُّ الشفرة وتُرمى من `read` نفسِها.
 *
 * وهذه الاختباراتُ تُحاكي الحالتين اللتين لا يُريهما جهازٌ سليم: تخزينٌ
 * يرمي، وتخزينٌ **يتعلّق ولا يرمي**.
 */

/// تخزينٌ يرمي في كل قراءة.
class _Throws extends SecureStore {
  @override
  Future<bool> readOnboarded() async => throw Exception('keystore');

  @override
  Future<String?> readToken() async => throw Exception('keystore');

  @override
  Future<String?> readEmployeeToken() async => throw Exception('keystore');

  @override
  Future<Map<String, dynamic>?> readUser() async => throw Exception('keystore');

  @override
  Future<Map<String, dynamic>?> readEmployee() async =>
      throw Exception('keystore');
}

/// تخزينٌ **يتعلّق** — أخبثُ من الذي يرمي: لا استثناءَ يُمسك.
class _Hangs extends SecureStore {
  Future<Never> _never() => Completer<Never>().future;

  @override
  Future<bool> readOnboarded() => _never();

  @override
  Future<String?> readToken() => _never();

  @override
  Future<String?> readEmployeeToken() => _never();

  @override
  Future<Map<String, dynamic>?> readUser() => _never();

  @override
  Future<Map<String, dynamic>?> readEmployee() => _never();
}

/// محوّلٌ لا يصل إلى شبكة — كلُّ نداءٍ يُخفق فوراً.
class _DeadAdapter implements HttpClientAdapter {
  @override
  void close({bool force = false}) {}

  @override
  Future<ResponseBody> fetch(RequestOptions o, Stream<List<int>>? a,
          Future<void>? b) async =>
      throw DioException.connectionError(
          requestOptions: o, reason: 'لا شبكة');
}

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  ApiClient dead(SecureStore s) =>
      ApiClient(s)..raw.httpClientAdapter = _DeadAdapter();

  group('⚠⚠ جلسةُ الوكيل تُحسَم', () {
    test('تخزينٌ يرمي ⇦ «خارج» لا «غير معروف»', () async {
      final store = _Throws();
      final c = AuthController(AuthRepository(dead(store), store), store);

      await Future<void>.delayed(const Duration(milliseconds: 60));

      expect(c.state.status, isNot(AuthStatus.unknown),
          reason: 'الراوتر يحبس المستخدم في شاشة البداية على `unknown`');
      expect(c.state.status, AuthStatus.signedOut);
      c.dispose();
    });

    test('⚠ وتخزينٌ يتعلّق ⇦ يُحسَم بالمهلة', () async {
      final store = _Hangs();
      final c = AuthController(AuthRepository(dead(store), store), store);

      // المهلةُ ثمانِ ثوانٍ — ننتظرها في زمنٍ حقيقيّ.
      await Future<void>.delayed(const Duration(seconds: 9));

      expect(c.state.status, AuthStatus.signedOut,
          reason: 'قراءةٌ معلّقة لا تُبقي التطبيق على الشعار أبداً');
      c.dispose();
    }, timeout: const Timeout(Duration(seconds: 30)));
  });

  group('⚠⚠ جلسةُ الموظف تُحسَم', () {
    test('تخزينٌ يرمي ⇦ «خارج»', () async {
      final store = _Throws();
      final c = EmployeeAuthController(dead(store), store);

      await Future<void>.delayed(const Duration(milliseconds: 60));

      expect(c.state.status, isNot(EmpSessionStatus.unknown));
      expect(c.state.status, EmpSessionStatus.signedOut);
      c.dispose();
    });

    test('⚠ وتخزينٌ يتعلّق ⇦ يُحسَم بالمهلة', () async {
      final store = _Hangs();
      final c = EmployeeAuthController(dead(store), store);

      await Future<void>.delayed(const Duration(seconds: 9));

      expect(c.state.status, EmpSessionStatus.signedOut);
      c.dispose();
    }, timeout: const Timeout(Duration(seconds: 30)));
  });
}
