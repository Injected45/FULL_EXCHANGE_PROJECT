<?php

use Illuminate\Support\Facades\DB;

/*
 * سقفُ الموظف — اختباراتُ القبول **عبر HTTP الحقيقيّ**.
 *
 *   1) شغّل الخادم:  php artisan serve
 *   2) ثمّ:
 *      php artisan tinker --execute="require base_path('tests/manual/employee_limits_http_acceptance.php');"
 *
 * ⚠ **لماذا HTTP لا استدعاءٌ مباشر للخدمات؟**
 *
 * الاختبارُ الآخر (`employee_limits_acceptance.php`) يفحص المنطق. وهذا يفحص
 * **الأبواب**: من يصل إلى ماذا. والوسائطُ والصلاحياتُ وعزلُ الوكلاء لا
 * تُختبر باستدعاء دالّة — استدعاءُ الدالّة يتخطّاها كلَّها، فيمرّ اختبارٌ
 * أخضر بينما الباب مفتوح.
 *
 * ⚠ ولا يمسّ مالاً: لا ينشئ حوالة، ويقف عند حدود التفويض. ولقطةٌ ماليّة
 * قبل وبعد.
 */

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, &$failed, $line) {
    if ($p) { $ok++; $line("  PASS  $n" . ($d ? "  ($d)" : '')); }
    else    { $fail++; $failed[] = $n; $line("  FAIL  $n" . ($d ? "  ($d)" : '')); }
};

$base = 'http://127.0.0.1:8000/api';

$call = function (string $method, string $path, ?string $token = null, array $body = []) use ($base) {
    $ch = curl_init($base . $path);
    $headers = ['Accept: application/json', 'Content-Type: application/json'];
    if ($token) $headers[] = 'Authorization: Bearer ' . $token;

    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_CUSTOMREQUEST  => $method,
        CURLOPT_HTTPHEADER     => $headers,
        CURLOPT_TIMEOUT        => 30,
    ]);
    if ($body !== []) {
        curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($body, JSON_UNESCAPED_UNICODE));
    }
    $raw = curl_exec($ch);
    $code = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    return ['status' => $code, 'body' => json_decode($raw, true)];
};

$line('══════════════════════════════════════════════════════════════');
$line('   سقفُ الموظف — الأبوابُ عبر HTTP');
$line('══════════════════════════════════════════════════════════════');

/* الخادمُ يعمل؟ */
$ping = $call('GET', '/employees/approvals');
if ($ping['status'] === 0) {
    $line('  ⚠ الخادم لا يستجيب على 127.0.0.1:8000 — شغّل `php artisan serve` أولاً.');
    return;
}

$snap = fn () => [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => (string) DB::table('InternalEx')->count(),
    'safe'     => (string) DB::table('EX24AccSafeActivityTb')->count(),
];
$before = $snap();

$agentId = 104;
$phone   = '911234577';

/* تنظيف. */
$stale = DB::table('employees')->where('phone', $phone)->pluck('id');
if ($stale->isNotEmpty()) {
    DB::table('employee_approval_requests')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_transfer_policies')->whereIn('employee_id', $stale)->delete();
    DB::table('transfer_attributions')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_permissions')->whereIn('employee_id', $stale)->delete();
    DB::table('employees')->whereIn('id', $stale)->delete();
}

$employeeId = DB::table('employees')->insertGetId([
    'agent_id' => $agentId, 'full_name' => 'موظف اختبار الأبواب', 'phone' => $phone,
    'status' => 'ACTIVE', 'created_at' => now(), 'updated_at' => now(),
]);

/* رمزُ الوكيل. */
$agent = DB::table('users')->where('id', $agentId)->first();
$agentToken = $agent
    ? \App\Models\User::find($agentId)->createToken('limits-test')->plainTextToken
    : null;

/* وكيلٌ آخر — لاختبار العزل. */
$other = DB::table('users')
    ->where('id', '<>', $agentId)
    ->where('AccountType', 'Main')
    ->whereNotNull('AccID')
    ->first();
$otherToken = $other
    ? \App\Models\User::find($other->id)->createToken('limits-test-other')->plainTextToken
    : null;

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ١) بلا رمز، لا شيء ───────────────────────────────────────');

$r = $call('GET', '/employees/approvals');
$check('1. القائمة تحتاج مصادقة', $r['status'] === 401, 'HTTP ' . $r['status']);

$r = $call('POST', '/employees/approvals/1/approve');
$check('2. والموافقة كذلك', $r['status'] === 401, 'HTTP ' . $r['status']);

