import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/transfers/transfer_qr.dart';

/// رمزُ استلام الحوالة — ما يُقبل وما يُرفض.
///
/// ⚠ هذا هو موضعُ الخطر: كاميرا في مكتب صرافةٍ ترى رموزَ فواتيرَ وملصقاتِ
/// منتجاتٍ وشبكاتِ واي فاي طوالَ اليوم. وماسحٌ يقبلها يفتح بحثاً عن حوالةٍ
/// لا وجودَ لها، فيقرأ الوكيلُ «لا نتيجة» ويظنّ إيصالَ الزبون معطوباً.
///
/// وليس أمناً: الرمزُ لا يحمل صلاحية — هو نسخةٌ من الأرقام المطبوعة بجانبه،
/// والتسليمُ بعده يمرّ بجلسة الوكيل وصلاحيته والتحوُّل الذرّيّ.

void main() {
  group('ما نكتبه', () {
    test('الحمولةُ تحمل الرقم وبادئتَنا', () {
      expect(transferQrPayload('13151-54-1'), 'RH-T:13151-54-1');
    });

    test('وتُقلَّم المسافات', () {
      expect(transferQrPayload('  99 '.replaceAll(' ', '')), 'RH-T:99');
    });

    test('وما نكتبه نقرؤه', () {
      for (final code in ['13151-54-1', '900123', 'AB-77-2', '5-1-9']) {
        expect(readTransferQr(transferQrPayload(code)), code,
            reason: 'ذهاباً وإياباً: $code');
      }
    });
  });

  group('ما نقبله', () {
    test('رمزُنا ببادئته', () {
      expect(readTransferQr('RH-T:13151-54-1'), '13151-54-1');
    });

    test('والبادئةُ لا تُميّز حالةَ الحرف', () {
      expect(readTransferQr('rh-t:13151-54-1'), '13151-54-1');
    });

    test('⚠ ورقمٌ مجرّدٌ بلا بادئة — إيصالٌ من نظامٍ آخر', () {
      // رفضُه يعني ماسحاً يعمل مع إيصالاتنا وحدَها، فيجرّبه الوكيل مع إيصالٍ
      // من التطبيق المكتبيّ فيفشل، ثم يكفّ عن استعماله مع إيصالاتنا أيضاً.
      expect(readTransferQr('13151-54-1'), '13151-54-1');
    });

    test('والمسافاتُ حولَه تُقلَّم', () {
      expect(readTransferQr('  RH-T:900123  '), '900123');
    });
  });

  group('⚠ ما نرفضه', () {
    test('رمزُ تفعيل الموظف — ٦٤ خانةً ستّ عشريّة', () {
      // ⚠ أهمُّ رفضٍ هنا: الرمزان يعيشان في التطبيق نفسِه، ومسحُ رمز تفعيلٍ
      // في شاشة التسليم كان سيفتح بحثاً عقيماً يُرسل الوكيل يبحث عن عطبٍ
      // في مكانٍ سليم.
      expect(readTransferQr('a' * 64), isNull);
      expect(readTransferQr('0123456789abcdef' * 4), isNull);
    });

    test('⚠ ويُرفض ولو حمل بادئتَنا', () {
      expect(readTransferQr('RH-T:${'f' * 64}'), isNull);
    });

    test('روابطُ الإعلانات', () {
      expect(readTransferQr('https://example.com/x'), isNull);
      expect(readTransferQr('http://a.b'), isNull);
    });

    test('وشبكاتُ الواي فاي', () {
      expect(readTransferQr('WIFI:S:MyNet;T:WPA;P:12345678;;'), isNull);
    });

    test('وبطاقاتُ vCard والنصوصُ الطويلة', () {
      expect(readTransferQr('BEGIN:VCARD\nFN:فلان\nEND:VCARD'), isNull);
      expect(readTransferQr('حوالة إلى محمد بمبلغ 500'), isNull);
    });

    test('والفارغ', () {
      expect(readTransferQr(''), isNull);
      expect(readTransferQr('   '), isNull);
      expect(readTransferQr('RH-T:'), isNull);
    });

    test('⚠ ورقمٌ قصيرٌ بلا بادئةٍ ولا شُرطة — باركودُ علبة عصير', () {
      // `12345` على علبةٍ في الدرج يُمسح سهواً فيفتح بحثاً عقيماً.
      expect(readTransferQr('12345'), isNull);
      expect(readTransferQr('7'), isNull);
    });

    test('ولكنّه يُقبل ببادئتنا — فهو رمزُنا يقيناً', () {
      expect(readTransferQr('RH-T:12345'), '12345');
    });

    test('ورقمٌ طويلٌ بلا بادئةٍ يُقبل — رقمُ حوالةٍ حقيقيّ', () {
      expect(readTransferQr('900123'), '900123');
    });

    test('والرموزُ الغريبة تُرفض', () {
      expect(readTransferQr('13151/54/1'), isNull);
      expect(readTransferQr('13151 54 1'), isNull);
      expect(readTransferQr('<script>'), isNull);
    });

    test('⚠ وما طال عن أربعين خانة', () {
      // رقمُ حوالةٍ لا يبلغ ذلك؛ وما بلغه فهو نصٌّ آخر.
      expect(readTransferQr('1' * 41), isNull);
      expect(readTransferQr('1' * 39), '1' * 39);
    });
  });
}
