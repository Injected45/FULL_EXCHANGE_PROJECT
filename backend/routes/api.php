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
use App\Http\Controllers\Api\ChatController;
use App\Http\Controllers\Api\EmployeeChatController;
use App\Http\Controllers\Api\CompanyBrandingController;
use App\Http\Controllers\Api\EmployeeActivationController;
use App\Http\Controllers\Api\EmployeeAdminController;
use App\Http\Controllers\Api\EmployeeApprovalController;
use App\Http\Controllers\Api\EmployeeController;
use App\Http\Controllers\Api\EmployeeReportsController;
use App\Http\Controllers\Api\SupportAuthController;
use App\Http\Controllers\Api\SupportController;
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
    // تقييدُ المعدّل: لا Rate Limiting كان في المنظومة، وOTP ٤ خانات بلا
    // عدّاد يعني تخمينَ الحساب في دقائق. مع مهلة OTP (٣ دقائق) يصير المجالُ
    // غيرَ قابلٍ للاستنفاد داخل نافذةٍ واحدة. المخزنُ ملفّيّ (انظر config/cache).
    Route::post('device/register', 'register')->middleware('throttle:10,1');
    Route::post('device/login', 'login')->name('login')->middleware('throttle:10,1');
    Route::post('device/reActivate', 'reActivate')->middleware('throttle:10,1');
    //////////////ارسال التوكين للتحقق من انة هذه الرمز يمكن يتم او لا//////////////////
Route::post('device/initAuth',  'initAuth');
});
////////////////////////////اضافة اشعار من خلال المنظومة للقراءاة////////////////////////////////////////////////
Route::post('device/storeNavction', [MobiledepositController::class, 'storeNavction']);

/* تغييرُ كلمة المرور — **خلف الجلسة**، وأُغلقت في 9 سبتمبر 2026.
 *
 * ⚠ كانت مفتوحةً بلا مصادقة: يكفي معرفةُ رقم الهاتف ومعرّف الجهاز
 * لتعيين كلمة مرورٍ جديدة، ثمّ الدخولُ بها من `device/login`. وذلك
 * استيلاءٌ كامل على حسابٍ ماليّ بلا رمز تحقّقٍ ولا جلسة.
 *
 * والتوثيقُ يسمّيها ثغرةً منذ إضافة `otp/login` (انظر تعليقَها في
 * `AuthController`، وتعليقَ `auth_repository.dart` في التطبيق) — ومع
 * ذلك بقي المسارُ مفتوحاً. ولا شيءَ في المشروع يناديه: لا تطبيقُ
 * الوكيل ولا التطبيقُ المكتبيّ ولا مركزُ الدعم.
 *
 * ⚠ والحارسُ طبقتان لا واحدة: الجلسةُ هنا، وتقييدُ التغيير بصاحبها
 * في الدالّة نفسِها — وإلّا غيّر وكيلٌ داخلٌ كلمةَ مرور وكيلٍ آخر. */
Route::post('device/update/password',
    [ MobileAuthController::class , 'updatePassword'])->middleware('auth:sanctum');
Route::get('device/send-notification', [NotificationController::class, 'send']);

Route::get('/user', function (Request $request) {
    // return $request->user();
})->middleware('auth:sanctum');



////////////////////////////////////////////////كود ارسال Otb ///////////////////////////////////////////////////////////////
// otp/send مقيَّدٌ أشدّ: كلُّ نداءٍ يُرسل واتساب حقيقياً ويحذف رموزَ الهاتف.
Route::post('device/otp/send', [OtpController::class, 'sendOtp'])->middleware('throttle:5,1');
Route::post('device/otp/checkOtp', [OtpController::class, 'checkOtp'])->middleware('throttle:10,1');
////تسجيل الدخول بالرمز وحده — يتحقّق الخادم من الـ OTP ثم يُصدر رمز Sanctum
Route::post('device/otp/login', [MobileAuthController::class, 'otpLogin'])->middleware('throttle:10,1');
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


/* ── تفعيل الموظف ─────────────────────────────────────────────────────
 * خارج التوثيق عمداً: الموظف لا يملك رمزاً بعد. حمايتها الطبقات الثلاث —
 * كود الإدارة + رمز التحقّق + ربط الجهاز — وحدّ معدّل على الرقم.
 */
Route::post('device/employee/activation/request',
    [ EmployeeActivationController::class , 'requestOtp' ]);
/* مسحُ رمز QR — الخطوةُ الأولى نفسُها بتمثيلٍ آخر، وتنتهي إلى
   `requestOtp` عينِها. خارج `auth:sanctum` كأختِها: الموظف لا رمزَ له بعد. */
Route::post('device/employee/activation/qr',
    [ EmployeeActivationController::class , 'requestByQr' ]);

