/* ============================================================================
   سكربت نشر — مركز الدعم، الدفعة الثانية
   التاريخ: 2026-09-07
   (بنود المالك 1 لوحة القيادة · 2 أزمنة الاستجابة · 6 منع التعارض
    · 35 حمولة الفريق · 36 حالة الموظّف)
   ============================================================================

   ⚠ **لا أثر ماليّ.** صفرُ جداولَ مالية، وصفرُ مفاتيحَ إليها، وصفرُ
   محفّزات. المفتاحان الجديدان إلى `chat_threads` و`support_staff`.

   ⚠ **تطويرٌ فوق القائم لا بناءٌ جديد:** أعمدةُ الأزمنة التي تقيس عليها
   هذه الدفعةُ أُضيفت في الدفعة الأولى ولم تُستبدل، و«حالة الموظّف» عمودان
   على `support_staff` لا جدولٌ ثانٍ يصف الموظّف مرّةً أخرى.

   ── ما يُضاف ──
   | الجدول/العمود | لماذا |
   |---|---|
   | `support_sla` | زمنُ الاستجابة لكل أولوية — تديره الإدارة لا الشيفرة |
   | `support_staff.presence` / `presence_at` | متاح · مشغول · بعيد · غير متصل |
   | `support_staff.capacity` | سقفُ المحادثات المُسنَدة، لقياس الحمولة |
   | `support_viewers` | من يفتح المحادثة الآن — كشفُ التعارض لا قفلُه |

   ── التشغيل ──
     sqlcmd -S <SERVER> -d EXCHANGESYS2026 -i 2026-09-07_support_sla.sql

   خاملٌ: تشغيلُه مرّتين لا يضرّ — كلُّ دفعةٍ محروسةٌ بـ`IF … IS NULL`،
   والقيمُ المزروعة محروسةٌ بـ`IF NOT EXISTS` فلا تُعاد كتابةُ ما ضبطته
   الإدارةُ بيدها.

   ⚠ ويُشترط أن يكون سكربتا 2026-09-06_support_center.sql
      و2026-09-07_support_ops.sql قد شُغِّلا قبله بهذا الترتيب.
   ============================================================================ */

USE EXCHANGESYS2026;
GO

/* ⚠ ضبطُ الجلسة قبل كلّ شيء — ولَيس تزييناً.
 *
 * `sqlcmd` يبدأ بـQUOTED_IDENTIFIER = OFF، والفهرسُ المُرشَّح يُرفض
 * أن يُنشأ عندها، فيفشل النشرُ في منتصفه ويترك القاعدةَ
 * نصفَ مبنيّة. ولا يظهر هذا في التطبيق لأن PDO يبدأ بـON —
 * فـ«يعمل عندي» لا يعني أنّه ينشر. */
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ============================================================================
   مركز الدعم — الدفعة الثانية
   (بنود المالك 1 لوحة القيادة · 2 SLA · 6 منع التعارض · 36 حالة الموظّف)

   ⚠ تطويرٌ فوق القائم: أعمدةُ الأزمنة أُضيفت في الدفعة الأولى وتُملأ هنا،
   ولا جدولَ يُكرّر وظيفةَ موجود.

   ⚠ ولا شيء يمسّ المال: إعداداتُ زمنٍ وحضورٌ ومشاهدة.

   ── ثلاثة قرارات ─────────────────────────────────────────────────────────

   1. **`support_sla` صفٌّ لكل أولوية لا قيمٌ في الشيفرة.** نصُّ البند 2:
      «قابلة للإدارة من إعدادات النظام ولا تكون Hardcoded». وربطُها
      بالأولوية لا بالتصنيف: العاجلُ عاجلٌ أياً كان نوعُه، والتصنيفُ يقول
      **ما** المشكلة لا **متى** يجب الردّ.

   2. **`support_viewers` جدولٌ يُكتب فوقه ولا يُراكم** — كـ`chat_typing`
      تماماً وللسبب نفسه: الحضورُ لحظيّ ولا يُحفظ في سجلّ. وصفٌّ لكل
      (محادثة، موظّف) لا لكل فتحة.

   3. **حالةُ الموظّف عمودٌ على `support_staff` لا جدولٌ ثانٍ.** صفةٌ واحدة
      للموظّف الواحد، وجدولٌ لها يعني وصلةً في كل استعلامِ إسناد.
   ============================================================================ */

