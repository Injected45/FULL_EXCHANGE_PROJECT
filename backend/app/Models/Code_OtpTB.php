<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Code_OtpTB extends Model
{
    protected $table = 'Code_OtpTB';

    public $timestamps = false; // مهم: يمنع Laravel من استخدام created_at و updated_at

    protected $fillable = [
        'UeserPohone',
        'CodeOtp',
        'ExpeaerTime',
        'ISActive',
        'insertdate'
    ];

    protected static function boot()
    {
        parent::boot();

        static::creating(function ($model) {
            // توليد OTP عشوائي 4 أرقام
            if (empty($model->CodeOtp)) {
                $model->CodeOtp = rand(1000, 9999);
            }

            // تعيين القيم الافتراضية
            if (is_null($model->ISActive)) {
                $model->ISActive = 0;
            }

            if (is_null($model->insertdate)) {
                $model->insertdate = now();
            }
        });
    }
}
