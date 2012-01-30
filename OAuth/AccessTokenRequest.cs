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
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using OAuth.Internal;

namespace OAuth
{
    public class AccessTokenRequest
    {
        private Uri requestUri;
        private ClientCredentials clientCredentials;
        private NegotiationToken negotiationToken;
        private string verifierCode;

        protected AccessTokenRequest(Uri requestUri, ClientCredentials credentials, NegotiationToken negociationToken, string verifierCode)
        {
            this.requestUri = requestUri;
            this.clientCredentials = credentials;
            this.negotiationToken = negociationToken;
            this.verifierCode = verifierCode;
        }

        public static AccessTokenRequest Create(Uri requestUri, ClientCredentials credentials, NegotiationToken negociationToken, string verifierCode)
        {
            return new AccessTokenRequest(requestUri, credentials, negociationToken, verifierCode);
        }

        public static AccessTokenRequest Create(Uri requestUri, ClientCredentials credentials, NegotiationToken negociationToken)
        {
            return new AccessTokenRequest(requestUri, credentials, negociationToken, "");
        }

        public AccessToken GetToken()
        {
            WebRequest request = WebRequest.Create(requestUri);
            request.Method = "POST";

            Nonce nonce = Nonce.Generate();
            TimeStamp timestamp = TimeStamp.Generate();

            BaseString baseString = new BaseString(request.RequestUri,
                request.Method, nonce, timestamp, clientCredentials, HmacSha1Signature.MethodName);
            baseString.Token = negotiationToken;

            Signature signature = new HmacSha1Signature(baseString.ToString(), clientCredentials, negotiationToken);

            AuthorizationHeader header = new AuthorizationHeader(clientCredentials, nonce, timestamp, signature);
            header.Token = negotiationToken;
            header.VerifierCode = verifierCode;

            request.Headers.Add(HttpRequestHeader.Authorization, header.ToString());

            using (WebResponse res = request.GetResponse())
            using (Stream s = res.GetResponseStream())
            using (StreamReader sr = new StreamReader(s))
            {
                NameValueCollection response = HttpUtility.ParseQueryString(sr.ReadToEnd());
                return new AccessToken(response["oauth_token"], response["oauth_token_secret"]);
            }
        }
    }
}
