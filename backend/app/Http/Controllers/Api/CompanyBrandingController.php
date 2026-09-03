<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\BrandingThemes;
use App\Services\CompanyBrandingService;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\DB;

/**
 * هوية الشركة — طبقة عرض، لا مالية.
 *
 * ⚠ العزل هنا وفي الخدمة معاً: مفتاح الشركة يُشتقّ من `Auth::user()->AccID`
 * ولا يُقرأ من جسم الطلب أبداً. لو قُرئ منه لكفى تعديلُ رقمٍ في الطلب ليغيّر
 * وكيلٌ هويةَ شركةٍ أخرى — وهو شرط المالك رقم 25.
 *
 * والصلاحية: الحساب الرئيسي (`AccountType = 'Main'`) وحده يعدّل، ونقاط
 * البيع تقرأ. لم يُخترع جدول صلاحيات جديد لأن هذا التمييز قائم في المنظومة
 * أصلاً، وجدولٌ ثانٍ للصلاحيات يعني مكانين يمكن أن يختلفا.
 */
class CompanyBrandingController extends BaseController
{
    public function __construct(private CompanyBrandingService $service)
    {
    }

    /** رقم حساب الشركة، أو null إن لم يكن للمستخدم حساب. */
    private function companyId($user): ?int
    {
        return empty($user->AccID) ? null : (int) $user->AccID;
    }

    private function canEdit($user): bool
    {
        return ($user->AccountType ?? '') === 'Main';
    }

    private function trace(Request $request): array
    {
        return [
            'ip'     => $request->ip(),
            'device' => $request->header('X-Device-Id'),
        ];
    }

    /**
     * GET /api/company/branding
     *
     * تُقرأ عند كل دخول وعند فتح التطبيق. لا تفشل أبداً على شركةٍ بلا هوية
     * محفوظة — تعود بالافتراضية، فالوكيل يرى تطبيقاً كاملاً لا شاشة خطأ.
     */
    public function show(Request $request)
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $companyId = $this->companyId($user);
        if ($companyId === null) {
            // لا حساب ⇒ الهوية الافتراضية بلا اسم شركة. لا خطأ: المستخدم
            // يجب أن يدخل التطبيق.
            return $this->sendResponse([
                'branding'  => $this->service->forCompany(0),
                'can_edit'  => false,
                'themes'    => $this->themesCatalog(),
            ], 'Success');
        }

