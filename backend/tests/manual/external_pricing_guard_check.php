<?php

/*
 * ════════════════════════════════════════════════════════════════════════════
 *  حارسُ تسعيرِ الحوالة الخارجية — سعرٌ واحد، ولا حوالةَ بالسالب
 * ════════════════════════════════════════════════════════════════════════════
 *
 *   php artisan tinker --execute="require base_path('tests/manual/external_pricing_guard_check.php');"
 *
 * أمرُ المالك (11 سبتمبر 2026): «ننشئ شرطاً وحارساً يمنع أيَّ حوالةٍ بالسالب …
 * وإذا كان فرقُ الأسعار في صالح الشركة والربح + يُنفَّذ دون أيّ مشاكل … ولا
 * مساسَ بأيّ حوالةٍ تمّ تنفيذها حتى وإن كانت بالسالب».
 *
 * ⚠ **هذا الفحصُ لا يكتب شيئاً.** لا حوالةَ تجريبية، ولا صفَّ يُدرَج ولا
 * يُعدَّل. يُقاس على البيانات القائمة وعلى الدوالّ نفسِها — لأنّ إدراجَ حوالةٍ
 * لإثبات أنّ الحارسَ يعمل هو بعينه ما يحرسه الحارس.
 */

use App\Services\CurrencyRatesService;
use App\Services\ExternalPricingGuard;
use Illuminate\Support\Facades\DB;

$line = fn ($s = '') => print($s . PHP_EOL);
$ok = 0; $fail = 0;
$check = function (string $n, bool $p, string $d = '') use (&$ok, &$fail, $line) {
    if ($p) { $ok++;   $line('  PASS  ' . $n . ($d ? "  ($d)" : '')); }
    else    { $fail++; $line('  FAIL  ' . $n . ($d ? "  ($d)" : '')); }
};

$line('════════════════════════════════════════════════════════════');
$line(' حارسُ تسعيرِ الحوالة الخارجية');
$line('════════════════════════════════════════════════════════════');

$guard = app(ExternalPricingGuard::class);
$rates = app(CurrencyRatesService::class);

/* ── لقطةٌ ماليّة قبل ─────────────────────────────────────────────────── */
$snapshot = fn () => [
    'ExternalEx'  => (int) DB::table('ExternalEx')->count(),
    'InternalEx'  => (int) DB::table('InternalEx')->count(),
    'SafeActivity' => (int) DB::table('EX24AccSafeActivityTb')->count(),
    'wallet'      => (string) (DB::table('wallet')->sum('Walet') ?? 0),
];
$before = $snapshot();

/* ── ١) سعرٌ واحدٌ لا اثنان ────────────────────────────────────────────
 *
 * شاشةُ «أسعار العملات» تعرض رقماً، والحارسُ يسعّر به، والدالّةُ تحسب به.
 * فإن افترقا، عرضنا على الوكيل سعراً ونفّذنا بغيره — وهو العطبُ الأصليّ
 * نفسُه في ثوبٍ جديد.
 */
$board = $rates->list();

$mismatch = [];
foreach ($board as $r) {
    $key = $r['country_id'] . ':' . $r['service_id'];
    $g   = $guard->rate((int) $r['country_id'], (int) $r['service_id']);

    if ($r['status'] === 'PRICED') {
        // سعرٌ معروضٌ ⇒ لا بدّ أن يكون هو سعرَ الحارس بعينه.
        if ($g === null || abs($g - (float) $r['rate']) > 1e-9) {
            $mismatch[] = "$key: شاشة={$r['rate']} حارس=" . var_export($g, true);
        }
        continue;
    }

    /*
     * ⚠ والاتجاهُ الآخرُ لازمٌ كلزوم الأوّل: سطرٌ تقول عنه الشاشةُ «غير متاح»
     * والحارسُ يسعّره يعني وكيلاً يرفض حوالةً يقبلها الخادم — وهو عكسُ العطب
     * ومثلُه في الضرر: خدمةٌ معطَّلةٌ بلا سبب.
     */
    if ($g !== null) {
        $mismatch[] = "$key: شاشة={$r['status']} بينما الحارسُ يسعّرها بـ$g";
    }
}
$check(
    '1. ⚠ الشاشةُ والحارسُ يقولان الشيءَ نفسَه — سعراً ومنعاً',
    $mismatch === [],
    $mismatch === []
        ? count($board) . ' سطراً (سعرٌ ومنعٌ معاً)'
        : implode(' · ', $mismatch)
);

