@echo off
setlocal EnableDelayedExpansion
title Rhalla Agent - Run on Emulator
cd /d "%~dp0rhalla_agent"

rem ============================================================
rem  Boot the Android emulator (if it is not already up) and run
rem  the agent app against a chosen backend.
rem
rem  Usage:  run_app.bat [API_BASE] [AVD_NAME]
rem
rem  API_BASE defaults to the local backend as seen FROM the
rem  emulator: 10.0.2.2 is the host machine's loopback. A phone
rem  on Wi-Fi cannot use that - pass this machine's LAN address:
rem
rem      run_app.bat http://192.168.1.20:8000/api
rem
rem  kApiBase in lib/core/net/api_client.dart defaults to the LIVE
rem  production server, so --dart-define is never optional here: a
rem  bare `flutter run` moves real money against real accounts.
rem ============================================================

set "API_BASE=%~1"
if not defined API_BASE set "API_BASE=http://10.0.2.2:8000/api"

set "AVD=%~2"

rem ---------- 1) Locate the tools -----------------------------
where flutter >nul 2>&1
if errorlevel 1 (
    echo [ERROR] flutter is not on PATH.
    pause
    exit /b 1
)

set "SDK=%ANDROID_SDK_ROOT%"
if not defined SDK set "SDK=%ANDROID_HOME%"
if not defined SDK set "SDK=%LOCALAPPDATA%\Android\Sdk"
if not exist "%SDK%\platform-tools\adb.exe" (
    echo [ERROR] Android SDK not found at "%SDK%".
    echo         Set ANDROID_SDK_ROOT to the SDK folder.
    pause
    exit /b 1
)
set "ADB=%SDK%\platform-tools\adb.exe"
set "EMULATOR=%SDK%\emulator\emulator.exe"

rem ---------- 2) Is a device already attached? ----------------
set "DEVICE_UP="
for /f "skip=1 tokens=1,2" %%a in ('"%ADB%" devices 2^>nul') do (
    if "%%b"=="device" set "DEVICE_UP=%%a"
)

if defined DEVICE_UP (
    echo Device already connected: !DEVICE_UP!
    goto :run
)

rem ---------- 3) Start an emulator ----------------------------
if not exist "%EMULATOR%" (
    echo [ERROR] emulator.exe not found and no device is attached.
    echo         Create an AVD in Android Studio, or plug in a phone.
    pause
    exit /b 1
)

if not defined AVD (
    for /f "delims=" %%i in ('"%EMULATOR%" -list-avds 2^>nul') do if not defined AVD set "AVD=%%i"
)
if not defined AVD (
    echo [ERROR] No AVD exists. Create one in Android Studio ^> Device Manager.
    pause
    exit /b 1
)

echo Starting emulator: !AVD!
start "" "%EMULATOR%" -avd "!AVD!"

echo Waiting for the device to come online...
"%ADB%" wait-for-device
if errorlevel 1 (
    echo [ERROR] adb wait-for-device failed.
    pause
    exit /b 1
)

rem  wait-for-device returns as soon as adb sees it - the system is
rem  still booting at that point and `flutter run` would fail to
rem  install. Poll sys.boot_completed instead. ~5 min ceiling.
echo Waiting for Android to finish booting...
set /a TRIES=0
:bootwait
set /a TRIES+=1
if !TRIES! gtr 150 (
    echo [ERROR] Emulator did not finish booting in time.
    pause
    exit /b 1
)
set "BOOTED="
for /f "delims=" %%b in ('"%ADB%" shell getprop sys.boot_completed 2^>nul') do set "BOOTED=%%b"
if not "!BOOTED!"=="1" (
    "%ADB%" wait-for-device >nul 2>&1
    ping -n 3 127.0.0.1 >nul
    goto :bootwait
)
echo Emulator is ready.

:run
rem ---------- 4) Dependencies --------------------------------
if not exist ".dart_tool\package_config.json" (
    echo Fetching Dart packages...
    call flutter pub get
    if errorlevel 1 (
        echo [ERROR] flutter pub get failed.
        pause
        exit /b 1
    )
)

rem ---------- 5) Run -----------------------------------------
echo.
echo ============================================================
echo  API_BASE = %API_BASE%
echo ============================================================
echo.
echo  If every screen shows a network error, check that:
echo   - run_backend.bat is running on this machine
echo   - the host part above matches how the DEVICE sees this PC
echo     ^(emulator: 10.0.2.2 - phone: this PC's LAN IP^)
echo.

call flutter run --dart-define=API_BASE=%API_BASE%

endlocal
