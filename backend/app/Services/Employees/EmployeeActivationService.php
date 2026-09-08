<?php

namespace App\Services\Employees;

use App\Services\Watsaoserversfrom;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Str;

/**
 * تفعيل الموظف — الطبقات الثلاث التي لا تُتجاوَز.
 *
 *     رقم الهاتف + كود التفعيل   ⇦ تصريح الإدارة
 *   + رمز تحقّق من 4 أرقام        ⇦ ملكية الرقم
 *   + ربط الجهاز                  ⇦ الجهاز المعتمد
 *
 * ولا يُرسَل الرمز إلا بعد نجاح الأولى كاملةً (بند 9): إرساله قبلها يحوّل
 * النظام إلى أداة إزعاج لأي رقم يُخمَّن.
 *
 * ── قرارات أمنية مقصودة ────────────────────────────────────────────────
 * • الكود والرمز يُخزَّنان **مُجزّأين** (`Hash::make`)، ولا يُسترجعان.
 *   الكود يُعرض للوكيل مرّة واحدة لحظة الإصدار.
 * • رسائل الفشل **موحّدة**: «الرقم أو الكود غير صحيح» لا تفرّق بين موظفٍ
 *   غير موجود وكودٍ خاطئ — التفريق يكشف أي الأرقام مسجّل.
 *   والاستثناء الوحيد حالة `COMPROMISED`، لأن الموظف الحقيقي يجب أن يعرف
 *   لماذا توقّف كوده وأن عليه مراجعة الإدارة.
 * • استعمال كودٍ مربوطٍ بجهاز على **جهاز ثانٍ** لا يُرفض فقط: يُعطَّل الكود
 *   ويصير `COMPROMISED` ولا يعود يعمل حتى على الجهاز الأصلي (بند 15).
 *   ذلك مؤلم عمداً — لأن البديل أن يظلّ كودٌ مسرَّب صالحاً.
 */
class EmployeeActivationService
{
    /**
     * مدّة صلاحية كود التفعيل — أمرُ المالك (8 سبتمبر 2026).
     *
     * ⚠ **لا كودَ مفتوح الصلاحية.** كان يُصدَر بلا نهاية (`expires_at`
     * فارغة ولا تُقرأ)، فيبقى صالحاً شهوراً في ورقةٍ أو رسالة — وكودٌ
     * منسيٌّ صالحٌ هو مفتاحُ حسابٍ متروكٌ في الطريق.
     *
     * وعشرُ دقائق كافيةٌ لما وُضع له: الوكيل يُصدره والموظف بين يديه أو
     * على الهاتف. ومن تأخّر يطلب غيره — وإصدارُ كودٍ ثانٍ أرخص من كودٍ
     * أوّلَ لا يموت.
     */
    private const CODE_TTL_MINUTES = 10;

    /**
     * طولُ رمز QR بالبايت قبل الترميز — 32 بايتاً = 256 بت.
     *
     * ⚠ عشوائيةٌ قويّة لا كودٌ مقروء: كود التفعيل اليدويّ ثمانُ خانات من
     * أبجديةٍ مقروءة لأنه يُملى صوتاً، وذلك سقفٌ على عشوائيّته يقبله لأنه
     * محميٌّ بحدّ المحاولات وبمدّةٍ قصيرة. أمّا رمزُ QR فلا يُملى ولا يُكتب،
     * فلا سببَ لإضعافه — ويُولَّد من `random_bytes` لا من `rand`.
     */
    private const QR_TOKEN_BYTES = 32;

    /** مدّة صلاحية رمز التحقّق. */
    private const OTP_TTL_MINUTES = 3;

    /** محاولات إدخال الرمز قبل إلغاء المحاولة كلّها. */
    private const OTP_MAX_ATTEMPTS = 5;

    /** محاولات كود التفعيل الخاطئة قبل إلغائه. */
    private const CODE_MAX_ATTEMPTS = 5;

    /** أقصى عدد طلبات رمز لرقمٍ واحد في النافذة. */
    private const OTP_RATE_WINDOW_MINUTES = 10;
    private const OTP_RATE_MAX = 5;

    public function __construct(
        private DeviceRegistryService $devices,
        private EmployeeAuditLogger $log,
    ) {
    }

    /* ===================================================================
       إصدار كود التفعيل — من تطبيق الوكيل
       =================================================================== */

