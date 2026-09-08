<?php

use App\Services\Employees\EmployeeReports;
use Illuminate\Support\Facades\DB;

/*
 * خزينةُ الموظف الموحّدة وكشفُ حوالاته — اختبارُ قبول.
 *
 *   php artisan tinker --execute="require base_path('tests/manual/employee_cashbox_unified_acceptance.php');"
 *
 * ⚠ **ما يُثبته**: أنّ كلَّ ما يمرّ بيد الموظف يثبت في **خزينةٍ واحدة** —
 * ما قبضه من مُرسل، وما دفعه لمستفيد، وما استلمه عند فتح الوردية — وأنّ
 * كشفَ حوالاته يجمع الجهتين في سطرٍ واحد للجرد.
 *
 * ⚠ ولا يمسّ مالاً: يعمل على موظّفٍ اختباريّ، ويكتب في جداول الموظف
 * التشغيليّة وحدَها. ولقطةٌ ماليّة قبل وبعد.
 */

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, &$failed, $line) {
    if ($p) { $ok++; $line("  PASS  $n" . ($d ? "  ($d)" : '')); }
    else    { $fail++; $failed[] = $n; $line("  FAIL  $n" . ($d ? "  ($d)" : '')); }
};

$line('══════════════════════════════════════════════════════════════');
$line('   خزينةُ الموظف الموحّدة وكشفُ حوالاته');
$line('══════════════════════════════════════════════════════════════');

$snap = fn () => [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => (string) DB::table('InternalEx')->count(),
    'safe'     => (string) DB::table('EX24AccSafeActivityTb')->count(),
];
$before = $snap();

$agentId = 104;
$phone   = '911234533';

/* تنظيف. */
$stale = DB::table('employees')->where('phone', $phone)->pluck('id');
foreach (['employee_cashbox_entries', 'employee_shift_closings', 'employee_shifts',
          'employee_cashboxes', 'transfer_attributions'] as $t) {
    try { DB::table($t)->whereIn('employee_id', $stale)->delete(); } catch (\Throwable) {}
}
DB::table('employees')->whereIn('id', $stale)->delete();

$employeeId = DB::table('employees')->insertGetId([
    'agent_id' => $agentId, 'full_name' => 'موظف اختبار الخزينة', 'phone' => $phone,
    'status' => 'ACTIVE', 'created_at' => now(), 'updated_at' => now(),
]);
$employee = DB::table('employees')->where('id', $employeeId)->first();

$cashbox = app(\App\Services\Employees\EmployeeCashboxService::class);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ١) وردية بنقدٍ افتتاحيّ ──────────────────────────────────');

$shift = $cashbox->startShift([
    'employee_id' => $employeeId, 'agent_id' => $agentId,
    'opening_cash' => 200.0,
]);
$check('1. فُتحت وردية برصيدٍ افتتاحيّ', isset($shift['id']),
    'وردية #' . ($shift['id'] ?? '؟'));

$open = $cashbox->openShift($employeeId);
$check('2. والوردية مفتوحة', $open !== null);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٢) حوالةٌ أنشأها ⇦ نقدٌ يدخل ────────────────────────────');

$cashbox->addEntry([
    'agent_id' => $agentId, 'employee_id' => $employeeId,
    'cashbox_id' => $open->cashbox_id, 'shift_id' => $open->id,
    'transaction_type' => 'TRANSFER_CREATED',
    'reference_type' => 'INTERNAL_TRANSFER', 'reference_id' => 'CB-TEST-1',
    'amount' => 600.0, 'direction' => \App\Services\Employees\EmployeeCashboxService::IN,
    'notes' => 'قيمة حوالة أنشأها الموظف', 'created_by' => $employeeId,
]);
DB::table('transfer_attributions')->insert([
    'action' => 'CREATED', 'transfer_number' => 'CB-TEST-1',
    'agent_id' => $agentId, 'employee_id' => $employeeId,
    'amount' => 600, 'recipient_phone' => '910000111', 'occurred_at' => now(),
]);

$check('3. ⚠ قيمةُ الحوالة دخلت الخزينة IN',
    DB::table('employee_cashbox_entries')->where('employee_id', $employeeId)
        ->where('transaction_type', 'TRANSFER_CREATED')
        ->where('direction', 'IN')->where('amount', 600)->exists());

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٣) حوالةٌ سلّمها ⇦ نقدٌ يخرج ────────────────────────────');

$cashbox->addEntry([
    'agent_id' => $agentId, 'employee_id' => $employeeId,
    'cashbox_id' => $open->cashbox_id, 'shift_id' => $open->id,
    'transaction_type' => 'TRANSFER_DELIVERY',
    'reference_type' => 'INTERNAL_TRANSFER', 'reference_id' => 'CB-TEST-2',
    'amount' => 250.0, 'direction' => \App\Services\Employees\EmployeeCashboxService::OUT,
    'created_by' => $employeeId,
]);
DB::table('transfer_attributions')->insert([
    'action' => 'DELIVERED', 'transfer_number' => 'CB-TEST-2',
    'agent_id' => $agentId, 'employee_id' => $employeeId,
    'amount' => 250, 'occurred_at' => now(),
]);

