/* ============================================================================
   مركز الدعم — الدفعة الأولى من التطوير التشغيلي
   (بنود المالك 3 · 4 · 7 · 12 · 14 · 17، 6 سبتمبر 2026)

   ⚠ **تطويرٌ فوق القائم لا بناءٌ جديد.** لا جدولَ يُكرّر وظيفةَ جدولٍ
   موجود، ولا منطقَ يُعاد. `support_thread_state` تُوسَّع بأعمدة، ولا
   تُستبدل؛ و`support_audit` تبقى كما هي ويُضاف إليها نوعُ حدثٍ لا جدولٌ
   ثانٍ للسجلّ.

   ⚠ ولا شيء هنا يمسّ المال: صفرُ مفاتيحَ إلى جدولٍ ماليّ، وصفرُ محفّزات.
   المفاتيح كلُّها إلى `support_staff` و`chat_threads` و`chat_messages`.

   ── ما يُضاف ولماذا ───────────────────────────────────────────────────────

   الأولوية والتصنيف والرقم المرجعي أعمدةٌ على `support_thread_state` لا
   جداولُ مستقلّة: كلُّها **صفةٌ واحدة للمحادثة الواحدة**، وجدولٌ لكلٍّ منها
   يعني ثلاثَ وصلاتٍ في كل استعلامِ قائمة — على قاعدةٍ بعيدة كلُّ رحلةٍ
   إليها ~45 مللي.

   `support_tags` جدولٌ لأن الوسم **كثيرٌ لكثير**: محادثةٌ تحمل وسمين،
   ووسمٌ يقع على مئة محادثة.

   `support_events` هو الـ Timeline (بند 17): سجلٌّ **تشغيليّ** لما جرى
   للمحادثة — لا نسخةٌ من الرسائل. منفصلٌ عن `support_audit` لأن سؤالَيهما
   مختلفان: السجلُّ يسأل «من فعل ماذا في النظام؟» ويُقرأ من شاشة الإدارة،
   والأحداثُ تسأل «ماذا جرى لهذه المحادثة؟» وتُقرأ داخلها. ودمجُهما يعني
   مسحَ سجلِّ النظام كلِّه لرسم شريطِ محادثةٍ واحدة.

   الملاحظة الداخلية (بند 7) **رسالةٌ في `chat_messages` بعلامة** لا جدولٌ
   ثانٍ للرسائل: تحتاج المرفقات والاقتباس والتعديل والبحث — وكلُّها مبنيّةٌ
   هناك. وعمودٌ واحد `is_internal` يجعلها لا تصل الوكيل، وهو أضمنُ من
   جدولٍ موازٍ قد يُنسى استثناؤه في استعلامٍ واحد.
   ============================================================================ */

/* ---------------------------------------------------------------------------
   1) أعمدة التشغيل على حالة المحادثة
   --------------------------------------------------------------------------- */
