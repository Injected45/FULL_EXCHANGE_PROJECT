<?php

namespace App\Services\Support;

use Illuminate\Support\Facades\DB;

/**
 * حضور موظّفي الدعم ومنع تعارض الردود.
 * (بندا المالك 6 و36)
 *
 * ── لماذا «غير متصل» لا تُخزَّن ──────────────────────────────────────────
 *
 * الحالاتُ المخزَّنة ثلاث: متاح، مشغول، استراحة. و«غير متصل» **تُحسب** من
 * آخر نشاط.
 *
 * وسببُه أن الحالة المخزَّنة تكذب: موظّفٌ يُغلق حاسوبه وينصرف يبقى «متاحاً»
 * إلى الغد، فيُسنَد إليه عملٌ لا أحد يراه — وهو أسوأ من ألّا يكون هناك
 * توزيعٌ أصلاً، لأن المحادثة تبدو مُعالَجة وليست كذلك.
 *
 * ── ومنعُ التعارض كشفٌ لا قفل ────────────────────────────────────────────
 *
 * نصُّ البند 6: «بدون منع المشرف من الدخول عند الحاجة». فالنظام يقول من
 * ينظر ومن يكتب، ولا يمنع أحداً. وقفلٌ صلب يعني محادثةَ وكيلٍ عالقةً لأن
 * موظّفاً نسي إغلاق لسانه ثم سافر.
 *
 * ⚠ ولا شيء هنا يمسّ المال.
 */
class SupportPresence
{
    public const AVAILABLE = 'AVAILABLE';
    public const BUSY      = 'BUSY';
    public const BREAK     = 'BREAK';
    public const OFFLINE   = 'OFFLINE';   // محسوبةٌ لا مخزَّنة

    public const LABELS = [
        self::AVAILABLE => 'متاح',
        self::BUSY      => 'مشغول',
        self::BREAK     => 'استراحة',
        self::OFFLINE   => 'غير متصل',
    ];

    public const COLORS = [
        self::AVAILABLE => '#1F8A5F',
        self::BUSY      => '#D98324',
        self::BREAK     => '#5A7D9A',
        self::OFFLINE   => '#9AA3A0',
    ];

    /**
     * بعد كم ثانيةٍ من الصمت يُعدّ الموظّف غير متصل.
     *
     * تسعون ثانية: نبضةُ اللوحة كل ثمانٍ، فثلاثُ نبضاتٍ ضائعة تعني انقطاعاً
     * حقيقياً لا تعثّراً في الشبكة. وقيمةٌ أقصر تجعل الموظّف يومض بين
     * «متصل» و«غير متصل» في شبكةٍ متذبذبة.
     */
    public const OFFLINE_AFTER_SEC = 90;

    /** كم يبقى «يشاهد الآن» بعد آخر نبضة. */
    public const VIEWER_TTL_SEC = 15;

    public static function exists(string $p): bool
    {
        return in_array($p, [self::AVAILABLE, self::BUSY, self::BREAK], true);
    }

    public function setPresence(int $staffId, string $presence): array
    {
        if (!self::exists($presence)) {
            return ['error' => 'حالة غير معروفة.'];
        }

        DB::table('support_staff')->where('id', $staffId)->update([
            'presence'    => $presence,
            'presence_at' => now(),
        ]);

        return ['ok' => true];
    }

    /**
     * الحالة الفعلية لموظّف — المخزَّنةُ ما لم يكن صامتاً.
     */
    public static function effective(?string $stored, $lastSeenAt): string
    {
        if (!$lastSeenAt) {
            return self::OFFLINE;
        }

        $silent = now()->diffInSeconds(\Carbon\Carbon::parse((string) $lastSeenAt), true);

        return $silent > self::OFFLINE_AFTER_SEC
            ? self::OFFLINE
            : ($stored ?: self::AVAILABLE);
    }

