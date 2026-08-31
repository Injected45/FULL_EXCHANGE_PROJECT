@echo off
chcp 65001 >nul
setlocal EnableDelayedExpansion
title بناء APK للشبكة المحلية - الرحالة للصرافة

rem ============================================================
rem  build-apk-production.bat
rem
rem  يبني APK موجّهًا إلى الواجهة الخلفية على شبكة الـ Wi-Fi المحلية
rem  بدل 10.0.2.2 الخاص بالمحاكي، فيعمل على هواتف حقيقية متصلة بنفس
rem  الشبكة ويصل كل منها إلى Laravel على هذا الجهاز.
rem
rem  الاستخدام:
rem    build-apk-production.bat                     كشف IP تلقائيًا، منفذ 8000، وضع debug
rem    build-apk-production.bat 192.168.1.12        تحديد العنوان يدويًا
rem    build-apk-production.bat 192.168.1.12 8000   تحديد العنوان والمنفذ
rem    build-apk-production.bat profile             بناء profile: أداء أقرب للإصدار
rem    build-apk-production.bat check               فحص فقط: هل عنوان الـ APK الحالي ما زال صحيحًا
rem    build-apk-production.bat auto                لا يعيد البناء إلا إذا تغيّر العنوان
rem    build-apk-production.bat install             يركّب الناتج على الأجهزة الموصولة بلا سؤال
rem    set RHALLA_YES=1                             لتخطي أسئلة التأكيد
rem
rem  تغيّر عنوان الجهاز:
rem    العنوان يُدمج داخل الـ APK عند البناء ولا يمكن تغييره بعده، فإن
rem    أعطى الراوتر الجهاز عنوانًا جديدًا تعطّلت كل النسخ الموزّعة دفعة
rem    واحدة. لذلك يسجّل السكربت عنوان آخر بناء في ملف .info.txt بجوار
rem    الـ APK، ويقارنه بالعنوان الحالي في كل تشغيل ويقول لك صراحةً
rem    أتغيّر أم لا. الحل الجذري ليس هنا بل في الراوتر: احجز للجهاز
rem    عنوانًا ثابتًا - DHCP reservation - فلا يتغيّر أصلًا.
rem
rem  لماذا ليس بناء release؟
rem    1) الخادم المحلي يعمل على http صريح، وأندرويد يمنع cleartext في
rem       بناء الإصدار. السماح به مقصور هنا على مجموعتَي المصادر debug
rem       و profile فلا يصل إلى الإصدار إطلاقًا. لا تنقله إلى src/main:
rem       ذلك يجعل التطبيق المنشور ينقل الأرصدة ورموز الحوالات بلا تشفير.
rem    2) بناء الإصدار يحتاج android\key.properties وهو غير موجود عمدًا.
rem  للإصدار الحقيقي استخدم build_apk.bat بعد تركيب شهادة TLS على الخادم.
rem
rem  يعدّل هذا الملف network_security_config مؤقتًا ليضيف عنوان الشبكة
rem  المحلية، ثم يعيده إلى أصله عند الانتهاء - وعند الفشل أيضًا.
rem ============================================================

set "ROOT=%~dp0"
set "APP_DIR=%ROOT%rhalla_agent"
set "OUTDIR=%ROOT%apk"

set "DBG_XML=%APP_DIR%\android\app\src\debug\res\xml\network_security_config.xml"
set "PRO_RES=%APP_DIR%\android\app\src\profile\res"
set "PRO_DIR=%PRO_RES%\xml"
set "PRO_XML=%PRO_DIR%\network_security_config.xml"
set "PRO_MAN=%APP_DIR%\android\app\src\profile\AndroidManifest.xml"

set "BK_XML=%TEMP%\rhalla_nsc_debug.bak"
set "BK_MAN=%TEMP%\rhalla_profile_manifest.bak"
set "MARK=%TEMP%\rhalla_lan_build.lock"
set "CHK=%TEMP%\rhalla_apk_check.txt"
set "TMPIP=%TEMP%\rhalla_lanip.txt"

