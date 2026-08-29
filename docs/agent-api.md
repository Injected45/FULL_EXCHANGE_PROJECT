# واجهة الـ API لتطبيق الوكيل

مستخرج بقراءة كود `backend/` مباشرة. كل سطر مرجعه `file:line`.
المسار الأساسي `{APP_URL}/api` — كل `routes/api.php` مسبوق بـ `/api` تلقائياً (`bootstrap/app.php:10`).

المصادقة: **Laravel Sanctum**، `Authorization: Bearer <token>`.
الرموز **لا تنتهي أبداً** (`config/sanctum.php:49` → `expiration => null`). احفظها في `flutter_secure_storage`.
**لا توجد نقطة خروج (logout)** — الخروج = مسح الرمز محلياً.

---

## 1. من هو «الوكيل»

لا يوجد نظام أدوار في الكود — لا enum ولا middleware ولا policy. الأدوار **أرقام سحرية** تُقارَن سطرياً.

| `UeserType` | الدور |
|---|---|
| 3 | **الوكيل** |
| 5 | فرع |
| 6 | عميل |
| 7 | مندوب / سائق |

**`UeserType=3` وحده لا يكفي.** العمود الثاني `AccountType` يفصل:

| `AccountType` | المعنى |
|---|---|
| `'Main'` | الوكيل الرئيسي — الوحيد الذي يدير نقاط البيع |
| `'pos'` | نقطة بيع أنشأها الوكيل — نفس `UeserType=3`، صلاحيات أقل |

المقارنة صارمة وحساسة لحالة الأحرف: `$user->AccountType !== 'Main'` ([depositController.php:141](../backend/app/Http/Controllers/Api/depositController.php:141)).
`AccountType` ليس في `$hidden`، فيصل ضمن كائن `user` عند الدخول — **يمكن التفرّع عليه في التطبيق**.

أعمدة الهوية الأخرى: `AccID` (حساب دفتر الأستاذ) · `BrancchID` (الفرع، بهذا الإملاء) · `Reg` نص `'Yes'`/`'NO'` لا boolean.

### امتيازات الوكيل المالية — سلوك مختلف لا 403

| النقطة | الوكيل (3) و الفرع (5) | غيرهما |
|---|---|---|
| `internal/exchange` :2387 | **العمولة المرسَلة من العميل تُقبل كما هي** | الخادم يحسبها من `Transfer_commissions` |
| `internal/exchange` :2424 | **فحص رصيد المحفظة يُتخطّى** | يجب أن يكفي `wallet.Walet` |
| `internal/exchange` :2416 | سقف الفرع عبر `Rollback_Branch_Trinsfrim_me` | لا يُطبَّق |
| `external/insert/transfer` :2940,2968,2981 | الثلاثة نفسها | الخادم + فحص المحفظة |
| `internal/trans/between/accounts` :2566 | **الاستثناء لم يُطبَّق هنا** — العمولة دائماً من الخادم | نفسه |

> المقارنات مزيج من `!= 3` و `== "3"` و `in_array($x,["3","5"])` غير صارم و `(int)$x !== 7`.
> نموذج الأدوار **ضمني وغير متسق** — اطلب القائمة المرجعية من المكتب الخلفي قبل الإطلاق.

---

## 2. المصادقة

### لا يوجد تسجيل ذاتي

`device/register` **تفعيل لا تسجيل**. إن لم يوجد صف للهاتف يرد 422 `' غير مسجل'`، وكتلة الإنشاء **معطّلة بالتعليق** ([AuthController.php:155-165](../backend/app/Http/Controllers/Api/AuthController.php:155)).
ما يفعله: يحدّث صفاً قائماً بـ `password` و `Reg='Yes'` و `device_id`.

مساران فقط ينشئان مستخدماً، ولا واحد منهما ذاتي:

1. **`device/AuthorizedUsers_Add`** — المسار الحي الوحيد الذي يكتب في `users` ([depositController.php:159](../backend/app/Http/Controllers/Api/depositController.php:159)). وكيل `Main` ينشئ **نقطة بيع** ترث `BrancchID` و `AccID`.
2. **`device/forgien/exchange/deposit/store`** — يكتب في `Table_ADD_forCostumerMobile` (جدول **طلبات**)، لا في `users`. يحتاج معالجة من المكتب الخلفي. والرمز الذي يعيده يذهب إلى `CustomerTokens` الذي **لا يتحقق منه أي middleware** — ميت.

