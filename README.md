C# OAuth Client Library
=======================

A C# library implementing the consumer side of the OAuth 1.0 protocol
([rfc5849](http://tools.ietf.org/html/rfc5849)).

Runs on Microsoft .NET Framework 3.5.

NuGet Install
-------------

[NuGet](https://nuget.org/) users can install this
[library](https://nuget.org/packages/OAuthClient) by running the following
command in their Package Manager Console:

    PM> Install-Package OAuthClient

How to use
----------

First of all, you have to instantiate a `RequestAuthenticator`, passing in your
`ClientCredentials` and the `AccessToken` granted by an OAuth provider.

```csharp
AccessToken accessToken = new AccessToken(ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
ClientCredentials credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);
RequestAuthenticator authenticator = RequestAuthenticatorFactory.GetHmacSha1Authenticator(credentials, accessToken);
```

Now, to consume protected resources, `WebRequest` need to be signed using the
authenticator that you've just created. For example, to access a Dropbox account
informations, one would do:

```csharp
WebRequest req = WebRequest.Create("https://api.dropbox.com/1/account/info");
authenticator.SignRequest(req);
string response = ReadResponse(req);
```

Behind the scene, the authenticator will add an Authorization header to your request.

### Request Authenticators

`RequestAuthenticator` is an interface specifying a method for signing `WebRequest`.

```csharp
public interface RequestAuthenticator
{
    void SignRequest(WebRequest request);
}
```

Authenticators implementing `HMAC-SHA1`, `RSA-SHA1` and `PLAINTEXT` signature
are provided. Instances are created through a factory,
`RequestAuthenticatorFactory`, which expose three static factory methods.

```csharp
RequestAuthenticator GetPlainTextAuthenticator(ClientCredentials credentials, AccessToken token);
RequestAuthenticator GetHmacSha1Authenticator(ClientCredentials credentials, AccessToken token);
RequestAuthenticator GetRsaSha1Authenticator(ClientCredentials credentials, AccessToken token, RSAParameters key);
```

All request authenticators are thread-safe.

Limitations
-----------

* Doesn't support `POST` requests with url encoded body.

Tests
-----

* Unit tests are grouped in the `OAuth.Tests` project.
* Integration tests are grouped in the `OAuth.Tests.Integration` project.
  Integration tests perform requests to several OAuth providers, ensuring
  a minimal level of interoperability.

[NUnit](http://www.nunit.org/) is required to run both test suite.

License
-------

This code is free to use under the terms of the [MIT license](http://mturcotte.mit-license.org/).
