<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\ChatService;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Storage;

/**
 * دردشة الوكيل — مع الإدارة، ومع كل موظّف من موظّفيه.
 *
 * الوكيل يُشتقّ من التوثيق دائماً ولا يُقرأ من الطلب: `agent_id` مرسَلاً من
 * الهاتف يعني أن تعديل الطلب يدوياً يفتح محادثات وكيل آخر. والعزل في
 * الاستعلام لا في الواجهة.
 *
 * ⚠ لا شيء هنا يمسّ المال. رسالةٌ تقول «سلّمتُ الحوالة» لا تُسجّل تسليماً.
 */
class ChatController extends BaseController
{
    public function __construct(private ChatService $chat)
    {
    }

    /**
     * GET chat/threads
     *
     * محادثة الإدارة أوّلاً دائماً، ثم الموظّفون بالأحدث.
     *
     * والإدارة تُنشأ هنا إن لم تكن موجودة: هي المحادثة التي يفتحها الوكيل
     * ليطلب المساعدة، فوجودُها شرطٌ لظهور الزرّ لا نتيجةٌ لضغطه.
     */
    public function threads()
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $agentId = (int) $user->id;
        $admin = $this->chat->adminThread($agentId);

        // محادثات الموظّفين — بأسمائهم من `employees`، وباستعلامٍ واحد.
        $rows = DB::table('chat_threads as t')
            ->leftJoin('employees as e', 'e.id', '=', 't.employee_id')
            ->where('t.agent_id', $agentId)
            ->where('t.kind', ChatService::EMPLOYEE)
            ->orderByDesc('t.last_message_at')
            ->get(['t.id', 't.kind', 't.employee_id', 't.last_message_at', 'e.full_name']);

        $ids = array_merge([(int) $admin->id], $rows->pluck('id')->map(fn ($v) => (int) $v)->all());
        $unread = $this->chat->unreadByThread($ids, ChatService::AGENT, $agentId);

        $last = $this->lastBodies($ids);

        $set = $this->chat->settingsFor($ids, ChatService::AGENT, $agentId);

        /* بطاقة المحادثة بإعداداتها — كتمٌ وتثبيتٌ وأرشفةٌ وقفل.
         *
         * و«غير مقروءة» تُحسب هنا لا في التطبيق: `forced_unread` قرارُ
         * المستخدم («تحديد كغير مقروءة»، البند 10) ويعلو على العدّاد
         * المحسوب — وواحدةٌ بلا رسائل جديدة تظهر غير مقروءة بطلبه. */
        $card = function (int $tid, string $kind, string $title, ?int $empId,
                          $lastAt) use ($set, $last, $unread) {
            $s = $set[$tid] ?? null;
            $n = $unread[$tid] ?? 0;

            return [
                'id'              => $tid,
                'kind'            => $kind,
                'title'           => $title,
                'employee_id'     => $empId,
                'last_message_at' => $lastAt,
                'last_body'       => $last[$tid] ?? null,
                'unread'          => $n,
                'forced_unread'   => (bool) ($s->forced_unread ?? false),
                'muted_until'     => $s->muted_until ?? null,
                'pinned'          => ($s->pinned_at ?? null) !== null,
                'archived'        => ($s->archived_at ?? null) !== null,
                'locked'          => (bool) ($s->locked ?? false),
            ];
        };

        $items = [
            $card((int) $admin->id, ChatService::ADMIN, 'دعم الرحالة',
                null, $admin->last_message_at),
        ];

        foreach ($rows as $r) {
            $items[] = $card((int) $r->id, ChatService::EMPLOYEE,
                $r->full_name ?: 'موظّف', (int) $r->employee_id, $r->last_message_at);
        }

        // المثبَّتة أولاً (البند 32)، ثم الأحدث. و«دعم الرحالة» يبقى في
        // الأعلى بين المثبَّتة: هي المحادثة التي يفتحها الوكيل حين يحتاج.
        usort($items, function ($a, $b) {
            if ($a['kind'] === ChatService::ADMIN) return -1;
            if ($b['kind'] === ChatService::ADMIN) return 1;
            if ($a['pinned'] !== $b['pinned']) return $a['pinned'] ? -1 : 1;
            return strcmp((string) $b['last_message_at'], (string) $a['last_message_at']);
        });

