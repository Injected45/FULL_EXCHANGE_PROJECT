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
}
