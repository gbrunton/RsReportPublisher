Option Explicit
Dim wsh
Set wsh=WScript.CreateObject("WScript.Shell")
wscript.Sleep(5000)
wsh.Run "tools\GoogleChromePortable\GoogleChromePortable.exe --app=http://localhost:8100"