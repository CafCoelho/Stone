@ECHO OFF
 
echo Installing WindowsService...
echo ---------------------------------------------------
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /i C:\InstSvc\ServiceStoneQueuePersist.exe
net start ServiceStoneQueuePersist
echo ---------------------------------------------------
echo Done.
pause