        // المكتومة لا تُعدّ في الشارة (البند 34): الكتم يعني «لا تنبّهني».
        $now = now();
        $badge = 0;
        foreach ($items as $i) {
            $muted = $i['muted_until'] !== null && $now->lt($i['muted_until']);
            if (!$muted && !$i['archived']) $badge += $i['unread'];
        }

        return $this->sendResponse([
            'items'        => $items,
            'total_unread' => $badge,
        ], 'Success');
    }

    /**
     * POST chat/threads/employee  {employee_id}
     *
     * يفتح محادثةً مع موظّف — أو يعيد القائمة إن كانت.
     */
    public function openEmployee(Request $request)
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $employeeId = (int) $request->input('employee_id');
        if ($employeeId <= 0) {
            return $this->sendError('رقم الموظّف مطلوب.', [], 422);
        }

        $thread = $this->chat->employeeThread((int) $user->id, $employeeId);
        if (!$thread) {
            // لا تفريق بين «غير موجود» و«ليس موظّفك»: التفريق يكشف أرقام
            // موظّفي وكلاء آخرين لمن يجرّب الأرقام.
            return $this->sendError('الموظّف غير موجود.', [], 404);
        }

        return $this->sendResponse(['thread_id' => (int) $thread->id], 'Success');
    }

    /** GET chat/threads/{id}/messages?after_id= */
    public function messages(Request $request, int $id)
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $thread = DB::table('chat_threads')->where('id', $id)->first();
        if (!$thread || !$this->chat->participates($thread, ChatService::AGENT, (int) $user->id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $after = max(0, (int) $request->query('after_id', 0));
        $messages = $this->chat->messages($id, $after);

        // فتحُ المحادثة يعني قراءتها. ولا يُعلَّم عند الجلب التزايدي بلا
        // رسائل: الوكيل قد يكون غادر الشاشة والاستطلاع ما زال يعمل.
        if ($after === 0 || $messages !== []) {
            $this->chat->markDelivered($id, ChatService::AGENT, (int) $user->id);
            $this->chat->markRead($id, ChatService::AGENT, (int) $user->id);
        }

        // التفاعلات والمحفوظات لصفحةٍ كاملة — استعلامان لا استعلامٌ لكل رسالة.
        $ids = array_map(fn ($m) => (int) $m->id, $messages);
        $me = (int) $user->id;

        // إيصالا رسائلي عند الطرف الآخر — يُقرآن مع كل جلب، فالعلامة تتحوّل
        // من ✓ إلى ✓✓ بلا أن يفعل الوكيل شيئاً.
        return $this->sendResponse([
            'items'     => $messages,
            'receipts'  => $this->chat->receipts($id, ChatService::AGENT),
            'reactions' => $this->chat->reactionsFor($ids, ChatService::AGENT, $me),
            'starred'   => $this->chat->starredIn($ids, ChatService::AGENT, $me),
            // حالة الطرف الآخر تُقرأ مع النبضة نفسها: طلبٌ ثانٍ كل خمس ثوانٍ
            // من أجل «يكتب الآن» يضاعف حركة الشبكة بلا داعٍ.
            'typing'    => $this->chat->typingIn($id, ChatService::AGENT),
        ], 'Success');
    }

    /** POST chat/threads/{id}/messages  {body} */
    public function send(Request $request, int $id)
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $thread = DB::table('chat_threads')->where('id', $id)->first();
        if (!$thread || !$this->chat->participates($thread, ChatService::AGENT, (int) $user->id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $attachment = [];
        if ($request->hasFile('attachment')) {
            $attachment = $this->chat->storeAttachment($request->file('attachment')) ?? [];
            if ($attachment === []) {
                return $this->sendError(
                    'المرفق غير مقبول: النوع غير مدعوم أو الحجم أكبر من '
                        . (ChatService::MAX_ATTACHMENT / 1048576) . ' ميغابايت.',
                    [],
                    422
                );
            }
        }

        $replyTo = (int) $request->input('reply_to_id', 0);

        $msg = $this->chat->send(
            $id,
            ChatService::AGENT,
            (int) $user->id,
            $user->name ?: null,
            (string) $request->input('body', ''),
            $attachment,
            $replyTo > 0 ? $replyTo : null
        );

        if (!$msg) {
            return $this->sendError(
                'الرسالة فارغة أو أطول من ' . ChatService::MAX_BODY . ' حرفاً.',
                [],
                422
            );
        }

        return $this->sendResponse(['message' => $msg], 'تم الإرسال.');
    }

    /** DELETE chat/threads/{id}/messages/{messageId} — لصاحب الرسالة وحده. */
    public function destroy(int $id, int $messageId)
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $thread = DB::table('chat_threads')->where('id', $id)->first();
        if (!$thread || !$this->chat->participates($thread, ChatService::AGENT, (int) $user->id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $ok = $this->chat->deleteMessage($messageId, $id, ChatService::AGENT, (int) $user->id);

        // 403 لا 404: الرسالة موجودة والوكيل يراها، لكنها ليست كلامه.
        return $ok
            ? $this->sendResponse(['deleted' => true], 'حُذفت الرسالة.')
            : $this->sendError('لا يمكن حذف رسالة ليست لك.', [], 403);
    }

    /**
     * GET chat/attachment/{name}
     *
     * داخل `auth:sanctum`: المرفقات صور إيصالات ووثائق عملاء، ورابطٌ مفتوح
     * يجعلها مقروءة لمن عرفه. والتطبيق يمرّر ترويسة التوثيق مع الصورة.
     *
     * والاسم مقيَّد بالشكل في المسار **و**يُفحص هنا: `basename` يقطع أي
     * مسار تسلّل، والقيد يمنعه من الوصول أصلاً.
     */
    public function attachment(string $name)
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        if (basename($name) !== $name) {
            return $this->sendError('اسم غير صالح.', [], 404);
        }

        // المرفق يُقدَّم لمن يشارك محادثته وحده — لا لكل موثَّق.
        //
        // بغير هذا الفحص يكفي أن يكون للمهاجم حسابٌ سليم ليقرأ مرفقات وكيلٍ
        // آخر إن خمّن الاسم. والاسم عشوائي، لكن العشوائية ليست تصريحاً.
        $row = DB::table('chat_messages as m')
            ->join('chat_threads as t', 't.id', '=', 'm.thread_id')
            ->where('m.attachment_path', $name)
            ->whereNull('m.deleted_at')
            ->where('t.agent_id', (int) $user->id)
            ->first(['m.attachment_mime', 'm.attachment_name']);

        if (!$row) {
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
    //  مزايا الرسالة والمحادثة
    // ══════════════════════════════════════════════════════════════════

    /**
     * المحادثة والمشارَكة فيها — الحارس الذي تبدأ به كل نقطة أدناه.
     *
     * ⚠ لا نقطة واحدة تثق برقمٍ من الطلب: البند 87 يوجب التحقّق في الخادم،
     * والبند 62 يمنع الوصول بتخمين الأرقام. هذا هو موضع المنع.
     */
    private function guard(int $threadId): ?object
    {
        $user = Auth::user();
        if (!$user) {
            return null;
        }

        $thread = DB::table('chat_threads')->where('id', $threadId)->first();
        if (!$thread || !$this->chat->participates($thread, ChatService::AGENT, (int) $user->id)) {
            return null;
        }

        return $thread;
    }

    /** POST chat/threads/{id}/messages/{messageId}/react  {emoji} */
    public function react(Request $request, int $id, int $messageId)
    {
        if (!$this->guard($id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $this->chat->react($messageId, $id, ChatService::AGENT,
            (int) Auth::id(), (string) $request->input('emoji', ''));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** PUT chat/threads/{id}/messages/{messageId}  {body} */
    public function edit(Request $request, int $id, int $messageId)
    {
        if (!$this->guard($id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $err = $this->chat->editMessage($messageId, $id, ChatService::AGENT,
            (int) Auth::id(), (string) $request->input('body', ''));

        return $err === null
            ? $this->sendResponse(['ok' => true], 'تم التعديل.')
            : $this->sendError($err, [], 422);
    }

    /** POST chat/threads/{id}/messages/{messageId}/pin  {pin} */
    public function pin(Request $request, int $id, int $messageId)
    {
        if (!$this->guard($id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $this->chat->pinMessage($messageId, $id, ChatService::AGENT,
            $request->boolean('pin', true));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** POST chat/threads/{id}/messages/{messageId}/star  {star} */
    public function star(Request $request, int $id, int $messageId)
    {
        if (!$this->guard($id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $this->chat->star($messageId, $id, ChatService::AGENT,
            (int) Auth::id(), $request->boolean('star', true));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /**
     * PUT chat/threads/{id}/settings  {mute_hours?, pinned?, archived?, locked?, unread?}
     *
     * كتمٌ بمدّة لا رايةٍ ثنائية (البند 34): `mute_hours` صفرٌ يرفع الكتم،
     * و`-1` يعني «دائماً» — تاريخٌ بعيد بدل عمودٍ ثانٍ يقول «إلى الأبد».
     */
    public function settings(Request $request, int $id)
    {
        if (!$this->guard($id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $changes = [];

        if ($request->has('mute_hours')) {
            $h = (int) $request->input('mute_hours');
            $changes['muted_until'] = $h === 0
                ? null
                : ($h < 0 ? now()->addYears(50) : now()->addHours($h));
        }
        if ($request->has('pinned')) {
            $changes['pinned_at'] = $request->boolean('pinned') ? now() : null;
        }
        if ($request->has('archived')) {
            $changes['archived_at'] = $request->boolean('archived') ? now() : null;
        }
        if ($request->has('locked')) {
            $changes['locked'] = $request->boolean('locked') ? 1 : 0;
        }
        if ($request->has('unread')) {
            $changes['forced_unread'] = $request->boolean('unread') ? 1 : 0;
        }

        $this->chat->updateSettings($id, ChatService::AGENT, (int) Auth::id(), $changes);

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** POST chat/threads/{id}/typing  {state} — TYPING · RECORDING · NONE */
    public function typing(Request $request, int $id)
    {
        $user = Auth::user();
        if (!$this->guard($id)) {
            return $this->sendError('المحادثة غير موجودة.', [], 404);
        }

        $this->chat->setTyping($id, ChatService::AGENT, (int) $user->id,
            $user->name ?: null, (string) $request->input('state', 'NONE'));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /**
     * GET chat/search?q=
     *
     * ⚠ النطاق من الخادم لا من الطلب: يُبحث في محادثات هذا الوكيل وحدها،
     * فلا يستطيع أحد توسيع بحثه بتعديل الطلب (البندان 35 و62).
     */
    public function search(Request $request)
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $ids = DB::table('chat_threads')
            ->where('agent_id', (int) $user->id)
            ->pluck('id')->map(fn ($v) => (int) $v)->all();

        return $this->sendResponse([
            'items' => $this->chat->search($ids, (string) $request->query('q', '')),
        ], 'Success');
    }

    /**
     * GET chat/unread
     *
     * رقمٌ واحد للشارة، يسأله التطبيق دورياً. منفصلٌ عن `threads` لأن
     * حمولة تلك أسماءٌ وآخرُ رسالةٍ لكل محادثة — وسحبُها كل نصف دقيقة من
     * أجل عدد إهدار.
     */
    public function unread()
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $ids = DB::table('chat_threads')
            ->where('agent_id', (int) $user->id)
            ->pluck('id')
            ->map(fn ($v) => (int) $v)
            ->all();

        $unread = $this->chat->unreadByThread($ids, ChatService::AGENT, (int) $user->id);

        return $this->sendResponse(['total' => array_sum($unread)], 'Success');
    }

    /**
     * آخر رسالة في كل محادثة — للمعاينة تحت الاسم.
     *
     * استعلامٌ واحد بـ ROW_NUMBER لا استعلامٌ لكل محادثة: النمط المقرَّر في
     * هذا المشروع بعد أن كلّف الشكلُ الآخر 68 ثانية في كشف الحساب.
     *
     * @return array<int,string>
     */
    private function lastBodies(array $threadIds): array
    {
        if ($threadIds === []) {
            return [];
        }

        $in = implode(',', array_map('intval', $threadIds));

        $rows = DB::select(
            "SELECT thread_id, body FROM (
                 SELECT thread_id, body,
                        ROW_NUMBER() OVER (PARTITION BY thread_id ORDER BY id DESC) AS rn
                   FROM chat_messages WHERE thread_id IN ($in)
             ) x WHERE rn = 1"
        );

        $out = [];
        foreach ($rows as $r) {
            $out[(int) $r->thread_id] = $r->body;
        }

        return $out;
    }
}
