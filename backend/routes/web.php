<?php

use App\Http\Controllers\AdminChatController;
use Illuminate\Support\Facades\Route;

Route::get('/', function () {
    return view('welcome');
});

/*
|--------------------------------------------------------------------------
| صندوق وارد الإدارة — ردّ الإدارة على رسائل الوكلاء
|--------------------------------------------------------------------------
|
| صفحة ويب على خادم الشركة، لأن موظّف الإدارة يحتاج أن يردّ والمنظومة
| المكتبية مرجعٌ لا يُعدَّل. حارسها مفتاحٌ في `.env` — انظر توثيق
| `AdminChatController`، وفيه لماذا مفتاحٌ مشترك وما حدوده.
|
| ⚠ لا شيء هنا يمسّ المال: قراءةٌ وكتابةٌ في جداول الدردشة وحدها.
|
*/
/*
|--------------------------------------------------------------------------
| مركز «الرحالة للدعم الفني» — الصفحة
|--------------------------------------------------------------------------
|
| صفحةُ React مبنيّة في `public/support/`، تُقدَّم من هنا. ولا شيء يُقرَّر في
| هذا المسار: التوثيق كلُّه في `/api/support/*` خلف الحارس `support`، وهذه
| ترسل ملفّاً ثابتاً لا غير.
|
| بلا حارسٍ هنا عمداً — الصفحة نفسها لا تحمل بياناً، وأوّل ما تفعله أن تسأل
| `auth/me`؛ فمن لا رمزَ له يرى شاشة الدخول. ووضعُ حارسٍ عليها كان يعني
| جلسةَ ويب ثانية إلى جانب رموز الـ API، ومكانين للحقيقة.
|
| ⚠ ولا شيء هنا يمسّ المال.
|
*/
Route::get('/support', function () {
    $index = public_path('support/index.html');

    abort_unless(is_file($index), 404, 'واجهة الدعم غير مبنيّة على هذا الخادم.');

    return response(file_get_contents($index))
        ->header('Content-Type', 'text/html; charset=utf-8')
        // الصفحةُ نفسها لا تُخزَّن: الأصولُ تحتها مبصومةٌ بالاسم وتُخزَّن
        // طويلاً، لكنّ `index.html` هو ما يشير إليها — وتخزينُه يعني نسخةً
        // قديمة تطلب أصولاً حُذفت بعد أوّل تحديث.
        ->header('Cache-Control', 'no-cache, must-revalidate');
});

Route::prefix('admin/chat')->group(function () {
    Route::get ('/login',  [AdminChatController::class, 'login']);
    Route::post('/login',  [AdminChatController::class, 'doLogin']);
    Route::get ('/logout', [AdminChatController::class, 'logout']);

    Route::get ('/',              [AdminChatController::class, 'index']);
    Route::post('/{id}/send',     [AdminChatController::class, 'send'])->whereNumber('id');
    Route::get ('/{id}/poll',     [AdminChatController::class, 'poll'])->whereNumber('id');
});