IF COL_LENGTH('dbo.support_thread_state', 'priority') IS NULL
BEGIN
    ALTER TABLE dbo.support_thread_state ADD
        -- NORMAL | HIGH | URGENT | CRITICAL — انظر SupportPriority.
        priority        VARCHAR(10)   NOT NULL
                        CONSTRAINT DF_sup_state_prio DEFAULT ('NORMAL'),
        priority_at     DATETIME2     NULL,
        priority_by     BIGINT        NULL,

        -- تصنيفٌ واحد للمحادثة (البند 4). الوسومُ كثيرةٌ وتُوضع في
        -- `support_tags`؛ والتصنيفُ واحدٌ يُجيب «ما نوع هذه المشكلة؟».
        category_id     BIGINT        NULL,

        -- الرقم المرجعي (البند 12).
        --
        -- ⚠ لا يُشتقّ من `thread_id` ولا من تسلسلٍ ظاهر: البرومبت يشترط
        -- ألّا يكون «قابلاً للتنبؤ بطريقة تضعف الأمان». يُولَّد من محارف
        -- عشوائية في PHP — انظر `SupportReference`.
        reference       VARCHAR(16)   NULL,

        -- التأجيل والمتابعة (البندان 15 و16) — تُملأ في الدفعة الثالثة،
        -- وتُضاف الآن كي لا يُعدَّل الجدول مرّتين.
        snoozed_until   DATETIME2     NULL,
        snoozed_by      BIGINT        NULL,
        snooze_reason   NVARCHAR(300) NULL,

        -- زمنُ الاستجابة (البند 2) — يُقاس في الدفعة الثانية.
        first_agent_msg_at   DATETIME2 NULL,  -- أوّل رسالة وكيلٍ بلا ردّ
        first_reply_at       DATETIME2 NULL,  -- أوّل ردٍّ من الدعم عليها
        last_agent_msg_at    DATETIME2 NULL,
        last_reply_at        DATETIME2 NULL,
        resolved_at          DATETIME2 NULL;
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_sup_state_priority')
    ALTER TABLE dbo.support_thread_state ADD CONSTRAINT CK_sup_state_priority
        CHECK (priority IN ('NORMAL', 'HIGH', 'URGENT', 'CRITICAL'));
GO

/* الرقم المرجعي فريدٌ بين الموجودين — و NULL مسموحٌ للمحادثات القديمة. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_sup_state_reference'
               AND object_id = OBJECT_ID('dbo.support_thread_state'))
    CREATE UNIQUE INDEX UQ_sup_state_reference
        ON dbo.support_thread_state (reference)
        WHERE reference IS NOT NULL;
GO

/* الفرزُ بالأولوية ثم بالأقدم — وهو ترتيبُ صندوق الوارد المقصود. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sup_state_priority'
               AND object_id = OBJECT_ID('dbo.support_thread_state'))
    CREATE INDEX IX_sup_state_priority
        ON dbo.support_thread_state (priority, status);
GO

/* المؤجَّلة التي حان وقتُها — يُسألُ عنها كلَّ دقيقة، ففهرسٌ عليها. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sup_state_snooze'
               AND object_id = OBJECT_ID('dbo.support_thread_state'))
    CREATE INDEX IX_sup_state_snooze
        ON dbo.support_thread_state (snoozed_until)
        WHERE snoozed_until IS NOT NULL;
GO

/* ---------------------------------------------------------------------------
   2) التصنيفات (البند 4)

   جدولٌ لا ثوابتُ في الشيفرة: نصُّ البند «قابلة للتعديل والإضافة من
   الإدارة بدون تعديل الكود كل مرة». وهو القرارُ نفسه المتّخذ في
   `BrandingThemes` ولسببه: تصنيفٌ جديد لا ينبغي أن ينتظر إصداراً.
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_categories', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_categories (
        id         BIGINT IDENTITY(1,1) NOT NULL,
        name       NVARCHAR(80)  NOT NULL,
        -- لونٌ للعرض — سداسيّ، ويُتحقَّق منه في الخادم.
        color      VARCHAR(9)    NULL,
        sort_order INT           NOT NULL CONSTRAINT DF_sup_cat_sort DEFAULT (100),
        is_active  BIT           NOT NULL CONSTRAINT DF_sup_cat_active DEFAULT (1),
        created_at DATETIME2     NOT NULL CONSTRAINT DF_sup_cat_at DEFAULT (SYSDATETIME()),
        created_by BIGINT        NULL,
        CONSTRAINT PK_support_categories PRIMARY KEY (id)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_sup_cat_name'
               AND object_id = OBJECT_ID('dbo.support_categories'))
    CREATE UNIQUE INDEX UQ_sup_cat_name ON dbo.support_categories (name);
GO

/* ---------------------------------------------------------------------------
   3) الوسوم — كثيرٌ لكثير (البند 4)
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_tag_defs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_tag_defs (
        id         BIGINT IDENTITY(1,1) NOT NULL,
        name       NVARCHAR(60) NOT NULL,
        color      VARCHAR(9)   NULL,
        is_active  BIT          NOT NULL CONSTRAINT DF_sup_tagdef_active DEFAULT (1),
        created_at DATETIME2    NOT NULL CONSTRAINT DF_sup_tagdef_at DEFAULT (SYSDATETIME()),
        created_by BIGINT       NULL,
        CONSTRAINT PK_support_tag_defs PRIMARY KEY (id)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_sup_tagdef_name'
               AND object_id = OBJECT_ID('dbo.support_tag_defs'))
    CREATE UNIQUE INDEX UQ_sup_tagdef_name ON dbo.support_tag_defs (name);
GO

IF OBJECT_ID('dbo.support_thread_tags', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_thread_tags (
        thread_id  BIGINT    NOT NULL,
        tag_id     BIGINT    NOT NULL,
        added_at   DATETIME2 NOT NULL CONSTRAINT DF_sup_ttag_at DEFAULT (SYSDATETIME()),
        added_by   BIGINT    NULL,
        CONSTRAINT PK_support_thread_tags PRIMARY KEY (thread_id, tag_id),
        CONSTRAINT FK_sup_ttag_thread FOREIGN KEY (thread_id)
            REFERENCES dbo.chat_threads (id),
        CONSTRAINT FK_sup_ttag_tag FOREIGN KEY (tag_id)
            REFERENCES dbo.support_tag_defs (id)
    );
END;
GO

/* «أيُّ المحادثات تحمل هذا الوسم؟» — سؤالُ تقرير أسباب المشاكل (البند 27). */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sup_ttag_tag'
               AND object_id = OBJECT_ID('dbo.support_thread_tags'))
    CREATE INDEX IX_sup_ttag_tag ON dbo.support_thread_tags (tag_id);
