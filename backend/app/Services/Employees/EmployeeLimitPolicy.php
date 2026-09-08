<?php

namespace App\Services\Employees;

use Illuminate\Support\Facades\DB;

/**
 * محرّكُ سياسة الموظف — يقرّر: يمضي، أم يُصعَّد إلى الوكيل؟
 *
 * ── ما لا يفعله هذا الصنف ────────────────────────────────────────────────
 *
 * ⚠ **لا يكتب في الدفتر، ولا يقرأ رصيداً، ولا يحسب عمولة، ولا يمنع حوالة.**
 *
 * هو طبقةُ تفويضٍ **قبل** المسار الماليّ (البند 51): يقول «هذا الطلب يمرّ»
 * أو «هذا الطلب يحتاج موافقة»، ثمّ يتنحّى. والحوالةُ حين تُنفَّذ تُنفَّذ
 * بمسار الوكيل نفسِه حرفاً بحرف، بكلّ فحوصه — الرصيد، وقاعدة الدقيقة،
 * والحدود المركزية، والعمولة. فموافقةُ الوكيل تعالج **سببَ التصعيد وحدَه**
 * ولا تتجاوز شيئاً من ذلك (البند 46).
 *
 * ── ولماذا «صعِّد» لا «ارفض» ─────────────────────────────────────────────
 *
 * الرفضُ يُضيّع عملَ الموظف ويُجبره على إعادة الإدخال، فيتعلّم أن يقسّم
 * المبلغ من تلقاء نفسِه — وهو بالضبط ما جاءت السياسةُ لتمنعه. أمّا التصعيدُ
 * فيحفظ الطلبَ كما هو ويضع القرارَ عند من يملكه.
 */
class EmployeeLimitPolicy
{
    /** لا سقفَ لموظفٍ بلا صفّ سياسة — وهو سلوك اليوم بالضبط. */
    public const DEFAULT_RECIPIENT_MINUTES = 60;
    public const DEFAULT_APPROVAL_TTL_HOURS = 24;

    /* أسبابُ التصعيد — مفاتيحُ ثابتة، ونصُّها العربيّ في `reasonLabel`. */
    public const R_PER_TRANSFER = 'PER_TRANSFER';
    public const R_CUMULATIVE   = 'CUMULATIVE';
    public const R_RECIPIENT    = 'RECIPIENT_REPEAT';

    /**
     * سياسةُ الموظف كما هي في القاعدة **الآن**.
     *
     * ⚠ تُقرأ عند كلّ طلب، ولا تُخزَّن في التطبيق (البند 44): سقفٌ خفّضه
     * الوكيلُ قبل ثانية يجب أن يسري على الطلب الحاليّ، ونسخةٌ في الهاتف
     * تعني موظفاً يعمل بسقف الأمس.
     */
    public function forEmployee(int $employeeId): object
    {
        $row = DB::table('employee_transfer_policies')
            ->where('employee_id', $employeeId)
            ->first();

        return (object) [
            'per_transfer_limit' => $row->per_transfer_limit ?? null,
            'cumulative_limit'   => $row->cumulative_limit ?? null,
            'cumulative_hours'   => $row->cumulative_hours ?? null,
            /*
             * ⚠ بلا صفِّ سياسة = **بلا مراقبةٍ إطلاقاً**، لا مراقبةً
             * بالقيمة الافتراضية.
             *
             * كشفه اختبارٌ قائم: موظفٌ لم يضع له وكيلُه سقفاً صُعِّدت
             * حوالتُه الثانية إلى المستفيد نفسِه — فصار كلُّ موظفٍ يعمل
             * اليوم يُصعَّد بقاعدةٍ لم يطلبها أحد، وهو الانقطاعُ الذي
             * قامت هذه السياسةُ كلُّها على تجنّبه.
             *
             * فالستّون دقيقة قيمةُ **البداية حين يُنشئ الوكيل سياسة**،
             * لا قيمةٌ تسري على من لا سياسةَ له.
             */
            'recipient_minutes'  => $row === null
                ? 0
                : (int) ($row->recipient_minutes ?? self::DEFAULT_RECIPIENT_MINUTES),
            'approval_ttl_hours' => (int) ($row->approval_ttl_hours ?? self::DEFAULT_APPROVAL_TTL_HOURS),
            'exists'             => $row !== null,
        ];
    }

