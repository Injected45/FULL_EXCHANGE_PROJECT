import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import '../auth/auth_repository.dart';
import '../employee_app/employee_session.dart';
import 'send_layout.dart';

/// رمزُ تأكيد الحوالة — **بطاقةٌ واحدة تخدم القناتين والبابين**.
///
/// ══════════════════════════════════════════════════════════════════════════
///  أمرُ المالك (11 سبتمبر 2026)
/// ══════════════════════════════════════════════════════════════════════════
///
/// «عند إضافة حوالة جديدة، لحظةَ تأكيدٍ وإرسال، أريد أن تُرسل رمزَ التحقق
///  ليصله OTP يدخله فيتمّ التحويل **بنفس آليّة التحويل الداخلي** — وهذا
///  المنطق يُطبَّق على الوكيل والموظف. وفي تطبيق الموظف يُنفَّذ على الحوالة
///  الداخلية والخارجية بالكامل، **لتوحيد سياسة العمل**».
///
/// ⚠ ولذلك خرجت من شاشة المراجعة الداخلية إلى هنا: أربعُ حالاتٍ تحتاجها —
/// الوكيلُ داخلياً وخارجياً، والموظفُ داخلياً وخارجياً. ونسخةٌ لكلّ واحدةٍ
/// منها تعني أربعَ بطاقاتٍ تفترق عند أوّل تعديلٍ في المهلة أو الطول أو نصّ
/// الخطأ — ثمّ يرى الوكيلُ في الخارجية شيئاً غيرَ ما رآه في الداخلية.
///
/// ── وما تملكه هذه البطاقة، وما لا تملكه ─────────────────────────────────
///
/// تملك: طلبَ الرمز عند الظهور، ومؤقّتَ إعادة الإرسال، وحقلَ الإدخال،
/// والتحقّقَ من الخادم، وحالاتِ العرض كلَّها.
///
/// ⚠ **ولا تملك تنفيذَ الحوالة.** حين يُقبل الرمز تنادي [onVerified] ويتولّى
/// صاحبُ الشاشة الإنشاء — لأنّ الداخليةَ والخارجية تُنشآن بنقطتين مختلفتين
/// وبيانين مختلفين. فما يُوحَّد هو المراسم، لا المال.
class TransferOtpPanel extends ConsumerStatefulWidget {
  const TransferOtpPanel({
    super.key,
    required this.controller,
    required this.onVerified,
  });

  /// يصل به صاحبُ الشاشة إلى جاهزية البطاقة وانشغالها، ويؤكّد من زرِّه.
  final TransferOtpController controller;

  /// يُنادى **بعد** قبول الخادم للرمز — وهو موضعُ إنشاء الحوالة.
  ///
  /// ⚠ يُنادى مرّةً واحدة: الرمزُ يُستهلَك على الخادم عند أوّل مطابقةٍ ناجحة،
  /// فأيُّ فشلٍ بعده يستلزم رمزاً جديداً لا محاولةً ثانية بالرمز ذاته.
  final Future<void> Function() onVerified;

  @override
  ConsumerState<TransferOtpPanel> createState() => _TransferOtpPanelState();
}

/// جسرٌ بين البطاقة وزرِّ التأكيد في أسفل الشاشة.
///
/// ⚠ بدونه كان صاحبُ الشاشة يحتاج أن يحتفظ بحالة الرمز بنفسه — أي أن تعود
/// نصفُ البطاقة إلى حيث خرجت منها.
class TransferOtpController extends ChangeNotifier {
  bool _ready = false;
  bool _busy = false;
  Future<void> Function()? _submit;

  /// اكتمل الرمزُ ولم يُستهلَك ولا نداءَ جارياً.
  bool get ready => _ready;

  /// نداءٌ جارٍ — طلبُ رمزٍ أو تحقّقٌ أو إنشاء.
  bool get busy => _busy;

  /// يبدأ التحقّقَ من الزرّ. والبطاقةُ تبدؤه من تلقاء نفسها عند اكتمال
  /// الخانات كذلك — فالزرُّ طريقٌ ثانٍ لا وحيد.
  Future<void> submit() async => _submit?.call();

  void _bind(Future<void> Function() submit) => _submit = submit;

  void _set({bool? ready, bool? busy}) {
    final changed =
        (ready != null && ready != _ready) || (busy != null && busy != _busy);
    if (ready != null) _ready = ready;
    if (busy != null) _busy = busy;
    // ⚠ لا إشعارَ بلا تغيير: إشعارٌ في كلّ ضغطةِ حرفٍ يُعيد بناء الشاشة
    // كاملةً تحت إصبع الوكيل.
    if (changed) notifyListeners();
  }
}

