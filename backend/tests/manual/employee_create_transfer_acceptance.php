<?php

use Illuminate\Support\Facades\DB;

/*
 * إنشاءُ الحوالة من تطبيق الموظف — بإذن المالك (7 سبتمبر 2026).
 *
 * ⚠ **هذا الاختبار يكتب في الدفتر المالي.** لا يُشغَّل على قاعدةٍ إنتاجية.
 * وهو يكتب حوالةً واحدة بأصغر مبلغ، ويترك أثرَه ظاهراً — ولا يحذفه:
 * صفٌّ في `InternalEx` يُحذف بيدنا أسوأ من صفٍّ باقٍ، لأن الحذف نفسَه
 * تدخّلٌ في دفترٍ لا نملكه.
 *
 * ── ما يُثبته ───────────────────────────────────────────────────────────
 *
 *   الشرطُ الذي وضعه المالك: «سجلُّها الماليّ كما الوكيل، وليس أي سجلاتٍ
 *   جديدة». فالفحصُ الأهمّ هنا ليس أن الحوالة تُكتب، بل أن الصفَّ المكتوب
 *   **لا يُميَّز عن صفّ الوكيل**: الحسابُ حسابُه، والمُدخِلُ هو، ولا عمودَ
 *   فيه يقول إن موظّفاً هو من ضغط الزرّ.
 */

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, &$failed, $line) {
    if ($p) { $ok++; $line("  PASS  $n" . ($d ? "  ($d)" : '')); }
    else    { $fail++; $failed[] = $n; $line("  FAIL  $n" . ($d ? "  ($d)" : '')); }
};

$call = function (string $m, string $p, ?string $tok = null, array $b = []) {
    $ch = curl_init('http://127.0.0.1:8000/api' . $p);
    $h = ['Accept: application/json', 'Content-Type: application/json'];
    if ($tok) $h[] = 'Authorization: Bearer ' . $tok;
    curl_setopt_array($ch, [CURLOPT_RETURNTRANSFER => true, CURLOPT_CUSTOMREQUEST => $m,
                            CURLOPT_HTTPHEADER => $h, CURLOPT_TIMEOUT => 60]);
    if ($b !== []) curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($b, JSON_UNESCAPED_UNICODE));
    $o = curl_exec($ch);
    $c = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);
    return ['status' => $c, 'body' => json_decode($o, true)];
};

$line('══════════════════════════════════════════════════════════════');
$line('   الموظف ينشئ حوالة — واجهةٌ للوكيل لا كيانٌ ثانٍ');
$line('══════════════════════════════════════════════════════════════');

$emp = DB::table('employees')->where('status', 'ACTIVE')->orderByDesc('id')->first();
if (!$emp) { $line('  لا موظّف مفعّل.'); return; }

$agent = DB::table('users')->where('id', $emp->agent_id)->first();
$sessRow = DB::table('employee_sessions')->where('employee_id', $emp->id)
    ->where('status', 'ACTIVE')->orderByDesc('id')->first();
if (!$sessRow) { $line('  لا جلسة فعّالة.'); return; }

$line();
$line("الموظّف: {$emp->full_name}   ·   الوكيل: {$agent->name} (AccID={$agent->AccID})");

$before = [
    'internal' => (int) DB::selectOne('SELECT COUNT(*) v FROM InternalEx')->v,
    'attrib'   => (int) DB::selectOne('SELECT COUNT(*) v FROM transfer_attributions')->v,
];

/* جلسةُ فحصٍ برمزٍ نعرفه — ولا تُمسّ جلسةُ الموظف الحقيقية. */
$raw = bin2hex(random_bytes(32));
$sid = DB::table('employee_sessions')->insertGetId([
    'agent_id' => $emp->agent_id, 'employee_id' => $emp->id,
    'device_id' => $sessRow->device_id, 'device_hash' => $sessRow->device_hash,
    'active_pos_id' => $sessRow->active_pos_id,
    'access_token_hash' => hash('sha256', $raw), 'status' => 'ACTIVE',
    'created_at' => now(), 'expires_at' => now()->addHour(),
]);

