<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\Support\SupportAudit;
use App\Services\Support\SupportAuthService;
use App\Services\Support\SupportPermissions;
use Illuminate\Http\Request;

/**
 * دخول مركز الدعم — نقاطٌ خارج الحارس بطبيعتها.
 *
 * ⚠ لا شيء في هذا المتحكّم يمسّ المال ولا يقرأ جدولاً مالياً.
 */
class SupportAuthController extends BaseController
{
    public function __construct(
        private SupportAuthService $auth,
        private SupportAudit $audit,
    ) {
    }

    /** POST support/auth/login  {username, password} */
    public function login(Request $request)
    {
        $out = $this->auth->login(
            (string) $request->input('username', ''),
            (string) $request->input('password', ''),
            $request->ip(),
            $request->userAgent(),
        );

        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        $this->audit->log($out['staff'], SupportAudit::LOGIN, null, null, null, $request->ip());

        return $this->sendResponse([
            'token'   => $out['token'],
            'staff'   => $this->shape($out['staff']),
            // الكتالوج يُرسَل مع الدخول لا في نداءٍ ثانٍ: الواجهةُ تحتاجه
            // لترسم الأسماء العربية للصلاحيات، وهو ثابتٌ لا يتغيّر بينهما.
            'catalog' => SupportPermissions::catalogFor($out['staff']->role),
            'roles'   => SupportPermissions::ROLES,
        ], 'أهلاً بك.');
    }

    /** POST support/auth/logout */
    public function logout(Request $request)
    {
        $staff = $request->attributes->get('support_staff');
        $this->audit->log($staff, SupportAudit::LOGOUT, null, null, null, $request->ip());
        $this->auth->logout($request->bearerToken());

        return $this->sendResponse(['ok' => true], 'إلى اللقاء.');
    }

    /**
     * GET support/auth/me
     *
     * تُنادى عند فتح الصفحة لاستعادة الجلسة، ومع كل تحديثٍ للصلاحيات: ما
     * يُعرض في الشاشة يجب أن يتبع ما في القاعدة، لا ما حُفظ في المتصفّح
     * يوم الدخول.
     */
    public function me(Request $request)
    {
        $staff = $request->attributes->get('support_staff');

        return $this->sendResponse([
            'staff'   => $this->shape($staff),
            'catalog' => SupportPermissions::catalogFor($staff->role),
            'roles'   => SupportPermissions::ROLES,
        ], 'Success');
    }

    /** POST support/auth/password  {current, new} */
    public function changePassword(Request $request)
    {
        $staff = $request->attributes->get('support_staff');

        $err = $this->auth->changePassword(
            (int) $staff->id,
            (string) $request->input('current', ''),
            (string) $request->input('new', ''),
        );

        if ($err !== null) {
            return $this->sendError($err, [], 422);
        }

        // الجلسات كلُّها أُلغيت — بما فيها هذه. والواجهة تعيد الدخول.
        return $this->sendResponse(
            ['ok' => true],
            'تم تغيير كلمة المرور. سجّل الدخول من جديد.'
        );
    }

    private function shape(object $s): array
    {
        return [
            'id'          => (int) $s->id,
            'name'        => $s->name,
            'username'    => $s->username,
            'role'        => $s->role,
            'role_label'  => SupportPermissions::ROLES[$s->role] ?? $s->role,
            'must_change' => (bool) $s->must_change,
            // حالتُه المخزّنة — وإلّا أظهر المبدّل «متاح» لمن ضبطها
            // «مشغول» أمس، فيظنّ أنّها لم تُحفظ.
            'presence'    => $s->presence ?? 'AVAILABLE',
            'permissions' => $s->permissions ?? [],
        ];
    }
}
