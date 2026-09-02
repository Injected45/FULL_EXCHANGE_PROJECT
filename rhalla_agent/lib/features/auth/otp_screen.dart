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
    if (_code.length >= _otpLength || _verifying) return;
    setState(() {
      _code += d;
      _error = null;
    });
    if (_code.length == _otpLength) _verify();
  }

  void _pop() {
    if (_code.isEmpty || _verifying) return;
    setState(() => _code = _code.substring(0, _code.length - 1));
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
      ref.read(authControllerProvider.notifier).adopt(session);
      context.go('/');
    } on ApiFailure catch (e) {
      if (!mounted) return;
      setState(() {
        _error = e.message;
        _code = '';
        _verifying = false;
      });
    }
  }

  Future<void> _resend() async {
    try {
      await ref.read(authRepositoryProvider).requestOtp(widget.phone);
      _startTimer();
    } on ApiFailure catch (e) {
      if (mounted) setState(() => _error = e.message);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Screen(
      child: Column(
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 8, R.padScreen, 0),
            child: Row(
              children: [
                CircleIconButton(
                  onPressed: () => context.pop(),
                  child: const Icon(Icons.arrow_back_ios_new, size: 16, color: R.ink),
                ),
              ],
            ),
          ),

          RiseIn(
            duration: const Duration(milliseconds: 500),
            child: Padding(
              padding: const EdgeInsets.fromLTRB(26, 24, 26, 0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text('رمز التحقّق', style: T.title),
                  const SizedBox(height: 8),
                  Text.rich(
                    TextSpan(
                      style: T.plex(13, FontWeight.w400,
                          color: R.inkA(.58), height: 1.75),
                      children: [
                        const TextSpan(text: 'أدخل الرمز المُرسل إلى '),
                        // عازل يونيكود حتى لا يختلّ ترتيب الرقم داخل جملة عربية.
                        TextSpan(
                          text: '\u{2066}+218 ${Fmt.phone(widget.phone)}\u{2069}',
                          style: T.plex(13, FontWeight.w600, color: R.ink),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ),

          Padding(
            padding: const EdgeInsets.fromLTRB(26, 28, 26, 0),
            child: OtpBoxes(value: _code, length: _otpLength),
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
    if (_error != null) {
      return Text(
        _error!,
        textAlign: TextAlign.center,
        style: T.plex(12, FontWeight.w500, color: R.error, height: 1.5),
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
              valueColor: const AlwaysStoppedAnimation(R.primary),
              backgroundColor: R.inkA(.14),
            ),
          ),
          const SizedBox(width: 9),
          Text('جارٍ التحقّق…',
              style: T.plex(12.5, FontWeight.w500, color: R.inkA(.6))),
        ],
      );
    }

    if (_resendIn > 0) {
      return Center(
        child: Text('إعادة إرسال الرمز بعد $_resendIn ثانية',
            style: T.plex(12.5, FontWeight.w400, color: R.inkA(.55))),
      );
    }

    return Center(
      child: TextButton(
        onPressed: _resend,
        style: TextButton.styleFrom(minimumSize: const Size(44, 44)),
        child: Text('إعادة إرسال الرمز',
            style: T.plex(12.5, FontWeight.w600, color: R.primaryGradEnd)),
      ),
    );
  }
}
