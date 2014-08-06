# The PayPal Core SDK for .NET

The PayPal Core SDK is a foundational library used by all of PayPal's C# SDKs. This SDK provides functionality such as configuration, credential management, connection management, logging, etc. that are used by the other SDKs. This SDK is developed using .NET Framework 3.5 and should compile on later versions of the .NET Framework. The SDK is also distributed via [NuGet](http://www.nuget.org/packages/PayPalCoreSDK/).

## Prerequisites

*	Visual Studio 2008 or higher


## Repository

This repository contains

*	PayPal Core SDK Class Libraries for Visual Studio 2013, 2012, 2010, and 2008. (C# .NET)
*	Visual Studio Test project for VS 2013, 2012, 2010, and 2008 (C# .NET)


## OpenId Connect Integration

   * Redirect your buyer to obtain authorization
   * Capture the authorization code that is available as a query parameter ("code") in the redirect url
   * Exchange the authorization code for an access token, refresh token, id token combo

```csharp	
    var configurationMap = new Dictionary<string, string>();
    configurationMap.Add("clientId", "...");
    configurationMap.Add("clientSecret", "...");
    configurationMap.Add("mode", "live");

    var apiContext = new APIContext();
    apiContext.Config = configurationMap;

    ...
    
    var codeParams = new CreateFromAuthorizationCodeParameters();
    codeParams.SetCode("code");
    var token = TokenInfo.CreateFromAuthorizationCode(apiContext, codeParams);
    string accessToken = token.access_token;
```

   * The access token is valid for a predefined duration and can be used for seamless XO or for retrieving user information

```csharp
    ...

    var tokenInfo = new TokenInfo();
    tokenInfo.refresh_token = "refreshToken";
    var userInfoParams = new UserInfoParameters();
    userInfoParams.SetAccessToken(tokenInfo.access_token);
    var userInfo = UserInfo.GetUserInfo(apiContext, userInfoParams);
```

   * If the access token has expired, you can obtain a new access token using the refresh token from the 3'rd step

```csharp
    ...
    
    var refreshTokenParams = new CreateFromRefreshTokenParameters();
    refreshTokenParams.SetScope("openid"); // Optional
    var tokenInfo = new TokenInfo(); // Create TokenInfo object; setting the refresh token
    tokenInfo.refresh_token = "refreshToken";
    
    tokenInfo.CreateFromRefreshToken(apiContext, refreshTokenParams);
```

## Unit Tests

*	Visual Studio Test C#.NET Project 2013, 2012, 2010, and 2008

*   Visual Studio Agents 2010 - ISO: http://www.microsoft.com/en-us/download/details.aspx?id=1334
	- Visual Studio Agents 2010 includes Test Controller 2010, Test Agent 2010 and Lab Agent 2010. Test Controller 2010 and Test Agent 2010 collectively enable scale-out load generation, distributed data collection, and distributed test execution. Lab Agent 2010 manages testing, workflow and network isolation for virtual machines used with Visual Studio Lab Management 2010. 

Or

*   Agents for Visual Studio 2012 Update 3: http://www.microsoft.com/en-us/download/details.aspx?id=38186
    - Agents for Visual Studio 2012 is the essential suite of agents and controllers that you can use to build and test applications across the desktop, the server, and the cloud

## Build Output Path

| Visual Studio | Output Path                       |
| ------------- | --------------------------------- |
| 2013          | Build\bin\\[Configuration]\net451 |
| 2012          | Build\bin\\[Configuration]\net45  |
| 2010          | Build\bin\\[Configuration]\net40  |
| 2008          | Build\bin\\[Configuration]\net35  |

## License

*	PayPal, Inc. SDK License - LICENSE.txt

