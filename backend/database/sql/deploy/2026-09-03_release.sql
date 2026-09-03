/* ============================================================================
   نشرة قاعدة البيانات — 3 سبتمبر 2026
   من قاعدة التجربة (نسخة الرحالة) إلى القاعدة الرئيسية.

   ── ما في هذه النشرة ───────────────────────────────────────────────────────
   **أربعة جداول جديدة، ولا شيء غيرها.**

   لا تعديل على جدولٍ قائم، ولا على إجراء مخزّن، ولا على مشهد، ولا على مشغّل
   (trigger). تُحقّق من ذلك بالاستعلام في نهاية الملف — وقد نُفِّذ على قاعدة
   التجربة فأعاد **صفراً** في الحالتين.

   ولا يمسّ أيٌّ من الأربعة المسار المالي: لا `wallet`، ولا `ExchangeAccData`،
   ولا `InternalEx`، ولا `EX24AccSafeActivityTb`. اثنان دفتر متابعة تسليم
   منفصل عن `InternalEx.ConfirmType` بقرار المالك، واثنان طبقة عرض
   (اسم الشركة وشعارها وألوانها).

   ── خصائص هذا السكربت ──────────────────────────────────────────────────────
   • **قابل لإعادة التنفيذ**: كل كائن محروس بـ `IF NOT EXISTS`. تشغيله مرّتين
     لا يُنشئ شيئاً مرّتين ولا يفشل.
   • **لا يحذف ولا يعدّل ولا يُدخل بيانات.** لا `DROP` ولا `ALTER` على قائم
     ولا `INSERT`. بيانات التجربة تبقى في قاعدة التجربة.
   • **بلا مفاتيح أجنبية** إلى جداول المنظومة عمداً: قيدٌ من جدول عرضٍ ملحق
     إلى شجرة الحسابات يجعل تعديلاً مالياً يفشل بسبب صفّ ألوان.

   ── قبل التنفيذ ────────────────────────────────────────────────────────────
   1. خذ نسخة احتياطية كاملة. هذه عادة لا استثناء.
   2. نفّذه على القاعدة الرئيسية وحدها بعد `USE [<اسم القاعدة الرئيسية>]`.
   3. راجع مخرجات القسم الأخير — يجب أن تظهر الجداول الأربعة.

   ── ما لا يفعله هذا السكربت (خطوات خارج قاعدة البيانات) ────────────────────
   • مجلّد الشعارات على الخادم: `storage/app/private/branding` يجب أن يكون
     موجوداً وقابلاً للكتابة من مستخدم PHP، وإلا فشل رفع الشعار وحده.
   • نشر كود الـ API الجديد (المتحكّمات والخدمات والمسارات).
   ============================================================================ */

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO


/* ============================================================================
   1) agent_incoming_transfers — دفتر تسليم الوكيل
   ----------------------------------------------------------------------------
   يجيب سؤالاً واحداً لا تجيبه المنظومة: **هل سلّم الوكيل المال للمستفيد؟**
   وهو غير `InternalEx.ConfirmType` الذي يقول أين الحوالة بين الوكيل والرحالة.
   الفصل بينهما قرار المالك: تلك الحالة تقوم عليها العمليات الحسابية ولا
   يكتبها التطبيق أبداً — وهذا الجدول لا يُكتب إلا منه.

   `core_confirm_type` و`core_status_label` **مرآة للقراءة فقط**: تُنسخ من
   المنظومة ليُعرف الإلغاء، ولا تُكتب إليها.
   ============================================================================ */
