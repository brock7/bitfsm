@echo off
echo Cleaning, a moment please...

del /f /s /q output\*.ilk
del /f /s /q output\*.pdb
del /f /s /q output\*.log
del /f /s /q output\Debug\*.exp
del /f /s /q output\Debug\*.lib
del /f /s /q *.ncb
del /f /s /q *.user
del /f /s /q *.fsm

rd /s /q temp\debug
rd /s /q temp\release
rd /s /q temp
rd /s /q output\debug
rd /s /q output\release
rd /s /q bitfsm_editor\obj

attrib *.suo -s -h -r
del /f /s /q *.suo

echo Cleaning done!
echo. & pause
