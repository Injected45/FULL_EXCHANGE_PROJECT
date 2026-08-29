# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Windows Forms desktop application (VB.NET, .NET Framework 4.8) for شركة الرحالة للصرافة (Alrhalla Exchange Company, Libya). Domain: currency buy/sell, internal/external money transfers, cash safes, branches, customers, employee salaries, and Central Bank of Libya reporting. The entire UI is Arabic and right-to-left.

## Build

Classic (non-SDK) MSBuild project — there is no dotnet CLI support; use MSBuild/Visual Studio:

```
nuget restore ExchangeSystem.sln
msbuild ExchangeSystem.sln /p:Configuration=Debug
```

Requirements that will break a clean checkout:
- **DevExpress v25.1** (licensed) must be installed on the machine — referenced from the GAC, not NuGet.
- `MetroFramework.dll` and `RestSharp.dll` are referenced via HintPath into a sibling repo: `..\..\..\shippingsystem\ShippingSystem\ShippingSystem\bin\Debug\`.
- NuGet packages restore to `..\packages` (solution-level `packages.config` style).

There are no tests and no linter. Startup form is `FRMMAIN` (see `My Project\Application.myapp`).

## Architecture

Single project `ExchangeSystem/ExchangeSystem.vbproj`. New forms/classes must be added to the `.vbproj` manually (no globbing).

### Two parallel data-access paths (both talk to SQL Server)

1. **Raw ADO.NET through global helpers (dominant pattern).** `Module1.vb` holds a global `SQLCON As SqlConnection` and the helpers `RUN_QUARY_TXT`, `RUN_QUARY_PRO`, `RUN_QUARY_PRO_ONLY`, `RUN_EXUTE_TXT`, `RUN_EXUTE_PRO`, `GETMAXID`. Nearly all business logic lives in **SQL Server stored procedures**; the VB code only marshals parameters. The `CLSFRM\CLS*.vb` classes are thin per-entity wrappers (e.g. `CLSSAFE.INSERTTB__Store` → stored proc `SAFETB_Insert`). Changing behavior usually means changing a stored proc in the database, not just VB code — the procs are not in this repo.

2. **LINQ to SQL** via `DAL\db.dbml` / `DataClasses1DataContext`, used by newer code such as `CLSFRM\Session.vb` and the user-permission system. Its connection string comes from `My.Settings.EXCHANGESYSConnectionString`.

### Database connection selection

`Module1.OPENCONNECTION()` (in `ExchangeSystem\Module1.vb`, ~line 76) hardcodes the connection string; environments (production server, test servers, local) are switched by commenting/uncommenting lines there, and the active one also sets the version caption shown in `FRMMAIN`. The database is `EXCHANGESYS2026`. `App.config` connection strings exist but the runtime path used everywhere is `OPENCONNECTION()`. Do not casually change the active connection string.

### Global state

`Module1.vb` also declares global session variables used throughout the app: `UserID`, `BID` (branch ID), `MAINBID`, `UserType`, `DefaultCurrency`, exchange-rate globals (`BrRate`, `withAgentRate`, …), etc. Forms read these directly. `MDD\MD_PUBLIC.VB` holds shared UI helpers and the `NUMBER_CHECK`/`NUMBER_FORM` flags used by the CLS classes.

### Permissions

Per-screen access control: `CLSFRM\Session.vb` loads a `ScreensAccessProfile` list (CanOpen/CanAdd/CanEdit/CanDelete/CanPrint/CanShow per screen) from `UserAccessProfileDetails` at login, keyed by the user's profile. Screens are registered in `CLSFRM\ScreensAccessProfile.vb`. Admin UI lives in `FORMS\UserPerForms\`.

### Folder layout / naming conventions

- `FORMS\` — all screens, grouped by domain subfolder (`BASICINFO`, `NewCurrencySaleandBuy`, `EXTERNALTRANSFORMS`, `SALARYSTATEMENTS`, `TrasferSafes`, `USERSFRM` for login/OTP, `CentralBank_Of_Libya`, …). Entry forms are `FRM*`; list/browse forms are `FRMVIEW*` / `FrmView*`.
- `FORMSEXCHANGE\` — internal/income/outcome transfer forms.
- `CLSFRM\` — per-entity data-access classes (`CLS*`), plus `Session`, `Master`, number-to-Arabic-words converters.
- `REPORTS\` — DevExpress XtraReports (`RPT*`), grouped by domain like FORMS.
- `MSGFORMS\` — reusable message/confirmation dialogs (saved, edit, remove).
- `MDD\`, `Module1-3.vb` — global modules (connection, helpers, session globals).
- `modeles_orcontrolls\` — external integrations and shared control standards.
- `Helper\` — DevExpress grid cell-merge helpers.

### External integrations

- **WhatsApp** messaging through a self-hosted gateway at `wa.rhalla.online` (`WatsapChick.vb`, `modeles_orcontrolls\SandWatsappMasggs.vb`); session id/API key are set in `Module1.vb`. Used for OTP login (`FORMS\USERSFRM\FrmLogInOTP.vb`) and transfer notifications.
- **Pusher** realtime channels (`modeles_orcontrolls\pusher_Mo.vb`) started after login, plus `SqlTableDependency` for DB change notifications.
- DevExpress WXI skin with "Droid Arabic Kufi" as the app font (App.config).

## Conventions

- UI text, messages, and report captions are in Arabic; keep new user-facing strings in Arabic and respect RTL layouts.
- Naming is legacy-style: ALL-CAPS form/class/method names with underscores (e.g. `SERACH_SAFE`, `INSERTTB__Store`), including existing typos — match the surrounding style; renaming public members ripples through designer files.
- `Option Strict` is Off; implicit conversions are everywhere. Designer files (`*.Designer.vb`) are generated — edit through the VS designer or with care.
