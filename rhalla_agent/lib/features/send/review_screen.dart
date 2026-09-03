import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
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
import '../auth/auth_repository.dart';
import '../shell/auto_refresh.dart';
import 'limit_dialog.dart';
import 'send_layout.dart';
import 'send_repository.dart';
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
  /// أربع خانات: الخادم يولّد rand(1000, 9999)، و checkOtp يتحقق digits:4.
  static const _otpLength = 4;

  /// مهلة إعادة الإرسال. صلاحية الرمز نفسه ثلاث دقائق (ExpeaerTime).
  static const _resendAfter = 60;

  final _otpCtl = TextEditingController();
  final _otpFocus = FocusNode();

  String _code = '';
  String? _otpError;
  bool _requesting = false;
  bool _sending = false;

  /// صار الرمز مستهلَكاً: checkOtp يضع ISActive=1 عند أول مطابقة ناجحة،
  /// فإن فشل إنشاء الحوالة بعدها لا ينفع الرمز مرّةً ثانية — والوكيل يحتاج
  /// رمزاً جديداً لا محاولةً ثانية بالرمز ذاته.
  bool _spent = false;


  int _left = 0;
  Timer? _timer;

  /// وقت فتح الشاشة — ثابتٌ لا يقفز مع كل إعادة بناء.
  final String _stamp = Fmt.nowStamp();

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) => _requestOtp());
  }

  @override
  void dispose() {
    _timer?.cancel();
    _otpCtl.dispose();
    _otpFocus.dispose();
    super.dispose();
  }

  void _startTimer() {
    _timer?.cancel();
    setState(() => _left = _resendAfter);
    _timer = Timer.periodic(const Duration(seconds: 1), (t) {
      if (!mounted) return t.cancel();
      setState(() => _left--);
      if (_left <= 0) t.cancel();
    });
  }

  Future<void> _requestOtp() async {
    final user = ref.read(authControllerProvider).user;
    if (user == null || _requesting) return;

    setState(() {
      _requesting = true;
      _otpError = null;
    });

    try {
      await ref.read(authRepositoryProvider).requestOtp(user.phone);
      if (!mounted) return;
      setState(() {
        _requesting = false;
        _spent = false;
        _code = '';
        _otpCtl.clear();
      });
      _startTimer();
    } on ApiFailure catch (e) {
      if (!mounted) return;
      setState(() {
        _requesting = false;
        _otpError = e.message;
        _left = 0;
      });
    }
  }

  /// الموافقة الآلية عند اكتمال الرمز.
  ///
  /// المهلة القصيرة ليست تجميلاً: بدونها تتحرّك الشاشة قبل أن يرى الوكيل
  /// خانته الأخيرة تمتلئ، فلا يعرف أضغَط شيئاً أم أخطأ. ونعيد فحص الطول
  /// بعدها لأنه قد يكون حذف رقماً في أثنائها.
  Future<void> _autoConfirm() async {
    await Future.delayed(const Duration(milliseconds: 180));
    if (!mounted || _code.length != _otpLength) return;
    await _confirm();
  }

  Future<void> _confirm() async {
    // حارسٌ صريح لا يتّكل على تعطيل الزرّ. الإرسال صار آلياً، وحدث onChanged
    // مكرَّر — أو لصقٌ، أو ضغطة على الزرّ في أثناء الإرسال — كان سينشئ
    // حوالتين لا واحدة. وهذا مالٌ لا يُسترجع.
    if (_sending || _spent) return;
    final user = ref.read(authControllerProvider).user;
    if (user == null || _code.length != _otpLength) return;

    setState(() {
      _sending = true;
      _otpError = null;
    });
    FocusScope.of(context).unfocus();

    // 1) التحقّق من الرمز على الخادم. لا يُقارَن هنا — العميل لا يعرفه.
    try {
      await ref.read(authRepositoryProvider).verifyOtp(user.phone, _code);
    } on ApiFailure catch (e) {
      if (!mounted) return;
      setState(() {
        _sending = false;
        _otpError = e.message;
        _code = '';
        _otpCtl.clear();
      });
      return;
    }

    // 2) الرمز صحيح ⇒ استُهلك على الخادم. أيّ فشلٍ بعد هذا السطر يستلزم
    //    رمزاً جديداً، لا إعادة محاولة بالرمز ذاته.
    if (!mounted) return;
    _spent = true;

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
      _timer?.cancel();
      setState(() {
        _sending = false;
        _code = '';
        _otpCtl.clear();
        _left = 0; // إعادة الإرسال متاحة فوراً — الرمز السابق مات
      });

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

  bool get _busy => _sending || _requesting;
  bool get _ready => !_busy && !_spent && _code.length == _otpLength;

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
                        KvRow('من حساب', user?.displayName ?? '—'),
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
                  child: _otpCard(user?.phone ?? ''),
                ),
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
                    loading: _sending,
                    onPressed: _ready ? _confirm : null,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _otpCard(String phone) => GlassCard(
        padding: kCardPad,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text('رمز التحقّق', style: T.kufi(15, FontWeight.w600)),
            const SizedBox(height: 4),
            Text.rich(
              TextSpan(
                style: T.plex(12.5, FontWeight.w400,
                    color: R.inkA(.58), height: 1.5),
                children: [
                  const TextSpan(text: 'أرسلنا رمزاً من 4 أرقام إلى رقمك '),
                  // عازل يونيكود حتى لا يختلّ ترتيب الرقم داخل جملة عربية.
                  TextSpan(
                    text: '\u{2066}${Fmt.phone(phone)}\u{2069}',
                    style: T.plex(12.5, FontWeight.w600, color: R.ink),
                  ),
                  const TextSpan(text: ' عبر واتساب.'),
                ],
              ),
            ),
            const SizedBox(height: kGap),
            _OtpField(
              length: _otpLength,
              code: _code,
              controller: _otpCtl,
              focusNode: _otpFocus,
              enabled: !_busy && !_spent,
              onChanged: (v) {
                setState(() {
                  _code = v;
                  _otpError = null;
                });
                // موافقةٌ آلية عند اكتمال الخانات الأربع — بلا ضغط زرّ.
                // «الصحيح والمطابق» يقرّره الخادم لا التطبيق: العميل لا يعرف
                // الرمز أصلاً، فالاكتمال يبدأ التحقّق، والتحقّق هو من يوافق.
                if (v.length == _otpLength) _autoConfirm();
              },
            ),
            const SizedBox(height: kGap),
            _otpStatus(),
          ],
        ),
      );

  Widget _otpStatus() {
    // الإرسال بلا ضغطة زرّ يوجب إشارةً صريحة: بدونها يظنّ الوكيل أن الرمز
    // لم يُقبَل فيمسحه ويعيد كتابته والحوالة في طريقها.
    if (_sending) {
      return Row(
        children: [
          const SizedBox(
              width: 14,
              height: 14,
              child: CircularProgressIndicator(strokeWidth: 2)),
          const SizedBox(width: 10),
          Text('جارٍ التحقّق وإرسال الحوالة…',
              style: T.plex(12, FontWeight.w600, color: R.primary)),
        ],
      );
    }

    if (_requesting) {
      return Row(
        children: [
          const SizedBox(
              width: 14,
              height: 14,
              child: CircularProgressIndicator(strokeWidth: 2)),
          const SizedBox(width: 10),
          Text('جارٍ إرسال الرمز…',
              style: T.plex(12, FontWeight.w500, color: R.inkA(.55))),
        ],
      );
    }

    if (_otpError != null) {
      return Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(Icons.error_outline_rounded, size: 15, color: R.error),
          const SizedBox(width: 7),
          Expanded(
            child: Text(_otpError!,
                style:
                    T.plex(12, FontWeight.w500, color: R.error, height: 1.5)),
          ),
        ],
      );
    }

    if (_spent) {
      return Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(Icons.info_outline_rounded, size: 15, color: R.warnIcon),
          const SizedBox(width: 7),
          Expanded(
            child: Text('استُهلك الرمز — اطلب رمزاً جديداً لإعادة المحاولة.',
                style:
                    T.plex(12, FontWeight.w500, color: R.warnInk, height: 1.5)),
          ),
        ],
      );
    }

    if (_left > 0) {
      final m = (_left ~/ 60).toString().padLeft(2, '0');
      final s = (_left % 60).toString().padLeft(2, '0');
      return Row(
        children: [
          Icon(Icons.schedule_rounded, size: 15, color: R.inkA(.4)),
          const SizedBox(width: 7),
          Text('الوقت المتبقّي لإعادة الإرسال',
              style: T.plex(12, FontWeight.w400, color: R.inkA(.55))),
          const Spacer(),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Text('$m:$s',
                style: T.kufi(13, FontWeight.w700, color: R.inkA(.7))),
          ),
        ],
      );
    }

    return Align(
      alignment: AlignmentDirectional.centerStart,
      child: TextButton(
        onPressed: _sending ? null : _requestOtp,
        style: TextButton.styleFrom(
          minimumSize: const Size(44, 40),
          padding: const EdgeInsets.symmetric(horizontal: 4),
        ),
        child: Text('إعادة إرسال الرمز',
            style: T.plex(13, FontWeight.w600, color: R.primary)),
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

/// خانات الرمز الأربع فوق حقلٍ شفّاف يلتقط لوحة المفاتيح.
///
/// `OtpBoxes` عرضٌ فقط — لا يقبل إدخالاً. وشاشة الدخول تستعمل لوحة أرقام
/// خاصة، لكنها هنا كانت ستزاحم مُلخّص الحوالة على الشاشة، فالمُلخَّص هو
/// ما يجب أن يراه الوكيل قبل أن يؤكّد.
class _OtpField extends StatelessWidget {
  const _OtpField({
    required this.length,
    required this.code,
    required this.controller,
    required this.focusNode,
    required this.enabled,
    required this.onChanged,
  });

  final int length;
  final String code;
  final TextEditingController controller;
  final FocusNode focusNode;
  final bool enabled;
  final ValueChanged<String> onChanged;

  @override
  Widget build(BuildContext context) => Stack(
        children: [
          OtpBoxes(value: code, length: length),
          Positioned.fill(
            child: TextField(
              controller: controller,
              focusNode: focusNode,
              enabled: enabled,
              keyboardType: TextInputType.number,
              textInputAction: TextInputAction.done,
              autofocus: false,
              showCursor: false,
              enableInteractiveSelection: false,
              // WesternDigits أولاً: المرشّح بعده يسمح بـ [0-9] فقط، فلو
              // سبقه لحذف الرقم الهندي قبل أن يُحوَّل — ولبدت لوحة المفاتيح
              // وكأنها لا تكتب شيئاً.
              inputFormatters: [
                WesternDigits(),
                FilteringTextInputFormatter.digitsOnly,
                LengthLimitingTextInputFormatter(length),
              ],
              style: const TextStyle(color: Colors.transparent, fontSize: 2),
              cursorColor: Colors.transparent,
              decoration: const InputDecoration(
                border: InputBorder.none,
                enabledBorder: InputBorder.none,
                focusedBorder: InputBorder.none,
                disabledBorder: InputBorder.none,
                counterText: '',
                contentPadding: EdgeInsets.zero,
                filled: false,
              ),
              onChanged: onChanged,
            ),
          ),
        ],
      );
}
