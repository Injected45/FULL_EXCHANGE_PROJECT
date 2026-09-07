<?php

namespace App\Services\Employees;

/**
 * كتالوج صلاحيات الموظفين — مصدر الحقيقة الوحيد.
 *
 * ⚠ **Default Deny**: ما ليس له صفٌّ في `employee_permissions` مرفوض. لا
 * توجد صلاحية ضمنية لمجرّد أن الموظف سجّل دخوله (بند 29).
 *
 * ولهذا الكتالوج في الخادم لا في التطبيق: ميزةٌ تُضاف غداً تظهر هنا فتصير
 * **مرفوضة للجميع تلقائياً** (بند 30) — بلا هجرة قاعدة بيانات، وبلا تحديث
 * للتطبيق، وبلا احتمال أن ينالها موظف بمجرّد تحديث النسخة على هاتفه.
 *
 * والتطبيق لا يعرّف صلاحية ولا يخمّنها: يقرأ ما مُنح، ويخفي ما لم يُمنح.
 * والإخفاء تجميل — الرفض الحقيقي في الخادم عند كل نداء.
 */
class EmployeePermissions
{
    /**
     * كل صلاحية: المفتاح ⇦ [التصنيف، الاسم العربي].
     *
     * التصنيف يجمعها في شاشة المنح، ولا يمنح شيئاً: منحُ «التقارير» لا يمنح
     * تقاريرها (بند 27) — كل تقرير مفتاحه المستقلّ.
     */
    public const CATALOG = [
        // ── الحوالات ────────────────────────────────────────────────
        'VIEW_INCOMING_TRANSFERS' => ['transfers', 'عرض الحوالات الواردة'],
        'DELIVER_TRANSFER'        => ['transfers', 'تسجيل تسليم حوالة'],
        'CREATE_TRANSFER'         => ['transfers', 'إنشاء حوالة'],
        'VIEW_OWN_TRANSFERS'      => ['transfers', 'عرض حوالاته هو'],
        'VIEW_POS_TRANSFERS'      => ['transfers', 'عرض حوالات نقطة بيعه'],
        'SEARCH_TRANSFER'         => ['transfers', 'البحث برقم الحوالة'],

        // ── الخزينة والورديات ──────────────────────────────────────
        'VIEW_OWN_CASHBOX'        => ['cashbox', 'عرض خزينته'],
        'CASHBOX_ENTRY'           => ['cashbox', 'تسجيل حركة خزينة'],
        'START_SHIFT'             => ['cashbox', 'بدء وردية'],
        'CLOSE_SHIFT'             => ['cashbox', 'إقفال وردية'],

        // ── الأرصدة ────────────────────────────────────────────────
        // تُفصّل عمداً: «قسم الأرصدة» ليس صلاحية واحدة (بند 28).
        'VIEW_AGENT_TOTAL_BALANCE' => ['balances', 'عرض رصيد الوكيل الكلّي'],
        'VIEW_FINANCIAL_SUMMARY'   => ['balances', 'عرض الملخّص المالي'],

        // ── التقارير ───────────────────────────────────────────────
        // `REPORTS_VIEW` تفتح القسم فقط، ولا تُظهر تقريراً واحداً.
        'REPORTS_VIEW'                => ['reports', 'فتح قسم التقارير'],
        'REPORT_DAILY_TRANSFERS'      => ['reports', 'تقرير حوالات اليوم'],
        'REPORT_DELIVERED_TRANSFERS'  => ['reports', 'تقرير الحوالات المسلَّمة'],
        'REPORT_PENDING_TRANSFERS'    => ['reports', 'تقرير الحوالات بانتظار التسليم'],
        'REPORT_EMPLOYEE_CASHBOX'     => ['reports', 'تقرير خزينة الموظف'],
        'REPORT_POINT_OF_SALE'        => ['reports', 'تقرير نقطة البيع'],
        'REPORT_AGENT_BALANCE'        => ['reports', 'تقرير رصيد الوكيل'],
        'REPORT_AUDIT'                => ['reports', 'تقرير سجلّ النشاط'],

        // ── المستفيدون ─────────────────────────────────────────────
        'VIEW_FAVORITES'          => ['customers', 'عرض المفضّلة'],
        'MANAGE_FAVORITES'        => ['customers', 'إدارة المفضّلة'],

        // ── الدردشة ────────────────────────────────────────────────
        // صلاحيةٌ تُمنح كسائرها — لا شيء مفتوح افتراضاً (بند 4).
        // ومحادثة الموظف مع وكيله وحده: لا يرى محادثة الوكيل مع الإدارة،
        // ولا محادثات زملائه.
        'CHAT_WITH_AGENT'         => ['chat', 'مراسلة الوكيل'],
    ];