class _TransferOtpPanelState extends ConsumerState<TransferOtpPanel> {
  /// أربع خانات: الخادم يولّد `rand(1000, 9999)`، و`checkOtp` يتحقق `digits:4`.
  static const _otpLength = 4;

  /// مهلة إعادة الإرسال. صلاحيةُ الرمز نفسِه ثلاث دقائق (`ExpeaerTime`).
  static const _resendAfter = 60;

  final _otpCtl = TextEditingController();
  final _otpFocus = FocusNode();

  String _code = '';
  String? _otpError;
  bool _requesting = false;
  bool _verifying = false;

  /// صار الرمزُ مستهلَكاً: `checkOtp` يضع `ISActive = 1` عند أوّل مطابقةٍ
  /// ناجحة، فإن أخفق الإنشاءُ بعدها لا ينفع الرمزُ مرّةً ثانية — وصاحبُه
  /// يحتاج رمزاً جديداً لا محاولةً ثانية بالرمز ذاته.
  bool _spent = false;

  int _left = 0;
  Timer? _timer;

  /// وضعُ الموظف — يغيّر بابَ الرمز وحدَه، لا شيئاً في الحوالة.
  bool get _asEmployee =>
      ref.read(employeeAuthProvider).status == EmpSessionStatus.signedIn;

  /// الهاتفُ المعروض — هاتفُ صاحب الجلسة.
  ///
  /// ⚠ عرضُ رقم الوكيل لموظفٍ ينتظر رمزاً على هاتفه هو كان سيجعله ينتظر
  /// رسالةً لن تصله.
  String get _phone => _asEmployee
      ? (ref.watch(employeeAuthProvider).profile?.phone ?? '')
      : (ref.watch(authControllerProvider).user?.phone ?? '');

  bool get _busy => _requesting || _verifying;

  @override
  void initState() {
    super.initState();
    widget.controller._bind(_confirm);
    WidgetsBinding.instance.addPostFrameCallback((_) => _request());
  }

  @override
  void dispose() {
    _timer?.cancel();
    _otpCtl.dispose();
    _otpFocus.dispose();
    super.dispose();
  }

  void _sync() => widget.controller._set(
        ready: !_busy && !_spent && _code.length == _otpLength,
        busy: _busy,
      );

  void _startTimer() {
    _timer?.cancel();
    setState(() => _left = _resendAfter);
    _timer = Timer.periodic(const Duration(seconds: 1), (t) {
      if (!mounted) return t.cancel();
      setState(() => _left--);
      if (_left <= 0) t.cancel();
    });
  }

  Future<void> _request() async {
    if (_requesting) return;

    setState(() {
      _requesting = true;
      _otpError = null;
    });
    _sync();

    try {
      await ref.read(authRepositoryProvider).requestTransferOtp(
            asEmployee: _asEmployee,
            agentPhone: ref.read(authControllerProvider).user?.phone,
          );
      if (!mounted) return;
      setState(() {
        _requesting = false;
        _spent = false;
        _code = '';
        _otpCtl.clear();
      });
      _sync();
      _startTimer();
    } on ApiFailure catch (e) {
      if (!mounted) return;
      setState(() {
        _requesting = false;
        _otpError = e.message;
        _left = 0;
      });
      _sync();
    }
  }

  /// الموافقةُ الآلية عند اكتمال الرمز.
  ///
  /// المهلةُ القصيرة ليست تجميلاً: بدونها تتحرّك الشاشةُ قبل أن يرى المستخدم
  /// خانته الأخيرة تمتلئ، فلا يعرف أضغَط شيئاً أم أخطأ. ويُعاد فحصُ الطول
  /// بعدها لأنه قد يكون حذف رقماً في أثنائها.
  Future<void> _autoConfirm() async {
    await Future.delayed(const Duration(milliseconds: 180));
    if (!mounted || _code.length != _otpLength) return;
    await _confirm();
  }

