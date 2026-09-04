import 'dart:ui' as ui;

import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:pdf/widgets.dart' as pw;
import 'package:printing/printing.dart';
import 'package:share_plus/share_plus.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../branding/arabic_to_latin.dart';
import '../branding/brand_mark.dart';
import '../branding/branding_controller.dart';

/// مكوّنات فاتورة الحوالة، مشتركة بين «بانتظار التسليم» و«سلَّمتُها»
/// وشاشة «تمّت الحوالة».
///
/// وُحِّدت في ملفّ واحد بطلب المالك أن تكون الفواتير الثلاث **متطابقة**؛
/// ونسخةٌ ثانية من صفٍّ أو حاوية كانت ستفترق عن أختها عند أول تعديل،
/// فيقرأ الوكيل الفاتورة نفسها بشكلين حسب الشاشة التي فتحها منها.

/// ترويسة الفاتورة — باسم الشركة وشعارها، لا باسم «الرحالة».
///
/// قرار المالك (3 سبتمبر 2026): الفاتورة هي الورقة التي تصل يد العميل، فهي
/// أوّل ما يجب أن يحمل هوية الشركة. وميزة «هوية الشركة» أُضيفت لهذا بالضبط:
/// أن يشعر الوكيل أن التطبيق تطبيقه.
///
/// وهي تُصوَّر للطباعة والمشاركة، لذلك يُهيَّأ الشعار في الذاكرة قبل التصوير
/// (انظر [ReceiptTools.runReceiptAction]) — وإلا خرجت أول فاتورة بشعارٍ فارغ.
class ReceiptHeader extends ConsumerWidget {
  const ReceiptHeader({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final b = ref.watch(brandingControllerProvider).branding;

    final ar = b.displayName;
    // اسمٌ إنجليزيّ لم تحفظه الشركة يُشتقّ من اسمها العربي — لا يبقى السطر
    // فارغاً، ولا يحمل اسم شركةٍ أخرى.
    final en = (b.companyNameEn ?? '').trim().isNotEmpty
        ? b.companyNameEn!.trim()
        : ArabicToLatin.suggest(ar);

    return Row(
      children: [
        BrandMark(size: 34, color: R.primary),
        const SizedBox(width: 12),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(ar,
                  maxLines: 2,
                  style: T.kufi(16, FontWeight.w800, color: R.primaryDark)),
              if (en.isNotEmpty) ...[
                const SizedBox(height: 3),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(en,
                      maxLines: 2,
                      style: T.plex(10.5, FontWeight.w500,
                          color: R.primaryA(.75))),
                ),
              ],
            ],
          ),
        ),
      ],
    );
  }
}

class ReceiptRow extends StatelessWidget {
  const ReceiptRow(this.k, this.v,
      {super.key, this.ltr = false, this.strong = false, this.currency});

  final String k;
  final String v;
  final bool ltr;
  final bool strong;

  /// رمز العملة — يُوضع على **يسار** القيمة كسائر شاشات التطبيق،
  /// ولا يُكتب في الكود بل يأتي من الخادم.
  final String? currency;

  @override
  Widget build(BuildContext context) {
    final style = ltr
        ? T.kufi(strong ? 15 : 13.5, strong ? FontWeight.w700 : FontWeight.w600)
        : T.plex(strong ? 14 : 13, strong ? FontWeight.w700 : FontWeight.w600);

    Widget value = ltr
        ? Directionality(
            textDirection: TextDirection.ltr, child: Text(v, style: style))
        : Text(v, textAlign: TextAlign.end, style: style);

    final c = currency;
    if (c != null && c.isNotEmpty) {
      value = Directionality(
        textDirection: TextDirection.ltr,
        child: Row(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.baseline,
          textBaseline: TextBaseline.alphabetic,
          children: [
            Text(c, style: T.plex(12, FontWeight.w500, color: R.inkA(.55))),
            const SizedBox(width: 12),
            Text(v, style: style),
          ],
        ),
      );
    }

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 3),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // التسمية مرنة تلتفّ ولا تُقتطع: بعضها طويل («تاريخ ووقت التحويل»)
          // وقد يضيق الصفّ بخطٍّ أكبر في إعدادات الجهاز. والالتفاف أهون من
          // الاقتطاع — اسم الحقل لا يُبتر.
          Flexible(
            child: Text(k, style: T.plex(12, FontWeight.w400, color: R.inkA(.55))),
          ),
          const SizedBox(width: 10),
          Expanded(
            child:
                Align(alignment: AlignmentDirectional.centerEnd, child: value),
          ),
        ],
      ),
    );
  }
}

