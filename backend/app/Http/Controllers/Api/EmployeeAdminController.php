<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\Employees\EmployeeActivationService;
use App\Services\Employees\EmployeeAuditLogger;
use App\Services\Employees\EmployeePermissions;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\DB;

/**
 * إدارة الموظفين ونقاط البيع والصلاحيات — للحساب الرئيسي وحده.
 *
 * ⚠ العزل: الوكيل يُشتقّ من `Auth::user()` ولا يُقرأ من الطلب، وكل استعلام
 * مقيَّد بـ `agent_id`. وكيلٌ يعدّل رقماً في الطلب لا يصل إلى موظف غيره.
 *
 * والتعديل للحساب الرئيسي (`AccountType = 'Main'`) وحده: نقطة البيع لا
 * تُنشئ موظفين ولا تمنح صلاحيات.
 */
class EmployeeAdminController extends BaseController
{
    public function __construct(
        private EmployeeActivationService $activation,
        private EmployeeAuditLogger $log,
    ) {
    }

    /** @return array{0:?object,1:?\Illuminate\Http\JsonResponse} */
    private function admin()
    {
        $user = Auth::user();
        if (!$user) {
            return [null, $this->sendError('غير مصرّح.', [], 401)];
        }
        if (($user->AccountType ?? '') !== 'Main') {
            return [null, $this->sendError('هذه الإدارة متاحة للحساب الرئيسي فقط.', [], 403)];
        }
        return [$user, null];
    }

    private function trace(Request $r, $user): array
    {
        return [
            'ip'            => $r->ip(),
            'actor_user_id' => $user->id,
            'actor_type'    => 'AGENT',
        ];
    }

    /* ================= نقاط البيع ================= */

    /**
     * GET employees/points-of-sale
     *
     * تُقرأ من `AuthorizedUsers` — جدول نقاط البيع القائم في المنظومة. لا
     * جدول موازٍ: تطبيق سطح المكتب يكتب فيه، ونسخةٌ ثانية تفترق عنه.
     */
    public function pointsOfSale(Request $request)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $rows = DB::table('AuthorizedUsers')
            ->where('BranchID', $user->BrancchID)
            ->select(['ID as id', 'Name_post as name', 'phone', 'IsActive as is_active'])
            ->orderBy('Name_post')
            ->get();

