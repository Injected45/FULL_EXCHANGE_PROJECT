import 'dart:io';

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
    this.onRetry,
  });

  // القائمة تُفتح بضغطةٍ مطوّلة على الفقاعة (اقتراح المالك، 6 سبتمبر 2026)،
  // ولا زرّ لها. جرّبتُ سهماً صغيراً في الزاوية فكان إمّا نشازاً بدائرته أو
  // هدفاً يُخطئه الإبهام بدونها — والضغطة المطوّلة هي ما اعتاده المستخدم
  // أصلاً في كل تطبيق محادثة، فلا شيء يحتاج أن يتعلّمه.

  /// إعادة إرسال رسالةٍ فشلت (البند 67).
  final VoidCallback? onRetry;

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

  /// خلفية الفقاعة الصادرة — **مسحةٌ فاتحة من لون الشركة، لا اللون نفسه**
  /// (قرار المالك، 6 سبتمبر 2026: «الأخضر الداكن صعب على العين»).
  ///
  /// ولّدتُها من `R.primary` لا لوناً ثابتاً، فتتبع هوية كل شركة تلقائياً:
  /// شركةٌ لونها أزرق تحصل على مسحةٍ زرقاء بالقدر نفسه من الخفّة.
  ///
  /// ⚠ **مصمَتة لا شفّافة** (`alphaBlend` فوق الأبيض): الفقاعات تقف على
  /// الخلفية المتدرّجة للتطبيق، ولونٌ شفّاف يلتقط ما تحته فيختلف من موضعٍ
  /// إلى آخر في الشاشة نفسها.
  /// 0.18 لا 0.15: أضعفَ من ذلك تبدو الفقاعة رماديةً لا ملوّنة، فلا يرى
  /// الوكيل أن ثيمه غيّر شيئاً — وهو ما طلب أن يراه (6 سبتمبر 2026).
  Color get _bg => mine
      ? Color.alphaBlend(R.primaryA(.18), Colors.white)
      : Colors.white;

  Color get _border => mine ? R.primaryA(.34) : R.inkA(.08);

  /// النصّ صار داكناً في الفقاعتين معاً بعد تفتيح الخلفية — أبيضُ على مسحةٍ
  /// فاتحة لا يُقرأ.
  Color get _fg => R.ink;

  /// الوقت والعلامات الثانوية.
  Color get _muted => R.inkA(.45);

  /// الساعة والعلامات: مُعدَّلة · محفوظة · مثبَّتة · حالة الإرسال.
  ///
  /// صفٌّ واحد يُستعمل في موضعين — داخل الفقرة حين يوجد نصّ، ومستقلّاً حين
  /// لا نصّ (صورةٌ أو صوتٌ وحدهما). ونسخُه مرّتين كان يعني أن إضافة علامةٍ
  /// جديدة تُنسى في أحدهما.
  Widget _meta() => Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Directionality(
            // الساعة رقمٌ — يُفرض اتجاهه وإلا انقلب داخل الفقرة العربية.
            textDirection: TextDirection.ltr,
            child: Text(
              // الساعة وحدها بلا تاريخ ولا ثوانٍ (قرار المالك، 5 سبتمبر
              // 2026): التاريخ في فاصل اليوم أعلى المجموعة.
              Fmt.hhmm(message.createdAt),
              style: T.plex(10, FontWeight.w400, color: _muted),
            ),
          ),
          // «تم التعديل» ظاهرة ولا تُخفى (البند 28).
          if (message.edited) ...[
            const SizedBox(width: 5),
            Text('مُعدَّلة',
                style: T.plex(9.5, FontWeight.w400, color: R.inkA(.4))),
          ],
          if (starred) ...[
            const SizedBox(width: 5),
            Icon(Icons.star_rounded, size: 12, color: R.warnIcon),
          ],
          if (message.pinned) ...[
            const SizedBox(width: 4),
            Icon(Icons.push_pin_rounded, size: 11, color: R.inkA(.45)),
          ],
          // الإيصال على رسائلي وحدها: علامةٌ على كلام الآخر تعني «قرأتُها
          // أنا»، وهي معلومةٌ لا يحتاجها.
          //
          // وقبله حالة الإرسال (البند 7): ساعةٌ صغيرة أثناء الإرسال، وعلامةُ
          // خطأ عند الفشل — والفقاعة تبقى معروضة فلا يضيع ما كتبه الوكيل.
          if (mine) ...[
            const SizedBox(width: 5),
            if (message.pending)
              Icon(Icons.schedule_rounded, size: 12, color: R.inkA(.4))
            else if (message.failed)
              Icon(Icons.error_outline_rounded, size: 13, color: R.error)
            else
              _Ticks(id: message.id, receipts: receipts),
          ],
        ],
      );

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
            color: _bg,
            border: Border.all(color: _border),
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
                    // الصورة من القرص ما دامت تُرفع: عرضُها فوراً هو ما يجعل
                    // الإرسال يبدو لحظياً — وانتظارُ رفعها ثم تنزيلها من
                    // الخادم لعرضها هو ذهابٌ وإياب بلا داعٍ.
                    child: message.localPath.isNotEmpty
                        ? Image.file(
                            File(message.localPath),
                            fit: BoxFit.cover,
                            height: 210,
                            width: double.infinity,
                            errorBuilder: (_, _, _) => const SizedBox(height: 210),
                          )
                        : Image.network(
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
              // النصّ والساعة في فقاعةٍ واحدة، والساعة في زاويتها السفلى
              // اليسرى (أمر المالك، 6 سبتمبر 2026: «كما في فقاعات واتساب»).
              //
              // الحيلة شاغلٌ **شفّاف** بمقاس الساعة في آخر النصّ: يحجز لها
              // موضعها في السطر الأخير فلا يمرّ الكلام تحتها، ولا يُرسم —
              // ثم تُرسم الساعة الحقيقية في زاوية الفقاعة بـ`Positioned`.
              //
              // والمحاولة السابقة وضعتها `WidgetSpan` مرئياً، فسالت ملتصقةً
              // بآخر كلمة أينما وقعت — وهو ما شكا منه المالك.
              //
              // و`bottom: -1` تُنزلها درجةً تحت خطّ الكلام: مساواتُها للسطر
              // تجعلها تُقرأ جزءاً من الجملة لا طابعاً عليها.
              if (message.body.isNotEmpty)
                Padding(
                  padding: EdgeInsets.symmetric(
                      horizontal: message.isImage ? 10 : 0),
                  child: Stack(
                    clipBehavior: Clip.none,
                    children: [
                      Text.rich(
                        TextSpan(children: [
                          TextSpan(text: message.body),
                          WidgetSpan(
                            alignment: PlaceholderAlignment.middle,
                            child: Opacity(
                              opacity: 0,
                              child: Padding(
                                padding: const EdgeInsets.only(right: 12),
                                child: _meta(),
                              ),
                            ),
                          ),
                        ]),
                        style: T.kufi(14.5, FontWeight.w500,
                            height: 1.5, color: _fg),
                      ),
                      PositionedDirectional(
                        bottom: -1,
                        end: 0,
                        child: _meta(),
                      ),
                    ],
                  ),
                ),

              // بلا نصّ (صورة أو صوت وحدهما): الساعة في صفٍّ مستقلّ، فلا
              // فقرة تسيل فيها.
              if (message.body.isEmpty) ...[
                const SizedBox(height: 5),
                Padding(
                  padding:
                      EdgeInsets.symmetric(horizontal: message.isImage ? 10 : 0),
                  child: Align(
                    alignment: AlignmentDirectional.centerEnd,
                    child: _meta(),
                  ),
                ),
              ],

              // «فشل الإرسال · إعادة المحاولة» (البند 67) — داخل الفقاعة
              // وتحت النصّ، فيبقى ما كتبه الوكيل معروضاً ولا يُعاد كتابته.
              if (message.failed && onRetry != null) ...[
                const SizedBox(height: 6),
                InkWell(
                  onTap: onRetry,
                  borderRadius: BorderRadius.circular(99),
                  child: Container(
                    padding:
                        const EdgeInsets.symmetric(horizontal: 9, vertical: 4),
                    decoration: BoxDecoration(
                      color: R.error.withValues(alpha: .10),
                      borderRadius: BorderRadius.circular(99),
                    ),
                    child: Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Icon(Icons.refresh_rounded, size: 13, color: R.error),
                        const SizedBox(width: 5),
                        Text('فشل الإرسال · إعادة المحاولة',
                            style: T.plex(10.5, FontWeight.w600,
                                color: R.error)),
                      ],
                    ),
                  ),
                ),
              ],

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
            color: R.inkA(.05),
            border: Border.all(
              color: mine
                  ? R.primary
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
                          color: R.inkA(.6))),
                ),
              ],
            ],
          ),
        ),
      );
}

