import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/net/api_envelope.dart';
import 'package:rhalla_agent/features/send/limit_dialog.dart';
import 'package:rhalla_agent/features/send/send_repository.dart';

/// رفض الحوالة لتجاوز السقف. الردّ الحقيقي كما رُصد من `checkTransferLimits`:
/// الأرقام في `violations` بجذر الجسم، لا في `data` ولا في `message`.
void main() {
  ApiFailure failure(Object body) {
    final env = Envelope.parse(422, body);
    return ApiFailure('x', statusCode: 422, envelope: env);
  }

  group('TransferLimitExceeded', () {
    test('يقرأ السقف المتجاوَز ومقداره', () {
      final e = failure({
        'success': false,
        'violations': [
          {'type_from': 'Daily', 'Debit': 5025.000, 'label': 'اليومي'}
        ],
        'total': 3015,
        'message': ' لقد تجاوزت حدود التحويل اليومي',
      });

      final v = TransferLimitExceeded.from(e)!;
      expect(v.labels, ['اليومي']);
      expect(v.debit, 5025.0);
      expect(v.total, 3015.0);
      expect(v.describe(), contains('السقف اليومي'));
      expect(v.describe(), contains('5,025.00'));
    });

    test('أكثر من سقف في حوالة واحدة', () {
      final e = failure({
        'violations': [
          {'type_from': 'Daily', 'Debit': 5025.0, 'label': 'اليومي'},
          {'type_from': 'Weekly', 'Debit': 9100.0, 'label': 'الأسبوعي'},
        ],
        'total': 3015,
      });

      final v = TransferLimitExceeded.from(e)!;
      expect(v.labels, ['اليومي', 'الأسبوعي']);
      // أكبر مخصوم هو الرقم الذي يعني الوكيل.
      expect(v.debit, 9100.0);
      expect(v.describe(), contains('اليومي والأسبوعي'));
    });

    test('خطأ آخر لا يُقرأ تجاوزَ سقف', () {
      expect(TransferLimitExceeded.from(failure({'message': 'رقم غير مسجّل'})),
          isNull);
      expect(TransferLimitExceeded.from(failure({'violations': []})), isNull);
    });
  });

  /// النصّ ثابت ولا تتغيّر منه إلا كلمة المدّة — قرار المالك.
  group('نصّ الإشعار', () {
    test('كلمة واحدة لكل مدّة', () {
      expect(limitExceededMessage(['اليومي']), 'لقد تجاوزت سقف التحويل اليومي');
      expect(limitExceededMessage(['الأسبوعي']),
          'لقد تجاوزت سقف التحويل الأسبوعي');
      expect(limitExceededMessage(['الشهري']), 'لقد تجاوزت سقف التحويل الشهري');
      expect(limitExceededMessage(['السنوي']), 'لقد تجاوزت سقف التحويل السنوي');
    });

    test('أكثر من مدّة تُعطَف', () {
      expect(limitExceededMessage(['اليومي', 'الأسبوعي']),
          'لقد تجاوزت سقف التحويل اليومي والأسبوعي');
      expect(limitExceededMessage(['اليومي', 'الأسبوعي', 'الشهري']),
          'لقد تجاوزت سقف التحويل اليومي، الأسبوعي والشهري');
    });

    test('بلا تسمية من الخادم تبقى الجملة سليمة', () {
      expect(limitExceededMessage([]), 'لقد تجاوزت سقف التحويل');
      expect(limitExceededMessage(['', '  ']), 'لقد تجاوزت سقف التحويل');
    });
  });
}
