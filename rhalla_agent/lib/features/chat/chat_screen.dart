import 'dart:async';
import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';
import 'package:image_picker/image_picker.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'chat_bubble.dart';
import 'chat_repository.dart';
import 'emoji_picker.dart';
import 'voice_note.dart';

/// شاشة محادثة واحدة — للوكيل وللموظّف معاً.
///
/// شاشةٌ واحدة لا اثنتان: الفرق بين الوضعين هو **من أين تُجلب الرسائل وأيّ
/// فقاعةٍ لي**، وكلاهما وسيطٌ يُمرَّر. ونسخُ الشاشة كان سيعني إصلاح كل عطبٍ
/// مرّتين، ونسيان إحداهما في المرّة الثالثة.
class ChatScreen extends ConsumerStatefulWidget {
  const ChatScreen({
    super.key,
    required this.title,
    this.threadId,
    this.asEmployee = false,
  });

  final String title;

  /// رقم المحادثة في وضع الوكيل. لا يُمرَّر في وضع الموظّف — الخادم يعرف
  /// محادثته من جلسته، وتمريرُ رقمٍ من الهاتف يفتح باب محادثة موظّفٍ آخر.
  final int? threadId;

  final bool asEmployee;

  @override
  ConsumerState<ChatScreen> createState() => _ChatScreenState();
}

class _ChatScreenState extends ConsumerState<ChatScreen> {
  final _input = TextEditingController();
  final _scroll = ScrollController();
  final _inputFocus = FocusNode();

  List<ChatMessage> _messages = const [];
  ChatReceipts _receipts = const ChatReceipts();
  Map<String, String> _headers = const {};
  Map<int, Map<String, ReactionCount>> _reactions = const {};
  Set<int> _starred = const {};
  ChatTyping? _typing;

  bool _loading = true;
  bool _sending = false;
  bool _emoji = false;
  String? _error;

  /// الرسالة التي يُردّ عليها الآن — تُعرض فوق صندوق الكتابة.
  ChatMessage? _replyTo;

  Timer? _timer;

  /// **كل خمس ثوانٍ، ولا يُجلب إلا الجديد.**
  ///
  /// النبضة تحمل `after_id` فتعود فارغةً حين لا جديد — سطرٌ في السجلّ لا
  /// تاريخُ محادثةٍ كامل. ولذلك تصحّ خمس ثوانٍ هنا حيث لا تصحّ للرصيد:
  /// المحادثة تفقد معناها إن تأخّر الردّ نصف دقيقة.
  static const _every = Duration(seconds: 5);

  /// من «أنا» في هذه المحادثة — به يُعرف جانب الفقاعة.
  String get _me => widget.asEmployee ? 'EMPLOYEE' : 'AGENT';

  @override
  void initState() {
    super.initState();
    _load();
    _timer = Timer.periodic(_every, (_) => _poll());
    // إغلاق لوحة الإيموجي حين تُفتح لوحة المفاتيح: اللوحتان معاً تأكلان
    // الشاشة كلّها ولا تبقى للمحادثة سطراً.
    _inputFocus.addListener(() {
      if (_inputFocus.hasFocus && _emoji) setState(() => _emoji = false);
    });
  }

  @override
  void dispose() {
    _timer?.cancel();
    _input.dispose();
    _scroll.dispose();
    _inputFocus.dispose();
    super.dispose();
  }

  ChatRepository get _repo => ref.read(chatRepositoryProvider);

  String _url(String name) => widget.asEmployee
      ? _repo.employeeAttachmentUrl(name)
      : _repo.attachmentUrl(name);

  Future<ChatPage> _fetch(int afterId) => widget.asEmployee
      ? _repo.employeeMessages(afterId: afterId)
      : _repo.messages(widget.threadId!, afterId: afterId);

  Future<void> _load() async {
    try {
      final headers = await _repo.imageHeaders();
      final page = await _fetch(0);
      if (!mounted) return;
      setState(() {
        _messages = page.items;
        _receipts = page.receipts;
        _reactions = page.reactions;
        _starred = page.starred;
        _typing = page.typing;
        _headers = headers;
        _loading = false;
        _error = null;
      });
      _toBottom(jump: true);
    } catch (e) {
      if (!mounted) return;
      setState(() {
        _loading = false;
        _error = '$e';
      });
    }
  }

  /// جلبٌ تزايدي. صامتٌ في الفشل: انقطاع لحظي لا يُفرغ محادثةً بين يدي
  /// صاحبها، والنبضة التالية تُصلحه.
  Future<void> _poll() async {
    if (!mounted || _loading) return;
    final after = _messages.isEmpty ? 0 : _messages.last.id;
    try {
      final page = await _fetch(after);
      if (!mounted) return;

      // الإيصالات تُحدَّث دائماً ولو لم تصل رسالة: ✓ تصير ✓✓ حين يقرأ
      // الطرف الآخر، وذلك ليس رسالةً جديدة.
      setState(() {
        _receipts = page.receipts;
        _typing = page.typing;
        // التفاعلات تصل للصفحة المطلوبة وحدها؛ في الجلب التزايدي تخصّ
        // الرسائل الجديدة، فتُدمَج ولا تُستبدل — وإلا اختفت تفاعلات ما فوقها.
        if (page.reactions.isNotEmpty) {
          _reactions = {..._reactions, ...page.reactions};
        }
        if (page.starred.isNotEmpty) _starred = {..._starred, ...page.starred};
        if (page.items.isNotEmpty) _messages = [..._messages, ...page.items];
      });
      if (page.items.isNotEmpty) _toBottom();
    } catch (_) {
      // انظر التوثيق أعلاه.
    }
  }

