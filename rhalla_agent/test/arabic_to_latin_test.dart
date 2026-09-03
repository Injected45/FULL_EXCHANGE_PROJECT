import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/branding/arabic_to_latin.dart';

/// الاسم الإنجليزي يُقترح لحظةَ الكتابة، ويُطبع على ما يراه العميل — فسلوكه
/// مثبَّت هنا. الحالات المهمّة ثلاث: النقل الحرفي للاسم العَلَم، وترجمة
/// الكلمات الوصفية، وألّا ينهار على مدخلٍ ناقص أثناء الكتابة.
void main() {
  group('ArabicToLatin', () {
    test('اسم شركة كامل — العَلَم يُنقل والوصف يُترجم', () {
      expect(
        ArabicToLatin.suggest('شركة الرحالة للحوالات المالية'),
        'Al Rhala Company for Financial Transfers',
      );
    });

    test('«شركة» تنتقل إلى ما بعد الاسم كما في الإنجليزية', () {
      expect(ArabicToLatin.suggest('شركة الأمانة'), 'Al Amana Company');
    });

    test('الصفة تسبق موصوفها بعد الترجمة', () {
      expect(
        ArabicToLatin.suggest('مؤسسة النور للصرافة الليبية'),
        'Al Nur Establishment for Libyan Exchange',
      );
    });

    test('اسمٌ بلا كلمات معروفة يُنقل حرفياً كلّه', () {
      expect(ArabicToLatin.suggest('دار السلام'), 'Dar Al Slam');
    });

    test('التشكيل والتطويل لا يغيّران النتيجة', () {
      expect(
        ArabicToLatin.suggest('شركةُ الأمانةِ'),
        ArabicToLatin.suggest('شركة الأمانة'),
      );
    });

    test('مدخل فارغ أو مسافات — سلسلة فارغة لا استثناء', () {
      expect(ArabicToLatin.suggest(''), '');
      expect(ArabicToLatin.suggest('   '), '');
    });

    test('حرف واحد أثناء الكتابة لا يكسر شيئاً', () {
      expect(ArabicToLatin.suggest('ش'), 'Sh');
      expect(ArabicToLatin.suggest('شر'), 'Shr');
    });

    test('الأرقام والحروف اللاتينية تمرّ كما هي', () {
      expect(ArabicToLatin.suggest('فرع 15'), 'Branch 15');
    });
  });
}
