# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## The goal of this project

**We are building a set of Flutter mobile apps — one per role — on top of the existing Laravel backend.** The first is the **الوكيل (agent / broker)** app.

That fixes the role of each existing directory:

- **`backend/` is the API we build against.** It is the backend for the new apps. Extend it where an endpoint is missing; do not replace it.
- **`ExchangeSystem/` is reference material, not a build target.** When a business rule, a field's meaning, a calculation, or a workflow is unclear, read the desktop app (and its stored procedures) to find out how the business actually does it. Do not modify it as part of Flutter work.

### Reference docs — read these before building

- **[docs/agent-api.md](docs/agent-api.md)** — the API contract, derived by reading the backend source. Roles, auth flow, every endpoint the agent app needs, and the response-envelope traps.
- **[docs/design-system.md](docs/design-system.md)** — tokens, typography, components, RTL and numeral rules, extracted from the Claude Design project.

### Decisions taken

| Decision | Choice |
|---|---|
| Structure | One Flutter app now (`rhalla_agent/`), layered internally so a shared package can be split out when the second app starts |
| Platforms | Android + iOS (iOS cannot be built on this Windows machine — needs a Mac) |
| Auth | **OTP-only.** The design has no password screen anywhere; the API requires one. We add a backend endpoint that issues a Sanctum token after a *server-verified* OTP, which also closes the public `update/password` takeover hole |
| Self-registration | **Removed from the implementation.** There is no self-registration in the API — `register` only claims a pre-provisioned row. Agents are created from the desktop back office. The design's `isInfo` (name / nationality / national ID / city) step is skipped; the file keeps it for a possible future customer app |
| Design scope | The inherited design was a **customer wallet** (receive-only, single wallet, tabs: الرئيسية · استعلامات · الدردشة · الحساب). The agent needs sending, POS management, commissions, limits and delivery, so those screens were designed in the same visual system first and are now built (the `design/` artboards that held them are gone — see below). The app's four tabs are الرئيسية · الحوالات · نقاط البيع · الحساب (`/`, `/transfers`, `/pos`, `/account`), not the wallet's |

### The Flutter app — `rhalla_agent/`

```bash
cd rhalla_agent
flutter run --dart-define=API_BASE=http://102.214.165.242:8080/api   # production
flutter analyze                          # the only linter; it is currently clean — keep it that way
flutter test                             # 26 tests: envelope_test.dart + session_expiry_test.dart
flutter test test/envelope_test.dart --plain-name 'Fmt'   # one group; --plain-name is a substring, --name a regexp
flutter build apk --release --dart-define=API_BASE=...    # needs android/key.properties — see below
```

**`--dart-define=API_BASE` is not optional in practice.** `kApiBase` in `core/net/api_client.dart` defaults to the **live production** server, so a bare `flutter run` moves real money against real accounts. Always pass the target you mean.

#### Release-only failures the debug build cannot show you

Two defects lived here precisely because debug builds hide them. Both are fixed; the point is that **debug success proves nothing about release** on this app — verify against a built APK.

- **`INTERNET` was missing from the release manifest.** Flutter's template declares it in `src/debug/` and `src/profile/` only, and `src/main/AndroidManifest.xml` declared no permission at all — so `app-release.apk` shipped with `DUMP` as its *only* permission and could not open a socket. Every screen would have failed at the first real user. It is now in the main manifest. To check a build rather than trust it, read the permissions straight out of the APK:

  ```bash
  python -c "import zipfile,re;m=zipfile.ZipFile('build/app/outputs/flutter-apk/app-release.apk').read('AndroidManifest.xml');print(sorted(set(re.findall(r'android\.permission\.[A-Z_]+',m.decode('utf-16-le','ignore')+m.decode('latin-1','ignore')))))"
  ```

- **Release was signed with the debug keystore** (the stock `// TODO` left in place), which Google Play rejects and which would have made the app un-upgradable. `android/app/build.gradle.kts` now reads `android/key.properties` and **throws a `GradleException` when it is absent** rather than falling back — a silent fallback is how the debug-key APK got built in the first place. Copy `android/key.properties.template`, generate the keystore with the `keytool` line inside it, and keep the `.jks` and its password off this machine. **The key cannot be replaced after the first Play release.**

