<?php

namespace App\Services\Employees;

use Illuminate\Support\Facades\DB;

/**
 * كشفُ حركة خزينة الموظف — للجرد والرقابة.
 *
 * ══════════════════════════════════════════════════════════════════════════
 *  ⚠ قراءةٌ خالصة. لا كتابةَ صفٍّ واحد في هذا الملف كلِّه.
 * ══════════════════════════════════════════════════════════════════════════
 *
 * الكشفُ يُحسب من الحركات الموجودة أصلاً ومن افتتاحيّ الورديات. ولا رصيدَ
 * يُخزَّن: رصيدٌ مخزَّنٌ يفارق حركاته عند أول انقطاع، ولا يعرف أحدٌ بعدها
 * أيَّهما يصدّق.
 *
 * ── المعادلة ──────────────────────────────────────────────────────────────
 *
 *     رصيدٌ سابق + الداخل − الخارج = الرصيد الحالي
 *
 * ── ⚠ ولماذا الرصيدُ يُصفَّر عند كل وردية ولا يتراكم ──────────────────────
 *
 * هذا أهمُّ قرارٍ في الملف، ومخالفتُه تُنتج رقماً كاذباً.
 *
 * افتتاحيُّ الوردية **عمودٌ على الوردية لا حركةٌ في الخزينة** — وهو صحيح:
 * `expectedCash` تقرأ الافتتاحيّ من الوردية وتجمع عليه حركاتها. فلو جمعنا
 * هنا افتتاحيّاتِ الورديات كلِّها مع الحركات كلِّها لَخرج رقمٌ لا يعني شيئاً.
 *
 * والسببُ الأعمق أن **الوردية دورةُ تصفية**: تُقفل بعدّ النقد فعلاً وتسليمه،
 * ثم تُفتح التالية بافتتاحيٍّ يُعلنه الموظف — وليس بما انتهت إليه سابقتُها.
 * فاستمرارُ الرصيد بينهما يزعم استمراريةً لا وجود لها.
 *
 * فالكشفُ **أقسامٌ بعدد الورديات**: كلُّ قسمٍ يبدأ بصفٍّ افتتاحيّ، وينتهي —
 * إن كانت الوردية مقفلة — بصفٍّ يقول المتوقَّع والمعدود والفرق. ورصيدُ آخر
 * حركةٍ في وردية مفتوحة **يساوي `expectedCash` لها حرفاً بحرف**، وهو ما
 * يظهر تحت اسم الموظف في الواجهة الرئيسية. رقمٌ واحد لا رقمان.
 *
 * ── ⚠ والصفوفُ الافتتاحية والختامية مُصطنَعةٌ للعرض لا تُكتب ───────────────
 *
 * كتابةُ الافتتاحيّ حركةً في الخزينة كانت ستُحسب مرّتين: مرّةً بوصفها حركة،
 * ومرّةً لأن `expectedCash` تقرأ العمود. فهي تُبنى عند القراءة وتُوسَم
 * `synthetic` حتى لا يظنّها أحدٌ صفّاً قابلاً للعكس.
 */
class EmployeeCashboxLedger
{
    /** أقصى ما يُعاد في نداءٍ واحد — الفلترةُ بالتاريخ هي الأداة لا الحدّ. */
    private const MAX_ROWS = 500;

    /**
     * أنواعُ الحركات بأسمائها العربية.
     *
     * ⚠ الترجمةُ في الخادم لا في التطبيق: نوعٌ يُضاف غداً يظهر باسمه العربيّ
     * بلا إصدارٍ جديد من التطبيق — وبلا أن يقرأ الموظف `TRANSFER_DELIVERY`.
     * والافتراضيُّ «حركة» لا الرمزُ نفسُه.
     */
    public const TYPES = [
        'OPENING'           => 'الرصيد الافتتاحي',
        'TRANSFER_CREATED'  => 'حوالة أنشأها',
        'TRANSFER_DELIVERY' => 'حوالة سلَّمها',
        'CASH_RECEIVED'     => 'نقد مستلَم',
        'CASH_HANDOVER'     => 'نقد سلَّمه للوكيل',
        'DEPOSIT'           => 'إيداع',
        'EXPENSE'           => 'مصروف',
        'ADJUSTMENT'        => 'تسوية',
        'REVERSAL'          => 'عكس حركة',
    ];

