<?php

namespace App\Services\Employees;

use App\Services\AgentIncomingTransfersService;
use Illuminate\Support\Facades\DB;

/**
 * ما يراه الموظف من الحوالات — الصلاحيات الثلاث التي كانت تُمنح ولا تفعل
 * شيئاً: `SEARCH_TRANSFER` و`VIEW_OWN_TRANSFERS` و`VIEW_POS_TRANSFERS`.
 *
 * ── ثلاثة أسئلة مختلفة، لا ثلاث نسخٍ من سؤالٍ واحد ─────────────────────
 *
 *   • **البحث برقم الحوالة**: «أين هذه الحوالة بعينها؟» — يسأله الموظف
 *     والزبون واقفٌ أمامه. فيرجع صفّاً واحداً أو لا شيء.
 *   • **حوالاتي أنا**: «ما الذي سلّمتُه اليوم؟» — مقياسُ عملِه هو.
 *   • **حوالات نقطة بيعي**: «ما الذي جرى على هذا الشبّاك؟» — يشمل
 *     زملاءه على النقطة نفسِها، وهو سؤالُ مسؤولِ نقطةٍ لا سؤالُ موظّف.
 *
 * ولذلك ثلاثةُ مفاتيح لا مفتاح: من يبحث عن حوالةٍ لزبونٍ ليس بالضرورة من
 * يُطلَّع على ما فعله زملاؤه.
 *
 * ── ولماذا `transfer_attributions` لا `InternalEx` ────────────────────
 *
 * ⚠ النسبةُ إلى الموظف مكتوبةٌ في `transfer_attributions` **بجوار** الدفتر
 * المالي لا داخله (انظر `CLAUDE.md`). فسؤالُ «ما الذي سلّمه هذا الموظف»
 * يُجاب من هناك، ولا يُلمس `InternalEx` بحال.
 *
 * ⚠ و`InternalEx.Code` **بلا فهرس**: أي سؤالٍ لكل صفٍّ على حدة يمسح
 * الجدول كلَّه مرّةً لكل صفّ — وهو ما جعل كشف الحساب يستغرق 68 ثانية. فما
 * يُقرأ من الدفتر هنا يُقرأ **مجموعةً واحدة** بعد تحديد الأكواد.
 *
 * ⚠ ولا شيء هنا يكتب: قراءةٌ خالصة.
 */
class EmployeeTransferViews
{
    public function __construct(private AgentIncomingTransfersService $core)
    {
    }

    /** حدُّ الصفحة — ومئةٌ سقفٌ لأن الهاتف لا يعرض أكثر بلا فائدة. */
    private const MAX_PER_PAGE = 100;

    /**
     * سقفُ «الصادرة» في الطلب الواحد.
     *
     * ⚠ بلا ترقيمِ صفحات عمداً: الشرائحُ والبحثُ تعمل على المجموعة كلِّها في
     * الشاشة — نظيرُ صادر الوكيل حرفياً — وترقيمٌ تحتها يجعل الشريحةَ تعدّ
     * صفحةً وتزعم أنها الكل. وخمسمئةٌ سقفٌ لا هدف: هذه حوالاتُ موظفٍ واحد
     * بيده، لا دفترُ وكالة.
     */
    private const MAX_OUTGOING = 500;

    /**
     * يقصر استعلامَ النسب على الحوالة **المحلّية**.
     *
     * ⚠ أُضيف يوم فُتح للموظف بابُ الحوالة الخارجية (10 سبتمبر 2026)، وهو
     * ليس تزيّناً: أكوادُ الخارجية (`codeForMobile`) **لا وجودَ لها في
     * `InternalEx`**، فبدونه كانت كلُّ حوالةٍ خارجيةٍ تظهر في «صادرتي
     * المحلّية» بلا مستفيدٍ ولا حالة، معلَّمةً `missing_in_core` — أي
     * كحوالةٍ مُسح أصلُها من المنظومة. وهو إنذارٌ كاذبٌ عن عطبٍ لم يقع.
     *
     * ⚠ و`NULL` محلّيةٌ بالضرورة: كلُّ صفٍّ كُتب قبل فتح ذلك الباب لم يكن
     * له طريقٌ آخر. ولم يُكتب على تلك الصفوف شيءٌ رجعياً.
     */
    private static function onlyLocal($query)
    {
        return $query->where(function ($w) {
            $w->where('channel', 'LOCAL')->orWhereNull('channel');
        });
    }

