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
import 'external_repository.dart';
import 'limit_dialog.dart';
import 'send_layout.dart';
import 'send_repository.dart';
import 'transfer_otp.dart';
import 'transfer_summary.dart';

/// شاشةُ مراجعة الحوالة **الخارجية** — تعرض ما سيُخصم ثمّ تطلب رمز تحقّق.
///
/// ══════════════════════════════════════════════════════════════════════════
///  أمرُ المالك (11 سبتمبر 2026)
/// ══════════════════════════════════════════════════════════════════════════
///
/// «ننشئ شاشةَ مراجعةٍ للحوالة الخارجية لنتمكّن من إرسال الرمز والتحقّق
///  **بنفس آليّة الداخلية** — بشرط عدم تعطيل أو التأثير على أيّ عملياتٍ ماليةٍ
///  أو محاسبية أو الشجرة المحاسبية أو المدين أو الدائن، ولا تقربَ أيَّ شيءٍ
///  في منظومة الرحالة».
///
/// ── والشرطُ محفوظٌ بالبناء لا بالوعد ────────────────────────────────────
///
/// ⚠ **هذه الشاشةُ لا تمسّ مالاً ولا قيداً.** هي بابٌ يقف **قبل** نداء
/// الإنشاء القائم (`ExternalRepository.create`) ولا يغيّر فيه معاملاً واحداً:
/// المبلغُ والعمولةُ والسعرُ والخدمةُ والوجهةُ كما خرجت من النموذج حرفاً.
/// الذي تغيّر **متى** يُنادى الإنشاء — بعد قبول الخادم للرمز — لا ما يفعله.
///
/// ولا استعلامَ جديداً ولا جدولَ ولا عمود: التسعيرةُ المعروضة هي التي حسبها
/// الخادمُ للنموذج قبل الدخول إلى هنا، تُمرَّر ولا يُعاد حسابُها — فورقةُ
/// الزبون لا تخالف ما سيُكتب في الدفتر.
///
/// ── وما تشترك فيه مع الداخلية، وما تفترق ───────────────────────────────
///
/// ⚠ **المراسمُ واحدة**: [TransferOtpPanel] نفسُها — الطلبُ والمؤقّتُ والحقلُ
/// والتحقّق. ونسخةٌ ثانية منها كانت ستفترق عن الأولى عند أوّل تعديل، فيرى
/// الوكيلُ في الخارجية شيئاً غيرَ ما رآه في الداخلية.
///
/// والمفترقُ **البياناتُ وحدَها**، لأنّ الحوالتين تختلفان فعلاً: الخارجيةُ
/// تعبر عملتين ووجهةً ونوعَ خدمة، فتُعرض ثلاثةُ أرقامٍ لا رقم — المقبوضُ
/// بالدينار، والسعرُ، وما يُستلم هناك.
class ReviewExternalScreen extends ConsumerStatefulWidget {
  const ReviewExternalScreen({super.key, required this.draft});

  final ExternalDraft draft;

  @override
  ConsumerState<ReviewExternalScreen> createState() =>
      _ReviewExternalScreenState();
}

class _ReviewExternalScreenState extends ConsumerState<ReviewExternalScreen> {
  final _otp = TransferOtpController();

  /// مفتاحُ هذه المحاولة — ثابتٌ ما دامت الشاشةُ قائمة.
  ///
  /// ⚠ في الحقل لا في النداء: مفتاحٌ يُولّد عند كلّ إرسالٍ يجعل كلَّ إعادةٍ
  /// طلباً جديداً — وهو عينُ ما وُضع ليمنعَه.
  final String _clientId = 'ex-${DateTime.now().microsecondsSinceEpoch}'
      '-${Random().nextInt(0x7fffffff).toRadixString(36)}';

  bool _sending = false;

  /// وقتُ فتح الشاشة — ثابتٌ لا يقفز مع كلّ إعادة بناء.
  final String _stamp = Fmt.nowStamp();