rem ---------- 0) استرجاع ما خلّفه تشغيل مقطوع ------------------
rem  لو أُغلقت النافذة في منتصف البناء تبقى ملفات المصادر معدّلة.
rem  نعيدها هنا قبل أي شيء، وإلا نسخنا الملف المولَّد كأنه الأصل.
if exist "%MARK%" (
    echo [تنبيه] تشغيل سابق لم يكتمل - إعادة ملفات المصادر إلى أصلها.
    call :restore
    echo.
)

rem ---------- 1) قراءة المعطيات -------------------------------
set "IP="
set "PORT="
set "MODE="
set "AUTO="
set "CHECKONLY="
set "DOINSTALL="

:parse
if "%~1"=="" goto :parsed
set "A=%~1"
if /i "!A!"=="debug"   ( set "MODE=debug"   & shift & goto :parse )
if /i "!A!"=="profile" ( set "MODE=profile" & shift & goto :parse )
if /i "!A!"=="auto"    ( set "AUTO=1"       & shift & goto :parse )
if /i "!A!"=="check"   ( set "CHECKONLY=1"  & shift & goto :parse )
if /i "!A!"=="install" ( set "DOINSTALL=1"  & shift & goto :parse )
if /i "!A!"=="release" (
    echo [خطأ] بناء release لا يستطيع الاتصال بخادم http على الشبكة المحلية.
    echo        أندرويد يمنع cleartext في الإصدار، وبناء الإصدار يحتاج مفاتيح توقيع.
    echo        استخدم debug أو profile هنا، و build_apk.bat للإصدار الحقيقي.
    goto :fail
)
echo(!A!| findstr /r /c:"^[0-9][0-9]*\.[0-9][0-9]*\.[0-9][0-9]*\.[0-9][0-9]*$" >nul
if not errorlevel 1 ( set "IP=!A!" & shift & goto :parse )
echo(!A!| findstr /r /c:"^[0-9][0-9]*$" >nul
if not errorlevel 1 ( set "PORT=!A!" & shift & goto :parse )
echo [خطأ] معطى غير مفهوم: "!A!"
echo        الاستخدام: build-apk-production.bat [IP] [PORT] [debug ^| profile] [auto ^| check] [install]
goto :fail

:parsed
if not defined MODE set "MODE=debug"
if not defined PORT set "PORT=8000"

rem ---------- 2) أدوات البناء ---------------------------------
if not exist "%APP_DIR%\pubspec.yaml" (
    echo [خطأ] مجلد التطبيق غير صحيح: "%APP_DIR%"
    goto :fail
)

set "FLUTTER="
for /f "delims=" %%i in ('where flutter.bat 2^>nul') do if not defined FLUTTER set "FLUTTER=%%i"
if not defined FLUTTER for /f "delims=" %%i in ('where flutter 2^>nul') do if not defined FLUTTER set "FLUTTER=%%i"
if not defined FLUTTER if exist "D:\Tools\SDKS\flutter\bin\flutter.bat" set "FLUTTER=D:\Tools\SDKS\flutter\bin\flutter.bat"
if not defined FLUTTER (
    echo [خطأ] لم يُعثر على flutter لا في PATH ولا في D:\Tools\SDKS\flutter\bin.
    goto :fail
)

rem  adb يُضاف إلى PATH ويُستدعى بلا مسار كامل: مساره يحوي مسافات،
rem  و "for /f" على أمر مقتبس داخل أقواس يفشل بخطأ مضلّل ويترك متغيّر
rem  الحلقة فارغًا بصمت.
set "ADBFULL="
if exist "%LOCALAPPDATA%\Android\Sdk\platform-tools\adb.exe" (
    set "ADBFULL=%LOCALAPPDATA%\Android\Sdk\platform-tools\adb.exe"
    set "PATH=%LOCALAPPDATA%\Android\Sdk\platform-tools;%PATH%"
)
if not defined ADBFULL for /f "delims=" %%i in ('where adb 2^>nul') do if not defined ADBFULL set "ADBFULL=%%i"

rem ---------- 3) عنوان هذا الجهاز على الشبكة -------------------
rem  البطاقة التي لها بوّابة افتراضية هي المتّصلة بالشبكة المحلية فعلًا،
rem  لا عناوين WSL أو VirtualBox أو المحاكي - وهي ما يراه الهاتف.
if not defined IP (
    echo البحث عن عنوان هذا الجهاز على الشبكة ...
    powershell -NoProfile -ExecutionPolicy Bypass -Command "$c = Get-NetIPConfiguration | Where-Object { $_.IPv4DefaultGateway -ne $null -and $_.NetAdapter.Status -eq 'Up' } | Select-Object -First 1; $ip = ''; if ($c) { $ip = @($c.IPv4Address)[0].IPAddress }; Set-Content -Path '%TMPIP%' -Value $ip -Encoding ASCII -NoNewline" >nul 2>&1
    if exist "%TMPIP%" (set /p IP=<"%TMPIP%")
    del "%TMPIP%" >nul 2>&1
)

