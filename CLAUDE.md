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
flutter build appbundle --release --dart-define=API_BASE=...   # what Play takes — never ship the fat APK
```

**`--dart-define=API_BASE` is not optional in practice.** `kApiBase` in `core/net/api_client.dart` defaults to the **live production** server, so a bare `flutter run` moves real money against real accounts. Always pass the target you mean.

#### Release-only failures the debug build cannot show you

Two defects lived here precisely because debug builds hide them. Both are fixed; the point is that **debug success proves nothing about release** on this app — verify against a built APK.

- **`INTERNET` was missing from the release manifest.** Flutter's template declares it in `src/debug/` and `src/profile/` only, and `src/main/AndroidManifest.xml` declared no permission at all — so `app-release.apk` shipped with `DUMP` as its *only* permission and could not open a socket. Every screen would have failed at the first real user. It is now in the main manifest. To check a build rather than trust it, read the permissions straight out of the APK:

  ```bash
  python -c "import zipfile,re;m=zipfile.ZipFile('build/app/outputs/flutter-apk/app-release.apk').read('AndroidManifest.xml');print(sorted(set(re.findall(r'android\.permission\.[A-Z_]+',m.decode('utf-16-le','ignore')+m.decode('latin-1','ignore')))))"
  ```

- **Release was signed with the debug keystore** (the stock `// TODO` left in place), which Google Play rejects and which would have made the app un-upgradable. `android/app/build.gradle.kts` now reads `android/key.properties` and **throws a `GradleException` when it is absent** rather than falling back — a silent fallback is how the debug-key APK got built in the first place. Copy `android/key.properties.template`, generate the keystore with the `keytool` line inside it, and keep the `.jks` and its password off this machine. **The key cannot be replaced after the first Play release.**

  Two things `build_apk.bat` learned the hard way while building the first signed release. **`storeFile` must be an absolute path** — Gradle resolves it from the `app` module, not from `android/` where `key.properties` sits, so a bare filename fails at `validateSigningRelease` after the whole build has already run; the template says so and is right. And **the debug-keystore guard was passing without reading anything**: `keytool -printcert -jarfile` reads only the v1 JAR signature, but at this `minSdk` Gradle signs with **APK Signature Scheme v2 alone** (measured: `v1 false, v2 true`), so `keytool` printed an empty file and the `findstr` for `Android Debug` matched nothing — which is indistinguishable from passing. The check now uses `apksigner verify --print-certs`, fails if it cannot verify *or* cannot find a certificate DN, and was tested against both a real release APK and a debug one (`CN=Android Debug`).

  **`org.gradle.jvmargs` from the Flutter template does not fit this machine.** `-Xmx8G -XX:MaxMetaspaceSize=4G` on an 8 GB box makes the JVM fail to reserve G1's virtual space mid-`assembleRelease`, and the message says only `Gradle build daemon disappeared unexpectedly` — never "memory". It is now `-Xmx3G -XX:MaxMetaspaceSize=768m`, which R8 and resource shrinking are comfortable in.

#### APK size — the fat APK is 62 MB and 95% of that is one number

Measured on a real release build (4 Sep 2026), uncompressed:

| Part | Size | Note |
|---|---|---|
| `lib/x86_64` + `lib/arm64-v8a` + `lib/armeabi-v7a` | **60.7 MB** | three copies of the engine; a device runs **one** |
| `classes.dex` | 0.87 MB | after R8 (was 1.33) |
| fonts + brand assets | 1.16 MB | |
| `res` + `resources.arsc` | 0.13 MB | after shrinking (was 0.43) |

So **never hand anyone `app-release.apk`**. `--split-per-abi` gives arm64 at **21.6 MB** and armeabi-v7a at 19.7 MB; the app bundle lets Play do the same split automatically and is what Play requires anyway. Everything else is rounding error next to that.

