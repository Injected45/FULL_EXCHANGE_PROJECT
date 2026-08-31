<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
 
use App\Models\User;
use App\Models\wallet;


use App\Models\Transfer_commissions;


use Illuminate\Support\Facades\Auth;
use Validator;
use App\Http\Controllers\BaseController;
use Illuminate\Support\Facades\Hash;
use Illuminate\Http\JsonResponse;
use App\Enums\ResponseEnums;
use Carbon\Carbon;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Http;
use App\Models\Navction;
use App\Helpers\ImageUploader;
use App\Services\Watsaoserversfrom;
use App\Services\AuthorizedUserService;
use App\Models\AuthorizedUser; // ✅ مهم
use App\Models\CommtionRetview; // ✅ مهم
use Illuminate\Validation\Rule; // ✅ مهم
use App\Services\SVSn8n_payments;

use Illuminate\Support\Str;

class depositController extends BaseController
{

 



    public function __construct(SVSn8n_payments $n8nPaymentsService) 
    {
         $this->n8nPaymentsService = $n8nPaymentsService;
         }
     /** * البحث عن عملية دفع */
      public function searchPayment(Request $request) 
      {

        $ueser = Auth::user();
        if (!$ueser) {
            return response()->json([
                'success' => false,
                'message' => "الرجاء تسجيل الدخول",
            ], 401);
        }
        $validator = Validator::make($request->all(), [
            'phone'       => 'required|string',
            'amount' => 'required|numeric|min:0',
            'ISID'      => 'required|string'
        ]);
    
        if ($validator->fails()) {
            return response()->json([
                'success' => false,
                'message' => 'فشل التحقق من البيانات',
                'errors'  => $validator->errors(),
            ], 422);
        }
        


         $phone = $request->input('phone'); 
         $amount = $request->input('amount'); 

         
       

         return
         response()->json( 
            $this->n8nPaymentsService->searchPayments(
                 $phone,
                  $amount ) 
        ); 
        } 

   

    

//////////////////////////////////جلب عمولات الفرع فقط////////////////////////////////////////////////////////////////
public function CommtionRetview_get(Request $request)
{
    $ueser = Auth::user();

    if (!$ueser) {
        return response()->json([
            'success' => false,
            'message' => "الرجاء تسجيل الدخول",
        ], 401);
    }

    $data = CommtionRetview::where('AccIDFrom', $ueser->AccID)
                ->orderBy('InsertDate', 'desc')
                ->get();

    if ($data->isEmpty()) {
        return response()->json([
            'success' => false,
            'message' => "لا يوجد بيانات",
        ], 404);
    }

    return response()->json([
        'success' => true,
        'data' => $data
    ]);
}
   
   /////////////////////////اضافة نقطة بيع ///////////////////////////////////////////
   public function AuthorizedUsers_Add(Request $request)
{
    try {

        $ueser = Auth::user();

        if (!$ueser) {
            return response()->json([
                'success' => false,
                'message' => "الرجاء تسجيل الدخول",
            ], 401);
        }

        if ($ueser->UeserType != 3) {
            return response()->json([
                'success' => false,
                'message' => "لا يوجد لديك صلاحية لهذه الخاصية",
            ], 403);
        }
        if ($ueser->AccountType !=='Main' )
        {
            return response()->json([
                'success' => false,
                'message' => "لا يوجد لديك صلاحية لهذه الخاصية",
            ], 403);

            

        }

        $request->validate([
            'Name'  => 'required|string|max:200',
            'phone' => 'required|string|max:9|unique:AuthorizedUsers,phone|unique:users,phone',
        ]);

        DB::beginTransaction();

        // 1️⃣ إدخال المستخدم
        DB::table('users')->insert([
            'phone'      => $request->phone,
            'Reg'        => 'NO',
            'created_at' => now(),
            'updated_at' => now(),
            'UeserType'  => 3,
            'BrancchID'  => $ueser->BrancchID,
            'Countries'  => 1,
            'AccID'      => $ueser->AccID ,
            'AccountType' =>'pos'
        ]);

        // 2️⃣ جلب userId حسب رقم الهاتف
        $user = DB::table('users')
            ->where('phone', $request->phone)
            ->orderBy('id', 'desc')
            ->first();

        $userId = $user->id;

        // 3️⃣ إنشاء المخول
        $servers = new AuthorizedUserService();

        $authorized = $servers->create([
            'Name_post'     => $request->Name,
            'BranchID'      => $ueser->BrancchID,
            'UserID'        => $userId,
            'AccID'         => $ueser->AccID,
            'InsertUserID'  => $ueser->id,
            'IsActive'      => 1,
            'CreatedDate'   => now(),
            'phone'         => $request->phone
        ]);

        DB::commit();

        return response()->json([
            'success' => true,
            'message' => "تم إضافة المخول بنجاح",
            'user_id' => $userId,
            'authorized_user' => $authorized
        ]);

    } catch (\Illuminate\Validation\ValidationException $e) {

        DB::rollBack();

        return response()->json([
            'success' => false,
            'message' => "بيانات غير صحيحة",
            'errors'  => $e->errors()
        ], 422);

    } catch (\Exception $e) {

        DB::rollBack();

        return response()->json([
            'success' => false,
            'message' => "حدث خطأ أثناء الحفظ",
            'error'   => $e->getMessage()
        ], 500);
    }
}
    public function AuthorizedUsersgetByBranch()
    {
        $ueser = Auth::user(); 
        
        // تحقق من تسجيل الدخول
        if (!$ueser) {
            return response()->json([
                'success' => false,
                'message' => "الرجاء تسجيل الدخول",
            ], 401);
        }
    
        // تحقق من الصلاحية
        if ($ueser->UeserType != 3) {
            return response()->json([
                'success' => false,
                'message' => "لا يوجد لديك صلاحية لهذه الخاصية",
            ], 403);
        }    

        if ($ueser->AccountType !=='Main' )
        {
            return response()->json([
                'success' => false,
                'message' => "لا يوجد لديك صلاحية لهذه الخاصية",
            ], 403);

            

        }
    
        $servers = new AuthorizedUserService(); 
    
        $data = $servers->getByBranch($ueser->BrancchID);
    
        // ✅ التحقق من عدم وجود بيانات
        if (!$data || (is_array($data) && count($data) == 0) || (is_object($data) &&
         method_exists($data, 'isEmpty') && $data->isEmpty())) {
            return response()->json([
                'success' => false,
                'message' => "لا توجد بيانات",
            ], 404);
        }
    
        // ✅ في حالة وجود بيانات
        return response()->json([
            'success' => true,
            'data' => $data
        ], 200);
    }


    public function AuthorizedUsers_update(Request $request)
    {
        try {
    
            $ueser = Auth::user(); 
        
            if (!$ueser) {
                return response()->json([
                    'success' => false,
                    'message' => "الرجاء تسجيل الدخول",
                ], 401);
            }
        
            if ($ueser->UeserType != 3) {
                return response()->json([
                    'success' => false,
                    'message' => "لا يوجد لديك صلاحية لهذه الخاصية",
                ], 403);
            }    

            if ($ueser->AccountType !=='Main' )
            {
                return response()->json([
                    'success' => false,
                    'message' => "لا يوجد لديك صلاحية لهذه الخاصية",
                ], 403);
    
                
    
            }
    
            // 🔹 نجيب السجل الحالي
            $authorizedUser = AuthorizedUser::find($request->ID);

            if (!$authorizedUser) {
                return response()->json([
                    'success' => false,
                    'message' => "المخول غير موجود"
                ], 404);
            }
    
            // ✅ Validation
            $request->validate([
                'ID' => 'required|integer|exists:AuthorizedUsers,ID',
            
                'Name' => 'required|string|max:200',
            
                'phone' => [
                    'required',
                    'string',
                    'max:9',
            
                    Rule::unique('AuthorizedUsers', 'phone')
                        ->ignore($request->ID, 'ID'),
            
                    Rule::unique('users', 'phone')
                        ->ignore($authorizedUser->UserID, 'id'),
                ],
            
                'IsActive' => 'required|boolean'
            ]);
    
            DB::beginTransaction();
    
            $servers = new AuthorizedUserService();
    
            $data = $servers->update($request->ID, [
                'Name'     => $request->Name,
                'phone'    => $request->phone,
                'IsActive' => $request->IsActive 
            ]);



            DB::table('users')
            ->where('id', $authorizedUser->UserID)
            ->update([
                'phone' => $request->phone,
                'Reg'   => 'NO',
            ]);
    
            DB::commit();
    
            return response()->json([
                'success' => true,
                'message' => "تم تعديل المخول بنجاح",
                'data'    => $data
            ]);
    
        } catch (\Illuminate\Validation\ValidationException $e) {
    
            return response()->json([
                'success' => false,
                'message' => "بيانات غير صحيحة",
                'errors'  => $e->errors()
            ], 422);
    
        } catch (\Exception $e) {
    
            DB::rollBack();
    
            return response()->json([
                'success' => false,
                'message' => "حدث خطأ أثناء التعديل",
                'error'   => $e->getMessage()
            ], 500);
        }
    }






