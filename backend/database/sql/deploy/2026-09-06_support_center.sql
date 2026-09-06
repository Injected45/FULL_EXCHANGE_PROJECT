/* ============================================================================
   سكربت نشر — مركز «الرحالة للدعم الفني»
   التاريخ: 2026-09-06
   ============================================================================

   ⚠ **لا أثر ماليّ إطلاقاً.** هذا السكربت:
     • لا يُنشئ ولا يُعدّل ولا يحذف أيّ جدولٍ ماليّ
     • لا يلمس رصيداً ولا قيداً ولا حوالةً ولا عمولةً ولا خزينة
     • لا يُنشئ مفتاحاً أجنبياً إلى جدولٍ ماليّ — كلُّ مفاتيحه إلى
       `support_staff` و `chat_threads`
     • لا يُنشئ محفّزاً (trigger) واحداً
     • لا يُعدّل جدولاً قائماً إلا بعمودٍ **واحد قابلٍ للفراغ** على
       `chat_messages` (جدول دردشة، ليس مالياً)

   وقد جُرّب على قاعدة الاختبار: 13 دفعة، نجحت كلُّها، وأُعيد تشغيله فلم
   يفعل شيئاً (خامل).

   ── ما يُنشئه ────────────────────────────────────────────────────────────

   | الجدول | لماذا |
   |---|---|
   | `support_staff`        | حسابٌ لكل موظّف دعم بدل المفتاح المشترك |
   | `support_permissions`  | صلاحياتٌ ممنوحة صفّاً صفّاً (Default Deny) |
   | `support_sessions`     | جلساتٌ منفصلة عن جلسات الوكلاء والموظفين |
   | `support_thread_state` | إسنادُ المحادثة وحالتُها |
   | `support_audit`        | سجلُّ نشاطٍ لا يُحذف منه |

   وعمودٌ واحد: `chat_messages.support_staff_id` — من ردّ فعلاً من الدعم.

   ── التشغيل ─────────────────────────────────────────────────────────────

   في SSMS على قاعدة `EXCHANGESYS2026`، أو:

     sqlcmd -S <SERVER> -d EXCHANGESYS2026 -i 2026-09-06_support_center.sql

   السكربت **خامل**: تشغيلُه مرّتين لا يضرّ.

   ── بعد التشغيل: أنشئ حساب المدير الأول ─────────────────────────────────

   في آخر الملفّ قسمٌ اختياريّ يُنشئ حساباً باسم `admin`. **غيّر كلمةَ
   المرور فيه قبل التشغيل**، وسيُطلب تغييرُها عند أوّل دخول على أي حال.
   ============================================================================ */

USE EXCHANGESYS2026;
GO

/* ============================================================================
   مركز «الرحالة للدعم الفني» — الجداول.

   ⚠ طبقةُ دعمٍ لا طبقة مالية. لا رصيد، ولا قيد، ولا حوالة، ولا مفتاحَ
   أجنبيّاً إلى جدولٍ ماليّ واحد. مفاتيحُ هذا الملفّ كلُّها تشير إلى
   `support_staff` و `chat_threads` لا غير — وهذا ما يجعل الفصلَ بنيوياً
   لا وعداً مكتوباً.

   ── لماذا جداولُ جديدة والدردشةُ قائمة؟ ──────────────────────────────────

   لأن ما يُضاف هنا **ليس دردشة**. الرسائل والمرفقات والتفاعلات والتثبيت
   والنجوم كلُّها تبقى في جداول `chat_*` بلا حرفٍ مكرَّر (أمرُ المالك:
   «لا تبنِ Backend Chat جديداً ولا Database Chat جديدة»). الجديد هنا
   ثلاثةُ أشياء لا وجود لها في نظام الدردشة أصلاً:

     1. **مَن موظّفو الدعم؟** كان الدخولُ مفتاحاً مشتركاً في `.env` — من
        عرفه دخل، والسجلُّ لا يقول من ردّ. `support_staff` تجعل لكلّ
        موظّفٍ هويّةً ودوراً وكلمةَ مرور.
     2. **مَن يملك أن يفعل ماذا؟** `support_permissions` صفوفٌ ممنوحة،
        و«لا صفَّ» يعني مرفوض — Default Deny، كصلاحيات الموظفين تماماً.
     3. **أين تقف المحادثة؟** `support_thread_state` تحمل الإسنادَ
        والحالة: محادثةٌ بلا مالكٍ ولا حالة تُنسى بين عشرين غيرها.

   ── قرارات داخل هذا الملفّ ───────────────────────────────────────────────

   `support_sessions`   جدولٌ مستقلّ لا `personal_access_tokens`: موظّف
                        الدعم **ليس صفّاً في `users`**، والخلطُ بينهما هو
                        الطريقُ إلى أن تصير جلسةُ دعمٍ يوماً جلسةَ وكيل.
                        وهو القرارُ نفسه المتّخذ في `employee_sessions`.

   الرمز مُجزَّأ         `token_hash` لا الرمزُ نفسه: قاعدةٌ تُسرَّق نسخةٌ
                        احتياطية منها لا تُسلِّم معها جلساتٍ حيّة.

   `support_audit`      لا يُحذف منه ولا يُعدَّل فيه. سؤال «من أغلق محادثة
                        الوكيل؟» يُجاب بعد شهر أو لا يُجاب أبداً.

   `status` نصٌّ مقيَّد  بـ CHECK لا جدولَ مرجعيّ: أربعُ حالاتٍ ثابتة
                        بطبيعتها، وجدولٌ لها يضيف وصلةً في كل استعلام
                        مقابل مرونةٍ لن تُستعمل.
   ============================================================================ */

