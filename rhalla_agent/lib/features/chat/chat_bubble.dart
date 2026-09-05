import 'package:flutter/material.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import 'chat_repository.dart';
import 'voice_note.dart';

/// فقاعة رسالة — نصّاً أو صورةً أو صوتاً، وعليها الاقتباس والإيصال.
class ChatBubble extends StatelessWidget {
  const ChatBubble({
    super.key,
    required this.message,
    required this.mine,
    required this.receipts,
    required this.imageUrl,
    required this.imageHeaders,
    this.onTapImage,
    this.reactions = const {},
    this.starred = false,
    this.onTapReaction,
  });

  /// تفاعلات هذه الرسالة — رمزٌ وعدده، وهل تفاعلتُ أنا به (البند 25).
  final Map<String, ReactionCount> reactions;

  /// محفوظة عندي (البند 30).
  final bool starred;

  /// الضغط على رمزٍ ظاهر يضيف تفاعلي به أو يزيله.
  final ValueChanged<String>? onTapReaction;

  final ChatMessage message;
  final bool mine;
  final ChatReceipts receipts;

  /// مسار المرفق الكامل — يُبنى في الشاشة لأنه يختلف بين وضعَي الوكيل
  /// والموظّف.
  final String imageUrl;

  /// ترويسة التوثيق: المرفقات خلف `auth:sanctum`، و`Image.network` لا يمرّ
  /// بـ dio فلا يحملها من تلقائه.
  final Map<String, String> imageHeaders;

  final VoidCallback? onTapImage;

