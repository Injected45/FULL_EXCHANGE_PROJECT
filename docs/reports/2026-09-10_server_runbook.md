# تعليمات تنفيذ على خادم الرحالة — للمهندس التقنيّ

> أعدّتها لجنة فحص تطبيق الصرافة (10 سبتمبر 2026). نفّذها بالترتيب. مع كلّ
> تعديلٍ حسّاس: **نسخة احتياطية أولاً**، وتحقُّقٌ بعده. توقّف وراجع إن ظهر أيُّ
> خطأ لا يوصف هنا.
>
> الكود المُصلَح مُودَعٌ في المستودع: الإيداعات `10/09-02` حتى `10/09-05`.

---

## P0 — عاجل: إغلاق تسريب المصدر (لا يمكن أن يعطّل الخادم)

### 1) حذف الأرشيف المكشوف
الخادم يعرض `http://102.214.165.242:8080/htdocs.rar` (≈182 م.ب) للتنزيل من أيّ أحد
— وهو على الأرجح نسخةٌ كاملة من المصدر وربّما `.env`. احذفه:

```powershell
# ابحث عنه واحذفه (PowerShell على الخادم)
Get-ChildItem C:\ -Recurse -Filter htdocs.rar -File -EA SilentlyContinue |
  ForEach-Object { "$($_.FullName)  ($([math]::Round($_.Length/1MB,1)) MB)"; Remove-Item $_.FullName -Force }
```
**تحقّق:** افتح `http://102.214.165.242:8080/htdocs.rar` ← يجب أن يظهر **Not Found**.

### 2) إطفاء فهرسة المجلّدات — **نسخة احتياطية أولاً**
الجذر يعرض قائمة `app/`، `routes/`، إلخ. السبب غالباً `Options Indexes` في
`httpd.conf` أو `AllowOverride None` يُبطل `.htaccess`.

```powershell
# انسخ إعداد Apache احتياطياً قبل أيّ تعديل
Copy-Item C:\xampp\apache\conf\httpd.conf C:\xampp\apache\conf\httpd.conf.bak-20260910
```
- في `httpd.conf` أزل كلمة `Indexes` من سطر `Options Indexes FollowSymLinks` لكتلة
  الجذر (تصير `Options FollowSymLinks`)، **أو** اجعل `AllowOverride All` لتُفعَّل
  `.htaccess` (المستودع يحمل `Options -Indexes` أصلاً في `.htaccess`).
- أعِد تشغيل Apache: `C:\xampp\apache\bin\httpd.exe -k restart` (أو من لوحة XAMPP).

**تحقّق:** `http://102.214.165.242:8080/app/` ← يجب أن يردّ **403** لا قائمة.
إن توقّف Apache: أعِد `httpd.conf.bak-20260910` وأعد التشغيل.

### 3) (الأفضل) جذر الويب = `public/` فقط
اجعل `DocumentRoot` يشير إلى `...\backend\public` بدل جذر مشروع Laravel — يُخفي
`app/` و`routes/` و`.env` نهائياً. عدّل `DocumentRoot` و`<Directory>` في `httpd.conf`،
واحفظ نسخةً احتياطية أولاً كما في (2).

---

## P1 — نشر إصلاحات الكود الحرجة

### 4) جلب الكود المُصلَح على الخادم
```bash
cd <مجلد المشروع على الخادم>/backend
git pull            # أو ارفع الملفّات المعدّلة يدوياً إن لم يكن git على الخادم
php artisan config:clear
```
الملفّات المعدّلة (إن كان الرفع يدوياً): `app/Http/Controllers/Api/` (AuthController,
depositController, OtpController, EmployeeController), `app/Services/Employees/`
(EmployeeCashboxService, EmployeeActivationService), `config/cache.php`,
`config/services.php`, `routes/api.php`.

