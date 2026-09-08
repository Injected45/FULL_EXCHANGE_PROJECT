<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\BaseController;
use App\Services\Employees\EmployeeApprovalExecutor;
use App\Services\Employees\EmployeeApprovals;
use App\Services\Employees\EmployeeAuditLogger;
use App\Services\Employees\EmployeeLimitPolicy;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

/**
 * طلباتُ موافقة الوكيل على تجاوز سقف موظفيه — واجهةُ **الوكيل**.
 *
 * ══════════════════════════════════════════════════════════════════════════
 *  فصلُ المسؤوليات، بنيوياً لا بشرط `if`
 * ══════════════════════════════════════════════════════════════════════════
 *
 * ⚠ هذا المتحكّم كلُّه خلف `auth:sanctum` — أي خلف **جلسة الوكيل**. وجلسةُ
 * الموظف نظامٌ آخر تماماً (`employee_sessions` خلف وسيطٍ آخر) ولا تُنتج رمز
 * Sanctum أصلاً.
 *
 * فالموظف لا يستطيع اعتمادَ طلبه بنفسه (البند 11) **لأنه لا يملك مفتاحَ
 * الباب**، لا لأن شرطاً في الشيفرة يمنعه. وهذا أقوى: الشروطُ تُنسى عند
 * إضافة مسارٍ جديد، والأبوابُ المنفصلة لا تُنسى.
 *
 * ⚠ وعزلُ الوكلاء (البند 31) في **شرط الاستعلام نفسِه** لا في فحصٍ سابق:
 * كلُّ قراءةٍ وكلُّ تحديثٍ هنا مقيَّدٌ بـ`agent_id` للمستخدم الحاليّ، فوكيلٌ
 * آخر لا يرى الطلبَ ولا يستطيع تغييره مهما أرسل من معرّفات.
 */
class EmployeeApprovalController extends BaseController
{
    public function __construct(
        private readonly EmployeeApprovals $approvals,
        private readonly EmployeeApprovalExecutor $executor,
        private readonly EmployeeAuditLogger $log,
    ) {
    }

    /**
     * الحسابُ الرئيسيّ وحده يقرّر.
     *
     * ⚠ ولا جدولَ صلاحياتٍ جديد: النظامُ يميّز `AccountType = 'Main'` عن
     * نقاط البيع منذ نشأته، وهو التمييزُ نفسُه الذي تعتمده هويّةُ الشركة.
     * وجدولٌ ثانٍ يعبّر عن التمييز نفسِه هو موضعٌ ثانٍ يختلف عن الأوّل يوماً.
     *
     * ونقطةُ البيع تقرأ ولا تقرّر: هي تشغيلٌ لا إدارة.
     */
    private function agent(Request $r): array
    {
        $user = $r->user();

        if (!$user) {
            return [null, $this->sendError('غير مصرّح.', [], 401)];
        }

        return [$user, null];
    }

    private function canDecide($user): bool
    {
        return ($user->AccountType ?? '') === 'Main';
    }

    /**
     * GET employees/approvals — قائمةُ الطلبات، بتصفيةٍ وبحث.
     */
    public function index(Request $request)
    {
        [$user, $err] = $this->agent($request);
        if ($err) return $err;

        /*
         * ⚠ يُنهى المنتهي عند القراءة — البند 27.
         *
         * ولا مُجدوِلَ زمنيّ: مشروعٌ بلا عاملِ طوابير لا يملك واحداً يعمل
         * دائماً، ومهمّةٌ مجدولةٌ متوقّفة تعني طلباً يُعرض «بانتظار الموافقة»
         * بعد أسبوعٍ من انتهائه — فيوافق عليه الوكيل ظانّاً أنه حيّ.
         */
        $this->approvals->expireDue((int) $user->id);

        $status = strtoupper(trim((string) $request->query('status', '')));
        $employeeId = (int) $request->query('employee_id', 0);
        $posId = (int) $request->query('point_of_sale_id', 0);
        $reason = strtoupper(trim((string) $request->query('reason', '')));
        $search = trim((string) $request->query('q', ''));
        $from = trim((string) $request->query('from', ''));
        $to = trim((string) $request->query('to', ''));
        $limit = min(100, max(1, (int) $request->query('limit', 50)));

        $q = DB::table('employee_approval_requests')
            ->where('agent_id', $user->id);

        if ($status !== '' && $status !== 'ALL') {
            $q->where('status', $status);
        }
        if ($employeeId > 0) {
            $q->where('employee_id', $employeeId);
        }
        if ($posId > 0) {
            $q->where('point_of_sale_id', $posId);
        }
        if ($reason !== '') {
            $q->where('reasons', 'like', '%' . $reason . '%');
        }
        if ($from !== '') {
            $q->where('created_at', '>=', $from);
        }
        if ($to !== '') {
            $q->where('created_at', '<=', $to . ' 23:59:59');
        }
        if ($search !== '') {
            $q->where(function ($w) use ($search) {
                $w->where('recipient_name', 'like', '%' . $search . '%')
                  ->orWhere('recipient_phone', 'like', '%' . $search . '%')
                  ->orWhere('employee_name_snap', 'like', '%' . $search . '%')
                  ->orWhere('transfer_number', 'like', '%' . $search . '%');
            });
        }

        $rows = $q->orderByDesc('id')->limit($limit)->get();

        return $this->sendResponse([
            'requests' => $rows->map(fn ($r) => EmployeeApprovals::present($r))->all(),
            'can_decide' => $this->canDecide($user),
        ], 'تم');
    }

