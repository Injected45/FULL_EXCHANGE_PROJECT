<?php

/*
 * اختبارات قبول — الدفعة الثالثة من تطوير مركز الدعم.
 * (بنود المالك 8 الردود الجاهزة · 13 التصعيد · 15 التأجيل · 16 المتابعة
 *  · 21 و37 المسودّات · 34 التسليم)
 *
 *   php artisan tinker --execute="require base_path('tests/manual/support_flow_acceptance.php');"
 *
 * ⚠ أخطرُ ما فيه فحصان:
 *
 *   • **التأجيلُ لا يوقف المهلة.** لو أوقفها لصار أسرعَ طريقةٍ إلى فريقٍ
 *     «ملتزمٍ» على الورق: يُؤجَّل ما قارب مهلتَه فيختفي الأحمر، ولا يتغيّر
 *     شيءٌ عند الوكيل الذي ما زال ينتظر.
 *
 *   • **رسالةُ الوكيل تُلغي التأجيل.** تأجيلٌ يصمد أمام رسالةٍ جديدة يعني
 *     وكيلاً يكتب ولا يراه أحد — وهو أسوأ عيبٍ ممكن في نظام دعم.
 */

use App\Services\Support\SupportFlow;
use App\Services\Support\SupportPermissions;
use App\Services\Support\SupportSla;
use App\Services\ChatService;
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
$line('   مركز الدعم — الدفعة الثالثة (سير العمل)');
$line('══════════════════════════════════════════════════════════════');

$snap = fn () => [
    'wallet'   => (string) DB::selectOne('SELECT COUNT(*) v FROM wallet')->v,
    'internal' => (string) DB::selectOne('SELECT COUNT(*) v FROM InternalEx')->v,
    'safe'     => (string) DB::selectOne('SELECT COUNT(*) v FROM EX24AccSafeActivityTb')->v,
    'accounts' => (string) DB::selectOne('SELECT COUNT(*) v FROM AccountsTb')->v,
];
$before = $snap();

$purge = function (array $u) {
    foreach ($u as $n) {
        $s = DB::table('support_staff')->whereRaw('LOWER(username)=?', [$n])->first();
        if (!$s) continue;
        DB::table('support_followups')->where('staff_id', $s->id)->delete();
        DB::table('support_drafts')->where('staff_id', $s->id)->delete();
        DB::table('support_saved_replies')->where('owner_staff_id', $s->id)->delete();
        DB::table('support_saved_replies')->where('created_by', $s->id)
            ->update(['created_by' => null]);
        DB::table('support_viewers')->where('staff_id', $s->id)->delete();
        DB::table('support_sessions')->where('staff_id', $s->id)->delete();
        DB::table('support_permissions')->where('staff_id', $s->id)->delete();
        DB::table('support_audit')->where('staff_id', $s->id)->delete();
        DB::table('support_events')->where('actor_id', $s->id)->delete();
        DB::table('chat_messages')->where('support_staff_id', $s->id)
            ->update(['support_staff_id' => null]);
        DB::table('support_thread_state')->where('assigned_to', $s->id)
            ->update(['assigned_to' => null, 'assigned_at' => null, 'assigned_by' => null]);
        DB::table('support_thread_state')->where('snoozed_by', $s->id)
            ->update(['snoozed_by' => null]);
        DB::table('support_thread_state')->where('escalated_by', $s->id)
            ->update(['escalated_by' => null]);
        DB::table('support_thread_state')->where('escalated_to', $s->id)
            ->update(['escalated_to' => null]);
        DB::table('support_staff')->where('id', $s->id)->delete();
    }
};
$purge(['f.admin', 'f.agent', 'f.super']);

$mk = function (string $u, string $n, string $role) {
    $id = (int) DB::table('support_staff')->insertGetId([
        'name' => $n, 'username' => $u, 'password_hash' => Hash::make('Flow1234'),
        'role' => $role, 'is_active' => 1, 'must_change' => 0,
    ]);
    foreach (SupportPermissions::ROLE_DEFAULTS[$role] as $p) {
        DB::table('support_permissions')->insert([
            'staff_id' => $id, 'permission' => $p, 'granted_at' => now(),
        ]);
    }
    return $id;
};

