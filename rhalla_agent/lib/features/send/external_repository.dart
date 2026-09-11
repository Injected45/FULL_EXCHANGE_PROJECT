import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import 'send_repository.dart';

/// نوع خدمة التسليم في بلد الوجهة.
class ServiceType {
  const ServiceType(this.id, this.name);
  final int id;
  final String name;

  factory ServiceType.fromJson(Map<String, dynamic> j) => ServiceType(
        int.tryParse('${j['SRID']}') ?? 0,
        '${j['SRNAME'] ?? ''}'.trim(),
      );
}

/// تسعيرة الحوالة الخارجية — من `/device/external/quote`.
///
/// ⚠️ لا تُستعمل `/device/external/get/exchange`: قيمتها `sale_price` تأتي من
/// `SalePrice_mo_Value(currency_id, …)` بينما مُشغِّل `ExternalEx` يستدعي
/// نفس الدالة بـ `SalePrice_mo_Value(CountryIDTo, …)` — وسيط أول مختلف
/// ونتيجة مختلفة تماماً: لـ 5 د.ل إلى مصر تعيد 2 بينما الصف المُدرَج فعلاً
/// يسجّل `NetTotal = 19`. عرضها للوكيل تسعير خاطئ للزبون.
///
/// ⚠⚠ **وهذا السطرُ كان يقول «تكرّر حساب المُشغِّل حرفياً» — وهو ما صُحِّح
/// في 11 سبتمبر 2026**، حين بلّغ المالك أنّ المعروض يخالف المنفَّذ.
///
/// فنسخةُ استعلامِ السعر داخل المحفّز **ينقصها شرطُ الخدمة** (`b.BankID`)
/// الموجودُ في `dbo.SalePrice_mo_Value`. فخمسةُ صفوفٍ تتأهّل لمصر، ويبقى
/// آخرُها اعتباطاً — سعراً لا يخصّ الخدمة المختارة. ومقيسٌ على القاعدة:
/// تسعُ حوالاتٍ من عشرٍ كُتبت بـ٥٫٣٠٠ وسعرُ خدمتها المُدرج ٥٫٥٥٠، وكلُّها
/// بعمولةِ خدمةٍ سالبة.
///
/// فمحاكاةُ المحفّز كانت ستنسخ العطبَ وتعرضه صواباً. والمرجعُ الآن **دالّةُ
/// المنظومة** التي تحسب `NetTotal` — أي القائمةُ المعتمدة نفسُها التي
/// تعرضها شاشةُ «أسعار العملات»، فالثلاثةُ رقمٌ واحد.
///
/// ⚠ ويبقى فرقٌ حتى يُصلَح المحفّز نفسُه (سطرٌ واحد، داخلَ الخطّ الأحمر
/// الماليّ ولا يُمسّ إلّا بأمرِ المالك):
/// `backend/database/sql/pending/2026-09-11_external_price_service_filter.sql`.
class ExternalQuote {
  const ExternalQuote({
    required this.rate,
    required this.delivered,
    required this.net,
    required this.serviceFee,
    required this.currencyCode,
  });

  /// `TransPrice` — سعر الصرف المطبَّق.
  final double rate;

  /// `CurrDeliveredVal` = المبلغ × السعر.
  final double delivered;

  /// `NetTotal` — **ما يستلمه المستفيد فعلاً** بعملة الوجهة.
  final double net;

  /// `ServiceExVal` = delivered − net.
  final double serviceFee;

  /// رمز عملة الوجهة، مثل «ج.م».
  final String currencyCode;

  factory ExternalQuote.fromJson(Map<String, dynamic> j) {
    final cur = j['DeliveredCurrency'];
    return ExternalQuote(
      rate: Fmt.num_(j['TransPrice']),
      delivered: Fmt.num_(j['CurrDeliveredVal']),
      net: Fmt.num_(j['NetTotal']),
      serviceFee: Fmt.num_(j['ServiceExVal']),
      currencyCode:
          cur is Map ? '${cur['CurCode'] ?? ''}'.trim() : '',
    );
  }
}

