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
    class BaseStringTest
    {
        [Test]
        public void RequestWithQueryParameters()
        {
            Nonce nonce = new Nonce("1234");
            TimeStamp timestamp = new TimeStamp("1327772674");
            Uri requestUri = new Uri("http://api.netflix.com/catalog/titles?arg1=value1&arg2=&arg3");
            ClientCredentials credentials = new ClientCredentials("key", "secret");

            BaseString baseString = new BaseString(requestUri, "GET", nonce, timestamp, credentials, SignatureType.HmacSha1);

            string expectedBaseString = "GET&http%3A%2F%2Fapi.netflix.com%2Fcatalog%2Ftitles&arg1%3Dvalue1%26arg2%3D%26arg3%3D%26oauth_consumer_key%3Dkey%26oauth_nonce%3D1234%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1327772674%26oauth_version%3D1.0";

            Assert.AreEqual(expectedBaseString, baseString.ToString());
        }

        [Test]
        public void RequestWithToken()
        {
            Nonce nonce = new Nonce("1234");
            TimeStamp timestamp = new TimeStamp("1327772674");
            Uri requestUri = new Uri("http://api.netflix.com/catalog/titles");
            ClientCredentials credentials = new ClientCredentials("key", "secret");
            AccessToken token = new AccessToken("token", "secret");

            BaseString baseString = new BaseString(requestUri, "PUT", nonce, timestamp, credentials, SignatureType.HmacSha1);
            baseString.Token = token;

            string expectedBaseString = "PUT&http%3A%2F%2Fapi.netflix.com%2Fcatalog%2Ftitles&oauth_consumer_key%3Dkey%26oauth_nonce%3D1234%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1327772674%26oauth_token%3Dtoken%26oauth_version%3D1.0";

            Assert.AreEqual(expectedBaseString, baseString.ToString());
        }

        [Test]
        public void RequestWithCallbackUrl()
        {
            Nonce nonce = new Nonce("1234");
            TimeStamp timestamp = new TimeStamp("1327772674");
            Uri requestUri = new Uri("http://api.netflix.com/catalog/titles");
            ClientCredentials credentials = new ClientCredentials("key", "secret");

            BaseString baseString = new BaseString(requestUri, "PUT", nonce, timestamp, credentials, SignatureType.HmacSha1);
            baseString.CallbackUrl = "oob";

            string expectedBaseString = "PUT&http%3A%2F%2Fapi.netflix.com%2Fcatalog%2Ftitles&oauth_callback%3Doob%26oauth_consumer_key%3Dkey%26oauth_nonce%3D1234%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1327772674%26oauth_version%3D1.0";

            Assert.AreEqual(expectedBaseString, baseString.ToString());
        }

        [Test]
        public void RequestWithVerifierCode()
        {
            Nonce nonce = new Nonce("1234");
            TimeStamp timestamp = new TimeStamp("1327772674");
            Uri requestUri = new Uri("http://api.netflix.com/catalog/titles");
            ClientCredentials credentials = new ClientCredentials("key", "secret");

            BaseString baseString = new BaseString(requestUri, "PUT", nonce, timestamp, credentials, SignatureType.HmacSha1);
            baseString.VerifierCode = "verifiercode";

            string expectedBaseString = "PUT&http%3A%2F%2Fapi.netflix.com%2Fcatalog%2Ftitles&oauth_consumer_key%3Dkey%26oauth_nonce%3D1234%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1327772674%26oauth_verifier%3Dverifiercode%26oauth_version%3D1.0";

            Assert.AreEqual(expectedBaseString, baseString.ToString());
        }

        [Test]
        public void RequestOnNonStandardPort()
        {
            Nonce nonce = new Nonce("1234");
            TimeStamp timestamp = new TimeStamp("1327772674");
            Uri requestUri = new Uri("http://api.netflix.com:5050/catalog/titles");
            ClientCredentials credentials = new ClientCredentials("key", "secret");

            BaseString baseString = new BaseString(requestUri, "PUT", nonce, timestamp, credentials, SignatureType.HmacSha1);

            string expectedBaseString = "PUT&http%3A%2F%2Fapi.netflix.com%3A5050%2Fcatalog%2Ftitles&oauth_consumer_key%3Dkey%26oauth_nonce%3D1234%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1327772674%26oauth_version%3D1.0";

            Assert.AreEqual(expectedBaseString, baseString.ToString());
        }
    }
}
