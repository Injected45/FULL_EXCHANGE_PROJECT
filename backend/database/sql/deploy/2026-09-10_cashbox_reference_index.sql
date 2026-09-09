/* ============================================================================
   لجنة فحص تطبيق الصرافة — M10: توسيع فهرس تفرّد حركات الخزينة ليشمل agent_id
   10 سبتمبر 2026

   المشكلة: UX_entry_reference كان فريداً على (reference_type, reference_id)
   **عالمياً**. و reference_id هو رقمُ الحوالة، وقد يتكرّر بين وكيلين — فقيدُ
   خروجٍ لوكيلٍ ثانٍ يصطدم بصفّ الوكيل الأول ويسقط صامتاً، فتنقص عهدةُ موظفه.

   الإصلاح: يُعاد إنشاء الفهرس على (agent_id, reference_type, reference_id).
   وهو **توسيعٌ يُخفّف** القيد (يسمح بتكرار الرقم بين وكلاء مختلفين)، فلا
   يمكن أن تخالفه بياناتٌ قائمة — الفهرسُ العالميّ القديم كان أضيق.

   ⚠ الجدولُ تشغيليّ (employee_cashbox_entries) لا محاسبيّ — ليس InternalEx
   ولا EX24AccSafeActivityTb، فلا يقع تحت حظر الفهرسة. ولا يمسّ رصيداً ولا
   قيداً ولا معادلةً في منظومة الرحالة.

   ⚠ نشرٌ منسّق: يُطبَّق هذا السكربت **مع** نشر الكود (EmployeeCashboxService
   الذي أضاف agent_id إلى فحص التكرار) — تطبيقُ أحدهما دون الآخر يُبقي فجوةً
   في الحالة العابرة للوكلاء.

   idempotent: يُسقط الفهرسَ القديم إن وُجد ثم يُنشئ الجديد إن غاب.
   ============================================================================ */

IF EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UX_entry_reference'
      AND object_id = OBJECT_ID('dbo.employee_cashbox_entries')
)
    DROP INDEX UX_entry_reference ON dbo.employee_cashbox_entries;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UX_entry_reference'
      AND object_id = OBJECT_ID('dbo.employee_cashbox_entries')
)
    CREATE UNIQUE INDEX UX_entry_reference
        ON dbo.employee_cashbox_entries (agent_id, reference_type, reference_id)
        WHERE reference_id IS NOT NULL AND reversal_of IS NULL;
GO