/// مسوّدة حوالة خارجية.
class ExternalDraft {
  const ExternalDraft({
    required this.country,
    required this.city,
    required this.branch,
    required this.service,
    required this.receiverName,
    required this.receiverPhone,
    required this.amountLyd,
    required this.commission,
    required this.deliveredCurrencyId,
    this.quote,
    this.notes,
  });

  final Ref2 country;
  final Ref2 city;
  final Ref2 branch;
  final ServiceType service;
  final String receiverName;
  final String receiverPhone;

  /// المبلغ بالدينار — هو ما يُخزَّن في `CurrRecievedVal`
  /// (`RecievedCurrencyID = 1` في كل الصفوف التاريخية).
  final double amountLyd;

  final double commission;

  /// عملة التسليم — عملة بلد الوجهة الافتراضية.
  final int deliveredCurrencyId;

  final ExternalQuote? quote;
  final String? notes;

  double get total => amountLyd + commission;
}

/// ما تحمله شاشةُ «تمّت الحوالة الخارجية».
///
/// ══════════════════════════════════════════════════════════════════════════
///  أمرُ المالك (11 سبتمبر 2026)
/// ══════════════════════════════════════════════════════════════════════════
///
/// «عند تنفيذ حوالة خارجية أريد أن تظهر شاشةُ تمّت الحوالة بنجاح **مثل شكل
///  ومظهر الداخلية** مع اختلاف البيانات: حيث الخارجية تُظهر سعرَ الصرف
///  والقيمةَ بالعملة المحلية وكم بالعملة المحوَّل لها، واسمَ الخدمة والدولة
///  والمدينة أو الخدمة حسب الاختيار من إنشاء الحوالة».
///
/// ⚠ وموضعُه هنا لا في شاشة النموذج: صار **شاشتان** تبنيانه — شاشةُ المراجعة
/// تبنيه بعد الإنشاء، والشاشةُ التي تعرضه. وصنفٌ خاصٌّ داخل إحداهما لا
/// تراه الأخرى.
class ExternalDoneArgs {
  const ExternalDoneArgs({
    required this.code,
    required this.favoriteCode,
    required this.name,
    required this.phone,
    required this.amount,
    required this.commission,
    required this.net,
    required this.rate,
    required this.currencyCode,
    this.country = '',
    this.city = '',
    this.service = '',
  });

  /// الرمز المعروض للوكيل والمستفيد.
  final String code;

  /// `ExternalEx.Code` — **وليس** رمز الموبايل. المفضّلة تُخزَّن به لأن
  /// `Favorites_GetByUserID` يربط `code_Favorite` بعمود `Code` وحده، فحفظ
  /// رمز الموبايل يعني صفّاً لا يظهر في القائمة أبداً.
  final String favoriteCode;
  final String name;
  final String phone;

  /// المقبوضُ بالدينار، والعمولة — بالعملة المحلّية.
  final double amount;
  final double commission;

  /// ما يستلمه المستفيد بعملة الوجهة — من الصف المُدرَج.
  final double net;

  /// سعرُ الصرف كما كتبه المحفّز بعد الإدراج.
  final double rate;
  final String currencyCode;

  /// الوجهةُ كما اختارها المُرسِل في النموذج.
  final String country;
  final String city;
  final String service;

  double get total => amount + commission;
}

class ExternalRepository {
  ExternalRepository(this._api);

  final ApiClient _api;

  /// الدينار الليبي — العملة المستلَمة من المرسل دائماً.
  static const lydId = 1;

