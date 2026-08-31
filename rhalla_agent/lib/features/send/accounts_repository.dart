import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';

/// حساب وجهة، كما يعيده البحث بالهاتف.
class AccountRef {
  const AccountRef({
    required this.accId,
    required this.userId,
    required this.name,
    required this.code,
    required this.branchName,
    required this.phone,
  });

  final int accId;
  final int userId;
  final String name;

  /// `AccCode` — رقم الحساب المعروض.
  final String code;
  final String branchName;
  final String phone;

  String get initial => name.isEmpty ? '؟' : name.substring(0, 1);

  static int _i(dynamic v) => v is int ? v : int.tryParse('$v') ?? 0;

  factory AccountRef.fromJson(Map<String, dynamic> j) => AccountRef(
        accId: _i(j['AccID']),
        userId: _i(j['user_id']),
        name: '${j['AccName'] ?? ''}'.trim(),
        code: '${j['AccCode'] ?? ''}'.trim(),
        branchName: '${j['BName'] ?? ''}'.trim(),
        phone: '${j['AccPhone'] ?? ''}'.trim(),
      );
}

/// عمولة التحويل بين الحسابات — يحسبها الخادم، لا الوكيل.
class AccountsFee {
  const AccountsFee({required this.commission, required this.total});

  final double commission;
  final double total;
}

/// مسوّدة تحويل بين الحسابات — تُمرَّر من النموذج إلى شاشة المراجعة.
///
/// العمولة هنا [AccountsFee] لا رقم يحدّده الوكيل: الخادم يحسبها، وقد
/// يرفض المبلغ أصلاً لثغرة في شرائح `Transfer_commissions`. المسوّدة
/// لا تُبنى إلا بعد أن يعيد الخادم عمولة صالحة، فوصولها إلى المراجعة
/// يعني أن الشريحة موجودة.
class AccountsDraft {
  const AccountsDraft({
    required this.target,
    required this.amount,
    required this.fee,
    this.notes,
  });

  final AccountRef target;
  final double amount;
  final AccountsFee fee;
  final String? notes;

  double get commission => fee.commission;
  double get total => fee.total;
}

class AccountsRepository {
  AccountsRepository(this._api);

  final ApiClient _api;

  /// البحث عن حساب الوجهة بالهاتف.
  ///
  /// النقطة **تستبعد المستخدم الحالي** بـ `a.id <> $currentUserId`، فبحث
  /// الوكيل عن رقمه هو يعيد 404 — وهذا سلوك مقصود لا خطأ.
  ///
  /// ⚠️ المطابقة في الخادم **حرفية** على `users.phone`، والعمود غير موحَّد:
  /// 291 صفاً بصفر بادئ و68 صفاً بدونه. فصيغة واحدة تُخفي حسابات موجودة —
  /// بل إن نفس الأرقام قد تشير إلى حسابات مختلفة بين الصيغتين. لذلك نستعلم
  /// بالصيغتين ونوحّد النتيجة بـ AccID.
  Future<List<AccountRef>> searchByPhone(String phone) async {
    final bare = Fmt.phoneForApi(phone);
    if (bare.isEmpty) return const [];

    final forms = <String>{bare, '0$bare'};
    final byAcc = <int, AccountRef>{};

    for (final form in forms) {
      for (final a in await _lookup(form)) {
        byAcc.putIfAbsent(a.accId, () => a);
      }
    }
    return byAcc.values.toList();
  }