**Transport is still HTTP, and that is the remaining launch blocker.** `http://102.214.165.242:8080` is unreachable from a release build on either platform: Android blocks cleartext by default at this `targetSdk` (the `network_security_config` exception is debug-scoped), and iOS blocks it via ATS. The fix is a TLS certificate on the server — **not** `usesCleartextTraffic` and **not** an ATS exception, both of which would ship an exchange app that transmits balances and transfer codes in the clear.

Stack: **Riverpod + go_router + dio + flutter_secure_storage**, hand-written models, **no build_runner**. Codegen was deliberately skipped: several endpoints return raw SQL result sets whose columns are not knowable from the backend source, so tolerant hand-parsing beats generated strict models.

Layers: `core/theme` (tokens → Dart) · `core/net` (dio + the envelope decoder) · `core/storage` (secure store, device id) · `core/format` (money, phone, Western numerals) · `ui/widgets` (ambient background, glass, controls) · `features/*` · `router.dart`.

Three one-letter statics carry the whole visual and numeric system; reach for them before writing a literal:

- **`R`** (`core/theme/tokens.dart`) — colours, gradients, radii. `R.primary`, `R.inkA(.55)`, `R.rCard`.
- **`T`** (`core/theme/app_theme.dart`) — typography. `T.kufi(...)` / `T.plex(...)` and named roles (`T.title`, `T.amountHero`, `T.cta`).
- **`Fmt`** (`core/format/fmt.dart`) — `Fmt.money` (**2 decimals**, LYD), `Fmt.rate` (4), `Fmt.phone` / `Fmt.phoneForApi`, `Fmt.num_` for the raw-SQL values that arrive as strings, and `WesternDigits`, a `TextInputFormatter`.

  Two decisions in there that look like bugs and are not. **`Fmt.money` is `#,##0.00#` — two decimals, and a third only when it carries a value** (`1,000.00`, but `1,000.325`), by owner decision (30 Aug 2026). Both halves matter: the dinar is 1000 dirham, so a fixed `#,##0.00` would hide real money and open accounting differences, while a fixed `#,##0.000` would show zeros that carry no meaning. Do not "simplify" it to either. And **every number in this app is Western — `0123456789`, never `٠١٢٣٤٥٦٧٨٩`, anywhere, for any reason.** That is why `Fmt` pins locale `'en'` rather than `'ar_LY'`, and why `WesternDigits` must come **first** in every `inputFormatters` list: the `FilteringTextInputFormatter` after it allows `[0-9]` only, so it would delete an Arabic-Indic digit before it could be converted — and the agent would just see a keyboard that types nothing.

  **Every field the agent types into takes an `AutoClearFocus`** (`core/format/fmt.dart`), which owns two owner decisions. It **empties the field the moment the caret enters it** — an agent's first keystroke must never join a leftover value and produce an amount nobody intended — and it **restores the previous value if the caret leaves without anything being typed**, because without that restore a stray tap silently wipes a transfer the agent had already filled in. With `formatOnExit: true` it also rewrites the amount in full display form on blur (`2500` → `2,500.00`); that happens on blur and not per keystroke, since forcing two decimals while typing makes entering a fraction impossible. Amounts, commissions, phone numbers, beneficiary names, notes and the transfer-code search all use it — the rule is not numeric-specific, which is why the class is not named after numbers.

  Every money input field uses the shared `moneyInputFormatters` list rather than its own — the order in it is correctness, not taste: `WesternDigits` → filter (which must allow `,`) → `ThousandsGrouping`. Grouping means the controller text now contains commas; that is safe **only** because every amount is read through `Fmt.num_`, which strips them. If you ever read `_amount.text` directly, strip the commas or you will send the server a different number than the agent typed.

  **The currency symbol always sits to the left of the amount** — `د.ل` first, then the number to its right, on every screen. Amount rows are wrapped in `Directionality(textDirection: TextDirection.ltr)` (a number is an LTR run and must not be reordered by the RTL paragraph), so in those rows the currency `Text` comes **first** in the `children` list, and amount fields use `prefixText`, never `suffixText`. The symbol itself is never hardcoded: it comes from the server as `currencyCode`, with `د.ل` only as a fallback.

