<?php

use App\Services\Employees\EmployeeActivationService;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Http;

/*
 * فحصان لا يُرسلان رسالةً واحدة إلى أحد:
 *
 *   ١) الوجهة: إلى أي رقمٍ يذهب رمزُ الموظّف فعلاً؟
 *   ٢) الأثر : هل يُسجَّل فشلُ الإرسال أم يبقى صامتاً؟
 *
 * ⚠ وبوّابةٌ مزيّفةٌ **واحدة** لا اثنتان: `Http::fake` تُراكِم المُزيَّفات
 * ولا تستبدلها، فأوّلُ ما يطابق هو ما يردّ. ونداءٌ ثانٍ لها بردٍّ فاشلٍ لا
 * يفعل شيئاً ما دام الأوّل ما زال يطابق — وهو ما أوهمني أوّل مرّة أن
 * التسجيل لا يعمل، والعيبُ كان في الفحص لا في الشيفرة.
 */

$svc = app(EmployeeActivationService::class);
$ref = new ReflectionClass($svc);
$send = $ref->getMethod('sendOtp');
$send->setAccessible(true);

$emp = DB::table('employees')->orderByDesc('id')->first(['id', 'phone', 'full_name']);
$phone = $emp->phone;

echo "الموظّف: {$emp->full_name}  ·  هاتفه: {$phone}" . PHP_EOL . PHP_EOL;

$mode = 'ok';
$sentTo = null;

Http::fake(function ($request) use (&$mode, &$sentTo) {
    $sentTo = $request->data()['chatId'] ?? null;

    return $mode === 'ok'
        ? Http::response(['ok' => true], 200)
        : Http::response(['error' => 'number not on whatsapp'], 422);
});

/* ── ١) الوجهة ────────────────────────────────────────────── */
$send->invoke($svc, $phone, '1234');

echo '① الوجهة الفعلية : ' . var_export($sentTo, true) . PHP_EOL;
echo '   المتوقَّع      : 218' . $phone . PHP_EOL;
echo '   ⇐ ' . ($sentTo === '218' . $phone ? '✓ إلى الموظّف' : '✗ إلى غيره') . PHP_EOL;
echo '   ولا رقمَ الوكيل : '
   . (str_contains((string) $sentTo, '925093709') ? '✗ ما زال!' : '✓') . PHP_EOL;

echo PHP_EOL;

/* ── ٢) هل يُسجَّل الفشل؟ ──────────────────────────────────── */
$mode = 'fail';
$before = (int) DB::table('security_logs')->where('event_type', 'OTP_SEND_FAILED')->count();
$send->invoke($svc, $phone, '5678');
$after = (int) DB::table('security_logs')->where('event_type', 'OTP_SEND_FAILED')->count();

echo '② قيود الفشل قبل/بعد : ' . $before . ' ⇐ ' . $after . PHP_EOL;
echo '   ⇐ ' . ($after > $before ? '✓ الفشل صار مرئياً' : '✗ ما زال صامتاً') . PHP_EOL;

$row = DB::table('security_logs')->where('event_type', 'OTP_SEND_FAILED')
    ->orderByDesc('id')->first(['detail', 'phone']);
if ($row) {
    echo '   السجلّ : ' . $row->detail . PHP_EOL;
    echo '   الرقم  : ' . $row->phone . PHP_EOL;
}

$leak = DB::table('security_logs')
    ->where(fn ($w) => $w->where('detail', 'like', '%5678%')
                         ->orWhere('detail', 'like', '%1234%'))->count();
echo '   تسريبُ الرمز : ' . ($leak === 0 ? '✓ لا شيء' : '✗ ' . $leak . ' قيد') . PHP_EOL;

/* تنظيف قيود الفحص وحدها. */
$n = DB::table('security_logs')->where('event_type', 'OTP_SEND_FAILED')
    ->where('created_at', '>=', now()->subMinutes(2))->delete();
echo PHP_EOL . "نُظِّف: {$n} قيداً من قيود الفحص." . PHP_EOL;
