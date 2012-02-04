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
    class SignatureParameterTest
    {
        const string NAME = "name";
        const string VALUE = "value";

        const string ENCODED_NAME = "encoded_name";
        const string ENCODED_VALUE = "encoded_value";

        const string NAME_VALUE_PAIR = NAME + "=" + VALUE;

        [Test]
        public void SignatureParameterWithSameNameAndValueShouldBeEqual()
        {
            BaseStringParameter parameter1 = new BaseStringParameter(NAME, VALUE);
            BaseStringParameter parameter2 = new BaseStringParameter(NAME, VALUE);

            BaseStringParameter parameter3 = new BaseStringParameter(NAME, "abc");
            BaseStringParameter parameter4 = new BaseStringParameter("abc", VALUE);

            Assert.That(parameter1, Is.EqualTo(parameter2));
            Assert.That(parameter2, Is.EqualTo(parameter1));

            Assert.That(parameter1, Is.Not.EqualTo(parameter3));
            Assert.That(parameter1, Is.Not.EqualTo(parameter4));
        }

        [Test]
        public void StringRepresentationShouldBeTheConcatenationOfNameAndValueUsingAnEqualsSign()
        {
            BaseStringParameter parameter = new BaseStringParameter(NAME, VALUE);
            Assert.That(parameter.ToString(), Is.EqualTo(NAME_VALUE_PAIR));
        }
    }
}
