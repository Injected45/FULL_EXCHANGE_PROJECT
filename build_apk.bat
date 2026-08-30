@echo off
setlocal EnableDelayedExpansion
title Rhalla Agent - Build APK
cd /d "%~dp0rhalla_agent"

rem ============================================================
rem  Build the agent APK and drop it in <repo>\apk\.
rem
rem  Usage:  build_apk.bat [release^|debug] [API_BASE]
rem          set RHALLA_YES=1 to skip the confirmation prompt.
rem
rem  Defaults to a RELEASE build against the production API.
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
if /i "%MODE%"=="debug"   goto :modeok
echo [ERROR] Unknown build type "%MODE%". Use "release" or "debug".
pause
exit /b 1
:modeok

set "API_BASE=%~2"
if not defined API_BASE set "API_BASE=http://102.214.165.242:8080/api"

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

rem ---------- 3) Confirm the target ---------------------------
echo.
echo ============================================================
echo   build type : %MODE%
echo   API_BASE   : %API_BASE%
echo   output     : %OUTDIR%
echo ============================================================
echo.
echo  API_BASE is compiled in and cannot be changed afterwards.
echo  The default is the LIVE production server - a build made with
echo  it moves real money against real accounts.
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
)
pause
endlocal
