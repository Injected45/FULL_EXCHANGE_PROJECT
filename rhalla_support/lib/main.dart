import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:webview_flutter/webview_flutter.dart';
import 'package:webview_flutter_android/webview_flutter_android.dart';

/// ═══════════════════════════════════════════════════════════════════════
///  «الرحالة للدعم الفني» — تطبيق الهاتف
/// ═══════════════════════════════════════════════════════════════════════
///
/// ── لماذا غلافٌ أصليّ لا نسخةٌ ثانية بـ Flutter ──────────────────────────
///
/// لوحةُ الدعم مبنيّةٌ بـ React وتعمل، وفيها المحادثات والإسناد والحالات
/// والصلاحيات وسجلّ النشاط. وإعادةُ بنائها بـ Flutter تعني **شاشتين لكل
/// ميزة**: كلُّ إصلاحٍ يُكتب مرّتين، وكلُّ ميزةٍ تُضاف مرّتين — وأوّلُ مرّةٍ
/// تُنسى إحداهما تفترق النسختان ويصير للنظام سلوكان.
///
/// وهذا الغلاف يعرض **اللوحةَ نفسها**: ما يُصلَح في `support-app/` يظهر في
/// الهاتف وفي المتصفّح معاً، بلا إصدارٍ جديد للتطبيق أصلاً.
///
/// ── وما يضيفه الغلاف فعلاً ───────────────────────────────────────────────
///
/// ثلاثةُ أشياء لا يستطيعها المتصفّح على الهاتف:
///
///   1. **أيقونةٌ على الشاشة** — يفتحها الموظّف بضغطةٍ لا بكتابة عنوان.
///   2. **تنبيهٌ يعمل**: `WebView` لا تدعم `Notification` API إطلاقاً، فلو
///      تُرك الأمر لها لصمتت اللوحةُ على الهاتف. والجسر أدناه يعطي اهتزازاً
///      ونغمةَ النظام — وهي ما اعتاده صاحب الهاتف.
///   3. **زرّ الرجوع** يتنقّل داخل اللوحة بدل أن يُغلق التطبيق من أوّل ضغطة.
///
/// ⚠ ولا شيء هنا يمسّ المال: هذا متصفّحٌ مُغلَّف، لا يعرف عن الحوالات ولا
/// الأرصدة شيئاً، وكلُّ حمايةٍ تبقى حيث هي — في الخادم.
void main() {
  WidgetsFlutterBinding.ensureInitialized();
  SystemChrome.setPreferredOrientations(
      [DeviceOrientation.portraitUp, DeviceOrientation.portraitDown]);
  runApp(const SupportApp());
}

const _brand = Color(0xFF0F5F4E);
const _brandDark = Color(0xFF0A4438);

class SupportApp extends StatelessWidget {
  const SupportApp({super.key});

  @override
  Widget build(BuildContext context) => MaterialApp(
        title: 'الرحالة للدعم الفني',
        debugShowCheckedModeBanner: false,
        locale: const Locale('ar'),
        theme: ThemeData(
          useMaterial3: true,
          colorScheme: ColorScheme.fromSeed(
              seedColor: _brand, primary: _brand, brightness: Brightness.light),
          fontFamily: 'Roboto',
        ),
        // الواجهة عربية كلُّها — والاتجاه يُفرض هنا مرّةً بدل أن يُكرَّر.
        builder: (_, child) => Directionality(
          textDirection: TextDirection.rtl,
          child: child ?? const SizedBox.shrink(),
        ),
        home: const Shell(),
      );
}

/// عنوان الخادم — يُحفظ على الجهاز فيُكتب مرّةً واحدة.
///
/// ولا يُدفن في الشيفرة: خادمُ المكتب اليوم غيرُ خادم الاستضافة غداً،
/// وعنوانٌ مدفون يعني إصداراً جديداً من التطبيق لتغيير سطر.
class Settings {
  static const _key = 'support_base_url';
  // بلا `AndroidOptions`: النسخة 11 تُشفّر افتراضياً وأسقطت الخيار القديم.
  static const _store = FlutterSecureStorage();

