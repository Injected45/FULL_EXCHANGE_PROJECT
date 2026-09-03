/* ============================================================================
   منصّة نقاط البيع والموظفين — المرحلة 1: قاعدة البيانات
   ----------------------------------------------------------------------------
   قرارات معمارية اتُّخذت بعد مراجعة القائم، وكلٌّ منها يمنع نظاماً موازياً:

   1. **نقاط البيع لا تُنشأ من جديد.** `AuthorizedUsers` هو جدول نقاط البيع
      في المنظومة منذ البدء (11 صفّاً حيّاً، وله `AuthorizedUsers_Add`
      و`_update` و`getByBranch` ويستعملها تطبيق سطح المكتب). إنشاء
      `points_of_sale` جديد يعني نقطتَي بيع لكل فرع تفترقان عند أول تعديل.
      فالجداول هنا تشير إليه بـ `point_of_sale_id = AuthorizedUsers.ID`.

   2. **الموظف ليس مستخدماً في `users`.** لا يُنشأ له صفّ هناك ولا يُمنح
      رمز Sanctum من مسار الوكيل: الفصل بين سياقَي التوثيق شرطٌ في المستند
      (بند 22)، وأسهل طريق لخرقه أن يكون الاثنان في جدول واحد بعمود دور.

   3. **لا عمود واحد يُضاف إلى `InternalEx` ولا إلى أي جدول مالي.** نسبةُ
      العملية إلى الموظف تُحفظ في `transfer_attributions` بجانبها. الدفتر
      المالي ملك المنظومة، والتطبيق لا يكتب فيه — قرار المالك القائم.

   4. **`device_registry` منفصل عن `employee_devices`.** الأول تصنيفٌ دائم
      للجهاز لا يُحذف أبداً (بند 19-20: الحظر يبقى بعد الخروج وإلغاء الجهاز
      وحذف التطبيق). والثاني ربطٌ تشغيليّ يُلغى ويُعاد. خلطهما يجعل
      «إلغاء الجهاز» يمحو الحظر الأمني — وهو بالضبط ما يُمنع.

   5. **الصلاحيات صفوفٌ مُمنوحة لا أعمدة.** `employee_permissions` يحمل ما
      مُنح صراحةً؛ وما ليس فيه مرفوض (بند 29). وميزةٌ جديدة تُضاف غداً لا
      تحتاج تعديل جدول ولا هجرة — تظهر في الكتالوج مرفوضةً للجميع (بند 30).

   6. **بلا مفاتيح أجنبية إلى جداول المنظومة** (`users`, `AuthorizedUsers`,
      `InternalEx`): قيدٌ من جدولنا إلى دفترها يجعل عمليةً مالية تفشل بسبب
      صفّ موظف. الترابط يُفرض في الخدمة، والفهارس موجودة. أما بين جداولنا
      فالمفاتيح الأجنبية مفروضة فعلاً.

   قابل لإعادة التنفيذ: كل كائن محروس بـ IF NOT EXISTS. لا DROP ولا حذف بيانات.
   ============================================================================ */

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* ============================================================================
   1) employees — الموظف التابع للوكيل
   ============================================================================ */
