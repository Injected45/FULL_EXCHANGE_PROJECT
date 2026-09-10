<?php

/*
 * ════════════════════════════════════════════════════════════════════════════
 *  ⚠⚠ البوّابة السيادية: لا تصل حوالةٌ إلى الوكيل قبل اعتمادها
 * ════════════════════════════════════════════════════════════════════════════
 *
 *   php artisan tinker --execute="require base_path('tests/manual/approval_gate_check.php');"
 *
 * أمرُ المالك، وأعاده في 9 سبتمبر 2026 بعد أن وجد الشرطَ منفرطاً:
 *
 *   «عند تنفيذ حوالة من الرحالة لا تصل إلى الوكيل ولا يراها في التطبيق ولا
 *    يصل إليه أيُّ إشعارٍ أو رسالة إلّا بعد أن تُعتمد من إدارة الرحالة».
 *
 * ── لماذا انفرط، وهو الدرسُ الذي يحرسه هذا الملف ──────────────────────────
 *
 * `syncFromCore` لا تُدخل إلّا `ConfirmType = 2`. فظُنّ الشرطُ محروساً عند
 * الباب — **وهو ليس كذلك**: الاعتماد يُسحب بعد الوصول، فيبقى الصفُّ ويتحدّث
 * رقمُه، وكلُّ قارئٍ كان يسأل «أليست ملغاة؟» فتمرّ حالةُ عدم الاعتماد (0).
 *
 * فالحراسةُ عند الباب وحدَها لا تكفي: **كلُّ منفذِ قراءةٍ حارسٌ**.
 *
 * ── وهذا الملفّ طبقتان ────────────────────────────────────────────────────
 *
 *  ١) **سلوكيّة**: يُصنع صفٌّ غيرُ معتمد فعلاً، ويُسأل كلُّ منفذ: أتراه؟
 *  ٢) **بنيويّة**: يُمسح الكود بحثاً عن كل من يمسّ الجدول بلا البوّابة —
 *     فمنفذٌ يُضاف غداً ويُنسى فيه الشرطُ **يُسقط هذا الفحص**، لا أن ينتظر
 *     وكيلاً يكتشفه بحوالةٍ دفع مالَها.
 */

use App\Services\AgentIncomingTransfersService as Svc;
use Illuminate\Support\Facades\DB;

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0;
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, $line) {
    if ($p) { $ok++;   $line('  PASS  ' . $n . ($d ? "  ($d)" : '')); }
    else    { $fail++; $line('  FAIL  ' . $n . ($d ? "  ($d)" : '')); }
};

$line('════════════════════════════════════════════════════════════');
$line(' ⚠⚠ البوّابة السيادية — لا وصولَ قبل الاعتماد');
$line('════════════════════════════════════════════════════════════');

$agentId = 104;
$svc = app(Svc::class);

$before = [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => DB::table('InternalEx')->count(),
    'ledger'   => DB::table('ExchangeAccData')->count(),
];

/* ── تنظيفُ بقايا ─────────────────────────────────────────────────────── */
DB::table('agent_incoming_transfers')
    ->where('transfer_number', 'like', 'GATE-TEST-%')->delete();

/*
 * ⚠⚠ **خطُّ أساسٍ للعدّادات — يُقاس ولا يُفترض صفراً.**
 *
 * كان الفحصُ رقم 3 يشترط `array_sum($counts) === 0`، أي أن يكون دفترُ
 * الوكيل **خالياً تماماً**. وهو شرطٌ كان صادقاً يومَ كُتب الفحص على قاعدةٍ
 * فارغة، ثمّ صار كاذباً بمجرّد أن سلّم الوكيل حوالةً واحدة: سقط الفحصُ
 * وأعلن تسرّباً لم يقع.
 *
 * وهذا هو العيبُ نفسُه الذي سُجّل في `CLAUDE.md` عن
 * `agent_incoming_acceptance` — فحصٌ يقيس «هل الجدولُ فارغ؟» ويسمّيها
 * «هل تسرّب الصفُّ غيرُ المعتمد؟». والفرقُ بين السؤالين هو الفحصُ كلُّه.
 *
 * ⚠ وفحصٌ يسقط دائماً هو فحصٌ لا يقرؤه أحد — ثمّ يُتجاوَز يومَ يسقط بحقّ.
 *
 * فيُقاس الآن **الفرق**: العدّاداتُ قبل إدراج الصفّ غير المعتمد، ثمّ بعده.
 * وثباتُها هو الدليل، لا خلوُّ الجدول.
 */