`isMinifyEnabled` + `isShrinkResources` are on, with `android/app/proguard-rules.pro`. R8 was verified against its own report rather than assumed: every plugin class survives (secure_storage 11, share 7, url_launcher 14, image_picker 34, printing 10) and the only removals inside `printing` are empty `<clinit>` bodies. If anything in printing, sharing or the image picker ever misbehaves in release **and not in debug**, R8 is the first suspect — turning both flags off is the one-line test.

**Do not subset the bundled fonts to save that 1.16 MB.** Beneficiary names, city names and status labels all arrive from the database, so any Arabic glyph can appear; a subset that fits today's data renders tofu on tomorrow's.

**Transport is still HTTP, and that is the remaining launch blocker.** `http://102.214.165.242:8080` is unreachable from a release build on either platform: Android blocks cleartext by default at this `targetSdk` (the `network_security_config` exception is debug-scoped), and iOS blocks it via ATS. The fix is a TLS certificate on the server — **not** `usesCleartextTraffic` and **not** an ATS exception, both of which would ship an exchange app that transmits balances and transfer codes in the clear.

Stack: **Riverpod + go_router + dio + flutter_secure_storage**, hand-written models, **no build_runner**. Codegen was deliberately skipped: several endpoints return raw SQL result sets whose columns are not knowable from the backend source, so tolerant hand-parsing beats generated strict models.

Layers: `core/theme` (tokens → Dart) · `core/net` (dio + the envelope decoder) · `core/storage` (secure store, device id) · `core/format` (money, phone, Western numerals) · `ui/widgets` (ambient background, glass, controls) · `features/*` · `router.dart`.

Three one-letter statics carry the whole visual and numeric system; reach for them before writing a literal:

- **`R`** (`core/theme/tokens.dart`) — colours, gradients, radii. `R.primary`, `R.inkA(.55)`, `R.rCard`.
- **`T`** (`core/theme/app_theme.dart`) — typography. `T.kufi(...)` / `T.plex(...)` and named roles (`T.title`, `T.amountHero`, `T.cta`).
- **`Fmt`** (`core/format/fmt.dart`) — `Fmt.money` (**2 decimals**, LYD), `Fmt.rate` (4), `Fmt.phone` / `Fmt.phoneForApi`, `Fmt.num_` for the raw-SQL values that arrive as strings, and `WesternDigits`, a `TextInputFormatter`.

  Two decisions in there that look like bugs and are not. **`Fmt.money` is `#,##0.00#` — two decimals, and a third only when it carries a value** (`1,000.00`, but `1,000.325`), by owner decision (30 Aug 2026). Both halves matter: the dinar is 1000 dirham, so a fixed `#,##0.00` would hide real money and open accounting differences, while a fixed `#,##0.000` would show zeros that carry no meaning. Do not "simplify" it to either. And **every number in this app is Western — `0123456789`, never `٠١٢٣٤٥٦٧٨٩`, anywhere, for any reason.** That is why `Fmt` pins locale `'en'` rather than `'ar_LY'`, and why `WesternDigits` must come **first** in every `inputFormatters` list: the `FilteringTextInputFormatter` after it allows `[0-9]` only, so it would delete an Arabic-Indic digit before it could be converted — and the agent would just see a keyboard that types nothing.

  **Every field the agent types into takes an `AutoClearFocus`** (`core/format/fmt.dart`), which owns two owner decisions. It **empties the field the moment the caret enters it** — an agent's first keystroke must never join a leftover value and produce an amount nobody intended — and it **restores the previous value if the caret leaves without anything being typed**, because without that restore a stray tap silently wipes a transfer the agent had already filled in. With `formatOnExit: true` it also rewrites the amount in full display form on blur (`2500` → `2,500.00`); that happens on blur and not per keystroke, since forcing two decimals while typing makes entering a fraction impossible. Amounts, commissions, phone numbers, beneficiary names, notes and the transfer-code search all use it — the rule is not numeric-specific, which is why the class is not named after numbers.

  **A comma typed into a money field is a decimal point, not a thousands separator** (`DecimalComma`, 5 Sep 2026). Android's numeric keypad puts the *device locale's* decimal symbol on that key, and the agent's device is `ar-LY` — so the key emits `,`. Typing "2,5" meaning two and a half was read as a grouping comma, stripped, and became **25**: a silent 10× error on money, which to the agent looked like the app deleting the separator. The fix is safe precisely because `ThousandsGrouping` inserts grouping commas itself, so the agent never has a reason to type one — every comma that arrives from the keyboard means the decimal split. Paste is the one exception and is handled by shape: a comma followed by exactly three digits with no dot in the string stays grouping, so copying a displayed `2,500` and pasting it back gives 2500. `test/decimal_comma_test.dart` types character by character through the real formatter chain rather than calling a function on a whole string — the defect lived in the sequence, and a test that passes "2,5" in one go sees nothing.

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

