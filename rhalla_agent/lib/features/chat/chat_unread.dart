import 'dart:async';

import 'package:flutter_riverpod/flutter_riverpod.dart';

import 'chat_repository.dart';

/// عدّاد الرسائل غير المقروءة — للشارة في أعلى الشاشة.
///
/// منفصلٌ عن [chatThreadsProvider] عمداً: تلك تجلب الأسماء وآخرَ رسالةٍ لكل
/// محادثة، وسحبُها كل نصف دقيقة من أجل رقمٍ واحد إهدارٌ لشبكة الوكيل. وهذه
/// نقطةٌ تعيد عدداً.
///
/// وبلا رنين — بخلاف جرس الحوالات. الحوالة الواردة عملٌ ينتظر ومالٌ يُدفع،
/// والرسالة كلام؛ وصوتان يتنازعان انتباه الوكيل يجعلانه يتجاهلهما معاً.
class ChatUnreadController extends StateNotifier<int> {
  ChatUnreadController(this._repo) : super(0);

  final ChatRepository _repo;

  /// وضع الموظّف يسأل نقطةً أخرى — محادثته هو وحدها.
  bool employeeMode = false;

  Timer? _timer;
  bool _busy = false;

  static const every = Duration(seconds: 30);

  void start() {
    _timer?.cancel();
    _timer = Timer.periodic(every, (_) => refresh());
    refresh();
  }

  void stop() {
    _timer?.cancel();
    _timer = null;
  }

  void reset() {
    stop();
    state = 0;
  }

  Future<void> refresh() async {
    if (_busy) return;
    _busy = true;
    try {
      final n = employeeMode
          ? await _repo.employeeUnread()
          : await _repo.unread();
      // ‏-1 تعني «تعذّرت القراءة» لا «صفر»: الشارة تبقى على آخر ما تعرف بدل
      // أن تختفي مع أول انقطاع فيظنّ الوكيل أنه قرأ كل شيء.
      if (n >= 0 && n != state) state = n;
    } finally {
      _busy = false;
    }
  }

  @override
  void dispose() {
    _timer?.cancel();
    super.dispose();
  }
}

final chatUnreadProvider =
    StateNotifierProvider<ChatUnreadController, int>(
  (ref) => ChatUnreadController(ref.watch(chatRepositoryProvider)),
);
