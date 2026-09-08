<?php

namespace App\Services\Employees;

use Illuminate\Support\Facades\DB;

/**
 * تقاريرُ الموظف — **قراءةٌ خالصة**.
 *
 * ⚠ لا سطرَ كتابةٍ واحد في هذا الصنف كلِّه: لا `insert` ولا `update` ولا
 * `delete`. تقاريرُ تعرض ما وقع، ولا تُغيّر منه شيئاً.
 *
 * ⚠ **ولا تُحسب أرقامٌ ماليّة جديدة هنا.** ما يُعرض مأخوذٌ كما هو من
 * الجداول التشغيلية (`transfer_attributions`, `agent_incoming_transfers`,
 * `employee_cashbox_entries`) أو من مسار الوكيل نفسِه. وحسابُ رصيدٍ أو
 * عمولةٍ بطريقةٍ ثانية هنا يعني رقمين لسؤالٍ واحد يفترقان عند أوّل تعديل.
 *
 * ── ولماذا كلُّ تقريرٍ خلف صلاحيته ────────────────────────────────────────
 *
 * الوكيلُ يمنح ما يشاء: موظفٌ يرى حوالاتِ يومه ولا يرى رصيدَ وكيله، وآخر
 * يرى نقطةَ بيعه كلَّها. فالتقاريرُ ليست كتلةً واحدة تُمنح أو تُمنع.
 */
class EmployeeReports
{
    /** نطاقُ الموظف: هو وحدَه، أو نقطةُ بيعه كلُّها. */
    private function scope(object $employee, bool $wholePos, ?int $posId)
    {
        $q = DB::table('transfer_attributions')
            ->where('agent_id', $employee->agent_id);

        if ($wholePos && $posId) {
            $q->where('point_of_sale_id', $posId);
        } else {
            $q->where('employee_id', $employee->id);
        }

        return $q;
    }

    /**
     * حوالاتُ اليوم — ما أنشأه الموظف اليوم.
     *
     * ⚠ «اليوم» من منتصف ليل الجهاز لا آخر 24 ساعة: الموظف يقارنه بورديته،
     * ونافذةٌ متحرّكة تعطيه رقماً يتغيّر كلّما نظر إليه.
     */
    public function daily(object $employee, ?string $date = null): array
    {
        $day = $date ? \Carbon\Carbon::parse($date) : now();
        $from = $day->copy()->startOfDay();
        $to   = $day->copy()->endOfDay();

        $rows = $this->scope($employee, false, null)
            ->where('action', 'CREATED')
            ->whereBetween('occurred_at', [$from, $to])
            ->orderByDesc('occurred_at')
            ->limit(200)
            ->get(['transfer_number', 'amount', 'recipient_phone', 'occurred_at']);

        return [
            'date'  => $from->toDateString(),
            'count' => $rows->count(),
            'total' => round((float) $rows->sum('amount'), 3),
            'items' => $rows->map(fn ($r) => [
                'transfer_number' => $r->transfer_number,
                'amount'          => (float) $r->amount,
                'recipient_phone' => $r->recipient_phone,
                'at'              => $r->occurred_at,
            ])->all(),
        ];
    }

    /** الحوالاتُ التي سلّمها الموظف. */
    public function delivered(object $employee, int $days = 30): array
    {
        $since = now()->subDays(max(1, $days));

        $rows = $this->scope($employee, false, null)
            ->where('action', 'DELIVERED')
            ->where('occurred_at', '>=', $since)
            ->orderByDesc('occurred_at')
            ->limit(200)
            ->get(['transfer_number', 'amount', 'occurred_at']);

        return [
            'days'  => $days,
            'count' => $rows->count(),
            'total' => round((float) $rows->sum('amount'), 3),
            'items' => $rows->map(fn ($r) => [
                'transfer_number' => $r->transfer_number,
                'amount'          => (float) $r->amount,
                'at'              => $r->occurred_at,
            ])->all(),
        ];
    }

