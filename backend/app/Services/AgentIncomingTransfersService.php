<?php

namespace App\Services;

use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Log;

/**
 * تتبّع تسليم الحوالات الواردة للوكيل.
 *
 * ⚠ هذه الخدمة **ليست خدمة مالية**، وقرار المالك (2 سبتمبر 2026) يجعل ذلك
 * شرطاً لا تفضيلاً: «حالة مسلمة وغير مسلمة الحالية هي المعتمدة وهي التي
 * تقوم عليها العمليات الحسابية ولا أريد المساس بها».
 *
 * فلا تكتب هذه الخدمة في `InternalEx.ConfirmType` ولا في `wallet` ولا في أي
 * قيد أو رصيد أو عمولة، ولا تستدعي خدمةً مالية. `DELIVERED` هنا تعني
 * «سجّل الوكيل أنه سلّم الحوالة للمستفيد» ولا تعني تسويةً ولا خصماً.
 *
 * أي تعديل يجعلها تكتب خارج جدولَيها يخالف الشرط — ويكسر اختبار القبول 11.
 */
class AgentIncomingTransfersService
{
    public const PENDING = 'PENDING_DELIVERY';
    public const DELIVERED = 'DELIVERED';

    /**
     * حالات المنظومة التي تعني «لا تُسلَّم».
     *
     * 3 و 4 «قيد الإلغاء» · 5 «ملغية» · 6 «ملغية مسلمة».
     * مضمّنةٌ فيها «قيد الإلغاء» عمداً: طلبُ الإلغاء وحده كافٍ لإيقاف يد
     * الوكيل عن الدفع — انتظارُ اكتماله يعني احتمال أن يدفع مالاً لحوالة
     * في طريقها إلى الإلغاء.
     */
    public const CORE_CANCELLED = [3, 4, 5, 6];

    /**
     * الحوالات التي وصلت إلى فرع الوكيل من المنظومة.
     *
     * `ConfirmType = 2` «مسلمه» تعني **سُلِّمت إلى الوكيل** لا إلى المستفيد
     * (قرار المالك، 2 سبتمبر 2026). تسليم الوكيل للمستفيد شأن هذا الجدول
     * وحده، ولا يُكتب في `InternalEx` أبداً.
     *
     * و 0 «غير معتمدة» مستبعدة: الحوالة تُنشأ بها وقد تُحوَّل عند الاعتماد
     * إلى وكيل آخر، فإظهارها قبله يجعل وكيلاً يسلّم حوالةً تُعتمد باسم غيره
     * ثم تختفي من تطبيقه بعد أن دفع مالها.
     */
    public function syncFromCore(int $agentId, int $branchId, int $userType): int
    {
        $viewName = $userType == 3
            ? 'InternalEx_SelectType_View_not_BRanchId'
            : 'InternalEx_SelectType_View_not_coustmers';

        $rows = DB::table("$viewName as a")
            ->join('InternalEx as t', 't.Code', '=', 'a.Code')
            ->where('a.BranchDeliveredID', '=', $branchId)
            ->where('t.ConfirmType', '=', 2)
            ->select([
                'a.Code',
                'a.RecievedName',
                'a.RPhone',
                'a.SenderName',
                // OverallVal هو المبلغ، و ExVal العمولة. خلطُهما يعرض على
                // الوكيل عمولةً في موضع المبلغ.
                'a.OverallVal',
                'a.ExVal',
                'a.BName',
                'a.InsertDate',
                'a.BranchDeliveredID',
            ])
            ->get();

        if ($rows->isEmpty()) {
            $this->refreshCoreState($agentId);
            return 0;
        }

        // الموجود سلفاً يُجلب باستعلام واحد لا باستعلامٍ لكل صفّ: رُصد 522
        // حوالة على حساب واحد، وفحصُ كلٍّ على حدة كان يعني مئات الاستعلامات
        // عند كل فتح للشاشة.
        $known = DB::table('agent_incoming_transfers')
            ->where('agent_id', $agentId)
            ->whereIn('transfer_number', $rows->pluck('Code'))
            ->pluck('transfer_number')
            ->flip();

        $added = 0;

        foreach ($rows as $r) {
            // موجودة سلفاً ⇒ لا تُلمس. حالتها ملكُ الوكيل لا ملكُ المزامنة:
            // إعادة كتابتها كانت ستعيد حوالةً سُلّمت إلى «بانتظار التسليم»
            // مع كل فتح للشاشة.
            if ($known->has($r->Code)) {
                continue;
            }

            try {
                DB::table('agent_incoming_transfers')->insert([
                    'agent_id'            => $agentId,
                    'transfer_number'     => $r->Code,
                    'beneficiary_name'    => $r->RecievedName,
                    'beneficiary_phone'   => $r->RPhone,
                    'sender_name'         => $r->SenderName,
                    'amount'              => $r->OverallVal,
                    'commission'          => $r->ExVal,
                    'sender_branch_name'  => $r->BName,
                    'sent_at'             => $r->InsertDate,
                    'branch_delivered_id' => $r->BranchDeliveredID,
                    'status'              => self::PENDING,
                ]);
                $added++;
            } catch (\Throwable $e) {
                // الفهرس الفريد (agent_id, transfer_number) هو الحارس: طلبان
                // متزامنان يجعلان أحدهما يسقط هنا، وهو السلوك المقصود.
                Log::debug('agent_incoming_transfers insert skipped', [
                    'code'  => $r->Code,
                    'agent' => $agentId,
                    'error' => $e->getMessage(),
                ]);
            }
        }

        $this->refreshCoreState($agentId);

        return $added;
    }

