<?php

namespace App\Http\Middleware;

use App\Services\Employees\EmployeePermissions;
use Closure;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

/**
 * حارس جلسة الموظف.
 *
 * ⚠ **لا يقرأ من الطلب إلا الرمز.** كل ما عداه — الوكيل، الموظف، نقطة
 * البيع، الجهاز، الصلاحيات — يُقرأ من قاعدة البيانات بذلك الرمز (بند 50).
 * `agent_id` أو `permissions` قادمةً من الهاتف لا تُصدَّق ولا تُقرأ أصلاً.
 *
 * والصلاحيات تُقرأ **مع كل طلب** لا من الرمز: بند 32 يوجب أن يسري سحب
 * الصلاحية فوراً، ورمزٌ يحمل صلاحياته داخله يبقى صالحاً بها حتى ينتهي.
 */
class AuthenticateEmployee
{
    public function handle(Request $request, Closure $next, ?string $permission = null)
    {
        $token = $request->bearerToken();
        if (!$token) {
            return response()->json([
                'data' => null, 'message' => 'غير مصرّح.', 'success' => false,
            ], 401);
        }

        $session = DB::table('employee_sessions')
            ->where('access_token_hash', hash('sha256', $token))
            ->where('status', 'ACTIVE')
            ->first();

        if (!$session) {
            return response()->json([
                'data' => null, 'message' => 'انتهت الجلسة. راجع الإدارة لإعادة التفعيل.',
                'success' => false,
            ], 401);
        }

        $employee = DB::table('employees')
            ->where('id', $session->employee_id)
            ->whereNull('deleted_at')
            ->first();

        // الموظف الموقوف تسقط جلسته في الطلب نفسه، لا في الطلب التالي (بند 23).
        if (!$employee || $employee->status !== 'ACTIVE') {
            DB::table('employee_sessions')->where('id', $session->id)->update([
                'status' => 'REVOKED', 'revoked_at' => now(),
                'revoked_reason' => 'حالة الموظف: ' . ($employee->status ?? 'محذوف'),
            ]);
            return response()->json([
                'data' => null, 'message' => 'الحساب غير مفعّل. راجع الإدارة.',
                'success' => false,
            ], 401);
        }

        // الجهاز الذي فُتحت به الجلسة يجب أن يكون ما زال فعّالاً: إلغاؤه من
        // لوحة الإدارة يسري فوراً (بند 18).
        $deviceOk = DB::table('employee_devices')
            ->where('employee_id', $employee->id)
            ->where('device_hash', $session->device_hash)
            ->where('status', 'ACTIVE')
            ->exists();

        if (!$deviceOk) {
            DB::table('employee_sessions')->where('id', $session->id)->update([
                'status' => 'REVOKED', 'revoked_at' => now(),
                'revoked_reason' => 'أُلغي الجهاز',
            ]);
            return response()->json([
                'data' => null, 'message' => 'أُلغي هذا الجهاز. راجع الإدارة.',
                'success' => false,
            ], 401);
        }

        $granted = DB::table('employee_permissions')
            ->where('employee_id', $employee->id)
            ->pluck('permission_key')
            ->all();

        // Default Deny: الصلاحية المطلوبة يجب أن تكون ممنوحة صراحةً، وأن
        // تكون أصلاً مما يجوز منحه لموظف.
        if ($permission !== null) {
            if (!EmployeePermissions::grantable($permission)
                || !in_array($permission, $granted, true)) {
                app(\App\Services\Employees\EmployeeAuditLogger::class)->security(
                    'UNAUTHORIZED',
                    'محاولة وصول بلا صلاحية: ' . $permission,
                    [
                        'agent_id' => $employee->agent_id,
                        'employee_id' => $employee->id,
                        'device_hash' => $session->device_hash,
                        'ip' => $request->ip(),
                    ]
                );
                return response()->json([
                    'data' => null, 'message' => 'لا تملك صلاحية هذه العملية.',
                    'success' => false,
                ], 403);
            }
        }

        DB::table('employee_sessions')->where('id', $session->id)
            ->update(['last_used_at' => now()]);
        DB::table('employees')->where('id', $employee->id)
            ->update(['last_activity_at' => now()]);

        // السياق الموثوق — منه تقرأ المتحكّمات، لا من جسم الطلب.
        $request->attributes->set('employee', $employee);
        $request->attributes->set('employee_session', $session);
        $request->attributes->set('employee_permissions', $granted);

        return $next($request);
    }
}