if not defined IP (
    echo [خطأ] تعذّر كشف عنوان الشبكة تلقائيًا.
    echo        شغّل  ipconfig  وخذ IPv4 Address من بطاقة الـ Wi-Fi، ثم:
    echo            build-apk-production.bat 192.168.1.12
    goto :fail
)

echo(!IP!| findstr /r /c:"^[0-9][0-9]*\.[0-9][0-9]*\.[0-9][0-9]*\.[0-9][0-9]*$" >nul
if errorlevel 1 (
    echo [خطأ] العنوان "!IP!" ليس عنوان IPv4 صالحًا.
    goto :fail
)

echo(!IP!| findstr /r /c:"^127\." /c:"^10\.0\.2\.2$" >nul
if not errorlevel 1 (
    echo [خطأ] "!IP!" عنوان محلي لا يراه الهاتف:
    echo        127.x هو الجهاز نفسه، و 10.0.2.2 خاص بالمحاكي وحده.
    echo        نحتاج عنوان الجهاز على شبكة الـ Wi-Fi - مثل 192.168.1.12.
    goto :fail
)

set "PRIVATE="
echo(!IP!| findstr /r /c:"^192\.168\." /c:"^10\." /c:"^172\.1[6-9]\." /c:"^172\.2[0-9]\." /c:"^172\.3[01]\." >nul
if not errorlevel 1 set "PRIVATE=1"

set "API_BASE=http://!IP!:!PORT!/api"
set "FINAL=%OUTDIR%\rhalla-agent-lan-!MODE!.apk"
set "STATE=%OUTDIR%\rhalla-agent-lan-!MODE!.info.txt"

rem ---------- 4) هل تغيّر العنوان منذ آخر بناء؟ -----------------
rem  الـ APK لا يحمل عنوانه في مكان يسهل قراءته، فنسجّله بجواره عند
rem  البناء. الملف نفسه يفيد من يستلم الـ APK: يعرف منه بأي خادم يتكلّم.
set "LAST_API="
set "LAST_BUILT="
if exist "!STATE!" (
    for /f "usebackq tokens=1,* delims==" %%a in ("!STATE!") do (
        if "%%a"=="API_BASE" set "LAST_API=%%b"
        if "%%a"=="BUILT"    set "LAST_BUILT=%%b"
    )
)

set "NEEDBUILD=1"
set "CHANGED="
if not exist "!FINAL!" (
    set "REASON=لا يوجد APK سابق بوضع !MODE!."
) else if not defined LAST_API (
    set "REASON=يوجد APK لكن عنوانه غير مسجّل - يُعاد بناؤه ليصير معروفًا."
) else if /i "!LAST_API!"=="!API_BASE!" (
    set "NEEDBUILD="
    set "REASON=العنوان لم يتغيّر منذ آخر بناء: !LAST_API!"
) else (
    set "CHANGED=1"
    set "REASON=العنوان تغيّر:  !LAST_API!  ==^>  !API_BASE!"
)

rem ---------- 5) وضع الفحص وحده --------------------------------
rem  يخرج بـ 1 حين لا يعود الـ APK صالحًا، فيصلح للاستدعاء من سكربت آخر.
rem  التفرّع ليس داخل قوسين لأن exit /b داخل كتلة لا يضبط رمز الخروج.
if not defined CHECKONLY goto :aftercheck
echo.
echo ============================================================
echo   فحص عنوان الـ APK - وضع !MODE!
echo ============================================================
echo   عنوان الجهاز الآن : !API_BASE!
if defined LAST_API (
    echo   عنوان الـ APK     : !LAST_API!
    echo   تاريخ بنائه       : !LAST_BUILT!
) else (
    echo   عنوان الـ APK     : غير مسجّل
)
echo.
if defined NEEDBUILD goto :check_stale
echo   !REASON!
echo   الـ APK الحالي ما زال صالحًا.
endlocal
exit /b 0

:check_stale
echo   !REASON!
if defined CHANGED (
    echo   كل نسخة موزّعة صارت لا تتصل بالخادم. أعد البناء ثم أعد تركيبها
    echo   على كل هاتف.
) else (
    echo   أعد التشغيل بلا "check" ليُبنى ويُسجَّل عنوانه.
)
endlocal
exit /b 1

:aftercheck


rem ---------- 6) تأكيد الهدف ----------------------------------
echo.
echo ============================================================
echo   تطبيق الوكيل - بناء APK للشبكة المحلية
echo   نوع البناء : !MODE!
echo   الخادم     : !API_BASE!
echo   المخرجات   : %OUTDIR%
echo ============================================================
echo.
echo  !REASON!
if defined CHANGED (
    echo  كل نسخة موزّعة من الـ APK القديم توقّفت الآن، لا نسخة هذا الجهاز
    echo  وحدها. أعد تركيب الناتج على كل هاتف بعد البناء.
    echo  ولتفادي التكرار: احجز للجهاز عنوانًا ثابتًا في الراوتر.
)
echo.
if not defined PRIVATE (
    echo [تنبيه] "!IP!" ليس ضمن نطاقات الشبكات الخاصة المعتادة.
    echo         تأكد أنه العنوان الذي تصل إليه الهواتف على نفس الشبكة.
    echo.
)

