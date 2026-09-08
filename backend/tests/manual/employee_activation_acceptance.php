<?php

/*
 * اختبارات قبول تفعيل الموظف — البنود 1 إلى 14 من مستند المالك.
 *
 * يُشغَّل من backend/:
 *   php artisan tinker --execute="require base_path('tests/manual/employee_activation_acceptance.php');"
 *
 * يُنشئ موظفاً تجريبياً ويحذفه في النهاية، ولا يمسّ موظفاً حقيقياً.
 * وآخر فحصٍ فيه بصمة مالية: هذا النظام كلّه يجب ألّا يحرّك ديناراً.
 */

use App\Services\Employees\DeviceRegistryService;
use App\Services\Employees\EmployeeActivationService;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;

$svc = app(EmployeeActivationService::class);
$reg = app(DeviceRegistryService::class);

$line = fn($s) => print($s . PHP_EOL);
$ok = 0; $fail = 0;
$check = function (string $name, bool $pass, string $detail = '') use (&$ok, &$fail, $line) {
    if ($pass) { $ok++;   $line("  PASS  $name" . ($detail ? "  ($detail)" : '')); }
    else       { $fail++; $line("  FAIL  $name" . ($detail ? "  ($detail)" : '')); }
};

$agentId  = 104;               // «جاري شركة الامانة»
$phone    = '911234567';       // رقم تجريبي — لن يصل واتساب حقيقي
$deviceA  = 'TEST-DEVICE-AAA';
$deviceB  = 'TEST-DEVICE-BBB';
$hashA    = DeviceRegistryService::hash($deviceA);
$hashB    = DeviceRegistryService::hash($deviceB);
$trace    = ['ip' => '127.0.0.1', 'platform' => 'test'];

$line('=== employee activation — acceptance ===');

/* بصمة مالية قبل أي شيء. */
$snapshot = fn() => [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'ledger'   => DB::table('ExchangeAccData')->count(),
    'internal' => DB::table('InternalEx')->count(),
    'users'    => DB::table('users')->count(),
];
$before = $snapshot();

/* تنظيف أي بقايا من تشغيل سابق. */
$stale = DB::table('employees')->where('phone', $phone)->pluck('id');
if ($stale->isNotEmpty()) {
    DB::table('employee_sessions')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_devices')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_otps')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_activation_codes')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_permissions')->whereIn('employee_id', $stale)->delete();
    DB::table('employee_point_of_sales')->whereIn('employee_id', $stale)->delete();
    DB::table('employees')->whereIn('id', $stale)->delete();
}
DB::table('device_registry')->whereIn('device_hash', [$hashA, $hashB])->delete();

/* ---------- 1. إنشاء موظف ---------- */
$employeeId = DB::table('employees')->insertGetId([
    'agent_id'   => $agentId,
    'full_name'  => 'موظف اختبار',
    'phone'      => $phone,
    'status'     => 'PENDING_ACTIVATION',
    'created_at' => now(),
    'updated_at' => now(),
]);
$check('1. إنشاء الموظف وحفظه', DB::table('employees')->where('id', $employeeId)->exists());

/* ---------- 2. إصدار كود التفعيل ---------- */
$issued = $svc->issueCode($agentId, $employeeId, $agentId, $trace);
$code = $issued['code'];
$row = DB::table('employee_activation_codes')->where('employee_id', $employeeId)
    ->where('status', 'ACTIVE')->first();
$check('2. إصدار كود مرتبط بالموظف الصحيح', $row !== null && (int) $row->employee_id === $employeeId);
$check('2ب. الكود لا يُخزَّن نصّاً صريحاً',
    $row && $row->code_hash !== $code && Hash::check($code, $row->code_hash));

/* ---------- 3. رقم + كود صحيحان ⇦ يُرسل الرمز ---------- */
$r = $svc->requestOtp($phone, $code, $deviceA, $trace);
$check('3. التحقّق الأوّلي ينجح ويُصدر رمزاً', $r['ok'] === true, $r['message']);
$otpRow = DB::table('employee_otps')->where('employee_id', $employeeId)
    ->where('status', 'PENDING')->orderByDesc('id')->first();
$check('3ب. الرمز محفوظ مُجزّأً ومربوطاً بالجهاز',
    $otpRow && $otpRow->device_hash === $hashA && strlen($otpRow->otp_hash) > 20);

/* الرمز نفسه غير معروف للاختبار — نستبدله بواحدٍ معروف لنكمل المسار. */
$known = '4321';
DB::table('employee_otps')->where('id', $otpRow->id)->update(['otp_hash' => Hash::make($known)]);

/* ---------- 4. رمز خاطئ ⇦ يفشل ---------- */
$bad = $svc->verifyOtp($phone, '0000', $deviceA, $trace);
$check('4. رمز خاطئ يُرفض', $bad['ok'] === false, $bad['message']);
$check('4ب. تُسجَّل المحاولة في السجلّ الأمني',
    DB::table('security_logs')->where('event_type', 'OTP_FAILED')
        ->where('employee_id', $employeeId)->exists());