Route::post('device/employee/activation/verify',
    [ EmployeeActivationController::class , 'verifyOtp'  ]);

/* مسارات الموظف بعد التفعيل — حارسها `employee` لا `auth:sanctum`.
 *
 * الصلاحية تُكتب في الوسيط نفسه: `employee:KEY`. وهي **الحارس الحقيقي** —
 * إخفاء الزرّ في التطبيق تجميل، والرفض هنا. وما لا صلاحية له يعود 403
 * ويُسجَّل في السجلّ الأمني.
 */
Route::middleware('employee')->group(function () {
    Route::get ('device/employee/me',     [ EmployeeController::class , 'me' ]);
    Route::post('device/employee/logout', [ EmployeeActivationController::class , 'logout' ]);
});

Route::get ('device/employee/transfers/incoming',
    [ EmployeeController::class , 'incoming' ])
    ->middleware('employee:VIEW_INCOMING_TRANSFERS');

Route::post('device/employee/transfers/{id}/deliver',
    [ EmployeeController::class , 'deliver' ])
    ->middleware('employee:DELIVER_TRANSFER')->whereNumber('id');

/* ── ما يراه الموظف من الحوالات ───────────────────────────────────────
 *
 * ⚠ ثلاثةُ مفاتيح لا مفتاح: من يبحث عن حوالةِ زبونٍ واقفٍ أمامه ليس
 * بالضرورة من يُطلَّع على ما فعله زملاؤه على نقطة البيع.
 *
 * وكلُّها قراءةٌ خالصة — لا تكتب حرفاً في أي دفتر.
 */
/* ── بياناتُ المراجع لشاشة الإنشاء ─────────────────────────────────────
 *
 * الدولُ والمدنُ والفروع. وهي **الدوالُّ نفسُها** التي يقرأ منها الوكيل —
 * لا نسخةٌ ثانية — ولا تقرأ هويّةَ المستخدم أصلاً (مفحوصٌ: صفرُ نداءات
 * `Auth::user()` في ثلاثتها). فالمشترَكُ بينها وبين الوكيل هو الشيفرة،
 * والمختلفُ هو الحارس.
 *
 * ومفتاحُها `CREATE_TRANSFER`: من لا يملك الإنشاء لا حاجة له بقائمة
 * الفروع، وفتحُها له توسيعٌ بلا سبب.
 */
Route::post('device/employee/ref/countries',
    [ EmployeeController::class , 'refCountries' ])
    ->middleware('employee:CREATE_TRANSFER');

Route::post('device/employee/ref/cities',
    [ EmployeeController::class , 'refCities' ])
    ->middleware('employee:CREATE_TRANSFER');

Route::get ('device/employee/ref/branches',
    [ EmployeeController::class , 'refBranches' ])
    ->middleware('employee:CREATE_TRANSFER');
/* ⚠ إنشاءُ الحوالة — الوحيدُ في تطبيق الموظف الذي **يكتب في الدفتر**.
 *
 * بإذن المالك الصريح (7 سبتمبر 2026)، وبشرطه: الموظف واجهةٌ للوكيل لا
 * كيانٌ ماليّ ثانٍ. والمسارُ لا يحمل منطقاً مالياً — ينادي دالّة الوكيل
 * نفسَها بهويّته. انظر `EmployeeActsAsAgent`.
 */
Route::post('device/employee/transfers/create',
    [ EmployeeController::class , 'createTransfer' ])
    ->middleware('employee:CREATE_TRANSFER');
Route::get ('device/employee/transfers/search',
    [ EmployeeController::class , 'searchTransfer' ])
    ->middleware('employee:SEARCH_TRANSFER');

/*
 * طلباتُ الموافقة التي أنشأها الموظف — قراءةٌ وإلغاءٌ لطلبه هو.
 *
 * ⚠ ولا اعتمادَ هنا ولا رفض: القرارُ خلف جلسة الوكيل، وهذه جلسةُ موظف.
 * فالفصلُ بابٌ مغلقٌ لا شرطٌ في الشيفرة (البند 11).
 *
 * وتحت صلاحية إنشاء الحوالة نفسِها: من لا يُنشئ حوالةً لا طلباتِ له.
 */
/*
 * هويّةُ شركة الوكيل — تقرؤها فواتيرُ الموظف وطباعتُه.
 *
 * ⚠ بلا صلاحية: فاتورةٌ باسمِ شركةٍ أخرى في يد الزبون ليست حمايةً.
 */
Route::get ('device/employee/branding',
    [ EmployeeController::class , 'branding' ])
    ->middleware('employee');

