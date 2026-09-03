<?php

namespace App\Services;

/**
 * كتالوج الثيمات — في الخادم لا في التطبيق.
 *
 * سببان: الثيم يجب أن يظهر كما هو على كل جهاز وكل نسخة من التطبيق، ولأن
 * إضافة ثيم جديد يجب ألّا تنتظر تحديثاً في المتجر.
 *
 * ولا يُسمح بألوان حرّة تُلصق في الواجهة: الشركة تختار **مفتاح ثيم**،
 * وله لوحةٌ متّسقة اختيرت بتباينٍ مقروء. تركُ كل لون للاختيار يُنتج شاشات
 * لا تُقرأ، والمالك اشترط ألّا يكسر التخصيصُ التصميم.
 */
class BrandingThemes
{
    public const DEFAULT_KEY = 'classic_green';

    /**
     * ألوان الحالات ثابتة في كل الثيمات عمداً.
     *
     * شرط المالك: الأخضر نجاح، والأحمر خطأ، والبرتقالي تحذير — مهما كانت
     * ألوان الشركة. لونُ شركةٍ أحمرُ يجعل شاشة النجاح حمراء، فيقرأ الوكيل
     * نجاحاً على أنه فشل، وهذا في تطبيق حوالات خطأ لا يُحتمل.
     */
    public const STATUS_COLORS = [
        'success' => '#12A150',
        'warning' => '#C77700',
        'error'   => '#D14343',
        'info'    => '#2C6ECB',
    ];

    /** @return array<string, array<string, string>> */
    public static function all(): array
    {
        return [
            'classic_green' => [
                'name_ar'    => 'الأخضر الكلاسيكي',
                'name_en'    => 'Classic Green',
                'primary'    => '#00875A',
                'secondary'  => '#00B17A',
                'background' => '#F1F8F5',
                'surface'    => '#FFFFFF',
                'on_primary' => '#FFFFFF',
                'text'       => '#032D21',
                'text_muted' => '#5B6F67',
                'border'     => '#DDE9E3',
            ],
            'emerald' => [
                'name_ar'    => 'الزمرّد',
                'name_en'    => 'Emerald',
                'primary'    => '#046C4E',
                'secondary'  => '#0E9F6E',
                'background' => '#F0FAF5',
                'surface'    => '#FFFFFF',
                'on_primary' => '#FFFFFF',
                'text'       => '#04241A',
                'text_muted' => '#54685F',
                'border'     => '#D6EAE0',
            ],
            'blue_corporate' => [
                'name_ar'    => 'الأزرق المؤسسي',
                'name_en'    => 'Blue Corporate',
                'primary'    => '#12508C',
                'secondary'  => '#2C6ECB',
                'background' => '#F2F6FB',
                'surface'    => '#FFFFFF',
                'on_primary' => '#FFFFFF',
                'text'       => '#0B2038',
                'text_muted' => '#5A6B7D',
                'border'     => '#DCE6F1',
            ],
            'gold_premium' => [
                'name_ar'    => 'الذهبي',
                'name_en'    => 'Gold Premium',
                'primary'    => '#8A6A12',
                'secondary'  => '#C9A227',
                'background' => '#FBF8EF',
                'surface'    => '#FFFFFF',
                'on_primary' => '#FFFFFF',
                'text'       => '#2E2408',
                'text_muted' => '#6E6650',
                'border'     => '#EDE3C9',
            ],
            'light' => [
                'name_ar'    => 'فاتح محايد',
                'name_en'    => 'Light',
                'primary'    => '#33475B',
                'secondary'  => '#5C7A99',
                'background' => '#F6F7F9',
                'surface'    => '#FFFFFF',
                'on_primary' => '#FFFFFF',
                'text'       => '#1B2733',
                'text_muted' => '#63707E',
                'border'     => '#E2E6EA',
            ],
            'dark' => [
                'name_ar'    => 'داكن',
                'name_en'    => 'Dark',
                'primary'    => '#00B17A',
                'secondary'  => '#00875A',
                'background' => '#101614',
                'surface'    => '#1A2320',
                'on_primary' => '#04241A',
                'text'       => '#E8F1EC',
                'text_muted' => '#9AAAA2',
                'border'     => '#2A3833',
            ],
        ];
    }

    public static function has(string $key): bool
    {
        return array_key_exists($key, self::all());
    }

    /** الثيم مع ألوان الحالات مضمومةً إليه — التطبيق يقرأ لوحةً واحدة. */
    public static function get(string $key): array
    {
        $all = self::all();
        $theme = $all[$key] ?? $all[self::DEFAULT_KEY];
        return $theme + ['status' => self::STATUS_COLORS] + ['key' => $key];
    }

    /**
     * لون نصٍّ مقروء فوق خلفية معيّنة.
     *
     * شرط المالك: ألّا يُنتج اختيارُ الشركة نصّاً لا يُقرأ. تُحسب الإضاءة
     * النسبية بمعادلة WCAG، والعتبة 0.5 تفصل الفاتح عن الداكن — وهي تقريبٌ
     * كافٍ هنا لأن اللون الآخر يبقى من لوحة الثيم لا حرّاً.
     */
    public static function readableTextOn(string $hex): string
    {
        $hex = ltrim($hex, '#');
        if (strlen($hex) !== 6) {
            return '#FFFFFF';
        }

        $channel = function (int $c): float {
            $s = $c / 255;
            return $s <= 0.03928 ? $s / 12.92 : pow(($s + 0.055) / 1.055, 2.4);
        };

        $l = 0.2126 * $channel((int) hexdec(substr($hex, 0, 2)))
           + 0.7152 * $channel((int) hexdec(substr($hex, 2, 2)))
           + 0.0722 * $channel((int) hexdec(substr($hex, 4, 2)));

        return $l > 0.5 ? '#1B2733' : '#FFFFFF';
    }
}