IF OBJECT_ID('dbo.agent_incoming_transfers', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.agent_incoming_transfers (
        id                  BIGINT IDENTITY(1,1) NOT NULL
                            CONSTRAINT PK_agent_incoming_transfers PRIMARY KEY,

        /* users.id للوكيل — لا الفرع: وكيلان في فرعٍ واحد لكلٍّ دفتره. */
        agent_id            BIGINT        NOT NULL,

        /* InternalEx.Code — رقم الحوالة كما يعرفه الوكيل والعميل. */
        transfer_number     VARCHAR(50)   NOT NULL,

        beneficiary_name    NVARCHAR(200) NULL,
        beneficiary_phone   VARCHAR(30)   NULL,
        sender_name         NVARCHAR(200) NULL,

        /* OverallVal — المبلغ المُرسل لا العمولة. */
        amount              DECIMAL(18,3) NULL,
        currency_id         INT           NULL,
        branch_delivered_id INT           NULL,

        /* 'PENDING_DELIVERY' أو 'DELIVERED' — حالة الوكيل وحدها. */
        status              VARCHAR(30)   NOT NULL
                            CONSTRAINT DF_ait_status DEFAULT ('PENDING_DELIVERY'),

        received_at         DATETIME2     NOT NULL
                            CONSTRAINT DF_ait_received DEFAULT (SYSUTCDATETIME()),
        delivered_at        DATETIME2     NULL,
        delivered_by        BIGINT        NULL,

        created_at          DATETIME2     NOT NULL
                            CONSTRAINT DF_ait_created DEFAULT (SYSUTCDATETIME()),
        updated_at          DATETIME2     NOT NULL
                            CONSTRAINT DF_ait_updated DEFAULT (SYSUTCDATETIME()),

        /* مرآة حالة المنظومة — قراءة فقط. 3،4 قيد الإلغاء · 5 ملغية ·
           6 ملغية مسلمة. */
        core_confirm_type   INT           NULL,
        core_status_label   NVARCHAR(50)  NULL,
        core_synced_at      DATETIME2     NULL,

        commission          DECIMAL(18,3) NULL,
        sent_at             DATETIME2     NULL,
        sender_branch_name  NVARCHAR(200) NULL
    );

    /* الحارس ضدّ التكرار في القاعدة لا في التطبيق: مزامنةٌ تتكرّر لا تُنشئ
       صفّاً ثانياً لنفس الحوالة عند نفس الوكيل. */
    CREATE UNIQUE INDEX UX_ait_agent_transfer
        ON dbo.agent_incoming_transfers (agent_id, transfer_number);

    /* فهرس تبويبات الشاشة: الحالة والإلغاء والبحث بالرقم. */
    CREATE INDEX IX_ait_agent_status
        ON dbo.agent_incoming_transfers (transfer_number, delivered_at, agent_id, status);

    CREATE INDEX IX_ait_phone
        ON dbo.agent_incoming_transfers (beneficiary_phone);

    CREATE INDEX IX_ait_created
        ON dbo.agent_incoming_transfers (created_at);

    CREATE INDEX IX_ait_agent_core
        ON dbo.agent_incoming_transfers (agent_id, core_confirm_type);

    PRINT 'أُنشئ: agent_incoming_transfers';
END
ELSE
    PRINT 'موجود سلفاً: agent_incoming_transfers';
GO


/* ============================================================================
   2) transfer_status_history — سجلّ تغيّر حالة التسليم
   ----------------------------------------------------------------------------
   صفٌّ لكل انتقال حالة، مع من غيّر ومن أي عنوان وأي جهاز. يجيب «متى سُلِّمت
   هذه الحوالة ومن سجّلها؟» بسؤال واحد، ولا يُحذف منه شيء.
   ============================================================================ */
IF OBJECT_ID('dbo.transfer_status_history', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.transfer_status_history (
        id              BIGINT IDENTITY(1,1) NOT NULL
                        CONSTRAINT PK_transfer_status_history PRIMARY KEY,

        transfer_id     BIGINT         NOT NULL,   /* agent_incoming_transfers.id */
        transfer_number VARCHAR(50)    NOT NULL,

        old_status      VARCHAR(30)    NULL,
        new_status      VARCHAR(30)    NOT NULL,

        changed_by      BIGINT         NULL,
        changed_at      DATETIME2      NOT NULL
                        CONSTRAINT DF_tsh_at DEFAULT (SYSUTCDATETIME()),

        ip_address      VARCHAR(64)    NULL,
        device_id       VARCHAR(128)   NULL,
        session_id      VARCHAR(128)   NULL,
        notes           NVARCHAR(500)  NULL
    );

    CREATE INDEX IX_tsh_transfer
        ON dbo.transfer_status_history (transfer_id, changed_at);

    CREATE INDEX IX_tsh_number
        ON dbo.transfer_status_history (transfer_number);

    PRINT 'أُنشئ: transfer_status_history';
END
ELSE
    PRINT 'موجود سلفاً: transfer_status_history';
GO


/* ============================================================================
   3) tenant_branding — هوية الشركة داخل التطبيق
   ----------------------------------------------------------------------------
   طبقة عرض لا غير: اسم الشركة وشعارها وثيمها بعد تسجيل الدخول.

   **مفتاح الشركة هو `users.AccID`** — لا يوجد في `users` عمود شركة، والذي
   يعرّفها فعلاً هو حسابها في شجرة الحسابات. ولا يُقرأ من جسم الطلب أبداً؛
   يُشتقّ من التوثيق في الخادم.

   `branding_version` عدّاد لا طابع وقت: ساعات الخوادم تتزحزح، والعدّاد لا
   يرجع إلى الوراء.
   ============================================================================ */
IF OBJECT_ID('dbo.tenant_branding', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tenant_branding (
        id                 BIGINT IDENTITY(1,1) NOT NULL
                           CONSTRAINT PK_tenant_branding PRIMARY KEY,

        company_account_id BIGINT        NOT NULL,   /* users.AccID */

        company_name_ar    NVARCHAR(200) NULL,
        company_name_en    NVARCHAR(200) NULL,

        /* مسار الملف لا الصورة: صورة داخل القاعدة تُثقل كل استعلام يقرأ الصفّ. */
        logo_path          NVARCHAR(400) NULL,

        primary_color      VARCHAR(9)    NULL,       /* #RRGGBB */
        secondary_color    VARCHAR(9)    NULL,
        background_color   VARCHAR(9)    NULL,

        theme_key          VARCHAR(40)   NOT NULL
                           CONSTRAINT DF_tb_theme DEFAULT ('classic_green'),

        is_active          BIT           NOT NULL
                           CONSTRAINT DF_tb_active DEFAULT (1),

        branding_version   INT           NOT NULL
                           CONSTRAINT DF_tb_version DEFAULT (1),

        created_at         DATETIME2     NOT NULL
                           CONSTRAINT DF_tb_created DEFAULT (SYSUTCDATETIME()),
        updated_at         DATETIME2     NOT NULL
                           CONSTRAINT DF_tb_updated DEFAULT (SYSUTCDATETIME())
    );

    /* صفٌّ واحد لا غير لكل شركة — الحارس في القاعدة لا في التطبيق. */
    CREATE UNIQUE INDEX UX_tb_company
        ON dbo.tenant_branding (company_account_id);

    PRINT 'أُنشئ: tenant_branding';
END
ELSE
    PRINT 'موجود سلفاً: tenant_branding';
GO


/* ============================================================================
   4) tenant_branding_audit — سجلّ تغيّر الهوية
   ----------------------------------------------------------------------------
   صفٌّ لكل **حقل** تغيّر لا لكل حفظ، فيُقرأ «من غيّر الشعار؟» بسؤال واحد.
   ============================================================================ */
IF OBJECT_ID('dbo.tenant_branding_audit', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tenant_branding_audit (
        id                 BIGINT IDENTITY(1,1) NOT NULL
                           CONSTRAINT PK_tenant_branding_audit PRIMARY KEY,

        company_account_id BIGINT         NOT NULL,
        changed_by         BIGINT         NULL,
        changed_at         DATETIME2      NOT NULL
                           CONSTRAINT DF_tba_at DEFAULT (SYSUTCDATETIME()),

        field_name         VARCHAR(60)    NOT NULL,
        old_value          NVARCHAR(400)  NULL,
        new_value          NVARCHAR(400)  NULL,

        ip_address         VARCHAR(64)    NULL,
        device_id          VARCHAR(128)   NULL
    );

    CREATE INDEX IX_tba_company
        ON dbo.tenant_branding_audit (company_account_id, changed_at);

    PRINT 'أُنشئ: tenant_branding_audit';
END
ELSE
    PRINT 'موجود سلفاً: tenant_branding_audit';
GO


/* ============================================================================
   التحقّق بعد التنفيذ — يجب أن تظهر الجداول الأربعة، وأن يكون العددان صفراً.
   ============================================================================ */
PRINT '';
PRINT '--- الجداول الأربعة ---';
SELECT name AS [الجدول], create_date AS [تاريخ الإنشاء]
FROM sys.tables
WHERE name IN ('agent_incoming_transfers','transfer_status_history',
               'tenant_branding','tenant_branding_audit')
ORDER BY name;

PRINT '--- يجب أن يكون 4 ---';
SELECT COUNT(*) AS [عدد الجداول المنشأة]
FROM sys.tables
WHERE name IN ('agent_incoming_transfers','transfer_status_history',
               'tenant_branding','tenant_branding_audit');

PRINT '--- جداول قائمة تغيّر تعريفها اليوم (يجب أن يكون 0) ---';
SELECT COUNT(*) AS [جداول قائمة تغيّرت]
FROM sys.tables
WHERE modify_date >= CAST(GETDATE() AS DATE)
  AND name NOT IN ('agent_incoming_transfers','transfer_status_history',
                   'tenant_branding','tenant_branding_audit');

PRINT '--- إجراءات/مشاهد/دوال/مشغّلات تغيّرت اليوم (يجب أن يكون 0) ---';
SELECT COUNT(*) AS [كائنات برمجية تغيّرت]
FROM sys.objects
WHERE type IN ('P','V','FN','TF','IF','TR')
  AND modify_date >= CAST(GETDATE() AS DATE);
GO