    /**
     * يُقيّم طلباً واحداً ويُعيد أسبابَ التصعيد — فارغةً إذا مضى.
     *
     * @return array{reasons:string[], policy:object, consumed:float}
     */
    public function evaluate(
        object $employee,
        float $amount,
        ?string $recipientPhone,
        ?string $recipientName,
    ): array {
        $policy  = $this->forEmployee((int) $employee->id);
        $reasons = [];

        /* ── 1) سقفُ الحوالة الواحدة ─────────────────────────────────── */
        if ($policy->per_transfer_limit !== null
            && $amount > (float) $policy->per_transfer_limit) {
            $reasons[] = self::R_PER_TRANSFER;
        }

        /* ── 2) السقفُ التراكميّ ─────────────────────────────────────── */
        $consumed = 0.0;

        if ($policy->cumulative_limit !== null && $policy->cumulative_hours) {
            $consumed = $this->consumed((int) $employee->id, (int) $policy->cumulative_hours);

            if (($consumed + $amount) > (float) $policy->cumulative_limit) {
                $reasons[] = self::R_CUMULATIVE;
            }
        }

        /* ── 3) تكرارُ المستفيد ──────────────────────────────────────── */
        if ($this->recipientRepeated(
            (int) $employee->id,
            $recipientPhone,
            $recipientName,
            (int) $policy->recipient_minutes,
        )) {
            $reasons[] = self::R_RECIPIENT;
        }

        return ['reasons' => $reasons, 'policy' => $policy, 'consumed' => $consumed];
    }

    /**
     * ما استهلكه الموظف من سقفه التراكميّ في النافذة.
     *
     * ── ماذا يُحسب ولماذا ────────────────────────────────────────────────
     *
     * ⚠ **الحوالاتُ التي وقعت فعلاً + الطلباتُ التي ما زالت معلّقة.**
     *
     * الأولى من `transfer_attributions` (فِعلٌ تمّ)، والثانية من
     * `employee_approval_requests` بحالة PENDING — والبند 18 يوجبها: موظفٌ
     * يترك عشرة طلباتٍ معلّقة ثمّ تُعتمد تباعاً يكون قد تجاوز سقفَه وكلُّ
     * طلبٍ منها كان «تحت السقف» لحظةَ إنشائه.
     *
     * ⚠ ولا يُحسب المرفوضُ ولا المنتهي ولا الملغى ولا الفاشل: لم يخرج منها
     * مالٌ (البند 21).
     *
     * ⚠ ولا ازدواجَ في العدّ: الطلبُ المعتمَد يصير حوالةً في
     * `transfer_attributions`، وحالتُه تغادر PENDING في اللحظة نفسِها —
     * فيُعدّ من جهةٍ واحدة دائماً.
     *
     * ⚠ وهذا **عدّادُ تفويضٍ لا قيدٌ ماليّ**: لا يُقرأ من الدفتر ولا يُكتب
     * فيه، ولا يغيّر رصيداً. مجموعُه لا يظهر في أي حساب.
     */
    public function consumed(int $employeeId, int $hours): float
    {
        $since = now()->subHours(max(1, $hours));

        $done = (float) DB::table('transfer_attributions')
            ->where('action', 'CREATED')
            ->where('employee_id', $employeeId)
            ->where('occurred_at', '>=', $since)
            ->sum('amount');

        $pending = (float) DB::table('employee_approval_requests')
            ->where('employee_id', $employeeId)
            ->where('status', 'PENDING')
            ->where('created_at', '>=', $since)
            ->sum('amount');

        return $done + $pending;
    }

