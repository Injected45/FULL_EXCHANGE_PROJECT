import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/keyboard.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'agent_incoming_repository.dart';
import 'delivery_receipt_screen.dart';
import 'transfers_repository.dart';
import '../auth/auth_controller.dart';
import '../home/home_repository.dart';
import '../home/home_screen.dart';

/// تبويب الحوالات — إدخال رمز للتسليم، وقائمة ما ينتظر التسليم في الفرع.
class TransfersScreen extends ConsumerStatefulWidget {
  const TransfersScreen({super.key});

  @override
  ConsumerState<TransfersScreen> createState() => _TransfersScreenState();
}

class _TransfersScreenState extends ConsumerState<TransfersScreen> {
  final _search = TextEditingController();
  String _query = '';

  /// عدد ما تعرضه الصفحة الواحدة. الترقيم في الخادم لا في الهاتف.
  static const _pageSize = 20;
  int _shown = _pageSize;

  IncomingTab _tab = IncomingTab.pending;

  /// القسم الأعلى: واردة (افتراضياً) أو صادرة.
  bool _outgoing = false;

  /// آخر صفحة وصلت — تُبقي الأعداد ثابتة أثناء التحميل بدل أن تومض صفراً.
  IncomingPage? _lastPage;

  /// الحالة كلّها من قاعدة الخادم — لا دفتر على الجهاز.
  ///
  /// كان الفرز يقوم على `deliveryLogProvider` في التخزين المحلّي: يضيع
  /// بحذف التطبيق أو تغيير الهاتف، ولا يراه الوكيل إن دخل من جهاز آخر.
  /// و«خطّ الأساس» فيه كان يُخفي كل ما وصل قبل أول تشغيل — وهو ما أخفى
  /// حوالةً معتمدة فعلاً. الملف باقٍ ولا يُستعمل هنا.
  AutoDisposeFutureProvider<IncomingPage> get _provider =>
      agentIncomingProvider(IncomingQuery(_tab, _query));

  @override
  void dispose() {
    _searchFocus.dispose();
    _search.dispose();
    super.dispose();
  }

  // حقل رقمي: يُفرَغ عند دخول المؤشّر. ونُزامن _query معه وإلا بقيت
  // القائمة مُرشَّحة بكودٍ لم يعد ظاهراً في الصندوق.
  late final _searchFocus = AutoClearFocus(_search, onChanged: () {
    setState(() {
      _query = _search.text;
      _shown = _pageSize;
    });
  });

  /// فاتورة الحوالة — ومنها يُسجَّل التسليم.
  void _openReceipt(AgentIncomingTransfer t) => Navigator.of(context, rootNavigator: true)
      .push(MaterialPageRoute(builder: (_) => DeliveryReceiptScreen(transfer: t)));

  // مسح السجل المحلّي زال مع الدفتر المحلّي — راجع _provider.

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(_provider);

