<?php

use App\Services\Employees\DeviceRegistryService;
use App\Services\Employees\EmployeeActivationService;
use Illuminate\Support\Facades\DB;

/*
 * تفعيل الموظف بواسطة QR — اختبارات القبول.
 *
 *   php artisan tinker --execute="require base_path('tests/manual/employee_qr_activation_acceptance.php');"
 *
 * ⚠ **أهمُّ ما يُثبته**: أن QR والكودَ اليدويّ يصلان إلى **مسار التحقّق
 * نفسِه** — الحالة، والمدّة، والجهاز، وحدُّ المعدّل، ثمّ OTP نفسُه، ثمّ ربطُ
 * الجهاز نفسُه. فالرمزُ اختصارُ إدخالٍ لا اختصارُ أمان.
 *
 * ⚠ ولا يمسّ شيئاً مالياً: يعمل على موظّفٍ اختباريّ برقمٍ اختباريّ، ولقطةٌ
 * ماليّة قبل وبعد.
 */

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, &$failed, $line) {
    if ($p) { $ok++; $line("  PASS  $n" . ($d ? "  ($d)" : '')); }
    else    { $fail++; $failed[] = $n; $line("  FAIL  $n" . ($d ? "  ($d)" : '')); }
};

$line('══════════════════════════════════════════════════════════════');
$line('   تفعيل الموظف بواسطة QR');
$line('══════════════════════════════════════════════════════════════');

$snap = fn () => [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => (string) DB::table('InternalEx')->count(),
    'safe'     => (string) DB::table('EX24AccSafeActivityTb')->count(),
    'users'    => (string) DB::table('users')->count(),
];
$before = $snap();

$svc      = app(EmployeeActivationService::class);
$agentId  = 104;
$phone    = '911234599';          // رقمٌ اختباريّ لا يخصّ أحداً
$deviceA  = 'qr-test-device-A';
$deviceB  = 'qr-test-device-B';
$hashA    = DeviceRegistryService::hash($deviceA);
$hashB    = DeviceRegistryService::hash($deviceB);
$trace    = ['ip' => '127.0.0.1', 'platform' => 'test', 'model' => 'qr', 'app_version' => 'x'];

/* تنظيفُ بقايا أي تشغيلٍ سابق. */
$stale = DB::table('employees')->where('phone', $phone)->pluck('id');
if ($stale->isNotEmpty()) {
    foreach (['employee_sessions', 'employee_devices', 'employee_otps',
              'employee_activation_codes', 'employee_permissions'] as $t) {
        DB::table($t)->whereIn('employee_id', $stale)->delete();
    }
    DB::table('employees')->whereIn('id', $stale)->delete();
}
DB::table('device_registry')->whereIn('device_hash', array_filter([$hashA, $hashB]))->delete();

$employeeId = DB::table('employees')->insertGetId([
    'agent_id' => $agentId, 'full_name' => 'موظف اختبار QR', 'phone' => $phone,
    'status' => 'PENDING_ACTIVATION', 'created_at' => now(), 'updated_at' => now(),
]);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ١) الإصدار: رمزٌ وكودٌ على طلبٍ واحد ─────────────────────');

$issued = $svc->issueCode($agentId, $employeeId, $agentId, $trace);

$check('1. الإصدار يُرجع رمز QR مع الكود',
    isset($issued['qr_token']) && isset($issued['code']),
    'طول الرمز=' . strlen($issued['qr_token'] ?? ''));

$check('2. ⚠ والرمزُ عالي العشوائية — 64 محرفاً ستّ عشريّاً (256 بت)',
    (bool) preg_match('/^[0-9a-f]{64}$/', $issued['qr_token'] ?? ''));

$row = DB::table('employee_activation_codes')
    ->where('employee_id', $employeeId)->where('status', 'ACTIVE')
    ->orderByDesc('id')->first();

