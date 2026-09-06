<?php

/*
 * اختبارات قبول — الدفعة الثانية من تطوير مركز الدعم.
 * (بنود المالك 1 لوحة القيادة · 2 SLA · 6 منع التعارض · 35/36 الفريق)
 *
 *   php artisan tinker --execute="require base_path('tests/manual/support_sla_acceptance.php');"
 *
 * ⚠ أهمُّ ما فيه القسم الثاني: **حالةُ SLA تُحسب من الوقت الحالي**، فهي
 * تُختبر بتزوير التواريخ في القاعدة لا بانتظار مرور الوقت. واختبارٌ ينتظر
 * ساعتين ليتحقّق من مهلة ساعتين لا يُشغَّل أبداً.
 */

use App\Services\Support\SupportPermissions;
use App\Services\Support\SupportPresence;
use App\Services\Support\SupportSla;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0; $failed = [];
$check = function (string $name, bool $pass, string $detail = '') use (&$ok, &$fail, &$failed, $line) {
    if ($pass) { $ok++; $line("  PASS  $name" . ($detail ? "  ($detail)" : '')); }
    else       { $fail++; $failed[] = $name; $line("  FAIL  $name" . ($detail ? "  ($detail)" : '')); }
};

$call = function (string $m, string $p, ?string $tok = null, array $b = []) {
    $ch = curl_init('http://127.0.0.1:8000/api' . $p);
    $h = ['Accept: application/json', 'Content-Type: application/json'];
    if ($tok) $h[] = 'Authorization: Bearer ' . $tok;
    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true, CURLOPT_CUSTOMREQUEST => $m,
        CURLOPT_HTTPHEADER => $h, CURLOPT_TIMEOUT => 30,
    ]);
    if ($b !== []) curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($b, JSON_UNESCAPED_UNICODE));
    $o = curl_exec($ch);
    $c = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);
    return ['status' => $c, 'body' => json_decode($o, true)];
};

$line('══════════════════════════════════════════════════════════════');
$line('   مركز الدعم — الدفعة الثانية (SLA · لوحة القيادة · التعارض)');
$line('══════════════════════════════════════════════════════════════');

$snap = fn () => [
    'wallet'   => (string) DB::selectOne('SELECT COUNT(*) v FROM wallet')->v,
    'internal' => (string) DB::selectOne('SELECT COUNT(*) v FROM InternalEx')->v,
    'safe'     => (string) DB::selectOne('SELECT COUNT(*) v FROM EX24AccSafeActivityTb')->v,
];
$before = $snap();

$purge = function (array $u) {
    foreach ($u as $n) {
        $s = DB::table('support_staff')->whereRaw('LOWER(username)=?', [$n])->first();
        if (!$s) continue;
        DB::table('support_viewers')->where('staff_id', $s->id)->delete();
        DB::table('support_sessions')->where('staff_id', $s->id)->delete();
        DB::table('support_permissions')->where('staff_id', $s->id)->delete();
        DB::table('support_audit')->where('staff_id', $s->id)->delete();
        DB::table('support_events')->where('actor_id', $s->id)->delete();
        DB::table('chat_messages')->where('support_staff_id', $s->id)
            ->update(['support_staff_id' => null]);
        DB::table('support_thread_state')->where('assigned_to', $s->id)
            ->update(['assigned_to' => null, 'assigned_at' => null, 'assigned_by' => null]);
        DB::table('support_staff')->where('id', $s->id)->delete();
    }
};
$purge(['s.admin', 's.two']);

$mk = function (string $u, string $n, string $role) {
    $id = (int) DB::table('support_staff')->insertGetId([
        'name' => $n, 'username' => $u, 'password_hash' => Hash::make('Sla12345'),
        'role' => $role, 'is_active' => 1, 'must_change' => 0,
    ]);
    foreach (SupportPermissions::ROLE_DEFAULTS[$role] as $p) {
        DB::table('support_permissions')->insert([
            'staff_id' => $id, 'permission' => $p, 'granted_at' => now(),
        ]);
    }
    return $id;
};

$aId = $mk('s.admin', 'مدير SLA', 'ADMIN');
$bId = $mk('s.two', 'موظّف ثانٍ', 'SUPERVISOR');

$aTok = $call('POST', '/support/auth/login', null,
    ['username' => 's.admin', 'password' => 'Sla12345'])['body']['data']['token'] ?? null;
$bTok = $call('POST', '/support/auth/login', null,
    ['username' => 's.two', 'password' => 'Sla12345'])['body']['data']['token'] ?? null;

