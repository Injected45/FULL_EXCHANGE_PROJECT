<?php

namespace App\Services\Support;

/**
 * كتالوج صلاحيات مركز الدعم — مصدر الحقيقة الوحيد.
 *
 * ⚠ **Default Deny**: ما ليس له صفٌّ في `support_permissions` مرفوض. لا
 * تُمنح صلاحيةٌ لمجرّد أن الموظّف سجّل دخوله، ولا لمجرّد أن دوره كبير.
 *
 * وهو النمطُ نفسه المتّبع في {@see \App\Services\Employees\EmployeePermissions}
 * عمداً: نظامُ صلاحياتٍ ثانٍ بقواعدَ مختلفة في المشروع نفسه هو مكانان
 * للخطأ بدل مكانٍ واحد، ومن يقرأ أحدهما يظنّ أنه فهم الآخر.
 *
 * ── الدور والصلاحية شيئان ────────────────────────────────────────────────
 *
 * الدور **سقفٌ** لا منحٌ. `ROLE_CEILING` تقول ما **يجوز** أن يناله صاحب
 * الدور، و `support_permissions` تقول ما **ناله فعلاً**. فمشرفٌ بلا صفوف
 * لا يملك شيئاً، وموظّفُ دعمٍ لا يُمنح إدارةَ الحسابات ولو أخطأ المدير
 * وضغط عليها — الرفض في الطبقتين.
 *
 * وسببُ السقف أن الخطأ في شاشة المنح وارد، ونتيجتُه هنا أن يصير موظّفُ
 * دعمٍ قادراً على إنشاء حساباتٍ ومنح نفسه ما شاء. سقفٌ يجعل ذلك مستحيلاً
 * لا مستبعَداً.
 *
 * ⚠ ولا صلاحيةَ في هذا الكتالوج تمسّ المال: ليس فيه رصيدٌ ولا حوالةٌ ولا
 * قيد. موظّف الدعم يقرأ المحادثات ويردّ عليها — لا يرى ميزانيةَ وكيلٍ ولا
 * يحرّك درهماً. وهذا حدُّ المركز كلِّه، لا حدُّ هذا الملفّ.
 */
class SupportPermissions
{
    public const SUPPORT_AGENT = 'SUPPORT_AGENT';
    public const SUPERVISOR    = 'SUPERVISOR';
    public const ADMIN         = 'ADMIN';

    public const ROLES = [
        self::SUPPORT_AGENT => 'موظّف دعم',
        self::SUPERVISOR    => 'مشرف',
        self::ADMIN         => 'مدير النظام',
    ];

    /** كل صلاحية: المفتاح ⇦ [التصنيف، الاسم العربي]. */
    public const CATALOG = [
        // ── المحادثات ───────────────────────────────────────────────
        'VIEW_THREADS'      => ['threads', 'عرض المحادثات المُسنَدة إليه'],
        // مفصولةٌ عمداً: موظّفٌ يرى محادثاته شيء، وموظّفٌ يرى محادثات
        // الوكلاء كلِّهم شيءٌ آخر — والثانية قرارُ مشرفٍ لا نتيجةُ الأولى.
        'VIEW_ALL_THREADS'  => ['threads', 'عرض كل المحادثات'],
        'REPLY'             => ['threads', 'الردّ على المحادثات'],
        'SEND_ATTACHMENT'   => ['threads', 'إرسال صورة أو ملفّ'],
        'SEND_VOICE'        => ['threads', 'إرسال رسالة صوتية'],
        'EDIT_OWN_MESSAGE'  => ['threads', 'تعديل رسالته خلال المهلة'],
        'PIN_MESSAGE'       => ['threads', 'تثبيت رسالة في المحادثة'],
        'FORWARD_MESSAGE'   => ['threads', 'إعادة توجيه رسالة'],
        'SEARCH_MESSAGES'   => ['threads', 'البحث في الرسائل'],

        // ── الإسناد والحالة ────────────────────────────────────────
        'ASSIGN_SELF'       => ['workflow', 'استلام محادثة لنفسه'],
        'ASSIGN_OTHERS'     => ['workflow', 'إسناد محادثة إلى غيره'],
        'CHANGE_STATUS'     => ['workflow', 'تغيير حالة المحادثة'],
        'CLOSE_THREAD'      => ['workflow', 'إغلاق محادثة'],
        'REOPEN_THREAD'     => ['workflow', 'إعادة فتح محادثة مغلقة'],

        // ── الإدارة ────────────────────────────────────────────────
        'MANAGE_STAFF'       => ['admin', 'إدارة حسابات الدعم'],
        'MANAGE_PERMISSIONS' => ['admin', 'منح الصلاحيات وسحبها'],
        'VIEW_AUDIT'         => ['admin', 'عرض سجلّ النشاط'],
        'VIEW_STATS'         => ['admin', 'عرض الإحصاءات'],
    ];

