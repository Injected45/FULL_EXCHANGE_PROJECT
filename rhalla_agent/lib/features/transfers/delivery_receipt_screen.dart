
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';
import 'package:url_launcher/url_launcher.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import 'delivery_log.dart';
import 'receipt.dart';
import 'transfers_repository.dart';

/// فاتورة حوالة داخلية — تُفتح بالضغط على حوالة في «سلَّمتُها».
///
/// **الطباعة والمشاركة تصوّران الفاتورة صورةً لا نصّاً.** السبب عربي بحت:
/// توليد PDF نصّي يتطلّب تشكيل الحروف العربية ووصلها داخل مكتبة الـ PDF،
/// وهو مصدر أعطال معروف (حروف منفصلة أو معكوسة). أما التصوير فيلتقط ما
/// يراه الوكيل حرفياً بخطّ التطبيق نفسه — فما يُطبع هو ما يُرى.
class DeliveryReceiptScreen extends ConsumerStatefulWidget {
  const DeliveryReceiptScreen({super.key, required this.transfer});

  final IncomingTransfer transfer;

  @override
  ConsumerState<DeliveryReceiptScreen> createState() =>
      _DeliveryReceiptScreenState();
}

class _DeliveryReceiptScreenState extends ConsumerState<DeliveryReceiptScreen>
    with ReceiptTools {
  Future<void> _print() =>
      printReceipt(name: widget.transfer.code);

  Future<void> _share() => shareReceipt(
        name: widget.transfer.code,
        text: 'حوالة ${widget.transfer.code} — ${widget.transfer.receiverName}',
      );


  @override
  Widget build(BuildContext context) {
    final t = widget.transfer;
    // الرمز من الخادم لا مكتوباً في الكود — قاعدة المشروع.
    final currency =
        ref.watch(authControllerProvider).user?.currencyCode ?? 'د.ل';
    // نفس الفاتورة في التبويبين — والفرق زرٌّ واحد لمن لم يُسجَّل بعد.
    final done = ref.watch(deliveryLogProvider).isDelivered(t.code);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'فاتورة حوالة داخلية',
            onBack: () => context.pop(),
            trailing: IconButton(
              tooltip: 'طباعة',
              onPressed: receiptBusy ? null : _print,
              icon: Icon(Icons.print_outlined, size: 22, color: R.primaryDark),
              constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
            ),
          ),
          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 18, R.padScreen, 30),
              children: [
                RepaintBoundary(
                  key: receiptKey,
                  child: _Invoice(
                      t: t,
                      currency: currency,
                      onCall: _call,
                      onCopy: _copyPhone),
                ),
                const SizedBox(height: 20),
                if (!done) ...[
                  PrimaryButton(
                    label: 'تسجيل التسليم',
                    icon: const Icon(Icons.check_rounded,
                        size: 18, color: Colors.white),
                    onPressed: receiptBusy ? null : _markDelivered,
                  ),
                  const SizedBox(height: 10),
                ],
                PrimaryButton(
                  label: 'مشاركة الفاتورة',
                  loading: receiptBusy,
                  icon: const Icon(Icons.share_rounded,
                      size: 18, color: Colors.white),
                  onPressed: receiptBusy ? null : _share,
                ),
                const SizedBox(height: 10),
                GlassButton(
                  label: 'طباعة',
                  onPressed: receiptBusy ? null : _print,
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _call() async {
    final p = Fmt.phoneForApi(widget.transfer.receiverPhone);
    if (p.isEmpty) return;
    final uri = Uri.parse('tel:+218$p');
    if (!await launchUrl(uri)) {
      if (mounted) receiptToast('تعذّر فتح تطبيق الاتصال.');
    }
  }

  /// تسجيل التسليم من داخل الفاتورة — بعد أن رأى الوكيل البيانات كاملة،
  /// لا قبلها. ولا استدعاء للخادم: دفتر محلّي.
  Future<void> _markDelivered() async {
    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _ConfirmDeliver(name: widget.transfer.receiverName),
    );
    if (ok != true || !mounted) return;
    await ref.read(deliveryLogProvider.notifier)
        .markDelivered(widget.transfer.code);
    if (!mounted) return;
    Navigator.of(context).pop();
  }

  void _copyPhone() {
    Clipboard.setData(ClipboardData(text: widget.transfer.receiverPhone));
    receiptToast('نُسخ رقم المستلم');
  }
}

/// جسم الفاتورة — هذا ما يُصوَّر للطباعة والمشاركة.
///
/// خلفيته بيضاء صريحة لا زجاجية: الزجاج شفّاف، فتصويره يلتقط ما خلفه
/// وتخرج الفاتورة على الورق بخلفية متّسخة.
class _Invoice extends StatelessWidget {
  const _Invoice({
    required this.t,
    required this.currency,
    required this.onCall,
    required this.onCopy,
  });

  final IncomingTransfer t;
  final String currency;
  final VoidCallback onCall;
  final VoidCallback onCopy;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(18, 20, 18, 20),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(R.rCardXl),
          border: Border.all(color: R.inkA(.06)),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            const ReceiptHeader(),
            const SizedBox(height: 20),
            ReceiptRow('تاريخ التحويل',
                Fmt.stamp(t.insertedAt, separator: '    '), ltr: true),
            ReceiptRow('رقم الكود', t.code, ltr: true, strong: true),
            ReceiptRow('اسم المرسل', t.senderName, strong: true),
            if (t.destination.isNotEmpty) ReceiptRow('الوجهة', t.destination),
            if (t.branchName.isNotEmpty) ReceiptRow('فرع المرسل', t.branchName),
            const SizedBox(height: 12),
            BoxedField(
              icon: Icons.person_outline_rounded,
              label: 'اسم المستلم',
              child: Text(t.receiverName,
                  textAlign: TextAlign.start,
                  style: T.kufi(17, FontWeight.w700)),
            ),
            const SizedBox(height: 10),
            BoxedField(
              icon: Icons.phone_outlined,
              label: 'هاتف المستلم',
              // الرقم أولاً — أي من يمين الحاوية — والأزرار بعده.
              child: Row(
                children: [
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Text(t.receiverPhone,
                        style: T.kufi(16, FontWeight.w700)),
                  ),
                  const Spacer(),
                  MiniButton(
                      label: 'نسخ',
                      icon: Icons.copy_rounded,
                      onTap: onCopy),
                  const SizedBox(width: 8),
                  MiniButton(
                      label: 'اتصال',
                      icon: Icons.call_rounded,
                      filled: true,
                      onTap: onCall),
                ],
              ),
            ),
            const SizedBox(height: 14),
            Divider(color: R.inkA(.07), height: 1),
            const SizedBox(height: 14),
            ReceiptRow('قيمة الحوالة', Fmt.money(t.amount),
                ltr: true, strong: true, currency: currency),
          ],
        ),
      );
}

/// تأكيد تسجيل التسليم — البيانات معروضة فوقه، فلا يُعيدها.
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
            Center(child: Text('تسجيل التسليم', style: T.kufi(17, FontWeight.w700))),
            const SizedBox(height: 10),
            Text('تأكّد من هوية المستلم قبل التسجيل:\n$name',
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w500, color: R.inkA(.65), height: 1.7)),
            const SizedBox(height: 16),
            const WarnBanner(
              text: 'تسجيل في دفترك وحدك — لا يمسّ حسابات المنظومة، '
                  'ولا رجعة فيه: لا تعود الحوالة إلى «بانتظار التسليم».',
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