**⇒ الوكيل يُنشأ من برنامج الديسك توب فقط.**

### التدفق

```
مستخدم جديد:  otp/send → otp/checkOtp → initAuth → register (+ ترويسة Secure-Token)
مستخدم عائد:  login
```

| النقطة | الحقول | ملاحظات |
|---|---|---|
| `POST device/otp/send` | `phone` | 9 خانات تبدأ بـ 9، **بدون 218** — الخادم يضيفه. يرسل عبر واتساب. **الرمز 4 خانات** (`rand(1000,9999)`)، صلاحية 3 دقائق |
| `POST device/otp/checkOtp` | `phone`, `CodeOtp` (4 خانات) | **خارج الغلاف**: يرد `{message, status}` فقط |
| `POST device/otp/login` | `phone`, `CodeOtp`, `device_id` | **أُضيفت لهذا المشروع** — انظر أدناه |

> ⚠️ **الرمز أربع خانات لا ست.** التصميم رسم ست خانات ونصّه يقول «6 أرقام»، والخادم يولّد أربعة. ست خانات في الواجهة تجعل الدخول مستحيلاً.

> `sendOtp` **مقيّد بليبيا في الكود**: يشترط `/^9[0-9]{8}$/` ثم يضيف `218` بنفسه. لا يمكن إرسال رمز إلى رقم غير ليبي دون تعديل المتحكّم.

### `POST device/otp/login` — الدخول بالرمز وحده

أُضيفت في `AuthController::otpLogin` لأن التصميم بلا كلمة مرور بينما `login` و`register` يفرضانها. تفعل ثلاثة أشياء في معاملة واحدة:

1. **تتحقق من الرمز على الخادم** — لا على العميل. وهذا ما يميّزها عن `update/password` التي تسمح بالتغيير بمعرفة الهاتف ومعرّف الجهاز فقط.
2. **تستهلك الرمز** — تحذف كل رموز الرقم، فلا إعادة استعمال.
3. **تعيد ربط الجهاز** بعد تحقّق مؤكَّد، وتضبط `Reg='Yes'` — وهذا يحلّ قفل إعادة التثبيت الذي كان يتطلب تدخّل المكتب الخلفي.

| الحالة | الرد |
|---|---|
| رمز صحيح | 200 · `{token, user, info, Name_post, rebound}` |
| رمز خاطئ أو مستهلَك | 422 · `data: "InvalidOtp"` |
| رمز منتهٍ | 422 · `data: "ExpiredOtp"` (ويُحذف) |
| رقم غير مُنشأ في `users` | 422 · `data: "NotProvisioned"` |
| حساب محذوف | 404 · `data: "UserDeleted"` |

وتُبطل الرموز السابقة (`tokens()->delete()`) لأن الجهاز واحد لكل مستخدم.

### وجهة تطوير لرموز التحقّق — `OTP_DEV_TO`

حين يُضبط `OTP_DEV_TO` في `.env`، يُرسَل كل رمز إلى ذلك الرقم بدل رقم المستخدم، ويُتخطّى فحص وجود الرقم الليبي على واتساب. الهوية تبقى الرقم الليبي — يتغيّر المُستلِم فقط.

**لا يعتمد على `APP_ENV`** عمداً: قيمتها `local` على الإنتاج أيضاً، فالحارس الوحيد هو غياب المفتاح. **احذفه قبل أي نشر.**
| `POST device/initAuth` | `device_id` | يعيد رمزاً مؤقتاً صالحاً **3 دقائق**، أحادي الاستخدام |
| `POST device/register` | `phone`, `password`, `device_id` + `Secure-Token` | يعيد `{token, user, info, Name_post}` |
| `POST device/login` | `phone`, `password`, `device_id` | `phone` هنا **أصرم**: `/^9\d{8}$/` بالضبط |

