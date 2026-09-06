/**
 * طبقةُ النداءات — كلُّ حديثٍ مع الخادم يمرّ من هنا.
 *
 * ── قرارٌ في الرمز ───────────────────────────────────────────────────────
 *
 * الرمزُ في `localStorage` لا في `sessionStorage`: موظّف الدعم يعمل يوماً
 * كاملاً ويُغلق اللسان ويفتحه، وجلسةٌ تسقط مع كل إغلاقِ لسانٍ تجعله يكتب
 * كلمتَه عشر مرّات في اليوم فيكتبها في ورقةٍ على الشاشة. والمدّة محدودة
 * بالخادم (اثنتا عشرة ساعة) لا بالمتصفّح.
 *
 * وكلُّ ردٍّ 401 يعني «انتهت الجلسة» فوراً: يُمسح الرمز وتُعاد الشاشة إلى
 * الدخول. من غير ذلك يبقى الموظّف داخل الواجهة يضغط أزراراً لا تفعل شيئاً.
 */

const TOKEN_KEY = 'rhalla_support_token'

export function getToken() {
  try {
    return localStorage.getItem(TOKEN_KEY) || ''
  } catch {
    return ''
  }
}

export function setToken(t) {
  try {
    t ? localStorage.setItem(TOKEN_KEY, t) : localStorage.removeItem(TOKEN_KEY)
  } catch {
    /* متصفّحٌ يمنع التخزين — الجلسة تبقى في الذاكرة لهذه الصفحة. */
  }
}

/** يُنادى عند كل 401 — يربطه `App` بإخراج المستخدم. */
let onUnauthorized = () => {}
export function setUnauthorizedHandler(fn) {
  onUnauthorized = fn
}

export const API_BASE = '/api/support'

class ApiError extends Error {
  constructor(message, status) {
    super(message)
    this.status = status
  }
}

async function request(method, path, { body, form, signal } = {}) {
  const headers = {}
  const token = getToken()
  if (token) headers.Authorization = `Bearer ${token}`

  let payload
  if (form) {
    // بلا `Content-Type`: المتصفّح يكتبه بنفسه مع الحدّ الفاصل، وكتابتُه
    // بيدنا تُفسد رفع الملفّات.
    payload = form
  } else if (body !== undefined) {
    headers['Content-Type'] = 'application/json; charset=utf-8'
    payload = JSON.stringify(body)
  }

  let res
  try {
    res = await fetch(API_BASE + path, { method, headers, body: payload, signal })
  } catch (e) {
    if (e.name === 'AbortError') throw e
    throw new ApiError('تعذّر الاتصال بالخادم.', 0)
  }

  if (res.status === 401) {
    setToken('')
    onUnauthorized()
    throw new ApiError('انتهت الجلسة. سجّل الدخول من جديد.', 401)
  }

  let json = null
  try {
    json = await res.json()
  } catch {
    /* ردٌّ بلا JSON — يُعالَج أدناه. */
  }

  if (!res.ok || (json && json.success === false)) {
    throw new ApiError(json?.message || `خطأ ${res.status}`, res.status)
  }

  return json?.data ?? {}
}

export const api = {
  login: (username, password) =>
    request('POST', '/auth/login', { body: { username, password } }),
  me: () => request('GET', '/auth/me'),
  logout: () => request('POST', '/auth/logout'),
  changePassword: (current, next) =>
    request('POST', '/auth/password', { body: { current, new: next } }),

  threads: (f = {}) => {
    const q = new URLSearchParams()
    if (f.scope) q.set('scope', f.scope)
    if (f.status) q.set('status', f.status)
    if (f.q) q.set('q', f.q)
    const s = q.toString()
    return request('GET', '/threads' + (s ? `?${s}` : ''))
  },
  unread: (signal) => request('GET', '/threads/unread', { signal }),
  messages: (id, afterId = 0, signal) =>
    request('GET', `/threads/${id}?after_id=${afterId}`, { signal }),

  send: (id, { body, clientId, replyToId, file }) => {
    // مرفقٌ ⇐ `FormData`، وإلا JSON. والاثنان يحملان `client_id` نفسه:
    // منعُ الازدواج عند إعادة المحاولة لا يخصّ النصّ وحده.
    if (file) {
      const fd = new FormData()
      fd.append('attachment', file)
      if (body) fd.append('body', body)
      if (clientId) fd.append('client_id', clientId)
      if (replyToId) fd.append('reply_to_id', String(replyToId))
      return request('POST', `/threads/${id}/messages`, { form: fd })
    }
    return request('POST', `/threads/${id}/messages`, {
      body: { body, client_id: clientId, reply_to_id: replyToId || 0 },
    })
  },

  typing: (id, state) => request('POST', `/threads/${id}/typing`, { body: { state } }),
  react: (id, mid, emoji) =>
    request('POST', `/threads/${id}/messages/${mid}/react`, { body: { emoji } }),
  edit: (id, mid, body) => request('PUT', `/threads/${id}/messages/${mid}`, { body: { body } }),
  pin: (id, mid, days) => request('POST', `/threads/${id}/messages/${mid}/pin`, { body: { days } }),
  star: (id, mid, on) => request('POST', `/threads/${id}/messages/${mid}/star`, { body: { on } }),
  forward: (id, mid, toThreadId) =>
    request('POST', `/threads/${id}/messages/${mid}/forward`, { body: { to_thread_id: toThreadId } }),

  assign: (id, staffId) => request('POST', `/threads/${id}/assign`, { body: { staff_id: staffId } }),
  setStatus: (id, status, note) =>
    request('POST', `/threads/${id}/status`, { body: { status, note } }),
  assignees: () => request('GET', '/assignees'),
  search: (q) => request('GET', `/search?q=${encodeURIComponent(q)}`),

  staff: () => request('GET', '/staff'),
  createStaff: (name, username, role) =>
    request('POST', '/staff', { body: { name, username, role } }),
  updateStaff: (id, changes) => request('PUT', `/staff/${id}`, { body: changes }),
  deleteStaff: (id) => request('DELETE', `/staff/${id}`),
  resetStaffPassword: (id) => request('POST', `/staff/${id}/password`),
  setPermissions: (id, permissions) =>
    request('PUT', `/staff/${id}/permissions`, { body: { permissions } }),

  audit: (threadId) =>
    request('GET', '/audit' + (threadId ? `?thread_id=${threadId}` : '')),
}

/**
 * رابطُ المرفق.
 *
 * ⚠ لا يُستعمل في `<img src>` مباشرة: المسار يحتاج ترويسة `Authorization`،
 * و`<img>` لا يرسلها. المرفقات تُجلب بـ `fetch` وتُحوَّل إلى `blob:` —
 * انظر `useAttachment`.
 */
export function attachmentUrl(name) {
  return `${API_BASE}/attachment/${encodeURIComponent(name)}`
}

export async function fetchAttachment(name) {
  const res = await fetch(attachmentUrl(name), {
    headers: { Authorization: `Bearer ${getToken()}` },
  })
  if (!res.ok) throw new ApiError('تعذّر جلب المرفق.', res.status)
  return URL.createObjectURL(await res.blob())
}