/// حالات التسليم الثلاث كما يعرفها المستخدم (قرار المالك، 6 سبتمبر 2026):
///
/// | العلامة | المعنى |
/// |---|---|
/// | ✓ واحدة، باهتة | وصلت الخادم، وجهاز الطرف الآخر لم يسحبها بعد — غالباً غير متّصل |
/// | ✓✓ باهتة | بلغت جهازه ولم يفتحها |
/// | ✓✓ زرقاء ساطعة | فتح المحادثة وقرأها |
///
/// واللون الباهت هنا **أبيض شفّاف لا رمادي**: الفقاعة الصادرة خضراء داكنة،
/// والرمادي عليها لا يُرى أصلاً. والأزرق فاتحٌ للسبب نفسه — الأزرق القياسي
/// يغرق في الأخضر.
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
      // الزرقاء أكبر قليلاً: الفارق بين شرطتين باهتتين وشرطتين زرقاوين هو
      // أهمّ فارقٍ في المحادثة، ولونٌ وحده على أيقونةٍ بحجم 14 لا يكفي
      // لعينٍ تمرّ سريعاً.
      size: read ? 15 : 14,
      color: read ? const Color(0xFF1E9BD7) : R.inkA(.38),
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
          color: R.inkA(.05),
          // شريطٌ في الجانب المبدوء به — علامة الاقتباس المعروفة.
          border: BorderDirectional(
            start: BorderSide(
                color: R.primary, width: 3),
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
                      color: R.primaryDark)),
            Text(
              message.replyPreview.isEmpty
                  ? 'رسالة محذوفة'
                  : message.replyPreview,
              maxLines: 2,
              overflow: TextOverflow.ellipsis,
              style: T.kufi(12, FontWeight.w400,
                  color: R.inkA(.6)),
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
        color: R.inkA(.05),
        borderRadius: BorderRadius.circular(10),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(Icons.insert_drive_file_outlined,
              size: 18, color: R.primary),
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
                        color: R.ink)),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text('$kb KB',
                      style: T.plex(10, FontWeight.w400,
                          color: R.inkA(.45))),
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
