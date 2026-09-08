import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'approvals_repository.dart';

/// «طلبات الموافقة» — حوالاتُ الموظفين التي تجاوزت سياستهم.
///
/// ⚠ **الطلبُ هنا ليس حوالةً معلّقة، بل ليس حوالةً بعد.** لا رصيدَ خُصم ولا
/// قيدَ كُتب. فالرفضُ لا يُلغي شيئاً — يمنع فقط أن يقع.
///
/// وهذه الجملةُ معروضةٌ في الشاشة، لأنّ وكيلاً يظنّ أنه يرفض حوالةً وقعت
/// سيتردّد في الرفض، وتردّدُه هو ما تلتفّ عليه السياسةُ كلُّها.
class ApprovalsScreen extends ConsumerStatefulWidget {
  const ApprovalsScreen({super.key});

  @override
  ConsumerState<ApprovalsScreen> createState() => _ApprovalsScreenState();
}

class _ApprovalsScreenState extends ConsumerState<ApprovalsScreen> {
  /// التبويبات — والمعلَّق أوّلاً لأنه وحده يحتاج فعلاً.
  static const _tabs = <(String, String)>[
    ('PENDING', 'بانتظار الموافقة'),
    ('APPROVED', 'تمت الموافقة'),
    ('REJECTED', 'مرفوضة'),
    ('EXPIRED', 'منتهية'),
    ('ALL', 'الكل'),
  ];

