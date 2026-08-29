<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Navction extends Model
{
    protected $table = 'Navction_Tb'; // اسم الجدول
    protected $primaryKey = 'ID';     // لو العمود الأساسي اسمه ID

    // الأعمدة المسموح تعبئتها
    protected $fillable = [
        'Type_ID',
        'Name_NEvction',
        'IS_showe',
        'BracnID',
    ];

    public $timestamps = false; // إذا الجدول ما فيه created_at / updated_at
}