if defined AUTO if not defined NEEDBUILD (
    echo  وضع auto: لا إعادة بناء.
    echo  لو كنت غيّرت كود التطبيق فأعد التشغيل بلا "auto" - المقارنة تخص
    echo  العنوان وحده ولا تعرف شيئًا عن تغيّر الشيفرة.
    goto :verify
)

if not "%RHALLA_YES%"=="1" (
    choice /c YN /n /m "المتابعة بهذه الإعدادات؟ [Y/N] "
    if errorlevel 2 (
        echo أُلغيت العملية.
        goto :end
    )
)
echo.

rem ---------- 7) السماح بـ HTTP الصريح لهذا العنوان -------------
rem  الإعداد الأصلي يسمح بـ 10.0.2.2 فقط، فهاتف على 192.168.x.x يُمنع
rem  من الاتصال بلا خطأ مفهوم: الطلب يفشل كأن الخادم غير موجود.
echo تجهيز إعداد الشبكة لعنوان !IP! ...
echo build in progress > "%MARK%"

if /i "!MODE!"=="debug" (
    if not exist "%DBG_XML%" (
        echo [خطأ] ملف الإعداد غير موجود: "%DBG_XML%"
        goto :fail
    )
    copy /y "%DBG_XML%" "%BK_XML%" >nul
    call :write_nsc "%DBG_XML%"
) else (
    if not exist "%PRO_MAN%" (
        echo [خطأ] ملف المانيفست غير موجود: "%PRO_MAN%"
        goto :fail
    )
    copy /y "%PRO_MAN%" "%BK_MAN%" >nul
    if not exist "%PRO_DIR%" mkdir "%PRO_DIR%"
    call :write_nsc "%PRO_XML%"
    call :write_profile_manifest
)

rem ---------- 8) البناء ---------------------------------------
cd /d "%APP_DIR%"

call "%FLUTTER%" pub get
if errorlevel 1 (
    call :restore
    echo [خطأ] فشل flutter pub get.
    goto :fail
)

echo.
echo بناء APK بوضع !MODE! ...
call "%FLUTTER%" build apk --!MODE! --dart-define=API_BASE=!API_BASE!
set "RC=!errorlevel!"

rem  الاسترجاع أولًا وقبل أي فحص: ملفات المصادر لا تُترك معدّلة مهما حدث.
call :restore

