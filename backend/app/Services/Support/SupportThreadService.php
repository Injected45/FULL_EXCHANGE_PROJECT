<?php

namespace App\Services\Support;

use App\Services\ChatService;
use Illuminate\Support\Facades\DB;

/**
 * إسنادُ محادثات الدعم وحالاتُها — وهو كلُّ ما يضيفه المركز فوق الدردشة.
 *
 * الرسائلُ نفسها لا تمرّ من هنا: تُقرأ وتُرسَل بـ {@see ChatService} بلا
 * سطرٍ مكرَّر. ما يُضاف هو الجوابُ عن سؤالين لا يجيب عنهما نظام الدردشة
 * ولا يجب أن يجيب: **من يعمل على هذه المحادثة؟** و**أين وصلت؟**
 *
 * ── الحالات الأربع ───────────────────────────────────────────────────────
 *
 * | الحالة | معناها |
 * |---|---|
 * | `NEW`     | وردت ولم يستلمها أحد |
 * | `OPEN`    | مُسنَدة ويُعمل عليها |
 * | `PENDING` | الدعم ردّ وينتظر جواب الوكيل |
 * | `CLOSED`  | أُغلقت |
 *
 * ولا تُخترع خامسة: كلُّ حالةٍ إضافية تُقسّم شاشةَ الموظّف بلا أن تغيّر ما
 * يفعله.
 *
 * ── قرارٌ يسهل إغفاله ────────────────────────────────────────────────────
 *
 * **رسالةٌ من الوكيل تُعيد المحادثة إلى الحياة.** محادثةٌ أُغلقت ثم كتب
 * فيها الوكيل ليست مغلقة — وإبقاؤها كذلك يعني أن أحداً لن يراها في
 * «المفتوحة» ولا في «المُسنَدة إليّ»، فتُنسى رسالتُه إلى الأبد. ولهذا
 * `onAgentMessage` تُنادى من مسار إرسال الوكيل: تُعيد `CLOSED` و`PENDING`
 * إلى `OPEN` إن كان لها مالك، وإلى `NEW` إن لم يكن.
 *
 * ⚠ ولا شيء هنا يمسّ المال: إسنادٌ وحالةٌ ونصّ. لا رصيد، ولا حوالة، ولا
 * قيد، ولا استعلامٌ عن جدولٍ ماليّ واحد.
 */
class SupportThreadService
{
    public const NEW     = 'NEW';
    public const OPEN    = 'OPEN';
    public const PENDING = 'PENDING';
    public const CLOSED  = 'CLOSED';

    public const STATUSES = [
        self::NEW     => 'جديدة',
        self::OPEN    => 'مفتوحة',
        self::PENDING => 'بانتظار الوكيل',
        self::CLOSED  => 'مغلقة',
    ];

    public function __construct(private ChatService $chat)
    {
    }

    public static function statusExists(string $s): bool
    {
        return array_key_exists($s, self::STATUSES);
    }