/* ---------------------------------------------------------------------------
   1) موظّفو الدعم
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_staff', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_staff (
        id            BIGINT IDENTITY(1,1) NOT NULL,
        name          NVARCHAR(120)  NOT NULL,
        -- اسمُ الدخول: لاتينيّ قصير، فريدٌ بين غير المحذوفين. لا هاتف:
        -- موظّف الدعم يجلس على حاسوبٍ في المكتب ولا يُرسَل إليه رمزٌ
        -- برسالة، فربطُ دخوله بشبكة الهاتف يُعطّله حين تنقطع.
        username      VARCHAR(60)    NOT NULL,
        password_hash VARCHAR(255)   NOT NULL,
        -- SUPPORT_AGENT | SUPERVISOR | ADMIN — انظر SupportRoles.
        role          VARCHAR(20)    NOT NULL,
        is_active     BIT            NOT NULL CONSTRAINT DF_sup_staff_active DEFAULT (1),
        -- إجبارُ تغيير كلمة المرور عند أول دخول: من أنشأ الحساب يعرف
        -- كلمتَه الأولى، وحسابٌ يبقى عليها ليس حساباً شخصياً.
        must_change   BIT            NOT NULL CONSTRAINT DF_sup_staff_chg DEFAULT (1),
        last_seen_at  DATETIME2      NULL,
        created_at    DATETIME2      NOT NULL CONSTRAINT DF_sup_staff_created DEFAULT (SYSDATETIME()),
        created_by    BIGINT         NULL,
        -- حذفٌ ناعم: صفٌّ يُحذف يأخذ معه معنى كلِّ سطرٍ في السجلّ يشير إليه.
        deleted_at    DATETIME2      NULL,
        CONSTRAINT PK_support_staff PRIMARY KEY (id),
        CONSTRAINT CK_support_staff_role CHECK
            (role IN ('SUPPORT_AGENT', 'SUPERVISOR', 'ADMIN'))
    );
END;
GO

/* فريدٌ بين الأحياء وحدهم: اسمُ موظّفٍ حُذف يجوز أن يُعاد استعماله،
   وقيدٌ مطلق كان سيمنعه إلى الأبد. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_support_staff_user'
               AND object_id = OBJECT_ID('dbo.support_staff'))
    CREATE UNIQUE INDEX UQ_support_staff_user
        ON dbo.support_staff (username)
        WHERE deleted_at IS NULL;
GO

/* ---------------------------------------------------------------------------
   2) الصلاحيات — صفوفٌ ممنوحة، و«لا صفَّ» يعني مرفوض
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_permissions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_permissions (
        id         BIGINT IDENTITY(1,1) NOT NULL,
        staff_id   BIGINT       NOT NULL,
        permission VARCHAR(60)  NOT NULL,
        granted_at DATETIME2    NOT NULL CONSTRAINT DF_sup_perm_at DEFAULT (SYSDATETIME()),
        granted_by BIGINT       NULL,
        CONSTRAINT PK_support_permissions PRIMARY KEY (id),
        CONSTRAINT FK_support_perm_staff FOREIGN KEY (staff_id)
            REFERENCES dbo.support_staff (id)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_support_perm'
               AND object_id = OBJECT_ID('dbo.support_permissions'))
    CREATE UNIQUE INDEX UQ_support_perm
        ON dbo.support_permissions (staff_id, permission);
GO

/* ---------------------------------------------------------------------------
   3) الجلسات
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_sessions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_sessions (
        id           BIGINT IDENTITY(1,1) NOT NULL,
        staff_id     BIGINT       NOT NULL,
        token_hash   CHAR(64)     NOT NULL,   -- SHA-256 للرمز، لا الرمز
        created_at   DATETIME2    NOT NULL CONSTRAINT DF_sup_sess_created DEFAULT (SYSDATETIME()),
        last_seen_at DATETIME2    NULL,
        expires_at   DATETIME2    NOT NULL,
        revoked_at   DATETIME2    NULL,
        ip           VARCHAR(45)  NULL,
        user_agent   NVARCHAR(255) NULL,
        CONSTRAINT PK_support_sessions PRIMARY KEY (id),
        CONSTRAINT FK_support_sess_staff FOREIGN KEY (staff_id)
            REFERENCES dbo.support_staff (id)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_support_sess_token'
               AND object_id = OBJECT_ID('dbo.support_sessions'))
    CREATE UNIQUE INDEX UQ_support_sess_token
        ON dbo.support_sessions (token_hash);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_support_sess_staff'
               AND object_id = OBJECT_ID('dbo.support_sessions'))
    CREATE INDEX IX_support_sess_staff
        ON dbo.support_sessions (staff_id, revoked_at);
GO

/* ---------------------------------------------------------------------------
   4) حالة المحادثة وإسنادها

   صفٌّ لكلّ محادثةِ إدارة. غيابُ الصفّ = محادثةٌ جديدة بلا مالك، وهي
   الحالةُ الافتراضية الصحيحة: لا نكتب صفّاً لكلّ وكيلٍ سجّل، بل عند أوّل
   إجراءٍ فعليّ عليه.
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_thread_state', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_thread_state (
        thread_id    BIGINT       NOT NULL,
        -- NEW      : وردت ولم يلمسها أحد
        -- OPEN     : مُسنَدة ويُعمل عليها
        -- PENDING  : الدعم ردّ وينتظر الوكيل
        -- CLOSED   : أُغلقت
        status       VARCHAR(12)  NOT NULL CONSTRAINT DF_sup_state_status DEFAULT ('NEW'),
        assigned_to  BIGINT       NULL,
        assigned_at  DATETIME2    NULL,
        assigned_by  BIGINT       NULL,
        closed_at    DATETIME2    NULL,
        closed_by    BIGINT       NULL,
        close_note   NVARCHAR(500) NULL,
        updated_at   DATETIME2    NOT NULL CONSTRAINT DF_sup_state_upd DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_support_thread_state PRIMARY KEY (thread_id),
        CONSTRAINT FK_support_state_thread FOREIGN KEY (thread_id)
            REFERENCES dbo.chat_threads (id),
        CONSTRAINT FK_support_state_assignee FOREIGN KEY (assigned_to)
            REFERENCES dbo.support_staff (id),
        CONSTRAINT CK_support_state_status CHECK
            (status IN ('NEW', 'OPEN', 'PENDING', 'CLOSED'))
    );
END;
GO

/* «محادثاتي» و«غير المُسنَدة» هما الفلترانِ المستعملان في كل فتحةِ شاشة. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_support_state_assignee'
               AND object_id = OBJECT_ID('dbo.support_thread_state'))
    CREATE INDEX IX_support_state_assignee
        ON dbo.support_thread_state (assigned_to, status);
GO

/* ---------------------------------------------------------------------------
   5) سجلّ النشاط — يُضاف إليه ولا يُحذف منه
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_audit', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_audit (
        id         BIGINT IDENTITY(1,1) NOT NULL,
        staff_id   BIGINT        NULL,          -- NULL إن حُذف الحسابُ لاحقاً
        staff_name NVARCHAR(120) NOT NULL,      -- منسوخٌ وقتَ الفعل: الاسم
                                                -- قد يتغيّر، والسجلُّ يقول
                                                -- ما كان يومَها
        action     VARCHAR(40)   NOT NULL,
        thread_id  BIGINT        NULL,
        target     VARCHAR(60)   NULL,
        detail     NVARCHAR(500) NULL,
        ip         VARCHAR(45)   NULL,
        created_at DATETIME2     NOT NULL CONSTRAINT DF_sup_audit_at DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_support_audit PRIMARY KEY (id)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_support_audit_time'
               AND object_id = OBJECT_ID('dbo.support_audit'))
    CREATE INDEX IX_support_audit_time
        ON dbo.support_audit (created_at DESC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_support_audit_thread'
               AND object_id = OBJECT_ID('dbo.support_audit'))
    CREATE INDEX IX_support_audit_thread
        ON dbo.support_audit (thread_id, created_at DESC);
GO

/* ---------------------------------------------------------------------------
   6) هويّة المُرسِل في `chat_messages`

   كان المُرسِلُ من الإدارة يُكتب بـ `sender_id = 0` لأن المفتاح كان مشتركاً
   ولا رقمَ خلفه. الآن له رقم — لكنّ الصفوف القديمة لا تُلمس ولا تُخمَّن:
   تبقى 0 وتعني «قبل أن تصير للدعم حسابات».

   ولا يُعاد استعمالُ `sender_id` لرقم موظّف الدعم: قراءةُ الاستلام
   (`chat_reads`) تقارن بـ (kind, id)، ولو صار لكلّ موظّفٍ رقمٌ مختلف
   لانقسمت حالةُ القراءة على موظّفي الدعم — والوكيل يرى شرطتين تتراجعان
   كلّما ردّ عليه موظّفٌ آخر. الإدارةُ طرفٌ واحد في نظر الوكيل، وتبقى
   `id = 0`؛ ومن ردّ فعلاً يُكتب في عمودٍ مستقلّ.
   --------------------------------------------------------------------------- */
