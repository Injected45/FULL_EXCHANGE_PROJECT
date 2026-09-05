/* ============================================================================
   نشرة قاعدة البيانات — من 1 إلى 5 سبتمبر 2026
   من قاعدة التجربة إلى القاعدة الرئيسية.

   وُلِّد هذا الملف **من مخطّط قاعدة التجربة الحيّ** لا من الذاكرة ولا من ملفات
   كُتبت سابقاً، فما ينتج عنه في القاعدة الرئيسية مطابقٌ لما جُرِّب عليه فعلاً.

   ── ما في هذه النشرة ───────────────────────────────────────────────────────
   **ستّة وعشرون جدولاً جديداً بفهارسها وقيودها، ولا شيء غيرها.**

       دفتر تسليم الوكيل (2 سبتمبر)
         agent_incoming_transfers      متابعة تسليم الحوالات الواردة
         transfer_status_history       سجلّ تغيّر حالة التسليم

       هوية الشركة داخل التطبيق (2 سبتمبر)
         tenant_branding               الاسم والشعار والألوان
         tenant_branding_audit         أثر كل تعديل عليها

       الموظفون ونقاط البيع (3 سبتمبر)
         employees                     الموظف — وليس صفّاً في users
         employee_point_of_sales       ربط الموظف بنقاط البيع
         device_registry               تصنيف دائم للأجهزة (لا يُحذف منه شيء)
         employee_activation_codes     أكواد التفعيل
         employee_otps                 رموز التحقّق
         employee_devices              ربط تشغيليّ للجهاز، يُلغى ويُعاد
         employee_sessions             جلسة الموظف — منفصلة عن Sanctum
         employee_permissions          الصلاحيات الممنوحة (صفوف لا أعمدة)
         transfer_attributions         من أنشأ ومن سلّم، بجانب الدفتر لا داخله
         audit_logs                    أثر العمليات
         security_logs                 أثر المحاولات الأمنية

       الدردشة (5 سبتمبر)
         chat_threads                  المحادثة — مع الإدارة أو مع موظّف
         chat_messages                 الرسائل، ومرفقاتها وردودها
         chat_reads                    إلى أين وصل وقرأ كلُّ طرف
         chat_reactions                التفاعلات — رمزٌ لكل شخص
         chat_settings                 كتم · تثبيت · أرشفة · قفل
         chat_stars                    الرسائل المحفوظة
         chat_typing                   «يكتب الآن» — لحظيّ ينتهي وحده

       صندوق الموظف (3 سبتمبر)
         employee_cashboxes            الصندوق
         employee_shifts               الورديات
         employee_cashbox_entries      الحركات — الرصيد يُحسب منها ولا يُخزَّن
         employee_shift_closings       إقفال الوردية

   ── ما لا يفعله هذا السكربت ────────────────────────────────────────────────
   • **لا يعدّل جدولاً قائماً في المنظومة.** فُحص بـ sys.tables: لا جدولٍ
     أُنشئ قبل 1 سبتمبر تغيّر تاريخُ تعديله بعده — العدد **صفر**.
   • **لا يُنشئ ولا يعدّل إجراءً مخزّناً ولا مشهداً ولا دالّة ولا مشغّلاً**
     (trigger). فُحص بـ sys.objects — العدد **صفر**.
   • **لا يمسّ المال.** لا wallet ولا InternalEx ولا ExternalEx ولا
     ExchangeAccData ولا EX24AccSafeActivityTb ولا AccountsTb ولا
     AuthorizedUsers ولا users. ولا فهرس عليها — الفهرس تغييرٌ في جدول ماليّ
     ولو لم يغيّر رقماً.
   • **لا مفتاح أجنبي إلى جداول المنظومة.** المفاتيح الأربعة والعشرون كلّها **بين
     الجداول الجديدة وحدها** (فُحصت بالاسم). فقيدٌ من جدولٍ ملحق إلى شجرة
     الحسابات كان يجعل تعديلاً مالياً يفشل بسبب صفٍّ في جدول موظفين.
   • **لا DROP ولا UPDATE ولا DELETE ولا INSERT.** بنيةٌ فقط. لا صفّ واحد من
     بيانات التجربة ينتقل، ولا رصيد عميل أو وكيل أو خزينة أو موظف يتغيّر —
     الجداول تصل **فارغة**، وتمتلئ من عمل المستخدمين.
   • **قابل لإعادة التنفيذ**: كل كائن محروس بـ IF NOT EXISTS. تشغيله مرّتين
     لا يُنشئ شيئاً مرّتين ولا يفشل. (نُفِّذ على قاعدة التجربة بعد توليده فلم
     يُغيّر شيئاً — وهو الدليل.)

   ── قبل التنفيذ ────────────────────────────────────────────────────────────
   1. **نسخة احتياطية كاملة.** عادة لا استثناء.
   2. USE [اسم القاعدة الرئيسية] أولاً — السكربت لا يختار قاعدة بنفسه عمداً،
      فسطرٌ يختار القاعدة الخطأ يُنفَّذ في مكانٍ لم يُقصد.
   3. نفّذه في SSMS بوضع SQLCMD مطفأ — فيه GO عادية لا غير.
   4. راجع قسم التحقّق في آخره: يجب أن يطبع 26 جدولاً، كلّها فارغة، وبلا
      مفتاح أجنبي خارج إلى المنظومة.

   ── خارج قاعدة البيانات ────────────────────────────────────────────────────
   • نشر كود الـ API (المتحكّمات والخدمات والمسارات).
   • مجلّد الشعارات: storage/app/private/branding موجوداً وقابلاً للكتابة من
     مستخدم PHP، وإلا فشل رفع الشعار وحده.
   ============================================================================ */

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* ---------- agent_incoming_transfers ---------- */
IF OBJECT_ID('dbo.agent_incoming_transfers', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[agent_incoming_transfers] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [transfer_number] VARCHAR(50) NOT NULL,
        [beneficiary_name] NVARCHAR(200) NULL,
        [beneficiary_phone] VARCHAR(30) NULL,
        [sender_name] NVARCHAR(200) NULL,
        [amount] DECIMAL(18,3) NULL,
        [currency_id] INT NULL,
        [branch_delivered_id] INT NULL,
        [status] VARCHAR(30) CONSTRAINT [DF_ait_status] DEFAULT ('PENDING_DELIVERY') NOT NULL,
        [received_at] DATETIME2 CONSTRAINT [DF_ait_received] DEFAULT (sysutcdatetime()) NOT NULL,
        [delivered_at] DATETIME2 NULL,
        [delivered_by] BIGINT NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_ait_created] DEFAULT (sysutcdatetime()) NOT NULL,
        [updated_at] DATETIME2 CONSTRAINT [DF_ait_updated] DEFAULT (sysutcdatetime()) NOT NULL,
        [core_confirm_type] INT NULL,
        [core_status_label] NVARCHAR(50) NULL,
        [core_synced_at] DATETIME2 NULL,
        [commission] DECIMAL(18,3) NULL,
        [sent_at] DATETIME2 NULL,
        [sender_branch_name] NVARCHAR(200) NULL,
        [core_missing_at] DATETIME2 NULL,
        CONSTRAINT [PK__agent_in__3213E83F4D85036F] PRIMARY KEY ([id])
    );
END;
GO

