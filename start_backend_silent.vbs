' ============================================================
'  يشغّل خادم Laravel صامتاً عند بدء تشغيل الجهاز.
'
'  ولا يشغّل ثانياً إن كان الأول يعمل: `artisan serve` على منفذٍ
'  مشغول يموت بصمت، فيظنّ المستخدم أن الخادم يعمل وهو ميّت.
'
'  ‏0 في Run تعني نافذةً مخفيّة، و False تعني «لا تنتظر انتهاءه».
' ============================================================
Set sh = CreateObject("WScript.Shell")
Set fso = CreateObject("Scripting.FileSystemObject")

root = fso.GetParentFolderName(WScript.ScriptFullName)

' هل المنفذ 8000 مشغول؟
Set ex = sh.Exec("cmd /c netstat -ano | findstr LISTENING | findstr "":8000 """)
out = ex.StdOut.ReadAll()
If Len(Trim(out)) > 0 Then WScript.Quit 0

php = "C:\xampp\php\php.exe"
If Not fso.FileExists(php) Then php = "php"

sh.CurrentDirectory = root & "\backend"
sh.Run """" & php & """ artisan serve --host=0.0.0.0 --port=8000", 0, False
