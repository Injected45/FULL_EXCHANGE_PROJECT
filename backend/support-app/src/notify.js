/**
 * التنبيه عند وصول رسالة — صوتٌ وإشعارُ متصفّحٍ وعنوانُ لسان.
 *
 * ── لماذا هذا مجّانيّ بالكامل ────────────────────────────────────────────
 *
 * أمرُ المالك: «ألغِ كل ما يحتاج اشتراكاً شهرياً ومصاريف، وأنشئ كل شيء
 * مجّاني ومتاح مثل الإشعارات». وهذا ما يجعل الويب أسهل من الهاتف هنا:
 *
 * - **الإشعار**: `Notification` API — مبنيٌّ في المتصفّح، بلا Firebase ولا
 *   خدمةِ دفعٍ ولا حسابٍ يُفتح. يعمل بلا إنترنتٍ خارجيّ أصلاً.
 * - **الصوت**: يُولَّد بـ `Web Audio` لحظةَ الحاجة — **لا ملفّ صوتٍ يُحمَّل**.
 *   نغمتان قصيرتان (880 ثم 1174 هرتز) بمنحنى خفوتٍ يمنع الطقطقة.
 *
 * ولذلك لا شيء هنا يحتاج شبكةً ولا اشتراكاً ولا يزيد حجم الصفحة بايتاً.
 *
 * ── وثلاثة قرارات في السلوك ──────────────────────────────────────────────
 *
 * 1. **أوّل استطلاعٍ لا يُصدر صوتاً أبداً.** هو يُثبّت الأساس؛ ورسائلُ كانت
 *    موجودةً قبل فتح الصفحة يعلنها العدّاد صامتاً. ورنينٌ لها يُدرّب الموظّف
 *    على تجاهل الرنين.
 * 2. **لا رنينَ للمحادثة المفتوحة أمامه.** الرسالة أمام عينيه، والصوتُ لها
 *    إزعاجٌ لا إفادة.
 * 3. **الإذنُ يُطلب بضغطةٍ من الموظّف لا عند فتح الصفحة.** المتصفّحات ترفض
 *    الطلبَ التلقائي وقد تحظره للأبد، والحظرُ لا يُرفع إلا من إعدادات
 *    الموقع — وهو ما لن يجده أحد.
 */

let ctx = null

/** نغمةٌ مولَّدة — بلا ملفّ ولا حزمة. */
export function beep() {
  try {
    if (!ctx) {
      const AC = window.AudioContext || window.webkitAudioContext
      if (!AC) return
      ctx = new AC()
    }
    // المتصفّح يوقف السياق حتى أول تفاعل — استئنافُه رخيصٌ وآمن.
    if (ctx.state === 'suspended') ctx.resume()

    const now = ctx.currentTime
    const play = (freq, at, dur) => {
      const osc = ctx.createOscillator()
      const gain = ctx.createGain()
      osc.type = 'sine'
      osc.frequency.value = freq
      // منحنى خفوت: قطعُ الموجة فجأةً يُسمع طقطقةً في السماعات.
      gain.gain.setValueAtTime(0.0001, now + at)
      gain.gain.exponentialRampToValueAtTime(0.16, now + at + 0.012)
      gain.gain.exponentialRampToValueAtTime(0.0001, now + at + dur)
      osc.connect(gain).connect(ctx.destination)
      osc.start(now + at)
      osc.stop(now + at + dur + 0.02)
    }
    play(880, 0, 0.12)
    play(1174.66, 0.13, 0.16)
  } catch {
    /* بلا صوتٍ خيرٌ من شاشةٍ تتعطّل. */
  }
}

export function notificationState() {
  // داخل الغلاف لا إذنَ يُطلب: التنبيه من النظام لا من المتصفّح، فيُبلَّغ
  // أنه ممنوح — وإلا ظلّ زرّ «تفعيل الإشعارات» ظاهراً في التطبيق يطلب إذناً
  // لا وجود له.
  if (native()) return 'granted'
  if (!('Notification' in window)) return 'unsupported'
  return Notification.permission
}

/** يُنادى من زرٍّ يضغطه الموظّف — انظر القرار 3 أعلاه. */
export async function askPermission() {
  if (!('Notification' in window)) return 'unsupported'
  try {
    return await Notification.requestPermission()
  } catch {
    return Notification.permission
  }
}

/**
 * هل نحن داخل غلاف الهاتف؟
 *
 * ⚠ `WebView` على أندرويد **لا تدعم `Notification` API إطلاقاً** — لا
 * ترفضها بإذن، بل لا توجد. فلولا هذا الجسر لصمتت اللوحةُ على الهاتف صمتاً
 * تامّاً مهما وصلها من رسائل، وهو أسوأُ من عدم وجود التطبيق: موظّفٌ يظنّ
 * أنه مُنبَّه وليس كذلك.
 *
 * والغلاف يحقن قناةً باسم `RhallaNative`، فتُستعمل حيث توجد ويُرجع إلى
 * إشعار المتصفّح حيث لا توجد — فرعٌ واحد لا شيفرتان.
 */
const native = () =>
  (typeof window !== 'undefined' && window.RhallaNative) || null

export function showNotification(title, body, onClick) {
  const bridge = native()
  if (bridge) {
    try {
      // النغمة والاهتزاز من النظام نفسه — وهو ما اعتاده صاحب الهاتف،
      // ويعمل والجهاز في جيبه.
      bridge.postMessage('alert')
      return
    } catch {
      /* الجسر موجودٌ ولم يعمل — يُكمَل إلى إشعار المتصفّح أدناه. */
    }
  }

  if (!('Notification' in window) || Notification.permission !== 'granted') return
  try {
    const n = new Notification(title, {
      body,
      // وسمٌ ثابت لكل محادثة: عشر رسائل من وكيلٍ واحد تُحدّث إشعاراً واحداً
      // بدل أن تملأ الشاشة بعشرة.
      tag: `rhalla-support-${title}`,
      renotify: true,
      dir: 'rtl',
      lang: 'ar',
    })
    n.onclick = () => {
      window.focus()
      n.close()
      onClick?.()
    }
    setTimeout(() => n.close(), 15000)
  } catch {
    /* المتصفّح قد يمنعها رغم الإذن — لا شيء يُكسر. */
  }
}

/**
 * عدّادٌ في عنوان اللسان — يُرى واللسان في الخلفية، بلا إذنٍ ولا صوت.
 *
 * وهو أهمُّ من الإشعار عملياً: موظّف الدعم يترك اللسان مفتوحاً طول اليوم
 * ويلمحه بطرف عينه.
 */
const BASE_TITLE = 'الرحالة للدعم الفني'
export function setTitleBadge(count) {
  document.title = count > 0 ? `(${count}) ${BASE_TITLE}` : BASE_TITLE
}