    /**
     * «حوالاتي الخارجية» — ما أنشأه هذا الموظف عبر القناة الخارجية.
     *
     * ⚠ نظيرُ [outgoing] حرفاً بحرف، والفرقُ دفترٌ واحد: `ExternalEx` بدل
     * `InternalEx`. وشكلُ الصفّ المُعاد **هو هو**، حتى تعرضه الشاشةُ نفسُها
     * ببطاقتها نفسِها — لا بطاقةٌ ثانية تفترق عن الأولى عند أوّل تعديل.
     *
     * ⚠ ولا واردةَ لها: الخارجيةُ تخرج ولا تعود، وقد نصّ الأمرُ على ذلك
     * صراحةً — «ولا يُعرض تبويبٌ فارغٌ باسم الواردة».
     */
    public function externalOutgoing(int $agentId, int $employeeId, int $limit = 200): array
    {
        $limit = max(1, min($limit, self::MAX_OUTGOING));

        $rows = DB::table('transfer_attributions')
            ->where('agent_id', $agentId)
            ->where('employee_id', $employeeId)
            ->where('action', 'CREATED')
            ->where('channel', 'EXTERNAL')
            ->orderByDesc('occurred_at')
            ->take($limit)
            ->get(['transfer_number', 'amount', 'occurred_at']);

        if ($rows->isEmpty()) {
            return ['items' => [], 'total' => 0, 'limit' => $limit];
        }

        $codes = $rows->pluck('transfer_number')->filter()->unique()->values()->all();

        /*
         * صفوفُ المنظومة — استعلامٌ واحدٌ لكل ألف، لا واحدٌ لكل صفّ.
         *
         * ⚠ **المفتاحُ `Code` لا `codeForMobile`.** الثاني مفتاحٌ عشوائيّ
         * يولّده المتحكّم ليعثر على صفِّه بعد الإدراج، والأوّلُ رقمُ الحوالة
         * الذي يبنيه المحفّز وتعرفه المنظومةُ كلُّها ويُكتب في
         * `EX24AccSafeActivityTb.ISID`. والنسبةُ تُسجَّل به الآن.
         */
        $core = [];
        foreach (array_chunk($codes, 1000) as $chunk) {
            foreach (DB::table('ExternalEx')
                        ->whereIn('Code', $chunk)
                        ->select('Code', 'RecievedName', 'RPhone1',
                                 'SenderName', 'CurrRecievedVal', 'ExVal',
                                 'CountryIDTo', 'CityIDTo', 'InsertDate')
                        ->get() as $c) {
                $core[$c->Code] = $c;
            }
        }

        /* اسمُ الدولة المقصودة — استعلامٌ واحدٌ للصفحة كلِّها. */
        $countryIds = [];
        foreach ($core as $c) {
            if (!empty($c->CountryIDTo)) {
                $countryIds[(int) $c->CountryIDTo] = true;
            }
        }
        $countries = $countryIds === [] ? [] : DB::table('CountiresTb')
            ->whereIn('ID', array_keys($countryIds))
            ->pluck('CName', 'ID')->all();

        $items = [];
        foreach ($rows as $r) {
            $c = $core[$r->transfer_number] ?? null;

            $items[] = [
                'transfer_number'   => $r->transfer_number,
                'amount'            => $c !== null && $c->CurrRecievedVal !== null
                    ? (float) $c->CurrRecievedVal
                    : ($r->amount !== null ? (float) $r->amount : null),
                'commission'        => $c !== null && $c->ExVal !== null ? (float) $c->ExVal : null,
                'beneficiary'       => $c->RecievedName ?? null,
                'beneficiary_phone' => $c->RPhone1 ?? null,
                'sender'            => $c->SenderName ?? null,
                // موضعُ «الفرع» في نظيرتها المحلّية — والوجهةُ هنا دولة.
                'branch'            => $c !== null && !empty($c->CountryIDTo)
                    ? ($countries[(int) $c->CountryIDTo] ?? null) : null,
                /*
                 * ⚠ بلا `status_label`: `ExternalEx` لا تحمل جدولَ حالاتٍ
                 * نظيرَ `InternalEx_Stautes`. واختلاقُ وصفٍ لها («مرسلة»،
                 * «قيد التنفيذ») ادّعاءٌ عن حالةٍ لا تقولها القاعدة — وهو
                 * أسوأُ من غيابه، لأن الموظف يبني عليه كلامَه للزبون.
                 */
                'status_label'      => null,
                'core_confirm_type' => null,
                'date'              => (string) ($c->InsertDate ?? $r->occurred_at),
                'occurred_at'       => (string) $r->occurred_at,
                'missing_in_core'   => $c === null,
            ];
        }

        return ['items' => $items, 'total' => count($items), 'limit' => $limit];
    }

    /**
     * فاتورةُ حوالةٍ خارجيةٍ بالرقم — للموظف الذي **أنشأها هو**.
     *
     * ⚠ نظيرُ [outgoingByCode] حرفاً بحرف في قاعدته: الملكيةُ تُفحص **قبل**
     * القراءة لا بعدها، ولا تُفرَّق النتيجةُ بين «غير موجودة» و«ليست لك» —
     * `null` في الحالتين، كي لا يُستدلّ بوجودها على شيء.
     *
     * ⚠ والصفُّ يُقرأ بمراجعه محلولةً (دولةٌ ومدينةٌ ونوعُ خدمةٍ وعملة): الشاشةُ
     * تعرض ولا تحسب ولا تسأل عن اسمٍ ثانياً — والفاتورةُ تُطبع وتُسلَّم للزبون،
     * فمرجعٌ لم يُحلّ يخرج على الورق رقماً بلا معنى.
     */
    public function externalByCode(int $agentId, int $employeeId, string $code): ?object
    {
        $code = trim($code);
        if ($code === '') {
            return null;
        }

        $owns = DB::table('transfer_attributions')
            ->where('agent_id', $agentId)
            ->where('employee_id', $employeeId)
            ->where('action', 'CREATED')
            ->where('channel', 'EXTERNAL')
            ->where('transfer_number', $code)
            ->exists();

        if (!$owns) {
            return null;
        }

        return $this->externalRow($code);
    }

