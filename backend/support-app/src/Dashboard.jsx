import { useCallback, useEffect, useRef, useState } from 'react'
import { api } from './api'
import { minutesText } from './util'

/**
 * لوحة القيادة — البند 1.
 *
 * ── ما تجيب عنه ────────────────────────────────────────────────────────
 *
 * سؤالٌ واحد: **ما الذي يحتاج تدخّلاً الآن؟** فكلُّ رقمٍ هنا إمّا عملٌ
 * ينتظر، أو قدرةٌ متاحةٌ لحمله. ورقمٌ لا يفعل أحدَهما زينةٌ تُبعد العين
 * عمّا يهمّ — ولذلك لا «إجمالي المحادثات منذ البداية» في هذه الشاشة.
 *
 * ── ولماذا كلُّ رقمٍ منها يُضغَط ─────────────────────────────────────────
 *
 * ⚠ رقمٌ لا يقود إلى ما يصفه يجعل الموظّف يبحث عنه بيده. فالضغطُ على
 * «متأخّرة» يفتح صندوق الوارد مفروزاً بأقرب مهلة، و«بلا مالك» يفتحه على
 * نطاقها. وهذا هو الفرق بين لوحةٍ تُدار منها الحالةُ ولوحةٍ تُقرأ.
 *
 * ── والتحديث ───────────────────────────────────────────────────────────
 *
 * كلَّ عشر ثوانٍ، **بلا وميض**: البياناتُ تُستبدل ولا تُفرَّغ الشاشةُ قبلها.
 * ولا تُعرَض حالةُ «تحميل» بعد أوّل قراءة — لوحةٌ ترتجف كلَّ عشر ثوانٍ لا
 * يُنظر إليها.
 */

const REFRESH_MS = 10000