/*
 * تقاريرُ الموظف وأرصدتُه ومفضّلتُه — كلٌّ خلف صلاحيته وحدَه.
 *
 * ⚠ **قراءةٌ خالصة**: لا مسارَ هنا يكتب في جدولٍ ماليّ. والأرصدةُ تُقرأ
 * بمسار الوكيل نفسِه، فالرقمُ واحدٌ لا رقمان.
 *
 * ⚠ ولا بوّابةَ واحدة لكلّ التقارير: `REPORTS_VIEW` يفتح القسم، وكلُّ
 * تقريرٍ داخله يحتاج مفتاحَه — فوكيلٌ يُري موظّفَه حوالاتِ يومه دون رصيده
 * يستطيع ذلك، ولو كانت كتلةً واحدة لَما استطاع.
 */
/*
 * ما في عهدة الموظف الآن — يُعرض تحت اسمه في شاشته الرئيسية.
 *
 * ⚠ تحت `VIEW_OWN_CASHBOX`: من يرى خزينتَه يرى ما فيها. ولا صلاحيةَ
 * جديدة لرقمٍ هو خلاصةُ ما يراه أصلاً.
 */
Route::get ('device/employee/custody',
    [ EmployeeController::class , 'custody' ])
    ->middleware('employee:VIEW_OWN_CASHBOX');

/*
 * كشفُ حساب حوالات الموظف — ما أنشأ وما سلّم في كشفٍ واحد، للجرد.
 *
 * ⚠ والنقدُ في يده واحد، فكشفُه واحد: تقريران منفصلان لا يُجرَد عليهما.
 */
Route::get ('device/employee/statement',
    [ EmployeeController::class , 'transferStatement' ])
    ->middleware('employee:VIEW_OWN_TRANSFERS');

Route::get ('device/employee/reports/daily',
    [ EmployeeController::class , 'reportDaily' ])
    ->middleware('employee:REPORT_DAILY_TRANSFERS');

Route::get ('device/employee/reports/delivered',
    [ EmployeeController::class , 'reportDelivered' ])
    ->middleware('employee:REPORT_DELIVERED_TRANSFERS');

Route::get ('device/employee/reports/pending',
    [ EmployeeController::class , 'reportPending' ])
    ->middleware('employee:REPORT_PENDING_TRANSFERS');

Route::get ('device/employee/reports/cashbox',
    [ EmployeeController::class , 'reportCashbox' ])
    ->middleware('employee:REPORT_EMPLOYEE_CASHBOX');

Route::get ('device/employee/reports/point-of-sale',
    [ EmployeeController::class , 'reportPointOfSale' ])
    ->middleware('employee:REPORT_POINT_OF_SALE');

Route::get ('device/employee/reports/audit',
    [ EmployeeController::class , 'reportAudit' ])
    ->middleware('employee:REPORT_AUDIT');

Route::get ('device/employee/reports/agent-balance',
    [ EmployeeController::class , 'reportAgentBalance' ])
    ->middleware('employee:REPORT_AGENT_BALANCE');

/* الملخّصُ الماليّ — تجميعُ ما تقوله التقاريرُ نفسُها، لا حسابٌ ثانٍ. */
Route::get ('device/employee/summary',
    [ EmployeeController::class , 'financialSummary' ])
    ->middleware('employee:VIEW_FINANCIAL_SUMMARY');

/* رصيدُ الوكيل — يقرؤه الموظف ولا يمسّه. */
Route::get ('device/employee/balance',
    [ EmployeeController::class , 'agentBalance' ])
    ->middleware('employee:VIEW_AGENT_TOTAL_BALANCE');

/* المستفيدون المفضّلون — العرضُ والإدارةُ صلاحيتان لا واحدة. */
Route::post('device/employee/favorites',
    [ EmployeeController::class , 'favorites' ])
    ->middleware('employee:VIEW_FAVORITES');

Route::post('device/employee/favorites/add',
    [ EmployeeController::class , 'favoriteAdd' ])
    ->middleware('employee:MANAGE_FAVORITES');

Route::post('device/employee/favorites/delete',
    [ EmployeeController::class , 'favoriteDelete' ])
    ->middleware('employee:MANAGE_FAVORITES');

Route::get ('device/employee/approvals',
    [ EmployeeController::class , 'myApprovals' ])
    ->middleware('employee:CREATE_TRANSFER');
/*
 * تنفيذُ ما أذن به الوكيل — بيد الموظف، وفي خزينته.
 *
 * ⚠ تحت `CREATE_TRANSFER` نفسِها: من سُحبت منه صلاحيةُ الإنشاء بعد
 * الموافقة لا ينفّذ — والموافقةُ لا تمنح صلاحيةً (البند 45).
 */
Route::post('device/employee/approvals/{id}/execute',
    [ EmployeeController::class , 'executeApproval' ])
    ->middleware('employee:CREATE_TRANSFER')->whereNumber('id');