    /**
     * صفُّ الحوالة الخارجية بمراجعه محلولة — بلا فحصِ ملكية.
     *
     * ⚠ `private`-كالمعنى: **لا يُنادى إلّا من بابٍ فحص الملكيةَ قبله.** هو
     * مفصولٌ عن [externalByCode] كي يستعمله بابُ الوكيل حين يُفتح — لا كي
     * يُنادى مباشرةً من مسارٍ بلا حارس.
     */
    public function externalRow(string $code): ?object
    {
        $r = DB::table('ExternalEx')->where('Code', $code)->first();

        if (!$r) {
            return null;
        }

        // ⚠ استعلامٌ لكلّ مرجعٍ مرّةً واحدة — لا واحدٌ لكلّ حقلٍ في حلقة.
        $country = $r->CountryIDTo
            ? DB::table('CountiresTb')->where('ID', $r->CountryIDTo)->value('CName')
            : null;

        $city = $r->CityIDTo
            ? DB::table('CitiesTb')->where('ID', $r->CityIDTo)->value('CityName')
            : null;

        $service = $r->ServiceType
            ? DB::table('ExtTraServiceTypeTb')->where('ID', $r->ServiceType)->value('ServiceName')
            : null;

        // ⚠ `CurCode` لا `CurrencyCode`: الأسماءُ قُرئت من المخطّط لا من
        // التخمين — عمودٌ غير موجود يردّ `null` صامتاً، فتخرج الفاتورةُ بلا
        // عملةٍ ولا يظهر في أيّ سجلّ لماذا.
        $currency = $r->DeliveredCurrencyID
            ? DB::table('CurrencyMainTb')->where('ID', $r->DeliveredCurrencyID)->value('CurCode')
            : null;

        $branch = $r->RecievedBranchID
            ? DB::table('CoBranch')->where('ID', $r->RecievedBranchID)->value('BName')
            : null;

        return (object) [
            'code'               => $r->Code,
            'mobile_code'        => $r->codeForMobile,
            'sender_name'        => $r->SenderName,
            'sender_phone'       => $r->Phone1,
            'beneficiary_name'   => $r->RecievedName,
            'beneficiary_phone'  => $r->RPhone1,
            'country'            => $country,
            'city'               => $city,
            'service'            => $service,
            'branch'             => $branch,
            // المبلغُ المقبوض بالدينار، والعمولةُ، وما يُسلَّم بعملة الوجهة.
            'amount'             => $r->CurrRecievedVal !== null ? (float) $r->CurrRecievedVal : null,
            'commission'         => $r->ExVal !== null ? (float) $r->ExVal : null,
            'rate'               => $r->TransPrice !== null ? (float) $r->TransPrice : null,
            'net_total'          => $r->NetTotal !== null ? (float) $r->NetTotal : null,
            'currency_code'      => $currency,
            'notes'              => $r->Notes,
            /*
             * ⚠ الحالةُ تُعاد **كما تقولها القاعدة** بأعلامها الثلاثة، ولا
             * تُترجَم هنا إلى نصّ: `ExternalEx` لا جدولَ حالاتٍ لها نظيرَ
             * `InternalEx_Stautes`، واختلاقُ وصفٍ («قيد التنفيذ») ادّعاءٌ عن
             * حالةٍ لا تقولها القاعدة — والموظف يبني عليه كلامَه للزبون.
             */
            'is_canceled'        => (int) ($r->IsCanceled ?? 0),
            'is_delivered'       => (int) ($r->IsDelivered ?? 0),
            'confirmed_type'     => $r->ConfirmedType !== null ? (int) $r->ConfirmedType : null,
            'at'                 => (string) $r->InsertDate,
        ];
    }

