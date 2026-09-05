<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\DB;

/**
 * تقارير نقاط البيع والموظفين — للحساب الرئيسي.
 *
 * ⚠ **كلّها قراءة.** لا `insert` ولا `update` ولا `delete` في هذا الملف،
 * ولا حرفَ واحد يمسّ `wallet` أو `ExchangeAccData` أو `InternalEx`. الأرقام
 * تُجمع من جداولنا وحدها: النسبة (`transfer_attributions`) والخزينة
 * (`employee_cashbox_entries`) والورديات.
 *
 * والأرقام هنا **تشغيلية لا محاسبية**: «كم حوالة سلّم هذا الموظف» و«كم نقداً
 * يُتوقّع في يده»، لا رصيد الوكيل لدى الرحالة. الخلط بين السؤالين هو ما
 * يجعل تقريراً يبدو خاطئاً وهو صحيح.
 */
class EmployeeReportsController extends BaseController
{
    private function admin()
    {
        $user = Auth::user();
        if (!$user) {
            return [null, $this->sendError('غير مصرّح.', [], 401)];
        }
        if (($user->AccountType ?? '') !== 'Main') {
            return [null, $this->sendError('التقارير متاحة للحساب الرئيسي فقط.', [], 403)];
        }
        return [$user, null];
    }

    /**
     * الفترة الزمنية.
     *
     * تُحسب في الخادم لا في التطبيق: هاتفٌ ساعته خاطئة كان سيطلب «اليوم»
     * فيحصل على يوم آخر، ثم يبدو التقرير ناقصاً بلا سبب ظاهر.
     */
    private function range(Request $r): array
    {
        $period = $r->query('period', 'today');

        return match ($period) {
            'yesterday' => [now()->subDay()->startOfDay(), now()->subDay()->endOfDay()],
            'week'      => [now()->startOfWeek(), now()->endOfDay()],
            'month'     => [now()->startOfMonth(), now()->endOfDay()],
            'custom'    => [
                $r->query('from') ? \Carbon\Carbon::parse($r->query('from'))->startOfDay() : now()->startOfDay(),
                $r->query('to')   ? \Carbon\Carbon::parse($r->query('to'))->endOfDay()     : now()->endOfDay(),
            ],
            default     => [now()->startOfDay(), now()->endOfDay()],
        };
    }

    /**
     * GET employees/reports/points-of-sale
     *
     * صفٌّ لكل موظف: ما سلّمه، وما استلمه نقداً، وما يُتوقّع في يده.
     *
     * استعلامات مجمّعة لا استعلامٌ لكل موظف: 20 موظفاً بـ 3 استعلامات لا 60.
     */
    public function pointsOfSale(Request $request)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        [$from, $to] = $this->range($request);
        $employeeId  = $request->query('employee_id');
        $posId       = $request->query('point_of_sale_id');

        $employees = DB::table('employees')
            ->where('agent_id', $user->id)
            ->whereNull('deleted_at')
            ->when($employeeId, fn($q) => $q->where('id', $employeeId))
            ->get(['id', 'full_name', 'phone', 'status', 'last_activity_at']);

        $ids = $employees->pluck('id');
        if ($ids->isEmpty()) {
            return $this->sendResponse(['rows' => [], 'totals' => $this->emptyTotals()], 'Success');
        }

        // ما سلّمه كل موظف في الفترة.
        $delivered = DB::table('transfer_attributions')
            ->where('agent_id', $user->id)
            ->where('action', 'DELIVERED')
            ->whereIn('employee_id', $ids)
            ->whereBetween('occurred_at', [$from, $to])
            ->when($posId, fn($q) => $q->where('point_of_sale_id', $posId))
            ->groupBy('employee_id')
            ->selectRaw('employee_id, COUNT(*) AS cnt, ISNULL(SUM(amount),0) AS total')
            ->get()->keyBy('employee_id');

        // ما أنشأه — يبقى صفراً حتى تُربط صلاحية إنشاء الحوالة، والعمود
        // موجود من الآن كي لا يتغيّر شكل التقرير لاحقاً.
        $created = DB::table('transfer_attributions')
            ->where('agent_id', $user->id)
            ->where('action', 'CREATED')
            ->whereIn('employee_id', $ids)
            ->whereBetween('occurred_at', [$from, $to])
            ->groupBy('employee_id')
            ->selectRaw('employee_id, COUNT(*) AS cnt, ISNULL(SUM(amount),0) AS total')
            ->get()->keyBy('employee_id');

