<?php

namespace App\Services\Support;

use Illuminate\Support\Facades\DB;
use Illuminate\Support\Str;

/**
 * التشغيل فوق الدردشة — الأولوية والتصنيف والوسوم والشريط الزمني.
 *
 * (بنود المالك 3 · 4 · 12 · 14 · 17)
 *
 * ⚠ **تطويرٌ فوق القائم لا بناءٌ ثانٍ.** لا رسالةَ تُكتب من هنا ولا تُقرأ:
 * الرسائل كلُّها في {@see \App\Services\ChatService}. وهذا الصنف يجيب عن
 * أسئلةٍ لا يعرفها نظام الدردشة ولا يجب أن يعرفها: ما أهميّةُ هذه الحالة،
 * ومن أيّ نوعٍ هي، وماذا جرى لها.
 *
 * ⚠ ولا شيء هنا يمسّ المال: أولويةٌ وتصنيفٌ ووسمٌ وسجلُّ أحداث.
 */
class SupportOps
{
    // ── الأولوية (البند 3) ──────────────────────────────────────────
    public const NORMAL   = 'NORMAL';
    public const HIGH     = 'HIGH';
    public const URGENT   = 'URGENT';
    public const CRITICAL = 'CRITICAL';

    /**
     * الأولويات مرتّبةً تصاعدياً — والرقمُ يُستعمل في الفرز.
     *
     * أربعٌ لا أكثر: سلّمٌ أطول يجعل الموظّف يتردّد بين درجتين متجاورتين
     * فيختار الوسط دائماً، فتفقد الأولويةُ معناها.
     */
    public const PRIORITIES = [
        self::NORMAL   => ['rank' => 0, 'label' => 'عادية',  'color' => '#6B7280'],
        self::HIGH     => ['rank' => 1, 'label' => 'مهمّة',   'color' => '#D98324'],
        self::URGENT   => ['rank' => 2, 'label' => 'عاجلة',  'color' => '#E2653C'],
        self::CRITICAL => ['rank' => 3, 'label' => 'حرجة',   'color' => '#C0392B'],
    ];

    // ── أنواع أحداث الشريط الزمني (البند 17) ────────────────────────
    public const EV_OPENED     = 'OPENED';
    public const EV_ASSIGNED   = 'ASSIGNED';
    public const EV_UNASSIGNED = 'UNASSIGNED';
    public const EV_STATUS     = 'STATUS';
    public const EV_PRIORITY   = 'PRIORITY';
    public const EV_CATEGORY   = 'CATEGORY';
    public const EV_TAG_ADD    = 'TAG_ADD';
    public const EV_TAG_REMOVE = 'TAG_REMOVE';
    public const EV_NOTE       = 'NOTE';
    public const EV_REOPENED   = 'REOPENED';
    public const EV_CLOSED     = 'CLOSED';
    public const EV_SNOOZED    = 'SNOOZED';
    public const EV_UNSNOOZED  = 'UNSNOOZED';

    public const EVENT_LABELS = [
        self::EV_OPENED     => 'فُتحت الحالة',
        self::EV_ASSIGNED   => 'أُسنِدت',
        self::EV_UNASSIGNED => 'نُزع الإسناد',
        self::EV_STATUS     => 'تغيّرت الحالة',
        self::EV_PRIORITY   => 'تغيّرت الأولوية',
        self::EV_CATEGORY   => 'تغيّر التصنيف',
        self::EV_TAG_ADD    => 'أُضيف وسم',
        self::EV_TAG_REMOVE => 'أُزيل وسم',
        self::EV_NOTE       => 'ملاحظة داخلية',
        self::EV_REOPENED   => 'أُعيد فتحها',
        self::EV_CLOSED     => 'أُغلقت',
        self::EV_SNOOZED    => 'أُجّلت',
        self::EV_UNSNOOZED  => 'عادت من التأجيل',
    ];

    public static function priorityExists(string $p): bool
    {
        return array_key_exists($p, self::PRIORITIES);
    }

    public static function priorityRank(?string $p): int
    {
        return self::PRIORITIES[$p ?? self::NORMAL]['rank'] ?? 0;
    }

