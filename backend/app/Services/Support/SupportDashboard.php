<?php

namespace App\Services\Support;

use App\Services\ChatService;
use Illuminate\Support\Facades\DB;

/**
 * لوحة قيادة مركز الدعم — البند 1.
 *
 * ── ما تجيب عنه، ولماذا هذه الأرقام دون غيرها ────────────────────────────
 *
 * اللوحةُ تجيب عن سؤالٍ واحد: **ما الذي يحتاج تدخّلاً الآن؟** فكلُّ رقمٍ
 * فيها إمّا يدلّ على عملٍ ينتظر، أو على قدرةٍ متاحة لحمله. ورقمٌ لا يفعل
 * أحدُهما زينةٌ تُبعد العين عمّا يهمّ.
 *
 * ولذلك لا يوجد فيها «إجمالي المحادثات منذ البداية»: رقمٌ يكبر ولا ينقص
 * ولا يُطلب منه فعلٌ.
 *
 * ── وكيف تبقى رخيصة ──────────────────────────────────────────────────────
 *
 * ⚠ **أربعةُ استعلاماتٍ لكل اللوحة**، لا استعلامٌ لكل رقم. اللوحةُ تُقرأ
 * كلَّ بضع ثوانٍ من كل لسانٍ مفتوح، والقاعدةُ بعيدة — فعشرون رقماً بعشرين
 * رحلةً كانت ستكلّف ثانيةً كاملة في كل نبضة.
 *
 * والصفوفُ تُقرأ مرّةً وتُعدّ في PHP: الأعدادُ صغيرة (مئاتُ المحادثات لا
 * ملايين)، وحسابُ SLA يحتاج مقارنةَ تواريخَ لا يفعلها SQL بسهولة.
 *
 * ⚠ ولا شيء هنا يمسّ المال: عدُّ محادثاتٍ وقياسُ أزمنة.
 */
class SupportDashboard
{
    public function __construct(
        private ChatService $chat,
        private SupportSla $sla,
        private SupportPresence $presence,
    ) {
    }

