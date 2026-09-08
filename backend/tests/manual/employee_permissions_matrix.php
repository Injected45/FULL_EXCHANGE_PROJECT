<?php

use App\Services\Employees\EmployeePermissions;
use Illuminate\Support\Facades\DB;

/*
 * مصفوفةُ الصلاحيات — كلُّ صلاحيةٍ على حدة، عبر HTTP الحقيقيّ.
 *
 *   1) php artisan serve
 *   2) php artisan tinker --execute="require base_path('tests/manual/employee_permissions_matrix.php');"
 *
 * ══════════════════════════════════════════════════════════════════════════
 *  ما يُثبته، ولماذا لا يكفي غيرُه
 * ══════════════════════════════════════════════════════════════════════════
 *
 * ⚠ لكلّ صلاحيةٍ سؤالان لا واحد:
 *
 *   ١. **تعمل حين تُمنح؟**  — وإلّا منح الوكيلُ مفتاحاً لا يفتح شيئاً، وهو
 *      ما وقع فعلاً: صلاحياتٌ محفوظةٌ في القاعدة بلا مسارٍ ولا شاشة.
 *   ٢. **تُمنع حين تُسحب؟** — وإلّا كان المنعُ وهماً، وكلُّ موظفٍ يملك كلَّ
 *      شيء مهما ضبط الوكيلُ شاشةَ الصلاحيات.
 *
 * والسؤالُ الثاني هو الأخطر، ولا يُجاب إلّا عبر HTTP: استدعاءُ الدالّة
 * مباشرةً يتخطّى الوسيطَ الذي يحرس — فيمرّ اختبارٌ أخضر والبابُ مفتوح.
 *
 * ⚠ ومسارات **الكتابة** تُختبر للمنع وحدَه: تنفيذُها يكتب في الدفتر الماليّ،
 * واختبارٌ يُنشئ حوالةً حقيقية ليقول «الصلاحية تعمل» ثمنُه أغلى من فائدته.
 */

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, &$failed, $line) {
    if ($p) { $ok++; $line("  PASS  $n" . ($d ? "  ($d)" : '')); }
    else    { $fail++; $failed[] = $n; $line("  FAIL  $n" . ($d ? "  ($d)" : '')); }
};

$base = 'http://127.0.0.1:8000/api';
$call = function (string $m, string $p, string $t, array $b = []) use ($base) {
    $ch = curl_init($base . $p);
    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_CUSTOMREQUEST  => $m,
        CURLOPT_HTTPHEADER     => [
            'Accept: application/json',
            'Content-Type: application/json',
            'Authorization: Bearer ' . $t,
        ],
        CURLOPT_TIMEOUT => 30,
    ]);
    if ($b !== []) curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($b, JSON_UNESCAPED_UNICODE));
    $raw = curl_exec($ch);
    $code = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);
    return ['status' => $code, 'body' => json_decode($raw, true)];
};

$line('══════════════════════════════════════════════════════════════');
$line('   مصفوفةُ صلاحيات الموظف');
$line('══════════════════════════════════════════════════════════════');

$ping = $call('GET', '/employees', 'x');
if ($ping['status'] === 0) {
    $line('  ⚠ الخادم لا يستجيب على 127.0.0.1:8000 — شغّل `php artisan serve`.');
    return;
}

$before = [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => (string) DB::table('InternalEx')->count(),
];

$agentId = 104;
$phone   = '911234500';

/* موظفٌ اختباريّ بجلسةٍ وجهاز — ولا يُمسّ موظفٌ يعمل. */
$stale = DB::table('employees')->where('phone', $phone)->pluck('id');
foreach (['employee_sessions', 'employee_devices', 'employee_permissions'] as $t) {
    try { DB::table($t)->whereIn('employee_id', $stale)->delete(); } catch (\Throwable) {}
}
DB::table('employees')->whereIn('id', $stale)->delete();

$empId = DB::table('employees')->insertGetId([
    'agent_id' => $agentId, 'full_name' => 'موظف مصفوفة الصلاحيات',
    'phone' => $phone, 'status' => 'ACTIVE',
    'created_at' => now(), 'updated_at' => now(),
]);

