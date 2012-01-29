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

namespace OAuth
{
    [TestFixture]
    class AuthorizationUriTest
    {
        private const string AUTHORIZE_END_POINT_WITHOUT_QUERYPARAM = "https://www.dropbox.com/1/oauth/authorize";
        private const string AUTHORIZE_END_POINT_WITH_QUERYPARAM = "https://www.dropbox.com/1/oauth/authorize?foo=bar";

        private const string OAUTH_TOKEN = "OAUTH_TOKEN";
        private const string OAUTH_TOKEN_SECRET = "OAUTH_TOKEN_SECRET";

        private NegotiationToken token = new NegotiationToken(OAUTH_TOKEN, OAUTH_TOKEN_SECRET);

        [Test]
        public void AuthorizationUriBuiltFromUriWithoutQueryParametersShouldContainTheOAuthTokenAsSoleQueryParameter()
        {
            Uri authorizationRequestUri = AuthorizationUri.Create(AUTHORIZE_END_POINT_WITHOUT_QUERYPARAM, token);
            Assert.AreEqual(authorizationRequestUri.Query, "?oauth_token=" + token.Value);
        }

        [Test]
        public void AuthorizationUriBuiltFromUriWithQueryParametersShouldPreserveAllQueryParameters()
        {
            Uri authorizationRequestUri = AuthorizationUri.Create(AUTHORIZE_END_POINT_WITH_QUERYPARAM, token);
            Assert.AreEqual(authorizationRequestUri.Query, "?foo=bar&oauth_token=" + token.Value);
        }
    }
}