GO

/* ---------------------------------------------------------------------------
   4) الشريط الزمني للمحادثة (البند 17)

   ⚠ **Metadata تشغيليّة فقط.** لا يُكتب هنا نصُّ رسالة ولا مرفق — الحدث
   يقول «تغيّرت الأولوية» لا «قال الوكيل كذا». وهو شرطُ البند نفسِه.

   ولا يُغني عنه `support_audit`: ذاك يُقرأ من شاشة الإدارة بسؤال «من فعل
   ماذا في النظام؟» وهذا يُقرأ **داخل المحادثة** بسؤال «ماذا جرى لها؟».
   ودمجُهما يعني مسحَ سجلِّ النظام كلِّه لرسم شريطِ محادثةٍ واحدة.
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_events', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_events (
        id         BIGINT IDENTITY(1,1) NOT NULL,
        thread_id  BIGINT        NOT NULL,
        -- OPENED | ASSIGNED | UNASSIGNED | STATUS | PRIORITY | CATEGORY
        -- | TAG_ADD | TAG_REMOVE | ESCALATED | SNOOZED | UNSNOOZED
        -- | NOTE | REOPENED | CLOSED | HANDOFF
        kind       VARCHAR(20)   NOT NULL,
        -- من فعل: رقمُ موظّف الدعم، أو NULL إن كان الفاعلَ النظامُ نفسُه
        -- (انتهاءُ تأجيل، إعادةُ فتحٍ برسالة وكيل).
        actor_id   BIGINT        NULL,
        actor_name NVARCHAR(120) NULL,
        -- من ⇦ إلى، نصّاً: الحالةُ والأولويةُ والاسمُ كلُّها نصوصٌ قصيرة،
        -- وعمودان يكفيان الثلاثةَ بلا جدولٍ لكلٍّ منها.
        from_value NVARCHAR(120) NULL,
        to_value   NVARCHAR(120) NULL,
        note       NVARCHAR(500) NULL,
        created_at DATETIME2     NOT NULL CONSTRAINT DF_sup_ev_at DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_support_events PRIMARY KEY (id),
        CONSTRAINT FK_sup_ev_thread FOREIGN KEY (thread_id)
            REFERENCES dbo.chat_threads (id)
    );
END;
GO

/* الشريط يُقرأ لمحادثةٍ واحدة بالأحدث — وهو الاستعلام الوحيد المتكرّر. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sup_ev_thread'
               AND object_id = OBJECT_ID('dbo.support_events'))
    CREATE INDEX IX_sup_ev_thread ON dbo.support_events (thread_id, id DESC);
GO

/* ---------------------------------------------------------------------------
   5) الملاحظة الداخلية (البند 7)

   ⚠ **عمودٌ على `chat_messages` لا جدولُ رسائلَ ثانٍ.**

   الملاحظة تحتاج ما تحتاجه الرسالة: مرفقاتٍ واقتباساً وتعديلاً وبحثاً —
   وكلُّها مبنيّةٌ في `chat_messages` و`ChatService`. وجدولٌ موازٍ يعني
   إعادةَ بناء ذلك كلِّه، وهو ما يمنعه البند 47 صراحةً.

   ⚠ والأخطرُ أن الاستثناء يجب أن يكون **في مكانٍ واحد**: عمودٌ واحد
   يُرشَّح في `ChatService::messages` يضمن ألّا تصل الوكيلَ ملاحظةٌ أبداً.
   وجدولٌ منفصل يعني استعلاماً قد يُنسى استثناؤه مرّةً — ومرّةٌ واحدة تكفي
   لتصل ملاحظةُ «يحتاج تصعيداً» إلى الوكيل نفسِه.
   --------------------------------------------------------------------------- */
