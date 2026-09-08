import 'package:flutter/material.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import 'send_repository.dart' show TransferPendingApproval;

/// «بانتظار موافقة الوكيل» — ما يراه الموظف بدل شاشة النجاح.
///
/// ⚠ **لا علامةَ صحٍّ خضراء ولا كلمةَ «تمّت».** الحوالةُ لم تُنفَّذ: لا صفَّ
/// في الدفتر، ولا رصيدَ خُصم، ولا رقمَ حوالةٍ يُسلَّم للمستفيد (البند 37).
///
/// وهذه أهمُّ شاشةٍ في الميزة كلِّها: موظفٌ يقرأ «تمّت» ثمّ يسلّم المستفيدَ
/// نقداً على حوالةٍ لم تقع هو الضررُ الوحيد الذي لا يُصلحه شيءٌ بعدها. فالنصّ
/// هنا صريحٌ إلى حدّ التكرار، والزرُّ الوحيد يعيده إلى شاشته.
class PendingApprovalSheet extends StatelessWidget {
  const PendingApprovalSheet({super.key, required this.pending});

  final TransferPendingApproval pending;

  @override
  Widget build(BuildContext context) => Builder(
        /*
         * ⚠ **لا `PopScope(canPop: false)` هنا** — وقد كانت، فجمّدت التطبيق.
         *
         * `canPop: false` لا يمنع زرَّ الرجوع وحدَه: هو يعترض **كلّ** إغلاق
         * على هذه الصفحة، بما فيه `Navigator.pop()` الذي يستدعيه زرُّ
         * «حسناً» نفسُه. فبقيت الورقةُ مفتوحةً لا تُغلق بأي وسيلة، والموظفُ
         * أمام شاشةٍ لا تستجيب — وهو ما بدا «تجمّداً».
         *
         * ومنعُ التمرير بلا قراءة يكفيه ما مُرّر عند الفتح:
         * `isDismissible: false` و`enableDrag: false` — فلا تُغلق بنقرةٍ
         * خارجها ولا بسحبها. ويبقى زرُّ الرجوع مخرجاً، وهو الصواب: شاشةٌ
         * لا مخرجَ منها إلّا زرٌّ واحد تتحوّل إلى سجنٍ عند أوّل عطب فيه.
         */
        builder: (context) => Container(
          padding: const EdgeInsets.fromLTRB(22, 22, 22, 26),
          decoration: BoxDecoration(
            color: R.whiteA(.97),
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
              const SizedBox(height: 22),

              // ⚠ أيقونةُ انتظارٍ كهرمانيّة لا علامةَ صحٍّ خضراء: اللونُ
              // وحدَه يقول «لم تكتمل» قبل أن يُقرأ حرف.
              Center(
                child: Container(
                  width: 74,
                  height: 74,
                  decoration: BoxDecoration(
                    color: R.warnIcon.withValues(alpha: .10),
                    shape: BoxShape.circle,
                    border:
                        Border.all(color: R.warnIcon.withValues(alpha: .30)),
                  ),
                  child: Icon(Icons.hourglass_top_rounded,
                      size: 34, color: R.warnIcon),
                ),
              ),
              const SizedBox(height: 18),

              Text('بانتظار موافقة الوكيل',
                  textAlign: TextAlign.center,
                  style: T.kufi(17, FontWeight.w800, color: R.warnIcon)),
              const SizedBox(height: 12),

              Text(pending.message,
                  textAlign: TextAlign.center,
                  style: T.plex(13, FontWeight.w500,
                      color: R.inkA(.7), height: 1.9)),

              if (pending.reasons.isNotEmpty) ...[
                const SizedBox(height: 16),
                Container(
                  padding: const EdgeInsets.all(12),
                  decoration: BoxDecoration(
                    color: R.warnIcon.withValues(alpha: .06),
                    border:
                        Border.all(color: R.warnIcon.withValues(alpha: .22)),
                    borderRadius: BorderRadius.circular(R.rCard),
                  ),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('السبب',
                          style: T.plex(11.5, FontWeight.w700,
                              color: R.warnIcon)),
                      const SizedBox(height: 6),
                      // قد تجتمع أسبابٌ عدّة في طلبٍ واحد — تُعرض كلُّها.
                      ...pending.reasons.map((s) => Padding(
                            padding: const EdgeInsets.only(bottom: 4),
                            child: Text('• $s',
                                style: T.plex(12.5, FontWeight.w500,
                                    color: R.inkA(.68), height: 1.7)),
                          )),
                    ],
                  ),
                ),
              ],

              const SizedBox(height: 16),
              Container(
                padding: const EdgeInsets.all(12),
                decoration: BoxDecoration(
                  color: R.error.withValues(alpha: .06),
                  border: Border.all(color: R.error.withValues(alpha: .22)),
                  borderRadius: BorderRadius.circular(R.rCard),
                ),
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Icon(Icons.info_outline_rounded, size: 17, color: R.error),
                    const SizedBox(width: 8),
                    Expanded(
                      child: Text(
                        'الحوالة لم تُنفَّذ بعد، ولا يوجد رقم حوالة. '
                        'لا تسلّم المستفيد أي مبلغ قبل موافقة الوكيل.',
                        style: T.plex(12.5, FontWeight.w600,
                            color: R.error, height: 1.8),
                      ),
                    ),
                  ],
                ),
              ),

              const SizedBox(height: 14),
              Text(
                'بياناتك محفوظة ولا تحتاج إعادة إدخالها. '
                'ستظهر النتيجة في «طلباتي» فور بتّ الوكيل فيها.',
                textAlign: TextAlign.center,
                style: T.plex(12, FontWeight.w400,
                    color: R.inkA(.55), height: 1.8),
              ),

              const SizedBox(height: 22),
              PrimaryButton(
                label: 'حسناً',
                onPressed: () => Navigator.of(context).pop(),
              ),
            ],
          ),
        ),
      );
}
