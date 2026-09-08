import 'package:flutter/material.dart';
import 'package:mobile_scanner/mobile_scanner.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';

/// 64 خانةً ستّ عشريّة — نفسُ ما يتحقّق منه الخادم قبل أن يسأل القاعدة.
final _qrShape = RegExp(r'^[0-9a-f]{64}$');

/// هل هذا الرمزُ رمزَنا؟
///
/// ⚠ الفحصُ هنا **قبل الشبكة** لا بعدها. فالكاميرا ترى في الشارع رموزَ
/// فواتيرَ وشبكاتِ واي فاي وروابطَ إعلانات، وأيُّها لو أُرسل إلى الخادم
/// لعاد بخطأ «رمز غير صالح» — فيظنّ الموظف أن رمزَه هو المعطوب. ثمّ إن
/// خمسَ محاولاتٍ من هذا النوع تصطدم بحدّ المعدّل فتُعطّل تفعيلاً سليماً.
///
/// ولا يُقاس عليه أمان: الخادم يُعيد الفحصَ نفسَه ولا يثق بشيءٍ يصله.
bool isEmployeeQrPayload(String raw) =>
    _qrShape.hasMatch(raw.trim().toLowerCase());

/// ماسحُ رمز تفعيل الموظف.
///
/// يُعيد **الرمزَ نصّاً** إلى الشاشة التي فتحته، ولا يتحدّث إلى الخادم بنفسه:
/// من يملك خطوةَ التفعيل يملكها كاملةً، فلا يصير للتفعيل مساران.
///
/// ⚠ ولا يقرأ إلا رمزاً بشكلِ رمزِنا. فالكاميرا ترى في الشارع رموزَ فواتيرَ
/// وشبكاتِ واي فاي وروابطَ إعلانات، وأيُّها لو أُرسل إلى الخادم لعاد بخطأ
/// «رمز غير صالح» — فيظنّ الموظف أن رمزَه هو المعطوب. فالرفضُ هنا صامتٌ:
/// الماسح يبقى مفتوحاً حتى يقع على رمزٍ يعرفه.
class EmployeeQrScanScreen extends StatefulWidget {
  const EmployeeQrScanScreen({super.key});

  @override
  State<EmployeeQrScanScreen> createState() => _EmployeeQrScanScreenState();
}

class _EmployeeQrScanScreenState extends State<EmployeeQrScanScreen> {
  final _controller = MobileScannerController(
    detectionSpeed: DetectionSpeed.noDuplicates,
    facing: CameraFacing.back,
    formats: const [BarcodeFormat.qrCode],
  );

  /// ⚠ حارسُ «مرّةً واحدة».
  ///
  /// `onDetect` يُستدعى لكلِّ إطارٍ فيه رمز — عشراتِ المرّات في الثانية.
  /// وبدون هذا الحارس يُغلق الماسحُ نفسَه عشر مرّات، فيسقط من فوقه ما لا
  /// شيءَ تحته، ويُطلَب من الخادم رمزُ تحقّقٍ عشرَ مرّات فيصطدم بحدّ المعدّل.
  bool _done = false;

