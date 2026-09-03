/* ============================================================
   هوية الشركة داخل التطبيق — طبقة عرض لا غير.

   قرار المالك (2 سبتمبر 2026): لكل شركة أو وكيل هويةٌ بصرية داخل التطبيق
   بعد تسجيل الدخول، وتبقى هوية «شركة الرحالة» الرسمية قبله. والميزة
   **Presentation Layer فقط**: لا تمسّ رصيداً ولا حوالة ولا صلاحية ولا
   عملية مالية.

   ── مفتاح الشركة ─────────────────────────────────────────────
   لا يوجد في `users` عمود شركة ولا tenant. الذي يعرّف الشركة فعلاً هو
   **`users.AccID`** — حسابها في شجرة الحسابات، وهو ما يحمل اسمها
   (AccID 530 = «جاري شركة الامانة»). فهو مفتاح العزل هنا.

   ولا مفتاح أجنبي إلى AccountsTb عمداً: هذا جدول عرضٍ ملحق، وربطه بقيدٍ
   يجعل تعديلاً في شجرة الحسابات المالية يفشل بسبب صفّ ألوان.
   ============================================================ */

IF OBJECT_ID('dbo.tenant_branding', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tenant_branding (
        id                 BIGINT IDENTITY(1,1) PRIMARY KEY,

        /* users.AccID — صفٌّ واحد لكل شركة. */
        company_account_id BIGINT        NOT NULL,

        company_name_ar    NVARCHAR(200) NULL,
        company_name_en    NVARCHAR(200) NULL,

        /* مسار الملف في التخزين لا الصورة نفسها: صورة داخل قاعدة بيانات
           تُثقل كل استعلام يقرأ الصفّ، ولا تُخدَّم بذاكرة وسيطة. */
        logo_path          NVARCHAR(400) NULL,

        /* #RRGGBB. تُتحقّق في الخدمة لا هنا: رسالة خطأ مفهومة خيرٌ من
           انتهاك قيد. */
        primary_color      VARCHAR(9)    NULL,
        secondary_color    VARCHAR(9)    NULL,
        background_color   VARCHAR(9)    NULL,

        /* مفتاح ثيم من كتالوج ثابت في الخادم — لا ألوان حرّة تكسر التصميم. */
        theme_key          VARCHAR(40)   NOT NULL
                           CONSTRAINT DF_tb_theme DEFAULT ('classic_green'),

        is_active          BIT           NOT NULL
                           CONSTRAINT DF_tb_active DEFAULT (1),

        /* يقرأه التطبيق ليعرف: هل تغيّرت الهوية منذ آخر مزامنة؟
           عدّاد لا طابع وقت — ساعات الخوادم تتزحزح، والعدّاد لا يرجع. */
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
END;
GO

/* سجلّ تغيّر الهوية — من غيّر، ومتى، وماذا كان قبل. */
IF OBJECT_ID('dbo.tenant_branding_audit', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tenant_branding_audit (
        id                 BIGINT IDENTITY(1,1) PRIMARY KEY,
        company_account_id BIGINT         NOT NULL,
        changed_by         BIGINT         NULL,
        changed_at         DATETIME2      NOT NULL
                           CONSTRAINT DF_tba_at DEFAULT (SYSUTCDATETIME()),

        /* الحقل الذي تغيّر وقيمتاه — صفٌّ لكل حقل لا صفٌّ لكل حفظ، فيُقرأ
           «من غيّر الشعار؟» بسؤال واحد. */
        field_name         VARCHAR(60)    NOT NULL,
        old_value          NVARCHAR(400)  NULL,
        new_value          NVARCHAR(400)  NULL,

        ip_address         VARCHAR(64)    NULL,
        device_id          VARCHAR(128)   NULL
    );

    CREATE INDEX IX_tba_company ON dbo.tenant_branding_audit
        (company_account_id, changed_at);
END;
GO
