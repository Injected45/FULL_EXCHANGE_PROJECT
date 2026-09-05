package com.rhalla.rhalla_agent

import android.media.RingtoneManager
import android.provider.Settings
import io.flutter.embedding.android.FlutterActivity
import io.flutter.embedding.engine.FlutterEngine
import io.flutter.plugin.common.MethodChannel

/**
 * قناة الجهاز: مُعرّف عتاد ثابت، ورنّة إشعار.
 *
 * الخادم يربط جهازاً واحداً بكل مستخدم ويرفض أي مُعرّف آخر، ولا يُستعاد
 * الحساب إلا بإعادة تعيين Reg='NO' من المكتب الخلفي. ومُعرّف عشوائي مخزَّن
 * في التطبيق يضيع مع أول إعادة تثبيت، فيُقفل الحساب.
 *
 * ANDROID_ID يبقى عبر إعادة التثبيت ولا يتغيّر إلا بإعادة ضبط المصنع.
 * وهو مُقيَّد بتوقيع التطبيق والمستخدم منذ Android 8، فلا يُشارَك بين التطبيقات.
 */
class MainActivity : FlutterActivity() {

    private val channel = "com.rhalla.rhalla_agent/device"

    override fun configureFlutterEngine(flutterEngine: FlutterEngine) {
        super.configureFlutterEngine(flutterEngine)

        MethodChannel(flutterEngine.dartExecutor.binaryMessenger, channel)
            .setMethodCallHandler { call, result ->
                when (call.method) {
                    "hardwareId" -> result.success(hardwareId())
                    "notificationSound" -> { playNotification(); result.success(null) }
                    else -> result.notImplemented()
                }
            }
    }

    /**
     * نغمة الإشعار التي اختارها صاحب الجهاز لنفسه.
     *
     * ولا ملف صوت مضمّن في التطبيق: نغمةٌ من عندنا تكبّر الـ APK وتأتي
     * بصوتٍ غريب على أذن المستخدم، بينما هذه هي النغمة التي تعلّم أن يلتفت
     * إليها. وهي تحترم وضع الصامت وحجمَ صوت الإشعارات، وهو الصواب — تطبيق
     * يرنّ في اجتماعٍ رغم إسكات الهاتف يُغلَق إشعارُه ولا يُسمَع بعدها.
     *
     * والفشل يُبتلع: تعذُّر الرنّة لا يبرّر إسقاط التنبيه، فالعدّاد
     * والاهتزاز قائمان.
     */
    private fun playNotification() {
        try {
            val uri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION)
                ?: return
            RingtoneManager.getRingtone(applicationContext, uri)?.play()
        } catch (e: Throwable) {
            // لا شيء: انظر التوثيق أعلاه.
        }
    }

    private fun hardwareId(): String? =
        try {
            val id = Settings.Secure.getString(contentResolver, Settings.Secure.ANDROID_ID)
            // "9774d56d682e549c" علّة معروفة في أجهزة قديمة تعيده لكل الأجهزة.
            if (id.isNullOrBlank() || id == "9774d56d682e549c") null else id
        } catch (e: Throwable) {
            null
        }
}