  Future<void> _confirm() async {
    // حارسٌ صريح لا يتّكل على تعطيل الزرّ: الإرسالُ آليّ، وحدثُ `onChanged`
    // مكرَّر — أو لصقٌ، أو ضغطةٌ على الزرّ أثناء الإرسال — كان سينشئ حوالتين
    // لا واحدة. وهذا مالٌ لا يُسترجع.
    if (_busy || _spent || _code.length != _otpLength) return;

    setState(() {
      _verifying = true;
      _otpError = null;
    });
    _sync();
    FocusScope.of(context).unfocus();

    // ١) التحقّقُ على الخادم. لا يُقارَن هنا — العميل لا يعرف الرمز.
    try {
      await ref.read(authRepositoryProvider).verifyTransferOtp(
            asEmployee: _asEmployee,
            agentPhone: ref.read(authControllerProvider).user?.phone,
            code: _code,
          );
    } on ApiFailure catch (e) {
      if (!mounted) return;
      setState(() {
        _verifying = false;
        _otpError = e.message;
        _code = '';
        _otpCtl.clear();
      });
      _sync();
      return;
    }

    // ٢) قُبِل ⇒ استُهلك على الخادم. أيُّ فشلٍ بعد هذا السطر يستلزم رمزاً
    //    جديداً، لا إعادةَ محاولةٍ بالرمز ذاته.
    if (!mounted) return;
    _spent = true;
    _sync();

    try {
      await widget.onVerified();
    } finally {
      if (mounted) {
        setState(() => _verifying = false);
        _sync();
      }
    }
  }

  @override
  Widget build(BuildContext context) => GlassCard(
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
                    text: '\u{2066}${Fmt.phone(_phone)}\u{2069}',
                    style: T.plex(12.5, FontWeight.w600, color: R.ink),
                  ),
                  const TextSpan(text: ' عبر واتساب.'),
                ],
              ),
            ),
            const SizedBox(height: kGap),
            OtpField(
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
                _sync();
                // موافقةٌ آلية عند اكتمال الخانات — بلا ضغط زرّ. و«الصحيح
                // والمطابق» يقرّره الخادم لا التطبيق: العميل لا يعرف الرمز
                // أصلاً، فالاكتمالُ يبدأ التحقّق، والتحقّقُ هو من يوافق.
                if (v.length == _otpLength) _autoConfirm();
              },
            ),
            const SizedBox(height: kGap),
            _status(),
          ],
        ),
      );

  Widget _status() {
    // الإرسالُ بلا ضغطة زرّ يوجب إشارةً صريحة: بدونها يظنّ المستخدم أنّ
    // الرمز لم يُقبَل فيمسحه ويعيد كتابته والحوالةُ في طريقها.
    if (_verifying) {
      return _line('جارٍ التحقّق وإرسال الحوالة…',
          color: R.primary, weight: FontWeight.w600, spinner: true);
    }

    if (_requesting) {
      return _line('جارٍ إرسال الرمز…',
          color: R.inkA(.55), weight: FontWeight.w500, spinner: true);
    }

    if (_otpError != null) {
      return _line(_otpError!,
          color: R.error, weight: FontWeight.w500, icon: Icons.error_outline_rounded);
    }

    if (_spent) {
      return _line('استُهلك الرمز — اطلب رمزاً جديداً لإعادة المحاولة.',
          color: R.warnInk,
          weight: FontWeight.w500,
          icon: Icons.info_outline_rounded,
          iconColor: R.warnIcon);
    }

    if (_left > 0) {
      return Text('يمكن إعادة الإرسال بعد $_left ثانية',
          style: T.plex(12, FontWeight.w400, color: R.inkA(.5)));
    }

    return TextButton(
      onPressed: _busy ? null : _request,
      style: TextButton.styleFrom(
        minimumSize: const Size(44, 44),
        padding: EdgeInsets.zero,
      ),
      child: Text('إعادة إرسال الرمز',
          style: T.plex(13, FontWeight.w600, color: R.primary)),
    );
  }

  Widget _line(String text,
          {required Color color,
          required FontWeight weight,
          bool spinner = false,
          IconData? icon,
          Color? iconColor}) =>
      Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          if (spinner)
            const SizedBox(
                width: 14,
                height: 14,
                child: CircularProgressIndicator(strokeWidth: 2))
          else if (icon != null)
            Icon(icon, size: 15, color: iconColor ?? color),
          SizedBox(width: spinner ? 10 : 7),
          Expanded(
            child: Text(text,
                style: T.plex(12, weight, color: color, height: 1.5)),
          ),
        ],
      );
}

/// خاناتُ الرمز — حقلٌ شفّاف فوق مربّعاتٍ مرسومة.
///
/// ⚠ عامٌّ لا خاصّ: تستعمله بطاقةُ الرمز التي تخدم القناتين، وهو نفسُه الذي
/// كان في شاشة المراجعة الداخلية — نُقل ولم يُنسخ.
class OtpField extends StatelessWidget {
  const OtpField({
    super.key,
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
