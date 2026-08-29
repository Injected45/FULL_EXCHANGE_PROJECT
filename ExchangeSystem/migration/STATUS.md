# ExchangeSystem → MySQL/MariaDB — migration status

## Fixes from the fourth UI test (2026-08-02)

### 12. `whatsapp_contacts` referenced a table that migrated under a different name

`CALL whatsapp_contacts` raised `Table 'rhalla2026teset.whatsapp_contacts' doesn't exist`. The proc reads a
CROSS-DB WhatsApp-contacts table; SQL Server calls it `rhalla2026Teset.[db_owner].[whatsapp_contacts]`, but on
the MySQL server that DB's table is named **`whatsapp_contacts_shipping`** (the only `whatsapp%` table there,
same schema: `phone_number`, …). The app (`WatsapChick.vb`) swallows the exception and returns False, so the
WhatsApp-contact check was silently ALWAYS false. The proc is `SQL SECURITY DEFINER`, so it reads the cross-DB
table under the definer's (`rhalla_app`) grants even though `exchange_app` has no direct SELECT there.

Fix: repoint the `FROM` table to `whatsapp_contacts_shipping`. Durable via `migration/srcpatch/whatsapp_contacts.sql`
(a future re-sync now emits the right name); production proc updated in place from its live body + EXECUTE
re-grant. Verified as `exchange_app`: `CALL whatsapp_contacts('00')` → empty (no error), a real registered
number → returns its row.



### 11. `SELECT .. INTO` over a multi-row match — ERROR 1172, and the hand-port trap it exposed

External transfers crashed in the price-power check: `Get_CurrencyPower(...)` threw
`MySqlException: Result consisted of more than one row`. T-SQL's `SELECT @x = col FROM ..` silently keeps ONE
row when several match; MySQL's `SELECT col INTO v FROM ..` raises **ERROR 1172** and aborts the routine. The
converter's `FixKnownNonUniqueLookup` only covered 4 hardcoded single-table lookups and **explicitly skipped
JOINs** — and `Get_CurrencyPower` is a 2-table JOIN whose WHERE matched 5 rows (all `CurrencyPower = 0`; a scan
of all 57 param-sets found 0 with a conflicting value, so the row choice is immaterial here).

Converter fix (`FixOneSelectInto`): after the known-list and GROUP BY handling, append a bare `LIMIT 1` to any
remaining `SELECT <non-aggregate> INTO <var> FROM ..` that has no GROUP BY / ORDER BY / LIMIT (guards out
FROM-less selects, top-level UNION, aggregates). This restores SQL Server's "return some row, don't crash"
semantics — it can never make a working lookup wrong (a no-op when the predicate is unique), only stop the
MySQL-only abort. A full scan found it changes **39 functions, 0 procedures**.

**The trap:** regenerating those 39 from the converter is NOT safe — several are hand-ports (e.g.
`SalePrice_mo_Value`, whose prod body has `<=` while a fresh auto-conversion emits an invalid spaced `< =`).
Blindly pushing converter output would REGRESS the hand-ports. So the production fix was applied two ways:
* `Get_CurrencyPower` — a clean auto-conversion (prod body == converter output apart from the LIMIT) — pushed
  from the converter. Verified: `(1,2,3,1,2,23)=0`, `(…,999)=2`.
* the other **38** — fixed SURGICALLY: `scratchpad/surgery.py` takes each function's ACTUAL prod body
  (`SHOW CREATE FUNCTION`) and appends `LIMIT 1` only to the qualifying `SELECT..INTO..FROM` statements
  (146 additions), touching nothing else. Emitted as `migration/push_limit1_functions_20260802.sql`, applied to
  local (all 38 create — `SalePrice_mo_Value` included), then production, with EXECUTE re-grants (§8). Spot-check
  `EXEX_Get_Transprice(1,1,1,1)=0` (was a latent 1172).

Lesson for future pushes: a routine that fails to reproduce from the converter is a hand-port — patch its live
body, do not overwrite it with a fresh conversion.



## Fixes from the third UI test (2026-07-27)

### 9. T-SQL parameter DEFAULTS were silently dropped — a whole CLASS of "saved value vanished"

The "add company / department" screen (`FrmCompanies`) showed an empty parent ("تابع لـ") dropdown and an
empty company listbox. Root cause was NOT that screen: `Companies_Crud` has `@IsActive BIT = 1`, and the form
(like the T-SQL caller) OMITS `@IsActive`. On SQL Server the parameter default made every inserted company
`IsActive = 1`; MySQL has **no proc-parameter defaults**, so the omitted arg arrives as NULL
(`EnsureAllProcParams` fills it), the row inserts `IsActive = NULL`, and then vanishes from every
`WHERE IsActive = 1` read. The single existing production company already carried `IsActive = NULL`.

This is general, not one proc. A scan of every T-SQL proc header for non-NULL parameter defaults found **6**:
`Companies_Crud (@IsActive=1)`, `EMPLOYEETB_Insert (@ContractID=0)`,
`EMPORCUSTWITHDRAWALTB_Insert (@OPType=38)` — one of the 3 ERROR_PROC-masked failures —
`AccSafeActivityTb_Statment (@BaseType=0)`, `SalaryCalculation_LoadToBankPortfolio (@BankID=0,@BBranchID=0)`,
and the scheduler helper `gather_statistics` (unmigrated). `sys.parameters.has_default_value` is USELESS here
(it is only set for CLR params), so defaults must be parsed from the definition text.

Fix is in the converter (`ParseNonNullParamDefaults` + injection in `ConvertProcDdl`): for each param with a
non-NULL T-SQL default, prepend `SET @x = ISNULL(@x, <default>)` to the RAW T-SQL body. Riding the normal
pipeline gets `ISNULL`→`IFNULL`, `@x`→`p_x`, and DECLARE-hoisting for free, so emitted order stays legal
(DECLAREs, handler, then these SETs, then the body). NULL defaults need nothing — the omitted path already
yields NULL. Applied + functionally verified on LOCAL (insert with no @IsActive → row is IsActive=1, listed).

Production: the existing company's `IsActive` was repaired to 1 via `exchange_app` (authorized MySQL data
edit; faithful — SQL Server's default would have made it 1), so the screen works NOW. Until the proc push,
NEW company/department inserts on production still land `IsActive = NULL`.

### 10. Leading `BEGIN…END` grouping block dropped the rest of the body — no auto account number

The "دليل الحسابات / add account" screen (`FrmAccountsTree`) stopped auto-filling the account number. It comes
from `ACCOUNTSTB_selectmax`, which on production returned an EMPTY result set (SQL Server returns
`code, accLine, IDcode`, e.g. `(0,0) → 5|0|5`). Cause: the T-SQL wraps its `DECLARE`s + first assignment in a
**leading bare `BEGIN…END` grouping block**, then continues with `IF` branches and the final `SELECT`. T-SQL
`BEGIN…END` is pure grouping (no variable scope); MySQL's is a scope. The converter's wrapper saw the body
already started with `BEGIN` and treated that inner block as the WHOLE proc body — everything after its `END`
(the IF branches + the returning SELECT) fell outside the compound and was lost at CREATE, and the vars would
have been mis-scoped regardless. Stored MySQL body was truncated to just the DECLAREs + one SET.

Fixed in the converter (`FlattenLeadingGroupingBegin`, called at the top of `ConvertProcDdl`): if a bare
`BEGIN` (not `BEGIN TRAN`/`TRY`) leads the body and real statements follow its matching `END` (CASE-aware
balance), drop that BEGIN/END pair so the DECLAREs hoist to proc scope and the trailing statements stay in the
body. Fires ONLY when content follows — the common `AS BEGIN <all> END` procs are byte-for-byte untouched. A
DB-wide scan confirms it changes exactly **2** procs: `ACCOUNTSTB_selectmax` (fixed, now byte-matches SQL
Server for (0,0)=5, (1,1)=101, (10,1)=1001) and `activate_oqs_scheduler` (an unmigrated scheduler — moot).

Applied + verified on local. Same production-push blocker as §9.

### Production push — DONE (2026-07-27)

`migration/push_ui_fixes_20260727.sql` (both §9's 5 param-default procs and §10's `ACCOUNTSTB_selectmax`, the
per-object EXECUTE re-grants §8, and the idempotent `Companies` data repair) was applied to production as
`rhalla_app` and verified as `exchange_app`: `ACCOUNTSTB_selectmax` returns `(0,0)=5, (1,1)=101, (10,1)=1001`
(byte-matches SQL Server), `Companies_Crud` insert without `@IsActive` lands `IsActive=1`, and all 6 procs are
executable by `exchange_app` (grants restored, no "denied"). A pre-push backup of the 6 prior definitions was
taken first.

Note on access: the earlier "denied from this IP" was the **password**, not the host — `rhalla_app` is
`rhalla_app@'%'`. The superuser password is NOT stored anywhere in the repo or a persistent cnf; the throwaway
admin cnf was deleted after the push.

## Fixes from the second UI test (2026-07-25)

### 7. `SELECT alias = expr` — a RUNTIME-only bug the pipeline could never catch

