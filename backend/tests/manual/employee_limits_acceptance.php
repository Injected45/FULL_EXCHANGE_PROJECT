<?php

use App\Services\Employees\EmployeeApprovals;
use App\Services\Employees\EmployeeLimitPolicy;
use Illuminate\Support\Facades\DB;

/*
 * سقفُ تحويل الموظف وموافقةُ الوكيل — اختبارات القبول (البند 54).
 *
 *   php artisan tinker --execute="require base_path('tests/manual/employee_limits_acceptance.php');"
 *
 * ⚠ **أهمُّ ما يُثبته**: أن الطلبَ المعلّق **ليس حوالة** — لا صفَّ له في
 * `InternalEx`، ولا رصيدَ خُصم، ولا قيدَ كُتب. ولقطةٌ ماليّة قبل وبعد تُثبت
 * أن شيئاً لم يتحرّك.
 *
 * ⚠ ولا ينفّذ حوالةً حقيقية: يقف عند حدود طبقة التفويض، فما بعدها هو مسارُ
 * الوكيل نفسُه المختبَرُ في مكانه.
 */

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, &$failed, $line) {
    if ($p) { $ok++; $line("  PASS  $n" . ($d ? "  ($d)" : '')); }
    else    { $fail++; $failed[] = $n; $line("  FAIL  $n" . ($d ? "  ($d)" : '')); }
};

$line('══════════════════════════════════════════════════════════════');
$line('   سقفُ تحويل الموظف وموافقةُ الوكيل');
$line('══════════════════════════════════════════════════════════════');

$snap = fn () => [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => (string) DB::table('InternalEx')->count(),
    'safe'     => (string) DB::table('EX24AccSafeActivityTb')->count(),
    'accounts' => (string) DB::table('AccountsTb')->count(),
];
$before = $snap();

$policy    = app(EmployeeLimitPolicy::class);
$approvals = app(EmployeeApprovals::class);

$agentId = 104;
$otherAgentId = (int) (DB::table('users')->where('id', '<>', $agentId)
    ->whereNotNull('AccID')->value('id') ?? 0);

$phone = '911234588';
$empName = 'موظف اختبار السقف';

/* تنظيفُ بقايا أي تشغيلٍ سابق. */
$stale = DB::table('employees')->where('phone', $phone)->pluck('id');
if ($stale->isNotEmpty()) {
    DB::table('employee_approval_requests')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_transfer_policies')->whereIn('employee_id', $stale)->delete();
    DB::table('transfer_attributions')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_permissions')->whereIn('employee_id', $stale)->delete();
    DB::table('employees')->whereIn('id', $stale)->delete();
}

$employeeId = DB::table('employees')->insertGetId([
    'agent_id' => $agentId, 'full_name' => $empName, 'phone' => $phone,
    'status' => 'ACTIVE', 'created_at' => now(), 'updated_at' => now(),
]);
$employee = DB::table('employees')->where('id', $employeeId)->first();
$session  = (object) ['id' => null, 'active_pos_id' => null, 'device_hash' => 'test-hash'];

$mk = fn (string $cid, float $amt, ?string $ph, ?string $nm, array $rs, $pol, float $con = 0)
    => $approvals->open($employee, $session, $cid,
        ['amount' => $amt, 'reviced_name' => $nm, 'reviced_phone' => $ph],
        $amt, $nm, $ph, $rs, $pol, $con);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ١) لا سقفَ افتراضيّ — لا انقطاعَ لمن يعمل اليوم ──────────');

$p = $policy->forEmployee($employeeId);
$check('1. ⚠ موظفٌ بلا صفّ سياسة = بلا سقف',
    $p->per_transfer_limit === null && $p->cumulative_limit === null && !$p->exists);

$v = $policy->evaluate($employee, 999999.0, '910000001', 'مستفيد أول');
$check('2. ⚠ فمبلغٌ ضخم يمرّ بلا تصعيد — لا Regression على العاملين',
    $v['reasons'] === [], 'أسباب=' . count($v['reasons']));

/*
 * ⚠ ولا مراقبةَ مستفيدٍ كذلك لمن لا سياسةَ له.
 *
 * كشفه اختبارُ إنشاء الحوالة القائم: بلا هذا كان كلُّ موظفٍ يعمل اليوم
 * يُصعَّد عند حوالته الثانية إلى المستفيد نفسِه — بقاعدةٍ لم يطلبها وكيلُه.
 */
$check('3. ⚠ ولا مراقبةَ تكرارٍ لمن لا سياسةَ له',
    $p->recipient_minutes === 0, 'دقائق=' . $p->recipient_minutes);

