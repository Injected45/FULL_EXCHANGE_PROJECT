import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../branding/brand_mark.dart';
import 'incoming_alerts.dart';

/* ══════════════════════════════════════════════════════════════════════════
   الشريط المنسدل: «لديك حوالة جديدة»
   ══════════════════════════════════════════════════════════════════════════

   ينسدل من أعلى الشاشة لحظةَ وصول واردةٍ جديدة — مع الرنّة نفسِها — ويبقى
   خمس ثوانٍ ثم يرتفع. والحوالةُ بعده لا تضيع: عدّادُ الجرس يحملها إلى أن
   يفتح الوكيل قائمة الوارد.

   ── أربعةُ قراراتٍ فيه ليست تفصيلاً ───────────────────────────────────────

   1. **يُركَّب فوق الـ Navigator في `main.dart`، لا داخل الهيكل.**
      الوكيل قد يكون في تفاصيل حوالة أو في كشف الحساب حين تصل واردة، وشريطٌ
      داخل الهيكل تغطّيه أوّلُ شاشةٍ تُدفَع فوقه. وهو فوق `AmbientBackground`
      للسبب ذاته الذي وضعها هناك: يُبنى مرّةً واحدة فلا تُستأنف حركتُه.

   2. **لا ينسدل قبل الدخول ولا في وضع الموظف.** لا شرطَ هنا يقول ذلك —
      المصدرُ نفسُه لا يَعُدّ: نبضةُ الوارد تعمل من هيكل الوكيل بعد الدخول
      وحدَه. وشرطٌ مكتوبٌ هنا يُنسى حين يُضاف مسارٌ جديد؛ المصدرُ لا يُنسى.

   3. **يُقاس عمرُه من آخر وصول لا من أوّله.** واردتان تفصلهما ثانيتان
      تُبقيان الشريط خمساً من الثانية — لا ثلاثاً. والنصُّ يتبدّل تحت
      الوكيل بلا أن يقفز الشريط.

   4. **لمسُه يفتح قائمة الوارد ويطويه.** إشعارٌ لا يوصل إلى ما أعلن عنه
      يجعل الوكيل يبحث عن الشاشة بنفسه، وقد نسي الرقم.
   ══════════════════════════════════════════════════════════════════════════ */

/// كم يبقى الشريط منسدلاً — أمر المالك (8 سبتمبر 2026).
const _kVisible = Duration(seconds: 5);

/// زمنُ الانسدال والارتفاع.
const _kSlide = Duration(milliseconds: 280);

class IncomingToast extends ConsumerStatefulWidget {
  const IncomingToast({super.key});

  @override
  ConsumerState<IncomingToast> createState() => _IncomingToastState();
}

class _IncomingToastState extends ConsumerState<IncomingToast> {
  /// منسدلٌ الآن — تتبعه الحركة.
  bool _open = false;

  /// مركَّبٌ في الشجرة أصلاً.
  ///
  /// ⚠ **منفصلٌ عن [_open] عمداً.** الشريط بلا هذا يبقى مبنيّاً طولَ عمر
  /// التطبيق ولو لم تصل حوالةٌ قطّ — فيُجلب شعارُ الشركة من الشبكة عند كل
  /// إقلاع لصورةٍ لا تُعرض. ورفعُه لحظةَ الطيّ يقطع حركة الارتفاع في
  /// منتصفها، فيختفي قفزاً لا انزلاقاً. فيبقى مركَّباً حتى تنتهي الحركة
  /// ثم يُرفع.
  bool _mounted = false;

  /// عددُ ما وصل في النبضة التي أظهرت الشريط.
  int _count = 0;

  Timer? _hide;

  void _show(int arrived) {
    _hide?.cancel();
    _count = arrived;

    if (_mounted) {
      // منسدلٌ أو في طريقه إلى الارتفاع: يُعاد إنزاله ويُجدَّد نصُّه.
      setState(() => _open = true);
    } else {
      /*
       * ⚠ **إطارٌ مرتفعٌ أوّلاً ثم الإنزال.**
       *
       * `AnimatedSlide` تُحرّك من قيمتها السابقة، ولا قيمةَ سابقة في أول
       * بناء — فتركيبُه منسدلاً يجعله يظهر قفزاً بلا انسدال. فيُركَّب
       * مرتفعاً، ويُنزَل في الإطار التالي.
       */
      setState(() {
        _mounted = true;
        _open = false;
      });
      WidgetsBinding.instance.addPostFrameCallback((_) {
        if (mounted) setState(() => _open = true);
      });
    }

    _hide = Timer(_kVisible, () {
      if (mounted) setState(() => _open = false);
    });
  }

  void _dismiss() {
    _hide?.cancel();
    if (mounted) setState(() => _open = false);
  }

