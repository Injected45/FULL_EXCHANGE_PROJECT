import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../transfers/agent_incoming_repository.dart';
import 'employee_session.dart';

/// الحوالات الواردة — بعين الموظف.
///
/// نفس دفتر الوكيل ونفس التبويبات، والفرق أنّ التسليم هنا **يُنسب** إلى
/// الموظف ونقطة بيعه وجهازه، ويُسجَّل حركةَ خزينةٍ في وردية إن كانت مفتوحة.
///
/// وزرّ التسليم لا يظهر إلا لمن مُنح `DELIVER_TRANSFER` — والخادم يرفضه
/// بـ 403 لغيره، فالإخفاء تجميل والرفض حماية.
class EmployeeTransfersScreen extends ConsumerStatefulWidget {
  const EmployeeTransfersScreen({super.key});

  @override
  ConsumerState<EmployeeTransfersScreen> createState() =>
      _EmployeeTransfersScreenState();
}

class _EmployeeTransfersScreenState
    extends ConsumerState<EmployeeTransfersScreen>
    with WidgetsBindingObserver {
  IncomingTab _tab = IncomingTab.pending;

  List<AgentIncomingTransfer> _rows = const [];
  Map<String, int> _counts = const {};
  bool _loading = true;
  String? _error;

  /*
   * ══════════════════════════════════════════════════════════════════════
   *  ⚠ التحديثُ الدوريّ ليس رفاهية
   * ══════════════════════════════════════════════════════════════════════
   *
   * الحوالةُ كيانٌ **واحد** مشترك بين الوكيل وكلّ موظفيه، لا نسخةٌ لكلٍّ.
   * فحين يسلّمها زميلٌ يجب أن تغادر شاشةَ الباقين — وبلا نبضةٍ تبقى معروضةً
   * إلى أن يسحب أحدُهم يدَه على الشاشة، وقد يضغط «تسجيل التسليم» قبل ذلك.
   *
   * والخادمُ يرفض الثانيَ قطعاً (التحوُّلُ الذرّيّ)، فلا يقع ازدواجٌ ماليّ.
   * لكن **الموظف يقف أمام مستفيدٍ ينتظر**: صفٌّ يُعرض ثم يُرفض عند لمسه
   * يُعلّمه أن التطبيق معطوب، وقد يدفع من درجه ثقةً بما رآه.
   *
   * ── ثلاثةُ قراراتٍ في النبضة ─────────────────────────────────────────
   *
   * • **ثلاثون ثانية** — نبضةُ جرس الوكيل نفسُها. أقصرُ منها لا يشتري شيئاً
   *   في فرعٍ يسلّم حوالاتٍ بالدقائق، وأطولُ يُبقي الصفَّ الميت معروضاً.
   *
   * • **وتتوقّف في الخلفية** (`didChangeAppLifecycleState`): نبضةٌ لتطبيقٍ
   *   لا يُنظَر إليه شبكةٌ تُستهلك بلا قارئ.
   *
   * • **ولا تُظهر مؤشّرَ تحميل** (`silent`): وميضُ هيكلٍ كلَّ نصف دقيقة
   *   تحت يد الموظف أسوأ من صفٍّ قديم بثوانٍ.
   */
  static const _pulse = Duration(seconds: 30);
  Timer? _timer;

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addObserver(this);
    _load();
    _timer = Timer.periodic(_pulse, (_) => _load(silent: true));
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    if (state == AppLifecycleState.resumed) {
      // ⚠ وقراءةٌ فوريةٌ عند العودة لا انتظارُ نبضةٍ كاملة: الرجوعُ إلى
      // التطبيق هو اللحظةُ التي يُنظر فيها إلى القائمة.
      _timer ??= Timer.periodic(_pulse, (_) => _load(silent: true));
      _load(silent: true);
    } else if (state == AppLifecycleState.paused) {
      _timer?.cancel();
      _timer = null;
    }
  }

  @override
  void dispose() {
    _timer?.cancel();
    WidgetsBinding.instance.removeObserver(this);
    super.dispose();
  }

  bool _polling = false;

  Future<void> _load({bool silent = false}) async {
    if (!mounted) return;
    // حارسُ «طلبٌ جارٍ»: نبضةٌ صامتةٌ فوق أخرى تجعل ردّاً قديماً يدهس أحدث.
    if (silent && _polling) return;
    _polling = true;
    if (!silent) setState(() { _loading = true; _error = null; });
    try {
      final env = await ref.read(apiClientProvider).get(
        '/device/employee/transfers/incoming',
        query: {'status': _tab.wire, 'per_page': 50},
      );
      final data = env.row ?? const {};
      // ⚠ الخادم يُعيد الصفوف تحت المفتاح `items` (كما يقرؤها تطبيق الوكيل).
      // كان الموظف يقرأ `rows`/`data` فقط، فيظهر العدّاد ولا تظهر الحوالات.
      final list = ((data['items'] ?? data['rows'] ?? data['data'] ?? []) as List)
          .whereType<Map>()
          .map((m) => AgentIncomingTransfer.fromJson(m.cast<String, dynamic>()))
          .toList();

      final counts = (data['counts'] as Map?)?.cast<String, dynamic>() ?? const {};

      if (!mounted) return;
      setState(() {
        _rows = list;
        _counts = counts.map((k, v) => MapEntry(k, int.tryParse('$v') ?? 0));
        _loading = false;
      });
    } on ApiFailure catch (e) {
      // ⚠ نبضةٌ أخفقت لا تمحو قائمةً صالحة: الشبكةُ تتقطّع في الفروع،
      // وشاشةُ خطأٍ مكانَ قائمةٍ كانت معروضة تُوقف العمل بلا سبب.
      if (mounted && !silent) {
        setState(() { _error = e.message; _loading = false; });
      }
    } catch (_) {
      if (mounted && !silent) {
        setState(() { _error = 'تعذّر الاتصال بالخادم.'; _loading = false; });
      }
    } finally {
      _polling = false;
    }
  }

  /*
   * ⚠ **«بانتظار التسليم» خلف صلاحية التسليم** — أمر المالك، 9 سبتمبر 2026.
   *
   * هي **قائمةُ عمل** لا عرضاً: من لا يسلّم لا شأن له بها، وعرضُها عليه
   * يجعله يقول للمستفيد «حوالتُك عندي» ثم لا يستطيع تسليمها.
   *
   * وتبقى «تم التسليم» و«الملغاة» لمن مُنح `VIEW_INCOMING_TRANSFERS` — تلك
   * استعلامٌ لا عمل، وهي ما مُنح الصلاحيةَ من أجله.
   *
   * ⚠ والإخفاءُ تجميلٌ لا حماية: الخادم يردّ 403 على طلب المعلَّق ممّن لا
   * يملكها، ويجعل الافتراضيَّ «تم التسليم» — فحذفُ المعامل لا يتجاوزه.
   */
  List<IncomingTab> _tabsFor(bool canDeliver) => canDeliver
      ? IncomingTab.values
      : const [IncomingTab.delivered, IncomingTab.cancelled];

  @override
  Widget build(BuildContext context) {
    final canDeliver =
        ref.watch(employeeAuthProvider).profile?.can('DELIVER_TRANSFER') ?? false;

    /*
     * ⚠ تصحيحٌ ذاتيّ: الصلاحيةُ تُقرأ عند كل تحديث، فسحبُها والموظفُ واقفٌ
     * على «بانتظار التسليم» يترك تبويباً محدَّداً لا يظهر في الشريط —
     * فتبدو الشاشةُ فارغةً بلا سبب. يُنقَل إلى أول ما هو متاح.
     */
    if (!canDeliver && _tab == IncomingTab.pending) {
      WidgetsBinding.instance.addPostFrameCallback((_) {
        if (!mounted) return;
        setState(() => _tab = IncomingTab.delivered);
        _load();
      });
    }

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'الحوالات الواردة', onBack: () => context.pop()),
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 6, R.padScreen, 10),
            child: _Tabs(
              current: _tab,
              counts: _counts,
              tabs: _tabsFor(canDeliver),
              onPick: (t) {
                setState(() => _tab = t);
                _load();
              },
            ),
          ),
          Expanded(
            child: _loading
                ? Center(child: CircularProgressIndicator(color: R.primary))
                : _error != null
                    ? _Failed(message: _error!, onRetry: _load)
                    : RefreshIndicator(
                        onRefresh: _load,
                        color: R.primary,
                        backgroundColor: Colors.white,
                        child: _rows.isEmpty
                            ? ListView(
                                physics: const AlwaysScrollableScrollPhysics(),
                                children: [
                                  const SizedBox(height: 70),
                                  _Empty(tab: _tab),
                                ],
                              )
                            : ListView(
                                padding: const EdgeInsets.fromLTRB(
                                    R.padScreen, 4, R.padScreen, 40),
                                physics: const AlwaysScrollableScrollPhysics(),
                                children: [
                                  for (var i = 0; i < _rows.length; i++) ...[
                                    if (i > 0) const SizedBox(height: R.gapRow),
                                    _TransferCard(
                                      t: _rows[i],
                                      canDeliver: canDeliver,
                                      onDelivered: _load,
                                    ),
                                  ],
                                ],
                              ),
                      ),
          ),
        ],
      ),
    );
  }
}