$v = $policy->evaluate($employee, 10.0, '910000001', 'مستفيد أول');
$check('3أ. ⚠ فحوالةٌ ثانية لنفس المستفيد تمرّ ما لم يضع الوكيل سياسة',
    $v['reasons'] === []);

$check('3ب. ومدّةُ صلاحية الطلب معلومةٌ لا مخترعة',
    $p->approval_ttl_hours === 24, 'صلاحية=' . $p->approval_ttl_hours . 'س');

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٢) سقفُ الحوالة الواحدة ──────────────────────────────────');

DB::table('employee_transfer_policies')->insert([
    'employee_id' => $employeeId, 'agent_id' => $agentId,
    'per_transfer_limit' => 1000, 'recipient_minutes' => 60,
    'approval_ttl_hours' => 24, 'created_at' => now(), 'updated_at' => now(),
]);

$v = $policy->evaluate($employee, 1000.0, '910000002', 'ضمن السقف');
$check('4. مبلغٌ مساوٍ للسقف يمرّ — الحدُّ شامل لا حصريّ', $v['reasons'] === []);

$v = $policy->evaluate($employee, 1000.001, '910000003', 'فوق السقف');
$check('5. ⚠ وأدنى تجاوزٍ يُصعَّد',
    $v['reasons'] === [EmployeeLimitPolicy::R_PER_TRANSFER]);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٣) الطلبُ المعلّق ليس حوالة ──────────────────────────────');

$midInternal = DB::table('InternalEx')->count();
$midWallet   = (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s');

$r1 = $mk('cid-over-1', 5000.0, '910000010', 'أحمد المستفيد',
    [EmployeeLimitPolicy::R_PER_TRANSFER], $policy->forEmployee($employeeId));

$check('6. أُنشئ طلبُ موافقة', ($r1['request'] ?? null) !== null
    && $r1['request']->status === 'PENDING', '#' . ($r1['request']->id ?? '؟'));

$check('7. ⚠ ولا صفَّ في الدفتر المالي',
    DB::table('InternalEx')->count() === $midInternal,
    'InternalEx=' . DB::table('InternalEx')->count());

$check('8. ⚠ ولا رصيدَ تحرّك',
    (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s') === $midWallet);

$check('9. ⚠ ولا سطرَ نسبةٍ كُتب — النسبةُ بعد النجاح لا قبله',
    !DB::table('transfer_attributions')->where('employee_id', $employeeId)->exists());

$check('10. وله موعدُ انتهاء لا يبقى للأبد',
    ($r1['request']->expires_at ?? null) !== null);

$check('11. ولقطةُ السياسة محفوظةٌ فيه',
    str_contains((string) $r1['request']->policy_snapshot, '1000'));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٤) منعُ ازدواج الطلب ─────────────────────────────────────');

$again = $mk('cid-over-1', 5000.0, '910000010', 'أحمد المستفيد',
    [EmployeeLimitPolicy::R_PER_TRANSFER], $policy->forEmployee($employeeId));

$check('12. ⚠ المفتاحُ نفسُه لا يُنشئ طلباً ثانياً',
    ($again['duplicate'] ?? false) === true
    && (int) $again['request']->id === (int) $r1['request']->id);

$check('13. والعددُ ما زال واحداً',
    DB::table('employee_approval_requests')->where('employee_id', $employeeId)->count() === 1);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٥) تكرارُ المستفيد ───────────────────────────────────────');

$v = $policy->evaluate($employee, 10.0, '910000010', 'أحمد المستفيد');
$check('14. ⚠ رقمٌ في طلبٍ معلّق يُصعِّد ولو كان المبلغ صغيراً',
    in_array(EmployeeLimitPolicy::R_RECIPIENT, $v['reasons'], true),
    implode('+', $v['reasons']));

$v = $policy->evaluate($employee, 10.0, '910000099', 'أحمد المستفيد');
$check('15. ⚠ ورقمٌ مختلف لا يُصعَّد لتشابه الاسم وحدَه',
    !in_array(EmployeeLimitPolicy::R_RECIPIENT, $v['reasons'], true));

/* والحوالاتُ المنفَّذة تدخل الفحص أيضاً. */
DB::table('transfer_attributions')->insert([
    'action' => 'CREATED', 'transfer_number' => 'TEST-LIMIT-1',
    'agent_id' => $agentId, 'employee_id' => $employeeId,
    'amount' => 300, 'recipient_phone' => '910000020',
    'recipient_name_norm' => EmployeeLimitPolicy::normalizeName('سالم علي'),
    'occurred_at' => now(),
]);

$v = $policy->evaluate($employee, 10.0, '910000020', 'سالم علي');
$check('16. ⚠ ورقمٌ في حوالةٍ منفَّذة يُصعِّد — التقسيمُ لا يمرّ',
    in_array(EmployeeLimitPolicy::R_RECIPIENT, $v['reasons'], true));

/* خارج النافذة لا يُصعَّد. */
DB::table('transfer_attributions')->where('transfer_number', 'TEST-LIMIT-1')
    ->update(['occurred_at' => now()->subMinutes(120)]);
$v = $policy->evaluate($employee, 10.0, '910000020', 'سالم علي');
$check('17. وخارجَ النافذة يمرّ — المراقبةُ مؤقّتة لا دائمة',
    !in_array(EmployeeLimitPolicy::R_RECIPIENT, $v['reasons'], true));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٦) تطبيعُ الاسم ──────────────────────────────────────────');

