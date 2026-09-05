<?php

namespace App\Services;

use Illuminate\Support\Facades\DB;

/**
 * الدردشة — الوكيل مع الإدارة، والوكيل مع موظّفيه.
 *
 * ⚠ **طبقة تواصل لا طبقة مالية.** لا تكتب هذه الخدمة في `wallet` ولا
 * `InternalEx` ولا أي قيد أو رصيد، ولا تستدعي خدمةً مالية. رسالةٌ تقول
 * «سلّمتُ الحوالة» لا تُسجّل تسليماً — التسجيل من شاشته وحدها.
 *
 * ## هُويّة الطرف: نوعٌ ورقم، لا رقم وحده
 *
 * الوكيل رقمه في `users`، والموظف رقمه في `employees` — فضاءان مختلفان.
 * فكل دالّة هنا تأخذ `$kind` و`$id` معاً، ولا واحدة تقبل رقماً عارياً.
 * وحيثما قُورن طرفان قُورن الاثنان: تجاهُل النوع يجعل الموظف رقم 7 يقرأ
 * محادثة المستخدم رقم 7.
 */
class ChatService
{
    public const ADMIN = 'ADMIN';
    public const EMPLOYEE = 'EMPLOYEE';
    public const AGENT = 'AGENT';

    /** أطول رسالة. يُفحص هنا أيضاً لا في الطلب وحده — الخدمة تُستدعى من مسارين. */
    public const MAX_BODY = 2000;

    /**
     * أكبر مرفق — 8 ميغابايت.
     *
     * الوكيل في فرعٍ قد تكون شبكته بطيئة، ومرفقٌ بعشرات الميغابايت يعلَق في
     * الرفع فيظنّ التطبيق معطّلاً. وصورةُ إيصالٍ من كاميرا هاتف تحت هذا
     * الحدّ بمراحل، والتطبيق يضغطها قبل الرفع.
     */
    public const MAX_ATTACHMENT = 8 * 1024 * 1024;

    /** مجلّد المرفقات على القرص الخاصّ — لا العامّ. */
    public const DISK = 'local';
    public const DIR = 'chat';

    /**
     * الأنواع المقبولة، وتصنيفُ كلٍّ منها.
     *
     * قائمةُ سماحٍ لا قائمةَ منع: قائمة المنع تُنسى فيها صيغةٌ واحدة فيمرّ
     * ما لا يُراد. و`svg` مستبعدة عمداً رغم كونها صورة — هي نصٌّ قد يحمل
     * سكربتاً.
     */
    public const MIMES = [
        'image/jpeg' => ['IMAGE', 'jpg'],
        'image/png'  => ['IMAGE', 'png'],
        'image/webp' => ['IMAGE', 'webp'],
        'image/heic' => ['IMAGE', 'heic'],
        'audio/mp4'  => ['AUDIO', 'm4a'],
        'audio/aac'  => ['AUDIO', 'aac'],
        'audio/mpeg' => ['AUDIO', 'mp3'],
        'audio/ogg'  => ['AUDIO', 'ogg'],
        'audio/wav'  => ['AUDIO', 'wav'],
        'application/pdf' => ['FILE', 'pdf'],
    ];

    /**
     * محادثة الوكيل مع الإدارة — تُنشأ عند أول فتح.
     *
     * الإنشاء عند الفتح لا عند تسجيل الوكيل: محادثةٌ فارغة لكل وكيل في
     * النظام صفوفٌ لا يقرؤها أحد، وقائمةُ الإدارة تمتلئ بمحادثات لم تبدأ.
     */
    public function adminThread(int $agentId): object
    {
        $row = DB::table('chat_threads')
            ->where('agent_id', $agentId)
            ->where('kind', self::ADMIN)
            ->first();

        if ($row) {
            return $row;
        }

        $id = DB::table('chat_threads')->insertGetId([
            'agent_id'   => $agentId,
            'kind'       => self::ADMIN,
            'created_at' => now(),
            'updated_at' => now(),
        ]);

        return DB::table('chat_threads')->where('id', $id)->first();
    }

