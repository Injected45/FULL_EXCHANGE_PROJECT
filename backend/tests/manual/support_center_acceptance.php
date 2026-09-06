<?php

/*
 * اختبارات قبول مركز «الرحالة للدعم الفني».
 *
 *   php artisan serve --host=127.0.0.1 --port=8000     (في نافذة)
 *   php artisan tinker --execute="require base_path('tests/manual/support_center_acceptance.php');"
 *
 * تُنفَّذ عبر **HTTP حقيقي** لا باستدعاء الخدمات: المطلوب إثبات أن الرفض يقع
 * في الخادم عند نداء الـ API. إخفاءُ زرٍّ في واجهة React ليس أمناً، واختبارٌ
 * يستدعي الخدمة مباشرةً لا يثبت شيئاً عن الحارس.
 *
 * ⚠ ويبدأ وينتهي بلقطةٍ ماليّة: المركز طبقةُ تواصلٍ لا طبقةٌ مالية، وأيُّ
 * فرقٍ بين اللقطتين إخفاقٌ مهما نجح ما عداه.
 *
 * والاختبار **ينظّف نفسه**: يحذف حساباته ورسائله ويُعيد حالة المحادثة إلى
 * ما كانت عليه. اختبارٌ يترك أثراً في قاعدةٍ حيّة أسوأ من اختبارٍ لا يُشغَّل.
 */

use App\Services\Support\SupportPermissions;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $name, bool $pass, string $detail = '') use (&$ok, &$fail, &$failed, $line) {
    if ($pass) { $ok++; $line("  PASS  $name" . ($detail ? "  ($detail)" : '')); }
    else       { $fail++; $failed[] = $name; $line("  FAIL  $name" . ($detail ? "  ($detail)" : '')); }
};

$base = 'http://127.0.0.1:8000/api';

$call = function (string $method, string $path, ?string $token = null, array $body = [], bool $raw = false) use ($base) {
    $ch = curl_init($base . $path);
    $headers = ['Accept: application/json'];
    if (!$raw) $headers[] = 'Content-Type: application/json';
    if ($token) $headers[] = 'Authorization: Bearer ' . $token;

    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_CUSTOMREQUEST  => $method,
        CURLOPT_HTTPHEADER     => $headers,
        CURLOPT_TIMEOUT        => 30,
    ]);
    if ($body !== []) {
        curl_setopt($ch, CURLOPT_POSTFIELDS,
            $raw ? $body : json_encode($body, JSON_UNESCAPED_UNICODE));
    }
    $out  = curl_exec($ch);
    $code = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    return ['status' => $code, 'body' => json_decode($out, true), 'raw' => $out];
};

$line('══════════════════════════════════════════════════════════════');
$line('   مركز الرحالة للدعم الفني — اختبارات القبول');
$line('══════════════════════════════════════════════════════════════');

/* ─── اللقطة المالية قبل ─────────────────────────────────────────── */
$snapshot = function () {
    $one = function (string $sql) {
        try { return (string) DB::selectOne($sql)->v; } catch (\Throwable) { return 'n/a'; }
    };
    return [
        'wallet_rows'    => $one('SELECT COUNT(*) v FROM wallet'),
        'internal_rows'  => $one('SELECT COUNT(*) v FROM InternalEx'),
        'internal_sum'   => $one('SELECT ISNULL(SUM(CAST(Amount AS FLOAT)),0) v FROM InternalEx'),
        'safe_rows'      => $one('SELECT COUNT(*) v FROM EX24AccSafeActivityTb'),
        'safe_sum'       => $one('SELECT ISNULL(SUM(CAST(Amount AS FLOAT)),0) v FROM EX24AccSafeActivityTb'),
        'ledger_rows'    => $one('SELECT COUNT(*) v FROM ExchangeAccData'),
        'accounts_rows'  => $one('SELECT COUNT(*) v FROM AccountsTb'),
        'cashbox_rows'   => $one('SELECT COUNT(*) v FROM employee_cashbox_entries'),
    ];
};
$before = $snapshot();

