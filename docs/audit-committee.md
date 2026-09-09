# لجنة فحص تطبيق الصرافة — الميثاق الدائم

> **الاستدعاء**: حين يكتب المالك «لجنة فحص تطبيق الصرافة» ولا شيءَ غيرها، فذلك
> أمرٌ بتشغيل هذا الفحص كاملاً — بنفس الأعضاء والنطاق والقواعد — على **أحدث**
> حالةِ الكود، ودون إعادة شرحٍ منه. تُقرأ أحدثُ متطلبات Apple و Google و OWASP
> و NIST و PCI وقتَ الانعقاد، لا ما كان صحيحاً في انعقادٍ سابق.

## طبيعة العمل — وهذا هو الحدّ الفاصل

اللجنة **تفحص وتُصلح نظاماً قائماً**، ولا تُعيد بناءه.

| الحالة | الإجراء |
|---|---|
| موجودٌ سليم | يُحافَظ عليه كما هو |
| موجودٌ يحتاج تحسيناً | يُطوَّر |
| خلل | يُصلَح، ثم يُعاد اختباره |
| غيرُ ضروريّ | يُزال **بعد** إثبات انعدام التبعية والأثر الجانبي |

ممنوع: إعادةُ البناء من الصفر · نظامٌ موازٍ · تغييرُ معماريةٍ مستقرّة بلا سببٍ
تقنيّ موثَّق · Refactoring للتجميل. المطلوب: **Inspect · Test · Harden ·
Optimize · Fix · Retest** — لا Rewrite.

## القاعدة السيادية المالية

يمنع منعاً باتاً المساسُ بالشجرة المحاسبية أو بالأرصدة أو القيود أو المدين
والدائن أو العمولات وطرق احتسابها أو التسويات أو الخزائن أو الترحيلات أو
الحسابات أو منطق الحوالات المالي أو أيّ Financial Business Logic قائم.

**خبير المحاسبة يفحص ويقارن فقط.** وإن اكتشف خللاً ماليّاً سجّله
**Critical Finding** ولم يغيّره إلّا بموافقةٍ صريحة من المالك.

## السرّية

المشروع كلُّه **CONFIDENTIAL**. يمنع رفعُ الشيفرة إلى خدمةٍ عامّة غير معتمدة،
أو إرسالُ بيانات عملاء أو أسرار أو محتوى E2EE إلى طرفٍ ثالث، أو نسخُ بيانات
الإنتاج. الاختبارات ببياناتٍ اختبارية أو مُقنَّعة. ولا يُطبع أيُّ سرٍّ كاملاً في
التقرير. ويمنع الاختبارُ التدميريّ على الإنتاج أو على بيانات عملاء حقيقية.

## الأعضاء والنطاق

### ١) خبير Apple و iOS
يفحص كأنّ النسخة تُرفع الآن إلى App Store Review: توافقُ App Store Review
Guidelines · جودةُ نسخة iOS · Crashes · Freezes · Memory · Background
Behavior · Permissions · Privacy · Authentication · Biometrics · Deep Links ·
Notifications · Camera · Microphone · QR · Device Binding · إصداراتُ iOS
المستهدفة · App Metadata · Privacy Disclosures · App Privacy · حسابُ
المراجعة/Demo عند الحاجة · عملُ الخادم أثناء المراجعة · وكلُّ سببٍ محتملٍ للرفض.
**الحكم**: `READY FOR APP STORE REVIEW` أو `NOT READY`، مع الأسباب بدقّة.

### ٢) خبير Google Play و Android
Play Policies · Technical Quality · Target API · Permissions والحسّاسُ منها ·
Data Safety · اتّساقُ سياسة الخصوصية · Financial Features Declaration ·
متطلباتُ المطوِّر/المنظّمة للتطبيقات المالية · Deep Links · Notifications ·
Background Services · Device & Android Compatibility · Crashes · ANRs ·
Battery · App Bundle · App Signing · Play Integrity عند الحاجة.
**الحكم**: `READY FOR GOOGLE PLAY` أو `NOT READY`.

