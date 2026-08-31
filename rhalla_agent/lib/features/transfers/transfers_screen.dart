import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import 'delivery_log.dart';
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

  /// الخادم يعيد كل الحوالات دفعة واحدة (رُصد 522 صفاً).
  /// نعرض دفعات ونزيد عند الطلب بدل تجميد القائمة.
  static const _pageSize = 20;
  int _shown = _pageSize;

  /// 0 = بانتظار التسليم · 1 = سلَّمتُها
  int _tab = 0;

  bool get _isPending => _tab == 0;

  /// مصدرٌ واحد للتبويبين — الدفتر المحلّي هو من يفرزهما.
  ///
  /// كان تبويب «سلَّمتُها» يُطلب من `InternalEx_SelectType_View_statetosForok`
  /// وهو سِجل تسليم في المنظومة الرئيسية يعيد **صفر صفوف** لحساب وكيل.
  /// وطلبٌ أقلّ على الخادم أيضاً.
  AutoDisposeFutureProvider<List<IncomingTransfer>> get _provider =>
      incomingTransfersProvider;

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

  /// فاتورة الحوالة — عرض فقط، لا تُغيّر شيئاً.
  void _openReceipt(IncomingTransfer t) => Navigator.of(context, rootNavigator: true)
      .push(MaterialPageRoute(builder: (_) => DeliveryReceiptScreen(transfer: t)));

  /// مسح السجل — بحاجز تأكيد، فهو زرّ لا رجعة فيه.
  Future<void> _confirmReset() async {
    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => const _ResetSheet(),
    );
    if (ok != true || !mounted) return;
    await ref.read(deliveryLogProvider.notifier).resetAll();
    if (!mounted) return;
    setState(() => _shown = _pageSize);
    _toast('أُفرِغ السجل — يبدأ من أول حوالة جديدة', ok: true);
  }

  void _toast(String msg, {required bool ok}) {
    ScaffoldMessenger.of(context)
      ..hideCurrentSnackBar()
      ..showSnackBar(SnackBar(
        content: Text(msg, style: T.plex(13, FontWeight.w500, color: Colors.white)),
        backgroundColor: ok ? R.primaryGradEnd : R.error,
        behavior: SnackBarBehavior.floating,
        margin: const EdgeInsets.fromLTRB(16, 0, 16, 100),
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
      ));
  }

  @override
  Widget build(BuildContext context) {
    final user = ref.watch(authControllerProvider).user;
    final async = ref.watch(_provider);
    final log = ref.watch(deliveryLogProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'الحوالات',
            subtitle: '${user?.branchName ?? ''} · التسليم والمتابعة',
            trailing: log.delivered.isEmpty
                ? null
                : IconButton(
                    tooltip: 'مسح السجل',
                    onPressed: _confirmReset,
                    icon: Icon(Icons.delete_sweep_outlined,
                        size: 21, color: R.inkA(.55)),
                    constraints:
                        const BoxConstraints(minWidth: 44, minHeight: 44),
                  ),
          ),
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 18, R.padScreen, 0),
            child: _Tabs(
              index: _tab,
              onChanged: (i) => setState(() {
                _tab = i;
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
              data: (server) {
                // خطّ الأساس يُلتقط من أول قائمة تصل — بعده تبدأ الشاشة
                // فارغة وتعمل من أول حوالة جديدة. راجع DeliveryState.baseline.
                if (!log.ready) {
                  WidgetsBinding.instance.addPostFrameCallback((_) {
                    ref
                        .read(deliveryLogProvider.notifier)
                        .captureBaseline(server.map((t) => t.code));
                  });
                  return const _Loading();
                }

                // الفرز محلّي بالكامل: الخادم لا يعرف شيئاً عن هذا الدفتر.
                final scope = server.where((t) => !log.isHidden(t.code));
                final list = scope
                    .where((t) =>
                        _isPending ? !log.isDelivered(t.code) : log.isDelivered(t.code))
                    .where((t) => t.matches(_query))
                    .toList();

                if (!_isPending) {
                  // الأحدث تسليماً أولاً.
                  list.sort((a, b) => (log.deliveredAt(b.code) ?? DateTime(0))
                      .compareTo(log.deliveredAt(a.code) ?? DateTime(0)));
                }

                if (list.isEmpty) {
                  return _Empty(
                      searching: _query.isNotEmpty, pending: _isPending);
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
                                  _isPending
                                      ? 'بانتظار التسليم'
                                      : 'سلَّمتُها',
                                  style: T.section),
                              const SizedBox(width: 10),
                              _Badge(count: list.length),
                            ],
                          ),
                        );
                      }
                      // آخر عنصر: تنبيه دائم بأن هذا دفتر على هذا الجهاز.
                      if (i == visible.length + 2) return const _LocalNote();
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
                          t: t,
                          // سِجل التسليم للقراءة — لا يُسلَّم ما سُلِّم.
                          // لا تراجع عن التسليم — منع نهائي بأمر المالك.
                          // الفاتورة نفسها في التبويبين — وزرّ التسجيل
                          // داخلها لمن لم يُسجَّل بعد.
                          onTap: () => _openReceipt(t),
                          done: !_isPending,
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