    public function build(object $me): array
    {
        /* ①: كلُّ محادثات الإدارة بحالتها وأزمنتها — قراءةٌ واحدة. */
        $rows = DB::table('chat_threads as t')
            ->leftJoin('support_thread_state as s', 's.thread_id', '=', 't.id')
            ->where('t.kind', ChatService::ADMIN)
            ->get([
                't.id',
                DB::raw("ISNULL(s.status, 'NEW') AS status"),
                DB::raw("ISNULL(s.priority, 'NORMAL') AS priority"),
                's.assigned_to', 's.resolved_at',
                's.first_agent_msg_at', 's.first_reply_at',
                's.last_agent_msg_at', 's.last_reply_at',
                's.snoozed_until',
            ]);

        $today = now()->startOfDay();

        $counts = [
            'new' => 0, 'open' => 0, 'pending' => 0, 'closed' => 0,
            'unassigned' => 0, 'mine' => 0, 'snoozed' => 0,
            'awaiting_support' => 0, 'awaiting_agent' => 0,
            'resolved_today' => 0,
            'sla_ok' => 0, 'sla_warning' => 0, 'sla_breached' => 0,
        ];
        $breachedIds = [];

        foreach ($rows as $r) {
            $st = $r->status;
            $counts[strtolower($st)] = ($counts[strtolower($st)] ?? 0) + 1;

            if ($st !== SupportThreadService::CLOSED) {
                if ($r->assigned_to === null) $counts['unassigned']++;
                if ((int) ($r->assigned_to ?? 0) === (int) $me->id) $counts['mine']++;
                if ($r->snoozed_until && strtotime((string) $r->snoozed_until) > time()) {
                    $counts['snoozed']++;
                }

                /*
                 * «ينتظر الدعم» و«ينتظر الوكيل» — البند 1 يطلبهما منفصلين،
                 * وهما أهمُّ رقمين في اللوحة: الأوّل عملٌ علينا، والثاني
                 * انتظارٌ لا نملك تعجيله.
                 */
                $lastAgent = $r->last_agent_msg_at;
                $lastReply = $r->last_reply_at;
                $awaitingUs = $lastAgent
                    && (!$lastReply || strtotime((string) $lastAgent) > strtotime((string) $lastReply));

                if ($awaitingUs) $counts['awaiting_support']++;
                elseif ($st === SupportThreadService::PENDING) $counts['awaiting_agent']++;

                $ev = $this->sla->evaluate($r);
                if ($ev['state'] === SupportSla::BREACHED) {
                    $counts['sla_breached']++;
                    $breachedIds[] = (int) $r->id;
                } elseif ($ev['state'] === SupportSla::WARNING) {
                    $counts['sla_warning']++;
                } elseif ($ev['state'] === SupportSla::OK) {
                    $counts['sla_ok']++;
                }
            }

            if ($r->resolved_at && \Carbon\Carbon::parse((string) $r->resolved_at)->gte($today)) {
                $counts['resolved_today']++;
            }
        }

        /* ②: غير المقروء — استعلامٌ واحد مجموعيّ (النمط المقرَّر). */
        $unreadRows = DB::select(
            'SELECT m.thread_id AS tid, COUNT(*) AS n
               FROM chat_messages m
               JOIN chat_threads t ON t.id = m.thread_id AND t.kind = ?
               LEFT JOIN chat_reads r
                 ON r.thread_id = m.thread_id AND r.reader_kind = ? AND r.reader_id = 0
              WHERE m.sender_kind <> ? AND m.deleted_at IS NULL AND m.is_internal = 0
                AND m.id > ISNULL(r.last_read_message_id, 0)
              GROUP BY m.thread_id',
            [ChatService::ADMIN, ChatService::ADMIN, ChatService::ADMIN]
        );
        $unreadTotal   = 0;
        $unreadThreads = 0;
        foreach ($unreadRows as $u) {
            $unreadTotal += (int) $u->n;
            $unreadThreads++;
        }

        /* ③: الفريق وحمولته. */
        $team = $this->presence->team();
        $online = array_values(array_filter($team,
            fn ($s) => $s['presence'] !== SupportPresence::OFFLINE));

        /* ④: المتوسّطات — على المحادثات التي لها أزمنةٌ مكتملة. */
        $avg = $this->averages();

        return [
            'counts' => $counts + [
                'unread_messages' => $unreadTotal,
                'unread_threads'  => $unreadThreads,
                'staff_online'    => count($online),
                'staff_available' => count(array_filter($online,
                    fn ($s) => $s['presence'] === SupportPresence::AVAILABLE)),
                'staff_busy'      => count(array_filter($online,
                    fn ($s) => $s['presence'] === SupportPresence::BUSY)),
            ],
            'averages' => $avg,
            'team'     => $team,
            /*
             * ⚠ أرقامُ المتأخّرة تُرسَل لا عددُها وحده: الغرضُ من اللوحة أن
             * يُضغط على الرقم فتُفتح قائمتُها — ورقمٌ لا يقود إلى ما يصفه
             * يجعل الموظّف يبحث عنه بيده.
             */
            'breached_ids' => array_slice($breachedIds, 0, 50),
            'sla'          => $this->sla->settingsForDisplay(),
            'generated_at' => now()->toIso8601String(),
        ];
    }

    /**
     * المتوسّطات — بالدقائق.
     *
     * ⚠ على آخر ثلاثين يوماً لا على التاريخ كلِّه: متوسّطٌ يشمل سنةً مضت
     * لا يتحرّك مهما تحسّن الفريق أو ساء، فيصير رقماً يُعرض ولا يُقرأ.
     *
     * و`DATEDIFF` في الخادم لا في PHP: الحسابُ مجموعيّ على صفوفٍ لا تُنقل.
     */
    private function averages(): array
    {
        $since = now()->subDays(30);

        $row = DB::selectOne(
            "SELECT
               AVG(CASE WHEN first_reply_at IS NOT NULL AND first_agent_msg_at IS NOT NULL
                        THEN DATEDIFF(MINUTE, first_agent_msg_at, first_reply_at) END) AS first_avg,
               AVG(CASE WHEN last_reply_at IS NOT NULL AND last_agent_msg_at IS NOT NULL
                         AND last_reply_at > last_agent_msg_at
                        THEN DATEDIFF(MINUTE, last_agent_msg_at, last_reply_at) END) AS next_avg,
               AVG(CASE WHEN resolved_at IS NOT NULL AND first_agent_msg_at IS NOT NULL
                        THEN DATEDIFF(MINUTE, first_agent_msg_at, resolved_at) END) AS resolve_avg,
               COUNT(CASE WHEN first_reply_at IS NOT NULL THEN 1 END) AS answered
             FROM support_thread_state
            WHERE ISNULL(updated_at, SYSDATETIME()) >= ?",
            [$since]
        );

        return [
            'first_response_min' => $row->first_avg !== null ? (int) $row->first_avg : null,
            'next_response_min'  => $row->next_avg !== null ? (int) $row->next_avg : null,
            'resolution_min'     => $row->resolve_avg !== null ? (int) $row->resolve_avg : null,
            'answered_threads'   => (int) ($row->answered ?? 0),
            'window_days'        => 30,
        ];
    }
}