### ٣) خبير الأمن السيبراني واختبار الاختراق
مرجعُه OWASP MASVS و MASTG و NIST SSDF. يفحص على الأقل: Authentication ·
Authorization · OTP · QR Activation · Device Binding · Session Management ·
Token Security · API Security · IDOR / Broken Access Control · Privilege
Escalation · Tenant Isolation · عزلُ الموظف عن الوكيل · فصلُ الإدارة · Replay ·
Race Conditions · Duplicate Requests · Rate Limiting · Brute Force · Deep Link
Security · Input Validation · Injection · Sensitive Data Exposure · Local
Storage · Secure Storage · Keychain/Keystore · التشفيرُ نقلاً وسكوناً · E2EE
حيث يُستعمل · TLS · Secrets Management · Logging · Debug Info · Crash Logs ·
Backup Exposure · Clipboard · Screenshots / App Switcher · Root/Jailbreak ·
Third-Party SDKs · Dependencies · CVEs · Supply Chain · Hardcoded Secrets ·
API Keys · Certificates · Expired Libraries · Misconfigurations · Push
Privacy · Voice/Media Storage · WebSocket · وكلُّ Attack Surface قائمٍ فعلاً.

التصنيف: `CRITICAL` · `HIGH` · `MEDIUM` · `LOW` · `INFORMATIONAL`.
ولا تنجح اللجنة مع ثغرةِ Critical أو High غير معالجة. وبعد كلّ إصلاح **Retest**
للتأكّد من الإغلاق ومن انعدام Regression.

### ٤) خبير الخصوصية وحماية بيانات العملاء
دورةُ حياة البيانة كاملة: ماذا يُجمع · لماذا · أين يُحفظ · إلى أين يُرسل · من
يصل إليه · مدّةُ الاحتفاظ · متى يُحذف · هل هناك زائدٌ عن الحاجة · هل تجمع
SDKs شيئاً إضافياً · هل يطابق إفصاحُ Privacy و Data Safety السلوكَ الحقيقيّ.
المبادئ: Data Minimization · Least Privilege · Need To Know.
ولا بياناتٍ حسّاسة في: Logs · Analytics · Crash Reports · Notifications ·
URLs · Clipboard · Temporary Files.

### ٥) خبير المحاسبة وسلامة المنظومة المالية — **Audit فقط**
يتحقّق أنّ التطبيق **لا يملك نظاماً محاسبياً منفصلاً** عن منظومة الرحالة:
الأرصدةُ من النظام وقاعدته المعتمدة · حالاتُ الحوالات من الجداول المعتمدة ·
لا عملياتٍ ماليّة تُنشأ محلّياً · الموظف ليس طرفاً ماليّاً مستقلاً عن الوكيل ·
عملياتُه تُنسب تشغيلياً له وماليّاً للوكيل · لا حسابات مكرّرة في العميل تخالف
الخادم · لا Shadow Accounting · لا Duplicate Financial Logic · لا Offline
Financial Execution · ولا يُنشئ انقطاعُ الشبكة أو Retry أو الضغطُ المتكرّر
عمليةً مكرّرة.
يقارن عيّناتٍ End-to-End بين التطبيق و API والخادم وقاعدة البيانات والنتيجة
المالية النهائية. **وأيُّ اختلافٍ Critical.**

### ٦) فريق مراجعة الكود
Senior/Principal في: Mobile · Financial Systems · Backend · API · Databases ·
Security · Performance · React · iOS · Android. يراجعون:
Correctness · Readability · Maintainability · Modularity · Cohesion ·
Coupling · Error Handling · Concurrency · Async · Race Conditions · Memory
Leaks · Dead Code · Duplicated Code · Unused Libraries/Assets · Unreachable
Functions · Debug Code · Workarounds · Hardcoded Values · TODOs · Mocks ·
Placeholder Logic · Exceptions · Transaction Boundaries · Queries · Indexes ·
API Calls · Caching · Retry · Real-Time · Resource Cleanup.
ويمنع Refactoring يزيد المخاطر بلا فائدة حقيقية.

### ٧) الأداء والحجم
سرعةُ التشغيل والفتح والتنقّل و API واستجابةِ الواجهات · الذاكرة · المعالج ·
البطارية · الشبكة · حجمُ التطبيق · زمنُ Startup · حجمُ Assets و Dependencies.
ولا يُحذف Component قبل إثبات أنّه غير مستعمل. **قياسٌ قبل وبعد.**

### ٨) اختبارات الاستقرار
Unit · Integration · E2E · Regression · API · DB Integration · Authentication ·
Authorization · Network Failure · Offline · Timeout · Retry · Concurrency ·
Duplicate Prevention · Background/Foreground · App Restart · Low Memory ·
Notification · Device Binding · OTP · QR · Employee Permission · Financial
Isolation.

### ٩) Supply Chain
لكلّ Dependency/Package/SDK: الاسم · الإصدار · سببُ الاستعمال · هل ما زال
مستعملاً · ثغرةٌ معروفة · قِدَمٌ خطر · بديلٌ أخفّ أو رسميّ عند الضرورة · غيرُ
المستعمَل. ويُنشأ SBOM أو ما يعادله إن سمحت البيئة.
**ولا تحديثَ أعمى لمكتبةٍ رئيسية** — كلُّ تحديثٍ يستوجب Regression Testing.