$n = [EmployeeLimitPolicy::class, 'normalizeName'];
$check('18. الهمزاتُ تُوحَّد', $n('أحمد') === $n('احمد') && $n('إبراهيم') === $n('ابراهيم'));
$check('19. والتاءُ المربوطة والألفُ المقصورة',
    $n('فاطمة') === $n('فاطمه') && $n('يحيى') === $n('يحيي'));
$check('20. والتشكيلُ والمسافاتُ المكرّرة',
    $n('مُحَمَّد  علي') === $n('محمد علي'));
$check('21. ⚠ ولا يُدمج اسمان مختلفان — لا تقاربَ غيرَ منضبط',
    $n('محمد علي') !== $n('محمد سالم')
    && $n('محمد علي') !== $n('محمدعلي'));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٧) السقفُ التراكميّ ──────────────────────────────────────');

DB::table('employee_transfer_policies')->where('employee_id', $employeeId)->update([
    'per_transfer_limit' => 10000, 'cumulative_limit' => 6000, 'cumulative_hours' => 24,
]);
DB::table('transfer_attributions')->where('employee_id', $employeeId)
    ->update(['occurred_at' => now()]);

$consumed = $policy->consumed($employeeId, 24);
$check('22. ⚠ المستهلَك = المنفَّذ + المعلّق',
    abs($consumed - (300 + 5000)) < 0.001, 'المستهلَك=' . $consumed);

$v = $policy->evaluate($employee, 500.0, '910000030', 'مستفيد جديد');
$check('23. وما دام تحت السقف التراكمي يمرّ',
    !in_array(EmployeeLimitPolicy::R_CUMULATIVE, $v['reasons'], true));

$v = $policy->evaluate($employee, 900.0, '910000031', 'مستفيد آخر');
$check('24. ⚠ وتجاوزُه يُصعَّد — التوزيعُ على مستفيدين لا يتجاوز الحدّ',
    in_array(EmployeeLimitPolicy::R_CUMULATIVE, $v['reasons'], true));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٨) اجتماعُ الأسباب ───────────────────────────────────────');

DB::table('employee_transfer_policies')->where('employee_id', $employeeId)
    ->update(['per_transfer_limit' => 100]);

$v = $policy->evaluate($employee, 900.0, '910000010', 'أحمد المستفيد');
$check('25. ⚠ ثلاثةُ أسبابٍ تجتمع في تقييمٍ واحد',
    count($v['reasons']) === 3, implode(' + ', $v['reasons']));

$r2 = $mk('cid-multi', 900.0, '910000010', 'أحمد المستفيد',
    $v['reasons'], $v['policy'], $v['consumed']);

$check('26. ⚠ وتُنشئ طلبَ موافقةٍ **واحداً** لا ثلاثة',
    DB::table('employee_approval_requests')->where('employee_id', $employeeId)->count() === 2);

$check('27. ويحمل الأسبابَ الثلاثة مقروءةً للوكيل',
    count(EmployeeLimitPolicy::reasonLabels($r2['request']->reasons)) === 3,
    implode(' · ', EmployeeLimitPolicy::reasonLabels($r2['request']->reasons)));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٩) القرار: ذرّيٌّ ومعزول ─────────────────────────────────');

$rid = (int) $r1['request']->id;

if ($otherAgentId > 0) {
    $foreign = $approvals->decide($rid, $otherAgentId, $otherAgentId, true);
    $check('28. ⚠ وكيلٌ آخر لا يستطيع البتّ في الطلب',
        $foreign['changed'] === false
        && DB::table('employee_approval_requests')->where('id', $rid)->value('status') === 'PENDING',
        'الوكيل الآخر #' . $otherAgentId);
} else {
    $check('28. وكيلٌ آخر لا يستطيع البتّ', false, 'لا وكيل آخر في القاعدة');
}

$d1 = $approvals->decide($rid, $agentId, $agentId, false, 'رفضٌ اختباريّ');
$check('29. والوكيلُ الصحيح يبتّ', $d1['changed'] === true);

