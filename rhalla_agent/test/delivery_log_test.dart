import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/transfers/delivery_log.dart';
import 'package:shared_preferences/shared_preferences.dart';

/// دفتر التسليم المحلّي. أخطر ما فيه **خطّ الأساس**: بدونه تعود حوالاتٌ
/// سُلّمت فعلاً لتظهر «بانتظار التسليم» بعد إعادة تثبيت، فتُسلَّم مرّتين.
void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  setUp(() => SharedPreferences.setMockInitialValues({}));

  Future<DeliveryLog> fresh() async {
    final log = DeliveryLog();
    // ننتظر قراءة التخزين في المُنشئ.
    await Future<void>.delayed(Duration.zero);
    return log;
  }

  test('أول تشغيل: كل ما وُجد يُخفى، فتبدأ الشاشة فارغة', () async {
    final log = await fresh();
    expect(log.state.ready, isFalse);

    await log.captureBaseline(['A1', 'A2', 'A3']);

    expect(log.state.ready, isTrue);
    for (final c in ['A1', 'A2', 'A3']) {
      expect(log.state.isHidden(c), isTrue, reason: 'قديمة ⇦ لا تُعرض');
    }
    expect(log.state.isHidden('B9'), isFalse, reason: 'جديدة ⇦ تُعرض');
  });

  test('خطّ الأساس يُلتقط مرّة واحدة ولا يبتلع الجديد', () async {
    final log = await fresh();
    await log.captureBaseline(['A1']);
    await log.captureBaseline(['A1', 'B2']); // نداء ثانٍ — يجب أن يُتجاهل
    expect(log.state.isHidden('B2'), isFalse);
  });

  test('التعليم نهائي — لا مسار يُعيده إلى الانتظار', () async {
    final log = await fresh();
    await log.captureBaseline(const []);

    await log.markDelivered('C7');
    expect(log.state.isDelivered('C7'), isTrue);
    expect(log.state.deliveredAt('C7'), isNotNull);

    // تعليمه ثانيةً لا يقلبه — لا يوجد في الدفتر ما يُزيل العلامة.
    await log.markDelivered('C7');
    expect(log.state.isDelivered('C7'), isTrue,
        reason: 'منع نهائي: لا تعود إلى بانتظار التسليم');
  });

  test('الحالة تبقى بعد إعادة الفتح', () async {
    final a = await fresh();
    await a.captureBaseline(['OLD']);
    await a.markDelivered('NEW1');

    final b = await fresh();
    expect(b.state.ready, isTrue);
    expect(b.state.isHidden('OLD'), isTrue);
    expect(b.state.isDelivered('NEW1'), isTrue);
  });

  test('مسح البيانات ⇦ خطّ أساس جديد وشاشة فارغة', () async {
    SharedPreferences.setMockInitialValues({});
    final log = await fresh();
    expect(log.state.ready, isFalse, reason: 'ينتظر التقاط خطّ أساس جديد');
    expect(log.state.isDelivered('NEW1'), isFalse);
  });

  test('المسح يبدأ دفتراً نظيفاً — لا يُعيد المُسلَّم إلى الانتظار', () async {
    final log = await fresh();
    await log.captureBaseline(const []);
    await log.markDelivered('X1');

    await log.resetAll();

    // ready=false ⇦ ستلتقط الشاشة خطّ أساس جديداً فتبدأ فارغة.
    expect(log.state.ready, isFalse);
    expect(log.state.delivered, isEmpty);
    // والأخطر: لا يعود X1 «بانتظار التسليم» فيُسلَّم مرّتين.
    expect(log.state.isDelivered('X1'), isFalse);

    await log.captureBaseline(['X1', 'X2']);
    expect(log.state.isHidden('X1'), isTrue);
    expect(log.state.isHidden('X2'), isTrue);
  });
}
