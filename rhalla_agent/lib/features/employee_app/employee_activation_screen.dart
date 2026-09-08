import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'employee_qr_scan_screen.dart';
import 'employee_session.dart';

/// تفعيل تطبيق الموظف — الطبقات الثلاث في شاشتين.
///
///     الشاشة 1: رقم الهاتف + كود التفعيل  ⇦ تصريح الإدارة
///     الشاشة 2: رمز تحقّق من 4 أرقام      ⇦ ملكية الرقم
///     وربط الجهاز يقع في الخادم           ⇦ الجهاز المعتمد
///
/// ولا يُرسَل الرمز قبل نجاح الأولى: الخادم لا يرسل شيئاً لرقمٍ لا يملك
/// كوداً صحيحاً، فلا يصير التطبيق أداة إزعاج لأي رقم يُخمَّن.
class EmployeeActivationScreen extends ConsumerStatefulWidget {
  const EmployeeActivationScreen({super.key});

  @override
  ConsumerState<EmployeeActivationScreen> createState() =>
      _EmployeeActivationScreenState();
}

class _EmployeeActivationScreenState
    extends ConsumerState<EmployeeActivationScreen> with HardwareDigits {
  final _phone = TextEditingController();
  final _code = TextEditingController();
  late final _phoneFocus = AutoClearFocus(_phone);
  late final _codeFocus = AutoClearFocus(_code);

  bool _busy = false;
  String? _error;

  /// الانتقال إلى شاشة الرمز يقع بعد ردّ الخادم لا قبله.
  String? _maskedPhone;

  /// معرّفُ الطلب — يصل مع ردّ الخطوة الأولى في الطريقين.
  ///
  /// ⚠ وهو الطريقُ الوحيد لإكمال ما بدأ بمسح رمز: من مسح لا يعرف رقمَ
  /// الهاتف، وما عاد إليه رقمٌ **مقنَّع** لا يصلح للإرسال — وذلك متعمَّد،
  /// فمن صوّر رمزاً لا يخرج منه برقم موظّفٍ كامل.
  int? _activationId;

  /// اسمُ الموظف كما في القاعدة — يُعرض بعد المسح ليطمئنّ أنّه رمزُه هو.
  String? _employeeName;

  /// هل اختار الموظف الإدخال اليدويّ؟
  ///
  /// ⚠ ثلاثُ خطواتٍ لا اثنتان الآن: الاختيار، ثمّ الكود، ثمّ رمز التحقّق.
  /// والأولى تُتخطّى تلقائياً في طريق المسح.
  bool _manual = false;

  /// الرمز يُجمع من لوحة الأرقام المرسومة في التطبيق — نفس شاشة دخول الوكيل.
  ///
  /// و`HardwareDigits` يجعل كيبورد الكمبيوتر يكتب فيها على المحاكي: هذه
  /// الشاشة لا تحوي `TextField` للرمز أصلاً، فلا كيبورد نظام يفتح لها،
  /// والمفاتيح الصلبة تصلها عبر ذلك الـ mixin.
  String _otpCode = '';

  @override
  void dispose() {
    _phoneFocus.dispose();
    _codeFocus.dispose();
    _phone.dispose();
    _code.dispose();
    super.dispose();
  }

  /// إدخال رقم من لوحة الأرقام أو من كيبورد الكمبيوتر.
  void _push(String d) {
    // ⚠ ولا شيء قبل خطوةِ الرمز: حارسٌ ثانٍ مستقلٌّ عن الذي
    // في `HardwareDigits`، لأنّ أحدَهما يكفي واجتماعَهما لا يكلّف
    // شيئاً: ضغطةٌ في الخطوة الأولى كانت تبني رمزاً لا يراه
    // أحد، ثمّ يُرسَل من تلقائِه عند بلوغِه أربعةً.
    if (_maskedPhone == null) return;
    if (_busy || _otpCode.length >= 4) return;
    setState(() {
      _otpCode += d;
      _error = null;
    });
    if (_otpCode.length == 4) _verify();
  }

  void _pop() {
    if (_busy || _otpCode.isEmpty) return;
    setState(() => _otpCode = _otpCode.substring(0, _otpCode.length - 1));
  }

  @override
  void onHardwareDigit(String d) => _push(d);

  @override
  void onHardwareDelete() => _pop();

  @override
  Widget build(BuildContext context) {
    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: _maskedPhone != null
                ? 'أدخل رمز التحقق'
                : (_manual ? 'إدخال كود التفعيل' : 'الدخول كموظف'),
            onBack: () {
              // الرجوعُ خطوةً واحدة لا خروجاً من الشاشة كلِّها.
              if (_maskedPhone != null) {
                setState(() { _maskedPhone = null; _error = null; });
              } else if (_manual) {
                setState(() { _manual = false; _error = null; });
              } else {
                context.pop();
              }
            },
          ),
          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 20, R.padScreen, 40),
              children: [
                const Center(child: BrandLockup()),
                const SizedBox(height: 26),
                if (_maskedPhone != null)
                  ..._step2()
                else if (_manual)
                  ..._step1()
                else
                  ..._step0(),
              ],
            ),
          ),
        ],
      ),
    );
  }

  /* ───────────────── الخطوة 0 — اختيار الطريق ───────────────── */

  /*
   * ⚠ المسحُ أولاً، والإدخالُ اليدويّ باقٍ كاملاً تحته.
   *
   * كودُ التفعيل ثمانُ خانات من أبجديةٍ فيها حروفٌ وأرقام، يُملى على الهاتف
   * ويُكتب على لوحةٍ صغيرة تحت ضغط الوقت — وكلُّ خطأٍ فيه يُنقص من خمس
   * محاولاتٍ ثم يُحرق الكود. والمسحُ يُلغي الكتابةَ كلَّها.
   *
   * ولا يُلغي طبقةَ أمانٍ واحدة: بعد المسح يأتي رمزُ التحقّق إلى هاتف الموظف
   * كما كان، ثمّ يُربط الجهاز كما كان — أمرُ المالك (8 سبتمبر 2026):
   * «QR + تحقق الخادم + OTP الموظف + ربط الجهاز».
   */
  List<Widget> _step0() => [
        Text('تفعيل تطبيق الموظف',
            textAlign: TextAlign.center,
            style: T.kufi(17, FontWeight.w700)),
        const SizedBox(height: 8),
        Text(
          'امسح الرمز الظاهر على شاشة الوكيل، ثم أدخل رمز التحقق '
          'الذي يصلك على واتساب.',
          textAlign: TextAlign.center,
          style: T.plex(12.5, FontWeight.w400,
              color: R.inkA(.55), height: 1.8),
        ),
        const SizedBox(height: 26),

        Center(
          child: Container(
            width: 96,
            height: 96,
            decoration: BoxDecoration(
              color: R.primaryA(.08),
              shape: BoxShape.circle,
              border: Border.all(color: R.primaryA(.24)),
            ),
            child: Icon(Icons.qr_code_scanner_rounded,
                size: 44, color: R.primaryDark),
          ),
        ),
        const SizedBox(height: 26),

        if (_error != null) ...[
          _ErrorBox(message: _error!),
          const SizedBox(height: 16),
        ],

        PrimaryButton(
          label: 'مسح رمز التفعيل',
          loading: _busy,
          icon: const Icon(Icons.qr_code_scanner_rounded,
              size: 19, color: Colors.white),
          onPressed: _busy ? null : _scan,
        ),
        const SizedBox(height: 12),
        SecondaryButton(
          label: 'إدخال كود التفعيل يدوياً',
          icon: Icon(Icons.keyboard_alt_outlined,
              size: 18, color: R.primaryDark),
          onPressed: _busy
              ? null
              : () => setState(() { _manual = true; _error = null; }),
        ),
      ];

  /// يفتح الماسح، ثم يُرسل ما عاد به إلى الخادم.
  ///
  /// ⚠ ولا نجاحَ محلّيّ: لا شيء يُحفظ ولا شاشةٌ تتقدّم قبل أن يردّ الخادم.
  /// فالرمزُ نفسُه لا يقول شيئاً — من يقول إنه صالحٌ ولمن هو ومن أصدره هو
  /// الصفُّ في القاعدة. وتفعيلٌ يبدأ بلا شبكةٍ لا معنى له، لأنّ رمز التحقّق
  /// يُرسَل من الخادم أصلاً.
  Future<void> _scan() async {
    final token = await Navigator.of(context, rootNavigator: true).push<String>(
      MaterialPageRoute(builder: (_) => const EmployeeQrScanScreen()),
    );
    if (token == null || !mounted) return;

    setState(() { _busy = true; _error = null; });
    try {
      final deviceId = await ref.read(secureStoreProvider).deviceId();
      final env = await ref.read(apiClientProvider).post(
        '/device/employee/activation/qr',
        // ⚠ الرمزُ والجهاز فقط. لا رقمَ هاتفٍ ولا معرّفَ موظّفٍ ولا وكيل:
        // ما يُرسله التطبيق لا يُحدّد التبعية، والخادم يشتقّها من سجلّه.
        body: {'qr_token': token, 'device_id': deviceId},
      );
      if (!mounted) return;

      final row = env.row;
      setState(() {
        _busy = false;
        _maskedPhone = '${row?['masked_phone'] ?? ''}';
        _activationId = int.tryParse('${row?['activation_id'] ?? ''}');
        _employeeName = '${row?['employee_name'] ?? ''}'.trim().isEmpty
            ? null
            : '${row?['employee_name']}';
        _otpCode = '';
      });
    } on ApiFailure catch (e) {
      // ⚠ رسالةُ الخادم كما هي: هو وحده يعرف أانتهى الرمز أم أُلغي أم
      // مُسح على جهازٍ آخر، وكلُّ حالةٍ تحتاج من الموظف فعلاً مختلفاً.
      if (mounted) setState(() { _busy = false; _error = e.message; });
    } catch (_) {
      if (mounted) {
        setState(() {
          _busy = false;
          _error = 'تعذّر الاتصال بالخادم. تحقّق من الشبكة وأعد المسح.';
        });
      }
    }
  }

  /* ───────────────── الخطوة 1 ───────────────── */

  List<Widget> _step1() => [
        Text('أدخل رقم هاتفك وكود التفعيل',
            textAlign: TextAlign.center,
            style: T.kufi(17, FontWeight.w700)),
        const SizedBox(height: 8),
        Text('الكود يصدره لك الوكيل من تطبيقه.',
            textAlign: TextAlign.center,
            style: T.plex(12.5, FontWeight.w400,
                color: R.inkA(.55), height: 1.8)),
        const SizedBox(height: 22),

        _Field(
          label: 'رقم الهاتف',
          controller: _phone,
          focusNode: _phoneFocus,
          hint: '9XXXXXXXX',
          digits: true,
          maxLength: 9,
        ),
        const SizedBox(height: R.gapCard),
        _Field(
          label: 'كود التفعيل',
          controller: _code,
          focusNode: _codeFocus,
          hint: 'الكود المكوّن من 8 خانات',
          upper: true,
          maxLength: 12,
        ),

        if (_error != null) ...[
          const SizedBox(height: 14),
          _ErrorBox(message: _error!),
        ],

        const SizedBox(height: 22),
        PrimaryButton(
          label: 'متابعة التفعيل',
          loading: _busy,
          onPressed: _busy ? null : _requestOtp,
        ),
      ];

  Future<void> _requestOtp() async {
    final phone = Fmt.phoneForApi(_phone.text);
    final code = _code.text.trim().toUpperCase();

    if (!RegExp(r'^9\d{8}$').hasMatch(phone)) {
      setState(() => _error = 'رقم الهاتف يجب أن يكون 9 أرقام يبدأ بـ 9.');
      return;
    }
    if (code.isEmpty) {
      setState(() => _error = 'اكتب كود التفعيل.');
      return;
    }

    setState(() { _busy = true; _error = null; });
    try {
      final deviceId = await ref.read(secureStoreProvider).deviceId();
      final env = await ref.read(apiClientProvider).post(
        '/device/employee/activation/request',
        body: {'phone': phone, 'code': code, 'device_id': deviceId},
      );
      if (!mounted) return;
      setState(() {
        _busy = false;
        _maskedPhone = '${env.row?['masked_phone'] ?? ''}';
        // الطريقُ اليدويّ يعرف الرقم، فلا يحتاج معرّفَ الطلب.
        _activationId = null;
        _otpCode = '';
      });
    } on ApiFailure catch (e) {
      if (mounted) setState(() { _busy = false; _error = e.message; });
    } catch (_) {
      if (mounted) {
        setState(() {
          _busy = false;
          _error = 'تعذّر الاتصال بالخادم. تحقّق من الشبكة.';
        });
      }
    }
  }

  /* ───────────────── الخطوة 2 ───────────────── */

  List<Widget> _step2() => [
        Text('أدخل رمز التحقق',
            textAlign: TextAlign.center, style: T.kufi(17, FontWeight.w700)),
        const SizedBox(height: 10),
        Text('أدخل رمز التحقق المرسل عبر WhatsApp',
            textAlign: TextAlign.center,
            style: T.plex(12.5, FontWeight.w400,
                color: R.inkA(.55), height: 1.8)),
        const SizedBox(height: 4),
        // الرقم في سطره وحده وبـ LTR: رقمٌ داخل جملة عربية ينقلب ترتيبه.
        Directionality(
          textDirection: TextDirection.ltr,
          child: Text(_maskedPhone ?? '',
              textAlign: TextAlign.center,
              style: T.kufi(14, FontWeight.w700, color: R.primaryDark)),
        ),
        const SizedBox(height: 24),

        // ⚠ ومن جاء من المسح يُعرض له اسمُه: هو لم يكتب رقماً ولا كوداً،
        // فلولا الاسمُ لأدخل رمزاً وهو لا يدري لحسابِ من.
        if (_employeeName != null) ...[
          const SizedBox(height: 4),
          Text(_employeeName!,
              textAlign: TextAlign.center,
              style: T.kufi(13.5, FontWeight.w700, color: R.inkA(.72))),
          const SizedBox(height: 6),
        ],
        const SizedBox(height: 18),
        Center(
          child: OtpBoxes(value: _otpCode, length: 4, error: _error != null),
        ),
        const SizedBox(height: 24),
        NumericKeypad(onDigit: _push, onDelete: _pop),

        if (_error != null) ...[
          const SizedBox(height: 16),
          _ErrorBox(message: _error!),
        ],

        const SizedBox(height: 24),
        PrimaryButton(
          label: 'تفعيل',
          loading: _busy,
          onPressed: _busy ? null : _verify,
        ),
        const SizedBox(height: 10),
        TextButton(
          onPressed: _busy
              ? null
              : () => setState(() { _maskedPhone = null; _error = null; }),
          style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
          child: Text('تغيير الرقم أو الكود',
              style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
        ),
      ];

  Future<void> _verify() async {
    final otp = _otpCode;
    if (otp.length != 4) {
      setState(() => _error = 'أدخل الرمز المكوّن من 4 أرقام.');
      return;
    }

    setState(() { _busy = true; _error = null; });
    try {
      final deviceId = await ref.read(secureStoreProvider).deviceId();

      /*
       * ⚠ معرّفُ الطلب حين يكون، وإلا الرقم.
       *
       * فمن أدخل الكود يدوياً كتب رقمَه بيده، ومن مسح رمزاً لا يملكه —
       * والخادم يقبل أيَّهما ويصل إلى الصفّ نفسِه. والجهازُ يحرس
       * الاثنين: الرمزُ مربوطٌ بـ`device_hash` فلا يُكمَّل من غيره.
       */
      final env = await ref.read(apiClientProvider).post(
        '/device/employee/activation/verify',
        body: {
          if (_activationId != null) 'activation_id': _activationId
          else 'phone': Fmt.phoneForApi(_phone.text),
          'otp': otp,
          'device_id': deviceId,
        },
      );

      final token = '${env.row?['access_token'] ?? ''}';
      if (token.isEmpty) throw 'رد غير متوقّع من الخادم.';

      await ref.read(employeeAuthProvider.notifier).adopt(
            token,
            (env.row?['employee'] as Map?)?.cast<String, dynamic>() ?? const {},
          );
      // الراوتر يراقب حالة الموظف وينقله بنفسه — لا `go` هنا.
    } on ApiFailure catch (e) {
      if (mounted) setState(() { _busy = false; _error = e.message; });
    } catch (e) {
      if (mounted) setState(() { _busy = false; _error = '$e'; });
    }
  }
}

