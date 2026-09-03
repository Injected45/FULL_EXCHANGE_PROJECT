import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';
import 'package:url_launcher/url_launcher.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import '../favorites/favorites_repository.dart';
import '../favorites/favorites_screen.dart';
import '../transfers/receipt.dart';
import 'send_layout.dart';
import 'send_repository.dart';

/// شاشة «تمّت الحوالة» — فاتورةٌ لا إشعارُ نجاحٍ فقط.
///
/// شكلها مطابق لفاتورة «بانتظار التسليم» و«سلَّمتُها» بقرار المالك: الوكيل
/// يطبع ويشارك الثلاث، فاختلافها كان سيجعله يشكّ أيّها الصحيحة. ولهذا
/// تُبنى من مكوّنات [receipt.dart] نفسها لا من نسخةٍ عنها.
///
/// ولا رمز حوالة فيها — أزاله المالك صراحةً.
class TransferDoneScreen extends ConsumerStatefulWidget {
  const TransferDoneScreen({super.key, required this.transfer});

  final CreatedTransfer transfer;

  @override
  ConsumerState<TransferDoneScreen> createState() => _TransferDoneScreenState();
}

class _TransferDoneScreenState extends ConsumerState<TransferDoneScreen>
    with ReceiptTools {
  /// «حوالة جديدة» تُسقط النموذج القديم من المكدّس قبل أن تفتح واحداً جديداً.
  ///
  /// المسار كان: النموذج ← المراجعة ← (استبدال) النجاح. فحين تستبدل
  /// «حوالة جديدة» شاشةَ النجاح وحدها، يبقى **النموذج الممتلئ** حيّاً تحتها
  /// في المكدّس — فيراه الوكيل بأول ضغطة رجوع ببيانات حوالةٍ نُفِّذت
  /// بالفعل، ويظنّها مسوّدةً بين يديه.
  ///
  /// go('/') تُسقط كل ما فوق الهيكل وتتخلّص من حالته، ثم push في الإطار
  /// التالي تبني نموذجاً وليداً بحقول فارغة. التقسيم على إطارين مقصود:
  /// نداءان متتاليان في اللحظة نفسها قد يبنيان على تهيئةٍ لم تُطبَّق بعد.
  void _newTransfer() {
    context.go('/');
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (mounted) context.push('/send/internal');
    });
  }

  /// اسم ملفّ الطباعة والمشاركة. لا رمز يُعرض في هذه الشاشة، فالمستفيد
  /// هو ما يميّز الملف للوكيل حين يبحث عنه في هاتفه لاحقاً.
  String get _fileName => widget.transfer.receiverName.isEmpty
      ? 'محلية'
      : widget.transfer.receiverName;

  Future<void> _print() => printReceipt(name: _fileName);

  Future<void> _share() => shareReceipt(
        name: _fileName,
        text: 'حوالة محلية إلى ${widget.transfer.receiverName}',
      );

  Future<void> _call() async {
    final p = Fmt.phoneForApi(widget.transfer.receiverPhone);
    if (p.isEmpty) return;
    final uri = Uri.parse('tel:+218$p');
    if (!await launchUrl(uri)) {
      if (mounted) receiptToast('تعذّر فتح تطبيق الاتصال.');
    }
  }

  void _copyPhone() {
    final p = widget.transfer.receiverPhone;
    if (p.isEmpty) return;
    Clipboard.setData(ClipboardData(text: p));
    receiptToast('نُسخ رقم المستلم');
  }

  @override
  Widget build(BuildContext context) {
    final t = widget.transfer;
    final user = ref.watch(authControllerProvider).user;
    // الرمز من الخادم لا مكتوباً في الكود — قاعدة المشروع.
    final currency = user?.currencyCode ?? 'د.ل';

    return PopScope(
      // العملية تمّت — لا رجوع إلى المراجعة.
      canPop: false,
      child: Screen(
        child: Column(
          children: [
            RhallaAppBar(
              title: 'تمّت الحوالة',
              trailing: IconButton(
                tooltip: 'طباعة',
                onPressed: receiptBusy ? null : _print,
                icon:
                    Icon(Icons.print_outlined, size: 22, color: R.primaryDark),
                constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
              ),
            ),
            Expanded(
              child: ListView(
                padding:
                    const EdgeInsets.fromLTRB(R.padScreen, 4, R.padScreen, 2),
                children: [
                  Center(
                    child: SizedBox(
                      width: 52,
                      height: 52,
                      child: Stack(
                        alignment: Alignment.center,
                        children: [
                          const Positioned.fill(child: PulseRing(seconds: 2.4)),
                          Container(
                            width: 40,
                            height: 40,
                            decoration: BoxDecoration(
                              shape: BoxShape.circle,
                              gradient: R.primaryGradient,
                              boxShadow: [
                                BoxShadow(
                                  color: R.primaryA(.36),
                                  blurRadius: 34,
                                  offset: const Offset(0, 16),
                                )
                              ],
                            ),
                            child: const Icon(Icons.check_rounded,
                                size: 22, color: Colors.white),
                          ),
                        ],
                      ),
                    ),
                  ),
                  const SizedBox(height: kGap),
                  RiseIn.small(
                    delay: const Duration(milliseconds: 120),
                    child: Text('تمّت الحوالة بنجاح',
                        textAlign: TextAlign.center, style: T.titleSm),
                  ),
                  const SizedBox(height: kGap),
                  RiseIn.small(
                    delay: const Duration(milliseconds: 200),
                    child: RepaintBoundary(
                      key: receiptKey,
                      child: _Invoice(
                        t: t,
                        senderName: user?.displayName ?? '',
                        currency: currency,
                        onCall: _call,
                        onCopy: _copyPhone,
                      ),
                    ),
                  ),
                  const SizedBox(height: kGap),
                  AddToFavoritesButton(
                    // الرمز الداخلي لا رمز الموبايل: المفضّلة تُربط بعمود Code.
                    code: t.code,
                    kind: FavoriteKind.internal,
                    name: t.receiverName,
                    phone: t.receiverPhone,
                  ),
                  const SizedBox(height: kGap),
                  // الأزرار الأربعة نفسها، في صفّين بدل عمودٍ واحد.
                  //
                  // أربعة أزرار بعرض الشاشة تأكل ~224 dp من 640، فتدفع
                  // الفاتورة خارج الشاشة. صفّان يردّانها إلى ~106 دون أن
                  // يسقط زرّ واحد — والترتيب محفوظ: الأول يمين كل صفّ.
                  Row(
                    children: [
                      Expanded(
                        child: PrimaryButton(
                          height: 48,
                          label: 'مشاركة الفاتورة',
                          loading: receiptBusy,
                          icon: const Icon(Icons.share_rounded,
                              size: 18, color: Colors.white),
                          onPressed: receiptBusy ? null : _share,
                        ),
                      ),
                      const SizedBox(width: kGap),
                      Expanded(
                        child: GlassButton(
                          height: 48,
                          label: 'طباعة',
                          onPressed: receiptBusy ? null : _print,
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: kGap),
                  Row(
                    children: [
                      Expanded(
                        child: PrimaryButton(
                          height: 48,
                          label: 'حوالة جديدة',
                          onPressed: receiptBusy ? null : _newTransfer,
                        ),
                      ),
                      const SizedBox(width: kGap),
                      Expanded(
                        child: GlassButton(
                          height: 48,
                          label: 'العودة إلى الرئيسية',
                          onPressed: receiptBusy ? null : () => context.go('/'),
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

/// فاتورة الحوالة المُنشأة — بنفس صفوف فاتورة التسليم وترتيبها.
///
/// لا صفَّ للعمولة: أخفاها المالك في فاتورة التسليم لأنها شأن الوكيل لا
/// شأن من تُسلَّم إليه الورقة، والقاعدة نفسها تسري هنا.
class _Invoice extends StatelessWidget {
  const _Invoice({
    required this.t,
    required this.senderName,
    required this.currency,
    required this.onCall,
    required this.onCopy,
  });

  final CreatedTransfer t;
  final String senderName;
  final String currency;
  final VoidCallback onCall;
  final VoidCallback onCopy;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(14, 8, 14, 8),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(R.rCardXl),
          border: Border.all(color: R.inkA(.06)),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            const ReceiptHeader(),
            const SizedBox(height: kGap),
            // وقت الخادم بالمباعدة نفسها المستعملة في فاتورة التسليم.
            ReceiptRow('تاريخ التحويل',
                Fmt.stamp(t.insertedAt, separator: '    '),
                ltr: true),
            if (senderName.isNotEmpty)
              ReceiptRow('اسم المرسل', senderName, strong: true),
            if (t.cityName.isNotEmpty)
              ReceiptRow('المدينة المحوَّل لها', t.cityName),
            const SizedBox(height: kGap),
            BoxedField(
              icon: Icons.person_outline_rounded,
              label: 'اسم المستلم',
              child: Text(t.receiverName,
                  textAlign: TextAlign.start,
                  style: T.kufi(17, FontWeight.w700)),
            ),
            const SizedBox(height: kGap),
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
                      label: 'نسخ', icon: Icons.copy_rounded, onTap: onCopy),
                  const SizedBox(width: 8),
                  MiniButton(
                      label: 'اتصال',
                      icon: Icons.call_rounded,
                      filled: true,
                      onTap: onCall),
                ],
              ),
            ),
            const SizedBox(height: kGap),
            Divider(color: R.inkA(.07), height: 1),
            const SizedBox(height: kGap),
            ReceiptRow('قيمة الحوالة', Fmt.money(t.amount),
                ltr: true, strong: true, currency: currency),
          ],
        ),
      );
}
