<?php

namespace App\Services\Employees;

use App\Models\User;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\DB;

/**
 * الموظف ينفّذ باسم وكيله — أمرُ المالك (7 سبتمبر 2026).
 *
 * ── نصُّ القرار ──────────────────────────────────────────────────────────
 *
 * «سجلُّها الماليّ كما الوكيل، وليس أي سجلاتٍ جديدة. الموظف ينفّذ الحوالة
 * وكأنه الوكيل — عبارة عن واجهةٍ من وكيل، وليس مستقلاً استقلاليةً تامّة.»
 *
 * ── وكيف يُنفَّذ ذلك حرفياً ──────────────────────────────────────────────
 *
 * ⚠ **لا تُكتب هنا سطرُ منطقٍ ماليٍّ واحد.** المسارُ الماليّ هو مسارُ الوكيل
 * نفسُه — الدالّةُ ذاتُها في `depositController` — تُنادى بهويّة الوكيل.
 * فما يُكتب في `InternalEx` يخرج من الشيفرة التي تكتبه للوكيل، حرفاً بحرف:
 * توليدُ الكود، وحدُّ الثلاث دقائق، والعمولة، وحدودُ التحويل، وفحصُ الرصيد.
 *
 * وهذا ليس اختصاراً بل هو **الشرط**: نسخةٌ ثانية من منطق الحوالة تفترق عن
 * الأصل عند أوّل تعديلٍ يُجرى على أحدهما، فتُكتب حوالتان بقاعدتين مختلفتين
 * في دفترٍ واحد. ومن يقرأ الدفتر بعدها لا يعرف أيَّهما الصحيح.
 *
 * ⚠ **ولا جدولَ جديداً ولا عموداً جديداً في أي جدولٍ ماليّ.** الصفُّ الذي
 * يُكتب لا يُميَّز عن صفّ الوكيل بشيء — لأنه صفُّ الوكيل.
 *
 * ⚠ **ومن نفّذ فعلاً يُسجَّل في `transfer_attributions` وحدها** — وهي جدولٌ
 * تشغيليّ **بجوار** الدفتر لا داخله، وهو الموضع الذي تُسجَّل فيه عمليةُ
 * التسليم منذ البداية. فالسؤالان مفصولان: «ما الذي جرى مالياً؟» يجيبه
 * الدفتر، و«من حرّك يده؟» يجيبه هذا الجدول. وخلطُهما هو ما يُفسد الدفتر.
 */
class EmployeeActsAsAgent
{
    /**
     * يُنفّذ `$work` بهويّة الوكيل، ثم يُعيد الحارس إلى ما كان.
     *
     * ⚠ الاستعادةُ في `finally`: استثناءٌ في منتصف العمل كان سيترك الطلبَ
     * كلَّه يظنّ أن الوكيل هو المستخدم الحالي — وأي شيءٍ يُقرأ بعده في
     * الطلب نفسِه يُقرأ بهويّةٍ خاطئة.
     *
     * @template T
     * @param  callable():T  $work
     * @return T
     */
    public function as(int $agentUserId, callable $work)
    {
        $agent = User::find($agentUserId);

        if (!$agent) {
            throw new \RuntimeException('حساب الوكيل غير موجود.');
        }

        $previous = Auth::user();

        try {
            Auth::setUser($agent);

            return $work($agent);
        } finally {
            $previous ? Auth::setUser($previous) : Auth::forgetUser();
        }
    }

    /**
     * قراءةُ حجزٍ سابق بالمفتاح نفسِه — بلا كتابة.
     *
     * ⚠ تُسأل **قبل** أي فحصٍ آخر: الضغطةُ المكرّرة تقع بعد ثوانٍ من
     * الأولى، فلو سبقتها قاعدةُ الدقيقة لَقيل للموظف «انتظر دقيقة» عن
     * حوالةٍ **نجحت للتوّ** — فيظنّها لم تقع ويعيدها ثالثةً.
     *
     * والحارسُ من السباق يبقى الفهرسَ الفريد في [claim]، لا هذه القراءة.
     */
    public function findClaim(object $employee, string $clientId): ?object
    {
        return DB::table('employee_transfer_claims')
            ->where('employee_id', $employee->id)
            ->where('client_id', mb_substr($clientId, 0, 64))
            ->first(['transfer_number', 'status']);
    }