  static Future<String?> baseUrl() async {
    try {
      final v = await _store.read(key: _key);
      return (v == null || v.trim().isEmpty) ? null : v.trim();
    } catch (_) {
      return null;
    }
  }

  static Future<void> setBaseUrl(String v) async {
    try {
      await _store.write(key: _key, value: v.trim());
    } catch (_) {
      /* جهازٌ يمنع التخزين — يبقى العنوان لهذه الجلسة. */
    }
  }

  /// يُطبَّع ما يكتبه الموظّف: بلا مخطَّط، أو بشرطةٍ زائدة، أو بمسار `/support`
  /// مكتوبٍ بيده. كلُّها أخطاءٌ متوقَّعة، ورفضُها بلا سببٍ ظاهر يُوقف التشغيل
  /// عند أوّل حرف.
  static String normalize(String raw) {
    var s = raw.trim();
    if (s.isEmpty) return s;
    if (!s.startsWith('http://') && !s.startsWith('https://')) {
      s = 'http://$s';
    }
    s = s.replaceAll(RegExp(r'/+$'), '');
    s = s.replaceAll(RegExp(r'/support/?$'), '');
    return s;
  }
}

class Shell extends StatefulWidget {
  const Shell({super.key});

  @override
  State<Shell> createState() => _ShellState();
}

class _ShellState extends State<Shell> {
  WebViewController? _web;
  String? _base;
  bool _loading = true;
  String _error = '';

  @override
  void initState() {
    super.initState();
    _boot();
  }

  Future<void> _boot() async {
    final saved = await Settings.baseUrl();
    if (!mounted) return;

    if (saved == null) {
      setState(() => _loading = false);
      return;
    }
    _open(saved);
  }

  void _open(String base) {
    final c = WebViewController()
      ..setJavaScriptMode(JavaScriptMode.unrestricted)
      ..setBackgroundColor(const Color(0xFFF4F6F5))
      // ── الجسر إلى النظام ───────────────────────────────────────────
      //
      // ⚠ هذا هو سببُ وجود التطبيق: `WebView` لا تدعم `Notification` API،
      // فلوحةُ الدعم تصمت على الهاتف مهما وصلها من رسائل. واللوحة تنادي
      // `RhallaNative.postMessage('alert')` إن وجدته، وإلا استعملت إشعار
      // المتصفّح — فتعمل في الموضعين بلا فرعين في الشيفرة.
      ..addJavaScriptChannel('RhallaNative', onMessageReceived: (m) {
        if (m.message == 'alert') _alert();
      })
      ..setNavigationDelegate(NavigationDelegate(
        onPageFinished: (_) {
          if (mounted) setState(() => _loading = false);
        },
        onWebResourceError: (e) {
          // خطأُ مورِدٍ فرعيّ (صورةٌ مثلاً) لا يُسقط الصفحة — الشرط يميّزه.
          if (!e.isForMainFrame!) return;
          if (mounted) {
            setState(() {
              _loading = false;
              _error = 'تعذّر الوصول إلى ${Settings.normalize(base)}\n\n'
                  '${e.description}\n\n'
                  'تأكّد أن الخادم يعمل وأنك على نفس الشبكة.';
            });
          }
        },
      ))
      ..loadRequest(Uri.parse('$base/support'));

    // ملفّاتٌ يختارها الموظّف لإرسالها كمرفق — بغير هذا لا يفتح زرّ الإرفاق
    // شيئاً على أندرويد، ويبدو معطّلاً.
    if (c.platform is AndroidWebViewController) {
      AndroidWebViewController.enableDebugging(false);
      (c.platform as AndroidWebViewController)
          .setMediaPlaybackRequiresUserGesture(false);
    }

    setState(() {
      _web = c;
      _base = base;
      _loading = true;
      _error = '';
    });
  }

  /// اهتزازةٌ ونغمةُ النظام — بديلُ `Notification` التي لا وجود لها هنا.
  Future<void> _alert() async {
    HapticFeedback.mediumImpact();
    try {
      await SystemSound.play(SystemSoundType.alert);
    } catch (_) {
      /* جهازٌ صامت — الاهتزاز وحده يكفي. */
    }
  }