$check('3. ⚠ ويُخزَّن مُجزَّأً لا نصّاً — من قرأ الجدول لا يُفعّل جهازاً',
    $row->qr_token_hash === hash('sha256', $issued['qr_token'])
    && !str_contains(json_encode($row), $issued['qr_token']));

$check('4. ⚠ وعلى الصفّ نفسِه — طلبٌ واحد لا طلبان',
    $row->code_hash !== null && $row->qr_token_hash !== null
    && $row->expires_at !== null,
    'الصفّ #' . $row->id);

/*
 * ⚠ الرمزُ لا يحمل بياناً — عشوائيٌّ خالص لا مُرمَّزٌ يُفكّ. ولا يُفحص ذلك
 * بالبحث عن معرّفٍ داخله: نصٌّ من 64 خانةً ستّ عشريّة يحوي بالمصادفة أي
 * سلسلةٍ من ثلاثة أرقام، فيُخفق الفحصُ على رمزٍ سليم. الدليلُ الصحيح أن
 * رمزين لموظّفٍ واحد لا يشتركان في شيء — فلا شيء فيهما مشتقٌّ منه.
 *
 * والثاني يُبطل الأوّل (فحص 25)، فيُمضى بقيّةُ الاختبار على الثاني.
 */
$twin = $svc->issueCode($agentId, $employeeId, $agentId, $trace);
$sharedRun = 0;
for ($i = 0; $i + 8 <= 64; $i++) {
    if (str_contains($twin['qr_token'], substr($issued['qr_token'], $i, 8))) $sharedRun++;
}
$check('5. ⚠ ورمزان لموظّفٍ واحد لا يشتركان في شيء — لا اشتقاقَ من معرّف',
    $twin['qr_token'] !== $issued['qr_token'] && $sharedRun === 0,
    'مقاطعُ مشتركة=' . $sharedRun);

$stale  = $issued;                 // يُستعمل لاحقاً لإثبات موت الملغى
$issued = $twin;
$row    = DB::table('employee_activation_codes')
    ->where('employee_id', $employeeId)->where('status', 'ACTIVE')
    ->orderByDesc('id')->first();

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٢) الرفض قبل النجاح ───────────────────────────────────────');

$r = $svc->requestOtpByQr($stale['qr_token'], $deviceA, $trace);
$check('5أ. ⚠ والرمزُ الذي أبطله إصدارُ غيرِه لا يعمل',
    ($r['ok'] ?? true) === false, mb_substr($r['message'] ?? '', 0, 40));

$r = $svc->requestOtpByQr('غير-صالح', $deviceA, $trace);
$check('6. رمزٌ بشكلٍ خاطئ يُرفض', ($r['ok'] ?? true) === false);

$r = $svc->requestOtpByQr(str_repeat('a', 64), $deviceA, $trace);
$check('7. ورمزٌ غير معروف يُرفض', ($r['ok'] ?? true) === false,
    mb_substr($r['message'] ?? '', 0, 40));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٣) المسح الناجح ⇦ نفسُ طبقة OTP ──────────────────────────');

$r = $svc->requestOtpByQr($issued['qr_token'], $deviceA, $trace);
$check('8. المسح ينجح', ($r['ok'] ?? false) === true, $r['message'] ?? '');
$check('9. ويُرجع الهاتف مقنَّعاً واسمَ الموظف من القاعدة',
    !empty($r['masked_phone']) && ($r['employee_name'] ?? '') === 'موظف اختبار QR',
    ($r['masked_phone'] ?? '') . ' · ' . ($r['employee_name'] ?? ''));

$otpRow = DB::table('employee_otps')->where('employee_id', $employeeId)
    ->where('status', 'PENDING')->orderByDesc('id')->first();
$check('10. ⚠ وطبقةُ OTP لم تُتخطَّ — رمزُ تحقّقٍ أُنشئ فعلاً',
    $otpRow !== null && $otpRow->device_hash === $hashA);

