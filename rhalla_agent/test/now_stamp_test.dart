import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/format/fmt.dart';

void main() {
  group('Fmt.nowStamp', () {
    test('الشكل dd/MM/yyyy HH:mm بنظام 24', () {
      expect(Fmt.nowStamp(),
          matches(RegExp(r'^\d{2}/\d{2}/\d{4} \d{2}:\d{2}$')));
    });

    test('لا AM ولا PM في أي طابع', () {
      // قرار المالك: الساعة 24 في كل موضع تظهر فيه في التطبيق.
      expect(Fmt.nowStamp(), isNot(contains('AM')));
      expect(Fmt.nowStamp(), isNot(contains('PM')));
    });

    test('لا رقم هنديّ واحد في الطابع', () {
      // 'ar' كانت ستُخرج ٣١/٠٨ — وهي ممنوعة في هذا التطبيق كلّه.
      expect(Fmt.nowStamp(), isNot(matches(RegExp('[٠-٩۰-۹]'))));
    });

    test('اليوم والشهر والساعة بخانتين', () {
      expect(Fmt.nowStamp(DateTime(2026, 8, 31, 9, 55)), '31/08/2026 09:55');
      expect(Fmt.nowStamp(DateTime(2026, 1, 5, 14, 7)), '05/01/2026 14:07');
    });

    test('منتصف الليل 00 والظهر 12 وآخر الليل 23', () {
      // في نظام 24 لا تنقلب الساعة: 0 تبقى 00 لا 12.
      expect(Fmt.nowStamp(DateTime(2026, 8, 31, 0, 5)), '31/08/2026 00:05');
      expect(Fmt.nowStamp(DateTime(2026, 8, 31, 12, 5)), '31/08/2026 12:05');
      expect(Fmt.nowStamp(DateTime(2026, 8, 31, 23, 59)), '31/08/2026 23:59');
    });
  });
}
