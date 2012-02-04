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
using System.Text;
using System.IO;

namespace OAuth.Internal
{
    class RsaSha1Signature : Signature
    {
        private const string METHOD_NAME = "RSA-SHA1";

        private UrlEncoder encoder = new UrlEncoder();

        private string baseString;
        private RSAParameters key;

        public RsaSha1Signature(string baseString, RSAParameters key)
        {
            this.baseString = baseString;
            this.key = key;
        }

        public string Method
        {
            get
            {
                return METHOD_NAME;
            }
        }

        public static string MethodName
        {
            get
            {
                return METHOD_NAME;
            }
        }

        public string Value
        {
            get
            {
                byte[] hash = HashBaseString();
                byte[] encrypted = EncryptHash(hash);
                return Convert.ToBase64String(encrypted);
            }
        }

        private byte[] EncryptHash(byte[] hash)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(key);
            return rsa.Encrypt(hash, true);
        }

        private byte[] HashBaseString()
        {
            SHA1 hasher = SHA1.Create();
            return hasher.ComputeHash(Encoding.ASCII.GetBytes(baseString));
        }
    }
}