    /**
     * هل سيرفض المحفّزُ هذه الحوالة لقاعدة الدقيقة؟
     *
     * ── العطب الذي تُعالجه ───────────────────────────────────────────────
     *
     * ⚠ في المنظومة قاعدتان لمهلة الدقيقة، لا واحدة:
     *
     *   • وحدةُ التحكّم تسأل: آخرُ حوالةٍ بـ`MAX(ID)`، والشرط `< 1`.
     *   • والمحفّزُ يسأل:     آخرُ حوالةٍ بـ`MAX(IDCode)`، والشرط `<= 1`.
     *
     * فعند الدقيقة الواحدة بالضبط **تسمح الأولى ويُلغي الثاني**: يُدرَج
     * الصفّ، ثمّ يُلغي المحفّزُ المعاملة، فيعجز السطرُ التالي في وحدة
     * التحكّم عن إيجاد ما أدرجه ويرمي «لم يتم العثور على السجل بعد
     * الإدخال في InternalEx» — خطأُ 500 لا يفهمه أحد، وسببُه مهلةٌ عادية.
     *
     * وقد وقع فعلاً في اختبارٍ متتابع.
     *
     * ⚠ **ولا يُغيَّر هنا منطقٌ ماليّ ولا عتبة ولا محفّز**: هذا فحصٌ
     * **للقراءة فقط** يتنبّأ برفض المحفّز بقاعدته هو حرفياً، ليقول للموظف
     * «انتظر دقيقة» بدل خطأٍ غامض. والقرارُ الماليّ يبقى للمحفّز وحده — فلو
     * سمح هذا الفحصُ خطأً لبقي المحفّزُ هو الحاكم.
     */
    public function minuteRuleBlocks(int $accFrom): bool
    {
        try {
            $row = DB::selectOne(
                'SELECT ISNULL((
                    SELECT DATEDIFF(MINUTE, a.InsertDate, GETDATE())
                      FROM InternalEx a
                     WHERE a.AccFrom = ?
                       AND a.IDCode = (SELECT MAX(a2.IDCode)
                                         FROM InternalEx a2
                                        WHERE a2.AccFrom = ?)
                 ), 6) AS mins',
                [$accFrom, $accFrom],
            );