  @override
  Widget build(BuildContext context) {
    final w = MediaQuery.sizeOf(context).width;

    if (message.deleted) return _Deleted(mine: mine);

    return Padding(
      padding: const EdgeInsets.only(bottom: 10),
      child: Align(
        alignment:
            mine ? AlignmentDirectional.centerEnd : AlignmentDirectional.centerStart,
        child: Container(
          constraints: BoxConstraints(maxWidth: w * .78),
          padding: message.isImage
              // الصورة تملأ الفقاعة إلى حوافّها — حشوةٌ حولها تجعلها تبدو
              // بطاقةً داخل بطاقة.
              ? const EdgeInsets.all(4)
              : const EdgeInsets.fromLTRB(14, 10, 14, 8),
          decoration: BoxDecoration(
            color: mine ? R.primary : Colors.white,
            border: Border.all(color: mine ? R.primary : R.inkA(.08)),
            borderRadius: BorderRadius.only(
              topLeft: const Radius.circular(16),
              topRight: const Radius.circular(16),
              bottomLeft: Radius.circular(mine ? 16 : 4),
              bottomRight: Radius.circular(mine ? 4 : 16),
            ),
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            mainAxisSize: MainAxisSize.min,
            children: [
              if (!mine && message.senderName.isNotEmpty && !message.isImage) ...[
                Text(message.senderName,
                    style: T.plex(11.5, FontWeight.w600, color: R.primaryDark)),
                const SizedBox(height: 4),
              ],

              if (message.hasReply) _Quote(message: message, mine: mine),

              if (message.isImage) ...[
                ClipRRect(
                  borderRadius: BorderRadius.circular(13),
                  child: GestureDetector(
                    onTap: onTapImage,
                    child: Image.network(
                      imageUrl,
                      headers: imageHeaders,
                      fit: BoxFit.cover,
                      // ارتفاعٌ محدود: صورةٌ طويلة تملأ الشاشة فتدفع ما
                      // قبلها وما بعدها خارج النظر.
                      height: 210,
                      width: double.infinity,
                      loadingBuilder: (c, child, p) => p == null
                          ? child
                          : Container(
                              height: 210,
                              alignment: Alignment.center,
                              color: R.inkA(.05),
                              child: CircularProgressIndicator(
                                  strokeWidth: 2, color: R.primary),
                            ),
                      errorBuilder: (_, _, _) => Container(
                        height: 120,
                        alignment: Alignment.center,
                        color: R.inkA(.05),
                        child: Icon(Icons.broken_image_outlined,
                            color: R.inkA(.4), size: 30),
                      ),
                    ),
                  ),
                ),
                if (message.body.isNotEmpty) const SizedBox(height: 8),
              ],

              if (message.isAudio)
                VoiceBubble(
                  url: imageUrl,
                  headers: imageHeaders,
                  mine: mine,
                  // مدّة تقديرية من حجم الملف قبل تحميله: Opus عند 24 kbps
                  // يعطي 3000 بايت للثانية تقريباً. تُستبدل بالمدّة الحقيقية
                  // فور بدء التشغيل، وتمنع ظهور «0:00» في الانتظار.
                  durationHint: Duration(
                      seconds: (message.attachmentSize / 3000).round().clamp(0, 3600)),
                ),

              if (message.hasAttachment && !message.isImage && !message.isAudio)
                _FileChip(message: message, mine: mine),

              if (message.body.isNotEmpty)
                Padding(
                  padding: EdgeInsets.symmetric(
                      horizontal: message.isImage ? 10 : 0),
                  child: Text(
                    message.body,
                    style: T.kufi(14.5, FontWeight.w500,
                        height: 1.5, color: mine ? Colors.white : R.ink),
                  ),
                ),

              const SizedBox(height: 5),
              Padding(
                padding:
                    EdgeInsets.symmetric(horizontal: message.isImage ? 10 : 0),
                child: Align(
                  alignment: AlignmentDirectional.centerEnd,
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Directionality(
                        textDirection: TextDirection.ltr,
                        child: Text(
                          Fmt.stamp(message.createdAt),
                          style: T.plex(10, FontWeight.w400,
                              color: mine ? R.whiteA(.75) : R.inkA(.45)),
                        ),
                      ),
                      // «تم التعديل» ظاهرة ولا تُخفى (البند 28).
                      if (message.edited) ...[
                        const SizedBox(width: 5),
                        Text('مُعدَّلة',
                            style: T.plex(9.5, FontWeight.w400,
                                color: mine ? R.whiteA(.7) : R.inkA(.4))),
                      ],
                      if (starred) ...[
                        const SizedBox(width: 5),
                        Icon(Icons.star_rounded,
                            size: 12,
                            color: mine ? R.whiteA(.85) : R.warnIcon),
                      ],
                      if (message.pinned) ...[
                        const SizedBox(width: 4),
                        Icon(Icons.push_pin_rounded,
                            size: 11,
                            color: mine ? R.whiteA(.85) : R.inkA(.45)),
                      ],
                      // الإيصال على رسائلي وحدها: علامةٌ على كلام الآخر
                      // تعني «قرأتُها أنا»، وهي معلومةٌ لا يحتاجها.
                      if (mine) ...[
                        const SizedBox(width: 5),
                        _Ticks(id: message.id, receipts: receipts),
                      ],
                    ],
                  ),
                ),
              ),

              // التفاعلات أسفل الفقاعة داخلها (البند 25): خارجَها كانت
              // تُزيح الفقاعة التالية وتكسر انتظام العمود.
              if (reactions.isNotEmpty) ...[
                const SizedBox(height: 6),
                Padding(
                  padding: EdgeInsets.symmetric(
                      horizontal: message.isImage ? 10 : 0),
                  child: Wrap(
                    spacing: 5,
                    runSpacing: 4,
                    children: [
                      for (final e in reactions.entries)
                        _ReactionChip(
                          emoji: e.key,
                          count: e.value.count,
                          mine: e.value.mine,
                          // فقاعة رسالتي أم رسالته — يُقرأ من الحقل لا من
                          // `e.value.mine` وهو «تفاعلتُ أنا بهذا الرمز».
                          onBubble: mine,
                          onTap: () => onTapReaction?.call(e.key),
                        ),
                    ],
                  ),
                ),
              ],
            ],
          ),
        ),
      ),
    );
  }
}

/// رمزُ تفاعلٍ وعدده. المضغوط له إطارٌ يميّزه — فيعرف صاحبه أنه تفاعل به.
class _ReactionChip extends StatelessWidget {
  const _ReactionChip({
    required this.emoji,
    required this.count,
    required this.mine,
    required this.onBubble,
    required this.onTap,
  });

  final String emoji;
  final int count;

  /// تفاعلتُ أنا بهذا الرمز.
  final bool mine;

  /// الفقاعة رسالتي — فألوان الرقاقة تتبعها لا الخلفية البيضاء.
  final bool onBubble;

  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(99),
        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 7, vertical: 3),
          decoration: BoxDecoration(
            color: onBubble ? R.whiteA(.18) : R.inkA(.05),
            border: Border.all(
              color: mine
                  ? (onBubble ? Colors.white70 : R.primary)
                  : Colors.transparent,
              width: 1.2,
            ),
            borderRadius: BorderRadius.circular(99),
          ),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Text(emoji, style: const TextStyle(fontSize: 13)),
              if (count > 1) ...[
                const SizedBox(width: 3),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text('$count',
                      style: T.plex(10, FontWeight.w700,
                          color: onBubble ? Colors.white : R.inkA(.6))),
                ),
              ],
            ],
          ),
        ),
      );
}