    /**
     * يُصدر كوداً جديداً ويُلغي كل كودٍ سابق للموظف.
     *
     * يُعيد الكود **نصّاً صريحاً مرّة واحدة**؛ لا يُخزَّن كذلك ولا يُقرأ ثانيةً.
     *
     * @return array{code:string, expires_at:?string}
     */
    public function issueCode(int $agentId, int $employeeId, int $issuedBy, array $trace): array
    {
        $employee = DB::table('employees')
            ->where('id', $employeeId)
            ->where('agent_id', $agentId)
            ->whereNull('deleted_at')
            ->first();

        if (!$employee) {
            throw new \InvalidArgumentException('الموظف غير موجود.');
        }

        // 8 خانات من أبجدية بلا أحرفٍ متشابهة (0/O، 1/I/L): الكود يُقرأ
        // بالصوت على الهاتف غالباً، وخلطُ حرفٍ برقم يُفشل التفعيل بلا سبب.
        $code = $this->readableCode(8);

        // يُعرض مرّةً واحدة كالكود، ولا يُخزَّن إلا مُجزَّأً.
        $qrToken = bin2hex(random_bytes(self::QR_TOKEN_BYTES));

        return DB::transaction(function () use ($agentId, $employeeId, $issuedBy, $code, $qrToken, $employee, $trace) {
            // كودٌ فعّال واحد لكل موظف — الفهرس الفريد يحرس هذا، والإلغاء هنا
            // يجعل «إصدار كود جديد» يعني حتماً إبطال القديم.
            DB::table('employee_activation_codes')
                ->where('employee_id', $employeeId)
                ->where('status', 'ACTIVE')
                ->update([
                    'status'         => 'REVOKED',
                    'revoked_at'     => now(),
                    'revoked_reason' => 'أُصدر كود جديد',
                ]);

            $id = DB::table('employee_activation_codes')->insertGetId([
                'agent_id'    => $agentId,
                'employee_id' => $employeeId,
                'phone'       => $employee->phone,
                'code_hash'   => Hash::make($code),
                'code_hint'   => substr($code, -2),
                'status'      => 'ACTIVE',
                'issued_by'   => $issuedBy,
                'issued_at'   => now(),
                // ⚠ لا كودَ مفتوحَ الصّلاحية — انظر `CODE_TTL_MINUTES`.
                'expires_at'  => now()->addMinutes(self::CODE_TTL_MINUTES),

                /*
                 * ⚠ رمزُ QR والكودُ اليدويّ **تمثيلان لطلبٍ واحد**، لا
                 * طلبان. فهما على الصفّ نفسِه: حالةٌ واحدة، ومدّةٌ واحدة،
                 * وإلغاءٌ واحد، واستهلاكٌ واحد.
                 *
                 * وجدولٌ ثانٍ كان سيعني كوداً مُلغى ورمزاً ما زال يعمل.
                 */
                'qr_token_hash' => hash('sha256', $qrToken),
                'qr_issued_at'  => now(),
            ]);

            // الموظف الموقوف أمنياً يعود «بانتظار التفعيل» بكودٍ جديد —
            // وهذا هو طريق العودة الوحيد بعد COMPROMISED.
            DB::table('employees')->where('id', $employeeId)->update([
                'status'     => 'PENDING_ACTIVATION',
                'updated_at' => now(),
            ]);

            $this->log->audit('EMPLOYEE_CODE_ISSUED', [
                'agent_id'    => $agentId,
                'employee_id' => $employeeId,
                'entity_type' => 'employee_activation_code',
                'entity_id'   => (string) $id,
            ] + $trace);

            return [
                'code'       => $code,
                // ⚠ يُعاد نصّاً هنا فقط — ولا يُقرأ من القاعدة ثانيةً.
                'qr_token'   => $qrToken,
                // يُعرَض للوكيل: كودٌ بلا موعدٍ يُملى على مهل.
                'expires_at' => now()->addMinutes(self::CODE_TTL_MINUTES)->toIso8601String(),
                'ttl_minutes' => self::CODE_TTL_MINUTES,
            ];
        });
    }

    /** أبجدية بلا 0/O و1/I/L — الكود يُملى صوتاً. */
    private function readableCode(int $len): string
    {
        $alphabet = '23456789ABCDEFGHJKMNPQRSTUVWXYZ';
        $out = '';
        for ($i = 0; $i < $len; $i++) {
            $out .= $alphabet[random_int(0, strlen($alphabet) - 1)];
        }
        return $out;
    }

    /* ===================================================================
       الخطوة 1 — التحقّق الأوّلي ثم إرسال الرمز
       =================================================================== */

