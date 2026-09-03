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
            $dup = DB::table('employee_cashbox_entries')
                ->where('reference_type', $data['reference_type'] ?? null)
                ->where('reference_id', $data['reference_id'])
                ->whereNull('reversal_of')
                ->value('id');
            if ($dup) {
                return ['id' => (int) $dup, 'duplicate' => true];
            }
        }

        $id = DB::table('employee_cashbox_entries')->insertGetId([
            'agent_id'         => $data['agent_id'],
            'employee_id'      => $data['employee_id'],
            'cashbox_id'       => $data['cashbox_id'],
            'shift_id'         => $data['shift_id'] ?? null,
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

        $this->log->audit('CASHBOX_ENTRY', [
            'agent_id'    => $data['agent_id'],
            'employee_id' => $data['employee_id'],
            'entity_type' => 'cashbox_entry',
            'entity_id'   => (string) $id,
            'new_value'   => ['amount' => $amount, 'direction' => $direction,
                              'type' => $data['transaction_type']],
        ]);

        return ['id' => (int) $id, 'duplicate' => false];
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
}
