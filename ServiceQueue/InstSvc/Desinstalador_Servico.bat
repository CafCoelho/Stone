@ECHO OFF
 
echo Un-Installing WindowsService...
echo ---------------------------------------------------
net stop ServiceStoneQueuePersist
InstallUtil /u ServiceStoneQueuePersist.exe
echo ---------------------------------------------------
echo Done.
pause