/* ── ٢) الحارسُ شبكةُ أمانٍ لا عائق ────────────────────────────────────
 *
 * ⚠ لو اعترض عملاً سليماً لتوقّف العملُ يومَ النشر. فيُسأل عن كلّ خدمةٍ
 * معروضةٍ فعلاً (`ExtTraServiceTypeTb`) وعلى مدى مبالغَ واسع.
 */
$services = DB::select('SELECT ID, ServiceName, CountryID FROM ExtTraServiceTypeTb ORDER BY CountryID, ID');
$amounts  = [1, 5, 10, 25, 50, 60, 100, 250, 500, 1000, 2500, 5000, 10000];

$blocked = [];
$priced  = 0;
foreach ($services as $s) {
    if ($guard->rate((int) $s->CountryID, (int) $s->ID) === null) {
        continue;                       // اقترانٌ غيرُ مسعَّر — يُفحص في (٣)
    }
    $priced++;
    foreach ($amounts as $a) {
        $v = $guard->evaluate((int) $s->CountryID, (int) $s->ID, (float) $a);
        if (!$v['ok']) {
            $blocked[] = trim($s->ServiceName) . " @ $a = " . round($v['margin'], 6);
        }
    }
}
$check(
    '2. ⚠ لا خدمةً مسعَّرةً تُمنع عند أيّ مبلغ',
    $blocked === [],
    $blocked === []
        ? "$priced خدمةً × " . count($amounts) . ' مبلغاً'
        : implode(' · ', array_slice($blocked, 0, 4))
);

/* ── ٣) خدمةٌ بلا سعرٍ تُمنع، ولا تستعير سعرَ جارتها ──────────────────── */
$unpriced = [];
foreach ($services as $s) {
    if ($guard->rate((int) $s->CountryID, (int) $s->ID) === null) {
        $v = $guard->evaluate((int) $s->CountryID, (int) $s->ID, 100.0);
        $unpriced[] = [trim($s->ServiceName), $s->CountryID, $v['ok'], $v['reason']];
    }
}
$allRefused = true;
foreach ($unpriced as $u) {
    if ($u[2] !== false || $u[3] !== 'NO_PRICE') { $allRefused = false; }
}
$check(
    '3. ⚠ الاقترانُ غيرُ المسعَّر يُمنع بـNO_PRICE',
    $allRefused,
    $unpriced === []
        ? 'لا اقترانَ غيرَ مسعَّرٍ اليوم'
        : implode(' · ', array_map(fn ($u) => "$u[0] (بلد $u[1])", $unpriced))
);

/* ── ٤) العتبةُ محصورةٌ بين الضجيج والخسارة الحقيقية ─────────────────────
 *
 * ⚠ هذا ما يمنع عطبين متضادّين:
 *   بلا عتبةٍ  ⇒ كلُّ حوالةٍ إلى السودان تُمنع بعجزٍ وهميٍّ (-2.9e-13).
 *   بعتبةٍ كبيرة ⇒ تمرّ خسارةٌ حقيقية.
 * والرقمان مقيسان على القاعدة لا مفترضان: الضجيجُ من ضربِ 555.555،
 * وأصغرُ خسارةٍ في الصفوف المعطوبة كانت 5.000.
 */
$eps = ExternalPricingGuard::EPSILON;
$check(
    '4. ⚠ العتبةُ تمرّر ضجيجَ الفاصلة العائمة وتمنع أصغرَ خسارةٍ مقيسة',
    (-2.9e-13 >= -$eps) && (-5.0 < -$eps) && ($eps > 0) && ($eps < 0.01),
    "EPSILON = $eps"
);

