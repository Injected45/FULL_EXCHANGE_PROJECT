import 'package:dio/dio.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';

/// محادثة في قائمة الوكيل: الإدارة، أو موظّف من موظّفيه.
class ChatThread {
  const ChatThread({
    required this.id,
    required this.kind,
    required this.title,
    required this.employeeId,
    required this.lastMessageAt,
    required this.lastBody,
    required this.unread,
  });

  final int id;

  /// `ADMIN` أو `EMPLOYEE`.
  final String kind;

  /// «إدارة الرحالة» أو اسم الموظّف — يأتي من الخادم لا يُبنى هنا.
  final String title;

  final int? employeeId;
  final String lastMessageAt;
  final String lastBody;
  final int unread;

  bool get isAdmin => kind == 'ADMIN';

  factory ChatThread.fromJson(Map<String, dynamic> j) => ChatThread(
        id: int.tryParse('${j['id']}') ?? 0,
        kind: '${j['kind'] ?? ''}',
        title: '${j['title'] ?? ''}'.trim(),
        employeeId: j['employee_id'] == null
            ? null
            : int.tryParse('${j['employee_id']}'),
        lastMessageAt: '${j['last_message_at'] ?? ''}'.trim(),
        lastBody: '${j['last_body'] ?? ''}'.trim(),
        unread: int.tryParse('${j['unread'] ?? 0}') ?? 0,
      );
}

/// رسالة واحدة.
///
/// [senderKind] هو ما يُبنى عليه جانبُ الفقاعة — لا مقارنةُ رقم المرسِل
/// برقم القارئ: الوكيل رقمه في `users` والموظف في `employees`، ورقمان
/// متساويان في جدولين مختلفين شخصان مختلفان.
class ChatMessage {
  const ChatMessage({
    required this.id,
    required this.senderKind,
    required this.senderName,
    required this.body,
    required this.createdAt,
    this.deleted = false,
    this.edited = false,
    this.pinned = false,
    this.replyToId,
    this.replyBody = '',
    this.replySenderName = '',
    this.replyAttachmentKind = '',
    this.attachmentPath = '',
    this.attachmentName = '',
    this.attachmentMime = '',
    this.attachmentSize = 0,
    this.attachmentKind = '',
  });

  final int id;

  /// `AGENT` · `EMPLOYEE` · `ADMIN`.
  final String senderKind;
  final String senderName;
  final String body;
  final String createdAt;

  /// حُذفت — يبقى الصفّ ويُخفى محتواه، والخادم هو من أخفاه لا هذه الشاشة.
  final bool deleted;

  /// الرسالة المُقتبَسة — تصل مع الرسالة في الاستعلام نفسه.
  final int? replyToId;
  final String replyBody;
  final String replySenderName;
  final String replyAttachmentKind;

  /// المرفق. [attachmentPath] اسمُه على الخادم — يُركَّب عليه المسار.
  final String attachmentPath;
  final String attachmentName;
  final String attachmentMime;
  final int attachmentSize;

  /// `IMAGE` · `AUDIO` · `FILE` — أو فارغ إن لا مرفق.
  final String attachmentKind;

  /// عُدِّلت (البند 28) · مثبَّتة في المحادثة (البند 31).
  final bool edited;
  final bool pinned;

  bool get hasAttachment => attachmentPath.isNotEmpty;
  bool get isImage => attachmentKind == 'IMAGE';
  bool get isAudio => attachmentKind == 'AUDIO';
  bool get hasReply => replyToId != null;

  /// نسخةٌ معدَّلة — للتفاؤل في الواجهة قبل تأكيد الخادم.
  ChatMessage copyWith({String? body, bool? edited, bool? pinned, bool? deleted}) =>
      ChatMessage(
        id: id,
        senderKind: senderKind,
        senderName: senderName,
        body: body ?? this.body,
        createdAt: createdAt,
        deleted: deleted ?? this.deleted,
        edited: edited ?? this.edited,
        pinned: pinned ?? this.pinned,
        replyToId: replyToId,
        replyBody: replyBody,
        replySenderName: replySenderName,
        replyAttachmentKind: replyAttachmentKind,
        attachmentPath: attachmentPath,
        attachmentName: attachmentName,
        attachmentMime: attachmentMime,
        attachmentSize: attachmentSize,
        attachmentKind: attachmentKind,
      );

