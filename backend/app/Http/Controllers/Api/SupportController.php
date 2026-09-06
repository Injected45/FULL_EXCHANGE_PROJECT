<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\ChatService;
use App\Services\Support\SupportAudit;
use App\Services\Support\SupportPermissions;
use App\Services\Support\SupportDashboard;
use App\Services\Support\SupportOps;
use App\Services\Support\SupportPresence;
use App\Services\Support\SupportSla;
use App\Services\Support\SupportStaffService;
use App\Services\Support\SupportThreadService;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Storage;

/**
 * مركز «الرحالة للدعم الفني» — واجهةُ الخادم لتطبيق React.
 *
 * ── ما يُعاد استعماله وما يُضاف ────────────────────────────────────────────
 *
 * الرسائلُ والمرفقاتُ والتفاعلاتُ والاقتباسُ والتثبيتُ والنجومُ والبحثُ
 * و«يكتب الآن» وإيصالاتُ القراءة كلُّها تمرّ بـ {@see ChatService} — **الصنف
 * نفسه** الذي يستعمله تطبيق الوكيل وتطبيق الموظّف. لا نسخةَ ثانية من منطق
 * الدردشة، ولا جدولَ رسائلَ ثانٍ، ولا بروتوكولَ مختلف (أمرُ المالك، البند
 * 13: «لا تبنِ Backend Chat جديداً»).
 *
 * والجديدُ ثلاثةٌ فقط، وكلُّها فوق الدردشة لا داخلها: **من موظّفو الدعم**،
 * و**من يملك أن يفعل ماذا**، و**أين وصلت المحادثة**.
 *
 * ── عزلُ الوكلاء بعضهم عن بعض ─────────────────────────────────────────────
 *
 * كلُّ نداءٍ يخصّ محادثةً يمرّ بـ {@see self::thread()}، وهي تشترط
 * `kind = ADMIN`. فمحادثةُ الوكيل مع **موظّفه** لا تُفتح من هنا أبداً — لا
 * بصلاحية ولا بدور ولا برقمٍ مخمَّن. تلك مراسلاتٌ بين الوكيل ومن يعمل عنده،
 * ولا شأن للدعم بها.
 *
 * ── ⚠ الخطّ الأحمر المالي ────────────────────────────────────────────────
 *
 * لا نقطةَ في هذا الملفّ تقرأ أو تكتب رصيداً أو حوالةً أو قيداً أو عمولةً أو
 * خزينة. موظّف الدعم يقرأ رسائلَ الوكيل ويردّ عليها — لا يرى ميزانيته ولا
 * يحرّك درهماً. ورقمُ حوالةٍ يكتبه وكيلٌ في رسالة يبقى **نصّاً في رسالة**،
 * لا يُفسَّر ولا يُنفَّذ ولا يُربط بشيء.
 */
class SupportController extends BaseController
{
    public function __construct(
        private ChatService $chat,
        private SupportThreadService $threads,
        private SupportStaffService $staffSvc,
        private SupportAudit $audit,
        private SupportOps $ops,
    ) {
    }

    private function me(Request $r): object
    {
        return $r->attributes->get('support_staff');
    }

    private function can(Request $r, string $key): bool
    {
        $me = $this->me($r);

        return SupportPermissions::allowedForRole($me->role, $key)
            && in_array($key, $me->permissions, true);
    }

    /**
     * رفضٌ داخل المتحكّم — يُسجَّل كما يُسجِّل الوسيط رفضَه.
     *
     * صلاحيتان لا يستطيع الوسيط الفصل بينهما لأن التمييز يحتاج جسم الطلب
     * («لنفسي» أم «لغيري»؟ «إغلاق» أم «تغيير حالة»؟) — ففُحصت هنا. وكان
     * ذلك يعني أن رفضَها **لا يظهر في السجلّ** بينما تظهر بقية أنواع
     * الرفض: سجلٌّ يُظهر ستّ محاولاتٍ من ثمانٍ يجعل من يقرؤه يطمئنّ في
     * غير موضعه.
     */
    private function deny(Request $r, string $permission, string $message, ?int $threadId = null)
    {
        $this->audit->log($this->me($r), 'DENIED', $threadId, $permission,
            'محاولة وصول بلا صلاحية', $r->ip());

        return $this->sendError($message, [], 403);
    }

    /**
     * المحادثة، بشرط أن تكون محادثةَ إدارة — وإلا فلا وجود لها.
     *
     * ⚠ هذا هو موضعُ منعِ الوصول إلى محادثات الوكيل مع موظّفيه. شرطٌ واحد
     * يُنسى هنا يكشفها كلَّها.
     */
    private function thread(int $id): ?object
    {
        $t = DB::table('chat_threads')->where('id', $id)->first();

        return ($t && $t->kind === ChatService::ADMIN) ? $t : null;
    }

    /**
     * هل يجوز لهذا الموظّف أن يفتح هذه المحادثة؟
     *
     * من يملك «عرض كل المحادثات» يفتح ما شاء. ومن لا يملكها يفتح المُسنَدة
     * إليه وغيرَ المُسنَدة — وغيرُ المُسنَدة عمداً، وإلا لما استطاع أن يستلم
     * شيئاً أصلاً.
     */
    private function mayOpen(Request $r, int $threadId): bool
    {
        if ($this->can($r, 'VIEW_ALL_THREADS')) {
            return true;
        }

        $st = DB::table('support_thread_state')->where('thread_id', $threadId)
            ->first(['assigned_to']);

        return $st === null
            || $st->assigned_to === null
            || (int) $st->assigned_to === (int) $this->me($r)->id;
    }

