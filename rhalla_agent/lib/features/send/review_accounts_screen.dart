import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import '../shell/auto_refresh.dart';
import 'accounts_repository.dart';
import 'limit_dialog.dart';
import 'send_repository.dart' show InsufficientFunds, TransferLimitExceeded;

/// مراجعة التحويل بين الحسابات — اللوحة `design/ReviewAccounts.dc.html`.
///
/// وُجدت هذه الخطوة متأخّرة: كان النموذج ينفّذ مباشرة، مع أن التحويل بين
/// الحسابات فوري ولا رجعة فيه تماماً كالحوالة الداخلية التي لها مراجعة.
///
/// وفرقها عن [ReviewTransferScreen] ليس شكلياً: العمولة هنا **يحسبها
/// الخادم**، فتُعرض قيمةً ساكنة موسومة بذلك. إظهارها كحقل قابل للتحرير
/// يوهم الوكيل بأنه يملك تعديلها.
class ReviewAccountsScreen extends ConsumerStatefulWidget {
  const ReviewAccountsScreen({super.key, required this.draft});

  final AccountsDraft draft;

  @override
  ConsumerState<ReviewAccountsScreen> createState() =>
      _ReviewAccountsScreenState();
}

class _ReviewAccountsScreenState extends ConsumerState<ReviewAccountsScreen> {
  bool _sending = false;

  Future<void> _confirm() async {
    final user = ref.read(authControllerProvider).user;
    if (user == null) return;

    setState(() => _sending = true);
    final d = widget.draft;

    try {
      final t = await ref.read(accountsRepositoryProvider).create(
            fromAccId: user.accId,
            toAccId: d.target.accId,
            currencyId: user.currencyId,
            amount: d.amount,
            branchId: user.branchId,
            notes: d.notes ?? '',
            receiverPhone: d.target.phone,
          );
      if (!mounted) return;
      // الرصيد تغيّر على الخادم.
      refreshAfterMoneyAction(ref);
      context.pushReplacement('/send/accounts/done', extra: t);
    } on ApiFailure catch (e) {
      if (!mounted) return;
      setState(() => _sending = false);

      // تجاوز السقف حدٌّ لا خطأ — حوار في وسط الشاشة لا شريط أحمر.
      final overLimit = TransferLimitExceeded.from(e);
      if (overLimit != null) {
        await showLimitExceededDialog(context, overLimit);
        return;
      }

      if (!mounted) return;
      // «رصيد غير كافٍ» يصل كحقول لا كنص — انظر InsufficientFunds.
      final short = InsufficientFunds.from(e);
      ScaffoldMessenger.of(context)
        ..hideCurrentSnackBar()
        ..showSnackBar(SnackBar(
          content: Text(short?.describe() ?? e.message,
              style: T.plex(13, FontWeight.w500, color: Colors.white)),
          backgroundColor: R.error,
          behavior: SnackBarBehavior.floating,
          margin: const EdgeInsets.fromLTRB(16, 0, 16, 24),
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
        ));
    }
  }