    // ══════════════════════════════════════════════════════════════════
    //  الشريط الزمني
    // ══════════════════════════════════════════════════════════════════

    /**
     * يسجّل حدثاً تشغيلياً على المحادثة.
     *
     * ⚠ **لا يرمي أبداً.** الشريط وصفٌ لما جرى، وفشلُ وصفٍ لا يجوز أن
     * يُلغيَ ما جرى — كما في `SupportAudit::log` وللسبب نفسه.
     *
     * ⚠ ولا يُكتب هنا نصُّ رسالةٍ ولا مرفق: `note` وصفٌ تشغيليّ يكتبه
     * موظّف الدعم (سببُ تصعيدٍ مثلاً)، لا نسخةٌ من محتوى المحادثة.
     */
    public function event(
        int $threadId,
        string $kind,
        ?object $actor = null,
        ?string $from = null,
        ?string $to = null,
        ?string $note = null,
    ): void {
        try {
            DB::table('support_events')->insert([
                'thread_id'  => $threadId,
                'kind'       => $kind,
                'actor_id'   => $actor->id ?? null,
                'actor_name' => isset($actor->name) ? mb_substr((string) $actor->name, 0, 120) : null,
                'from_value' => $from !== null ? mb_substr($from, 0, 120) : null,
                'to_value'   => $to !== null ? mb_substr($to, 0, 120) : null,
                'note'       => $note !== null ? mb_substr($note, 0, 500) : null,
            ]);
        } catch (\Throwable) {
            // متعمَّد.
        }
    }

    /** الشريط الزمني لمحادثة — الأحدث أوّلاً. */
    public function timeline(int $threadId, int $limit = 100): array
    {
        return DB::table('support_events')
            ->where('thread_id', $threadId)
            ->orderByDesc('id')
            ->limit($limit)
            ->get()
            ->map(fn ($e) => [
                'id'         => (int) $e->id,
                'kind'       => $e->kind,
                'label'      => self::EVENT_LABELS[$e->kind] ?? $e->kind,
                'actor_name' => $e->actor_name,
                'from'       => $e->from_value,
                'to'         => $e->to_value,
                'note'       => $e->note,
                'created_at' => (string) $e->created_at,
            ])->all();
    }

    // ══════════════════════════════════════════════════════════════════
    //  الرقم المرجعي (البند 12)
    // ══════════════════════════════════════════════════════════════════

    /**
     * يضمن أن للمحادثة رقماً مرجعياً، ويُرجعه.
     *
     * ── لماذا عشوائيّ لا تسلسليّ ─────────────────────────────────────
     *
     * نصُّ البند: «يجب ألا يكون رقم المرجع قابلاً للتنبؤ بطريقة تضعف
     * الأمان». ورقمٌ تسلسليّ يُخبر من رآه بعددِ حالات الدعم كلِّها، ويجعل
     * الرقمَ التالي معلوماً — فمن حصل على مرجعٍ يستطيع تخمين غيره.
     *
     * الشكل `RH-XXXXXXXX` بثمانية محارف من أبجديةٍ بلا `0/O/1/I` — لأنه
     * يُملى في الهاتف ويُكتب باليد، والحروفُ المتشابهة تُقرأ خطأً فيُهدَر
     * وقتُ الطرفين.
     *
     * والتصادمُ يُعالَج بإعادة المحاولة لا بالتجاهل: 32^8 احتمالاً تجعله
     * نادراً، والنادرُ يقع.
     */
    public function ensureReference(int $threadId): string
    {
        $existing = DB::table('support_thread_state')
            ->where('thread_id', $threadId)->value('reference');

        if ($existing) {
            return $existing;
        }

        for ($attempt = 0; $attempt < 6; $attempt++) {
            $ref = self::newReference();
            try {
                $affected = DB::table('support_thread_state')
                    ->where('thread_id', $threadId)
                    ->whereNull('reference')
                    ->update(['reference' => $ref, 'updated_at' => now()]);

                if ($affected > 0) {
                    return $ref;
                }

                // صفرُ صفوف: إمّا لا صفَّ حالةٍ بعد، أو سبقنا إليه نداءٌ آخر.
                $now = DB::table('support_thread_state')
                    ->where('thread_id', $threadId)->value('reference');
                if ($now) {
                    return $now;
                }
                return '';
            } catch (\Throwable) {
                // تصادمٌ على الفهرس الفريد — تُعاد المحاولة برقمٍ آخر.
            }
        }

        return '';
    }