  Future<void> _send({String? filePath}) async {
    final body = _input.text.trim();
    if ((body.isEmpty && filePath == null) || _sending) return;

    setState(() => _sending = true);
    try {
      final msg = widget.asEmployee
          ? await _repo.employeeSend(body,
              filePath: filePath, replyToId: _replyTo?.id)
          : await _repo.send(widget.threadId!, body,
              filePath: filePath, replyToId: _replyTo?.id);
      if (!mounted) return;

      // الحقل يُفرَغ بعد تأكيد الخادم لا قبله: إفراغُه أولاً يضيّع ما كتبه
      // الوكيل إن سقطت الشبكة، وهو ما لا يُغتفر في رسالةٍ طويلة.
      _input.clear();
      setState(() {
        if (msg != null) _messages = [..._messages, msg];
        _replyTo = null;
        _sending = false;
      });
      _toBottom();
    } catch (e) {
      if (!mounted) return;
      setState(() => _sending = false);
      _toast('تعذّر الإرسال. $e');
    }
  }

  /// صورة من الكاميرا أو المعرض.
  ///
  /// `imageQuality: 70` و`maxWidth: 1600`: صورة كاميرا هاتف حديث تتجاوز
  /// 5 ميغابايت، وحدُّ الخادم 8 — والضغط هنا يجعل الرفع ممكناً على شبكة
  /// فرعٍ بطيئة، وإيصالٌ مصوَّر يبقى مقروءاً تماماً عند هذه الدقّة.
  Future<void> _pickImage(ImageSource source) async {
    try {
      final x = await ImagePicker().pickImage(
        source: source,
        imageQuality: 70,
        maxWidth: 1600,
      );
      if (x == null) return;
      await _send(filePath: x.path);
    } catch (e) {
      if (mounted) _toast('تعذّر اختيار الصورة. $e');
    }
  }

  // ── الرسائل الصوتية (البنود 19–21) ─────────────────────────────────

  final _recorder = VoiceRecorder();
  Duration _recElapsed = Duration.zero;
  double _recAmp = 0;
  bool _recPaused = false;
  Timer? _recTimer;

  Future<void> _startRecording() async {
    FocusScope.of(context).unfocus();
    setState(() => _emoji = false);

    if (!await _recorder.hasPermission()) {
      // رسالةٌ تشرح لماذا (البند 46): «رُفض الإذن» وحدها لا تقول للوكيل
      // ماذا يفعل، ولا لماذا يحتاجه تطبيقُ صرافة أصلاً.
      if (mounted) {
        _toast('التسجيل يحتاج إذن الميكروفون. افتح إعدادات التطبيق وامنحه الإذن.');
      }
      return;
    }

    if (!await _recorder.start()) return;
    if (!mounted) return;

    setState(() {
      _recElapsed = Duration.zero;
      _recPaused = false;
    });

    // «يسجّل رسالة صوتية» للطرف الآخر (البند 17).
    _setTyping('RECORDING');

    _recTimer = Timer.periodic(const Duration(milliseconds: 200), (_) async {
      if (!mounted) return;
      final a = await _recorder.amplitude();
      if (!mounted) return;
      setState(() {
        _recElapsed = _recorder.elapsed;
        _recAmp = a;
      });
    });
  }

  Future<void> _pauseResumeRecording() async {
    if (_recPaused) {
      await _recorder.resume();
    } else {
      await _recorder.pause();
    }
    if (mounted) setState(() => _recPaused = !_recPaused);
  }

  Future<void> _cancelRecording() async {
    _recTimer?.cancel();
    await _recorder.cancel();
    _setTyping('NONE');
    if (mounted) setState(() => _recPaused = false);
  }

  Future<void> _finishRecording() async {
    _recTimer?.cancel();
    final path = await _recorder.stop();
    _setTyping('NONE');
    if (!mounted) return;
    setState(() => _recPaused = false);

    // تسجيلٌ أقصر من ثانية ضغطةٌ بالخطأ لا رسالة.
    if (path == null || _recElapsed.inMilliseconds < 900) {
      if (path != null) {
        try {
          await File(path).delete();
        } catch (_) {}
      }
      return;
    }

    await _send(filePath: path);
  }

  /// يُعلن الخادمَ أنّي أكتب أو أسجّل (البندان 16–17).
  ///
  /// صامتٌ في الفشل: مؤشّرٌ لم يصل لا يستحقّ رسالة خطأ على شاشة الوكيل.
  void _setTyping(String state) {
    if (widget.threadId == null && !widget.asEmployee) return;
    unawaited(() async {
      try {
        if (widget.asEmployee) {
          await _repo.employeeTyping(state);
        } else {
          await _repo.typing(widget.threadId!, state);
        }
      } catch (_) {}
    }());
  }

