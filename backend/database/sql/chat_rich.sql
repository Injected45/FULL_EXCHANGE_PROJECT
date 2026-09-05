/* ============================================================================
   الدردشة — مزايا الرسالة: مرفق، ردٌّ على رسالة، حذف، وإيصال قراءة.

   أمر المالك (5 سبتمبر 2026): «كل شيء وكأنه يستخدم واتساب».

   ⚠ طبقة تواصل لا طبقة مالية — كسابقتها. لا رصيد ولا قيد ولا حوالة، ولا
   مفتاح أجنبي إلى جدولٍ من جداول المنظومة.

   ── ما يُضاف ولماذا ───────────────────────────────────────────────────────

   `reply_to_id`  — الردّ على رسالة بعينها. عمودٌ يشير إلى `chat_messages`
                    نفسه، لا نسخةٌ من النصّ المقتبَس: نسخُ النصّ يعني أن
                    تصحيح رسالةٍ لا يُصحّح ما اقتُبس منها، فيقرأ الطرفان
                    نصّين مختلفين لرسالةٍ واحدة.

   `deleted_at`   — حذفٌ ناعم. الرسالة تبقى في الجدول ويُخفى نصّها، تماماً
                    كما تفعل تطبيقات المحادثة: «حُذفت هذه الرسالة» يقول
                    للطرف الآخر أن شيئاً كان هنا. والحذف الصلب كان يجعل
                    الردودَ عليها تشير إلى العدم.

   `attachment_*` — المرفق. اسمُ الملف عشوائي على قرصٍ خاصّ لا عامّ،
                    و`attachment_name` هو الاسم الأصلي للعرض والتنزيل.
                    والحجم والنوع محفوظان ليُعرضا قبل التحميل.

   `delivered_at` على `chat_reads` — «وصلت» غير «قُرئت»، وهما العلامتان
                    اللتان يعرفهما المستخدم: رماديّتان ثم زرقاوان.
   ============================================================================ */

IF COL_LENGTH('dbo.chat_messages', 'reply_to_id') IS NULL
BEGIN
    ALTER TABLE dbo.chat_messages
        ADD reply_to_id       BIGINT        NULL,
            deleted_at        DATETIME2     NULL,
            attachment_path   VARCHAR(200)  NULL,   -- الاسم على القرص
            attachment_name   NVARCHAR(255) NULL,   -- الاسم الأصلي للعرض
            attachment_mime   VARCHAR(120)  NULL,
            attachment_size   INT           NULL,
            attachment_kind   VARCHAR(20)   NULL;   -- IMAGE | AUDIO | FILE
END;
GO

/* الردّ يشير إلى رسالةٍ في الجدول نفسه.
   بلا ON DELETE: الحذف ناعمٌ أصلاً، فلا صفّ يُحذف حتى يُشلّع ما يشير إليه. */
IF OBJECT_ID('dbo.FK_chat_msg_reply', 'F') IS NULL
    ALTER TABLE dbo.chat_messages
        ADD CONSTRAINT FK_chat_msg_reply
        FOREIGN KEY (reply_to_id) REFERENCES dbo.chat_messages(id);
GO

/* `body` صار يحتمل الفراغ: رسالةٌ صورةٍ بلا تعليق.
   الحارس انتقل إلى الخدمة — نصٌّ فارغ **وبلا مرفق** هو المرفوض. */
IF EXISTS (SELECT 1 FROM sys.columns
           WHERE object_id = OBJECT_ID('dbo.chat_messages')
             AND name = 'body' AND is_nullable = 0)
    ALTER TABLE dbo.chat_messages ALTER COLUMN body NVARCHAR(2000) NULL;
GO

/* «وصلت» إلى جانب «قُرئت». */
IF COL_LENGTH('dbo.chat_reads', 'last_delivered_message_id') IS NULL
BEGIN
    ALTER TABLE dbo.chat_reads
        ADD last_delivered_message_id BIGINT NOT NULL
            CONSTRAINT DF_chat_reads_delivered DEFAULT (0);
END;
GO
