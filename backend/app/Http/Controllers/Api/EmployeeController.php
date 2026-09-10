<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\AgentIncomingTransfersService;
use App\Services\Employees\EmployeeAuditLogger;
use App\Services\Employees\EmployeeActsAsAgent;
use App\Services\Employees\EmployeeApprovals;
use App\Services\Employees\EmployeeLimitPolicy;
use App\Services\Employees\EmployeeReports;
use App\Services\Employees\EmployeeApprovalExecutor;
use App\Services\Employees\EmployeeTransferViews;
use App\Services\Employees\EmployeeCashboxLedger;
use App\Services\Employees\EmployeeCashboxService;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Log;

/**
 * واجهة الموظف — كل ما يفعله الموظف يمرّ من هنا.
 *
 * ⚠ **السياق يُقرأ من الجلسة لا من الطلب.** الوسيط `AuthenticateEmployee`
 * يضع الموظف وصلاحياته في `$request->attributes` بعد قراءتهما من القاعدة،
 * وهذا المتحكّم لا يقرأ `agent_id` ولا `employee_id` ولا `permissions` من
 * جسم الطلب أبداً (بند 50).
 *
 * ⚠ **ولا شيء هنا يكتب في دفتر المنظومة المالي.** تسليم الحوالة يُحدّث
 * دفتر تسليم الوكيل (جدولنا)، ويسجّل نسبةً وحركة خزينة تشغيلية — ولا يمسّ
 * `wallet` ولا `ExchangeAccData` ولا `InternalEx`. حسابات الوكيل مع الرحالة
 * تبقى كما هي حرفياً.
 */
class EmployeeController extends BaseController
{
    public function __construct(
        private AgentIncomingTransfersService $transfers,
        private EmployeeCashboxService $cashbox,
        private EmployeeCashboxLedger $ledger,
        private EmployeeAuditLogger $log,
    ) {
    }

    /** @return array{0:object,1:object,2:array} الموظف، الجلسة، الصلاحيات */
    private function ctx(Request $r): array
    {
        return [
            $r->attributes->get('employee'),
            $r->attributes->get('employee_session'),
            $r->attributes->get('employee_permissions') ?? [],
        ];
    }

    /**
     * GET device/employee/approvals — طلباتُ هذا الموظف هو.
     *
     * ⚠ مقيَّدٌ بـ`employee_id` من الجلسة لا من الطلب: موظفٌ لا يرى طلبات
     * زميله ولو كانا تحت وكيلٍ واحد.
     */
    public function myApprovals(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        $rows = DB::table('employee_approval_requests')
            ->where('employee_id', $employee->id)
            ->orderByDesc('id')
            ->limit(50)
            ->get();

        return $this->sendResponse([
            'requests' => $rows->map(
                fn ($r) => \App\Services\Employees\EmployeeApprovals::present($r))->all(),
        ], 'تم');
    }

    /**
     * POST employee/approvals/{id}/execute — الموظفُ ينفّذ ما أذن به وكيلُه.
     *
     * ══════════════════════════════════════════════════════════════════════
     *  لماذا التنفيذُ هنا لا عند الوكيل
     * ══════════════════════════════════════════════════════════════════════
     *
     * أمرُ المالك (8 سبتمبر 2026): «لمّا يوافق الوكيل ترجع لتُنفَّذ من قِبل
     * الموظف، لأن المال مع الموظف وتدخل ضمن عهدته وخزينته وباسمه».
     *
     * ⚠ والسببُ التشغيليّ الذي يجعل هذا صحيحاً: **النقدُ لم يُقبض بعد**.
     * الوكيل يوافق من مكتبه، والزبونُ واقفٌ عند الموظف بالمال في يده. فحوالةٌ
     * تُنفَّذ لحظةَ الموافقة تدخل خزينةَ الموظف في عجزٍ عن مبلغٍ لم يستلمه —
     * وقد لا يستلمه أصلاً إن انصرف الزبون.
     *
     * فالإذنُ من الوكيل، والفعلُ من الموظف، والعهدةُ عليه.
     *
     * ⚠ **وحركةُ الخزينة تُسجَّل هنا `IN`** — نقدٌ دخل عهدتَه مقابل الحوالة،
     * كما يُسجَّل التسليمُ `OUT` عند صرفها. وهي حركةٌ تشغيليّة في دفتر
     * الموظف وحدَه، لا تمسّ دفترَ المنظومة ولا حسابَ الوكيل مع الرحالة.
     */
    public function executeApproval(Request $request, int $id)
    {
        [$employee, $session, ] = $this->ctx($request);

        $req = DB::table('employee_approval_requests')->where('id', $id)->first();

        /*
         * ⚠ صاحبُ الطلب وحده — لا زميلٌ تحت الوكيل نفسِه. و404 لا 403: طلبُ
         * غيره لا يُعلَم بوجوده أصلاً.
         */
        if (!$req || (int) $req->employee_id !== (int) $employee->id) {
            return $this->sendError('الطلب غير موجود.', [], 404);
        }

        if ($req->status !== 'APPROVED') {
            return $this->sendError(
                match ($req->status) {
                    'PENDING'   => 'الطلب ما زال بانتظار موافقة الوكيل.',
                    'REJECTED'  => 'رفض الوكيل هذا الطلب.',
                    'EXPIRED'   => 'انتهت مدّة الطلب. أنشئ الحوالة من جديد.',
                    'CANCELLED' => 'أُلغي هذا الطلب.',
                    default     => 'لا يمكن تنفيذ هذا الطلب الآن.',
                }, [], 422);
        }

        /*
         * ⚠ ونُفِّذ من قبل ⇦ لا يُنفَّذ ثانية.
         *
         * الحارسُ رقمُ الحوالة المحفوظ لا فحصٌ في الذاكرة: ضغطتان متسارعتان
         * تمرّان معاً على أي `if`، أمّا الرقمُ فيُكتب مرّةً واحدة — والثانية
         * تجده فتردّ بنتيجة الأولى لا بخطأ.
         */
        if ($req->transfer_number !== null) {
            return $this->sendResponse([
                'already'         => true,
                'transfer_number' => $req->transfer_number,
            ], 'نُفِّذت هذه الحوالة بالفعل.');
        }

        /*
         * ⚠ حجزٌ ذرّيّ قبل التنفيذ — الفحصُ أعلاه في الذاكرة، وضغطتان
         * متسارعتان (أو إعادةُ إرسال) تمرّان معاً عليه فتُنفَّذ حوالتان
         * لموافقةٍ واحدة. الشرطُ داخل `UPDATE` نفسِه هو الحارس، كما في
         * `markDelivered` و`decide()`: من يُغيّر صفّاً واحداً يملك التنفيذ،
         * والخاسرُ يرى صفراً. وكلُّ مسارات المنفِّذ تُنهي هذه الحالة (نجاح ⇦
         * APPROVED+رقم · فشل ⇦ FAILED · مهلةُ الدقيقة ⇦ PENDING).
         */
        $claimed = DB::table('employee_approval_requests')
            ->where('id', $id)
            ->where('status', 'APPROVED')
            ->whereNull('transfer_number')
            ->update(['status' => 'EXECUTING', 'updated_at' => now()]);

        if ($claimed !== 1) {
            $fresh = DB::table('employee_approval_requests')->where('id', $id)->first();
            if ($fresh && $fresh->transfer_number !== null) {
                return $this->sendResponse([
                    'already'         => true,
                    'transfer_number' => $fresh->transfer_number,
                ], 'نُفِّذت هذه الحوالة بالفعل.');
            }
            return $this->sendError('يُنفَّذ هذا الطلب الآن — انتظر لحظة.', [], 409);
        }

        try {
            $result = app(EmployeeApprovalExecutor::class)->execute($req);
        } catch (\Throwable $e) {
            // استثناءٌ قبل أن يُنهي المنفِّذ الحالة ⇦ نُعيدها APPROVED كي لا
            // يعلق الطلبُ في EXECUTING بلا رجعة.
            DB::table('employee_approval_requests')
                ->where('id', $id)
                ->where('status', 'EXECUTING')
                ->update(['status' => 'APPROVED', 'updated_at' => now()]);
            throw $e;
        }

        if (!$result['ok']) {
            return $this->sendError($result['message'], [], 422);
        }

        /*
         * ⚠ حركةُ الخزينة بعد نجاح الحوالة لا قبله، ولا تُبطلها إن أخفقت:
         * المالُ خرج فعلاً، ووصفُه لا يُلغيه. وهو ترتيبُ التسليم نفسُه.
         */
        $shift = $this->cashbox->ensureOpenShift(
            (int) $employee->agent_id, (int) $employee->id,
            $session->active_pos_id ?? null, $session->device_hash ?? null);
        if ($shift) {
            try {
                $this->cashbox->addEntry([
                    'agent_id'         => $employee->agent_id,
                    'employee_id'      => $employee->id,
                    'cashbox_id'       => $shift->cashbox_id,
                    'shift_id'         => $shift->id,
                    'point_of_sale_id' => $session->active_pos_id ?? null,
                    /* ⚠ نوعٌ مرجعيّ يخصّ الإنشاء وحدَه — لا `INTERNAL_TRANSFER`.
                     *
                     * الفهرسُ `UX_entry_reference` فريدٌ على (النوع، الرقم)،
                     * فلو تشارك الإنشاءُ والتسليمُ نوعاً واحداً لَاصطدم قيدُ
                     * تسليمِ حوالةٍ أنشأها الموظف نفسُه — وهو أمرٌ عاديّ حين
                     * تكون نقطةُ الوصول تابعةً للوكيل ذاته — فتُعيد `addEntry`
                     * «تكرار» بلا إدراج، فيسقط قيدُ الخروج صامتاً وتزيد عهدةُ
                     * الموظف بقيمة الحوالة. انظر 19_cashbox_ledger.sql.
                     */
                    'transaction_type' => 'TRANSFER_CREATED',
                    'reference_type'   => 'INTERNAL_TRANSFER_CREATED',
                    'reference_id'     => $result['transfer_number'],
                    'amount'           => (float) $req->amount,
                    'direction'        => EmployeeCashboxService::IN,
                    'notes'            => 'حوالة تجاوزت السقف — بموافقة الوكيل',
                    'device_hash'      => $session->device_hash ?? null,
                    'created_by'       => $employee->id,
                ]);
            } catch (\Throwable $e) {
                Log::warning('cashbox entry failed after approved transfer', [
                    'transfer' => $result['transfer_number'],
                    'error'    => $e->getMessage(),
                ]);
            }
        }

        return $this->sendResponse([
            'transfer_number' => $result['transfer_number'],
            // ⚠ البيانُ الذي طلبه المالك: تُقرأ الحوالةُ فيُعرف أنها مرّت
            // بموافقةٍ ولم تكن ضمن سقف الموظف.
            'note'            => 'حوالة من ضمن السقف بموافقة الوكيل',
            'cashbox'         => $shift !== null,
        ], 'نُفِّذت الحوالة ودخلت خزينتك.');
    }

