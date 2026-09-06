import 'package:flutter_test/flutter_test.dart';

import 'package:rhalla_support/main.dart';

/// اختبارٌ صغير على تطبيع العنوان.
///
/// وهو ما يكتبه موظّفٌ بيده في أوّل تشغيل — فأخطاؤه المتوقَّعة (بلا مخطَّط،
/// بشرطةٍ زائدة، بـ `/support` مكتوبةٍ ظنّاً أنها لازمة) يجب أن تُقبل لا أن
/// تُوقف التشغيل عند أوّل حرف.
void main() {
  group('Settings.normalize', () {
    test('يُضاف المخطَّط حين يُنسى', () {
      expect(Settings.normalize('192.168.1.10:8000'),
          'http://192.168.1.10:8000');
    });

    test('يُحترم https حين يُكتب', () {
      expect(Settings.normalize('https://rhalla.online'),
          'https://rhalla.online');
    });

    test('تُحذف الشرطة الزائدة', () {
      expect(Settings.normalize('http://x.local:8000///'), 'http://x.local:8000');
    });

    test('يُحذف /support لأنه يُضاف وحده', () {
      expect(Settings.normalize('http://x.local:8000/support'),
          'http://x.local:8000');
      expect(Settings.normalize('x.local:8000/support/'), 'http://x.local:8000');
    });

    test('الفراغ يبقى فراغاً فيُرفض في الشاشة', () {
      expect(Settings.normalize('   '), '');
    });
  });
}