    /**
     * تفعيلٌ بمسح QR — **يدخل هذا المسار نفسَه** لا مساراً ثانياً.
     *
     * ⚠ نصُّ البند 21: «ويجب ألا يكون هناك منطقان مختلفان للتفعيل». فكلُّ
     * ما يفعله هذا هو **ترجمةُ الرمز إلى (رقمٍ + صفِّ طلب)**، ثمّ تسليمُ
     * الأمر إلى `requestOtp` — فتجري عليه الفحوصُ نفسُها: الحالة، والمدّة،
     * والجهاز الثاني، وحدُّ المعدّل، وحالةُ الموظف. ثمّ `verifyOtp` نفسُها
     * بلا حرفٍ يتغيّر.
     *
     * ⚠ والرمزُ **ليس مصدرَ ثقة**: لا يحمل معرّفَ موظّفٍ ولا وكيلٍ ولا
     * صلاحيات. هو مفتاحٌ معتِمٌ يُقاد به إلى صفٍّ في القاعدة، والصفُّ وحده
     * يقول لمن هو ولأي وكيل — فلا يستطيع أحدٌ تعديلَه لينتقل إلى وكيلٍ آخر.
     */
    public function requestOtpByQr(string $qrToken, string $deviceId, array $trace): array
    {
        $deviceHash = DeviceRegistryService::hash($deviceId);
        if ($deviceHash === null) {
            return ['ok' => false, 'message' => 'تعذّر التعرّف على الجهاز.'];
        }

        $qrToken = trim($qrToken);

        /*
         * شكلُ الرمز يُفحص قبل لمس القاعدة: نصٌّ ستّ عشريّ بطولٍ معلوم.
         * ورمزٌ لا يطابق الشكل لا حاجة لسؤال القاعدة عنه.
         */
        if (!preg_match('/^[0-9a-f]{' . (self::QR_TOKEN_BYTES * 2) . '}$/', $qrToken)) {
            return ['ok' => false, 'message' => 'رمز التفعيل غير صالح.'];
        }

        $row = DB::table('employee_activation_codes')
            ->where('qr_token_hash', hash('sha256', $qrToken))
            ->first(['id', 'phone', 'status', 'scan_device_hash', 'employee_id', 'agent_id']);

        if (!$row) {
            $this->log->security('QR_UNKNOWN', 'رمز تفعيل غير معروف', [
                'device_hash' => $deviceHash,
            ] + $trace);

            return ['ok' => false, 'message' => 'رمز التفعيل غير صالح.'];
        }

        /*
         * ⚠ حسمُ التزامن هنا، قبل إرسال أي رمز تحقّق — البند 10: «رفض
         * محاولة استخدامه بالتزامن من جهازين».
         *
         * والحارسُ **تحديثٌ شرطيّ في القاعدة** لا فحصٌ في الذاكرة: جهازان
         * يمسحان في اللحظة نفسِها يقرآن معاً `scan_device_hash = NULL`،
         * فيمرّ كلاهما على أي `if`. أمّا التحديثُ فيصيب صفّاً واحداً،
         * والثاني يعود بصفرٍ فيُرفض.
         *
         * ولا يُحرق الرمز هنا: الجهازُ نفسُه قد يعيد المسح بعد انقطاعٍ في
         * الشبكة، وحرقُ الرمز على انقطاعٍ عابر يُجبر الوكيل على إصدار غيره
         * بلا سبب. فالشرطُ «لم يمسحه أحدٌ بعد، أو مسحه هذا الجهاز نفسُه».
         */
        $claimed = DB::table('employee_activation_codes')
            ->where('id', $row->id)
            ->where('status', 'ACTIVE')
            ->where(function ($w) use ($deviceHash) {
                $w->whereNull('scan_device_hash')
                  ->orWhere('scan_device_hash', $deviceHash);
            })
            ->update([
                'scanned_at'       => now(),
                'scan_device_hash' => $deviceHash,
            ]);

        if ($claimed === 0) {
            /*
             * إمّا أن الرمز ليس `ACTIVE` — وحينها تتكفّل `requestOtp`
             * بالرسالة الصحيحة (منتهٍ، مُلغى، مُستعمَل) — أو أن جهازاً آخر
             * سبق إليه، وتلك حالةٌ أمنيةٌ تُسجَّل وتُرفض صراحةً.
             */
            if ($row->status === 'ACTIVE'
                && $row->scan_device_hash !== null
                && $row->scan_device_hash !== $deviceHash) {

                /*
                 * ⚠ ويُنسب الحدثُ إلى صاحبه. حدثٌ أمنيٌّ بلا موظّفٍ ولا
                 * رقمٍ يبقى صفّاً يتيماً: لا يظهر في سجلّ الموظف حين
                 * يُسأل عنه، ولا يُحذف مع بياناته حين تُحذف.
                 *
                 * ولا يُكتب الرمزُ نفسُه ولا جزءٌ منه — البند 24.
                 */
                $this->log->security('QR_OTHER_DEVICE',
                    'محاولة مسح رمز تفعيلٍ مسحه جهازٌ آخر', [
                        'device_hash' => $deviceHash,
                        'phone'       => $row->phone,
                        'employee_id' => $row->employee_id,
                        'agent_id'    => $row->agent_id,
                    ] + $trace);

                return ['ok' => false, 'message' => 'هذا الرمز يُستخدم على جهاز آخر. اطلب رمزاً جديداً.'];
            }
        }

        /*
         * ⚠ والكودُ لا يُقرأ من القاعدة ولا يُقارَن: `requestOtp` تحتاج
         * إثباتَ ملكية الطلب، وقد أُثبت بالرمز نفسِه. فتُستدعى بعلامةٍ
         * تقول «تُخطَّ مقارنةُ الكود وحدها» — وما دونها من فحوصٍ يجري كلُّه.
         */
        return $this->requestOtp($row->phone, '', $deviceId, $trace, true, $row->id);
    }

