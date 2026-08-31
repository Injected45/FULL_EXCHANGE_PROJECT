@echo off
setlocal EnableDelayedExpansion
title Rhalla Agent - Build APK
cd /d "%~dp0rhalla_agent"

rem ============================================================
rem  Build the agent APK and drop it in <repo>\apk\.
rem
rem  Usage:  build_apk.bat [release^|profile^|debug] [API_BASE]
rem          set RHALLA_YES=1 to skip the confirmation prompt.
rem          set RHALLA_PORT=8000 to change the backend port.
rem
rem  profile is the one to hand someone for testing: AOT-compiled
rem  like release, ~80 MB against debug's ~216 MB, and it needs no
rem  signing key - so it builds when release cannot. It is not for
rem  the store: it carries observability hooks and debug signing.
rem
rem  API_BASE defaults to THIS MACHINE'S LAN ADDRESS, so the APK
rem  talks to the backend running here from any phone on the same
rem  Wi-Fi. 10.0.2.2 is NOT used: that alias exists only inside the
rem  emulator and is unreachable from a real device.
rem
rem  The address is detected at build time and compiled in - it
rem  cannot be changed afterwards. If this PC's IP changes, the APK
rem  stops working and must be rebuilt. Pass API_BASE explicitly to
rem  target anything else, including production.
rem
rem  A release build is refused unless android\key.properties
rem  exists - signing with the debug keystore produces an APK that
rem  Google Play rejects and that can never be upgraded, and the
rem  silent fallback is exactly how such an APK got built before.
rem
rem  Verify, do not trust: debug builds hide two defects that only
rem  appear in release (a missing INTERNET permission, and debug
rem  signing). This script checks the built APK for both.
rem ============================================================

set "MODE=%~1"
if not defined MODE set "MODE=release"
if /i "%MODE%"=="release" goto :modeok
if /i "%MODE%"=="profile" goto :modeok
if /i "%MODE%"=="debug"   goto :modeok
echo [ERROR] Unknown build type "%MODE%". Use "release", "profile" or "debug".
pause
exit /b 1
:modeok

rem ---------- 0) Where is the backend? -----------------------
rem  Detect this PC's LAN IPv4 so a phone on the same Wi-Fi can
rem  reach the backend running here. WellKnown/link-local/loopback
rem  are excluded, and the lowest interface metric wins - that is
rem  the adapter Windows itself routes through.
set "API_BASE=%~2"
if defined API_BASE goto :apiok

if not defined RHALLA_PORT set "RHALLA_PORT=8000"

set "LANIP="
rem  No pipes in the PowerShell below: cmd does not unescape ^| inside
rem  the quoted argument, so a piped one-liner reaches PowerShell broken
rem  and Where-Object fails. The .Where() method keeps it pipe-free.
for /f "usebackq delims=" %%i in (`powershell -NoProfile -Command "$a=@(Get-NetIPAddress -AddressFamily IPv4).Where({$_.PrefixOrigin -ne 'WellKnown' -and ($_.IPAddress -like '192.168.*' -or $_.IPAddress -like '10.*' -or $_.IPAddress -like '172.*')}); if($a.Count){$a[0].IPAddress}"`) do set "LANIP=%%i"

if not defined LANIP (
    echo [ERROR] Could not detect this machine's LAN address.
    echo         Pass it yourself, e.g.:
    echo             build_apk.bat debug http://192.168.0.173:8000/api
    pause
    exit /b 1
)

set "API_BASE=http://!LANIP!:%RHALLA_PORT%/api"
:apiok

set "OUTDIR=%~dp0apk"

set "SDK=%ANDROID_SDK_ROOT%"
if not defined SDK set "SDK=%ANDROID_HOME%"
if not defined SDK set "SDK=%LOCALAPPDATA%\Android\Sdk"

rem ---------- 1) Toolchain -----------------------------------
where flutter >nul 2>&1
if errorlevel 1 (
    echo [ERROR] flutter is not on PATH.
    pause
    exit /b 1
)

