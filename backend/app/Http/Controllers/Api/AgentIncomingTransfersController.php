<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\AgentIncomingTransfersService;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Log;

/**
 * الحوالات الواردة للوكيل — متابعة تسليم، لا حركة مالية.
 *
 * الوكيل يُشتقّ من التوثيق دائماً، ولا يُقرأ من جسم الطلب: `agent_id` مرسَلاً
 * من الهاتف يعني أن تعديل الطلب يدوياً يفتح حوالات وكيل آخر. العزل هنا
 * وفي الاستعلام معاً، لا في واجهة التطبيق.
 */
class AgentIncomingTransfersController extends BaseController
{
    public function __construct(private AgentIncomingTransfersService $service)
    {
    }

    /** GET /api/agent/incoming-transfers?status=&search=&page=&per_page= */
    public function index(Request $request)
    {
        $user = Auth::user();

        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }
        if (empty($user->BrancchID)) {
            return $this->sendError('لا يوجد فرع مرتبط بهذا المستخدم.', [], 403);
        }

        $status = $request->query('status');
        if ($status !== null && !in_array($status, [
            AgentIncomingTransfersService::PENDING,
            AgentIncomingTransfersService::DELIVERED,
            AgentIncomingTransfersService::CANCELLED_TAB,
        ], true)) {
            return $this->sendError('حالة غير معروفة.', [], 422);
        }

        // المزامنة قبل القراءة: كل حوالة معتمدة جديدة تصير صفّاً محفوظاً،
        // فتبقى بعد ذلك ولو غادرت الـ View بتغيّر حالتها في المنظومة.
        try {
            $this->service->syncFromCore(
                (int) $user->id,
                (int) $user->BrancchID,
                (int) $user->UeserType
            );
        } catch (\Throwable $e) {
            // فشل المزامنة لا يمنع عرض المحفوظ: الوكيل يرى ما لديه ولو
            // انقطع الاتصال بالمنظومة.
            Log::warning('agent incoming sync failed', [
                'user'  => $user->id,
                'error' => $e->getMessage(),
            ]);
        }

        $perPage = (int) $request->query('per_page', 20);
        $perPage = max(1, min($perPage, 100));

        $result = $this->service->list(
            (int) $user->id,
            $status,
            $request->query('search'),
            max(1, (int) $request->query('page', 1)),
            $perPage
        );

        $result['counts'] = $this->service->counts((int) $user->id);