### Employees and points of sale (`features/employees/`, `features/employee_app/`)

An agent creates employees, grants each a set of permissions, issues an activation code, and watches what they do. Employees run the same APK in a separate mode with its own session, its own screens and its own guard.

**The single most important decision: points of sale were not re-invented.** `AuthorizedUsers` is the system's POS table (11 live rows, written by the desktop app through `AuthorizedUsers_Add/_update/getByBranch`), and the new tables reference `AuthorizedUsers.ID`. The list endpoint filters by `BrancchID` because `AuthorizedUserService::getByBranch` does — matching the system's own definition rather than inventing a second one.

Fifteen tables (`backend/database/sql/employees/`), and these are the decisions inside them:

1. **`device_registry` is separate from `employee_devices`, and nothing is ever deleted from it.** The first is a permanent classification that implements the ban on admin login from an employee device; the second is an operational binding that gets revoked and re-created. Merging them would make "revoke device" silently lift a security ban. The device id is hashed (SHA-256) before storage.
2. **The employee is not a row in `users`,** and activation issues no Sanctum token. `employee_sessions` is a separate table behind a separate middleware — the surest way to keep an employee session from ever becoming an admin one.
3. **Attribution never touches `InternalEx`.** `transfer_attributions` records who created or delivered a transfer, from which POS and device, beside the core ledger rather than inside it.
   **Two employees delivering the same transfer at the same instant cannot both win.** What guarantees it is the `where('status', PENDING)` inside `markDelivered`'s own `UPDATE`, not the `first()` above it: concurrent requests serialize on the row lock and the loser re-evaluates the predicate against the committed value, so it affects zero rows. Measured with three simultaneous processes on one row — one `changed => true`, one `transfer_status_history` row. Everything with a financial or reporting effect (the attribution, the cashbox `TRANSFER_DELIVERY` entry, the audit record) sits inside `if ($result['changed'])` in `EmployeeController::deliver`, so the losers write nothing. The loser's response re-reads the row rather than returning the stale pre-update copy — otherwise it says "already delivered" while carrying `PENDING_DELIVERY`, and the delivery button stays live in that employee's app for a transfer that was already paid.
4. **Permissions are granted rows, never columns.** No row means denied. A feature added tomorrow appears in the server-side catalog (`EmployeePermissions::CATALOG`) already denied to everyone, with no migration and no app release — which is why no permission name is written in any Dart file.
5. **A code used on a second device is burned, not just refused.** Status becomes `COMPROMISED`, sessions and devices drop, and the code no longer works even on the original device. Painful on purpose: the alternative leaves a leaked code valid.
6. **Failure messages are uniform** ("رقم الهاتف أو كود التفعيل غير صحيح") so they cannot be used to discover which numbers are registered. The one exception is `COMPROMISED`, because the real employee must know why their code stopped.

