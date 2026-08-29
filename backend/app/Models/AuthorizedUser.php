<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class AuthorizedUser extends Model
{
    protected $table = 'AuthorizedUsers'; // اسم الجدول

    protected $primaryKey = 'ID'; // المفتاح الأساسي

    public $timestamps = false; // لأننا مش مستخدمين created_at و updated_at

    protected $fillable = [
        'Name_post',
        'CreatedDate',
        'IsActive',
        'BranchID',
        'UserID',
        'InsertUserID',
        'AccID' , 
        'phone'
    ];
}