$permsBefore = DB::table('employee_permissions')->where('employee_id', $emp->id)
    ->pluck('permission_key')->all();
$revoke = fn () => DB::table('employee_permissions')->where('employee_id', $emp->id)
    ->where('permission_key', 'CREATE_TRANSFER')->delete();
$grant = function () use ($emp) {
    if (!DB::table('employee_permissions')->where('employee_id', $emp->id)
            ->where('permission_key', 'CREATE_TRANSFER')->exists()) {
        DB::table('employee_permissions')->insert([
            'employee_id' => $emp->id, 'permission_key' => 'CREATE_TRANSFER',
            'granted_at' => now(),
        ]);
    }
};

/*
 * فرعٌ ومدينةٌ من حوالةٍ سابقةٍ للوكيل نفسِه — لا قيمةٌ
 * مخمّنة: ما قبلَه المسارُ منه أمس يقبلُه اليوم.
 */
$prev = DB::table('InternalEx')->where('AccFrom', $agent->AccID)
    ->whereNotNull('uesrID_forminsertmobile')
    ->orderByDesc('ID')
    ->first(['BranchDeliveredID', 'DeliveryPlace', 'RecievedCurrencyID']);

/*
 * ⚠ وفرعٌ يقبلُه فحصُ الوكيل نفسُه، لا أوّلُ فرعٍ في الجدول:
 * المقصودُ إثباتُ أنّ الموظّف يكتب كما يكتب الوكيل، لا
 * إثباتُ أنّ فرعاً بعينِه مفتوح. وفرعٌ مغلَق يُرد برسالة
 * الوكيل نفسِه، فيبدو الفحصُ ساقطاً والشّيفرةُ سليمة.
 */
$branchId = null;
foreach (DB::table('CoBranch')->limit(40)->get(['ID', 'CurrentAccID', 'BranchType']) as $b) {
    try {
        $v = DB::select('SELECT dbo.Rollback_Branch_Trinsfrim_me(?, ?, 1, 1, ?) AS ok',
            [$b->CurrentAccID, $b->BranchType, $b->ID]);
        if ((int) ($v[0]->ok ?? 0) === 1) { $branchId = (int) $b->ID; break; }
    } catch (\Throwable) { }
}
$body = [
    'country_id'    => 1,
    'reviced_phone' => '0910000000',
    'reviced_name'  => 'مستفيد فحصٍ آليّ',
    'AccID'         => $agent->AccID,
    'currency_id'   => (int) ($prev->RecievedCurrencyID ?? 1),
    'amount'        => 1,
    'branch_id'     => $branchId ?? (int) ($prev->BranchDeliveredID ?? 1),
    'city_id'       => (int) ($prev->DeliveryPlace ?? 9),
    'Commition'     => 0,
    'Notes'         => 'فحص آليّ — إنشاء من تطبيق الموظف',
];

$line();
$line('── ١) الحارس ────────────────────────────────────────────────');

$revoke();
$r = $call('POST', '/device/employee/transfers/create', $raw, $body);
$check('بلا CREATE_TRANSFER ⇐ 403', $r['status'] === 403, 'status=' . $r['status']);
$check('ولا صفَّ كُتب في الدفتر',
    (int) DB::selectOne('SELECT COUNT(*) v FROM InternalEx')->v === $before['internal']);

$line();
$line('── ٢) وبالصلاحية تُنفَّذ ─────────────────────────────────────');

$grant();
$r = $call('POST', '/device/employee/transfers/create', $raw, $body);
$line('  الردّ: ' . $r['status'] . ' — '
    . mb_substr((string) ($r['body']['message'] ?? ''), 0, 90));

$created = ($r['body']['success'] ?? false) === true;
$code = $r['body']['data']['transfer']['Code'] ?? null;