        // حركات الخزينة — المعكوسة وعكسها مستبعدان معاً.
        $cash = DB::table('employee_cashbox_entries')
            ->where('agent_id', $user->id)
            ->whereIn('employee_id', $ids)
            ->where('is_reversed', 0)
            ->whereNull('reversal_of')
            ->whereBetween('created_at', [$from, $to])
            ->groupBy('employee_id')
            ->selectRaw("
                employee_id,
                ISNULL(SUM(CASE WHEN direction='IN'  THEN amount ELSE 0 END),0) AS cash_in,
                ISNULL(SUM(CASE WHEN direction='OUT' THEN amount ELSE 0 END),0) AS cash_out
            ")->get()->keyBy('employee_id');

        // الوردية المفتوحة الآن — منها الافتتاحي للنقد المتوقّع.
        $openShifts = DB::table('employee_shifts')
            ->where('agent_id', $user->id)
            ->whereIn('employee_id', $ids)
            ->where('status', 'OPEN')
            ->get(['employee_id', 'opening_cash', 'cashbox_id', 'id'])
            ->keyBy('employee_id');

        $rows = [];
        $totals = $this->emptyTotals();

        foreach ($employees as $e) {
            $d = $delivered->get($e->id);
            $c = $created->get($e->id);
            $m = $cash->get($e->id);
            $s = $openShifts->get($e->id);

            $cashIn  = (float) ($m->cash_in ?? 0);
            $cashOut = (float) ($m->cash_out ?? 0);
            $opening = (float) ($s->opening_cash ?? 0);

            $row = [
                'employee_id'      => (int) $e->id,
                'full_name'        => $e->full_name,
                'phone'            => $e->phone,
                'status'           => $e->status,
                'last_activity_at' => $e->last_activity_at,
                'delivered_count'  => (int) ($d->cnt ?? 0),
                'delivered_total'  => (float) ($d->total ?? 0),
                'created_count'    => (int) ($c->cnt ?? 0),
                'created_total'    => (float) ($c->total ?? 0),
                'cash_in'          => $cashIn,
                'cash_out'         => $cashOut,
                // المتوقّع لا يُحسب إلا لوردية مفتوحة: بلا وردية لا افتتاحيّ،
                // ورقمٌ بلا افتتاحيّ يضلّل.
                'has_open_shift'   => $s !== null,
                'expected_cash'    => $s === null ? null : $opening + $cashIn - $cashOut,
            ];

            $totals['delivered_count'] += $row['delivered_count'];
            $totals['delivered_total'] += $row['delivered_total'];
            $totals['created_count']   += $row['created_count'];
            $totals['created_total']   += $row['created_total'];
            $totals['cash_in']         += $cashIn;
            $totals['cash_out']        += $cashOut;

            $rows[] = $row;
        }

        return $this->sendResponse([
            'from'   => $from->toDateTimeString(),
            'to'     => $to->toDateTimeString(),
            'rows'   => $rows,
            'totals' => $totals,
        ], 'Success');
    }

    private function emptyTotals(): array
    {
        return [
            'delivered_count' => 0, 'delivered_total' => 0.0,
            'created_count'   => 0, 'created_total'   => 0.0,
            'cash_in'         => 0.0, 'cash_out'      => 0.0,
        ];
    }

    /**
     * GET employees/{id}/statement — كشف الموظف
     */
    public function employeeStatement(Request $request, int $id)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $employee = DB::table('employees')
            ->where('id', $id)->where('agent_id', $user->id)
            ->whereNull('deleted_at')->first();
        if (!$employee) return $this->sendError('الموظف غير موجود.', [], 404);

        [$from, $to] = $this->range($request);

        $attributions = DB::table('transfer_attributions')
            ->where('employee_id', $id)
            ->whereBetween('occurred_at', [$from, $to])
            ->orderByDesc('occurred_at')->limit(200)
            ->get(['action', 'transfer_number', 'amount', 'point_of_sale_id', 'occurred_at']);

        $entries = DB::table('employee_cashbox_entries')
            ->where('employee_id', $id)
            ->whereBetween('created_at', [$from, $to])
            ->orderByDesc('id')->limit(200)
            ->get(['id', 'transaction_type', 'reference_id', 'amount', 'direction',
                   'is_reversed', 'reversal_of', 'notes', 'created_at']);

        $shifts = DB::table('employee_shifts as s')
            ->leftJoin('employee_shift_closings as c', 'c.shift_id', '=', 's.id')
            ->where('s.employee_id', $id)
            ->whereBetween('s.started_at', [$from, $to])
            ->orderByDesc('s.id')->limit(50)
            ->get([
                's.id', 's.opening_cash', 's.status', 's.started_at', 's.ended_at',
                'c.expected_cash', 'c.actual_cash', 'c.difference', 'c.result',
            ]);

        $open = DB::table('employee_shifts')
            ->where('employee_id', $id)->where('status', 'OPEN')->first();

