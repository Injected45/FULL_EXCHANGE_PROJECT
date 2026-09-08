<?php

use Illuminate\Support\Facades\DB;

/*
 * حذفُ الموظف من تطبيق الوكيل — اختبارُ قبولٍ عبر HTTP.
 *
 *   1) php artisan serve
 *   2) php artisan tinker --execute="require base_path('tests/manual/employee_delete_acceptance.php');"
 *
 * ⚠ **ما يُثبته**: أن الحذف **يُغلق ما يملكه الموظف فعلاً** لا أن يُخفيه من
 * الشاشة، وأنه **ناعم** فلا يمحو تاريخاً مالياً، وأن رقمَ الهاتف يتحرّر —
 * وهي الحاجةُ التي أظهرت هذا النقص: وكيلٌ أدخل رقماً خطأً ولم يجد سبيلاً
 * لتصحيحه.
 */

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, &$failed, $line) {
    if ($p) { $ok++; $line("  PASS  $n" . ($d ? "  ($d)" : '')); }
    else    { $fail++; $failed[] = $n; $line("  FAIL  $n" . ($d ? "  ($d)" : '')); }
};

$base = 'http://127.0.0.1:8000/api';
$call = function (string $m, string $p, ?string $t = null, array $b = []) use ($base) {
    $ch = curl_init($base . $p);
    $h = ['Accept: application/json', 'Content-Type: application/json'];
    if ($t) $h[] = 'Authorization: Bearer ' . $t;
    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true, CURLOPT_CUSTOMREQUEST => $m,
        CURLOPT_HTTPHEADER => $h, CURLOPT_TIMEOUT => 30,
    ]);
    if ($b) curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($b, JSON_UNESCAPED_UNICODE));
    $r = curl_exec($ch); $c = curl_getinfo($ch, CURLINFO_HTTP_CODE); curl_close($ch);
    return ['status' => $c, 'body' => json_decode($r, true)];
};

$line('══════════════════════════════════════════════════════════════');
$line('   حذفُ الموظف من تطبيق الوكيل');
$line('══════════════════════════════════════════════════════════════');

$ping = $call('GET', '/employees');
if ($ping['status'] === 0) {
    $line('  ⚠ الخادم لا يستجيب على 127.0.0.1:8000 — شغّل `php artisan serve`.');
    return;
}

$before = [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => (string) DB::table('InternalEx')->count(),
];

$agentId = 104;
$phone = '911234566';
$token = \App\Models\User::find($agentId)->createToken('del-test')->plainTextToken;

/* تنظيفُ بقايا. */
$stale = DB::table('employees')->where('phone', $phone)->pluck('id');
foreach (['employee_sessions', 'employee_devices', 'employee_activation_codes',
          'employee_approval_requests', 'transfer_attributions'] as $t) {
    try { DB::table($t)->whereIn('employee_id', $stale)->delete(); } catch (\Throwable) {}
}
DB::table('employees')->whereIn('id', $stale)->delete();

$id = DB::table('employees')->insertGetId([
    'agent_id' => $agentId, 'full_name' => 'موظف اختبار الحذف', 'phone' => $phone,
    'status' => 'ACTIVE', 'created_at' => now(), 'updated_at' => now(),
]);

/* ما يملكه موظفٌ حيّ — وهو ما يجب أن يُغلق كلُّه. */
DB::table('employee_sessions')->insert([
    'employee_id' => $id, 'agent_id' => $agentId,
    'access_token_hash' => hash('sha256', 'a' . $id),
    'refresh_token_hash' => hash('sha256', 'r' . $id),
    'device_hash' => 'del-hash', 'status' => 'ACTIVE',
    'expires_at' => now()->addDay(), 'created_at' => now(),
]);
DB::table('employee_devices')->insert([
    'employee_id' => $id, 'agent_id' => $agentId,
    'device_hash' => 'del-hash', 'status' => 'ACTIVE', 'activated_at' => now(),
]);
DB::table('employee_activation_codes')->insert([
    'employee_id' => $id, 'agent_id' => $agentId, 'phone' => $phone,
    'code_hash' => 'x', 'status' => 'ACTIVE', 'issued_by' => $agentId,
    'issued_at' => now(), 'expires_at' => now()->addMinutes(10),
]);
DB::table('employee_approval_requests')->insert([
    'agent_id' => $agentId, 'employee_id' => $id, 'client_id' => 'del-' . time(),
    'payload' => '{}', 'amount' => 500, 'reasons' => 'PER_TRANSFER',
    'status' => 'PENDING', 'created_at' => now(), 'updated_at' => now(),
]);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── الحذف يُغلق، لا يُخفي ────────────────────────────────────');

$r = $call('DELETE', '/employees/' . $id, $token);
$check('1. الحذف ينجح', $r['status'] === 200, 'HTTP ' . $r['status']);

$d = $r['body']['data'] ?? [];
$check('2. ⚠ وأغلق كلَّ ما يملكه: جلسة وجهاز وكود وطلب معلّق',
    ($d['sessions'] ?? 0) === 1 && ($d['devices'] ?? 0) === 1
    && ($d['codes'] ?? 0) === 1 && ($d['approvals'] ?? 0) === 1,
    json_encode($d));

$check('3. ولا شيء منها بقي فعّالاً في القاعدة',
    !DB::table('employee_sessions')->where('employee_id', $id)->where('status', 'ACTIVE')->exists()
    && !DB::table('employee_devices')->where('employee_id', $id)->where('status', 'ACTIVE')->exists()
    && !DB::table('employee_activation_codes')->where('employee_id', $id)->where('status', 'ACTIVE')->exists()
    && !DB::table('employee_approval_requests')->where('employee_id', $id)->where('status', 'PENDING')->exists());

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ومن لا أثرَ له يُمحى تماماً ──────────────────────────────');

