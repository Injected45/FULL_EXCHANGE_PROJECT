import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/format/fmt.dart';

/// حقول الأسماء: حروف ومسافات فقط — قرار المالك.
/// الاسم يُقارَن بوثيقة المستلم على الشبّاك، فرمزٌ فيه يجعله غير مطابق.
void main() {
  String filter(String input) {
    var v = TextEditingValue(text: input);
    for (final f in lettersOnlyFormatters) {
      v = f.formatEditUpdate(const TextEditingValue(), v);
    }
    return v.text;
  }

  test('الحروف العربية والمسافات تمرّ', () {
    expect(filter('المهدي عبدالله محمد'), 'المهدي عبدالله محمد');
    expect(filter('أحمد إبراهيم آدم ة ى'), 'أحمد إبراهيم آدم ة ى');
  });

  test('الحروف اللاتينية تمرّ — أسماء المستفيدين خارج ليبيا', () {
    expect(filter('Ahmed Ali'), 'Ahmed Ali');
  });

  test('الأرقام تُحذف — غربية وهندية', () {
    expect(filter('احمد123'), 'احمد');
    expect(filter('احمد٤٥٦'), 'احمد');
  });

  test('الإشارات والعلامات تُحذف', () {
    expect(filter('احمد@محمد'), 'احمدمحمد');
    expect(filter('احمد-محمد'), 'احمدمحمد');
    expect(filter('احمد.محمد،'), 'احمدمحمد');
    expect(filter('احمد_محمد#\$%'), 'احمدمحمد');
  });

  test('التطويل والتشكيل يُحذفان — يجعلان الاسم غير مطابق للوثيقة', () {
    expect(filter('محـمد'), 'محمد');
    expect(filter('مُحَمَّد'), 'محمد');
  });
}