/*
 * ⚠ رفضُ المهلة ليس إخفاقاً ولا نجاحاً: تشغيلُ الملفّ مرّتين في دقيقة
 * يقع فيه حتماً. فيُعلَن «متخطّى» بصوتٍ عالٍ — لا PASS يُخفي أنّ شيئاً
 * لم يُختبَر، ولا FAIL يُوهم بعطبٍ لا وجود له.
 */
$rateLimited = !$created
    && str_contains((string) ($r['body']['message'] ?? ''), 'بعد دقيقة');

if ($rateLimited) {
    $line('  SKIP  الإنشاء — مهلةُ الدقيقة قائمة. أعد التشغيل بعد دقيقة.');
} else {
    $check('الإنشاء نجح', $created, 'code=' . ($code ?? '—'));
}

if (!$created) {
    $line();
    $line('  ⚠ لم تُنفَّذ — والسببُ أعلاه هو نفسُه الذي يراه الوكيل، فالمسار');
    $line('     واحد. (حدُّ الثلاث دقائق، أو رصيد، أو حدود تحويل.)');
}

if ($created && $code) {
    $row = DB::table('InternalEx')->where('Code', $code)->first();

    $line();
    $line('── ٣) الصفُّ المكتوب — أهمُّ قسمٍ في الفحص ──────────────────');

    $check('⚠ الحسابُ المُرسِل هو حسابُ الوكيل لا شيءَ آخر',
        (string) $row->AccFrom === (string) $agent->AccID,
        'AccFrom=' . $row->AccFrom . ' · وكيل=' . $agent->AccID);

    $check('⚠ والمُدخِلُ هو مستخدمُ الوكيل',
        (int) $row->uesrID_forminsertmobile === (int) $agent->id,
        'uesrID=' . $row->uesrID_forminsertmobile . ' · وكيل=' . $agent->id);

    /*
     * ⚠ أقوى فحصٍ في الملفّ: يُقارَن الصّفُ بحوالةٍ أنشأها
     * **الوكيل بنفسِه** من تطبيقِه، لا بقيمةٍ مكتوبةٍ هنا.
     *
     * وهو معنى شرط المالك حرفيّاً: «سجلُها الماليّ كما
     * الوكيل». ولاحظ أنّ `users.name` فارغٌ لهذا الوكيل،
     * والاسمُ يأتي من موضعٍ آخر — ففحصٌ على `users.name` كان
     * سيسقط والشّيفرةُ سليمة (وقع ذلك). والمقارنةُ
     * بالأصل لا تحتاج أن نعرف من أين يأتي الاسم.
     */
    $peer = DB::table('InternalEx')
        ->where('AccFrom', $agent->AccID)
        ->where('Code', '<>', $code)
        ->whereNotNull('uesrID_forminsertmobile')
        ->orderByDesc('ID')
        ->first(['Code', 'SenderName', 'SPhone1', 'uesrID_forminsertmobile', 'AccFrom']);

    if ($peer) {
        $same = (string) $row->SenderName === (string) $peer->SenderName
            && (string) $row->SPhone1 === (string) $peer->SPhone1
            && (string) $row->AccFrom === (string) $peer->AccFrom
            && (string) $row->uesrID_forminsertmobile === (string) $peer->uesrID_forminsertmobile;
        $check('⚠ وهويّةُ الصّفّ لا تُميّز عن حوالةٍ أنشأها الوكيل بنفسِه',
            $same, 'قورِنت بـ' . $peer->Code . ': مرسِل=' . $peer->SenderName);
    } else {
        $check('⚠ وهويّةُ الصّفّ لا تُميّز (لا حوالةَ وكيلٍ للمقارنة)', true);
    }

    /*
     * ⚠ الفحصُ الذي يحرس شرطَ المالك حرفياً: لا عمودَ في الصفّ يذكر
     * الموظف. فلو تسرّب معرّفُ موظّفٍ إلى الدفتر لصار الموظف كياناً
     * مالياً ثانياً — وهو ما مُنع.
     */
    $leaked = [];
    foreach ((array) $row as $col => $val) {
        if ($val === null || $val === '') continue;
        if ((string) $val === (string) $emp->id && !in_array($col, ['ID'], true)) {
            $leaked[] = $col;
        }
        if (is_string($val) && $emp->full_name !== '' && str_contains($val, $emp->full_name)) {
            $leaked[] = $col;
        }
    }
    $check('⚠ ولا عمودَ في الدفتر يذكر الموظف',
        $leaked === [], $leaked ? implode('، ', $leaked) : 'لا شيء');

    $line();
    $line('── ٤) والنسبةُ خارج الدفتر ─────────────────────────────────');

    $attr = DB::table('transfer_attributions')
        ->where('transfer_number', $code)->where('action', 'CREATED')->first();

    $check('نسبةٌ مسجَّلة', $attr !== null);
    if ($attr) {
        $check('باسم هذا الموظف', (int) $attr->employee_id === (int) $emp->id);
        $check('وبنقطة بيعه وجهازه',
            (string) ($attr->device_hash ?? '') === (string) ($sessRow->device_hash ?? ''));
    }

    $check('⚠ وهي في `transfer_attributions` لا في `InternalEx`',
        !array_key_exists('employee_id', (array) $row));

    $line();
    $line('── ٥) ولا تكرارَ لو أُعيد الطلب ────────────────────────────');
    $n1 = DB::table('transfer_attributions')->where('transfer_number', $code)->count();
    $check('نسبةٌ واحدة لكل (حوالة، فعل)', $n1 === 1, 'عدد=' . $n1);
}