IF COL_LENGTH('dbo.chat_messages', 'is_internal') IS NULL
BEGIN
    ALTER TABLE dbo.chat_messages
        ADD is_internal BIT NOT NULL CONSTRAINT DF_chat_msg_internal DEFAULT (0);
END;
GO

/* الفهرسُ القائم `IX_chat_messages_thread` يكفي: الترشيح على عمودٍ بتّيّ
   بعد تحديد المحادثة رخيصٌ، وفهرسٌ ثانٍ على جدولٍ يكبر لا يبرّره ذلك. */

/* ---------------------------------------------------------------------------
   6) بذرة التصنيفات — أمثلةُ البند 4 نفسُها

   تُزرع مرّةً ولا تُفرض: الإدارة تعدّلها وتضيف وتُعطّل. و`NOT EXISTS`
   تجعل إعادةَ التشغيل بلا أثر، ولا تُعيد ما حذفته الإدارة عمداً… بل
   تُعيده، ولذلك الشرطُ على وجود **أي** صفٍّ لا على كل اسم: جدولٌ فيه
   تصنيفاتٌ يعني أن الإدارة تولّته.
   --------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM dbo.support_categories)
BEGIN
    INSERT INTO dbo.support_categories (name, color, sort_order) VALUES
        (N'تسجيل الدخول',  '#1E9BD7', 10),
        (N'التفعيل',        '#0F5F4E', 20),
        (N'الموظفون',       '#7C5CBF', 30),
        (N'الصلاحيات',      '#D98324', 40),
        (N'الحوالات',       '#1F8A5F', 50),
        (N'التقارير',       '#5A7D9A', 60),
        (N'الإشعارات',      '#C08A2B', 70),
        (N'مشكلة تقنية',    '#C0392B', 80),
        (N'اقتراح',         '#2E8B57', 90),
        (N'استفسار عام',    '#6B7280', 100);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.support_tag_defs)
BEGIN
    INSERT INTO dbo.support_tag_defs (name, color) VALUES
        (N'يحتاج متابعة',    '#D98324'),
        (N'تمّ التواصل هاتفياً', '#1E9BD7'),
        (N'عطل عام',         '#C0392B'),
        (N'حُلّت من أول تواصل', '#1F8A5F'),
        (N'تحتاج تصعيداً',    '#7C5CBF');
END;
GO
