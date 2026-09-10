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
     * بحثٌ برقم الحوالة — البند: `SEARCH_TRANSFER`.
     *
     * ⚠ يبحث في دفتر **وكيله هو** لا في المنظومة كلِّها: موظّفٌ يبحث برقمٍ
     * فيرى حوالةَ وكيلٍ آخر خرقٌ لعزل الوكلاء. والخدمةُ الأمّ تأخذ
     * `agent_id` فتحرسه بنفسها.
     */
    public function search(int $agentId, string $term): array
    {
        $term = trim($term);

        if (mb_strlen($term) < 3) {
            return ['error' => 'اكتب ثلاثة محارف على الأقل من رقم الحوالة.'];
        }

        $res = $this->core->list($agentId, null, $term, 1, 20);

        return [
            'items' => $res['items'] ?? [],
            'total' => $res['total'] ?? count($res['items'] ?? []),
        ];
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

        $rows = DB::table('transfer_attributions')
            ->where('agent_id', $agentId)
            ->where('employee_id', $employeeId)
            ->where('action', 'CREATED')
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
}
