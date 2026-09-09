<?php

/*
 * ════════════════════════════════════════════════════════════════════════════
 *  كشفُ خزينة الموظف + تسليمُ الحوالات الواردة — اختبارات قبول
 * ════════════════════════════════════════════════════════════════════════════
 *
 *   php artisan tinker --execute="require base_path('tests/manual/employee_cashbox_ledger_acceptance.php');"
 *
 * ويحتاج القسمُ الرابع خادماً يعمل على 127.0.0.1:8000 (`php artisan serve`).
 * وبلا خادمٍ تُخطّى فحوصُه ويُعلَن ذلك — لا تُحسب نجاحاً.
 *
 * ── ما يُثبَّت هنا ─────────────────────────────────────────────────────────
 *
 *  1) المعادلة: افتتاحيّ + داخل − خارج = الرصيد، حركةً حركة.
 *  2) ورصيدُ آخر حركةٍ في وردية مفتوحة == `expectedCash` لها. رقمٌ واحد.
 *  3) والمعكوسةُ تُعرض ولا تُحسب.
 *  4) والرصيدُ يُصفَّر عند كل وردية ولا يتراكم — الوردية دورةُ تصفية.
 *  5) وقيدُ الإنشاء وقيدُ التسليم للحوالة نفسِها **لا يصطدمان**.
 *  6) وموظفان يسلّمان في اللحظة ذاتها: واحدٌ يفوز، ولا أثرَ ماليّاً مزدوجاً.
 *  7) والصلاحياتُ من الخادم لا من الواجهة.
 *  8) ولا شيء من هذا يمسّ حسابَ الوكيل مع الرحالة — بصمةٌ ماليّة قبلُ وبعدُ.
 * ════════════════════════════════════════════════════════════════════════════
 */

use App\Services\Employees\EmployeeCashboxService;
use App\Services\Employees\EmployeeCashboxLedger;
use Illuminate\Support\Facades\DB;

$svc    = app(EmployeeCashboxService::class);
$ledger = app(EmployeeCashboxLedger::class);

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $skip = 0;
$check = function (string $name, bool $pass, string $detail = '') use (&$ok, &$fail, $line) {
    if ($pass) { $ok++;   $line("  PASS  $name" . ($detail ? "  ($detail)" : '')); }
    else       { $fail++; $line("  FAIL  $name" . ($detail ? "  ($detail)" : '')); }
};
$skipped = function (string $name, string $why) use (&$skip, $line) {
    $skip++; $line("  SKIP  $name  ($why)");
};
/** مقارنةُ مالٍ بعتبةٍ لا بتساوٍ صارم. */
$eq = fn ($a, $b) => abs((float) $a - (float) $b) < 0.0005;

$agentId = 104;
$phones  = ['911234701', '911234702', '911234703'];

$line('════════════════════════════════════════════════════════════');
$line(' كشف خزينة الموظف + تسليم الحوالات — قبول');
$line('════════════════════════════════════════════════════════════');

/* ── البصمة المالية قبل أي شيء ───────────────────────────────────────── */
$snapshot = fn () => [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'ledger'   => DB::table('ExchangeAccData')->count(),
    'internal' => DB::table('InternalEx')->count(),
    'safe'     => DB::table('EX24AccSafeActivityTb')->count(),
    'accounts' => DB::table('AccountsTb')->count(),
];
$before = $snapshot();