    // ══════════════════════════════════════════════════════════════════
    //  صندوق الوارد
    // ══════════════════════════════════════════════════════════════════

    /** GET support/threads?scope=&status=&q= */
    public function threads(Request $r)
    {
        return $this->sendResponse([
            'items'     => $this->threads->threads($this->me($r), [
                'scope'  => (string) $r->query('scope', 'all'),
                'status' => (string) $r->query('status', ''),
                'q'      => (string) $r->query('q', ''),
                // `sla` يفرز بأقرب مهلة (البند 2)، وغيرُه يُبقي الترتيب
                // الافتراضي: الأولوية ثم الأحدث.
                'sort'   => (string) $r->query('sort', ''),
            ]),
            'stats'     => $this->threads->stats($this->me($r)),
            'statuses'  => SupportThreadService::STATUSES,
        ], 'Success');
    }

    /**
     * GET support/threads/unread
     *
     * نقطةٌ صغيرة منفصلة عن `threads` عمداً: الشاشةُ تسألها كلَّ بضع ثوانٍ
     * لتُظهر العدّاد وتُصدر الصوت، و`threads` حمولتُها صفوفٌ كاملة. هذا هو
     * القرارُ نفسه المتّخذ في جرس الحوالات الواردة.
     */
    public function unread(Request $r)
    {
        // ── استعلامٌ واحد ─────────────────────────────────────────────
        //
        // كانت ثلاثة: أرقامُ المحادثات، ثم علاماتُ القراءة، ثم شرطٌ مركَّب
        // يُبنى منها بطول عدد المحادثات. والقاعدة بعيدةٌ فكلُّ رحلةٍ ~45
        // مللي، وهذه النقطة تُنادى كل ثوانٍ قليلة من كل لسانٍ مفتوح.
        //
        // والوصلة هنا **لا** تُشبه الاستعلام الفرعيّ لكل صفّ: العدُّ يجري
        // في الخادم مرّةً واحدة على المحادثات كلِّها، لا مرّةً لكلّ محادثة.
        $rows = DB::select(
            'SELECT m.thread_id AS tid, COUNT(*) AS n
               FROM chat_messages m
               JOIN chat_threads t
                 ON t.id = m.thread_id AND t.kind = ?
               LEFT JOIN chat_reads r
                 ON r.thread_id = m.thread_id
                AND r.reader_kind = ? AND r.reader_id = 0
              WHERE m.sender_kind <> ?
                AND m.deleted_at IS NULL
                AND m.id > ISNULL(r.last_read_message_id, 0)
              GROUP BY m.thread_id',
            [ChatService::ADMIN, ChatService::ADMIN, ChatService::ADMIN]
        );

        $total = 0;
        $threads = [];
        foreach ($rows as $row) {
            $total += (int) $row->n;
            // أرقامُ المحادثات وحدها: تكفي الواجهةَ لتعرف **أيّها** جديد،
            // بلا نقل نصوصٍ لن تُعرض في العدّاد.
            $threads[] = (int) $row->tid;
        }

        return $this->sendResponse(['total' => $total, 'threads' => $threads], 'Success');
    }

