<?php

namespace App\Http\Controllers\Api;


use Illuminate\Http\Request;
use Illuminate\Routing\Controller;
use Modules\BankVisaTransfers\Models\BankVisaTransfer;
use Illuminate\Validation\ValidationException;
use Illuminate\Database\QueryException;
use Illuminate\Support\Facades\DB;

class BankVisaTransferController extends Controller
{
    public function BankVisaTransfer_insert(Request $request)
    {
        try {
            // ✅ Validation كامل للحقول مع رسائل عربية
            $validatedData = $request->validate([
                'FullName'       => 'required|string|min:3',
                'NationalID'     => 'required|string|max:20|unique:BankVisaTransfers_CentralLibya,NationalID',
                'AmountUSD'      => 'required|numeric|min:0',
                'AmountLocal'    => 'required|numeric|min:0',
                'ExchangeRate'   => 'required|numeric|min:0',
                'AccountNumber'  => 'required|string|max:50|unique:BankVisaTransfers_CentralLibya,AccountNumber',
                'Phone'          => 'required|string|max:50' ,
                'code'           =>'required|string'
            ], [
                'FullName.required'      => 'الاسم الكامل مطلوب',
                'FullName.string'        => 'الاسم يجب أن يكون نصًا',
                'FullName.min'           => 'الاسم يجب أن يكون على الأقل 3 أحرف',

                'NationalID.required'    => 'الرقم الوطني مطلوب',
                'NationalID.max'         => 'الرقم الوطني لا يمكن أن يزيد عن 20 حرف',
                'NationalID.unique'      => 'الرقم الوطني موجود مسبقًا',

                'AmountUSD.required'     => 'القيمة بالدولار مطلوبة',
                'AmountUSD.numeric'      => 'القيمة بالدولار يجب أن تكون رقمًا',
                'AmountUSD.min'          => 'القيمة بالدولار يجب أن تكون أكبر من أو تساوي 0',

                'AmountLocal.required'   => 'القيمة بالعملة المحلية مطلوبة',
                'AmountLocal.numeric'    => 'القيمة بالعملة المحلية يجب أن تكون رقمًا',
                'AmountLocal.min'        => 'القيمة بالعملة المحلية يجب أن تكون أكبر من أو تساوي 0',

                'ExchangeRate.required'  => 'سعر الصرف مطلوب',
                'ExchangeRate.numeric'   => 'سعر الصرف يجب أن يكون رقمًا',
                'ExchangeRate.min'       => 'سعر الصرف يجب أن يكون أكبر من أو يساوي 0',

                'AccountNumber.required' => 'رقم الحساب مطلوب',
                'AccountNumber.max'      => 'رقم الحساب لا يمكن أن يزيد عن 50 حرف',
                'AccountNumber.unique'   => 'رقم الحساب موجود مسبقًا',

                'Phone.string'           => 'رقم الهاتف يجب أن يكون نصًا',
                'Phone.max'              => 'رقم الهاتف لا يمكن أن يزيد عن 50 حرف',



                'code.required' => 'رقم العملية  مطلوب',
                'code.unique'   => 'رقم الحساب موجود مسبقًا',
            ]);

            // ✅ استخدام Transaction لحماية عمليات الإدخال عند التزامن
            $transfer = DB::transaction(function() use ($validatedData) {
                // TransferSeq يتولد تلقائيًا من SQL Sequence
                return BankVisaTransfer::create($validatedData);
            });

            return response()->json([
                'status' => true,
                'message' => 'تم إنشاء التحويل بنجاح',
                'data' => $transfer
            ], 200);

        } catch (ValidationException $e) {
            return response()->json([
                'status' => false,
                'message' => 'خطأ في البيانات المدخلة',
                'errors' => $e->errors()
            ], 422);

        } catch (QueryException $e) {
            return response()->json([
                'status' => false,
                'message' => 'خطأ في قاعدة البيانات: ربما الرقم الوطني أو رقم الحساب موجود مسبقًا',
                'errors' => $e->getMessage()
            ], 409);

        } catch (\Exception $e) {
            return response()->json([
                'status' => false,
                'message' => 'حدث خطأ غير متوقع أثناء معالجة الطلب',
                'errors' => $e->getMessage()
            ], 500);
        }
    }
}
