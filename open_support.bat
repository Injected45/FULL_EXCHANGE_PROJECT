@echo off
setlocal EnableDelayedExpansion
title Rhalla Support Center

rem ============================================================
rem  Opens the Rhalla support centre. Double-click, nothing else.
rem
rem    1) checks the Laravel server, starts it if it is down
rem    2) waits until it actually answers (not a fixed sleep)
rem    3) opens Firefox on the page
rem
rem  ASCII only, on purpose. Arabic text inside a .bat breaks cmd
rem  parsing regardless of `chcp` - the interpreter reads the file
rem  in the OEM codepage before any chcp on line 1 can take effect,
rem  so every Arabic `rem` line was being executed as a command.
rem  The rest of this repo's .bat files are English for the same
rem  reason; the Arabic belongs in the UI, not in the tooling.
rem
rem  Does NOT start a second server when one is already up: two
rem  `artisan serve` on one port means the second dies silently
rem  and the user believes it is running.
rem ============================================================

set "PORT=8000"
set "URL=http://localhost:%PORT%/support"

rem ---------- 1) Is the port already taken? --------------------
set "RUNNING="
for /f "tokens=*" %%L in ('netstat -ano ^| findstr /r /c:"LISTENING" ^| findstr ":%PORT% "') do set "RUNNING=1"

if defined RUNNING (
    echo [OK] Server already listening on port %PORT%.
    goto :open
)

rem ---------- 2) Start it -------------------------------------
set "PHP="
if defined RHALLA_PHP if exist "%RHALLA_PHP%" set "PHP=%RHALLA_PHP%"
if not defined PHP if exist "C:\xampp\php\php.exe" set "PHP=C:\xampp\php\php.exe"
if not defined PHP for /f "delims=" %%i in ('where php.exe 2^>nul') do if not defined PHP set "PHP=%%i"

if not defined PHP (
    echo [ERROR] PHP not found.
    echo         Install XAMPP, or set RHALLA_PHP to a php.exe path.
    pause
    exit /b 1
)

echo [..] Starting server...
rem  0.0.0.0 so a phone on the same Wi-Fi can reach it too, not
rem  only this machine.
cd /d "%~dp0backend"
start "Rhalla API" /min "%PHP%" artisan serve --host=0.0.0.0 --port=%PORT%

rem ---------- 3) Wait until it really answers ------------------
rem  Not a fixed `timeout 5`: on a slow machine the browser opens
rem  before the server is ready and shows an error page; on a fast
rem  one it waits for nothing.
echo [..] Waiting for the server...
set "READY="
for /l %%i in (1,1,40) do (
    if not defined READY (
        powershell -NoProfile -Command "try{ Invoke-WebRequest 'http://127.0.0.1:%PORT%/up' -UseBasicParsing -TimeoutSec 2 ^| Out-Null; exit 0 }catch{ exit 1 }" >nul 2>&1
        if !errorlevel! equ 0 (set "READY=1") else (ping -n 2 127.0.0.1 >nul)
    )
)

if not defined READY (
    echo [ERROR] Server did not answer. Run run_backend.bat to see why.
    pause
    exit /b 1
)

:open
echo.
echo   ============================================
echo     %URL%
echo.
echo     user : admin
echo   ============================================
echo.

rem ---------- 4) Open Firefox specifically ---------------------
rem
rem  Not left to `start ""`: the default handler on this machine
rem  can be the code editor, which opens the page in an internal
rem  tab instead of a real browser window. That is what happened.
set "FF="
if exist "%ProgramFiles%\Mozilla Firefox\firefox.exe" set "FF=%ProgramFiles%\Mozilla Firefox\firefox.exe"
if not defined FF if exist "%ProgramFiles(x86)%\Mozilla Firefox\firefox.exe" set "FF=%ProgramFiles(x86)%\Mozilla Firefox\firefox.exe"
if not defined FF if exist "%LOCALAPPDATA%\Mozilla Firefox\firefox.exe" set "FF=%LOCALAPPDATA%\Mozilla Firefox\firefox.exe"

if defined FF (
    start "" "%FF%" -new-window "%URL%"
) else (
    rem  No Firefox: fall back to the default handler rather than
    rem  not opening the page at all.
    start "" "%URL%"
)

ping -n 3 127.0.0.1 >nul
endlocal
