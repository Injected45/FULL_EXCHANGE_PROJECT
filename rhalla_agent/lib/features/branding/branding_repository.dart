import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/net/api_client.dart';
import '../../core/theme/tokens.dart';

/// هوية الشركة داخل التطبيق — **طبقة عرض لا غير**.
///
/// قرار المالك (2 سبتمبر 2026): لكل شركة أو وكيل هويةٌ بصرية بعد الدخول،
/// وتبقى هوية «شركة الرحالة» الرسمية قبله. ولا يجوز أن يمسّ هذا التخصيص
/// رصيداً ولا حوالةً ولا صلاحيةً ولا تقريراً — ولذلك لا يوجد في هذا الملف
/// حرفٌ واحد يقرأ مالاً أو يكتبه.
///
/// المفتاح `company_account_id` لا يُرسَل أبداً من هنا: الخادم يشتقّه من
/// التوثيق. إرساله يعني أن تعديل الطلب يدوياً يغيّر هوية شركة أخرى.

/// لون من `#RRGGBB`. أي شيء آخر يسقط إلى البديل بدل أن يرمي استثناءً —
/// خطأٌ في لونٍ يجب ألّا يمنع الوكيل من دخول التطبيق.
Color _hex(dynamic v, Color fallback) {
  if (v is! String) return fallback;
  final s = v.replaceFirst('#', '').trim();
  if (s.length != 6) return fallback;
  final n = int.tryParse(s, radix: 16);
  return n == null ? fallback : Color(0xFF000000 | n);
}

/// لوحة ألوان الشركة كما حلّها الخادم.
class BrandColors {
  const BrandColors({
    required this.primary,
    required this.secondary,
    required this.background,
    required this.surface,
    required this.onPrimary,
    required this.text,
    required this.textMuted,
    required this.border,
    required this.success,
    required this.warning,
    required this.error,
  });

  final Color primary;
  final Color secondary;
  final Color background;
  final Color surface;
  final Color onPrimary;
  final Color text;
  final Color textMuted;
  final Color border;

  /// ألوان الحالات — ثابتة في كل الثيمات بقرار المالك.
  ///
  /// الأخضر نجاح والأحمر خطأ مهما كانت ألوان الشركة. شركةٌ لونها أحمر
  /// تجعل شاشة النجاح حمراء، فيقرأ الوكيل نجاحاً على أنه فشل — وهذا في
  /// تطبيق حوالات خطأ لا يُحتمل. الخادم يفرضها، والتطبيق لا يشتقّها.
  final Color success;
  final Color warning;
  final Color error;

  static BrandColors fromJson(Map<String, dynamic> j) {
    final st = (j['status'] as Map?)?.cast<String, dynamic>() ?? const {};
    return BrandColors(
      primary: _hex(j['primary'], R.primaryDark),
      secondary: _hex(j['secondary'], R.primary),
      background: _hex(j['background'], R.bgTop),
      surface: _hex(j['surface'], const Color(0xFFFFFFFF)),
      onPrimary: _hex(j['on_primary'], const Color(0xFFFFFFFF)),
      text: _hex(j['text'], R.ink),
      textMuted: _hex(j['text_muted'], R.inkA(.55)),
      border: _hex(j['border'], R.inkA(.08)),
      success: _hex(st['success'], const Color(0xFF12A150)),
      warning: _hex(st['warning'], const Color(0xFFC77700)),
      error: _hex(st['error'], const Color(0xFFD14343)),
    );
  }
}

/// ثيم معروض في شبكة الاختيار.
class BrandTheme {
  const BrandTheme({
    required this.key,
    required this.nameAr,
    required this.primary,
    required this.secondary,
    required this.background,
  });

  final String key;
  final String nameAr;
  final Color primary;
  final Color secondary;
  final Color background;

  static BrandTheme fromJson(Map<String, dynamic> j) => BrandTheme(
        key: (j['key'] ?? '').toString(),
        nameAr: (j['name_ar'] ?? j['name_en'] ?? '').toString(),
        primary: _hex(j['primary'], R.primaryDark),
        secondary: _hex(j['secondary'], R.primary),
        background: _hex(j['background'], R.bgTop),
      );
}

class Branding {
  const Branding({
    required this.companyNameAr,
    required this.companyNameEn,
    required this.logoUrl,
    required this.themeKey,
    required this.version,
    required this.colors,
    required this.canEdit,
    required this.themes,
  });

  final String? companyNameAr;
  final String? companyNameEn;

  /// رابط الشعار كاملاً. الخادم يعيده نسبةً إلى `API_BASE`، ويُركَّب هنا
  /// مرّة واحدة — فلا ينكسر حين ينتقل التطبيق من المحاكي إلى الإنتاج.
  final String? logoUrl;

  final String themeKey;

  /// عدّاد يرفعه الخادم مع كل حفظ. لا طابع وقت: ساعات الخوادم تتزحزح،
  /// والعدّاد لا يرجع.
  final int version;

  final BrandColors colors;

  /// هل يملك هذا المستخدم تعديل الهوية؟ يقرّره الخادم (الحساب الرئيسي
  /// وحده)، ولا يُخمَّن هنا: إخفاء زرٍّ ليس حمايةً.
  final bool canEdit;

