<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class wallet extends Model
{
    protected $table = 'wallet';

    protected $fillable = [
          'UeserID', 'Currency_ID' ,  'Walet'

    ];
	
	
	
    protected $casts = [
        'Walet' => 'double',   
    ];
}
