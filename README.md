C# OAuth Client Library
=======================

A C# 3.5 library implementing the client side part OAuth 1.0 protocol 
([rfc5849](http://tools.ietf.org/html/rfc5849)).

Runs on Microsoft .NET Framework 3.5.

How to use
----------

First of all, you have to instantiate a `RequestAuthenticator`, passing in your 
`ClientCredentials` and the `AccessToken` granted by an OAuth provider.

    AccessToken accessToken = new AccessToken(ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
    ClientCredentials credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);
    RequestAuthenticator authenticator = RequestAuthenticatorFactory.GetHmacSha1Authenticator(credentials, accessToken);
	
Now, to consume protected resources, `WebRequest` need to be signed using the 
authenticator that you've just created. For example, to access a Dropbox account
informations, one would do:

    WebRequest req = WebRequest.Create("https://api.dropbox.com/1/account/info");
    authenticator.SignRequest(req);
    string response = ReadResponse(req);

Behind the scene, the authenticator will add an Authorization header to your request.

Limitations
-----------

* Doesn't support `POST` requests with url encoded body.

Tests
-----

* Unit tests are grouped in the `OAuth.Tests` project. 
* Integration tests are grouped in the `OAuth.Tests.Integration` project. 
  Integration tests perform requests to differents OAuth provider, ensuring 
  a minimal level of interoperability.

[NUnit](http://www.nunit.org/) is required to run the both test suite.
  
License
-------

This code is free to use under the terms of the MIT license.