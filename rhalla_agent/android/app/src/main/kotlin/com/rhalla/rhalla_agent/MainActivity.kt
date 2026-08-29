package com.rhalla.rhalla_agent

import android.provider.Settings
import io.flutter.embedding.android.FlutterActivity
import io.flutter.embedding.engine.FlutterEngine
import io.flutter.plugin.common.MethodChannel

/**
 * قناة واحدة فقط: مُعرّف عتاد ثابت.
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
                    else -> result.notImplemented()
                }
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