    /**
     * POST device/employee/approvals/{id}/cancel — سحبُ الطلب قبل القرار.
     *
     * ⚠ وليس إلغاءَ حوالة: لم تُنفَّذ بعد، فلا رصيدَ يُعاد ولا قيدَ يُعكَس
     * (البند 29). وهو مسجَّلٌ في سجلّ التدقيق كحدثٍ قائم بذاته.
     */
    public function cancelApproval(Request $request, int $id)
    {
        [$employee, $session, ] = $this->ctx($request);

        $result = app(\App\Services\Employees\EmployeeApprovals::class)
            ->cancel($id, (int) $employee->id);

        $req = $result['request'];

        if (!$req || (int) $req->employee_id !== (int) $employee->id) {
            return $this->sendError('الطلب غير موجود.', [], 404);
        }

        if (!$result['changed']) {
            return $this->sendResponse([
                'already' => true,
                'status'  => $req->status,
            ], 'لم يعد الطلب قابلاً للإلغاء.');
        }

        return $this->sendResponse(['cancelled' => true], 'أُلغي الطلب.');
    }
    private function trace(Request $r, $employee, $session): array
    {
        return [
            'ip'          => $r->ip(),
            'agent_id'    => $employee->agent_id,
            'employee_id' => $employee->id,
            'device_hash' => $session->device_hash,
            'actor_type'  => 'EMPLOYEE',
        ];
    }

    /* ===================================================================
       الملف الشخصي والصلاحيات
       =================================================================== */

    /**
     * GET employee/me
     *
     * يُقرأ عند كل فتح للتطبيق ومع كل تغيّر: الواجهة تُبنى من الصلاحيات
     * التي يعيدها هذا النداء، فسحبُ صلاحية يظهر في الشاشة عند أول تحديث
     * (بند 32). ولا تُخزَّن الصلاحيات في الرمز — انظر الوسيط.
     */
    public function me(Request $request)
    {
        [$employee, $session, $permissions] = $this->ctx($request);

        $pos = DB::table('employee_point_of_sales as ep')
            ->leftJoin('AuthorizedUsers as a', 'a.ID', '=', 'ep.point_of_sale_id')
            ->where('ep.employee_id', $employee->id)
            ->where('ep.is_active', 1)
            ->select(['ep.point_of_sale_id as id', 'a.Name_post as name', 'ep.is_primary'])
            ->get();

        $shift = $this->cashbox->openShift((int) $employee->id);

        // حالةُ الإيقاف — يقرؤها تطبيقُ الموظف فيعرض شاشةَ التجميد. (المسارُ
        // بلا صلاحية فلا تحجبه البوّابة، فيعرف الموظفُ حالته دائماً.)
        $paused = ($employee->paused_at ?? null) !== null
            || DB::table('employee_pause_gate')
                ->where('agent_id', $employee->agent_id)
                ->whereNotNull('all_paused_at')->exists();

        return $this->sendResponse([
            'employee' => [
                'id'    => (int) $employee->id,
                'name'  => $employee->full_name,
                'phone' => $employee->phone,
            ],
            'active_point_of_sale_id' => $session->active_pos_id,
            'points_of_sale'          => $pos,
            'permissions'             => $permissions,
            'paused'                  => $paused,
            'pause_message'           => $paused
                ? 'أوقفَ وكيلُك الخدمةَ مؤقتاً. تواصل مع الإدارة.'
                : null,
            'open_shift'              => $shift ? [
                'id'           => (int) $shift->id,
                'opening_cash' => (float) $shift->opening_cash,
                'started_at'   => $shift->started_at,
            ] : null,
        ], 'Success');
    }

    /* ===================================================================
       الحوالات الواردة
       =================================================================== */

