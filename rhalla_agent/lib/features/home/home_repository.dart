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
    this.coreConfirmType,
    this.executedBy = '',
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

  /// رقم حالة الحوالة في منظومة الرحالة (`InternalEx.ConfirmType`).
  ///
  /// الرقم لا الاسم هو العقد: الجدول فيه اسمان متطابقان لرقمين مختلفين
  /// (3 و4 كلاهما «قيد الإلغاء»)، والاسم نصٌّ قد يُحرَّر. فالتلوين والترشيح
  /// على الرقم، والعرض بالاسم كما كتبته المنظومة.
  final int? coreConfirmType;

  /// عائلة الحالة — أربعُ عائلات تجمع إحدى عشرة حالة بلا أن تمحو أيّاً منها.
  ///
  /// العائلة تحكم اللون والأيقونة والترشيح **فقط**؛ والنصّ المعروض يبقى اسم
  /// الحالة كما هو في `InternalEx_Stautes`. اختصارُ الأسماء إلى أربعة كان
  /// سيُخفي عن الوكيل فرقاً حقيقياً — «مرسلة مع مندوب» ليست «غير مسلمه».
  CoreStage get stage => switch (coreConfirmType) {
        0 => CoreStage.pending,
        1 || 7 || 8 || 9 => CoreStage.onWay,
        2 => CoreStage.delivered,
        3 || 4 || 10 => CoreStage.cancelling,
        5 || 6 => CoreStage.cancelled,
        _ => CoreStage.unknown,
      };

  /// اسم من نفّذ الحركة — للكشف المطبوع، من `transfer_attributions`.
  ///
  /// **فارغ يعني «لا سجلّ نسبة»، لا «الوكيل»**: حركةٌ أنشأها فرعٌ في
  /// المنظومة أو سبقت هذه الميزة لا منفّذ معروف لها، وملؤها بالوكيل
  /// تخميناً يضع اسمه على عملٍ قد لا يكون عمله. والكشف يطبع «—».
  final String executedBy;

  /// 3 و4 «قيد الإلغاء» · 5 «ملغية» · 6 «ملغية مسلمة».
  ///
  /// «قيد الإلغاء» محسوبةٌ منها كما في شاشة الحوالات الواردة تماماً: طلب
  /// الإلغاء وحده يكفي لإيقاف يد الوكيل عن الدفع.
  bool get isCancelledByCore =>
      agentCoreType != null && const [3, 4, 5, 6].contains(agentCoreType);

  /// وسم الحالة كما تقوله شاشة «الحوالات الواردة» — أو فارغ إن لم تكن
  /// الحركة في دفتر التسليم.
  ///
  /// الترتيب مطابق لقواعد تبويبات تلك الشاشة حرفياً: **«ملغاة» تسبق «تم
  /// التسليم»** (قرار المالك، 3 سبتمبر 2026). الدفتر تنظيمٌ ظاهريّ في
  /// الواجهة لا قيدٌ محاسبي، فالحالة الأحدث في المنظومة هي التي تُعرض —
  /// وحوالةٌ أُلغيت لم يعد وضعُها «تم التسليم» مهما وقع قبل الإلغاء.
  ///
  /// والملغاة تُعرض «ملغاة» وحدها بلا وسمٍ ثانٍ يقول إنها سُلّمت (أمر المالك
  /// الصريح، 3 سبتمبر 2026): هذا تقريرٌ ظاهريّ للتنظيم، والوسم المزدوج فيه
  /// يزحمه بلا فائدة. و`status` يبقى محفوظاً في القاعدة كما هو.
  String get agentBadge {
    if (isCancelledByCore) return 'ملغاة';
    if (agentStatus == 'DELIVERED') return 'تم التسليم';
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
      coreConfirmType: _intOrNull(j['CoreConfirmType']),
      executedBy: '${j['ExecutedBy'] ?? ''}'.trim(),
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

/// عائلات حالات الحوالة في منظومة الرحالة.
///
/// إحدى عشرة حالة في `InternalEx_Stautes` لا تُعرض كإحدى عشرة شريحة — تُجمع
/// في خمس مراحل تصف **رحلة الحوالة** لا حالتها التقنية:
///
/// | العائلة | الحالات | المعنى للوكيل |
/// |---|---|---|
/// | [pending]    | 0 غير معتمدة | أُنشئت ولم تُعتمد بعد |
/// | [onWay]      | 1 غير مسلمه · 7 علية طلب تاكسي · 8 تاكسي غير مرسل · 9 مرسلة مع مندوب | في الطريق، لم تصل المستفيد |
/// | [delivered]  | 2 مسلمه | وصلت المستفيد |
/// | [cancelling] | 3 و4 قيد الإلغاء · 10 طلب إلغاء من مندوب | طلب إلغاء لم يُبتّ |
/// | [cancelled]  | 5 ملغية · 6 ملغية مسلمة | أُلغيت |
///
/// ⚠ العائلة للّون والترشيح فقط. **الاسم المعروض يبقى كما كتبته المنظومة**،
/// فلا يفقد الوكيل الفرق بين «مرسلة مع مندوب» و«غير مسلمه» — وهو فرقٌ يعني
/// أين الحوالة الآن.
enum CoreStage {
  pending('بانتظار الاعتماد'),
  onWay('في الطريق'),
  delivered('مسلَّمة'),
  cancelling('قيد الإلغاء'),
  cancelled('ملغاة'),

  /// حركة ليست حوالةً في `InternalEx` — أو حوالةٌ لم يُرسل الخادم حالتها.
  unknown('');

  const CoreStage(this.label);

  /// اسم العائلة — يُستعمل في شرائح الترشيح لا في وسم البطاقة.
  final String label;

  bool get known => this != CoreStage.unknown;
}

/// الكشف الكامل — كل حركات الوكيل بلا اقتطاع.
///
/// موضعه هنا لا في شاشة كشف الحساب: يقرأه **ثلاثةُ** مستهلكين (كشف الحساب،
/// وتبويب «صادرة»، وتصدير الـ PDF)، ومزوّدٌ لكلٍّ منها يعني ثلاثة طلبات
/// للبيانات نفسها.
///
/// و`homeSnapshotProvider` لا يصلح بديلاً: يقتطع خمس حركات للواجهة، فقائمةٌ
/// مبنيّة عليه تعرض خمساً وتُخفي الباقي بلا أن يظهر النقص.
///
/// الخادم يعيد **كل** التاريخ بلا ترقيم (رُصد 3345 صفاً)، فيُجلب مرّة
/// ويُعرض على دفعات في الشاشة.
final statementProvider = FutureProvider.autoDispose<List<Movement>>((ref) async {
  final api = ref.watch(apiClientProvider);
  try {
    final env = await api.get('/device/local/account/statment');
    return env.rows.map(Movement.fromJson).toList();
  } on ApiFailure catch (e) {
    if (e.isEmptyResult) return const [];
    rethrow;
  }
});

/// حوالاتٌ أرسلها الوكيل ولم تُعتمد بعد.
///
/// مصدرها `InternalEx` لا كشف الحساب: القيد المحاسبي لا يُكتب إلا عند
/// الاعتماد، فحوالةٌ أُنشئت للتوّ لا أثر لها في الكشف — وكان الوكيل يُدرجها
/// ثم لا يجدها في التطبيق إطلاقاً.
///
/// والخادم يشكّلها بمفاتيح كشف الحساب نفسها، فتُقرأ بـ [Movement] وتُعرض
/// ببطاقته — لا نموذج ثانٍ ولا بطاقة ثانية.
final pendingOutgoingProvider =
    FutureProvider.autoDispose<List<Movement>>((ref) async {
  final api = ref.watch(apiClientProvider);
  try {
    final env = await api.get('/agent/outgoing-transfers/pending');
    return env.rows.map(Movement.fromJson).toList();
  } on ApiFailure catch (e) {
    if (e.isEmptyResult) return const [];
    rethrow;
  }
});
