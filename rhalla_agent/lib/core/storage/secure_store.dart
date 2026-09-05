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

  /// رمز الجلسة الفعّالة — رمز الوكيل أو رمز الموظف.
  ///
  /// ⚠ **واحد لا اثنان.** التطبيق إمّا في وضع الوكيل أو وضع الموظف ولا يجمع
  /// بينهما: الفصل بين سياقَي التوثيق شرطٌ أمني (بند 22 من مستند الموظفين)،
  /// وأقصر طريق لخرقه أن يحمل الجهاز رمزين ويختار `ApiClient` أحدهما بخطأ.
  ///
  /// ولذلك [writeEmployeeToken] تمحو رمز الوكيل، و[writeToken] تمحو رمز
  /// الموظف — لا تتعايشان في التخزين أصلاً.
  Future<String?> readToken() async =>
      (await _s.read(key: _kEmployeeToken)) ?? (await _s.read(key: _kToken));

  Future<void> writeToken(String v) async {
    await _s.delete(key: _kEmployeeToken);
    await _s.delete(key: _kEmployee);
    await _s.write(key: _kToken, value: v);
  }

  Future<void> clearToken() => _s.delete(key: _kToken);

  /* ── جلسة الموظف ─────────────────────────────────────────────── */

  static const _kEmployeeToken = 'employee_token';
  static const _kEmployee = 'employee_json';

  Future<String?> readEmployeeToken() => _s.read(key: _kEmployeeToken);

  Future<void> writeEmployeeToken(String v) async {
    await _s.delete(key: _kToken);
    await _s.delete(key: _kUser);
    await _s.write(key: _kEmployeeToken, value: v);
  }

  Future<Map<String, dynamic>?> readEmployee() async {
    final raw = await _s.read(key: _kEmployee);
    if (raw == null || raw.isEmpty) return null;
    try {
      return (jsonDecode(raw) as Map).cast<String, dynamic>();
    } catch (_) {
      return null;
    }
  }

  Future<void> writeEmployee(Map<String, dynamic> e) =>
      _s.write(key: _kEmployee, value: jsonEncode(e));

  /// خروج الموظف — يمحو رمزه وبياناته ولا يمسّ معرّف الجهاز.
  ///
  /// معرّف الجهاز يبقى دائماً: الخادم يربط به التفعيل، وتغييره يفقد الموظف
  /// جهازه المعتمد ويحتاج كوداً جديداً بلا سبب.
  Future<void> clearEmployee() async {
    await _s.delete(key: _kEmployeeToken);
    await _s.delete(key: _kEmployee);
  }

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

  // ─── جرس الوارد: ما فتحه الوكيل من الحوالات الواردة ──────────────────

  static const _kSeenIncoming = 'seen_incoming_ids';

  /// أرقام الحوالات الواردة التي فتحها الوكيل.
  ///
  /// على الجهاز لا في الخادم، لأن السؤال نفسه محلّي: «هل رأيتُ هذه؟» يخصّ
  /// من يمسك الهاتف، لا الحساب. وحفظُه في الخادم يعني جدولاً جديداً وكتابةً
  /// عند كل فتح فاتورة.
  ///
  /// ⚠ **لا تُمسح عند تسجيل الخروج.** الوكيل هو الوكيل نفسه بعد أن يعود،
  /// ومسحُها يجعل كل حوالاته السابقة «جديدة» فيرنّ الجرس لها كلّها.
  Future<Set<int>> readSeenIncoming() async {
    final raw = await _s.read(key: _kSeenIncoming);
    if (raw == null || raw.isEmpty) return {};
    try {
      return (jsonDecode(raw) as List)
          .map((e) => int.tryParse('$e') ?? -1)
          .where((e) => e >= 0)
          .toSet();
    } catch (_) {
      // قائمة تالفة تُعامَل كغياب، لا كخطأ: أسوأ ما يقع أن يرنّ الجرس
      // مرّةً واحدة زائدة، وهو أهون من شاشة لا تفتح.
      return {};
    }
  }

  /// يُبقي الأحدث فقط.
  ///
  /// الأرقام تصاعدية، فالأكبر هو الأحدث. والخادم لا يعيد أكثر من 200 رقم
  /// أصلاً، فحفظ 400 يغطّي ضعف ما يُسأل عنه ولا ينمو مع عمر الحساب —
  /// وقائمة تكبر بلا حدّ في تخزينٍ مشفَّر تُبطئ كل قراءة لها.
  Future<void> writeSeenIncoming(Set<int> ids) async {
    final kept = ids.toList()..sort();
    final trimmed =
        kept.length > 400 ? kept.sublist(kept.length - 400) : kept;
    await _s.write(key: _kSeenIncoming, value: jsonEncode(trimmed));
  }

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
