import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';

/// حوالة واردة إلى فرع الوكيل، بانتظار التسليم.
///
/// الأعمدة كما يعيدها الـ view فعلاً — رُصدت من الخادم لا من الوثائق:
/// Code · BName · BranchDeliveredID · BranchRecievedID · CaseStauts ·
/// ExVal · InsertDate · OverallVal · RecievedName · RPhone · SenderName · SendStatus
class IncomingTransfer {
  const IncomingTransfer({
    required this.code,
    required this.receiverName,
    required this.receiverPhone,
    required this.senderName,
    required this.amount,
    required this.commission,
    required this.branchName,
    required this.insertedAt,
    required this.status,
  });

  final String code;
  final String receiverName;
  final String receiverPhone;
  final String senderName;

  /// `OverallVal` — المبلغ المُرسل.
  final double amount;

  /// `ExVal` — العمولة.
  final double commission;

  final String branchName;
  final String insertedAt;
  final String status;

  factory IncomingTransfer.fromJson(Map<String, dynamic> j) => IncomingTransfer(
        code: '${j['Code'] ?? ''}'.trim(),
        receiverName: '${j['RecievedName'] ?? ''}'.trim(),
        // بهذا الإملاء في الـ view — RPhone لا RPhone1.
        receiverPhone: '${j['RPhone'] ?? ''}'.trim(),
        senderName: '${j['SenderName'] ?? ''}'.trim(),
        amount: Fmt.num_(j['OverallVal']),
        commission: Fmt.num_(j['ExVal']),
        branchName: '${j['BName'] ?? ''}'.trim(),
        insertedAt: '${j['InsertDate'] ?? ''}'.trim(),
        status: '${j['SendStatus'] ?? ''}'.trim(),
      );

  /// بحث محلي — الخادم لا يوفّر بحثاً ولا ترقيم صفحات.
  bool matches(String q) {
    if (q.isEmpty) return true;
    final n = q.trim();
    return code.contains(n) ||
        receiverPhone.contains(n) ||
        receiverName.contains(n) ||
        senderName.contains(n);
  }
}

class TransfersRepository {
  TransfersRepository(this._api);

  final ApiClient _api;

  /// الواردة إلى الفرع وبانتظار التسليم.
  ///
  /// ⚠️ بلا ترقيم صفحات: رُصد **522 صفاً** على حساب اختباري واحد.
  /// نقتطع للعرض ونبحث محلياً حتى يوفّر الخادم ترقيماً.
  Future<List<IncomingTransfer>> incoming() async {
    try {
      final env = await _api
          .post('/device/exchange/InternalEx_SelectType_View_not_coustmers_get');
      return env.rows.map(IncomingTransfer.fromJson).toList();
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  /// **ما سلّمه هذا الحساب** — لا ما أرسله.
  ///
  /// ⚠️ اسم النقطة يوحي بـ«الصادرة» وهو مضلِّل. الـ view يرشّح بـ
  /// `ACCID_FRom = $user->AccID`، و`ACCID_FRom` **لا يكتبه الإدراج**: يكتبه
  /// `InternalEx_costimer` — أي نقطة **التسليم** — بحساب من سلَّم
  /// ([:1343](../../../../backend/app/Http/Controllers/Api/depositController.php:1343)).
  ///
  /// تحقّقنا على القاعدة الحيّة: الـ 141 صفاً الوحيدة التي تحمل `ACCID_FRom`
  /// كلها `AccFrom = 0` و`Type_Moble_costimer = 1`. وحساب وكيل أنشأ حوالات
  /// فعلاً يعيد **صفر صفوف** هنا.
  ///
  /// والـ view يرشّح كذلك بـ `ConfirmType = 2` (مسلَّمة)، فالنتيجة
  /// **سِجل تسليم** لا قائمة صادرة. لعرض ما أرسله الوكيل، الكشف
  /// (`ExchangeAccData`) هو المصدر — لا هذه.
  Future<List<IncomingTransfer>> delivered() async {
    try {
      final env = await _api
          .post('/device/exchange/InternalEx_SelectType_View_statetosForok');
      return env.rows.map(IncomingTransfer.fromJson).toList();
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  /// تسليم حوالة بالرمز.
  ///
  /// الخادم يقفل الإدخال 15 دقيقة بعد 5 رموز خاطئة (WrongCodeAttempts)،
  /// ويرد 409 إن كانت مُسلَّمة سلفاً.
  Future<String> deliver({required String code, String? notes}) async {
    final env = await _api.post('/device/exchange/InternalEx_costimer', body: {
      'Code': code,
      if (notes != null && notes.trim().isNotEmpty) 'Notes': notes.trim(),
    });
    return env.displayMessage('تم تسليم الحوالة.');
  }
}

final transfersRepositoryProvider = Provider<TransfersRepository>(
  (ref) => TransfersRepository(ref.watch(apiClientProvider)),
);

final incomingTransfersProvider =
    FutureProvider.autoDispose<List<IncomingTransfer>>(
  (ref) => ref.watch(transfersRepositoryProvider).incoming(),
);

/// سِجل ما سلّمه هذا الحساب — يُطلب فقط حين يفتح المستخدم التبويب،
/// فقائمة الانتظار وحدها بلغت 522 صفاً ولا داعي لجلب الاثنتين معاً.
final deliveredTransfersProvider =
    FutureProvider.autoDispose<List<IncomingTransfer>>(
  (ref) => ref.watch(transfersRepositoryProvider).delivered(),
);