    /** GET support/threads/{id}?after_id= */
    public function messages(Request $r, int $id)
    {
        // ── سياقُ المحادثة كلُّه في استعلامٍ واحد ─────────────────────
        //
        // كانت هذه النقطة تُنفّذ نحو ستّ عشرة رحلةً إلى القاعدة في كل نبضة،
        // والقاعدة بعيدةٌ فكلُّ رحلةٍ ~45 مللي: **710 مللي لترجع 523 بايت**
        // (مقيسة). المحتوى نفسه بلا نقصان، والرحلات أقلّ.
        $ctx = $this->threads->context($id);

        if (!$ctx) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        // الإسناد جاء مع السياق، فلا حاجة إلى `mayOpen` باستعلامها الخاصّ.
        if (!$this->can($r, 'VIEW_ALL_THREADS')
            && $ctx->assigned_to !== null
            && (int) $ctx->assigned_to !== (int) $this->me($r)->id) {
            return $this->sendError('هذه المحادثة مُسنَدة إلى موظّف آخر.', [], 403);
        }

        $after = max(0, (int) $r->query('after_id', 0));
        $first = ($after === 0);
        $maxId = (int) ($ctx->max_msg_id ?? 0);

        // ⚠ لا يُسأل عن الرسائل حين لا يكون ثمّة جديد.
        //
        // النبضة تسأل `after_id=<آخر ما عندي>`، و`max_msg_id` جاء مع
        // السياق — فمقارنةُ رقمين في PHP تُغني عن رحلةٍ إلى قاعدةٍ بعيدة.
        // وهذه حالُ أغلب النبضات: صامتةٌ لا جديد فيها.
        //
        // والسلوك مطابق: الاستعلام المحذوف كان شرطُه `id > after_id`، وهو
        // لا يُرجع شيئاً حين `max <= after`.
        // ⚠ الملاحظات الداخلية تُطلب صراحةً ولمن يملكها وحده — والافتراض
        // في `messages` إخفاؤها، فمن ينسى هنا يحصل على السلوك الآمن.
        $seeNotes = $this->can($r, 'INTERNAL_NOTES');

        $items = (!$first && $maxId <= $after)
            ? []
            : $this->chat->messages($id, $after, 50, $seeNotes);

        // «وصلت» مع كل نبضة، و«قُرئت» عند فتح الشاشة أو وصول جديد — الترتيب
        // نفسه المعتمد في تطبيق الوكيل، ولسببه نفسه. ورحلةٌ واحدة بدل ستّ.
        $receipts = $this->chat->syncReceipts(
            $id, ChatService::ADMIN, 0, $first || $items !== [], $maxId);

        $ids = array_map(fn ($m) => (int) $m->id, $items);

        // اسمُ من ردّ فعلاً: `sender_id` تبقى 0 لأن الإدارة طرفٌ واحد في
        // نظر الوكيل (انظر `support_center.sql`)، والهويّة في عمودٍ مستقلّ.
        $this->attachStaffNames($items);

        return $this->sendResponse([
            'items'     => $items,
            'receipts'  => $receipts,
            // ⚠ تُسأل عن الرسائل الواصلة وحدها. النبضة تسأل
            // `after_id=<آخر>` فتعود بلا رسائل في أغلب الأحيان، واستعلامان
            // عن تفاعلاتِ لا شيء ثمنُهما 90 مللي في كل مرّة. والواجهة تدمج
            // ولا تستبدل، فما جاء عند الفتح يبقى.
            'reactions' => $ids !== [] ? $this->chat->reactionsFor($ids, ChatService::ADMIN, 0) : [],
            'starred'   => $ids !== [] ? $this->chat->starredIn($ids, ChatService::ADMIN, 0) : [],
            // «يكتب الآن» جاءت مع السياق في الوصلة نفسها.
            'typing'    => $ctx->typing_state
                ? ['actor_name' => $ctx->typing_name, 'state' => $ctx->typing_state]
                : null,
            // المثبَّتة تتغيّر نادراً: تُقرأ عند الفتح، وكلُّ تثبيتٍ أو إلغاءٍ
            // يُعيد القراءة من الصفر — فلا شيء يفوت.
            'pinned'       => $first ? $this->chat->pinnedIn($id) : null,
            'pinned_known' => $first,
            'agent'     => [
                'id'    => (int) $ctx->agent_id,
                'name'  => SupportThreadService::agentName(
                    $ctx->acc_name, $ctx->user_name, (int) $ctx->agent_id),
                'phone' => $ctx->agent_phone,
            ],
            'state'     => [
                'status'        => $ctx->status ?? SupportThreadService::NEW,
                'status_label'  => SupportThreadService::STATUSES[$ctx->status ?? SupportThreadService::NEW],
                'assigned_to'   => $ctx->assigned_to !== null ? (int) $ctx->assigned_to : null,
                'assignee_name' => $ctx->assignee_name,
                'close_note'    => $ctx->close_note,

                // ── التشغيل (بنود 3 · 4 · 12) ──────────────────────
                'priority'       => $pri = ($ctx->priority ?: SupportOps::NORMAL),
                'priority_label' => SupportOps::PRIORITIES[$pri]['label'],
                'priority_color' => SupportOps::PRIORITIES[$pri]['color'],
                // الرقمُ المرجعي يُولَّد عند أوّل فتحٍ للحالة لا عند
                // إنشائها: محادثاتٌ لم يفتحها أحد لا تحتاج مرجعاً يُملى.
                'reference'      => $first ? $this->ops->ensureReference($id) : $ctx->reference,
                'category_id'    => $ctx->category_id ? (int) $ctx->category_id : null,
                'category_name'  => $ctx->category_name,
                'category_color' => $ctx->category_color,
                'tags'           => $this->ops->tagsForThreads([$id])[$id] ?? [],
            ],
            // هل يرى هذا الموظّف الملاحظات الداخلية؟ الواجهة ترسم زرَّها
            // بناءً عليه — والرفضُ الحقيقي في الخادم على أي حال.
            'can_internal' => $seeNotes,

            // ── الدفعة الثانية ──────────────────────────────────────
            //
            // حالةُ SLA تُحسب من الصفّ المقروء أصلاً — بلا رحلةٍ إضافية.
            'sla' => app(SupportSla::class)->evaluate($ctx),

            /*
             * ⚠ من غيري يشاهد الآن (البند 6).
             *
             * والنبضةُ تُسجَّل هنا لا في مسارٍ منفصل: قراءةُ المحادثة **هي**
             * دليلُ أن الموظّف ينظر إليها. ومسارٌ ثانٍ يعني نداءً إضافياً
             * كلَّ ثانيتين، ونسيانَه يعني حضوراً لا يُرى.
             */
            'viewers' => (function () use ($id, $r) {
                $p = app(SupportPresence::class);
                $p->touchViewer($id, $this->me($r));
                return $p->othersViewing($id, (int) $this->me($r)->id);
            })(),
        ], 'Success');
    }