$check('4. ⚠ وقيمةُ التسليم خرجت OUT',
    DB::table('employee_cashbox_entries')->where('employee_id', $employeeId)
        ->where('transaction_type', 'TRANSFER_DELIVERY')
        ->where('direction', 'OUT')->where('amount', 250)->exists());

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٤) صرفٌ يدويّ ⇦ نقدٌ يخرج ───────────────────────────────');

$cashbox->addEntry([
    'agent_id' => $agentId, 'employee_id' => $employeeId,
    'cashbox_id' => $open->cashbox_id, 'shift_id' => $open->id,
    'transaction_type' => 'CASH_OUT',
    'amount' => 50.0, 'direction' => \App\Services\Employees\EmployeeCashboxService::OUT,
    'notes' => 'مصروف', 'created_by' => $employeeId,
]);

$check('5. والصرفُ اليدويّ يثبت كذلك',
    DB::table('employee_cashbox_entries')->where('employee_id', $employeeId)
        ->where('transaction_type', 'CASH_OUT')->exists());

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٥) الخزينةُ واحدة ⇦ الجرد ───────────────────────────────');

$entries = DB::table('employee_cashbox_entries')
    ->where('employee_id', $employeeId)->get();

$in  = (float) $entries->where('direction', 'IN')->sum('amount');
$out = (float) $entries->where('direction', 'OUT')->sum('amount');

$check('6. ⚠ كلُّ الحركات في خزينةٍ واحدة لا خزائن',
    $entries->pluck('cashbox_id')->unique()->count() === 1,
    'خزائن=' . $entries->pluck('cashbox_id')->unique()->count()
        . ' · حركات=' . $entries->count());

$check('7. والداخلُ والخارجُ يُجمعان',
    abs($in - 600.0) < 0.001 && abs($out - 300.0) < 0.001,
    "داخل=$in · خارج=$out");

/*
 * ⚠ **المتوقَّع = الافتتاحيّ + الداخل − الخارج** — وهي معادلةُ الخزينة منذ
 * نشأتها، ويُحسب من الحركات لا يُقرأ مخزَّناً.
 */
$expected = 200.0 + $in - $out;
$check('8. ⚠ والمتوقَّع في الدرج = افتتاحيّ + داخل − خارج',
    abs($expected - 500.0) < 0.001, 'المتوقَّع=' . $expected);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٦) كشفُ حساب الحوالات ───────────────────────────────────');

$st = app(EmployeeReports::class)->statement($employee, 30);

$check('9. الكشفُ يجمع ما أنشأ وما سلّم',
    $st['created_count'] === 1 && $st['delivered_count'] === 1,
    'أنشأ=' . $st['created_count'] . ' · سلّم=' . $st['delivered_count']);

$check('10. ⚠ ومجاميعُ كلِّ جهةٍ على حدة',
    abs($st['created_total'] - 600.0) < 0.001
    && abs($st['delivered_total'] - 250.0) < 0.001,
    'أنشأ=' . $st['created_total'] . ' · سلّم=' . $st['delivered_total']);

$check('11. ⚠ والصافي = ما قبض − ما دفع',
    abs($st['net'] - 350.0) < 0.001, 'الصافي=' . $st['net']);

$check('12. والسطورُ مرتّبةٌ بالأحدث في كشفٍ واحد',
    count($st['items']) === 2);

/* ⚠ وموظفٌ آخر لا يظهر في كشفه. */
$other = DB::table('employees')->insertGetId([
    'agent_id' => $agentId, 'full_name' => 'زميل', 'phone' => '911234522',
    'status' => 'ACTIVE', 'created_at' => now(), 'updated_at' => now(),
]);
DB::table('transfer_attributions')->insert([
    'action' => 'CREATED', 'transfer_number' => 'CB-OTHER-1',
    'agent_id' => $agentId, 'employee_id' => $other,
    'amount' => 9999, 'occurred_at' => now(),
]);

$st2 = app(EmployeeReports::class)->statement($employee, 30);
$check('13. ⚠ وحوالةُ زميلٍ لا تدخل كشفَه — الجردُ عليه هو وحدَه',
    $st2['created_count'] === 1
    && abs($st2['created_total'] - 600.0) < 0.001,
    'أنشأ=' . $st2['created_count']);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── تنظيف ────────────────────────────────────────────────────');

$ids = [$employeeId, $other];
foreach (['employee_cashbox_entries', 'employee_shift_closings', 'employee_shifts',
          'employee_cashboxes', 'transfer_attributions', 'audit_logs'] as $t) {
    try { DB::table($t)->whereIn('employee_id', $ids)->delete(); } catch (\Throwable) {}
}
DB::table('employees')->whereIn('id', $ids)->delete();
$line('  نُظّف.');

$line();
$line('── اللقطة المالية ────────────────────────────────────────────');
$after = $snap();
$same = true;
foreach ($before as $k => $v) {
    $eq = ($v === $after[$k]);
    if (!$eq) $same = false;
    printf("  %-10s %-14s ⇦ %-14s %s\n", $k, $v, $after[$k], $eq ? '✓' : '✗');
}
$check('⚠ لا شيء ماليّ تغيّر — الخزينةُ عهدةٌ لا دفتر', $same);

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) $line('   المُخفِق: ' . implode(' · ', $failed));
$line('══════════════════════════════════════════════════════════════');
