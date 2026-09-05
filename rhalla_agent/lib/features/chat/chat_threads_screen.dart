import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'chat_repository.dart';
import 'chat_screen.dart';

/// قائمة محادثات الوكيل: الإدارة أوّلاً، ثم موظّفوه.
///
/// الإدارة في الأعلى دائماً وليست في الترتيب الزمني: هي المحادثة التي يفتحها
/// الوكيل حين يحتاج مساعدة، وبحثُه عنها بين عشر محادثات موظّفين يجعل الشاشة
/// تعمل ضدّ سبب وجودها.
class ChatThreadsScreen extends ConsumerWidget {
  const ChatThreadsScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(chatThreadsProvider);

    return Screen(
      child: Column(
        children: [
          // بلا زرّ رجوع: هذه تبويبٌ في الشريط لا شاشةٌ مدفوعة، والرجوع منها
          // يعني الخروج من التطبيق.
          const RhallaAppBar(title: 'الدردشة'),
          Expanded(
            child: async.when(
              loading: () => Center(
                child: CircularProgressIndicator(
                    color: R.primary, strokeWidth: 2.4),
              ),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(chatThreadsProvider),
              ),
              data: (items) => RefreshIndicator(
                onRefresh: () async => ref.refresh(chatThreadsProvider.future),
                color: R.primary,
                backgroundColor: Colors.white,
                child: ListView.separated(
                  // 120 في الأسفل: الشريط الزجاجي يعلو المحتوى، وبدونها
                  // يختفي آخر صفٍّ تحته.
                  padding: const EdgeInsets.fromLTRB(
                      R.padScreen, 16, R.padScreen, 120),
                  physics: const AlwaysScrollableScrollPhysics(),
                  itemCount: items.length,
                  separatorBuilder: (_, _) => const SizedBox(height: R.gapRow),
                  itemBuilder: (_, i) => _ThreadCard(
                    thread: items[i],
                    onTap: () async {
                      await Navigator.of(context, rootNavigator: true).push(
                        MaterialPageRoute(
                          builder: (_) => ChatScreen(
                            title: items[i].title,
                            threadId: items[i].id,
                          ),
                        ),
                      );
                      // العودة من المحادثة تُحدّث العدّادات: قرأها فنزل
                      // عدّادها، وقائمةٌ لا تعرف ذلك تعرض رقماً كاذباً.
                      ref.invalidate(chatThreadsProvider);
                    },
                  ),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}

class _ThreadCard extends StatelessWidget {
  const _ThreadCard({required this.thread, required this.onTap});

  final ChatThread thread;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    final unread = thread.unread;

    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(R.rCard),
      child: Container(
        padding: const EdgeInsets.fromLTRB(14, 13, 14, 13),
        decoration: BoxDecoration(
          color: Colors.white,
          border: Border.all(
              color: unread > 0 ? R.primaryA(.32) : R.inkA(.07)),
          borderRadius: BorderRadius.circular(R.rCard),
        ),
        child: Row(
          children: [
            Container(
              width: 44,
              height: 44,
              alignment: Alignment.center,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: thread.isAdmin ? R.primaryA(.12) : R.inkA(.05),
                border: Border.all(
                    color: thread.isAdmin ? R.primaryA(.3) : R.inkA(.1)),
              ),
              child: Icon(
                thread.isAdmin
                    ? Icons.support_agent_rounded
                    : Icons.person_outline_rounded,
                size: 21,
                color: thread.isAdmin ? R.primary : R.inkA(.55),
              ),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(thread.title,
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      style: T.kufi(14.5, FontWeight.w700)),
                  const SizedBox(height: 5),
                  Text(
                    thread.lastBody.isEmpty ? 'لا رسائل بعد' : thread.lastBody,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: T.plex(12, FontWeight.w400,
                        color: unread > 0 ? R.inkA(.8) : R.inkA(.5)),
                  ),
                ],
              ),
            ),
            const SizedBox(width: 10),
            Column(
              crossAxisAlignment: CrossAxisAlignment.end,
              children: [
                if (thread.lastMessageAt.isNotEmpty)
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Text(Fmt.stamp(thread.lastMessageAt),
                        style: T.plex(10, FontWeight.w400, color: R.inkA(.45))),
                  ),
                if (unread > 0) ...[
                  const SizedBox(height: 6),
                  Container(
                    constraints:
                        const BoxConstraints(minWidth: 20, minHeight: 20),
                    padding: const EdgeInsets.symmetric(horizontal: 5),
                    alignment: Alignment.center,
                    decoration: BoxDecoration(
                      color: R.error,
                      borderRadius: BorderRadius.circular(99),
                    ),
                    child: Directionality(
                      textDirection: TextDirection.ltr,
                      child: Text(unread > 9 ? '9+' : '$unread',
                          style: T.plex(11, FontWeight.w700,
                              color: Colors.white)),
                    ),
                  ),
                ],
              ],
            ),
          ],
        ),
      ),
    );
  }
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
