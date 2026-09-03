import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';

class Limits {
  const Limits({this.daily = 0, this.weekly = 0, this.monthly = 0, this.annual = 0});

  final double daily;
  final double weekly;
  final double monthly;
  final double annual;

  /// الرد يأتي تحت المفتاح `datat` لا `data` — انظر Envelope.
  /// وأسماء الحقول كما يكتبها الخادم: Daily, monthly, Annual, Weekly.
  factory Limits.fromJson(Map<String, dynamic> j) => Limits(
        daily: Fmt.num_(j['Daily']),
        weekly: Fmt.num_(j['Weekly']),
        monthly: Fmt.num_(j['monthly']),
        annual: Fmt.num_(j['Annual']),
      );
}

class Movement {
  const Movement({
    required this.title,
    required this.date,
    required this.amount,
    required this.isCredit,
    required this.balance,
    this.code = '',
    this.time = '',
    this.deliveryStatus = '',
    this.agentStatus = '',
    this.agentCoreType,
    this.isCommissionRow = false,
    this.commission,
  });

  final String title;
  final String date;
  final double amount;
  final bool isCredit;
  final double balance;

  /// رقم الحوالة (`ISID` في حركة الخزنة = `Code` في الحوالة).
  final String code;

  /// وقت الحوالة الأصل بدقّته. `InsertDate` في حركة الخزنة تاريخٌ بلا وقت،
  /// والوقت يأتي من `InternalEx`.
  final String time;

  /// «مسلمه» / «غير مسلمه» كما تقولها المنظومة.
  ///
  /// **لم تعد تُعرَض في «آخر العمليات»** بقرار المالك (2 سبتمبر 2026): تلك
  /// حالة الحوالة بين الوكيل والرحالة، لا حالة تسليمها للمستفيد. تبقى في
  /// النموذج لأن الخادم يرسلها ولأن سياقات أخرى قد تحتاجها.
  final String deliveryStatus;

  /// حالة التسليم من **دفتر الوكيل** — وهي ما يُعرض.
  ///
  /// `PENDING_DELIVERY` أو `DELIVERED`، وهي نفسها التي تبني تبويبات شاشة
  /// «الحوالات الواردة». فارغة لحركةٍ ليست في ذلك الدفتر — حوالةٍ صادرة
  /// مثلاً، فهي لا تُسلَّم من هذا الوكيل أصلاً.
  final String agentStatus;

  /// مرآة `InternalEx.ConfirmType` داخل الدفتر — منها يُعرف الإلغاء وحده.
  final int? agentCoreType;

  /// 3 و4 «قيد الإلغاء» · 5 «ملغية» · 6 «ملغية مسلمة».
  ///
  /// «قيد الإلغاء» محسوبةٌ منها كما في شاشة الحوالات الواردة تماماً: طلب
  /// الإلغاء وحده يكفي لإيقاف يد الوكيل عن الدفع.
  bool get isCancelledByCore =>
      agentCoreType != null && const [3, 4, 5, 6].contains(agentCoreType);

  /// وسم الحالة كما تقوله شاشة «الحوالات الواردة» — أو فارغ إن لم تكن
  /// الحركة في دفتر التسليم.
  ///
  /// الترتيب مطابق لقواعد تبويبات تلك الشاشة حرفياً: «تم التسليم» تسبق
  /// «ملغاة» لأن الوكيل إن كان قد سلّم فقد سلّم، وإلغاءٌ لاحق لا يمحو ذلك.
  String get agentBadge {
    if (agentStatus == 'DELIVERED') return 'تم التسليم';
    if (isCancelledByCore) return 'ملغاة';
    if (agentStatus == 'PENDING_DELIVERY') return 'بانتظار التسليم';
    return '';
  }

  /// حركة ليست حوالة (عمولة، إقفال ميزانية) — لا رقم لها ولا حالة تسليم.
  bool get isTransfer => code.isNotEmpty && !isCommission;

  /// هذا الصفّ **هو** عمولة — يقولها الخادم لا التطبيق.
  ///
  /// حسمُها في الخادم يجعل تعريف العمولة واحداً: ما يخفيه التطبيق هو نفسه
  /// ما يجمعه الخادم في `CommissionAmount`. تعريفان يفترقان عند أول حالة.
  final bool isCommissionRow;

  /// عمولة هذه الحوالة، مجموعةً من صفوف العمولة التي تحمل رقمها.
  ///
  /// **null تعني «لم تصل بعد»، و0.0 تعني «بلا عمولة»** — والفرق مقصود
  /// (بند 22): عرضُ صفرٍ قبل وصول البيانات يقول للوكيل ما لا نعرفه.
  final double? commission;

  /// عمولة التحويل.
  ///
  /// تحمل رقم الحوالة الأمّ، فحالةُ تلك الحوالة تنعكس عليها ويظهر على
  /// العمولة وسم «غير مسلمه» — وهو مربك: العمولة خُصمت فعلاً ولا تُسلَّم
  /// لأحد. لذلك تُميَّز بنوعها لا بحالة أمّها.
  ///
  /// يُقدَّم قول الخادم على تخمين النصّ، ويبقى النصّ احتياطاً لردٍّ قديم.
  bool get isCommission => isCommissionRow || title.contains('عمولة');

