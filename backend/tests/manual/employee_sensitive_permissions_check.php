<?php

/*
 * ════════════════════════════════════════════════════════════════════════════
 *  رصيدُ الوكيل لا يظهر للموظف إلّا بصلاحيةٍ يمنحها الوكيل بيده
 *  أمر المالك — 9 سبتمبر 2026
 * ════════════════════════════════════════════════════════════════════════════
 *
 *   php artisan tinker --execute="require base_path('tests/manual/employee_sensitive_permissions_check.php');"
 *
 * ويحتاج القسمُ الثالث خادماً على 127.0.0.1:8000.
 *
 * ⚠ **ما لم تكن المشكلة**: تسرُّباً. القسمُ الثالث يُثبت أنّ كلَّ مسارِ رصيدٍ
 * يردّ 403 بلا صلاحيته، وأنّ لا نقطةَ أخرى تحمل الرقم — وذلك كان قائماً قبل
 * هذا التغيير، ويجب أن يبقى.
 *
 * ⚠ **وما كانت المشكلة**: «تحديد الكل». مجموعةُ التقارير تضمّ «تقرير رصيد
 * الوكيل»، فضغطةٌ واحدة يريد بها الوكيل إعطاءَ تقاريرِ اليوم كانت تُسلّم
 * رصيدَه معها بلا أن يلاحظ.
 */

use App\Services\Employees\EmployeePermissions as P;
use App\Services\Employees\EmployeeReports;
use Illuminate\Support\Facades\DB;

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $skip = 0;
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, $line) {
    if ($p) { $ok++;   $line('  PASS  ' . $n . ($d ? "  ($d)" : '')); }
    else    { $fail++; $line('  FAIL  ' . $n . ($d ? "  ($d)" : '')); }
};

$line('════════════════════════════════════════════════════════════');
$line(' رصيد الوكيل: صلاحيةٌ تُمنح بيدٍ لا بضغطةٍ جماعية');
$line('════════════════════════════════════════════════════════════');

$before = [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => DB::table('InternalEx')->count(),
];

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ١) الكتالوج يَسِم ما يكشف رصيد الوكيل ────────────────────');

$catalog = P::catalogForAdmin();
$flat = [];
foreach ($catalog as $g) {
    foreach ($g['items'] as $i) {
        $flat[$i['key']] = $i + ['group' => $g['group']];
    }
}

$check('1. الرصيدُ الكلّي موسومٌ حسّاساً',
    ($flat['VIEW_AGENT_TOTAL_BALANCE']['sensitive'] ?? false) === true);

$check('2. وتقريرُ الرصيد كذلك — وهو المدفونُ في مجموعة التقارير',
    ($flat['REPORT_AGENT_BALANCE']['sensitive'] ?? false) === true);

$check('3. ⚠ والملخّصُ الماليّ غيرُ موسوم — لا رصيدَ فيه',
    ($flat['VIEW_FINANCIAL_SUMMARY']['sensitive'] ?? true) === false);

/* ⚠ الوسمُ الكاذب أسوأ من غيابه: يُعلّم الوكيل تجاهلَ العلامة حين تهمّ. */
$keys = array_keys(app(EmployeeReports::class)
    ->summary((object) ['id' => 0, 'agent_id' => 104]));
$check('4. ⚠ وذلك صحيحٌ فعلاً: الملخّصُ لا يُرجع رصيداً',
    !in_array('balance', $keys, true) && !in_array('wallet', $keys, true),
    implode(' · ', $keys));

$sensitiveCount = count(array_filter($flat, fn ($i) => $i['sensitive'] ?? false));
$check('5. ولا شيءَ آخر موسوم — الوسمُ نادرٌ ليبقى مقروءاً',
    $sensitiveCount === 2, 'موسومة = ' . $sensitiveCount);

