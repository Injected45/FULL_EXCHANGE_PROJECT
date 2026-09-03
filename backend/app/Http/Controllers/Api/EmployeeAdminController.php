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
                'e.id', 'e.full_name', 'e.phone', 'e.status',
                'e.last_login_at', 'e.last_activity_at', 'e.activated_at',
                'd.id as device_id', 'd.platform', 'd.model',
                'd.activated_at as device_activated_at', 'd.last_activity_at as device_last_activity',
            ])
            ->orderByDesc('e.id')
            ->get();

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

        $out = $rows->map(function ($r) use ($pos, $perms) {
            $r->points_of_sale = $pos->get($r->id, collect())->values();
            $r->permissions    = $perms->get($r->id, collect())->pluck('permission_key')->values();
            return $r;
        });

        return $this->sendResponse($out, 'Success');
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