    /** أبجديةٌ بلا `0 O 1 I` — تُملى وتُكتب باليد بلا التباس. */
    private const ALPHABET = '23456789ABCDEFGHJKLMNPQRSTUVWXYZ';

    public static function newReference(): string
    {
        $out = '';
        for ($i = 0; $i < 8; $i++) {
            $out .= self::ALPHABET[random_int(0, strlen(self::ALPHABET) - 1)];
        }
        return 'RH-' . $out;
    }

    // ══════════════════════════════════════════════════════════════════
    //  الأولوية
    // ══════════════════════════════════════════════════════════════════

    public function setPriority(int $threadId, string $priority, object $by): array
    {
        if (!self::priorityExists($priority)) {
            return ['error' => 'أولوية غير معروفة.'];
        }

        $cur = DB::table('support_thread_state')->where('thread_id', $threadId)
            ->value('priority') ?? self::NORMAL;

        if ($cur === $priority) {
            return ['ok' => true, 'unchanged' => true];
        }

        DB::table('support_thread_state')->where('thread_id', $threadId)->update([
            'priority'    => $priority,
            'priority_at' => now(),
            'priority_by' => $by->id,
            'updated_at'  => now(),
        ]);

        $this->event($threadId, self::EV_PRIORITY, $by,
            self::PRIORITIES[$cur]['label'], self::PRIORITIES[$priority]['label']);

        return ['ok' => true];
    }

    // ══════════════════════════════════════════════════════════════════
    //  التصنيفات والوسوم (البند 4)
    // ══════════════════════════════════════════════════════════════════

    public function categories(bool $activeOnly = true): array
    {
        $q = DB::table('support_categories')->orderBy('sort_order')->orderBy('name');
        if ($activeOnly) {
            $q->where('is_active', 1);
        }

        return $q->get()->map(fn ($c) => [
            'id'        => (int) $c->id,
            'name'      => $c->name,
            'color'     => $c->color,
            'is_active' => (bool) $c->is_active,
        ])->all();
    }

    public function tags(bool $activeOnly = true): array
    {
        $q = DB::table('support_tag_defs')->orderBy('name');
        if ($activeOnly) {
            $q->where('is_active', 1);
        }

        return $q->get()->map(fn ($t) => [
            'id'        => (int) $t->id,
            'name'      => $t->name,
            'color'     => $t->color,
            'is_active' => (bool) $t->is_active,
        ])->all();
    }

    public function setCategory(int $threadId, ?int $categoryId, object $by): array
    {
        $name = null;
        if ($categoryId !== null) {
            $name = DB::table('support_categories')->where('id', $categoryId)
                ->where('is_active', 1)->value('name');
            if (!$name) {
                return ['error' => 'التصنيف غير موجود أو معطَّل.'];
            }
        }

        $curId = DB::table('support_thread_state')->where('thread_id', $threadId)
            ->value('category_id');
        $curName = $curId
            ? DB::table('support_categories')->where('id', $curId)->value('name')
            : null;

        DB::table('support_thread_state')->where('thread_id', $threadId)->update([
            'category_id' => $categoryId,
            'updated_at'  => now(),
        ]);

        $this->event($threadId, self::EV_CATEGORY, $by, $curName, $name);

        return ['ok' => true];
    }

    public function addTag(int $threadId, int $tagId, object $by): array
    {
        $name = DB::table('support_tag_defs')->where('id', $tagId)
            ->where('is_active', 1)->value('name');
        if (!$name) {
            return ['error' => 'الوسم غير موجود أو معطَّل.'];
        }

        $exists = DB::table('support_thread_tags')
            ->where('thread_id', $threadId)->where('tag_id', $tagId)->exists();

        if (!$exists) {
            DB::table('support_thread_tags')->insert([
                'thread_id' => $threadId, 'tag_id' => $tagId, 'added_by' => $by->id,
            ]);
            $this->event($threadId, self::EV_TAG_ADD, $by, null, $name);
        }

        return ['ok' => true];
    }

