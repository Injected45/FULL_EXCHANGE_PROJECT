import { useCallback, useEffect, useMemo, useRef, useState } from 'react'
import { api } from './api'
import Bubble from './Bubble'
import { dayLabel, hhmm, mmss, newClientId, sameDay } from './util'

const EMOJI = ['👍', '❤️', '😂', '😮', '😢', '🤲']

/**
 * ── النبض المتكيّف ───────────────────────────────────────────────────────
 *
 * فترةٌ واحدة ثابتة تُجبرك على اختيارٍ سيّئ: أربعُ ثوانٍ بطيئةٌ أثناء حديثٍ
 * جارٍ، وثانيةٌ واحدة إسرافٌ على محادثةٍ لم يكتب فيها أحدٌ منذ ساعة.
 *
 * فالفترة تتبع الحال: **1.2 ثانية** ما دام أحد الطرفين يكتب أو مرّت رسالةٌ
 * في الدقيقة الماضية، و**3 ثوانٍ** حين تهدأ. والانتقال بينهما فوريّ — أوّل
 * رسالةٍ تصل تُسرّع النبض من تلقائها.
 *
 * وصار ذلك ممكناً لأن النبضة نفسها رخصت: **ثلاث رحلاتٍ إلى القاعدة بدل
 * ستّ عشرة** (مقيسة). بالتكلفة القديمة كانت 1.2 ثانية ستُغرق الخادم.
 */
const POLL_HOT_MS = 1200
const POLL_IDLE_MS = 3000
const HOT_WINDOW_MS = 60000

/**
 * شاشة المحادثة.
 *
 * ── الإرسال المتفائل، ولماذا هو ضرورةٌ لا زينة ────────────────────────────
 *
 * الرسالة تظهر في الشاشة **قبل** أن يردّ الخادم، برقمٍ سالبٍ مؤقّت. وحين
 * يردّ، تُستبدل بالحقيقية بمطابقة `client_id`.
 *
 * وهو حلُّ عيبين اصطدم بهما تطبيق الوكيل فعلاً: الرسالة كانت تتأخّر ثانيةً
 * كاملة قبل أن تُرى، **وكانت تظهر مرّتين** — لأن الاستطلاع كان يلتقط
 * `after_id` قبل أن يعود ردُّ الإرسال. المطابقة بـ `client_id` تحلّ
 * الاثنين، ولذلك يُرسَل مع النصّ والمرفق معاً.
 *
 * ── ولماذا الاستطلاع لا WebSocket ────────────────────────────────────────
 *
 * WebSocket في Laravel يعني Pusher (اشتراك شهري) أو خادم Reverb يُشغَّل
 * ويُراقَب. وأمرُ المالك صريح في إلغاء ما له تكلفة. أربعُ ثوانٍ داخل محادثةٍ
 * مفتوحة فرقٌ لا يُلاحَظ في عمل الدعم، وهي **الآلية نفسها** التي يستعملها
 * تطبيق الوكيل — فالطرفان متساويان في التأخير.
 */
