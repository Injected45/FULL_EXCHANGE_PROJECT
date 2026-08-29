<?php

namespace App\Http\Controllers;

use App\Events\NotificationSent;

class NotificationController extends Controller
{
    public function send()
    {
        $message = "يوجد إشعار جديد!";

        event(new NotificationSent($message));

        return response()->json(['status' => 'Notification sent!']);
    }
}