class BoxedField extends StatelessWidget {
  const BoxedField(
      {super.key,
      required this.icon,
      required this.label,
      required this.child});

  final IconData icon;
  final String label;
  final Widget child;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(12, 6, 12, 8),
        decoration: BoxDecoration(
          color: R.primaryA(.04),
          border: Border.all(color: R.primaryA(.22)),
          borderRadius: BorderRadius.circular(R.rRow),
        ),
        // الاتجاه مفروض صراحةً: الفاتورة تُرسم داخل سياق قد يكون LTR
        // (‏Directionality حول المبالغ)، فالاعتماد على المحيط يقلب الصفّ.
        child: Directionality(
          textDirection: TextDirection.rtl,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              // من يمين الحاوية: الأيقونة ثم العنوان، كاتجاه القراءة.
              Row(
                children: [
                  Icon(icon, size: 15, color: R.primary),
                  const SizedBox(width: 6),
                  Text(label,
                      style:
                          T.plex(11.5, FontWeight.w400, color: R.inkA(.55))),
                ],
              ),
              const SizedBox(height: 8),
              child,
            ],
          ),
        ),
      );
}

class MiniButton extends StatelessWidget {
  const MiniButton({
    super.key,
    required this.label,
    required this.icon,
    required this.onTap,
    this.filled = false,
  });

  final String label;
  final IconData icon;
  final VoidCallback onTap;
  final bool filled;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(R.rPill),
        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
          decoration: BoxDecoration(
            color: filled ? R.primary : Colors.white,
            border: Border.all(color: filled ? R.primary : R.inkA(.12)),
            borderRadius: BorderRadius.circular(R.rPill),
          ),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(icon, size: 14, color: filled ? Colors.white : R.inkA(.6)),
              const SizedBox(width: 5),
              Text(label,
                  style: T.plex(11.5, FontWeight.w600,
                      color: filled ? Colors.white : R.inkA(.6))),
            ],
          ),
        ),
      );
}

/// معاينة الطباعة — شاشتنا لا شاشة النظام، فلها سهم رجوع.
class PrintPreview extends StatelessWidget {
  const PrintPreview({super.key, required this.bytes, required this.name});

  final Uint8List bytes;

  /// يُستعمل في اسم ملفّ الـ PDF فقط.
  final String name;

  @override
  Widget build(BuildContext context) => Screen(
        child: Column(
          children: [
            RhallaAppBar(
              title: 'معاينة الطباعة',
              onBack: () => Navigator.of(context).pop(),
            ),
            Expanded(
              child: PdfPreview(
                build: (_) => bytes,
                pdfFileName: 'حوالة-$name.pdf',
                canChangePageFormat: false,
                canChangeOrientation: false,
                canDebug: false,
                allowPrinting: true,
                allowSharing: true,
                useActions: true,
                loadingWidget: Center(
                    child: CircularProgressIndicator(color: R.primary)),
              ),
            ),
          ],
        ),
      );
}