/* ── ٥) الصفرُ يمرّ ───────────────────────────────────────────────────
 *
 * نصُّ الأمر منعُ **السالب**، وأكثرُ الخدمات اليوم مسعَّرةٌ بلا حسمٍ فهامشُها
 * صفر. ومنعُ الصفر كان سيوقف العملَ كلَّه.
 */
$zeroCount = 0;
foreach ($services as $s) {
    $r = $guard->rate((int) $s->CountryID, (int) $s->ID);
    if ($r === null) { continue; }
    $v = $guard->evaluate((int) $s->CountryID, (int) $s->ID, 100.0);
    if (abs($v['margin']) <= $eps && $v['ok']) { $zeroCount++; }
}
$check('5. ⚠ الهامشُ صفراً يمرّ — ليس خسارة', $zeroCount > 0, "$zeroCount خدمةً بهامشٍ صفر");

/* ── ٦) الحارسُ البعديُّ كان سيمنع التسعَ المعطوبة ─────────────────────
 *
 * ⚠ أقوى ما في هذا الفحص: يُقاس على الصفوف التي وقعت فعلاً. الحارسُ الذي
 * يُضاف بعد الإدراج وقبل `commit` يقرأ `ServiceExVal` المكتوب — فلو كان
 * قائماً يومَها ما اعتُمدت واحدةٌ منها.
 *
 * ⚠ ولا تُمَسّ: أمرُ المالك «ولا مساسَ بأيّ حوالةٍ تمّ تنفيذها حتى وإن كانت
 * بالسالب». تُقرأ فقط.
 */
$negatives = DB::select(
    'SELECT Code, ServiceExVal FROM ExternalEx WHERE Code IS NOT NULL AND ServiceExVal < ?',
    [-$eps]
);
$wouldStop = true;
foreach ($negatives as $n) {
    if (!((float) $n->ServiceExVal < -$eps)) { $wouldStop = false; }
}
$check(
    '6. ⚠ كلُّ صفٍّ سالبٍ قائمٍ كان الحارسُ البعديُّ سيمنعه',
    $wouldStop,
    count($negatives) . ' صفّاً سالباً — تُقرأ ولا تُمَسّ'
);

/* ── ٧) نصُّ الاعتذار بكلمات المالك حرفاً بحرف ─────────────────────── */
$check(
    '7. ⚠ نصُّ الرسالة كما أمر المالك حرفاً بحرف',
    ExternalPricingGuard::REFUSAL
        === 'نأسف لعدم اتمام التحويل . الاسعار في طور التحديث عليك مراجعة الشركة',
    ExternalPricingGuard::REFUSAL
);

/* ── ٨) ولا سعرَ يُقرأ من خارج الحارس في مسارات الحوالة الخارجية ────────
 *
 * ⚠ فحصٌ بنيويّ لا سلوكيّ: منفذٌ يُكتب غداً بالطريقة القديمة يسقط هنا، بدل
 * أن ينتظر حتى يخسر وكيلٌ مالاً. والاستثناءُ يُسمّى بسببه، لا يُترك صمتاً.
 */
$sources = [
    'app/Http/Controllers/Api/depositController.php',
    'app/Http/Controllers/Api/EmployeeController.php',
    'app/Services/CurrencyRatesService.php',
];

/*
 * ⚠ الاستثناءُ **باسم الدالّة** لا باسم الملفّ.
 *
 * استثناءُ ملفٍّ كاملٍ كان سيغطّي `transInsertExternal` و`externalQuote` معاً —
 * أي يُعمي الفحصَ عن المسارين اللذين وُضع لأجلهما.
 */
