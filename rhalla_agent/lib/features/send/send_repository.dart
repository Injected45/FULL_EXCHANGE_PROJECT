import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';

/// مرجع بسيط: دولة أو مدينة أو فرع.
class Ref2 {
  const Ref2(this.id, this.name);
  final int id;
  final String name;

  static int _i(dynamic v) => v is int ? v : int.tryParse('$v') ?? 0;

  factory Ref2.country(Map<String, dynamic> j) =>
      Ref2(_i(j['ID']), '${j['CName'] ?? ''}'.trim());
  factory Ref2.city(Map<String, dynamic> j) =>
      Ref2(_i(j['ID']), '${j['CityName'] ?? ''}'.trim());
  factory Ref2.branch(Map<String, dynamic> j) =>
      Ref2(_i(j['ID']), '${j['BName'] ?? ''}'.trim());
}

/// مسوّدة الحوالة الداخلية — تنتقل بين النموذج والمراجعة.
class TransferDraft {
  const TransferDraft({
    required this.receiverName,
    required this.receiverPhone,
    required this.amount,
    required this.commission,
    required this.city,
    required this.branch,
    required this.currencyId,
    this.notes,
  });

  final String receiverName;
  final String receiverPhone;
  final double amount;
  final double commission;
  final Ref2 city;
  final Ref2 branch;
  final int currencyId;
  final String? notes;

  double get total => amount + commission;
}

/// نتيجة حوالة أُنشئت.
class CreatedTransfer {
  const CreatedTransfer({
    required this.code,
    required this.mobileCode,
    required this.receiverName,
    required this.amount,
    required this.commission,
    required this.insertedAt,
  });

  /// `Code` — الرمز الداخلي الكامل.
  final String code;

  /// `Code_For_mobules` — الرمز الذي يُعطى للمستفيد.
  final String mobileCode;

  final String receiverName;
  final double amount;
  final double commission;
  final String insertedAt;

  /// ما يُعرض ويُشارَك — رمز الموبايل إن وُجد، وإلا الرمز الداخلي.
  String get shareCode => mobileCode.isNotEmpty ? mobileCode : code;

  factory CreatedTransfer.fromJson(Map<String, dynamic> j) => CreatedTransfer(
        code: '${j['Code'] ?? ''}'.trim(),
        // بهذا الإملاء — Code_For_mobules.
        mobileCode: '${j['Code_For_mobules'] ?? ''}'.trim(),
        receiverName: '${j['RecievedName'] ?? ''}'.trim(),
        amount: Fmt.num_(j['OverallVal']),
        commission: Fmt.num_(j['ExVal']),
        insertedAt: '${j['InsertDate'] ?? ''} ${j['InsertTime'] ?? ''}'.trim(),
      );
}

/// تفصيل «رصيد غير كافٍ» كما يعيده الخادم.
///
/// ⚠️ يصل داخل `message` لا `data`: وسائط sendError معكوسة في هذا المسار،
/// فالكائن في message والنص العربي في data. تمرير message إلى Text() يُسقط
/// التطبيق — ولهذا لا يُقرأ إلا من هنا.
class InsufficientFunds {
  const InsufficientFunds({
    required this.wallet,
    required this.amount,
    required this.commission,
    required this.total,
  });

  /// ⚠️ `null` غالباً. استعلام الخادم هو
  /// `wallet::where('Walet','>=',$total)->first()` — أي أنه يعيد الصف فقط
  /// حين يكفي الرصيد، فعند الفشل يكون `wallet = null` دائماً تقريباً.
  /// قراءته صفراً تعني ادّعاء «رصيدك 0.000» وهو غير صحيح.
  final double? wallet;

  final double amount;
  final double commission;
  final double total;

  /// النقص — لا يُحسب إلا حين يعرف الخادم الرصيد فعلاً.
  double? get shortfall => wallet == null ? null : total - wallet!;

  /// نص عربي أمين: لا يذكر رصيداً لا نملكه.
  String describe() {
    final w = wallet;
    if (w == null) {
      return 'الرصيد لا يغطّي ${Fmt.money(total)} '
          '(${Fmt.money(amount)} + عمولة ${Fmt.money(commission)}).';
    }
    return 'رصيدك ${Fmt.money(w)} والمطلوب ${Fmt.money(total)} — '
        'ينقص ${Fmt.money(total - w)}.';
  }

