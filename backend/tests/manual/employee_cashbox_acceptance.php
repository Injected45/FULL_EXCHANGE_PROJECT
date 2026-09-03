<?php

/*
 * اختبارات قبول خزينة الموظف والورديات — بنود 38 إلى 41 و46 و51.
 *
 *   php artisan tinker --execute="require base_path('tests/manual/employee_cashbox_acceptance.php');"
 *
 * وأهمّها الأخير: هذا الدفتر التشغيلي **لا يمسّ** حسابات الوكيل مع الرحالة.
 * البصمة المالية تُؤخذ قبل كل شيء وتُقارن في النهاية.
 */

use App\Services\Employees\EmployeeCashboxService;
use Illuminate\Support\Facades\DB;

$svc = app(EmployeeCashboxService::class);

$line = fn($s) => print($s . PHP_EOL);
$ok = 0; $fail = 0;
$check = function (string $name, bool $pass, string $detail = '') use (&$ok, &$fail, $line) {
    if ($pass) { $ok++;   $line("  PASS  $name" . ($detail ? "  ($detail)" : '')); }
    else       { $fail++; $line("  FAIL  $name" . ($detail ? "  ($detail)" : '')); }
};

$agentId = 104;
$phone   = '911234599';

$line('=== employee cashbox & shifts — acceptance ===');

$snapshot = fn() => [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'ledger'   => DB::table('ExchangeAccData')->count(),
    'internal' => DB::table('InternalEx')->count(),
    'safe'     => DB::table('EX24AccSafeActivityTb')->count(),
    'accounts' => DB::table('AccountsTb')->count(),
];
$before = $snapshot();

/* تنظيف بقايا سابقة. */
$stale = DB::table('employees')->where('phone', $phone)->pluck('id');
if ($stale->isNotEmpty()) {
    DB::table('employee_shift_closings')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_cashbox_entries')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_shifts')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_cashboxes')->whereIn('employee_id', $stale)->delete();
    DB::table('employees')->whereIn('id', $stale)->delete();
}

$employeeId = DB::table('employees')->insertGetId([
    'agent_id'   => $agentId,
    'full_name'  => 'موظف خزينة اختبار',
    'phone'      => $phone,
    'status'     => 'ACTIVE',
    'created_at' => now(),
    'updated_at' => now(),
]);

/* ---------- 1. الخزينة تُنشأ عند أول حاجة، وواحدة لا أكثر ---------- */
$cb1 = $svc->cashboxFor($agentId, $employeeId, null);
$cb2 = $svc->cashboxFor($agentId, $employeeId, null);
$check('1. خزينة واحدة لكل (موظف، نقطة بيع)', $cb1 === $cb2, "id=$cb1");

/* ---------- 2. بدء الوردية بافتتاحي معلَن ---------- */
$shift = $svc->startShift([
    'agent_id' => $agentId, 'employee_id' => $employeeId, 'opening_cash' => 1000,
]);
$check('2. بدء الوردية', $shift['already_open'] === false && $shift['id'] > 0);

$again = $svc->startShift(['agent_id' => $agentId, 'employee_id' => $employeeId, 'opening_cash' => 500]);
$check('2ب. وردية مفتوحة واحدة لا أكثر',
    $again['already_open'] === true && $again['id'] === $shift['id']);

/* ---------- 3. الحركات والمعادلة ---------- */
$svc->addEntry([
    'agent_id' => $agentId, 'employee_id' => $employeeId, 'cashbox_id' => $cb1,
    'shift_id' => $shift['id'], 'transaction_type' => 'CASH_RECEIVED',
    'amount' => 500, 'direction' => EmployeeCashboxService::IN,
]);
$svc->addEntry([
    'agent_id' => $agentId, 'employee_id' => $employeeId, 'cashbox_id' => $cb1,
    'shift_id' => $shift['id'], 'transaction_type' => 'TRANSFER_DELIVERY',
    'reference_type' => 'INTERNAL_TRANSFER', 'reference_id' => 'TEST-1111',
    'amount' => 300, 'direction' => EmployeeCashboxService::OUT,
]);

$calc = $svc->expectedCash($cb1, $shift['id']);
$check('3. المعادلة: الافتتاحي + الوارد − المسلَّم',
    abs($calc['expected'] - 1200) < 0.001,
    "1000 + 500 − 300 = {$calc['expected']}");

/* ---------- 4. تحايُد الطلب — بند 51 ---------- */
$a = $svc->addEntry([
    'agent_id' => $agentId, 'employee_id' => $employeeId, 'cashbox_id' => $cb1,
    'shift_id' => $shift['id'], 'transaction_type' => 'CASH_RECEIVED',
    'amount' => 200, 'direction' => EmployeeCashboxService::IN,
    'client_ref' => 'REQ-ABC',
]);
$b = $svc->addEntry([
    'agent_id' => $agentId, 'employee_id' => $employeeId, 'cashbox_id' => $cb1,
    'shift_id' => $shift['id'], 'transaction_type' => 'CASH_RECEIVED',
    'amount' => 200, 'direction' => EmployeeCashboxService::IN,
    'client_ref' => 'REQ-ABC',
]);
$check('4. تكرار الطلب لا يُنشئ حركة ثانية',
    $a['id'] === $b['id'] && $b['duplicate'] === true);

