@echo off
setlocal enabledelayedexpansion
title Rhalla - Build installable APK
cd /d "%~dp0rhalla_agent"

rem ============================================================
rem  Builds a DEBUG apk wired to the backend running on THIS machine,
rem  so it can be installed on any phone on the same Wi-Fi.
rem
rem  Usage:  build_apk.bat [ip] [port]
rem          build_apk.bat                 auto-detect IP, port 8000
rem          build_apk.bat 192.168.1.25    force an IP
rem          build_apk.bat 192.168.1.25 8080
rem
rem  Why DEBUG and not RELEASE - both reasons are hard blockers:
rem    1. Release and profile forbid cleartext HTTP. Only the debug source
rem       set carries a network_security_config that permits it, so a release
rem       apk cannot open a single connection to http://<lan-ip>:<port>.
rem    2. Release refuses to build without android/key.properties, and that
rem       signing key must never be improvised for a test build.
rem  Serving the backend over HTTPS is what unlocks a release apk.
rem ============================================================

rem ============================================================
rem  1) Where the backend is. The address is compiled INTO the apk
rem     (API_BASE is a dart-define), so it is detected fresh on every build.
rem ============================================================
set "IP=%~1"
if "%IP%"=="" (
    for /f "delims=" %%i in ('powershell -NoProfile -Command "(Get-NetIPAddress -AddressFamily IPv4 ^| Where-Object { $_.IPAddress -notlike '127.*' -and $_.IPAddress -notlike '169.254.*' -and $_.PrefixOrigin -ne 'WellKnown' } ^| Select-Object -First 1 -ExpandProperty IPAddress)" 2^>nul') do set "IP=%%i"
)
if "%IP%"=="" (
    echo [ERROR] Could not detect this machine's LAN IP address.
    echo         Pass it explicitly:  build_apk.bat 192.168.1.10
    pause
    exit /b 1
)

set "PORT=%~2"
if "%PORT%"=="" set "PORT=8000"

set "API_BASE=http://%IP%:%PORT%/api"

echo ============================================================
echo   Backend address baked into this apk:
echo     %API_BASE%
echo ============================================================
echo.

rem ============================================================
rem  2) Android blocks cleartext HTTP unless the address is listed.
rem     The apk would install and then fail on every screen, so make
rem     sure this IP is in the debug network security config.
rem ============================================================
set "NSC=android\app\src\debug\res\xml\network_security_config.xml"
if not exist "%NSC%" (
    echo [ERROR] Missing %NSC%
    pause
    exit /b 1
)

powershell -NoProfile -Command ^
  "$f='%NSC%'; $ip='%IP%'; $x=Get-Content $f -Raw -Encoding UTF8;" ^
  "if ($x -match [regex]::Escape('>'+$ip+'<')) { 'IP already allowed for cleartext.'; exit 0 }" ^
  "$line='        <domain includeSubdomains=\"false\">'+$ip+'</domain>';" ^
  "$x=$x -replace '(?m)^\s*</domain-config>', ($line + \"`r`n    </domain-config>\");" ^
  "[IO.File]::WriteAllText((Resolve-Path $f), $x, (New-Object Text.UTF8Encoding($false)));" ^
  "'Added ' + $ip + ' to the cleartext allow-list.'"
if errorlevel 1 (
    echo [ERROR] Could not update %NSC%
    pause
    exit /b 1
)
echo.

rem ============================================================
rem  3) Build scratch space.
rem     C: on this machine has run to 0 bytes free, and Gradle then fails
rem     deep inside ExtractJniTransform with "not enough space on the disk".
rem     Keep the Gradle home and TEMP on a drive that has room.
rem ============================================================
if exist "D:\" (
    if not exist "D:\gradle" mkdir "D:\gradle"
    if not exist "D:\tmp"    mkdir "D:\tmp"
    set "GRADLE_USER_HOME=D:\gradle"
    set "TMP=D:\tmp"
    set "TEMP=D:\tmp"
    echo Gradle home: !GRADLE_USER_HOME!
)

rem ============================================================
rem  4) Locate Flutter. Ask for flutter.bat by name - a bare
rem     "where flutter" lists the extensionless POSIX script first,
rem     which cmd cannot execute.
rem ============================================================
set "FLUTTER="
for /f "delims=" %%i in ('where flutter.bat 2^>nul') do if not defined FLUTTER set "FLUTTER=%%i"
if not defined FLUTTER if exist "C:\flutter\bin\flutter.bat" set "FLUTTER=C:\flutter\bin\flutter.bat"
if not defined FLUTTER (
    echo [ERROR] flutter.bat not found.
    pause
    exit /b 1
)

rem ============================================================
rem  5) Build.  "call" is required for a .bat, or control never returns.
rem ============================================================
echo Building. First run downloads dependencies and takes several minutes.
echo.
call "%FLUTTER%" build apk --debug --dart-define=API_BASE=%API_BASE%
if errorlevel 1 (
    echo.
    echo [ERROR] Build failed. Read the Gradle message above.
    echo   "not enough space on the disk"     - free space on C:
    echo   "daemon disappeared unexpectedly"  - lower org.gradle.jvmargs
    echo   "Failed to install ... platforms"  - open Android Studio SDK Manager
    pause
    exit /b 1
)

set "APK=build\app\outputs\flutter-apk\app-debug.apk"
if not exist "%APK%" (
    echo [ERROR] Build reported success but %APK% is missing.
    pause
    exit /b 1
)

echo.
echo ============================================================
echo   APK ready:
echo     %CD%\%APK%
echo.
echo   It will talk to:  %API_BASE%
echo.
echo   Before installing on a phone:
echo     1. run_backend.bat %PORT%          (must stay running)
echo     2. allow_backend_firewall.bat %PORT%   (once, needs admin)
echo     3. phone on the same Wi-Fi as this PC
echo.
echo   Quick check from the phone's browser:
echo     %API_BASE%/device/exchange/AppTerms_get
echo ============================================================
echo.

rem ============================================================
rem  6) Offer to install on an attached device / emulator
rem ============================================================
set "SDK=%ANDROID_HOME%"
if not defined SDK set "SDK=%LOCALAPPDATA%\Android\Sdk"
set "ADB=%SDK%\platform-tools\adb.exe"

if exist "%ADB%" (
    set "DEV="
    for /f "skip=1 tokens=1,2" %%a in ('"%ADB%" devices 2^>nul') do (
        if "%%b"=="device" if not defined DEV set "DEV=%%a"
    )
    if defined DEV (
        echo Attached device found: !DEV!
        choice /c YN /n /m "Install on it now? [Y/N] "
        if !errorlevel!==1 (
            "%ADB%" install -r "%APK%"
            "%ADB%" shell monkey -p com.rhalla.rhalla_agent -c android.intent.category.LAUNCHER 1 >nul 2>&1
            echo Launched.
        )
    )
)

pause
endlocal
