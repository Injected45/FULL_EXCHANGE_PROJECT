import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';

/// سقفُ الموظف وطلباتُ الموافقة — جانبُ الوكيل.
///
/// ⚠ **لا قرارَ هنا.** الشاشةُ تعرض وتُرسل، والخادمُ وحده يقرّر: هل تجاوز
/// الموظف سقفَه؟ ومن يملك البتّ؟ وهل ما زال الطلب صالحاً؟ (البند 47).
///
/// فحتى `canDecide` أدناه ليست حارساً — هي ما يقوله الخادم لتُخفي الشاشةُ
/// زرّاً لا يعمل. والحارسُ الحقيقيّ ردُّ 403 على كل نداء.

/// سببُ تصعيد طلبٍ إلى الوكيل.
class EscalationReason {
  const EscalationReason(this.key, this.label);
  final String key;
  final String label;
}

/// طلبُ موافقةٍ كما يراه الوكيل.
class ApprovalRequest {
  const ApprovalRequest({
    required this.id,
    required this.employeeId,
    required this.employeeName,
    required this.pointOfSale,
    required this.amount,
    required this.currencyCode,
    required this.recipientName,
    required this.recipientPhone,
    required this.reasonLabels,
    required this.limitAtRequest,
    required this.status,
    required this.transferNumber,
    required this.failureReason,
    required this.createdAt,
    required this.expiresAt,
    required this.decidedAt,
  });

  final int id;
  final int employeeId;
  final String employeeName;
  final String? pointOfSale;
  final double amount;
  final String? currencyCode;
  final String? recipientName;
  final String? recipientPhone;

  /// أسبابُ التصعيد مقروءةً — قد تكون أكثر من واحد لطلبٍ واحد.
  final List<String> reasonLabels;

  /// ⚠ سقفُ الموظف **يومَ الطلب** لا سقفُه اليوم: الوكيل قد يكون غيّره
  /// بعده، وعرضُ الجديد يجعل سببَ التصعيد يبدو خاطئاً.
  final double? limitAtRequest;

  final String status;
  final String? transferNumber;
  final String? failureReason;
  /// ⚠ نصّاً كما وصل: `Fmt.stampShort` هي منسّق الوقت في التطبيق كلِّه
  /// وتأخذ نصّاً. وتحويلٌ إلى `DateTime` ثمّ إعادةُ تنسيقه بيدٍ أخرى يعني
  /// وقتاً يُعرض هنا بشكلٍ يخالف بقيّة الشاشات.
  final String? createdAt;
  final String? expiresAt;
  final String? decidedAt;

  bool get isPending => status == 'PENDING';

  /// نصُّ الحالة كما يُعرض.
  String get statusLabel => switch (status) {
        'PENDING'   => 'بانتظار موافقتك',
        // ⚠ الموافقةُ إذنٌ لا تنفيذ: الموظف ينفّذها حين يقبض النقد.
        'APPROVED'  => transferNumber != null
            ? 'وُوفق عليها ونفّذها الموظف'
            : 'وُوفق عليها — بانتظار تنفيذ الموظف',
        'REJECTED'  => 'مرفوضة',
        'EXPIRED'   => 'انتهت صلاحية الطلب',
        'CANCELLED' => 'ألغاها الموظف',
        'FAILED'    => 'وُوفق عليها ولم تُنفَّذ',
        _           => status,
      };

  static ApprovalRequest fromJson(Map<String, dynamic> j) => ApprovalRequest(
        id: int.tryParse('${j['id']}') ?? 0,
        employeeId: int.tryParse('${j['employee_id']}') ?? 0,
        employeeName: '${j['employee_name'] ?? ''}'.trim(),
        pointOfSale: _s(j['point_of_sale']),
        amount: Fmt.num_(j['amount']),
        currencyCode: _s(j['currency_code']),
        recipientName: _s(j['recipient_name']),
        recipientPhone: _s(j['recipient_phone']),
        reasonLabels: (j['reason_labels'] as List?)
                ?.map((e) => '$e')
                .where((e) => e.trim().isNotEmpty)
                .toList() ??
            const [],
        limitAtRequest: j['limit_at_request'] == null
            ? null
            : Fmt.num_(j['limit_at_request']),
        status: '${j['status'] ?? ''}',
        transferNumber: _s(j['transfer_number']),
        failureReason: _s(j['failure_reason']),
        createdAt: _s(j['created_at']),
        expiresAt: _s(j['expires_at']),
        decidedAt: _s(j['decided_at']),
      );

  static String? _s(dynamic v) {
    final s = '${v ?? ''}'.trim();
    return s.isEmpty || s == 'null' ? null : s;
  }
}

