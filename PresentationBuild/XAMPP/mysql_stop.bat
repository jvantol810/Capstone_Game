@echo off
cd /D %~dp0
echo Mysql shutdowm ...
cmd.exe /C start "" /MIN call "D:\TestingDatabaseStuff\killprocess.bat" "mysqld.exe"

if not exist mysql\data\%computername%.pid GOTO exit
echo Delete %computername%.pid ...
del mysql\data\%computername%.pid

:exit
