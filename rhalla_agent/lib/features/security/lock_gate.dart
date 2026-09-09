import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../auth/auth_controller.dart';
import '../branding/brand_mark.dart';
import '../employee_app/employee_session.dart';
import 'app_lock.dart';

/* ══════════════════════════════════════════════════════════════════════════
   بوّابةُ القفل — تُركَّب فوق الـ Navigator
   ══════════════════════════════════════════════════════════════════════════

   ⚠ **فوق الـ Navigator لا داخل شاشة.** القفلُ يجب أن يستر ما كان معروضاً
   أياً كان: تفاصيلَ حوالة، كشفَ حساب، رصيداً. وبوّابةٌ داخل شاشةٍ واحدة
   تترك سائرَ الشاشات مكشوفة.

   وتفعل شيئين:

   1. **تستر المحتوى** حين يكون التطبيق مقفلاً أو لم يُقرأ حالُه بعد.
   2. **تستره كذلك في مُبدِّل التطبيقات** — ما دامت الشاشةُ غير فعّالة.

   ── ⚠ وما لا تفعله ───────────────────────────────────────────────────────

   **لا تُخرج المستخدم.** الجلسةُ تبقى، والراوتر لا يتغيّر، والشاشةُ التي
   كانت مفتوحة تبقى تحتها. فإن نجحت البصمة عاد إلى موضعه بلا أن يبدأ من أول
   الطريق.

   ولا تحمي من ملتقطِ شاشةٍ متعمَّد على أندرويد: ذلك يحتاج `FLAG_SECURE`،
   وهو يمنع لقطاتِ الشاشة كلَّها — ومنها ما يحتاجه الوكيل للدعم. فتُرك
   قراراً للمالك، ولم يُفرض بلا إذنه.
   ══════════════════════════════════════════════════════════════════════════ */

class LockGate extends ConsumerStatefulWidget {
  const LockGate({super.key, required this.child});

  final Widget child;

  @override
  ConsumerState<LockGate> createState() => _LockGateState();
}

class _LockGateState extends ConsumerState<LockGate>
    with WidgetsBindingObserver {
  /// هل الشاشةُ الآن غيرُ فعّالة (لقطةُ مُبدِّل التطبيقات تُلتقط في هذه اللحظة)؟
  bool _obscured = false;

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addObserver(this);
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    /*
     * ⚠ `inactive` **هي** اللحظة المهمّة هنا — عكسَ متحكّم القفل.
     *
     * النظامُ يلتقط معاينةَ مُبدِّل التطبيقات عند مغادرة الواجهة، وذلك في
     * `inactive` قبل `paused`. فالسترُ يقع هنا وإلّا التُقطت المعاينةُ
     * وفيها الأرصدة.
     *
     * وهو سترُ عرضٍ لا قفل: العودةُ قبل خمس دقائق ترفعه بلا بصمة.
     */
    final hide = state != AppLifecycleState.resumed;
    if (hide != _obscured && mounted) setState(() => _obscured = hide);
  }

  @override
  void dispose() {
    WidgetsBinding.instance.removeObserver(this);
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final lock = ref.watch(appLockProvider);

    /*
     * ⚠ الشجرةُ تبقى مبنيّةً تحت الستر — لا تُهدَم.
     *
     * هدمُها يعني فقدَ ما كتبه الوكيل في نموذج حوالةٍ لم يُرسلها بعد،
     * وإعادةَ جلبِ كلِّ شاشةٍ عند الفتح. والسترُ طبقةٌ فوقها لا بديلٌ عنها.
     */
    return Stack(
      children: [
        widget.child,
        if (lock.shouldHide || _obscured)
          Positioned.fill(
            child: lock.isLocked
                ? const _LockScreen()
                : const _PrivacyVeil(),
          ),
      ],
    );
  }
}

/// سترُ مُبدِّل التطبيقات — شعارُ الشركة على أرضية التطبيق، بلا بيانات.
class _PrivacyVeil extends StatelessWidget {
  const _PrivacyVeil();

  @override
  Widget build(BuildContext context) => ColoredBox(
        color: R.bgTop,
        child: Center(child: BrandMark(size: 76, color: R.primaryA(.5))),
      );
}

/* ═══════════════════════ شاشة القفل ═══════════════════════ */

class _LockScreen extends ConsumerStatefulWidget {
  const _LockScreen();

  @override
  ConsumerState<_LockScreen> createState() => _LockScreenState();
}

class _LockScreenState extends ConsumerState<_LockScreen> {
  bool _busy = false;

  @override
  void initState() {
    super.initState();

    /*
     * ⚠ تُطلب البصمةُ تلقائياً أوّلَ ما تُعرض الشاشة — لا بانتظار ضغطة.
     *
     * الوكيل عاد إلى التطبيق ليعمل، وزرٌّ إضافيٌّ بينه وبين عمله لا يشتري
     * أمناً: النظامُ هو من يعرض النافذة ويقرّر، والضغطةُ لا تضيف إليه شيئاً.
     * ويبقى الزرّ لمن ألغى النافذة أو أخفقت.
     */
    WidgetsBinding.instance.addPostFrameCallback((_) {
      final lock = ref.read(appLockProvider);
      if (lock.biometricsAvailable) _unlock();
    });
  }

