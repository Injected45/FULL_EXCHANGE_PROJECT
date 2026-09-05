import 'dart:async';

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
import 'auth_controller.dart';
import 'auth_repository.dart';

class OtpScreen extends ConsumerStatefulWidget {
  const OtpScreen({super.key, required this.phone});

  final String phone;

  @override
  ConsumerState<OtpScreen> createState() => _OtpScreenState();
}

class _OtpScreenState extends ConsumerState<OtpScreen> with HardwareDigits {
  @override
  void onHardwareDigit(String d) => _push(d);

  @override
  void onHardwareDelete() => _pop();

  /// أربع خانات لا ستّاً.
  ///
  /// التصميم رسم ستّ خانات، لكن الخادم يولّد rand(1000, 9999) —
  /// أي أربعة أرقام (Code_OtpTB boot hook)، و checkOtp يتحقق digits:4.
  /// ستّ خانات تجعل الدخول مستحيلاً.
  static const _otpLength = 4;

  String _code = '';
  String? _error;
  bool _verifying = false;

  /// شاشة «تم التحقق بنجاح» تُعرض قبل الدخول. الجلسة تكون قد صدرت فعلاً —
  /// هذه لحظة طمأنة لا انتظار عمل.
  bool _succeeded = false;

  // المؤقّت في التصميم يبدأ من 45 ثانية.
  int _resendIn = 45;
  Timer? _timer;

  @override
  void initState() {
    super.initState();
    _startTimer();
  }

  void _startTimer() {
    _timer?.cancel();
    setState(() => _resendIn = 45);
    _timer = Timer.periodic(const Duration(seconds: 1), (t) {
      if (!mounted) return t.cancel();
      setState(() => _resendIn--);
      if (_resendIn <= 0) t.cancel();
    });
  }

  @override
  void dispose() {
    _timer?.cancel();
    super.dispose();
  }

  void _push(String d) {
    if (_verifying || _succeeded) return;

    // رمزٌ مرفوض ما زال معروضاً: أول رقم جديد يبدأ محاولةً نظيفة بدل أن
    // يُلحق برقمٍ مكتمل فلا يُقبَل شيء.
    if (_error != null) {
      setState(() {
        _code = d;
        _error = null;
      });
      return;
    }

    if (_code.length >= _otpLength) return;
    setState(() => _code += d);
    if (_code.length == _otpLength) _verify();
  }

  void _pop() {
    if (_code.isEmpty || _verifying || _succeeded) return;
    setState(() {
      _code = _code.substring(0, _code.length - 1);
      _error = null;
    });
  }

  Future<void> _verify() async {
    setState(() {
      _verifying = true;
      _error = null;
    });

    final repo = ref.read(authRepositoryProvider);
    try {
      // نقطة واحدة: الخادم يتحقّق من الرمز ويستهلكه ويُصدر رمز Sanctum.
      // استدعاء checkOtp قبلها زائد — وكان يستهلك الرمز مرّتين.
      final session = await repo.completeOtpLogin(widget.phone, _code);
      if (!mounted) return;

      setState(() {
        _verifying = false;
        _succeeded = true;
      });
      _timer?.cancel();

      // وقفةٌ قصيرة ليرى الوكيل أن التحقّق نجح. أطول من ذلك يصير انتظاراً
      // بلا سبب، وأقصر لا يُقرأ.
      await Future.delayed(const Duration(milliseconds: 1400));
      if (!mounted) return;
      ref.read(authControllerProvider.notifier).adopt(session);
      if (mounted) context.go('/');
    } on ApiFailure catch (e) {
      if (!mounted) return;
      // الرمز يبقى معروضاً — انظر OtpBoxes.error.
      setState(() {
        _error = e.message;
        _verifying = false;
      });
    }
  }

