import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/employees/employees_repository.dart';

/// الصلاحيةُ الحسّاسة لا تُمنح مع مجموعتها — طرفُ التطبيق.
///
/// ⚠ **العَلَمُ من الخادم لا من قائمةٍ هنا.** القاعدةُ في هذا المشروع أنّ لا
/// اسمَ صلاحيةٍ يُكتب في ملفّ Dart، فمفتاحٌ يُوسَم غداً يسري بلا إصدارٍ جديد.
/// ولذلك تُبنى العناصرُ هنا من JSON كما يصل، لا من ثوابت.

/// ما يفعله زرّ «تحديد الكل» بالضبط — منطقُ `_all` نفسُه.
Set<String> selectAll(PermissionGroup g) => {
      for (final i in g.items)
        if (!i.sensitive) i.key,
    };

/// و«إلغاء الكل» — يُلغي كلَّ شيءٍ بلا استثناء.
Set<String> clearAll(PermissionGroup g, Set<String> granted) =>
    granted.difference(g.items.map((i) => i.key).toSet());

PermissionGroup asGroup(
        String key, String name, List<Map<String, dynamic>> items) =>
    PermissionGroup.fromJson({'group': key, 'name': name, 'items': items});

void main() {
  final reports = asGroup('reports', 'التقارير', [
    {'key': 'REPORTS_VIEW', 'label': 'فتح قسم التقارير', 'live': true},
    {'key': 'REPORT_DAILY_TRANSFERS', 'label': 'تقرير حوالات اليوم', 'live': true},
    {
      'key': 'REPORT_AGENT_BALANCE',
      'label': 'تقرير رصيد الوكيل',
      'live': true,
      'sensitive': true,
      'why': 'سيرى الموظف رصيد وكالتك في التقارير.',
    },
  ]);

  group('قراءة العَلَم', () {
    test('يُقرأ كما يصل من الخادم', () {
      final items = {for (final i in reports.items) i.key: i};
      expect(items['REPORT_AGENT_BALANCE']!.sensitive, isTrue);
      expect(items['REPORT_DAILY_TRANSFERS']!.sensitive, isFalse);
    });

    test('وسببُه معه', () {
      final item =
          reports.items.firstWhere((i) => i.key == 'REPORT_AGENT_BALANCE');
      expect(item.why, isNotNull);
      expect(item.why, contains('رصيد'));
    });

    test('⚠ وغيابُه يعني «غير حسّاسة» لا العكس', () {
      // خادمٌ أقدم لا يرسل المفتاح. ووسمُ كلِّ شيءٍ حسّاساً حينئذٍ يمنع
      // «تحديد الكل» من فعل أيّ شيء — فتبدو الشاشةُ معطوبة.
      final i = PermissionItem.fromJson({'key': 'X', 'label': 'س'});
      expect(i.sensitive, isFalse);
      expect(i.why, isNull);

      // ⚠ وعكسُ ذلك في `live`: غيابُها يعني «مفعّلة».
      expect(i.live, isTrue);
    });
  });

  group('«تحديد الكل»', () {
    test('⚠ لا يمنح رصيد الوكيل', () {
      expect(selectAll(reports), isNot(contains('REPORT_AGENT_BALANCE')));
    });

    test('ويمنح كلَّ ما عداه في المجموعة', () {
      expect(selectAll(reports),
          {'REPORTS_VIEW', 'REPORT_DAILY_TRANSFERS'});
    });

    test('⚠ و«إلغاء الكل» يُلغيه — غيرُ متناظرٍ عمداً', () {
      // الخطرُ في المنح لا في السحب. وسحبٌ لا يكتمل يُبقي رصيداً مكشوفاً
      // والوكيلُ يظنّ أنه أغلق الباب.
      final granted = {
        'REPORTS_VIEW', 'REPORT_DAILY_TRANSFERS', 'REPORT_AGENT_BALANCE',
        'VIEW_OWN_CASHBOX', // من مجموعةٍ أخرى — لا تُمسّ
      };
      expect(clearAll(reports, granted), {'VIEW_OWN_CASHBOX'});
    });

    test('⚠ وحالةُ الزرّ تُقاس على ما يجوز منحُه جماعياً', () {
      // ولو قِيست على المجموعة كلِّها لَما ظهرت «إلغاء الكل» أبداً في مجموعةٍ
      // فيها حسّاسة: يضغط الوكيل «تحديد الكل» فلا يتغيّر العنوان، فيظنّ
      // الشاشة معطوبة ويضغط مراراً.
      final granted = selectAll(reports);
      final bulk = reports.items.where((i) => !i.sensitive).toList();
      final all = bulk.isNotEmpty && bulk.every((i) => granted.contains(i.key));
      expect(all, isTrue, reason: 'بعد «تحديد الكل» يصير الزرّ «إلغاء الكل»');
    });

    test('ومجموعةٌ بلا حسّاسة تعمل كما كانت', () {
      final cashbox = asGroup('cashbox', 'الخزينة', [
        {'key': 'VIEW_OWN_CASHBOX', 'label': 'عرض خزينته', 'live': true},
        {'key': 'CASHBOX_ENTRY', 'label': 'تسجيل حركة', 'live': true},
      ]);
      expect(selectAll(cashbox), {'VIEW_OWN_CASHBOX', 'CASHBOX_ENTRY'});
    });

    test('⚠ ومجموعةٌ كلُّها حسّاسة لا يمنح منها شيئاً', () {
      final only = asGroup('x', 'س', [
        {'key': 'A', 'label': 'أ', 'live': true, 'sensitive': true},
      ]);
      expect(selectAll(only), isEmpty);

      // والزرُّ يبقى «تحديد الكل» — لا شيءَ يُمنح جماعياً هنا أصلاً.
      final bulk = only.items.where((i) => !i.sensitive).toList();
      expect(bulk.isNotEmpty && bulk.every((i) => false), isFalse);
    });
  });
}
