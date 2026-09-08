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

/// «طلباتي» — ما أرسله الموظف إلى وكيله وما آل إليه.
///
/// ══════════════════════════════════════════════════════════════════════════
///  لماذا هذه الشاشة ليست ترفاً
/// ══════════════════════════════════════════════════════════════════════════
///
/// ⚠ بدونها تنتهي دورةُ الطلب عند الموظف بلا خبر: يرسل، ثمّ لا يعرف أوُوفق
/// أم رُفض إلّا بسؤال وكيله. وهو واقفٌ أمام زبونٍ ينتظر.
///
/// ⚠ **والأهمُّ أنّ الموافقة لا تعني التنفيذ دائماً**: الوكيل قد يوافق ثمّ
/// يردّ المسارُ الماليّ الحوالةَ لعدم كفاية الرصيد أو لمهلة الدقيقة. فحالةُ
/// «وُوفق عليها ولم تُنفَّذ» يجب أن يراها الموظف بنصّها وسببها — وإلّا سلّم
/// الزبونَ مالاً على حوالةٍ لم تقع.
class EmployeeApprovalsScreen extends ConsumerStatefulWidget {
  const EmployeeApprovalsScreen({super.key});

  @override
  ConsumerState<EmployeeApprovalsScreen> createState() =>
      _EmployeeApprovalsScreenState();
}