if not "!RC!"=="0" (
    echo.
    echo [خطأ] فشل البناء.
    echo        إن اشتكى Gradle من تراخيص الـ SDK:  flutter doctor --android-licenses
    goto :fail
)

set "BUILT=build\app\outputs\flutter-apk\app-!MODE!.apk"
if not exist "!BUILT!" (
    echo [خطأ] البناء نجح لكن "!BUILT!" غير موجود.
    goto :fail
)

if not exist "%OUTDIR%" mkdir "%OUTDIR%"
copy /y "!BUILT!" "!FINAL!" >nul
if errorlevel 1 (
    echo [خطأ] تعذّر نسخ الـ APK إلى "!FINAL!".
    goto :fail
)

rem  تسجيل العنوان فور نجاح النسخ: هو ما تقرأه المقارنة في التشغيل
rem  التالي، وما يعرف منه مستلم الـ APK بأي خادم يتكلّم هذا الملف.
> "!STATE!" (
    echo # من إنتاج build-apk-production.bat - لا تحرّره يدويًا.
    echo # يصف الـ APK المجاور: بأي خادم يتكلّم، ومتى بُني.
    echo API_BASE=!API_BASE!
    echo IP=!IP!
    echo PORT=!PORT!
    echo MODE=!MODE!
    echo APK=!FINAL!
    echo BUILT=%DATE% %TIME%
)

rem ---------- 9) فحص الـ APK نفسه ------------------------------
rem  لا نثق بنجاح البناء: نسخة إصدار سابقة شُحنت بلا صلاحية INTERNET
rem  وما كانت تفتح مقبسًا أصلًا. والعنوان قد لا يدخل الإعداد لو فشلت
rem  الكتابة بصمت - والنتيجة تطبيق يبدو سليمًا ولا يتصل بشيء.
:verify
if not exist "!FINAL!" (
    echo [خطأ] لا يوجد APK للفحص: "!FINAL!"
    goto :fail
)

powershell -NoProfile -ExecutionPolicy Bypass -Command "Add-Type -AssemblyName System.IO.Compression.FileSystem; $z=[IO.Compression.ZipFile]::OpenRead('!FINAL!'); $g={param($n) $e=$z.GetEntry($n); if($e -eq $null){return $null}; $s=$e.Open(); $ms=New-Object IO.MemoryStream; $s.CopyTo($ms); $s.Close(); return ,$ms.ToArray()}; $m=(& $g 'AndroidManifest.xml'); if($m -eq $null){'PERM=MISSING'}else{$t=[Text.Encoding]::Unicode.GetString($m)+[Text.Encoding]::GetEncoding(28591).GetString($m); if($t.Contains('android.permission.INTERNET')){'PERM=OK'}else{'PERM=FAIL'}}; $n=(& $g 'res/xml/network_security_config.xml'); if($n -eq $null){'NSC=MISSING'}else{$u=[Text.Encoding]::Unicode.GetString($n)+[Text.Encoding]::GetEncoding(28591).GetString($n); if($u.Contains('!IP!')){'NSC=OK'}else{'NSC=FAIL'}}; $z.Dispose()" > "%CHK%" 2>nul

set "V_PERM=?"
set "V_NSC=?"
if exist "%CHK%" (
    for /f "usebackq tokens=1,2 delims==" %%a in ("%CHK%") do (
        if "%%a"=="PERM" set "V_PERM=%%b"
        if "%%a"=="NSC"  set "V_NSC=%%b"
    )
    del "%CHK%" >nul 2>&1
)

echo.
if "!V_PERM!"=="OK" (
    echo فحص الصلاحيات    : INTERNET معلنة.
) else if "!V_PERM!"=="?" (
    echo [تنبيه] تعذّر فحص صلاحيات الـ APK.
) else (
    echo.
    echo [خطأ] الـ APK لا يعلن android.permission.INTERNET - لن يفتح أي اتصال.
    echo        راجع android\app\src\main\AndroidManifest.xml
    goto :fail
)

