<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\ChatService;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

/**
 * دردشة الموظّف — **مع وكيله وحده**.
 *
 * لا قائمة محادثات هنا ولا اختيار: للموظّف محادثةٌ واحدة ممكنة، وهي مع من
 * فعّله. فلا يرى محادثة الوكيل مع الإدارة (شأن الوكيل)، ولا محادثات زملائه.
 *
 * والموظّف يُقرأ من الوسيط لا من الطلب، كسائر مسارات الموظّف: رقمٌ مرسَل من
 * الهاتف يعني أن تعديل الطلب يفتح محادثة موظّفٍ آخر.
 */
class EmployeeChatController extends BaseController
{
    public function __construct(private ChatService $chat)
    {
    }

    /** الموظّف ومحادثته — أو null إن تعذّر إنشاؤها. */
    private function thread(Request $r): array
    {
        $employee = $r->attributes->get('employee');
        if (!$employee) {
            return [null, null];
        }

        $thread = $this->chat->employeeThread(
            (int) $employee->agent_id,
            (int) $employee->id
        );

        return [$employee, $thread];
    }

    /** GET device/employee/chat?after_id= */
    public function messages(Request $request)
    {
        [$employee, $thread] = $this->thread($request);
        if (!$thread) {
            return $this->sendError('تعذّر فتح المحادثة.', [], 404);
        }

        $after = max(0, (int) $request->query('after_id', 0));
        $messages = $this->chat->messages((int) $thread->id, $after);

        if ($after === 0 || $messages !== []) {
            $this->chat->markDelivered((int) $thread->id, ChatService::EMPLOYEE, (int) $employee->id);
            $this->chat->markRead((int) $thread->id, ChatService::EMPLOYEE, (int) $employee->id);
        }

        $ids = array_map(fn ($m) => (int) $m->id, $messages);
        $me = (int) $employee->id;

        return $this->sendResponse([
            'thread_id' => (int) $thread->id,
            'items'     => $messages,
            'receipts'  => $this->chat->receipts((int) $thread->id, ChatService::EMPLOYEE),
            'reactions' => $this->chat->reactionsFor($ids, ChatService::EMPLOYEE, $me),
            'starred'   => $this->chat->starredIn($ids, ChatService::EMPLOYEE, $me),
            'typing'    => $this->chat->typingIn((int) $thread->id, ChatService::EMPLOYEE),
        ], 'Success');
    }

