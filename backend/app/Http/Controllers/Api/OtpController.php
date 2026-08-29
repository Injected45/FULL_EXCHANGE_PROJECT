<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Validator;
use App\Models\Code_OtpTB;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Http;


use Illuminate\Support\Facades\Auth;

class OtpController extends BaseController
{
  
  

 


    protected function checkWhatsapp(string $phone): array
    {
        // تنظيف رقم الهاتف من المسافات والرموز
        $phone = preg_replace('/[^0-9]/', '', $phone);
    
        // API Key
        $apiKey = config('services.whatsapp.token');
    
        // رابط التحقق من الرقم
        $url = "https://wa.rhalla.online/api/sessions/867ea69e-1926-4a85-baa8-2c1b65a068aa/contacts/check/{$phone}";
    
        try {
            $response = Http::withOptions([
                'verify' => false,
                'timeout' => 10,
            ])
            ->withHeaders([
                'X-API-Key' => $apiKey,
            ])
            ->get($url);
    
            // في حالة وجود خطأ من API
            if (!$response->successful()) {
                return [
                    'success' => false,
                    'message' => 'خدمة التحقق من واتساب أعادت خطأ',
                    'data' => $response->body(),
                ];
            }
    
            // تحويل الاستجابة إلى JSON
            $data = $response->json();
    
            /*
             * الاستجابة المتوقعة:
             *
             * {
             *     "number": "218916121181",
             *     "exists": true,
             *     "whatsappId": "218916121181@c.us"
             * }
             */
    
            // التحقق من وجود الرقم على واتساب
            if (isset($data['exists']) && $data['exists'] === true) {
                return [
                    'success' => true,
                    'message' => 'الرقم موجود على واتساب',
                    'data' => $data,
                ];
            }
    
            // الرقم غير موجود على واتساب
            return [
                'success' => false,
                'message' => 'الرقم غير موجود على واتساب',
                'data' => $data,
            ];
    
        } catch (\Exception $e) {
            return [
                'success' => false,
                'message' => 'فشل الاتصال بخدمة التحقق من واتساب',
                'data' => $e->getMessage(),
            ];
        }
    }
      protected function sendWhatsapp(string $phone, string $message): array
    {
        // تنظيف الرقم
        $phone = preg_replace('/[^0-9]/', '', $phone);
    
        // تحويل الرقم الليبي إلى الصيغة الدولية
        if (str_starts_with($phone, '0')) {
            $phone = '218' . substr($phone, 1);
        } elseif (str_starts_with($phone, '9')) {
            $phone = '218' . $phone;
        }
    
        $chatId = $phone . '@c.us';
    
        $apiKey = config('services.whatsapp.token');
    
        $url = 'https://wa.rhalla.online/api/sessions/867ea69e-1926-4a85-baa8-2c1b65a068aa/messages/send-text';
    
        try {
            $response = Http::withOptions([
                'verify' => false,
                'timeout' => 20,
            ])
            ->withHeaders([
                'X-API-Key' => $apiKey,
                'Accept' => 'application/json',
                'Content-Type' => 'application/json',
            ])
            ->post($url, [
                'chatId' => $chatId,
                'text' => $message,
            ]);
    
            return [
                'success' => $response->successful(),
                'phone_original' => $phone,
                'chatId' => $chatId,
                'http_status' => $response->status(),
                'response' => $response->json(),
                'raw_response' => $response->body(),
            ];
    
        } catch (\Throwable $e) {
            return [
                'success' => false,
                'message' => $e->getMessage(),
                'phone' => $phone,
                'chatId' => $chatId,
            ];
        }
    }

    
    
    
    public function sendOtp(Request $request)
    {
        // التحقق من صحة الرقم
        $validator = Validator::make($request->all(), [
            'phone' => 'required|string'
        ]);
    
        if ($validator->fails()) {
            return $this->sendError(
                $validator->errors()->first(),
                [],
                422
            );
        }
    
        // تنظيف الرقم
        $phone = preg_replace('/[^0-9]/', '', $request->phone);
    
        // الرقم الليبي يجب أن يكون 9 أرقام ويبدأ بـ 9
        if (!preg_match('/^9[0-9]{8}$/', $phone)) {
            return $this->sendError(
                'رقم الهاتف غير صالح. يجب أن يبدأ بـ 91 أو 92 ويحتوي على 9 أرقام فقط.',
                [],
                422
            );
        }
    
        try {
    
            // إضافة كود الدولة ليبيا
            $phoneTo = '218' . $phone;
    
            // حذف OTP القديم
            Code_OtpTB::where('UeserPohone', $phoneTo)->delete();
    
            // إنشاء OTP جديد
            $otp = Code_OtpTB::create([
                'UeserPohone' => $phoneTo,
                'ExpeaerTime' => now()->addMinutes(3),
            ]);
    
            // وجهة التطوير — انظر config/services.php. غائبة في الإنتاج ⇒ معطّلة.
            $devTo = config('services.whatsapp.dev_otp_to');

            // التحقق من وجود الرقم على WhatsApp
            // يُتخطّى في التطوير: الرقم الليبي التجريبي قد لا يكون على واتساب أصلاً.
            if (!$devTo) {
                $check = $this->checkWhatsapp($phoneTo);

                if (!$check['success']) {
                    return $this->sendError(
                        $check['message'],
                        $check['data'] ?? [],
                        404
                    );
                }
            }

            // إرسال OTP عبر WhatsApp
            $message = "رمز التحقق الخاص بك هو: " . $otp->CodeOtp;

            $whatsappResponse = $this->sendWhatsapp(
                $devTo ?: $phoneTo,
                $message
            );
    
            // التأكد من نجاح الإرسال
            if (!$whatsappResponse['success']) {
                return $this->sendError(
                    $whatsappResponse['message'],
                    $whatsappResponse['data'] ?? [],
                    500
                );
            }
    
        } catch (\Exception $e) {
            return $this->sendError(
                $e->getMessage(),
                [],
                500
            );
        }
    
        return $this->sendResponse(
            [
                'id' => $otp->ID,
                'expires_at' => $otp->ExpeaerTime,
                'whatsapp_response' => $whatsappResponse,
            ],
            'تم إرسال رمز التحقق بنجاح'
        );
    }
    
    
    public function checkOtp(Request $request)
    {
        $validator = Validator::make($request->all(), [
            'phone' => 'required|string',
            'CodeOtp' => 'required|integer|digits:4',
        ]);
    
        if ($validator->fails()) {
            return $this->sendError(
                $validator->errors()->first(),
                [],
                422
            );
        }
    
        // إضافة كود الدولة
        $phone = '218' . $request->phone;
    
        // البحث عن OTP
        $otp = Code_OtpTB::where('UeserPohone', $phone)
            ->where('CodeOtp', $request->CodeOtp)
            ->where('ISActive', 0)
            ->first();
    
        if (!$otp) {
            return response()->json([
                'message' => 'عذرا لا يوجد بيانات متطابقة',
                'status' => false,
            ], 404);
        }
    
        // التحقق من انتهاء مدة OTP
        if ($otp->ExpeaerTime < now()) {
            return response()->json([
                'message' => 'عذرا تم انتهاء مدة هذا الكود',
                'status' => false,
            ], 422);
        }
    
        // تفعيل الكود بعد استخدامه
        Code_OtpTB::where('ID', $otp->ID)
            ->update([
                'ISActive' => 1
            ]);
    
        return response()->json([
            'message' => 'تمت المطابقة بنجاح',
            'status' => true,
            'data' => $otp,
        ], 200);
    }