`InternalEx_LOADTOCONFIRM` failed with `Unknown column 'CANCELSTATUS' in 'field list'`. The T-SQL used the
`SELECT CANCELSTATUS = 'غير معتمدة'` alias idiom, and the converter's flip rule (`name = expr` -> `expr AS
name`) was **retry-only** — it ran only when a proc FAILED to create. But `SELECT x = 'y'` is VALID MySQL
*syntax* (a boolean comparison), so the proc created fine and the retry never fired; it failed only at RUN
TIME. A create-time-only check can never catch this class.

The fix had to run unconditionally, but a naive whole-body regex flipped **UPDATE..SET** assignments too
(`SET col = expr, col = expr` is the same shape) and broke 60+ procs. So it is now a scoped walker,
`FixSelectListAliasAssign`, that rewrites only the span between a top-level SELECT and its own FROM — never an
UPDATE SET, an INSERT, or a `SELECT..INTO` with no table. Two follow-on bugs were found and fixed while
tightening it: a SELECT with no FROM (`SELECT LAST_INSERT_ID()`) let the FROM-search run past a `;` into a
later UPDATE (fixed by stopping at statement boundaries), and a `CASE..END` in the list tripped that boundary
check (fixed by skipping CASE..END while scanning). Result: `CANCELSTATUS` flips, `ExBanksTb_CRUD` /
`ExBBranchTb_CRUD` UPDATE SETs stay intact, 911 procs, audit ALL CLEAN.

### 8. GRANT-loss on every routine re-sync — OPERATIONAL TRAP

`AccSafeActivityTb_SelectByEmSafe` failed on production with `execute command denied to user
'exchange_app'@'%'`. Cause: `exchange_app` holds **per-object** EXECUTE grants (the DB-level grant is
unavailable — `mysql.db` is corrupt, see below), and **`DROP PROCEDURE` deletes the grant with the object**.
A mysqldump-based routine push does `DROP PROCEDURE IF EXISTS` + `CREATE` for every routine, so a full push
**wipes all 1041 EXECUTE grants** — measured, not guessed. After this push, 1041/1041 were missing until
re-granted.

> **RULE: every routine push to production MUST be followed by the EXECUTE re-grant.** The generator query
> (routines the app user cannot currently execute) is in scratch as `regrant2.sql`'s source and is trivial to
> re-run:
> ```sql
> SELECT CONCAT('GRANT EXECUTE ON ', IF(ROUTINE_TYPE='PROCEDURE','PROCEDURE','FUNCTION'),
>               ' `',ROUTINE_SCHEMA,'`.`',ROUTINE_NAME,'` TO ''exchange_app''@''%'';')
>   FROM information_schema.ROUTINES r
>  WHERE r.ROUTINE_SCHEMA IN ('EXCHANGESYS2026','ExSyAccounts2026','ExSyAccountsCurrency2026')
>    AND NOT EXISTS (SELECT 1 FROM mysql.procs_priv p WHERE p.User='exchange_app'
>                    AND p.Db=r.ROUTINE_SCHEMA AND p.Routine_name=r.ROUTINE_NAME
>                    AND FIND_IN_SET('Execute', p.Proc_priv));
> ```
> Run it, execute the output, `FLUSH PRIVILEGES`. This trap would not exist if `mysql.db` were repaired and a
> single database-level `GRANT EXECUTE ON schema.*` could be used instead of 1041 per-object grants.

After this fix: local == production, all 1041 routine bodies byte-identical (aggregate MD5
`af7494aa8cacfb689db32cdc9cf0c477`), and exchange_app holds all 1041 EXECUTE grants again.

### Still-open items from this round (not port bugs)

* **`CurrencyMainTb_Insert` / `EMPORCUSTWITHDRAWALTB_Insert` / `UserAccessProfileTemplate_ID_inser_for_update`**
  logged the generic `ERROR_PROC` signal ("error"). The underlying cause is masked by the CATCH handler and
  needs a per-proc repro to find. Not yet root-caused.
* **`Unable to connect to any of the specified MySQL hosts` / `Connect Timeout expired`** on a few calls are
  network blips to the remote server, not port bugs.
* The `ConnectionString غير مهيأ` on one `CurrencyMainTb_Insert` call is a transient (config not yet loaded on
  that code path); watch for recurrence.

## Fixes from the first real UI test (2026-07-23)

Two distinct failures came back from the team, both in the porting layer rather than in a stored procedure.

### 1. NullReferenceException in `MD_MYSQL.CopyOut` — many lookup screens

`ProfileName_fill`, `CurrencyMainTb_LOADTOLKP_Dl` and every other screen that fills a LookUpEdit.

`CopyOut` copies OUT/INOUT values back into the caller's `SqlParameter` array, and it assumed the array was
non-Null. Dozens of screens reach the data layer through `LoadToControlar(...)`, which passes its optional
parameter array straight through as **`Nothing`** — the SQL Server path explicitly tests
`If PRM Is Nothing` in `Module1.RUN_QUARY_PRO_alter`, so those screens always worked there and only broke
once routed through MySQL. Every sibling helper (`StageTvps`, `AddParams`, `LogMyError`) already had the
guard; `CopyOut` was the one that did not.

Fixed, and two further latent faults in the same six lines were fixed with it:

* an individual `prm(i)` may be `Nothing` — the legacy style is `Dim PRM(43) As SqlParameter` with only some
  slots filled;
* it matched `cmd.Parameters` **by index**. `EnsureAllProcParams` appends parameters the caller omitted and
  `StageTvps` removes TVP parameters, so the two lists can diverge — and when they did, an OUTPUT parameter
  would silently receive the **wrong value**. Matching is now by name (accepting the `p_` prefix that
  `ToMy()` adds).

### 2. Command timeout on `AccSafeActivityTb_SelectByEmSafe`

Not a network or connector problem: it hung when run directly too. It reads
`View_AccSafeActiv_SafeLoad`, whose select list contains a **window function** and a **per-row scalar
function**. Either one blocks derived-table merging in MariaDB, so the view was **materialised in full** —
488,749 rows, with `SafeIDEMP_GetNetTotal(...)` evaluated once per row — before the caller's WHERE was
applied, to return a handful of rows. SQL Server merges the view and never pays that cost, and MariaDB 10.6+
could push the condition down, but 10.4 cannot.

The proc uses **neither** expensive column — it computes its own `ROW_NUMBER()` and its own running balance,
and three of its four references to the view only aggregate `Debit`/`Credit`. So it now reads a derived table
over the base table exposing the same columns under the same Arabic aliases. Proven equivalent, not assumed:
same predicates, **3052 rows and identical MD5** from the derived table and from the base table.

**911 ms locally, 1417 ms on production — previously a client timeout, with the abandoned query still
running 19 minutes later.**

The view itself also carried a `GROUP BY` whose key includes `a.ID`, the base table's PRIMARY KEY, making it
a no-op (measured: 506,189 base rows, 506,189 distinct grouping keys). It is removed in
`handport_view_AccSafeActiv_SafeLoad_perf.sql` so the view is cheap for any future consumer; this proc was
its only one.

### 3. `FileLoadException` on DiagnosticSource — caused by the rebuild itself

Once the new build was deployed, **every** query failed at `MySqlConnection.Open()`:

```
Could not load file or assembly 'System.Diagnostics.DiagnosticSource, Version=7.0.0.2 ...'
The located assembly's manifest definition does not match the assembly reference.
```

Same class as the three missing transitive dependencies already documented below, and it takes down device
activation too — so the app reports the machine as unlicensed even though nothing is wrong with it.

Cause: MySqlConnector 2.3.7 is compiled against **7.0.0.2**, but the project's `Reference` carries
`SpecificVersion=False`, so MSBuild is free to resolve the **9.0.0.0** copy that DevExpress pulls in — and
does. It had been deploying 7.0.0.2 until a rebuild changed the resolution. There was no binding redirect for
this assembly, unlike `System.Text.Json` and `Microsoft.Bcl.AsyncInterfaces`, which are already redirected to
9.0.0.0 in the same file.

Fixed by adding the missing redirect to `App.config`:

```xml
<assemblyIdentity name="System.Diagnostics.DiagnosticSource" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
<bindingRedirect oldVersion="0.0.0.0-9.0.0.0" newVersion="9.0.0.0" />
```

Verified deployed: the redirect is present in `bin\Debug\ExchangeSystem.exe.config`, the shipped DLL is
9.0.0.0, and the public key token matches the one in the error.

**Lesson: after ANY rebuild, check which version of these four assemblies landed in `bin\Debug`.**
`SpecificVersion=False` on a Reference means the deployed file can change without the project changing.

### 4. The log now captures EVERY error, not just data-layer ones

`LogMyError` only ever saw exceptions raised INSIDE the data layer. A failure in a form — reported as
*"Conversion from type 'DBNull' to type 'String' is not valid"* — happens after the query has returned, so
nothing was written to `mysql_errors.log` and there was nothing to send to support but a screenshot.

Two additions:

* `InstallGlobalErrorLog()` hooks `Application.ThreadException` and `AppDomain.UnhandledException`, so any
  unhandled exception anywhere is appended in the same block format, with the **active form name** — a
  DBNull-conversion error otherwise gives no clue which screen produced it. It is wired from
  `EnsureTargetLoaded()`, which runs on first data access (earlier than any form's Load) and is idempotent.
* The 5 places that catch an exception and show the generic support dialog now call `LogAppError` first.
  Those are invisible to `ThreadException` precisely because they are caught — and the screenshot showed
  exactly that dialog.

### 5. The DBNull crash itself — pre-existing, not a migration defect

`Currency_settingForBancksRet_getUESER` returns `UESERinsert` with **no** `IFNULL`, unlike the two sibling
columns which have one. It is populated by `SELECT b.UName INTO v_UESERinsert .. WHERE a.ID = p_ID`, so it
stays NULL whenever the record is missing — and `Currency_settingForBancksRet` is currently **empty**. The
form then assigns that straight into `.Text` and the screen dies.

**The T-SQL original does exactly the same** (verified against the source), so SQL Server fails here too;
the proc is a faithful port and was left alone so the result set still matches. The fix is in the form,
via a new shared `NullSafeText()` helper — worth sharing rather than patching one line, because any column
that was never NULL on SQL Server can be NULL here when a lookup finds no row, and every screen that writes
a query result into `.Text` has the same exposure.

### 6. "the phone isn't saved when adding a user" — it is; the real fault is a DUPLICATE LOGIN

Investigated 2026-07-23. **Adding a user saves the phone correctly.** Evidence, not assumption:

* users `USID 72..83` on production all have a phone stored;
* `USID 83` was created **today, on production**, with `Phone = 201130572182` — it saved;
* calling `TB_Users_Insert` directly with a phone stores it (`Phone=[201130572182]`), and the proc handles
  the column on BOTH branches — the INSERT lists 13 columns / 13 values with `p_Phone` last, and the
  `IsUpdate = 1` branch has `Phone = p_Phone` in its SET list;
* the form passes `phone.Text` on both save paths and reloads it on open.

The two accounts with no phone, `USID 70` and `USID 71 (تجريب)`, are **already in the 2026-07-08 snapshot**
with `Phone = NULL` — they were created on the original SQL Server system and never had one.

**The actual problem: `UNameLog = '1'` / `UPass = '1'` matches TWO accounts.**

| USID | UName | Branch | Phone (before) |
|---:|---|---|---|
| 71 | تجريب | 49 — فرع سرت | NULL |
| 83 | محمد | 1 — فرع البيضاء | 201130572182 |

`User_TB_CHECKACOUNT('1','1')` returns both rows and `FrmLogin` takes `dtloguser.Rows(0)` — so logging in as
`1`/`1` always lands on **USID 71**, whose phone was empty. That is why a phone that had been saved on 83
looked missing.

> This is more than a cosmetic problem: the two accounts are in **different branches**, so which branch's
> money the session operates on depends on row order. It should be resolved by changing one account's login
> — a data decision for the owner, so it has NOT been done here.

Phone set to `201130572182` on `USID 1` and `USID 71` on production as requested (83 already had it).
> Two abandoned calls were still burning CPU on production 2.7 hours later and blocking DDL on the view.
> They were verified read-only (`trx_rows_modified = 0`) before being killed. Worth checking
> `information_schema.PROCESSLIST` after any timeout report.

## Coverage: 909 / 916 stored procedures (99.2%), 127 / 127 functions, all 13 audits CLEAN

`EXCHANGESYS2026` holds **911** procedures: the 909 ported ones plus two cross-database shims
(`ExSyAccounts_AccSafeActivityTb_Insert`, `ExSyAccountsCurrency_AccSafeActivityTb_Inert`) that stand in for
SQL Server's cross-DB `ExSyAccounts2026.dbo.…` calls.

### The 7 that are NOT ported — and why none of them can be

| Proc | Why | Called by the app? |
|---|---|---|
| `activate_oqs_scheduler`, `gather_statistics`, `start_scheduler`, `stop_scheduler` | SQL Server **Open Query Store** diagnostics (Service Broker `conversation_handle`, `sys.…` internals). Not application logic and there is no MySQL equivalent. | no — 0 references |
| `ASSMember_Statment` | Reads `ExAssociationAct.dbo.AssActivityTb` / `.MPAMNTWDMEMBERTB`. **That database does not exist**, so the proc already fails on SQL Server: `Msg 208 — Invalid object name 'ExAssociationAct.dbo.AssActivityTb'`. | yes (association-statement screen) |
| `DriversTb_Add_From` | Reads synonym `GEt_DriversTb` → `[ShippingTransportSystem2024Teset].[dbo].[DriversTb]`. That database does not exist either: `Msg 5313 — Synonym 'GEt_DriversTb' refers to an invalid object`. | yes (mobile add-driver) |
| `CustomersTb_insert_Mobile` | The stored T-SQL contains a bare `returns` statement (not `return`) inside the body. | yes (mobile add-customer) |

**The last three are already broken in production today** — verified by executing them against SQL Server, not
inferred. Porting them is impossible without the two missing databases, and a faithful port would reproduce the
same failure. They need a decision from the vendor: restore `ExAssociationAct` and
`ShippingTransportSystem2024Teset`, or retire the three screens.

## Push to 100% — converter fixes found along the way

Each of these was found by chasing a *specific* failing proc and turned out to be a whole class:

* **`MAXVALUE` is a MariaDB reserved word** (partition DDL) used as a plain column name → every proc selecting
  it failed with a bare syntax error. The full set of reserved words used as columns was then found
  EMPIRICALLY — asking the server `SELECT 1 AS <name>` for all 982 distinct column names rather than trusting
  a remembered list: `Order`, `key`, `keys`, `long`, `MaxValue`. Only `keys`/`first_value`/`MaxValue` are
  backticked unconditionally; `Order`/`key`/`long` are deliberately NOT, because a blanket rewrite would
  corrupt `ORDER BY` / `PRIMARY KEY` / `LONG`. **+3 procs**
* **`ORDER BY` after a top-level `UNION` may not use table qualifiers** in MySQL (error 1250,
  "Table 'x' from one of the SELECTs cannot be used in ORDER clause"). The qualified name is always also an
  output alias, so the qualifier is stripped — only for ORDER BYs at paren-depth 0 after a depth-0 UNION, so
  subquery ORDER BYs are untouched. **+2 procs**
* **Parameterized TVFs inlined as derived tables** — exact for any parameter, unlike the view+caller-filter
  trick. Applied RETRY-ONLY (the same convention as `applyConcat`): inlining a TVF can be far slower than the
  TVF was on SQL Server, and applying it to procs that already converted made the diff-test phase hang on a
  heavy query. **+4 procs**
* **`sp_executesql` procs are SKIPPED by the migrator entirely** — they appear in NO fail list, so they look
  like they never existed. Two are not really dynamic (a constant string) and became plain statements; the
  genuinely dynamic ones become `PREPARE`/`EXECUTE` over a `CONCAT`-built string.
* **Comment stripping was not string-aware** — the worst bug in this batch. `Regex.Replace(@"--[^\n]*", "")`
  also ate the *inside* of a literal, and these procs use `'---'` as a placeholder:
  `ELSE '---' END SalePriceType` became `ELSE '` — truncating the literal and swallowing the rest of the CASE.
  It surfaced as a syntax error in `NewCurrencyBuyandSale_CRUD`, but a literal that merely came out *shorter*
  would have been corrupted **silently**. Replaced with `StripSqlComments()`, which skips string literals and
  bracketed identifiers and handles nested `/* */`. **Every routine was re-converted after this fix.**