    /**
     * محادثة الوكيل مع موظّفٍ من موظّفيه.
     *
     * الموظف يُتحقّق أنه **تابعٌ لهذا الوكيل** في الاستعلام نفسه لا في
     * الواجهة: رقم موظّفٍ من طلبٍ معدَّل يدوياً كان يفتح محادثةً مع موظّف
     * وكيلٍ آخر.
     */
    public function employeeThread(int $agentId, int $employeeId): ?object
    {
        $owns = DB::table('employees')
            ->where('id', $employeeId)
            ->where('agent_id', $agentId)
            ->exists();

        if (!$owns) {
            return null;
        }

        $row = DB::table('chat_threads')
            ->where('agent_id', $agentId)
            ->where('kind', self::EMPLOYEE)
            ->where('employee_id', $employeeId)
            ->first();

        if ($row) {
            return $row;
        }

        $id = DB::table('chat_threads')->insertGetId([
            'agent_id'    => $agentId,
            'kind'        => self::EMPLOYEE,
            'employee_id' => $employeeId,
            'created_at'  => now(),
            'updated_at'  => now(),
        ]);

        return DB::table('chat_threads')->where('id', $id)->first();
    }

    /**
     * هل يملك هذا الطرف الحقّ في هذه المحادثة؟
     *
     * الحارس الوحيد لكل قراءةٍ وكتابة. والوكيل يملك محادثاته كلّها، والموظف
     * محادثته هو وحدها — ولا يرى محادثة الوكيل مع الإدارة، فتلك شأن الوكيل.
     */
    public function participates(object $thread, string $kind, int $id): bool
    {
        return match ($kind) {
            self::AGENT    => (int) $thread->agent_id === $id,
            self::EMPLOYEE => $thread->kind === self::EMPLOYEE
                              && (int) $thread->employee_id === $id,
            self::ADMIN    => $thread->kind === self::ADMIN,
            default        => false,
        };
    }

    /**
     * رسائل محادثة. `$afterId` للجلب التزايدي — الاستطلاع لا يعيد ما وصل.
     *
     * الترتيب تصاعدي والسقف يقتطع **الأحدث** لا الأقدم: من يفتح محادثةً
     * قديمة يريد آخرها، لا أوّلها.
     */
    public function messages(int $threadId, int $afterId = 0, int $limit = 50): array
    {
        // الرسالة المُقتبَسة تأتي معها في الاستعلام نفسه — نصّاً واسماً.
        //
        // `LEFT JOIN` على المفتاح الأساسي، لا استعلامٌ فرعي لكل صفّ: المفتاح
        // مفهرس فالربط رخيص، والشكل الآخر هو ما كلّف كشف الحساب 68 ثانية.
        // ولا يضاعف صفّاً — `reply_to_id` يشير إلى صفٍّ واحد بحكم المفتاح.
        $cols = [
            'm.id', 'm.thread_id', 'm.sender_kind', 'm.sender_id', 'm.sender_name',
            'm.body', 'm.created_at', 'm.deleted_at', 'm.reply_to_id',
            'm.attachment_path', 'm.attachment_name', 'm.attachment_mime',
            'm.attachment_size', 'm.attachment_kind',
            'r.body as reply_body',
            'r.sender_name as reply_sender_name',
            'r.sender_kind as reply_sender_kind',
            'r.attachment_kind as reply_attachment_kind',
        ];

        $q = DB::table('chat_messages as m')
            ->leftJoin('chat_messages as r', 'r.id', '=', 'm.reply_to_id')
            ->where('m.thread_id', $threadId);

        if ($afterId > 0) {
            // جلب تزايدي: ما بعد آخر ما عنده، بترتيبه الطبيعي.
            return $q->where('m.id', '>', $afterId)
                ->orderBy('m.id')
                ->limit($limit)
                ->get($cols)
                ->map(fn ($m) => $this->hideDeleted($m))
                ->all();
        }

        // أول فتح: الأحدث، ثم يُقلب ليُقرأ من الأقدم إلى الأحدث.
        return $q->orderByDesc('m.id')->limit($limit)->get($cols)
            ->reverse()->values()
            ->map(fn ($m) => $this->hideDeleted($m))
            ->all();
    }

