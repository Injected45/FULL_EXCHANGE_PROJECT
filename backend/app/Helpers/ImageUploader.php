<?php

namespace App\Helpers;

use Illuminate\Support\Facades\Storage;
use Illuminate\Http\UploadedFile;

class ImageUploader
{
    /**
     * رفع صورة واحدة مع إمكانية استخدامها لحقلين
     */
    public static function uploadSingleForTwoFields(?UploadedFile $passportFile, ?UploadedFile $profileFile)
    {
        $result = [
            'pasbort_link' => null,
            'image_link' => null,
        ];

        // رفع صورة جواز السفر
        if ($passportFile) {
            $passportPath = $passportFile->store('customers/passport', 'public');
            $passportUrl = asset('storage/' . $passportPath);
            $result['pasbort_link'] = $passportUrl;

            // إذا لم يتم إرسال صورة شخصية، استخدم نفس الصورة
            if (!$profileFile) {
                $result['image_link'] = $passportUrl;
            }
        }

        // رفع الصورة الشخصية
        if ($profileFile) {
            $profilePath = $profileFile->store('customers/profile', 'public');
            $profileUrl = asset('storage/' . $profilePath);
            $result['image_link'] = $profileUrl;

            // إذا لم يتم إرسال جواز السفر، استخدم نفس الصورة
            if (!$passportFile) {
                $result['pasbort_link'] = $profileUrl;
            }
        }

        return $result;
    }

    /**
     * تعديل أو حذف صور العميل
     */
    public static function updateOrDeleteImages($customer, ?UploadedFile $passportFile, ?UploadedFile $profileFile, bool $deletePassport = false, bool $deleteProfile = false)
    {
        $result = [
            'pasbort_link' => $customer->pasbort_link,
            'image_link' => $customer->image_link,
        ];

        // حذف جواز السفر إذا مطلوب
        if ($deletePassport && $customer->pasbort_link) {
            $path = str_replace(asset('storage/'), '', $customer->pasbort_link);
            if (Storage::disk('public')->exists($path)) {
                Storage::disk('public')->delete($path);
            }
            $result['pasbort_link'] = null;
        }

        // حذف الصورة الشخصية إذا مطلوب
        if ($deleteProfile && $customer->image_link) {
            $path = str_replace(asset('storage/'), '', $customer->image_link);
            if (Storage::disk('public')->exists($path)) {
                Storage::disk('public')->delete($path);
            }
            $result['image_link'] = null;
        }

        // رفع صور جديدة إذا موجودة
        if ($passportFile || $profileFile) {
            $images = self::uploadSingleForTwoFields($passportFile, $profileFile);

            if ($images['pasbort_link']) $result['pasbort_link'] = $images['pasbort_link'];
            if ($images['image_link']) $result['image_link'] = $images['image_link'];
        }

        return $result;
    }
}
