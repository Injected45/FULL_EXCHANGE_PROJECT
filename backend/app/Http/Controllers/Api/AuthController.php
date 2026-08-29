<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\User;
use Illuminate\Support\Facades\Auth;
use Validator;
use App\Http\Controllers\BaseController;
use Illuminate\Support\Facades\Hash;
use Illuminate\Http\JsonResponse;
use App\Enums\ResponseEnums;
use Carbon\Carbon;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Str;  // <<-- هذا السطر مهم جداً
use App\Models\AuthorizedUser; // ✅ مهم
use Illuminate\Support\Facades\Http;

class AuthController extends BaseController
{

  /////////////////////حذف اليوزر او الغاء تفعيلة بالكامل في الزقت الحالي//////////////////////////////////////////
  public function Delete_Account(Request $request)
  {
      // التحقق من البيانات المرسلة
      $validator = Validator::make($request->all(), [
          'device_id' => 'required|string|max:255',
      ]);
  
      if ($validator->fails()) {
          return response()->json([
              'success' => false,
              'message' => 'البيانات المرسلة غير صحيحة.',
              'errors' => $validator->errors()
          ], 422);
      }
      
  

      // جلب المستخدم الحالي
      $user = Auth::user();
  
      if (!$user) {
          return response()->json([
              'success' => false,
              'message' => 'المستخدم غير مصرح له أو غير مسجل الدخول.'
          ], 401);
      }
  
      // التحقق من مطابقة الجهاز
      $deviceMatch = User::where('id', $user->id)
          ->where('device_id', $request->device_id)
          ->exists();
  
      if (!$deviceMatch) {
          return response()->json([
              'success' => false,
              'message' => 'عذرًا، لا يمكن حذف الحساب من هذا الجهاز.'
          ], 422);
      }
  
      // التحقق إن كان الحساب محذوف مسبقًا
      if ($user->Reg == "NO") {
          return response()->json([
              'success' => false,
              'message' => 'تم حذف الحساب مسبقًا.'
          ], 422);
      }
  
      // تحديث بيانات المستخدم (اعتبار الحساب محذوف)
      DB::table('users')
          ->where('id', $user->id)
          ->update([
              'Reg' => 'NO',
              'deleted_at' => now(),
          ]);
  
      // حذف التوكينات (تسجيل الخروج)
      $user->tokens()->delete();
  
      return response()->json([
          'success' => true,
          'message' => 'تم حذف الحساب وتسجيل الخروج بنجاح.'
      ], 200);
  }
  


    
    public function initAuth(Request $request)
    {
        $request->validate([
            'device_id' => 'required|string|max:255',
        ]);

        $token = Str::random(64);

        DB::table('secure_api_tokens')->insert([
            'token' => $token,
            'device_id' => $request->device_id,
            'expires_at' => now()->addMinutes(3),
            'created_at' => now(),
            'updated_at' => now(),
        ]);

        return $this->sendResponse(['token' => $token], 'رمز التحقق صالح لمدة 3 دقائق.');
    }

/////////////////////////////////////////////////اضافة مستخدم//////////////////////////

public function register(Request $request)
{
    // التحقق من وجود التوكين في الهيدر أو الجسم
    $secureToken = $request->header('Secure-Token') ?? $request->secure_token;
    $device_id = $request->device_id;

    $validToken = DB::table('secure_api_tokens')
        ->where('token', $secureToken)
        ->where('device_id', $device_id)
        ->where('expires_at', '>', now())
        ->where('used', false)
        ->first();

    if (!$validToken) {
        return $this->sendError('رمز التحقق غير صالح أو منتهي أو تم استخدامه مسبقًا.', [], 403);
    }

    // التحقق من المدخلات
    $validator = Validator::make($request->all(), [
        'phone'      => 'required|string|regex:/^\d{8,15}$/',
        'password' => [
            'required',
            'string',
            'min:8',
            'regex:/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/'
        ],
        'device_id'  => 'required|string|max:255',
    ]);

    if ($validator->fails()) {
        return $this->sendError('خطأ في التحقق من البيانات.', $validator->errors(), 422);
    }

    $phones = $request->phone ;

    // جلب المستخدم بناءً على رقم الهاتف
    $user = DB::table('users')->where('phone', $phones)->first();

    DB::beginTransaction();

    try {
        if (!$user) {
            // إنشاء مستخدم جديد
        /////    DB::table('users')->insert([
            //    'phone'      =>$phones,
         //       'password'   => bcrypt($request->password),
            //    'device_id'  => $device_id,
           //     'Reg'        => 'Yes',
             //   'created_at' => now(),
             //   'updated_at' => now(),
            //    'UeserType' => 6 , 
            //    'BrancchID' =>1 ,
           //     'Countries' =>1
           // ]);
           return $this->sendError(' غير مسجل', 'المستخدم غير مسجل بالفعل', 422);

        } elseif ($user->Reg === 'Yes') {
            DB::rollBack();
            return $this->sendError('المستخدم مسجل بالفعل', 'المستخدم مسجل بالفعل', 422);
        } else {
            // تحديث كلمة المرور وبيانات التسجيل للمستخدم الموجود لكنه غير مسجل
            DB::table('users')
                ->where('phone', $phones)
                ->update([
                    'password' => bcrypt($request->password),
                    'Reg' => 'Yes',
                    'device_id' => $device_id,
                    'updated_at' => now(),
                    'created_at' => now() ,
                    'deleted_at' =>NULL
                ]);
        }

        // تعليم التوكن بأنه مستخدم
        DB::table('secure_api_tokens')->where('id', $validToken->id)->update([
            'used' => true
        ]);

        DB::commit();

    } catch (\Exception $e) {
        DB::rollBack();
        return $this->sendError('فشل في التسجيل.', $e->getMessage(), 500);
    }

    // جلب المستخدم بعد العملية
    $updatedUser = User::where('phone', $phones)->first();


    $authorizedUser = AuthorizedUser::where('UserID', $user->id)->first();

    if ($authorizedUser) {
        $name = $authorizedUser->Name_post;
    } else {
        $name = null;
    }

    // إنشاء توكن المصادقة
    $success['token'] = $updatedUser->createToken('mobile-token')->plainTextToken;
    $success['user'] = $updatedUser;
    $success['info'] = $this->getInfo($updatedUser->id);
    $success['Name_post'] = $name;

    return $this->sendResponse($success, 'تم التسجيل وتحديث كلمة المرور بنجاح.');
}



