<?php

/*
 * ════════════════════════════════════════════════════════════════════════════
 *  كلُّ مسارٍ يناديه التطبيق موجودٌ في الخادم — ولا مسارَ ميّت
 * ════════════════════════════════════════════════════════════════════════════
 *
 *   php artisan tinker --execute="require base_path('tests/manual/app_routes_wiring_check.php');"
 *
 * ⚠ **ما لا يمسكه أيُّ فحصٍ آخر**: خطأٌ حرفيٌّ في مسارٍ مكتوبٍ نصّاً في Dart.
 * لا التحليلُ يراه ولا الاختباراتُ — لأن المسار سلسلةُ حروفٍ لا رمزٌ يُحلَّل.
 * ولا يظهر إلّا حين يفتح وكيلٌ شاشةً فتردّ 404، وهو يظنّ التطبيق معطوباً.
 *
 * يقرأ مسارات `rhalla_agent/lib` ويقابلها بجدول مسارات لارافيل.
 */

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0;
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, $line) {
    if ($p) { $ok++;   $line('  PASS  ' . $n . ($d ? "  ($d)" : '')); }
    else    { $fail++; $line('  FAIL  ' . $n . ($d ? "  ($d)" : '')); }
};

$line('════════════════════════════════════════════════════════════');
$line(' وصلُ التطبيق بالخادم — مسارٌ مسارٌ');
$line('════════════════════════════════════════════════════════════');

/* ── ١) مسارات الخادم ─────────────────────────────────────────────────── */
$server = [];
foreach (app('router')->getRoutes() as $r) {
    $u = $r->uri();
    if (!str_starts_with($u, 'api/')) { continue; }
    $server[substr($u, 4)] = implode(',', $r->methods());
}

/* ── ٢) ما يناديه التطبيق ─────────────────────────────────────────────── */
$libDir = base_path('../rhalla_agent/lib');
if (!is_dir($libDir)) {
    $line('  SKIP  لم يُعثر على مجلّد التطبيق: ' . $libDir);
    return;
}

$calls = [];
$it = new RecursiveIteratorIterator(new RecursiveDirectoryIterator($libDir));
foreach ($it as $f) {
    if ($f->isDir() || $f->getExtension() !== 'dart') { continue; }
    $src = file_get_contents($f->getPathname());

    /*
     * ⚠ تُلتقط السلاسلُ التي تبدأ بـ`/device/` أو `/agent/` أو `/employees`
     * وحدَها — لا كلُّ سلسلة. ومسارٌ يُبنى بالتركيب (`'/x/$id/y'`) يُطبَّع
     * بإبدال ما بين `${}` بمعامل.
     */
    if (!preg_match_all(
        "~'(/(?:device|agent|employees|company|support)[^']*)'~", $src, $m)) {
        continue;
    }
    foreach ($m[1] as $path) {
        $p = ltrim($path, '/');
        // تطبيعُ التركيب: ‎${x}‎ ⇦ {p}
        $p = preg_replace('~\$\{[^}]*\}~', '{p}', $p);
        $p = preg_replace('~\$[A-Za-z_][A-Za-z0-9_]*~', '{p}', $p);
        // إسقاطُ سلسلة الاستعلام
        $p = explode('?', $p)[0];
        $p = rtrim($p, '/');
        if ($p === '') { continue; }
        $calls[$p][] = basename($f->getPathname());
    }
}

$line();
$line('── مسارات ينادِيها التطبيق: ' . count($calls));
$line();

/* ── ٣) هل كلٌّ منها مسجَّل؟ ──────────────────────────────────────────── */
$missing = [];
foreach ($calls as $path => $files) {
    $found = isset($server[$path]);

    if (!$found) {
        /*
         * مطابقةٌ بالنمط: مسارُ لارافيل قد يحمل `{id}` والتطبيق يضع `{p}`.
         * تُقارَن المقاطعُ واحداً واحداً، والمعاملُ يقابل أيَّ مقطع.
         */
        $need = explode('/', $path);
        foreach (array_keys($server) as $s) {
            $have = explode('/', $s);
            if (count($have) !== count($need)) { continue; }
            $all = true;
            foreach ($have as $i => $seg) {
                $isParam = str_starts_with($seg, '{');
                if ($isParam && $need[$i] === '{p}') { continue; }
                if ($isParam && $need[$i] !== '{p}') { continue; }
                if ($seg !== $need[$i]) { $all = false; break; }
            }
            if ($all) { $found = true; break; }
        }
    }

    if (!$found) { $missing[$path] = array_unique($files); }
}

$check('1. ⚠ كلُّ مسارٍ يناديه التطبيق مسجَّلٌ في الخادم',
    $missing === [], count($calls) . ' مساراً');

foreach ($missing as $path => $files) {
    $line('     ✗ ' . $path . '   ← ' . implode('، ', $files));
}

/* ── ٤) مسارات الموظف: كلٌّ منها خلف حارس ─────────────────────────────── */
$bare = [];
foreach (app('router')->getRoutes() as $r) {
    $u = $r->uri();
    if (!str_starts_with($u, 'api/device/employee')) { continue; }
    if (str_contains($u, 'activation')) { continue; }   // قبل الجلسة بالضرورة
    if (!str_contains(implode(',', $r->gatherMiddleware()), 'employee')) {
        $bare[] = $u;
    }
}
$check('2. ⚠ ولا مسارَ موظفٍ بلا حارس جلسة', $bare === [],
    $bare === [] ? 'نظيف' : implode(' · ', $bare));

/* ── ٥) ولا مسارَ ماليٍّ للوكيل بلا مصادقة ────────────────────────────── */
$financial = ['trans', 'transfer', 'deposit/store', 'balance', 'wallet'];
$open = [];
foreach (app('router')->getRoutes() as $r) {
    $u = $r->uri();
    if (!str_starts_with($u, 'api/')) { continue; }
    if (str_contains($u, 'device/employee')) { continue; }
    $mw = implode(',', $r->gatherMiddleware());
    if (str_contains($mw, 'auth:sanctum')) { continue; }

    foreach ($financial as $needle) {
        if (str_contains(strtolower($u), $needle)) { $open[] = $u; break; }
    }
}
/*
 * ⚠ `forgien/exchange/deposit/store` مفتوحٌ عمداً: طلبُ فتحِ حسابٍ يقدّمه
 * زبونٌ لا حساب له بعد، ويكتب في طابورِ طلباتٍ (`Table_ADD_forCostumerMobile`)
 * لا في دفترٍ ماليّ. فيُستثنى بالاسم لا بالصمت.
 */
$open = array_values(array_diff($open, ['api/device/forgien/exchange/deposit/store']));

$check('3. ⚠ ولا مسارَ ماليٍّ مفتوحٍ بلا مصادقة', $open === [],
    $open === [] ? 'نظيف' : implode(' · ', $open));

$line();
$line('════════════════════════════════════════════════════════════');
$line('  نجح: ' . $ok . '   أخفق: ' . $fail);
$line('════════════════════════════════════════════════════════════');
