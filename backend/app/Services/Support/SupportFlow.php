<?php

namespace App\Services\Support;

use Illuminate\Support\Facades\DB;

/**
 * سيرُ العمل — الدفعة الثالثة.
 *
 * بنودُ المالك: 8 الردود الجاهزة · 13 التصعيد · 15 التأجيل · 16 المتابعة
 * · 21 و37 المسودّات · 34 التسليم بين الموظّفين.
 *
 * ── ما يجمع هذه الخمسة ─────────────────────────────────────────────────
 *
 * كلُّها تجيب عن سؤالٍ واحد: **كيف تنتقل الحالةُ من يدٍ إلى يد، أو من
 * اليوم إلى الغد، بلا أن يضيع ما يُعرف عنها؟** فالتسليمُ نقلٌ في المكان،
 * والتأجيلُ نقلٌ في الزمن، والتصعيدُ نقلٌ في المستوى — وثلاثتُها تفشل
 * بالطريقة نفسِها: تصل الحالةُ ولا يصل سببُها.
 *
 * ولذلك **السببُ إلزاميّ في الثلاثة**، ويُكتب ملاحظةً داخلية في المحادثة
 * نفسِها لا في جدولٍ جانبيّ: من يفتح المحادثةَ غداً يقرأ لماذا وصلته حيث
 * يقرأ كلَّ شيءٍ آخر، لا في شاشةٍ ثانية يجب أن يعرف أنها موجودة.
 *
 * ⚠ ولا شيء هنا يمسّ المال: قوالبُ نصّ، وتذكيراتٌ، ومواعيدُ عودة.
 */
class SupportFlow
{
    /** أحداثُ الشريط الزمنيّ التي تخصّ هذه الدفعة. */
    public const EV_HANDOFF   = 'HANDOFF';
    public const EV_ESCALATED = 'ESCALATED';
    public const EV_FOLLOWUP  = 'FOLLOWUP';

    /*
     * ⚠ وأسماءُها العربية في `SupportOps::EVENT_LABELS` لا هنا:
     * الشّريطُ الزّمنيّ يقرأُ من قائمةٍ واحدة، وقائمةٌ ثانية
     * تعني حدثاً يظهر باسمِه الإنجليزيّ لأنّ أحداً نسي نسخَه.
     */

    /**
     * ⚠ سقفُ التأجيل ثلاثون يوماً.
     *
     * ليس رقماً اعتباطياً: التأجيلُ يُخفي محادثةَ وكيلٍ عن الفريق كلِّه،
     * وتأجيلٌ بلا سقفٍ يصير إخفاءً دائماً — وهو أسوأ من الإغلاق، لأن
     * المغلقةَ يراها من يفتح فلترَها بينما المؤجَّلةُ إلى سنةٍ لا يراها
     * أحد. من احتاج أكثر من شهر فما يريده الإغلاق.
     */
    public const MAX_SNOOZE_DAYS = 30;

    /*
     * ⚠ `ensureState` تُستعار من `SupportThreadService` ولا تُكتب
     * هنا ثانيةً: نسختان من «أنشئ الصّف إن غاب» تفترقان
     * عند أوّل عمودٍ يُضاف إلى الجدول.
     */
    public function __construct(
        private SupportOps $ops,
        private SupportThreadService $threads,
    ) {
    }

    // ══════════════════════════════════════════════════════════════════
    //  الردود الجاهزة (البند 8)
    // ══════════════════════════════════════════════════════════════════