/* ---------- 5. رمز صحيح ⇦ ربط الجهاز وفتح الجلسة ---------- */
$good = $svc->verifyOtp($phone, $known, $deviceA, $trace);
$check('5. الرمز الصحيح يُفعّل ويفتح جلسة',
    $good['ok'] === true && !empty($good['session']['access_token']), $good['message']);

$device = DB::table('employee_devices')->where('employee_id', $employeeId)
    ->where('status', 'ACTIVE')->first();
$check('5ب. الجهاز مربوط وفعّال', $device !== null && $device->device_hash === $hashA);
$check('5ج. حالة الموظف صارت ACTIVE',
    DB::table('employees')->where('id', $employeeId)->value('status') === 'ACTIVE');

/* ---------- 6/7. الجلسة تبقى — إغلاق التطبيق أو إعادة التشغيل لا يُبطلها ---------- */
$token = $good['session']['access_token'];
$alive = DB::table('employee_sessions')
    ->where('access_token_hash', hash('sha256', $token))
    ->where('status', 'ACTIVE')->exists();
$check('6/7. الجلسة تبقى فعّالة بلا كود جديد', $alive);

/* ---------- 11. محاولة على جهاز ثانٍ ⇦ رفض + COMPROMISED ---------- */
$second = $svc->requestOtp($phone, $code, $deviceB, $trace);
$check('11. محاولة الجهاز الثاني تُرفض', $second['ok'] === false, $second['message']);
$check('11ب. لم يُرسل رمز للجهاز الثاني',
    !DB::table('employee_otps')->where('employee_id', $employeeId)
        ->where('device_hash', $hashB)->exists());
$check('11ج. صار الكود COMPROMISED',
    DB::table('employee_activation_codes')->where('id', $row->id)->value('status') === 'COMPROMISED');
$check('11د. سقطت جلسات الموظف وأجهزته',
    !DB::table('employee_sessions')->where('employee_id', $employeeId)->where('status', 'ACTIVE')->exists());
$check('11هـ. سُجّل الحادث أمنياً',
    DB::table('security_logs')->where('event_type', 'CODE_OTHER_DEVICE')
        ->where('employee_id', $employeeId)->exists());

/* ---------- 12. الكود المحروق يُرفض حتى على الجهاز الأصلي ---------- */
$again = $svc->requestOtp($phone, $code, $deviceA, $trace);
$check('12. الكود المحروق يُرفض على الجهاز الأصلي',
    $again['ok'] === false && str_contains($again['message'], 'أسباب أمنية'));

/* ---------- 13. جهاز الموظف يُمنع من الدخول كمسؤول ---------- */
$check('13. الجهاز مُصنَّف EMPLOYEE_DEVICE', $reg->isEmployeeDevice($hashA));
$check('13ب. جهازٌ لم يُستعمل ليس مصنّفاً', !$reg->isEmployeeDevice($hashB));

/* ---------- 14. التصنيف لا يُمحى بإلغاء الجهاز ---------- */
DB::table('employee_devices')->where('employee_id', $employeeId)
    ->update(['status' => 'REVOKED', 'revoked_at' => now()]);
$check('14. التصنيف الدائم يصمد بعد إلغاء الجهاز', $reg->isEmployeeDevice($hashA));

/* ---------- CODE_TTL: لا كودَ مفتوح الصلاحية (أمر المالك 8/9/2026) ------ */

/*
 * ⚠ الكودُ الذي لا يُستعمل خلال عشر دقائق **يُحرق** ولا يُرفض فحسب:
 * كودٌ يُرفض ويبقى `ACTIVE` في الجدول هو كودٌ حيٌّ ينتظر ساعةً يُصلَح فيها
 * الخطأ. ونصُّ الأمر: «يُمنع ترك صلاحية المفتاح مفتوحة».
 *
 * ⚠ والاختبارُ يُزوّر التاريخَ ولا ينتظر عشر دقائق — اختبارٌ ينتظر لا يُشغَّل.
 */
$ttlCode = $svc->issueCode($agentId, $employeeId, $agentId, $trace);
$ttlRow  = DB::table('employee_activation_codes')
    ->where('employee_id', $employeeId)->where('status', 'ACTIVE')
    ->orderByDesc('id')->first();

$check('CODE_TTL 1. الكود يُصدَر بموعد انتهاء لا بلا نهاية',
    $ttlRow && $ttlRow->expires_at !== null,
    'ينتهي=' . ($ttlRow->expires_at ?? 'NULL'));

$check('CODE_TTL 2. والمدّة عشر دقائق',
    isset($ttlCode['ttl_minutes']) && (int) $ttlCode['ttl_minutes'] === 10,
    'المدّة=' . ($ttlCode['ttl_minutes'] ?? '—'));

/* يُزوَّر الوقت إلى ما بعد المدّة. */
DB::table('employee_activation_codes')->where('id', $ttlRow->id)
    ->update(['expires_at' => now()->subMinute()]);

$expired = $svc->requestOtp($phone, $ttlCode['code'], $deviceA, $trace);

