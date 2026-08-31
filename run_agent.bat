@echo off
chcp 65001 >nul
setlocal EnableDelayedExpansion
title تشغيل تطبيق الوكيل - الرحالة للصرافة

REM ============================================================
REM  تشغيل المحاكي + تطبيق الوكيل (rhalla_agent)
REM
REM  الاستخدام:
REM    run_agent.bat                  المحاكي الافتراضي + الخادم المحلي
REM    run_agent.bat Pixel_4_API_30   اختيار جهاز افتراضي آخر
REM    run_agent.bat prod             الخادم الإنتاجي (يطلب تأكيدًا)
REM
REM  ملاحظة: kApiBase في lib/core/net/api_client.dart يتخلّف إلى
REM  الخادم الإنتاجي. لذلك يمرّر هذا الملف API_BASE دائمًا بشكل صريح،
REM  فلا يمكن أن يبدأ التطبيق على الإنتاج بالخطأ.
REM ============================================================

set "SDK=%LOCALAPPDATA%\Android\Sdk"
set "EMULATOR=%SDK%\emulator\emulator.exe"
set "ADB=%SDK%\platform-tools\adb.exe"
set "FLUTTER=D:\Tools\SDKS\flutter\bin\flutter.bat"
set "PHP=C:\xampp\php\php.exe"

set "ROOT=%~dp0"
set "APP_DIR=%ROOT%rhalla_agent"
set "BACKEND_DIR=%ROOT%backend"

REM أدوات الـ SDK تُضاف إلى PATH وتُستدعى بلا مسار كامل: المسار يحوي
REM مسافات، و"for /f" يفشل على أمر مقتبس داخل أقواس بخطأ مضلّل
REM ("The filename, directory name, or volume label syntax is incorrect")
REM ويترك متغيّر الحلقة فارغًا بصمت.
set "PATH=%SDK%\platform-tools;%SDK%\emulator;%PATH%"

set "AVD=Rahalla_Test_API36"
set "MODE=local"

REM ---------- قراءة المعطيات ----------
set "ARG1=%~1"
set "ARG2=%~2"
if /I "%ARG1%"=="prod"  ( set "MODE=prod"  & set "ARG1=" )
if /I "%ARG1%"=="local" ( set "MODE=local" & set "ARG1=" )
if not "%ARG1%"=="" set "AVD=%ARG1%"
if not "%ARG2%"=="" set "AVD=%ARG2%"

if /I "%MODE%"=="prod" (
    set "API_BASE=http://102.214.165.242:8080/api"
) else (
    set "API_BASE=http://10.0.2.2:8000/api"
)

echo.
echo ============================================================
echo   تطبيق الوكيل — الرحالة للصرافة
echo   الجهاز الافتراضي : %AVD%
echo   الخادم           : !API_BASE!
echo ============================================================
echo.

REM ---------- التحقق من المسارات ----------
if not exist "%EMULATOR%" ( echo [خطأ] لم يُعثر على المحاكي: "%EMULATOR%" & goto :fail )
if not exist "%ADB%"      ( echo [خطأ] لم يُعثر على adb: "%ADB%"          & goto :fail )
if not exist "%FLUTTER%"  ( echo [خطأ] لم يُعثر على flutter: "%FLUTTER%"  & goto :fail )
if not exist "%APP_DIR%\pubspec.yaml" ( echo [خطأ] مجلد التطبيق غير صحيح: "%APP_DIR%" & goto :fail )

REM ---------- تأكيد الإنتاج ----------
if /I "%MODE%"=="prod" call :confirm_prod
if errorlevel 1 goto :end

REM ---------- المحاكي ----------
call :ensure_emulator
if errorlevel 1 goto :fail

REM ---------- خادم Laravel المحلي ----------
call :ensure_backend

REM ---------- تشغيل التطبيق ----------
echo [3/3] تشغيل التطبيق ...
echo.
cd /d "%APP_DIR%"
"%FLUTTER%" run -d !DEVICE! --dart-define=API_BASE=!API_BASE!
goto :end


REM ============================================================
REM  الإجراءات
REM ============================================================

:confirm_prod
    echo   *** تحذير: الهدف هو الخادم الإنتاجي ***
    echo   حسابات وأرصدة وحوالات حقيقية. أي عملية هنا تحرّك أموالًا فعلية.
    echo.
    set "OK="
    set /p "OK=اكتب  YES  للمتابعة، أو Enter للإلغاء: "
    if /I not "!OK!"=="YES" (
        echo.
        echo   أُلغيت العملية.
        exit /b 1
    )
    echo.
    exit /b 0


:ensure_emulator
    set "DEVICE="
    for /f "tokens=1" %%d in ('adb devices 2^>nul ^| findstr /r "^emulator-"') do set "DEVICE=%%d"

    if defined DEVICE (
        echo [1/3] محاكٍ يعمل بالفعل: !DEVICE!
        exit /b 0
    )

    REM التحقق من وجود الجهاز الافتراضي قبل محاولة تشغيله
    set "FOUND="
    for /f "delims=" %%a in ('emulator -list-avds 2^>nul') do (
        if /I "%%a"=="%AVD%" set "FOUND=1"
    )
    if not defined FOUND (
        echo [خطأ] لا يوجد جهاز افتراضي بالاسم "%AVD%". المتاح:
        emulator -list-avds
        exit /b 1
    )

    echo [1/3] بدء المحاكي: %AVD% ...
    start "Android Emulator" "%EMULATOR%" -avd "%AVD%" -netdelay none -netspeed full
    adb wait-for-device

    echo       بانتظار اكتمال الإقلاع ...
    set /a TRIES=0

:wait_boot
    set /a TRIES+=1
    if !TRIES! GTR 150 (
        echo [خطأ] انتهت المهلة قبل اكتمال إقلاع المحاكي.
        exit /b 1
    )
    set "BOOT="
    for /f "delims=" %%b in ('adb shell getprop sys.boot_completed 2^>nul') do set "BOOT=%%b"
    echo !BOOT! | findstr /c:"1" >nul
    if errorlevel 1 (
        timeout /t 2 /nobreak >nul 2>&1 || ping -n 3 127.0.0.1 >nul
        goto :wait_boot
    )

    set "DEVICE="
    for /f "tokens=1" %%d in ('adb devices 2^>nul ^| findstr /r "^emulator-"') do set "DEVICE=%%d"
    echo       جاهز: !DEVICE!
    exit /b 0


:ensure_backend
    if /I "%MODE%"=="prod" (
        echo [2/3] وضع الإنتاج — لا حاجة لخادم محلي.
        exit /b 0
    )

    netstat -ano | findstr ":8000" | findstr "LISTENING" >nul
    if not errorlevel 1 (
        echo [2/3] المنفذ 8000 يستمع بالفعل.
        exit /b 0
    )

    if not exist "%PHP%" (
        echo [2/3] [تنبيه] php غير موجود في "%PHP%" — شغّل الخادم يدويًّا.
        exit /b 0
    )

    echo [2/3] بدء خادم Laravel على المنفذ 8000 ...
    start "Laravel API - الرحالة" cmd /k ""%PHP%" "%BACKEND_DIR%\artisan" serve --host=0.0.0.0 --port=8000"
    timeout /t 4 /nobreak >nul 2>&1 || ping -n 5 127.0.0.1 >nul
    exit /b 0


:fail
    echo.
    echo   فشل التشغيل.
    endlocal
    exit /b 1

:end
    endlocal
    exit /b 0