    /**
     * الرسالة المحذوفة تصل بلا نصّها ولا مرفقها.
     *
     * الإخفاء في الخادم لا في التطبيق: إخفاءٌ في الواجهة يعني أن النصّ سافر
     * إلى الجهاز فعلاً، ومن قرأ الشبكة قرأه. والصفّ يبقى ليقول «حُذفت رسالة»
     * ولئلّا تشير الردود عليه إلى العدم.
     */
    private function hideDeleted(object $m): object
    {
        if ($m->deleted_at === null) {
            return $m;
        }

        $m->body = null;
        $m->attachment_path = null;
        $m->attachment_name = null;
        $m->attachment_mime = null;
        $m->attachment_size = null;
        $m->attachment_kind = null;

        return $m;
    }

    /**
     * حذف رسالة — لصاحبها وحده، وناعماً.
     *
     * المرسِل وحده: حذفُ أحدهم كلامَ الآخر من محادثةٍ بينهما يجعل السجلّ
     * غير جدير بالثقة. والمقارنة بالنوع والرقم معاً — رقمٌ وحده يجعل الموظف
     * رقم 7 يحذف رسالة المستخدم رقم 7.
     */
    public function deleteMessage(int $messageId, int $threadId, string $kind, int $id): bool
    {
        return DB::table('chat_messages')
            ->where('id', $messageId)
            ->where('thread_id', $threadId)
            ->where('sender_kind', $kind)
            ->where('sender_id', $id)
            ->whereNull('deleted_at')
            ->update(['deleted_at' => now()]) > 0;
    }

    /**
     * «وصلت» — يُسجَّل بمجرّد أن يجلب الطرف الرسائل، ولو لم يفتح المحادثة.
     *
     * منفصلٌ عن «قُرئت» لأنهما سؤالان: الأولى «بلغت جهازه؟» والثانية «نظر
     * إليها؟». وعلامةٌ واحدة لهما تجعل «قُرئت» تظهر لمن لم يفتح شيئاً.
     */
    public function markDelivered(int $threadId, string $kind, int $id): void
    {
        $upTo = (int) DB::table('chat_messages')->where('thread_id', $threadId)->max('id');
        if ($upTo === 0) {
            return;
        }

        $row = DB::table('chat_reads')
            ->where('thread_id', $threadId)
            ->where('reader_kind', $kind)
            ->where('reader_id', $id)
            ->first();

        if ($row === null) {
            DB::table('chat_reads')->insert([
                'thread_id'                 => $threadId,
                'reader_kind'               => $kind,
                'reader_id'                 => $id,
                'last_read_message_id'      => 0,
                'last_delivered_message_id' => $upTo,
                'updated_at'                => now(),
            ]);
            return;
        }

        if ((int) $row->last_delivered_message_id >= $upTo) {
            return;
        }

        DB::table('chat_reads')->where('id', $row->id)
            ->update(['last_delivered_message_id' => $upTo, 'updated_at' => now()]);
    }

    /**
     * إيصالا رسائلي: إلى أين وصلت وإلى أين قُرئت **عند الطرف الآخر**.
     *
     * يُقرأ من صفوف من ليس أنا في هذه المحادثة — استعلامٌ واحد لا استعلامٌ
     * لكل رسالة، والعلامة تُحسب في التطبيق بمقارنة رقم الرسالة بالرقمين.
     *
     * @return array{delivered:int,read:int}
     */
    public function receipts(int $threadId, string $myKind): array
    {
        $rows = DB::table('chat_reads')
            ->where('thread_id', $threadId)
            ->where('reader_kind', '!=', $myKind)
            ->get(['last_delivered_message_id', 'last_read_message_id']);

        $d = 0;
        $r = 0;
        foreach ($rows as $x) {
            $d = max($d, (int) $x->last_delivered_message_id);
            $r = max($r, (int) $x->last_read_message_id);
        }

        return ['delivered' => $d, 'read' => $r];
    }

