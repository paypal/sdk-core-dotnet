###CHANGE LOG

#### v1.5.0 - September 9, 2014

  * Added future payments support

#### v1.4.3 - July 10, 2014

  * Fix for subject population issue on credentials.

#### v1.4.2 - January 16, 2014

  * Fix for OS exception when used in Microsoft Azure.

#### v1.4.1 - September 27, 2013

  * Support (Optimization) for target .NET Frameworks 2.0, 3.5, 4.0, and 4.5.

#### v1.4.0 - September 26, 2013

  * Updating core to support genio.

#### v1.3.5 - September 11, 2013

  * Adding new mandatory parameters - clientId and clientSecret to openid classes.
  * Added scope for [seamless checkout] (https://developer.paypal.com/webapps/developer/docs/integration/direct/log-in-with-paypal/detailed/#seamlesscheckout) in Session.GetRedirectUrl().

#### v1.3.4 - July 31, 2013

  * Updating version for Reauthorization functionality.

#### v1.3.3 - July 24, 2013

  * Updating version for REST User-Agent header for REST SDK bug fix (https://github.com/paypal/rest-api-sdk-dotnet/issues/7) 

#### v1.3.2 - June 21, 2013

  * Fixing 500 internal service error with OAuth calls - Adding necessary Content-Type header.
  * Exposing HTTP response data in ConnectionException for non-200 responses.(#6)

