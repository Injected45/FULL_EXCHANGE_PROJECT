import { useEffect, useState } from 'react'
import { api } from './api'
import { ACTIONS, stampFull } from './util'

/**
 * سجلّ النشاط — يُقرأ ولا يُعدَّل ولا يُحذف منه.
 *
 * وهو ما يجعل «من أغلق محادثة هذا الوكيل؟» و«من سحب هذه الصلاحية؟» سؤالين
 * لهما جوابٌ بعد شهر. ويسجّل المحاولاتِ المرفوضة أيضاً — «حاول فلانٌ فعلَ ما
 * ليس له» معلومةٌ يريدها المدير قبل أن تتكرّر.
 */
export default function Audit() {
  const [rows, setRows] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [only, setOnly] = useState('')

  useEffect(() => {
    api.audit()
      .then((d) => setRows(d.items || []))
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }, [])

  const shown = only ? rows.filter((r) => r.action === only) : rows
  const kinds = [...new Set(rows.map((r) => r.action))]

  if (loading) return <div className="page"><div className="spin" /></div>

  return (
    <div className="page">
      <div className="inner">
        {error && <div className="alert alert-error">{error}</div>}

        <div className="card">
          <h2>سجلّ النشاط</h2>
          <p className="sub">
            آخر <span className="num">200</span> حركة. يُضاف إليه ولا يُحذف منه،
            واسمُ الموظّف منسوخٌ وقتَ الفعل — الأسماء تتغيّر والسجلُّ يقول ما كان يومَها.
          </p>

          <div className="filters" style={{ padding: 0, border: 0, marginBottom: 12 }}>
            <button className={`chip${only === '' ? ' on' : ''}`} onClick={() => setOnly('')}>
              الكلّ
            </button>
            {kinds.map((k) => (
              <button key={k} className={`chip${only === k ? ' on' : ''}`}
                      onClick={() => setOnly(k)}>
                {ACTIONS[k] || k}
              </button>
            ))}
          </div>

          <table className="grid">
            <thead>
              <tr>
                <th>الوقت</th><th>الموظّف</th><th>الفعل</th>
                <th>المحادثة</th><th>التفاصيل</th><th>العنوان</th>
              </tr>
            </thead>
            <tbody>
              {shown.map((r) => (
                <tr key={r.id}>
                  <td><span className="num" style={{ fontSize: 11.5 }}>{stampFull(r.created_at)}</span></td>
                  <td>{r.staff_name}</td>
                  <td>
                    <span className={`pill ${r.action === 'DENIED' ? 'pill-NEW' : 'pill-OPEN'}`}>
                      {ACTIONS[r.action] || r.action}
                    </span>
                  </td>
                  <td>{r.thread_id ? <span className="num">#{r.thread_id}</span> : '—'}</td>
                  <td style={{ fontSize: 12.5, color: 'var(--muted)' }}>
                    {[r.target, r.detail].filter(Boolean).join(' — ') || '—'}
                  </td>
                  <td><span className="num" style={{ fontSize: 11, color: 'var(--muted-2)' }}>
                    {r.ip || '—'}
                  </span></td>
                </tr>
              ))}
            </tbody>
          </table>

          {shown.length === 0 && (
            <div className="empty" style={{ height: 130 }}>لا حركات مطابقة</div>
          )}
        </div>
      </div>
    </div>
  )
}
