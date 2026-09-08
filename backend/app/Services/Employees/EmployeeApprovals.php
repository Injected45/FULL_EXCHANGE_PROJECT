<?php

namespace App\Services\Employees;

use Illuminate\Support\Facades\DB;

/**
 * طلبُ موافقة الوكيل — دورةُ حياته من الإنشاء إلى القرار.
 *
 * ⚠ **الطلبُ ليس حوالةً غير منفَّذة، بل ليس حوالةً بعد.** لا صفَّ له في
 * `InternalEx`، ولا رصيدَ خُصم، ولا عمولةَ احتُسبت، ولا قيدَ كُتب (البند 4).
 * فإن رُفض أو انتهت مدّتُه، لم يقع شيءٌ يُلغى — وهذا هو الفرقُ الذي يجعل
 * الرفضَ آمناً.
 *
 * وحين يوافق الوكيل، يُمرَّر **الطلبُ نفسُه** إلى مسار الحوالة القائم؛ ولا
 * يُنشأ طلبٌ جديد ولا تُعاد كتابةُ بياناته (البند 8).
 */
class EmployeeApprovals
{
    public function __construct(
        private readonly EmployeeLimitPolicy $policy,
        private readonly EmployeeAuditLogger $log,
    ) {
    }

    /**
     * يُنشئ طلبَ موافقة — أو يُعيد القائمَ إن كان المفتاح مستعمَلاً.
     *
     * ⚠ الحارسُ من الازدواج هو **الفهرس الفريد** لا فحصٌ في الشيفرة (البند
     * 24): ضغطتان متسارعتان تمرّان معاً على أي `EXISTS`، أمّا الإدراجُ
     * فينجح مرّةً واحدة والثانية تصطدم فتقرأ الأولى.
     *
     * @return array{request:object, duplicate:bool}
     */
    public function open(
        object $employee,
        object $session,
        string $clientId,
        array $payload,
        float $amount,
        ?string $recipientName,
        ?string $recipientPhone,
        array $reasons,
        object $policy,
        float $consumed,
    ): array {
        $ttl = max(1, (int) $policy->approval_ttl_hours);

        /* لقطةُ السياسة وقتَ الطلب — تُقرأ بعد سنة فتشرح سببَ التصعيد. */
        $snapshot = json_encode([
            'per_transfer_limit' => $policy->per_transfer_limit,
            'cumulative_limit'   => $policy->cumulative_limit,
            'cumulative_hours'   => $policy->cumulative_hours,
            'recipient_minutes'  => $policy->recipient_minutes,
            'consumed_at_request' => round($consumed, 3),
        ], JSON_UNESCAPED_UNICODE);

        $posName = null;
        if (($session->active_pos_id ?? null) !== null) {
            $posName = DB::table('AuthorizedUsers')
                ->where('ID', $session->active_pos_id)
                ->value('UserName');
        }

        $row = [
            'agent_id'           => (int) $employee->agent_id,
            'employee_id'        => (int) $employee->id,
            'point_of_sale_id'   => $session->active_pos_id ?? null,
            'session_id'         => $session->id ?? null,
            'device_hash'        => $session->device_hash ?? null,
            'client_id'          => mb_substr($clientId, 0, 64),
            'payload'            => json_encode($payload, JSON_UNESCAPED_UNICODE),
            'amount'             => $amount,
            'currency_code'      => $payload['currency_code'] ?? null,
            'recipient_name'     => $recipientName,
            'recipient_name_norm' => EmployeeLimitPolicy::normalizeName($recipientName),
            'recipient_phone'    => $recipientPhone,
            'reasons'            => implode(',', $reasons),
            'policy_snapshot'    => $snapshot,
            'employee_name_snap' => $employee->full_name ?? null,
            'pos_name_snap'      => $posName,
            'status'             => 'PENDING',
            'expires_at'         => now()->addHours($ttl),
            'created_at'         => now(),
            'updated_at'         => now(),
        ];

        try {
            $id = DB::table('employee_approval_requests')->insertGetId($row);

            $this->log->audit('EMPLOYEE_APPROVAL_REQUESTED', [
                'agent_id'    => (int) $employee->agent_id,
                'employee_id' => (int) $employee->id,
                'entity_type' => 'employee_approval_request',
                'entity_id'   => (string) $id,
            ]);

            return [
                'request'   => $this->find((int) $id),
                'duplicate' => false,
            ];
        } catch (\Throwable) {
            /* الاصطدامُ بالفهرس هو الحالةُ المقصودة — يُعاد الطلبُ الأوّل. */
            $existing = DB::table('employee_approval_requests')
                ->where('employee_id', $employee->id)
                ->where('client_id', mb_substr($clientId, 0, 64))
                ->first();

            return ['request' => $existing, 'duplicate' => true];
        }
    }

    public function find(int $id): ?object
    {
        return DB::table('employee_approval_requests')->where('id', $id)->first();
    }

    /**
     * يُنهي الطلباتِ التي مضت مدّتُها — بلا تنفيذ (البند 27 و28).
     *
     * ⚠ يُنادى عند القراءة، لا من مُجدوِلٍ زمنيّ. فالمشروعُ بلا عاملِ طوابير
     * يعمل دائماً، ومهمّةٌ مجدولةٌ لا تعمل هي أسوأ من عدمها: طلبٌ يظهر
     * «بانتظار الموافقة» بعد أسبوع من انتهائه.
     *
     * وهو رخيص: تحديثٌ واحدٌ بشرطٍ على فهرس، ولا يمسّ إلّا ما انتهى فعلاً.
     */
    public function expireDue(int $agentId): int
    {
        return DB::table('employee_approval_requests')
            ->where('agent_id', $agentId)
            ->where('status', 'PENDING')
            ->whereNotNull('expires_at')
            ->where('expires_at', '<', now())
            ->update([
                'status'     => 'EXPIRED',
                'updated_at' => now(),
            ]);
    }

