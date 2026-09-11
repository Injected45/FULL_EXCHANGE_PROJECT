<?php

namespace App\Services;

use Illuminate\Support\Facades\DB;

/**
 * أسعارُ العملات المعتمدة — **قراءةٌ خالصة من مصدر المنظومة نفسِه**.
 *
 * ══════════════════════════════════════════════════════════════════════════
 *  أمرُ المالك (11 سبتمبر 2026)
 * ══════════════════════════════════════════════════════════════════════════
 *
 * «شاشةٌ جديدة داخل قسم الحوالات الخارجية باسم أسعار العملات، مخصَّصةٌ للعرض
 *  فقط … يجب أوّلاً فحصُ قاعدة البيانات وتحديدُ الجدول أو المصدر البرمجيّ
 *  المعتمد الذي يحتوي على أسعار الصرف الرسمية، **وعدم إنشاء جدول أسعارٍ
 *  جديد ما دام المصدر موجوداً بالفعل**».
 *
 * ── المصدرُ، وكيف عُرف ───────────────────────────────────────────────────
 *
 * لم يُبحَث عنه بالاسم بل **تُتُبِّع من حيث تُسعَّر الحوالة فعلاً**: محفّزُ
 * `ExternalEx_insert_Mobile` هو الذي يكتب `TransPrice` على كلّ حوالةٍ خارجية،
 * وهو يقرأ السعرَ من:
 *
 *   `NewCurrencyPriceOwnDetailsTb`  (SalePrice · CurrencyIDFrom · CurrencyIDTo)
 *     ⟵ CPID ⟶ `NewCurrencyPricesOwnTb`  (CountryID · PriceType · AccountType
 *                                          · BranchID · BankID)
 *     ⟵ نطاقُ التطبيق ⟶ `AppBriceTB`  (CountryID · AccountType · BranchID)
 *
 * بشرطِ `CurrencyIDFrom = 1` (الدينار) و`PriceType = 2`، والعملةُ المقصودة
 * هي `CountiresTb.DefualtCurrency` للدولة.
 *
 * ⚠ و`AppBriceTB` ليست جدولاً ثانوياً: هي **التي تقرّر أيَّ قائمةِ أسعارٍ
 * يستعملها التطبيق** من بين قوائم الفروع. وبدونها يُقرأ سعرُ فرعٍ آخر —
 * وقيسَ ذلك: الجنيه السوداني وحدَه له ستُّ قيمٍ مختلفة بحسب الفرع
 * (338.983 · 370.370 · 425.531 · 555.555 · 606.060 · 653.594).
 *
 * ── ولا جدولَ جديد ولا نسخةَ أسعارٍ في التطبيق ──────────────────────────
 *
 * ⚠ **لا جدولَ أُنشئ، ولا عمودَ، ولا صفّ.** هذه قراءةٌ واحدةٌ من الجداول
 * القائمة. وأيُّ تعديلٍ يجريه المكتبُ الخلفيّ على السعر يظهر في التطبيقين
 * عند أوّل فتحٍ للشاشة — لأنّ التطبيق **لا يحتفظ بنسخة**.
 *
 * ⚠ **ولا تُستعمل هذه القراءةُ في حسابٍ ولا قيد.** هي عرضٌ فقط، ولا يناديها
 * مسارٌ يكتب. والتسعيرُ يبقى حيث هو: في المحفّز ونقطةِ التسعير.
 */
class CurrencyRatesService
{
    /**
     * قائمةُ الأسعار المعتمدة للتطبيق — دولةً دولةً، وخدمةً خدمة.
     *
     * ⚠ **ولماذا الخدمةُ سطرٌ مستقلّ وليست الدولةُ سطراً واحداً؟**
     *
     * لأنّ السعرَ يختلف بها فعلاً في القاعدة، وقيسَ: مصر ٥٫٥٥٠ لحوالات
     * البريد وفودافون والتسليم باليد، و٥٫٣٠٠ للحوالة البنكية وإنستا باي.
     * وتونس ٣٫٢٢٥ للتسليم باليد و٢٫٥٠٠ للبريد.
     *
     * فجمعُها في سطرٍ واحدٍ لكلّ دولة يعني رقماً واحداً يُعرض على الوكيل
     * **وهو خطأٌ لأربعة أخماس الخدمات** — ويُسعّر به زبوناً.
     *
     * ⚠ ولا يُحسب هنا شيء: `SalePrice` كما هو في القاعدة، والترتيبُ عرضٌ.
     *
     * @return array<int,array<string,mixed>>
     */
    public function list(): array
    {
        $rows = DB::select("
            SELECT
                c.ID            AS country_id,
                c.CName         AS country,
                cur.CuName      AS currency,
                cur.CurCode     AS code,
                a.SalePrice     AS rate,
                b.BankID        AS service_id,
                st.ServiceName  AS service,
                b.ID            AS price_id
            FROM NewCurrencyPriceOwnDetailsTb AS a
            INNER JOIN NewCurrencyPricesOwnTb AS b
                    ON a.CPID = b.ID
            INNER JOIN CountiresTb AS c
                    ON b.CountryID = c.ID
                   AND a.CurrencyIDTo = c.DefualtCurrency
            /* نطاقُ التطبيق — أيُّ قائمةِ أسعارٍ يقرؤها. انظر ترويسة الصنف. */
            INNER JOIN AppBriceTB AS d
                    ON b.CountryID   = d.CountryID
                   AND b.AccountType = d.AccountType
                   AND b.BranchID    = d.BranchID
            INNER JOIN CurrencyMainTb AS cur
                    ON cur.ID = a.CurrencyIDTo
            LEFT  JOIN ExtTraServiceTypeTb AS st
                    ON st.ID = b.BankID
            WHERE a.CurrencyIDFrom = 1
              AND b.PriceType      = 2
            ORDER BY c.CName, st.ServiceName
        ");

        return array_map(fn ($r) => [
            'country_id' => (int) $r->country_id,
            'country'    => trim((string) $r->country),
            'currency'   => trim((string) $r->currency),
            'code'       => trim((string) $r->code),
            'rate'       => $r->rate !== null ? (float) $r->rate : null,
            'service'    => $r->service !== null ? trim((string) $r->service) : null,
            'service_id' => (int) $r->service_id,
        ], $rows);
    }
}