    /**
     * يتحقّق من الرقم والكود والجهاز، وعند النجاح **فقط** يُرسل الرمز.
     *
     * @param bool $viaQr مرّ التحقّقُ من الملكية عبر رمز QR، فلا يُقارَن
     *                    الكودُ المكتوب. وما عداه من فحوصٍ يجري كما هو.
     *
     * @return array{ok:bool, message:string, masked_phone?:string, activation_id?:int}
     */
    public function requestOtp(
        string $phone,
        string $code,
        string $deviceId,
        array $trace,
        bool $viaQr = false,
        ?int $pinnedCodeId = null,
    ): array {
        $deviceHash = DeviceRegistryService::hash($deviceId);
        if ($deviceHash === null) {
            return ['ok' => false, 'message' => 'تعذّر التعرّف على الجهاز.'];
        }

        $phone = $this->normalizePhone($phone);

        // حدّ المعدّل قبل أي عمل: يمنع تجريب الأكواد ويمنع إغراق رقمٍ برسائل.
        if ($this->rateLimited($phone)) {
            $this->log->security('RATE_LIMIT', 'تجاوز حدّ طلبات رمز التحقّق', [
                'phone' => $phone, 'device_hash' => $deviceHash,
            ] + $trace);
            return ['ok' => false, 'message' => 'محاولات كثيرة. أعد المحاولة بعد قليل.'];
        }

        $record = DB::table('employee_activation_codes as c')
            ->join('employees as e', 'e.id', '=', 'c.employee_id')
            ->where('c.phone', $phone)
            ->whereIn('c.status', ['ACTIVE', 'USED', 'COMPROMISED'])
            /*
             * ⚠ مسارُ QR يُثبّت الصفَّ الذي يقود إليه الرمزُ بعينِه.
             *
             * فبدونه يُقاد التحقّقُ بالرقم وحده ثمّ يأخذ **أحدثَ** صفّ،
             * فيصير رمزٌ ألغاه الوكيلُ أمسِ صالحاً لأن للموظف صفّاً
             * أحدثَ منه — أي أن الإلغاء لا يُلغي. والفرقُ يظهر في QR
             * وحدَه لأنه لا يُقارَن بكود: الكودُ اليدويّ يحرسه
             * `Hash::check` على الصفّ الذي وُجد.
             */
            ->when($pinnedCodeId !== null, fn ($q) => $q->where('c.id', $pinnedCodeId))
            ->whereNull('e.deleted_at')
            ->orderByDesc('c.id')
            ->select([
                'c.id as code_id', 'c.code_hash', 'c.status as code_status',
                'c.attempts', 'c.bound_device_hash', 'c.employee_id', 'c.agent_id',
                'c.expires_at',
                'e.status as employee_status', 'e.full_name',
            ])
            ->first();

        // رسالة موحّدة: لا تكشف أي الأرقام مسجّل.
        $generic = ['ok' => false, 'message' => 'رقم الهاتف أو كود التفعيل غير صحيح.'];

        if (!$record) {
            $this->log->security('CODE_FAILED', 'لا يوجد كود لهذا الرقم', [
                'phone' => $phone, 'device_hash' => $deviceHash,
            ] + $trace);
            return $generic;
        }

        /*
         * ⚠ كودٌ مضى عليه أكثر من `CODE_TTL_MINUTES` **يُحرق** ولا يُرفض
         * فحسب — أمرُ المالك (8 سبتمبر 2026): «يُمنع ترك صلاحية المفتاح
         * مفتوحة».
         *
         * والحرقُ في القاعدة لا الرفضُ في الذاكرة: كودٌ يُرفض اليوم ويبقى
         * `ACTIVE` في الجدول هو كودٌ حيٌّ ينتظر ساعةً يُصلَح فيها الخطأ.
         *
         * ⚠ ويُطبَّق على `ACTIVE` وحدها: `USED` كودٌ أدّى عمله وربط جهازاً،
         * وإعادةُ التفعيل على الجهاز نفسِه طريقٌ مشروع — فلا معنى لانتهاء
         * صلاحيةِ ما استُعمل.
         *
         * والرسالةُ صريحة هنا خلافاً لقاعدة الرسائل الموحّدة: من انتهى
         * كودُه يحتاج أن يعرف أنه انتهى ليطلب غيره، لا أن يظنّ رقمَه خاطئاً
         * فيعيد المحاولة عشراً. وهي لا تكشف شيئاً: من بلغ هذه النقطة قد
         * أثبت أنه يملك كوداً صحيحاً لهذا الرقم.
         */
        if ($record->code_status === 'ACTIVE'
            && $record->expires_at !== null
            && now()->greaterThan($record->expires_at)) {

            DB::table('employee_activation_codes')
                ->where('id', $record->code_id)
                ->where('status', 'ACTIVE')
                ->update([
                    'status'         => 'EXPIRED',
                    'revoked_at'     => now(),
                    'revoked_reason' => 'انتهت المدّة قبل الاستعمال',
                ]);

            $this->log->security('CODE_EXPIRED', 'كود تفعيل انتهت مدّته قبل استعماله', [
                'phone'       => $phone,
                'device_hash' => $deviceHash,
                'employee_id' => $record->employee_id,
                'agent_id'    => $record->agent_id,
            ] + $trace);

            return [
                'ok' => false,
                'message' => 'انتهت مدّة كود التفعيل. اطلب من الإدارة كوداً جديداً.',
            ];
        }

        if ($record->code_status === 'COMPROMISED') {
            return [
                'ok' => false,
                'message' => 'تم إيقاف كود التفعيل لأسباب أمنية، ولتفعيل التطبيق مرة أخرى '
                    . 'يرجى التواصل مع الإدارة لاستلام كود جديد',
            ];
        }

        if (!$viaQr && !Hash::check($code, $record->code_hash)) {
            $this->bumpCodeAttempts($record);
            $this->log->security('CODE_FAILED', 'كود خاطئ', [
                'phone' => $phone, 'device_hash' => $deviceHash,
                'employee_id' => $record->employee_id, 'agent_id' => $record->agent_id,
            ] + $trace);
            return $generic;
        }

        if (!in_array($record->employee_status, ['PENDING_ACTIVATION', 'ACTIVE', 'REQUIRES_REACTIVATION'], true)) {
            return ['ok' => false, 'message' => 'حساب الموظف غير مفعّل. راجع الإدارة.'];
        }

        // ── خرق «جهاز ثانٍ» (بند 15) ──────────────────────────────────
        // الكود مربوط بجهاز، والمحاولة من غيره: لا يُرفض الطلب فحسب — يُحرق
        // الكود. البديل أن يبقى كودٌ مسرَّب صالحاً على الجهاز الأصلي.
        if ($record->code_status === 'USED'
            && $record->bound_device_hash !== null
            && $record->bound_device_hash !== $deviceHash) {
            $this->markCompromised($record, $deviceHash, $trace);
            return [
                'ok' => false,
                'message' => 'تم إيقاف كود التفعيل لأسباب أمنية، ولتفعيل التطبيق مرة أخرى '
                    . 'يرجى التواصل مع الإدارة لاستلام كود جديد',
            ];
        }

        // كودٌ مستهلك على الجهاز نفسه ⇦ إعادة تفعيل مشروعة، يُسمح بها.

        $otp = (string) random_int(1000, 9999);   // 4 أرقام كما في دخول الوكيل

        DB::transaction(function () use ($record, $phone, $deviceHash, $otp) {
            // رمزٌ جديد يُلغي السابق (بند 11).
            DB::table('employee_otps')
                ->where('employee_id', $record->employee_id)
                ->where('status', 'PENDING')
                ->update(['status' => 'CANCELLED']);

            DB::table('employee_otps')->insert([
                'employee_id'   => $record->employee_id,
                'activation_id' => $record->code_id,
                'phone'         => $phone,
                'device_hash'   => $deviceHash,
                'otp_hash'      => Hash::make($otp),
                'status'        => 'PENDING',
                'max_attempts'  => self::OTP_MAX_ATTEMPTS,
                'created_at'    => now(),
                'expires_at'    => now()->addMinutes(self::OTP_TTL_MINUTES),
            ]);
        });

        $this->sendOtp($phone, $otp);

        return [
            'ok'            => true,
            'message'       => 'أُرسل رمز التحقّق.',
            'masked_phone'  => $this->maskPhone($phone),
            'activation_id' => (int) $record->code_id,
            // يُعرَض في شاشة المسح: من مسح رمزاً يطمئنّ أنّه رمزُه هو،
            // والاسمُ من القاعدة لا من الرمز — الرمزُ لا يحمل شيئاً.
            'employee_name' => $record->full_name,
        ];
    }

