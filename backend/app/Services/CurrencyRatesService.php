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
        /*
         * ⚠⚠ **يُبدأ من الخدمات المعروضة، لا من الأسعار الموجودة.**
         *
         * لوحةٌ تسرد الأسعارَ فقط تُخفي أخطرَ ما فيها: خدمةً يراها الوكيلُ في
         * شاشة الإنشاء **ولا سعرَ لها**. فيختارها أمام زبونٍ ثمّ تُردّ.
         * والبدءُ من `ExtTraServiceTypeTb` (ما يُعرض فعلاً) ثمّ الوصلُ الخارجيّ
         * بالسعر يجعل النقصَ **سطراً مرئياً** لا صمتاً.
         *
         * ⚠ و`OUTER APPLY` لا `LEFT JOIN`: يُحتاج إلى عدّ الصفوف المطابقة
         * لا إلى أوّلها، لأنّ التعدّدَ نفسَه حالةُ منع (انظر
         * [ExternalPricingGuard::rate]).
         *
         * ⚠ **ولا يُرشَّح بـ`IsActive`** — ولا في السطر الواحد.
         *
         * لا المحفّزُ ولا `SalePrice_mo_Value` يرشّحان به. فترشيحُه هنا يعني
         * شاشةً تُخفي سعراً **يُحتسب فعلاً** على الزبون — وهو أسوأُ عطبٍ يمكن
         * أن تحمله لوحةُ أسعار. يُقرأ العَلَمُ ويُعرض، ولا يُقصى به صفّ.
         */
        $rows = DB::select("
            SELECT
                c.ID            AS country_id,
                c.CName         AS country,
                cur.CuName      AS currency,
                cur.CurCode     AS code,
                s.ID            AS service_id,
                s.ServiceName   AS service,
                p.SalePrice     AS rate,
                p.Matches       AS matches,
                p.CurrencyPower AS currency_power,
                p.IsActive      AS detail_active,
                p.HeaderActive  AS header_active,
                p.InsertDate    AS priced_on
            FROM ExtTraServiceTypeTb AS s
            INNER JOIN CountiresTb AS c
                    ON c.ID = s.CountryID
            LEFT  JOIN CurrencyMainTb AS cur
                    ON cur.ID = c.DefualtCurrency
            OUTER APPLY (
                SELECT
                    MIN(a.SalePrice)     AS SalePrice,
                    COUNT(*)             AS Matches,
                    MIN(CAST(a.CurrencyPower AS INT)) AS CurrencyPower,
                    MIN(CAST(a.IsActive     AS INT)) AS IsActive,
                    MIN(CAST(b.IsActive     AS INT)) AS HeaderActive,
                    MAX(b.InsertDate)    AS InsertDate
                FROM NewCurrencyPriceOwnDetailsTb AS a
                INNER JOIN NewCurrencyPricesOwnTb AS b
                        ON a.CPID = b.ID
                /* نطاقُ التطبيق — أيُّ قائمةِ أسعارٍ يقرؤها. انظر ترويسة الصنف. */
                INNER JOIN AppBriceTB AS d
                        ON b.CountryID   = d.CountryID
                       AND b.AccountType = d.AccountType
                       AND b.BranchID    = d.BranchID
                WHERE a.CurrencyIDFrom = 1
                  AND b.PriceType      = 2
                  AND b.CountryID      = s.CountryID
                  AND b.BankID         = s.ID
                  AND a.CurrencyIDTo   = c.DefualtCurrency
            ) AS p
            ORDER BY c.CName, s.ServiceName
        ");

        return array_map(function ($r) {
            $matches = (int) ($r->matches ?? 0);

            return [
                'country_id' => (int) $r->country_id,
                'country'    => trim((string) $r->country),
                'currency'   => trim((string) ($r->currency ?? '')),
                'code'       => trim((string) ($r->code ?? '')),
                'service'    => $r->service !== null ? trim((string) $r->service) : null,
                'service_id' => (int) $r->service_id,

                /*
                 * ⚠ السعرُ يُعرض حين يكون واحداً لا غير. فمع التعدّد لا يوجد
                 * «السعر» أصلاً: المنفّذُ يختار عشوائياً، وعرضُ أحدِهما وعدٌ
                 * لا نملك الوفاء به.
                 */
                'rate'       => $matches === 1 && $r->rate !== null ? (float) $r->rate : null,

                /*
                 * حالةُ الاقتران — ما تعرضه الشاشةُ حين لا يوجد سعر:
                 *   PRICED    سعرٌ واحدٌ معتمد
                 *   NO_PRICE  لم يُسعَّر بعد
                 *   AMBIGUOUS أكثرُ من سعر — يُمنع التحويلُ به
                 *
                 * ⚠ وهي **نفسُ أسباب** [ExternalPricingGuard]: ما يمنعه الحارسُ
                 * هو ما تقوله الشاشة، بالحرف نفسِه. وشاشةٌ تقول «متاح» لما
                 * يرفضه الحارسُ تجعل الوكيل يظنّ التطبيق معطوباً.
                 */
                'status'     => $matches === 0 ? 'NO_PRICE'
                              : ($matches > 1 ? 'AMBIGUOUS' : 'PRICED'),

                /*
                 * عَلَمُ المنظومة: 1 = عملةٌ أقوى من الدينار.
                 *
                 * ⚠ **يُعرض ولا يُحسب به.** لا المحفّزُ ولا `SalePrice_mo_Value`
                 * يقرآنه: كلاهما يضرب ضرباً (`المسلَّم = المبلغ × السعر`) مهما
                 * كانت قيمتُه. فاستعمالُه في اتجاه العرض كان سيُظهر للوكيل
                 * اتجاهاً غيرَ الذي يُنفَّذ — وهو العطبُ الذي أُصلح للتوّ.
                 */
                'currency_power' => $r->currency_power === null ? null : (int) $r->currency_power,

                /*
                 * ⚠ يُعرض ولا يُقصي: انظر ترويسة الاستعلام. صفٌّ غيرُ نشطٍ
                 * ما زال يُحتسب على الزبون لأنّ المنظومة لا ترشّح به.
                 */
                'is_active'  => ($r->detail_active === null || $r->header_active === null)
                                ? null
                                : ((int) $r->detail_active === 1 && (int) $r->header_active === 1),

                /*
                 * تاريخُ آخر إدراجٍ للسعر — مقياسُ طزاجته.
                 *
                 * ⚠ ليس زينة: في القاعدة اليوم سعرُ «حوالات البريد» لمصر
                 * مُدرَجٌ منذ 2024-12-28 بينما «فودافون» و«تسليم باليد»
                 * مُحدَّثان في 2026-06-29. وسعرٌ عمرُه سنةٌ ونصف على لوحةٍ
                 * تُسعَّر منها حوالاتٌ اليوم هو خطرُ خسارةٍ قائم — والحارسُ
                 * يمنع الخسارة، وهذا السطرُ يمنع سببَها.
                 */
                'priced_on'  => $r->priced_on !== null
                                ? substr((string) $r->priced_on, 0, 10)
                                : null,
            ];
        }, $rows);
    }
}
