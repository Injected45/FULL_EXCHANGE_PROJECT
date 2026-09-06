<?php

use Illuminate\Foundation\Application;
use Illuminate\Foundation\Configuration\Exceptions;
use Illuminate\Foundation\Configuration\Middleware;

return Application::configure(basePath: dirname(__DIR__))
    ->withRouting(
        web: __DIR__.'/../routes/web.php',
        api: __DIR__.'/../routes/api.php',
        commands: __DIR__.'/../routes/console.php',
        health: '/up',
    )
    ->withMiddleware(function (Middleware $middleware) {
        // حارس جلسة الموظف — منفصل تماماً عن `auth:sanctum`.
        // الفصل مقصود: رمز موظف لا يفتح مسار مسؤول ولو أخطأ أحدٌ في الوسم.
        //
        // وحارس موظّف الدعم ثالثٌ منفصل عنهما: ثلاثةُ فضاءاتِ جلساتٍ لا
        // يفتح أحدُها الآخر، لأن الخلط بينها هو الطريقُ إلى أن تصير جلسةُ
        // دعمٍ يوماً جلسةَ وكيل.
        $middleware->alias([
            'employee' => \App\Http\Middleware\AuthenticateEmployee::class,
            'support'  => \App\Http\Middleware\AuthenticateSupport::class,
        ]);
    })
    ->withExceptions(function (Exceptions $exceptions) {
        //
        $exceptions->shouldRenderJsonWhen(function ($request, $e) {
            return true; 
        });
    })->create();