**قيود كلمة المرور** (طابقها حرفياً في العميل):
```
/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/
```
أي رمز خارج `@$!%*?&` (مثل `#` أو `_` أو `-`) **يُرفض**.

### `device_id` — دائم وغير قابل للاسترجاع

لكل مستخدم جهاز **واحد**. `login` و `updatePassword` يرفضان عدم التطابق.
ولّده مرة واحدة واحفظه في التخزين الآمن. **تغييره يقفل المستخدم نهائياً** ولا يفكّه إلا إعادة تعيين `Reg='NO'` من المكتب الخلفي.

### حقول الرد

`user` — صف `users` كاملاً عدا `password`/`remember_token`. يحوي `UeserType`, `AccountType`, `AccID`, `BrancchID`, `Reg`, `phone`, `device_id`.
`info` — `SELECT * FROM getInfo WHERE id=?`، وهو **view** أعمدته غير معروفة من الكود. عامله `Map<String,dynamic>?`.
`Name_post` — اسم نقطة البيع، `null` للوكيل الرئيسي.

---

## 3. النقاط التي يحتاجها تطبيق الوكيل

`A` = يتطلب `Authorization`.

### الرصيد والكشوف

| المسار | M | A | الطلب | الرد |
|---|---|---|---|---|
| `device/current/balance/local/currency` | POST | ✅ | `currency_id` | **مصفوفة من كائن واحد** `[{Walet}]` لا رقماً |
| `device/forgien/exchange/deposit/balance` | POST | ✅ | `currency_id` | `[{Walet,CuName,CurCode,Currency_ID}]` — يستبعد العملة المرسَلة و`Walet<>0` |
| `device/local/account/statment` | **GET** | ✅ | — | `[{Type_from,Values_to,InsertDate,MovementType,Balnce}]` — **بلا ترقيم صفحات** (رُصد **3345 صفاً** لحساب واحد) |

> ⚠️ **`Type_from` نص لا رقم.** يعيده الخادم `'ايداع'` أو `'خصم'` — وهو `CASE` على `AccDmType` و`Debit/Credit` في المتحكّم — بينما **`Values_to` هو المبلغ** المطلق أياً كان الاتجاه. قراءة `Type_from` كمبلغ تجعل **كل حركة تبدو واردة**، وهو خطأ صامت في تطبيق مالي.
>
> اختبار التحقق: مجموع الوارد ناقص مجموع الصادر يجب أن يساوي الرصيد بالضبط.

### أحجام حقيقية — لا ترقيم صفحات في أي نقطة

| النقطة | صفوف مرصودة | زمن |
|---|---|---|
| `InternalEx_SelectType_View_not_coustmers_get` | **522** | سريع |
| `local/account/statment` | **3345** | ~1 ث |
| `internal/CommtionRetview_get` | **1385** | **~8 ث** |

الخادم لا يقبل `limit` ولا `offset` ولا بحثاً. العميل يجلب الكل ويقتطع ويبحث محلياً، ولا ينتظر النداء البطيء مع السريع في `Future.wait` واحد.

### أعمدة الـ views كما تعود فعلاً

```
InternalEx_SelectType_View_not_coustmers_get
  Code · BName · BranchDeliveredID · BranchRecievedID · CaseStauts
  ExVal (العمولة) · OverallVal (المبلغ) · InsertDate
  RecievedName · RPhone  ← RPhone لا RPhone1 · SenderName · SendStatus

AuthorizedUsersgetByBranch
  ID · Name_post · CreatedDate · IsActive · BranchID · UserID · InsertUserID · AccID · phone
  (لا يعيد Reg — فحالة «بانتظار التسجيل» غير متاحة من هذه النقطة)

CommtionRetview_get
  commion  ← بهذا الإملاء · InsertDate · AccIDFrom · AccBranchID · BName · STATUESSTRING

getInfo (view)
  id · phone · UeserType · BranchID · Countries · AccCode · National_number
  BName (اسم الجهة) · CName · DefualtCurrency · Countires_ID · CuName · CurCode · created_at
```
| `device/forgien/exchange/deposit/account/statement` | POST | ✅ | `currency_id` | كشف بعملة أجنبية (عبر قاعدة بيانات أخرى) |
| `device/Daily_transfer` | POST | ✅ | — | **`{success, datat:{Daily,monthly,Annual,Weekly}}`** — المفتاح `datat` لا `data`. **404 عند الفراغ**. ⚠️ هذه **سقوف** لا استهلاك — جدول `Daily_transfer_preparer_schedule_DEttelse` يعيد الحدّ المسموح (قيم مشاهَدة: 100,000 يومياً و20 مليار سنوياً). **لا يوجد في الـ API أي نقطة تعيد المستهلك**، فلا تُرسم نسبة تقدّم بلا بسط |
| `device/internal/CommtionRetview_get` | POST | ✅ | — | العمولات، `ORDER BY InsertDate DESC`. **404 عند الفراغ** |