### ١٠) Secrets and Credentials Audit
API Keys · Passwords · Tokens · Private Keys · Certificates · DB Credentials ·
Hardcoded Secrets · أسرارُ Git History · Environment Files · Build Configs.
ولا يُطبع سرٌّ كاملاً. وإن وُجد مكشوفاً: يُصنَّف، ويُنقل إلى تخزينه الصحيح،
ويوصى بتدويره أو يُدوَّر إن توفّرت الصلاحية الآمنة.

### ١١) Threat Modeling
الأصولُ الحسّاسة · الأطراف · Trust Boundaries · Entry Points · Attack
Surfaces · أعلى سيناريوهات إساءة الاستخدام. بتركيزٍ على: حسابِ الوكيل ·
حسابِ الموظف · Admin · QR Activation · OTP · Device Binding · الحوالات ·
Approvals · الدردشة · E2EE · تطبيقِ الدعم · APIs · قاعدةِ البيانات · Push.

### ١٢) Release Gate
يمنع إصدارُ `READY FOR PRODUCTION` مع وجود: Critical · High غير مقبولٍ رسمياً ·
Data Leakage · Broken Authorization · Accounting Mismatch · Duplicate
Financial Execution · Crash متكرّر · Store Blocking Issue · Hardcoded
Production Secret · أو أيّ خللٍ يهدّد أموال العملاء أو بياناتهم.

## آلية الإصلاح

كلُّ خبير: **يفحص · يوثّق · يحدّد الشدّة · يحدّد Root Cause · يُصلح إن كان
الإصلاح آمناً وضمن النطاق · يُعيد الاختبار · يُثبت النتيجة.**

وما كان عالي المخاطر، أو يمسّ النظام المالي، أو يستوجب قراراً تجارياً أو
قانونياً، أو يغيّر المعمارية جوهرياً — **لا يُنفَّذ تلقائياً**، بل يُرفع إلى
المالك كقرارٍ مطلوب مع التوصية.

## المراجع الإلزامية

Apple App Store Review Guidelines · Apple Privacy Requirements · Google Play
Developer Policies · Play Technical Quality · Play Data Safety · Play
Financial Services Requirements · OWASP MASVS · OWASP MASTG · NIST SSDF ·
PCI DSS و PCI SSF عند انطباقهما. ولا يُعتمد قديمٌ مع توفّر أحدث.

## التقرير

مخرَجٌ **واحد** باسم **«تقرير لجنة فحص تطبيق الصرافة»**، مختصرٌ دقيقٌ بلا
إنشاء، **Evidence-Based Findings فقط**، يحوي: الحكمَ العام · App Store
Readiness · Google Play Readiness · Security · Privacy · Accounting
Integrity · Code Quality · Performance · Test Results · أعدادَ
Critical/High/Medium/Low · ما أُصلح · ما بقي · ما يحتاج قرارَ المالك · والحكمَ
النهائي: `READY FOR PRODUCTION` أو `CONDITIONALLY READY` أو `NOT READY` مع
أسبابه.

## تعريف النجاح

لا تنجح اللجنة لأنّها انتهت. تنجح فقط حين: لا Critical غيرَ معالجة · لا High
غيرَ معالجةٍ أو مقبولةٍ رسمياً · لا أخطاءَ محاسبية · لا عملياتٍ ماليّة منفصلة
عن النظام الأصلي · لا Duplicate Financial Transactions · لا تسريبَ بيانات ·
لا أسرارَ مكشوفة · لا مانعَ من Play أو App Store · الاختباراتُ الحرجة ناجحة ·
الكودُ مستقر · والتطبيق جاهزٌ فعلاً.

**ولا يُفترض أنّ ميزةً سليمةٌ لأنّها تعمل بصرياً** — تُختبر من الواجهة إلى
الخادم و API وقاعدة البيانات والصلاحيات والأمن والنتيجة النهائية.
المطلوب أعلى **جودة** ملاحظات لا أعلى **عدد**، وأدقُّ إصلاحٍ بأقلّ تغييرٍ آمن.

## سجلّ الانعقادات

| التاريخ | الحكم | Critical | High | التقرير |
|---|---|---|---|---|
| 10 سبتمبر 2026 | `NOT READY FOR PRODUCTION` | 10 | ~28 | [2026-09-10](reports/2026-09-10-audit-committee.md) — أُصلح ١٢ بنداً آمناً؛ الباقي قرارُ المالك |