/* ── تنظيفُ بقايا تشغيلٍ سابق ─────────────────────────────────────────── */
$cleanup = function () use ($phones) {
    $ids = DB::table('employees')->whereIn('phone', $phones)->pluck('id');
    if ($ids->isEmpty()) { return; }

    DB::table('transfer_status_history')
        ->whereIn('changed_by_employee_id', $ids)->delete();
    DB::table('transfer_attributions')->whereIn('employee_id', $ids)->delete();

    /* العكوسُ أولاً: صفٌّ يشير إلى صفٍّ في الجدول نفسِه. */
    DB::table('employee_cashbox_entries')->whereIn('employee_id', $ids)
        ->whereNotNull('reversal_of')->delete();
    DB::table('employee_cashbox_entries')->whereIn('employee_id', $ids)->delete();

    DB::table('employee_shift_closings')->whereIn('employee_id', $ids)->delete();
    DB::table('employee_shifts')->whereIn('employee_id', $ids)->delete();
    DB::table('employee_cashboxes')->whereIn('employee_id', $ids)->delete();
    DB::table('employee_permissions')->whereIn('employee_id', $ids)->delete();
    DB::table('employee_sessions')->whereIn('employee_id', $ids)->delete();
    DB::table('employee_devices')->whereIn('employee_id', $ids)->delete();
    DB::table('employee_activation_codes')->whereIn('employee_id', $ids)->delete();
    DB::table('employee_otps')->whereIn('employee_id', $ids)->delete();
    DB::table('employees')->whereIn('id', $ids)->delete();
};
$cleanup();

DB::table('agent_incoming_transfers')
    ->where('transfer_number', 'like', 'LEDGERTEST-%')->delete();

$mkEmployee = function (string $phone, string $name) use ($agentId) {
    return (int) DB::table('employees')->insertGetId([
        'agent_id'   => $agentId,
        'full_name'  => $name,
        'phone'      => $phone,
        'status'     => 'ACTIVE',
        'created_at' => now(),
        'updated_at' => now(),
    ]);
};

$empA = $mkEmployee($phones[0], 'موظف كشف أ');

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ١) المعادلة، حركةً حركة ──────────────────────────────────');

$shift1 = $svc->startShift([
    'agent_id' => $agentId, 'employee_id' => $empA, 'opening_cash' => 1000,
]);
$cashboxId = $shift1['cashbox_id'];

$add = function (string $type, float $amount, string $dir, ?string $ref = null,
                 ?string $refType = null, ?int $shiftId = null)
       use ($svc, $agentId, $empA, $cashboxId, $shift1) {
    return $svc->addEntry([
        'agent_id'         => $agentId,
        'employee_id'      => $empA,
        'cashbox_id'       => $cashboxId,
        'shift_id'         => $shiftId ?? $shift1['id'],
        'transaction_type' => $type,
        'reference_type'   => $refType,
        'reference_id'     => $ref,
        'amount'           => $amount,
        'direction'        => $dir,
        'created_by'       => $empA,
    ]);
};

$e1 = $add('TRANSFER_CREATED',  500, 'IN',  'LEDGERTEST-1', 'INTERNAL_TRANSFER_CREATED');
$e2 = $add('TRANSFER_DELIVERY', 200, 'OUT', 'LEDGERTEST-9', 'INTERNAL_TRANSFER');
$e3 = $add('CASH_RECEIVED',     100, 'IN');
$e4 = $add('CASH_HANDOVER',      50, 'OUT');

$emp = DB::table('employees')->where('id', $empA)->first();
$L = $ledger->ledger($emp);

$moves = array_values(array_filter($L['rows'], fn ($r) => $r['kind'] === 'MOVE'));
$open  = array_values(array_filter($L['rows'], fn ($r) => $r['kind'] === 'OPENING'));

$check('1. صفٌّ افتتاحيّ واحد، مُصطنَع، بقيمة الافتتاحيّ',
    count($open) === 1 && $open[0]['synthetic'] === true && $eq($open[0]['in'], 1000),
    'الافتتاحيّ = ' . ($open[0]['in'] ?? '—'));

$check('2. أربعُ حركات بالترتيب الزمنيّ', count($moves) === 4);

$expected = [1500.0, 1300.0, 1400.0, 1350.0];
$got = array_map(fn ($m) => (float) $m['balance'], $moves);
$check('3. ⚠ الرصيدُ بعد كل حركة صحيح',
    count($got) === 4 && $eq($got[0], $expected[0]) && $eq($got[1], $expected[1])
        && $eq($got[2], $expected[2]) && $eq($got[3], $expected[3]),
    implode(' → ', array_map(fn ($v) => rtrim(rtrim(number_format($v, 3, '.', ''), '0'), '.'), $got)));