**The employee cashbox is an operational ledger, authorised separately by the owner (3 Sep 2026) on condition it does not conflict with the agent↔Rhalla financial flow.** `Expected = opening + in − out`. Two decisions: the balance is **computed from entries, never stored** (a stored balance drifts from its entries at the first interruption and then nobody knows which is right), and **entries are never deleted or edited** — corrections are reversal rows that keep the original. Shift closing compares expected against counted with a `0.0005` threshold, because exact float equality reports a phantom "shortage" of 0.0000001 on every close. `tests/manual/employee_cashbox_acceptance.php` and `employee_activation_acceptance.php` and `employee_permissions_acceptance.php` (the last over real HTTP) pin all of it — 62 checks, each suite ending in a financial-invariant snapshot of `wallet`, `ExchangeAccData`, `InternalEx`, `EX24AccSafeActivityTb` and `AccountsTb`.

**`CREATE_TRANSFER` is in the catalog but wired to nothing.** Creating a transfer writes to the core financial system, and the owner's standing order requires a separate explicit authorisation for that. Do not connect it without one.

### `InternalEx.Code` has no index — never probe it per row

Verified on the live schema: `InternalEx` carries `PK(ID)`, `UQ(IDCode)` and two indexes on the cancel flags. **Nothing on `Code`**, which is the column every join from the app's side uses. So any correlated subquery keyed on `Code` rescans the whole table once per row — the same shape that made `LocalStatmentAccount` take 68 seconds.

`AgentIncomingTransfersService::list()` was doing exactly that: five correlated subqueries per row (three against `InternalEx`, two against `TransCancelRequestTb`) for cancel reason, cancel notes and the sender's note. A 20-row page meant 60 scans of `InternalEx`. It is now `attachCoreText()` — one query per table for the whole page, keyed on the page's codes, joined in PHP. Measured on the emptied database: `list()` **202 ms → 7 ms**, the `index` endpoint 321 ms → 118 ms; the gap widens with row count, which is the point. Equivalence was checked against the old SQL on every code present, and the new form is also *more* deterministic — the old `TOP 1` without `ORDER BY` picked an arbitrary row when a code repeated, this one takes the newest.

Adding an index on `Code` would fix the root and is **forbidden**: `InternalEx` is a financial table under the standing order.

### A row with no original in the core disappears from the app — but is not deleted

Owner's order (5 Sep 2026): the app shows exactly what the database holds, nothing here that is not there and nothing there that is not here. `agent_incoming_transfers` is added to and never deleted from — correct for status changes, since a transfer cancelled after arriving must stay in the ledger *labelled* cancelled rather than vanish — but a row whose `InternalEx` original is wiped (a manual database cleanup) stayed on display with nothing behind it. Measured: after one cleanup, 5 of 6 ledger rows pointed at transfers that no longer existed.

`refreshCoreState` already fetches the core rows for exactly this agent's ledger codes, so **the reconciliation costs no extra query** — the codes that do not come back are the ones that are gone. `core_missing_at` is stamped on them, and `list()`, `counts()`, `alerts()` and the employee report's pending count all filter `core_missing_at IS NULL`.

**Never ask the question per row.** There is no index on `InternalEx.Code` (verified: only `PK(ID)`, `UQ(IDCode)`, and two on the cancel flags), so a correlated `EXISTS` per row is the exact shape that made the statement endpoint take 68 seconds. The set comparison happens in PHP over rows already fetched, and the fetch itself is chunked at 1000 because `IN` stops at 2100 arguments.

**A flag, not a `DELETE`,** for three reasons: `transfer_status_history` has a foreign key to this table, so deleting erases who delivered and when; the absence may be temporary and the flag lifts itself when the original returns (verified in both directions), while a delete does not; and a flagged row carries the timestamp it vanished, which a deleted row cannot say. `markDelivered` refuses a flagged row too — the screen may have been open since before the original disappeared, and recording a delivery would write an attribution and a cashbox entry for a transfer that does not exist.