/// سياسةُ موظفٍ واحد.
class EmployeeLimits {
  const EmployeeLimits({
    required this.perTransfer,
    required this.cumulative,
    required this.cumulativeHours,
    required this.recipientMinutes,
    required this.approvalTtlHours,
    required this.canEdit,
  });

  /// NULL = بلا سقف. وهو حالُ كل موظفٍ حتى يضع له الوكيل واحداً.
  final double? perTransfer;
  final double? cumulative;
  final int? cumulativeHours;
  final int recipientMinutes;
  final int approvalTtlHours;
  final bool canEdit;

  bool get hasAny => perTransfer != null || cumulative != null;

  static EmployeeLimits fromJson(Map<String, dynamic> j) => EmployeeLimits(
        perTransfer:
            j['per_transfer_limit'] == null ? null : Fmt.num_(j['per_transfer_limit']),
        cumulative:
            j['cumulative_limit'] == null ? null : Fmt.num_(j['cumulative_limit']),
        cumulativeHours: int.tryParse('${j['cumulative_hours'] ?? ''}'),
        recipientMinutes: int.tryParse('${j['recipient_minutes'] ?? ''}') ?? 60,
        approvalTtlHours: int.tryParse('${j['approval_ttl_hours'] ?? ''}') ?? 24,
        canEdit: j['can_edit'] == true,
      );
}

class ApprovalsRepository {
  ApprovalsRepository(this._api);
  final ApiClient _api;

  Future<List<ApprovalRequest>> list({
    String status = 'ALL',
    int? employeeId,
    String? query,
  }) async {
    final env = await _api.get('/employees/approvals', query: {
      'status': status,
      'employee_id': employeeId,
      if (query != null && query.trim().isNotEmpty) 'q': query.trim(),
      'limit': 100,
    });

    final rows = (env.row?['requests'] as List?) ?? const [];
    return rows
        .map((e) => ApprovalRequest.fromJson((e as Map).cast<String, dynamic>()))
        .toList();
  }

  /// عددُ المعلَّق — للشارة. سؤالٌ رخيصٌ يتكرّر كل ربع دقيقة.
  Future<int> pendingCount() async {
    final env = await _api.get('/employees/approvals/count');
    return int.tryParse('${env.row?['pending'] ?? 0}') ?? 0;
  }

  /// يُعيد رسالةَ الخادم — وهي التي تقول أنُفِّذت الحوالة أم رُدّت ولماذا.
  Future<String> approve(int id, {String? note}) async {
    final env = await _api.post('/employees/approvals/$id/approve',
        body: {if (note != null && note.trim().isNotEmpty) 'note': note.trim()});
    // ⚠ رسالةُ الخادم كما هي — «نُفِّذت» أو «رصيد غير كافٍ» أو «قرارٌ سبق».
    return env.messageText ?? 'تم';
  }

  Future<String> reject(int id, {String? note}) async {
    final env = await _api.post('/employees/approvals/$id/reject',
        body: {if (note != null && note.trim().isNotEmpty) 'note': note.trim()});
    // ⚠ رسالةُ الخادم كما هي — «نُفِّذت» أو «رصيد غير كافٍ» أو «قرارٌ سبق».
    return env.messageText ?? 'تم';
  }

  Future<EmployeeLimits> limits(int employeeId) async {
    final env = await _api.get('/employees/$employeeId/limits');
    return EmployeeLimits.fromJson(env.row ?? const {});
  }

  /// ⚠ `null` يعني «بلا سقف» صراحةً، لا «لا تغيّر». فالحقول تُرسل كلُّها
  /// في كل حفظ — وإلّا صار محوُ سقفٍ مستحيلاً من الواجهة.
  Future<void> saveLimits(
    int employeeId, {
    required double? perTransfer,
    required double? cumulative,
    required int? cumulativeHours,
    required int recipientMinutes,
    required int approvalTtlHours,
  }) async {
    await _api.put('/employees/$employeeId/limits', body: {
      'per_transfer_limit': perTransfer,
      'cumulative_limit': cumulative,
      'cumulative_hours': cumulativeHours,
      'recipient_minutes': recipientMinutes,
      'approval_ttl_hours': approvalTtlHours,
    });
  }
}

final approvalsRepositoryProvider = Provider<ApprovalsRepository>(
    (ref) => ApprovalsRepository(ref.watch(apiClientProvider)));

/// قائمةُ الطلبات لتبويبٍ واحد.
final approvalsProvider = FutureProvider.autoDispose
    .family<List<ApprovalRequest>, String>(
        (ref, status) => ref.watch(approvalsRepositoryProvider).list(status: status));

/// سقفُ موظفٍ بعينه.
final employeeLimitsProvider =
    FutureProvider.autoDispose.family<EmployeeLimits, int>(
        (ref, id) => ref.watch(approvalsRepositoryProvider).limits(id));