    /**
     * بحثٌ برقم الحوالة — البند: `SEARCH_TRANSFER`.
     *
     * ⚠ يبحث في دفتر **وكيله هو** لا في المنظومة كلِّها: موظّفٌ يبحث برقمٍ
     * فيرى حوالةَ وكيلٍ آخر خرقٌ لعزل الوكلاء. والخدمةُ الأمّ تأخذ
     * `agent_id` فتحرسه بنفسها.
     */
    public function search(int $agentId, string $term, ?int $employeeId = null): array
    {
        $term = trim($term);

        if (mb_strlen($term) < 3) {
            return ['error' => 'اكتب ثلاثة محارف على الأقل من رقم الحوالة.'];
        }

        /*
         * ══════════════════════════════════════════════════════════════════
         *  البحثُ يسأل دفترين، لأنّ الموظف يسأل عن اتجاهين
         * ══════════════════════════════════════════════════════════════════
         *
         * بلاغُ المالك (10 سبتمبر 2026): «الموظف يطلب رقم أيّ حوالةٍ هو قام
         * بتنفيذها فيستدعيها ويعرضها … ولا يعرض».
         *
         * وكان يسأل دفترَ الوارد وحدَه. والحوالةُ التي **ينشئها** الموظف صادرةٌ
         * إلى وكيلٍ آخر، فلا صفَّ لها في ذلك الدفتر أصلاً — فالبحثُ عنها يعود
         * فارغاً دائماً، مهما كان الرقمُ صحيحاً.
         *
         * فصار السؤالُ سؤالين:
         *
         *   ١) **الوارد** — دفترُ الوكيل، على مستوى الوكالة. وهو الصواب هنا:
         *      من يبحث برقمٍ يبحث عن حوالةِ زبونٍ واقفٍ أمامه، وقد يكون
         *      استلمها زميلُه. وهذا سلوكُه القائم ولم يُمَسّ.
         *
         *   ٢) **الصادر** — ما أنشأه **هذا الموظف وحدَه** (`transfer_attributions`
         *      بفعل `CREATED`)، تبعاً لقاعدة المالك: الموظف لا يرى عمل زميله.
         *
         * ⚠ ولكلٍّ منهما فاتورةٌ مختلفة، فيحمل كلُّ صفٍّ `kind` تقرأه الشاشةُ
         * لتعرف أيَّهما تفتح. وبغيره كانت الشاشةُ ستخمّن من شكل الحقول.
         */
        $incoming = $this->core->list($agentId, null, $term, 1, 20);

        $items = [];
        foreach ($incoming['items'] ?? [] as $row) {
            $r = (array) $row;
            $r['kind'] = 'INCOMING';
            $items[] = $r;
        }

        if ($employeeId !== null) {
            foreach ($this->searchOwnOutgoing($agentId, $employeeId, $term) as $row) {
                $items[] = $row;
            }
        }

        return ['items' => $items, 'total' => count($items)];
    }

    /**
     * الصادرُ الذي أنشأه هذا الموظف، مطابقةً جزئيةً على الرقم.
     *
     * ⚠ لا يُسأل `InternalEx` بالرقم مباشرةً: `Code` بلا فهرس، ومطابقةُ
     * `LIKE` عليه تمسح جدولاً بنصف مليون صفّ. فتُرشَّح **أكوادُ هذا الموظف**
     * أوّلاً من `transfer_attributions` — وهي مفهرسةٌ على الموظف — ثمّ تُقرأ
     * صفوفُ المنظومة لما طابق منها، مجموعةً واحدة.
     *
     * والمطابقةُ في PHP على قائمةٍ صغيرة: أكوادُ موظفٍ واحد، لا دفترُ منظومة.
     */
    private function searchOwnOutgoing(int $agentId, int $employeeId, string $term): array
    {
        $codes = self::onlyLocal(
                DB::table('transfer_attributions')
                    ->where('agent_id', $agentId)
                    ->where('employee_id', $employeeId)
                    ->where('action', 'CREATED'))
            ->orderByDesc('occurred_at')
            ->take(self::MAX_OUTGOING)
            ->pluck('transfer_number')
            ->filter()
            ->unique()
            ->filter(fn ($c) => mb_stripos((string) $c, $term) !== false)
            ->take(20)
            ->values()
            ->all();

        if ($codes === []) {
            return [];
        }

        $out = [];
        foreach (DB::table('InternalEx as t')
                    ->leftJoin('InternalEx_Stautes as s', 's.ConfirmType', '=', 't.ConfirmType')
                    ->whereIn('t.Code', $codes)
                    ->select('t.Code', 't.ConfirmType', 's.SName', 't.RecievedName',
                             't.SenderName', 't.RPhone1', 't.OverallVal', 't.ExVal',
                             't.InsertDate')
                    ->get() as $c) {
            $out[$c->Code] = [
                'kind'              => 'OUTGOING',
                'transfer_number'   => $c->Code,
                'beneficiary_name'  => $c->RecievedName,
                'beneficiary_phone' => $c->RPhone1,
                'sender_name'       => $c->SenderName,
                'amount'            => $c->OverallVal !== null ? (float) $c->OverallVal : null,
                'commission'        => $c->ExVal !== null ? (float) $c->ExVal : null,
                'core_status_label' => $c->SName,
                'core_confirm_type' => $c->ConfirmType !== null ? (int) $c->ConfirmType : null,
                'sent_at'           => (string) $c->InsertDate,
            ];
        }

        return array_values($out);
    }

    /**
     * حوالاتُ هذا الموظف — ما أنشأه أو سلّمه هو.
     *
     * ⚠ استعلامان لا أكثر: النسب أوّلاً، ثمّ صفوفُ الحوالات لأكوادها
     * **دفعةً واحدة**. والدمجُ في PHP.
     */
    public function mine(int $agentId, int $employeeId, int $page, int $perPage): array
    {
        return $this->byAttribution(
            $agentId,
            fn ($q) => $q->where('employee_id', $employeeId),
            $page, $perPage,
        );
    }

