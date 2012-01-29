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

using System.Net;
using NUnit.Framework;

namespace OAuth.Authenticator
{
    [TestFixture]
    class RequestAuthenticatorTest
    {
        private const string CLIENT_IDENTIFIER = "kgpr346op32etwe";
        private const string CLIENT_SHARED_SECRET = "3nis2ci4qp4zksz";

        private const string ACCESS_TOKEN = "kb0funrnoephp7v";
        private const string ACCESS_TOKEN_SECRET = "4i5cdsnulour8f5";

        private ClientCredentials credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);
        private AccessToken accessToken = new AccessToken(ACCESS_TOKEN, ACCESS_TOKEN_SECRET);

        private WebRequest request;

        [SetUp]
        public void CreateWebRequest()
        {
            request = WebRequest.Create("http://foo.com/");
        }

        [Test]
        public void HmacSha1RequestAuthenticatorShouldAddAnAuthorizationHeader()
        {
            OAuthRequestAuthenticator authenticator = new HmacSha1RequestAuthenticator(credentials, accessToken);

            authenticator.SignRequest(request);

            string header = request.Headers.Get("Authorization");

            Assert.That(header, Is.StringStarting("OAuth"));
            Assert.That(header, Contains.Substring("HMAC-SHA1"));
        }

        [Test]
        public void PlainTextRequestAuthenticatorShouldAddAnAuthorizationHeader()
        {
            OAuthRequestAuthenticator authenticator = new PlainTextRequestAuthenticator(credentials, accessToken);

            authenticator.SignRequest(request);

            string header = request.Headers.Get("Authorization");

            Assert.That(header, Is.StringStarting("OAuth"));
            Assert.That(header, Contains.Substring("PLAINTEXT"));
        }
    }
}