  /*
   * ══════════════════════════════════════════════════════════════════════
   *  ⚠ الشاشةُ واحدة، والمسارُ يختلف — كما في الحوالة المحلّية حرفاً بحرف
   * ══════════════════════════════════════════════════════════════════════
   *
   * أمرُ إعادة الهيكلة (10 سبتمبر 2026): الحوالةُ الخارجية «تُحضَر من تطبيق
   * الوكيل **طبق الأصل**». فشاشةُ `SendExternalScreen` هي هي في التطبيقين،
   * والمختلفُ الحارسُ وحدَه: رمزُ الموظف لا يفتح مسارات الوكيل
   * (`auth:sanctum`)، فتُنادى نظائرُها تحت `employee:CREATE_EXTERNAL_TRANSFER`.
   *
   * ⚠ ولا منطقَ ماليَّ ثانٍ خلف تلك النظائر: الخادمُ ينادي
   * `transInsertExternal` نفسَها بهويّة الوكيل (`EmployeeActsAsAgent`)،
   * فيخرج في `ExternalEx` صفٌّ لا يُميَّز عن صفّ الوكيل — لأنه صفُّه.
   *
   * ⚠ والتخزينُ يحمل رمزاً واحداً لا اثنين (انظر `SecureStore.readToken`)،
   * فوجودُ رمز الموظف هو **تعريفُ** الوضع لا تخمينٌ له. وهي القاعدةُ نفسُها
   * في `SendRepository._asEmployee` — لا طريقةٌ ثانية لمعرفة الوضع.
   */
  Future<bool> _asEmployee() async =>
      (await _api.store.readEmployeeToken())?.isNotEmpty ?? false;

  Future<String> _path(String agentPath, String employeePath) async =>
      await _asEmployee() ? employeePath : agentPath;

  /// مرجعُ الدول — نقطةُ الوكيل، أو نظيرُها تحت جلسة الموظف.
  ///
  /// ⚠ ونقطةُ الموظف هي `ref/countries` نفسُها التي تستعملها الحوالةُ
  /// المحلّية، لا نقطةٌ ثالثة: قائمتان للدول تفترقان يوماً ما.
  Future<String> _countriesPath() async =>
      _path('/device/countries', '/device/employee/ref/countries');

  /// الدول التي تقبل حوالة خارجية فعلاً.
  ///
  /// ⚠️ يجب الترشيح بـ `IsService = 1`: بقية الدول تعيد قائمة أنواع خدمة
  /// **فارغة**، فيصل الوكيل إلى طريق مسدود بعد اختيارها.
  Future<List<Ref2>> serviceCountries() async {
    final env =
        await _api.post(await _countriesPath(), body: {'country_id': 0});
    return env.rows
        .where((r) => '${r['IsService']}' == '1' && '${r['IsActive']}' == '1')
        .map(Ref2.country)
        .toList();
  }

  /// عملة الوجهة الافتراضية — من `DefualtCurrency` في صف الدولة.
  Future<int> defaultCurrencyOf(int countryId) async {
    final env =
        await _api.post(await _countriesPath(), body: {'country_id': 0});
    for (final r in env.rows) {
      if ('${r['ID']}' == '$countryId') {
        return int.tryParse('${r['DefualtCurrency']}') ?? 0;
      }
    }
    return 0;
  }