    /**
     * GET employees/approvals/count — للشارة (البند 39).
     *
     * ⚠ مستقلٌّ عن `index` عمداً: الشارةُ تُسأل كلَّ ربع دقيقة، وحملُ
     * `index` صفوفٌ كاملة بأسبابها ولقطاتها. سؤالٌ يتكرّر يجب أن يكون
     * أرخصَ ما يمكن — عددٌ واحد.
     */
    public function count(Request $request)
    {
        [$user, $err] = $this->agent($request);
        if ($err) return $err;

        $this->approvals->expireDue((int) $user->id);

        $pending = DB::table('employee_approval_requests')
            ->where('agent_id', $user->id)
            ->where('status', 'PENDING')
            ->count();

        $newest = DB::table('employee_approval_requests')
            ->where('agent_id', $user->id)
            ->where('status', 'PENDING')
            ->max('id');

        return $this->sendResponse([
            'pending'   => $pending,
            'newest_id' => $newest !== null ? (int) $newest : null,
        ], 'تم');
    }

    /** POST employees/approvals/{id}/approve */
    public function approve(Request $request, int $id)
    {
        return $this->decide($request, $id, true);
    }

    /** POST employees/approvals/{id}/reject */
    public function reject(Request $request, int $id)
    {
        return $this->decide($request, $id, false);
    }

    private function decide(Request $request, int $id, bool $approve)
    {
        [$user, $err] = $this->agent($request);
        if ($err) return $err;

        if (!$this->canDecide($user)) {
            $this->log->security('UNAUTHORIZED',
                'محاولةُ البتّ في طلب موافقة من حسابٍ غير رئيسيّ', [
                    'agent_id' => (int) $user->id,
                    'ip'       => $request->ip(),
                ]);

            return $this->sendError(
                'الموافقة على تجاوز سقف الموظف من الحساب الرئيسي فقط.', [], 403);
        }

        $note = trim((string) $request->input('note', ''));

        $result = $this->approvals->decide(
            $id,
            (int) $user->id,
            (int) $user->id,
            $approve,
            $note !== '' ? mb_substr($note, 0, 500) : null,
        );

        $req = $result['request'];

        /*
         * ⚠ طلبٌ ليس لهذا الوكيل يُردّ 404 لا 403 — فلا يُعلَم بوجوده أصلاً.
         * والفرقُ بين الردّين يكشف أن الطلب موجودٌ عند غيره.
         */
        if (!$req || (int) $req->agent_id !== (int) $user->id) {
            return $this->sendError('طلب الموافقة غير موجود.', [], 404);
        }

        /*
         * ⚠ ولم يتغيّر شيء ⇐ قرارٌ سبق. والردُّ **حالةُ الطلب الحالية** لا
         * رسالةُ خطأ (البند 26): الوكيل ضغط مرّتين ونجحت واحدة، فالشاشةُ
         * يجب أن تريه النتيجة لا فشلاً.
         */
        if (!$result['changed']) {
            return $this->sendResponse([
                'already'  => true,
                'request'  => EmployeeApprovals::present($req),
            ], 'اتُّخذ القرار في هذا الطلب من قبل.');
        }

        if (!$approve) {
            return $this->sendResponse([
                'request' => EmployeeApprovals::present($this->approvals->find($id)),
            ], 'رُفض الطلب، ولم تُنفَّذ الحوالة.');
        }

        /*
         * ⚠ الموافقةُ تُثبَّت أوّلاً ثم يُنفَّذ — بهذا الترتيب.
         *
         * فالتحديثُ الذرّيّ هو ما يمنع تنفيذين متزامنين (البند 25): من أصاب
         * الصفَّ يملك حقّ التنفيذ وحدَه، ومن أصاب صفراً يقرأ الحالة ولا
         * ينفّذ. ولو نُفِّذ قبل التثبيت لمرّ الطلبان معاً.
         */
        $exec = $this->executor->execute($this->approvals->find($id));

        return $this->sendResponse([
            'request'         => EmployeeApprovals::present($this->approvals->find($id)),
            'executed'        => $exec['ok'],
            'transfer_number' => $exec['transfer_number'],
        ], $exec['message']);
    }