/* ─── تنظيفٌ مسبق ────────────────────────────────────────────────── */
$purge = function (array $usernames) {
    foreach ($usernames as $u) {
        $s = DB::table('support_staff')->whereRaw('LOWER(username)=?', [$u])->first();
        if (!$s) continue;
        DB::table('support_sessions')->where('staff_id', $s->id)->delete();
        DB::table('support_permissions')->where('staff_id', $s->id)->delete();
        DB::table('support_thread_state')->where('assigned_to', $s->id)
          ->update(['assigned_to' => null, 'assigned_at' => null, 'assigned_by' => null]);
        DB::table('support_thread_state')->where('closed_by', $s->id)->update(['closed_by' => null]);
        DB::table('chat_messages')->where('support_staff_id', $s->id)
          ->update(['support_staff_id' => null]);
        DB::table('support_staff')->where('id', $s->id)->delete();
    }
};
$purge(['t.admin', 't.agent', 't.super']);

/* ─── تجهيز: مديرٌ للاختبار ─────────────────────────────────────── */
$mkStaff = function (string $username, string $name, string $role, string $pass) {
    $id = (int) DB::table('support_staff')->insertGetId([
        'name' => $name, 'username' => $username,
        'password_hash' => Hash::make($pass),
        'role' => $role, 'is_active' => 1, 'must_change' => 0,
    ]);
    $rows = [];
    foreach (SupportPermissions::ROLE_DEFAULTS[$role] as $p) {
        $rows[] = ['staff_id' => $id, 'permission' => $p, 'granted_at' => now()];
    }
    DB::table('support_permissions')->insert($rows);
    return $id;
};

$adminId = $mkStaff('t.admin', 'مدير الاختبار', SupportPermissions::ADMIN, 'Test1234');

/* محادثةُ إدارةٍ للعمل عليها. تُستعمل القائمة إن وُجدت، وإلا تُنشأ. */
$thread = DB::table('chat_threads')->where('kind', 'ADMIN')->orderBy('id')->first();
$createdThread = false;
if (!$thread) {
    $agent = DB::table('users')->orderBy('id')->first(['id']);
    $tid = (int) DB::table('chat_threads')->insertGetId([
        'agent_id' => $agent->id, 'kind' => 'ADMIN',
    ]);
    $thread = DB::table('chat_threads')->where('id', $tid)->first();
    $createdThread = true;
}
$T = (int) $thread->id;
$stateBefore = DB::table('support_thread_state')->where('thread_id', $T)->first();

$line();
$line("── ١) الدخول والجلسة ──────────────────────────────────────");

$r = $call('POST', '/support/auth/login', null, ['username' => 't.admin', 'password' => 'wrong-pass']);
$check('كلمة خاطئة تُرفض 422', $r['status'] === 422, 'status=' . $r['status']);
$check('الرسالة موحّدة لا تكشف وجود الاسم',
    str_contains((string) ($r['body']['message'] ?? ''), 'اسم الدخول أو كلمة المرور'));

$r = $call('POST', '/support/auth/login', null, ['username' => 'no.such.user', 'password' => 'Test1234']);
$check('اسمٌ غير موجود يعطي الرسالة نفسها',
    str_contains((string) ($r['body']['message'] ?? ''), 'اسم الدخول أو كلمة المرور'),
    'لا تصلح لاكتشاف الأسماء');

$r = $call('POST', '/support/auth/login', null, ['username' => 't.admin', 'password' => 'Test1234']);
$check('دخولٌ صحيح 200', $r['status'] === 200);
$adminTok = $r['body']['data']['token'] ?? null;
$check('يُصدر رمزاً', is_string($adminTok) && strlen($adminTok) === 64);
$check('يُرجع الدور والصلاحيات',
    ($r['body']['data']['staff']['role'] ?? '') === 'ADMIN'
    && count($r['body']['data']['staff']['permissions'] ?? []) === 18,
    'صلاحيات=' . count($r['body']['data']['staff']['permissions'] ?? []));
$check('يُرجع كتالوج الصلاحيات', count($r['body']['data']['catalog'] ?? []) === 3);