foreach (P::SENSITIVE as $k) {
    $check('6. يحمل سببَه بنصٍّ من الخادم: ' . $k,
        !empty($flat[$k]['why']), $flat[$k]['why'] ?? '—');
}

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٢) «تحديد الكل» في كل مجموعة لا يمنح رصيداً ──────────────');

/*
 * محاكاةُ ما يفعله التطبيق بالضبط: لكلّ مجموعةٍ يُمنح ما ليس حسّاساً.
 * ⚠ والمحاكاةُ تتبع العَلَم لا اسمَ المفتاح — كما التطبيق تماماً، إذ لا اسمَ
 * صلاحيةٍ مكتوبٌ في ملفّ Dart واحد.
 */
$bulk = [];
foreach ($catalog as $g) {
    foreach ($g['items'] as $i) {
        if (!($i['sensitive'] ?? false)) {
            $bulk[] = $i['key'];
        }
    }
}

foreach (P::SENSITIVE as $k) {
    $check('7. ليست في حصيلة «تحديد الكل»: ' . $k, !in_array($k, $bulk, true));
}

$check('8. ومع ذلك يمنح «تحديد الكل» كلَّ ما عداهما',
    count($bulk) === count($flat) - 2,
    count($bulk) . ' من ' . count($flat));

$reports = array_filter($flat, fn ($i) => $i['group'] === 'reports');
$reportsBulk = array_filter($reports, fn ($i) => !($i['sensitive'] ?? false));
$check('9. ⚠ ومجموعةُ التقارير تُمنح كاملةً إلّا الرصيد',
    count($reportsBulk) === count($reports) - 1,
    count($reportsBulk) . ' من ' . count($reports));

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٣) والحراسةُ في الخادم لا في الوسم ───────────────────────');

$base  = 'http://127.0.0.1:8000/api';
$sock  = @fsockopen('127.0.0.1', 8000, $errno, $errstr, 1);
$alive = $sock !== false;
if ($sock) { fclose($sock); }

