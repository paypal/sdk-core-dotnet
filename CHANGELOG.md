### CHANGE LOG

#### v1.7.0 - December 14, 2015

  * Connections to PayPal should use TLSv1.2
  * Add support for TLS test sandbox endpoints

#### v1.6.3 - September 25, 2015

  * Update IPN endpoint

#### v1.6.2 - April 29, 2015

  * Fix Issue #43 - Calls to check the X509 stores were incorrectly hard-coded to only check the CurrentUser store.

#### v1.6.1 - April 22, 2015

  * Fix compatibility with PayPal .NET SDK
  * Add X509Store support when loading cert if using certificate credentials

#### v1.6.0 - November 11, 2014

  * Re-added .NET 2.0 support
  * Added UserAgent to Classic SDK prehandlers

#### v1.5.2 - October 23, 2014

  * Fixed null exception being thrown for connection timeout errors

#### v1.5.1 - October 9, 2014

  * Fixed PUT and PATCH requests to send payload

#### v1.5.0 - September 9, 2014

  * Added future payments support
  * Dropped .NET 2.0 support

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
