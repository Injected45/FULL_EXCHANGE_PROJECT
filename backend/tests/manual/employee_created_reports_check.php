<?php

use Illuminate\Support\Facades\DB;

/*
 * تقاريرُ «من أنشأ ماذا» — قراءةٌ خالصة من `transfer_attributions`.
 *
 * ⚠ ولا تلمس ديناراً: لا دفترَ يُقرأ ولا قيمةَ تُجمع منه. ولقطةٌ ماليّة
 * قبل وبعد تُثبت ذلك لا تدّعيه.
 */

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, &$failed, $line) {
    if ($p) { $ok++; $line("  PASS  $n" . ($d ? "  ($d)" : '')); }
    else    { $fail++; $failed[] = $n; $line("  FAIL  $n" . ($d ? "  ($d)" : '')); }
};

$call = function (string $p, ?string $tok) {
    $ch = curl_init('http://127.0.0.1:8000/api' . $p);
    curl_setopt_array($ch, [CURLOPT_RETURNTRANSFER => true,
        CURLOPT_HTTPHEADER => ['Accept: application/json',
                               'Authorization: Bearer ' . $tok],
        CURLOPT_TIMEOUT => 60]);
    $o = curl_exec($ch);
    $c = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);
    return ['status' => $c, 'body' => json_decode($o, true)];
};

$line('══════════════════════════════════════════════════════════════');
$line('   تقارير: من أنشأ الحوالة، ومن أي نقطة بيع');
$line('══════════════════════════════════════════════════════════════');

$snap = fn () => [
    'wallet'   => (string) DB::selectOne('SELECT COUNT(*) v FROM wallet')->v,
    'internal' => (string) DB::selectOne('SELECT COUNT(*) v FROM InternalEx')->v,
    'safe'     => (string) DB::selectOne('SELECT COUNT(*) v FROM EX24AccSafeActivityTb')->v,
];
$before = $snap();

$emp = DB::table('employees')->where('status', 'ACTIVE')->orderByDesc('id')->first();
$agent = App\Models\User::find($emp->agent_id);
$tok = $agent->createToken('report-check')->plainTextToken;

$line();
$line("الوكيل: AccID={$agent->AccID}   ·   الموظّف: {$emp->full_name}");

$line();
$line('── ١) ملخّص من أنشأ كم ───────────────────────────────────────');

$r = $call('/employees/reports/created-transfers?period=month', $tok);
$check('التقرير 200', $r['status'] === 200, 'status=' . $r['status']);

$d = $r['body']['data'] ?? [];
$check('ويحمل الشكل المتوقّع',
    array_key_exists('items', $d) && array_key_exists('transfers', $d),
    'مفاتيح=' . implode(',', array_keys($d)));

$mine = collect($d['items'] ?? [])->firstWhere('employee_id', (int) $emp->id);
$check('ويظهر فيه الموظّف الذي أنشأ حوالات',
    $mine !== null,
    $mine ? ($mine['employee_name'] . ' — ' . $mine['transfers'] . ' حوالة') : 'غير موجود');

/* العددُ يطابق جدول النسب لا رقماً محسوباً في مكانٍ ثانٍ. */
$expected = DB::table('transfer_attributions')
    ->where('agent_id', $agent->id)->where('action', 'CREATED')
    ->where('employee_id', $emp->id)
    ->whereBetween('occurred_at', [now()->startOfMonth(), now()->endOfDay()])
    ->count();
$check('والعددُ يطابق جدول النسب',
    (int) ($mine['transfers'] ?? -1) === $expected,
    'التقرير=' . ($mine['transfers'] ?? '—') . ' · القاعدة=' . $expected);

$line();
$line('── ٢) حوالاتُ موظّفٍ بعينه ───────────────────────────────────');

$r = $call('/employees/' . $emp->id . '/transfers', $tok);
$check('القائمة 200', $r['status'] === 200, 'status=' . $r['status']);
$d2 = $r['body']['data'] ?? [];
$check('وتحمل اسمه وعناصرها',
    isset($d2['employee_name']) && array_key_exists('items', $d2),
    ($d2['employee_name'] ?? '—') . ' · عناصر=' . count($d2['items'] ?? []));

/* ⚠ العزل: موظّفُ وكيلٍ آخر لا يُقرأ من هنا. */
$other = DB::table('employees')->where('agent_id', '<>', $agent->id)
    ->whereNull('deleted_at')->first(['id']);
if ($other) {
    $r = $call('/employees/' . $other->id . '/transfers', $tok);
    $check('⚠ ولا تُقرأ حوالاتُ موظّفِ وكيلٍ آخر', $r['status'] === 404,
        'status=' . $r['status']);
} else {
    $check('⚠ ولا تُقرأ حوالاتُ موظّفِ وكيلٍ آخر (لا موظّفَ آخر للمقارنة)', true);
}

$line();
$line('── ٣) الحارس ────────────────────────────────────────────────');

$r = $call('/employees/reports/created-transfers', null);
$check('بلا رمزٍ ⇐ 401', $r['status'] === 401, 'status=' . $r['status']);

$line();
$line('── تنظيف ────────────────────────────────────────────────────');
DB::table('personal_access_tokens')->where('name', 'report-check')->delete();
$line('  حُذف رمز الفحص.');

$line();
$line('── اللقطة المالية ────────────────────────────────────────────');
$after = $snap();
$same = true;
foreach ($before as $k => $v) {
    $eq = ($v === $after[$k]);
    if (!$eq) $same = false;
    printf("  %-10s %-10s ⇦ %-10s %s\n", $k, $v, $after[$k], $eq ? '✓' : '✗');
}
$check('⚠ لا شيء ماليّ تغيّر — والتقارير قراءةٌ خالصة', $same);

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) $line('   المُخفِق: ' . implode(' · ', $failed));
$line('══════════════════════════════════════════════════════════════');