  Future<List<AccountRef>> _lookup(String phone) async {
    try {
      final env =
          await _api.post('/device/exchange/account', body: {'phone': phone});
      return env.rows.map(AccountRef.fromJson).toList();
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  /// معاينة العمولة قبل الإرسال.
  ///
  /// ⚠️ شرائح `Transfer_commissions` فيها ثغرات حقيقية — لا شريحة بين
  /// 10,000 و11,000 ولا بين 15,000 و16,000 ولا فوق 100,000 — فيرد الخادم
  /// 422. نُظهر ذلك هنا بدل أن يكتشفه الوكيل بعد ملء النموذج.
  ///
  /// يُعاد `null` حين لا تطابق شريحة.
  Future<AccountsFee?> fee(double amount) async {
    try {
      final env = await _api.post(
          '/device/internal/trans/between/accounts/commission',
          body: {'amount': amount});
      final p = env.payload;
      if (p is Map) {
        return AccountsFee(
          commission: Fmt.num_(p['commission']),
          total: Fmt.num_(p['total']),
        );
      }
      return null;
    } on ApiFailure catch (e) {
      if (e.statusCode == 422) return null;
      rethrow;
    }
  }

  /// دقائق مضت منذ آخر تحويل بين الحسابات. الخادم يمنع قبل مرور 3 دقائق.
  Future<int?> minutesSinceLast(int accId) async {
    try {
      final env = await _api.post('/device/internal/check/between/time',
          body: {'acc_id': accId});
      final rows = env.rows;
      if (rows.isEmpty) return null;
      final v = rows.first['DifferenceInMinutes'];
      return v == null ? null : Fmt.num_(v).round();
    } on ApiFailure {
      return null;
    }
  }

  /// تنفيذ التحويل بين الحسابات.
  ///
  /// خلافاً للحوالة الداخلية والخارجية، **العمولة هنا ليست بيد الوكيل**:
  /// الخادم يقرأها من `Transfer_commissions` ويتجاهل أي قيمة تُرسل.
  /// وفحص رصيد المحفظة يُطبَّق على الوكيل (يُستثنى `UeserType == 5` وحده).
  Future<AccountsTransfer> create({
    required int fromAccId,
    required int toAccId,
    required int currencyId,
    required double amount,
    required int branchId,
    String? notes,
    String receiverPhone = '',
  }) async {
    final env = await _api.post('/device/internal/trans/between/accounts', body: {
      'acc_id': fromAccId,
      'acc_id_to': toAccId,
      'currency_id': currencyId,
      'amount': amount,
      'branch_id': branchId,
      if (notes != null && notes.trim().isNotEmpty) 'Notes': notes.trim(),
    });

    final p = env.payload;
    if (p is Map && p['result'] is Map) {
      return AccountsTransfer.fromJson(
        (p['result'] as Map).cast<String, dynamic>(),
        fallbackPhone: receiverPhone,
      );
    }
    throw ApiFailure(
      'تمّت العملية لكن رد الخادم غير متوقّع. راجع كشف الحساب قبل إعادة الإرسال.',
      statusCode: env.statusCode,
      envelope: env,
    );
  }
}

/// نتيجة تحويل بين حسابين.
class AccountsTransfer {
  const AccountsTransfer({
    required this.code,
    required this.mobileCode,
    required this.senderName,
    required this.receiverName,
    required this.receiverPhone,
    required this.amount,
    required this.commission,
    required this.insertedAt,
  });

  final String code;
  final String mobileCode;
  final String senderName;
  final String receiverName;
  final String receiverPhone;
  final double amount;
  final double commission;
  final String insertedAt;

  String get shareCode => mobileCode.isNotEmpty ? mobileCode : code;

  factory AccountsTransfer.fromJson(
    Map<String, dynamic> j, {
    String fallbackPhone = '',
  }) =>
      AccountsTransfer(
        code: '${j['Code'] ?? ''}'.trim(),
        mobileCode: '${j['codeForMobile'] ?? ''}'.trim(),
        senderName: '${j['senderName'] ?? ''}'.trim(),
        receiverName: '${j['recievedName'] ?? ''}'.trim(),
        // الخادم لا يُرجع هاتف صاحب الحساب هنا — يأتي من الحساب المقصود،
        // والمفضّلة تشترطه.
        receiverPhone: '${j['AccPhone'] ?? ''}'.trim().isEmpty
            ? fallbackPhone
            : '${j['AccPhone']}'.trim(),
        amount: Fmt.num_(j['amount'] ?? j['TransValue']),
        commission: Fmt.num_(j['commission']),
        insertedAt: '${j['InsertDate'] ?? ''} ${j['InsertTime'] ?? ''}'.trim(),
      );
}

final accountsRepositoryProvider = Provider<AccountsRepository>(
    (ref) => AccountsRepository(ref.watch(apiClientProvider)));