    /**
     * ما يراه هذا الموظّف: المشترَكُ للفريق، وخاصُّه هو وحده.
     *
     * ⚠ استعلامٌ واحد لا اثنان. والأكثرُ استعمالاً أوّلاً: قائمةٌ من عشرين
     * قالباً مرتّبةً أبجدياً تجعل الموظّف يبحث كلَّ مرّةٍ عن الثلاثة التي
     * يكتبها كلَّ يوم.
     */
    public function savedReplies(int $staffId): array
    {
        return DB::table('support_saved_replies as r')
            ->leftJoin('support_categories as c', 'c.id', '=', 'r.category_id')
            ->where('r.is_active', 1)
            ->where(fn ($w) => $w->whereNull('r.owner_staff_id')
                                 ->orWhere('r.owner_staff_id', $staffId))
            ->orderByDesc('r.uses')
            ->orderBy('r.title')
            ->limit(200)
            ->get(['r.id', 'r.title', 'r.body', 'r.owner_staff_id', 'r.uses',
                   'r.category_id', 'c.name as category_name'])
            ->map(fn ($r) => [
                'id'            => (int) $r->id,
                'title'         => $r->title,
                'body'          => $r->body,
                'is_shared'     => $r->owner_staff_id === null,
                'uses'          => (int) $r->uses,
                'category_id'   => $r->category_id ? (int) $r->category_id : null,
                'category_name' => $r->category_name,
            ])->all();
    }

    /**
     * إنشاءُ قالب.
     *
     * ⚠ المشترَكُ يحتاج صلاحيةً والخاصُّ لا: قالبٌ خاصٌّ اختصارُ يدٍ لصاحبه
     * ولا يراه أحد، أمّا المشترَك فنصٌّ يُرسَل باسم الشركة إلى وكلائها —
     * ومن يكتبه يكتب لسانَ المؤسّسة.
     */
    public function createReply(string $title, string $body, bool $shared,
                                ?int $categoryId, object $by): array
    {
        $title = trim($title);
        $body  = trim($body);

        if ($title === '' || mb_strlen($title) > 120) {
            return ['error' => 'العنوان: بين حرفٍ و120 حرفاً.'];
        }
        if ($body === '' || mb_strlen($body) > 4000) {
            return ['error' => 'النصّ: بين حرفٍ و4000 حرف.'];
        }
        /*
         * ⚠ الفحصُ على **الممنوح فعلاً** لا على سقف الدّور.
         *
         * كانت هذه `allowedForRole` — وهي تقول ما **يجوز** للدّور
         * أن يحملَه، لا ما يحملُه هذا الحساب. والفرقُ بينهما هو
         * المنعُ بالأصل كلُّه: موظّفٌ لم تُمنَح له الصّلاحية كان
         * يكتب قالباً يُرسل باسم الشّركة إلى وكلائها، لأنّ
         * المفتاح داخلٌ في سقفِه. كشفَه الاختبار.
         */
        if ($shared && !in_array('MANAGE_SAVED_REPLIES', $by->permissions ?? [], true)) {
            return ['error' => 'القوالب المشترَكة تحتاج صلاحيةً.'];
        }
        if ($categoryId !== null
            && !DB::table('support_categories')->where('id', $categoryId)->exists()) {
            return ['error' => 'تصنيفٌ غير معروف.'];
        }

        $id = DB::table('support_saved_replies')->insertGetId([
            'title'          => $title,
            'body'           => $body,
            'owner_staff_id' => $shared ? null : (int) $by->id,
            'category_id'    => $categoryId,
            'created_by'     => (int) $by->id,
            'created_at'     => now(),
        ]);

        return ['id' => (int) $id];
    }

    /** تعديلُ قالب — ولا يُبدَّل مالكُه: مشترَكٌ يبقى مشترَكاً وخاصٌّ خاصّاً. */
    public function updateReply(int $id, array $changes, object $by): array
    {
        $r = DB::table('support_saved_replies')->where('id', $id)->first();
        if (!$r) {
            return ['error' => 'القالب غير موجود.'];
        }
        if (!$this->mayEditReply($r, $by)) {
            return ['error' => 'لا تملك تعديل هذا القالب.'];
        }

        $set = [];
        if (array_key_exists('title', $changes)) {
            $t = trim((string) $changes['title']);
            if ($t === '' || mb_strlen($t) > 120) {
                return ['error' => 'العنوان: بين حرفٍ و120 حرفاً.'];
            }
            $set['title'] = $t;
        }
        if (array_key_exists('body', $changes)) {
            $b = trim((string) $changes['body']);
            if ($b === '' || mb_strlen($b) > 4000) {
                return ['error' => 'النصّ: بين حرفٍ و4000 حرف.'];
            }
            $set['body'] = $b;
        }
        if (array_key_exists('is_active', $changes)) {
            $set['is_active'] = $changes['is_active'] ? 1 : 0;
        }

        if ($set === []) {
            return ['ok' => true];
        }

        $set['updated_at'] = now();
        DB::table('support_saved_replies')->where('id', $id)->update($set);

        return ['ok' => true];
    }

