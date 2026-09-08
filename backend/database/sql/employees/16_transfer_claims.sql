/* ============================================================================
   منعُ ازدواج الحوالة عند تكرار الطلب — البند: Idempotency
   ============================================================================

   ⚠ **جدولٌ تشغيليّ لا ماليّ.** لا رصيد فيه ولا قيد ولا عمولة؛ صفٌّ يقول
   «هذا الطلب بعينه عولج، ونتيجتُه الحوالة الفلانية». والدفترُ لا يعلم به.

   ── لماذا جدولٌ لا عمودٌ على `transfer_attributions` ─────────────────────

   ⚠ الحجزُ يجب أن يقع **قبل** الكتابة المالية لا بعدها.

   لو كان الحقلُ على جدول النسب لَما أمكن حجزُه إلّا بعد وجود حوالة — وحينها
   يكون الأوان قد فات: طلبان متسارعان يمرّان معاً، فتُكتب حوالتان، ثم يفشل
   الصفُّ الثاني على الفهرس الفريد. المالُ خرج مرّتين والفهرسُ يشتكي بعده.

   فالصفُّ هنا يُكتب أوّلاً بلا رقم حوالة. من نجح إدراجُه يملك حقّ التنفيذ،
   ومن اصطدم بالفهرس الفريد يعرف أن الطلب معالَجٌ أو قيد المعالجة.

   ── ولماذا (employee_id, client_id) لا `client_id` وحده ─────────────────

   المفتاحُ يولّده التطبيق، وتطبيقان على جهازين قد يولّدان المفتاح نفسه.
   وربطُه بالموظف يجعل تصادمَ موظّفين لا أثر له.

   قابل لإعادة التنفيذ: محروسٌ بـ IF NOT EXISTS، ولا DROP ولا حذف بيانات.
   ============================================================================ */

IF OBJECT_ID('dbo.employee_transfer_claims', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_transfer_claims (
        id              BIGINT IDENTITY(1,1) NOT NULL
                        CONSTRAINT PK_emp_tclaim PRIMARY KEY,

        employee_id     BIGINT       NOT NULL,
        agent_id        BIGINT       NOT NULL,

        /* مفتاحُ الطلب كما ولّده التطبيق — لا معنى له خارج هذا الجدول. */
        client_id       VARCHAR(64)  NOT NULL,

        /* يُملأ بعد نجاح الإنشاء. فارغٌ = لم تكتمل بعد. */
        transfer_number VARCHAR(50)  NULL,

        /* PENDING · DONE · FAILED */
        status          VARCHAR(10)  NOT NULL
                        CONSTRAINT DF_emp_tclaim_status DEFAULT ('PENDING'),

        created_at      DATETIME2    NOT NULL
                        CONSTRAINT DF_emp_tclaim_at DEFAULT (SYSDATETIME()),
        completed_at    DATETIME2    NULL,

        CONSTRAINT FK_emp_tclaim_emp FOREIGN KEY (employee_id)
            REFERENCES dbo.employees (id)
    );

    /* ⚠ هذا الفهرس **هو** الحارس، لا شرطُ `IF EXISTS` في الشيفرة: طلبان
       متوازيان يقرآن معاً فيجدانه خالياً، ولا يمنعهما إلّا القاعدة. */
    CREATE UNIQUE INDEX UX_emp_tclaim_key
        ON dbo.employee_transfer_claims (employee_id, client_id);

    CREATE INDEX IX_emp_tclaim_number
        ON dbo.employee_transfer_claims (transfer_number);
END;
GO
