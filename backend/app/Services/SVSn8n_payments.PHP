<?php

namespace App\Services;

use App\Models\n8n_payments;
use Illuminate\Support\Facades\Log;

class SVSn8n_payments
{
    /**
     * البحث عن عملية دفع بالهاتف والمبلغ
     *
     * @param string $phone
     * @param float $amount
     * @return mixed
     */
    public function searchPayments(string $phone, float $amount)
    {
        try {

            return n8n_payments::where('amount', $amount)
                ->where('phone', $phone)
                ->orderBy('id', 'desc')
                ->first();

        } catch (\Exception $e) {

            Log::error('N8n Payments Search Error', [
                'phone' => $phone,
                'amount' => $amount,
                'error' => $e->getMessage()
            ]);

            return null;
        }
    }
}

