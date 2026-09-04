import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/storage/secure_store.dart';
import '../../core/net/api_envelope.dart';
import 'transfers_repository.dart';

/// حوالة واردة للوكيل، كما يحفظها الخادم في `agent_incoming_transfers`.
///
/// مصدر الحقيقة هو الخادم لا الهاتف: الحالة تبقى بعد إغلاق التطبيق وحذفه
/// وتغيير الجهاز، وتظهر كما هي عند الدخول من هاتف آخر. الدفتر المحلي الذي
/// كان يفرز التبويبين لم يكن يحقّق شيئاً من ذلك — مسحُ بيانات التطبيق كان
/// يمحو سجلّ من سُلِّم.
class AgentIncomingTransfer {
  const AgentIncomingTransfer({
    required this.id,
    required this.code,
    required this.receiverName,
    required this.receiverPhone,
    required this.senderName,
    required this.amount,
    required this.commission,
    required this.branchName,
    required this.sentAt,
    required this.status,
    required this.deliveredAt,
    required this.coreConfirmType,
    required this.coreStatusLabel,
    this.cancelReason = '',
    this.cancelNotes = '',
  });

  /// مفتاح الصفّ في جدول التتبّع — هو ما تُرسله نقطة التسليم.
  final int id;

  /// رقم الحوالة كما في المنظومة (`InternalEx.Code`).
  final String code;

  final String receiverName;
  final String receiverPhone;
  final String senderName;

  /// المبلغ المُرسل (`OverallVal`) — لا العمولة.
  final double amount;
  final double commission;

  final String branchName;
  final String sentAt;

  /// `PENDING_DELIVERY` أو `DELIVERED` — حالة **تسليم الوكيل** وحدها.
  final String status;
  final String deliveredAt;

  /// مرآة حالة المنظومة. تُقرأ ولا تُكتب: تلك الحالة تقوم عليها العمليات
  /// الحسابية، وقرار المالك ألّا يمسّها التطبيق.
  final int? coreConfirmType;
  final String coreStatusLabel;

  /// سبب الإلغاء كما كُتب في منظومة الرحالة لحظة الإلغاء — لا يُصاغ هنا.
  ///
  /// [cancelReason] هو السبب المختار من قائمة المنظومة، و[cancelNotes] نصٌّ
  /// حرّ يُكتب بجانبه. وكلاهما **قراءةٌ حيّة** من المنظومة لا نسخةٌ في دفتر
  /// الوكيل: سببٌ يُصحَّح هناك يجب أن يُقرأ مصحَّحاً هنا.
  ///
  /// وفراغهما لا يعني «بلا سبب» بل **«لم يُسجَّل سبب»** — والفرق يُعرض
  /// للوكيل صراحةً، فادّعاءُ أن الإلغاء بلا سبب أسوأ من الاعتراف بالجهل.
  final String cancelReason;
  final String cancelNotes;

  bool get hasCancelReason =>
      cancelReason.isNotEmpty || cancelNotes.isNotEmpty;

  bool get isDelivered => status == 'DELIVERED';

  /// 3 و4 «قيد الإلغاء» · 5 «ملغية» · 6 «ملغية مسلمة».
  ///
  /// «قيد الإلغاء» محسوبةٌ منها عمداً: طلب الإلغاء وحده يكفي لإيقاف يد
  /// الوكيل عن الدفع.
  bool get isCancelled =>
      coreConfirmType != null && const [3, 4, 5, 6].contains(coreConfirmType);

  /// بطاقة الحوالة وفاتورتها مكتوبتان على [IncomingTransfer]، وهما مجرّبتان.
  ///
  /// المحوّل أرخص من إعادة كتابتهما: نقلُ مصدر البيانات وحده هو التغيير
  /// المقصود، وإعادة كتابة شاشة فاتورة تعمل كانت ستُدخل خطراً بلا مقابل.
  ///
  /// «الوجهة» تُترك فارغة عمداً — هي فرع الوكيل نفسه، وذِكرها له في فاتورته
  /// حشوٌ. والفاتورة تُخفي الحقل الفارغ أصلاً.
  IncomingTransfer get legacy => IncomingTransfer(
        code: code,
        receiverName: receiverName,
        receiverPhone: receiverPhone,
        senderName: senderName,
        amount: amount,
        commission: commission,
        branchName: branchName,
        insertedAt: sentAt,
        status: coreStatusLabel,
        destination: '',
      );

  static double _n(dynamic v) => Fmt.num_(v);