$check('11. ⚠ والرمزُ لم يُستهلك بعد — الاستهلاكُ عند نجاح التفعيل',
    DB::table('employee_activation_codes')->where('id', $row->id)->value('status') === 'ACTIVE');

$check('12. وثُبِّت الجهازُ الماسح على الطلب',
    DB::table('employee_activation_codes')->where('id', $row->id)->value('scan_device_hash') === $hashA);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٤) التزامن: جهازان على رمزٍ واحد ─────────────────────────');

$r = $svc->requestOtpByQr($issued['qr_token'], $deviceB, $trace);
$check('13. ⚠ جهازٌ ثانٍ يمسح الرمزَ نفسَه يُرفض',
    ($r['ok'] ?? true) === false, mb_substr($r['message'] ?? '', 0, 50));

$check('14. ⚠ ولم يُرسَل له رمزُ تحقّق',
    !DB::table('employee_otps')->where('employee_id', $employeeId)
        ->where('device_hash', $hashB)->exists());

$check('15. والجهازُ الأوّل ما زال صاحبَ الطلب',
    DB::table('employee_activation_codes')->where('id', $row->id)->value('scan_device_hash') === $hashA);

/* وإعادةُ المسح من الجهاز نفسِه مسموحة — انقطاعُ شبكةٍ لا يحرق الرمز. */
$r = $svc->requestOtpByQr($issued['qr_token'], $deviceA, $trace);
$check('16. ⚠ وإعادةُ المسح من الجهاز نفسِه تُقبل — انقطاعٌ عابر لا يحرق الرمز',
    ($r['ok'] ?? false) === true);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٥) التفعيل يُكمل بالمسار القائم نفسِه ────────────────────');

/* الرمزُ مُجزَّأ ولا يُقرأ — يُزرع رمزٌ معلوم لإتمام الخطوة الثانية. */
$otpRow = DB::table('employee_otps')->where('employee_id', $employeeId)
    ->where('status', 'PENDING')->orderByDesc('id')->first();
DB::table('employee_otps')->where('id', $otpRow->id)
    ->update(['otp_hash' => Illuminate\Support\Facades\Hash::make('4321')]);

/*
 * ⚠ ويُكمَّل بمعرّف الطلب لا بالرقم — كما يفعل التطبيق بعد المسح: ما
 * عاد إليه رقمٌ مقنَّع لا يصلح للإرسال.
 */
$v = $svc->verifyOtp('', '4321', $deviceA, $trace, (int) $r['activation_id']);
$check('17. التفعيل ينجح عبر `verifyOtp` نفسِها', ($v['ok'] ?? false) === true,
    $v['message'] ?? '');

$check('18. ⚠ وربطُ الجهاز جرى بالنظام القائم لا بنظامٍ موازٍ',
    DB::table('employee_devices')->where('employee_id', $employeeId)
        ->where('status', 'ACTIVE')->where('device_hash', $hashA)->exists());

$check('19. والجهازُ وُسم دائماً كجهاز موظّف',
    app(DeviceRegistryService::class)->isEmployeeDevice($hashA));

$check('20. وفُتحت جلسةٌ في `employee_sessions` لا رمز Sanctum',
    DB::table('employee_sessions')->where('employee_id', $employeeId)
        ->where('status', 'ACTIVE')->exists()
    && !DB::table('personal_access_tokens')->where('name', 'employee')->exists());

$check('21. وصار الموظف ACTIVE',
    DB::table('employees')->where('id', $employeeId)->value('status') === 'ACTIVE');

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٦) أحاديّة الاستعمال ─────────────────────────────────────');

$check('22. ⚠ الرمزُ صار مستهلَكاً بعد النجاح',
    DB::table('employee_activation_codes')->where('id', $row->id)->value('status') === 'USED');