    /**
     * تسجيلُ استعمال — يُنادى عند الإدراج لا عند الإرسال.
     *
     * ⚠ عمداً: القالبُ نفع صاحبَه حين وفّر عليه الكتابة، سواءٌ أرسل نصَّه
     * كما هو أم عدّله. وعدُّ الإرسال وحده يُنزل القوالبَ التي تُعدَّل
     * دائماً — وهي غالباً الأفضل.
     *
     * ولا يُخطئ إن فشل: عدّادُ ترتيبٍ لا رقمٌ محاسبيّ.
     */
    public function markReplyUsed(int $id, int $staffId): void
    {
        try {
            DB::table('support_saved_replies')
                ->where('id', $id)
                ->where(fn ($w) => $w->whereNull('owner_staff_id')
                                     ->orWhere('owner_staff_id', $staffId))
                ->increment('uses');
        } catch (\Throwable) {
        }
    }

    private function mayEditReply(object $r, object $by): bool
    {
        if ($r->owner_staff_id !== null) {
            return (int) $r->owner_staff_id === (int) $by->id;
        }

        return in_array('MANAGE_SAVED_REPLIES', $by->permissions ?? [], true);
    }

    // ══════════════════════════════════════════════════════════════════
    //  المسودّات (البندان 21 و37)
    // ══════════════════════════════════════════════════════════════════

    /**
     * حفظُ مسودّة — أو محوُها حين تفرغ.
     *
     * ⚠ الفراغُ يمحو الصفَّ ولا يحفظ نصّاً فارغاً: مسودّةٌ فارغة تجعل
     * الواجهةَ تُظهر مؤشّر «لديك مسودّة» على محادثةٍ لا شيء فيها.
     */
    public function saveDraft(int $threadId, int $staffId, string $body, bool $isInternal): void
    {
        $body = rtrim($body);

        if ($body === '') {
            DB::table('support_drafts')
                ->where('thread_id', $threadId)->where('staff_id', $staffId)->delete();
            return;
        }

        // 8000 حرفٍ سقفٌ للنصّ المحفوظ: ما فوقها لصقٌ لا كتابة.
        $body = mb_substr($body, 0, 8000);

        DB::table('support_drafts')->updateOrInsert(
            ['thread_id' => $threadId, 'staff_id' => $staffId],
            ['body' => $body, 'is_internal' => $isInternal ? 1 : 0, 'updated_at' => now()],
        );
    }

    /** المسودّةُ عند فتح المحادثة — ولا تُقرأ في كل نبضة. */
    public function draft(int $threadId, int $staffId): ?array
    {
        $d = DB::table('support_drafts')
            ->where('thread_id', $threadId)->where('staff_id', $staffId)
            ->first(['body', 'is_internal', 'updated_at']);

        return $d ? [
            'body'        => $d->body,
            'is_internal' => (bool) $d->is_internal,
            'updated_at'  => (string) $d->updated_at,
        ] : null;
    }

    public function clearDraft(int $threadId, int $staffId): void
    {
        DB::table('support_drafts')
            ->where('thread_id', $threadId)->where('staff_id', $staffId)->delete();
    }

    /** المحادثاتُ التي لهذا الموظّف فيها مسودّة — استعلامٌ واحد للصفحة. */
    public function draftThreads(array $threadIds, int $staffId): array
    {
        if ($threadIds === []) {
            return [];
        }

        $out = [];
        foreach (array_chunk($threadIds, 1000) as $chunk) {
            foreach (DB::table('support_drafts')
                        ->whereIn('thread_id', $chunk)
                        ->where('staff_id', $staffId)
                        ->get(['thread_id']) as $r) {
                $out[] = (int) $r->thread_id;
            }
        }

        return $out;
    }

