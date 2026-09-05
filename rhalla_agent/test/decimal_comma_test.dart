import 'package:flutter/services.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/format/fmt.dart';

/// الفاصلة العشرية في حقول المال.
///
/// العطب الذي أوجد هذه الاختبارات: لوحة المفاتيح الرقمية في أندرويد تضع على
/// مفتاح الفصل العشري **رمز لغة الجهاز**، وجهاز الوكيل `ar-LY` — فالمفتاح
/// يُخرج `,` لا `.`. فكان «2,5» يصير **25**: خطأٌ بعشرة أضعاف في مبلغ مال،
/// صامتٌ، ويبدو للوكيل أن التطبيق حذف الفاصلة.
///
/// ولذلك تكتب هذه الاختبارات **حرفاً حرفاً** عبر سلسلة المنسّقات كما يفعل
/// `TextField`، لا باستدعاء دالةٍ على النصّ كاملاً: العطب كان في التتابع —
/// كل حرف يمرّ على السلسلة وحالةُ الحقل السابقة جزءٌ من المدخل — وفحصٌ
/// يمرّر «2,5» دفعةً واحدة كان سيمرّ ولا يرى شيئاً.
void main() {
  /// يحاكي الكتابة على لوحة المفاتيح: حرفٌ في كل مرّة، عبر كل المنسّقات.
  String type(String keys, [List<TextInputFormatter>? fs]) {
    final chain = fs ?? moneyInputFormatters;
    var v = TextEditingValue.empty;
    for (final ch in keys.split('')) {
      final off =
          v.selection.baseOffset < 0 ? v.text.length : v.selection.baseOffset;
      var next = TextEditingValue(
        text: v.text.substring(0, off) + ch + v.text.substring(off),
        selection: TextSelection.collapsed(offset: off + 1),
      );
      for (final f in chain) {
        next = f.formatEditUpdate(v, next);
      }
      v = next;
    }
    return v.text;
  }

  /// لصقٌ دفعةً واحدة — مسارٌ آخر في `DecimalComma`.
  String paste(String text) {
    var v = TextEditingValue(
      text: text,
      selection: TextSelection.collapsed(offset: text.length),
    );
    for (final f in moneyInputFormatters) {
      v = f.formatEditUpdate(TextEditingValue.empty, v);
    }
    return v.text;
  }

  group('الفاصلة المكتوبة فاصلةٌ عشرية', () {
    test('«2,5» تعني اثنين ونصفاً لا خمسة وعشرين', () {
      expect(type('2,5'), '2.5');
      expect(Fmt.num_(type('2,5')), 2.5);
    });

    test('النقطة تعمل كما هي — لم تُكسر بإصلاح الفاصلة', () {
      expect(type('2.5'), '2.5');
      expect(Fmt.num_(type('2.5')), 2.5);
    });

    test('الفاصلة العربية ٫ تُقرأ عشرية كذلك', () {
      expect(type('٢٫٥'), '2.5');
    });

    test('فاصلةٌ ثانية تُبتلع ولا تُبدَّل', () {
      // «2.5,» لو صارت «2.5.» لألصق التجميعُ الكسرين فصار «2.55» —
      // أي رقماً لم يكتبه أحد.
      expect(type('2,5,'), '2.5');
      expect(type('2.5,7'), '2.57');
    });

    test('الكسر يبقى مع تجميع الآلاف', () {
      expect(type('2500,75'), '2,500.75');
      expect(Fmt.num_(type('2500,75')), 2500.75);
    });

    test('التجميع نفسه لم يتأثّر', () {
      expect(type('2500'), '2,500');
      expect(type('1234567'), '1,234,567');
    });

    test('الكسر الثالث يمرّ — الدينار 1000 درهم', () {
      expect(type('1000,325'), '1,000.325');
      expect(Fmt.num_(type('1000,325')), 1000.325);
    });
  });

  group('اللصق', () {
    test('«2,500» ملصوقةً تبقى تجميعاً — هي شكلُ العرض نفسه', () {
      expect(paste('2,500'), '2,500');
      expect(Fmt.num_(paste('2,500')), 2500);
    });

    test('«2,5» ملصوقةً عشرية — لا ثلاث خانات بعد الفاصلة', () {
      expect(paste('2,5'), '2.5');
    });

    test('«1,234,567.89» ملصوقةً تمرّ كما هي', () {
      expect(paste('1,234,567.89'), '1,234,567.89');
      expect(Fmt.num_(paste('1,234,567.89')), 1234567.89);
    });
  });

  group('العرض يُظهر الكسر', () {
    test('خانتان دائماً وثالثة متى حملت قيمة', () {
      expect(Fmt.money(2.5), '2.50');
      expect(Fmt.money(0.5), '0.50');
      expect(Fmt.money(1000.325), '1,000.325');
      expect(Fmt.money(2.05), '2.05');
    });
  });
}
