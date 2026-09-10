<?php

namespace App\Services\Employees;

use Illuminate\Support\Facades\DB;

/**
 * خزينة الموظف التشغيلية والورديات.
 *
 * ⚠ **الحدّ الذي لا يُتجاوَز**، وهو شرط المالك في إذنه (3 سبتمبر 2026):
 * هذه الخدمة **تقرأ** من دفتر المنظومة ولا **تكتب** فيه أبداً. لا سطر هنا
 * يمسّ `wallet` ولا `ExchangeAccData` ولا `InternalEx` ولا
 * `EX24AccSafeActivityTb` ولا `AccountsTb`، ولا يغيّر عمولةً ولا معادلة.
 *
 * حساب الوكيل لدى الرحالة يبقى كما هو حرفياً. وهذا الدفتر يجيب سؤالاً آخر:
 * **كم نقداً في يد الموظف الآن؟** — سؤالٌ عن الصندوق في الدرج.
 *
 * ── قراران يمنعان أخطاءً كلاسيكية ─────────────────────────────────────
 * • **الرصيد يُحسب ولا يُخزَّن**: `expectedCash()` تجمع الحركات في كل مرّة.
 *   رصيدٌ مخزَّن ينحرف عن مجموع حركاته عند أول انقطاع، ثم لا يُعرف أيّهما
 *   الصحيح.
 * • **لا حذف ولا تعديل لحركة**: التصحيح بحركةٍ عكسية والأصل يبقى.
 */
class EmployeeCashboxService
{
    public const IN  = 'IN';
    public const OUT = 'OUT';

    public function __construct(private EmployeeAuditLogger $log)
    {
    }

    /* ===================================================================
       الخزينة
       =================================================================== */

    /**
     * خزينة الموظف في نقطة بيع، تُنشأ عند أول حاجة.
     *
     * خزينة لكل (موظف، نقطة بيع): موظفٌ في نقطتين له صندوقان، وخلطهما يجعل
     * عجز إحداهما يختفي في زيادة الأخرى.
     */
    public function cashboxFor(int $agentId, int $employeeId, ?int $posId, string $currency = 'LYD'): int
    {
        $existing = DB::table('employee_cashboxes')
            ->where('employee_id', $employeeId)
            ->where('currency_code', $currency)
            ->when($posId === null,
                fn($q) => $q->whereNull('point_of_sale_id'),
                fn($q) => $q->where('point_of_sale_id', $posId))
            ->value('id');

        if ($existing) {
            return (int) $existing;
        }

        return (int) DB::table('employee_cashboxes')->insertGetId([
            'agent_id'         => $agentId,
            'employee_id'      => $employeeId,
            'point_of_sale_id' => $posId,
            'currency_code'    => $currency,
            'is_active'        => 1,
            'created_at'       => now(),
        ]);
    }

