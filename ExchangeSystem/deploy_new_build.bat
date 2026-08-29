@echo off
REM ---------------------------------------------------------------------------
REM Close the app, do a full REBUILD, and start it.
REM
REM Why a full Rebuild and not just "copy obj\Debug\ExchangeSystem.exe":
REM a compile-only build (msbuild -t:Compile) produces an exe with NO embedded
REM .resources, and the app then crashes on startup with
REM   System.Resources.MissingManifestResourceException in FRMMAIN.InitializeComponent()
REM Only a real Build/Rebuild runs resgen and embeds the ~445 form resources, and
REM it also writes straight to bin\Debug, so there is nothing to copy by hand.
REM ---------------------------------------------------------------------------
cd /d "%~dp0"

echo Closing ExchangeSystem if it is running...
taskkill /IM ExchangeSystem.exe /F >nul 2>&1
timeout /t 2 /nobreak >nul

REM locate MSBuild via vswhere (works regardless of VS edition/year)
set "MSBUILD="
for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe 2^>nul`) do set "MSBUILD=%%i"
if not defined MSBUILD (
  echo Could not locate MSBuild via vswhere. Open the solution in Visual Studio and Rebuild instead.
  pause
  exit /b 1
)

echo Rebuilding (this embeds form resources)...
"%MSBUILD%" "ExchangeSystem\ExchangeSystem.vbproj" -t:Rebuild -p:Configuration=Debug -v:m -nologo
if errorlevel 1 (
  echo BUILD FAILED - see the errors above.
  pause
  exit /b 1
)

echo Starting...
start "" "ExchangeSystem\bin\Debug\ExchangeSystem.exe"
