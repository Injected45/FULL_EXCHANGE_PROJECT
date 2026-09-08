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
            foreach (DB::table('agent_incoming_transfers')
                        ->where('agent_id', $agentId)
                        ->whereIn('transfer_number', $chunk)
                        ->whereNull('core_missing_at')
                        ->get() as $t) {
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
                'action_label'    => $r->action === 'DELIVER' ? 'سلَّمتُها' : 'أنشأتُها',
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
