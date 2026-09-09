<?php

/*
 * ════════════════════════════════════════════════════════════════════════════
 *  شرطُ المالك: يُنشأ الموظف **بلا صلاحيةٍ واحدة**، وكلُّ ما يمنحه الوكيل يظهر
 * ════════════════════════════════════════════════════════════════════════════
 *
 *   php artisan tinker --execute="require base_path('tests/manual/employee_default_deny_check.php');"
 *
 * ويحتاج خادماً على 127.0.0.1:8000.
 *
 * ⚠ **يُنشأ الموظف بمسار الوكيل الحقيقيّ** (`POST employees`) لا بإدراجٍ في
 * الجدول: المقصودُ إثباتُ أنّ **الإنشاء نفسَه** لا يمنح شيئاً — وإدراجٌ يدويّ
 * فارغ كان سيُثبت أنّ الفارغَ فارغ لا غير.
 */

use App\Services\Employees\EmployeePermissions as P;
use Illuminate\Support\Facades\DB;

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $skip = 0;
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, $line) {
    if ($p) { $ok++;   $line('  PASS  ' . $n . ($d ? "  ($d)" : '')); }
    else    { $fail++; $line('  FAIL  ' . $n . ($d ? "  ($d)" : '')); }
};

$line('════════════════════════════════════════════════════════════');
$line(' Default Deny: الموظفُ يُنشأ فارغاً، والمنحُ وحدَه يفتح');
$line('════════════════════════════════════════════════════════════');

$before = [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => DB::table('InternalEx')->count(),
];

$base  = 'http://127.0.0.1:8000/api';
$sock  = @fsockopen('127.0.0.1', 8000, $e1, $e2, 1);
if ($sock === false) {
    $line('  SKIP  كل الفحوص  (لا خادم على 127.0.0.1:8000)');
    return;
}
fclose($sock);

$agentId = 104;
$phone   = '911234797';

/* رمزُ الوكيل — المالكُ نفسُه ينشئ الموظف. */
$agent = \App\Models\User::find($agentId);
if (!$agent) {
    $line('  SKIP  لا وكيل بالمعرّف ' . $agentId);
    return;
}
$agentToken = $agent->createToken('default-deny-check')->plainTextToken;

$call = function (string $method, string $path, ?string $token, ?array $body = null)
        use ($base) {
    $ch = curl_init($base . $path);
    $headers = ['Accept: application/json', 'Content-Type: application/json'];
    if ($token !== null) { $headers[] = 'Authorization: Bearer ' . $token; }
    curl_setopt_array($ch, [
        CURLOPT_CUSTOMREQUEST  => $method,
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_TIMEOUT        => 25,
        CURLOPT_HTTPHEADER     => $headers,
    ]);
    if ($body !== null) {
        curl_setopt($ch, CURLOPT_POSTFIELDS,
            json_encode($body, JSON_UNESCAPED_UNICODE));
    }
    $raw = curl_exec($ch);
    $st  = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);
    return ['status' => $st, 'json' => json_decode((string) $raw, true) ?: [],
            'raw' => (string) $raw];
};

/* تنظيفُ بقايا سابقة. */
$stale = DB::table('employees')->where('phone', $phone)->pluck('id');
if ($stale->isNotEmpty()) {
    DB::table('employee_permissions')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_sessions')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_devices')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_activation_codes')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_otps')->whereIn('employee_id', $stale)->delete();
    $t = DB::table('chat_threads')->whereIn('employee_id', $stale)->pluck('id');
    if ($t->isNotEmpty()) {
        DB::table('chat_reads')->whereIn('thread_id', $t)->delete();
        DB::table('chat_messages')->whereIn('thread_id', $t)->delete();
        DB::table('chat_threads')->whereIn('id', $t)->delete();
    }
    DB::table('employees')->whereIn('id', $stale)->delete();
}

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ١) الإنشاءُ بمسار الوكيل لا يمنح شيئاً ────────────────────');

$r = $call('POST', '/employees', $agentToken, [
    'full_name' => 'موظف Default Deny',
    'phone'     => $phone,
]);
$check('1. أُنشئ الموظف', in_array($r['status'], [200, 201], true),
    'HTTP ' . $r['status']);

$employeeId = (int) DB::table('employees')->where('phone', $phone)->value('id');
$check('2. وهو في الجدول', $employeeId > 0, 'id=' . $employeeId);