    /**
     * أسماءُ موظّفي الدعم على رسائلهم — استعلامٌ واحد للصفحة كلّها.
     *
     * ولا يُلمس `sender_name` القديم: رسائلُ ما قبل الحسابات تحمل ما كتبه
     * صاحبُها بيده، وهي شهادةٌ على لحظتها كما هي.
     */
    private function attachStaffNames(array $items): void
    {
        $staffIds = [];
        foreach ($items as $m) {
            if (!empty($m->support_staff_id)) {
                $staffIds[] = (int) $m->support_staff_id;
            }
        }

        if ($staffIds === []) {
            return;
        }

        $names = DB::table('support_staff')->whereIn('id', array_unique($staffIds))
            ->pluck('name', 'id');

        foreach ($items as $m) {
            if (!empty($m->support_staff_id)) {
                $m->staff_name = $names[(int) $m->support_staff_id] ?? null;
            }
        }
    }

    // ══════════════════════════════════════════════════════════════════
    //  الردّ
    // ══════════════════════════════════════════════════════════════════

    /** POST support/threads/{id}/messages */
    public function send(Request $r, int $id)
    {
        if (!$this->thread($id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }
        if (!$this->mayOpen($r, $id)) {
            return $this->sendError('هذه المحادثة مُسنَدة إلى موظّف آخر.', [], 403);
        }

        $me = $this->me($r);

        // ── ملاحظةٌ داخلية أم ردٌّ على الوكيل؟ (البند 7) ──────────────
        //
        // ⚠ صلاحيةٌ مستقلّة: من يردّ على الوكيل ليس بالضرورة من يُطلعه
        // الفريقُ على مداولاته. والفحصُ هنا قبل أي كتابة.
        $isInternal = (bool) $r->input('internal', false);
        if ($isInternal && !$this->can($r, 'INTERNAL_NOTES')) {
            return $this->deny($r, 'INTERNAL_NOTES',
                'لا تملك صلاحية الملاحظات الداخلية.', $id);
        }
        // ولا يُشترط `REPLY` للملاحظة: هي ليست رداً على الوكيل أصلاً.
        if (!$isInternal && !$this->can($r, 'REPLY')) {
            return $this->deny($r, 'REPLY', 'لا تملك صلاحية الردّ.', $id);
        }

        $attachment = [];
        if ($r->hasFile('attachment')) {
            $file = $r->file('attachment');
            $mime = (string) $file->getMimeType();
            $kind = ChatService::MIMES[$mime][0] ?? null;

            // صلاحيتان لا واحدة: إرسالُ صورةٍ وإرسالُ صوتٍ يُمنحان على حدة،
            // فقد يُراد لموظّفٍ أن يُرسل مستنداً ولا يُرسل تسجيلاً.
            $need = $kind === 'AUDIO' ? 'SEND_VOICE' : 'SEND_ATTACHMENT';
            if (!$this->can($r, $need)) {
                return $this->deny($r, $need, 'لا تملك صلاحية إرسال هذا النوع.', $id);
            }

            $attachment = $this->chat->storeAttachment($file) ?? [];
            if ($attachment === []) {
                return $this->sendError(
                    'المرفق غير مقبول: النوع غير مدعوم أو الحجم أكبر من '
                        . (ChatService::MAX_ATTACHMENT / 1048576) . ' ميغابايت.',
                    [], 422
                );
            }
        }

        $replyTo = (int) $r->input('reply_to_id', 0);

        $msg = $this->chat->send(
            $id,
            ChatService::ADMIN,
            // ‏0 عمداً — انظر `support_center.sql`: الإدارة طرفٌ واحد في نظر
            // الوكيل، ورقمٌ لكلّ موظّف كان يقسّم حالةَ القراءة عليهم فيرى
            // الوكيل شرطتيه تتراجعان كلّما ردّ عليه زميلٌ آخر.
            0,
            $me->name,
            (string) $r->input('body', ''),
            $attachment,
            $replyTo > 0 ? $replyTo : null,
            mb_substr((string) $r->input('client_id', ''), 0, 64) ?: null,
            $isInternal,
        );

        if (!$msg) {
            return $this->sendError(
                'الرسالة فارغة أو أطول من ' . ChatService::MAX_BODY . ' حرفاً.',
                [], 422
            );
        }

        // من ردّ فعلاً — في عمودٍ مستقلّ، بلا مساسٍ بمنطق الدردشة.
        DB::table('chat_messages')->where('id', $msg->id)
            ->update(['support_staff_id' => $me->id]);
        $msg->support_staff_id = (int) $me->id;
        $msg->staff_name = $me->name;

        // ⚠ الملاحظة الداخلية لا تُغيّر حالة المحادثة.
        //
        // `onSupportReply` تنقلها إلى «بانتظار الوكيل» — وذلك كذبٌ حين لا
        // يكون الوكيل قد رأى شيئاً. ملاحظةُ «تواصلتُ معه هاتفياً» لا تجعل
        // الكرةَ في ملعبه.
        if (!$isInternal) {
            $this->threads->onSupportReply($id, $me);
            $this->audit->log($me, SupportAudit::REPLY, $id, null, null, $r->ip());
        } else {
            // تُسجَّل في الشريط الزمني لا في نصّها: الشريط يقول «كُتبت
            // ملاحظة» ومن كتبها ومتى — ونصُّها في المحادثة لمن يملك قراءتَها.
            $this->ops->event($id, SupportOps::EV_NOTE, $me);
            $this->audit->log($me, 'NOTE', $id, null, null, $r->ip());
        }

        return $this->sendResponse(['message' => $msg], $isInternal ? 'حُفظت الملاحظة.' : 'تم الإرسال.');
    }

    /**
     * GET support/attachment/{name}
     *
     * ⚠ المرفقُ يُقدَّم لمن يجوز له فتحُ محادثته، لا لكلّ من سجّل دخوله.
     * الاسمُ عشوائيّ بأربعةٍ وعشرين محرفاً، **والعشوائية ليست تصريحاً**.
     */
    public function attachment(Request $r, string $name)
    {
        if (basename($name) !== $name) {
            return $this->sendError('اسم غير صالح.', [], 404);
        }

        $row = DB::table('chat_messages as m')
            ->join('chat_threads as t', 't.id', '=', 'm.thread_id')
            ->where('m.attachment_path', $name)
            ->whereNull('m.deleted_at')
            ->where('t.kind', ChatService::ADMIN)
            ->first(['m.thread_id', 'm.attachment_mime', 'm.attachment_name']);

        if (!$row || !$this->mayOpen($r, (int) $row->thread_id)) {
            return $this->sendError('المرفق غير موجود.', [], 404);
        }

        $path = ChatService::DIR . '/' . $name;
        if (!Storage::disk(ChatService::DISK)->exists($path)) {
            return $this->sendError('المرفق غير موجود.', [], 404);
        }

        return response(Storage::disk(ChatService::DISK)->get($path), 200, [
            'Content-Type'        => $row->attachment_mime ?: 'application/octet-stream',
            'Content-Disposition' => 'inline; filename="' . rawurlencode((string) $row->attachment_name) . '"',
            'Cache-Control'       => 'private, max-age=86400',
        ]);
    }

    // ══════════════════════════════════════════════════════════════════
    //  مزايا الرسالة — كلُّها عبر ChatService
    // ══════════════════════════════════════════════════════════════════

    /** POST support/threads/{id}/messages/{mid}/react  {emoji} */
    public function react(Request $r, int $id, int $mid)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $this->chat->react($mid, $id, ChatService::ADMIN, 0, (string) $r->input('emoji', ''));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** PUT support/threads/{id}/messages/{mid}  {body} */
    public function edit(Request $r, int $id, int $mid)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $err = $this->chat->editMessage($mid, $id, ChatService::ADMIN, 0, (string) $r->input('body', ''));

        if ($err !== null) {
            return $this->sendError($err, [], 422);
        }

        $this->audit->log($this->me($r), SupportAudit::EDIT, $id, (string) $mid, null, $r->ip());

        return $this->sendResponse(['ok' => true], 'تم التعديل.');
    }