    /**
     * قرارُ الوكيل — **ذرّيّ** (البند 26).
     *
     * ⚠ الشرطُ `status = PENDING` داخل `UPDATE` نفسِه هو ما يمنع قرارين
     * متضاربين، لا قراءةٌ قبله. طلبان متزامنان (موافقةٌ ورفض، أو ضغطتان على
     * «موافقة») يتسلسلان على قفل الصفّ، فيُعيد الثاني تقييمَ الشرط على
     * القيمة المُثبَتة فيصيب صفراً من الصفوف.
     *
     * فمن أصاب صفّاً هو صاحبُ القرار، ومن أصاب صفراً يُعاد له وضعُ الطلب
     * الحاليّ — لا رسالةُ خطأ: الوكيل ضغط مرّتين ونجحت واحدة.
     *
     * @return array{changed:bool, request:?object}
     */
    public function decide(
        int $requestId,
        int $agentId,
        int $decidedBy,
        bool $approve,
        ?string $note = null,
    ): array {
        $changed = DB::table('employee_approval_requests')
            ->where('id', $requestId)
            // ⚠ عزلُ الوكيل في شرط التحديث نفسِه (البند 31)، لا في قراءةٍ
            // سابقة: وكيلٌ آخر لا يستطيع تغيير صفّاً ليس له مهما أرسل.
            ->where('agent_id', $agentId)
            ->where('status', 'PENDING')
            ->update([
                'status'        => $approve ? 'APPROVED' : 'REJECTED',
                'decided_by'    => $decidedBy,
                'decided_at'    => now(),
                'decision_note' => $note,
                'updated_at'    => now(),
            ]) > 0;

        $request = $this->find($requestId);

        if ($changed && $request) {
            $this->log->audit(
                $approve ? 'EMPLOYEE_APPROVAL_APPROVED' : 'EMPLOYEE_APPROVAL_REJECTED',
                [
                    'agent_id'    => $agentId,
                    'employee_id' => (int) $request->employee_id,
                    'entity_type' => 'employee_approval_request',
                    'entity_id'   => (string) $requestId,
                ],
            );
        }

        return ['changed' => $changed, 'request' => $request];
    }

    /** إلغاءُ الموظف لطلبه قبل القرار (البند 29) — ذرّيٌّ كالقرار. */
    public function cancel(int $requestId, int $employeeId): array
    {
        $changed = DB::table('employee_approval_requests')
            ->where('id', $requestId)
            // ⚠ صاحبُ الطلب وحده — لا موظّفٌ آخر ولو تحت الوكيل نفسِه.
            ->where('employee_id', $employeeId)
            ->where('status', 'PENDING')
            ->update([
                'status'     => 'CANCELLED',
                'updated_at' => now(),
            ]) > 0;

        if ($changed) {
            $this->log->audit('EMPLOYEE_APPROVAL_CANCELLED', [
                'employee_id' => $employeeId,
                'entity_type' => 'employee_approval_request',
                'entity_id'   => (string) $requestId,
            ]);
        }

        return ['changed' => $changed, 'request' => $this->find($requestId)];
    }

    /** يُثبّت نتيجةَ التنفيذ بعد الموافقة. */
    public function markExecuted(int $requestId, ?string $transferNumber, ?string $failure): void
    {
        DB::table('employee_approval_requests')->where('id', $requestId)->update([
            'status'          => $transferNumber !== null ? 'APPROVED' : 'FAILED',
            'transfer_number' => $transferNumber,
            'failure_reason'  => $failure !== null ? mb_substr($failure, 0, 500) : null,
            'executed_at'     => now(),
            'updated_at'      => now(),
        ]);
    }

    /** صفٌّ جاهزٌ للعرض — بأسبابٍ مقروءة ولقطةِ سياسة مفكوكة. */
    public static function present(object $r): array
    {
        $snap = json_decode($r->policy_snapshot ?? '{}', true) ?: [];

        return [
            'id'              => (int) $r->id,
            'employee_id'     => (int) $r->employee_id,
            'employee_name'   => $r->employee_name_snap,
            'point_of_sale'   => $r->pos_name_snap,
            'amount'          => (float) $r->amount,
            'currency_code'   => $r->currency_code,
            'recipient_name'  => $r->recipient_name,
            'recipient_phone' => $r->recipient_phone,
            'reasons'         => array_values(array_filter(explode(',', $r->reasons))),
            'reason_labels'   => EmployeeLimitPolicy::reasonLabels($r->reasons),
            // ⚠ السقفُ المعروض هو **سقفُ يومِ الطلب** لا سقفُ اليوم.
            'limit_at_request' => $snap['per_transfer_limit'] ?? null,
            'cumulative_limit_at_request' => $snap['cumulative_limit'] ?? null,
            'consumed_at_request' => $snap['consumed_at_request'] ?? null,
            'status'          => $r->status,
            'transfer_number' => $r->transfer_number,
            'failure_reason'  => $r->failure_reason,
            'decision_note'   => $r->decision_note,
            'created_at'      => $r->created_at,
            'expires_at'      => $r->expires_at,
            'decided_at'      => $r->decided_at,
        ];
    }
}
