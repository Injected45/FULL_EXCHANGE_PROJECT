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
    this.highlightMessageId,
  });

  /// رسالةٌ تُفتح المحادثة عليها وتُومض بلونٍ ظاهر (أمر المالك، 6 سبتمبر
  /// 2026): من فتح رسالةً مهمّة يريد **الرسالة**، لا المحادثة التي قيلت
  /// فيها.
  final int? highlightMessageId;

  final String title;

  /// رقم المحادثة في وضع الوكيل. لا يُمرَّر في وضع الموظّف — الخادم يعرف
  /// محادثته من جلسته، وتمريرُ رقمٍ من الهاتف يفتح باب محادثة موظّفٍ آخر.
  final int? threadId;

  final bool asEmployee;

  @override
  ConsumerState<ChatScreen> createState() => _ChatScreenState();
}

class _ChatScreenState extends ConsumerState<ChatScreen>
    with WidgetsBindingObserver {
  final _input = TextEditingController();
  final _scroll = ScrollController();
  final _inputFocus = FocusNode();

  List<ChatMessage> _messages = const [];
  ChatReceipts _receipts = const ChatReceipts();
  Map<String, String> _headers = const {};
  Map<int, Map<String, ReactionCount>> _reactions = const {};
  Set<int> _starred = const {};
  ChatTyping? _typing;
  ChatMessage? _pinned;

  bool _loading = true;
  bool _emoji = false;
  String? _error;

  /// الرسالة التي يُردّ عليها الآن — تُعرض فوق صندوق الكتابة.
  ChatMessage? _replyTo;

  Timer? _timer;

  /// ── النبض المتكيّف ─────────────────────────────────────────────────
  ///
  /// النبضة تحمل `after_id` فتعود فارغةً حين لا جديد — سطرٌ في السجلّ لا
  /// تاريخُ محادثةٍ كامل.
  ///
  /// وكانت خمس ثوانٍ ثابتة، وهي اختيارٌ سيّئ في الحالين: بطيئةٌ أثناء حديثٍ
  /// جارٍ — يكتب الوكيل ثم ينتظر خمساً ليرى الردّ — ومُسرفةٌ على محادثةٍ لم
  /// يكتب فيها أحدٌ منذ ساعة، وهي على بطارية هاتف.
  ///
  /// فالفترة تتبع الحال: **1.2 ثانية** ما دام أحدهم يكتب أو مرّت رسالةٌ في
  /// الدقيقة الماضية، و**4 ثوانٍ** حين تهدأ.
  ///
  /// وصار ذلك ممكناً لأن النبضة رخصت: **خمسُ رحلاتٍ إلى القاعدة بدل عشر**
  /// (مقيسة)، ورحلتان منها توثيقُ Sanctum نفسُه.
  static const _hot = Duration(milliseconds: 1200);
  static const _idle = Duration(seconds: 4);

  /// نافذةُ «الحديث جارٍ» — تُمدَّد مع كل رسالةٍ أو «يكتب الآن».
  static const _hotWindow = Duration(seconds: 60);
  DateTime _hotUntil = DateTime.fromMillisecondsSinceEpoch(0);

  /// من «أنا» في هذه المحادثة — به يُعرف جانب الفقاعة.
  String get _me => widget.asEmployee ? 'EMPLOYEE' : 'AGENT';

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addObserver(this);
    _load();
    _schedule();
    // إغلاق لوحة الإيموجي حين تُفتح لوحة المفاتيح: اللوحتان معاً تأكلان
    // الشاشة كلّها ولا تبقى للمحادثة سطراً.
    _inputFocus.addListener(() {
      if (_inputFocus.hasFocus && _emoji) setState(() => _emoji = false);
    });
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    // ⚠ النبضُ يتوقّف في الخلفية: لا شاشةَ تُقرأ، وإبقاؤه يستنزف البطارية
    // والشبكة ويُخاطر بـANR — كما توقف الهيكلُ وقائمةُ الموظف. وعند العودة
    // نبضةٌ فورية لا انتظارُ الدورة الكاملة.
    if (state == AppLifecycleState.resumed) {
      _poll();
      _schedule();
    } else if (state == AppLifecycleState.paused) {
      _timer?.cancel();
      _timer = null;
    }
  }

  @override
  void dispose() {
    WidgetsBinding.instance.removeObserver(this);
    _timer?.cancel();
    // مؤقّتان آخران يعيشان مع هذه الشاشة: إخفاء شارة التاريخ، ونبضة عدّاد
    // التسجيل. مؤقّتٌ ينجو من الهدم يستدعي `setState` على شاشةٍ ذهبت.
    _floatHide?.cancel();
    _recTimer?.cancel();
    _typingStop?.cancel();
    _highlightFade?.cancel();
    // ورفعُ «يكتب الآن» عند مغادرة الشاشة: من خرج وهو يكتب لا يبقى كذلك
    // عند الطرف الآخر ثماني ثوانٍ.
    _setTyping('NONE');
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
        _pinned = page.pinned;
        _headers = headers;
        _loading = false;
        _error = null;
      });
      // فُتحت على رسالةٍ بعينها ⇦ نقفز إليها بدل النزول إلى الأسفل.
      if (widget.highlightMessageId != null) {
        _jumpTo(widget.highlightMessageId!);
      } else {
        _toBottom(jump: true);
      }
    } catch (e) {
      if (!mounted) return;
      setState(() {
        _loading = false;
        _error = '$e';
      });
    }
  }

  // ── القفز إلى رسالة وإبرازها ────────────────────────────────────────

  /// الرسالة المُبرَزة الآن — تُومض ثم يخفت الإبراز.
  int? _highlight;
  Timer? _highlightFade;

  /// مفاتيح الفقاعات المبنيّة — للقفز الدقيق.
  ///
  /// تُملأ في `itemBuilder`، أي للمبنيّ وحده. ولذلك القفز على مرحلتين:
  /// تقديرٌ يُدخل الرسالة في نطاق البناء، ثم `ensureVisible` يضبطها بدقّة.
  final _keys = <int, GlobalKey>{};

  void _jumpTo(int messageId) {
    final i = _messages.indexWhere((m) => m.id == messageId);
    if (i < 0) {
      // ليست في الصفحة المحمَّلة (رسالةٌ قديمة): نفتح على الأسفل بلا إبراز
      // بدل أن نقفز إلى موضعٍ خطأ ونوهم أنها هي.
      _toBottom(jump: true);
      return;
    }

    setState(() => _highlight = messageId);

    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (!mounted || !_scroll.hasClients) return;

      // 1) تقديرٌ من نسبة موضعها في القائمة — يكفي لبنائها.
      final ratio = _messages.length <= 1 ? 0.0 : i / (_messages.length - 1);
      _scroll.jumpTo(
          (ratio * _scroll.position.maxScrollExtent).clamp(0.0, _scroll.position.maxScrollExtent));

      // 2) ضبطٌ دقيق بعد أن صارت مبنيّة.
      WidgetsBinding.instance.addPostFrameCallback((_) {
        final ctx = _keys[messageId]?.currentContext;
        if (ctx != null) {
          Scrollable.ensureVisible(ctx,
              duration: const Duration(milliseconds: 260),
              alignment: .35,
              curve: Curves.easeOut);
        }
      });
    });

    // يخفت وحده: إبرازٌ دائم يجعل الرسالة تبدو محدَّدة لا مُشاراً إليها.
    _highlightFade?.cancel();
    _highlightFade = Timer(const Duration(milliseconds: 2600), () {
      if (mounted) setState(() => _highlight = null);
    });
  }

  /// يدمج ما وصل من الخادم مع المعروض، **بلا تكرار**.
  ///
  /// حارسان لا واحد، لأن للتكرار مصدرين مختلفين:
  ///
  /// 1. **بالرقم** — طلبان متداخلان قد يعيدان الرسالة نفسها.
  /// 2. **بمُعرّف الجهاز** — الفقاعة المحلّية التي أضفناها عند الضغط تعود من
  ///    الخادم برقمٍ حقيقي؛ فتُستبدل بها ولا تُضاف بجانبها.
  ///
  /// وترتيبُ النتيجة بالرقم: النبضة قد تصل بعد فقاعةٍ محلّية أحدث، وإلحاقٌ
  /// بلا ترتيب كان يضع رسالة الطرف الآخر تحت رسالةٍ أرسلتُها بعدها.
  static List<ChatMessage> _merge(List<ChatMessage> current, List<ChatMessage> fresh) {
    final byId = {for (final m in current) if (m.id > 0) m.id};
    final byClient = {
      for (final m in current)
        if (m.clientId.isNotEmpty) m.clientId: m,
    };

    final out = [...current];

    for (final m in fresh) {
      if (byId.contains(m.id)) continue;

      // نسختي المحلّية عادت من الخادم: تُستبدل في مكانها.
      if (m.clientId.isNotEmpty && byClient.containsKey(m.clientId)) {
        final i = out.indexWhere((x) => x.clientId == m.clientId);
        if (i >= 0) {
          out[i] = m;
          continue;
        }
      }

      out.add(m);
    }

    out.sort((a, b) {
      // الفقاعات المحلّية (رقمها سالب) تبقى في الأسفل — هي الأحدث دائماً.
      if (a.id > 0 && b.id > 0) return a.id.compareTo(b.id);
      if (a.id <= 0 && b.id <= 0) return b.id.compareTo(a.id);
      return a.id > 0 ? -1 : 1;
    });

    return out;
  }

  /// آخر رقمٍ **من الخادم**. الفقاعات المحلّية أرقامها سالبة فتُتجاوز —
  /// وأخذُ `_messages.last.id` مباشرةً كان يرسل رقماً سالباً في `after_id`
  /// فيعيد الخادم المحادثة كلّها.
  int get _lastServerId {
    for (var i = _messages.length - 1; i >= 0; i--) {
      if (_messages[i].id > 0) return _messages[i].id;
    }
    return 0;
  }

  /// يجدول النبضة التالية بحسب الحال.
  ///
  /// `Timer` متسلسل لا `Timer.periodic`: الثاني يُطلق نبضةً جديدة ولو لم
  /// تعد السابقة، فتتكدّس الطلبات على شبكةٍ ضعيفة — وشبكةُ فرعٍ في الجنوب
  /// ليست شبكةَ مكتب. وهذا يبدأ العدّ **بعد** انتهاء النبضة.
  void _schedule() {
    _timer?.cancel();
    if (!mounted) return;

    final hot = DateTime.now().isBefore(_hotUntil);
    _timer = Timer(hot ? _hot : _idle, () async {
      await _poll();
      _schedule();
    });
  }

  /// يُبقي النبض سريعاً دقيقةً من الآن.
  void _markHot() => _hotUntil = DateTime.now().add(_hotWindow);

  /// جلبٌ تزايدي. صامتٌ في الفشل: انقطاع لحظي لا يُفرغ محادثةً بين يدي
  /// صاحبها، والنبضة التالية تُصلحه.
  Future<void> _poll() async {
    if (!mounted || _loading) return;
    final after = _lastServerId;
    try {
      final page = await _fetch(after);
      if (!mounted) return;

      // الإيصالات تُحدَّث دائماً ولو لم تصل رسالة: ✓ تصير ✓✓ حين يقرأ
      // الطرف الآخر، وذلك ليس رسالةً جديدة.
      setState(() {
        _receipts = page.receipts;
        _typing = page.typing;
        // المثبَّتة تُقرأ عند فتح المحادثة وحدها — انظر `pinnedKnown`.
        // وأخذُ `null` على ظاهرها هنا كان يُخفي الشريط بعد أوّل نبضة.
        if (page.pinnedKnown) _pinned = page.pinned;
        // التفاعلات تصل للصفحة المطلوبة وحدها؛ في الجلب التزايدي تخصّ
        // الرسائل الجديدة، فتُدمَج ولا تُستبدل — وإلا اختفت تفاعلات ما فوقها.
        if (page.reactions.isNotEmpty) {
          _reactions = {..._reactions, ...page.reactions};
        }
        if (page.starred.isNotEmpty) _starred = {..._starred, ...page.starred};
        if (page.items.isNotEmpty) _messages = _merge(_messages, page.items);
      });

      // رسالةٌ وصلت، أو الطرف الآخر يكتب ⇐ الحديث جارٍ فيُسرَّع النبض.
      if (page.items.isNotEmpty || page.typing != null) _markHot();
      // النزول إلى الأسفل **إن كان الوكيل هناك أصلاً**.
      //
      // بلا هذا الشرط تخطفه كل رسالةٍ واردة من موضعٍ يقرؤه — وهو ما كان
      // سيُلغي القفزة إلى رسالةٍ مهمّة بعد ثوانٍ من الوصول إليها.
      if (page.items.isNotEmpty && _nearBottom) _toBottom();
    } catch (_) {
      // انظر التوثيق أعلاه.
    }
  }

  /// هل الوكيل عند آخر المحادثة الآن؟
  ///
  /// 140 بكسلاً هامشٌ عملي: فقاعةٌ أو اثنتان. من ابتعد أكثر يقرأ شيئاً
  /// بعينه، ومن كان أقرب لم يغادر الأسفل حقّاً.
  bool get _nearBottom {
    if (!_scroll.hasClients) return true;
    final p = _scroll.position;
    return p.maxScrollExtent - p.pixels < 140;
  }

  /// عدّادٌ يضمن تفرّد مُعرّف الرسالة داخل الجلسة الواحدة.
  int _clientSeq = 0;

  /// إرسالٌ **متفائل**: الرسالة تظهر في المحادثة فور الضغط، ثم تُستبدل بما
  /// يعيده الخادم.
  ///
  /// هذا يحلّ عطبين معاً كانا يظهران للوكيل:
  ///
  /// 1. **التأخّر عند الإرسال.** كنّا ننتظر ردّ الخادم قبل عرض الرسالة، فعلى
  ///    شبكة فرعٍ بطيئة يضغط الوكيل ولا يرى شيئاً ثانيةً أو ثانيتين — فيظنّ
  ///    الضغطة لم تُسجَّل ويضغط ثانية. الآن تظهر فوراً بعلامة «جارٍ».
  ///
  /// 2. **ظهور الرسالة مرّتين.** الاستطلاع كان ينطلق بـ`after_id` محسوبٍ قبل
  ///    الإرسال، فيعود بالرسالة نفسها **بعد** أن أضفناها محلّياً. والعلاج
  ///    `client_id`: مُعرّفٌ يولّده الجهاز قبل الإرسال، فتُطابَق به النسخة
  ///    المحلّية مع ما يعود — ويُهمَل المكرّر. وهو نفسه ما يجعل الخادم يردّ
  ///    الرسالة القائمة بدل كتابة ثانية عند إعادة المحاولة (البند 68).
  Future<void> _send({String? filePath}) async {
    final body = _input.text.trim();
    if (body.isEmpty && filePath == null) return;

    final cid = 'c${DateTime.now().millisecondsSinceEpoch}_${_clientSeq++}';
    final reply = _replyTo;

    // أرسلتُ ⇐ ردٌّ متوقَّع، فيُسرَّع النبض من الآن لا بعد وصوله.
    _markHot();
    _schedule();

    // الفقاعة المحلّية. رقمها سالبٌ فلا يصطدم برقم من الخادم، ولا يدخل في
    // حساب `after_id` — انظر `_lastServerId`.
    final local = ChatMessage(
      id: -(_clientSeq),
      senderKind: _me,
      senderName: '',
      body: body,
      createdAt: DateTime.now().toIso8601String(),
      clientId: cid,
      sendState: SendState.sending,
      localPath: filePath ?? '',
      replyToId: reply?.id,
      replyBody: reply?.body ?? '',
      replySenderName: reply?.senderName ?? '',
      attachmentKind: filePath == null
          ? ''
          : (filePath.endsWith('.ogg') ? 'AUDIO' : 'IMAGE'),
      attachmentPath: filePath ?? '',
    );

    // الحقل يُفرَغ الآن لأن النصّ صار في الفقاعة: لم يعد الإفراغ يضيّع شيئاً،
    // والرسالة الفاشلة تبقى معروضة بزرّ إعادة.
    _input.clear();
    setState(() {
      _messages = [..._messages, local];
      _replyTo = null;
    });
    _toBottom();

    await _deliver(local);
  }

  /// يرسل فقاعةً محلّية إلى الخادم ويستبدلها بما يعود — أو يسمها «فشل».
  Future<void> _deliver(ChatMessage local) async {
    try {
      final msg = widget.asEmployee
          ? await _repo.employeeSend(local.body,
              filePath: local.localPath.isEmpty ? null : local.localPath,
              replyToId: local.replyToId,
              clientId: local.clientId)
          : await _repo.send(widget.threadId!, local.body,
              filePath: local.localPath.isEmpty ? null : local.localPath,
              replyToId: local.replyToId,
              clientId: local.clientId);
      if (!mounted) return;

      setState(() {
        _messages = [
          for (final x in _messages)
            if (x.clientId == local.clientId && msg != null) msg else x,
        ];
      });
    } catch (_) {
      if (!mounted) return;
      setState(() {
        _messages = [
          for (final x in _messages)
            if (x.clientId == local.clientId)
              x.copyWith(sendState: SendState.failed)
            else
              x,
        ];
      });
    }
  }

  /// إعادة إرسال رسالةٍ فشلت (البند 67).
  ///
  /// بالمُعرّف نفسه: الخادم يردّ الرسالة القائمة إن كانت قد وصلت فعلاً في
  /// المحاولة الأولى — فلا تُكتب مرّتين.
  Future<void> _retry(ChatMessage m) async {
    setState(() {
      _messages = [
        for (final x in _messages)
          if (x.clientId == m.clientId)
            x.copyWith(sendState: SendState.sending)
          else
            x,
      ];
    });
    await _deliver(m);
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

  // ── «يكتب الآن» (البند 16) ──────────────────────────────────────────

  /// آخر مرّة أُعلن فيها أنّي أكتب — لخنق الإرسال.
  DateTime? _typingSentAt;
  Timer? _typingStop;

  /// يُستدعى مع كل حرف.
  ///
  /// **مخنوقٌ عمداً**: إعلانٌ مع كل ضغطة مفتاح يعني عشرات الطلبات في الجملة
  /// الواحدة. الحالة تعيش ثماني ثوانٍ في الخادم، فتجديدها كل ثلاث يكفي
  /// لإبقائها حيّة بلا انقطاع.
  ///
  /// ويُرفع الإعلان بعد سكونٍ قصير: من توقّف عن الكتابة لا يبقى «يكتب الآن»
  /// عند الطرف الآخر إلى أن تنتهي مهلة الخادم.
  void _onTextChanged() {
    // إعادة بناء الشريط: زرّ الإرسال يتبدّل بين ميكروفون وسهم حسب النصّ.
    setState(() {});

    final now = DateTime.now();
    if (_typingSentAt == null ||
        now.difference(_typingSentAt!) > const Duration(seconds: 3)) {
      _typingSentAt = now;
      _setTyping('TYPING');
    }

    _typingStop?.cancel();
    _typingStop = Timer(const Duration(seconds: 3), () {
      _typingSentAt = null;
      _setTyping('NONE');
    });
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
            // لا حالة «جارٍ» على الزرّ: الإرسال متفائل، والرسالة تظهر فوراً
            // بعلامتها الخاصة — وزرٌّ يدور بلا شيء ينتظره إرباك.
            sending: false,
            emojiOpen: _emoji,
            hasText: _input.text.trim().isNotEmpty,
            onChanged: _onTextChanged,
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
        // الرسالة المثبَّتة أعلى كل شيء: الغرض منها أن تُرى بلا بحث.
        if (_pinned != null)
          _PinnedBar(pinned: _pinned!, onUnpin: () => _pin(_pinned!, 0)),

        // «يكتب الآن» / «يسجّل رسالة صوتية» (البندان 16–17).
        if (_typing != null) _TypingBar(typing: _typing!),
        Expanded(
          child: Stack(
            children: [
              NotificationListener<ScrollNotification>(
                onNotification: _onScroll,
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

              final key = _keys.putIfAbsent(m.id, GlobalKey.new);
              final lit = _highlight == m.id;

              return Column(
                key: key,
                children: [
                  if (sep != null) _DayChip(label: sep),
                  // شريطٌ ملوّن حول الفقاعة يقول «هذه هي» ثم يخفت.
                  //
                  // يمتدّ عرض الشاشة لا حول الفقاعة وحدها: عينٌ تبحث بعد
                  // قفزةٍ تلتقط شريطاً عريضاً قبل أن تلتقط إطاراً رفيعاً.
                  AnimatedContainer(
                    duration: const Duration(milliseconds: 420),
                    curve: Curves.easeOut,
                    padding: const EdgeInsets.symmetric(horizontal: 6),
                    decoration: BoxDecoration(
                      color: lit
                          ? R.warnIcon.withValues(alpha: .22)
                          : Colors.transparent,
                      borderRadius: BorderRadius.circular(14),
                    ),
                    child: GestureDetector(
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
                      onRetry: m.failed ? () => _retry(m) : null,
                      imageUrl:
                          m.hasAttachment ? _url(m.attachmentPath) : '',
                      imageHeaders: _headers,
                      onTapImage: m.isImage
                          ? () => _openImage(_url(m.attachmentPath))
                          : null,
                    ),
                  ),
                  ),
                ],
              );
            },
                ),
              ),

              // شارة التاريخ العائمة — تظهر أثناء السحب وتختفي بعده
              // (قرار المالك، 5 سبتمبر 2026).
              //
              // تقول اليوم الذي تقرؤه الآن، فيعرف الوكيل أين هو في المحادثة
              // بلا أن ينتظر بلوغ الفاصل الثابت. وتختفي حين يتوقّف لأن
              // شارةً دائمة تحجب أوّل فقاعة.
              PositionedDirectional(
                top: 8,
                start: 0,
                end: 0,
                child: IgnorePointer(
                  child: AnimatedOpacity(
                    opacity: _showFloatingDay && _floatingDay.isNotEmpty ? 1 : 0,
                    duration: const Duration(milliseconds: 180),
                    child: Center(child: _DayChip(label: _floatingDay)),
                  ),
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }

  // ── شارة التاريخ العائمة ────────────────────────────────────────────

  String _floatingDay = '';
  bool _showFloatingDay = false;
  Timer? _floatHide;

  /// يحسب اليوم المعروض من موضع السحب.
  ///
  /// ⚠ **تقديرٌ من الإزاحة لا قياسٌ لكل فقاعة.** قياس ارتفاع كل رسالة يحتاج
  /// مفتاحاً عامّاً لكلٍّ منها وبحثاً في كل بكسل سحب — ثمنٌ باهظ لشارة
  /// تعريفية. والتقدير يخطئ فقاعةً أو اثنتين في محادثةٍ فيها صورٌ متفاوتة
  /// الطول، وهو خطأٌ لا يضرّ: الشارة تقول اليوم، واليوم لا يتغيّر بين
  /// فقاعتين متجاورتين إلا عند الفاصل نفسه — وهناك الفاصل الثابت يصحّحها.
  bool _onScroll(ScrollNotification n) {
    if (_messages.isEmpty) return false;

    final max = n.metrics.maxScrollExtent;
    final ratio = max <= 0 ? 1.0 : (n.metrics.pixels / max).clamp(0.0, 1.0);
    final i = (ratio * (_messages.length - 1)).round();

    final d = DateTime.tryParse(_messages[i].createdAt);
    final label = d == null ? '' : _dayLabel(d);

    if (label != _floatingDay || !_showFloatingDay) {
      setState(() {
        _floatingDay = label;
        _showFloatingDay = true;
      });
    }

    // تختفي بعد سكونٍ قصير — لا عند `ScrollEndNotification` وحدها: السحب
    // بالقصور الذاتي يرسلها متأخّرة، والشارة تبقى معلّقة بعد أن يستقرّ كل
    // شيء.
    _floatHide?.cancel();
    _floatHide = Timer(const Duration(milliseconds: 900), () {
      if (mounted) setState(() => _showFloatingDay = false);
    });

    return false;
  }

  /// «اليوم» · «أمس» · التاريخ — مصدرٌ واحد للفاصل الثابت وللشارة العائمة.
  String _dayLabel(DateTime d) {
    final now = DateTime.now();
    final today = DateTime(now.year, now.month, now.day);
    final day = DateTime(d.year, d.month, d.day);
    final diff = today.difference(day).inDays;

    if (diff == 0) return 'اليوم';
    if (diff == 1) return 'أمس';
    return '${day.year}-${day.month.toString().padLeft(2, '0')}'
        '-${day.day.toString().padLeft(2, '0')}';
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
        canForward: !widget.asEmployee,
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
        await _pinSheet(m);
      case 'forward':
        await _forward(m);
      case 'delete':
        await _delete(m);
    }
  }

  /// اختيار مدّة التثبيت (أمر المالك، 6 سبتمبر 2026).
  Future<void> _pinSheet(ChatMessage m) async {
    if (m.pinned) {
      await _pin(m, 0);
      return;
    }

    final days = await showModalBottomSheet<int>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => const _PinDurationSheet(),
    );
    if (days != null && mounted) await _pin(m, days);
  }

  /// إعادة توجيه إلى محادثةٍ أخرى من محادثات الوكيل.
  ///
  /// ⚠ في وضع الموظّف لا وجهة أصلاً: له محادثةٌ واحدة، وإعادة التوجيه إليها
  /// من نفسها لا معنى لها. فالخيار لا يظهر له.
  Future<void> _forward(ChatMessage m) async {
    final threads = await _repo.threads();
    if (!mounted) return;

    final targets = threads.where((t) => t.id != widget.threadId).toList();
    if (targets.isEmpty) {
      _toast('لا توجد محادثة أخرى لإعادة التوجيه إليها.');
      return;
    }

    final to = await showModalBottomSheet<int>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _ForwardSheet(threads: targets),
    );
    if (to == null || !mounted) return;

    try {
      await _repo.forward(widget.threadId!, m.id, to);
      if (mounted) _toast('أُعيد التوجيه.');
    } catch (e) {
      if (mounted) _toast('تعذّرت إعادة التوجيه. $e');
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

  Future<void> _pin(ChatMessage m, int days) async {
    setState(() {
      _messages = [
        for (final x in _messages)
          if (x.id == m.id) x.copyWith(pinned: days > 0) else x,
      ];
    });
    try {
      if (widget.asEmployee) {
        await _repo.employeePinMessage(m.id, days);
      } else {
        await _repo.pinMessage(widget.threadId!, m.id, days);
      }
      // شريط المثبَّتة يأتي من الخادم — تُعاد قراءته بالنبضة التالية،
      // ونعجّلها هنا فيرى الوكيل أثر ما فعل.
      await _poll();
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
    required this.onChanged,
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
  final VoidCallback onChanged;
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
                          onChanged: (_) => onChanged(),
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
        // سقفٌ لارتفاع الورقة: 85% من الشاشة.
        //
        // بدونه كانت قائمة الخيارات على شاشةٍ قصيرة تتجاوز حدّها، فيرسم
        // Flutter شريطه المخطّط بالأسود والأصفر أسفلها — وهو تحذير تجاوز
        // تخطيط لا زخرفة (شكا منه المالك، 6 سبتمبر 2026).
        constraints: BoxConstraints(
          maxHeight: MediaQuery.sizeOf(context).height * .85,
        ),
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
              // المحتوى يتمرّر إن طال بدل أن يتجاوز. و`shrinkWrap` يُبقي
              // الورقة بارتفاع محتواها حين يكون قصيراً — فلا تمتدّ فارغة.
              Flexible(
                child: SingleChildScrollView(
                  physics: const ClampingScrollPhysics(),
                  child: child,
                ),
              ),
            ],
          ),
        ),
      );
}