/// ✓ أُرسلت · ✓✓ وصلت · ✓✓ زرقاء قُرئت — كما اعتادها المستخدم.
class _Ticks extends StatelessWidget {
  const _Ticks({required this.id, required this.receipts});

  final int id;
  final ChatReceipts receipts;

  @override
  Widget build(BuildContext context) {
    final read = receipts.isRead(id);
    final delivered = receipts.isDelivered(id);

    return Icon(
      delivered ? Icons.done_all_rounded : Icons.check_rounded,
      size: 14,
      // أزرق فاتح على الأخضر: الأزرق القياسي لا يُرى على فقاعة داكنة.
      color: read ? const Color(0xFF7FD4FF) : R.whiteA(.7),
    );
  }
}

/// اقتباس الرسالة المُردود عليها.
class _Quote extends StatelessWidget {
  const _Quote({required this.message, required this.mine});

  final ChatMessage message;
  final bool mine;

  @override
  Widget build(BuildContext context) => Container(
        margin: const EdgeInsets.only(bottom: 7),
        padding: const EdgeInsets.fromLTRB(9, 6, 9, 6),
        decoration: BoxDecoration(
          color: mine ? R.whiteA(.16) : R.inkA(.04),
          // شريطٌ في الجانب المبدوء به — علامة الاقتباس المعروفة.
          border: BorderDirectional(
            start: BorderSide(
                color: mine ? Colors.white70 : R.primary, width: 3),
          ),
          borderRadius: BorderRadius.circular(8),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          mainAxisSize: MainAxisSize.min,
          children: [
            if (message.replySenderName.isNotEmpty)
              Text(message.replySenderName,
                  style: T.plex(10.5, FontWeight.w700,
                      color: mine ? Colors.white : R.primaryDark)),
            Text(
              message.replyPreview.isEmpty
                  ? 'رسالة محذوفة'
                  : message.replyPreview,
              maxLines: 2,
              overflow: TextOverflow.ellipsis,
              style: T.kufi(12, FontWeight.w400,
                  color: mine ? R.whiteA(.85) : R.inkA(.6)),
            ),
          ],
        ),
      );
}

class _FileChip extends StatelessWidget {
  const _FileChip({required this.message, required this.mine});

  final ChatMessage message;
  final bool mine;

  @override
  Widget build(BuildContext context) {
    final kb = (message.attachmentSize / 1024).round();

    return Container(
      margin: const EdgeInsets.only(bottom: 6),
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 8),
      decoration: BoxDecoration(
        color: mine ? R.whiteA(.16) : R.inkA(.04),
        borderRadius: BorderRadius.circular(10),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(Icons.insert_drive_file_outlined,
              size: 18, color: mine ? Colors.white : R.primary),
          const SizedBox(width: 8),
          Flexible(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(message.attachmentName,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: T.kufi(12.5, FontWeight.w600,
                        color: mine ? Colors.white : R.ink)),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text('$kb KB',
                      style: T.plex(10, FontWeight.w400,
                          color: mine ? R.whiteA(.7) : R.inkA(.45))),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

/// «حُذفت هذه الرسالة» — تبقى مكانها ولا تختفي.
///
/// اختفاؤها كان يجعل ردّاً عليها معلّقاً في الهواء، ويجعل الطرف الآخر يظنّ
/// أنه لم يقرأ شيئاً أصلاً.
class _Deleted extends StatelessWidget {
  const _Deleted({required this.mine});

  final bool mine;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.only(bottom: 10),
        child: Align(
          alignment: mine
              ? AlignmentDirectional.centerEnd
              : AlignmentDirectional.centerStart,
          child: Container(
            padding: const EdgeInsets.symmetric(horizontal: 13, vertical: 9),
            decoration: BoxDecoration(
              color: R.inkA(.04),
              border: Border.all(color: R.inkA(.09)),
              borderRadius: BorderRadius.circular(14),
            ),
            child: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(Icons.block_rounded, size: 14, color: R.inkA(.4)),
                const SizedBox(width: 6),
                Text('حُذفت هذه الرسالة',
                    style: T.kufi(12.5, FontWeight.w400, color: R.inkA(.5))),
              ],
            ),
          ),
        ),
      );
}