    // ══════════════════════════════════════════════════════════════════
    //  التأجيل (البند 15)
    // ══════════════════════════════════════════════════════════════════

    /**
     * تأجيلُ محادثة إلى موعد.
     *
     * ⚠ **والمهلةُ لا تتوقّف بالتأجيل.**
     *
     * هذا قرارٌ لا سهو. لو أوقف التأجيلُ عدّادَ المهلة لصار أسرعَ طريقةٍ
     * إلى فريقٍ «ملتزمٍ» على الورق: يُؤجَّل ما قارب مهلتَه فيختفي الأحمر،
     * ولا يتغيّر شيءٌ عند الوكيل الذي ما زال ينتظر. فالتأجيلُ يُخفي
     * المحادثةَ عن **الصندوق** ليريح العين، ولا يُخفيها عن **القياس**.
     *
     * ومحادثةٌ يكتب فيها الوكيلُ تعود فوراً — يُلغى تأجيلُها في
     * `onAgentMessage`. من كتب لك لا يُؤجَّل.
     */
    public function snooze(int $threadId, $until, string $reason, object $by): array
    {
        $reason = trim($reason);
        if ($reason === '') {
            return ['error' => 'اكتب سببَ التأجيل — من يفتحها بعدك يحتاج أن يعرف لماذا انتظرت.'];
        }

        try {
            $due = \Carbon\Carbon::parse((string) $until);
        } catch (\Throwable) {
            return ['error' => 'موعدٌ غير مفهوم.'];
        }

        if ($due->lte(now())) {
            return ['error' => 'الموعد في الماضي.'];
        }
        if ($due->gt(now()->addDays(self::MAX_SNOOZE_DAYS))) {
            return ['error' => 'أقصى تأجيلٍ ' . self::MAX_SNOOZE_DAYS . ' يوماً. ما تجاوز ذلك يُغلَق لا يُؤجَّل.'];
        }

        $this->threads->ensureState($threadId);

        DB::table('support_thread_state')->where('thread_id', $threadId)->update([
            'snoozed_until' => $due,
            'snoozed_at'    => now(),
            'snoozed_by'    => (int) $by->id,
            'snooze_reason' => mb_substr($reason, 0, 400),
            'updated_at'    => now(),
        ]);

        $this->ops->event($threadId, SupportOps::EV_SNOOZED, $by,
            null, $due->toDateTimeString(), $reason);

        return ['ok' => true, 'until' => $due->toIso8601String()];
    }

    public function unsnooze(int $threadId, object $by, string $why = ''): array
    {
        $s = DB::table('support_thread_state')->where('thread_id', $threadId)
            ->first(['snoozed_until']);

        if (!$s || $s->snoozed_until === null) {
            return ['ok' => true, 'changed' => false];
        }

        DB::table('support_thread_state')->where('thread_id', $threadId)->update([
            'snoozed_until' => null,
            'snoozed_at'    => null,
            'snoozed_by'    => null,
            'snooze_reason' => null,
            'updated_at'    => now(),
        ]);

        $this->ops->event($threadId, SupportOps::EV_UNSNOOZED, $by, null, null,
            $why !== '' ? $why : null);

        return ['ok' => true, 'changed' => true];
    }

