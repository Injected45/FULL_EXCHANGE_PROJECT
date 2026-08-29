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
/// النقطة الجديدة تكرّر حساب المُشغِّل حرفياً، فما يظهر هنا هو ما يُكتب.
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

class ExternalRepository {
  ExternalRepository(this._api);

  final ApiClient _api;

  /// الدينار الليبي — العملة المستلَمة من المرسل دائماً.
  static const lydId = 1;

  /// الدول التي تقبل حوالة خارجية فعلاً.
  ///
  /// ⚠️ يجب الترشيح بـ `IsService = 1`: بقية الدول تعيد قائمة أنواع خدمة
  /// **فارغة**، فيصل الوكيل إلى طريق مسدود بعد اختيارها.
  Future<List<Ref2>> serviceCountries() async {
    final env = await _api.post('/device/countries', body: {'country_id': 0});
    return env.rows
        .where((r) => '${r['IsService']}' == '1' && '${r['IsActive']}' == '1')
        .map(Ref2.country)
        .toList();
  }

  /// عملة الوجهة الافتراضية — من `DefualtCurrency` في صف الدولة.
  Future<int> defaultCurrencyOf(int countryId) async {
    final env = await _api.post('/device/countries', body: {'country_id': 0});
    for (final r in env.rows) {
      if ('${r['ID']}' == '$countryId') {
        return int.tryParse('${r['DefualtCurrency']}') ?? 0;
      }
    }
    return 0;
  }

  Future<List<ServiceType>> services(int countryId) async {
    try {
      final env = await _api
          .post('/device/service/external/transfer', body: {'country_id': countryId});
      return env.rows.map(ServiceType.fromJson).toList();
    } on ApiFailure catch (e) {
      if (e.isEmptyResult) return const [];
      rethrow;
    }
  }

  /// تسعيرة قبل الإرسال — نفس حساب المُشغِّل.
  Future<ExternalQuote> quote({
    required int countryIdTo,
    required double amount,
    required int serviceType,
  }) async {
    final env = await _api.post('/device/external/quote', body: {
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
  Future<Map<String, dynamic>> create({
    required ExternalDraft d,
    required int accId,
    String? senderName,
    String? senderPhone,
  }) async {
    final env = await _api.post('/device/external/insert/transfer', body: {
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
