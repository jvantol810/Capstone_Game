@echo off
rem START or STOP Services
rem ----------------------------------
rem Check if argument is STOP or START

if not ""%1"" == ""START"" goto stop

if exist D:\TestingDatabaseStuff\hypersonic\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\server\hsql-sample-database\scripts\ctl.bat START)
if exist D:\TestingDatabaseStuff\ingres\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\ingres\scripts\ctl.bat START)
if exist D:\TestingDatabaseStuff\mysql\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\mysql\scripts\ctl.bat START)
if exist D:\TestingDatabaseStuff\postgresql\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\postgresql\scripts\ctl.bat START)
if exist D:\TestingDatabaseStuff\apache\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\apache\scripts\ctl.bat START)
if exist D:\TestingDatabaseStuff\openoffice\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\openoffice\scripts\ctl.bat START)
if exist D:\TestingDatabaseStuff\apache-tomcat\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\apache-tomcat\scripts\ctl.bat START)
if exist D:\TestingDatabaseStuff\resin\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\resin\scripts\ctl.bat START)
if exist D:\TestingDatabaseStuff\jetty\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\jetty\scripts\ctl.bat START)
if exist D:\TestingDatabaseStuff\subversion\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\subversion\scripts\ctl.bat START)
rem RUBY_APPLICATION_START
if exist D:\TestingDatabaseStuff\lucene\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\lucene\scripts\ctl.bat START)
if exist D:\TestingDatabaseStuff\third_application\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\third_application\scripts\ctl.bat START)
goto end

:stop
echo "Stopping services ..."
if exist D:\TestingDatabaseStuff\third_application\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\third_application\scripts\ctl.bat STOP)
if exist D:\TestingDatabaseStuff\lucene\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\lucene\scripts\ctl.bat STOP)
rem RUBY_APPLICATION_STOP
if exist D:\TestingDatabaseStuff\subversion\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\subversion\scripts\ctl.bat STOP)
if exist D:\TestingDatabaseStuff\jetty\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\jetty\scripts\ctl.bat STOP)
if exist D:\TestingDatabaseStuff\hypersonic\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\server\hsql-sample-database\scripts\ctl.bat STOP)
if exist D:\TestingDatabaseStuff\resin\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\resin\scripts\ctl.bat STOP)
if exist D:\TestingDatabaseStuff\apache-tomcat\scripts\ctl.bat (start /MIN /B /WAIT D:\TestingDatabaseStuff\apache-tomcat\scripts\ctl.bat STOP)
if exist D:\TestingDatabaseStuff\openoffice\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\openoffice\scripts\ctl.bat STOP)
if exist D:\TestingDatabaseStuff\apache\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\apache\scripts\ctl.bat STOP)
if exist D:\TestingDatabaseStuff\ingres\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\ingres\scripts\ctl.bat STOP)
if exist D:\TestingDatabaseStuff\mysql\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\mysql\scripts\ctl.bat STOP)
if exist D:\TestingDatabaseStuff\postgresql\scripts\ctl.bat (start /MIN /B D:\TestingDatabaseStuff\postgresql\scripts\ctl.bat STOP)

:end

