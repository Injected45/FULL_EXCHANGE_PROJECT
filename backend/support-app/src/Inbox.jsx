import { useCallback, useEffect, useRef, useState } from 'react'
import { api } from './api'
import Conversation from './Conversation'
import { initials, listStamp } from './util'

const SCOPES = [
  ['all', 'الكلّ'],
  ['mine', 'المُسنَدة إليّ'],
  ['unassigned', 'بلا مالك'],
]

/**
 * صندوق الوارد — قائمةُ محادثات الوكلاء، والمحادثةُ المفتوحة إلى جانبها.
 *
 * ── لماذا لا تُفتح محادثةٌ تلقائياً ─────────────────────────────────────
 *
 * صفحةُ الإدارة القديمة كانت تفتح أوّل محادثة عند كل تحميل، فتُعلّم رسائل
 * وكيلٍ «مقروءة» لأن موظّفاً فتح المتصفّح — والوكيل يرى شرطتين زرقاوين على
 * رسالةٍ لم يقرأها أحد. الشرطتان وعدٌ، ووعدٌ كاذبٌ في الدعم أسوأ من غيابه.
 */
export default function Inbox({ can, me, onUnreadChange }) {
  const [items, setItems] = useState([])
  const [stats, setStats] = useState({})
  const [statuses, setStatuses] = useState({})
  const [scope, setScope] = useState('all')
  const [status, setStatus] = useState('')
  const [q, setQ] = useState('')
  const [open, setOpen] = useState(0)
  const [jumpTo, setJumpTo] = useState(0)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [searching, setSearching] = useState(false)

  const debounce = useRef(null)

  const load = useCallback(async () => {
    try {
      const d = await api.threads({ scope, status, q })
      setItems(d.items || [])
      setStats(d.stats || {})
      setStatuses(d.statuses || {})
      setError('')
    } catch (e) {
      setError(e.message)
    } finally {
      setLoading(false)
    }
  }, [scope, status, q])

  useEffect(() => {
    clearTimeout(debounce.current)
    // البحثُ مؤجَّل ربعَ ثانية: طلبٌ مع كل حرفٍ يُغرق الخادم ويُظهر نتائجَ
    // تسبق ما كُتب.
    debounce.current = setTimeout(load, q ? 250 : 0)
    return () => clearTimeout(debounce.current)
  }, [load, q])

  // تحديثٌ دوريّ صامت للقائمة — بلا وميض: البيانات تُستبدل والشاشة تبقى.
  useEffect(() => {
    const t = setInterval(() => { load(); onUnreadChange?.() }, 12000)
    return () => clearInterval(t)
  }, [load, onUnreadChange])

  return (
    <div className={`inbox${open ? ' has-open' : ''}`}>
      <div className="thread-list">
        <div className="stats">
          <div className="s-new"><b className="num">{stats.new ?? 0}</b><span>جديدة</span></div>
          <div className="s-open"><b className="num">{stats.open ?? 0}</b><span>مفتوحة</span></div>
          <div className="s-pending"><b className="num">{stats.pending ?? 0}</b><span>بانتظار</span></div>
          <div className="s-closed"><b className="num">{stats.closed ?? 0}</b><span>مغلقة</span></div>
        </div>

        <div className="filters">
          <input value={q} onChange={(e) => setQ(e.target.value)}
                 placeholder="ابحث باسم الوكيل أو رقمه…" />
          {can('SEARCH_MESSAGES') && (
            <button className="chip" onClick={() => setSearching(true)}
                    title="البحث في نصّ الرسائل نفسها">🔍 في الرسائل</button>
          )}
          {SCOPES.map(([k, label]) => (
            <button key={k} className={`chip${scope === k ? ' on' : ''}`}
                    onClick={() => setScope(k)}>{label}</button>
          ))}
          <button className={`chip${status === 'CLOSED' ? ' on' : ''}`}
                  onClick={() => setStatus(status === 'CLOSED' ? '' : 'CLOSED')}>
            المغلقة
          </button>
        </div>

        <div className="threads">
          {loading && <div className="spin" />}
          {error && <div className="alert alert-error" style={{ margin: 10 }}>{error}</div>}
          {!loading && items.length === 0 && (
            <div className="empty" style={{ height: 200 }}>
              <div><div className="big">📭</div>لا محادثات تطابق هذا الفلتر</div>
            </div>
          )}
          {items.map((t) => (
            <button key={t.id} className={`thread-row${open === t.id ? ' on' : ''}`}
                    onClick={() => setOpen(t.id)}>
              <div className="av">{initials(t.agent_name)}</div>
              <div className="mid">
                <div className="nm">{t.agent_name}</div>
                {/* الهاتف تحت الاسم مباشرةً: موظّف الدعم يحتاجه ليتأكّد
                    ممّن يكلّمه وليتّصل به، وإخفاؤه خلف فتح المحادثة يجعله
                    يفتح محادثاتٍ ليقرأ رقماً. و`num` تفرض الاتجاه اللاتيني
                    وإلا قلبته الفقرةُ العربية. */}
                {t.agent_phone && (
                  <div className="pv num" style={{ marginTop: 2, fontSize: 11.5, opacity: .85 }}>
                    {t.agent_phone}
                  </div>
                )}
                <div className="pv">
                  {t.last_from === 'ADMIN' && <span style={{ opacity: .6 }}>أنت: </span>}
                  {t.last_body || 'لا رسائل بعد'}
                </div>
                {t.assignee_name && (
                  <div className="pv" style={{ marginTop: 2, fontSize: 11, opacity: .8 }}>
                    👤 {t.assignee_name}
                  </div>
                )}
              </div>
              <div className="end">
                <span className="tm">{listStamp(t.last_message_at)}</span>
                <span className={`pill pill-${t.status}`}>{t.status_label}</span>
                {t.unread > 0 && (
                  <span className="unread-dot">{t.unread > 99 ? '99+' : t.unread}</span>
                )}
              </div>
            </button>
          ))}
        </div>
      </div>

      {searching && (
        <MessageSearch
          onClose={() => setSearching(false)}
          onPick={(threadId, messageId) => {
            setSearching(false)
            setOpen(threadId)
            // مفتاحٌ يتغيّر يُعيد بناء المحادثة، فتُفتح على الرسالة المطلوبة
            // ولو كانت المحادثة نفسها مفتوحةً أصلاً.
            setJumpTo(messageId)
          }}
        />
      )}

      {open ? (
        <Conversation
          key={`${open}:${jumpTo}`}
          threadId={open}
          jumpToMessageId={jumpTo}
          can={can}
          me={me}
          statuses={statuses}
          onChanged={load}
          onUnreadTouched={onUnreadChange}
        />
      ) : (
        <div className="empty">
          <div>
            <div className="big">💬</div>
            <div style={{ fontWeight: 700, marginBottom: 6 }}>اختر محادثةً من القائمة</div>
            <div style={{ fontSize: 12.5, maxWidth: 320, lineHeight: 1.7 }}>
              لا تُفتح محادثةٌ من تلقائها: فتحُها يُعلّم رسائل الوكيل «مقروءة»،
              وشرطتان زرقاوان على رسالةٍ لم يقرأها أحدٌ وعدٌ كاذب.
            </div>
          </div>
        </div>
      )}
    </div>
  )
}

