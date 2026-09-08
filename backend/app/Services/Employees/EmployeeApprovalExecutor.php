<?php

namespace App\Services\Employees;

use App\Http\Controllers\Api\depositController;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

/**
 * تنفيذُ طلبٍ وافق عليه الوكيل — عبر مسار الحوالة القائم، لا غيرِه.
 *
 * ══════════════════════════════════════════════════════════════════════════
 *  القاعدةُ المعمارية التي يجسّدها هذا الصنف
 * ══════════════════════════════════════════════════════════════════════════
 *
 *   الموظفُ ينشئ الطلب · السياسةُ تفحصه · الوكيلُ يعتمد التجاوز ·
 *   **نظامُ الحوالات القائم وحده ينفّذ** · والرحالة تحاسب الوكيل.
 *
 * ⚠ فلا سطرَ منطقٍ ماليّ هنا. ما يجري هو استدعاءُ `InternalExchange` نفسِها
 * التي يستدعيها الوكيل من تطبيقه، بهويّة الوكيل، بنفس الحقول التي أرسلها
 * الموظف. فالعمولةُ والرصيدُ والحدودُ والكودُ كلُّها من الشيفرة القائمة.
 *
 * ══════════════════════════════════════════════════════════════════════════
 *  ولماذا يُعاد التحقّق كلُّه هنا (البند 9)
 * ══════════════════════════════════════════════════════════════════════════
 *
 * الطلبُ قد يبقى ساعاتٍ عند الوكيل، وفي تلك الساعات يمكن أن:
 *
 *   • تُسحب صلاحيةُ `CREATE_TRANSFER` من الموظف
 *   • يُوقَف الموظف أو يُحذف
 *   • يتغيّر رصيدُ الوكيل فلا يكفي
 *   • تُنشأ حوالةٌ أخرى قبل دقيقة فتمنعها قاعدةُ المهلة
 *
 * ⚠ **وموافقةُ الوكيل تعالج سببَ التصعيد وحدَه** (البند 46): هي تقول «أقبل
 * تجاوزَ سياسة موظفي لهذه العملية»، ولا تقول «نفّذوها مهما كان». فكلُّ فحصٍ
 * كان يجري لو أرسلها الموظف الآن يجري هنا حرفياً.
 */
class EmployeeApprovalExecutor
{
    public function __construct(
        private readonly EmployeeActsAsAgent $actor,
        private readonly EmployeeApprovals $approvals,
        private readonly EmployeeAuditLogger $log,
    ) {
    }