$thread = DB::table('chat_threads')->where('kind', 'ADMIN')->orderBy('id')->first();
$T = (int) $thread->id;
$stateBefore = DB::table('support_thread_state')->where('thread_id', $T)->first();

$sla = app(SupportSla::class);

$line();
$line('── ١) إعدادات SLA (البند 2) ──────────────────────────────────');

$r = $call('GET', '/support/sla', $aTok);
$check('قراءة الإعدادات 200', $r['status'] === 200, 'status=' . $r['status']);
$items = $r['body']['data']['items'] ?? [];
$check('أربع أولوياتٍ مضبوطة', count($items) === 4, 'عدد=' . count($items));
$check('وكلٌّ لها مهلةُ أوّل ردّ',
    count(array_filter($items, fn ($i) => $i['first_minutes'] > 0)) === 4);

/* ⚠ نصُّ البند: «ولا تكون Hardcoded» — فالتعديلُ يجب أن يسري فوراً. */
$r = $call('PUT', '/support/sla/URGENT', $aTok, ['first_minutes' => 7]);
$check('تعديل المهلة 200', $r['status'] === 200, 'status=' . $r['status']);
$check('وسرى فوراً بلا إعادة تشغيل',
    ($call('GET', '/support/sla', $aTok)['body']['data']['items'][1]['first_minutes'] ?? 0) === 7
    || collect($call('GET', '/support/sla', $aTok)['body']['data']['items'])
        ->firstWhere('priority', 'URGENT')['first_minutes'] === 7);
/*
 * ⚠ الفحصُ الذي كشف عيباً حقيقياً: تعديلُ حقلٍ واحد كان **يمحو الآخرين**.
 *
 * الغائبُ من الطلب كان يُمرَّر `null`، و`null` تعني «لا هدف». فتعديلُ مهلة
 * أوّل الردّ وحدها جعل مهلتَي «الردّ التالي» و«المعالجة» فارغتين — فصارت
 * المحادثاتُ كلُّها «لا هدف» ولا يُنبَّه أحدٌ على تأخيرٍ أبداً.
 */
$urgent = fn () => collect($call('GET', '/support/sla', $aTok)['body']['data']['items'])
    ->firstWhere('priority', 'URGENT');
$check('⚠ وتعديلُ حقلٍ لا يمحو الحقلين الآخرين',
    ($urgent()['next_minutes'] ?? null) !== null
    && ($urgent()['resolve_minutes'] ?? null) !== null,
    'تالٍ=' . var_export($urgent()['next_minutes'] ?? null, true)
    . ' معالجة=' . var_export($urgent()['resolve_minutes'] ?? null, true));

$check('ورقمٌ خارج المدى يُرفض',
    $call('PUT', '/support/sla/URGENT', $aTok, ['first_minutes' => 99999])['status'] === 422);
$check('وأولويةٌ مجهولة تُرفض 404',
    in_array($call('PUT', '/support/sla/NOPE', $aTok, ['first_minutes' => 5])['status'], [404, 405], true));
$check('والتغيير مسجَّل في السجلّ',
    DB::table('support_audit')->where('staff_id', $aId)->where('action', 'SLA')->exists());

/* إعادةُ المهلة الأصلية قبل قياس الحالات. */
$call('PUT', '/support/sla/URGENT', $aTok, ['first_minutes' => 15]);

$line();
$line('── ٢) حساب الحالة — بتزوير التواريخ لا بانتظار الوقت ─────────');

$mkRow = fn (array $o) => (object) array_merge([
    'priority' => 'URGENT', 'status' => 'OPEN',
    'first_agent_msg_at' => null, 'first_reply_at' => null,
    'last_agent_msg_at' => null, 'last_reply_at' => null, 'resolved_at' => null,
], $o);

$fresh = $sla->evaluate($mkRow(['first_agent_msg_at' => now()->subMinutes(2)]));
$check('رسالةٌ عمرُها دقيقتان: ضمن الوقت',
    $fresh['state'] === SupportSla::OK, $fresh['label'] . ' · متبقٍّ=' . $fresh['remaining_min']);

$warn = $sla->evaluate($mkRow(['first_agent_msg_at' => now()->subMinutes(12)]));
$check('واثنتا عشرة دقيقة (80% من 15): تقترب',
    $warn['state'] === SupportSla::WARNING, $warn['label']);

