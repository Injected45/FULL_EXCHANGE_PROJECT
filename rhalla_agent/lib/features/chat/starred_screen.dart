import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'chat_repository.dart';
import 'chat_screen.dart';

/// «الرسائل المهمّة» — ما مُيِّز بنجمة، عبر كل المحادثات (البند 30).
///
/// شاشةٌ واحدة لكل المحادثات لا نجمةٌ داخل كلٍّ منها: الغرض من التمييز أن
/// يجد الوكيل ما حفظه **بلا أن يتذكّر أين قيل** — وقائمةٌ داخل كل محادثة
/// تُبقي عليه عبء التذكّر.
class StarredScreen extends ConsumerWidget {
  const StarredScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(starredMessagesProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'الرسائل المهمّة', onBack: () => context.pop()),
          Expanded(
            child: async.when(
              loading: () => Center(
                child: CircularProgressIndicator(
                    color: R.primary, strokeWidth: 2.4),
              ),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(starredMessagesProvider),
              ),
              data: (items) {
                if (items.isEmpty) return const _Empty();

                return RefreshIndicator(
                  onRefresh: () => ref
                      .refresh(starredMessagesProvider.future)
                      .then((_) {}, onError: (_) {}),
                  color: R.primary,
                  backgroundColor: Colors.white,
                  child: ListView.separated(
                    padding: const EdgeInsets.fromLTRB(
                        R.padScreen, 16, R.padScreen, 40),
                    physics: const AlwaysScrollableScrollPhysics(),
                    itemCount: items.length,
                    separatorBuilder: (_, _) => const SizedBox(height: R.gapRow),
                    itemBuilder: (_, i) => _StarCard(
                      item: items[i],
                      onTap: () async {
                        // فتح المحادثة التي قيلت فيها: نجمةٌ لا تعيدك إلى
                        // سياقها نصفُ ميزة.
                        await Navigator.of(context, rootNavigator: true).push(
                          MaterialPageRoute(
                            builder: (_) => ChatScreen(
                              title: items[i].threadTitle,
                              threadId: items[i].threadId,
                              // تُفتح المحادثة **على هذه الرسالة** وتُبرزها
                              // — لا على آخرها.
                              highlightMessageId: items[i].id,
                            ),
                          ),
                        );
                        // قد يكون رفع النجمة من هناك — تُعاد القراءة.
                        ref.invalidate(starredMessagesProvider);
                      },
                    ),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}

class _StarCard extends StatelessWidget {
  const _StarCard({required this.item, required this.onTap});

  final StarredMessage item;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(R.rCard),
        child: Container(
          padding: const EdgeInsets.fromLTRB(14, 12, 14, 12),
          decoration: BoxDecoration(
            color: Colors.white,
            border: Border.all(color: R.inkA(.08)),
            borderRadius: BorderRadius.circular(R.rCard),
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  Icon(Icons.star_rounded, size: 15, color: R.warnIcon),
                  const SizedBox(width: 6),
                  Expanded(
                    child: Text(item.threadTitle,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: T.plex(11.5, FontWeight.w700,
                            color: R.primaryDark)),
                  ),
                  Text(Fmt.stampShort(item.createdAt),
                      style:
                          T.plex(10, FontWeight.w400, color: R.inkA(.45))),
                ],
              ),
              const SizedBox(height: 8),
              Text(
                item.preview,
                maxLines: 3,
                overflow: TextOverflow.ellipsis,
                style: T.kufi(13.5, FontWeight.w500, height: 1.5),
              ),
              if (item.senderName.isNotEmpty) ...[
                const SizedBox(height: 6),
                Text('— ${item.senderName}',
                    style: T.plex(11, FontWeight.w400, color: R.inkA(.5))),
              ],
            ],
          ),
        ),
      );
}

class _Empty extends StatelessWidget {
  const _Empty();

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 40),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(Icons.star_outline_rounded, size: 46, color: R.warnIcon),
              const SizedBox(height: 14),
              Text('لا رسائل مهمّة بعد',
                  style: T.kufi(15, FontWeight.w700, color: R.inkA(.7))),
              const SizedBox(height: 8),
              Text(
                'اضغط مطوّلاً على أي رسالة ثم «حفظ كمهمّة»، '
                'وستجدها هنا مهما مضى عليها.',
                textAlign: TextAlign.center,
                style: T.plex(12.5, FontWeight.w400, color: R.inkA(.5)),
              ),
            ],
          ),
        ),
      );
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 32),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(Icons.error_outline_rounded, size: 40, color: R.error),
              const SizedBox(height: 12),
              Text(message,
                  textAlign: TextAlign.center,
                  style: T.plex(12.5, FontWeight.w400, color: R.inkA(.6))),
              const SizedBox(height: 16),
              GlassButton(label: 'إعادة المحاولة', onPressed: onRetry),
            ],
          ),
        ),
      );
}