  Future<void> _unlock() async {
    if (_busy) return;
    setState(() => _busy = true);
    await ref.read(appLockProvider.notifier).unlockWithBiometrics();
    if (mounted) setState(() => _busy = false);
  }

  /*
   * ⚠ **البديلُ خروجٌ ثمّ دخولٌ كامل — لا تجاوزٌ للقفل.**
   *
   * جهازٌ بلا بصمة، أو بصمةٌ أخفقت مراراً، يجب ألّا يترك المستخدم عالقاً.
   * والمخرجُ الوحيد الآمن أن يُعيد إثبات هويته بالمسار الكامل: رمزُ تحقّقٍ
   * إلى رقمه هو. وأيُّ بديلٍ أخفّ من ذلك يجعل القفلَ زينة.
   */
  Future<void> _signOutAndVerify() async {
    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => const _ConfirmFallback(),
    );
    if (ok != true || !mounted) return;

    /*
     * ⚠ يُخرَج من الوضع الفعّال وحدَه.
     *
     * الجهازُ يحمل جلسةَ وكيلٍ أو جلسةَ موظف لا كليهما — والخروجُ من
     * الاثنين معاً كان سيمحو جلسةً لا علاقةَ لها بالقفل.
     */
    final employee = ref.read(employeeAuthProvider).profile != null;
    if (employee) {
      await ref.read(employeeAuthProvider.notifier).signOut();
    } else {
      await ref.read(authControllerProvider.notifier).signOut();
    }

    // ⚠ ويُرفع القفل بعد الخروج: شاشةُ الدخول تحته، وسترُها يترك المستخدم
    // أمام بصمةٍ لا تفتح شيئاً.
    if (mounted) ref.read(appLockProvider.notifier).markOpen();
  }

  @override
  Widget build(BuildContext context) {
    final lock = ref.watch(appLockProvider);

    return ColoredBox(
      color: R.bgTop,
      child: SafeArea(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(28, 0, 28, 28),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Spacer(),
              BrandMark(size: 72, color: R.primary),
              const SizedBox(height: 26),
              Icon(Icons.lock_outline_rounded, size: 30, color: R.primaryDark),
              const SizedBox(height: 14),
              Text('التطبيق مقفل',
                  textAlign: TextAlign.center,
                  style: T.kufi(19, FontWeight.w700)),
              const SizedBox(height: 8),
              Text(
                lock.biometricsAvailable
                    ? 'أثبت هويتك لمتابعة عملك — جلستك ما زالت مفتوحة.'
                    : 'لا توجد بصمة مسجَّلة في هذا الجهاز.',
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w400,
                    color: R.inkA(.6), height: 1.7),
              ),

              if (lock.lastError != null) ...[
                const SizedBox(height: 14),
                WarnBanner(text: lock.lastError!),
              ],

              const Spacer(),

              if (lock.biometricsAvailable)
                PrimaryButton(
                  label: 'فتح بالبصمة',
                  loading: _busy,
                  icon: const Icon(Icons.fingerprint_rounded,
                      size: 20, color: Colors.white),
                  onPressed: _busy ? null : _unlock,
                ),

              if (lock.biometricsAvailable) const SizedBox(height: 10),

              SecondaryButton(
                label: 'الدخول بالتحقّق من جديد',
                onPressed: _signOutAndVerify,
              ),
              const SizedBox(height: 12),
              Text(
                'القفل يستر الشاشة فقط — لم تُغلق جلستك ولم تتغيّر بياناتك.',
                textAlign: TextAlign.center,
                style: T.plex(11.5, FontWeight.w400, color: R.inkA(.45)),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class _ConfirmFallback extends StatelessWidget {
  const _ConfirmFallback();

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(22, 22, 22, 26),
        decoration: BoxDecoration(
          color: R.whiteA(.96),
          borderRadius:
              const BorderRadius.vertical(top: Radius.circular(R.rNav)),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Center(
              child: Container(
                width: 44,
                height: 4,
                decoration: BoxDecoration(
                  color: R.inkA(.16),
                  borderRadius: BorderRadius.circular(99),
                ),
              ),
            ),
            const SizedBox(height: 20),
            Text('الدخول بالتحقّق من جديد',
                textAlign: TextAlign.center,
                style: T.kufi(17, FontWeight.w700)),
            const SizedBox(height: 10),
            Text(
              'سيُغلق التطبيق جلستك، ثم تدخل برقمك ورمز تحقّقٍ جديد يصلك على '
              'واتساب. استعمل هذا إن تعذّرت البصمة.',
              textAlign: TextAlign.center,
              style: T.plex(13, FontWeight.w400, color: R.inkA(.6), height: 1.7),
            ),
            const SizedBox(height: 20),
            PrimaryButton(
              label: 'متابعة',
              onPressed: () => Navigator.of(context).pop(true),
            ),
            const SizedBox(height: 10),
            SecondaryButton(
              label: 'رجوع',
              onPressed: () => Navigator.of(context).pop(false),
            ),
          ],
        ),
      );
}