export default function Conversation({
  threadId, jumpToMessageId, can, me, statuses, onChanged, onUnreadTouched, onBack,
}) {
  const [msgs, setMsgs] = useState([])
  const [receipts, setReceipts] = useState({ delivered: 0, read: 0 })
  const [reactions, setReactions] = useState({})
  const [starred, setStarred] = useState([])
  const [typing, setTyping] = useState(null)
  const [pinned, setPinned] = useState(null)
  const [agent, setAgent] = useState(null)
  const [state, setState] = useState(null)
  const [assignees, setAssignees] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  const [text, setText] = useState('')
  const [replyTo, setReplyTo] = useState(null)
  const [menu, setMenu] = useState(null)
  const [editing, setEditing] = useState(null)
  const [highlight, setHighlight] = useState(0)
  const [viewer, setViewer] = useState(null)
  const [busy, setBusy] = useState(false)

  // ── التشغيل (بنود 3 · 4 · 7 · 12 · 17) ─────────────────────────────
  const [tax, setTax] = useState(null)        // التصنيفات والوسوم والأولويات
  const [canInternal, setCanInternal] = useState(false)
  const [isNote, setIsNote] = useState(false) // الوضع الحالي للملحن
  const [panel, setPanel] = useState('')      // '' | 'tags' | 'timeline'
  const [timeline, setTimeline] = useState([])

  const lastServerId = useRef(0)
  // إلى متى يبقى النبض سريعاً — تُرفع مع كل رسالةٍ أو «يكتب الآن».
  const hotUntil = useRef(0)
  const scroller = useRef(null)
  const nearBottom = useRef(true)
  const typingSentAt = useRef(0)
  const typingStop = useRef(null)
  const fileInput = useRef(null)
  const textarea = useRef(null)

  // ── الدمج: بالرقم أوّلاً ثم بـ client_id ───────────────────────────
  //
  // الترتيبُ مهمّ: رسالةٌ عادت من الخادم تحمل رقماً حقيقياً و`client_id`
  // معاً، ومطابقةُ الرقم وحدها كانت ستُبقي النسخة المتفائلة إلى جانبها.
  const merge = useCallback((incoming) => {
    setMsgs((prev) => {
      const out = [...prev]
      for (const m of incoming) {
        let i = out.findIndex((x) => x.id === m.id)
        if (i < 0 && m.client_id) {
          i = out.findIndex((x) => x.client_id && x.client_id === m.client_id)
        }
        if (i >= 0) out[i] = { ...out[i], ...m }
        else out.push(m)
      }
      // الترتيب بالرقم، والسوالبُ (المحلّية) في آخر القائمة دائماً.
      out.sort((a, b) => {
        const A = a.id < 0 ? Number.MAX_SAFE_INTEGER + a.id : a.id
        const B = b.id < 0 ? Number.MAX_SAFE_INTEGER + b.id : b.id
        return A - B
      })
      return out
    })
  }, [])

  const load = useCallback(async (afterId, signal) => {
    const d = await api.messages(threadId, afterId, signal)

    if (d.items?.length) {
      merge(d.items)
      const max = Math.max(...d.items.map((m) => m.id).filter((n) => n > 0))
      if (max > lastServerId.current) lastServerId.current = max
      // رسالةٌ وصلت ⇐ الحديث جارٍ، فيُسرَّع النبض.
      hotUntil.current = Date.now() + HOT_WINDOW_MS
    }

    // والطرف الآخر يكتب أو يسجّل ⇐ ردٌّ في الطريق، فلا يُنتظر ثلاث ثوانٍ.
    if (d.typing) hotUntil.current = Date.now() + HOT_WINDOW_MS

    setReceipts(d.receipts || { delivered: 0, read: 0 })
    setTyping(d.typing || null)

    // ⚠ `pinned_known` تميّز «لا رسالة مثبَّتة» من «لم تُسأل هذه النبضة».
    //
    // الخادم يقرأ المثبَّتة عند الفتح وحده — قراءتُها كل نبضتين رحلةٌ إلى
    // قاعدةٍ بعيدة عن شيءٍ يتغيّر مرّةً في اليوم. ولو أخذنا `null` على
    // ظاهرها لاختفى شريط التثبيت بعد أوّل نبضة.
    if (d.pinned_known) setPinned(d.pinned || null)
    if (d.agent) setAgent(d.agent)
    if (d.state) setState(d.state)
    if (typeof d.can_internal === 'boolean') setCanInternal(d.can_internal)

    // التفاعلات والنجوم تعودان للصفحة المطلوبة وحدها، فتُدمجان لا تُستبدلان
    // — وإلا اختفت نجومُ ما فوق الشاشة مع كل نبضة.
    if (d.reactions) setReactions((p) => (afterId === 0 ? d.reactions : { ...p, ...d.reactions }))
    if (d.starred) setStarred((p) => (afterId === 0 ? d.starred : [...new Set([...p, ...d.starred])]))
  }, [threadId, merge])

  // ── فتحُ محادثةٍ جديدة ──────────────────────────────────────────────
  useEffect(() => {
    let alive = true
    const ac = new AbortController()

    setMsgs([]); setReactions({}); setStarred([]); setPinned(null)
    setReplyTo(null); setEditing(null); setHighlight(0); setError('')
    setLoading(true)
    lastServerId.current = 0
    nearBottom.current = true

    load(0, ac.signal)
      .then(() => {
        // القدومُ من نتيجة بحث: تُبرَز الرسالة ويُقفَز إليها، ولا يُنزَل إلى
        // آخر المحادثة. و`nearBottom` تُطفأ أوّلاً وإلا سحبَها التمريرُ
        // التلقائي إلى الأسفل بعد لحظة.
        if (alive && jumpToMessageId > 0) {
          nearBottom.current = false
          setHighlight(jumpToMessageId)
          setTimeout(() => alive && setHighlight(0), 2600)
        }
      })
      .catch((e) => alive && e.name !== 'AbortError' && setError(e.message))
      .finally(() => alive && setLoading(false))

    api.assignees().then((d) => alive && setAssignees(d.items || [])).catch(() => {})

    // التصنيفات تُجلب مرّةً لكل محادثةٍ تُفتح — قوائمُ صغيرة ثابتة، ونداءٌ
    // واحد يحمل الثلاثة (انظر `taxonomy` في الخادم).
    api.taxonomy().then((d) => alive && setTax(d)).catch(() => {})

    setPanel('')
    setIsNote(false)

    return () => { alive = false; ac.abort() }
  }, [threadId, load, jumpToMessageId])

  // الشريط الزمني يُجلب عند فتح لوحته لا مع كل نبضة: يتغيّر عند فعلٍ لا
  // مع الوقت، وجلبُه كل ثانيتين رحلةٌ إلى قاعدةٍ بعيدة بلا جديد.
  const loadTimeline = useCallback(async () => {
    try {
      const d = await api.timeline(threadId)
      setTimeline(d.items || [])
    } catch { /* لوحةٌ فارغة خيرٌ من شاشةٍ تتعطّل */ }
  }, [threadId])

  // ── الاستطلاع المتكيّف ─────────────────────────────────────────────
  //
  // `setTimeout` متسلسل لا `setInterval`: الثاني يُطلق نبضةً جديدة ولو لم
  // تعد السابقة، فتتكدّس الطلبات على شبكةٍ بطيئة. وهذا يبدأ العدّ **بعد**
  // انتهاء النبضة، فلا يتجاوز الخادمَ طلبٌ معلّق أبداً.
  useEffect(() => {
    const ac = new AbortController()
    let timer = null
    let alive = true

    const tick = async () => {
      try {
        await load(lastServerId.current, ac.signal)
        onUnreadTouched?.()
      } catch { /* انقطاعٌ لحظي — النبضة التالية تُصلحه */ }

      if (!alive) return
      const hot = Date.now() < hotUntil.current
      timer = setTimeout(tick, hot ? POLL_HOT_MS : POLL_IDLE_MS)
    }

    timer = setTimeout(tick, POLL_HOT_MS)
    return () => { alive = false; clearTimeout(timer); ac.abort() }
  }, [load, onUnreadTouched])

  // ── التمرير إلى الأسفل ─────────────────────────────────────────────
  //
  // بشرط أن يكون قريباً منه أصلاً: من يقرأ رسالةً قديمة لا يُقفَز به إلى
  // الأسفل كلّما وصلت رسالة، فيفقد موضعه في كل مرّة.
  useEffect(() => {
    const el = scroller.current
    if (el && nearBottom.current) el.scrollTop = el.scrollHeight
  }, [msgs])

  const onScroll = (e) => {
    const el = e.currentTarget
    nearBottom.current = el.scrollHeight - el.scrollTop - el.clientHeight < 130
  }

  // ── «يكتب الآن» — مخنوقٌ لا مع كل حرف ─────────────────────────────
  //
  // ثلاثُ ثوانٍ بين إشارتين، وإيقافٌ بعد ثلاثٍ من التوقّف. وإرسالُها مع كل
  // ضغطة مفتاح كان سيعني عشرين طلباً في جملةٍ واحدة.
  const onType = (v) => {
    setText(v)
    // ⚠ لا «يكتب الآن» أثناء كتابة ملاحظة: الوكيل يرى المؤشّر فينتظر
    // ردّاً لن يأتي — والانتظارُ على وعدٍ كاذب أسوأ من الصمت.
    if (isNote || !can('REPLY')) return

    const now = Date.now()
    if (v && now - typingSentAt.current > 3000) {
      typingSentAt.current = now
      api.typing(threadId, 'TYPING').catch(() => {})
    }
    clearTimeout(typingStop.current)
    typingStop.current = setTimeout(() => {
      typingSentAt.current = 0
      api.typing(threadId, 'STOP').catch(() => {})
    }, 3000)
  }

  useEffect(() => () => clearTimeout(typingStop.current), [threadId])

  // ── الإرسال ────────────────────────────────────────────────────────
  const send = async (file) => {
    const body = text.trim()
    if (!body && !file) return
    if (busy) return

    const clientId = newClientId()

    /*
     * ⚠ الوضعُ يُلتقط **الآن** لا وقتَ وصول الردّ.
     *
     * `send` غير متزامنة، والموظّف قد يبدّل الوضع أثناء الإرسال. وقراءةُ
     * `isNote` بعد `await` تعني أن ملاحظةً قد تُرسَل ردّاً — أو العكس.
     */
    const noteNow = isNote

    const optimistic = {
      id: -Date.now(),
      thread_id: threadId,
      sender_kind: 'ADMIN',
      sender_name: me.name,
      staff_name: me.name,
      body,
      client_id: clientId,
      created_at: new Date().toISOString(),
      reply_to_id: replyTo?.id || 0,
      reply_body: replyTo?.body || null,
      reply_sender_kind: replyTo?.sender_kind || null,
      reply_sender_name: replyTo?.sender_name || null,
      // الفقاعة المتفائلة تحمل الوضع نفسه — وإلا ظهرت الملاحظةُ رسالةً
      // عاديّة لثانيةٍ ثم تغيّر شكلُها، وتلك الثانيةُ تكفي لسوء الفهم.
      is_internal: noteNow,
      attachment_kind: file
        ? (file.type.startsWith('image/') ? 'IMAGE'
          : file.type.startsWith('audio/') ? 'AUDIO' : 'FILE')
        : null,
      attachment_name: file?.name || null,
    }

    merge([optimistic])
    setText('')
    const keepReply = replyTo
    setReplyTo(null)
    nearBottom.current = true
    setBusy(true)
    // أرسلتُ ⇐ ردٌّ متوقَّع، فيُسرَّع النبض من الآن لا بعد وصوله.
    hotUntil.current = Date.now() + HOT_WINDOW_MS

    try {
      const d = await api.send(threadId, {
        body, clientId, replyToId: keepReply?.id || 0, file,
        internal: noteNow,
      })
      if (d.message) {
        merge([d.message])
        if (d.message.id > lastServerId.current) lastServerId.current = d.message.id
      }
      onChanged?.()
    } catch (e) {
      // الفاشلةُ تُوسَم ولا تُحذف: حذفُها يجعل الموظّف يظنّ أنها وصلت.
      setMsgs((p) => p.map((m) => (m.client_id === clientId
        ? { ...m, failed: true, body: (m.body || '') + '  ⚠ لم تُرسَل' }
        : m)))
      setError(e.message)
    } finally {
      setBusy(false)
      clearTimeout(typingStop.current)
      api.typing(threadId, 'STOP').catch(() => {})
    }
  }

  // ── التسجيل الصوتي ─────────────────────────────────────────────────
  //
  // `MediaRecorder` مبنيٌّ في المتصفّح — بلا حزمةٍ ولا خدمة. و Opus في
  // WebM هو ما تدعمه المتصفّحات، وهو مقبولٌ في الخادم (`audio/ogg` /
  // `audio/mp4` في `ChatService::MIMES`)، فيُرسَل بالنوع الذي يقبله.
  const [rec, setRec] = useState(null)
  const recRef = useRef(null)
  const chunks = useRef([])
  const recTimer = useRef(null)

  const startRec = async () => {
    if (!can('SEND_VOICE')) return
    try {
      const stream = await navigator.mediaDevices.getUserMedia({
        audio: { echoCancellation: true, noiseSuppression: true, channelCount: 1 },
      })
      const mime = MediaRecorder.isTypeSupported('audio/webm;codecs=opus')
        ? 'audio/webm;codecs=opus'
        : MediaRecorder.isTypeSupported('audio/ogg;codecs=opus')
          ? 'audio/ogg;codecs=opus' : ''
      const mr = new MediaRecorder(stream, mime ? { mimeType: mime, audioBitsPerSecond: 24000 } : {})
      chunks.current = []
      mr.ondataavailable = (e) => e.data.size && chunks.current.push(e.data)
      mr.start(250)
      recRef.current = { mr, stream }
      setRec({ seconds: 0 })
      recTimer.current = setInterval(() => setRec((r) => r && { seconds: r.seconds + 1 }), 1000)
      api.typing(threadId, 'RECORDING').catch(() => {})
    } catch {
      setError('تعذّر الوصول إلى الميكروفون. تحقّق من إذن المتصفّح.')
    }
  }

  const stopRec = (keep) => {
    const r = recRef.current
    if (!r) return
    clearInterval(recTimer.current)

    r.mr.onstop = () => {
      r.stream.getTracks().forEach((t) => t.stop())
      if (keep && chunks.current.length) {
        const type = r.mr.mimeType?.split(';')[0] || 'audio/ogg'
        const blob = new Blob(chunks.current, { type })
        // الاسم يحمل الامتداد الصحيح: الخادم يقرأ النوع من المحتوى، لكن
        // الاسمَ هو ما يُعرض ويُحفظ.
        const ext = type.includes('webm') ? 'webm' : type.includes('mp4') ? 'm4a' : 'ogg'
        send(new File([blob], `voice-${Date.now()}.${ext}`, { type }))
      }
      chunks.current = []
    }
    r.mr.stop()
    recRef.current = null
    setRec(null)
    api.typing(threadId, 'STOP').catch(() => {})
  }

  useEffect(() => () => {
    // مغادرةُ الشاشة أثناء التسجيل تُطفئ الميكروفون — وإلا بقي ضوءُه
    // مضاءً والمتصفّح يسجّل بلا أن يعرف أحد.
    if (recRef.current) {
      clearInterval(recTimer.current)
      recRef.current.mr.onstop = null
      recRef.current.mr.stop()
      recRef.current.stream.getTracks().forEach((t) => t.stop())
      recRef.current = null
    }
  }, [threadId])

  // ── أفعال القائمة ──────────────────────────────────────────────────
  const act = async (fn, after) => {
    try { await fn(); after?.(); await load(0) }
    catch (e) { setError(e.message) }
    finally { setMenu(null) }
  }

  const jumpTo = (id) => {
    setHighlight(id)
    setTimeout(() => setHighlight(0), 2600)
  }

  // ── الفواصل اليومية ────────────────────────────────────────────────
  const rows = useMemo(() => {
    const out = []
    msgs.forEach((m, i) => {
      const prev = msgs[i - 1]
      if (!prev || !sameDay(prev.created_at, m.created_at)) {
        out.push({ day: dayLabel(m.created_at), key: `d${m.id}` })
      }
      out.push({ msg: m, key: `m${m.id}` })
    })
    return out
  }, [msgs])

  const changeStatus = async (status) => {
    let note = ''
    if (status === 'CLOSED') {
      note = window.prompt('ملاحظة الإغلاق (اختيارية):') ?? ''
    }
    try {
      await api.setStatus(threadId, status, note)
      await load(0)
      onChanged?.()
    } catch (e) { setError(e.message) }
  }

  /** فعلٌ تشغيليّ ثم إعادةُ قراءة — الحالةُ والشريط يتغيّران معاً. */
  const opsAct = async (fn) => {
    try {
      await fn()
      await load(0)
      if (panel === 'timeline') await loadTimeline()
      onChanged?.()
    } catch (e) { setError(e.message) }
  }

  const changeAssignee = async (v) => {
    try {
      await api.assign(threadId, v === '' ? null : Number(v))
      await load(0)
      onChanged?.()
    } catch (e) { setError(e.message) }
  }

  if (loading) return <div className="conv"><div className="spin" /></div>

  const closed = state?.status === 'CLOSED'

  return (
    <div className="conv">
      {/* ── الترويسة ── */}
      <div className="conv-head">
        {/*
          ⚠ زرّ الرجوع — على الشاشة الضيّقة وحدها.

          فتحُ محادثةٍ على الهاتف يُخفي قائمة المحادثات (`has-open`)، فبغير
          هذا الزرّ يعلق الموظّف داخلها: زرُّ الرجوع في أندرويد ينادي
          `goBack` على الـ WebView، وهذه صفحةٌ واحدة بلا سجلّ تنقّل — فلا
          يفعل شيئاً. وعلى الحاسوب القائمةُ ظاهرةٌ إلى جانبها فيُخفى.
        */}
        <button className="back-btn" onClick={onBack} aria-label="رجوع">‹</button>

        <div className="t">
          <b>{agent?.name || `محادثة #${threadId}`}</b>
          <span className="num">
            {agent?.phone || ''}
            {/* الرقم المرجعي بجوار الهاتف: كلاهما يُملى في مكالمة. */}
            {state?.reference && (
              <>{' · '}<span className="ref" title="الرقم المرجعي للحالة">{state.reference}</span></>
            )}
          </span>
        </div>
        <div className="acts">
          <span className={`pill pill-${state?.status || 'NEW'}`}>{state?.status_label}</span>

          {/* الأولوية — قائمةٌ مصغّرة بلونها. */}
          {tax?.priorities && (
            <select
              className="prio-select"
              value={state?.priority || 'NORMAL'}
              style={{ color: state?.priority_color, borderColor: (state?.priority_color || '') + '66' }}
              onChange={(e) => opsAct(() => api.setPriority(threadId, e.target.value))}
            >
              {Object.entries(tax.priorities).map(([k, v]) => (
                <option key={k} value={k}>{v.label}</option>
              ))}
            </select>
          )}

          {/* التصنيف */}
          {tax?.categories && (
            <select
              value={state?.category_id ?? ''}
              onChange={(e) => opsAct(() =>
                api.setCategory(threadId, e.target.value === '' ? null : Number(e.target.value)))}
            >
              <option value="">— بلا تصنيف —</option>
              {tax.categories.map((c) => (
                <option key={c.id} value={c.id}>{c.name}</option>
              ))}
            </select>
          )}

          <select value={state?.assigned_to ?? ''} onChange={(e) => changeAssignee(e.target.value)}>
            <option value="">— بلا مالك —</option>
            {assignees.map((a) => (
              <option key={a.id} value={a.id}>{a.name} · {a.role}</option>
            ))}
          </select>

          <select value={state?.status || 'NEW'} onChange={(e) => changeStatus(e.target.value)}>
            {Object.entries(statuses || {}).map(([k, v]) => (
              <option key={k} value={k}>{v}</option>
            ))}
          </select>

          <button className={`btn btn-ghost btn-sm${panel === 'tags' ? ' on' : ''}`}
                  onClick={() => setPanel(panel === 'tags' ? '' : 'tags')}>
            🏷 الوسوم{state?.tags?.length ? ` (${state.tags.length})` : ''}
          </button>
          <button className={`btn btn-ghost btn-sm${panel === 'timeline' ? ' on' : ''}`}
                  onClick={() => {
                    const next = panel === 'timeline' ? '' : 'timeline'
                    setPanel(next)
                    if (next) loadTimeline()
                  }}>
            🕘 السجلّ
          </button>
        </div>
      </div>

      {/* ── لوحة الوسوم ── */}
      {panel === 'tags' && (
        <div className="ops-panel">
          <div className="ops-title">الوسوم المرفقة بهذه الحالة</div>
          <div className="tag-cloud">
            {(tax?.tags || []).map((g) => {
              const on = (state?.tags || []).some((x) => x.id === g.id)
              return (
                <button
                  key={g.id}
                  className={`tag pick${on ? ' on' : ''}`}
                  style={g.color && on ? {
                    background: g.color + '22', borderColor: g.color, color: g.color,
                  } : undefined}
                  onClick={() => opsAct(() =>
                    on ? api.removeTag(threadId, g.id) : api.addTag(threadId, g.id))}
                >
                  {on ? '✓ ' : '+ '}{g.name}
                </button>
              )
            })}
            {(tax?.tags || []).length === 0 && (
              <span className="ops-empty">لا وسوم بعد — تُضاف من شاشة الإدارة.</span>
            )}
          </div>
        </div>
      )}

      {/* ── الشريط الزمني (البند 17) ── */}
      {panel === 'timeline' && (
        <div className="ops-panel">
          <div className="ops-title">
            سجلّ الحالة
            <span className="ops-hint">
              أحداثٌ تشغيلية فقط — لا يحتوي نصَّ الرسائل
            </span>
          </div>
          <div className="timeline">
            {timeline.map((e) => (
              <div className="tl-row" key={e.id}>
                <span className="tl-time num">{hhmm(e.created_at)}</span>
                <span className="tl-dot" />
                <span className="tl-body">
                  <b>{e.label}</b>
                  {e.from && e.to && (
                    <span className="tl-change">
                      {' '}{e.from} <span className="tl-arrow">←</span> {e.to}
                    </span>
                  )}
                  {!e.from && e.to && <span className="tl-change"> {e.to}</span>}
                  {e.actor_name && <span className="tl-actor"> — {e.actor_name}</span>}
                  {e.note && <div className="tl-note">{e.note}</div>}
                </span>
                <span className="tl-day">{dayLabel(e.created_at)}</span>
              </div>
            ))}
            {timeline.length === 0 && (
              <span className="ops-empty">لا أحداث بعد على هذه الحالة.</span>
            )}
          </div>
        </div>
      )}

      {/* ── المثبَّتة ── */}
      {pinned && (
        <div className="pinned-bar">
          <span>📌</span>
          <span className="txt" onClick={() => jumpTo(pinned.id)} style={{ cursor: 'pointer' }}>
            {pinned.body || 'مرفق'}
          </span>
          {can('PIN_MESSAGE') && (
            <button className="btn btn-ghost btn-sm"
                    onClick={() => act(() => api.pin(threadId, pinned.id, 0))}>
              إلغاء
            </button>
          )}
        </div>
      )}

      {/* ── الرسائل ── */}
      <div className="msgs" ref={scroller} onScroll={onScroll}>
        {error && <div className="alert alert-error" onClick={() => setError('')}>{error}</div>}
        {rows.map((r) => r.day !== undefined
          ? <div className="day-chip" key={r.key}>{r.day}</div>
          : (
            <Bubble
              key={r.key}
              message={r.msg}
              receipts={receipts}
              reactions={reactions[r.msg.id]}
              starred={starred.includes(r.msg.id)}
              highlighted={highlight === r.msg.id}
              onMenu={(m, x, y) => setMenu({ m, x, y })}
              onQuoteClick={jumpTo}
              onView={setViewer}
            />
          ))}
        {msgs.length === 0 && (
          <div className="empty"><div><div className="big">💬</div>لا رسائل في هذه المحادثة</div></div>
        )}
      </div>

      {/* ── الملحن ── */}
      <div className={`composer${isNote ? ' note-mode' : ''}`}>
        {/*
          ⚠ وضعُ الملاحظة يُعلَن بشريطٍ كامل لا بزرٍّ مضاء.
          نصُّ البند 7: «حتى لا يتم إرسالها للوكيل بالخطأ» — والعكسُ أخطر:
          أن يظنّ الموظّف أنه يكتب ملاحظةً فيرسلها ردّاً. فالشريطُ يشغل
          عرض الملحن كلَّه ويتغيّر لونُه ونصُّ الحقل معه.
        */}
        {canInternal && (
          <div className="mode-bar">
            <button className={`mode${!isNote ? ' on' : ''}`} onClick={() => setIsNote(false)}>
              ↩ ردٌّ على الوكيل
            </button>
            <button className={`mode note${isNote ? ' on' : ''}`} onClick={() => setIsNote(true)}>
              🔒 ملاحظة داخلية
            </button>
          </div>
        )}

        {isNote && (
          <div className="note-hint">
            ما تكتبه هنا <b>لا يصل الوكيل</b> — يراه موظّفو الدعم وحدهم.
          </div>
        )}

        {typing && (
          <div className="typing-bar">
            {typing.state === 'RECORDING' ? '🎤 الوكيل يسجّل رسالة صوتية…' : '✍ الوكيل يكتب الآن…'}
          </div>
        )}

        {replyTo && (
          <div className="reply-strip">
            <div className="g">
              <b>{replyTo.sender_kind === 'ADMIN' ? 'الدعم' : (agent?.name || 'الوكيل')}</b>
              <span>{replyTo.body || 'مرفق'}</span>
            </div>
            <button className="btn btn-ghost btn-sm" onClick={() => setReplyTo(null)}>✕</button>
          </div>
        )}

        {closed && (
          <div className="alert alert-warn" style={{ marginBottom: 8 }}>
            هذه المحادثة مغلقة. الردّ فيها يبقى ممكناً، ورسالةٌ من الوكيل تعيد فتحها.
          </div>
        )}

        {rec ? (
          <div className="recording">
            <span className="rec-dot" />
            <span className="num" style={{ fontWeight: 700 }}>{mmss(rec.seconds)}</span>
            <span style={{ color: 'var(--muted)', fontSize: 12.5 }}>جارٍ التسجيل…</span>
            <div style={{ flex: 1 }} />
            <button className="btn btn-ghost" onClick={() => stopRec(false)}>إلغاء</button>
            <button className="btn btn-primary" onClick={() => stopRec(true)}>إرسال</button>
          </div>
        ) : (
          <div className="composer-row">
            {can('SEND_ATTACHMENT') && (
              <>
                <button className="icon-btn" title="إرفاق ملفّ"
                        onClick={() => fileInput.current?.click()}>📎</button>
                <input ref={fileInput} type="file" hidden
                       accept="image/*,audio/*,application/pdf"
                       onChange={(e) => {
                         const f = e.target.files?.[0]
                         e.target.value = ''
                         if (f) send(f)
                       }} />
              </>
            )}

            <textarea
              ref={textarea}
              value={text}
              placeholder={
                isNote ? 'اكتب ملاحظةً لزملائك…'
                : can('REPLY') ? 'اكتب رسالتك…'
                : 'لا تملك صلاحية الردّ'
              }
              disabled={!isNote && !can('REPLY')}
              rows={1}
              onChange={(e) => {
                onType(e.target.value)
                e.target.style.height = 'auto'
                e.target.style.height = `${Math.min(130, e.target.scrollHeight)}px`
              }}
              onKeyDown={(e) => {
                // Enter يُرسل، و Shift+Enter سطرٌ جديد — العُرف المتوقَّع.
                if (e.key === 'Enter' && !e.shiftKey) {
                  e.preventDefault()
                  send()
                  e.target.style.height = 'auto'
                }
              }}
            />

            {can('SEND_VOICE') && !text.trim() && (
              <button className="icon-btn" title="رسالة صوتية" onClick={startRec}>🎤</button>
            )}

            <button className="icon-btn send" disabled={!can('REPLY') || busy || !text.trim()}
                    onClick={() => send()} title="إرسال">➤</button>
          </div>
        )}
      </div>

      {/* ── قائمة الفقاعة ── */}
      {menu && (
        <>
          <div className="menu-back" onClick={() => setMenu(null)} />
          <div className="menu" style={{
            // تُثبَّت داخل الشاشة: قائمةٌ تُفتح عند حافّة النافذة تخرج منها.
            top: Math.min(menu.y, window.innerHeight - 330),
            left: Math.min(Math.max(8, menu.x - 90), window.innerWidth - 200),
          }}>
            {can('REPLY') && (
              <div className="emoji-row">
                {EMOJI.map((e) => (
                  <button key={e} onClick={() => act(() => api.react(threadId, menu.m.id, e))}>{e}</button>
                ))}
              </div>
            )}
            {can('REPLY') && (
              <button onClick={() => { setReplyTo(menu.m); setMenu(null); textarea.current?.focus() }}>
                ↩ ردّ
              </button>
            )}
            {can('FORWARD_MESSAGE') && (
              <button onClick={() => {
                const to = window.prompt('رقم المحادثة المُراد التوجيه إليها:')
                if (to) act(() => api.forward(threadId, menu.m.id, Number(to)))
                else setMenu(null)
              }}>↪ إعادة توجيه</button>
            )}
            <button onClick={() => {
              navigator.clipboard?.writeText(menu.m.body || '').catch(() => {})
              setMenu(null)
            }}>⧉ نسخ</button>
            {can('REPLY') && (
              <button onClick={() => act(() => api.star(threadId, menu.m.id, !starred.includes(menu.m.id)))}>
                {starred.includes(menu.m.id) ? '☆ إزالة التمييز' : '⭐ تمييز بنجمة'}
              </button>
            )}
            {can('PIN_MESSAGE') && (
              <button onClick={() => setMenu({ ...menu, pinning: true })}>📌 تثبيت…</button>
            )}
            {can('EDIT_OWN_MESSAGE') && menu.m.sender_kind === 'ADMIN' && (
              <button onClick={() => { setEditing(menu.m); setMenu(null) }}>✎ تعديل</button>
            )}
          </div>

          {menu.pinning && (
            <div className="modal-back" onClick={() => setMenu(null)}>
              <div className="modal" style={{ maxWidth: 320 }} onClick={(e) => e.stopPropagation()}>
                <h3>مدّة التثبيت</h3>
                <p className="sub">تُعرض الرسالة في شريطٍ أعلى المحادثة حتى تنتهي المدّة.</p>
                {[[1, 'يوم واحد'], [7, 'أسبوع'], [30, 'شهر']].map(([d, label]) => (
                  <button key={d} className="btn btn-ghost btn-block" style={{ marginBottom: 7 }}
                          onClick={() => act(() => api.pin(threadId, menu.m.id, d))}>
                    {label}
                  </button>
                ))}
              </div>
            </div>
          )}
        </>
      )}

      {/* ── تعديل ── */}
      {editing && (
        <div className="modal-back" onClick={() => setEditing(null)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <h3>تعديل الرسالة</h3>
            <p className="sub">
              التعديل متاحٌ خلال ‏<span className="num">15</span>‏ دقيقة من الإرسال، وتظهر كلمة
              «مُعدَّلة» على الرسالة — لا يُخفى أنها عُدّلت.
            </p>
            <div className="field">
              <textarea rows={4} defaultValue={editing.body}
                        onChange={(e) => { editing._new = e.target.value }} />
            </div>
            <div className="modal-acts">
              <button className="btn btn-ghost" onClick={() => setEditing(null)}>إلغاء</button>
              <button className="btn btn-primary" onClick={async () => {
                try {
                  await api.edit(threadId, editing.id, editing._new ?? editing.body)
                  setEditing(null)
                  await load(0)
                } catch (e) { setError(e.message) }
              }}>حفظ</button>
            </div>
          </div>
        </div>
      )}

      {viewer && (
        <div className="viewer-back" onClick={() => setViewer(null)}>
          <img src={viewer} alt="" />
        </div>
      )}
    </div>
  )
}
