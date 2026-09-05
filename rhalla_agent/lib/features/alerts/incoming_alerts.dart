import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import '../../core/storage/secure_store.dart';

/// جرس الوارد: كم حوالةً **بانتظار التسليم** وصلت ولم يرَها الوكيل بعد.
///
/// «بانتظار التسليم» وحدها (أمر المالك، 5 سبتمبر 2026): الجرس ينبّه إلى عملٍ
/// لم يُنجَز. المسلَّمة انتهت والملغاة لا تُسلَّم، وعدُّهما يجعل الرقم لا
/// يهبط أبداً فيفقد معناه.
///
/// و«رآها» تعني أنه فتح شاشة الحوالات — فينزل العدّاد إلى صفر، ثم يعود 1 مع
/// أول واردة جديدة، و2 إن جاءت ثانية قبل أن يفتحها، وهكذا.
@immutable
class IncomingAlerts {
  const IncomingAlerts({this.unseen = 0});

  final int unseen;

  bool get any => unseen > 0;
}

/// يسأل الخادم دورياً، ويحفظ ما فُتح على الجهاز.
///
/// **ما فُتح محلّي عمداً**: السؤال «هل رأيتُ هذه؟» يخصّ من يمسك الهاتف لا
/// الحساب. ونقلُه إلى الخادم يعني جدولاً جديداً وكتابةً عند كل فتح فاتورة،
/// وثمنَ ذلك بلا مقابل — الجرس لا يقرّر شيئاً ماليّاً.
class IncomingAlertsController extends StateNotifier<IncomingAlerts> {
  IncomingAlertsController(this._api, this._store)
      : super(const IncomingAlerts());

  final ApiClient _api;
  final SecureStore _store;

  /// ما أقرّ الوكيل برؤيته — محفوظ بين الجلسات.
  Set<int> _seen = <int>{};

  /// ما رنّ له الجرس في **هذه الجلسة**.
  ///
  /// منفصلة عن [_seen] لأنها تجيب سؤالاً آخر: [_seen] «هل نظر إليها؟»
  /// وهذه «هل نبّهناه إليها؟». وبلا الثانية يرنّ الجرس عند كل نبضة ما دامت
  /// حوالةٌ غير مفتوحة — أي كل ثلاثين ثانية إلى أن يفتحها.
  Set<int> _announced = <int>{};

  /// آخر ما أعاده الخادم — مرجعُ «أقرّ برؤية الكل».
  Set<int> _pending = <int>{};

  bool _loaded = false;
  bool _busy = false;

  Timer? _timer;

  /// كل نصف دقيقة.
  ///
  /// النبضة تُشغّل مزامنةً في الخادم (`syncFromCore`)، وهي قياساً 0.36 ثانية —
  /// رخيصة لا مجّانية. وأقصر من ذلك لا يشتري شيئاً: حوالةٌ تُرى بعد نصف
  /// دقيقة من وصولها تُرى في وقتها.
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

  /// يُنسى ما رنّ له، ولا يُنسى ما أُقرّ برؤيته.
  ///
  /// الوكيل الذي يعود بعد خروج هو الوكيل نفسه: إعادةُ الرنين لحوالاتٍ نظر
  /// إليها إنذارٌ كاذب، وهو أسوأ من لا إنذار لأنه يُعلِّم تجاهل الجرس.
  void reset() {
    stop();
    _announced = <int>{};
    _pending = <int>{};
    state = const IncomingAlerts();
  }

