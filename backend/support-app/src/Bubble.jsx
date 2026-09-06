import { useEffect, useRef, useState } from 'react'
import { fetchAttachment } from './api'
import { hhmm, mmss } from './util'

/**
 * فقاعة الرسالة — مطابقةٌ لما يراه الوكيل في تطبيقه، عمداً.
 *
 * حين يقول الوكيل «الرسالة التي عليها نجمة» أو «الفقاعة التي فيها الصورة»،
 * يجب أن يرى موظّف الدعم الشيءَ نفسه بالشكل نفسه. واجهتان مختلفتان تجعلان
 * كلَّ مكالمةِ دعمٍ تبدأ بشرح الشاشة بدل حلّ المشكلة.
 *
 * ── تفصيلٌ صغيرٌ مقصود ───────────────────────────────────────────────────
 *
 * الساعةُ **أسفل يسار الفقاعة** لا ملتصقةً بآخر كلمة (قرار المالك). في
 * Flutter بُني ذلك بـ `WidgetSpan` شفّاف يحجز المكان ثم `Positioned` فوقه؛
 * وهنا يكفي `float` على عنصر الوقت داخل النصّ نفسه — فالنصّ يلتفّ حوله
 * والسطرُ الأخير يترك له فراغَه.
 */

const TICK_SENT = '✓'
const TICK_DELIVERED = '✓✓'

function Ticks({ message, receipts }) {
  // الإيصالات تخصّ رسائلنا وحدها.
  if (message.sender_kind !== 'ADMIN') return null

  // رسالةٌ ما زالت في الطريق: رقمٌ سالب = محلّية لم يردّ عليها الخادم بعد.
  if (message.id < 0) return <span className="ticks" title="جارٍ الإرسال">🕐</span>

  const read = (receipts?.read ?? 0) >= message.id
  const delivered = (receipts?.delivered ?? 0) >= message.id

  if (read) return <span className="ticks read" title="قرأها الوكيل">{TICK_DELIVERED}</span>
  if (delivered) return <span className="ticks" title="وصلت">{TICK_DELIVERED}</span>
  return <span className="ticks" title="أُرسلت">{TICK_SENT}</span>
}

/**
 * مرفقٌ يُجلب بترويسة التوثيق ثم يُعرض من `blob:`.
 *
 * ⚠ لا يمكن وضع الرابط في `<img src>` مباشرة: المسار محميّ بـ `Bearer`،
 * و`<img>` لا ترسل ترويسات. ووضعُه بلا حماية كان سيجعل كلَّ من خمّن الاسم
 * يقرأ مرفقات الوكلاء.
 */
function useBlob(name) {
  const [url, setUrl] = useState(null)
  const [failed, setFailed] = useState(false)

  useEffect(() => {
    if (!name) return
    let alive = true
    let made = null

    fetchAttachment(name)
      .then((u) => {
        if (!alive) { URL.revokeObjectURL(u); return }
        made = u
        setUrl(u)
      })
      .catch(() => alive && setFailed(true))

    return () => {
      alive = false
      // تحريرُ الذاكرة: محادثةٌ فيها ثلاثون صورة تُبقي ثلاثين نسخةً في
      // الذاكرة إن لم تُحرَّر عند إغلاقها.
      if (made) URL.revokeObjectURL(made)
    }
  }, [name])

  return { url, failed }
}

function ImageAttachment({ message, onView }) {
  const { url, failed } = useBlob(message.attachment_path)

  if (failed) return <div className="att-file">📷 تعذّر تحميل الصورة</div>
  if (!url) return <div className="att-file">📷 جارٍ التحميل…</div>

  return <img className="att" src={url} alt={message.attachment_name || 'صورة'}
              onClick={() => onView(url)} />
}