$granted = DB::table('employee_permissions')
    ->where('employee_id', $employeeId)->count();
$check('3. ⚠ وبصفرِ صلاحيات — لا افتراضيّ ولا «الأساسيات»',
    $granted === 0, 'صفوف = ' . $granted);

/* ⚠ ولا حتّى حين يُرسل الطلبُ مصفوفةً فارغة صراحةً. */
$check('4. ولا صفٌّ لمفتاحٍ غير قابلٍ للمنح تسلّل',
    DB::table('employee_permissions')->where('employee_id', $employeeId)
        ->whereNotIn('permission_key', array_keys(P::CATALOG))->count() === 0);

/*
 * ⚠ وحالتُه `PENDING_ACTIVATION` لا `ACTIVE` — وهذا صحيحٌ لا خلل: الموظف
 * لا يعمل قبل أن يُفعّل جهازَه بكودٍ وOTP. فجلستُه لا تُفتح أصلاً، وهي
 * طبقةُ منعٍ سابقةٌ للصلاحيات.
 */
$status = DB::table('employees')->where('id', $employeeId)->value('status');
$check('4ب. ⚠ ويُنشأ «بانتظار التفعيل» لا فعّالاً',
    $status === 'PENDING_ACTIVATION', $status);

/* ولإكمال الفحص نُفعّله — كما يفعل التفعيلُ الحقيقيّ. */
DB::table('employees')->where('id', $employeeId)->update(['status' => 'ACTIVE']);

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٢) وجلستُه ترى صفراً ──────────────────────────────────────');

$token  = bin2hex(random_bytes(24));
$device = hash('sha256', 'deny-' . $employeeId);
DB::table('employee_devices')->insert([
    'agent_id' => $agentId, 'employee_id' => $employeeId,
    'device_hash' => $device, 'status' => 'ACTIVE', 'activated_at' => now(),
]);
DB::table('employee_sessions')->insert([
    'agent_id' => $agentId, 'employee_id' => $employeeId,
    'access_token_hash' => hash('sha256', $token),
    'device_hash' => $device, 'status' => 'ACTIVE', 'created_at' => now(),
]);

$me = $call('GET', '/device/employee/me', $token);
$perms = $me['json']['data']['permissions'] ?? null;
$check('5. `employee/me` يفتح', $me['status'] === 200, 'HTTP ' . $me['status']);
$check('6. ⚠ ويُعيد قائمةَ صلاحياتٍ فارغة — لا غائبة',
    is_array($perms) && $perms === [],
    is_array($perms) ? 'عدد = ' . count($perms) : 'ليست مصفوفة');

/*
 * ⚠ والتطبيقُ يبني واجهتَه من هذه القائمة وحدَها. فارغةٌ ⇦ لافتة «لم تُمنح
 * صلاحيات بعد» ولا بلاطة. يحرسه `test/employee_home_permissions_test.dart`.
 */

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٣) وكلُّ مفتاحٍ يُمنح يفتح بابَه — واحداً واحداً ──────────');

/* المفاتيحُ التي يفرضها مسارٌ مباشر، وطريقُ فحصِ كلٍّ منها. */
$probe = [
    'VIEW_INCOMING_TRANSFERS' => ['GET', '/device/employee/transfers/incoming?status=DELIVERED'],
    'VIEW_OWN_CASHBOX'        => ['GET', '/device/employee/cashbox'],
    'SEARCH_TRANSFER'         => ['GET', '/device/employee/transfers/search?code=X'],
    'VIEW_OWN_TRANSFERS'      => ['GET', '/device/employee/transfers/mine'],
    'VIEW_POS_TRANSFERS'      => ['GET', '/device/employee/transfers/point-of-sale'],
    'VIEW_FINANCIAL_SUMMARY'  => ['GET', '/device/employee/summary'],
    'VIEW_AGENT_TOTAL_BALANCE'=> ['GET', '/device/employee/balance'],
    'REPORT_DAILY_TRANSFERS'  => ['GET', '/device/employee/reports/daily'],
    'REPORT_AGENT_BALANCE'    => ['GET', '/device/employee/reports/agent-balance'],
    'REPORT_AUDIT'            => ['GET', '/device/employee/reports/audit'],
    'VIEW_FAVORITES'          => ['POST', '/device/employee/favorites'],
    'CHAT_WITH_AGENT'         => ['GET', '/device/employee/chat'],
];