  @override
  void dispose() {
    _hide?.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    // ⚠ `listen` لا `watch`: الشريط يتفاعل مع **حدث** الوصول، ومراقبةُ
    // الحالة تعيد بناءه مع كل تغيّرٍ في العدّاد — بما فيه هبوطُه إلى صفر
    // حين يفتح الوكيل القائمة.
    ref.listen<IncomingAlerts>(incomingAlertsProvider, (prev, next) {
      if (next.ping > (prev?.ping ?? 0)) _show(next.arrived);
    });

    if (!_mounted) return const SizedBox.shrink();

    final top = MediaQuery.paddingOf(context).top;

    return Positioned(
      top: 0,
      left: 0,
      right: 0,
      child: IgnorePointer(
        // مرتفعاً لا يعترض لمسةً على ما تحته — وهو يشغل عرض الشاشة كلَّه.
        ignoring: !_open,
        child: AnimatedSlide(
          duration: _kSlide,
          curve: _open ? Curves.easeOutCubic : Curves.easeInCubic,
          offset: _open ? Offset.zero : const Offset(0, -1),
          onEnd: () {
            if (!_open && mounted) setState(() => _mounted = false);
          },
          child: AnimatedOpacity(
            duration: _kSlide,
            opacity: _open ? 1 : 0,
            child: Padding(
              padding: EdgeInsets.fromLTRB(12, top + 8, 12, 0),
              child: _Card(count: _count, onDismiss: _dismiss),
            ),
          ),
        ),
      ),
    );
  }
}

class _Card extends StatelessWidget {
  const _Card({required this.count, required this.onDismiss});

  final int count;
  final VoidCallback onDismiss;

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      /*
       * السحبُ إلى أعلى يطويه قبل الخمس — لمن رآه وانتهى.
       *
       * ⚠ **وليس `Dismissible`**: تلك تشترط أن تُرفع الودجت من الشجرة بعد
       * السحب وتُطلق تأكيداً إن بقيت، وهذه تبقى — فهي ثابتةٌ فوق الـNavigator
       * تُطوى ولا تُهدَم. والانهيارُ حينها في الإصدار لا في التحليل.
       */
      onVerticalDragEnd: (d) {
        if ((d.primaryVelocity ?? 0) < -100) onDismiss();
      },
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          borderRadius: BorderRadius.circular(R.rCard),
          onTap: () {
            onDismiss();
            context.push('/transfers');
          },
          child: Ink(
            decoration: BoxDecoration(
              gradient: R.headerGradient,
              borderRadius: BorderRadius.circular(R.rCard),
              boxShadow: [
                BoxShadow(
                  color: R.inkA(.28),
                  blurRadius: 22,
                  offset: const Offset(0, 8),
                ),
              ],
            ),
            child: Padding(
              padding: const EdgeInsets.fromLTRB(12, 11, 12, 11),
              child: Row(
                children: [
                  /*
                   * شعار الشركة في دائرة بيضاء.
                   *
                   * ⚠ **الدائرةُ بيضاء لا شفّافة**: شعارُ الشركة يصل صورةً
                   * كما رفعها صاحبُها — أكثرُها بخلفيةٍ بيضاء وحبرٍ داكن،
                   * وهذا الشريط تدرّجٌ داكن. فبلا أرضيةٍ بيضاء تحته يختفي
                   * الشعارُ في لونه. وهي القاعدةُ نفسُها في الفاتورة.
                   *
                   * و[BrandMark] يعني هوية الشركة المسجَّلة في تطبيق الوكيل
                   * — وشعارُ الرحالة احتياطُها حين لا تكون الشركة رفعت شعاراً.
                   */
                  Container(
                    width: 40,
                    height: 40,
                    alignment: Alignment.center,
                    decoration: BoxDecoration(
                      color: Colors.white,
                      shape: BoxShape.circle,
                      border: Border.all(color: R.whiteA(.5), width: 1.5),
                    ),
                    child: const ClipOval(
                      child: Padding(
                        padding: EdgeInsets.all(5),
                        child: BrandMark(size: 26),
                      ),
                    ),
                  ),
                  const SizedBox(width: 11),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Text(
                          // ⚠ النصُّ يُجمع مع العدد: «وصلتك حوالتان» ثم
                          // يفتح الوكيل القائمة فيجد اثنتين. وعبارةٌ مفردة
                          // فوق عددٍ آخر تجعله يظنّ أنه فاته شيء.
                          count > 1
                              ? 'لديك $count حوالات جديدة'
                              : 'لديك حوالة جديدة',
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style:
                              T.kufi(14.5, FontWeight.w700, color: Colors.white),
                        ),
                        const SizedBox(height: 2),
                        Text(
                          'اضغط لعرض الحوالات الواردة',
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                          style: T.plex(11, FontWeight.w400,
                              color: R.whiteA(.85)),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(width: 6),
                  Icon(Icons.call_received_rounded,
                      size: 19, color: R.whiteA(.92)),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