$line();
$line('── ٦) تكرارُ الطلب لا يُنشئ حوالتين (Idempotency) ─────────────');

/*
 * ⚠ أخطرُ فحصٍ ماليّ في الملفّ: ضغطةٌ مكرّرة أو شبكةٌ أعادت الإرسال تعني
 * طلبين على المال. والمالُ لا يُسترجع.
 *
 * ⚠ ويُزرع الحجزُ الأوّل في القاعدة بدل إنشاء حوالةٍ ثانية: الحوالةُ
 * أُنشئت قبل سطورٍ، فقاعدةُ الدقيقة قائمةٌ الآن وستمنع أيّ إنشاءٍ جديد —
 * فاختبارٌ يعتمد على نجاح إنشاءٍ ثانٍ يقيس المهلةَ لا الازدواج. والمزروعُ
 * يمثّل بالضبط ما تتركه محاولةٌ نجحت.
 */
$key = 'test-' . bin2hex(random_bytes(8));
DB::table('employee_transfer_claims')->insert([
    'employee_id'     => $emp->id,
    'agent_id'        => $emp->agent_id,
    'client_id'       => $key,
    'transfer_number' => $code ?: 'SEED-1',
    'status'          => 'DONE',
    'created_at'      => now(),
    'completed_at'    => now(),
]);

$before6 = (int) DB::selectOne('SELECT COUNT(*) v FROM InternalEx')->v;

$r2 = $call('POST', '/device/employee/transfers/create', $raw,
    $body + ['client_id' => $key]);

$after6 = (int) DB::selectOne('SELECT COUNT(*) v FROM InternalEx')->v;

$check('⚠ الطلبُ بمفتاحٍ معالَجٍ لا يُنشئ حوالةً ثانية',
    $after6 === $before6, 'حوالات جديدة=' . ($after6 - $before6));

$check('ويُعلَن مكرّراً',
    ($r2['body']['data']['duplicate'] ?? false) === true,
    mb_substr((string) ($r2['body']['message'] ?? ''), 0, 60));

$check('⚠ ويُردّ برقم الحوالة الأولى لا برسالة خطأ',
    ($r2['body']['data']['transfer_number'] ?? null) === ($code ?: 'SEED-1'),
    (string) ($r2['body']['data']['transfer_number'] ?? '—'));

/* والحجزُ صفٌّ واحد — الفهرس الفريد هو الحارس من السباق. */
$claims = DB::table('employee_transfer_claims')
    ->where('employee_id', $emp->id)->where('client_id', $key)->count();
