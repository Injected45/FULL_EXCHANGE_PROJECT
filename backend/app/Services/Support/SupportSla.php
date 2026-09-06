<?php

namespace App\Services\Support;

use Illuminate\Support\Facades\DB;

/**
 * زمن الاستجابة — البند 2.
 *
 * ── لماذا يُحسب عند القراءة لا بمهمّةٍ دوريّة ─────────────────────────────
 *
 * الحالةُ دالّةٌ في **الوقت الحالي**: محادثةٌ سليمة الآن تصير متأخّرةً بعد
 * دقيقة بلا أن يفعل أحدٌ شيئاً. ومهمّةٌ دوريّة تكتب الحالةَ في عمود تعني
 * أن الشاشة تعرض ما كان قبل دقيقةٍ أو خمس — والفرقُ في الدعم هو الفرق بين
 * «تدارَكها» و«فاتت».
 *
 * والحسابُ رخيص: طرحُ تاريخين لصفوفٍ **مقروءةٍ أصلاً** لعرض القائمة. لا
 * استعلامَ إضافي ولا رحلةَ إلى القاعدة.
 *
 * ── ثلاثة أزمنة، وثلاثة أسئلةٍ مختلفة ────────────────────────────────────
 *
 * | الزمن | السؤال | يتوقّف عند |
 * |---|---|---|
 * | أوّل ردّ | كم انتظر الوكيلُ **أوّل** إشارةٍ بشرية؟ | أوّل ردٍّ من الدعم |
 * | الردّ التالي | كم ينتظر **الآن** بعد رسالته الأخيرة؟ | ردٍّ بعدها |
 * | المعالجة | كم استغرقت الحالةُ كلُّها؟ | الإغلاق |
 *
 * وأوّلُها وحده لا يكفي: فريقٌ يردّ في دقيقةٍ ثم يصمت يوماً يبدو ممتازاً
 * في مقياس «أوّل ردّ» وحده.
 *
 * ⚠ ولا شيء هنا يمسّ المال: تواريخُ وعدُّ دقائق.
 */
class SupportSla
{
    public const OK       = 'OK';
    public const WARNING  = 'WARNING';
    public const BREACHED = 'BREACHED';
    public const NONE     = 'NONE';

    public const LABELS = [
        self::OK       => 'ضمن الوقت',
        self::WARNING  => 'يقترب من التأخير',
        self::BREACHED => 'متأخّرة',
        self::NONE     => 'لا هدف',
    ];

    public const COLORS = [
        self::OK       => '#1F8A5F',
        self::WARNING  => '#D98324',
        self::BREACHED => '#C0392B',
        self::NONE     => '#6B7280',
    ];

    /**
     * عتبةُ التحذير: عند بلوغ 75% من المهلة.
     *
     * لا 90%: تحذيرٌ قبل التأخير بدقيقةٍ لا يُتيح تدارُكاً — والغرضُ من
     * التحذير أن يُفعَل شيء، لا أن يُعلَن ما فات.
     */
    private const WARN_AT = 0.75;

    /** @var array<string, array{first:?int,next:?int,resolve:?int}>|null */
    private ?array $cache = null;

    /**
     * الإعدادات — تُقرأ مرّةً لكل طلب.
     *
     * الجدولُ أربعةُ صفوف، وقراءتُه لكلّ محادثةٍ في قائمةٍ من ثلاثمئة تعني
     * ثلاثمئة رحلة. والذاكرةُ هنا داخل الطلب وحده — فتعديلُ الإدارة يسري
     * في الطلب التالي مباشرةً، ولا يحتاج مسحَ ذاكرةٍ ولا إعادةَ تشغيل.
     */
    public function settings(): array
    {
        if ($this->cache !== null) {
            return $this->cache;
        }

        $out = [];
        foreach (DB::table('support_sla')->get() as $r) {
            $out[$r->priority] = [
                'first'   => $r->first_response_min ? (int) $r->first_response_min : null,
                'next'    => $r->next_response_min ? (int) $r->next_response_min : null,
                'resolve' => $r->resolution_min ? (int) $r->resolution_min : null,
            ];
        }

        return $this->cache = $out;
    }