    /**
     * إرسال رسالة.
     *
     * الكتابتان في معاملةٍ واحدة: الرسالة و`last_message_at` على المحادثة.
     * وانفصالُهما يعني محادثةً تحمل رسالةً لا يعرفها ترتيبُ القائمة، فتبقى
     * في أسفلها ولا يراها صاحبها.
     */
    public function send(
        int $threadId,
        string $kind,
        int $id,
        ?string $name,
        string $body,
        array $attachment = [],
        ?int $replyToId = null,
        ?string $clientId = null
    ): ?object {
        $body = trim($body);

        /* منع التكرار (البند 68).
         *
         * الجهاز يولّد `client_id` لكل رسالة **قبل** الإرسال، فإعادة
         * المحاولة تحمل الرقم نفسه. والرسالة الموجودة تُعاد كما هي بدل أن
         * تُكتب ثانية — فالضغط المكرّر وضعفُ الشبكة وإعادةُ الاتصال لا
         * تُنتج رسالتين.
         *
         * والفحص هنا قبل كل شيء، والفهرس الفريد
         * `(thread_id, client_id)` هو الحارس الأخير حين يتسابق طلبان.
         */
        if ($clientId !== null && $clientId !== '') {
            $existing = DB::table('chat_messages')
                ->where('thread_id', $threadId)
                ->where('client_id', $clientId)
                ->first();
            if ($existing) {
                return $existing;
            }
        }

        // رسالةٌ بلا نصّ **وبلا مرفق** لا معنى لها. أمّا صورةٌ بلا تعليق
        // فرسالةٌ تامّة — ولذلك لم يعد الفراغ وحده سبب الرفض.
        if ($body === '' && $attachment === []) {
            return null;
        }
        if (mb_strlen($body) > self::MAX_BODY) {
            return null;
        }

        // نصٌّ ليس UTF-8 صحيحاً يُرفض هنا، لا في قاعدة البيانات.
        //
        // سائق SQL Server يحوّل إلى UCS-2 عند الكتابة في NVARCHAR، فبايتٌ
        // فاسد يرمي «No mapping for the Unicode character» — خطأ 500 غامض
        // بعد أن كُتب نصفُ المعاملة. ورفضُه عند الحدّ يجعله 422 مفهوماً.
        //
        // والاسم كذلك: يأتي من جلسةٍ قد تكون كُتبت من عميلٍ غير متصفّح.
        if (!mb_check_encoding($body, 'UTF-8')
            || ($name !== null && !mb_check_encoding($name, 'UTF-8'))) {
            return null;
        }

        // الردّ يجب أن يكون على رسالةٍ في المحادثة نفسها. رقمٌ من محادثةٍ
        // أخرى كان يعرض للطرف اقتباساً من كلامٍ لم يُقَل له.
        if ($replyToId !== null) {
            $ok = DB::table('chat_messages')
                ->where('id', $replyToId)
                ->where('thread_id', $threadId)
                ->exists();
            if (!$ok) {
                $replyToId = null;
            }
        }

        return DB::transaction(function () use (
            $threadId, $kind, $id, $name, $body, $attachment, $replyToId, $clientId
        ) {
            $now = now();

            $msgId = DB::table('chat_messages')->insertGetId([
                'thread_id'       => $threadId,
                'sender_kind'     => $kind,
                'sender_id'       => $id,
                'sender_name'     => $name,
                'body'            => $body === '' ? null : $body,
                'reply_to_id'     => $replyToId,
                'client_id'       => ($clientId ?? '') === '' ? null : $clientId,
                'attachment_path' => $attachment['path'] ?? null,
                'attachment_name' => $attachment['name'] ?? null,
                'attachment_mime' => $attachment['mime'] ?? null,
                'attachment_size' => $attachment['size'] ?? null,
                'attachment_kind' => $attachment['kind'] ?? null,
                'created_at'      => $now,
            ]);

            DB::table('chat_threads')->where('id', $threadId)->update([
                'last_message_at' => $now,
                'updated_at'      => $now,
            ]);

            // المرسِل قرأ رسالته بحكم كتابتها — بغير ذلك يرى عدّاداً على
            // كلامه هو.
            $this->markRead($threadId, $kind, $id, $msgId);

            return DB::table('chat_messages')->where('id', $msgId)->first();
        });
    }