  bool _torch = false;

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  void _onDetect(BarcodeCapture capture) {
    if (_done) return;

    for (final b in capture.barcodes) {
      final raw = (b.rawValue ?? '').trim().toLowerCase();
      if (raw.isEmpty) continue;
      if (!isEmployeeQrPayload(raw)) continue;

      _done = true;
      _controller.stop();
      Navigator.of(context).pop(raw);
      return;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.black,
      body: Stack(
        fit: StackFit.expand,
        children: [
          MobileScanner(
            controller: _controller,
            onDetect: _onDetect,
            /*
             * ⚠ ورفضُ إذن الكاميرا ليس عُطلاً يُعرض كخطأٍ تقنيّ.
             *
             * فالموظف قد يرفض الإذن عن قصد، والإدخالُ اليدويّ للكود ما زال
             * طريقاً كاملاً. فتُقال له الجملةُ التي تُعيده إليه، لا رسالةُ
             * استثناءٍ لا يفهمها ولا يستطيع فعل شيء حيالها.
             */
            errorBuilder: (context, error) => _Blocked(
              message: switch (error.errorCode) {
                MobileScannerErrorCode.permissionDenied =>
                  'لم يُسمح للتطبيق باستخدام الكاميرا.\n'
                      'يمكنك السماح من إعدادات الهاتف، أو إدخال الكود يدوياً.',
                MobileScannerErrorCode.unsupported =>
                  'هذا الجهاز لا يدعم مسح الرموز.\nأدخل الكود يدوياً.',
                _ => 'تعذّر تشغيل الكاميرا.\nأدخل الكود يدوياً.',
              },
              onBack: () => Navigator.of(context).pop(),
            ),
          ),

          // النافذة: تعتيمٌ حول مربّعٍ شفّاف يقول للموظف أين يضع الرمز.
          const _Cutout(),

          SafeArea(
            child: Column(
              children: [
                _bar(context),
                const Spacer(),
                Padding(
                  padding: const EdgeInsets.fromLTRB(24, 0, 24, 34),
                  child: Column(
                    children: [
                      // ⚠ ولا رسالةَ خطأٍ هنا: الرفضُ صامت (رمزٌ ليس رمزَنا)،
                      // وما عداه يُردّ من الخادم فيُعرض في شاشة التفعيل بعد
                      // إغلاق الماسح — لا في شاشةٍ تُغلق قبل أن تُقرأ.
                      Text(
                        'وجّه الكاميرا إلى الرمز الظاهر على شاشة الوكيل',
                        textAlign: TextAlign.center,
                        style: T.kufi(14, FontWeight.w600, color: Colors.white),
                      ),
                      const SizedBox(height: 8),
                      Text(
                        'يبقى الرمز صالحاً عشر دقائق من إصداره.',
                        textAlign: TextAlign.center,
                        style: T.plex(12, FontWeight.w400,
                            color: Colors.white70, height: 1.7),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _bar(BuildContext context) => Padding(
        padding: const EdgeInsets.fromLTRB(8, 8, 8, 0),
        child: Row(
          children: [
            IconButton(
              onPressed: () => Navigator.of(context).pop(),
              icon: const Icon(Icons.arrow_forward_rounded,
                  color: Colors.white, size: 24),
              tooltip: 'رجوع',
            ),
            Expanded(
              child: Text('مسح رمز التفعيل',
                  textAlign: TextAlign.center,
                  style: T.kufi(16, FontWeight.w700, color: Colors.white)),
            ),
            // ⚠ الإضاءة يدويّة لا تلقائية: الرمز يُمسح من شاشةِ هاتفٍ مضيئة
            // غالباً، وإشعالُ الكشّاف عليها يغسلها فلا تُقرأ. فيبقى للموظف
            // إن كان الرمز مطبوعاً على ورق في مكانٍ مظلم.
            IconButton(
              onPressed: () {
                _controller.toggleTorch();
                setState(() => _torch = !_torch);
              },
              icon: Icon(
                _torch ? Icons.flash_on_rounded : Icons.flash_off_rounded,
                color: Colors.white,
                size: 22,
              ),
              tooltip: 'الإضاءة',
            ),
          ],
        ),
      );
}

/// تعتيمٌ حول نافذةٍ مربّعة في الوسط.
class _Cutout extends StatelessWidget {
  const _Cutout();

  @override
  Widget build(BuildContext context) {
    final side = MediaQuery.sizeOf(context).width * .68;
    return IgnorePointer(
      child: Center(
        child: Container(
          width: side,
          height: side,
          decoration: BoxDecoration(
            border: Border.all(color: Colors.white.withValues(alpha: .9), width: 3),
            borderRadius: BorderRadius.circular(R.rCard),
          ),
        ),
      ),
    );
  }
}

class _Blocked extends StatelessWidget {
  const _Blocked({required this.message, required this.onBack});

  final String message;
  final VoidCallback onBack;

  @override
  Widget build(BuildContext context) => ColoredBox(
        color: Colors.black,
        child: Center(
          child: Padding(
            padding: const EdgeInsets.all(30),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(Icons.no_photography_rounded,
                    size: 46, color: Colors.white.withValues(alpha: .7)),
                const SizedBox(height: 18),
                Text(message,
                    textAlign: TextAlign.center,
                    style: T.kufi(14.5, FontWeight.w600,
                        color: Colors.white, height: 1.8)),
                const SizedBox(height: 24),
                SizedBox(
                  width: 220,
                  child: PrimaryButton(label: 'إدخال الكود يدوياً', onPressed: onBack),
                ),
              ],
            ),
          ),
        ),
      );
}