/* ---------------------------------------------------------------------------
   1) إعدادات زمن الاستجابة (البند 2)
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_sla', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_sla (
        priority             VARCHAR(10) NOT NULL,
        -- بالدقائق. صفرٌ أو NULL يعني «لا هدف لهذه الأولوية».
        first_response_min   INT NULL,
        next_response_min    INT NULL,
        resolution_min       INT NULL,
        updated_at           DATETIME2 NOT NULL
                             CONSTRAINT DF_sup_sla_upd DEFAULT (SYSDATETIME()),
        updated_by           BIGINT NULL,
        CONSTRAINT PK_support_sla PRIMARY KEY (priority),
        CONSTRAINT CK_support_sla_priority CHECK
            (priority IN ('NORMAL', 'HIGH', 'URGENT', 'CRITICAL'))
    );
END;
GO

/*
   القيم الأولى — تُزرع مرّةً وتُعدَّل من الإدارة.

   وهي **اقتراحٌ لا حكم**: الأرقام مبنيّة على أن العاجل يحتاج ردّاً خلال
   ربع ساعة والعاديّ خلال ساعتين، وهو ما يناسب فريقاً صغيراً في ساعات
   عمل. والإدارةُ تعرف واقعَها فتضبطها.
*/
IF NOT EXISTS (SELECT 1 FROM dbo.support_sla)
BEGIN
    INSERT INTO dbo.support_sla (priority, first_response_min, next_response_min, resolution_min) VALUES
        ('CRITICAL',   5,   10,   120),
        ('URGENT',    15,   30,   240),
        ('HIGH',      45,   60,   480),
        ('NORMAL',   120,  180,  1440);
END;
GO

/* ---------------------------------------------------------------------------
   2) حالة الموظّف (البند 36)

   AVAILABLE | BUSY | BREAK — و«غير متصل» **لا تُخزَّن**: تُحسب من
   `last_seen_at`. حالةٌ مخزَّنة تبقى «متاح» بعد أن يُغلق الموظّف حاسوبه
   ويذهب، فيُسنَد إليه عملٌ لا أحد يراه.
   --------------------------------------------------------------------------- */
IF COL_LENGTH('dbo.support_staff', 'presence') IS NULL
BEGIN
    ALTER TABLE dbo.support_staff ADD
        presence     VARCHAR(12) NOT NULL
                     CONSTRAINT DF_sup_staff_presence DEFAULT ('AVAILABLE'),
        presence_at  DATETIME2 NULL,
        -- سعةُ الموظّف (البند 5): كم محادثةً مفتوحة يُعقل أن يحمل.
        -- تُستعمل في التوزيع التلقائي، وتُعرض في لوحة المشرف.
        capacity     INT NOT NULL CONSTRAINT DF_sup_staff_cap DEFAULT (10);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_sup_staff_presence')
    ALTER TABLE dbo.support_staff ADD CONSTRAINT CK_sup_staff_presence
        CHECK (presence IN ('AVAILABLE', 'BUSY', 'BREAK'));
GO

/* ---------------------------------------------------------------------------
   3) من يشاهد المحادثة الآن (البند 6)

   ⚠ **كشفُ تعارضٍ لا قفلٌ صلب.** نصُّ البند: «بدون منع المشرف من الدخول
   عند الحاجة». فالجدولُ يقول من ينظر ومن يكتب، ولا يمنع أحداً — ومنعُ
   الدخول يعني محادثةَ وكيلٍ عالقةً لأن موظّفاً نسي إغلاق لسانه.

   ويُكتب فوقه ولا يُراكم: صفٌّ لكل (محادثة، موظّف)، و`expires_at` تُسقط
   من أغلق الشاشة بلا وداع.
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.support_viewers', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.support_viewers (
        thread_id   BIGINT        NOT NULL,
        staff_id    BIGINT        NOT NULL,
        staff_name  NVARCHAR(120) NOT NULL,
        -- VIEWING | TYPING — «يكتب» أقوى من «ينظر»، وهي ما يمنع التعارض.
        state       VARCHAR(10)   NOT NULL
                    CONSTRAINT DF_sup_view_state DEFAULT ('VIEWING'),
        expires_at  DATETIME2     NOT NULL,
        updated_at  DATETIME2     NOT NULL
                    CONSTRAINT DF_sup_view_upd DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_support_viewers PRIMARY KEY (thread_id, staff_id),
        CONSTRAINT FK_sup_view_thread FOREIGN KEY (thread_id)
            REFERENCES dbo.chat_threads (id),
        CONSTRAINT FK_sup_view_staff FOREIGN KEY (staff_id)
            REFERENCES dbo.support_staff (id)
    );
END;
GO

/* الساري وحده يُقرأ — والمنتهي يُكنَس مع أوّل كتابة. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_sup_view_expires'
               AND object_id = OBJECT_ID('dbo.support_viewers'))
    CREATE INDEX IX_sup_view_expires ON dbo.support_viewers (expires_at);
GO


/* ---------------------------------------------------------------------------
   مفاتيحُ الصّلاحيات الجديدة للحسابات القائمة

   ⚠ حسابٌ أُنشئ قبل هذه الدفعة لا يحمل مفاتيحَها، فيفتح
   المدير لوحتَه ولا يجد ما بُني له — ولا خطأَ يُرشدُه إلى
   السّبب. وقد وقع ذلك فعلاً على حساب المالك نفسِه.

   ⚠ والمنحُ مقتصرٌ على مفاتيح هذه الدفعة وحدها: مفتاحٌ لم
   يكن موجوداً أمس لا يمكن أن يكون أحدٌ قد سحبه، فمنحُه
   لا يُلغي قراراً لأحد. أمّا منحُ «كلّ ما ينقص عن الافتراضيّ»
   فيُعيد ما سحبته الإدارةُ عمداً.

   وخامل: من يحملُه لا يُمنحُه مرّةً ثانية.
   --------------------------------------------------------------------------- */
