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
     * اسم الوكيل كما هو مسجَّل في منظومة الرحالة.
     *
     * ⚠ **`users.name` فارغٌ لكلّ وكيلٍ في المنظومة** (مُتحقَّقٌ منه على
     * القاعدة الحيّة: `NULL` في كل صفّ). فالاعتماد عليه يجعل موظّف الدعم
     * يرى «وكيل #104» بينما يعرّف الوكيل نفسه في الهاتف بـ«شركة الأمانة» —
     * ولا يستطيع الموظّف أن يعرف بمن يتحدّث.
     *
     * والمصدر هو **`AccountsTb.AccName`** — اسمُ الحساب في الشجرة المحاسبية،
     * أي «جاري شركة الامانة» كما هو مسجَّل في منظومة الرحالة.
     *
     * ⚠ **وليس `tenant_branding.company_name_ar`** (قرار المالك، 6 سبتمبر
     * 2026). ذلك الحقل يكتبه **الوكيل بنفسه** في تبويب «هويّة الشركة» في
     * تطبيقه، ويستطيع تغييره متى شاء. فلو عُرض هنا لرأى موظّفُ الدعم اسماً
     * اختاره الوكيل لا الاسم الذي تعرفه به الشركة — وقد يختلفان، ووقتَ
     * الخلاف تكون سجلات الرحالة هي المرجع لا ما كتبه الوكيل عن نفسه.
     *
     * ⚠ والقراءة من `AccountsTb` **قراءةٌ فقط**، ولا تكتب فيه شيئاً: هو
     * جدولٌ ماليّ والأمر الدائم يمنع المساس به. والوصلةُ على مفتاحه
     * الأساسي `AccID`، فهي رخيصة ولا تُشبه الاستعلامَ الفرعيَّ لكلّ صفّ.
     */
    public static function agentName(?string $accName, ?string $userName, int $agentId): string
    {
        foreach ([$accName, $userName] as $candidate) {
            if ($candidate !== null && trim($candidate) !== '') {
                return self::stripLedgerPrefix(trim($candidate));
            }
        }

        return 'وكيل #' . $agentId;
    }

    /**
     * إخفاء سابقة «جاري» المحاسبية عند العرض (أمر المالك، 6 سبتمبر 2026).
     *
     * «جاري شركة الامانة» في الشجرة المحاسبية تعني «الحساب الجاري لشركة
     * الأمانة» — و«جاري» مصطلحٌ محاسبيّ لا جزءٌ من اسم الوكيل. وموظّف الدعم
     * يتحدّث إلى شركة الأمانة لا إلى حسابٍ جارٍ.
     *
     * ⚠ **إخفاءٌ عند العرض فقط — ولا تعديل ولا حذف في قاعدة البيانات.**
     * الاسم يبقى كما هو في `AccountsTb`، والمنظومة المكتبية تراه كاملاً كما
     * كانت. التغيير هنا في شاشة الدعم وحدها.
     *
     * ⚠ **وتُحذف كلمة «جاري» بعينها، لا «أوّل كلمةٍ أياً كانت».** الفرق ليس
     * تدقيقاً: من 363 اسماً في القاعدة، 64 يبدأ بها و**299 لا يبدأ**. ومنها
     * «الحسن يوسف هارون محمد» و«صفوت عبدالواحد حسن» — وحذفُ أوّل كلمةٍ منها
     * يمسخها إلى «يوسف هارون محمد» و«عبدالواحد حسن»، فيصير موظّف الدعم
     * ينادي الناس بغير أسمائهم.
     *
     * وهذا هو القرار نفسه المتّخذ في `Fmt.localName` بتطبيق الوكيل: تُستبدل
     * «داخلية» وحدها ولا تُمسّ «نقل محلي»، لأن قاعدةً عامّة تُفسد ما لم
     * تُقصد.
     *
     * والإملاءان معاً — «جاري» بالياء و«جارى» بالألف المقصورة — لأن الإدخال
     * اليدويّ في المنظومة يكتبهما كليهما.
     */
    public static function stripLedgerPrefix(string $name): string
    {
        $out = preg_replace('/^(?:جاري|جارى)\s+/u', '', $name);

        // احتياطٌ: اسمٌ ليس فيه غير السابقة يبقى كما هو، فاسمٌ فارغ أسوأ
        // من سابقةٍ ظاهرة.
        return ($out === null || trim($out) === '') ? $name : trim($out);
    }

    /**
     * الوصلة التي تجلب الاسم — في دالّةٍ واحدة كي لا تختلف الشاشتان.
     *
     * كتابتُها مرّتين هو ما جعل القائمة تعرض «وكيل #104» والترويسة تعرض
     * فراغاً في أوّل نسخة.
     */
    public static function joinAgentIdentity($q, string $usersAlias = 'u')
    {
        return $q->leftJoin('AccountsTb as acc', 'acc.AccID', '=', "$usersAlias.AccID");
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
            // التصنيف يأتي مع الصفّ لا في نداءٍ ثانٍ: وصلةٌ على مفتاحٍ
            // أساسيّ في جدولٍ من عشرة صفوف، وثمنُها صفر.
            ->leftJoin('support_categories as cat', 'cat.id', '=', 's.category_id')
            ->where('t.kind', ChatService::ADMIN);

        self::joinAgentIdentity($q);

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
            // البحث على الاسم المعروض نفسه: من يكتب «الأمانة» يبحث عمّا
            // يراه في الشاشة، وبحثٌ لا يجد ما يعرضه أسوأ من غياب البحث.
            $q->where(fn ($w) => $w->where('acc.AccName', 'like', $like)
                                   ->orWhere('u.name', 'like', $like)
                                   ->orWhere('u.phone', 'like', $like));
        }

        // ── الترتيب: الأولوية أوّلاً ثم الأحدث ─────────────────────────
        //
        // الأولوية بلا فرزٍ زينةٌ: موظّفٌ يرى «حرجة» في الصفّ الأربعين لن
        // يصل إليها. و`CASE` صريحةٌ لا وصلةٌ بجدول رتب — أربعُ قيمٍ ثابتة
        // بطبيعتها، ووصلةٌ لها ثمنُ رحلةٍ بلا مقابل.
        //
        // والأحدثُ ثانياً داخل كل درجة: بين حرجتين، الأقدمُ انتظاراً أولى.
        $rows = $q->orderByRaw(
                "CASE s.priority WHEN 'CRITICAL' THEN 0 WHEN 'URGENT' THEN 1
                                 WHEN 'HIGH' THEN 2 ELSE 3 END")
            ->orderByRaw('CASE WHEN t.last_message_at IS NULL THEN 1 ELSE 0 END')
            ->orderByDesc('t.last_message_at')
            ->limit(300)
            ->get([
                't.id', 't.agent_id', 't.last_message_at',
                'u.name as user_name', 'u.phone as agent_phone',
                'acc.AccName as acc_name',
                's.status', 's.assigned_to', 's.assigned_at',
                's.priority', 's.reference', 's.category_id',
                // أزمنةُ SLA تُقرأ مع الصفّ — الحالةُ تُحسب منها في PHP بلا
                // رحلةٍ إضافية (انظر `SupportSla::evaluate`).
                's.first_agent_msg_at', 's.first_reply_at',
                's.last_agent_msg_at', 's.last_reply_at', 's.resolved_at',
                's.snoozed_until',
                'a.name as assignee_name',
                'cat.name as category_name', 'cat.color as category_color',
            ]);

        $ids = $rows->pluck('id')->map(fn ($v) => (int) $v)->all();
        if ($ids === []) {
            return [];
        }

        // غيرُ المقروء وآخرُ رسالةٍ والوسوم: ثلاثةُ استعلاماتٍ للصفحة كلّها،
        // لا ثلاثةٌ لكلّ صفّ — النمط المقرَّر في هذا المشروع.
        $unread = $this->chat->unreadByThread($ids, ChatService::ADMIN, 0);
        $last   = $this->lastMessages($ids);
        $tags   = app(SupportOps::class)->tagsForThreads($ids);
        // كائنٌ واحد للصفحة كلّها: يقرأ الإعدادات مرّةً ثم يحسب في الذاكرة.
        $sla    = app(SupportSla::class);

        return $rows->map(function ($r) use ($unread, $last, $tags, $sla) {
            $tid = (int) $r->id;
            $lm  = $last[$tid] ?? null;
            $pri = $r->priority ?: SupportOps::NORMAL;

            return [
                'id'            => $tid,
                'agent_id'      => (int) $r->agent_id,
                'agent_name'    => self::agentName(
                    $r->acc_name, $r->user_name, (int) $r->agent_id),
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

                // ── التشغيل (بنود 3 · 4 · 12) ──────────────────────
                'priority'       => $pri,
                'priority_label' => SupportOps::PRIORITIES[$pri]['label'],
                'priority_color' => SupportOps::PRIORITIES[$pri]['color'],
                'priority_rank'  => SupportOps::PRIORITIES[$pri]['rank'],
                'reference'      => $r->reference,
                'category_id'    => $r->category_id ? (int) $r->category_id : null,
                'category_name'  => $r->category_name,
                'category_color' => $r->category_color,
                'tags'           => $tags[$tid] ?? [],

                // ── SLA (البند 2) ───────────────────────────────────
                'sla' => $sla->evaluate($r),
                'snoozed_until' => $r->snoozed_until ? (string) $r->snoozed_until : null,
            ];
        })->pipe(function ($list) use ($f) {
            /*
             * ── الفرز بأقرب SLA (البند 2) ─────────────────────────────
             *
             * نصُّ البند: «حتى لا تبقى محادثة وكيل بدون متابعة». والفرزُ
             * في PHP لا في SQL: الحالةُ دالّةٌ في الوقت الحالي وتُحسب بعد
             * القراءة — و`ORDER BY` لا يرى ما لم يُخزَّن.
             *
             * والصفحةُ ثلاثمئة صفٍّ على الأكثر، فترتيبُها في الذاكرة
             * يُقاس بأجزاء المللي.
             *
             * والمتأخّرُ أوّلاً ثم الأقربُ إلى التأخير: الترتيبُ يجيب عن
             * «بمن أبدأ؟» لا عن «ما الأقدم؟».
             */
            if (($f['sort'] ?? '') !== 'sla') {
                return $list;
            }

            $rank = [
                SupportSla::BREACHED => 0,
                SupportSla::WARNING  => 1,
                SupportSla::OK       => 2,
                SupportSla::NONE     => 3,
            ];

            return $list->sortBy(function ($t) use ($rank) {
                $s = $t['sla'];
                return [
                    $rank[$s['state']] ?? 9,
                    // داخل الدرجة: الأقلُّ وقتاً متبقّياً أوّلاً. والمتأخّرُ
                    // متبقّيه سالبٌ فيسبق تلقائياً.
                    $s['remaining_min'] ?? PHP_INT_MAX,
                ];
            })->values();
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

    /**
     * كلُّ ما تحتاجه شاشة المحادثة عن سياقها — في **استعلامٍ واحد**.
     *
     * كان أربعة منفصلة في كل نبضة: المحادثة (للتأكّد من `kind = ADMIN`)،
     * وصفُّ الحالة (لفحص الإسناد)، وبيانات الوكيل، وصفُّ الحالة **ثانيةً**
     * مع اسم المُسنَد إليه. والقاعدة بعيدة فكلُّ رحلةٍ ~45 مللي، فذلك وحده
     * كان يكلّف نحو 180 مللي قبل قراءة رسالةٍ واحدة.
     *
     * وكلُّها وصلاتٌ على مفاتيحَ أساسية، فالاستعلام المدمج لا يكلّف أكثر من
     * أصغرها — الثمن هو عدد الرحلات لا حجمها.
     *
     * يُرجع `null` إن لم توجد المحادثة أو لم تكن محادثةَ إدارة — وهو
     * الفحصُ الأمنيّ نفسه الذي كان في `thread()`، في مكانه.
     */
    public function context(int $threadId): ?object
    {
        $q = DB::table('chat_threads as t')
            ->leftJoin('users as u', 'u.id', '=', 't.agent_id')
            ->leftJoin('support_thread_state as s', 's.thread_id', '=', 't.id')
            ->leftJoin('support_staff as a', 'a.id', '=', 's.assigned_to')
            ->leftJoin('support_categories as cat', 'cat.id', '=', 's.category_id')
            // «يكتب الآن» يُقرأ هنا أيضاً: صفٌّ واحد على الأكثر، ووصلةٌ
            // على `thread_id` أرخص من رحلةٍ خامسة.
            ->leftJoin('chat_typing as ty', function ($j) {
                $j->on('ty.thread_id', '=', 't.id')
                  ->where('ty.actor_kind', '!=', ChatService::ADMIN)
                  ->where('ty.expires_at', '>', now());
            })
            ->where('t.id', $threadId)
            ->where('t.kind', ChatService::ADMIN);

        self::joinAgentIdentity($q);

        return $q->first([
            't.id', 't.kind', 't.agent_id',
            'u.name as user_name', 'u.phone as agent_phone',
            'acc.AccName as acc_name',
            's.status', 's.assigned_to', 's.close_note',
            's.priority', 's.reference', 's.category_id',
            /*
             * أزمنةُ الاستجابة — يُقرأن من الصّف نفسِه بلا رحلةٍ زائدة.
             *
             * ⚠ وغيابُها لا يُخطِئ: `evaluate` ترى NULL فتقول «لا مهلة»،
             * فلا تظهر الشّارة في المحادثة أبداً ولا يظهر خطأ. وقد
             * وقع ذلك فعلاً، ولم يكشفه إلّا فتحُ الشّاشة بالعين.
             */
            's.first_agent_msg_at', 's.first_reply_at',
            's.last_agent_msg_at', 's.last_reply_at', 's.resolved_at',
            'a.name as assignee_name',
            'cat.name as category_name', 'cat.color as category_color',
            'ty.actor_name as typing_name', 'ty.state as typing_state',
            // رقمُ آخر رسالة — يُغني عن استعلامٍ ثانٍ، وعن استعلام الرسائل
            // نفسِه حين لا يكون ثمّة جديد (وهي حال أغلب النبضات).
            //
            // ⚠ واستعلامٌ فرعيّ هنا **آمن** خلافاً لما في `InternalEx`:
            // الشرط على `chat_messages.thread_id` وعليه فهرسٌ
            // (`IX_chat_messages_thread`)، والمحادثة واحدة — فهو بحثٌ في
            // الفهرس لا مسحٌ للجدول، ولا يتكرّر لكل صفّ لأن الصفّ واحد.
            DB::raw('(SELECT MAX(id) FROM chat_messages WHERE thread_id = t.id) AS max_msg_id'),
        ]);
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

        $prevName = $cur->assigned_to
            ? DB::table('support_staff')->where('id', $cur->assigned_to)->value('name')
            : null;

        app(SupportOps::class)->event(
            $threadId,
            $toStaffId === null ? SupportOps::EV_UNASSIGNED : SupportOps::EV_ASSIGNED,
            $by,
            $prevName,
            $toStaffId === null ? null : ($target->name ?? null),
        );

        return ['ok' => true, 'assignee' => $toStaffId === null ? null : ($target->name ?? null)];
    }

    public function setStatus(int $threadId, string $status, object $by, ?string $note = null): array
    {
        if (!self::statusExists($status)) {
            return ['error' => 'حالة غير معروفة.'];
        }

        // الحالةُ السابقة تُقرأ **قبل** الكتابة: الشريط الزمني يقول «من ⇦
        // إلى»، وقراءتُها بعدها تعطي «من الجديدة إلى الجديدة».
        $cur = $this->ensureState($threadId)->status;

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

        // زمنُ الحلّ: يُختم بالإغلاق ويُمسح بإعادة الفتح — حالةٌ عادت
        // ليست محلولة، وإبقاءُ الختم يجعل «متوسّط المعالجة» يكذب.
        $sla = app(SupportSla::class);
        if ($status === self::CLOSED) {
            $sla->onResolved($threadId);
        } else {
            $sla->onReopened($threadId);
        }

        // والشريط الزمني يسجّل التغيّر (البند 17).
        app(SupportOps::class)->event(
            $threadId,
            $status === self::CLOSED ? SupportOps::EV_CLOSED
                : ($cur === self::CLOSED ? SupportOps::EV_REOPENED : SupportOps::EV_STATUS),
            $by,
            self::STATUSES[$cur] ?? $cur,
            self::STATUSES[$status],
            $note ?: null,
        );

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

            /*
             * ⚠ صفُّ الحالة يُنشأ هنا إن لم يوجد — قبل أي شيء آخر.
             *
             * أزمنةُ SLA تُكتب في هذا الصفّ، ومحادثةٌ لم يلمسها الدعم بعد
             * لا صفَّ لها. فبغير الإنشاء تضيع لحظةُ **أوّل رسالةٍ من
             * وكيل** — وهي بالضبط بدايةُ عدّاد «أوّل ردّ»، أي المقياس الذي
             * وُجد النظام ليحرسه.
             */
            if (!$row) {
                $this->ensureState($threadId);
            }
            app(SupportSla::class)->onAgentMessage($threadId);

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

            // زمنُ الردّ يُسجَّل ولو كانت الحالة مغلقة: ردٌّ بعد الإغلاق
            // ردٌّ، وإخفاؤه من القياس يجمّل الأرقام.
            app(SupportSla::class)->onSupportReply($threadId);

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
