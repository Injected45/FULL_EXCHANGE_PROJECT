<?php

use App\Services\Employees\EmployeePermissions;
use Illuminate\Support\Facades\DB;

/*
 * فحصُ المفاتيح الثلاثة التي وُصلت: البحث · حوالاتي · حوالات نقطة البيع.
 *
 * ⚠ يمرّ عبر HTTP بجلسةِ موظّفٍ حقيقية لا بنداء الخدمة مباشرةً: الحارسُ
 * في الوسيط، وفحصٌ يتخطّاه يُثبت أن الشيفرة تعمل ولا يُثبت أنها محروسة.
 *
 * ⚠ ولا يكتب شيئاً في أي دفتر: قراءةٌ خالصة، ولقطةٌ ماليّة قبل وبعد.
 */

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, &$failed, $line) {
    if ($p) { $ok++; $line("  PASS  $n" . ($d ? "  ($d)" : '')); }
    else    { $fail++; $failed[] = $n; $line("  FAIL  $n" . ($d ? "  ($d)" : '')); }
};

$call = function (string $m, string $p, ?string $tok = null, array $b = []) {
    $ch = curl_init('http://127.0.0.1:8000/api' . $p);
    $h = ['Accept: application/json', 'Content-Type: application/json'];
    if ($tok) $h[] = 'Authorization: Bearer ' . $tok;
    curl_setopt_array($ch, [CURLOPT_RETURNTRANSFER => true, CURLOPT_CUSTOMREQUEST => $m,
                            CURLOPT_HTTPHEADER => $h, CURLOPT_TIMEOUT => 40]);
    if ($b !== []) curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($b, JSON_UNESCAPED_UNICODE));
    $o = curl_exec($ch);
    $c = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);
    return ['status' => $c, 'body' => json_decode($o, true)];
};

$line('══════════════════════════════════════════════════════════════');
$line('   صلاحيات الموظف — ما يراه من الحوالات');
$line('══════════════════════════════════════════════════════════════');

$snap = fn () => [
    'wallet'   => (string) DB::selectOne('SELECT COUNT(*) v FROM wallet')->v,
    'internal' => (string) DB::selectOne('SELECT COUNT(*) v FROM InternalEx')->v,
    'safe'     => (string) DB::selectOne('SELECT COUNT(*) v FROM EX24AccSafeActivityTb')->v,
    'attrib'   => (string) DB::selectOne('SELECT COUNT(*) v FROM transfer_attributions')->v,
];
$before = $snap();

/* موظّفٌ فعّالٌ بجلسةٍ قائمة. */
$emp = DB::table('employees')->where('status', 'ACTIVE')->orderByDesc('id')->first();
if (!$emp) { $line('  لا موظّف مفعّل — الفحص يحتاج واحداً.'); return; }

$sessRow = DB::table('employee_sessions')->where('employee_id', $emp->id)
    ->where('status', 'ACTIVE')->orderByDesc('id')->first();
if (!$sessRow) { $line('  لا جلسة فعّالة لهذا الموظّف.'); return; }

$line();
$line("الموظّف: {$emp->full_name}  ·  الوكيل: {$emp->agent_id}");

/*
 * ⚠ الرمزُ مُجزَّأ في القاعدة ولا يُقرأ. فتُنشأ جلسةُ فحصٍ برمزٍ نعرفه،
 * وتُحذف في النهاية — ولا تُمسّ جلسةُ الموظف الحقيقية.
 */
$raw = bin2hex(random_bytes(32));
$testSessionId = DB::table('employee_sessions')->insertGetId([
    'agent_id'          => $emp->agent_id,
    'employee_id'       => $emp->id,
    'device_id'         => $sessRow->device_id,
    'device_hash'       => $sessRow->device_hash,
    'active_pos_id'     => $sessRow->active_pos_id,
    'access_token_hash' => hash('sha256', $raw),
    'status'            => 'ACTIVE',
    'created_at'        => now(),
    'expires_at'        => now()->addHour(),
]);

$permsBefore = DB::table('employee_permissions')->where('employee_id', $emp->id)
    ->pluck('permission_key')->all();

$grant = function (string $key) use ($emp) {
    if (!DB::table('employee_permissions')->where('employee_id', $emp->id)
            ->where('permission_key', $key)->exists()) {
        DB::table('employee_permissions')->insert([
            'employee_id' => $emp->id, 'permission_key' => $key, 'granted_at' => now(),
        ]);
    }
};
$revoke = fn (string $key) => DB::table('employee_permissions')
    ->where('employee_id', $emp->id)->where('permission_key', $key)->delete();

$line();
$line('── ١) الحارس: بلا صلاحية لا يُفتح شيء ────────────────────────');