Each feature follows the same three-part shape in `features/<name>/<name>_repository.dart`: hand-written models, a plain `Provider` exposing the repository, and one `FutureProvider.autoDispose` per screen-load (`.family` when it takes an argument). Screens `ref.watch` those and never call the `ApiClient` directly. Follow it for new features rather than inventing a second pattern.

Four things that will bite anyone changing this code:

1. **`core/net/api_envelope.dart` is not boilerplate — it is the whole defence** against the backend's inconsistent envelope. `test/envelope_test.dart` encodes every real deviation found in the source. Run it before changing the parser.
2. **`AmbientBackground` is mounted above the `Navigator`** in `main.dart`, deliberately — putting it per-screen restarts the 24–31s orbit animations on every push.
3. **A 401 must sign the agent out.** `ApiClient.onUnauthorized` is wired in `authControllerProvider` — not in `core/net`, because `core` must not import `features`, and because the router always watches that provider so the wiring cannot be forgotten. Without it a dead token stays in secure storage and the router keeps the agent "inside" the app, facing an error on every screen with no way out but a manual logout. `test/session_expiry_test.dart` pins the behaviour: fires once for concurrent 401s, resets after a success, and **never** fires on 403 (that is the POS-permission path, not an expired session).
4. **`SecureStore.deviceId()` must never be regenerated.** The backend binds one device per user and refuses a mismatch; only a back-office `Reg='NO'` reset recovers the account. See the warning in that file — reinstalling the app currently locks the user out, and the fix (a stable hardware id, or a server re-bind path) is still open.

**Login works end to end.** `POST /device/otp/login` was added to the backend (`AuthController::otpLogin`) — it verifies the OTP *server-side*, consumes it, rebinds the device, and issues a Sanctum token. See [docs/agent-api.md](docs/agent-api.md). `AuthRepository.loginWithPassword` remains as a fallback for the existing password path.

Two facts that only surfaced by running it against a real database, both now encoded in the app:

- **The OTP is 4 digits, not 6.** The design drew six cells and its copy said six; the server generates `rand(1000, 9999)`. Six cells make login impossible.
- **`Daily_transfer` returns ceilings, not consumption.** Labelling it "transferred today" misleads an agent about what they can still send. No endpoint returns consumption, so the home card shows the ceiling and says so.
- **`external/get/exchange`'s `sale_price` is not the amount the beneficiary receives.** It calls `SalePrice_mo_Value($currency_id, …)` while the `ExternalEx` trigger — the thing that actually computes the transfer — calls `SalePrice_mo_Value(CountryIDTo, …)`. Measured against the live database: 5 LYD to Egypt gives 2 from the endpoint and **19** in the row that was actually written. Quoting a customer from it is wrong, not approximate.
- **Account-to-account transfers are rate-limited to one per 3 minutes** by the server (`accounts_repository.dart:138`). Nothing in the API documents it; the form warns about it because the rejection is otherwise unexplainable to the agent.
- **Account-to-account commission is set by the server, and its band table has holes.** `Transfer_commissions` covers nothing between 10,000–11,000, nothing between 15,000–16,000, and nothing above 100,000, so `transInsert` hard-fails with 422 after the agent has filled the form. It also charges 2000 in the 20,000–25,000 band against 160 and 250 either side — a data error worth raising with the back office.

**Three endpoints were added to the backend** (all read-only except the login one), because the app could not be correct without them:

| Endpoint | Why |
|---|---|
| `POST device/otp/login` | Exchange a server-verified OTP for a Sanctum token |
| `POST device/external/quote` | Mirrors the `ExternalEx` trigger's arithmetic literally, so the agent quotes the customer the number that will actually be written |
| `POST device/internal/trans/between/accounts/commission` | Same query `transInsert` uses, so a band gap is caught before the form is filled |

The two quote endpoints duplicate logic that lives in a trigger and in `transInsert`. **If either of those changes, these must change with them** — the comments in both say so.

- **`InternalEx_SelectType_View_statetosForok` is not "outgoing transfers".** It filters on `ACCID_FRom`, which the *delivery* endpoint writes with the account that delivered — not the account that sent. On the live database, an agent who has created transfers gets **zero rows**, and the 141 rows that do carry an `ACCID_FRom` all have `AccFrom = 0`. It is a delivery log; the app labels it «سلَّمتُها». Sent transfers come from the statement (`ExchangeAccData`).

