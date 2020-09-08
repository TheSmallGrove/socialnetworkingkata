@ECHO off

if not exist .\build\Elite.SocialNetworkingKata.exe (
	echo publishing project
	dotnet publish -o .\build\ -c Release 
)

echo running
.\build\Elite.SocialNetworkingKata.exe