    /**
     * حوالاتُ نقطة البيع — كلُّ من عمل عليها، لا هذا الموظف وحده.
     *
     * وموظّفٌ بلا نقطة بيع لا يرى شيئاً — ولا يُخطئ: الجوابُ الصحيح على
     * «ما الذي جرى على نقطتي؟» حين لا نقطةَ له هو «لا شيء»، لا رسالةُ خطأ.
     */
    public function pointOfSale(int $agentId, ?int $posId, int $page, int $perPage,
                                ?int $employeeId = null): array
    {
        if (!$posId) {
            return ['items' => [], 'total' => 0, 'page' => 1, 'per_page' => $perPage,
                    'note' => 'لا نقطة بيع مُسنَدة إليك.'];
        }

        /*
         * ⚠ **موظّفٌ لا يرى عملَ زميله** — أمرُ المالك (8 سبتمبر 2026):
         * «الموظفون لا يرون جلسات بعضهم، وكلُّ موظفٍ يرى جلستَه هو،
         * والوكيلُ يرى كلَّ الموظفين».
         *
         * وكان هذا العرضُ يُرجع حوالاتِ النقطة كلَّها بلا تمييز، فيرى
         * الموظفُ ما أنشأه زملاؤه عليها ومبالغَه — وهو كشفٌ لم يأذن به أحد.
         *
         * ويبقى للصلاحيتين معنيان مختلفان: `VIEW_OWN_TRANSFERS` حوالاتُه
         * أينما عمل، و`VIEW_POS_TRANSFERS` حوالاتُه **على هذه النقطة**
         * وحدَها — وهو ما يوافق ورديّته عليها.
         */
        return $this->byAttribution(
            $agentId,
            function ($q) use ($posId, $employeeId) {
                $q->where('point_of_sale_id', $posId);
                if ($employeeId !== null) {
                    $q->where('employee_id', $employeeId);
                }
            },
            $page, $perPage,
        );
    }

