import 'package:flutter/widgets.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';

enum PosState {
  /// مسجّلة وتعمل.
  active,

  /// أُنشئت لكنها لم تُسجّل بعد — Reg = 'NO'.
  awaitingRegistration,

  /// موقوفة — IsActive = 0.
  suspended,
}

/// نقطة بيع (مخوّل) تابعة للوكيل.
///
/// أعمدة AuthorizedUsersgetByBranch كما رُصدت:
/// ID · Name_post · CreatedDate · IsActive · BranchID · UserID · InsertUserID · AccID · phone
class PointOfSale {
  const PointOfSale({
    required this.id,
    required this.name,
    required this.phone,
    required this.userId,
    required this.branchId,
    required this.accId,
    required this.isActive,
    required this.createdAt,
    this.registered,
  });

  final int id;
  final String name;
  final String phone;
  final int userId;
  final int branchId;
  final int accId;
  final bool isActive;
  final String createdAt;

  /// من جدول users لا من AuthorizedUsers — الـ view لا يعيده،
  /// فيبقى null حتى نجلبه من مكان آخر.
  final bool? registered;

  PosState get state {
    if (!isActive) return PosState.suspended;
    if (registered == false) return PosState.awaitingRegistration;
    return PosState.active;
  }

  String get initial => name.trim().isEmpty ? '؟' : name.trim().characters.first;

  static int _int(dynamic v) {
    if (v is int) return v;
    if (v is num) return v.toInt();
    return int.tryParse('$v') ?? 0;
  }

  static bool _bool(dynamic v) {
    if (v is bool) return v;
    final s = '$v'.trim().toLowerCase();
    return s == '1' || s == 'true' || s == 'yes';
  }

  factory PointOfSale.fromJson(Map<String, dynamic> j) => PointOfSale(
        id: _int(j['ID']),
        name: '${j['Name_post'] ?? ''}'.trim(),
        phone: '${j['phone'] ?? ''}'.trim(),
        userId: _int(j['UserID']),
        branchId: _int(j['BranchID']),
        accId: _int(j['AccID']),
        isActive: _bool(j['IsActive']),
        createdAt: '${j['CreatedDate'] ?? ''}'.trim(),
        registered: j['Reg'] == null ? null : '${j['Reg']}'.toLowerCase() == 'yes',
      );
}

class PosRepository {
  PosRepository(this._api);

  final ApiClient _api;

  /// ⚠️ يعيد كل مخوّلي **الفرع**، لا المرشّحين بحساب الوكيل —
  /// فوكلاء الفرع الواحد يرون قوائم بعضهم. سلوك الخادم، لا خطأ هنا.
  Future<List<PointOfSale>> list() async {
    try {
      final env = await _api.post('/device/AuthorizedUsersgetByBranch');
      return env.rows.map(PointOfSale.fromJson).toList();
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  Future<void> add({required String name, required String phone}) async {
    await _api.post('/device/AuthorizedUsers_Add', body: {
      'Name': name.trim(),
      'phone': phone.trim(),
    });
  }

  /// ⚠️ الخادم يعيد `Reg` إلى 'NO' في **كل** تعديل — حتى تبديل IsActive وحده.
  /// أي أن تصحيح اسم يُخرج نقطة البيع ويجبرها على التسجيل من جديد.
  /// لا تستدعِ هذه إلا بعد تحذير صريح للمستخدم.
  Future<void> update({
    required int id,
    required String name,
    required String phone,
    required bool isActive,
  }) async {
    await _api.post('/device/AuthorizedUsers_update', body: {
      'ID': id,
      'Name': name.trim(),
      'phone': phone.trim(),
      'IsActive': isActive,
    });
  }
}

final posRepositoryProvider =
    Provider<PosRepository>((ref) => PosRepository(ref.watch(apiClientProvider)));

final posListProvider = FutureProvider.autoDispose<List<PointOfSale>>(
  (ref) => ref.watch(posRepositoryProvider).list(),
);
