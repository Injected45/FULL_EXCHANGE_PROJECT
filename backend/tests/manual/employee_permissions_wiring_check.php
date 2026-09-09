<?php

use App\Services\Employees\EmployeePermissions;

/*
 * ⚠ يمنع عودةَ العطب: مفتاحٌ في `LIVE` بلا مسارٍ يفرضه = وعدٌ كاذب،
 * ومسارٌ يفرض مفتاحاً ليس في `LIVE` = وسمٌ خاطئ في شاشة المنح.
 *
 * والمصدرُ هو `routes/api.php` نفسُه لا قائمةٌ ثانية تُنسى.
 */

/*
 * ⚠ التعليقات تُسقَط أوّلاً: في رأس الملفّ شرحٌ مكتوبٌ
 * فيه `employee:KEY` — وفحصٌ يقرأ الشّرح على أنّه مسارٌ يرفض
 * مفتاحاً لا وجود له.
 */
$routes = preg_replace('#/\*.*?\*/#s', '', file_get_contents(base_path('routes/api.php')));
$routes = preg_replace('/^\s*\*.*$/m', '', $routes);

/* المفاتيح التي تفرضها مسارات الموظف فعلاً. */
preg_match_all("/employee:([A-Z_]+)/", $routes, $m);
$enforced = array_values(array_unique($m[1]));
sort($enforced);

$live = EmployeePermissions::LIVE;
sort($live);

/*
 * ⚠ مفاتيحُ بوّابةِ الواجهة تُستثنى — غيابُ المسار فيها مقصودٌ لا سهو.
 *
 * والاستثناءُ يُقرأ من `EmployeePermissions::UI_ONLY` لا يُكتب هنا:
 * قائمتان تفترقان يوماً ما، فيمرّ سهوٌ حقيقيّ لأن الفحصَ يستثنيه.
 */
$missingRoute = array_values(array_diff($live, $enforced,
    \App\Services\Employees\EmployeePermissions::UI_ONLY));
$missingLive  = array_values(array_diff($enforced, $live));

echo 'مفاتيح تفرضها المسارات (' . count($enforced) . '): ' . implode('، ', $enforced) . PHP_EOL;
echo 'مفاتيح موسومة موصولة (' . count($live) . '): ' . implode('، ', $live) . PHP_EOL . PHP_EOL;

$ok = true;

if ($missingRoute) {
    $ok = false;
    echo '✗ موسومةٌ «موصولة» ولا مسارَ يفرضها: ' . implode('، ', $missingRoute) . PHP_EOL;
} else {
    echo '✓ كلُّ ما وُسم موصولاً له مسارٌ يفرضه' . PHP_EOL;
}

if ($missingLive) {
    $ok = false;
    echo '✗ يفرضها مسارٌ ولم تُوسم موصولة: ' . implode('، ', $missingLive) . PHP_EOL;
} else {
    echo '✓ وكلُّ ما يفرضه مسارٌ موسومٌ موصولاً' . PHP_EOL;
}

/* وكلُّ مفتاحٍ في LIVE موجودٌ في الكتالوج أصلاً. */
$unknown = array_values(array_diff($live, array_keys(EmployeePermissions::CATALOG)));
if ($unknown) {
    $ok = false;
    echo '✗ مفاتيح خارج الكتالوج: ' . implode('، ', $unknown) . PHP_EOL;
} else {
    echo '✓ ولا مفتاحَ خارج الكتالوج' . PHP_EOL;
}

echo PHP_EOL . ($ok ? '=== PASS ===' : '=== FAIL ===') . PHP_EOL;
