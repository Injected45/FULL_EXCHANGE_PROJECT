<?php

/*
 * اختبارات قبول — الدفعة الأولى من تطوير مركز الدعم.
 * (بنود المالك 3 · 4 · 7 · 12 · 17)
 *
 *   php artisan serve --host=127.0.0.1 --port=8000     (في نافذة)
 *   php artisan tinker --execute="require base_path('tests/manual/support_ops_acceptance.php');"
 *
 * ⚠ أهمُّ ما فيه القسم السادس: **الملاحظة الداخلية لا تصل الوكيل**. وهو
 * فحصٌ من ستّ زوايا لا زاويةٍ واحدة — المحادثة، والبحث، والعدّاد، وترتيب
 * القائمة، والتثبيت، وإعادة التوجيه. لأن تسريبَها من أيٍّ منها كافٍ.
 */

use App\Services\ChatService;
use App\Services\Support\SupportPermissions;
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
$line('   مركز الدعم — الدفعة الأولى (أولوية · تصنيف · ملاحظات · شريط)');
$line('══════════════════════════════════════════════════════════════');

$snap = fn () => [
    'wallet'   => (string) DB::selectOne('SELECT COUNT(*) v FROM wallet')->v,
    'internal' => (string) DB::selectOne('SELECT COUNT(*) v FROM InternalEx')->v,
    'safe'     => (string) DB::selectOne('SELECT COUNT(*) v FROM EX24AccSafeActivityTb')->v,
    'ledger'   => (string) DB::selectOne('SELECT COUNT(*) v FROM ExchangeAccData')->v,
];
$before = $snap();

/* ─── تنظيفٌ مسبق ─────────────────────────────────────────────────── */
$purge = function (array $usernames) {
    foreach ($usernames as $u) {
        $s = DB::table('support_staff')->whereRaw('LOWER(username)=?', [$u])->first();
        if (!$s) continue;
        DB::table('support_sessions')->where('staff_id', $s->id)->delete();
        DB::table('support_permissions')->where('staff_id', $s->id)->delete();
        /* ⚠ الدفعةُ الثانية أضافت `support_viewers` بمفتاحٍ أجنبيّ إلى
           الموظّف، فحذفُه دونها يسقط بقيدٍ مرجعيّ — وهو ما أسقط تنظيفَ
           هذا الاختبار بعد أن مرّت فحوصُه كلُّها. */
        DB::table('support_viewers')->where('staff_id', $s->id)->delete();
        DB::table('support_audit')->where('staff_id', $s->id)->delete();
        DB::table('support_events')->where('actor_id', $s->id)->delete();
        DB::table('support_thread_tags')->where('added_by', $s->id)->delete();
        DB::table('chat_messages')->where('support_staff_id', $s->id)
            ->update(['support_staff_id' => null]);
        DB::table('support_thread_state')->where('assigned_to', $s->id)
            ->update(['assigned_to' => null, 'assigned_at' => null, 'assigned_by' => null]);
        DB::table('support_staff')->where('id', $s->id)->delete();
    }
};
$purge(['o.admin', 'o.agent']);

$mk = function (string $u, string $n, string $role) {
    $id = (int) DB::table('support_staff')->insertGetId([
        'name' => $n, 'username' => $u, 'password_hash' => Hash::make('Ops12345'),
        'role' => $role, 'is_active' => 1, 'must_change' => 0,
    ]);
    foreach (SupportPermissions::ROLE_DEFAULTS[$role] as $p) {
        DB::table('support_permissions')->insert([
            'staff_id' => $id, 'permission' => $p, 'granted_at' => now(),
        ]);
    }
    return $id;
};

$adminId = $mk('o.admin', 'مدير الاختبار', 'ADMIN');
$agentId = $mk('o.agent', 'موظّف الاختبار', 'SUPPORT_AGENT');

$adminTok = $call('POST', '/support/auth/login', null,
    ['username' => 'o.admin', 'password' => 'Ops12345'])['body']['data']['token'] ?? null;
$agentTok = $call('POST', '/support/auth/login', null,
    ['username' => 'o.agent', 'password' => 'Ops12345'])['body']['data']['token'] ?? null;

$thread = DB::table('chat_threads')->where('kind', 'ADMIN')->orderBy('id')->first();
$T = (int) $thread->id;
$stateBefore = DB::table('support_thread_state')->where('thread_id', $T)->first();

$svc = app(ChatService::class);

$line();
$line('── ١) التصنيفات والوسوم (البند 4) ────────────────────────────');