$aId = $mk('f.admin', 'مدير التدفّق', 'ADMIN');
$eId = $mk('f.agent', 'موظّف التدفّق', 'SUPPORT_AGENT');
$sId = $mk('f.super', 'مشرف التدفّق', 'SUPERVISOR');

$tok = fn ($u) => $call('POST', '/support/auth/login', null,
    ['username' => $u, 'password' => 'Flow1234'])['body']['data']['token'] ?? null;

$aTok = $tok('f.admin');
$eTok = $tok('f.agent');
$sTok = $tok('f.super');

$thread = DB::table('chat_threads')->where('kind', 'ADMIN')->orderBy('id')->first();
$T = (int) $thread->id;
$stateBefore = DB::table('support_thread_state')->where('thread_id', $T)->first();
$madeIds = [];
// يُحدّد ما كتبه هذا التّشغيل وحده عند التّنظيف.
$startedAt = now();

$state = fn () => DB::table('support_thread_state')->where('thread_id', $T)->first();

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ١) الردود الجاهزة (البند 8) ───────────────────────────────');

$r = $call('GET', '/support/saved-replies', $eTok);
$check('موظّف الدعم يقرأ القوالب', $r['status'] === 200, 'status=' . $r['status']);
$seeded = $r['body']['data']['items'] ?? [];
$check('والقوالبُ المشترَكة مزروعة', count($seeded) >= 6, 'عدد=' . count($seeded));
$check('وكلُّها مشترَكة',
    count(array_filter($seeded, fn ($i) => $i['is_shared'])) === count($seeded));

/* ⚠ القالبُ الخاصّ لا يحتاج مفتاحاً: اختصارُ يدٍ لصاحبه لا يراه أحد. */
$r = $call('POST', '/support/saved-replies', $eTok,
    ['title' => 'قالبي أنا', 'body' => 'نصٌّ خاصّ', 'is_shared' => false]);
$check('⚠ والخاصُّ يُنشأ بلا صلاحية', $r['status'] === 200, 'status=' . $r['status']);
$mine = (int) ($r['body']['data']['id'] ?? 0);

/* ⚠ والمشترَكُ يحتاجها: نصٌّ يُرسَل باسم الشركة إلى وكلائها. */
$r = $call('POST', '/support/saved-replies', $eTok,
    ['title' => 'قالبٌ للفريق', 'body' => 'نصٌّ مشترك', 'is_shared' => true]);
$check('⚠ والمشترَكُ يُرفض بلا صلاحية', $r['status'] === 422, 'status=' . $r['status']);

$r = $call('POST', '/support/saved-replies', $sTok,
    ['title' => 'قالبُ المشرف', 'body' => 'نصٌّ مشترك', 'is_shared' => true]);
$check('والمشرفُ يُنشئ المشترَك', $r['status'] === 200, 'status=' . $r['status']);
$shared = (int) ($r['body']['data']['id'] ?? 0);

/* ⚠ العزل: قالبُ زميلٍ خاصٌّ لا يظهر لغيره. */
$his = collect($call('GET', '/support/saved-replies', $sTok)['body']['data']['items'] ?? []);
$check('⚠ ولا يرى المشرفُ قالبَ الموظّف الخاصّ',
    !$his->contains('id', $mine) && $his->contains('id', $shared));

$check('ويرى الموظّفُ قالبَه والمشترَكَ معاً',
    collect($call('GET', '/support/saved-replies', $eTok)['body']['data']['items'] ?? [])
        ->contains('id', $mine));

/* ولا يعدّل قالبَ غيره. */
$r = $call('PUT', "/support/saved-replies/$shared", $eTok, ['body' => 'تحريفٌ']);
$check('⚠ ولا يعدّل الموظّفُ قالباً مشترَكاً', $r['status'] === 422, 'status=' . $r['status']);
$check('والنصُّ لم يتغيّر',
    DB::table('support_saved_replies')->where('id', $shared)->value('body') === 'نصٌّ مشترك');

/* العدّادُ يرفع الأكثرَ استعمالاً. */
$call('POST', "/support/saved-replies/$mine/used", $eTok);
$call('POST', "/support/saved-replies/$mine/used", $eTok);
$check('عدّادُ الاستعمال يزيد',
    (int) DB::table('support_saved_replies')->where('id', $mine)->value('uses') === 2,
    'uses=' . DB::table('support_saved_replies')->where('id', $mine)->value('uses'));

