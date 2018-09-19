@echo off

ECHO PayPal Core SDK for .NET
ECHO ======================================

SET VSTOOLS=%VS150PROCOMNTOOLS%
IF "%VSTOOLS%"=="" GOTO :VS_NOT_FOUND

SET IDE_DIR=%VSTOOLS%\..\IDE
SET DEVENV="%IDE_DIR%\devenv.com"
SET DOTNET="%PROGRAMFILES%\dotnet\dotnet.exe"
SET SCRIPT_ROOT=%CD%

%DEVENV% PayPal.Core.SDK.NET20.sln /build Release
%DEVENV% PayPal.Core.SDK.NET35.sln /build Release
%DEVENV% PayPal.Core.SDK.NET40.sln /build Release
%DEVENV% PayPal.Core.SDK.NET45.sln /build Release
%DEVENV% PayPal.Core.SDK.NET451.sln /build Release
%DOTNET% build SDK\PayPal.Core.SDK.NETStandard2.0.csproj -c Release

IF NOT EXIST TestResults MKDIR TestResults
DEL TestResults\results_net*.xml

SET MSTEST="%IDE_DIR%\MSTest.exe"
SET TEST_DLL=PayPalCoreSDK.Tests.dll
%MSTEST% /testcontainer:UnitTests\bin\Release\net20\%TEST_DLL% /resultsfile:TestResults\results_net20.xml
%MSTEST% /testcontainer:UnitTests\bin\Release\net35\%TEST_DLL% /resultsfile:TestResults\results_net35.xml
%MSTEST% /testcontainer:UnitTests\bin\Release\net40\%TEST_DLL% /resultsfile:TestResults\results_net40.xml
%MSTEST% /testcontainer:UnitTests\bin\Release\net45\%TEST_DLL% /resultsfile:TestResults\results_net45.xml
%MSTEST% /testcontainer:UnitTests\bin\Release\net451\%TEST_DLL% /resultsfile:TestResults\results_net451.xml
%DOTNET% test UnitTests\PayPal.Core.SDK.NETCore2.1.Tests.csproj --logger "trx;LogFileName=results_netcore21.xml" -r "%SCRIPT_ROOT%\TestResults"
GOTO :END

:VS_NOT_FOUND
ECHO Visual Studio 2017 was not found.

:END
