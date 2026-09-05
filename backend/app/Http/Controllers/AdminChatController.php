<?php

namespace App\Http\Controllers;

use App\Services\ChatService;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\RateLimiter;

/**
 * صندوق وارد الإدارة — صفحة ويب على خادم الشركة، لا تطبيق ولا شاشة في
 * المنظومة المكتبية.
 *
 * سببُ كونها صفحة ويب: موظّف الإدارة يحتاج أن يردّ، والمنظومة المكتبية
 * (VB.NET) مرجعٌ لا يُعدَّل في عمل التطبيق، وبناء تطبيقٍ ثالث لأجل صندوق
 * رسائل مبالغة. صفحةٌ على الخادم القائم تكفي وتُفتح من أي جهاز في المكتب.
 *
 * ## الدخول: مفتاحٌ في `.env`، لا حسابات
 *
 * ⚠ **هذا مفتاح مشترك لا هويّة شخصية.** من يعرفه يدخل. وهو مقبولٌ لصفحةٍ
 * داخلية على خادم الشركة لا تصل إلى مالٍ ولا رصيد — لكنه **لا يقول من
 * ردّ**. ولذلك يكتب الموظّف اسمه مرّةً عند الدخول، فيُحفظ مع كل رسالة
 * يرسلها: السجلّ يبقى قادراً على الإجابة عن «من ردّ على الوكيل؟».
 *
 * وإن أراد المالك هويّةً حقيقية لكل موظّف إدارة، فذلك تسجيل دخول كامل
 * يُقرَّر على حدة — ولا يُخترع هنا.
 *
 * ⚠ ولا شيء في هذه الصفحة يمسّ المال: لا رصيد ولا قيد ولا حوالة.
 */
class AdminChatController extends Controller
{
    public function __construct(private ChatService $chat)
    {
    }

    private function key(): ?string
    {
        $k = (string) config('chat.admin_key', '');
        return $k === '' ? null : $k;
    }

    private function signedIn(Request $r): bool
    {
        return $r->session()->get('chat_admin_ok') === true;
    }

    /** شاشة الدخول. */
    public function login(Request $r)
    {
        if ($this->signedIn($r)) {
            return redirect('/admin/chat');
        }

        return view('admin.chat_login', [
            'configured' => $this->key() !== null,
            'error'      => $r->session()->get('chat_error'),
        ]);
    }

    public function doLogin(Request $r)
    {
        $key = $this->key();
        if ($key === null) {
            return back()->with('chat_error', 'لم يُضبط مفتاح الدخول على الخادم.');
        }

        // خنقٌ بالعنوان: مفتاحٌ واحد مشترك يعني أن التخمين ممكن، والحدّ هو
        // ما يجعله غير عملي. عشر محاولات في الدقيقة.
        $bucket = 'chat-admin:' . $r->ip();
        if (RateLimiter::tooManyAttempts($bucket, 10)) {
            return back()->with('chat_error', 'محاولات كثيرة. انتظر دقيقة.');
        }
        RateLimiter::hit($bucket, 60);

        // مقارنةٌ ثابتة الزمن: المقارنة العادية تُسرّب طول المطابقة.
        if (!hash_equals($key, (string) $r->input('key'))) {
            return back()->with('chat_error', 'المفتاح غير صحيح.');
        }

        $name = trim((string) $r->input('name'));
        if ($name === '') {
            return back()->with('chat_error', 'اكتب اسمك — يظهر مع ردودك.');
        }

        RateLimiter::clear($bucket);
        $r->session()->put('chat_admin_ok', true);
        $r->session()->put('chat_admin_name', mb_substr($name, 0, 100));

        return redirect('/admin/chat');
    }

    public function logout(Request $r)
    {
        $r->session()->forget(['chat_admin_ok', 'chat_admin_name']);
        return redirect('/admin/chat/login');
    }

    /** قائمة محادثات الوكلاء مع الإدارة، وأكثرها انتظاراً في الأعلى. */
    public function index(Request $r)
    {
        if (!$this->signedIn($r)) {
            return redirect('/admin/chat/login');
        }

        // محادثات الإدارة وحدها: محادثة الوكيل مع موظّفه شأنهما، ولا تُعرض
        // هنا ولو كانت على الخادم نفسه.
        $threads = DB::table('chat_threads as t')
            ->leftJoin('users as u', 'u.id', '=', 't.agent_id')
            ->where('t.kind', ChatService::ADMIN)
            ->orderByDesc('t.last_message_at')
            ->get(['t.id', 't.agent_id', 't.last_message_at', 'u.name', 'u.phone']);

        $ids = $threads->pluck('id')->map(fn ($v) => (int) $v)->all();
        $unread = $this->chat->unreadByThread($ids, ChatService::ADMIN, 0);

        $open = (int) $r->query('thread', 0);
        if ($open === 0 && $threads->isNotEmpty()) {
            $open = (int) $threads->first()->id;
        }

        $messages = [];
        if ($open > 0 && in_array($open, $ids, true)) {
            $messages = $this->chat->messages($open, 0, 200);
            $this->chat->markRead($open, ChatService::ADMIN, 0);
        }

        return view('admin.chat', [
            'threads'  => $threads,
            'unread'   => $unread,
            'open'     => $open,
            'messages' => $messages,
            'me'       => $r->session()->get('chat_admin_name'),
        ]);
    }

    public function send(Request $r, int $id)
    {
        if (!$this->signedIn($r)) {
            return redirect('/admin/chat/login');
        }

        $thread = DB::table('chat_threads')->where('id', $id)->first();
        if (!$thread || $thread->kind !== ChatService::ADMIN) {
            return redirect('/admin/chat');
        }

        $this->chat->send(
            $id,
            ChatService::ADMIN,
            // ‏0 لأن المفتاح مشترك ولا رقم مستخدمٍ خلفه. والاسم هو ما يُقرأ
            // في السجلّ — انظر ترويسة الصنف.
            0,
            $r->session()->get('chat_admin_name'),
            (string) $r->input('body', '')
        );

        return redirect('/admin/chat?thread=' . $id);
    }

    /**
     * نقطة الاستطلاع للصفحة — ترجع الجديد وحده.
     *
     * لتظهر رسالة الوكيل بلا أن يُعيد موظّف الإدارة تحميل الصفحة، وبلا أن
     * يُسحب تاريخ المحادثة في كل نبضة.
     */
    public function poll(Request $r, int $id)
    {
        if (!$this->signedIn($r)) {
            return response()->json(['items' => []], 401);
        }

        $after = max(0, (int) $r->query('after_id', 0));
        $items = $this->chat->messages($id, $after, 100);

        if ($items !== []) {
            $this->chat->markRead($id, ChatService::ADMIN, 0);
        }

        return response()->json(['items' => $items]);
    }
}