  factory AgentIncomingTransfer.fromJson(Map<String, dynamic> j) =>
      AgentIncomingTransfer(
        id: int.tryParse('${j['id']}') ?? 0,
        code: '${j['transfer_number'] ?? ''}'.trim(),
        receiverName: '${j['beneficiary_name'] ?? ''}'.trim(),
        receiverPhone: '${j['beneficiary_phone'] ?? ''}'.trim(),
        senderName: '${j['sender_name'] ?? ''}'.trim(),
        amount: _n(j['amount']),
        commission: _n(j['commission']),
        branchName: '${j['sender_branch_name'] ?? ''}'.trim(),
        sentAt: '${j['sent_at'] ?? ''}'.trim(),
        status: '${j['status'] ?? ''}'.trim(),
        deliveredAt: '${j['delivered_at'] ?? ''}'.trim(),
        coreConfirmType: j['core_confirm_type'] == null
            ? null
            : int.tryParse('${j['core_confirm_type']}'),
        coreStatusLabel: '${j['core_status_label'] ?? ''}'.trim(),
        cancelReason: '${j['cancel_reason'] ?? ''}'.trim(),
        cancelNotes: '${j['cancel_notes'] ?? ''}'.trim(),
      );
}

/// صفحة نتائج مع أعداد التبويبات الثلاثة.
class IncomingPage {
  const IncomingPage({
    required this.items,
    required this.total,
    required this.pending,
    required this.delivered,
    required this.cancelled,
  });

  final List<AgentIncomingTransfer> items;
  final int total;

  /// الأعداد تأتي من الخادم لا تُحسب في الهاتف: الصفحة الواحدة لا تعرف
  /// كم في التبويبات الأخرى، والعدّ محلياً كان يكذب مع أول ترقيم صفحات.
  final int pending;
  final int delivered;
  final int cancelled;

  static const empty = IncomingPage(
      items: [], total: 0, pending: 0, delivered: 0, cancelled: 0);
}

/// تبويبات الشاشة. `cancelled` تقاطعٌ محسوب في الخادم لا حالةٌ مخزّنة.
enum IncomingTab {
  pending('PENDING_DELIVERY'),
  delivered('DELIVERED'),
  cancelled('CANCELLED');

  const IncomingTab(this.wire);
  final String wire;
}

class AgentIncomingRepository {
  AgentIncomingRepository(this._api, this._store);

  final ApiClient _api;
  final SecureStore _store;

  Future<IncomingPage> page({
    required IncomingTab tab,
    String search = '',
    int page = 1,
    int perPage = 20,
  }) async {
    try {
      final env = await _api.get('/agent/incoming-transfers', query: {
        'status': tab.wire,
        if (search.trim().isNotEmpty) 'search': search.trim(),
        'page': page,
        'per_page': perPage,
      });

      // الحمولة كائن واحد (items/total/counts)، و`row` هو الوصول إليه.
      final data = env.row;
      if (data == null) return IncomingPage.empty;

      final counts = (data['counts'] as Map?)?.cast<String, dynamic>() ?? {};
      final items = (data['items'] as List? ?? const [])
          .whereType<Map>()
          .map((m) => AgentIncomingTransfer.fromJson(m.cast<String, dynamic>()))
          .toList();

      int c(String k) => int.tryParse('${counts[k] ?? 0}') ?? 0;

      return IncomingPage(
        items: items,
        total: int.tryParse('${data['total'] ?? 0}') ?? 0,
        pending: c('PENDING_DELIVERY'),
        delivered: c('DELIVERED'),
        cancelled: c('CANCELLED'),
      );
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return IncomingPage.empty;
      rethrow;
    }
  }

  /// حوالة بعينها برقمها، في أي تبويب كانت.
  ///
  /// تُستعمل من «آخر العمليات» لفتح فاتورة الحوالة: الحركة هناك تحمل رقمها
  /// لا بياناتها. وبلا `status` عمداً — الحوالة قد تكون مسلَّمة أو ملغاة،
  /// والبحث عن رقمٍ بعينه لا يعنيه التبويب.
  ///
  /// وتُطابق الرقم تطابقاً تامّاً: بحث الخادم `LIKE %..%`، فرقمٌ يحوي رقماً
  /// آخر كان يفتح فاتورة حوالة أخرى.
  //
  // ملاحظة: [findOutgoing] أدناه هو نظيرها للصادرة.

  /// فاتورة حوالةٍ **أرسلها** الوكيل — ليست في دفتر الوارد فتُقرأ من المنظومة.
  ///
  /// الخادم يقيّدها بحساب الوكيل نفسه: لا تُعاد حوالة لا أثر لها في حركاته،
  /// ويردّ 404 بلا تفريقٍ بين «غير موجودة» و«ليست لك».
  Future<OutgoingTransfer?> findOutgoing(String code) async {
    final key = code.trim();
    if (key.isEmpty) return null;

    try {
      final env = await _api.get('/agent/outgoing-transfers/$key');
      final j = env.row;
      if (j == null) return null;
      return OutgoingTransfer.fromJson(j);
    } on ApiFailure catch (e) {
      // 404 = ليست حوالةً صادرة لهذا الوكيل — حالةٌ عادية لا عطب.
      if (e.statusCode == 404 || e.isEmptyResult) return null;
      rethrow;
    }
  }

