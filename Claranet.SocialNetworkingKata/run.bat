@ECHO off

if not exist .\build\Claranet.SocialNetworkingKata.exe (
	echo publishing project
	dotnet publish -o .\build\ -c Release 
)

echo running
.\build\Claranet.SocialNetworkingKata.exe