$c = $svc->addEntry([
    'agent_id' => $agentId, 'employee_id' => $employeeId, 'cashbox_id' => $cb1,
    'shift_id' => $shift['id'], 'transaction_type' => 'TRANSFER_DELIVERY',
    'reference_type' => 'INTERNAL_TRANSFER', 'reference_id' => 'TEST-1111',
    'amount' => 300, 'direction' => EmployeeCashboxService::OUT,
]);
$check('4ب. تسليم الحوالة نفسها لا يُسجَّل مرّتين', $c['duplicate'] === true);

$calc = $svc->expectedCash($cb1, $shift['id']);
$check('4ج. المتوقّع بعد التكرار = 1,400 لا 1,900',
    abs($calc['expected'] - 1400) < 0.001, "{$calc['expected']}");

/* ---------- 5. التصحيح بعكسٍ لا بحذف — بند 41 ---------- */
$revId = $svc->reverseEntry($a['id'], $agentId, 'خطأ في الإدخال');
$origStill = DB::table('employee_cashbox_entries')->where('id', $a['id'])->first();
$check('5. الأصل يبقى ويُوسم معكوساً',
    $origStill !== null && (int) $origStill->is_reversed === 1);
$check('5ب. أُضيف صفّ عكسي يشير إلى الأصل',
    DB::table('employee_cashbox_entries')->where('id', $revId)
        ->where('reversal_of', $a['id'])->exists());

$calc = $svc->expectedCash($cb1, $shift['id']);
$check('5ج. العكس يُلغي أثر الأصل من المتوقّع',
    abs($calc['expected'] - 1200) < 0.001, "{$calc['expected']}");

/* ---------- 6. الإقفال: مطابق / عجز / زيادة — بند 46 ---------- */
$closed = $svc->closeShift($shift['id'], 1200, $agentId);
$check('6. إقفال مطابق', $closed['result'] === 'MATCH' && $closed['label'] === 'مطابق',
    "متوقّع {$closed['expected']} · فعلي {$closed['actual']}");
$check('6ب. الوردية صارت مقفلة',
    DB::table('employee_shifts')->where('id', $shift['id'])->value('status') === 'CLOSED');

$s2 = $svc->startShift(['agent_id' => $agentId, 'employee_id' => $employeeId, 'opening_cash' => 100]);
$svc->addEntry([
    'agent_id' => $agentId, 'employee_id' => $employeeId, 'cashbox_id' => $cb1,
    'shift_id' => $s2['id'], 'transaction_type' => 'CASH_RECEIVED',
    'amount' => 50, 'direction' => EmployeeCashboxService::IN,
]);
$c2 = $svc->closeShift($s2['id'], 130, $agentId);
$check('6ج. عجز يُحسب ويُسمّى',
    $c2['result'] === 'SHORTAGE' && abs($c2['difference'] + 20) < 0.001,
    "الفرق {$c2['difference']} · {$c2['label']}");

$s3 = $svc->startShift(['agent_id' => $agentId, 'employee_id' => $employeeId, 'opening_cash' => 0]);
$c3 = $svc->closeShift($s3['id'], 25, $agentId);
$check('6د. زيادة تُحسب وتُسمّى',
    $c3['result'] === 'SURPLUS' && abs($c3['difference'] - 25) < 0.001,
    "الفرق {$c3['difference']} · {$c3['label']}");

$check('6هـ. الفرق يُسجَّل أمنياً لينتبه الوكيل',
    DB::table('security_logs')->where('event_type', 'CASHBOX_DIFFERENCE')
        ->where('employee_id', $employeeId)->count() >= 2);

/* ---------- 7. لا إقفال مرّتين ---------- */
$twice = false;
try { $svc->closeShift($s3['id'], 25, $agentId); }
catch (\InvalidArgumentException $e) { $twice = true; }
$check('7. الوردية لا تُقفل مرّتين', $twice);

/* ---------- 8. الإقفالات محفوظة تاريخياً ---------- */
$check('8. نتائج الإقفال محفوظة',
    DB::table('employee_shift_closings')->where('employee_id', $employeeId)->count() === 3);

/* ---------- تنظيف ---------- */
DB::table('employee_shift_closings')->where('employee_id', $employeeId)->delete();
DB::table('employee_cashbox_entries')->where('employee_id', $employeeId)->delete();
DB::table('employee_shifts')->where('employee_id', $employeeId)->delete();
DB::table('employee_cashboxes')->where('employee_id', $employeeId)->delete();
DB::table('audit_logs')->where('employee_id', $employeeId)->delete();
DB::table('security_logs')->where('employee_id', $employeeId)->delete();
DB::table('employees')->where('id', $employeeId)->delete();

/* ---------- الشرط الحاسم: لا تعارض مع حسابات الوكيل والرحالة ---------- */
$after = $snapshot();
$check('★ لا مساس بالمالية القائمة — wallet · ExchangeAccData · InternalEx · الخزائن · شجرة الحسابات',
    $before == $after, json_encode(['before' => $before, 'after' => $after]));

$line('');
$line("=== PASS: $ok   FAIL: $fail ===");
