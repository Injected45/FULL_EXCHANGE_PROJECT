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

        // ترشيح الاعتماد باستعلامٍ ثانٍ لا بـ JOIN — قياسٌ لا ذوق.
        //
        // العرض مرشَّحاً بالفرع وحده يعود في 0.05 ثانية، وبإضافة
        // `JOIN InternalEx` على البيانات نفسها صار 16.9 ثانية: المخطِّط
        // يجسّد العرض لكل الفروع قبل الربط بدل أن يدفع ترشيح الفرع إلى
        // داخله. وكشف الحساب يستدعي هذه المزامنة عند كل فتح للشاشة الرئيسية،
        // فكان الطلب يتجاوز مهلة PHP (60 ثانية) ويسقط، وتبقى «آخر العمليات»
        // هياكلَ تحميل إلى الأبد.
        //
        // والقراءتان الصغيرتان تعطيان النتيجة نفسها حرفياً: صفوف الفرع التي
        // حالتها في `InternalEx` «معتمدة» (2) — الشرط لم يتغيّر، موضعُه فقط.
        $rows = DB::table("$viewName as a")
            ->where('a.BranchDeliveredID', '=', $branchId)
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

        if ($rows->isNotEmpty()) {
            // على دفعات: `IN` في SQL Server يقف عند 2100 وسيط، وفرعٌ نشط
            // رُصد عليه 522 حوالة — الحدّ قريب بما يكفي ليُبلَغ لا ليُتجاهل.
            $approved = collect();
            foreach ($rows->pluck('Code')->unique()->chunk(1000) as $chunk) {
                $approved = $approved->merge(
                    DB::table('InternalEx')
                        ->whereIn('Code', $chunk->values())
                        ->where('ConfirmType', '=', 2)
                        ->pluck('Code')
                );
            }
            $approved = $approved->flip();

            $rows = $rows->filter(fn ($r) => $approved->has($r->Code))->values();
        }

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
            ->get(['transfer_number', 'core_confirm_type', 'core_missing_at'])
            ->keyBy('transfer_number');

        if ($mine->isEmpty()) {
            return;
        }

        // مُجزّأً عند 1000: `IN` في SQL Server يقف عند 2100 وسيط، وفرعٌ حيّ
        // واحد يحمل 522 حوالة اليوم — فالحدّ ليس بعيداً، وتجاوزُه يرمي
        // استثناءً يُسقط كل فتحٍ للشاشة لا صفّاً واحداً.
        $core = collect();
        foreach ($mine->keys()->chunk(1000) as $chunk) {
            $core = $core->merge(
                DB::table('InternalEx as t')
                    ->leftJoin('InternalEx_Stautes as s', 's.ConfirmType', '=', 't.ConfirmType')
                    ->whereIn('t.Code', $chunk->values())
                    ->select('t.Code', 't.ConfirmType', 's.SName')
                    ->get()
            );
        }

        // ما تغيّر فعلاً وحده يُكتب، ومجموعةً واحدة لكل حالة لا صفّاً صفّاً.
        // تحديثٌ بلا تغيير يحرّك updated_at فيبدو الصفّ كأنه تعدّل في كل فتح
        // للشاشة، والتحديث صفّاً صفّاً يعني استعلاماً لكل حوالة.
        $changed = [];
        foreach ($core as $c) {
            $current = $mine[$c->Code]->core_confirm_type ?? null;
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

        $this->reconcileMissing($agentId, $mine, $core->pluck('Code')->flip());
    }

    /**
     * صفٌّ لا أصل له في المنظومة يختفي من التطبيق — ولا يُحذف.
     *
     * أمر المالك (5 سبتمبر 2026): ما لا تراه قاعدة البيانات لا يراه التطبيق،
     * والعكس. والدفتر يُضاف إليه ولا يُحذف منه، فحوالةٌ مُحيت من `InternalEx`
     * نهائياً — كما يقع عند تنظيف القاعدة — كانت تبقى معروضة بلا وجود.
     *
     * **وسمٌ لا حذف**، لثلاثة أسباب: `transfer_status_history` مرتبط بمفتاح
     * أجنبي بهذا الجدول فالحذف يمحو سجلّ من سلّم ومتى؛ والغياب قد يكون
     * مؤقّتاً والوسم يُرفع من تلقائه حين يعود الأصل بينما الحذف لا يُستردّ؛
     * والوسم يحمل تاريخ الاختفاء فيُقرأ بعد شهر، والمحذوف لا يقول شيئاً.
     *
     * ولا استعلام إضافي: [$alive] هي رموز الصفوف التي عادت من الاستعلام
     * الذي جرى فعلاً في [refreshCoreState] — الجديد هنا مقارنةٌ في الذاكرة،
     * لا قراءةٌ ثانية للمنظومة. وسؤالُ الوجود صفّاً صفّاً كان ليكون كارثة:
     * لا فهرس على `InternalEx.Code`، فكل صفّ يعيد مسح الجدول (وهو ما جعل
     * كشف الحساب يستغرق 68 ثانية حين جُرّب الشكل نفسه هناك).
     *
     * @param  \Illuminate\Support\Collection  $mine   صفوف الدفتر، مفتاحها الرمز
     * @param  \Illuminate\Support\Collection  $alive  رموز ما وُجد في المنظومة
     */
    private function reconcileMissing(int $agentId, $mine, $alive): void
    {
        $gone = [];
        $back = [];

        foreach ($mine as $code => $row) {
            $exists  = $alive->has($code);
            $flagged = $row->core_missing_at !== null;

            if (!$exists && !$flagged) {
                $gone[] = $code;
            } elseif ($exists && $flagged) {
                $back[] = $code;
            }
        }

        if ($gone !== []) {
            // يُسجَّل لأنه ليس حدثاً عادياً: في التشغيل الطبيعي لا تُحذف صفوف
            // من `InternalEx` — تُلغى فتصير ConfirmType = 5. فاختفاءُ صفٍّ
            // يعني تدخّلاً يدوياً في القاعدة، وأثرٌ في السجل هو ما يجيب
            // «لماذا نقصت حوالات الوكيل؟» بعد أسبوع.
            Log::info('agent incoming: core rows vanished', [
                'agent' => $agentId,
                'codes' => $gone,
            ]);

            foreach (array_chunk($gone, 1000) as $chunk) {
                DB::table('agent_incoming_transfers')
                    ->where('agent_id', $agentId)
                    ->whereIn('transfer_number', $chunk)
                    ->update(['core_missing_at' => now()]);
            }
        }

        if ($back !== []) {
            Log::info('agent incoming: core rows returned', [
                'agent' => $agentId,
                'codes' => $back,
            ]);

            foreach (array_chunk($back, 1000) as $chunk) {
                DB::table('agent_incoming_transfers')
                    ->where('agent_id', $agentId)
                    ->whereIn('transfer_number', $chunk)
                    ->update(['core_missing_at' => null]);
            }
        }
    }

    /**
     * تبويب ثالث محسوب لا مخزَّن.
     *
     * قرار المالك (3 سبتمبر 2026، ناسخٌ لقرار 2 سبتمبر): ما تلغيه الرحالة
     * ينتقل إلى «الملغاة» **ولو كان قد سُلِّم**. الدفتر تنظيمٌ ظاهريّ في
     * الواجهة لا قيدٌ محاسبي، فالحالة الأحدث في المنظومة هي التي تُعرض.
     *
     * وليست حالةً ثالثة في العمود `status` — لو كانت، لاحتاج تسليمُ الوكيل
     * وإلغاءُ المنظومة أن يتنازعا خانةً واحدة، فيمحو أحدهما الآخر. هي
     * ترشيحٌ محسوب: إلغاء المنظومة يعلو، و`status` يبقى محفوظاً تحته.
     *
     * ولا وسم ثانياً يقول إنها سُلّمت (أمر المالك الصريح): «ملغاة» وحدها.
     * والعمود `status` يبقى في القاعدة وفي الرد كما هو — لم يُحذف، وإنما
     * لم يعد هو ما يُعرض.
     */
    public const CANCELLED_TAB = 'CANCELLED';

    /** صفحة من حوالات الوكيل بحالةٍ ما. */
    public function list(int $agentId, ?string $status, ?string $search, int $page, int $perPage): array
    {
        $q = DB::table('agent_incoming_transfers')
            ->where('agent_id', $agentId)
            // ما محي أصله من المنظومة لا يُعرض في أي تبويب — أمر المالك
            // (5 سبتمبر 2026): لا يظهر في التطبيق ما ليس في القاعدة.
            // انظر `reconcileMissing`.
            ->whereNull('core_missing_at');

        if ($status === self::CANCELLED_TAB) {
            // بلا شرطٍ على `status`: الملغاة تشمل المسلَّمة وغير المسلَّمة.
            $q->whereIn('core_confirm_type', self::CORE_CANCELLED);
        } elseif ($status === self::DELIVERED) {
            // والمسلَّمة الملغاة تخرج من هنا، وإلا ظهرت في تبويبين معاً.
            $q->where('status', self::DELIVERED)
              ->where(function ($w) {
                  $w->whereNull('core_confirm_type')
                    ->orWhereNotIn('core_confirm_type', self::CORE_CANCELLED);
              });
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

        $items = $q->orderByDesc('id')->forPage($page, $perPage)->get();

        $this->attachCoreText($items);

        return [
            'items'     => $items,
            'total'     => $total,
            'page'      => $page,
            'per_page'  => $perPage,
            'last_page' => $perPage > 0 ? (int) ceil($total / $perPage) : 1,
        ];
    }

    /**
     * نصوص المنظومة الثلاثة: سبب الإلغاء وملاحظته، وملاحظة منشئ الحوالة.
     *
     * **تُقرأ حيّةً ولا تُنسخ إلى دفتر الوكيل**: نصٌّ يُصحَّح في المنظومة يجب
     * أن يُقرأ مصحّحاً هنا، لا كما كان لحظة المزامنة. فلو نُسخ سببُ إلغاء ثم
     * تراجعت الرحالة عنه، لقرأ الوكيل سبباً لم يعد قائماً.
     *
     * ## استعلامان لكل صفحة، لا خمسة لكل صفّ
     *
     * كان هذا خمسة استعلامات فرعية مرتبطة داخل `SELECT` القائمة — ثلاثة منها
     * على `InternalEx`. وهي **الشكل نفسه** الذي جعل كشف الحساب يستغرق 68
     * ثانية: `InternalEx.Code` **بلا فهرس** (فُحص: `PK(ID)` و`UQ(IDCode)`
     * وفهرسان على رايتَي الإلغاء لا غير)، فكل صفّ يعيد مسح الجدول. صفحةٌ من
     * عشرين صفّاً كانت ستعني ستّين مسحاً لجدولٍ فيه مئات الآلاف من الصفوف،
     * وأربعين مسحاً لـ`TransCancelRequestTb`.
     *
     * والآن: استعلامٌ واحد لكل جدول، مقيَّدٌ برموز الصفحة (عشرون رمزاً، ومئة
     * في أقصى `per_page`)، والربط في PHP. وهو النمط المُقرَّر في هذا المشروع
     * منذ `LocalStatmentAccount`: ما يمكن جمعه في الذاكرة لا يُعاد سؤاله من
     * قاعدة البيانات صفّاً صفّاً.
     *
     * والفهرس على `InternalEx.Code` كان سيحلّ الأصل، وهو ممنوع: جدولٌ ماليّ
     * تحت الأمر الدائم.
     *
     * ## لماذا لا JOIN
     *
     * الرمز قد يتكرّر في الجدولين — أكثر من طلب إلغاء لرقم واحد، وأكثر من
     * صفّ في `InternalEx` — و JOIN عليه يضاعف صفّ الحوالة في القائمة. فيُؤخذ
     * **الأحدث** (‏`ID` الأكبر) لكل رمز، وهو الحوالة الحيّة والطلب الأخير.
     *
     * وذلك أيضاً أدقّ ممّا كان: `TOP 1` بلا `ORDER BY` في الاستعلامين
     * الفرعيّين القديمين كان يعيد صفّاً غير محدَّد حين يتكرّر الرمز.
     *
     * ولا ترشيح بـ`IsActive` على طلب الإلغاء: `..._DeleteRequestByID` يضع
     * `IsActive = 0` عند التأكيد ولا يحذف الصفّ، فسببُ طلبٍ نُفِّذ وأُقفل
     * يبقى قائماً ويجب أن يُقرأ.
     *
     * @param  \Illuminate\Support\Collection  $items  صفوف الصفحة — تُعدَّل مكانها
     */
    private function attachCoreText($items): void
    {
        $codes = $items->pluck('transfer_number')->filter()->unique()->values();

        $core    = [];
        $cancels = [];

        // مُجزّأ عند 1000 كسائر الاستعلامات هنا: `IN` يقف عند 2100 وسيط.
        // وصفحةٌ لا تتجاوز 100 صفّ، فالتجزئة احتياطٌ لا حاجة يومية — لكن
        // حدّاً يُتجاوز مرّةً واحدة يُسقط الشاشة كلّها.
        foreach ($codes->chunk(1000) as $chunk) {
            // الأقدم أولاً، فيَغلب الأحدث حين يُكتب فوقه في المصفوفة.
            foreach (
                DB::table('InternalEx as ie')
                    ->leftJoin('AddCancelReason as r', 'r.ID', '=', 'ie.AddCancelReason_ID')
                    ->whereIn('ie.Code', $chunk->values())
                    ->orderBy('ie.ID')
                    ->get(['ie.Code', 'r.NewCause', 'ie.AddCancelReason_NameFrom_Driver', 'ie.Notes'])
                as $r
            ) {
                $core[$r->Code] = $r;
            }

            foreach (
                DB::table('TransCancelRequestTb as tc')
                    ->leftJoin('AddCancelReason as r', 'r.ID', '=', 'tc.ReasonID')
                    ->whereIn('tc.ISID', $chunk->values())
                    ->orderBy('tc.ID')
                    ->get(['tc.ISID', 'r.NewCause', 'tc.Notes'])
                as $r
            ) {
                $cancels[$r->ISID] = $r;
            }
        }

        $text = static function (?string $v): ?string {
            $v = trim((string) $v);
            return $v === '' ? null : $v;
        };

        foreach ($items as $row) {
            $c = $core[$row->transfer_number]    ?? null;
            $x = $cancels[$row->transfer_number] ?? null;

            // ترتيب الأولوية كما كان حرفياً: مبرّر طلب الإلغاء، ثم المبرّر
            // المكتوب على الحوالة (مسار التاكسي)، ثم نصّ السائق الحرّ.
            $row->cancel_reason = $text($x->NewCause ?? null)
                ?? $text($c->NewCause ?? null)
                ?? $text($c->AddCancelReason_NameFrom_Driver ?? null);

            $row->cancel_notes = $x->Notes ?? null;

            // ملاحظة منشئ الحوالة. والفراغ يصير NULL: العمود يحتمل '' ومسافات
            // بيضاء، وهي «بلا ملاحظة» لا ملاحظةٌ فارغة — والتطبيق يُخفي الحقل
            // على NULL ويعرض حاويةً فارغة على ''.
            $row->notes = $text($c->Notes ?? null);
        }
    }

    /**
     * أعداد التبويبين، والملغاة معدودةٌ على حدة.
     *
     * لا تبويب يعدّ الملغاة غيرُ تبويبها: العدد وعدٌ بما في التبويب، وصفٌّ
     * يُعدّ مرّتين يجعل مجموع التبويبات أكبر من عدد الحوالات.
     */
    public function counts(int $agentId): array
    {
        $base = DB::table('agent_incoming_transfers')
            ->where('agent_id', $agentId)
            // ما محي أصله من المنظومة لا يُعدّ — انظر `reconcileMissing`.
            ->whereNull('core_missing_at');

        $pending = (clone $base)
            ->where('status', self::PENDING)
            ->where(function ($w) {
                $w->whereNull('core_confirm_type')
                  ->orWhereNotIn('core_confirm_type', self::CORE_CANCELLED);
            })
            ->count();

        $delivered = (clone $base)
            ->where('status', self::DELIVERED)
            ->where(function ($w) {
                $w->whereNull('core_confirm_type')
                  ->orWhereNotIn('core_confirm_type', self::CORE_CANCELLED);
            })
            ->count();

        $cancelled = (clone $base)
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

            // لا أصل لها في المنظومة ⇒ لا تُسجَّل تسليماً كذلك.
            //
            // لا تصل من شاشةٍ محدَّثة — الصفّ لا يُعرض أصلاً — لكن الشاشة قد
            // تكون مفتوحة منذ ما قبل اختفاء الأصل. وتسجيلُ التسليم يكتب
            // إسناداً وقيداً في صندوق الموظف لحوالةٍ لا وجود لها، وهو أسوأ
            // من رفضٍ يفهمه الوكيل.
            if ($row->core_missing_at !== null) {
                return [
                    'changed' => false,
                    'row'     => $row,
                    'missing' => true,
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
                /* سبقنا إليها طلبٌ آخر بين القراءة والتحديث.
                 *
                 * الشرط `status = PENDING` في التحديث نفسه هو ما يمنع التسليم
                 * المزدوج: طلبان متزامنان يتسلسلان على قفل الصفّ، فيعيد الثاني
                 * تقييم الشرط بعد التزام الأول فلا يصيب شيئاً. قِيس بثلاثة
                 * طلبات متزامنة على الصفّ نفسه: واحدٌ نجح، وصفٌّ تاريخيّ واحد.
                 *
                 * و`$row` هنا **قراءةٌ قديمة** التُقطت قبل فوز غيرنا، فحالتها
                 * ما زالت «بانتظار التسليم». إعادتها كما هي كانت تعطي الخاسر
                 * رسالة «مسجّلة كمسلَّمة سلفاً» وحمولةً تقول عكسها، فيبقى
                 * الزرّ في تطبيقه صالحاً لحوالةٍ سُلّمت. تُقرأ من جديد.
                 */
                $fresh = DB::table('agent_incoming_transfers')->where('id', $id)->first();

                return ['changed' => false, 'row' => $fresh ?: $row];
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