### الحوالة الداخلية

| المسار | M | A |
|---|---|---|
| `device/internal/exchange` | POST | ✅ |

الحقول: `country_id`, `reviced_phone`, `reviced_name`, `AccID`, `currency_id`, `amount` (min 1), `branch_id`, `city_id`, `Commition` (min 0) — واختيارياً `SenderName`, `SPhone1`, `Notes`.

> `country_id` و `AccID` **يُتحقق منهما ثم يُهملان** — أرسلهما وإلا فشل التحقق.

خرائط الحقل ← العمود: `Commition`→`ExVal` · `amount`→`OverallVal` · `branch_id`→`BranchDeliveredID` **و** `BBRANCHID` · `city_id`→`DeliveryPlace` · `currency_id`→`RecievedCurrencyID` **و** `DeliveredCurrencyID` · `reviced_phone`→`RPhone1`.

فترة تهدئة: الرسالة تقول «3 دقائق» لكن الفحص `< 1` دقيقة ([:2378](../backend/app/Http/Controllers/Api/depositController.php:2378)). **اعرض رسالة الخادم، لا تثبّت المدة.**

| المسار | M | A | الغرض |
|---|---|---|---|
| `exchange/InternalEx_SelectType_View_not_coustmers_get` | POST | ✅ | الحوالات الواردة لفرع الوكيل (view خاص بـ `UeserType=3`). **200 بمصفوفة فارغة** |
| `exchange/InternalEx_SelectType_View_statetosForok` | POST | ✅ | **ليست الصادرة** — سِجل ما سلّمه هذا الحساب. انظر أدناه |
| `exchange/InternalEx_costimer` | POST | ✅ | **تسليم حوالة** — `Code`, `Notes`. داخل معاملة مع `lockForUpdate`. 409 إن سُلّمت |

### الحوالة الخارجية

| المسار | الحقول |
|---|---|
| `device/service/external/transfer` | `country_id` → أنواع الخدمة المتاحة |
| `device/external/get/exchange` | `currency_id`, `amount`, `service_type`, `type` (1\|2\|3), `Type_From` → `{sale_price, commission_value, buy_price}`. **`sale_price` يُصفَّر إلا إن كان `type==3`** |
| `device/external/insert/transfer` | `RecievedCurrencyID, CountryIDFrom, RecievedBranchID, RecievedName, RPhone1, CityIDTo, DeliveredCurrencyID, CountryIDTo, ServiceType, CurrRecievedVal, AccFrom, IsPrivateAccount, Commition` + اختياري `OwnAccNo, SenderName, SPhone1, Notes` |

> `AccFrom` مطلوب في التحقق ثم **يُرمى** — الإدراج يستخدم `$user->AccID` ([:3034](../backend/app/Http/Controllers/Api/depositController.php:3034)).
> الرد يحوي **صف `ExternalEx` الخام كاملاً** — شكله غير مضبوط من التطبيق.

#### 🔴 `sale_price` من `get/exchange` **ليس ما يستلمه المستفيد**

المُشغِّل `ExternalEx_insert_Mobile` هو من يحسب أرقام الحوالة بعد الإدراج:

```
SalePrice        ← NewCurrencyPriceOwnDetailsTb (CurrencyIDFrom=1, PriceType=2, AccountType=3, CountryID=CountryIDTo)
CurrDeliveredVal ← CurrRecievedVal × SalePrice
NetTotal         ← dbo.SalePrice_mo_Value(CountryIDTo, CurrRecievedVal, ServiceType, IsPrivateAccount)   ← ما يستلمه المستفيد
ServiceExVal     ← CurrDeliveredVal − NetTotal
```