    /**
     * الفريق كلُّه بحالته وحمولته — لوحة المشرف (البندان 1 و35).
     *
     * ⚠ استعلامان لا استعلامٌ لكل موظّف: الحمولةُ تُعدّ مرّةً واحدة
     * مجموعةً بالموظّف. وهو النمطُ المقرَّر في هذا المشروع.
     */
    public function team(): array
    {
        $staff = DB::table('support_staff')
            ->whereNull('deleted_at')->where('is_active', 1)
            ->orderBy('name')
            ->get(['id', 'name', 'role', 'presence', 'last_seen_at', 'capacity']);

        $load = DB::table('support_thread_state')
            ->whereNotNull('assigned_to')
            ->where('status', '<>', SupportThreadService::CLOSED)
            ->selectRaw('assigned_to, COUNT(*) AS n')
            ->groupBy('assigned_to')
            ->pluck('n', 'assigned_to');

        return $staff->map(function ($s) use ($load) {
            $eff = self::effective($s->presence, $s->last_seen_at);
            $n   = (int) ($load[$s->id] ?? 0);
            $cap = max(1, (int) $s->capacity);

            return [
                'id'            => (int) $s->id,
                'name'          => $s->name,
                'role'          => SupportPermissions::ROLES[$s->role] ?? $s->role,
                'presence'      => $eff,
                'presence_label' => self::LABELS[$eff],
                'presence_color' => self::COLORS[$eff],
                'open_threads'  => $n,
                'capacity'      => $cap,
                // نسبةُ الحمولة — تُلوَّن في الشاشة، وتتجاوز 100% عمداً حين
                // يُحمَّل الموظّف فوق سعته: إخفاءُ التجاوز يُخفي المشكلة.
                'load_pct'      => (int) round($n / $cap * 100),
                'last_seen_at'  => $s->last_seen_at ? (string) $s->last_seen_at : null,
            ];
        })->all();
    }

    // ══════════════════════════════════════════════════════════════════
    //  من يشاهد المحادثة الآن (البند 6)
    // ══════════════════════════════════════════════════════════════════

    /**
     * نبضةُ «أنا هنا» — تُنادى مع كل قراءةٍ للمحادثة.
     *
     * صفٌّ يُكتب فوقه لا صفٌّ جديد: عشرُ دقائقَ في محادثةٍ واحدة كانت
     * ستكتب ثلاثمئة صفّ.
     */
    public function touchViewer(int $threadId, object $staff, string $state = 'VIEWING'): void
    {
        try {
            $exp = now()->addSeconds(self::VIEWER_TTL_SEC);

            $updated = DB::table('support_viewers')
                ->where('thread_id', $threadId)->where('staff_id', $staff->id)
                ->update(['state' => $state, 'expires_at' => $exp, 'updated_at' => now()]);

            if ($updated === 0) {
                DB::table('support_viewers')->insert([
                    'thread_id'  => $threadId,
                    'staff_id'   => $staff->id,
                    'staff_name' => mb_substr((string) $staff->name, 0, 120),
                    'state'      => $state,
                    'expires_at' => $exp,
                ]);
            }
        } catch (\Throwable) {
            // متعمَّد: علامةُ حضورٍ لا تُسقط قراءةَ محادثة.
        }
    }

    public function leave(int $threadId, int $staffId): void
    {
        try {
            DB::table('support_viewers')
                ->where('thread_id', $threadId)->where('staff_id', $staffId)->delete();
        } catch (\Throwable) {
        }
    }

    /**
     * من غيري يشاهد هذه المحادثة الآن.
     *
     * والكنسُ يجري هنا لا في مهمّةٍ دوريّة: الصفوفُ قليلة، والقراءةُ تمرّ
     * على الجدول أصلاً — فمهمّةٌ منفصلة لتنظيفه عملٌ زائدٌ بلا مقابل.
     */
    public function othersViewing(int $threadId, int $meId): array
    {
        try {
            DB::table('support_viewers')->where('expires_at', '<', now())->delete();
        } catch (\Throwable) {
        }

        return DB::table('support_viewers')
            ->where('thread_id', $threadId)
            ->where('staff_id', '<>', $meId)
            ->where('expires_at', '>', now())
            ->orderBy('updated_at')
            ->get(['staff_id', 'staff_name', 'state'])
            ->map(fn ($v) => [
                'staff_id' => (int) $v->staff_id,
                'name'     => $v->staff_name,
                'state'    => $v->state,
                // ⚠ نصٌّ صريح لا رمز: «يكتب» تعني ردّاً في الطريق، و«يشاهد»
                // تعني عيناً على الحالة. والخلطُ بينهما هو ما يُنتج ردّين.
                'text'     => $v->state === 'TYPING'
                    ? $v->staff_name . ' يكتب ردّاً الآن'
                    : $v->staff_name . ' يشاهد هذه المحادثة',
            ])->all();
    }
}
