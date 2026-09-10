import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../chat/chat_screen.dart';
import 'employee_session.dart';

/// شاشةُ تجميدٍ فوق كلّ شيء حين يُوقف الوكيلُ الموظفَ (فردياً أو جماعياً).
///
/// تُبنى فوق الـ Navigator في `main.dart` فتغطّي أيّ شاشةٍ كان الموظف فيها.
/// **لا يُفقَد شيء**: الجلسة والبيانات باقية؛ هذه واجهةٌ فقط تُرفع فور أن
/// يُعيد الوكيلُ التشغيل (نبضُ me كلّ ١٢ ثانية يكشف التغيير). والخادمُ يرفض
/// كلّ عملٍ للموظف المُوقَف بمعزلٍ عن هذه الشاشة، فلا حيلة في تجاوزها.
///
/// وتظهر في وضع الموظف وحده: `paused` لا يُضبط إلّا من جلسة الموظف.
class EmployeeFreezeGate extends ConsumerWidget {
  const EmployeeFreezeGate({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final profile = ref.watch(employeeAuthProvider).profile;
    if (profile == null || !profile.paused) return const SizedBox.shrink();

    final message = (profile.pauseMessage != null &&
            profile.pauseMessage!.trim().isNotEmpty)
        ? profile.pauseMessage!.trim()
        : 'أوقفَ وكيلُك الخدمةَ مؤقتاً. تواصل مع الإدارة.';

    return Positioned.fill(
      /*
       * ⚠ طبقتان لا واحدة: حاجبٌ يمتصّ، ومحتوىً يتفاعل.
       *
       * كان `AbsorbPointer` يلفّ كلَّ شيء — فيمنع الوصول إلى ما تحته، ويمنع
       * كذلك أيَّ زرٍّ **فيه**. فما إن صار للشاشة بابُ مراسلةٍ حتى كان لا
       * يُضغط. الآن الحاجبُ طبقةٌ سفلى تبتلع كلَّ لمسةٍ تُفلت من المحتوى،
       * والمحتوى فوقه فيأخذ لمساته أولاً — وترتيبُ الطبقتين هو الحارس.
       */
      child: Stack(
        children: [
          const Positioned.fill(
            child: AbsorbPointer(
              child: ColoredBox(color: Color(0xF20B1220)),
            ),
          ),
          Container(
            alignment: Alignment.center,
            padding: const EdgeInsets.symmetric(horizontal: 32),
            child: SingleChildScrollView(
              child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Container(
                width: 96,
                height: 96,
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  color: Colors.white.withValues(alpha: .08),
                  border: Border.all(color: Colors.white.withValues(alpha: .18)),
                ),
                child: const Icon(Icons.pause_circle_outline_rounded,
                    size: 52, color: Colors.white),
              ),
              const SizedBox(height: 28),
              Text('توقّفٌ مؤقّت',
                  style: T.kufi(22, FontWeight.w700, color: Colors.white)),
              const SizedBox(height: 14),
              Text(
                message,
                textAlign: TextAlign.center,
                style: T.plex(14.5, FontWeight.w400,
                    color: Colors.white.withValues(alpha: .82), height: 1.7),
              ),
              /*
               * ⚠ بابُ المراسلة — وبغيره تكون الشاشة قد أمرت بما تمنع.
               *
               * الرسالةُ تقول «تواصل مع الإدارة»، والحاجبُ يمتصّ كلَّ لمسة
               * تحته، فلم يكن للموظف سبيلٌ إلى وكيله من داخل التطبيق أصلاً.
               * والخادمُ يستثني `CHAT_WITH_AGENT` وحدَها من بوّابة الإيقاف
               * (`EmployeePermissions::ALLOWED_WHILE_PAUSED`)، فهذا الزرّ هو
               * نظيرُ ذلك الاستثناء في الواجهة — لا عملٌ يُفتح، قناةُ كلامٍ فقط.
               *
               * ولا يظهر لمن لا يملك الصلاحية: زرٌّ يفتح ثمّ يُردّ 403 أسوأُ
               * من غيابه.
               */
              if (profile.can('CHAT_WITH_AGENT')) ...[
                const SizedBox(height: 26),
                TextButton.icon(
                  onPressed: () =>
                      Navigator.of(context, rootNavigator: true).push(
                    MaterialPageRoute(
                      builder: (_) => const ChatScreen(
                        title: 'الوكيل',
                        asEmployee: true,
                      ),
                    ),
                  ),
                  icon: const Icon(Icons.chat_bubble_outline_rounded,
                      size: 18, color: Colors.white),
                  label: Text('مراسلة الوكيل',
                      style: T.kufi(13.5, FontWeight.w700,
                          color: Colors.white)),
                  style: TextButton.styleFrom(
                    padding: const EdgeInsets.symmetric(
                        horizontal: 22, vertical: 13),
                    backgroundColor: Colors.white.withValues(alpha: .12),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(R.rCard),
                      side: BorderSide(
                          color: Colors.white.withValues(alpha: .22)),
                    ),
                  ),
                ),
              ],
              const SizedBox(height: 24),
              Container(
                padding:
                    const EdgeInsets.symmetric(horizontal: 18, vertical: 10),
                decoration: BoxDecoration(
                  color: Colors.white.withValues(alpha: .06),
                  borderRadius: BorderRadius.circular(R.rCard),
                  border: Border.all(color: Colors.white.withValues(alpha: .12)),
                ),
                child: Text('تعود الخدمة تلقائياً فور تشغيل الوكيل لك',
                    textAlign: TextAlign.center,
                    style: T.plex(12, FontWeight.w400,
                        color: Colors.white.withValues(alpha: .6))),
              ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}