  Future<void> _delete(ChatMessage m) async {
    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => const _ConfirmDelete(),
    );
    if (ok != true || !mounted) return;

    try {
      if (widget.asEmployee) {
        await _repo.employeeDeleteMessage(m.id);
      } else {
        await _repo.deleteMessage(widget.threadId!, m.id);
      }
      if (!mounted) return;
      // الحذف ناعم في الخادم، فنعكسه هنا بلا إعادة جلب المحادثة كلّها.
      setState(() {
        _messages = [
          for (final x in _messages)
            if (x.id == m.id)
              ChatMessage(
                id: x.id,
                senderKind: x.senderKind,
                senderName: x.senderName,
                body: '',
                createdAt: x.createdAt,
                deleted: true,
              )
            else
              x,
        ];
      });
    } catch (e) {
      if (mounted) _toast('تعذّر الحذف. $e');
    }
  }

  void _toast(String text) => ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text(text, style: T.kufi(13, FontWeight.w600))),
      );

  void _toBottom({bool jump = false}) {
    // بعد الإطار: الرسالة لم تُرسم بعد حين نُستدعى، فقياس الامتداد الآن
    // يقفز إلى ما قبلها بسطر.
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (!mounted || !_scroll.hasClients) return;
      final to = _scroll.position.maxScrollExtent;
      if (jump) {
        _scroll.jumpTo(to);
      } else {
        _scroll.animateTo(to,
            duration: const Duration(milliseconds: 240), curve: Curves.easeOut);
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: widget.title, onBack: () => context.pop()),
          Expanded(child: _body()),
          if (_replyTo != null)
            _ReplyBar(
              message: _replyTo!,
              onCancel: () => setState(() => _replyTo = null),
            ),
          if (_recorder.isRecording)
            _RecordingBar(
              elapsed: _recElapsed,
              paused: _recPaused,
              amplitude: _recAmp,
              onCancel: _cancelRecording,
              onPauseResume: _pauseResumeRecording,
              onSend: _finishRecording,
            )
          else
          _Composer(
            controller: _input,
            focusNode: _inputFocus,
            sending: _sending,
            emojiOpen: _emoji,
            hasText: _input.text.trim().isNotEmpty,
            onRecord: _startRecording,
            onSend: _send,
            onEmoji: () {
              if (_emoji) {
                setState(() => _emoji = false);
              } else {
                // إغلاق لوحة المفاتيح أولاً وإلا تراكمت اللوحتان.
                FocusScope.of(context).unfocus();
                setState(() => _emoji = true);
              }
            },
            onAttach: _attachSheet,
          ),
          if (_emoji)
            EmojiPicker(onPick: (e) {
              // الإدراج عند المؤشّر لا في آخر النصّ: من عاد ليصحّح كلمةً في
              // وسط رسالته يجد الرمز حيث وضع إصبعه.
              final sel = _input.selection;
              final at = sel.isValid ? sel.start : _input.text.length;
              final t = _input.text;
              _input.text = t.substring(0, at) + e + t.substring(at);
              _input.selection =
                  TextSelection.collapsed(offset: at + e.length);
            }),
        ],
      ),
    );
  }

  Future<void> _attachSheet() async {
    FocusScope.of(context).unfocus();
    final source = await showModalBottomSheet<ImageSource>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => const _AttachSheet(),
    );
    if (source != null) await _pickImage(source);
  }

  Widget _body() {
    if (_loading) {
      return Center(
        child: CircularProgressIndicator(color: R.primary, strokeWidth: 2.4),
      );
    }
    if (_error != null) {
      return _Failed(message: _error!, onRetry: () {
        setState(() => _loading = true);
        _load();
      });
    }
    if (_messages.isEmpty) return const _EmptyChat();

    return Column(
      children: [
        // «يكتب الآن» / «يسجّل رسالة صوتية» (البندان 16–17).
        if (_typing != null) _TypingBar(typing: _typing!),
        Expanded(
          child: ListView.builder(
            controller: _scroll,
            padding: const EdgeInsets.fromLTRB(R.padScreen, 16, R.padScreen, 16),
            itemCount: _messages.length,
            itemBuilder: (_, i) {
              final m = _messages[i];
              final mine = m.senderKind == _me;

              // فاصل التاريخ حين يتغيّر اليوم (البند 78): «اليوم» و«أمس»
              // ثم التاريخ — فلا يقرأ الوكيل وقتاً بلا يومه.
              final sep = _daySeparator(i);

              return Column(
                children: [
                  if (sep != null) _DayChip(label: sep),
                  GestureDetector(
                    // ضغطةٌ مطوّلة تفتح خيارات الرسالة — كما اعتاد المستخدم.
                    onLongPress:
                        m.deleted ? null : () => _messageMenu(m, mine),
                    child: ChatBubble(
                      message: m,
                      mine: mine,
                      receipts: _receipts,
                      reactions: _reactions[m.id] ?? const {},
                      starred: _starred.contains(m.id),
                      onTapReaction: (e) => _react(m, e),
                      imageUrl:
                          m.hasAttachment ? _url(m.attachmentPath) : '',
                      imageHeaders: _headers,
                      onTapImage: m.isImage
                          ? () => _openImage(_url(m.attachmentPath))
                          : null,
                    ),
                  ),
                ],
              );
            },
          ),
        ),
      ],
    );
  }

  /// نصّ فاصل اليوم، أو null إن كانت الرسالة في يوم سابقتها.
  String? _daySeparator(int i) {
    final cur = DateTime.tryParse(_messages[i].createdAt);
    if (cur == null) return null;
    if (i > 0) {
      final prev = DateTime.tryParse(_messages[i - 1].createdAt);
      if (prev != null &&
          prev.year == cur.year &&
          prev.month == cur.month &&
          prev.day == cur.day) {
        return null;
      }
    }

    final now = DateTime.now();
    final today = DateTime(now.year, now.month, now.day);
    final day = DateTime(cur.year, cur.month, cur.day);
    final diff = today.difference(day).inDays;

    if (diff == 0) return 'اليوم';
    if (diff == 1) return 'أمس';
    return '${day.year}-${day.month.toString().padLeft(2, '0')}'
        '-${day.day.toString().padLeft(2, '0')}';
  }

  Future<void> _react(ChatMessage m, String emoji) async {
    // تفاؤلٌ في الواجهة: التفاعل يظهر فوراً ثم يُثبَّت في الخادم. النبضة
    // التالية تصحّحه إن فشل — وانتظارُ الشبكة على ضغطةٍ صغيرة يجعلها تبدو
    // معطّلة.
    setState(() {
      final cur = Map<String, ReactionCount>.from(_reactions[m.id] ?? const {});
      final old = cur[emoji];
      if (old != null && old.mine) {
        if (old.count <= 1) {
          cur.remove(emoji);
        } else {
          cur[emoji] = ReactionCount(count: old.count - 1, mine: false);
        }
      } else {
        // تفاعلي السابق برمزٍ آخر يُرفع: واحدٌ لكل شخص.
        for (final e in cur.keys.toList()) {
          final c = cur[e]!;
          if (!c.mine) continue;
          if (c.count <= 1) {
            cur.remove(e);
          } else {
            cur[e] = ReactionCount(count: c.count - 1, mine: false);
          }
        }
        cur[emoji] = ReactionCount(count: (old?.count ?? 0) + 1, mine: true);
      }
      _reactions = {..._reactions, m.id: cur};
    });

    try {
      if (widget.asEmployee) {
        await _repo.employeeReact(m.id, emoji);
      } else {
        await _repo.react(widget.threadId!, m.id, emoji);
      }
    } catch (_) {
      // النبضة التالية تعيد الحقيقة من الخادم.
    }
  }

  Future<void> _messageMenu(ChatMessage m, bool mine) async {
    final action = await showModalBottomSheet<String>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _MessageMenu(
        canDelete: mine,
        // التعديل لصاحب رسالةٍ نصّية خلال المهلة — والخادم يفرضها كذلك.
        canEdit: mine && !m.hasAttachment && m.body.isNotEmpty,
        starred: _starred.contains(m.id),
        pinned: m.pinned,
        hasText: m.body.isNotEmpty,
      ),
    );
    if (!mounted || action == null) return;

    // رمزٌ سريع من صفّ التفاعلات أعلى القائمة.
    if (action.startsWith('emoji:')) {
      await _react(m, action.substring(6));
      return;
    }

    switch (action) {
      case 'reply':
        setState(() => _replyTo = m);
        _inputFocus.requestFocus();
      case 'copy':
        await Clipboard.setData(ClipboardData(text: m.body));
        if (mounted) _toast('نُسخ النصّ.');
      case 'edit':
        await _edit(m);
      case 'star':
        await _star(m);
      case 'pin':
        await _pin(m);
      case 'delete':
        await _delete(m);
    }
  }

  /// تعديل رسالة (البند 28) — والخادم يفرض المهلة والملكية.
  Future<void> _edit(ChatMessage m) async {
    final text = await showModalBottomSheet<String>(
      context: context,
      useRootNavigator: true,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _EditSheet(initial: m.body),
    );
    if (text == null || !mounted || text.trim() == m.body.trim()) return;

    try {
      if (widget.asEmployee) {
        await _repo.employeeEditMessage(m.id, text);
      } else {
        await _repo.editMessage(widget.threadId!, m.id, text);
      }
      if (!mounted) return;
      setState(() {
        _messages = [
          for (final x in _messages)
            if (x.id == m.id) x.copyWith(body: text.trim(), edited: true) else x,
        ];
      });
    } catch (e) {
      if (mounted) _toast('$e');
    }
  }

  Future<void> _star(ChatMessage m) async {
    final on = !_starred.contains(m.id);
    setState(() => _starred = on
        ? {..._starred, m.id}
        : (_starred.toSet()..remove(m.id)));
    try {
      if (widget.asEmployee) {
        await _repo.employeeStarMessage(m.id, on);
      } else {
        await _repo.starMessage(widget.threadId!, m.id, on);
      }
    } catch (_) {}
  }

  Future<void> _pin(ChatMessage m) async {
    final on = !m.pinned;
    setState(() {
      _messages = [
        for (final x in _messages)
          if (x.id == m.id) x.copyWith(pinned: on) else x,
      ];
    });
    try {
      if (widget.asEmployee) {
        await _repo.employeePinMessage(m.id, on);
      } else {
        await _repo.pinMessage(widget.threadId!, m.id, on);
      }
    } catch (_) {}
  }

  void _openImage(String url) => Navigator.of(context, rootNavigator: true).push(
        MaterialPageRoute(
          builder: (_) => _ImageViewer(url: url, headers: _headers),
        ),
      );
}

