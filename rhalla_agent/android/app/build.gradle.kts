import java.util.Properties

// مفاتيح التوقيع تأتي من android/key.properties — مُستثنى من git.
// انظر buildTypes.release أدناه: بناء إصدار بلا هذا الملف يفشل عمداً.
val keystoreProperties = Properties().apply {
    val f = rootProject.file("key.properties")
    if (f.exists()) f.inputStream().use { load(it) }
}

val hasSigningKeys = keystoreProperties.getProperty("storeFile") != null

plugins {
    id("com.android.application")
    id("kotlin-android")
    // The Flutter Gradle Plugin must be applied after the Android and Kotlin Gradle plugins.
    id("dev.flutter.flutter-gradle-plugin")
}

android {
    namespace = "com.rhalla.rhalla_agent"
    compileSdk = flutter.compileSdkVersion
    ndkVersion = flutter.ndkVersion

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }

    kotlinOptions {
        jvmTarget = JavaVersion.VERSION_17.toString()
    }

    signingConfigs {
        // لا يُنشأ إلا حين توجد المفاتيح فعلاً — إعداد توقيع نصفه فارغ
        // أسوأ من غيابه، لأنه يفشل متأخّراً وبرسالة غامضة.
        if (hasSigningKeys) {
            create("release") {
                keyAlias = keystoreProperties.getProperty("keyAlias")
                keyPassword = keystoreProperties.getProperty("keyPassword")
                storeFile = keystoreProperties.getProperty("storeFile")?.let { file(it) }
                storePassword = keystoreProperties.getProperty("storePassword")
            }
        }
    }

    defaultConfig {
        applicationId = "com.rhalla.rhalla_agent"
        // You can update the following values to match your application needs.
        // For more information, see: https://flutter.dev/to/review-gradle-config.
        minSdk = flutter.minSdkVersion
        targetSdk = flutter.targetSdkVersion
        versionCode = flutter.versionCode
        versionName = flutter.versionName
    }

    buildTypes {
        release {
            // بلا مفاتيح: لا نُسنِد إعداد توقيع إطلاقاً. الحارس أدناه يمنع
            // خروج نسخة إصدار غير موقّعة، وnull هنا أوضح من مفاتيح التصحيح.
            signingConfig = if (hasSigningKeys) signingConfigs.getByName("release") else null
        }
    }
}

// التوقيع بمفاتيح التصحيح كان يمرّ صامتاً وينتج APK لا يقبله Google Play،
// والأسوأ أنه يمنع ترقية التطبيق لاحقاً بمفتاح آخر. نفشل بصوت عالٍ بدلاً منه.
//
// الحارس هنا لا داخل buildTypes.release عمداً: كتل buildTypes تُقيَّم في وقت
// الإعداد لكل بناء، فرمي الاستثناء داخلها كان يُفشل assembleDebug أيضاً —
// أي أنه منع بناء أي APK على جهاز بلا keystore، وهو ما لم يكن مقصوداً.
// taskGraph.whenReady يؤجّل الفحص إلى ما بعد تحديد المهام المطلوبة فعلاً.
if (!hasSigningKeys) {
    gradle.taskGraph.whenReady {
        val wantsRelease = allTasks.any {
            it.name.matches(Regex("^(assemble|bundle|package).*Release$"))
        }
        if (wantsRelease) {
            throw GradleException(
                """
                بناء إصدار بلا مفاتيح توقيع.
                أنشئ android/key.properties بالمفاتيح: storeFile, storePassword, keyAlias, keyPassword
                وولّد الـ keystore بـ keytool — التعليمات في CLAUDE.md.
                المفتاح لا يُستبدل بعد أول نشر: احتفظ به وبكلمة سره خارج الجهاز.
                """.trimIndent()
            )
        }
    }
}

flutter {
    source = "../.."
}
