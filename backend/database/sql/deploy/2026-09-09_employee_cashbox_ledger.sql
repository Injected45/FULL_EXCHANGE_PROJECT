SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

/* ============================================================================
   نقلٌ إلى القاعدة الرئيسية — 9 سبتمبر 2026
   كشفُ خزينة الموظف وتتبُّعُ منفِّذ التسليم
   ============================================================================

   يُنفَّذ على قاعدة EXCHANGESYS2026 الرئيسية.

   ⚠ **إضافةٌ محضة**: لا جدولَ جديد، ولا DROP، ولا DELETE، ولا UPDATE على أيّ
   صفٍّ قائم. عمودان وفهرسان، وكلُّ خطوةٍ محروسةٌ بوجودها فيُعاد تنفيذُ
   السكربت بلا أثر.

   ⚠ **ولا مساسَ بأيّ جدولٍ ماليّ**: الجدولان المُعدَّلان
   (`transfer_status_history` و`employee_cashbox_entries`) أنشأهما هذا
   المشروع، وليس فيهما رصيدٌ ولا قيدٌ ولا عمولة. و`InternalEx` و
   `EX24AccSafeActivityTb` و`wallet` و`AccountsTb` **لا تُذكَر هنا أصلاً**.

   ── ماذا يفعل ──────────────────────────────────────────────────────────────

   1. `transfer_status_history.changed_by_employee_id`
      صفُّ تحوُّل الحالة يحمل `changed_by` = معرّفَ الوكيل في المسارين، لأن
      الموظف ينفّذ بوصفه واجهةً من وكيله. فبلا هذا العمود لا يُفرَّق تسليمُ
      الوكيل من تسليم موظّفه إلّا بالتوفيق بين ثلاثة جداول بالتاريخ.
      القديمُ يبقى NULL — وهو الصادق: لم يكن يُعرف.

   2. `IX_tsh_employee` — فهرسُ سؤال الجرد: «ماذا سلّم هذا الموظف؟»

   3. `IX_entry_employee_at` — فهرسُ الكشف: «حركاتُ هذا الموظف بين تاريخين».
      الفهارسُ القائمة على cashbox_id و shift_id و agent_id، ولا واحدَ منها
      يخدم نداءَ كلِّ فتحةٍ للشاشة.

   ⚠ **ولا يُصحَّح صفٌّ سابق.** أُصلح في الشيفرة اصطدامٌ كان يُسقط قيدَ
   خروجٍ صامتاً (نوعٌ مرجعيّ مشترك بين الإنشاء والتسليم — صار
   `INTERNAL_TRANSFER_CREATED` للإنشاء). والصفوفُ التي سبقت الإصلاح تبقى كما
   هي: الحركةُ لا تُعدَّل ولا تُحذف، والتصحيحُ — إن لزم — يكون بحركةٍ عكسية
   يسجّلها الوكيل، لا بكتابةٍ من سكربت نقل.
   ============================================================================ */

PRINT '── 1) transfer_status_history.changed_by_employee_id ──';
GO

IF OBJECT_ID('dbo.transfer_status_history', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.transfer_status_history', 'changed_by_employee_id') IS NULL
BEGIN
    ALTER TABLE dbo.transfer_status_history
        ADD changed_by_employee_id BIGINT NULL;

    PRINT '   أُضيف العمود.';
END ELSE PRINT '   موجود سلفاً.';
GO

IF OBJECT_ID('dbo.transfer_status_history', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.transfer_status_history', 'changed_by_employee_id') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.indexes
                   WHERE name = 'IX_tsh_employee'
                     AND object_id = OBJECT_ID('dbo.transfer_status_history'))
BEGIN
    CREATE INDEX IX_tsh_employee
        ON dbo.transfer_status_history (changed_by_employee_id, changed_at)
        WHERE changed_by_employee_id IS NOT NULL;

    PRINT '   أُنشئ IX_tsh_employee.';
END ELSE PRINT '   IX_tsh_employee موجود سلفاً.';
GO

PRINT '';
PRINT '── 2) فهرس كشف الخزينة ──';
GO

IF OBJECT_ID('dbo.employee_cashbox_entries', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.indexes
                   WHERE name = 'IX_entry_employee_at'
                     AND object_id = OBJECT_ID('dbo.employee_cashbox_entries'))
BEGIN
    CREATE INDEX IX_entry_employee_at
        ON dbo.employee_cashbox_entries (employee_id, created_at)
        INCLUDE (direction, amount, transaction_type, reference_type,
                 reference_id, shift_id, is_reversed, reversal_of, notes);

    PRINT '   أُنشئ IX_entry_employee_at.';
END ELSE PRINT '   موجود سلفاً.';
GO

/* ── تحقّقٌ لا ثقة ─────────────────────────────────────────────────────── */

PRINT '';
PRINT '── التحقّق ──';
GO

SELECT
    CASE WHEN COL_LENGTH('dbo.transfer_status_history','changed_by_employee_id')
         IS NOT NULL THEN N'موجود' ELSE N'غائب' END AS [changed_by_employee_id],
    CASE WHEN EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tsh_employee')
         THEN N'موجود' ELSE N'غائب' END AS [IX_tsh_employee],
    CASE WHEN EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_entry_employee_at')
         THEN N'موجود' ELSE N'غائب' END AS [IX_entry_employee_at];
GO

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT ' 2026-09-09 — تمّ. لا جدولَ جديد · لا صفَّ عُدِّل أو حُذف';
PRINT '════════════════════════════════════════════════════════════';
GO
