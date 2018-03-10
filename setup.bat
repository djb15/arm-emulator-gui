cd src\Emulator
.paket\paket.bootstrapper.exe
.paket\paket.exe install
.paket\paket.exe update
cd ..\..
dotnet restore src\Main\Main.fsproj
dotnet restore src\Renderer\Renderer.fsproj