    /**
     * يخزّن مرفقاً ويعيد وصفه — أو null إن رُفض.
     *
     * ## قرصٌ خاصّ لا عامّ
     *
     * مرفقات الوكلاء قد تكون صور إيصالات ووثائق عملاء. على القرص العامّ
     * تصير مقروءةً لمن عرف الرابط بلا توثيق. فهي على `local`، وتُقدَّم من
     * نقطةٍ **داخل `auth:sanctum`** — والتطبيق يمرّر ترويسة التوثيق مع
     * الصورة (`Image.network(headers:)`).
     *
     * ## الاسم على القرص لا يأتي من المستخدم
     *
     * اسم الملف يُولَّد عشوائياً (32 محرفاً) وامتداده من **نوع المحتوى
     * المُتحقَّق منه** لا من الاسم المُرسَل. اسمٌ من المستخدم يعني
     * `../../.env`، وامتدادٌ منه يعني `.php` على قرصٍ قد يُخدَم يوماً.
     * والاسم الأصلي يُحفظ في عمودٍ للعرض وحده ولا يُستعمل في أيّ مسار.
     */
    public function storeAttachment(\Illuminate\Http\UploadedFile $file): ?array
    {
        if (!$file->isValid() || $file->getSize() > self::MAX_ATTACHMENT) {
            return null;
        }

        // النوع من محتوى الملف (finfo)، لا من ترويسة الطلب: الترويسة يكتبها
        // المُرسِل ويستطيع الكذب فيها.
        $mime = $file->getMimeType();
        if (!isset(self::MIMES[$mime])) {
            return null;
        }

        [$kind, $ext] = self::MIMES[$mime];
        $name = bin2hex(random_bytes(16)) . '.' . $ext;

        $file->storeAs(self::DIR, $name, self::DISK);

        return [
            'path' => $name,
            'name' => mb_substr((string) $file->getClientOriginalName(), 0, 255),
            'mime' => $mime,
            'size' => $file->getSize(),
            'kind' => $kind,
        ];
    }

    // ══════════════════════════════════════════════════════════════════
    //  مزايا الرسالة — تفاعل، تعديل، تثبيت، حفظ  (البنود 25, 28, 30, 31)
    // ══════════════════════════════════════════════════════════════════

    /**
     * تفاعلٌ على رسالة — واحدٌ لكل شخص (البند 25).
     *
     * الضغط على الرمز نفسه يزيله، وعلى رمزٍ آخر يستبدله. وهو ما يتوقّعه
     * المستخدم: تفاعلان من شخصٍ واحد على رسالةٍ واحدة لا معنى لهما.
     *
     * ورمزٌ فارغ يعني «أزل تفاعلي».
     */
    public function react(int $messageId, int $threadId, string $kind, int $id, string $emoji): void
    {
        $emoji = trim($emoji);

        $base = DB::table('chat_reactions')
            ->where('message_id', $messageId)
            ->where('actor_kind', $kind)
            ->where('actor_id', $id);

        if ($emoji === '') {
            $base->delete();
            return;
        }

        $current = (clone $base)->value('emoji');

        if ($current === $emoji) {
            $base->delete();
            return;
        }

        if ($current !== null) {
            $base->update(['emoji' => $emoji, 'created_at' => now()]);
            return;
        }

        DB::table('chat_reactions')->insert([
            'message_id' => $messageId,
            'thread_id'  => $threadId,
            'actor_kind' => $kind,
            'actor_id'   => $id,
            'emoji'      => $emoji,
            'created_at' => now(),
        ]);
    }