rem ---------- 2) Signing keys (release only) ------------------
if /i "%MODE%"=="release" if not exist "android\key.properties" (
    echo [ERROR] android\key.properties is missing - refusing to build a release APK.
    echo.
    echo   1. Generate the keystore ONCE, and store it OFF this machine:
    echo.
    echo      keytool -genkey -v -keystore rhalla-agent-release.jks -keyalg RSA -keysize 2048 -validity 10000 -alias rhalla
    echo.
    echo   2. Copy android\key.properties.template to android\key.properties
    echo      and fill in storeFile, storePassword, keyAlias, keyPassword.
    echo.
    echo   The key cannot be replaced after the first Play release. Losing it
    echo   means the app can never be updated under the same listing.
    echo.
    echo   For a throwaway build for testing, use:  build_apk.bat debug
    pause
    exit /b 1
)

rem ---------- 2b) Cleartext allow-list (debug over http) ------
rem  Android blocks cleartext HTTP unless the host is listed. Only the
rem  debug source set carries a config that permits it at all, so this
rem  applies to debug builds only - a release apk over http:// cannot
rem  work, and the fix for that is TLS on the server, not an exception.
set "NSC=android\app\src\debug\res\xml\network_security_config.xml"
if /i "%MODE%"=="debug" (
    echo %API_BASE% | findstr /b /i /c:"http://" >nul
    if not errorlevel 1 (
        for /f "tokens=2 delims=/" %%h in ("%API_BASE%") do set "APIHOST=%%h"
        for /f "tokens=1 delims=:" %%h in ("!APIHOST!") do set "APIHOST=%%h"
        if exist "%NSC%" (
            rem  -ExecutionPolicy Bypass: this machine's policy is Restricted,
            rem  so a .ps1 will not run without it. Scoped to this one call.
            powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0tools\allow_cleartext_host.ps1" -Config "%NSC%" -HostName "!APIHOST!"
            if errorlevel 1 (
                echo [ERROR] Could not update %NSC%
                pause
                exit /b 1
            )
        ) else (
            echo [WARN] %NSC% is missing - the apk may not be allowed to use http://.
        )
    )
)

rem ---------- 2c) Build scratch space -------------------------
rem  Gradle fails deep inside ExtractJniTransform with "not enough
rem  space on the disk" when C: runs out. Keep its home and TEMP on
rem  a drive that has room, if there is one.
if exist "D:\" (
    if not exist "D:\gradle" mkdir "D:\gradle"
    if not exist "D:\tmp"    mkdir "D:\tmp"
    set "GRADLE_USER_HOME=D:\gradle"
    set "TMP=D:\tmp"
    set "TEMP=D:\tmp"
)

rem ---------- 3) Confirm the target ---------------------------
echo.
echo ============================================================
echo   build type : %MODE%
echo   API_BASE   : %API_BASE%
echo   output     : %OUTDIR%
echo ============================================================
echo.
echo  API_BASE is compiled in and cannot be changed afterwards.
echo  The default is this PC's LAN address - the phone must be on
echo  the same Wi-Fi and run_backend.bat must be running here.
echo  Pass an API_BASE argument to target production instead.
echo.
if not "%RHALLA_YES%"=="1" (
    choice /c YN /n /m "Build with these settings? [Y/N] "
    if errorlevel 2 (
        echo Cancelled.
        exit /b 1
    )
)
echo.

rem ---------- 4) Build ----------------------------------------
call flutter pub get
if errorlevel 1 (
    echo [ERROR] flutter pub get failed.
    pause
    exit /b 1
)

echo Building %MODE% APK...
call flutter build apk --%MODE% --dart-define=API_BASE=%API_BASE%
if errorlevel 1 (
    echo.
    echo [ERROR] The build failed.
    echo         If Gradle complained about SDK licences, run:
    echo             flutter doctor --android-licenses
    echo         "not enough space on the disk"    - free space on C:
    echo         "daemon disappeared unexpectedly" - lower org.gradle.jvmargs
    pause
    exit /b 1
)

set "BUILT=build\app\outputs\flutter-apk\app-%MODE%.apk"
if not exist "%BUILT%" (
    echo [ERROR] Build reported success but "%BUILT%" is not there.
    pause
    exit /b 1
)

rem ---------- 5) Copy into <repo>\apk\ ------------------------
if not exist "%OUTDIR%" mkdir "%OUTDIR%"
set "FINAL=%OUTDIR%\rhalla-agent-%MODE%.apk"
copy /y "%BUILT%" "%FINAL%" >nul
if errorlevel 1 (
    echo [ERROR] Could not copy the APK to "%FINAL%".
    pause
    exit /b 1
)

