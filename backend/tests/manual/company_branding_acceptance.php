<?php

/*
 * اختبارات قبول «هوية الشركة» (White-Label).
 *
 * يُشغَّل من backend/:
 *     php artisan tinker --execute="require 'tests/manual/company_branding_acceptance.php';"
 *
 * أهمّها البند 10: تخصيص الهوية طبقة عرض — يجب ألّا يحرّك رصيداً ولا قيداً
 * ولا حوالة. وهو شرط المالك الصريح.
 */

use App\Services\BrandingThemes;
use App\Services\CompanyBrandingService;
use Illuminate\Support\Facades\DB;

$svc = app(CompanyBrandingService::class);

$line = fn($s) => print($s . PHP_EOL);
$ok = 0; $fail = 0;
$check = function (string $name, bool $pass, string $detail = '') use (&$ok, &$fail, $line) {
    if ($pass) { $ok++;   $line("  PASS  $name" . ($detail ? "  ($detail)" : '')); }
    else       { $fail++; $line("  FAIL  $name" . ($detail ? "  ($detail)" : '')); }
};

$companyA = 530;   /* «جاري شركة الامانة» */
$companyB = 993;   /* شركة أخرى — تُستعمل لاختبار العزل */
$userId   = 104;

$line('=== company branding — acceptance ===');

/* ---------- بصمة مالية قبل أي شيء ---------- */
$snapshot = function () {
    return [
        'wallet'    => (string) DB::table('wallet')->selectRaw('ISNULL(SUM(Walet),0) s')->value('s'),
        'ledger'    => DB::table('ExchangeAccData')->count(),
        'internal'  => DB::table('InternalEx')->count(),
        'users'     => DB::table('users')->count(),
    ];
};
$before = $snapshot();

/* ---------- 1. لا هوية محفوظة ⇒ افتراضية لا خطأ ---------- */
DB::table('tenant_branding')->whereIn('company_account_id', [$companyA, $companyB])->delete();
$d = $svc->forCompany($companyA);
$check('1. شركة بلا هوية تعود بالافتراضية',
    $d['theme_key'] === BrandingThemes::DEFAULT_KEY && $d['logo_url'] === null,
    'theme=' . $d['theme_key']);

/* ---------- 2. الحفظ يعمل ويرفع العدّاد ---------- */
$v0 = $d['branding_version'];
$saved = $svc->save($companyA, $userId, [
    'company_name_ar' => 'شركة الأمانة للحوالات المالية',
    'theme_key'       => 'blue_corporate',
], ['ip' => '127.0.0.1', 'device' => 'test']);
$check('2. الحفظ يثبت الاسم والثيم',
    $saved['company_name_ar'] === 'شركة الأمانة للحوالات المالية'
    && $saved['theme_key'] === 'blue_corporate');
$check('3. العدّاد يرتفع بعد الحفظ',
    $saved['branding_version'] > $v0,
    "$v0 → " . $saved['branding_version']);

/* ---------- 4. العزل: شركة أخرى لا ترى شيئاً ---------- */
$other = $svc->forCompany($companyB);
$check('4. عزل الشركات — B لا ترى هوية A',
    $other['company_name_ar'] === null
    && $other['theme_key'] === BrandingThemes::DEFAULT_KEY);

/* ---------- 5. صفٌّ واحد لكل شركة مهما تكرّر الحفظ ---------- */
$svc->save($companyA, $userId, ['theme_key' => 'gold_premium'], []);
$svc->save($companyA, $userId, ['theme_key' => 'emerald'], []);
$rows = DB::table('tenant_branding')->where('company_account_id', $companyA)->count();
$check('5. صفٌّ واحد لكل شركة', $rows === 1, "rows=$rows");

/* ---------- 6. سجلّ التدقيق يسجّل التغيّر فقط ---------- */
$audit = DB::table('tenant_branding_audit')
    ->where('company_account_id', $companyA)
    ->where('field_name', 'theme_key')
    ->orderByDesc('id')->first();
$check('6. التدقيق يحفظ القيمة القديمة والجديدة',
    $audit && $audit->old_value === 'gold_premium' && $audit->new_value === 'emerald',
    $audit ? "{$audit->old_value} → {$audit->new_value}" : 'no row');

/* حفظٌ بلا تغيير فعليّ لا يكتب صفّ تدقيق. */
$auditCountBefore = DB::table('tenant_branding_audit')->where('company_account_id', $companyA)->count();
$svc->save($companyA, $userId, ['theme_key' => 'emerald'], []);
$auditCountAfter = DB::table('tenant_branding_audit')->where('company_account_id', $companyA)->count();
$check('7. حفظٌ بلا تغيير لا يلوّث سجلّ التدقيق',
    $auditCountBefore === $auditCountAfter,
    "$auditCountBefore → $auditCountAfter");

/* ---------- 8. ألوان الحالات ثابتة في كل الثيمات ---------- */
$statusStable = true;
foreach (array_keys(BrandingThemes::all()) as $key) {
    $b = $svc->save($companyA, $userId, ['theme_key' => $key], []);
    if ($b['colors']['status'] !== BrandingThemes::STATUS_COLORS) { $statusStable = false; }
}
$check('8. ألوان النجاح/الخطأ/التحذير لا تتغيّر بتغيّر الثيم', $statusStable);

/* ---------- 9. تباين النص فوق اللون الأساسي ---------- */
$contrastOk = BrandingThemes::readableTextOn('#FFFFFF') === '#1B2733'
           && BrandingThemes::readableTextOn('#101614') === '#FFFFFF'
           && BrandingThemes::readableTextOn('#00875A') === '#FFFFFF';
$check('9. لون النصّ يُحسب ليبقى مقروءاً', $contrastOk);

/* ---------- 10. الاستعادة تُرجع الثيم ولا تمحو الاسم ---------- */
$reset = $svc->resetTheme($companyA, $userId, []);
$check('10. الاستعادة تُرجع الثيم الافتراضي وتُبقي الاسم',
    $reset['theme_key'] === BrandingThemes::DEFAULT_KEY
    && $reset['company_name_ar'] === 'شركة الأمانة للحوالات المالية');

/* ---------- 11. مفتاح ثيم مجهول لا يكسر التطبيق ---------- */
DB::table('tenant_branding')->where('company_account_id', $companyA)
    ->update(['theme_key' => 'no_such_theme']);
$bad = $svc->forCompany($companyA);
$check('11. ثيم مجهول يسقط إلى الافتراضي بلا خطأ',
    $bad['colors']['primary'] === BrandingThemes::get(BrandingThemes::DEFAULT_KEY)['primary']);
DB::table('tenant_branding')->where('company_account_id', $companyA)
    ->update(['theme_key' => BrandingThemes::DEFAULT_KEY]);

/* ---------- 12. مسار الشعار يرفض الخروج من المجلد ---------- */
$check('12. اسمٌ فيه مسار يُرفض',
    $svc->logoStream('../.env') === null && $svc->logoStream('a/b.png') === null);

/* ---------- 13. البصمة المالية لم تتغيّر ---------- */
$after = $snapshot();
$check('13. لا أثر مالي إطلاقاً — الأرصدة والقيود والحوالات كما هي',
    $before == $after,
    json_encode(['before' => $before, 'after' => $after], JSON_UNESCAPED_UNICODE));

$line('');
$line("=== PASS: $ok   FAIL: $fail ===");