    /** للعرض في شاشة الإعدادات. */
    public function settingsForDisplay(): array
    {
        $s = $this->settings();
        $out = [];
        foreach (SupportOps::PRIORITIES as $k => $meta) {
            $out[] = [
                'priority'       => $k,
                'label'          => $meta['label'],
                'color'          => $meta['color'],
                'first_minutes'  => $s[$k]['first'] ?? null,
                'next_minutes'   => $s[$k]['next'] ?? null,
                'resolve_minutes' => $s[$k]['resolve'] ?? null,
            ];
        }
        return $out;
    }

    public function updateSettings(string $priority, array $vals, object $by): array
    {
        if (!SupportOps::priorityExists($priority)) {
            return ['error' => 'أولوية غير معروفة.'];
        }

        $clean = [];
        foreach (['first_response_min' => 'first', 'next_response_min' => 'next',
                  'resolution_min' => 'resolve'] as $col => $key) {
            if (!array_key_exists($key, $vals)) {
                continue;
            }
            $v = $vals[$key];
            if ($v === null || $v === '') {
                $clean[$col] = null;   // «لا هدف»
                continue;
            }
            $n = (int) $v;
            // حدٌّ أعلى معقول: ثلاثون يوماً. رقمٌ أكبر خطأُ إدخالٍ لا نيّة،
            // وقبولُه يجعل كلَّ حالةٍ «ضمن الوقت» إلى الأبد.
            if ($n < 1 || $n > 43200) {
                return ['error' => 'المهلة بالدقائق: بين 1 و43200 (ثلاثون يوماً).'];
            }
            $clean[$col] = $n;
        }

        if ($clean === []) {
            return ['ok' => true];
        }

        $clean['updated_at'] = now();
        $clean['updated_by'] = $by->id;

        DB::table('support_sla')->where('priority', $priority)->update($clean);
        $this->cache = null;

        return ['ok' => true];
    }

    /**
     * حالةُ محادثةٍ واحدة — من صفٍّ **مقروءٍ أصلاً**.
     *
     * @param object $row صفٌّ يحمل: priority, status, first_agent_msg_at,
     *                    first_reply_at, last_agent_msg_at, last_reply_at,
     *                    resolved_at
     * @return array{state:string,label:string,color:string,kind:?string,
     *               due_at:?string,remaining_min:?int,elapsed_min:?int}
     */
    public function evaluate(object $row, ?\DateTimeInterface $now = null): array
    {
        $now = $now ? \Carbon\Carbon::instance($now) : now();
        $cfg = $this->settings()[$row->priority ?? SupportOps::NORMAL] ?? [];

        /*
         * الترتيبُ مقصود: أوّلُ ردٍّ أولاً، فإن تمّ فالردُّ التالي.
         *
         * ومحادثةٌ **مغلقة** لا مهلةَ عليها: أغلقها من أغلقها، ومطالبتُه
         * بردٍّ على حالةٍ منتهية عبثٌ يملأ الشاشة بأحمرَ لا معنى له.
         */
        if (($row->status ?? '') === SupportThreadService::CLOSED) {
            return $this->none();
        }

        // ١) أوّل ردّ — ما دام لم يردّ أحدٌ بعد رسالة الوكيل الأولى.
        $firstAgent = $row->first_agent_msg_at ?? null;
        $firstReply = $row->first_reply_at ?? null;
        if ($firstAgent && !$firstReply && ($cfg['first'] ?? null)) {
            return $this->measure('FIRST', 'أوّل ردّ', $firstAgent, $cfg['first'], $now);
        }

        // ٢) الردُّ التالي — رسالةُ وكيلٍ بعد آخر ردّ.
        $lastAgent = $row->last_agent_msg_at ?? null;
        $lastReply = $row->last_reply_at ?? null;
        $awaiting = $lastAgent && (!$lastReply || strtotime((string) $lastAgent) > strtotime((string) $lastReply));
        if ($awaiting && ($cfg['next'] ?? null)) {
            return $this->measure('NEXT', 'الردّ التالي', $lastAgent, $cfg['next'], $now);
        }

        // ٣) المعالجة — الحالةُ مفتوحةٌ ولا ردَّ منتظَراً من الدعم.
        if ($firstAgent && ($cfg['resolve'] ?? null)) {
            return $this->measure('RESOLVE', 'المعالجة', $firstAgent, $cfg['resolve'], $now);
        }

        return $this->none();
    }