$closedBefore = 0; $openedAfter = 0; $stayedShut = []; $notClosed = [];

foreach ($probe as $key => [$method, $path]) {
    /* مغلقٌ قبل المنح — و403 بعينها: «لا صلاحية». */
    $r1 = $call($method, $path, $token, $method === 'POST' ? [] : null);
    if ($r1['status'] === 403) { $closedBefore++; }
    else { $notClosed[] = $key . ':' . $r1['status']; }

    DB::table('employee_permissions')->insert([
        'employee_id' => $employeeId, 'permission_key' => $key,
        'granted_by' => $agentId, 'granted_at' => now(),
    ]);

    /*
     * ⚠ **«فُتح» تعني 200 لا «ليس 403»**.
     *
     * جلسةٌ ساقطة تردّ 401 على كلّ شيء، و«ليس 403» كانت تعدّ ذلك فتحاً —
     * فيمرّ الفحصُ كاملاً وكلُّ بابٍ مغلق. وهو أسوأ من فحصٍ يفشل.
     */
    $r2 = $call($method, $path, $token, $method === 'POST' ? [] : null);

    /*
     * ⚠ **«فُتح» = ليس 401 ولا 403** — لا «200» ولا «ليس 403».
     *
     * «ليس 403» كانت تعدّ 401 فتحاً، فتمرّ جلسةٌ ساقطة على أنها اثنتا
     * عشرة صلاحيةً مفتوحة. و«200» تُسقط 422 — وهي **إثباتٌ للفتح**: من
     * وصل إلى التحقّق من المدخلات فقد عبر الحارس. (`SEARCH_TRANSFER`
     * ترفض رقمَ حوالةٍ صوريّاً بـ422، وذلك صحيح.)
     */
    if ($r2['status'] !== 403 && $r2['status'] !== 401) { $openedAfter++; }
    else { $stayedShut[] = $key . ':' . $r2['status']; }

    DB::table('employee_permissions')->where('employee_id', $employeeId)
        ->where('permission_key', $key)->delete();
}

$check('7. ⚠ كلُّها مغلقةٌ قبل المنح',
    $closedBefore === count($probe),
    $notClosed === [] ? $closedBefore . ' من ' . count($probe)
                      : 'لم تُغلق: ' . implode(' · ', $notClosed));

$check('8. ⚠ وكلُّها تُفتح بمنحها وحدَها',
    $openedAfter === count($probe),
    $stayedShut === [] ? count($probe) . ' من ' . count($probe)
                       : 'بقيت مغلقة: ' . implode(' · ', $stayedShut));

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٤) والحذفُ يُعيده إلى الصفر ───────────────────────────────');

$check('9. لا صفَّ صلاحيةٍ بقي بعد الفحص',
    DB::table('employee_permissions')->where('employee_id', $employeeId)->count() === 0);

/* ⚠ فحصُ الدردشة يُنشئ خيطاً يشير إلى الموظف — يُحذف قبله. */
$threads = DB::table('chat_threads')->where('employee_id', $employeeId)->pluck('id');
if ($threads->isNotEmpty()) {
    DB::table('chat_reads')->whereIn('thread_id', $threads)->delete();
    DB::table('chat_messages')->whereIn('thread_id', $threads)->delete();
    DB::table('chat_threads')->whereIn('id', $threads)->delete();
}
DB::table('employee_sessions')->where('employee_id', $employeeId)->delete();
DB::table('employee_devices')->where('employee_id', $employeeId)->delete();
DB::table('employee_activation_codes')->where('employee_id', $employeeId)->delete();
DB::table('employee_otps')->where('employee_id', $employeeId)->delete();
DB::table('employees')->where('id', $employeeId)->delete();
DB::table('personal_access_tokens')->where('name', 'default-deny-check')->delete();

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٥) البصمة المالية ────────────────────────────────────────');
$after = [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => DB::table('InternalEx')->count(),
];
foreach ($before as $k => $v) {
    $check('10. ' . $k . ' لم يتغيّر', (string) $after[$k] === (string) $v,
        'قبل=' . $v . ' بعد=' . $after[$k]);
}

$line();
$line('════════════════════════════════════════════════════════════');
$line('  نجح: ' . $ok . '   أخفق: ' . $fail . '   تُخطّي: ' . $skip);
$line('════════════════════════════════════════════════════════════');