  static InsufficientFunds? from(ApiFailure e) {
    final m = e.envelope?.messageMap;
    if (m == null) return null;
    final hasShape = m.containsKey('wallet') || m.containsKey('total');
    if (!hasShape) return null;
    final w = m['wallet'];
    return InsufficientFunds(
      // الخادم يعيد الصف كاملاً حين يكفي الرصيد، و null حين لا يكفي.
      wallet: w == null
          ? null
          : (w is Map ? Fmt.num_(w['Walet']) : Fmt.num_(w)),
      amount: Fmt.num_(m['amount']),
      commission: Fmt.num_(m['Commission'] ?? m['commission']),
      total: Fmt.num_(m['total']),
    );
  }
}

class SendRepository {
  SendRepository(this._api);

  final ApiClient _api;

  /// ليبيا. `IsMain = 1` في CountiresTb.
  static const libyaId = 1;

  /// ⚠️ النقطة **تستبعد** المعرّف المُرسل — إنها «الدول الأخرى».
  /// نمرّر 0 لنحصل على الكل.
  Future<List<Ref2>> countries() async {
    final env = await _api.post('/device/countries', body: {'country_id': 0});
    return env.rows.map(Ref2.country).toList();
  }

  Future<List<Ref2>> cities({int countryId = libyaId}) async {
    final env = await _api.post('/device/cities',
        body: {'country_id': countryId, 'exclude_city_id': 0});
    return env.rows.map(Ref2.city).toList();
  }

  Future<List<Ref2>> branches() async {
    final env = await _api.get('/device/exchange/CoBranch_select_get');
    return env.rows.map(Ref2.branch).toList();
  }

  /// إنشاء حوالة داخلية.
  ///
  /// `country_id` و `AccID` **يُتحقق منهما ثم يُهملان** في الخادم — نرسلهما
  /// لأن التحقق يفشل بدونهما، لا لأنهما يؤثّران.
  ///
  /// للوكيل (UeserType 3 و 5): العمولة تُقبل كما تُرسل بلا حساب من الخادم،
  /// وفحص رصيد المحفظة يُتخطّى، ويُطبَّق سقف الفرع بدلاً منه.
  Future<CreatedTransfer> createInternal({
    required TransferDraft d,
    required int accId,
    String? senderName,
    String? senderPhone,
  }) async {
    final env = await _api.post('/device/internal/exchange', body: {
      'country_id': libyaId,
      'AccID': accId,
      'reviced_name': d.receiverName.trim(),
      'reviced_phone': Fmt.phoneForApi(d.receiverPhone),
      'currency_id': d.currencyId,
      'amount': d.amount,
      'branch_id': d.branch.id,
      'city_id': d.city.id,
      'Commition': d.commission,
      if (senderName != null && senderName.trim().isNotEmpty)
        'SenderName': senderName.trim(),
      if (senderPhone != null && senderPhone.trim().isNotEmpty)
        'SPhone1': senderPhone.trim(),
      if (d.notes != null && d.notes!.trim().isNotEmpty) 'Notes': d.notes!.trim(),
    });

    final payload = env.payload;
    if (payload is Map && payload['transfer'] is Map) {
      return CreatedTransfer.fromJson(
          (payload['transfer'] as Map).cast<String, dynamic>());
    }
    // الحوالة قد تكون أُنشئت رغم اختلاف شكل الرد — لا نزعم الفشل.
    throw ApiFailure(
      'تمّت العملية لكن رد الخادم غير متوقّع. راجع قائمة الحوالات قبل إعادة الإرسال.',
      statusCode: env.statusCode,
      envelope: env,
    );
  }
}

final sendRepositoryProvider =
    Provider<SendRepository>((ref) => SendRepository(ref.watch(apiClientProvider)));

final citiesProvider = FutureProvider.autoDispose<List<Ref2>>(
    (ref) => ref.watch(sendRepositoryProvider).cities());

final branchesProvider = FutureProvider.autoDispose<List<Ref2>>(
    (ref) => ref.watch(sendRepositoryProvider).branches());