**Fonts are bundled, not fetched.** `google_fonts` is gone — the agent works in branches that may be offline, and a font arriving late means a fallback flash on every screen. Noto Kufi is the **variable** file, so `T.kufi` passes `fontVariations` as well as `fontWeight`: weight alone does not move a variable axis. IBM Plex ships three static weights (400/500/600) — do not use w700+ with it.

**iOS is prepared as far as Windows allows.** The app icons (`ios/Runner/Assets.xcassets/AppIcon.appiconset/`, all 15 sizes, RGB with no alpha — the App Store rejects alpha in the 1024) and `CFBundleDisplayName` are done. What still needs a Mac is only the toolchain: `xcodebuild`, the iOS SDK, CocoaPods (`ios/Podfile` does not exist yet — it is generated on the first Mac build), and `codesign`. A cloud macOS runner (Codemagic / GitHub Actions / Bitrise / Xcode Cloud) satisfies all of it without buying hardware.

Note `Info.plist` has **no `NSAppTransportSecurity` key**, which is correct: iOS blocks cleartext HTTP by default and production must be HTTPS. Do not add an ATS exception to reach `http://102.214.165.242:8080` — fix the transport instead. (The Android debug build's cleartext allowance is scoped to the emulator host only and cannot reach release.)

**The device id is now derived from hardware**, closing the reinstall lockout: `ANDROID_ID` through a method channel in `MainActivity.kt`, `identifierForVendor` on iOS, random as a last resort. **A stored id is never replaced** — accounts provisioned before this change are bound to a random id in the database, and overwriting it would lock every one of them out.

### `design/` — no longer on disk

The twenty-nine `.dc.html` artboards, `canvas.json`, `rhalla-agent-screens.html` and the four Node ESM generators that emitted them (`build.mjs` … `build4.mjs`, plus `fix.mjs`) are **gone from this tree, and were never committed** — the unification commit does not contain them, and there is no history to restore them from. Do not follow an instruction to `cd design`; it does not exist.

Nothing was lost that the app does not already hold: every one of those screens was built in code first and the artboards were derived from the actual widgets and API behaviour, so `rhalla_agent/lib/features/` is now the sole authority on what a screen looks like. What the artboards recorded and the code does not say out loud is the *reasoning*, which is why these three are kept here:

- **`BandGap`** is the commission-band 422 (`accounts_repository.dart:110` returns `null` when no band matches). It is red, not amber, and disables the button: the agent cannot proceed at all, so it is an obstacle and not a warning. It sits on the *form*, which is the point — the band gap is caught before the review, not after the form is filled.
- **`PosEdit`** puts the amber warning **above** the fields. `AuthorizedUsers_update` resets `Reg` to `'NO'` on every save, so fixing a typo in a name signs the point of sale out and forces it to re-register. The warning is the subject of that sheet, not a footnote on it.
- **`SignOut` and `DeleteAccount`** are deliberately opposite. Signing out keeps the device id, so nothing is lost and nothing is red. Deleting stops the account (`Reg='NO'` plus `deleted_at`, a soft delete) and cannot be undone from the app — so it is red, down to the confirm button.

Two states that are easy to forget exist because they are rare, not because they are edge cases: `EmptyTerms` — the terms are published from the back office and can legitimately be absent — and the city/branch picker, one pattern shared by the internal and external sheets.

### Design source

Claude Design project **"تطبيق محفظة رقمية ليبيا"** (`c2ba969c-1a10-40d6-a85d-08519bed9682`), read via the `DesignSync` tool.

- `Rhalla Full Flow.dc.html` — splash → onboarding → phone → OTP → info → done → app. **256.7 KB, so `get_file` truncates it at its 256 KiB cap** — the tail (the app shell) cannot be read this way. ~84% of the file is inert base64 Figma paste residue, not design.
- `Rhalla App.dc.html` — reads complete, and its `<script>` holds the full component logic: every dynamic value (OTP cell states, dot widths, all copy, the 45s resend timer, city/nationality lists), plus two alternative home screens and the 4-tab bottom nav.
- `android-frame.jsx` — a canvas-preview device bezel. **Not part of the app.**

Everything below describes the existing systems.

## Scope

**The root is the git repository.** The projects were unified into one monorepo in a single commit (`الدفعة الأولى: توحيد مشروع الرحالة للصرافة في مستودع واحد`); none of them carries its own `.git` any more. So `rhalla_agent/` **is** under version control now, one `git status` at the root sees every project at once, and a Flutter change plus the backend endpoint it needs belong in the same commit.

There is exactly one commit, so `git log` explains nothing about why anything is the way it is — this file and [migration/STATUS.md](ExchangeSystem/migration/STATUS.md) are the history.

It holds three projects serving the same business — شركة الرحالة للصرافة (Alrhalla Exchange, Libya) — the first two sharing one production database:

| Path | What it is | Stack |
|---|---|---|
| `ExchangeSystem/` | Back-office desktop app used by branch staff — **reference only** | VB.NET WinForms, .NET Framework 4.8, DevExpress v25.1 |
| `backend/` | REST API — **the backend for the new Flutter apps** | Laravel 11, PHP 8.2 |
| `rhalla_agent/` | The agent app being built | Flutter 3.41 / Dart 3.11 |
| `docs/` | The API contract and the design system, both derived by reading source | Markdown |

`.claude/skills/` holds nineteen general-purpose security skills (recon, malware analysis, cloud auditing, …) with their own Python scripts. They are **committed tooling, not part of this business** — nothing in `backend/`, `rhalla_agent/` or `ExchangeSystem/` calls them, and they are not a description of what this project does.

The root `.gitignore` is a **credential backstop, not a convenience.** It ignores `.env*`, `RhallaConfig.ini`, `key.properties`, `*.jks`, `*.keystore`, `*.pem`, `*.p12`, `*.pfx` and `auth.json` **at any depth**, deliberately duplicating what each sub-project already ignores, so that editing one sub-`.gitignore` cannot leak a secret. Keep the `!*.template` negations when adding to it.

[ExchangeSystem/CLAUDE.md](ExchangeSystem/CLAUDE.md) carries the detailed guidance for the desktop app — read it before changing anything there. This file covers what is only visible from the root: how the two halves relate, and the database split between them.

## The database split — read this first

Both projects target a database called `EXCHANGESYS2026`, but **through different engines**:

- **`backend/`** connects with `DB_CONNECTION=sqlsrv` to **SQL Server** (`148.251.245.41:1433`, user `sa`).
- **`ExchangeSystem/`** has been fully migrated to **MariaDB 10.4** on that same host (port 3306, user `exchange_app`) — 191 tables and 1041 routines, deployed and verified (see [migration/STATUS.md](ExchangeSystem/migration/STATUS.md)). Which engine it uses is a config switch, not a code change, and it still ships with `DB_ENGINE=SQLSERVER` as the default.

So the desktop app can run against either engine and **the API cannot**: its queries are raw T-SQL with `[dbo].[table]` bracket identifiers, `EXEC dbo.<proc>`, and `ISNULL`/`GETDATE`/`DATEDIFF`. Repointing `backend/` at MariaDB is a rewrite, not a config change. Before changing a stored procedure or a table, work out which engine's copy you are changing and whether the other side needs the same change.

## How the two halves talk

- **Shared database.** The API reads and writes the same operational tables the desktop app owns (`wallet`, `CurrencyMainTb`, transfers, …), mostly through `DB::table` and raw `DB::select` rather than Eloquent.
- **Desktop → API.** The desktop app POSTs to the Laravel API to push mobile notifications and re-activate devices — `/api/device/send-notification-vbnet` and `/api/device/reActivate` on `http://102.214.165.242:8080` (see `modeles_orcontrolls/pusher_Mo.vb` and `FORMS/FRMMobile/`). The API broadcasts over **Pusher**; the desktop app subscribes to Pusher channels as well.
- **Shared WhatsApp gateway.** Both call the self-hosted gateway at `wa.rhalla.online` for OTP and transfer notifications — [Watsaoserversfrom.php](backend/app/Services/Watsaoserversfrom.php) on one side, `Module1.vb` / `modeles_orcontrolls/SandWatsappMasggs.vb` on the other.

## backend/ — Laravel API

```bash
composer install && npm install
php artisan serve                      # or: composer dev  (serve + queue + pail + vite)
./vendor/bin/pint                      # the only linter
php artisan test                       # or: ./vendor/bin/phpunit
php artisan test --filter=ExampleTest  # a single test
```

`tests/` holds only the two stock Laravel example tests — there is no real suite, so changes are verified by exercising the endpoints.

### Running the API locally against local SQL Server

This machine is set up for it. `DESKTOP-M233HRE\SQLEXPRESS` already holds `EXCHANGESYS2026` (189 tables), `ExSyAccounts2026` and `ExSyAccountsCurrency2026`, including the tables Laravel needs (`users`, `personal_access_tokens`) and the hand-made `secure_api_tokens`.

```bash
cd backend
composer install --no-dev          # --no-dev is required: laravel/pint needs PHP 8.3, local PHP is 8.2.12
php artisan config:clear
php artisan serve --host=0.0.0.0 --port=8000
```

Four things had to be true, and now are:

1. **PHP needed the sqlsrv extensions.** XAMPP's PHP 8.2.12 is **ZTS x64**, so it takes the `_ts` x64 build — `php_sqlsrv_82_ts.dll` and `php_pdo_sqlsrv_82_ts.dll` from [msphpsql v5.11.1](https://github.com/microsoft/msphpsql/releases/tag/v5.11.1) (the last stable release shipping PHP 8.2 Windows binaries). They are in `C:\xampp\php\ext\` and enabled at the end of `C:\xampp\php\php.ini`; the original is kept at `php.ini.bak-before-sqlsrv`. ODBC Driver 17/18 were already installed.
2. **`DB_PORT` must be empty.** `SQLEXPRESS` is a *named instance* on a dynamic port, and 1433 is closed. Laravel's connector appends `,port` to `Server=` only when `port` is truthy, so `DB_HOST=localhost\SQLEXPRESS` with `DB_PORT=` produces the right DSN. Setting the port breaks it.
3. **Auth is Windows integrated.** `DB_USERNAME=` and `DB_PASSWORD=` empty makes the driver use the logged-in Windows account.
4. **`backend/.env` was repointed** at local, and the previous production values are kept in `backend/.env.production.bak` (git-ignored). Everything except the `DB_*` keys was left untouched, so Twilio, Pusher and the WhatsApp gateway still point at the live services — **`device/otp/send` sends a real WhatsApp message.**

For the Flutter app on an Android emulator, the host is `10.0.2.2`:

```bash
cd rhalla_agent && flutter run --dart-define=API_BASE=http://10.0.2.2:8000/api
```

Cleartext HTTP is allowed only in the debug build, and only for the emulator host — `android/app/src/debug/res/xml/network_security_config.xml`, referenced from the debug manifest. It cannot reach a release build.

> Note: the local `EXCHANGESYS2026` is also the read-only source the migration harness diffs against (see `migration/` rules). Running the API against it writes to that database — which the owner has accepted. If the migration comparison ever looks wrong, suspect API-written rows first.

### Never run `php artisan migrate` against the configured database

`.env` points at the **live production** exchange database, and `database/migrations/` is the stock Laravel skeleton only (users, cache, jobs, personal access tokens). It does **not** describe the live schema: `App\Models\User` marks `phone`, `device_id`, `UeserType`, `AccID`, `BrancchID` and `AccountType` as fillable, none of which appear in the users migration — those columns were added outside Laravel. `migrate:fresh` / `migrate:refresh` would drop live tables, and plain `migrate` is not safe either. Treat the database as the source of truth and the migrations directory as decoration.

### Structure

- All routes are in [routes/api.php](backend/routes/api.php), one flat list with Arabic comments. Everything inside the `auth:sanctum` group requires a Sanctum device token.
- [depositController.php](backend/app/Http/Controllers/Api/depositController.php) is the bulk of the API (~3.2k lines) — transfers, balances, favourites, driver/taxi dispatch, countries and cities.
- Responses go through `BaseController::sendResponse` / `sendError`, which wrap `{data, message, success, key}` where `key` is an `App\Enums\ResponseEnums` constant. Match that envelope for new endpoints.
- External services: Twilio (SMS + Verify), Pusher (broadcast), the WhatsApp gateway, and an n8n payments table read by `app/Services/SVSn8n_payments.PHP`.
- `.env` is git-ignored and holds live credentials — never print it or commit it.
- Deployed on shared hosting: the root `.htaccess` rewrites into `public/`.

## ExchangeSystem/ — desktop app

Build requirements, architecture and conventions are in [ExchangeSystem/CLAUDE.md](ExchangeSystem/CLAUDE.md) — but see the staleness note at the end of this file before trusting its database sections.

### Building and running it

There is no `dotnet` CLI path — it is a classic non-SDK .NET Framework 4.8 project. Two batch files at `ExchangeSystem/`, both locating MSBuild through `vswhere`:

- `run.bat` — incremental build of the **solution**, then launch. The everyday one.
- `deploy_new_build.bat` — kills a running instance, then `-t:Rebuild` on the **`.vbproj`**. Use it when resources may be stale: a compile-only build produces an exe with no embedded `.resources`, and the app then dies on startup with `MissingManifestResourceException` in `FRMMAIN.InitializeComponent()`. Only a real Build/Rebuild runs resgen over the ~445 form resources.

### Choosing a database at runtime

Runtime configuration is **not** in source. `RhallaConfig.ini` sits next to the exe, is git-ignored, and is read once at startup by `MDD/MD_SECRETS.vb`. Copy [RhallaConfig.ini.template](ExchangeSystem/RhallaConfig.ini.template) into `ExchangeSystem/ExchangeSystem/bin/Debug/` and fill it in. No rebuild is needed to change engine or server.

- `DB_ENGINE=SQLSERVER|MYSQL` drives `MD_MYSQL.USE_MYSQL` (a read-only property in `MDD/MD_CONNECTION_MYSQL.vb`, not a constant you edit). Anything unrecognised or missing means `SQLSERVER`, deliberately — a mistyped config must not silently land on a different engine.
- `MYSQL_TARGET=LOCAL|PROD` picks `MYSQL_CONN_LOCAL` or `MYSQL_CONN_PROD`. It **defaults to LOCAL and fails safe to LOCAL**; `USE_PRODUCTION_MYSQL()` throws rather than falling back when `MYSQL_CONN_PROD` is blank, because a silent fallback is how you write to the wrong database.
- `ConvertZeroDateTime=true` is required in the MySQL connection strings (migrated data contains `0000-00-00`); do not also add `AllowZeroDateTime`, the two conflict.
- Keep `AUTO_UPDATE=OFF` when running a local build — ON overwrites the exe with the vendor's production build and repoints the app at the production server.

The MySQL path was added without changing a single `CLS*` class or form: they still build `SqlParameter` arrays, and `MD_MYSQL` converts each to a `p_`-prefixed `MySqlParameter`, executes against MariaDB, and copies OUTPUT values back into the original objects. `Module1`'s helpers dispatch to `*_MY` twins behind `If MD_MYSQL.USE_MYSQL`.

`bin\Debug\mysql_errors.log` is the **application-wide** error log despite the name — it works on both engines, records the engine and target chosen at startup, and logs each failed call with its proc name and every parameter value. A global handler also captures exceptions a form's `Catch` would otherwise swallow. It is the first place to look when a screen is silently empty or the app reports the device as unlicensed (usually a missing transitive NuGet DLL, not licensing).

### The migration toolchain (`ExchangeSystem/migration/`)

The SQL Server → MariaDB port is driven by a C# converter, not by hand-editing SQL. `STATUS.md` there is a detailed and current log of what was ported, what broke and why — consult it before touching any routine. The guiding rule throughout is **literal translation, never redesign**.

- `migration/migrator/` — .NET 8 console app (`Program.cs`, ~3300 lines). Modes: `schema | data | verify | functions | procs | hardprocs | tvpprocs | views | harvest | crossdb | debugfn | dumpbodies`. It reads `appsettings.json` by a bare relative path, so **it must be run from `migration/migrator/`**; swap in `appsettings.main.json` / `appsettings.exsyaccounts.json` to change target schema.
- `migration/proof/` — 39 hand-ported MySQL routines the converter cannot produce.
- `migration/srcpatch/` — minimal patches to the *source T-SQL* (usually just an omitted `BEGIN`/`END`) so the normal converter can handle it. These must stay semantically identical to what SQL Server runs; behaviour changes belong in `proof/`.

```bash
cd ExchangeSystem/migration/migrator && dotnet run -- debugfn <RoutineName>   # T-SQL vs converted MySQL, side by side
bash ExchangeSystem/migration/apply_handports.sh
bash ExchangeSystem/migration/audit.sh
```

Reach for `debugfn` before hand-porting anything — most "failures" are one systematic converter gap, not fifty unique problems.

Rules that are easy to violate and expensive to discover:

1. **Never run `migrator hardverify`.** It executes write procs against SQL Server, and the T-SQL reaches the real 506k-row ledger by absolute name and through synonyms — it would mutate production data. `cmp_writes.py` (audit checks 10–11) exists to compare write paths statically instead.
2. **Pipeline order is `functions → procs → hardprocs → tvpprocs → views`.** Omitting `tvpprocs` silently leaves procs absent — they appear in no failure list.
3. **Re-run `apply_handports.sh` after *any* migrator routine run.** The migrator `DROP`s each routine before recreating it; for a hand-ported routine the CREATE then fails, so the DROP has deleted a working routine and left nothing behind. Its internal order (cross-DB → synonyms → functions → procs) matters, because later objects resolve against earlier ones.
4. **Re-run `audit.sh` afterwards, and every check must print `0`.** These catch the bugs where both engines accept the syntax and only the meaning differs — wrong numbers rather than an error. Two lessons are baked into it: an audit that matches nothing looks exactly like an audit that passes (so test every new regex against a known-bad *and* a known-good string), and a fix can introduce the very class it is fixing (so always re-audit after a converter change).
5. **Every production routine push must be followed by the per-object `EXECUTE` re-grant.** `DROP PROCEDURE` deletes the grant along with the object, and `exchange_app` holds per-object grants because `mysql.db` on that server is a crashed table. The generator query is in `STATUS.md`.
6. **A routine that will not reproduce from the converter is a hand-port.** Patch its live body; regenerating it from the converter regresses the hand-port.
7. `replay_internal_transfer.sh <Code>` / `replay_external_transfer.sh <Code>` replay a real historical transfer and diff the ledger rows against what SQL Server actually wrote. This is the only cross-engine test available for the money paths and it has caught defects no static check could — but both are **local-only; never point them at production**.

The source SQL Server (`DESKTOP-M233HRE\SQLEXPRESS`) is read-only and is never written to. The local MariaDB target is XAMPP's 10.4.32, driven through `/c/xampp/mysql/bin/mysql.exe`. Bodies are dumped via `migrator dumpbodies`, never `sqlcmd` — sqlcmd renders through the console code page and turns the Arabic column aliases these procs are full of into `?????`.

## Conventions

- **Everything user-facing is Arabic and RTL** — UI strings, API response messages, route comments and commit messages. Keep new strings in Arabic.
- Legacy naming with typos baked in (`Watsaoserversfrom`, `depositController`, `UeserID`, `BrancchID`, `Navction`) is load-bearing on both sides, because route and column names match the database. Match the surrounding style rather than correcting spelling.
- Credentials live in git-ignored files (`backend/.env`, `ExchangeSystem/…/RhallaConfig.ini`), never in source.

## Known-stale guidance in `ExchangeSystem/CLAUDE.md`

That file predates the MariaDB migration and the move to `RhallaConfig.ini`. Its architecture, permissions, folder-layout and naming sections are still accurate; these specific claims are not:

- It says both data-access paths talk to SQL Server. The app now runs on either engine, chosen by `DB_ENGINE`.
- It says `Module1.OPENCONNECTION()` hardcodes the connection string and that environments are switched by commenting lines in and out. `Module1.vb` now states the opposite explicitly — the server comes from `RhallaConfig.ini`.
- It lists `FORMSEXCHANGE\` and `MSGFORMS\` as top-level folders; both live under `FORMS\`. `Module3.vb` is under `MDD\`.
- It says there is no linter and no tests. True for the VB project, but `migration/` has a real verification harness (`audit.sh`, `cmp_writes.py`, `test_cmp_writes.py`, the replay scripts).

`migration/STATUS.md` also still says the engine is switched with `MD_MYSQL.USE_MYSQL = True/False`; that flag is now a read-only property driven by the ini.