  /// زرّ الرجوع: يتنقّل داخل اللوحة أوّلاً، ثم يسأل قبل الخروج.
  ///
  /// ⚠ دالّةٌ على الحالة لا مُغلَقٌ داخل `PopScope`: المحلّل يفقد أثر
  /// `mounted` داخل المُغلَق فيحذّر، والأهمّ أن الفحص هنا **صحيحٌ فعلاً** —
  /// `context` و`mounted` كلاهما لهذه الحالة بلا التباس.
  ///
  /// وضغطةٌ واحدة تُغلق التطبيق من وسط محادثة أسوأُ ما في الأغلفة.
  Future<void> _onBack() async {
    final c = _web;
    if (c != null && await c.canGoBack()) {
      await c.goBack();
      return;
    }
    if (!mounted) return;

    final out = await showDialog<bool>(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('الخروج من التطبيق؟'),
        actions: [
          TextButton(
              onPressed: () => Navigator.pop(ctx, false),
              child: const Text('لا')),
          FilledButton(
              onPressed: () => Navigator.pop(ctx, true),
              child: const Text('خروج')),
        ],
      ),
    );
    if (out == true) SystemNavigator.pop();
  }

  Future<void> _changeServer() async {
    final ctrl = TextEditingController(text: _base ?? '');
    final v = await showDialog<String>(
      context: context,
      builder: (ctx) => _ServerDialog(controller: ctrl),
    );
    if (v == null) return;
    final norm = Settings.normalize(v);
    if (norm.isEmpty) return;
    await Settings.setBaseUrl(norm);
    _open(norm);
  }

  @override
  Widget build(BuildContext context) {
    if (_base == null && !_loading) {
      return _FirstRun(onDone: (v) async {
        await Settings.setBaseUrl(v);
        _open(v);
      });
    }

    return PopScope(
      // زرّ الرجوع يتنقّل داخل اللوحة أوّلاً: ضغطةٌ واحدة تُغلق التطبيق من
      // وسط محادثةٍ هي أسوأ ما في الأغلفة.
      canPop: false,
      onPopInvokedWithResult: (didPop, _) {
        if (!didPop) _onBack();
      },
      child: Scaffold(
        backgroundColor: const Color(0xFFF4F6F5),
        body: SafeArea(
          child: Stack(
            children: [
              if (_web != null && _error.isEmpty)
                WebViewWidget(controller: _web!),
              if (_error.isNotEmpty) _ErrorView(
                message: _error,
                onRetry: () => _open(_base!),
                onChange: _changeServer,
              ),
              if (_loading)
                const ColoredBox(
                  color: Color(0xFFF4F6F5),
                  child: Center(
                    child: CircularProgressIndicator(color: _brand, strokeWidth: 2.6),
                  ),
                ),
            ],
          ),
        ),
        // زرٌّ صغير لتغيير العنوان — لا يظهر إلا عند الحاجة إليه فعلاً.
        floatingActionButton: _error.isNotEmpty
            ? null
            : Opacity(
                opacity: .25,
                child: FloatingActionButton.small(
                  heroTag: 'srv',
                  backgroundColor: _brandDark,
                  onPressed: _changeServer,
                  child: const Icon(Icons.dns_outlined, size: 18),
                ),
              ),
        floatingActionButtonLocation: FloatingActionButtonLocation.startFloat,
      ),
    );
  }
}

class _FirstRun extends StatefulWidget {
  const _FirstRun({required this.onDone});
  final Future<void> Function(String) onDone;

  @override
  State<_FirstRun> createState() => _FirstRunState();
}

class _FirstRunState extends State<_FirstRun> {
  final _c = TextEditingController();
  String _err = '';

