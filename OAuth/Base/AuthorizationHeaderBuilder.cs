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
using System.Text;
using OAuth.Utils;

namespace OAuth.Base
{
    internal class AuthorizationHeaderBuilder
    {
        StringBuilder header = new StringBuilder();

        public AuthorizationHeaderBuilder Append(string value)
        {
            header.Append(value);
            return this;
        }

        public AuthorizationHeaderBuilder AppendField(string name, string value)
        {
            header.Append(name).Append("=\"").Append(value).Append("\"");
            return this;
        }

        public AuthorizationHeaderBuilder AppendField(string name)
        {
            return AppendField(name, "");
        }

        public AuthorizationHeaderBuilder AppendComma()
        {
            header.Append(',');
            return this;
        }

        public override string ToString()
        {
            return header.ToString();
        }
    }
}