IF COL_LENGTH('dbo.chat_messages', 'support_staff_id') IS NULL
BEGIN
    ALTER TABLE dbo.chat_messages
        ADD support_staff_id BIGINT NULL;
END;
GO

/* ============================================================================
   اختياريّ — حساب المدير الأوّل

   ⚠ غيّر `@Password` قبل التشغيل. والتجزئة أدناه من نوع bcrypt وتُولَّد
   بـ PHP لا بـ T-SQL، فالسطر مُعطَّل عمداً وله بديلان في التعليق.

   الأسهل: شغّل هذا في مجلّد `backend` بعد نشر الملفّات:

     php artisan tinker --execute="
       \$id = DB::table('support_staff')->insertGetId([
         'name' => 'مدير النظام', 'username' => 'admin',
         'password_hash' => Hash::make('غيّرها-هنا-123'),
         'role' => 'ADMIN', 'is_active' => 1, 'must_change' => 1,
       ]);
       foreach (App\Services\Support\SupportPermissions::ROLE_DEFAULTS['ADMIN'] as \$p)
         DB::table('support_permissions')->insert([
           'staff_id' => \$id, 'permission' => \$p, 'granted_at' => now(),
         ]);
       echo 'أُنشئ #' . \$id;
     "

   ولا يُنشأ الحساب من هنا مباشرةً لأن كلمة المرور تُجزَّأ بـ bcrypt، وليس
   في SQL Server دالّةٌ له — وكتابةُ كلمةٍ نصّاً في عمود التجزئة تعني حساباً
   لا يمكن الدخول به أبداً، وهو أسوأ الأخطاء لأنه يبدو ناجحاً.
   ============================================================================ */

/* ============================================================================
   تحقّقٌ بعد التشغيل — يجب أن تعود خمسةُ صفوفٍ وعمودٌ واحد
   ============================================================================ */
SELECT name AS [الجدول],
       (SELECT COUNT(*) FROM sys.columns WHERE object_id = t.object_id) AS [الأعمدة]
  FROM sys.tables t
 WHERE name LIKE 'support[_]%'
 ORDER BY name;
GO

SELECT CASE WHEN COL_LENGTH('dbo.chat_messages', 'support_staff_id') IS NULL
            THEN N'✗ العمود غائب' ELSE N'✓ العمود موجود' END AS [chat_messages.support_staff_id];
GO

/* ولا شيء ماليّ تغيّر — للتأكّد بعينك: */
SELECT name AS [جدول ماليّ], modify_date AS [آخر تعديل بنيويّ]
  FROM sys.tables
 WHERE name IN ('wallet', 'InternalEx', 'EX24AccSafeActivityTb', 'ExchangeAccData', 'AccountsTb')
 ORDER BY modify_date DESC;
GO