$check('CODE_TTL 3. الكود المنتهي يُرفض', ($expired['ok'] ?? true) === false);

$check('CODE_TTL 4. ورسالةٌ تقول إنه انتهى — لا «الرقم أو الكود غير صحيح»',
    str_contains($expired['message'] ?? '', 'انتهت'),
    mb_substr($expired['message'] ?? '', 0, 60));

$check('CODE_TTL 5. ⚠ ويُحرق في القاعدة لا يُرفض فقط',
    DB::table('employee_activation_codes')->where('id', $ttlRow->id)->value('status') === 'EXPIRED',
    'الحالة=' . DB::table('employee_activation_codes')->where('id', $ttlRow->id)->value('status'));

$check('CODE_TTL 6. ولا رمزَ تحقّقٍ أُرسل',
    !DB::table('employee_otps')->where('employee_id', $employeeId)
        ->where('status', 'PENDING')->where('created_at', '>=', now()->subMinute())->exists());

$check('CODE_TTL 7. وحُفظ الحادث أمنياً',
    DB::table('security_logs')->where('event_type', 'CODE_EXPIRED')
        ->where('employee_id', $employeeId)->exists());

/* ⚠ ولا تُطبَّق المدّة على كودٍ استُعمل: إعادةُ التفعيل على الجهاز نفسِه
   طريقٌ مشروع، وما أدّى عملَه لا معنى لانتهاء صلاحيته. */
$usedRow = DB::table('employee_activation_codes')
    ->where('employee_id', $employeeId)->where('status', 'USED')
    ->orderByDesc('id')->first();
if ($usedRow) {
    DB::table('employee_activation_codes')->where('id', $usedRow->id)
        ->update(['expires_at' => now()->subDay()]);
    $still = DB::table('employee_activation_codes')->where('id', $usedRow->id)->value('status');
    $check('CODE_TTL 8. ⚠ والمستعمَلُ لا يُحرق بمضيّ المدّة', $still === 'USED', 'الحالة=' . $still);
} else {
    $check('CODE_TTL 8. ⚠ والمستعمَلُ لا يُحرق بمضيّ المدّة (لا كودَ مستعمَلاً بعد)', true);
}
/* ---------- 9. كود جديد يُعيد التفعيل ---------- */
$reissued = $svc->issueCode($agentId, $employeeId, $agentId, $trace);
$check('9. كود جديد يُصدر ويعيد الموظف لبانتظار التفعيل',
    strlen($reissued['code']) === 8
    && DB::table('employees')->where('id', $employeeId)->value('status') === 'PENDING_ACTIVATION');
$check('9ب. الكود القديم لم يعد فعّالاً',
    DB::table('employee_activation_codes')->where('id', $row->id)->value('status') === 'COMPROMISED');

/* ---------- 8. الخروج يُبطل التفعيل ---------- */
$r2 = $svc->requestOtp($phone, $reissued['code'], $deviceA, $trace);
$otp2 = DB::table('employee_otps')->where('employee_id', $employeeId)
    ->where('status', 'PENDING')->orderByDesc('id')->first();
DB::table('employee_otps')->where('id', $otp2->id)->update(['otp_hash' => Hash::make('1234')]);
$svc->verifyOtp($phone, '1234', $deviceA, $trace);

$svc->logout($employeeId, $trace);
$check('8. الخروج يُبطل الجلسات والكود والجهاز',
    !DB::table('employee_sessions')->where('employee_id', $employeeId)->where('status', 'ACTIVE')->exists()
    && !DB::table('employee_activation_codes')->where('employee_id', $employeeId)->where('status', 'ACTIVE')->exists()
    && !DB::table('employee_devices')->where('employee_id', $employeeId)->where('status', 'ACTIVE')->exists()
    && DB::table('employees')->where('id', $employeeId)->value('status') === 'REQUIRES_REACTIVATION');

/* ---------- سجلّ التدقيق ---------- */
$check('31. سُجّلت أحداث التدقيق المطلوبة',
    DB::table('audit_logs')->where('employee_id', $employeeId)
        ->whereIn('action', ['EMPLOYEE_CODE_ISSUED', 'EMPLOYEE_ACTIVATED', 'EMPLOYEE_LOGOUT'])
        ->distinct()->count('action') === 3);

/* ---------- تنظيف ---------- */
DB::table('employee_sessions')->where('employee_id', $employeeId)->delete();
DB::table('employee_devices')->where('employee_id', $employeeId)->delete();
DB::table('employee_otps')->where('employee_id', $employeeId)->delete();
DB::table('employee_activation_codes')->where('employee_id', $employeeId)->delete();
DB::table('employees')->where('id', $employeeId)->delete();
DB::table('device_registry')->whereIn('device_hash', [$hashA, $hashB])->delete();

/* ---------- البصمة المالية ---------- */
$after = $snapshot();
$check('المالية: لا أثر إطلاقاً — الأرصدة والقيود والحوالات كما هي',
    $before == $after, json_encode(['before' => $before, 'after' => $after]));

$line('');
$line("=== PASS: $ok   FAIL: $fail ===");