    // الأعداد تأتي مع كل صفحة من الخادم — هو من يعرف ما في التبويبات
    // الأخرى، والعدّ في الهاتف كان يكذب مع أول ترقيم صفحات.
    // ويُحتفظ بآخر قيمة أثناء التحميل حتى لا تومض الأعداد صفراً.
    final page = async.valueOrNull ?? _lastPage ?? IncomingPage.empty;
    if (async.hasValue) _lastPage = async.value;

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'الحوالات',
            // زرّ رجوع لأنها لم تعد تبويباً في الشريط (قرار المالك، 3 سبتمبر
            // 2026): تُدفع من زرّ «الحوالات» في الرئيسية، وشاشةٌ مدفوعة بلا
            // رجوع تحبس الوكيل فيها.
            onBack: () => context.pop(),
            // بلا سطر فرعي — قرار المالك (2 سبتمبر 2026): لا اسم الوكيل ولا
            // «التسليم والمتابعة». اسم الفرع يبقى ظاهراً في تبويب الحساب.
            // لا زرّ «مسح السجل»: السجل صار في قاعدة الخادم، ومسحُه من
            // الهاتف كان يمحو دليل من سُلِّم.
          ),

          // واردة | صادرة — القسمة العليا (قرار المالك، 3 سبتمبر 2026).
          //
          // الواردة تبقى **كما هي حرفياً**: تبويباتها الثلاثة وبحثها وبطاقاتها
          // لم يُمسّ منها شيء، وإنما صارت تحت هذا المفتاح. والصادرة انتقلت
          // إليه من «آخر العمليات» في الواجهة، فخفّت الواجهة وزال التكرار.
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 16, R.padScreen, 0),
            child: _SectionSwitch(
              outgoing: _outgoing,
              // إغلاق اللوحة قبل تبديل القسم: حقل البحث يختفي مع «واردة»،
              // وحقلٌ يُنتزع من الشجرة وهو مركَّز يترك اللوحة معلّقة.
              onChanged: (v) {
                hideKeyboard();
                setState(() => _outgoing = v);
              },
            ),
          ),

          if (_outgoing)
            const Expanded(child: _OutgoingList())
          else ...[
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 18, R.padScreen, 0),
            child: _Tabs(
              index: _tab.index,
              pendingCount: page.pending,
              deliveredCount: page.delivered,
              cancelledCount: page.cancelled,
              onChanged: (i) => setState(() {
                _tab = IncomingTab.values[i];
                _shown = _pageSize;
              }),
            ),
          ),
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 12, R.padScreen, 0),
            child: _SearchField(
              controller: _search,
              focusNode: _searchFocus,
              onChanged: (v) => setState(() {
                _query = v;
                _shown = _pageSize;
              }),
            ),
          ),
          Expanded(
            child: async.when(
              loading: () => const _Loading(),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(_provider),
              ),
              data: (result) {
                // الفرز والترتيب والبحث كلّها في الخادم — لا شيء يُعاد
                // حسابه هنا، فلا يفترق ما يُعرَض عمّا هو محفوظ.
                final list = result.items;

                if (list.isEmpty) {
                  return _Empty(searching: _query.isNotEmpty, tab: _tab);
                }

                final visible = list.take(_shown).toList();
                return RefreshIndicator(
                  onRefresh: () async => ref.refresh(_provider.future),
                  color: R.primary,
                  backgroundColor: Colors.white,
                  child: ListView.separated(
                    padding: const EdgeInsets.fromLTRB(
                        R.padScreen, 16, R.padScreen, 120),
                    physics: const AlwaysScrollableScrollPhysics(),
                    itemCount: visible.length + 3,
                    separatorBuilder: (_, _) =>
                        const SizedBox(height: R.gapRow),
                    itemBuilder: (_, i) {
                      if (i == 0) {
                        return Padding(
                          padding: const EdgeInsets.only(bottom: 4),
                          child: Row(
                            children: [
                              Text(
                                  switch (_tab) {
                                    IncomingTab.pending => 'بانتظار التسليم',
                                    IncomingTab.delivered => 'تم التسليم',
                                    IncomingTab.cancelled => 'الملغاة',
                                  },
                                  style: T.section),
                              const SizedBox(width: 10),
                              _Badge(count: list.length),
                            ],
                          ),
                        );
                      }
                      if (i == visible.length + 2) return const SizedBox.shrink();
                      if (i == visible.length + 1) {
                        if (visible.length >= list.length) {
                          return const SizedBox.shrink();
                        }
                        return _MoreButton(
                          remaining: list.length - visible.length,
                          onTap: () => setState(() => _shown += _pageSize),
                        );
                      }
                      final t = visible[i - 1];
                      return RiseIn.small(
                        delay: Duration(milliseconds: 30 * ((i - 1) % _pageSize)),
                        child: _TransferRow(
                          t: t.legacy,
                          // الفاتورة نفسها في التبويبات الثلاث، وزرّ التسجيل
                          // داخلها لمن لم يُسجَّل بعد. ولا تراجع عن التسليم
                          // — منع نهائي بأمر المالك.
                          onTap: () => _openReceipt(t),
                          done: t.isDelivered,
                          cancelled: t.isCancelled,
                        ),
                      );
                    },
                  ),
                );
              },
            ),
          ),
          ],
        ],
      ),
    );
  }
}