    /** POST support/threads/{id}/messages/{mid}/pin  {days} */
    public function pin(Request $r, int $id, int $mid)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $days = (int) $r->input('days', 7);
        $this->chat->pinMessage($mid, $id, ChatService::ADMIN, $days);
        $this->audit->log($this->me($r), SupportAudit::PIN, $id, (string) $mid,
            $days > 0 ? "لمدة {$days} يوماً" : 'إلغاء التثبيت', $r->ip());

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** POST support/threads/{id}/messages/{mid}/star  {on} */
    public function star(Request $r, int $id, int $mid)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $this->chat->star($mid, $id, ChatService::ADMIN, 0, (bool) $r->input('on', true));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** POST support/threads/{id}/messages/{mid}/forward  {to_thread_id} */
    public function forward(Request $r, int $id, int $mid)
    {
        $to = (int) $r->input('to_thread_id', 0);

        if (!$this->thread($id) || !$this->thread($to)
            || !$this->mayOpen($r, $id) || !$this->mayOpen($r, $to)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $me = $this->me($r);
        $msg = $this->chat->forward($mid, $id, $to, ChatService::ADMIN, 0, $me->name);

        if (!$msg) {
            return $this->sendError('تعذّرت إعادة التوجيه.', [], 422);
        }

        DB::table('chat_messages')->where('id', $msg->id)
            ->update(['support_staff_id' => $me->id]);

        $this->audit->log($me, SupportAudit::FORWARD, $id, (string) $mid, "إلى #{$to}", $r->ip());

        return $this->sendResponse(['message' => $msg], 'أُعيد التوجيه.');
    }

    /** POST support/threads/{id}/typing  {state} */
    public function typing(Request $r, int $id)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $this->chat->setTyping($id, ChatService::ADMIN, 0, $this->me($r)->name,
            (string) $r->input('state', 'TYPING'));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** GET support/search?q= */
    public function search(Request $r)
    {
        $term = trim((string) $r->query('q', ''));
        if (mb_strlen($term) < 2) {
            return $this->sendResponse(['items' => []], 'Success');
        }

        // البحثُ محصورٌ فيما يجوز له فتحُه — لا في كل الرسائل.
        $ids = array_map(
            fn ($t) => $t['id'],
            $this->threads->threads($this->me($r), ['include_closed' => true]),
        );

        return $this->sendResponse([
            'items' => $ids === []
                ? []
                : $this->chat->search($ids, $term, 40, $this->can($r, 'INTERNAL_NOTES')),
        ], 'Success');
    }

    // ══════════════════════════════════════════════════════════════════
    //  الإسناد والحالة
    // ══════════════════════════════════════════════════════════════════

    /** POST support/threads/{id}/assign  {staff_id|null} */
    public function assign(Request $r, int $id)
    {
        if (!$this->thread($id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $me = $this->me($r);
        $raw = $r->input('staff_id');
        $to  = ($raw === null || $raw === '') ? null : (int) $raw;

        // إسنادٌ لنفسه شيء، ولغيره شيءٌ آخر — والثاني صلاحيةُ مشرف.
        $needed = ($to !== null && $to === (int) $me->id) ? 'ASSIGN_SELF' : 'ASSIGN_OTHERS';
        if (!$this->can($r, $needed)) {
            return $this->deny($r, $needed, 'لا تملك صلاحية هذا الإسناد.', $id);
        }

        $out = $this->threads->assign($id, $to, $me);
        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        $this->audit->log($me, $to === null ? SupportAudit::UNASSIGN : SupportAudit::ASSIGN,
            $id, $to ? (string) $to : null, $out['assignee'] ?? null, $r->ip());

        return $this->sendResponse(['ok' => true], $to === null ? 'نُزع الإسناد.' : 'تم الإسناد.');
    }

    /** POST support/threads/{id}/status  {status, note} */
    public function status(Request $r, int $id)
    {
        if (!$this->thread($id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $status = (string) $r->input('status', '');
        $me     = $this->me($r);

        $needed = match ($status) {
            SupportThreadService::CLOSED => 'CLOSE_THREAD',
            SupportThreadService::OPEN   => 'REOPEN_THREAD',
            default                      => 'CHANGE_STATUS',
        };

        // إعادةُ الفتح تحتاج صلاحيتها إن كانت مغلقةً فعلاً؛ وإلا فتغييرُ حالة.
        if ($status === SupportThreadService::OPEN) {
            $cur = DB::table('support_thread_state')->where('thread_id', $id)->first(['status']);
            if (($cur->status ?? '') !== SupportThreadService::CLOSED) {
                $needed = 'CHANGE_STATUS';
            }
        }

        if (!$this->can($r, $needed)) {
            return $this->deny($r, $needed, 'لا تملك صلاحية هذا التغيير.', $id);
        }

        $out = $this->threads->setStatus($id, $status, $me, (string) $r->input('note', ''));
        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        $action = match ($status) {
            SupportThreadService::CLOSED => SupportAudit::CLOSE,
            SupportThreadService::OPEN   => SupportAudit::REOPEN,
            default                      => SupportAudit::STATUS,
        };

        $this->audit->log($me, $action, $id, $status,
            (string) $r->input('note', '') ?: null, $r->ip());

        return $this->sendResponse(['ok' => true], 'تم التحديث.');
    }

    /** GET support/assignees */
    public function assignees()
    {
        return $this->sendResponse(['items' => $this->staffSvc->assignees()], 'Success');
    }

    // ══════════════════════════════════════════════════════════════════
    //  التشغيل: الأولوية والتصنيف والوسوم والشريط الزمني
    //  (بنود المالك 3 · 4 · 12 · 17)
    // ══════════════════════════════════════════════════════════════════

    /** POST support/threads/{id}/priority  {priority} */
    public function priority(Request $r, int $id)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $me  = $this->me($r);
        $out = $this->ops->setPriority($id, (string) $r->input('priority', ''), $me);

        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        $this->audit->log($me, 'PRIORITY', $id, (string) $r->input('priority'), null, $r->ip());

        return $this->sendResponse(['ok' => true], 'تم التحديث.');
    }

    /** POST support/threads/{id}/category  {category_id|null} */
    public function category(Request $r, int $id)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $raw = $r->input('category_id');
        $out = $this->ops->setCategory(
            $id,
            ($raw === null || $raw === '') ? null : (int) $raw,
            $this->me($r),
        );

        return isset($out['error'])
            ? $this->sendError($out['error'], [], 422)
            : $this->sendResponse(['ok' => true], 'تم التصنيف.');
    }

    /** POST support/threads/{id}/tags  {tag_id}  ·  DELETE .../tags/{tagId} */
    public function addTag(Request $r, int $id)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $out = $this->ops->addTag($id, (int) $r->input('tag_id', 0), $this->me($r));

        return isset($out['error'])
            ? $this->sendError($out['error'], [], 422)
            : $this->sendResponse(['ok' => true], 'أُضيف الوسم.');
    }

    public function removeTag(Request $r, int $id, int $tagId)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $this->ops->removeTag($id, $tagId, $this->me($r));

        return $this->sendResponse(['ok' => true], 'أُزيل الوسم.');
    }

    /** GET support/threads/{id}/timeline */
    public function timeline(Request $r, int $id)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        return $this->sendResponse([
            'items' => $this->ops->timeline($id),
        ], 'Success');
    }