    public function senotpGroupFr(Request $request)
    {
        $validator = Validator::make($request->all(), [
            'ISID' => 'required|string'
        ]);
    
        if ($validator->fails()) {
            return response()->json([
                'success' => false,
                'message' => 'فشل التحقق من البيانات',
                'errors'  => $validator->errors(),
            ], 422);
        }
    
        $user = Auth::user();
    
        if (!$user) {
            return response()->json([
                'success' => false,
                'message' => 'الرجاء تسجيل الدخول',
            ], 401);
        }
    
        $results = DB::table('ExchangeAccData')
            ->join('users', 'ExchangeAccData.AccID', '=', 'users.AccID')
            ->where('users.id', $user->id)
            ->where('ExchangeAccData.IsActive', 1)
            ->where('ExchangeAccData.Code', $request->ISID)
            ->select([
                'ExchangeAccData.AccCode',
                'ExchangeAccData.AccID',
                'ExchangeAccData.AccName',
                'ExchangeAccData.TransDate',
                'ExchangeAccData.TransValue',
                'ExchangeAccData.Type_trns',
                'ExchangeAccData.commint',
                'ExchangeAccData.Code',
                'ExchangeAccData.States_spinng',
                'ExchangeAccData.Type_from',
                'ExchangeAccData.States_Ineger',
                'ExchangeAccData.ServiceName',
                'ExchangeAccData.CName',
                'ExchangeAccData.TransPrice',
                'ExchangeAccData.NetTotal',
                'ExchangeAccData.ServiceExVal',
                'ExchangeAccData.Notes',
    
                // رقم هاتف المستخدم
                'users.phone',
            ])
            ->first();
    
        // لا توجد بيانات
        if (!$results) {
            return response()->json([
                'success' => false,
                'message' => 'لا توجد بيانات'
            ], 404);
        }
    
        // رقم الهاتف الذي سترسل له الرسالة
        $phoneTo = $results->phone;
    
        // نص الرسالة
        $message =
            "مطلوب سكرين" . PHP_EOL .
            "رقم الهاتف: " . $results->AccCode . PHP_EOL .
            "قيمة الحوالة: " . number_format((float) $results->TransValue, 2) . PHP_EOL .
            "للغرفة: " . "218" . $results->phone;
    
        // إرسال WhatsApp
        $whatsappResponse = $this->sendWhatsapp(
           "218914200648",
            $message
        );
    
        return response()->json([
            'success' => true,
            'message' => 'تم إرسال الرسالة بنجاح',
            'data' => $message,
            'whatsapp' => $whatsappResponse
        ]);
    }
  
    
  
}
