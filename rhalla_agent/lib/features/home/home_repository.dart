import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';

class Limits {
  const Limits({this.daily = 0, this.weekly = 0, this.monthly = 0, this.annual = 0});

  final double daily;
  final double weekly;
  final double monthly;
  final double annual;

  /// الرد يأتي تحت المفتاح `datat` لا `data` — انظر Envelope.
  /// وأسماء الحقول كما يكتبها الخادم: Daily, monthly, Annual, Weekly.
  factory Limits.fromJson(Map<String, dynamic> j) => Limits(
        daily: Fmt.num_(j['Daily']),
        weekly: Fmt.num_(j['Weekly']),
        monthly: Fmt.num_(j['monthly']),
        annual: Fmt.num_(j['Annual']),
      );
}

class Movement {
  const Movement({
    required this.title,
    required this.date,
    required this.amount,
    required this.isCredit,
    required this.balance,
  });

  final String title;
  final String date;
  final double amount;
  final bool isCredit;
  final double balance;

  /// أعمدة كشف الحساب — لاحظ `Balnce` بهذا الإملاء.
  ///
  /// ⚠️ `Type_from` **نص لا رقم**: يعيده الخادم كـ 'ايداع' أو 'خصم'
  /// (CASE على AccDmType و Debit/Credit في depositController)،
  /// و`Values_to` هو المبلغ المطلق أياً كان الاتجاه.
  /// قراءة Type_from كمبلغ تجعل كل حركة تبدو واردة.
  factory Movement.fromJson(Map<String, dynamic> j) {
    final type = '${j['Type_from'] ?? ''}'.trim();
    // متسامح مع «ايداع» و«إيداع».
    final isCredit = type.contains('يداع');
    return Movement(
      title: '${j['MovementType'] ?? ''}'.trim(),
      date: '${j['InsertDate'] ?? ''}'.trim(),
      amount: Fmt.num_(j['Values_to']),
      isCredit: isCredit,
      balance: Fmt.num_(j['Balnce']),
    );
  }
}

class HomeSnapshot {
  const HomeSnapshot({
    required this.balance,
    required this.limits,
    required this.movements,
  });

  final double balance;
  final Limits limits;
  final List<Movement> movements;
}

class HomeRepository {
  HomeRepository(this._api);

  final ApiClient _api;

  /// العملة المحلية. المعرّف يجب تأكيده من المكتب الخلفي — مفترض 1.
  static const localCurrencyId = 1;

  Future<double> balance() async {
    final env = await _api.post('/device/current/balance/local/currency',
        body: {'currency_id': localCurrencyId});
    // يعيد مصفوفة من كائن واحد، لا رقماً.
    final row = env.row;
    return Fmt.num_(row?['Walet']);
  }

  Future<Limits> limits() async {
    try {
      final env = await _api.post('/device/Daily_transfer', body: {});
      final row = env.row;
      return row == null ? const Limits() : Limits.fromJson(row);
    } on ApiFailure catch (e) {
      // 404 هنا تعني «لا توجد بيانات» لا خطأً.
      if (e.isEmptyResult) return const Limits();
      rethrow;
    }
  }

  Future<List<Movement>> movements({int take = 5}) async {
    try {
      final env = await _api.get('/device/local/account/statment');
      // لا ترقيم صفحات في الخادم — يعيد التاريخ كاملاً. نقتطع محلياً.
      return env.rows.take(take).map(Movement.fromJson).toList();
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  Future<HomeSnapshot> load() async {
    final results = await Future.wait([
      balance(),
      limits(),
      movements(),
    ]);
    return HomeSnapshot(
      balance: results[0] as double,
      limits: results[1] as Limits,
      movements: results[2] as List<Movement>,
    );
  }
}

final homeRepositoryProvider =
    Provider<HomeRepository>((ref) => HomeRepository(ref.watch(apiClientProvider)));

final homeSnapshotProvider = FutureProvider.autoDispose<HomeSnapshot>(
  (ref) => ref.watch(homeRepositoryProvider).load(),
);
