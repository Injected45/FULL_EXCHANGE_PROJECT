import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/employee_app/employee_session.dart';

/*
 * ══════════════════════════════════════════════════════════════════════════
 *  ⚠⚠ نبضةٌ لا تُغيّر شيئاً يجب ألّا تُوقظ أحداً
 * ══════════════════════════════════════════════════════════════════════════
 *
 * بلاغُ المالك (10 سبتمبر 2026): «في تطبيق الموظف الصفحات غير مستقرّة… أفتح
 * إنشاء حوالة وأبدأ أكتب البيانات تُقفل ولا تدعني أُكمل».
 *
 * والسلسلة كانت: نبضُ `me` كلَّ اثنتي عشرة ثانية ⇦ حالةٌ **جديدة** تُسنَد ولو
 * لم يتغيّر فيها حرف ⇦ `routerProvider` يراقب الحالة كاملةً فيُعاد بناؤه ⇦
 * وهو يبني `GoRouter` نفسَه ⇦ مُوجِّهٌ جديد بكومةِ تنقّلٍ جديدة ⇦ كلُّ شاشةٍ
 * مدفوعة تُغلق. كلُّ اثنتي عشرة ثانية، وبيدِ الموظف نموذجُ حوالةِ زبون.
 *
 * ⚠ ولا يظهر هذا في `flutter analyze` ولا في اختبارٍ يبني شاشةً واحدة: هو
 * سلوكُ **تركيبٍ** بين مؤقّتٍ ومزوّدٍ ومُوجِّه، ولا يُرى إلّا بالانتظار اثنتي
 * عشرة ثانية أمام الشاشة — وهو ما لا يفعله أحد إلّا المستخدم.
 *
 * فيُحرَس عند جذره: المساواةُ بالقيمة. وهي شرطٌ لازم — إن سقطت، عاد العطب
 * حتماً مهما كان `select` دقيقاً في المُوجِّه، لأن الحالةَ ستبدو متغيّرةً في
 * كلّ نبضة لكلّ من يقرؤها.
 */

/// نفسُ ما يُعيده الخادم من `device/employee/me` — تُبنى منه نسختان.
Map<String, dynamic> _payload({
  bool paused = false,
  List<String> permissions = const ['CREATE_TRANSFER', 'VIEW_OWN_TRANSFERS'],
  int posId = 7,
}) =>
    {
      'employee': {'id': 12, 'name': 'محمد علي', 'phone': '0912345678'},
      'active_point_of_sale_id': posId,
      'points_of_sale': [
        {'id': 7, 'name': 'شبّاك ١'},
        {'id': 8, 'name': 'شبّاك ٢'},
      ],
      'permissions': permissions,
      'open_shift': {
        'id': 3,
        'opening_cash': 250.5,
        'started_at': '2026-09-10 08:00:00',
      },
      'paused': paused,
      'pause_message': paused ? 'أوقفَ وكيلُك الخدمةَ مؤقتاً.' : null,
    };

EmployeeAuthState _state(Map<String, dynamic> j) => EmployeeAuthState(
      status: EmpSessionStatus.signedIn,
      profile: EmployeeProfile.fromJson(j),
    );

void main() {
  group('⚠⚠ استقرارُ جلسة الموظف — نبضةٌ بلا تغيير لا تُنشئ حالةً جديدة', () {
    test('ملفّان من نفس الاستجابة متساويان', () {
      /*
       * ⚠ وهذا هو بيتُ القصيد: `fromJson` تبني في كل مرّة قوائمَ جديدة
       * (الصلاحيات، نقاط البيع) وكائناتٍ جديدة (الوردية). ومقارنةُ المراجع
       * تجعلها «مختلفة» أبداً — فلولا المقارنةُ بالمحتوى لما تساوت نبضتان
       * متطابقتان قطّ.
       */
      expect(_state(_payload()), equals(_state(_payload())));
      expect(_state(_payload()).hashCode, _state(_payload()).hashCode);
    });

    test('وليسا الكائنَ نفسَه — فالمساواةُ بالقيمة لا بالمرجع', () {
      final a = _state(_payload());
      final b = _state(_payload());
      expect(identical(a, b), isFalse,
          reason: 'لو كانا الكائنَ نفسَه لما أثبت الفحصُ شيئاً');
      expect(a, equals(b));
    });

    test('⚠ وتغيُّرُ الإيقاف يُغيّر الحالة — وإلّا لم تظهر شاشةُ التجميد', () {
      expect(_state(_payload()), isNot(equals(_state(_payload(paused: true)))));
    });

    test('⚠ وسحبُ صلاحية يُغيّرها — وإلّا بقي بابٌ مفتوحاً بعد إغلاقه', () {
      expect(
        _state(_payload()),
        isNot(equals(_state(_payload(permissions: const ['CREATE_TRANSFER'])))),
      );
    });

    test('وتبديلُ نقطة البيع يُغيّرها', () {
      expect(_state(_payload()), isNot(equals(_state(_payload(posId: 8)))));
    });

    test('وحالتان بحالتَي جلسةٍ مختلفتين ليستا سواء', () {
      final p = EmployeeProfile.fromJson(_payload());
      expect(
        const EmployeeAuthState(status: EmpSessionStatus.signedOut),
        isNot(equals(EmployeeAuthState(
            status: EmpSessionStatus.signedIn, profile: p))),
      );
    });

    test('والملفُّ الفارغ يساوي الفارغ — حالةُ ما قبل القراءة', () {
      expect(EmployeeAuthState.initial,
          equals(const EmployeeAuthState(status: EmpSessionStatus.unknown)));
    });
  });
}
