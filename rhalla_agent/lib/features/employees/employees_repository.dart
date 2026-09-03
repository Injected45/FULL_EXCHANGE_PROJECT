import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/net/api_client.dart';

/// إدارة الموظفين ونقاط البيع — طبقة بيانات.
///
/// ⚠ **لا شيء هنا يمسّ المال.** لا رصيد يُقرأ ولا حوالة تُنشأ ولا قيد يُكتب.
/// هذه إدارةُ من يعمل وما يُسمح له، لا إدارةُ مال — والخطّ الأحمر الذي
/// وضعه المالك (3 سبتمبر 2026) يمرّ من هنا: أي نداء في هذا الملف يمكن أن
/// يحرّك ديناراً هو خطأ لا ميزة.
///
/// والصلاحيات تُقرأ ولا تُشتقّ: الخادم يقول ما مُنح، والتطبيق يعرض ويخفي.
/// إخفاء زرٍّ ليس حماية — الرفض الحقيقي عند كل نداء في الخادم.

/// نقطة بيع كما هي في `AuthorizedUsers` — الجدول القائم في المنظومة.
class PosRef {
  const PosRef({required this.id, required this.name, this.isPrimary = false});

  final int id;
  final String name;
  final bool isPrimary;

  static PosRef fromJson(Map<String, dynamic> j) => PosRef(
        id: int.tryParse('${j['id'] ?? j['point_of_sale_id'] ?? 0}') ?? 0,
        name: '${j['name'] ?? ''}'.trim(),
        isPrimary: j['is_primary'] == 1 || j['is_primary'] == true,
      );
}

/// حالة حساب الموظف — النصّ العربي مأخوذ من مستند المالك حرفياً.
enum EmployeeStatus {
  pendingActivation('PENDING_ACTIVATION', 'بانتظار التفعيل'),
  active('ACTIVE', 'نشط'),
  suspended('SUSPENDED', 'موقوف مؤقتاً'),
  disabled('DISABLED', 'معطل'),
  requiresReactivation('REQUIRES_REACTIVATION', 'بحاجة لإعادة تفعيل'),
  compromised('COMPROMISED', 'تم إيقاف التفعيل أمنياً');

  const EmployeeStatus(this.wire, this.label);
  final String wire;
  final String label;

  static EmployeeStatus of(String? raw) => EmployeeStatus.values.firstWhere(
        (e) => e.wire == raw,
        orElse: () => EmployeeStatus.pendingActivation,
      );
}

class Employee {
  const Employee({
    required this.id,
    required this.fullName,
    required this.phone,
    required this.status,
    this.lastLoginAt = '',
    this.lastActivityAt = '',
    this.activatedAt = '',
    this.deviceId,
    this.devicePlatform = '',
    this.deviceModel = '',
    this.deviceActivatedAt = '',
    this.pointsOfSale = const [],
    this.permissions = const [],
  });

  final int id;
  final String fullName;
  final String phone;
  final EmployeeStatus status;

  final String lastLoginAt;
  final String lastActivityAt;
  final String activatedAt;

  /// الجهاز الفعّال — null يعني لا جهاز مربوط الآن.
  final int? deviceId;
  final String devicePlatform;
  final String deviceModel;
  final String deviceActivatedAt;

  final List<PosRef> pointsOfSale;
  final List<String> permissions;

  bool get hasDevice => deviceId != null;

  /// «يحتاج كوداً من الإدارة» — الحالات التي لا يستطيع فيها الموظف الدخول
  /// حتى يُصدر له الوكيل كوداً جديداً.
  bool get needsCode =>
      status == EmployeeStatus.pendingActivation ||
      status == EmployeeStatus.requiresReactivation ||
      status == EmployeeStatus.compromised;

  String get posLabel => pointsOfSale.isEmpty
      ? 'بلا نقطة بيع'
      : pointsOfSale.map((p) => p.name).where((s) => s.isNotEmpty).join(' · ');

  static Employee fromJson(Map<String, dynamic> j) => Employee(
        id: int.tryParse('${j['id'] ?? 0}') ?? 0,
        fullName: '${j['full_name'] ?? ''}'.trim(),
        phone: '${j['phone'] ?? ''}'.trim(),
        status: EmployeeStatus.of('${j['status'] ?? ''}'.trim()),
        lastLoginAt: '${j['last_login_at'] ?? ''}'.trim(),
        lastActivityAt: '${j['last_activity_at'] ?? ''}'.trim(),
        activatedAt: '${j['activated_at'] ?? ''}'.trim(),
        deviceId: j['device_id'] == null
            ? null
            : int.tryParse('${j['device_id']}'),
        devicePlatform: '${j['platform'] ?? ''}'.trim(),
        deviceModel: '${j['model'] ?? ''}'.trim(),
        deviceActivatedAt: '${j['device_activated_at'] ?? ''}'.trim(),
        pointsOfSale: ((j['points_of_sale'] as List?) ?? const [])
            .whereType<Map>()
            .map((e) => PosRef.fromJson(e.cast<String, dynamic>()))
            .toList(),
        permissions: ((j['permissions'] as List?) ?? const [])
            .map((e) => '$e')
            .toList(),
      );
}

/// جهاز مفعّل كما تعرضه شاشة «الأجهزة المفعّلة».
class EmployeeDevice {
  const EmployeeDevice({
    required this.id,
    required this.employeeName,
    required this.phone,
    required this.status,
    this.pointOfSale = '',
    this.platform = '',
    this.model = '',
    this.deviceRef = '',
    this.activatedAt = '',
    this.lastActivityAt = '',
  });

  final int id;
  final String employeeName;
  final String phone;

  /// ACTIVE · REVOKED · REPLACED
  final String status;

