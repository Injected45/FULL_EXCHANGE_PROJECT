import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../security/otp_paste.dart';
import 'auth_controller.dart';

/// تأكيدُ حذف الحساب برمز تحقّق — **بنفس واجهة الدخول** (أمر المالك): الحذفُ
/// لا يُتراجَع عنه، فلا يقع بضغطةٍ واحدة. يُرسَل رمزٌ إلى هاتف الوكيل، ولا
/// يُحذف الحساب إلّا بعد أن يتحقّق الخادمُ منه.
class DeleteAccountOtpScreen extends ConsumerStatefulWidget {
  const DeleteAccountOtpScreen({super.key});

  @override
  ConsumerState<DeleteAccountOtpScreen> createState() =>
      _DeleteAccountOtpScreenState();
}

class _DeleteAccountOtpScreenState
    extends ConsumerState<DeleteAccountOtpScreen> with HardwareDigits {
  static const _otpLength = 4;

  String _code = '';
  bool _busy = false;
  bool _sending = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _send();
  }

  Future<void> _send() async {
    setState(() {
      _sending = true;
      _error = null;
    });
    try {
      await ref.read(authControllerProvider.notifier).requestDeleteOtp();
    } on ApiFailure catch (e) {
      if (mounted) setState(() => _error = e.message);
    } catch (_) {
      if (mounted) {
        setState(() => _error = 'تعذّر إرسال الرمز — تحقّق من الاتصال.');
      }
    } finally {
      if (mounted) setState(() => _sending = false);
    }
  }

  void _fill(String code) {
    if (_busy) return;
    setState(() => _code = code);
    _confirm();
  }

  void _push(String d) {
    if (_busy || _code.length >= _otpLength) return;
    setState(() {
      _error = null;
      _code += d;
    });
    if (_code.length == _otpLength) _confirm();
  }

  void _pop() {
    if (_code.isEmpty || _busy) return;
    setState(() => _code = _code.substring(0, _code.length - 1));
  }

  @override
  void onHardwareDigit(String d) => _push(d);

  @override
  void onHardwareDelete() => _pop();

  Future<void> _confirm() async {
    if (_code.length != _otpLength || _busy) return;
    setState(() => _busy = true);
    try {
      await ref.read(authControllerProvider.notifier).deleteAccount(_code);
      // النجاح ينقل الراوتر إلى شاشة الدخول تلقائياً (signedOut).
    } on ApiFailure catch (e) {
      if (mounted) {
        setState(() {
          _busy = false;
          _code = '';
          _error = e.message;
        });
      }
    } catch (_) {
      if (mounted) {
        setState(() {
          _busy = false;
          _code = '';
          _error = 'تعذّر حذف الحساب — تحقّق من الاتصال.';
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final phone = ref.watch(authControllerProvider).user?.phone;
    final failed = _error != null;

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'حذف الحساب',
            subtitle: 'تأكيدٌ برمز تحقّق',
            onBack: _busy ? null : () => Navigator.of(context).maybePop(),
          ),
          Expanded(
            child: ListView(
              padding: const EdgeInsets.fromLTRB(26, 24, 26, 40),
              children: [
                Container(
                  width: 72,
                  height: 72,
                  alignment: Alignment.center,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: R.error.withValues(alpha: .1),
                    border: Border.all(color: R.error.withValues(alpha: .25)),
                  ),
                  child: Icon(Icons.delete_outline_rounded,
                      size: 34, color: R.error),
                ),
                const SizedBox(height: 20),
                Text('أدخل رمز التحقّق لحذف حسابك',
                    textAlign: TextAlign.center,
                    style: T.kufi(17, FontWeight.w700)),
                const SizedBox(height: 10),
                Text(
                  phone == null
                      ? 'أرسلنا رمزاً عبر واتساب.'
                      : 'أرسلنا رمزاً عبر واتساب إلى ${Fmt.phone(phone)}.',
                  textAlign: TextAlign.center,
                  style: T.plex(12.5, FontWeight.w400,
                      color: R.inkA(.55), height: 1.6),
                ),
                const SizedBox(height: 24),
                OtpBoxes(value: _code, length: _otpLength, error: failed),
                const SizedBox(height: 16),
                _status(),
                const SizedBox(height: 8),
                NumericKeypad(onDigit: _push, onDelete: _pop),
                PasteOtpButton(enabled: !_busy, onCode: _fill),
                const SizedBox(height: 8),
                Center(
                  child: TextButton(
                    onPressed: (_sending || _busy) ? null : _send,
                    style: TextButton.styleFrom(
                        minimumSize: const Size(44, 44)),
                    child: Text('إعادة إرسال الرمز',
                        style: T.plex(12.5, FontWeight.w600,
                            color: R.primaryGradEnd)),
                  ),
                ),
                Center(
                  child: Text(
                    '⚠ الحذف لا يمكن التراجع عنه من التطبيق.',
                    style: T.plex(11.5, FontWeight.w500, color: R.error),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _status() {
    if (_busy) {
      return Center(
        child: Text('جارٍ الحذف…',
            style: T.plex(12.5, FontWeight.w500, color: R.inkA(.6))),
      );
    }
    if (_sending) {
      return Center(
        child: Text('جارٍ إرسال الرمز…',
            style: T.plex(12.5, FontWeight.w500, color: R.inkA(.6))),
      );
    }
    if (_error != null) {
      return Center(
        child: Text(_error!,
            textAlign: TextAlign.center,
            style: T.plex(12.5, FontWeight.w500, color: R.errorText)),
      );
    }
    return const SizedBox(height: 18);
  }
}