    /** ما يجوز أن يُطلَب في `type` — والفلترةُ بغيره تُرفض لا تُهمَل. */
    public static function filterableTypes(): array
    {
        return array_values(array_diff(array_keys(self::TYPES), ['OPENING']));
    }

    public function __construct(private EmployeeCashboxService $cashbox)
    {
    }

    /**
     * الكشفُ الكامل.
     *
     * @param  array{from?:?string,to?:?string,type?:?string}  $filters
     */
    public function ledger(object $employee, array $filters = []): array
    {
        $employeeId = (int) $employee->id;

        $from = $this->day($filters['from'] ?? null);
        $to   = $this->day($filters['to'] ?? null, true);
        $type = $filters['type'] ?? null;
        if ($type !== null && !in_array($type, self::filterableTypes(), true)) {
            $type = null;   // نوعٌ مجهول ⇦ لا فلترة، لا كشفٌ فارغ
        }

        $shifts = $this->shiftsOf($employeeId, $from, $to);

        /*
         * ⚠ **الحركاتُ تُجلب بالمدى وحدَه، لا بالورديات المُجلَبة.**
         *
         * حركةٌ سُجّلت بلا وردية مفتوحة (`shift_id = NULL`) موجودةٌ في
         * القاعدة، وإسقاطُها من الكشف يجعل مجموعَ الكشف يخالف مجموعَ
         * الجدول — وهو أسوأ ما يقع في جرد.
         */
        $entries = $this->entriesOf($employeeId, $from, $to, $type);

        [$rows, $totals] = $this->build($shifts, $entries, $type !== null);

        $open = $this->cashbox->openShift($employeeId);
        $current = null;
        if ($open) {
            $c = $this->cashbox->expectedCash(
                (int) $open->cashbox_id, (int) $open->id);
            $current = round((float) $c['expected'], 3);
        }

        return [
            'from' => $from?->toDateString(),
            'to'   => $to?->toDateString(),
            'type' => $type,

            /* ⚠ صحيحٌ حين لا فلترةَ نوع. مع فلترةِ نوعٍ يبقى مجموعَ المعروض
               وحدَه، ويقول العَلَمُ ذلك صراحةً حتى لا يُقرأ الجزءُ كلاًّ. */
            'partial'      => $type !== null,

            'opening'      => $totals['opening'],
            'in'           => $totals['in'],
            'out'          => $totals['out'],
            'net'          => round($totals['in'] - $totals['out'], 3),

            /* الرصيدُ الحالي = `expectedCash` للوردية المفتوحة. `null` بلا
               وردية — وصفرٌ حينئذٍ يُقرأ «لا شيء عليك» وهو غيرُ «لم يبدأ». */
            'current'      => $current,
            'has_shift'    => $open !== null,

            'count'        => $totals['moves'],
            'shift_count'  => count($shifts),
            'truncated'    => $totals['truncated'],
            'types'        => self::TYPES,
            'rows'         => $rows,
        ];
    }

    /* ===================================================================
       البناء
       =================================================================== */