  Future<void> refresh() async {
    if (_busy) return;
    _busy = true;

    try {
      if (!_loaded) {
        _seen = await _store.readSeenIncoming();
        _loaded = true;
      }

      final env = await _api.get('/agent/incoming-transfers/alerts');
      final ids = _idsOf(env);
      _pending = ids;

      // الحوالة التي سُلِّمت أو أُلغيت غادرت القائمة، فلا تُحسب ولا يُحتفظ
      // بها في أيّ من المجموعتين — وإلا كبرتا بلا حدّ.
      _seen = _seen.intersection(ids);
      _announced = _announced.intersection(ids);

      final unseen = ids.difference(_seen);

      // الرنين لما لم يُنبَّه إليه بعد — لا لكل ما لم يُفتح.
      //
      // الفرق هو الفرق بين تنبيهٍ يُسمع مرّة عند الوصول، وجرسٍ يرنّ كل
      // ثلاثين ثانية إلى أن يفتح الوكيل الحوالة. والثاني يُسكَت بإسكات
      // الهاتف كلّه، فيضيع التنبيه الحقيقي معه.
      if (unseen.difference(_announced).isNotEmpty) _ring();
      _announced.addAll(unseen);

      if (unseen.length != state.unseen) {
        state = IncomingAlerts(unseen: unseen.length);
      }
    } on ApiFailure {
      // الجرس لا يعرض أخطاء. انقطاعُ الشبكة يُبقيه على آخر ما يعرف، ورسالةُ
      // عطبٍ في أعلى الشاشة الرئيسية تُخيف بلا أن تفيد.
    } catch (_) {
      // كما فوقها.
    } finally {
      _busy = false;
    }
  }

  /// فتح الوكيل شاشة الحوالات ⇒ العدّاد إلى صفر (أمر المالك، 5 سبتمبر 2026).
  ///
  /// الشاشة كلّها لا فاتورةً فاتورة: القائمة تعرض الوارد بأرقامه وأسمائه
  /// ومبالغه، ومن فتحها فقد رأى ما وصله. والعدّاد بعدها يَعِد بشيء واحد
  /// واضح — **كم وصل منذ آخر مرّة نظرتُ فيها**.
  ///
  /// ولا يُلغى شيء في الخادم: الحوالة تبقى «بانتظار التسليم» حتى يسجّل
  /// الوكيل تسليمها. هذا عدّاد نظر، لا عدّاد عمل.
  Future<void> markAllSeen() async {
    if (!_loaded) {
      _seen = await _store.readSeenIncoming();
      _loaded = true;
    }
    if (_pending.difference(_seen).isEmpty) return;

    _seen = {..._seen, ..._pending};
    await _store.writeSeenIncoming(_seen);

    if (state.unseen != 0) state = const IncomingAlerts();
  }

  Set<int> _idsOf(Envelope env) {
    final raw = env.row?['ids'];
    if (raw is! List) return <int>{};
    return raw
        .map((e) => int.tryParse('$e') ?? -1)
        .where((e) => e > 0)
        .toSet();
  }

  /// رنّة النظام واهتزازة.
  ///
  /// القناة الأصلية أولاً لأنها تعطي نغمة الإشعار التي عوّد النظامُ صاحبَه
  /// عليها. و[SystemSound] بديلها حيث لا قناة (iOS هنا)، والاهتزاز يعمل في
  /// الحالتين — والوكيل قد يكون في فرعٍ صاخب أو هاتفُه صامت.
  ///
  /// ولا حزمة صوت: نغمة مضمّنة تعني ملفاً في الـ APK ومكتبةً تشغّله، مقابل
  /// صوتٍ أغرب على المستخدم من نغمة جهازه.
  Future<void> _ring() async {
    HapticFeedback.mediumImpact();
    try {
      await _sound.invokeMethod<void>('notificationSound');
    } catch (_) {
      await SystemSound.play(SystemSoundType.alert);
    }
  }

  static const _sound = MethodChannel('com.rhalla.rhalla_agent/device');

  @override
  void dispose() {
    _timer?.cancel();
    super.dispose();
  }
}

final incomingAlertsProvider =
    StateNotifierProvider<IncomingAlertsController, IncomingAlerts>(
  (ref) => IncomingAlertsController(
    ref.watch(apiClientProvider),
    ref.watch(secureStoreProvider),
  ),
);