    /** GET employee/transfers/incoming — يتطلّب VIEW_INCOMING_TRANSFERS */
    public function incoming(Request $request)
    {
        [$employee, , $permissions] = $this->ctx($request);

        // المزامنة تتمّ باسم الوكيل: الحوالات تصل إليه لا إلى الموظف.
        try {
            $agent = DB::table('users')->where('id', $employee->agent_id)->first();
            if ($agent && !empty($agent->BrancchID)) {
                $this->transfers->syncFromCore(
                    (int) $agent->id, (int) $agent->BrancchID, (int) ($agent->UeserType ?? 0)
                );
            }
        } catch (\Throwable $e) {
            Log::warning('employee incoming sync failed', ['error' => $e->getMessage()]);
        }

        $status = $request->query('status');
        if ($status !== null && !in_array($status, [
            AgentIncomingTransfersService::PENDING,
            AgentIncomingTransfersService::DELIVERED,
            AgentIncomingTransfersService::CANCELLED_TAB,
        ], true)) {
            return $this->sendError('حالة غير معروفة.', [], 422);
        }

        /*
         * ⚠ **قائمةُ الانتظار خلف صلاحية التسليم — أمر المالك، 9 سبتمبر 2026.**
         *
         * `VIEW_INCOMING_TRANSFERS` تفتح الدفتر، و«بانتظار التسليم» ليست
         * عرضاً بل **قائمةَ عمل**: من لا يسلّم لا شأن له بها، وعرضُها عليه
         * يجعله يقول للمستفيد «حوالتُك عندي» ثم لا يستطيع تسليمها.
         *
         * وتبقى «تم التسليم» و«الملغاة» له: تلك استعلامٌ لا عمل، وهي ما
         * مُنح الصلاحيةَ من أجله.
         *
         * ⚠ والحارسُ هنا لا في الواجهة وحدَها: إخفاءُ التبويب تجميل،
         * ونداءُ المسار مباشرةً بالمعامل نفسِه كان يُعيد القائمة كاملة.
         */
        $canDeliver = in_array('DELIVER_TRANSFER', $permissions, true);

        if (!$canDeliver && $status === AgentIncomingTransfersService::PENDING) {
            return $this->sendError(
                'لا تملك صلاحية تسليم الحوالات الواردة.', [], 403);
        }

        /*
         * ولا حالةَ مطلوبة ⇦ الافتراضيُّ يتبع الصلاحية.
         *
         * تركُه بلا حالة كان سيُعيد الدفترَ كلَّه، والمعلَّقُ فيه — فيصير
         * الحارسُ أعلاه بلا معنى: يكفي حذفُ المعامل لتجاوزه.
         */
        if ($status === null && !$canDeliver) {
            $status = AgentIncomingTransfersService::DELIVERED;
        }

        $perPage = max(1, min((int) $request->query('per_page', 20), 100));

        /*
         * ⚠ «تم التسليم» = **تسليماتُ هذا الموظف وحدَه** — أمرُ المالك
         * (10 سبتمبر 2026): «لا يظهر له كل الحوالات المسلَّمة في الوكيل، بل
         * تظهر حوالته المسلَّمة من قِبل الموظف فقط».
         *
         * و«بانتظار التسليم» تبقى للوكالة كلِّها عمداً: هي **قائمةُ عمل**
         * مشتركة — أيُّ موظفٍ يسلّم أيَّ حوالةٍ واردة، ومستفيدٌ واقفٌ أمام
         * شبّاكٍ لا يُردّ لأنّ زميلاً هو من استلم الإشعار.
         *
         * والقصرُ في الخدمة لا هنا، فيسري على العدّاد كما يسري على القائمة.
         */
        $mineOnly = (int) $employee->id;

        $result = $this->transfers->list(
            (int) $employee->agent_id, $status, $request->query('search'),
            max(1, (int) $request->query('page', 1)), $perPage, $mineOnly
        );
        $result['counts'] = $this->transfers->counts((int) $employee->agent_id, $mineOnly);

        return $this->sendResponse($result, 'Success');
    }

