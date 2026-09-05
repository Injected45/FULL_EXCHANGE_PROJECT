/* ============================================================================
   الدردشة — الوكيل مع الإدارة، والوكيل مع موظّفيه.

   أمر المالك (5 سبتمبر 2026): «الوكيل يتحدّث مع الإدارة لطلب أي مساعدة،
   والوكيل يتحدّث مع موظّفيه الذين أعطاهم صلاحية باشتراك بمفتاح من قبله».

   ── طبقة تواصل، لا طبقة مالية ────────────────────────────────────────────
   لا يمسّ هذا شيئاً من المال: لا رصيد ولا قيد ولا حوالة ولا خزينة. ولا مفتاح
   أجنبي إلى جدولٍ من جداول المنظومة — قيدٌ من جدول رسائل إلى شجرة الحسابات
   يجعل تعديلاً مالياً يفشل بسبب رسالة.

   ── ثلاثة جداول، وسببُ كلٍّ منها ─────────────────────────────────────────

   1. `chat_threads`  — المحادثة. نوعها ADMIN أو EMPLOYEE.
   2. `chat_messages` — الرسائل.
   3. `chat_reads`    — إلى أين قرأ كلُّ طرف.

   **لماذا `chat_reads` جدولٌ وليس عموداً `is_read` على الرسالة:** للرسالة
   الواحدة قارئان (المرسِل والمستقبِل)، وراية واحدة عليها لا تقول أيّهما قرأ.
   وعدّاد «غير المقروء» يصير حينها كذبة على أحد الطرفين. الصفّ هنا يقول
   «هذا الطرف بلغ هذه الرسالة»، ورقمٌ واحد يكفي لأن الرسائل تصاعدية.

   **لماذا المرسِل عمودان (`sender_kind` + `sender_id`) لا عمود واحد:**
   الوكيل رقمه في `users`، والموظف رقمه في `employees`، والإدارة في `users`
   كذلك — فضاءان مختلفان للأرقام. عمودٌ واحد بلا مميّز يجعل رسالة الموظف
   رقم 7 تُنسب إلى المستخدم رقم 7، وهو شخص آخر تماماً.

   **لماذا `last_message_at` مخزَّن رغم أنه مشتقّ:** قائمة المحادثات تُرتَّب
   به، وترتيبٌ باستعلامٍ فرعي على `chat_messages` لكل صفّ هو الشكل الذي جعل
   كشف الحساب يستغرق 68 ثانية. يُكتب مع كل رسالة في المعاملة نفسها.
   ============================================================================ */

/* ---------------------------------------------------------------------------
   1) المحادثات
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.chat_threads', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.chat_threads (
        id               BIGINT IDENTITY(1,1) NOT NULL
                         CONSTRAINT PK_chat_threads PRIMARY KEY,

        /* صاحب المحادثة دائماً — `users.id` للوكيل.
           بلا مفتاح أجنبي إلى `users` عمداً: جدول المنظومة لا يُقيَّد بجدولنا. */
        agent_id         BIGINT NOT NULL,

        /* ADMIN: الوكيل مع إدارة الرحالة.  EMPLOYEE: الوكيل مع موظّف بعينه. */
        kind             VARCHAR(20) NOT NULL
                         CONSTRAINT CK_chat_thread_kind
                         CHECK (kind IN ('ADMIN', 'EMPLOYEE')),

        /* يُملأ في محادثة الموظف وحدها. */
        employee_id      BIGINT NULL
                         CONSTRAINT FK_chat_thread_employee
                         REFERENCES dbo.employees(id),

        /* مشتقّ ومخزَّن — انظر ترويسة الملف. */
        last_message_at  DATETIME2 NULL,

        created_at       DATETIME2 NOT NULL
                         CONSTRAINT DF_chat_threads_created DEFAULT (sysutcdatetime()),
        updated_at       DATETIME2 NOT NULL
                         CONSTRAINT DF_chat_threads_updated DEFAULT (sysutcdatetime())
    );
END;
GO

