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
    class PlainTextSignatureTest
    {
        private const string CLIENT_IDENTIFIER = "kgpr346op32etwe";
        private const string CLIENT_SHARED_SECRET = "3nis2ci4qp4zksz";

        private const string ACCESS_TOKEN = "kb0funrnoephp7v";
        private const string ACCESS_TOKEN_SECRET = "4i5cdsnulour8f5";

        [Test]
        public void WithoutToken()
        {
            Nonce nonce = new Nonce(4543704);
            TimeStamp timestamp = new TimeStamp("1327332614");
            ClientCredentials credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);

            PlainTextSignature signature = new PlainTextSignature(credentials);

            Assert.AreEqual("PLAINTEXT", signature.Method);
            Assert.AreEqual("3nis2ci4qp4zksz&", signature.Value);
        }

        [Test]
        public void WithToken()
        {
            Nonce nonce = new Nonce(4543704);
            TimeStamp timestamp = new TimeStamp("1327332614");
            ClientCredentials credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);
            AccessToken token = new AccessToken(ACCESS_TOKEN, ACCESS_TOKEN_SECRET); ;

            PlainTextSignature signature = new PlainTextSignature(credentials, token);

            Assert.AreEqual("PLAINTEXT", signature.Method);
            Assert.AreEqual("3nis2ci4qp4zksz&4i5cdsnulour8f5", signature.Value);
        }
    }
}