/// شريط «تردّ على…» فوق صندوق الكتابة.
class _ReplyBar extends StatelessWidget {
  const _ReplyBar({required this.message, required this.onCancel});

  final ChatMessage message;
  final VoidCallback onCancel;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(R.padScreen, 8, R.padScreen, 8),
        decoration: BoxDecoration(
          color: R.primaryA(.06),
          border: Border(top: BorderSide(color: R.primaryA(.18))),
        ),
        child: Row(
          children: [
            Container(width: 3, height: 32, color: R.primary),
            const SizedBox(width: 10),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisSize: MainAxisSize.min,
                children: [
                  Text(
                    message.senderName.isEmpty
                        ? 'ردّ على رسالة'
                        : 'ردّ على ${message.senderName}',
                    style: T.plex(11, FontWeight.w700, color: R.primaryDark),
                  ),
                  const SizedBox(height: 2),
                  Text(
                    message.body.isNotEmpty
                        ? message.body
                        : (message.isImage ? '📷 صورة' : '📎 مرفق'),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: T.kufi(12, FontWeight.w400, color: R.inkA(.6)),
                  ),
                ],
              ),
            ),
            IconButton(
              onPressed: onCancel,
              icon: Icon(Icons.close_rounded, size: 19, color: R.inkA(.5)),
              constraints: const BoxConstraints(minWidth: 40, minHeight: 40),
            ),
          ],
        ),
      );
}

