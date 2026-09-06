@echo off
setlocal EnableDelayedExpansion
title Rhalla Support Center
chcp 65001 >nul

rem ============================================================
rem  يفتح مركز «الرحالة للدعم الفني».
rem
rem  انقر عليه نقرتين — لا شيء غير ذلك:
rem    1) يتأكّد أن خادم Laravel يعمل، ويشغّله إن كان متوقّفاً
rem    2) ينتظر حتى يستجيب فعلاً (لا ينتظر عدداً ثابتاً من الثواني)
rem    3) يفتح المتصفّح على الصفحة
rem
rem  ولا يشغّل خادماً ثانياً إن كان الأول يعمل: نسختان على المنفذ
rem  نفسه تعني أن الثانية تفشل بصمت والمستخدم يظنّ أنها تعمل.
rem ============================================================

set "PORT=8000"
set "URL=http://localhost:%PORT%/support"

rem ---------- 1) هل المنفذ مشغول أصلاً؟ ------------------------
set "RUNNING="
for /f "tokens=*" %%L in ('netstat -ano ^| findstr /r /c:"LISTENING" ^| findstr ":%PORT% "') do set "RUNNING=1"

if defined RUNNING (
    echo [OK] الخادم يعمل بالفعل على المنفذ %PORT%.
    goto :open
)

rem ---------- 2) تشغيله ----------------------------------------
set "PHP="
if defined RHALLA_PHP if exist "%RHALLA_PHP%" set "PHP=%RHALLA_PHP%"
if not defined PHP if exist "C:\xampp\php\php.exe" set "PHP=C:\xampp\php\php.exe"
if not defined PHP for /f "delims=" %%i in ('where php.exe 2^>nul') do if not defined PHP set "PHP=%%i"

if not defined PHP (
    echo [ERROR] لم يُعثر على PHP.
    echo         ثبّت XAMPP، أو اضبط RHALLA_PHP على مسار php.exe
    pause
    exit /b 1
)

echo [..] تشغيل الخادم...
rem  0.0.0.0 كي يُفتح من الهاتف وأي جهاز على نفس الشبكة، لا من هذا
rem  الحاسوب وحده.
cd /d "%~dp0backend"
start "Rhalla API" /min "%PHP%" artisan serve --host=0.0.0.0 --port=%PORT%

rem ---------- 3) الانتظار حتى يستجيب فعلاً ----------------------
rem  لا `timeout 5` ثابت: على جهازٍ بطيء يفتح المتصفّح قبل أن يجهز
rem  الخادم فتظهر صفحة خطأ، وعلى جهازٍ سريع ينتظر بلا داعٍ.
echo [..] في انتظار استجابة الخادم...
set "READY="
for /l %%i in (1,1,40) do (
    if not defined READY (
        powershell -NoProfile -Command "try{ $r=Invoke-WebRequest 'http://127.0.0.1:%PORT%/up' -UseBasicParsing -TimeoutSec 2; exit 0 }catch{ exit 1 }" >nul 2>&1
        if !errorlevel! equ 0 set "READY=1"
        if not defined READY ping -n 2 127.0.0.1 >nul
    )
)

if not defined READY (
    echo [ERROR] الخادم لم يستجب. افتح run_backend.bat لترى الرسالة.
    pause
    exit /b 1
)

:open
echo [OK] جاهز.
echo.
echo     %URL%
echo.
echo     اسم الدخول : admin
echo.
start "" "%URL%"
timeout /t 3 >nul
endlocal
