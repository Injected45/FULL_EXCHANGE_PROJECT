import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/send/currency_rates_screen.dart';

/*
 * ════════════════════════════════════════════════════════════════════════════
 *  لوحةُ أسعار العملات — المقلوبُ والحالاتُ والبحث
 * ════════════════════════════════════════════════════════════════════════════
 *
 * ⚠ المقلوبُ رقمٌ **يُقرأ ويُسعَّر به** أمام زبون، لا زينةَ عرض. فيُختبر
 * بالأسعار الحقيقية في القاعدة لا بأرقامٍ مخترَعة: 5.55 و5.30 (مصر)، 3.225
 * و2.5 (تونس)، 555.555 (السودان)، 12.5 (تشاد).
 */

void main() {
  group('مقلوبُ السعر', () {
    test('⚠ أربعُ خاناتٍ معنويةٍ لا أربعُ خاناتٍ عشرية', () {
      /*
       * هذا هو سببُ وجود الدالّة. تثبيتُ أربعِ خاناتٍ عشرية يعطي للسودان
       * «0.0018» — وقد ضاعت ثلاثُ خاناتٍ دالّة — بينما يعطي لتونس «0.3101»
       * كاملةً. فيُعرض رقمان بدقّتين مختلفتين في العمود نفسِه.
       */
      expect(inverseRate(555.555), '0.0018');
      expect(inverseRate(3.225), '0.3101');
    });

    test('العملاتُ الأضعف من الدينار — مصر', () {
      expect(inverseRate(5.55), '0.1802');
      expect(inverseRate(5.30), '0.1887');
    });

    test('تونس وتشاد', () {
      expect(inverseRate(2.5), '0.4');
      expect(inverseRate(12.5), '0.08');
    });

    test('⚠ تُقصّ الأصفارُ الزائدة — «0.4» لا «0.4000»', () {
      expect(inverseRate(2.0), '0.5');
      expect(inverseRate(4.0), '0.25');
      expect(inverseRate(1.0), '1');
    });

    test('⚠ سعرٌ صفرٌ أو سالبٌ لا يُقسَم عليه', () {
      // القسمةُ على صفرٍ في Dart تعطي `Infinity` لا استثناءً، فتُطبع
      // «Infinity» في وجه الوكيل مكانَ سعر.
      expect(inverseRate(0), '—');
      expect(inverseRate(-1), '—');
    });
  });

  group('حالةُ السطر', () {
    RateRow row(String status, {double? rate, String? day}) => RateRow(
          service: 'خدمة',
          rate: rate,
          status: status,
          pricedOn: day,
        );

    test('⚠ الحالةُ تأتي من الخادم ولا تُشتقّ من غياب الرقم', () {
      expect(row('NO_PRICE').status, 'NO_PRICE');
      expect(row('AMBIGUOUS').status, 'AMBIGUOUS');
    });

    test('⚠ لا يدخل اللوحةَ إلّا سعرٌ قابلٌ للتنفيذ', () {
      // أمرُ المالك (11 سبتمبر 2026): «أيُّ عملةٍ بدون سعرٍ امنع ظهورَها في
      // الشاشة … والسعرُ الظاهر في الحوالة الخارجية يجب أن يكون ظاهراً في
      // شاشة الأسعار».
      expect(row('PRICED', rate: 5.55).isDisplayable, isTrue);
      expect(row('NO_PRICE').isDisplayable, isFalse);
      expect(row('AMBIGUOUS').isDisplayable, isFalse);
    });

    test('⚠ والشرطان معاً — التناقضُ يُحسم بالإخفاء', () {
      // خادمٌ يعيد PRICED بلا سعر، أو سعراً مع حالةٍ مانعة: كلاهما تناقض،
      // وترجيحُ أحد طرفيه على لوحةِ أسعارٍ تسعيرٌ بالتخمين.
      expect(row('PRICED').isDisplayable, isFalse);
      expect(row('NO_PRICE', rate: 5.55).isDisplayable, isFalse);
    });

    test('⚠ والصفرُ ليس سعراً', () {
      // به يستلم المستفيد لا شيء، بينما الهامشُ (مسلَّم − صافي) يساوي صفراً
      // فيبدو سليماً — فحصُ الهامش وحدَه لا يمسك هذه الحالة.
      expect(row('PRICED', rate: 0).isDisplayable, isFalse);
      expect(row('PRICED', rate: -1).isDisplayable, isFalse);
    });
  });

  group('تقادمُ السعر', () {
    RateRow aged(int days) => RateRow(
          service: 'خدمة',
          rate: 5.55,
          status: 'PRICED',
          pricedOn: DateTime.now()
              .subtract(Duration(days: days))
              .toIso8601String()
              .substring(0, 10),
        );

    test('⚠ العتبةُ ١٨٠ يوماً — أحدثُ أسعار القاعدة (٧٤ يوماً) لا تُشعلها', () {
      // عتبةٌ قصيرة كانت ستُضيء كلَّ سطرٍ في اللوحة، وتحذيرٌ يعمّ الجميعَ
      // لا يقرؤه أحد.
      expect(aged(74).isStale, isFalse);
      expect(aged(179).isStale, isFalse);
    });

    test('⚠ وتُشعلها سنةٌ ونصف — وهو حالُ سعرِ بريدِ مصر فعلاً', () {
      expect(aged(600).isStale, isTrue);
    });

    test('بلا تاريخٍ لا يُدّعى تقادم', () {
      expect(
        const RateRow(service: 'خدمة', rate: 5.55, status: 'PRICED', pricedOn: null)
            .isStale,
        isFalse,
      );
      expect(
        const RateRow(service: 'خدمة', rate: 5.55, status: 'PRICED', pricedOn: 'ليس تاريخاً')
            .isStale,
        isFalse,
      );
    });
  });

  group('البحث', () {
    RateGroup egypt() => RateGroup(
          country: 'مصر',
          currency: 'الجنيه المصري',
          code: 'ج.م',
          isStrong: false,
          rows: const [
            RateRow(service: 'فودافون', rate: 5.55, status: 'PRICED', pricedOn: null),
            RateRow(service: 'حوالة بنكية', rate: 5.30, status: 'PRICED', pricedOn: null),
            RateRow(service: 'انستا باي', rate: 5.30, status: 'PRICED', pricedOn: null),
          ],
        );

    test('⚠ مطابقةُ الدولة تُبقي خدماتِها كلَّها', () {
      // من كتب «مصر» يريد لوحتَها كاملة، لا سطراً منها.
      expect(egypt().filtered('مصر')!.rows.length, 3);
      expect(egypt().filtered('الجنيه')!.rows.length, 3);
      expect(egypt().filtered('ج.م')!.rows.length, 3);
    });

    test('⚠ ومطابقةُ الخدمة تُبقي المطابقَ وحدَه', () {
      // ومن كتب «فودافون» يريد سطرَ فودافون، لا مصرَ بخدماتها الخمس.
      final f = egypt().filtered('فودافون')!;
      expect(f.rows.length, 1);
      expect(f.rows.first.service, 'فودافون');
      // والدولةُ تبقى في الترويسة كي لا يُقرأ السعرُ بلا وجهته.
      expect(f.country, 'مصر');
      expect(f.code, 'ج.م');
    });

    test('لا مطابقةَ ⇒ المجموعةُ تسقط كلُّها', () {
      expect(egypt().filtered('تركيا'), isNull);
    });
  });
}