بينما `externalGetExchnage` تستدعي **نفس الدالة بوسيط أول مختلف**:
`SalePrice_mo_Value($currency_id, …)` بدل `SalePrice_mo_Value(CountryIDTo, …)`.
الفارق ليس تقريبياً — مقيس على قاعدة حيّة، مصر، `ServiceType=1`:

| المبلغ (د.ل) | `NetTotal` الفعلي | `sale_price` من النقطة |
|---|---|---|
| 5 | **19** | 2 |
| 100 | **570** | 240 |
| 1000 | **5740** | 2470 |

الصف المُدرَج فعلاً بمبلغ 5 د.ل سجّل `NetTotal = 19` — أي أن رقم النقطة خاطئ لا مختلف الوحدة.
عرضه للوكيل يعني **تسعيراً خاطئاً للزبون**. لا يُستعمل.

| المسار | الحقول |
|---|---|
| `device/external/quote` ✨ | `CountryIDTo, CurrRecievedVal, ServiceType, IsPrivateAccount` → `{TransPrice, CurrDeliveredVal, NetTotal, ServiceExVal, DeliveredCurrency:{ID,CurCode,CuName}}` |

نقطة مُضافة، **قراءة فقط**، تكرّر حساب المُشغِّل حرفياً. أي تعديل في المُشغِّل يجب أن يُنقل إليها.

> وبما أن المُشغِّل يحسب بعد الإدراج و`transInsertExternal` يعيد الصف **بعده**،
> فرد الإنشاء يحمل `TransPrice`/`NetTotal`/`CurrDeliveredVal` الحقيقية — تُعرض في شاشة النجاح.
> `buy_price` من النقطة القديمة غير موثوق أصلاً: استعلامه يمرّر `currency_id` في موضع
> `b.CountryID` و`service_type` في موضع `b.BankID`، فلا يجد صفاً ويعيد الافتراضي `1`.

### التحويل بين الحسابات

| المسار | الحقول |
|---|---|
| `device/exchange/account` | `phone` → بيانات المستفيد `[{user_id,AccName,AccCode,BName,AccID,AccPhone}]`. 404 إن لم يوجد. **مطابقة حرفية** — انظر أدناه |
| `device/internal/trans/between/accounts` | `acc_id, acc_id_to, currency_id, amount (min .01), branch_id` + `Notes`. تهدئة 3 دقائق **مطبَّقة فعلاً** |
| `device/internal/trans/between/accounts/commission` ✨ | `amount` → `{amount, commission, total, matched}`. 422 حين لا تطابق شريحة |
| `device/exchange/accounts/data` | GET — يتفرع حسب الدور. **404 عند الفراغ** |
| `device/add/user/trans` / `list` / `delete` | ⚠️ `delete` **بلا فحص ملكية (IDOR)** |

#### البحث بالهاتف يطابق النص حرفياً وعمود `phone` غير موحَّد

`ExchangeAcc` تبحث بـ `where('a.phone', $phone)` — مطابقة نصية تامة. وعمود `users.phone`
في القاعدة الحيّة يحمل صيغتين: **291 صفاً بصفر بادئ و68 صفاً بدونه**. فصيغة واحدة تُخفي
حسابات موجودة، بل قد تعيد **حسابات مختلفة تماماً**:

| ما يُرسَل | ما يعود |
|---|---|
| `925093709` | حساب واحد (AccID 993) |
| `0925093709` | ثلاثة حسابات أخرى (1386, 3490, 3492) |

لذلك يستعلم التطبيق **بالصيغتين ويوحّد بـ `AccID`**. ولاحظ أن النقطة **تستبعد المستخدم
الحالي** (`a.id <> $currentUserId`) فبحث الوكيل عن رقمه هو يعيد 404 — سلوك مقصود.

#### العمولة هنا ليست بيد الوكيل — خلافاً للحوالة الداخلية والخارجية