    /**
     * المتوقّع نقداً: **الافتتاحي + الوارد − المسلَّم**.
     *
     * يُحسب من الحركات نفسها لا من رصيدٍ محفوظ. والحركات المعكوسة تُستبعد مع
     * عكسها معاً — وإلا حُسب المبلغ مرّتين بإشارتين وبقي الأثر.
     *
     * @return array{opening:float, in:float, out:float, expected:float}
     */
    public function expectedCash(int $cashboxId, ?int $shiftId = null): array
    {
        $shift = $shiftId !== null
            ? DB::table('employee_shifts')->where('id', $shiftId)->first()
            : DB::table('employee_shifts')
                ->where('cashbox_id', $cashboxId)
                ->where('status', 'OPEN')
                ->orderByDesc('id')->first();

        $opening = (float) ($shift->opening_cash ?? 0);

        $q = DB::table('employee_cashbox_entries')
            ->where('cashbox_id', $cashboxId)
            ->where('is_reversed', 0)          // الأصل الذي عُكس لا يُحسب
            ->whereNull('reversal_of');        // ولا العكس نفسه

        if ($shift) {
            $q->where('shift_id', $shift->id);
        }

        $sums = (clone $q)
            ->selectRaw("
                ISNULL(SUM(CASE WHEN direction = 'IN'  THEN amount ELSE 0 END), 0) AS cash_in,
                ISNULL(SUM(CASE WHEN direction = 'OUT' THEN amount ELSE 0 END), 0) AS cash_out
            ")->first();

        $in  = (float) ($sums->cash_in ?? 0);
        $out = (float) ($sums->cash_out ?? 0);

        return [
            'opening'  => $opening,
            'in'       => $in,
            'out'      => $out,
            'expected' => $opening + $in - $out,
        ];
    }

    /* ===================================================================
       الحركات
       =================================================================== */

    /**
     * تسجيل حركة خزينة.
     *
     * تحايُد مزدوج: `client_ref` يحمي من تكرار الطلب نفسه، و`reference_id`
     * يحمي من تسجيل العملية المرجعية مرّتين بطلبين مختلفين — وكلاهما فهرس
     * فريد في القاعدة، فالحماية ليست فحصاً يسبق الكتابة بل قيداً لا يُخترق.
     *
     * @return array{id:int, duplicate:bool}
     */
    public function addEntry(array $data): array
    {
        $direction = $data['direction'] === self::IN ? self::IN : self::OUT;
        $amount    = round((float) $data['amount'], 3);

        if ($amount <= 0) {
            throw new \InvalidArgumentException('المبلغ يجب أن يكون أكبر من صفر.');
        }

        // تكرارٌ معروف سلفاً ⇦ يُعاد الصفّ الأصلي بلا صفٍّ ثانٍ.
        if (!empty($data['client_ref'])) {
            $dup = DB::table('employee_cashbox_entries')
                ->where('employee_id', $data['employee_id'])
                ->where('client_ref', $data['client_ref'])
                ->value('id');
            if ($dup) {
                return ['id' => (int) $dup, 'duplicate' => true];
            }
        }
        if (!empty($data['reference_id'])) {
            // ‏agent_id ضمن المفتاح: رقمُ الحوالة قد يتكرّر بين وكيلين، فالتفرّدُ
            // على الرقم وحده يُسقط قيدَ وكيلٍ لتشابهٍ عابرٍ للوكلاء (M10).
            $dup = DB::table('employee_cashbox_entries')
                ->where('agent_id', $data['agent_id'])
                ->where('reference_type', $data['reference_type'] ?? null)
                ->where('reference_id', $data['reference_id'])
                ->whereNull('reversal_of')
                ->value('id');
            if ($dup) {
                return ['id' => (int) $dup, 'duplicate' => true];
            }
        }

        /*
         * كلُّ ما تحته في معاملةٍ واحدة كي يُمسَك قفلُ الوردية حتى الإدراج:
         *
         *  • M9 — لا تُكتب حركةٌ في ورديةٍ مُقفَلة: نقفل صفَّ الوردية (نفسُ
         *    ترتيب closeShift فلا جمود) ونتأكّد أنها OPEN؛ وإلّا نُسند القيد
         *    إلى قسمٍ مستقلّ (shift_id = NULL) فيظهر في الكشف ولا يُبتلَع في
         *    ورديةٍ أُقفلت للتوّ ولا يُفقد.
         *
         *  • MD-05 — الفحصُ أعلاه «اقرأ ثمّ اكتب» وطلبان متزامنان يمرّان معاً.
         *    الحارسُ الحقيقيّ فهرسا التفرّد؛ والخرقُ كان يتسرّب صامتاً فتسقط
         *    الحركة وتُبلَّغ الوردية عن فائضٍ وهميّ. نلتقطه ونُعيد الصفّ القائم.
         */
        return DB::transaction(function () use ($data, $amount, $direction) {
            $shiftId = $data['shift_id'] ?? null;
            if ($shiftId !== null) {
                $shift = DB::table('employee_shifts')
                    ->where('id', $shiftId)->lockForUpdate()->first();
                if (!$shift || $shift->status !== 'OPEN') {
                    $shiftId = null;
                }
            }

            try {
                $id = DB::table('employee_cashbox_entries')->insertGetId([
                    'agent_id'         => $data['agent_id'],
                    'employee_id'      => $data['employee_id'],
                    'cashbox_id'       => $data['cashbox_id'],
                    'shift_id'         => $shiftId,
                    'point_of_sale_id' => $data['point_of_sale_id'] ?? null,
                    'transaction_type' => $data['transaction_type'],
                    'reference_type'   => $data['reference_type'] ?? null,
                    'reference_id'     => $data['reference_id'] ?? null,
                    'amount'           => $amount,
                    'direction'        => $direction,
                    'currency_code'    => $data['currency_code'] ?? 'LYD',
                    'notes'            => $data['notes'] ?? null,
                    'client_ref'       => $data['client_ref'] ?? null,
                    'device_hash'      => $data['device_hash'] ?? null,
                    'created_by'       => $data['created_by'] ?? null,
                    'created_at'       => now(),
                ]);
            } catch (\Illuminate\Database\QueryException $e) {
                // ⚠ تصحيحُ وصفٍ لا تصحيحُ سلوك (لجنة الفحص — 10 سبتمبر 2026):
                // الشرطُ هنا **ليس** رقمَ خطأ SQL Server (2601/2627) كما قد
                // يُفهَم، بل وجودُ صفٍّ مطابق. فأيُّ خطأ استعلامٍ آخر يقع وقد
                // سبق وجودُ صفٍّ بنفس `client_ref` سيُقرأ «تكراراً». نافذةٌ
                // ضيّقة، وتُركت كما هي عمداً: هذا دفترُ خزينة، وتحت الخطّ
                // الأحمر لا يُغيَّر منطقُه إلّا بإذنٍ صريح من المالك.
                //
                // خرقُ التفرّد ⇦ الصفُّ كُتب في طلبٍ موازٍ. نُعيده
                // تكراراً؛ وأيُّ خطأٍ آخر يُرمى كما هو.
                $existing = null;
                if (!empty($data['client_ref'])) {
                    $existing = DB::table('employee_cashbox_entries')
                        ->where('employee_id', $data['employee_id'])
                        ->where('client_ref', $data['client_ref'])
                        ->value('id');
                }
                if (!$existing && !empty($data['reference_id'])) {
                    $existing = DB::table('employee_cashbox_entries')
                        ->where('agent_id', $data['agent_id'])
                        ->where('reference_type', $data['reference_type'] ?? null)
                        ->where('reference_id', $data['reference_id'])
                        ->whereNull('reversal_of')
                        ->value('id');
                }
                if ($existing) {
                    return ['id' => (int) $existing, 'duplicate' => true];
                }
                throw $e;
            }

            $this->log->audit('CASHBOX_ENTRY', [
                'agent_id'    => $data['agent_id'],
                'employee_id' => $data['employee_id'],
                'entity_type' => 'cashbox_entry',
                'entity_id'   => (string) $id,
                'new_value'   => ['amount' => $amount, 'direction' => $direction,
                                  'type' => $data['transaction_type']],
            ]);

            return ['id' => (int) $id, 'duplicate' => false];
        });
    }

    /**
     * عكس حركة — التصحيح الوحيد المسموح.
     *
     * الأصل يبقى ويُوسم `is_reversed`، ويُضاف صفٌّ معاكس يشير إليه. حذف
     * الحركة كان سيمحو أن خطأً وقع أصلاً، وهو ما يُراجَع لاحقاً.
     */
    public function reverseEntry(int $entryId, int $byUserId, ?string $reason = null): int
    {
        return DB::transaction(function () use ($entryId, $byUserId, $reason) {
            $orig = DB::table('employee_cashbox_entries')
                ->where('id', $entryId)->lockForUpdate()->first();

            if (!$orig) {
                throw new \InvalidArgumentException('الحركة غير موجودة.');
            }
            if ($orig->is_reversed) {
                throw new \InvalidArgumentException('الحركة معكوسة سلفاً.');
            }
            if ($orig->reversal_of !== null) {
                throw new \InvalidArgumentException('لا تُعكس حركةُ عكسٍ.');
            }

            DB::table('employee_cashbox_entries')->where('id', $entryId)
                ->update(['is_reversed' => 1]);

            $id = DB::table('employee_cashbox_entries')->insertGetId([
                'agent_id'         => $orig->agent_id,
                'employee_id'      => $orig->employee_id,
                'cashbox_id'       => $orig->cashbox_id,
                'shift_id'         => $orig->shift_id,
                'point_of_sale_id' => $orig->point_of_sale_id,
                'transaction_type' => 'REVERSAL',
                'reference_type'   => $orig->reference_type,
                'reference_id'     => null,   // الفهرس الفريد للمرجع للأصل وحده
                'amount'           => $orig->amount,
                'direction'        => $orig->direction === self::IN ? self::OUT : self::IN,
                'currency_code'    => $orig->currency_code,
                'notes'            => $reason,
                'reversal_of'      => $orig->id,
                'created_by'       => $byUserId,
                'created_at'       => now(),
            ]);

            $this->log->audit('CASHBOX_ENTRY_REVERSED', [
                'agent_id'    => $orig->agent_id,
                'employee_id' => $orig->employee_id,
                'entity_type' => 'cashbox_entry',
                'entity_id'   => (string) $orig->id,
                'new_value'   => ['reversal_id' => $id, 'reason' => $reason],
            ]);

            return (int) $id;
        });
    }

    /* ===================================================================
       الورديات
       =================================================================== */

    /**
     * بدء وردية.
     *
     * الافتتاحي **تصريحٌ من الموظف** لا حساب: هو يعدّ ما في الدرج ويعلنه.
     * وأخذُه من إقفال أمس تلقائياً يخفي أي فرقٍ حدث بين الورديتين.
     */
    public function startShift(array $data): array
    {
        $employeeId = (int) $data['employee_id'];

        $open = DB::table('employee_shifts')
            ->where('employee_id', $employeeId)->where('status', 'OPEN')->first();
        if ($open) {
            return ['id' => (int) $open->id, 'already_open' => true];
        }

        $cashboxId = $this->cashboxFor(
            (int) $data['agent_id'], $employeeId,
            $data['point_of_sale_id'] ?? null,
            $data['currency_code'] ?? 'LYD'
        );

        $opening = round((float) ($data['opening_cash'] ?? 0), 3);
        if ($opening < 0) {
            throw new \InvalidArgumentException('الرصيد الافتتاحي لا يكون سالباً.');
        }

        return DB::transaction(function () use ($data, $employeeId, $cashboxId, $opening) {
            $id = DB::table('employee_shifts')->insertGetId([
                'agent_id'         => $data['agent_id'],
                'employee_id'      => $employeeId,
                'cashbox_id'       => $cashboxId,
                'point_of_sale_id' => $data['point_of_sale_id'] ?? null,
                'opening_cash'     => $opening,
                'status'           => 'OPEN',
                'started_at'       => now(),
                'device_hash'      => $data['device_hash'] ?? null,
            ]);

            $this->log->audit('SHIFT_STARTED', [
                'agent_id'    => $data['agent_id'],
                'employee_id' => $employeeId,
                'entity_type' => 'shift',
                'entity_id'   => (string) $id,
                'new_value'   => ['opening_cash' => $opening],
            ]);

            return ['id' => (int) $id, 'already_open' => false, 'cashbox_id' => $cashboxId];
        });
    }

    /**
     * إقفال الوردية.
     *
     *     المتوقّع = الافتتاحي + الوارد − المسلَّم
     *     الفرق    = الفعلي − المتوقّع
     *
     * الفرق يُحفظ كما هو ولا يُصحَّح بحركة: الإقفال إثباتُ واقعٍ، وتحويلُ
     * العجز إلى حركةٍ تُنهيه يمحو أن عجزاً وقع.
     *
     * @return array{shift_id:int, expected:float, actual:float, difference:float, result:string, label:string}
     */
    public function closeShift(int $shiftId, float $actualCash, int $byUserId, ?string $notes = null): array
    {
        return DB::transaction(function () use ($shiftId, $actualCash, $byUserId, $notes) {
            $shift = DB::table('employee_shifts')
                ->where('id', $shiftId)->lockForUpdate()->first();

            if (!$shift) {
                throw new \InvalidArgumentException('الوردية غير موجودة.');
            }
            if ($shift->status !== 'OPEN') {
                throw new \InvalidArgumentException('الوردية مقفلة سلفاً.');
            }

            $calc     = $this->expectedCash((int) $shift->cashbox_id, (int) $shift->id);
            $actual   = round($actualCash, 3);
            $expected = round($calc['expected'], 3);
            $diff     = round($actual - $expected, 3);

            // مقارنة بعتبةٍ لا بتساوٍ صارم: حسابُ عشريّ يُنتج فروقاً بحجم
            // 0.0000001 لا وجود لها في الصندوق.
            $result = abs($diff) < 0.0005 ? 'MATCH' : ($diff < 0 ? 'SHORTAGE' : 'SURPLUS');
            $label  = ['MATCH' => 'مطابق', 'SHORTAGE' => 'عجز', 'SURPLUS' => 'زيادة'][$result];

            DB::table('employee_shift_closings')->insert([
                'shift_id'      => $shift->id,
                'agent_id'      => $shift->agent_id,
                'employee_id'   => $shift->employee_id,
                'opening_cash'  => $calc['opening'],
                'cash_in'       => $calc['in'],
                'cash_out'      => $calc['out'],
                'expected_cash' => $expected,
                'actual_cash'   => $actual,
                'difference'    => $diff,
                'result'        => $result,
                'notes'         => $notes,
                'closed_by'     => $byUserId,
                'closed_at'     => now(),
            ]);

            DB::table('employee_shifts')->where('id', $shift->id)
                ->update(['status' => 'CLOSED', 'ended_at' => now()]);

            $this->log->audit('SHIFT_CLOSED', [
                'agent_id'    => $shift->agent_id,
                'employee_id' => $shift->employee_id,
                'entity_type' => 'shift',
                'entity_id'   => (string) $shift->id,
                'new_value'   => ['expected' => $expected, 'actual' => $actual,
                                  'difference' => $diff, 'result' => $result],
            ]);

            // العجز والزيادة حدثٌ يستحقّ الانتباه، لا سطراً في سجلّ التدقيق
            // وحده — الوكيل يقرأ السجلّ الأمني ولا يقرأ آلاف صفوف التدقيق.
            if ($result !== 'MATCH') {
                $this->log->security('CASHBOX_DIFFERENCE',
                    "إقفال وردية بـ$label بمقدار " . abs($diff), [
                        'agent_id'    => $shift->agent_id,
                        'employee_id' => $shift->employee_id,
                        'severity'    => 'WARNING',
                    ]);
            }

            return [
                'shift_id'   => (int) $shift->id,
                'opening'    => $calc['opening'],
                'in'         => $calc['in'],
                'out'        => $calc['out'],
                'expected'   => $expected,
                'actual'     => $actual,
                'difference' => $diff,
                'result'     => $result,
                'label'      => $label,
            ];
        });
    }

    /** الوردية المفتوحة الآن، أو null. */
    public function openShift(int $employeeId)
    {
        return DB::table('employee_shifts')
            ->where('employee_id', $employeeId)->where('status', 'OPEN')
            ->orderByDesc('id')->first();
    }

    /**
     * ══════════════════════════════════════════════════════════════════════
     *  الوردية تُفتح بأوّل حركة — ولا تُقفل إلّا بيد الموظف بعد الجرد
     * ══════════════════════════════════════════════════════════════════════
     *
     * أمرُ المالك (10 سبتمبر 2026): «الوردية تُفتح بمجرّد فتح يومٍ جديد وبأيّ
     * حركةٍ تتمّ — صرفٌ أو قبضٌ أو حوالة — تُفتح بشكلٍ آليّ، والإقفالُ يدويٌّ
     * بعد أن يتمّ الجرد عليه آخر الوردية».
     *
     * ── وما الذي كان يقع بغيرها ──────────────────────────────────────────
     *
     * حركةُ الخزينة اليدوية كانت تُردّ بـ«ابدأ وردية أولاً»، وحركاتُ الحوالات
     * كانت **تُكتب بلا وردية** (`if ($shift)` ثمّ لا شيء): فالموظفُ الذي نسي
     * الضغط على «بدء وردية» ينشئ حوالاتٍ ويسلّمها ولا يظهر منها في عهدته شيء.
     * وهو أخطرُ من الرفض: الرفضُ يُعلِم، والصمتُ يُخفي.
     *
     * ── وثلاثةُ قراراتٍ فيها ─────────────────────────────────────────────
     *
     * ١) **الافتتاحيُّ صفر.** الوردية المفتوحة آلياً لم يُصرّح أحدٌ فيها بعهدةٍ
     *    نقدية، وافتراضُ رقمٍ غير الصفر اختلاقٌ. ومن استلم عهدةً نقدية يفتح
     *    ورديّتَه بنفسه ويُدخل قيمتَها — والزرُّ باقٍ لذلك، لم يُحذف.
     *
     * ٢) **بلا صلاحية `START_SHIFT`.** هذا فتحٌ من النظام لا فعلٌ من الموظف:
     *    اشتراطُ الصلاحية يعني أنّ من لا يملكها تُكتب حركاتُه بلا وردية —
     *    أي العطبُ نفسُه بثوبٍ آخر. والصلاحيةُ تحرس **الإعلان عن عهدةٍ
     *    افتتاحية**، وذلك ما زال خلفها.
     *
     * ٣) **ولا إقفالَ آليّ أبداً.** «يومٌ جديد» يفتح ورديةً إن لم تكن مفتوحة،
     *    ولا يُقفل مفتوحةً: الإقفالُ جردٌ وتسليمُ نقد، ولا يقع بمرور الوقت.
     *    فورديةٌ بقيت مفتوحةً من أمس تبقى، وحركةُ اليوم تدخل فيها حتى يجردها.
     *
     * ⚠ والتزامنُ يحرسه فهرسٌ فريد في القاعدة لا فحصٌ في الشيفرة:
     * `UX_shift_open_employee` — طلبان متسارعان يمرّان معاً من أيّ `EXISTS`،
     * والخاسرُ هنا يقرأ الصفَّ الذي كتبه الرابح بدل أن يفتح ورديةً ثانية.
     * وبغير الفهرس تبقى المعاملةُ والقفلُ حارساً كافياً في الحالة العادية.
     *
     * @return object|null صفُّ الوردية — و`null` تعذّرٌ نادر لا يُسقط العملية.
     */
    public function ensureOpenShift(int $agentId, int $employeeId, ?int $posId,
                                    ?string $deviceHash = null, string $currency = 'LYD')
    {
        $open = $this->openShift($employeeId);
        if ($open) {
            return $open;
        }

        try {
            return DB::transaction(function () use ($agentId, $employeeId, $posId, $deviceHash, $currency) {
                // قراءةٌ ثانية داخل المعاملة: بين القراءة الأولى وهنا قد يكون
                // طلبٌ موازٍ قد فتحها.
                $again = DB::table('employee_shifts')
                    ->where('employee_id', $employeeId)->where('status', 'OPEN')
                    ->lockForUpdate()->orderByDesc('id')->first();
                if ($again) {
                    return $again;
                }

                $cashboxId = $this->cashboxFor($agentId, $employeeId, $posId, $currency);

                $id = DB::table('employee_shifts')->insertGetId([
                    'agent_id'         => $agentId,
                    'employee_id'      => $employeeId,
                    'cashbox_id'       => $cashboxId,
                    'point_of_sale_id' => $posId,
                    'opening_cash'     => 0,
                    'status'           => 'OPEN',
                    'started_at'       => now(),
                    'device_hash'      => $deviceHash,
                ]);

                // ⚠ يُسجَّل بفعلٍ يميّزه عن الفتح اليدويّ: من يقرأ التدقيق
                // يجب أن يعرف أنّ لا أحدَ صرّح بعهدةٍ افتتاحية هنا.
                $this->log->audit('SHIFT_AUTO_STARTED', [
                    'agent_id'    => $agentId,
                    'employee_id' => $employeeId,
                    'entity_type' => 'shift',
                    'entity_id'   => (string) $id,
                    'new_value'   => ['opening_cash' => 0, 'auto' => true],
                ]);

                return DB::table('employee_shifts')->where('id', $id)->first();
            });
        } catch (\Illuminate\Database\QueryException $e) {
            // خرقُ الفهرس ⇦ طلبٌ موازٍ سبقنا. نقرأ ما كتبه.
            return $this->openShift($employeeId);
        }
    }
}