  Future<void> _resend() async {
    try {
      await ref.read(authRepositoryProvider).requestOtp(widget.phone);
      if (!mounted) return;

      // رمزٌ جديد وصل، فالخانات تُفرَغ استعداداً له.
      //
      // الرمز القديم صار مستهلكاً على الخادم: تركُه معروضاً يجعل الوكيل
      // يواجه أربع خانات ممتلئة برقمٍ لن يُقبل، وعليه أن يمسحها بنفسه
      // قبل أن يكتب ما وصله. ورسالة الخطأ تذهب معه — هي عن المحاولة
      // السابقة، لا عن هذه.
      setState(() {
        _code = '';
        _error = null;
      });
      _startTimer();
    } on ApiFailure catch (e) {
      // لم يُرسَل شيء: الخانات تبقى كما هي، والخطأ هو خطأ الإرسال.
      if (mounted) setState(() => _error = e.message);
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_succeeded) return const _VerifiedScreen();

    final failed = _error != null;

    return Screen(
      child: Column(
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 8, R.padScreen, 0),
            // زرّ الرجوع في مكانه، والشعار متوسّطٌ فوقه — Stack لا Row، وإلا
            // أزاح الزرُّ الشعارَ عن مركز الشاشة.
            child: Stack(
              alignment: Alignment.center,
              children: [
                // الرمز مرفوض ⇒ قرص أحمر مكان الشعار: النتيجة تحتلّ موضع
                // الصدارة بدل أن تُذكر في سطرٍ تحت الخانات.
                if (failed)
                  const StatusDisc.failure(size: 84)
                else
                  const BrandLockup(logoSize: 52),
                Positioned.directional(
                  textDirection: TextDirection.rtl,
                  start: 0,
                  top: 0,
                  child: CircleIconButton(
                    onPressed: () => context.pop(),
                    child: Icon(Icons.arrow_back_ios_new,
                        size: 16, color: R.ink),
                  ),
                ),
              ],
            ),
          ),

          RiseIn(
            duration: const Duration(milliseconds: 500),
            child: Padding(
              padding: const EdgeInsets.fromLTRB(26, 24, 26, 0),
              // قرار المالك (2 سبتمبر 2026): كل جملة في وسط الشاشة، فلا
              // تميل واحدة إلى جانب دون أخرى.
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  Text(failed ? 'رمز التحقق غير صحيح' : 'أدخل رمز التحقق',
                      textAlign: TextAlign.center,
                      style: failed
                          ? T.kufi(26, FontWeight.w800, color: R.error)
                          : T.title),
                  const SizedBox(height: 8),
                  if (failed)
                    Text(
                      'الرمز الذي أدخلته غير صحيح\nيرجى التحقق والمحاولة مرة أخرى',
                      textAlign: TextAlign.center,
                      style: T.plex(13, FontWeight.w400,
                          color: R.inkA(.58), height: 1.75),
                    )
                  else
                  Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const WhatsAppMark(size: 26),
                      const SizedBox(width: 9),
                      Flexible(
                        child: Text.rich(
                          textAlign: TextAlign.center,
                          TextSpan(
                            style: T.plex(13, FontWeight.w400,
                                color: R.inkA(.58), height: 1.75),
                            children: [
                              // كسر سطر مقصود قبل «إلى الرقم»: بلا هذا يقع
                              // الالتفاف داخل الرقم نفسه فيُقرأ نصفه في
                              // سطر ونصفه في آخر. الرقم يبقى كتلةً واحدة.
                              const TextSpan(
                                  text:
                                      'أدخل رمز التحقق المرسل عبر WhatsApp\nإلى الرقم '),
                              // عازل يونيكود حتى لا يختلّ ترتيب الرقم داخل
                              // جملة عربية. والرقم مستور الوسط — انظر
                              // Fmt.phoneMasked.
                              TextSpan(
                                text:
                                    '\u{2066}+218 ${Fmt.phoneMasked(widget.phone)}\u{2069}',
                                style:
                                    T.plex(13, FontWeight.w600, color: R.ink),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),

          Padding(
            padding: const EdgeInsets.fromLTRB(26, 28, 26, 0),
            child: OtpBoxes(
                value: _code, length: _otpLength, error: failed),
          ),

          Padding(
            padding: const EdgeInsets.fromLTRB(26, 20, 26, 0),
            child: _status(),
          ),

          const Spacer(),

          NumericKeypad(onDigit: _push, onDelete: _pop),
        ],
      ),
    );
  }

  Widget _status() {
    // سطر الخطأ ثم عرض إعادة الإرسال تحته: الوكيل يقرأ ما حدث، ثم يجد
    // الطريق إلى المحاولة التالية في المكان نفسه بلا بحث.
    final message = _error;
    if (message != null) {
      return Column(
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(Icons.error_outline_rounded, size: 16, color: R.error),
              const SizedBox(width: 7),
              Flexible(
                child: Text(
                  message,
                  textAlign: TextAlign.center,
                  style: T.plex(12, FontWeight.w600,
                      color: R.error, height: 1.5),
                ),
              ),
            ],
          ),
          const SizedBox(height: 18),
          _resendBlock(),
        ],
      );
    }

    if (_verifying) {
      return Row(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          SizedBox(
            width: 16,
            height: 16,
            child: CircularProgressIndicator(
              strokeWidth: 2,
              valueColor: AlwaysStoppedAnimation(R.primary),
              backgroundColor: R.inkA(.14),
            ),
          ),
          const SizedBox(width: 9),
          Text('جارٍ التحقّق…',
              style: T.plex(12.5, FontWeight.w500, color: R.inkA(.6))),
        ],
      );
    }

