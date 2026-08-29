<?php

namespace Modules\BankVisaTransfers\Models;

use Illuminate\Database\Eloquent\Model;

class BankVisaTransfer extends Model
{
    protected $table = 'BankVisaTransfers_CentralLibya';

    protected $fillable = [
        'TransferSeq',
        'FullName',
        'NationalID',
        'AmountUSD',
        'AmountLocal',
        'ExchangeRate',
        'AccountNumber',
        'Phone',
        'Status' ,
        'Code'
    ];

    public $timestamps = false; // لأنك تستخدم CreatedAt يدوي
}