$late = $sla->evaluate($mkRow(['first_agent_msg_at' => now()->subMinutes(40)]));
$check('وأربعون دقيقة: متأخّرة',
    $late['state'] === SupportSla::BREACHED, $late['label'] . ' · متبقٍّ=' . $late['remaining_min']);
$check('والمتبقّي سالبٌ في المتأخّرة', $late['remaining_min'] < 0);

/* ⚠ الحالة المغلقة لا مهلةَ عليها. */
$closed = $sla->evaluate($mkRow([
    'first_agent_msg_at' => now()->subDays(3), 'status' => 'CLOSED',
]));
$check('⚠ والمغلقة بلا مهلة — لا تُصبغ بالأحمر بلا معنى',
    $closed['state'] === SupportSla::NONE);

/* أوّلُ ردٍّ تمّ ⇒ ينتقل القياس إلى «الردّ التالي». */
$next = $sla->evaluate($mkRow([
    'first_agent_msg_at' => now()->subHours(3),
    'first_reply_at'     => now()->subHours(3)->addMinutes(2),
    'last_agent_msg_at'  => now()->subMinutes(50),
    'last_reply_at'      => now()->subHours(3)->addMinutes(2),
]));
$check('وبعد أوّل ردٍّ يقيس «الردّ التالي»',
    $next['kind'] === 'NEXT', 'النوع=' . ($next['kind'] ?? '—'));
$check('⚠ وفريقٌ ردّ بسرعةٍ ثم صمت لا يُعدّ ملتزماً',
    $next['state'] === SupportSla::BREACHED, $next['label']);

/* الأولوية تُغيّر المهلة. */
$normal = $sla->evaluate($mkRow([
    'priority' => 'NORMAL', 'first_agent_msg_at' => now()->subMinutes(40),
]));
$check('والأولوية تُغيّر المهلة — «عادية» بعد 40 دقيقة ما زالت ضمن الوقت',
    $normal['state'] === SupportSla::OK, $normal['label']);

$line();
$line('── ٣) لوحة القيادة (البند 1) ─────────────────────────────────');

$r = $call('GET', '/support/dashboard', $aTok);
$check('لوحة القيادة 200', $r['status'] === 200, 'status=' . $r['status']);
$d = $r['body']['data'] ?? [];

foreach (['new', 'open', 'pending', 'closed', 'unassigned', 'mine',
          'awaiting_support', 'awaiting_agent', 'resolved_today',
          'sla_ok', 'sla_warning', 'sla_breached',
          'unread_messages', 'staff_online', 'staff_available'] as $k) {
    $check("العدّاد يحوي $k", array_key_exists($k, $d['counts'] ?? []));
}
foreach (['first_response_min', 'next_response_min', 'resolution_min'] as $k) {
    $check("المتوسّطات تحوي $k", array_key_exists($k, $d['averages'] ?? []));
}
$check('والفريق مُرفَق', is_array($d['team'] ?? null) && count($d['team']) >= 2);
$check('وأرقامُ المتأخّرة تُرسَل لا عددُها وحده',
    array_key_exists('breached_ids', $d));

/* ⚠ اللوحة رخيصة: أربعةُ استعلاماتٍ لا عشرون. */
$n = 0;
DB::listen(function () use (&$n) { $n++; });
app(\App\Services\Support\SupportDashboard::class)->build((object) ['id' => $aId]);
$check('⚠ واللوحة كلُّها بأقلّ من ثماني رحلاتٍ إلى القاعدة',
    $n <= 8, "استعلامات=$n");

$line();
$line('── ٤) حالة الموظّف (البند 36) ────────────────────────────────');

$check('تعيين «مشغول» 200',
    $call('POST', '/support/me/presence', $aTok, ['presence' => 'BUSY'])['status'] === 200);
$check('وحالةٌ مجهولة تُرفض 422',
    $call('POST', '/support/me/presence', $aTok, ['presence' => 'NOPE'])['status'] === 422);

$team = $call('GET', '/support/team', $aTok)['body']['data']['items'] ?? [];
$mine = collect($team)->firstWhere('id', $aId);
$check('والفريق يعكسها', ($mine['presence'] ?? '') === 'BUSY', $mine['presence_label'] ?? '—');
$check('وكلُّ موظّفٍ له حمولةٌ وسعة',
    isset($mine['open_threads'], $mine['capacity'], $mine['load_pct']));

/* ⚠ «غير متصل» محسوبةٌ لا مخزَّنة. */
DB::table('support_staff')->where('id', $aId)
    ->update(['last_seen_at' => now()->subMinutes(5)]);
