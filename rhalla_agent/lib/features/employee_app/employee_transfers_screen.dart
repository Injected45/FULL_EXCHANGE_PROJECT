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
    extends ConsumerState<EmployeeTransfersScreen> {
  IncomingTab _tab = IncomingTab.pending;

  List<AgentIncomingTransfer> _rows = const [];
  Map<String, int> _counts = const {};
  bool _loading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    setState(() { _loading = true; _error = null; });
    try {
      final env = await ref.read(apiClientProvider).get(
        '/device/employee/transfers/incoming',
        query: {'status': _tab.wire, 'per_page': 50},
      );
      final data = env.row ?? const {};
      final list = ((data['rows'] ?? data['data'] ?? []) as List)
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
      if (mounted) setState(() { _error = e.message; _loading = false; });
    } catch (_) {
      if (mounted) {
        setState(() { _error = 'تعذّر الاتصال بالخادم.'; _loading = false; });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final canDeliver =
        ref.watch(employeeAuthProvider).profile?.can('DELIVER_TRANSFER') ?? false;

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'الحوالات الواردة', onBack: () => context.pop()),
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 6, R.padScreen, 10),
            child: _Tabs(
              current: _tab,
              counts: _counts,
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
    required this.onPick,
  });

  final IncomingTab current;
  final Map<String, int> counts;
  final ValueChanged<IncomingTab> onPick;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          for (final t in IncomingTab.values) ...[
            if (t != IncomingTab.values.first) const SizedBox(width: 8),
            Expanded(
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

  String _label(IncomingTab t) => switch (t) {
        IncomingTab.pending => 'بانتظار التسليم',
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
      await ref.read(apiClientProvider)
          .post('/device/employee/transfers/${widget.t.id}/deliver');
      await widget.onDelivered();
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
              IncomingTab.pending => 'لا توجد حوالات بانتظار التسليم',
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