$check('الرمز مُجزَّأ في القاعدة لا نصّاً',
    DB::table('support_sessions')->where('token_hash', hash('sha256', $adminTok))->exists()
    && !DB::table('support_sessions')->where('token_hash', $adminTok)->exists());

$r = $call('GET', '/support/threads', null);
$check('بلا رمز ⇐ 401', $r['status'] === 401, 'status=' . $r['status']);

$r = $call('GET', '/support/threads', str_repeat('x', 64));
$check('رمزٌ مزوّر ⇐ 401', $r['status'] === 401, 'status=' . $r['status']);

$line();
$line("── ٢) Default Deny — موظّف دعمٍ عاديّ ────────────────────────");

$agentStaffId = $mkStaff('t.agent', 'موظّف الاختبار', SupportPermissions::SUPPORT_AGENT, 'Test1234');
$r = $call('POST', '/support/auth/login', null, ['username' => 't.agent', 'password' => 'Test1234']);
$agentTok = $r['body']['data']['token'] ?? null;
$check('دخول موظّف الدعم', $r['status'] === 200);
$check('صلاحياته الافتراضية ثمانٍ',
    count($r['body']['data']['staff']['permissions'] ?? []) === 8,
    'عدد=' . count($r['body']['data']['staff']['permissions'] ?? []));

$denied = [
    'إدارة الحسابات'   => ['GET',  '/support/staff', []],
    'إنشاء حساب'       => ['POST', '/support/staff', ['name' => 'x', 'username' => 'x.y', 'role' => 'ADMIN']],
    'منح الصلاحيات'    => ['PUT',  "/support/staff/$adminId/permissions", ['permissions' => ['MANAGE_STAFF']]],
    'سجلّ النشاط'      => ['GET',  '/support/audit', []],
    'تثبيت رسالة'      => ['POST', "/support/threads/$T/messages/1/pin", ['days' => 7]],
    'إعادة توجيه'      => ['POST', "/support/threads/$T/messages/1/forward", ['to_thread_id' => $T]],
];
foreach ($denied as $name => [$m, $p, $b]) {
    $r = $call($m, $p, $agentTok, $b);
    $check("مرفوض 403: $name", $r['status'] === 403, 'status=' . $r['status']);
}

$r = $call('POST', "/support/threads/$T/status", $agentTok, ['status' => 'CLOSED']);
$check('مرفوض 403: إغلاق محادثة', $r['status'] === 403, 'status=' . $r['status']);

$r = $call('POST', "/support/threads/$T/assign", $agentTok, ['staff_id' => $adminId]);
$check('مرفوض 403: الإسناد لغيره', $r['status'] === 403, 'status=' . $r['status']);

$allowed = [
    'عرض المحادثات' => ['GET', '/support/threads'],
    'عدّاد غير المقروء' => ['GET', '/support/threads/unread'],
    'البحث' => ['GET', '/support/search?q=' . rawurlencode('اختبار')],
    'قائمة المُسنَد إليهم' => ['GET', '/support/assignees'],
];
foreach ($allowed as $name => [$m, $p]) {
    $r = $call($m, $p, $agentTok);
    $check("مسموح 200: $name", $r['status'] === 200, 'status=' . $r['status']);
}

$r = $call('POST', "/support/threads/$T/assign", $agentTok, ['staff_id' => $agentStaffId]);
$check('مسموح: الاستلام لنفسه', $r['status'] === 200, 'status=' . $r['status']);

/* عشرُ محاولاتٍ رُفضت أعلاه: ستٌّ يرفضها الوسيط، وأربعٌ يرفضها المتحكّم
   لأن التمييز فيها يحتاج جسم الطلب. والعشرُ يجب أن تُسجَّل — سجلٌّ يُظهر
   بعضَ الرفض يجعل من يقرؤه يطمئنّ في غير موضعه. */
$deniedLogged = DB::table('support_audit')->where('staff_id', $agentStaffId)
    ->where('action', 'DENIED')->count();
$check('كل محاولة مرفوضة سُجِّلت باسمه — من الوسيط ومن المتحكّم معاً',
    $deniedLogged >= 8, "عدد=$deniedLogged");

