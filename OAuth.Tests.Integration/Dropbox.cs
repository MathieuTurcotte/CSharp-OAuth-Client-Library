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
using OAuth.Authenticator;
using OAuth.Base;

namespace OAuth.IntegrationTests
{
    [TestFixture]
    public class Dropbox
    {
        private const string CLIENT_IDENTIFIER = "53i85ykttkrep13";
        private const string CLIENT_SHARED_SECRET = "jns9kry4g4jmefu";

        private const string ACCESS_TOKEN = "uu6h4g1xg3raqnb";
        private const string ACCESS_TOKEN_SECRET = "oeet7ou6mavjyhs";

        private const string REQUEST_TOKEN_END_POINT = "https://api.dropbox.com/1/oauth/request_token";
        private const string AUTHORIZE_END_POINT = "https://www.dropbox.com/1/oauth/authorize";
        private const string ACCESS_TOKEN_END_POINT = "https://api.dropbox.com/1/oauth/access_token";

        private ClientCredentials credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);
        private AccessToken accessToken = new AccessToken(ACCESS_TOKEN, ACCESS_TOKEN_SECRET);

        [Test]
        public void RetrieveRequestToken()
        {
            Uri uri = new Uri(REQUEST_TOKEN_END_POINT);
            NegotiationTokenRequest request = NegotiationTokenRequest.Create(uri, credentials);

            NegotiationToken token = request.GetToken();

            Assert.That(token.Value, Is.Not.Empty);
            Assert.That(token.Secret, Is.Not.Empty);
        }

        [Test]
        public void RetrieveAcccountMetadataUsingHmacSha1Signature()
        {
            RetrieveAccountMetadataUsing(RequestAuthenticatorFactory.GetHmacSha1Authenticator(credentials, accessToken));
        }

        [Test]
        public void RetrieveAcccountMetadataUsingPlainTextSignature()
        {
            RetrieveAccountMetadataUsing(RequestAuthenticatorFactory.GetPlainTextAuthenticator(credentials, accessToken));
        }

        private void RetrieveAccountMetadataUsing(RequestAuthenticator authenticator)
        {
            WebRequest req = WebRequest.Create("https://api.dropbox.com/1/account/info");

            authenticator.SignRequest(req);

            string response = Helpers.ReadResponse(req);

            Assert.That(response, Contains.Substring("uid"));
            Assert.That(response, Contains.Substring("country"));
            Assert.That(response, Contains.Substring("quota_info"));
        }
    }
}
