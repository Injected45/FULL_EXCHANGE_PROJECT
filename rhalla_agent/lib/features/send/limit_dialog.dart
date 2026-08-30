import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import 'send_repository.dart';

/// إشعار تجاوز السقف — في وسط الشاشة، لا شريطاً أحمر.
///
/// **ليس خطأً بل حدّ.** الوكيل لم يُخطئ في شيء: الحوالة صحيحة والرصيد
/// يكفي، لكن السقف بلغ مداه. فاللون كهرماني كسائر تنبيهات التطبيق
/// (‏[WarnBanner]) لا أحمر، والنبرة اعتذار لا لوم.
///
/// النصّ ثابت ولا تتغيّر منه إلا كلمة المدّة — قرار المالك:
///
///     عفواً
///     لقد تجاوزت سقف التحويل **اليومي**
///
/// وحين يُتجاوز أكثر من سقف بالحوالة نفسها تُعطف الكلمات: «اليومي والأسبوعي».
Future<void> showLimitExceededDialog(
  BuildContext context,
  TransferLimitExceeded limit,
) =>
    showDialog<void>(
      context: context,
      barrierColor: R.inkA(.32),
      builder: (_) => _LimitDialog(limit: limit),
    );

/// نصّ التجاوز. ثابتٌ إلا كلمة المدّة — قرار المالك:
///
///     ['اليومي']                ⇦ لقد تجاوزت سقف التحويل اليومي
///     ['اليومي','الأسبوعي']     ⇦ لقد تجاوزت سقف التحويل اليومي والأسبوعي
///     []                        ⇦ لقد تجاوزت سقف التحويل
///
/// الحالة الأخيرة تقع إن لم يُرسل الخادم تسمية: تبقى الجملة سليمة بلا
/// كلمة معلّقة، ولا تُعرض كلمة إنجليزية من `type_from`.
String limitExceededMessage(List<String> labels) {
  final l = labels.map((e) => e.trim()).where((e) => e.isNotEmpty).toList();
  const head = 'لقد تجاوزت سقف التحويل';
  if (l.isEmpty) return head;
  if (l.length == 1) return '$head ${l.first}';
  // مسافة قبل الواو: «اليومي والأسبوعي» لا «اليوميوالأسبوعي».
  return '$head ${l.sublist(0, l.length - 1).join('، ')} و${l.last}';
}

class _LimitDialog extends StatelessWidget {
  const _LimitDialog({required this.limit});

  final TransferLimitExceeded limit;

  @override
  Widget build(BuildContext context) {
    return Dialog(
      backgroundColor: Colors.transparent,
      elevation: 0,
      insetPadding: const EdgeInsets.symmetric(horizontal: 32),
      child: ClipRRect(
        borderRadius: BorderRadius.circular(R.rCardXl),
        child: BackdropFilter(
          filter: ImageFilter.blur(sigmaX: R.blurGlassXl, sigmaY: R.blurGlassXl),
          child: Container(
            padding: const EdgeInsets.fromLTRB(24, 28, 24, 20),
            decoration: BoxDecoration(
              color: R.whiteA(.96),
              border: Border.all(color: R.whiteA(.9)),
              borderRadius: BorderRadius.circular(R.rCardXl),
            ),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Container(
                  width: 68,
                  height: 68,
                  alignment: Alignment.center,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: R.warnBg,
                    border: Border.all(color: R.warnBorder),
                  ),
                  child: Icon(Icons.speed_rounded, size: 31, color: R.warnIcon),
                ),
                const SizedBox(height: 20),
                Text('عفواً', style: T.kufi(21, FontWeight.w800)),
                const SizedBox(height: 12),
                Text(
                  limitExceededMessage(limit.labels),
                  textAlign: TextAlign.center,
                  style: T.kufi(15, FontWeight.w600, color: R.inkA(.72)),
                ),
                const SizedBox(height: 26),
                PrimaryButton(
                  label: 'حسناً',
                  onPressed: () => Navigator.of(context).pop(),
                ),
                const SizedBox(height: 4),
                TextButton(
                  onPressed: () {
                    Navigator.of(context).pop();
                    context.push('/limits');
                  },
                  style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
                  child: Text('عرض السقوف',
                      style:
                          T.plex(12.5, FontWeight.w600, color: R.primaryDark)),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