* **Line endings are now normalised to `\n` before anything else runs.** `sys.sql_modules` stores whatever the
  author saved: `SendTypeTB_Roll_luckbedit` is stored **CR-only**, so every line-anchored rule saw its whole
  body as one line and it could not be converted at all. Other rules joined lines on `\n` and left a stray
  `\r` behind, which later showed up as a phantom one-byte difference between local and production
  (`GetCostOf_Currency`). T-SQL treats CR/LF/CRLF alike, so collapsing them changes nothing and makes every
  later rule reliable. **+1 proc, and local/production now compare byte-for-byte.**
* **`SELECT @v = expr … GROUP BY k` is multi-row** — T-SQL assigns each row in turn and the LAST wins; MySQL
  raises `ERROR 1172 Result consisted of more than one row` and *aborts the routine*. This broke
  `EMP_GetADVPMNTTtoalsIndivdual`, which `VW_SALARYCALCLAST` depends on, so the whole salary screen died.
  Which row SQL Server leaves behind was **measured, not assumed** — two independent cases both land on the
  last group in ascending key order — so the converter now appends `ORDER BY <keys> DESC LIMIT 1`, but only
  when the `GROUP BY` is at the statement's own top level (never one inside a derived table).
* **Table variables, `WHILE … BEGIN`, multi-line `DECLARE` lists, `IDENTITY(1,1)`, and leading-comma
  multi-target assignment** were all unsupported and are now handled generally rather than per-proc.

### `migration/srcpatch/` — minimal T-SQL patches

Three procs use a braceless `IF` whose body is a multi-line statement. A regex cannot find where such a body
ends (telling an `UPDATE`'s own `SET` clause from the start of the next statement needs the `;`-pass state
machine — the attempt truncated `UPDATE t SET …` to `UPDATE t` and was reverted). Rather than hand-transcribe
300–1200 line money procs, `srcpatch/<Proc>.sql` holds the **T-SQL** with only the omitted `BEGIN`/`END` added,
and the same proven converter and audit harness do the rest. A patch must stay semantically identical to what
SQL Server runs; behaviour changes belong in `proof/` instead.

A checker false positive was also fixed: `canon()`'s CONVERT pattern was not paren-aware, so in
`ROUND((CONVERT(DECIMAL(12,0), @S/30) * @D) / 5.0), 0)` it ran past CONVERT's own closing paren, grabbed
ROUND's `, 0)` and mis-read a 2-arg CONVERT as a styled 3-arg one — reporting a phantom write-path difference
against the perfectly correct `CAST(.. AS DECIMAL(12,0))`.

## Transfers now migrated — but they MUST be live-tested

`ExternalEx_Insert` and `InternalEx_Insert1` (the external/internal transfer write procs) are live on BOTH
local and production. They required rewriting T-SQL `GOTO`, which MySQL does not have.

The jumps were genuinely unstructured — each leaves its branch and lands *inside a later branch*. All five
were rewritten with the same mechanical pattern, after asserting every anchor against the converter output
and computing block nesting with `CASE..END` accounted for:

```
GOTO X          ->  SET v_goto_X = 1;              (then fall out of the branch)
IF <guard>      ->  IF (<guard> OR v_goto_X = 1)   so the flag can enter that branch
<skipped body>  ->  wrapped in IF v_goto_X = 0 THEN .. END IF;   (omitted when empty)
X:              ->  removed
```

Also found: **the converter does not treat `GOTO` as a statement boundary**, so the statement immediately
before a jump (`SET @ID = 0`) was emitted without its `;`. Two such statements needed terminating.

### LIVE-TESTED 2026-07-21 — and it found two real money defects

`InternalEx_Insert1` was exercised end to end on local MySQL by **replaying a real historical transfer** and
comparing the ledger rows it wrote against the ones SQL Server actually wrote for that same transfer. (Replay
is the only cross-engine test available: writing to SQL Server is forbidden, but its historical output is
already in the migrated data.) Two defects turned up that **no static check could have caught** — both were
control-flow, not column lists, and `cmp_writes.py` was green throughout:

1. **A plain save also posted the delivery-side money.** The rewrite widened the guard of the block the GOTO
   *jumps from* (`IF @ConfirmType = 0`) instead of the block that *contains the label* (`IF @ConfirmType = 1`).
   That left the whole confirm body nested inside the save block, so saving an unconfirmed transfer
   immediately credited the delivering branch — a double post.
2. **Delivery-on-confirm posted nothing.** `GOTO Delivered` fires inside the ConfirmType = 1 block but its
   label lives inside `IF @ConfirmType = 2`, and that outer gate had not been widened — so the jump could
   never enter the block and the delivery rows were silently never written.

**Rule that came out of it:** for `GOTO X`, every gate between the procedure body and the label must accept
the flag, and each gate's preamble must be skipped — not just the innermost one. A gate may be left alone only
when the jump provably cannot fire unless that gate is already true (this is why `ExternalEx_Insert`'s inner
`IF @IsInOrOut = 1` is deliberately untouched — the GOTO sits inside that very block).

After the fix, replaying save → confirm → deliver produced **all 10 ledger rows byte-identical to SQL Server**
— same accounts, amounts, currency, branch, TypeID, OperationTypeID and SafeID — and balanced
(SUM(Debit) = SUM(Credit) = 1,300.000). Test rows were deleted afterwards; both procs are byte-identical on
production.

### A third defect, found by widening the replay to every variant

`replay_internal_transfer.sh <Code>` automates the whole thing: it reads a historical transfer's own
parameters, recovers which user performed each step from the ledger rows, replays save → confirm → deliver
under a `TESTR-` code, diffs against the historical rows and deletes everything it wrote.

Running it across the cash / account-from / account-to / agent variants exposed:

* **`ERROR 1172` again — this time with no `GROUP BY`.** `BranchRatesTb` is not unique on
  `(FBranchID, SBranchID)`: 46 pairs have two rows and for 33 of them the rates DIFFER. So
  `SELECT a.FBRate INTO v_FIRSTBRRATE FROM BranchRatesTb a WHERE ...` returned two rows and **every agent
  transfer aborted outright**. The converter's multi-row rule only fires on a top-level `GROUP BY`, so it
  could not have caught this — the predicate is simply not unique in the data.
  Measured against SQL Server on the three discriminating pairs (38,2), (38,14), (38,39): it keeps the
  **highest-ID** row, matching the same "last row wins" rule measured twice before. Fixed with
  `ORDER BY a.ID DESC LIMIT 1` on all six rate lookups (2 in InternalEx_Insert1, 4 in ExternalEx_Insert).

**Result across the variants — every one byte-identical to SQL Server:** cash (10 rows), account-from (10),
account-to (10), agent (10), agent with commission (14), confirm-only (4), plus a 12-transfer sweep at
17/14/10/4 rows each.

One replay could not match and it is **not a port defect**: `118491-75-32406`'s historical ledger splits its
commission 1.250 / 3.750, but `BranchRatesTb` now holds a single row each way for that pair giving 50 / 0 —
and **both engines read those same rates today** (verified read-only against SQL Server). The rates were
edited after that transfer was posted; the transfer's own recorded `BdShare = 2.500` already disagrees with
its own ledger rows. A replay cannot reproduce numbers computed from data that no longer exists.

### The risky pattern is `SELECT .. INTO` over a NON-UNIQUE predicate — now an audit gate

Two of the three defects were this. T-SQL silently keeps the last row; MySQL raises 1172 and aborts. It is
invisible to every code-reading check because whether it fires depends on the *data*.

So it is now checked against the data. `find_multirow_select_into.py` extracts every single-table
`SELECT .. INTO` and asks the server whether its WHERE columns are actually unique:

```
SELECT 1 FROM <table> [WHERE <literal preds>] GROUP BY <parameter cols> HAVING COUNT(*) > 1
```

Two refinements matter, and without them the report is useless noise (it started at 827 hits):

* **literal predicates must be applied, not grouped on.** `WHERE IsMain = 1` is unique, but `GROUP BY IsMain`
  "finds duplicates" among the `IsMain = 0` rows. Literals become a WHERE; only parameter columns are grouped.
* **an aggregate with no GROUP BY can never return two rows.** `SELECT SUM(x) INTO v ..` is always one row
  however duplicated the key is.

That left **8 real findings in 4 routines**, every one verified against the data before being touched:

| Routine | Lookup | Reality |
|---|---|---|
| `CoBranch_Insert` (×3), `CountriesTb_Insert` | `CurrencyMainTb WHERE IsDefault = 0` | **8 rows** — these failed on EVERY call |
| `AdvancePaymentTb_ADPMNTTOUPDATE` (×2) | `AdvancePaymentTb WHERE EMPID = ?` | employee 38 has two advances |
| `BranchRatesTempTb_Truncate` (×2) | `BranchRatesTb WHERE FBranchID/SBranchID = ?` | 51 branches duplicated |
| `TB_Users_Insert` | `TB_Users WHERE EMPID = ?` | 35 users share `EMPID = 0` |

All fixed by the converter (`FixKnownNonUniqueLookup`) rather than by hand-porting a 287 KB proc. Which row
SQL Server keeps was **measured six times** — always the highest-ID row — so `ORDER BY <pk> DESC LIMIT 1`
reproduces it and is a no-op wherever the predicate happens to be unique.

**Audit check 12** re-runs the probe, so a data reload that introduces new duplicates is caught here instead
of by a user hitting a dead screen. Blind spots are reported as counts, not hidden: 481 lookups are not
analysed (join / subquery / non-equality) and are listable with `--show-skipped`.

### `ExternalEx_Insert` — save path replayed, confirm path NOT verified

`replay_external_transfer.sh` does the same job for external transfers, and the result is deliberately stated
as a partial one:

* **The save step reproduces SQL Server exactly** — same accounts, amounts, branch, TypeID/OperationTypeID —
  on every transfer tried.
* **The confirm step could not be replayed.** It consistently writes one row fewer than history (the
  `@CurrentDelAccID -> @CurrentAccID` posting at T-SQL line 1561), because that posting's amount `@NewPrice1`
  comes from the agent-pricing chain `@OurAccID -> NewCurrencyPriceOwnDetailsTb -> @CurrPower -> @OldPrice1`,
  and those inputs are **not recoverable from the stored row** — `OurAccID` is 0 on the historical rows while
  the lookup is keyed on it, so the pricing lookup finds nothing and the code takes a different branch than it
  did when the transfer was really made.

**This is a gap in the harness, not a known defect.** What was checked instead, directly against the T-SQL:

| Check | T-SQL | MySQL |
|---|---:|---:|
| ledger write calls | 98 | 98 |
| `@CurrentDelAccID` postings | 20 | 20 |
| `IF @AgentCheck` guards | 11 | 11 |
| `@NewPrice1` arithmetic | `FLOOR(@OldTotalPrice1 / @TransPrice)` / `* @TransPrice` | identical |
| the missing posting itself | line 1561 | present, same arguments |

The only textual difference in that whole region is a **commented-out** `--@CurrentDelAccID` line, which the
comment stripper correctly removes (26 vs 25 raw occurrences).

So: the code is faithful as far as reading can establish, and the save path is proven by replay — but nobody
should claim the external **confirm/deliver/cancel** paths are verified until one real transfer per path has
been put through the **UI** and its `AccSafeActivityTb` rows compared. That is the single largest remaining
unknown in this migration.

## PRODUCTION DEPLOYMENT (148.251.245.41) — done, verified

Deployed to the production MariaDB 10.4.32 (the same server the shipping system runs on). Three schemas
created fresh — **none existed before**, so nothing was overwritten:

| Schema | tables | procs | funcs | views | rows |
|---|---:|---:|---:|---:|---:|
| `EXCHANGESYS2026` | 188 | 911 | 127 | 31 | 100,798 |
| `ExSyAccounts2026` | 1 | 1 | 1 | 0 | 506,189 |
| `ExSyAccountsCurrency2026` | 2 | 1 | 0 | 0 | 184 |

### Re-synced 2026-07-21 after the converter fixes

Every routine was rebuilt locally (the string-aware comment stripper and the line-ending normalisation change
output for many procs) and pushed to production. Re-verified afterwards, not assumed:

* all **1041** routines — `MD5(body + return type + parameter list)` identical local vs production, 0 differing
* all **31** views — `MD5(VIEW_DEFINITION)` identical
* the 14 ledger indexes on `ExSyAccounts2026.AccSafeActivityTb` identical, including the 3 added for
  performance (see `proof/handport_indexes_ledger.sql`)
* `VW_SALARYCALCLAST` returns its 58 rows (it raised error 1172 before the multi-row `SELECT .. INTO` fix)
* `EMP_GetADVPMNTTtoalsIndivdual(38)` = 600.000 and `Get_AllBranchSafesVal(3)` = 33944.414 — both exactly
  what SQL Server returns
* ledger `SUM(Debit)` / `SUM(Credit)` still 1,103,715,865.657 / .660 — **no data was touched**, only routines
* `shippingtransportsystem2026` still 218 tables / 989 procs — untouched

The previous production routines were dumped to `migration/backup_prod_routines/` before the push, so the
change is reversible.

**Verified against local, not assumed:**
* every one of the 191 tables — row counts identical
* ledger `SUM(Debit)` / `SUM(Credit)` identical to the milli-unit (1,103,715,865.657 / .660)
* Arabic text — server-side `MD5(GROUP_CONCAT(...))` identical for currencies and users
* all 1,004 routine bodies — `MD5(ROUTINE_DEFINITION)` identical
* all 31 view definitions — `MD5(VIEW_DEFINITION)` identical
* login + activation procs execute on production and return correct Arabic
* pre-existing databases untouched (`shippingtransportsystem2026` still 218 tables / 989 procs)

Data is the **2026-07-08 snapshot** (13 days old at deploy time) — deployed by explicit decision. If this
becomes the live system, transactions from Jul 8 onward must be reloaded from the authoritative source first.

### Two defects this deployment exposed (both fixed)

* **All 31 views deployed as mysqldump PLACEHOLDER stubs** (`select 1 AS col,...`). mysqldump writes views in
  two passes — dummy first, real definition last — and the first structure load was killed by a 10-minute
  timeout before the final pass. The stubs are *syntactically valid and queryable*, so a "does it error?" check
  passes: `View_User_TB_CHECKACOUNT` returned one row of literal `1`s and **login silently failed**. Rebuilt all
  31 from `information_schema.VIEWS` and now compared by MD5, not by "it runs".
  **Lesson: verify a deployment by comparing definitions/row counts, never by absence of errors.**
* **3 view columns were `utf8mb4_general_ci`** instead of `utf8mb4_unicode_ci`. `CAST(x AS CHAR)` (stored by
  MariaDB as `cast(x as char charset utf8mb4)`) and `FORMAT(x,n)` take the charset's DEFAULT collation and
  IGNORE `collation_connection` — even though all 31 views were created with the correct session collation.
  Any view UNIONing those columns died with `ERROR 1271 Illegal mix of collations`, which also made mysqldump
  abort on the whole schema. Fixed in `proof/handport_view_collation_fix.sql`.
  Note audit check 6 covers ROUTINE collation only — **views were a blind spot**.

Switch the app between targets with `MD_MYSQL.USE_LOCAL_MYSQL()` / `USE_PRODUCTION_MYSQL()` (defaults to LOCAL).

## The app no longer ships with a superuser — `exchange_app`

`RhallaConfig.ini` used to carry `rhalla_app`, which holds `GRANT ALL PRIVILEGES ON *.* WITH GRANT OPTION`.
Since that file ships next to the exe in plaintext, every recipient of the Debug folder had full control of the
server — including the **live shipping system's 218 tables**. Both connection strings now use `exchange_app`.

**Discovered while doing it: `mysql.db` on the production server is a CRASHED Aria table.**
`CHECK TABLE mysql.db` reports *"Table is marked as crashed / Wrong base information on indexpage at page: 1 /
Corrupt"*. The data is readable (9 rows) but the index is broken, so **the server cannot write any
database-level GRANT** — `GRANT .. ON db.* ..` fails with error 1034. Every other privilege table
(`user`, `global_priv`, `tables_priv`, `procs_priv`, `columns_priv`) checks out OK.

`REPAIR TABLE mysql.db` is the fix, but it write-locks the privilege table on a server that also runs the live
shipping system, so it was **not** run — it needs the owner's go-ahead during a quiet window.

The scoped user was therefore built **without touching the corrupt table**, using per-object grants
(`mysql.tables_priv` / `mysql.procs_priv`, both healthy):

