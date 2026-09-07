/* ============================================================================
   مركز الدعم — الدفعة الثالثة
   (بنود المالك 8 الردود الجاهزة · 13 التصعيد · 15 التأجيل · 16 المتابعة
    · 21 و37 المسودّات · 34 التسليم بين الموظّفين)

   ⚠ تطويرٌ فوق القائم: أعمدةُ التأجيل أُضيفت في الدفعة الأولى وتُستعمل
   هنا، والتسليمُ يستعمل الإسنادَ والملاحظةَ الداخلية القائمَين ولا يبني
   لهما نظيراً.

   ⚠ ولا شيء يمسّ المال: قوالبُ نصّ، وتذكيراتٌ، ومسودّاتٌ، وتواريخُ عودة.

   ── أربعة قرارات ─────────────────────────────────────────────────────────

   1. **المسودّة صفٌّ لكل (محادثة، موظّف) لا لكل محادثة.** موظّفان على
      محادثةٍ واحدة يكتب كلٌّ منهما نصفَ ردّ، وصفٌّ مشترك يجعل الثاني
      يمحو ما كتبه الأوّل وهو لا يدري.

   2. **الردُّ الجاهز إمّا مشترَكٌ للفريق أو خاصٌّ بصاحبه** — يميّزهما
      `owner_staff_id`. وجدولٌ واحد لا جدولان: الفرقُ بينهما عمودٌ لا
      طبيعةٌ مختلفة، وجدولان يعنيان استعلامين في كل فتحةِ قائمة.

   3. **المتابعةُ تذكيرٌ شخصيّ، والتأجيلُ حالةٌ للمحادثة.** الأوّل يخصّ من
      كتبه وحده ولا يغيّر شيئاً للفريق؛ والثاني يُخفي المحادثة عن
      الجميع حتّى موعدها. الخلطُ بينهما يجعل تذكيرَ موظّفٍ يُخفي محادثةً
      عن زملائه.

   4. **لا شيء هنا يُحذف.** الردُّ الجاهز يُعطَّل (`is_active`)،
      والمتابعةُ تُختَم (`done_at`) — فسجلُّ من ذكّر نفسَه بماذا ومتى
      يبقى، وقائمةُ الردود لا تفقد ما استُعمل مئةَ مرّة لأن أحداً رآه
      قديماً.
   ============================================================================ */

/* ---------------------------------------------------------------------------
   1) الردود الجاهزة (البند 8)
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_saved_replies', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_saved_replies (
        id              BIGINT IDENTITY(1,1) NOT NULL,
        title           NVARCHAR(120)  NOT NULL,
        body            NVARCHAR(MAX)  NOT NULL,
        -- NULL ⇐ مشترَكٌ للفريق. وغيرُه ⇐ خاصٌّ بصاحبه لا يراه أحدٌ سواه.
        owner_staff_id  BIGINT         NULL,
        -- تصنيفٌ اختياريّ يجمع القوالبَ المتشابهة في قائمةٍ طويلة.
        category_id     BIGINT         NULL,
        -- عدّادُ الاستعمال: يرفع الأكثرَ استعمالاً إلى أعلى القائمة، فلا
        -- يبحث الموظّف في عشرين قالباً عن الثلاثة التي يكتبها كلَّ يوم.
        uses            INT            NOT NULL
                        CONSTRAINT DF_sup_reply_uses DEFAULT (0),
        is_active       BIT            NOT NULL
                        CONSTRAINT DF_sup_reply_active DEFAULT (1),
        created_by      BIGINT         NULL,
        created_at      DATETIME2      NOT NULL
                        CONSTRAINT DF_sup_reply_created DEFAULT (SYSDATETIME()),
        updated_at      DATETIME2      NULL,
        CONSTRAINT PK_support_saved_replies PRIMARY KEY (id),
        CONSTRAINT FK_sup_reply_owner FOREIGN KEY (owner_staff_id)
            REFERENCES dbo.support_staff (id),
        CONSTRAINT FK_sup_reply_cat FOREIGN KEY (category_id)
            REFERENCES dbo.support_categories (id)
    );
END;
GO

/* القائمةُ تُقرأ بـ(المالك، السريان) في كل فتحةٍ للمنتقي. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sup_reply_owner'
               AND object_id = OBJECT_ID('dbo.support_saved_replies'))
    CREATE INDEX IX_sup_reply_owner ON dbo.support_saved_replies (owner_staff_id, is_active);
GO

/*
   قوالبُ أولى — تُزرع مرّةً وتُعدَّل من الشاشة.

   وهي مكتوبةٌ بصيغة الدعم لا بصيغة النظام: «سنراجع الحوالة» لا «تمّت
   معالجة الطلب». والموظّف يعدّلها قبل الإرسال دائماً — القالبُ يوفّر
   الكتابة لا يستبدل القراءة.

   والحقولُ بين قوسين معقوفين تُملأ من بيانات المحادثة عند الإدراج.
*/
IF NOT EXISTS (SELECT 1 FROM dbo.support_saved_replies)
BEGIN
    INSERT INTO dbo.support_saved_replies (title, body, owner_staff_id) VALUES
        (N'ترحيب',
         N'أهلاً {الوكيل}، معك فريق الرحالة للدعم. تفضّل كيف نساعدك؟', NULL),
        (N'طلب رقم الحوالة',
         N'حتى نتابع معك، أرسل لنا رقم الحوالة وتاريخها من فضلك.', NULL),
        (N'قيد المراجعة',
         N'استلمنا طلبك ورقمه المرجعي {المرجع}. نراجعه الآن ونوافيك بالنتيجة.', NULL),
        (N'طلب صورة الإيصال',
         N'أرسل لنا صورة الإيصال من فضلك لنطابقها مع السجلّ.', NULL),
        (N'تأخير الشبكة',
         N'العملية قد تتأخر بسبب ضغط الشبكة. راجع التطبيق بعد قليل، وإن لم يتغيّر شيء أخبرنا.', NULL),
        (N'الإغلاق بعد الحلّ',
         N'تمّت معالجة طلبك. إن ظهر شيء آخر راسلنا في أي وقت وسنكمل معك.', NULL);
