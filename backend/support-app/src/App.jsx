import { useCallback, useEffect, useRef, useState } from 'react'
import { api, getToken, setToken, setUnauthorizedHandler } from './api'
import Audit from './Audit'
import Inbox from './Inbox'
import Login from './Login'
import Staff from './Staff'
import { askPermission, beep, notificationState, setTitleBadge, showNotification } from './notify'

const UNREAD_MS = 8000

export default function App() {
  const [me, setMe] = useState(null)
  const [booting, setBooting] = useState(true)
  const [tab, setTab] = useState('inbox')
  const [unread, setUnread] = useState(0)
  const [notifyState, setNotifyState] = useState(notificationState())
  const [changingPass, setChangingPass] = useState(false)

  // الأساس: أوّل استطلاعٍ لا يرنّ أبداً — انظر `notify.js`.
  const seen = useRef(null)

  useEffect(() => {
    setUnauthorizedHandler(() => { setMe(null); setTitleBadge(0) })
  }, [])

  // ── استعادة الجلسة ────────────────────────────────────────────────
  useEffect(() => {
    if (!getToken()) { setBooting(false); return }
    api.me()
      .then((d) => setMe(d.staff))
      .catch(() => setToken(''))
      .finally(() => setBooting(false))
  }, [])

  /**
   * صلاحيةٌ واحدة — **للعرض فقط.**
   *
   * ⚠ إخفاءُ الزرّ تجميل، والرفضُ الحقيقي في الخادم عند كل نداء. من فتح
   * أدوات المتصفّح وأظهر زرّاً مخفيّاً يحصل على 403، لا على الفعل.
   */
  const can = useCallback(
    (key) => Boolean(me?.permissions?.includes(key)),
    [me],
  )

  // ── عدّاد غير المقروء + التنبيه ────────────────────────────────────
  const pollUnread = useCallback(async () => {
    if (!me) return
    try {
      const d = await api.unread()
      const total = d.total || 0
      const ids = d.threads || []

      setUnread(total)
      setTitleBadge(total)

      if (seen.current === null) {
        // الأساس. صامتٌ عمداً: ما كان موجوداً قبل فتح الصفحة يعلنه العدّاد.
        seen.current = new Set(ids)
        return
      }

      const fresh = ids.filter((id) => !seen.current.has(id))
      if (fresh.length > 0) {
        beep()
        showNotification(
          'رسالة جديدة من وكيل',
          fresh.length === 1
            ? 'وصلت رسالة في محادثة واحدة.'
            : `وصلت رسائل في ${fresh.length} محادثات.`,
          () => setTab('inbox'),
        )
      }
      seen.current = new Set(ids)
    } catch {
      /* انقطاعٌ عابر — العدّاد يبقى على آخر قيمة معروفة. */
    }
  }, [me])

  useEffect(() => {
    if (!me) return
    pollUnread()
    const t = setInterval(() => {
      // لا استطلاع واللسان في الخلفية؟ بل نعم — العدّاد في العنوان هو
      // الفائدة الكبرى، ويُرى واللسان غير منظور.
      pollUnread()
    }, UNREAD_MS)
    return () => clearInterval(t)
  }, [me, pollUnread])

  useEffect(() => () => setTitleBadge(0), [])

  if (booting) {
    return <div style={{ display: 'grid', placeItems: 'center', height: '100%' }}>
      <div className="spin" />
    </div>
  }

  if (!me) {
    return <Login onDone={(staff) => { seen.current = null; setMe(staff) }} />
  }

  // كلمةٌ أولى لم تُغيَّر: لا شيء يُفتح قبلها. حسابٌ يبقى على كلمة من أنشأه
  // ليس حساباً شخصياً، والسجلُّ الذي يحمل اسمَه يصير شهادةَ زور.
  if (me.must_change && !changingPass) {
    return <ChangePassword forced onDone={() => { setToken(''); setMe(null) }} />
  }

  const tabs = [
    ['inbox', 'صندوق الوارد', unread],
    can('MANAGE_STAFF') && ['staff', 'الحسابات', 0],
    can('VIEW_AUDIT') && ['audit', 'سجلّ النشاط', 0],
  ].filter(Boolean)

  return (
    <div className="app">
      <header className="topbar">
        <div className="brand">الرحالة للدعم الفني</div>
        <nav>
          {tabs.map(([k, label, badge]) => (
            <button key={k} className={tab === k ? 'on' : ''} onClick={() => setTab(k)}>
              {label}
              {badge > 0 && <span className="nav-badge">{badge > 99 ? '99+' : badge}</span>}
            </button>
          ))}
        </nav>

        <div className="spacer" />

        {notifyState !== 'granted' && notifyState !== 'unsupported' && (
          <button className="btn btn-sm"
                  style={{ background: 'rgba(255,255,255,.16)', color: '#fff', border: 0 }}
                  title="إشعارات المتصفّح — مجّانية بالكامل، بلا أي خدمة خارجية"
                  onClick={async () => setNotifyState(await askPermission())}>
            🔔 تفعيل الإشعارات
          </button>
        )}

        <div className="who">
          <b>{me.name}</b>
          <span>{me.role_label}</span>
        </div>

        <button className="btn btn-sm"
                style={{ background: 'rgba(255,255,255,.16)', color: '#fff', border: 0 }}
                onClick={() => setChangingPass(true)}>كلمة المرور</button>

        <button className="btn btn-sm"
                style={{ background: 'rgba(255,255,255,.16)', color: '#fff', border: 0 }}
                onClick={async () => {
                  try { await api.logout() } catch { /* الرمز يُمسح محلّياً على أي حال */ }
                  setToken(''); setMe(null); setTitleBadge(0)
                }}>خروج</button>
      </header>

      {tab === 'inbox' && <Inbox can={can} me={me} onUnreadChange={pollUnread} />}
      {tab === 'staff' && can('MANAGE_STAFF') && <Staff me={me} />}
      {tab === 'audit' && can('VIEW_AUDIT') && <Audit />}

      {changingPass && (
        <ChangePassword
          onClose={() => setChangingPass(false)}
          onDone={() => { setToken(''); setMe(null); setChangingPass(false) }}
        />
      )}
    </div>
  )
}

