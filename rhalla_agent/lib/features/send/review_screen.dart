import 'dart:async';
import 'dart:math';

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
import '../employee_app/employee_session.dart';
import '../shell/auto_refresh.dart';
import 'limit_dialog.dart';
import 'send_layout.dart';
import 'pending_approval_sheet.dart';
import 'send_repository.dart';
import 'transfer_otp.dart';
import 'transfer_summary.dart';

/// شاشة تأكيد الحوالة الداخلية — تعرض ما سيُخصم، ثم تطلب رمز تحقّق.
///
/// الرمز يُرسَل إلى هاتف **الوكيل نفسه** عبر واتساب، ويتحقّق منه الخادم
/// (`device/otp/checkOtp`) قبل استدعاء `internal/exchange`.
///
/// ⚠ حدّ هذه الحماية: الخادم لا يربط الرمز بالحوالة — نقطة الإنشاء لا تطلب
/// رمزاً أصلاً. فهي تحمي من عبثٍ بهاتفٍ مفتوح، لا من تطبيقٍ معدَّل. جعلها
/// إلزامية على الخادم تتطلّب تعديلاً في الواجهة الخلفية.
class ReviewTransferScreen extends ConsumerStatefulWidget {
  const ReviewTransferScreen({super.key, required this.draft});

  final TransferDraft draft;

  @override
  ConsumerState<ReviewTransferScreen> createState() =>
      _ReviewTransferScreenState();
}

class _ReviewTransferScreenState extends ConsumerState<ReviewTransferScreen> {
  /*
   * ⚠ بطاقةُ الرمز ومنطقُه خرجا إلى [TransferOtpPanel] — أمرُ المالك
   * (11 سبتمبر 2026) بتوحيد المراسم على القناتين والبابين.
   *
   * أربعُ حالاتٍ تحتاجها: الوكيلُ داخلياً وخارجياً، والموظفُ داخلياً
   * وخارجياً. ونسخةٌ لكلّ واحدةٍ منها تعني أربعَ بطاقاتٍ تفترق عند أوّل
   * تعديلٍ في المهلة أو الطول أو نصّ الخطأ.
   *
   * وما بقي في هذه الشاشة هو **إنشاءُ الحوالة** وحدَه — لأنّ الداخلية
   * والخارجية تُنشآن بنقطتين مختلفتين. فما وُحِّد هو المراسم، لا المال.
   */
  final _otp = TransferOtpController();

  /// مفتاحُ هذه المحاولة — ثابتٌ ما دامت الشّاشة قائمة.
  ///
  /// ⚠ في الحقل لا في النّداء: مفتاحٌ يُولّد عند كلّ
  /// إرسال يجعل كلّ إعادةٍ طلباً جديداً — وهو عينُ ما
  /// وُضع ليمنعَه.
  final String _clientId = 'tx-${DateTime.now().microsecondsSinceEpoch}'
      '-${Random().nextInt(0x7fffffff).toRadixString(36)}';

  bool _sending = false;

  /// وقت فتح الشاشة — ثابتٌ لا يقفز مع كل إعادة بناء.
  final String _stamp = Fmt.nowStamp();

  @override
  void initState() {
    super.initState();
    // جاهزيةُ الرمز وانشغالُه يحكمان زرَّ الأسفل — فيُعاد البناء عند تغيّرهما.
    _otp.addListener(_onOtp);
  }

  void _onOtp() {
    if (mounted) setState(() {});
  }

  @override
  void dispose() {
    _otp.removeListener(_onOtp);
    _otp.dispose();
    super.dispose();
  }