Route::post('device/employee/approvals/{id}/cancel',
    [ EmployeeController::class , 'cancelApproval' ])
    ->middleware('employee:CREATE_TRANSFER')->whereNumber('id');

Route::get ('device/employee/transfers/mine',
    [ EmployeeController::class , 'myTransfers' ])
    ->middleware('employee:VIEW_OWN_TRANSFERS');

Route::get ('device/employee/transfers/point-of-sale',
    [ EmployeeController::class , 'posTransfers' ])
    ->middleware('employee:VIEW_POS_TRANSFERS');
Route::get ('device/employee/cashbox',
    [ EmployeeController::class , 'cashbox' ])
    ->middleware('employee:VIEW_OWN_CASHBOX');

/* كشفُ حركة الخزينة — للجرد: الرصيدُ بعد كل حركة، وفلترةٌ بالتاريخ والنوع.
 *
 * ⚠ الصلاحيةُ نفسُها لأنه السؤالُ نفسُه بتفصيلٍ أوفى — ولا صلاحيةٌ ثانية
 * تجعل موظفاً يرى خزينتَه ولا يرى حركاتها. وقراءةٌ خالصة. */
Route::get ('device/employee/cashbox/ledger',
    [ EmployeeController::class , 'cashboxLedger' ])
    ->middleware('employee:VIEW_OWN_CASHBOX');

Route::post('device/employee/cashbox/entry',
    [ EmployeeController::class , 'addEntry' ])
    ->middleware('employee:CASHBOX_ENTRY');

Route::post('device/employee/shift/start',
    [ EmployeeController::class , 'startShift' ])
    ->middleware('employee:START_SHIFT');

Route::post('device/employee/shift/close',
    [ EmployeeController::class , 'closeShift' ])
    ->middleware('employee:CLOSE_SHIFT');

// دردشة الموظّف مع وكيله. صلاحيةٌ تُمنح كسائرها — لا شيء مفتوح افتراضاً.
Route::get ('device/employee/chat',
    [ EmployeeChatController::class , 'messages' ])
    ->middleware('employee:CHAT_WITH_AGENT');

Route::post('device/employee/chat',
    [ EmployeeChatController::class , 'send' ])
    ->middleware('employee:CHAT_WITH_AGENT');

Route::get ('device/employee/chat/unread',
    [ EmployeeChatController::class , 'unread' ])
    ->middleware('employee:CHAT_WITH_AGENT');

Route::delete('device/employee/chat/{messageId}',
    [ EmployeeChatController::class , 'destroy' ])
    ->middleware('employee:CHAT_WITH_AGENT')->whereNumber('messageId');

Route::post('device/employee/chat/{messageId}/react',
    [ EmployeeChatController::class , 'react' ])
    ->middleware('employee:CHAT_WITH_AGENT')->whereNumber('messageId');

Route::put ('device/employee/chat/{messageId}',
    [ EmployeeChatController::class , 'edit' ])
    ->middleware('employee:CHAT_WITH_AGENT')->whereNumber('messageId');

Route::post('device/employee/chat/{messageId}/star',
    [ EmployeeChatController::class , 'star' ])
    ->middleware('employee:CHAT_WITH_AGENT')->whereNumber('messageId');

Route::post('device/employee/chat/{messageId}/pin',
    [ EmployeeChatController::class , 'pin' ])
    ->middleware('employee:CHAT_WITH_AGENT')->whereNumber('messageId');

Route::post('device/employee/chat/typing',
    [ EmployeeChatController::class , 'typing' ])
    ->middleware('employee:CHAT_WITH_AGENT');