$exemptFunctions = [
    /*
     * `CurrencyRatesService::list` — شاشةُ العرض. تقرأ القائمة كلَّها دفعةً
     * واحدة (لا اقتراناً بعينه)، ولا تسعّر حوالةً ولا يناديها مسارٌ يكتب.
     * وتطابقُها مع الحارس يُقاس في الفحص (١) أعلاه رقماً برقم — وهو أقوى من
     * منعِ الاستعلام.
     */
    'list',

    /*
     * `externalGetExchnage` — نقطةٌ قديمة، ولا يناديها هذا التطبيق إطلاقاً
     * (مُقاس: لا وجودَ للمسار في rhalla_agent/lib إلّا في تعليقٍ يحذّر منه).
     * تقرأ بنطاق AccountType=3 بلا AppBriceTB، و`sale_price` فيها موصوفةٌ في
     * CLAUDE.md بأنها لا تصلح لتسعير زبون.
     *
     * ⚠ وتُترك كما هي عمداً: لها مستهلكون خارج هذا المشروع (تطبيقٌ قديم أو
     * سطحُ المكتب)، وتغييرُ ما لا نعرف مستهلكيه ليس إصلاحاً. وهي **خارج مسار
     * إنشاء الحوالة الخارجية** في هذا التطبيق، فلا تمسّ «المعروض = المنفَّذ».
     * بُلِّغ المالكُ بها.
     */
    'externalGetExchnage',
];

$rogue = [];
foreach ($sources as $f) {
    $src = @file_get_contents(base_path($f));
    if ($src === false) { continue; }

    /*
     * ⚠ يُمشى على الرموز لا على النصّ: `token_get_all` يفصل التعليقاتِ عن
     * الشيفرة، فذكرُ اسم الجدول في توثيقٍ يشرح العطبَ لا يُحسب مخالفة —
     * وإلّا لأخفق الفحصُ على **توثيق إصلاحه نفسِه**.
     */
    $toks = token_get_all($src);
    $fn = '(عامّ)';

    foreach ($toks as $i => $t) {
        if (!is_array($t)) { continue; }

        if ($t[0] === T_FUNCTION) {
            /*
             * ⚠ لا يُشترط `T_STRING` لاسم الدالّة: `list` كلمةٌ محجوزة في PHP
             * فتأتي `T_LIST`، و`CurrencyRatesService::list` اسمُها كذلك —
             * فكانت تُقرأ «مجهولة» ويسقط استثناؤها المُسمّى. يُؤخذ أوّلُ رمزٍ
             * شكلُه معرِّفٌ صحيح بدل تثبيت نوعه.
             */
            $fn = '(مجهولة)';
            for ($j = $i + 1; $j < $i + 8 && isset($toks[$j]); $j++) {
                if (!is_array($toks[$j])) { continue; }
                if ($toks[$j][0] === T_WHITESPACE) { continue; }
                if (preg_match('/^[A-Za-z_]\w*$/', $toks[$j][1])) {
                    $fn = $toks[$j][1];
                }
                break;
            }
            continue;
        }

        if ($t[0] === T_COMMENT || $t[0] === T_DOC_COMMENT) { continue; }

        if (str_contains($t[1], 'NewCurrencyPriceOwnDetailsTb')
            && !in_array($fn, $exemptFunctions, true)) {
            $rogue[] = basename($f) . "::$fn";
        }
    }
}
$rogue = array_values(array_unique($rogue));
$check(
    '8. ⚠ لا دالّةَ تقرأ جدولَ الأسعار خارجَ الحارس إلّا باستثناءٍ مُسمّى',
    $rogue === [],
    $rogue === [] ? 'نظيف — والاستثناءان مُسمّيان بسببهما' : implode(' · ', $rogue)
);

/* ── ٩) لم يتحرّك شيءٌ ماليّ ──────────────────────────────────────────── */
$after = $snapshot();
$same  = $before == $after;
$check(
    '9. ⚠ الفحصُ لم يكتب شيئاً — لقطةٌ ماليّة متطابقة',
    $same,
    $same ? 'ExternalEx/InternalEx/القيود/المحافظ كما هي' : json_encode(['قبل' => $before, 'بعد' => $after], JSON_UNESCAPED_UNICODE)
);

$line();
$line('════════════════════════════════════════════════════════════');
$line("  نجح: $ok   أخفق: $fail");
$line('════════════════════════════════════════════════════════════');