class _Tabs extends StatelessWidget {
  const _Tabs({
    required this.current,
    required this.counts,
    required this.tabs,
    required this.onPick,
  });

  final IncomingTab current;
  final Map<String, int> counts;

  /// ما يُعرض منها — ليست دائماً الثلاثةَ كلَّها. انظر الشاشة.
  final List<IncomingTab> tabs;

  final ValueChanged<IncomingTab> onPick;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          for (final t in tabs) ...[
            if (t != tabs.first) const SizedBox(width: 8),
            // ⚠ العرضُ بحسب الكلمة — الشرحُ عند نظيره في تبويبات الوكيل.
            Expanded(
              flex: _weight(t, counts),
              child: GestureDetector(
                onTap: () => onPick(t),
                child: AnimatedContainer(
                  duration: const Duration(milliseconds: 180),
                  padding: const EdgeInsets.symmetric(vertical: 11),
                  decoration: BoxDecoration(
                    gradient: t == current ? R.primaryGradient : null,
                    color: t == current ? null : R.whiteA(.66),
                    border: Border.all(
                        color: t == current ? Colors.transparent : R.inkA(.08)),
                    borderRadius: BorderRadius.circular(R.rPill),
                  ),
                  child: Text(
                    '${_label(t)}${counts[t.wire] == null ? '' : ' (${counts[t.wire]})'}',
                    textAlign: TextAlign.center,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: T.plex(11.5, FontWeight.w600,
                        color: t == current ? Colors.white : R.inkA(.6)),
                  ),
                ),
              ),
            ),
          ],
        ],
      );

  /// وزنُ التبويب = طولُ ما يُعرض فيه، بحدٍّ أدنى يسع عدّادَه.
  int _weight(IncomingTab t, Map<String, int> counts) {
    final c = counts[t.wire];
    final shown = _label(t).length + (c == null ? 0 : ' ($c)'.length);
    return shown < 9 ? 9 : shown;
  }

  String _label(IncomingTab t) => switch (t) {
        IncomingTab.pending => 'غير مسلَّمة',
        IncomingTab.delivered => 'تم التسليم',
        IncomingTab.cancelled => 'الملغاة',
      };
}

