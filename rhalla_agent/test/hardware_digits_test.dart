import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/ui/widgets/controls.dart';

/// `HardwareDigits` يلتقط الأرقام على مستوى [HardwareKeyboard] كي تعمل
/// لوحةُ الأرقام المرسومة بكيبورد الكمبيوتر على المحاكي.
///
/// وهو التقاطٌ **عامّ**: لا يعرف أين وقعت الضغطة، ويُرجع `true` فيمنع
/// وصولها إلى ما تحته. فأُضيف إليه شرطٌ واحد — أن يقف حين يكون التركيز
/// داخل حقل نصّ — وهذا الملفّ يثبّته.
///
/// ⚠ الخلل الذي وُلد منه: شاشة تفعيل الموظف تجمع في الشاشة نفسِها حقلَ
/// هاتفٍ وحقلَ كودٍ (الخطوة 1) ولوحةَ أرقام (الخطوة 2). فكان الملتقِط
/// يبتلع كلَّ رقمٍ يُكتب في حقل الهاتف، فيبدو الحقل **لا يقبل الكتابة**
/// على كل جهازٍ يصل منه حدثُ مفتاح — بلا رسالة خطأ وبلا أثرٍ يُرشد إلى
/// السبب. ويبتلع كذلك Backspace، فلا يُمحى ما كُتب.
///
/// ⚠ والاختبار يقيس **ما يلتقطه المِزيج** لا ما يظهر في الحقل: الكتابةُ في
/// `TextField` تمرّ في الاختبارات عبر قناة الإدخال لا عبر أحداث المفاتيح،
/// فـ`sendKeyEvent` لا تكتب حرفاً فيه أصلاً. والمقيسُ هنا هو موضعُ العيب
/// بالضبط: هل ابتلع المِزيجُ الضغطة أم تركها لصاحبها؟
void main() {
  testWidgets('⚠ المِزيج يقف حين يكون التركيز في حقل نصّ', (t) async {
    final captured = <String>[];
    await t.pumpWidget(_App(captured: captured));

    await t.tap(find.byType(TextField));
    await t.pumpAndSettle();
    expect(FocusManager.instance.primaryFocus?.hasFocus, isTrue,
        reason: 'التمهيد: الحقل مركَّزٌ فعلاً');

    for (final k in [
      LogicalKeyboardKey.digit9,
      LogicalKeyboardKey.digit2,
      LogicalKeyboardKey.digit5,
    ]) {
      await t.sendKeyEvent(k);
    }
    await t.pump();

    expect(captured, isEmpty,
        reason: 'الأرقام للحقل لا للوحة — وهذا هو العيب الذي أُصلح');
  });

  testWidgets('و Backspace كذلك يُترك للحقل', (t) async {
    final captured = <String>[];
    await t.pumpWidget(_App(captured: captured));

    await t.tap(find.byType(TextField));
    await t.pumpAndSettle();

    await t.sendKeyEvent(LogicalKeyboardKey.backspace);
    await t.pump();

    expect(captured, isEmpty);
  });

  /// وبلا تركيزٍ في حقل يعمل المِزيج كما وُضع له — وهي حال شاشتَي الهاتف
  /// والرمز في دخول الوكيل: لا `TextField` فيهما أصلاً.
  testWidgets('وبلا تركيزٍ في حقل يأخذ الأرقام كما وُضع له', (t) async {
    final captured = <String>[];
    await t.pumpWidget(_App(captured: captured));
    await t.pumpAndSettle();

    for (final k in [
      LogicalKeyboardKey.digit1,
      LogicalKeyboardKey.digit2,
      LogicalKeyboardKey.digit3,
      LogicalKeyboardKey.digit4,
    ]) {
      await t.sendKeyEvent(k);
    }
    await t.pump();

    expect(captured, ['1', '2', '3', '4']);
  });

  testWidgets('و Backspace يصله حين لا حقلَ مركَّزاً', (t) async {
    final captured = <String>[];
    await t.pumpWidget(_App(captured: captured));
    await t.pumpAndSettle();

    await t.sendKeyEvent(LogicalKeyboardKey.backspace);
    await t.pump();

    expect(captured, ['⌫']);
  });

  /// وبعد أن يترك الحقلُ التركيز تعود اللوحة إلى العمل — وهو ما يجري
  /// فعلاً في شاشة التفعيل حين تنتقل من خطوة الحقول إلى خطوة الرمز.
  testWidgets('وتعود اللوحة حين يترك الحقلُ التركيز', (t) async {
    final captured = <String>[];
    await t.pumpWidget(_App(captured: captured));

    await t.tap(find.byType(TextField));
    await t.pumpAndSettle();
    await t.sendKeyEvent(LogicalKeyboardKey.digit7);
    expect(captured, isEmpty);

    FocusManager.instance.primaryFocus?.unfocus();
    await t.pumpAndSettle();

    await t.sendKeyEvent(LogicalKeyboardKey.digit7);
    await t.pump();
    expect(captured, ['7']);
  });
}

class _App extends StatelessWidget {
  const _App({required this.captured});

  final List<String> captured;

  @override
  Widget build(BuildContext context) =>
      MaterialApp(home: _Screen(captured: captured));
}

/// ⚠ الشاشةُ **داخل** `MaterialApp`: المِزيج يفحص `ModalRoute.of(context)`
/// ليتجاهل الضغطة حين تكون شاشتُه تحت أخرى، وفوق `MaterialApp` لا مسارَ
/// أصلاً فيصمت المِزيجُ لسببٍ لا علاقة له بما نقيس.
class _Screen extends StatefulWidget {
  const _Screen({required this.captured});

  final List<String> captured;

  @override
  State<_Screen> createState() => _ScreenState();
}

class _ScreenState extends State<_Screen> with HardwareDigits {
  final _controller = TextEditingController();

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  void onHardwareDigit(String d) => widget.captured.add(d);

  @override
  void onHardwareDelete() => widget.captured.add('⌫');

  @override
  Widget build(BuildContext context) => Scaffold(
        body: Center(
          child: TextField(
            controller: _controller,
            keyboardType: TextInputType.number,
          ),
        ),
      );
}
