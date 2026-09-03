#!/usr/bin/env bash
# ============================================================
#  شعار الشركة: من ملفٍ على الجهاز ⇦ معرض المحاكي + شعار الشركة
#
#  الاستعمال:
#      bash set_company_logo.sh [مسار الصورة]
#
#  بلا وسيط، يأخذ أحدث صورة على سطح المكتب أو في التنزيلات.
#
#  يفعل شيئين:
#    1) يدفع الصورة إلى /sdcard/Pictures ويحدّث فهرس الوسائط،
#       فتظهر في «اختيار» داخل شاشة «هوية الشركة».
#    2) يرفعها شعاراً للشركة عبر POST company/branding/logo.
#
#  الثيم لا يُمسّ — اختياره للمالك.
# ============================================================
set -euo pipefail

# Git Bash يحوّل «/sdcard/...» إلى مسار ويندوز فيفشل الدفع.
# MSYS_NO_PATHCONV=1 يوقف هذا التحويل — بدونه: "remote secure_mkdirs failed".
export MSYS_NO_PATHCONV=1

ADB="${ADB:-C:/Users/HP/AppData/Local/Android/Sdk/platform-tools/adb.exe}"
API="${API:-http://127.0.0.1:8000/api}"
BACKEND="${BACKEND:-/d/AI/FULL_EXCHANGE_PROJECT/backend}"
USER_ID="${USER_ID:-104}"

SRC="${1:-}"
if [ -z "$SRC" ]; then
  SRC=$(ls -t /c/Users/HP/Desktop/*.png /c/Users/HP/Desktop/*.jpg \
              /c/Users/HP/Downloads/*.png /c/Users/HP/Downloads/*.jpg 2>/dev/null | head -1 || true)
fi

[ -n "$SRC" ] && [ -f "$SRC" ] || { echo "لم أجد ملف صورة. مرّر المسار: bash set_company_logo.sh <path>"; exit 1; }
echo "الملف: $SRC  ($(stat -c %s "$SRC") بايت)"

# 1) إلى معرض المحاكي
# المصدر يُحوَّل إلى مسار ويندوز يدوياً (adb.exe برنامج ويندوز)، والوجهة
# تبقى كما هي بفضل MSYS_NO_PATHCONV.
SRC_WIN=$(cygpath -w "$SRC")
"$ADB" push "$SRC_WIN" /sdcard/Pictures/ >/dev/null
BASE=$(basename "$SRC")
"$ADB" shell am broadcast -a android.intent.action.MEDIA_SCANNER_SCAN_FILE \
      -d "file:///sdcard/Pictures/$BASE" >/dev/null 2>&1 || true
echo "✔ أُضيفت إلى معرض المحاكي: /sdcard/Pictures/$BASE"

# 2) رفعها شعاراً للشركة
cd "$BACKEND"
TOK=$(php artisan tinker --execute="echo App\Models\User::find($USER_ID)->createToken('logo-upload')->plainTextToken;" 2>&1 | tail -1)
RES=$(curl -s -H "Authorization: Bearer $TOK" -H 'Accept: application/json' \
           -F "logo=@$SRC_WIN" "$API/company/branding/logo")
php artisan tinker --execute="DB::table('personal_access_tokens')->where('name','logo-upload')->delete();" >/dev/null 2>&1

echo "$RES" | php -r '
$d = json_decode(file_get_contents("php://stdin"), true);
$url = $d["data"]["branding"]["logo_url"] ?? null;
echo $url ? "✔ رُفع الشعار: $url\n" : ("✖ فشل الرفع: " . ($d["message"] ?? "رد غير متوقّع") . "\n");
'
