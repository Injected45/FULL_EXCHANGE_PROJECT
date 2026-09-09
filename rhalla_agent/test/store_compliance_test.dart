import 'dart:io';

import 'package:flutter_test/flutter_test.dart';

/*
 * ══════════════════════════════════════════════════════════════════════════
 *  ⚠⚠ شروطُ Google Play و App Store — مقروءةً من ملفّات المشروع نفسِها
 * ══════════════════════════════════════════════════════════════════════════
 *
 * أمرُ المالك (10 سبتمبر 2026): التطبيق يُرفع إلى المتجرين، فلا تبقى ثغرةٌ
 * يرفضها أحدُهما.
 *
 * ── ولماذا اختبارٌ لا قائمةُ مراجعة ──────────────────────────────────────
 *
 * قائمةُ المراجعة تُقرأ مرّةً يومَ تُكتب. وهذه الشروطُ تنكسر **صامتةً**:
 * إضافةٌ جديدة تطلب إذناً بلا نصّ استعمال، أو أحدٌ يُعيد `allowBackup`، أو
 * تُنسى رايةُ HTTP مفتوحةً — ولا يظهر شيءٌ من ذلك في `flutter analyze` ولا
 * في تشغيل، إنّما في بريدِ رفضٍ بعد أسابيع.
 *
 * فيُقرأ المانيفستُ و`Info.plist` كنصّ ويُفحصان مع كلّ `flutter test`.
 *
 * ⚠ وهو يفحص **المصدر** لا الحزمة المبنيّة: الحزمةُ تُفحص بـ`aapt2` عند
 * البناء، وهذا يمسك الخطأ قبل أن يُبنى شيء.
 */

/// يُصعد من `test/` إلى جذر مشروع فلاتر.
File _f(String rel) => File('${Directory.current.path}/$rel');

