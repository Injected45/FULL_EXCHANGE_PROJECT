<?php

namespace App\Services;

use Illuminate\Support\Facades\DB;
use GuzzleHttp\Client;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Http;
use Carbon\Carbon;
class Watsaoserversfrom 
{
  
public function sendFormasggme(string $phone, string $body)
{
    // API Key من إعدادات Laravel
    $apiKey = config('services.whatsapp.token');

    // تنظيف الرقم
    $phone = preg_replace('/[^0-9]/', '', $phone);

    // تحويل الرقم إلى WhatsApp Chat ID
    $chatId = $phone ;

    try {

        $response = Http::withOptions([
            'verify' => false,
            'timeout' => 10,
        ])
        ->withHeaders([
            'X-API-Key' => $apiKey,
            'Content-Type' => 'application/json',
        ])
        ->post(
            'https://wa.rhalla.online/api/sessions/867ea69e-1926-4a85-baa8-2c1b65a068aa/messages/send-text',
            [
                'chatId' => $chatId,
                'text'   => $body,
            ]
        );

        if (!$response->successful()) {
            \Log::error('WhatsApp Send Error', [
                'phone' => $phone,
                'response' => $response->body(),
                'status' => $response->status(),
            ]);

            return [
                'success' => false,
                'message' => 'فشل إرسال رسالة WhatsApp',
                'data' => $response->body(),
            ];
        }

        return [
            'success' => true,
            'data' => $response->json(),
        ];

    } catch (\Exception $e) {

        \Log::error('WhatsApp Exception', [
            'phone' => $phone,
            'error' => $e->getMessage(),
        ]);

        return [
            'success' => false,
            'message' => 'فشل الاتصال بخدمة WhatsApp',
            'data' => $e->getMessage(),
        ];
    }
}


/**
 * إرسال رسالة إلى مجموعة واتساب.
 *
 * كانت مفقودة: sendAgentTransactionMessage تستدعيها في السطر ~157 وهي غير
 * معرّفة، فكان **كل إنشاء حوالة داخلية من وكيل يفشل** بـ
 * «Call to undefined method … sendFormaGROUP()» — والاستدعاء داخل
 * DB::transaction، فتُدرَج الحوالة ثم تُلغى بالكامل.
 *
 * ولا تصلح sendFormasggme بديلاً: هي تجرّد كل ما ليس رقماً من المُعرّف،
 * ومعرّف المجموعة يحمل «-» و«@g.us» فيُدمَّر. هنا يُمرَّر كما هو.
 */
public function sendFormaGROUP(string $groupId, string $body)
{
    $apiKey = config('services.whatsapp.token');

    try {
        $response = Http::withOptions([
            'verify'  => false,
            'timeout' => 10,
        ])
        ->withHeaders([
            'X-API-Key'    => $apiKey,
            'Content-Type' => 'application/json',
        ])
        ->post(
            'https://wa.rhalla.online/api/sessions/867ea69e-1926-4a85-baa8-2c1b65a068aa/messages/send-text',
            [
                // معرّف المجموعة كما هو — بلا تنظيف.
                'chatId' => $groupId,
                'text'   => $body,
            ]
        );

        if (!$response->successful()) {
            \Log::error('WhatsApp Group Send Error', [
                'group'    => $groupId,
                'status'   => $response->status(),
                'response' => $response->body(),
            ]);

            return [
                'success' => false,
                'message' => 'فشل إرسال رسالة المجموعة',
                'data'    => $response->body(),
            ];
        }

        return ['success' => true, 'data' => $response->json()];
    } catch (\Throwable $e) {
        \Log::error('WhatsApp Group Send Exception', [
            'group' => $groupId,
            'error' => $e->getMessage(),
        ]);

        return [
            'success' => false,
            'message' => 'فشل الاتصال بخدمة WhatsApp',
            'data'    => $e->getMessage(),
        ];
    }
}


// إرسال رسالة حوالة وكيل
public function sendAgentTransactionMessage(string $code)
{
    $user = Auth::user();

    if ($user->UeserType == 3) {

        $whatsappGroup = DB::table('whtsappGRoup')
            ->where('Branchid', $user->BrancchID)
            ->first();

        $result = DB::table('ExchangeAccData')
            ->join(
                'users',
                'ExchangeAccData.AccID',
                '=',
                'users.AccID'
            )
            ->where('users.id', $user->id)
            ->where('ExchangeAccData.IsActive', 1)
            ->where('ExchangeAccData.Code', $code)
            ->select([
                'ExchangeAccData.AccCode',
                'ExchangeAccData.AccName',
                'ExchangeAccData.TransDate',
                'ExchangeAccData.TransValue',
                'ExchangeAccData.commint',
                'ExchangeAccData.ServiceName',
                'ExchangeAccData.CName',
                'ExchangeAccData.NetTotal',
            ])
            ->first();

        $getname = DB::table('getInfo')
            ->where('id', $user->id)
            ->first();

        if ($result && $whatsappGroup) {

            $amountNumber = $result->TransValue;

            $amount = number_format(
                $amountNumber,
                2
            );

            $fee = number_format(
                $result->commint ?? 0,
                2
            );

            $carbonDate = Carbon::parse(
                $result->TransDate
            );

            $date = $carbonDate->format(
                'Y-m-d'
            );

            $hour = $carbonDate->format(
                'H:i'
            );

            $amountText = $this->tafqit(
                $amountNumber
            );

            $message  = "🔹 مرسلة من : {$getname->AccName}\n";
            $message .= "🔸 CODE : {$code}\n";
            $message .= "👤 اسم المستلم : {$result->AccName}\n";
            $message .= "📞 هاتف المستلم : {$result->AccCode}\n";
            $message .= "📍 مدينة : {$result->CName}\n";
            $message .= "💰 القيمه : {$amount} د.ل\n";
            $message .= "✍️ فقط {$amountText} دينار ليبي لاغير\n";
            $message .= "🧾 العمولة : {$fee} د.ل\n";
            $message .= "📅 التاريخ : {$date}\n";
            $message .= "⏰ الوقت : {$hour}\n";
            $message .= "🙏 شكراً لتعاملكم معنا";

            // إرسال الرسالة إلى مجموعة WhatsApp
            $this->sendFormaGROUP(
                $whatsappGroup->IDGroup,
                $message
            );
        }
    }
}


    
    private function tafqit($number)
{
    $formatter = new \NumberFormatter("ar", \NumberFormatter::SPELLOUT);
    return $formatter->format($number);
}
}