    private function bumpCodeAttempts(object $record): void
    {
        $attempts = ((int) $record->attempts) + 1;
        $update = ['attempts' => $attempts];

        if ($attempts >= self::CODE_MAX_ATTEMPTS && $record->code_status === 'ACTIVE') {
            $update += [
                'status'         => 'REVOKED',
                'revoked_at'     => now(),
                'revoked_reason' => 'تجاوز حدّ المحاولات الخاطئة',
            ];
        }

        DB::table('employee_activation_codes')->where('id', $record->code_id)->update($update);
    }

    /** حرق الكود وتعطيل الموظف أمنياً + إشعار الوكيل. */
    private function markCompromised(object $record, string $attemptDeviceHash, array $trace): void
    {
        DB::transaction(function () use ($record) {
            DB::table('employee_activation_codes')->where('id', $record->code_id)->update([
                'status'         => 'COMPROMISED',
                'revoked_at'     => now(),
                'revoked_reason' => 'محاولة استخدام على جهاز آخر',
            ]);

            DB::table('employees')->where('id', $record->employee_id)->update([
                'status'     => 'COMPROMISED',
                'updated_at' => now(),
            ]);

            // كل جلسة وكل جهاز تشغيلي يسقطان فوراً.
            DB::table('employee_sessions')
                ->where('employee_id', $record->employee_id)
                ->where('status', 'ACTIVE')
                ->update(['status' => 'REVOKED', 'revoked_at' => now(),
                          'revoked_reason' => 'تفعيل مُخترَق']);

            DB::table('employee_devices')
                ->where('employee_id', $record->employee_id)
                ->where('status', 'ACTIVE')
                ->update(['status' => 'REVOKED', 'revoked_at' => now(),
                          'revoked_reason' => 'تفعيل مُخترَق']);
        });

        $this->log->security('CODE_OTHER_DEVICE', 'محاولة استخدام كود التفعيل على جهاز آخر', [
            'agent_id'    => $record->agent_id,
            'employee_id' => $record->employee_id,
            'device_hash' => $attemptDeviceHash,
            'severity'    => 'CRITICAL',
        ] + $trace);

        $this->log->audit('EMPLOYEE_COMPROMISED', [
            'agent_id'    => $record->agent_id,
            'employee_id' => $record->employee_id,
            'entity_type' => 'employee',
            'entity_id'   => (string) $record->employee_id,
        ] + $trace);

        $this->notifyAgent($record->agent_id, $record->employee_id);
    }

