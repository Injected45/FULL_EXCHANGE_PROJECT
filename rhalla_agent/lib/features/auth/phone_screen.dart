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

class _PhoneScreenState extends ConsumerState<PhoneScreen>
    with HardwareDigits {
  String _digits = '';
  String? _error;
  bool _sending = false;

  @override
  void onHardwareDigit(String d) => _push(d);

  @override
  void onHardwareDelete() => _pop();

  @override
  void onHardwareSubmit() {
    if (Fmt.isValidLibyanPhone(_digits) && !_sending) _submit();
  }

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
          // الشعار في الوسط لا في الزاوية: هو ترويسة الشاشة لا علامة مائية.
          const Padding(
            padding: EdgeInsets.fromLTRB(R.padScreen, 14, R.padScreen, 0),
            child: BrandLockup(),
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
                  Text('أدخل رقم هاتفك',
                      textAlign: TextAlign.center, style: T.title),
                  const SizedBox(height: 8),
                  Text('يجب أن يكون رقم الهاتف مسجلاً لدى الشركة مسبقاً',
                      textAlign: TextAlign.center, style: T.body),
                ],
              ),
            ),
          ),

          Padding(
            padding: const EdgeInsets.fromLTRB(26, 22, 26, 0),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                _PhoneField(digits: _digits),
                const SizedBox(height: 12),
                if (_error != null)
                  Text(_error!,
                      textAlign: TextAlign.center,
                      style: T.plex(12, FontWeight.w500,
                          color: R.error, height: 1.5))
                else
                  // Flexible لا Expanded: Expanded يمدّ النصّ على ما تبقّى من
                  // العرض بعد الأيقونة، فيبدو مزاحاً عن الوسط. Flexible يجعل
                  // الأيقونة والنصّ كتلةً واحدة تتوسّط الصفّ.
                  Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const WhatsAppMark(size: 22),
                      const SizedBox(width: 9),
                      Flexible(
                        child: Text(
                          'سيصلك رمز التحقق عبر WhatsApp على رقمك المسجل لدينا',
                          textAlign: TextAlign.center,
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
              label: 'إرسال رمز التحقق',
              loading: _sending,
              // السهم إلى اليسار — جهة المتابعة في واجهة عربية.
              trailing: const Icon(Icons.chevron_left_rounded,
                  size: 22, color: Colors.white),
              onPressed: valid ? _submit : null,
            ),
          ),

          // المسار الثاني — دخول الموظف.
          //
          // زرٌّ نصّي لا زرّ رئيسي: الوكيل هو المسار الغالب، والموظف يدخل
          // مرّة واحدة عند التفعيل ثم يبقى داخلاً. وإبرازهما معاً يجعل
          // الوكيل يتردّد كل مرّة أمام خيارين متساويين.
          Padding(
            padding: const EdgeInsets.only(bottom: 6),
            child: TextButton(
              onPressed: _sending ? null : () => context.push('/employee/activate'),
              style: TextButton.styleFrom(minimumSize: const Size(44, 44)),
              child: Text('الدخول كموظف',
                  style: T.plex(13, FontWeight.w600, color: R.primaryDark)),
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