`transInsert` يقرأ العمولة من شرائح `Transfer_commissions` ويتجاهل أي قيمة تُرسَل.
والجدول **فيه ثغرات حقيقية** تُرجع 422 «لم يتم العثور على نسبة العمولة المناسبة»:

- لا شريحة بين **10,000 و11,000**
- لا شريحة بين **15,000 و16,000**
- لا شريحة **فوق 100,000**
- والشريحة 20,000–25,000 تحمل عمولة **2000** بينما جاراتها 160 و250 — يبدو خطأ بيانات (صفر زائد)، والمطابقة تختار الأدنى معرّفاً فـ 20,000 بالضبط تقع في 16,000–20,000 وتُحتسب 160

نقطة `…/commission` المضافة تُعيد **نفس استعلام الإدراج حرفياً**، فيمنع التطبيق الإرسال
قبل أن يصطدم الوكيل بالرفض.

#### فحص الرصيد يُطبَّق على الوكيل هنا — على عكس الحوالة الداخلية

`transInsert` يستثني `UeserType == "5"` **وحده** من فحص المحفظة، فالوكيل (`3`) يخضع له.
وهذا يخالف `InternalExchange` التي تتخطّى الفحص للنوعين معاً.

وحين يفشل الفحص فإن الرد يحمل **`wallet: null`** — لأن الاستعلام نفسه
`wallet::where('Walet','>=',$total)->first()` لا يعيد صفاً إلا حين يكفي الرصيد.
قراءته صفراً تعني عرض «رصيدك 0.000» وهو غير صحيح؛ التطبيق يمتنع عن ذكر رصيد لا يملكه.

#### 🔴 «الصادرة» ليست صادرة — إنها سِجل تسليم

الـ view يرشّح بـ `a.ACCID_FRom = $user->AccID`، و**`ACCID_FRom` لا يكتبه
الإدراج**. يكتبه `InternalEx_costimer` — نقطة **التسليم** — بحساب من سلَّم
([:1343](../backend/app/Http/Controllers/Api/depositController.php:1343)).
ويرشّح كذلك بـ `ConfirmType = 2` (مسلَّمة).

مقيس على القاعدة الحيّة:

| ما قيس | النتيجة |
|---|---|
| صفوف الـ view كلها | 31,309 |
| منها `ACCID_FRom = 0` | 31,168 |
| صفوف حساب وكيل أنشأ حوالات فعلاً (1369) | **0** |
| صفوف الحساب 3351، وكلها `AccFrom = 0` و`Type_Moble_costimer = 1` | 141 |

أي أنها **تاريخ ما سلّمه هذا الحساب**، وبناء شاشة «الصادرة» عليها يعطي قائمة
فارغة إلى الأبد. لعرض ما أرسله الوكيل، المصدر هو الكشف `ExchangeAccData`.

التطبيق يسمّيها «سلَّمتُها» ويعرضها للقراءة فقط.


### نقاط البيع — جوهر إدارة الوكيل

الثلاثة تتطلب `UeserType==3 && AccountType==='Main'`، وإلا **403**.

| المسار | الحقول | الأثر |
|---|---|---|
| `device/AuthorizedUsers_Add` | `Name` (max 200), `phone` (max 9، فريد في `AuthorizedUsers` و`users` معاً) | ينشئ صف `users` بـ `Reg='NO'`, `UeserType=3`, `AccountType='pos'` + صف `AuthorizedUsers`. ثم تمر نقطة البيع بـ `otp/send → initAuth → register` |
| `device/AuthorizedUsers_update` | `ID`, `Name`, `phone`, `IsActive` | ⚠️ **يعيد `Reg` إلى `'NO'` دائماً** ([:349-354](../backend/app/Http/Controllers/Api/depositController.php:349)) — أي تعديل، ولو تبديل `IsActive` فقط، **يُخرج نقطة البيع ويجبرها على إعادة التسجيل**. حذّر المستخدم في الشاشة |
| `device/AuthorizedUsersgetByBranch` | — | كل صفوف **الفرع** لا المرشحة بـ `AccID` — وكلاء نفس الفرع يرون قوائم بعضهم. **404 عند الفراغ** |

### المفضلة والبحث