    /**
     * إشعار الوكيل بحادث أمني عبر القناة نفسها (بند 49).
     *
     * الفشل لا يُوقف شيئاً: الحادث مُسجَّل في القاعدة، والرسالة تنبيهٌ إضافي.
     */
    private function notifyAgent(int $agentId, int $employeeId): void
    {
        try {
            $agent = DB::table('users')->where('id', $agentId)->first(['phone']);
            $emp   = DB::table('employees')->where('id', $employeeId)->first(['full_name']);
            if (!$agent || empty($agent->phone)) {
                return;
            }

            $text = "تنبيه أمني — تطبيق الوكيل\n"
                . 'جرت محاولة استخدام كود تفعيل الموظف «' . ($emp->full_name ?? '') . '» على جهاز آخر.'
                . "\nأُوقف الكود، ويحتاج الموظف كوداً جديداً منك.";

            (new Watsaoserversfrom())->sendFormasggme($this->waNumber($agent->phone), $text);
        } catch (\Throwable $e) {
            // متروك عمداً.
        }
    }

    /* ===================================================================
       الخطوة 2 — التحقّق من الرمز وإتمام التفعيل
       =================================================================== */

    /**
     * @return array{ok:bool, message:string, session?:array}
     */
    /**
     * @param int|null $activationId معرّفُ الطلب، لمن جاء من مسح QR.
     *
     * ⚠ مسارُ QR لا يعرف رقمَ الهاتف — ولا ينبغي أن يعرفه: ما يعود إليه من
     * الخادم رقمٌ **مقنَّع**، فمن صوّر رمزاً لا يخرج منه برقم موظّفٍ كامل.
     * فيُقاد إلى صفّه بمعرّف الطلب، ويُشتقّ الرقمُ من الصفّ.
     *
     * وذلك لا يُضعف شيئاً: الصفُّ مربوطٌ بـ`device_hash` كما هو، فمعرّفُ
     * الطلب وحده لا يفتح شيئاً على جهازٍ آخر — ويبقى رمزُ التحقّق الواصلُ
     * إلى هاتف الموظف هو الحارسَ الأخير في الحالتين.
     */
    public function verifyOtp(
        string $phone,
        string $otp,
        string $deviceId,
        array $trace,
        ?int $activationId = null,
    ): array {
        $deviceHash = DeviceRegistryService::hash($deviceId);
        if ($deviceHash === null) {
            return ['ok' => false, 'message' => 'تعذّر التعرّف على الجهاز.'];
        }

        $phone = $this->normalizePhone($phone);

        $row = DB::table('employee_otps')
            ->where('device_hash', $deviceHash)   // رمزُ جهازٍ لا يُستعمل من غيره
            ->where('status', 'PENDING')
            ->when(
                $activationId !== null,
                fn ($q) => $q->where('activation_id', $activationId),
                fn ($q) => $q->where('phone', $phone),
            )
            ->orderByDesc('id')
            ->first();

        // والرقمُ من الصفّ لا من الطلب: ما بعده يعمل على رقمٍ واحد.
        if ($row && $activationId !== null) {
            $phone = $row->phone;
        }

        if (!$row) {
            return ['ok' => false, 'message' => 'انتهت صلاحية رمز التحقق'];
        }

        if (now()->greaterThan($row->expires_at)) {
            DB::table('employee_otps')->where('id', $row->id)->update(['status' => 'EXPIRED']);
            return ['ok' => false, 'message' => 'انتهت صلاحية رمز التحقق'];
        }

        if (!Hash::check($otp, $row->otp_hash)) {
            $attempts = ((int) $row->attempts) + 1;
            $exhausted = $attempts >= (int) $row->max_attempts;

            DB::table('employee_otps')->where('id', $row->id)->update([
                'attempts' => $attempts,
                'status'   => $exhausted ? 'CANCELLED' : 'PENDING',
            ]);

            $this->log->security('OTP_FAILED', 'رمز تحقّق خاطئ', [
                'phone' => $phone, 'device_hash' => $deviceHash,
                'employee_id' => $row->employee_id,
            ] + $trace);

            return [
                'ok' => false,
                'message' => $exhausted
                    ? 'تجاوزت عدد المحاولات. اطلب رمزاً جديداً.'
                    : 'رمز التحقق غير صحيح',
            ];
        }

        return DB::transaction(function () use ($row, $deviceHash, $trace, $phone) {
            DB::table('employee_otps')->where('id', $row->id)
                ->update(['status' => 'USED', 'used_at' => now()]);

            $employee = DB::table('employees')->where('id', $row->employee_id)->first();

            // الكود يصير مستهلكاً ومربوطاً بهذا الجهاز — منه يُعرف الخرق لاحقاً.
            if ($row->activation_id) {
                DB::table('employee_activation_codes')
                    ->where('id', $row->activation_id)
                    ->update([
                        'status'            => 'USED',
                        'used_at'           => now(),
                        'bound_device_hash' => $deviceHash,
                        'attempts'          => 0,
                    ]);
            }

            // جهاز فعّال واحد: ما سبق يُستبدل لا يُترك.
            DB::table('employee_devices')
                ->where('employee_id', $employee->id)
                ->where('status', 'ACTIVE')
                ->update(['status' => 'REPLACED', 'revoked_at' => now(),
                          'revoked_reason' => 'تفعيل جهاز جديد']);

            $deviceRowId = DB::table('employee_devices')->insertGetId([
                'agent_id'      => $employee->agent_id,
                'employee_id'   => $employee->id,
                'activation_id' => $row->activation_id,
                'device_hash'   => $deviceHash,
                'platform'      => $trace['platform'] ?? null,
                'model'         => $trace['model'] ?? null,
                'app_version'   => $trace['app_version'] ?? null,
                'status'        => 'ACTIVE',
                'activated_at'  => now(),
                'last_activity_at' => now(),
            ]);

            // ⚠ الوسم الدائم — بعد النجاح لا قبله.
            $this->devices->remember(
                $deviceHash,
                DeviceRegistryService::EMPLOYEE_DEVICE,
                (int) $employee->agent_id,
                (int) $employee->id,
                $trace['platform'] ?? null,
                $trace['model'] ?? null
            );

            $primaryPos = DB::table('employee_point_of_sales')
                ->where('employee_id', $employee->id)
                ->where('is_active', 1)
                ->orderByDesc('is_primary')
                ->value('point_of_sale_id');

            $accessToken  = Str::random(64);
            $refreshToken = Str::random(64);

            $sessionId = DB::table('employee_sessions')->insertGetId([
                'agent_id'           => $employee->agent_id,
                'employee_id'        => $employee->id,
                'device_id'          => $deviceRowId,
                'device_hash'        => $deviceHash,
                'active_pos_id'      => $primaryPos,
                'access_token_hash'  => hash('sha256', $accessToken),
                'refresh_token_hash' => hash('sha256', $refreshToken),
                'status'             => 'ACTIVE',
                'created_at'         => now(),
                'last_used_at'       => now(),
                'ip_address'         => $trace['ip'] ?? null,
            ]);

            DB::table('employees')->where('id', $employee->id)->update([
                'status'           => 'ACTIVE',
                'activated_at'     => $employee->activated_at ?? now(),
                'last_login_at'    => now(),
                'last_activity_at' => now(),
                'updated_at'       => now(),
            ]);

            $this->log->audit('EMPLOYEE_ACTIVATED', [
                'agent_id'    => $employee->agent_id,
                'employee_id' => $employee->id,
                'device_hash' => $deviceHash,
                'entity_type' => 'employee',
                'entity_id'   => (string) $employee->id,
            ] + $trace);

            return [
                'ok'      => true,
                'message' => 'تم التفعيل بنجاح.',
                'session' => [
                    'access_token'  => $accessToken,
                    'refresh_token' => $refreshToken,
                    'session_id'    => $sessionId,
                    'employee'      => [
                        'id'   => (int) $employee->id,
                        'name' => $employee->full_name,
                        'phone' => $phone,
                        'point_of_sale_id' => $primaryPos,
                    ],
                ],
            ];
        });
    }

