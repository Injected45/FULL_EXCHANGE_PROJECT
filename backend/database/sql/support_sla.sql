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
