import 'dart:ui';

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
import 'auth_repository.dart';

class PhoneScreen extends ConsumerStatefulWidget {
  const PhoneScreen({super.key});

  @override
  ConsumerState<PhoneScreen> createState() => _PhoneScreenState();
}

class _PhoneScreenState extends ConsumerState<PhoneScreen> {
  String _digits = '';
  String? _error;
  bool _sending = false;

  void _push(String d) {
    if (_digits.length >= 9) return;
    setState(() {
      _digits += d;
      _error = null;
    });
  }

  void _pop() {
    if (_digits.isEmpty) return;
    setState(() => _digits = _digits.substring(0, _digits.length - 1));
  }

  Future<void> _submit() async {
    if (!Fmt.isValidLibyanPhone(_digits)) {
      setState(() => _error = 'أدخل رقماً ليبياً صحيحاً من 9 أرقام يبدأ بـ 9');
      return;
    }

    setState(() {
      _sending = true;
      _error = null;
    });

    try {
      await ref.read(authRepositoryProvider).requestOtp(_digits);
      if (!mounted) return;
      context.push('/otp', extra: _digits);
    } on ApiFailure catch (e) {
      if (!mounted) return;
      setState(() => _error = e.message);
    } finally {
      if (mounted) setState(() => _sending = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final valid = Fmt.isValidLibyanPhone(_digits);

    return Screen(
      child: Column(
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 8, R.padScreen, 0),
            child: Row(
              children: [
                const Spacer(),
                const RhallaLogo(size: 30, color: Color(0xBF00B17A)),
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
                  Text('أدخل رقم هاتفك', style: T.title),
                  const SizedBox(height: 8),
                  Text('سنرسل لك رمز تحقّق من 4 أرقام عبر واتساب.', style: T.body),
                ],
              ),
            ),
          ),

          Padding(
            padding: const EdgeInsets.fromLTRB(26, 22, 26, 0),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                _PhoneField(digits: _digits),
                const SizedBox(height: 12),
                if (_error != null)
                  Text(_error!,
                      style: T.plex(12, FontWeight.w500,
                          color: R.error, height: 1.5))
                else
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Icon(Icons.info_outline, size: 15, color: R.inkA(.45)),
                      const SizedBox(width: 9),
                      Expanded(
                        child: Text(
                          'يجب أن يكون الرقم مسجّلاً لدى الشركة مسبقاً.',
                          style: T.plex(11.5, FontWeight.w400,
                              color: R.inkA(.55), height: 1.5),
                        ),
                      ),
                    ],
                  ),
              ],
            ),
          ),

          const Spacer(),

          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 0, R.padScreen, 12),
            child: PrimaryButton(
              label: 'إرسال الرمز',
              loading: _sending,
              onPressed: valid ? _submit : null,
            ),
          ),

          NumericKeypad(onDigit: _push, onDelete: _pop),
        ],
      ),
    );
  }
}

class _PhoneField extends StatelessWidget {
  const _PhoneField({required this.digits});

  final String digits;

  @override
  Widget build(BuildContext context) {
    return DecoratedBox(
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(R.rCard),
        boxShadow: [
          BoxShadow(
            color: const Color(0xFF032D21).withValues(alpha: .07),
            blurRadius: 30,
            offset: const Offset(0, 12),
          )
        ],
      ),
      child: ClipRRect(
        borderRadius: BorderRadius.circular(R.rCard),
        child: BackdropFilter(
          filter: ImageFilter.blur(sigmaX: 10, sigmaY: 10),
          child: Container(
            padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 16),
            decoration: BoxDecoration(
              gradient: R.glassGradient(from: .85, to: .58),
              border: Border.all(color: R.whiteA(.92)),
              borderRadius: BorderRadius.circular(R.rCard),
            ),
            child: Directionality(
              textDirection: TextDirection.ltr,
              child: Row(
                children: [
                  const LibyaFlag(),
                  const SizedBox(width: 12),
                  Text('+218', style: T.kufi(16, FontWeight.w600)),
                  const SizedBox(width: 12),
                  Container(width: 1, height: 26, color: R.inkA(.1)),
                  const SizedBox(width: 12),
                  Expanded(
                    child: Text(
                      Fmt.phone(digits),
                      style: T.kufi(19, FontWeight.w600, spacing: 1.14),
                    ),
                  ),
                  const BlinkingCaret(),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