/// مفتاح القسم الأعلى — واردة | صادرة.
///
/// شريحتان لا ثلاث: القسمة هنا اتجاهُ الحوالة، وحالاتُ الواردة الثلاث تبقى
/// في شريطها الخاصّ تحته. دمجُ المستويين في شريطٍ واحد كان يعطي خمس شرائح
/// لا تُقرأ، ويخلط سؤال «من أين؟» بسؤال «أين وصلت؟».
class _SectionSwitch extends StatelessWidget {
  const _SectionSwitch({required this.outgoing, required this.onChanged});

  final bool outgoing;
  final ValueChanged<bool> onChanged;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          Expanded(
            child: _SectionChip(
              label: 'واردة',
              icon: Icons.call_received_rounded,
              on: !outgoing,
              onTap: () => onChanged(false),
            ),
          ),
          const SizedBox(width: 8),
          Expanded(
            child: _SectionChip(
              label: 'صادرة',
              icon: Icons.call_made_rounded,
              on: outgoing,
              onTap: () => onChanged(true),
            ),
          ),
        ],
      );
}

class _SectionChip extends StatelessWidget {
  const _SectionChip({
    required this.label,
    required this.icon,
    required this.on,
    required this.onTap,
  });

  final String label;
  final IconData icon;
  final bool on;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => Material(
        color: on ? Colors.transparent : R.whiteA(.7),
        borderRadius: BorderRadius.circular(99),
        child: InkWell(
          borderRadius: BorderRadius.circular(99),
          onTap: onTap,
          child: Ink(
            height: 46,
            decoration: BoxDecoration(
              gradient: on ? R.primaryGradient : null,
              border: on ? null : Border.all(color: R.whiteA(.9)),
              borderRadius: BorderRadius.circular(99),
            ),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(icon, size: 17, color: on ? Colors.white : R.inkA(.55)),
                const SizedBox(width: 8),
                Text(label,
                    style: T.kufi(13, FontWeight.w700,
                        color: on ? Colors.white : R.inkA(.62))),
              ],
            ),
          ),
        ),
      );
}

/// «صادرة» — الحوالات التي أرسلها الوكيل.
///
/// تُقرأ من كشف الحساب لا من دفتر التسليم: ذلك الدفتر للواردة وحدها
/// (`InternalEx_SelectType_View_statetosForok` سجلّ تسليمٍ لا سجلّ إرسال —
/// انظر CLAUDE.md). والصادر هو ما خرج من حساب الوكيل.
///
/// وبطاقاتها هي **بطاقات «آخر العمليات» نفسها** بلا تغيير — نُقلت من الواجهة
/// إلى هنا، فما ألِفه الوكيل بقي كما هو.
class _OutgoingList extends ConsumerStatefulWidget {
  const _OutgoingList();

  @override
  ConsumerState<_OutgoingList> createState() => _OutgoingListState();
}

class _OutgoingListState extends ConsumerState<_OutgoingList> {
  /// null = «الكل». وترتيب الشرائح يتبع رحلة الحوالة لا الأبجدية.
  CoreStage? _stage;

  static const _stages = [
    CoreStage.pending,
    CoreStage.onWay,
    CoreStage.delivered,
    CoreStage.cancelling,
    CoreStage.cancelled,
  ];

