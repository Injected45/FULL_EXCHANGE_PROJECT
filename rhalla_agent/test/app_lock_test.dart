import 'package:flutter/widgets.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:local_auth/local_auth.dart';
import 'package:rhalla_agent/core/storage/secure_store.dart';
import 'package:rhalla_agent/features/security/app_lock.dart';
import 'package:rhalla_agent/features/security/otp_paste.dart';

/// قفلُ التطبيق بعد الخمول — القرارُ نفسُه لا شكلُ الشاشة.
///
/// ⚠ **الخطرُ هنا في الوقت**، والوقتُ لا يُرى في التشغيل: قفلٌ لا يقع بعد
/// ساعةٍ في جيبٍ مسروق لا يظهر عطبُه إلّا يومَ يُسرق الهاتف. فيُقاد الزمن
/// هنا يدوياً بدل انتظاره.

/// مُخزِّنٌ في الذاكرة — يحفظ الطابع كما يحفظه التخزين الآمن.
class _FakeStore extends SecureStore {
  DateTime? _at;
  bool _bio = false;

  /// جلسةٌ قائمة. `null` = لم يدخل أحدٌ بعد.
  String? token = 'رمز-جلسة';

  @override
  Future<String?> readToken() async => token;

  @override
  Future<DateTime?> readBackgroundedAt() async => _at;

  @override
  Future<void> writeBackgroundedAt(DateTime at) async => _at = at.toUtc();

  @override
  Future<void> clearBackgroundedAt() async => _at = null;

  @override
  Future<bool> readBiometricUnlock() async => _bio;

  @override
  Future<void> writeBiometricUnlock(bool on) async => _bio = on;
}

/// مصادقةٌ حيويّة مزيّفة — تُعيد ما يُملى عليها.
class _FakeAuth implements LocalAuthentication {
  _FakeAuth({this.supported = true, this.kinds = const [BiometricType.fingerprint]});

  bool supported;
  List<BiometricType> kinds;
  bool result = true;
  Object? throws;
  int calls = 0;

  @override
  Future<bool> isDeviceSupported() async => supported;

  @override
  Future<bool> get canCheckBiometrics async => supported;

  @override
  Future<List<BiometricType>> getAvailableBiometrics() async => kinds;

  @override
  Future<bool> authenticate({
    required String localizedReason,
    Iterable<Object> authMessages = const [],
    AuthenticationOptions options = const AuthenticationOptions(),
  }) async {
    calls++;
    if (throws != null) throw throws!;
    return result;
  }

  @override
  Future<bool> stopAuthentication() async => true;