| Grant | Scope |
|---|---|
| `SELECT, INSERT, UPDATE, DELETE` | each of the 191 base tables in the three Exchange schemas |
| `SELECT` | each of the 31 views |
| `EXECUTE` | each of the 1041 routines |
| `CREATE TEMPORARY TABLES` | **global** — see caveat |

1263 object-level grants. `CREATE TEMPORARY TABLES` exists only at database or global level, and the data
layer needs it: `MD_CONNECTION_MYSQL.vb` stages every TVP parameter into a client-side
`CREATE TEMPORARY TABLE tvp_<name>`. With the DB level unavailable it is granted globally — it conveys **no
data access**, only the right to create a session-local temp table.

Verified against production, as `exchange_app`:

* reads tables, views and the 506k-row ledger; creates temp tables; calls procedures and functions
* `View_User_TB_CHECKACOUNT` returns all **69** rows (login works)
* `ActivivationTb_SELECHDDPROMac` returns this PC's row — identical to `rhalla_app`
* **denied** on `shippingtransportsystem2026` (`ERROR 1142`), and **cannot grant** (`ERROR 1045`)

Residual, stated plainly: the global `CREATE TEMPORARY TABLES` makes `SHOW DATABASES` list every schema
name. Names only — the data is not readable. Repairing `mysql.db` and moving that one grant to database level
would close it.

### Debug folder prepared for the team — 383 MB -> 316 MB

Removed: 73 IntelliSense `.xml` doc files (dev-only, regenerable), `APPFORUPDat1.exe` (the auto-updater's
leftover copy of the **old SQL Server build** — shipping it alongside would be actively confusing), and
`mysql_errors.log`. The pre-change config backup was moved to `_removed_from_debug/` because it still holds
the old `rhalla_app` password. The 9 `.pdb` files were **kept on purpose**: they turn a tester's crash into a
stack trace with line numbers. Confirmed the old password appears nowhere in the shipping folder.

## How to launch on MySQL

Everything on the app side is already wired — just run **`run.bat`** (builds with VS2022 MSBuild, then starts
`ExchangeSystem\bin\Debug\ExchangeSystem.exe`). Verified working: build exit 0; MariaDB running as an
auto-start Windows service; `MD_MYSQL.USE_MYSQL = True`; `RhallaConfig.ini` next to the exe holds `MYSQL_CONN`;
`MySqlConnector.dll` deployed; device **activation** returns this PC's row; the **login** proc executes.

To fall back to SQL Server at any time: set `MD_MYSQL.USE_MYSQL = False` (one flag, no other code change).

### Gotcha: "هذا الجهاز غير مرخص" is (usually) a MISSING DLL, not a licensing problem