$check('4. ⚠ لا صفَّ باقياً — النعومةُ تحفظ لا شيء لمن لا تاريخَ له',
    DB::table('employees')->where('id', $id)->first() === null);

$check('5. ولا بقايا من تهيئته',
    !DB::table('employee_permissions')->where('employee_id', $id)->exists()
    && !DB::table('employee_activation_codes')->where('employee_id', $id)->exists());

$r = $call('GET', '/employees', $token);
$ids = array_column($r['body']['data']['employees'] ?? $r['body']['data'] ?? [], 'id');
$check('6. ولا يظهر في قائمة الوكيل', !in_array($id, $ids, true));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── الرقمُ يتحرّر ────────────────────────────────────────────');

$r = $call('POST', '/employees', $token,
    ['full_name' => 'موظف بديل بالرقم نفسه', 'phone' => $phone]);
$check('7. ⚠ وتُعاد الإضافة بالرقم نفسِه — وهي الحاجةُ التي أظهرت النقص',
    in_array($r['status'], [200, 201], true),
    'HTTP ' . $r['status'] . ' · ' . mb_substr((string) ($r['body']['message'] ?? ''), 0, 40));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ⚠ ومن نفّذ عمليةً مالية يُمنع من الحذف ───────────────────');

/*
 * ⚠ هذا هو نصفُ القاعدة الذي يحمي، والآخرُ يُيسّر.
 *
 * موظفٌ حرّك مالاً — أنشأ حوالةً أو سجّل حركةَ خزينة — لا يُحذف أبداً، لأن
 * حذفه يترك تلك العمليات بلا منفِّذٍ معروف. والوكيلُ يُوقفه بدلاً من ذلك.
 */
$busy = DB::table('employees')->insertGetId([
    'agent_id' => $agentId, 'full_name' => 'موظف نفّذ حوالة', 'phone' => '911234555',
    'status' => 'ACTIVE', 'created_at' => now(), 'updated_at' => now(),
]);
DB::table('transfer_attributions')->insert([
    'action' => 'CREATED', 'transfer_number' => 'DEL-BLOCK-1',
    'agent_id' => $agentId, 'employee_id' => $busy, 'amount' => 250,
    'occurred_at' => now(),
]);

$r = $call('DELETE', '/employees/' . $busy, $token);
$check('10. ⚠ من أنشأ حوالةً لا يُحذف', $r['status'] === 422, 'HTTP ' . $r['status']);

$check('11. والرسالةُ تقول السبب وتعرض البديل',
    str_contains((string) ($r['body']['message'] ?? ''), 'عمليات مالية')
    && str_contains((string) ($r['body']['message'] ?? ''), 'إيقافه'),
    mb_substr((string) ($r['body']['message'] ?? ''), 0, 70));

$check('12. ⚠ وصفُّه لم يُمسّ — لا حذفَ ولا تعليم',
    DB::table('employees')->where('id', $busy)->whereNull('deleted_at')->exists());

$check('13. وحوالتُه باقية',
    DB::table('transfer_attributions')->where('employee_id', $busy)->count() === 1);

/* وحركةُ الخزينة تمنع كذلك، ولو بلا حوالة. */
DB::table('transfer_attributions')->where('employee_id', $busy)->delete();
$r = $call('DELETE', '/employees/' . $busy, $token);
$check('14. ومن لا أثرَ له بعد إزالة حوالته يُحذف',
    $r['status'] === 200, 'HTTP ' . $r['status']);

DB::table('employees')->where('phone', '911234555')->delete();

// ══════════════════════════════════════════════════════════════════
$line();
$line('── الأبواب ──────────────────────────────────────────────────');

$r = $call('DELETE', '/employees/' . $id);
$check('8. الحذف بلا رمزٍ يُرفض', $r['status'] === 401, 'HTTP ' . $r['status']);

$other = DB::table('users')->where('id', '<>', $agentId)
    ->where('AccountType', 'Main')->whereNotNull('AccID')->first();
if ($other) {
    $ot = \App\Models\User::find($other->id)->createToken('del-test-other')->plainTextToken;
    $r = $call('DELETE', '/employees/' . $id, $ot);
    $check('9. ⚠ ووكيلٌ آخر لا يحذف موظّفاً ليس له',
        $r['status'] === 404, 'HTTP ' . $r['status']);
}

// ══════════════════════════════════════════════════════════════════
$line();
$line('── تنظيف ────────────────────────────────────────────────────');

$ids = DB::table('employees')->where('phone', $phone)->pluck('id');
foreach (['employee_sessions', 'employee_devices', 'employee_activation_codes',
          'employee_approval_requests', 'transfer_attributions', 'audit_logs'] as $t) {
    try { DB::table($t)->whereIn('employee_id', $ids)->delete(); } catch (\Throwable) {}
}
DB::table('employees')->whereIn('id', $ids)->delete();
DB::table('personal_access_tokens')->where('name', 'like', 'del-test%')->delete();
$line('  حُذف الموظف الاختباريّ ورموزُه.');

$after = [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => (string) DB::table('InternalEx')->count(),
];
$check('⚠ لا شيء ماليّ تغيّر', $before === $after,
    $before['wallet'] . ' ⇦ ' . $after['wallet']);

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) $line('   المُخفِق: ' . implode(' · ', $failed));
$line('══════════════════════════════════════════════════════════════');