  @override
  dynamic noSuchMethod(Invocation invocation) => super.noSuchMethod(invocation);
}

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  late _FakeStore store;
  late _FakeAuth auth;
  late AppLockController c;

  Future<void> settle() => Future<void>.delayed(const Duration(milliseconds: 20));

  setUp(() async {
    store = _FakeStore();
    auth = _FakeAuth();
    c = AppLockController(store, auth);
    await settle();
  });

  tearDown(() => c.dispose());

  group('الإقلاع', () {
    test('يُقلع مفتوحاً — لا شاشةَ بصمةٍ فوق شاشة دخول', () async {
      expect(c.state.phase, LockPhase.open);
    });

    test('⚠ ويمحو طابعَ مغادرةٍ سابقٍ لإغلاق التطبيق', () async {
      // غيابٌ سبق الإغلاق ليس خمولاً في جلسةٍ قائمة.
      expect(await store.readBackgroundedAt(), isNull);
    });

    test('ويقرأ توفّرَ البصمة', () async {
      expect(c.state.biometricsAvailable, isTrue);
    });
  });

  group('المغادرة والعودة', () {
    test('عودةٌ قبل خمس دقائق لا تقفل', () async {
      c.didChangeAppLifecycleState(AppLifecycleState.paused);
      await settle();

      // غابَ دقيقتين.
      await store.writeBackgroundedAt(
          DateTime.now().toUtc().subtract(const Duration(minutes: 2)));

      c.didChangeAppLifecycleState(AppLifecycleState.resumed);
      await settle();

      expect(c.state.phase, LockPhase.open);
    });

    test('⚠ وعودةٌ بعد أكثر من خمس دقائق تقفل', () async {
      c.didChangeAppLifecycleState(AppLifecycleState.paused);
      await settle();

      await store.writeBackgroundedAt(
          DateTime.now().toUtc().subtract(const Duration(minutes: 6)));

      c.didChangeAppLifecycleState(AppLifecycleState.resumed);
      await settle();

      expect(c.state.isLocked, isTrue);
    });

    test('⚠ وساعةٌ كاملة تقفل كذلك — لا يُتحايل بإطالة الغياب', () async {
      await store.writeBackgroundedAt(
          DateTime.now().toUtc().subtract(const Duration(hours: 3)));
      c.didChangeAppLifecycleState(AppLifecycleState.resumed);
      await settle();

      expect(c.state.isLocked, isTrue);
    });

    test('⚠ و`inactive` لا تُسجّل مغادرة — وإلّا أقفلت نافذةُ البصمة نفسُها',
        () async {
      c.didChangeAppLifecycleState(AppLifecycleState.inactive);
      await settle();
      expect(await store.readBackgroundedAt(), isNull);
    });

    test('⚠ والطابعُ يُكتب مرّةً واحدة رغم `paused` ثم `detached`', () async {
      c.didChangeAppLifecycleState(AppLifecycleState.paused);
      await settle();
      final first = await store.readBackgroundedAt();

      await Future<void>.delayed(const Duration(milliseconds: 30));
      c.didChangeAppLifecycleState(AppLifecycleState.detached);
      await settle();

      expect(await store.readBackgroundedAt(), first,
          reason: 'الثانيةُ كانت ستؤخّر الطابع فتُقصّر الغياب المحسوب');
    });

    test('والطابعُ يُمحى بعد القراءة — فلا يُقفل مرّتين بغيابٍ واحد', () async {
      await store.writeBackgroundedAt(
          DateTime.now().toUtc().subtract(const Duration(minutes: 9)));
      c.didChangeAppLifecycleState(AppLifecycleState.resumed);
      await settle();

      expect(await store.readBackgroundedAt(), isNull);
    });
  });

  group('⚠ لا قفلَ بلا جلسة', () {
    test('من ترك شاشةَ الدخول ستّ دقائق يعود إليها لا إلى قفل', () async {
      // كان يجد «التطبيق مقفل» فوق شاشة دخول، ويضغط «فتح بالبصمة»
      // ليصل إلى… شاشة الدخول نفسِها.
      store.token = null;
      await store.writeBackgroundedAt(
          DateTime.now().toUtc().subtract(const Duration(minutes: 6)));

      c.didChangeAppLifecycleState(AppLifecycleState.resumed);
      await settle();

      expect(c.state.isLocked, isFalse);
    });

    test('والقفلُ الفوريّ كذلك لا يقع بلا جلسة', () async {
      store.token = null;
      await c.lockNow();
      expect(c.state.isLocked, isFalse);
    });

    test('ومع جلسةٍ قائمة يقع', () async {
      await c.lockNow();
      expect(c.state.isLocked, isTrue);
    });
  });

  group('الفتح', () {
    Future<void> lock() async {
      await store.writeBackgroundedAt(
          DateTime.now().toUtc().subtract(const Duration(minutes: 10)));
      c.didChangeAppLifecycleState(AppLifecycleState.resumed);
      await settle();
    }

    test('بصمةٌ ناجحة تفتح', () async {
      await lock();
      expect(await c.unlockWithBiometrics(), isTrue);
      expect(c.state.phase, LockPhase.open);
    });

    test('⚠ وبصمةٌ فاشلة تُبقيه مقفلاً', () async {
      await lock();
      auth.result = false;
      expect(await c.unlockWithBiometrics(), isFalse);
      expect(c.state.isLocked, isTrue);
    });

    test('⚠ وإلغاءُ المستخدم ليس خطأً يُعرض', () async {
      await lock();
      auth.result = false;
      await c.unlockWithBiometrics();
      expect(c.state.lastError, isNull);
    });

    test('واستثناءُ المنصّة يصير جملةً يفهمها المستخدم', () async {
      await lock();
      auth.throws = Exception('NotEnrolled');
      await c.unlockWithBiometrics();

      expect(c.state.lastError, isNotNull);
      expect(c.state.lastError, contains('بصمة'));
      // ⚠ ولا أثرَ مكدّسٍ ولا اسمَ استثناء.
      expect(c.state.lastError, isNot(contains('Exception')));
    });

    test('⚠ ولمستان لا تُنتجان نداءين', () async {
      await lock();
      final a = c.unlockWithBiometrics();
      final b = c.unlockWithBiometrics();
      await Future.wait([a, b]);

      expect(auth.calls, 1, reason: 'الثاني كان يُرفض فيُعرض فشلٌ لم يقع');
    });
  });

  group('توفّر البصمة', () {
    test('⚠ جهازٌ يدعم العتاد بلا بصمةٍ مسجّلة = لا بصمة', () async {
      final a = _FakeAuth(kinds: const []);
      final ctl = AppLockController(_FakeStore(), a);
      await settle();

      expect(ctl.state.biometricsAvailable, isFalse,
          reason: 'زرُّ بصمةٍ يُخفق دائماً أسوأ من غيابه');
      ctl.dispose();
    });

    test('وجهازٌ لا يدعم أصلاً', () async {
      final ctl = AppLockController(_FakeStore(), _FakeAuth(supported: false));
      await settle();
      expect(ctl.state.biometricsAvailable, isFalse);
      ctl.dispose();
    });
  });

  group('القفل الفوريّ', () {
    test('يقفل بلا انتظار الخمس دقائق', () async {
      await c.lockNow();
      expect(c.state.isLocked, isTrue);
    });
  });

  group('⚠ ما يُعرض قبل أن يُعرف الحال', () {
    test('يُستر في `unknown` — لا تُومض الأرصدة ثم تُخفى', () {
      const s = AppLockState();
      expect(s.phase, LockPhase.unknown);
      expect(s.shouldHide, isTrue);
    });

    test('ويُعرض في `open` وحدَها', () {
      expect(const AppLockState(phase: LockPhase.open).shouldHide, isFalse);
      expect(const AppLockState(phase: LockPhase.locked).shouldHide, isTrue);
    });
  });

  group('استخراجُ الرمز من الحافظة', () {
    test('من رسالة واتساب كما هي', () {
      expect(extractOtp('رمز التحقق الخاص بك هو: 4821'), '4821');
    });

    test('ومن الرقم مجرّداً', () {
      expect(extractOtp('4821'), '4821');
      expect(extractOtp('  4821  '), '4821');
    });

    test('⚠ ولا يُقتطع من رقمٍ أطول — رقمُ هاتفٍ في الحافظة', () {
      // `0922015243` كانت ستُعطي «0922» فيُستهلك من عدد المحاولات.
      expect(extractOtp('0922015243'), isNull);
      expect(extractOtp('اتصل بي على 0922015243'), isNull);
    });

    test('⚠ ولا يُخمَّن بين رمزين محتملين', () {
      expect(extractOtp('الرمز 1234 والقديم 5678'), isNull);
    });

    test('والفارغُ وغيرُ ذي الرمز', () {
      expect(extractOtp(null), isNull);
      expect(extractOtp(''), isNull);
      expect(extractOtp('مرحباً كيف حالك'), isNull);
    });

    test('ونصٌّ طويلٌ يُرفض — ليس حافظةَ رمز', () {
      expect(extractOtp('${'ا' * 500} 4821'), isNull);
    });

    test('والطولُ قابلٌ للضبط', () {
      expect(extractOtp('رمزك 481523', length: 6), '481523');
      expect(extractOtp('رمزك 481523'), isNull);
    });
  });
}
