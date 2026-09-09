import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/employee_app/employee_ledger_view.dart';

/// كشفُ خزينة الموظف — طرفُ التطبيق.
///
/// المحسوبُ كلُّه في الخادم، فالمختبَرُ هنا ما يستطيع التطبيق أن يُفسده:
/// أن يقرأ `null` صفراً، أو أن يرسل مدىً غير الذي اختاره الموظف، أو أن
/// يخلط «لا قيمة في هذا العمود» بـ«قيمةٌ صفر».

void main() {
  group('قراءة الكشف', () {
    test('لا يُخلَط «لا قيمة» بـ«صفر»', () {
      final r = LedgerRow.fromJson({
        'kind': 'MOVE', 'type_label': 'حوالة سلَّمها',
        'in': null, 'out': 250, 'balance': 750, 'status': 'مثبتة',
      });

      // ⚠ عمودُ الداخل فارغٌ في حركة خروج — وصفرٌ فيه يُقرأ «دخل صفر»،
      // فيظهر في الكشف عمودان مملوءان لحركةٍ واحدة.
      expect(r.cashIn, isNull);
      expect(r.cashOut, 250);
      expect(r.balance, 750);
    });

    test('الرصيدُ الحالي يبقى null بلا وردية — لا صفراً', () {
      final l = Ledger.fromJson({
        'opening': 0, 'in': 0, 'out': 0, 'net': 0,
        'current': null, 'has_shift': false, 'rows': [],
      });

      // ⚠ صفرٌ يُقرأ «لا شيء عليك»، وهو غيرُ «لم يبدأ وردية».
      expect(l.current, isNull);
      expect(l.hasShift, isFalse);
    });

    test('والصفرُ الحقيقيّ يبقى صفراً', () {
      final l = Ledger.fromJson({
        'opening': 0, 'in': 0, 'out': 0, 'net': 0,
        'current': 0, 'has_shift': true, 'rows': [],
      });
      expect(l.current, 0);
      expect(l.hasShift, isTrue);
    });

    test('أسماءُ الأنواع تُقرأ من الخادم ولا تُترجَم محلياً', () {
      final l = Ledger.fromJson({
        'types': {'CASH_RECEIVED': 'نقد مستلَم', 'EXPENSE': 'مصروف'},
        'rows': [],
      });
      expect(l.types['CASH_RECEIVED'], 'نقد مستلَم');
      expect(l.types['EXPENSE'], 'مصروف');
    });

    test('ونوعٌ لا يعرفه التطبيق يظهر باسمه من الخادم لا برمزه', () {
      final r = LedgerRow.fromJson(
          {'kind': 'MOVE', 'type': 'SOMETHING_NEW', 'type_label': 'نوعٌ جديد'});
      expect(r.typeLabel, 'نوعٌ جديد');
    });

    test('المعكوسةُ تُقرأ غيرَ محسوبة', () {
      final r = LedgerRow.fromJson({
        'kind': 'MOVE', 'in': 100, 'balance': 500,
        'counted': false, 'reversed': true, 'status': 'معكوسة',
      });
      expect(r.counted, isFalse);
      expect(r.reversed, isTrue);
    });

    test('والحركةُ العاديّة محسوبةٌ ولو غاب المفتاح', () {
      // ⚠ الغيابُ يعني «محسوبة»: خادمٌ أقدم لا يُرسل المفتاح، وقراءتُه
      // «غير محسوبة» كانت ستشطب الكشفَ كلَّه.
      final r = LedgerRow.fromJson({'kind': 'MOVE', 'in': 100, 'balance': 500});
      expect(r.counted, isTrue);
    });

    test('صفُّ الإقفال يحمل المتوقَّع والمعدود والفرق', () {
      final r = LedgerRow.fromJson({
        'kind': 'CLOSING', 'type_label': 'إقفال الوردية', 'status': 'عجز',
        'expected': 1000, 'actual': 970, 'difference': -30, 'balance': 1000,
      });
      expect(r.isClosing, isTrue);
      expect(r.expected, 1000);
      expect(r.actual, 970);
      expect(r.difference, -30);
    });

    test('وردٌّ فارغٌ لا يُسقط الشاشة', () {
      final l = Ledger.fromJson(const {});
      expect(l.rows, isEmpty);
      expect(l.opening, 0);
      expect(l.current, isNull);
      expect(l.types, isEmpty);
    });
  });

  group('المدى المرسَل إلى الخادم', () {
    test('«الكل» لا يرسل تاريخاً', () {
      final p = const LedgerQuery(range: LedgerRange.all).params;
      expect(p.containsKey('from'), isFalse);
      expect(p.containsKey('to'), isFalse);
    });

    test('«اليوم» يرسل حدّين', () {
      final p = const LedgerQuery(range: LedgerRange.day).params;
      expect(p['from'], isNotNull);
      expect(p['to'], isNotNull);
      expect(p['from'], p['to'], reason: 'اليومُ يومٌ واحد');
    });

    test('التاريخُ بصيغة YYYY-MM-DD بأرقامٍ غربية', () {
      final p = const LedgerQuery(range: LedgerRange.month).params;
      // ⚠ كلُّ رقمٍ في هذا التطبيق غربيّ — والخادم يحلّل هذا الشكل وحدَه.
      expect(RegExp(r'^\d{4}-\d{2}-\d{2}$').hasMatch('${p['from']}'), isTrue,
          reason: 'from = ${p['from']}');
      expect(RegExp(r'^\d{4}-\d{2}-\d{2}$').hasMatch('${p['to']}'), isTrue);
    });

    test('«من — إلى» يرسل ما اختاره الموظف حرفاً بحرف', () {
      final q = LedgerQuery(
        range: LedgerRange.custom,
        custom: DateTimeRange(
          start: DateTime(2026, 3, 5),
          end: DateTime(2026, 4, 17),
        ),
      );
      expect(q.params['from'], '2026-03-05');
      expect(q.params['to'], '2026-04-17');
    });

    test('و«من — إلى» بلا اختيارٍ بعد لا يرسل مدىً مخترعاً', () {
      final p = const LedgerQuery(range: LedgerRange.custom).params;
      expect(p.containsKey('from'), isFalse);
      expect(p.containsKey('to'), isFalse);
    });

    test('النوعُ يُرسل حين يُختار وحدَه', () {
      expect(const LedgerQuery().params.containsKey('type'), isFalse);
      expect(const LedgerQuery(type: 'EXPENSE').params['type'], 'EXPENSE');
    });

    test('ورفعُ الفلتر يزيله فعلاً', () {
      // ⚠ `copyWith(type: null)` لا يمكنه التمييز بين «لا تغيّر» و«امحُ»،
      // فالمحوُ عَلَمٌ صريح — وبلا ذلك يبقى الفلتر لاصقاً إلى أن تُغلق الشاشة.
      final q = const LedgerQuery(type: 'EXPENSE').copyWith(clearType: true);
      expect(q.type, isNull);
      expect(q.params.containsKey('type'), isFalse);
    });

    test('والمدى يُغيَّر بلا أن يسقط النوع', () {
      final q = const LedgerQuery(type: 'EXPENSE')
          .copyWith(range: LedgerRange.day);
      expect(q.type, 'EXPENSE');
      expect(q.range, LedgerRange.day);
    });

    test('واستعلامان متساويان مفتاحٌ واحد للمزوّد', () {
      // ⚠ بلا `==` يُنشئ `family` مزوّداً جديداً لكل بناء، فيُنادى الخادم
      // مع كل إطار.
      expect(const LedgerQuery(range: LedgerRange.day, type: 'EXPENSE'),
          const LedgerQuery(range: LedgerRange.day, type: 'EXPENSE'));
      expect(const LedgerQuery(range: LedgerRange.day).hashCode,
          const LedgerQuery(range: LedgerRange.day).hashCode);
    });
  });
}