if "!V_NSC!"=="OK" (
    echo فحص إعداد الشبكة : !IP! مسموح له بـ HTTP.
) else if "!V_NSC!"=="?" (
    echo [تنبيه] تعذّر فحص إعداد الشبكة داخل الـ APK.
) else (
    echo.
    echo [خطأ] العنوان !IP! غير موجود في إعداد شبكة الـ APK.
    echo        سيفشل كل طلب على الهاتف بخطأ اتصال غامض. أعد التشغيل.
    goto :fail
)

rem ---------- 10) الخادم: هل يستمع على كل البطاقات؟ -------------
rem  php artisan serve بلا --host=0.0.0.0 يستمع على 127.0.0.1 وحده،
rem  فيعمل على هذا الجهاز ولا يراه أي هاتف.
set "LISTEN_ALL="
set "LISTEN_ANY="
for /f "delims=" %%l in ('netstat -an ^| findstr /i "LISTENING" ^| findstr /c:":!PORT! "') do (
    set "LISTEN_ANY=1"
    echo %%l| findstr /c:"0.0.0.0:!PORT!" >nul
    if not errorlevel 1 set "LISTEN_ALL=1"
)

if defined LISTEN_ALL (
    echo فحص الخادم       : يستمع على 0.0.0.0:!PORT! - الهواتف تصل إليه.
) else if defined LISTEN_ANY (
    echo [تنبيه] المنفذ !PORT! مفتوح لكن على 127.0.0.1 فقط، فلا يراه أي هاتف.
    echo         أوقفه وشغّل:  run_backend.bat !PORT!
    echo         وهو يستدعي:  php artisan serve --host=0.0.0.0 --port=!PORT!
) else (
    echo [تنبيه] لا شيء يستمع على المنفذ !PORT! الآن.
    echo         شغّل الخادم قبل تجربة التطبيق:  run_backend.bat !PORT!
)

rem ---------- 11) جدار الحماية ---------------------------------
rem  ويندوز يمنع الوارد افتراضيًا: الخادم يستمع ومع ذلك ينتهي طلب
rem  الهاتف بمهلة. هذا أكثر سبب يُظن معه أن التطبيق معطوب.
rem  والسماح قد يأتي من قاعدة منفذ أو من قاعدة برنامج على php.exe -
rem  وويندوز ينشئ الثانية بنفسه عند أول تشغيل للخادم إن وافق المستخدم
rem  على نافذته. نقرأ الاثنتين: فحص المنفذ وحده يحذّر من عائق غير موجود.
set "PHPEXE="
if defined RHALLA_PHP if exist "%RHALLA_PHP%" set "PHPEXE=%RHALLA_PHP%"
if not defined PHPEXE if exist "C:\xampp\php\php.exe" set "PHPEXE=C:\xampp\php\php.exe"
if not defined PHPEXE for /f "delims=" %%i in ('where php.exe 2^>nul') do if not defined PHPEXE set "PHPEXE=%%i"
if not defined PHPEXE set "PHPEXE=-"

set "FW="
set "NETCAT="
powershell -NoProfile -ExecutionPolicy Bypass -Command "$cat = 'private'; $p = Get-NetConnectionProfile | Select-Object -First 1; if ($p) { if ($p.NetworkCategory -eq 'Public') { $cat = 'public' } elseif ($p.NetworkCategory -eq 'DomainAuthenticated') { $cat = 'domain' } }; $ok = { param($r) $r.Direction -eq 'Inbound' -and $r.Action -eq 'Allow' -and $r.Enabled -eq 'True' -and ($r.Profile -eq 'Any' -or $r.Profile -match $cat) }; $port = @(Get-NetFirewallPortFilter | Where-Object { $_.LocalPort -eq '!PORT!' } | Get-NetFirewallRule | Where-Object { & $ok $_ }); $prog = @(Get-NetFirewallApplicationFilter | Where-Object { $_.Program -ieq '!PHPEXE!' } | Get-NetFirewallRule | Where-Object { & $ok $_ }); if ($port.Count -gt 0) { 'FW=PORT' } elseif ($prog.Count -gt 0) { 'FW=PROGRAM' } else { 'FW=NONE' }; ('CAT=' + $cat)" > "%CHK%" 2>nul
if exist "%CHK%" (
    for /f "usebackq tokens=1,2 delims==" %%a in ("%CHK%") do (
        if "%%a"=="FW"  set "FW=%%b"
        if "%%a"=="CAT" set "NETCAT=%%b"
    )
    del "%CHK%" >nul 2>&1
)
if not defined NETCAT set "NETCAT=private"