$check('4. المعادلة: افتتاحيّ + داخل − خارج = الرصيد',
    $eq($L['opening'] + $L['in'] - $L['out'], end($got)),
    "{$L['opening']} + {$L['in']} − {$L['out']}");

$calc = $svc->expectedCash($cashboxId, (int) $shift1['id']);
$check('5. ⚠ ورصيدُ الكشف == المتوقَّع للوردية — رقمٌ واحد لا رقمان',
    $eq($L['current'], $calc['expected']) && $eq(end($got), $calc['expected']),
    "كشف={$L['current']} متوقَّع={$calc['expected']}");

$check('6. الافتتاحيّ لا يُجمَع في «الداخل» فيُعَدّ مرّتين',
    $eq($L['in'], 600), 'الداخل = ' . $L['in']);

$refs = array_column($moves, 'reference');
$check('7. رقمُ المرجع يظهر مع الحركة',
    in_array('LEDGERTEST-1', $refs, true) && in_array('LEDGERTEST-9', $refs, true));

$labels = array_column($moves, 'type_label');
$check('8. ⚠ ولا نوعَ يظهر برمزه الإنجليزيّ',
    !in_array('حركة', $labels, true), implode(' · ', $labels));

$check('9. ولكل حركةٍ حالة', count(array_filter($moves, fn ($m) => $m['status'] !== '')) === 4);

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٢) المعكوسةُ تُعرض ولا تُحسب ─────────────────────────────');

$balanceBefore = $L['current'];
$rev = $svc->reverseEntry((int) $e3['id'], $empA, 'اختبار العكس');

$L2 = $ledger->ledger($emp);
$calc2 = $svc->expectedCash($cashboxId, (int) $shift1['id']);

$moves2 = array_values(array_filter($L2['rows'], fn ($r) => $r['kind'] === 'MOVE'));

/* العكسُ صفٌّ واحدٌ يُضاف والأصلُ يبقى: أربعٌ + واحد = خمس. */
$check('10. صفُّ الأصل وصفُّ العكس كلاهما معروض',
    count($moves2) === 5, 'حركات = ' . count($moves2));

$notCounted = array_filter($moves2, fn ($m) => $m['counted'] === false);
$check('11. ⚠ وكلاهما غيرُ محسوب', count($notCounted) === 2);

$check('12. ⚠ والرصيدُ عاد إلى ما قبل الحركة المعكوسة',
    $eq($L2['current'], $balanceBefore - 100),
    "قبل={$balanceBefore} بعد={$L2['current']}");

$check('13. ⚠ ورصيدُ الكشف ما زال == المتوقَّع بعد العكس',
    $eq($L2['current'], $calc2['expected']),
    "كشف={$L2['current']} متوقَّع={$calc2['expected']}");

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٣) الوردية دورةُ تصفية — الرصيدُ لا يتراكم ───────────────');

$closed = $svc->closeShift((int) $shift1['id'], (float) $calc2['expected'], $agentId, 'إقفال اختبار');
$check('14. الإقفالُ يُطابق', ($closed['result'] ?? '') === 'MATCH', $closed['result'] ?? '—');

$shift2 = $svc->startShift([
    'agent_id' => $agentId, 'employee_id' => $empA, 'opening_cash' => 300,
]);
$add('CASH_RECEIVED', 70, 'IN', null, null, (int) $shift2['id']);

$L3 = $ledger->ledger($emp);
$opens3 = array_values(array_filter($L3['rows'], fn ($r) => $r['kind'] === 'OPENING'));
$closes3 = array_values(array_filter($L3['rows'], fn ($r) => $r['kind'] === 'CLOSING'));

$check('15. قسمانِ بعددِ الورديتين', count($opens3) === 2, 'أقسام = ' . count($opens3));
$check('16. وصفُّ إقفالٍ للمقفلة وحدَها', count($closes3) === 1);

$check('17. ⚠ وصفُّ الإقفال يقول المتوقَّع والمعدود والفرق',
    isset($closes3[0]['expected'], $closes3[0]['actual'], $closes3[0]['difference'])
        && $eq($closes3[0]['difference'], 0));

