import 'package:dio/dio.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../storage/secure_store.dart';
import 'api_envelope.dart';

/// عنوان الخادم. يُمرَّر عند البناء:
///   flutter run --dart-define=API_BASE=http://102.214.165.242:8080/api
const kApiBase = String.fromEnvironment(
  'API_BASE',
  defaultValue: 'http://102.214.165.242:8080/api',
);

class ApiClient {
  ApiClient(this._store) {
    _dio = Dio(BaseOptions(
      baseUrl: kApiBase,
      connectTimeout: const Duration(seconds: 20),
      receiveTimeout: const Duration(seconds: 60),
      headers: {'Accept': 'application/json'},
      // نتولّى قراءة الحالة بأنفسنا — 404 ليست خطأً في هذا الـ API،
      // و 422 قد تحمل success:true.
      validateStatus: (_) => true,
      // الخادم يضع Cache-Control لسنة كاملة على JSON عبر .htaccess،
      // وهو خطر على كشف مالي. نبطله من العميل.
      extra: const {},
    ));

    _dio.interceptors.add(InterceptorsWrapper(
      onRequest: (options, handler) async {
        options.headers['Cache-Control'] = 'no-store';
        final token = await _store.readToken();
        if (token != null && token.isNotEmpty) {
          options.headers['Authorization'] = 'Bearer $token';
        }
        handler.next(options);
      },
    ));
  }

  late final Dio _dio;
  final SecureStore _store;

  /// يُستدعى مرّة واحدة حين يرد الخادم 401، ليخرج التطبيق من الجلسة المنتهية.
  /// يُوصَّل في [apiClientProvider]؛ تركه فارغاً يعني بقاء المستخدم عالقاً
  /// أمام خطأ على كل شاشة بلا مخرج.
  void Function()? onUnauthorized;

  /// طلبات متوازية قد ترد 401 معاً — نُخرج مرّة واحدة لا مرّة لكل طلب.
  bool _signingOut = false;

  Dio get raw => _dio;

  Future<Envelope> post(String path, {Object? body, Map<String, String>? headers}) =>
      _send(() => _dio.post(path, data: body, options: Options(headers: headers)));

  Future<Envelope> get(String path, {Map<String, dynamic>? query}) =>
      _send(() => _dio.get(path, queryParameters: query));

  Future<Envelope> delete(String path) => _send(() => _dio.delete(path));

  Future<Envelope> put(String path, {Object? body}) =>
      _send(() => _dio.put(path, data: body));

  /// ترويسات الطلب كما يبنيها الاعتراض — للصور التي تُجلب خارج dio.
  ///
  /// `Image.network` يفتح اتصاله بنفسه ولا يمرّ بـ dio، فلا يحمل رمز
  /// الجلسة. ومرفقات الدردشة خلف `auth:sanctum` — انظر توثيق نقطتها في
  /// الخادم: هي صور إيصالات ووثائق عملاء لا شعار شركة.
  Future<Map<String, String>> authHeaders() async {
    final token = await _store.readToken();
    return token == null || token.isEmpty
        ? const {}
        : {'Authorization': 'Bearer $token'};
  }

  Future<Envelope> _send(Future<Response> Function() run) async {
    late final Response res;
    try {
      res = await run();
    } on DioException catch (e) {
      throw ApiFailure(
        _networkMessage(e),
        isNetwork: true,
      );
    }

    final status = res.statusCode ?? 0;

    // الخادم يعيد أحياناً صفحة HTML بدل JSON — 404 من Apache حين لا يصل
    // الطلب إلى Laravel أصلاً، أو أثر استثناء PHP لأن APP_DEBUG=true.
    // لا يجوز أن يتسرّب أيٌّ منهما إلى الشاشة.
    if (res.data is String && Envelope.looksLikeMarkup(res.data as String)) {
      throw ApiFailure(_serverPageMessage(status), statusCode: status);
    }

    final env = Envelope.parse(status, res.data);

    // 401 دائماً خطأ مصادقة مهما قال الجسم.
    if (env.statusCode == 401) {
      if (!_signingOut) {
        _signingOut = true;
        onUnauthorized?.call();
      }
      throw ApiFailure(
        env.displayMessage('انتهت الجلسة. سجّل الدخول من جديد.'),
        statusCode: 401,
        envelope: env,
      );
    }
    _signingOut = false;

    if (!env.ok) {
      throw ApiFailure(
        env.firstValidationError() ?? env.displayMessage('تعذّر إتمام العملية.'),
        statusCode: env.statusCode,
        envelope: env,
      );
    }

    return env;
  }

  /// رسالة نظيفة حين يرد الخادم صفحة بدل JSON.
  String _serverPageMessage(int status) {
    if (status == 404) {
      return 'الخدمة غير متاحة على هذا العنوان (404).\n'
          'لم يصل الطلب إلى التطبيق على الخادم — تحقّق من عنوان الـ API.';
    }
    if (status >= 500) return 'خطأ في الخادم ($status). حاول لاحقاً.';
    return 'رد غير متوقّع من الخادم ($status).';
  }

  String _networkMessage(DioException e) {
    switch (e.type) {
      case DioExceptionType.connectionTimeout:
      case DioExceptionType.sendTimeout:
      case DioExceptionType.receiveTimeout:
        return 'انتهت مهلة الاتصال بالخادم.';
      case DioExceptionType.connectionError:
        return 'لا يوجد اتصال بالإنترنت.';
      default:
        return 'تعذّر الوصول إلى الخادم.';
    }
  }
}

final secureStoreProvider = Provider<SecureStore>((ref) => SecureStore());

// [ApiClient.onUnauthorized] يُوصَّل من `authControllerProvider` لا من هنا:
// core لا يعرف features، والوصل هناك يجعله غير قابل للنسيان.
final apiClientProvider = Provider<ApiClient>(
  (ref) => ApiClient(ref.watch(secureStoreProvider)),
);