/* أعمدة أُضيفت بعد أوّل إنشاء — تُستدرك على قاعدةٍ نُشر إليها سابقاً. */
IF COL_LENGTH('dbo.agent_incoming_transfers', 'beneficiary_name') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [beneficiary_name] NVARCHAR(200) NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'beneficiary_phone') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [beneficiary_phone] VARCHAR(30) NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'sender_name') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [sender_name] NVARCHAR(200) NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'amount') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [amount] DECIMAL(18,3) NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'currency_id') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [currency_id] INT NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'branch_delivered_id') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [branch_delivered_id] INT NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'status') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [status] VARCHAR(30) CONSTRAINT [DF_ait_status] DEFAULT ('PENDING_DELIVERY') NOT NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'received_at') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [received_at] DATETIME2 CONSTRAINT [DF_ait_received] DEFAULT (sysutcdatetime()) NOT NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'delivered_at') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [delivered_at] DATETIME2 NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'delivered_by') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [delivered_by] BIGINT NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'created_at') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [created_at] DATETIME2 CONSTRAINT [DF_ait_created] DEFAULT (sysutcdatetime()) NOT NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'updated_at') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [updated_at] DATETIME2 CONSTRAINT [DF_ait_updated] DEFAULT (sysutcdatetime()) NOT NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'core_confirm_type') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [core_confirm_type] INT NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'core_status_label') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [core_status_label] NVARCHAR(50) NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'core_synced_at') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [core_synced_at] DATETIME2 NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'commission') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [commission] DECIMAL(18,3) NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'sent_at') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [sent_at] DATETIME2 NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'sender_branch_name') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [sender_branch_name] NVARCHAR(200) NULL;
GO
IF COL_LENGTH('dbo.agent_incoming_transfers', 'core_missing_at') IS NULL
    ALTER TABLE dbo.[agent_incoming_transfers] ADD [core_missing_at] DATETIME2 NULL;
GO

/* ---------- transfer_status_history ---------- */
IF OBJECT_ID('dbo.transfer_status_history', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[transfer_status_history] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [transfer_id] BIGINT NOT NULL,
        [transfer_number] VARCHAR(50) NOT NULL,
        [old_status] VARCHAR(30) NULL,
        [new_status] VARCHAR(30) NOT NULL,
        [changed_by] BIGINT NULL,
        [changed_at] DATETIME2 CONSTRAINT [DF_tsh_changed] DEFAULT (sysutcdatetime()) NOT NULL,
        [ip_address] VARCHAR(64) NULL,
        [device_id] VARCHAR(128) NULL,
        [session_id] VARCHAR(128) NULL,
        [notes] NVARCHAR(500) NULL,
        CONSTRAINT [PK__transfer__3213E83FB8E38223] PRIMARY KEY ([id])
    );
END;
GO

/* أعمدة أُضيفت بعد أوّل إنشاء — تُستدرك على قاعدةٍ نُشر إليها سابقاً. */
IF COL_LENGTH('dbo.transfer_status_history', 'old_status') IS NULL
    ALTER TABLE dbo.[transfer_status_history] ADD [old_status] VARCHAR(30) NULL;
GO
IF COL_LENGTH('dbo.transfer_status_history', 'changed_by') IS NULL
    ALTER TABLE dbo.[transfer_status_history] ADD [changed_by] BIGINT NULL;
GO
IF COL_LENGTH('dbo.transfer_status_history', 'changed_at') IS NULL
    ALTER TABLE dbo.[transfer_status_history] ADD [changed_at] DATETIME2 CONSTRAINT [DF_tsh_changed] DEFAULT (sysutcdatetime()) NOT NULL;
GO
IF COL_LENGTH('dbo.transfer_status_history', 'ip_address') IS NULL
    ALTER TABLE dbo.[transfer_status_history] ADD [ip_address] VARCHAR(64) NULL;
GO
IF COL_LENGTH('dbo.transfer_status_history', 'device_id') IS NULL
    ALTER TABLE dbo.[transfer_status_history] ADD [device_id] VARCHAR(128) NULL;
GO
IF COL_LENGTH('dbo.transfer_status_history', 'session_id') IS NULL
    ALTER TABLE dbo.[transfer_status_history] ADD [session_id] VARCHAR(128) NULL;
GO
IF COL_LENGTH('dbo.transfer_status_history', 'notes') IS NULL
    ALTER TABLE dbo.[transfer_status_history] ADD [notes] NVARCHAR(500) NULL;
GO

/* ---------- tenant_branding ---------- */
IF OBJECT_ID('dbo.tenant_branding', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tenant_branding] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [company_account_id] BIGINT NOT NULL,
        [company_name_ar] NVARCHAR(200) NULL,
        [company_name_en] NVARCHAR(200) NULL,
        [logo_path] NVARCHAR(400) NULL,
        [primary_color] VARCHAR(9) NULL,
        [secondary_color] VARCHAR(9) NULL,
        [background_color] VARCHAR(9) NULL,
        [theme_key] VARCHAR(40) CONSTRAINT [DF_tb_theme] DEFAULT ('classic_green') NOT NULL,
        [is_active] BIT CONSTRAINT [DF_tb_active] DEFAULT ((1)) NOT NULL,
        [branding_version] INT CONSTRAINT [DF_tb_version] DEFAULT ((1)) NOT NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_tb_created] DEFAULT (sysutcdatetime()) NOT NULL,
        [updated_at] DATETIME2 CONSTRAINT [DF_tb_updated] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK__tenant_b__3213E83FC91FFC08] PRIMARY KEY ([id])
    );
END;
GO

/* أعمدة أُضيفت بعد أوّل إنشاء — تُستدرك على قاعدةٍ نُشر إليها سابقاً. */
IF COL_LENGTH('dbo.tenant_branding', 'company_name_ar') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [company_name_ar] NVARCHAR(200) NULL;
GO
IF COL_LENGTH('dbo.tenant_branding', 'company_name_en') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [company_name_en] NVARCHAR(200) NULL;
GO
IF COL_LENGTH('dbo.tenant_branding', 'logo_path') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [logo_path] NVARCHAR(400) NULL;
GO
IF COL_LENGTH('dbo.tenant_branding', 'primary_color') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [primary_color] VARCHAR(9) NULL;
GO
IF COL_LENGTH('dbo.tenant_branding', 'secondary_color') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [secondary_color] VARCHAR(9) NULL;
GO
IF COL_LENGTH('dbo.tenant_branding', 'background_color') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [background_color] VARCHAR(9) NULL;
GO
IF COL_LENGTH('dbo.tenant_branding', 'theme_key') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [theme_key] VARCHAR(40) CONSTRAINT [DF_tb_theme] DEFAULT ('classic_green') NOT NULL;
GO
IF COL_LENGTH('dbo.tenant_branding', 'is_active') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [is_active] BIT CONSTRAINT [DF_tb_active] DEFAULT ((1)) NOT NULL;
GO
IF COL_LENGTH('dbo.tenant_branding', 'branding_version') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [branding_version] INT CONSTRAINT [DF_tb_version] DEFAULT ((1)) NOT NULL;
GO
IF COL_LENGTH('dbo.tenant_branding', 'created_at') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [created_at] DATETIME2 CONSTRAINT [DF_tb_created] DEFAULT (sysutcdatetime()) NOT NULL;
GO
IF COL_LENGTH('dbo.tenant_branding', 'updated_at') IS NULL
    ALTER TABLE dbo.[tenant_branding] ADD [updated_at] DATETIME2 CONSTRAINT [DF_tb_updated] DEFAULT (sysutcdatetime()) NOT NULL;
GO