  String _status = 'PENDING';
  int? _busyId;

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(approvalsProvider(_status));

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'طلبات الموافقة',
            onBack: () => context.pop(),
          ),
          _tabsRow(),
          Expanded(
            child: RefreshIndicator(
              onRefresh: () async => ref.invalidate(approvalsProvider(_status)),
              child: async.when(
                loading: () => const Center(child: CircularProgressIndicator()),
                error: (e, _) => _Message(
                  icon: Icons.wifi_off_rounded,
                  text: 'تعذّر تحميل الطلبات.\n$e',
                ),
                data: (rows) => rows.isEmpty
                    ? _Message(
                        icon: Icons.inbox_rounded,
                        text: _status == 'PENDING'
                            ? 'لا طلبات تنتظر موافقتك.'
                            : 'لا طلبات في هذا التبويب.',
                      )
                    : ListView.separated(
                        padding: const EdgeInsets.fromLTRB(
                            R.padScreen, 12, R.padScreen, 30),
                        itemCount: rows.length,
                        separatorBuilder: (_, _) =>
                            const SizedBox(height: R.gapCard),
                        itemBuilder: (_, i) => _Card(
                          r: rows[i],
                          busy: _busyId == rows[i].id,
                          onApprove: () => _decide(rows[i], true),
                          onReject: () => _decide(rows[i], false),
                        ),
                      ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _tabsRow() => SizedBox(
        height: 46,
        child: ListView.separated(
          scrollDirection: Axis.horizontal,
          padding: const EdgeInsets.symmetric(horizontal: R.padScreen),
          itemCount: _tabs.length,
          separatorBuilder: (_, _) => const SizedBox(width: 8),
          itemBuilder: (_, i) {
            final (key, label) = _tabs[i];
            final on = _status == key;
            return Center(
              child: InkWell(
                onTap: () => setState(() => _status = key),
                borderRadius: BorderRadius.circular(99),
                child: Container(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 15, vertical: 8),
                  decoration: BoxDecoration(
                    color: on ? R.primaryA(.12) : R.whiteA(.55),
                    border: Border.all(
                        color: on ? R.primaryA(.45) : R.inkA(.10),
                        width: on ? 1.4 : 1),
                    borderRadius: BorderRadius.circular(99),
                  ),
                  child: Text(label,
                      style: T.plex(12.5, on ? FontWeight.w700 : FontWeight.w500,
                          color: on ? R.primaryDark : R.inkA(.6))),
                ),
              ),
            );
          },
        ),
      );

  Future<void> _decide(ApprovalRequest r, bool approve) async {
    /*
     * ⚠ تأكيدٌ قبل الموافقة — لا قبل الرفض.
     *
     * الموافقةُ تُخرج مالاً ولا تُستردّ بضغطة، والرفضُ لا يُخرج شيئاً
     * ويستطيع الموظف إعادة الطلب. فالحاجزُ حيث الأثرُ لا حيث السلبية.
     */
    if (approve) {
      final ok = await showModalBottomSheet<bool>(
        context: context,
        useRootNavigator: true,
        backgroundColor: Colors.transparent,
        builder: (_) => _ConfirmApprove(r: r),
      );
      if (ok != true || !mounted) return;
    }

    setState(() => _busyId = r.id);

    try {
      final repo = ref.read(approvalsRepositoryProvider);
      // ⚠ رسالةُ الخادم كما هي: هو وحده يعرف أنُفِّذت الحوالة أم ردّها
      // المسارُ الماليّ ولماذا — «رصيد غير كافٍ» مثلاً.
      final msg = approve ? await repo.approve(r.id) : await repo.reject(r.id);

      if (!mounted) return;
      _say(msg);
      for (final (k, _) in _tabs) {
        ref.invalidate(approvalsProvider(k));
      }
    } on ApiFailure catch (e) {
      if (mounted) _say(e.message);
    } catch (_) {
      if (mounted) _say('تعذّر تنفيذ العملية — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busyId = null);
    }
  }

  void _say(String m) {
    ScaffoldMessenger.of(context)
      ..hideCurrentSnackBar()
      ..showSnackBar(SnackBar(
        content: Text(m,
            style: T.plex(13, FontWeight.w500, color: Colors.white)),
        backgroundColor: R.inkA(.92),
        behavior: SnackBarBehavior.floating,
        duration: const Duration(seconds: 4),
      ));
  }
}

class _Card extends StatelessWidget {
  const _Card({
    required this.r,
    required this.busy,
    required this.onApprove,
    required this.onReject,
  });

  final ApprovalRequest r;
  final bool busy;
  final VoidCallback onApprove;
  final VoidCallback onReject;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(r.employeeName,
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style: T.kufi(14.5, FontWeight.w700)),
                      if (r.pointOfSale != null) ...[
                        const SizedBox(height: 2),
                        Text(r.pointOfSale!,
                            style: T.plex(11.5, FontWeight.w400,
                                color: R.inkA(.5))),
                      ],
                    ],
                  ),
                ),
                _StatusPill(r: r),
              ],
            ),
            const SizedBox(height: 12),

            // المبلغُ بارزٌ: هو أوّلُ ما يُقرَّر عليه.
            Directionality(
              textDirection: TextDirection.ltr,
              child: Row(
                mainAxisAlignment: MainAxisAlignment.end,
                children: [
                  Text(r.currencyCode ?? 'د.ل',
                      style: T.kufi(13, FontWeight.w700, color: R.primaryDark)),
                  const SizedBox(width: 6),
                  Text(Fmt.money(r.amount),
                      style: T.kufi(21, FontWeight.w800, color: R.ink)),
                ],
              ),
            ),

            if (r.limitAtRequest != null) ...[
              const SizedBox(height: 4),
              // ⚠ «وقت الطلب» مكتوبةٌ صراحةً: السقفُ قد يكون تغيّر بعده.
              Directionality(
                textDirection: TextDirection.ltr,
                child: Text(
                  'الحد: ${Fmt.money(r.limitAtRequest!)}',
                  style: T.plex(11.5, FontWeight.w500, color: R.inkA(.5)),
                ),
              ),
            ],

            const SizedBox(height: 12),
            if (r.recipientName != null)
              _Line(icon: Icons.person_outline_rounded, text: r.recipientName!),
            if (r.recipientPhone != null)
              _Line(
                icon: Icons.phone_outlined,
                text: Fmt.phone(r.recipientPhone!),
                ltr: true,
              ),
            _Line(
              icon: Icons.schedule_rounded,
              text: 'وقت الطلب · ${Fmt.stampShort(r.createdAt)}',
            ),

            const SizedBox(height: 10),
            // سببُ التصعيد — وقد يكون أكثر من واحد لطلبٍ واحد.
            ...r.reasonLabels.map((s) => Padding(
                  padding: const EdgeInsets.only(bottom: 6),
                  child: Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Icon(Icons.report_gmailerrorred_rounded,
                          size: 15, color: R.warnIcon),
                      const SizedBox(width: 6),
                      Expanded(
                        child: Text(s,
                            style: T.plex(12, FontWeight.w600,
                                color: R.warnIcon, height: 1.6)),
                      ),
                    ],
                  ),
                )),

            if (r.transferNumber != null) ...[
              const SizedBox(height: 4),
              _Line(
                icon: Icons.receipt_long_rounded,
                text: 'رقم الحوالة · ${r.transferNumber}',
                ltr: true,
              ),
            ],
            if (r.failureReason != null) ...[
              const SizedBox(height: 6),
              Text(r.failureReason!,
                  style: T.plex(12, FontWeight.w600, color: R.error, height: 1.6)),
            ],

            if (r.isPending) ...[
              const SizedBox(height: 14),
              Row(
                children: [
                  Expanded(
                    child: PrimaryButton(
                      label: 'موافقة',
                      loading: busy,
                      onPressed: busy ? null : onApprove,
                    ),
                  ),
                  const SizedBox(width: 10),
                  Expanded(
                    child: SecondaryButton(
                      label: 'رفض',
                      icon: Icon(Icons.close_rounded, size: 18, color: R.error),
                      onPressed: busy ? null : onReject,
                    ),
                  ),
                ],
              ),
            ],
          ],
        ),
      );
}

