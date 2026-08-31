import 'dart:convert';

import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:shared_preferences/shared_preferences.dart';

/// سِجل التسليم — **محلّي بالكامل. لا خادم، ولا قاعدة بيانات، ولا حسابات.**
///
/// شرط المالك الحازم (31 أغسطس 2026): «لا تغيير في قواعد البيانات ولا في
/// العمليات المحاسبية المتّبعة».
///
/// وهو شرطٌ صحيح لا مجرّد رغبة: الحوالة تصل الوكيل **مسلَّمةً ومسجَّلة
/// عليه** في المنظومة الرئيسية، والمحاسبة بينه وبين الشركة مُغلقة قبل أن
/// يفتح التطبيق أصلاً. فما هنا ليس إجراءً مالياً بل **دفترٌ خاصّ به وحده**
/// ينظّم به ما ناوله للعميل وما لم يناوله، لا يراه غيره.
///
/// ولهذا لا يُستدعى `InternalEx_costimer` في هذا المسار إطلاقاً.
class DeliveryState {
  const DeliveryState({
    this.baseline = const <String>{},
    this.delivered = const <String, DateTime>{},
    this.ready = false,
  });

  /// أكواد كانت موجودة لحظة أول تشغيل — تُخفى إلى الأبد.
  ///
  /// **هذه ليست تحسيناً بل حاجزُ أمان مالي.** بعد إعادة تثبيت أو مسح
  /// بيانات تضيع العلامات، فتعود حوالاتٌ سُلّمت فعلاً لتظهر «بانتظار
  /// التسليم»؛ فيظنّ الوكيل أنه لم يُسلّمها **فيُسلّمها مرّة ثانية**.
  /// الشاشة الفارغة تقول «لا أعلم عن هذه» وهو صدق؛ أما إظهارها فيقول
  /// «لم تُسلَّم» وهو كذبٌ يُكلّف مالاً. قرار المالك.
  final Set<String> baseline;

  /// الرمز ⇦ لحظة تعليمه مُسلَّماً.
  final Map<String, DateTime> delivered;

  /// لا نعرض شيئاً قبل قراءة التخزين، وإلا ومضت القائمة كاملةً ثم انكمشت.
  final bool ready;

  bool isHidden(String code) => baseline.contains(code);
  bool isDelivered(String code) => delivered.containsKey(code);
  DateTime? deliveredAt(String code) => delivered[code];

  DeliveryState copyWith({
    Set<String>? baseline,
    Map<String, DateTime>? delivered,
    bool? ready,
  }) =>
      DeliveryState(
        baseline: baseline ?? this.baseline,
        delivered: delivered ?? this.delivered,
        ready: ready ?? this.ready,
      );
}

class DeliveryLog extends StateNotifier<DeliveryState> {
  DeliveryLog() : super(const DeliveryState()) {
    _load();
  }

  static const _kBaseline = 'delivery.baseline.codes';
  static const _kBaselineAt = 'delivery.baseline.at';
  static const _kDelivered = 'delivery.delivered';

  Future<SharedPreferences> get _prefs => SharedPreferences.getInstance();

  Future<void> _load() async {
    final p = await _prefs;
    final baseline = p.getStringList(_kBaseline);
    final raw = p.getString(_kDelivered);

    final delivered = <String, DateTime>{};
    if (raw != null && raw.isNotEmpty) {
      try {
        (jsonDecode(raw) as Map<String, dynamic>).forEach((k, v) {
          final t = DateTime.tryParse('$v');
          if (t != null) delivered[k] = t;
        });
      } catch (_) {
        // تخزين تالف: نبدأ نظيفاً بدل أن نُسقط الشاشة. لا خسارة —
        // هذه بيانات توضيحية لا حسابية.
      }
    }

    state = DeliveryState(
      baseline: baseline?.toSet() ?? const <String>{},
      delivered: delivered,
      // خطّ الأساس غير مكتوب بعد ⇦ لم يُلتقط، ننتظر أول قائمة من الخادم.
      ready: baseline != null,
    );
  }

  /// يُلتقط خطّ الأساس مرّة واحدة في عمر التثبيت.
  ///
  /// يُستدعى بأكواد أول قائمة تصل من الخادم. وإن كان مكتوباً سلفاً فلا
  /// يُمسّ — وإلا ابتلع كل حوالة جديدة تصل بعده.
  Future<void> captureBaseline(Iterable<String> serverCodes) async {
    if (state.ready) return;
    final codes = serverCodes.where((c) => c.isNotEmpty).toSet();
    final p = await _prefs;
    await p.setStringList(_kBaseline, codes.toList());
    await p.setString(_kBaselineAt, DateTime.now().toIso8601String());
    if (!mounted) return;
    state = state.copyWith(baseline: codes, ready: true);
  }

  Future<void> markDelivered(String code) async {
    if (code.isEmpty) return;
    final next = Map<String, DateTime>.from(state.delivered)
      ..[code] = DateTime.now();
    await _saveDelivered(next);
  }

  // ⛔ **لا تُضِف تراجعاً عن التسليم.**
  //
  // كانت هنا undo() فحُذفت بأمر المالك: «بعد أن تتحول إلى سلَّمتُها لا
  // ترجع إلى بانتظار التسليم أبداً مهما حصل — منع نهائي».
  //
  // والسبب هو نفسه الذي بُني عليه خطّ الأساس: حوالةٌ تعود إلى قائمة
  // الانتظار تقول للوكيل «لم تُسلَّم» فيُسلّمها مرّة ثانية. والمسار
  // الوحيد المسموح لتفريغ السجل هو [resetAll] — وهو **يُخفي** ولا يُعيد
  // إلى الانتظار.

  /// تفريغ السجل والبدء من جديد — **بخطّ أساس جديد لا بإعادة الكل إلى
  /// الانتظار.**
  ///
  /// قرار المالك: «مسح البيانات ينطبق عليه شروط تثبيت تطبيق جديد».
  /// فالمسح يعني دفتراً نظيفاً: تُخفى الحوالات القائمة، ويعمل السجل من
  /// أول حوالة تصل بعده. ولو أعدنا المُسجَّل إلى «بانتظار التسليم» لظنّ
  /// الوكيل أنه لم يُسلّمه فسلَّمه مرّة ثانية.
  Future<void> resetAll() async {
    final p = await _prefs;
    await p.remove(_kBaseline);
    await p.remove(_kBaselineAt);
    await p.remove(_kDelivered);
    if (!mounted) return;
    // ready=false ⇦ الشاشة تلتقط خطّ أساس جديداً من أول قائمة تصل.
    state = const DeliveryState();
  }

  Future<void> _saveDelivered(Map<String, DateTime> next) async {
    final p = await _prefs;
    await p.setString(
      _kDelivered,
      jsonEncode(next.map((k, v) => MapEntry(k, v.toIso8601String()))),
    );
    if (!mounted) return;
    state = state.copyWith(delivered: next);
  }
}

final deliveryLogProvider =
    StateNotifierProvider<DeliveryLog, DeliveryState>((ref) => DeliveryLog());
