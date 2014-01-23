# The PayPal Core SDK for .NET

The PayPal Core SDK is a foundational library used by all of PayPal's C# SDKs. This SDK provides functionality such as configuration, credential management, connection management, logging etc that are used by the other SDKs. This SDK is developed using .NET Framework 2.0 and should compile on later versions of the .NET Framework. The SDK is also distributed via [NuGet](http://www.nuget.org/packages/PayPalCoreSDK/).

## Prerequisites

*	Visual Studio 2005 or higher
*	NUnit 2.6.2 for running test cases (only for Visual Studio 2005 Professional Edition users)


## Repository

This repository contains

*	PayPal Core SDK Class Libraries for Visual Studio 2013, 2012, 2010, 2008, and 2005. (C#.NET)
*	Visual Studio Test project for VS 2013, 2012, 2010, 2008, and 2005 (C#.NET)
*	NUnit Test project - only for Visual Studio 2005 Professional Edition users (C#.NET)


## OpenId Connect Integration

   * Redirect your buyer to obtain authorization
   * Capture the authorization code that is available as a query parameter ("code") in the redirect url
   * Exchange the authorization code for an access token, refresh token, id token combo

```csharp	
    Dictionary<string, string> configurationMap = new Dictionary<string, string>();
    configurationMap.Add("clientId", "...");
    configurationMap.Add("clientSecret", "...");
    configurationMap.Add("mode", "live");

    APIContext apiContext = new APIContext();
    apiContext.Config = configurationMap;

    ...
    
    CreateFromAuthorizationCodeParameters codeParams = new CreateFromAuthorizationCodeParameters();
    codeParams.SetCode("code");
    TokenInfo token = TokenInfo.CreateFromAuthorizationCode(apiContext, codeParams);
    string accessToken = token.access_token;
```

   * The access token is valid for a predefined duration and can be used for seamless XO or for retrieving user information

```csharp
    ...

    TokenInfo infoToken = new TokenInfo();
    infoToken.refresh_token = "refreshToken";
    UserInfoParameters infoUserParams = new UserInfoParameters();
    infoUserParams.SetAccessToken(infoToken.access_token);
    UserInfo infoUser = UserInfo.GetUserInfo(apiContext, infoUserParams);
```

   * If the access token has expired, you can obtain a new access token using the refresh token from the 3'rd step

```csharp
    ...
    
    CreateFromRefreshTokenParameters params = new CreateFromRefreshTokenParameters();
    params.SetScope("openid"); // Optional
    TokenInfo info = new TokenInfo(); // Create TokenInfo object; setting the refresh token
    info.refresh_token = "refreshToken";
    
    info.CreateFromRefreshToken(apiContext, params);
```

## Unit Tests

*	Visual Studio Test C#.NET Project 2013, 2012, 2010, 2008, and 2005
*	NUnit Test C#.NET Project only for Visual Studio 2005 Professional Edition as Visual Studio Unit Test feature is not available in Visual Studio 2005 Professional Edition
    - Note: Visual Studio 2005 Professional Edition users can still run the Visual Studio 2005 Unit Test using Visual Studio Agents 2010 or higher

*   Visual Studio Agents 2010 - ISO: http://www.microsoft.com/en-us/download/details.aspx?id=1334
	- Visual Studio Agents 2010 includes Test Controller 2010, Test Agent 2010 and Lab Agent 2010. Test Controller 2010 and Test Agent 2010 collectively enable scale-out load generation, distributed data collection, and distributed test execution. Lab Agent 2010 manages testing, workflow and network isolation for virtual machines used with Visual Studio Lab Management 2010. 

Or

*   Agents for Visual Studio 2012 Update 3: http://www.microsoft.com/en-us/download/details.aspx?id=38186
    - Agents for Visual Studio 2012 is the essential suite of agents and controllers that you can use to build and test applications across the desktop, the server, and the cloud

*	Note: Please copy the directory path of "sdk-cert.p12" file in the Unit Test/NUnit Test Project Resources folder
*	In <Root folder>\sdk-core-dotnet\Source\App.config:
	- apiCertificate="<Root folder>\sdk-core-dotnet\Source\Resources\sdk-cert.p12"
*	And in <Root folder>\sdk-core-dotnet\Source\Constants.cs:
	- public const string CertificatePath = @"<Root folder>\sdk-core-dotnet\Source\Resources\sdk-cert.p12";

## Build Output Path

*	Visual Studio 2013
	- Debug Configuration: In the root of folder of .git: Build\bin\Debug\net451
	- Release Configuration: In the root of folder of .git: Build\bin\Release\net451
	
*	Visual Studio 2012
	- Debug Configuration: In the root of folder of .git: Build\bin\Debug\net45
	- Release Configuration: In the root of folder of .git: Build\bin\Release\net45
	
*	Visual Studio 2010
	- Debug Configuration: In the root of folder of .git: Build\bin\Debug\net40
	- Release Configuration: In the root of folder of .git: Build\bin\Release\net40

*	Visual Studio 2008
	- Debug Configuration: In the root of folder of .git: Build\bin\Debug\net35
	- Release Configuration: In the root of folder of .git: Build\bin\Release\net35

*	Visual Studio 2005
	- Debug Configuration: In the root of folder of .git: Build\bin\Debug\net20
	- Release Configuration: In the root of folder of .git: Build\bin\Release\net20

## License

*	PayPal, Inc. SDK License - LICENSE.txt

