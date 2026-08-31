@echo off
setlocal

rem ============================================================
rem  Opens the Windows Firewall for the backend dev server so a
rem  phone on the same Wi-Fi can reach it.
rem
rem  Run once. Requires administrator - it elevates itself.
rem
rem  Usage:  allow_backend_firewall.bat [port]      (default 8000)
rem
rem  Scoped to the Private profile on purpose: this must not open
rem  the port on a public/untrusted network.
rem ============================================================

set "PORT=%~1"
if "%PORT%"=="" set "PORT=8000"

net session >nul 2>&1
if errorlevel 1 (
    echo Requesting administrative privileges...
    powershell -NoProfile -Command "Start-Process -FilePath '%~f0' -ArgumentList '%PORT%' -Verb RunAs"
    exit /b 0
)

set "RULE=Rhalla backend dev (TCP %PORT%)"

echo Removing any previous rule named "%RULE%" ...
netsh advfirewall firewall delete rule name="%RULE%" >nul 2>&1

echo Adding inbound allow rule for TCP %PORT% on the Private profile...
netsh advfirewall firewall add rule name="%RULE%" dir=in action=allow protocol=TCP localport=%PORT% profile=private
if errorlevel 1 (
    echo.
    echo [ERROR] Could not add the firewall rule.
    pause
    exit /b 1
)

echo.
echo Done. Verify the network profile is Private ^(not Public^):
powershell -NoProfile -Command "Get-NetConnectionProfile | Select-Object Name,NetworkCategory | Format-Table -AutoSize"
echo.
echo If NetworkCategory is Public, this rule will NOT apply. Change it with:
echo   Set-NetConnectionProfile -InterfaceAlias "<name>" -NetworkCategory Private
echo.
pause
endlocal
