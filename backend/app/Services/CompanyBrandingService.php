<?php

namespace App\Services;

use Illuminate\Http\UploadedFile;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Storage;
use Illuminate\Support\Str;

/**
 * هوية الشركة داخل التطبيق — طبقة عرض مستقلّة.
 *
 * ⚠ شرط المالك (2 سبتمبر 2026): «لا يجوز أن يؤثر تخصيص الهوية على الحسابات
 * أو الأرصدة أو الحوالات أو الصلاحيات أو التقارير أو العمليات المالية».
 *
 * فهذه الخدمة تكتب في `tenant_branding` و`tenant_branding_audit` فقط، ولا
 * تقرأ من `users` إلا مفتاح الشركة. أي كتابة خارجهما تخالف الشرط وتكسر
 * اختبار القبول 10.
 */
class CompanyBrandingService
{
    /**
     * قرص خاص لا عام، والشعار يُخدَّم عبر مسارٍ في الـ API.
     *
     * القرص العام يحتاج `storage:link` ويبني رابطه من `APP_URL` — وهي هنا
     * `http://localhost`، أي رابطٌ لا يفتحه هاتف ولا محاكٍ. والمسار في الـ API
     * يعمل أينما كان الخادم لأن التطبيق يركّبه على `API_BASE` الذي يعرفه.
     */
    private const DISK = 'local';
    private const DIR = 'branding';

    /** حدّ حجم الشعار. أكبر من ذلك يُبطئ فتح كل شاشة تعرضه. */
    public const MAX_BYTES = 2 * 1024 * 1024;

    public const MIN_SIDE = 64;
    public const MAX_SIDE = 2048;

    public const MIMES = ['image/png', 'image/jpeg', 'image/webp'];

    /**
     * هوية الشركة كاملةً، مع الثيم المحلول وألوان الحالات.
     *
     * لا تُرجع null أبداً: شركةٌ بلا هوية محفوظة تأخذ الافتراضية، فلا شاشة
     * فارغة ولا خطأ عند أول دخول — وهو شرط المالك رقم 19.
     */
    public function forCompany(int $companyAccountId, ?string $fallbackName = null): array
    {
        $row = DB::table('tenant_branding')
            ->where('company_account_id', $companyAccountId)
            ->where('is_active', 1)
            ->first();

        $themeKey = $row->theme_key ?? BrandingThemes::DEFAULT_KEY;
        $theme = BrandingThemes::get($themeKey);

        // ألوان الشركة تُغلّب على ألوان الثيم حين تُضبط، ولون النصّ فوق
        // اللون الأساسي يُحسب لا يُختار — شرط المالك 14.
        $primary = $row->primary_color ?? $theme['primary'];
        $secondary = $row->secondary_color ?? $theme['secondary'];
        $background = $row->background_color ?? $theme['background'];

        return [
            'company_account_id' => $companyAccountId,
            'company_name_ar'    => $row->company_name_ar ?? $fallbackName,
            'company_name_en'    => $row->company_name_en ?? null,
            'logo_url'           => $this->logoUrl($row->logo_path ?? null),
            'theme_key'          => $themeKey,
            'branding_version'   => (int) ($row->branding_version ?? 0),
            'updated_at'         => $row->updated_at ?? null,
            'colors'             => [
                'primary'    => $primary,
                'secondary'  => $secondary,
                'background' => $background,
                'surface'    => $theme['surface'],
                'on_primary' => BrandingThemes::readableTextOn($primary),
                'text'       => $theme['text'],
                'text_muted' => $theme['text_muted'],
                'border'     => $theme['border'],
                'status'     => BrandingThemes::STATUS_COLORS,
            ],
        ];
    }

    /**
     * مسار الشعار **نسبةً إلى `API_BASE`** لا رابطاً مطلقاً.
     *
     * رابطٌ مطلق يُخزَّن أو يُبنى من `APP_URL` ينكسر كلما تغيّر عنوان الخادم
     * أو انتقل التطبيق من المحاكي إلى الإنتاج. النسبيّ يصحّ في الحالتين.
     */
    private function logoUrl(?string $path): ?string
    {
        if ($path === null || $path === '') {
            return null;
        }
        return 'company/branding/logo/' . basename($path);
    }

    /** الملف نفسه — يقرؤه مسار الخدمة. لا يقبل إلا اسماً بلا مسار. */
    public function logoStream(string $name): ?array
    {
        // اسمٌ فيه فاصل مسار = محاولة خروج من المجلد. يُرفض قبل أي قراءة.
        if ($name !== basename($name) || str_contains($name, '..')) {
            return null;
        }

        $path = self::DIR . '/' . $name;
        $disk = Storage::disk(self::DISK);
        if (!$disk->exists($path)) {
            return null;
        }

        return [
            'stream' => $disk->readStream($path),
            'mime'   => $disk->mimeType($path) ?: 'application/octet-stream',
            'size'   => $disk->size($path),
        ];
    }

