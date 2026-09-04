import 'package:flutter/material.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'agent_incoming_repository.dart';

/// «تمّ تسليم الحوالة بنجاح» — تأكيدٌ بين ضغط الزرّ وما بعده.
///
/// قبلها كانت الفاتورة تُغلق صامتةً بعد نجاح التسجيل، فيعود الوكيل إلى
/// القائمة بلا شيء يقول إن التسليم وقع. وهذه عمليةٌ لا تراجع فيها ويُدفع
/// فيها مال، فتأكيدٌ صريحٌ بعدها ليس زينة: بلا إقرار مرئي يظلّ الوكيل
/// يتساءل هل سُجّل التسليم، فيعيد فتح الحوالة ليطمئنّ.
///
/// وتُعرَض بـ `pushReplacement` مكان الفاتورة لا فوقها: الحوالة صارت
/// مسلَّمة، وفاتورةٌ بزرّ «تسجيل التسليم» تحتها في المكدّس يعود إليها
/// بضغطة رجوع واحدة.
class DeliveryDoneScreen extends StatelessWidget {
  const DeliveryDoneScreen({
    super.key,
    required this.transfer,
    required this.currency,
  });

  final AgentIncomingTransfer transfer;
  final String currency;

  @override
  Widget build(BuildContext context) => Screen(
        child: Column(
          children: [
            const Spacer(),
            Padding(
              padding: const EdgeInsets.symmetric(horizontal: R.padScreen),
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  // العلامة نفسها التي تختم بها الحوالة الصادرة — نجاحٌ واحد
                  // بمظهرٍ واحد في التطبيق كلّه.
                  SizedBox(
                    width: 84,
                    height: 84,
                    child: Stack(
                      alignment: Alignment.center,
                      children: [
                        const Positioned.fill(child: PulseRing(seconds: 2.4)),
                        Container(
                          width: 64,
                          height: 64,
                          decoration: BoxDecoration(
                            shape: BoxShape.circle,
                            gradient: R.primaryGradient,
                            boxShadow: [
                              BoxShadow(
                                color: R.primaryA(.36),
                                blurRadius: 34,
                                offset: const Offset(0, 16),
                              )
                            ],
                          ),
                          child: const Icon(Icons.check_rounded,
                              size: 34, color: Colors.white),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(height: 22),
                  RiseIn.small(
                    delay: const Duration(milliseconds: 120),
                    child: Text('تمّ تسليم الحوالة بنجاح',
                        textAlign: TextAlign.center, style: T.titleSm),
                  ),
                  const SizedBox(height: 20),
                  RiseIn.small(
                    delay: const Duration(milliseconds: 200),
                    child: GlassCard(
                      child: Column(
                        children: [
                          _Line(
                            label: 'المستفيد',
                            value: transfer.receiverName.isEmpty
                                ? 'بلا اسم'
                                : transfer.receiverName,
                          ),
                          const SizedBox(height: 10),
                          _Line(label: 'رقم الحوالة', value: transfer.code, ltr: true),
                          const SizedBox(height: 10),
                          // المبلغ آخر سطر ليقع عليه البصر أخيراً — وهو ما
                          // دفعه الوكيل من يده.
                          _Line(
                            label: 'المبلغ المسلَّم',
                            value: Fmt.money(transfer.amount),
                            currency: currency,
                            strong: true,
                          ),
                        ],
                      ),
                    ),
                  ),
                ],
              ),
            ),
            const Spacer(),
            Padding(
              padding: const EdgeInsets.fromLTRB(
                  R.padScreen, 0, R.padScreen, R.padScreen),
              child: PrimaryButton(
                label: 'تمّ',
                onPressed: () => Navigator.of(context).pop(),
              ),
            ),
          ],
        ),
      );
}

class _Line extends StatelessWidget {
  const _Line({
    required this.label,
    required this.value,
    this.currency,
    this.ltr = false,
    this.strong = false,
  });

  final String label;
  final String value;
  final String? currency;
  final bool ltr;
  final bool strong;

  @override
  Widget build(BuildContext context) {
    final text = Text(
      value,
      maxLines: 1,
      overflow: TextOverflow.ellipsis,
      style: strong
          ? T.kufi(16, FontWeight.w800, color: R.primaryDark)
          : T.plex(13.5, FontWeight.w600),
    );

    return Row(
      children: [
        Expanded(child: Text(label, style: T.label)),
        const SizedBox(width: 12),
        // الرمز يسار الرقم، والمقطع كلّه LTR لأن الرقم لا يُعاد ترتيبه.
        if (currency != null)
          Directionality(
            textDirection: TextDirection.ltr,
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(currency!,
                    style: T.plex(11.5, FontWeight.w500, color: R.inkA(.5))),
                const SizedBox(width: 5),
                text,
              ],
            ),
          )
        else if (ltr)
          Directionality(textDirection: TextDirection.ltr, child: text)
        else
          Flexible(child: text),
      ],
    );
  }
}
