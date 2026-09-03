/* ============================================================================
   خزينة الموظف التشغيلية والورديات — بإذنٍ صريح من المالك (3 سبتمبر 2026)
   ----------------------------------------------------------------------------
   نصّ الإذن: «يسمح لك ببناء الخزائن والعمليات المالية للموظف بما لا يتعارض
   مع العمليات المالية المتّبعة بين الوكيل والرحالة».

   ولذلك هذا **دفترٌ تشغيليّ موازٍ، لا حسابٌ في الشجرة المحاسبية**:

   • لا يُكتب فيه شيء إلى `wallet` ولا `ExchangeAccData` ولا `InternalEx`
     ولا `EX24AccSafeActivityTb` ولا `AccountsTb`.
   • لا يغيّر عمولةً ولا معادلةً قائمة، ولا يرحّل قيداً.
   • يجيب سؤالاً واحداً لا تجيبه المنظومة: **كم نقداً في يد هذا الموظف الآن؟**
     وهو سؤالٌ عن الصندوق في الدرج، لا عن رصيد الوكيل لدى الرحالة.

   وحسابات الوكيل مع الرحالة تبقى كما هي حرفياً: هذا الدفتر يقرأ منها ولا
   يكتب فيها، واختبار القبول يثبت ذلك ببصمةٍ قبل وبعد.

   ── قراران يمنعان أخطاءً كلاسيكية ───────────────────────────────────────
   1. **الرصيد يُحسب ولا يُخزَّن.** لا عمود `balance` في `employee_cashboxes`.
      رصيدٌ مخزَّن يُحدَّث بعد كل حركة ينحرف عن مجموع حركاته عند أول خطأ أو
      انقطاع، ثم لا يعرف أحد أيّهما الصحيح. المجموع يُحسب من الحركات دائماً.

   2. **لا حذف ولا تعديل لحركة.** التصحيح بحركةٍ عكسية (`reversal_of`) —
      والأصل يبقى. دفترٌ يُحذف منه ليس دفتراً.
   ============================================================================ */

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* ============================================================================
   1) employee_cashboxes — خزينة الموظف
   ----------------------------------------------------------------------------
   خزينة واحدة لكل (موظف، نقطة بيع): موظفٌ يعمل في نقطتين له صندوقان، وخلطهما
   يجعل عجز إحداهما يختفي في زيادة الأخرى.
   ============================================================================ */
IF OBJECT_ID('dbo.employee_cashboxes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_cashboxes (
        id               BIGINT IDENTITY(1,1) NOT NULL
                         CONSTRAINT PK_emp_cashboxes PRIMARY KEY,
        agent_id         BIGINT      NOT NULL,
        employee_id      BIGINT      NOT NULL
                         CONSTRAINT FK_cashbox_employee REFERENCES dbo.employees(id),
        point_of_sale_id INT         NULL,      /* AuthorizedUsers.ID */

        currency_code    VARCHAR(10) NOT NULL
                         CONSTRAINT DF_cashbox_currency DEFAULT ('LYD'),

        is_active        BIT         NOT NULL
                         CONSTRAINT DF_cashbox_active DEFAULT (1),

        created_at       DATETIME2   NOT NULL
                         CONSTRAINT DF_cashbox_created DEFAULT (SYSUTCDATETIME())
        /* ⚠ لا عمود رصيد عمداً — انظر القرار 1 أعلاه. */
    );

    CREATE UNIQUE INDEX UX_cashbox_emp_pos
        ON dbo.employee_cashboxes (employee_id, point_of_sale_id, currency_code);
    CREATE INDEX IX_cashbox_agent ON dbo.employee_cashboxes (agent_id);

    PRINT 'أُنشئ: employee_cashboxes';
END ELSE PRINT 'موجود: employee_cashboxes';
GO

/* ============================================================================
   2) employee_shifts — الورديات
   ============================================================================ */