IF OBJECT_ID('dbo.employees', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employees (
        id                  BIGINT IDENTITY(1,1) NOT NULL
                            CONSTRAINT PK_employees PRIMARY KEY,

        /* users.id للوكيل المالك — كل موظف تحت وكيل واحد لا يتغيّر. */
        agent_id            BIGINT        NOT NULL,
        /* users.AccID — حساب الشركة. يُنسخ للاستعلام لا للتفويض. */
        agent_account_id    BIGINT        NULL,
        branch_id           INT           NULL,

        full_name           NVARCHAR(200) NOT NULL,
        /* بصيغة الخادم: 9 خانات بلا صفر ولا مفتاح دولة (Fmt.phoneForApi). */
        phone               VARCHAR(20)   NOT NULL,
        national_number     VARCHAR(30)   NULL,
        notes               NVARCHAR(500) NULL,

        /* PENDING_ACTIVATION · ACTIVE · SUSPENDED · DISABLED
           REQUIRES_REACTIVATION · COMPROMISED */
        status              VARCHAR(30)   NOT NULL
                            CONSTRAINT DF_emp_status DEFAULT ('PENDING_ACTIVATION'),

        last_login_at       DATETIME2     NULL,
        last_activity_at    DATETIME2     NULL,
        activated_at        DATETIME2     NULL,

        created_by          BIGINT        NULL,
        created_at          DATETIME2     NOT NULL
                            CONSTRAINT DF_emp_created DEFAULT (SYSUTCDATETIME()),
        updated_at          DATETIME2     NOT NULL
                            CONSTRAINT DF_emp_updated DEFAULT (SYSUTCDATETIME()),
        /* حذف ناعم: تاريخ الموظف المالي لا يُمحى بحذف صفّه. */
        deleted_at          DATETIME2     NULL
    );

    /* رقم الهاتف مفتاح التفعيل، فلا يتكرّر عند وكيلٍ واحد — وإلا صار
       «أي موظف يملك هذا الرقم؟» سؤالاً بلا جواب واحد. */
    CREATE UNIQUE INDEX UX_emp_agent_phone
        ON dbo.employees (agent_id, phone) WHERE deleted_at IS NULL;

    CREATE INDEX IX_emp_agent_status ON dbo.employees (agent_id, status);
    CREATE INDEX IX_emp_phone        ON dbo.employees (phone);

    PRINT 'أُنشئ: employees';
END ELSE PRINT 'موجود: employees';
GO

/* ============================================================================
   2) employee_point_of_sales — الموظف × نقطة البيع
   ----------------------------------------------------------------------------
   علاقة مستقلّة لأن الموظف قد يعمل في أكثر من نقطة (بند 5). و`is_primary`
   يحدّد الافتراضية، و«نقطة البيع الفعّالة» وقت العملية تُحفظ في الوردية
   وفي كل عملية على حدة — لا يُستنتج من هنا لاحقاً.
   ============================================================================ */
IF OBJECT_ID('dbo.employee_point_of_sales', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_point_of_sales (
        id                BIGINT IDENTITY(1,1) NOT NULL
                          CONSTRAINT PK_emp_pos PRIMARY KEY,
        employee_id       BIGINT   NOT NULL
                          CONSTRAINT FK_emp_pos_employee
                          REFERENCES dbo.employees(id),
        /* AuthorizedUsers.ID — نقطة البيع القائمة في المنظومة. */
        point_of_sale_id  INT      NOT NULL,
        is_primary        BIT      NOT NULL CONSTRAINT DF_emp_pos_primary DEFAULT (0),
        is_active         BIT      NOT NULL CONSTRAINT DF_emp_pos_active  DEFAULT (1),
        created_at        DATETIME2 NOT NULL CONSTRAINT DF_emp_pos_created DEFAULT (SYSUTCDATETIME())
    );

    CREATE UNIQUE INDEX UX_emp_pos ON dbo.employee_point_of_sales (employee_id, point_of_sale_id);
    CREATE INDEX IX_emp_pos_pos    ON dbo.employee_point_of_sales (point_of_sale_id, is_active);

    PRINT 'أُنشئ: employee_point_of_sales';
END ELSE PRINT 'موجود: employee_point_of_sales';
GO

/* ============================================================================
   3) device_registry — التصنيف الأمني الدائم للجهاز
   ----------------------------------------------------------------------------
   ⚠ هذا الجدول **لا يُحذف منه صفّ أبداً**. هو ما ينفّذ البندين 19 و20:
   جهازٌ استُعمل مرّة كجهاز موظف يُمنع من الدخول كمسؤول إلى الأبد، ولا يرفع
   المنعَ خروجٌ ولا إلغاء جهاز ولا حذف التطبيق وإعادة تثبيته.

   ولهذا هو منفصل عن `employee_devices`: ذاك ربطٌ تشغيليّ يُلغى، وهذا حكمٌ
   تاريخي يبقى.

   `device_hash` لا `device_id` خاماً: المعرّف يأتي من عتاد الجهاز
   (ANDROID_ID / identifierForVendor)، وتخزينه مُجزّأً يمنع تسريبه من نسخة
   احتياطية، ويبقى قابلاً للمطابقة لأن التجزئة ثابتة.
   ============================================================================ */
IF OBJECT_ID('dbo.device_registry', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.device_registry (
        id             BIGINT IDENTITY(1,1) NOT NULL
                       CONSTRAINT PK_device_registry PRIMARY KEY,

        device_hash    VARCHAR(64)   NOT NULL,   /* SHA-256 لمعرّف الجهاز */

        /* EMPLOYEE_DEVICE · AGENT_DEVICE */
        classification VARCHAR(30)   NOT NULL,

        first_seen_at  DATETIME2     NOT NULL
                       CONSTRAINT DF_dr_first DEFAULT (SYSUTCDATETIME()),
        last_seen_at   DATETIME2     NOT NULL
                       CONSTRAINT DF_dr_last  DEFAULT (SYSUTCDATETIME()),

        /* لأثر التدقيق فقط — لا للتفويض. */
        first_agent_id    BIGINT     NULL,
        first_employee_id BIGINT     NULL,
        platform          VARCHAR(20) NULL,
        model             NVARCHAR(120) NULL
    );

    /* تصنيفٌ واحد لكل جهاز لكل نوع — الحارس في القاعدة. */
    CREATE UNIQUE INDEX UX_dr_hash_class ON dbo.device_registry (device_hash, classification);
    CREATE INDEX IX_dr_hash ON dbo.device_registry (device_hash);

    PRINT 'أُنشئ: device_registry';
END ELSE PRINT 'موجود: device_registry';
GO

/* ============================================================================
   4) employee_activation_codes — كود التفعيل
   ----------------------------------------------------------------------------
   `code_hash` لا الكود: بند 6 يمنع تخزينه نصّاً صريحاً. يُعرض للوكيل مرّة
   واحدة لحظة الإصدار ولا يُسترجع بعدها — وهذا مقصود، فاسترجاعه يعني أن
   من يقرأ القاعدة يستطيع تفعيل أي موظف.
   ============================================================================ */
IF OBJECT_ID('dbo.employee_activation_codes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_activation_codes (
        id             BIGINT IDENTITY(1,1) NOT NULL
                       CONSTRAINT PK_emp_codes PRIMARY KEY,
        agent_id       BIGINT       NOT NULL,
        employee_id    BIGINT       NOT NULL
                       CONSTRAINT FK_emp_codes_employee REFERENCES dbo.employees(id),
        phone          VARCHAR(20)  NOT NULL,

        code_hash      VARCHAR(255) NOT NULL,
        /* آخر 2 من الكود — ليتحقّق الوكيل أنه يقرأ الكود الصحيح، بلا كشفه. */
        code_hint      VARCHAR(4)   NULL,

        /* ACTIVE · USED · REVOKED · COMPROMISED · EXPIRED */
        status         VARCHAR(20)  NOT NULL
                       CONSTRAINT DF_emp_codes_status DEFAULT ('ACTIVE'),

        issued_by      BIGINT       NULL,
        issued_at      DATETIME2    NOT NULL
                       CONSTRAINT DF_emp_codes_issued DEFAULT (SYSUTCDATETIME()),
        expires_at     DATETIME2    NULL,
        used_at        DATETIME2    NULL,
        revoked_at     DATETIME2    NULL,
        revoked_reason NVARCHAR(300) NULL,

        /* الجهاز الذي اُستهلك عليه الكود — منه يُعرف خرق «جهاز ثانٍ». */
        bound_device_hash VARCHAR(64) NULL,
        attempts       INT          NOT NULL
                       CONSTRAINT DF_emp_codes_attempts DEFAULT (0)
    );

    /* كودٌ فعّال واحد لكل موظف: إصدار كود جديد يُلغي سابقه في الخدمة،
       وهذا الفهرس يمنع الحالتين معاً لو أخطأ الكود. */
    CREATE UNIQUE INDEX UX_emp_codes_active
        ON dbo.employee_activation_codes (employee_id) WHERE status = 'ACTIVE';

    CREATE INDEX IX_emp_codes_phone  ON dbo.employee_activation_codes (phone, status);
    CREATE INDEX IX_emp_codes_agent  ON dbo.employee_activation_codes (agent_id, issued_at);

    PRINT 'أُنشئ: employee_activation_codes';
END ELSE PRINT 'موجود: employee_activation_codes';
GO

/* ============================================================================
   5) employee_otps — رمز التحقّق
   ----------------------------------------------------------------------------
   مربوط بالموظف والهاتف **والجهاز ومحاولة التفعيل** (بند 11): رمزٌ صدر
   لجهاز لا يُستعمل من جهاز آخر ولو عُرف رقمه.
   ============================================================================ */
IF OBJECT_ID('dbo.employee_otps', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_otps (
        id             BIGINT IDENTITY(1,1) NOT NULL
                       CONSTRAINT PK_emp_otps PRIMARY KEY,
        employee_id    BIGINT       NOT NULL
                       CONSTRAINT FK_emp_otps_employee REFERENCES dbo.employees(id),
        activation_id  BIGINT       NULL,
        phone          VARCHAR(20)  NOT NULL,
        device_hash    VARCHAR(64)  NOT NULL,

        otp_hash       VARCHAR(255) NOT NULL,   /* 4 أرقام، مُجزّأة */
        /* PENDING · USED · EXPIRED · CANCELLED */
        status         VARCHAR(20)  NOT NULL
                       CONSTRAINT DF_emp_otps_status DEFAULT ('PENDING'),

        attempts       INT          NOT NULL CONSTRAINT DF_emp_otps_attempts DEFAULT (0),
        max_attempts   INT          NOT NULL CONSTRAINT DF_emp_otps_max      DEFAULT (5),

        created_at     DATETIME2    NOT NULL CONSTRAINT DF_emp_otps_created DEFAULT (SYSUTCDATETIME()),
        expires_at     DATETIME2    NOT NULL,
        used_at        DATETIME2    NULL
    );

    CREATE INDEX IX_emp_otps_lookup ON dbo.employee_otps (employee_id, status, expires_at);
    CREATE INDEX IX_emp_otps_phone  ON dbo.employee_otps (phone, created_at);

    PRINT 'أُنشئ: employee_otps';
END ELSE PRINT 'موجود: employee_otps';
GO

/* ============================================================================
   6) employee_devices — الربط التشغيلي للجهاز
   ============================================================================ */
IF OBJECT_ID('dbo.employee_devices', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_devices (
        id             BIGINT IDENTITY(1,1) NOT NULL
                       CONSTRAINT PK_emp_devices PRIMARY KEY,
        agent_id       BIGINT       NOT NULL,
        employee_id    BIGINT       NOT NULL
                       CONSTRAINT FK_emp_devices_employee REFERENCES dbo.employees(id),
        activation_id  BIGINT       NULL,

        device_hash    VARCHAR(64)  NOT NULL,
        platform       VARCHAR(20)  NULL,
        model          NVARCHAR(120) NULL,
        app_version    VARCHAR(30)  NULL,

        /* ACTIVE · REVOKED · REPLACED */
        status         VARCHAR(20)  NOT NULL
                       CONSTRAINT DF_emp_devices_status DEFAULT ('ACTIVE'),

        activated_at   DATETIME2    NOT NULL CONSTRAINT DF_emp_dev_act DEFAULT (SYSUTCDATETIME()),
        last_activity_at DATETIME2  NULL,
        revoked_at     DATETIME2    NULL,
        revoked_by     BIGINT       NULL,
        revoked_reason NVARCHAR(300) NULL
    );

    /* جهاز فعّال واحد لكل موظف (بند 14) — الحارس في القاعدة لا في الخدمة. */
    CREATE UNIQUE INDEX UX_emp_devices_active
        ON dbo.employee_devices (employee_id) WHERE status = 'ACTIVE';

    CREATE INDEX IX_emp_devices_hash  ON dbo.employee_devices (device_hash, status);
    CREATE INDEX IX_emp_devices_agent ON dbo.employee_devices (agent_id, status);

    PRINT 'أُنشئ: employee_devices';
END ELSE PRINT 'موجود: employee_devices';
GO

/* ============================================================================
   7) employee_sessions — جلسات الموظف
   ----------------------------------------------------------------------------
   جلسةٌ منفصلة عن `personal_access_tokens` تماماً: بند 22 يمنع ترقية جلسة
   موظف إلى مسؤول، وأضمن طريقة لمنعه أن يكون الرمزان في جدولين مختلفين
   يحرسهما حارسان مختلفان.
   ============================================================================ */
IF OBJECT_ID('dbo.employee_sessions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_sessions (
        id                BIGINT IDENTITY(1,1) NOT NULL
                          CONSTRAINT PK_emp_sessions PRIMARY KEY,
        agent_id          BIGINT       NOT NULL,
        employee_id       BIGINT       NOT NULL
                          CONSTRAINT FK_emp_sessions_employee REFERENCES dbo.employees(id),
        device_id         BIGINT       NULL
                          CONSTRAINT FK_emp_sessions_device REFERENCES dbo.employee_devices(id),
        device_hash       VARCHAR(64)  NOT NULL,

        /* نقطة البيع الفعّالة لهذه الجلسة — تُثبَّت عند الدخول أو بدء الوردية. */
        active_pos_id     INT          NULL,

        access_token_hash  VARCHAR(255) NOT NULL,
        refresh_token_hash VARCHAR(255) NULL,

        /* ACTIVE · REVOKED · EXPIRED */
        status            VARCHAR(20)  NOT NULL
                          CONSTRAINT DF_emp_sess_status DEFAULT ('ACTIVE'),

        created_at        DATETIME2    NOT NULL CONSTRAINT DF_emp_sess_created DEFAULT (SYSUTCDATETIME()),
        expires_at        DATETIME2    NULL,
        last_used_at      DATETIME2    NULL,
        revoked_at        DATETIME2    NULL,
        revoked_reason    NVARCHAR(300) NULL,
        ip_address        VARCHAR(64)  NULL
    );

    CREATE UNIQUE INDEX UX_emp_sess_token ON dbo.employee_sessions (access_token_hash);
    CREATE INDEX IX_emp_sess_employee     ON dbo.employee_sessions (employee_id, status);

    PRINT 'أُنشئ: employee_sessions';
END ELSE PRINT 'موجود: employee_sessions';
GO

/* ============================================================================
   8) employee_permissions — الممنوح صراحةً
   ----------------------------------------------------------------------------
   صفٌّ = صلاحية ممنوحة. لا صفّ = مرفوضة (بند 29). ولا عمود «مرفوض»: غياب
   الصفّ هو الرفض، فلا تحتمل الحالة تفسيرين.
   ============================================================================ */
IF OBJECT_ID('dbo.employee_permissions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_permissions (
        id             BIGINT IDENTITY(1,1) NOT NULL
                       CONSTRAINT PK_emp_perms PRIMARY KEY,
        employee_id    BIGINT      NOT NULL
                       CONSTRAINT FK_emp_perms_employee REFERENCES dbo.employees(id),
        permission_key VARCHAR(80) NOT NULL,
        granted_by     BIGINT      NULL,
        granted_at     DATETIME2   NOT NULL
                       CONSTRAINT DF_emp_perms_at DEFAULT (SYSUTCDATETIME())
    );

    CREATE UNIQUE INDEX UX_emp_perms ON dbo.employee_permissions (employee_id, permission_key);

    PRINT 'أُنشئ: employee_permissions';
END ELSE PRINT 'موجود: employee_permissions';
GO

/* ============================================================================
   9) transfer_attributions — نسبة العملية إلى منفّذها
   ----------------------------------------------------------------------------
   بديل إضافة أعمدة إلى `InternalEx`. العملية تبقى ملك المنظومة، ونسبتها
   إلى موظفٍ ونقطة بيع وجهاز تُحفظ هنا بجانبها بالرقم.
   ============================================================================ */
IF OBJECT_ID('dbo.transfer_attributions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.transfer_attributions (
        id               BIGINT IDENTITY(1,1) NOT NULL
                         CONSTRAINT PK_transfer_attr PRIMARY KEY,
        /* CREATED · DELIVERED */
        action           VARCHAR(20)  NOT NULL,
        transfer_number  VARCHAR(50)  NOT NULL,   /* InternalEx.Code */

        agent_id         BIGINT       NOT NULL,
        employee_id      BIGINT       NULL,       /* NULL = نفّذها الوكيل نفسه */
        point_of_sale_id INT          NULL,
        device_hash      VARCHAR(64)  NULL,
        session_id       BIGINT       NULL,

        amount           DECIMAL(18,3) NULL,
        occurred_at      DATETIME2    NOT NULL
                         CONSTRAINT DF_tattr_at DEFAULT (SYSUTCDATETIME())
    );

    /* نسبةٌ واحدة لكل (عملية، فعل): تكرار الطلب لا يُنشئ صفّاً ثانياً. */
    CREATE UNIQUE INDEX UX_tattr_number_action
        ON dbo.transfer_attributions (transfer_number, action);

    CREATE INDEX IX_tattr_employee ON dbo.transfer_attributions (employee_id, occurred_at);
    CREATE INDEX IX_tattr_pos      ON dbo.transfer_attributions (point_of_sale_id, occurred_at);
    CREATE INDEX IX_tattr_agent    ON dbo.transfer_attributions (agent_id, occurred_at);

    PRINT 'أُنشئ: transfer_attributions';
END ELSE PRINT 'موجود: transfer_attributions';
GO

/* ============================================================================
   10) audit_logs — سجلّ الأحداث الإدارية
   ============================================================================ */
IF OBJECT_ID('dbo.audit_logs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.audit_logs (
        id               BIGINT IDENTITY(1,1) NOT NULL
                         CONSTRAINT PK_audit_logs PRIMARY KEY,
        actor_user_id    BIGINT        NULL,
        actor_type       VARCHAR(20)   NULL,     /* AGENT · EMPLOYEE · SYSTEM */
        agent_id         BIGINT        NULL,
        employee_id      BIGINT        NULL,
        point_of_sale_id INT           NULL,
        device_hash      VARCHAR(64)   NULL,

        action           VARCHAR(60)   NOT NULL,
        entity_type      VARCHAR(60)   NULL,
        entity_id        VARCHAR(60)   NULL,
        old_value        NVARCHAR(1000) NULL,
        new_value        NVARCHAR(1000) NULL,

        ip_address       VARCHAR(64)   NULL,
        created_at       DATETIME2     NOT NULL
                         CONSTRAINT DF_audit_at DEFAULT (SYSUTCDATETIME())
    );

    CREATE INDEX IX_audit_agent    ON dbo.audit_logs (agent_id, created_at);
    CREATE INDEX IX_audit_employee ON dbo.audit_logs (employee_id, created_at);
    CREATE INDEX IX_audit_action   ON dbo.audit_logs (action, created_at);

    PRINT 'أُنشئ: audit_logs';
END ELSE PRINT 'موجود: audit_logs';
GO

/* ============================================================================
   11) security_logs — الأحداث الأمنية
   ----------------------------------------------------------------------------
   منفصل عن التدقيق عمداً (بند 48): محاولةُ اختراق يجب أن تُقرأ في جدولٍ
   صغير لا أن تضيع بين آلاف صفوف «أنشأ موظفاً».
   ============================================================================ */
IF OBJECT_ID('dbo.security_logs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.security_logs (
        id             BIGINT IDENTITY(1,1) NOT NULL
                       CONSTRAINT PK_security_logs PRIMARY KEY,

        /* OTP_FAILED · CODE_FAILED · CODE_OTHER_DEVICE · CODE_COMPROMISED
           ADMIN_LOGIN_FROM_EMPLOYEE_DEVICE · RATE_LIMIT · UNAUTHORIZED */
        event_type     VARCHAR(60)   NOT NULL,
        severity       VARCHAR(20)   NOT NULL
                       CONSTRAINT DF_seclog_sev DEFAULT ('WARNING'),

        agent_id       BIGINT        NULL,
        employee_id    BIGINT        NULL,
        phone          VARCHAR(20)   NULL,
        device_hash    VARCHAR(64)   NULL,

        detail         NVARCHAR(1000) NULL,
        ip_address     VARCHAR(64)   NULL,
        created_at     DATETIME2     NOT NULL
                       CONSTRAINT DF_seclog_at DEFAULT (SYSUTCDATETIME())
    );

    CREATE INDEX IX_seclog_type   ON dbo.security_logs (event_type, created_at);
    CREATE INDEX IX_seclog_agent  ON dbo.security_logs (agent_id, created_at);
    CREATE INDEX IX_seclog_device ON dbo.security_logs (device_hash, created_at);

    PRINT 'أُنشئ: security_logs';
END ELSE PRINT 'موجود: security_logs';
GO

/* ============================================================================
   التحقّق
   ============================================================================ */
SELECT name AS [الجدول], create_date AS [أُنشئ]
FROM sys.tables
WHERE name IN ('employees','employee_point_of_sales','device_registry',
               'employee_activation_codes','employee_otps','employee_devices',
               'employee_sessions','employee_permissions','transfer_attributions',
               'audit_logs','security_logs')
ORDER BY name;
GO