export default function Dashboard({ can, onOpenInbox }) {
  const [d, setD] = useState(null)
  const [error, setError] = useState('')
  const [editingSla, setEditingSla] = useState(false)
  const alive = useRef(true)

  const load = useCallback(async () => {
    try {
      const data = await api.dashboard()
      if (!alive.current) return
      setD(data)
      setError('')
    } catch (e) {
      if (alive.current) setError(e.message)
    }
  }, [])

  useEffect(() => {
    alive.current = true
    load()
    const t = setInterval(load, REFRESH_MS)
    return () => { alive.current = false; clearInterval(t) }
  }, [load])

  if (!d && error) return <div className="alert alert-error" style={{ margin: 16 }}>{error}</div>
  if (!d) return <div className="dash"><div className="spin" /></div>

  const c = d.counts || {}
  const a = d.averages || {}

  return (
    <div className="dash">
      {/* ── ما ينتظر عملاً ───────────────────────────────────────── */}
      <section className="dash-sec">
        <h3>ما ينتظر عملاً الآن</h3>
        <div className="kpis">
          <Kpi n={c.awaiting_support} label="بانتظار ردّنا" tone="hot"
               hint="آخر رسالةٍ فيها من الوكيل، ولم يردّ الدعم بعدها"
               onClick={() => onOpenInbox({ scope: 'all', sort: 'sla' })} />
          <Kpi n={c.unassigned} label="بلا مالك" tone="warn"
               hint="مفتوحةٌ ولم يأخذها أحد — وما لا مالك له لا يُتابَع"
               onClick={() => onOpenInbox({ scope: 'unassigned' })} />
          <Kpi n={c.mine} label="المُسنَدة إليّ" tone="mine"
               onClick={() => onOpenInbox({ scope: 'mine' })} />
          <Kpi n={c.unread_threads} label="فيها غير مقروء" tone="info"
               hint={`${c.unread_messages || 0} رسالة في ${c.unread_threads || 0} محادثة`}
               onClick={() => onOpenInbox({ scope: 'all' })} />
        </div>
      </section>

      {/* ── المهل ────────────────────────────────────────────────── */}
      <section className="dash-sec">
        <h3>
          أزمنة الاستجابة
          {can('MANAGE_SLA') && (
            <button className="btn btn-sm btn-ghost" style={{ marginInlineStart: 8 }}
                    onClick={() => setEditingSla(true)}>ضبط المهل</button>
          )}
        </h3>
        <div className="kpis">
          <Kpi n={c.sla_breached} label="تجاوزت المهلة" tone="bad"
               hint="اضغط لفتح الصندوق مفروزاً بأقرب مهلة"
               onClick={() => onOpenInbox({ scope: 'all', sort: 'sla' })} />
          <Kpi n={c.sla_warning} label="قاربت المهلة" tone="warn"
               hint="تجاوزت ثلاثة أرباع المهلة ولم تُتجاوز بعد"
               onClick={() => onOpenInbox({ scope: 'all', sort: 'sla' })} />
          <Kpi n={c.sla_ok} label="ضمن المهلة" tone="good" />
          <Kpi n={c.resolved_today} label="عولجت اليوم" tone="good" />
        </div>

        {/*
          ⚠ المتوسّطات على آخر ثلاثين يوماً لا على التاريخ كلِّه: متوسّطٌ
          يشمل سنةً مضت لا يتحرّك مهما تحسّن الفريق أو ساء، فيصير رقماً
          يُعرض ولا يُقرأ.
        */}
        <div className="avg-row">
          <Avg label="متوسّط أوّل ردّ" v={a.first_response_min} />
          <Avg label="متوسّط الردّ التالي" v={a.next_response_min} />
          <Avg label="متوسّط المعالجة" v={a.resolution_min} />
          <div className="avg-note">
            على آخر {a.window_days || 30} يوماً · <span className="num">{a.answered_threads || 0}</span> محادثةً أُجيبت
          </div>
        </div>
      </section>

      {/* ── الحالة العامّة ───────────────────────────────────────── */}
      <section className="dash-sec">
        <h3>الحالة</h3>
        <div className="kpis">
          <Kpi n={c.new} label="جديدة" tone="info" onClick={() => onOpenInbox({ status: 'NEW' })} />
          <Kpi n={c.open} label="مفتوحة" tone="info" onClick={() => onOpenInbox({ status: 'OPEN' })} />
          <Kpi n={c.pending} label="بانتظار الوكيل" tone="mute"
               hint="ردَّ الدعمُ وينتظر جوابَ الوكيل — انتظارٌ لا نملك تعجيله"
               onClick={() => onOpenInbox({ status: 'PENDING' })} />
          <Kpi n={c.closed} label="مغلقة" tone="mute" onClick={() => onOpenInbox({ status: 'CLOSED' })} />
        </div>
      </section>

      {/* ── الفريق ───────────────────────────────────────────────── */}
      {can('VIEW_TEAM') && <Team d={d} />}

      <div className="dash-foot">
        آخر تحديث: {new Date(d.generated_at).toLocaleTimeString('en-GB')}
        {error && <span style={{ color: 'var(--error)' }}> · تعذّر آخرُ تحديث: {error}</span>}
      </div>

      {editingSla && can('MANAGE_SLA') && (
        <SlaEditor rows={d.sla || []} onClose={() => setEditingSla(false)}
                   onSaved={() => { setEditingSla(false); load() }} />
      )}
    </div>
  )
}

function Kpi({ n, label, hint, tone, onClick }) {
  const Tag = onClick ? 'button' : 'div'
  return (
    <Tag className={`kpi kpi-${tone || 'info'}${onClick ? ' clickable' : ''}`}
         title={hint || ''} onClick={onClick}>
      <b className="num">{n ?? 0}</b>
      <span>{label}</span>
    </Tag>
  )
}

function Avg({ label, v }) {
  return (
    <div className="avg">
      <span>{label}</span>
      {/* «—» لا صفر: صفرٌ يعني «ردٌّ فوريّ»، والغيابُ يعني «لا قياس بعد». */}
      <b className="mins">{v === null || v === undefined ? '—' : minutesText(v)}</b>
    </div>
  )
}

/**
 * الفريق وحمولته — البند 35.
 *
 * ⚠ ونسبةُ الحمولة تتجاوز 100% عمداً حين يُحمَّل الموظّف فوق سعته:
 * إخفاءُ التجاوز يُخفي المشكلة التي وُضع المقياس ليكشفها.
 */