/// صندوق الكتابة.
///
/// ⚠ **بلا `AutoClearFocus`** خلافاً لكل حقول التطبيق: تلك تُفرغ الحقل عند
/// دخول المؤشّر — وهو الصواب في حقل مبلغ، وكارثة في رسالةٍ نصفُها مكتوب.
class _Composer extends StatelessWidget {
  const _Composer({
    required this.controller,
    required this.focusNode,
    required this.sending,
    required this.emojiOpen,
    required this.hasText,
    required this.onSend,
    required this.onEmoji,
    required this.onAttach,
    required this.onRecord,
  });

  final TextEditingController controller;
  final FocusNode focusNode;
  final bool sending;
  final bool emojiOpen;
  final bool hasText;
  final VoidCallback onSend;
  final VoidCallback onEmoji;
  final VoidCallback onAttach;
  final VoidCallback onRecord;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(R.padScreen, 10, R.padScreen, 10),
        decoration: BoxDecoration(
          color: R.whiteA(.92),
          border: Border(top: BorderSide(color: R.inkA(.07))),
        ),
        child: SafeArea(
          top: false,
          child: Row(
            crossAxisAlignment: CrossAxisAlignment.end,
            children: [
              Expanded(
                child: Container(
                  constraints: const BoxConstraints(maxHeight: 120),
                  padding: const EdgeInsets.only(right: 4, left: 4),
                  decoration: BoxDecoration(
                    color: Colors.white,
                    border: Border.all(color: R.inkA(.12)),
                    borderRadius: BorderRadius.circular(22),
                  ),
                  child: Row(
                    crossAxisAlignment: CrossAxisAlignment.end,
                    children: [
                      _RoundIcon(
                        icon: emojiOpen
                            ? Icons.keyboard_alt_outlined
                            : Icons.emoji_emotions_outlined,
                        onTap: onEmoji,
                      ),
                      Expanded(
                        child: TextField(
                          controller: controller,
                          focusNode: focusNode,
                          minLines: 1,
                          maxLines: 5,
                          // 2000 هو حدّ الخادم — يُفرض هنا أيضاً ليرى الوكيل
                          // الحدّ وهو يكتب، لا بعد أن يضغط إرسال فيُرفض.
                          maxLength: 2000,
                          textInputAction: TextInputAction.newline,
                          keyboardType: TextInputType.multiline,
                          style: T.kufi(14.5, FontWeight.w500, height: 1.45),
                          decoration: InputDecoration(
                            counterText: '',
                            isDense: true,
                            contentPadding:
                                const EdgeInsets.symmetric(vertical: 12),
                            border: InputBorder.none,
                            hintText: 'اكتب رسالتك…',
                            hintStyle: T.kufi(14, FontWeight.w400,
                                color: R.inkA(.4)),
                          ),
                        ),
                      ),
                      _RoundIcon(icon: Icons.attach_file_rounded, onTap: onAttach),
                    ],
                  ),
                ),
              ),
              const SizedBox(width: 8),
              // ميكروفون حين لا نصّ، وسهم إرسال حين يوجد — كما هو مألوف:
              // زرّان دائمان يجعلان الوكيل يبحث في كل مرّة عن أيّهما يريد.
              if (hasText)
                _SendButton(sending: sending, onTap: onSend)
              else
                _MicButton(onStart: onRecord),
            ],
          ),
        ),
      );
}