  final String pointOfSale;
  final String platform;
  final String model;

  /// آخر 8 خانات من تجزئة المعرّف — تكفي للتمييز ولا تكشف الجهاز.
  final String deviceRef;

  final String activatedAt;
  final String lastActivityAt;

  bool get isActive => status == 'ACTIVE';

  String get statusLabel => switch (status) {
        'ACTIVE' => 'مفعّل',
        'REVOKED' => 'ملغى',
        'REPLACED' => 'مُستبدَل',
        _ => status,
      };

  static EmployeeDevice fromJson(Map<String, dynamic> j) => EmployeeDevice(
        id: int.tryParse('${j['id'] ?? 0}') ?? 0,
        employeeName: '${j['full_name'] ?? ''}'.trim(),
        phone: '${j['phone'] ?? ''}'.trim(),
        status: '${j['status'] ?? ''}'.trim(),
        pointOfSale: '${j['point_of_sale'] ?? ''}'.trim(),
        platform: '${j['platform'] ?? ''}'.trim(),
        model: '${j['model'] ?? ''}'.trim(),
        deviceRef: '${j['device_ref'] ?? ''}'.trim(),
        activatedAt: '${j['activated_at'] ?? ''}'.trim(),
        lastActivityAt: '${j['last_activity_at'] ?? ''}'.trim(),
      );
}

/// مجموعة صلاحيات في شاشة المنح.
class PermissionGroup {
  const PermissionGroup({
    required this.key,
    required this.name,
    required this.items,
  });

  final String key;
  final String name;
  final List<PermissionItem> items;

  static PermissionGroup fromJson(Map<String, dynamic> j) => PermissionGroup(
        key: '${j['group'] ?? ''}',
        name: '${j['name'] ?? ''}',
        items: ((j['items'] as List?) ?? const [])
            .whereType<Map>()
            .map((e) => PermissionItem.fromJson(e.cast<String, dynamic>()))
            .toList(),
      );
}

class PermissionItem {
  const PermissionItem({required this.key, required this.label});

  final String key;
  final String label;

  static PermissionItem fromJson(Map<String, dynamic> j) => PermissionItem(
        key: '${j['key'] ?? ''}',
        label: '${j['label'] ?? ''}',
      );
}

class EmployeesRepository {
  EmployeesRepository(this._api);

  final ApiClient _api;

  Future<List<Employee>> list() async {
    final env = await _api.get('/employees');
    return env.rows.map(Employee.fromJson).toList();
  }

  Future<List<PosRef>> pointsOfSale() async {
    final env = await _api.get('/employees/points-of-sale');
    return env.rows.map(PosRef.fromJson).toList();
  }

  Future<List<PermissionGroup>> permissionCatalog() async {
    final env = await _api.get('/employees/permissions/catalog');
    return env.rows.map(PermissionGroup.fromJson).toList();
  }

  Future<List<EmployeeDevice>> devices() async {
    final env = await _api.get('/employees/devices');
    return env.rows.map(EmployeeDevice.fromJson).toList();
  }

  Future<int> create({
    required String fullName,
    required String phone,
    List<int> pointsOfSale = const [],
    List<String> permissions = const [],
    String? notes,
  }) async {
    final env = await _api.post('/employees', body: {
      'full_name': fullName,
      'phone': phone,
      'points_of_sale': pointsOfSale,
      'permissions': permissions,
      if (notes != null && notes.isNotEmpty) 'notes': notes,
    });
    return int.tryParse('${env.row?['id'] ?? 0}') ?? 0;
  }

  Future<void> update({
    required int id,
    String? fullName,
    List<int>? pointsOfSale,
    String? notes,
  }) async {
    // الخادم يقبل الحقول الموجودة فقط (`sometimes`)، فلا يُرسَل إلا ما تغيّر.
    final body = <String, dynamic>{};
    if (fullName != null) body['full_name'] = fullName;
    if (pointsOfSale != null) body['points_of_sale'] = pointsOfSale;
    if (notes != null) body['notes'] = notes;

    await _api.raw.put('/employees/$id', data: body);
  }

  Future<void> setStatus({required int id, required String status}) =>
      _api.post('/employees/$id/status', body: {'status': status});

  /// يُعيد الكود **نصّاً صريحاً مرّة واحدة**؛ الخادم لا يحفظه كذلك ولا يُعيده
  /// ثانيةً. لذلك تعرضه الشاشة فوراً وتقول للوكيل إنه لن يظهر مرّة أخرى.
  Future<String> issueCode(int id) async {
    final env = await _api.post('/employees/$id/activation-code');
    return '${env.row?['code'] ?? ''}';
  }

  Future<void> setPermissions({
    required int id,
    required List<String> permissions,
  }) async {
    await _api.raw.put('/employees/$id/permissions',
        data: {'permissions': permissions});
  }

  Future<void> revokeDevice(int deviceId) =>
      _api.post('/employees/devices/$deviceId/revoke');
}

final employeesRepositoryProvider = Provider<EmployeesRepository>(
  (ref) => EmployeesRepository(ref.watch(apiClientProvider)),
);

final employeesProvider = FutureProvider.autoDispose<List<Employee>>(
  (ref) => ref.watch(employeesRepositoryProvider).list(),
);

final employeePosProvider = FutureProvider.autoDispose<List<PosRef>>(
  (ref) => ref.watch(employeesRepositoryProvider).pointsOfSale(),
);

final permissionCatalogProvider =
    FutureProvider.autoDispose<List<PermissionGroup>>(
  (ref) => ref.watch(employeesRepositoryProvider).permissionCatalog(),
);

final employeeDevicesProvider = FutureProvider.autoDispose<List<EmployeeDevice>>(
  (ref) => ref.watch(employeesRepositoryProvider).devices(),
);