  Future<List<ServiceType>> services(int countryId) async {
    try {
      final env = await _api.post(
          await _path('/device/service/external/transfer',
              '/device/employee/external/services'),
          body: {'country_id': countryId});
      return env.rows.map(ServiceType.fromJson).toList();
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  /// تسعيرة قبل الإرسال — من القائمة المعتمدة لخدمةِ الوجهة (انظر ترويسة
  /// [ExternalQuote]). وخدمةٌ بلا سعرٍ معرَّف تُردّ بـ422 ورسالةٍ صريحة،
  /// ولا يُستعار لها سعرُ خدمةٍ أخرى.
  Future<ExternalQuote> quote({
    required int countryIdTo,
    required double amount,
    required int serviceType,
  }) async {
    final env = await _api.post(
        await _path(
            '/device/external/quote', '/device/employee/external/quote'),
        body: {
      'CountryIDTo': countryIdTo,
      'CurrRecievedVal': amount,
      'ServiceType': serviceType,
      'IsPrivateAccount': 0,
    });
    final p = env.payload;
    if (p is Map) return ExternalQuote.fromJson(p.cast<String, dynamic>());
    throw ApiFailure('تعذّر احتساب التسعيرة.',
        statusCode: env.statusCode, envelope: env);
  }

  /// إنشاء حوالة خارجية.
  ///
  /// الخرائط مستنتجة من صفوف حقيقية في ExternalEx لا من الوثائق:
  ///   CountryIDFrom = 1 (ليبيا) · CountryIDTo = بلد الوجهة
  ///   RecievedCurrencyID = 1 (دينار — ما يُستلم من المرسل)
  ///   DeliveredCurrencyID = عملة الوجهة
  ///   RecievedBranchID = فرع ليبي (القيم التاريخية 1 و2)
  ///   CurrRecievedVal = المبلغ بالدينار · Commition → يُخزَّن ExVal
  ///
  /// `AccFrom` مطلوب في التحقق لكن الإدراج يستعمل `$user->AccID` — يُرسل ولا يؤثّر.
  /// [clientId] مفتاحُ الطلب — يمنع ازدواجَ الحوالة عند تكرار الضغط أو
  /// إعادة الإرسال بعد انقطاع.
  ///
  /// ⚠ **كان غائباً عن هذا المسار وحدَه.** نقطةُ الموظف
  /// (`employee/external/create`) تقرأ `client_id` وتحجزه في
  /// `employee_transfer_claims` قبل أيّ كتابةٍ مالية — تماماً كنقطة الحوالة
  /// المحلّية — لكنّ التطبيق لم يكن يرسله، فكان الحارسُ قائماً في الخادم
  /// ومعطَّلاً من الطرف الآخر.
  ///
  /// ويُولّد **مرّةً لكلّ محاولة** في شاشة المراجعة لا عند كلّ نداء: مفتاحٌ
  /// جديد مع كلّ إعادةٍ يُبطل الحمايةَ من أصلها.
  Future<Map<String, dynamic>> create({
    required ExternalDraft d,
    required int accId,
    String? senderName,
    String? senderPhone,
    String? clientId,
  }) async {
    final env = await _api.post(
        await _path('/device/external/insert/transfer',
            '/device/employee/external/create'),
        body: {
      'RecievedCurrencyID': lydId,
      'CountryIDFrom': SendRepository.libyaId,
      'CountryIDTo': d.country.id,
      'CityIDTo': d.city.id,
      'RecievedBranchID': d.branch.id,
      'DeliveredCurrencyID': d.deliveredCurrencyId,
      'ServiceType': d.service.id,
      'RecievedName': d.receiverName.trim(),
      'RPhone1': d.receiverPhone.trim(),
      'CurrRecievedVal': d.amountLyd,
      'AccFrom': accId,
      'IsPrivateAccount': 0,
      'Commition': d.commission,
      if (senderName != null && senderName.trim().isNotEmpty)
        'SenderName': senderName.trim(),
      if (senderPhone != null && senderPhone.trim().isNotEmpty)
        'SPhone1': senderPhone.trim(),
      if (d.notes != null && d.notes!.trim().isNotEmpty) 'Notes': d.notes!.trim(),
      if (clientId != null && clientId.isNotEmpty) 'client_id': clientId,
    });

    final p = env.payload;
    if (p is Map && p['transfer'] is Map) {
      return (p['transfer'] as Map).cast<String, dynamic>();
    }
    throw ApiFailure(
      'تمّت العملية لكن رد الخادم غير متوقّع. راجع الحوالات قبل إعادة الإرسال.',
      statusCode: env.statusCode,
      envelope: env,
    );
  }
}

final externalRepositoryProvider = Provider<ExternalRepository>(
    (ref) => ExternalRepository(ref.watch(apiClientProvider)));

final serviceCountriesProvider = FutureProvider.autoDispose<List<Ref2>>(
    (ref) => ref.watch(externalRepositoryProvider).serviceCountries());

final servicesProvider =
    FutureProvider.autoDispose.family<List<ServiceType>, int>(
        (ref, countryId) =>
            ref.watch(externalRepositoryProvider).services(countryId));

final destCitiesProvider = FutureProvider.autoDispose.family<List<Ref2>, int>(
    (ref, countryId) =>
        ref.watch(sendRepositoryProvider).cities(countryId: countryId));