/* التعطيلُ لا الحذف. */
$call('PUT', "/support/saved-replies/$mine", $eTok, ['is_active' => false]);
$check('⚠ والتعطيلُ يُخفيه ولا يحذفه',
    !collect($call('GET', '/support/saved-replies', $eTok)['body']['data']['items'] ?? [])
        ->contains('id', $mine)
    && DB::table('support_saved_replies')->where('id', $mine)->exists());

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٢) المسودّات (البندان 21 و37) ─────────────────────────────');

$r = $call('PUT', "/support/threads/$T/draft", $eTok, ['body' => 'نصفُ ردٍّ لم يُرسَل']);
$check('حفظ المسودّة 200', $r['status'] === 200, 'status=' . $r['status']);

$d = $call('GET', "/support/threads/$T", $eTok)['body']['data']['draft'] ?? null;
$check('وتعود مع فتح المحادثة',
    ($d['body'] ?? '') === 'نصفُ ردٍّ لم يُرسَل', $d['body'] ?? '—');

/* ⚠ العزل: مسودّةُ موظّفٍ لا يراها زميلُه على المحادثة نفسِها. */
$d2 = $call('GET', "/support/threads/$T", $sTok)['body']['data']['draft'] ?? null;
$check('⚠ ولا يرى الزميلُ مسودّةَ زميله على المحادثة نفسِها', $d2 === null);

$call('PUT', "/support/threads/$T/draft", $sTok, ['body' => 'مسودّةُ المشرف']);
$check('وكلٌّ يحتفظ بمسودّته',
    (($call('GET', "/support/threads/$T", $eTok)['body']['data']['draft']['body'] ?? '')
        === 'نصفُ ردٍّ لم يُرسَل')
    && (($call('GET', "/support/threads/$T", $sTok)['body']['data']['draft']['body'] ?? '')
        === 'مسودّةُ المشرف'));

/* والفراغُ يمحو. */
$call('PUT', "/support/threads/$T/draft", $sTok, ['body' => '   ']);
$check('والفراغُ يمحوها ولا يحفظ نصّاً فارغاً',
    !DB::table('support_drafts')->where('thread_id', $T)->where('staff_id', $sId)->exists());

/* ⚠ والإرسالُ يمحوها — وإلّا أُرسلت مرّتين. */
$r = $call('POST', "/support/threads/$T/messages", $eTok, [
    'body' => 'ردٌّ من اختبار التدفّق',
    'client_id' => 'f-' . bin2hex(random_bytes(6)),
]);
$madeIds[] = (int) ($r['body']['data']['message']['id'] ?? 0);
$check('⚠ والإرسالُ يمحو المسودّة',
    !DB::table('support_drafts')->where('thread_id', $T)->where('staff_id', $eId)->exists());

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٣) التأجيل (البند 15) ─────────────────────────────────────');

$r = $call('POST', "/support/threads/$T/snooze", $eTok, ['until' => now()->addHours(3)->toIso8601String()]);
$check('⚠ تأجيلٌ بلا سبب يُرفض', $r['status'] === 422, 'status=' . $r['status']);

$r = $call('POST', "/support/threads/$T/snooze", $eTok,
    ['until' => now()->subHour()->toIso8601String(), 'reason' => 'ماضٍ']);
$check('وموعدٌ في الماضي يُرفض', $r['status'] === 422, 'status=' . $r['status']);

$r = $call('POST', "/support/threads/$T/snooze", $eTok,
    ['until' => now()->addDays(90)->toIso8601String(), 'reason' => 'بعيدٌ جداً']);
$check('⚠ وما تجاوز ثلاثين يوماً يُرفض — إخفاءٌ دائمٌ أسوأ من الإغلاق',
    $r['status'] === 422, 'status=' . $r['status']);

$r = $call('POST', "/support/threads/$T/snooze", $eTok,
    ['until' => now()->addHours(3)->toIso8601String(), 'reason' => 'ننتظر ردّ المصرف']);
$check('والتأجيلُ الصحيح 200', $r['status'] === 200, 'status=' . $r['status']);
$check('والسببُ محفوظ',
    $state()->snooze_reason === 'ننتظر ردّ المصرف', (string) $state()->snooze_reason);

