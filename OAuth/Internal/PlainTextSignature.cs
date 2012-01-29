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

namespace OAuth.Internal
{
    class PlainTextSignature : Signature
    {
        private UrlEncoder encoder = new UrlEncoder();

        private ClientCredentials credentials;
        private Token token;

        public PlainTextSignature(ClientCredentials credentials, Token token = null)
        {
            this.credentials = credentials;
            this.token = token;
        }

        public SignatureType Type
        {
            get
            {
                return SignatureType.PlainText;
            }
        }

        public string Value
        {
            get
            {
                return GenerateSignature(credentials, token);
            }
        }

        private string GenerateSignature(ClientCredentials credentials, Token token)
        {
            return String.Join("&", new string[] {
                ClientSharedSecret(credentials),
                TokenSharedSecret(token)
            });
        }

        private string ClientSharedSecret(ClientCredentials credentials)
        {
            return encoder.Encode(credentials.Secret);
        }

        private string TokenSharedSecret(Token token)
        {
            string secret = token != null ? token.Secret : String.Empty;
            return encoder.Encode(secret);
        }
    }
}