        return $this->sendResponse($result, 'Success');
    }

    /** POST /api/agent/incoming-transfers/{id}/deliver */
    public function deliver(Request $request, int $id)
    {
        $user = Auth::user();

        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $result = $this->service->markDelivered(
            (int) $user->id,
            $id,
            (int) $user->id,
            [
                'ip'      => $request->ip(),
                'device'  => $request->header('X-Device-Id'),
                'session' => $request->header('X-Session-Id'),
            ]
        );

        if ($result['row'] === null) {
            // لا نفرّق بين «غير موجودة» و«تخصّ وكيلاً آخر»: التفريق يكشف
            // وجود حوالة لمن لا يملكها.
            return $this->sendError('الحوالة غير موجودة.', [], 404);
        }

        if (!empty($result['cancelled'])) {
            return $this->sendError(
                'هذه الحوالة ملغاة في المنظومة — لا يجوز تسجيل تسليمها.',
                ['core_status' => $result['row']->core_status_label],
                409
            );
        }

        return $this->sendResponse([
            'transfer' => $result['row'],
            'changed'  => $result['changed'],
            'counts'   => $this->service->counts((int) $user->id),
        ], $result['changed'] ? 'تم تسجيل التسليم.' : 'الحوالة مسجّلة كمسلَّمة سلفاً.');
    }

    /**
     * GET /api/agent/outgoing-transfers/{code} — فاتورة حوالةٍ أرسلها الوكيل.
     *
     * قراءةٌ محضة: لا تكتب حرفاً، ولا تمسّ رصيداً ولا قيداً.
     *
     * **العزل يقوم على كشف حساب الوكيل نفسه**، لا على عمودٍ في `InternalEx`:
     * لا يُعاد صفٌّ إلا إن كان رقمه ظاهراً في حركات حساب هذا الوكيل
     * (`EX24AccSafeActivityTb.accidfrom = users.AccID`). وهذا أدقّ حارسٍ
     * متاح — أن يكون للحوالة أثرٌ مالي على حسابه هو معنى «حوالتي» حرفياً،
     * ولا يعتمد على أعمدة `AccFrom` التي رُصد أنها تُكتب صفراً في مسارات
     * كثيرة (انظر CLAUDE.md).
     *
     * ولا تُفرّق الرسالة بين «غير موجودة» و«ليست لك»، كما في التسليم أعلاه.
     */
    public function outgoingByCode(Request $request, string $code)
    {
        $user = Auth::user();

        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $code = trim($code);
        if ($code === '') {
            return $this->sendError('رقم الحوالة مطلوب.', [], 422);
        }

        if (!$this->ownsOutgoing($user, $code)) {
            return $this->sendError('الحوالة غير موجودة.', [], 404);
        }

        $row = \Illuminate\Support\Facades\DB::table('InternalEx as t')
            ->leftJoin('InternalEx_Stautes as s', 's.ConfirmType', '=', 't.ConfirmType')
            ->leftJoin('BBranchTb as b', 'b.BranchID', '=', 't.BranchDeliveredID')
            // مدينة الاستلام من `DeliveryPlace` لا من الفرع (قرار المالك،
            // 4 سبتمبر 2026): الفرع يُسنَد عند الاعتماد ويكون صفراً قبله،
            // بينما المدينة يختارها الوكيل لحظة الإنشاء فتوجد دائماً.
            // تحقّقتُ على كل الصفوف: `DeliveryPlace` مُعرِّف في `CitiesTb`.
            ->leftJoin('CitiesTb as ct', 'ct.ID', '=', 't.DeliveryPlace')
            ->where('t.Code', $code)
            ->selectRaw("t.Code, t.InsertDate, t.SenderName, t.SPhone1,
                t.RecievedName, t.RPhone1, t.OverallVal, t.ExVal,
                t.ConfirmType, s.SName AS StatusName,
                b.BranchName AS DeliveredBranchName,
                ct.CityName AS DeliveryCityName,
                -- سبب الإلغاء من موضعه في المنظومة — نفس ترتيب المصادر
                -- المستعمل في قائمة الواردة، فلا يفترق ما يُعرض هنا عمّا
                -- يُعرض هناك.
                COALESCE(
                    (SELECT TOP 1 r.NewCause
                       FROM TransCancelRequestTb tc
                       LEFT JOIN AddCancelReason r ON r.ID = tc.ReasonID
                      WHERE tc.ISID = t.Code ORDER BY tc.ID DESC),
                    (SELECT TOP 1 r2.NewCause FROM AddCancelReason r2
                      WHERE r2.ID = t.AddCancelReason_ID),
                    NULLIF(LTRIM(RTRIM(t.AddCancelReason_NameFrom_Driver)), '')
                ) AS cancel_reason,
                (SELECT TOP 1 tc.Notes FROM TransCancelRequestTb tc
                  WHERE tc.ISID = t.Code ORDER BY tc.ID DESC) AS cancel_notes")
            ->first();

        if (!$row) {
            return $this->sendError('الحوالة غير موجودة.', [], 404);
        }

        return $this->sendResponse($row, 'تم جلب الحوالة.');
    }

    /**
     * هل هذه الحوالة حوالةُ هذا الوكيل؟ — بثلاثة أدلّة، أيّها كفى.
     *
     * 1. **أثرٌ في كشف حسابه** — أقوى دليل، لكنه لا يوجد قبل الاعتماد: القيد
     *    لا يُكتب إلا عند اعتماد الحوالة (رُصد: حوالة بحالة 0 لها صفر حركات).
     * 2. **أنشأها من التطبيق** — `uesrID_forminsertmobile` يحمل رقم المستخدم.
     * 3. **حسابُه هو حساب الإرسال** — `AccFrom` مع `IsAccFrom = 1`، وهو ما
     *    يكتبه مسار سطح المكتب.
     *
     * والثلاثة لازمة: الأول وحده يُخفي غير المعتمدة، والثاني وحده يُخفي ما
     * أنشأه الفرع لحسابه، والثالث وحده لا يُعتمد عليه لأن `AccFrom` تُكتب
     * صفراً في مسارات كثيرة.
     */
    private function ownsOutgoing($user, string $code): bool
    {
        $db = \Illuminate\Support\Facades\DB::class;

        $inStatement = \Illuminate\Support\Facades\DB::table('EX24AccSafeActivityTb')
            ->where('ISID', $code)
            ->whereIn('accidfrom', function ($q) use ($user) {
                $q->select('AccID')->from('users')->where('id', $user->id);
            })
            ->exists();

        if ($inStatement) {
            return true;
        }

        return \Illuminate\Support\Facades\DB::table('InternalEx')
            ->where('Code', $code)
            ->where(function ($w) use ($user) {
                $w->where('uesrID_forminsertmobile', $user->id);
                if (!empty($user->AccID)) {
                    $w->orWhere(function ($x) use ($user) {
                        $x->where('IsAccFrom', 1)->where('AccFrom', $user->AccID);
                    });
                }
            })
            ->exists();
    }

    /**
     * GET /api/agent/outgoing-transfers/pending — حوالاتٌ أرسلها ولم تُعتمد.
     *
     * سببها أن الوكيل كان **لا يرى حوالته بعد إنشائها إطلاقاً**: تبويب
     * «صادرة» مبنيّ على كشف الحساب، والقيد المحاسبي لا يُكتب إلا عند
     * الاعتماد. فيُدرج حوالة ثم لا يجد لها أثراً — وهو أسوأ ما يمكن أن
     * يشعر به من ائتمنك على ماله.
     *
     * قراءةٌ محضة من `InternalEx`، ولا تكتب شيئاً.
     *
     * والصفوف تُشكَّل **بمفاتيح كشف الحساب نفسها**، فيقرأها التطبيق بنموذج
     * `Movement` القائم ويعرضها ببطاقته القائمة — لا نموذج ثانٍ ولا بطاقة
     * ثانية تفترق عن أختها عند أول تعديل.
     */
    public function pendingOutgoing(Request $request)
    {
        $user = Auth::user();

        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }

        $rows = \Illuminate\Support\Facades\DB::table('InternalEx as t')
            ->leftJoin('InternalEx_Stautes as s', 's.ConfirmType', '=', 't.ConfirmType')
            // غير المعتمدة وحدها: ما اعتُمد له قيدٌ في كشف الحساب، وإدراجه هنا
            // يعرضه مرّتين في القائمة نفسها.
            ->where('t.ConfirmType', 0)
            ->where(function ($w) use ($user) {
                $w->where('t.uesrID_forminsertmobile', $user->id);
                if (!empty($user->AccID)) {
                    $w->orWhere(function ($x) use ($user) {
                        $x->where('t.IsAccFrom', 1)->where('t.AccFrom', $user->AccID);
                    });
                }
            })
            ->orderByDesc('t.ID')
            ->limit(100)
            ->selectRaw("
                'حوالة داخلية'                       AS MovementType,
                'خصم'                                AS Type_from,
                t.OverallVal                         AS Values_to,
                0                                    AS Balnce,
                t.InsertDate                         AS InsertDate,
                t.Code                               AS Code,
                t.InsertDate                         AS TransTime,
                s.SName                              AS DeliveryStatus,
                t.ConfirmType                        AS CoreConfirmType,
                0                                    AS IsCommission,
                ISNULL(t.ExVal, 0)                   AS CommissionAmount
            ")
            ->get();

        return $this->sendResponse($rows, 'Success');
    }
}