$check('18. ⚠ والرصيدُ ابتدأ من افتتاحيّ الوردية الثانية لا من رصيد الأولى',
    $eq($opens3[1]['balance'], 300),
    'رصيد بداية الثانية = ' . $opens3[1]['balance']);

$calc3 = $svc->expectedCash($cashboxId, (int) $shift2['id']);
$check('19. ⚠ والرصيدُ الحالي == متوقَّعُ الوردية المفتوحة وحدَها',
    $eq($L3['current'], 370) && $eq($L3['current'], $calc3['expected']),
    "كشف={$L3['current']} متوقَّع={$calc3['expected']}");

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٤) الفلترة ───────────────────────────────────────────────');

$Lf = $ledger->ledger($emp, ['type' => 'CASH_RECEIVED']);
$movesF = array_values(array_filter($Lf['rows'], fn ($r) => $r['kind'] === 'MOVE'));
$check('20. فلترةُ النوع تُرجع نوعَها وحدَه',
    $movesF !== [] && count(array_filter($movesF,
        fn ($m) => $m['type'] !== 'CASH_RECEIVED')) === 0);

$check('21. ⚠ وتُعلن أنّ المجموعَ جزئيّ — فلا يُقرأ الجزءُ كلاًّ',
    $Lf['partial'] === true);

$Lall = $ledger->ledger($emp);
$check('22. وبلا فلترةٍ المجموعُ كليّ', $Lall['partial'] === false);

$Lt = $ledger->ledger($emp, ['type' => 'لا-يوجد-هذا']);
$check('23. ⚠ ونوعٌ مجهول يُهمَل ولا يُفرِغ الكشف',
    $Lt['type'] === null && $Lt['rows'] !== []);

$tomorrow = now()->addDay()->toDateString();
$Ld = $ledger->ledger($emp, ['from' => $tomorrow, 'to' => $tomorrow]);
$check('24. وفلترةُ التاريخ تُحترم',
    count(array_filter($Ld['rows'], fn ($r) => $r['kind'] === 'MOVE')) === 0);

$Lbad = $ledger->ledger($emp, ['from' => 'ليس تاريخاً']);
$check('25. ⚠ وتاريخٌ مشوَّه يعني بلا فلترة لا شاشةَ خطأ',
    $Lbad['from'] === null && $Lbad['rows'] !== []);

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٥) حركةٌ خارج وردية لا تختفي من الكشف ────────────────────');

$orphan = $svc->addEntry([
    'agent_id' => $agentId, 'employee_id' => $empA, 'cashbox_id' => $cashboxId,
    'shift_id' => null, 'transaction_type' => 'ADJUSTMENT',
    'amount' => 25, 'direction' => 'IN', 'created_by' => $empA,
]);

$Lo = $ledger->ledger($emp);
$outside = array_filter($Lo['rows'], fn ($r) => !empty($r['outside_shift']));
$check('26. ⚠ تُعرض موسومةً بأنها خارج وردية', count($outside) === 1);

$tableIn = (float) DB::table('employee_cashbox_entries')
    ->where('employee_id', $empA)->where('direction', 'IN')
    ->where('is_reversed', 0)->whereNull('reversal_of')->sum('amount');
$check('27. ⚠ ومجموعُ الكشف == مجموعُ الجدول — لا صفَّ يسقط في جرد',
    $eq($Lo['in'], $tableIn), "كشف={$Lo['in']} جدول={$tableIn}");

$check('28. ولا تدخل في متوقَّع الوردية',
    $eq($ledger->ledger($emp)['current'], 370));

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٦) ⚠ قيدُ الإنشاء وقيدُ التسليم للحوالة نفسِها ───────────');

$sameCode = 'LEDGERTEST-SAME';
$cin  = $add('TRANSFER_CREATED',  800, 'IN',  $sameCode, 'INTERNAL_TRANSFER_CREATED',
             (int) $shift2['id']);
$cout = $add('TRANSFER_DELIVERY', 800, 'OUT', $sameCode, 'INTERNAL_TRANSFER',
             (int) $shift2['id']);