    /**
     * POST employee/transfers/create — يتطلّب CREATE_TRANSFER
     *
     * ── أمرُ المالك (7 سبتمبر 2026) ────────────────────────────────────
     *
     * «سجلُّها الماليّ كما الوكيل، وليس أي سجلاتٍ جديدة. الموظف ينفّذ
     * الحوالة وكأنه الوكيل — واجهةٌ من وكيل، لا مستقلٌّ استقلاليةً تامّة.»
     *
     * ⚠ ولذلك **لا سطرَ منطقٍ ماليٍّ واحد هنا**: يُنادى `InternalExchange`
     * في `depositController` — الدالّةُ نفسُها التي ينفّذ بها الوكيل —
     * بهويّة الوكيل. فالكودُ والعمولةُ وحدُّ الثلاث دقائق وحدودُ التحويل
     * وفحصُ الرصيد كلُّها تجري بشيفرةِ الوكيل، ويخرج في `InternalEx` صفٌّ
     * **لا يُميَّز عن صفّه** — لأنه صفُّه.
     *
     * ونسخةٌ ثانية من هذا المنطق كانت ستفترق عن الأصل عند أوّل تعديل، فتُكتب
     * حوالتان بقاعدتين مختلفتين في دفترٍ واحد.
     *
     * ⚠ ومن نفّذ فعلاً يُسجَّل في `transfer_attributions` وحدها — بجوار
     * الدفتر لا داخله، وهو موضعُ تسجيل التسليم منذ البداية.
     */
    public function createTransfer(Request $request)
    {
        [$employee, $session, ] = $this->ctx($request);

        $actor = app(EmployeeActsAsAgent::class);

        /*
         * ⚠ الحجزُ قبل الكتابة الماليّة — ولا شيء قبله.
         *
         * ضغطةٌ مكرّرة، أو شبكةٌ ضعيفة أعادت الإرسال، تعني طلبين على المال.
         * والحارسُ فهرسٌ فريد في القاعدة لا فحصٌ في الشيفرة: طلبان
         * متسارعان يمرّان معاً على أي `EXISTS`.
         *
         * ومن جاء بمفتاحٍ معالَجٍ يُردّ عليه بنتيجة الطلب الأوّل — لا برسالة
         * خطأ: الموظف ضغط مرّتين ونجحت واحدة، فالشاشةُ يجب أن تريه نجاحاً.
         */
        $clientId = trim((string) $request->input('client_id', ''));
        $claimId = null;

        /*
         * ⚠ الترتيبُ هنا مقصود، وقد أخطأتُه مرّةً فكشفه الاختبار:
         *
         *   ١) طلبٌ مكرّرٌ بمفتاحٍ معروف  ⇐ يُردّ بنتيجته.
         *   ٢) ثمّ قاعدةُ الدقيقة.
         *   ٣) ثمّ الحجز، ثمّ التنفيذ.
         *
         * ولو سبقت قاعدةُ الدقيقة قراءةَ المفتاح لقيل للموظف «انتظر دقيقة»
         * عن حوالةٍ **نجحت للتوّ**: الضغطةُ المكرّرة تقع بعد ثوانٍ، فتقع
         * دائماً داخل المهلة. فيظنّها لم تقع ويعيدها ثالثةً.
         */
        if ($clientId !== '') {
            $prior = $actor->findClaim($employee, $clientId);

            if ($prior) {
                return $this->sendResponse([
                    'duplicate'       => true,
                    'transfer_number' => $prior->transfer_number,
                    'status'          => $prior->status,
                ], $prior->transfer_number !== null
                    ? 'هذه الحوالة أُنشئت بالفعل.'
                    : 'الطلب قيد التنفيذ — لا تُعد الإرسال.');
            }
        }

        /*
         * ══════════════════════════════════════════════════════════════
         *  طبقةُ سياسة الموظف — قبل المسار الماليّ لا داخلَه
         * ══════════════════════════════════════════════════════════════
         *
         * ⚠ هنا يُقرَّر: **هل يصل هذا الطلبُ إلى مسار الحوالة أصلاً؟**
         *
         * ولا يُغيَّر في ذلك المسار حرفٌ واحد. فإن مضى الطلبُ نُفِّذ كما كان
         * يُنفَّذ حرفياً، وإن صُعِّد توقّف **قبل** أي كتابةٍ ماليّة: لا صفَّ
         * في `InternalEx`، ولا رصيدَ يُخصم، ولا عمولةَ تُحتسب (البند 4).
         *
         * ⚠ **وموضعُها قبل قاعدة الدقيقة مقصود.** قاعدةُ الدقيقة تحرس
         * التنفيذَ الفوريّ، والطلبُ المصعَّد لن يُنفَّذ الآن بل بعد قرار
         * الوكيل — فمنعُه بمهلةٍ تخصّ لحظةً لن يُنفَّذ فيها يعني أن يقال
         * للموظف «انتظر دقيقة» عن طلبٍ سيبقى ساعاتٍ عند وكيله على أي حال.
         * والمهلةُ تُفحص من جديد عند التنفيذ الحقيقيّ، وهناك موضعُها.
         */
        $limits = app(EmployeeLimitPolicy::class);

        /* ⚠ المبلغُ والمستفيد من الطلب — وهما ما يراه الوكيل وما يُقاس. */
        $amount = (float) $request->input('amount', 0);
        $recipientName  = trim((string) $request->input('reviced_name', ''));
        $recipientPhone = trim((string) $request->input('reviced_phone', ''));

        $verdict = $limits->evaluate(
            $employee,
            $amount,
            $recipientPhone !== '' ? $recipientPhone : null,
            $recipientName !== '' ? $recipientName : null,
        );

        if ($verdict['reasons'] !== []) {
            /*
             * ⚠ مفتاحُ الطلب إلزاميّ هنا وحدَه.
             *
             * فبدونه لا يمكن منعُ ازدواج طلب الموافقة (البند 24): ضغطتان
             * تُنشئان طلبين، ويوافق الوكيل عليهما فتُنفَّذ حوالتان. والتطبيقُ
             * يرسله دائماً؛ وغيابُه يعني نداءً من خارج التطبيق فيُردّ.
             */
            if ($clientId === '') {
                return $this->sendError(
                    'تعذّر إرسال الطلب للموافقة. أعد المحاولة من التطبيق.', [], 422);
            }

            $opened = app(EmployeeApprovals::class)->open(
                $employee,
                $session,
                $clientId,
                // ⚠ يُحفظ الطلبُ **كما أرسله الموظف** ليُمرَّر كما هو عند
                // الموافقة. وإعادةُ تجميعه من حقولٍ متفرّقة فرصةٌ لأن
                // يُنفَّذ غيرُ ما راجعه الوكيل.
                $request->except(['device_id', 'AccID']),
                $amount,
                $recipientName !== '' ? $recipientName : null,
                $recipientPhone !== '' ? $recipientPhone : null,
                $verdict['reasons'],
                $verdict['policy'],
                $verdict['consumed'],
            );

            $req = $opened['request'];

            /*
             * ⚠ ولا يُقال للموظف «تمّت بنجاح» (البند 37): النجاحُ الماليّ
             * لم يقع، وقولُه يعني موظفاً يسلّم المستفيدَ مالاً على حوالةٍ
             * لم تُكتب.
             */
            return $this->sendResponse([
                'pending_approval' => true,
                'request_id'       => $req ? (int) $req->id : null,
                'status'           => 'PENDING_AGENT_APPROVAL',
                'reasons'          => $verdict['reasons'],
                'reason_labels'    => array_map(
                    [EmployeeLimitPolicy::class, 'reasonLabel'], $verdict['reasons']),
                'expires_at'       => $req->expires_at ?? null,
            ], 'قيمة الحوالة تتجاوز سقف التحويل المسموح لك، تم إرسال طلب للوكيل للموافقة.');
        }

        /*
         * ⚠ قاعدةُ الدقيقة — قبل الحجز لا بعده، وإلّا احترق مفتاحُ الطلب
         * على محاولةٍ لم تقع فلا تُعاد به بعد دقيقة.
         *
         * والرسالةُ صريحة بدل «لم يتم العثور على السجل بعد الإدخال»: 500
         * غامضٌ سببُه مهلةٌ عادية. انظر `minuteRuleBlocks`.
         */
        $agentAcc = DB::table('users')->where('id', $employee->agent_id)->value('AccID');

        if ($agentAcc !== null && $actor->minuteRuleBlocks((int) $agentAcc)) {
            return $this->sendError(
                'يمكن إنشاء حوالة أخرى بعد دقيقة من السابقة.', [], 422);
        }

        if ($clientId !== '') {
            $claim = $actor->claim($employee, $clientId);

            if (!($claim['ok'] ?? false)) {
                return $this->sendResponse([
                    'duplicate'       => true,
                    'transfer_number' => $claim['transfer_number'] ?? null,
                    'status'          => $claim['status'] ?? 'PENDING',
                ], ($claim['transfer_number'] ?? null) !== null
                    ? 'هذه الحوالة أُنشئت بالفعل.'
                    : 'الطلب قيد التنفيذ — لا تُعد الإرسال.');
            }

            $claimId = (int) $claim['claim_id'];
        }

        /* ⚠ الاستجابة تُعاد كما هي: رسائلُ الرفض التي يراها الوكيل هي
           نفسُها التي يجب أن يراها الموظف — «رصيد غير كافٍ» و«يمكن
           المحاولة بعد دقيقة» وغيرُهما. وترجمتُها هنا تعني نصّين
           يفترقان. */
        $response = $actor->as((int) $employee->agent_id, function ($agent) use ($request) {
            /*
             * ⚠ `AccID` يُملأ من الوكيل لا من الطلب.
             *
             * المُتحقِّقُ في مسار الوكيل يشترط وجودَه، والخادمُ بعد ذلك
             * **يُهمله** ويستعمل `Auth::user()->AccID`. فلو تُرك للتطبيق
             * لصار حقلاً يُرسَل ولا يُقرأ — وأسوأ من ذلك: حقلاً يظنّ من
             * يقرأ الشيفرة أنه يؤثّر، فيحاول تغييرَه يوماً.
             *
             * وملؤُه هنا يعني أيضاً أن الموظف **لا يستطيع** تسمية حسابٍ
             * آخر مهما أرسل: القيمةُ تُدهَس بحساب وكيله قبل أن تُقرأ.
             */
            $request->merge([
                'AccID'      => $agent->AccID,
                'country_id' => $request->input('country_id') ?: 1,
            ]);

            return app(depositController::class)->InternalExchange($request);
        });

        $payload = json_decode($response->getContent(), true);
        $ok = ($payload['success'] ?? false) === true;

        if ($ok) {
            $t = $payload['data']['transfer'] ?? [];
            $actor->attributeCreate(
                $employee, $session,
                (string) ($t['Code'] ?? ''),
                (float) ($t['OverallVal'] ?? $request->input('amount', 0)),
                // ⚠ ويُسجَّل المستفيد: هو مادّةُ فحص التكرار للحوالة التالية.
                $recipientPhone !== '' ? $recipientPhone : null,
                $recipientName !== '' ? $recipientName : null,
            );

            $this->log->audit('EMPLOYEE_CREATED_TRANSFER',
                $this->trace($request, $employee, $session) + [
                    'entity_type' => 'transfer',
                    'entity_id'   => (string) ($t['Code'] ?? ''),
                ]);
            /*
             * ⚠ **النقدُ يدخل خزينة الموظف مع كلّ حوالةٍ ينشئها** — أمرُ
             * المالك (8 سبتمبر 2026): «لا بدّ أن يثبت في خزينةٍ واحدة لنعرف
             * نجرد على الموظف».
             *
             * وكانت الخزينةُ تسجّل التسليمَ `OUT` ولا تسجّل الإنشاءَ `IN`،
             * فتُقرأ خزينةُ موظفٍ عمل يوماً كاملاً وكأنها لم تستقبل ديناراً
             * — والمالُ الذي قبضه من الزبائن لا أثرَ له فيها. فالجردُ عليه
             * كان يقارن نقداً في يده بدفترٍ لا يعرف من أين جاء.
             *
             * ⚠ وهي حركةٌ **تشغيليّة في دفتر الموظف وحدَه**: لا تمسّ
             * `wallet` ولا `InternalEx` ولا حسابَ الوكيل مع الرحالة. عهدةٌ
             * تُجرد، لا قيدٌ يُرحَّل.
             *
             * ⚠ وبعد نجاح الحوالة لا قبله، ولا تُبطلها إن أخفقت: المالُ
             * تحرّك فعلاً، ووصفُه لا يُلغيه. وهو ترتيبُ التسليم نفسُه.
             */
            $shift = $this->cashbox->ensureOpenShift(
                (int) $employee->agent_id, (int) $employee->id,
                $session->active_pos_id ?? null, $session->device_hash ?? null);
            if ($shift) {
                try {
                    $this->cashbox->addEntry([
                        'agent_id'         => $employee->agent_id,
                        'employee_id'      => $employee->id,
                        'cashbox_id'       => $shift->cashbox_id,
                        'shift_id'         => $shift->id,
                        'point_of_sale_id' => $session->active_pos_id ?? null,
                        /* ⚠ نوعٌ مرجعيّ يخصّ الإنشاء وحدَه — الشرحُ عند
                         * نظيره في مسار الموافقة أعلاه. */
                        'transaction_type' => 'TRANSFER_CREATED',
                        'reference_type'   => 'INTERNAL_TRANSFER_CREATED',
                        'reference_id'     => (string) ($t['Code'] ?? ''),
                        'amount'           => (float) ($t['OverallVal']
                            ?? $request->input('amount', 0)),
                        'direction'        => EmployeeCashboxService::IN,
                        'notes'            => 'قيمة حوالة أنشأها الموظف',
                        'device_hash'      => $session->device_hash ?? null,
                        'created_by'       => $employee->id,
                    ]);
                } catch (\Throwable $e) {
                    Log::warning('cashbox entry failed after transfer create', [
                        'transfer' => $t['Code'] ?? '', 'error' => $e->getMessage(),
                    ]);
                }
            }

        }

        /*
         * ⚠ ويُختم الحجز بنتيجته — النجاحُ يحفظ رقم الحوالة فيُردّ به على
         * أي تكرار، والفشلُ يُعلَّم `FAILED` فلا يبقى الطلبُ «قيد التنفيذ»
         * إلى الأبد ويعجز الموظف عن إعادة المحاولة بمفتاحٍ جديد.
         */
        if ($claimId !== null) {
            $actor->closeClaim(
                $claimId,
                $ok ? (string) (($payload['data']['transfer']['Code'] ?? '')) : null,
                $ok,
            );
        }

        return $response;
    }
    /**
     * بياناتُ المراجع لشاشة الإنشاء — الدولُ والمدنُ والفروع.
     *
     * ⚠ تُنادى **بهويّة الوكيل** كما يُنادى الإنشاء نفسُه، لا لأنها تقرأ
     * الهويّة بل لأن بعضَها يشترط وجودَها: `CoBranch_select` تبدأ بـ
     * `Auth::check()` وتردّ 401 بدونها — وهو ما ظهر عملياً.
     *
     * وتوحيدُ المسار أسلمُ من فحصِ كلِّ دالّةٍ على حدة: دالّةٌ لا تسأل عن
     * الهويّة اليوم قد تسألها غداً، وحينها يسقط مسارُ الموظف بلا سبب ظاهر.
     */
    /**
     * GET employee/branding — هويّةُ شركة وكيله.
     *
     * ⚠ **بلا صلاحية**، وهو مقصود: الهويّةُ ليست بياناً يُمنح أو يُمنع، بل
     * اسمُ الشركة وشعارُها على كلّ فاتورةٍ يطبعها الموظف ويسلّمها للزبون.
     * ومنعُها يعني فاتورةً باسمِ شركةٍ أخرى في يد الزبون — لا حمايةً.
     *
     * ⚠ والشركةُ تُشتقّ من **وكيل الموظف** في القاعدة، لا مما يُرسله
     * التطبيق: موظفٌ يطلب هويّةَ شركةٍ أخرى لا يجد إلى ذلك سبيلاً.
     *
     * ⚠ ولا تعديلَ من هنا: `can_edit` تعود false دائماً. تغييرُ الهويّة
     * فعلُ صاحبِ الشركة وحدَه، وهو خلف جلسة الوكيل.
     */
    public function branding(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        $accId = DB::table('users')->where('id', $employee->agent_id)->value('AccID');
        $svc = app(\App\Services\CompanyBrandingService::class);

        if (empty($accId)) {
            return $this->sendResponse([
                'branding' => $svc->forCompany(0),
                'can_edit' => false,
                'themes'   => [],
            ], 'Success');
        }

        $name = null;
        try {
            $name = DB::table('AccountsTb')->where('AccID', $accId)->value('AccName');
        } catch (\Throwable) {
        }

        return $this->sendResponse([
            'branding' => $svc->forCompany((int) $accId, $name),
            'can_edit' => false,
            'themes'   => [],
        ], 'Success');
    }