    /**
     * ══════════════════════════════════════════════════════════════════════
     *  «الصادرة» بعين الموظف — ما أنشأه هو، بحالته في المنظومة
     * ══════════════════════════════════════════════════════════════════════
     *
     * أمرُ المالك (10 سبتمبر 2026): للموظف تبويبُ حوالاتٍ كتبويب الوكيل،
     * صادرُه فيه بكامل العرض والبحث والترشيح. ونطاقُه «ما أنشأه هو وحدَه»
     * بقراره كذلك — امتدادُ قاعدة 8 سبتمبر: الموظف لا يرى عمل زميله.
     *
     * ── ولماذا لا يُعاد استعمالُ [mine] ────────────────────────────────
     *
     * [mine] تقرأ تفاصيلَ الصفّ من `agent_incoming_transfers` — دفترِ
     * **الوارد**. والحوالةُ التي يُنشئها الموظف صادرةٌ إلى وكيلٍ آخر، فلا صفَّ
     * لها هناك أصلاً: تخرج بلا اسم مستفيدٍ ولا حالة، أي بطاقةٌ فارغة. فحالةُ
     * الصادر تُقرأ من `InternalEx` نفسِه، وهو الموضعُ الوحيد الذي يعرفها.
     *
     * ── ولا شيءَ من مال الوكيل يعبر من هنا ─────────────────────────────
     *
     * ⚠ كشفُ الوكيل (`LocalStatmentAccount`) يحمل **رصيدَه الجاري** في كل
     * صفّ، ورصيدُ الوكالة خلف صلاحيةٍ حسّاسة لا تُمنح بضغطةٍ جماعية. فلم
     * يُبنَ هذا عليه: لا `Balnce` هنا ولا مجموعَ وكالة — أرقامُ الحوالة
     * وحدَها، وهي أرقامُ حوالةٍ كتبها الموظف بيده.
     *
     * ⚠ وقراءةٌ خالصة: لا كتابةَ ولا حسابَ عمولةٍ ولا رصيد. `ExVal` و
     * `OverallVal` تُنقلان كما كتبتهما المنظومة.
     *
     * ── وقفةٌ عند الأداء ───────────────────────────────────────────────
     *
     * ⚠ `InternalEx.Code` **بلا فهرس** (مُثبَت: `PK(ID)` و`UQ(IDCode)` وفهرسا
     * الإلغاء لا غير). فسؤالُه صفّاً صفّاً يمسح الجدول مرّةً لكل حوالة — وهو
     * ما جعل كشفَ الحساب يستغرق 68 ثانية. فالأكوادُ تُجمَع أوّلاً ويُسأل
     * الدفترُ **مرّةً واحدة** لها، مُجزّأةً عند 1000 لأنّ `IN` يقف عند 2100.
     *
     * @param int $limit سقفُ ما يُعاد — أحدثُ ما أنشأه. الترشيحُ والبحثُ في
     *        الشاشة كما في صادر الوكيل تماماً، فتُجلب دفعةً ثمّ تُرشَّح بلا
     *        طلبٍ جديد عند كل ضغطةِ شريحة.
     */
    public function outgoing(int $agentId, int $employeeId, int $limit = 200): array
    {
        $limit = max(1, min($limit, self::MAX_OUTGOING));

        $rows = self::onlyLocal(
                DB::table('transfer_attributions')
                    ->where('agent_id', $agentId)
                    ->where('employee_id', $employeeId)
                    ->where('action', 'CREATED'))
            ->orderByDesc('occurred_at')
            ->take($limit)
            ->get(['transfer_number', 'amount', 'occurred_at', 'point_of_sale_id']);

        if ($rows->isEmpty()) {
            return ['items' => [], 'total' => 0, 'limit' => $limit];
        }

        $codes = $rows->pluck('transfer_number')->filter()->unique()->values()->all();

        /* صفوفُ المنظومة لهذه الأكواد — استعلامٌ واحد لكل ألف. */
        $core = [];
        foreach (array_chunk($codes, 1000) as $chunk) {
            foreach (DB::table('InternalEx as t')
                        ->leftJoin('InternalEx_Stautes as s', 's.ConfirmType', '=', 't.ConfirmType')
                        ->whereIn('t.Code', $chunk)
                        ->select('t.Code', 't.ConfirmType', 's.SName', 't.RecievedName',
                                 't.SenderName', 't.RPhone1', 't.OverallVal', 't.ExVal',
                                 't.InsertDate', 't.BranchRecievedID')
                        ->get() as $c) {
                // أحدثُ صفٍّ يفوز إن تكرّر الكود — نظيرُ قاعدة `attachCoreText`.
                $core[$c->Code] = $c;
            }
        }

        /* أسماءُ الفروع المستقبِلة — استعلامٌ واحد كذلك، لا واحدٌ لكل صفّ. */
        $branchIds = [];
        foreach ($core as $c) {
            if (!empty($c->BranchRecievedID)) {
                $branchIds[(int) $c->BranchRecievedID] = true;
            }
        }
        // ⚠ `CoBranch` لا `BBranchTb`: الثاني فروعُ **بنوك** لا فروعَ صرافة،
        // واسمُه يوحي بغير ذلك. والمرجعُ نفسُه الذي يستعمله `depositController`
        // حين يترجم `BranchRecievedID` إلى اسم (`join CoBranch on ID`).
        $branches = $branchIds === [] ? [] : DB::table('CoBranch')
            ->whereIn('ID', array_keys($branchIds))
            ->pluck('BName', 'ID')->all();

        $items = [];
        foreach ($rows as $r) {
            $c = $core[$r->transfer_number] ?? null;

            $items[] = [
                'transfer_number'   => $r->transfer_number,
                // مبلغُ النسبة احتياطيٌّ فقط: الأصلُ ما كتبته المنظومة.
                'amount'            => $c !== null && $c->OverallVal !== null
                    ? (float) $c->OverallVal
                    : ($r->amount !== null ? (float) $r->amount : null),
                'commission'        => $c !== null && $c->ExVal !== null ? (float) $c->ExVal : null,
                'beneficiary'       => $c->RecievedName ?? null,
                'beneficiary_phone' => $c->RPhone1 ?? null,
                'sender'            => $c->SenderName ?? null,
                'branch'            => $c !== null && !empty($c->BranchRecievedID)
                    ? ($branches[(int) $c->BranchRecievedID] ?? null) : null,
                // ⚠ الوصفُ كما كتبته المنظومة لا ترجمةً له: الفرقُ بين
                // «مرسلة مع مندوب» و«غير مسلمه» يعني أين الحوالةُ الآن.
                'status_label'      => $c->SName ?? null,
                'core_confirm_type' => $c !== null && $c->ConfirmType !== null
                    ? (int) $c->ConfirmType : null,
                // تاريخُ المنظومة هو المرجع؛ ووقتُ النسبة حين يغيب.
                'date'              => (string) ($c->InsertDate ?? $r->occurred_at),
                'occurred_at'       => (string) $r->occurred_at,
                // ⚠ صفٌّ نُسب ثمّ مُسح أصلُه من المنظومة: يُعرَض ولا يُخفى،
                // ويُعلَّم كي لا يُقرأ غيابُ حالته على أنّه حالةٌ مجهولة.
                'missing_in_core'   => $c === null,
            ];
        }

        return ['items' => $items, 'total' => count($items), 'limit' => $limit];
    }

    /**
     * فاتورةُ حوالةٍ صادرة بالرقم — للموظف الذي **أنشأها هو**.
     *
     * ⚠ الملكيةُ هنا أضيقُ من ملكية الوكيل عمداً: الوكيلُ يملكها بأثرٍ في
     * كشفه أو بإنشائها، والموظفُ لا يملكها إلّا بصفٍّ باسمه في
     * `transfer_attributions`. فحوالةُ زميله — وهي حوالةُ وكيله نفسِه —
     * لا تُفتح من هنا، تبعاً لقاعدة «الموظف لا يرى عمل زميله».
     *
     * ⚠ والفحصُ **قبل** القراءة لا بعدها: قراءةٌ ثمّ ترشيح تعني أنّ الصفَّ
     * قد غادر القاعدة إلى الذاكرة قبل أن يُسأل عن حقّه فيه.
     *
     * ولا تُفرّق النتيجةُ بين «غير موجودة» و«ليست لك» — `null` في الحالتين،
     * كما في باب الوكيل، كي لا يُستدلّ بوجودها على شيء.
     */
    public function outgoingByCode(int $agentId, int $employeeId, string $code): ?object
    {
        $code = trim($code);
        if ($code === '') {
            return null;
        }

        $owns = DB::table('transfer_attributions')
            ->where('agent_id', $agentId)
            ->where('employee_id', $employeeId)
            ->where('action', 'CREATED')
            ->where('transfer_number', $code)
            ->exists();

        return $owns ? $this->core->outgoingRowByCode($code) : null;
    }

