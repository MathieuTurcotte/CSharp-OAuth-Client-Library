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

using System.Net;
using OAuth.Internal;

namespace OAuth.Authenticator
{
    internal abstract class OAuthRequestAuthenticator : RequestAuthenticator
    {
        protected ClientCredentials credentials;
        protected AccessToken token;

        public OAuthRequestAuthenticator(ClientCredentials credentials, AccessToken token)
        {
            this.credentials = credentials;
            this.token = token;
        }

        public void SignRequest(WebRequest request)
        {
            Nonce nonce = Nonce.Generate();
            TimeStamp timestamp = TimeStamp.Generate();
            Signature signature = GenerateSignature(request, nonce, timestamp);
            AuthorizationHeader header = GenerateAuthorizationHeader(nonce, timestamp, signature);
            AddAuthorizationHeaderToRequest(request, header);
        }

        private AuthorizationHeader GenerateAuthorizationHeader(Nonce nonce, TimeStamp timestamp, Signature signature)
        {
            AuthorizationHeader header = new AuthorizationHeader(credentials, nonce, timestamp, signature);
            header.Token = token;
            return header;
        }

        protected abstract Signature GenerateSignature(WebRequest request, Nonce nonce, TimeStamp timestamp);

        private void AddAuthorizationHeaderToRequest(WebRequest request, AuthorizationHeader header)
        {
            request.Headers.Add(HttpRequestHeader.Authorization, header.ToString());
        }
    }
}