    public const GROUPS = [
        'threads'  => 'المحادثات',
        'workflow' => 'الإسناد والحالة',
        'admin'    => 'الإدارة',
    ];

    /**
     * سقفُ كلّ دور: ما يجوز أن يُمنح لصاحبه أصلاً.
     *
     * موظّف الدعم لا يُسنِد لغيره ولا يرى محادثات الجميع ولا يقترب من
     * الإدارة. والمشرف يُسنِد ويرى الكلّ ويقرأ السجلّ — ولا يُنشئ حساباً
     * ولا يمنح صلاحية: من يمنح الصلاحيات يمنح نفسه، وتلك رتبةٌ واحدة.
     */
    public const ROLE_CEILING = [
        self::SUPPORT_AGENT => [
            'VIEW_THREADS',
            // ⚠ أُضيفت بأمر المالك (6 سبتمبر 2026) بعد أن ظهر العيب عملياً.
            //
            // كان موظّف الدعم يرى المُسنَدة إليه وغيرَ المُسنَدة وحدها. فأنشأ
            // المالك موظّفاً، وكانت محادثةُ الوكيل الوحيد مُسنَدةً إلى مدير
            // النظام — فلم يرَ الموظّفُ **وكيلاً واحداً**، ولم يستطع أن يردّ
            // على أحد. أي أن الحساب أُنشئ ولا يعمل، ولا شيء في الشاشة يقول
            // لماذا.
            //
            // والغرض من الموظّف متابعةُ الوكلاء، فرؤيتُهم شرطُ وجوده.
            // والتزاحمُ الذي كان يمنعه الحدُّ القديم يُعالجه الإسناد نفسُه:
            // اسمُ المسؤول ظاهرٌ على كل محادثة، والحالةُ تقول أين وصلت.
            'VIEW_ALL_THREADS',
            'REPLY', 'SEND_ATTACHMENT', 'SEND_VOICE',
            'EDIT_OWN_MESSAGE', 'PIN_MESSAGE', 'FORWARD_MESSAGE',
            'SEARCH_MESSAGES', 'ASSIGN_SELF', 'CHANGE_STATUS',
        ],
        self::SUPERVISOR => [
            'VIEW_THREADS', 'VIEW_ALL_THREADS', 'REPLY', 'SEND_ATTACHMENT',
            'SEND_VOICE', 'EDIT_OWN_MESSAGE', 'PIN_MESSAGE',
            'FORWARD_MESSAGE', 'SEARCH_MESSAGES', 'ASSIGN_SELF',
            'ASSIGN_OTHERS', 'CHANGE_STATUS', 'CLOSE_THREAD',
            'REOPEN_THREAD', 'VIEW_AUDIT', 'VIEW_STATS',
        ],
        // المدير سقفُه الكتالوج كلّه — ويبقى محتاجاً إلى الصفوف.
        self::ADMIN => null,
    ];

