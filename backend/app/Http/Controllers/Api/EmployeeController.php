<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\AgentIncomingTransfersService;
use App\Services\Employees\EmployeeAuditLogger;
use App\Services\Employees\EmployeeActsAsAgent;
use App\Services\Employees\EmployeeApprovals;
use App\Services\Employees\EmployeeLimitPolicy;
use App\Services\Employees\EmployeeTransferViews;
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

        return $this->sendResponse([
            'employee' => [
                'id'    => (int) $employee->id,
                'name'  => $employee->full_name,
                'phone' => $employee->phone,
            ],
            'active_point_of_sale_id' => $session->active_pos_id,
            'points_of_sale'          => $pos,
            'permissions'             => $permissions,
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
        [$employee, , ] = $this->ctx($request);

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

        $perPage = max(1, min((int) $request->query('per_page', 20), 100));

        $result = $this->transfers->list(
            (int) $employee->agent_id, $status, $request->query('search'),
            max(1, (int) $request->query('page', 1)), $perPage
        );
        $result['counts'] = $this->transfers->counts((int) $employee->agent_id);

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

        $result = $this->transfers->markDelivered(
            (int) $employee->agent_id, $id, (int) $employee->agent_id,
            ['ip' => $request->ip(), 'device' => $session->device_hash]
        );

        if ($result['row'] === null) {
            return $this->sendError('الحوالة غير موجودة.', [], 404);
        }
        // اختفى أصلها من المنظومة — كالحارس نفسه في مسار الوكيل.
        if (!empty($result['missing'])) {
            return $this->sendError('هذه الحوالة لم تعد موجودة في المنظومة.', [], 404);
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
            $shift = $this->cashbox->openShift((int) $employee->id);
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

            $this->log->audit('TRANSFER_DELIVERED', [
                'entity_type' => 'transfer',
                'entity_id'   => $row->transfer_number,
                'point_of_sale_id' => $posId,
                'new_value'   => ['amount' => $row->amount],
            ] + $trace);
        }

        return $this->sendResponse([
            'transfer' => $result['row'],
            'changed'  => $result['changed'],
            'counts'   => $this->transfers->counts((int) $employee->agent_id),
        ], $result['changed'] ? 'تم تسجيل التسليم.' : 'الحوالة مسجّلة كمسلَّمة سلفاً.');
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

        $shift = $this->cashbox->openShift((int) $employee->id);
        if (!$shift) {
            return $this->sendError('ابدأ وردية أولاً لتسجيل حركة.', [], 422);
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