class _TransferCard extends ConsumerStatefulWidget {
  const _TransferCard({
    required this.t,
    required this.canDeliver,
    required this.onDelivered,
  });

  final AgentIncomingTransfer t;
  final bool canDeliver;
  final Future<void> Function() onDelivered;

  @override
  ConsumerState<_TransferCard> createState() => _TransferCardState();
}

class _TransferCardState extends ConsumerState<_TransferCard> {
  bool _busy = false;

  @override
  Widget build(BuildContext context) {
    final t = widget.t;
    final tone = t.isCancelled
        ? R.error
        : (t.isDelivered ? R.inkA(.5) : R.primaryDark);

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            children: [
              IconTile(
                size: 38,
                background: tone.withValues(alpha: .12),
                icon: Icon(Icons.call_received_rounded, size: 19, color: tone),
              ),
              const SizedBox(width: 12),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(t.receiverName.isEmpty ? 'مستفيد' : t.receiverName,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: T.kufi(14.5, FontWeight.w700)),
                    const SizedBox(height: 3),
                    Directionality(
                      textDirection: TextDirection.ltr,
                      child: Text(t.code,
                          style: T.plex(11.5, FontWeight.w500,
                              color: R.inkA(.55))),
                    ),
                  ],
                ),
              ),
              Directionality(
                textDirection: TextDirection.ltr,
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Text('د.ل ',
                        style: T.plex(11, FontWeight.w500, color: R.inkA(.5))),
                    Text(Fmt.money(t.amount),
                        style: T.kufi(15, FontWeight.w800, color: tone)),
                  ],
                ),
              ),
            ],
          ),
          if (widget.canDeliver && !t.isDelivered && !t.isCancelled) ...[
            const SizedBox(height: 12),
            PrimaryButton(
              label: 'تسجيل التسليم',
              loading: _busy,
              onPressed: _busy ? null : _deliver,
            ),
          ],
          if (t.isCancelled) ...[
            const SizedBox(height: 10),
            const WarnBanner(
              text: 'هذه الحوالة ملغاة في المنظومة — لا تسلّم مالها.',
            ),
          ],
        ],
      ),
    );
  }

  Future<void> _deliver() async {
    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _ConfirmDeliver(name: widget.t.receiverName),
    );
    if (ok != true || !mounted) return;

    setState(() => _busy = true);
    try {
      final env = await ref.read(apiClientProvider)
          .post('/device/employee/transfers/${widget.t.id}/deliver');

      /*
       * ⚠ **سبقني زميلي** — حالةٌ ناجحةٌ في الشبكة ومرفوضةٌ في المعنى.
       *
       * الخادم يردّ 200 لأن المطلوبَ محقَّق: الحوالة مسلَّمة. ولو ردّ
       * خطأً لبقيت القائمةُ القديمة معروضةً — حمولةُ الأخطاء لا تُقرأ
       * هنا — فيبقى الزرُّ حيّاً لحوالةٍ دُفع مالُها.
       *
       * فالعَلَمُ يُقرأ صريحاً لا من نصّ الرسالة: نصٌّ يُعاد صياغتُه في
       * الخادم غداً يكسر شرطاً مكتوباً عليه.
       */
      final taken = (env.row ?? const {})['already_delivered_by_other'] == true;

      // التحديثُ أولاً ثم الرسالة: الموظف يجب أن يرى الصفَّ قد تغيّر
      // وهو يقرأ سببَ ذلك.
      await widget.onDelivered();
      if (taken && mounted) {
        _say(env.displayMessage(
            'تم تسليم هذه الحوالة مسبقاً ولا يمكن تنفيذ العملية مرة أخرى.'));
      }
    } on ApiFailure catch (e) {
      if (mounted) _say(e.message);
    } catch (_) {
      if (mounted) _say('تعذّر تسجيل التسليم — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  void _say(String m) {
    ScaffoldMessenger.of(context)
      ..hideCurrentSnackBar()
      ..showSnackBar(SnackBar(
        content:
            Text(m, style: T.plex(13, FontWeight.w500, color: Colors.white)),
        backgroundColor: R.inkA(.92),
        behavior: SnackBarBehavior.floating,
      ));
  }
}