Route::get ('device/employee/chat/attachment/{name}',
    [ EmployeeChatController::class , 'attachment' ])
    ->middleware('employee:CHAT_WITH_AGENT')->where('name', '[A-Za-z0-9._-]+');


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

  // جرس التنبيه: أرقام الصفوف وحدها، يسألها التطبيق دورياً.
  Route::get('agent/incoming-transfers/alerts',
      [ AgentIncomingTransfersController::class , 'alerts' ]);

  //
  // -- الدردشة: الوكيل مع الإدارة، والوكيل مع موظّفيه --------------------
  //
  // طبقة تواصل لا طبقة مالية: لا رصيد ولا قيد ولا حوالة. والوكيل يُشتقّ من
  // التوثيق ولا يُقرأ من الطلب، وكل استعلام مقيَّد به.
  Route::get ('chat/threads',                  [ ChatController::class , 'threads' ]);
  Route::post('chat/threads/employee',         [ ChatController::class , 'openEmployee' ]);
  Route::get ('chat/unread',                   [ ChatController::class , 'unread' ]);
  Route::get ('chat/threads/{id}/messages',    [ ChatController::class , 'messages' ])->whereNumber('id');
  Route::post('chat/threads/{id}/messages',    [ ChatController::class , 'send' ])->whereNumber('id');
  Route::delete('chat/threads/{id}/messages/{messageId}',
      [ ChatController::class , 'destroy' ])->whereNumber('id')->whereNumber('messageId');

  // المرفق داخل التوثيق: صور إيصالات ووثائق عملاء، لا شعار شركة.
  // والاسم مقيَّد بالشكل هنا وبـ basename في المتحكّم.
  Route::get ('chat/attachment/{name}',        [ ChatController::class , 'attachment' ])
      ->where('name', '[A-Za-z0-9._-]+');

  // مزايا الرسالة والمحادثة — كلّها تبدأ بحارس `guard()` في المتحكّم.
  Route::post('chat/threads/{id}/messages/{messageId}/react',
      [ ChatController::class , 'react' ])->whereNumber('id')->whereNumber('messageId');
  Route::put ('chat/threads/{id}/messages/{messageId}',
      [ ChatController::class , 'edit' ])->whereNumber('id')->whereNumber('messageId');
  Route::post('chat/threads/{id}/messages/{messageId}/pin',
      [ ChatController::class , 'pin' ])->whereNumber('id')->whereNumber('messageId');
  Route::post('chat/threads/{id}/messages/{messageId}/star',
      [ ChatController::class , 'star' ])->whereNumber('id')->whereNumber('messageId');
  // إعادة التوجيه: المحادثتان تُفحصان معاً في المتحكّم — المصدر والوجهة.
  Route::post('chat/threads/{id}/messages/{messageId}/forward',
      [ ChatController::class , 'forward' ])->whereNumber('id')->whereNumber('messageId');
  Route::put ('chat/threads/{id}/settings',
      [ ChatController::class , 'settings' ])->whereNumber('id');
  Route::post('chat/threads/{id}/typing',
      [ ChatController::class , 'typing' ])->whereNumber('id');
  Route::get ('chat/search',                   [ ChatController::class , 'search' ]);
  Route::get ('chat/starred',                  [ ChatController::class , 'starred' ]);

  Route::get('agent/outgoing-transfers/pending',
      [ AgentIncomingTransfersController::class , 'pendingOutgoing' ]);

  Route::get('agent/outgoing-transfers/{code}',
      [ AgentIncomingTransfersController::class , 'outgoingByCode' ])->where('code', '[A-Za-z0-9-]+');

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

  //
  // -- إدارة الموظفين ونقاط البيع: للحساب الرئيسي وحده --------------------
  //
  // الوكيل يُشتقّ من التوثيق ولا يُقرأ من الطلب، وكل استعلام مقيَّد به.
  // ونقاط البيع تُقرأ من `AuthorizedUsers` القائم — لا جدول موازٍ.
  Route::get ('employees',                      [ EmployeeAdminController::class , 'index' ]);
  Route::post('employees',                      [ EmployeeAdminController::class , 'store' ]);
  Route::get ('employees/permissions/catalog',  [ EmployeeAdminController::class , 'permissionCatalog' ]);
  Route::get ('employees/points-of-sale',       [ EmployeeAdminController::class , 'pointsOfSale' ]);
  Route::get ('employees/devices',              [ EmployeeAdminController::class , 'devices' ]);
  Route::post('employees/devices/{id}/revoke',  [ EmployeeAdminController::class , 'revokeDevice' ])->whereNumber('id');
  Route::put ('employees/{id}',                 [ EmployeeAdminController::class , 'update' ])->whereNumber('id');
  /*
   * حذفُ الموظف — ناعمٌ دائماً (انظر `destroy`).
   *
   * ⚠ ويحرّر رقمَ الهاتف لإعادة الإضافة: من أدخل رقماً خطأً لم يكن
   * يملك سبيلاً لتصحيحه من التطبيق قبل هذا — الإيقافُ يُبقي الرقم
   * محجوزاً، فيصطدم بالفهرس الفريد عند إعادة الإضافة.
   */
  Route::delete('employees/{id}',
      [ EmployeeAdminController::class , 'destroy' ])->whereNumber('id');

  Route::post('employees/{id}/status',          [ EmployeeAdminController::class , 'setStatus' ])->whereNumber('id');
  Route::post('employees/{id}/activation-code', [ EmployeeAdminController::class , 'issueCode' ])->whereNumber('id');
  Route::post('employees/{id}/activation-code/revoke', [ EmployeeAdminController::class , 'revokeCode' ])->whereNumber('id');

  /*
   * سقفُ الموظف وطلباتُ الموافقة — واجهةُ الوكيل.
   *
   * ⚠ داخل `auth:sanctum` عمداً: الموافقةُ فعلُ وكيلٍ لا فعلُ موظف،
   * وجلسةُ الموظف لا تُنتج رمزَ Sanctum أصلاً — فلا يبلغ هذه المسارات.
   */
  Route::get ('employees/approvals',       [ EmployeeApprovalController::class , 'index' ]);
  Route::get ('employees/approvals/count', [ EmployeeApprovalController::class , 'count' ]);
  Route::post('employees/approvals/{id}/approve',
      [ EmployeeApprovalController::class , 'approve' ])->whereNumber('id');
  Route::post('employees/approvals/{id}/reject',
      [ EmployeeApprovalController::class , 'reject' ])->whereNumber('id');

  Route::get ('employees/{id}/limits',
      [ EmployeeApprovalController::class , 'showLimits' ])->whereNumber('id');
  Route::put ('employees/{id}/limits',
      [ EmployeeApprovalController::class , 'updateLimits' ])->whereNumber('id');
  Route::put ('employees/{id}/permissions',     [ EmployeeAdminController::class , 'setPermissions' ])->whereNumber('id');

  // تقارير تشغيلية — قراءة فقط، ولا تمسّ رصيداً ولا قيداً.
  Route::get('employees/dashboard',
      [ EmployeeReportsController::class , 'dashboard' ]);
  /* من أنشأ كم حوالة، ومن أي نقطة بيع — قراءةٌ من جدول النسب وحده.
     ولا يُقرأ هنا دفترٌ ماليّ ولا تُجمع منه قيمة. */
  Route::get('employees/reports/created-transfers',
      [ EmployeeReportsController::class , 'createdTransfers' ]);

  Route::get('employees/{id}/transfers',
      [ EmployeeReportsController::class , 'employeeTransfers' ])->whereNumber('id');

  Route::get('employees/reports/points-of-sale',
      [ EmployeeReportsController::class , 'pointsOfSale' ]);
  Route::get('employees/{id}/statement',
      [ EmployeeReportsController::class , 'employeeStatement' ])->whereNumber('id');

});

