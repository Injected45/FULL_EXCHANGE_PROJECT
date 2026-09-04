import 'dart:typed_data';

import 'package:flutter/services.dart' show rootBundle;
import 'package:pdf/pdf.dart';
import 'package:pdf/widgets.dart' as pw;

import '../../core/format/fmt.dart';
import '../home/home_repository.dart';

/// كشف حساب مطبوع — جدولٌ كامل بأعمدته، لا صورةً عن الشاشة.
///
/// فاتورة الحوالة تُطبع بالتقاط صورة للبطاقة ثم وضعها في صفحة (انظر
/// `ReceiptTools`)، وهي حيلةٌ تصلح لبطاقةٍ واحدة تُرى كاملةً على الشاشة.
/// والكشف لا يصلح لها: صفوفه بالمئات، وتمتدّ على صفحات، ولا بدّ أن يتكرّر
/// رأس الجدول في كل صفحة. فيُبنى هنا نصّاً حقيقياً في الـ PDF — يُنتقى منه
/// النصّ ويُبحث فيه، ووزنُ الملفّ كسرٌ من وزن الصور.
///
/// ولا حزمة جديدة: `pdf` و`printing` مستعملتان أصلاً في الفواتير، والخط
/// مضمَّنٌ في التطبيق. الحجم لا يزيد.
class StatementPdf {
  /// نطاق الكشف — يظهر في عنوان الملفّ وفي رأس الصفحة.
  ///
  /// الثلاثة هي مرشِّحات شاشة كشف الحساب نفسها، فما يُطبع هو ما يُرى.
  static const kAll = 'الكل';
  static const kCredit = 'الوارد';
  static const kDebit = 'الصادر';
  static const kCancelled = 'الملغاة';

  /// الخطّ يُحمَّل مرّة ويُحتفظ به: تحميله لكل كشف يعيد قراءة ملفّ من
  /// الأصول ويؤخّر فتح المعاينة بلا سبب.
  static pw.Font? _regular;
  static pw.Font? _bold;

  static Future<void> _loadFonts() async {
    if (_regular != null) return;
    _regular = pw.Font.ttf(
        await rootBundle.load('assets/fonts/IBMPlexSansArabic-Regular.ttf'));
    _bold = pw.Font.ttf(
        await rootBundle.load('assets/fonts/IBMPlexSansArabic-SemiBold.ttf'));
  }

  /// يبني الكشف ويعيد بايتاته.
  ///
  /// [rows] كما تعرضها الشاشة بعد الترشيح — فالمطبوع هو المعروض حرفياً.
  static Future<Uint8List> build({
    required List<Movement> rows,
    required String scope,
    required String currency,
    required String companyName,
    String? companyNameEn,
    String? accountLabel,
  }) async {
    await _loadFonts();

    final credits = rows.where((m) => m.isCredit).fold<double>(0, (s, m) => s + m.amount);
    final debits = rows.where((m) => !m.isCredit).fold<double>(0, (s, m) => s + m.amount);

    // الرصيد المعروض هو رصيد أحدث حركة — وهو ما تعرضه الشاشة نفسها. ولا
    // يُحسب هنا بجمعٍ أو طرح: حسابُ رصيدٍ في التطبيق يعني رقماً ثانياً قد
    // يفترق عمّا في المنظومة، والكشف يعرض ما تقوله المنظومة لا ما يستنتجه.
    final closing = rows.isEmpty ? null : rows.first.balance;

    final doc = pw.Document(
      title: 'كشف حساب — $scope',
      theme: pw.ThemeData.withFont(base: _regular!, bold: _bold!),
    );

    doc.addPage(
      pw.MultiPage(
        pageFormat: PdfPageFormat.a4,
        margin: const pw.EdgeInsets.fromLTRB(24, 26, 24, 30),
        textDirection: pw.TextDirection.rtl,
        header: (ctx) => ctx.pageNumber == 1
            ? _head(scope, currency, companyName, companyNameEn, accountLabel,
                rows.length, credits, debits, closing)
            : pw.SizedBox(height: 0),
        footer: (ctx) => pw.Container(
          alignment: pw.Alignment.center,
          margin: const pw.EdgeInsets.only(top: 8),
          child: pw.Text(
            'صفحة ${ctx.pageNumber} من ${ctx.pagesCount}',
            style: pw.TextStyle(fontSize: 8, color: PdfColors.grey600),
          ),
        ),
        build: (ctx) => [_table(rows)],
      ),
    );

    return doc.save();
  }