    /**
     * GET/PUT employees/{id}/limits — سقفُ الموظف.
     */
    public function showLimits(Request $request, int $id)
    {
        [$user, $err] = $this->agent($request);
        if ($err) return $err;

        $employee = DB::table('employees')
            ->where('id', $id)
            ->where('agent_id', $user->id)
            ->whereNull('deleted_at')
            ->first(['id', 'full_name']);

        if (!$employee) {
            return $this->sendError('الموظف غير موجود.', [], 404);
        }

        $policy = app(EmployeeLimitPolicy::class)->forEmployee($id);

        return $this->sendResponse([
            'employee_id'        => $id,
            'per_transfer_limit' => $policy->per_transfer_limit !== null
                ? (float) $policy->per_transfer_limit : null,
            'cumulative_limit'   => $policy->cumulative_limit !== null
                ? (float) $policy->cumulative_limit : null,
            'cumulative_hours'   => $policy->cumulative_hours,
            'recipient_minutes'  => $policy->recipient_minutes,
            'approval_ttl_hours' => $policy->approval_ttl_hours,
            'can_edit'           => $this->canDecide($user),
        ], 'تم');
    }

    public function updateLimits(Request $request, int $id)
    {
        [$user, $err] = $this->agent($request);
        if ($err) return $err;

        if (!$this->canDecide($user)) {
            return $this->sendError(
                'تعديل سقف الموظف من الحساب الرئيسي فقط.', [], 403);
        }

        $employee = DB::table('employees')
            ->where('id', $id)
            ->where('agent_id', $user->id)
            ->whereNull('deleted_at')
            ->first(['id']);

        if (!$employee) {
            return $this->sendError('الموظف غير موجود.', [], 404);
        }

        $data = $request->validate([
            // ⚠ `nullable` مقصود: NULL = بلا سقف، وهو حالةُ كلّ موظفٍ اليوم.
            //   والصفرُ ليس بديلاً عنه — صفرٌ يعني «لا يُسمح بشيء».
            'per_transfer_limit' => 'nullable|numeric|min:0',
            'cumulative_limit'   => 'nullable|numeric|min:0',
            'cumulative_hours'   => 'nullable|integer|min:1|max:8760',
            'recipient_minutes'  => 'nullable|integer|min:0|max:10080',
            'approval_ttl_hours' => 'nullable|integer|min:1|max:720',
        ]);

        /*
         * ⚠ سقفٌ تراكميٌّ بلا نافذة لا معنى له — «لا يتجاوز 10,000» في أي
         * مدّة؟ اليوم؟ العمر كلّه؟ فيُرفض الطلبُ بدل أن يُخترع له جواب.
         */
        if (($data['cumulative_limit'] ?? null) !== null
            && ($data['cumulative_hours'] ?? null) === null) {
            return $this->sendError(
                'حدِّد فترة احتساب السقف التراكمي.', [], 422);
        }

        $now = now();

        $values = [
            'agent_id'           => (int) $user->id,
            'per_transfer_limit' => $data['per_transfer_limit'] ?? null,
            'cumulative_limit'   => $data['cumulative_limit'] ?? null,
            'cumulative_hours'   => $data['cumulative_hours'] ?? null,
            'recipient_minutes'  => $data['recipient_minutes']
                ?? EmployeeLimitPolicy::DEFAULT_RECIPIENT_MINUTES,
            'approval_ttl_hours' => $data['approval_ttl_hours']
                ?? EmployeeLimitPolicy::DEFAULT_APPROVAL_TTL_HOURS,
            'updated_by'         => (int) $user->id,
            'updated_at'         => $now,
        ];

        $exists = DB::table('employee_transfer_policies')
            ->where('employee_id', $id)->exists();

        if ($exists) {
            DB::table('employee_transfer_policies')
                ->where('employee_id', $id)->update($values);
        } else {
            DB::table('employee_transfer_policies')->insert(
                $values + ['employee_id' => $id, 'created_at' => $now]);
        }

        /*
         * ⚠ ولا يُمَسّ طلبٌ معلّق (البند 43): لقطةُ السياسة محفوظةٌ في الطلب
         * نفسِه، فسببُ تصعيده يبقى مكتوباً بسقف يومِه. ورفعُ السقف اليوم لا
         * يعتمد طلبَ الأمس تلقائياً — الاعتمادُ قرارٌ يُضغط، لا أثرٌ جانبيّ.
         */
        $this->log->audit('EMPLOYEE_LIMITS_UPDATED', [
            'agent_id'    => (int) $user->id,
            'employee_id' => $id,
            'entity_type' => 'employee_transfer_policy',
            'entity_id'   => (string) $id,
        ]);

        return $this->sendResponse(['updated' => true], 'حُفظ سقف التحويل.');
    }
}
