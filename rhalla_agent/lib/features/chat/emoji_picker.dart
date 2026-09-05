import 'package:flutter/material.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';

/// لوحة إيموجي — بلا حزمة.
///
/// ## لماذا قائمةٌ مكتوبة لا حزمة جاهزة
///
/// حزم الإيموجي تحمل آلاف الرموز وجداولَ بحثٍ وأيقوناتٍ خاصّة بها، فتضيف
/// ميغابايتات إلى APK قرّر المالك تصغيره إلى أقصى حدّ. وهذه قائمةٌ منتقاة
/// لعملٍ لا لدردشة أصدقاء: ما يحتاجه وكيلٌ يراسل إدارته أو موظّفه.
///
/// والرموز نفسها **لا تُخزَّن في التطبيق**: يرسمها خطّ النظام، فلا حجم لها
/// أصلاً — ما هنا نصٌّ لا صور.
///
/// ⚠ ولا يمنع هذا لوحةَ مفاتيح الجهاز: من أراد رمزاً خارج القائمة يجده في
/// لوحته كما في أي تطبيق. هذه اختصارٌ لا سجن.
class EmojiPicker extends StatelessWidget {
  const EmojiPicker({super.key, required this.onPick});

  final ValueChanged<String> onPick;

  static const _groups = <String, List<String>>{
    'شائعة': [
      '👍', '🙏', '✅', '❌', '⚠️', '❗', '❓', '🔴', '🟢', '🟡',
      '👌', '👏', '🤝', '💪', '🙌', '☝️', '✋', '🤲', '👋', '✍️',
    ],
    'وجوه': [
      '🙂', '😊', '😀', '😅', '😂', '🥲', '😍', '🤔', '😐', '😴',
      '😢', '😭', '😡', '😰', '🤒', '😎', '🤗', '🫡', '🥺', '😉',
    ],
    'عمل': [
      '💰', '💵', '💳', '🧾', '📄', '📎', '📌', '📥', '📤', '🏦',
      '📱', '📞', '☎️', '📧', '🖨️', '🗓️', '⏰', '⌛', '🔒', '🔑',
    ],
    'أخرى': [
      '🌹', '🌟', '🔥', '💯', '🎉', '☕', '🚗', '🚕', '🏠', '📍',
      '🇱🇾', '☀️', '🌙', '🕌', '❤️', '💚', '💙', '⭐', '🎯', '🧿',
    ],
  };

  @override
  Widget build(BuildContext context) => Container(
        height: 232,
        decoration: BoxDecoration(
          color: Colors.white,
          border: Border(top: BorderSide(color: R.inkA(.08))),
        ),
        child: DefaultTabController(
          length: _groups.length,
          child: Column(
            children: [
              SizedBox(
                height: 38,
                child: TabBar(
                  labelColor: R.primaryDark,
                  unselectedLabelColor: R.inkA(.5),
                  indicatorColor: R.primary,
                  indicatorSize: TabBarIndicatorSize.label,
                  labelStyle: T.kufi(12.5, FontWeight.w700),
                  unselectedLabelStyle: T.kufi(12.5, FontWeight.w500),
                  tabs: [for (final g in _groups.keys) Tab(text: g)],
                ),
              ),
              Expanded(
                child: TabBarView(
                  children: [
                    for (final list in _groups.values)
                      GridView.builder(
                        padding: const EdgeInsets.symmetric(
                            horizontal: 10, vertical: 8),
                        gridDelegate:
                            const SliverGridDelegateWithFixedCrossAxisCount(
                          crossAxisCount: 8,
                          mainAxisSpacing: 2,
                          crossAxisSpacing: 2,
                        ),
                        itemCount: list.length,
                        itemBuilder: (_, i) => InkWell(
                          onTap: () => onPick(list[i]),
                          borderRadius: BorderRadius.circular(8),
                          child: Center(
                            child: Text(list[i],
                                style: const TextStyle(fontSize: 24)),
                          ),
                        ),
                      ),
                  ],
                ),
              ),
            ],
          ),
        ),
      );
}
