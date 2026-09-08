import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/employee_app/employee_qr_scan_screen.dart';

/// حارسُ ما يُرسَل إلى الخادم من الكاميرا.
///
/// ⚠ الفحصُ الحقيقيّ في الخادم، وهذا لا يحلّ محلّه. لكنّ الماسح يقرأ كلَّ
/// رمزٍ يقع في مجاله — رمزَ فاتورةٍ على الطاولة، أو ملصقَ واي فاي على الجدار
/// خلف الوكيل — وإرسالُ أيٍّ منها يستهلك محاولةً من خمسٍ ثمّ يصطدم بحدّ
/// المعدّل، فيُعطَّل تفعيلٌ سليم بسبب رمزٍ لا علاقة له بنا.
void main() {
  const good =
      'a3f1b2c4d5e6f708192a3b4c5d6e7f8091a2b3c4d5e6f708192a3b4c5d6e7f80';

  group('شكلُ رمز التفعيل', () {
    test('يقبل رمزاً بالشكل الصحيح', () {
      expect(isEmployeeQrPayload(good), isTrue);
      expect(good.length, 64);
    });

    test('ويقبله مع مسافاتٍ حوله وبأحرفٍ كبيرة', () {
      // بعض الماسحات تُعيد الرمز بحروفٍ كبيرة، والمسافةُ تأتي من نسخٍ ولصق.
      expect(isEmployeeQrPayload('  ${good.toUpperCase()}  '), isTrue);
    });

    test('⚠ ويرفض ما ليس رمزَنا — لا يُرسَل شيءٌ منه إلى الخادم', () {
      for (final bad in [
        '',
        '   ',
        'https://example.com/promo',
        'WIFI:S:Rhalla;T:WPA;P:12345678;;',
        'tel:+218911234567',
        '{"employee_id":7,"agent_id":104}',   // محاولةُ حقنِ تبعية
        good.substring(0, 63),                // أقصر بخانة
        '${good}0',                           // أطول بخانة
        good.replaceFirst('a', 'g'),          // حرفٌ خارج الستّ عشريّ
        good.replaceFirst('a', ' '),
      ]) {
        expect(isEmployeeQrPayload(bad), isFalse, reason: 'قُبل: «$bad»');
      }
    });

    test('⚠ ورمزٌ يحمل بياناً لا يُقبل ولو كان صحيح الطول', () {
      // نصٌّ من 64 محرفاً لكنه ليس ستّ عشريّاً — لو قُبل لصار الرمزُ وعاءً
      // يُكتب فيه، وهو ما مُنع صراحةً: لا معرّفَ موظّفٍ ولا وكيلٍ في الرمز.
      final payload = 'employee=7;agent=104;perm=ALL'.padRight(64, 'x');
      expect(payload.length, 64);
      expect(isEmployeeQrPayload(payload), isFalse);
    });
  });
}