    /* ===================================================================
       التقارير والأرصدة والمفضّلة — قراءةٌ خالصة
       ===================================================================

       ⚠ **كلُّها للقراءة، ولا واحدةَ منها تكتب في أي جدولٍ ماليّ.** والأرصدةُ
       تُقرأ بمسار الوكيل نفسِه (`EmployeeActsAsAgent`) لا بحسابٍ ثانٍ: رقمٌ
       يُحسب هنا بطريقةٍ أخرى يفترق عمّا يراه الوكيل في تطبيقه، فيصير للرصيد
       جوابان.

       وكلُّ مسارٍ خلف صلاحيته وحدَه — فالوكيل يمنح ما يشاء ويمنع ما يشاء.
       =================================================================== */

    /** GET employee/reports/daily — يتطلّب REPORT_DAILY_TRANSFERS */
    public function reportDaily(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        return $this->sendResponse(
            app(EmployeeReports::class)->daily($employee, $request->query('date')),
            'تم',
        );
    }

    /** GET employee/reports/delivered — يتطلّب REPORT_DELIVERED_TRANSFERS */
    public function reportDelivered(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        return $this->sendResponse(
            app(EmployeeReports::class)->delivered($employee, (int) $request->query('days', 30)),
            'تم',
        );
    }

    /** GET employee/reports/pending — يتطلّب REPORT_PENDING_TRANSFERS */
    public function reportPending(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        return $this->sendResponse(
            app(EmployeeReports::class)->pending($employee),
            'تم',
        );
    }

    /**
     * GET device/employee/cashbox/ledger — كشفُ حركة خزينته للجرد.
     *
     * ⚠ **مقيَّدٌ بـ`employee_id` من الجلسة لا من الطلب**: كلُّ موظفٍ يرى
     * كشفَه هو. ولا معامل في هذا النداء يقبل معرّفَ موظف — فلا سبيلَ إلى
     * كشفِ زميلٍ ولو عُرف رقمُه.
     *
     * ⚠ وقراءةٌ خالصة: لا يكتب صفّاً واحداً.
     */
    public function cashboxLedger(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        return $this->sendResponse(
            $this->ledger->ledger($employee, [
                'from' => $request->query('from'),
                'to'   => $request->query('to'),
                'type' => $request->query('type'),
            ]),
            'تم'
        );
    }
    /**
     * GET employee/custody — ما في عهدته الآن، سطرٌ واحد.
     *
     * ══════════════════════════════════════════════════════════════════════
     *  المعنى المحاسبيّ
     * ══════════════════════════════════════════════════════════════════════
     *
     * ⚠ **النقدُ في درج الموظف ليس ملكَه — هو عهدةٌ للوكيل.** فكلُّ دينارٍ
     * فيه التزامٌ عليه، وحين يسلّمه ينقضي.
     *
     *     المتوقَّع = الافتتاحيّ + الداخل − الخارج
     *
     * ⚠ **والإشارةُ تقلب المعنى**، وهي ما يجعل الرقم محاسبياً صحيحاً:
     *
     *   • موجبٌ ⇦ نقدٌ في يده، **مستحقٌّ عليه** يسلّمه للوكيل.
     *   • سالبٌ ⇦ دفع أكثر ممّا قبض، **مستحقٌّ له** على الوكيل. وهي حالةٌ
     *     واقعية: موظفٌ بدأ ورديّته بلا نقدٍ ثمّ سلّم حوالاتٍ من ماله.
     *
     * ⚠ **وهو عهدةٌ لا حساب**: لا يُقرأ من `wallet` ولا يُكتب فيه، ولا يظهر
     * في الشجرة المحاسبية. جردٌ على من يحمل المال، لا قيدٌ يُرحَّل.
     *
     * ⚠ ومستقلٌّ عن `cashbox` عمداً: هذا يُسأل مع كلّ فتحٍ للشاشة الرئيسية،
     * وحملُ `cashbox` قائمةُ الحركات كلِّها. سؤالٌ يتكرّر يجب أن يكون
     * أرخصَ ما يمكن.
     */
    public function custody(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        $shift = $this->cashbox->openShift((int) $employee->id);

        if (!$shift) {
            /*
             * ⚠ لا ورديةَ ⇦ لا عهدة. ولا يُعرض صفرٌ: صفرٌ يُقرأ «لا شيء
             * عليك» وهو غيرُ «لم تبدأ بعد» — والفرقُ بينهما يومُ عملٍ كامل.
             */
            return $this->sendResponse([
                'has_shift' => false,
                'expected'  => null,
            ], 'لا وردية مفتوحة.');
        }

        $c = $this->cashbox->expectedCash((int) $shift->cashbox_id, (int) $shift->id);

        return $this->sendResponse([
            'has_shift'  => true,
            'opening'    => round((float) $c['opening'], 3),
            'in'         => round((float) $c['in'], 3),
            'out'        => round((float) $c['out'], 3),
            'expected'   => round((float) $c['expected'], 3),
            'started_at' => $shift->started_at ?? null,
        ], 'تم');
    }

