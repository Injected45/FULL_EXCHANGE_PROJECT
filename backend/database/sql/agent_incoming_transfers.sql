/* ============================================================
   تتبّع تسليم الحوالات الواردة للوكيل — جداول مستقلّة تماماً.

   قرار المالك (2 سبتمبر 2026)، وهو شرط قبولٍ لا تفضيل:

     «حالة مسلمة وغير مسلمة الحالية هي المعتمدة وهي التي تقوم عليها
      العمليات الحسابية ولا أريد المساس بها. أما الحوالات الواردة فهي
      عملية توضيح للتطبيق للقراءة فقط.»

   فهذه الجداول **لا تكتب ولا تقرأ قراراً** من dbo.InternalEx.ConfirmType،
   ولا تمسّ wallet ولا أي قيد أو رصيد أو عمولة. تغيير الحالة هنا يعني
   «سجّل الوكيل أنه سلّم» ولا يعني تسويةً ولا خصماً ولا إضافة.

   ولأن نقطة «الحوالات الواردة» صارت ترشّح على ConfirmType = 1 (معتمدة
   وغير مسلّمة)، تختفي الحوالة من الـ View لحظة اعتمادها مسلّمة في
   المنظومة. لذلك تُحفَظ هنا **نسخة** من بيانات الحوالة لا إشارة إليها،
   وإلا اختفى من تبويب «تم التسليم» ما سُلّم فعلاً.

   يُنفَّذ يدوياً على القاعدة. لا يُستعمل `php artisan migrate` إطلاقاً:
   مجلّد الترحيلات لا يصف هذا المخطّط، و migrate:fresh يُسقط جداول حيّة.
   ============================================================ */

/* ---------- 1) الحوالات الواردة وحالة تسليمها عند الوكيل ---------- */
IF OBJECT_ID('dbo.agent_incoming_transfers', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.agent_incoming_transfers (
        id                  BIGINT IDENTITY(1,1) PRIMARY KEY,

        /* الوكيل من users.id — يُشتقّ من التوثيق لا مما يرسله الهاتف. */
        agent_id            BIGINT        NOT NULL,

        /* رقم الحوالة كما في InternalEx.Code. مفتاح الربط الوحيد
           بالمنظومة، وهو ربطُ قراءةٍ لا مفتاح أجنبيّ يفرض سلوكاً عليها. */
        transfer_number     VARCHAR(50)   NOT NULL,

        /* نسخة البيانات وقت الاستلام — تبقى بعد خروج الحوالة من الـ View. */
        beneficiary_name    NVARCHAR(200) NULL,
        beneficiary_phone   VARCHAR(30)   NULL,
        sender_name         NVARCHAR(200) NULL,
        amount              DECIMAL(18,3) NULL,
        currency_id         INT           NULL,
        branch_delivered_id INT           NULL,

        /* PENDING_DELIVERY | DELIVERED — لا قيمة ثالثة اليوم. */
        status              VARCHAR(30)   NOT NULL
                            CONSTRAINT DF_ait_status DEFAULT ('PENDING_DELIVERY'),

        received_at         DATETIME2     NOT NULL
                            CONSTRAINT DF_ait_received DEFAULT (SYSUTCDATETIME()),
        delivered_at        DATETIME2     NULL,
        delivered_by        BIGINT        NULL,

        created_at          DATETIME2     NOT NULL
                            CONSTRAINT DF_ait_created DEFAULT (SYSUTCDATETIME()),
        updated_at          DATETIME2     NOT NULL
                            CONSTRAINT DF_ait_updated DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT CK_ait_status
            CHECK (status IN ('PENDING_DELIVERY', 'DELIVERED'))
    );

    /* حارس التكرار الحقيقي: صفٌّ واحد لكل (وكيل، حوالة). طلبان متزامنان
       لتسليم الحوالة نفسها يصطدمان بالفهرس لا بمنطقٍ في التطبيق. */
    CREATE UNIQUE INDEX UX_ait_agent_transfer
        ON dbo.agent_incoming_transfers (agent_id, transfer_number);

    /* أكثر استعلام استعمالاً: حوالات وكيلٍ بحالةٍ ما. */
    CREATE INDEX IX_ait_agent_status
        ON dbo.agent_incoming_transfers (agent_id, status)
        INCLUDE (transfer_number, delivered_at);

    CREATE INDEX IX_ait_phone   ON dbo.agent_incoming_transfers (beneficiary_phone);
    CREATE INDEX IX_ait_created ON dbo.agent_incoming_transfers (created_at);
END;

/* ---------- 2) سجلّ تغيّر الحالات ---------- */
IF OBJECT_ID('dbo.transfer_status_history', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.transfer_status_history (
        id              BIGINT IDENTITY(1,1) PRIMARY KEY,

        /* صفّ agent_incoming_transfers لا حوالة المنظومة. */
        transfer_id     BIGINT        NOT NULL,
        transfer_number VARCHAR(50)   NOT NULL,

        old_status      VARCHAR(30)   NULL,
        new_status      VARCHAR(30)   NOT NULL,

        changed_by      BIGINT        NULL,
        changed_at      DATETIME2     NOT NULL
                        CONSTRAINT DF_tsh_changed DEFAULT (SYSUTCDATETIME()),

        /* أثر التتبّع. تُملأ من الطلب لا من جسمه. */
        ip_address      VARCHAR(64)   NULL,
        device_id       VARCHAR(128)  NULL,
        session_id      VARCHAR(128)  NULL,
        notes           NVARCHAR(500) NULL,

        CONSTRAINT FK_tsh_transfer FOREIGN KEY (transfer_id)
            REFERENCES dbo.agent_incoming_transfers (id)
    );

    CREATE INDEX IX_tsh_transfer ON dbo.transfer_status_history (transfer_id, changed_at);
    CREATE INDEX IX_tsh_number   ON dbo.transfer_status_history (transfer_number);
END;