class _RoundIcon extends StatelessWidget {
  const _RoundIcon({required this.icon, required this.onTap});

  final IconData icon;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(99),
        // 44 هو أصغر هدف لمس مقبول — أيقونةٌ أصغر منه تُخطأ باستمرار.
        child: SizedBox(
          width: 40,
          height: 44,
          child: Icon(icon, size: 21, color: R.inkA(.5)),
        ),
      );
}

/// زرّ الميكروفون — ضغطةٌ تبدأ التسجيل، وشريط التسجيل يتولّى الباقي.
///
/// ضغطةٌ لا ضغطٌ مستمرّ (البند 19 يطلب «قفل التسجيل»): إبقاء الإصبع دقيقتين
/// على الشاشة متعبٌ، والقفل هو ما يحلّ ذلك — فنبدأ منه مباشرة.
class _MicButton extends StatelessWidget {
  const _MicButton({required this.onStart});

  final VoidCallback onStart;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onStart,
        borderRadius: BorderRadius.circular(99),
        child: Container(
          width: 46,
          height: 46,
          alignment: Alignment.center,
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            gradient: R.primaryGradient,
          ),
          child: const Icon(Icons.mic_rounded, size: 21, color: Colors.white),
        ),
      );
}

/// شريط التسجيل — يحلّ محلّ صندوق الكتابة أثناء التسجيل.
///
/// ثلاثة أفعال ظاهرة لا مخفيّة: إلغاء · إيقاف مؤقّت/استئناف · إرسال. والسحب
/// للإلغاء (البند 19) مذكورٌ في النصّ تحت المؤقّت، والزرّ الأحمر يفعله لمن
/// لم يسحب.
class _RecordingBar extends StatelessWidget {
  const _RecordingBar({
    required this.elapsed,
    required this.paused,
    required this.amplitude,
    required this.onCancel,
    required this.onPauseResume,
    required this.onSend,
  });

  final Duration elapsed;
  final bool paused;
  final double amplitude;
  final VoidCallback onCancel;
  final VoidCallback onPauseResume;
  final VoidCallback onSend;

  @override
  Widget build(BuildContext context) {
    final m = elapsed.inMinutes;
    final s = elapsed.inSeconds % 60;

    return Container(
      padding: const EdgeInsets.fromLTRB(R.padScreen, 10, R.padScreen, 10),
      decoration: BoxDecoration(
        color: R.whiteA(.96),
        border: Border(top: BorderSide(color: R.inkA(.07))),
      ),
      child: SafeArea(
        top: false,
        child: Row(
          children: [
            IconButton(
              onPressed: onCancel,
              icon: Icon(Icons.delete_outline_rounded, size: 23, color: R.error),
              constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
            ),
            // نقطةٌ حمراء تنبض ما دام التسجيل جارياً، وتثبت عند الإيقاف
            // المؤقّت — إشارةٌ واحدة تقول الحالة بلا كلمة.
            AnimatedOpacity(
              opacity: paused ? .35 : (0.45 + amplitude * .55),
              duration: const Duration(milliseconds: 160),
              child: Container(
                width: 10,
                height: 10,
                decoration: BoxDecoration(
                  color: R.error,
                  shape: BoxShape.circle,
                ),
              ),
            ),
            const SizedBox(width: 10),
            Directionality(
              textDirection: TextDirection.ltr,
              child: Text('$m:${s.toString().padLeft(2, '0')}',
                  style: T.plex(14, FontWeight.w700, color: R.ink)),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: Text(paused ? 'التسجيل متوقّف' : 'يسجّل…',
                  style: T.kufi(12.5, FontWeight.w500, color: R.inkA(.5))),
            ),
            IconButton(
              onPressed: onPauseResume,
              icon: Icon(paused ? Icons.play_arrow_rounded : Icons.pause_rounded,
                  size: 24, color: R.primaryDark),
              constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
            ),
            const SizedBox(width: 4),
            _SendButton(sending: false, onTap: onSend),
          ],
        ),
      ),
    );
  }
}

class _SendButton extends StatelessWidget {
  const _SendButton({required this.sending, required this.onTap});

  final bool sending;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: sending ? null : onTap,
        borderRadius: BorderRadius.circular(99),
        child: Container(
          width: 46,
          height: 46,
          alignment: Alignment.center,
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            gradient: R.primaryGradient,
          ),
          child: sending
              ? const SizedBox(
                  width: 18,
                  height: 18,
                  child: CircularProgressIndicator(
                      strokeWidth: 2, color: Colors.white),
                )
              : const Icon(Icons.send_rounded, size: 19, color: Colors.white),
        ),
      );
}

/// غلاف الورقة السفلية — الحشوة والحوافّ والمقبض، كما في بقية أوراق التطبيق.
///
/// ثلاث أوراق في هذا الملف تستعمله؛ نسخُه ثلاثاً كان يعني ورقةً تختلف عن
/// أختيها في أوّل تعديل.
class _SheetShell extends StatelessWidget {
  const _SheetShell({required this.child});