class _ConfirmDeliver extends StatelessWidget {
  const _ConfirmDeliver({required this.name});

  final String name;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(22, 22, 22, 26),
        decoration: BoxDecoration(
          color: R.whiteA(.94),
          borderRadius:
              const BorderRadius.vertical(top: Radius.circular(R.rNav)),
        ),
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
            const SizedBox(height: 20),
            Center(
                child: Text('تسجيل التسليم',
                    style: T.kufi(17, FontWeight.w700))),
            const SizedBox(height: 10),
            Text('تأكّد من هوية المستلم قبل التسجيل:\n$name',
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w500,
                    color: R.inkA(.65), height: 1.7)),
            const SizedBox(height: 16),
            const WarnBanner(
              text: 'تأكّد من تسليم الحوالة للمستفيد قبل تسجيل التسليم.',
            ),
            const SizedBox(height: 20),
            PrimaryButton(
              label: 'تسجيل التسليم',
              onPressed: () => Navigator.of(context).pop(true),
            ),
            const SizedBox(height: 10),
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
              child: Text('إلغاء',
                  style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
            ),
          ],
        ),
      );
}

class _Empty extends StatelessWidget {
  const _Empty({required this.tab});

  final IncomingTab tab;

  @override
  Widget build(BuildContext context) => Column(
        children: [
          Icon(Icons.inbox_rounded, size: 42, color: R.primaryA(.3)),
          const SizedBox(height: 16),
          Text(
            switch (tab) {
              IncomingTab.pending => 'لا توجد حوالات غير مسلَّمة',
              IncomingTab.delivered => 'لم تُسلَّم حوالات بعد',
              IncomingTab.cancelled => 'لا توجد حوالات ملغاة',
            },
            style: T.kufi(15, FontWeight.w600, color: R.inkA(.6)),
          ),
        ],
      );
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.all(R.padScreen),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(Icons.error_outline_rounded, size: 38, color: R.error),
            const SizedBox(height: 14),
            Text(message,
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w500,
                    color: R.inkA(.65), height: 1.7)),
            const SizedBox(height: 18),
            GlassButton(label: 'إعادة المحاولة', onPressed: onRetry),
          ],
        ),
      );
}