    /**
     * هل حوّل هذا الموظف إلى هذا المستفيد داخل النافذة؟
     *
     * ── هويّةُ المستفيد ──────────────────────────────────────────────────
     *
     * ⚠ **رقمُ الهاتف مؤشّرٌ قويٌّ مستقلّ** (البند 15): تطابقُه وحده يكفي
     * للتصعيد. فهو معرّفٌ لا يتشابه بالمصادفة، والتحايلُ بتغييره يعني
     * تحويلاً إلى رقمٍ آخر — أي إلى شخصٍ آخر عملياً.
     *
     * ⚠ **والاسمُ وحده لا يكفي أبداً** (البند 16 و14). «محمد علي» اسمٌ
     * يحمله آلاف، ومنعُ حوالةٍ لأن اسمَ مستفيدها يشبه اسمَ مستفيدٍ آخر هو
     * إيقافُ عملٍ مشروع بلا سبب. فالاسمُ لا يُصعِّد إلّا مقروناً بغياب
     * الرقم — أي حين لا يوجد مؤشّرٌ أقوى منه أصلاً.
     */
    public function recipientRepeated(
        int $employeeId,
        ?string $phone,
        ?string $name,
        int $minutes,
    ): bool {
        if ($minutes <= 0) {
            return false;
        }

        $since = now()->subMinutes($minutes);

        /* ── الرقم: مؤشّرٌ قويٌّ مستقلّ ───────────────────────────────── */
        if ($phone !== null && $phone !== '') {
            $inLedger = DB::table('transfer_attributions')
                ->where('action', 'CREATED')
                ->where('employee_id', $employeeId)
                ->where('recipient_phone', $phone)
                ->where('occurred_at', '>=', $since)
                ->exists();

            if ($inLedger) {
                return true;
            }

            /* ⚠ والطلباتُ المعلّقة كذلك — البند 18. */
            $inPending = DB::table('employee_approval_requests')
                ->where('employee_id', $employeeId)
                ->where('recipient_phone', $phone)
                ->where('status', 'PENDING')
                ->where('created_at', '>=', $since)
                ->exists();

            if ($inPending) {
                return true;
            }

            /*
             * ⚠ ورقمٌ موجود يُنهي الفحص هنا.
             *
             * فلو تابعنا إلى الاسم، لَصُعِّدت حوالةٌ إلى «أحمد» برقمٍ جديد
             * لمجرّد أن الموظف حوّل قبل نصف ساعة إلى «أحمد» آخر — وهما
             * شخصان مختلفان بدليل اختلاف رقميهما.
             */
            return false;
        }

        /* ── الاسمُ وحده: فقط حين لا رقمَ أصلاً ──────────────────────── */
        $norm = self::normalizeName($name);

        if ($norm === '' || mb_strlen($norm) < 5) {
            /*
             * اسمٌ قصير بعد التطبيع («علي») يطابق كثيرين. والتصعيدُ عليه
             * يوقف عملاً مشروعاً كل يوم، فيتعلّم الوكيل تجاهلَ الطلبات.
             */
            return false;
        }

        return DB::table('transfer_attributions')
            ->where('action', 'CREATED')
            ->where('employee_id', $employeeId)
            ->whereNull('recipient_phone')
            ->where('recipient_name_norm', $norm)
            ->where('occurred_at', '>=', $since)
            ->exists();
    }

    /**
     * تطبيعُ اسمٍ عربيّ للمقارنة.
     *
     * ⚠ **يُوحِّد الشكلَ ولا يُقارب المعنى.** الهمزاتُ والتاءُ المربوطة
     * والتشكيلُ والمسافاتُ المكرّرة فروقٌ في الكتابة لا في الشخص، فتوحيدُها
     * صحيح. أمّا التقاربُ غير المنضبط (Fuzzy) فممنوع أن يكون سبباً منفرداً
     * لإيقاف عملية مالية (البند 14) — لأنه يجمع أشخاصاً مختلفين.
     *
     * فلا حذفَ لحروفٍ ولا مسافةَ تُزال بين الكلمات: «محمد علي» و«محمدعلي»
     * يبقيان مختلفين، وهو الصواب — الأوّل اسمان والثاني قد يكون اسماً آخر.
     */
    public static function normalizeName(?string $name): string
    {
        if ($name === null) {
            return '';
        }

        $s = trim($name);
        if ($s === '') {
            return '';
        }

        // التشكيل والتطويل — علاماتُ نطقٍ لا تغيّر الاسم.
        $s = preg_replace('/[\x{064B}-\x{0652}\x{0640}]/u', '', $s);

        // الهمزات: أ إ آ ٱ ⇦ ا
        $s = preg_replace('/[\x{0623}\x{0625}\x{0622}\x{0671}]/u', 'ا', $s);
        // ى ⇦ ي   ،   ة ⇦ ه   ،   ؤ ⇦ و   ،   ئ ⇦ ي
        $s = str_replace(
            ["\u{0649}", "\u{0629}", "\u{0624}", "\u{0626}"],
            ["\u{064A}", "\u{0647}", "\u{0648}", "\u{064A}"],
            $s,
        );

        // مسافاتٌ مكرّرة ⇦ واحدة.
        $s = preg_replace('/\s+/u', ' ', $s);

        return mb_strtolower(trim($s));
    }

    /** نصُّ سبب التصعيد كما يراه الوكيل والموظف. */
    public static function reasonLabel(string $key): string
    {
        return match ($key) {
            self::R_PER_TRANSFER => 'تجاوز سقف الحوالة الواحدة',
            self::R_CUMULATIVE   => 'تجاوز السقف التراكمي للموظف',
            self::R_RECIPIENT    => 'تحويل متكرر لنفس المستفيد خلال الفترة المحددة',
            default              => $key,
        };
    }

    /** أسبابٌ مجموعةٌ نصّاً — طلبٌ واحد وإن تعدّدت (البند 23). */
    public static function reasonLabels(string $reasons): array
    {
        return array_map(
            static fn ($k) => self::reasonLabel($k),
            array_filter(explode(',', $reasons)),
        );
    }
}