  static pw.Widget _head(
    String scope,
    String currency,
    String companyName,
    String? companyNameEn,
    String? accountLabel,
    int count,
    double credits,
    double debits,
    double? closing,
  ) {
    final now = DateTime.now();
    final stamp = '${now.year}-${_pad2(now.month)}-${_pad2(now.day)}'
        '  ${_pad2(now.hour)}:${_pad2(now.minute)}';

    return pw.Column(
      crossAxisAlignment: pw.CrossAxisAlignment.stretch,
      children: [
        // هوية الشركة لا هوية الرحالة — كما في الفواتير تماماً: الورقة
        // التي يخرجها الوكيل تحمل اسمه.
        pw.Text(companyName,
            style: pw.TextStyle(fontSize: 15, fontWeight: pw.FontWeight.bold)),
        if (companyNameEn != null && companyNameEn.trim().isNotEmpty)
          pw.Directionality(
            textDirection: pw.TextDirection.ltr,
            child: pw.Text(companyNameEn,
                style: pw.TextStyle(fontSize: 9, color: PdfColors.grey700)),
          ),
        pw.SizedBox(height: 10),
        pw.Row(
          mainAxisAlignment: pw.MainAxisAlignment.spaceBetween,
          crossAxisAlignment: pw.CrossAxisAlignment.end,
          children: [
            pw.Column(
              crossAxisAlignment: pw.CrossAxisAlignment.start,
              children: [
                pw.Text('كشف حساب · $scope',
                    style: pw.TextStyle(
                        fontSize: 12, fontWeight: pw.FontWeight.bold)),
                if (accountLabel != null && accountLabel.trim().isNotEmpty) ...[
                  pw.SizedBox(height: 3),
                  pw.Text(accountLabel,
                      style: pw.TextStyle(fontSize: 8.5, color: PdfColors.grey700)),
                ],
              ],
            ),
            pw.Column(
              crossAxisAlignment: pw.CrossAxisAlignment.end,
              children: [
                pw.Text('تاريخ الإصدار: $stamp',
                    style: pw.TextStyle(fontSize: 8.5, color: PdfColors.grey700)),
                pw.SizedBox(height: 3),
                pw.Text('عدد الحركات: ${Fmt.count(count)}',
                    style: pw.TextStyle(fontSize: 8.5, color: PdfColors.grey700)),
              ],
            ),
          ],
        ),
        pw.SizedBox(height: 10),
        pw.Container(
          padding: const pw.EdgeInsets.symmetric(horizontal: 10, vertical: 7),
          decoration: pw.BoxDecoration(
            color: PdfColors.grey100,
            borderRadius: pw.BorderRadius.circular(4),
          ),
          child: pw.Row(
            mainAxisAlignment: pw.MainAxisAlignment.spaceBetween,
            children: [
              _sum('إجمالي الوارد', credits, currency),
              _sum('إجمالي الصادر', debits, currency),
              // الرصيد يغيب حين لا حركة — ولا يُطبع صفراً: صفرٌ يُقرأ رصيداً.
              if (closing != null) _sum('الرصيد', closing, currency),
            ],
          ),
        ),
        pw.SizedBox(height: 12),
      ],
    );
  }

  static pw.Widget _sum(String label, double value, String currency) =>
      pw.Row(
        crossAxisAlignment: pw.CrossAxisAlignment.center,
        children: [
          pw.Text('$label: ',
              style: pw.TextStyle(fontSize: 8.5, color: PdfColors.grey700)),
          // الرمز يسار الرقم كما في كل شاشات التطبيق، والمقطع LTR لأن الرقم
          // لا يُعاد ترتيبه في فقرة عربية.
          //
          // ⚠ والرمز نفسه يعود RTL داخله، وإلا طُبع «ل.د» بدل «د.ل»: حزمة
          // `pdf` تورّث اتجاه المقطع إلى كل نصّ فيه، فتقلب حروف الكلمة
          // العربية. (في فلاتر لا يقع هذا لأن كل `Text` فقرةٌ مستقلّة.)
          pw.Directionality(
            textDirection: pw.TextDirection.ltr,
            child: pw.Row(
              mainAxisSize: pw.MainAxisSize.min,
              children: [
                pw.Directionality(
                  textDirection: pw.TextDirection.rtl,
                  child: pw.Text(currency,
                      style: pw.TextStyle(
                          fontSize: 9.5, fontWeight: pw.FontWeight.bold)),
                ),
                pw.SizedBox(width: 3),
                pw.Text(Fmt.money(value),
                    style: pw.TextStyle(
                        fontSize: 9.5, fontWeight: pw.FontWeight.bold)),
              ],
            ),
          ),
        ],
      );

