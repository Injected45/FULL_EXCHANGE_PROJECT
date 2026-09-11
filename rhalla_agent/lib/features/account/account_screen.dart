import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/app_version.dart';
import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import '../auth/delete_account_otp_screen.dart';
import '../branding/brand_mark.dart';
import '../branding/branding_controller.dart';

class AccountScreen extends ConsumerWidget {
  const AccountScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final user = ref.watch(authControllerProvider).user;

    return Screen(
      child: ListView(
        padding: EdgeInsets.zero,
        children: [
          _ProfileHeader(
            name: user?.displayName ?? '',
            initial: user?.initial,
            role: user?.isMainAgent == true ? 'وكيل رئيسي' : 'نقطة بيع',
            accId: user?.accId,
          ),
          Padding(
            padding: const EdgeInsets.fromLTRB(R.padScreen, 20, R.padScreen, 120),
            child: Column(
              children: [
                GlassCard(
                  padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 6),
                  child: Column(
                    children: [
                      _Row(
                        icon: Icons.badge_outlined,
                        label: 'بياناتي',
                        value: user == null ? null : Fmt.phone(user.phone),
                      ),
                      if (user?.isMainAgent == true)
                        _Row(
                          icon: Icons.storefront_outlined,
                          label: 'نقاط البيع',
                          onTap: () => context.push('/pos'),
                        ),
                      _Row(
                        icon: Icons.star_outline_rounded,
                        label: 'المفضّلة',
                        onTap: () => context.push('/favorites'),
                      ),
                      _Row(
                        icon: Icons.speed_outlined,
                        label: 'سقوف الرحالة',
                        onTap: () => context.push('/limits'),
                      ),
                      _Row(
                        icon: Icons.description_outlined,
                        label: 'كشف الحساب',
                        onTap: () => context.push('/statement'),
                        last: true,
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: R.gapCard),
                GlassCard(
                  padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 6),
                  child: Column(
                    children: [
                      // «الإشعارات · مفعّلة» حُذف: لا شيء مُوصَّل — Pusher
                      // غير مربوط بعد — فكان الصف يدّعي ما لا يحدث.
                      //
                      // «هوية الشركة» يراها الجميع ولا يعدّلها إلا الحساب
                      // الرئيسي — والخادم هو من يمنع، لا إخفاء الصفّ.
                      // «الموظفون» للوكيل الرئيسي وحده — والخادم يرفض لغيره.
                      if (user?.isMainAgent == true)
                        _Row(
                          icon: Icons.groups_2_outlined,
                          label: 'الموظفون',
                          onTap: () => context.push('/employees'),
                        ),
                      _Row(
                        icon: Icons.palette_outlined,
                        label: 'هوية الشركة',
                        onTap: () => context.push('/branding'),
                      ),
                      _Row(
                        icon: Icons.shield_outlined,
                        label: 'الخصوصية والأمان',
                        onTap: () => context.push('/security'),
                        last: true,
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: R.gapCard),
                GlassCard(
                  padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 6),
                  child: Column(
                    children: [
                      _Row(
                        icon: Icons.article_outlined,
                        label: 'الشروط والأحكام',
                        onTap: () => context.push('/terms'),
                      ),
                      _Row(
                        icon: Icons.logout_rounded,
                        label: 'تسجيل الخروج',
                        onTap: () => _confirmSignOut(context, ref),
                        last: true,
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: R.gapCard),
                GlassCard(
                  padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 6),
                  child: _Row(
                    icon: Icons.delete_outline_rounded,
                    label: 'حذف الحساب',
                    danger: true,
                    last: true,
                    onTap: () => _confirmDelete(context, ref),
                  ),
                ),
                const SizedBox(height: 20),
                /*
                 * تذييلٌ من ثلاثة أسطر، وترتيبُها مقصود:
                 *
                 * ١) اسمُ شركة الوكيل — قرارُ المالك (٣ سبتمبر ٢٠٢٦): «كل شيء
                 *    باسمها ظاهرياً»، وهو نفسُ ما تحمله ترويسةُ الفاتورة.
                 *    ⚠ كان قد سقط حين أُضيف سطرُ الحقوق (١٠ سبتمبر) — أُعيد
                 *    مكانه، فالسطران يجتمعان ولا يُبدَّل أحدهما بالآخر.
                 *    و«رحلة» تبقى معه: هي اسم التطبيق لا اسم الشركة.
                 *
                 * ٢) حقوقُ الجهة المطوّرة.  ٣) رقمُ الإصدار (kAppVersion —
                 *    مصدرٌ واحد، يتدرّج ولا يتكرّر، ويطابق pubspec).
                 */
                Text(
                  'رحلة · ${ref.watch(brandingControllerProvider).branding.displayName}',
                  textAlign: TextAlign.center,
                  style: T.meta,
                ),
                const SizedBox(height: 8),
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Container(
                      width: 16,
                      height: 16,
                      alignment: Alignment.center,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        border: Border.all(color: R.inkA(.35)),
                      ),
                      child: Text('C',
                          style: T.plex(8.5, FontWeight.w700, color: R.inkA(.55))),
                    ),
                    const SizedBox(width: 7),
                    Flexible(
                      child: Text(kDeveloperName,
                          textAlign: TextAlign.center, style: T.meta),
                    ),
                  ],
                ),
                const SizedBox(height: 6),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text('v$kAppVersion', style: T.meta),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _confirmSignOut(BuildContext context, WidgetRef ref) async {
    final ok = await _ask(
      context,
      title: 'تسجيل الخروج',
      body: 'ستحتاج إلى رمز تحقّق جديد للدخول مرة أخرى.',
      confirm: 'تسجيل الخروج',
    );
    if (ok == true) await ref.read(authControllerProvider.notifier).signOut();
  }

  Future<void> _confirmDelete(BuildContext context, WidgetRef ref) async {
    final ok = await _ask(
      context,
      title: 'حذف الحساب',
      // الخادم يحذف حذفاً ناعماً: Reg='NO' و deleted_at — لا يُزال الصف.
      // وبعد التأكيد يُطلب رمزُ تحقّقٍ عبر واتساب (أمان أكثر).
      body: 'سيتوقّف حسابك عن العمل ولن تستطيع الدخول. '
          'لا يمكن التراجع عن هذا من التطبيق — يحتاج مراجعة الفرع.\n\n'
          'سنرسل رمزَ تحقّقٍ إلى هاتفك لتأكيد الحذف.',
      confirm: 'متابعة',
      danger: true,
    );
    if (ok != true || !context.mounted) return;
    // الحذفُ الفعليّ يتمّ في شاشة الرمز بعد تحقّق الخادم منه.
    Navigator.of(context, rootNavigator: true).push(
      MaterialPageRoute(builder: (_) => const DeleteAccountOtpScreen()),
    );
  }

  Future<bool?> _ask(
    BuildContext context, {
    required String title,
    required String body,
    required String confirm,
    bool danger = false,
  }) =>
      showModalBottomSheet<bool>(
        context: context,
        backgroundColor: Colors.transparent,
        builder: (_) => Container(
          padding: const EdgeInsets.fromLTRB(22, 22, 22, 26),
          decoration: BoxDecoration(
            color: R.whiteA(.94),
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
              Text(title,
                  style: T.kufi(17, FontWeight.w600,
                      color: danger ? R.error : R.ink)),
              const SizedBox(height: 10),
              Text(body,
                  style: T.plex(12.5, FontWeight.w400,
                      color: R.inkA(.6), height: 1.7)),
              const SizedBox(height: 20),
              PrimaryButton(
                label: confirm,
                onPressed: () => Navigator.of(context).pop(true),
              ),
              const SizedBox(height: 10),
              TextButton(
                onPressed: () => Navigator.of(context).pop(false),
                style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
                child: Text('إلغاء',
                    style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
              ),
            ],
          ),
        ),
      );
}

class _ProfileHeader extends StatelessWidget {
  const _ProfileHeader({
    required this.name,
    required this.initial,
    required this.role,
    required this.accId,
  });

  final String name;
  final String? initial;
  final String role;
  final int? accId;

  @override
  Widget build(BuildContext context) {
    final top = MediaQuery.paddingOf(context).top;

    return Container(
      padding: EdgeInsets.fromLTRB(R.padScreen, top + 16, R.padScreen, 44),
      decoration: BoxDecoration(
        gradient: R.headerGradient,
        borderRadius:
            BorderRadius.vertical(bottom: Radius.circular(R.rHeaderBottom)),
      ),
      clipBehavior: Clip.antiAlias,
      child: Stack(
        clipBehavior: Clip.none,
        children: [
          // نفس علامة الشاشة الرئيسية — شعار الشركة إن وُجد.
          PositionedDirectional(
            top: -40,
            end: -30,
            child: const BrandWatermark(size: 220),
          ),
          /*
           * ══════════════════════════════════════════════════════════════
           *  ترويسةُ الحساب — أمرُ المالك (11 سبتمبر 2026)
           * ══════════════════════════════════════════════════════════════
           *
           * «الدائرةُ تعرض شعار الوكيل · الاسمُ كما هو · وحقلُ ACC يكون أمام
           *  اسم الوكيل على نفس السطر، في حقلٍ يشمل الاسمَ مع ACC بالكامل بلا
           *  فاصلٍ بينهما — منسّقاً لا مكدّساً كلٌّ في سطر».
           *
           * ── ما كان، ولماذا بدا مكدّساً ────────────────────────────────
           *
           * كانت ثلاثةَ كتلٍ فوق بعضها: الاسمُ، ثمّ الصفة، ثمّ حبّةٌ منفصلة
           * لرقم الحساب. فأخذت الترويسةُ ثلاثةَ أسطرٍ لثلاث معلوماتٍ يقرؤها
           * الوكيل في نظرةٍ واحدة.
           *
           * فصارت **حقلاً واحداً** يحمل الاسمَ ورقمَ الحساب على سطرٍ واحد،
           * والصفةُ سطرٌ صغيرٌ تحتهما **داخل الحقل نفسِه** — فلم تُحذف كلمة،
           * وسقط سطرٌ كامل من الارتفاع.
           *
           * ⚠ والدائرةُ صارت [BrandAvatar] — **العنصرُ نفسُه** الذي يحمل
           * الشعار في ترويستَي الرئيسية عند الوكيل والموظف، لا دائرةٌ رابعة
           * تُرسم هنا. فشعارٌ يُحفظ يظهر في الثلاث معاً، والحرفُ احتياطُه حين
           * لا شعار.
           */
          RiseIn.small(
            child: Row(
              children: [
                BrandAvatar(initial: initial),
                const SizedBox(width: 12),
                Expanded(
                  child: Container(
                    padding: const EdgeInsets.fromLTRB(14, 9, 14, 9),
                    decoration: BoxDecoration(
                      color: R.whiteA(.16),
                      border: Border.all(color: R.whiteA(.28)),
                      borderRadius: BorderRadius.circular(R.rCard),
                    ),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Row(
                          crossAxisAlignment: CrossAxisAlignment.center,
                          children: [
                            // ⚠ الاسمُ يتقلّص ولا يدفع الرقمَ خارج الشاشة:
                            // اسمُ شركةٍ طويل كان سيزيح `ACC` إلى حافّةٍ
                            // مقصوصة على الهواتف الضيّقة.
                            Flexible(
                              child: Text(
                                name,
                                maxLines: 1,
                                overflow: TextOverflow.ellipsis,
                                style: T.kufi(15.5, FontWeight.w700,
                                    color: Colors.white),
                              ),
                            ),
                            const SizedBox(width: 10),
                            /*
                             * ⚠ بلا فاصلٍ بينهما كما نصّ الأمر — لا خطٌّ ولا
                             * نقطة. والرقمُ يُميَّز بوزنه ولونه لا بحاجز.
                             *
                             * وباتجاهٍ لاتينيّ مفروض: «ACC 530» مقطعٌ لاتينيّ
                             * في فقرةٍ عربية، وبغيره تقلبه الفقرةُ فيُقرأ
                             * «530 ACC».
                             */
                            Directionality(
                              textDirection: TextDirection.ltr,
                              child: Text(
                                accId == null ? '—' : 'ACC $accId',
                                style: T.kufi(12.5, FontWeight.w700,
                                    color: R.whiteA(.9), spacing: .6),
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 3),
                        // الصفةُ باقيةٌ — داخل الحقل لا فوقه، فلا تأخذ سطراً
                        // مستقلاً ولا تسقط من الشاشة.
                        Text(role,
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis,
                            style: T.plex(11, FontWeight.w400,
                                color: R.whiteA(.72))),
                      ],
                    ),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _Row extends StatelessWidget {
  const _Row({
    required this.icon,
    required this.label,
    this.value,
    this.onTap,
    this.danger = false,
    this.last = false,
  });

  final IconData icon;
  final String label;
  final String? value;
  final VoidCallback? onTap;
  final bool danger;
  final bool last;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onTap,
        child: Container(
          constraints: const BoxConstraints(minHeight: 44),
          padding: const EdgeInsets.symmetric(vertical: 14),
          decoration: last
              ? null
              : BoxDecoration(
                  border: Border(bottom: BorderSide(color: R.inkA(.07))),
                ),
          child: Row(
            children: [
              IconTile(
                icon: Icon(icon,
                    size: 18, color: danger ? R.error : R.primaryGradEnd),
                background: danger ? R.error.withValues(alpha: .08) : null,
              ),
              const SizedBox(width: 13),
              Expanded(
                child: Text(label,
                    style: T.plex(13.5, FontWeight.w500,
                        color: danger ? R.error : R.ink)),
              ),
              if (value != null) ...[
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(value!, style: T.meta),
                ),
                const SizedBox(width: 8),
              ],
              if (!danger && onTap != null)
                Icon(Icons.arrow_forward_ios, size: 14, color: R.inkA(.4)),
            ],
          ),
        ),
      );
}