END;
GO

/* ---------------------------------------------------------------------------
   2) المسودّات (البندان 21 و37)

   ⚠ في الخادم لا في المتصفّح: الموظّف يفتح اللوحة على الحاسوب وعلى
   الهاتف، ومسودّةٌ محفوظةٌ في متصفّحٍ واحد ضائعةٌ من الآخر. وهي أيضاً
   ما ينقذ ردّاً طويلاً من انقطاع كهرباء.
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_drafts', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_drafts (
        thread_id   BIGINT        NOT NULL,
        staff_id    BIGINT        NOT NULL,
        body        NVARCHAR(MAX) NOT NULL,
        -- هل هي مسودّةُ ردٍّ أم مسودّةُ ملاحظةٍ داخلية؟ الخلطُ بينهما هو
        -- أن يُرسَل إلى الوكيل ما كُتب للفريق.
        is_internal BIT           NOT NULL
                    CONSTRAINT DF_sup_draft_int DEFAULT (0),
        updated_at  DATETIME2     NOT NULL
                    CONSTRAINT DF_sup_draft_upd DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_support_drafts PRIMARY KEY (thread_id, staff_id),
        CONSTRAINT FK_sup_draft_thread FOREIGN KEY (thread_id)
            REFERENCES dbo.chat_threads (id),
        CONSTRAINT FK_sup_draft_staff FOREIGN KEY (staff_id)
            REFERENCES dbo.support_staff (id)
    );
END;
GO

/* ---------------------------------------------------------------------------
   3) المتابعات (البند 16)

   تذكيرٌ شخصيّ بموعد: «راجع هذه الحالة غداً الساعة العاشرة». لا يغيّر
   حالةَ المحادثة ولا يراه أحدٌ غير صاحبه.
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_followups', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_followups (
        id          BIGINT IDENTITY(1,1) NOT NULL,
        thread_id   BIGINT        NOT NULL,
        staff_id    BIGINT        NOT NULL,
        due_at      DATETIME2     NOT NULL,
        note        NVARCHAR(400) NULL,
        created_at  DATETIME2     NOT NULL
                    CONSTRAINT DF_sup_fu_created DEFAULT (SYSDATETIME()),
        -- تُختَم ولا تُحذف: ما ذكّر به الموظّفُ نفسَه ومتى جزءٌ من قصّة
        -- الحالة، وحذفُه يمحو لماذا عاد إليها في ذلك اليوم.
        done_at     DATETIME2     NULL,
        CONSTRAINT PK_support_followups PRIMARY KEY (id),
        CONSTRAINT FK_sup_fu_thread FOREIGN KEY (thread_id)
            REFERENCES dbo.chat_threads (id),
        CONSTRAINT FK_sup_fu_staff FOREIGN KEY (staff_id)
            REFERENCES dbo.support_staff (id)
    );
END;
GO

/* الاستعلامُ الوحيد عليها: «ما استحقّ منها لي». */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sup_fu_due'
               AND object_id = OBJECT_ID('dbo.support_followups'))
    CREATE INDEX IX_sup_fu_due ON dbo.support_followups (staff_id, done_at, due_at);
GO

/* ---------------------------------------------------------------------------
   4) التأجيل والتصعيد على حالة المحادثة (البندان 13 و15)

   `snoozed_until` و`snoozed_by` و`snooze_reason` أُضيفت في الدفعة الأولى.
   الناقصُ هنا: **متى** أُجّلت (لا متى تعود)، وأعمدةُ التصعيد.
   --------------------------------------------------------------------------- */
IF COL_LENGTH('dbo.support_thread_state', 'snoozed_at') IS NULL
    ALTER TABLE dbo.support_thread_state ADD snoozed_at DATETIME2 NULL;
GO

IF COL_LENGTH('dbo.support_thread_state', 'escalated_at') IS NULL
BEGIN
    ALTER TABLE dbo.support_thread_state ADD
        escalated_at     DATETIME2     NULL,
        escalated_by     BIGINT        NULL,
        escalated_to     BIGINT        NULL,
        -- ⚠ السببُ إلزاميّ في الشيفرة: تصعيدٌ بلا سبب تغييرُ أولويةٍ لا
        -- تصعيد، ويجعل المشرفَ يقرأ المحادثةَ كلَّها ليعرف ما المطلوب.
        escalation_reason NVARCHAR(400) NULL;
END;
GO

/* «ما أُجّل وحان موعدُه» يُقرأ في كل فتحةٍ للصندوق. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sup_state_snoozed'
               AND object_id = OBJECT_ID('dbo.support_thread_state'))
    CREATE INDEX IX_sup_state_snoozed ON dbo.support_thread_state (snoozed_until);
GO