class _StatusPill extends StatelessWidget {
  const _StatusPill({required this.r});
  final ApprovalRequest r;

  @override
  Widget build(BuildContext context) {
    final tone = switch (r.status) {
      'PENDING' => R.warnIcon,
      'APPROVED' => R.primaryDark,
      'REJECTED' || 'FAILED' => R.error,
      _ => R.inkA(.5),
    };

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
      decoration: BoxDecoration(
        color: tone.withValues(alpha: .10),
        border: Border.all(color: tone.withValues(alpha: .3)),
        borderRadius: BorderRadius.circular(99),
      ),
      child: Text(r.statusLabel,
          style: T.plex(11, FontWeight.w700, color: tone)),
    );
  }
}

class _Line extends StatelessWidget {
  const _Line({required this.icon, required this.text, this.ltr = false});

  final IconData icon;
  final String text;
  final bool ltr;

  @override
  Widget build(BuildContext context) {
    final body = Text(text,
        maxLines: 2,
        overflow: TextOverflow.ellipsis,
        style: T.plex(12.5, FontWeight.w500, color: R.inkA(.62)));

    return Padding(
      padding: const EdgeInsets.only(bottom: 5),
      child: Row(
        children: [
          Icon(icon, size: 15, color: R.inkA(.4)),
          const SizedBox(width: 7),
          Expanded(
            child: ltr
                ? Directionality(textDirection: TextDirection.ltr, child: body)
                : body,
          ),
        ],
      ),
    );
  }
}

/// تأكيدُ الموافقة — يقول بصراحة ما الذي سيقع.
class _ConfirmApprove extends StatelessWidget {
  const _ConfirmApprove({required this.r});
  final ApprovalRequest r;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(22, 22, 22, 26),
        decoration: BoxDecoration(
          color: R.whiteA(.96),
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
            Text('تأكيد الموافقة',
                textAlign: TextAlign.center,
                style: T.kufi(16, FontWeight.w700)),
            const SizedBox(height: 12),
            Directionality(
              textDirection: TextDirection.ltr,
              child: Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text(r.currencyCode ?? 'د.ل',
                      style: T.kufi(14, FontWeight.w700, color: R.primaryDark)),
                  const SizedBox(width: 6),
                  Text(Fmt.money(r.amount),
                      style: T.kufi(24, FontWeight.w800, color: R.ink)),
                ],
              ),
            ),
            const SizedBox(height: 12),
            Text(
              'ستُنفَّذ الحوالة الآن باسمك، وتُحتسب على حسابك مع الرحالة '
              'كأي حوالة تنشئها بنفسك.\n\n'
              'والموافقة لهذه الحوالة وحدها — لا ترفع سقف الموظف، '
              'ولا تتجاوز رصيدك أو شروط الحوالة.',
              textAlign: TextAlign.center,
              style: T.plex(12.5, FontWeight.w400,
                  color: R.inkA(.62), height: 1.9),
            ),
            const SizedBox(height: 20),
            PrimaryButton(
              label: 'موافقة وتنفيذ',
              onPressed: () => Navigator.of(context).pop(true),
            ),
            const SizedBox(height: 8),
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
              child: Text('تراجع',
                  style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
            ),
          ],
        ),
      );
}

class _Message extends StatelessWidget {
  const _Message({required this.icon, required this.text});
  final IconData icon;
  final String text;

  @override
  Widget build(BuildContext context) => ListView(
        padding: const EdgeInsets.fromLTRB(30, 80, 30, 30),
        children: [
          Icon(icon, size: 44, color: R.inkA(.24)),
          const SizedBox(height: 16),
          Text(text,
              textAlign: TextAlign.center,
              style: T.plex(13, FontWeight.w500,
                  color: R.inkA(.5), height: 1.8)),
        ],
      );
}