$ids = fn ($t) => array_column($call('GET', '/support/threads', $t)['body']['data']['items'] ?? [], 'id');
$check('⚠ وتختفي من الصندوق', !in_array($T, $ids($eTok), true));
$check('وتظهر بفلترها وحدها',
    in_array($T, array_column(
        $call('GET', '/support/threads?scope=snoozed', $eTok)['body']['data']['items'] ?? [],
        'id'), true));

/*
 * ⚠ أخطرُ فحصٍ في هذه الدفعة: **التأجيلُ لا يوقف المهلة.**
 *
 * لو أوقفها لصار أسرعَ طريقةٍ إلى فريقٍ ملتزمٍ على الورق. فيُزوَّر التاريخُ
 * إلى ما قبل مهلة أوّل الردّ بكثير، ويُتحقّق من أن الحالة «متأخّرة» رغم
 * التأجيل.
 */
DB::table('support_thread_state')->where('thread_id', $T)->update([
    'priority'           => 'NORMAL',
    'first_agent_msg_at' => now()->subHours(9),
    'first_reply_at'     => null,
    'last_agent_msg_at'  => now()->subHours(9),
    'last_reply_at'      => null,
    'resolved_at'        => null,
    'status'             => 'OPEN',
]);
$ev = app(SupportSla::class)->evaluate($state());
$check('⚠ والمهلةُ تستمرّ رغم التأجيل — وإلّا صار إخفاءُ الأحمر التزاماً',
    $ev['state'] === SupportSla::BREACHED, 'حالة=' . $ev['state']);

/* ⚠ ورسالةُ الوكيل تُعيدها فوراً. */
app(ChatService::class)->send($T, ChatService::AGENT,
    (int) $thread->agent_id, 'الوكيل', 'ما زلت أنتظر');
$madeIds[] = (int) DB::table('chat_messages')->where('thread_id', $T)
    ->orderByDesc('id')->value('id');
app(\App\Services\Support\SupportThreadService::class)->onAgentMessage($T);

$check('⚠ ورسالةُ الوكيل تُلغي التأجيل — من كتب لك لا يُؤجَّل',
    $state()->snoozed_until === null);
$check('وتعود إلى الصندوق', in_array($T, $ids($eTok), true));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٤) التصعيد (البند 13) ─────────────────────────────────────');

DB::table('support_thread_state')->where('thread_id', $T)->update(['priority' => 'NORMAL']);

$r = $call('POST', "/support/threads/$T/escalate", $eTok, ['reason' => '']);
$check('⚠ تصعيدٌ بلا سبب يُرفض', $r['status'] === 422, 'status=' . $r['status']);

$r = $call('POST', "/support/threads/$T/escalate", $eTok,
    ['reason' => 'المبلغ لم يصل والوكيل ينتظر منذ الصباح']);
$check('التصعيد 200', $r['status'] === 200, 'status=' . $r['status']);
$check('⚠ ويرفع درجةً واحدة لا يقفز إلى «حرجة»',
    $state()->priority === 'HIGH', 'أولوية=' . $state()->priority);
$check('والسببُ محفوظ', (string) $state()->escalation_reason !== '');

/* ⚠ والسببُ يُكتب ملاحظةً داخلية في المحادثة نفسِها. */
$note = DB::table('chat_messages')->where('thread_id', $T)
    ->where('is_internal', 1)->orderByDesc('id')->first(['id', 'body']);
if ($note) $madeIds[] = (int) $note->id;
$check('⚠ والسببُ يُكتب في المحادثة لا في شاشةٍ جانبية',
    $note && str_contains((string) $note->body, 'ينتظر منذ الصباح'));
$check('وداخليةً — لا يراها الوكيل',
    $note && (int) DB::table('chat_messages')->where('id', $note->id)
        ->value('is_internal') === 1);

/* التصعيدُ مرّتين يصل «عاجلة» ثم «حرجة» ثم يقف. */
$call('POST', "/support/threads/$T/escalate", $aTok, ['reason' => 'مرّة ثانية']);
$call('POST', "/support/threads/$T/escalate", $aTok, ['reason' => 'مرّة ثالثة']);
$r = $call('POST', "/support/threads/$T/escalate", $aTok, ['reason' => 'مرّة رابعة']);
$check('ويقف عند «حرجة» ولا يُرفض بعدها',
    $r['status'] === 200 && $state()->priority === 'CRITICAL',
    'أولوية=' . $state()->priority);

