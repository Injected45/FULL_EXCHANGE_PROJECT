@echo off
setlocal
title ExchangeSystem - Build and Run
cd /d "%~dp0"

rem ============================================================
rem  1) Locate MSBuild (Visual Studio 2022)
rem ============================================================
set "MSBUILD="
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if exist "%VSWHERE%" (
    for /f "usebackq delims=" %%i in (`"%VSWHERE%" -latest -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe"`) do set "MSBUILD=%%i"
)
if not defined MSBUILD set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
if not exist "%MSBUILD%" (
    echo [ERROR] MSBuild not found. Install Visual Studio 2022.
    pause
    exit /b 1
)
echo Using MSBuild: %MSBUILD%

rem ============================================================
rem  2) Restore NuGet packages (only if packages folder missing)
rem ============================================================
if not exist "packages\" (
    echo Restoring NuGet packages...
    "%MSBUILD%" ExchangeSystem.sln -t:Restore -p:RestorePackagesConfig=true -v:minimal -nologo
    if errorlevel 1 (
        echo [ERROR] NuGet restore failed.
        pause
        exit /b 1
    )
)

rem ============================================================
rem  3) Build (Debug)
rem ============================================================
echo Building ExchangeSystem...
"%MSBUILD%" ExchangeSystem.sln -p:Configuration=Debug -v:minimal -nologo
if errorlevel 1 (
    echo.
    echo [ERROR] Build failed - see errors above.
    pause
    exit /b 1
)

rem ============================================================
rem  4) Run
rem ============================================================
set "EXE=ExchangeSystem\bin\Debug\ExchangeSystem.exe"
if not exist "%EXE%" (
    echo [ERROR] %EXE% not found after build.
    pause
    exit /b 1
)
echo.
echo Starting ExchangeSystem...
start "" "%EXE%"
exit /b 0