$check('⚠ ومن صمت خمسَ دقائق يُحسب «غير متصل» ولو كانت حالتُه «مشغول»',
    SupportPresence::effective('BUSY', now()->subMinutes(5)) === SupportPresence::OFFLINE);
$check('ومن نبض قبل ثانيةٍ يبقى على حالته',
    SupportPresence::effective('BUSY', now()) === 'BUSY');

$line();
$line('── ٥) منع تعارض الردود (البند 6) ─────────────────────────────');

/*
 * ⚠ يُبحث عن حسابَي الاختبار وحدهما لا عن «عدد المشاهدين».
 *
 * القاعدةُ حيّة، وقد يكون موظّفٌ حقيقيّ فاتحاً المحادثةَ نفسَها في هذه
 * اللحظة — وقد وقع ذلك فعلاً: ظهر «بالعيد» في القائمة فأسقط الفحص لسببٍ
 * لا علاقة له بما يختبره. واختبارٌ يسقط بسبب مستخدمٍ يعمل اختبارٌ سيّئ.
 */
$mineOnly = fn (array $vs) => array_values(array_filter($vs,
    fn ($v) => in_array((int) $v['staff_id'], [$aId, $bId], true)));

DB::table('support_viewers')->where('thread_id', $T)
    ->whereIn('staff_id', [$aId, $bId])->delete();

/* الموظّف الأوّل يفتح المحادثة — القراءةُ نفسُها تسجّل الحضور. */
$call('GET', "/support/threads/$T", $aTok);
$viewers = $mineOnly($call('GET', "/support/threads/$T", $bTok)['body']['data']['viewers'] ?? []);
$check('الثاني يرى الأوّل داخل المحادثة',
    count($viewers) === 1 && (int) $viewers[0]['staff_id'] === $aId,
    'مشاهدون=' . count($viewers));
$check('والنصُّ صريحٌ لا رمز',
    str_contains($viewers[0]['text'] ?? '', 'يشاهد'), $viewers[0]['text'] ?? '—');

$call('POST', "/support/threads/$T/viewing", $aTok, ['state' => 'TYPING']);
$v2 = $mineOnly($call('GET', "/support/threads/$T", $bTok)['body']['data']['viewers'] ?? []);
$check('⚠ و«يكتب ردّاً» تُميَّز عن «يشاهد» — وهي ما يمنع الردّين',
    ($v2[0]['state'] ?? '') === 'TYPING' && str_contains($v2[0]['text'] ?? '', 'يكتب'),
    $v2[0]['text'] ?? '—');

$check('ولا يرى المرءُ نفسَه في القائمة',
    !collect($call('GET', "/support/threads/$T", $aTok)['body']['data']['viewers'] ?? [])
        ->contains('staff_id', $aId));

$call('POST', "/support/threads/$T/viewing", $aTok, ['state' => 'LEAVE']);
$v3 = $mineOnly($call('GET', "/support/threads/$T", $bTok)['body']['data']['viewers'] ?? []);
$check('ومن غادر يختفي فوراً', $v3 === [], 'باقٍ=' . count($v3));

/* ⚠ كشفٌ لا قفل: لا شيء يمنع الثاني من الردّ. */
DB::table('support_viewers')->insert([
    'thread_id' => $T, 'staff_id' => $aId, 'staff_name' => 'مدير SLA',
    'state' => 'TYPING', 'expires_at' => now()->addMinutes(5),
]);
$r = $call('POST', "/support/threads/$T/messages", $bTok, [
    'body' => 'ردٌّ رغم وجود آخر', 'client_id' => 'c-' . bin2hex(random_bytes(6)),
]);
$check('⚠ كشفٌ لا قفل: الثاني يردّ رغم أن الأوّل يكتب',
    $r['status'] === 200, 'status=' . $r['status']);
$msgId = (int) ($r['body']['data']['message']['id'] ?? 0);
DB::table('support_viewers')->where('thread_id', $T)->delete();

$line();
$line('── ٦) الفرز بأقرب مهلة ───────────────────────────────────────');

$r = $call('GET', '/support/threads?sort=sla', $aTok);
$check('الفرز بـSLA 200', $r['status'] === 200);
$states = array_map(fn ($i) => $i['sla']['state'], $r['body']['data']['items'] ?? []);
$rank = [SupportSla::BREACHED => 0, SupportSla::WARNING => 1, SupportSla::OK => 2, SupportSla::NONE => 3];
$ranks = array_map(fn ($s) => $rank[$s] ?? 9, $states);
$sorted = $ranks; sort($sorted);
$check('والمتأخّرُ أوّلاً', $ranks === $sorted, 'رتب=' . implode(',', $ranks));

