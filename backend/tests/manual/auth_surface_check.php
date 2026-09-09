<?php

/*
 * ════════════════════════════════════════════════════════════════════════════
 *  سطحُ المصادقة: ما هو مفتوحٌ للعموم، وما يجب أن يكون مغلقاً
 * ════════════════════════════════════════════════════════════════════════════
 *
 *   php artisan tinker --execute="require base_path('tests/manual/auth_surface_check.php');"
 *
 * ويحتاج خادماً على 127.0.0.1:8000.
 *
 * ⚠ **سببُ وجوده**: `device/update/password` كانت مفتوحةً بلا مصادقة — يكفي
 * معرفةُ رقم الهاتف ومعرّف الجهاز لتعيين كلمةِ مرور، ثمّ الدخولُ بها من
 * `device/login`. أي استيلاءٌ كامل على حسابٍ ماليّ بلا رمزِ تحقّقٍ ولا جلسة.
 * وكان التوثيقُ يسمّيها ثغرةً منذ إضافة `otp/login` والمسارُ مفتوح.
 *
 * فهذا الفحصُ يُبقيها مغلقة، ويحرس ما يجب أن يبقى مفتوحاً من أن يُغلق سهواً.
 */

use Illuminate\Support\Facades\DB;

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $skip = 0;
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, $line) {
    if ($p) { $ok++;   $line('  PASS  ' . $n . ($d ? "  ($d)" : '')); }
    else    { $fail++; $line('  FAIL  ' . $n . ($d ? "  ($d)" : '')); }
};

$line('════════════════════════════════════════════════════════════');
$line(' سطح المصادقة — ما يُفتح وما يُغلق');
$line('════════════════════════════════════════════════════════════');

$before = [
    'wallet' => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'users'  => DB::table('users')->count(),
];

$base = 'http://127.0.0.1:8000/api';
$sock = @fsockopen('127.0.0.1', 8000, $e1, $e2, 1);
if ($sock === false) {
    $line('  SKIP  كل الفحوص  (لا خادم على 127.0.0.1:8000)');
    return;
}
fclose($sock);

$call = function (string $path, ?string $token, array $body) use ($base) {
    $ch = curl_init($base . $path);
    $h = ['Accept: application/json', 'Content-Type: application/json'];
    if ($token !== null) { $h[] = 'Authorization: Bearer ' . $token; }
    curl_setopt_array($ch, [
        CURLOPT_POST => true,
        CURLOPT_POSTFIELDS => json_encode($body, JSON_UNESCAPED_UNICODE),
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_TIMEOUT => 25,
        CURLOPT_HTTPHEADER => $h,
    ]);
    $raw = curl_exec($ch);
    $st  = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);
    return ['status' => $st, 'json' => json_decode((string) $raw, true) ?: []];
};

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ١) تغييرُ كلمة المرور مغلقٌ بلا جلسة ─────────────────────');

/* هدفٌ حقيقيّ: مستخدمٌ قائم — ولا تُغيَّر كلمتُه، إنما تُرفَض المحاولة. */
$victim = DB::table('users')->whereNull('deleted_at')
    ->whereNotNull('phone')->where('phone', 'like', '9%')
    ->orderBy('id')->first(['id', 'phone', 'device_id', 'password']);

if (!$victim) {
    $line('  SKIP  لا مستخدم صالحٌ للفحص');
} else {
    $r = $call('/device/update/password', null, [
        'phone'     => $victim->phone,
        'password'  => 'Attacker@2026x',
        'device_id' => (string) ($victim->device_id ?? 'x'),
    ]);
    $check('1. ⚠ بلا رمزِ جلسةٍ تُرفض', $r['status'] === 401,
        'HTTP ' . $r['status']);

    $r = $call('/device/update/password', 'رمز-مزيّف', [
        'phone'     => $victim->phone,
        'password'  => 'Attacker@2026x',
        'device_id' => (string) ($victim->device_id ?? 'x'),
    ]);
    $check('2. ⚠ وبرمزٍ مزيَّف تُرفض', $r['status'] === 401,
        'HTTP ' . $r['status']);

    $after = DB::table('users')->where('id', $victim->id)->value('password');
    $check('3. ⚠ وكلمةُ المرور لم تتغيّر', (string) $after === (string) $victim->password);
}

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٢) ولا يغيّرها الداخلُ لغيره ─────────────────────────────');

$me = DB::table('users')->whereNull('deleted_at')->where('id', 104)->first();
$other = DB::table('users')->whereNull('deleted_at')
    ->where('id', '<>', 104)->whereNotNull('phone')
    ->where('phone', 'like', '9%')->orderBy('id')->first(['id', 'phone', 'password']);

if (!$me || !$other) {
    $line('  SKIP  4–5. يحتاج حسابين');
} else {
    $token = \App\Models\User::find($me->id)
        ->createToken('auth-surface-check')->plainTextToken;

    $r = $call('/device/update/password', $token, [
        'phone'     => $other->phone,
        'password'  => 'Attacker@2026x',
        'device_id' => 'anything',
    ]);
    $check('4. ⚠ وكيلٌ داخلٌ لا يغيّر كلمةَ وكيلٍ آخر', $r['status'] === 403,
        'HTTP ' . $r['status']);

    $after = DB::table('users')->where('id', $other->id)->value('password');
    $check('5. ⚠ وكلمتُه لم تتغيّر',
        (string) $after === (string) $other->password);

    DB::table('personal_access_tokens')
        ->where('name', 'auth-surface-check')->delete();
}

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٣) وما يجب أن يبقى مفتوحاً بقي ──────────────────────────');

/*
 * ⚠ هذه المساراتُ مفتوحةٌ **بالضرورة**: لا جلسةَ قبل الدخول. وحراستُها
 * بحدّ المعدّل وبالتحقّق من الرمز لا بالمصادقة.
 *
 * ويُفحص أنها لا تردّ 401: إغلاقُها سهواً يمنع الدخولَ كلَّه.
 */
foreach ([
    '/device/otp/send'                    => ['phone' => '900000000'],
    '/device/employee/activation/request' => ['phone' => '900000000', 'code' => 'XXXXXXXX'],
] as $path => $body) {
    $r = $call($path, null, $body);
    $check('6. مفتوحٌ كما يجب: ' . $path, $r['status'] !== 401,
        'HTTP ' . $r['status']);
}

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٤) البصمة المالية ────────────────────────────────────────');
$after = [
    'wallet' => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'users'  => DB::table('users')->count(),
];
foreach ($before as $k => $v) {
    $check('7. ' . $k . ' لم يتغيّر', (string) $after[$k] === (string) $v,
        'قبل=' . $v . ' بعد=' . $after[$k]);
}

$line();
$line('════════════════════════════════════════════════════════════');
$line('  نجح: ' . $ok . '   أخفق: ' . $fail . '   تُخطّي: ' . $skip);
$line('════════════════════════════════════════════════════════════');
