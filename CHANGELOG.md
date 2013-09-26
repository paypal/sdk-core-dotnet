###CHANGE LOG

#### V1.4.0 (Sep 26, 2013)

   * Updating core to support genio.

#### V1.3.5 (Sep 11, 2013)

   * Adding new mandatory parameters - clientId and clientSecret to openid classes.
   * Added scope for [seamless checkout] (https://developer.paypal.com/webapps/developer/docs/integration/direct/log-in-with-paypal/detailed/#seamlesscheckout) in Session.GetRedirectUrl().
   
#### V1.3.4 (Jul 31, 2013)

   * Updating version for Reauthorization functionality.
   
#### V1.3.3 (Jul 24, 2013)

   * Updating version for REST User-Agent header for REST SDK bug fix (https://github.com/paypal/rest-api-sdk-dotnet/issues/7) 

#### V1.3.2 (Jun 21, 2013)

   * Fixing 500 internal service error with OAuth calls - Adding necessary Content-Type header.
   * Exposing HTTP response data in ConnectionException for non-200 responses.(#6)