    return _resendBlock();
  }

  /// سطر السؤال فوق، والحبّة تحته — في الحالتين، فلا يقفز مكانها حين ينتهي
  /// العدّ.
  Widget _resendBlock() {
    final waiting = _resendIn > 0;

    return Column(
      children: [
        Text('لم يصلك رمز التحقق؟',
            textAlign: TextAlign.center,
            style: T.plex(13, FontWeight.w500, color: R.ink)),
        const SizedBox(height: 10),
        _ResendPill(
          onTap: waiting ? null : _resend,
          icon: waiting ? Icons.schedule_rounded : Icons.refresh_rounded,
          label: waiting
              // mm:ss لا «ثانية»: العدّ التنازلي يُقرأ بلمحة في هذا الشكل،
              // والعين تلتقط تناقصه بلا قراءة كلمة.
              ? 'إعادة الإرسال خلال ${_mmss(_resendIn)}'
              : 'إعادة إرسال رمز التحقق',
        ),
      ],
    );
  }

  static String _mmss(int seconds) {
    final s = seconds < 0 ? 0 : seconds;
    final m = (s ~/ 60).toString().padLeft(2, '0');
    final r = (s % 60).toString().padLeft(2, '0');
    return '$m:$r';
  }
}

/// حبّة إعادة الإرسال — أيقونة ونصّ داخل شكل بيضويّ.
///
/// شكل واحد لحالتين: انتظارٌ بعدّ تنازلي (معطّلة)، ثم إعادة إرسال (قابلة
/// للنقر). الشكل نفسه يمنع قفزة التخطيط عند انتهاء العدّ، وتغيّر اللون
/// والأيقونة يكفيان ليعرف الوكيل أنها صارت قابلة للضغط.
class _ResendPill extends StatelessWidget {
  const _ResendPill({required this.label, required this.icon, this.onTap});

  final String label;
  final IconData icon;
  final VoidCallback? onTap;

  @override
  Widget build(BuildContext context) {
    final enabled = onTap != null;
    final fg = enabled ? R.primaryDark : R.inkA(.55);

    return Material(
      color: enabled ? R.primaryA(.10) : R.inkA(.05),
      borderRadius: BorderRadius.circular(R.rPill),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(R.rPill),
        child: Container(
          // 44 حدّ الإصابة بالإبهام، ويبقى محفوظاً في حالة الانتظار أيضاً
          // فلا يتغيّر ارتفاع الشاشة حين تصير الحبّة قابلة للنقر.
          constraints: const BoxConstraints(minHeight: 44),
          padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 10),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(icon, size: 17, color: fg),
              const SizedBox(width: 9),
              Text(label, style: T.plex(12.5, FontWeight.w600, color: fg)),
            ],
          ),
        ),
      ),
    );
  }
}

/// «تم التحقق بنجاح» — لحظة طمأنة بين إصدار الجلسة والدخول.
///
/// الجلسة صدرت فعلاً قبل عرضها، فليست شاشة انتظارٍ لعمل: الوكيل أدخل رمزاً
/// يخصّ مالاً، وإخفاء النتيجة خلف انتقالٍ فوريّ يتركه غير واثق أنّ ما فعله
/// نجح.
class _VerifiedScreen extends StatelessWidget {
  const _VerifiedScreen();

  @override
  Widget build(BuildContext context) => Screen(
        child: Column(
          children: [
            const Spacer(),
            const StatusDisc.success(size: 116),
            const SizedBox(height: 26),
            Text('تم التحقق بنجاح',
                textAlign: TextAlign.center,
                style: T.kufi(28, FontWeight.w800, color: R.primaryDark)),
            const SizedBox(height: 10),
            Text('تم التحقق من رقم هاتفك بنجاح',
                textAlign: TextAlign.center,
                style: T.plex(14, FontWeight.w500, color: R.inkA(.6))),
            const SizedBox(height: 30),
            Padding(
              padding: const EdgeInsets.symmetric(horizontal: 40),
              child: Container(
                width: double.infinity,
                padding: const EdgeInsets.symmetric(vertical: 18),
                decoration: BoxDecoration(
                  color: R.primaryA(.06),
                  borderRadius: BorderRadius.circular(R.rCard),
                ),
                child: Column(
                  children: [
                    SizedBox(
                      width: 26,
                      height: 26,
                      child: CircularProgressIndicator(
                        strokeWidth: 2.6,
                        valueColor: AlwaysStoppedAnimation(R.primaryDark),
                        backgroundColor: R.primaryA(.15),
                      ),
                    ),
                    const SizedBox(height: 14),
                    Text('جاري تسجيل الدخول…',
                        textAlign: TextAlign.center,
                        style: T.kufi(15, FontWeight.w700,
                            color: R.primaryDark)),
                  ],
                ),
              ),
            ),
            const Spacer(),
            Padding(
              padding: const EdgeInsets.fromLTRB(26, 0, 26, 22),
              child: Column(
                children: [
                  Divider(color: R.primaryA(.22), height: 1),
                  const SizedBox(height: 14),
                  Text('معاً .. نحو خدمات مالية أكثر سهولة وأماناً',
                      textAlign: TextAlign.center,
                      style: T.plex(12.5, FontWeight.w500,
                          color: R.primaryA(.75))),
                ],
              ),
            ),
          ],
        ),
      );
}