$line();
$line("── ٣) سقفُ الدور فوق المنح ───────────────────────────────────");

$r = $call('PUT', "/support/staff/$agentStaffId/permissions", $adminTok,
    ['permissions' => ['VIEW_THREADS', 'REPLY', 'MANAGE_STAFF']]);
$check('المدير لا يستطيع منح موظّف دعمٍ صلاحيةً فوق سقفه',
    $r['status'] === 422, 'status=' . $r['status']);
$check('ولا تُكتب في القاعدة',
    !DB::table('support_permissions')->where('staff_id', $agentStaffId)
        ->where('permission', 'MANAGE_STAFF')->exists());

$r = $call('PUT', "/support/staff/$adminId/permissions", $adminTok, ['permissions' => []]);
$check('ولا يعدّل المديرُ صلاحياتِ نفسه', $r['status'] === 422, 'status=' . $r['status']);

$r = $call('PUT', "/support/staff/$adminId", $adminTok, ['is_active' => false]);
$check('ولا يوقف حسابَه بنفسه', $r['status'] === 422, 'status=' . $r['status']);

/* خفضُ الدور يسحب ما صار فوق السقف. */
$superId = $mkStaff('t.super', 'مشرف الاختبار', SupportPermissions::SUPERVISOR, 'Test1234');
$hadClose = DB::table('support_permissions')->where('staff_id', $superId)
    ->where('permission', 'CLOSE_THREAD')->exists();
$r = $call('PUT', "/support/staff/$superId", $adminTok, ['role' => 'SUPPORT_AGENT']);
$stillClose = DB::table('support_permissions')->where('staff_id', $superId)
    ->where('permission', 'CLOSE_THREAD')->exists();
$check('خفضُ الدور يسحب ما صار فوق السقف',
    $hadClose && !$stillClose, 'CLOSE_THREAD قبل=' . ($hadClose ? 'نعم' : 'لا') . ' بعد=' . ($stillClose ? 'نعم' : 'لا'));

$line();
$line("── ٤) العزل — محادثة الوكيل مع موظّفه ────────────────────────");

$emp = DB::table('chat_threads')->where('kind', 'EMPLOYEE')->first();
if ($emp) {
    $r = $call('GET', "/support/threads/{$emp->id}", $adminTok);
    $check('فتحُ محادثة موظّف-وكيل ⇐ 404', $r['status'] === 404, 'status=' . $r['status']);
    $r = $call('POST', "/support/threads/{$emp->id}/messages", $adminTok, ['body' => 'اختراق']);
    $check('الإرسال فيها ⇐ 404', $r['status'] === 404, 'status=' . $r['status']);
    $r = $call('POST', "/support/threads/{$emp->id}/assign", $adminTok, ['staff_id' => $adminId]);
    $check('إسنادُها ⇐ 404', $r['status'] === 404, 'status=' . $r['status']);
} else {
    $line('  SKIP  لا توجد محادثة موظّف-وكيل في القاعدة');
}

$r = $call('GET', '/support/threads/99999999', $adminTok);
$check('محادثةٌ غير موجودة ⇐ 404', $r['status'] === 404, 'status=' . $r['status']);

$listed = $call('GET', '/support/threads', $adminTok)['body']['data']['items'] ?? [];
$empIds = DB::table('chat_threads')->where('kind', 'EMPLOYEE')->pluck('id')
    ->map(fn ($v) => (int) $v)->all();
$leak = array_intersect(array_column($listed, 'id'), $empIds);
$check('لا محادثة موظّف في القائمة أصلاً', $leak === [], 'تسرّب=' . count($leak));

$line();
$line("── ٥) دورة العمل: إسناد وحالة وردّ ──────────────────────────");

/* شرطٌ ابتدائيّ مضبوط: المحادثة تُعاد إلى `NEW` بلا مالك قبل الاختبار.
   بغيره يقيس الاختبار حالةً تركها تشغيلٌ سابق ويُخفق بلا سبب — وذلك ما
   حدث فعلاً في أوّل تشغيل. */