    /**
     * قائمة محادثات الوكلاء مع الإدارة.
     *
     * ⚠ محادثةُ الوكيل مع موظّفه لا تظهر هنا أبداً — لا بفلتر ولا بصلاحية.
     * الشرط `kind = ADMIN` في الاستعلام نفسه لا في طبقةٍ فوقه: شرطٌ يُنسى
     * إضافتُه مرّةً يكشف مراسلاتِ وكيلٍ مع موظّفه لموظّف دعمٍ لا شأن له بها.
     *
     * والاستعلامُ مجموعيّ لا صفّيّ: لا استعلامَ فرعيَّ لكلّ محادثة — ذلك هو
     * الشكلُ الذي كلّف كشفَ الحساب ثمانيَ وستّين ثانية.
     *
     * @param array{scope?:string,status?:string,q?:string} $f
     */
    public function threads(object $staff, array $f = []): array
    {
        $scope  = $f['scope']  ?? 'all';
        $status = $f['status'] ?? '';
        $term   = trim((string) ($f['q'] ?? ''));

        $canSeeAll = in_array('VIEW_ALL_THREADS', $staff->permissions ?? [], true);

        $q = DB::table('chat_threads as t')
            ->leftJoin('users as u', 'u.id', '=', 't.agent_id')
            ->leftJoin('support_thread_state as s', 's.thread_id', '=', 't.id')
            ->leftJoin('support_staff as a', 'a.id', '=', 's.assigned_to')
            ->where('t.kind', ChatService::ADMIN);

        // من لا يملك «عرض كل المحادثات» يرى المُسنَدة إليه وغيرَ المُسنَدة.
        //
        // وغيرُ المُسنَدة تُعرض له عمداً: لو رأى المُسنَدةَ إليه وحدها لما
        // استطاع أن يستلم شيئاً، ولانتظرت محادثاتُ الوكلاء مشرفاً يوزّعها.
        if (!$canSeeAll) {
            $q->where(function ($w) use ($staff) {
                $w->where('s.assigned_to', $staff->id)->orWhereNull('s.assigned_to');
            });
        }

        match ($scope) {
            'mine'       => $q->where('s.assigned_to', $staff->id),
            'unassigned' => $q->whereNull('s.assigned_to'),
            default      => null,
        };

        if ($status !== '' && self::statusExists($status)) {
            // `NEW` هي أيضاً حالُ محادثةٍ بلا صفٍّ أصلاً.
            $status === self::NEW
                ? $q->where(fn ($w) => $w->where('s.status', self::NEW)->orWhereNull('s.status'))
                : $q->where('s.status', $status);
        } elseif ($status === '' && $scope === 'all' && empty($f['include_closed'])) {
            // الافتراضي يُخفي المغلقة: صندوقُ الوارد هو ما يحتاج عملاً،
            // والمغلقةُ تُطلب بفلترها حين تُطلب.
            //
            // و`include_closed` للبحث: من يبحث عن كلمةٍ قالها وكيلٌ الشهرَ
            // الماضي يريدها ولو أُغلقت محادثتُه — بل غالباً لأنها أُغلقت.
            $q->where(fn ($w) => $w->where('s.status', '<>', self::CLOSED)->orWhereNull('s.status'));
        }

        if ($term !== '') {
            $like = '%' . str_replace(['[', '%', '_'], ['[[]', '[%]', '[_]'], $term) . '%';
            $q->where(fn ($w) => $w->where('u.name', 'like', $like)
                                   ->orWhere('u.phone', 'like', $like));
        }

        $rows = $q->orderByRaw('CASE WHEN t.last_message_at IS NULL THEN 1 ELSE 0 END')
            ->orderByDesc('t.last_message_at')
            ->limit(300)
            ->get([
                't.id', 't.agent_id', 't.last_message_at',
                'u.name as agent_name', 'u.phone as agent_phone',
                's.status', 's.assigned_to', 's.assigned_at',
                'a.name as assignee_name',
            ]);

        $ids = $rows->pluck('id')->map(fn ($v) => (int) $v)->all();
        if ($ids === []) {
            return [];
        }

        // غيرُ المقروء وآخرُ رسالة: استعلامان للصفحة كلّها، لا لكلّ صفّ.
        $unread = $this->chat->unreadByThread($ids, ChatService::ADMIN, 0);
        $last   = $this->lastMessages($ids);

        return $rows->map(function ($r) use ($unread, $last) {
            $tid = (int) $r->id;
            $lm  = $last[$tid] ?? null;

            return [
                'id'            => $tid,
                'agent_id'      => (int) $r->agent_id,
                'agent_name'    => $r->agent_name ?: 'وكيل #' . $r->agent_id,
                'agent_phone'   => $r->agent_phone,
                'last_message_at' => $r->last_message_at ? (string) $r->last_message_at : null,
                'last_body'     => $lm['body'] ?? '',
                'last_from'     => $lm['from'] ?? null,
                'unread'        => $unread[$tid] ?? 0,
                'status'        => $r->status ?: self::NEW,
                'status_label'  => self::STATUSES[$r->status ?: self::NEW],
                'assigned_to'   => $r->assigned_to ? (int) $r->assigned_to : null,
                'assignee_name' => $r->assignee_name,
                'assigned_at'   => $r->assigned_at ? (string) $r->assigned_at : null,
            ];
        })->all();
    }