/* ---------- tenant_branding_audit ---------- */
IF OBJECT_ID('dbo.tenant_branding_audit', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tenant_branding_audit] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [company_account_id] BIGINT NOT NULL,
        [changed_by] BIGINT NULL,
        [changed_at] DATETIME2 CONSTRAINT [DF_tba_at] DEFAULT (sysutcdatetime()) NOT NULL,
        [field_name] VARCHAR(60) NOT NULL,
        [old_value] NVARCHAR(400) NULL,
        [new_value] NVARCHAR(400) NULL,
        [ip_address] VARCHAR(64) NULL,
        [device_id] VARCHAR(128) NULL,
        CONSTRAINT [PK__tenant_b__3213E83F8494094F] PRIMARY KEY ([id])
    );
END;
GO

/* أعمدة أُضيفت بعد أوّل إنشاء — تُستدرك على قاعدةٍ نُشر إليها سابقاً. */
IF COL_LENGTH('dbo.tenant_branding_audit', 'changed_by') IS NULL
    ALTER TABLE dbo.[tenant_branding_audit] ADD [changed_by] BIGINT NULL;
GO
IF COL_LENGTH('dbo.tenant_branding_audit', 'changed_at') IS NULL
    ALTER TABLE dbo.[tenant_branding_audit] ADD [changed_at] DATETIME2 CONSTRAINT [DF_tba_at] DEFAULT (sysutcdatetime()) NOT NULL;
GO
IF COL_LENGTH('dbo.tenant_branding_audit', 'old_value') IS NULL
    ALTER TABLE dbo.[tenant_branding_audit] ADD [old_value] NVARCHAR(400) NULL;
GO
IF COL_LENGTH('dbo.tenant_branding_audit', 'new_value') IS NULL
    ALTER TABLE dbo.[tenant_branding_audit] ADD [new_value] NVARCHAR(400) NULL;
GO
IF COL_LENGTH('dbo.tenant_branding_audit', 'ip_address') IS NULL
    ALTER TABLE dbo.[tenant_branding_audit] ADD [ip_address] VARCHAR(64) NULL;
GO
IF COL_LENGTH('dbo.tenant_branding_audit', 'device_id') IS NULL
    ALTER TABLE dbo.[tenant_branding_audit] ADD [device_id] VARCHAR(128) NULL;
GO

/* ---------- employees ---------- */
IF OBJECT_ID('dbo.employees', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employees] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [agent_account_id] BIGINT NULL,
        [branch_id] INT NULL,
        [full_name] NVARCHAR(200) NOT NULL,
        [phone] VARCHAR(20) NOT NULL,
        [national_number] VARCHAR(30) NULL,
        [notes] NVARCHAR(500) NULL,
        [status] VARCHAR(30) CONSTRAINT [DF_emp_status] DEFAULT ('PENDING_ACTIVATION') NOT NULL,
        [last_login_at] DATETIME2 NULL,
        [last_activity_at] DATETIME2 NULL,
        [activated_at] DATETIME2 NULL,
        [created_by] BIGINT NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_emp_created] DEFAULT (sysutcdatetime()) NOT NULL,
        [updated_at] DATETIME2 CONSTRAINT [DF_emp_updated] DEFAULT (sysutcdatetime()) NOT NULL,
        [deleted_at] DATETIME2 NULL,
        CONSTRAINT [PK_employees] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- employee_point_of_sales ---------- */
IF OBJECT_ID('dbo.employee_point_of_sales', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employee_point_of_sales] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [employee_id] BIGINT NOT NULL,
        [point_of_sale_id] INT NOT NULL,
        [is_primary] BIT CONSTRAINT [DF_emp_pos_primary] DEFAULT ((0)) NOT NULL,
        [is_active] BIT CONSTRAINT [DF_emp_pos_active] DEFAULT ((1)) NOT NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_emp_pos_created] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_emp_pos] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- device_registry ---------- */
IF OBJECT_ID('dbo.device_registry', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[device_registry] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [device_hash] VARCHAR(64) NOT NULL,
        [classification] VARCHAR(30) NOT NULL,
        [first_seen_at] DATETIME2 CONSTRAINT [DF_dr_first] DEFAULT (sysutcdatetime()) NOT NULL,
        [last_seen_at] DATETIME2 CONSTRAINT [DF_dr_last] DEFAULT (sysutcdatetime()) NOT NULL,
        [first_agent_id] BIGINT NULL,
        [first_employee_id] BIGINT NULL,
        [platform] VARCHAR(20) NULL,
        [model] NVARCHAR(120) NULL,
        CONSTRAINT [PK_device_registry] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- employee_activation_codes ---------- */
IF OBJECT_ID('dbo.employee_activation_codes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employee_activation_codes] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [employee_id] BIGINT NOT NULL,
        [phone] VARCHAR(20) NOT NULL,
        [code_hash] VARCHAR(255) NOT NULL,
        [code_hint] VARCHAR(4) NULL,
        [status] VARCHAR(20) CONSTRAINT [DF_emp_codes_status] DEFAULT ('ACTIVE') NOT NULL,
        [issued_by] BIGINT NULL,
        [issued_at] DATETIME2 CONSTRAINT [DF_emp_codes_issued] DEFAULT (sysutcdatetime()) NOT NULL,
        [expires_at] DATETIME2 NULL,
        [used_at] DATETIME2 NULL,
        [revoked_at] DATETIME2 NULL,
        [revoked_reason] NVARCHAR(300) NULL,
        [bound_device_hash] VARCHAR(64) NULL,
        [attempts] INT CONSTRAINT [DF_emp_codes_attempts] DEFAULT ((0)) NOT NULL,
        CONSTRAINT [PK_emp_codes] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- employee_otps ---------- */
IF OBJECT_ID('dbo.employee_otps', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employee_otps] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [employee_id] BIGINT NOT NULL,
        [activation_id] BIGINT NULL,
        [phone] VARCHAR(20) NOT NULL,
        [device_hash] VARCHAR(64) NOT NULL,
        [otp_hash] VARCHAR(255) NOT NULL,
        [status] VARCHAR(20) CONSTRAINT [DF_emp_otps_status] DEFAULT ('PENDING') NOT NULL,
        [attempts] INT CONSTRAINT [DF_emp_otps_attempts] DEFAULT ((0)) NOT NULL,
        [max_attempts] INT CONSTRAINT [DF_emp_otps_max] DEFAULT ((5)) NOT NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_emp_otps_created] DEFAULT (sysutcdatetime()) NOT NULL,
        [expires_at] DATETIME2 NOT NULL,
        [used_at] DATETIME2 NULL,
        CONSTRAINT [PK_emp_otps] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- employee_devices ---------- */
IF OBJECT_ID('dbo.employee_devices', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employee_devices] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [employee_id] BIGINT NOT NULL,
        [activation_id] BIGINT NULL,
        [device_hash] VARCHAR(64) NOT NULL,
        [platform] VARCHAR(20) NULL,
        [model] NVARCHAR(120) NULL,
        [app_version] VARCHAR(30) NULL,
        [status] VARCHAR(20) CONSTRAINT [DF_emp_devices_status] DEFAULT ('ACTIVE') NOT NULL,
        [activated_at] DATETIME2 CONSTRAINT [DF_emp_dev_act] DEFAULT (sysutcdatetime()) NOT NULL,
        [last_activity_at] DATETIME2 NULL,
        [revoked_at] DATETIME2 NULL,
        [revoked_by] BIGINT NULL,
        [revoked_reason] NVARCHAR(300) NULL,
        CONSTRAINT [PK_emp_devices] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- employee_sessions ---------- */
IF OBJECT_ID('dbo.employee_sessions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employee_sessions] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [employee_id] BIGINT NOT NULL,
        [device_id] BIGINT NULL,
        [device_hash] VARCHAR(64) NOT NULL,
        [active_pos_id] INT NULL,
        [access_token_hash] VARCHAR(255) NOT NULL,
        [refresh_token_hash] VARCHAR(255) NULL,
        [status] VARCHAR(20) CONSTRAINT [DF_emp_sess_status] DEFAULT ('ACTIVE') NOT NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_emp_sess_created] DEFAULT (sysutcdatetime()) NOT NULL,
        [expires_at] DATETIME2 NULL,
        [last_used_at] DATETIME2 NULL,
        [revoked_at] DATETIME2 NULL,
        [revoked_reason] NVARCHAR(300) NULL,
        [ip_address] VARCHAR(64) NULL,
        CONSTRAINT [PK_emp_sessions] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- employee_permissions ---------- */
IF OBJECT_ID('dbo.employee_permissions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employee_permissions] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [employee_id] BIGINT NOT NULL,
        [permission_key] VARCHAR(80) NOT NULL,
        [granted_by] BIGINT NULL,
        [granted_at] DATETIME2 CONSTRAINT [DF_emp_perms_at] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_emp_perms] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- transfer_attributions ---------- */
IF OBJECT_ID('dbo.transfer_attributions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[transfer_attributions] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [action] VARCHAR(20) NOT NULL,
        [transfer_number] VARCHAR(50) NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [employee_id] BIGINT NULL,
        [point_of_sale_id] INT NULL,
        [device_hash] VARCHAR(64) NULL,
        [session_id] BIGINT NULL,
        [amount] DECIMAL(18,3) NULL,
        [occurred_at] DATETIME2 CONSTRAINT [DF_tattr_at] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_transfer_attr] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- audit_logs ---------- */
IF OBJECT_ID('dbo.audit_logs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[audit_logs] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [actor_user_id] BIGINT NULL,
        [actor_type] VARCHAR(20) NULL,
        [agent_id] BIGINT NULL,
        [employee_id] BIGINT NULL,
        [point_of_sale_id] INT NULL,
        [device_hash] VARCHAR(64) NULL,
        [action] VARCHAR(60) NOT NULL,
        [entity_type] VARCHAR(60) NULL,
        [entity_id] VARCHAR(60) NULL,
        [old_value] NVARCHAR(1000) NULL,
        [new_value] NVARCHAR(1000) NULL,
        [ip_address] VARCHAR(64) NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_audit_at] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_audit_logs] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- security_logs ---------- */