    /**
     * ما يُمنح تلقائياً عند إنشاء الحساب — بدايةٌ معقولة لا سقف.
     *
     * وهي صفوفٌ تُكتب فعلاً في `support_permissions`، لا استثناءٌ في
     * الفحص: لو كانت استثناءً لصار في النظام مصدرانِ للحقيقة، ولَما
     * استطاع مديرٌ سحبَ ما «مُنح ضمناً».
     */
    public const ROLE_DEFAULTS = [
        self::SUPPORT_AGENT => [
            // `VIEW_ALL_THREADS` افتراضيّةٌ لا ممنوحةً باليد: موظّفٌ يُنشأ
            // ولا يرى وكيلاً واحداً هو حسابٌ لا يعمل، ومن أنشأه لا يعلم أن
            // عليه منحَ شيءٍ بعد الإنشاء.
            'VIEW_THREADS', 'VIEW_ALL_THREADS',
            'REPLY', 'SEND_ATTACHMENT', 'SEND_VOICE',
            'EDIT_OWN_MESSAGE', 'SEARCH_MESSAGES', 'ASSIGN_SELF',
            'CHANGE_STATUS',
        ],
        self::SUPERVISOR => [
            'VIEW_THREADS', 'VIEW_ALL_THREADS', 'REPLY', 'SEND_ATTACHMENT',
            'SEND_VOICE', 'EDIT_OWN_MESSAGE', 'PIN_MESSAGE',
            'FORWARD_MESSAGE', 'SEARCH_MESSAGES', 'ASSIGN_SELF',
            'ASSIGN_OTHERS', 'CHANGE_STATUS', 'CLOSE_THREAD',
            'REOPEN_THREAD', 'VIEW_STATS',
        ],
        self::ADMIN => [
            'VIEW_THREADS', 'VIEW_ALL_THREADS', 'REPLY', 'SEND_ATTACHMENT',
            'SEND_VOICE', 'EDIT_OWN_MESSAGE', 'PIN_MESSAGE',
            'FORWARD_MESSAGE', 'SEARCH_MESSAGES', 'ASSIGN_SELF',
            'ASSIGN_OTHERS', 'CHANGE_STATUS', 'CLOSE_THREAD',
            'REOPEN_THREAD', 'MANAGE_STAFF', 'MANAGE_PERMISSIONS',
            'VIEW_AUDIT', 'VIEW_STATS',
        ],
    ];

    public static function exists(string $key): bool
    {
        return array_key_exists($key, self::CATALOG);
    }

    public static function roleExists(string $role): bool
    {
        return array_key_exists($role, self::ROLES);
    }

    /** هل يجوز لصاحب هذا الدور أن يحمل هذا المفتاح أصلاً؟ */
    public static function allowedForRole(string $role, string $key): bool
    {
        if (!self::exists($key) || !self::roleExists($role)) {
            return false;
        }

        $ceiling = self::ROLE_CEILING[$role];

        return $ceiling === null || in_array($key, $ceiling, true);
    }

    /** الكتالوج مسطّحاً لشاشة المنح، مع بيان ما يمنعه سقفُ الدور. */
    public static function catalogFor(string $role): array
    {
        $out = [];
        foreach (self::GROUPS as $groupKey => $groupName) {
            $items = [];
            foreach (self::CATALOG as $key => [$g, $label]) {
                if ($g !== $groupKey) {
                    continue;
                }
                $items[] = [
                    'key'     => $key,
                    'label'   => $label,
                    // تُعرض رماديةً لا تُخفى: الإخفاء يجعل المدير يظنّ أن
                    // الصلاحية غير موجودة، والرمادي يقول إنها ممنوعة
                    // *لهذا الدور* — وهما إجابتان مختلفتان.
                    'allowed' => self::allowedForRole($role, $key),
                ];
            }
            if ($items !== []) {
                $out[] = [
                    'group' => $groupKey,
                    'name'  => $groupName,
                    'items' => $items,
                ];
            }
        }

        return $out;
    }
}