  final Widget child;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(22, 14, 22, 26),
        decoration: BoxDecoration(
          color: R.whiteA(.96),
          borderRadius:
              const BorderRadius.vertical(top: Radius.circular(R.rNav)),
        ),
        child: SafeArea(
          top: false,
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              Center(
                child: Container(
                  width: 44,
                  height: 4,
                  decoration: BoxDecoration(
                    color: R.inkA(.16),
                    borderRadius: BorderRadius.circular(99),
                  ),
                ),
              ),
              const SizedBox(height: 14),
              child,
            ],
          ),
        ),
      );
}

/// «يكتب الآن» أو «يسجّل رسالة صوتية» (البندان 16–17).
class _TypingBar extends StatelessWidget {
  const _TypingBar({required this.typing});

  final ChatTyping typing;

  @override
  Widget build(BuildContext context) => Container(
        width: double.infinity,
        padding: const EdgeInsets.symmetric(horizontal: R.padScreen, vertical: 7),
        color: R.primaryA(.06),
        child: Row(
          children: [
            Icon(typing.isRecording ? Icons.mic_rounded : Icons.more_horiz_rounded,
                size: 15, color: R.primary),
            const SizedBox(width: 7),
            Text(
              typing.name.isEmpty ? typing.label : '${typing.name} ${typing.label}',
              style: T.kufi(12, FontWeight.w600, color: R.primaryDark),
            ),
          ],
        ),
      );
}

/// فاصل اليوم بين الرسائل (البند 78).
class _DayChip extends StatelessWidget {
  const _DayChip({required this.label});

  final String label;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.only(bottom: 12, top: 2),
        child: Center(
          child: Container(
            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 5),
            decoration: BoxDecoration(
              color: R.inkA(.06),
              borderRadius: BorderRadius.circular(99),
            ),
            child: Directionality(
              // «2026-09-05» رقمٌ لاتيني — يُفرض اتجاهه وإلا انقلب.
              textDirection: TextDirection.ltr,
              child: Text(label,
                  style: T.plex(11, FontWeight.w600, color: R.inkA(.55))),
            ),
          ),
        ),
      );
}

/// ورقة تعديل الرسالة (البند 28).
class _EditSheet extends StatefulWidget {
  const _EditSheet({required this.initial});

  final String initial;

  @override
  State<_EditSheet> createState() => _EditSheetState();
}

class _EditSheetState extends State<_EditSheet> {
  late final _c = TextEditingController(text: widget.initial);

  @override
  void dispose() {
    _c.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => Padding(
        // فوق لوحة المفاتيح: الورقة تحتها تجعل الوكيل يكتب فيما لا يراه.
        padding: EdgeInsets.only(bottom: MediaQuery.viewInsetsOf(context).bottom),
        child: _SheetShell(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              Text('تعديل الرسالة', style: T.kufi(16, FontWeight.w700)),
              const SizedBox(height: 6),
              Text('ستظهر «مُعدَّلة» بجانبها.',
                  style: T.plex(12, FontWeight.w400, color: R.inkA(.5))),
              const SizedBox(height: 14),
              TextField(
                controller: _c,
                autofocus: true,
                minLines: 1,
                maxLines: 6,
                maxLength: 2000,
                style: T.kufi(14.5, FontWeight.w500, height: 1.45),
                decoration: InputDecoration(
                  counterText: '',
                  filled: true,
                  fillColor: Colors.white,
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(R.rRow),
                    borderSide: BorderSide(color: R.inkA(.12)),
                  ),
                ),
              ),
              const SizedBox(height: 16),
              PrimaryButton(
                label: 'حفظ',
                onPressed: () => Navigator.pop(context, _c.text),
              ),
            ],
          ),
        ),
      );
}

/// خيارات الرسالة عند الضغط المطوّل — وأعلاها صفّ تفاعلٍ سريع (البند 25).
class _MessageMenu extends StatelessWidget {
  const _MessageMenu({
    required this.canDelete,
    required this.canEdit,
    required this.starred,
    required this.pinned,
    required this.hasText,
  });

  final bool canDelete;
  final bool canEdit;
  final bool starred;
  final bool pinned;
  final bool hasText;

  /// الستّة التي نصّ عليها البند 25، وبقيّة الرموز في لوحة الإيموجي.
  static const _quick = ['👍', '❤️', '😂', '😮', '😢', '🙏'];

  @override
  Widget build(BuildContext context) => _SheetShell(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              children: [
                for (final e in _quick)
                  InkWell(
                    onTap: () => Navigator.pop(context, 'emoji:$e'),
                    borderRadius: BorderRadius.circular(99),
                    child: Padding(
                      padding: const EdgeInsets.all(7),
                      child: Text(e, style: const TextStyle(fontSize: 25)),
                    ),
                  ),
              ],
            ),
            const SizedBox(height: 8),
            Divider(color: R.inkA(.07), height: 1),
            const SizedBox(height: 4),

