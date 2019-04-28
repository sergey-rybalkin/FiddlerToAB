set configuration=%1
if "%configuration%" == "" @set configuration=Release

msbuild.exe /t:Rebuild /p:Configuration=%configuration%
copy src\bin\%configuration%\FiddlerToAB.dll %LOCALAPPDATA%\Programs\Fiddler\ImportExport
copy src\bin\%configuration%\FiddlerToAB.pdb %LOCALAPPDATA%\Programs\Fiddler\ImportExport