IF OBJECT_ID('dbo.employee_shifts', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_shifts (
        id               BIGINT IDENTITY(1,1) NOT NULL
                         CONSTRAINT PK_emp_shifts PRIMARY KEY,
        agent_id         BIGINT      NOT NULL,
        employee_id      BIGINT      NOT NULL
                         CONSTRAINT FK_shift_employee REFERENCES dbo.employees(id),
        cashbox_id       BIGINT      NOT NULL
                         CONSTRAINT FK_shift_cashbox REFERENCES dbo.employee_cashboxes(id),
        point_of_sale_id INT         NULL,

        /* الافتتاحي كما أعلنه الموظف عند البدء — تصريحٌ لا حساب. */
        opening_cash     DECIMAL(18,3) NOT NULL
                         CONSTRAINT DF_shift_opening DEFAULT (0),

        /* OPEN · CLOSED */
        status           VARCHAR(20) NOT NULL
                         CONSTRAINT DF_shift_status DEFAULT ('OPEN'),

        started_at       DATETIME2   NOT NULL
                         CONSTRAINT DF_shift_started DEFAULT (SYSUTCDATETIME()),
        ended_at         DATETIME2   NULL,
        device_hash      VARCHAR(64) NULL
    );

    /* وردية مفتوحة واحدة لكل موظف — الحارس في القاعدة: ورديتان مفتوحتان
       تجعلان كل حركة تحتمل انتماءين. */
    CREATE UNIQUE INDEX UX_shift_open
        ON dbo.employee_shifts (employee_id) WHERE status = 'OPEN';

    CREATE INDEX IX_shift_employee ON dbo.employee_shifts (employee_id, started_at);
    CREATE INDEX IX_shift_agent    ON dbo.employee_shifts (agent_id, started_at);

    PRINT 'أُنشئ: employee_shifts';
END ELSE PRINT 'موجود: employee_shifts';
GO

/* ============================================================================
   3) employee_cashbox_entries — حركات الخزينة
   ----------------------------------------------------------------------------
   `amount` موجبٌ دائماً و`direction` تقول الاتجاه. مبلغٌ بإشارة يجعل خطأ
   إشارةٍ واحدة يقلب عجزاً إلى زيادة بلا أثر ظاهر.
   ============================================================================ */
IF OBJECT_ID('dbo.employee_cashbox_entries', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_cashbox_entries (
        id               BIGINT IDENTITY(1,1) NOT NULL
                         CONSTRAINT PK_emp_entries PRIMARY KEY,
        agent_id         BIGINT      NOT NULL,
        employee_id      BIGINT      NOT NULL
                         CONSTRAINT FK_entry_employee REFERENCES dbo.employees(id),
        cashbox_id       BIGINT      NOT NULL
                         CONSTRAINT FK_entry_cashbox REFERENCES dbo.employee_cashboxes(id),
        shift_id         BIGINT      NULL
                         CONSTRAINT FK_entry_shift REFERENCES dbo.employee_shifts(id),
        point_of_sale_id INT         NULL,

        /* TRANSFER_DELIVERY · CASH_RECEIVED · CASH_HANDOVER · OPENING
           ADJUSTMENT · REVERSAL */
        transaction_type VARCHAR(40)  NOT NULL,

        /* ما تشير إليه الحركة في المنظومة — قراءةً فقط، بالرقم لا بمفتاح
           أجنبي: قيدٌ إلى دفتر المنظومة يجعل عمليةً مالية تفشل بسبب صفّ خزينة. */
        reference_type   VARCHAR(40)  NULL,
        reference_id     VARCHAR(60)  NULL,

        amount           DECIMAL(18,3) NOT NULL,
        direction        VARCHAR(3)   NOT NULL,   /* IN · OUT */
        currency_code    VARCHAR(10)  NOT NULL
                         CONSTRAINT DF_entry_currency DEFAULT ('LYD'),

        notes            NVARCHAR(500) NULL,

        /* التصحيح بعكسٍ لا بحذف — والأصل يبقى (بند 41). */
        reversal_of      BIGINT       NULL
                         CONSTRAINT FK_entry_reversal REFERENCES dbo.employee_cashbox_entries(id),
        is_reversed      BIT          NOT NULL
                         CONSTRAINT DF_entry_reversed DEFAULT (0),

        /* تحايُد الطلب: نقرتان أو إعادة إرسال لا تُنشئان حركتين (بند 51). */
        client_ref       VARCHAR(80)  NULL,

        device_hash      VARCHAR(64)  NULL,
        created_by       BIGINT       NULL,
        created_at       DATETIME2    NOT NULL
                         CONSTRAINT DF_entry_created DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT CK_entry_direction CHECK (direction IN ('IN','OUT')),
        CONSTRAINT CK_entry_amount    CHECK (amount > 0)
    );

    CREATE UNIQUE INDEX UX_entry_client_ref
        ON dbo.employee_cashbox_entries (employee_id, client_ref)
        WHERE client_ref IS NOT NULL;

    /* حركةٌ واحدة لكل عملية مرجعية: تسليم الحوالة نفسها لا يُسجَّل مرتين. */
    CREATE UNIQUE INDEX UX_entry_reference
        ON dbo.employee_cashbox_entries (reference_type, reference_id)
        WHERE reference_id IS NOT NULL AND reversal_of IS NULL;

    CREATE INDEX IX_entry_cashbox ON dbo.employee_cashbox_entries (cashbox_id, created_at);
    CREATE INDEX IX_entry_shift   ON dbo.employee_cashbox_entries (shift_id);
    CREATE INDEX IX_entry_agent   ON dbo.employee_cashbox_entries (agent_id, created_at);

    PRINT 'أُنشئ: employee_cashbox_entries';
END ELSE PRINT 'موجود: employee_cashbox_entries';
GO

/* ============================================================================
   4) employee_shift_closings — نتيجة الإقفال
   ----------------------------------------------------------------------------
   تُحفظ تاريخياً ولا تُعدَّل: «كم كان متوقّعاً وكم وُجد فعلاً» سؤالٌ تُسأل عنه
   الوردية بعد شهور.
   ============================================================================ */
IF OBJECT_ID('dbo.employee_shift_closings', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_shift_closings (
        id             BIGINT IDENTITY(1,1) NOT NULL
                       CONSTRAINT PK_shift_closings PRIMARY KEY,
        shift_id       BIGINT        NOT NULL
                       CONSTRAINT FK_closing_shift REFERENCES dbo.employee_shifts(id),
        agent_id       BIGINT        NOT NULL,
        employee_id    BIGINT        NOT NULL,

        opening_cash   DECIMAL(18,3) NOT NULL,
        cash_in        DECIMAL(18,3) NOT NULL,
        cash_out       DECIMAL(18,3) NOT NULL,
        expected_cash  DECIMAL(18,3) NOT NULL,   /* opening + in − out */
        actual_cash    DECIMAL(18,3) NOT NULL,   /* ما عدّه الموظف */
        difference     DECIMAL(18,3) NOT NULL,   /* actual − expected */

        /* MATCH · SHORTAGE · SURPLUS */
        result         VARCHAR(20)   NOT NULL,

        notes          NVARCHAR(500) NULL,
        closed_by      BIGINT        NULL,
        closed_at      DATETIME2     NOT NULL
                       CONSTRAINT DF_closing_at DEFAULT (SYSUTCDATETIME())
    );

    /* إقفالٌ واحد لكل وردية. */
    CREATE UNIQUE INDEX UX_closing_shift ON dbo.employee_shift_closings (shift_id);
    CREATE INDEX IX_closing_agent ON dbo.employee_shift_closings (agent_id, closed_at);
    CREATE INDEX IX_closing_result ON dbo.employee_shift_closings (result, closed_at);

    PRINT 'أُنشئ: employee_shift_closings';
END ELSE PRINT 'موجود: employee_shift_closings';
GO

SELECT name AS [الجدول], create_date AS [أُنشئ]
FROM sys.tables
WHERE name IN ('employee_cashboxes','employee_shifts',
               'employee_cashbox_entries','employee_shift_closings')
ORDER BY name;
GO
