<!doctype html>
<html lang="ar" dir="rtl">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta name="csrf-token" content="{{ csrf_token() }}">
<title>صندوق وارد الإدارة — الرحالة</title>
<style>
  :root { --p:#1f6f5c; --pd:#175647; --ink:#14201d; --mut:#6b7a76;
          --line:#e3e8e6; --bg:#f4f7f6; --err:#c0392b; }
  * { box-sizing:border-box; }
  html,body { height:100%; }
  body { margin:0; background:var(--bg); color:var(--ink);
         font-family:"Segoe UI",Tahoma,system-ui,sans-serif; }

  .wrap { display:grid; grid-template-columns:320px 1fr; height:100vh; }
  @media (max-width:820px) { .wrap { grid-template-columns:1fr; }
                             .side { display:none; } }

  /* ── القائمة ───────────────────────────────────────────── */
  .side { background:#fff; border-inline-end:1px solid var(--line);
          display:flex; flex-direction:column; min-height:0; }
  .side header { padding:16px 18px; border-bottom:1px solid var(--line); }
  .side h1 { margin:0; font-size:16px; }
  .side .me { margin-top:4px; font-size:12px; color:var(--mut); }
  .list { overflow-y:auto; flex:1; }
  a.row { display:block; padding:13px 18px; border-bottom:1px solid var(--line);
          text-decoration:none; color:inherit; }
  a.row:hover { background:#f8faf9; }
  a.row.on { background:#eef5f3; border-inline-start:3px solid var(--p); }
  .row .top { display:flex; align-items:center; gap:8px; }
  .row .nm { font-size:13.5px; font-weight:600; flex:1;
             white-space:nowrap; overflow:hidden; text-overflow:ellipsis; }
  .badge { background:var(--err); color:#fff; border-radius:99px;
           padding:1px 7px; font-size:11px; font-weight:700; }
  .row .meta { margin-top:3px; font-size:11.5px; color:var(--mut); direction:ltr;
               text-align:right; }

  /* ── المحادثة ──────────────────────────────────────────── */
  .main { display:flex; flex-direction:column; min-height:0; }
  .main header { background:#fff; border-bottom:1px solid var(--line);
                 padding:14px 20px; display:flex; align-items:center; gap:12px; }
  .main header h2 { margin:0; font-size:15px; flex:1; }
  .main header a { font-size:12.5px; color:var(--mut); text-decoration:none; }
  .feed { flex:1; overflow-y:auto; padding:20px; }
  .b { max-width:min(70%,620px); margin-bottom:12px; padding:10px 14px;
       border-radius:14px; font-size:14px; line-height:1.6; white-space:pre-wrap;
       word-wrap:break-word; }
  .b.them { background:#fff; border:1px solid var(--line); margin-inline-end:auto;
            border-bottom-right-radius:4px; }
  .b.me { background:var(--p); color:#fff; margin-inline-start:auto;
          border-bottom-left-radius:4px; }
  .b .who { font-size:11.5px; font-weight:600; color:var(--pd); margin-bottom:3px; }
  .b .at { font-size:10.5px; opacity:.7; margin-top:5px; direction:ltr; text-align:left; }
  .empty { color:var(--mut); font-size:13.5px; text-align:center; margin-top:60px; }

  form.send { background:#fff; border-top:1px solid var(--line); padding:12px 20px;
              display:flex; gap:10px; align-items:flex-end; }
  form.send textarea { flex:1; resize:none; min-height:44px; max-height:140px;
                       padding:11px 14px; border:1px solid var(--line);
                       border-radius:12px; font:inherit; font-size:14px; }
  form.send textarea:focus { outline:2px solid rgba(31,111,92,.25); border-color:var(--p); }
  form.send button { padding:12px 22px; border:0; border-radius:12px; background:var(--p);
                     color:#fff; font:inherit; font-size:14px; font-weight:600;
                     cursor:pointer; }
  form.send button:hover { background:var(--pd); }
</style>
</head>
<body>
<div class="wrap">

  <aside class="side">
    <header>
      <h1>صندوق وارد الإدارة</h1>
      <div class="me">{{ $me }} · <a href="/admin/chat/logout">خروج</a></div>
    </header>
    <div class="list">
      @forelse ($threads as $t)
        <a class="row {{ (int)$t->id === $open ? 'on' : '' }}"
           href="/admin/chat?thread={{ $t->id }}">
          <div class="top">
            <span class="nm">{{ $t->name ?: 'وكيل #'.$t->agent_id }}</span>
            @if (($unread[(int)$t->id] ?? 0) > 0)
              <span class="badge">{{ $unread[(int)$t->id] }}</span>
            @endif
          </div>
          <div class="meta">{{ $t->phone }} · {{ $t->last_message_at ?: '—' }}</div>
        </a>
      @empty
        <div class="empty">لا محادثات بعد.</div>
      @endforelse
    </div>
  </aside>

  <section class="main">
    @if ($open > 0)
      @php $cur = $threads->firstWhere('id', $open); @endphp
      <header>
        <h2>{{ $cur->name ?? ('وكيل #'.($cur->agent_id ?? '')) }}</h2>
        <a href="/admin/chat?thread={{ $open }}">تحديث</a>
      </header>

      <div class="feed" id="feed">
        @forelse ($messages as $m)
          <div class="b {{ $m->sender_kind === 'ADMIN' ? 'me' : 'them' }}"
               data-id="{{ $m->id }}">
            @if ($m->sender_kind !== 'ADMIN' && $m->sender_name)
              <div class="who">{{ $m->sender_name }}</div>
            @endif
            {{ $m->body }}
            <div class="at">{{ $m->created_at }}</div>
          </div>
        @empty
          <div class="empty">لا رسائل في هذه المحادثة.</div>
        @endforelse
      </div>

      <form class="send" method="post" action="/admin/chat/{{ $open }}/send">
        @csrf
        <textarea name="body" maxlength="2000" required
                  placeholder="اكتب ردّك…"></textarea>
        <button type="submit">إرسال</button>
      </form>
    @else
      <div class="empty">اختر محادثة من القائمة.</div>
    @endif
  </section>
</div>

<script>
// نزول إلى آخر المحادثة عند الفتح.
const feed = document.getElementById('feed');
if (feed) feed.scrollTop = feed.scrollHeight;

// استطلاعٌ تزايدي: ما بعد آخر رسالة معروضة، لا المحادثة كلّها.
// بغيره يضطر موظّف الإدارة إلى تحديث الصفحة ليرى ردّ الوكيل.
const threadId = @json($open);
let lastId = (() => {
  const all = feed ? feed.querySelectorAll('.b[data-id]') : [];
  return all.length ? Number(all[all.length - 1].dataset.id) : 0;
})();

async function poll() {
  if (!threadId || !feed) return;
  try {
    const r = await fetch(`/admin/chat/${threadId}/poll?after_id=${lastId}`,
                          { headers: { 'Accept': 'application/json' } });
    if (!r.ok) return;
    const { items } = await r.json();
    for (const m of items) {
      const d = document.createElement('div');
      d.className = 'b ' + (m.sender_kind === 'ADMIN' ? 'me' : 'them');
      d.dataset.id = m.id;
      if (m.sender_kind !== 'ADMIN' && m.sender_name) {
        const w = document.createElement('div');
        w.className = 'who'; w.textContent = m.sender_name; d.appendChild(w);
      }
      d.appendChild(document.createTextNode(m.body));
      const at = document.createElement('div');
      at.className = 'at'; at.textContent = m.created_at; d.appendChild(at);
      feed.appendChild(d);
      lastId = Number(m.id);
    }
    if (items.length) feed.scrollTop = feed.scrollHeight;
  } catch (_) { /* انقطاعٌ لحظي — النبضة التالية تُصلحه. */ }
}
setInterval(poll, 5000);

// Enter يُرسل، وShift+Enter سطرٌ جديد — كما يتوقّع من يكتب طوال اليوم.
const ta = document.querySelector('form.send textarea');
if (ta) ta.addEventListener('keydown', e => {
  if (e.key === 'Enter' && !e.shiftKey) { e.preventDefault(); ta.form.submit(); }
});
</script>
</body>
</html>