$countsBefore = $svc->counts($agentId);

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ١) صفٌّ غيرُ معتمد: هل يراه أحد؟ ─────────────────────────');

$code = 'GATE-TEST-' . time();
$id = (int) DB::table('agent_incoming_transfers')->insertGetId([
    'agent_id'          => $agentId,
    'transfer_number'   => $code,
    'beneficiary_name'  => 'مستفيد بوّابة',
    'beneficiary_phone' => '910000123',
    'amount'            => 750,
    'status'            => Svc::PENDING,
    /* ⚠ 0 = لم تعتمدها إدارة الرحالة بعد. */
    'core_confirm_type' => 0,
    'core_status_label' => 'غير معتمدة',
    'core_synced_at'    => now(),
    'received_at'       => now(),
    'created_at'        => now(),
    'updated_at'        => now(),
]);

/* أ) القائمة — كلُّ تبويب، وبلا تبويب */
$seen = [];
foreach ([Svc::PENDING, Svc::DELIVERED, 'CANCELLED', null] as $tab) {
    $items = collect($svc->list($agentId, $tab, null, 1, 200)['items'])
        ->pluck('transfer_number');
    if ($items->contains($code)) { $seen[] = $tab ?? '(بلا تبويب)'; }
}
$check('1. ⚠ لا تظهر في أيّ تبويب — ولا حتى بلا تبويب',
    $seen === [], $seen === [] ? 'أربعة تبويبات' : 'ظهرت في: ' . implode('، ', $seen));

/* ب) البحث — بابٌ آخر إلى الصفّ نفسِه */
$found = collect($svc->list($agentId, null, $code, 1, 50)['items'])
    ->pluck('transfer_number')->contains($code);
$check('2. ⚠ ولا يجدها البحثُ برقمها', !$found);

/* ج) العدّادات — الفرقُ لا المجموع. انظر `$countsBefore` أعلاه. */
$counts = $svc->counts($agentId);
$check('3. ⚠ ولا تُعَدّ في العدّادات',
    $counts == $countsBefore,
    'قبل=' . json_encode($countsBefore) . ' بعد=' . json_encode($counts));

/* د) الجرس — أخطرُها: رنّةٌ تُفشي وجودَ حوالة */
$user = \App\Models\User::find($agentId);
$ids = Svc::onlyApproved(
        DB::table('agent_incoming_transfers')->where('agent_id', $agentId)
    )
    ->where('status', Svc::PENDING)
    ->pluck('id');
$check('4. ⚠⚠ ولا يرنّ لها الجرس — «ولا يصل إليه أيُّ إشعار»',
    !$ids->contains($id), 'معرّفات الجرس = ' . $ids->count());

/* هـ) تقريرُ الموظف */
$emp = (object) ['id' => 0, 'agent_id' => $agentId];
$rep = app(\App\Services\Employees\EmployeeReports::class)->pending($emp);
$inRep = collect($rep['items'])->pluck('transfer_number')->contains($code);
$check('5. ⚠ ولا تظهر في تقرير الموظف', !$inRep,
    'عددُ المعلَّق = ' . $rep['count']);

/* و) التسليم — الحارسُ الأخير */
$res = $svc->markDelivered($agentId, $id, $agentId, []);
$check('6. ⚠⚠ وتسليمُها مرفوضٌ من الخدمة نفسِها',
    empty($res['changed']) && !empty($res['not_approved']));

$still = DB::table('agent_incoming_transfers')->where('id', $id)->value('status');
$check('7. وحالتُها لم تتغيّر', $still === Svc::PENDING, (string) $still);

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٢) وحين تُعتمد، تصل ─────────────────────────────────────');

DB::table('agent_incoming_transfers')->where('id', $id)
    ->update(['core_confirm_type' => Svc::CORE_APPROVED, 'core_status_label' => 'مسلمه']);

$items = collect($svc->list($agentId, Svc::PENDING, null, 1, 200)['items'])
    ->pluck('transfer_number');
$check('8. ⚠ الاعتمادُ يفتح البابَ فوراً — لا مَنعَ مطلق',
    $items->contains($code));

$counts = $svc->counts($agentId);
$check('9. ويُعَدّ على العدّاد', ($counts[Svc::PENDING] ?? 0) >= 1,
    'معلَّق = ' . ($counts[Svc::PENDING] ?? 0));