    /**
     * رسالةٌ من الوكيل تُعيد المحادثةَ من التأجيل.
     *
     * ⚠ تُنادى من `ChatController::send` كما تُنادى `onAgentMessage` في
     * الحالة والمهلة. ومن كتب لك لا يُؤجَّل: تأجيلٌ يصمد أمام رسالةٍ
     * جديدة يعني وكيلاً يكتب ولا يراه أحد.
     *
     * ولا تُخطئ أبداً: مسارُ إرسال الوكيل ليس مكاناً لخطأٍ في التأجيل.
     */
    public function onAgentMessage(int $threadId): void
    {
        try {
            $n = DB::table('support_thread_state')
                ->where('thread_id', $threadId)
                ->whereNotNull('snoozed_until')
                ->update([
                    'snoozed_until' => null,
                    'snoozed_at'    => null,
                    'snoozed_by'    => null,
                    'snooze_reason' => null,
                    'updated_at'    => now(),
                ]);

            /*
             * ⚠ والعودةُ تُكتب في الشّريط وإن لم يفعلها موظّف.
             *
             * شريطٌ يقول «أُجّلت» ولا يقول «عادت» يُقرأ على أنّها
             * ما زالت مخفيّةً. والفاعلُ هنا ليس موظّفاً بل الوكيل
             * نفسُه، فيُكتب بلا فاعلٍ ويُذكر السّبب.
             */
            if ($n > 0) {
                /*
                 * ⚠ وفاعلٌ باسمٍ لا بلا اسم: سطرٌ في الشّريط بلا
                 * فاعلٍ يُقرأ عطلاً لا حدثاً تلقائيّاً. والمُعرّفُ يبقى
                 * فارغاً لأنّ الفاعِل ليس موظّفاً في `support_staff`.
                 */
                $this->ops->event($threadId, SupportOps::EV_UNSNOOZED,
                    (object) ['name' => 'النظام'],
                    null, null, 'رسالةٌ جديدةٌ من الوكيل');
            }
        } catch (\Throwable) {
        }
    }

    // ══════════════════════════════════════════════════════════════════
    //  التسليم والتصعيد (البندان 34 و13)
    // ══════════════════════════════════════════════════════════════════

    /**
     * تسليمُ محادثةٍ إلى زميل — بملاحظةٍ إلزامية.
     *
     * ⚠ **الملاحظةُ ليست تزييناً.** إسنادٌ صامت يجعل من وصلته الحالةُ يقرأ
     * المحادثةَ كلَّها ليعرف ما المطلوب منه، وهو ما يُهدر الوقتَ الذي
     * وُضع التسليمُ ليوفّره. ولذلك التسليمُ **ليس** زرَّ إسنادٍ بمسمّى
     * آخر: هو إسنادٌ + ملاحظةٌ داخلية + حدثٌ في الشريط، في عمليةٍ واحدة.
     *
     * والملاحظةُ تُكتب في المحادثة نفسِها لا في حقلٍ جانبيّ: من يفتحها
     * يقرأ سببَ وصولها حيث يقرأ كلَّ شيءٍ آخر.
     */
    public function handoff(int $threadId, int $toStaffId, string $note, object $by): array
    {
        $note = trim($note);
        if ($note === '') {
            return ['error' => 'اكتب ما تريد من زميلك — تسليمٌ بلا ملاحظةٍ يجعله يقرأ المحادثة كلَّها.'];
        }

        $to = DB::table('support_staff')->where('id', $toStaffId)
            ->whereNull('deleted_at')->where('is_active', 1)
            ->first(['id', 'name']);

        if (!$to) {
            return ['error' => 'الموظّف غير موجود أو موقوف.'];
        }
        if ((int) $to->id === (int) $by->id) {
            return ['error' => 'لا يُسلَّم المرءُ إلى نفسه.'];
        }

        $this->threads->ensureState($threadId);

        $from = DB::table('support_thread_state')->where('thread_id', $threadId)
            ->value('assigned_to');

        DB::table('support_thread_state')->where('thread_id', $threadId)->update([
            'assigned_to' => (int) $to->id,
            'assigned_at' => now(),
            'assigned_by' => (int) $by->id,
            'updated_at'  => now(),
        ]);

        $this->ops->event($threadId, self::EV_HANDOFF, $by,
            $from ? (string) $from : null, $to->name, $note);

        return ['ok' => true, 'to' => $to->name, 'note' => $note];
    }