    /**
     * الحوالاتُ الواردة التي تنتظر التسليم.
     *
     * ⚠ من دفتر الوكيل، ومقيَّدةٌ بـ`core_missing_at IS NULL`: صفٌّ فُقد
     * أصلُه من الدفتر المالي لا يُعرض، وإلّا انتظر الموظف تسليمَ حوالةٍ لا
     * وجود لها.
     */
    public function pending(object $employee): array
    {
        $rows = DB::table('agent_incoming_transfers')
            ->where('agent_id', $employee->agent_id)
            ->where('status', 'PENDING_DELIVERY')
            ->whereNull('core_missing_at')
            ->orderByDesc('id')
            ->limit(200)
            ->get(['transfer_number', 'amount', 'beneficiary_name', 'beneficiary_phone', 'created_at']);

        return [
            'count' => $rows->count(),
            'total' => round((float) $rows->sum('amount'), 3),
            'items' => $rows->map(fn ($r) => [
                'transfer_number' => $r->transfer_number,
                'amount'          => (float) $r->amount,
                'receiver_name'   => $r->beneficiary_name,
                'receiver_phone'  => $r->beneficiary_phone,
                'at'              => $r->created_at,
            ])->all(),
        ];
    }

    /**
     * خزينةُ الموظف — الحركاتُ والمحصّلة.
     *
     * ⚠ والرصيدُ **يُحسب من الحركات ولا يُقرأ مخزَّناً** — وهو قرارُ الخزينة
     * منذ نشأتها: رصيدٌ محفوظ ينحرف عن حركاته عند أوّل انقطاع، ثمّ لا يُعرف
     * أيُّهما الصحيح.
     */
    public function cashbox(object $employee, int $days = 7): array
    {
        $since = now()->subDays(max(1, $days));

        $rows = DB::table('employee_cashbox_entries')
            ->where('employee_id', $employee->id)
            ->where('created_at', '>=', $since)
            ->orderByDesc('id')
            ->limit(300)
            ->get(['direction', 'amount', 'transaction_type', 'notes', 'created_at']);

        $in  = (float) $rows->where('direction', 'IN')->sum('amount');
        $out = (float) $rows->where('direction', 'OUT')->sum('amount');

        return [
            'days'  => $days,
            'in'    => round($in, 3),
            'out'   => round($out, 3),
            'net'   => round($in - $out, 3),
            'count' => $rows->count(),
            'items' => $rows->map(fn ($r) => [
                'direction' => $r->direction,
                'amount'    => (float) $r->amount,
                'type'      => $r->transaction_type,
                'note'      => $r->notes,
                'at'        => $r->created_at,
            ])->all(),
        ];
    }

    /**
     * تقريرُ نقطة البيع — كلُّ من عمل عليها لا الموظفُ وحدَه.
     *
     * ⚠ صلاحيةٌ منفصلة عمداً: من يرى نقطةَ بيعه يرى عملَ زملائه عليها،
     * وذلك قرارُ الوكيل لا حقٌّ تلقائيّ لكلّ موظف.
     */
    public function pointOfSale(object $employee, ?int $posId, int $days = 7): array
    {
        if (!$posId) {
            return ['count' => 0, 'total' => 0.0, 'items' => [], 'no_pos' => true];
        }

        $since = now()->subDays(max(1, $days));

        $rows = DB::table('transfer_attributions as t')
            ->leftJoin('employees as e', 'e.id', '=', 't.employee_id')
            ->where('t.agent_id', $employee->agent_id)
            ->where('t.point_of_sale_id', $posId)
            ->where('t.occurred_at', '>=', $since)
            ->orderByDesc('t.occurred_at')
            ->limit(200)
            ->get(['t.transfer_number', 't.amount', 't.action', 't.occurred_at', 'e.full_name']);

        return [
            'days'  => $days,
            'count' => $rows->count(),
            'total' => round((float) $rows->where('action', 'CREATED')->sum('amount'), 3),
            'items' => $rows->map(fn ($r) => [
                'transfer_number' => $r->transfer_number,
                'amount'          => (float) $r->amount,
                'action'          => $r->action,
                'employee_name'   => $r->full_name,
                'at'              => $r->occurred_at,
            ])->all(),
        ];
    }