/// مدّة التثبيت — ثلاث مدد لا حقلٌ حرّ (أمر المالك، 6 سبتمبر 2026).
class _PinDurationSheet extends StatelessWidget {
  const _PinDurationSheet();

  @override
  Widget build(BuildContext context) => _SheetShell(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Text('تثبيت في أعلى المحادثة',
                textAlign: TextAlign.center, style: T.kufi(16, FontWeight.w700)),
            const SizedBox(height: 6),
            Text('يُرفع التثبيت وحده بعد المدّة.',
                textAlign: TextAlign.center,
                style: T.plex(12, FontWeight.w400, color: R.inkA(.5))),
            const SizedBox(height: 14),
            _MenuItem(
              icon: Icons.today_outlined,
              label: 'يوم',
              onTap: () => Navigator.pop(context, 1),
            ),
            _MenuItem(
              icon: Icons.date_range_outlined,
              label: 'أسبوع',
              onTap: () => Navigator.pop(context, 7),
            ),
            _MenuItem(
              icon: Icons.calendar_month_outlined,
              label: 'شهر',
              onTap: () => Navigator.pop(context, 30),
            ),
          ],
        ),
      );
}

/// اختيار المحادثة التي تُعاد إليها الرسالة.
///
/// ⚠ القائمة من محادثات هذا الوكيل وحدها، **والخادم يفحصها ثانيةً**: هذه
/// الورقة راحةٌ للمستخدم لا حارس (البندان 36 و62).
class _ForwardSheet extends StatelessWidget {
  const _ForwardSheet({required this.threads});