    /**
     * تصعيدُ حالة — البند 13.
     *
     * ⚠ **يرفع درجةً واحدة لا يقفز إلى «حرجة».** التصعيدُ اعترافٌ بأن
     * الحالة أكبرُ ممّا ظُنّ، لا إعلانُ طوارئ — ولو قفز كلُّ تصعيدٍ إلى
     * أعلى درجة لصارت «حرجة» تعني «صُعِّدت» وضاع معناها.
     *
     * ⚠ **والسببُ إلزاميّ**، فبغيره التصعيدُ تغييرُ أولويةٍ لا أكثر.
     *
     * والإسنادُ اختياريّ: من يعرف المشرفَ المناسب يسمّيه، ومن لا يعرف
     * يترك الحالةَ مرفوعةً بلا مالكٍ ليأخذها من يستطيع — وإجبارُ الموظّف
     * على اختيار اسمٍ يجعله يختار أوّلَ اسمٍ في القائمة.
     */
    public function escalate(int $threadId, string $reason, ?int $toStaffId, object $by): array
    {
        $reason = trim($reason);
        if ($reason === '') {
            return ['error' => 'اكتب سببَ التصعيد — تصعيدٌ بلا سبب تغييرُ أولويةٍ لا تصعيد.'];
        }

        $this->threads->ensureState($threadId);

        $cur = DB::table('support_thread_state')->where('thread_id', $threadId)
            ->first(['priority', 'assigned_to', 'snoozed_until']);

        $now  = $cur->priority ?: SupportOps::NORMAL;
        $next = SupportOps::nextPriorityUp($now);

        $set = [
            'escalated_at'      => now(),
            'escalated_by'      => (int) $by->id,
            'escalation_reason' => mb_substr($reason, 0, 400),
            'updated_at'        => now(),
        ];

        if ($next !== $now) {
            $set['priority']    = $next;
            $set['priority_at'] = now();
            $set['priority_by'] = (int) $by->id;
        }

        $toName = null;
        if ($toStaffId !== null) {
            $to = DB::table('support_staff')->where('id', $toStaffId)
                ->whereNull('deleted_at')->where('is_active', 1)->first(['id', 'name']);
            if (!$to) {
                return ['error' => 'الموظّف غير موجود أو موقوف.'];
            }
            $set['escalated_to'] = (int) $to->id;
            $set['assigned_to']  = (int) $to->id;
            $set['assigned_at']  = now();
            $set['assigned_by']  = (int) $by->id;
            $toName = $to->name;
        }

        /*
         * ⚠ والتأجيلُ يُلغى مع التصعيد. حالةٌ صُعِّدت ومؤجَّلةٌ في آنٍ واحد
         * تناقضٌ: الأولى تقول «انظر إليها الآن» والثانية تُخفيها.
         */
        $set['snoozed_until'] = null;
        $set['snoozed_at']    = null;
        $set['snoozed_by']    = null;
        $set['snooze_reason'] = null;

        $wasSnoozed = $cur && $cur->snoozed_until !== null;

        DB::table('support_thread_state')->where('thread_id', $threadId)->update($set);

        // وإن ألغى التصعيدُ تأجيلاً، فالشّريطُ يقولُها.
        if ($wasSnoozed) {
            $this->ops->event($threadId, SupportOps::EV_UNSNOOZED, $by,
                null, null, 'أُلغي بالتّصعيد');
        }

        $this->ops->event($threadId, self::EV_ESCALATED, $by, $now,
            $toName ? ($next . ' ⇐ ' . $toName) : $next, $reason);

        return ['ok' => true, 'priority' => $next, 'to' => $toName];
    }

    // ══════════════════════════════════════════════════════════════════
    //  المتابعات (البند 16)
    // ══════════════════════════════════════════════════════════════════