$r = $call('PUT', '/employees/' . $employeeId . '/limits', null, ['per_transfer_limit' => 1]);
$check('3. وتعديلُ السقف كذلك', $r['status'] === 401, 'HTTP ' . $r['status']);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٢) الوكيلُ يضبط سقفَ موظفه ───────────────────────────────');

$r = $call('PUT', '/employees/' . $employeeId . '/limits', $agentToken, [
    'per_transfer_limit' => 1500,
    'recipient_minutes'  => 60,
    'approval_ttl_hours' => 24,
]);
$check('4. الحفظُ ينجح', $r['status'] === 200, 'HTTP ' . $r['status']);

$check('5. وحُفظ في القاعدة',
    (float) DB::table('employee_transfer_policies')
        ->where('employee_id', $employeeId)->value('per_transfer_limit') === 1500.0);

$r = $call('GET', '/employees/' . $employeeId . '/limits', $agentToken);
$check('6. ويُقرأ كما حُفظ',
    (float) ($r['body']['data']['per_transfer_limit'] ?? 0) === 1500.0);

/* ⚠ سقفٌ تراكميّ بلا نافذة يُرفض — لا يُخترع له جواب. */
$r = $call('PUT', '/employees/' . $employeeId . '/limits', $agentToken, [
    'per_transfer_limit' => 1500,
    'cumulative_limit'   => 9000,
]);
$check('7. ⚠ وسقفٌ تراكميّ بلا فترةٍ يُرفض',
    $r['status'] === 422, 'HTTP ' . $r['status']);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٣) عزلُ الوكلاء ──────────────────────────────────────────');

if ($otherToken) {
    $r = $call('GET', '/employees/' . $employeeId . '/limits', $otherToken);
    $check('8. ⚠ وكيلٌ آخر لا يرى سقفَ موظّفٍ ليس له',
        $r['status'] === 404, 'HTTP ' . $r['status']);

    $r = $call('PUT', '/employees/' . $employeeId . '/limits', $otherToken,
        ['per_transfer_limit' => 999999]);
    $check('9. ⚠ ولا يعدّله',
        $r['status'] === 404 || $r['status'] === 403, 'HTTP ' . $r['status']);

    $check('10. ⚠ والسقفُ لم يتغيّر رغم المحاولة',
        (float) DB::table('employee_transfer_policies')
            ->where('employee_id', $employeeId)->value('per_transfer_limit') === 1500.0);
} else {
    $line('  (لا وكيل رئيسيّ آخر في القاعدة — تُخطّى فحوص العزل)');
}

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٤) طلبٌ معلّق: من يراه ومن يبتّ فيه ──────────────────────');

$reqId = DB::table('employee_approval_requests')->insertGetId([
    'agent_id' => $agentId, 'employee_id' => $employeeId,
    'client_id' => 'http-test-' . time(),
    'payload' => json_encode(['amount' => 5000]),
    'amount' => 5000, 'reasons' => 'PER_TRANSFER',
    'recipient_name' => 'مستفيد اختبار', 'recipient_phone' => '910000077',
    'employee_name_snap' => 'موظف اختبار الأبواب',
    'policy_snapshot' => json_encode(['per_transfer_limit' => 1500]),
    'status' => 'PENDING', 'expires_at' => now()->addDay(),
    'created_at' => now(), 'updated_at' => now(),
]);

$r = $call('GET', '/employees/approvals?status=PENDING', $agentToken);
$ids = array_column($r['body']['data']['requests'] ?? [], 'id');
$check('11. الوكيلُ صاحبُ الطلب يراه', in_array($reqId, $ids, true));

$r = $call('GET', '/employees/approvals/count', $agentToken);
$check('12. والشارةُ تعدّه',
    (int) ($r['body']['data']['pending'] ?? 0) >= 1,
    'المعلَّق=' . ($r['body']['data']['pending'] ?? '؟'));

if ($otherToken) {
    $r = $call('GET', '/employees/approvals?status=PENDING', $otherToken);
    $ids2 = array_column($r['body']['data']['requests'] ?? [], 'id');
    $check('13. ⚠ ووكيلٌ آخر لا يراه في قائمته', !in_array($reqId, $ids2, true));

    $r = $call('POST', '/employees/approvals/' . $reqId . '/approve', $otherToken);
    $check('14. ⚠ ولا يوافق عليه', $r['status'] === 404, 'HTTP ' . $r['status']);

    $check('15. ⚠ والطلبُ ما زال معلّقاً بعد محاولته',
        DB::table('employee_approval_requests')->where('id', $reqId)->value('status') === 'PENDING');
}

/*
 * ⚠ **الموافقةُ إذنٌ لا تنفيذ** — أمرُ المالك (8 سبتمبر 2026).
 *
 * فالمالُ بيد الموظف والزبونُ واقفٌ عنده؛ وحوالةٌ تُنفَّذ لحظةَ موافقة
 * الوكيل تدخل خزينةَ الموظف في عجزٍ عن مبلغٍ لم يستلمه بعد.
 */