if (!$alive) {
    $skip++;
    $line('  SKIP  10–15. الحراسة عبر HTTP  (لا خادم على 8000)');
} else {
    $agentId = 104;
    $phone   = '911234798';

    $stale = DB::table('employees')->where('phone', $phone)->pluck('id');
    if ($stale->isNotEmpty()) {
        DB::table('employee_permissions')->whereIn('employee_id', $stale)->delete();
        DB::table('employee_sessions')->whereIn('employee_id', $stale)->delete();
        DB::table('employee_devices')->whereIn('employee_id', $stale)->delete();
        DB::table('employees')->whereIn('id', $stale)->delete();
    }

    $id = (int) DB::table('employees')->insertGetId([
        'agent_id'   => $agentId,
        'full_name'  => 'موظف بلا رصيد',
        'phone'      => $phone,
        'status'     => 'ACTIVE',
        'created_at' => now(),
        'updated_at' => now(),
    ]);

    $token  = bin2hex(random_bytes(24));
    $device = hash('sha256', 'sens-' . $id);
    DB::table('employee_devices')->insert([
        'agent_id' => $agentId, 'employee_id' => $id, 'device_hash' => $device,
        'status' => 'ACTIVE', 'activated_at' => now(),
    ]);
    DB::table('employee_sessions')->insert([
        'agent_id' => $agentId, 'employee_id' => $id,
        'access_token_hash' => hash('sha256', $token),
        'device_hash' => $device, 'status' => 'ACTIVE', 'created_at' => now(),
    ]);

    /*
     * ⚠ يُمنح **حصيلةَ «تحديد الكل» كاملةً** — أي كلَّ ما يستطيع الوكيل منحَه
     * بضغطةٍ واحدة في كل مجموعة. فإن ظهر الرصيد بعد ذلك فقد ظهر بلا قرارٍ منه.
     */
    foreach ($bulk as $k) {
        if (!P::grantable($k)) { continue; }
        DB::table('employee_permissions')->insert([
            'employee_id' => $id, 'permission_key' => $k,
            'granted_by' => $agentId, 'granted_at' => now(),
        ]);
    }

    $call = function (string $path) use ($base, $token) {
        $ch = curl_init($base . $path);
        curl_setopt_array($ch, [
            CURLOPT_RETURNTRANSFER => true,
            CURLOPT_TIMEOUT => 25,
            CURLOPT_HTTPHEADER => [
                'Accept: application/json',
                'Authorization: Bearer ' . $token,
            ],
        ]);
        $raw = curl_exec($ch);
        $st  = curl_getinfo($ch, CURLINFO_HTTP_CODE);
        curl_close($ch);
        return [$st, (string) $raw];
    };

    [$st] = $call('/device/employee/balance');
    $check('10. ⚠ رصيدُ الوكيل مرفوضٌ لمن مُنح «الكل»', $st === 403, 'HTTP ' . $st);

    [$st] = $call('/device/employee/reports/agent-balance');
    $check('11. ⚠ وتقريرُ الرصيد كذلك', $st === 403, 'HTTP ' . $st);

    [$st] = $call('/device/employee/summary');
    $check('12. والملخّصُ يُفتح — فليس فيه رصيد', $st === 200, 'HTTP ' . $st);

    [$st] = $call('/device/employee/reports/daily');
    $check('13. وتقاريرُ العمل تُفتح كاملةً', $st === 200, 'HTTP ' . $st);

    /* ولا يظهر الرقمُ في أيّ مسارٍ مفتوح — بابٌ خلفيّ لا وسمَ عليه. */
    $real = (float) DB::table('wallet')->where('UeserID', $agentId)
        ->where('Currency_ID', 1)->value('Walet');
    $needle = rtrim(rtrim(number_format($real, 3, '.', ''), '0'), '.');

    $leaks = [];
    foreach ([
        '/device/employee/me',
        '/device/employee/summary',
        '/device/employee/reports/daily',
        '/device/employee/reports/cashbox',
        '/device/employee/cashbox/ledger',
        '/device/employee/custody',
        '/device/employee/transfers/incoming',
    ] as $path) {
        [, $body] = $call($path);
        if ($needle !== '0' && str_contains($body, $needle)) { $leaks[] = $path; }
    }
    $check('14. ⚠ ولا مسارَ مفتوحٍ يحمل الرقم من بابٍ خلفيّ',
        $leaks === [], $leaks === [] ? 'الرصيد ' . $real : implode(' · ', $leaks));

    /* والقاعدةُ منعُ المنح **الضمنيّ** لا منعُ المنح: بيدِ الوكيل تُفتح. */
    DB::table('employee_permissions')->insert([
        'employee_id' => $id, 'permission_key' => 'VIEW_AGENT_TOTAL_BALANCE',
        'granted_by' => $agentId, 'granted_at' => now(),
    ]);

    [$st] = $call('/device/employee/balance');
    $check('15. ⚠ ومنحُها بيد الوكيل يفتحها فوراً — لا مَنعَ مطلق',
        $st === 200, 'HTTP ' . $st);

    DB::table('employee_permissions')->where('employee_id', $id)->delete();
    DB::table('employee_sessions')->where('employee_id', $id)->delete();
    DB::table('employee_devices')->where('employee_id', $id)->delete();
    DB::table('employees')->where('id', $id)->delete();
}

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٤) البصمة المالية ────────────────────────────────────────');

$after = [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => DB::table('InternalEx')->count(),
];
foreach ($before as $k => $v) {
    $check('16. ' . $k . ' لم يتغيّر', (string) $after[$k] === (string) $v,
        'قبل=' . $v . ' بعد=' . $after[$k]);
}

$line();
$line('════════════════════════════════════════════════════════════');
$line('  نجح: ' . $ok . '   أخفق: ' . $fail . '   تُخطّي: ' . $skip);
$line('════════════════════════════════════════════════════════════');
