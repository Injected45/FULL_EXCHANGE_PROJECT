<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class n8n_payments extends Model
{
    protected $table = 'n8n_payments';

    protected $fillable = [
        'id',
        'amount',
        'phone',
        'accountID',
        'media',
        'date',
        'created_at',
        'Phone',
        'Status' ,
        'updated_at'
    ];

    public $timestamps = false; // لأنك تستخدم CreatedAt يدوي
}


