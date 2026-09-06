<?php

namespace App\Http\Middleware;

use App\Services\Support\SupportAuthService;
use App\Services\Support\SupportPermissions;
use Closure;
use Illuminate\Http\Request;

/**
 * حارس جلسة موظّف الدعم — منفصلٌ تماماً عن `auth:sanctum` وعن `employee`.
 *
 * ⚠ **لا يقرأ من الطلب إلا الرمز.** الهويّةُ والدورُ والصلاحيات تُقرأ كلُّها
 * من قاعدة البيانات بذلك الرمز. `staff_id` أو `role` أو `permissions` قادمةً
 * من المتصفّح لا تُقرأ أصلاً — وهذا هو الفرقُ بين حارسٍ وزينة.
 *
 * والصلاحيات تُقرأ **مع كل طلب** لا من الرمز: سحبُ صلاحيةٍ يجب أن يسري في
 * النداء التالي مباشرة، ورمزٌ يحمل صلاحياته داخله يبقى صالحاً بها إلى أن
 * ينتهي — أي إلى اثنتي عشرة ساعة بعد قرار المدير.
 *
 * وهذا هو القرارُ نفسه المتّخذ في {@see AuthenticateEmployee}، للسبب نفسه.
 */
class AuthenticateSupport
{
    public function __construct(private SupportAuthService $auth)
    {
    }

    public function handle(Request $request, Closure $next, ?string $permission = null)
    {
        $staff = $this->auth->resolve($request->bearerToken());

        if (!$staff) {
            return response()->json([
                'data' => null, 'success' => false,
                'message' => 'انتهت الجلسة. سجّل الدخول من جديد.',
            ], 401);
        }

        // Default Deny: الصلاحيةُ يجب أن تكون ممنوحةً صراحةً **وأن يسمح بها
        // سقفُ الدور**. صفٌّ بقي من دورٍ سابق لا يفتح شيئاً.
        if ($permission !== null) {
            $ok = SupportPermissions::allowedForRole($staff->role, $permission)
                && in_array($permission, $staff->permissions, true);

            if (!$ok) {
                app(\App\Services\Support\SupportAudit::class)->log(
                    $staff,
                    'DENIED',
                    null,
                    $permission,
                    'محاولة وصول بلا صلاحية',
                    $request->ip(),
                );

                return response()->json([
                    'data' => null, 'success' => false,
                    'message' => 'لا تملك صلاحية هذه العملية.',
                ], 403);
            }
        }

        $request->attributes->set('support_staff', $staff);

        return $next($request);
    }
}