### The incoming-transfer bell (`features/alerts/`)

The bell in the home header carries a count of incoming transfers the agent has **not opened the invoice of**, and rings when one arrives while the app is open. `GET agent/incoming-transfers/alerts` returns nothing but the row ids (newest 200) — separate from `index` because the poll asks one question every 30 s and `index`'s payload is full rows with per-row cancel-reason subqueries. Measured at **0.20 s** including its `syncFromCore`.

Four decisions in it:

1. **What counts as "seen" is stored on the device, not the server.** "Have I read this?" belongs to whoever holds the phone, not to the account; putting it on the server means a new table and a write on every invoice open, bought nothing, and the bell decides nothing financial. `SecureStore.readSeenIncoming` keeps the newest 400 ids and is **not cleared on sign-out** — the returning agent is the same agent, and forgetting would ring for their whole history.
2. **The first poll of a session never rings.** It establishes the baseline; a backlog that existed before the app opened is announced by the counter, silently. Ringing for it would train the agent to ignore the bell.
3. **Opening the invoice is what clears a transfer from the count, not opening the list.** A count that drops because a screen with twenty rows was displayed is a false promise.
4. **The sound is the device's own notification tone** (`RingtoneManager` through the existing `com.rhalla.rhalla_agent/device` channel, `SystemSound.play` as the iOS fallback, haptics on both). No bundled audio file and no audio package: a stranger's tone in a bigger APK, against the one sound the user is already trained to look up at. It respects silent mode, which is correct.

The poll lives in `AutoRefresh` (the shell), not in the home screen — a bell that only rings while you are looking at it is not a bell — and stops when the app is backgrounded. Alerting a closed app is push-notification work, which this is not.

### Commission is displayed with its transfer, not as its own row

In «آخر العمليات» a commission no longer appears as a separate card; it appears as a line inside its transfer's card, and the transfer details show value / commission / operation total.

**The relationship already existed and was not created:** a commission row in `EX24AccSafeActivityTb` carries the same `ISID` as its parent transfer (verified: `13151-54-1` and its 10.00 commission share the number). `LocalStatmentAccount` returns `IsCommission`, and **sums the commission per `ISID` in PHP over the rows it has already fetched** — the statement returns every movement of the agent, commission rows included, so nothing further needs to be asked of the server. **The accounting is untouched:** the commission is still its own independent entry, still in the response, and still shown in full in كشف الحساب — a statement that hides a debit is not a statement. Only the home screen groups. The app distinguishes "no commission" from "not loaded" by **key presence**, not by value, so the field is always a number.

**Do not move that grouping back into SQL.** Both SQL shapes were measured against the same six rows and both made the endpoint unusable:

| Shape | Result |
|---|---|
| Aggregated derived table (`leftJoinSub` on `EX24AccSafeActivityTb` + `LIKE N'%عمولة%'`) | The optimizer estimates an aggregate over the whole half-million-row table and asks for a huge memory grant; the query then sits on **`RESOURCE_SEMAPHORE` for 70+ seconds** and the request dies with `Maximum execution time of 60 seconds exceeded` |
| Correlated scalar subquery | Correct, and **68 seconds** — there is no index on `(accidfrom, ISID)`, so every row rescans the table |

The symptom is worth recognising because it does not look like a slow query: the home screen shows loading skeletons forever, the app retries, and `php artisan serve`'s workers fill up until *other* endpoints hang too. `SELECT session_id, blocking_session_id, wait_type, wait_time FROM sys.dm_exec_requests` names it immediately — `blocking_session_id = 0` with `wait_type = RESOURCE_SEMAPHORE` is a memory grant, not a lock. Adding the index would fix it and is **forbidden**: `EX24AccSafeActivityTb` is a financial table and the standing order covers it.

### White-label company identity (`features/branding/`)

