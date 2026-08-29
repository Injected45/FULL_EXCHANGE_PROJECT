import 'package:flutter/widgets.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import '../../core/storage/secure_store.dart';

/// الوكيل — مستخدم الـ API.
///
/// لا يوجد نظام أدوار في الخلفية: الأدوار أرقام سحرية تُقارَن سطرياً.
/// الوكيل الحقيقي هو UeserType == 3 **و** AccountType == 'Main'.
/// UeserType 3 مع AccountType 'pos' هو نقطة بيع تابعة، بصلاحيات أقل.
class AgentUser {
  const AgentUser({
    required this.id,
    required this.phone,
    required this.userType,
    required this.accountType,
    required this.accId,
    required this.branchId,
    required this.registered,
    this.name,
    this.postName,
    this.info,
  });

  final int id;
  final String phone;
  final int userType;
  final String accountType;
  final int accId;
  final int branchId;
  final bool registered;
  final String? name;

  /// اسم نقطة البيع — null للوكيل الرئيسي.
  final String? postName;

  /// نتيجة الـ view المسمّى getInfo. أعمدته غير معروفة من الكود،
  /// فيبقى خريطة حرّة حتى يوثّقها المكتب الخلفي.
  final Map<String, dynamic>? info;

  bool get isAgent => userType == 3;
  bool get isMainAgent => isAgent && accountType == 'Main';
  bool get isPos => isAgent && accountType == 'pos';

  /// اسم الفرع/الجهة من الـ view المسمّى getInfo.
  String? get branchName {
    final v = info?['BName']?.toString().trim();
    return (v == null || v.isEmpty) ? null : v;
  }

  /// رمز العملة المحلية كما يعرّفه الخادم — «د.ل» عادةً، ولا يُثبَّت في الكود.
  String get currencyCode {
    final v = info?['CurCode']?.toString().trim();
    return (v == null || v.isEmpty) ? 'د.ل' : v;
  }

  int get currencyId {
    final v = info?['DefualtCurrency'];
    if (v is int) return v;
    return int.tryParse('$v') ?? 1;
  }

  /// نقطة البيع تُعرَف باسمها، والوكيل الرئيسي باسم جهته.
  /// الهاتف آخر ملاذ — لا نعرض حرفاً من رقم كصورة رمزية.
  String get displayName => postName ?? branchName ?? name ?? Fmt.phone(phone);

  /// حرف الصورة الرمزية. فارغ حين لا يوجد اسم حقيقي — الواجهة تعرض أيقونة.
  String? get initial {
    final n = postName ?? branchName ?? name;
    if (n == null || n.trim().isEmpty) return null;
    return n.trim().characters.first;
  }

  static int _int(dynamic v) {
    if (v is int) return v;
    if (v is num) return v.toInt();
    return int.tryParse('$v') ?? 0;
  }

  factory AgentUser.fromJson(Map<String, dynamic> j, {Map<String, dynamic>? info, String? postName}) {
    return AgentUser(
      id: _int(j['id']),
      phone: '${j['phone'] ?? ''}',
      userType: _int(j['UeserType']),
      accountType: '${j['AccountType'] ?? ''}',
      accId: _int(j['AccID']),
      // بهذا الإملاء في قاعدة البيانات — لا تصحّحه.
      branchId: _int(j['BrancchID']),
      registered: '${j['Reg'] ?? ''}'.toLowerCase() == 'yes',
      name: (j['name'] as String?)?.trim().isEmpty ?? true ? null : j['name'] as String?,
      postName: postName,
      info: info,
    );
  }

  Map<String, dynamic> toJson() => {
        'id': id,
        'phone': phone,
        'UeserType': userType,
        'AccountType': accountType,
        'AccID': accId,
        'BrancchID': branchId,
        'Reg': registered ? 'Yes' : 'NO',
        'name': name,
        '_postName': postName,
        // يُحفظ أيضاً — منه يأتي اسم الجهة ورمز العملة، وبدونه
        // يعود الاسم إلى رقم الهاتف بعد إعادة تشغيل التطبيق.
        '_info': info,
      };

  factory AgentUser.fromCache(Map<String, dynamic> j) => AgentUser.fromJson(
        j,
        postName: j['_postName'] as String?,
        info: (j['_info'] as Map?)?.cast<String, dynamic>(),
      );
}

class AuthSession {
  const AuthSession(this.token, this.user);
  final String token;
  final AgentUser user;
}