    /**
     * آخر رسالةٍ في كلّ محادثة — استعلامان مجموعيّان لا استعلامٌ لكلّ صفّ.
     *
     * @param  int[] $ids
     * @return array<int, array{body:string, from:string}>
     */
    private function lastMessages(array $ids): array
    {
        // `selectRaw` ثم `get`، لا `pluck` على تعبيرٍ خام: `pluck` تقرأ
        // النتيجة بالاسم، واسمُ العمود يصير حرفياً `MAX(id)` فتفشل.
        $maxIds = DB::table('chat_messages')
            ->whereIn('thread_id', $ids)
            ->whereNull('deleted_at')
            ->groupBy('thread_id')
            ->selectRaw('thread_id, MAX(id) AS mid')
            ->get();

        $msgIds = $maxIds->map(fn ($r) => (int) $r->mid)->all();
        if ($msgIds === []) {
            return [];
        }

        $rows = DB::table('chat_messages')
            ->whereIn('id', $msgIds)
            ->get(['id', 'thread_id', 'body', 'sender_kind', 'attachment_kind']);

        $out = [];
        foreach ($rows as $r) {
            $body = (string) $r->body;
            if ($body === '' && $r->attachment_kind) {
                $body = match ($r->attachment_kind) {
                    'IMAGE' => '📷 صورة',
                    'AUDIO' => '🎤 رسالة صوتية',
                    default => '📎 ملفّ',
                };
            }
            $out[(int) $r->thread_id] = ['body' => $body, 'from' => (string) $r->sender_kind];
        }

        return $out;
    }

    /** يضمن وجود صفّ الحالة، ويُرجعه. */
    public function ensureState(int $threadId): object
    {
        $row = DB::table('support_thread_state')->where('thread_id', $threadId)->first();

        if (!$row) {
            DB::table('support_thread_state')->insert([
                'thread_id' => $threadId,
                'status'    => self::NEW,
            ]);
            $row = DB::table('support_thread_state')->where('thread_id', $threadId)->first();
        }

        return $row;
    }

    /**
     * إسناد المحادثة — أو نزعُها بتمرير `null`.
     *
     * والاستلامُ ينقل `NEW` إلى `OPEN` تلقائياً: محادثةٌ لها مالكٌ وما زالت
     * «جديدة» تناقضُ نفسها، وترك ذلك للموظّف خطوةٌ ثانية سيُهملها.
     */
    public function assign(int $threadId, ?int $toStaffId, object $by): array
    {
        $this->ensureState($threadId);

        if ($toStaffId !== null) {
            $target = DB::table('support_staff')->where('id', $toStaffId)
                ->whereNull('deleted_at')->where('is_active', 1)->first(['id', 'name']);

            if (!$target) {
                return ['error' => 'موظّف الدعم غير موجود أو موقوف.'];
            }
        }

        $cur = DB::table('support_thread_state')->where('thread_id', $threadId)->first();

        $update = [
            'assigned_to' => $toStaffId,
            'assigned_at' => $toStaffId === null ? null : now(),
            'assigned_by' => $toStaffId === null ? null : $by->id,
            'updated_at'  => now(),
        ];

        if ($toStaffId !== null && in_array($cur->status, [self::NEW, self::CLOSED], true)) {
            $update['status']    = self::OPEN;
            $update['closed_at'] = null;
            $update['closed_by'] = null;
        }

        DB::table('support_thread_state')->where('thread_id', $threadId)->update($update);

        return ['ok' => true, 'assignee' => $toStaffId === null ? null : ($target->name ?? null)];
    }

    public function setStatus(int $threadId, string $status, object $by, ?string $note = null): array
    {
        if (!self::statusExists($status)) {
            return ['error' => 'حالة غير معروفة.'];
        }

        $this->ensureState($threadId);

        $update = ['status' => $status, 'updated_at' => now()];

        if ($status === self::CLOSED) {
            $update['closed_at']  = now();
            $update['closed_by']  = $by->id;
            $update['close_note'] = $note ? mb_substr($note, 0, 500) : null;
        } else {
            $update['closed_at'] = null;
            $update['closed_by'] = null;
        }

        DB::table('support_thread_state')->where('thread_id', $threadId)->update($update);

        return ['ok' => true];
    }