Each company gets its own name, logo and colour theme **inside the app after login**; the pre-login screens stay Rhalla's official identity. It is a **presentation layer only** — by owner decision it must not touch balances, transfers, permissions, reports or any financial path. `tests/manual/company_branding_acceptance.php` pins that (13 checks, the last one a financial-invariant snapshot of `wallet`, `ExchangeAccData`, `InternalEx` and `users` taken before and after every write).

Backend: `tenant_branding` + `tenant_branding_audit` (`backend/database/sql/tenant_branding.sql`), `BrandingThemes` (the theme catalog — server-side so a new theme does not need a store release), `CompanyBrandingService`, `CompanyBrandingController`, and `GET/PUT company/branding`, `POST company/branding/logo`, `POST company/branding/reset`, `GET company/branding/logo/{name}`.

Six things in there are decisions, not incidentals:

1. **The tenant key is `users.AccID`.** There is no company or tenant column on `users`; the account in the chart of accounts is what carries the company (AccID 530 = «جاري شركة الامانة»). It is **never read from the request body** — verified: a POS account POSTing `company_account_id: 530` got 403, and a different Main account sending the same forgery wrote to its *own* row while 530 stayed untouched.
2. **Only `AccountType = 'Main'` may edit**; POS accounts read. No new permissions table — that distinction already exists in the system, and a second place to express it is a second place for the two to disagree.
3. **Status colours are fixed in every theme** (green success, red error, amber warning). A company whose brand colour is red would otherwise get a red success screen, and an agent reading success as failure in a transfers app is not an acceptable trade.
4. **`R`'s brand tokens are now assignable statics, not `const`,** with `R.applyBrand()` / `R.resetBrand()`. Every screen already reads `R.primary` directly, so a provider would have meant editing dozens of files and one forgotten file means another company's colours inside this one. **`resetBrand()` on sign-out is mandatory** — without it the login screen shows the previous company's colours. `main.dart` keys the widget tree on `BrandingController.epoch` because changing a static notifies Flutter of nothing.
5. **The logo lives on the private `local` disk and is served by `GET company/branding/logo/{name}`,** not the `public` disk. `Storage::url()` builds from `APP_URL` (`http://localhost` here) — a URL no phone can open — and needs `storage:link`. The route is outside `auth:sanctum` because `Image.network` sends no auth header; the filename carries 24 random characters, and the route's `[A-Za-z0-9_.-]+` constraint plus a `basename` check in the service stop traversal (verified: `..%2f..%2f.env` → 404).
6. **`branding_version` is a counter, not a timestamp** — server clocks drift, counters do not go backwards.

**«حوالة داخلية» is displayed as «حوالة محلية» everywhere** (owner decision, 3 Sep 2026). The fixed UI strings were renamed, but the movement titles on the home and statement screens come from `OperationTypeTb.OperationType` in the **database shared with the desktop app** — renaming them there would change what branch staff see. So they are translated at display time by `Fmt.localName`, which replaces only the feminine «داخلية»/«داخليه»: the table also contains «ارسال فاتورة نقل داخلي مع تاكسي», a taxi delivery and not a transfer, and «نقل محلي» there would be a corruption rather than a correction. `test/local_name_test.dart` pins that case.

**The invoice header carries the company's identity, not Rhalla's** (owner decision, 3 Sep 2026) — `ReceiptHeader` reads the branding for the Arabic name, the English name and the logo, so every receipt the customer is handed is the agent's own. The account-screen footer follows the same rule. Three places deliberately stay Rhalla: the pre-login screens (the owner's original decision), the terms screen (a legal document Rhalla publishes — attributing it to a tenant would misstate who wrote it), and the app name «رحلة» itself. Because the receipt is *captured as an image* for printing and sharing, `ReceiptTools.runReceiptAction` precaches the logo before the capture — without it the very first printed invoice comes out with the fallback mark while the network image is still decoding.