DB::table('support_thread_state')->where('thread_id', $T)->delete();

$r = $call('POST', "/support/threads/$T/assign", $adminTok, ['staff_id' => $adminId]);
$check('الإسناد 200', $r['status'] === 200);
$st = DB::table('support_thread_state')->where('thread_id', $T)->first();
$check('الاستلام ينقل NEW إلى OPEN', $st->status === 'OPEN', 'الحالة=' . $st->status);

$cid = 'acc-' . bin2hex(random_bytes(8));
$r = $call('POST', "/support/threads/$T/messages", $adminTok,
    ['body' => 'رسالة قبولٍ آلية — تُحذف تلقائياً.', 'client_id' => $cid]);
$check('الردّ 200', $r['status'] === 200);
$msgId = (int) ($r['body']['data']['message']['id'] ?? 0);
$check('الرسالة تحمل اسم من ردّ',
    ($r['body']['data']['message']['staff_name'] ?? '') === 'مدير الاختبار');
$check('sender_id يبقى 0 (طرفٌ واحد في نظر الوكيل)',
    (int) DB::table('chat_messages')->where('id', $msgId)->value('sender_id') === 0);
$check('support_staff_id يحمل الهويّة الحقيقية',
    (int) DB::table('chat_messages')->where('id', $msgId)->value('support_staff_id') === $adminId);

$r2 = $call('POST', "/support/threads/$T/messages", $adminTok,
    ['body' => 'رسالة قبولٍ آلية — تُحذف تلقائياً.', 'client_id' => $cid]);
$check('إعادةُ الإرسال بنفس client_id لا تُكرّر',
    (int) ($r2['body']['data']['message']['id'] ?? -1) === $msgId,
    "الأول=$msgId الثاني=" . ($r2['body']['data']['message']['id'] ?? '-'));

$st = DB::table('support_thread_state')->where('thread_id', $T)->first();
$check('الردّ ينقل الحالة إلى PENDING', $st->status === 'PENDING', 'الحالة=' . $st->status);

$line();
$line("── ٦) مزايا الرسالة ─────────────────────────────────────────");

foreach ([
    'تفاعل'          => ['POST', "/support/threads/$T/messages/$msgId/react", ['emoji' => '👍']],
    'نجمة'           => ['POST', "/support/threads/$T/messages/$msgId/star",  ['on' => true]],
    'تثبيت أسبوعاً'  => ['POST', "/support/threads/$T/messages/$msgId/pin",   ['days' => 7]],
    'تعديل'          => ['PUT',  "/support/threads/$T/messages/$msgId",       ['body' => 'نصٌّ مُعدَّل — قبول آليّ.']],
] as $name => [$m, $p, $b]) {
    $r = $call($m, $p, $adminTok, $b);
    $check("$name 200", $r['status'] === 200, 'status=' . $r['status']);
}

$d = $call('GET', "/support/threads/$T", $adminTok)['body']['data'] ?? [];
$check('التفاعل ظهر', isset($d['reactions'][$msgId]['👍']));
$check('النجمة ظهرت', in_array($msgId, $d['starred'] ?? [], true));
$check('التثبيت ظهر في الشريط', (int) ($d['pinned']['id'] ?? 0) === $msgId);
$row = DB::table('chat_messages')->where('id', $msgId)->first();
$check('التعديل مُعلَن (edited_at)', $row->edited_at !== null);
$check('النصّ تغيّر فعلاً', str_contains((string) $row->body, 'مُعدَّل'));

/* ⚠ المقارنة غير صارمة عمداً: مُشغّل SQL Server يُرجع `BIGINT` **نصّاً**،
   فـ `id` يصل `"37"` لا `37`. وهذا ما كشفه أوّل تشغيل، وهو عيبٌ حقيقيّ في
   واجهة React عولج بتسوية الأنواع في `api.js` — لأن `[36].includes("36")`
   تعطي `false` فتختفي النجمة ولا يعمل القفز إلى الرسالة، بلا رسالة خطأ. */