    /** POST device/employee/chat  {body, attachment?, reply_to_id?} */
    public function send(Request $request)
    {
        [$employee, $thread] = $this->thread($request);
        if (!$thread) {
            return $this->sendError('تعذّر فتح المحادثة.', [], 404);
        }

        $attachment = [];
        if ($request->hasFile('attachment')) {
            $attachment = $this->chat->storeAttachment($request->file('attachment')) ?? [];
            if ($attachment === []) {
                return $this->sendError('المرفق غير مقبول.', [], 422);
            }
        }

        $replyTo = (int) $request->input('reply_to_id', 0);

        $msg = $this->chat->send(
            (int) $thread->id,
            ChatService::EMPLOYEE,
            (int) $employee->id,
            $employee->full_name ?: null,
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

    /** POST device/employee/chat/typing  {state} — TYPING · RECORDING · NONE */
    public function typing(Request $request)
    {
        [$employee, $thread] = $this->thread($request);
        if (!$thread) {
            return $this->sendError('تعذّر فتح المحادثة.', [], 404);
        }

        $this->chat->setTyping((int) $thread->id, ChatService::EMPLOYEE,
            (int) $employee->id, $employee->full_name ?: null,
            (string) $request->input('state', 'NONE'));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** POST device/employee/chat/{messageId}/react  {emoji} */
    public function react(Request $request, int $messageId)
    {
        [$employee, $thread] = $this->thread($request);
        if (!$thread) {
            return $this->sendError('تعذّر فتح المحادثة.', [], 404);
        }

        $this->chat->react($messageId, (int) $thread->id, ChatService::EMPLOYEE,
            (int) $employee->id, (string) $request->input('emoji', ''));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** PUT device/employee/chat/{messageId}  {body} */
    public function edit(Request $request, int $messageId)
    {
        [$employee, $thread] = $this->thread($request);
        if (!$thread) {
            return $this->sendError('تعذّر فتح المحادثة.', [], 404);
        }

        $err = $this->chat->editMessage($messageId, (int) $thread->id,
            ChatService::EMPLOYEE, (int) $employee->id,
            (string) $request->input('body', ''));

        return $err === null
            ? $this->sendResponse(['ok' => true], 'تم التعديل.')
            : $this->sendError($err, [], 422);
    }

    /** POST device/employee/chat/{messageId}/star  {star} */
    public function star(Request $request, int $messageId)
    {
        [$employee, $thread] = $this->thread($request);
        if (!$thread) {
            return $this->sendError('تعذّر فتح المحادثة.', [], 404);
        }

        $this->chat->star($messageId, (int) $thread->id, ChatService::EMPLOYEE,
            (int) $employee->id, $request->boolean('star', true));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** POST device/employee/chat/{messageId}/pin  {pin} */
    public function pin(Request $request, int $messageId)
    {
        [, $thread] = $this->thread($request);
        if (!$thread) {
            return $this->sendError('تعذّر فتح المحادثة.', [], 404);
        }

        $this->chat->pinMessage($messageId, (int) $thread->id,
            ChatService::EMPLOYEE, $request->boolean('pin', true));

        return $this->sendResponse(['ok' => true], 'Success');
    }

    /** DELETE device/employee/chat/{messageId} — لصاحبها وحده. */
    public function destroy(Request $request, int $messageId)
    {
        [$employee, $thread] = $this->thread($request);
        if (!$thread) {
            return $this->sendError('تعذّر فتح المحادثة.', [], 404);
        }

        $ok = $this->chat->deleteMessage(
            $messageId, (int) $thread->id, ChatService::EMPLOYEE, (int) $employee->id
        );

        return $ok
            ? $this->sendResponse(['deleted' => true], 'حُذفت الرسالة.')
            : $this->sendError('لا يمكن حذف رسالة ليست لك.', [], 403);
    }

    /**
     * GET device/employee/chat/attachment/{name}
     *
     * كنظيرتها في مسار الوكيل: المرفق يُقدَّم لمن يشارك محادثته وحده.
     */
    public function attachment(Request $request, string $name)
    {
        [$employee, $thread] = $this->thread($request);
        if (!$thread || basename($name) !== $name) {
            return $this->sendError('المرفق غير موجود.', [], 404);
        }

        $row = DB::table('chat_messages')
            ->where('attachment_path', $name)
            ->where('thread_id', (int) $thread->id)
            ->whereNull('deleted_at')
            ->first(['attachment_mime', 'attachment_name']);

        $path = ChatService::DIR . '/' . $name;
        if (!$row || !\Illuminate\Support\Facades\Storage::disk(ChatService::DISK)->exists($path)) {
            return $this->sendError('المرفق غير موجود.', [], 404);
        }

        return response(
            \Illuminate\Support\Facades\Storage::disk(ChatService::DISK)->get($path),
            200,
            [
                'Content-Type'        => $row->attachment_mime ?: 'application/octet-stream',
                'Content-Disposition' => 'inline; filename="' . rawurlencode((string) $row->attachment_name) . '"',
                'Cache-Control'       => 'private, max-age=86400',
            ]
        );
    }

    /** GET device/employee/chat/unread — رقمٌ واحد للشارة. */
    public function unread(Request $request)
    {
        $employee = $request->attributes->get('employee');
        if (!$employee) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        // بلا إنشاء: الشارة تُسأل دورياً، وإنشاء محادثةٍ من استطلاعٍ يملأ
        // قائمة الوكيل بمحادثات لم يبدأها أحد.
        $thread = DB::table('chat_threads')
            ->where('agent_id', (int) $employee->agent_id)
            ->where('kind', ChatService::EMPLOYEE)
            ->where('employee_id', (int) $employee->id)
            ->first();

        if (!$thread) {
            return $this->sendResponse(['total' => 0], 'Success');
        }

        $unread = $this->chat->unreadByThread(
            [(int) $thread->id],
            ChatService::EMPLOYEE,
            (int) $employee->id
        );

        return $this->sendResponse(['total' => array_sum($unread)], 'Success');
    }
}