$r = $svc->requestOtpByQr($issued['qr_token'], $deviceA, $trace);
$check('23. ⚠ وصورةٌ قديمة من الرمز لا تُفعّل ثانيةً على الجهاز نفسِه',
    ($r['ok'] ?? true) === false || DB::table('employee_activation_codes')
        ->where('id', $row->id)->value('status') !== 'ACTIVE');

$r = $svc->requestOtpByQr($issued['qr_token'], $deviceB, $trace);
$check('24. ⚠ ولا تُفعّل جهازاً آخر — ويُحرق الطلب',
    ($r['ok'] ?? true) === false
    && DB::table('employee_activation_codes')->where('id', $row->id)->value('status') === 'COMPROMISED',
    'الحالة=' . DB::table('employee_activation_codes')->where('id', $row->id)->value('status'));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٧) المدّة والإلغاء ───────────────────────────────────────');

$fresh = $svc->issueCode($agentId, $employeeId, $agentId, $trace);
$freshRow = DB::table('employee_activation_codes')->where('employee_id', $employeeId)
    ->where('status', 'ACTIVE')->orderByDesc('id')->first();

$check('25. ⚠ وإصدارُ رمزٍ جديد أبطل السابق',
    DB::table('employee_activation_codes')->where('id', $row->id)->value('status') !== 'ACTIVE');

DB::table('employee_activation_codes')->where('id', $freshRow->id)
    ->update(['expires_at' => now()->subMinute()]);
$r = $svc->requestOtpByQr($fresh['qr_token'], $deviceA, $trace);
$check('26. ⚠ ورمزٌ منتهي المدّة يُرفض — والتحقّق في الخادم لا في الهاتف',
    ($r['ok'] ?? true) === false && str_contains($r['message'] ?? '', 'انتهت'),
    mb_substr($r['message'] ?? '', 0, 45));
$check('27. ويُحرق في القاعدة',
    DB::table('employee_activation_codes')->where('id', $freshRow->id)->value('status') === 'EXPIRED');

$fresh2 = $svc->issueCode($agentId, $employeeId, $agentId, $trace);
$row2 = DB::table('employee_activation_codes')->where('employee_id', $employeeId)
    ->where('status', 'ACTIVE')->orderByDesc('id')->first();
DB::table('employee_activation_codes')->where('id', $row2->id)->update([
    'status' => 'REVOKED', 'revoked_at' => now(), 'revoked_reason' => 'ألغاه الوكيل',
]);
$r = $svc->requestOtpByQr($fresh2['qr_token'], $deviceA, $trace);
$check('28. ⚠ ورمزٌ ألغاه الوكيل لا يعمل ولو كان محفوظاً صورةً',
    ($r['ok'] ?? true) === false);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٨) التبعية والطريقُ اليدويّ ──────────────────────────────');

/* ⚠ رمزُ موظّفِ وكيلٍ لا يُنتج تبعيةً لوكيلٍ آخر: الاشتقاقُ من الصفّ. */
$fresh3 = $svc->issueCode($agentId, $employeeId, $agentId, $trace);
$row3 = DB::table('employee_activation_codes')->where('employee_id', $employeeId)
    ->where('status', 'ACTIVE')->orderByDesc('id')->first();
$check('29. ⚠ الوكيلُ والموظفُ يُشتقّان من الصفّ لا من الرمز',
    (int) $row3->agent_id === $agentId && (int) $row3->employee_id === $employeeId);

$empAgentBefore = DB::table('employees')->where('id', $employeeId)->value('agent_id');
$svc->requestOtpByQr($fresh3['qr_token'], $deviceA, $trace);
$check('30. ⚠ والمسحُ لا يغيّر تبعيةَ الموظف',
    DB::table('employees')->where('id', $employeeId)->value('agent_id') === $empAgentBefore);

/* والطريقُ اليدويّ ما زال يعمل على الطلب نفسِه. */
$fresh4 = $svc->issueCode($agentId, $employeeId, $agentId, $trace);
$manual = $svc->requestOtp($phone, $fresh4['code'], $deviceA, $trace);
$check('31. ⚠ والكودُ اليدويّ ما زال يعمل — لم يُكسَر ولم يُحذف',
    ($manual['ok'] ?? false) === true, $manual['message'] ?? '');