    private function measure(string $kind, string $kindLabel, $since, int $minutes, $now): array
    {
        $start   = \Carbon\Carbon::parse((string) $since);
        $due     = $start->copy()->addMinutes($minutes);
        $elapsed = $start->diffInMinutes($now, false);
        $remain  = $now->diffInMinutes($due, false);

        $state = $remain < 0
            ? self::BREACHED
            : ($elapsed >= $minutes * self::WARN_AT ? self::WARNING : self::OK);

        return [
            'state'         => $state,
            'label'         => self::LABELS[$state],
            'color'         => self::COLORS[$state],
            'kind'          => $kind,
            'kind_label'    => $kindLabel,
            'due_at'        => $due->toIso8601String(),
            'remaining_min' => (int) $remain,
            'elapsed_min'   => (int) $elapsed,
            'target_min'    => $minutes,
        ];
    }

    private function none(): array
    {
        return [
            'state' => self::NONE, 'label' => self::LABELS[self::NONE],
            'color' => self::COLORS[self::NONE], 'kind' => null, 'kind_label' => null,
            'due_at' => null, 'remaining_min' => null, 'elapsed_min' => null,
            'target_min' => null,
        ];
    }

    // ══════════════════════════════════════════════════════════════════
    //  تسجيل الأزمنة — تُنادى من مساري الوكيل والدعم
    // ══════════════════════════════════════════════════════════════════

    /**
     * رسالةٌ من الوكيل وصلت.
     *
     * `first_agent_msg_at` تُكتب **مرّةً واحدة** ولا تُحدَّث: هي بدايةُ عدّاد
     * «أوّل ردّ»، وتحديثُها مع كل رسالةٍ يُصفّر العدّاد فيبدو الفريق سريعاً
     * لأن الوكيل يكرّر السؤال.
     *
     * ولا ترمي شيئاً: قياسٌ فوق الدردشة، وفشلُه لا يمنع رسالة.
     */
    public function onAgentMessage(int $threadId): void
    {
        try {
            DB::table('support_thread_state')->where('thread_id', $threadId)->update([
                'last_agent_msg_at'  => now(),
                'first_agent_msg_at' => DB::raw('ISNULL(first_agent_msg_at, SYSDATETIME())'),
                'updated_at'         => now(),
            ]);
        } catch (\Throwable) {
        }
    }

    /** ردٌّ من الدعم — والملاحظةُ الداخلية ليست ردّاً فلا تُنادي هذه. */
    public function onSupportReply(int $threadId): void
    {
        try {
            DB::table('support_thread_state')->where('thread_id', $threadId)->update([
                'last_reply_at'  => now(),
                'first_reply_at' => DB::raw('ISNULL(first_reply_at, SYSDATETIME())'),
                'updated_at'     => now(),
            ]);
        } catch (\Throwable) {
        }
    }

    public function onResolved(int $threadId): void
    {
        try {
            DB::table('support_thread_state')->where('thread_id', $threadId)
                ->update(['resolved_at' => now(), 'updated_at' => now()]);
        } catch (\Throwable) {
        }
    }

    /** إعادةُ الفتح تمسح زمنَ الحلّ — حالةٌ عادت ليست محلولة. */
    public function onReopened(int $threadId): void
    {
        try {
            DB::table('support_thread_state')->where('thread_id', $threadId)
                ->update(['resolved_at' => null, 'updated_at' => now()]);
        } catch (\Throwable) {
        }
    }
}
