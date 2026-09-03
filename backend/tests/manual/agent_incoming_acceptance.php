<?php

/*
 * اختبارات قبول «الحوالات الواردة للوكيل».
 *
 * يُشغَّل هكذا من backend/:
 *     php artisan tinker --execute="require 'tests/manual/agent_incoming_acceptance.php';"
 *
 * ليس PHPUnit عمداً: المشروع بلا مجموعة اختبارات حقيقية، وهذه تُشغَّل
 * على قاعدة محلّية بعينها وتقرأ حالتها الفعلية. تُبقى هنا لتُعاد عند أي
 * تغيير يمسّ هذا المسار.
 *
 * أهمّها البند 11: تسجيل التسليم يجب ألّا يحرّك رصيداً ولا قيداً.
 */

use App\Services\AgentIncomingTransfersService;
use Illuminate\Support\Facades\DB;

$svc = app(AgentIncomingTransfersService::class);

$line = fn($s) => print($s . PHP_EOL);
$ok = 0;
$fail = 0;
$check = function (string $name, bool $pass, string $detail = '') use (&$ok, &$fail, $line) {
    if ($pass) { $ok++;  $line("  PASS  $name" . ($detail ? "  ($detail)" : '')); }
    else       { $fail++; $line("  FAIL  $name" . ($detail ? "  ($detail)" : '')); }
};

/* الوكيل المستعمَل في التجربة — 104 هو «جاري شركة الامانة»، فرع 15. */
$agentId  = 104;
$branchId = 15;
$userType = 3;

$line('=== agent incoming transfers — acceptance ===');

/* ---------- بصمة مالية قبل أي شيء ---------- */
$snapshot = function () {
    return [
        'wallet'   => DB::table('wallet')->selectRaw('COUNT(*) c, ISNULL(SUM(Walet),0) s')->first(),
        'ledger'   => DB::table('ExchangeAccData')->count(),
        'internal' => DB::table('InternalEx')
            ->selectRaw('COUNT(*) c, ISNULL(SUM(CAST(ConfirmType AS INT)),0) s')->first(),
    ];
};
$before = $snapshot();

/* ---------- 1) المزامنة لا تجلب غير المعتمد ---------- */
$svc->syncFromCore($agentId, $branchId, $userType);

$unapproved = DB::table('InternalEx')->where('ConfirmType', 0)->pluck('Code');
$leaked = DB::table('agent_incoming_transfers')
    ->where('agent_id', $agentId)
    ->whereIn('transfer_number', $unapproved)
    ->count();
$check('غير المعتمدة لا تصل إلى الوكيل', $leaked === 0, "leaked=$leaked");

/* ---------- 2) عزل الوكلاء ---------- */
$other = DB::table('agent_incoming_transfers')->where('agent_id', '<>', $agentId)->count();
$mine  = $svc->list($agentId, null, null, 1, 100);
$foreign = collect($mine['items'])->where('agent_id', '<>', $agentId)->count();
$check('لا تُعاد حوالات وكيل آخر', $foreign === 0, "others_in_db=$other");

/* ---------- 3) التسليم: مرّة واحدة مهما تكرّر الطلب ---------- */
$row = DB::table('agent_incoming_transfers')
    ->where('agent_id', $agentId)
    ->where('status', AgentIncomingTransfersService::PENDING)
    ->first();

if (!$row) {
    $line('  SKIP  لا توجد حوالة بانتظار التسليم — اعتمد حوالةً ثم أعد التشغيل');
} else {
    $trace = ['ip' => '127.0.0.1', 'device' => 'test-device', 'session' => 'test-session'];

    $r1 = $svc->markDelivered($agentId, $row->id, $agentId, $trace);
    $r2 = $svc->markDelivered($agentId, $row->id, $agentId, $trace);

    $check('التسليم الأول يغيّر الحالة', $r1['changed'] === true);
    $check('التسليم المكرّر لا يغيّر شيئاً', $r2['changed'] === false);

    $hist = DB::table('transfer_status_history')->where('transfer_id', $row->id)->count();
    $check('سجلّ واحد في تاريخ الحالات', $hist === 1, "rows=$hist");

    $fresh = DB::table('agent_incoming_transfers')->where('id', $row->id)->first();
    $check('الحالة DELIVERED', $fresh->status === AgentIncomingTransfersService::DELIVERED);
    $check('وقت التسليم مسجّل من الخادم', $fresh->delivered_at !== null);
    $check('منفّذ التسليم مسجّل', (int) $fresh->delivered_by === $agentId);
}

/* ---------- 4) البند 11: لا أثر مالي ---------- */
$after = $snapshot();

$check('مجموع المحافظ لم يتغيّر',
    (string) $before['wallet']->s === (string) $after['wallet']->s,
    "{$before['wallet']->s} -> {$after['wallet']->s}");
$check('عدد صفوف المحافظ لم يتغيّر',
    $before['wallet']->c === $after['wallet']->c);
$check('القيد لم يتغيّر',
    $before['ledger'] === $after['ledger'],
    "{$before['ledger']} -> {$after['ledger']}");
$check('حالات المنظومة (ConfirmType) لم تُمسّ',
    (string) $before['internal']->s === (string) $after['internal']->s,
    "{$before['internal']->s} -> {$after['internal']->s}");

$line('');
$line("=== passed: $ok   failed: $fail ===");