/*
|--------------------------------------------------------------------------
| مركز «الرحالة للدعم الفني»
|--------------------------------------------------------------------------
|
| واجهة الخادم لتطبيق React الذي يجلس عليه موظّفو دعم الرحالة. يحلّ محلّ
| صفحة `/admin/chat` ومفتاحها المشترك: حسابٌ لكل موظّف، ودورٌ، وصلاحياتٌ
| ممنوحة صفّاً صفّاً.
|
| ⚠ **Default Deny**: كل مسارٍ يحمل صلاحيته في وسمه (`support:KEY`). مسارٌ
| بلا وسمٍ يعني «يكفي أن تكون داخل النظام» — ولا يُكتب هكذا إلا حين يكون
| ذلك هو المقصود فعلاً (الخروج، تغيير كلمة المرور، بياناتُ نفسه).
|
| ⚠ ولا مسار هنا يمسّ المال: لا رصيد ولا حوالة ولا قيد ولا خزينة. ومحادثة
| الوكيل مع موظّفه لا تُفتح من أيٍّ منها — الشرط `kind = ADMIN` في المتحكّم.
|
*/
Route::prefix('support')->group(function () {

    // خارج الحارس بطبيعتها.
    Route::post('auth/login', [SupportAuthController::class, 'login']);

    Route::middleware('support')->group(function () {
        Route::post('auth/logout',   [SupportAuthController::class, 'logout']);
        Route::get ('auth/me',       [SupportAuthController::class, 'me']);
        Route::post('auth/password', [SupportAuthController::class, 'changePassword']);
    });

    // ── صندوق الوارد ───────────────────────────────────────────────
    Route::middleware('support:VIEW_THREADS')->group(function () {
        Route::get('threads',           [SupportController::class, 'threads']);
        Route::get('threads/unread',    [SupportController::class, 'unread']);
        Route::get('assignees',         [SupportController::class, 'assignees']);
        Route::get('attachment/{name}', [SupportController::class, 'attachment'])
            ->where('name', '[A-Za-z0-9_.-]+');
        Route::get('threads/{id}',      [SupportController::class, 'messages'])->whereNumber('id');
    });

    Route::get('search', [SupportController::class, 'search'])
        ->middleware('support:SEARCH_MESSAGES');

    // ── الردّ ──────────────────────────────────────────────────────
    //
    // فحصُ المرفقات أدقّ من الوسم: `SEND_ATTACHMENT` و`SEND_VOICE` يُفحصان
    // داخل المتحكّم بحسب نوع الملفّ، لأن المسار واحدٌ والنوعان مختلفان.
    Route::middleware('support:REPLY')->group(function () {
        Route::post('threads/{id}/messages', [SupportController::class, 'send'])->whereNumber('id');
        Route::post('threads/{id}/typing',   [SupportController::class, 'typing'])->whereNumber('id');
        Route::post('threads/{id}/messages/{mid}/react', [SupportController::class, 'react'])
            ->whereNumber('id')->whereNumber('mid');
        Route::post('threads/{id}/messages/{mid}/star',  [SupportController::class, 'star'])
            ->whereNumber('id')->whereNumber('mid');
    });

    Route::put ('threads/{id}/messages/{mid}', [SupportController::class, 'edit'])
        ->middleware('support:EDIT_OWN_MESSAGE')->whereNumber('id')->whereNumber('mid');
    Route::post('threads/{id}/messages/{mid}/pin', [SupportController::class, 'pin'])
        ->middleware('support:PIN_MESSAGE')->whereNumber('id')->whereNumber('mid');
    Route::post('threads/{id}/messages/{mid}/forward', [SupportController::class, 'forward'])
        ->middleware('support:FORWARD_MESSAGE')->whereNumber('id')->whereNumber('mid');

    // ── الإسناد والحالة ────────────────────────────────────────────
    //
    // بلا وسمٍ هنا عمداً: «لنفسي» و«لغيري» صلاحيتان مختلفتان، والتمييز
    // بينهما يحتاج جسم الطلب — فالفحص في المتحكّم حيث يُقرأ.
    Route::middleware('support')->group(function () {
        Route::post('threads/{id}/assign', [SupportController::class, 'assign'])->whereNumber('id');
        Route::post('threads/{id}/status', [SupportController::class, 'status'])->whereNumber('id');
    });

    // ── الإدارة ────────────────────────────────────────────────────
    Route::middleware('support:MANAGE_STAFF')->group(function () {
        Route::get   ('staff',                   [SupportController::class, 'staff']);
        Route::post  ('staff',                   [SupportController::class, 'createStaff']);
        Route::put   ('staff/{id}',              [SupportController::class, 'updateStaff'])->whereNumber('id');
        Route::delete('staff/{id}',              [SupportController::class, 'deleteStaff'])->whereNumber('id');
        Route::post  ('staff/{id}/password',     [SupportController::class, 'resetStaffPassword'])->whereNumber('id');
    });

    Route::put('staff/{id}/permissions', [SupportController::class, 'setPermissions'])
        ->middleware('support:MANAGE_PERMISSIONS')->whereNumber('id');

    Route::get('audit', [SupportController::class, 'auditLog'])
        ->middleware('support:VIEW_AUDIT');
});

