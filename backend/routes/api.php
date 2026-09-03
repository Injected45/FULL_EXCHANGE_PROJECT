<?php
use App\Http\Controllers\NotificationController;

use App\Events\NotificationSent;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\Api\AuthController as MobileAuthController;
use App\Http\Controllers\Api\depositController as MobiledepositController;
use App\Http\Controllers\Api\SmsController;
use App\Http\Controllers\Api\OtpController;
use App\Http\Controllers\Api\BankVisaTransferController;;
use App\Http\Controllers\Api\AgentIncomingTransfersController;
use App\Http\Controllers\Api\CompanyBrandingController;
Route::get('/user', function (Request $request) {
    return $request->user();
})->middleware('auth:sanctum');


////ارسال اشعار من خلال افيجول الاستوديو للاتطبيق /////////////////////
// routes/api.php
Route::post('device/send-notification-vbnet', function (Request $request) {
  $message = $request->input('message', 'رسالة افتراضية');
  event(new NotificationSent($message));
  return response()->json(['status' => 'Notification sent']);
});


Route::controller(MobileAuthController::class)->group(function(){
    Route::post('device/register', 'register');
    Route::post('device/login', 'login')->name('login');
    Route::post('device/reActivate', 'reActivate');
    //////////////ارسال التوكين للتحقق من انة هذه الرمز يمكن يتم او لا//////////////////
Route::post('device/initAuth',  'initAuth');
});
////////////////////////////اضافة اشعار من خلال المنظومة للقراءاة////////////////////////////////////////////////
Route::post('device/storeNavction', [MobiledepositController::class, 'storeNavction']);

 //  update Password
 Route::post('device/update/password',  [ MobileAuthController::class , 'updatePassword']   );
Route::get('device/send-notification', [NotificationController::class, 'send']);

Route::get('/user', function (Request $request) {
    // return $request->user();
})->middleware('auth:sanctum');



////////////////////////////////////////////////كود ارسال Otb ///////////////////////////////////////////////////////////////
Route::post('device/otp/send', [OtpController::class, 'sendOtp']);
Route::post('device/otp/checkOtp', [OtpController::class, 'checkOtp']) ;
////تسجيل الدخول بالرمز وحده — يتحقّق الخادم من الـ OTP ثم يُصدر رمز Sanctum
Route::post('device/otp/login', [MobileAuthController::class, 'otpLogin']);
/////////////////////////////////////////////////////////////////////////////////

Route::post('device/send/whatsapp/message',  [ MobileAuthController::class , 'sendMessageWithCurl']   );

  ////اضافة عميل من قبل  قاعد البيانت من خلال التطبيق

 Route::post('device/forgien/exchange/deposit/store',  [ MobiledepositController::class , 'storeaddcostmer']   );
  Route::post('device/countries',  [ MobiledepositController::class , 'getCountries']   );


  Route::post('device/cities',  [ MobiledepositController::class , 'GetCities']   );
  
  
Route::post('/send-sms', [SmsController::class, 'send']);
Route::post('/send-verification', [SmsController::class, 'sendVerification']);
///كود التحقق من رمز التحقق
Route::post('/check-verification', [SmsController::class, 'checkVerification']);



////جلب الشروط والاجكام ///////////////////////////////////////////////////////////////

Route::get('device/exchange/AppTerms_get',  [ MobiledepositController::class , 'AppTerms']   ) ;


// صورة شعار الشركة — خارج التوثيق عمداً، وسببه مشروح في المتحكّم.
// الاسم عشوائي، والقيد `[A-Za-z0-9_.-]+` يمنع أي فاصل مسار قبل أن يصل الاسم
// إلى الكود أصلاً.
Route::get('company/branding/logo/{name}',
    [ CompanyBrandingController::class , 'logo' ])
    ->where('name', '[A-Za-z0-9_.\-]+');