function FileAttachment({ message }) {
  const { url, failed } = useBlob(message.attachment_path)
  const kb = Math.round((message.attachment_size || 0) / 1024)

  if (failed) return <div className="att-file">📎 تعذّر تحميل الملفّ</div>

  return (
    <a className="att-file" href={url || undefined} download={message.attachment_name}
       target="_blank" rel="noreferrer">
      <span>📎</span>
      <span style={{ flex: 1, minWidth: 0, overflow: 'hidden', textOverflow: 'ellipsis' }}>
        {message.attachment_name || 'ملفّ'}
      </span>
      {kb > 0 && <span className="num" style={{ opacity: .6, fontSize: 11 }}>{kb} KB</span>}
    </a>
  )
}

/**
 * الرسالة الصوتية.
 *
 * الموجةُ مرسومةٌ من بصمة اسم الملفّ لا من تحليل الصوت — وهو القرار نفسه
 * المتّخذ في `voice_note.dart`: فكُّ ترميز مقطعٍ كامل في المتصفّح لرسم
 * أعمدةٍ زخرفية يُجمّد الصفحة عند كلّ رسالة، والموجةُ لا تحمل معنى يُقرأ.
 * وثباتُها لنفس الملفّ يجعلها تبدو «موجةَ هذه الرسالة» لا عشوائيةً متغيّرة.
 */
function VoiceAttachment({ message }) {
  const { url, failed } = useBlob(message.attachment_path)
  const audioRef = useRef(null)
  const [playing, setPlaying] = useState(false)
  const [pos, setPos] = useState(0)
  const [dur, setDur] = useState(0)
  const [rate, setRate] = useState(1)

  const bars = (() => {
    let h = 0
    const s = String(message.attachment_path || 'x')
    for (let i = 0; i < s.length; i++) h = (h * 31 + s.charCodeAt(i)) >>> 0
    return Array.from({ length: 27 }, () => {
      h = (h * 1103515245 + 12345) >>> 0
      return 25 + ((h >>> 8) % 75)
    })
  })()

  const toggle = () => {
    const a = audioRef.current
    if (!a) return
    if (playing) { a.pause() } else { a.playbackRate = rate; a.play().catch(() => {}) }
  }

  const cycleRate = () => {
    const next = rate === 1 ? 1.5 : rate === 1.5 ? 2 : 1
    setRate(next)
    if (audioRef.current) audioRef.current.playbackRate = next
  }

  if (failed) return <div className="att-file">🎤 تعذّر تحميل التسجيل</div>

  const pct = dur > 0 ? pos / dur : 0

  return (
    <div className="voice">
      <button onClick={toggle} disabled={!url} aria-label={playing ? 'إيقاف' : 'تشغيل'}>
        {playing ? '❚❚' : '▶'}
      </button>
      <div className="wave" onClick={(e) => {
        const a = audioRef.current
        if (!a || !dur) return
        const r = e.currentTarget.getBoundingClientRect()
        // الواجهة RTL: البداية على اليمين.
        const ratio = (r.right - e.clientX) / r.width
        a.currentTime = Math.max(0, Math.min(dur, ratio * dur))
      }}>
        {bars.map((h, i) => (
          <i key={i} className={i / bars.length <= pct ? 'on' : ''} style={{ height: `${h}%` }} />
        ))}
      </div>
      <span className="dur">{mmss(playing || pos > 0 ? pos : dur)}</span>
      <button onClick={cycleRate} style={{
        width: 'auto', height: 22, padding: '0 7px', borderRadius: 11,
        background: 'rgba(15,95,78,.1)', color: 'var(--primary-dark)',
        fontSize: 10.5, fontWeight: 700,
      }}>{rate}×</button>
      {url && (
        <audio ref={audioRef} src={url} preload="metadata"
               onPlay={() => setPlaying(true)}
               onPause={() => setPlaying(false)}
               onEnded={() => { setPlaying(false); setPos(0) }}
               onTimeUpdate={(e) => setPos(e.currentTarget.currentTime)}
               onLoadedMetadata={(e) => {
                 const d = e.currentTarget.duration
                 // Opus في بعض المتصفّحات يُبلّغ `Infinity` قبل أول تشغيل.
                 setDur(Number.isFinite(d) ? d : 0)
               }} />
      )}
    </div>
  )
}