    /**
     * تفاعلات صفحةٍ من الرسائل — استعلامٌ واحد لكل الصفحة.
     *
     * لا استعلامٌ لكل رسالة: صفحةٌ من خمسين رسالة كانت ستعني خمسين
     * استعلاماً في كل فتح، وهو الشكل المحظور في هذا المشروع.
     *
     * @return array<int,array<string,array{count:int,mine:bool}>>
     */
    public function reactionsFor(array $messageIds, string $kind, int $id): array
    {
        if ($messageIds === []) {
            return [];
        }

        $out = [];
        foreach (array_chunk($messageIds, 1000) as $chunk) {
            foreach (DB::table('chat_reactions')->whereIn('message_id', $chunk)->get() as $r) {
                $m = (int) $r->message_id;
                $e = $r->emoji;
                $out[$m][$e]['count'] = ($out[$m][$e]['count'] ?? 0) + 1;
                $out[$m][$e]['mine'] = ($out[$m][$e]['mine'] ?? false)
                    || ($r->actor_kind === $kind && (int) $r->actor_id === $id);
            }
        }

        return $out;
    }

    /**
     * تعديل رسالة — لصاحبها، وخلال مهلة (البند 28).
     *
     * المهلة تمنع تحريف سجلٍّ قديم: من ردّ على رسالةٍ بالأمس لا يجد نصّها
     * تغيّر تحته اليوم. و«تم التعديل» تظهر بعدها ولا يُخفى أنها عُدّلت.
     */
    public const EDIT_WINDOW_MINUTES = 15;

    public function editMessage(int $messageId, int $threadId, string $kind, int $id, string $body): ?string
    {
        $body = trim($body);
        if ($body === '' || mb_strlen($body) > self::MAX_BODY || !mb_check_encoding($body, 'UTF-8')) {
            return 'نصّ غير صالح.';
        }

        $row = DB::table('chat_messages')
            ->where('id', $messageId)
            ->where('thread_id', $threadId)
            ->first();

        if (!$row || $row->deleted_at !== null) {
            return 'الرسالة غير موجودة.';
        }
        if ($row->sender_kind !== $kind || (int) $row->sender_id !== $id) {
            return 'لا يمكن تعديل رسالة ليست لك.';
        }
        // المرفق لا يُعدَّل — يُحذف ويُرسل غيره. تعديلُه يعني أن ما رآه
        // الطرف الآخر صار شيئاً غيره بلا أثر.
        if ($row->attachment_path !== null) {
            return 'لا يمكن تعديل رسالة تحمل مرفقاً.';
        }
        if (now()->diffInMinutes($row->created_at, true) > self::EDIT_WINDOW_MINUTES) {
            return 'انقضت مهلة التعديل (' . self::EDIT_WINDOW_MINUTES . ' دقيقة).';
        }

        DB::table('chat_messages')->where('id', $messageId)
            ->update(['body' => $body, 'edited_at' => now()]);

        return null;
    }

    /** تثبيت رسالة داخل المحادثة أو فكّه (البند 31). */
    public function pinMessage(int $messageId, int $threadId, string $kind, bool $pin): bool
    {
        return DB::table('chat_messages')
            ->where('id', $messageId)
            ->where('thread_id', $threadId)
            ->whereNull('deleted_at')
            ->update([
                'pinned_at' => $pin ? now() : null,
                'pinned_by' => $pin ? $kind : null,
            ]) > 0;
    }

    /** حفظ رسالة أو إلغاء حفظها — لهذا المشارك وحده (البند 30). */
    public function star(int $messageId, int $threadId, string $kind, int $id, bool $on): void
    {
        $q = DB::table('chat_stars')
            ->where('message_id', $messageId)
            ->where('actor_kind', $kind)
            ->where('actor_id', $id);

        if (!$on) {
            $q->delete();
            return;
        }
        if ((clone $q)->exists()) {
            return;
        }

        DB::table('chat_stars')->insert([
            'message_id' => $messageId,
            'thread_id'  => $threadId,
            'actor_kind' => $kind,
            'actor_id'   => $id,
            'created_at' => now(),
        ]);
    }

    /** أرقام الرسائل المحفوظة في صفحة — استعلامٌ واحد. */
    public function starredIn(array $messageIds, string $kind, int $id): array
    {
        if ($messageIds === []) {
            return [];
        }

        return DB::table('chat_stars')
            ->whereIn('message_id', $messageIds)
            ->where('actor_kind', $kind)
            ->where('actor_id', $id)
            ->pluck('message_id')
            ->map(fn ($v) => (int) $v)
            ->all();
    }