    /**
     * يعكس حالة المنظومة الحالية في صفوف الوكيل — قراءةً لا كتابةً فيها.
     *
     * بلا هذا تبقى حوالةٌ أُلغيت بعد وصولها «بانتظار التسليم» إلى الأبد،
     * لأنها تخرج من ترشيح المزامنة (= 2) فلا يعود شيء يذكرها.
     *
     * يُحدَّث `core_confirm_type` وحده — و`status` يبقى ملكاً للوكيل: حوالةٌ
     * سلّمها ثم أُلغيت تظلّ «تم التسليم» في دفتره، وتُوسَم ملغاةً فوقها.
     * طمسُ ذلك كان سيخفي عنه أنه دفع مالاً لحوالة أُلغيت.
     */
    public function refreshCoreState(int $agentId): void
    {
        $mine = DB::table('agent_incoming_transfers')
            ->where('agent_id', $agentId)
            ->pluck('core_confirm_type', 'transfer_number');

        if ($mine->isEmpty()) {
            return;
        }

        $core = DB::table('InternalEx as t')
            ->leftJoin('InternalEx_Stautes as s', 's.ConfirmType', '=', 't.ConfirmType')
            ->whereIn('t.Code', $mine->keys())
            ->select('t.Code', 't.ConfirmType', 's.SName')
            ->get();

        // ما تغيّر فعلاً وحده يُكتب، ومجموعةً واحدة لكل حالة لا صفّاً صفّاً.
        // تحديثٌ بلا تغيير يحرّك updated_at فيبدو الصفّ كأنه تعدّل في كل فتح
        // للشاشة، والتحديث صفّاً صفّاً يعني استعلاماً لكل حوالة.
        $changed = [];
        foreach ($core as $c) {
            $current = $mine[$c->Code] ?? null;
            if ($current !== null && (int) $current === (int) $c->ConfirmType) {
                continue;
            }
            $changed[$c->ConfirmType . '|' . $c->SName][] = $c->Code;
        }

        foreach ($changed as $key => $codes) {
            [$type, $label] = explode('|', $key, 2);
            DB::table('agent_incoming_transfers')
                ->where('agent_id', $agentId)
                ->whereIn('transfer_number', $codes)
                ->update([
                    'core_confirm_type' => (int) $type,
                    'core_status_label' => $label,
                    'core_synced_at'    => now(),
                ]);
        }
    }

    /**
     * تبويب ثالث محسوب لا مخزَّن.
     *
     * قرار المالك (2 سبتمبر 2026): ما تلغيه الرحالة ينتقل إلى «الملغاة»،
     * **إلا إن كان قد سُلِّم** فيبقى في «تم التسليم». فليست حالةً ثالثة في
     * العمود `status` — لو كانت، لاحتاج تسليمُ الوكيل وإلغاءُ المنظومة أن
     * يتنازعا خانةً واحدة. هي تقاطع: حالة الوكيل PENDING مع إلغاء المنظومة.
     */
    public const CANCELLED_TAB = 'CANCELLED';

    /** صفحة من حوالات الوكيل بحالةٍ ما. */
    public function list(int $agentId, ?string $status, ?string $search, int $page, int $perPage): array
    {
        $q = DB::table('agent_incoming_transfers')->where('agent_id', $agentId);

        if ($status === self::CANCELLED_TAB) {
            $q->where('status', self::PENDING)
              ->whereIn('core_confirm_type', self::CORE_CANCELLED);
        } elseif ($status === self::PENDING) {
            // «بانتظار التسليم» لا يعرض الملغاة: الوكيل لا يُطالَب بتسليمها.
            $q->where('status', self::PENDING)
              ->where(function ($w) {
                  $w->whereNull('core_confirm_type')
                    ->orWhereNotIn('core_confirm_type', self::CORE_CANCELLED);
              });
        } elseif ($status !== null && $status !== '') {
            $q->where('status', $status);
        }

        if ($search !== null && $search !== '') {
            // رقم الحوالة أو هاتف المستفيد — وهما ما تعد به خانة البحث.
            $q->where(function ($w) use ($search) {
                $w->where('transfer_number', 'like', "%{$search}%")
                  ->orWhere('beneficiary_phone', 'like', "%{$search}%");
            });
        }

        $total = (clone $q)->count();

        $items = $q->orderByDesc('id')
            ->forPage($page, $perPage)
            ->get();

        return [
            'items'     => $items,
            'total'     => $total,
            'page'      => $page,
            'per_page'  => $perPage,
            'last_page' => $perPage > 0 ? (int) ceil($total / $perPage) : 1,
        ];
    }

