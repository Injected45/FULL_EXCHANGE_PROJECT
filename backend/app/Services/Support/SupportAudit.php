<?php

namespace App\Services\Support;

use Illuminate\Support\Facades\DB;

/**
 * سجلّ نشاط مركز الدعم — يُضاف إليه ولا يُحذف منه ولا يُعدَّل فيه.
 *
 * وهو ما يجعل «من أغلق محادثة هذا الوكيل؟» و«من سحب هذه الصلاحية؟»
 * سؤالين لهما جواب بعد شهر. واسمُ الموظّف يُنسخ وقتَ الفعل لا يُقرأ
 * وقتَ العرض: الأسماء تتغيّر، والسجلُّ يقول ما كان يومَها.
 *
 * والكتابةُ لا ترمي استثناءً أبداً — سطرُ سجلٍّ فشل لا يجوز أن يُسقط
 * العمليةَ التي كان يصفها.
 */
class SupportAudit
{
    public const LOGIN            = 'LOGIN';
    public const LOGOUT           = 'LOGOUT';
    public const REPLY            = 'REPLY';
    public const ASSIGN           = 'ASSIGN';
    public const UNASSIGN         = 'UNASSIGN';
    public const STATUS           = 'STATUS';
    public const CLOSE            = 'CLOSE';
    public const REOPEN           = 'REOPEN';
    public const STAFF_CREATE     = 'STAFF_CREATE';
    public const STAFF_UPDATE     = 'STAFF_UPDATE';
    public const STAFF_DISABLE    = 'STAFF_DISABLE';
    public const STAFF_ENABLE     = 'STAFF_ENABLE';
    public const STAFF_DELETE     = 'STAFF_DELETE';
    public const STAFF_RESET_PASS = 'STAFF_RESET_PASS';
    public const PERM_GRANT       = 'PERM_GRANT';
    public const PERM_REVOKE      = 'PERM_REVOKE';
    public const PIN              = 'PIN';
    public const FORWARD          = 'FORWARD';
    public const EDIT             = 'EDIT';

    public function log(
        ?object $staff,
        string $action,
        ?int $threadId = null,
        ?string $target = null,
        ?string $detail = null,
        ?string $ip = null,
    ): void {
        try {
            DB::table('support_audit')->insert([
                'staff_id'   => $staff->id ?? null,
                'staff_name' => mb_substr((string) ($staff->name ?? 'غير معروف'), 0, 120),
                'action'     => $action,
                'thread_id'  => $threadId,
                'target'     => $target ? mb_substr($target, 0, 60) : null,
                'detail'     => $detail ? mb_substr($detail, 0, 500) : null,
                'ip'         => $ip ? mb_substr($ip, 0, 45) : null,
            ]);
        } catch (\Throwable) {
            // متعمَّد: سجلٌّ لا يُسقط عملية.
        }
    }

    /** آخر ما جرى — لشاشة السجلّ. */
    public function recent(int $limit = 200, ?int $threadId = null): array
    {
        $q = DB::table('support_audit')->orderByDesc('id')->limit($limit);

        if ($threadId !== null) {
            $q->where('thread_id', $threadId);
        }

        return $q->get()->map(fn ($r) => [
            'id'         => (int) $r->id,
            'staff_name' => $r->staff_name,
            'action'     => $r->action,
            'thread_id'  => $r->thread_id ? (int) $r->thread_id : null,
            'target'     => $r->target,
            'detail'     => $r->detail,
            'created_at' => (string) $r->created_at,
        ])->all();
    }
}
