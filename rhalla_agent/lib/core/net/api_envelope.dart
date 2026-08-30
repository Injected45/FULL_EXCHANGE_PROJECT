/// فكّاك متسامح لغلاف استجابة الـ API.
///
/// الخلفية لا تلتزم بغلاف واحد. الانحرافات موثّقة في docs/agent-api.md، وأخطرها:
///
///  • في مسار «الرصيد غير كافٍ» انعكست وسائط sendError، فيصل `message`
///    ككائن JSON و`data` كنص. تمرير message إلى Text() يُسقط التطبيق.
///  • `datat` بدل `data` في Daily_transfer.
///  • `{message, status}` بلا غلاف في checkOtp.
///  • نموذج عارٍ أو null في الجذر في searchPayment.
///  • جسم 200 فارغ تماماً في Rollback_Branch_Trinsfrim_me.
///  • success:true مع HTTP 422، و success:false مع HTTP 200.
///  • 404 كحالة «لا توجد بيانات» في ست نقاط على الأقل.
///
/// لذلك: لا تكتب [ApiResponse] واحداً. اقرأ بتسامح، وافحص النوع قبل العرض.
library;

class Envelope {
  const Envelope({
    required this.statusCode,
    required this.ok,
    required this.raw,
    this.messageText,
    this.messageMap,
    this.payload,
    this.key,
  });

  final int statusCode;

  /// نجاح منطقي — من `success`، ثم `status`، ثم رمز HTTP.
  final bool ok;

  /// الجسم كما وصل، للحالات التي لا يغطيها أي شيء آخر.
  final dynamic raw;

  /// `message` حين يكون نصاً. فارغ حين يكون كائناً — انظر [messageMap].
  final String? messageText;

  /// `message` حين يصل ككائن JSON (مسار الرصيد غير الكافي).
  final Map<String, dynamic>? messageMap;

  /// الحمولة: `data` ?? `datat` ?? `errors` ?? الجذر.
  final dynamic payload;

  /// ثابت ResponseEnums. عملياً لا يُصدر إلا SUCCESS و INVALID_CREDENTIALS،
  /// فلا تبنِ عليه توجيه الأخطاء — اعتمد statusCode.
  final String? key;

  /// نص صالح للعرض في Text() مهما كان شكل الرد.
  ///
  /// لا يعيد أبداً جسماً غير-JSON: صفحة خطأ من Apache أو أثر استثناء PHP
  /// (والخادم يعيدهما بـ APP_DEBUG=true) نص طويل بلا معنى للمستخدم.
  String displayMessage(String fallback) {
    final fromMessage = _presentable(messageText);
    if (fromMessage != null) return fromMessage;

    // حين تنعكس وسائط sendError، يحمل data النص العربي.
    if (payload is String) {
      final fromPayload = _presentable(payload as String);
      if (fromPayload != null) return fromPayload;
    }
    return fallback;
  }

  /// يرفض ما لا يصلح للعرض: الفراغ، والوسوم، والنصوص الطويلة.
  static String? _presentable(String? s) {
    if (s == null) return null;
    final t = s.trim();
    if (t.isEmpty) return null;
    if (looksLikeMarkup(t)) return null;
    if (t.length > 240) return null;
    return t;
  }

  /// جسم ليس JSON — صفحة خادم أو أثر استثناء.
  static bool looksLikeMarkup(String s) {
    final t = s.trimLeft();
    return t.startsWith('<') ||
        t.startsWith('{"trace"') ||
        t.contains('<!DOCTYPE') ||
        t.contains('<html');
  }

  /// أول رسالة تحقّق من حقيبة أخطاء Laravel، إن وُجدت.
  ///
  /// **لا تُخرج إلا نصاً.** كانت تعيد `v.first.toString()` لأي قائمة، وحين
  /// رفض الخادم حوالةً لتجاوز السقف ردّ بجذرٍ فيه
  /// `violations: [{type_from: Daily, Debit: 5025.000, label: اليومي}]` —
  /// فرأى الوكيل بنية البيانات الخام في شريط أحمر بدل رسالة. حقيبة أخطاء
  /// Laravel نصوصٌ دائماً `{حقل: [رسائل]}`، وقائمةُ كائنات ليست رسائل.
  String? firstValidationError() {
    final src = payload;
    if (src is! Map) return null;
    for (final v in src.values) {
      if (v is List) {
        for (final item in v) {
          final s = _presentable(item is String ? item : null);
          if (s != null) return s;
        }
        continue;
      }
      final s = _presentable(v is String ? v : null);
      if (s != null) return s;
    }
    return null;
  }

  List<Map<String, dynamic>> get rows {
    final p = payload;
    if (p is List) {
      return p.whereType<Map>().map((e) => e.cast<String, dynamic>()).toList();
    }
    if (p is Map) return [p.cast<String, dynamic>()];
    return const [];
  }

  Map<String, dynamic>? get row => rows.isEmpty ? null : rows.first;

  static Envelope parse(int statusCode, dynamic body) {
    // جسم فارغ — نجاح صامت (مثل Rollback_Branch_Trinsfrim_me).
    if (body == null || (body is String && body.trim().isEmpty)) {
      return Envelope(
        statusCode: statusCode,
        ok: statusCode >= 200 && statusCode < 300,
        raw: body,
      );
    }

    // ليس كائناً — نموذج عارٍ أو قيمة في الجذر.
    if (body is! Map) {
      return Envelope(
        statusCode: statusCode,
        ok: statusCode >= 200 && statusCode < 300,
        raw: body,
        payload: body,
      );
    }

    final m = body.cast<String, dynamic>();

    // النجاح: success ثم status ثم رمز HTTP.
    bool ok;
    if (m['success'] is bool) {
      ok = m['success'] as bool;
    } else if (m['status'] is bool) {
      ok = m['status'] as bool;
    } else {
      ok = statusCode >= 200 && statusCode < 300;
    }

    // الرسالة قد تكون نصاً أو كائناً.
    String? msgText;
    Map<String, dynamic>? msgMap;
    final rawMsg = m['message'] ?? m['error'];
    if (rawMsg is String) {
      msgText = rawMsg;
    } else if (rawMsg is Map) {
      msgMap = rawMsg.cast<String, dynamic>();
    }

    // الحمولة — لاحظ datat.
    dynamic payload;
    if (m.containsKey('data')) {
      payload = m['data'];
    } else if (m.containsKey('datat')) {
      payload = m['datat'];
    } else if (m.containsKey('errors')) {
      payload = m['errors'];
    } else if (m.containsKey('details')) {
      payload = m['details'];
    } else {
      payload = m;
    }

    return Envelope(
      statusCode: statusCode,
      ok: ok,
      raw: m,
      messageText: msgText,
      messageMap: msgMap,
      payload: payload,
      key: m['key'] as String?,
    );
  }
}

/// خطأ موحّد للطبقة الأعلى.
class ApiFailure implements Exception {
  ApiFailure(this.message, {this.statusCode, this.envelope, this.isNetwork = false});

  final String message;
  final int? statusCode;
  final Envelope? envelope;
  final bool isNetwork;

  /// نقاط ترد 404 لتعني «لا توجد بيانات» لا خطأً.
  bool get isEmptyResult => statusCode == 404;

  @override
  String toString() => message;
}
