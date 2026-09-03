import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/net/api_client.dart';
import '../../core/storage/secure_store.dart';

/// جلسة الموظف — منفصلة تماماً عن جلسة الوكيل.
///
/// ⚠ الفصل ليس تنظيماً بل شرط أمني (بند 22): رمز الموظف لا يفتح مسار
/// مسؤول، ولا تُرقّى جلسته. وهو محفوظ بمفتاحٍ آخر في التخزين، ولا يتعايش
/// مع رمز الوكيل أصلاً — انظر [SecureStore.writeEmployeeToken].
///
/// والصلاحيات **لا تُخزَّن في الجهاز مصدراً للحقيقة**: تُقرأ من `employee/me`
/// عند كل فتح، فسحبُها من الإدارة يظهر في الشاشة. وما يُخزَّن نسخةٌ لعرضٍ
/// أوّليّ سريع فقط — الرفض الحقيقي في الخادم عند كل نداء.

class EmployeeProfile {
  const EmployeeProfile({
    required this.id,
    required this.name,
    required this.phone,
    this.activePosId,
    this.pointsOfSale = const [],
    this.permissions = const [],
    this.openShift,
  });

  final int id;
  final String name;
  final String phone;

  final int? activePosId;
  final List<EmployeePos> pointsOfSale;
  final List<String> permissions;
  final OpenShift? openShift;

  bool can(String key) => permissions.contains(key);

  String get posName {
    for (final p in pointsOfSale) {
      if (p.id == activePosId) return p.name;
    }
    return pointsOfSale.isEmpty ? '' : pointsOfSale.first.name;
  }

  static EmployeeProfile fromJson(Map<String, dynamic> j) {
    final e = (j['employee'] as Map?)?.cast<String, dynamic>() ?? const {};
    final shift = (j['open_shift'] as Map?)?.cast<String, dynamic>();

    return EmployeeProfile(
      id: int.tryParse('${e['id'] ?? 0}') ?? 0,
      name: '${e['name'] ?? ''}'.trim(),
      phone: '${e['phone'] ?? ''}'.trim(),
      activePosId: j['active_point_of_sale_id'] == null
          ? null
          : int.tryParse('${j['active_point_of_sale_id']}'),
      pointsOfSale: ((j['points_of_sale'] as List?) ?? const [])
          .whereType<Map>()
          .map((m) => EmployeePos.fromJson(m.cast<String, dynamic>()))
          .toList(),
      permissions:
          ((j['permissions'] as List?) ?? const []).map((e) => '$e').toList(),
      openShift: shift == null ? null : OpenShift.fromJson(shift),
    );
  }

  Map<String, dynamic> toJson() => {
        'employee': {'id': id, 'name': name, 'phone': phone},
        'active_point_of_sale_id': activePosId,
        'points_of_sale':
            pointsOfSale.map((p) => {'id': p.id, 'name': p.name}).toList(),
        'permissions': permissions,
      };
}

class EmployeePos {
  const EmployeePos({required this.id, required this.name});

  final int id;
  final String name;

  static EmployeePos fromJson(Map<String, dynamic> j) => EmployeePos(
        id: int.tryParse('${j['id'] ?? 0}') ?? 0,
        name: '${j['name'] ?? ''}'.trim(),
      );
}

class OpenShift {
  const OpenShift({
    required this.id,
    required this.openingCash,
    required this.startedAt,
  });

  final int id;
  final double openingCash;
  final String startedAt;

  static OpenShift fromJson(Map<String, dynamic> j) => OpenShift(
        id: int.tryParse('${j['id'] ?? 0}') ?? 0,
        openingCash: double.tryParse('${j['opening_cash'] ?? 0}') ?? 0,
        startedAt: '${j['started_at'] ?? ''}'.trim(),
      );
}

/// حالة جلسة الموظف في التطبيق.
enum EmpSessionStatus { unknown, signedOut, signedIn }

class EmployeeAuthState {
  const EmployeeAuthState({required this.status, this.profile});

  final EmpSessionStatus status;
  final EmployeeProfile? profile;

  static const initial = EmployeeAuthState(status: EmpSessionStatus.unknown);
}

class EmployeeAuthController extends StateNotifier<EmployeeAuthState> {
  EmployeeAuthController(this._api, this._store)
      : super(EmployeeAuthState.initial) {
    _restore();
  }

  final ApiClient _api;
  final SecureStore _store;

  /// استعادة الجلسة عند الإقلاع.
  ///
  /// الرمز وحده لا يكفي: يُسأل الخادم عن صحّته وعن الصلاحيات الحالية. رمزٌ
  /// أُلغي من لوحة الإدارة يجب أن يسقط هنا لا أن يبقى الموظف «داخلاً».
  Future<void> _restore() async {
    final token = await _store.readEmployeeToken();
    if (token == null || token.isEmpty) {
      if (mounted) state = const EmployeeAuthState(status: EmpSessionStatus.signedOut);
      return;
    }

    try {
      final env = await _api.get('/device/employee/me');
      final profile = EmployeeProfile.fromJson(env.row ?? const {});
      await _store.writeEmployee(profile.toJson());
      if (!mounted) return;
      state = EmployeeAuthState(
          status: EmpSessionStatus.signedIn, profile: profile);
    } catch (_) {
      // 401 يعني رمزاً ملغى أو موظفاً موقوفاً — يُمحى ويعود إلى الدخول.
      await _store.clearEmployee();
      if (!mounted) return;
      state = const EmployeeAuthState(status: EmpSessionStatus.signedOut);
    }
  }

  /// بعد نجاح التفعيل.
  Future<void> adopt(String token, Map<String, dynamic> employeeJson) async {
    await _store.writeEmployeeToken(token);
    await refresh();
  }

  /// إعادة قراءة الملف والصلاحيات — تُستدعى عند فتح الشاشات المهمّة.
  Future<void> refresh() async {
    try {
      final env = await _api.get('/device/employee/me');
      final profile = EmployeeProfile.fromJson(env.row ?? const {});
      await _store.writeEmployee(profile.toJson());
      if (!mounted) return;
      state = EmployeeAuthState(
          status: EmpSessionStatus.signedIn, profile: profile);
    } catch (_) {
      await _store.clearEmployee();
      if (!mounted) return;
      state = const EmployeeAuthState(status: EmpSessionStatus.signedOut);
    }
  }

  /// خروج صريح — يُبطل التفعيل في الخادم أيضاً.
  ///
  /// النداء أولاً ثم المحو محلياً: لو مُحي الرمز أوّلاً لما استطعنا إبلاغ
  /// الخادم، فيبقى التفعيل حيّاً على جهازٍ خرج منه صاحبه.
  Future<void> signOut() async {
    try {
      await _api.post('/device/employee/logout');
    } catch (_) {
      // الشبكة قد تكون منقطعة؛ الخروج المحلّي يقع على أي حال.
    }
    await _store.clearEmployee();
    if (!mounted) return;
    state = const EmployeeAuthState(status: EmpSessionStatus.signedOut);
  }
}

final employeeAuthProvider =
    StateNotifierProvider<EmployeeAuthController, EmployeeAuthState>(
  (ref) => EmployeeAuthController(
    ref.watch(apiClientProvider),
    ref.watch(secureStoreProvider),
  ),
);