    // ══════════════════════════════════════════════════════════════════
    //  إعدادات المحادثة — كتم، تثبيت، أرشفة، قفل  (البنود 32–34, 59)
    // ══════════════════════════════════════════════════════════════════

    /**
     * إعدادات مشاركٍ في محادثة، تُنشأ عند أول تعديل.
     *
     * صفٌّ لكل (محادثة، مشارك): الكتم رأيُ صاحبه لا صفةُ المحادثة، وكتمُ
     * الوكيل لمحادثةٍ لا يُسكتها عند موظّفه.
     */
    public function updateSettings(int $threadId, string $kind, int $id, array $changes): void
    {
        $allowed = array_intersect_key($changes, array_flip(
            ['muted_until', 'pinned_at', 'archived_at', 'locked', 'forced_unread']
        ));
        if ($allowed === []) {
            return;
        }

        $exists = DB::table('chat_settings')
            ->where('thread_id', $threadId)
            ->where('actor_kind', $kind)
            ->where('actor_id', $id)
            ->exists();

        if ($exists) {
            DB::table('chat_settings')
                ->where('thread_id', $threadId)
                ->where('actor_kind', $kind)
                ->where('actor_id', $id)
                ->update($allowed + ['updated_at' => now()]);
            return;
        }

        DB::table('chat_settings')->insert($allowed + [
            'thread_id'  => $threadId,
            'actor_kind' => $kind,
            'actor_id'   => $id,
            'updated_at' => now(),
        ]);
    }

    /** إعدادات محادثاتٍ عدّة — استعلامٌ واحد للقائمة كلّها. */
    public function settingsFor(array $threadIds, string $kind, int $id): array
    {
        if ($threadIds === []) {
            return [];
        }

        $out = [];
        foreach (
            DB::table('chat_settings')
                ->whereIn('thread_id', $threadIds)
                ->where('actor_kind', $kind)
                ->where('actor_id', $id)
                ->get() as $r
        ) {
            $out[(int) $r->thread_id] = $r;
        }

        return $out;
    }

    // ══════════════════════════════════════════════════════════════════
    //  «يكتب الآن» و«يسجّل»  (البندان 16–17)
    // ══════════════════════════════════════════════════════════════════

    /** ثوانٍ تبقى فيها الحالة قائمة بلا تجديد. */
    public const TYPING_TTL = 8;

    /**
     * يُعلن أن هذا المشارك يكتب أو يسجّل.
     *
     * صفٌّ يُكتب فوقه ولا يُراكم، وينتهي من تلقائه: من أغلق التطبيق وهو
     * يكتب لا يبقى «يكتب الآن» إلى الأبد. ولا يُحفظ في سجلّ الرسائل —
     * نصّ البند 16.
     */
    public function setTyping(int $threadId, string $kind, int $id, ?string $name, string $state): void
    {
        if (!in_array($state, ['TYPING', 'RECORDING', 'NONE'], true)) {
            return;
        }

        $q = DB::table('chat_typing')
            ->where('thread_id', $threadId)
            ->where('actor_kind', $kind)
            ->where('actor_id', $id);

        if ($state === 'NONE') {
            $q->delete();
            return;
        }

        $data = [
            'actor_name' => $name,
            'state'      => $state,
            'expires_at' => now()->addSeconds(self::TYPING_TTL),
        ];

        if ((clone $q)->exists()) {
            $q->update($data);
            return;
        }

        DB::table('chat_typing')->insert($data + [
            'thread_id'  => $threadId,
            'actor_kind' => $kind,
            'actor_id'   => $id,
        ]);
    }

    /** حالة **الطرف الآخر** — لا حالتي أنا. */
    public function typingIn(int $threadId, string $myKind): ?object
    {
        return DB::table('chat_typing')
            ->where('thread_id', $threadId)
            ->where('actor_kind', '!=', $myKind)
            ->where('expires_at', '>', now())
            ->first(['actor_name', 'state']);
    }

    // ══════════════════════════════════════════════════════════════════
    //  البحث  (البند 35)
    // ══════════════════════════════════════════════════════════════════

