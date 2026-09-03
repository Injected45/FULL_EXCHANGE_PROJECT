<?php

namespace App\Services\Employees;

use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Log;

/**
 * كتابة سجلّي التدقيق والأمن.
 *
 * فُصلا عمداً (بند 48): محاولةُ اختراق يجب أن تُقرأ في جدولٍ صغير، لا أن
 * تضيع بين آلاف صفوف «أنشأ موظفاً» و«عدّل صلاحية».
 *
 * ⚠ **فشل الكتابة لا يُسقط العملية.** سجلٌّ ضائع أهون من حوالةٍ تفشل لأن
 * جدول السجلّ ممتلئ — لكنه يُكتب في سجلّ Laravel كي لا يمرّ صامتاً.
 */
class EmployeeAuditLogger
{
    public function audit(string $action, array $ctx = []): void
    {
        try {
            DB::table('audit_logs')->insert([
                'actor_user_id'    => $ctx['actor_user_id'] ?? null,
                'actor_type'       => $ctx['actor_type'] ?? null,
                'agent_id'         => $ctx['agent_id'] ?? null,
                'employee_id'      => $ctx['employee_id'] ?? null,
                'point_of_sale_id' => $ctx['point_of_sale_id'] ?? null,
                'device_hash'      => $ctx['device_hash'] ?? null,
                'action'           => $action,
                'entity_type'      => $ctx['entity_type'] ?? null,
                'entity_id'        => isset($ctx['entity_id']) ? (string) $ctx['entity_id'] : null,
                'old_value'        => $this->text($ctx['old_value'] ?? null),
                'new_value'        => $this->text($ctx['new_value'] ?? null),
                'ip_address'       => $ctx['ip'] ?? null,
                'created_at'       => now(),
            ]);
        } catch (\Throwable $e) {
            Log::warning('audit_logs write failed', ['action' => $action, 'error' => $e->getMessage()]);
        }
    }

    public function security(string $eventType, string $detail, array $ctx = []): void
    {
        try {
            DB::table('security_logs')->insert([
                'event_type'  => $eventType,
                'severity'    => $ctx['severity'] ?? 'WARNING',
                'agent_id'    => $ctx['agent_id'] ?? null,
                'employee_id' => $ctx['employee_id'] ?? null,
                'phone'       => $ctx['phone'] ?? null,
                'device_hash' => $ctx['device_hash'] ?? null,
                'detail'      => $detail,
                'ip_address'  => $ctx['ip'] ?? null,
                'created_at'  => now(),
            ]);
        } catch (\Throwable $e) {
            Log::warning('security_logs write failed', ['event' => $eventType, 'error' => $e->getMessage()]);
        }
    }

    /** القيم المركّبة تُحفظ JSON، والنصّ كما هو — والعمود محدود الطول. */
    private function text($value): ?string
    {
        if ($value === null) {
            return null;
        }
        $s = is_scalar($value) ? (string) $value : json_encode($value, JSON_UNESCAPED_UNICODE);
        return mb_substr($s, 0, 1000);
    }
}