  Future<AgentIncomingTransfer?> findByCode(String code) async {
    final key = code.trim();
    if (key.isEmpty) return null;

    try {
      final env = await _api.get('/agent/incoming-transfers', query: {
        'search': key,
        'per_page': 20,
      });

      final data = env.row;
      if (data == null) return null;

      for (final m in (data['items'] as List? ?? const []).whereType<Map>()) {
        final t = AgentIncomingTransfer.fromJson(m.cast<String, dynamic>());
        if (t.code == key) return t;
      }
      return null;
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return null;
      rethrow;
    }
  }

  /// تسجيل التسليم. النقل في الواجهة لا يقع إلا بعد تأكيد الخادم — انقطاعُ
  /// الشبكة يرمي [ApiFailure] فتبقى الحوالة حيث هي.
  ///
  /// معرّف الجهاز يُرسَل في ترويسة ليُسجَّل في أثر التتبّع: سؤال «من أي جهاز
  /// سُجّل هذا التسليم؟» لا جواب له بدونه. ولا يُرسَل في الجسم — الخادم لا
  /// يبني عليه قراراً، فهو شهادةٌ لا هويّة.
  Future<void> deliver(int id) async {
    final device = await _store.deviceId();
    await _api.post(
      '/agent/incoming-transfers/$id/deliver',
      headers: {'X-Device-Id': device},
    );
  }
}

final agentIncomingRepositoryProvider = Provider<AgentIncomingRepository>(
  (ref) => AgentIncomingRepository(
    ref.watch(apiClientProvider),
    ref.watch(secureStoreProvider),
  ),
);

/// وسيط طلبٍ واحد — التبويب والبحث معاً، فتغيّر أيّهما يُعيد الجلب.
class IncomingQuery {
  const IncomingQuery(this.tab, this.search);

  final IncomingTab tab;
  final String search;

  @override
  bool operator ==(Object other) =>
      other is IncomingQuery && other.tab == tab && other.search == search;

  @override
  int get hashCode => Object.hash(tab, search);
}

final agentIncomingProvider = FutureProvider.autoDispose
    .family<IncomingPage, IncomingQuery>((ref, q) async {
  return ref
      .watch(agentIncomingRepositoryProvider)
      .page(tab: q.tab, search: q.search, perPage: 50);
});

/// حوالةٌ أرسلها الوكيل — للعرض في فاتورة «صادرة».
///
/// نموذجٌ منفصل عن [AgentIncomingTransfer] عمداً: تلك صفٌّ في دفتر التسليم
/// له معرّف وحالة تسليم، وهذه قراءةٌ من المنظومة لا تُسلَّم ولا تُسجَّل.
/// دمجُهما كان يعني حقولاً فارغة في أحد السياقين وزرَّ تسليمٍ لا معنى له.
class OutgoingTransfer {
  const OutgoingTransfer({
    required this.legacy,
    required this.confirmType,
    required this.statusName,
    required this.cancelReason,
    required this.cancelNotes,
  });

  /// جسم الفاتورة يقرأ هذا النموذج — نفسه الذي تقرأه فاتورة الواردة.
  final IncomingTransfer legacy;

  final int? confirmType;
  final String statusName;
  final String cancelReason;
  final String cancelNotes;

  /// 3 و4 «قيد الإلغاء» · 5 «ملغية» · 6 «ملغية مسلمة» — كما في الواردة.
  bool get isCancelled =>
      confirmType != null && const [3, 4, 5, 6].contains(confirmType);

  bool get hasCancelReason =>
      cancelReason.isNotEmpty || cancelNotes.isNotEmpty;

  static OutgoingTransfer fromJson(Map<String, dynamic> j) => OutgoingTransfer(
        legacy: IncomingTransfer(
          code: '${j['Code'] ?? ''}'.trim(),
          receiverName: '${j['RecievedName'] ?? ''}'.trim(),
          // `RPhone1` هنا لا `RPhone`: هذه قراءةٌ من الجدول لا من الـ view.
          receiverPhone: '${j['RPhone1'] ?? ''}'.trim(),
          senderName: '${j['SenderName'] ?? ''}'.trim(),
          amount: Fmt.num_(j['OverallVal']),
          commission: Fmt.num_(j['ExVal']),
          branchName: '${j['DeliveredBranchName'] ?? ''}'.trim(),
          insertedAt: '${j['InsertDate'] ?? ''}'.trim(),
          status: '${j['StatusName'] ?? ''}'.trim(),
          // «الوجهة» = مدينة الاستلام لا الفرع (قرار المالك، 4 سبتمبر 2026).
          //
          // الفرع يُسنَد عند الاعتماد ويكون صفراً قبله، فكانت الوجهة تغيب عن
          // فاتورة الحوالة الجديدة. أما المدينة فيختارها الوكيل لحظة الإنشاء
          // (`InternalEx.DeliveryPlace`)، فتوجد في كل الحالات.
          destination: '${j['DeliveryCityName'] ?? ''}'.trim(),
        ),
        confirmType: int.tryParse('${j['ConfirmType'] ?? ''}'),
        statusName: '${j['StatusName'] ?? ''}'.trim(),
        cancelReason: '${j['cancel_reason'] ?? ''}'.trim(),
        cancelNotes: '${j['cancel_notes'] ?? ''}'.trim(),
      );
}
