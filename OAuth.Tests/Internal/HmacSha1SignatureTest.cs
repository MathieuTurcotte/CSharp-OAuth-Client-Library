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
        [Test]
        public void WithoutToken()
        {
            ClientCredentials credentials = new ClientCredentials("key", "secret");
            string baseString = "POST&http%3A%2F%2Fterm.ie%2Foauth%2Fexample%2Frequest_token.php&oauth_consumer_key%3Dkey%26oauth_nonce%3D6971488%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1327336019%26oauth_version%3D1.0";
            
            Signature signature = new HmacSha1Signature(baseString.ToString(), credentials);

            Assert.AreEqual(SignatureType.HmacSha1, signature.Type);
            Assert.AreEqual("Qw2B3uOPRWj%2FgzL3jvdwBbkN6zE%3D", signature.Value);
        }

        [Test]
        public void WithToken()
        {
            ClientCredentials credentials = new ClientCredentials("key", "secret");
            NegotiationToken token = new NegotiationToken("requestkey", "requestsecret");
            string baseString = "POST&http%3A%2F%2Fterm.ie%2Foauth%2Fexample%2Faccess_token.php&oauth_consumer_key%3Dkey%26oauth_nonce%3D6971488%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1327336019%26oauth_token%3Drequestkey%26oauth_version%3D1.0";

            Signature signature = new HmacSha1Signature(baseString, credentials, token);

            Assert.AreEqual(SignatureType.HmacSha1, signature.Type);
            Assert.AreEqual("TtSu4YZhB3uuWPwmCetVARH5f7c%3D", signature.Value);
        }
    }
}
