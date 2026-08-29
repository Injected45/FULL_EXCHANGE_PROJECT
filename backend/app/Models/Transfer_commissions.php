<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Transfer_commissions extends Model
{
    protected $table = 'Transfer_commissions';

    protected $fillable = [
          'First_Value', 'Second_value' ,  'Commission_value'

    ];



}
