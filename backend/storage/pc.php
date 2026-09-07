<?php
use App\Services\Support\SupportPermissions as P;
echo 'الكتالوج: ' . count(P::CATALOG) . PHP_EOL;
foreach (['SUPPORT_AGENT','SUPERVISOR','ADMIN'] as $r) {
    $ceil = P::ROLE_CEILING[$r] ?? null;
    $def  = P::ROLE_DEFAULTS[$r] ?? [];
    $bad  = $ceil === null ? [] : array_diff($def, $ceil);
    echo "$r: سقف=" . ($ceil === null ? 'الكل' : count($ceil))
       . ' · افتراضي=' . count($def)
       . ' · خارج السقف=' . (count($bad) ? implode(',', $bad) : '0') . PHP_EOL;
}
$unknown = array_diff(array_unique(array_merge(...array_values(P::ROLE_DEFAULTS))), array_keys(P::CATALOG));
echo 'مفاتيح افتراضية خارج الكتالوج: ' . (count($unknown) ? implode(',', $unknown) : '0') . PHP_EOL;