### 5) ضبط `.env` على الخادم
```env
APP_DEBUG=false
APP_ENV=production
CUSTOM_X_TOKEN=fe1068000c495fa96c4d9f67978a1a2df73455ab3c9d2890ef70a16302086168
CACHE_STORE=file
```
- `APP_DEBUG=false`: يمنع تسريب مسارات النظام وكلمة القاعدة عند أيّ خطأ.
- `CUSTOM_X_TOKEN`: السرّ الذي يحرس `reActivate` (انظر §7).
- `CACHE_STORE=file`: تقييدُ المعدّل (throttle) يكتب في ملفّ لا في قاعدة الإنتاج.
ثم: `php artisan config:clear` (لا `config:cache` إلّا إن كنت تستعمله عادةً).

### 6) تطبيق سكربت قاعدة البيانات (فهرس الخزينة — M10)
شغّل على SQL Server قاعدة `EXCHANGESYS2026`:
`backend/database/sql/deploy/2026-09-10_cashbox_reference_index.sql`
(يُسقط فهرس `UX_entry_reference` القديم ويُنشئه على `(agent_id, reference_type,
reference_id)`. توسيعٌ آمن، جدولٌ تشغيليّ لا محاسبيّ.) **يُطبَّق مع الكود في §4.**

---

## P2 — إغلاق C-01 (نشرٌ منسّق: الخادم + المكتبيّ)

### 7) بعد §5 (السرّ مضبوط)، حدّث المكتبيّ
مسار `device/reActivate` صار يرفض أيّ طلبٍ بلا `xtoken`. فالمكتبيّ يجب أن يرسله:
1. في `RhallaConfig.ini` بجانب المكتبيّ أضف — **بنفس قيمة `CUSTOM_X_TOKEN`**:
   ```
   API_X_TOKEN=fe1068000c495fa96c4d9f67978a1a2df73455ab3c9d2890ef70a16302086168
   ```
2. أعِد بناء المكتبيّ من الكود المُصلَح (`deploy_new_build.bat`) ووزّعه على أجهزة
   الفروع التي تستعمل شاشة «إعادة تفعيل حساب».
3. انشر الخادم (§4–5) والمكتبيّ **معاً** — أو انشر الخادم أولاً وتقبّل توقّف زرّ
   «إعادة التفعيل» (وظيفة نادرة) حتى يصل المكتبيّ المُحدَّث.

**تحقّق:** بعد النشر، إعادةُ التفعيل من المكتبيّ تعمل؛ وطلبُ `reActivate` بلا
`xtoken` من الخارج يردّ **401**.

---

## P3 — تدوير الأسرار (لأنّ الأرشيف كان مكشوفاً منذ أكتوبر)

### 8) دوّر كلّ سرٍّ قد يكون في الأرشيف المكشوف
- كلمة مرور قاعدة البيانات (مستخدم `sa`).
- مفاتيح Twilio و Pusher.
- `WHATSAPP_TOKEN` لبوّابة واتساب.
- `CHAT_ADMIN_KEY` — أو اتركه فارغاً في `.env` (يُغلق الباب الخلفيّ `/admin/chat`).
حدّث القيم الجديدة في `.env` الخادم و`RhallaConfig.ini` المكتبيّ حيث تلزم.

---

## ملخّص الأولويات
| # | البند | الخطر لو تُرك | يعطّل الخادم لو أُخطئ؟ |
|---|---|---|---|
| 1 | حذف htdocs.rar | تنزيل كامل المصدر | لا — آمن |
| 2 | إطفاء الفهرسة | كشف بنية المصدر | نعم إن أُخطئ — خذ نسخة احتياطية |
| 4-6 | نشر الكود + السرّ + الفهرس | ثغرات مالية مغلقة في الكود لكنها غير منشورة | لا (نشرٌ عاديّ) |
| 7 | تحديث المكتبيّ لـ C-01 | استيلاء على أيّ حساب عبر reActivate | لا (يوقف زرّاً نادراً مؤقتاً) |
| 8 | تدوير الأسرار | أسرارٌ مكشوفة قابلة للاستغلال | لا |