    /**
     * حفظ الهوية. يُعيد ما حُفظ.
     *
     * @param array<string, mixed> $input
     */
    public function save(int $companyAccountId, int $userId, array $input, array $trace): array
    {
        return DB::transaction(function () use ($companyAccountId, $userId, $input, $trace) {
            $before = DB::table('tenant_branding')
                ->where('company_account_id', $companyAccountId)
                ->first();

            $fields = [];
            foreach (['company_name_ar', 'company_name_en', 'theme_key',
                      'primary_color', 'secondary_color', 'background_color'] as $f) {
                if (array_key_exists($f, $input)) {
                    $fields[$f] = $input[$f];
                }
            }

            if ($fields === []) {
                return $this->forCompany($companyAccountId);
            }

            $fields['updated_at'] = now();

            if ($before === null) {
                $fields['company_account_id'] = $companyAccountId;
                $fields['branding_version'] = 1;
                DB::table('tenant_branding')->insert($fields);
            } else {
                // العدّاد يزيد مع كل حفظ فعليّ — به يعرف التطبيق أن عليه
                // إعادة الجلب بدل مقارنة كل حقل.
                $fields['branding_version'] = ((int) $before->branding_version) + 1;
                DB::table('tenant_branding')
                    ->where('company_account_id', $companyAccountId)
                    ->update($fields);
            }

            $this->audit($companyAccountId, $userId, $before, $fields, $trace);

            return $this->forCompany($companyAccountId);
        });
    }

    /** صفٌّ لكل حقل تغيّر — فيُقرأ «من غيّر الشعار؟» بسؤال واحد. */
    private function audit(int $companyAccountId, int $userId, ?object $before, array $after, array $trace): void
    {
        $rows = [];
        foreach ($after as $field => $newValue) {
            if (in_array($field, ['updated_at', 'company_account_id', 'branding_version'], true)) {
                continue;
            }
            $oldValue = $before->{$field} ?? null;
            if ((string) $oldValue === (string) $newValue) {
                continue;
            }
            $rows[] = [
                'company_account_id' => $companyAccountId,
                'changed_by'         => $userId,
                'field_name'         => $field,
                'old_value'          => $oldValue === null ? null : (string) $oldValue,
                'new_value'          => $newValue === null ? null : (string) $newValue,
                'ip_address'         => $trace['ip'] ?? null,
                'device_id'          => $trace['device'] ?? null,
            ];
        }

        if ($rows !== []) {
            DB::table('tenant_branding_audit')->insert($rows);
        }
    }

    /**
     * رفع الشعار.
     *
     * التحقّق من النوع بقراءة أبعاد الصورة لا بامتدادها ولا بترويسة العميل:
     * كلاهما يُزوَّر، وملفٌّ يدّعي أنه PNG قد يكون شيئاً آخر. `getimagesize`
     * تفشل على ما ليس صورة.
     */
    public function saveLogo(int $companyAccountId, int $userId, UploadedFile $file, array $trace): array
    {
        if ($file->getSize() > self::MAX_BYTES) {
            throw new \InvalidArgumentException('حجم الشعار أكبر من 2 ميغابايت.');
        }

        $info = @getimagesize($file->getRealPath());
        if ($info === false) {
            throw new \InvalidArgumentException('الملف ليس صورة صالحة.');
        }

        [$w, $h, $type] = [$info[0], $info[1], $info['mime'] ?? ''];

        if (!in_array($type, self::MIMES, true)) {
            throw new \InvalidArgumentException('الصيغ المقبولة: PNG أو JPG أو WEBP.');
        }
        if ($w < self::MIN_SIDE || $h < self::MIN_SIDE) {
            throw new \InvalidArgumentException('الشعار صغير جداً — الحدّ الأدنى 64 بكسل.');
        }
        if ($w > self::MAX_SIDE || $h > self::MAX_SIDE) {
            throw new \InvalidArgumentException('الشعار كبير جداً — الحدّ الأقصى 2048 بكسل.');
        }

        $ext = match ($type) {
            'image/png'  => 'png',
            'image/jpeg' => 'jpg',
            default      => 'webp',
        };

        // اسمٌ عشوائي لا اسم الملف المرفوع: اسم المستخدم قد يحمل مساراً،
        // ورقم الشركة وحده يجعل الرابط قابلاً للتخمين عبر الشركات.
        $name = $companyAccountId . '_' . Str::random(24) . '.' . $ext;
        $path = self::DIR . '/' . $name;

        Storage::disk(self::DISK)->putFileAs(self::DIR, $file, $name);

        return DB::transaction(function () use ($companyAccountId, $userId, $path, $trace) {
            $before = DB::table('tenant_branding')
                ->where('company_account_id', $companyAccountId)
                ->first();

            if ($before === null) {
                DB::table('tenant_branding')->insert([
                    'company_account_id' => $companyAccountId,
                    'logo_path'          => $path,
                    'branding_version'   => 1,
                    'updated_at'         => now(),
                ]);
            } else {
                DB::table('tenant_branding')
                    ->where('company_account_id', $companyAccountId)
                    ->update([
                        'logo_path'        => $path,
                        'branding_version' => ((int) $before->branding_version) + 1,
                        'updated_at'       => now(),
                    ]);

                // الشعار القديم يُحذف بعد نجاح الحفظ لا قبله: الحذف أولاً
                // يترك الشركة بلا شعار إن فشل الرفع.
                if (!empty($before->logo_path)) {
                    Storage::disk(self::DISK)->delete($before->logo_path);
                }
            }

            $this->audit($companyAccountId, $userId, $before, ['logo_path' => $path], $trace);

            return $this->forCompany($companyAccountId);
        });
    }

    /**
     * استعادة الهوية الافتراضية — الثيم والألوان فقط.
     *
     * الاسم والشعار يبقيان: شرط المالك 18 يقول «ليس بالضرورة حذف اسم
     * الشركة»، ومحو شعارٍ رفعه المستخدم بضغطة «استعادة» خسارةٌ لا رجعة فيها.
     */
    public function resetTheme(int $companyAccountId, int $userId, array $trace): array
    {
        return $this->save($companyAccountId, $userId, [
            'theme_key'        => BrandingThemes::DEFAULT_KEY,
            'primary_color'    => null,
            'secondary_color'  => null,
            'background_color' => null,
        ], $trace);
    }
}