$r = $call('GET', '/support/taxonomy', $adminTok);
$tax = $r['body']['data'] ?? [];
$check('taxonomy 200', $r['status'] === 200, 'status=' . $r['status']);
$check('التصنيفات مزروعة', count($tax['categories'] ?? []) >= 10,
    'عدد=' . count($tax['categories'] ?? []));
$check('الوسوم مزروعة', count($tax['tags'] ?? []) >= 5, 'عدد=' . count($tax['tags'] ?? []));
$check('الأولويات أربع', count($tax['priorities'] ?? []) === 4);

/* الإدارةُ تضيف بلا تعديل شيفرة — نصّ البند. */
$newCat = 'تصنيف اختبار ' . bin2hex(random_bytes(3));
$r = $call('POST', '/support/taxonomy/categories', $adminTok,
    ['name' => $newCat, 'color' => '#123456']);
$check('الإدارة تضيف تصنيفاً بلا تعديل الكود', $r['status'] === 200, 'status=' . $r['status']);
$catNewId = (int) ($r['body']['data']['id'] ?? 0);
$check('واسمٌ مكرَّر يُرفض',
    $call('POST', '/support/taxonomy/categories', $adminTok, ['name' => $newCat])['status'] === 422);
$check('ولونٌ غير سداسيّ يُهمَل لا يُكسر',
    DB::table('support_categories')->where('id', $catNewId)->value('color') === '#123456');

$line();
$line('── ٢) الأولوية (البند 3) ─────────────────────────────────────');

$r = $call('POST', "/support/threads/$T/priority", $adminTok, ['priority' => 'URGENT']);
$check('تعيين أولوية 200', $r['status'] === 200, 'status=' . $r['status']);
$check('حُفظت في القاعدة',
    DB::table('support_thread_state')->where('thread_id', $T)->value('priority') === 'URGENT');
$check('أولوية مجهولة تُرفض 422',
    $call('POST', "/support/threads/$T/priority", $adminTok, ['priority' => 'NOPE'])['status'] === 422);

/* ⚠ ليست افتراضيةً لموظّف الدعم: من يرفع أولويةَ محادثاته كلَّها يُلغي
   معنى الأولوية على الفريق. */
$r = $call('POST', "/support/threads/$T/priority", $agentTok, ['priority' => 'HIGH']);
$check('موظّف الدعم مرفوض 403 — ليست في افتراضياته', $r['status'] === 403, 'status=' . $r['status']);
$check('ومحاولتُه مسجَّلة',
    DB::table('support_audit')->where('staff_id', $agentId)->where('action', 'DENIED')
      ->where('target', 'SET_PRIORITY')->exists());

$line();
$line('── ٣) التصنيف والوسم والرقم المرجعي ─────────────────────────');

$cat = $tax['categories'][0]['id'];
$tag = $tax['tags'][0]['id'];
$check('تصنيف المحادثة 200',
    $call('POST', "/support/threads/$T/category", $adminTok, ['category_id' => $cat])['status'] === 200);
$check('وسمُها 200',
    $call('POST', "/support/threads/$T/tags", $adminTok, ['tag_id' => $tag])['status'] === 200);

$d = $call('GET', "/support/threads/$T", $adminTok)['body']['data'] ?? [];
$check('التصنيف يظهر في المحادثة', (int) ($d['state']['category_id'] ?? 0) === (int) $cat);
$check('الوسم يظهر', count($d['state']['tags'] ?? []) === 1);
$check('الأولوية تظهر', ($d['state']['priority'] ?? '') === 'URGENT');

/* ⚠ عشوائيّ لا تسلسليّ — نصّ البند 12: «ألّا يكون قابلاً للتنبؤ». */
$ref = $d['state']['reference'] ?? '';
$check('رقمٌ مرجعيّ وُلِّد بالشكل المقصود',
    (bool) preg_match('/^RH-[2-9A-HJ-NP-Z]{8}$/', $ref), 'ref=' . ($ref ?: '—'));
$check('ولا يحوي محارفَ ملتبسة (0 O 1 I)',
    !preg_match('/[01OI]/', substr($ref, 3)));
$check('ولا يُشتقّ من رقم المحادثة', !str_contains($ref, (string) $T) || strlen((string) $T) > 3);
$check('ولا يتغيّر عند القراءة الثانية',
    ($call('GET', "/support/threads/$T", $adminTok)['body']['data']['state']['reference'] ?? '') === $ref);

$check('حذف الوسم 200',
    $call('DELETE', "/support/threads/$T/tags/$tag", $adminTok)['status'] === 200);

$line();
$line('── ٤) الشريط الزمني (البند 17) ───────────────────────────────');

