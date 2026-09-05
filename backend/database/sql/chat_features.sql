/* ============================================================================
   المحادثات — بقيّة مزايا برومبت المالك (109 بنود، 5 سبتمبر 2026).

   ⚠ طبقة تواصل لا طبقة مالية (البنود 103–105): لا رصيد ولا قيد ولا حوالة،
   ولا مفتاح أجنبي إلى جدولٍ من جداول المنظومة.

   ── ما يُضاف ولماذا ───────────────────────────────────────────────────────

   `chat_reactions`         (25) تفاعلٌ لكل شخصٍ على كل رسالة. جدولٌ لا عمود:
                                 لرسالةٍ واحدة عدّة متفاعلين، وعمودٌ واحد
                                 يجعل الثاني يمحو الأول.

   `chat_settings`          (30–34, 59) إعدادات المشارك في محادثةٍ بعينها —
                                 كتم، تثبيت، أرشفة، قفل. صفٌّ لكل
                                 (محادثة، مشارك): الكتم رأيُ صاحبه لا صفةُ
                                 المحادثة، وكتمُ أحدهم لا يُسكتها عند الآخر.

   `chat_stars`             (30) رسائل حفظها هذا المشارك. جدولٌ منفصل لأن
                                 «مهمّة» تخصّ القارئ لا الرسالة.

   `chat_typing`            (16–17) «يكتب الآن» و«يسجّل رسالة صوتية».
                                 صفٌّ يُكتب فوقه ولا يُراكم — الحالة لحظية
                                 ولا تُحفظ في سجلّ الرسائل (نصّ البند 16).

   على `chat_messages`:
   `edited_at`              (28) تُعرض «تم التعديل» ولا يُخفى أن الرسالة عُدّلت.
   `pinned_at` `pinned_by`  (31) تثبيت رسالة داخل المحادثة.
   `client_id`              (68) مُعرّف من الجهاز يمنع التكرار عند إعادة
                                 المحاولة وضعف الشبكة. فريدٌ داخل المحادثة.
   ============================================================================ */

/* ---------------------------------------------------------------------------
   أعمدة الرسالة
   --------------------------------------------------------------------------- */
IF COL_LENGTH('dbo.chat_messages', 'edited_at') IS NULL
BEGIN
    ALTER TABLE dbo.chat_messages
        ADD edited_at  DATETIME2   NULL,
            pinned_at  DATETIME2   NULL,
            pinned_by  VARCHAR(20) NULL,   -- نوع المُثبِّت، للعرض
            client_id  VARCHAR(64) NULL;
END;
GO

/* منع التكرار (68): مُعرّف الجهاز فريدٌ داخل المحادثة الواحدة.
   مُرشَّح لأن الرسائل القديمة بلا `client_id`، و NULL لا يساوي NULL في
   SQL Server فقيدٌ عادي كان سيمرّ — لكنّ المُرشَّح أوضح في القصد. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_msg_client'
               AND object_id = OBJECT_ID('dbo.chat_messages'))
    CREATE UNIQUE INDEX UQ_chat_msg_client
        ON dbo.chat_messages (thread_id, client_id)
        WHERE client_id IS NOT NULL;
GO

/* البحث داخل المحادثة (35). فهرسٌ على المحادثة والتاريخ — والنصّ يُرشَّح
   بـ LIKE فوقه. لا فهرس نصّي كامل: صفحةٌ واحدة من نتائج البحث لا تبرّره،
   وهو يضاعف حجم الجدول. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_chat_msg_search'
               AND object_id = OBJECT_ID('dbo.chat_messages'))
    CREATE INDEX IX_chat_msg_search
        ON dbo.chat_messages (thread_id, deleted_at, id DESC);
GO

/* ---------------------------------------------------------------------------
   التفاعلات (25)
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.chat_reactions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.chat_reactions (
        id           BIGINT IDENTITY(1,1) NOT NULL
                     CONSTRAINT PK_chat_reactions PRIMARY KEY,
        message_id   BIGINT NOT NULL
                     CONSTRAINT FK_chat_react_msg REFERENCES dbo.chat_messages(id),
        thread_id    BIGINT NOT NULL,

        actor_kind   VARCHAR(20) NOT NULL,
        actor_id     BIGINT NOT NULL,

        /* الرمز نفسه لا رقمه: قائمةٌ ثابتة من ستّة رموز كانت ستمنع البند 25
           («مع إمكانية الوصول إلى بقية Emoji»). و16 محرفاً تكفي أطول
           تسلسل ZWJ في الاستعمال العملي. */
        emoji        NVARCHAR(16) NOT NULL,

        created_at   DATETIME2 NOT NULL
                     CONSTRAINT DF_chat_react_created DEFAULT (sysutcdatetime())
    );
