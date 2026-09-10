/* ============================================================================
   وردية مفتوحة واحدة لكل موظف — حارسُ الفتح التلقائيّ
   10 سبتمبر 2026

   أمرُ المالك: «الوردية تُفتح بمجرّد فتح يومٍ جديد وبأيّ حركةٍ تتمّ سواء صرف
   أو قبض أو حوالة، تُفتح بشكلٍ آليّ، والإقفالُ يكون يدويّاً بعد أن يتمّ الجرد
   عليه آخر الوردية».

   والفتحُ التلقائيّ يقع من أربعة مسارات (إنشاء حوالة · تنفيذ موافقة · تسليم ·
   حركة خزينة يدوية). فطلبان متسارعان — ضغطتان، أو شبكةٌ أعادت الإرسال — قد
   يمرّان معاً من أيّ فحصٍ في الشيفرة ويفتحان ورديتين لموظّفٍ واحد. وحينها
   تنقسم عهدتُه بين ورديتين ولا يُجرَد على واحدةٍ منهما بصدق.

   ⚠ الحارسُ الحقيقيّ فهرسٌ فريد في القاعدة، لا فحصٌ في الشيفرة. والشيفرةُ
   تلتقط خرقَه وتقرأ الوردية التي كتبها الطلبُ الرابح
   (`EmployeeCashboxService::ensureOpenShift`).

   ⚠ الجدولُ تشغيليّ (`employee_shifts`) لا محاسبيّ: ليس `InternalEx` ولا
   `EX24AccSafeActivityTb` ولا شجرةَ حسابات. ولا يمسّ رصيداً ولا قيداً ولا
   مديناً ولا دائناً في منظومة الرحالة.

   idempotent: لا يُنشئ الفهرس إن وُجد.
   ============================================================================ */

/* ── ١) هل توجد ورديتان مفتوحتان لموظّفٍ واحد الآن؟ ───────────────────────
   إن وُجدت، **لا يُنشأ الفهرس** ويُطبع الموظفون: إغلاقُ إحداهما جردٌ لا
   تنظيفُ بيانات، ولا يقع إلّا بيد صاحبها. عالجها ثم أعد تشغيل السكربت. */
IF EXISTS (
    SELECT employee_id
      FROM dbo.employee_shifts
     WHERE status = 'OPEN'
     GROUP BY employee_id
    HAVING COUNT(*) > 1
)
BEGIN
    PRINT '⚠ لم يُنشأ الفهرس: يوجد موظفون بورديتين مفتوحتين. القائمة:';

    SELECT employee_id, COUNT(*) AS open_shifts,
           MIN(id) AS oldest_shift_id, MAX(id) AS newest_shift_id
      FROM dbo.employee_shifts
     WHERE status = 'OPEN'
     GROUP BY employee_id
    HAVING COUNT(*) > 1;
END
ELSE
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM sys.indexes
         WHERE name = 'UX_shift_open_employee'
           AND object_id = OBJECT_ID('dbo.employee_shifts')
    )
    BEGIN
        CREATE UNIQUE INDEX UX_shift_open_employee
            ON dbo.employee_shifts (employee_id)
            WHERE status = 'OPEN';

        PRINT 'أُنشئ: UX_shift_open_employee';
    END
    ELSE PRINT 'موجود: UX_shift_open_employee';
END
GO
