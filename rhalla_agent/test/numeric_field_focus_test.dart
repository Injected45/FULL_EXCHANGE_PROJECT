import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/format/fmt.dart';

/// قاعدتان تمسّان المال مباشرةً:
/// • الحقل الرقمي يُفرَغ عند دخول المؤشّر، فلا يلتحم رقمٌ سابق بما يُكتَب.
/// • لكن الخروج بلا كتابة يُعيد ما كان — وإلا ضاعت الحوالة بلمسة عابرة.
void main() {
  Future<(TextEditingController, NumericFieldFocus, int Function())> pumpField(
    WidgetTester tester, {
    required String initial,
    bool formatOnExit = false,
  }) async {
    final c = TextEditingController(text: initial);
    var count = 0;
    final node = NumericFieldFocus(c,
        onChanged: () => count++, formatOnExit: formatOnExit);
    addTearDown(node.dispose);
    addTearDown(c.dispose);

    await tester.pumpWidget(MaterialApp(
      home: Scaffold(
        body: Column(children: [
          TextField(controller: c, focusNode: node),
          const TextField(key: Key('other')),
        ]),
      ),
    ));
    return (c, node, () => count);
  }

  Future<void> blur(WidgetTester tester) async {
    await tester.tap(find.byKey(const Key('other')));
    await tester.pump();
  }

  group('NumericFieldFocus — التنظيف عند الدخول', () {
    testWidgets('يُفرَغ الحقل بمجرّد دخول المؤشّر', (tester) async {
      final (c, node, _) = await pumpField(tester, initial: '5,650.00');
      node.requestFocus();
      await tester.pump();
      expect(c.text, '', reason: 'جاهز لاستقبال أول رقم');
    });

    testWidgets('أول رقم يُكتب لا يلتحم بالقيمة السابقة', (tester) async {
      final (c, node, _) =
          await pumpField(tester, initial: '5,650.00', formatOnExit: true);
      node.requestFocus();
      await tester.pump();
      c.text = '2500'; // ما كتبه الوكيل بعد التفريغ
      await blur(tester);
      expect(c.text, '2,500.00');
    });

    testWidgets('الخروج بلا كتابة يُعيد ما كان — لا تضيع الحوالة بلمسة',
        (tester) async {
      final (c, node, _) =
          await pumpField(tester, initial: '5,650.00', formatOnExit: true);
      node.requestFocus();
      await tester.pump();
      expect(c.text, '');
      await blur(tester);
      expect(c.text, '5,650.00');
    });

    testWidgets('الحقل الفارغ أصلاً لا يُبلَّغ عنه تغيير', (tester) async {
      final (c, node, count) = await pumpField(tester, initial: '');
      node.requestFocus();
      await tester.pump();
      await blur(tester);
      expect(c.text, '');
      expect(count(), 0, reason: 'لا إعادة بناء بلا سبب');
    });
  });

  group('NumericFieldFocus — التنسيق عند الخروج', () {
    testWidgets('الخانة الثالثة تبقى إن حملت قيمة', (tester) async {
      final (c, node, _) =
          await pumpField(tester, initial: '', formatOnExit: true);
      node.requestFocus();
      await tester.pump();
      c.text = '2500.325';
      await blur(tester);
      expect(c.text, '2,500.325');
    });

    testWidgets('حقل غير مالي لا يُنسَّق — الهاتف يبقى كما كُتب',
        (tester) async {
      final (c, node, _) = await pumpField(tester, initial: '');
      node.requestFocus();
      await tester.pump();
      c.text = '924458817';
      await blur(tester);
      expect(c.text, '924458817');
    });
  });
}
