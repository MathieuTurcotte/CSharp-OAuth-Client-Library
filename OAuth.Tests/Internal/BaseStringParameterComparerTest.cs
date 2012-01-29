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

using System.Collections.Generic;
using NUnit.Framework;
using OAuth.Tests.Helpers;

namespace OAuth.Internal
{
    [TestFixture]
    class SignatureParameterComparerTest
    {
        [Test]
        public void SignatureParameterShouldSortByNameAndValueUsingAscendingByteValueOrdering()
        {
            List<BaseStringParameter> sortedParameters = new List<BaseStringParameter>()
            {
                new BaseStringParameter("a2", "r%20b"),
                new BaseStringParameter("a3", "2%20q"),
                new BaseStringParameter("a3", "a"),
                new BaseStringParameter("b5", "%3D%253D"),
                new BaseStringParameter("c%40", ""),
                new BaseStringParameter("c2", ""),
                new BaseStringParameter("oauth_consumer_key", "9djdj82h48djs9d2"),
                new BaseStringParameter("oauth_nonce", "7d8f3e4a"),
                new BaseStringParameter("oauth_signature_method", "HMAC-SHA1"),
                new BaseStringParameter("oauth_timestamp", "137131201"),
                new BaseStringParameter("oauth_token", "kkk9d7dh3k39sjv7")
	        };

            List<BaseStringParameter> parameters = new List<BaseStringParameter>(sortedParameters);
            parameters.Shuffle();

            parameters.Sort(new BaseStringParameterComparer());

            // Since version 2.2, NUnit has been able to compare two single-dimensioned arrays.
            // Two arrays or collections will be treated as equal by Assert.AreEqual if they 
            // have the same dimensions and if each of the corresponding elements is equal.
            Assert.That(parameters, Is.EqualTo(sortedParameters));
        }
    }
}
