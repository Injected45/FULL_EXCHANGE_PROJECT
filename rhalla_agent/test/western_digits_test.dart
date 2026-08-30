import 'package:flutter/services.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/format/fmt.dart';

/// قاعدة المشروع: لا أرقام هندية في أي حقل. لوحة المفاتيح العربية تُدخل ٠١٢٣،
/// وكانت مرشِّحات الحقول تحذفها بصمت فيبدو للوكيل أنّ الكتابة لا تعمل.
void main() {
  TextEditingValue v(String s) =>
      TextEditingValue(text: s, selection: TextSelection.collapsed(offset: s.length));

  const f = WesternDigits();

  group('WesternDigits', () {
    test('يحوّل الأرقام العربية-الهندية', () {
      expect(WesternDigits.normalize('٠١٢٣٤٥٦٧٨٩'), '0123456789');
    });

    test('يحوّل الأرقام الفارسية', () {
      expect(WesternDigits.normalize('۰۱۲۳۴۵۶۷۸۹'), '0123456789');
    });

    test('الفاصلة العشرية العربية ٫ تصبح نقطة', () {
      expect(WesternDigits.normalize('١٢٫٥٠٠'), '12.500');
    });

    test('لا يمسّ ما هو غربيّ أو نصّاً عربياً', () {
      expect(WesternDigits.normalize('12.500'), '12.500');
      expect(WesternDigits.normalize('مصر'), 'مصر');
    });

    test('الطول محفوظ فلا يقفز مؤشّر الكتابة', () {
      final out = f.formatEditUpdate(v(''), v('٩٢٤٤٥٨٨١٧'));
      expect(out.text, '924458817');
      expect(out.selection.baseOffset, 9);
    });

    test('يمرّ النصّ الغربيّ كما هو بلا نسخ', () {
      final input = v('500');
      expect(identical(f.formatEditUpdate(v(''), input), input), isTrue);
    });
  });
}