$check('29. ⚠ القيدان أُدرجا كلاهما — لا اصطدامَ على الفهرس الفريد',
    empty($cin['duplicate']) && empty($cout['duplicate'])
        && (int) $cin['id'] !== (int) $cout['id'],
    'إنشاء=' . ($cin['duplicate'] ?? false ? 'أُسقط' : 'أُدرج')
    . ' تسليم=' . ($cout['duplicate'] ?? false ? 'أُسقط' : 'أُدرج'));

$dupOut = $add('TRANSFER_DELIVERY', 800, 'OUT', $sameCode, 'INTERNAL_TRANSFER',
               (int) $shift2['id']);
$check('30. ⚠ وتسليمٌ ثانٍ للحوالة نفسِها يُرَدّ تكراراً — لا قيدَ ثانٍ',
    !empty($dupOut['duplicate']) && (int) $dupOut['id'] === (int) $cout['id']);

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٧) موظفان يسلّمان الحوالة نفسها في اللحظة ذاتها ──────────');

$base = 'http://127.0.0.1:8000/api';
$alive = @file_get_contents($base . '/../up') !== false
      || @fsockopen('127.0.0.1', 8000, $x, $y, 1) !== false;

if (!$alive) {
    $skipped('31–41. سيناريو التزامن عبر HTTP', 'لا خادم على 127.0.0.1:8000');
} else {
    /* موظفان بصلاحية التسليم، ولكلٍّ جلستُه وجهازُه ووردية. */
    $empB = $mkEmployee($phones[1], 'موظف تسليم ب');
    $empC = $mkEmployee($phones[2], 'موظف تسليم ج');

    $session = function (int $employeeId) use ($agentId) {
        $token  = bin2hex(random_bytes(24));
        $device = hash('sha256', 'dev-' . $employeeId);

        DB::table('employee_devices')->insert([
            'agent_id' => $agentId, 'employee_id' => $employeeId,
            'device_hash' => $device, 'status' => 'ACTIVE',
            'activated_at' => now(),
        ]);
        DB::table('employee_sessions')->insert([
            'agent_id' => $agentId, 'employee_id' => $employeeId,
            'access_token_hash' => hash('sha256', $token),
            'device_hash' => $device, 'status' => 'ACTIVE',
            'created_at' => now(),
        ]);
        return $token;
    };

    $grant = function (int $employeeId, array $keys) use ($agentId) {
        foreach ($keys as $k) {
            DB::table('employee_permissions')->insert([
                'employee_id' => $employeeId, 'permission_key' => $k,
                'granted_by' => $agentId, 'granted_at' => now(),
            ]);
        }
    };

    $tokB = $session($empB);
    $tokC = $session($empC);
    $grant($empB, ['VIEW_INCOMING_TRANSFERS', 'DELIVER_TRANSFER', 'VIEW_OWN_CASHBOX']);
    $grant($empC, ['VIEW_INCOMING_TRANSFERS', 'DELIVER_TRANSFER']);

    $svc->startShift(['agent_id' => $agentId, 'employee_id' => $empB, 'opening_cash' => 0]);
    $svc->startShift(['agent_id' => $agentId, 'employee_id' => $empC, 'opening_cash' => 0]);

    /* حوالةٌ واحدة بانتظار التسليم — كيانٌ واحد لا نسخةٌ لكلّ موظف. */
    $code = 'LEDGERTEST-' . time();
    $transferId = (int) DB::table('agent_incoming_transfers')->insertGetId([
        'agent_id'          => $agentId,
        'transfer_number'   => $code,
        'beneficiary_name'  => 'مستفيد اختبار التزامن',
        'amount'            => 640,
        'status'            => 'PENDING_DELIVERY',
        'core_confirm_type' => 2,
        'core_status_label' => 'مسلمه',
        'core_synced_at'    => now(),
        'received_at'       => now(),
        'created_at'        => now(),
        'updated_at'        => now(),
    ]);

    /*
     * ⚠ **طلباتٌ متوازية حقيقيّة، لا حلقةٌ متتابعة.**
     *
     * حلقةٌ تُنفَّذ واحدةً بعد واحدة تُثبت التحايُد ولا تُثبت شيئاً عن
     * التزامن: الشرطُ الذي يُختبَر هنا هو ما يجري بين قراءة الحالة
     * وتحديثها، وهو لا يقع إلّا مع تسابقٍ فعليّ على الصفّ.
     */
    $mh = curl_multi_init();
    $handles = [];
    foreach ([$tokB, $tokB, $tokC, $tokC] as $i => $tok) {
        $ch = curl_init("$base/device/employee/transfers/$transferId/deliver");
        curl_setopt_array($ch, [
            CURLOPT_POST => true,
            CURLOPT_POSTFIELDS => '{}',
            CURLOPT_RETURNTRANSFER => true,
            CURLOPT_TIMEOUT => 30,
            CURLOPT_HTTPHEADER => [
                'Accept: application/json',
                'Content-Type: application/json',
                'Authorization: Bearer ' . $tok,
            ],
        ]);
        curl_multi_add_handle($mh, $ch);
        $handles[$i] = ['ch' => $ch, 'who' => $tok === $tokB ? 'ب' : 'ج'];
    }
    $running = null;
    do { curl_multi_exec($mh, $running); curl_multi_select($mh, 0.5); } while ($running > 0);

    $results = [];
    foreach ($handles as $i => $h) {
        $body = curl_multi_getcontent($h['ch']);
        $status = curl_getinfo($h['ch'], CURLINFO_HTTP_CODE);
        curl_multi_remove_handle($mh, $h['ch']);
        curl_close($h['ch']);
        $results[$i] = [
            'who' => $h['who'], 'status' => $status,
            'json' => json_decode($body, true) ?: [],
        ];
    }
    curl_multi_close($mh);

    $changed = array_filter($results, fn ($r) => ($r['json']['data']['changed'] ?? false) === true);

    $check('31. ⚠ طلبٌ واحدٌ فقط غيَّر الحالة من بين أربعة متوازية',
        count($changed) === 1, 'غيَّر = ' . count($changed) . ' من 4');

    $check('32. ولا طلبَ أخفق بخطأ خادم',
        count(array_filter($results, fn ($r) => $r['status'] >= 500)) === 0,
        implode(' · ', array_map(fn ($r) => $r['who'] . ':' . $r['status'], $results)));

    $row = DB::table('agent_incoming_transfers')->where('id', $transferId)->first();
    $check('33. والحوالةُ صارت مسلَّمة', $row->status === 'DELIVERED', $row->status);

    /* ── ولا أثرَ ماليّاً مزدوجاً ─────────────────────────────────── */
    $attr = DB::table('transfer_attributions')
        ->where('transfer_number', $code)->where('action', 'DELIVERED')->count();
    $check('34. ⚠ نسبةُ تسليمٍ واحدة لا أربع', $attr === 1, "نسب = $attr");

    $entries = DB::table('employee_cashbox_entries')
        ->where('reference_id', $code)->where('reference_type', 'INTERNAL_TRANSFER')
        ->whereNull('reversal_of')->get();
    $check('35. ⚠ وقيدُ خزينةٍ واحد لا أكثر', $entries->count() === 1,
        'قيود = ' . $entries->count());
    $check('36. ⚠ وبقيمة الحوالة وخروجاً',
        $entries->count() === 1 && $eq($entries[0]->amount, 640)
            && $entries[0]->direction === 'OUT');

    $hist = DB::table('transfer_status_history')->where('transfer_id', $transferId)->get();
    $check('37. ⚠ وصفُّ تحوُّلٍ واحد', $hist->count() === 1, 'صفوف = ' . $hist->count());

    $winner = $entries->count() === 1 ? (int) $entries[0]->employee_id : 0;
    $check('38. ⚠ وصفُّ التحوُّل يسمّي الموظفَ المنفِّذ لا وكيلَه وحدَه',
        $hist->count() === 1 && (int) $hist[0]->changed_by_employee_id === $winner
            && $winner > 0,
        'employee_id = ' . ($hist[0]->changed_by_employee_id ?? 'null'));

    $check('39. ⚠ والحالتان القديمةُ والجديدة مسجَّلتان',
        $hist->count() === 1 && $hist[0]->old_status === 'PENDING_DELIVERY'
            && $hist[0]->new_status === 'DELIVERED');

    $audit = DB::table('audit_logs')->where('action', 'TRANSFER_DELIVERED')
        ->where('entity_id', $code)->get();
    $check('40. وصفُّ تدقيقٍ يحمل الموظف والوكيل والجهاز',
        $audit->count() === 1 && (int) $audit[0]->employee_id === $winner
            && (int) $audit[0]->agent_id === $agentId
            && !empty($audit[0]->device_hash));

    /* ── الرسالةُ لمن سبقه غيرُه ──────────────────────────────────── */
    $losers = array_filter($results, fn ($r) => ($r['json']['data']['changed'] ?? false) !== true);
    $told = array_filter($losers,
        fn ($r) => ($r['json']['data']['already_delivered_by_other'] ?? null) === true);
    $mine = array_filter($losers,
        fn ($r) => ($r['json']['data']['already_delivered_by_other'] ?? null) === false);

    $check('41. ⚠ ومن سبقه زميلُه يُبلَّغ صراحةً',
        count($told) >= 1
            && str_contains($told[array_key_first($told)]['json']['message'] ?? '',
                            'تم تسليم هذه الحوالة مسبقاً'),
        'أُبلغوا = ' . count($told) . ' من ' . count($losers));

    $check('42. ⚠ وإعادةُ إرسالِ الطلب نفسِه ليست رفضاً — وإلّا دفع مرّتين',
        count($mine) === 0 || str_contains(
            $mine[array_key_first($mine)]['json']['message'] ?? '', 'تم تسجيل التسليم'),
        'إعاداتُ الفائز = ' . count($mine));

    /* ── والحوالةُ تغادر قائمةَ الانتظار عند الجميع ───────────────── */
    $callAs = function (string $method, string $path, string $tok) use ($base) {
        $ch = curl_init($base . $path);
        curl_setopt_array($ch, [
            CURLOPT_CUSTOMREQUEST => $method,
            CURLOPT_RETURNTRANSFER => true,
            CURLOPT_TIMEOUT => 30,
            CURLOPT_HTTPHEADER => [
                'Accept: application/json', 'Content-Type: application/json',
                'Authorization: Bearer ' . $tok,
            ],
        ]);
        $raw = curl_exec($ch);
        $st = curl_getinfo($ch, CURLINFO_HTTP_CODE);
        curl_close($ch);
        return ['status' => $st, 'json' => json_decode($raw, true) ?: []];
    };

    $pendB = $callAs('GET', '/device/employee/transfers/incoming?status=PENDING_DELIVERY', $tokB);
    $itemsB = $pendB['json']['data']['items'] ?? $pendB['json']['data']['data'] ?? [];
    $stillB = array_filter((array) $itemsB,
        fn ($t) => ($t['transfer_number'] ?? '') === $code);
    $check('43. ⚠ وغادرت قائمةَ انتظارِ الفائز', count($stillB) === 0);

    $pendC = $callAs('GET', '/device/employee/transfers/incoming?status=PENDING_DELIVERY', $tokC);
    $itemsC = $pendC['json']['data']['items'] ?? $pendC['json']['data']['data'] ?? [];
    $stillC = array_filter((array) $itemsC,
        fn ($t) => ($t['transfer_number'] ?? '') === $code);
    $check('44. ⚠ وقائمةَ انتظارِ زميله — كيانٌ واحدٌ لا نسخةٌ لكلٍّ',
        count($stillC) === 0);

    // ══════════════════════════════════════════════════════════════════
    $line();
    $line('── ٨) الصلاحياتُ من الخادم ──────────────────────────────────');

    /* موظف ج مُنح التسليم ولم يُمنح الخزينة. */
    $r = $callAs('GET', '/device/employee/cashbox/ledger', $tokC);
    $check('45. ⚠ بلا VIEW_OWN_CASHBOX لا كشفَ ولو نُودي المسارُ مباشرةً',
        $r['status'] === 403, 'HTTP ' . $r['status']);

    $r = $callAs('GET', '/device/employee/cashbox/ledger', $tokB);
    $check('46. وبها يُقرأ', $r['status'] === 200, 'HTTP ' . $r['status']);

    $check('47. ⚠ والكشفُ كشفُ صاحب الجلسة — لا معاملَ يقبل معرّفَ موظفٍ آخر',
        ($r['json']['data']['count'] ?? -1) === 1,
        'حركاتُ ب = ' . ($r['json']['data']['count'] ?? '—'));

    /* سحبُ صلاحية التسليم يسري في الطلب التالي لا بعد إعادة الدخول. */
    DB::table('employee_permissions')->where('employee_id', $empB)
        ->where('permission_key', 'DELIVER_TRANSFER')->delete();

    $r = $callAs('POST', "/device/employee/transfers/$transferId/deliver", $tokB);
    $check('48. ⚠ وسحبُ الصلاحية يُغلق البابَ فوراً',
        $r['status'] === 403, 'HTTP ' . $r['status']);

    $sec = DB::table('security_logs')->where('event_type', 'UNAUTHORIZED')
        ->where('employee_id', $empB)->count();
    $check('49. والمحاولةُ سُجّلت أمنيّاً', $sec >= 1, "صفوف = $sec");

    $r = $callAs('GET', '/device/employee/cashbox/ledger', 'رمز-مزيّف');
    $check('50. ورمزٌ مزيَّف لا يفتح شيئاً', $r['status'] === 401, 'HTTP ' . $r['status']);

    /* ── قائمةُ الانتظار خلف صلاحية التسليم ─────────────────────── */

    /* موظف ب يملك VIEW_INCOMING_TRANSFERS من أول الاختبار، وسُحبت منه
       DELIVER_TRANSFER في الفحص 48 — فهو الآن الحالةُ المطلوبة تماماً:
       يرى الدفتر ولا يسلّم. */

    $r = $callAs('GET',
        '/device/employee/transfers/incoming?status=PENDING_DELIVERY', $tokB);
    $check('51. ⚠ بلا صلاحية التسليم لا تُفتح قائمةُ الانتظار ولو نُودي المسار',
        $r['status'] === 403, 'HTTP ' . $r['status']);

    $r = $callAs('GET', '/device/employee/transfers/incoming', $tokB);
    $rows = $r['json']['data']['rows'] ?? $r['json']['data']['data'] ?? [];
    $leaked = array_filter((array) $rows,
        fn ($t) => ($t['status'] ?? '') === 'PENDING_DELIVERY');
    $check('52. ⚠ وحذفُ المعامل لا يتجاوز الحارس',
        $r['status'] === 200 && count($leaked) === 0,
        'HTTP ' . $r['status'] . ' · معلَّقة مسرَّبة = ' . count($leaked));

    $r = $callAs('GET',
        '/device/employee/transfers/incoming?status=DELIVERED', $tokB);
    $check('53. والمسلَّمةُ تبقى له — استعلامٌ لا عمل',
        $r['status'] === 200, 'HTTP ' . $r['status']);

    $r = $callAs('GET',
        '/device/employee/transfers/incoming?status=PENDING_DELIVERY', $tokC);
    $check('54. ومن يملكها يراها',
        $r['status'] === 200, 'HTTP ' . $r['status']);
}

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٩) البصمة المالية ────────────────────────────────────────');

$after = $snapshot();
foreach ($before as $k => $v) {
    $check("51. $k لم يتغيّر", (string) $after[$k] === (string) $v,
        "قبل=$v بعد={$after[$k]}");
}

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── تنظيف ────────────────────────────────────────────────────');
$cleanup();
DB::table('agent_incoming_transfers')->where('transfer_number', 'like', 'LEDGERTEST-%')->delete();
DB::table('audit_logs')->where('entity_id', 'like', 'LEDGERTEST-%')->delete();
$line('  نُظِّف كلُّ ما أُنشئ.');

$line();
$line('════════════════════════════════════════════════════════════');
$line("  نجح: $ok   أخفق: $fail   تُخطّي: $skip");
$line('════════════════════════════════════════════════════════════');
