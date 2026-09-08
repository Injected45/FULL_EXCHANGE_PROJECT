/* ============================================================================
   سقفُ تحويل الموظف، وطلبُ موافقة الوكيل عند تجاوزه
   ============================================================================

   ⚠ **طبقةُ تفويضٍ قبل الدفتر، لا داخلَه.**

   لا شيء هنا يمسّ الشجرة المحاسبية ولا الأرصدة ولا القيود ولا العمولات ولا
   الخزائن. الحوالةُ حين تُنفَّذ تُكتب بمسار الوكيل نفسِه حرفاً بحرف
   (`EmployeeActsAsAgent`)، وهذه الجداولُ تقرّر **متى يُسمح للطلب بالوصول
   إلى ذلك المسار** — لا أكثر.

   والفرقُ جوهريّ: طلبٌ «بانتظار موافقة الوكيل» ليس حوالةً غير منفَّذة، بل
   **ليس حوالةً بعد**. لا صفَّ له في `InternalEx`، ولا رصيدَ خُصم، ولا عمولةَ
   احتُسبت. فإن رُفض، لم يقع شيءٌ يُلغى.

   ── ثلاثة جداول؟ لا، جدولان وعمودان ──────────────────────────────────────

   السقفُ سياسةٌ لكل موظف، والتصعيدُ طلبٌ لكل حوالة. أمّا «هل حوّل هذا
   الموظفُ لهذا المستفيد قبل ساعة؟» فسؤالٌ يجيبه جدولٌ **قائم** هو
   `transfer_attributions` — وهو منذ نشأته الموضعُ الذي يقول «من حرّك يده».
   فيُوسَّع بعمودين بدل جدولٍ ثالث يقول الشيء نفسَه.

   قابل لإعادة التنفيذ: محروسٌ بـ IF، ولا DROP ولا حذف بيانات.
   ============================================================================ */

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

/* ============================================================================
   1) سياسةُ الموظف — سقفٌ لكل موظف على حدة
   ============================================================================

   ⚠ **لا صفَّ يعني لا سقف**، وهو سلوك اليوم بالضبط.

   فالجدولُ يبدأ فارغاً، وكلُّ موظفٍ قائمٍ اليوم يبقى يعمل كما كان حرفياً
   حتى يضع له وكيلُه سقفاً. ولو كان الافتراضُ سقفاً مخترعاً لتوقّف عملُ
   موظفين يعملون الآن، في اللحظة التي يُنشر فيها هذا الملف.

   ⚠ وكلُّ عمودِ سقفٍ يقبل NULL، وNULL = **بلا حدّ لهذا البند وحدَه**. فوكيلٌ
   يريد سقفاً للحوالة الواحدة دون سقفٍ تراكميّ يملأ الأوّل ويترك الثاني.
   والصفرُ ليس بديلاً عن NULL: صفرٌ يعني «لا يُسمح بشيء»، وهو قرارٌ آخر
   تماماً — وقد يريده الوكيل فعلاً لموظفٍ يريد إيقاف إنشائه للحوالات مؤقتاً
   دون سحب الصلاحية.
   ============================================================================ */