set "FWRULE=RhallaLanApi!PORT!"
set "FWCMD=netsh advfirewall firewall add rule name=!FWRULE! dir=in action=allow protocol=TCP localport=!PORT! profile=!NETCAT!"

if "!FW!"=="PORT" (
    echo فحص جدار الحماية : قاعدة منفذ تسمح بـ !PORT! واردًا.
    goto :installstep
)
if "!FW!"=="PROGRAM" (
    echo فحص جدار الحماية : قاعدة برنامج تسمح بالوارد إلى
    echo                    !PHPEXE!
    echo                    فلا حاجة لقاعدة منفذ ما دام الخادم يعمل بهذا الملف نفسه.
    goto :installstep
)
if not "!FW!"=="NONE" (
    echo [تنبيه] تعذّر فحص جدار الحماية - تأكّد يدويًا إن لم يصل الهاتف.
    goto :installstep
)

echo.
echo [تنبيه] لا توجد قاعدة تسمح بالمنفذ !PORT! واردًا، ولا قاعدة برنامج لـ php.
echo         بدونها يستمع الخادم ولا يصله شيء من الهواتف.
echo         تصنيف الشبكة الحالية: !NETCAT!
echo.
if "%RHALLA_YES%"=="1" goto :fw_manual

choice /c YN /n /m "إضافة القاعدة الآن؟ سيطلب ويندوز صلاحيات المدير [Y/N] "
if errorlevel 2 goto :fw_manual

powershell -NoProfile -ExecutionPolicy Bypass -Command "Start-Process netsh -Verb RunAs -WindowStyle Hidden -Wait -ArgumentList 'advfirewall firewall add rule name=!FWRULE! dir=in action=allow protocol=TCP localport=!PORT! profile=!NETCAT!'" >nul 2>&1
netsh advfirewall firewall show rule name=!FWRULE! >nul 2>&1
if errorlevel 1 goto :fw_manual
echo         تمت إضافة القاعدة !FWRULE!.
goto :installstep

:fw_manual
echo         أضفها من نافذة أوامر بصلاحيات مدير:
echo             !FWCMD!

rem ---------- 12) التركيب على الأجهزة الموصولة -----------------
rem  تغيّر العنوان يبطل النسخة المركّبة على كل هاتف، لا الملف هنا وحده.
rem  فمن كان موصولًا بـ USB نحدّثه فورًا، وللبقية النقل اليدوي.
:installstep
set "DEVS="
set "NDEV=0"
if defined ADBFULL (
    for /f "tokens=1" %%d in ('adb devices 2^>nul ^| findstr /r "device$"') do (
        set "DEVS=!DEVS! %%d"
        set /a NDEV+=1
    )
)

echo.
if "!NDEV!"=="0" (
    if defined ADBFULL (
        echo لا جهاز موصول عبر USB الآن - لم يُركَّب شيء تلقائيًا.
    ) else (
        echo adb غير موجود - التركيب التلقائي غير متاح.
    )
    goto :summary
)

echo أجهزة موصولة:!DEVS!
if defined DOINSTALL goto :doinstall
if "%RHALLA_YES%"=="1" (
    echo         لم يُركَّب شيء. أضف "install" لتركيبه تلقائيًا.
    goto :summary
)
choice /c YN /n /m "تركيب الـ APK على هذه الأجهزة الآن؟ [Y/N] "
if errorlevel 2 (
    echo         تخطّي التركيب.
    goto :summary
)

:doinstall
for %%d in (!DEVS!) do (
    echo   تركيب على %%d ...
    adb -s %%d install -r "!FINAL!" >nul 2>&1
    if errorlevel 1 (
        echo   [تنبيه] فشل التركيب على %%d.
        echo           إن كان مركّبًا بتوقيع مختلف:  adb -s %%d uninstall com.rhalla.rhalla_agent
    ) else (
        echo   تم على %%d.
    )
)