        /**
     * @throws \Exception
     */

    public function login(Request $request): JsonResponse
    {
        // ✅ Validation
        $validator = Validator::make($request->all(), [
            'phone' => [
                'required',
                'string',
                'regex:/^9\d{8}$/', // يبدأ بـ 9 وطوله 9 أرقام
            ],
            'password' => 'required|string|min:8',
            'device_id' => 'required|string|max:255',
        ]);
    
        if ($validator->fails()) {
            return $this->sendError(
                'رقم الهاتف أو كلمة المرور غير صحيحة',
                $validator->errors(),
                422
            );
        }
    
        // ✅ توحيد صيغة الهاتف
        $phone = $request->phone;
    
        $credentials = [
            'phone'    => $phone,
            'password' => $request->password,
        ];
    
        // ✅ محاولة تسجيل الدخول
        if (!Auth::attempt($credentials)) {
            return $this->sendError(
                'رقم الهاتف أو كلمة المرور غير صحيحة',
                null,
                401,
                ResponseEnums::INVALID_CREDENTIALS
            );
        }
    
        // ✅ المستخدم الحالي
        $user = Auth::user();
    
        // ✅ التحقق من الحذف
        if (!is_null($user->deleted_at)) {
            Auth::logout();
            return $this->sendError('تم حذف الحساب.', 'UserDeleted', 404);
        }
    
        // ✅ التحقق من الجهاز (باستثناء رقم محدد)
        if ($user->phone !== '0916121181') {
            if ($user->device_id !== $request->device_id) {
                return $this->sendError(
                    'لا يمكنك تسجيل الدخول من هذا الجهاز.',
                    'DeviceMismatch',
                    422
                );
            }
        }
    
        // ✅ التحقق من التفعيل
        if ($user->Reg === 'NO') {
            Auth::logout();
            return $this->sendError(
                'المستخدم غير مفعل. الرجاء التسجيل أولاً.',
                'UserNotActivated',
                403
            );
        }
    

        $authorizedUser = AuthorizedUser::where('UserID', $user->id)->first();

        if ($authorizedUser) {
            $name = $authorizedUser->Name_post;
        } else {
            $name = null;
        }
        // ✅ إنشاء التوكن
        $success = [
            'token' => $user->createToken('mobile-token')->plainTextToken,
            'user'  => $user,
            'info'  => $this->getInfo($user->id),
            'Name_post'=>$name
        ];
    
        return $this->sendResponse($success, 'تم تسجيل الدخول بنجاح.');
    }
    

public function getInfo($userID)
{
    $results = DB::select("
        SELECT *
        FROM getInfo AS a
        WHERE a.id = ?", [$userID]);

    return $results[0] ?? null;
}

/**
 * تسجيل الدخول برمز التحقّق وحده — بلا كلمة مرور.
 *
 * سبب وجودها: تصميم التطبيق لا يحتوي على شاشة كلمة مرور إطلاقاً، بينما
 * login و register يفرضانها. وبدل تخزين كلمة مرور في الجهاز، تتحقّق هذه
 * النقطة من الرمز **على الخادم** ثم تُصدر رمز Sanctum.
 *
 * وهي تغلق في الوقت نفسه ثغرة update/password: تلك النقطة تسمح بتغيير كلمة
 * المرور بمعرفة الهاتف ومعرّف الجهاز فقط، لأن خطوة الـ OTP مفروضة من العميل
 * لا من الخادم. هنا الرمز يُتحقَّق منه ويُستهلك في نفس المعاملة.
 *
 * وتعالج كذلك قفل إعادة التثبيت: الجهاز يُعاد ربطه بعد تحقّق OTP مؤكَّد،
 * فلا يحتاج المستخدم إلى إعادة تعيين Reg='NO' من المكتب الخلفي.
 */
public function otpLogin(Request $request)
{
    $validator = Validator::make($request->all(), [
        'phone'     => 'required|string|regex:/^9\d{8}$/',
        'CodeOtp'   => 'required|digits:4',
        'device_id' => 'required|string|max:255',
    ], [
        'phone.regex'    => 'رقم الهاتف يجب أن يكون 9 أرقام يبدأ بـ 9.',
        'CodeOtp.digits' => 'رمز التحقّق يجب أن يكون 4 أرقام.',
    ]);

    if ($validator->fails()) {
        return $this->sendError('خطأ في البيانات المُرسلة.', $validator->errors(), 422);
    }

    $phone     = $request->phone;                 // 9 خانات بلا بادئة
    $phoneTo   = '218' . $phone;                  // الصيغة المخزّنة في Code_OtpTB
    $deviceId  = $request->device_id;

    return DB::transaction(function () use ($phone, $phoneTo, $deviceId, $request) {

        // 1) الرمز — يُقرأ داخل قفل حتى لا يُستهلك مرتين على التوازي.
        $otp = DB::table('Code_OtpTB')
            ->where('UeserPohone', $phoneTo)
            ->where('CodeOtp', $request->CodeOtp)
            ->orderByDesc('ID')
            ->lockForUpdate()
            ->first();

        if (!$otp) {
            return $this->sendError('رمز التحقّق غير صحيح.', 'InvalidOtp', 422);
        }

        if (Carbon::parse($otp->ExpeaerTime)->isPast()) {
            DB::table('Code_OtpTB')->where('ID', $otp->ID)->delete();
            return $this->sendError('انتهت صلاحية رمز التحقّق. اطلب رمزاً جديداً.', 'ExpiredOtp', 422);
        }

        // 2) استهلاك الرمز — أحادي الاستخدام مهما كانت نتيجة ما بعده.
        DB::table('Code_OtpTB')->where('UeserPohone', $phoneTo)->delete();

        // 3) المستخدم. لا تسجيل ذاتي: الحساب يُنشأ من المكتب الخلفي.
        $user = User::where('phone', $phone)->first();

        if (!$user) {
            return $this->sendError('هذا الرقم غير مسجّل لدى الشركة.', 'NotProvisioned', 422);
        }

        if (!is_null($user->deleted_at)) {
            return $this->sendError('تم حذف الحساب.', 'UserDeleted', 404);
        }

        // 4) ربط الجهاز. الرمز تحقّق منه الخادم للتوّ، فإعادة الربط آمنة —
        //    وهي ما يجعل إعادة تثبيت التطبيق لا تقفل الحساب.
        $rebound = $user->device_id && $user->device_id !== $deviceId;

        $user->device_id = $deviceId;
        $user->Reg       = 'Yes';
        $user->save();

        // 5) جهاز واحد لكل مستخدم ⇒ إبطال الرموز السابقة.
        $user->tokens()->delete();

        $authorizedUser = AuthorizedUser::where('UserID', $user->id)->first();

        $success = [
            'token'      => $user->createToken('mobile-token')->plainTextToken,
            'user'       => $user,
            'info'       => $this->getInfo($user->id),
            'Name_post'  => $authorizedUser ? $authorizedUser->Name_post : null,
            'rebound'    => $rebound,
        ];

        return $this->sendResponse($success, 'تم تسجيل الدخول بنجاح.');
    });
}

public function updatePassword(Request $request)
{
    // ✅ التحقق من المدخلات
    $validator = Validator::make($request->all(), [
        'phone' => 'required|string|regex:/^\d{8,15}$/',
        'password' => [
            'required',
            'string',
            'min:8',
            'regex:/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/'
        ],
        'device_id' => 'required|string|max:255'
    ], [
        'password.min' => 'يجب أن تتكون كلمة المرور من 8 أحرف على الأقل.',
        'password.regex' => 'يجب أن تحتوي كلمة المرور على حرف كبير، حرف صغير، رقم ورمز خاص على الأقل.',
    ]);
    
    if ($validator->fails()) {
        return $this->sendError('خطأ في التحقق من البيانات.', $validator->errors(), 422);
    }

    // ✅ التحقق من وجود المستخدم
    $user = DB::table('users')->where('phone', $request->phone)->first();

    if (!$user) {
        return $this->sendError('المستخدم غير موجود على النظام.', [], 404);
    }

    if ($user->device_id !=$request->device_id)
    {
        return $this->sendError('عذرا لايمكن تغير كلمة المرور من هذه الجهاز', [], 404);
    }

    // ✅ التحقق من التفعيل
    if ($user->Reg === 'NO') {
        return $this->sendError('المستخدم غير مفعل. لا يمكن تغيير كلمة المرور.', 'UserNotActivated', 403);
    }

    // ✅ بدء معاملة آمنة
    DB::beginTransaction();

    try {
        // ✅ تحديث كلمة المرور
        DB::update("
            UPDATE [dbo].[users]
            SET [password] = ?
            WHERE phone = ?
        ", [bcrypt($request->password), $request->phone]);

        // ✅ جلب نموذج المستخدم الكامل
        $userModel = User::where('phone', $request->phone)->first();

        // ✅ حذف جميع التوكنات السابقة
        $userModel->tokens()->delete();    
        DB::commit();
    } catch (\Exception $e) {
        DB::rollBack();
        return $this->sendError('حدث خطأ أثناء تحديث كلمة المرور.', $e->getMessage(), 500);
    }

    // ✅ إنشاء توكن جديد
    $updatedUser = User::where('phone', $request->phone)->first();
    $success['token'] = $updatedUser->createToken('mobile-token')->plainTextToken;
    $success['user'] = $updatedUser;
    $success['info'] = $this->getInfo($updatedUser->id);

    return $this->sendResponse(
        $success,
        'تم تغيير كلمة المرور بنجاح. تم تسجيل الخروج من جميع الأجهزة الأخرى.'
    );
}



public function sendMessageWithCurl(Request $request)
{
    $validator = Validator::make($request->all(), [
        'phone' => 'required|string',
        'message' => 'required|string',
        'xtoken' => 'required|string'
    ]);

    if ($validator->fails()) {
        return response()->json([
            'success' => false,
            'errors' => $validator->errors()
        ], 422);
    }

    // ✅ التحقق من xtoken
    $providedToken = $request->xtoken;
    $expectedToken = env('CUSTOM_X_TOKEN', 'your-secure-token-here');

    if ($providedToken !== $expectedToken) {
        return response()->json([
            'success' => false,
            'message' => 'Unauthorized: Invalid xtoken'
        ], 401);
    }

    // ✅ تجهيز رقم الهاتف وإضافة @c.us إن لم تكن موجودة
    $phoneRaw = preg_replace('/[^0-9]/', '', $request->phone);
    $chatId = str_ends_with($phoneRaw, '@c.us') ? $phoneRaw : $phoneRaw . '@c.us';

    // ✅ 1) التحقق من وجود الرقم على واتساب
    $checkUrl = "https://api.ultramsg.com/instance130356/contacts/check"
        . "?token=" . urlencode(env('WHATSAPP_TOKEN', 'ek352njuob4t2wh7'))
        . "&chatId=" . urlencode($chatId);

    try {
        // ⛔ الحل هنا: تعطيل التحقق من SSL لتفادي خطأ cURL error 60
        $checkResponse = Http::withOptions(['verify' => false])
            ->timeout(10)
            ->get($checkUrl);
    } catch (\Exception $e) {
        return response()->json([
            'success' => false,
            'message' => 'فشل الاتصال بخدمة التحقق من واتساب',
            'error' => $e->getMessage()
        ], 500);
    }

    if (!$checkResponse->successful()) {
        return response()->json([
            'success' => false,
            'message' => 'خدمة التحقق من واتساب أعادت خطأ',
            'status_code' => $checkResponse->status(),
            'body' => $checkResponse->body()
        ], 500);
    }

    $checkData = $checkResponse->json();

    // ✅ إذا لم يكن الرقم موجود على واتساب
    if (!isset($checkData['status']) || $checkData['status'] !== 'valid') {
        return response()->json([
            'success' => false,
            'message' => 'الرقم غير موجود على واتساب',
            'data' => $checkData
        ], 404);
    }

    // ✅ 2) إرسال الرسالة إذا الرقم فعلاً على واتساب
    $sendUrl = "https://api.ultramsg.com/instance130356/messages/chat?token=" 
        . urlencode(env('WHATSAPP_TOKEN', 'ek352njuob4t2wh7'));

    $postData = http_build_query([
        'to'   => $chatId,
        'body' => $request->message
    ]);

    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $sendUrl);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, $postData);
    curl_setopt($ch, CURLOPT_HTTPHEADER, ['Content-Type: application/x-www-form-urlencoded']);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false); // ⛔ لمنع خطأ SSL
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);

    $response = curl_exec($ch);
    $curlErrNo = curl_errno($ch);
    $curlErr = curl_error($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    if ($curlErrNo) {
        return response()->json([
            'success' => false,
            'message' => 'خطأ أثناء إرسال الرسالة باستخدام cURL',
            'curl_errno' => $curlErrNo,
            'curl_error' => $curlErr
        ], 500);
    }

    if ($httpCode < 200 || $httpCode >= 300) {
        return response()->json([
            'success' => false,
            'message' => 'خدمة إرسال الرسائل أعادت رمز خطأ',
            'http_code' => $httpCode,
            'body' => $response
        ], 500);
    }

    return response()->json([
        'success' => true,
        'message' => 'تم إرسال الرسالة بنجاح 🎉',
        'check_status' => $checkData,
        'send_result_raw' => json_decode($response, true) ?? $response
    ]);
}

///////////////////////اعادة تفعيل الكود///////////////////////////////////////
public function reActivate(Request $request)
{
    // 👈 ابحث عن المستخدم بأي طريقة (بالبريد، أو بالـ ID، أو غيره)
    $user = User::where('id', $request->user_id)->first();

    if (!$user) {
        return response()->json([
            'status' => false,
            'message' => 'المستخدم غير موجود'
        ], 404);
    }

    // 🗑️ حذف جميع التوكينات السابقة
    $user->tokens()->delete();

    // 🎟️ إنشاء توكين جديد
    $newToken = $user->createToken('mobile-token')->plainTextToken;

    return response()->json([
        'status' => true,
        'message' => 'تمت إعادة التفعيل بنجاح، وتم تسجيل الخروج من كل الأجهزة',
        'token' => $newToken
    ]);
}

}