$tl = $call('GET', "/support/threads/$T/timeline", $adminTok)['body']['data']['items'] ?? [];
$kinds = array_column($tl, 'kind');
foreach (['PRIORITY', 'CATEGORY', 'TAG_ADD', 'TAG_REMOVE'] as $k) {
    $check("الشريط يحوي $k", in_array($k, $kinds, true));
}
$check('كلُّ حدثٍ يحمل فاعله',
    $tl !== [] && count(array_filter($tl, fn ($e) => ($e['actor_name'] ?? '') !== '')) === count($tl));
$check('وكلُّ حدثٍ له اسمٌ عربيّ',
    $tl !== [] && count(array_filter($tl, fn ($e) => ($e['label'] ?? '') !== $e['kind'])) === count($tl));

/* ⚠ Metadata فقط — لا متنَ رسالةٍ في الشريط.
 *
 * وحدثُ الإغلاق يحمل ملاحظةَ الإغلاق التي كتبها الموظّف، وهي
 * سببُ الإغلاق لا كلامُ المحادثة. فالشرطُ ليس وجودَ نصٍّ من
 * عدمه، بل أن لا يكون النصّ متنَ رسالةٍ — وهو هنا مقاسٌ
 * بالمقارنة بمتون رسائل المحادثة نفسها. */
$bodies = DB::table('chat_messages')->where('thread_id', $T)
    ->whereNotNull('body')->pluck('body')
    ->map(fn ($b) => trim((string) $b))->filter()->values()->all();
$check('ولا يحمل نصَّ رسالة',
    !array_filter($tl, fn ($e) =>
        (mb_strlen((string) ($e['note'] ?? '')) > 0
            && !in_array($e['kind'], ['NOTE', 'CLOSED'], true))
        || in_array(trim((string) ($e['note'] ?? '')), $bodies, true)
        || in_array(trim((string) ($e['to'] ?? '')), $bodies, true)));

$line();
$line('── ٥) الملاحظة الداخلية (البند 7) — أخطرُ فحصٍ في الدفعة ─────');

/* ── ما قبل الكتابة يُقاس أوّلاً ─────────────────────────────────────
 *
 * ⚠ العدّاد و`last_message_at` يُقارَنان **قبل وبعد**، لا بقيمةٍ مطلقة:
 * المحادثة قد تحمل رسائلَ إدارةٍ سابقة لم يقرأها الوكيل، فالصفرُ ليس
 * المتوقَّع — المتوقَّع ألّا **تزيد** بالملاحظة. واختبارٌ يقيس بعد الفعل
 * وحده يمرّ أو يسقط لأسبابٍ لا علاقة لها بما يختبره.
 */
$agentId2     = (int) $thread->agent_id;
$lastAtBefore = (string) DB::table('chat_threads')->where('id', $T)->value('last_message_at');
$unreadBefore = $svc->unreadByThread([$T], 'AGENT', $agentId2)[$T] ?? 0;

$secret = 'ملاحظةٌ داخلية سرّية ' . bin2hex(random_bytes(5));
$r = $call('POST', "/support/threads/$T/messages", $adminTok, [
    'body' => $secret, 'internal' => true, 'client_id' => 'n-' . bin2hex(random_bytes(6)),
]);
$check('كتابة الملاحظة 200', $r['status'] === 200, 'status=' . $r['status']);
$nid = (int) ($r['body']['data']['message']['id'] ?? 0);
$check('عُلِّمت is_internal في القاعدة',
    (int) DB::table('chat_messages')->where('id', $nid)->value('is_internal') === 1);

$visible = array_map(fn ($m) => (int) $m->id, $svc->messages($T, 0, 200));
$check('⚠ لا تظهر في محادثة الوكيل', !in_array($nid, $visible, true));
$check('⚠ ولا في بحثه', $svc->search([$T], mb_substr($secret, 0, 14)) === []);
$check('⚠ ولا ترفع عدّاد غير المقروء عنده',
    ($svc->unreadByThread([$T], 'AGENT', $agentId2)[$T] ?? 0) === $unreadBefore,
    "قبل=$unreadBefore بعد=" . ($svc->unreadByThread([$T], 'AGENT', $agentId2)[$T] ?? 0));
$check('⚠ ولا تقفز بالمحادثة إلى رأس قائمته',
    (string) DB::table('chat_threads')->where('id', $T)->value('last_message_at')
        === $lastAtBefore);