    /**
     * GET employee/statement — كشفُ حساب حوالاته للجرد.
     *
     * ⚠ تحت `VIEW_OWN_TRANSFERS`: من يرى حوالاته يرى كشفَها. ولا صلاحيةَ
     * جديدة لشيءٍ هو تجميعُ ما يراه أصلاً.
     */
    public function transferStatement(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        return $this->sendResponse(
            app(EmployeeReports::class)->statement(
                $employee, (int) $request->query('days', 30)),
            'تم');
    }

    /** GET employee/reports/cashbox — يتطلّب REPORT_EMPLOYEE_CASHBOX */
    public function reportCashbox(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        return $this->sendResponse(
            app(EmployeeReports::class)->cashbox($employee, (int) $request->query('days', 7)),
            'تم',
        );
    }

    /**
     * GET employee/reports/point-of-sale — يتطلّب REPORT_POINT_OF_SALE
     *
     * ⚠ ونقطةُ البيع تُؤخذ من **الجلسة** لا من الطلب: موظفٌ يرسل معرّفَ
     * نقطةِ بيعٍ أخرى كان سيرى عملَ من ليس معه.
     */
    public function reportPointOfSale(Request $request)
    {
        [$employee, $session, ] = $this->ctx($request);

        return $this->sendResponse(
            app(EmployeeReports::class)->pointOfSale(
                $employee,
                $session->active_pos_id ?? null,
                (int) $request->query('days', 7),
            ),
            'تم',
        );
    }

    /** GET employee/reports/audit — يتطلّب REPORT_AUDIT */
    public function reportAudit(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        return $this->sendResponse(
            app(EmployeeReports::class)->audit($employee, (int) $request->query('days', 14)),
            'تم',
        );
    }

    /**
     * GET employee/balance — رصيدُ الوكيل الكلّي. يتطلّب VIEW_AGENT_TOTAL_BALANCE
     *
     * ⚠ **قراءةٌ بمسار الوكيل نفسِه.** لا استعلامَ ثانياً على `wallet` ولا
     * جمعَ أرصدةٍ هنا: الرقمُ الذي يراه الموظف هو الرقمُ الذي يراه وكيله،
     * حرفاً بحرف، لأنه من الدالّة نفسِها.
     */
    public function agentBalance(Request $request)
    {
        return $this->balanceAsAgent($request);
    }

    /**
     * قراءةُ الرصيد بمسار الوكيل — والعملةُ تُملأ إن لم تُرسَل.
     *
     * ⚠ الدالّةُ الأصليّة تشترط `currency_id` وترفض بـ422 بدونه. وتركُ
     * ذلك للتطبيق يعني شاشةً تُخفق برسالةٍ إنجليزية «Validation Error»
     * لا يفهمها الموظف — فيُملأ هنا بالدينار الليبيّ، وهو عملةُ
     * التطبيق كلِّه، ويبقى قابلاً للتجاوز إن أُرسل.
     */
    private function balanceAsAgent(Request $request)
    {
        $request->merge([
            'currency_id' => $request->input('currency_id') ?: 1,
        ]);

        return $this->refAsAgent($request, 'getBalanceLocal');
    }

    /** GET employee/summary — يتطلّب VIEW_FINANCIAL_SUMMARY */
    public function financialSummary(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        return $this->sendResponse(
            app(EmployeeReports::class)->summary($employee), 'تم');
    }

    /**
     * GET employee/reports/agent-balance — يتطلّب REPORT_AGENT_BALANCE
     *
     * ⚠ المصدرُ نفسُه الذي يخدم `VIEW_AGENT_TOTAL_BALANCE`، والصلاحيةُ
     * غيرُها: الأولى تفتح رقمَ الرصيد في قسم التقارير، والثانية تعرضه في
     * الشاشة الرئيسية. ووكيلٌ قد يريد أحدهما دون الآخر.
     */
    public function reportAgentBalance(Request $request)
    {
        return $this->balanceAsAgent($request);
    }

    /** GET employee/favorites — يتطلّب VIEW_FAVORITES */
    public function favorites(Request $request)
    {
        return $this->refAsAgent($request, 'Favorites');
    }

    /** POST employee/favorites/add — يتطلّب MANAGE_FAVORITES */
    public function favoriteAdd(Request $request)
    {
        return $this->refAsAgent($request, 'Favorites_Table_inser');
    }

    /** POST employee/favorites/delete — يتطلّب MANAGE_FAVORITES */
    public function favoriteDelete(Request $request)
    {
        return $this->refAsAgent($request, 'Favorites_Table_delete');
    }

    private function refAsAgent(Request $request, string $method)
    {
        [$employee, , ] = $this->ctx($request);

        return app(EmployeeActsAsAgent::class)->as(
            (int) $employee->agent_id,
            fn () => app(depositController::class)->{$method}($request),
        );
    }

    /** POST employee/ref/countries — يتطلّب CREATE_TRANSFER */
    public function refCountries(Request $request)
    {
        return $this->refAsAgent($request, 'getCountries');
    }

    /** POST employee/ref/cities — يتطلّب CREATE_TRANSFER */
    public function refCities(Request $request)
    {
        return $this->refAsAgent($request, 'GetCities');
    }

    /** GET employee/ref/branches — يتطلّب CREATE_TRANSFER */
    public function refBranches(Request $request)
    {
        return $this->refAsAgent($request, 'CoBranch_select');
    }

    /**
     * GET employee/transfers/search?q= — يتطلّب SEARCH_TRANSFER
     *
     * ⚠ في دفتر وكيله وحده: الخدمة تأخذ `agent_id` فتحرس العزل بنفسها.
     */
    public function searchTransfer(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        $out = app(EmployeeTransferViews::class)
            ->search((int) $employee->agent_id, (string) $request->query('q', ''));

        if (isset($out['error'])) {
            return $this->sendError($out['error'], [], 422);
        }

        return $this->sendResponse($out, 'Success');
    }

    /**
     * GET employee/transfers/outgoing — يتطلّب VIEW_OWN_TRANSFERS
     *
     * «الصادرة» في تبويب حوالات الموظف: ما أنشأه هو، بحالته في المنظومة.
     * التفصيل — ولماذا لا تُبنى على كشف الوكيل — في [EmployeeTransferViews::outgoing].
     *
     * ⚠ الصلاحيةُ هي `VIEW_OWN_TRANSFERS` نفسُها لا مفتاحٌ جديد: السؤالُ هو
     * سؤالُها («ما حوالاتي؟»)، ومفتاحٌ ثانٍ له معناه يعني وكيلاً يمنح أحدهما
     * ويظنّ أنه منح الآخر.
     */
    public function outgoingTransfers(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        return $this->sendResponse(
            app(EmployeeTransferViews::class)->outgoing(
                (int) $employee->agent_id,
                (int) $employee->id,
                (int) $request->query('limit', 200),
            ),
            'Success');
    }

    /**
     * GET employee/transfers/outgoing/{code} — يتطلّب VIEW_OWN_TRANSFERS
     *
     * فاتورةُ حوالةٍ أنشأها هذا الموظف — نظيرُ `agent/outgoing-transfers/{code}`
     * وبنفس الاستعلام حرفياً (`AgentIncomingTransfersService::outgoingRowByCode`)،
     * والمختلفُ فحصُ الملكية وحدَه.
     */
    public function outgoingTransferByCode(Request $request, string $code)
    {
        [$employee, , ] = $this->ctx($request);

        $row = app(EmployeeTransferViews::class)->outgoingByCode(
            (int) $employee->agent_id, (int) $employee->id, $code);

        if (!$row) {
            return $this->sendError('الحوالة غير موجودة.', [], 404);
        }

        return $this->sendResponse($row, 'Success');
    }

    /** GET employee/transfers/mine — يتطلّب VIEW_OWN_TRANSFERS */
    public function myTransfers(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        return $this->sendResponse(
            app(EmployeeTransferViews::class)->mine(
                (int) $employee->agent_id,
                (int) $employee->id,
                (int) $request->query('page', 1),
                (int) $request->query('per_page', 20),
                // ⚠ مقيَّدٌ بصاحبه: موظّفٌ لا يرى عملَ زميله على النقطة نفسِها.
                (int) $employee->id,
            ),
            'Success');
    }