            _MenuItem(
              icon: Icons.reply_rounded,
              label: 'ردّ',
              onTap: () => Navigator.pop(context, 'reply'),
            ),
            if (hasText)
              _MenuItem(
                icon: Icons.copy_rounded,
                label: 'نسخ النصّ',
                onTap: () => Navigator.pop(context, 'copy'),
              ),
            if (canEdit)
              _MenuItem(
                icon: Icons.edit_outlined,
                label: 'تعديل',
                onTap: () => Navigator.pop(context, 'edit'),
              ),
            _MenuItem(
              icon: starred ? Icons.star_rounded : Icons.star_outline_rounded,
              label: starred ? 'إزالة من المهمّة' : 'حفظ كمهمّة',
              onTap: () => Navigator.pop(context, 'star'),
            ),
            _MenuItem(
              icon: pinned
                  ? Icons.push_pin_rounded
                  : Icons.push_pin_outlined,
              label: pinned ? 'إلغاء التثبيت' : 'تثبيت في المحادثة',
              onTap: () => Navigator.pop(context, 'pin'),
            ),
            // الحذف لصاحب الرسالة وحده — والخادم يرفض غيره كذلك.
            if (canDelete)
              _MenuItem(
                icon: Icons.delete_outline_rounded,
                label: 'حذف',
                danger: true,
                onTap: () => Navigator.pop(context, 'delete'),
              ),
          ],
        ),
      );
}

class _AttachSheet extends StatelessWidget {
  const _AttachSheet();

  @override
  Widget build(BuildContext context) => _SheetShell(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            _MenuItem(
              icon: Icons.photo_camera_outlined,
              label: 'كاميرا',
              onTap: () => Navigator.pop(context, ImageSource.camera),
            ),
            _MenuItem(
              icon: Icons.photo_library_outlined,
              label: 'من المعرض',
              onTap: () => Navigator.pop(context, ImageSource.gallery),
            ),
          ],
        ),
      );
}

class _ConfirmDelete extends StatelessWidget {
  const _ConfirmDelete();

  @override
  Widget build(BuildContext context) => _SheetShell(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Text('حذف الرسالة؟',
                textAlign: TextAlign.center,
                style: T.kufi(16, FontWeight.w700)),
            const SizedBox(height: 8),
            Text('ستظهر للطرف الآخر «حُذفت هذه الرسالة».',
                textAlign: TextAlign.center,
                style: T.plex(12.5, FontWeight.w400, color: R.inkA(.55))),
            const SizedBox(height: 20),
            // زرٌّ أحمر مبنيّ هنا لا `PrimaryButton`: تلك بتدرّج العلامة،
            // وفعلٌ لا رجعة فيه لا يُقدَّم بلون الشركة.
            SizedBox(
              height: 52,
              child: FilledButton(
                onPressed: () => Navigator.pop(context, true),
                style: FilledButton.styleFrom(
                  backgroundColor: R.error,
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(R.rCard)),
                ),
                child: Text('حذف',
                    style: T.kufi(15, FontWeight.w700, color: Colors.white)),
              ),
            ),
            const SizedBox(height: 10),
            GlassButton(
              label: 'تراجع',
              onPressed: () => Navigator.pop(context, false),
            ),
          ],
        ),
      );
}

class _MenuItem extends StatelessWidget {
  const _MenuItem({
    required this.icon,
    required this.label,
    required this.onTap,
    this.danger = false,
  });

  final IconData icon;
  final String label;
  final VoidCallback onTap;
  final bool danger;

  @override
  Widget build(BuildContext context) {
    final c = danger ? R.error : R.primaryDark;

    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(R.rRow),
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 6, vertical: 14),
        child: Row(
          children: [
            Icon(icon, size: 20, color: c),
            const SizedBox(width: 14),
            Text(label, style: T.kufi(14.5, FontWeight.w600, color: c)),
          ],
        ),
      ),
    );
  }
}

/// عارض الصورة بحجمها الكامل، بتكبيرٍ وتصغير.
class _ImageViewer extends StatelessWidget {
  const _ImageViewer({required this.url, required this.headers});

  final String url;
  final Map<String, String> headers;

  @override
  Widget build(BuildContext context) => Scaffold(
        backgroundColor: Colors.black,
        appBar: AppBar(
          backgroundColor: Colors.black,
          foregroundColor: Colors.white,
          elevation: 0,
        ),
        body: Center(
          child: InteractiveViewer(
            minScale: 1,
            maxScale: 4,
            child: Image.network(
              url,
              headers: headers,
              errorBuilder: (_, _, _) => const Icon(
                  Icons.broken_image_outlined, color: Colors.white54, size: 44),
            ),
          ),
        ),
      );
}

class _EmptyChat extends StatelessWidget {
  const _EmptyChat();

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 40),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(Icons.forum_outlined, size: 46, color: R.primaryA(.35)),
              const SizedBox(height: 14),
              Text('لا رسائل بعد',
                  style: T.kufi(15, FontWeight.w700, color: R.inkA(.7))),
              const SizedBox(height: 8),
              Text('اكتب أوّل رسالة في الأسفل.',
                  textAlign: TextAlign.center,
                  style: T.plex(12.5, FontWeight.w400, color: R.inkA(.5))),
            ],
          ),
        ),
      );
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 32),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(Icons.error_outline_rounded, size: 40, color: R.error),
              const SizedBox(height: 12),
              Text(message,
                  textAlign: TextAlign.center,
                  style: T.plex(12.5, FontWeight.w400, color: R.inkA(.6))),
              const SizedBox(height: 16),
              GlassButton(label: 'إعادة المحاولة', onPressed: onRetry),
            ],
          ),
        ),
      );
}