        $expected = null;
        if ($open) {
            $sums = DB::table('employee_cashbox_entries')
                ->where('shift_id', $open->id)
                ->where('is_reversed', 0)->whereNull('reversal_of')
                ->selectRaw("
                    ISNULL(SUM(CASE WHEN direction='IN'  THEN amount ELSE 0 END),0) AS cash_in,
                    ISNULL(SUM(CASE WHEN direction='OUT' THEN amount ELSE 0 END),0) AS cash_out
                ")->first();
            $expected = (float) $open->opening_cash
                + (float) ($sums->cash_in ?? 0) - (float) ($sums->cash_out ?? 0);
        }

        return $this->sendResponse([
            'employee'      => $employee,
            'from'          => $from->toDateTimeString(),
            'to'            => $to->toDateTimeString(),
            'attributions'  => $attributions,
            'cashbox'       => $entries,
            'shifts'        => $shifts,
            'expected_cash' => $expected,
        ], 'Success');
    }

    /**
     * GET employees/dashboard — لوحة متابعة الوكيل
     *
     * أرقامٌ تشغيلية فقط. رصيد الوكيل ليس هنا: مصدره المنظومة وله شاشته.
     */
    public function dashboard(Request $request)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $todayFrom = now()->startOfDay();
        $todayTo   = now()->endOfDay();

        $employees = DB::table('employees')->where('agent_id', $user->id)
            ->whereNull('deleted_at');

        $activePos = DB::table('employee_point_of_sales as ep')
            ->join('employees as e', 'e.id', '=', 'ep.employee_id')
            ->where('e.agent_id', $user->id)
            ->whereNull('e.deleted_at')
            ->where('ep.is_active', 1)
            ->distinct()->count('ep.point_of_sale_id');

        $deliveredToday = DB::table('transfer_attributions')
            ->where('agent_id', $user->id)->where('action', 'DELIVERED')
            ->whereBetween('occurred_at', [$todayFrom, $todayTo])
            ->selectRaw('COUNT(*) AS cnt, ISNULL(SUM(amount),0) AS total')->first();

        $pending = DB::table('agent_incoming_transfers')
            ->where('agent_id', $user->id)
            // ما محي أصله من المنظومة لا يُعدّ هنا كما لا يُعدّ في تبويبات
            // الوكيل — انظر `AgentIncomingTransfersService::reconcileMissing`.
            ->whereNull('core_missing_at')
            ->where('status', 'PENDING_DELIVERY')
            ->where(function ($q) {
                $q->whereNull('core_confirm_type')
                  ->orWhereNotIn('core_confirm_type', [3, 4, 5, 6]);
            })->count();

        // النقد المتوقّع لدى الموظفين — مجموع الورديات المفتوحة وحدها.
        $openShifts = DB::table('employee_shifts')
            ->where('agent_id', $user->id)->where('status', 'OPEN')
            ->get(['id', 'opening_cash']);

        $expectedTotal = 0.0;
        foreach ($openShifts as $s) {
            $sums = DB::table('employee_cashbox_entries')
                ->where('shift_id', $s->id)
                ->where('is_reversed', 0)->whereNull('reversal_of')
                ->selectRaw("
                    ISNULL(SUM(CASE WHEN direction='IN'  THEN amount ELSE 0 END),0) AS cash_in,
                    ISNULL(SUM(CASE WHEN direction='OUT' THEN amount ELSE 0 END),0) AS cash_out
                ")->first();
            $expectedTotal += (float) $s->opening_cash
                + (float) ($sums->cash_in ?? 0) - (float) ($sums->cash_out ?? 0);
        }

        // فروق الإقفال غير المطابقة اليوم — ما يستحقّ نظرة الوكيل.
        $differences = DB::table('employee_shift_closings')
            ->where('agent_id', $user->id)
            ->where('result', '<>', 'MATCH')
            ->whereBetween('closed_at', [$todayFrom, $todayTo])
            ->selectRaw('COUNT(*) AS cnt, ISNULL(SUM(difference),0) AS total')->first();

        $recent = DB::table('audit_logs')
            ->where('agent_id', $user->id)
            ->orderByDesc('id')->limit(15)
            ->get(['action', 'employee_id', 'entity_type', 'entity_id', 'created_at']);

        return $this->sendResponse([
            'active_points_of_sale' => $activePos,
            'active_employees'      => (clone $employees)->where('status', 'ACTIVE')->count(),
            'total_employees'       => (clone $employees)->count(),
            'open_shifts'           => $openShifts->count(),
            'pending_transfers'     => $pending,
            'delivered_today_count' => (int) ($deliveredToday->cnt ?? 0),
            'delivered_today_total' => (float) ($deliveredToday->total ?? 0),
            'expected_cash_total'   => round($expectedTotal, 3),
            'differences_today'     => (int) ($differences->cnt ?? 0),
            'differences_total'     => (float) ($differences->total ?? 0),
            'recent_activity'       => $recent,
        ], 'Success');
    }
}
