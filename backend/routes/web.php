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
Route::prefix('admin/chat')->group(function () {
    Route::get ('/login',  [AdminChatController::class, 'login']);
    Route::post('/login',  [AdminChatController::class, 'doLogin']);
    Route::get ('/logout', [AdminChatController::class, 'logout']);

    Route::get ('/',              [AdminChatController::class, 'index']);
    Route::post('/{id}/send',     [AdminChatController::class, 'send'])->whereNumber('id');
    Route::get ('/{id}/poll',     [AdminChatController::class, 'poll'])->whereNumber('id');
});