foreach ([
    'SEARCH_TRANSFER'    => '/device/employee/transfers/search?q=131',
    'VIEW_OWN_TRANSFERS' => '/device/employee/transfers/mine',
    'VIEW_POS_TRANSFERS' => '/device/employee/transfers/point-of-sale',
] as $key => $path) {
    $revoke($key);
    $r = $call('GET', $path, $raw);
    $check("بلا $key ⇐ 403", $r['status'] === 403, 'status=' . $r['status']);
}

$line();
$line('── ٢) وبالصلاحية يُفتح ──────────────────────────────────────');

foreach ([
    'SEARCH_TRANSFER'    => '/device/employee/transfers/search?q=131',
    'VIEW_OWN_TRANSFERS' => '/device/employee/transfers/mine',
    'VIEW_POS_TRANSFERS' => '/device/employee/transfers/point-of-sale',
] as $key => $path) {
    $grant($key);
    $r = $call('GET', $path, $raw);
    $check("مع $key ⇐ 200", $r['status'] === 200, 'status=' . $r['status']);
}

$line();
$line('── ٣) وما يُرجعه معقول ──────────────────────────────────────');

$r = $call('GET', '/device/employee/transfers/mine', $raw);
$d = $r['body']['data'] ?? [];
$check('«حوالاتي» تحمل الشكل المتوقّع',
    array_key_exists('items', $d) && array_key_exists('total', $d),
    'مفاتيح=' . implode(',', array_keys($d)));

$mineCount = DB::table('transfer_attributions')
    ->where('agent_id', $emp->agent_id)->where('employee_id', $emp->id)->count();
$check('والعددُ يطابق ما في جدول النسب',
    (int) ($d['total'] ?? -1) === $mineCount,
    'الردّ=' . ($d['total'] ?? '—') . ' · القاعدة=' . $mineCount);

$r = $call('GET', '/device/employee/transfers/point-of-sale', $raw);
$d2 = $r['body']['data'] ?? [];
$check('و«نقطة البيع» لا تُخطئ لموظّفٍ بلا نقطة',
    array_key_exists('items', $d2),
    isset($d2['note']) ? $d2['note'] : ('عناصر=' . count($d2['items'] ?? [])));

$r = $call('GET', '/device/employee/transfers/search?q=ab', $raw);
$check('⚠ وبحثٌ أقصر من ثلاثة محارف يُرفض — لا يُرجع الدفتر كلَّه',
    $r['status'] === 422, 'status=' . $r['status']);

/* ⚠ العزل: لا يرى حوالات وكيلٍ آخر. */
$otherAgent = DB::table('transfer_attributions')
    ->where('agent_id', '<>', $emp->agent_id)->value('agent_id');
if ($otherAgent) {
    $r = $call('GET', '/device/employee/transfers/mine', $raw);
    $codes = array_column($r['body']['data']['items'] ?? [], 'transfer_number');
    $foreign = DB::table('transfer_attributions')->where('agent_id', $otherAgent)
        ->whereIn('transfer_number', $codes ?: ['-'])->count();
    $check('⚠ ولا يظهر فيها ما يخصّ وكيلاً آخر', $foreign === 0);
} else {
    $check('⚠ ولا يظهر فيها ما يخصّ وكيلاً آخر (لا وكيلَ ثانٍ للمقارنة)', true);
}

$line();
$line('── تنظيف ────────────────────────────────────────────────────');

DB::table('employee_sessions')->where('id', $testSessionId)->delete();

/* الصلاحيات تعود كما كانت بالضبط. */
foreach (['SEARCH_TRANSFER', 'VIEW_OWN_TRANSFERS', 'VIEW_POS_TRANSFERS'] as $k) {
    in_array($k, $permsBefore, true) ? $grant($k) : $revoke($k);
}
$permsAfter = DB::table('employee_permissions')->where('employee_id', $emp->id)
    ->pluck('permission_key')->all();
sort($permsBefore); sort($permsAfter);
$check('والصلاحيات عادت كما كانت', $permsBefore === $permsAfter,
    count($permsBefore) . ' ⇐ ' . count($permsAfter));

$line();
$line('── اللقطة المالية ────────────────────────────────────────────');
$after = $snap();
$same = true;
foreach ($before as $k => $v) {
    $eq = ($v === $after[$k]);
    if (!$eq) $same = false;
    printf("  %-10s %-10s ⇦ %-10s %s\n", $k, $v, $after[$k], $eq ? '✓' : '✗');
}
$check('لا شيء ماليّ تغيّر', $same);

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) $line('   المُخفِق: ' . implode(' · ', $failed));
$line('══════════════════════════════════════════════════════════════');