END;
GO

/* تفاعلٌ واحد لكل شخص على كل رسالة — الضغط على رمزٍ آخر يستبدل لا يضيف. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_react_actor'
               AND object_id = OBJECT_ID('dbo.chat_reactions'))
    CREATE UNIQUE INDEX UQ_chat_react_actor
        ON dbo.chat_reactions (message_id, actor_kind, actor_id);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_chat_react_thread'
               AND object_id = OBJECT_ID('dbo.chat_reactions'))
    CREATE INDEX IX_chat_react_thread ON dbo.chat_reactions (thread_id, message_id);
GO

/* ---------------------------------------------------------------------------
   إعدادات المشارك في محادثة (30–34, 59)
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.chat_settings', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.chat_settings (
        id            BIGINT IDENTITY(1,1) NOT NULL
                      CONSTRAINT PK_chat_settings PRIMARY KEY,
        thread_id     BIGINT NOT NULL
                      CONSTRAINT FK_chat_set_thread REFERENCES dbo.chat_threads(id),
        actor_kind    VARCHAR(20) NOT NULL,
        actor_id      BIGINT NOT NULL,

        /* الكتم إلى وقتٍ بعينه لا رايةٌ ثنائية: البند 34 يطلب مدداً
           (ساعة · 8 ساعات · يوم · دائماً)، و«دائماً» تاريخٌ بعيد. */
        muted_until   DATETIME2 NULL,

        pinned_at     DATETIME2 NULL,
        archived_at   DATETIME2 NULL,

        /* قفل المحادثة (59) — قرارُ صاحبه، والقفل نفسه يقع على الجهاز
           ببصمةٍ أو وجه. الخادم يحفظ الرغبة لا وسيلة الفتح. */
        locked        BIT NOT NULL CONSTRAINT DF_chat_set_locked DEFAULT (0),

        /* «تحديد كغير مقروءة» (10): يعلو على `chat_reads` في العرض وحده. */
        forced_unread BIT NOT NULL CONSTRAINT DF_chat_set_unread DEFAULT (0),

        updated_at    DATETIME2 NOT NULL
                      CONSTRAINT DF_chat_set_updated DEFAULT (sysutcdatetime())
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_settings_actor'
               AND object_id = OBJECT_ID('dbo.chat_settings'))
    CREATE UNIQUE INDEX UQ_chat_settings_actor
        ON dbo.chat_settings (thread_id, actor_kind, actor_id);
GO

/* ---------------------------------------------------------------------------
   الرسائل المهمّة (30)
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.chat_stars', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.chat_stars (
        id          BIGINT IDENTITY(1,1) NOT NULL
                    CONSTRAINT PK_chat_stars PRIMARY KEY,
        message_id  BIGINT NOT NULL
                    CONSTRAINT FK_chat_star_msg REFERENCES dbo.chat_messages(id),
        thread_id   BIGINT NOT NULL,
        actor_kind  VARCHAR(20) NOT NULL,
        actor_id    BIGINT NOT NULL,
        created_at  DATETIME2 NOT NULL
                    CONSTRAINT DF_chat_star_created DEFAULT (sysutcdatetime())
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_star_actor'
               AND object_id = OBJECT_ID('dbo.chat_stars'))
    CREATE UNIQUE INDEX UQ_chat_star_actor
        ON dbo.chat_stars (message_id, actor_kind, actor_id);
GO

/* ---------------------------------------------------------------------------
   «يكتب الآن» و«يسجّل» (16–17)
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.chat_typing', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.chat_typing (
        id          BIGINT IDENTITY(1,1) NOT NULL
                    CONSTRAINT PK_chat_typing PRIMARY KEY,
        thread_id   BIGINT NOT NULL
                    CONSTRAINT FK_chat_typing_thread REFERENCES dbo.chat_threads(id),
        actor_kind  VARCHAR(20) NOT NULL,
        actor_id    BIGINT NOT NULL,
        actor_name  NVARCHAR(200) NULL,

        /* TYPING | RECORDING */
        state       VARCHAR(20) NOT NULL,

        /* تنتهي من تلقائها: من أغلق التطبيق وهو يكتب لا يبقى «يكتب الآن»
           إلى الأبد. القراءة ترشّح على هذا العمود. */
        expires_at  DATETIME2 NOT NULL
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_typing_actor'
               AND object_id = OBJECT_ID('dbo.chat_typing'))
    CREATE UNIQUE INDEX UQ_chat_typing_actor
        ON dbo.chat_typing (thread_id, actor_kind, actor_id);
GO