$r = $call('GET', '/support/search?q=' . rawurlencode('قبول'), $adminTok);
$found = array_map('intval', array_column($r['body']['data']['items'] ?? [], 'id'));
$check('البحث يجد الرسالة', in_array($msgId, $found, true),
    'نتائج=' . count($found) . ' · ids=' . implode(',', $found));
$check('مُعرّفات الرسائل تعود نصّاً من المُشغّل (موثَّق ومُسوّى في الواجهة)',
    is_string($r['body']['data']['items'][0]['id'] ?? null));

$check('المحادثة تحمل بيانات الوكيل واسمَه',
    isset($d['agent']['id']) && ($d['agent']['name'] ?? '') !== '');
$check('الإيصالات موجودة',
    isset($d['receipts']['delivered'], $d['receipts']['read']));

$line();
$line("── ٧) رسالةُ الوكيل تُعيد فتح المحادثة ───────────────────────");

$call('POST', "/support/threads/$T/status", $adminTok, ['status' => 'CLOSED', 'note' => 'قبول آليّ']);
$st = DB::table('support_thread_state')->where('thread_id', $T)->first();
$check('الإغلاق يعمل', $st->status === 'CLOSED' && $st->closed_at !== null);

$agentUser = DB::table('users')->where('id', $thread->agent_id)->first();
$reopened = false;
if ($agentUser) {
    $tok = null;
    try {
        $u = \App\Models\User::find($agentUser->id);
        $tok = $u?->createToken('support-acceptance')->plainTextToken;
    } catch (\Throwable) { /* يُتجاوَز أدناه */ }

    if ($tok) {
        $rr = $call('POST', "/chat/threads/$T/messages", $tok,
            ['body' => 'رسالة وكيلٍ آلية — تُحذف تلقائياً.', 'client_id' => 'acc-agent-' . bin2hex(random_bytes(6))]);
        $check('الوكيل يرسل عبر مساره هو 200', $rr['status'] === 200, 'status=' . $rr['status']);
        $agentMsgId = (int) ($rr['body']['data']['message']['id'] ?? 0);

        $st = DB::table('support_thread_state')->where('thread_id', $T)->first();
        $reopened = in_array($st->status, ['OPEN', 'NEW'], true);
        $check('رسالتُه تُخرج المحادثة من CLOSED', $reopened, 'الحالة=' . $st->status);
        $check('و`closed_at` تُمسح', $st->closed_at === null);

        /* تنظيف رسالة الوكيل ورمزه. */
        if ($agentMsgId) DB::table('chat_messages')->where('id', $agentMsgId)->delete();
        DB::table('personal_access_tokens')->where('name', 'support-acceptance')->delete();
    } else {
        $line('  SKIP  تعذّر إصدار رمز وكيلٍ للاختبار');
    }
}

$line();
$line("── ٨) إدارة الحسابات ────────────────────────────────────────");

$r = $call('POST', '/support/staff', $adminTok,
    ['name' => 'حسابٌ مؤقّت', 'username' => 'BAD NAME!', 'role' => 'SUPPORT_AGENT']);
$check('اسمُ دخولٍ غير صالح يُرفض', $r['status'] === 422, 'status=' . $r['status']);

$r = $call('POST', '/support/staff', $adminTok,
    ['name' => 'حسابٌ مؤقّت', 'username' => 't.agent', 'role' => 'SUPPORT_AGENT']);
$check('اسمٌ مستعمَل يُرفض', $r['status'] === 422, 'status=' . $r['status']);

$r = $call('POST', '/support/staff', $adminTok,
    ['name' => 'حسابٌ مؤقّت', 'username' => 't.agent', 'role' => 'NOT_A_ROLE']);
$check('دورٌ غير معروف يُرفض', $r['status'] === 422, 'status=' . $r['status']);

$r = $call('POST', "/support/staff/$agentStaffId/password", $adminTok);
$newPass = $r['body']['data']['password'] ?? '';
$check('تصفير كلمة المرور يُرجع كلمةً جديدة', $r['status'] === 200 && strlen($newPass) >= 12);
$check('وكلمةُ المرور لا تُخزَّن نصّاً',
    !DB::table('support_staff')->where('id', $agentStaffId)
        ->where('password_hash', $newPass)->exists());
