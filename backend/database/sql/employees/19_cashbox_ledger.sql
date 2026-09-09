SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

/* ============================================================================
   كشفُ خزينة الموظف وتتبُّعُ من نفّذ التسليم — 9 سبتمبر 2026
   ============================================================================

   ⚠ **لا جدولَ ماليّاً جديد، ولا رصيدَ مخزَّن.** الكشفُ يُحسب من الحركات
   الموجودة أصلاً في `employee_cashbox_entries` مع افتتاحيّ الوردية من
   `employee_shifts` — ورصيدٌ مخزَّنٌ ثانٍ كان سيختلف عن حركاته عند أول
   انقطاع، فلا يعرف أحدٌ بعدها أيَّهما الصحيح.

   فهذا السكربت لا يضيف إلّا **عمودين وفهرسين**:

   ───────────────────────────────────────────────────────────────────────────
   1) `transfer_status_history.changed_by_employee_id`
   ───────────────────────────────────────────────────────────────────────────

   جدولُ تحوُّلات الحالة يسجّل `changed_by` — ومسارُ الموظف يمرّ فيه **بمعرّف
   وكيله** لأن الموظف ينفّذ بوصفه واجهةً من الوكيل. فالنتيجة أن صفَّ التحوُّل
   لا يُفرّق بين تسليمٍ نفّذه الوكيل وتسليمٍ نفّذه موظّفُه.

   والهويةُ محفوظةٌ فعلاً في `transfer_attributions` و`audit_logs`، لكن
   **صفَّ التحوُّل نفسَه** يجب أن يقول من حوّل الحالة: من يقرأ تاريخ الحوالة
   يقرأ هذا الجدول، لا ثلاثةَ جداول يوفّق بينها بالتاريخ.

   ⚠ عمودٌ ثانٍ لا استبدالُ `changed_by`: ذاك يعني معرّف الوكيل في مسار
   الوكيل، وإعادةُ تعريفه تكسر كلَّ صفٍّ كُتب قبل اليوم. والقديمُ يبقى
   `NULL` هنا — وهو الصادق: لم يكن يُعرف.

   ───────────────────────────────────────────────────────────────────────────
   2) `employee_cashbox_entries.reference_type` — إصلاحُ اصطدام
   ───────────────────────────────────────────────────────────────────────────

   ⚠ **هذا إصلاحُ ثقبٍ محاسبيّ صامت، لا تحسينٌ.**

   الفهرسُ `UX_entry_reference` فريدٌ على `(reference_type, reference_id)`،
   وكان مسارُ الإنشاء ومسارُ التسليم يكتبان النوعَ نفسَه `INTERNAL_TRANSFER`.
   فحوالةٌ أنشأها الموظف ثم سُلّمت عند وكيله نفسِه — وهو أمرٌ عاديّ حين تكون
   نقطةُ الوصول تابعةً للوكيل ذاته — تُصطدم حركتُها الثانية بالفهرس،
   و`addEntry` تُعيد `duplicate` **بلا إدراجٍ وبلا استثناء**.

   الأثر: قيدُ الخروج يسقط صامتاً، فتزيد عهدةُ الموظف بقيمة الحوالة، ويُظهر
   إقفالُ الوردية **زيادةً وهميّة** — وكأنّ معه نقداً أكثر ممّا يملك.

   والإصلاحُ أنّ الإنشاء يكتب `INTERNAL_TRANSFER_CREATED` والتسليم يُبقي
   `INTERNAL_TRANSFER`. فيعود الفهرسُ يحرس ما وُضع له: **قيدٌ واحدٌ لكلّ
   إجراءٍ على الحوالة** لا قيدٌ واحد للحوالة كلِّها.

   ⚠ والصفوفُ السابقة **لا تُمسّ**: تصحيحُها يعني كتابةً في سجلٍّ ماليّ
   تشغيليّ، والقاعدةُ أن الحركة لا تُعدَّل ولا تُحذف. الكشفُ يعرضها كما هي،
   والتعامد الجديد يبدأ من اليوم.

   ولذلك يبقى الفهرسُ نفسُه بلا تغيير — النوعُ الجديد يمرّ فيه بطبيعته.

   ───────────────────────────────────────────────────────────────────────────
   قابل لإعادة التنفيذ: كلُّ خطوة محروسةٌ بوجودها. لا DROP، ولا DELETE،
   ولا UPDATE على أيّ صفٍّ قائم.
   ============================================================================ */

/* ── 1) من نفّذ تحوُّل الحالة ─────────────────────────────────────────── */

IF OBJECT_ID('dbo.transfer_status_history', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.transfer_status_history', 'changed_by_employee_id') IS NULL
BEGIN
    ALTER TABLE dbo.transfer_status_history
        ADD changed_by_employee_id BIGINT NULL;

    PRINT 'أُضيف: transfer_status_history.changed_by_employee_id';
END ELSE PRINT 'موجود: transfer_status_history.changed_by_employee_id';
GO

/* فهرسٌ لسؤالٍ واحد: «ماذا سلّم هذا الموظف؟» — وهو سؤالُ الجرد والرقابة.
   ⚠ مُرشَّح على NOT NULL: أكثرُ الصفوف مسارُ وكيلٍ لا موظف. */
IF OBJECT_ID('dbo.transfer_status_history', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.transfer_status_history', 'changed_by_employee_id') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.indexes
                   WHERE name = 'IX_tsh_employee'
                     AND object_id = OBJECT_ID('dbo.transfer_status_history'))
BEGIN
    CREATE INDEX IX_tsh_employee
        ON dbo.transfer_status_history (changed_by_employee_id, changed_at)
        WHERE changed_by_employee_id IS NOT NULL;

    PRINT 'أُنشئ: IX_tsh_employee';
END ELSE PRINT 'موجود: IX_tsh_employee';
GO

/* ── 2) فهرسُ الكشف ───────────────────────────────────────────────────────

   الكشفُ يُقرأ بـ`(employee_id, created_at)` ويُرتَّب بالتاريخ ثم بالمعرّف.
   الفهارسُ القائمة على `cashbox_id` و`shift_id` و`agent_id` — ولا واحدَ منها
   يخدم «حركاتُ هذا الموظف بين تاريخين»، وهو نداءُ كلِّ فتحةٍ للشاشة.

   ⚠ جدولٌ تشغيليّ لا ماليّ (لا رصيد ولا قيد ولا عمولة)، فالفهرسُ عليه لا
   يمسّ الخطَّ الأحمر — والحظرُ على `InternalEx` و`EX24AccSafeActivityTb`
   وأمثالهما، لا على سجلّ خزينةٍ أنشأه هذا المشروع.
   ──────────────────────────────────────────────────────────────────────── */

IF OBJECT_ID('dbo.employee_cashbox_entries', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.indexes
                   WHERE name = 'IX_entry_employee_at'
                     AND object_id = OBJECT_ID('dbo.employee_cashbox_entries'))
BEGIN
    CREATE INDEX IX_entry_employee_at
        ON dbo.employee_cashbox_entries (employee_id, created_at)
        INCLUDE (direction, amount, transaction_type, reference_type,
                 reference_id, shift_id, is_reversed, reversal_of, notes);

    PRINT 'أُنشئ: IX_entry_employee_at';
END ELSE PRINT 'موجود: IX_entry_employee_at';
GO

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT ' 19_cashbox_ledger.sql — تمّ';
PRINT ' لا جدول جديد · لا رصيد مخزَّن · لا صفَّ عُدِّل أو حُذف';
PRINT '════════════════════════════════════════════════════════════';
GO