    public function Daily_transfer_preparer_schedule_DEttelse_GetUeser()
    {
        $ueser = Auth::user(); 

      
        if (!$ueser) {
            return response()->json([
                'success' => false,
                'message' => "الرجاء تسجيل الدخول",
            ], 401);
        }
        
      
        $datat = DB::table('Daily_transfer_preparer_schedule_DEttelse')
            ->where("ACCID", $ueser->AccID)
            ->select("Daily", "monthly", "Annual" , "Weekly")
            ->first();
        
       
        if (empty($datat)) {
            return response()->json([
                'success' => false,
                'message' => "الرجاء تسجيل الدخول",
            ], 404);
        }
        
        return response()->json([
            'success' => true,
            'datat' => $datat,
        ], 200);
       
    }

    
/////////////////////////////////////]دالة التحقق من معد التحويل الخاص بالفروع والوكلاء///////////////////////////////////////////////////////
public function Rollback_Branch_Trinsfrim_me($branchID, $val_value)
{
    // ✅ تنفيذ استعلام SQL آمن
    $rows = DB::select(
        "SELECT dbo.Rollback_Branch_Trinsfrim_me(a.CurrentAccID, a.BranchType, 1, ?, a.ID) AS Rollback_Branch_Trinsfrim_me
         FROM CoBranch AS a
         WHERE a.ID = ?",
        [$val_value, $branchID]
    );

    // ✅ التحقق من النتيجة
    if (!empty($rows)) {
        $result = $rows[0]->Rollback_Branch_Trinsfrim_me;

        if ($result == 0) {
            // تجاوز الحد المسموح به للتحويل
            return response()->json([
                'success' => false,
                'message' => "لقد تجاوز الحد المسموح به للتحويل.",
            ], 422);
        }

        // ✅ العملية ناجحة - لا يوجد خطأ
        return null;
    }

    // ⚠️ لم يتم العثور على الفرع
    return response()->json([
        'success' => false,
        'message' => "لم يتم العثور على الفرع المطلوب.",
        'BracnID' => $branchID ,
    ], 404);
}

////////////////////////////////////////////////////////دالة تقوم بالتخقق من معدل التحويلا ت

protected function checkTransferLimits($accID, $totalAmount)
{
    $rows = DB::select(
        'SELECT * 
         FROM GET_forDaily_transfer_preparer_schedule(?, ?)',
        [$accID, $totalAmount]
    );

    if (empty($rows)) {
        return null; // لا توجد قيود
    }

    $collection = collect($rows);

    // عناوين لكل نوع
    $labels = [
        'Daily'   => 'اليومي',
        'Weekly'  => 'الأسبوعي',
        'Monthly' => 'الشهري',
        'Annual'  => 'السنوي',
    ];

    $violations = [];
    $violatedTypes = [];

    foreach ($labels as $type => $label) {
        $row = $collection->firstWhere('type_from', $type);
        if ($row && $row->ISACtive == 1) {
            $violations[] = [
                'type_from' => $row->type_from,
                'Debit'     => $row->Debit,
                'label'     => $label,
            ];
            $violatedTypes[] = $label;
        }
    }

    if (!empty($violations)) {
        $finalMessage = ' لقد تجاوزت حدود التحويل ' . implode(' – ', $violatedTypes) . '';

        return response()->json([
            'success'    => false,
            'violations' => $violations,
            'total'      => $totalAmount,
            'message'    => $finalMessage,
        ], 422);
    }

    return null; // لا توجد تجاوزات
}


///////////////////////انشاء اشعار جديد/////////////////////////////////////////////
public function storeNavction(Request $request)
{
    // ✅ التحقق من البيانات المرسلة
    $validator = Validator::make($request->all(), [
        'Type_ID'       => 'required|integer',
        'Name_NEvction' => 'required|string|max:255',
        'IS_showe'      => 'required|boolean',
        'BracnID'       => 'required|integer',
    ]);

    if ($validator->fails()) {
        return response()->json([
            'success' => false,
            'message' => 'فشل التحقق من البيانات',
            'errors'  => $validator->errors(),
        ], 422);
    }

    // ✅ إدخال البيانات في الجدول
    $navction = Navction::create([
        'Type_ID'       => $request->Type_ID,
        'Name_NEvction' => $request->Name_NEvction,
        'IS_showe'      => $request->IS_showe,
        'BracnID'       => $request->BracnID,
    ]);

    // ✅ تجهيز رسالة الإشعار
    $driverName = auth()->user()->name ?? "غير معروف"; // لو عندك اسم السائق من المستخدم الحالي
    $message = "{$request->Name_NEvction}";

    // ✅ استدعاء API الإشعارات
    $response = Http::post(url('/api/device/send-notification-vbnet'), [
        'message' => $message,
    ]);

    // ✅ إرجاع النتيجة النهائية
    return response()->json([
        'success' => true,
        'message' => 'تمت إضافة البيانات بنجاح وتم إرسال الإشعار',
        'data'    => $navction,
        'notification_response' => $response->json(),
    ], 201);
}







///////////////////////////////////////////تعديل حالة الغاء تسليم المندوب والسبب ////////////////////////////////////////////////////////////////////////////////////////
public function InternalExAddCancelReason(Request $request)
{
    $user = Auth::user();
    if (!$user || (int)$user->UeserType !== 7) {
        return response()->json([
            'success' => false,
            'message' => 'عذراً، لا يمكنك الوصول إلى هذه الخدمة.',
        ], 403);
    }

    $bracnhID = Auth::user()->BrancchID;

    // ✅ التحقق من صحة البيانات
    $validator = Validator::make($request->all(), [
        'Code' => 'required|string|max:50',
        'AddCancelReason_ID' => 'required|integer|min:1',
        'AddCancelReason_NameFrom_Driver' => 'required|string|max:255',
    ]);

    if ($validator->fails()) {
        return response()->json([
            'success' => false,
            'message' => 'البيانات غير صالحة.',
            'errors' => $validator->errors(),
        ], 422);
    }

    $data = $validator->validated();

    // ✅ جلب بيانات السائق
    $driver = DB::table('DriversTb')
        ->select('DriverName', 'Phone1', 'Phone2')
        ->where('accontID', $user->AccID)
        ->first();

    if (!$driver) {
        return response()->json([
            'success' => false,
            'message' => 'لم يتم العثور على بيانات السائق.',
        ], 404);
    }

    // ✅ جلب الطلب
    $approvedRequest = DB::table('InternalEx')
        ->where('Code', $data['Code'])
        ->first();

    if (!$approvedRequest) {
        return response()->json([
            'success' => false,
            'message' => 'لا توجد طلبات متاحة حالياً.',
        ], 404);
    }

    if ((int)$approvedRequest->ConfirmType !== 9) {
        return response()->json([
            'success' => false,
            'message' => 'لا يمكنك إلغاء هذا الطلب حالياً.',
        ], 403);
    }

    // ✅ تحديث الطلب
    $updated = DB::table('InternalEx')
        ->where('Code', $approvedRequest->Code)
        ->update([
            'AddCancelReason_ID' => $data['AddCancelReason_ID'],
            'AddCancelReason_NameFrom_Driver' => $data['AddCancelReason_NameFrom_Driver'],
            'ConfirmType' => 10,
        ]);

    if (!$updated) {
        return response()->json([
            'success' => false,
            'message' => 'لم يتم التحديث. قد تكون البيانات كما هي.',
        ], 400);
    }

    // ✅ رسالة واتساب
    $message = "مرحبا السيد: {$approvedRequest->RecievedName}\n"
        . "تم إلغاء طلب التوصيل من قبل المندوب: {$driver->DriverName}\n"
        . "هاتف رقم: {$driver->Phone1}\n"
        . "الجوال: {$driver->Phone2}\n"
        . "لسبب: {$data['AddCancelReason_NameFrom_Driver']}\n"
        . "الرجاء التواصل مع المندوب لمزيد من التفاصيل.";

    try {
        // إرسال رسالة واتساب
        $whatsappResponse = Http::post(url('/api/device/send/whatsapp/message'), [
            'phone' => $approvedRequest->RPhone1 ?? $driver->Phone1,
            'message' => $message,
            'xtoken' => env('CUSTOM_X_TOKEN'),
        ])->json();

        // ✅ استدعاء API التخزين والإشعار (storeNavction)
        $notifyResponse = Http::post(url('/api/device/storeNavction'), [
            'Type_ID'       => 3,
            'Name_NEvction' => "تم الغاء حوالة Code : {$approvedRequest->Code}\n"
                . "سبب الالغاء : {$data['AddCancelReason_NameFrom_Driver']}\n"
                ."من قبل السائق :{$driver->DriverName} ",
            'IS_showe'      => 0,
            'BracnID'       => $bracnhID,
        ])->json();

        return response()->json([
            'success' => true,
            'message' => 'تم التحديث بنجاح.',
            'whatsapp_response' => $whatsappResponse,
            'notify_response' => $notifyResponse,
        ], 201);

    } catch (\Exception $e) {
        return response()->json([
            'success' => true,
            'message' => 'تم التحديث لكن فشل إرسال الإشعار أو الواتساب.',
            'error' => $e->getMessage(),
        ], 201);
    }
}

////////////////////////////////////جلب حالات الالاغاء الاستلام من قبل المستلم من خلال المندوب//////////////////////////////////////////////
public function AddCancelReason()
{
    if (!Auth::check()) {
        return response()->json([
            'success' => false,
            'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
        ], 401);
    }

    $userID = Auth::user()->id;

    if  ( $userID  == null)
    {
        return response()->json([
            'success' => false,
            'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
           
        ], 401);
    }
    $UeserType = Auth::user()->UeserType;

    // التحقق من نوع المستخدم (7 يعني سائق حسب النظام)
    if ($UeserType != 7) {
        return response()->json([
            'success' => false,
            'message' => 'عذراً، لا يمكنك الوصول الي هذه الخدمة في الوقت الحالي .',
            
        ], 403);
    }

    $approvedRequest =DB::table('AddCancelReason')
   -> where('IsActive' , 1)
   ->select(
    'ID' ,
    'NewCause'
   )
   ->get();

   if ($approvedRequest->isEmpty()) {
    return response()->json([
        'success' => false,
        'message' => 'لا توجد طلبات متاحة حالياً.',
    ], 404);
}

return response()->json([
'success' => true,
'data' => $approvedRequest
]);

}
///////////////////////////////جلب الحوالات التي مع التاكسي وغير مسملة/////////////////////////////////////////////////////////////////
public function TaxiInvoiceDrivers_getInternalEx()
{
    if (!Auth::check()) {
        return response()->json([
            'success' => false,
            'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
        ], 401);
    }

    $userID = Auth::user()->id;

    if  ( $userID  == null)
    {
        return response()->json([
            'success' => false,
            'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
           
        ], 401);
    }
    $UeserType = Auth::user()->UeserType;

    // التحقق من نوع المستخدم (7 يعني سائق حسب النظام)
    if ($UeserType != 7) {
        return response()->json([
            'success' => false,
            'message' => 'عذراً، لا يمكنك طلب التوصيل في الوقت الحالي.',
            
        ], 403);
    }

    // جلب الطلبات الخاصة بالسائق مع حالة الطلب
    $approvedRequest = DB::table('InternalEx as a')
        ->join('DriversTb as b', 'a.Driver_ID', '=', 'b.ID')
        ->join('CoBranch as d', 'a.BranchDeliveredID', '=', 'd.ID')
        ->join('users as c', 'b.accontID', '=', 'c.AccID')
        ->join('CoBranch as e', 'a.BranchRecievedID', '=', 'e.ID')
        ->join('InternalEx_Stautes as f', 'a.ConfirmType', '=', 'f.ConfirmType')
        ->join('TaxiInvoiceDrivers as g', 'a.ID_Delvre_For_Taxie', '=', 'g.ID')
        ->where('c.id', $userID)
        ->whereIn('a.ConfirmType', [9,10])
        ->select(
            'a.RecievedName',
            'a.RPhone1',
            'a.Notes',
            'a.lat',
            'a.loge',
            'a.OverallVal',
            'a.Taxi_Ret_DriverS',
            DB::raw('(a.OverallVal - a.TaxiValues) as OverallVal_NEt'),
            'b.DriverName',
            'd.BName as BranchDeliveredID',
            'a.SenderName',
            'a.SPhone1',
            'e.BName as BranchRecievedID',
            'a.Code',
            'f.SName',
            'a.ConfirmType',
            'g.insertDate as dateTaxiInvoiceDrivers',
            'g.date_time as TimeTaxiInvoiceDrivers'
        )
        ->orderby('a.id', 'asc')
        ->get();

        if ($approvedRequest->isEmpty()) {
            return response()->json([
                'success' => false,
                'message' => 'لا توجد طلبات متاحة حالياً.',
            ], 404);
        }

    return response()->json([
        'success' => true,
        'data' => $approvedRequest
    ]);
}




///////////////////////////////////عرض طلبات التوصيل الموافق عليه او التي تحت الانجاز////////////////////////////////////////////////////////////////////////
public function Request_to_summon_driversTB_Notvigtion()
{
    try {
        // التحقق من تسجيل الدخول
        if (!Auth::check()) {
            return response()->json([
                'success' => false,
                'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
            ], 401);
        }

        $userID = Auth::user()->AccID;
        $branchID = Auth::user()->BrancchID;
        $UeserType = Auth::user()->UeserType;

        // التحقق من الفرع
        if (is_null($branchID)) {
            return response()->json([
                'success' => false,
                'message' => 'عذراً، الرجاء اختيار فرع أولاً أو التواصل مع مطور النظام.',
            ], 401);
        }

        // التحقق من نوع المستخدم (7 يعني سائق حسب النظام)
        if ($UeserType != 7) {
            return response()->json([
                'success' => false,
                'message' => 'عذراً، لا يمكنك طلب التوصيل في الوقت الحالي.',
                'UeserType' =>  $userID 
            ], 403);
        }

        // جلب الطلبات الخاصة بالسائق مع حالة الطلب
        $approvedRequest = DB::table('Request_to_summon_driversTB as a')
            ->join('CoBranch as b', 'a.BranchID', '=', 'b.ID')
            ->where('a.BranchID', $branchID)
            ->where('a.AccID_Accipe', $userID)
            ->select(
                'a.ID',
                'a.insertDate',
                'a.Time',
                'a.BranchID',
                'b.BName',
                DB::raw("CASE 
                            WHEN a.IsAccpit = 0 THEN 'تحت الطلب' 
                            WHEN a.IsAccpit = 1 THEN 'تمت الموافقة' 
                            WHEN a.IsAccpit = 2 THEN 'تم تأكيد الوصول' 
                            ELSE 'لم يتم قبول الطلب' 
                        END AS StatusText")
            )
            ->orderBy('a.ID', 'desc')
            ->get();

        if ($approvedRequest->isEmpty()) {
            return response()->json([
                'success' => false,
                'message' => 'لا توجد طلبات متاحة حالياً.',
            ], 404);
        }

        return $this->sendResponse($approvedRequest, 'تم جلب الطلبات بنجاح.');

    } catch (\Throwable $th) {
        Log::error('حدث خطأ أثناء جلب بيانات الطلبات', [
            'error' => $th->getMessage(),
            'user_id' => Auth::id(),
        ]);

        return response()->json([
            'success' => false,
            'message' => 'حدث خطأ غير متوقع أثناء جلب البيانات.',
        ], 500);
    }
}



////////////////////////كود قبول الطلب التعديل الحوالة ///////////////////////////////////////////

public function Request_to_summon_driversTB_Accipet(Request $request)
{
    try {
        if (!Auth::check()) {
            return response()->json([
                'success' => false,
                'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
            ], 401);
        }

        $userID    = Auth::user()->AccID;
        $branchID  = Auth::user()->BrancchID;
        $userType  = Auth::user()->UeserType;

        if (is_null($userID)) {
            return response()->json([
                'success' => false,
                'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
            ], 401);
        }
       
        $driver = DB::table('DriversTb')->where('accontID', $userID)->first();

       
        if (!$driver) {
            return response()->json([
                'success' => false,
                'message' => 'عذراً، لم يتم العثور على بيانات السائق.',
            ], 404);
        }
       


        if (is_null($branchID)) {
            return response()->json([
                'success' => false,
                'message' => 'عذراً، الرجاء اختيار فرع أولاً أو التواصل مع مطور النظام.',
            ], 401);
        }

        if ($userType != 7) {
            return response()->json([
                'success' => false,
                'message' => 'عذراً، لا يمكنك طلب التوصيل في الوقت الحالي.',
            ], 401);
        }
       


        $validator = Validator::make($request->all(), [
            'ID' => ['required', 'integer'],
        ]);

        if ($validator->fails()) {
            return response()->json([
                'success' => false,
                'message' => 'البيانات غير صالحة.',
                'errors' => $validator->errors(),
            ], 422);
        }

        $updated = DB::table('Request_to_summon_driversTB')
            ->where('ID', $request->ID)
            ->where('IsAccpit', 0)
            ->update([
                'IsAccpit'     => 1,
                'AccID_Accipe' => $userID,
                'AccpitTime'   => \Carbon\Carbon::now(),
                'LastUpdate' =>\Carbon\Carbon::now(),
            ]);

        if ($updated > 0) {
            $driverName = $driver->DriverName ?? 'السائق';
          
            $message = "تم قبول الطلب رقم {$request->ID} من قبل {$driverName}";
            $response = Http::post(url('/api/device/send-notification-vbnet'), ['message' => $message]);



            $notifyResponse = Http::post(url('/api/device/storeNavction'), [
                'Type_ID'       => 2,
                'Name_NEvction' =>  $message  ,
                'IS_showe'      => 0,
                'BracnID'       =>  $branchID,
            ])->json();


            return response()->json([
                'success' => true,
                'message' => 'تم التحديث بنجاح.',
                'notification_response' => $response->json(),
            ]);
        }

        return response()->json([
            'success' => false,
            'message' => 'عذراً لم يتم قبول الطلب في الوقت الحالي.',
        ], 200);

    } catch (\Throwable $th) {
        Log::error('حدث خطأ أثناء جلب بيانات الطلبات', [
            'error' => $th->getMessage(),
            'user_id' => Auth::id(),
        ]);

        return response()->json([
            'success' => false,
            'message' => 'حدث خطأ غير متوقع أثناء جلب البيانات.',
        ], 500);
    }
}



/////////////////////////كود عرض الاشعارات التاكسي////////////////////////////////////////////////////////////////


public function Request_to_summon_driversTB_getnavction()
{
    try {
        // التحقق من تسجيل الدخول
        if (!Auth::check()) {
            return response()->json([
                'success' => false,
                'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
            ], 401);
        }

        $userID = Auth::user()->AccID;
        $branchID = Auth::user()->BrancchID;

        // التحقق من وجود الفرع
        if (empty($branchID)) {
            return response()->json([
                'success' => false,
                'message' => 'عذراً، الرجاء اختيار فرع أولاً أو التواصل مع مطور النظام.',
            ], 401);
        }         
        // التحقق من وجود طلب تمت الموافقة عليه لنفس السائق
        $approvedRequest = DB::table('Request_to_summon_driversTB as a')
        ->join('CoBranch as b', 'a.BranchID', '=', 'b.ID')
        ->where('a.BranchID', $branchID)
        ->where('a.IsAccpit', 1)
        ->where('a.AccID_Accipe', $userID)
        ->select(
            'a.ID',
            'a.insertDate',
            'a.Time',
            'a.BranchID',
            'b.BName'
        )
        ->orderBy('a.ID', 'desc')
        ->first();
        
        if ($approvedRequest) {

            // تحويل النتيجة إلى كائن يمكن تعديله
            $approvedRequest->type = 'تم قبول الطلب';
        
            return response()->json([
                'success' => false,
                'message' => ' لايمكن استقبال إشعار حالياً لأنه تمت الموافقة على طلب سابق',
                'data' => $approvedRequest, // إرجاع البيانات مع الحقل الجديد
            ], 403);
        }
        // جلب آخر إشعار غير مقبول
        $pendingNotification = DB::table('Request_to_summon_driversTB as a')
            ->join('CoBranch as b', 'a.BranchID', '=', 'b.ID')
            ->where('a.BranchID', $branchID)
            ->where('a.IsAccpit', 0)
            ->select(
                'a.ID',
                'a.insertDate',
                'a.Time',
                'a.BranchID',
                'b.BName'
            )
            ->orderBy('a.ID', 'desc')
            ->first();

        if (!$pendingNotification) {
            return response()->json([
                'success' => false,
                'message' => 'لا يوجد لديك إشعارات حالياً.',
            ], 404);
        }

        return $this->sendResponse($pendingNotification, 'تم جلب الإشعار بنجاح');

    } catch (\Throwable $th) {
        Log::error('حدث خطأ أثناء جلب إشعارات السائق', [
            'error' => $th->getMessage(),
            'user_id' => Auth::id(),
        ]);

        return response()->json([
            'success' => false,
            'message' => 'حدث خطأ غير متوقع أثناء جلب البيانات.',
        ], 500);
    }
}





/// طلب تاكسي من خلال التطبيق ////////////////////////////
public function Update_for_InternalEx_Taxi(Request $request)
{
    try {
        // ✅ التحقق من تسجيل الدخول
        if (!Auth::check()) {
            return response()->json([
                'success' => false,
                'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
            ], 401);
        }

        $userID = Auth::id();

        // ✅ التحقق من البيانات المدخلة
        $validator = Validator::make($request->all(), [
            'lat'  => 'required|numeric',
            'loge' => 'required|numeric',
            'Code' => ['required', 'string', 'regex:/^[0-9\-]+$/', 'max:50'],
        ]);

        if ($validator->fails()) {
            return response()->json([
                'success' => false,
                'message' => 'البيانات غير صالحة.',
                'errors'  => $validator->errors()
            ], 422);
        }

        // ✅ التحقق من ملكية الحوالة
        $record = DB::table('InternalEx')
            ->join('users', 'InternalEx.RPhone1', '=', 'users.phone')
            ->where('InternalEx.Code', $request->Code)
            ->where('users.ID', $userID)
            ->select('InternalEx.ID', 'InternalEx.ConfirmType', 'BranchDeliveredID')
            ->first();

        if (!$record) {
            return response()->json([
                'success' => false,
                'message' => 'لم يتم العثور على السجل أو ليس لديك صلاحية للتعديل.',
            ], 403);
        }

        // ✅ التحقق من الحالة
        if ($record->ConfirmType != 1) {
            return response()->json([
                'success' => false,
                'message' => 'لا يمكن طلب هذه الحوالة في الوقت الحالي.',
            ], 403);
        }

        // ✅ تنفيذ التحديث
        DB::table('InternalEx')
            ->where('ID', $record->ID)
            ->update([
                'lat'        => $request->lat,
                'loge'       => $request->loge,
                'ConfirmType'=> 7, // حالة "طلب توصيل"
            ]);

        // ✅ إعادة جلب السجل للتحقق
        $check = DB::table('InternalEx')
            ->where('ID', $record->ID)
            ->select('ConfirmType', 'lat', 'loge')
            ->first();

        if ($check && $check->ConfirmType == 7 
            && $check->lat == $request->lat 
            && $check->loge == $request->loge) {

            // 🔔 إشعار الفرع
            $notifyResponse = Http::post(url('/api/device/storeNavction'), [
                'Type_ID'       => 4,
                'Name_NEvction' => "طلب توصيل داخلي للحوالة رقم \n {$request->Code}",
                'IS_showe'      => 0,
                'BracnID'       => $record->BranchDeliveredID,
            ])->json();

            return response()->json([
                'success' => true,
                'message' => 'تم تحديث الطلب بنجاح.',
                'notifyResponse' => $notifyResponse
            ], 200);
        }

        // ❌ لو ما تغيرت القيم لأي سبب
        return response()->json([
            'success' => false,
            'message' => 'لم يتم التحديث. قد تكون البيانات كما هي.',
        ], 409);

    } catch (\Throwable $th) {
        Log::error('حدث خطأ أثناء تحديث InternalEx', [
            'error'   => $th->getMessage(),
            'user_id' => Auth::id(),
            'input'   => $request->all()
        ]);

        return response()->json([
            'success' => false,
            'message' => 'حدث خطأ أثناء التحديث.',
            'details' => app()->environment('production') ? null : $th->getMessage()
        ], 500);
    }
}




///تسليم حوالة للزبون
public function InternalEx_costimer(Request $request)
{
    try {
        if (!Auth::check()) {
            return response()->json([
                'success' => false,
                'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
            ], 401);
        }

        $AccID     = Auth::user()->AccID;
        $AccTyprID = Auth::user()->UeserType;
        $brancID   = Auth::user()->BrancchID;

        if ($AccID === null || $brancID === null) {
            return response()->json([
                'success' => false,
                'message' => 'لا يمكن العثور على بيانات حساب المستخدم أو الفرع.',
            ], 403);
        }

        $rules = [
            'Code'  => 'required|string',
            'Notes' => 'nullable|string',
        ];

        if ($AccTyprID == 7) {
            $rules['Code_for_Accipt_Mobile_or_coustemer'] = 'required|string';
        }

        $validator = Validator::make($request->all(), $rules);
        if ($validator->fails()) {
            return response()->json([
                'success' => false,
                'message' => 'خطأ في التحقق من البيانات.',
                'errors'  => $validator->errors()
            ], 422);
        }

        // **كل العمليات داخل معاملة واحدة**
        $result = DB::transaction(function () use ($request, $AccID, $AccTyprID, $brancID) {

            $record = DB::table('InternalEx')
                ->where('Code', $request->Code)
                ->where('BranchDeliveredID', $brancID)
                ->lockForUpdate() // قفل السجل لمنع التعديل المتزامن
                ->first();

            if (!$record) {
                return response()->json([
                    'success' => false,
                    'message' => 'لم يتم العثور على حوالة بالبيانات المدخلة.',
                ], 404);
            }

            if ($record->Type_Moble_costimer == 1) {
                return response()->json([
                    'success' => false,
                    'message' => 'تم تسليم الحوالة مسبقاً ولا يمكن تسليمها مرة أخرى.',
                ], 409);
            }

            // تحقق كود الاستلام للمندوب
            if ($AccTyprID == 7) {
                $attempt = DB::table('WrongCodeAttempts')
                    ->where('InternalExID', $record->ID)
                    ->where('BranchID', $brancID)
                    ->where('UserID', Auth::id())
                    ->lockForUpdate() // قفل محاولة الخطأ
                    ->first();

                if ($attempt && now()->diffInMinutes($attempt->LastAttemptTime) >= 15) {
                    DB::table('WrongCodeAttempts')
                        ->where('ID', $attempt->ID)
                        ->update([
                            'AttemptCount' => 0,
                            'LastAttemptTime' => now()
                        ]);
                    $attempt->AttemptCount = 0;
                }

                if ($attempt && $attempt->AttemptCount >= 5 && now()->diffInMinutes($attempt->LastAttemptTime) < 15) {
                    $remaining = 15 - now()->diffInMinutes($attempt->LastAttemptTime);
                    return response()->json([
                        'success' => false,
                        'message' => "تم إيقاف المحاولة. يرجى الانتظار $remaining دقيقة قبل إعادة المحاولة.",
                    ], 422);
                }

                if ($record->Code_for_Accipt_Mobile_or_coustemer != $request->Code_for_Accipt_Mobile_or_coustemer) {
                    if ($attempt) {
                        DB::table('WrongCodeAttempts')
                            ->where('ID', $attempt->ID)
                            ->update([
                                'AttemptCount' => $attempt->AttemptCount + 1,
                                'LastAttemptTime' => now()
                            ]);
                    } else {
                        DB::table('WrongCodeAttempts')->insert([
                            'InternalExID' => $record->ID,
                            'BranchID'     => $brancID,
                            'UserID'       => Auth::id(),
                            'AttemptCount' => 1,
                            'LastAttemptTime' => now()
                        ]);
                    }

                    return response()->json([
                        'success' => false,
                        'message' => 'كود الاستلام غير صحيح، لا يمكن تسليم الحوالة.',
                    ], 403);
                }

                // حذف المحاولات في حالة الكود صحيح
                DB::table('WrongCodeAttempts')
                    ->where('InternalExID', $record->ID)
                    ->where('BranchID', $brancID)
                    ->where('UserID', Auth::id())
                    ->delete();
            }

            // جلب بيانات قبل التحديث (قفل السجل)
            $recordBefore = DB::table('InternalEx')
                ->where('Code', $request->Code)
                ->lockForUpdate()
                ->first();

            // التحديث
            DB::table('InternalEx')
                ->where('Code', $request->Code)
                ->update([
                    'ACCID_FRom'          => $AccID,
                    'Type_Moble_costimer' => 1,
                    'Notes'               => $request->Notes,
                ]);

            // جلب بيانات بعد التحديث
            $recordAfter = DB::table('InternalEx')
                ->where('Code', $request->Code)
                ->first();

            $changesHappened = $recordBefore != $recordAfter;

            $transfer = DB::table('InternalEx as a')
                ->select('a.ID', 'a.Code', 'a.SenderName', 'a.RecievedName',
                         'a.ExVal', 'a.OverallVal', 'a.Notes',
                         'a.Code_For_mobules', 'a.InsertDate', 'a.InsertTime')
                ->where('a.Code', $request->Code)
                ->first();

            if ($transfer) {
                $transfer->Type_from = 1;
            }

            if ($changesHappened) {
                $notifyResponse = Http::post(url('/api/device/storeNavction'), [
                    'Type_ID'       => 5,
                    'Name_NEvction' => "تم تسليم الحوالة رقم ({$request->Code}) عن طريق المندوب",
                    'IS_showe'      => 0,
                    'BracnID'       => $brancID,
                ])->json();

                return response()->json([
                    'success'        => true,
                    'message'        => 'تم تسليم الحوالة بنجاح.',
                    'notifyResponse' => $notifyResponse,
                    'transfer'       => $transfer,
                ], 200);
            }

            return response()->json([
                'success' => false,
                'message' => 'لم يتم تعديل أي بيانات على الحوالة، ربما كانت القيم كما هي.',
            ], 200);
        });

        return $result;

    } catch (\Throwable $th) {
        $errorMessage = app()->environment('production')
            ? 'حدث خطأ في الخادم، يرجى المحاولة لاحقًا.'
            : $th->getMessage();

        return response()->json([
            'success' => false,
            'message' => 'حدث خطأ أثناء تنفيذ العملية.',
            'details' => $errorMessage
        ], 500);
    }
}


////////////////////////حولات واردة////////////////////////////////////////////////////////
public function InternalEx_SelectType_View_not_coustmers(Request $request)
{
    try {

        $user = Auth::user();

        if (!$user) {
            return response()->json([
                'success' => false,
                'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
            ], 401);
        }

        if (empty($user->BrancchID)) {
            return response()->json([
                'success' => false,
                'message' => 'لا يوجد فرع مرتبط بهذا المستخدم.'
            ], 403);
        }

        $branchID = $user->BrancchID;

        // تحديد الـ View حسب نوع المستخدم
        $viewName = ($user->UeserType == 3)
            ? 'InternalEx_SelectType_View_not_BRanchId'
            : 'InternalEx_SelectType_View_not_coustmers';

        $results = DB::table("$viewName as a")
            ->where('a.BranchDeliveredID', '=', $branchID)
            ->select([
                'a.Code',
                'a.BName',
                'a.BranchDeliveredID',
                'a.BranchRecievedID',
                'a.CaseStauts',
                'a.ExVal',
                'a.InsertDate',
                'a.OverallVal',
                'a.RecievedName',
                'a.RPhone',
                'a.SenderName',
                'a.SendStatus'
            ])
            ->get();

        return response()->json([
            'success' => true,
            'message' => $results->isEmpty()
                ? 'لا توجد حوالات حالياً.'
                : 'تم جلب الحوالات بنجاح.',
            'data' => $results
        ], 200);

    } catch (\Throwable $e) {

        Log::error('InternalEx Error', [
            'user_id' => Auth::id(),
            'error' => $e->getMessage()
        ]);

        return response()->json([
            'success' => false,
            'message' => app()->environment('production')
                ? 'حدث خطأ في الخادم، يرجى المحاولة لاحقًا.'
                : $e->getMessage()
        ], 500);
    }
}
////////////////////////////////////////////////حوالات المسلمة للزبائن ////////////////////////////////////////////
public function InternalEx_SelectType_View_statetosForok(Request $request)
{
    try {

        $user = Auth::user();

        if (!$user) {
            return response()->json([
                'success' => false,
                'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
            ], 401);
        }

        if (empty($user->BrancchID)) {
            return response()->json([
                'success' => false,
                'message' => 'لا يوجد فرع مرتبط بهذا المستخدم.'
            ], 403);
        }

        $branchID = $user->BrancchID;

        // تحديد الـ View حسب نوع المستخدم
        $viewName = 'InternalEx_SelectType_View_statetosForok';

        $results = DB::table("$viewName as a")
            ->where('a.ACCID_FRom', '=', $user->AccID)
            ->select([
                'a.Code',
                'a.BName',
                'a.BranchDeliveredID',
                'a.BranchRecievedID',
                'a.CaseStauts',
                'a.ExVal',
                'a.InsertDate',
                'a.OverallVal',
                'a.RecievedName',
                'a.RPhone',
                'a.SenderName',
                'a.SendStatus'
            ])
            ->get();

        return response()->json([
            'success' => true,
            'message' => $results->isEmpty()
                ? 'لا توجد حوالات حالياً.'
                : 'تم جلب الحوالات بنجاح.',
            'data' => $results
        ], 200);

    } catch (\Throwable $e) {

        Log::error('InternalEx Error', [
            'user_id' => Auth::id(),
            'error' => $e->getMessage()
        ]);

        return response()->json([
            'success' => false,
            'message' => app()->environment('production')
                ? 'حدث خطأ في الخادم، يرجى المحاولة لاحقًا.'
                : $e->getMessage()
        ], 500);
    }
}


/// [جلب الشروط والاحكام]
public function AppTerms(Request $request)
{
    try {
        // استرجاع الشروط والأحكام مرتبة حسب عمود Order
        $terms = DB::table('AppTerms')
            ->select('Term', 'Order')
            ->orderBy('Order','asc')
            ->get();

        // دمج النصوص مع إضافة الرقم قبل كل شرط
        $fullTermsText = $terms->map(function($item) {
            return $item->Order . '. ' . $item->Term;
        })->implode("\n\n");

        return response()->json([
            'success' => true,
            'data' => $fullTermsText,
            'message' => 'تم استرجاع الشروط والأحكام بنجاح'
        ], 200);

    } catch (\Exception $e) {
        return response()->json([
            'success' => false,
            'message' => 'حدث خطأ أثناء استرجاع الشروط والأحكام',
            'error' => $e->getMessage()
        ], 500);
    }
}


//جلب الفروع للا==////////////////////////////
    public function CoBranch_select(Request $request)
    {
        try {
            // تحقق من أن المستخدم مسجّل دخول
            if (!Auth::check()) {
                return response()->json([
                    'success' => false,
                    'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
                ], 401);
            }
    
            // جلب الفروع
            $results = DB::table('CoBranch as a')
                ->where('a.BranchType', 1)
                ->where('a.IsActive', 1)
                ->select(
                    'a.ID',
                    'a.BName',
                    // CityID: التطبيق يشتقّ فرع الاستلام من المدينة بدل أن
                    // يسأل عنه الوكيل — عمود قراءة فقط، لا تغيير في الجدول.
                    'a.CityID',
                    'a.Mobile1',
                    'a.Mobile2',
                    'a.MapLink',
                    'a.BAddress' ,
                    'A.latitude' , 
                    'A.longtite'
                )
                ->get();
    
            // الاستجابة الناجحة
            return response()->json([
                'success' => true,
                'message' => 'تم جلب الفروع بنجاح.',
                'data' => $results
            ], 200);
    
        } catch (\Exception $e) {
            // إذا كان في بيئة الإنتاج، لا تعرض التفاصيل
            if (app()->environment('production')) {
                return response()->json([
                    'success' => false,
                    'message' => 'حدث خطأ في الخادم، يرجى المحاولة لاحقًا.'
                ], 500);
            }
    
            // في بيئة التطوير، اعرض الخطأ للمطور
            return response()->json([
                'success' => false,
                'message' => 'Exception',
                'details' => $e->getMessage()
            ], 500);
        }
    }
    



// جملة جلب حساب المستخدم بواسطة رقم الهاتف للبحث 
public function ExchangeAcc(Request $request)
{
    // ✅ التأكد من أن المستخدم مسجل دخول
    if (!Auth::check()) {
        return response()->json([
            'success' => false,
            'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
        ], 401); // Unauthorized
    }

    // ✅ التحقق من صحة رقم الهاتف
    $validator = Validator::make($request->all(), [
        'phone' => [
            'required',
            'string',
            'max:20',
            'regex:/^[0-9+\-\s()]+$/'
        ],
    ], [
        'phone.regex' => 'رقم الهاتف يحتوي على أحرف غير مسموح بها.',
        'phone.required' => 'حقل رقم الهاتف مطلوب.',
    ]);

    if ($validator->fails()) {
        return response()->json([
            'success' => false,
            'message' => 'خطأ في البيانات المدخلة',
            'errors' => $validator->errors()
        ], 422);
    }

    $phone = $request->input('phone');
    $currentUserId = Auth::id();

    // ✅ تنفيذ الاستعلام مع استثناء المستخدم الحالي
    $results = DB::table('users as a')
        ->join('AccountsTb as b', 'a.AccID', '=', 'b.AccID')
        ->join('CoBranch as c', 'b.BranchID', '=', 'c.ID')
        ->where('a.phone', $phone)
        ->where('a.id', '<>', $currentUserId)
        ->select(
            'a.id as user_id',
            'b.AccName',
            'b.AccCode',
            'c.BName',
            'a.AccID',
            'b.AccPhone',
        )
        ->get();

    
     
    
   

    if ($results->isEmpty()) {
        return response()->json([
            'success' => false,
            'message' => 'لا يوجد حساب مطابق لرقم الهاتف المطلوب.'
        ], 404);
    }

    return response()->json([
        'success' => true,
        'message' => 'تم جلب البيانات بنجاح.',
        'data' => $results
    ], 200);
}






//كود اضافة الي المفضلة ///

public function Favorites_Table_delete(Request $request)
{
    // التحقق من صحة البيانات المدخلة
    $validator = Validator::make($request->all(), [
        'code_Favorite'  => 'required|STRING',
        'Type_Favorite'  => 'required|integer',
    ]);

    if ($validator->fails()) {
        return $this->sendError('Validation Error.', $validator->errors(), 422);
    }

    // التأكد من أن المستخدم مسجّل
    $user_id = Auth::id();
    if (!$user_id) {
        return $this->sendError('Unauthorized. User not authenticated.', [], 401);
    }

    try {
        // حذف السجل من جدول المفضلات بناءً على المعطيات
        DB::table('Favorites_Table')
            ->where("code_Favorite", $request->code_Favorite)
            ->where("Type_Favorite", $request->Type_Favorite)
            ->where('UEserID', $user_id) // تأكد من أن الحذف خاص بالمستخدم الحالي
            ->delete();

        return $this->sendResponse([], 'تم حذف العنصر من المفضلة بنجاح.');

    } catch (\Exception $e) {
        return $this->sendError('حدث خطأ أثناء حذف المفضلة.', ['error' => $e->getMessage()], 500);
    }
}


// كود الحذف من المفضلة ///

public function Favorites_Table_inser(Request $request)
{
    // التحقق من صحة البيانات المدخلة
    $validator = Validator::make($request->all(), [
        'code_Favorite'  => 'required|string|max:255',
        'Type_Favorite'  => 'required|integer', // تم التعديل هنا
        'phone' =>'required|string|max:50'
    ]);

    if ($validator->fails()) {
        return $this->sendError('Validation Error.', $validator->errors(), 422);
    }

    // التأكد من أن المستخدم مسجّل
    $user_id = Auth::id();
    if (!$user_id) {
        return $this->sendError('Unauthorized. User not authenticated.', [], 401);
    }

    // التحقق من التكرار: لا يمكن تكرار نفس code_Favorite لنفس المستخدم
    $exists = DB::table('Favorites_Table')
        ->where('code_Favorite', $request->code_Favorite)
        ->where('UEserID', $user_id)
        ->where("Type_Favorite" ,$request->Type_Favorite )
        ->exists();

    if ($exists) {
        return $this->sendError('هذا العنصر مضاف مسبقًا إلى المفضلة.', [], 409);
    }


    try {
        // إدخال السجل في جدول المفضلات
        DB::table('Favorites_Table')->insert([
            'code_Favorite' => $request->code_Favorite,
            'Type_Favorite' => $request->Type_Favorite,
            'UEserID'       =>  $user_id  ,
            'phone' =>$request->phone
        ]);

        return $this->sendResponse([], 'تمت إضافة العنصر إلى المفضلة بنجاح.');

    } catch (\Exception $e) {
        return $this->sendError('حدث خطأ أثناء حفظ المفضلة.', ['error' => $e->getMessage()], 500);
    }
}

/////////////////////////////////////
public function Favorites(Request $request)
{
    try {
        // الحصول على ID المستخدم الحالي
        $user_id = Auth::id();

        // التحقق من وجود المستخدم
        if (!$user_id) {
            return $this->sendError('Unauthorized', [], 401);
        }

        // استدعاء الإجراء المخزن مع تمرير user_id
        $results = DB::select('EXEC dbo.Favorites_GetByUserID ?', [$user_id]);

        // إعادة البيانات بنجاح
        return $this->sendResponse($results, 'Success');
    } catch (\Throwable $e) {
        // معالجة الأخطاء
        return $this->sendError('Server Error', ['error' => $e->getMessage()], 500);
    }
}


//////////////////////////////////

public function storeaddcostmer(Request $request)
{
    // تنظيف القيم المدخلة
    $input = $request->all();
    $input['Name'] = trim($input['Name'] ?? '');
    $input['National_Number'] = trim($input['National_Number'] ?? '');
    $input['phone'] = trim($input['phone'] ?? '');
    $input['device_id'] = trim($input['device_id'] ?? '');
    $input['city_ID'] = trim($input['city_ID'] ?? '');
    $input['Countries_ID'] = trim($input['Countries_ID'] ?? '');

    // التحقق من المدخلات
    $validator = Validator::make($input, [
        'Name' => 'required|string|min:3|max:450|regex:/^[\pL\s\-\'\.0-9]+$/u',
        'National_Number' => 'required|string|min:12|max:20|regex:/^[0-9]+$/',
        'phone' => 'required|string|min:9|max:30|regex:/^[0-9\+\-\(\)\s]+$/',
        'Type_Account' => 'required|integer|min:0|max:10',
        'device_id' => 'required|string|max:250',
        'Countries_ID' => 'required|integer',
        'city_ID' => 'required|integer'
    ]);

    if ($validator->fails()) {
        return response()->json([
            'error' => 'Validation failed',
            'details' => $validator->errors()
        ], 422);
    }

    $data = $validator->validated();

    // التحقق من التكرار في جدول الإدخال
    $existsInCurrentTable = DB::table('Table_ADD_forCostumerMobile')
        ->where('National_Number', $data['National_Number'])
        ->orWhere('phone', $data['phone'])
        ->exists();

    if ($existsInCurrentTable) {
        return response()->json([
            'error' => 'عذراً، هذه البيانات موجودة مسبقاً. الرجاء التأكد من صحتها.'
        ], 409);
    }

    // التحقق من التكرار في جدول الحسابات
    $existsInAccountsTable = DB::table('AccountsTb')
        ->where('AccPhone', $data['phone'])
        ->exists();

    if ($existsInAccountsTable) {
        return response()->json([
            'error' => 'عذراً، رقم الهاتف مُسجل مسبقاً. الرجاء التأكد من صحة البيانات.'
        ], 409);
    }

    // رفع الصور باستخدام Helper الذكي
    $images = \App\Helpers\ImageUploader::uploadSingleForTwoFields(
        $request->file('pasbort_image'),
        $request->file('profile_image')
    );

    $data['pasbort_link'] = $images['pasbort_link'];
    $data['image_link'] = $images['image_link'];

    try {
        // إدخال البيانات في DB
        $id = DB::table('Table_ADD_forCostumerMobile')->insertGetId($data);

        // إنشاء توكن تلقائي بعد التسجيل
        $token = \Illuminate\Support\Str::random(60);
        DB::table('CustomerTokens')->insert([
            'customer_id' => $id,
            'token' => $token,
            'created_at' => now(),
            'updated_at' => now()
        ]);

        // إعداد رسالة واتساب
        $message = "مرحبا السيد : {$data['Name']}\n\n"
                 . "تم تسجيل بياناتك بنجاح في نظامنا.  \n\nشكراً لاختيارك خدماتنا.\n\n"
                 . "سيتم مراجعة طلبك والموافقة عليه خلال مدة لا تتجاوز 24 ساعة.\n\n"
                 . "نحن دائماً في خدمتك لأي استفسار.\n\n"
                 . ".للتواصل مع الرقم : 0916121181"
                 . "\nمع تحياتنا شركة الرحالة القابضة\n"
                 . " فريق الدعم";

        $response = \Illuminate\Support\Facades\Http::post(url('/api/device/send/whatsapp/message'), [
            'phone' => $data['phone'],
            'message' => $message,
            'xtoken' => env('CUSTOM_X_TOKEN')
        ]);

        // الرد النهائي
        return response()->json([
            'status' => true,
            'customer_id' => $id,
            'pasbort_link' => $data['pasbort_link'] ?? null,
            'image_link' => $data['image_link'] ?? null,
            'token' => $token,
            'whatsapp_response' => $response->json()
        ], 201);

    } catch (\Exception $e) {
        return response()->json([
            'error' => 'حدث خطأ في الخادم.',
            'message' => env('APP_DEBUG') ? $e->getMessage() : 'حدث خطأ غير متوقع.'
        ], 500);
    }
}


    //جلب رصيد ودائع النقد الاجنبي.  
    public function ForginDepositExchage(Request $request   ){ 

        $validator = Validator::make($request->all(), [
            'currency_id' => 'required' 
        ]);
        
        if ($validator->fails()) {
            return $this->sendError('Validation Error.', $validator->errors(), 422);
        }

   $user_id = Auth::User()->id;
   $currency_id = $request->currency_id;

 $results = DB::select("
    SELECT 
        a.Walet, 
        b.CuName, 
        b.CurCode   , 
		a.Currency_ID
    FROM [dbo].[wallet] as a  
    INNER JOIN CurrencyMainTb as b ON a.Currency_ID = b.ID
    WHERE a.UeserID = ? 
    AND a.Currency_ID <> ?  
    AND a.Walet <> 0
", [$user_id ,   $currency_id ]);  // Replace $currency_id with your variable


return $this->sendResponse( $results , 'Success');
        
     }






     // كشف حساب ودائع النقد الاجنبي. 
    public function ForginDepositExchageAS(Request $request   ){ 

        $validator = Validator::make($request->all(), [
            'currency_id' => 'required' 
        ]);
        
        if ($validator->fails()) {
            return $this->sendError('Validation Error.', $validator->errors(), 422);
        }

   $user_id = Auth::User()->id;
   $currency_id = $request->currency_id;

   $results = DB::select("
   SELECT 
   CASE 
       WHEN  a.Credit >a.Debit  THEN 'خصم'  
       ELSE 'ايداع' 
   END AS Type_from, 
   CASE 
       WHEN a.Debit > a.Credit THEN a.Debit  
       ELSE a.Credit 
   END AS Values_to, 
   a.InsertDate, 
   a.MovementType , 
   Balnce
FROM [ExSyAccountsCurrency2026].[dbo].[AccSafeActivityTbCurrency] as a
INNER JOIN users as b ON a.accidfrom = b.AccID
WHERE b.id = ? AND a.CurrencyID = ?
", [$user_id , $currency_id]);  // Replace $user->id and $currency_id with your actual variables



return $this->sendResponse( $results , 'Success');
        
  }





  //   جلب الدول. 
  public function getCountries(Request $request)
{
    // تحقق من أن country_id موجود وأنه عدد صحيح
    $validator = Validator::make($request->all(), [
        'country_id' => 'required|integer',
    ]);

    if ($validator->fails()) {
        return $this->sendError('Validation Error.', $validator->errors(), 422);
    }

    // استخدام Query Builder (طريقة Laravelية احترافية وآمنة)
    $results = DB::table('CountiresTb as a')
        ->select([
            'a.ID',
            'a.CCode',
            'a.CName',
            'a.DefualtCurrency',
            'a.IsService',
            'a.IsActive',
            'a.IsMain'
        ])
        ->where('a.ID', '<>', $request->country_id)
        ->get();

    return $this->sendResponse($results, 'Success');
}

  

  //   جلب الرصيد الحالي الخاص بالمستخدم في العملة المحلية. 
  public function getBalanceLocal(Request $request   ){ 

    $validator = Validator::make($request->all(), [
        'currency_id' => 'required' 
    ]);
    
    if ($validator->fails()) {
        return $this->sendError('Validation Error.', $validator->errors(), 422);
    }

    $user_id = Auth::User()->id;
 

    $results = DB::select("
    SELECT a.Walet 
    FROM [dbo].[wallet] as a 
    WHERE a.UeserID = ? AND a.Currency_ID = ?
", [$user_id, $request->currency_id]);  // Replace $user_id and $currency_id with your actual variables


    return $this->sendResponse( $results , 'Success');

}

 

  //   نوع الخدمة علي حسب الدولة الي مرسلة اليها في الحوالة الخاريجية. 
  public function getServicesExternal(Request $request   ){ 

    $validator = Validator::make($request->all(), [
        'country_id' => 'required' 
    ]);
    
    if ($validator->fails()) {
        return $this->sendError('Validation Error.', $validator->errors(), 422);
    }

    $user_id = Auth::User()->id;


    $results = DB::select("
    SELECT 
        a.ID AS SRID,
        a.ServiceName AS SRNAME,
        a.DisConstant,
        a.CountryID,
       a.Type_String 
    FROM dbo.ExtTraServiceTypeTb AS a
    WHERE a.CountryID = ?
", [$request->country_id]);  // Replace $country_id with your actual variable


    return $this->sendResponse( $results , 'Success');

}


  //  كشف حساب العملة المحلية. 
  public function LocalStatmentAccount(Request $request)
  {
      $userId = Auth::id();
  
      if (is_null($userId)) {
          return $this->sendResponse(null, 'الرجاء تسجيل الدخول أولاً.', 422);
      }
  
      $results = DB::table('EX24AccSafeActivityTb as a')
      ->join('users as b', 'a.accidfrom', '=', 'b.AccID')
      ->join('AccountsTb as c', 'b.AccID', '=', 'c.AccID')
      ->join('OperationTypeTb as d', 'a.OperationTypeID', '=', 'd.OpNum')
      ->where('b.id', $userId)
      ->orderByDesc('a.ID')
      ->selectRaw("
          CASE 
              WHEN c.AccDmType = 0 THEN 
                  CASE 
                      WHEN a.Debit > a.Credit THEN 'ايداع'
                      ELSE 'خصم'
                  END
              ELSE 
                  CASE 
                      WHEN a.Debit > a.Credit THEN 'خصم'
                      ELSE 'ايداع'
                  END
          END AS Type_from,
  
          CASE 
              WHEN a.Debit > a.Credit THEN a.Debit
              ELSE a.Credit
          END AS Values_to,
  
          a.InsertDate,
          d.OperationType AS MovementType,
          a.Balnce
      ")
      ->get();
  
  
      return $this->sendResponse($results, 'تم جلب كشف الحساب بنجاح.');
  } 
  public function GetCities(Request $request)
  {
      // التحقق من صحة المدخلات
      $validator = Validator::make($request->all(), [
          'country_id' => 'required|integer',
          'exclude_city_id' => 'required|integer',
      ]);
  
      if ($validator->fails()) {
          return $this->sendError('Validation Error.', $validator->errors(), 422);
      }
  
      // استخدام Laravel Query Builder الآمن
      $results = DB::table('CitiesTb as a')
          ->select('a.ID', 'a.Code', 'a.CityName', 'a.CountryID')
          ->where('a.CountryID', $request->country_id)
          ->where('a.ID', '!=', $request->exclude_city_id)
          ->get();
  
      return $this->sendResponse($results, 'Success');
  }
  
// تمت  التعديل وتحسين الكود من قبل حسن هارون

public function ExchangeAccData(Request $request)
{
    try {
        if (!Auth::check()) {
            return response()->json(['error' => 'يجب تسجيل الدخول'], 401);
        }

        $user      = Auth::user();
        $user_id   = $user->id;
        $userType  = $user->UeserType ?? null;
        $AccountType = $user->AccountType;

        if ($userType === null) {
            return response()->json(['error' => 'نوع المستخدم غير متوفر'], 422);
        }

        $columns = [
            'AccCode','AccID','AccName','TransDate','TransValue','Type_trns',
            'commint','Code','STates_spinng','Type_from','STates_Ineger',
            'ServiceName','CName','TransPrice','NetTotal','ServiceExVal','Notes' ,"ExtTraServiceTypeTbid"
        ];

        // ✅ 1- لو POS → فقط حركات النقطة
        if ($AccountType == 'pos') {

            $results = DB::table('ExchangeAccData')
                ->where('uesrID_forminsertmobile', $user_id)
                ->where('IsActive', 1)
                ->select($columns)
                ->get();
        }

        // ✅ 2- باقي المستخدمين (نفس الكود السابق)
        else {

            switch ($userType) {

                case 6:
                    $results = DB::table('ExchangeAccData_notACCid')
                        ->join('users', 'ExchangeAccData_notACCid.AccCode', '=', 'users.phone')
                        ->where('users.id', $user_id)
                        ->where('ExchangeAccData_notACCid.IsActive', 1)
                        ->select(array_merge($columns, ['RecievedName','RPhone1']))
                        ->get();
                    break;

                case 7:
                    $results = DB::table('ExchangeAccData_DRiverFrom')
                        ->join('DriversTb', 'DriversTb.ID', '=', 'ExchangeAccData_DRiverFrom.Driver_ID')
                        ->join('users', 'DriversTb.accontID', '=', 'users.AccID')
                        ->where('users.id', $user_id)
                        ->where('ExchangeAccData_DRiverFrom.IsActive', 1)
                        ->select($columns)
                        ->get();
                    break;

                default:
                    $results = DB::table('ExchangeAccData')
                        ->where('AccID', $user->AccID )
                        ->where('IsActive', 1)
                        ->select($columns)
                        ->get();
                    break;
            }
        }

        // ✅ تحقق من البيانات
        if ($results->isEmpty()) {
            return response()->json([
                'success' => false,
                'message' => 'لا توجد بيانات'
            ], 404);
        }

        return $this->sendResponse($results, 'تم جلب البيانات بنجاح');

    } catch (\Throwable $e) {
        return response()->json([
            'success' => false,
            'message' => 'حدث خطأ داخلي في الخادم',
            'error' => $e->getMessage(),
        ], 500);
    }
}


///حوالة واردة من الفروع مستخدم عادي 

public function ExchangeAccData_notACCid_Cosumer(Request $request)
{
    try {
        // التحقق من تسجيل الدخول
        if (!Auth::check()) {
            return response()->json(['error' => 'يجب تسجيل الدخول'], 401);
        }

        $user_id = Auth::id();
        $user = Auth::user();

        // ✅ تأكد من وجود الخاصية UserType في جدول users
        $userType = $user->UeserType ?? null;

        if ($userType === null) {
            return response()->json(['error' => 'نوع المستخدم غير متوفر'], 422);
        }

        $results = collect(); // مصفوفة فارغة افتراضيًا

        if ($userType != 6) {
            $results = DB::table('ExchangeAccData')
                ->join('users', 'ExchangeAccData.AccID', '=', 'users.AccID')
                ->where('users.id', $user_id)
                ->where('ExchangeAccData.IsActive', 1)
                ->select([
                    'ExchangeAccData.AccCode',
                    'ExchangeAccData.AccID',
                    'ExchangeAccData.AccName',
                    'ExchangeAccData.TransDate',
                    'ExchangeAccData.TransValue',
                    'ExchangeAccData.Type_trns',
                    'ExchangeAccData.commint',
                    'ExchangeAccData.Code',
                    'ExchangeAccData.STates_spinng',
                    'ExchangeAccData.Type_from',
                    'ExchangeAccData.STates_Ineger',
                    'ExchangeAccData.ServiceName',
                    'ExchangeAccData.CName',
                    'ExchangeAccData.TransPrice',
                    'ExchangeAccData.NetTotal',
                    'ExchangeAccData.ServiceExVal',
                    'ExchangeAccData.Notes',
                ])
                ->get();
        } else {
            $results = DB::table('ExchangeAccData_notACCid_Cosumer')
                ->join('users', 'ExchangeAccData_notACCid_Cosumer.RPhone1', '=', 'users.phone')
                ->where('users.id', $user_id)
                ->where('ExchangeAccData_notACCid_Cosumer.IsActive', 1)
                ->select([
                    'ExchangeAccData_notACCid_Cosumer.AccCode',
                    'ExchangeAccData_notACCid_Cosumer.AccID',
                    'ExchangeAccData_notACCid_Cosumer.AccName',
                    'ExchangeAccData_notACCid_Cosumer.TransDate',
                    'ExchangeAccData_notACCid_Cosumer.TransValue',
                    'ExchangeAccData_notACCid_Cosumer.Type_trns',
                    'ExchangeAccData_notACCid_Cosumer.commint',
                    'ExchangeAccData_notACCid_Cosumer.Code',
                    'ExchangeAccData_notACCid_Cosumer.STates_spinng',
                    'ExchangeAccData_notACCid_Cosumer.Type_from',
                    'ExchangeAccData_notACCid_Cosumer.STates_Ineger',
                    'ExchangeAccData_notACCid_Cosumer.ServiceName',
                    'ExchangeAccData_notACCid_Cosumer.CName',
                    'ExchangeAccData_notACCid_Cosumer.TransPrice',
                    'ExchangeAccData_notACCid_Cosumer.NetTotal',
                    'ExchangeAccData_notACCid_Cosumer.ServiceExVal',
                    'ExchangeAccData_notACCid_Cosumer.Notes',
                    'ExchangeAccData_notACCid_Cosumer.RecievedName',
                    'ExchangeAccData_notACCid_Cosumer.RPhone1',
                    'ExchangeAccData_notACCid_Cosumer.Code_for_Accipt_Mobile_or_coustemer',
                    'ExchangeAccData_notACCid_Cosumer.TaxiValues'
                ])
                ->get();
        }

        return $this->sendResponse($results, 'تم جلب البيانات بنجاح');

    } catch (\Throwable $e) {
        return response()->json([
            'success' => false,
            'message' => 'حدث خطأ داخلي في الخادم',
            'error' => $e->getMessage(),
            'line' => $e->getLine(),
            'file' => $e->getFile()
        ], 500);
    }
}

 ///////////////////// الجوالة الداخلية
 public function InternalExchange(Request $request)
 {
     try {
         // ✅ التحقق من الطلب
         $validator = Validator::make($request->all(), [
             'country_id'     => 'required',
             'reviced_phone'  => 'required',
             'reviced_name'   => 'required',
             'AccID'          => 'required',
             'currency_id'    => 'required',
             'amount'         => 'required|numeric|min:1',
             'branch_id'      => 'required',
             'city_id'        => 'required',
             'Commition'      => 'required|numeric|min:0',
             'SenderName'     => 'nullable',
             'SPhone1'        => 'nullable'
         ]);
 
         if ($validator->fails()) {
             return $this->sendError('Validation Error.', $validator->errors(), 422);
         }
 
         $user = Auth::user();
         $user_id = $user->id;
         $accID = $user->AccID;
 
         // ✅ منع التحويل قبل مرور 3 دقائق على آخر عملية
         $result = DB::selectOne('
             SELECT ISNULL(
                 (
                     SELECT DATEDIFF(MINUTE, InsertDate, GETDATE())
                     FROM InternalEx
                     WHERE AccFrom = ?
                     AND ID = (SELECT MAX(ID) FROM InternalEx WHERE AccFrom = ?)
                 ), 6
             ) AS DifferenceInMinutes
         ', [$accID, $accID]);
 
         $DifferenceInMinutes = $result->DifferenceInMinutes ?? 6;
 
         if ($DifferenceInMinutes < 1) {
             return $this->sendError('يمكن المحاولة بعد 1 دقائق', [], 422);
         }
 
         // ✅ جلب العمولة المناسبة
         $transferCommission = Transfer_commissions::where('First_Value', '<=', $request->amount)
             ->where('Second_value', '>=', $request->amount)
             ->first();
 
         if ($user->UeserType == "3" ||$user->UeserType == "5"  ) {
             $commissionValue = floatval($request->Commition);
         } else {
             if (!$transferCommission) {
                 return $this->sendError('لم يتم العثور على العمولة المناسبة.', [], 422);
             }
 
             $commissionValue = floatval($transferCommission->Commission_value);
         }
 
         $totalAmount = $commissionValue + floatval($request->amount);
 
         // ✅ إضافة التعديل فقط
         $isTypeFive = $user->UeserType == "5";
 
         $senderName = $isTypeFive
             ? ($request->input('SenderName') ?: $user->name)
             : $user->name;
 
         $senderPhone = $isTypeFive
             ? ($request->input('SPhone1') ?: $user->phone)
             : $user->phone;
 
         // ✅ التحقق من حدود التحويل
         if ($error = $this->checkTransferLimits($accID, $totalAmount)) {
             return $error;
         }
 
         // ✅ التحقق من معدل التحويل إذا كان المستخدم فرع أو وكيل
         if ($user->UeserType == "5" || $user->UeserType == "3") {
             $rollbackResult = $this->Rollback_Branch_Trinsfrim_me($request->branch_id, $totalAmount);
             if ($rollbackResult instanceof \Illuminate\Http\JsonResponse) {
                 return $rollbackResult;
             }
         }
 
         // ✅ التحقق من الرصيد
         if ($user->UeserType != "3" && $user->UeserType != "5") {
             $wallet = wallet::where('UeserID', $user_id)
                 ->where('Currency_ID', $request->currency_id)
                 ->where('Walet', '>=', $totalAmount)
                 ->first();
 
             if (!$wallet) {
                 return $this->sendError([
                     'wallet' => $wallet,
                     'amount' => floatval($request->amount),
                     'Commission' => $commissionValue,
                     'total' => $totalAmount,
                 ], 'رصيد غير كافٍ.', 422);
             }
         }
 
         // ✅ تنفيذ العملية داخل Transaction
         return DB::transaction(function () use ($request, $user, $commissionValue, $totalAmount, $senderName, $senderPhone) {
             $maxAttempts = 5;
             $codeForMobile = null;
 
             for ($attempt = 1; $attempt <= $maxAttempts; $attempt++) {
                 $userPart = str_pad(substr($user->id, -3), 3, '0', STR_PAD_LEFT);
                 $timePart = date('s');
                 $randomPart = rand(10, 99);
                 $codeForMobile = $userPart . $timePart . $randomPart;
 
                 if (!DB::table('InternalEx')->where('Code_For_mobules', $codeForMobile)->exists()) {
                     break;
                 }
 
                 if ($attempt === $maxAttempts) {
                     throw new \Exception("فشل في توليد كود فريد بعد عدة محاولات");
                 }
             }
 
             DB::table('InternalEx')->insert([
                 'Code_For_mobules' => $codeForMobile,
                 'Code' => $codeForMobile,
                 'SenderName' => $senderName,
                 'SPhone1' => $senderPhone,
                 'AccFrom' => $user->AccID,
                 'RecievedName' => $request->reviced_name,
                 'RPhone1' => $request->reviced_phone,
                 'RecievedCurrencyID' => $request->currency_id,
                 'DeliveredCurrencyID' => $request->currency_id,
                 'OverallVal' => $request->amount,
                 'ExVal' => $commissionValue,
                 'SafeDeliveredID' => 1,
                 'BranchDeliveredID' => $request->branch_id,
                 'BBRANCHID' => $request->branch_id,
                 'DeliveryPlace' => $request->city_id,
                 'Type_Moble' => 1,
                 'BranchRecievedID' => 0,
                 'Notes' => $request->Notes,
                 'uesrID_forminsertmobile' => $user->id
             ]);
 
             $transfer = DB::table('InternalEx as a')
                 ->select(
                     'a.ID', 'a.Code', 'a.SenderName', 'a.RecievedName',
                     'a.ExVal', 'a.OverallVal', 'a.Notes',
                     'a.Code_For_mobules', 'a.InsertDate', 'a.InsertTime'
                 )
                 ->where('a.Code_For_mobules', $codeForMobile)
                 ->first();
 
             if (!$transfer) {
                 throw new \Exception("لم يتم العثور على السجل بعد الإدخال في InternalEx");
             }
 
             $transfer->Type_from = 2;

             // الإشعار داخل DB::transaction، فأي فشل فيه كان يُلغي حوالة
             // مكتملة بالفعل. رسالة واتساب لا يجوز أن تُبطل حركة مالية —
             // تُسجَّل ويمضي الإرسال.
             try {
                 $whatsappService = new Watsaoserversfrom();
                 $whatsappService->sendAgentTransactionMessage($transfer->Code);
             } catch (\Throwable $notifyError) {
                 \Log::error('فشل إشعار واتساب لحوالة داخلية — الحوالة سليمة', [
                     'code'  => $transfer->Code,
                     'error' => $notifyError->getMessage(),
                 ]);
             }

             return $this->sendResponse(['transfer' => $transfer], 'تمت العملية بنجاح');
         }, 3);
     }
 
     catch (\Throwable $th) {
         return $this->sendError('حدث خطأ غير متوقع: ' . $th->getMessage(), [], 500);
     }
 }
 
////////////////////// انتهاء الحوالة الداخلية ///////////////////////

/**
 * معاينة عمولة التحويل بين الحسابات قبل التنفيذ.
 *
 * transInsert يحتسب العمولة من Transfer_commissions ولا يقبل قيمة من العميل،
 * ولم تكن هناك نقطة لمعرفتها مسبقاً — فكان التطبيق يُرسل ثم يكتشف الخصم.
 * وشرائح الجدول فيها ثغرات حقيقية (لا شريحة بين 10000 و11000، ولا فوق 100000)
 * فيرد الإدراج 422 بعد أن يكون المستخدم أكمل النموذج.
 *
 * الاستعلام هنا **نسخة حرفية** من الذي في transInsert — أي تعديل هناك يجب أن
 * يُنقل هنا وإلا انحرفت المعاينة عن التنفيذ.
 */
public function transBetweenAccountsCommission(Request $request)
{
    $validator = Validator::make($request->all(), [
        'amount' => 'required|numeric|min:0.01',
    ]);

    if ($validator->fails()) {
        return $this->sendError('Validation Error.', $validator->errors(), 422);
    }

    $amount = floatval($request->amount);

    $band = Transfer_commissions::where('First_Value', '<=', $amount)
        ->where('Second_value', '>=', $amount)
        ->first();

    if (!$band) {
        return $this->sendError(
            'لا توجد عمولة مطابقة لهذا المبلغ.',
            ['amount' => $amount, 'matched' => false],
            422
        );
    }

    $commission = floatval($band->Commission_value);

    return $this->sendResponse([
        'amount'     => $amount,
        'commission' => $commission,
        'total'      => $amount + $commission,
        'matched'    => true,
    ], 'تم احتساب العمولة');
}

public function transInsert(Request $request)
{
    $step = 'initial_validation';

    try {
        // ✅ 1. التحقق من صحة البيانات
        $validator = Validator::make($request->all(), [
            'acc_id'      => 'required',
            'acc_id_to'   => 'required',
            'currency_id' => 'required',
            'amount'      => 'required|numeric|min:0.01',
            'branch_id'   => 'required',
        ]);

        if ($validator->fails()) {
            return $this->sendError('Validation Error.', $validator->errors(), 422);
        }

        $user = Auth::user();
        $user_id = $user->id;
        $AccID = $user->AccID;

        // ✅ 2. منع التحويل خلال 3 دقائق من آخر عملية
        $check = DB::selectOne("
            SELECT 
                DATEDIFF(MINUTE, 
                        TransDate ,  
                         GETDATE()
                ) AS DifferenceInMinutes
            FROM TransBetweenAccountsTB
            WHERE TransFrom = ? 
              AND ID = (
                  SELECT MAX(ID) 
                  FROM TransBetweenAccountsTB 
                  WHERE TransFrom = ?
              )
        ", [$AccID, $AccID]);

        $DifferenceInMinutes = $check ? $check->DifferenceInMinutes : 10;

        if ($DifferenceInMinutes <= 3) {
            return $this->sendError(
                'خطأ في عملية التحويل',
                ['message' => 'يمكن المحاولة بعد 3 دقائق'],
                422
            );
        }

        // ✅ 3. التحقق من العمولة
        $step = 'commission_check';

        $amount = floatval($request->amount);

        $transferCommission = Transfer_commissions::where('First_Value', '<=', $amount)
            ->where('Second_value', '>=', $amount)
            ->first();

        if (!$transferCommission) {
            return $this->sendError('لم يتم العثور على نسبة العمولة المناسبة.', [], 422);
        }
        $commissionValue = floatval($transferCommission->Commission_value);
        $totalAmount = $amount + $commissionValue;

        // ✅ 4. التحقق من حدود التحويل (باستثناء الفروع والوكلاء)
        
            $error = $this->checkTransferLimits($AccID, $totalAmount);
            if ($error instanceof \Illuminate\Http\JsonResponse) {
                return $error; // خروج فوري عند الخطأ
            }
        

        // ✅ 5. التحقق من معدل التحويل للفروع أو الوكلاء
        if (in_array($user->UeserType, ["3", "5"])) {
            $rollbackResult = $this->Rollback_Branch_Trinsfrim_me($request->branch_id, $totalAmount);
            if ($rollbackResult instanceof \Illuminate\Http\JsonResponse) {
                return $rollbackResult; // خروج فوري عند الخطأ
            }
        }

        // ✅ 6. التحقق من الرصيد
        $step = 'wallet_check';

        $wallet = wallet::where('UeserID', $user_id)
            ->where('Currency_ID', $request->currency_id)
            ->where('Walet', '>=', $totalAmount)
            ->first();

        if (!$wallet && $user->UeserType != "5") {
            return $this->sendError([
                'wallet'     => $wallet,
                'amount'     => $amount,
                'Commission' => $commissionValue,
                'total'      => $totalAmount,
            ], 'رصيد غير كافي', 422);
        }
     // ✅ 7. بدء المعاملة
        DB::beginTransaction();

        // ✅ 8. توليد كود تحويل فريد
        $step = 'generate_and_reserve_code';

        $codeForMobile = null;
        do {
            $maxCode = DB::table('MobileTransferCodes')->lockForUpdate()->max('codeForMobile') ?? 0;
            $codeForMobile = $maxCode + 1;

            $exists = DB::table('MobileTransferCodes')
                ->where('codeForMobile', $codeForMobile)
                ->exists();
        } while ($exists);

        // ✅ 9. حجز الكود
        DB::table('MobileTransferCodes')->insert([
            'codeForMobile' => $codeForMobile,
            'user_id'       => $user_id,
            'status'        => 'reserved',
            'created_at'    => now(),
        ]);
           // ✅ 10. إدخال سجل التحويل
        $step = 'insert_transfer';

        $insertedId = DB::table('TransBetweenAccountsTB')->insertGetId([
            'TransFrom'      => $request->acc_id,
            'TransTo'        => $request->acc_id_to,
            'CurrencyID'     => $request->currency_id,
            'TransValue'     => $request->amount,
            'BranchID'       => $request->branch_id,
            'ACC_typeMobile' => 1,
            'codeForMobile'  => $codeForMobile,
            'Notes'          => $request->Notes ?? null,
            'ExtraVal'       => $commissionValue
        ]);

        // ✅ 11. تحديث حالة الكود إلى "used"
        DB::table('MobileTransferCodes')
            ->where('codeForMobile', $codeForMobile)
            ->update(['status' => 'used']);

        // ✅ 12. جلب تفاصيل العملية
        $result = DB::table('TransBetweenAccountsTB as a')
            ->join('AccountsTb as b', 'a.TransFrom', '=', 'b.AccID')
            ->join('AccountsTb as c', 'a.TransTo', '=', 'c.AccID')
            ->leftJoin('users as d', 'a.TransFrom', '=', 'd.AccID')
            ->where('a.codeForMobile', $codeForMobile)
            ->select([
                'a.Code',
                'a.ID',
                'b.AccName as senderName',
                'c.AccName as recievedName',
                DB::raw('0 as customZero'),
                'a.TransValue',
                DB::raw('1 as typeFrom'),
                'b.AccPhone as SPHONE',
                'c.AccPhone as RPHNE',
                'a.Notes',
                'a.TransDate AS InsertDate',
                'a.InsertTime'
            ])
            ->first();

        if (!$result) {
            throw new \Exception("لم يتم العثور على بيانات التحويل بعد الإدخال.");
        }

        // ✅ 13. إضافة العمولة والمبلغ الأصلي للرد
        $result->commission     = $commissionValue;
        $result->amount         = $amount;
        $result->codeForMobile  = $codeForMobile;

        DB::commit();

        return $this->sendResponse(['result' => $result], 'تمت عملية التحويل بنجاح');

    } catch (\Exception $e) {
        DB::rollBack();

        // ✅ في حالة وجود كود حجز، يتم إلغاؤه عند الخطأ
        if (!empty($codeForMobile)) {
            DB::table('MobileTransferCodes')
                ->where('codeForMobile', $codeForMobile)
                ->update(['status' => 'cancelled']);
        }       return $this->sendError("خطأ أثناء تنفيذ العملية في المرحلة: $step", $e->getMessage(), 500);
    }
}


//  حساب فرق التحويل بين الحسبات التوقيت
       public function check_between_time(Request $request     )
       {


        $validator = Validator::make($request->all(), [
            'acc_id' => 'required'  
            
        ]);
        
        if ($validator->fails()) {
            return $this->sendError('Validation Error.', $validator->errors(), 422);
        }
    
    
    
    
    
        $user_id = Auth::User()->id;
        $results = DB::select("
        SELECT 
            DATEDIFF(MINUTE, 
                     CAST(TransDate AS DATETIME) + CAST(InsertTime AS DATETIME),  
                     GETDATE()
            ) AS DifferenceInMinutes
        FROM TransBetweenAccountsTB
        WHERE TransFrom = ? 
        AND ID = (
            SELECT MAX(ID) 
            FROM TransBetweenAccountsTB 
            WHERE TransFrom = ?
        )
    ", [$request->acc_id ,  $request->acc_id]);  
    
    
       return $this->sendResponse( $results , 'Success');
      }
 
      
      /**
       * تسعيرة حوالة خارجية — بنفس حساب المُشغِّل ExternalEx_insert_Mobile.
       *
       * externalGetExchnage الموجودة تعيد sale_price من
       * SalePrice_mo_Value(currency_id, ...) بينما المُشغِّل يستدعيها بـ
       * SalePrice_mo_Value(CountryIDTo, ...) — أي وسيط أول مختلف تماماً،
       * فالنتيجة ليست ما يستلمه المستفيد: لمبلغ 5 د.ل إلى مصر تعيد 2 بينما
       * الصف الفعلي في ExternalEx يسجّل NetTotal = 19. عرضها للوكيل يعني
       * تسعير خاطئ للزبون، فلا تُستعمل.
       *
       * هنا نكرّر منطق المُشغِّل حرفياً، قراءةً فقط:
       *   SalePrice        ← NewCurrencyPriceOwnDetailsTb (PriceType=2, AccountType=3)
       *   CurrDeliveredVal ← CurrRecievedVal × SalePrice
       *   NetTotal         ← SalePrice_mo_Value(CountryIDTo, ...)   ← ما يستلمه المستفيد
       *   ServiceExVal     ← CurrDeliveredVal − NetTotal
       *
       * أي تعديل في المُشغِّل يجب أن يُنقل هنا وإلا انحرفت التسعيرة عن التنفيذ.
       */
      public function externalQuote(Request $request)
      {
          $validator = Validator::make($request->all(), [
              'CountryIDTo'      => 'required|integer',
              'CurrRecievedVal'  => 'required|numeric|min:0.01',
              'ServiceType'      => 'required|integer',
              'IsPrivateAccount' => 'nullable|integer',
          ]);

          if ($validator->fails()) {
              return $this->sendError('Validation Error.', $validator->errors(), 422);
          }

          $countryTo = (int) $request->CountryIDTo;
          $amount    = floatval($request->CurrRecievedVal);
          $service   = (int) $request->ServiceType;
          $isPrivate = (int) ($request->IsPrivateAccount ?? 0);

          $rateRow = DB::selectOne("
              SELECT ISNULL(a.SalePrice, 1) AS SalePrice
              FROM NewCurrencyPriceOwnDetailsTb AS a
              INNER JOIN NewCurrencyPricesOwnTb AS b ON a.CPID = b.ID
              INNER JOIN CountiresTb AS c ON b.CountryID = c.ID AND a.CurrencyIDTo = c.DefualtCurrency
              WHERE a.CurrencyIDFrom = 1
                AND b.PriceType = 2
                AND b.AccountType = 3
                AND b.CountryID = ?
          ", [$countryTo]);

          if (!$rateRow) {
              return $this->sendError(
                  'لا يوجد سعر تحويل معرَّف لهذه الوجهة.',
                  ['CountryIDTo' => $countryTo],
                  422
              );
          }

          $rate      = floatval($rateRow->SalePrice);
          $delivered = $amount * $rate;

          $netRow = DB::selectOne(
              "SELECT ISNULL(dbo.SalePrice_mo_Value(?, ?, ?, ?), 0) AS NetTotal",
              [$countryTo, $amount, $service, $isPrivate]
          );

          $net = floatval($netRow->NetTotal);

          $currency = DB::selectOne("
              SELECT c.DefualtCurrency AS ID, m.CurCode, m.CuName
              FROM CountiresTb AS c
              LEFT JOIN CurrencyMainTb AS m ON m.ID = c.DefualtCurrency
              WHERE c.ID = ?
          ", [$countryTo]);

          return $this->sendResponse([
              'CurrRecievedVal'   => $amount,
              'TransPrice'        => $rate,
              'CurrDeliveredVal'  => $delivered,
              'NetTotal'          => $net,
              'ServiceExVal'      => $delivered - $net,
              'DeliveredCurrency' => [
                  'ID'      => $currency->ID      ?? null,
                  'CurCode' => $currency->CurCode ?? null,
                  'CuName'  => $currency->CuName  ?? null,
              ],
          ], 'تم احتساب التسعيرة');
      }

      public function externalGetExchnage(Request $request)
      {
          $user = Auth::user();
      
          if (!$user) {
              return response()->json([
                  'success' => false,
                  'message' => 'يجب تسجيل الدخول أولاً للوصول إلى هذه الخدمة.',
              ], 401);
          }
      
          // ================== Validation ==================
          $validator = Validator::make($request->all(), [
              'currency_id'       => 'required|integer',
              'amount'            => 'required|numeric|min:0',
              'service_type'      => 'required|integer',
              'type'              => 'required|in:1,2,3',
              'Type_From'         => 'required|integer',
              'commission_value'  => 'nullable|numeric|min:0'
          ]);
      
          if ($validator->fails()) {
              return $this->sendError('Validation Error.', $validator->errors(), 422);
          }
      
          $currencyId  = (int) $request->currency_id;
          $amount      = (float) $request->amount;
          $serviceType = (int) $request->service_type;
          $type        = (int) $request->type;
          $typeFrom    = (int) $request->Type_From;
      
          // ================== 1. Sale Price ==================
          $salePrice = DB::selectOne("
              SELECT dbo.SalePrice_mo_Value(?, ?, ?, ?) AS a
          ", [$currencyId, $amount, $serviceType, $typeFrom]);
      
          $salePrice = $salePrice->a ?? 0;
      
          // ================== 2. Commission ==================
          $commissionValue = 0;
      
          if ($user->UeserType == 3 || $user->UeserType ==  5) {
      
              $commissionValue = (float) $request->commission_value;
      
          } else {
      
              $transferCommission = Transfer_commissions::where('First_Value', '<=', $amount)
                  ->where('Second_value', '>=', $amount)
                  ->value('Commission_value');
      
              if ($transferCommission) {
                  $commissionValue = (float) $transferCommission;
              }
          }
      
          // ================== 3. Buy Price ==================
          $buyPrice = DB::selectOne("
              SELECT a.SalePrice 
              FROM NewCurrencyPriceOwnDetailsTb AS a
              INNER JOIN NewCurrencyPricesOwnTb AS b ON a.CPID = b.ID
              INNER JOIN CountiresTb AS c 
                  ON b.CountryID = c.ID 
                  AND a.CurrencyIDTo = c.DefualtCurrency
              WHERE a.CurrencyIDFrom = ?
              AND b.PriceType = ?
              AND b.AccountType = ?
              AND b.CountryID = ? 
              and b.BankID = ?
          ", [1, 2, 3, $currencyId ,$serviceType ]);
      
          $buyPrice = $buyPrice->SalePrice ?? 1;
      
          // ================== Response ==================
          $response = [
              'sale_price'        => ($type == 3) ? (float)$salePrice : 0,
              'commission_value'  => (float)$commissionValue,
              'buy_price'         => (float)$buyPrice,
          ];
      
          return $this->sendResponse($response, 'Success');
      }
      
      

   




           //  كود احتساب الوقت external 
     public function getDiffernceTimeExternal(Request $request     ){ 

        $validator = Validator::make($request->all(), [
            'acc_id' => 'required' ,
      
        ]);
        
        if ($validator->fails()) {
            return $this->sendError('Validation Error.', $validator->errors(), 422);
        }
    
    
 
    
        $user_id = Auth::User()->id;
        $results = DB::select("
        SELECT ISNULL(
            (
                SELECT DATEDIFF(MINUTE, 
                    CAST(InsertDate AS DATETIME) + CAST(InsertTime AS DATETIME),  
                    GETDATE()
                )
                FROM ExternalEx
                WHERE AccFrom = ?
                AND ID = (SELECT MAX(ID) FROM ExternalEx WHERE AccFrom = ?)
            ), 6
        ) AS DifferenceInMinutes
    ", [ $request->acc_id ,  $request->acc_id ]);
    
    
  
    
       return $this->sendResponse( $results , 'Success');
      }


 


//  كود الحوالة الخارجية اضافة.  
public function transInsertExternal(Request $request)
{
    // ✅ تحقق من البيانات المطلوبة
    $validator = Validator::make($request->all(), [
        'RecievedCurrencyID'   => 'required',
        'CountryIDFrom'        => 'required',
        'RecievedBranchID'     => 'required',
        'RecievedName'         => 'required',
        'RPhone1'              => 'required',
        'CityIDTo'             => 'required',
        'DeliveredCurrencyID'  => 'required',
        'CountryIDTo'          => 'required',
        'ServiceType'          => 'required',
        'CurrRecievedVal'      => 'required',
        'AccFrom'              => 'required',
        'OwnAccNo'             => 'nullable' ,
        'IsPrivateAccount'     => 'required' , 
        'Commition'      => 'required|numeric|min:0',
        'SenderName'     => 'nullable',
        'SPhone1'        => 'nullable'
    ]);

    if ($validator->fails()) {
        return $this->sendError('Validation Error.', $validator->errors(), 422);
    }

    $user = Auth::user();
    $user_id = $user->id;
    $AccID = $user->AccID;

    // ================= 3 دقائق منع التحويل المتكرر =================
    $check = DB::selectOne("
        SELECT DATEDIFF(MINUTE, InsertDate , GETDATE()) AS DifferenceInMinutes
        FROM ExternalEx
        WHERE AccFrom = ? 
          AND ID = (SELECT MAX(ID) FROM ExternalEx WHERE AccFrom = ?)
    ", [$AccID, $AccID]);

    $DifferenceInMinutes = $check ? $check->DifferenceInMinutes : 10;

    if ($DifferenceInMinutes <= 1) {
        return $this->sendError(
            'لا يمكن التحويل الآن',
            ['message' => 'يمكن المحاولة بعد 1 دقائق'],
            422
        );
    }

    // ================= حساب العمولة =================
    $transferCommission = Transfer_commissions::where('First_Value', '<=', $request->CurrRecievedVal)
        ->where('Second_value', '>=', $request->CurrRecievedVal)
        ->first();



         // ✅ إضافة التعديل فقط
         $isTypeFive = $user->UeserType == "5";
 
         $senderName = $isTypeFive
             ? ($request->input('SenderName') ?: $user->name)
             : $user->name;
 
         $senderPhone = $isTypeFive
             ? ($request->input('SPhone1') ?: $user->phone)
             : $user->phone;
 
        



        
        if ($user->UeserType == 3 || $user->UeserType == 5 )
        {
            $commissionValue = floatval($request->Commition );

        }else {
            if (!$transferCommission) {
                return $this->sendError('خطأ في احتساب العمولة', ['message' => 'لا توجد عمولة مطابقة لهذا المبلغ'], 422);
            }
            $commissionValue = floatval($transferCommission->Commission_value);

        }
 
    $totalAmount     = $commissionValue + floatval($request->CurrRecievedVal);

    // ================= التحقق من حدود التحويل =================
    
        $limitCheck = $this->checkTransferLimits($AccID, $totalAmount);
        if ($limitCheck instanceof \Illuminate\Http\JsonResponse) {
            // تعديل الرد ليكون بصيغة violations موحدة
            $decoded = json_decode($limitCheck->getContent(), true);
            return response()->json([
                'success'    => false,
                'violations' => $decoded['violations'] ?? [],
                'total'      => $decoded['total'] ?? 0,
                'message'    => $decoded['message'] ?? 'لقد تجاوزت حدود التحويل'
            ], 422);
        }
    // ================= التحقق من معدل التحويل للفروع أو الوكلاء =================
    if (in_array($user->UeserType, ["3", "5"])) {
        $rollbackResult = $this->Rollback_Branch_Trinsfrim_me($request->RecievedBranchID, $totalAmount);
        if ($rollbackResult instanceof \Illuminate\Http\JsonResponse) {
            return $rollbackResult; // خروج فوري عند الخطأ
        }
    }

    // ================= التحقق من الرصيد =================
    $wallet = wallet::where('UeserID', $user_id)
        ->where('Currency_ID', $request->RecievedCurrencyID)
        ->where('Walet', '>=', $totalAmount)
        ->first();    

    if ($user->UeserType != "3" && !$wallet && $user->UeserType != "5") {
        return $this->sendError([
            'wallet'     => $wallet,
            'amount'     => floatval($request->CurrRecievedVal),
            'Commission' => $commissionValue,
            'total'      => $totalAmount
        ], 'رصيد غير كافي', 422);
    }

    // ================= توليد كود فريد للموبايل =================
    $maxAttempts    = 5;
    $attempt        = 0;
    $codeForMobile  = null;

    while ($attempt < $maxAttempts) {
        $attempt++;
        $userPart    = str_pad(substr($user_id, -3), 3, '0', STR_PAD_LEFT);
        $timePart    = date('s'); 
        $randomPart  = rand(10, 99);
        $codeForMobile = $userPart . $timePart . $randomPart;

        $exists = DB::table('ExternalEx')->where('codeForMobile', $codeForMobile)->exists();
        if (!$exists) break;
    }

    if ($attempt >= $maxAttempts && $exists) {
        return $this->sendError('فشل في توليد كود فريد للموبايل بعد عدة محاولات', [], 500);
    }

    DB::beginTransaction();

    try {
        // ================= إدخال سجل التحويل =================
        DB::insert("
            INSERT INTO [dbo].[ExternalEx] (
                [RecievedCurrencyID],[CountryIDFrom],[RecievedBranchID],[RecievedName],[RPhone1],
                [CityIDTo],[DeliveredCurrencyID],[CountryIDTo],[ServiceType],[CurrRecievedVal],
                [ExVal],[IsAccFrom],[AccFrom],[Type_Moble],[OwnAccNo],[codeForMobile],[Notes],
                [IsPrivateAccount],[SenderName] ,[Phone1],uesrID_forminsertmobile
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,?,?,?)
        ", [
            $request->RecievedCurrencyID,
            $request->CountryIDFrom,
            $request->RecievedBranchID,
            $request->RecievedName,
            $request->RPhone1,
            $request->CityIDTo,
            $request->DeliveredCurrencyID,
            $request->CountryIDTo,
            $request->ServiceType,
            $request->CurrRecievedVal,
            $commissionValue,
            1,
            $user->AccID ,
            1,
            $request->OwnAccNo,
            $codeForMobile,
            $request->Notes,
            $request->IsPrivateAccount ,
            $senderName , 
            $senderPhone ,
            $user->id
        ]);

        // ================= جلب بيانات العملية للرد =================
        $transfer = DB::table('ExternalEx')
            ->where('codeForMobile', $codeForMobile)
            ->first();

        if ($transfer) {
            $transfer->Type_from = 3;
        }

        DB::commit();

        return $this->sendResponse(['transfer' => $transfer], 'تم تنفيذ العملية بنجاح');
  } catch (\Exception $e)
   {
        DB::rollBack();
        \Log::error('خطأ أثناء تنفيذ transInsertExternal: ' . $e->getMessage(), [
            'line'  => $e->getLine(),
            'file'  => $e->getFile(),
            'trace' => $e->getTrace()
        ]);

        return $this->sendError('خطأ في تنفيذ العملية', [
            'message' => $e->getMessage(),
            'line'    => $e->getLine(),
            'file'    => $e->getFile()
        ], 500);   }
}
    public function ListUsersAddedToTrans(Request $request     )
    { 
 
 
        $user_id = Auth::User()->id;
        $results = DB::select("
        SELECT 
            d.AccName, 
            f.phone as AccPhone ,  
            a.inserdate, 
            d.AccCode,
            a.ID
        FROM [dbo].[AddUserTransTb] AS a
        INNER JOIN AccountsTb AS c ON a.ID_UESER_ACCID = c.AccID 
        INNER JOIN AccountsTb AS d ON d.AccID = a.UserTo_ACCID 
        INNER JOIN users AS e ON a.ID_UESER_ACCID = e.AccID
        INNER JOIN users AS f ON d.AccID = f.AccID
        WHERE e.id = ?
    ", [$user_id]);
    
 
    
       return $this->sendResponse( $results , 'Success');
      }

    //  

public function addUserTrans(Request $request     ){   

    $validator = Validator::make($request->all(), [
       
        
      
        'acc_id' => 'required' ,
        'acc_to' => 'required' ,
   



    ]);
    
    if ($validator->fails()) {
        return $this->sendError('Validation Error.', $validator->errors(), 422);
    }

    $user_id = Auth::User()->id;
 
    DB::beginTransaction();

try {


    DB::insert("
    INSERT INTO [dbo].[AddUserTransTb] 
        (ID_UESER_ACCID, UserTo_ACCID) 
    VALUES (?, ?)
", [$request->acc_id , $request->acc_to]);


    DB::commit();

} catch (\Exception $e) {
    // Rollback transaction if something goes wrong
    DB::rollBack();
    throw $e;
}



    return $this->sendResponse(  "success" , 'Success');
}








               //  
  public function deleteUser(Request $request     ){ 
 
 
    $validator = Validator::make($request->all(), [
       
        
      
        'id' => 'required' 
    
   



    ]);
    
    if ($validator->fails()) {
        return $this->sendError('Validation Error.', $validator->errors(), 422);
    }
 
        DB::delete("DELETE FROM AddUserTransTb WHERE ID = ?", [$request->id ]);

               return $this->sendResponse(  "success" , 'Success');
              }




 



}
