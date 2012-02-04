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
using System.Security.Cryptography;

namespace OAuth.Base
{
    internal class Nonce
    {
        private string value;

        public Nonce(int value)
        {
            this.value = Convert.ToString(value);
        }

        public Nonce(String value)
        {
            this.value = value;
        }

        public static Nonce Generate()
        {
            byte[] b = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(b);
            int seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];

            Random random = new Random(seed);
            return new Nonce(random.Next(123400, 9999999));
        }

        public override bool Equals(Object obj)
        {
            Nonce other = obj as Nonce;
            return other != null && value == other.value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return value;
        }
    }
}
