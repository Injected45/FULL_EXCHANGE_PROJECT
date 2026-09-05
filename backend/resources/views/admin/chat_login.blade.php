<!doctype html>
<html lang="ar" dir="rtl">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>دخول صندوق الوارد — الرحالة</title>
<style>
  :root { --p:#1f6f5c; --pd:#175647; --ink:#14201d; --line:#e3e8e6; --bg:#f4f7f6; }
  * { box-sizing:border-box; }
  body { margin:0; min-height:100vh; display:grid; place-items:center; background:var(--bg);
         font-family:"Segoe UI",Tahoma,system-ui,sans-serif; color:var(--ink); }
  .card { width:min(420px,92vw); background:#fff; border:1px solid var(--line);
          border-radius:16px; padding:28px; }
  h1 { margin:0 0 6px; font-size:19px; }
  p.sub { margin:0 0 22px; font-size:13px; color:#6b7a76; }
  label { display:block; font-size:12.5px; margin:14px 0 6px; color:#4a5956; }
  input { width:100%; padding:11px 13px; border:1px solid var(--line); border-radius:10px;
          font-size:14px; font-family:inherit; }
  input:focus { outline:2px solid rgba(31,111,92,.25); border-color:var(--p); }
  button { width:100%; margin-top:20px; padding:12px; border:0; border-radius:10px;
           background:var(--p); color:#fff; font-size:14.5px; font-weight:600;
           font-family:inherit; cursor:pointer; }
  button:hover { background:var(--pd); }
  .err { margin-top:16px; padding:11px 13px; border-radius:10px; font-size:13px;
         background:#fdecec; border:1px solid #f5c6c6; color:#a12828; }
</style>
</head>
<body>
  <form class="card" method="post" action="/admin/chat/login">
    @csrf
    <h1>صندوق وارد الإدارة</h1>
    <p class="sub">رسائل الوكلاء ــ للردّ على طلبات المساعدة.</p>

    @if (!$configured)
      <div class="err">
        لم يُضبط مفتاح الدخول على الخادم.<br>
        أضِف <code>CHAT_ADMIN_KEY</code> إلى ملف <code>.env</code> ثم شغّل
        <code>php artisan config:clear</code>.
      </div>
    @else
      <label for="name">اسمك</label>
      <input id="name" name="name" required maxlength="100" placeholder="يظهر مع ردودك">

      <label for="key">مفتاح الدخول</label>
      <input id="key" name="key" type="password" required autocomplete="off">

      <button type="submit">دخول</button>
    @endif

    @if ($error)
      <div class="err">{{ $error }}</div>
    @endif
  </form>
</body>
</html>
