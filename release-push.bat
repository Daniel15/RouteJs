@echo off
IF EXIST "%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" (
	"%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" build.proj /t:Push /p:BuildType=Release
) ELSE (
	"%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe" build.proj /t:Push /p:BuildType=Release
)
pause