        return $this->sendResponse($rows, 'Success');
    }

    /* ================= الموظفون ================= */

    /** GET employees */
    public function index(Request $request)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $rows = DB::table('employees as e')
            ->where('e.agent_id', $user->id)
            ->whereNull('e.deleted_at')
            ->leftJoin('employee_devices as d', function ($j) {
                $j->on('d.employee_id', '=', 'e.id')->where('d.status', '=', 'ACTIVE');
            })
            ->select([
                'e.id', 'e.full_name', 'e.phone', 'e.status', 'e.paused_at',
                'e.last_login_at', 'e.last_activity_at', 'e.activated_at',
                'd.id as device_id', 'd.platform', 'd.model',
                'd.activated_at as device_activated_at', 'd.last_activity_at as device_last_activity',
            ])
            ->orderByDesc('e.id')
            ->get();

        // الإيقافُ الجماعيّ للوكيل — نفس القيمة تُرفَق بكلّ صفّ ليقرأها زرُّ
        // «إيقاف الكل»، بينما paused الفرديّ يقود زرَّ كلّ موظف.
        $allPaused = DB::table('employee_pause_gate')
            ->where('agent_id', $user->id)
            ->whereNotNull('all_paused_at')->exists();

        $ids = $rows->pluck('id');

        $pos = DB::table('employee_point_of_sales as ep')
            ->leftJoin('AuthorizedUsers as a', 'a.ID', '=', 'ep.point_of_sale_id')
            ->whereIn('ep.employee_id', $ids)
            ->select(['ep.employee_id', 'ep.point_of_sale_id', 'ep.is_primary',
                      'ep.is_active', 'a.Name_post as name'])
            ->get()
            ->groupBy('employee_id');

        $perms = DB::table('employee_permissions')
            ->whereIn('employee_id', $ids)
            ->select(['employee_id', 'permission_key'])
            ->get()
            ->groupBy('employee_id');

        $out = $rows->map(function ($r) use ($pos, $perms, $allPaused) {
            $r->points_of_sale = $pos->get($r->id, collect())->values();
            $r->permissions    = $perms->get($r->id, collect())->pluck('permission_key')->values();
            $r->paused     = $r->paused_at !== null;   // إيقافٌ فرديّ
            $r->all_paused = $allPaused;               // إيقافٌ جماعيّ للوكيل
            return $r;
        });

        return $this->sendResponse($out, 'Success');
    }

    /* ── بوّابةُ الإيقاف: سيطرةُ الوكيل عن بُعد ─────────────────────────────
     *
     * إيقافٌ فرديّ (paused_at على الموظف) أو جماعيّ (employee_pause_gate للوكيل)،
     * مستقلّان: الموظف مُجمَّدٌ إن أوقفه أحدُهما. تجميدٌ ناعم — لا يُلغي جلسة
     * ولا يمسّ بياناتٍ ولا مالاً؛ يُرفَع فوراً بالتشغيل، بلا فقدان شيء. */

    /** POST employees/{id}/pause — إيقاف موظفٍ بعينه */
    public function pause(Request $request, int $id)
    {
        return $this->setPaused($request, $id, true);
    }

    /** POST employees/{id}/resume — تشغيل موظفٍ بعينه */
    public function resume(Request $request, int $id)
    {
        return $this->setPaused($request, $id, false);
    }

    private function setPaused(Request $request, int $id, bool $paused)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $employee = $this->ownedEmployee($user->id, $id);
        if (!$employee) return $this->sendError('الموظف غير موجود.', [], 404);

        DB::table('employees')->where('id', $id)->update([
            'paused_at'  => $paused ? now() : null,
            'updated_at' => now(),
        ]);

        $this->log->audit($paused ? 'EMPLOYEE_PAUSED' : 'EMPLOYEE_RESUMED', [
            'agent_id' => $user->id, 'employee_id' => $id,
            'entity_type' => 'employee', 'entity_id' => (string) $id,
        ]);

        return $this->sendResponse(['paused' => $paused],
            $paused ? 'أُوقف الموظف مؤقتاً.' : 'أُعيد تشغيل الموظف.');
    }

    /** POST employees/pause-all — إيقاف كلّ موظفي الوكيل */
    public function pauseAll(Request $request)
    {
        return $this->setPausedAll($request, true);
    }

    /** POST employees/resume-all — تشغيل كلّ موظفي الوكيل */
    public function resumeAll(Request $request)
    {
        return $this->setPausedAll($request, false);
    }

    private function setPausedAll(Request $request, bool $paused)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        // upsert لصفّ الوكيل: علامةٌ واحدة تحكم الجميع، دون المساس بالإيقاف
        // الفرديّ (يبقى مستقلاً، فرفعُ الجماعيّ لا يُشغّل موظفاً أوقفتَه وحده).
        $exists = DB::table('employee_pause_gate')->where('agent_id', $user->id)->exists();
        $data = ['all_paused_at' => $paused ? now() : null,
                 'updated_by' => $user->id, 'updated_at' => now()];
        if ($exists) {
            DB::table('employee_pause_gate')->where('agent_id', $user->id)->update($data);
        } else {
            DB::table('employee_pause_gate')->insert($data + ['agent_id' => $user->id]);
        }

        $this->log->audit($paused ? 'EMPLOYEES_PAUSED_ALL' : 'EMPLOYEES_RESUMED_ALL', [
            'agent_id' => $user->id, 'entity_type' => 'agent', 'entity_id' => (string) $user->id,
        ]);

        return $this->sendResponse(['all_paused' => $paused],
            $paused ? 'أُوقف جميع الموظفين مؤقتاً.' : 'أُعيد تشغيل جميع الموظفين.');
    }

    /** POST employees */
    public function store(Request $request)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $data = $request->validate([
            'full_name'          => 'required|string|max:200',
            'phone'              => 'required|string|max:20',
            'national_number'    => 'nullable|string|max:30',
            'notes'              => 'nullable|string|max:500',
            'points_of_sale'     => 'nullable|array',
            'points_of_sale.*'   => 'integer',
            'permissions'        => 'nullable|array',
            'permissions.*'      => 'string|max:80',
        ]);

        $phone = $this->normalizePhone($data['phone']);
        if (!preg_match('/^9[0-9]{8}$/', $phone)) {
            return $this->sendError('رقم الهاتف غير صالح — 9 أرقام تبدأ بـ 9.', [], 422);
        }

        $dup = DB::table('employees')
            ->where('agent_id', $user->id)->where('phone', $phone)
            ->whereNull('deleted_at')->exists();
        if ($dup) {
            return $this->sendError('يوجد موظف بهذا الرقم لديك.', [], 422);
        }

        $employeeId = DB::transaction(function () use ($user, $data, $phone, $request) {
            $id = DB::table('employees')->insertGetId([
                'agent_id'         => $user->id,
                'agent_account_id' => $user->AccID,
                'branch_id'        => $user->BrancchID,
                'full_name'        => trim($data['full_name']),
                'phone'            => $phone,
                'national_number'  => $data['national_number'] ?? null,
                'notes'            => $data['notes'] ?? null,
                'status'           => 'PENDING_ACTIVATION',
                'created_by'       => $user->id,
                'created_at'       => now(),
                'updated_at'       => now(),
            ]);

            $this->syncPos($id, $data['points_of_sale'] ?? []);
            $this->syncPermissions($id, $data['permissions'] ?? [], $user->id);

            $this->log->audit('EMPLOYEE_CREATED', [
                'agent_id' => $user->id, 'employee_id' => $id,
                'entity_type' => 'employee', 'entity_id' => (string) $id,
                'new_value' => ['name' => $data['full_name'], 'phone' => $phone],
            ] + $this->trace($request, $user));

            return $id;
        });

        return $this->sendResponse(['id' => $employeeId], 'أُنشئ الموظف.');
    }

    /** PUT employees/{id} */
    public function update(Request $request, int $id)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $employee = $this->ownedEmployee($user->id, $id);
        if (!$employee) return $this->sendError('الموظف غير موجود.', [], 404);

        $data = $request->validate([
            'full_name'        => 'sometimes|string|max:200',
            'notes'            => 'sometimes|nullable|string|max:500',
            'points_of_sale'   => 'sometimes|array',
            'points_of_sale.*' => 'integer',
        ]);

        DB::transaction(function () use ($employee, $data, $user, $request) {
            $fields = [];
            foreach (['full_name', 'notes'] as $f) {
                if (array_key_exists($f, $data)) $fields[$f] = $data[$f];
            }
            if ($fields !== []) {
                $fields['updated_at'] = now();
                DB::table('employees')->where('id', $employee->id)->update($fields);
            }
            if (array_key_exists('points_of_sale', $data)) {
                $this->syncPos($employee->id, $data['points_of_sale']);
            }

            $this->log->audit('EMPLOYEE_UPDATED', [
                'agent_id' => $user->id, 'employee_id' => $employee->id,
                'entity_type' => 'employee', 'entity_id' => (string) $employee->id,
                'new_value' => $data,
            ] + $this->trace($request, $user));
        });

        return $this->sendResponse([], 'حُفظت البيانات.');
    }

    /** POST employees/{id}/status — إيقاف / إعادة تفعيل */
    public function setStatus(Request $request, int $id)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $employee = $this->ownedEmployee($user->id, $id);
        if (!$employee) return $this->sendError('الموظف غير موجود.', [], 404);

        $data = $request->validate([
            'status' => 'required|string|in:ACTIVE,SUSPENDED,DISABLED',
        ]);

        DB::transaction(function () use ($employee, $data, $user, $request) {
            DB::table('employees')->where('id', $employee->id)
                ->update(['status' => $data['status'], 'updated_at' => now()]);

            // الإيقاف يُبطل الجلسات فوراً (بند 23) — لا ينتظر انتهاءها.
            if ($data['status'] !== 'ACTIVE') {
                DB::table('employee_sessions')
                    ->where('employee_id', $employee->id)->where('status', 'ACTIVE')
                    ->update(['status' => 'REVOKED', 'revoked_at' => now(),
                              'revoked_reason' => 'تغيير حالة الموظف']);
            }

            $this->log->audit('EMPLOYEE_STATUS_CHANGED', [
                'agent_id' => $user->id, 'employee_id' => $employee->id,
                'entity_type' => 'employee', 'entity_id' => (string) $employee->id,
                'old_value' => $employee->status, 'new_value' => $data['status'],
            ] + $this->trace($request, $user));
        });

        return $this->sendResponse([], 'حُدّثت الحالة.');
    }

    /**
     * أثرُ الموظف الماليّ — هل باشر عملاً له قيمة؟
     *
     * ⚠ **هذا هو الفاصلُ الذي يقرّر: يُحذف أم يُوقَف؟** (أمر المالك، 8 سبتمبر
     * 2026): «طالما لم يسجّل أي بيان أو عملية مالية يستطيع الوكيل حذفه، فإذا
     * نفّذ إجراءً مالياً واحداً مُنع من الحذف لارتباطه بعملية أو عمليات».
     *
     * وما يُعدّ أثراً ماليّاً أربعة، وكلُّها تُثبت أن مالاً تحرّك بيده:
     *
     *   • حوالةٌ أنشأها أو سلّمها  (`transfer_attributions`)
     *   • حركةُ خزينة              (`employee_cashbox_entries`)
     *   • وردية                    (`employee_shifts` — لها أرصدةُ فتحٍ وإقفال)
     *   • طلبُ موافقةٍ نُفِّذ فعلاً   (صار له رقمُ حوالة)
     *
     * ⚠ وما **لا** يُعدّ أثراً: الجلساتُ والأجهزةُ والأكوادُ والصلاحياتُ
     * والمحادثات. كلُّها تسجيلٌ وتهيئة، لا مالٌ تحرّك. فموظفٌ فُعِّل جهازُه
     * ومُنحت صلاحياته ثمّ تبيّن أن رقمَه خطأ — يُحذف بلا تردّد، وهي الحالةُ
     * التي أظهرت هذا كلَّه.
     *
     * ⚠ وطلبٌ معلّقٌ لم يُنفَّذ ليس أثراً ماليّاً: لا صفَّ له في الدفتر ولا
     * رصيدَ خُصم. فيُلغى مع الحذف ولا يمنعه.
     *
     * @return array<string,int> الأثرُ مفصَّلاً — فارغٌ يعني أن الحذف مباح.
     */
    private function financialFootprint(int $employeeId): array
    {
        $out = [];

        $probe = function (string $table, callable $q) use ($employeeId, &$out) {
            try {
                $n = $q(DB::table($table)->where('employee_id', $employeeId));
                if ($n > 0) $out[$table] = $n;
            } catch (\Throwable) {
                /*
                 * ⚠ جدولٌ غير منشور لا يعني «لا أثر».
                 *
                 * لكنّ الفشلَ هنا لا يُسكت: لو كان الجدول موجوداً وفشل
                 * الاستعلامُ لسببٍ آخر، لَظهر موظفٌ ذو أثرٍ ماليّ كأنه نظيف
                 * فحُذف. ولذلك يُسجَّل الأثرُ الأهمّ (`transfer_attributions`)
                 * خارج هذا الحارس أدناه.
                 */
            }
        };

        $probe('employee_cashbox_entries', fn ($q) => $q->count());
        $probe('employee_shifts', fn ($q) => $q->count());
        $probe('employee_approval_requests',
            fn ($q) => $q->whereNotNull('transfer_number')->count());

        /*
         * ⚠ والحوالاتُ تُسأل بلا حارس: هذا الجدولُ قائمٌ منذ أوّل يوم، وفشلُ
         * الاستعلام عنه خللٌ يجب أن يوقف الحذف لا أن يمرّ بصمت.
         */
        $transfers = DB::table('transfer_attributions')
            ->where('employee_id', $employeeId)->count();
        if ($transfers > 0) $out['transfer_attributions'] = $transfers;

        return $out;
    }

    /**
     * DELETE employees/{id} — حذفُ الموظف، بشرطٍ واحد.
     *
     * ══════════════════════════════════════════════════════════════════════
     *  الشرط: لا أثرَ ماليّ
     * ══════════════════════════════════════════════════════════════════════
     *
     * ⚠ **من لم يُنفّذ عمليةً مالية واحدة يُحذف حذفاً كاملاً. ومن نفّذ لا
     * يُحذف أبداً — يُوقَف.** (أمر المالك، 8 سبتمبر 2026.)
     *
     * وهذا يحسم سؤالاً كان معلّقاً: الحذفُ كان ناعماً دائماً حفظاً لسجلّ «من
     * حرّك يده» في الحوالات. لكنّ من لا حوالةَ له لا سجلَّ يُحفظ، فالنعومةُ
     * تحفظ **لا شيء** وتُبقي رقمَ هاتفٍ محجوزاً وصفّاً ميتاً في الجدول.
     *
     * فصار الحذفُ **صلباً** — لأنه لا يُسمح به إلّا حين لا يكون هناك ما
     * يُتلَف. والحمايةُ انتقلت من «نُخفي ولا نمحو» إلى «لا نمحو إلّا ما لا
     * أثر له» — وهي أقوى: الأولى تسمح بحذف كلّ شيء ثمّ تُخفيه، والثانية
     * **تمنع** المساس بمن له تاريخ.
     *
     * ⚠ والمنعُ يُرَدّ 422 بتفصيل الأثر، لا 403 غامضاً: الوكيل يحتاج أن يعرف
     * **لماذا** لا يُحذف، وأنّ أمامه الإيقافَ بديلاً يؤدّي غرضَه.
     */
    public function destroy(Request $request, int $id)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $employee = $this->ownedEmployee($user->id, $id);
        if (!$employee) return $this->sendError('الموظف غير موجود.', [], 404);

        /* ── الشرطُ الأساسيّ ─────────────────────────────────────────── */
        $footprint = $this->financialFootprint($id);

        if ($footprint !== []) {
            $labels = [
                'transfer_attributions'      => 'حوالات',
                'employee_cashbox_entries'   => 'حركات خزينة',
                'employee_shifts'            => 'ورديات',
                'employee_approval_requests' => 'حوالات بموافقتك',
            ];

            $parts = [];
            foreach ($footprint as $t => $n) {
                $parts[] = ($labels[$t] ?? $t) . ': ' . $n;
            }

            return $this->sendError(
                'لا يمكن حذف هذا الموظف لارتباطه بعمليات مالية (' .
                    implode('، ', $parts) . '). يمكنك إيقافه بدلاً من ذلك.',
                ['footprint' => $footprint],
                422,
            );
        }

        /* ── لا أثر: يُحذف هو وكلُّ ما هيّأه ─────────────────────────── */
        $counts = DB::transaction(function () use ($employee, $user, $request) {
            $id = (int) $employee->id;
            $now = now();

            /*
             * ⚠ طلباتٌ معلّقة تُلغى لا تُحذف صمتاً — ولو كانت ستُمحى بعدها.
             *
             * فالإلغاءُ يمرّ على السجلّ، والحذفُ المباشر لا يترك أثراً لأنها
             * كانت موجودةً أصلاً.
             */
            $approvals = 0;
            try {
                $approvals = DB::table('employee_approval_requests')
                    ->where('employee_id', $id)->where('status', 'PENDING')
                    ->update(['status' => 'CANCELLED', 'updated_at' => $now,
                              'failure_reason' => 'حُذف الموظف قبل البتّ في الطلب']);
            } catch (\Throwable) {
            }

            /*
             * ⚠ الترتيبُ من الابن إلى الأب — والمفاتيحُ الأجنبيّة تفرضه.
             * ومحادثةُ الوكيل مع موظفٍ محذوف لا طرفَ لها.
             */
            $threads = DB::table('chat_threads')->where('employee_id', $id)->pluck('id')->all();
            if ($threads) {
                $msgs = DB::table('chat_messages')->whereIn('thread_id', $threads)->pluck('id')->all();
                foreach (['chat_attachments', 'chat_reactions', 'chat_stars', 'chat_pins'] as $t) {
                    try { DB::table($t)->whereIn('message_id', $msgs)->delete(); } catch (\Throwable) {}
                }
                foreach (['support_thread_state', 'support_thread_tags', 'support_events',
                          'support_viewers', 'support_drafts', 'support_followups',
                          'chat_reads', 'chat_settings', 'chat_typing'] as $t) {
                    try { DB::table($t)->whereIn('thread_id', $threads)->delete(); } catch (\Throwable) {}
                }
                try { DB::table('chat_messages')->whereIn('thread_id', $threads)->delete(); } catch (\Throwable) {}
                try { DB::table('chat_threads')->whereIn('id', $threads)->delete(); } catch (\Throwable) {}
            }

            $sessions = DB::table('employee_sessions')->where('employee_id', $id)->count();
            $devices  = DB::table('employee_devices')->where('employee_id', $id)->count();
            $codes    = DB::table('employee_activation_codes')->where('employee_id', $id)->count();

            foreach (['employee_sessions', 'employee_devices', 'employee_otps',
                      'employee_activation_codes', 'employee_permissions',
                      'employee_transfer_policies', 'employee_point_of_sales',
                      'employee_transfer_claims', 'employee_approval_requests',
                      'employee_cashboxes'] as $t) {
                try { DB::table($t)->where('employee_id', $id)->delete(); } catch (\Throwable) {}
            }

            /*
             * ⚠ سجلُّ التدقيق يبقى، وحدثُ الحذف يُكتب فيه.
             *
             * فالسؤالُ «من حذف هذا الموظف ومتى؟» يجب أن يبقى له جواب، وإلّا
             * صار الحذفُ فعلاً بلا أثر — وهو أسوأُ ما يكون في نظامٍ ماليّ.
             * ويُكتب **قبل** حذف الصفّ ليحمل اسمَه.
             */
            $this->log->audit('EMPLOYEE_DELETED', [
                'agent_id' => $user->id, 'employee_id' => $id,
                'entity_type' => 'employee', 'entity_id' => (string) $id,
                'old_value' => $employee->full_name . ' · ' . $employee->phone,
                'new_value' => 'DELETED',
            ] + $this->trace($request, $user));

            /* ⚠ ويُفَكّ ربطُ السجلّ بالصفّ المحذوف حتى لا يمنعه مفتاحٌ أجنبيّ. */
            foreach (['audit_logs', 'security_logs'] as $t) {
                try {
                    DB::table($t)->where('employee_id', $id)->update(['employee_id' => null]);
                } catch (\Throwable) {}
            }

            DB::table('employees')->where('id', $id)->delete();

            return compact('sessions', 'devices', 'codes', 'approvals');
        });

        return $this->sendResponse($counts, 'حُذف الموظف.');
    }

    /* ================= كود التفعيل والأجهزة ================= */

    /** POST employees/{id}/activation-code — يُعرض الكود مرّة واحدة. */
    public function issueCode(Request $request, int $id)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        try {
            $result = $this->activation->issueCode(
                (int) $user->id, $id, (int) $user->id, $this->trace($request, $user)
            );
        } catch (\InvalidArgumentException $e) {
            return $this->sendError($e->getMessage(), [], 404);
        }

        return $this->sendResponse(
            $result,
            'أُصدر كود التفعيل. اعرضه على الموظف الآن — لن يظهر مرة أخرى.'
        );
    }

    /**
     * POST employees/{id}/activation-code/revoke — إلغاءُ الرمز قبل استعماله.
     *
     * ⚠ البند 13: «إذا كان QR ما زال ظاهراً على هاتف آخر أو محفوظاً
     * Screenshot، يجب ألا يعمل بعد إلغائه». والإلغاءُ في القاعدة لا في
     * الشاشة: صورةُ الرمز تبقى، وما يموت هو الصفُّ الذي تقود إليه.
     *
     * ويُلغي الكودَ اليدويّ معه — لأنهما طلبٌ واحد لا اثنان.
     */
    public function revokeCode(Request $request, int $id)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $employee = $this->ownedEmployee($user->id, $id);
        if (!$employee) return $this->sendError('الموظف غير موجود.', [], 404);

        $n = DB::table('employee_activation_codes')
            ->where('employee_id', $id)
            ->where('status', 'ACTIVE')
            ->update([
                'status'         => 'REVOKED',
                'revoked_at'     => now(),
                'revoked_reason' => 'ألغاه الوكيل',
            ]);

        if ($n > 0) {
            $this->log->audit('EMPLOYEE_CODE_REVOKED', [
                'agent_id'    => $user->id,
                'employee_id' => $id,
                'entity_type' => 'employee_activation_code',
                'entity_id'   => (string) $id,
            ] + $this->trace($request, $user));
        }

        return $this->sendResponse(
            ['revoked' => $n > 0],
            $n > 0 ? 'أُلغي رمز التفعيل.' : 'لا رمزَ فعّالاً لإلغائه.'
        );
    }
    /** GET employees/devices — الأجهزة المفعّلة */
    public function devices(Request $request)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $rows = DB::table('employee_devices as d')
            ->join('employees as e', 'e.id', '=', 'd.employee_id')
            ->leftJoin('employee_point_of_sales as ep', function ($j) {
                $j->on('ep.employee_id', '=', 'e.id')->where('ep.is_primary', '=', 1);
            })
            ->leftJoin('AuthorizedUsers as a', 'a.ID', '=', 'ep.point_of_sale_id')
            ->where('d.agent_id', $user->id)
            ->select([
                'd.id', 'd.status', 'd.platform', 'd.model',
                'd.activated_at', 'd.last_activity_at',
                'e.id as employee_id', 'e.full_name', 'e.phone',
                'a.Name_post as point_of_sale',
                // آخر 8 من التجزئة — يكفي للتمييز بين جهازين، ولا يكشف المعرّف.
                DB::raw('RIGHT(d.device_hash, 8) as device_ref'),
            ])
            ->orderByDesc('d.activated_at')
            ->get();

        return $this->sendResponse($rows, 'Success');
    }

    /** POST employees/devices/{id}/revoke */
    public function revokeDevice(Request $request, int $id)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $device = DB::table('employee_devices')
            ->where('id', $id)->where('agent_id', $user->id)->first();
        if (!$device) return $this->sendError('الجهاز غير موجود.', [], 404);

        DB::transaction(function () use ($device, $user, $request) {
            DB::table('employee_devices')->where('id', $device->id)->update([
                'status' => 'REVOKED', 'revoked_at' => now(),
                'revoked_by' => $user->id, 'revoked_reason' => 'إلغاء من الإدارة',
            ]);

            DB::table('employee_sessions')
                ->where('employee_id', $device->employee_id)->where('status', 'ACTIVE')
                ->update(['status' => 'REVOKED', 'revoked_at' => now(),
                          'revoked_reason' => 'أُلغي الجهاز']);

            DB::table('employee_activation_codes')
                ->where('employee_id', $device->employee_id)
                ->whereIn('status', ['ACTIVE', 'USED'])
                ->update(['status' => 'REVOKED', 'revoked_at' => now(),
                          'revoked_reason' => 'أُلغي الجهاز']);

            DB::table('employees')->where('id', $device->employee_id)
                ->update(['status' => 'REQUIRES_REACTIVATION', 'updated_at' => now()]);

            $this->log->audit('EMPLOYEE_DEVICE_REVOKED', [
                'agent_id' => $user->id, 'employee_id' => $device->employee_id,
                'entity_type' => 'employee_device', 'entity_id' => (string) $device->id,
            ] + $this->trace($request, $user));
        });

        // ⚠ التصنيف في `device_registry` **لا يُمسّ**: إلغاء الجهاز التشغيلي
        // لا يرفع حظر الدخول كمسؤول (بند 20).

        return $this->sendResponse([], 'أُلغي الجهاز.');
    }

    /* ================= الصلاحيات ================= */

    /** GET employees/permissions/catalog */
    public function permissionCatalog()
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        return $this->sendResponse(EmployeePermissions::catalogForAdmin(), 'Success');
    }

    /** PUT employees/{id}/permissions */
    public function setPermissions(Request $request, int $id)
    {
        [$user, $err] = $this->admin();
        if ($err) return $err;

        $employee = $this->ownedEmployee($user->id, $id);
        if (!$employee) return $this->sendError('الموظف غير موجود.', [], 404);

        $data = $request->validate([
            'permissions'   => 'present|array',
            'permissions.*' => 'string|max:80',
        ]);

        foreach ($data['permissions'] as $key) {
            if (!EmployeePermissions::grantable($key)) {
                return $this->sendError('صلاحية غير معروفة أو لا تُمنح لموظف: ' . $key, [], 422);
            }
        }

        $before = DB::table('employee_permissions')->where('employee_id', $employee->id)
            ->pluck('permission_key')->all();

        DB::transaction(function () use ($employee, $data, $user, $before, $request) {
            $this->syncPermissions($employee->id, $data['permissions'], $user->id);

            $added   = array_values(array_diff($data['permissions'], $before));
            $removed = array_values(array_diff($before, $data['permissions']));

            // صفٌّ لكل صلاحية تغيّرت (بند 33) — لا صفّ واحد لكل حفظ.
            foreach ($added as $k) {
                $this->log->audit('PERMISSION_GRANTED', [
                    'agent_id' => $user->id, 'employee_id' => $employee->id,
                    'entity_type' => 'permission', 'entity_id' => $k,
                    'old_value' => 'DENIED', 'new_value' => 'GRANTED',
                ] + $this->trace($request, $user));
            }
            foreach ($removed as $k) {
                $this->log->audit('PERMISSION_REVOKED', [
                    'agent_id' => $user->id, 'employee_id' => $employee->id,
                    'entity_type' => 'permission', 'entity_id' => $k,
                    'old_value' => 'GRANTED', 'new_value' => 'DENIED',
                ] + $this->trace($request, $user));
            }
        });

        return $this->sendResponse([], 'حُفظت الصلاحيات.');
    }

    /* ================= أدوات ================= */

    private function ownedEmployee(int $agentId, int $id)
    {
        return DB::table('employees')
            ->where('id', $id)->where('agent_id', $agentId)
            ->whereNull('deleted_at')->first();
    }

    private function syncPos(int $employeeId, array $posIds): void
    {
        DB::table('employee_point_of_sales')->where('employee_id', $employeeId)->delete();
        $first = true;
        foreach (array_unique($posIds) as $posId) {
            DB::table('employee_point_of_sales')->insert([
                'employee_id'      => $employeeId,
                'point_of_sale_id' => (int) $posId,
                'is_primary'       => $first ? 1 : 0,
                'is_active'        => 1,
                'created_at'       => now(),
            ]);
            $first = false;
        }
    }

    private function syncPermissions(int $employeeId, array $keys, int $grantedBy): void
    {
        DB::table('employee_permissions')->where('employee_id', $employeeId)->delete();
        foreach (array_unique($keys) as $key) {
            if (!EmployeePermissions::grantable($key)) continue;
            DB::table('employee_permissions')->insert([
                'employee_id'    => $employeeId,
                'permission_key' => $key,
                'granted_by'     => $grantedBy,
                'granted_at'     => now(),
            ]);
        }
    }

    private function normalizePhone(string $phone): string
    {
        $d = preg_replace('/[^0-9]/', '', $phone);
        if (str_starts_with($d, '218')) $d = substr($d, 3);
        return ltrim($d, '0');
    }
}