  @override
  Widget build(BuildContext context) {
    final snap = ref.watch(statementProvider);
    // غير المعتمدة تُجلب على حدة: لا قيد لها في كشف الحساب حتى تُعتمد.
    // وفشلها لا يُسقط القائمة — تُعرض المعتمدة ويغيب الجديد وحده.
    final pending = ref.watch(pendingOutgoingProvider).valueOrNull ?? const [];
    final currency =
        ref.watch(authControllerProvider).user?.currencyCode ?? 'د.ل';

    return snap.when(
      loading: () => const Padding(
        padding: EdgeInsets.fromLTRB(R.padScreen, 16, R.padScreen, 0),
        child: MovementsSkeleton(),
      ),
      error: (e, _) => _Failed(
        message: '$e',
        onRetry: () => ref.invalidate(statementProvider),
      ),
      data: (rowsAll) {
        // الصادر = حركةُ حوالةٍ خرجت من الحساب. والعمولة مستثناة لأنها تظهر
        // سطراً داخل بطاقة حوالتها لا بطاقةً مستقلّة (قرار 3 سبتمبر 2026).
        // غير المعتمدة أوّلاً: هي الأحدث دائماً، وهي ما يبحث عنه الوكيل
        // فور إنشائه حوالة.
        final all = [
          ...pending,
          ...rowsAll
              .where((m) => m.isTransfer && !m.isCommission && !m.isCredit),
        ];

        if (all.isEmpty) return const _NoOutgoing();

        // الأعداد تُحسب على **كل** الصادرة لا على المعروض، وإلا صار كل عدد
        // صفراً إلا عدد الشريحة المختارة.
        final counts = <CoreStage, int>{};
        for (final m in all) {
          counts[m.stage] = (counts[m.stage] ?? 0) + 1;
        }

        final rows =
            _stage == null ? all : all.where((m) => m.stage == _stage).toList();

        return Column(
          children: [
            // شرائح المراحل — تظهر المرحلة فقط إن كان لها حوالةٌ فعلاً.
            //
            // شريحةٌ فارغة تعد الوكيل بشيء ثم تريه لا شيء؛ وإخفاؤها يجعل
            // الشريط يصف حوالاته هو، لا كتالوج المنظومة.
            Padding(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 14, R.padScreen, 2),
              child: SingleChildScrollView(
                scrollDirection: Axis.horizontal,
                physics: const ClampingScrollPhysics(),
                child: Row(
                  children: [
                    _StageChip(
                      label: 'الكل',
                      count: all.length,
                      on: _stage == null,
                      onTap: () => setState(() => _stage = null),
                    ),
                    for (final st in _stages)
                      if ((counts[st] ?? 0) > 0) ...[
                        const SizedBox(width: 8),
                        _StageChip(
                          label: st.label,
                          count: counts[st]!,
                          on: _stage == st,
                          onTap: () => setState(() => _stage = st),
                        ),
                      ],
                  ],
                ),
              ),
            ),
            Expanded(
              child: rows.isEmpty
                  ? const _NoOutgoing()
                  : RefreshIndicator(
                      onRefresh: () async =>
                          ref.refresh(statementProvider.future).then(
                              (_) => ref.refresh(pendingOutgoingProvider.future)),
                      color: R.primary,
                      backgroundColor: Colors.white,
                      child: ListView.separated(
                        padding: const EdgeInsets.fromLTRB(
                            R.padScreen, 14, R.padScreen, 120),
                        physics: const AlwaysScrollableScrollPhysics(),
                        itemCount: rows.length,
                        separatorBuilder: (_, _) =>
                            const SizedBox(height: R.gapRow),
                        itemBuilder: (_, i) => RiseIn.small(
                          delay: Duration(milliseconds: 40 * i),
                          child: MovementRow(m: rows[i], currency: currency),
                        ),
                      ),
                    ),
            ),
          ],
        );
      },
    );
  }
}

/// شريحة مرحلة — الاسم وبجانبه عددُه.
///
/// العدد ليس زينة: الوكيل يريد أن يعرف كم حوالةً عالقة «في الطريق» قبل أن
/// يفتحها، والرقم بجانب الاسم يجيب عن ذلك بلا ضغطة.
class _StageChip extends StatelessWidget {
  const _StageChip({
    required this.label,
    required this.count,
    required this.on,
    required this.onTap,
  });

  final String label;
  final int count;
  final bool on;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => Material(
        color: on ? Colors.transparent : R.whiteA(.7),
        borderRadius: BorderRadius.circular(99),
        child: InkWell(
          borderRadius: BorderRadius.circular(99),
          onTap: onTap,
          child: Ink(
            height: 40,
            decoration: BoxDecoration(
              gradient: on ? R.primaryGradient : null,
              border: on ? null : Border.all(color: R.whiteA(.9)),
              borderRadius: BorderRadius.circular(99),
            ),
            child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 15),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Text(label,
                      style: T.kufi(11.5, FontWeight.w600,
                          color: on ? Colors.white : R.inkA(.62))),
                  const SizedBox(width: 6),
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Text('$count',
                        style: T.plex(11, FontWeight.w700,
                            color: on ? R.whiteA(.85) : R.inkA(.42))),
                  ),
                ],
              ),
            ),
          ),
        ),
      );
}

