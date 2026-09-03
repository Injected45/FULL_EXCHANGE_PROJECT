<?php

/*
 * اختبارات قبول الصلاحيات — بنود 15 إلى 21 و29 و30 و32.
 *
 *   php artisan tinker --execute="require base_path('tests/manual/employee_permissions_acceptance.php');"
 *
 * تُنفَّذ عبر **HTTP حقيقي** لا باستدعاء الخدمة: المطلوب إثبات أن الرفض يقع
 * في الخادم عند نداء الـ API — إخفاء الزرّ في التطبيق ليس أمناً، والاختبار
 * الذي يستدعي الخدمة مباشرةً لا يثبت شيئاً عن الحارس.
 */

use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Str;

$line = fn($s) => print($s . PHP_EOL);
$ok = 0; $fail = 0;
$check = function (string $name, bool $pass, string $detail = '') use (&$ok, &$fail, $line) {
    if ($pass) { $ok++;   $line("  PASS  $name" . ($detail ? "  ($detail)" : '')); }
    else       { $fail++; $line("  FAIL  $name" . ($detail ? "  ($detail)" : '')); }
};

$base    = 'http://127.0.0.1:8000/api';
$agentId = 104;
$phone   = '911234588';

/** نداء HTTP بسيط — بلا حزم إضافية. */
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
    $raw  = curl_exec($ch);
    $code = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    return ['status' => $code, 'body' => json_decode($raw, true)];
};

$line('=== employee permissions — acceptance (HTTP) ===');

$snapshot = fn() => [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'ledger'   => DB::table('ExchangeAccData')->count(),
    'internal' => DB::table('InternalEx')->count(),
    'safe'     => DB::table('EX24AccSafeActivityTb')->count(),
];
$before = $snapshot();

/* تنظيف. */
$stale = DB::table('employees')->where('phone', $phone)->pluck('id');
if ($stale->isNotEmpty()) {
    foreach (['employee_shift_closings','employee_cashbox_entries','employee_shifts',
              'employee_cashboxes','employee_sessions','employee_devices','employee_otps',
              'employee_activation_codes','employee_permissions','employee_point_of_sales'] as $t) {
        DB::table($t)->whereIn('employee_id', $stale)->delete();
    }
    DB::table('employees')->whereIn('id', $stale)->delete();
}

/* موظف نشط بجهاز وجلسة — نُنشئها مباشرةً لأن مسار التفعيل مُختبَر في ملفّه. */
$employeeId = DB::table('employees')->insertGetId([
    'agent_id' => $agentId, 'full_name' => 'موظف صلاحيات', 'phone' => $phone,
    'status' => 'ACTIVE', 'created_at' => now(), 'updated_at' => now(),
]);
$deviceHash = hash('sha256', 'PERM-TEST-DEVICE');
$deviceId = DB::table('employee_devices')->insertGetId([
    'agent_id' => $agentId, 'employee_id' => $employeeId, 'device_hash' => $deviceHash,
    'status' => 'ACTIVE', 'activated_at' => now(),
]);
$token = Str::random(64);
$sessionId = DB::table('employee_sessions')->insertGetId([
    'agent_id' => $agentId, 'employee_id' => $employeeId, 'device_id' => $deviceId,
    'device_hash' => $deviceHash, 'access_token_hash' => hash('sha256', $token),
    'status' => 'ACTIVE', 'created_at' => now(),
]);

$grant = function (array $keys) use ($employeeId, $agentId) {
    DB::table('employee_permissions')->where('employee_id', $employeeId)->delete();
    foreach ($keys as $k) {
        DB::table('employee_permissions')->insert([
            'employee_id' => $employeeId, 'permission_key' => $k,
            'granted_by' => $agentId, 'granted_at' => now(),
        ]);
    }
};

/* ---------- 29ب. الملف الشخصي متاح بلا صلاحية خاصة ---------- */
$grant([]);
$me = $call('GET', '/device/employee/me', $token);
$check('0. الملف الشخصي يعمل بلا صلاحيات', $me['status'] === 200);
$check('0ب. ويعيد قائمة صلاحيات فارغة',
    ($me['body']['data']['permissions'] ?? null) === []);

/* ---------- 15/16. الموظف يرى ما مُنح فقط ---------- */
$r = $call('GET', '/device/employee/transfers/incoming', $token);
$check('15. بلا VIEW_INCOMING_TRANSFERS ⇦ 403', $r['status'] === 403,
    $r['body']['message'] ?? '');

$grant(['VIEW_INCOMING_TRANSFERS']);
$r = $call('GET', '/device/employee/transfers/incoming', $token);
$check('15ب. بعد المنح ⇦ 200', $r['status'] === 200);

$r = $call('POST', '/device/employee/transfers/1/deliver', $token);
$check('16. من يملك العرض فقط لا يستطيع التسليم ⇦ 403', $r['status'] === 403);