  @override
  Widget build(BuildContext context) {
    final d = widget.draft;
    final currency =
        ref.watch(authControllerProvider).user?.currencyCode ?? 'د.ل';
    final notes = d.notes?.trim() ?? '';

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'مراجعة التحويل',
            subtitle: 'تأكّد من البيانات قبل التنفيذ',
            onBack: _sending ? null : () => context.pop(),
          ),
          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 22, R.padScreen, 20),
              children: [
                RiseIn(
                  duration: const Duration(milliseconds: 500),
                  child: GlassCard(
                    large: true,
                    sheen: true,
                    padding: const EdgeInsets.symmetric(
                        horizontal: 22, vertical: 22),
                    child: Column(
                      children: [
                        Text('يُضاف إلى الحساب المستفيد', style: T.label),
                        const SizedBox(height: 13),
                        Directionality(
                          textDirection: TextDirection.ltr,
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            crossAxisAlignment: CrossAxisAlignment.baseline,
                            textBaseline: TextBaseline.alphabetic,
                            children: [
                              Text(currency,
                                  style: T.plex(12, FontWeight.w400,
                                      color: R.inkA(.5))),
                              const SizedBox(width: 8),
                              Text(Fmt.money(d.amount),
                                  style: T.kufi(36, FontWeight.w800)),
                            ],
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
                const SizedBox(height: R.gapCard),
                RiseIn.small(
                  delay: const Duration(milliseconds: 80),
                  child: GlassCard(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('الحساب المستفيد', style: T.label),
                        const SizedBox(height: 12),
                        Row(
                          children: [
                            IconTile(letter: d.target.name.characters.first),
                            const SizedBox(width: 13),
                            Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(d.target.name,
                                      maxLines: 1,
                                      overflow: TextOverflow.ellipsis,
                                      style: T.name),
                                  const SizedBox(height: 7),
                                  Directionality(
                                    textDirection: TextDirection.ltr,
                                    child: Align(
                                      alignment:
                                          AlignmentDirectional.centerStart,
                                      child: Text(
                                          '+218 ${Fmt.phone(d.target.phone)}',
                                          style: T.meta),
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 14),
                        Divider(color: R.inkA(.07), height: 1),
                        const SizedBox(height: 14),
                        KvRow('رقم الحساب', d.target.code, numeric: true),
                        if (d.target.branchName.isNotEmpty) ...[
                          const SizedBox(height: 12),
                          KvRow('الفرع', d.target.branchName),
                        ],
                      ],
                    ),
                  ),
                ),
                const SizedBox(height: R.gapCard),
                RiseIn.small(
                  delay: const Duration(milliseconds: 140),
                  child: GlassCard(
                    child: Column(
                      children: [
                        KvRow('المبلغ', Fmt.money(d.amount), numeric: true),
                        const SizedBox(height: 12),
                        KvRow('العمولة', Fmt.money(d.commission),
                            numeric: true, sub: 'يحسبها الخادم'),
                        const SizedBox(height: 14),
                        Divider(color: R.inkA(.07), height: 1),
                        const SizedBox(height: 14),
                        KvRow('الإجمالي المخصوم من رصيدك', Fmt.money(d.total),
                            numeric: true, strong: true),
                      ],
                    ),
                  ),
                ),
                if (notes.isNotEmpty) ...[
                  const SizedBox(height: R.gapCard),
                  GlassCard(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('سبب التحويل', style: T.label),
                        const SizedBox(height: 9),
                        Text(notes,
                            style: T.plex(14, FontWeight.w500, height: 1.7)),
                      ],
                    ),
                  ),
                ],
                const SizedBox(height: R.gapCard),
                const WarnBanner(
                  text: 'التحويل بين الحسابات فوري ولا يمكن التراجع عنه — '
                      'إلغاؤه يتطلّب مراجعة الفرع.',
                ),
              ],
            ),
          ),
          Container(
            padding:
                const EdgeInsets.fromLTRB(R.padScreen, 14, R.padScreen, 22),
            decoration: const BoxDecoration(
              gradient: LinearGradient(
                begin: Alignment.topCenter,
                end: Alignment.bottomCenter,
                colors: [Color(0x00F1F8F5), Color(0xF0F1F8F5), R.scrimBottom],
                stops: [0, .34, 1],
              ),
            ),
            child: Column(
              children: [
                PrimaryButton(
                  label: 'تأكيد التنفيذ',
                  loading: _sending,
                  onPressed: _sending ? null : _confirm,
                ),
                const SizedBox(height: 8),
                TextButton(
                  onPressed: _sending ? null : () => context.pop(),
                  style: TextButton.styleFrom(minimumSize: const Size(44, 44)),
                  child: Text('تعديل البيانات',
                      style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