    /**
     * الأساسُ المشترك: صفوفُ النسب ⇦ أكوادُها ⇦ صفوفُ الدفتر دفعةً واحدة.
     */
    private function byAttribution(int $agentId, callable $filter,
                                   int $page, int $perPage): array
    {
        $perPage = max(1, min($perPage, self::MAX_PER_PAGE));
        $page    = max(1, $page);

        $q = DB::table('transfer_attributions')->where('agent_id', $agentId);
        $filter($q);

        $total = (clone $q)->count();

        $rows = $q->orderByDesc('occurred_at')
            ->skip(($page - 1) * $perPage)->take($perPage)
            ->get(['transfer_number', 'action', 'amount', 'occurred_at',
                   'employee_id', 'point_of_sale_id']);

        if ($rows->isEmpty()) {
            return ['items' => [], 'total' => 0, 'page' => $page, 'per_page' => $perPage];
        }

        $codes = $rows->pluck('transfer_number')->filter()->unique()->values()->all();

        /*
         * صفوفُ دفتر الوكيل لهذه الأكواد — استعلامٌ واحد.
         *
         * ⚠ ومقسَّمٌ عند 1000: `IN` في SQL Server تقف عند 2100 وسيط، وصفحةٌ
         * واحدة لا تبلغها لكنّ الحدَّ يبقى مكتوباً لأن الصفحة قد تكبر.
         */
        $ledger = [];
        foreach (array_chunk($codes, 1000) as $chunk) {
            // ⚠ البوّابةُ السيادية: حالةُ حوالةٍ غيرِ معتمدة لا تُلحق
            // بصفٍّ يراه الموظف — ولو لم تُعرض الحوالةُ نفسُها.
            foreach (\App\Services\AgentIncomingTransfersService::onlyApproved(
                        DB::table('agent_incoming_transfers')
                            ->where('agent_id', $agentId)
                            ->whereIn('transfer_number', $chunk)
                    )->get() as $t) {
                $ledger[$t->transfer_number] = $t;
            }
        }

        /* أسماءُ من نسبت إليهم — استعلامٌ واحد كذلك. */
        $empIds = $rows->pluck('employee_id')->filter()->unique()->values()->all();
        $names = $empIds === [] ? [] : DB::table('employees')
            ->whereIn('id', $empIds)->pluck('full_name', 'id')->all();

        $items = [];
        foreach ($rows as $r) {
            $t = $ledger[$r->transfer_number] ?? null;

            $items[] = [
                'transfer_number' => $r->transfer_number,
                'action'          => $r->action,
                // ⚠ `DELIVERED` لا `DELIVER`: القيمةُ المكتوبة في
                // `EmployeeController::deliver` هي `'DELIVERED'`، فالمقارنةُ
                // القديمة لم تكن تصدُق أبداً — وكلُّ ما سلّمه الموظف كان
                // يُوسَم «أنشأتُها». عيبُ وسمٍ لا عيبُ بيانات، والصفوفُ سليمة.
                'action_label'    => $r->action === 'DELIVERED' ? 'سلَّمتُها' : 'أنشأتُها',
                'amount'          => $r->amount !== null ? (float) $r->amount : null,
                'occurred_at'     => (string) $r->occurred_at,
                'by_name'         => $names[$r->employee_id] ?? null,
                // ⚠ ما يعرفه الدفتر عنها — وقد لا يكون معروفاً: صفٌّ نُسب
                // إلى موظّف ثم مُسح أصلُه من المنظومة يبقى في النسب.
                'beneficiary'     => $t->beneficiary_name ?? null,
                'sender'          => $t->sender_name ?? null,
                'status'          => $t->status ?? null,
                // الوصفُ العربيّ من المنظومة لا رمزُ الحالة وحده.
                'status_label'    => $t->core_status_label ?? null,
                'commission'      => isset($t->commission) ? (float) $t->commission : null,
            ];
        }

        return ['items' => $items, 'total' => $total,
                'page' => $page, 'per_page' => $perPage];
    }

