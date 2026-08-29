<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class CommtionRetview extends Model
{
    protected $table = 'COMMTION_RETVIEW';

    // مهم جداً لأن الفيو ما فيه primary key
    protected $primaryKey = null;

    public $incrementing = false;

    public $timestamps = false;

    // لأن الفيو read only
    protected $guarded = [];
}