/// أدوات التصوير والطباعة والمشاركة، مشتركة بين شاشات الفاتورة.
///
/// **الطباعة والمشاركة تصوّران الفاتورة صورةً لا نصّاً.** السبب عربي بحت:
/// توليد PDF نصّي يتطلّب تشكيل الحروف العربية ووصلها داخل مكتبة الـ PDF،
/// وهو مصدر أعطال معروف (حروف منفصلة أو معكوسة). أما التصوير فيلتقط ما
/// يراه الوكيل حرفياً بخطّ التطبيق نفسه — فما يُطبع هو ما يُرى.
// معامل النوع W لا T: صنف الخطوط اسمه T، ولو سُمّي هنا T لحجبه داخل
// الـ mixin كلّه فصار T.plex خطأ ترجمة.
mixin ReceiptTools<W extends StatefulWidget> on State<W> {
  final GlobalKey receiptKey = GlobalKey();
  bool receiptBusy = false;

  /// تحميل شعار الشركة إلى الذاكرة قبل التصوير.
  ///
  /// التصوير يلتقط ما هو مرسوم في تلك اللحظة، وشعارٌ يأتي من الشبكة قد يكون
  /// لم يُفكّ ترميزه بعد — فتخرج **أول فاتورة** بشعارٍ فارغ أو بشعار الرحالة
  /// الاحتياطي، وهي بالضبط الورقة التي تصل يد العميل.
  ///
  /// الحاوية تُقرأ من الشجرة لا عبر `ref`: هذا mixin على `State` عادية،
  /// وبعض شاشات الفاتورة ليست `ConsumerState`.
  ///
  /// وفشلُ التحميل لا يمنع الطباعة: تُطبع بالشعار الاحتياطي، وفاتورةٌ بشعارٍ
  /// افتراضي خيرٌ من فاتورةٍ لا تُطبع.
  Future<void> _warmLogo() async {
    try {
      final url = ProviderScope.containerOf(context, listen: false)
          .read(brandingControllerProvider)
          .branding
          .logoUrl;
      if (url == null || !mounted) return;
      await precacheImage(NetworkImage(url), context);
    } catch (_) {
      // متروك عمداً.
    }
  }

  /// تصوير الفاتورة بدقّة الطباعة.
  Future<Uint8List?> captureReceipt() async {
    final ctx = receiptKey.currentContext;
    if (ctx == null) return null;
    final boundary = ctx.findRenderObject() as RenderRepaintBoundary?;
    if (boundary == null) return null;
    // 3× — أقل من ذلك يخرج نصّ الفاتورة مهترئاً على الورق.
    final image = await boundary.toImage(pixelRatio: 3);
    final data = await image.toByteData(format: ui.ImageByteFormat.png);
    return data?.buffer.asUint8List();
  }

  Future<void> runReceiptAction(
      Future<void> Function(Uint8List png) action) async {
    if (receiptBusy) return;
    setState(() => receiptBusy = true);
    try {
      await _warmLogo();
      final png = await captureReceipt();
      if (png == null) throw 'تعذّر تجهيز الفاتورة.';
      await action(png);
    } catch (e) {
      if (!mounted) return;
      receiptToast('$e');
    } finally {
      if (mounted) setState(() => receiptBusy = false);
    }
  }

  /// الطباعة تفتح **معاينة داخل التطبيق** لا واجهة نظام الطباعة.
  ///
  /// كانت `Printing.layoutPdf` تفتح شاشة النظام مباشرةً، وهي خارج تحكّمنا
  /// فلا نستطيع وضع سهم رجوع فيها — فوجد الوكيل نفسه عالقاً بلا مخرج.
  /// المعاينة هنا شاشتنا: لها شريط التطبيق وسهمه، وأزرار الطباعة
  /// والمشاركة داخلها.
  Future<void> printReceipt({required String name}) =>
      runReceiptAction((png) async {
        final doc = pw.Document();
        final img = pw.MemoryImage(png);
        doc.addPage(pw.Page(
          build: (_) => pw.Center(child: pw.Image(img, fit: pw.BoxFit.contain)),
        ));
        final bytes = await doc.save();
        if (!mounted) return;
        await Navigator.of(context, rootNavigator: true).push(
          MaterialPageRoute(
            builder: (_) => PrintPreview(bytes: bytes, name: name),
          ),
        );
      });

  Future<void> shareReceipt({required String name, required String text}) =>
      runReceiptAction((png) async {
        await SharePlus.instance.share(
          ShareParams(
            files: [
              XFile.fromData(png, name: 'حوالة-$name.png', mimeType: 'image/png'),
            ],
            text: text,
          ),
        );
      });

  void receiptToast(String msg) => ScaffoldMessenger.of(context)
    ..hideCurrentSnackBar()
    ..showSnackBar(SnackBar(
      content:
          Text(msg, style: T.plex(13, FontWeight.w500, color: Colors.white)),
      backgroundColor: R.primaryGradEnd,
      behavior: SnackBarBehavior.floating,
      margin: const EdgeInsets.fromLTRB(16, 0, 16, 24),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
    ));
}
