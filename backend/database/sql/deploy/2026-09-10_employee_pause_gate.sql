/* ============================================================================
   بوّابةُ إيقاف/تشغيل الموظفين — سيطرةُ الوكيل عن بُعد (10 سبتمبر 2026)
   ============================================================================

   الوكيل يستطيع تجميدَ موظفٍ بعينه (فرديّ) أو الجميع (جماعيّ) بضغطة. الموظف
   المُجمَّد يرى شاشةَ «تواصل مع الإدارة» ويُمنع من كلّ عمل — ولا يفقد شيئاً،
   ويعود فور التشغيل.

   ⚠ معزولٌ عن المال تماماً: بوّابةُ واجهةٍ وعلامةٌ فقط، لا تمسّ رصيداً ولا
   قيداً ولا عمولةً ولا أيّ جدولٍ محاسبيّ. جداولُ تشغيليّة بحتة.

   إضافيٌّ و idempotent: عمودٌ على employees + جدولٌ صغير للحالة الجماعية.
   ============================================================================ */

/* 1) إيقافٌ فرديّ: عمودٌ على employees. NULL = فعّال، وقتٌ = مُوقَف منذُه. */
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.employees') AND name = 'paused_at'
)
    ALTER TABLE dbo.employees ADD paused_at DATETIME2 NULL;
GO

/* 2) إيقافٌ جماعيّ: صفٌّ واحدٌ لكلّ وكيل. all_paused_at NULL = الكلّ فعّال. */
IF OBJECT_ID('dbo.employee_pause_gate', 'U') IS NULL
    CREATE TABLE dbo.employee_pause_gate (
        agent_id       BIGINT       NOT NULL
                       CONSTRAINT PK_emp_pause_gate PRIMARY KEY,
        all_paused_at  DATETIME2    NULL,
        updated_by     BIGINT       NULL,
        updated_at     DATETIME2    NOT NULL
                       CONSTRAINT DF_emp_pause_updated DEFAULT (SYSUTCDATETIME())
    );
GO