  /// هل لهذه الحوالة عمولة فعليّة؟
  bool get hasCommission => (commission ?? 0) > 0;

  /// إجمالي العملية = قيمة الحوالة + العمولة (بند 17).
  ///
  /// ⚠ لا يحلّ محلّ قيمة الحوالة ولا يُعرض مكانها: القيمة الأصلية تبقى هي
  /// الرقم الرئيسي، وهذا رقمٌ ثانٍ بجانبه.
  double get operationTotal => amount + (commission ?? 0);

  bool get isDelivered => deliveryStatus.contains('مسلمه') &&
      !deliveryStatus.contains('غير');

  /// أعمدة كشف الحساب — لاحظ `Balnce` بهذا الإملاء.
  ///
  /// ⚠️ `Type_from` **نص لا رقم**: يعيده الخادم كـ 'ايداع' أو 'خصم'
  /// (CASE على AccDmType و Debit/Credit في depositController)،
  /// و`Values_to` هو المبلغ المطلق أياً كان الاتجاه.
  /// قراءة Type_from كمبلغ تجعل كل حركة تبدو واردة.
  factory Movement.fromJson(Map<String, dynamic> j) {
    final type = '${j['Type_from'] ?? ''}'.trim();
    // متسامح مع «ايداع» و«إيداع».
    final isCredit = type.contains('يداع');
    return Movement(
      title: '${j['MovementType'] ?? ''}'.trim(),
      date: '${j['InsertDate'] ?? ''}'.trim(),
      amount: Fmt.num_(j['Values_to']),
      isCredit: isCredit,
      balance: Fmt.num_(j['Balnce']),
      code: '${j['Code'] ?? ''}'.trim(),
      time: '${j['TransTime'] ?? ''}'.trim(),
      deliveryStatus: '${j['DeliveryStatus'] ?? ''}'.trim(),
      agentStatus: '${j['AgentStatus'] ?? ''}'.trim(),
      agentCoreType: _intOrNull(j['AgentCoreType']),
      isCommissionRow: '${j['IsCommission'] ?? ''}' == '1',
      // غياب المفتاح = خادمٌ قديم لم يُحدَّث بعد ⇦ null «لم يصل»، لا 0.
      commission:
          j.containsKey('CommissionAmount') ? Fmt.num_(j['CommissionAmount']) : null,
    );
  }

  /// `core_confirm_type` قد يصل رقماً أو نصّاً — القراءة الخام من SQL Server
  /// لا تضمن نوعاً، وقاعدة المشروع أن يكون التحليل متسامحاً.
  static int? _intOrNull(dynamic v) {
    if (v == null) return null;
    if (v is int) return v;
    return int.tryParse('$v'.trim());
  }
}

class HomeSnapshot {
  const HomeSnapshot({
    required this.balance,
    required this.limits,
    required this.movements,
  });

  final double balance;
  final Limits limits;
  final List<Movement> movements;
}

class HomeRepository {
  HomeRepository(this._api);

  final ApiClient _api;

  /// العملة المحلية. المعرّف يجب تأكيده من المكتب الخلفي — مفترض 1.
  static const localCurrencyId = 1;

  Future<double> balance() async {
    final env = await _api.post('/device/current/balance/local/currency',
        body: {'currency_id': localCurrencyId});
    // يعيد مصفوفة من كائن واحد، لا رقماً.
    final row = env.row;
    return Fmt.num_(row?['Walet']);
  }

  Future<Limits> limits() async {
    try {
      final env = await _api.post('/device/Daily_transfer', body: {});
      final row = env.row;
      return row == null ? const Limits() : Limits.fromJson(row);
    } on ApiFailure catch (e) {
      // 404 هنا تعني «لا توجد بيانات» لا خطأً.
      if (e.isEmptyResult) return const Limits();
      rethrow;
    }
  }

  Future<List<Movement>> movements({int take = 5}) async {
    try {
      final env = await _api.get('/device/local/account/statment');
      // لا ترقيم صفحات في الخادم — يعيد التاريخ كاملاً. نقتطع محلياً.
      return env.rows.take(take).map(Movement.fromJson).toList();
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  Future<HomeSnapshot> load() async {
    final results = await Future.wait([
      balance(),
      limits(),
      movements(),
    ]);
    return HomeSnapshot(
      balance: results[0] as double,
      limits: results[1] as Limits,
      movements: results[2] as List<Movement>,
    );
  }
}

final homeRepositoryProvider =
    Provider<HomeRepository>((ref) => HomeRepository(ref.watch(apiClientProvider)));

final homeSnapshotProvider = FutureProvider.autoDispose<HomeSnapshot>(
  (ref) => ref.watch(homeRepositoryProvider).load(),
);