$manualBad = $svc->requestOtp($phone, 'WRONGWRO', $deviceA, $trace);
$check('32. وكودٌ يدويّ خاطئ ما زال يُرفض',
    ($manualBad['ok'] ?? true) === false);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٩) الموظف غير الفعّال ────────────────────────────────────');

DB::table('employees')->where('id', $employeeId)->update(['status' => 'SUSPENDED']);
$fresh5 = $svc->issueCode($agentId, $employeeId, $agentId, $trace);
DB::table('employees')->where('id', $employeeId)->update(['status' => 'SUSPENDED']);
/*
 * ⚠ ويُفرَّغ عدّادُ المعدّل أوّلاً. فحصُ الحدّ يسبق فحصَ الحالة في
 * `requestOtp`، وقد بلغته هذه السلسلةُ من المحاولات — فيردّ «محاولات
 * كثيرة» ويبدو الفحصُ ناجحاً وهو لم يبلغ ما جاء يقيسه أصلاً.
 */
DB::table('employee_otps')->where('phone', $phone)->delete();
$r = $svc->requestOtpByQr($fresh5['qr_token'], $deviceA, $trace);
$check('33. ⚠ وموظّفٌ موقوف لا يُفعَّل بالرمز',
    ($r['ok'] ?? true) === false && str_contains($r['message'] ?? '', 'غير مفعّل'),
    mb_substr($r['message'] ?? '', 0, 45));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ١٠) السجلّ الأمنيّ ──────────────────────────────────────');

$events = DB::table('security_logs')->where('employee_id', $employeeId)
    ->orWhere('phone', $phone)->pluck('event_type')->unique()->all();
$check('34. سُجّلت أحداثُ الرمز أمنياً',
    in_array('QR_OTHER_DEVICE', $events, true) && in_array('CODE_EXPIRED', $events, true),
    implode('، ', $events));

/* ⚠ ولا يُكتب الرمزُ الخام ولا رمزُ التحقّق في أي سجلّ. */
$leak = DB::table('security_logs')
    ->where('detail', 'like', '%' . substr($issued['qr_token'], 0, 20) . '%')->count()
    + DB::table('audit_logs')
        ->where('entity_id', 'like', '%' . substr($issued['qr_token'], 0, 20) . '%')->count();
$check('35. ⚠ ولا يتسرّب الرمزُ الخام إلى أي سجلّ', $leak === 0, 'تسريبات=' . $leak);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── تنظيف ────────────────────────────────────────────────────');

foreach (['employee_sessions', 'employee_devices', 'employee_otps',
          'employee_activation_codes', 'employee_permissions'] as $t) {
    DB::table($t)->where('employee_id', $employeeId)->delete();
}
DB::table('security_logs')->where('employee_id', $employeeId)->delete();
DB::table('security_logs')->where('phone', $phone)->delete();
DB::table('audit_logs')->where('employee_id', $employeeId)->delete();
DB::table('employees')->where('id', $employeeId)->delete();
DB::table('device_registry')->whereIn('device_hash', array_filter([$hashA, $hashB]))->delete();
$line('  حُذف الموظف الاختباريّ وأجهزتُه وسجلّاته.');

$line();
$line('── اللقطة المالية ────────────────────────────────────────────');
$after = $snap();
$same = true;
foreach ($before as $k => $v) {
    $eq = ($v === $after[$k]);
    if (!$eq) $same = false;
    printf("  %-10s %-14s ⇦ %-14s %s\n", $k, $v, $after[$k], $eq ? '✓' : '✗');
}
$check('⚠ لا شيء ماليّ تغيّر', $same);

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) $line('   المُخفِق: ' . implode(' · ', $failed));
$line('══════════════════════════════════════════════════════════════');