  @override
  void initState() {
    super.initState();
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

  bool get _busy => _sending || _otp.busy;
  bool get _ready => !_sending && _otp.ready;

  bool get _asEmployee =>
      ref.read(employeeAuthProvider).status == EmpSessionStatus.signedIn;

  /*
   * إنشاءُ الحوالة — **بعد** أن يقبل الخادمُ الرمز.
   *
   * ⚠ النداءُ هو النداءُ القائم بمعاملاته كما هي. لا حقلَ يُضاف ولا يُحذف،
   * ولا رقمَ يُحسب هنا.
   */
  Future<void> _create() async {
    if (_sending) return;

    final user = ref.read(authControllerProvider).user;
    final d = widget.draft;

    setState(() => _sending = true);

    try {
      /*
       * ⚠ `accId` يُدهَس في الخادم بحساب الجلسة قبل أن يُقرأ — في البابين.
       * فالقيمةُ هنا لا تقرّر شيئاً، وتُمرَّر كما كانت تُمرَّر من النموذج.
       */
      final row = await ref.read(externalRepositoryProvider).create(
            d: d,
            accId: user?.accId ?? 0,
            clientId: _clientId,
          );

      if (!mounted) return;
      refreshAfterMoneyAction(ref);

      // المحفّزُ يحسب `NetTotal` و`TransPrice` بعد الإدراج، والخادمُ يعيد
      // الصفَّ بعدها — فهذه أرقامُ ما كُتب فعلاً، لا تقديرُ العميل.
      context.pushReplacement(
        '/send/external/done',
        extra: ExternalDoneArgs(
          code: '${row['codeForMobile'] ?? row['Code'] ?? ''}',
          favoriteCode: '${row['Code'] ?? ''}'.trim(),
          name: d.receiverName,
          phone: d.receiverPhone,
          amount: d.amountLyd,
          commission: d.commission,
          net: Fmt.num_(row['NetTotal']),
          rate: Fmt.num_(row['TransPrice']),
          // ⚠ رمزُ العملة من التسعيرة: الصفُّ المُعاد يحمل معرّفَها لا رمزَها،
          // وترجمتُه هنا تعني استعلاماً ثانياً على شاشة نجاح.
          currencyCode: d.quote?.currencyCode ?? '',
          country: d.country.name,
          city: d.city.name,
          service: d.service.name,
        ),
      );
    } on ApiFailure catch (e) {
      if (!mounted) return;
      // ⚠ الرمزُ استُهلك على الخادم، والبطاقةُ تقول ذلك بنفسها. فلا تُمسّ
      // حالتُه من هنا.
      setState(() => _sending = false);

      // تجاوزُ السقف حدٌّ لا خطأ — حوارٌ في وسط الشاشة لا شريطٌ أحمر.
      final overLimit = TransferLimitExceeded.from(e);
      if (overLimit != null) {
        await showLimitExceededDialog(context, overLimit);
        return;
      }

      if (!mounted) return;
      _say(e.message);
    } catch (_) {
      /*
       * ⚠ خطأٌ غير متوقّع: يُفكّ التجميد ويُحذَّر من إعادة الإرسال قبل
       * التحقّق — فقد يكون المالُ خرج والفشلُ في قراءة الردّ فقط.
       */
      if (!mounted) return;
      setState(() => _sending = false);
      _say('تعذّر تأكيد نتيجة الحوالة. راجع قائمة الحوالات قبل إعادة المحاولة.');
    }
  }

  void _say(String m) => ScaffoldMessenger.of(context)
    ..hideCurrentSnackBar()
    ..showSnackBar(SnackBar(
      content: Text(m, style: T.plex(13, FontWeight.w500, color: Colors.white)),
      backgroundColor: R.error,
      behavior: SnackBarBehavior.floating,
      margin: const EdgeInsets.fromLTRB(16, 0, 16, 24),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
    ));

  @override
  Widget build(BuildContext context) {
    final d = widget.draft;
    final currency =
        ref.watch(authControllerProvider).user?.currencyCode ?? 'د.ل';

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'حوالة خارجية',
            subtitle: 'راجع البيانات ثم أدخل رمز التحقّق',
            onBack: _busy ? null : () => context.pop(),
          ),
          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 8, R.padScreen, 4),
              children: [
                RiseIn.small(child: _destinationCard(d)),
                const SizedBox(height: kGap),
                RiseIn.small(
                  delay: const Duration(milliseconds: 80),
                  child: _beneficiaryCard(d),
                ),
                const SizedBox(height: kGap),
                RiseIn.small(
                  delay: const Duration(milliseconds: 140),
                  child: _moneyCard(d, currency),
                ),
                const SizedBox(height: kGap),
                RiseIn.small(
                  delay: const Duration(milliseconds: 200),
                  child: TransferOtpPanel(
                    controller: _otp,
                    onVerified: _create,
                  ),
                ),

                // ⚠ سطرُ النسبة للموظف — كما في الداخلية حرفاً: من لا يعرف
                // أنّ العملية تُنسَب إليه يتصرّف كأنها بلا أثر.
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
            padding: const EdgeInsets.fromLTRB(R.padScreen, 6, R.padScreen, 8),
            decoration: BoxDecoration(
              gradient: LinearGradient(
                begin: Alignment.topCenter,
                end: Alignment.bottomCenter,
                colors: [Color(0x00F1F8F5), Color(0xF0F1F8F5), R.scrimBottom],
                stops: const [0, .34, 1],
              ),
            ),
            // الزرّان في صفٍّ واحد لا فوق بعضهما — القاعدةُ نفسُها في
            // الداخلية: الشاشةُ تفيض عن هاتف 360×640 لو تكدّسا.
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

  /* ───────────────── البطاقات ───────────────── */

  Widget _destinationCard(ExternalDraft d) => GlassCard(
        padding: kCardPad,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(Icons.public_rounded, size: 17, color: R.primary),
                const SizedBox(width: 8),
                Text('الوجهة', style: T.label),
                const Spacer(),
                Text(_stamp,
                    style: T.plex(11, FontWeight.w400, color: R.inkA(.45))),
              ],
            ),
            const SizedBox(height: kGapLabel),
            _row('الدولة', d.country.name),
            _row('المدينة', d.city.name),
            // ⚠ نوعُ الخدمة سطرٌ في الخارجية وحدَها: هو الذي يحدّد **كيف**
            // يستلم المستفيد — بريدٌ أو بنكٌ أو نقداً — والزبون يُسأل عنه.
            _row('نوع الخدمة', d.service.name),
          ],
        ),
      );

  Widget _beneficiaryCard(ExternalDraft d) => GlassCard(
        padding: kCardPad,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(Icons.person_outline_rounded, size: 17, color: R.primary),
                const SizedBox(width: 8),
                Text('المستفيد', style: T.label),
              ],
            ),
            const SizedBox(height: kGapLabel),
            _row('الاسم', d.receiverName),
            Padding(
              padding: const EdgeInsets.only(top: kGapRow),
              child: Row(
                children: [
                  Text('الهاتف',
                      style:
                          T.plex(12.5, FontWeight.w400, color: R.inkA(.58))),
                  const Spacer(),
                  Directionality(
                    // ⚠ رقمٌ أجنبيّ بلا بادئة ليبيا: المستفيدُ خارجَها.
                    textDirection: TextDirection.ltr,
                    child: Text(d.receiverPhone,
                        style: T.kufi(13.5, FontWeight.w700)),
                  ),
                ],
              ),
            ),
          ],
        ),
      );

  /// أرقامُ الحوالة — **كما حسبها الخادم**، لا يُضرب رقمٌ في رقمٍ هنا.
  ///
  /// ⚠ حسابٌ ثانٍ في الهاتف يفترق عن الأوّل عند أوّل كسر، ثمّ يحمل الزبونُ
  /// ورقةً تقول غيرَ ما في الدفتر.
  Widget _moneyCard(ExternalDraft d, String currency) {
    final q = d.quote;

    return GlassCard(
      padding: kCardPad,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(Icons.payments_outlined, size: 17, color: R.primary),
              const SizedBox(width: 8),
              Text('المبالغ', style: T.label),
            ],
          ),
          const SizedBox(height: kGapLabel),

          MoneyRow('المبلغ المقبوض', d.amountLyd, currency: currency),
          if (d.commission > 0)
            MoneyRow('العمولة', d.commission, currency: currency),
          const SizedBox(height: kGapRule),
          Divider(color: R.inkA(.08), height: 1),
          const SizedBox(height: kGapRule),
          MoneyRow('الإجمالي المخصوم', d.total,
              currency: currency, strong: true),

          if (q != null) ...[
            const SizedBox(height: kGapRule),
            Divider(color: R.inkA(.08), height: 1),
            const SizedBox(height: kGapRule),
            Row(
              children: [
                Text('سعر الصرف',
                    style: T.plex(12.5, FontWeight.w400, color: R.inkA(.58))),
                const Spacer(),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child:
                      Text(Fmt.rate(q.rate), style: T.kufi(13.5, FontWeight.w700)),
                ),
              ],
            ),
            const SizedBox(height: kGapRow),
            MoneyRow('يستلم المستفيد', q.net,
                currency: q.currencyCode, strong: true),
          ],
        ],
      ),
    );
  }

  Widget _row(String k, String v) => Padding(
        padding: const EdgeInsets.only(top: kGapRow),
        child: Row(
          children: [
            Text(k, style: T.plex(12.5, FontWeight.w400, color: R.inkA(.58))),
            const SizedBox(width: 12),
            Expanded(
              child: Text(v.isEmpty ? '—' : v,
                  textAlign: TextAlign.end,
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: T.kufi(13.5, FontWeight.w700)),
            ),
          ],
        ),
      );

  Widget _attributionNote() => GlassCard(
        padding: kCardPad,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(Icons.verified_user_outlined, size: 18, color: R.primary),
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
