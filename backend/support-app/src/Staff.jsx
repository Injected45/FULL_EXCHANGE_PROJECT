import { useEffect, useState } from 'react'
import { api } from './api'
import { stampFull } from './util'

/**
 * إدارة حسابات الدعم وصلاحياتها.
 *
 * ── لماذا تُعرض الصلاحية الممنوعة رماديةً ولا تُخفى ───────────────────────
 *
 * الإخفاء يجعل المدير يظنّ أن الصلاحية غير موجودة في النظام، فيبحث عنها في
 * مكانٍ آخر أو يطلب بناءها. والرماديُّ يقول شيئاً أدقّ: هي موجودة، وممنوعةٌ
 * **لهذا الدور** — فإن أرادها رفع الدور. وهما جوابان مختلفان لسؤالٍ واحد.
 *
 * ⚠ وكلمةُ المرور تُعرض **مرّةً واحدة** ولا تُخزَّن نصّاً ولا تُرسَل. من
 * أغلق النافذة قبل أن ينسخها يُصفّرها من جديد — وهذا أهون من كلمةٍ محفوظة
 * في القاعدة يقرؤها من يقرأ نسخةً احتياطية.
 */
export default function Staff({ me }) {
  const [rows, setRows] = useState([])
  const [roles, setRoles] = useState({})
  const [catalogs, setCatalogs] = useState({})
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [ok, setOk] = useState('')
  const [creating, setCreating] = useState(false)
  const [editingPerms, setEditingPerms] = useState(null)
  const [secret, setSecret] = useState(null)

  const load = async () => {
    try {
      const d = await api.staff()
      setRows(d.items || [])
      setRoles(d.roles || {})
      setCatalogs(d.catalog_by_role || {})
      setError('')
    } catch (e) { setError(e.message) } finally { setLoading(false) }
  }

  useEffect(() => { load() }, [])

  const run = async (fn, msg) => {
    try {
      const r = await fn()
      setOk(msg || 'تم.')
      setError('')
      await load()
      return r
    } catch (e) { setError(e.message); setOk('') }
  }

  if (loading) return <div className="page"><div className="spin" /></div>

  return (
    <div className="page">
      <div className="inner">
        {error && <div className="alert alert-error">{error}</div>}
        {ok && <div className="alert alert-ok" onClick={() => setOk('')}>{ok}</div>}

        <div className="card">
          <h2>حسابات الدعم</h2>
          <p className="sub">
            كلُّ موظّفٍ حسابٌ باسمه ودورِه — لا مفتاحَ مشتركاً. والصلاحيات
            <b> مرفوضةٌ افتراضياً</b>: ما لم يُمنح صراحةً لا يعمل، ولو ظهر زرُّه.
          </p>

          <button className="btn btn-primary" onClick={() => setCreating(true)}>
            + حساب جديد
          </button>

          <table className="grid" style={{ marginTop: 16 }}>
            <thead>
              <tr>
                <th>الاسم</th><th>اسم الدخول</th><th>الدور</th>
                <th>الصلاحيات</th><th>آخر ظهور</th><th>الحالة</th><th></th>
              </tr>
            </thead>
            <tbody>
              {rows.map((s) => (
                <tr key={s.id}>
                  <td><b>{s.name}</b></td>
                  <td><span className="num">{s.username}</span></td>
                  <td>
                    <select value={s.role} disabled={s.id === me.id}
                            onChange={(e) => run(
                              () => api.updateStaff(s.id, { role: e.target.value }),
                              'تغيّر الدور، وسُحب ما صار فوق سقفه.',
                            )}>
                      {Object.entries(roles).map(([k, v]) => (
                        <option key={k} value={k}>{v}</option>
                      ))}
                    </select>
                  </td>
                  <td>
                    <button className="btn btn-ghost btn-sm" disabled={s.id === me.id}
                            onClick={() => setEditingPerms(s)}>
                      {/* ‏` ` لا فراغٌ عادي: العنصر `.num` معزولٌ
                          اتجاهياً (`unicode-bidi: isolate`)، والفراغُ
                          العادي على حدّ العزل يُبتلع فتُقرأ «18صلاحية». */}
                      <span className="num">{s.permissions.length}</span>{' '}صلاحية
                    </button>
                  </td>
                  <td><span className="num" style={{ fontSize: 11.5, color: 'var(--muted)' }}>
                    {s.last_seen_at ? stampFull(s.last_seen_at) : '—'}
                  </span></td>
                  <td>
                    {s.is_active
                      ? <span className="pill pill-OPEN">فعّال</span>
                      : <span className="pill pill-CLOSED">موقوف</span>}
                    {s.must_change && (
                      <span className="pill pill-NEW" style={{ marginInlineStart: 4 }}>
                        كلمة أولى
                      </span>
                    )}
                  </td>
                  <td style={{ whiteSpace: 'nowrap' }}>
                    {s.id !== me.id && (
                      <>
                        <button className="btn btn-ghost btn-sm"
                                onClick={() => run(
                                  () => api.updateStaff(s.id, { is_active: !s.is_active }),
                                  s.is_active ? 'أُوقف الحساب وأُنهيت جلساته فوراً.' : 'فُعِّل الحساب.',
                                )}>
                          {s.is_active ? 'إيقاف' : 'تفعيل'}
                        </button>
                        <button className="btn btn-ghost btn-sm" style={{ marginInlineStart: 4 }}
                                onClick={async () => {
                                  if (!window.confirm(`تصفير كلمة مرور «${s.name}»؟ ستنتهي جلساته فوراً.`)) return
                                  const r = await run(() => api.resetStaffPassword(s.id), '')
                                  if (r?.password) setSecret({ name: s.name, password: r.password })
                                }}>
                          كلمة جديدة
                        </button>
                        <button className="btn btn-ghost btn-sm"
                                style={{ marginInlineStart: 4, color: 'var(--error)' }}
                                onClick={() => {
                                  if (!window.confirm(`حذف حساب «${s.name}»؟ لا يُحذف سجلّ نشاطه.`)) return
                                  run(() => api.deleteStaff(s.id), 'حُذف الحساب ونُزع إسناد محادثاته.')
                                }}>
                          حذف
                        </button>
                      </>
                    )}
                    {s.id === me.id && <span style={{ fontSize: 11.5, color: 'var(--muted)' }}>أنت</span>}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <p className="sub" style={{ marginTop: 14, marginBottom: 0 }}>
            لا يُغيّر أحدٌ دورَه ولا صلاحياته ولا يوقف حسابه بنفسه — خطوةٌ واحدة
            تُخرج آخر مديرٍ من النظام ولا سبيل للعودة إلا من قاعدة البيانات.
          </p>
        </div>
      </div>

      {/* ── إنشاء ── */}
      {creating && (
        <CreateStaff roles={roles} onClose={() => setCreating(false)}
                     onDone={async (r, name) => {
                       setCreating(false)
                       setSecret({ name, password: r.password })
                       await load()
                     }}
                     onError={setError} />
      )}

      {/* ── الصلاحيات ── */}
      {editingPerms && (
        <PermissionEditor
          staff={editingPerms}
          catalog={catalogs[editingPerms.role] || []}
          onClose={() => setEditingPerms(null)}
          onSave={async (keys) => {
            await run(() => api.setPermissions(editingPerms.id, keys), 'حُفظت الصلاحيات.')
            setEditingPerms(null)
          }}
        />
      )}

      {/* ── الكلمة المعروضة مرّةً ── */}
      {secret && (
        <div className="modal-back">
          <div className="modal" style={{ maxWidth: 400 }}>
            <h3>كلمة المرور الأولى</h3>
            <p className="sub">
              سلّمها إلى <b>{secret.name}</b> الآن. <b>لن تظهر ثانيةً</b> — لا هنا
              ولا في قاعدة البيانات؛ وسيُطلب منه تغييرها عند أوّل دخول.
            </p>
            <div style={{
              background: 'var(--primary-light)', border: '1px solid rgba(15,95,78,.25)',
              borderRadius: 10, padding: '14px 16px', textAlign: 'center',
              fontFamily: 'var(--font-num)', fontSize: 21, fontWeight: 700,
              letterSpacing: 1.5, direction: 'ltr', userSelect: 'all',
            }}>{secret.password}</div>
            <div className="modal-acts">
              <button className="btn btn-ghost" onClick={() => {
                navigator.clipboard?.writeText(secret.password).catch(() => {})
              }}>نسخ</button>
              <button className="btn btn-primary" onClick={() => setSecret(null)}>
                نسختُها — إغلاق
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  )
}

function CreateStaff({ roles, onClose, onDone, onError }) {
  const [name, setName] = useState('')
  const [username, setUsername] = useState('')
  const [role, setRole] = useState('SUPPORT_AGENT')
  const [busy, setBusy] = useState(false)

  const submit = async () => {
    setBusy(true)
    try {
      const r = await api.createStaff(name, username, role)
      onDone(r, name)
    } catch (e) { onError(e.message) } finally { setBusy(false) }
  }

  return (
    <div className="modal-back" onClick={onClose}>
      <div className="modal" onClick={(e) => e.stopPropagation()}>
        <h3>حساب دعمٍ جديد</h3>
        <p className="sub">
          يُنشأ بصلاحيات دوره الافتراضية — تُراجَع بعد الإنشاء. وكلمةُ المرور
          تُولَّد هنا وتُعرض مرّةً واحدة.
        </p>
        <div className="field">
          <label>اسم الموظّف</label>
          <input value={name} onChange={(e) => setName(e.target.value)}
                 placeholder="مثال: أحمد المبروك" />
        </div>
        <div className="field">
          <label>اسم الدخول</label>
          <input value={username} dir="ltr"
                 onChange={(e) => setUsername(e.target.value.toLowerCase())}
                 placeholder="ahmed.m" />
          <div style={{ fontSize: 11.5, color: 'var(--muted)', marginTop: 5 }}>
            حروفٌ لاتينية صغيرة وأرقام و <span className="num">. _ -</span> فقط.
          </div>
        </div>
        <div className="field">
          <label>الدور</label>
          <select value={role} onChange={(e) => setRole(e.target.value)}>
            {Object.entries(roles).map(([k, v]) => <option key={k} value={k}>{v}</option>)}
          </select>
        </div>
        <div className="modal-acts">
          <button className="btn btn-ghost" onClick={onClose}>إلغاء</button>
          <button className="btn btn-primary" disabled={busy || !name || !username}
                  onClick={submit}>إنشاء</button>
        </div>
      </div>
    </div>
  )
}

function PermissionEditor({ staff, catalog, onClose, onSave }) {
  const [sel, setSel] = useState(new Set(staff.permissions))
  const [busy, setBusy] = useState(false)

  const toggle = (k) => {
    const next = new Set(sel)
    next.has(k) ? next.delete(k) : next.add(k)
    setSel(next)
  }

  return (
    <div className="modal-back" onClick={onClose}>
      <div className="modal" style={{ maxWidth: 620 }} onClick={(e) => e.stopPropagation()}>
        <h3>صلاحيات «{staff.name}»</h3>
        <p className="sub">
          الرماديُّ ممنوعٌ بسقف دور «{staff.role_label}» — ارفع الدور إن أردته.
          وكلُّ سحبٍ أو منحٍ يُسجَّل في سجلّ النشاط باسمك.
        </p>

        <div className="perm-groups">
          {catalog.map((g) => (
            <div className="perm-group" key={g.group}>
              <h4>{g.name}</h4>
              {g.items.map((it) => (
                <label className={`perm-item${it.allowed ? '' : ' off'}`} key={it.key}>
                  <input type="checkbox" disabled={!it.allowed}
                         checked={sel.has(it.key)} onChange={() => toggle(it.key)} />
                  <span>{it.label}</span>
                </label>
              ))}
            </div>
          ))}
        </div>

        <div className="modal-acts">
          <button className="btn btn-ghost" onClick={onClose}>إلغاء</button>
          <button className="btn btn-primary" disabled={busy} onClick={async () => {
            setBusy(true)
            await onSave([...sel])
            setBusy(false)
          }}>حفظ</button>
        </div>
      </div>
    </div>
  )
}
