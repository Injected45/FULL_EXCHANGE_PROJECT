/**
 * أدواتٌ صغيرة مشتركة.
 *
 * ⚠ كلُّ رقمٍ هنا غربيّ `0123456789` — القاعدة نفسها في تطبيق الوكيل، ولا
 * استثناء لها. ولهذا لا يُستعمل `toLocaleString` بلا تحديد `en`: متصفّحٌ
 * على `ar-LY` يُخرج `20٢٦` من تلقاء نفسه.
 */

const WEEKDAYS = ['الأحد', 'الاثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت']

function parse(raw) {
  if (!raw) return null
  // الخادم يكتب `2026-09-06 07:45:12.1234567` — بلا `T` وبلا منطقة.
  // `Date` في بعض المتصفّحات ترفض هذا الشكل، فيُطبَّع أولاً.
  const s = String(raw).trim().replace(' ', 'T').replace(/\.(\d{3})\d+$/, '.$1')
  const d = new Date(s)
  return isNaN(d.getTime()) ? null : d
}

/** `14:33` — بلا ثوانٍ (قرار المالك). */
export function hhmm(raw) {
  const d = parse(raw)
  if (!d) return ''
  return `${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}`
}

/**
 * طابعُ صفّ القائمة — سلّمٌ من أربع درجات، وهو النظام نفسه المبنيّ في
 * `chat_threads_screen.dart`:
 *
 * | متى | ما يُعرض |
 * |---|---|
 * | اليوم | `14:33` |
 * | أمس | «أمس» |
 * | خلال الأسبوع | «الثلاثاء» |
 * | أقدم | `2026-08-28` |
 *
 * والمقارنة بالأيام التقويمية لا بفارق الساعات: رسالةٌ في الحادية عشرة
 * مساءً وأخرى في الواحدة صباحاً بينهما ساعتان وهما في يومين مختلفين.
 */
export function listStamp(raw) {
  const d = parse(raw)
  if (!d) return ''

  const now = new Date()
  const today = new Date(now.getFullYear(), now.getMonth(), now.getDate())
  const day = new Date(d.getFullYear(), d.getMonth(), d.getDate())
  const diff = Math.round((today - day) / 86400000)

  if (diff === 0) return hhmm(raw)
  if (diff === 1) return 'أمس'
  if (diff > 1 && diff < 7) return WEEKDAYS[day.getDay()]
  return ymd(d)
}

/** فاصلُ اليوم فوق الرسائل — «اليوم» / «أمس» / اسم اليوم / التاريخ. */
export function dayLabel(raw) {
  const d = parse(raw)
  if (!d) return ''

  const now = new Date()
  const today = new Date(now.getFullYear(), now.getMonth(), now.getDate())
  const day = new Date(d.getFullYear(), d.getMonth(), d.getDate())
  const diff = Math.round((today - day) / 86400000)

  if (diff === 0) return 'اليوم'
  if (diff === 1) return 'أمس'
  if (diff > 1 && diff < 7) return WEEKDAYS[day.getDay()]
  return ymd(d)
}

export function ymd(d) {
  const x = d instanceof Date ? d : parse(d)
  if (!x) return ''
  return `${x.getFullYear()}-${String(x.getMonth() + 1).padStart(2, '0')}-${String(x.getDate()).padStart(2, '0')}`
}

export function stampFull(raw) {
  const d = parse(raw)
  return d ? `${ymd(d)} ${hhmm(raw)}` : ''
}

/** هل الرسالتان في يومين مختلفين؟ — لوضع فاصل اليوم. */
export function sameDay(a, b) {
  const x = parse(a)
  const y = parse(b)
  if (!x || !y) return false
  return x.getFullYear() === y.getFullYear()
    && x.getMonth() === y.getMonth()
    && x.getDate() === y.getDate()
}

export function mmss(seconds) {
  const s = Math.max(0, Math.floor(seconds))
  return `${String(Math.floor(s / 60)).padStart(2, '0')}:${String(s % 60).padStart(2, '0')}`
}

/** مُعرّفٌ من الجهاز يمنع الازدواج عند إعادة المحاولة (نظير البند 68). */
export function newClientId() {
  if (crypto?.randomUUID) return crypto.randomUUID()
  return `c${Date.now()}-${Math.random().toString(36).slice(2, 10)}`
}

export function initials(name) {
  const s = String(name || '').trim()
  if (!s) return '؟'
  const parts = s.split(/\s+/)
  return parts.length > 1 ? parts[0][0] + parts[1][0] : s.slice(0, 2)
}

/**
 * ترجمةُ رمز الفعل في السجلّ إلى عربية.
 *
 * الرمزُ يبقى في القاعدة كما هو — الترجمةُ للعرض وحده، والقاعدة تُقرأ
 * ببرامجَ أخرى غداً.
 */
export const ACTIONS = {
  LOGIN: 'تسجيل دخول',
  LOGOUT: 'خروج',
  DENIED: 'محاولة بلا صلاحية',
  REPLY: 'ردّ على وكيل',
  ASSIGN: 'إسناد محادثة',
  UNASSIGN: 'نزع إسناد',
  STATUS: 'تغيير حالة',
  CLOSE: 'إغلاق محادثة',
  REOPEN: 'إعادة فتح',
  PIN: 'تثبيت رسالة',
  FORWARD: 'إعادة توجيه',
  EDIT: 'تعديل رسالة',
  STAFF_CREATE: 'إنشاء حساب',
  STAFF_UPDATE: 'تعديل حساب',
  STAFF_ENABLE: 'تفعيل حساب',
  STAFF_DISABLE: 'إيقاف حساب',
  STAFF_DELETE: 'حذف حساب',
  STAFF_RESET_PASS: 'تصفير كلمة مرور',
  PERM_GRANT: 'منح صلاحية',
  PERM_REVOKE: 'سحب صلاحية',
}

/**
 * مدّةٌ بالدقائق ⇐ نصٌّ عربيّ قصير: «45 د» · «3 س 10 د» · «2 ي».
 *
 * ⚠ بأرقامٍ لاتينية دائماً — كبقيّة أرقام المنظومة. والدقائقُ تُلفظ
 * سالبةً حين تُتجاوز المهلة، فالإشارةُ تُقرأ من اللون والنصّ معاً:
 * «تأخّرَ 20 د» لا «-20 د».
 */
export function minutesText(min) {
  const m = Math.abs(Math.round(Number(min) || 0))
  if (m < 60) return `${m} د`
  const h = Math.floor(m / 60)
  const r = m % 60
  if (h < 24) return r ? `${h} س ${r} د` : `${h} س`
  const d = Math.floor(h / 24)
  const rh = h % 24
  return rh ? `${d} ي ${rh} س` : `${d} ي`
}

/** نصُّ شارة المهلة: ما المقيس، وكم بقي أو تأخّر. */
export function slaText(sla) {
  if (!sla || !sla.kind) return ''
  const t = minutesText(sla.remaining_min)
  return sla.remaining_min < 0
    ? `${sla.kind_label}: تأخّر ${t}`
    : `${sla.kind_label}: بقي ${t}`
}
