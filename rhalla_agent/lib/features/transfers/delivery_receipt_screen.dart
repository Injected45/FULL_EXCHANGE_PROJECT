
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
import '../../core/net/api_envelope.dart';
import 'agent_incoming_repository.dart';
import 'receipt.dart';
import 'delivery_done_screen.dart';
import 'transfers_repository.dart';

/// فاتورة حوالة داخلية — تُفتح بالضغط على حوالة في «سلَّمتُها».
///
/// **الطباعة والمشاركة تصوّران الفاتورة صورةً لا نصّاً.** السبب عربي بحت:
/// توليد PDF نصّي يتطلّب تشكيل الحروف العربية ووصلها داخل مكتبة الـ PDF،
/// وهو مصدر أعطال معروف (حروف منفصلة أو معكوسة). أما التصوير فيلتقط ما
/// يراه الوكيل حرفياً بخطّ التطبيق نفسه — فما يُطبع هو ما يُرى.
class DeliveryReceiptScreen extends ConsumerStatefulWidget {
  const DeliveryReceiptScreen({
    super.key,
    required this.transfer,
    this.commission,
  });

  final AgentIncomingTransfer transfer;

  /// عمولة معروفة من سياق آخر — تُمرَّر من «آخر العمليات» حيث تُجمع العمولة
  /// من صفوف الحركة. وحين تكون null تُؤخذ عمولة الدفتر.
  final double? commission;

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
    // نفس الفاتورة في كل سياق — والفرق زرٌّ واحد لمن لم يُسجَّل بعد.
    //
    // الحالة وحدها تحكم، لا الشاشة التي جاء منها الوكيل (قرار المالك،
    // 3 سبتمبر 2026): حوالةٌ «بانتظار التسليم» تُسلَّم من «آخر العمليات» كما
    // تُسلَّم من «الحوالات الواردة»، وحوالةٌ «تم التسليم» تُعرض ولا زرّ لها
    // في الحالتين. مصدرٌ واحد للقرار لا مصدران يمكن أن يختلفا.
    //
    // والحالة من الخادم لا من دفتر الجهاز: هي ما يبقى بعد حذف التطبيق.
    //
    // والملغاة لا زرّ لها كذلك (قرار المالك، 4 سبتمبر 2026): الرحالة ألغت
    // الحوالة، فلا تسليم يُسجَّل عليها. وكان الخادم يرفضها بـ 409 وحده —
    // حارسٌ سليم لكنه يأتي **بعد** أن يضغط الوكيل ويؤكّد، فيقرأ الرفض عطباً.
    // وحاوية «سبب الإلغاء» تحت الفاتورة تقول له لماذا.
    final canDeliver = !t.isDelivered && !t.isCancelled;

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'فاتورة حوالة محلية واردة',
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
                      commission: widget.commission ?? t.commission,
                      onCall: _call,
                      onCopy: _copyPhone,
                      cancelled: t.isCancelled,
                      // حالة الواردة من **دفتر تسليم الوكيل** لا من المنظومة
                      // (قرار المالك، 2 سبتمبر 2026): «هل دفعتُ للمستفيد؟»
                      // لا «أين الحوالة بيني وبين الرحالة؟». والملغاة تعلو
                      // على المسلَّمة كما في القائمة.
                      statusLabel: t.isCancelled
                          ? 'ملغاة'
                          : t.isDelivered
                              ? 'تم التسليم'
                              : 'بانتظار التسليم',
                      statusColor: t.isCancelled
                          ? R.error
                          : t.isDelivered
                              ? R.primaryGradEnd
                              : R.warnIcon,
                      notes: t.notes),
                ),

                // «سبب الإلغاء» — للملغاة وحدها، وخارج `RepaintBoundary`
                // عمداً: الفاتورة تُصوَّر للطباعة والمشاركة، وهي ورقة العميل.
                // وسببُ الإلغاء شأنٌ بين الرحالة والوكيل لا يُسلَّم للعميل.
                if (t.isCancelled) ...[
                  const SizedBox(height: 16),
                  CancelReasonBox(reason: t.cancelReason, notes: t.cancelNotes),
                ],

                const SizedBox(height: 20),
                if (canDeliver) ...[
                  PrimaryButton(
                    label: 'تسجيل التسليم',
                    icon: const Icon(Icons.check_rounded,
                        size: 18, color: Colors.white),
                    onPressed: (receiptBusy || _sending) ? null : _markDelivered,
                    loading: _sending,
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
  /// لا قبلها.
  ///
  /// النقل إلى «تم التسليم» لا يقع في الواجهة: يُرسَل الطلب، وإن أكّده
  /// الخادم أُبطلت الذاكرة المؤقّتة فتُعاد القراءة من قاعدته. وإن انقطعت
  /// الشبكة تبقى الحوالة حيث هي ويرى الوكيل أن التسجيل لم يكتمل — لا حالةٌ
  /// «مسلَّمة» في الهاتف وحده لا يعرفها الخادم.
  bool _sending = false;

  Future<void> _markDelivered() async {
    if (_sending) return;

    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _ConfirmDeliver(name: widget.transfer.receiverName),
    );
    if (ok != true || !mounted) return;

    // قفلٌ في الواجهة فوق تحايُد الخادم: ضغطتان سريعتان لا ترسلان طلبين.
    setState(() => _sending = true);
    try {
      await ref
          .read(agentIncomingRepositoryProvider)
          .deliver(widget.transfer.id);
      if (!mounted) return;
      ref.invalidate(agentIncomingProvider);

      // شاشة تأكيد بدل إغلاقٍ صامت — تحلّ محلّ الفاتورة لا تعلوها، وإلا
      // عادت بضغطة رجوع واحدة بزرّ «تسجيل التسليم» لحوالةٍ سُلّمت.
      await Navigator.of(context).pushReplacement(
        MaterialPageRoute(
          builder: (_) => DeliveryDoneScreen(
            transfer: widget.transfer,
            currency: ref.read(authControllerProvider).user?.currencyCode ??
                'د.ل',
          ),
        ),
      );
    } on ApiFailure catch (e) {
      if (!mounted) return;
      setState(() => _sending = false);
      receiptToast(e.message);
    } catch (_) {
      if (!mounted) return;
      setState(() => _sending = false);
      receiptToast('تعذّر تسجيل التسليم — تحقّق من الاتصال وأعد المحاولة.');
    }
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
class TransferInvoice extends StatelessWidget {
  const TransferInvoice({
    super.key,
    required this.t,
    required this.currency,
    required this.commission,
    required this.onCall,
    required this.onCopy,
    this.cancelled = false,
    this.showSender = true,
    this.statusLabel,
    this.statusColor,
    this.notes = '',
  });

  final IncomingTransfer t;
  final String currency;

  /// عمولة هذه الحوالة — من الخادم لا محسوبةً هنا (بند 15).
  final double commission;
  final VoidCallback onCall;
  final VoidCallback onCopy;

  /// ألغتها الرحالة — فتُصبَغ الورقة نفسها بحمرةٍ خفيفة (قرار المالك،
  /// 3 سبتمبر 2026): يعرف الوكيل من نظرةٍ واحدة أن ما بين يديه ملغى، قبل
  /// أن يقرأ حرفاً.
  final bool cancelled;

  /// اسم المرسل — يُخفى في الفاتورة الصادرة (قرار المالك، 4 سبتمبر 2026).
  ///
  /// المرسل فيها هو حساب الوكيل نفسه («جاري شركة …»)، وذِكرُه له في فاتورته
  /// حشوٌ يزاحم ما يعنيه: المستفيد والمدينة والمبلغ.
  final bool showSender;

  /// حالة الحوالة ولونها — تُمرَّر من الشاشة، وتُخفى إن كانت فارغة.
  final String? statusLabel;
  final Color? statusColor;

  /// ملاحظة كُتبت على الحوالة في منظومة الرحالة عند إنشائها.
  ///
  /// تُعرض في أسفل الفاتورة، **وإن كانت فارغة فلا يظهر شيء** — لا عنوان
  /// ولا حاوية ولا فراغ: أغلب الحوالات بلا ملاحظة، وحقلٌ فارغ في كل فاتورة
  /// يوحي بأن نصّاً لم يصل.
  final String notes;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(18, 20, 18, 20),
        decoration: BoxDecoration(
          // حمرةٌ بين البياض والوردي الخفيف — لا لونٌ واحد مسطّح.
          //
          // ثلاث محطّات متقاربة جداً في تدرّج مائل تعطي أثر البلّور: الضوء
          // يبدو منزلقاً على الورقة بدل أن تكون مصبوغة. والفرق بين أطرافها
          // أقلّ من 3%، فلا تُقرأ «بطاقة حمراء» بل ورقةٌ عليها مسحة.
          //
          // ⚠ ومصمَتة لا شفّافة رغم مظهرها: الفاتورة **تُصوَّر** للطباعة
          // والمشاركة، وشفافيةٌ هنا تلتقط ما خلفها فتخرج على الورق بخلفية
          // متّسخة. الشفافية في اللون نفسه لا في الطبقة.
          color: cancelled ? null : Colors.white,
          gradient: cancelled
              ? const LinearGradient(
                  begin: Alignment.topRight,
                  end: Alignment.bottomLeft,
                  colors: [
                    Color(0xFFFFF7F7),
                    Color(0xFFFDEEEE),
                    Color(0xFFFFF9F9),
                  ],
                  stops: [0, .55, 1],
                )
              : null,
          borderRadius: BorderRadius.circular(R.rCardXl),
          border: Border.all(
            color: cancelled ? R.error.withValues(alpha: .16) : R.inkA(.06),
          ),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            const ReceiptHeader(),
            const SizedBox(height: 20),
            ReceiptRow('تاريخ ووقت التحويل',
                Fmt.stamp(t.insertedAt, separator: '  '), ltr: true),
            ReceiptRow('رقم الكود', t.code, ltr: true, strong: true),
            if (showSender) ReceiptRow('اسم المرسل', t.senderName, strong: true),
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
            // قيمة الحوالة وحدها — والعمولة والإجمالي لا يُعرضان في الفاتورة
            // (أمر المالك، 3 سبتمبر 2026).
            //
            // الفاتورة ورقةٌ تُسلَّم للعميل، والعميل يسأل عن قيمة حوالته لا
            // عن عمولة الوكيل. والعمولة لم تُحذف من المنظومة: تبقى قيداً
            // مستقلاً في كشف الحساب، وتظهر سطراً داخل بطاقة حوالتها في
            // «آخر العمليات» — للوكيل وحده، لا في ورقة العميل.
            ReceiptRow('قيمة الحوالة', Fmt.money(t.amount),
                ltr: true, strong: true, currency: currency),

            // حالة الحوالة تحت قيمتها مباشرةً (قرار المالك، 4 سبتمبر 2026):
            // من يفتح الفاتورة يرى الحوالة وحالتها في نظرة واحدة، فلا يحتاج
            // أن يرجع إلى القائمة ليعرف أين وصلت.
            //
            // والنصّ يُمرَّر من الشاشة لا يُشتقّ هنا: الواردة حالتُها من دفتر
            // تسليم الوكيل، والصادرة من `InternalEx_Stautes` — وهما قاعدتان
            // مختلفتان قرّرهما المالك، فلا تُخلطان في مكانٍ واحد.
            if (statusLabel != null && statusLabel!.isNotEmpty) ...[
              const SizedBox(height: 10),
              Row(
                children: [
                  Text('حالة الحوالة',
                      style:
                          T.plex(12, FontWeight.w400, color: R.inkA(.55))),
                  const SizedBox(width: 10),
                  Expanded(
                    child: Align(
                      alignment: AlignmentDirectional.centerEnd,
                      child: Container(
                        padding: const EdgeInsets.symmetric(
                            horizontal: 12, vertical: 6),
                        decoration: BoxDecoration(
                          color: (statusColor ?? R.primary)
                              .withValues(alpha: .10),
                          border: Border.all(
                              color: (statusColor ?? R.primary)
                                  .withValues(alpha: .32)),
                          borderRadius: BorderRadius.circular(99),
                        ),
                        child: Text(
                          statusLabel!,
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style: T.kufi(12.5, FontWeight.w700,
                              color: statusColor ?? R.primaryDark),
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ],

            // ملاحظة منشئ الحوالة — أسفل الفاتورة وآخر ما فيها.
            //
            // موضعها بعد المبلغ والحالة عمداً: هي شرحٌ لما فوقها لا معلومةٌ
            // قائمة بذاتها، ووضعها بين البيانات كان يزاحم ما يُقرأ أولاً.
            //
            // وهي **داخل** الفاتورة لا خارجها — بخلاف «سبب الإلغاء» الذي
            // يقف خارج `RepaintBoundary`. الفرق أن سبب الإلغاء شأنٌ بين
            // الرحالة والوكيل، أما هذه فكُتبت على الحوالة نفسها لتُقرأ معها،
            // فتظهر في الورقة المطبوعة والمشاركة كما تظهر على الشاشة.
            if (notes.isNotEmpty) ...[
              const SizedBox(height: 14),
              BoxedField(
                icon: Icons.sticky_note_2_outlined,
                label: 'ملاحظة',
                child: Text(notes,
                    textAlign: TextAlign.start,
                    style: T.kufi(14, FontWeight.w500, height: 1.55)),
              ),
            ],
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
            // نصّ المالك حرفياً (3 سبتمبر 2026): جملةٌ واحدة تقول للوكيل ما
            // عليه فعله قبل الضغط. النصّ السابق كان يشرح **آلية** التسجيل
            // (دفتر الوكيل، لا رجعة فيه) — وهي معلومةٌ للمبرمج لا للواقف
            // أمام مستفيد. والقيد نفسه ما زال مفروضاً في الخادم.
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

/// حاوية «سبب الإلغاء» — تعرض ما كُتب في منظومة الرحالة، لا أكثر.
///
/// قرار المالك (3 سبتمبر 2026): الوكيل يفتح الحوالة الملغاة فيقرأ سببها،
/// بدل أن يتّصل بالفرع ليسأل.
///
/// **ولا يُصاغ النصّ هنا ولا يُترجَم ولا يُختصَر**: يُعرض حرفياً كما أُدخل
/// هناك. صياغةٌ في التطبيق تعني نسختين من السبب تفترقان، والوكيل يحتجّ
/// بالنسخة التي قرأها.
class CancelReasonBox extends StatelessWidget {
  const CancelReasonBox({
    super.key,
    required this.reason,
    required this.notes,
  });

  /// المبرّر المختار من قائمة المنظومة، والملاحظة الحرّة بجانبه.
  final String reason;
  final String notes;

  bool get _has => reason.isNotEmpty || notes.isNotEmpty;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(16, 14, 16, 15),
        decoration: BoxDecoration(
          color: R.error.withValues(alpha: .06),
          border: Border.all(color: R.error.withValues(alpha: .22)),
          borderRadius: BorderRadius.circular(R.rCard),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Row(
              children: [
                Icon(Icons.block_rounded, size: 17, color: R.error),
                const SizedBox(width: 9),
                Text('سبب الإلغاء',
                    style: T.kufi(13.5, FontWeight.w700, color: R.error)),
              ],
            ),
            const SizedBox(height: 10),
            if (_has) ...[
              if (reason.isNotEmpty)
                Text(reason,
                    style: T.plex(13, FontWeight.w600, height: 1.7)),
              // الملاحظة الحرّة تحت السبب المختار — وتُعرض وحدها إن غاب،
              // فقد يُكتفى بها في المنظومة.
              if (notes.isNotEmpty) ...[
                if (reason.isNotEmpty) const SizedBox(height: 8),
                Text(notes,
                    style: T.plex(12.5, FontWeight.w400,
                        color: R.inkA(.62), height: 1.7)),
              ],
            ] else
              // فراغٌ صريح لا حاويةٌ خالية: الوكيل يعرف أن السبب غير مسجَّل
              // في المنظومة، فلا يظنّ التطبيق عجز عن جلبه.
              Text('لم يُسجَّل سبب في منظومة الرحالة لهذا الإلغاء.',
                  style: T.plex(12.5, FontWeight.w400,
                      color: R.inkA(.5), height: 1.7)),
          ],
        ),
      );
}