  static pw.Widget _table(List<Movement> rows) {
    // الأعمدة السبعة: ما طلبه المالك (مدين · دائن · رصيد · المنفّذ · التاريخ)
    // ومعها البيان ورقم الحوالة — وبدونهما لا يُعرف أيّ حركةٍ يخصّ الرقم.
    const w = {
      0: pw.FlexColumnWidth(2.0),   // التاريخ
      1: pw.FlexColumnWidth(3.1),   // البيان
      2: pw.FlexColumnWidth(2.1),   // رقم الحوالة
      3: pw.FlexColumnWidth(1.9),   // المنفّذ
      4: pw.FlexColumnWidth(1.9),   // مدين
      5: pw.FlexColumnWidth(1.9),   // دائن
      6: pw.FlexColumnWidth(2.0),   // الرصيد
    };

    return pw.Table(
      columnWidths: w,
      border: pw.TableBorder.symmetric(
        inside: const pw.BorderSide(color: PdfColors.grey300, width: .4),
        outside: const pw.BorderSide(color: PdfColors.grey400, width: .6),
      ),
      children: [
        // `repeat: true` يعيد رأس الجدول في كل صفحة — وبدونه تصير الصفحة
        // الثانية أرقاماً بلا عناوين.
        pw.TableRow(
          repeat: true,
          decoration: const pw.BoxDecoration(color: PdfColors.grey200),
          children: [
            _Th('التاريخ'),
            _Th('البيان'),
            _Th('رقم الحوالة'),
            _Th('المنفّذ'),
            _Th('مدين'),
            _Th('دائن'),
            _Th('الرصيد'),
          ],
        ),
        for (var i = 0; i < rows.length; i++)
          pw.TableRow(
            // تظليل كل صفٍّ ثانٍ: سبعة أعمدة بلا تظليل تُقرأ بصعوبة، وتنتقل
            // العين إلى سطرٍ غير سطرها عند نهاية الصفّ.
            decoration: i.isOdd
                ? const pw.BoxDecoration(color: PdfColors.grey50)
                : null,
            children: [
              _Td(rows[i].date.split(' ').first, ltr: true),
              _Td(Fmt.localName(rows[i].title)),
              _Td(rows[i].code.isEmpty ? '—' : rows[i].code, ltr: true),
              // «المنفّذ» يبقى «—» حين لا سجلّ نسبة، ولا يُملأ بالوكيل تخميناً:
              // حركةٌ أنشأها فرعٌ في المنظومة ليست من تنفيذه.
              _Td(rows[i].executedBy.isEmpty ? '—' : rows[i].executedBy),
              _Td(rows[i].isCredit ? '' : Fmt.money(rows[i].amount), ltr: true),
              _Td(rows[i].isCredit ? Fmt.money(rows[i].amount) : '', ltr: true),
              _Td(Fmt.money(rows[i].balance), ltr: true, bold: true),
            ],
          ),
      ],
    );
  }

  static String _pad2(int n) => n.toString().padLeft(2, '0');
}

class _Th extends pw.StatelessWidget {
  _Th(this.text);

  final String text;

  @override
  pw.Widget build(pw.Context context) => pw.Padding(
        padding: const pw.EdgeInsets.symmetric(horizontal: 4, vertical: 5),
        child: pw.Text(text,
            style: pw.TextStyle(fontSize: 8, fontWeight: pw.FontWeight.bold)),
      );
}

class _Td extends pw.StatelessWidget {
  _Td(this.text, {this.ltr = false, this.bold = false});

  final String text;
  final bool ltr;
  final bool bold;

  @override
  pw.Widget build(pw.Context context) {
    final child = pw.Text(
      text,
      style: pw.TextStyle(
        fontSize: 7.8,
        fontWeight: bold ? pw.FontWeight.bold : pw.FontWeight.normal,
      ),
    );

    return pw.Padding(
      padding: const pw.EdgeInsets.symmetric(horizontal: 4, vertical: 4),
      child: ltr
          ? pw.Directionality(textDirection: pw.TextDirection.ltr, child: child)
          : child,
    );
  }
}