`packages.config` does **not** pull transitive NuGet dependencies. MySqlConnector 2.3.7's nuspec declares three
for net48, and without them the **first `new MySqlConnection(...)` throws `FileNotFoundException`** — so every
query returns nothing, the activation lookup finds 0 rows, and `Module2.Get_Acctvionpc` reports the device as
unlicensed. The device is fine; the data layer never ran. All three are now referenced in the `.vbproj` and
present under `ExchangeSystem\packages\`:

| Assembly | Version it binds to | Package |
|---|---|---|
| `Microsoft.Extensions.Logging.Abstractions` | 7.0.0.1 | 7.0.1 (`lib\net462`) |
| `System.Threading.Tasks.Extensions` | 4.2.0.1 | 4.5.4 (`lib\net461`) |
| `System.Diagnostics.DiagnosticSource` | 7.0.0.2 | 7.0.2 (`lib\net462`) |

Use the **net46x** builds, not `netstandard2.0` — only those carry the exact assembly versions above
(`netstandard2.0` ships Logging.Abstractions as 7.0.0.**0**, which does not satisfy the 7.0.0.**1** bind).
`System.Memory` / `System.Buffers` / `…CompilerServices.Unsafe` already resolve via the existing App.config
`bindingRedirect`s.

**Diagnosing this class:** `bin\Debug\mysql_errors.log` records every failed call with its exception — it named
the missing assembly directly. It is the first place to look when a screen is silently empty or "unlicensed".

### Screens that WILL fail (41 procs still missing — see the table at the bottom)

The blockers are concentrated in the **transfer** and **currency buy/sale** write paths, because those procs
use `GOTO`, which MySQL has no equivalent for and which cannot be restructured safely without live testing:

| Screen / feature | Missing proc | Why |
|---|---|---|
| External transfer — create | `ExternalEx_Insert` | GOTO |
| Internal transfer — create | `InternalEx_Insert1` | GOTO |
| Both transfer screens (lookup) | `SendTypeTB_Roll_luckbedit` | temp-table/DROP ordering |
| External transfer (service val) | `CATEGORYTYPESDETAILSTB_GET_ServiceExVal` | parameterized TVF |
| Currency buy / sale / statement | `NewCurrencyBuyandSale_CRUD`, `NewCurrencyBuyandSale_Insert`, `NewCurrenciesBuyandsellTB_Update` | Arabic-literal CASE / TVF / fn call |
| Leave request — save | `LeaveTB_Insert` | GOTO |
| Salary calc / bank portfolio | `ZRPT_SalaryCalc_LoadToCalculate`, `SalaryCalculation_LoadToBankPortfolio` | UNION alias / @@ROWCOUNT |
| Taxi + mobile screens (5 procs) | `GET_deteelsForMobile`, `GET_deteels_for_taxe`, `DriversTb_Add_From`, `InternalEx_getinsert_DailyCount`, `esertype_mobile_…` | **dynamic SQL** (`sp_executesql`) → needs PREPARE/EXECUTE |
| Statements/reports (~12) | `Cashstatement`, `BalanceSheet_Statment_Detials`, `ASSMember_Statment`, `Budget_per_mini_iliali*`, `accacounselecttotaldor`, `ACCOUNTSTB_SelectICOUNTDetelles2`, `GET_Total_Currency_CustomersTb_Proc`, several `ZRPT_*` | parameterized TVFs / UNION-alias / derived-table |

**Not app logic — safe to ignore:** `activate_oqs_scheduler`, `gather_statistics`, `start_scheduler`,
`stop_scheduler` (SQL Server Open Query Store diagnostics). **Source-broken** (invalid T-SQL too, does not
compile on SQL Server either): `CustomersTb_insert_Mobile` (a `returns` typo).

> Everything else — login, activation, permissions, accounts, customers, employees, safes, branches, banks,
> currencies/prices, WhatsApp price broadcast, and the whole reporting surface bar the rows above — is live.
> **All 13 silent-bug audits are green.** But note: the last three real bugs were caught by *diffing against
> SQL Server*, not by the static audits — so treat screen-level testing (Phase 6) as required, not optional.

## Coverage (current)

| Object | Source | Live | % |
|---|---:|---:|---:|
| Tables + data | 188 / 100,798 rows | 188 / 100,798 | 100% (row counts verified) |
| Scalar functions | 127 | **127** | **100%** |
| Views | 22 | 21 | 95% (`query_stats` is a SQL Server DMV diagnostic view — N/A) |
| Stored procedures | 916 | **877** | ~96% |
| Table-valued functions | 20 | 4 (as views) | 2 parameterless + 2 parameterized (view + caller-side filter) |

> **The pipeline must include `tvpprocs`.** `functions → procs → hardprocs → tvpprocs → views` — the TVP bucket
> is separate, and leaving it out silently left 6 procs absent (they appear in NO fail list, so nothing flags
> it). Always finish with `apply_handports.sh` + `audit.sh`.

### What adding `tvpprocs` exposed (all now fixed)

Creating those 6 procs immediately tripped the audit — which is exactly its job. Three were **real** and two
were **holes in the checker itself**:

* **`CONVERT(<char>, <date>, <style>)` dropped its STYLE CODE** → a bare `CAST(x AS CHAR)`. Silent and live:
  `CONVERT(NVARCHAR, DateForTime, 22)` must render `07/16/26  7:06:01 PM` but the app was getting the whole
  raw datetime. Hit `CurrencyMovements_fillCrid`, `ZRPT_CurrencyMovements_fillCrid`,
  `ExternalEx_LoadToConfirm` (live!) + 3 TVP procs. Styles now mapped (verified against SQL Server output):
  `108`→`%H:%i:%s`; `0` on a TIME→`%l:%i%p`; `22`→ `mm/dd/yy` + a **space-padded** 12-h hour + `:%i:%s %p`
  (T-SQL right-aligns the hour in 2 chars — no single `DATE_FORMAT` pattern does that). **New audit check 9b**
  guards the class.
* **`cmp_writes` was blind to whole UPDATE statements.** An `UPDATE t SET .. FROM a JOIN b ON ..` with no
  WHERE (the JOIN *is* the filter — legal and common here) let the lazy SET capture run through the FOLLOWING
  statement to a later WHERE, so that next UPDATE was never compared. The same bleed made `guarded()` report a
  phantom "lost a WHERE". Both terminators are now statement-aware; the checker now compares MORE statements
  than before and they all match.
* **`canon()` flagged a correct translation.** `CONVERT(VARCHAR(8),GETDATE(),108)` vs
  `DATE_FORMAT(NOW(),'%H:%i:%s')` are the same operation; both now reduce to `~datefmt <expr>` (the 2-arg
  CONVERT still collapses to a plain cast, so nothing is over-normalised).

Hand-ported this pass (all in `proof/`, auto-applied by `apply_handports.sh`): the 3 price-list **STUFF/FOR XML**
Grid procs → `GROUP_CONCAT` over a derived table (inner `ROW_NUMBER()` — MariaDB 10.4 err 4074 — moved out;
`group_concat_max_len` raised so a long list is not truncated); the 2 `DBCC CHECKIDENT` reseed procs → dynamic
`ALTER TABLE .. AUTO_INCREMENT`; `ExternalEx_SelectType` `FULL OUTER JOIN`→`LEFT JOIN` (WHERE eliminates the
right-only rows); `Accounts_LimitedStatment` + `SalaryCalculationTb_MoneyCard` `OUTER APPLY`→ inline correlated
subqueries; **the first 2 parameterized TVFs** (see below).
Converter: a digit glued to a keyword (`+1from`) now gets a space; `.NET "F<n>"` money format →
`REPLACE(FORMAT(x,n),',','')` (no thousands separator); a CATCH that returns the error as a **result set**
(`SELECT ERROR_NUMBER(), ERROR_MESSAGE()`) → `GET DIAGNOSTICS CONDITION 1 .. MYSQL_ERRNO/MESSAGE_TEXT` into
handler-local vars (+2 procs). Audit check 5 now catches a raw `FORMAT(arg,'..')` whose first arg holds a
nested call (money OR date silent-wrong), a gap the old `[^)]*` missed.

### Parameterized TVFs — the pattern (verified byte-identical vs SQL Server)

MySQL has no TVFs and a view takes no parameter. Two sub-cases:

* **Param is a GROUP BY key that the TVF also returns** (`GET_TABLE_FOR_CostofByBranch`,
  `NEW_GET_TABLE_FOR_CostofByBranch`): a group lies entirely inside one key value, so filtering *before* or
  *after* the GROUP BY selects the same groups. → **view without that predicate + caller-side
  `WHERE key = @p` in a derived table** (keeps the caller's alias/column list untouched). Both verified equal
  to SQL Server on values AND row counts across branches.
* **Param is a pre-aggregation filter** (a date range: `InsertDate BETWEEN @D1 AND @D2`) or **selects an IF
  branch** (`GET_Total_Currency_CustomersTb(@Type,..)`): hoisting is NOT sound — these need the TVF body
  inlined into a proc (or a proc + temp table). Still outstanding.

> **A `RETURNS @T TABLE (Costof DEC(13,3), SalePrice DECIMAL(12,3), …)` declaration is an implicit CAST on
> every returned column.** A plain view returns raw expression precision instead — SQL Server hands the app
> `6.976`/`5.800`, an uncast view hands it `6.9762250`/`5.800000`. Every TVF→view port must CAST each column
> to its declared RETURNS type. This was caught only by diffing against SQL Server, not by any audit.

**Remaining to create: 46 procs.** Categories: parameterized-TVF callers (~6 left), TVP procs (8),
**dynamic SQL via `sp_executesql` (5 — needs `PREPARE`/`EXECUTE`)**, `GOTO` label/loop (2, one is a 2 500-line
ledger write proc), a braceless-`IF`+plain-`BEGIN` control-flow case (ZRPT grid), UNION/derived-table alias
errors (2), plus SQL-Server-only diagnostics that are **N/A** (`activate_oqs_scheduler`, `gather_statistics`,
`start_scheduler`, `stop_scheduler` — Open Query Store) and a few genuinely **source-broken** procs (e.g. a
`returns` typo that is invalid T-SQL too). No cursors. All 12 silent-bug audits + the write-path checks stay
clean throughout.

Converter fixes added while pushing coverage (each is a *translator* fix; business logic untouched, audits
re-run green after every one): DATEADD/DATEDIFF → DATE_ADD/TIMESTAMPDIFF and **DATEPART → the scalar
`DAY()`/`MONTH()`/`YEAR()`/… functions (no inner `FROM` to confuse the SELECT-assignment rewrite), with
`WEEKDAY`→`DAYOFWEEK` and `DAYOFYEAR`→`DAYOFYEAR`**; stray `BEGIN;` block-opener semicolon dropped; `CONVERT()`
conversion no longer capped at 50 per proc (giant salary procs have >100); N-prefixed literal `N'…'` kept whole
in a `+` chain (was producing `NCONCAT`); `INSERT..(cols) SELECT <values>` followed by another `SELECT` now
terminated; blank line between a bare `DECLARE` and its list handled (was dropping the last var as `DECLARE ;`);
T-SQL table lock hints `WITH (UPDLOCK, HOLDLOCK)`/`(NOLOCK)` stripped; the block-close `END;` recognised as a
statement boundary (last statement before it now terminated); the SET-RHS concat fix bounded to its own
statement (**a numeric `SET x=x+1` was swallowing a following string `SET y=..+'..'` and mis-wrapping both in
one broken `CONCAT`** — a silent-correctness fix); multi-line `SELECT @x = ISNULL((SELECT..FROM..), 0)`
subquery assignments collapsed onto one line before the assignment rewrites. Audit tool hardened: multi-table
`UPDATE` target resolved by SET-column ownership (col map), CASE-aware SET-body cut, and a `+ <number>`
arithmetic false-positive removed from the string-concat check. Earlier fixes: CTE (`WITH`)
kept as one statement; `TIME(7)`→`TIME(6)`; CATCH "capture+re-raise" → `RESIGNAL`; `ELSEIF` statement
boundary; interleaved comma/comment DECLARE lists; multi-line & compound (`+=`) SELECT-assignments;
CASE-with-subquery masking so `SELECT @x=CASE..END FROM t` keeps its FROM; `SELECT TOP n`→`LIMIT n` (and
`TOP 100 PERCENT` dropped); `DECLARE x DECIMAL(18, 3) = 0`→`DEFAULT 0`; a full expression-converter for VIEWS
(comments, `alias = expr`→`expr AS alias`, TOP, CONVERT, concat, collation, trailing-space name);
balanced-group paren matcher so deeply-nested `CAST(IFNULL((SELECT MAX(..)..)..) )` in a `+` chain no longer
mis-splits into `CAST)(`; **multi-line `EXEC proc @a,@b,..` args captured** (was silently becoming an empty
`CALL proc()` that skipped the ledger write — now audit 6b guards it); bare-`SELECT`-on-its-own-line pull-up.

---


Target engine: **MariaDB 10.4.32** (XAMPP, `127.0.0.1:3306`) — same instance the shipping system already runs on.
Source: `EXCHANGESYS2026` on `DESKTOP-M233HRE\SQLEXPRESS` (read-only; never written to).
Playbook: `shiping-systm2026-on-github/README_MYSQL.md`. Rule followed throughout: **literal translation, no redesign.**

---

## Where it stands

| Object | Source | Live in MariaDB | Still to hand-port |
|---|---:|---:|---:|
| Tables | 188 | **188** (100%, 100,798 rows, row counts verified equal) | 0 |
| Stored procedures | 911 | **792** | 119 |
| Scalar functions | 127 | **118** (108 diff-verified, **0 diff-failures**) | 9 |
| Views | 22 | 10 | 12 |
| Table-valued functions | 20 | 0 | 20 |
| Column DEFAULTs | 327 | **326** | 1 (dead sequence, see below) |

**910 routines live.** All carry `utf8mb4_unicode_ci`. All four audits below return 0.

MariaDB now runs as a **Windows service** (`MariaDB`, auto-start) — previously it was a foreground process
that died whenever its parent shell exited, taking the whole database offline.

## ⚠️ After ANY `migrator functions|procs|hardprocs|views|tvpprocs` run: `bash migration/apply_handports.sh`

The migrator prefixes every object with `DROP ... IF EXISTS`, then CREATEs the auto-converted version. For a
routine we hand-ported *because the converter cannot do it*, that CREATE fails — so the DROP has silently
**deleted the working hand-port** and left nothing. The routine vanishes and the screen calling it breaks.
`apply_handports.sh` re-applies everything in `proof/` in dependency order (cross-DB → synonyms → functions → procs).

The VB app **builds and runs against MariaDB**. Verified end-to-end:
- device activation (`ActivivationTb_SELECHDDPROMac`) — passes,
- login (`User_TB_CHECKACOUNT`) — returns byte-identical rows to SQL Server (same columns, same order),
- Arabic text and RTL data round-trip correctly (`فرع البيضاء`, `المهدي`).

---

## The big finding: SQL Server SYNONYMS

`EXCHANGESYS2026` has **9 SYNONYMs**. They are the system's *real* cross-database mechanism: a proc writes
`FROM dbo.ExSyAccounts_AccSafeActivityTb` and SQL Server silently redirects that to
`[ExSyAccounts2026].[dbo].[AccSafeActivityTb]`.

Because the 3-part name never appears in the proc text, **a text scan of `sys.sql_modules` does not reveal
these dependencies.** They surface only at runtime, as `Table 'a' doesn't exist` — MySQL reports the table
*alias*, not the missing table, which is thoroughly misleading.

This was worth **41 functions**: diff-passes went 47 → 88 once the synonym shims existed.

MySQL has no synonyms, so `proof/handport_synonyms.sql` recreates each one under its original name:
table synonyms → updatable views, proc synonyms → wrapper procs. No proc body was edited.

**If a screen fails with "Table 'x' doesn't exist" where x is a one-letter alias, look for a missing synonym first.**

## Co-located databases

| Schema | Why | Rows |
|---|---|---:|
| `ExSyAccounts2026` | the accounting ledger, reached via synonyms | 506,189 |
| `ExSyAccountsCurrency2026` | currency ledger + its `AccSafeActivityTb_Insert` proc | 184 |
| `rhalla2026Teset` | `whatsapp_contacts` (already existed from the shipping migration) | 1 |
| `EXCHANGESYS` | **compatibility shim** — see below | — |

## Things that are already broken in SQL Server (not caused by the migration)

1. **`EXCHANGESYS` database does not exist.** 90 routines reference it — 88 of them call
   `EXCHANGESYS.dbo.ERROR_PROC` from a CATCH block. Handled the way the shipping migration did: a local
   `ERROR_PROC` that `SIGNAL`s the error to the app (without the SIGNAL the handler swallows failures and the
   app thinks a failed write succeeded). `EXCHANGESYS.InternalEx` is exposed as a view onto the real local
   `InternalEx` table — this **restores** `InternalEx_SearchForCurrentRecorde2025`, which cannot run today.
2. **Synonym `GEt_DriversTb`** points at `ShippingTransportSystem2024Teset`, which does not exist. 2 routines
   use it. Deliberately **not** shimmed — inventing a target would invent behavior. Needs a product decision.
3. **`BankVisaTransfers_CentralLibya.TransferSeq`** defaults to `NEXT VALUE FOR Seq_BankVisaTransfers`, whose
   start value (`-9223372036854775808`, a bigint) cannot fit its `int` column. Never consumed. DEFAULT omitted
   and logged to `migrator/schema_manual_defaults.txt`.

---

## Converter fixes made (in `migration/migrator/Program.cs`)

The shipping migrator was reused as-is except for these gaps, each of which was blocking dozens–hundreds of
routines. Every one is a *translator* fix; no business logic was altered.

| Fix | Impact |
|---|---|
| **`+` string concat left as-is → MySQL does ARITHMETIC** (README §3.2) | **57 routines were silently returning wrong data.** Now 0. See below. |
| **Routines created without `collation_connection`** (README §7) | all **897** routines had `general_ci` baked in vs `unicode_ci` columns → error 1267 at runtime on any UNION/string compare |
| `SET XACT_ABORT ON` not stripped (only `NOCOUNT` was) | **+102 procs** — the single biggest create-fail win |
| SYNONYM shims (above) | **+41 functions** diff-passing |
| Multi-line / CASE-bearing `SELECT @a = …, @b = … FROM` not handled | functions 88 → 100 diff-passing, diff-failures → **0**; unblocked `Account_GetAccVal` etc., which many procs call |
| `CONVERT()` regex could not span nested parens | e.g. `CONVERT(VARCHAR, ROW_NUMBER() OVER (…))`; replaced with a balanced-paren parser |
| Column `DEFAULT`s were not carried across at all | 326 defaults — see README §8.2, this is a silent-data-loss bug |
| `DECLARE` alone on its line (list starts next line) | multi-declare procs |
| `SET @x += 1` (compound assignment — no MySQL equivalent) | |
| Multi-line braceless `IF … ELSE …` | had to run *before* `ConvertControlFlow`, which mis-scopes them |
| Greedy single-line `IF` rule re-wrapping an already-converted `IF` | produced a stray `THEN RETURN …; END IF;` |
| `[VARCHAR](200)` → backticked into an invalid *type* | |
| `CAST(x AS VARCHAR/INT)` → MySQL needs `CHAR`/`SIGNED` | |
| `db_owner.` schema prefix (rhalla2026Teset) not stripped | |
| `;;` empty statement | |

New: `migrator debugfn <name>` prints the T-SQL and the converted MySQL side by side. Use it before
hand-porting anything — most "failures" are one systematic converter gap, not 50 unique problems.

### PERFORMANCE — indexes, and the one report that is still slow

Correctness is not the only way a migration fails. Two index problems, both now fixed:

1. **`crossdb` copied no secondary indexes.** The co-located ledger `ExSyAccounts2026.AccSafeActivityTb`
   (506,189 rows) arrived with only its PRIMARY KEY, so every proc touching it did a full scan.
2. **SQL Server's CLUSTERED index has no MySQL equivalent.** The source table is clustered on `AccIDFrom`
   with `Debit, Credit` as INCLUDE columns, so SQL Server reads the row data straight from the index. MySQL
   clusters on the PK (`ID`), so a secondary-index hit needs a bookmark lookup per row. The fix is a
   **covering** index that carries the summed columns:
   `IX_cover_AccIDFrom (AccIDFrom, AccBranchID, IsActive, Debit, Credit)`.

Result on `Branch_GetNetVal`: **10.3s → 0.055s** (EXPLAIN goes to "Using index").
**Run `ANALYZE TABLE` after creating indexes** — MariaDB kept choosing the wrong index until statistics
were refreshed.

> ⚠️ **Still open: `AgentAccountStatement`** (and its `ZRPT_` twin). SQL Server: 2.2s. MySQL: minutes.
> The proc calls the scalar `Branch_GetNetVal()` from its SELECT list; SQL Server evaluates it far fewer
> times than MariaDB does. Every individual query inside the function is now fast (<0.06s) — this is an
> optimizer difference, not a translation bug. It needs a targeted hand-port (materialize the per-branch
> values into a temp table once, then join) which keeps the result set identical.

### The two SILENT bugs (worth understanding — they do not raise errors)

**1. `+` string concatenation.** MySQL's `+` is arithmetic only. `'رقم: ' + @code` does not fail — it coerces
the text to a number and yields `0`. A proc created this way looks perfectly healthy and returns garbage.
A real one found here, in a proc that had "successfully" converted:

```sql
-- T-SQL                                          -- what MySQL got (silently wrong)
ACCCODE like + convert(varchar,@accacount) + '%'  ACCCODE like + CAST(p_accacount AS CHAR) + '%'
                                                  -- => LIKE 0  => matches nothing, no error
-- now correct:
ACCCODE like CONCAT(CAST(p_accacount AS CHAR), '%')
```

A blanket `+`→`CONCAT` is NOT safe (it destroys numeric addition). The converter rewrites a `+` chain only when
it can PROVE an operand is a string. Each of these proofs had to be added after finding a live miss:

| Proof | The case that forced it |
|---|---|
| a string literal `'x'` | the common case |
| a `CAST(… AS CHAR)` | `ACCCODE like + CAST(@x AS CHAR) + '%'` |
| a variable DECLAREd VARCHAR/TEXT | `SET @msg = @a + @b` |
| a **string-returning function** (`SPACE`, `SUBSTRING`, `LEFT`, `TRIM`, …) | `B.BankName + SPACE(1) + A.BranchName` — two plain COLUMNS, no literal anywhere. The branch-name column was rendering as **`0`**. |
| the chain **feeds a `LIKE`** | `WHERE GCODE + GRNAME LIKE '%'+@ISID+'%'` — MySQL computes `GCODE+GRNAME` = `0`, so the predicate becomes `0 LIKE '%x%'` and the **search screen silently returns no rows** |

**57 routines were affected initially; the last two proofs caught 3 more that all the earlier audits had passed.**
Audits are clean now — re-run them after any converter change.

**2. `FORMAT()` — the same function name, a different meaning.** BOTH engines have `FORMAT()`, so nothing
errors, but they take different second arguments:

```
T-SQL : FORMAT(x, 'N3', 'en-us')   -- .NET format string: thousands separators + 3 decimals
MySQL : FORMAT(x, D [, locale])    -- D = NUMBER of decimal places
```

MySQL coerces the string `'N3'` to the number `0`, so **every formatted money value came back with zero
decimals**: `1,022,611` instead of `1,022,611.330`. Found in **61 routines / 331 call sites** — i.e. most of
the financial reports were quietly rounding. Now translated to `FORMAT(x, 3)`; `CreditAccountStatement` output
is byte-identical to SQL Server again.

**2b. …and T-SQL `FORMAT()` is ALSO the date formatter.** The fix above, applied naively, created a *new*
silent bug of exactly the same shape:

```
FORMAT(CONVERT(datetime, a.InserTime), 'hh:mm:ss tt')   -- T-SQL: a TIME
  -> FORMAT(x, 0)        gave  20,260,712,191,353       -- numeric formatting of a datetime!
  -> DATE_FORMAT(x, '%h:%i:%s %p')  gives  07:13:53 PM  -- correct
```

A `.NET` pattern is now classified before translating: `N<digits>` / `#,##0.000` → numeric `FORMAT`, anything
containing `y M d H h m s t` → `DATE_FORMAT` with mapped specifiers (**case matters**: `M` = month, `m` =
minute). Unknown patterns (`'C'`, `'P'`) are deliberately left alone so MySQL errors **loudly** rather than
silently producing a wrong value. Audit 9 cross-checks every date-formatting routine in the SQL Server source
against its MySQL counterpart.

**3. Baked-in collation.** MySQL stores the session collation *with* the routine at CREATE time. The migrator
set `SET NAMES utf8mb4` but not `collation_connection`, so every routine was created as `utf8mb4_general_ci`
while the columns are `utf8mb4_unicode_ci` → error 1267 on any UNION or string comparison. All routines
have been recreated with the correct collation.

> **The pattern:** the dangerous bugs are the ones where BOTH engines accept the syntax and only the *meaning*
> differs (`+`, `FORMAT`, `bit = 'true'`). A create-failure is loud and lands in a worklist. These are silent.
> After ANY converter change, re-run the audits at the bottom of this file.

---

## What's left (Phase 4)

119 procs + 22 functions + 12 views + 20 table-valued functions need hand-porting into `migration/proof/`.
The remaining failures are a genuine long tail — T-SQL `ERROR_NUMBER()`/`ERROR_MESSAGE()` inside CATCH blocks,
`CHARINDEX`, `STUFF`/`FOR XML`, `PIVOT`, table variables — not one more big systematic win.

Also open: **14 simple procs are created but diff-FAIL** (right row count, wrong content/OUT values). Those are
more urgent than the create-fails: a create-fail is loud, a diff-fail is quiet. They are listed under
`== created but diff FAILED ==` in `migrator/procs_needs_manual.txt`.

Practical order: hand-port **per screen**, driven by what actually breaks, rather than alphabetically —
`mysql_errors.log` (next to the exe) names the failing proc and dumps every parameter.

## The write path — 565 procs that were never verified at all

The diff test only compares **read** procs (it executes them and diffs the rows). The 565 transactional procs
had **no output verification whatsoever**. The migrator ships a `hardverify` mode for this, but it is **not
safe to run here**: it EXECUTES each write proc against SQL Server, and the T-SQL references
`ExSyAccounts2026` by absolute name and through SYNONYMs — so even pointing it at a snapshot copy would still
mutate the **real 506,189-row ledger**. It is deliberately never run.

Instead they are verified **statically** (`cmp_writes.py`, wired in as audit 10): for every proc, the INSERT
and `UPDATE … SET` column lists are extracted from both the T-SQL and the MySQL body and compared —
same target table, same columns, **same order**. The VALUES list is positional, so a single shifted column
silently writes the debit into the credit field.

**This immediately found a real, silent bug.** `CATEGORYTYPESTB_Insert`:

```sql
-- T-SQL
UPDATE CATEGORYTYPESDETAILSTB          -- target
SET    ValFrom = a.ValFrom, ...
FROM   @Type AS a                      -- source (a TVP)
WHERE  ID = @ID;

-- what the converter produced
UPDATE tvp_Type AS a SET ValFrom = a.ValFrom, ...   -- !! updates the TEMP TABLE
```

MySQL has no `UPDATE … FROM`, so the rewrite moves the FROM source up next to `UPDATE`. That is right when the
target is an *alias* defined in the FROM (`UPDATE a SET … FROM Invoices a JOIN …`) — but here the target is a
**table that never appears in the FROM**, so it was dropped entirely and the temp table became the target.
The real table was never written: the user edits a discount tier, the screen saves, nothing changes, no error.
Fixed to emit `UPDATE CATEGORYTYPESDETAILSTB, tvp_Type AS a SET …`.

Four write-path checks now run over **all three schemas** (918 procs), all clean:

| Check | Covers | Why it matters |
|---|---:|---|
| **lost `WHERE`** | 918 procs | an `UPDATE`/`DELETE` that dropped its `WHERE` rewrites **every row** in the table — the most destructive possible outcome, and completely silent (the proc succeeds) |
| INSERT columns | 73 procs | the VALUES list is positional |
| `UPDATE … SET` columns | 76 procs | a dropped column means the screen "saves" and that field never changes |
| DELETE targets | 18 procs | deleting from the wrong table |
| **INSERT `VALUES`, positionally** | 59 procs | the column checks prove the right *columns* are named. They are blind to a wrong *value*: swap two expressions and the INSERT still names the right columns, still succeeds — and files the **Debit under Credit**. Nobody notices until the books are wrong. |
| **`UPDATE … SET` values** | 76 procs | same class, on the update path |

The value checks compare the expressions **positionally**, after canonicalising the translations that are
legitimate — `@X`↔`p_X`, `ISNULL`↔`IFNULL`, `GETDATE`↔`NOW`, `a + b`↔`CONCAT(a,b)`, `CAST(x AS nvarchar)`↔
`CAST(x AS CHAR)`, and T-SQL's `Cnt += 1` ↔ MySQL's `Cnt = Cnt + 1`. A genuinely different literal, or two
expressions swapped, still fails. `test_cmp_writes.py` proves exactly that: a Debit/Credit swap is caught,
while every one of those spelling differences is not flagged.

`ExSyAccounts2026` / `ExSyAccountsCurrency2026` are included deliberately: the **money ledger** lives there
(506,189 rows) and its insert proc is reached from the main schema through a SYNONYM, so it sits on the
hottest write path in the system. It also means the two hand-ported ledger procs are now diffed against their
T-SQL originals rather than taken on trust.

**The bodies are dumped through the migrator (`migrator dumpbodies`), never `sqlcmd`.** sqlcmd renders through
the console code page and turns every Arabic identifier into `?????` — and these procs are full of Arabic
column aliases (`AS 'الرمز'`). A sqlcmd-based comparison silently compares mangled text against real text.

### The detectors are themselves tested — `python test_cmp_writes.py`

Non-negotiable, because a parser that silently matches **nothing** reports a perfect `0`, which is
indistinguishable from "clean" (we already shipped one such check — see the `\b` note below). So every
detector is fired at a known-bad **and** a known-good input: guarded vs. unguarded `UPDATE`, `CASE` inside a
`SET` (must not truncate the window), a subquery inside the `WHERE`, T-SQL without semicolons, an alias target
resolved back to its real table, `+=` compound assignment, and a parameter literally named `p_IsUpdate`
(which a regex without `\b` happily reads as an `UPDATE` statement). `audit.sh` runs this test first and
refuses to trust checks 10–11 if it fails.

**End-to-end proof the audit is not vacuous:** injecting a proc with `'x' + 'y'` makes check 1 report 1;
stripping the `WHERE` off `NationalityTb_Insert` makes check 10 report 1 and name the proc. Both return to 0
when the routine is restored.

### What is deliberately NOT a gate

Comparing **result-column aliases** textually is reported, but only as *"go and look"* — never as a failure.
Every proc it flagged turned out to be an artifact of the extractor, not a bug: `CoBranch_LoadDataIntoDataGridview`
was reported as differing, but **executing** it on both engines returns byte-identical columns
(`الرقم, Code, BName, BType, Mobile1, Mobile2, IsActive`). Making it precise needs a real SQL parser.

A noisy gate is worse than no gate — it teaches you to ignore audit failures, and then the one real failure
gets ignored too. Result-set columns *are* properly verified for read procs by the migrator's diff test, which
compares the actual rows (203 procs passing).

### Known gaps (be honest about these)

- The write-path checks are **differential** — they compare MySQL against T-SQL. They cannot catch a bug in a
  routine that exists **only** in MySQL. The synonym wrapper procs and the `ERROR_PROC` shim have no T-SQL
  counterpart and are therefore unchecked by them.
- The 565 transactional procs are still not **behaviourally** verified (no execution). The static checks now
  cover the write *target*, the *columns* AND the *values* — but they are still a text comparison. They would
  not catch a difference in the surrounding **control flow** (e.g. an `IF` branch that now takes the wrong
  path, so the right INSERT runs under the wrong condition).
- 14 of the 73 INSERT procs use `INSERT … SELECT` rather than `INSERT … VALUES`, so the positional value check
  does not cover them (only their column lists are checked).
- Hand-ported **functions** are verified by executing them against SQL Server and diffing the output
  (`NormalizePhone`, `ExtractNumbers`, `ExtractLastNumbers` all match), but that is a manual spot-check, not
  an automated gate.

## Table-valued functions (20) — MySQL has NONE of these

A T-SQL TVF returns a table; a MySQL function can only return a SCALAR. So a TVF cannot be a function here.
The mapping depends on parameters and call site:

- **Parameterless, only ever `FROM dbo.fn() AS a WHERE ...`  → a VIEW.** Done for the two that matter:
  `GET_TABLE_FOR_Costof` (17 refs) and `NEW_GET_TABLE_FOR_Costof` (3 refs) — see
  `proof/handport_tvf_GET_TABLE_FOR_Costof.sql`. Each was a single `INSERT INTO @t <SELECT>; RETURN`, so the
  view is exactly that SELECT; the 9/13 columns line up 1:1 with the `RETURNS @t TABLE(...)` declaration.
  Verified equal to SQL Server (row counts + summed aggregates). The converter now strips the empty `()` from
  callers of these two names (`FROM GET_TABLE_FOR_Costof() AS a` → `FROM GET_TABLE_FOR_Costof AS a`).
- **The other 18 take parameters.** A MySQL view can't. But 14 of them have **zero** references from other
  procs (called from the app, or dead), and only 4 are referenced at all (2/1/1/1). So this is a genuine long
  tail, handled per-function by its call site — not a systematic win. Not yet done.

## Audits — `bash migration/audit.sh` (11 checks, all must print 0)

Run it after **every** migrator run and after `apply_handports.sh`. Two hard-won lessons about the audits:

1. **An audit that matches nothing looks identical to an audit that passes.** `\b` silently matches nothing in
   this MariaDB build, so check 5 was reporting a clean 0 while real bugs sat in the database. Every regex in
   `audit.sh` is now sanity-tested against a known-bad and a known-good string. Do the same for any new check.
2. **A fix can create the very bug class it is fixing** (see 2b below: the `FORMAT` money fix broke dates; the
   CASE-masking fix hid concat chains inside CASE branches). *Always re-run the audit after a converter change* —
   it caught both.

The queries below are the reference copies; `audit.sh` is what you actually run.

Each one must return **0**. They catch the silent classes that no error message will tell you about.

```sql
-- 1) '+' string concatenation that MySQL would evaluate as arithmetic (-> 0)
SELECT ROUTINE_NAME FROM information_schema.ROUTINES
WHERE ROUTINE_SCHEMA='EXCHANGESYS2026'
  AND ROUTINE_DEFINITION REGEXP "[+][[:space:]]*'|'[[:space:]]*[+]";

-- 2) T-SQL FORMAT(x,'N3',...) left intact -> MySQL silently drops the decimals
--    (use the [(] character class; a backslash-escaped paren does not survive the shell)
SELECT ROUTINE_NAME FROM information_schema.ROUTINES
WHERE ROUTINE_SCHEMA='EXCHANGESYS2026'
  AND ROUTINE_DEFINITION REGEXP "FORMAT[(][^)]*,[[:space:]]*'";

-- 3) routines created with the wrong collation -> error 1267 at runtime
SELECT ROUTINE_NAME, COLLATION_CONNECTION FROM information_schema.ROUTINES
WHERE ROUTINE_SCHEMA='EXCHANGESYS2026' AND COLLATION_CONNECTION <> 'utf8mb4_unicode_ci';

-- 4) a bare SQL keyword swallowed into a CONCAT, or a CONCAT spliced into the middle of a word
--    ("ECONCAT(ND," — the rewriter starting an operand inside the word END)
SELECT ROUTINE_NAME FROM information_schema.ROUTINES
WHERE ROUTINE_SCHEMA='EXCHANGESYS2026'
  AND (ROUTINE_DEFINITION REGEXP 'CONCAT[[:space:]]*[(][[:space:]]*(END|THEN|ELSE|WHEN)[[:space:]]*[,)]'
    OR ROUTINE_DEFINITION REGEXP '[[:alpha:]]CONCAT[(]');

-- 4b) '+' concat that has NO literal to give it away. Two forms found in this database, both silent:
--       B.BankName + SPACE(1) + A.BranchName      (columns around a string FUNCTION)
--       WHERE GCODE + GRNAME LIKE '%'+@ISID+'%'   (columns feeding a LIKE -> "0 LIKE '%x%'" -> no rows)
SELECT ROUTINE_NAME FROM information_schema.ROUTINES
WHERE ROUTINE_SCHEMA='EXCHANGESYS2026'
  AND (ROUTINE_DEFINITION REGEXP '(SPACE|CONCAT|SUBSTRING|LEFT|RIGHT|TRIM|REPLACE|UPPER|LOWER)[(][^)]*[)][[:space:]]*[+]'
    OR ROUTINE_DEFINITION REGEXP '[+][[:space:]]*(SPACE|CONCAT|SUBSTRING|LEFT|RIGHT|TRIM|REPLACE|UPPER|LOWER)[(]'
    OR ROUTINE_DEFINITION REGEXP '[+][^,;)]{0,60}LIKE');

-- Anything still holding a '+' after these pass is presumed ARITHMETIC. Spot-check with:
--   SELECT ROUTINE_NAME FROM information_schema.ROUTINES
--   WHERE ROUTINE_SCHEMA='EXCHANGESYS2026' AND ROUTINE_DEFINITION LIKE '%+%';
-- and confirm each operand is numeric. (Currently 80 routines; all reviewed = arithmetic.)

-- 5) the ID=0 trap (README §8.3): a 0 id makes tree screens recurse forever
--    (checked across all AUTO_INCREMENT `ID` columns — currently clean)
```

## Hand-ports done so far (`proof/`)

| File | Why it could not be auto-converted |
|---|---|
| `handport_synonyms.sql` | MySQL has no SYNONYMs → updatable views + wrapper procs |
| `handport_crossdb_EXCHANGESYS_compat.sql` | 88 CATCH blocks call `ERROR_PROC` in a database that no longer exists |
| `handport_crossdb_ExSyAccounts2026_*.sql`, `..Currency2026_*.sql` | cross-schema ledger insert procs |
| `handport_Extract_text_functions.sql` | **T-SQL `LIKE '[0-9]'` character class** — MySQL LIKE has none; needs `REGEXP '^[0-9]$'`. Left as LIKE it matches nothing and returns `''` **silently** |
| `handport_NormalizePhone.sql` | chained `ELSE IF` must become a single `ELSEIF` chain with one `END IF` |
| `handport_CurrencyPriceShow.sql` | `ORDER BY` after a UNION may only use the first SELECT's output names |

## Switching engines

`MD_MYSQL.USE_MYSQL` in `ExchangeSystem/MDD/MD_CONNECTION_MYSQL.vb`:
`True` → MariaDB, `False` → the original SQL Server path (unchanged and still working).
Credentials live in `RhallaConfig.ini` next to the exe (git-ignored; template at repo root).