class _NoOutgoing extends StatelessWidget {
  const _NoOutgoing();

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.all(R.padScreen),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Container(
                width: 74,
                height: 74,
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  color: R.primaryA(.09),
                ),
                child: Icon(Icons.call_made_rounded,
                    size: 30, color: R.primaryGradEnd),
              ),
              const SizedBox(height: 16),
              Text('لا حوالات صادرة بعد',
                  textAlign: TextAlign.center, style: T.titleSm),
              const SizedBox(height: 8),
              Text('ستظهر هنا كل حوالة ترسلها من الشاشة الرئيسية.',
                  textAlign: TextAlign.center,
                  style: T.plex(12.5, FontWeight.w400, color: R.inkA(.5))),
            ],
          ),
        ),
      );
}

/// شريط التبويب — بانتظار التسليم / تم التسليم، وبجانب كلٍّ عددُه.
///
/// المسمّيات تصف **حالة الحوالة** لا فعل المستخدم (قرار المالك، 2 سبتمبر
/// 2026): «تم التسليم» لا «سلَّمتُها». الحالة تُقرأ في أي سياق — سجلّ،
/// إشعار، تقرير — أما صيغة المتكلّم فتقرأ خطأً حين يقرؤها غير من سلَّم.
class _Tabs extends StatelessWidget {
  const _Tabs({
    required this.index,
    required this.onChanged,
    required this.pendingCount,
    required this.deliveredCount,
    required this.cancelledCount,
  });

  final int index;
  final ValueChanged<int> onChanged;

  /// العددان يُشتقّان من الدفتر نفسه الذي يبني القائمتين، فلا يفترقان عنهما
  /// ولا يحتاجان تحديثاً يدوياً بعد كل تسليم.
  final int pendingCount;
  final int deliveredCount;

