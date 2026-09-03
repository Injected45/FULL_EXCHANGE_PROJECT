import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';

/// نوع المفضّلة كما يُخزَّن في `Type_Favorite`.
///
/// **الخادم لا يستعمل هذا الحقل في الاستعلام إطلاقاً:** إجراء
/// `Favorites_GetByUserID` يربط `code_Favorite` بعمود `Code` في الجداول
/// الثلاثة (‏`InternalEx` · `ExternalEx` · `TransBetweenAccountsTB`) بلا
/// تصفية بالنوع، ثم يُعيده كما وصله. فهو تصنيفٌ للعرض عندنا لا شرطُ
/// استعلام، **والأرقام اصطلاحُنا نحن** — لا تُغيَّر بعد أول نشر وإلا صارت
/// الصفوف القديمة موسومة بنوع خاطئ.
enum FavoriteKind {
  internal(1, 'حوالة محلية'),
  external(2, 'حوالة خارجية'),
  accounts(3, 'بين الحسابات');

  const FavoriteKind(this.code, this.label);

  final int code;
  final String label;

  static FavoriteKind from(int v) => switch (v) {
        2 => FavoriteKind.external,
        3 => FavoriteKind.accounts,
        _ => FavoriteKind.internal,
      };
}

/// صفّ مفضّلة واحد كما يعيده الإجراء المخزَّن.
///
/// القراءة متسامحة عمداً: الأعمدة تأتي من إجراء مخزَّن لا من كود Laravel،
/// فأسماؤها غير مضمونة الحالة بين SQL Server وMariaDB.
class Favorite {
  const Favorite({
    required this.id,
    required this.name,
    required this.phone,
    required this.code,
    required this.kind,
    required this.amount,
    required this.addedAt,
  });

  final int id;
  final String name;
  final String phone;

  /// رمز الحوالة — هو مفتاح الحذف أيضاً (`code_Favorite`).
  final String code;
  final FavoriteKind kind;

  /// مبلغ تلك الحوالة، يُعرض كآخر تعامل مع هذا المستفيد.
  final double amount;
  final String addedAt;

  static dynamic _pick(Map<String, dynamic> j, List<String> keys) {
    for (final k in keys) {
      if (j.containsKey(k)) return j[k];
    }
    final lower = {for (final e in j.entries) e.key.toLowerCase(): e.value};
    for (final k in keys) {
      final v = lower[k.toLowerCase()];
      if (v != null) return v;
    }
    return null;
  }

  factory Favorite.fromJson(Map<String, dynamic> j) => Favorite(
        id: Fmt.num_(_pick(j, ['ID'])).toInt(),
        name: '${_pick(j, ['Name_to', 'RecievedName', 'AccName']) ?? ''}'.trim(),
        phone: '${_pick(j, ['RPhone1', 'AccPhone', 'phone']) ?? ''}'.trim(),
        code: '${_pick(j, ['CODE', 'Code']) ?? ''}'.trim(),
        kind: FavoriteKind.from(Fmt.num_(_pick(j, ['Type_Favorite'])).toInt()),
        amount: Fmt.num_(_pick(j, ['OverallVal'])),
        addedAt: '${_pick(j, ['insertdate']) ?? ''}'.trim(),
      );
}

/// عميل في المفضّلة.
///
/// المفضّلة في الخادم تُخزَّن لكل **حوالة** لا لكل عميل، فمن حوّل لنفس
/// الشخص مرّتين وفضّل الحوالتين يجد اسمه مكرّراً. نجمعها بالهاتف حتى يرى
/// الوكيل عميلاً واحداً، والحذف يزيل كل صفوفه لا صفاً واحداً — وإلا عاد
/// الاسم للظهور بعد الحذف وبدا كأن الحذف لم يعمل.
class FavoriteCustomer {
  const FavoriteCustomer({required this.entries});

  /// مرتّبة من الأحدث إلى الأقدم.
  final List<Favorite> entries;

  Favorite get latest => entries.first;
  String get name => latest.name;
  String get phone => latest.phone;
  double get lastAmount => latest.amount;
  FavoriteKind get kind => latest.kind;

  String get initial => name.isEmpty ? '؟' : name.substring(0, 1);