rem ---------- 13) الخلاصة --------------------------------------
:summary
echo.
echo ============================================================
echo  الملف : !FINAL!
for %%f in ("!FINAL!") do echo  الحجم : %%~zf بايت
echo  الخادم: !API_BASE!
echo  السجل : !STATE!
echo ============================================================
echo.
echo  التركيب يدويًا على هاتف موصول بـ USB:
if defined ADBFULL (
    echo      "!ADBFULL!" install -r "!FINAL!"
) else (
    echo      adb install -r "!FINAL!"
)
echo.
echo  أو انسخ الملف إلى الهاتف وثبّته منه - يلزم السماح بتثبيت
echo  التطبيقات من مصادر غير معروفة.
echo.
echo  شروط عمله على الهاتف:
echo    1. الهاتف على نفس شبكة الـ Wi-Fi، لا على بيانات الجوال.
echo    2. الخادم يعمل:  run_backend.bat !PORT!
echo    3. للتأكد، افتح من متصفّح الهاتف:  http://!IP!:!PORT!/api
echo.
echo  إن شككت لاحقًا في تغيّر عنوان الجهاز:  build-apk-production.bat check
echo  يخبرك في ثوانٍ إن كانت النسخة الموزّعة ما زالت صالحة.
echo.
if /i "!MODE!"=="debug" (
    echo  ملاحظة: هذه نسخة debug - أبطأ من الإصدار ويظهر عليها شريط التصحيح.
    echo  لأداء أقرب للحقيقي:  build-apk-production.bat profile
    echo.
)
pause
goto :end


rem ============================================================
rem  الإجراءات
rem ============================================================

:write_nsc
    > "%~1" (
        echo ^<?xml version="1.0" encoding="utf-8"?^>
        echo ^<^^!--
        echo   مولَّد من build-apk-production.bat للتجربة على الشبكة المحلية.
        echo   يُعاد الملف الأصلي فور انتهاء البناء. لا تحرّره يدويًا.
        echo --^>
        echo ^<network-security-config^>
        echo     ^<domain-config cleartextTrafficPermitted="true"^>
        echo         ^<domain includeSubdomains="false"^>!IP!^</domain^>
        echo         ^<domain includeSubdomains="false"^>10.0.2.2^</domain^>
        echo         ^<domain includeSubdomains="false"^>localhost^</domain^>
        echo         ^<domain includeSubdomains="false"^>127.0.0.1^</domain^>
        echo     ^</domain-config^>
        echo     ^<base-config cleartextTrafficPermitted="false" /^>
        echo ^</network-security-config^>
    )
    exit /b 0

:write_profile_manifest
    rem  مجموعة مصادر profile لا تحمل إعداد شبكة في الأصل، فنضيفه هنا
    rem  مؤقتًا. يبقى خارج بناء الإصدار تمامًا كنظيره في debug.
    > "%PRO_MAN%" (
        echo ^<manifest xmlns:android="http://schemas.android.com/apk/res/android"
        echo     xmlns:tools="http://schemas.android.com/tools"^>
        echo     ^<uses-permission android:name="android.permission.INTERNET"/^>
        echo     ^<application
        echo         android:networkSecurityConfig="@xml/network_security_config"
        echo         tools:replace="android:networkSecurityConfig" /^>
        echo ^</manifest^>
    )
    exit /b 0

:restore
    if exist "%BK_XML%" (
        copy /y "%BK_XML%" "%DBG_XML%" >nul
        del "%BK_XML%" >nul 2>&1
    )
    if exist "%BK_MAN%" (
        copy /y "%BK_MAN%" "%PRO_MAN%" >nul
        del "%BK_MAN%" >nul 2>&1
    )
    if exist "%PRO_XML%" del "%PRO_XML%" >nul 2>&1
    if exist "%PRO_DIR%" rd "%PRO_DIR%" >nul 2>&1
    if exist "%PRO_RES%" rd "%PRO_RES%" >nul 2>&1
    if exist "%MARK%" del "%MARK%" >nul 2>&1
    exit /b 0

:fail
    call :restore
    echo.
    echo   لم يكتمل البناء.
    pause
    endlocal
    exit /b 1

:end
    endlocal
    exit /b 0
