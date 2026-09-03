/* ============================================================
   عكس حالة المنظومة في صفوف تتبّع الوكيل — قراءةً فقط.

   المشكلة: الحوالة قد تُلغى في المنظومة بعد وصولها إلى الوكيل
   (ConfirmType ينتقل إلى 3/4 «قيد الإلغاء» أو 5 «ملغية» أو 6 «ملغية
   مسلمة»). وحينها تخرج من ترشيح المزامنة (= 2) فتبقى في جدولنا
   «بانتظار التسليم» إلى الأبد — والوكيل قد يسلّم مالاً لحوالة ملغاة.

   الحلّ: عمودٌ يعكس حالة المنظومة، **ولا يخلط** بحالة تسليم الوكيل.
   يبقى `status` ملكاً للوكيل (PENDING_DELIVERY / DELIVERED)، ويقول
   `core_confirm_type` ما تقوله المنظومة. لا يُكتب في InternalEx شيء.

   عمودان لا واحد: الرقم للمنطق، والاسم للعرض ولقراءة السجلّ بعد سنة
   حين لا يتذكّر أحد أن 6 تعني «ملغية مسلمة».
   ============================================================ */

IF COL_LENGTH('dbo.agent_incoming_transfers', 'core_confirm_type') IS NULL
BEGIN
    ALTER TABLE dbo.agent_incoming_transfers
        ADD core_confirm_type  INT           NULL,
            core_status_label  NVARCHAR(50)  NULL,
            core_synced_at     DATETIME2     NULL;
END;
GO

/* الاستعلام الأكثر استعمالاً يصير: حوالات وكيلٍ غير الملغاة. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE name = 'IX_ait_agent_core'
                 AND object_id = OBJECT_ID('dbo.agent_incoming_transfers'))
BEGIN
    CREATE INDEX IX_ait_agent_core
        ON dbo.agent_incoming_transfers (agent_id, core_confirm_type);
END;
GO