`device/exchange/Favorites_Table_add` (409 عند التكرار) · `Favorites_Table_delete_from` (200 دائماً) · `Favorites_ALL` (**إجراء مخزن**، أعمدة غير معروفة)
`device/searchPayment` — يرد **النموذج عارياً في الجذر بلا غلاف**

### القوائم المرجعية

| المسار | M | A | ملاحظة |
|---|---|---|---|
| `device/countries` | POST | ✗ | **يستبعد المعرّف المرسَل** — إنه «الدول الأخرى» لا «اجلب دولة» |
| `device/cities` | POST | ✗ | `country_id`, `exclude_city_id` |
| `device/exchange/CoBranch_select_get` | GET | ✅ | الفروع النشطة. خارج الغلاف `{success,message,data}` |
| `device/exchange/AppTerms_get` | GET | ✗ | **نص واحد مدموج** مفصول بـ `\n\n` لا مصفوفة. وقد يعود **فارغاً** و`success = true` — جدول `AppTerms` خالٍ في القاعدة المحلية، فالفراغ حالة عادية لا خطأ |

---

## 4. الغلاف — لا تكتب `ApiResponse<T>` واحداً

الشكل المقصود ([BaseController.php](../backend/app/Http/Controllers/BaseController.php)):
```jsonc
{ "data": <any>, "message": "<string>", "success": true, "key": "SUCCESS" }
{ "success": false, "message": <string|object>, "key": "<string>", "data": <إن وُجد> }
```

`ResponseEnums` يعرّف ستة ثوابت لكن **يُصدَر اثنان فقط**: `SUCCESS` و`INVALID_CREDENTIALS`.
البقية سلسلة فارغة `""`. **لا تبنِ توجيه الأخطاء على `key`** — اعتمد رمز HTTP + `success`.

### الانحرافات التي تكسر نموذجاً مُنمَّطاً

| الانحراف | الموضع |
|---|---|
| **`message` كائن JSON لا نص** — الوسائط معكوسة في `sendError`. وهذا **مسار «الرصيد غير كافٍ»** في مسارات التحويل الثلاثة | `:2431, :2601, :2982` |
| `data` نص لا كائن | `AuthController :166, :269, :278, :288` |
| `success:true` مع HTTP **422** | `:2099` |
| HTTP **403 كحالة عمل طبيعية** | `:1058` |
| HTTP **200 مع `success:false`** | `:989, :1382` |
| بلا غلاف — `{message,status}` | `OtpController::checkOtp` |
| بلا غلاف — نموذج عارٍ أو `null` في الجذر | `searchPayment :79` |
| `datat` بدل `data` | `Daily_transfer :415` |
| جسم 200 **فارغ تماماً** | `Rollback_Branch_Trinsfrim_me` |

**التوصية:** فكّاك متسامح يقرأ `success` (ثم `status`، ثم «2xx = صحيح»)، ويأخذ `message` كـ `dynamic`، ويقرأ الحمولة من `data ?? datat ?? errors ?? الجذر`. **افحص نوع `message` قبل عرضه في `Text`.**

**404 ليست خطأً دائماً** — ست نقاط على الأقل ترد 404 لتعني «لا توجد بيانات». لا تعاملها كخطأ في الـ interceptor.

---

## 5. مزالق أخرى

### أخطاء إملائية حاملة للمعنى — لا تُصحَّح

`UeserType` · `UeserID` · `BrancchID` (في `users`) مقابل `BranchID` (في `AuthorizedUsers`) مقابل `BracnID` (في `Navction_Tb`) · `Navction_Tb` · `reviced_phone` · `RecievedName` · `Commition` · `loge` مقابل `longtite` · `Balnce` · `IsAccpit` · `ISActive` مقابل `IsActive` · `device/dRIVER/…` (**المسار حساس لحالة الأحرف**)

### تنسيق الهاتف مختلف بين النقاط

`login` يريد `9XXXXXXXX` · `register` يقبل 8–15 خانة · `otp/send` ينظّف ثم يطلب `9XXXXXXXX` ويضيف `218` بنفسه · `AuthorizedUsers_Add` يحدّه بـ `max:9`.
**وحّد على `9XXXXXXXX` عند حدود التطبيق ولا ترسل بادئة أبداً.**