void main() {
  late String manifest;
  late String plist;

  setUpAll(() {
    manifest = _f('android/app/src/main/AndroidManifest.xml').readAsStringSync();
    plist = _f('ios/Runner/Info.plist').readAsStringSync();
  });

  group('⚠⚠ أندرويد — ما يفحصه Google Play', () {
    test('لا نسخَ احتياطياً لبيانات التطبيق', () {
      /*
       * ⚠ الافتراضيُّ في أندرويد هو `true`: تُرفع بياناتُ التطبيق إلى
       * Google Drive وتُستعاد على **هاتفٍ آخر**. وما نخزّنه رموزُ جلسات
       * ومعرّفُ جهازٍ مربوط — فذلك ينقض ربطَ الجهاز نفسَه.
       */
      expect(manifest, contains('android:allowBackup="false"'),
          reason: 'غيابُه يعني رفعَ رموز الجلسات إلى السحابة');
    });

    test('وقواعدُ الاستخراج معلنةٌ للنظامين القديم والحديث', () {
      // أندرويد 12+ يقرأ الأولى، وما دونه الثانية. والجهازان في الأيدي.
      expect(manifest, contains('android:dataExtractionRules='));
      expect(manifest, contains('android:fullBackupContent='));

      for (final p in const [
        'android/app/src/main/res/xml/data_extraction_rules.xml',
        'android/app/src/main/res/xml/backup_rules.xml',
      ]) {
        expect(_f(p).existsSync(), isTrue, reason: 'مفقود: $p');
      }
    });

    test('⚠⚠ ولا HTTP صريحٌ في الإعداد الافتراضي', () {
      /*
       * الإعدادُ متغيّرٌ من Gradle، وافتراضيُّه الصارم. فالمانيفست يجب أن
       * يشير إلى المتغيّر لا إلى ملفّ الاستثناء مباشرةً — وإلّا صار
       * الاستثناءُ دائماً بلا أن يقصده أحد.
       */
      expect(manifest, contains(r'@xml/${netSecConfig}'),
          reason: 'إشارةٌ مباشرة إلى ملفّ الاستثناء تجعله دائماً');

      expect(manifest, isNot(contains('android:usesCleartextTraffic="true"')),
          reason: 'يفتح كلَّ مضيف — لا مضيفاً واحداً');

      final strict = _f('android/app/src/main/res/xml/network_security_strict.xml');
      expect(strict.existsSync(), isTrue);
      final s = strict.readAsStringSync();
      expect(s, contains('cleartextTrafficPermitted="false"'));
      expect(s, isNot(contains('cleartextTrafficPermitted="true"')),
          reason: 'الملفُّ الصارم لا يحمل استثناءً واحداً');
    });

    test('وحارسُ حزمة المتجر قائمٌ في Gradle', () {
      final g = _f('android/app/build.gradle.kts').readAsStringSync();
      expect(g, contains('allowCleartext'));
      expect(g, contains('network_security_strict'),
          reason: 'الافتراضيُّ يجب أن يكون الصارم');
      expect(g, contains('GradleException'),
          reason: 'الحزمةُ يجب أن تفشل لا أن تُنبّه');
    });

    test('⚠ ولا إذنَ بلا استعمال', () {
      /*
       * كلُّ إذنٍ يُسأل عنه في Play Console، ويزيد سطحَ المراجعة. وإذنٌ
       * لا يستعمله التطبيق سببُ أسئلةٍ ورفضٍ بلا مقابل.
       */
      final asked = RegExp(r'android\.permission\.([A-Z_]+)')
          .allMatches(manifest)
          .map((m) => m.group(1)!)
          .toSet();

      expect(asked, containsAll(<String>{'INTERNET', 'CAMERA', 'RECORD_AUDIO'}),
          reason: 'الشبكة والماسح والرسائل الصوتية');

      // ⚠ أذونٌ يرفضها Play أو يشترط لها استمارةً خاصّة — ولا نستعملها.
      for (final banned in const [
        'READ_SMS', 'RECEIVE_SMS', 'READ_PHONE_STATE', 'QUERY_ALL_PACKAGES',
        'MANAGE_EXTERNAL_STORAGE', 'ACCESS_FINE_LOCATION', 'READ_CONTACTS',
        'SYSTEM_ALERT_WINDOW', 'REQUEST_INSTALL_PACKAGES',
      ]) {
        expect(asked, isNot(contains(banned)),
            reason: '$banned يفتح مراجعةً خاصّة بلا حاجة');
      }
    });

    test('والنشاطُ الوحيد المُصدَّر هو المُطلِق', () {
      // ⚠ نشاطٌ مُصدَّر بلا سبب بابٌ لتطبيقٍ آخر.
      final exported = 'android:exported="true"'.allMatches(manifest).length;
      expect(exported, 1, reason: 'المُطلِق وحدَه');
    });
  });

  group('⚠⚠ iOS — ما يفحصه App Review', () {
    test('كلُّ إذنٍ له نصُّ استعمال', () {
      /*
       * ⚠ **بلا النصّ يُسقط النظامُ التطبيقَ لحظةَ أوّل استعمال** — لا رسالةَ
       * خطأ ولا شيء. وترفض Apple النسخة في المراجعة.
       *
       * والأربعةُ مستعملةٌ فعلاً: الكاميرا لماسح QR، والميكروفون للرسائل
       * الصوتية، والألبومُ لإرفاق صورةٍ وشعارِ الشركة، والوجهُ لقفل الخمول.
       */
      for (final key in const [
        'NSCameraUsageDescription',
        'NSMicrophoneUsageDescription',
        'NSPhotoLibraryUsageDescription',
        'NSFaceIDUsageDescription',
      ]) {
        expect(plist, contains('<key>$key</key>'), reason: 'مفقود: $key');
      }
    });

    test('⚠ والنصوصُ تشرح السبب — لا كلمةً واحدة', () {
      /*
       * Apple ترفض نصّاً لا يقول **لماذا**. و«للكاميرا» ليست سبباً.
       * والحدُّ هنا تقريبيّ لكنه يمسك النصَّ الفارغ والكلمةَ الواحدة.
       */
      final re = RegExp(
        r'<key>(NS\w+UsageDescription)</key>\s*<string>([^<]*)</string>',
        multiLine: true,
      );
      final found = re.allMatches(plist);
      expect(found, isNotEmpty);

      for (final m in found) {
        final key = m.group(1)!;
        final text = m.group(2)!.trim();
        expect(text.length, greaterThan(30),
            reason: '$key: نصٌّ قصير لا يشرح السبب — «$text»');
      }
    });

    test('وإقرارُ التشفير معلَن', () {
      // تسأل عنه Apple عند كل رفع؛ وغيابُه يعني سؤالاً يدوياً في كل مرّة.
      expect(plist, contains('<key>ITSAppUsesNonExemptEncryption</key>'));
    });

    test('⚠⚠ ولا استثناءَ ATS', () {
      /*
       * `NSAppTransportSecurity` غائبٌ عمداً: iOS يمنع HTTP افتراضاً، وهذا
       * هو الصواب. واستثناؤه لتطبيقٍ ماليّ يُرفض عادةً في المراجعة.
       */
      expect(plist, isNot(contains('NSAppTransportSecurity')),
          reason: 'استثناءُ ATS يُرفض لتطبيقٍ يحمل أرصدة');
      expect(plist, isNot(contains('NSAllowsArbitraryLoads')));
    });
  });
}