/*
 * ⚠ والمحادثةُ تقول ما تقولُه القائمةُ عنها — وهذا فحصٌ ولد من عيب.
 *
 * `context()` كانت لا تقرأ أعمدةَ الأزمنة، فترى `evaluate` قيماً
 * خاليةً فتقول «لا مهلة» — فلا تظهر الشّارةُ في المحادثة
 * أبداً، ولا يظهر خطأٌ واحد. ولم يكشفه إلّا فتحُ الشّاشة
 * بالعين، وثمانٍ وخمسون فحصاً تمُرّ من فوقِه.
 */
$fromList = collect($r['body']['data']['items'] ?? [])->firstWhere('id', $T);
$fromConv = $call('GET', "/support/threads/$T", $aTok)['body']['data']['sla'] ?? [];
$check('⚠ والمحادثةُ تعرف مهلتَها كما تعرفُها القائمة',
    $fromList !== null
    && ($fromConv['state'] ?? '') === ($fromList['sla']['state'] ?? '')
    && ($fromConv['kind'] ?? null) === ($fromList['sla']['kind'] ?? null)
    && ($fromConv['kind'] ?? null) !== null,
    'محادثة=' . ($fromConv['kind'] ?? '—') . '/' . ($fromConv['state'] ?? '—')
    . ' · قائمة=' . ($fromList['sla']['kind'] ?? '—') . '/' . ($fromList['sla']['state'] ?? '—'));

$line();
$line('── ٧) الصلاحيات ──────────────────────────────────────────────');

/* موظّفُ دعمٍ عاديّ: يرى اللوحة ولا يضبط المعيار. */
$purge(['s.plain']);
$pId = $mk('s.plain', 'موظّف عاديّ', 'SUPPORT_AGENT');
$pTok = $call('POST', '/support/auth/login', null,
    ['username' => 's.plain', 'password' => 'Sla12345'])['body']['data']['token'] ?? null;

$check('يرى لوحة القيادة',
    $call('GET', '/support/dashboard', $pTok)['status'] === 200);
$check('⚠ ولا يضبط SLA 403 — من يوسّع المهلة يجعل الفريق «ملتزماً»',
    $call('PUT', '/support/sla/NORMAL', $pTok, ['first_minutes' => 999])['status'] === 403);
$check('ولا يرى الفريق 403',
    $call('GET', '/support/team', $pTok)['status'] === 403);
$check('ومحاولاتُه مسجَّلة',
    DB::table('support_audit')->where('staff_id', $pId)->where('action', 'DENIED')->count() >= 2);

$line();
$line('── تنظيف ─────────────────────────────────────────────────────');

if ($msgId) {
    DB::table('chat_reactions')->where('message_id', $msgId)->delete();
    DB::table('chat_stars')->where('message_id', $msgId)->delete();
    DB::table('chat_messages')->where('reply_to_id', $msgId)->update(['reply_to_id' => null]);
    DB::table('chat_messages')->where('id', $msgId)->delete();
}
DB::table('support_viewers')->where('thread_id', $T)->delete();
DB::table('support_events')->where('thread_id', $T)
    ->whereIn('actor_id', [$aId, $bId, $pId])->delete();
if ($stateBefore) {
    DB::table('support_thread_state')->where('thread_id', $T)->update([
        'status'   => $stateBefore->status,
        'priority' => $stateBefore->priority ?? 'NORMAL',
    ]);
}
DB::table('support_sla')->where('priority', 'URGENT')->update(['first_response_min' => 15]);
$purge(['s.admin', 's.two', 's.plain']);
$line('  نُظِّف: الحسابات والمشاهدون والرسائل، وأُعيدت الإعدادات.');

$line();
$line('── اللقطة المالية ────────────────────────────────────────────');
$after = $snap();
$same = true;
foreach ($before as $k => $v) {
    $eq = ($v === $after[$k]);
    if (!$eq) $same = false;
    printf("  %-10s %-10s ⇦ %-10s %s\n", $k, $v, $after[$k], $eq ? '✓' : '✗');
}
$check('لا شيء ماليّ تغيّر', $same);

$line();
$line('══════════════════════════════════════════════════════════════');
$line("   نجح: $ok    ·    أخفق: $fail");
if ($failed) $line('   المُخفِق: ' . implode(' · ', $failed));
$line('══════════════════════════════════════════════════════════════');