$token = bin2hex(random_bytes(24));
DB::table('employee_devices')->insert([
    'employee_id' => $empId, 'agent_id' => $agentId,
    'device_hash' => 'matrix-test', 'status' => 'ACTIVE', 'activated_at' => now(),
]);
DB::table('employee_sessions')->insert([
    'employee_id' => $empId, 'agent_id' => $agentId, 'device_hash' => 'matrix-test',
    'access_token_hash' => hash('sha256', $token),
    'refresh_token_hash' => hash('sha256', $token . 'r'),
    'status' => 'ACTIVE', 'created_at' => now(), 'last_used_at' => now(),
]);

$grant = function (string $key) use ($empId, $agentId) {
    DB::table('employee_permissions')->insert([
        'employee_id' => $empId,
        'permission_key' => $key, 'granted_by' => $agentId, 'granted_at' => now(),
    ]);
};
$revokeAll = fn () => DB::table('employee_permissions')->where('employee_id', $empId)->delete();

/*
 * كلُّ صلاحيةٍ ومسارُها.
 *
 * `write` يعني أن تنفيذها يكتب — فتُختبر للمنع وحدَه.
 * `ui` يعني بوّابةَ واجهةٍ بلا مسارٍ خاصّ — تُوثَّق ولا تُنادى.
 */
$matrix = [
    ['VIEW_INCOMING_TRANSFERS', 'GET',  '/device/employee/transfers/incoming'],
    ['VIEW_OWN_TRANSFERS',      'GET',  '/device/employee/transfers/mine'],
    ['VIEW_OWN_TRANSFERS',      'GET',  '/device/employee/statement'],
    ['VIEW_POS_TRANSFERS',      'GET',  '/device/employee/transfers/point-of-sale'],
    ['SEARCH_TRANSFER',         'GET',  '/device/employee/transfers/search?code=X'],
    ['VIEW_OWN_CASHBOX',        'GET',  '/device/employee/cashbox'],
    ['VIEW_AGENT_TOTAL_BALANCE','GET',  '/device/employee/balance'],
    ['VIEW_FINANCIAL_SUMMARY',  'GET',  '/device/employee/summary'],
    ['REPORT_DAILY_TRANSFERS',  'GET',  '/device/employee/reports/daily'],
    ['REPORT_DELIVERED_TRANSFERS','GET','/device/employee/reports/delivered'],
    ['REPORT_PENDING_TRANSFERS','GET',  '/device/employee/reports/pending'],
    ['REPORT_EMPLOYEE_CASHBOX', 'GET',  '/device/employee/reports/cashbox'],
    ['REPORT_POINT_OF_SALE',    'GET',  '/device/employee/reports/point-of-sale'],
    ['REPORT_AUDIT',            'GET',  '/device/employee/reports/audit'],
    ['REPORT_AGENT_BALANCE',    'GET',  '/device/employee/reports/agent-balance'],
    ['VIEW_FAVORITES',          'POST', '/device/employee/favorites'],
    ['CREATE_TRANSFER',         'GET',  '/device/employee/approvals'],
];

$writeOnly = [
    ['CREATE_TRANSFER',  'POST', '/device/employee/transfers/create'],
    ['DELIVER_TRANSFER', 'POST', '/device/employee/transfers/1/deliver'],
    ['CASHBOX_ENTRY',    'POST', '/device/employee/cashbox/entry'],
    ['START_SHIFT',      'POST', '/device/employee/shift/start'],
    ['CLOSE_SHIFT',      'POST', '/device/employee/shift/close'],
    ['MANAGE_FAVORITES', 'POST', '/device/employee/favorites/add'],
];

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ١) بلا صلاحيةٍ واحدة: كلُّ الأبواب مغلقة ─────────────────');

$revokeAll();
$blocked = 0; $leaked = [];
foreach (array_merge($matrix, $writeOnly) as [$key, $method, $path]) {
    $r = $call($method, $path, $token, $method === 'POST' ? ['x' => 1] : []);
    if ($r['status'] === 403) { $blocked++; }
    else { $leaked[] = "$key ⇦ HTTP {$r['status']}"; }
}

$check('1. ⚠ كلُّ مسارٍ يُردّ 403 بلا صلاحيته',
    $leaked === [],
    $leaked === [] ? ($blocked . ' مسار') : implode(' · ', array_slice($leaked, 0, 3)));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٢) كلُّ صلاحيةٍ تفتح مسارَها ─────────────────────────────');

