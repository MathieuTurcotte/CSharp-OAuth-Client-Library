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

namespace OAuth.Utils
{
    [TestFixture]
    class UrlEncoderTest
    {
        private const string UNRESERVED_ALPHA = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
        private const string UNRESERVED_DIGITS = "0123456789";
        private const string UNRESERVED_SYMBOLS = "-_.~";

        private UrlEncoder encoder;

        [SetUp]
        public void CreateNewUrlEncoder()
        {
            encoder = new UrlEncoder();
        }

        [Test]
        public void EncodedCharactersMustBeUppercase()
        {
            Assert.That(encoder.Encode(" "), Is.EqualTo("%20"));
            Assert.That(encoder.Encode(" %"), Is.EqualTo("%20%25"));
        }

        [Test]
        public void UnreservedAlphaCharactersAsPerRfc3986Section23MustNotBeEncoded()
        {
            Assert.That(encoder.Encode(UNRESERVED_ALPHA), Is.EqualTo(UNRESERVED_ALPHA));
        }

        [Test]
        public void UnreservedDigitsAsPerRfc3986Section23MustNotBeEncoded()
        {
            Assert.That(encoder.Encode(UNRESERVED_DIGITS), Is.EqualTo(UNRESERVED_DIGITS));
        }

        [Test]
        public void UnreservedSymbolsAsPerRfc3986Section23MustNotBeEncoded()
        {
            Assert.That(encoder.Encode(UNRESERVED_SYMBOLS), Is.EqualTo(UNRESERVED_SYMBOLS));
        }
    }
}
