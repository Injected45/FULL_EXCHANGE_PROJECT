<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\CurrencyRatesService;

/**
 * أسعارُ العملات — **بابان وقراءةٌ واحدة**.
 *
 * أمرُ المالك (11 سبتمبر 2026): «يُفضَّل استخدامُ نفس مصدر البيانات ونفس API
 * للشاشتين في تطبيق الوكيل وتطبيق الموظف لضمان تطابق الأسعار وعدم حدوث
 * اختلافٍ بينهما».
 *
 * ⚠ فالمسارانِ يناديان [CurrencyRatesService] نفسَها، ولا استعلامَ ثانيَ
 * لأحدهما: استعلامان يفترقان عند أوّل تعديل، ثمّ يقرأ الوكيلُ سعراً ويقرأ
 * موظفُه غيرَه — وهما يقفان أمام زبونٍ واحد.
 *
 * ⚠ **والمختلفُ الحارسُ وحدَه**: رمزُ الموظف لا يفتح مسارات الوكيل، ورمزُ
 * الوكيل لا يفتح مسارات الموظف.
 *
 * ⚠ **وقراءةٌ خالصة**: لا `POST` ولا `PUT` ولا `DELETE` لهذه الشاشة، ولا
 * نقطةَ كتابةٍ واحدة. تغييرُ السعر فعلُ المكتب الخلفيّ وحدَه.
 */
class CurrencyRatesController extends BaseController
{
    public function __construct(private CurrencyRatesService $rates)
    {
    }

    /** GET device/currency-rates — للوكيل. */
    public function index()
    {
        return $this->sendResponse(['items' => $this->rates->list()], 'Success');
    }

    /**
     * GET device/employee/currency-rates — للموظف.
     *
     * ⚠ **بلا صلاحيةٍ إضافية**: السعرُ معلومةٌ يحتاجها كلُّ من يقف أمام زبون،
     * وهو يراه أصلاً في شاشة إنشاء الحوالة الخارجية حين يسعّر. ومفتاحٌ ثالثٌ
     * لبيانٍ يراه أصلاً يعني وكيلاً يمنع ما لا يُمنَع.
     *
     * ⚠ ولا رقمَ حسابٍ ولا رصيدَ في هذه القراءة — أسعارٌ فقط.
     */
    public function employeeIndex()
    {
        return $this->sendResponse(['items' => $this->rates->list()], 'Success');
    }
}
