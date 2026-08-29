<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Services\TwilioService;
use Twilio\Rest\Client;

class SmsController extends Controller
{
    protected $twilio;
    protected $verifySid;
    protected $client;

    public function __construct(TwilioService $twilio)
    {
        $this->twilio = $twilio;

        // انشاء عميل Twilio مباشرة لاستخدام Verify
        $this->client = new Client(
            config('services.twilio.sid'),
            config('services.twilio.token')
        );

        $this->verifySid = config('services.twilio.verify_sid'); // لازم تضيفها في config/services.php و .env
    }

    // ارسال رسالة SMS عادية
    public function send(Request $request)
    {
        $request->validate([
            'to' => 'required|string',
            'message' => 'required|string',
        ]);

        try {
            $sid = $this->twilio->sendSMS($request->to, $request->message);

            return response()->json([
                'success' => true,
                'message_sid' => $sid,
            ]);
        } catch (\Exception $e) {
            return response()->json([
                'success' => false,
                'error' => $e->getMessage(),
            ], 500);
        }
    }

    // ارسال رمز التحقق OTP باستخدام Twilio Verify
    public function sendVerification(Request $request)
    {
        $request->validate([
            'phone' => 'required|string',
        ]);

        try {
            $verification = $this->client->verify->v2->services($this->verifySid)
                ->verifications
                ->create($request->phone, "sms");

            return response()->json([
                'success' => true,
                'status' => $verification->status,
                'message' => 'تم إرسال رمز التحقق',
            ]);
        } catch (\Exception $e) {
            return response()->json([
                'success' => false,
                'error' => $e->getMessage(),
            ], 500);
        }
    }

    // التحقق من رمز OTP
    public function checkVerification(Request $request)
{
    $request->validate([
        'phone' => 'required|string',
        'code' => 'required|string',
    ]);

    try {
        $verificationCheck = $this->client->verify->v2->services($this->verifySid)
            ->verificationChecks
            ->create([
                'to' => $request->phone,
                'code' => $request->code,
            ]);

        if ($verificationCheck->status === "approved") {
            return response()->json(['success' => true, 'message' => 'تم التحقق بنجاح']);
        } else {
            return response()->json(['success' => false, 'message' => 'رمز التحقق غير صحيح'], 422);
        }
    } catch (\Exception $e) {
        return response()->json([
            'success' => false,
            'error' => $e->getMessage(),
        ], 500);
    }
}
}