    /**
     * كشفُ حساب حوالات الموظف — لجرد ما مرّ بيده.
     *
     * ══════════════════════════════════════════════════════════════════════
     *  لماذا كشفٌ واحد لا تقريران
     * ══════════════════════════════════════════════════════════════════════
     *
     * أمرُ المالك (8 سبتمبر 2026): «لا بدّ له من كشف حساب حوالات خاصّ به
     * لكي يستطيع جرد حوالاته».
     *
     * ⚠ والجردُ لا يصحّ على تقريرين منفصلين — ما أنشأ في شاشة وما سلّم في
     * أخرى — لأن النقدَ في يده **واحد**: ما قبضه من مُرسلٍ وما دفعه لمستفيد
     * في درجٍ واحد. فالكشفُ يعرضهما في سطرٍ واحد بترتيبٍ زمنيّ، ويجمع كلَّ
     * جهةٍ على حدة ثمّ الصافي.
     *
     * ⚠ **والصافي هنا حركةُ نقدٍ لا رصيدُ حساب**: كم دخل جيبَه وكم خرج منه.
     * وهو لا يُقرأ من `wallet` ولا يُكتب فيه — عهدةٌ تُجرد، لا حسابٌ يُرحَّل.
     *
     * ⚠ ويُقيَّد بالموظف نفسِه دائماً: موظفٌ لا يجرد على زميله.
     */
    public function statement(object $employee, int $days = 30): array
    {
        $since = now()->subDays(max(1, $days));

        $rows = DB::table('transfer_attributions')
            ->where('agent_id', $employee->agent_id)
            ->where('employee_id', $employee->id)
            ->where('occurred_at', '>=', $since)
            ->orderByDesc('occurred_at')
            ->limit(500)
            ->get(['transfer_number', 'action', 'amount', 'recipient_phone',
                   'occurred_at']);

        $created   = $rows->where('action', 'CREATED');
        $delivered = $rows->where('action', 'DELIVERED');

        $in  = (float) $created->sum('amount');
        $out = (float) $delivered->sum('amount');

        return [
            'days'            => $days,
            'created_count'   => $created->count(),
            'created_total'   => round($in, 3),
            'delivered_count' => $delivered->count(),
            'delivered_total' => round($out, 3),
            // ⚠ الصافي = ما قبضه ناقصَ ما دفعه. موجبٌ يعني نقداً عنده.
            'net'             => round($in - $out, 3),
            'items' => $rows->map(fn ($r) => [
                'transfer_number' => $r->transfer_number,
                'action'          => $r->action,
                'amount'          => (float) $r->amount,
                'recipient_phone' => $r->recipient_phone,
                'at'              => $r->occurred_at,
            ])->all(),
        ];
    }

    /**
     * الملخّصُ الماليّ للموظف — سطرٌ واحد عن يومه.
     *
     * ⚠ **لا رقمَ جديدٌ يُحسب هنا**: هو تجميعُ ما تُرجعه التقاريرُ نفسُها
     * (`daily` و`cashbox` و`pending`). فلو اختلف رقمُ الملخّص عن رقم
     * التقرير لَما عرف الموظف أيَّهما يصدّق — والاثنان من مصدرٍ واحد.
     */
    public function summary(object $employee): array
    {
        $daily   = $this->daily($employee);
        $cashbox = $this->cashbox($employee, 1);
        $pending = $this->pending($employee);

        return [
            'today_count'   => $daily['count'],
            'today_total'   => $daily['total'],
            'cashbox_in'    => $cashbox['in'],
            'cashbox_out'   => $cashbox['out'],
            'cashbox_net'   => $cashbox['net'],
            'pending_count' => $pending['count'],
            'pending_total' => $pending['total'],
        ];
    }

    /**
     * سجلُّ نشاط الموظف — من سجلّ التدقيق القائم.
     *
     * ⚠ ولا يرى إلّا **أحداثَ نفسِه**: سجلُّ التدقيق يحمل أحداثَ الوكيل
     * وزملائه، وفتحُه كلِّه لموظفٍ يكشف ما ليس له.
     */
    public function audit(object $employee, int $days = 14): array
    {
        $since = now()->subDays(max(1, $days));

        $rows = DB::table('audit_logs')
            ->where('employee_id', $employee->id)
            ->where('created_at', '>=', $since)
            ->orderByDesc('id')
            ->limit(200)
            ->get(['action', 'entity_type', 'entity_id', 'created_at']);

        return [
            'days'  => $days,
            'count' => $rows->count(),
            'items' => $rows->map(fn ($r) => [
                'action'      => $r->action,
                'entity_type' => $r->entity_type,
                'entity_id'   => $r->entity_id,
                'at'          => $r->created_at,
            ])->all(),
        ];
    }
}