    /**
     * GET support/taxonomy — التصنيفات والوسوم والأولويات معاً.
     *
     * نداءٌ واحد لا ثلاثة: الواجهة تحتاجها كلَّها عند فتح الشاشة، وثلاثةُ
     * نداءاتٍ إلى قاعدةٍ بعيدة ثمنُها ثلاثُ رحلات — والقوائمُ صغيرةٌ ثابتة.
     */
    public function taxonomy(Request $r)
    {
        $all = $this->can($r, 'MANAGE_TAXONOMY');

        return $this->sendResponse([
            'categories' => $this->ops->categories(!$all),
            'tags'       => $this->ops->tags(!$all),
            'priorities' => SupportOps::PRIORITIES,
        ], 'Success');
    }

    /** POST support/taxonomy/categories  ·  POST support/taxonomy/tags */
    public function createCategory(Request $r)
    {
        $out = $this->ops->createCategory(
            (string) $r->input('name', ''), $r->input('color'), $this->me($r));

        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        $this->audit->log($this->me($r), 'TAXONOMY', null, 'CATEGORY',
            (string) $r->input('name'), $r->ip());

        return $this->sendResponse($out, 'أُضيف التصنيف.');
    }

    public function createTag(Request $r)
    {
        $out = $this->ops->createTag(
            (string) $r->input('name', ''), $r->input('color'), $this->me($r));

        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        $this->audit->log($this->me($r), 'TAXONOMY', null, 'TAG',
            (string) $r->input('name'), $r->ip());

        return $this->sendResponse($out, 'أُضيف الوسم.');
    }

