@echo off
setlocal EnableDelayedExpansion
title Rhalla Backend - Laravel API
cd /d "%~dp0backend"

rem ============================================================
rem  Rhalla backend - install check + local API server.
rem
rem  Usage:  run_backend.bat [port]          default port 8000
rem
rem  Serves on 0.0.0.0 so both the Android emulator (10.0.2.2)
rem  and a physical phone on the same Wi-Fi can reach it.
rem
rem  This NEVER runs `php artisan migrate`. database/migrations/
rem  is the stock Laravel skeleton and does not describe the live
rem  EXCHANGESYS2026 schema - running it would drop live tables.
rem ============================================================

set "PORT=%~1"
if not defined PORT set "PORT=8000"

rem ---------- 1) Locate PHP -----------------------------------
set "PHP="
if defined RHALLA_PHP if exist "%RHALLA_PHP%" set "PHP=%RHALLA_PHP%"
if not defined PHP if exist "C:\xampp\php\php.exe" set "PHP=C:\xampp\php\php.exe"
if not defined PHP (
    for /f "delims=" %%i in ('where php.exe 2^>nul') do if not defined PHP set "PHP=%%i"
)
if not defined PHP (
    echo [ERROR] PHP not found.
    echo         Install XAMPP ^(PHP 8.2^) or set RHALLA_PHP to a php.exe.
    pause
    exit /b 1
)
echo Using PHP: %PHP%
for /f "delims=" %%v in ('"%PHP%" -r "echo PHP_VERSION;" 2^>nul') do echo PHP version: %%v

rem ---------- 2) SQL Server driver check ----------------------
rem  The API talks raw T-SQL over sqlsrv. Without the extension
rem  the server still starts and every endpoint 500s on connect,
rem  which is a confusing way to find out. Fail here instead.
"%PHP%" -m | findstr /i /c:"sqlsrv" >nul
if errorlevel 1 (
    echo [ERROR] The sqlsrv PHP extension is not loaded.
    echo         Download msphpsql v5.11.1 ^(Windows-8.2.zip^), copy the DLLs
    echo         matching this PHP build ^(ZTS/NTS, x64^) into the ext folder,
    echo         then enable both in php.ini:
    echo             extension=php_sqlsrv_82_ts.dll
    echo             extension=php_pdo_sqlsrv_82_ts.dll
    echo         ODBC Driver 17 or 18 for SQL Server is required as well.
    pause
    exit /b 1
)
echo sqlsrv: loaded

rem ---------- 3) Composer dependencies ------------------------
if not exist "vendor\autoload.php" (
    echo.
    echo vendor/ is missing - installing Composer dependencies...

    set "COMPOSER_PHAR="
    if exist "C:\ProgramData\ComposerSetup\bin\composer.phar" set "COMPOSER_PHAR=C:\ProgramData\ComposerSetup\bin\composer.phar"
    if not defined COMPOSER_PHAR if exist "composer.phar" set "COMPOSER_PHAR=composer.phar"
    if not defined COMPOSER_PHAR (
        echo [ERROR] composer.phar not found.
        echo         Install Composer from https://getcomposer.org/download/
        pause
        exit /b 1
    )

    rem --no-dev on purpose: laravel/pint requires PHP 8.3 and local PHP is 8.2.
    "%PHP%" "!COMPOSER_PHAR!" install --no-dev --no-interaction
    if errorlevel 1 (
        echo [ERROR] composer install failed.
        pause
        exit /b 1
    )
)
echo Dependencies: installed

rem ---------- 4) .env -----------------------------------------
if not exist ".env" (
    echo [ERROR] backend\.env is missing.
    echo         It is git-ignored and holds credentials, so it is never
    echo         committed. Create it from .env.example and point DB_* at
    echo         the LOCAL SQL Server, not production.
    pause
    exit /b 1
)

rem ---------- 5) APP_KEY --------------------------------------
findstr /b /c:"APP_KEY=base64:" .env >nul
if errorlevel 1 (
    echo APP_KEY is empty - generating...
    "%PHP%" artisan key:generate --force
    if errorlevel 1 (
        echo [ERROR] key:generate failed.
        pause
        exit /b 1
    )
)

rem ---------- 6) Clear stale cached config --------------------
"%PHP%" artisan config:clear >nul 2>&1
"%PHP%" artisan route:clear  >nul 2>&1

rem ---------- 7) Report the database being served -------------
echo.
echo ============================================================
for /f "tokens=1,* delims==" %%a in ('findstr /b /c:"DB_HOST=" /c:"DB_DATABASE=" /c:"DB_CONNECTION=" .env') do echo  %%a = %%b
echo ============================================================
echo.

rem ---------- 8) Serve ----------------------------------------
echo Starting API on port %PORT% ...
echo   this machine     http://localhost:%PORT%/api
echo   Android emulator http://10.0.2.2:%PORT%/api
for /f "tokens=2 delims=:" %%i in ('ipconfig ^| findstr /c:"IPv4"') do echo   this LAN        http://%%i:%PORT%/api
echo.
echo Press Ctrl+C to stop.
echo.
"%PHP%" artisan serve --host=0.0.0.0 --port=%PORT%

if errorlevel 1 (
    echo.
    echo [ERROR] The server exited with an error.
    pause
    exit /b 1
)
endlocal
