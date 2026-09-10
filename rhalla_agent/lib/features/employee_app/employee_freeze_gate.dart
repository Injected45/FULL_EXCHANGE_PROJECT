import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
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
      // يمتصّ كلَّ لمسة: لا وصول لأيّ زرٍّ تحته.
      child: AbsorbPointer(
        child: Container(
          color: const Color(0xF20B1220),
          alignment: Alignment.center,
          padding: const EdgeInsets.symmetric(horizontal: 32),
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
    );
  }
}
