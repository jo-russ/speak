@echo off
for /f "tokens=*" %%a in (%~nx1) do call:line "%%a"
pause
goto:eof

:line
echo %1
Speak.exe --l EN %1 >nul
goto:eof