    /**
     * تذكيرٌ شخصيّ بموعد.
     *
     * ⚠ **يخصّ صاحبَه وحده ولا يغيّر شيئاً للفريق.** الفرقُ بينه وبين
     * التأجيل هو الفرقُ بين «ذكّرني» و«أخفِها عن الجميع» — وخلطُهما يجعل
     * تذكيرَ موظّفٍ يُخفي محادثةً عن زملائه.
     */
    public function addFollowup(int $threadId, $dueAt, string $note, object $by): array
    {
        try {
            $due = \Carbon\Carbon::parse((string) $dueAt);
        } catch (\Throwable) {
            return ['error' => 'موعدٌ غير مفهوم.'];
        }

        if ($due->lte(now())) {
            return ['error' => 'الموعد في الماضي.'];
        }
        if ($due->gt(now()->addYear())) {
            return ['error' => 'أقصى متابعةٍ سنةٌ واحدة.'];
        }

        // ⚠ عشرُ متابعاتٍ مفتوحة لكل موظّفٍ سقفاً: قائمةُ تذكيراتٍ بمئة
        // بندٍ لا تُقرأ، وما لا يُقرأ لا يُذكّر بشيء.
        $open = DB::table('support_followups')
            ->where('staff_id', $by->id)->whereNull('done_at')->count();
        if ($open >= 50) {
            return ['error' => 'لديك 50 متابعةً مفتوحة. أنهِ بعضَها أوّلاً.'];
        }

        $id = DB::table('support_followups')->insertGetId([
            'thread_id'  => $threadId,
            'staff_id'   => (int) $by->id,
            'due_at'     => $due,
            'note'       => mb_substr(trim($note), 0, 400) ?: null,
            'created_at' => now(),
        ]);

        $this->ops->event($threadId, self::EV_FOLLOWUP, $by, null,
            $due->toDateTimeString(), trim($note) ?: null);

        return ['id' => (int) $id, 'due_at' => $due->toIso8601String()];
    }

    /**
     * متابعاتُ هذا الموظّف — المستحقّةُ أوّلاً.
     *
     * استعلامان: المتابعاتُ نفسُها، ثم أسماءُ وكلائها دفعةً واحدة. وتذكيرٌ
     * يقول «محادثة #14» بلا اسمٍ لا يُعرف بمَ يذكّر.
     */
    public function followups(int $staffId, int $limit = 50): array
    {
        $rows = DB::table('support_followups')
            ->where('staff_id', $staffId)
            ->whereNull('done_at')
            ->orderBy('due_at')
            ->limit($limit)
            ->get(['id', 'thread_id', 'due_at', 'note', 'created_at']);

        if ($rows->isEmpty()) {
            return [];
        }

        $names = $this->threadNames($rows->pluck('thread_id')->map(fn ($v) => (int) $v)->all());

        return $rows->map(fn ($f) => [
            'id'         => (int) $f->id,
            'thread_id'  => (int) $f->thread_id,
            'agent_name' => $names[(int) $f->thread_id] ?? ('محادثة #' . $f->thread_id),
            'due_at'     => (string) $f->due_at,
            'is_due'     => strtotime((string) $f->due_at) <= time(),
            'note'       => $f->note,
        ])->all();
    }

    public function doneFollowup(int $id, int $staffId): array
    {
        // ⚠ الشرطُ على `staff_id` في التحديث نفسِه لا في قراءةٍ قبله:
        // متابعةُ زميلٍ لا تُنهى من هنا، والفحصُ في الاستعلام هو ما يضمنه.
        $n = DB::table('support_followups')
            ->where('id', $id)->where('staff_id', $staffId)->whereNull('done_at')
            ->update(['done_at' => now()]);

        return ['ok' => true, 'changed' => $n > 0];
    }

    /** أسماءُ الوكلاء لمجموعة محادثات — استعلامٌ واحد. */
    private function threadNames(array $threadIds): array
    {
        if ($threadIds === []) {
            return [];
        }

        $q = DB::table('chat_threads as t')
            ->leftJoin('users as u', 'u.id', '=', 't.agent_id')
            ->whereIn('t.id', array_slice($threadIds, 0, 1000));

        SupportThreadService::joinAgentIdentity($q);

        $out = [];
        foreach ($q->get(['t.id', 't.agent_id', 'u.name as user_name',
                          'acc.AccName as acc_name']) as $r) {
            $out[(int) $r->id] = SupportThreadService::agentName(
                $r->acc_name, $r->user_name, (int) $r->agent_id);
        }

        return $out;
    }
}
