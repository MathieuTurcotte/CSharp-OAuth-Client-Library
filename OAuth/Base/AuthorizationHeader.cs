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

namespace OAuth.Base
{
    internal class AuthorizationHeader
    {
        private readonly ClientCredentials credentials;
        private readonly string version = "1.0";
        private readonly TimeStamp timestamp;
        private readonly Nonce nonce;
        private readonly Signature signature;
        private Token token;
        private string callback;
        private string verifier;

        public AuthorizationHeader(ClientCredentials credentials, Nonce nonce, TimeStamp timestamp, Signature signature)
        {
            this.credentials = credentials;
            this.timestamp = timestamp;
            this.nonce = nonce;
            this.signature = signature;
        }

        public Token Token { set { token = value; } }
        public string CallbackUrl { set { callback = value; } }
        public string VerifierCode { set { verifier = value; } }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("OAuth ");
            builder.Append(GetHeaderField(AuthorizationHeaderFields.REALM, "")).Append(',');
            builder.Append(GetHeaderField(AuthorizationHeaderFields.VERSION, version)).Append(',');
            builder.Append(GetHeaderField(AuthorizationHeaderFields.CONSUMER_KEY, credentials.Identifier)).Append(',');
            builder.Append(GetHeaderField(AuthorizationHeaderFields.SIGNATURE_METHOD, signature.Method)).Append(',');

            if (token != null)
            {
                builder.Append(GetHeaderField(AuthorizationHeaderFields.TOKEN, token.Value)).Append(',');
            }

            if (!String.IsNullOrEmpty(verifier))
            {
                builder.Append(GetHeaderField(AuthorizationHeaderFields.VERIFIER, verifier)).Append(',');
            }

            if (!String.IsNullOrEmpty(callback))
            {
                builder.Append(GetHeaderField(AuthorizationHeaderFields.CALLBACK, callback)).Append(',');
            }

            builder.Append(GetHeaderField(AuthorizationHeaderFields.NONCE, nonce.ToString())).Append(',');
            builder.Append(GetHeaderField(AuthorizationHeaderFields.TIMESTAMP, timestamp.ToString())).Append(',');
            builder.Append(GetHeaderField(AuthorizationHeaderFields.SIGNATURE, signature.Value));

            return builder.ToString();
        }

        private string GetHeaderField(string name, string value)
        {
            return name + "=\"" + value + "\"";
        }
    }
}
