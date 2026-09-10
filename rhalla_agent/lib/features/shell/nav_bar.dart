import 'dart:ui';

import 'package:flutter/material.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';

/// شريطُ التبويبات الزجاجيّ — **واحدٌ للتطبيقين**.
///
/// ══════════════════════════════════════════════════════════════════════════
///  لماذا خرج من `app_shell.dart` إلى ملفٍّ مستقلّ
/// ══════════════════════════════════════════════════════════════════════════
///
/// أمرُ إعادة هيكلة واجهة الموظف (10 سبتمبر 2026) أعطاه شريطاً سفلياً بخمسة
/// تبويبات. والقاعدةُ الحاكمة في ذلك الأمر: «تطبيق الموظف صورةٌ معكوسة من
/// تطبيق الوكيل — أيُّ تعديلٍ هناك يتبعه هنا».
///
/// ⚠ وشريطٌ ثانٍ منسوخٌ كان سينقض تلك القاعدة في اليوم الأوّل: أيُّ تعديلٍ
/// على شريط الوكيل — لونٌ، ارتفاعٌ، شارةٌ، حركةٌ — يبقى في شريطه وحدَه، ثمّ
/// يُقال إنّ شريط الموظف «مثلُه» بينما هو مثلُه كما كان قبل شهر.
///
/// فهو شيفرةٌ واحدةٌ تُبنى مرّتين بقائمةِ عناصرَ مختلفة — لا شريطان.
class AppNavBar extends StatelessWidget {
  const AppNavBar({
    super.key,
    required this.items,
    required this.index,
    required this.onTap,
    this.badgeAt,
    this.badge = 0,
  });

  final List<NavItem> items;
  final int index;
  final ValueChanged<int> onTap;

  /// موضعُ التبويب الذي يحمل الشارة — و`null` يعني بلا شارة.
  final int? badgeAt;

  /// عددُ غير المقروء. صفرٌ يعني بلا شارة كذلك.
  final int badge;

  @override
  Widget build(BuildContext context) {
    return DecoratedBox(
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(R.rNav),
        boxShadow: R.shNav,
      ),
      child: ClipRRect(
        borderRadius: BorderRadius.circular(R.rNav),
        child: BackdropFilter(
          filter: ImageFilter.blur(sigmaX: R.blurNav, sigmaY: R.blurNav),
          child: Container(
            height: 72,
            padding: const EdgeInsets.symmetric(horizontal: 8),
            decoration: BoxDecoration(
              color: R.whiteA(.8),
              border: Border.all(color: R.whiteA(.92)),
              borderRadius: BorderRadius.circular(R.rNav),
            ),
            child: Row(
              children: [
                for (var i = 0; i < items.length; i++)
                  Expanded(
                    child: _Tab(
                      item: items[i],
                      active: i == index,
                      // ⚠ الاسمُ يُضغط حين تكثر التبويبات: شريطُ الموظف
                      // خمسةٌ لا أربعة، و«مراسلة الوكيل» على عرض الأربعة
                      // كان يُقصّ بثلاث نقاط فلا يُقرأ.
                      compact: items.length > 4,
                      // لا شارة على التبويب المفتوح: المستخدم ينظر إليه
                      // الآن، ورقمٌ فوق ما يقرؤه إلحاحٌ بلا معنى.
                      badge: i == badgeAt && i != index ? badge : 0,
                      onTap: () => onTap(i),
                    ),
                  ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

class NavItem {
  const NavItem(this.label, this.icon, this.activeIcon);
  final String label;
  final IconData icon;
  final IconData activeIcon;
}

class _Tab extends StatelessWidget {
  const _Tab({
    required this.item,
    required this.active,
    required this.onTap,
    this.badge = 0,
    this.compact = false,
  });

  final NavItem item;
  final bool active;
  final VoidCallback onTap;

  /// عدد غير المقروء — صفرٌ يعني بلا شارة.
  final int badge;

  /// شريطٌ بأكثر من أربعة تبويبات: خطٌّ أصغر وحشوةٌ أضيق.
  final bool compact;

  @override
  Widget build(BuildContext context) {
    // .5 بدل .42 — الأخيرة تسقط تحت 2.5:1 على الزجاج، وهي التنقّل الرئيسي.
    final color = active ? R.primaryGradEnd : R.inkA(.5);

    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(R.rNav),
      child: SizedBox(
        height: 72,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            AnimatedContainer(
              duration: const Duration(milliseconds: 220),
              padding: EdgeInsets.symmetric(
                  horizontal: active ? (compact ? 11 : 16) : 0, vertical: 5),
              decoration: BoxDecoration(
                color: active ? R.primaryA(.14) : Colors.transparent,
                borderRadius: BorderRadius.circular(99),
              ),
              child: Stack(
                clipBehavior: Clip.none,
                children: [
                  Icon(active ? item.activeIcon : item.icon,
                      size: compact ? 20 : 21, color: color),

                  // شارة غير المقروء على أيقونة التبويب.
                  //
                  // على الأيقونة لا بجانب الاسم: التبويب النشط يتمدّد أفقياً
                  // (‏AnimatedContainer أعلاه)، فشارةٌ في الصفّ كانت تتحرّك
                  // مع كل تبديل تبويب.
                  if (badge > 0)
                    PositionedDirectional(
                      top: -5,
                      end: -8,
                      child: Container(
                        constraints:
                            const BoxConstraints(minWidth: 16, minHeight: 16),
                        padding: const EdgeInsets.symmetric(horizontal: 4),
                        alignment: Alignment.center,
                        decoration: BoxDecoration(
                          color: R.error,
                          borderRadius: BorderRadius.circular(99),
                          // حدٌّ بلون الشريط الزجاجي يفصل الرقم عن الأيقونة
                          // حين يعلوها.
                          border: Border.all(color: Colors.white, width: 1.5),
                        ),
                        child: Directionality(
                          // رقمٌ لاتيني في فقرة عربية — يُفرض اتجاهه.
                          textDirection: TextDirection.ltr,
                          child: Text(badge > 9 ? '9+' : '$badge',
                              style: T.plex(9.5, FontWeight.w700,
                                  color: Colors.white)),
                        ),
                      ),
                    ),
                ],
              ),
            ),
            const SizedBox(height: 6),
            // ⚠ الاسمُ يُصغَّر ولا يُقصّ: «لا يُحذف حقلٌ ولا كلمة» شرطٌ في
            // الأمر، وثلاثُ نقاطٍ مكان نصف الكلمة حذفٌ يراه المستخدم.
            Padding(
              padding: EdgeInsets.symmetric(horizontal: compact ? 2 : 0),
              child: FittedBox(
                fit: BoxFit.scaleDown,
                child: Text(
                  item.label,
                  maxLines: 1,
                  style: T.plex(compact ? 10.5 : 11,
                      active ? FontWeight.w600 : FontWeight.w500, color: color),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