/* ⚠ والتصعيدُ يُلغي التأجيل: «انظر إليها الآن» و«أخفِها» تناقض. */
$call('POST', "/support/threads/$T/snooze", $aTok,
    ['until' => now()->addHours(2)->toIso8601String(), 'reason' => 'مؤقّت']);
$call('POST', "/support/threads/$T/escalate", $aTok, ['reason' => 'عاد الأمر عاجلاً']);
$check('⚠ والتصعيدُ يُلغي التأجيل — لا تُخفى حالةٌ صُعِّدت',
    $state()->snoozed_until === null);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٥) التسليم بين الموظّفين (البند 34) ───────────────────────');

$r = $call('POST', "/support/threads/$T/handoff", $aTok,
    ['to_staff_id' => $sId, 'note' => '']);
$check('⚠ تسليمٌ بلا ملاحظة يُرفض', $r['status'] === 422, 'status=' . $r['status']);

$r = $call('POST', "/support/threads/$T/handoff", $aTok, ['to_staff_id' => $aId, 'note' => 'لي']);
$check('ولا يُسلَّم المرءُ إلى نفسه', $r['status'] === 422, 'status=' . $r['status']);

$r = $call('POST', "/support/threads/$T/handoff", $aTok,
    ['to_staff_id' => $sId, 'note' => 'راجع كشف الحساب معه، أنا خارج الدوام']);
$check('التسليم 200', $r['status'] === 200, 'status=' . $r['status']);
$check('والإسنادُ انتقل', (int) $state()->assigned_to === $sId,
    'مالك=' . $state()->assigned_to);

$hn = DB::table('chat_messages')->where('thread_id', $T)->where('is_internal', 1)
    ->orderByDesc('id')->first(['id', 'body']);
if ($hn) $madeIds[] = (int) $hn->id;
$check('⚠ والملاحظةُ تصل مع الحالة — لا يقرأ المستلمُ المحادثةَ كلَّها',
    $hn && str_contains((string) $hn->body, 'خارج الدوام'));

/* ⚠ وموظّفُ الدعم لا يُسلّم: التسليمُ إسنادٌ إلى غيرِك. */
$r = $call('POST', "/support/threads/$T/handoff", $eTok,
    ['to_staff_id' => $aId, 'note' => 'خذها']);
$check('⚠ وموظّفُ الدعم لا يملك التسليم (ASSIGN_OTHERS)',
    $r['status'] === 403, 'status=' . $r['status']);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٦) المتابعات (البند 16) ───────────────────────────────────');

$r = $call('POST', "/support/threads/$T/followup", $eTok,
    ['due_at' => now()->addDay()->toIso8601String(), 'note' => 'اتصل به غداً']);
$check('جدولة متابعة 200', $r['status'] === 200, 'status=' . $r['status']);
$fu = (int) ($r['body']['data']['id'] ?? 0);

$mineFu = $call('GET', '/support/followups', $eTok)['body']['data']['items'] ?? [];
$check('وتظهر لصاحبها', collect($mineFu)->contains('id', $fu));
$check('ومعها اسمُ الوكيل لا رقمُ المحادثة وحده',
    ($mineFu[0]['agent_name'] ?? '') !== '' && !str_starts_with($mineFu[0]['agent_name'], 'محادثة #'),
    $mineFu[0]['agent_name'] ?? '—');

/* ⚠ والفرقُ عن التأجيل: لا تُخفي المحادثة عن أحد. */
$check('⚠ والمتابعةُ لا تُخفي المحادثة عن الفريق — بخلاف التأجيل',
    in_array($T, $ids($sTok), true) && $state()->snoozed_until === null);

$check('⚠ ولا يراها زميلُه', !collect(
    $call('GET', '/support/followups', $sTok)['body']['data']['items'] ?? [])->contains('id', $fu));

$r = $call('POST', "/support/followups/$fu/done", $sTok);
$check('⚠ ولا يُنهيها زميلُه',
    ($r['body']['data']['changed'] ?? true) === false
    && DB::table('support_followups')->where('id', $fu)->value('done_at') === null);

$call('POST', "/support/followups/$fu/done", $eTok);
$check('وصاحبُها يُنهيها',
    DB::table('support_followups')->where('id', $fu)->value('done_at') !== null);
