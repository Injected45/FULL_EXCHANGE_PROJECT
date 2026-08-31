import 'package:flutter/services.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/format/fmt.dart';

/// فواصل الآلاف تُوضع أثناء الكتابة. الخطر الوحيد هنا محاسبي: لو وصلت
/// الفاصلة إلى الخادم لصار المبلغ خطأً — ولذلك يُثبَّت أن [Fmt.num_]
/// تزيلها، فكل مواضع قراءة المبلغ تمرّ بها.
void main() {
  const f = ThousandsGrouping();

  TextEditingValue at(String s, int caret) =>
      TextEditingValue(text: s, selection: TextSelection.collapsed(offset: caret));

  group('ThousandsGrouping', () {
    test('يجمّع الآلاف', () {
      expect(ThousandsGrouping.group('2500'), '2,500');
      expect(ThousandsGrouping.group('1234567'), '1,234,567');
      expect(ThousandsGrouping.group('100'), '100');
      expect(ThousandsGrouping.group(''), '');
    });

    test('لا يمسّ الكسر ولا يمنع مواصلة الكتابة', () {
      expect(ThousandsGrouping.group('2500.'), '2,500.');
      expect(ThousandsGrouping.group('2500.7'), '2,500.7');
      expect(ThousandsGrouping.group('1234567.325'), '1,234,567.325');
    });

    test('يُبقي أول نقطة فقط — «1.2.3» ليس مبلغاً', () {
      expect(ThousandsGrouping.group('1.2.3'), '1.23');
    });

    test('الفواصل السابقة لا تتراكم', () {
      expect(ThousandsGrouping.group('1,234,567'), '1,234,567');
      expect(ThousandsGrouping.group('1,2,3,4'), '1,234');
    });

    test('المؤشّر يبقى بعد آخر رقم كُتب', () {
      // الوكيل كتب «2500» فأصبحت «2,500»: المؤشّر بعد الرقم الرابع لا الثالث.
      final out = f.formatEditUpdate(at('250', 3), at('2500', 4));
      expect(out.text, '2,500');
      expect(out.selection.baseOffset, 5);
    });

    test('المؤشّر في وسط النص لا يقفز إلى آخره', () {
      // «1234567» والمؤشّر بعد الرقم الثالث ⇦ «1,234,567» والمؤشّر بعد «3».
      final out = f.formatEditUpdate(at('123456', 3), at('1234567', 3));
      expect(out.text, '1,234,567');
      expect(out.text.substring(0, out.selection.baseOffset), '1,23');
    });

    test('المبلغ يصل إلى الخادم بلا فواصل — الحارس المحاسبي', () {
      expect(Fmt.num_('2,500'), 2500.0);
      expect(Fmt.num_('1,234,567.325'), 1234567.325);
    });
  });
}