/*
|--------------------------------------------------------------------------
| مركز الدعم — التشغيل: الأولوية والتصنيف والوسوم والشريط الزمني
|--------------------------------------------------------------------------
|
| (بنود المالك 3 · 4 · 7 · 12 · 17)
|
| ⚠ تطويرٌ فوق القائم: لا مسارَ هنا يُكرّر مساراً موجوداً، والملاحظةُ
| الداخلية تمرّ بمسار الإرسال نفسِه بعلامةٍ في الجسم — لا بمسارٍ ثانٍ.
|
| ⚠ ولا مسار يمسّ المال.
|
*/
Route::prefix('support')->group(function () {

    // التصنيفات والوسوم والأولويات — تُقرأ لكل من يفتح صندوق الوارد.
    Route::get('taxonomy', [SupportController::class, 'taxonomy'])
        ->middleware('support:VIEW_THREADS');

    Route::post('threads/{id}/priority', [SupportController::class, 'priority'])
        ->middleware('support:SET_PRIORITY')->whereNumber('id');

    Route::middleware('support:SET_CATEGORY')->group(function () {
        Route::post  ('threads/{id}/category',      [SupportController::class, 'category'])->whereNumber('id');
        Route::post  ('threads/{id}/tags',          [SupportController::class, 'addTag'])->whereNumber('id');
        Route::delete('threads/{id}/tags/{tagId}',  [SupportController::class, 'removeTag'])
            ->whereNumber('id')->whereNumber('tagId');
    });

    Route::get('threads/{id}/timeline', [SupportController::class, 'timeline'])
        ->middleware('support:VIEW_THREADS')->whereNumber('id');

    // إدارة التصنيفات — «بدون تعديل الكود كل مرة» (نصّ البند 4).
    Route::middleware('support:MANAGE_TAXONOMY')->group(function () {
        Route::post('taxonomy/categories',      [SupportController::class, 'createCategory']);
        Route::post('taxonomy/tags',            [SupportController::class, 'createTag']);
        Route::put ('taxonomy/categories/{id}', [SupportController::class, 'setCategoryActive'])->whereNumber('id');
        Route::put ('taxonomy/tags/{id}',       [SupportController::class, 'setTagActive'])->whereNumber('id');
    });
});

