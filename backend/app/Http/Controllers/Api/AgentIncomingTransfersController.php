<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\AgentIncomingTransfersService;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\DB;
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

    /**
     * GET /api/agent/incoming-transfers/alerts
     *
     * أرقام صفوف الوارد وحدها — لجرس التنبيه في أعلى الشاشة الرئيسية.
     *
     * منفصلة عن `index` لأن الجرس يسأل كل دقيقة تقريباً وهو سؤال واحد:
     * «هل وصل شيء جديد؟». وحمولة `index` صفوفٌ كاملة بأسماء ومبالغ
     * وسببِ إلغاء واستعلاماتٍ فرعية لكلٍّ منها — تحميلُ ذلك في دورةٍ
     * متكرّرة إهدارٌ لشبكة الوكيل ولوقت الخادم معاً.
     *
     * والتطبيق يحسب «غير المفتوحة» بنفسه: الخادم لا يعرف ما فتحه الوكيل،
     * وتسجيلُ ذلك في الخادم يعني جدولاً جديداً وكتابةً عند كل فتح فاتورة —
     * ثمنٌ لا يشتري شيئاً، فالجرس شأن الجهاز الذي بين يديه.
     */
    public function alerts()
    {
        $user = Auth::user();

        if (!$user) {
            return $this->sendError('غير مصرّح.', [], 401);
        }
        if (empty($user->BrancchID)) {
            return $this->sendError('لا يوجد فرع مرتبط بهذا المستخدم.', [], 403);
        }

        // المزامنة أولاً، وإلا لم يرنّ الجرس إلا بعد أن يفتح الوكيل شاشةً
        // أخرى تزامن — أي بعد أن يكون قد رأى الحوالة، وهو نقيض الغرض.
        try {
            $this->service->syncFromCore(
                (int) $user->id,
                (int) $user->BrancchID,
                (int) $user->UeserType
            );
        } catch (\Throwable $e) {
            // كما في `index`: فشل المزامنة لا يُسقط الطلب. الجرس يبقى على
            // آخر ما يعرفه بدل أن يعرض عطباً في أعلى الشاشة الرئيسية.
            Log::warning('agent incoming alerts sync failed', [
                'user'  => $user->id,
                'error' => $e->getMessage(),
            ]);
        }

        // **«بانتظار التسليم» وحدها** (أمر المالك، 5 سبتمبر 2026).
        //
        // الجرس ينبّه إلى عملٍ لم يُنجَز، لا إلى تاريخ الحساب: المسلَّمة
        // انتهت، والملغاة لا تُسلَّم — وعدُّهما يجعل الرقم لا يهبط أبداً
        // فيفقد معناه.
        //
        // والشرط هو شرط `counts()` نفسه حرفياً، لا شرطاً شبيهاً به: تبويب
        // «بانتظار التسليم» في الشاشة والرقمُ على الجرس يجب أن يعدّا الشيء
        // نفسه، وتعريفان متقاربان يفترقان عند أول تغيير في أحدهما.
        //
        // 200 سقفٌ لا ترشيح: الجرس يعني الجديد، والأقدم من مئتَي حوالة
        // ليس جديداً بحال. وبلا سقف تكبر الحمولة بلا حدّ مع عمر الحساب.
        /*
         * ⚠⚠ **البوّابة السيادية على الجرس أيضاً.**
         *
         * أمرُ المالك: «ولا يصل إليه أيُّ إشعارٍ أو رسالة إلّا بعد أن
         * تُعتمد». والجرسُ يرنّ ويُنزل شريطاً — فرنّةٌ لحوالةٍ غيرِ معتمدة
         * إفشاءٌ لوجودها، ولو لم تُعرض في أيّ قائمة.
         */
        $ids = AgentIncomingTransfersService::onlyApproved(
                DB::table('agent_incoming_transfers')
                    ->where('agent_id', (int) $user->id)
            )
            ->where('status', AgentIncomingTransfersService::PENDING)
            ->where(function ($w) {
                $w->whereNull('core_confirm_type')
                  ->orWhereNotIn(
                      'core_confirm_type',
                      AgentIncomingTransfersService::CORE_CANCELLED
                  );
            })
            ->orderByDesc('id')
            ->limit(200)
            ->pluck('id')
            ->map(fn ($v) => (int) $v)
            ->values();

        return $this->sendResponse(['ids' => $ids], 'Success');
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

        // اختفى أصلها من المنظومة بعد أن فُتحت الشاشة.
        //
        // 404 لا 409: الرسالة تصف ما يراه الوكيل — الحوالة لم تعد موجودة —
        // بينما 409 تعني «موجودة وحالتها تمنع». والتطبيق يعامل 404 على أنها
        // «ليست هنا» فيعود بالوكيل إلى القائمة بدل أن يبقيه أمام صفٍّ ميت.
        if (!empty($result['missing'])) {
            return $this->sendError(
                'هذه الحوالة لم تعد موجودة في المنظومة.',
                [],
                404
            );
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

        // ⚠ الاستعلامُ نفسُه انتقل إلى `AgentIncomingTransfersService` ليقرأ
        // منه بابُ الموظف كذلك — نسخةٌ ثانية منه كانت ستفترق عن هذه عند أوّل
        // تصحيح، فتُقرأ للحوالة الواحدة فاتورتان مختلفتان. الفحصُ أعلاه
        // (`ownsOutgoing`) يبقى هنا: ملكيةُ الوكيل غيرُ ملكيةِ الموظف.
        $row = $this->service->outgoingRowByCode($code);

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
