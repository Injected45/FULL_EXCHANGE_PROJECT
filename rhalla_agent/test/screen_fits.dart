// أدوات مشتركة لاختبارات «الشاشة تظهر كاملةً بلا تمرير».
//
// المقاس الحاكم **360×640 dp** — ما تعطيه Oppo A37 و A57 (720×1280 عند كثافة
// 320). و Note 10 أوسع وأطول (411×868) فيفضل عنها هامش، فنجاح الأصغر يعني
// نجاح الثلاثة والعكس غير صحيح.
//
// قرار المالك (1 سبتمبر 2026): كل شاشات الحوالة الداخلية تظهر كاملةً على
// جهاز متوسّط بلا تمرير لأعلى ولا لأسفل، وبلا إخفاء أو حذف أي بيان.

import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:rhalla_agent/core/net/api_client.dart';
import 'package:rhalla_agent/core/storage/secure_store.dart';
import 'package:rhalla_agent/core/theme/app_theme.dart';
import 'package:rhalla_agent/features/auth/auth_repository.dart';

/// أصغر الأجهزة الثلاثة المستهدفة.
const kPhone = Size(360, 640);

/// شريط الحالة. `Screen` يلفّ محتواه بـ SafeArea فيقتطعه، وإغفاله هنا يعطي
/// نتيجة متفائلة بمقدار ارتفاعه.
const kPadding = EdgeInsets.only(top: 24);

// AuthController يُقلع عند إنشائه فيقرأ من التخزين الآمن ومن الشبكة، وكلاهما
// غير موجود في بيئة الاختبار. فبدل تزييف المتحكّم — وهو غير قابل للوراثة بلا
// تمرير هذين — نزيّف ما يعتمد عليه.
//
// noSuchMethod يغطّي البقية: ما لا تستدعيه الشاشة لا يلزم، وما تستدعيه بلا
// تزييف سيفشل بصوت عالٍ بدل أن يمرّ بقيمة صامتة.

class FakeStore implements SecureStore {
  @override
  Future<bool> readOnboarded() async => true;

  @override
  dynamic noSuchMethod(Invocation invocation) => super.noSuchMethod(invocation);
}

/// `restore` تعيد null، فتبقى `user` فارغة وتسقط الشاشة إلى «د.ل».
/// اختبار تخطيط لا يحتاج AgentUser كاملاً.
class FakeAuthRepo implements AuthRepository {
  @override
  Future<AgentUser?> restore() async => null;

  @override
  dynamic noSuchMethod(Invocation invocation) => super.noSuchMethod(invocation);
}

/// بلا هذا يقيس الاختبار خطّاً بديلاً لا خطَّ التطبيق.
///
/// `flutter test` لا يحمّل خطوط الحزمة، فيرسم كل محرف مربّعاً بعرض ثابت —
/// وهي مقاييس لا تشبه Noto Kufi ولا IBM Plex. اختبار تخطيط مبنيّ عليها
/// يرفض تصميماً سليماً أو يقبل واحداً يفيض على الجهاز.
Future<void> loadAppFonts() async {
  Future<void> load(String family, List<String> paths) async {
    final loader = FontLoader(family);
    for (final p in paths) {
      loader.addFont(
        File(p).readAsBytes().then((b) => ByteData.view(b.buffer)),
      );
    }
    await loader.load();
  }

  await load('NotoKufiArabic', ['assets/fonts/NotoKufiArabic.ttf']);
  await load('IBMPlexSansArabic', [
    'assets/fonts/IBMPlexSansArabic-Regular.ttf',
    'assets/fonts/IBMPlexSansArabic-Medium.ttf',
    'assets/fonts/IBMPlexSansArabic-SemiBold.ttf',
  ]);
}

/// يبني [screen] على مقاس جهاز متوسّط ويعيد قائمتها القابلة للتمرير.
Future<ScrollableState> pumpAtPhoneSize(
  WidgetTester tester,
  Widget screen, {
  List<Override> overrides = const [],

  /// اجعله false لشاشةٍ فيها حركة لا تتوقّف — لمعان البطاقة يتكرّر كل سبع
  /// ثوانٍ مثلاً، و`pumpAndSettle` ينتظر سكوناً لا يأتي فينتهي بمهلة.
  bool settle = true,
}) async {
  tester.view.physicalSize = kPhone;
  tester.view.devicePixelRatio = 1.0;
  addTearDown(tester.view.reset);

  await tester.pumpWidget(
    ProviderScope(
      overrides: [
        secureStoreProvider.overrideWithValue(FakeStore()),
        authRepositoryProvider.overrideWithValue(FakeAuthRepo()),
        ...overrides,
      ],
      child: MaterialApp(
        theme: buildTheme(),
        home: MediaQuery(
          data: const MediaQueryData(size: kPhone, padding: kPadding),
          child: screen,
        ),
      ),
    ),
  );
  if (settle) {
    await tester.pumpAndSettle();
  } else {
    // يكفي لإتمام التخطيط ودخول الحركات مرحلتها المستقرّة.
    await tester.pump();
    await tester.pump(const Duration(milliseconds: 800));
  }

  // ليس `find.byType(Scrollable).first` على مستوى الشجرة كلها: كل TextField
  // يحوي Scrollable خاصاً به وقد يسبق القائمة، فيقيس الاختبار تمرير حقلٍ
  // ويعلن النجاح دائماً. القيد على ListView ثم أوّل مطابقة داخله = القائمة.
  return tester.state<ScrollableState>(
    find
        .descendant(of: find.byType(ListView), matching: find.byType(Scrollable))
        .first,
  );
}

/// يفشل برقمٍ يقول كم يلزم تقليصه، لا بمجرّد «لم ينجح».
void expectNoScroll(ScrollableState scrollable) {
  final overflow = scrollable.position.maxScrollExtent;
  expect(
    overflow,
    0.0,
    reason: 'المحتوى يفيض عن الشاشة بمقدار ${overflow.toStringAsFixed(1)} dp '
        '— يحتاج تقليصاً في المسافات أو الأحجام، لا تمريراً.',
  );
}

/// يُستدعى **بعد** [expectNoScroll]: قبل ذلك تكون العناصر الأخيرة خارج
/// النافذة فلا تُبنى أصلاً، فيصير غيابها نتيجةَ الفيض لا سبباً مستقلاً.
void expectAllVisible(List<String> labels) {
  for (final label in labels) {
    expect(find.text(label), findsOneWidget, reason: 'غاب «$label»');
  }
}