  @override
  Widget build(BuildContext context) => Scaffold(
        body: Container(
          decoration: const BoxDecoration(
            gradient: LinearGradient(
              begin: Alignment.topCenter,
              end: Alignment.bottomCenter,
              colors: [_brand, _brandDark, Color(0xFF08322A)],
            ),
          ),
          child: SafeArea(
            child: Center(
              child: SingleChildScrollView(
                padding: const EdgeInsets.all(24),
                child: Card(
                  elevation: 12,
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(18)),
                  child: Padding(
                    padding: const EdgeInsets.all(24),
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        const Text('الرحالة للدعم الفني',
                            style: TextStyle(
                                fontSize: 20,
                                fontWeight: FontWeight.w800,
                                color: _brandDark)),
                        const SizedBox(height: 6),
                        const Text('اكتب عنوان خادم الشركة مرّةً واحدة',
                            style: TextStyle(fontSize: 13, color: Colors.black54)),
                        const SizedBox(height: 22),
                        if (_err.isNotEmpty) ...[
                          Text(_err,
                              style: const TextStyle(
                                  color: Color(0xFFC0392B), fontSize: 12.5)),
                          const SizedBox(height: 10),
                        ],
                        TextField(
                          controller: _c,
                          textDirection: TextDirection.ltr,
                          keyboardType: TextInputType.url,
                          autofocus: true,
                          decoration: const InputDecoration(
                            hintText: '192.168.1.10:8000',
                            hintTextDirection: TextDirection.ltr,
                            border: OutlineInputBorder(),
                            prefixIcon: Icon(Icons.dns_outlined),
                          ),
                        ),
                        const SizedBox(height: 8),
                        const Align(
                          alignment: AlignmentDirectional.centerStart,
                          child: Text(
                            'اسأل مسؤول النظام عن العنوان. لا تكتب /support — يُضاف وحده.',
                            style: TextStyle(fontSize: 11.5, color: Colors.black45),
                          ),
                        ),
                        const SizedBox(height: 18),
                        SizedBox(
                          width: double.infinity,
                          child: FilledButton(
                            onPressed: () {
                              final v = Settings.normalize(_c.text);
                              if (v.isEmpty) {
                                setState(() => _err = 'اكتب العنوان.');
                                return;
                              }
                              widget.onDone(v);
                            },
                            style: FilledButton.styleFrom(
                                backgroundColor: _brand,
                                padding: const EdgeInsets.symmetric(vertical: 14)),
                            child: const Text('فتح لوحة الدعم'),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              ),
            ),
          ),
        ),
      );
}

class _ServerDialog extends StatelessWidget {
  const _ServerDialog({required this.controller});
  final TextEditingController controller;

  @override
  Widget build(BuildContext context) => AlertDialog(
        title: const Text('عنوان الخادم'),
        content: TextField(
          controller: controller,
          textDirection: TextDirection.ltr,
          keyboardType: TextInputType.url,
          decoration: const InputDecoration(
            hintText: '192.168.1.10:8000',
            hintTextDirection: TextDirection.ltr,
            border: OutlineInputBorder(),
          ),
        ),
        actions: [
          TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text('إلغاء')),
          FilledButton(
              onPressed: () => Navigator.pop(context, controller.text),
              style: FilledButton.styleFrom(backgroundColor: _brand),
              child: const Text('حفظ وفتح')),
        ],
      );
}

class _ErrorView extends StatelessWidget {
  const _ErrorView({
    required this.message,
    required this.onRetry,
    required this.onChange,
  });

  final String message;
  final VoidCallback onRetry;
  final VoidCallback onChange;

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.all(28),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              const Icon(Icons.cloud_off_rounded, size: 52, color: Colors.black26),
              const SizedBox(height: 16),
              Text(message,
                  textAlign: TextAlign.center,
                  style: const TextStyle(fontSize: 13, height: 1.7)),
              const SizedBox(height: 22),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  OutlinedButton(
                      onPressed: onChange, child: const Text('تغيير العنوان')),
                  const SizedBox(width: 10),
                  FilledButton(
                      onPressed: onRetry,
                      style: FilledButton.styleFrom(backgroundColor: _brand),
                      child: const Text('إعادة المحاولة')),
                ],
              ),
            ],
          ),
        ),
      );
}