/**
 * البحث في نصّ الرسائل — لا في أسماء الوكلاء.
 *
 * ⚠ نطاقُه يأتي من الخادم لا من الطلب: `SupportController::search` تبني
 * قائمة المحادثات من صلاحيات هذا الموظّف نفسِه، فلا يستطيع أحدٌ توسيع بحثه
 * ليشمل محادثةً لا يملك فتحَها.
 *
 * والنتيجةُ تفتح المحادثة **على الرسالة نفسها وتُبرزها** — لا على آخرها.
 * نتيجةُ بحثٍ تعيدك إلى أعلى محادثةٍ فيها مئة رسالة ليست نتيجة.
 */
function MessageSearch({ onClose, onPick }) {
  const [term, setTerm] = useState('')
  const [items, setItems] = useState([])
  const [busy, setBusy] = useState(false)
  const [done, setDone] = useState(false)
  const timer = useRef(null)

  useEffect(() => {
    clearTimeout(timer.current)
    if (term.trim().length < 2) { setItems([]); setDone(false); return }
    setBusy(true)
    timer.current = setTimeout(() => {
      api.search(term.trim())
        .then((d) => { setItems(d.items || []); setDone(true) })
        .catch(() => setItems([]))
        .finally(() => setBusy(false))
    }, 300)
    return () => clearTimeout(timer.current)
  }, [term])

  return (
    <div className="modal-back" onClick={onClose}>
      <div className="modal" style={{ maxWidth: 560 }} onClick={(e) => e.stopPropagation()}>
        <h3>البحث في الرسائل</h3>
        <p className="sub">
          يشمل محادثاتِ الوكلاء التي تملك فتحَها وحدها — بما فيها المغلقة.
        </p>

        <div className="field">
          <input autoFocus value={term} onChange={(e) => setTerm(e.target.value)}
                 placeholder="اكتب حرفين على الأقل…" />
        </div>

        {busy && <div className="spin" />}

        {done && items.length === 0 && !busy && (
          <div className="empty" style={{ height: 90 }}>لا نتائج</div>
        )}

        <div style={{ maxHeight: 340, overflowY: 'auto' }}>
          {items.map((m) => (
            <button key={m.id} className="thread-row" style={{ borderInlineStart: 0 }}
                    onClick={() => onPick(m.thread_id, m.id)}>
              <div className="mid">
                <div className="nm" style={{ fontSize: 12.5 }}>
                  {m.sender_kind === 'ADMIN' ? 'الدعم' : (m.sender_name || 'الوكيل')}
                  <span style={{ fontWeight: 400, color: 'var(--muted)' }}>
                    {' · '}محادثة <span className="num">#{m.thread_id}</span>
                  </span>
                </div>
                <div className="pv" style={{ whiteSpace: 'normal' }}>{m.body}</div>
              </div>
              <div className="end"><span className="tm">{listStamp(m.created_at)}</span></div>
            </button>
          ))}
        </div>

        <div className="modal-acts">
          <button className="btn btn-ghost" onClick={onClose}>إغلاق</button>
        </div>
      </div>
    </div>
  )
}
