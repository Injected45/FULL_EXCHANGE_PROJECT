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
- **[docs/audit-committee.md](docs/audit-committee.md)** — «لجنة فحص تطبيق الصرافة», the owner's standing audit charter (10 Sep 2026). **When he writes that phrase and nothing else, it is an order to run the whole thing** — twelve scopes (Apple · Play · security · privacy · accounting *audit only* · code review · performance · stability tests · supply chain · secrets · threat model · release gate) against the newest code and the newest store/OWASP/NIST rules, ending in one report and one verdict. Inspect · Test · Harden · Optimize · Fix · Retest — never Rewrite. A financial defect is recorded as CRITICAL and **never fixed without his explicit approval**.

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
flutter test                             # 117 tests across test/ — all of them must stay green
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

**Transport is still HTTP, and that is the remaining launch blocker.** Cleartext is now **off by default** and a store bundle cannot be built with it on — see «Store readiness» below. A test APK for a real phone is built with `-PallowCleartext=true`; everything else needs a TLS certificate on the server. `http://102.214.165.242:8080` is unreachable from a release build on either platform: Android blocks cleartext by default at this `targetSdk` (the `network_security_config` exception is debug-scoped), and iOS blocks it via ATS. The fix is a TLS certificate on the server — **not** `usesCleartextTraffic` and **not** an ATS exception, both of which would ship an exchange app that transmits balances and transfer codes in the clear.

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
4. **Permissions are granted rows, never columns.** No row means denied. A feature added tomorrow appears in the server-side catalog (`EmployeePermissions::CATALOG`) already denied to everyone, with no migration and no app release — which is why no permission name is written in any Dart file. **Two of them can never be granted by a bulk tap** — see below.
5. **A code used on a second device is burned, not just refused.** Status becomes `COMPROMISED`, sessions and devices drop, and the code no longer works even on the original device. Painful on purpose: the alternative leaves a leaked code valid.
6. **Failure messages are uniform** ("رقم الهاتف أو كود التفعيل غير صحيح") so they cannot be used to discover which numbers are registered. The one exception is `COMPROMISED`, because the real employee must know why their code stopped.
7. **An unused code dies after ten minutes** (`CODE_TTL_MINUTES`, owner's order 8 Sep 2026: «يمنع ترك صلاحية المفتاح مفتوحة»). Three details in that: it is **burned in the database**, not merely refused — a code that is rejected today but stays `ACTIVE` is a live key waiting for the rejection to be "fixed"; it applies to `ACTIVE` only, since a `USED` code has already bound a device and re-activating that same device is a legitimate path; and the message says plainly that it expired, breaking the uniform-message rule on purpose, because whoever reached that point already proved they hold a valid code for that number, and telling them "wrong number or code" sends them to retry nine more times. The agent's sheet says the ten minutes out loud for the same reason — a rule nobody was told about reads as a broken app.

**The employee cashbox is an operational ledger, authorised separately by the owner (3 Sep 2026) on condition it does not conflict with the agent↔Rhalla financial flow.** `Expected = opening + in − out`. Two decisions: the balance is **computed from entries, never stored** (a stored balance drifts from its entries at the first interruption and then nobody knows which is right), and **entries are never deleted or edited** — corrections are reversal rows that keep the original. Shift closing compares expected against counted with a `0.0005` threshold, because exact float equality reports a phantom "shortage" of 0.0000001 on every close. `tests/manual/employee_cashbox_acceptance.php` and `employee_activation_acceptance.php` and `employee_permissions_acceptance.php` (the last over real HTTP) pin all of it — 62 checks, each suite ending in a financial-invariant snapshot of `wallet`, `ExchangeAccData`, `InternalEx`, `EX24AccSafeActivityTb` and `AccountsTb`.

**What the employee owes is shown under his name on his own home screen** (`GET device/employee/custody`, behind `VIEW_OWN_CASHBOX`). It is the same `Expected` — no second calculation exists, and a second one would eventually disagree with the first.

Three things about that line are the point of it:

- **The cash in his drawer is not his; it is the agent's custody with him.** So it reads «في عهدتك», never «رصيدك» — the number is an obligation on him, not a balance of his.
- **The sign flips the sentence, not just the number.** Positive means cash he holds and owes; negative means he paid out more than he took in and the agent owes *him* — real whenever an employee starts a shift with no float and delivers transfers from his own money. The value is rendered absolute and the text carries the direction, because a bare minus sign reads as a shortage in his custody when he is in fact the creditor.
- **With no open shift it shows nothing at all — not zero.** Zero reads as "nothing owed", which is a different statement from "hasn't started", and the difference between them is a full day's work.

### The employee's cashbox statement, and delivery as a shared entity (9 Sep 2026)

`GET device/employee/cashbox/ledger` (behind `VIEW_OWN_CASHBOX`, read-only, scoped to the session's employee — **no parameter accepts an employee id**, so one employee's statement is unreachable from another's token). `EmployeeCashboxLedger` computes it; nothing is stored. 59 checks in `tests/manual/employee_cashbox_ledger_acceptance.php`, ending in the usual financial-invariant snapshot.

**The running balance restarts at every shift, and that is the accounting, not a limitation.** `opening_cash` is a column on the shift, not an entry — `expectedCash` reads it and adds the shift's entries. So summing every shift's opening plus every entry produces a number that means nothing. The deeper reason is that **a shift is a settlement cycle**: it closes by counting the cash and handing it over, and the next opens with a float the employee declares — not with what the last one ended at. A balance continuing across that boundary asserts a continuity that does not exist. The statement is therefore sections, one per shift: a synthetic opening row, the movements, and for a closed shift a closing row carrying expected / counted / difference **read from `employee_shift_closings`, not recomputed** (an entry added after the close would otherwise produce a different difference today than the one recorded that night).

Four more things in it:

- **The opening and closing rows are synthetic and never written.** Writing the opening as an entry would count it twice — once as an entry, once from the shift column.
- **The opening is not summed into `in`.** The equation is `opening + in − out`; folding it into `in` double-counts it in the displayed total.
- **A reversed entry is shown, struck through, and not counted.** `expectedCash` excludes both the reversed original and the reversal, so the statement must exclude the same pair or its balance would disagree with the shift close. It stays visible because correction is by reversal, not deletion.
- **An entry with `shift_id = NULL` gets its own section rather than being dropped.** Dropping it would make the statement's total disagree with the table's — the one thing a stocktake must never do.

**Type names come from the server (`EmployeeCashboxLedger::TYPES`), not the app.** A type added tomorrow shows its Arabic name with no app release. The app had no case for `TRANSFER_CREATED` — the most common IN movement — and rendered it as «حركة».

**A silent accounting hole was found and closed here.** `UX_entry_reference` is unique on `(reference_type, reference_id)`, and both the create path and the delivery path wrote `INTERNAL_TRANSFER`. A transfer an employee **created** and later **delivered** — ordinary when the destination point of sale belongs to the same agent — collided on the second entry, and `addEntry` returns `['duplicate' => true]` **without inserting and without throwing**. The OUT entry vanished silently, the employee's custody overstated by the transfer amount, and the shift close reported a phantom **surplus**. Creation now writes `INTERNAL_TRANSFER_CREATED`; the index guards one entry per *action*, which is what it was for. Rows written before the fix are left exactly as they are — a movement is never edited or deleted.

**Delivery was already atomic and stays so.** The guarantee is the `where('status', PENDING)` inside `markDelivered`'s own `UPDATE`, and it was verified again the only way that means anything: **four genuinely parallel HTTP requests from two employees against one transfer** (`curl_multi`, not a loop — a sequential loop proves idempotency and nothing about concurrency). Exactly one `changed`, one attribution, one cashbox entry, one history row. What was added:

- **`transfer_status_history.changed_by_employee_id`.** `changed_by` carries the *agent* id on both paths, because the employee acts as a face of the agent — so the transition row itself could not tell an agent's delivery from an employee's. The identity was recoverable from `transfer_attributions` and `audit_logs`, but whoever reads a transfer's history reads this table, not three joined on timestamp.
- **The audit row now carries old and new status**, so one row answers the whole compliance question: who moved what, from which state to which.
- **«تم تسليم هذه الحوالة مسبقاً» is said only to whoever was actually beaten.** A repeat after a dropped connection is the *same* request retried; telling that employee the delivery failed when it succeeded is how someone pays twice. The two are distinguished by reading `transfer_attributions` — same employee ⇒ silent success, someone else ⇒ the message. Both return 200 with the fresh row, because the requested state *is* achieved and an error status leaves stale lists on screen.

**«بانتظار التسليم» is behind `DELIVER_TRANSFER`, not `VIEW_INCOMING_TRANSFERS`** (owner, 9 Sep 2026). It is a work queue, not a view: showing it to someone who cannot act on it makes them tell a beneficiary "your transfer is here" and then fail. «تم التسليم» and «الملغاة» stay with the view permission. Enforced in the controller — including the no-parameter default, or dropping the parameter would walk straight past the guard.

**The employee's incoming list polls every 30 s** (the bell's own cadence), stops when backgrounded, refreshes on resume, and shows no spinner on a pulse. Without it a transfer a colleague delivered stays on screen until someone pulls to refresh — and the server would refuse the second delivery, correctly, but the employee is standing in front of a customer and may have paid from the drawer already.

### "Opens with nothing, and every grant shows" — verified, and half of it was broken

Owner's question (9 Sep 2026). The first half held; the second did not.

**Nothing is granted at creation, and that is now proven through the agent's own endpoint** rather than by inserting a row: `POST employees` creates with `status = PENDING_ACTIVATION` and zero permission rows, `employee/me` returns `permissions: []` (an empty array, not a missing key), and each of the twelve route-guarded keys returns 403 before its grant and opens after it. `tests/manual/employee_default_deny_check.php`, 12 checks.

Two things that suite got wrong first, and both would have made it pass while proving nothing: a forged session on a `PENDING_ACTIVATION` employee returns 401 on everything, and the "did the door open?" rule was `status != 403` — which counted every one of those 401s as an opened door. The rule is now "not 401 and not 403", because 422 *is* proof the gate was passed (`SEARCH_TRANSFER` rejects a dummy transfer number after the middleware, correctly).

**The second half was broken: `nothingGranted` was computed from five hand-listed permissions while the screen renders thirteen tiles.** So:

- An employee granted only `SEARCH_TRANSFER` — or favourites, chat, reports, POS transfers, own transfers, or the balances pair — saw «لم تُمنح صلاحيات بعد» **above a working tile**. The agent, being told the employee sees nothing, would conclude the grant had failed.
- An employee granted only `DELIVER_TRANSFER` saw a blank screen with no banner at all, because the incoming tile requires `VIEW_INCOMING_TRANSFERS`. Same for `CASHBOX_ENTRY` without `VIEW_OWN_CASHBOX`.

The tiles are now built into a `List<Widget>` and the banner is `tiles.isEmpty && !canStartShift && !hasShift` — derived from what the screen actually offers, so a tile added tomorrow counts itself instead of being forgotten in a second list. The shift card counts as work, so `START_SHIFT` alone is not "no permissions".

The two permission pairs that produced a blank screen now each get a tile that says what is missing («تحتاج صلاحية … — راجع وكيلك») instead of rendering nothing. And gaps moved out of the tiles: the create tile used to carry a trailing gap conditional on the *next* tile's permission, which left a floating gap whenever that next one was denied.

`test/employee_home_permissions_test.dart` (9 tests) walks every catalog key one at a time and asserts the banner and a tile are never on screen together — it was run against the old computation first and failed on `DELIVER_TRANSFER`, which is the only reason to trust it. Seven report/favourite/close-shift keys are deliberately listed as reachable only from inside another tile; for those the banner is the truth.
### Store readiness — what was fixed, and the one thing code cannot fix (10 Sep 2026)

Owner's instruction: the app goes to Google Play and the App Store; close anything either would reject, cleartext first.

#### ⚠ The root problem is not in this repository

`http://102.214.165.242:8080` is a **bare IP with no TLS**. No amount of app-side work fixes that, and a public CA will not issue a certificate for a bare IP. The path is: point a name at the server (the domain already exists — `wa.rhalla.online` runs on it), get a certificate for that name, then build with `--dart-define=API_BASE=https://…`. Everything below makes the app correct *for* that moment and safe until it arrives.

#### Cleartext is now off by default and cannot reach a store

`AndroidManifest.xml` no longer names a network config directly — it carries `@xml/${netSecConfig}`, filled by Gradle:

- **default** → `network_security_strict.xml`: `cleartextTrafficPermitted="false"`, no exceptions at all.
- **`-PallowCleartext=true`** → `network_security_config.xml`: cleartext for the single production IP, everything else still TLS.

**Why the default is the safe one:** an exception that is on by default gets forgotten and shipped. Both mistakes have already happened here — release APKs were built that could not reach the server at all, and then a permanent exception was added to fix them. With a safe default the mistake is *visible* (the app cannot connect) instead of *silent* (it ships unencrypted).

And **`gradle.taskGraph.whenReady` throws if `bundleRelease` runs with the flag on** — Play's artefact is the AAB, so the store path is closed by construction rather than by remembering. ⚠ The first version matched any task containing "bundle" and blocked APK builds too, because `assembleRelease` runs `bundleReleaseResources` on its way; it now matches `^bundle(Debug|Profile|Release)$` exactly. A guard that blocks what it was not aimed at gets disabled within days.

`android:usesCleartextTraffic="true"` was never used: it opens every host, so a mistyped address or a third-party redirect would travel unencrypted too.

#### ⚠ Backup was uploading session tokens to Google Drive

`android:allowBackup` defaults to **true**. Everything this app stores is sensitive — the agent's session token, the employee's session token, the bound device id, the employee's cached permissions — and auto-backup put all of it in the user's Drive, restorable **onto a different phone**. That contradicts the device binding both account types rest on.

Now `allowBackup="false"`, plus `data_extraction_rules.xml` (Android 12+) and `backup_rules.xml` (below) excluding every domain — kept as a second line for whoever re-enables the flag one day.

**This also explains a real crash.** The secure store is encrypted with an Android Keystore key, and **the key is never backed up**. A restore put an encrypted preferences file on a device without its key, `read` threw, and the app froze on the splash screen (9 Sep 2026). The code guards now handle the symptom; this removes the cause.

#### iOS would have crashed on first use and been rejected

`Info.plist` had `NSCameraUsageDescription` and `NSFaceIDUsageDescription` — but the app also records voice notes (`voice_note.dart` → `record`) and picks images (`image_picker`, used in chat and the branding screen). **iOS terminates the app the instant a permission is used without its usage string**, and App Review rejects the build. Added `NSMicrophoneUsageDescription` and `NSPhotoLibraryUsageDescription`, both saying *why* rather than *what* — Apple rejects one-word strings.

Also added `ITSAppUsesNonExemptEncryption = false`: Apple asks at every upload, and the answer is genuinely "no" — the only encryption in use is the system's own TLS, which is exempt. `NSAppTransportSecurity` is still **absent on purpose**; an ATS exception for an app carrying balances is rarely accepted.

#### Measured, not assumed

- **16 KB page alignment** (a Play requirement for apps targeting Android 15+): every packaged `.so` read with `llvm-readelf` — Flutter's own at `0x10000`, the plugin libraries (MLKit barcode, dart JNI, camera utils) at `0x4000`. All pass.
- **The AAB builds and is signed** — 74.6 MB before Play's per-device split.
- The shipped APK's manifest and its network config were read back out of the APK with `aapt2 dump xmltree`, not trusted from source.
- Permissions requested: `INTERNET`, `CAMERA`, `RECORD_AUDIO`, `USE_BIOMETRIC` (+ `USE_FINGERPRINT`, injected by `local_auth` and required for API 24–27 — removing it would break biometrics on Android 7 and 8). Nothing that triggers a Play declaration form.

#### `test/store_compliance_test.dart` (10 checks) keeps it true

It reads the manifest, `Info.plist` and `build.gradle.kts` as text on every `flutter test`. ⚠ **These conditions break silently** — a new plugin adds a permission with no usage string, someone re-enables backup, a flag is left on — and none of it shows in `flutter analyze` or in a run. It shows up in a rejection email weeks later.

It also asserts the absence of what must not appear: `usesCleartextTraffic="true"`, `NSAppTransportSecurity`, and nine permissions that open a special Play review (`READ_SMS`, `QUERY_ALL_PACKAGES`, `MANAGE_EXTERNAL_STORAGE`, …). Verified by breaking `allowBackup` deliberately: the suite failed on that check and passed again when restored.

#### What is still required before submission, and is not code

1. **A TLS certificate** — a hostname for the API, then `API_BASE=https://…`. Nothing else on this list matters until this is done.
2. **A published privacy policy URL**, plus Play's Data Safety form and Apple's privacy nutrition labels — both must match what the app actually collects (phone number, device id, transfer data).
3. **iOS needs a Mac** (or a cloud runner) to build; `ios/Podfile` is still generated on first Mac build.

### ⚠⚠ The employee's screens closed themselves every 12 seconds (10 Sep 2026)

The owner: *«في تطبيق الموظف الصفحات غير مستقرّة… أفتح إنشاء حوالة وأبدأ أكتب
البيانات تُقفل ولا تدعني أُكمل»*. A transfer form closing under the employee's
hands while a customer stands at the counter.

**The chain, and every link is ordinary on its own:**

1. `EmployeeAuthController` polls `me` every 12 s (added with the remote-pause
   feature, so a paused employee sees the veil within seconds).
2. `refresh()` assigned a **new** `EmployeeAuthState` on every poll — the class
   had no `==`, so two identical responses were two different states.
3. `routerProvider` did `ref.watch(employeeAuthProvider)` — the whole object.
4. **That provider builds `GoRouter` itself.** A new state meant a new router,
   a new navigation stack starting at `initialLocation`, and every pushed screen
   gone.

So the app worked perfectly for eleven seconds at a time. It is invisible to
`flutter analyze`, to every widget test, and to anyone who does not sit in front
of one screen for twelve seconds — which is to say, invisible to everyone except
the person trying to use it.

**Fixed in two layers, and neither alone is enough:**

- **`select` in the router.** It now watches only the four primitives `redirect`
  actually reads (`authStatus`, `onboarded`, `isMainAgent`, `empStatus`, plus the
  one permission the shared-route rule consults). A change to `paused`, a shift,
  or a POS name no longer touches the router at all. Re-evaluating `redirect`
  never needed a router rebuild — `refreshListenable` already does that.
- **Value equality on `EmployeeAuthState`, `EmployeeProfile`, `EmployeePos` and
  `OpenShift`, plus `if (next != state) state = next` in `refresh`.** The guard on
  the assignment is not redundant: `StateNotifier` compares with `identical`, not
  `==`, so an equal-but-new object still notifies. And the lists are compared with
  `listEquals` — `List.==` is reference equality in Dart, and the permissions list
  is rebuilt from the response every time, so without it no two states could ever
  be equal.

`test/employee_session_stability_test.dart` (7 tests) pins it, and was proven the
only way that means anything: replacing `listEquals` with `==` on the permissions
list makes the first test fail, and restoring it passes. It also asserts the
*opposite* direction — pausing, revoking a permission, or switching POS **must**
produce a different state, or the freeze veil would never appear.

**The rule this leaves behind:** a provider that builds the router must watch
primitives, never objects. Anything else is a periodic navigation reset waiting
for a poll to be added.

### The employee's transfers tab is the agent's screen, not a copy of it (10 Sep 2026)

Owner's order: *«تبويب مخصّص للحوالات نفس تبويب الوكيل … بأكمل الشكل والعرض
والبحث والفلترة … والواردة بنفس منطقها ونفس طريقة العرض ونفس طريقة التسليم
ونفسها في كل شيء»*.

**"The same" is taken literally: one `TransfersScreen`, built twice.**
`TransfersScreen(asEmployee: true)` is what `/employee/transfers` now builds, and
the 540-line `employee_transfers_screen.dart` — a parallel screen with its own
cards, its own tabs and its own delivery button — is gone. Two screens diverge at
the first edit, and then the employee's "same" screen is the agent's screen as it
was a month ago.

Exactly three things differ, all marked `asEmployee` in that file:

1. **The paths**, decided in `AgentIncomingRepository` by a new `TransfersMode`
   (list, deliver, outgoing-receipt). Nothing else in the screen knows.
2. **The source of «صادرة»** — the agent's statement, or `employeeOutgoingProvider`.
3. **What the permissions allow to show** — the incoming tabs and the outgoing
   section.

Everything else — the card, the receipt, the delivery flow, the search box, the
stage chips, the counters — is one piece of code.

**«صادرة» for an employee is what he created, and it needed a new endpoint.**
`GET device/employee/transfers/outgoing` (behind `VIEW_OWN_TRANSFERS`), scoped by
the owner's rule of 8 Sep — an employee does not see a colleague's work.

- **It is not built on `mine`.** That view reads its detail from
  `agent_incoming_transfers`, the **incoming** ledger; a transfer the employee
  *created* is outgoing to another agent and has no row there at all — it would
  render as a card with no beneficiary and no status.
- **Nor on the agent's statement.** `LocalStatmentAccount` carries the agency's
  running balance on every row, and that balance is behind a permission the owner
  deliberately keeps off the bulk-grant. So the endpoint reads
  `transfer_attributions` (action `CREATED`) for the codes, then `InternalEx` +
  `InternalEx_Stautes` **once per page of codes** for the real state — chunked at
  1000, because `InternalEx.Code` has no index and a per-row probe is the shape
  that made the statement take 68 seconds.
- It returns rows shaped as `Movement`, so `MovementRow` and `CoreStage` render
  them with no second card and no second model.

**⚠ «تم التسليم» now shows only what *this* employee delivered** — the owner, same
day: *«لا يظهر له كل الحوالات المسلَّمة في الوكيل، بل تظهر حوالته المسلَّمة من
قِبل الموظف فقط»*. The filter is a subquery on `transfer_attributions` inside
`AgentIncomingTransfersService`, so **the counter and the list pass through the
same rule** — a count that disagrees with its own tab is worse than no count. It
is `null`-guarded so the agent's own path is byte-for-byte unchanged.
**«بانتظار التسليم» stays agency-wide on purpose**: it is a shared work queue, and
a beneficiary must not be turned away because a colleague received the alert.
**«الملغاة» is also still agency-wide** — the owner named the delivered tab only,
and a cancelled transfer is nobody's work; say so before changing it.

Two defects surfaced while building it and are fixed: `EmployeeTransferViews`
compared `action === 'DELIVER'` while the writer stores `'DELIVERED'`, so every
transfer an employee delivered was labelled «أنشأتُها»; and the pause routes were
built as one interpolated string, which `app_routes_wiring_check` could not parse
— see the note in the batch review above.

### The audit committee sat, and what came out of it (10 Sep 2026)

«لجنة فحص تطبيق الصرافة» was convened. The full report is
[docs/reports/2026-09-10-audit-committee.md](docs/reports/2026-09-10-audit-committee.md)
and the server-side actions the owner's technician must perform are
[docs/reports/2026-09-10_server_runbook.md](docs/reports/2026-09-10_server_runbook.md).
**The verdict is `NOT READY FOR PRODUCTION`** and it stands — nothing since has
changed the four things holding it: no TLS, a test signing key, no privacy
policy, and the financial-features declaration.

The fixes landed in three commits (`10/09-02`, `10/09-03`, `10/09-05`). The
report lists them one by one; what is worth carrying here is the shape of them:

- **What was fixed unasked is exactly what is safe to fix unasked** — non-financial
  defects with a wrong answer and a right one: a dead «حذف الحساب» button, three
  repositories reporting success on any server rejection (`raw.put` bypasses the
  envelope decoder — use `_api.put`, which throws), a chat poll that never stopped
  in the background, `rand()` for OTP, PII in `laravel.log`, OTP rows returned in
  full to an unauthenticated caller, and a POS update that accepted any `ID`
  without a branch check (cross-company write — CRITICAL).
- **What touches money was fixed only under the owner's explicit authorisation**,
  and even then only as a *guard*, never as arithmetic: double-tap guards on the
  two money screens, an atomic claim on approval execution
  (`UPDATE … WHERE status='APPROVED' AND transfer_number IS NULL`), and
  `transInsert` writing `TransFrom` from the **session's** account rather than the
  request body — the check had always been on the session, so the body was a way
  to move money out of a third party's account.
- **What is left is the owner's, and it is written down rather than done:** the
  cleartext transport, the exposed `htdocs.rar` on the production server, secret
  rotation, session expiry, `verify=false` on the WhatsApp channel, and the
  retention policy the two stores' disclosure forms depend on.

**⚠ Two of those fixes need a coordinated deployment, not just a `git pull`.**
`device/reActivate` now demands a shared secret (`CUSTOM_X_TOKEN` in the backend
`.env`, `API_X_TOKEN` in the desktop's `RhallaConfig.ini`) — deploy the backend
without rebuilding the desktop app and the branch «إعادة التفعيل» button stops
working. And the throttling added on the OTP and login routes writes its counters
to the cache store, so `CACHE_STORE=file` must be set on the server or the
counters land in the production financial database. Both are step-by-step in the
runbook.

### ⚠⚠ Retiring a permission froze the permissions screen for everyone who had it (10 Sep 2026)

The owner granted a permission marked «تُمنح يدوياً» and the save came back
«صلاحية غير معروفة أو لا تُمنح لموظف» — nothing saved. The permission he granted
was fine; **the message named a different key entirely**, and he had no way to know
that.

**The chain:** the screen sends the **whole granted set**, not the delta. His
employees still carry the five cashbox/shift rows retired the same day — left in
the database on purpose, because they are inert and deleting them erases who was
granted what. So every save shipped them along, and the loop rejected the request
on the first key no longer in the catalog. Measured: 7 such rows across 2 employees.

**Retiring a feature therefore froze the permissions screen for every employee who
had ever held one of its keys** — no grant, no revoke, nothing, until someone
noticed the message named a key the agent never touched.

**The fix is one distinction the old condition could not express.** `grantable()`
is `exists() && !NEVER_FOR_EMPLOYEES`, and the admin keys are **not in the catalog
either** — so both cases produced the same "unknown" refusal. They are opposites:

- **Not in the catalog** ⇒ retired, or a typo from an older client. Nothing enforces
  it, so granting it gives nothing and refusing it protects nothing. **Dropped
  silently**, and the row cleans itself on the next save.
- **In `NEVER_FOR_EMPLOYEES`** ⇒ an attempt to hand an employee an admin
  permission. **Refused loudly** — that is the case the check exists for.

⚠ **And the order matters, or the loud half never runs.** Asking "is it known?"
first sends every escalation attempt into the silent branch: verified by running the
first version, where `MANAGE_EMPLOYEES` came out `dropped` instead of `REFUSED`.
`forbiddenForEmployees()` is asked first, and its doc comment says why it is not
merely the negation of `grantable()`.

The app now also intersects the selected set with the catalogue it received before
sending — not redundancy: the server decides, and this stops the bad request from
being made at all, so the screen never claims to have granted something that no
longer exists.

Verified: the four cases behave as `dropped / REFUSED 422 / SAVED / dropped`
(retired · admin · sensitive · typo), `employee_permissions_wiring_check` PASS,
`employee_sensitive_permissions_check` 19/19 with the financial snapshot,
`flutter analyze` clean, 214 tests green.

### ⚠⚠ The shift and the custody were removed a few hours after they were finished (10 Sep 2026)

Owner, the same day the automatic-shift work landed: *«في تطبيق الوكيل إلغاء نظام
الوردية والعهدة ليصبح حوالات فقط لتقليل الضغط وتقليل حدوث المشاكل … وألغِها من
كل التبعات بالكامل، لا وجود لعهدة أو وردية تفتح وتغلق، ليصبح التطبيق بالكامل
حوالةً استلمها أو حوالةً سلّمها فقط»*.

**What was deleted, and it is the whole subsystem:**

| Layer | Gone |
|---|---|
| Routes (6) | `cashbox` · `cashbox/ledger` · `cashbox/entry` · `shift/start` · `shift/close` · `reports/cashbox`, plus `custody` |
| Permissions (5) | `VIEW_OWN_CASHBOX` · `CASHBOX_ENTRY` · `START_SHIFT` · `CLOSE_SHIFT` · `REPORT_EMPLOYEE_CASHBOX`, and the whole `cashbox` group |
| Services | `EmployeeCashboxService`, `EmployeeCashboxLedger`, `EmployeeReports::cashbox`, the cash columns of the agent's employee report and dashboard |
| Screens | the cashbox screen, the ledger view, both shift screens, the custody block, the shift card |
| Writes | the three cashbox entries written on create / approve / deliver |

**⚠ Not one table and not one row was dropped.** `employee_cashboxes`,
`employee_shifts`, `employee_cashbox_entries` and `employee_shift_closings` keep
their data: those are movements of money that actually happened, and the standing
rule in this project is that a movement is never edited or deleted. **Its use was
cancelled; its record was not.** The same for the granted permission rows — no
route enforces them any more, so they are inert, and deleting them would erase who
was granted what and when.

**What remains is exactly the sentence he used.** `transfer_attributions` is
untouched, and it is literally "a transfer he received or a transfer he delivered":
the agent's employee report now shows delivered count and value, created count and
value, and nothing else. The employee delete-guard still probes the cashbox tables
— historical rows still count as a financial footprint, so an employee with past
cash movements still cannot be deleted.

⚠ **`ensureOpenShift`, added earlier the same day, went with it.** It is worth
recording why it existed rather than pretending it never did: three transfer paths
were writing cash entries under `if ($shift)` and silently writing nothing when no
shift was open, so an employee who never pressed «بدء وردية» had transfers that
never appeared in his custody. That defect is now moot — there is no custody — but
the shape of it is not: **a guard that silently does nothing is worse than a
refusal**, and that lesson outlives the feature.

Verified after removal: `flutter analyze` clean, **214 tests** green (three cashbox
cases and the ledger suite removed with the feature), `php -l` clean,
`employee_permissions_wiring_check` PASS, `app_routes_wiring_check` 3/3 over **99
paths**.

### The shift opens itself; only the employee closes it (10 Sep 2026)

Owner's order, and his own definition of custody: *«العهدة إمّا قيمةٌ مودعةٌ
كعهدة أو قيمةُ حوالةٍ مستلمة، وجميعها تُجمع … وإذا صرف قيمةً أو سلّم حوالة
فتنقص … بحيث يكون مفهومُ العهدة والخزينة واحداً، ويظهر في الأعلى ليرى الموظف
من الواجهة كم في حوزته»*, and *«الوردية تُفتح بمجرّد فتح يومٍ جديد وبأيّ حركة …
والإقفالُ يدويٌّ بعد أن يتمّ الجرد»*.

He also settled the scope question that had been holding this back: *«جميع ما
طلبته هو في عرض تصميم الموظف وليس له أيُّ علاقة بحسابات الرحالة ولا العمليات
الحسابية والشجرة الحسابية والدائن والمدين للمنظومة الرئيسية»*.

**The arithmetic did not change, because it already said exactly this.**
`expectedCash` is `opening + in − out`, where IN is the declared opening custody
plus created-transfer values plus cash received, and OUT is payouts plus delivered
transfers. His 100 + 1000 − 200 − 500 = 400 is that formula, unmodified. What
changed is when a shift exists, and how the number is presented.

**`ensureOpenShift` — the first movement opens the shift.** Wired into the four
paths that write a movement (create transfer, execute approval, deliver, manual
cashbox entry); the four that only *read* (`me`, custody, ledger, close) still use
plain `openShift`.

- **⚠ What it replaces is worse than a refusal.** The manual entry was refused with
  «ابدأ وردية أولاً», but the three transfer paths were `if ($shift)` and then
  **nothing** — an employee who forgot to press «بدء وردية» created and delivered
  transfers whose cash never appeared in his custody. A refusal informs; silence
  hides.
- **Opening cash is zero**, because nobody declared any. The «بدء وردية» button
  stays for the employee who actually receives a float and types its value.
- **No `START_SHIFT` permission is required** for the automatic open: it is the
  system acting, not the employee. Requiring it would put whoever lacks it back in
  the silent state — the same defect wearing another coat. The permission still
  guards *declaring an opening float*.
- **Nothing ever closes automatically.** A new day opens a shift if none is open; it
  does not close one. Closing is a stocktake and a handover of cash, and it does not
  happen by the clock — so yesterday's open shift stays open and today's movements
  join it until he counts.
- **Concurrency is a unique filtered index**, `UX_shift_open_employee` on
  `employee_shifts(employee_id) WHERE status='OPEN'`
  (`deploy/2026-09-10_shift_single_open.sql`), with the loser re-reading the winner's
  row. ⚠ The script refuses to create the index if an employee already has two open
  shifts and prints them instead: closing one is a stocktake, not data cleanup.

**«في عهدتك» moved from a footnote to the number it is.** It was 11.5pt under the
name; it is now a block at the top of the employee's header on the same scale as
the agent's balance (15 · 30 · 19), with the three terms of the equation under it —
`opening + in − out` — so the answer to "where did this number come from?" is on
screen instead of behind a full statement. The sign still flips the sentence rather
than the number («في عهدتك» / «مستحقٌّ لك»), and no shift still shows nothing rather
than zero — though with the automatic open that state now means literally "nothing
has happened yet".

Verified: `employee_cashbox_acceptance` 18/18 and `employee_cashbox_ledger_acceptance`
59/59, both ending in the financial-invariant snapshot — `wallet`, `ExchangeAccData`,
`InternalEx`, the safes and the chart of accounts identical before and after.

### ⚠⚠ Why the theme half-applied, and then stopped opening (10 Sep 2026)

The owner, in one sentence each: *«فتحتُ وغيّرتُ الألوان — لأجزاءٍ من التطبيق،
وأجزاءٌ بقيت بالثيم السابق»*, then *«رجعتُ لأعدّل الثيم — لم يفتح بالمرّة»*.

**Two unrelated defects that looked like one broken screen.**

#### The catalogue was being emptied by the save itself

`GET company/branding` returns `{branding, can_edit, themes}`. `PUT`, the logo
upload and the reset returned `{branding}` **only** — and the app parses the whole
response into a `Branding` and replaces what it holds. So the first successful save
left `themes: []`, and the picker after it had nothing to show. Nothing was wrong
with the button or the sheet: **the save is what emptied them.**

Fixed in the server (one `payload()` used by all four endpoints, so the shape
cannot drift again) and guarded in the app (`_merge` keeps the known catalogue when
a response carries none) — the second is not redundant: a phone updates before a
server does, and a new app against an old server would hit the same wall.

**The rule:** an endpoint that returns *part* of a shape the client parses *whole*
is a deletion on a delay.

#### The colours only reached the screens that had not been built yet

This one was documented in `router.dart` from the beginning and still bit:
`go_router` keeps `StatefulShellRoute` branches alive by `GlobalKey`, so a branch
that was already built is **moved, never rebuilt** — and `R`'s colours are statics
read at build time. Any tab the agent had visited before saving kept its old
palette for the rest of the session. The `KeyedSubtree(ValueKey(epoch))` in
`main.dart` rebuilt everything *above* the `Navigator` and never reached them.

So `routerProvider` now watches the brand epoch and **rebuilds the router itself** —
new branches, new keys, every screen built fresh with the new colours.

- **⚠ This is not a contradiction of the `select` fix above.** That one stopped a
  rebuild that happened *every twelve seconds for no reason*; this one happens when
  the identity actually changes — sign-in, save, reset — a handful of times in the
  app's life, each on an explicit tap.
- **The location is preserved** (`_lastLocation`, updated in `redirect`). Without it,
  saving a theme from the settings screen throws the agent to the home tab, and a
  successful save reads as being kicked out.
- **And the `KeyedSubtree` epoch key was removed, not kept as a belt.** Tearing the
  tree down while a new router mounts puts `_rootKey` — one `GlobalKey` — inside two
  live trees in the same frame, which is precisely how "Duplicate GlobalKey" is
  produced. The router rebuild subsumes what it was doing.

`tests/manual/company_branding_acceptance.php` still passes 13/13, financial
snapshot included.

### Three small things the owner saw before anyone else (10 Sep 2026)

- **The commission row now carries its transfer number.** `COMMTION_RETVIEW`
  returns `ISID` — the transfer's `Code`, `1111-1-1` — and «عمولاتي» was showing
  amount, date and branch only. Three commissions on one day can match on all
  three, so a commission that looks wrong could not be traced to its transfer.
  Rendered LTR beside the date; the paragraph's RTL run would otherwise reverse it.
- **The circle beside the name in both home headers carries the company's logo.**
  `BrandAvatar` — the logo the moment it is saved, the initial when there is none,
  and the initial again if the image fails (a broken-image icon reads as a bug, a
  letter reads as a design). ⚠ **The white disc under it is not decoration**: logos
  arrive as uploaded, mostly dark ink on white, and the header is a dark gradient —
  the same rule already applied to the invoice header and the arrival banner.
- **The balance line was three mismatched sizes** — `13 · 44 · 22`, the integer
  three and a half times the currency symbol, so it read as three separate blocks
  rather than one number. Now `15 · 30 · 19` with the weights stepping down with
  the sizes, and the fraction no longer separated by an 8px gap that made it look
  like a second number.

### Remote pause of an employee, and one version number (10 Sep 2026)

The agent can freeze one employee or all of them from the employees screen; the
frozen employee sees a full-screen «توقّفٌ مؤقّت» and can do nothing until the
agent lifts it. `2026-09-10_employee_pause_gate.sql` adds `employees.paused_at`
and a one-row-per-agent `employee_pause_gate` table — additive, idempotent, and
**no financial table is touched**: it is a flag and a veil, no balance, no entry,
no commission.

Four decisions in it:

- **The two switches are independent.** Lifting the group pause does not resume an
  employee the agent had paused individually — otherwise «تشغيل الكل» silently
  un-punishes the one person the agent meant to keep stopped.
- **The refusal is in `AuthenticateEmployee`, not in the screen.** The middleware
  returns 403 `EMPLOYEE_PAUSED` for every *permissioned* route, so a paused
  employee cannot act from outside the app either. `me`, `logout` and `branding`
  carry no permission and stay open deliberately — the app must be able to learn
  its own state and leave; a pause that also blinds the client produces a frozen
  screen with no explanation.
- **`EmployeeFreezeGate` is mounted above the `Navigator`** in `main.dart`, like
  `AmbientBackground` and the lock veil, so it covers whatever screen the employee
  was on. It appears in employee mode only, because `paused` is only ever set on an
  employee profile.
- **Nothing is lost.** The session stays, the device stays bound, the drafts stay;
  the `me` pulse (12 s) lifts the veil on its own when the agent resumes.

**The version number now has one source and a test that keeps it that way.**
`core/app_version.dart` holds `kAppVersion` (displayed in the account screen's
footer beside the developer name) and `pubspec.yaml` holds the number that becomes
the APK's `versionName`. They had already drifted — the screen said **1.0.1** while
the built APK said **1.0.0** — which is invisible to `flutter analyze`, to every
test and to a run, and surfaces as a bug report against a version that was never
shipped. `store_compliance_test.dart` now reads both files and fails if they
disagree, and fails if the build number ever goes backwards (Play refuses a
`versionCode` that does not rise). It was proven by breaking it: both checks fail
on the drifted pair and pass on the fixed one. **Bump both together, every
release.** Current: `1.0.1+2`.

Three defects fixed alongside it, each one a thing the app claimed and did not do:

- **Voice notes were recorded as Opus and could not be played back on every
  device.** They are now AAC-LC in an `.m4a` container — the one encoder Android
  and iOS both guarantee for recording *and* playback. `ChatService` also accepts
  `audio/x-m4a` and `audio/m4a`, because `finfo` reports the same file under
  different names and the upload was being rejected on the server's own sniff.
- **The employee's transfer list read `rows`/`data` while the server returns
  `items`** — so the counter showed a number and the list below it was empty.
- **The chat threads list only refreshed on a manual pull.** It now polls every
  8 s with `skipLoadingOnRefresh: true` (no spinner flash), stops when
  backgrounded and pulses once on resume — the same shape as every other poll here.

«عمولاتي» was split out of the limits screen into `reports/commissions_screen.dart`
with its own route, and the employee cards on the agent's employees screen fold.

#### The committee re-read its own two batches, and eight things came back (10 Sep 2026)

A review of `10/09-05` and `10/09-06` against the source. Everything below is
fixed and green (`flutter analyze` clean, **227 tests**, `php -l` clean on every
touched file). **Nothing accounting was touched** — the owner's condition — and
the one finding that lands inside the cashbox was deliberately left as a comment
correction rather than a code change.

- **⚠ The voice-note fix worked on iOS and failed on Android — measured, not
  guessed.** `ChatService` reads the type with `finfo` (content, never the
  request header). iOS writes an `M4A ` brand and is read as `audio/x-m4a`, which
  the batch added. Android's `record` routes AAC-LC through `MediaMuxer`
  (`AacFormat.kt` → `MUXER_OUTPUT_MPEG_4`), which stamps `mp42` — and `finfo`
  reads *that* as **`video/mp4`**, which was not in the allow-list. So the
  recording was made on the employee's phone and then refused by the server, on
  the one platform every branch device runs. `video/mp4` now maps to `AUDIO`/`m4a`;
  the cost is that a genuine mp4 video would be labelled audio, and no path in the
  app uploads video.
- **⚠ `addUserTrans` still wrote `ID_UESER_ACCID` from the request body** while
  `$user_id` sat read and unused two lines above — the same shape as the `transInsert`
  hole the committee closed as CRITICAL, and the twin of the ownership check it
  added to `deleteUser`. Any signed-in agent could write a row into another agent's
  favourites. It now writes the session's `AccID`. No accounting effect:
  `AddUserTransTb` is a list of preferred beneficiaries — no balance, no entry.
- **The freeze screen ordered an action the app forbade.** It says «تواصل مع
  الإدارة» while chat sits behind `CHAT_WITH_AGENT`, which the pause gate refused
  like any other permission — so a paused employee had no way to reach his agent
  from inside the app. `EmployeePermissions::ALLOWED_WHILE_PAUSED` now names what
  survives a pause (chat, and only chat), and the middleware consults it. The list
  lives with the rest of the permission policy, not in the middleware, so tomorrow's
  key is decided in one place.

  ⚠ **And the server-side exception alone would have changed nothing**, which is the
  part worth remembering: the veil wrapped the *whole* screen in an `AbsorbPointer`,
  so every button inside it was dead too — including any door one might add. The gate
  is now two layers, an absorbing veil underneath and the content above it, and it
  carries a «مراسلة الوكيل» button shown only to an employee who actually holds
  `CHAT_WITH_AGENT` (a button that opens and then gets a 403 is worse than no button).
  Opening a permission on the server while the UI cannot reach it is a fix that
  measures as done and reads as broken.
- **The employee's `me` pulse never stopped in the background.** Every other poll
  in this app stops on `paused` and pulses once on `resumed` — `10/09-06` added
  exactly that to the chat screen and then opened a 12 s timer here that ran
  forever. `EmployeeAuthController` is now a `WidgetsBindingObserver` like
  `AppLockController`.
- **The account footer lost the company's name.** «رحلة · اسم الشركة» was
  *replaced* by the copyright line rather than joined by it, which quietly reversed
  the owner's decision of 3 Sep («كل شيء باسمها ظاهرياً») that the account footer
  carries the tenant's identity like the invoice header does. The footer is now
  three lines: company, copyright, version.
- **The token cache could be repoisoned by a read already in flight.** `readToken`
  is async, so a read that started before a sign-out finished after it and wrote the
  **old** token back into the cache — every later request then carried a dead token
  and ended in a 401 the user could not explain. A generation counter now guards the
  write-back, and every mutator invalidates **before and after** its awaits (a read
  starting in between would otherwise have cached the pre-write state).
- **`pause-all` was check-then-insert on a primary key.** Two taps in the same
  instant meant a duplicate-key 500 on an operation that had in fact succeeded. It
  updates first and only inserts when nothing was updated, with the duplicate folded
  back into an update.
- **The chat threads poll was 3 s** — twenty requests a minute from one screen,
  against 30 s for the bell and the employee's incoming list. Now 8 s.
- **⚠ The four pause routes were invisible to `app_routes_wiring_check`.** The
  repository built them as one interpolated string with the verb chosen inline, so
  the extractor read `employees/{p}/${paused` and the suite failed — and a path that
  the check cannot parse is a path outside the guard it exists to provide (a typo in
  a route string is invisible to `flutter analyze` and to every test, because it is
  text, not a symbol). Both calls now pick between two complete literals. Re-run:
  **104 paths, all registered, all three checks pass.**

  And a trap worth knowing before writing the comment that explains this: **the
  check reads the Dart file as raw text and does not strip Dart comments**, so the
  first version of that very comment quoted the old interpolated path — and the
  suite kept failing, now on the documentation of its own fix.

Two more, and what was deliberately *not* changed:

- `employees_screen.dart` had ~110 lines inside `if (_expanded) ...[` indented as
  though they were outside it. The structure was correct; the indentation was an
  invitation to break it. Re-indented, no behaviour change.
- **`addEntry`'s duplicate catch was left exactly as it is.** Its comment claimed
  the condition was SQL Server's `2601/2627`; the code actually keys on finding a
  matching row, so an unrelated query failure with a pre-existing `client_ref` would
  read as a duplicate. The window is narrow, and this is the cashbox ledger — under
  the standing red line — so only the comment was corrected to say what the code
  does. Changing it needs the owner's word.

**And a test that had started lying.** `employee_home_permissions_test` decided
"a tile is on screen" by counting `InkWell`s above a hardcoded baseline of one
(the logout button). `10/09-07` added a second header button — «الأمان», permissionless
by design — and the check began failing on `CLOSE_SHIFT` for a contradiction that did
not exist. The baseline is now *measured* by mounting the screen with no permissions
at all, and is itself bounded (`expect(chrome, lessThan(4))`) so a future screen that
renders tiles unconditionally cannot inflate the baseline and swallow the very defect
the test exists to catch.

### ⚠⚠ Release APKs could not open a socket at all (9 Sep 2026)

An employee's device reported «تعذّر الاتصال بالخادم» after scanning the QR *and* after typing the code, on a healthy network. It was not the QR, not the code, not the server.

**Three measurements settled it:**

| Fact | Value |
|---|---|
| `targetSdkVersion` in the merged release manifest | **36** — Android blocks cleartext HTTP by default from API 28 |
| `networkSecurityConfig` / `usesCleartextTraffic` in the **release** manifest | **absent** — the allowance lived only in `src/debug/`, and only for `10.0.2.2` / `localhost` / `192.168.1.10` |
| The server | **alive on HTTP** (0.26 s), and **HTTPS does not open a socket** |

So every release APK built against `http://102.214.165.242:8080` was incapable of reaching it — the failure is at the socket, before any request. The app's message was truthful; the build was not usable. **This file had already named it the launch blocker, and release APKs were handed over anyway.**

**The bridge, and it is a bridge.** `android/app/src/main/res/xml/network_security_config.xml` permits cleartext **for that one IP literal**, with `base-config cleartextTrafficPermitted="false"` so everything else stays TLS-only. `android:usesCleartextTraffic="true"` was deliberately *not* used — it opens every host, so a mistyped address or a third-party redirect would travel unencrypted too.

⚠ **What it costs, plainly:** balances, transfer numbers and verification codes cross the network unencrypted, readable by anyone sharing it. And with the app now headed for Google Play and the App Store, this is a submission problem as well: Play flags cleartext in review and Apple's ATS refuses it without a written justification that a financial app rarely gets.

**Removing it is one line in the manifest plus one file** — and that is the point of scoping it this way rather than flipping a flag. The real prerequisite for submission is a TLS certificate on the server, not an edit here.

Verified the only way that means anything: `aapt2 dump xmltree` on the built APK shows `networkSecurityConfig` present in the packaged manifest, not merely in the source tree.

#### ⚠ And it happened a second time — `build_apk.bat` now prevents it (10 Sep 2026)

The APK handed over on 10 Sep was built against `http://192.168.1.10:8000/api`
(this machine's LAN backend, for a phone on the same Wi-Fi) and carried
`network_security_strict.xml`, which permits **no** cleartext at all. Read back
out of the packaged APK: no `domain-config`, no host, nothing — so the app could
not open a socket to the address compiled into it, on any network. Exactly the
defect above, one day later, for the opposite reason: the safe default had been
made the default and nothing taught the build script about it.

`build_apk.bat` step 2b used to add the host to the **debug** config only, and
said in its own comment that a release APK over http "cannot work". That is true
of a *store* build and false of the test APK the owner actually installs. It now
picks the config by build type — debug → the debug allow-list, release/profile →
`src/main/.../network_security_config.xml` — and passes `-PallowCleartext=true`,
printing that the result is a test APK. The store path is still closed by
construction: gradle throws if `bundleRelease` runs with the flag, and Play's
artefact is the bundle.

**⚠ `build_apk.bat` must keep CRLF line endings.** It was LF in the repository and
had been surviving on luck: `cmd.exe` re-seeks the file by byte offset after each
command, so an LF-only batch file breaks the moment its content length shifts —
which it did on the first edit, producing `'e' is not recognized`, `'eok' is not
recognized` and a bogus "could not detect this machine's LAN address". The file is
now CRLF. Do not let an editor normalise it back.


### ⚠⚠ The sovereign approval gate — nothing reaches the agent before Rhalla approves it

Owner's standing rule, restated on 9 Sep 2026 after he found it broken: *«عند تنفيذ حوالة من الرحالة لا تصل إلى الوكيل ولا يراها في التطبيق ولا يصل إليه أيُّ إشعارٍ أو رسالة إلّا بعد أن تُعتمد من إدارة الرحالة».*

**Why it came apart, and it is the lesson, not the bug.** `syncFromCore` ingests `ConfirmType = 2` and nothing else, so the rule looked guarded at the door. It was not: **approval is withdrawn after arrival**, `refreshCoreState` updates the number on the existing row and does not hide it, and every reader asked "is it *not cancelled*?" (`CORE_CANCELLED = [3,4,5,6]`) — so state `0`, *not approved*, sailed through all of them.

It leaked in **seven** places: the list (delivered / cancelled / no-tab branches), the counts, **the alerts bell**, the employee's pending report, the agent's summary, the employee transfer views, and the home-screen badge. Guarding the entrance is not guarding the thing; **every read path is a gate**.

`AgentIncomingTransfersService::onlyApproved()` is now the single place that knows what may be seen, and every reader passes through it:

- **`CORE_VISIBLE = [2, 3, 4, 5, 6]` is an allow-list, deliberately.** Approved, plus cancelled-after-approval (the agent must learn it was cancelled). A deny-list would silently admit every new state the core system invents later — a door nobody opened.
- **`null` is admitted on purpose**: a row whose state has not been refreshed yet. It cannot have entered unapproved (sync requires 2), and excluding it would empty the screen on a first run before sync completes.
- **The gate sits on the base query, not on the branches.** A branch added tomorrow inherits it instead of waiting for someone to remember.
- **`markDelivered` refuses too** (`not_approved`, surfaced in both delivery paths as «هذه الحوالة غير معتمدة في المنظومة — لا يجوز تسليمها»). Hiding it from a tab is cosmetic: the screen may have been open since before the withdrawal, and the request can come from outside the app.

The seventh place — the two subqueries in `depositController` that label a row of the agent's own statement — was examined and **left alone**: an unapproved transfer produces no `ExchangeAccData` movement at all (verified: `InternalEx` held 2 unapproved rows while `ExchangeAccData` was empty), so there is no row to label, and editing that large financial statement query buys nothing against real risk.

**`tests/manual/approval_gate_check.php` (14 checks) guards it in two layers**, because a behavioural test only covers the ports that exist today:

1. **Behavioural** — it creates a genuinely unapproved row and asks every port whether it can see it: four tabs, search by number, the counters, **the bell**, the employee report, and delivery. Then it approves the row and confirms it appears immediately (the gate is not a blanket ban), then withdraws approval and confirms it disappears again — the exact case that broke.
2. **Structural** — it scans `app/` and `tests/manual/` for any file touching `agent_incoming_transfers` without going through `onlyApproved`, and fails naming the file. Exemptions are listed **by name with a written reason**, never by silence. So a port added next month and written the old way fails this check instead of waiting for an agent to pay out cash on a transfer the system does not recognise.

Proven by reintroducing the old predicate: the check failed on five ports at once, then passed again when restored.

⚠ **The pre-existing `agent_incoming_acceptance` check was measuring the wrong thing** and had been failing (`leaked=1`). It counted unapproved rows *present in the table*, which catches a sync leak and nothing else — and a row whose approval was withdrawn legitimately stays (approval can return; deleting it erases its history). It now measures what it can honestly prove: that **sync introduces no new** unapproved row, with visibility and deliverability covered by the two checks added beside it.

### Readiness audit (9 Sep 2026) — three real defects found and fixed

The owner asked for a full readiness pass over both apps. Everything was green before it started, which is exactly why the pass was worth doing: both defects were in paths no test covered.

**⚠ 1. `device/update/password` was still a public account-takeover route.** Knowing a phone number and a device id was enough to set a new password with no OTP and no session, and `device/login` then accepts it — full control of a financial account. This file and `auth_repository.dart:151` had *named* it a hole since `otp/login` was added, and the route stayed open anyway; nothing in the project calls it (not the agent app, not the desktop app, not the support SPA).

Closed in two layers, because either alone is insufficient: the route now requires `auth:sanctum`, **and** the handler rejects a phone that is not the session owner's — the phone arrives in the request body, so a signed-in agent could otherwise have reset another agent's password. `tests/manual/auth_surface_check.php` (10 checks) holds both shut and also guards the routes that must stay open (`otp/send`, `employee/activation/request`) against being closed by mistake, since closing those blocks every login.

**⚠ 2. The idle lock fired with no session behind it.** An agent sitting on the phone/OTP screen — or an employee on the activation screen — who backgrounded the app for six minutes came back to «التطبيق مقفل» over a login screen. Tapping «فتح بالبصمة» led to the same login screen; a device with no biometrics offered only «الدخول بالتحقّق من جديد», to someone who had not logged in. `AppLockController` now reads `SecureStore.readToken()` (which returns the employee *or* agent token, so one guard covers both modes) and does not lock without one. `lockNow()` carries the same guard and became async for it.

**⚠ 3. An un-approved transfer stayed deliverable.** `syncFromCore` ingests `ConfirmType = 2` and nothing else, so the ledger only ever receives approved transfers — but **neither the display nor the delivery path re-applied that rule**. The pending filter asked "is it not cancelled?" (`CORE_CANCELLED = [3,4,5,6]`), so a transfer whose approval was **withdrawn in the core system after it arrived** kept sitting in «بانتظار التسليم», and `markDelivered` would record a delivery for it. The agent pays out cash for a transfer the system no longer recognises.

`CORE_APPROVED = 2` now gates both: the pending tab shows only `core_confirm_type = 2` (or `null`, meaning never refreshed — excluding null would empty the tab on a first run before sync), and `markDelivered` returns `not_approved` for anything else, surfaced in both delivery paths as «هذه الحوالة غير معتمدة في المنظومة — لا يجوز تسليمها».

The existing suite caught it — `agent_incoming_acceptance` had been failing on «غير المعتمدة لا تصل إلى الوكيل» (`leaked=1`). ⚠ **But that check only measured whether the row exists in the table**, which catches a sync leak and nothing else; the row in question was old residue, and deleting it plus re-running sync confirmed sync does not recreate it. The check now also asserts the two things that actually protect the agent — that such a row is **not shown** in the pending tab and that `markDelivered` **refuses** it. 13 → 16 checks.

**Two new checks that no existing suite performed:**

- `tests/manual/app_routes_wiring_check.php` extracts every `/device/*`, `/agent/*`, `/employees*` path written as a string literal in `rhalla_agent/lib` and matches it against Laravel's route table — 99 paths, all registered. **A typo in a path string is invisible to `flutter analyze` and to every test**, because it is a string, not a symbol; it surfaces as a 404 in an agent's hands. The same file also asserts no employee route lacks a session guard and no financial route is open without authentication (`forgien/exchange/deposit/store` is excluded **by name** — it is a customer account-opening request that writes to a request queue, not a ledger).
- A route-middleware sweep confirmed the only unguarded employee routes are the three activation endpoints, which cannot have a session by definition and are guarded by rate limiting and code/OTP verification instead.

**What the audit could not verify, and why:** the employee UI against a live session on the emulator. That needs an activation code plus an OTP delivered to a real WhatsApp number, and sending real messages to the owner's staff to satisfy a test is not something to do unasked. The employee surface is covered instead by the HTTP acceptance suites, which forge sessions for throwaway employees and delete them.

### App lock after idle, and why OTP AutoFill cannot apply here (9 Sep 2026)

Owner asked for faster verification and an idle lock. **The app is going to Google Play and the App Store** (his instruction, same day), so every part of this was measured against both stores.

#### What was already there — and was not rebuilt

Auto-verify on the fourth digit already existed on **both** OTP screens (`otp_screen.dart:89`, `employee_activation_screen.dart:89`). Nothing was added for it.

#### ⚠ OS AutoFill genuinely cannot present on these screens

`AutofillHints.oneTimeCode` (and iOS `textContentType: .oneTimeCode`) renders **above the system keyboard** and needs a focused real text field. Neither OTP screen has one: they are display boxes plus a drawn keypad, and that is a written decision — `employee_activation_screen.dart:64` says «الشاشة لا تحوي `TextField` للرمز أصلاً، فلا كيبورد نظام يفتح لها». Adding a hidden field to summon AutoFill would open the system keyboard over the drawn keypad, to surface a suggestion that **would not appear anyway**: the OS reads *SMS* for one-time codes, not WhatsApp.

And reading WhatsApp is not something to engineer around — no Accessibility Service, no Notification Listener, no `READ_SMS`. Each is a Play rejection and an App Review rejection.

So the friction was removed where it actually is: **«لصق الرمز المنسوخ»** (`otp_paste.dart`). `extractOtp` takes the message as copied («رمز التحقق الخاص بك هو: 4821»), not a bare number, and refuses to guess — a phone number in the clipboard (`0922015243`) yields nothing rather than `0922`, which would burn an attempt, and two candidate 4-digit runs yield nothing. The clipboard is read **only on an explicit tap**: iOS shows its own paste prompt and Android 12+ toasts, and a silent read on screen-open produces that toast for no reason the user can connect to anything. Both screens route the pasted code into the same `_verify` the keypad uses — the server remains the only thing that decides.

#### The lock itself

`app_lock.dart` + `lock_gate.dart`, mounted above the `Navigator` in `main.dart` so it covers whatever was on screen. 26 tests in `test/app_lock_test.dart`.

- **⚠ Idle is measured from a stored UTC timestamp, never an in-memory timer.** A timer stops when the OS suspends the app, so the user returns after an hour having aged the timer by seconds — and the lock never fires. The stored stamp also means closing the app does not evade it. UTC because a phone clock is set by hand and moves with the timezone.
- **⚠ `inactive` does not record a departure; `paused` / `detached` / `hidden` do.** `inactive` fires for every system surface that covers the app — permission dialogs, the share sheet, an incoming call, **and the biometric prompt itself**. Recording it there makes a successful biometric unlock re-lock the app at the moment it succeeds.
- **The stamp is written once**: `paused` then `detached` arrive in sequence, and letting the second overwrite would shorten every measured absence.
- **It is a lock, not a logout.** The session stays, the widget tree stays mounted (so a half-filled transfer form survives), and the router does not move. The fallback for a device with no biometrics is a real sign-out plus the full verification flow — anything lighter would make the lock decorative.
- **⚠ The veil also covers `unknown`,** the first frame before the stamp has been read. Otherwise balances paint and then hide — a flash that shows exactly what the lock exists to hide.
- **The App Switcher veil keys on `inactive`** — the opposite of the lock, deliberately — because that is when the OS takes the preview snapshot. A fully blank Android preview needs `FLAG_SECURE`, which also disables the screenshots an agent may need for support, so it was left as the owner's call rather than imposed.
- **No biometric data is stored or ever reaches the app.** `local_auth` over `BiometricPrompt` / `LocalAuthentication` returns success or failure and nothing else. `USE_BIOMETRIC` only, not the deprecated `USE_FINGERPRINT` (Play flags deprecated permissions in review), and `NSFaceIDUsageDescription` is mandatory — without it iOS terminates the app on the first call and Apple rejects the build.
- **`MainActivity` is now `FlutterFragmentActivity`.** `BiometricPrompt` requires a `FragmentActivity`; with `FlutterActivity` the failure appears **only at the first unlock attempt in a user's hand** — not in analyze, not in the build.
- **The class is `AppLockState`, not `LockState`** — Flutter exports its own `LockState` from `shortcuts.dart`, and the short name breaks every file that imports both.
- APK cost: arm64 28.94 → 29.4 MB.

#### ⚠ One Tap verification is designed and blocked on TLS — nothing was shipped for it

The intended path — a link in the WhatsApp message that opens the app and completes verification with a single-use, short-lived, attempt-bound token verified server-side — is **blocked by transport, not by effort**:

1. The gateway is send-text only (`send-text` on `wa.rhalla.online`). That part is fine: WhatsApp linkifies a URL in the message body.
2. **But WhatsApp only linkifies `http(s)://`, and this backend has no HTTPS** (`http://102.214.165.242:8080`). A custom scheme (`rhalla://`) is not linkified at all, and any other installed app can claim it.
3. **Android App Links and iOS Universal Links both require HTTPS** plus a hosted `assetlinks.json` / apple-app-site-association to be verified. Without verification there is no secure binding between the link and this app.

Shipping it over HTTP would put a single-use authentication token in a cleartext URL that also lands in WhatsApp history and browser history. That is the same cleartext problem this file already names as the launch blocker, and it is **a store blocker on its own**: Apple's ATS and Android's default cleartext policy both refuse it, and neither `usesCleartextTraffic` nor an ATS exception is an acceptable answer for an app that carries balances and transfer codes.

So One Tap waits on the TLS certificate that store submission needs regardless. No table, endpoint or deep-link scheme was added for it — an unused auth path is a liability, not progress.

### Scan-to-find a transfer: built, then stopped at the owner's own decision (9 Sep 2026)

`lib/features/transfers/transfer_qr.dart` (+18 tests) and the now-parameterised scanner exist and are green; **nothing user-facing ships**, because completing the loop would have reversed a decision the code records explicitly.

The idea: the collection code goes on the receipt as text plus a QR, and the receiving agent or employee scans it instead of having the customer read a code aloud. Both dependencies are already bundled (`qr_flutter`, and `mobile_scanner` at 7 MB used only for employee activation), so it costs nothing to ship and needs no backend change — a scan just fills the existing search.

**Then `success_screen.dart:28` turned up: `ولا رمز حوالة فيها — أزاله المالك صراحةً`.** The absence of a code on that receipt is the owner's explicit removal, not an oversight, and a collection code on a printed money document is exactly the kind of thing an owner removes for fraud reasons. It was reverted rather than argued with.

The question left for the owner is a real distinction, not a re-ask: `CreatedTransfer` carries **two** codes — `code` (`Code`, the internal system key, e.g. `11261-54-13`) and `mobileCode` (`Code_For_mobules`, e.g. `542613`), whose own doc calls it «الرمز الذي يُعطى للمستفيد» and which `shareCode` exposes. `send_accounts_screen.dart:573` already displays `shareCode` on the account-transfer receipt, so showing it is not banned app-wide. Whether the removal covered the beneficiary's code as well as the internal one is his to say.

What was kept, because it is inert and breaks nothing: the payload module with its tests, and `EmployeeQrScanScreen` generalised to take `accept` / `title` / `hint` / `note` / `fallbackHint` (defaults identical to the activation behaviour, so that path is byte-for-byte unchanged). What was **not** kept: the QR and code on the receipt, the code in the share text, and the scan buttons on both incoming-transfer screens — a scan button with no QR to read is a button that opens and does nothing, which is the failure this codebase already refuses elsewhere.

### The agent's balance is never granted by a bulk tap (9 Sep 2026)

Owner's instruction: the agent's balance must not appear in the employee app except by a permission the agent grants deliberately.

**The balance was never leaking.** That was checked before changing anything: a session granted *every* permission except the three financial ones was pointed at every employee route, and `balance`, `summary` and `reports/agent-balance` all returned 403 while no other endpoint's body contained the number. `tests/manual/employee_sensitive_permissions_check.php` (19 checks) keeps that true.

**What was actually wrong is «تحديد الكل».** The reports group contains `REPORT_AGENT_BALANCE`, so one tap meant to hand over the daily and delivered reports handed over the agent's balance with them — and the agent had no reason to look. Same for the balances group.

So `EmployeePermissions::SENSITIVE` lists the two keys that expose that number, and:

- **«تحديد الكل» skips them; «إلغاء الكل» clears them.** Deliberately asymmetric — the risk is in granting, and a revoke that quietly skipped one would leave the balance exposed while the agent believed he had closed the door.
- **Switching one on asks first, switching it off does not.** A confirmation on the way out teaches the agent to tap through the one that matters.
- **The question names what will be exposed** («سيرى الموظف رصيد وكالتك الكلّي») rather than "are you sure?". The sentence comes from the server (`SENSITIVE_WHY`), like the flag itself — no permission name and no warning text is written in any Dart file, so marking another key tomorrow needs no app release.
- **The group's all/none button is computed over the non-sensitive items only.** Measured against the whole group it would never flip to «إلغاء الكل» in a group containing a sensitive key: the agent taps, everything grantable is granted, the label does not change, and he taps again thinking the screen is broken.
- **`VIEW_FINANCIAL_SUMMARY` is deliberately *not* marked**, even though it sits in the balances group — `EmployeeReports::summary` returns today's count and total, the cashbox in/out/net and the pending count, and no agent balance. A flag that is not true where it claims to be teaches the agent to ignore the flag.

The employee's «الأرصدة» tile now takes its title and subtitle from what was actually granted; it used to promise «رصيد الوكيل» to an employee holding only the summary permission, who would open it, find no balance, and go ask the agent for it.

Unrelated but found while running the suites: `employee_permissions_wiring_check` had been failing on `REPORTS_VIEW`, which is `LIVE` with no route enforcing it — correct, because it only opens the reports section in the app while each report inside is guarded on its own route. It is now declared in `EmployeePermissions::UI_ONLY` and the check reads that list, so the rule states the truth instead of failing permanently — a check that always fails is a check nobody reads.
**`CREATE_TRANSFER` is now wired — under the owner's explicit authorisation of 8 Sep 2026**, which came with the shape of the thing attached: «الموظف ينفذ الحوالة وكأنه الوكيل، عبارة عن واجهة من وكيل وليس مستقلاً استقلالية تامة». So the employee is a *face* of the agent, not a second party.

That single sentence decides the architecture, and `EmployeeActsAsAgent` implements it literally: the employee's request is executed **as the agent** (`Auth::setUser($agent)` inside a try, restored in a `finally`), through the same `InternalExchange` the agent's own app calls. Verified byte-for-byte on a real transfer — `AccFrom`, `uesrID`, `SenderName` and `SPhone1` are identical to an agent-created row, and **no column in `InternalEx` mentions the employee at all**. Who actually typed it lives beside the ledger in `transfer_attributions`, never inside it.

Two orderings in `EmployeeController::createTransfer` are correctness, not taste. The **duplicate-key lookup comes before the minute rule** — reversed, a double-tap answers "wait a minute" about the transfer that had just succeeded, so the employee sends it again. And the minute-rule check is **read-only and predictive**: the server's own rule lives in a trigger, and the app-side check exists only to fail early with a sentence the employee can act on.

**Deleting an employee is allowed only while they have no financial footprint** (owner's rule, 8 Sep 2026). The app had suspend-only, and suspend keeps the phone number reserved — so an agent who typed a wrong number had no way to correct it, which is exactly how this surfaced. `DELETE employees/{id}` now refuses with 422 and a breakdown when the employee has any of: a row in `transfer_attributions`, a cashbox entry, a shift, or an approval request that actually executed. Sessions, devices, codes, permissions and chats are **not** a footprint — they are setup, not money.

That rule also settled a question the first version left open. The delete was soft, to preserve who created each transfer. But an employee with no transfers has nothing to preserve, so softness was protecting **nothing** while holding a phone number hostage. The delete is now hard — precisely because it is only permitted when there is nothing to destroy. The guarantee moved from "hide, never erase" to "erase only what has no trace", which is the stronger of the two: the first still permits erasing everything.

### Employee transfer limits and agent approval (8 Sep 2026)

The agent sets a per-employee ceiling; anything above it becomes an approval request instead of a transfer. Owner's architecture, stated as the closing line of the spec: *employee creates the request · policy engine evaluates it · agent authorizes exceptions · **the existing transfer engine alone executes it** · Rhalla keeps accounting with the agent.*

**The pending request is not an unexecuted transfer — it is not a transfer at all.** No `InternalEx` row, no balance moved, no commission computed, no entry written. That is what makes rejection safe: nothing happened that needs undoing. `tests/manual/employee_limits_acceptance.php` (44 checks) and `employee_limits_http_acceptance.php` (21, over real HTTP) pin it, both ending in a financial-invariant snapshot.

Three escalation reasons, and a single request carries all that apply: the per-transfer ceiling, an optional cumulative ceiling over a window the agent picks, and repeat transfers to the same recipient.

Five decisions that are not incidental:

1. **No employee has a ceiling until the agent gives them one — and that includes the repeat-recipient watch.** The policy table starts empty and an absent row means *nothing is enforced*. The first version defaulted the recipient window to 60 minutes even with no row, and the **existing** create-transfer suite caught it: every employee working today would have been escalated on their second transfer to the same recipient, under a rule nobody asked for. A feature that stops current work on the day it ships is the one failure this whole design exists to avoid.
2. **Escalate, never reject.** A rejected entry makes the employee re-type the transfer, and an employee who re-types learns to split the amount himself — which is exactly what the policy is for. The request is kept verbatim and re-submitted to the agent's own path on approval; the employee re-enters nothing.
3. **A phone number escalates on its own; a name never does.** The number is a strong independent indicator. A name is not — «محمد علي» belongs to thousands, and blocking a transfer because a name resembles another stops legitimate work daily until the agent learns to ignore the requests. So the name is consulted only when there is no number at all, and normalization unifies *spelling* (hamzas, ة/ه, ى/ي, diacritics) without ever merging two different names: «محمد علي» and «محمدعلي» stay distinct.
4. **The employee cannot approve his own request because he cannot reach the door**, not because a condition forbids it. Approval lives behind `auth:sanctum`; employee sessions are a separate table behind a separate middleware and issue no Sanctum token. Conditions get forgotten when a new route is added; separate doors do not.
5. **Approval covers the escalation reason and nothing else.** It does not raise the employee's ceiling, does not become a precedent, and does not bypass balance, the minute rule, or any central limit — so `EmployeeApprovalExecutor` re-runs every check (employee still active, permission still granted, agent unchanged, minute rule) before handing the request to `InternalExchange`. Hence the `FAILED` state: *approved, and did not go through*. Without it such a request would read as executed.

Two guards are the database, not code: `UX_emp_appr_key` on `(employee_id, client_id)` makes a double tap reuse the first request rather than open a second, and every decision is a conditional `UPDATE` carrying `status = PENDING` **and** `agent_id` — so two simultaneous decisions cannot conflict, and another agent cannot touch a row that is not his (verified live: 404, not 403, so the request's existence is not disclosed).

`transfer_attributions` gained `recipient_phone` and `recipient_name_norm` rather than a third table: it has always been the operational record of *who moved their hand*, and "did this employee send to this number an hour ago?" is the same question. It is an operational table — no balance, no entry, no commission — so extending it touches no financial logic.

**The 24-hour approval expiry is a visible starting point, not a hidden constant.** §27 forbids leaving a request valid forever and §49 forbids inventing values, so it is a per-employee field the agent sees and changes. Expiry is applied on read, not by a scheduler: this project has no queue worker running permanently, and a stopped scheduled job would show a request as "awaiting approval" a week after it died.

### Employee activation by QR (8 Sep 2026)

The agent shows a QR; the employee scans it instead of typing an eight-character code. **The QR replaces the typing and nothing else** — the owner was explicit about why: «جعل QR وحده يفعّل الجهاز سيكون مريحاً، لكنه يضعف الحماية إذا صُوّر الرمز أو وصل لشخص آخر. التصميم الأقوى: QR + تحقق الخادم + OTP الموظف + ربط الجهاز». So after the scan the OTP still goes to the employee's own WhatsApp, and the device is still bound. `tests/manual/employee_qr_activation_acceptance.php` pins it — 37 checks.

**The QR is not a second activation request; it is a second representation of the same one.** `17_activation_qr.sql` adds four columns to `employee_activation_codes` rather than creating a table, so the token inherits the code's entire lifecycle: one status, one expiry, one revocation, one consumption. A parallel table would have meant a revoked code with a QR that still works. For the same reason `requestOtpByQr` does not re-implement anything — it translates the token into a row, then hands off to `requestOtp` with one flag that skips the `Hash::check` and nothing else.

Four things in there that look incidental and are not:

1. **The token is pinned to its own row (`$pinnedCodeId`).** `requestOtp` finds the code by phone and takes the *newest*; without the pin, a QR the agent revoked yesterday works today because a newer row exists for that employee — i.e. revocation silently stops revoking. The manual path never had this hole because `Hash::check` runs against whichever row was found. **The gap appears only on the path that has no code to compare**, which is why it has to be closed explicitly.
2. **Concurrency is decided by a conditional `UPDATE`, not an `if`.** Two devices scanning the same instant both read `scan_device_hash = NULL`, so both pass any in-memory check; the update touches one row and the loser gets zero. The predicate is "nobody has scanned it, **or this same device did**" — burning the token on a dropped connection would force the agent to reissue for no reason.
3. **The scanner filters payload shape before the network.** A camera pointed at a shop sees invoice codes and Wi-Fi stickers; forwarding those spends activation attempts and then trips the rate limit, disabling a perfectly good activation. `isEmployeeQrPayload` is a top-level function precisely so `test/qr_payload_test.dart` can test it — and it is **not** a security check: the server re-validates and trusts nothing.
4. **Verification carries `activation_id`, not the phone.** The scan returns a *masked* number, so whoever photographs a QR cannot read an employee's full phone off it — which means the second step has no phone to send. It is still guarded by `device_hash`, so the id alone opens nothing on another device.

The manual code is kept whole, deliberately: a QR needs a working camera, a granted permission and enough light, and any one of those failing must not block activation. Both paths end in the same `verifyOtp`.

**The scanner costs ~7 MB (arm64 release went 23.0 → 29.9 MB) and that is the right trade.** `mobile_scanner` can be pointed at the *unbundled* MLKit, which downloads the barcode model through Play Services and gives most of that back — but the model then has to arrive over the network on first use, and the app is useless without it on a device with no Play Services at all. This is an app for branch staff in Libya whose offline tolerance is already why the fonts are bundled rather than fetched (`google_fonts` was removed for exactly this reason). A scanner that works everywhere beats seven megabytes.

**Adding the scanner broke the release build, and the message did not say so.** `mobile_scanner` pulls MLKit, which enlarges R8's job past what this 8 GB machine had left: `assembleRelease` died with «Gradle build daemon disappeared unexpectedly», and the crash log named the real cause — `mmap failed to map 140509184 bytes ... G1 virtual space`. Lowering `org.gradle.jvmargs` does not reach it, because **the Kotlin daemon is a second JVM with its own default heap**; `kotlin.daemon.jvmargs` and `org.gradle.workers.max` in `android/gradle.properties` are what fixed it.

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
2. **The first poll of a session never rings.** It establishes the baseline; a backlog that existed before the app opened is announced by the counter, silently. Ringing for it would train the agent to ignore the bell. This was documented here before it was true — `_announced` started empty every session, so the first poll rang for the whole backlog. `_baseline` now implements it, and `test/incoming_alerts_test.dart` (6 tests) pins it along with the rest of the bell's timing.
3. **Opening the invoice is what clears a transfer from the count, not opening the list.** A count that drops because a screen with twenty rows was displayed is a false promise.
4. **The sound is the device's own notification tone** (`RingtoneManager` through the existing `com.rhalla.rhalla_agent/device` channel, `SystemSound.play` as the iOS fallback, haptics on both). No bundled audio file and no audio package: a stranger's tone in a bigger APK, against the one sound the user is already trained to look up at. It respects silent mode, which is correct.

The poll lives in `AutoRefresh` (the shell), not in the home screen — a bell that only rings while you are looking at it is not a bell — and stops when the app is backgrounded. Alerting a closed app is push-notification work, which this is not.

**The drop-down banner** (`incoming_toast.dart`, owner's request 8 Sep 2026) rides the same poll: it slides down from the top of the screen with the ring, carries the company logo in a white circle and «لديك حوالة جديدة», stays five seconds, and lifts. Tapping it opens `/transfers`; swiping up dismisses it early.

- **It is mounted above the `Navigator` in `main.dart`**, beside `AmbientBackground` and for the same reason — the agent may be inside a transfer's details or the statement when one arrives, and a banner under the `Navigator` is covered by the first screen pushed over it.
- **Nothing in it says "agent only" or "after login only".** The *source* cannot count before then: the incoming poll runs from the agent shell alone. A condition written in the widget gets forgotten when a route is added; a source that cannot fire does not.
- **The event is a counter (`IncomingAlerts.ping`), not a flag.** A flag would have to be lowered by whoever displayed it, and a second arrival landing before it was lowered would be swallowed. The counter also survives `markAllSeen`, which resets `unseen` to zero — reset the counter there and opening the list would itself drop a banner.
- **The white circle behind the logo is not decoration.** Company logos arrive as uploaded — mostly dark ink on white — and the banner is a dark gradient; without the disc the logo disappears into it. Same rule as the invoice.
- **There is still no system-tray notification, and adding one would buy nothing today.** The poll stops when the app is backgrounded, so a local notification could only fire while the app is open — exactly when the banner already shows. A notification that reaches a closed app is push (FCM), which this project does not have; the backend's Pusher broadcast is a separate channel the app never subscribes to.

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

### The support centre — `backend/support-app/` (React), served at `/support`

Rhalla's own staff answer agents from a **React SPA built into `backend/public/support/`** and served by Laravel on the same origin — no CORS, no second host, no monthly bill. **`public/support/` is committed on purpose**: the hosting is shared and has no Node, so the built output is what gets uploaded. Change the UI ⇒ `npm run build` ⇒ commit the result with it. Node lives at `D:\tools\nodejs` (portable, on D: by standing order) with its npm cache on D: as well.

**No second chat system was built.** Messages, attachments, reactions, quoting, pinning, stars, search, typing and read receipts all go through **the same `ChatService`** the agent and employee apps use. What is new is only what chat cannot answer: *who are the support staff* (`support_staff`), *who may do what* (`support_permissions`), *where does the conversation stand* (`support_thread_state`), and *who did what* (`support_audit`), plus one nullable column `chat_messages.support_staff_id`.

Five decisions that are not incidental:

1. **`sender_id` stays `0` for every support reply.** `chat_reads` compares `(kind, id)`, so giving each staff member their own id would split read state across them — and the agent would watch their two ticks retreat every time a different person answered. The admin is one party in the agent's eyes; who actually replied goes in `support_staff_id`.
2. **Default Deny in two layers.** `ROLE_CEILING` says what a role *may* hold, granted rows say what it *does* hold. Hiding a button is cosmetic; the refusal is in the middleware on every call — verified live: a plain `SUPPORT_AGENT` gets 403 on staff management, permission granting, the audit log, closing, assigning to others and pinning, and each attempt is logged under their name.
3. **`kind = ADMIN` is enforced in `SupportController::thread()`**, so an agent's conversation with *their own employee* can never be opened from here — verified: 404 on both read and send.
4. **An agent message revives a closed conversation** (`onAgentMessage`, called from `ChatController::send`). Without it a conversation closed yesterday and written into today appears in neither «المفتوحة» nor «المُسنَدة إليّ», and the message is simply lost.
5. **No conversation opens automatically.** The old Blade page opened the first thread on every load, marking an agent's messages read because somebody opened a browser. Blue ticks are a promise.

**⚠ The old `/admin/chat` page is a back door around all of this** and is still live while `CHAT_ADMIN_KEY` has a value: a shared key, no identity, no roles, no audit. Emptying that key in `.env` closes it. The code was left in place because that is the owner's one-line call, not the code's.

Nothing here touches money: zero financial tables referenced in the whole support codebase, 4 foreign keys all pointing at `support_staff`/`chat_threads`, 0 triggers, and `wallet`/`InternalEx`/`AccountsTb` last structurally modified in April 2026 — before this project started. Full detail in [docs/support-center.md](docs/support-center.md), including the three things the brief assumed exist and do not (E2EE, voice calls, WebSocket real-time).

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