  /// ما ألغته الرحالة ولم يكن قد سُلِّم — تقاطعٌ يحسبه الخادم لا حالةٌ ثالثة
  /// في العمود، وإلا تنازع تسليمُ الوكيل وإلغاءُ المنظومة خانةً واحدة.
  final int cancelledCount;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(4),
        decoration: BoxDecoration(
          color: R.whiteA(.62),
          border: Border.all(color: R.whiteA(.9)),
          borderRadius: BorderRadius.circular(R.rActions),
        ),
        child: Row(
          children: [
            Expanded(
                child: _tab('بانتظار التسليم', 0, Icons.schedule_rounded,
                    pendingCount)),
            const SizedBox(width: 5),
            Expanded(
                child:
                    _tab('تم التسليم', 1, Icons.check_rounded, deliveredCount)),
            const SizedBox(width: 5),
            Expanded(
                child: _tab('الملغاة', 2, Icons.block_rounded, cancelledCount)),
          ],
        ),
      );

  Widget _tab(String label, int i, IconData icon, int count) {
    final on = i == index;
    return Material(
      color: Colors.transparent,
      borderRadius: BorderRadius.circular(R.rRow),
      child: InkWell(
        borderRadius: BorderRadius.circular(R.rRow),
        onTap: () => onChanged(i),
        child: Ink(
          height: 46,
          decoration: BoxDecoration(
            gradient: on ? R.primaryGradient : null,
            borderRadius: BorderRadius.circular(R.rRow),
          ),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(icon,
                  size: 14, color: on ? Colors.white : R.inkA(.45)),
              const SizedBox(width: 5),
              Flexible(
                child: Text(label,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: T.kufi(10.5, FontWeight.w600,
                        color: on ? Colors.white : R.inkA(.55))),
              ),
              const SizedBox(width: 4),
              // العدد بين قوسين لاتينيّين واتّجاه LTR: الرقم داخل قوسين في
              // فقرة عربية ينقلب ترتيبه فيظهر «(8» و«)».
              Directionality(
                textDirection: TextDirection.ltr,
                child: Text('($count)',
                    style: T.kufi(10.5, FontWeight.w600,
                        color: on ? R.whiteA(.85) : R.inkA(.42))),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class _SearchField extends StatelessWidget {
  const _SearchField({
    required this.controller,
    required this.focusNode,
    required this.onChanged,
  });

  final TextEditingController controller;
  final FocusNode focusNode;
  final ValueChanged<String> onChanged;

  @override
  Widget build(BuildContext context) => GlassCard(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
        child: Row(
          children: [
            Icon(Icons.search_rounded, size: 20, color: R.inkA(.5)),
            const SizedBox(width: 10),
            Expanded(
              child: TextField(
                controller: controller,
                focusNode: focusNode,
                onChanged: onChanged,
                keyboardType: TextInputType.number,
                inputFormatters: [
                  WesternDigits(),
                  FilteringTextInputFormatter.digitsOnly,
                ],
                style: T.kufi(15, FontWeight.w600),
                decoration: InputDecoration(
                  isDense: true,
                  border: InputBorder.none,
                  hintText: 'ابحث برقم الحوالة أو رقم هاتف المستفيد',
                  hintStyle: T.plex(12.5, FontWeight.w400, color: R.inkA(.42)),
                ),
              ),
            ),
            if (controller.text.isNotEmpty)
              IconButton(
                onPressed: () {
                  controller.clear();
                  onChanged('');
                },
                icon: Icon(Icons.close_rounded, size: 18, color: R.inkA(.5)),
                constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
              ),
          ],
        ),
      );
}

class _TransferRow extends StatelessWidget {
  const _TransferRow({
    required this.t,
    required this.onTap,
    this.done = false,
    this.cancelled = false,
  });

  final IncomingTransfer t;
  final VoidCallback? onTap;

  /// سُجِّل تسليمها للمستفيد.
  final bool done;

  /// ألغتها الرحالة — وتعلو على [done] في الأيقونة كما تعلو عليه في
  /// التبويبات (أمر المالك، 3 سبتمبر 2026): الملغاة ملغاةٌ ولو سُلّمت،
  /// ولا وسم ثانياً يقول إنها سُلّمت.
  final bool cancelled;

  @override
  Widget build(BuildContext context) => GlassRow(
        onTap: onTap,
        children: [
          cancelled
              ? IconTile(
                  background: R.error.withValues(alpha: .12),
                  icon: Icon(Icons.block_rounded, size: 19, color: R.error),
                )
              : done
              ? IconTile(
                  background: R.primaryA(.14),
                  icon: Icon(Icons.check_rounded,
                      size: 19, color: R.primaryGradEnd),
                )
              : IconTile(
                  letter: t.receiverName.trim().isEmpty
                      ? '؟'
                      : t.receiverName.trim().characters.first),
          const SizedBox(width: 13),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  t.receiverName.isEmpty ? 'بلا اسم' : t.receiverName,
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: T.name,
                ),
                const SizedBox(height: 7),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Align(
                    alignment: AlignmentDirectional.centerStart,
                    child: Text('${t.code}  ·  ${t.receiverPhone}',
                        maxLines: 1, overflow: TextOverflow.ellipsis, style: T.meta),
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(width: 10),
          Column(
            crossAxisAlignment: CrossAxisAlignment.end,
            children: [
              Directionality(
                textDirection: TextDirection.ltr,
                child: Text(Fmt.money(t.amount), style: T.amount),
              ),
              const SizedBox(height: 7),
              Text(t.insertedAt.split(' ').first, style: T.meta),
            ],
          ),
        ],
      );
}

/// ورقة تأكيد التسليم — العملية غير قابلة للتراجع، فلا تُنفَّذ بلمسة واحدة.
class _Badge extends StatelessWidget {
  const _Badge({required this.count});

  final int count;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 11, vertical: 6),
        decoration: BoxDecoration(
          color: R.primaryA(.14),
          borderRadius: BorderRadius.circular(99),
        ),
        child: Directionality(
          textDirection: TextDirection.ltr,
          child: Text(Fmt.count(count),
              style: T.plex(11, FontWeight.w600, color: R.primaryDark)),
        ),
      );
}

class _MoreButton extends StatelessWidget {
  const _MoreButton({required this.remaining, required this.onTap});

  final int remaining;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.only(top: 6),
        child: GlassButton(
          label: 'عرض المزيد · بقي ${Fmt.count(remaining)}',
          onPressed: onTap,
        ),
      );
}

class _Loading extends StatelessWidget {
  const _Loading();

  @override
  Widget build(BuildContext context) => ListView.separated(
        padding: const EdgeInsets.fromLTRB(R.padScreen, 16, R.padScreen, 120),
        itemCount: 5,
        separatorBuilder: (_, _) => const SizedBox(height: R.gapRow),
        itemBuilder: (_, _) => GlassRow(children: [
          Container(
            width: 40,
            height: 40,
            decoration: BoxDecoration(
              color: R.inkA(.06),
              borderRadius: BorderRadius.circular(R.rTile),
            ),
          ),
          const SizedBox(width: 13),
          Expanded(
            child: Container(
              height: 12,
              decoration: BoxDecoration(
                color: R.inkA(.06),
                borderRadius: BorderRadius.circular(9),
              ),
            ),
          ),
        ]),
      );
}

class _Empty extends StatelessWidget {
  const _Empty({required this.searching, required this.tab});

  final bool searching;
  final IncomingTab tab;

  bool get pending => tab == IncomingTab.pending;

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(40, 0, 40, 90),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              // أيقونة تصف الحالة لا شعار الشركة: الشعار نفسه في التبويبين
              // لا يقول أيّهما فارغ ولماذا.
              Floaty(
                child: Container(
                  width: 108,
                  height: 108,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: R.primaryA(.07),
                  ),
                  alignment: Alignment.center,
                  child: Icon(
                    searching
                        ? Icons.search_off_rounded
                        : switch (tab) {
                            IncomingTab.pending => Icons.pending_actions_rounded,
                            IncomingTab.delivered => Icons.task_alt_rounded,
                            IncomingTab.cancelled => Icons.block_rounded,
                          },
                    size: 46,
                    color: R.primaryA(.55),
                  ),
                ),
              ),
              const SizedBox(height: 22),
              Text(
                searching
                    ? 'لا نتائج لهذا البحث'
                    : switch (tab) {
                        IncomingTab.pending =>
                          'لا توجد حوالات بانتظار التسليم حالياً',
                        IncomingTab.delivered =>
                          'لا توجد حوالات تم تسليمها بعد',
                        IncomingTab.cancelled => 'لا توجد حوالات ملغاة',
                      },
                textAlign: TextAlign.center,
                style: T.kufi(15, FontWeight.w600, height: 1.5),
              ),
              const SizedBox(height: 10),
              Text(
                searching
                    ? 'جرّب رقم حوالة أو رقم هاتف آخر.'
                    : switch (tab) {
                        IncomingTab.pending =>
                          'ستظهر هنا الحوالات الجديدة المحالة إليك فور استلامها',
                        IncomingTab.delivered =>
                          'ستظهر هنا الحوالات التي تم تسليمها للمستفيدين',
                        IncomingTab.cancelled =>
                          'ستظهر هنا الحوالات التي ألغتها الرحالة قبل تسليمها',
                      },
                textAlign: TextAlign.center,
                style: T.plex(12.5, FontWeight.w400, color: R.inkA(.55), height: 1.7),
              ),
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
          padding: const EdgeInsets.fromLTRB(R.padScreen, 0, R.padScreen, 90),
          child: GlassCard(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Icon(Icons.error_outline_rounded, size: 18, color: R.error),
                    const SizedBox(width: 10),
                    Expanded(
                      child: Text(message,
                          style: T.plex(12.5, FontWeight.w500,
                              color: R.errorText, height: 1.6)),
                    ),
                  ],
                ),
                const SizedBox(height: 12),
                TextButton(
                  onPressed: onRetry,
                  style: TextButton.styleFrom(minimumSize: const Size(44, 44)),
                  child: Text('إعادة المحاولة',
                      style: T.plex(12.5, FontWeight.w600, color: R.primaryGradEnd)),
                ),
              ],
            ),
          ),
        ),
      );
}

/// تنبيه أسفل القائمة: هذا دفتر على هذا الجهاز لا سِجل في المنظومة.
///
/// دائمٌ لا يُغلَق عمداً — الوكيل قد يبني عليه ترتيب يومه، فيجب أن يعرف
/// أنه يزول مع إعادة التثبيت، وأنه لا علاقة له بالحسابات.