    /**
     * بحثٌ داخل محادثات هذا المشارك وحدها.
     *
     * ⚠ النطاق يأتي من الخادم لا من الطلب: `$threadIds` تُبنى من محادثات
     * المستخدم المُوثَّق، فلا يستطيع أحدٌ توسيع بحثه بتعديل الطلب — وهو
     * نصّ البند 35 والبند 62.
     */
    public function search(array $threadIds, string $term, int $limit = 40): array
    {
        $term = trim($term);
        if ($threadIds === [] || mb_strlen($term) < 2) {
            return [];
        }

        // تهريب محارف LIKE: `%` من المستخدم كان يجعل البحث يمسح كل شيء.
        $safe = str_replace(['[', '%', '_'], ['[[]', '[%]', '[_]'], $term);

        return DB::table('chat_messages')
            ->whereIn('thread_id', $threadIds)
            ->whereNull('deleted_at')
            ->where('body', 'like', "%{$safe}%")
            ->orderByDesc('id')
            ->limit($limit)
            ->get(['id', 'thread_id', 'sender_kind', 'sender_name', 'body', 'created_at'])
            ->all();
    }

    /** إلى أين بلغ هذا الطرف. لا يرجع إلى الوراء أبداً — القراءة لا تُنقض. */
    public function markRead(int $threadId, string $kind, int $id, ?int $upTo = null): void
    {
        $upTo ??= (int) DB::table('chat_messages')->where('thread_id', $threadId)->max('id');

        $existing = DB::table('chat_reads')
            ->where('thread_id', $threadId)
            ->where('reader_kind', $kind)
            ->where('reader_id', $id)
            ->first();

        if ($existing === null) {
            DB::table('chat_reads')->insert([
                'thread_id'            => $threadId,
                'reader_kind'          => $kind,
                'reader_id'            => $id,
                'last_read_message_id' => $upTo,
                'updated_at'           => now(),
            ]);
            return;
        }

        if ((int) $existing->last_read_message_id >= $upTo) {
            return;
        }

        DB::table('chat_reads')
            ->where('id', $existing->id)
            ->update(['last_read_message_id' => $upTo, 'updated_at' => now()]);
    }

    /**
     * غير المقروء في كل محادثة من محادثات الوكيل — استعلامان لا استعلامٌ
     * لكل محادثة.
     *
     * ورسائل الطرف نفسه لا تُعدّ: العدّاد يقول «ما ينتظر ردّك»، ورسالتُك
     * أنت لا تنتظر ردّك.
     *
     * @return array<int,int> رقم المحادثة ⇦ عدد غير المقروء
     */
    public function unreadByThread(array $threadIds, string $kind, int $id): array
    {
        if ($threadIds === []) {
            return [];
        }

        $marks = DB::table('chat_reads')
            ->whereIn('thread_id', $threadIds)
            ->where('reader_kind', $kind)
            ->where('reader_id', $id)
            ->pluck('last_read_message_id', 'thread_id');

        // عدٌّ في الخادم لا في PHP: جلبُ الرسائل كلّها ثم عدُّها هنا يعني نقل
        // كل تاريخ المحادثات عبر الشبكة في كل استطلاع، لحساب رقمٍ صغير.
        //
        // وشرطٌ واحد مركَّب لا استعلامٌ لكل محادثة: المحادثات قليلة (الإدارة
        // وموظّفو الوكيل)، لكن استعلاماً لكلٍّ منها يتكرّر مع كل نبضة.
        $clauses = [];
        $bind = [];
        foreach ($threadIds as $tid) {
            $clauses[] = '(thread_id = ? AND id > ?)';
            $bind[] = $tid;
            $bind[] = (int) ($marks[$tid] ?? 0);
        }

        $rows = DB::select(
            'SELECT thread_id, COUNT(*) AS cnt FROM chat_messages
              WHERE sender_kind <> ? AND (' . implode(' OR ', $clauses) . ')
              GROUP BY thread_id',
            array_merge([$kind], $bind)
        );

        $out = array_fill_keys($threadIds, 0);
        foreach ($rows as $r) {
            $out[(int) $r->thread_id] = (int) $r->cnt;
        }

        return $out;
    }
}