    /**
     * يبني الصفوف بالترتيب الزمنيّ مع الرصيد بعد كل حركة.
     *
     * ⚠ **الترتيبُ تصاعديّ في الحساب دائماً** ولو عُرض تنازليّاً: الرصيدُ
     * التراكميّ لا يُحسب إلّا من الأقدم إلى الأحدث، وحسابُه بالعكس يعطي
     * لكل حركةٍ رصيدَ ما بعدها. والعرضُ شأنُ التطبيق.
     */
    private function build(array $shifts, array $entries, bool $filtered): array
    {
        $rows = [];
        $totals = ['opening' => 0.0, 'in' => 0.0, 'out' => 0.0,
                   'moves' => 0, 'truncated' => false];

        /* الحركاتُ موزَّعةٌ على ورديّاتها، وما لا وردية له في دلوٍ خاصّ. */
        $byShift = [];
        $orphans = [];
        foreach ($entries as $e) {
            $sid = $e->shift_id === null ? null : (int) $e->shift_id;
            if ($sid === null) { $orphans[] = $e; continue; }
            $byShift[$sid][] = $e;
        }

        foreach ($shifts as $shift) {
            $sid     = (int) $shift->id;
            $opening = round((float) $shift->opening_cash, 3);

            $totals['opening'] += $opening;

            /*
             * ⚠ الصفُّ الافتتاحيّ **مُصطنَع**: الافتتاحيّ عمودٌ على الوردية
             * لا حركةٌ في الخزينة، وكتابتُه حركةً كانت ستُحسب مرّتين.
             *
             * ولا يُحسب في `in`: `in` تعني الحركات الداخلة، والمعادلةُ
             * «افتتاحيّ + داخل − خارج» تُفرّق بينهما. جمعُه في `in` يجعل
             * الافتتاحيّ يُعَدّ مرّتين في المجموع المعروض.
             */
            $balance = $opening;
            $rows[] = [
                'kind'       => 'OPENING',
                'synthetic'  => true,
                'shift_id'   => $sid,
                'at'         => $shift->started_at,
                'type'       => 'OPENING',
                'type_label' => self::TYPES['OPENING'],
                'reference'  => null,
                'in'         => $opening,
                'out'        => null,
                'balance'    => round($balance, 3),
                'status'     => $shift->status === 'OPEN' ? 'وردية مفتوحة'
                                                          : 'وردية مقفلة',
                'note'       => null,
            ];

            foreach ($byShift[$sid] ?? [] as $e) {
                [$row, $balance] = $this->moveRow($e, $balance, $totals);
                $rows[] = $row;
            }

            /*
             * صفُّ الإقفال — للورديات المقفلة وحدَها.
             *
             * ⚠ يُقرأ من `employee_shift_closings` لا يُعاد حسابُه: ما عُدّ
             * فعلاً في تلك الليلة هو ما حُفظ، وإعادةُ الحساب اليوم قد تُنتج
             * فرقاً آخر إن أُضيفت حركةٌ بعد الإقفال.
             */
            if ($shift->status !== 'OPEN' && $shift->closing_id !== null) {
                $rows[] = [
                    'kind'       => 'CLOSING',
                    'synthetic'  => true,
                    'shift_id'   => $sid,
                    'at'         => $shift->closed_at,
                    'type'       => 'CLOSING',
                    'type_label' => 'إقفال الوردية',
                    'reference'  => null,
                    'in'         => null,
                    'out'        => null,
                    'balance'    => round((float) $shift->expected_cash, 3),
                    'status'     => $shift->closing_label,
                    'note'       => $shift->closing_notes,
                    'expected'   => round((float) $shift->expected_cash, 3),
                    'actual'     => round((float) $shift->actual_cash, 3),
                    'difference' => round((float) $shift->difference, 3),
                ];
            }
        }

        /*
         * ⚠ الحركاتُ بلا وردية — قسمٌ خاصّ في آخر الكشف.
         *
         * لا افتتاحيّ لها فرصيدُها التراكميّ يبدأ من صفر، وهي لا تدخل في
         * `expectedCash` لأنّ تلك مقيَّدةٌ بالوردية. وإخفاؤها كان سيجعل
         * مجموعَ الكشف يخالف الجدول، فتُعرض موسومةً بأنها خارج وردية.
         */
        if ($orphans !== []) {
            $balance = 0.0;
            foreach ($orphans as $e) {
                [$row, $balance] = $this->moveRow($e, $balance, $totals);
                $row['outside_shift'] = true;
                $rows[] = $row;
            }
        }

        if (count($rows) > self::MAX_ROWS) {
            $rows = array_slice($rows, 0, self::MAX_ROWS);
            $totals['truncated'] = true;
        }

        $totals['opening'] = round($totals['opening'], 3);
        $totals['in']      = round($totals['in'], 3);
        $totals['out']     = round($totals['out'], 3);

        return [$rows, $totals];
    }