class AuthRepository {
  AuthRepository(this._api, this._store);

  final ApiClient _api;
  final SecureStore _store;

  /// 1) إرسال رمز التحقّق عبر واتساب.
  /// الخادم يضيف بادئة 218 بنفسه — نرسل 9 خانات فقط.
  Future<void> requestOtp(String phone) async {
    await _api.post('/device/otp/send', body: {'phone': Fmt.phoneForApi(phone)});
  }

  /// 2) التحقّق من الرمز.
  /// هذه النقطة **خارج الغلاف** — ترد {message, status} فقط.
  Future<void> verifyOtp(String phone, String code) async {
    await _api.post('/device/otp/checkOtp', body: {
      'phone': Fmt.phoneForApi(phone),
      'CodeOtp': int.tryParse(code) ?? code,
    });
  }

  /// 2) استبدال الرمز برمز Sanctum — نقطة واحدة تتحقّق وتستهلك وتُصدر.
  ///
  /// الخادم هو من يتحقّق من الـ OTP هنا، لا العميل — وهذا ما يميّزها عن
  /// update/password التي تسمح بالتغيير بمعرفة الهاتف ومعرّف الجهاز فقط.
  /// وتعيد ربط الجهاز بعد تحقّق مؤكَّد، فلا تقفل إعادةُ التثبيت الحساب.
  ///
  /// إن ردّت 404 فالنقطة غير منشورة على ذلك الخادم بعد.
  Future<AuthSession> completeOtpLogin(String phone, String code) async {
    final deviceId = await _store.deviceId();

    late final Envelope env;
    try {
      env = await _api.post('/device/otp/login', body: {
        'phone': Fmt.phoneForApi(phone),
        'CodeOtp': int.tryParse(code) ?? code,
        'device_id': deviceId,
      });
    } on ApiFailure catch (e) {
      if (e.statusCode == 404 || e.statusCode == 405) {
        throw ApiFailure(
          'نقطة الدخول بالرمز غير مفعّلة على الخادم بعد. '
          'راجع فريق الخلفية — device/otp/login.',
          statusCode: e.statusCode,
        );
      }
      rethrow;
    }

    return _sessionFrom(env);
  }

  /// المسار البديل — الدخول بكلمة مرور، وهو ما تدعمه الخلفية اليوم.
  /// يبقى موجوداً حتى تُضاف نقطة الـ OTP، ولاختبار الحسابات القائمة.
  Future<AuthSession> loginWithPassword(String phone, String password) async {
    final deviceId = await _store.deviceId();
    final env = await _api.post('/device/login', body: {
      'phone': Fmt.phoneForApi(phone),
      'password': password,
      'device_id': deviceId,
    });
    return _sessionFrom(env);
  }

  Future<AuthSession> _sessionFrom(Envelope env) async {
    final data = env.payload;
    if (data is! Map) {
      throw ApiFailure('رد غير متوقّع من الخادم عند تسجيل الدخول.');
    }
    final m = data.cast<String, dynamic>();

    final token = '${m['token'] ?? ''}';
    if (token.isEmpty) {
      throw ApiFailure('لم يصل رمز الدخول من الخادم.');
    }

    final userJson = (m['user'] as Map?)?.cast<String, dynamic>();
    if (userJson == null) {
      throw ApiFailure('لم تصل بيانات المستخدم من الخادم.');
    }

    final user = AgentUser.fromJson(
      userJson,
      info: (m['info'] as Map?)?.cast<String, dynamic>(),
      postName: m['Name_post'] as String?,
    );

    // هذا تطبيق الوكيل — لا يُفتح لدور آخر.
    if (!user.isAgent) {
      throw ApiFailure('هذا الحساب ليس حساب وكيل. استخدم التطبيق المخصّص لدورك.');
    }

    await _store.writeToken(token);
    await _store.writeUser(user.toJson());

    return AuthSession(token, user);
  }

  Future<AgentUser?> restore() async {
    final token = await _store.readToken();
    if (token == null || token.isEmpty) return null;
    final cached = await _store.readUser();
    if (cached == null) return null;
    return AgentUser.fromCache(cached);
  }

  Future<void> signOut() => _store.signOut();
}

final authRepositoryProvider = Provider<AuthRepository>(
  (ref) => AuthRepository(ref.watch(apiClientProvider), ref.watch(secureStoreProvider)),
);