$permId = DB::table('employee_approval_requests')->insertGetId([
    'agent_id' => $agentId, 'employee_id' => $employeeId,
    'client_id' => 'http-perm-' . time(),
    'payload' => json_encode(['amount' => 5000]),
    'amount' => 5000, 'reasons' => 'PER_TRANSFER',
    'status' => 'PENDING', 'expires_at' => now()->addDay(),
    'created_at' => now(), 'updated_at' => now(),
]);

$internalBefore = DB::table('InternalEx')->count();
$r = $call('POST', '/employees/approvals/' . $permId . '/approve', $agentToken);

$check('16أ. الموافقة تنجح', $r['status'] === 200, 'HTTP ' . $r['status']);

$check('16ب. ⚠ ولا تُنفِّذ الحوالة — الإذنُ للموظف لا فعلٌ عن الوكيل',
    ($r['body']['data']['executed'] ?? true) === false
    && DB::table('InternalEx')->count() === $internalBefore);

$check('16ج. والحالةُ APPROVED بلا رقم حوالة — بانتظار تنفيذ الموظف',
    DB::table('employee_approval_requests')->where('id',$permId)->value('status') === 'APPROVED'
    && DB::table('employee_approval_requests')->where('id',$permId)->value('transfer_number') === null);

$check('16د. ⚠ والرسالةُ تقول للوكيل إنّ الموظف ينفّذها',
    str_contains((string)($r['body']['message'] ?? ''), 'ينفّذ الموظف'),
    mb_substr((string)($r['body']['message'] ?? ''), 0, 50));

DB::table('employee_approval_requests')->where('id',$permId)->delete();

$r = $call('POST', '/employees/approvals/' . $reqId . '/reject', $agentToken);
$check('16. وصاحبُه يرفضه', $r['status'] === 200,
    mb_substr((string) ($r['body']['message'] ?? ''), 0, 40));

$check('17. ⚠ والرفضُ لم يُنشئ حوالة',
    DB::table('employee_approval_requests')->where('id', $reqId)->value('status') === 'REJECTED'
    && DB::table('employee_approval_requests')->where('id', $reqId)->value('transfer_number') === null);

$r = $call('POST', '/employees/approvals/' . $reqId . '/approve', $agentToken);
$check('18. ⚠ ولا يُوافَق عليه بعد رفضه — لا قرارَ متضارب',
    ($r['body']['data']['already'] ?? false) === true
    && DB::table('employee_approval_requests')->where('id', $reqId)->value('status') === 'REJECTED');

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٥) الموظف لا يبلغ بابَ القرار ────────────────────────────');

/*
 * ⚠ جلسةُ الموظف رمزٌ من `employee_sessions` لا رمزَ Sanctum. وهي هنا
 * تُجرَّب على مسار الوكيل: يجب أن تُردّ 401 — فالبابان مختلفان.
 */
$fakeEmployeeToken = bin2hex(random_bytes(32));
$r = $call('POST', '/employees/approvals/' . $reqId . '/approve', $fakeEmployeeToken);
$check('19. ⚠ رمزٌ ليس رمزَ وكيل لا يفتح باب الموافقة',
    $r['status'] === 401, 'HTTP ' . $r['status']);

$r = $call('GET', '/device/employee/approvals', $fakeEmployeeToken);
$check('20. ورمزٌ باطل لا يفتح باب الموظف كذلك',
    in_array($r['status'], [401, 403], true), 'HTTP ' . $r['status']);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── تنظيف ────────────────────────────────────────────────────');

DB::table('employee_approval_requests')->where('employee_id', $employeeId)->delete();
DB::table('employee_transfer_policies')->where('employee_id', $employeeId)->delete();
DB::table('employees')->where('id', $employeeId)->delete();
DB::table('personal_access_tokens')->where('name', 'like', 'limits-test%')->delete();
$line('  حُذف الموظف الاختباريّ ورموزُ الاختبار.');

$line();
$line('── اللقطة المالية ────────────────────────────────────────────');
$after = $snap();
$same = true;
foreach ($before as $k => $v) {
    $eq = ($v === $after[$k]);
    if (!$eq) $same = false;
    printf("  %-10s %-14s ⇦ %-14s %s\n", $k, $v, $after[$k], $eq ? '✓' : '✗');
}
$check('⚠ لا شيء ماليّ تغيّر', $same);

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) $line('   المُخفِق: ' . implode(' · ', $failed));
$line('══════════════════════════════════════════════════════════════');