/* ───────────────── عناصر ───────────────── */

class _Field extends StatelessWidget {
  const _Field({
    required this.label,
    required this.controller,
    required this.focusNode,
    required this.hint,
    this.digits = false,
    this.upper = false,
    this.maxLength,
  });

  final String label;
  final TextEditingController controller;
  final FocusNode focusNode;
  final String hint;
  final bool digits;
  final bool upper;
  final int? maxLength;

  @override
  Widget build(BuildContext context) {
    final field = TextField(
      controller: controller,
      focusNode: focusNode,
      keyboardType: digits ? TextInputType.number : TextInputType.text,
      textCapitalization:
          upper ? TextCapitalization.characters : TextCapitalization.none,
      inputFormatters: [
        const WesternDigits(),
        if (digits)
          FilteringTextInputFormatter.digitsOnly
        else
          // أبجدية الكود: أرقام وحروف لاتينية كبيرة — والتحويل هنا كي لا
          // يفشل التفعيل لأن الموظف كتب بحروف صغيرة.
          FilteringTextInputFormatter.allow(RegExp(r'[0-9A-Za-z]')),
        if (maxLength != null) LengthLimitingTextInputFormatter(maxLength),
        if (upper) _UpperCase(),
      ],
      style: T.kufi(16, FontWeight.w700, spacing: upper ? 2 : 0),
      decoration: InputDecoration(
        isDense: true,
        border: InputBorder.none,
        counterText: '',
        hintText: hint,
        hintStyle: T.plex(13, FontWeight.w400, color: R.inkA(.42)),
      ),
    );

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(label, style: T.label),
          const SizedBox(height: 9),
          Directionality(textDirection: TextDirection.ltr, child: field),
        ],
      ),
    );
  }
}

/// يحوّل الكود إلى حروف كبيرة أثناء الكتابة.
class _UpperCase extends TextInputFormatter {
  @override
  TextEditingValue formatEditUpdate(
      TextEditingValue oldValue, TextEditingValue newValue) {
    final up = newValue.text.toUpperCase();
    if (up == newValue.text) return newValue;
    return TextEditingValue(text: up, selection: newValue.selection);
  }
}

class _ErrorBox extends StatelessWidget {
  const _ErrorBox({required this.message});

  final String message;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
        decoration: BoxDecoration(
          color: R.error.withValues(alpha: .07),
          border: Border.all(color: R.error.withValues(alpha: .26)),
          borderRadius: BorderRadius.circular(R.rRow),
        ),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Icon(Icons.error_outline_rounded, size: 17, color: R.errorText),
            const SizedBox(width: 9),
            Expanded(
              child: Text(message,
                  style: T.plex(12.5, FontWeight.w500,
                      color: R.errorText, height: 1.7)),
            ),
          ],
        ),
      );
}