/* ---------- 18. نداء API مباشرةً بلا صلاحية يُرفض ---------- */
$r = $call('GET', '/device/employee/cashbox', $token);
$check('18. الخزينة بلا VIEW_OWN_CASHBOX ⇦ 403', $r['status'] === 403);

/* ---------- 19/30. مفتاح غير موجود في الكتالوج لا يُمنح ---------- */
DB::table('employee_permissions')->insert([
    'employee_id' => $employeeId, 'permission_key' => 'FUTURE_FEATURE_X',
    'granted_by' => $agentId, 'granted_at' => now(),
]);
$r = $call('GET', '/device/employee/cashbox', $token);
$check('19/30. صفٌّ لمفتاح ليس في الكتالوج لا يفتح شيئاً ⇦ 403', $r['status'] === 403);

/* ---------- 21. سحب الصلاحية يسري فوراً ---------- */
$grant(['VIEW_OWN_CASHBOX', 'START_SHIFT', 'CASHBOX_ENTRY', 'CLOSE_SHIFT']);
$r = $call('GET', '/device/employee/cashbox', $token);
$check('20. بعد المنح ⇦ 200', $r['status'] === 200);

DB::table('employee_permissions')->where('employee_id', $employeeId)
    ->where('permission_key', 'VIEW_OWN_CASHBOX')->delete();
$r = $call('GET', '/device/employee/cashbox', $token);
$check('21. سحب الصلاحية أثناء العمل يسري في الطلب التالي ⇦ 403',
    $r['status'] === 403);

/* ---------- الخزينة عبر HTTP: وردية ⇦ حركة ⇦ إقفال ---------- */
$grant(['VIEW_OWN_CASHBOX', 'START_SHIFT', 'CASHBOX_ENTRY', 'CLOSE_SHIFT']);

$r = $call('POST', '/device/employee/shift/start', $token, ['opening_cash' => 300]);
$check('26. بدء الوردية عبر API', $r['status'] === 200, $r['body']['message'] ?? '');

$r = $call('POST', '/device/employee/cashbox/entry', $token,
    ['amount' => 100, 'direction' => 'IN', 'client_ref' => 'HTTP-1']);
$check('26ب. حركة وارد', $r['status'] === 200);

// 28: تكرار الطلب نفسه
$r2 = $call('POST', '/device/employee/cashbox/entry', $token,
    ['amount' => 100, 'direction' => 'IN', 'client_ref' => 'HTTP-1']);
$check('28. تكرار الطلب لا يُنشئ حركة ثانية',
    ($r2['body']['data']['duplicate'] ?? false) === true);

$r = $call('GET', '/device/employee/cashbox', $token);
$expected = $r['body']['data']['summary']['expected'] ?? null;
$check('26ج. المتوقّع = 300 + 100 = 400', abs(((float) $expected) - 400) < 0.001,
    "expected=$expected");

$r = $call('POST', '/device/employee/shift/close', $token, ['actual_cash' => 380]);
$check('27. الإقفال يحسب عجزاً 20',
    ($r['body']['data']['result'] ?? '') === 'SHORTAGE'
    && abs(((float) ($r['body']['data']['difference'] ?? 0)) + 20) < 0.001,
    $r['body']['message'] ?? '');

/* ---------- 29. إيقاف الموظف يُبطل الجلسة فوراً ---------- */
DB::table('employees')->where('id', $employeeId)->update(['status' => 'SUSPENDED']);
$r = $call('GET', '/device/employee/me', $token);
$check('29. إيقاف الموظف يُبطل جلسته فوراً ⇦ 401', $r['status'] === 401);
$check('29ب. وتُوسم الجلسة ملغاة في القاعدة',
    DB::table('employee_sessions')->where('id', $sessionId)->value('status') === 'REVOKED');

/* ---------- 30. رمز غير صالح ---------- */
$r = $call('GET', '/device/employee/me', 'not-a-real-token');
$check('30. رمز غير صالح ⇦ 401', $r['status'] === 401);

/* ---------- السجلّ الأمني ---------- */
$check('32. محاولات الوصول بلا صلاحية مُسجَّلة أمنياً',
    DB::table('security_logs')->where('event_type', 'UNAUTHORIZED')
        ->where('employee_id', $employeeId)->count() >= 3);

/* ---------- تنظيف ---------- */
foreach (['employee_shift_closings','employee_cashbox_entries','employee_shifts',
          'employee_cashboxes','employee_sessions','employee_devices',
          'employee_permissions'] as $t) {
    DB::table($t)->where('employee_id', $employeeId)->delete();
}
DB::table('audit_logs')->where('employee_id', $employeeId)->delete();
DB::table('security_logs')->where('employee_id', $employeeId)->delete();
DB::table('employees')->where('id', $employeeId)->delete();

/* ---------- البصمة المالية ---------- */
$after = $snapshot();
$check('★ لا مساس بالمالية القائمة', $before == $after,
    json_encode(['before' => $before, 'after' => $after]));

$line('');
$line("=== PASS: $ok   FAIL: $fail ===");