IF OBJECT_ID('dbo.security_logs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[security_logs] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [event_type] VARCHAR(60) NOT NULL,
        [severity] VARCHAR(20) CONSTRAINT [DF_seclog_sev] DEFAULT ('WARNING') NOT NULL,
        [agent_id] BIGINT NULL,
        [employee_id] BIGINT NULL,
        [phone] VARCHAR(20) NULL,
        [device_hash] VARCHAR(64) NULL,
        [detail] NVARCHAR(1000) NULL,
        [ip_address] VARCHAR(64) NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_seclog_at] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_security_logs] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- employee_cashboxes ---------- */
IF OBJECT_ID('dbo.employee_cashboxes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employee_cashboxes] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [employee_id] BIGINT NOT NULL,
        [point_of_sale_id] INT NULL,
        [currency_code] VARCHAR(10) CONSTRAINT [DF_cashbox_currency] DEFAULT ('LYD') NOT NULL,
        [is_active] BIT CONSTRAINT [DF_cashbox_active] DEFAULT ((1)) NOT NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_cashbox_created] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_emp_cashboxes] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- employee_shifts ---------- */
IF OBJECT_ID('dbo.employee_shifts', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employee_shifts] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [employee_id] BIGINT NOT NULL,
        [cashbox_id] BIGINT NOT NULL,
        [point_of_sale_id] INT NULL,
        [opening_cash] DECIMAL(18,3) CONSTRAINT [DF_shift_opening] DEFAULT ((0)) NOT NULL,
        [status] VARCHAR(20) CONSTRAINT [DF_shift_status] DEFAULT ('OPEN') NOT NULL,
        [started_at] DATETIME2 CONSTRAINT [DF_shift_started] DEFAULT (sysutcdatetime()) NOT NULL,
        [ended_at] DATETIME2 NULL,
        [device_hash] VARCHAR(64) NULL,
        CONSTRAINT [PK_emp_shifts] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- employee_cashbox_entries ---------- */
IF OBJECT_ID('dbo.employee_cashbox_entries', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employee_cashbox_entries] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [employee_id] BIGINT NOT NULL,
        [cashbox_id] BIGINT NOT NULL,
        [shift_id] BIGINT NULL,
        [point_of_sale_id] INT NULL,
        [transaction_type] VARCHAR(40) NOT NULL,
        [reference_type] VARCHAR(40) NULL,
        [reference_id] VARCHAR(60) NULL,
        [amount] DECIMAL(18,3) NOT NULL,
        [direction] VARCHAR(3) NOT NULL,
        [currency_code] VARCHAR(10) CONSTRAINT [DF_entry_currency] DEFAULT ('LYD') NOT NULL,
        [notes] NVARCHAR(500) NULL,
        [reversal_of] BIGINT NULL,
        [is_reversed] BIT CONSTRAINT [DF_entry_reversed] DEFAULT ((0)) NOT NULL,
        [client_ref] VARCHAR(80) NULL,
        [device_hash] VARCHAR(64) NULL,
        [created_by] BIGINT NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_entry_created] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_emp_entries] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- employee_shift_closings ---------- */
IF OBJECT_ID('dbo.employee_shift_closings', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[employee_shift_closings] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [shift_id] BIGINT NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [employee_id] BIGINT NOT NULL,
        [opening_cash] DECIMAL(18,3) NOT NULL,
        [cash_in] DECIMAL(18,3) NOT NULL,
        [cash_out] DECIMAL(18,3) NOT NULL,
        [expected_cash] DECIMAL(18,3) NOT NULL,
        [actual_cash] DECIMAL(18,3) NOT NULL,
        [difference] DECIMAL(18,3) NOT NULL,
        [result] VARCHAR(20) NOT NULL,
        [notes] NVARCHAR(500) NULL,
        [closed_by] BIGINT NULL,
        [closed_at] DATETIME2 CONSTRAINT [DF_closing_at] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_shift_closings] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- chat_threads ---------- */
IF OBJECT_ID('dbo.chat_threads', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[chat_threads] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [agent_id] BIGINT NOT NULL,
        [kind] VARCHAR(20) NOT NULL,
        [employee_id] BIGINT NULL,
        [last_message_at] DATETIME2 NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_chat_threads_created] DEFAULT (sysutcdatetime()) NOT NULL,
        [updated_at] DATETIME2 CONSTRAINT [DF_chat_threads_updated] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_chat_threads] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- chat_messages ---------- */
IF OBJECT_ID('dbo.chat_messages', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[chat_messages] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [thread_id] BIGINT NOT NULL,
        [sender_kind] VARCHAR(20) NOT NULL,
        [sender_id] BIGINT NOT NULL,
        [sender_name] NVARCHAR(200) NULL,
        [body] NVARCHAR(2000) NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_chat_messages_created] DEFAULT (sysutcdatetime()) NOT NULL,
        [reply_to_id] BIGINT NULL,
        [deleted_at] DATETIME2 NULL,
        [attachment_path] VARCHAR(200) NULL,
        [attachment_name] NVARCHAR(255) NULL,
        [attachment_mime] VARCHAR(120) NULL,
        [attachment_size] INT NULL,
        [attachment_kind] VARCHAR(20) NULL,
        [edited_at] DATETIME2 NULL,
        [pinned_at] DATETIME2 NULL,
        [pinned_by] VARCHAR(20) NULL,
        [client_id] VARCHAR(64) NULL,
        CONSTRAINT [PK_chat_messages] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- chat_reads ---------- */
