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

### RequestAuthenticators

`RequestAuthenticator` implementing the `HMAC-SHA1`, `RSA-SHA1` and `PLAINTEXT`
signature methods are provided. `RequestAuthenticator` instances are created
through the `RequestAuthenticatorFactory` which expose three factory methods.

    public static RequestAuthenticator GetPlainTextAuthenticator(ClientCredentials credentials, AccessToken token);
    public static RequestAuthenticator GetHmacSha1Authenticator(ClientCredentials credentials, AccessToken token);
    public static RequestAuthenticator GetRsaSha1Authenticator(ClientCredentials credentials, AccessToken token, RSAParameters key);

Authenticators are thread-safe.

Limitations
-----------

* Doesn't support `POST` requests with url encoded body.

Tests
-----

* Unit tests are grouped in the `OAuth.Tests` project.
* Integration tests are grouped in the `OAuth.Tests.Integration` project.
  Integration tests perform requests to differents OAuth provider, ensuring
  a minimal level of interoperability.

[NUnit](http://www.nunit.org/) is required to run both test suite.

License
-------

This code is free to use under the terms of the [MIT license](http://mturcotte.mit-license.org/).