The English company name **writes itself** from the Arabic one as the manager types (`arabic_to_latin.dart`, 8 tests). It is done locally, not through Google Translate, for a reason that is not just offline-tolerance: a company name is **transliterated, not translated** — «الأمانة» is "Al Amana", not "The Trust", and a general translator gets that wrong on a name that ends up on invoices. So the descriptive words are translated from a small table and the proper noun is transliterated: «شركة الأمانة للحوالات المالية» → `Al Amana Company for Financial Transfers`. The first manual edit to the English field stops the generation permanently — an officially registered name must not be overwritten because someone fixed a letter in the Arabic.

Two Flutter traps cost real time here and are worth knowing before touching this code:

- **Riverpod forbids a provider from modifying another provider during its own initialization.** `brandingBootstrapProvider` called `load()`, whose first act is setting `loading = true` on the branding notifier — an assertion fires, and because it fires inside an unobserved future it is **swallowed silently**, so the symptom is simply that the request is never sent. The `await Future.delayed(Duration.zero)` in that provider is the fix and is not cosmetic. The same trap killed the first design, which triggered the load from a `ref.listen(..., fireImmediately: true)` inside the controller provider.
- **`go_router` keeps `StatefulShellRoute` branches alive by `GlobalKey`, so re-keying the widget tree cannot force them to rebuild.** Re-theming after the shell was already built left the home screen's action buttons green inside an otherwise blue app. Hence `BrandingState.settled` and the router gate: while signed in and not settled, every route redirects to `/splash`, so no post-login screen is ever built with the wrong palette. The wait is capped at 3 s (`BrandingController._gate`) — a colour must never be the reason the app will not open.

**«آخر العمليات» on the home screen labels a transfer from the agent's own delivery ledger, not from the core state** (owner decision, 2 Sep 2026). The two answer different questions: `InternalEx.ConfirmType` says where the transfer stands *between the agent and Rhalla* («مسلمه» = it reached the agent), while `agent_incoming_transfers.status` says whether the agent *paid the beneficiary*. Showing the first where the second belongs makes an agent read that they delivered money they never delivered. So `LocalStatmentAccount` now also returns `AgentStatus` and `AgentCoreType` from `agent_incoming_transfers` (two subqueries, never a JOIN — a JOIN on a repeated code would duplicate a financial row), and syncs the ledger first so a just-approved transfer is labelled without opening the transfers tab. The badge follows the same precedence as that screen's tabs: **cancelled wins over delivered** (owner decision, 3 Sep 2026, replacing the opposite rule of 2 Sep). The ledger is a display-level organiser, not an accounting entry, so the newest core state is what shows — a transfer Rhalla cancelled reads «ملغاة» even if the agent had already delivered it, and it moves out of the «تم التسليم» tab into «الملغاة» so no row is counted twice. The owner explicitly ruled out any second marker saying it had been delivered. `status` is never rewritten — it stays `DELIVERED` in the table and in the response; it is simply no longer what the screen shows. `DeliveryStatus` stays in the response and in `Movement` — it is not deleted, just no longer the badge. A movement outside the ledger (an outgoing transfer, a commission) keeps its direction label.

Because that sync runs on **every** statement load, it must stay cheap — and it was not. `AgentIncomingTransfersService::syncFromCore` joined `InternalEx_SelectType_View_not_BRanchId` to `InternalEx` to keep only approved transfers, and **that one JOIN cost 16.9 seconds** where the view filtered by branch alone answers in 0.05: SQL Server materializes the view for every branch before applying the join instead of pushing the branch filter inside it. The filter is now a second small `whereIn` query (chunked at 1000 — `IN` stops at 2100 arguments and one live branch already carries 522 transfers), which is the same predicate in a different place; equivalence was checked against the old JOIN on every branch present. `tests/manual/agent_incoming_acceptance.php` **restores the transfer it delivers** — it writes to the owner's real ledger, and a test that leaves a delivery behind is worse than one that does not run.

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
