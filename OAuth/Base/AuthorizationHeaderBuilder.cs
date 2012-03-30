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

using System.Text;

namespace OAuth.Base
{
    internal class AuthorizationHeaderBuilder
    {
        private StringBuilder header = new StringBuilder();
        private bool comma = false;

        public AuthorizationHeaderBuilder Append(string value)
        {
            header.Append(value);
            return this;
        }

        public AuthorizationHeaderBuilder AppendField(string name, string value)
        {
            header.Append(comma ? "," : "");
            header.Append(name).Append("=\"").Append(value).Append("\"");
            comma = true;
            return this;
        }

        public AuthorizationHeaderBuilder AppendField(string name)
        {
            return AppendField(name, "");
        }

        public override string ToString()
        {
            return header.ToString();
        }
    }
}
