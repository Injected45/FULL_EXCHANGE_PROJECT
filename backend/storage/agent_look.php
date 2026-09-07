<?php
use Illuminate\Support\Facades\DB;
$rows = DB::table('users as u')
    ->leftJoin('AccountsTb as acc', 'acc.AccID', '=', 'u.AccID')
    ->where(function ($w) {
        $w->where('acc.AccName', 'like', '%الامان%')
          ->orWhere('acc.AccName', 'like', '%الأمان%')
          ->orWhere('u.name', 'like', '%الامان%')
          ->orWhere('u.phone', 'like', '%925093709%');
    })
    ->limit(10)
    ->get(['u.id', 'u.name', 'u.phone', 'u.AccID', 'u.UeserType', 'u.AccountType',
           'u.BrancchID', 'u.Reg', 'u.device_id', 'acc.AccName']);
foreach ($rows as $r) {
    echo "id={$r->id} | هاتف={$r->phone} | الاسم={$r->name} | الحساب={$r->AccName} (AccID={$r->AccID})"
       . " | نوع={$r->UeserType}/{$r->AccountType} | فرع={$r->BrancchID} | Reg={$r->Reg}"
       . " | جهاز=" . ($r->device_id ? mb_substr($r->device_id, 0, 10) . '…' : 'لا شيء') . PHP_EOL;
}
echo 'العدد: ' . count($rows) . PHP_EOL;