    /** صفُّ حركةٍ حقيقية، والرصيدُ بعدها. */
    private function moveRow(object $e, float $balance, array &$totals): array
    {
        $amount   = round((float) $e->amount, 3);
        $isIn     = $e->direction === 'IN';
        $reversed = (int) $e->is_reversed === 1;

        /*
         * ⚠ **المعكوسةُ تُعرض ولا تُحسب.**
         *
         * `expectedCash` تستثني الأصلَ المعكوس والعكسَ نفسَه، فلا بدّ من
         * الاستثناء نفسِه هنا وإلّا خالف رصيدُ الكشف رصيدَ الإقفال. لكنها
         * **تبقى في العرض** موسومةً: التصحيحُ بعكسٍ لا بحذف، وكشفٌ يُخفي
         * ما صُحّح لا يشهد على ما جرى.
         */
        $counts = !$reversed && $e->reversal_of === null;

        if ($counts) {
            $balance += $isIn ? $amount : -$amount;
            $totals[$isIn ? 'in' : 'out'] += $amount;
            $totals['moves']++;
        }

        return [[
            'kind'       => 'MOVE',
            'synthetic'  => false,
            'id'         => (int) $e->id,
            'shift_id'   => $e->shift_id === null ? null : (int) $e->shift_id,
            'at'         => $e->created_at,
            'type'       => $e->transaction_type,
            'type_label' => self::TYPES[$e->transaction_type] ?? 'حركة',
            'reference'  => $e->reference_id,
            'in'         => $isIn  ? $amount : null,
            'out'        => !$isIn ? $amount : null,

            /* ⚠ رصيدُ المعكوسة يساوي ما قبلها — لأنها لا تُحرّكه. */
            'balance'    => round($balance, 3),
            'counted'    => $counts,
            'reversed'   => $reversed,
            'is_reversal' => $e->reversal_of !== null,
            'status'     => $reversed ? 'معكوسة'
                          : ($e->reversal_of !== null ? 'عكس حركة' : 'مثبتة'),
            'note'       => $e->notes,
        ], $balance];
    }

    /* ===================================================================
       الجلب
       =================================================================== */

    /** ورديّاتُ الموظف في المدى، مع نتيجة إقفالها إن أُقفلت. */
    private function shiftsOf(int $employeeId, $from, $to): array
    {
        $q = DB::table('employee_shifts as s')
            ->leftJoin('employee_shift_closings as c', 'c.shift_id', '=', 's.id')
            ->where('s.employee_id', $employeeId);

        /*
         * ⚠ وردياتٌ تتقاطع مع المدى لا وردياتٌ بدأت فيه: ورديةٌ فُتحت أمس
         * ولمّا تُقفل تحمل حركاتِ اليوم، وإسقاطُها يُسقط افتتاحيَّها فيصير
         * رصيدُ حركاتها كلِّه مغلوطاً.
         */
        if ($to !== null)   $q->where('s.started_at', '<=', $to);
        if ($from !== null) {
            $q->where(fn ($w) => $w->whereNull('s.ended_at')
                                   ->orWhere('s.ended_at', '>=', $from));
        }

        return $q->orderBy('s.started_at')->orderBy('s.id')
            ->limit(200)
            ->get([
                's.id', 's.opening_cash', 's.status', 's.started_at', 's.ended_at',
                'c.id as closing_id', 'c.expected_cash', 'c.actual_cash',
                'c.difference', 'c.result', 'c.notes as closing_notes',
                'c.closed_at',
            ])
            ->map(function ($s) {
                $s->closing_label = match ($s->result) {
                    'MATCH'    => 'مطابق',
                    'SHORTAGE' => 'عجز',
                    'SURPLUS'  => 'زيادة',
                    default    => null,
                };
                return $s;
            })
            ->all();
    }

    /** حركاتُ الموظف في المدى، الأقدمُ أولاً. */
    private function entriesOf(int $employeeId, $from, $to, ?string $type): array
    {
        $q = DB::table('employee_cashbox_entries')
            ->where('employee_id', $employeeId);

        if ($from !== null) $q->where('created_at', '>=', $from);
        if ($to !== null)   $q->where('created_at', '<=', $to);
        if ($type !== null) $q->where('transaction_type', $type);

        return $q->orderBy('created_at')->orderBy('id')
            ->limit(self::MAX_ROWS + 1)
            ->get([
                'id', 'shift_id', 'transaction_type', 'reference_type',
                'reference_id', 'amount', 'direction', 'notes',
                'is_reversed', 'reversal_of', 'created_at',
            ])
            ->all();
    }

    /**
     * تاريخٌ من الطلب.
     *
     * ⚠ يُتساهَل في الشكل ولا يُرفض الطلب: تاريخٌ مشوَّه يعني كشفاً بلا
     * فلترة، لا شاشةَ خطأ أمام موظفٍ يريد جرد يومه.
     */
    private function day(?string $v, bool $endOfDay = false)
    {
        if ($v === null || trim($v) === '') return null;
        try {
            $d = \Illuminate\Support\Carbon::parse(trim($v));
        } catch (\Throwable) {
            return null;
        }
        return $endOfDay ? $d->endOfDay() : $d->startOfDay();
    }
}