  /// نصٌّ يصف المُقتبَس حين يكون مرفقاً بلا تعليق.
  String get replyPreview {
    if (replyBody.isNotEmpty) return replyBody;
    return switch (replyAttachmentKind) {
      'IMAGE' => '📷 صورة',
      'AUDIO' => '🎤 رسالة صوتية',
      'FILE' => '📎 ملف',
      _ => '',
    };
  }

  factory ChatMessage.fromJson(Map<String, dynamic> j) => ChatMessage(
        id: int.tryParse('${j['id']}') ?? 0,
        senderKind: '${j['sender_kind'] ?? ''}',
        senderName: '${j['sender_name'] ?? ''}'.trim(),
        body: j['body'] == null ? '' : '${j['body']}',
        createdAt: '${j['created_at'] ?? ''}'.trim(),
        deleted: j['deleted_at'] != null,
        edited: j['edited_at'] != null,
        pinned: j['pinned_at'] != null,
        replyToId: j['reply_to_id'] == null
            ? null
            : int.tryParse('${j['reply_to_id']}'),
        replyBody: j['reply_body'] == null ? '' : '${j['reply_body']}',
        replySenderName: '${j['reply_sender_name'] ?? ''}'.trim(),
        replyAttachmentKind: '${j['reply_attachment_kind'] ?? ''}'.trim(),
        attachmentPath: '${j['attachment_path'] ?? ''}'.trim(),
        attachmentName: '${j['attachment_name'] ?? ''}'.trim(),
        attachmentMime: '${j['attachment_mime'] ?? ''}'.trim(),
        attachmentSize: int.tryParse('${j['attachment_size'] ?? 0}') ?? 0,
        attachmentKind: '${j['attachment_kind'] ?? ''}'.trim(),
      );
}

/// إلى أين وصلت رسائلي عند الطرف الآخر، وإلى أين قرأها.
///
/// رقمان لا رايتان على كل رسالة: الأرقام تصاعدية، فرقمٌ واحد يصف «كل ما
/// قبله» — وصفٌّ لكل رسالة كان يعني جدولاً يكبر بعدد الرسائل × القرّاء.
class ChatReceipts {
  const ChatReceipts({this.delivered = 0, this.read = 0});

  final int delivered;
  final int read;

  /// ✓✓ زرقاء · ✓✓ رمادية · ✓ واحدة.
  bool isRead(int messageId) => messageId <= read;
  bool isDelivered(int messageId) => messageId <= delivered;

  factory ChatReceipts.fromJson(Map<String, dynamic>? j) => ChatReceipts(
        delivered: int.tryParse('${j?['delivered'] ?? 0}') ?? 0,
        read: int.tryParse('${j?['read'] ?? 0}') ?? 0,
      );
}

/// «يكتب الآن» أو «يسجّل رسالة صوتية» — حالةٌ لحظية لا تُحفظ.
class ChatTyping {
  const ChatTyping({required this.name, required this.state});

  final String name;

  /// `TYPING` أو `RECORDING`.
  final String state;

  bool get isRecording => state == 'RECORDING';

  String get label => isRecording ? 'يسجّل رسالة صوتية…' : 'يكتب الآن…';

  static ChatTyping? fromJson(Map<String, dynamic>? j) {
    if (j == null) return null;
    final s = '${j['state'] ?? ''}';
    if (s.isEmpty) return null;
    return ChatTyping(name: '${j['actor_name'] ?? ''}'.trim(), state: s);
  }
}

/// رسائل صفحةٍ مع إيصالاتها وتفاعلاتها.
class ChatPage {
  const ChatPage({
    required this.items,
    required this.receipts,
    this.reactions = const {},
    this.starred = const {},
    this.typing,
  });

  final List<ChatMessage> items;
  final ChatReceipts receipts;

  /// رقم الرسالة ⇦ {رمز: (العدد، هل تفاعلتُ أنا)}.
  final Map<int, Map<String, ReactionCount>> reactions;

  /// أرقام الرسائل التي حفظتُها.
  final Set<int> starred;

  final ChatTyping? typing;

  static const empty = ChatPage(items: [], receipts: ChatReceipts());

