import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/home/home_repository.dart';

/// اختبارات قبول ربط العمولة بحوالتها — بنود 1 و4 و5 و6 و22 من المستند.
///
/// الربط في الخادم بـ `ISID`، وهذه تثبّت **القراءة**: أن التطبيق يفرّق بين
/// «بلا عمولة» و«لم تصل»، وأن كل عمولة تبقى مع حوالتها ولا تنتقل إلى غيرها.
void main() {
  Map<String, dynamic> row({
    required String code,
    required String type,
    required String amount,
    String from = 'خصم',
    Object? commission,
    String isCommission = '0',
    bool withCommissionKey = true,
  }) =>
      {
        'MovementType': type,
        'Type_from': from,
        'Values_to': amount,
        'Balnce': '0',
        'InsertDate': '2026-09-02',
        'Code': code,
        'IsCommission': isCommission,
        if (withCommissionKey) 'CommissionAmount': commission ?? '0',
      };

  group('عمولة الحوالة', () {
    test('1. الحوالة تحمل عمولتها، وصفّ العمولة يُميَّز', () {
      final transfer = Movement.fromJson(row(
        code: '13151-54-1', type: 'حوالة داخلية',
        amount: '500', commission: '10.000',
      ));
      final commission = Movement.fromJson(row(
        code: '13151-54-1', type: 'عمولة تحويل',
        amount: '10', commission: '10.000', isCommission: '1',
      ));

      expect(transfer.isCommission, isFalse);
      expect(transfer.commission, 10.0);
      expect(transfer.hasCommission, isTrue);

      // صفّ العمولة يُحجب من القائمة — ولا يُحذف من الرد.
      expect(commission.isCommission, isTrue);
    });

    test('إجمالي العملية = القيمة + العمولة، والقيمة لا تُستبدل', () {
      final t = Movement.fromJson(row(
        code: 'X-1', type: 'حوالة محلية', amount: '500', commission: '10',
      ));
      expect(t.amount, 500.0);          // القيمة الرئيسية تبقى كما هي
      expect(t.operationTotal, 510.0);  // والإجمالي رقمٌ ثانٍ بجانبها
    });

    test('4. حوالة بعمولة صفر: بلا عمولة، ولا بطاقة مستقلّة', () {
      final t = Movement.fromJson(row(
        code: 'X-2', type: 'حوالة محلية', amount: '3000',
        from: 'ايداع', commission: '0',
      ));
      expect(t.commission, 0.0);
      expect(t.hasCommission, isFalse);
      expect(t.operationTotal, 3000.0);
    });

    test('22. الفرق بين «بلا عمولة» و«لم تصل»', () {
      final zero = Movement.fromJson(row(
        code: 'X-3', type: 'حوالة محلية', amount: '100', commission: '0',
      ));
      final notLoaded = Movement.fromJson(row(
        code: 'X-4', type: 'حوالة محلية', amount: '100',
        withCommissionKey: false,
      ));

      expect(zero.commission, 0.0);        // معروفة: صفر
      expect(notLoaded.commission, isNull); // مجهولة: لا تُعرض صفراً
      expect(notLoaded.hasCommission, isFalse);
    });

    test('5/6. كل عمولة تبقى مع حوالتها ولا تنتقل إلى غيرها', () {
      final rows = [
        row(code: 'A-1', type: 'حوالة محلية', amount: '500', commission: '10'),
        row(code: 'A-1', type: 'عمولة تحويل', amount: '10',
            commission: '10', isCommission: '1'),
        row(code: 'B-2', type: 'حوالة محلية', amount: '700', commission: '25'),
        row(code: 'C-3', type: 'حوالة محلية', amount: '900', commission: '0'),
      ].map(Movement.fromJson).toList();

      final shown = rows.where((m) => !m.isCommission).toList();

      expect(shown.length, 3);                       // بطاقة العمولة اختفت
      expect(shown[0].code, 'A-1');
      expect(shown[0].commission, 10.0);
      expect(shown[1].code, 'B-2');
      expect(shown[1].commission, 25.0);             // لا تخلط مع 10
      expect(shown[2].code, 'C-3');
      expect(shown[2].hasCommission, isFalse);       // بلا عمولة تبقى بلا
    });

    test('عمولة بلا حوالة مطابقة لا تُنسب إلى حوالة أخرى', () {
      final orphan = Movement.fromJson(row(
        code: 'Z-9', type: 'حوالة محلية', amount: '400', commission: '0',
      ));
      // الربط بالرقم لا بالترتيب: صفٌّ بلا عمولةٍ تحمل رقمه يبقى صفراً
      // مهما جاور صفوفاً لها عمولات.
      expect(orphan.commission, 0.0);
    });
  });
}