class _EmployeeApprovalsScreenState
    extends ConsumerState<EmployeeApprovalsScreen> {
  int? _busyId;

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(myApprovalsProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'طلباتي', onBack: () => context.pop()),
          Expanded(
            child: RefreshIndicator(
              onRefresh: () async => ref.invalidate(myApprovalsProvider),
              color: R.primary,
              backgroundColor: Colors.white,
              child: async.when(
                loading: () => const Center(child: CircularProgressIndicator()),
                error: (e, _) => _Msg(
                  icon: Icons.wifi_off_rounded,
                  text: 'تعذّر تحميل طلباتك.\n$e',
                ),
                data: (rows) => rows.isEmpty
                    ? const _Msg(
                        icon: Icons.inbox_rounded,
                        text: 'لا طلبات.\n\nالحوالات التي تتجاوز سقفك '
                            'تظهر هنا حتى يبتّ فيها الوكيل.',
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
                          onCancel: () => _cancel(rows[i]),
                          onExecute: () => _execute(rows[i]),
                        ),
                      ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _cancel(MyApproval r) async {
    setState(() => _busyId = r.id);
    try {
      final env = await ref
          .read(apiClientProvider)
          .post('/device/employee/approvals/${r.id}/cancel');
      if (!mounted) return;
      _say(env.messageText ?? 'أُلغي الطلب.');
      ref.invalidate(myApprovalsProvider);
    } on ApiFailure catch (e) {
      if (mounted) _say(e.message);
    } catch (_) {
      if (mounted) _say('تعذّر الإلغاء — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busyId = null);
    }
  }

  /// تنفيذُ ما أذن به الوكيل — بتأكيدٍ يقول ما سيقع.
  Future<void> _execute(MyApproval r) async {
    setState(() => _busyId = r.id);
    try {
      final env = await ref
          .read(apiClientProvider)
          .post('/device/employee/approvals/${r.id}/execute');
      if (!mounted) return;
      // ⚠ رسالةُ الخادم كما هي: هو من يعرف أنُفِّذت أم ردّها المسار
      // الماليّ ولماذا — «رصيد غير كافٍ» مثلاً.
      _say(env.messageText ?? 'نُفِّذت الحوالة.');
      ref.invalidate(myApprovalsProvider);
    } on ApiFailure catch (e) {
      if (mounted) _say(e.message);
    } catch (_) {
      if (mounted) _say('تعذّر التنفيذ — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busyId = null);
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

/* ───────────────── الطراز والمزوّد ───────────────── */

class MyApproval {
  const MyApproval({
    required this.id,
    required this.amount,
    required this.recipientName,
    required this.recipientPhone,
    required this.reasonLabels,
    required this.status,
    required this.transferNumber,
    required this.failureReason,
    required this.createdAt,
  });

  final int id;
  final double amount;
  final String? recipientName;
  final String? recipientPhone;
  final List<String> reasonLabels;
  final String status;
  final String? transferNumber;
  final String? failureReason;
  final String? createdAt;

  bool get isPending => status == 'PENDING';

  /// ⚠ أُذن بها ولم تُنفَّذ بعد — **وهي حالةُ فعلٍ لا حالةُ انتظار**.
  ///
  /// الوكيلُ أذن، والمالُ عند الموظف، فالتنفيذُ بيده هو. وما دامت هنا
  /// فلا حوالةَ وقعت ولا نقدَ دخل خزينته.
  bool get needsExecute => status == 'APPROVED' && transferNumber == null;

  /// ⚠ النصوصُ من زاوية الموظف لا الوكيل: هو يقرأ «بانتظار موافقة الوكيل»
  /// لا «بانتظار موافقتك».
  String get label => switch (status) {
        'PENDING'   => 'بانتظار موافقة الوكيل',
        'APPROVED'  => transferNumber != null
            ? 'نُفِّذت'
            : 'وافق الوكيل — نفّذها الآن',
        'REJECTED'  => 'رفضها الوكيل',
        'EXPIRED'   => 'انتهت مدّة الطلب',
        'CANCELLED' => 'ألغيتها',
        'FAILED'    => 'وُوفق عليها ولم تُنفَّذ',
        _           => status,
      };

  Color get tone => switch (status) {
        'PENDING' => R.warnIcon,
        'APPROVED' => transferNumber != null ? R.primaryDark : R.warnIcon,
        'REJECTED' || 'FAILED' => R.error,
        _ => R.inkA(.5),
      };

  static MyApproval fromJson(Map<String, dynamic> j) => MyApproval(
        id: int.tryParse('${j['id']}') ?? 0,
        amount: Fmt.num_(j['amount']),
        recipientName: _s(j['recipient_name']),
        recipientPhone: _s(j['recipient_phone']),
        reasonLabels: (j['reason_labels'] as List?)
                ?.map((e) => '$e')
                .where((e) => e.trim().isNotEmpty)
                .toList() ??
            const [],
        status: '${j['status'] ?? ''}',
        transferNumber: _s(j['transfer_number']),
        failureReason: _s(j['failure_reason']),
        createdAt: _s(j['created_at']),
      );

  static String? _s(dynamic v) {
    final s = '${v ?? ''}'.trim();
    return s.isEmpty || s == 'null' ? null : s;
  }
}

final myApprovalsProvider =
    FutureProvider.autoDispose<List<MyApproval>>((ref) async {
  final env = await ref.watch(apiClientProvider).get('/device/employee/approvals');
  final rows = (env.row?['requests'] as List?) ?? const [];
  return rows
      .map((e) => MyApproval.fromJson((e as Map).cast<String, dynamic>()))
      .toList();
});

/* ───────────────── البطاقة ───────────────── */

class _Card extends StatelessWidget {
  const _Card({
    required this.r,
    required this.busy,
    required this.onCancel,
    required this.onExecute,
  });

  final MyApproval r;
  final bool busy;
  final VoidCallback onCancel;
  final VoidCallback onExecute;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Expanded(
                  child: Directionality(
                    textDirection: TextDirection.ltr,
                    child: Row(
                      children: [
                        Text('د.ل',
                            style: T.kufi(12.5, FontWeight.w700,
                                color: R.primaryDark)),
                        const SizedBox(width: 5),
                        Text(Fmt.money(r.amount),
                            style: T.kufi(19, FontWeight.w800, color: R.ink)),
                      ],
                    ),
                  ),
                ),
                Container(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
                  decoration: BoxDecoration(
                    color: r.tone.withValues(alpha: .10),
                    border: Border.all(color: r.tone.withValues(alpha: .32)),
                    borderRadius: BorderRadius.circular(99),
                  ),
                  child: Text(r.label,
                      style: T.plex(11, FontWeight.w700, color: r.tone)),
                ),
              ],
            ),
            const SizedBox(height: 10),

            if (r.recipientName != null)
              _Line(Icons.person_outline_rounded, r.recipientName!),
            if (r.recipientPhone != null)
              _Line(Icons.phone_outlined, Fmt.phone(r.recipientPhone!), ltr: true),
            _Line(Icons.schedule_rounded, Fmt.stampShort(r.createdAt)),

            if (r.transferNumber != null)
              _Line(Icons.receipt_long_rounded,
                  'رقم الحوالة · ${r.transferNumber}',
                  ltr: true),

            if (r.reasonLabels.isNotEmpty) ...[
              const SizedBox(height: 6),
              ...r.reasonLabels.map((s) => Text('• $s',
                  style: T.plex(11.5, FontWeight.w500,
                      color: R.inkA(.55), height: 1.7))),
            ],

            // ⚠ سببُ عدم التنفيذ يُعرض بارزاً: هو الفرقُ بين «سلِّم الزبون»
            // و«لا تسلّمه».
            if (r.failureReason != null) ...[
              const SizedBox(height: 8),
              Container(
                padding: const EdgeInsets.all(10),
                decoration: BoxDecoration(
                  color: R.error.withValues(alpha: .06),
                  border: Border.all(color: R.error.withValues(alpha: .25)),
                  borderRadius: BorderRadius.circular(R.rCard),
                ),
                child: Text(r.failureReason!,
                    style: T.plex(12, FontWeight.w600,
                        color: R.error, height: 1.7)),
              ),
            ],

            if (r.isPending) ...[
              const SizedBox(height: 12),
              SecondaryButton(
                label: 'سحب الطلب',
                icon: Icon(Icons.undo_rounded, size: 18, color: R.inkA(.6)),
                onPressed: busy ? null : onCancel,
              ),
            ],

            /*
             * ⚠ زرُّ التنفيذ — والمالُ بيد الموظف لا بيد وكيله.
             *
             * الوكيلُ أذن من مكتبه، والزبونُ واقفٌ هنا. فالحوالةُ تُنفَّذ
             * حين يقبض الموظفُ النقد، ويدخل عندها عهدتَه وخزينتَه.
             */
            if (r.needsExecute) ...[
              const SizedBox(height: 12),
              Text(
                'وافق الوكيل. نفّذها بعد استلام المبلغ من الزبون — '
                'يدخل حينها في خزينتك.',
                style: T.plex(12, FontWeight.w500,
                    color: R.inkA(.62), height: 1.7),
              ),
              const SizedBox(height: 10),
              PrimaryButton(
                label: 'تنفيذ الحوالة',
                loading: busy,
                onPressed: busy ? null : onExecute,
              ),
            ],
          ],
        ),
      );
}

class _Line extends StatelessWidget {
  const _Line(this.icon, this.text, {this.ltr = false});

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

class _Msg extends StatelessWidget {
  const _Msg({required this.icon, required this.text});
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