/* PERMISSION BACKFILL */

INSERT INTO dbo.support_permissions (staff_id, permission, granted_at, granted_by)
SELECT s.id, 'VIEW_DASHBOARD', SYSDATETIME(), NULL
  FROM dbo.support_staff s
 WHERE s.deleted_at IS NULL
   AND s.role IN ('SUPPORT_AGENT', 'SUPERVISOR', 'ADMIN')
   AND NOT EXISTS (SELECT 1 FROM dbo.support_permissions p
                    WHERE p.staff_id = s.id AND p.permission = 'VIEW_DASHBOARD');

INSERT INTO dbo.support_permissions (staff_id, permission, granted_at, granted_by)
SELECT s.id, 'VIEW_TEAM', SYSDATETIME(), NULL
  FROM dbo.support_staff s
 WHERE s.deleted_at IS NULL
   AND s.role IN ('SUPERVISOR', 'ADMIN')
   AND NOT EXISTS (SELECT 1 FROM dbo.support_permissions p
                    WHERE p.staff_id = s.id AND p.permission = 'VIEW_TEAM');

INSERT INTO dbo.support_permissions (staff_id, permission, granted_at, granted_by)
SELECT s.id, 'MANAGE_SLA', SYSDATETIME(), NULL
  FROM dbo.support_staff s
 WHERE s.deleted_at IS NULL
   AND s.role IN ('SUPERVISOR', 'ADMIN')
   AND NOT EXISTS (SELECT 1 FROM dbo.support_permissions p
                    WHERE p.staff_id = s.id AND p.permission = 'MANAGE_SLA');
GO

/* ============================================================================
   التحقّق بعد النشر — يُقرأ بالعين، ولا يُكتفى بأن السكربت لم يُخطئ
   ============================================================================ */

SELECT name AS [جدول], create_date AS [أُنشئ]
  FROM sys.tables
 WHERE name IN ('support_sla', 'support_viewers')
 ORDER BY name;
GO

SELECT CASE WHEN COL_LENGTH('dbo.support_staff','presence') IS NULL
            THEN N'✗ غائب' ELSE N'✓ موجود' END AS [support_staff.presence],
       CASE WHEN COL_LENGTH('dbo.support_staff','presence_at') IS NULL
            THEN N'✗ غائب' ELSE N'✓ موجود' END AS [support_staff.presence_at],
       CASE WHEN COL_LENGTH('dbo.support_staff','capacity') IS NULL
            THEN N'✗ غائب' ELSE N'✓ موجود' END AS [support_staff.capacity];
GO

/*
   ⚠ الأزمنةُ الأربعة، وكلُّ سطرٍ منها يجب أن يحمل ثلاثةَ أرقامٍ لا اثنين.

   السببُ ليس تجميلاً: عيبٌ في هذه الدفعة كان يمحو الحقلين اللذين لم
   يُرسَلا عند تعديل حقلٍ واحد، فيصير الحقلُ الممحوّ «لا هدف» بصمت —
   ويبدو الفريقُ ملتزماً بزمنٍ لم يعد يُقاس. فإن رأيت NULL هنا بعد النشر
   فاضبط الصفّ من إعدادات اللوحة قبل الاعتماد على أرقامها.
*/
SELECT priority                AS [الأولوية],
       first_response_min      AS [أوّل ردّ (د)],
       next_response_min       AS [الردّ التالي (د)],
       resolution_min          AS [المعالجة (د)]
  FROM dbo.support_sla
 ORDER BY CASE priority WHEN 'CRITICAL' THEN 1 WHEN 'URGENT' THEN 2
                        WHEN 'HIGH'     THEN 3 ELSE 4 END;
GO

/* المفاتيحُ الأجنبية الجديدة — إلى جداول الدعم وحدها. */
SELECT fk.name                              AS [المفتاح],
       OBJECT_NAME(fk.parent_object_id)     AS [من],
       OBJECT_NAME(fk.referenced_object_id) AS [إلى]
  FROM sys.foreign_keys fk
 WHERE fk.name IN ('FK_sup_view_thread', 'FK_sup_view_staff');
GO

/* ⚠ ولا شيء ماليّ تغيّر — للتأكّد بعينك: */
SELECT name AS [جدول ماليّ], modify_date AS [آخر تعديل بنيويّ]
  FROM sys.tables
 WHERE name IN ('wallet','InternalEx','EX24AccSafeActivityTb','ExchangeAccData','AccountsTb')
 ORDER BY modify_date DESC;
GO