function Team({ d }) {
  const team = d.team || []
  const c = d.counts || {}
  if (team.length === 0) return null

  return (
    <section className="dash-sec">
      <h3>
        الفريق
        <span className="sub-inline">
          {c.staff_online || 0} متّصل · {c.staff_available || 0} متاح · {c.staff_busy || 0} مشغول
        </span>
      </h3>
      <div className="team">
        {team.map((s) => (
          <div key={s.id} className="tm-card">
            <span className="dot" style={{ background: s.presence_color }} title={s.presence_label} />
            <div className="tm-mid">
              <b>{s.name}</b>
              <span>{s.role} · {s.presence_label}</span>
            </div>
            <div className="tm-load" title={`${s.open_threads} من ${s.capacity}`}>
              <div className="bar">
                <i style={{
                  width: `${Math.min(100, s.load_pct)}%`,
                  background: s.load_pct >= 100 ? 'var(--error)'
                    : s.load_pct >= 75 ? 'var(--warn)' : 'var(--success)',
                }} />
              </div>
              <span className="num">{s.open_threads}/{s.capacity}</span>
            </div>
          </div>
        ))}
      </div>
    </section>
  )
}

/**
 * ضبط المهل — البند 2: «قابلة للإدارة من إعدادات النظام ولا تكون Hardcoded».
 *
 * ⚠ ولا يُرسَل إلّا ما تغيّر فعلاً. عيبٌ في هذه الدفعة كان يرسل الحقولَ
 * الثلاثة دائماً، فالحقلُ الذي لم يُكتب يصير «لا هدف» بصمت — فمن عدّل زمنَ
 * أوّل ردّ محا الآخرين، وبدا الفريقُ ملتزماً بزمنٍ لم يعد يُقاس. الخادمُ
 * أُصلح، والواجهةُ لا تُرسل ما لم يُلمَس أصلاً.
 */
function SlaEditor({ rows, onClose, onSaved }) {
  const [draft, setDraft] = useState(() => {
    const m = {}
    for (const r of rows) {
      m[r.priority] = {
        first: r.first_minutes ?? '',
        next: r.next_minutes ?? '',
        resolve: r.resolve_minutes ?? '',
      }
    }
    return m
  })
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState('')

  const original = useRef(JSON.parse(JSON.stringify(
    rows.reduce((m, r) => {
      m[r.priority] = {
        first: r.first_minutes ?? '',
        next: r.next_minutes ?? '',
        resolve: r.resolve_minutes ?? '',
      }
      return m
    }, {}),
  )))

  const set = (p, k, v) => setDraft((d) => ({ ...d, [p]: { ...d[p], [k]: v } }))

  const save = async () => {
    setBusy(true)
    setError('')
    try {
      for (const r of rows) {
        const p = r.priority
        const now = draft[p]
        const was = original.current[p] || {}
        const changes = {}
        if (String(now.first) !== String(was.first)) changes.first_minutes = now.first === '' ? null : Number(now.first)
        if (String(now.next) !== String(was.next)) changes.next_minutes = now.next === '' ? null : Number(now.next)
        if (String(now.resolve) !== String(was.resolve)) changes.resolve_minutes = now.resolve === '' ? null : Number(now.resolve)
        if (Object.keys(changes).length > 0) await api.updateSla(p, changes)
      }
      onSaved()
    } catch (e) {
      setError(e.message)
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="modal-back" onClick={onClose}>
      <div className="modal" style={{ maxWidth: 640 }} onClick={(e) => e.stopPropagation()}>
        <h3>أزمنة الاستجابة</h3>
        <p className="sub">
          بالدقائق، لكل أولوية. واتركِ الحقلَ فارغاً ليعني «لا هدف لهذه
          الحالة» — وحينها لا تظهر شارةُ مهلةٍ على المحادثة أصلاً.
        </p>

        {error && <div className="alert alert-error">{error}</div>}

        <div className="sla-grid">
          <div className="sla-head">
            <span>الأولوية</span><span>أوّل ردّ</span><span>الردّ التالي</span><span>المعالجة</span>
          </div>
          {rows.map((r) => (
            <div key={r.priority} className="sla-row">
              <span className="sla-p" style={{ color: r.color }}>{r.label}</span>
              {['first', 'next', 'resolve'].map((k) => (
                <input key={k} type="number" min="1" max="43200" dir="ltr" className="num"
                       value={draft[r.priority]?.[k] ?? ''}
                       onChange={(e) => set(r.priority, k, e.target.value)} />
              ))}
            </div>
          ))}
        </div>

        <div className="modal-acts">
          <button className="btn btn-ghost" onClick={onClose}>إلغاء</button>
          <button className="btn btn-primary" disabled={busy} onClick={save}>حفظ</button>
        </div>
      </div>
    </div>
  )
}
