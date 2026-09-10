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

        /*
         * ⚠ الخزينةُ والورديةُ خرجتا من هذا التقرير — أمرُ المالك
         * (10 سبتمبر 2026): «ألغِها من كلّ التبعات بالكامل … ليصبح التطبيق
         * بالكامل حوالةً استلمها أو حوالةً سلّمها فقط».
         *
         * فسقطت من كلّ صفّ: `cash_in` و`cash_out` و`has_open_shift` و
         * `expected_cash`، ومن المجاميع معها. وبقي ما يصف عملَه بالحوالات:
         * كم سلّم وبكم، وكم أنشأ وبكم.
         *
         * ⚠ ولم تُحذف الجداول: `employee_cashbox_entries` و`employee_shifts`
         * ببياناتها كما هي — حركاتُ مالٍ وقعت فعلاً، ولا تُمحى. أُلغي
         * استعمالُها لا سجلُّها.
         */

        $rows = [];
        $totals = $this->emptyTotals();

        foreach ($employees as $e) {
            $d = $delivered->get($e->id);
            $c = $created->get($e->id);

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
            ];

            $totals['delivered_count'] += $row['delivered_count'];
            $totals['delivered_total'] += $row['delivered_total'];
            $totals['created_count']   += $row['created_count'];
            $totals['created_total']   += $row['created_total'];

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

        ];
    }

    /**
     * GET employees/{id}/statement — كشف الموظف
     */
    /**
     * GET employees/reports/created-transfers — من أنشأ كم، ومن أي نقطة.
     *
     * ── ما يجيب عنه ──────────────────────────────────────────────────
     *
     * نصُّ البند: «عرض تقرير يوضح الحوالات التي أنشأها كل موظف، مع
     * الاعتماد على الحوالات الأصلية نفسها وعدم نسخ البيانات المالية في
     * نظام تقارير موازٍ».
     *
     * ⚠ ولذلك **لا يُقرأ هنا دفترٌ ماليّ ولا تُجمع منه قيمة**: المصدر
     * `transfer_attributions` وحدها — وهي وصفُ من نفّذ، لا سجلُّ ما جرى
     * مالياً. والمبلغُ فيها نسخةٌ تشغيلية للعرض، والحقيقةُ الماليّة تبقى
     * في الدفتر حيث كانت.
     *
     * ⚠ واستعلامان لا استعلامٌ لكل موظّف: التجميعُ في القاعدة، والأسماءُ
     * دفعةً واحدة.
     */
    public function createdTransfers(Request $request)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        [$from, $to] = $this->range($request);

        $rows = DB::table('transfer_attributions')
            ->where('agent_id', $user->id)
            ->where('action', 'CREATED')
            ->whereNotNull('employee_id')
            ->whereBetween('occurred_at', [$from, $to])
            ->groupBy('employee_id', 'point_of_sale_id')
            ->selectRaw('employee_id, point_of_sale_id,
                         COUNT(*) AS transfers,
                         ISNULL(SUM(amount), 0) AS total')
            ->get();

        if ($rows->isEmpty()) {
            return $this->sendResponse([
                'items' => [], 'transfers' => 0, 'total' => 0,
                'from' => (string) $from, 'to' => (string) $to,
            ], 'Success');
        }

        $names = DB::table('employees')
            ->whereIn('id', $rows->pluck('employee_id')->unique()->all())
            ->pluck('full_name', 'id')->all();

        $posIds = $rows->pluck('point_of_sale_id')->filter()->unique()->values()->all();
        $posNames = $posIds === [] ? [] : DB::table('AuthorizedUsers')
            ->whereIn('ID', $posIds)->pluck('Name_post', 'ID')->all();

        $items = $rows->map(fn ($r) => [
            'employee_id'   => (int) $r->employee_id,
            'employee_name' => $names[$r->employee_id] ?? ('موظّف #' . $r->employee_id),
            'pos_id'        => $r->point_of_sale_id ? (int) $r->point_of_sale_id : null,
            'pos_name'      => $r->point_of_sale_id
                ? ($posNames[$r->point_of_sale_id] ?? null) : null,
            'transfers'     => (int) $r->transfers,
            'total'         => (float) $r->total,
        ])->sortByDesc('transfers')->values()->all();

        return $this->sendResponse([
            'items'     => $items,
            'transfers' => array_sum(array_column($items, 'transfers')),
            'total'     => array_sum(array_column($items, 'total')),
            'from'      => (string) $from,
            'to'        => (string) $to,
        ], 'Success');
    }

    /**
     * GET employees/{id}/transfers — حوالاتُ موظّفٍ بعينه، مفصَّلة.
     *
     * ⚠ تُعيد استعمال `EmployeeTransferViews` نفسِها التي يقرأ بها الموظف
     * حوالاتِه — لا نسخةً ثانية من المنطق. السؤالُ واحد («ما الذي نُسب إلى
     * هذا الموظف؟») والسائلُ مختلف، فالحارسُ وحده يختلف.
     */
    public function employeeTransfers(Request $request, int $id)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $employee = DB::table('employees')
            ->where('id', $id)->where('agent_id', $user->id)
            ->whereNull('deleted_at')->first(['id', 'full_name']);

        if (!$employee) return $this->sendError('الموظف غير موجود.', [], 404);

        $out = app(\App\Services\Employees\EmployeeTransferViews::class)->mine(
            (int) $user->id, $id,
            (int) $request->query('page', 1),
            (int) $request->query('per_page', 20),
        );

        return $this->sendResponse(
            $out + ['employee_name' => $employee->full_name], 'Success');
    }
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

        /* ⚠ الخزينةُ والورديةُ خرجتا من كشف الموظف — أمرُ المالك
         * (10 سبتمبر 2026). فما بقي هو النسبُ وحدَها: ما أنشأ وما سلّم،
         * وهو تعريفُه للتطبيق كلِّه — «حوالةٌ استلمها أو حوالةٌ سلّمها».
         *
         * ولم تُحذف الجداول: بياناتُها قائمة، وإنّما لا تُقرأ من هنا. */

        return $this->sendResponse([
            'employee'      => $employee,
            'from'          => $from->toDateTimeString(),
            'to'            => $to->toDateTimeString(),
            'attributions'  => $attributions,
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

        // ⚠ البوّابةُ السيادية — والعدُّ يتبع العرض.
        $pending = \App\Services\AgentIncomingTransfersService::onlyApproved(
                DB::table('agent_incoming_transfers')
                    ->where('agent_id', $user->id)
            )
            ->where('status', 'PENDING_DELIVERY')
            ->where(function ($q) {
                $q->whereNull('core_confirm_type')
                  ->orWhereNotIn('core_confirm_type', [3, 4, 5, 6]);
            })->count();

        /* ⚠ الورديةُ والنقدُ المتوقّع وفروقُ الإقفال خرجت من اللوحة — أمرُ
         * المالك (10 سبتمبر 2026). ولوحةُ الوكيل بعدها تصف ما بقي: نقاطُ
         * بيعه وموظفوه وما ينتظر التسليم وما سُلِّم اليوم. */

        $recent = DB::table('audit_logs')
            ->where('agent_id', $user->id)
            ->orderByDesc('id')->limit(15)
            ->get(['action', 'employee_id', 'entity_type', 'entity_id', 'created_at']);

        return $this->sendResponse([
            'active_points_of_sale' => $activePos,
            'active_employees'      => (clone $employees)->where('status', 'ACTIVE')->count(),
            'total_employees'       => (clone $employees)->count(),
            'pending_transfers'     => $pending,
            'delivered_today_count' => (int) ($deliveredToday->cnt ?? 0),
            'delivered_today_total' => (float) ($deliveredToday->total ?? 0),
            'recent_activity'       => $recent,
        ], 'Success');
    }
}