  final List<BrandTheme> themes;

  /// الاسم المعروض — العربي أولاً، ثم الإنجليزي، ثم اسم الرحالة الرسمي.
  String get displayName {
    final ar = companyNameAr?.trim() ?? '';
    if (ar.isNotEmpty) return ar;
    final en = companyNameEn?.trim() ?? '';
    if (en.isNotEmpty) return en;
    return 'شركة الرحالة للحوالات المالية';
  }

  bool get hasLogo => (logoUrl ?? '').isNotEmpty;

  static Branding fromJson(Map<String, dynamic> j) {
    final b = (j['branding'] as Map?)?.cast<String, dynamic>() ?? const {};
    final rawLogo = (b['logo_url'] ?? '').toString();

    return Branding(
      companyNameAr: b['company_name_ar']?.toString(),
      companyNameEn: b['company_name_en']?.toString(),
      logoUrl: rawLogo.isEmpty ? null : _absolute(rawLogo),
      themeKey: (b['theme_key'] ?? 'classic_green').toString(),
      version: int.tryParse('${b['branding_version'] ?? 0}') ?? 0,
      colors: BrandColors.fromJson(
        (b['colors'] as Map?)?.cast<String, dynamic>() ?? const {},
      ),
      canEdit: j['can_edit'] == true,
      themes: ((j['themes'] as List?) ?? const [])
          .whereType<Map>()
          .map((e) => BrandTheme.fromJson(e.cast<String, dynamic>()))
          .toList(),
    );
  }

  /// مسارٌ نسبيّ من الخادم + `API_BASE` = رابطٌ يعمل في المحاكي والإنتاج معاً.
  static String _absolute(String path) {
    if (path.startsWith('http://') || path.startsWith('https://')) return path;
    final base = kApiBase.endsWith('/')
        ? kApiBase.substring(0, kApiBase.length - 1)
        : kApiBase;
    return '$base/${path.startsWith('/') ? path.substring(1) : path}';
  }

  /// هوية الرحالة الرسمية — تُستعمل قبل الدخول وحين يتعذّر الجلب.
  static Branding get fallback => const Branding(
        companyNameAr: 'شركة الرحالة للحوالات المالية',
        // الاسم الإنجليزي الرسمي، لا مشتقّاً من العربي: «الرحالة» عَلَمٌ له
        // كتابةٌ معتمدة، والاشتقاق يعطي جملةً أطول تكسر تخطيط الفاتورة.
        companyNameEn: 'Al Rhalla Exchange Company',
        logoUrl: null,
        themeKey: 'classic_green',
        version: 0,
        colors: BrandColors(
          primary: Color(0xFF00875E),
          secondary: Color(0xFF00B17A),
          background: Color(0xFFF3FAF7),
          surface: Color(0xFFFFFFFF),
          onPrimary: Color(0xFFFFFFFF),
          text: Color(0xFF0A261E),
          textMuted: Color(0xFF5B6F67),
          border: Color(0xFFDDE9E3),
          success: Color(0xFF12A150),
          warning: Color(0xFFC77700),
          error: Color(0xFFD14343),
        ),
        canEdit: false,
        themes: [],
      );
}

class BrandingRepository {
  BrandingRepository(this._api);

  final ApiClient _api;

  Future<Branding> load() async {
    final env = await _api.get('/company/branding');
    return Branding.fromJson(env.row ?? const {});
  }

  /// حفظ الاسم والثيم. لا تُرسَل إلا الحقول التي تغيّرت فعلاً — إرسال
  /// الكلّ في كل حفظ يملأ سجلّ التدقيق بصفوفٍ لا معنى لها.
  Future<Branding> save({String? nameAr, String? nameEn, String? themeKey}) async {
    final body = <String, dynamic>{};
    if (nameAr != null) body['company_name_ar'] = nameAr;
    if (nameEn != null) body['company_name_en'] = nameEn;
    if (themeKey != null) body['theme_key'] = themeKey;

    final env = await _api.raw.put('/company/branding', data: body);
    return _fromRaw(env.data);
  }

  Future<Branding> uploadLogo(String filePath) async {
    final form = FormData.fromMap({
      'logo': await MultipartFile.fromFile(filePath),
    });
    final env = await _api.post('/company/branding/logo', body: form);
    return Branding.fromJson(env.row ?? const {});
  }

  Future<Branding> reset() async {
    final env = await _api.post('/company/branding/reset');
    return Branding.fromJson(env.row ?? const {});
  }

  /// الـ PUT يمرّ عبر `dio` مباشرةً لأن [ApiClient] لا يكشف PUT؛ والغلاف
  /// يُقرأ هنا يدوياً بنفس شكل الخادم.
  Branding _fromRaw(dynamic data) {
    if (data is Map && data['data'] is Map) {
      return Branding.fromJson((data['data'] as Map).cast<String, dynamic>());
    }
    return Branding.fallback;
  }
}

final brandingRepositoryProvider = Provider<BrandingRepository>(
  (ref) => BrandingRepository(ref.watch(apiClientProvider)),
);
