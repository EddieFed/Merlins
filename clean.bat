@echo off

echo %cd%
echo "this will delete some files within this directory!  Make sure unity is not running!"
pause

echo "are you sure you would like to do this?"
pause

rd /s /q Library
rd /s /q Logs
rd /s /q obj
rd /s /q Temp
rd /s /q .idea
rd /s /q .vscode
rd /s /q Builds
del /s /q /f *.csproj
del /s /q /f *.pidb
del /s /q /f *.unityproj
del /s /q /f *.DS_Store
del /s /q /f *.sln
del /s /q /f *.userprefs

echo "done."
pause