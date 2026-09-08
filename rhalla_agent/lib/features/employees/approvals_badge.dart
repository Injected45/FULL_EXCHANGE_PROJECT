import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import 'approvals_repository.dart';

/// شارةُ طلبات الموافقة المعلّقة.
///
/// ⚠ **لا نظامَ استطلاعٍ ثانياً** (البند 38): هذه الشارةُ تُحدَّث من
/// `AutoRefresh` نفسِه الذي يُحدّث جرسَ الوارد وشارةَ الدردشة — نبضةٌ واحدة
/// في الهيكل تسأل ما يلزم. ومؤقّتٌ ثالث يعني ثلاثةَ نداءاتٍ متفرّقة على
/// شبكةٍ ضعيفة، وبطاريّةً تُستهلك ثلاث مرّات لسؤالٍ واحد.
///
/// وهي في الهيكل لا في الشاشة عمداً: الوكيل قد يكون في التقارير أو الحساب
/// حين يُرسل موظفٌ طلباً، وشارةٌ لا تظهر إلّا وصاحبُها ينظر إليها ليست شارة.
class ApprovalsBadgeController extends StateNotifier<int> {
  ApprovalsBadgeController(this._ref) : super(0);

  final Ref _ref;

  /// ⚠ يبتلع الأخطاء عمداً: انقطاعُ شبكةٍ لحظيّ لا يجوز أن يُفرغ الشارة
  /// فيظنّ الوكيل أن الطلبات عولجت. تبقى على آخر عددٍ معلوم حتى يصل غيره.
  Future<void> refresh() async {
    try {
      state = await _ref.read(approvalsRepositoryProvider).pendingCount();
    } catch (_) {
      // متروك عمداً — انظر أعلاه.
    }
  }

  void clearLocal() => state = 0;
}

final approvalsBadgeProvider =
    StateNotifierProvider<ApprovalsBadgeController, int>(
        (ref) => ApprovalsBadgeController(ref));

/// أيقونةٌ بعدّادٍ صغير — تُستعمل في رأس شاشة الموظفين.
class ApprovalsBadgeIcon extends ConsumerWidget {
  const ApprovalsBadgeIcon({super.key, required this.onTap});

  final VoidCallback onTap;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final n = ref.watch(approvalsBadgeProvider);

    return Stack(
      clipBehavior: Clip.none,
      children: [
        IconButton(
          tooltip: 'طلبات الموافقة',
          onPressed: onTap,
          icon: Icon(Icons.fact_check_outlined,
              size: 22, color: n > 0 ? R.warnIcon : R.primaryDark),
          constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
        ),
        if (n > 0)
          Positioned(
            top: 4,
            left: 4,
            child: IgnorePointer(
              child: Container(
                padding: const EdgeInsets.symmetric(horizontal: 5, vertical: 1),
                constraints: const BoxConstraints(minWidth: 17),
                decoration: BoxDecoration(
                  color: R.error,
                  borderRadius: BorderRadius.circular(99),
                  border: Border.all(color: Colors.white, width: 1.5),
                ),
                // العددُ رقمٌ غربيّ وترتيبُه من اليسار — كسائر أرقام التطبيق.
                child: Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(
                    n > 99 ? '99+' : '$n',
                    textAlign: TextAlign.center,
                    style: T.kufi(9.5, FontWeight.w800, color: Colors.white),
                  ),
                ),
              ),
            ),
          ),
      ],
    );
  }
}