    /**
     * رسالةٌ جديدة من الوكيل — تُعيد المحادثة إلى دائرة العمل.
     *
     * تُنادى من مسار إرسال الوكيل، ولا ترمي شيئاً أبداً: هذه إضافةٌ فوق
     * الدردشة، وفشلُها لا يجوز أن يمنع وصول رسالة.
     */
    public function onAgentMessage(int $threadId): void
    {
        try {
            $row = DB::table('support_thread_state')->where('thread_id', $threadId)->first();

            // لا صفَّ = محادثةٌ لم يلمسها الدعم بعد، وهي `NEW` أصلاً.
            if (!$row || !in_array($row->status, [self::PENDING, self::CLOSED], true)) {
                return;
            }

            DB::table('support_thread_state')->where('thread_id', $threadId)->update([
                'status'     => $row->assigned_to ? self::OPEN : self::NEW,
                'closed_at'  => null,
                'closed_by'  => null,
                'updated_at' => now(),
            ]);
        } catch (\Throwable) {
            // متعمَّد.
        }
    }

    /**
     * ردَّ الدعم — المحادثة تصير «بانتظار الوكيل».
     *
     * ولا تُغيَّر المغلقة: من أغلق ثم كتب سطرَ توديعٍ لم يُعِد فتحها.
     */
    public function onSupportReply(int $threadId, object $by): void
    {
        try {
            $row = $this->ensureState($threadId);

            if ($row->status === self::CLOSED) {
                return;
            }

            $update = ['status' => self::PENDING, 'updated_at' => now()];

            // من ردّ على محادثةٍ بلا مالكٍ صار مالكَها. لا يُترك ذلك لخطوةٍ
            // منفصلة: موظّفٌ ردّ ثم نسي أن يستلم يجعلها تظهر «بلا مالك»
            // لزميلٍ يردّ عليها مرّةً ثانية.
            if ($row->assigned_to === null) {
                $update['assigned_to'] = $by->id;
                $update['assigned_at'] = now();
                $update['assigned_by'] = $by->id;
            }

            DB::table('support_thread_state')->where('thread_id', $threadId)->update($update);
        } catch (\Throwable) {
            // متعمَّد.
        }
    }

    /** أرقامُ الشاشة العلوية. */
    public function stats(object $staff): array
    {
        // استعلامٌ خام: `groupByRaw` مع ارتباطٍ يضع المُعامل في غير موضعه
        // فتشتكي SQL Server أن العمود ليس في `GROUP BY`. والقيمةُ ثابتةٌ
        // من صنفٍ لا من طلب، فلا مدخلَ لحقنٍ هنا.
        $byStatus = collect(DB::select(
            "SELECT ISNULL(s.status, '" . self::NEW . "') AS st, COUNT(*) AS cnt
               FROM chat_threads t
               LEFT JOIN support_thread_state s ON s.thread_id = t.id
              WHERE t.kind = ?
              GROUP BY ISNULL(s.status, '" . self::NEW . "')",
            [ChatService::ADMIN]
        ))->pluck('cnt', 'st');

        $mine = DB::table('support_thread_state')
            ->where('assigned_to', $staff->id)
            ->where('status', '<>', self::CLOSED)
            ->count();

        return [
            'new'        => (int) ($byStatus[self::NEW] ?? 0),
            'open'       => (int) ($byStatus[self::OPEN] ?? 0),
            'pending'    => (int) ($byStatus[self::PENDING] ?? 0),
            'closed'     => (int) ($byStatus[self::CLOSED] ?? 0),
            'mine'       => $mine,
            'unassigned' => DB::table('chat_threads as t')
                ->leftJoin('support_thread_state as s', 's.thread_id', '=', 't.id')
                ->where('t.kind', ChatService::ADMIN)
                ->whereNull('s.assigned_to')
                ->count(),
        ];
    }
}