    /* ===================================================================
       الخروج — إنهاء صريح للتفعيل (بند 16)
       =================================================================== */

    public function logout(int $employeeId, array $trace): void
    {
        DB::transaction(function () use ($employeeId, $trace) {
            DB::table('employee_sessions')
                ->where('employee_id', $employeeId)
                ->where('status', 'ACTIVE')
                ->update(['status' => 'REVOKED', 'revoked_at' => now(),
                          'revoked_reason' => 'تسجيل خروج']);

            // الكود المستهلك لا يعود يعمل — العودة تحتاج كوداً جديداً.
            DB::table('employee_activation_codes')
                ->where('employee_id', $employeeId)
                ->whereIn('status', ['ACTIVE', 'USED'])
                ->update(['status' => 'REVOKED', 'revoked_at' => now(),
                          'revoked_reason' => 'تسجيل خروج الموظف']);

            DB::table('employee_devices')
                ->where('employee_id', $employeeId)
                ->where('status', 'ACTIVE')
                ->update(['status' => 'REVOKED', 'revoked_at' => now(),
                          'revoked_reason' => 'تسجيل خروج الموظف']);

            DB::table('employees')->where('id', $employeeId)->update([
                'status'     => 'REQUIRES_REACTIVATION',
                'updated_at' => now(),
            ]);

            $this->log->audit('EMPLOYEE_LOGOUT', [
                'employee_id' => $employeeId,
                'entity_type' => 'employee',
                'entity_id'   => (string) $employeeId,
            ] + $trace);
        });
    }

