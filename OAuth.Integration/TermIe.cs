#region Copyright
// Copyright (C) 2012 Mathieu Turcotte
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Net;
using NUnit.Framework;

namespace OAuth.IntegrationTests
{
    [TestFixture]
    class TermIe
    {
        private const string CLIENT_IDENTIFIER = "key";
        private const string CLIENT_SHARED_SECRET = "secret";

        private const string NEGOTIATION_TOKEN = "requestkey";
        private const string NEGOTIATION_TOKEN_SECRET = "requestsecret";

        private const string ACCESS_TOKEN = "accesskey";
        private const string ACCESS_TOKEN_SECRET = "accesssecret";

        private ClientCredentials credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);

        private AccessToken accessToken = new AccessToken(ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
        private NegotiationToken negotiationToken = new NegotiationToken(NEGOTIATION_TOKEN, NEGOTIATION_TOKEN_SECRET);

        [Test]
        public void RetrieveRequestToken()
        {
            Uri uri = new Uri("http://term.ie/oauth/example/request_token.php");
            NegotiationTokenRequest request = NegotiationTokenRequest.Create(uri, credentials);

            NegotiationToken token = request.GetToken();

            Assert.That(token.Value, Is.Not.Empty);
            Assert.That(token.Secret, Is.Not.Empty);
        }

        [Test]
        public void RetrieveAccessToken()
        {
            Uri uri = new Uri("http://term.ie/oauth/example/access_token.php");
            AccessTokenRequest request = AccessTokenRequest.Create(uri, credentials, negotiationToken);

            AccessToken token = request.GetToken();

            Assert.That(token.Value, Is.Not.Empty);
            Assert.That(token.Secret, Is.Not.Empty);
        }

        [Test]
        public void HmacSha1AuthenticatedRequestParametersShouldBeEchoed()
        {
            RequestAuthenticator authenticator = RequestAuthenticatorFactory.GetHmacSha1Authenticator(credentials, accessToken);

            WebRequest req = WebRequest.Create("http://term.ie/oauth/example/echo_api.php?method=foo&bar=baz");

            authenticator.SignRequest(req);

            string response = Helpers.ReadResponse(req);

            Assert.That(response, Contains.Substring("method=foo&bar=baz"));
        }

        [Test]
        public void PlainTextAuthenticatedRequestParametersShouldBeEchoed()
        {
            RequestAuthenticator authenticator = RequestAuthenticatorFactory.GetPlainTextAuthenticator(credentials, accessToken);

            WebRequest req = WebRequest.Create("http://term.ie/oauth/example/echo_api.php?method=foo&bar=baz");

            authenticator.SignRequest(req);

            string response = Helpers.ReadResponse(req);

            Assert.That(response, Contains.Substring("method=foo&bar=baz"));
        }
    }
}
