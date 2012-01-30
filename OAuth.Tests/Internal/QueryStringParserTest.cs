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
    class QueryStringParserTest
    {

        QueryStringParser parser;

        [SetUp]
        public void CreateQueryStringParser()
        {
            parser = new QueryStringParser();
        }

        [Test]
        public void EmptyQueryString()
        {
            parser.ParseQueryString("?   ");

            Assert.That(parser.ParsedParameters.Count, Is.EqualTo(0));
        }

        [Test]
        public void NullQueryString()
        {
            parser.ParseQueryString(null);

            Assert.That(parser.ParsedParameters.Count, Is.EqualTo(0));
        }

        [Test]
        public void QueryStringWithEmptyFields()
        {
            parser.ParseQueryString("foo=&bar");

            Assert.That(parser.ParsedParameters["foo"], Is.EqualTo(""));
            Assert.That(parser.ParsedParameters["bar"], Is.EqualTo(""));
        }
    }
}
