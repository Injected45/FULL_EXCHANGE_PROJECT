import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';
import 'package:url_launcher/url_launcher.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import 'agent_incoming_repository.dart';
import 'delivery_receipt_screen.dart';
import 'receipt.dart';

/// فاتورة حوالةٍ **أرسلها** الوكيل.
///
/// جسمها هو [TransferInvoice] نفسه الذي تعرضه فاتورة الواردة، وحاوية سبب
/// الإلغاء هي [CancelReasonBox] نفسها — نسخةٌ ثانية منهما كانت ستفترق عن
/// الأولى عند أول تعديل، فيرى الوكيل فاتورتين بتنسيقين.
///
/// **والفرق الوحيد: لا زرّ تسليم.** الحوالة الصادرة يسلّمها فرعٌ آخر
/// للمستفيد، ولا شأن لهذا الوكيل بتسجيل تسليمها — وزرٌّ هنا كان سيسجّل
/// تسليماً لا يملكه.
class OutgoingReceiptScreen extends ConsumerStatefulWidget {
  const OutgoingReceiptScreen({super.key, required this.transfer});

  final OutgoingTransfer transfer;

  @override
  ConsumerState<OutgoingReceiptScreen> createState() =>
      _OutgoingReceiptScreenState();
}

class _OutgoingReceiptScreenState extends ConsumerState<OutgoingReceiptScreen>
    with ReceiptTools {
  OutgoingTransfer get t => widget.transfer;

  Future<void> _print() => printReceipt(name: t.legacy.code);

  Future<void> _share() => shareReceipt(
        name: t.legacy.code,
        text: 'حوالة ${t.legacy.code} — ${t.legacy.receiverName}',
      );

  Future<void> _call() async {
    final p = Fmt.phoneForApi(t.legacy.receiverPhone);
    if (p.isEmpty) return;
    if (!await launchUrl(Uri.parse('tel:+218$p'))) {
      if (mounted) receiptToast('تعذّر فتح تطبيق الاتصال.');
    }
  }

  void _copyPhone() {
    Clipboard.setData(ClipboardData(text: t.legacy.receiverPhone));
    receiptToast('نُسخ رقم المستلم');
  }

  /// لون الحالة بمرحلتها لا باسمها: الأسماء قد تُحرَّر في جدول المنظومة،
  /// والأرقام عقدٌ ثابت (0 غير معتمدة · 1 و7–9 في الطريق · 2 مسلمه ·
  /// 3 و4 و10 قيد الإلغاء · 5 و6 ملغية).
  Color get _statusColor => switch (t.confirmType) {
        2 => R.primaryGradEnd,
        3 || 4 || 5 || 6 || 10 => R.error,
        0 => R.warnIcon,
        _ => R.primaryDark,
      };

  @override
  Widget build(BuildContext context) {
    final currency =
        ref.watch(authControllerProvider).user?.currencyCode ?? 'د.ل';

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'فاتورة حوالة محلية صادرة',
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
                  child: TransferInvoice(
                    t: t.legacy,
                    currency: currency,
                    // العمولة لا تُعرض في الفاتورة (قرار المالك، 3 سبتمبر
                    // 2026): صفرٌ هنا يُبقي السطر مخفياً كما في الواردة.
                    commission: 0,
                    onCall: _call,
                    onCopy: _copyPhone,
                    cancelled: t.isCancelled,
                    showSender: false,
                    // حالة الصادرة **كما تكتبها المنظومة حرفياً**: «غير
                    // معتمدة» · «غير مسلمه» · «مسلمه» · «ملغية» · «مرسلة مع
                    // مندوب» … لا اختصار، فالفرق بينها يعني أين الحوالة الآن.
                    statusLabel: t.statusName,
                    statusColor: _statusColor,
                  ),
                ),
                if (t.isCancelled) ...[
                  const SizedBox(height: 16),
                  CancelReasonBox(
                    reason: t.cancelReason,
                    notes: t.cancelNotes,
                  ),
                ],
                const SizedBox(height: 20),
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
}
