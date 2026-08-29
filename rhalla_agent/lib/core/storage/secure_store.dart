import 'dart:convert';
import 'dart:io' show Platform;
import 'dart:math';

import 'package:device_info_plus/device_info_plus.dart';
import 'package:flutter/services.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

/// التخزين الآمن.
///
/// رمز Sanctum **لا ينتهي أبداً** (config/sanctum.php: expiration => null)،
/// فلا يجوز وضعه في SharedPreferences.
///
/// ⚠️ معرّف الجهاز — أخطر قيمة في التطبيق:
/// لكل مستخدم جهاز واحد فقط في جدول users. `login` يرفض عدم التطابق،
/// ولا يفكّ القفل إلا إعادة تعيين Reg='NO' من المكتب الخلفي.
/// وبما أن التخزين الآمن يُمسح مع إزالة التطبيق، كان **إعادة التثبيت تقفل
/// المستخدم**. عولج ذلك بشقّين:
///   1) `deviceId()` يشتقّ المُعرّف من العتاد لا من العشوائية —
///      `ANDROID_ID` على أندرويد و`identifierForVendor` على iOS —
///      فيبقى نفسه بعد إعادة التثبيت.
///   2) `POST device/otp/login` في الخادم **يعيد ربط** الجهاز بعد تحقّق
///      OTP ناجح، فحتى لو تغيّر المُعرّف لا يُقفل الحساب.
///
/// ⚠️ ومع ذلك: المُعرّف المخزَّن **لا يُستبدل أبداً** إن وُجد. الحسابات
/// المُفعَّلة قبل هذا التغيير مربوطة بمُعرّف عشوائي في قاعدة البيانات،
/// واستبداله بمُعرّف عتادي كان سيقفلها جميعاً.
class SecureStore {
  static const _kToken = 'auth_token';
  static const _kDeviceId = 'device_id';
  static const _kUser = 'user_json';
  static const _kOnboarded = 'onboarded';

  final _s = const FlutterSecureStorage(
    aOptions: AndroidOptions(encryptedSharedPreferences: true),
    iOptions: IOSOptions(accessibility: KeychainAccessibility.first_unlock),
  );

  Future<String?> readToken() => _s.read(key: _kToken);
  Future<void> writeToken(String v) => _s.write(key: _kToken, value: v);
  Future<void> clearToken() => _s.delete(key: _kToken);

  Future<Map<String, dynamic>?> readUser() async {
    final raw = await _s.read(key: _kUser);
    if (raw == null || raw.isEmpty) return null;
    try {
      return (jsonDecode(raw) as Map).cast<String, dynamic>();
    } catch (_) {
      return null;
    }
  }

  Future<void> writeUser(Map<String, dynamic> u) =>
      _s.write(key: _kUser, value: jsonEncode(u));

  Future<bool> readOnboarded() async => (await _s.read(key: _kOnboarded)) == '1';
  Future<void> setOnboarded() => _s.write(key: _kOnboarded, value: '1');

  static const _device =
      MethodChannel('com.rhalla.rhalla_agent/device');

  /// مُعرّف الجهاز — يُحسب مرة واحدة ثم يُعاد نفسه إلى الأبد.
  ///
  /// الترتيب مقصود:
  ///   1. المخزَّن إن وُجد — لا يُمسّ، وإلا قُفلت الحسابات القائمة.
  ///   2. مُعرّف العتاد — يبقى عبر إعادة التثبيت.
  ///   3. عشوائي — احتياطي أخير حين يفشل الاثنان.
  Future<String> deviceId() async {
    final existing = await _s.read(key: _kDeviceId);
    if (existing != null && existing.isNotEmpty) return existing;

    final id = await _hardwareId() ?? _randomId();
    await _s.write(key: _kDeviceId, value: id);
    return id;
  }

  Future<String?> _hardwareId() async {
    try {
      if (Platform.isAndroid) {
        // ANDROID_ID عبر قناة في MainActivity — يبقى عبر إعادة التثبيت
        // ولا يتغيّر إلا بإعادة ضبط المصنع.
        final v = await _device.invokeMethod<String>('hardwareId');
        if (v != null && v.trim().isNotEmpty) return 'a:${v.trim()}';
      } else if (Platform.isIOS) {
        // identifierForVendor: يبقى ما دام تطبيق واحد من الشركة مُثبَّتاً،
        // وهو أثبت ما تتيحه المنصّة بلا صلاحيات إضافية.
        final info = await DeviceInfoPlugin().iosInfo;
        final v = info.identifierForVendor;
        if (v != null && v.trim().isNotEmpty) return 'i:${v.trim()}';
      }
    } catch (_) {
      // منصّة غير مدعومة أو قناة غير مسجَّلة — نسقط إلى العشوائي.
    }
    return null;
  }

  String _randomId() {
    final rnd = Random.secure();
    final bytes = List<int>.generate(16, (_) => rnd.nextInt(256));
    return bytes.map((b) => b.toRadixString(16).padLeft(2, '0')).join();
  }

  Future<void> signOut() async {
    await _s.delete(key: _kToken);
    await _s.delete(key: _kUser);
    // معرّف الجهاز يبقى عمداً — مسحه يقفل الحساب.
  }
}