  final List<ChatThread> threads;

  @override
  Widget build(BuildContext context) => _SheetShell(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Text('إعادة توجيه إلى',
                textAlign: TextAlign.center, style: T.kufi(16, FontWeight.w700)),
            const SizedBox(height: 12),
            ConstrainedBox(
              // سقفٌ للارتفاع: وكيلٌ له عشرون موظّفاً يملأ الشاشة كلّها.
              constraints: const BoxConstraints(maxHeight: 320),
              child: ListView(
                shrinkWrap: true,
                children: [
                  for (final t in threads)
                    _MenuItem(
                      icon: t.isAdmin
                          ? Icons.support_agent_rounded
                          : Icons.person_outline_rounded,
                      label: t.title,
                      onTap: () => Navigator.pop(context, t.id),
                    ),
                ],
              ),
            ),
          ],
        ),
      );
}

/// شريط الرسالة المثبَّتة أعلى المحادثة (البند 31).
class _PinnedBar extends StatelessWidget {
  const _PinnedBar({required this.pinned, required this.onUnpin});

  final ChatMessage pinned;
  final VoidCallback onUnpin;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(R.padScreen, 8, 6, 8),
        decoration: BoxDecoration(
          color: R.warnIcon.withValues(alpha: .10),
          border: Border(bottom: BorderSide(color: R.warnIcon.withValues(alpha: .28))),
        ),
        child: Row(
          children: [
            Icon(Icons.push_pin_rounded, size: 15, color: R.warnIcon),
            const SizedBox(width: 8),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisSize: MainAxisSize.min,
                children: [
                  Text('رسالة مثبَّتة',
                      style: T.plex(10.5, FontWeight.w700, color: R.inkA(.55))),
                  Text(
                    pinned.body.isNotEmpty
                        ? pinned.body
                        : (pinned.isImage ? '📷 صورة' : '🎤 رسالة صوتية'),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: T.kufi(12.5, FontWeight.w600),
                  ),
                ],
              ),
            ),
            IconButton(
              tooltip: 'إلغاء التثبيت',
              onPressed: onUnpin,
              icon: Icon(Icons.close_rounded, size: 18, color: R.inkA(.5)),
              constraints: const BoxConstraints(minWidth: 40, minHeight: 40),
            ),
          ],
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
    required this.canForward,
  });

  final bool canDelete;
  final bool canEdit;
  final bool starred;
  final bool pinned;
  final bool hasText;

  /// إعادة التوجيه للوكيل وحده: للموظّف محادثةٌ واحدة، فلا وجهة له.
  final bool canForward;

  /// ستّة رموز سريعة، وبقيّتها في لوحة الإيموجي.
  ///
  /// 🤲 لا 🙏 (أمر المالك، 6 سبتمبر 2026): الأخير يُرسَم كفّين ملتصقتين —
  /// إيماءةُ شكرٍ في ثقافاتٍ أخرى — بينما 🤲 كفّان مبسوطتان إلى أعلى، وهي
  /// هيئة الدعاء التي يقصدها الوكيل حين يكتب «يا رب» أو «الحمد لله».
  static const _quick = ['👍', '❤️', '😂', '😮', '😢', '🤲'];

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
            if (canForward)
              _MenuItem(
                icon: Icons.forward_rounded,
                label: 'إعادة توجيه',
                onTap: () => Navigator.pop(context, 'forward'),
              ),
            _MenuItem(
              icon: pinned
                  ? Icons.push_pin_rounded
                  : Icons.push_pin_outlined,
              label: pinned ? 'إلغاء التثبيت' : 'تثبيت في أعلى المحادثة',
              onTap: () => Navigator.pop(context, 'pin'),
            ),
            // «حذف» أُزيل من القائمة بأمر المالك (6 سبتمبر 2026): لم يطلبه،
            // وأضفتُه أنا اجتهاداً.
            //
            // ⚠ ونقطة الحذف في الخادم **باقية** ولم تُحذف: هي محروسة
            // (لصاحب الرسالة وحده، وحذفٌ ناعم لا يمحو الصفّ)، وحذفُ شيفرةٍ
            // مجرَّبة من أجل إخفاء زرٍّ يجعل إعادته لاحقاً عملاً من جديد.
            // إعادةُ السطر هنا وحدها تُرجعه.
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
  });

  final IconData icon;
  final String label;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    // كان هنا `danger` للتلوين بالأحمر، ولم يبقَ في القائمة بندٌ خطر بعد
    // إزالة «حذف» — ومعاملٌ لا يستعمله أحد يوهم بخيارٍ غير موجود.
    final c = R.primaryDark;

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