/*
|--------------------------------------------------------------------------
| مركز الدعم — الدفعة الثانية: لوحة القيادة · SLA · الحضور · منع التعارض
|--------------------------------------------------------------------------
|
| (بنود المالك 1 · 2 · 6 · 35 · 36)
|
| ⚠ لا مسارَ هنا يُكرّر موجوداً: حالةُ SLA والمشاهدون يعودان مع قراءة
| المحادثة نفسِها لا بنداءٍ ثانٍ — فالقراءة **هي** دليلُ الحضور.
|
| ⚠ ولا مسار يمسّ المال.
|
*/
Route::prefix('support')->group(function () {

    Route::get('dashboard', [SupportController::class, 'dashboard'])
        ->middleware('support:VIEW_DASHBOARD');

    Route::get('team', [SupportController::class, 'team'])
        ->middleware('support:VIEW_TEAM');

    // حالةُ الموظّف نفسِه: بلا صلاحية — كلُّ من دخل يملك أن يقول «مشغول».
    Route::post('me/presence', [SupportController::class, 'setPresence'])
        ->middleware('support');

    Route::get('sla', [SupportController::class, 'slaSettings'])
        ->middleware('support:VIEW_DASHBOARD');
    Route::put('sla/{priority}', [SupportController::class, 'updateSla'])
        ->middleware('support:MANAGE_SLA')
        ->where('priority', 'NORMAL|HIGH|URGENT|CRITICAL');

    /* ── الدفعة الثالثة: سير العمل ─────────────────────────────────────
     *
     * ⚠ ثلاثةُ مستويات من الحراسة هنا، وكلٌّ منها مقصود:
     *
     *   • القوالبُ والمسودّاتُ والمتابعاتُ بلا مفتاح — من دخل يملك أن
     *     يكتب لنفسه اختصاراً ومسودّةً وتذكيراً. ومفتاحٌ عليها يمنع
     *     ترتيبَ عملِ الموظّف لا يمنع قدرةً.
     *   • إنشاءُ قالبٍ **مشترَك** يُفحص داخل الخدمة لا في المسار: المسارُ
     *     واحدٌ للخاصّ والمشترَك، والفرقُ بينهما في جسم الطلب.
     *   • التأجيلُ والتصعيدُ والتسليمُ بمفاتيحها — كلٌّ منها يغيّر ما
     *     يراه الفريقُ كلُّه لا ما يراه صاحبُه.
     */

    Route::get ('saved-replies', [SupportController::class, 'savedReplies'])
        ->middleware('support');
    Route::post('saved-replies', [SupportController::class, 'createSavedReply'])
        ->middleware('support');
    Route::put ('saved-replies/{id}', [SupportController::class, 'updateSavedReply'])
        ->middleware('support')->whereNumber('id');
    Route::post('saved-replies/{id}/used', [SupportController::class, 'usedSavedReply'])
        ->middleware('support')->whereNumber('id');

    Route::put ('threads/{id}/draft', [SupportController::class, 'saveDraft'])
        ->middleware('support:VIEW_THREADS')->whereNumber('id');

    Route::post('threads/{id}/snooze', [SupportController::class, 'snooze'])
        ->middleware('support:SNOOZE_THREAD')->whereNumber('id');
    Route::post('threads/{id}/unsnooze', [SupportController::class, 'unsnooze'])
        ->middleware('support:SNOOZE_THREAD')->whereNumber('id');

    // التسليمُ إسنادٌ إلى غيرِك — فمفتاحُه مفتاحُ الإسناد نفسُه، ولا
    // يُخترع له ثانٍ يعطي القدرةَ ذاتَها باسمٍ آخر.
    Route::post('threads/{id}/handoff', [SupportController::class, 'handoff'])
        ->middleware('support:ASSIGN_OTHERS')->whereNumber('id');

    Route::post('threads/{id}/escalate', [SupportController::class, 'escalate'])
        ->middleware('support:ESCALATE')->whereNumber('id');

    Route::get ('followups', [SupportController::class, 'followups'])
        ->middleware('support');
    Route::post('threads/{id}/followup', [SupportController::class, 'addFollowup'])
        ->middleware('support:VIEW_THREADS')->whereNumber('id');
    Route::post('followups/{id}/done', [SupportController::class, 'doneFollowup'])
        ->middleware('support')->whereNumber('id');
    // نبضةُ «أنا أكتب» — الحضورُ العاديّ يُسجَّل مع قراءة المحادثة.
    Route::post('threads/{id}/viewing', [SupportController::class, 'viewing'])
        ->middleware('support:VIEW_THREADS')->whereNumber('id');
});
