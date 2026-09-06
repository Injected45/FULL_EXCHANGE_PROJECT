import { useState } from 'react'
import { api, setToken } from './api'

/**
 * شاشة الدخول.
 *
 * ── ما تغيّر عن الصفحة القديمة ──────────────────────────────────────────
 *
 * كان الدخول **مفتاحاً واحداً مشتركاً** في `.env`، ويكتب الموظّف اسمه بيده
 * إلى جانبه. فالسجلُّ يقول «ردّ محمد» لأن أحدَهم كتب «محمد» — لا لأنه محمد.
 * ولم يكن يمكن منعُ شخصٍ بعينه إلا بتغيير المفتاح على الجميع.
 *
 * الآن: اسمُ دخولٍ وكلمةُ مرورٍ لكلّ موظّف، وإيقافُ واحدٍ لا يمسّ الباقين.
 *
 * ورسالةُ الفشل موحّدة («الاسم أو كلمة المرور غير صحيحة») فلا تصلح لاكتشاف
 * أسماء الدخول الموجودة — ويُستثنى منها الحسابُ الموقوف وحده، لأن صاحبه
 * يجب أن يعرف أن حسابه أُوقف بدل أن يظنّ أنه نسي كلمتَه.
 */
export default function Login({ onDone }) {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [busy, setBusy] = useState(false)

  const submit = async (e) => {
    e?.preventDefault()
    if (busy) return
    setBusy(true)
    setError('')
    try {
      const d = await api.login(username, password)
      setToken(d.token)
      onDone(d.staff)
    } catch (err) {
      setError(err.message)
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="login-wrap">
      <form className="login-card" onSubmit={submit}>
        <div className="login-brand">
          <h1>الرحالة للدعم الفني</h1>
          <p>مركز خدمة وكلاء شركة الرحالة للصرافة</p>
        </div>

        {error && <div className="alert alert-error">{error}</div>}

        <div className="field">
          <label>اسم الدخول</label>
          <input dir="ltr" autoFocus autoComplete="username" value={username}
                 onChange={(e) => setUsername(e.target.value)} />
        </div>

        <div className="field">
          <label>كلمة المرور</label>
          <input dir="ltr" type="password" autoComplete="current-password" value={password}
                 onChange={(e) => setPassword(e.target.value)} />
        </div>

        <button className="btn btn-primary btn-block" type="submit"
                disabled={busy || !username || !password}
                style={{ marginTop: 8 }}>
          {busy ? 'جارٍ الدخول…' : 'دخول'}
        </button>

        <p style={{
          marginTop: 18, marginBottom: 0, fontSize: 11.5,
          color: 'var(--muted)', textAlign: 'center', lineHeight: 1.75,
        }}>
          حسابُك شخصيّ ويُسجَّل باسمك.<br />
          إن نسيتَ كلمتَك فاطلب من مدير النظام تصفيرها.
        </p>
      </form>
    </div>
  )
}