$check('والجلسة القديمة سقطت فوراً',
    $call('GET', '/support/threads', $agentTok)['status'] === 401);

$r = $call('POST', '/support/auth/login', null, ['username' => 't.agent', 'password' => $newPass]);
$check('الدخول بالكلمة الجديدة يعمل', $r['status'] === 200);
$check('ويُطلب تغييرها', ($r['body']['data']['staff']['must_change'] ?? false) === true);
$agentTok = $r['body']['data']['token'] ?? null;

$r = $call('PUT', "/support/staff/$agentStaffId", $adminTok, ['is_active' => false]);
$check('الإيقاف 200', $r['status'] === 200);
$check('وجلستُه تسقط في الطلب التالي',
    $call('GET', '/support/threads', $agentTok)['status'] === 401);
$check('ولا يستطيع الدخول',
    str_contains((string) ($call('POST', '/support/auth/login', null,
        ['username' => 't.agent', 'password' => $newPass])['body']['message'] ?? ''), 'موقوف'));

$line();
$line("── ٩) سجلّ النشاط ───────────────────────────────────────────");

$r = $call('GET', '/support/audit', $adminTok);
$check('السجلّ يُقرأ بالصلاحية 200', $r['status'] === 200);
$items = $r['body']['data']['items'] ?? [];
$acts = array_unique(array_column($items, 'action'));
foreach (['LOGIN', 'REPLY', 'ASSIGN', 'CLOSE', 'DENIED', 'STAFF_DISABLE', 'PERM_REVOKE'] as $a) {
    $check("السجلّ يحوي $a", in_array($a, $acts, true));
}
$check('كل قيدٍ يحمل اسم فاعله',
    count(array_filter($items, fn ($i) => ($i['staff_name'] ?? '') !== '')) === count($items));

$line();
$line("── ١٠) الخروج ───────────────────────────────────────────────");

$r = $call('POST', '/support/auth/logout', $adminTok);
$check('الخروج 200', $r['status'] === 200);
$check('والرمز لا يعمل بعده', $call('GET', '/support/threads', $adminTok)['status'] === 401);

$line();
$line("── ١١) الصفحة والباب القديم ─────────────────────────────────");

$pageUrl = 'http://127.0.0.1:8000/support';
$page = @file_get_contents($pageUrl);
$check('صفحة /support تُقدَّم', $page !== false && str_contains((string) $page, 'الرحالة للدعم الفني'));

/*
 * ⚠ المسار يُحلّ **كما يحلّه المتصفّح** — لا يُركَّب بيدنا.
 *
 * كان هذا الفحص يجلب الأصل من `/support/assets/…` مباشرةً، فمرّ بينما
 * الصفحة **لا تعمل في أي متصفّح**: `base` كان نسبيّاً (`./`) والصفحة
 * تُقدَّم على `/support` بلا شرطة مائلة، فيطلب المتصفّح `/assets/…` من جذر
 * الموقع ويعود 404 وتبقى الصفحة بيضاء. اختبارٌ يبني المسار الصحيح بنفسه
 * لا يختبر شيئاً — يختبر أننا نعرف المسار الصحيح.
 */
$resolve = function (string $base, string $href) {
    if (str_starts_with($href, 'http')) return $href;
    if (str_starts_with($href, '/')) {
        $p = parse_url($base);
        return $p['scheme'] . '://' . $p['host'] . ($p['port'] ? ':' . $p['port'] : '') . $href;
    }
    // نسبيّ: يُحلّ من مجلّد الصفحة — و`/support` مجلّدُه `/`.
    return preg_replace('#[^/]*$#', '', $base) . preg_replace('#^\./#', '', $href);
};

if (preg_match('#<script[^>]+src="([^"]+)"#', (string) $page, $m)) {
    $url = $resolve($pageUrl, $m[1]);
    $js  = @file_get_contents($url);
    $check('حزمة React تُحمَّل من المسار الذي يطلبه المتصفّح فعلاً',
        $js !== false && strlen($js) > 100000,
        $url . ' ⇐ ' . ($js === false ? '404' : round(strlen($js) / 1024) . ' KB'));
} else {
    $check('حزمة React تُحمَّل', false, 'لا وسم script في الصفحة');
}

