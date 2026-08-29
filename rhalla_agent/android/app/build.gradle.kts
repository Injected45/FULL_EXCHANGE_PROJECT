import java.util.Properties

// مفاتيح التوقيع تأتي من android/key.properties — مُستثنى من git.
// انظر buildTypes.release أدناه: بناء إصدار بلا هذا الملف يفشل عمداً.
val keystoreProperties = Properties().apply {
    val f = rootProject.file("key.properties")
    if (f.exists()) f.inputStream().use { load(it) }
}

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
        create("release") {
            keyAlias = keystoreProperties.getProperty("keyAlias")
            keyPassword = keystoreProperties.getProperty("keyPassword")
            storeFile = keystoreProperties.getProperty("storeFile")?.let { file(it) }
            storePassword = keystoreProperties.getProperty("storePassword")
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
            // التوقيع بمفاتيح التصحيح كان يمرّ صامتاً وينتج APK لا يقبله Google Play،
            // والأسوأ أنه يمنع ترقية التطبيق لاحقاً بمفتاح آخر. نفشل بصوت عالٍ بدلاً منه.
            if (keystoreProperties.getProperty("storeFile") == null) {
                throw GradleException(
                    """
                    بناء إصدار بلا مفاتيح توقيع.
                    أنشئ android/key.properties بالمفاتيح: storeFile, storePassword, keyAlias, keyPassword
                    وولّد الـ keystore بـ keytool — التعليمات في CLAUDE.md.
                    المفتاح لا يُستبدل بعد أول نشر: احتفظ به وبكلمة سره خارج الجهاز.
                    """.trimIndent()
                )
            }
            signingConfig = signingConfigs.getByName("release")
        }
    }
}

flutter {
    source = "../.."
}