/* ⚠ وسحبُ الاعتماد بعد الوصول يُخفيها ثانيةً — وهي الحالةُ التي انفرط بها. */
DB::table('agent_incoming_transfers')->where('id', $id)
    ->update(['core_confirm_type' => 0]);

$items = collect($svc->list($agentId, Svc::PENDING, null, 1, 200)['items'])
    ->pluck('transfer_number');
$check('10. ⚠⚠ وسحبُ الاعتماد بعد الوصول يُخفيها ثانيةً',
    !$items->contains($code));

DB::table('agent_incoming_transfers')->where('id', $id)->delete();

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٣) ولا منفذَ يمسّ الجدول بلا البوّابة ────────────────────');

/*
 * ⚠ **الطبقةُ البنيوية.**
 *
 * السلوكُ أعلاه يفحص المنافذَ المعروفة اليوم. وهذا يفحص ما يُضاف غداً:
 * كلُّ ملفٍّ يمسّ `agent_incoming_transfers` يجب أن يمرّ بـ`onlyApproved`
 * — أو يُستثنى **بالاسم وبسبب مكتوب**، لا بالصمت.
 */
$allowed = [
    /* الخدمةُ نفسُها: فيها البوّابة، والمزامنةُ تكتب ولا تقرأ للعرض. */
    'app/Services/AgentIncomingTransfersService.php'
        => 'موطنُ البوّابة، والمزامنةُ والتحديثُ يكتبان',

    /* استعلامان فرعيان يُلصقان حالةً على حركةٍ في كشف الوكيل نفسِه.
       ⚠ ولا تسريبَ فيهما: الحوالةُ غيرُ المعتمدة لا تُنتج حركةً في
       `ExchangeAccData` أصلاً، فلا صفَّ تُلصق عليه حالتُها. وتعديلُ هذا
       الاستعلام الماليّ الضخم خطرٌ بلا مقابل. */
    'app/Http/Controllers/Api/depositController.php'
        => 'وسمٌ على كشف الوكيل — والحوالةُ غيرُ المعتمدة لا حركةَ لها فيه',

    /* فحوصٌ تصنع صفوفاً وتفحصها عمداً. */
    'tests/manual/approval_gate_check.php'     => 'هذا الفحص',
    'tests/manual/agent_incoming_acceptance.php' => 'فحصُ قبولٍ يصنع صفوفاً',
    'tests/manual/employee_cashbox_ledger_acceptance.php' => 'فحصُ قبول',
    'tests/manual/employee_default_deny_check.php' => 'فحصُ قبول',
];

$root = base_path();
$offenders = [];
$rii = new RecursiveIteratorIterator(new RecursiveDirectoryIterator($root . '/app'));
$files = [];
foreach ($rii as $f) {
    if (!$f->isDir() && $f->getExtension() === 'php') { $files[] = $f->getPathname(); }
}
foreach (glob($root . '/tests/manual/*.php') as $f) { $files[] = $f; }

foreach ($files as $path) {
    $src = file_get_contents($path);
    if (!str_contains($src, 'agent_incoming_transfers')) { continue; }

    $rel = str_replace('\\', '/', substr($path, strlen($root) + 1));
    if (isset($allowed[$rel])) { continue; }

    if (!str_contains($src, 'onlyApproved')) { $offenders[] = $rel; }
}

$check('11. ⚠⚠ كلُّ من يمسّ الجدول يمرّ بالبوّابة',
    $offenders === [],
    $offenders === [] ? 'نظيف' : 'بلا بوّابة: ' . implode(' · ', $offenders));

$line();
$line('  الملفّاتُ المستثناة بالاسم:');
foreach ($allowed as $f => $why) { $line('    · ' . $f . ' — ' . $why); }

// ══════════════════════════════════════════════════════════════════════════
$line();
$line('── ٤) البصمة المالية ────────────────────────────────────────');
$after = [
    'wallet'   => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
    'internal' => DB::table('InternalEx')->count(),
    'ledger'   => DB::table('ExchangeAccData')->count(),
];
foreach ($before as $k => $v) {
    $check('12. ' . $k . ' لم يتغيّر', (string) $after[$k] === (string) $v,
        'قبل=' . $v . ' بعد=' . $after[$k]);
}

$line();
$line('════════════════════════════════════════════════════════════');
$line('  نجح: ' . $ok . '   أخفق: ' . $fail);
$line('════════════════════════════════════════════════════════════');