    /* ===================================================================
       أدوات
       =================================================================== */

    private function rateLimited(string $phone): bool
    {
        return DB::table('employee_otps')
            ->where('phone', $phone)
            ->where('created_at', '>=', now()->subMinutes(self::OTP_RATE_WINDOW_MINUTES))
            ->count() >= self::OTP_RATE_MAX;
    }

    /** 9 خانات تبدأ بـ 9 — نفس صيغة `Fmt.phoneForApi` في التطبيق. */
    private function normalizePhone(string $phone): string
    {
        $digits = preg_replace('/[^0-9]/', '', $phone);
        if (str_starts_with($digits, '218')) {
            $digits = substr($digits, 3);
        }
        return ltrim($digits, '0');
    }

    private function waNumber(string $phone): string
    {
        return '218' . $this->normalizePhone($phone);
    }

    /** +218 92 *** 3709 — يطمئن الموظف أن الرمز ذهب إلى رقمه بلا كشفه. */
    private function maskPhone(string $phone): string
    {
        if (strlen($phone) < 9) {
            return $phone;
        }
        return '+218 ' . substr($phone, 0, 2) . ' *** ' . substr($phone, -4);
    }

    /**
     * القناة نفسها المعتمدة في دخول الوكيل — واتساب (بند 10).
     *
     * وجهة التطوير تُحترم كما في `OtpController`: رقمٌ تجريبي ليبي قد لا يكون
     * على واتساب أصلاً، فيفشل التفعيل لسببٍ لا علاقة له بالكود.
     */
    private function sendOtp(string $phone, string $otp): void
    {
        /*
         * ⚠ إلى رقم الموظّف وحده، ولا وجهةَ بديلةً من الإعدادات.
         *
         * كان يقرأ `OTP_DEV_TO` فيرسل إليه كلّ رمزٍ أيّاً كان
         * صاحبُه، وقيمتُه في هذا الجهاز كانت رقمَ الوكيل. وليس
         * ذلك إزعاجاً في التجربة، بل نقضٌ للنظام نفسِه:
         *
         * الرمزُ إثباتُ **ملكيةِ الرقم** — وهي الطبقةُ الثانية
         * بعد كود الوكيل. فإن وصل إلى الوكيل صار الوكيلُ قادراً
         * على تفعيل حساب موظّفِه على جهازِه هو — ومعَه تسقط
         * نسبةُ من سلّم أيّ حوالة، وهي أصلُ ما بُني له نظامُ
         * الموظّفين.
         *
         * ومن أراد تحويلاً للتجربة فموضِعُه دخولُ الوكيل: هناك
         * المرسِل والمستقبِل واحد، وهنا اثنان.
         */
        $to = $this->waNumber($phone);
        try {
            $res = (new Watsaoserversfrom())
                ->sendFormasggme($to, 'رمز التحقق الخاص بك هو: ' . $otp);

            /*
             * ⚠ البوّابة **تُرجِع** الفشل ولا ترميه: `['success' => false]`.
             *
             * فـ`catch` وحده لا يرى شيئاً، وفشلُ الإرسال كان يظهر
             * للموظّف رمزاً لا يصل **بلا أي أثرٍ في أي سجلّ** —
             * فيصير تشخيصُه تخميناً: رقمٌ غير مسجّل؟ بوّابةٌ معطّلة؟
             * واتسابٌ غير مفعّلٍ على الرقم؟
             */
            if (is_array($res) && ($res['success'] ?? null) === false) {
                // ⚠ السّبب في `detail` لا في السّياق: `security_logs` لا
                // تحمل عموداً له، فما يُمرّر في السّياق يُسقَط
                // بصمت — وهو عينُ ما نُصلِحُه هنا.
                $this->log->security('OTP_SEND_FAILED',
                    'تعذّر إرسال رمز التحقّق إلى رقم الموظّف — '
                        . mb_substr((string) ($res['message'] ?? '?'), 0, 200), [
                        'phone' => $phone,
                    ]);
            }
        } catch (\Throwable $e) {
            $this->log->security('OTP_SEND_FAILED',
                'استثناء أثناء إرسال رمز التحقّق — '
                    . mb_substr($e->getMessage(), 0, 200), [
                    'phone' => $phone,
                ]);
        }

        // ⚠ ولا يُكتب الرمزُ نفسُه في أي سجلّ — الفشلُ يُسجّل لا محتواه.
    }
}