export default function Bubble({
  message, receipts, reactions, starred, highlighted,
  onMenu, onQuoteClick, onView,
}) {
  const mine = message.sender_kind === 'ADMIN'
  const ref = useRef(null)
  const timer = useRef(null)

  useEffect(() => {
    if (highlighted && ref.current) {
      ref.current.scrollIntoView({ block: 'center', behavior: 'smooth' })
    }
  }, [highlighted])

  // ضغطةٌ مطوّلة تفتح القائمة — بأمر المالك، بدل سهمٍ صغير قد تُخطئه الإصبع.
  // وزرُّ الفأرة الأيمن يفعل الشيء نفسه: هذه شاشةُ حاسوب.
  const startHold = (e) => {
    const x = e.touches?.[0]?.clientX ?? e.clientX
    const y = e.touches?.[0]?.clientY ?? e.clientY
    timer.current = setTimeout(() => onMenu(message, x, y), 480)
  }
  const cancelHold = () => {
    clearTimeout(timer.current)
    timer.current = null
  }

  if (message.deleted_at) {
    return (
      <div className={`row ${mine ? 'mine' : 'theirs'}`}>
        <div className={`bubble ${mine ? 'mine' : 'theirs'}`} style={{ opacity: .6 }}>
          <div className="txt" style={{ fontStyle: 'italic', color: 'var(--muted)' }}>
            🚫 حُذفت هذه الرسالة
          </div>
        </div>
      </div>
    )
  }

  const kind = message.attachment_kind

  return (
    <div className={`row ${mine ? 'mine' : 'theirs'}`}>
      <div
        ref={ref}
        className={`bubble ${mine ? 'mine' : 'theirs'}${highlighted ? ' hl' : ''}`}
        onMouseDown={startHold}
        onMouseUp={cancelHold}
        onMouseLeave={cancelHold}
        onTouchStart={startHold}
        onTouchEnd={cancelHold}
        onContextMenu={(e) => { e.preventDefault(); onMenu(message, e.clientX, e.clientY) }}
      >
        {/* اسمُ من ردّ من الدعم — الوكيل يرى «الإدارة»، ونحن نرى الشخص. */}
        {mine && (message.staff_name || message.sender_name) && (
          <div className="sender">{message.staff_name || message.sender_name}</div>
        )}

        {message.reply_to_id > 0 && (
          <div className="quote" onClick={(e) => { e.stopPropagation(); onQuoteClick(message.reply_to_id) }}>
            <b>{message.reply_sender_kind === 'ADMIN' ? 'الدعم' : (message.reply_sender_name || 'الوكيل')}</b>
            <span>
              {message.reply_body
                || (message.reply_attachment_kind === 'IMAGE' ? '📷 صورة'
                  : message.reply_attachment_kind === 'AUDIO' ? '🎤 رسالة صوتية'
                  : message.reply_attachment_kind ? '📎 ملفّ' : '')}
            </span>
          </div>
        )}

        {kind === 'IMAGE' && <ImageAttachment message={message} onView={onView} />}
        {kind === 'AUDIO' && <VoiceAttachment message={message} />}
        {kind === 'FILE' && <FileAttachment message={message} />}

        <div className="txt">
          {message.body}
          <span className="meta">
            {starred && <span title="مهمّة">⭐</span>}
            {message.pinned_at && <span title="مثبَّتة">📌</span>}
            {message.edited_at && <span>مُعدَّلة</span>}
            <span className="num">{hhmm(message.created_at)}</span>
            <Ticks message={message} receipts={receipts} />
          </span>
        </div>

        {/* الخادم يُرجع كائناً مفتاحُه الإيموجي: `{"👍": {count, mine}}`
            — لا مصفوفةً. `Object.entries` هي الشكل الصحيح لقراءته. */}
        {reactions && Object.keys(reactions).length > 0 && (
          <div className="reactions">
            {Object.entries(reactions).map(([emoji, r]) => (
              <span key={emoji} className={r.mine ? 'mine' : ''}>
                {emoji} <span className="num">{r.count}</span>
              </span>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
