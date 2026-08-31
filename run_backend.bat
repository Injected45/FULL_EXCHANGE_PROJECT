@echo off
setlocal enabledelayedexpansion
title Rhalla - Backend (php artisan serve)
cd /d "%~dp0backend"

rem ============================================================
rem  Rhalla backend dev server.
rem
rem  Serves on 0.0.0.0 so the Android emulator can reach it at
rem  10.0.2.2 - binding to 127.0.0.1 would make the app unreachable.
rem
rem  Usage:  run_backend.bat [port]      (default 8000)
rem ============================================================

set "PORT=%~1"
if "%PORT%"=="" set "PORT=8000"

rem ============================================================
rem  1) Locate PHP  (not on PATH by default - XAMPP install)
rem ============================================================
set "PHP="
for /f "delims=" %%i in ('where php 2^>nul') do if not defined PHP set "PHP=%%i"
if not defined PHP if exist "C:\xampp\php\php.exe" set "PHP=C:\xampp\php\php.exe"
if not defined PHP (
    echo [ERROR] PHP not found on PATH and not at C:\xampp\php\php.exe
    echo         Install PHP 8.2 or edit this script.
    pause
    exit /b 1
)
echo Using PHP: %PHP%
"%PHP%" -r "echo 'PHP '.PHP_VERSION.' '.(PHP_ZTS?'ZTS':'NTS').PHP_EOL;"

rem ============================================================
rem  2) Check the SQL Server driver
rem     Without sqlsrv every database call fails at runtime.
rem ============================================================
"%PHP%" -m | findstr /i /c:"pdo_sqlsrv" >nul
if errorlevel 1 (
    echo.
    echo [WARN] pdo_sqlsrv is NOT loaded - every DB endpoint will fail.
    echo        Install msphpsql and enable it in php.ini:
    echo          extension=php_sqlsrv_82_ts.dll
    echo          extension=php_pdo_sqlsrv_82_ts.dll
    echo.
)

rem ============================================================
rem  3) Dependencies
rem     --no-dev is deliberate: laravel/pint requires PHP 8.3.
rem ============================================================
if not exist "vendor\autoload.php" (
    echo Installing composer dependencies...
    set "COMPOSER="
    for /f "delims=" %%i in ('where composer 2^>nul') do if not defined COMPOSER set "COMPOSER=%%i"
    if defined COMPOSER (
        call "!COMPOSER!" install --no-dev --no-interaction
    ) else if exist "C:\ProgramData\ComposerSetup\bin\composer.phar" (
        "%PHP%" "C:\ProgramData\ComposerSetup\bin\composer.phar" install --no-dev --no-interaction
    ) else (
        echo [ERROR] composer not found. Install it, or run composer install manually.
        pause
        exit /b 1
    )
    if errorlevel 1 (
        echo [ERROR] composer install failed.
        pause
        exit /b 1
    )
)

rem ============================================================
rem  4) .env must exist - it is git-ignored, so a fresh clone has none
rem ============================================================
if not exist ".env" (
    echo [ERROR] backend\.env is missing ^(it is git-ignored^).
    echo         Copy .env.example and set the DB_* keys.
    pause
    exit /b 1
)

rem ============================================================
rem  5) Clear cached config so .env edits actually take effect
rem ============================================================
echo Clearing config cache...
"%PHP%" artisan config:clear

rem ============================================================
rem  6) Serve
rem ============================================================
rem  LAN address, for testing on a real phone over Wi-Fi.
set "LANIP="
for /f "delims=" %%i in ('powershell -NoProfile -Command "(Get-NetIPAddress -AddressFamily IPv4 ^| Where-Object { $_.IPAddress -notlike '127.*' -and $_.IPAddress -notlike '169.254.*' } ^| Select-Object -First 1 -ExpandProperty IPAddress)" 2^>nul') do set "LANIP=%%i"

echo.
echo ============================================================
echo   Backend      http://localhost:%PORT%/api
echo   Emulator     http://10.0.2.2:%PORT%/api
if defined LANIP echo   Phone (LAN)  http://%LANIP%:%PORT%/api
echo.
echo   Start the app with:
echo     run_app_emulator.bat
echo.
echo   For a real phone the APK must be built with that LAN address
echo   baked in, and the address must also be listed in
echo   android/app/src/debug/res/xml/network_security_config.xml
echo   Firewall: run allow_backend_firewall.bat once.
echo.
echo   Ctrl+C to stop.
echo ============================================================
echo.

"%PHP%" artisan serve --host=0.0.0.0 --port=%PORT%

if errorlevel 1 (
    echo.
    echo [ERROR] artisan serve exited with an error ^(port %PORT% already in use?^).
    pause
    exit /b 1
)

endlocal