/* محادثةٌ واحدة لا أكثر بين الطرفين نفسهما.
   فهرسٌ فريد مُرشَّح لأن `employee_id` فارغ في محادثات الإدارة، و NULL في
   SQL Server لا يساوي NULL — فقيدٌ فريد عادي كان يسمح بعشر محادثات إدارة. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_thread_admin'
               AND object_id = OBJECT_ID('dbo.chat_threads'))
    CREATE UNIQUE INDEX UQ_chat_thread_admin
        ON dbo.chat_threads (agent_id)
        WHERE kind = 'ADMIN';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_thread_employee'
               AND object_id = OBJECT_ID('dbo.chat_threads'))
    CREATE UNIQUE INDEX UQ_chat_thread_employee
        ON dbo.chat_threads (agent_id, employee_id)
        WHERE kind = 'EMPLOYEE';
GO

/* قائمة محادثات الوكيل، مرتّبةً بالأحدث. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_chat_threads_agent'
               AND object_id = OBJECT_ID('dbo.chat_threads'))
    CREATE INDEX IX_chat_threads_agent
        ON dbo.chat_threads (agent_id, last_message_at DESC);
GO

/* ---------------------------------------------------------------------------
   2) الرسائل
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.chat_messages', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.chat_messages (
        id          BIGINT IDENTITY(1,1) NOT NULL
                    CONSTRAINT PK_chat_messages PRIMARY KEY,

        thread_id   BIGINT NOT NULL
                    CONSTRAINT FK_chat_msg_thread REFERENCES dbo.chat_threads(id),

        /* AGENT | EMPLOYEE | ADMIN — انظر ترويسة الملف. */
        sender_kind VARCHAR(20) NOT NULL
                    CONSTRAINT CK_chat_sender_kind
                    CHECK (sender_kind IN ('AGENT', 'EMPLOYEE', 'ADMIN')),
        sender_id   BIGINT NOT NULL,

        /* اسم المرسِل وقت الإرسال.
           منسوخ عمداً: الموظف قد يُحذف أو يتغيّر اسمه، ورسالةٌ قديمة تحمل
           اسماً جديداً تُحرّف سجلّ المحادثة. الاسم هنا شهادةٌ على لحظته. */
        sender_name NVARCHAR(200) NULL,

        /* 2000 حرفاً لا MAX: هذه دردشة تشغيلية لا مستودع مستندات، وحدٌّ
           معلوم يُفحص في الخادم أهون من صفٍّ بحجم ميغابايت لا أحد يقرؤه. */
        body        NVARCHAR(2000) NOT NULL,

        created_at  DATETIME2 NOT NULL
                    CONSTRAINT DF_chat_messages_created DEFAULT (sysutcdatetime())
    );
END;
GO

/* الاستعلام الوحيد المتكرّر: رسائل محادثةٍ بعد رقم معيّن. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_chat_messages_thread'
               AND object_id = OBJECT_ID('dbo.chat_messages'))
    CREATE INDEX IX_chat_messages_thread
        ON dbo.chat_messages (thread_id, id);
GO

/* ---------------------------------------------------------------------------
   3) إلى أين قرأ كل طرف
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.chat_reads', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.chat_reads (
        id                   BIGINT IDENTITY(1,1) NOT NULL
                             CONSTRAINT PK_chat_reads PRIMARY KEY,

        thread_id            BIGINT NOT NULL
                             CONSTRAINT FK_chat_read_thread REFERENCES dbo.chat_threads(id),

        reader_kind          VARCHAR(20) NOT NULL
                             CONSTRAINT CK_chat_reader_kind
                             CHECK (reader_kind IN ('AGENT', 'EMPLOYEE', 'ADMIN')),
        reader_id            BIGINT NOT NULL,

        /* آخر رسالة بلغها هذا الطرف. الأرقام تصاعدية، فرقمٌ واحد يصف
           «قرأتُ كل ما قبله» بلا صفٍّ لكل رسالة. */
        last_read_message_id BIGINT NOT NULL
                             CONSTRAINT DF_chat_reads_last DEFAULT (0),

        updated_at           DATETIME2 NOT NULL
                             CONSTRAINT DF_chat_reads_updated DEFAULT (sysutcdatetime())
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_read_participant'
               AND object_id = OBJECT_ID('dbo.chat_reads'))
    CREATE UNIQUE INDEX UQ_chat_read_participant
        ON dbo.chat_reads (thread_id, reader_kind, reader_id);
GO