/// شريط التبويب — بانتظار التسليم / سلَّمتُها.
class _Tabs extends StatelessWidget {
  const _Tabs({required this.index, required this.onChanged});

  final int index;
  final ValueChanged<int> onChanged;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(5),
        decoration: BoxDecoration(
          color: R.whiteA(.62),
          border: Border.all(color: R.whiteA(.9)),
          borderRadius: BorderRadius.circular(R.rActions),
        ),
        child: Row(
          children: [
            Expanded(
                child: _tab('بانتظار التسليم', 0, Icons.schedule_rounded)),
            const SizedBox(width: 5),
            Expanded(child: _tab('سلَّمتُها', 1, Icons.check_rounded)),
          ],
        ),
      );

  Widget _tab(String label, int i, IconData icon) {
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
                  size: 16, color: on ? Colors.white : R.inkA(.45)),
              const SizedBox(width: 7),
              Flexible(
                child: Text(label,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: T.kufi(12, FontWeight.w600,
                        color: on ? Colors.white : R.inkA(.55))),
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
                  hintText: 'ابحث برمز الحوالة أو رقم المستفيد',
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
  const _TransferRow({required this.t, required this.onTap, this.done = false});

  final IncomingTransfer t;
  final VoidCallback? onTap;

  /// صف في سِجل التسليم — لا إجراء عليه.
  final bool done;

  @override
  Widget build(BuildContext context) => GlassRow(
        onTap: onTap,
        children: [
          done
              ? IconTile(
                  background: R.primaryA(.14),
                  icon: const Icon(Icons.check_rounded,
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
  const _Empty({required this.searching, this.pending = true});

  final bool searching;
  final bool pending;

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(40, 0, 40, 90),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Floaty(child: RhallaLogo(size: 64, color: R.primaryA(.3))),
              const SizedBox(height: 22),
              Text(
                searching
                    ? 'لا نتائج لهذا البحث'
                    : pending
                        ? 'لا توجد حوالات بانتظار التسليم'
                        : 'لم تُسلِّم أي حوالة بعد',
                textAlign: TextAlign.center,
                style: T.kufi(15, FontWeight.w600, height: 1.5),
              ),
              const SizedBox(height: 10),
              Text(
                searching
                    ? 'جرّب رمزاً أو رقماً آخر.'
                    : pending
                        ? 'كل ما وصل إلى فرعك تمّ تسليمه. الحوالات الجديدة تظهر هنا فور وصولها.'
                        : 'ما تُسلِّمه من تبويب «بانتظار التسليم» يُسجَّل هنا.',
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
class _LocalNote extends StatelessWidget {
  const _LocalNote();

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.only(top: 14),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Icon(Icons.info_outline_rounded, size: 15, color: R.inkA(.38)),
            const SizedBox(width: 8),
            Expanded(
              child: Text(
                'سِجل تنظيمي على هذا الجهاز وحده — لا يمسّ حسابات المنظومة. '
                'ويُفرَغ تماماً عند إعادة تثبيت التطبيق أو مسح بياناته، '
                'فيبدأ من أول حوالة جديدة.',
                style: T.plex(10.5, FontWeight.w400,
                    color: R.inkA(.45), height: 1.75),
              ),
            ),
          ],
        ),
      );
}

/// حاجز التأكيد قبل مسح السجل.
class _ResetSheet extends StatelessWidget {
  const _ResetSheet();

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
            const SizedBox(height: 22),
            Center(
              child: Text('عفواً — هل تريد حذف البيانات؟',
                  textAlign: TextAlign.center,
                  style: T.kufi(17, FontWeight.w700, color: R.error)),
            ),
            const SizedBox(height: 12),
            Text(
              'سيُفرَغ سِجل التسليم على هذا الجهاز ويبدأ من جديد، '
              'ويعمل من أول حوالة تصل بعد المسح.\n'
              'لا أثر لهذا على حسابات المنظومة ولا على الحوالات نفسها.',
              textAlign: TextAlign.center,
              style: T.plex(12.5, FontWeight.w400, color: R.inkA(.6), height: 1.7),
            ),
            const SizedBox(height: 22),
            PrimaryButton(
              label: 'نعم، احذف',
              onPressed: () => Navigator.of(context).pop(true),
            ),
            const SizedBox(height: 10),
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
              child: Text('لا',
                  style: T.plex(13.5, FontWeight.w600, color: R.inkA(.6))),
            ),
          ],
        ),
      );
}