    public function removeTag(int $threadId, int $tagId, object $by): array
    {
        $name = DB::table('support_tag_defs')->where('id', $tagId)->value('name');

        $deleted = DB::table('support_thread_tags')
            ->where('thread_id', $threadId)->where('tag_id', $tagId)->delete();

        if ($deleted > 0) {
            $this->event($threadId, self::EV_TAG_REMOVE, $by, $name, null);
        }

        return ['ok' => true];
    }

    /**
     * وسومُ صفحةٍ كاملة من المحادثات — استعلامٌ واحد لا استعلامٌ لكل صفّ.
     *
     * وهو النمطُ المقرَّر في هذا المشروع بعد أن كلّف الشكلُ الآخر ثمانياً
     * وستّين ثانية في كشف الحساب.
     *
     * @param  int[] $threadIds
     * @return array<int, array<int, array{id:int,name:string,color:?string}>>
     */
    public function tagsForThreads(array $threadIds): array
    {
        if ($threadIds === []) {
            return [];
        }

        $rows = DB::table('support_thread_tags as tt')
            ->join('support_tag_defs as d', 'd.id', '=', 'tt.tag_id')
            ->whereIn('tt.thread_id', $threadIds)
            ->orderBy('d.name')
            ->get(['tt.thread_id', 'd.id', 'd.name', 'd.color']);

        $out = [];
        foreach ($rows as $r) {
            $out[(int) $r->thread_id][] = [
                'id'    => (int) $r->id,
                'name'  => $r->name,
                'color' => $r->color,
            ];
        }

        return $out;
    }

    // ══════════════════════════════════════════════════════════════════
    //  إدارة التصنيفات والوسوم — من الإدارة لا من الشيفرة (البند 4)
    // ══════════════════════════════════════════════════════════════════

    /** لونٌ سداسيّ أو لا لون — والتحقّق في الخادم لا في الشاشة. */
    private static function color(?string $c): ?string
    {
        $c = trim((string) $c);
        return preg_match('/^#[0-9A-Fa-f]{6}$/', $c) ? $c : null;
    }

    public function createCategory(string $name, ?string $color, object $by): array
    {
        $name = trim($name);
        if (mb_strlen($name) < 2) {
            return ['error' => 'اكتب اسم التصنيف.'];
        }
        if (DB::table('support_categories')->where('name', $name)->exists()) {
            return ['error' => 'هذا التصنيف موجود.'];
        }

        $id = (int) DB::table('support_categories')->insertGetId([
            'name'       => mb_substr($name, 0, 80),
            'color'      => self::color($color),
            'created_by' => $by->id,
        ]);

        return ['id' => $id];
    }

    public function createTag(string $name, ?string $color, object $by): array
    {
        $name = trim($name);
        if (mb_strlen($name) < 2) {
            return ['error' => 'اكتب اسم الوسم.'];
        }
        if (DB::table('support_tag_defs')->where('name', $name)->exists()) {
            return ['error' => 'هذا الوسم موجود.'];
        }

        $id = (int) DB::table('support_tag_defs')->insertGetId([
            'name'       => mb_substr($name, 0, 60),
            'color'      => self::color($color),
            'created_by' => $by->id,
        ]);

        return ['id' => $id];
    }

    /**
     * تعطيلٌ لا حذف.
     *
     * تصنيفٌ يُحذف يترك محادثاتٍ تشير إلى رقمٍ لا وجود له، فيختفي تصنيفُها
     * من التقارير كأنها لم تُصنَّف قطّ. والتعطيلُ يُخفيه من قوائم الاختيار
     * ويُبقي ما مضى مفهوماً.
     */
    public function setCategoryActive(int $id, bool $active): array
    {
        DB::table('support_categories')->where('id', $id)
            ->update(['is_active' => $active ? 1 : 0]);
        return ['ok' => true];
    }

    public function setTagActive(int $id, bool $active): array
    {
        DB::table('support_tag_defs')->where('id', $id)
            ->update(['is_active' => $active ? 1 : 0]);
        return ['ok' => true];
    }
}