$check('⚠ وتُختَم ولا تُحذف — سجلُّ لماذا عاد إليها يبقى',
    DB::table('support_followups')->where('id', $fu)->exists());

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٧) الشريط الزمني يحوي الجديد ──────────────────────────────');

$tl = $call('GET', "/support/threads/$T/timeline", $aTok)['body']['data']['items'] ?? [];
$kinds = array_column($tl, 'kind');
foreach ([SupportFlow::EV_HANDOFF, SupportFlow::EV_ESCALATED,
          SupportFlow::EV_FOLLOWUP, 'SNOOZED', 'UNSNOOZED'] as $k) {
    $check("الشريط يحوي $k", in_array($k, $kinds, true));
}
$check('⚠ وكلُّ حدثٍ جديدٍ له اسمٌ عربيّ لا رمزُه الإنجليزيّ',
    count(array_filter($tl, fn ($e) => ($e['label'] ?? '') !== $e['kind'])) === count($tl));

// ══════════════════════════════════════════════════════════════════
$line();
$line('── ٨) الكلفة ─────────────────────────────────────────────────');

DB::flushQueryLog();
DB::enableQueryLog();
$call('GET', "/support/threads/$T", $aTok);
DB::disableQueryLog();

$q = 0;
DB::flushQueryLog();
DB::enableQueryLog();
app(SupportFlow::class)->savedReplies($aId);
$q = count(DB::getQueryLog());
DB::disableQueryLog();
$check('⚠ والقوالبُ استعلامٌ واحد لا اثنان', $q === 1, 'استعلامات=' . $q);

DB::flushQueryLog();
DB::enableQueryLog();
app(SupportFlow::class)->followups($aId);
$q2 = count(DB::getQueryLog());
DB::disableQueryLog();
$check('والمتابعاتُ استعلامان: هي وأسماءُ وكلائها',
    $q2 <= 2, 'استعلامات=' . $q2);

// ══════════════════════════════════════════════════════════════════
$line();
$line('── تنظيف ─────────────────────────────────────────────────────');

foreach (array_filter($madeIds) as $mid) {
    DB::table('chat_reactions')->where('message_id', $mid)->delete();
    DB::table('chat_stars')->where('message_id', $mid)->delete();
    DB::table('chat_messages')->where('reply_to_id', $mid)->update(['reply_to_id' => null]);
    DB::table('chat_messages')->where('id', $mid)->delete();
}
DB::table('support_saved_replies')->whereIn('id', array_filter([$mine, $shared]))->delete();
DB::table('support_events')->where('thread_id', $T)
    ->whereIn('actor_id', [$aId, $eId, $sId])->delete();
// والحدثُ التّلقائيّ لا فاعِل له في `support_staff`، فلا يطالُه
// التّنظيفُ بمُعرّف الفاعِل — ويبقى فيكسر اختباراً آخر.
DB::table('support_events')->where('thread_id', $T)
    ->whereNull('actor_id')
    ->whereIn('kind', ['UNSNOOZED'])
    ->where('created_at', '>=', $startedAt)->delete();
DB::table('support_drafts')->where('thread_id', $T)->delete();

if ($stateBefore) {
    DB::table('support_thread_state')->where('thread_id', $T)->update([
        'status'             => $stateBefore->status,
        'priority'           => $stateBefore->priority ?? 'NORMAL',
        'assigned_to'        => $stateBefore->assigned_to,
        'assigned_at'        => $stateBefore->assigned_at,
        'assigned_by'        => $stateBefore->assigned_by,
        'snoozed_until'      => null,
        'snoozed_at'         => null,
        'snoozed_by'         => null,
        'snooze_reason'      => null,
        'escalated_at'       => null,
        'escalated_by'       => null,
        'escalated_to'       => null,
        'escalation_reason'  => null,
        'first_agent_msg_at' => $stateBefore->first_agent_msg_at,
        'first_reply_at'     => $stateBefore->first_reply_at,
        'last_agent_msg_at'  => $stateBefore->last_agent_msg_at,
        'last_reply_at'      => $stateBefore->last_reply_at,
        'resolved_at'        => $stateBefore->resolved_at,
    ]);
}
$purge(['f.admin', 'f.agent', 'f.super']);
$line('  نُظِّف: الحسابات والقوالب والمسودّات والرسائل، وأُعيدت حالةُ المحادثة.');

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