    /** GET employee/transfers/point-of-sale — يتطلّب VIEW_POS_TRANSFERS */
    public function posTransfers(Request $request)
    {
        [$employee, $session, ] = $this->ctx($request);

        // ⚠ نقطةُ البيع من **الجلسة** لا من صفّ الموظّف:
        // الموظّف قد يعمل على شبّاكٍ اليوم وآخرَ غداً، والنّسبة
        // تُكتب بالنّقطة التي كان عليها حينئذ. ولا عمودَ لها
        // على `employees` أصلاً.
        return $this->sendResponse(
            app(EmployeeTransferViews::class)->pointOfSale(
                (int) $employee->agent_id,
                isset($session->active_pos_id) && $session->active_pos_id
                    ? (int) $session->active_pos_id : null,
                (int) $request->query('page', 1),
                (int) $request->query('per_page', 20),
                // ⚠ مقيَّدٌ بصاحبه: موظّفٌ لا يرى عملَ زميله على النقطة نفسِها.
                (int) $employee->id,
            ),
            'Success');
    }

    /**
     * POST employee/transfers/{id}/deliver — يتطلّب DELIVER_TRANSFER
     *
     * ثلاثة آثار، ولا رابع:
     *   1. دفتر تسليم الوكيل ⇦ «تم التسليم».
     *   2. نسبة العملية إلى الموظف ونقطة البيع والجهاز.
     *   3. حركة خزينة تشغيلية OUT بقيمة الحوالة — إن كانت للموظف وردية.
     *
     * ولا شيء منها يمسّ دفتر المنظومة المالي.
     */
    public function deliver(Request $request, int $id)
    {
        [$employee, $session, ] = $this->ctx($request);
        $trace = $this->trace($request, $employee, $session);

        /*
         * ⚠ المنفِّذُ الماليّ هو الوكيل (`userId = agent_id`) — الموظف واجهةٌ
         * منه لا كيانٌ ثانٍ، وذلك ما يجعل كشفَ الوكيل يبقى كما هو حرفاً
         * بحرف. وهويةُ الموظف تمرّ في الأثر فتُسجَّل بلا أن تدخل الدفتر.
         */
        $result = $this->transfers->markDelivered(
            (int) $employee->agent_id, $id, (int) $employee->agent_id,
            [
                'ip'          => $request->ip(),
                'device'      => $session->device_hash,
                'session'     => (string) $session->id,
                'employee_id' => (int) $employee->id,
            ]
        );

        if ($result['row'] === null) {
            return $this->sendError('الحوالة غير موجودة.', [], 404);
        }
        // اختفى أصلها من المنظومة — كالحارس نفسه في مسار الوكيل.
        if (!empty($result['missing'])) {
            return $this->sendError('هذه الحوالة لم تعد موجودة في المنظومة.', [], 404);
        }
        /*
         * ⚠ سُحب اعتمادُها في المنظومة بعد أن وصلت — انظر الحارسَ في
         * `markDelivered`. ورسالةٌ تقول ذلك صراحةً تمنع الوكيلَ من الدفع
         * بينما يظنّ العطبَ في التطبيق.
         */
        if (!empty($result['not_approved'])) {
            return $this->sendError(
                'هذه الحوالة غير معتمدة في المنظومة — لا يجوز تسليمها.',
                ['core_status' => $result['row']->core_status_label ?? null], 409);
        }

        if (!empty($result['cancelled'])) {
            return $this->sendError(
                'هذه الحوالة ملغاة في المنظومة — لا يجوز تسجيل تسليمها.',
                ['core_status' => $result['row']->core_status_label], 409
            );
        }

        // النسبة والحركة تُسجَّلان مرّة واحدة فقط — عند التغيير الفعلي.
        if (!empty($result['changed'])) {
            $row = $result['row'];
            $posId = $session->active_pos_id;

            DB::table('transfer_attributions')->insert([
                'action'           => 'DELIVERED',
                'transfer_number'  => $row->transfer_number,
                'agent_id'         => $employee->agent_id,
                'employee_id'      => $employee->id,
                'point_of_sale_id' => $posId,
                'device_hash'      => $session->device_hash,
                'session_id'       => $session->id,
                'amount'           => $row->amount,
                'occurred_at'      => now(),
            ]);

            // الحركة تُسجَّل داخل الوردية المفتوحة وحدها: بلا وردية لا يوجد
            // افتتاحيّ ولا إقفال، فحركةٌ خارجها لا تدخل في أي معادلة.
            $shift = $this->cashbox->ensureOpenShift(
                (int) $employee->agent_id, (int) $employee->id,
                $posId, $session->device_hash ?? null);
            if ($shift) {
                try {
                    $this->cashbox->addEntry([
                        'agent_id'         => $employee->agent_id,
                        'employee_id'      => $employee->id,
                        'cashbox_id'       => $shift->cashbox_id,
                        'shift_id'         => $shift->id,
                        'point_of_sale_id' => $posId,
                        'transaction_type' => 'TRANSFER_DELIVERY',
                        'reference_type'   => 'INTERNAL_TRANSFER',
                        'reference_id'     => $row->transfer_number,
                        'amount'           => (float) $row->amount,
                        'direction'        => EmployeeCashboxService::OUT,
                        'device_hash'      => $session->device_hash,
                        'created_by'       => $employee->id,
                    ]);
                } catch (\Throwable $e) {
                    // التسليم مُسجَّل في دفتر الوكيل وهو الأهمّ؛ وفشل حركة
                    // الخزينة يُسجَّل ولا يُبطل التسليم.
                    Log::warning('cashbox entry failed after delivery', [
                        'transfer' => $row->transfer_number, 'error' => $e->getMessage(),
                    ]);
                }
            }

            /*
             * ⚠ الحالتان القديمةُ والجديدة في صفّ التدقيق نفسِه.
             *
             * صفُّ `transfer_status_history` يحملهما، وصفُّ التدقيق يحمل
             * الموظفَ والوكيلَ والجهاز. وقارئُ الرقابة يسأل سؤالاً واحداً —
             * «من حوّل ماذا من أيّ حالة إلى أيّ حالة؟» — فيجيبه صفٌّ واحد
             * لا توفيقٌ بين جدولين بالتاريخ.
             */
            $this->log->audit('TRANSFER_DELIVERED', [
                'entity_type' => 'transfer',
                'entity_id'   => $row->transfer_number,
                'point_of_sale_id' => $posId,
                'old_value'   => ['status' => AgentIncomingTransfersService::PENDING],
                'new_value'   => [
                    'status'     => AgentIncomingTransfersService::DELIVERED,
                    'amount'     => $row->amount,
                    'session_id' => (string) $session->id,
                ],
            ] + $trace);
        }

        /*
         * ══════════════════════════════════════════════════════════════════
         *  «سُلِّمت مسبقاً» — ولمن تُقال؟
         * ══════════════════════════════════════════════════════════════════
         *
         * الطلبُ الثاني بعد تسليمٍ ناجح لا يُغيّر شيئاً (`changed = false`)،
         * لكن **ليس كلُّ طلبٍ ثانٍ محاولةً ثانية**:
         *
         *   • شبكةٌ انقطعت بعد أن وصل الطلب فأعاد التطبيق إرساله ⇦ نجاحٌ
         *     صامت. ورسالةُ رفضٍ هنا تُخبر الموظف أن تسليمَه فشل وقد نجح،
         *     فيدفع مرّةً ثانية — وهذا أخطر ما يقع في هذا المسار.
         *
         *   • وزميلٌ سبقه إليها، أو الوكيلُ سلّمها من تطبيقه ⇦ **هنا** تُقال
         *     الرسالة صراحةً: «تم تسليم هذه الحوالة مسبقاً…».
         *
         * والفرقُ يُقرأ من `transfer_attributions`: من نُسب إليه التسليم.
         * فإن كان هو نفسَه فطلبُه الأول نجح، وإن كان غيرَه فقد سبقه.
         *
         * ⚠ والحالتان **تُعادان بـ200 لا بخطأ**: الحالةُ المطلوبة محقَّقةٌ
         * فعلاً (الحوالة مسلَّمة)، والحمولةُ تحمل الصفَّ الطازج فيسقط الزرّ
         * من شاشة الجميع. رمزُ خطأٍ هنا يُبقي بعض التطبيقات تعرض القائمة
         * القديمة لأنها لا تقرأ حمولةَ الأخطاء.
         */
        $mine = null;
        if (!$result['changed']
            && ($result['row']->status ?? null) === AgentIncomingTransfersService::DELIVERED) {

            $by = DB::table('transfer_attributions')
                ->where('action', 'DELIVERED')
                ->where('transfer_number', $result['row']->transfer_number)
                ->orderBy('id')
                ->first(['employee_id']);

            // لا نسبة ⇦ سلّمها الوكيل من تطبيقه.
            $mine = $by !== null && (int) $by->employee_id === (int) $employee->id;
        }

        $message = match (true) {
            $result['changed'] => 'تم تسجيل التسليم.',
            $mine === true     => 'تم تسجيل التسليم.',
            $mine === false    => 'تم تسليم هذه الحوالة مسبقاً ولا يمكن تنفيذ العملية مرة أخرى.',
            default            => 'الحوالة مسجّلة كمسلَّمة سلفاً.',
        };

        return $this->sendResponse([
            'transfer' => $result['row'],
            'changed'  => $result['changed'],

            // ⚠ عَلَمٌ صريح: التطبيق لا يستنبط الحالة من نصّ الرسالة.
            'already_delivered_by_other' => $mine === false,

            'counts'   => $this->transfers->counts((int) $employee->agent_id),
        ], $message);
    }

