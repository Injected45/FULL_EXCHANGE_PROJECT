import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/send/send_repository.dart';

/// اشتقاق فرع الاستلام من المدينة — الدالة الحقيقية لا نسخة عنها.

void main() {
  // عيّنة من القاعدة الحقيقية: البيضاء 1، بنغازي 2، طبرق 8 بفرعين.
  final branches = [
    const Ref2(5, 'فرع البيضاء', cityId: 1),
    const Ref2(9, 'فرع بنغازي', cityId: 2),
    const Ref2(21, 'فرع طبرق - بوبكر انجكشن', cityId: 8),
    const Ref2(12, 'فرع طبرق', cityId: 8),
  ];

  group('اشتقاق الفرع', () {
    test('مدينة بفرع واحد تعطيه', () {
      expect(resolveDeliveryBranch(branches, 2)!.name, 'فرع بنغازي');
    });

    test('مدينة بفرعين تعطي الأصغر معرّفاً لا الأول في القائمة', () {
      // ترتيب الخادم يضع «بوبكر انجكشن» أولاً؛ الاختيار يجب أن يكون
      // ثابتاً لا تابعاً لترتيب وصول الصفوف.
      expect(resolveDeliveryBranch(branches, 8)!.id, 12);
    });

    test('مدينة بلا فرع تقع على أول فرع في القائمة', () {
      // 62 من 79 مدينة ليبية لا فرع لها — لا يجوز أن ترجع null،
      // فالخادم يشترط branch_id ويردّ 404 على فرعٍ غير موجود.
      expect(resolveDeliveryBranch(branches, 999), isNotNull);
      expect(resolveDeliveryBranch(branches, 999)!.id, 5);
    });

    test('قائمة فارغة لا تعطي وجهة مخترَعة', () {
      expect(resolveDeliveryBranch(const [], 1), isNull);
    });

    test('Ref2.branch يقرأ CityID من الخادم', () {
      final b = Ref2.branch({'ID': 12, 'BName': 'فرع طبرق', 'CityID': 8});
      expect(b.cityId, 8);
      // غيابه لا يُسقط التحليل — يصير صفراً فلا يطابق مدينةً.
      expect(Ref2.branch({'ID': 1, 'BName': 'س'}).cityId, 0);
    });
  });
}