IF OBJECT_ID('dbo.employee_transfer_policies', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_transfer_policies (
        id                  BIGINT IDENTITY(1,1) NOT NULL
                            CONSTRAINT PK_emp_policy PRIMARY KEY,

        employee_id         BIGINT        NOT NULL,
        /* يُنسخ للعزل بين الوكلاء في الاستعلام — لا للتفويض. */
        agent_id            BIGINT        NOT NULL,

        /* سقفُ الحوالة الواحدة. NULL = بلا سقف. */
        per_transfer_limit  DECIMAL(18,3) NULL,

        /* السقفُ التراكميّ ونافذتُه. NULL = معطّل (البند 19: اختياريّ). */
        cumulative_limit    DECIMAL(18,3) NULL,
        cumulative_hours    INT           NULL,

        /*
         * نافذةُ مراقبة تكرار المستفيد بالدقائق.
         *
         * ⚠ 60 دقيقة هي ما اعتمده المالك نصّاً (البند 13)، وهي هنا **عمودٌ
         * لا ثابتٌ في الشيفرة** — تتغيّر بتحديث صفّ، بلا إصدارِ تطبيق.
         */
        recipient_minutes   INT           NOT NULL
                            CONSTRAINT DF_emp_policy_recip DEFAULT (60),

        /*
         * مدّةُ صلاحية طلب الموافقة بالساعات.
         *
         * ⚠ البند 27 يمنع تركَ الطلب صالحاً للأبد، والبند 49 يمنع اختراعَ
         * قيمة. فالقيمةُ ظاهرةٌ للوكيل في الواجهة يغيّرها متى شاء، وهذه
         * نقطةُ البداية لا حكمٌ مغلق.
         */
        approval_ttl_hours  INT           NOT NULL
                            CONSTRAINT DF_emp_policy_ttl DEFAULT (24),

        updated_by          BIGINT        NULL,
        created_at          DATETIME2     NOT NULL
                            CONSTRAINT DF_emp_policy_created DEFAULT (SYSDATETIME()),
        updated_at          DATETIME2     NOT NULL
                            CONSTRAINT DF_emp_policy_updated DEFAULT (SYSDATETIME()),

        CONSTRAINT FK_emp_policy_emp FOREIGN KEY (employee_id)
            REFERENCES dbo.employees (id)
    );

    /* سياسةٌ واحدة لكل موظف — وإلّا صار «ما سقفُه؟» سؤالاً بجوابين. */
    CREATE UNIQUE INDEX UX_emp_policy_emp
        ON dbo.employee_transfer_policies (employee_id);

    CREATE INDEX IX_emp_policy_agent
        ON dbo.employee_transfer_policies (agent_id);
END;
GO

/* ============================================================================
   2) طلبُ الموافقة — حوالةٌ لم تُنفَّذ بعد
   ============================================================================

   ⚠ ما يُحفظ هنا هو **طلبُ الموظف كما أرسله**، لا حوالة. و`payload` نصُّ
   JSON يُعاد تمريرُه إلى مسار الوكيل حرفياً عند الموافقة — فلا تُعاد كتابةُ
   بيانات الحوالة ولا يُعاد تجميعُها من حقولٍ متفرّقة، إذ أن كلَّ إعادةِ
   تجميعٍ فرصةٌ لأن يُنفَّذ غيرُ ما راجعه الوكيل.

   ── الحالات ──────────────────────────────────────────────────────────────

     PENDING   بانتظار قرار الوكيل
     APPROVED  وافق الوكيل، والتنفيذ جارٍ أو تمّ
     REJECTED  رفض الوكيل — لا يدخل الدفترَ أصلاً
     EXPIRED   مضت مدّتُه بلا قرار
     CANCELLED سحبه الموظف قبل القرار
     FAILED    وافق الوكيل ثمّ رفضه المسارُ الماليّ (رصيد، مهلة، حدّ مركزيّ)

   ⚠ و`FAILED` ليست حالةً زائدة: موافقةُ الوكيل تعالج **سببَ التصعيد وحدَه**
   (البند 46)، ولا تتجاوز رصيداً ولا قاعدةَ دقيقةٍ ولا سقفاً مركزياً. فلا بدّ
   من حالةٍ تقول «وافق، ولم تمرّ» — وإلّا ظهرت كأنها نُفِّذت.
   ============================================================================ */

IF OBJECT_ID('dbo.employee_approval_requests', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.employee_approval_requests (
        id                  BIGINT IDENTITY(1,1) NOT NULL
                            CONSTRAINT PK_emp_appr PRIMARY KEY,

        agent_id            BIGINT        NOT NULL,
        employee_id         BIGINT        NOT NULL,
        point_of_sale_id    INT           NULL,
        session_id          BIGINT        NULL,
        device_hash         VARCHAR(64)   NULL,

        /*
         * مفتاحُ الطلب من التطبيق — هو نفسُه `client_id` في
         * `employee_transfer_claims`، عمداً: ضغطةٌ واحدة من الموظف تعني
         * مفتاحاً واحداً، سواءٌ مرّت مباشرةً أو صُعّدت. فلا يُنشئ التكرارُ
         * طلبَي موافقة (البند 24).
         */
        client_id           VARCHAR(64)   NOT NULL,

        /* طلبُ الحوالة كما أرسله الموظف — يُعاد تمريرُه كما هو. */
        payload             NVARCHAR(MAX) NOT NULL,

        amount              DECIMAL(18,3) NOT NULL,
        currency_code       NVARCHAR(20)  NULL,

        /*
         * المستفيد — للعرض على الوكيل، ولفحص التكرار.
         *
         * و`recipient_phone` مخزّنٌ بصيغة الخادم نفسِها (تسع خانات) لا كما
         * كُتب، وإلّا لم يتطابق «0912…» مع «912…» وهما رقمٌ واحد.
         */
        recipient_name      NVARCHAR(200) NULL,
        recipient_name_norm NVARCHAR(200) NULL,
        recipient_phone     VARCHAR(20)   NULL,

        /*
         * أسبابُ التصعيد مجموعةً بفواصل — طلبٌ واحد وإن اجتمعت الأسباب
         * (البند 23). مثل: 'PER_TRANSFER,RECIPIENT_REPEAT'
         */
        reasons             VARCHAR(200)  NOT NULL,

        /*
         * ⚠ لقطةُ السياسة وقتَ الطلب (البند 43).
         *
         * فلو رفع الوكيل السقفَ غداً، بقي مكتوباً أن هذا الطلب صُعِّد لأن
         * سقفَه يومَها كان كذا. وبدونها يقرأ المدقّقُ طلباً بلا سبب ظاهر،
         * لأن السببَ اختفى مع تغيّر الرقم.
         */
        policy_snapshot     NVARCHAR(1000) NULL,

        /* لقطةُ الهويّة التشغيلية (البند 33) — الاسمُ قد يتغيّر، والسجلّ لا. */
        employee_name_snap  NVARCHAR(200) NULL,
        pos_name_snap       NVARCHAR(200) NULL,

        status              VARCHAR(20)   NOT NULL
                            CONSTRAINT DF_emp_appr_status DEFAULT ('PENDING'),

        /* من قرّر ومتى — users.id للوكيل. */
        decided_by          BIGINT        NULL,
        decided_at          DATETIME2     NULL,
        decision_note       NVARCHAR(500) NULL,

        /* نتيجةُ التنفيذ بعد الموافقة. */
        transfer_number     VARCHAR(50)   NULL,
        failure_reason      NVARCHAR(500) NULL,
        executed_at         DATETIME2     NULL,

        expires_at          DATETIME2     NULL,
        created_at          DATETIME2     NOT NULL
                            CONSTRAINT DF_emp_appr_created DEFAULT (SYSDATETIME()),
        updated_at          DATETIME2     NOT NULL
                            CONSTRAINT DF_emp_appr_updated DEFAULT (SYSDATETIME()),

        CONSTRAINT FK_emp_appr_emp FOREIGN KEY (employee_id)
            REFERENCES dbo.employees (id)
    );

    /*
     * ⚠ مفتاحٌ واحد = طلبٌ واحد، يحرسه **الفهرس** لا فحصٌ في الشيفرة.
     *
     * ضغطتان متسارعتان تمرّان معاً على أي `EXISTS`؛ أمّا الإدراجُ فينجح
     * مرّةً واحدة والثانية تصطدم — فتُقرأ وتُعاد نتيجةُ الأولى.
     */
    CREATE UNIQUE INDEX UX_emp_appr_key
        ON dbo.employee_approval_requests (employee_id, client_id);

    /* شاشةُ الوكيل: طلباتُه مرتّبةً بالأحدث، مع تصفيةٍ بالحالة. */
    CREATE INDEX IX_emp_appr_agent
        ON dbo.employee_approval_requests (agent_id, status, created_at DESC);

    /*
     * ⚠ فهرسُ فحص التكرار (البند 48).
     *
     * الفحصُ يجري **قبل كل حوالة**، فلا يجوز أن يكون مسحاً للجدول. وترتيبُ
     * الأعمدة هو ترتيبُ السؤال: هذا الموظف ⇦ هذا الرقم ⇦ في هذه النافذة.
     */
    CREATE INDEX IX_emp_appr_recipient
        ON dbo.employee_approval_requests (employee_id, recipient_phone, created_at DESC);

    CREATE INDEX IX_emp_appr_employee
        ON dbo.employee_approval_requests (employee_id, status, created_at DESC);
END;
GO

/* ============================================================================
   3) المستفيدُ على جدول النسبة القائم — عمودان لا جدولٌ ثالث
   ============================================================================

   ⚠ `transfer_attributions` هو منذ نشأته الموضعُ الذي يقول «من نفّذ هذه
   الحوالة، من أي نقطة بيع، بأي جهاز، وبكم». وسؤالُ التكرار — «هل حوّل هذا
   الموظفُ لهذا الرقم قبل ساعة؟» — امتدادٌ مباشر لذلك، لا موضوعٌ آخر.

   وجدولٌ ثالث كان سيعني صفّين لحوالةٍ واحدة يفترقان عند أوّل إخفاقٍ جزئيّ،
   ثمّ سؤالاً بلا جوابٍ واحد.

   ⚠ وهو **جدولٌ تشغيليّ لا ماليّ**: لا رصيد فيه ولا قيد ولا عمولة، والدفترُ
   لا يعلم به. فتوسيعُه لا يمسّ المنطق الماليّ في شيء.
   ============================================================================ */

IF COL_LENGTH('dbo.transfer_attributions', 'recipient_phone') IS NULL
BEGIN
    ALTER TABLE dbo.transfer_attributions ADD
        recipient_phone     VARCHAR(20)   NULL,
        recipient_name_norm NVARCHAR(200) NULL;
END;
GO

/*
   فهرسُ نافذة المستفيد على الحوالات المنفَّذة.

   ⚠ ومُرشَّحٌ على `CREATED` وحدها: الجدولُ يحمل التسليمَ أيضاً، والتسليمُ
   ليس إنشاءً فلا يدخل حسابَ السقف ولا حسابَ التكرار. وترشيحُ الفهرس يمنع
   الخطأ في القراءة ويصغّره في آنٍ واحد.
*/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tattr_recipient'
               AND object_id = OBJECT_ID('dbo.transfer_attributions'))
    CREATE INDEX IX_tattr_recipient
        ON dbo.transfer_attributions (employee_id, recipient_phone, occurred_at DESC)
        WHERE action = 'CREATED';
GO

/*
   فهرسُ السقف التراكميّ: مجموعُ ما أنشأه موظفٌ في نافذة.

   ⚠ `amount` في قائمة التضمين لا في المفتاح: الاستعلامُ يجمعها ولا يبحث
   بها، فتضمينُها يجعل الفهرس مكتفياً بنفسِه ولا يعود إلى الجدول أصلاً.
*/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tattr_emp_window'
               AND object_id = OBJECT_ID('dbo.transfer_attributions'))
    CREATE INDEX IX_tattr_emp_window
        ON dbo.transfer_attributions (employee_id, occurred_at DESC)
        INCLUDE (amount)
        WHERE action = 'CREATED';
GO