IF OBJECT_ID('dbo.chat_reads', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[chat_reads] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [thread_id] BIGINT NOT NULL,
        [reader_kind] VARCHAR(20) NOT NULL,
        [reader_id] BIGINT NOT NULL,
        [last_read_message_id] BIGINT CONSTRAINT [DF_chat_reads_last] DEFAULT ((0)) NOT NULL,
        [updated_at] DATETIME2 CONSTRAINT [DF_chat_reads_updated] DEFAULT (sysutcdatetime()) NOT NULL,
        [last_delivered_message_id] BIGINT CONSTRAINT [DF_chat_reads_delivered] DEFAULT ((0)) NOT NULL,
        CONSTRAINT [PK_chat_reads] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- chat_reactions ---------- */
IF OBJECT_ID('dbo.chat_reactions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[chat_reactions] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [message_id] BIGINT NOT NULL,
        [thread_id] BIGINT NOT NULL,
        [actor_kind] VARCHAR(20) NOT NULL,
        [actor_id] BIGINT NOT NULL,
        [emoji] NVARCHAR(16) NOT NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_chat_react_created] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_chat_reactions] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- chat_settings ---------- */
IF OBJECT_ID('dbo.chat_settings', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[chat_settings] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [thread_id] BIGINT NOT NULL,
        [actor_kind] VARCHAR(20) NOT NULL,
        [actor_id] BIGINT NOT NULL,
        [muted_until] DATETIME2 NULL,
        [pinned_at] DATETIME2 NULL,
        [archived_at] DATETIME2 NULL,
        [locked] BIT CONSTRAINT [DF_chat_set_locked] DEFAULT ((0)) NOT NULL,
        [forced_unread] BIT CONSTRAINT [DF_chat_set_unread] DEFAULT ((0)) NOT NULL,
        [updated_at] DATETIME2 CONSTRAINT [DF_chat_set_updated] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_chat_settings] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- chat_stars ---------- */
