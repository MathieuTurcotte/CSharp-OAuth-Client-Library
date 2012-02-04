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
using OAuth.Utils;

namespace OAuth.Base
{
    class HmacSha1Signature : Signature
    {
        private const string METHOD_NAME = "HMAC-SHA1";

        private UrlEncoder encoder = new UrlEncoder();

        private string baseString;
        private ClientCredentials credentials;
        private Token token;

        public HmacSha1Signature(string baseString, ClientCredentials credentials, Token token = null)
        {
            this.baseString = baseString;
            this.credentials = credentials;
            this.token = token;
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
                byte[] key = ConstructHmacSha1Key();
                string signature = HashBaseString(key);
                return encoder.Encode(signature);
            }
        }

        private string HashBaseString(byte[] key)
        {
            HMACSHA1 hasher = new HMACSHA1(key);

            byte[] data = Encoding.ASCII.GetBytes(baseString);
            byte[] hash = hasher.ComputeHash(data);

            return Convert.ToBase64String(hash);
        }

        private byte[] ConstructHmacSha1Key()
        {
            // As per RFC5849, the HMAC-SHA1 key is set to the concatenated values of:
            // 1.  The client shared-secret, after being encoded (Section 3.6).
            // 2.  An "&" character (ASCII code 38), which MUST be included even when 
            //     either secret is empty.
            // 3.  The token shared-secret, after being encoded (Section 3.6).

            string clientSecret = encoder.Encode(credentials.Secret);
            string tokenSecret = encoder.Encode(token == null ? "" : token.Secret);

            string key = string.Format("{0}&{1}", clientSecret, tokenSecret);

            return Encoding.ASCII.GetBytes(key);
        }
    }
}