Route::middleware('auth:sanctum')->group(function ()
{


 //////////////////////////////////اضافة نقطة بيع ////////////////////////////////////////////////////////////// 
 Route::post('device/AuthorizedUsers_Add',  [ MobiledepositController::class , 'AuthorizedUsers_Add']   );
 Route::post('device/AuthorizedUsers_update',  [ MobiledepositController::class , 'AuthorizedUsers_update']   );
 Route::post('device/AuthorizedUsersgetByBranch',  [ MobiledepositController::class , 'AuthorizedUsersgetByBranch']   );
 
 /////////////////////////جلب معدل التحويل اليومي والاسبوعي والشهري والسنوي الخاص بكل مستخدم//////////////////////////////////////////////////////////////////////////////////////////////
Route::post('device/Daily_transfer',  [ MobiledepositController::class , 'Daily_transfer_preparer_schedule_DEttelse_GetUeser']   );
  /////////////////////////////////////حذف اليوزر الحالي او الغاء تفعيلة //////////////////////////////////////////////////////////////////////
  Route::post('device/dRIVER/Delete_Account',  [ MobileAuthController::class , 'Delete_Account']   );
///////////////////////الغاء حوالة من قبل المندوب////////////////////////////////////////////////////////////////

 Route::post('device/dRIVER/InternalExAddCancelReason',  [ MobiledepositController::class , 'InternalExAddCancelReason']   );

///////////////////////////////////////////////////////////////////////جلب حالات الاغاء من للسائق///////////////////////////////
  Route::post('device/dRIVER/AddCancelReason',  [ MobiledepositController::class , 'AddCancelReason']   );

  Route::POST('device/BankVisaTransfer', [BankVisaTransferController::class, "BankVisaTransfer_insert" ]);
///جلب الحوالات التي مع المندوب وغير مسملة///////////////////////////////////////////
Route::post('device/dRIVER/TaxiInvoiceDrivers_getInternalEx',  [ MobiledepositController::class , 'TaxiInvoiceDrivers_getInternalEx']   );

/////جلب الطلبات التي تمت الموافقه عليه من قبل المندوب //////////////////////////////////////////////////////////////////////////

Route::post('device/dRIVER/Request_to_summon_driversTB_Notvigtion',  [ MobiledepositController::class , 'Request_to_summon_driversTB_Notvigtion']   );
  ///كود قبول طلب من قبل التطبيق///////////////////////////////////////////////////////////////////////////////////////////////////////////
  Route::post('device/dRIVER/Request_to_summon_driversTB_Accipet',  [ MobiledepositController::class , 'Request_to_summon_driversTB_Accipet']   );

////--------------------كود عرض الاشعارات في التطبيق----------------------------------///////////////////////////

Route::post('device/dRIVER/Request_to_summon_driversTB_getnavction',  [ MobiledepositController::class , 'Request_to_summon_driversTB_getnavction']   );

 ///كود التسليم الحوالة الدالخلية
 Route::post('device/exchange/InternalEx_costimer',  [ MobiledepositController::class , 'InternalEx_costimer']   ) ;
 


 Route::post('device/exchange/InternalEx_SelectType_View_not_coustmers_get',  [ MobiledepositController::class , 'InternalEx_SelectType_View_not_coustmers']   ) ;
 Route::post('device/exchange/InternalEx_SelectType_View_statetosForok',  [ MobiledepositController::class , 'InternalEx_SelectType_View_statetosForok']   ) ;
 
 /////////////////////////////////////////طلب تاكسي للحوالة العادية للزبون////////////////////////////////////////////////////////////
 
 Route::post('device/Update_for_InternalEx_Taxi',  [ MobiledepositController::class , 'Update_for_InternalEx_Taxi']   ) ;
 
 

/////////////////////////////////////////////////////////////////////////////////////////////////////////

  Route::get('device/exchange/CoBranch_select_get',  [ MobiledepositController::class , 'CoBranch_select']   );


 Route::post('device/exchange/Favorites_Table_add',  [ MobiledepositController::class , 'Favorites_Table_inser']   );
  Route::post('device/exchange/Favorites_Table_delete_from',  [ MobiledepositController::class , 'Favorites_Table_delete']   );
////////////////////////////////////////////////
 Route::post('device/exchange/Favorites_ALL',  [ MobiledepositController::class , 'Favorites']   );

  


  Route::post('device/forgien/exchange/deposit/balance',  [ MobiledepositController::class , 'ForginDepositExchage']   );


 Route::post('device/forgien/exchange/deposit/account/statement',  [ MobiledepositController::class , 'ForginDepositExchageAS']   );



 
  Route::post('device/current/balance/local/currency',  [ MobiledepositController::class , 'getBalanceLocal']   );



 
  Route::post('device/service/external/transfer',  [ MobiledepositController::class , 'getServicesExternal']   );



  Route::get('device/local/account/statment',  [ MobiledepositController::class , 'LocalStatmentAccount']   );

 Route::post('device/exchange/account',  [ MobiledepositController::class , 'ExchangeAcc']   );
 ///////////////////////////////////////////////////////داله الحقق من الرصيد الخاص بالجاري///////////////////////////////////

 Route::get('device/Rollback_Branch_Trinsfrim_me/{branchID}/{val_value}',  [ MobiledepositController::class , 'Rollback_Branch_Trinsfrim_me']   );

 Route::get('device/exchange/accounts/data',  [ MobiledepositController::class , 'ExchangeAccData']   );
///جملة جلب الحولات الواردة للحساب العادي /////////////////////////////////////////////////////////////////////
 Route::get('device/exchange/accounts/ExchangeAccData_notACCid_Cosumer',  [ MobiledepositController::class , 'ExchangeAccData_notACCid_Cosumer']   );
 //  internal exchage
 Route::post('device/internal/exchange',  [ MobiledepositController::class , 'InternalExchange']   );


 Route::post('device/internal/exchange/time/check',  [ MobiledepositController::class , 'InternalEx_minut']   );


Route::post('device/internal/exchange/external/check',  [ MobiledepositController::class , 'checkTtans']   );


//////////////////جلب حساب العمولة//////////////////////////////////////////////////////////////////////////
Route::post('device/internal/CommtionRetview_get',  [ MobiledepositController::class , 'CommtionRetview_get']   );

Route::post('device/internal/trans/between/accounts',  [ MobiledepositController::class , 'transInsert']   );

Route::post('device/internal/trans/between/accounts/commission',  [ MobiledepositController::class , 'transBetweenAccountsCommission']   );


Route::post('device/internal/check/between/time',  [ MobiledepositController::class , 'check_between_time']   );


Route::post('device/external/get/exchange',  [ MobiledepositController::class , 'externalGetExchnage']   );

//get/exchange تعيد رقماً بوسيط أول خاطئ فلا يصلح لتسعير الزبون.
Route::post('device/external/quote',  [ MobiledepositController::class , 'externalQuote']   );



Route::post('device/external/time/exchange',  [ MobiledepositController::class , 'getDiffernceTimeExternal']   );

      
Route::post('device/external/insert/transfer',  [ MobiledepositController::class , 'transInsertExternal']   );


 

//  
Route::post('device/add/user/trans',  [ MobiledepositController::class , 'addUserTrans']   );


Route::post('device/list/user/trans',  [ MobiledepositController::class , 'ListUsersAddedToTrans']   );


Route::post('device/delete/user/trans',  [ MobiledepositController::class , 'deleteUser']   );

Route::post('device/otp/senotpGroupFr',  [ OtpController::class , 'senotpGroupFr']   );

/////بحث عن سكرين 
Route::post('device/searchPayment',  [ MobiledepositController::class , 'searchPayment']   );



  //
  // -- الحوالات الواردة للوكيل: متابعة تسليم لا حركة مالية --------------
  //
  // مفصولة تماماً عن حالة المنظومة (InternalEx.ConfirmType) بقرار المالك:
  // تلك تقوم عليها العمليات الحسابية ولا تُمسّ من التطبيق. وهذه للقراءة
  // ومساعدة الوكيل على معرفة ما سلّمه وما لم يسلّمه.
  Route::get('agent/incoming-transfers',
      [ AgentIncomingTransfersController::class , 'index' ]);
  Route::post('agent/incoming-transfers/{id}/deliver',
      [ AgentIncomingTransfersController::class , 'deliver' ])->whereNumber('id');

  //
  // -- هوية الشركة داخل التطبيق: طبقة عرض لا غير -------------------------
  //
  // لا مسار يقبل رقم الشركة من جسم الطلب: يُشتقّ من التوثيق في المتحكّم.
  // والتعديل للحساب الرئيسي وحده، ونقاط البيع تقرأ فقط.
  Route::get ('company/branding',       [ CompanyBrandingController::class , 'show'   ]);
  Route::put ('company/branding',       [ CompanyBrandingController::class , 'update' ]);
  Route::post('company/branding/logo',  [ CompanyBrandingController::class , 'uploadLogo' ]);
  Route::post('company/branding/reset', [ CompanyBrandingController::class , 'reset'  ]);

});