IF OBJECT_ID('dbo.chat_stars', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[chat_stars] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [message_id] BIGINT NOT NULL,
        [thread_id] BIGINT NOT NULL,
        [actor_kind] VARCHAR(20) NOT NULL,
        [actor_id] BIGINT NOT NULL,
        [created_at] DATETIME2 CONSTRAINT [DF_chat_star_created] DEFAULT (sysutcdatetime()) NOT NULL,
        CONSTRAINT [PK_chat_stars] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- chat_typing ---------- */
IF OBJECT_ID('dbo.chat_typing', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[chat_typing] (
        [id] BIGINT IDENTITY(1,1) NOT NULL,
        [thread_id] BIGINT NOT NULL,
        [actor_kind] VARCHAR(20) NOT NULL,
        [actor_id] BIGINT NOT NULL,
        [actor_name] NVARCHAR(200) NULL,
        [state] VARCHAR(20) NOT NULL,
        [expires_at] DATETIME2 NOT NULL,
        CONSTRAINT [PK_chat_typing] PRIMARY KEY ([id])
    );
END;
GO

/* ---------- الفهارس ---------- */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_ait_agent_transfer' AND object_id = OBJECT_ID('dbo.agent_incoming_transfers'))
    CREATE UNIQUE INDEX [UX_ait_agent_transfer] ON dbo.[agent_incoming_transfers] ([agent_id], [transfer_number]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ait_agent_status' AND object_id = OBJECT_ID('dbo.agent_incoming_transfers'))
    CREATE INDEX [IX_ait_agent_status] ON dbo.[agent_incoming_transfers] ([agent_id], [status]) INCLUDE ([transfer_number], [delivered_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ait_phone' AND object_id = OBJECT_ID('dbo.agent_incoming_transfers'))
    CREATE INDEX [IX_ait_phone] ON dbo.[agent_incoming_transfers] ([beneficiary_phone]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ait_created' AND object_id = OBJECT_ID('dbo.agent_incoming_transfers'))
    CREATE INDEX [IX_ait_created] ON dbo.[agent_incoming_transfers] ([created_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ait_agent_core' AND object_id = OBJECT_ID('dbo.agent_incoming_transfers'))
    CREATE INDEX [IX_ait_agent_core] ON dbo.[agent_incoming_transfers] ([agent_id], [core_confirm_type]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tsh_transfer' AND object_id = OBJECT_ID('dbo.transfer_status_history'))
    CREATE INDEX [IX_tsh_transfer] ON dbo.[transfer_status_history] ([transfer_id], [changed_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tsh_number' AND object_id = OBJECT_ID('dbo.transfer_status_history'))
    CREATE INDEX [IX_tsh_number] ON dbo.[transfer_status_history] ([transfer_number]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_tb_company' AND object_id = OBJECT_ID('dbo.tenant_branding'))
    CREATE UNIQUE INDEX [UX_tb_company] ON dbo.[tenant_branding] ([company_account_id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tba_company' AND object_id = OBJECT_ID('dbo.tenant_branding_audit'))
    CREATE INDEX [IX_tba_company] ON dbo.[tenant_branding_audit] ([company_account_id], [changed_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_emp_agent_phone' AND object_id = OBJECT_ID('dbo.employees'))
    CREATE UNIQUE INDEX [UX_emp_agent_phone] ON dbo.[employees] ([agent_id], [phone]) WHERE ([deleted_at] IS NULL);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_emp_agent_status' AND object_id = OBJECT_ID('dbo.employees'))
    CREATE INDEX [IX_emp_agent_status] ON dbo.[employees] ([agent_id], [status]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_emp_phone' AND object_id = OBJECT_ID('dbo.employees'))
    CREATE INDEX [IX_emp_phone] ON dbo.[employees] ([phone]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_emp_pos' AND object_id = OBJECT_ID('dbo.employee_point_of_sales'))
    CREATE UNIQUE INDEX [UX_emp_pos] ON dbo.[employee_point_of_sales] ([employee_id], [point_of_sale_id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_emp_pos_pos' AND object_id = OBJECT_ID('dbo.employee_point_of_sales'))
    CREATE INDEX [IX_emp_pos_pos] ON dbo.[employee_point_of_sales] ([point_of_sale_id], [is_active]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_dr_hash_class' AND object_id = OBJECT_ID('dbo.device_registry'))
    CREATE UNIQUE INDEX [UX_dr_hash_class] ON dbo.[device_registry] ([device_hash], [classification]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_dr_hash' AND object_id = OBJECT_ID('dbo.device_registry'))
    CREATE INDEX [IX_dr_hash] ON dbo.[device_registry] ([device_hash]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_emp_codes_active' AND object_id = OBJECT_ID('dbo.employee_activation_codes'))
    CREATE UNIQUE INDEX [UX_emp_codes_active] ON dbo.[employee_activation_codes] ([employee_id]) WHERE ([status]='ACTIVE');
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_emp_codes_phone' AND object_id = OBJECT_ID('dbo.employee_activation_codes'))
    CREATE INDEX [IX_emp_codes_phone] ON dbo.[employee_activation_codes] ([phone], [status]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_emp_codes_agent' AND object_id = OBJECT_ID('dbo.employee_activation_codes'))
    CREATE INDEX [IX_emp_codes_agent] ON dbo.[employee_activation_codes] ([agent_id], [issued_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_emp_otps_lookup' AND object_id = OBJECT_ID('dbo.employee_otps'))
    CREATE INDEX [IX_emp_otps_lookup] ON dbo.[employee_otps] ([employee_id], [status], [expires_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_emp_otps_phone' AND object_id = OBJECT_ID('dbo.employee_otps'))
    CREATE INDEX [IX_emp_otps_phone] ON dbo.[employee_otps] ([phone], [created_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_emp_devices_active' AND object_id = OBJECT_ID('dbo.employee_devices'))
    CREATE UNIQUE INDEX [UX_emp_devices_active] ON dbo.[employee_devices] ([employee_id]) WHERE ([status]='ACTIVE');
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_emp_devices_hash' AND object_id = OBJECT_ID('dbo.employee_devices'))
    CREATE INDEX [IX_emp_devices_hash] ON dbo.[employee_devices] ([device_hash], [status]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_emp_devices_agent' AND object_id = OBJECT_ID('dbo.employee_devices'))
    CREATE INDEX [IX_emp_devices_agent] ON dbo.[employee_devices] ([agent_id], [status]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_emp_sess_token' AND object_id = OBJECT_ID('dbo.employee_sessions'))
    CREATE UNIQUE INDEX [UX_emp_sess_token] ON dbo.[employee_sessions] ([access_token_hash]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_emp_sess_employee' AND object_id = OBJECT_ID('dbo.employee_sessions'))
    CREATE INDEX [IX_emp_sess_employee] ON dbo.[employee_sessions] ([employee_id], [status]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_emp_perms' AND object_id = OBJECT_ID('dbo.employee_permissions'))
    CREATE UNIQUE INDEX [UX_emp_perms] ON dbo.[employee_permissions] ([employee_id], [permission_key]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_tattr_number_action' AND object_id = OBJECT_ID('dbo.transfer_attributions'))
    CREATE UNIQUE INDEX [UX_tattr_number_action] ON dbo.[transfer_attributions] ([transfer_number], [action]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tattr_employee' AND object_id = OBJECT_ID('dbo.transfer_attributions'))
    CREATE INDEX [IX_tattr_employee] ON dbo.[transfer_attributions] ([employee_id], [occurred_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tattr_pos' AND object_id = OBJECT_ID('dbo.transfer_attributions'))
    CREATE INDEX [IX_tattr_pos] ON dbo.[transfer_attributions] ([point_of_sale_id], [occurred_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tattr_agent' AND object_id = OBJECT_ID('dbo.transfer_attributions'))
    CREATE INDEX [IX_tattr_agent] ON dbo.[transfer_attributions] ([agent_id], [occurred_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_audit_agent' AND object_id = OBJECT_ID('dbo.audit_logs'))
    CREATE INDEX [IX_audit_agent] ON dbo.[audit_logs] ([agent_id], [created_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_audit_employee' AND object_id = OBJECT_ID('dbo.audit_logs'))
    CREATE INDEX [IX_audit_employee] ON dbo.[audit_logs] ([employee_id], [created_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_audit_action' AND object_id = OBJECT_ID('dbo.audit_logs'))
    CREATE INDEX [IX_audit_action] ON dbo.[audit_logs] ([action], [created_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_seclog_type' AND object_id = OBJECT_ID('dbo.security_logs'))
    CREATE INDEX [IX_seclog_type] ON dbo.[security_logs] ([event_type], [created_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_seclog_agent' AND object_id = OBJECT_ID('dbo.security_logs'))
    CREATE INDEX [IX_seclog_agent] ON dbo.[security_logs] ([agent_id], [created_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_seclog_device' AND object_id = OBJECT_ID('dbo.security_logs'))
    CREATE INDEX [IX_seclog_device] ON dbo.[security_logs] ([device_hash], [created_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_cashbox_emp_pos' AND object_id = OBJECT_ID('dbo.employee_cashboxes'))
    CREATE UNIQUE INDEX [UX_cashbox_emp_pos] ON dbo.[employee_cashboxes] ([employee_id], [point_of_sale_id], [currency_code]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_cashbox_agent' AND object_id = OBJECT_ID('dbo.employee_cashboxes'))
    CREATE INDEX [IX_cashbox_agent] ON dbo.[employee_cashboxes] ([agent_id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_shift_open' AND object_id = OBJECT_ID('dbo.employee_shifts'))
    CREATE UNIQUE INDEX [UX_shift_open] ON dbo.[employee_shifts] ([employee_id]) WHERE ([status]='OPEN');
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_shift_employee' AND object_id = OBJECT_ID('dbo.employee_shifts'))
    CREATE INDEX [IX_shift_employee] ON dbo.[employee_shifts] ([employee_id], [started_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_shift_agent' AND object_id = OBJECT_ID('dbo.employee_shifts'))
    CREATE INDEX [IX_shift_agent] ON dbo.[employee_shifts] ([agent_id], [started_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_entry_client_ref' AND object_id = OBJECT_ID('dbo.employee_cashbox_entries'))
    CREATE UNIQUE INDEX [UX_entry_client_ref] ON dbo.[employee_cashbox_entries] ([employee_id], [client_ref]) WHERE ([client_ref] IS NOT NULL);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_entry_reference' AND object_id = OBJECT_ID('dbo.employee_cashbox_entries'))
    CREATE UNIQUE INDEX [UX_entry_reference] ON dbo.[employee_cashbox_entries] ([reference_type], [reference_id]) WHERE ([reference_id] IS NOT NULL AND [reversal_of] IS NULL);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_entry_cashbox' AND object_id = OBJECT_ID('dbo.employee_cashbox_entries'))
    CREATE INDEX [IX_entry_cashbox] ON dbo.[employee_cashbox_entries] ([cashbox_id], [created_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_entry_shift' AND object_id = OBJECT_ID('dbo.employee_cashbox_entries'))
    CREATE INDEX [IX_entry_shift] ON dbo.[employee_cashbox_entries] ([shift_id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_entry_agent' AND object_id = OBJECT_ID('dbo.employee_cashbox_entries'))
    CREATE INDEX [IX_entry_agent] ON dbo.[employee_cashbox_entries] ([agent_id], [created_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_closing_shift' AND object_id = OBJECT_ID('dbo.employee_shift_closings'))
    CREATE UNIQUE INDEX [UX_closing_shift] ON dbo.[employee_shift_closings] ([shift_id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_closing_agent' AND object_id = OBJECT_ID('dbo.employee_shift_closings'))
    CREATE INDEX [IX_closing_agent] ON dbo.[employee_shift_closings] ([agent_id], [closed_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_closing_result' AND object_id = OBJECT_ID('dbo.employee_shift_closings'))
    CREATE INDEX [IX_closing_result] ON dbo.[employee_shift_closings] ([result], [closed_at]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_thread_admin' AND object_id = OBJECT_ID('dbo.chat_threads'))
    CREATE UNIQUE INDEX [UQ_chat_thread_admin] ON dbo.[chat_threads] ([agent_id]) WHERE ([kind]='ADMIN');
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_thread_employee' AND object_id = OBJECT_ID('dbo.chat_threads'))
    CREATE UNIQUE INDEX [UQ_chat_thread_employee] ON dbo.[chat_threads] ([agent_id], [employee_id]) WHERE ([kind]='EMPLOYEE');
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_chat_threads_agent' AND object_id = OBJECT_ID('dbo.chat_threads'))
    CREATE INDEX [IX_chat_threads_agent] ON dbo.[chat_threads] ([agent_id], [last_message_at] DESC);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_chat_messages_thread' AND object_id = OBJECT_ID('dbo.chat_messages'))
    CREATE INDEX [IX_chat_messages_thread] ON dbo.[chat_messages] ([thread_id], [id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_msg_client' AND object_id = OBJECT_ID('dbo.chat_messages'))
    CREATE UNIQUE INDEX [UQ_chat_msg_client] ON dbo.[chat_messages] ([thread_id], [client_id]) WHERE ([client_id] IS NOT NULL);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_chat_msg_search' AND object_id = OBJECT_ID('dbo.chat_messages'))
    CREATE INDEX [IX_chat_msg_search] ON dbo.[chat_messages] ([thread_id], [deleted_at], [id] DESC);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_read_participant' AND object_id = OBJECT_ID('dbo.chat_reads'))
    CREATE UNIQUE INDEX [UQ_chat_read_participant] ON dbo.[chat_reads] ([thread_id], [reader_kind], [reader_id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_react_actor' AND object_id = OBJECT_ID('dbo.chat_reactions'))
    CREATE UNIQUE INDEX [UQ_chat_react_actor] ON dbo.[chat_reactions] ([message_id], [actor_kind], [actor_id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_chat_react_thread' AND object_id = OBJECT_ID('dbo.chat_reactions'))
    CREATE INDEX [IX_chat_react_thread] ON dbo.[chat_reactions] ([thread_id], [message_id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_settings_actor' AND object_id = OBJECT_ID('dbo.chat_settings'))
    CREATE UNIQUE INDEX [UQ_chat_settings_actor] ON dbo.[chat_settings] ([thread_id], [actor_kind], [actor_id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_star_actor' AND object_id = OBJECT_ID('dbo.chat_stars'))
    CREATE UNIQUE INDEX [UQ_chat_star_actor] ON dbo.[chat_stars] ([message_id], [actor_kind], [actor_id]);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_chat_typing_actor' AND object_id = OBJECT_ID('dbo.chat_typing'))
    CREATE UNIQUE INDEX [UQ_chat_typing_actor] ON dbo.[chat_typing] ([thread_id], [actor_kind], [actor_id]);
GO

/* ---------- قيود التحقّق ---------- */
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_ait_status' AND parent_object_id = OBJECT_ID('dbo.agent_incoming_transfers'))
    ALTER TABLE dbo.[agent_incoming_transfers] ADD CONSTRAINT [CK_ait_status] CHECK ([status]='DELIVERED' OR [status]='PENDING_DELIVERY');
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_entry_direction' AND parent_object_id = OBJECT_ID('dbo.employee_cashbox_entries'))
    ALTER TABLE dbo.[employee_cashbox_entries] ADD CONSTRAINT [CK_entry_direction] CHECK ([direction]='OUT' OR [direction]='IN');
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_entry_amount' AND parent_object_id = OBJECT_ID('dbo.employee_cashbox_entries'))
    ALTER TABLE dbo.[employee_cashbox_entries] ADD CONSTRAINT [CK_entry_amount] CHECK ([amount]>(0));
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_chat_thread_kind' AND parent_object_id = OBJECT_ID('dbo.chat_threads'))
    ALTER TABLE dbo.[chat_threads] ADD CONSTRAINT [CK_chat_thread_kind] CHECK ([kind]='EMPLOYEE' OR [kind]='ADMIN');
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_chat_sender_kind' AND parent_object_id = OBJECT_ID('dbo.chat_messages'))
    ALTER TABLE dbo.[chat_messages] ADD CONSTRAINT [CK_chat_sender_kind] CHECK ([sender_kind]='ADMIN' OR [sender_kind]='EMPLOYEE' OR [sender_kind]='AGENT');
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_chat_reader_kind' AND parent_object_id = OBJECT_ID('dbo.chat_reads'))
    ALTER TABLE dbo.[chat_reads] ADD CONSTRAINT [CK_chat_reader_kind] CHECK ([reader_kind]='ADMIN' OR [reader_kind]='EMPLOYEE' OR [reader_kind]='AGENT');
GO

/* ---------- المفاتيح الأجنبية ---------- */
IF OBJECT_ID('dbo.FK_chat_msg_reply', 'F') IS NULL
    ALTER TABLE dbo.[chat_messages] ADD CONSTRAINT [FK_chat_msg_reply] FOREIGN KEY ([reply_to_id]) REFERENCES dbo.[chat_messages] ([id]);
GO
IF OBJECT_ID('dbo.FK_chat_msg_thread', 'F') IS NULL
    ALTER TABLE dbo.[chat_messages] ADD CONSTRAINT [FK_chat_msg_thread] FOREIGN KEY ([thread_id]) REFERENCES dbo.[chat_threads] ([id]);
GO
IF OBJECT_ID('dbo.FK_chat_react_msg', 'F') IS NULL
    ALTER TABLE dbo.[chat_reactions] ADD CONSTRAINT [FK_chat_react_msg] FOREIGN KEY ([message_id]) REFERENCES dbo.[chat_messages] ([id]);
GO
IF OBJECT_ID('dbo.FK_chat_read_thread', 'F') IS NULL
    ALTER TABLE dbo.[chat_reads] ADD CONSTRAINT [FK_chat_read_thread] FOREIGN KEY ([thread_id]) REFERENCES dbo.[chat_threads] ([id]);
GO
IF OBJECT_ID('dbo.FK_chat_set_thread', 'F') IS NULL
    ALTER TABLE dbo.[chat_settings] ADD CONSTRAINT [FK_chat_set_thread] FOREIGN KEY ([thread_id]) REFERENCES dbo.[chat_threads] ([id]);
GO
IF OBJECT_ID('dbo.FK_chat_star_msg', 'F') IS NULL
    ALTER TABLE dbo.[chat_stars] ADD CONSTRAINT [FK_chat_star_msg] FOREIGN KEY ([message_id]) REFERENCES dbo.[chat_messages] ([id]);
GO
IF OBJECT_ID('dbo.FK_chat_thread_employee', 'F') IS NULL
    ALTER TABLE dbo.[chat_threads] ADD CONSTRAINT [FK_chat_thread_employee] FOREIGN KEY ([employee_id]) REFERENCES dbo.[employees] ([id]);
GO
IF OBJECT_ID('dbo.FK_chat_typing_thread', 'F') IS NULL
    ALTER TABLE dbo.[chat_typing] ADD CONSTRAINT [FK_chat_typing_thread] FOREIGN KEY ([thread_id]) REFERENCES dbo.[chat_threads] ([id]);
GO
IF OBJECT_ID('dbo.FK_emp_codes_employee', 'F') IS NULL
    ALTER TABLE dbo.[employee_activation_codes] ADD CONSTRAINT [FK_emp_codes_employee] FOREIGN KEY ([employee_id]) REFERENCES dbo.[employees] ([id]);
GO
IF OBJECT_ID('dbo.FK_entry_employee', 'F') IS NULL
    ALTER TABLE dbo.[employee_cashbox_entries] ADD CONSTRAINT [FK_entry_employee] FOREIGN KEY ([employee_id]) REFERENCES dbo.[employees] ([id]);
GO
IF OBJECT_ID('dbo.FK_entry_cashbox', 'F') IS NULL
    ALTER TABLE dbo.[employee_cashbox_entries] ADD CONSTRAINT [FK_entry_cashbox] FOREIGN KEY ([cashbox_id]) REFERENCES dbo.[employee_cashboxes] ([id]);
GO
IF OBJECT_ID('dbo.FK_entry_shift', 'F') IS NULL
    ALTER TABLE dbo.[employee_cashbox_entries] ADD CONSTRAINT [FK_entry_shift] FOREIGN KEY ([shift_id]) REFERENCES dbo.[employee_shifts] ([id]);
GO
IF OBJECT_ID('dbo.FK_entry_reversal', 'F') IS NULL
    ALTER TABLE dbo.[employee_cashbox_entries] ADD CONSTRAINT [FK_entry_reversal] FOREIGN KEY ([reversal_of]) REFERENCES dbo.[employee_cashbox_entries] ([id]);
GO
IF OBJECT_ID('dbo.FK_cashbox_employee', 'F') IS NULL
    ALTER TABLE dbo.[employee_cashboxes] ADD CONSTRAINT [FK_cashbox_employee] FOREIGN KEY ([employee_id]) REFERENCES dbo.[employees] ([id]);
GO
IF OBJECT_ID('dbo.FK_emp_devices_employee', 'F') IS NULL
    ALTER TABLE dbo.[employee_devices] ADD CONSTRAINT [FK_emp_devices_employee] FOREIGN KEY ([employee_id]) REFERENCES dbo.[employees] ([id]);
GO
IF OBJECT_ID('dbo.FK_emp_otps_employee', 'F') IS NULL
    ALTER TABLE dbo.[employee_otps] ADD CONSTRAINT [FK_emp_otps_employee] FOREIGN KEY ([employee_id]) REFERENCES dbo.[employees] ([id]);
GO
IF OBJECT_ID('dbo.FK_emp_perms_employee', 'F') IS NULL
    ALTER TABLE dbo.[employee_permissions] ADD CONSTRAINT [FK_emp_perms_employee] FOREIGN KEY ([employee_id]) REFERENCES dbo.[employees] ([id]);
GO
IF OBJECT_ID('dbo.FK_emp_pos_employee', 'F') IS NULL
    ALTER TABLE dbo.[employee_point_of_sales] ADD CONSTRAINT [FK_emp_pos_employee] FOREIGN KEY ([employee_id]) REFERENCES dbo.[employees] ([id]);
GO
IF OBJECT_ID('dbo.FK_emp_sessions_employee', 'F') IS NULL
    ALTER TABLE dbo.[employee_sessions] ADD CONSTRAINT [FK_emp_sessions_employee] FOREIGN KEY ([employee_id]) REFERENCES dbo.[employees] ([id]);
GO
IF OBJECT_ID('dbo.FK_emp_sessions_device', 'F') IS NULL
    ALTER TABLE dbo.[employee_sessions] ADD CONSTRAINT [FK_emp_sessions_device] FOREIGN KEY ([device_id]) REFERENCES dbo.[employee_devices] ([id]);
GO
IF OBJECT_ID('dbo.FK_closing_shift', 'F') IS NULL
    ALTER TABLE dbo.[employee_shift_closings] ADD CONSTRAINT [FK_closing_shift] FOREIGN KEY ([shift_id]) REFERENCES dbo.[employee_shifts] ([id]);
GO
IF OBJECT_ID('dbo.FK_shift_employee', 'F') IS NULL
    ALTER TABLE dbo.[employee_shifts] ADD CONSTRAINT [FK_shift_employee] FOREIGN KEY ([employee_id]) REFERENCES dbo.[employees] ([id]);
GO
IF OBJECT_ID('dbo.FK_shift_cashbox', 'F') IS NULL
    ALTER TABLE dbo.[employee_shifts] ADD CONSTRAINT [FK_shift_cashbox] FOREIGN KEY ([cashbox_id]) REFERENCES dbo.[employee_cashboxes] ([id]);
GO
IF OBJECT_ID('dbo.FK_tsh_transfer', 'F') IS NULL
    ALTER TABLE dbo.[transfer_status_history] ADD CONSTRAINT [FK_tsh_transfer] FOREIGN KEY ([transfer_id]) REFERENCES dbo.[agent_incoming_transfers] ([id]);
GO

/* ============================================================================
   التحقّق — يُقرأ بعد التنفيذ
   ============================================================================ */

/* 1) الجداول الستّة والعشرون موجودة؟ */
SELECT t.name AS [الجدول],
       (SELECT COUNT(*) FROM sys.columns c WHERE c.object_id = t.object_id) AS [أعمدة]
  FROM sys.tables t
 WHERE t.name IN (
        'agent_incoming_transfers','transfer_status_history',
        'tenant_branding','tenant_branding_audit',
        'employees','employee_point_of_sales','device_registry',
        'employee_activation_codes','employee_otps','employee_devices',
        'employee_sessions','employee_permissions','transfer_attributions',
        'audit_logs','security_logs',
        'employee_cashboxes','employee_shifts','employee_cashbox_entries',
        'employee_shift_closings',
        'chat_threads','chat_messages','chat_reads',
        'chat_reactions','chat_settings','chat_stars','chat_typing')
 ORDER BY t.name;
GO

/* 2) الجداول الجديدة كلّها فارغة؟ السكربت بنيةٌ لا بيانات. */
SELECT OBJECT_NAME(p.object_id) AS [الجدول], SUM(p.rows) AS [صفوف]
  FROM sys.partitions p
 WHERE p.index_id IN (0,1)
   AND OBJECT_NAME(p.object_id) IN (
        'agent_incoming_transfers','transfer_status_history',
        'tenant_branding','tenant_branding_audit','employees',
        'employee_point_of_sales','device_registry','employee_activation_codes',
        'employee_otps','employee_devices','employee_sessions',
        'employee_permissions','transfer_attributions','audit_logs',
        'security_logs','employee_cashboxes','employee_shifts',
        'employee_cashbox_entries','employee_shift_closings',
        'chat_threads','chat_messages','chat_reads',
        'chat_reactions','chat_settings','chat_stars','chat_typing')
 GROUP BY OBJECT_NAME(p.object_id)
 ORDER BY 1;
GO

/* 3) لا مفتاح أجنبي من الجديد إلى جداول المنظومة.
      يجب أن يعود **صفر صفوف**. */
SELECT fk.name AS [مفتاح يخرج إلى المنظومة],
       OBJECT_NAME(fk.parent_object_id) AS [من],
       OBJECT_NAME(fk.referenced_object_id) AS [إلى]
  FROM sys.foreign_keys fk
 WHERE OBJECT_NAME(fk.parent_object_id) IN (
        'agent_incoming_transfers','transfer_status_history','tenant_branding',
        'tenant_branding_audit','employees','employee_point_of_sales',
        'device_registry','employee_activation_codes','employee_otps',
        'employee_devices','employee_sessions','employee_permissions',
        'transfer_attributions','audit_logs','security_logs',
        'employee_cashboxes','employee_shifts','employee_cashbox_entries',
        'employee_shift_closings',
        'chat_threads','chat_messages','chat_reads',
        'chat_reactions','chat_settings','chat_stars','chat_typing')
   AND OBJECT_NAME(fk.referenced_object_id) NOT IN (
        'agent_incoming_transfers','transfer_status_history','tenant_branding',
        'tenant_branding_audit','employees','employee_point_of_sales',
        'device_registry','employee_activation_codes','employee_otps',
        'employee_devices','employee_sessions','employee_permissions',
        'transfer_attributions','audit_logs','security_logs',
        'employee_cashboxes','employee_shifts','employee_cashbox_entries',
        'employee_shift_closings',
        'chat_threads','chat_messages','chat_reads',
        'chat_reactions','chat_settings','chat_stars','chat_typing');
GO

/* 4) لم يُمَسّ جدولٌ ماليّ.
      اقرأ تواريخ التعديل: يجب أن تكون **قبل** يوم تشغيلك لهذا السكربت. */
SELECT name AS [جدول ماليّ], modify_date AS [آخر تعديل على بنيته]
  FROM sys.tables
 WHERE name IN ('wallet','InternalEx','ExternalEx','AccountsTb','users',
                'AuthorizedUsers','CurrencyMainTb')
 ORDER BY name;
GO