/**
 * تغيير كلمة المرور.
 *
 * ⚠ ينهي **كلَّ** الجلسات بما فيها هذه. من غيّر كلمتَه غالباً لأنه يخشى أن
 * أحداً يعرفها، وتغييرٌ يُبقي جلسةَ ذلك الأحد مفتوحةً لا يفعل شيئاً.
 */
function ChangePassword({ forced, onClose, onDone }) {
  const [cur, setCur] = useState('')
  const [next, setNext] = useState('')
  const [again, setAgain] = useState('')
  const [error, setError] = useState('')
  const [busy, setBusy] = useState(false)

  const submit = async () => {
    if (next !== again) { setError('الكلمتان غير متطابقتين.'); return }
    setBusy(true)
    try {
      await api.changePassword(cur, next)
      alert('تم تغيير كلمة المرور. سجّل الدخول من جديد.')
      onDone()
    } catch (e) { setError(e.message) } finally { setBusy(false) }
  }

  const body = (
    <div className="modal" onClick={(e) => e.stopPropagation()}>
      <h3>{forced ? 'غيّر كلمة المرور الأولى' : 'تغيير كلمة المرور'}</h3>
      <p className="sub">
        {forced
          ? 'دخلتَ بكلمةٍ يعرفها من أنشأ حسابك. غيّرها الآن ليصير الحساب حسابك وحدك — فسجلّ النشاط يحمل اسمك.'
          : 'ثمانيةُ محارف على الأقل، فيها حرفٌ ورقم. وستنتهي جلساتُك كلُّها بعد الحفظ.'}
      </p>

      {error && <div className="alert alert-error">{error}</div>}

      <div className="field">
        <label>كلمة المرور الحالية</label>
        <input type="password" dir="ltr" value={cur} onChange={(e) => setCur(e.target.value)} />
      </div>
      <div className="field">
        <label>الكلمة الجديدة</label>
        <input type="password" dir="ltr" value={next} onChange={(e) => setNext(e.target.value)} />
      </div>
      <div className="field">
        <label>تأكيد الكلمة الجديدة</label>
        <input type="password" dir="ltr" value={again} onChange={(e) => setAgain(e.target.value)}
               onKeyDown={(e) => e.key === 'Enter' && submit()} />
      </div>

      <div className="modal-acts">
        {!forced && <button className="btn btn-ghost" onClick={onClose}>إلغاء</button>}
        <button className="btn btn-primary" disabled={busy || !cur || !next} onClick={submit}>
          حفظ
        </button>
      </div>
    </div>
  )

  return <div className="modal-back" onClick={forced ? undefined : onClose}>{body}</div>
}
