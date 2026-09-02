// شاشات الحوالة الداخلية الثلاث تظهر كاملةً بلا تمرير على جهاز متوسّط.
//
// المسار: الإدخال ← المراجعة ورمز التحقّق ← «تمّت الحوالة».
// الشروط ومقاس الجهاز مشروحة في screen_fits.dart.

import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/features/send/review_screen.dart';
import 'package:rhalla_agent/features/send/send_internal_screen.dart';
import 'package:rhalla_agent/features/send/send_repository.dart';
import 'package:rhalla_agent/features/send/success_screen.dart';

import 'screen_fits.dart';

/// مسوّدة بأطول قيم معقولة لا بأقصرها: اسم ثلاثي، ومبلغ من ستّ خانات،
/// وملاحظة. تخطيطٌ يسع «5.00» ويضيق بـ «250,000.00» لم يُختبر.
const _draft = TransferDraft(
  receiverName: 'محمد عبدالسلام الفيتوري',
  receiverPhone: '925093709',
  amount: 250000,
  commission: 2000,
  city: Ref2(3, 'بنغازي'),
  branch: Ref2(7, 'فرع بنغازي الرئيسي'),
  currencyId: 1,
  notes: 'قيمة بضاعة',
);

const _created = CreatedTransfer(
  code: '11261-54-13',
  mobileCode: '542613',
  receiverName: 'محمد عبدالسلام الفيتوري',
  receiverPhone: '925093709',
  amount: 250000,
  commission: 2000,
  insertedAt: '2026-09-01 14:30:00',
  cityName: 'بنغازي',
  branchName: 'فرع بنغازي الرئيسي',
);

void main() {
  setUpAll(loadAppFonts);

  testWidgets('1/3 شاشة إدخال الحوالة الداخلية', (tester) async {
    final s = await pumpAtPhoneSize(tester, const SendInternalScreen());
    expectNoScroll(s);
    expectAllVisible([
      'مبلغ الحوالة',
      'اسم المستفيد',
      'هاتف المستفيد',
      'مدينة الاستلام',
      'ملاحظات · اختياري',
      'الإجمالي المخصوم',
    ]);
  });

  testWidgets('2/3 شاشة المراجعة ورمز التحقّق', (tester) async {
    final s = await pumpAtPhoneSize(
      tester,
      const ReviewTransferScreen(draft: _draft),
    );
    expectNoScroll(s);
  });

  testWidgets('3/3 شاشة «تمّت الحوالة»', (tester) async {
    final s = await pumpAtPhoneSize(
      tester,
      TransferDoneScreen(transfer: _created),
      settle: false,
    );
    expectNoScroll(s);
  });
}
