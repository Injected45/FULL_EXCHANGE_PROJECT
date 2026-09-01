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
rem  e.g.    run_app_emulator.bat
rem          run_app_emulator.bat Pixel_4_API_30_x64
rem          run_app_emulator.bat Pixel_4_API_30_x64 http://10.0.2.2:8080/api
rem ============================================================

rem  Default AVD: API 30 on x86_64.
rem
rem  Not API 36, and not any x86 image:
rem    - Pixel_4_API_36 hangs forever on a machine without Vulkan. Its GPU
rem      mode falls back to llvmpipe and the graphics composer spins at ~93%
rem      CPU while bootanim never stops - it looks like a slow boot and never
rem      finishes. Pass it explicitly if this machine has a capable GPU.
rem    - A 32-bit x86 AVD cannot run this app at all: Flutter builds
rem      arm64-v8a, armeabi-v7a and x86_64 only, so install fails with
rem      INSTALL_FAILED_NO_MATCHING_ABIS after the emulator has booted.
set "AVD=%~1"
if "%AVD%"=="" set "AVD=Pixel_4_API_30_x64"

set "API_BASE=%~2"
if "%API_BASE%"=="" set "API_BASE=http://10.0.2.2:8000/api"

rem ============================================================
rem  0) Keep everything that GROWS off C:
rem ============================================================
rem  The disk layout here is the whole reason this section exists:
rem
rem    C: and D: are two partitions of ONE small SSD (KingFast, 238 GB).
rem    Enlarging C: from D: with a partition tool does not create room -
rem    it moves the shortage from one partition to the other.
rem    F:/G:/H: are a separate 932 GB mechanical disk. H: has the space.
rem
rem  Where the caches live was measured, and the result is worth writing
rem  down because it contradicts the obvious guess:
rem
rem      cache on D: (SSD)   warm build           assembleDebug  106.7s
rem      cache on H: (HDD)   cold, after pub get  assembleDebug  404.2s
rem      cache on D: (SSD)   cold, after pub get  assembleDebug  416.0s
rem
rem  The two COLD builds are within 3% of each other. The disk made no
rem  measurable difference; the variable that mattered was cold vs warm
rem  (a fresh Gradle daemon, and "flutter pub get" invalidating the Dart
rem  kernel). An earlier version of this comment blamed the HDD for a
rem  "3.8x slowdown" - that was the warm number compared against a cold
rem  one, and it was wrong.
rem
rem  So the layout below is chosen for HEADROOM, not speed. Caches sit on
rem  D: because that is where they already are and D: has room; if D: ever
rem  gets tight, moving them to H:\dev-cache costs nothing measurable.
rem
rem    D:\gradle-cache      Gradle cache - in the build path, see above
rem    D:\pub-cache         pub cache - package SOURCES are compiled from
rem                         here on every build, not just on "pub get"
rem    D:\avd               emulator devices - the emulator reads these
rem                         constantly; an HDD makes an already-slow
rem                         software-GL boot far worse
rem    D:\tmp               build temp - small, and every hot reload writes
rem                         its incremental .dill here
rem    H:\dev-cache\avd-archive
rem                         the two AVDs that are never launched
rem                         (Pixel_4_API_36, Medium_Phone). Parked, not
rem                         deleted - move a folder plus its .ini back into
rem                         D:\avd to get it listed again. This is what
rem                         buys D: the headroom for the caches above.
rem
rem  Paths are ASCII with no spaces deliberately. aapt2, Gradle and pub
rem  have a long history of failing on Windows paths containing non-Latin
rem  characters or spaces, and they fail with an unrelated-looking error
rem  in the middle of a build.
rem
rem  The same values are also set permanently as USER environment
rem  variables, so a bare "flutter run" outside this script stays off C:
rem  too. They are repeated here on purpose: the script must not depend on
rem  an env var someone may have cleared, and a child process inherits the
rem  environment as it was when it started - setting them later has no
rem  effect on a build already under way.
rem
rem  If this is ever undone, the failure does NOT say "disk full". It
rem  names a file:
rem      FileSystemException: writeFrom failed, path = '...\
rem      app.dill.incremental.dill' (OS Error: There is not enough space
rem      on the disk, errno = 112)
rem  and it fires AFTER the APK has built, installed and launched - so the
rem  app sits there running on the emulator and only hot reload is dead.
rem  It reads like a Flutter bug and is a full disk.
rem  Changing PUB_CACHE breaks every existing checkout until "flutter pub
rem  get" is re-run: .dart_tool\package_config.json stores ~107 ABSOLUTE
rem  paths into the old cache. The build then fails inside Flutter's own
rem  source - "Matrix4 isn't a type" in semantics.dart - which looks like a
rem  corrupt Flutter SDK and is really just an unresolvable vector_math.
set "GRADLE_USER_HOME=D:\gradle-cache"
set "PUB_CACHE=D:\pub-cache"
set "ANDROID_AVD_HOME=D:\avd"
set "TMP=D:\tmp"
set "TEMP=D:\tmp"
for %%D in ("D:\gradle-cache" "D:\pub-cache" "D:\avd" "D:\tmp") do if not exist %%D mkdir %%D

rem  Fail here rather than fifteen minutes into a build. A debug build
rem  plus its temp needs roughly 3 GB; below that the failure is certain,
rem  and the message it produces points at the wrong thing.
set "FREE_C=0"
set "FREE_D=0"
set "FREE_H=0"
rem  No pipe character in the PowerShell expression on purpose: cmd parses
rem  the text inside these backquotes before PowerShell ever sees it, and a
rem  "|" there is taken as a cmd pipe. Escaping it reaches PowerShell as a
rem  literal "^|" and is a parse error - which reads as "0 GB free" and
rem  trips the guard below for the wrong reason.
for /f "usebackq tokens=1-3" %%a in (`powershell -NoProfile -Command "$r=@(); foreach ($n in 'C','D','H') { $d = Get-PSDrive $n -ErrorAction SilentlyContinue; if ($d) { $r += [int]($d.Free/1GB) } else { $r += 0 } }; $r -join ' '"`) do (
    set "FREE_C=%%a"
    set "FREE_D=%%b"
    set "FREE_H=%%c"
)
echo Free space:  C: !FREE_C! GB   D: !FREE_D! GB   H: !FREE_H! GB
if !FREE_D! LSS 3 goto :nospace
if !FREE_H! LSS 3 goto :nospace
goto :space_ok

:nospace
echo.
echo [ERROR] Not enough free disk space - stopping before the build starts.
echo         D: !FREE_D! GB  (caches, emulator devices, build temp)
echo         H: !FREE_H! GB  (parked AVDs)
echo         At least 3 GB is needed on each.
echo.
echo         Do NOT fix this by resizing C: from D: - they are one SSD,
echo         so that just moves the shortage. Instead:
echo           - clear D:\gradle-cache  (a cache; it re-downloads), or
echo           - park another unused device: move its folder and .ini from
echo             D:\avd to H:\dev-cache\avd-archive
echo.
pause
exit /b 1

:space_ok

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