            return ((int) ($row->mins ?? 6)) <= 1;
        } catch (\Throwable) {
            // تعذّر الفحص ⇐ لا يُمنع شيء: الحاكمُ هو المحفّز لا نحن.
            return false;
        }
    }

    /**
     * يحجز مفتاحَ الطلب قبل أي كتابةٍ مالية — البند: Idempotency.
     *
     * ⚠ **الحجزُ قبل التنفيذ لا بعده.** طلبان متسارعان (ضغطةٌ مكرّرة، أو
     * شبكةٌ ضعيفة أعادت الإرسال) يمرّان معاً على أي فحصٍ بـ`EXISTS`، فتُكتب
     * حوالتان ثم يشتكي الفهرس. والمالُ خرج مرّتين.
     *
     * فالحارسُ هو **القاعدة**: من نجح إدراجُه يملك حقّ التنفيذ، ومن اصطدم
     * بالفهرس الفريد فطلبُه معالَجٌ أو قيد المعالجة.
     *
     * @return array{ok:true,claim_id:int}|array{ok:false,duplicate:true,transfer_number:?string}
     */
    public function claim(object $employee, string $clientId): array
    {
        try {
            $id = DB::table('employee_transfer_claims')->insertGetId([
                'employee_id' => (int) $employee->id,
                'agent_id'    => (int) $employee->agent_id,
                'client_id'   => mb_substr($clientId, 0, 64),
                'status'      => 'PENDING',
                'created_at'  => now(),
            ]);

            return ['ok' => true, 'claim_id' => (int) $id];
        } catch (\Throwable) {
            /*
             * الاصطدامُ بالفهرس هو الحالةُ المقصودة لا خطأً عارضاً — ويُقرأ
             * الصفُّ القائم ليُعاد للتطبيق ما انتهى إليه الطلبُ الأوّل.
             */
            $row = DB::table('employee_transfer_claims')
                ->where('employee_id', $employee->id)
                ->where('client_id', mb_substr($clientId, 0, 64))
                ->first(['transfer_number', 'status']);

            return [
                'ok'              => false,
                'duplicate'       => true,
                'transfer_number' => $row->transfer_number ?? null,
                'status'          => $row->status ?? 'PENDING',
            ];
        }
    }

    /** يختم الحجز بنتيجته — ولا يُخطئ: الحوالة وقعت، والوصفُ لا يُلغيها. */
    public function closeClaim(int $claimId, ?string $transferNumber, bool $ok): void
    {
        try {
            DB::table('employee_transfer_claims')->where('id', $claimId)->update([
                'transfer_number' => $transferNumber,
                'status'          => $ok ? 'DONE' : 'FAILED',
                'completed_at'    => now(),
            ]);
        } catch (\Throwable) {
        }
    }

    /**
     * تسجيل النسبة — بعد نجاح الكتابة الماليّة لا قبلها.
     *
     * ⚠ الترتيبُ مقصود: نسبةٌ تُكتب قبل نجاح الحوالة تُنشئ سجلاً لعمليةٍ لم
     * تقع. وهي **لا تُخطئ**: فشلُ سطرِ نسبةٍ لا يجوز أن يُلغيَ حوالةً كُتبت
     * في الدفتر بالفعل — المال أوّلاً، والوصفُ بعده.
     */
    public function attributeCreate(
        object $employee,
        object $session,
        string $transferNumber,
        float $amount,
        ?string $recipientPhone = null,
        ?string $recipientName = null,
        string $channel = 'LOCAL',
    ): void {
        try {
            DB::table('transfer_attributions')->insert([
                'action'           => 'CREATED',
                /*
                 * ⚠ القناةُ تُكتب لتُقرأ، لا لتُحسب: «حوالاتي الخارجية»
                 * و«كشف الخارجية» يُفرزان بها. والبديلُ سؤالُ الدفتر عن كلّ
                 * رقمٍ على حدة — وهو الشكلُ الذي أبطأ كشفَ الحساب 68 ثانية.
                 */
                'channel'          => $channel,
                'transfer_number'  => $transferNumber,
                'agent_id'         => $employee->agent_id,
                'employee_id'      => $employee->id,
                'point_of_sale_id' => $session->active_pos_id ?? null,
                'device_hash'      => $session->device_hash ?? null,
                'session_id'       => $session->id ?? null,
                'amount'           => $amount,
                /*
                 * ⚠ المستفيدُ يُسجَّل هنا لأن سؤال «هل حوّل هذا الموظف
                 * لهذا الرقم قبل ساعة؟» يُسأل عن **الحوالات التي وقعت**،
                 * وهذا الجدولُ هو سجلُّها التشغيليّ. وبدونه يبدأ فحصُ
                 * التكرار من فراغٍ عند كل حوالة فلا يمنع شيئاً.
                 *
                 * والرقمُ بصيغة الخادم نفسِها كما وصل، فيطابق ما يُبحث به.
                 */
                'recipient_phone'     => $recipientPhone !== '' ? $recipientPhone : null,
                'recipient_name_norm' => EmployeeLimitPolicy::normalizeName($recipientName),
                'occurred_at'      => now(),
            ]);
        } catch (\Throwable) {
            // متعمَّد: الحوالة وقعت، ووصفُها لا يُلغيها.
        }
    }
}