### الأنواع

`wallet.Walet` مُعرَّف كـ `double` في Eloquent، لكن مسارات SQL الخام تتجاوز التحويل — قد يصل رقماً أو **نصاً**.
استخدم `double.tryParse(v.toString())` دائماً.

### Pusher

حدث واحد فقط: `NotificationSent` على قناة **عامة** اسمها `notifications`، اسم الحدث `notification.sent`، الحمولة `{"message": "<string>"}`.

⚠️ القناة عامة بلا نطاق — **كل جهاز يستقبل إشعارات كل الفروع**، ومعرّف الفرع **ليس في الحمولة** فلا يمكن الترشيح حتى في العميل.
عاملها كإشارة «حدث تغيير، أعد الجلب» لا كبيانات.

`laravel-echo-server.json` بقايا نظام بث آخر — تُتجاهل.

### أمن — أبلغ فريق الباك اند

1. 🔴 `device/reActivate` — **تجاوز مصادقة كامل**: `user_id` من طلب غير مصادَق يعيد رمزاً صالحاً
2. 🔴 `device/update/password` — خارج `auth:sanctum` وبلا تحقق OTP من الخادم = استيلاء على الحساب
3. 🔴 باب خلفي: تخطي فحص الجهاز لرقم مثبّت `'0916121181'`
4. `delete/user/trans` — IDOR
5. `storeNavction` و `send-notification-vbnet` غير مصادَقتين
6. `APP_DEBUG=true` وبعض معالجات 500 تعيد `line` و`file`
7. `.htaccess` يضع `ExpiresByType application/json A31536000` — **تخزين سنة كاملة على كشوف مالية**
8. لا تحديد معدّل (rate limiting) على أي مسار

### نقطتان معطّلتان

`device/internal/exchange/time/check` و `device/internal/exchange/external/check` تستدعيان `InternalEx_minut` و`checkTtans` — **غير موجودتين**. سترد 500.

### 🔴 كانت الحوالة الداخلية معطّلة كلياً — أُصلحت

`Watsaoserversfrom::sendAgentTransactionMessage()` تستدعي `$this->sendFormaGROUP(...)` وهي **غير معرّفة في الصنف**. النتيجة:

```
500 — Call to undefined method App\Services\Watsaoserversfrom::sendFormaGROUP()
```

وبما أن الاستدعاء **داخل `DB::transaction`**، كانت الحوالة تُدرَج ثم تُلغى بالكامل. أي أن **كل حوالة داخلية ينشئها وكيل (`UeserType==3`) كانت تفشل** — وهي العملية الأساسية للوكيل.

أُجري إصلاحان:

1. **أُضيفت `sendFormaGROUP(string $groupId, string $body)`** إلى الخدمة. ولا تصلح `sendFormasggme` بديلاً: هي تجرّد كل ما ليس رقماً من المُعرّف (`preg_replace('/[^0-9]/','')`)، ومعرّف المجموعة يحمل `-` و`@g.us` فيُدمَّر.

2. **أُخرج الإشعار من المعاملة** — لُفَّ الاستدعاء في `try/catch` مع تسجيل الخطأ. رسالة واتساب لا يجوز أن تُبطل حركة مالية مكتملة. هذا يعالج صنفاً كاملاً: أي عطل في البوابة أو في امتداد `intl` كان يُلغي حوالات ناجحة.

مُختبَر بعد الإصلاح: `InternalEx` 32566 و32567، بالرمزين `0655516` و`0653518`.

> ملاحظة: مسار الوكيل **لا يخصم من `wallet` عند الإنشاء** — بقي الرصيد كما هو. القيد المالي يقع في مرحلة الاعتماد/التسليم.

### مزالق نشر (تظهر على Linux فقط)

- `app/Services/SVSn8n_payments.PHP` بامتداد كبير — PSR-4 لن يجده على نظام حساس لحالة الأحرف ⇒ **كل نقاط `depositController` تسقط**
- `app/Models/BankVisaTransfers.php` يعلن `namespace Modules\...` وهو تحت `app/` ⇒ غير قابل للتحميل
- `SmsController` معرّف مرتين
