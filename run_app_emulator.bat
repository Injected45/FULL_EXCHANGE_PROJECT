@echo off
setlocal enabledelayedexpansion
title Rhalla - Agent app on Android emulator
cd /d "%~dp0rhalla_agent"

rem ============================================================
rem  Boots an Android emulator (if none is running) and launches
rem  the agent app against the local backend.
rem
rem  10.0.2.2 is the host machine as seen from inside the emulator.
rem  Cleartext HTTP to that address is allowed only in the debug
rem  build - see android/app/src/debug/res/xml/network_security_config.xml
rem
rem  Usage:  run_app_emulator.bat [avd-id] [api-base]
rem  e.g.    run_app_emulator.bat Pixel_4_API_36
rem          run_app_emulator.bat Pixel_4_API_36 http://10.0.2.2:8080/api
rem ============================================================

set "AVD=%~1"
if "%AVD%"=="" set "AVD=Pixel_4_API_36"

set "API_BASE=%~2"
if "%API_BASE%"=="" set "API_BASE=http://10.0.2.2:8000/api"

rem ============================================================
rem  1) Locate Flutter
rem ============================================================
rem  Ask for flutter.bat specifically. Plain "where flutter" lists the
rem  extensionless POSIX script first, which cmd cannot execute.
set "FLUTTER="
for /f "delims=" %%i in ('where flutter.bat 2^>nul') do if not defined FLUTTER set "FLUTTER=%%i"
if not defined FLUTTER if exist "C:\flutter\bin\flutter.bat" set "FLUTTER=C:\flutter\bin\flutter.bat"
if not defined FLUTTER (
    echo [ERROR] flutter.bat not found on PATH and not at C:\flutter\bin
    pause
    exit /b 1
)
echo Using Flutter: %FLUTTER%

rem ============================================================
rem  2) Locate the Android SDK
rem ============================================================
set "SDK=%ANDROID_HOME%"
if not defined SDK set "SDK=%ANDROID_SDK_ROOT%"
if not defined SDK set "SDK=%LOCALAPPDATA%\Android\Sdk"

set "ADB=%SDK%\platform-tools\adb.exe"
if not exist "%ADB%" (
    echo [ERROR] adb not found at "%ADB%"
    echo         Set ANDROID_HOME to your SDK folder.
    pause
    exit /b 1
)

rem ============================================================
rem  3) Is a device already attached? If so, reuse it.
rem ============================================================
"%ADB%" start-server >nul 2>&1

set "RUNNING="
for /f "skip=1 tokens=1,2" %%a in ('"%ADB%" devices 2^>nul') do (
    if "%%b"=="device" if not defined RUNNING set "RUNNING=%%a"
)

if defined RUNNING (
    echo Device already attached: !RUNNING!
    goto :run
)

rem ============================================================
rem  4) Boot the emulator
rem ============================================================
rem  "call" is required: invoking a .bat without it transfers control
rem  permanently and the rest of this script never runs.
echo Launching emulator: %AVD%
call "%FLUTTER%" emulators --launch %AVD%
if errorlevel 1 (
    echo.
    echo [ERROR] Could not launch "%AVD%".
    echo.
    echo   The most common cause on this machine is missing CPU virtualization.
    echo   x86_64 emulation REQUIRES hardware acceleration; without it the
    echo   emulator exits with code 1 and Flutter hides the real reason.
    echo.
    echo   Check it:
    echo     powershell -c "(Get-ComputerInfo).HyperVRequirementVirtualizationFirmwareEnabled"
    echo.
    echo   If that prints False, enable Intel VT-x ^(or AMD-V^) in the BIOS/UEFI
    echo   first - no driver or SDK install can work around it. Then install the
    echo   "Android Emulator hypervisor driver" from Android Studio's SDK Manager
    echo   ^(SDK Tools tab^).
    echo.
    echo   To see the emulator's own error instead of Flutter's summary:
    echo     "%SDK%\emulator\emulator.exe" -avd %AVD%
    echo.
    echo   Available emulators:
    call "%FLUTTER%" emulators
    pause
    exit /b 1
)

rem ============================================================
rem  5) Wait for boot to finish
rem     wait-for-device only waits for the shell, not for Android
rem     to be usable - so poll sys.boot_completed as well.
rem ============================================================
echo Waiting for the device to come online...
"%ADB%" wait-for-device

echo Waiting for Android to finish booting...
rem  200 tries x ~3s = about 10 minutes. A COLD boot here renders in software
rem  (Intel UHD 620 reports Vulkan 1.1.95, below the 1.3.240 the emulator
rem  wants), so the first boot is minutes, not seconds. Later boots reuse the
rem  quickboot snapshot and are much faster.
set /a TRIES=0
:waitboot
set /a TRIES+=1
if %TRIES% gtr 200 (
    echo.
    echo [ERROR] Emulator did not finish booting in time.
    echo         Check it directly:  "%ADB%" shell getprop init.svc.bootanim
    pause
    exit /b 1
)
set "BOOTED="
for /f "usebackq delims=" %%b in (`"%ADB%" shell getprop sys.boot_completed 2^>nul`) do set "BOOTED=%%b"
rem Compare only the first character - the value carries a trailing CR.
rem  Absolute path to timeout.exe on purpose: when this script is started from
rem  a shell whose PATH contains Unix tools (Git Bash), a bare "timeout" hits
rem  GNU coreutils instead, which rejects /t - the loop then spins with no
rem  delay and gives up long before a cold boot finishes.
if not "!BOOTED:~0,1!"=="1" (
    "%SystemRoot%\System32\timeout.exe" /t 3 /nobreak >nul 2>&1 || ping -n 4 127.0.0.1 >nul 2>&1
    goto :waitboot
)
echo Device is ready.

:run
rem ============================================================
rem  6) Run the app
rem     --dart-define is NOT optional: kApiBase falls back to the
rem     LIVE production server, which moves real money.
rem ============================================================
echo.
echo ============================================================
echo   API_BASE   %API_BASE%
echo.
echo   Make sure run_backend.bat is running in another window.
echo.
echo   r = hot reload   R = hot restart   q = quit
echo ============================================================
echo.

call "%FLUTTER%" run --dart-define=API_BASE=%API_BASE%

if errorlevel 1 (
    echo.
    echo [ERROR] flutter run exited with an error.
    pause
    exit /b 1
)

endlocal