    /**
     * @return array{ok:bool, message:string, transfer_number:?string}
     */
    public function execute(object $req): array
    {
        /* ── 1) الموظف ما زال قائماً وفعّالاً؟ ───────────────────────── */
        $employee = DB::table('employees')
            ->where('id', $req->employee_id)
            ->whereNull('deleted_at')
            ->first();

        if (!$employee) {
            return $this->fail($req, 'الموظف لم يعد موجوداً.');
        }

        if ($employee->status !== 'ACTIVE') {
            return $this->fail($req, 'حساب الموظف غير مفعّل، فلم تُنفَّذ الحوالة.');
        }

        /*
         * ⚠ 2) والصلاحيةُ ما زالت ممنوحة؟
         *
         * سحبُ الصلاحية بعد إنشاء الطلب يعني أن الوكيل قرّر منعَ هذا الموظف
         * من إنشاء الحوالات — وتنفيذُ طلبٍ قديمٍ له بعد ذلك يُبطل القرار.
         */
        $hasPermission = DB::table('employee_permissions')
            ->where('employee_id', $req->employee_id)
            ->where('permission_key', 'CREATE_TRANSFER')
            ->exists();

        if (!$hasPermission) {
            return $this->fail($req, 'صلاحية إنشاء الحوالة سُحبت من الموظف.');
        }

        /* ── 3) تبعيةُ الموظف لم تتغيّر عمّا كانت وقتَ الطلب ─────────── */
        if ((int) $employee->agent_id !== (int) $req->agent_id) {
            return $this->fail($req, 'تغيّرت تبعية الموظف، فلم تُنفَّذ الحوالة.');
        }

        /* ── 4) قاعدةُ الدقيقة — هنا موضعُها، عند التنفيذ الفعليّ ────── */
        $agentAcc = DB::table('users')->where('id', $req->agent_id)->value('AccID');

        if ($agentAcc !== null && $this->actor->minuteRuleBlocks((int) $agentAcc)) {
            return $this->fail($req,
                'أُنشئت حوالة أخرى قبل قليل. أعد الموافقة بعد دقيقة.', keepPending: true);
        }

        /* ── 5) التنفيذ بمسار الوكيل نفسِه ───────────────────────────── */
        $payload = json_decode($req->payload ?? '{}', true) ?: [];

        try {
            $response = $this->actor->as((int) $req->agent_id, function ($agent) use ($payload) {
                /*
                 * ⚠ طلبٌ جديد يُبنى من الحمولة المحفوظة — لا من طلب HTTP
                 * الحاليّ. فالطلبُ الحاليّ هو طلبُ **الوكيل** وهو يضغط
                 * «موافقة»، ولا علاقة لحقوله بحوالةٍ أنشأها موظفٌ قبل ساعة.
                 */
                $inner = new Request();
                $inner->merge($payload);

                // ⚠ الحسابُ من الوكيل لا من الحمولة — كما في المسار المباشر.
                $inner->merge([
                    'AccID'      => $agent->AccID,
                    'country_id' => $payload['country_id'] ?? 1,
                ]);

                return app(depositController::class)->InternalExchange($inner);
            });
        } catch (\Throwable $e) {
            return $this->fail($req, 'تعذّر تنفيذ الحوالة: ' . mb_substr($e->getMessage(), 0, 200));
        }

        $body = json_decode($response->getContent(), true);
        $ok = ($body['success'] ?? false) === true;

        if (!$ok) {
            /*
             * ⚠ رسالةُ المسار الماليّ كما هي — «رصيد غير كافٍ» وغيرُها.
             * وترجمتُها هنا تعني نصّين يفترقان عن نصّ الوكيل.
             */
            return $this->fail($req, (string) ($body['message'] ?? 'رفض المسار المالي تنفيذ الحوالة.'));
        }

        $t = $body['data']['transfer'] ?? [];
        $code = (string) ($t['Code'] ?? '');

        /* ── 6) النسبة — بعد نجاح الكتابة لا قبلها ───────────────────── */
        $this->actor->attributeCreate(
            // ⚠ لقطةُ التنفيذ من **الطلب** لا من جلسةٍ حيّة: جلسةُ الموظف
            // قد تكون انتهت وهو نائم، والحوالةُ تبقى منسوبةً إليه وإلى
            // نقطة بيعه وجهازه كما كانت لحظةَ الإنشاء (البند 32 و33).
            (object) [
                'id'       => $req->employee_id,
                'agent_id' => $req->agent_id,
            ],
            (object) [
                'active_pos_id' => $req->point_of_sale_id,
                'device_hash'   => $req->device_hash,
                'id'            => $req->session_id,
            ],
            $code,
            (float) ($t['OverallVal'] ?? $req->amount),
            $req->recipient_phone,
            $req->recipient_name,
        );

        $this->approvals->markExecuted((int) $req->id, $code, null);

        $this->log->audit('EMPLOYEE_APPROVAL_EXECUTED', [
            'agent_id'    => (int) $req->agent_id,
            'employee_id' => (int) $req->employee_id,
            'entity_type' => 'transfer',
            'entity_id'   => $code,
        ]);

        return ['ok' => true, 'message' => 'نُفِّذت الحوالة.', 'transfer_number' => $code];
    }

    /**
     * إخفاقٌ بعد الموافقة.
     *
     * ⚠ `keepPending` لحالةٍ واحدة: مهلةُ الدقيقة. فهي عائقٌ **مؤقّت** يزول
     * بنفسه بعد ستّين ثانية، وحرقُ الطلب عليه يعني أن يعيد الموظف إدخالَ
     * الحوالة كلَّها ويعيد الوكيل مراجعتَها — لسببٍ انتهى قبل أن يقرأه أحد.
     * فيُعاد الطلبُ إلى `PENDING` ليضغط الوكيل «موافقة» مرّةً أخرى.
     *
     * وما عداه يُثبَّت `FAILED`: رصيدٌ غير كافٍ أو صلاحيةٌ مسحوبة قرارٌ قائم،
     * وتركُه معلّقاً يعني طلباً يُعاد تجريبُه إلى الأبد.
     */
    private function fail(object $req, string $message, bool $keepPending = false): array
    {
        if ($keepPending) {
            DB::table('employee_approval_requests')->where('id', $req->id)->update([
                'status'         => 'PENDING',
                'decided_by'     => null,
                'decided_at'     => null,
                'failure_reason' => mb_substr($message, 0, 500),
                'updated_at'     => now(),
            ]);
        } else {
            $this->approvals->markExecuted((int) $req->id, null, $message);
        }

        return ['ok' => false, 'message' => $message, 'transfer_number' => null];
    }
}