rem ---------- 6) Verify the APK itself ------------------------
rem  Not paranoia: the release manifest really did ship without
rem  INTERNET once, and the app could not open a socket at all.
set "AAPT="
for /f "delims=" %%d in ('dir /b /ad /o-n "%SDK%\build-tools" 2^>nul') do (
    if not defined AAPT if exist "%SDK%\build-tools\%%d\aapt.exe" set "AAPT=%SDK%\build-tools\%%d\aapt.exe"
)

if defined AAPT (
    "!AAPT!" dump permissions "%FINAL%" | findstr /c:"android.permission.INTERNET" >nul
    if errorlevel 1 (
        echo.
        echo [ERROR] The APK does not declare android.permission.INTERNET.
        echo         It cannot open a network socket - every screen would fail.
        echo         Check android\app\src\main\AndroidManifest.xml.
        pause
        exit /b 1
    )
    echo Permission check : INTERNET is declared.
) else (
    echo [WARN] aapt.exe not found under "%SDK%\build-tools" - skipped the
    echo        INTERNET permission check on the built APK.
)

rem ---------- 6b) Signature check (release only) --------------
rem  keytool is not on PATH on this machine; it lives in the JDK
rem  that Android Studio bundles. Look there before giving up, so
rem  the check cannot pass by silently doing nothing.
if /i "%MODE%"=="release" (
    set "KEYTOOL="
    if exist "%ProgramFiles%\Android\Android Studio\jbr\bin\keytool.exe" set "KEYTOOL=%ProgramFiles%\Android\Android Studio\jbr\bin\keytool.exe"
    if not defined KEYTOOL if defined JAVA_HOME if exist "%JAVA_HOME%\bin\keytool.exe" set "KEYTOOL=%JAVA_HOME%\bin\keytool.exe"
    if not defined KEYTOOL for /f "delims=" %%k in ('where keytool 2^>nul') do if not defined KEYTOOL set "KEYTOOL=%%k"

    if defined KEYTOOL (
        "!KEYTOOL!" -printcert -jarfile "%FINAL%" > "%TEMP%\rhalla_cert.txt" 2>nul
        findstr /i /c:"Android Debug" "%TEMP%\rhalla_cert.txt" >nul
        if not errorlevel 1 (
            echo.
            echo [ERROR] This release APK is signed with the DEBUG keystore.
            echo         Google Play rejects it, and an app published with it
            echo         can never be upgraded. Check android\key.properties.
            del "%TEMP%\rhalla_cert.txt" >nul 2>&1
            pause
            exit /b 1
        )
        for /f "tokens=1,* delims=:" %%a in ('findstr /c:"Owner:" "%TEMP%\rhalla_cert.txt"') do echo Signature check  : signed by%%b
        del "%TEMP%\rhalla_cert.txt" >nul 2>&1
    ) else (
        echo [WARN] keytool.exe not found - could not confirm this APK is signed
        echo        with the release key rather than the debug one.
    )
)

rem ---------- 7) Done -----------------------------------------
echo.
echo ============================================================
echo  Built: %FINAL%
for %%f in ("%FINAL%") do echo  Size : %%~zf bytes
echo  Talks to: %API_BASE%
echo ============================================================
echo.
echo  Install on a connected device with:
echo      adb install -r "%FINAL%"
echo.
if /i "%MODE%"=="release" (
    echo  NOTE: this build targets %API_BASE%
    echo  If that is a plain http:// address the build cannot reach it.
    echo  Android blocks cleartext at this targetSdk and iOS blocks it via
    echo  ATS. The fix is a TLS certificate on the server - not
    echo  usesCleartextTraffic, which would put balances and transfer codes
    echo  on the wire in the clear.
    echo.
) else (
    echo  Check from the phone's browser first:
    echo      %API_BASE%/device/exchange/AppTerms_get
    echo.
)
pause
endlocal
goto :eof

rem ============================================================
rem  Subroutines
rem ============================================================

rem  Reads this PC's LAN address, via tools\detect_lan_ip.ps1.
rem
rem  The query lives in a .ps1 rather than inline here because as a
rem  one-liner in a for-loop it needs "^|" for every pipe and dies
rem  inside an if-block, where cmd ends the block at the first
rem  unescaped ")" - and it dies silently, returning an empty
rem  address instead of an error.
:detect_lanip
set "LANIP="
for /f "delims=" %%i in ('powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0tools\detect_lan_ip.ps1" 2^>nul') do if not defined LANIP set "LANIP=%%i"
exit /b 0