    /** PUT support/taxonomy/categories/{id}  ·  .../tags/{id}  {is_active} */
    public function setCategoryActive(Request $r, int $id)
    {
        $this->ops->setCategoryActive($id, (bool) $r->input('is_active', true));
        return $this->sendResponse(['ok' => true], 'تم التحديث.');
    }

    public function setTagActive(Request $r, int $id)
    {
        $this->ops->setTagActive($id, (bool) $r->input('is_active', true));
        return $this->sendResponse(['ok' => true], 'تم التحديث.');
    }

    // ══════════════════════════════════════════════════════════════════
    //  الدفعة الثانية: لوحة القيادة · SLA · الحضور · منع التعارض
    //  (بنود المالك 1 · 2 · 6 · 36)
    // ══════════════════════════════════════════════════════════════════

    /** GET support/dashboard — البند 1 */
    public function dashboard(Request $r)
    {
        return $this->sendResponse(
            app(SupportDashboard::class)->build($this->me($r)), 'Success');
    }

    /** GET support/team — من يعمل على ماذا (البندان 35 و36) */
    public function team(Request $r)
    {
        return $this->sendResponse([
            'items'    => app(SupportPresence::class)->team(),
            'statuses' => SupportPresence::LABELS,
            'colors'   => SupportPresence::COLORS,
        ], 'Success');
    }

    /** POST support/me/presence  {presence} — البند 36 */
    public function setPresence(Request $r)
    {
        $me  = $this->me($r);
        $out = app(SupportPresence::class)
            ->setPresence((int) $me->id, (string) $r->input('presence', ''));

        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        return $this->sendResponse(['ok' => true], 'تم التحديث.');
    }

    /** GET support/sla  ·  PUT support/sla/{priority} — البند 2 */
    public function slaSettings()
    {
        return $this->sendResponse([
            'items' => app(SupportSla::class)->settingsForDisplay(),
        ], 'Success');
    }

    public function updateSla(Request $r, string $priority)
    {
        $me = $this->me($r);

        /*
         * ⚠ **لا يُمرَّر إلا ما أُرسل فعلاً.**
         *
         * كان الثلاثةُ تُمرَّر دائماً، والغائبُ منها `null` — و`null` في
         * `updateSettings` تعني «لا هدف». فتعديلُ مهلة أوّل الردّ وحدها كان
         * **يمحو مهلتَي الردّ التالي والمعالجة**، فتصير المحادثاتُ كلُّها
         * «لا هدف» ولا يُنبَّه أحدٌ على تأخيرٍ أبداً.
         *
         * وكشفه الاختبار: قياسُ «الردّ التالي» عاد فارغاً بعد تعديل مهلةٍ
         * لا علاقة له بها.
         */
        $vals = [];
        foreach (['first' => 'first_minutes', 'next' => 'next_minutes',
                  'resolve' => 'resolve_minutes'] as $key => $field) {
            if ($r->has($field)) {
                $vals[$key] = $r->input($field);
            }
        }

        $out = app(SupportSla::class)->updateSettings($priority, $vals, $me);

        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        // ⚠ يُسجَّل: تغييرُ المعيار الذي يُقاس به الفريق قرارٌ إداريّ، ومن
        // غيَّره يجب أن يُعرف — وإلا صار «الالتزام بـSLA» رقماً بلا مرجع.
        $this->audit->log($me, 'SLA', null, $priority,
            json_encode($r->only(['first_minutes', 'next_minutes', 'resolve_minutes']),
                JSON_UNESCAPED_UNICODE), $r->ip());

        return $this->sendResponse(['ok' => true], 'حُفظت المهلة.');
    }

