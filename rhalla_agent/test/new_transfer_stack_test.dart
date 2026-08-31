import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:go_router/go_router.dart';

/// يحسم سؤالاً واحداً: حين تفتح «حوالة جديدة» مساراً موجوداً أصلاً في
/// المكدّس، هل يُعاد استعمال حالة الشاشة القديمة فتظهر بيانات آخر حوالة؟
///
/// المسار الحقيقي: النموذج ← المراجعة ← (استبدال) النجاح ← (استبدال) النموذج.
/// النموذج القديم يبقى تحت المكدّس طوال ذلك، ومفتاح الصفحة هو ما يقرّر
/// أتُعاد حالته أم تُبنى حالة وليدة.

class _Form extends StatefulWidget {
  const _Form();
  @override
  State<_Form> createState() => _FormState();
}

class _FormState extends State<_Form> {
  final c = TextEditingController();

  @override
  void initState() {
    super.initState();
    c.addListener(() => setState(() {}));
  }

  @override
  void dispose() {
    c.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => Scaffold(
        appBar: AppBar(), // ليعمل زرّ الرجوع في الاختبار
        body: Column(
          children: [
            TextField(controller: c),
            Text('القيمة: ${c.text}', key: const Key('echo')),
            TextButton(
              onPressed: () => context.push('/review'),
              child: const Text('مراجعة'),
            ),
          ],
        ),
      );
}

void main() {
  testWidgets('«حوالة جديدة» تفتح نموذجاً بحقول فارغة', (tester) async {
    final router = GoRouter(
      initialLocation: '/',
      routes: [
        GoRoute(
          path: '/',
          builder: (_, _) => Scaffold(
            body: Builder(
              builder: (c) => TextButton(
                onPressed: () => c.push('/form'),
                child: const Text('حوالة داخلية'),
              ),
            ),
          ),
        ),
        GoRoute(path: '/form', builder: (_, _) => const _Form()),
        GoRoute(
          path: '/review',
          builder: (_, _) => Scaffold(
            body: Builder(
              builder: (c) => TextButton(
                onPressed: () => c.pushReplacement('/done'),
                child: const Text('تأكيد'),
              ),
            ),
          ),
        ),
        GoRoute(
          path: '/done',
          builder: (_, _) => Scaffold(
            body: Builder(
              builder: (c) => TextButton(
                // نفس ما تفعله «حوالة جديدة» في شاشة النجاح.
                onPressed: () {
                  c.go('/');
                  WidgetsBinding.instance.addPostFrameCallback((_) {
                    if (c.mounted) c.push('/form');
                  });
                },
                child: const Text('حوالة جديدة'),
              ),
            ),
          ),
        ),
      ],
    );

    await tester.pumpWidget(MaterialApp.router(routerConfig: router));

    await tester.tap(find.text('حوالة داخلية'));
    await tester.pumpAndSettle();

    await tester.enterText(find.byType(TextField), '2310');
    await tester.pumpAndSettle();
    expect(find.text('القيمة: 2310'), findsOneWidget);

    await tester.tap(find.text('مراجعة'));
    await tester.pumpAndSettle();
    await tester.tap(find.text('تأكيد'));
    await tester.pumpAndSettle();
    await tester.tap(find.text('حوالة جديدة'));
    await tester.pumpAndSettle();

    // النموذج المعروض الآن يجب أن يكون وليداً — لا أثر لِـ 2310.
    expect(find.text('القيمة: 2310'), findsNothing,
        reason: 'النموذج الجديد يعرض بيانات الحوالة السابقة');
    expect(find.text('القيمة: '), findsOneWidget);

    // والنموذج الممتلئ لم يعد في المكدّس: الرجوع يخرج إلى الرئيسية لا إليه.
    await tester.pageBack();
    await tester.pumpAndSettle();
    expect(find.text('حوالة داخلية'), findsOneWidget);
    expect(find.byType(TextField), findsNothing);
  });
}
