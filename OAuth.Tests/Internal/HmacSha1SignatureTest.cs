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
using NUnit.Framework;

namespace OAuth.Internal
{
    [TestFixture]
    class HmacSha1SignatureTest
    {
        private const string CLIENT_IDENTIFIER = "kgpr346op32etwe";
        private const string CLIENT_SHARED_SECRET = "3nis2ci4qp4zksz";

        private const string ACCESS_TOKEN = "kb0funrnoephp7v";
        private const string ACCESS_TOKEN_SECRET = "4i5cdsnulour8f5";

        [Test]
        public void PostRequest()
        {
            Nonce nonce = new Nonce(6971488);
            TimeStamp timestamp = new TimeStamp("1327336019");
            Uri requestUri = new Uri("http://term.ie/oauth/example/request_token.php");
            ClientCredentials credentials = new ClientCredentials("key", "secret");

            BaseString baseString = new BaseString(requestUri, "POST", nonce, timestamp, credentials, SignatureType.HmacSha1);
            Signature signature = new HmacSha1Signature(baseString.ToString(), credentials);

            Assert.AreEqual(SignatureType.HmacSha1, signature.Type);
            Assert.AreEqual("Qw2B3uOPRWj%2FgzL3jvdwBbkN6zE%3D", signature.Value);
        }

        [Test]
        public void PostRequestWithToken()
        {
            Nonce nonce = new Nonce(6971488);
            TimeStamp timestamp = new TimeStamp("1327336019");
            Uri requestUri = new Uri("http://term.ie/oauth/example/access_token.php");
            ClientCredentials credentials = new ClientCredentials("key", "secret");
            NegotiationToken token = new NegotiationToken("requestkey", "requestsecret");

            BaseString baseString = new BaseString(requestUri, "POST", nonce, timestamp, credentials, SignatureType.HmacSha1);
            baseString.Token = token;

            Signature signature = new HmacSha1Signature(baseString.ToString(), credentials, token);

            Assert.AreEqual(SignatureType.HmacSha1, signature.Type);
            Assert.AreEqual("TtSu4YZhB3uuWPwmCetVARH5f7c%3D", signature.Value);
        }

        [Test]
        public void GetRequestWithToken()
        {
            Nonce nonce = new Nonce(4543704);
            TimeStamp timestamp = new TimeStamp("1327332614");
            Uri requestUri = new Uri("https://api.dropbox.com/1/metadata/dropbox/");
            ClientCredentials credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);
            AccessToken token = new AccessToken(ACCESS_TOKEN, ACCESS_TOKEN_SECRET);

            BaseString baseString = new BaseString(requestUri, "GET", nonce, timestamp, credentials, SignatureType.HmacSha1);
            baseString.Token = token;

            Signature signature = new HmacSha1Signature(baseString.ToString(), credentials, token);

            Assert.AreEqual(SignatureType.HmacSha1, signature.Type);
            Assert.AreEqual("%2FeYb4E0zH%2Bdpknfst1Bo47kJj2E%3D", signature.Value);
        }

        [Test]
        public void GetRequestWithQueryParameters()
        {
            Nonce nonce = new Nonce(1327553465);
            TimeStamp timestamp = new TimeStamp("1327553564");
            ClientCredentials credentials = new ClientCredentials("abcd", "efgh");
            Uri requestUri = new Uri("http://host.net/resource?name1=val-1&name2=val 2");

            BaseString baseString = new BaseString(requestUri, "GET", nonce, timestamp, credentials, SignatureType.HmacSha1);
            Signature signature = new HmacSha1Signature(baseString.ToString(), credentials);

            Assert.AreEqual(SignatureType.HmacSha1, signature.Type);
            Assert.AreEqual("ubxHzcd2tu9IVPSXeOvVaZrBUkI%3D", signature.Value);
        }

        [Test]
        public void GetRequestToTwitterTimeline()
        {
            Nonce nonce = new Nonce("140fe91261e58ec809cf15fda4f688bc");
            TimeStamp timestamp = new TimeStamp("1327615782");
            Uri requestUri = new Uri("https://api.twitter.com/1/statuses/home_timeline.json?include_entities=true");
            ClientCredentials credentials = new ClientCredentials("oRKaZOrjadgyFRQjLwucIQ", "hqmosjLp4tn83xb5XVBFfOG3nkPFmJGI4aVDBeHoJHY");
            AccessToken token = new AccessToken("30460897-EnT52zG9mNCbdeAujfW5QhAlc1NnZXaGGOLymugqP", "Rk695IboW3wekBgrFCe6iOBCpd0qaXQbwnqYcHcopw0");

            BaseString baseString = new BaseString(requestUri, "GET", nonce, timestamp, credentials, SignatureType.HmacSha1);
            baseString.Token = token;

            Signature signature = new HmacSha1Signature(baseString.ToString(), credentials, token);

            Assert.AreEqual(SignatureType.HmacSha1, signature.Type);
            Assert.AreEqual("jxyS2MD2I1ReIanOHi1uxGG%2FTZM%3D", signature.Value);
        }
    }
}
