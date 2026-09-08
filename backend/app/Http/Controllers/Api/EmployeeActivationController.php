<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\Employees\EmployeeActivationService;
use Illuminate\Http\Request;

/**
 * مسار تفعيل الموظف — **خارج `auth:sanctum` عمداً**.
 *
 * الموظف لا يملك رمزاً بعد؛ هذا هو المسار الذي يحصل به عليه. وحمايته ليست
 * توثيقاً بل الطبقات الثلاث: كود الإدارة + رمز التحقّق + ربط الجهاز، مع
 * حدّ معدّل على الرقم.
 *
 * ⚠ ولا يُصدر هذا المسار رمز Sanctum ولا يمسّ `personal_access_tokens`:
 * جلسة الموظف في `employee_sessions` وحدها. بند 22 يمنع ترقية جلسة موظف
 * إلى مسؤول، وأضمن منعٍ أن يكون الرمزان في جدولين لا يعرف أحدهما الآخر.
 */
class EmployeeActivationController extends BaseController
{
    public function __construct(private EmployeeActivationService $service)
    {
    }

    private function trace(Request $r): array
    {
        return [
            'ip'          => $r->ip(),
            'platform'    => $r->header('X-Platform'),
            'model'       => $r->header('X-Device-Model'),
            'app_version' => $r->header('X-App-Version'),
        ];
    }

    /** POST device/employee/activation/request — رقم + كود ⇦ إرسال الرمز. */
    public function requestOtp(Request $request)
    {
        $data = $request->validate([
            'phone'     => 'required|string|max:20',
            'code'      => 'required|string|max:40',
            'device_id' => 'required|string|max:200',
        ]);

        $result = $this->service->requestOtp(
            $data['phone'], $data['code'], $data['device_id'], $this->trace($request)
        );

        if (!$result['ok']) {
            // 422 لا 401: هذه ليست جلسة منتهية، والتطبيق يفرّق بينهما —
            // 401 تُخرج المستخدم من التطبيق كلّه.
            return $this->sendError($result['message'], [], 422);
        }

        return $this->sendResponse([
            'masked_phone'  => $result['masked_phone'],
            'activation_id' => $result['activation_id'],
        ], $result['message']);
    }

    /**
     * POST device/employee/activation/qr — مسحُ رمز QR ⇦ إرسال رمز التحقّق.
     *
     * ⚠ الخطوةُ الأولى نفسُها بتمثيلٍ آخر: تنتهي إلى `requestOtp` عينِها،
     * ثمّ يُكمل الموظف بـ`activation/verify` بلا فرقٍ عن الطريقة اليدوية.
     *
     * ولا يُرسَل هنا رقمُ هاتفٍ ولا معرّفُ موظّفٍ ولا وكيل: الرمزُ وحده،
     * والخادمُ يشتقّ الباقي من سجلّه — البند 6.
     */
    public function requestByQr(Request $request)
    {
        $data = $request->validate([
            'qr_token'  => 'required|string|max:200',
            'device_id' => 'required|string|max:200',
        ]);

        $result = $this->service->requestOtpByQr(
            $data['qr_token'], $data['device_id'], $this->trace($request)
        );

        if (!$result['ok']) {
            return $this->sendError($result['message'], [], 422);
        }

        return $this->sendResponse([
            'masked_phone'  => $result['masked_phone'],
            'activation_id' => $result['activation_id'],
            // اسمُ الموظف يُعرض في شاشة «جارٍ التفعيل» ليطمئنّ أنه رمزُه هو.
            'employee_name' => $result['employee_name'] ?? null,
        ], $result['message']);
    }
    /** POST device/employee/activation/verify — الرمز ⇦ ربط الجهاز وفتح الجلسة. */
    public function verifyOtp(Request $request)
    {
        /*
         * ⚠ الرقمُ **أو** معرّفُ الطلب — واحدٌ منهما يكفي ولا بدّ من أحدهما.
         *
         * فمن أدخل الكود يدوياً يعرف رقمَه، ومن مسح رمزاً لا يعرفه: ما
         * عاد إليه رقمٌ مقنَّع. والخطوةُ بعدها واحدةٌ في الحالتين.
         */
        $data = $request->validate([
            'phone'         => 'required_without:activation_id|nullable|string|max:20',
            'activation_id' => 'required_without:phone|nullable|integer',
            'otp'           => 'required|string|max:10',
            'device_id'     => 'required|string|max:200',
        ]);

        $result = $this->service->verifyOtp(
            (string) ($data['phone'] ?? ''),
            $data['otp'],
            $data['device_id'],
            $this->trace($request),
            isset($data['activation_id']) ? (int) $data['activation_id'] : null,
        );

        if (!$result['ok']) {
            return $this->sendError($result['message'], [], 422);
        }

        return $this->sendResponse($result['session'], $result['message']);
    }

    /** POST device/employee/logout — إنهاء صريح للتفعيل. */
    public function logout(Request $request)
    {
        $employee = $request->attributes->get('employee');
        if (!$employee) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $this->service->logout((int) $employee->id, $this->trace($request));

        return $this->sendResponse(
            [],
            'لتفعيل التطبيق مرة أخرى، يرجى التواصل مع الإدارة لاستلام كود جديد'
        );
    }
}
