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
import 'limit_dialog.dart';
import 'send_repository.dart';

class ReviewTransferScreen extends ConsumerStatefulWidget {
  const ReviewTransferScreen({super.key, required this.draft});

  final TransferDraft draft;

  @override
  ConsumerState<ReviewTransferScreen> createState() =>
      _ReviewTransferScreenState();
}

class _ReviewTransferScreenState extends ConsumerState<ReviewTransferScreen> {
  bool _sending = false;

  Future<void> _confirm() async {
    final user = ref.read(authControllerProvider).user;
    if (user == null) return;

    setState(() => _sending = true);

    try {
      final created = await ref.read(sendRepositoryProvider).createInternal(
            d: widget.draft,
            accId: user.accId,
          );
      if (!mounted) return;
      // الرصيد والعمليات تغيّرا على الخادم.
      refreshAfterMoneyAction(ref);
      context.pushReplacement('/send/internal/done', extra: created);
    } on ApiFailure catch (e) {
      if (!mounted) return;
      setState(() => _sending = false);

      final short = InsufficientFunds.from(e);
      if (short != null) {
        await showModalBottomSheet(
          context: context,
          isScrollControlled: true,
          backgroundColor: Colors.transparent,
          builder: (_) => _InsufficientSheet(
            data: short,
            currency: user.currencyCode,
          ),
        );
        return;
      }

      // تجاوز السقف ليس خطأً من الوكيل بل حدّاً بلغه — يُعرض حواراً
      // كهرمانياً في وسط الشاشة لا شريطاً أحمر.
      final overLimit = TransferLimitExceeded.from(e);
      if (overLimit != null) {
        await showLimitExceededDialog(context, overLimit);
        return;
      }

      if (!mounted) return;
      ScaffoldMessenger.of(context)
        ..hideCurrentSnackBar()
        ..showSnackBar(SnackBar(
          content: Text(e.message,
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

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'مراجعة الحوالة',
            subtitle: 'تأكّد من البيانات قبل الإرسال',
            onBack: _sending ? null : () => context.pop(),
          ),
          Expanded(
            child: ListView(
              padding: const EdgeInsets.fromLTRB(R.padScreen, 22, R.padScreen, 20),
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
                        Text('يستلم المستفيد', style: T.label),
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
                        Text('المستفيد', style: T.label),
                        const SizedBox(height: 12),
                        Row(
                          children: [
                            IconTile(letter: d.receiverName.characters.first),
                            const SizedBox(width: 13),
                            Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(d.receiverName,
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
                                          '+218 ${Fmt.phone(d.receiverPhone)}',
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
                        KvRow('مدينة الاستلام', d.city.name),
                        const SizedBox(height: 12),
                        KvRow('فرع الاستلام', d.branch.name),
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
                            numeric: true, sub: 'حدّدتها أنت'),
                        const SizedBox(height: 14),
                        Divider(color: R.inkA(.07), height: 1),
                        const SizedBox(height: 14),
                        KvRow('الإجمالي المخصوم من رصيدك', Fmt.money(d.total),
                            numeric: true, strong: true),
                      ],
                    ),
                  ),
                ),
                if (d.notes != null && d.notes!.trim().isNotEmpty) ...[
                  const SizedBox(height: R.gapCard),
                  GlassCard(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('ملاحظات', style: T.label),
                        const SizedBox(height: 9),
                        Text(d.notes!.trim(),
                            style: T.plex(14, FontWeight.w500, height: 1.7)),
                      ],
                    ),
                  ),
                ],
                const SizedBox(height: R.gapCard),
                const WarnBanner(
                  text:
                      'بعد الإرسال لا يمكن تعديل الحوالة — إلغاؤها يتطلّب مراجعة الفرع.',
                ),
              ],
            ),
          ),
          Container(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 14, R.padScreen, 22),
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
                  label: 'تأكيد وإرسال',
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


/// «رصيد غير كافٍ» — مبنيّة من الحقول التي يعيدها الخادم داخل `message`.
class _InsufficientSheet extends StatelessWidget {
  const _InsufficientSheet({required this.data, required this.currency});

  final InsufficientFunds data;
  final String currency;

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
            Row(
              children: [
                Container(
                  width: 46,
                  height: 46,
                  alignment: Alignment.center,
                  decoration: BoxDecoration(
                    color: R.error.withValues(alpha: .08),
                    borderRadius: BorderRadius.circular(16),
                  ),
                  child: Icon(Icons.warning_amber_rounded,
                      size: 22, color: R.error),
                ),
                const SizedBox(width: 13),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('رصيد غير كافٍ',
                          style: T.kufi(17, FontWeight.w600)),
                      const SizedBox(height: 8),
                      Text('الإجمالي المطلوب يتجاوز رصيد وكالتك.',
                          style: T.plex(12, FontWeight.w400,
                              color: R.inkA(.58), height: 1.5)),
                    ],
                  ),
                ),
              ],
            ),
            const SizedBox(height: 20),
            Container(
              padding:
                  const EdgeInsets.symmetric(horizontal: 18, vertical: 16),
              decoration: BoxDecoration(
                color: R.inkA(.05),
                borderRadius: BorderRadius.circular(R.rRow),
              ),
              child: Column(
                children: [
                  // الخادم لا يعيد الرصيد حين لا يكفي — استعلامه يشترط
                  // Walet >= total. لا نعرض رقماً لا نملكه.
                  if (data.wallet != null) ...[
                    KvRow('رصيدك الحالي', Fmt.money(data.wallet), numeric: true),
                    const SizedBox(height: 12),
                  ],
                  KvRow('المبلغ', Fmt.money(data.amount), numeric: true),
                  const SizedBox(height: 12),
                  KvRow('العمولة', Fmt.money(data.commission), numeric: true),
                  const SizedBox(height: 12),
                  Divider(color: R.inkA(.07), height: 1),
                  const SizedBox(height: 12),
                  KvRow('الإجمالي المطلوب', Fmt.money(data.total),
                      numeric: true, strong: true),
                  if (data.shortfall != null) ...[
                    const SizedBox(height: 12),
                    Row(
                      children: [
                        Text('المبلغ الناقص',
                            style:
                                T.plex(12.5, FontWeight.w500, color: R.error)),
                        const Spacer(),
                        Directionality(
                          textDirection: TextDirection.ltr,
                          child: Text(Fmt.money(data.shortfall),
                              style:
                                  T.kufi(17, FontWeight.w700, color: R.error)),
                        ),
                      ],
                    ),
                  ],
                ],
              ),
            ),
            const SizedBox(height: 20),
            PrimaryButton(
              label: 'تعديل المبلغ',
              onPressed: () => Navigator.of(context).pop(),
            ),
          ],
        ),
      );
}