    /** التصنيفات بأسمائها — لترتيب شاشة المنح. */
    public const GROUPS = [
        'transfers' => 'الحوالات',
        'cashbox'   => 'الخزينة والورديات',
        'balances'  => 'الأرصدة',
        'reports'   => 'التقارير',
        'customers' => 'المستفيدون',
        'chat'      => 'الدردشة',
    ];

    /**
     * صلاحياتٌ لا تُمنح لموظف مهما فعل الوكيل.
     *
     * إدارة الموظفين والأجهزة والصلاحيات وهوية الشركة تبقى للحساب الرئيسي.
     * موظفٌ يستطيع منح نفسه صلاحية هو مسؤولٌ بخطوة واحدة.
     */
    public const NEVER_FOR_EMPLOYEES = [
        'MANAGE_EMPLOYEES',
        'MANAGE_PERMISSIONS',
        'MANAGE_DEVICES',
        'MANAGE_POINTS_OF_SALE',
        'MANAGE_COMPANY_BRANDING',
    ];

    /**
     * ما هو **موصولٌ فعلاً** — لا ما نيّتُنا أن نصله.
     *
     * ── لماذا هذه القائمة موجودة ─────────────────────────────────────
     *
     * ⚠ الكتالوج كُتب قبل الميزات، فصار فيه مفاتيحُ تُمنح ولا تفعل شيئاً:
     * يمنحها الوكيل، وتُحفظ، ولا يظهر عند الموظف شيء — ولا رسالةَ تقول
     * لماذا. وهذا أسوأ من غياب الصلاحية أصلاً: الوكيل يظنّ أنه أعطى
     * موظّفه قدرةً، والموظف يظنّ أن التطبيق معطوب.
     *
     * فصارت الشاشةُ تقول الحقيقة: ما ليس هنا يُعرض «لم تُفعَّل بعد».
     *
     * ⚠ وهي في الخادم لا في التطبيق: ميزةٌ تُوصَل غداً تُضاف هنا فتظهر
     * مفعّلةً بلا إصدارٍ جديد من التطبيق — وهو المبدأ نفسُه الذي يجعل
     * كتالوج الصلاحيات كلَّه في الخادم.
     *
     * والمصدرُ الوحيد للحقيقة: مساراتُ `device/employee/*` في
     * `routes/api.php`. من أضاف مساراً بمفتاحٍ جديد يُضيفه هنا معه.
     */
    public const LIVE = [
        'VIEW_INCOMING_TRANSFERS',
        'DELIVER_TRANSFER',
        'VIEW_OWN_CASHBOX',
        'CASHBOX_ENTRY',
        'START_SHIFT',
        'CLOSE_SHIFT',
        'CHAT_WITH_AGENT',
        // الدفعة الأولى من المفاتيح المُعطّلة: قراءةٌ خالصة.
        'SEARCH_TRANSFER',
        'VIEW_OWN_TRANSFERS',
        'VIEW_POS_TRANSFERS',
    ];

    /** هل للمفتاح أثرٌ فعليّ اليوم؟ */
    public static function isLive(string $key): bool
    {
        return in_array($key, self::LIVE, true);
    }
    public static function exists(string $key): bool
    {
        return array_key_exists($key, self::CATALOG);
    }

    /** هل يجوز منح هذا المفتاح لموظف أصلاً؟ */
    public static function grantable(string $key): bool
    {
        return self::exists($key)
            && !in_array($key, self::NEVER_FOR_EMPLOYEES, true);
    }

    /** الكتالوج مسطّحاً لشاشة المنح. */
    public static function catalogForAdmin(): array
    {
        $out = [];
        foreach (self::GROUPS as $groupKey => $groupName) {
            $items = [];
            foreach (self::CATALOG as $key => [$g, $label]) {
                if ($g === $groupKey) {
                    $items[] = [
                        'key'   => $key,
                        'label' => $label,
                        // ⚠ الشاشةُ تعرض هذا: مفتاحٌ غير موصولٍ يُمنح
                        // ولا يفعل شيئاً، فيجب أن يقول ذلك بنفسه.
                        'live'  => self::isLive($key),
                    ];
                }
            }
            if ($items !== []) {
                $out[] = ['group' => $groupKey, 'name' => $groupName, 'items' => $items];
            }
        }
        return $out;
    }
}
