<?php

namespace App\Services\Employees;

use Illuminate\Support\Facades\DB;

/**
 * سجلّ الأجهزة الدائم — الذي ينفّذ البندين 19 و20.
 *
 * ⚠ **لا يُحذف من `device_registry` صفٌّ أبداً.** جهازٌ استُعمل مرّة كجهاز
 * موظف يُمنع من الدخول كمسؤول إلى الأبد، ولا يرفع المنعَ خروجٌ ولا إلغاء
 * جهاز ولا تعطيل موظف ولا حذف التطبيق وإعادة تثبيته.
 *
 * ولهذا هذا الجدول منفصل عن `employee_devices`: ذاك ربطٌ تشغيلي يُلغى،
 * وهذا حكمٌ تاريخي يبقى. لو كانا واحداً لكان «إلغاء الجهاز» يمحو الحظر.
 *
 * ── حدود ما نستطيع ضمانه، بصراحة ──────────────────────────────────────
 * المعرّف يأتي من عتاد الجهاز: `ANDROID_ID` على أندرويد و
 * `identifierForVendor` على iOS، ويصل مُجزّأً هنا.
 *
 *   • حذف التطبيق وإعادة تثبيته: **الحظر يصمد** على أندرويد — `ANDROID_ID`
 *     لا يتغيّر بإعادة التثبيت.
 *   • مسح بيانات التطبيق: **يصمد** للسبب نفسه.
 *   • ضبط المصنع: **لا يصمد**، ولا يستطيع أي تطبيق ادّعاء غير ذلك — النظام
 *     يُولّد `ANDROID_ID` جديداً ولا يتيح للتطبيقات معرّفاً دائماً بعده.
 *     المستند نفسه يطلب عدم ادّعاء ضمانٍ مطلق هنا (بند 21)، وهذا هو الحدّ.
 *   • على iOS يتغيّر `identifierForVendor` بحذف كل تطبيقات المطوّر، وهو
 *     أضعف من أندرويد بحكم النظام لا بحكم التنفيذ.
 *
 * وتقوية ذلك تحتاج Play Integrity / DeviceCheck، وكلاهما يضيف حزمة وبنية
 * خادم — ولم يُضف الآن حفاظاً على حجم التطبيق. الواجهة هنا مهيّأة لهما:
 * يكفي أن يصل `attestation_token` ويُتحقّق منه قبل [remember].
 */
class DeviceRegistryService
{
    public const EMPLOYEE_DEVICE = 'EMPLOYEE_DEVICE';
    public const AGENT_DEVICE    = 'AGENT_DEVICE';

    /**
     * تجزئة معرّف الجهاز.
     *
     * يُخزَّن مُجزّأً لا خاماً: نسخةٌ احتياطية مسروقة من القاعدة لا تُعطي
     * سارقها معرّفات أجهزة الوكلاء. والمطابقة تبقى ممكنة لأن التجزئة ثابتة.
     */
    public static function hash(?string $deviceId): ?string
    {
        $id = trim((string) $deviceId);
        return $id === '' ? null : hash('sha256', $id);
    }

    /**
     * تسجيل الجهاز بتصنيفٍ دائم. يُستدعى **بعد نجاح التفعيل** لا قبله:
     * محاولةٌ فاشلة يجب ألّا تَسِم جهاز شخصٍ بريء إلى الأبد.
     */
    public function remember(
        string $deviceHash,
        string $classification,
        ?int $agentId = null,
        ?int $employeeId = null,
        ?string $platform = null,
        ?string $model = null
    ): void {
        $existing = DB::table('device_registry')
            ->where('device_hash', $deviceHash)
            ->where('classification', $classification)
            ->first();

        if ($existing) {
            DB::table('device_registry')
                ->where('id', $existing->id)
                ->update(['last_seen_at' => now()]);
            return;
        }

        DB::table('device_registry')->insert([
            'device_hash'       => $deviceHash,
            'classification'    => $classification,
            'first_agent_id'    => $agentId,
            'first_employee_id' => $employeeId,
            'platform'          => $platform,
            'model'             => $model,
            'first_seen_at'     => now(),
            'last_seen_at'      => now(),
        ]);
    }

    /**
     * هل هذا الجهاز مُصنَّف كجهاز موظف؟
     *
     * تُستدعى في **مسار دخول الوكيل** قبل إرسال أي رمز تحقّق: بند 20 يمنع
     * الدخول كمسؤول من جهاز موظف ولو كانت البيانات كلّها صحيحة.
     */
    public function isEmployeeDevice(?string $deviceHash): bool
    {
        if ($deviceHash === null) {
            return false;
        }
        return DB::table('device_registry')
            ->where('device_hash', $deviceHash)
            ->where('classification', self::EMPLOYEE_DEVICE)
            ->exists();
    }
}