if (preg_match('#<link[^>]+href="([^"]+\.css)"#', (string) $page, $m)) {
    $url = $resolve($pageUrl, $m[1]);
    $css = @file_get_contents($url);
    $check('التنسيق يُحمَّل من المسار نفسه', $css !== false && strlen($css) > 5000,
        $url . ' ⇐ ' . ($css === false ? '404' : round(strlen($css) / 1024) . ' KB'));
}
$check('المفتاح المشترك القديم مُفرَغ ⇐ /admin/chat مغلقة',
    (string) config('chat.admin_key', '') === '');

$line();
$line("── ١٢) العزل البنيويّ عن المال ──────────────────────────────");

$fk = DB::select("SELECT OBJECT_NAME(referenced_object_id) d FROM sys.foreign_keys
                   WHERE OBJECT_NAME(parent_object_id) LIKE 'support[_]%'");
$dests = array_unique(array_column($fk, 'd'));
sort($dests);
$check('كل المفاتيح الأجنبية داخلية',
    $dests === ['chat_threads', 'support_staff'], implode(' · ', $dests));

$tr = DB::select("SELECT name FROM sys.triggers WHERE OBJECT_NAME(parent_id) LIKE 'support[_]%'");
$check('لا محفّزات على جداول الدعم', count($tr) === 0, 'عدد=' . count($tr));

$line();
$line("── تنظيف ────────────────────────────────────────────────────");

/* ⚠ الترتيب لازم: `chat_reactions` و`chat_stars` لهما مفتاحٌ أجنبيّ إلى
   الرسالة، وحذفُها أوّلاً يُسقط الحذف بخطأ قيدٍ مرجعيّ — وقد أسقط تشغيلَ
   الاختبار الأوّل قبل أن ينظّف شيئاً، فبقيت بيانات الاختبار في القاعدة. */
if ($msgId) {
    DB::table('chat_reactions')->where('message_id', $msgId)->delete();
    DB::table('chat_stars')->where('message_id', $msgId)->delete();
    DB::table('chat_messages')->where('reply_to_id', $msgId)->update(['reply_to_id' => null]);
    DB::table('chat_messages')->where('id', $msgId)->delete();
}
$purge(['t.admin', 't.agent', 't.super']);
DB::table('support_audit')->whereIn('staff_name',
    ['مدير الاختبار', 'موظّف الاختبار', 'مشرف الاختبار'])->delete();

/* حالة المحادثة تعود كما كانت. */
if ($stateBefore) {
    DB::table('support_thread_state')->where('thread_id', $T)->update([
        'status' => $stateBefore->status,
        'assigned_to' => $stateBefore->assigned_to,
        'assigned_at' => $stateBefore->assigned_at,
        'assigned_by' => $stateBefore->assigned_by,
        'closed_at' => $stateBefore->closed_at,
        'closed_by' => $stateBefore->closed_by,
        'close_note' => $stateBefore->close_note,
    ]);
} else {
    DB::table('support_thread_state')->where('thread_id', $T)->delete();
}
if ($createdThread) DB::table('chat_threads')->where('id', $T)->delete();
$line('  نُظِّف: الحسابات والرسائل وقيود السجلّ، وأُعيدت حالة المحادثة.');

$line();
$line("── اللقطة المالية ───────────────────────────────────────────");

$after = $snapshot();
$same = true;
foreach ($before as $k => $v) {
    $eq = ($v === $after[$k]);
    if (!$eq) $same = false;
    printf("  %-16s %-16s ⇦ %-16s  %s\n", $k, $v, $after[$k], $eq ? '✓' : '✗ تغيّر');
}
$check('لا شيء ماليّ تغيّر', $same);

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) { $line('   المُخفِق: ' . implode(' · ', $failed)); }
$line('══════════════════════════════════════════════════════════════');
