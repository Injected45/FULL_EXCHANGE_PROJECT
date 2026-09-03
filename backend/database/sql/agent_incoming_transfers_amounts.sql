/* ============================================================
   حقول المبلغ الناقصة في تتبّع الحوالات الواردة.

   خطأ أُصلح قبل أن يظهر على شاشة: كان `amount` يُملأ من `ExVal` وهي
   **العمولة**، والمبلغ المُرسل هو `OverallVal`. الشاشة كانت ستعرض
   للوكيل عمولةً في موضع المبلغ — رقمٌ صغير مكان رقم كبير في تطبيق
   حوالات.

   وتُضاف هنا بقيّة ما تعرضه بطاقة الحوالة، لأن الصفّ يجب أن يبقى مقروءاً
   بعد خروج الحوالة من الـ View (بإلغائها أو بتغيّر حالتها في المنظومة).
   ============================================================ */

IF COL_LENGTH('dbo.agent_incoming_transfers', 'commission') IS NULL
BEGIN
    ALTER TABLE dbo.agent_incoming_transfers
        ADD commission         DECIMAL(18,3) NULL,
            sent_at            DATETIME2     NULL,   -- InternalEx.InsertDate
            sender_branch_name NVARCHAR(200) NULL;   -- الفرع المُرسِل (BName)
END;
GO