$d2 = $approvals->decide($rid, $agentId, $agentId, true);
$check('30. ⚠ وقرارٌ ثانٍ على النسخة نفسِها لا يمرّ — لا تضارب',
    $d2['changed'] === false
    && DB::table('employee_approval_requests')->where('id', $rid)->value('status') === 'REJECTED');

$check('31. ⚠ والمرفوضُ لم يمسّ الدفتر',
    DB::table('InternalEx')->count() === $midInternal);

$check('32. ⚠ ولا يُحذف سجلُّه — يبقى للتدقيق',
    DB::table('employee_approval_requests')->where('id', $rid)->exists());

$check('33. والمرفوضُ خرج من حساب السقف التراكميّ',
    abs($policy->consumed($employeeId, 24) - (300 + 900)) < 0.001,
    'المستهلَك=' . $policy->consumed($employeeId, 24));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ١٠) الإلغاء والانتهاء ────────────────────────────────────');

$rid2 = (int) $r2['request']->id;

$foreignCancel = $approvals->cancel($rid2, $employeeId + 999999);
$check('34. ⚠ موظفٌ آخر لا يُلغي طلبَ غيره', $foreignCancel['changed'] === false);

$c = $approvals->cancel($rid2, $employeeId);
$check('35. وصاحبُ الطلب يُلغيه ما دام معلّقاً', $c['changed'] === true);

$c2 = $approvals->cancel($rid2, $employeeId);
$check('36. ولا يُلغى مرّتين', $c2['changed'] === false);

$r3 = $mk('cid-expire', 500.0, '910000040', 'منتهٍ', [EmployeeLimitPolicy::R_PER_TRANSFER],
    $policy->forEmployee($employeeId));
DB::table('employee_approval_requests')->where('id', $r3['request']->id)
    ->update(['expires_at' => now()->subHour()]);

$n = $approvals->expireDue($agentId);
$check('37. ⚠ والمنتهي يُنهى عند القراءة — لا يبقى معلّقاً للأبد',
    $n >= 1
    && DB::table('employee_approval_requests')->where('id', $r3['request']->id)
        ->value('status') === 'EXPIRED');

$check('38. ⚠ ولا يُنفَّذ تلقائياً بانتهائه',
    DB::table('InternalEx')->count() === $midInternal);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ١١) تغييرُ السقف لا يغيّر التاريخ ────────────────────────');

$snapBefore = DB::table('employee_approval_requests')->where('id', $rid)
    ->value('policy_snapshot');

DB::table('employee_transfer_policies')->where('employee_id', $employeeId)
    ->update(['per_transfer_limit' => 999999]);

$check('39. ⚠ لقطةُ السقف في الطلب القديم لم تتغيّر',
    DB::table('employee_approval_requests')->where('id', $rid)
        ->value('policy_snapshot') === $snapBefore);

$check('40. ⚠ ورفعُ السقف لا يعتمد طلباً مرفوضاً تلقائياً',
    DB::table('employee_approval_requests')->where('id', $rid)->value('status') === 'REJECTED');

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ١٢) العرضُ للوكيل ────────────────────────────────────────');

$row = DB::table('employee_approval_requests')->where('id', $rid)->first();
$pres = EmployeeApprovals::present($row);

$check('41. يرى الوكيلُ ما يقرّر به',
    ($pres['employee_name'] ?? '') === $empName
    && $pres['amount'] > 0
    && $pres['recipient_phone'] !== null
    && $pres['reason_labels'] !== []
    && $pres['limit_at_request'] !== null,
    $pres['employee_name'] . ' · ' . $pres['amount'] . ' · ' . implode('/', $pres['reason_labels']));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── تنظيف ────────────────────────────────────────────────────');

DB::table('employee_approval_requests')->where('employee_id', $employeeId)->delete();
DB::table('employee_transfer_policies')->where('employee_id', $employeeId)->delete();
DB::table('transfer_attributions')->where('employee_id', $employeeId)->delete();
DB::table('audit_logs')->where('employee_id', $employeeId)->delete();
DB::table('employees')->where('id', $employeeId)->delete();
$line('  حُذف الموظف الاختباريّ وسياستُه وطلباتُه.');

$line();
$line('── اللقطة المالية ────────────────────────────────────────────');
$after = $snap();
$same = true;
foreach ($before as $k => $v2) {
    $eq = ($v2 === $after[$k]);
    if (!$eq) $same = false;
    printf("  %-10s %-14s ⇦ %-14s %s\n", $k, $v2, $after[$k], $eq ? '✓' : '✗');
}
$check('⚠ لا شيء ماليّ تغيّر — لا رصيد ولا قيد ولا حوالة', $same);

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) $line('   المُخفِق: ' . implode(' · ', $failed));
$line('══════════════════════════════════════════════════════════════');