  static Map<int, Map<String, ReactionCount>> parseReactions(dynamic raw) {
    if (raw is! Map) return const {};
    final out = <int, Map<String, ReactionCount>>{};
    raw.forEach((k, v) {
      final id = int.tryParse('$k');
      if (id == null || v is! Map) return;
      final m = <String, ReactionCount>{};
      v.forEach((emoji, info) {
        if (info is! Map) return;
        m['$emoji'] = ReactionCount(
          count: int.tryParse('${info['count'] ?? 0}') ?? 0,
          mine: info['mine'] == true || '${info['mine']}' == '1',
        );
      });
      if (m.isNotEmpty) out[id] = m;
    });
    return out;
  }
}

class ReactionCount {
  const ReactionCount({required this.count, required this.mine});
  final int count;
  final bool mine;
}

/// مسارا الدردشة: مسار الوكيل ومسار الموظّف.
///
/// المستودع واحد والمسارات مختلفة عمداً: الشاشة نفسها تعرض المحادثة في
/// الوضعين، وازدواجُ الشاشة كان سيعني إصلاحَ كل عطبٍ مرّتين.
class ChatRepository {
  ChatRepository(this._api);

  final ApiClient _api;

  // ── الوكيل ────────────────────────────────────────────────────────

  Future<List<ChatThread>> threads() async {
    try {
      final env = await _api.get('/chat/threads');
      final data = env.row;
      if (data == null) return const [];
      return (data['items'] as List? ?? const [])
          .whereType<Map>()
          .map((m) => ChatThread.fromJson(m.cast<String, dynamic>()))
          .toList();
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  /// يفتح محادثةً مع موظّف — أو يعيد رقم القائمة إن كانت.
  Future<int?> openEmployee(int employeeId) async {
    try {
      final env = await _api.post('/chat/threads/employee',
          body: {'employee_id': employeeId});
      final v = env.row?['thread_id'];
      return v == null ? null : int.tryParse('$v');
    } on ApiFailure catch (e) {
      if (e.statusCode == 404) return null;
      rethrow;
    }
  }

  Future<ChatPage> messages(int threadId, {int afterId = 0}) =>
      _messages('/chat/threads/$threadId/messages', afterId);

  Future<ChatMessage?> send(
    int threadId,
    String body, {
    String? filePath,
    int? replyToId,
  }) =>
      _send('/chat/threads/$threadId/messages', body,
          filePath: filePath, replyToId: replyToId);

  Future<void> deleteMessage(int threadId, int messageId) =>
      _api.delete('/chat/threads/$threadId/messages/$messageId');

  /// مسار المرفق — يُطلب بترويسة التوثيق، فالمرفقات خلف `auth:sanctum`.
  String attachmentUrl(String name) => '$kApiBase/chat/attachment/$name';

  Future<int> unread() => _unread('/chat/unread');

  Future<void> typing(int threadId, String state) =>
      _api.post('/chat/threads/$threadId/typing', body: {'state': state});

  Future<void> react(int threadId, int messageId, String emoji) =>
      _api.post('/chat/threads/$threadId/messages/$messageId/react',
          body: {'emoji': emoji});

  Future<void> editMessage(int threadId, int messageId, String body) =>
      _api.put('/chat/threads/$threadId/messages/$messageId', body: {'body': body});

  Future<void> starMessage(int threadId, int messageId, bool on) =>
      _api.post('/chat/threads/$threadId/messages/$messageId/star',
          body: {'star': on});

  Future<void> pinMessage(int threadId, int messageId, bool on) =>
      _api.post('/chat/threads/$threadId/messages/$messageId/pin', body: {'pin': on});

  /// كتم · تثبيت · أرشفة · قفل · تحديد كغير مقروءة (البنود 10, 32–34, 59).
  Future<void> threadSettings(int threadId, Map<String, dynamic> changes) =>
      _api.put('/chat/threads/$threadId/settings', body: changes);

  Future<List<ChatMessage>> search(String q) async {
    if (q.trim().length < 2) return const [];
    try {
      final env = await _api.get('/chat/search', query: {'q': q.trim()});
      return (env.row?['items'] as List? ?? const [])
          .whereType<Map>()
          .map((m) => ChatMessage.fromJson(m.cast<String, dynamic>()))
          .toList();
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  // ── الموظّف ───────────────────────────────────────────────────────
  //
  // بلا رقم محادثة: للموظّف محادثةٌ واحدة ممكنة — مع وكيله — والخادم يعرفها
  // من جلسته. وتمريرُ رقمٍ من الهاتف كان يفتح باب محادثة موظّفٍ آخر.

  Future<ChatPage> employeeMessages({int afterId = 0}) =>
      _messages('/device/employee/chat', afterId);

  Future<ChatMessage?> employeeSend(
    String body, {
    String? filePath,
    int? replyToId,
  }) =>
      _send('/device/employee/chat', body,
          filePath: filePath, replyToId: replyToId);

  Future<void> employeeDeleteMessage(int messageId) =>
      _api.delete('/device/employee/chat/$messageId');

  String employeeAttachmentUrl(String name) =>
      '$kApiBase/device/employee/chat/attachment/$name';

  Future<int> employeeUnread() => _unread('/device/employee/chat/unread');

  Future<void> employeeTyping(String state) =>
      _api.post('/device/employee/chat/typing', body: {'state': state});

  Future<void> employeeReact(int messageId, String emoji) =>
      _api.post('/device/employee/chat/$messageId/react', body: {'emoji': emoji});

  Future<void> employeeEditMessage(int messageId, String body) =>
      _api.put('/device/employee/chat/$messageId', body: {'body': body});

  Future<void> employeeStarMessage(int messageId, bool on) =>
      _api.post('/device/employee/chat/$messageId/star', body: {'star': on});

  Future<void> employeePinMessage(int messageId, bool on) =>
      _api.post('/device/employee/chat/$messageId/pin', body: {'pin': on});

  // ── المشترك ───────────────────────────────────────────────────────

  /// ترويسة التوثيق للصور — `Image.network` لا يمرّ بـ dio.
  Future<Map<String, String>> imageHeaders() => _api.authHeaders();

  Future<ChatPage> _messages(String path, int afterId) async {
    try {
      final env = await _api.get(path,
          query: afterId > 0 ? {'after_id': afterId} : null);
      final data = env.row;
      if (data == null) return ChatPage.empty;

      return ChatPage(
        items: (data['items'] as List? ?? const [])
            .whereType<Map>()
            .map((m) => ChatMessage.fromJson(m.cast<String, dynamic>()))
            .toList(),
        receipts: ChatReceipts.fromJson(
            (data['receipts'] as Map?)?.cast<String, dynamic>()),
        reactions: ChatPage.parseReactions(data['reactions']),
        starred: (data['starred'] as List? ?? const [])
            .map((e) => int.tryParse('$e') ?? 0)
            .where((e) => e > 0)
            .toSet(),
        typing: ChatTyping.fromJson(
            (data['typing'] as Map?)?.cast<String, dynamic>()),
      );
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return ChatPage.empty;
      rethrow;
    }
  }

  /// إرسالٌ بمرفق أو بدونه.
  ///
  /// `FormData` حين يوجد مرفق و JSON حين لا يوجد: الأولى تحمل ملفاً والثانية
  /// أخفّ، ورسائل النصّ هي الأغلب.
  Future<ChatMessage?> _send(
    String path,
    String body, {
    String? filePath,
    int? replyToId,
  }) async {
    final Object payload;

    if (filePath != null && filePath.isNotEmpty) {
      payload = FormData.fromMap({
        'body': body,
        'reply_to_id': ?replyToId,
        'attachment': await MultipartFile.fromFile(filePath),
      });
    } else {
      payload = {
        'body': body,
        'reply_to_id': ?replyToId,
      };
    }

    final env = await _api.post(path, body: payload);
    final m = env.row?['message'];
    return m is Map ? ChatMessage.fromJson(m.cast<String, dynamic>()) : null;
  }

  Future<int> _unread(String path) async {
    try {
      final env = await _api.get(path);
      return int.tryParse('${env.row?['total'] ?? 0}') ?? 0;
    } on ApiFailure {
      // الشارة لا تعرض أخطاء: انقطاعُ الشبكة يُبقيها على آخر ما تعرف.
      return -1;
    }
  }
}

final chatRepositoryProvider =
    Provider<ChatRepository>((ref) => ChatRepository(ref.watch(apiClientProvider)));

final chatThreadsProvider = FutureProvider.autoDispose<List<ChatThread>>(
  (ref) => ref.watch(chatRepositoryProvider).threads(),
);