$check('⚠ ولا تُثبَّت فتظهر في شريطه', !$svc->pinMessage($nid, $T, 'ADMIN', 7));
$check('⚠ ولا تُعاد توجيهاً إلى محادثةٍ أخرى',
    $svc->forward($nid, $T, $T, 'ADMIN', 0, 'x') === null);

$check('لكنها تظهر لمن يملك الصلاحية',
    in_array($nid, array_map(fn ($m) => (int) $m->id, $svc->messages($T, 0, 200, true)), true));
$check('وتظهر في بحثه هو',
    $svc->search([$T], mb_substr($secret, 0, 14), 40, true) !== []);

/* موظّف الدعم يملكها افتراضياً — بقرارٍ مقصود: من يعمل على المشكلة هو من
   يكتب لزميله ما وجده. فالفحصُ هنا على من **سُحبت** منه. */
$check('موظّف الدعم يكتب ملاحظةً بصلاحيته الافتراضية',
    $call('POST', "/support/threads/$T/messages", $agentTok, [
        'body' => 'ملاحظةُ موظّف', 'internal' => true,
        'client_id' => 'a-' . bin2hex(random_bytes(6)),
    ])['status'] === 200);

DB::table('support_permissions')->where('staff_id', $agentId)
    ->where('permission', 'INTERNAL_NOTES')->delete();

$r = $call('POST', "/support/threads/$T/messages", $agentTok, [
    'body' => 'محاولة', 'internal' => true, 'client_id' => 'x-' . bin2hex(random_bytes(6)),
]);
$check('⚠ ومن سُحبت منه يُرفض 403 — والسحبُ يسري فوراً بلا إعادة دخول',
    $r['status'] === 403, 'status=' . $r['status']);
$check('ومحاولتُه مسجَّلة',
    DB::table('support_audit')->where('staff_id', $agentId)->where('action', 'DENIED')
      ->where('target', 'INTERNAL_NOTES')->exists());
$check('ويبقى قادراً على الردّ العاديّ',
    $call('POST', "/support/threads/$T/messages", $agentTok, [
        'body' => 'ردٌّ عاديّ', 'client_id' => 'b-' . bin2hex(random_bytes(6)),
    ])['status'] === 200);

$check('والشريط يسجّل أن ملاحظةً كُتبت',
    in_array('NOTE', array_column(
        $call('GET', "/support/threads/$T/timeline", $adminTok)['body']['data']['items'] ?? [],
        'kind'), true));

$line();
$line('── ٦) الفرز بالأولوية ────────────────────────────────────────');

$items = $call('GET', '/support/threads', $adminTok)['body']['data']['items'] ?? [];
$ranks = array_map(fn ($i) => (int) $i['priority_rank'], $items);
$sorted = $ranks;
rsort($sorted);
$check('القائمة مرتّبة بالأولوية أوّلاً', $ranks === $sorted,
    'رتب=' . implode(',', $ranks));
$check('وكلُّ صفٍّ يحمل أولويتَه ووسومَه',
    $items === [] || (isset($items[0]['priority_label']) && array_key_exists('tags', $items[0])));

$line();
$line('── تنظيف ─────────────────────────────────────────────────────');

/* رسائل الاختبار كلُّها — لا الملاحظةُ الأولى وحدها. */
$testIds = DB::table('chat_messages')->where('thread_id', $T)
    ->where(function ($q) use ($secret) {
        $q->where('body', $secret)
          ->orWhere('body', 'ملاحظةُ موظّف')
          ->orWhere('body', 'ردٌّ عاديّ');
    })->pluck('id')->all();

foreach ($testIds as $mid) {
    DB::table('chat_reactions')->where('message_id', $mid)->delete();
    DB::table('chat_stars')->where('message_id', $mid)->delete();
}
DB::table('chat_messages')->whereIn('reply_to_id', $testIds)->update(['reply_to_id' => null]);
DB::table('chat_messages')->whereIn('id', $testIds)->delete();
DB::table('support_events')->where('thread_id', $T)
    ->whereIn('actor_id', [$adminId, $agentId])->delete();
DB::table('support_thread_tags')->where('thread_id', $T)->delete();
DB::table('support_categories')->where('id', $catNewId)->delete();

if ($stateBefore) {
    DB::table('support_thread_state')->where('thread_id', $T)->update([
        'priority'    => $stateBefore->priority ?? 'NORMAL',
        'category_id' => $stateBefore->category_id ?? null,
        'reference'   => $stateBefore->reference ?? null,
        'status'      => $stateBefore->status,
    ]);
}
$purge(['o.admin', 'o.agent']);
$line('  نُظِّف: الحسابات والرسائل والأحداث، وأُعيدت حالة المحادثة.');

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