  bool matches(String q) {
    final t = q.trim();
    if (t.isEmpty) return true;
    return name.contains(t) || phone.contains(t.replaceAll(RegExp(r'\D'), ''));
  }
}

class FavoritesRepository {
  FavoritesRepository(this._api);

  final ApiClient _api;

  /// [kind] غير فارغ ⇦ نوع واحد فقط.
  ///
  /// **قرار المالك:** مستفيدو الحوالات الداخلية لا يظهرون في مفضّلة
  /// الخارجية ولا العكس — العملاء مختلفون، وخلطهم يُكدّس أسماءً لا تصلح
  /// للوجهة أصلاً (رقم ليبي في حوالة إلى مصر مثلاً).
  Future<List<FavoriteCustomer>> customers({FavoriteKind? kind}) async {
    final rows = (await _all()).where((f) => kind == null || f.kind == kind);

    final groups = <String, List<Favorite>>{};
    for (final f in rows) {
      // بلا هاتف لا يمكن الجمع ولا التحويل لاحقاً — نُبقيه مستقلاً بالرمز.
      final id = f.phone.isEmpty ? 'code:${f.code}' : f.phone;
      // المفتاح يشمل النوع: عميلٌ فُضّل في الداخلية غير عميلٍ فُضّل في
      // الخارجية ولو تطابق الرقم. بدون هذا يندمجان في صفّ واحد فيُنسَب
      // إلى أحد النوعين خطأً.
      groups.putIfAbsent('${f.kind.code}:$id', () => <Favorite>[]).add(f);
    }

    final out = groups.values.map((e) {
      e.sort((a, b) => b.id.compareTo(a.id));
      return FavoriteCustomer(entries: e);
    }).toList();
    out.sort((a, b) => b.latest.id.compareTo(a.latest.id));
    return out;
  }

  Future<List<Favorite>> _all() async {
    try {
      final env = await _api.post('/device/exchange/Favorites_ALL');
      return env.rows
          .map(Favorite.fromJson)
          .where((f) => f.code.isNotEmpty)
          .toList();
    } on ApiFailure catch (e) {
      // 404 هنا تعني «لا مفضّلة» لا عطلاً.
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  /// يعيد `false` إن كان مضافاً مسبقاً (الخادم يرد 409).
  ///
  /// وهذه ليست حالة فشل تُعرض للوكيل كخطأ: النتيجة التي أرادها متحقّقة —
  /// المستفيد في مفضّلته.
  Future<bool> add({
    required String code,
    required FavoriteKind kind,
    required String phone,
  }) async {
    try {
      await _api.post('/device/exchange/Favorites_Table_add', body: {
        'code_Favorite': code,
        'Type_Favorite': kind.code,
        // الخادم يشترط الهاتف ويرفض الطلب بدونه (‏required|string|max:50).
        'phone': phone,
      });
      return true;
    } on ApiFailure catch (e) {
      if (e.statusCode == 409) return false;
      rethrow;
    }
  }

  /// يحذف كل صفوف هذا العميل — لا صفاً واحداً. انظر [FavoriteCustomer].
  Future<void> removeCustomer(FavoriteCustomer c) async {
    for (final f in c.entries) {
      await _api.post('/device/exchange/Favorites_Table_delete_from', body: {
        'code_Favorite': f.code,
        'Type_Favorite': f.kind.code,
      });
    }
  }
}

final favoritesRepositoryProvider = Provider<FavoritesRepository>(
    (ref) => FavoritesRepository(ref.watch(apiClientProvider)));

/// كل المفضّلة — لشاشة الإدارة في «الحساب».
final favoritesProvider = FutureProvider.autoDispose<List<FavoriteCustomer>>(
    (ref) => ref.watch(favoritesRepositoryProvider).customers());

/// مفضّلة نوعٍ واحد — لورقة الاختيار داخل نموذج الحوالة.
final favoritesOfKindProvider =
    FutureProvider.autoDispose.family<List<FavoriteCustomer>, FavoriteKind>(
  (ref, kind) => ref.watch(favoritesRepositoryProvider).customers(kind: kind),
);

/// يُستدعى بعد كل إضافة أو حذف — القائمتان تشتقّان من نفس الجدول.
void invalidateFavorites(WidgetRef ref) {
  ref.invalidate(favoritesProvider);
  ref.invalidate(favoritesOfKindProvider);
}