    /**
     * أعداد التبويبين، والملغاة معدودةٌ على حدة.
     *
     * «بانتظار التسليم» لا يعدّ الملغاة: العدد وعدٌ بعملٍ باقٍ على الوكيل،
     * وحوالةٌ ملغاة ليست عملاً بل تنبيه. أما «تم التسليم» فيعدّها لأنها
     * وقعت فعلاً مهما صار حالها بعدُ.
     */
    public function counts(int $agentId): array
    {
        $base = DB::table('agent_incoming_transfers')->where('agent_id', $agentId);

        $pending = (clone $base)
            ->where('status', self::PENDING)
            ->where(function ($w) {
                $w->whereNull('core_confirm_type')
                  ->orWhereNotIn('core_confirm_type', self::CORE_CANCELLED);
            })
            ->count();

        $delivered = (clone $base)->where('status', self::DELIVERED)->count();

        $cancelled = (clone $base)
            ->where('status', self::PENDING)
            ->whereIn('core_confirm_type', self::CORE_CANCELLED)
            ->count();

        return [
            self::PENDING   => $pending,
            self::DELIVERED => $delivered,
            'CANCELLED'     => $cancelled,
        ];
    }

    /**
     * تسجيل التسليم.
     *
     * مُتَحايِدة (idempotent): استدعاؤها مرّتين لا يسجّل تسليمين. الشرط
     * `status = PENDING` داخل جملة التحديث نفسها هو الحارس — لا فحصٌ قبلها
     * ثم تحديثٌ بعدها، فبين الاثنين تتّسع نافذةُ سباق.
     *
     * @return array{changed: bool, row: object|null}
     */
    public function markDelivered(int $agentId, int $id, int $userId, array $trace): array
    {
        return DB::transaction(function () use ($agentId, $id, $userId, $trace) {
            $row = DB::table('agent_incoming_transfers')
                ->where('id', $id)
                ->where('agent_id', $agentId)   // عزل الوكلاء — في الاستعلام لا في الواجهة
                ->first();

            if (!$row) {
                return ['changed' => false, 'row' => null];
            }

            if ($row->status === self::DELIVERED) {
                // نجاحٌ لا خطأ: الطلب المكرّر يجد ما أراده محقّقاً.
                return ['changed' => false, 'row' => $row];
            }

            // ملغاة في المنظومة ⇒ لا تُسجَّل تسليماً.
            //
            // الحارس هنا لا في الواجهة وحدها: الزرّ قد يكون مُعطَّلاً على
            // الشاشة والإلغاء وقع بعد آخر تحديث، أو يُرسَل الطلب من خارج
            // التطبيق. ومنعُ التسجيل لا يمنع الوكيل من الدفع، لكنه يمنع
            // المنظومة من أن تشهد بتسليمٍ لحوالة ملغاة.
            if (in_array((int) $row->core_confirm_type, self::CORE_CANCELLED, true)) {
                return [
                    'changed'   => false,
                    'row'       => $row,
                    'cancelled' => true,
                ];
            }

            $affected = DB::table('agent_incoming_transfers')
                ->where('id', $id)
                ->where('agent_id', $agentId)
                ->where('status', self::PENDING)
                ->update([
                    'status'       => self::DELIVERED,
                    // وقت الخادم لا ساعة الهاتف: ساعة الجهاز تُضبط باليد.
                    'delivered_at' => now(),
                    'delivered_by' => $userId,
                    'updated_at'   => now(),
                ]);

            if ($affected === 0) {
                return ['changed' => false, 'row' => $row];
            }

            DB::table('transfer_status_history')->insert([
                'transfer_id'     => $row->id,
                'transfer_number' => $row->transfer_number,
                'old_status'      => self::PENDING,
                'new_status'      => self::DELIVERED,
                'changed_by'      => $userId,
                'ip_address'      => $trace['ip'] ?? null,
                'device_id'       => $trace['device'] ?? null,
                'session_id'      => $trace['session'] ?? null,
            ]);

            $fresh = DB::table('agent_incoming_transfers')->where('id', $id)->first();

            return ['changed' => true, 'row' => $fresh];
        });
    }
}