    /* ===================================================================
       الخزينة والورديات
       =================================================================== */

    /** GET employee/cashbox — يتطلّب VIEW_OWN_CASHBOX */
    public function cashbox(Request $request)
    {
        [$employee, $session, ] = $this->ctx($request);

        $shift = $this->cashbox->openShift((int) $employee->id);
        if (!$shift) {
            return $this->sendResponse([
                'open_shift' => null,
                'summary'    => null,
                'entries'    => [],
            ], 'لا توجد وردية مفتوحة.');
        }

        $calc = $this->cashbox->expectedCash((int) $shift->cashbox_id, (int) $shift->id);

        $entries = DB::table('employee_cashbox_entries')
            ->where('shift_id', $shift->id)
            ->orderByDesc('id')
            ->limit(100)
            ->get([
                'id', 'transaction_type', 'reference_type', 'reference_id',
                'amount', 'direction', 'notes', 'is_reversed', 'reversal_of',
                'created_at',
            ]);

        return $this->sendResponse([
            'open_shift' => [
                'id'           => (int) $shift->id,
                'opening_cash' => (float) $shift->opening_cash,
                'started_at'   => $shift->started_at,
            ],
            // المعادلة تُعاد كاملةً لا نتيجتها وحدها: الموظف يرى من أين جاء
            // الرقم، فلا يفاجئه المتوقّع عند الإقفال.
            'summary' => [
                'opening'  => $calc['opening'],
                'in'       => $calc['in'],
                'out'      => $calc['out'],
                'expected' => $calc['expected'],
            ],
            'entries' => $entries,
        ], 'Success');
    }

    /** POST employee/cashbox/entry — يتطلّب CASHBOX_ENTRY */
    public function addEntry(Request $request)
    {
        [$employee, $session, ] = $this->ctx($request);

        $data = $request->validate([
            'amount'     => 'required|numeric|min:0.001',
            'direction'  => 'required|string|in:IN,OUT',
            'notes'      => 'nullable|string|max:500',
            'client_ref' => 'nullable|string|max:80',
        ]);

        /*
         * ⚠ لا تُردّ حركةٌ بـ«ابدأ وردية أولاً» — أمرُ المالك (10 سبتمبر 2026):
         * أيُّ حركةٍ تفتح ورديةً إن لم تكن مفتوحة. والصرفُ والقبضُ حركة.
         *
         * وكان الرفضُ يعني أنّ الموظف يقبض مالاً من زبونٍ ثم يُخبره التطبيقُ
         * أنّ عليه إجراءً إدارياً أوّلاً — فيُسجّله متأخّراً أو لا يسجّله.
         */
        $shift = $this->cashbox->ensureOpenShift(
            (int) $employee->agent_id, (int) $employee->id,
            $session->active_pos_id, $session->device_hash ?? null);

        if (!$shift) {
            return $this->sendError('تعذّر فتح وردية — أعد المحاولة.', [], 422);
        }

        try {
            $res = $this->cashbox->addEntry([
                'agent_id'         => $employee->agent_id,
                'employee_id'      => $employee->id,
                'cashbox_id'       => $shift->cashbox_id,
                'shift_id'         => $shift->id,
                'point_of_sale_id' => $session->active_pos_id,
                'transaction_type' => $data['direction'] === 'IN'
                    ? 'CASH_RECEIVED' : 'CASH_HANDOVER',
                'amount'           => $data['amount'],
                'direction'        => $data['direction'],
                'notes'            => $data['notes'] ?? null,
                'client_ref'       => $data['client_ref'] ?? null,
                'device_hash'      => $session->device_hash,
                'created_by'       => $employee->id,
            ]);
        } catch (\InvalidArgumentException $e) {
            return $this->sendError($e->getMessage(), [], 422);
        }

        $calc = $this->cashbox->expectedCash((int) $shift->cashbox_id, (int) $shift->id);

        return $this->sendResponse(
            ['entry_id' => $res['id'], 'duplicate' => $res['duplicate'], 'summary' => $calc],
            $res['duplicate'] ? 'الحركة مسجّلة سلفاً.' : 'سُجّلت الحركة.'
        );
    }

    /** POST employee/shift/start — يتطلّب START_SHIFT */
    public function startShift(Request $request)
    {
        [$employee, $session, ] = $this->ctx($request);

        $data = $request->validate([
            'opening_cash'     => 'required|numeric|min:0',
            'point_of_sale_id' => 'nullable|integer',
        ]);

        // نقطة البيع تُؤخذ من الجلسة، ولا تُقبل من الطلب إلا إن كانت من
        // نقاط الموظف فعلاً — وإلا سُجّلت عملياته على نقطة بيع ليست له.
        $posId = $session->active_pos_id;
        if (!empty($data['point_of_sale_id'])) {
            $allowed = DB::table('employee_point_of_sales')
                ->where('employee_id', $employee->id)
                ->where('point_of_sale_id', $data['point_of_sale_id'])
                ->where('is_active', 1)->exists();
            if (!$allowed) {
                return $this->sendError('نقطة بيع غير مسموحة لك.', [], 403);
            }
            $posId = (int) $data['point_of_sale_id'];
            DB::table('employee_sessions')->where('id', $session->id)
                ->update(['active_pos_id' => $posId]);
        }

        try {
            $res = $this->cashbox->startShift([
                'agent_id'         => $employee->agent_id,
                'employee_id'      => $employee->id,
                'point_of_sale_id' => $posId,
                'opening_cash'     => $data['opening_cash'],
                'device_hash'      => $session->device_hash,
            ]);
        } catch (\InvalidArgumentException $e) {
            return $this->sendError($e->getMessage(), [], 422);
        }

        return $this->sendResponse(
            $res,
            $res['already_open'] ? 'لديك وردية مفتوحة سلفاً.' : 'بدأت الوردية.'
        );
    }

    /** POST employee/shift/close — يتطلّب CLOSE_SHIFT */
    public function closeShift(Request $request)
    {
        [$employee, , ] = $this->ctx($request);

        $data = $request->validate([
            'actual_cash' => 'required|numeric|min:0',
            'notes'       => 'nullable|string|max:500',
        ]);

        $shift = $this->cashbox->openShift((int) $employee->id);
        if (!$shift) {
            return $this->sendError('لا توجد وردية مفتوحة.', [], 422);
        }

        try {
            $res = $this->cashbox->closeShift(
                (int) $shift->id, (float) $data['actual_cash'],
                (int) $employee->id, $data['notes'] ?? null
            );
        } catch (\InvalidArgumentException $e) {
            return $this->sendError($e->getMessage(), [], 422);
        }

        return $this->sendResponse($res, 'أُقفلت الوردية — ' . $res['label']);
    }
}