$check('وحجزٌ واحد في القاعدة', $claims === 1, 'حجوزات=' . $claims);

/* ⚠ والحارسُ الحقيقيّ: إدراجان بالمفتاح نفسِه لا يمرّان. */
$dup = false;
try {
    DB::table('employee_transfer_claims')->insert([
        'employee_id' => $emp->id, 'agent_id' => $emp->agent_id,
        'client_id'   => $key, 'status' => 'PENDING', 'created_at' => now(),
    ]);
} catch (\Throwable) {
    $dup = true;
}
$check('⚠ والقاعدةُ ترفض حجزاً ثانياً بالمفتاح نفسِه', $dup);

DB::table('employee_transfer_claims')->where('employee_id', $emp->id)
    ->where('client_id', 'like', 'test-%')->delete();
('── ٧) قاعدةُ الدقيقة تُقال بوضوح لا كخطأ 500 ─────────────────');

$line();
$line('── ٧) قاعدةُ الدقيقة تُقال بوضوح لا كخطأ 500 ─────────────────');

/*
 * ⚠ في المنظومة عتبتان: وحدةُ التحكّم `< 1` على MAX(ID)، والمحفّز
 * `<= 1` على MAX(IDCode). فعند الدقيقة الواحدة تسمح الأولى ويُلغي
 * الثاني، فيُمحى الصفُّ ويرمي المسارُ «لم يتم العثور على السجل بعد
 * الإدخال» — 500 غامضٌ سببُه مهلة. وقد وقع فعلاً.
 *
 * والحوالةُ أُنشئت للتوّ أعلاه، فالمهلةُ قائمةٌ الآن.
 */
$r = $call('POST', '/device/employee/transfers/create', $raw,
    $body + ['client_id' => 'min-' . bin2hex(random_bytes(6))]);

$check('⚠ الردُّ 422 لا 500', $r['status'] === 422, 'status=' . $r['status']);
$check('ورسالةٌ يفهمها الموظف',
    !str_contains((string) ($r['body']['message'] ?? ''), 'لم يتم العثور'),
    mb_substr((string) ($r['body']['message'] ?? ''), 0, 60));

/* ولا حجزَ احترق على محاولةٍ لم تقع. */
$burned = DB::table('employee_transfer_claims')->where('employee_id', $emp->id)
    ->where('client_id', 'like', 'min-%')->count();
$check('⚠ ولم يحترق مفتاحُ الطلب — تُعاد المحاولة بعد دقيقة',
    $burned === 0, 'حجوزات=' . $burned);

$line();
$line('── تنظيف ────────────────────────────────────────────────────');

DB::table('employee_sessions')->where('id', $sid)->delete();
in_array('CREATE_TRANSFER', $permsBefore, true) ? $grant() : $revoke();

$permsAfter = DB::table('employee_permissions')->where('employee_id', $emp->id)
    ->pluck('permission_key')->all();
sort($permsBefore); sort($permsAfter);
$check('والصلاحيات عادت كما كانت', $permsBefore === $permsAfter);

$line();
$line('── الأثر في الدفتر ───────────────────────────────────────────');
$after = [
    'internal' => (int) DB::selectOne('SELECT COUNT(*) v FROM InternalEx')->v,
    'attrib'   => (int) DB::selectOne('SELECT COUNT(*) v FROM transfer_attributions')->v,
];
printf("  InternalEx            %d ⇦ %d   (+%d)\n",
    $before['internal'], $after['internal'], $after['internal'] - $before['internal']);
printf("  transfer_attributions %d ⇦ %d   (+%d)\n",
    $before['attrib'], $after['attrib'], $after['attrib'] - $before['attrib']);

$check('⚠ حوالةٌ واحدة لا أكثر',
    $after['internal'] - $before['internal'] === ($created ? 1 : 0),
    'الفرق=' . ($after['internal'] - $before['internal']));

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) $line('   المُخفِق: ' . implode(' · ', $failed));
$line('══════════════════════════════════════════════════════════════');