    /**
     * ══════════════════════════════════════════════════════════════════════
     *  خزينةُ الموظف — مالُ الحوالات وحدَه، لا عهدةَ ولا وردية
     * ══════════════════════════════════════════════════════════════════════
     *
     * تصحيحُ المالك (10 سبتمبر 2026): «طلبتُ إيقاف خدمة العهدة … من قبضٍ
     * وصرفٍ والورديةِ وفتحِها وإقفالِها، ولكن أنت أخفيتَ حتى الخزينة وهذا
     * خطأ. أريد إعادة تفعيل خزينة الموظف بحيث يظهر فيها قيمةُ الحوالات
     * الصادرة والواردة والرصيد … ليجرد الدرج ويطابق».
     *
     * فالذي أُلغي **إدخالُ اليد**: قبضٌ وصرفٌ يدويّان، وعهدةٌ افتتاحية،
     * ووردية تُفتح وتُقفل. والذي يعود **مرآةُ الحوالات**: كم دخل الدرجَ وكم
     * خرج منه، والفرقُ بينهما.
     *
     * ── ولا جدولَ لها ولا صفَّ يُكتب ───────────────────────────────────────
     *
     * ⚠ تُحسب كلُّها من `transfer_attributions` — وهي السجلُّ القائم لمن
     * أنشأ ومن سلّم. فلا `employee_cashboxes` ولا `employee_shifts` ولا صفٌّ
     * جديدٌ في أيّ مكان: **قراءةٌ خالصة**، وهي الطريقةُ الوحيدة التي تجعل
     * الخزينةَ لا تفترق عن الحوالات أبداً — لأنها هي الحوالاتُ منظوراً إليها
     * من جهة النقد.
     *
     * ── والاتجاهُ من فعل الموظف لا من اتجاه الحوالة ───────────────────────
     *
     * • **أنشأ حوالة** ⇦ قبض قيمتَها من الزبون ⇦ **داخل** (`in`).
     * • **سلّم حوالة** ⇦ دفع قيمتَها للمستفيد ⇦ **خارج** (`out`).
     *
     * ⚠ والتسميةُ في الشاشة تتبع ما يفهمه هو — «صادرة» لما أنشأه و«واردة»
     * لما سلّمه — لكنّ الحسابَ على النقد: ما دخل الدرجَ وما خرج منه. وخلطُ
     * الاثنين يقلب الرصيدَ إشارةً كاملة.
     *
     * @param int $days نافذةُ الجرد. والدرجُ يُجرد يومياً، فالافتراضيُّ يوم.
     */
    public function cashbox(int $agentId, int $employeeId, int $days = 1): array
    {
        $days  = max(1, min($days, 90));
        $since = now()->subDays($days);

        $rows = DB::table('transfer_attributions')
            ->where('agent_id', $agentId)
            ->where('employee_id', $employeeId)
            ->whereIn('action', ['CREATED', 'DELIVERED'])
            ->where('occurred_at', '>=', $since)
            ->orderByDesc('occurred_at')
            ->take(self::MAX_OUTGOING)
            ->get(['action', 'transfer_number', 'amount', 'occurred_at']);

        /*
         * أسماءُ المستفيدين — استعلامٌ واحدٌ لكلّ الأكواد، لا واحدٌ لكلّ صفّ.
         *
         * ⚠ و`InternalEx.Code` بلا فهرس، فيُجزّأ عند 1000 كما في كلّ قراءةٍ
         * منه هنا. والاسمُ زينةٌ في هذه الشاشة: غيابُه لا يُسقط صفّاً، لأنّ
         * الجردَ على المبلغ لا على الاسم.
         */
        $codes = $rows->pluck('transfer_number')->filter()->unique()->values()->all();
        $names = [];
        foreach (array_chunk($codes, 1000) as $chunk) {
            foreach (DB::table('InternalEx')
                        ->whereIn('Code', $chunk)
                        ->get(['Code', 'RecievedName']) as $c) {
                $names[$c->Code] = $c->RecievedName;
            }
        }

        $in = 0.0;
        $out = 0.0;
        $inCount = 0;
        $outCount = 0;
        $items = [];

        foreach ($rows as $r) {
            $amount   = $r->amount !== null ? (float) $r->amount : 0.0;
            $isCreate = $r->action === 'CREATED';

            if ($isCreate) {
                $in += $amount;
                $inCount++;
            } else {
                $out += $amount;
                $outCount++;
            }

            $items[] = [
                'transfer_number' => $r->transfer_number,
                'action'          => $r->action,
                'label'           => $isCreate ? 'حوالة صادرة — قبضتَ قيمتها'
                                               : 'حوالة واردة — سلّمتَ قيمتها',
                'direction'       => $isCreate ? 'IN' : 'OUT',
                'amount'          => $amount,
                'beneficiary'     => $names[$r->transfer_number] ?? null,
                'at'              => (string) $r->occurred_at,
            ];
        }

        return [
            'days'        => $days,
            'in'          => round($in, 3),
            'in_count'    => $inCount,
            'out'         => round($out, 3),
            'out_count'   => $outCount,
            // ⚠ الرصيدُ فرقٌ لا مجموع، وقد يكون سالباً: موظفٌ سلّم أكثر ممّا
            // قبض دفع من ماله، والوكيلُ مدينٌ له. والإشارةُ تُقال بالنصّ في
            // الشاشة لا برقمٍ عارٍ.
            'balance'     => round($in - $out, 3),
            'items'       => $items,
        ];
    }
}