foreach ($matrix as [$key, $method, $path]) {
    $revokeAll();
    $grant($key);

    $r = $call($method, $path, $token, $method === 'POST' ? [] : []);

    /*
     * ⚠ المقبولُ 200 أو 422 لا 200 وحدَه.
     *
     * فبعضُ المسارات تشترط حقولاً (البحث يحتاج رقماً صالحاً)، و422 يعني
     * أنّ **الوسيطَ سمح** ثمّ رفض المتحكّمُ المدخلات — وهو ما نقيسه هنا.
     * أمّا 403 فيعني أن الصلاحية لا تفتح بابَها، وهو العطب.
     */
    $passed = in_array($r['status'], [200, 422], true);

    $check("2.$key",
        $passed,
        'HTTP ' . $r['status'] . ($passed ? '' : ' ✗'));
}

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٣) وصلاحيةٌ لا تفتح بابَ غيرها ──────────────────────────');

/*
 * ⚠ الاختبارُ الأهمّ: موظفٌ مُنح **قراءةَ حوالاته** يجب ألّا يبلغ الرصيد،
 * ولا التقارير، ولا المفضّلة. وبدون هذا الفحص قد يكون الوسيطُ يقبل أيَّ
 * صلاحيةٍ ما دام هناك واحدة — وهو عطبٌ يمرّ من الفحصين السابقين معاً.
 */
$revokeAll();
$grant('VIEW_OWN_TRANSFERS');

$crossed = [];
foreach ([
    ['VIEW_AGENT_TOTAL_BALANCE', 'GET', '/device/employee/balance'],
    ['REPORT_AUDIT', 'GET', '/device/employee/reports/audit'],
    ['VIEW_FAVORITES', 'POST', '/device/employee/favorites'],
    ['VIEW_INCOMING_TRANSFERS', 'GET', '/device/employee/transfers/incoming'],
] as [$key, $method, $path]) {
    $r = $call($method, $path, $token);
    if ($r['status'] !== 403) $crossed[] = "$key ⇦ {$r['status']}";
}

$check('3. ⚠ صلاحيةٌ واحدة لا تفتح سواها',
    $crossed === [], $crossed === [] ? 'أربعةُ أبوابٍ مغلقة' : implode(' · ', $crossed));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٤) سحبُ الصلاحية يُغلق فوراً ────────────────────────────');

$revokeAll();
$grant('VIEW_AGENT_TOTAL_BALANCE');
$r1 = $call('GET', '/device/employee/balance', $token);

$revokeAll();
$r2 = $call('GET', '/device/employee/balance', $token);

$check('4. ⚠ ما فُتح بالمنح يُغلق بالسحب في الطلب التالي',
    $r1['status'] === 200 && $r2['status'] === 403,
    "بالمنح={$r1['status']} · بالسحب={$r2['status']}");

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٥) الكتالوجُ كلُّه موصول ────────────────────────────────');

$live = EmployeePermissions::LIVE;
$cat  = array_keys(EmployeePermissions::CATALOG);
$check('5. ⚠ لا مفتاحَ في الكتالوج بلا أثر',
    array_diff($cat, $live) === [],
    count($live) . ' من ' . count($cat));

/*
 * ⚠ و`REPORTS_VIEW` بوّابةُ واجهةٍ بلا مسارٍ خاصّ — وهو مقصود: هي تفتح
 * القسمَ في التطبيق، وكلُّ تقريرٍ داخله يحرسه مفتاحُه. فتُوثَّق هنا حتى لا
 * يُظنّ يوماً أنها نُسيت.
 */
$check('6. و`REPORTS_VIEW` بوّابةُ واجهةٍ لا مسار — بابُها ما بداخلها',
    in_array('REPORTS_VIEW', $live, true));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── تنظيف ────────────────────────────────────────────────────');

foreach (['employee_sessions', 'employee_devices', 'employee_permissions'] as $t) {
    try { DB::table($t)->where('employee_id', $empId)->delete(); } catch (\Throwable) {}
}
DB::table('audit_logs')->where('employee_id', $empId)->delete();
DB::table('employees')->where('id', $empId)->delete();
$line('  حُذف الموظف الاختباريّ.');

$after = [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => (string) DB::table('InternalEx')->count(),
];
$check('⚠ لا شيء ماليّ تغيّر', $before === $after);

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) $line('   المُخفِق: ' . implode(' · ', $failed));
$line('══════════════════════════════════════════════════════════════');
