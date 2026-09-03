import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'agent_incoming_repository.dart';
import 'delivery_receipt_screen.dart';
import 'transfers_repository.dart';

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
            title: 'الحوالات الواردة',
            // بلا سطر فرعي — قرار المالك (2 سبتمبر 2026): لا اسم الوكيل ولا
            // «التسليم والمتابعة». اسم الفرع يبقى ظاهراً في تبويب الحساب.
            // لا زرّ «مسح السجل»: السجل صار في قاعدة الخادم، ومسحُه من
            // الهاتف كان يمحو دليل من سُلِّم.
          ),
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
      ),
    );
  }
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

  /// ألغتها الرحالة. تُميَّز بالأحمر حتى في «تم التسليم»: الوكيل دفع مالها،
  /// ومعرفته بالإلغاء تعنيه فوراً.
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