        return $this->sendResponse([
            'branding' => $this->service->forCompany($companyId, $this->accountName($companyId)),
            'can_edit' => $this->canEdit($user),
            'themes'   => $this->themesCatalog(),
        ], 'Success');
    }

    /**
     * اسم الحساب من شجرة الحسابات — يُستعمل اسماً افتراضياً للشركة قبل أن
     * تكتب لنفسها اسماً. قراءة فقط، ولا يُكتب إليه أبداً.
     */
    private function accountName(int $accId): ?string
    {
        try {
            $row = DB::table('AccountsTb')
                ->where('AccID', $accId)
                ->first(['AccName']);
            return $row->AccName ?? null;
        } catch (\Throwable $e) {
            return null;
        }
    }

    /** الكتالوج بصيغة قائمة — يعرضه التطبيق شبكةَ اختيار. */
    private function themesCatalog(): array
    {
        $out = [];
        foreach (BrandingThemes::all() as $key => $t) {
            $out[] = ['key' => $key] + $t;
        }
        return $out;
    }

    /** PUT /api/company/branding */
    public function update(Request $request)
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $companyId = $this->companyId($user);
        if ($companyId === null) {
            return $this->sendError('لا يوجد حساب شركة مرتبط بهذا المستخدم.', [], 403);
        }
        if (!$this->canEdit($user)) {
            return $this->sendError(
                'تعديل هوية الشركة متاح للحساب الرئيسي فقط.', [], 403
            );
        }

        $hex = 'regex:/^#[0-9A-Fa-f]{6}$/';

        $data = $request->validate([
            'company_name_ar'  => 'sometimes|nullable|string|max:200',
            'company_name_en'  => 'sometimes|nullable|string|max:200',
            'theme_key'        => 'sometimes|string|max:40',
            'primary_color'    => "sometimes|nullable|string|$hex",
            'secondary_color'  => "sometimes|nullable|string|$hex",
            'background_color' => "sometimes|nullable|string|$hex",
        ]);

        if (isset($data['theme_key']) && !BrandingThemes::has($data['theme_key'])) {
            return $this->sendError('مفتاح ثيم غير معروف.', [], 422);
        }

        // اسمٌ فارغ لا يُحفظ فارغاً: شاشةٌ بلا اسم شركة أسوأ من اسمٍ قديم.
        if (array_key_exists('company_name_ar', $data)
            && trim((string) $data['company_name_ar']) === '') {
            unset($data['company_name_ar']);
        }

        $branding = $this->service->save(
            $companyId, (int) $user->id, $data, $this->trace($request)
        );

        return $this->sendResponse(
            ['branding' => $branding], 'تم حفظ هوية الشركة.'
        );
    }

    /** POST /api/company/branding/logo  (multipart: logo) */
    public function uploadLogo(Request $request)
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $companyId = $this->companyId($user);
        if ($companyId === null) {
            return $this->sendError('لا يوجد حساب شركة مرتبط بهذا المستخدم.', [], 403);
        }
        if (!$this->canEdit($user)) {
            return $this->sendError(
                'تعديل هوية الشركة متاح للحساب الرئيسي فقط.', [], 403
            );
        }

        if (!$request->hasFile('logo')) {
            return $this->sendError('لم يُرفَق ملف الشعار.', [], 422);
        }

        $file = $request->file('logo');
        if (!$file->isValid()) {
            return $this->sendError('فشل رفع الملف — أعد المحاولة.', [], 422);
        }

        try {
            $branding = $this->service->saveLogo(
                $companyId, (int) $user->id, $file, $this->trace($request)
            );
        } catch (\InvalidArgumentException $e) {
            return $this->sendError($e->getMessage(), [], 422);
        }

        return $this->sendResponse(['branding' => $branding], 'تم حفظ الشعار.');
    }

    /**
     * GET /api/company/branding/logo/{name}
     *
     * خارج مجموعة التوثيق عمداً: `Image.network` في فلاتر يطلب الصورة بطلبٍ
     * منفصل بلا ترويسة التوثيق، وحقن الترويسة في كل موضع يعرض الشعار مصدر
     * أعطال أكثر مما يحمي. والشعار ليس سرّاً — هو ما يُعلَّق على الواجهة —
     * واسم الملف عشوائي 24 حرفاً فلا يُخمَّن ولا يُعدّ من شركة إلى أخرى.
     */
    public function logo(string $name)
    {
        $file = $this->service->logoStream($name);
        if ($file === null) {
            return $this->sendError('الشعار غير موجود.', [], 404);
        }

        return response()->stream(function () use ($file) {
            fpassthru($file['stream']);
        }, 200, [
            'Content-Type'   => $file['mime'],
            'Content-Length' => $file['size'],
            // الاسم عشوائي وثابت لكل شعار، فالتخزين المؤقّت الطويل آمن:
            // شعارٌ جديد = اسمٌ جديد = رابطٌ جديد.
            'Cache-Control'  => 'public, max-age=604800',
        ]);
    }

    /** POST /api/company/branding/reset — الثيم والألوان فقط. */
    public function reset(Request $request)
    {
        $user = Auth::user();
        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $companyId = $this->companyId($user);
        if ($companyId === null) {
            return $this->sendError('لا يوجد حساب شركة مرتبط بهذا المستخدم.', [], 403);
        }
        if (!$this->canEdit($user)) {
            return $this->sendError(
                'تعديل هوية الشركة متاح للحساب الرئيسي فقط.', [], 403
            );
        }

        $branding = $this->service->resetTheme(
            $companyId, (int) $user->id, $this->trace($request)
        );

        return $this->sendResponse(
            ['branding' => $branding], 'تمت استعادة الهوية الافتراضية.'
        );
    }
}