  /*
   * إنشاءُ الحوالة — **بعد** أن يقبل الخادمُ الرمز.
   *
   * ⚠ لا تحقّقَ هنا: [TransferOtpPanel] تتولّاه ثمّ تنادي هذه. والرمزُ
   * استُهلك على الخادم لحظةَ قبوله، فأيُّ فشلٍ بعد هذا السطر يستلزم رمزاً
   * جديداً — وهو ما تقوله البطاقةُ بنفسها («استُهلك الرمز»).
   */
  Future<void> _createTransfer() async {
    if (_sending) return;
    final user = ref.read(authControllerProvider).user;

    setState(() => _sending = true);

    try {
      // ⚠ في وضع الموظف يُملأ `AccID` في الخادم من حساب وكيله ويُدهَس
      // ما يُرسَل — فلا يستطيع الموظف تسمية حسابٍ آخر مهما فعل.
      final created = await ref.read(sendRepositoryProvider).createInternal(
            d: widget.draft,
            accId: user?.accId ?? 0,
            // ⚠ يُولّد مرّةً في `initState` لا هنا: مفتاحٌ جديد
            // مع كلّ محاولةٍ يُبطل الحمايةَ من أصلِها.
            clientId: _clientId,
          );
      if (!mounted) return;
      // الرصيد والعمليات تغيّرا على الخادم.
      refreshAfterMoneyAction(ref);
      context.pushReplacement('/send/internal/done', extra: created);
    } on TransferPendingApproval catch (p) {
      /*
       * ⚠ ولا شاشةَ «تمّت» هنا: الحوالة لم تُنفَّذ.
       *
       * تُعرض ورقةٌ تقول ما جرى وما ينتظره الموظف، ثمّ يعود إلى
       * شاشته — والطلبُ محفوظٌ عند وكيله لا يحتاج إعادةَ إدخال.
       */
      if (!mounted) return;
      setState(() => _sending = false);
      await showModalBottomSheet<void>(
        context: context,
        useRootNavigator: true,
        isDismissible: false,
        enableDrag: false,
        backgroundColor: Colors.transparent,
        builder: (_) => PendingApprovalSheet(pending: p),
      );
      /*
       * ⚠ ووجهةُ العودة بحسب من يستعمل الشاشة.
       *
       * الشاشةُ مشتركةٌ بين الوكيل والموظف عمداً (أمرُ المالك: «واجهةٌ من
       * وكيل»)، لكنّ `/` مسارُ الوكيل وحدَه. وإرسالُ الموظف إليه يجعله
       * يمرّ بإعادة توجيهٍ في الراوتر قبل أن يستقرّ — أو يقف حيث لا شاشة
       * له. فالوجهةُ تُحسب من الوضع لا تُفترض.
       */
      if (!mounted) return;
      final asEmployee = ref.read(employeeAuthProvider).status ==
          EmpSessionStatus.signedIn;
      context.go(asEmployee ? '/employee/home' : '/');
    } on ApiFailure catch (e) {
      if (!mounted) return;
      // ⚠ الرمزُ استُهلك على الخادم قبل هذا النداء، والبطاقةُ تقول ذلك
      // بنفسها («استُهلك الرمز — اطلب رمزاً جديداً»). فلا يُمسح حقلٌ هنا
      // ولا يُعاد ضبطُ مؤقّت: حالةُ الرمز صارت ملكَ البطاقة وحدَها.
      setState(() => _sending = false);

      final short = InsufficientFunds.from(e);
      if (short != null) {
        await showModalBottomSheet(
          context: context,
          isScrollControlled: true,
          backgroundColor: Colors.transparent,
          builder: (_) => _InsufficientSheet(
            data: short,
            // ⚠ في وضع الموظف لا مستخدمَ وكيلٍ في الجلسة — والرمزُ الافتراضيّ
            // هو الذي تعرضه بقيّةُ الشاشة أصلاً، فلا يفترق سطرٌ عن سطر.
            currency: user?.currencyCode ?? 'د.ل',
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

  bool get _busy => _sending || _otp.busy;

  /// وضعُ الموظف — يغيّر خطوةَ التأكيد وحدها، لا شيئاً في الحوالة.
  bool get _asEmployee =>
      ref.read(employeeAuthProvider).status == EmpSessionStatus.signedIn;

  /*
   * ⚠⚠ الرمزُ شرطٌ على الاثنين — أمرُ المالك (11 سبتمبر 2026).
   *
   * «حوالةٌ داخلية أو خارجية مُنفَّذة من تطبيق الموظف يُرسَل رمزُ التحقق
   * على رقم الواتس المرخَّص به الوكيلُ لذلك الموظف — لأنّ كلّ موظفٍ له
   * رقمٌ مخصَّصٌ معتمدٌ من الوكيل، وإلّا كيف فتح التطبيق أصلاً؟ …
   * **الموظفُ لا ينتظر الوكيل**».
   *
   * ── وما كان قبله، ولماذا سقط ─────────────────────────────────────
   *
   * كان الموظف مُستثنى، وسببُه أنّ الرمز يُرسَل إلى **هاتف الوكيل** —
   * حاضرٌ حين يرسل الوكيل بنفسه، غائبٌ حين يقف الموظف خلف الشبّاك. فكان
   * اشتراطُه يعني حوالةً لا تُنفَّذ حتى يردّ الوكيل على هاتفه.
   *
   * وقال التوثيقُ يومَها بالحرف: «ولو أُريد للموظف رمزٌ بالقوّة نفسِها
   * فمحلُّه **هاتفُ الموظف** لا هاتفُ الوكيل — وهاتفُه موثَّقٌ عند
   * التفعيل، فالبنيةُ قائمة». وهو ما نُفِّذ: `device/employee/otp/send`
   * تقرأ رقمَه من **جلسته** لا من الطلب.
   *
   * فالسياسةُ واحدةٌ الآن، والحمايةُ حقيقيةٌ لا صورية: الرمزُ يصل إلى من
   * يقف أمام الزبون، لا إلى هاتفٍ في مكتبٍ آخر.
   */
  // الجاهزيةُ تقولها البطاقة: هي التي تعرف طولَ الرمز واستهلاكَه.
  bool get _ready => !_sending && _otp.ready;

  @override
  Widget build(BuildContext context) {
    final d = widget.draft;
    final user = ref.watch(authControllerProvider).user;
    final currency = user?.currencyCode ?? 'د.ل';

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'حوالة محلية',
            // ⚠ الرمزُ صار شرطاً على الاثنين، فالسطرُ واحد.
            subtitle: 'راجع البيانات ثم أدخل رمز التحقّق',
            onBack: _sending ? null : () => context.pop(),
          ),
          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 8, R.padScreen, 4),
              children: [
                RiseIn.small(
                  delay: const Duration(milliseconds: 80),
                  child: GlassCard(
                    padding: kCardPad,
                    child: Column(
                      children: [
                        // ⚠ في وضع الموظف لا مستخدمَ وكيلٍ في الجلسة،
                        // والحوالة تخرج باسم الوكيل — فيُقال ذلك صراحةً
                        // بدل شرطةٍ لا تعني شيئاً.
                        KvRow('من حساب',
                            user?.displayName ??
                                (_asEmployee ? 'حساب الوكيل' : '—')),
                        const SizedBox(height: kGapRow),
                        KvRow('إلى المستلم', d.receiverName),
                        const SizedBox(height: kGapRow),
                        PhoneRow(d.receiverPhone),
                        const SizedBox(height: kGapRow),
                        KvRow('مدينة الاستلام', d.city.name),
                        const SizedBox(height: kGapRow),
                        KvRow('التاريخ', _stamp, numeric: true),
                      ],
                    ),
                  ),
                ),
                const SizedBox(height: kGap),
                RiseIn.small(
                  delay: const Duration(milliseconds: 140),
                  child: TotalsBox(
                      amount: d.amount,
                      commission: d.commission,
                      currency: currency),
                ),
                if (d.notes != null && d.notes!.trim().isNotEmpty) ...[
                  const SizedBox(height: kGap),
                  GlassCard(
                    padding: kCardPad,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('ملاحظات', style: T.label),
                        const SizedBox(height: 4),
                        Text(d.notes!.trim(),
                            style: T.plex(14, FontWeight.w500, height: 1.7)),
                      ],
                    ),
                  ),
                ],
                const SizedBox(height: kGap),
                RiseIn.small(
                  delay: const Duration(milliseconds: 200),
                  /*
                   * ⚠ بطاقةُ الرمز للاثنين — أمرُ المالك (11 سبتمبر 2026).
                   *
                   * والهاتفُ المعروض هاتفُ صاحب الجلسة: رقمُ الوكيل عنده،
                   * ورقمُ الموظف عنده. وعرضُ رقم الوكيل لموظفٍ ينتظر رمزاً
                   * على هاتفه هو كان سيجعله ينتظر رسالةً لن تصله.
                   */
                  // ⚠ البطاقةُ المشتركة — تتولّى الطلبَ والمؤقّتَ والحقلَ
                  // والتحقّق، ثمّ تنادي الإنشاء. والهاتفُ المعروض تقرؤه من
                  // الجلسة بنفسها، فلا يُمرَّر من هنا.
                  child: TransferOtpPanel(
                    controller: _otp,
                    onVerified: _createTransfer,
                  ),
                ),

                /*
                 * ⚠ وسطرُ النسبة يبقى للموظف — **لم يُحذف بحذف بطاقته**.
                 *
                 * كان نصَّ البطاقة التي حلّت محلَّها بطاقةُ الرمز، وهو يقول
                 * ما يحدث فعلاً: الحوالة تخرج باسم الوكيل وتُنسَب إلى الموظف
                 * باسمه. وموظّفٌ لا يعرف أن العملية تُنسَب إليه يتصرّف كأنها
                 * بلا أثر. فنُقل فوق الرمز بدل أن يسقط معها.
                 */
                if (_asEmployee) ...[
                  const SizedBox(height: kGap),
                  RiseIn.small(
                    delay: const Duration(milliseconds: 240),
                    child: _attributionNote(),
                  ),
                ],
                const SizedBox(height: kGap),
                const WarnBanner(
                  text:
                      'بعد الإرسال لا يمكن تعديل الحوالة — إلغاؤها يتطلّب مراجعة الفرع.',
                ),
              ],
            ),
          ),
          Container(
            padding:
                const EdgeInsets.fromLTRB(R.padScreen, 6, R.padScreen, 8),
            decoration: BoxDecoration(
              gradient: LinearGradient(
                begin: Alignment.topCenter,
                end: Alignment.bottomCenter,
                colors: [Color(0x00F1F8F5), Color(0xF0F1F8F5), R.scrimBottom],
                stops: [0, .34, 1],
              ),
            ),
            // الزرّان في صفٍّ واحد لا فوق بعضهما.
            //
            // إعادة ترتيب لا حذف: الشاشة كانت تفيض عن هاتف 360×640، و«تعديل
            // البيانات» تحت الزرّ الرئيسي كان يأخذ 48 نقطة من ارتفاعٍ تحتاجه
            // البيانات. الكلمتان باقيتان، ومساحة اللمس باقية (44 نقطة).
            child: Row(
              children: [
                TextButton(
                  onPressed: _busy ? null : () => context.pop(),
                  style: TextButton.styleFrom(
                    minimumSize: const Size(44, kButtonHeight),
                    padding: const EdgeInsets.symmetric(horizontal: 12),
                  ),
                  child: Text('تعديل البيانات',
                      style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
                ),
                const SizedBox(width: 10),
                Expanded(
                  child: PrimaryButton(
                    height: kButtonHeight,
                    label: 'تأكيد وإرسال',
                    loading: _busy,
                    onPressed: _ready ? _otp.submit : null,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }


  /// نسبةُ العملية — يُعرض للموظف مع بطاقة الرمز لا بدلاً منها.
  ///
  /// ⚠ يقول ما يحدث فعلاً: الحوالة تخرج **باسم الوكيل**، وتُنسَب إلى
  /// الموظف باسمه. وموظّفٌ لا يعرف أن العملية تُنسَب إليه يتصرّف كأنها بلا
  /// أثر.
  Widget _attributionNote() => GlassCard(
        padding: kCardPad,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(Icons.verified_user_outlined,
                    size: 18, color: R.primary),
                const SizedBox(width: 8),
                Text('نسبةُ العملية', style: T.label),
              ],
            ),
            const SizedBox(height: 8),
            Text(
              'الحوالة تخرج باسم الوكيل، وتُسجَّل هذه العملية باسمك '
              'وبنقطة بيعك. راجع المبلغ واسم المستفيد قبل التأكيد.',
              style: T.plex(12.5, FontWeight.w400,
                  color: R.inkA(.62), height: 1.8),
            ),
          ],
        ),
      );
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
                      const SizedBox(height: 4),
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

