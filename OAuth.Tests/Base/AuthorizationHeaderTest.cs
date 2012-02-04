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

using NUnit.Framework;

namespace OAuth.Base
{
    [TestFixture]
    class AuthorizationHeaderTest
    {
        private const string CLIENT_IDENTIFIER = "kgpr346op32etwe";
        private const string CLIENT_SHARED_SECRET = "3nis2ci4qp4zksz";

        private const string ACCESS_TOKEN = "kb0funrnoephp7v";
        private const string ACCESS_TOKEN_SECRET = "4i5cdsnulour8f5";

        private const string SIGNATURE_VALUE = CLIENT_SHARED_SECRET + "&" + ACCESS_TOKEN_SECRET;
        private const string TIMESTAMP_VALUE = "1327336019";
        private const string VERIFIER_CODE = "verifier_code";
        private const string REALM_VALUE = "";
        private const string NONCE_VALUE = "6971488";
        private const string CALLBACK_URL = "http://mathieuturcotte.ca/oauth_back";

        private const string OAUTH_REALM = "OAuth realm=\"" + REALM_VALUE + "\"";
        private const string OAUTH_CONSUMER_KEY = "oauth_consumer_key=\"" + CLIENT_IDENTIFIER + "\"";
        private const string OAUTH_SIGNATURE_METHOD = "oauth_signature_method=\"PLAINTEXT\"";
        private const string OAUTH_NONCE = "oauth_nonce=\"" + NONCE_VALUE + "\"";
        private const string OAUTH_TIMESTAMP = "oauth_timestamp=\"" + TIMESTAMP_VALUE + "\"";
        private const string OAUTH_SIGNATURE = "oauth_signature=\"" + SIGNATURE_VALUE + "\"";
        private const string OAUTH_TOKEN = "oauth_token=\"" + ACCESS_TOKEN + "\"";
        private const string OAUTH_VERIFIER = "oauth_verifier=\"" + VERIFIER_CODE + "\"";
        private const string OAUTH_CALLBACK_URL = "oauth_callback=\"" + CALLBACK_URL + "\"";

        private Nonce nonce;
        private TimeStamp timestamp;
        private Signature signature;
        private AccessToken token;
        private ClientCredentials credentials;

        [SetUp]
        public void Init()
        {
            nonce = new Nonce(int.Parse(NONCE_VALUE));
            timestamp = new TimeStamp(TIMESTAMP_VALUE);
            token = new AccessToken(ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
            credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);
            signature = new PlainTextSignature(credentials, token);
        }

        [Test]
        public void ConvertingAuthorizationHeaderToString()
        {
            AuthorizationHeader header = new AuthorizationHeader(credentials, nonce, timestamp, signature);

            string headerContent = header.ToString();

            Assert.That(headerContent, Is.StringStarting(OAUTH_REALM));
            Assert.That(headerContent, Contains.Substring(OAUTH_CONSUMER_KEY));
            Assert.That(headerContent, Contains.Substring(OAUTH_SIGNATURE_METHOD));
            Assert.That(headerContent, Contains.Substring(OAUTH_NONCE));
            Assert.That(headerContent, Contains.Substring(OAUTH_TIMESTAMP));
            Assert.That(headerContent, Contains.Substring(OAUTH_SIGNATURE));
        }

        [Test]
        public void ConvertingAuthorizationHeaderWithAccessTokenToString()
        {
            AuthorizationHeader header = new AuthorizationHeader(credentials, nonce, timestamp, signature);
            header.Token = token;

            string headerContent = header.ToString();

            Assert.That(headerContent, Is.StringStarting(OAUTH_REALM));
            Assert.That(headerContent, Contains.Substring(OAUTH_CONSUMER_KEY));
            Assert.That(headerContent, Contains.Substring(OAUTH_SIGNATURE_METHOD));
            Assert.That(headerContent, Contains.Substring(OAUTH_NONCE));
            Assert.That(headerContent, Contains.Substring(OAUTH_TIMESTAMP));
            Assert.That(headerContent, Contains.Substring(OAUTH_SIGNATURE));
            Assert.That(headerContent, Contains.Substring(OAUTH_TOKEN));
        }

        [Test]
        public void ConvertingAuthorizationHeaderWithAccessTokenAndVerifierCodeToString()
        {
            AuthorizationHeader header = new AuthorizationHeader(credentials, nonce, timestamp, signature);
            header.VerifierCode = VERIFIER_CODE;
            header.Token = token;

            string headerContent = header.ToString();

            Assert.That(headerContent, Is.StringStarting(OAUTH_REALM));
            Assert.That(headerContent, Contains.Substring(OAUTH_CONSUMER_KEY));
            Assert.That(headerContent, Contains.Substring(OAUTH_SIGNATURE_METHOD));
            Assert.That(headerContent, Contains.Substring(OAUTH_NONCE));
            Assert.That(headerContent, Contains.Substring(OAUTH_TIMESTAMP));
            Assert.That(headerContent, Contains.Substring(OAUTH_SIGNATURE));
            Assert.That(headerContent, Contains.Substring(OAUTH_TOKEN));
            Assert.That(headerContent, Contains.Substring(OAUTH_VERIFIER));
        }

        [Test]
        public void ConvertingAuthorizationHeaderWithAccessTokenVerifierCodeAndCallbackUrlToString()
        {
            AuthorizationHeader header = new AuthorizationHeader(credentials, nonce, timestamp, signature);
            header.VerifierCode = VERIFIER_CODE;
            header.CallbackUrl = CALLBACK_URL;
            header.Token = token;

            string headerContent = header.ToString();

            Assert.That(headerContent, Is.StringStarting(OAUTH_REALM));
            Assert.That(headerContent, Contains.Substring(OAUTH_CONSUMER_KEY));
            Assert.That(headerContent, Contains.Substring(OAUTH_SIGNATURE_METHOD));
            Assert.That(headerContent, Contains.Substring(OAUTH_NONCE));
            Assert.That(headerContent, Contains.Substring(OAUTH_TIMESTAMP));
            Assert.That(headerContent, Contains.Substring(OAUTH_SIGNATURE));
            Assert.That(headerContent, Contains.Substring(OAUTH_TOKEN));
            Assert.That(headerContent, Contains.Substring(OAUTH_VERIFIER));
            Assert.That(headerContent, Contains.Substring(OAUTH_CALLBACK_URL));
        }
    }
}
