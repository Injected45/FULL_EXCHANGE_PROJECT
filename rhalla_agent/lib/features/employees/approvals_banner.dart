import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import 'approvals_badge.dart';

/// تنبيهُ طلبات الموافقة — في الشاشة الرئيسية.
///
/// ══════════════════════════════════════════════════════════════════════════
///  لماذا هنا، وقد كانت الشارةُ في رأس شاشة «الموظفون»؟
/// ══════════════════════════════════════════════════════════════════════════
///
/// ⚠ **لأنّ الوكيل لا ينظر إلى شاشة الموظفين.**
///
/// حدث ذلك فعلاً: موظفٌ أنشأ حوالةً تجاوزت سقفه فرأى «بانتظار موافقة
/// الوكيل»، والطلبُ وصل الخادمَ سليماً وكان العدّاد يقول «واحد» — لكنّ
/// الوكيل لم يرَ شيئاً، لأنّ الشارةَ الوحيدة كانت مدفونةً في رأس شاشةٍ
/// فرعيّة لا يفتحها إلّا حين يريد إدارة موظفيه.
///
/// وحوالةُ زبونٍ تنتظر قراراً لا يجوز أن تنتظر حتى يتذكّر الوكيل فتح شاشة.
/// فالقاعدة: **ما ينتظر قرارك يظهر حيث تنظر**.
///
/// ⚠ ولا يظهر إلّا حين يكون هناك ما ينتظر فعلاً — لافتةٌ دائمة تُقرأ مرّةً
/// ثمّ تصير جزءاً من الخلفية.
class ApprovalsBanner extends ConsumerWidget {
  const ApprovalsBanner({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final n = ref.watch(approvalsBadgeProvider);
    if (n <= 0) return const SizedBox.shrink();

    return Padding(
      padding: const EdgeInsets.fromLTRB(R.padScreen, 0, R.padScreen, R.gapCard),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: () => context.push('/employees/approvals'),
          borderRadius: BorderRadius.circular(R.rCard),
          child: Container(
            padding: const EdgeInsets.all(14),
            decoration: BoxDecoration(
              // ⚠ كهرمانيّ لا أخضر: هذا انتظارٌ يحتاج فعلاً، لا خبرٌ سارّ.
              color: R.warnIcon.withValues(alpha: .09),
              border: Border.all(color: R.warnIcon.withValues(alpha: .35)),
              borderRadius: BorderRadius.circular(R.rCard),
            ),
            child: Row(
              children: [
                Container(
                  width: 38,
                  height: 38,
                  decoration: BoxDecoration(
                    color: R.warnIcon.withValues(alpha: .14),
                    shape: BoxShape.circle,
                  ),
                  child: Icon(Icons.fact_check_outlined,
                      size: 20, color: R.warnIcon),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        n == 1
                            ? 'حوالة بانتظار موافقتك'
                            : 'حوالات بانتظار موافقتك',
                        style: T.kufi(14, FontWeight.w800, color: R.warnIcon),
                      ),
                      const SizedBox(height: 3),
                      Text(
                        // ⚠ ويُقال إنها لم تُنفَّذ صراحةً: وكيلٌ يظنّها وقعت
                        // لا يستعجل، وموظفُه واقفٌ أمام الزبون ينتظر.
                        n == 1
                            ? 'حوالة تجاوزت سقف موظفها ولم تُنفَّذ بعد.'
                            : '$n حوالات تجاوزت سقف موظفيها ولم تُنفَّذ بعد.',
                        style: T.plex(12, FontWeight.w500,
                            color: R.inkA(.6), height: 1.6),
                      ),
                    ],
                  ),
                ),
                const SizedBox(width: 8),
                Icon(Icons.chevron_left_rounded, size: 22, color: R.warnIcon),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
