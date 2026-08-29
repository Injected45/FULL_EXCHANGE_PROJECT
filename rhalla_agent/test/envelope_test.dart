import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/format/fmt.dart';
import 'package:rhalla_agent/core/net/api_envelope.dart';

/// اختبارات الفكّاك المتسامح.
/// كل حالة هنا انحراف حقيقي رُصد في كود الخلفية — لا حالة مفترضة.
void main() {
  group('Envelope', () {
    test('الغلاف القياسي', () {
      final e = Envelope.parse(200, {
        'data': {'x': 1},
        'message': 'تم',
        'success': true,
        'key': 'SUCCESS',
      });
      expect(e.ok, isTrue);
      expect(e.messageText, 'تم');
      expect(e.row?['x'], 1);
    });

    test('datat بدل data — Daily_transfer', () {
      final e = Envelope.parse(200, {
        'success': true,
        'datat': {'Daily': 76400, 'Weekly': 312900},
      });
      expect(e.row?['Daily'], 76400);
    });

    test('message ككائن — مسار الرصيد غير الكافي', () {
      // الوسائط معكوسة في sendError: message يحمل الأرقام و data يحمل النص.
      final e = Envelope.parse(422, {
        'success': false,
        'message': {'wallet': 48320.75, 'amount': 62000, 'total': 62310},
        'data': 'رصيد غير كافي',
      });
      expect(e.messageText, isNull, reason: 'يجب ألا يُعامَل الكائن كنص');
      expect(e.messageMap?['wallet'], 48320.75);
      // ما يُعرض للمستخدم يجب أن يبقى نصاً صالحاً.
      expect(e.displayMessage('احتياطي'), 'رصيد غير كافي');
    });

    test('بلا غلاف — checkOtp يرد {message, status}', () {
      final e = Envelope.parse(200, {'message': 'تمت المطابقة بنجاح', 'status': true});
      expect(e.ok, isTrue);
      expect(e.messageText, 'تمت المطابقة بنجاح');
    });

    test('status:false مع HTTP 200', () {
      final e = Envelope.parse(200, {'message': 'خطأ', 'status': false});
      expect(e.ok, isFalse);
    });

    test('success:true مع HTTP 422', () {
      final e = Envelope.parse(422, {'success': true, 'data': []});
      expect(e.ok, isTrue);
    });

    test('جسم فارغ تماماً', () {
      final e = Envelope.parse(200, null);
      expect(e.ok, isTrue);
      expect(e.rows, isEmpty);
    });

    test('نموذج عارٍ في الجذر — searchPayment', () {
      final e = Envelope.parse(200, {'id': 7, 'amount': 250});
      expect(e.row?['id'], 7);
    });

    test('مصفوفة من كائن واحد — getBalanceLocal', () {
      final e = Envelope.parse(200, {
        'success': true,
        'data': [
          {'Walet': '48320.750'}
        ],
      });
      expect(Fmt.num_(e.row?['Walet']), 48320.75);
    });

    test('صفحة 404 من Apache لا تتسرّب إلى الشاشة', () {
      // حدث فعلاً: عنوان API خاطئ ⇒ Apache يرد صفحة HTML،
      // والفكّاك كان يعرضها للمستخدم كرسالة خطأ.
      const page = '<!DOCTYPE HTML PUBLIC "-//IETF//DTD HTML 2.0//EN">\n'
          '<html><head><title>404 Not Found</title></head>'
          '<body><h1>Not Found</h1></body></html>';
      expect(Envelope.looksLikeMarkup(page), isTrue);
      final e = Envelope.parse(404, page);
      expect(e.displayMessage('تعذّر إتمام العملية.'), 'تعذّر إتمام العملية.');
    });

    test('أثر استثناء PHP لا يتسرّب — APP_DEBUG=true على الخادم', () {
      const trace = '{"trace":[{"file":"C:\\\\xampp\\\\htdocs\\\\app.php"}]}';
      expect(Envelope.looksLikeMarkup(trace), isTrue);
    });

    test('رسالة طويلة جداً تُستبدل بالاحتياطي', () {
      final long = 'ا' * 500;
      final e = Envelope.parse(500, {'message': long});
      expect(e.displayMessage('خطأ في الخادم.'), 'خطأ في الخادم.');
    });

    test('الرسالة العربية القصيرة تمرّ كما هي', () {
      final e = Envelope.parse(422, {'success': false, 'message': 'رقم غير مسجّل'});
      expect(e.displayMessage('احتياطي'), 'رقم غير مسجّل');
    });

    test('حقيبة أخطاء التحقّق', () {
      final e = Envelope.parse(422, {
        'success': false,
        'message': 'Validation Error.',
        'data': {
          'phone': ['رقم الهاتف مطلوب']
        },
      });
      expect(e.firstValidationError(), 'رقم الهاتف مطلوب');
    });
  });

  group('Fmt', () {
    test('الدينار بثلاث خانات عشرية', () {
      expect(Fmt.money(48320.75), '48,320.750');
      expect(Fmt.money(1250), '1,250.000');
    });

    test('الأرقام غربية لا عربية-هندية', () {
      expect(Fmt.money(1234.5), isNot(contains('١')));
      expect(Fmt.count(2026), '2,026');
    });

    test('توحيد الهاتف عند حدود التطبيق', () {
      expect(Fmt.phoneForApi('+218 92 445 8817'), '924458817');
      expect(Fmt.phoneForApi('00218924458817'), '924458817');
      expect(Fmt.phoneForApi('0924458817'), '924458817');
      expect(Fmt.phoneForApi('924458817'), '924458817');
    });

    test('تحقّق الرقم الليبي — 9 خانات تبدأ بـ 9', () {
      expect(Fmt.isValidLibyanPhone('924458817'), isTrue);
      expect(Fmt.isValidLibyanPhone('824458817'), isFalse);
      expect(Fmt.isValidLibyanPhone('92445881'), isFalse);
    });

    test('العرض 92 445 8817', () {
      expect(Fmt.phone('924458817'), '92 445 8817');
    });

    test('الرقم قد يصل نصاً من SQL الخام', () {
      expect(Fmt.num_('1,250.000'), 1250.0);
      expect(Fmt.num_(1250), 1250.0);
      expect(Fmt.num_(null), 0);
    });
  });
}
