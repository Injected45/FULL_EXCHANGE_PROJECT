<?php

use Illuminate\Support\Facades\DB;

/* الموظّفون المسجَّلون، وأكوادُهم، وما جرى لرموز التحقّق. */

echo "── الموظّفون ─────────────────────────────────────" . PHP_EOL;
foreach (DB::table('employees')->orderByDesc('id')->limit(6)
            ->get(['id', 'agent_id', 'full_name', 'phone', 'status']) as $e) {
    echo "  id={$e->id} | {$e->full_name} | هاتف={$e->phone} | وكيل={$e->agent_id} | {$e->status}" . PHP_EOL;
}

echo PHP_EOL . "── أكواد التفعيل ─────────────────────────────────" . PHP_EOL;
foreach (DB::table('employee_activation_codes')->orderByDesc('id')->limit(6)
            ->get(['id', 'employee_id', 'status', 'expires_at', 'created_at']) as $c) {
    echo "  id={$c->id} | موظّف={$c->employee_id} | {$c->status} | ينتهي={$c->expires_at}" . PHP_EOL;
}

echo PHP_EOL . "── رموز التحقّق ──────────────────────────────────" . PHP_EOL;
foreach (DB::table('employee_otps')->orderByDesc('id')->limit(8)
            ->get(['id', 'employee_id', 'phone', 'status', 'attempts',
                   'expires_at', 'created_at']) as $o) {
    echo "  id={$o->id} | موظّف={$o->employee_id} | أُرسل إلى الرقم المخزَّن={$o->phone}"
       . " | {$o->status} | محاولات={$o->attempts} | ينتهي={$o->expires_at}" . PHP_EOL;
}

echo PHP_EOL . "── الوجهة الفعلية للإرسال ────────────────────────" . PHP_EOL;
$dev = config('services.whatsapp.dev_otp_to');
echo '  OTP_DEV_TO = ' . var_export($dev, true) . PHP_EOL;
echo '  ⇐ ' . ($dev
    ? "كلُّ رمزٍ يذهب إلى {$dev} أياً كان صاحبُه"
    : 'كلُّ رمزٍ يذهب إلى صاحبه') . PHP_EOL;
