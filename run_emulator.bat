@echo off
setlocal EnableDelayedExpansion
title Rhalla - Android Emulator
cd /d "%~dp0"

rem ============================================================
rem  Start the Android emulator and PROVE it can render.
rem
rem  Usage:  run_emulator.bat [AVD_NAME] [soft]
rem            soft = force the CPU renderer (see below)
rem
rem  WHY THIS SCRIPT CHECKS THE RENDERER
rem  -----------------------------------
rem  On 30 Aug 2026 this machine (Intel Iris Xe, driver 32.0.101.7082)
rem  rendered EVERY Flutter app as a pure black screen - including an
rem  empty `flutter create` app - while Android's own UI drew fine.
rem  That combination makes it look like an app bug. It is not.
rem
rem  Updating the Intel driver to 32.0.101.7088 fixed it, and the
rem  emulator now runs on the GPU at full speed. The check below stays
rem  because the failure is silent and costs hours to re-diagnose: if
rem  the screen is ever black again, run this and read the GLES line.
rem
rem  Fallback: `run_emulator.bat "" soft` forces SwiftShader, a CPU
rem  renderer that bypasses the graphics card entirely. It DOES render,
rem  but it is slow enough to trigger "System UI isn't responding" and
rem  it segfaulted once under load - use it only to unblock yourself
rem  while updating the driver.
rem
rem  NOTE: hw.gpu.mode in the AVD config only accepts auto/host/
rem  swiftshader/swangle. Both `swiftshader_indirect` and
rem  `angle_indirect` are REJECTED there and silently fall back to
rem  auto with an ERROR line in the log - the command-line -gpu flag
rem  below is the only reliable way to force a renderer.
rem ============================================================

set "AVD=%~1"
set "MODE=%~2"

set "SDK=%ANDROID_SDK_ROOT%"
if not defined SDK set "SDK=%ANDROID_HOME%"
if not defined SDK set "SDK=%LOCALAPPDATA%\Android\Sdk"

set "ADB=%SDK%\platform-tools\adb.exe"
set "EMULATOR=%SDK%\emulator\emulator.exe"

if not exist "%EMULATOR%" (
    echo [ERROR] emulator.exe not found under "%SDK%".
    echo         Set ANDROID_SDK_ROOT to the SDK folder.
    pause
    exit /b 1
)

rem ---------- Already running? --------------------------------
for /f "skip=1 tokens=1,2" %%a in ('"%ADB%" devices 2^>nul') do (
    if "%%b"=="device" (
        echo A device is already connected: %%a
        echo Nothing to do.
        pause
        exit /b 0
    )
)

rem ---------- Pick an AVD -------------------------------------
if not defined AVD (
    for /f "delims=" %%i in ('"%EMULATOR%" -list-avds 2^>nul') do if not defined AVD set "AVD=%%i"
)
if not defined AVD (
    echo [ERROR] No AVD exists. Create one in Android Studio ^> Device Manager.
    pause
    exit /b 1
)

rem ---------- Launch ------------------------------------------
if /i "%MODE%"=="soft" (
    echo Starting !AVD! with the CPU renderer ^(slow - fallback only^)...
    start "" "%EMULATOR%" -avd "!AVD!" -gpu swiftshader_indirect
) else (
    echo Starting !AVD! ...
    start "" "%EMULATOR%" -avd "!AVD!"
)

rem ---------- Wait for a full boot ----------------------------
rem  wait-for-device returns while Android is still booting, so poll
rem  sys.boot_completed instead.
"%ADB%" wait-for-device
echo Waiting for Android to finish booting...
set /a TRIES=0
:bootwait
set /a TRIES+=1
if !TRIES! gtr 200 (
    echo [ERROR] The emulator did not finish booting in time.
    pause
    exit /b 1
)
set "BOOTED="
for /f "delims=" %%b in ('"%ADB%" shell getprop sys.boot_completed 2^>nul') do set "BOOTED=%%b"
if not "!BOOTED!"=="1" (
    "%ADB%" wait-for-device >nul 2>&1
    ping -n 4 127.0.0.1 >nul
    goto :bootwait
)

rem ---------- Report the renderer -----------------------------
echo.
for /f "delims=" %%g in ('"%ADB%" shell dumpsys SurfaceFlinger 2^>nul ^| findstr /b "GLES:"') do set "GLES=%%g"
echo !GLES!
echo.
echo If the app shows a black screen, the graphics driver is the
echo suspect - not the app. Update the Intel driver, or run:
echo     run_emulator.bat "" soft
echo.
echo Emulator ready. Run the app with:  run_app.bat
echo.
endlocal