    /**
     * POST support/threads/{id}/viewing  {state}
     *
     * نبضةُ «أنا هنا» (البند 6) — تُنادى من شاشة المحادثة.
     */
    public function viewing(Request $r, int $id)
    {
        if (!$this->thread($id) || !$this->mayOpen($r, $id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $state = $r->input('state') === 'LEAVE' ? 'LEAVE'
            : ($r->input('state') === 'TYPING' ? 'TYPING' : 'VIEWING');

        $p = app(SupportPresence::class);
        if ($state === 'LEAVE') {
            $p->leave($id, (int) $this->me($r)->id);
        } else {
            $p->touchViewer($id, $this->me($r), $state);
        }

        return $this->sendResponse(['ok' => true], 'Success');
    }

    // ══════════════════════════════════════════════════════════════════
    //  الإدارة
    // ══════════════════════════════════════════════════════════════════

    /** GET support/staff */
    public function staff()
    {
        return $this->sendResponse([
            'items' => $this->staffSvc->all(),
            'roles' => SupportPermissions::ROLES,
            'catalog_by_role' => array_map(
                fn ($role) => SupportPermissions::catalogFor($role),
                array_combine(array_keys(SupportPermissions::ROLES), array_keys(SupportPermissions::ROLES)),
            ),
        ], 'Success');
    }

    /** POST support/staff  {name, username, role} */
    public function createStaff(Request $r)
    {
        $me  = $this->me($r);
        $out = $this->staffSvc->create(
            (string) $r->input('name', ''),
            (string) $r->input('username', ''),
            (string) $r->input('role', SupportPermissions::SUPPORT_AGENT),
            $me,
        );

        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        $this->audit->log($me, SupportAudit::STAFF_CREATE, null, (string) $out['id'],
            $r->input('name') . ' — ' . $r->input('role'), $r->ip());

        // الكلمةُ تُعرض مرّةً واحدة ولا تُخزَّن نصّاً ولا تُرسَل — انظر
        // `SupportStaffService::create`.
        return $this->sendResponse($out, 'أُنشئ الحساب. سلّم كلمة المرور لصاحبها الآن — لن تظهر ثانيةً.');
    }

    /** PUT support/staff/{id} */
    public function updateStaff(Request $r, int $id)
    {
        $me = $this->me($r);

        // لا يُوقف المديرُ نفسه ولا يُخفّض دورَه: خطوةٌ واحدة تُخرج آخرَ
        // مديرٍ من النظام ولا سبيل للعودة إلا من قاعدة البيانات.
        if ($id === (int) $me->id
            && (array_key_exists('is_active', $r->all()) || $r->has('role'))) {
            return $this->sendError('لا يمكنك تغيير دورك أو إيقاف حسابك بنفسك.', [], 422);
        }

        $changes = array_intersect_key($r->all(), array_flip(['name', 'role', 'is_active']));
        $out = $this->staffSvc->update($id, $changes, $me);

        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        $action = array_key_exists('is_active', $changes)
            ? ($changes['is_active'] ? SupportAudit::STAFF_ENABLE : SupportAudit::STAFF_DISABLE)
            : SupportAudit::STAFF_UPDATE;

        $this->audit->log($me, $action, null, (string) $id, json_encode($changes, JSON_UNESCAPED_UNICODE), $r->ip());

        // خفضُ الدور يسحب ما صار فوق السقف — ويُسجَّل سحباً صريحاً لكلٍّ
        // منها، لا يُبتلع داخل «تعديل حساب».
        foreach ($out['revoked'] ?? [] as $p) {
            $this->audit->log($me, SupportAudit::PERM_REVOKE, null, (string) $id,
                $p . ' — بخفض الدور', $r->ip());
        }

        return $this->sendResponse(['ok' => true], 'تم التحديث.');
    }

    /** DELETE support/staff/{id} */
    public function deleteStaff(Request $r, int $id)
    {
        $me = $this->me($r);

        if ($id === (int) $me->id) {
            return $this->sendError('لا يمكنك حذف حسابك بنفسك.', [], 422);
        }

        $out = $this->staffSvc->delete($id);
        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        $this->audit->log($me, SupportAudit::STAFF_DELETE, null, (string) $id, null, $r->ip());

        return $this->sendResponse(['ok' => true], 'حُذف الحساب.');
    }

    /** POST support/staff/{id}/password */
    public function resetStaffPassword(Request $r, int $id)
    {
        $me  = $this->me($r);
        $out = $this->staffSvc->resetPassword($id);

        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        $this->audit->log($me, SupportAudit::STAFF_RESET_PASS, null, (string) $id, null, $r->ip());

        return $this->sendResponse($out, 'سلّم كلمة المرور لصاحبها الآن — لن تظهر ثانيةً.');
    }

    /** PUT support/staff/{id}/permissions  {permissions:[]} */
    public function setPermissions(Request $r, int $id)
    {
        $me = $this->me($r);

        // لا يمنح المديرُ نفسَه: من يعدّل صلاحياته بنفسه لا سقفَ له فعلياً.
        if ($id === (int) $me->id) {
            return $this->sendError('لا يمكنك تعديل صلاحياتك بنفسك.', [], 422);
        }

        $out = $this->staffSvc->setPermissions($id, (array) $r->input('permissions', []), $me);
        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        foreach ($out['added'] as $p) {
            $this->audit->log($me, SupportAudit::PERM_GRANT, null, (string) $id, $p, $r->ip());
        }
        foreach ($out['removed'] as $p) {
            $this->audit->log($me, SupportAudit::PERM_REVOKE, null, (string) $id, $p, $r->ip());
        }

        return $this->sendResponse($out, 'حُفظت الصلاحيات.');
    }

    /** GET support/audit?thread_id= */
    public function auditLog(Request $r)
    {
        $tid = (int) $r->query('thread_id', 0);

        return $this->sendResponse([
            'items' => $this->audit->recent(200, $tid > 0 ? $tid : null),
        ], 'Success');
    }
}
