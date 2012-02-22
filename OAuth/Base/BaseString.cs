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
using System.Collections.Generic;
using System.Web;
using System.Collections.Specialized;
using OAuth.Utils;

namespace OAuth.Base
{
    internal class BaseString
    {
        private readonly Uri uri;
        private string httpMethod;
        private readonly Nonce nonce;
        private readonly TimeStamp timestamp;
        private readonly ClientCredentials credentials;
        private readonly string signatureType;

        private Token token;
        private string callback;
        private string verifier;

        private UrlEncoder encoder = new UrlEncoder();

        public BaseString(Uri uri, string httpMethod, Nonce nonce, TimeStamp timestamp, ClientCredentials credentials, string type)
        {
            this.uri = uri;
            this.httpMethod = httpMethod;
            this.nonce = nonce;
            this.timestamp = timestamp;
            this.credentials = credentials;
            this.signatureType = type;
        }

        public Token Token { set { token = value; } }
        public string CallbackUrl { set { callback = value; } }
        public string VerifierCode { set { verifier = value; } }

        public override string ToString()
        {
            // As per RFC5849, base string is constrcuted from:
            // 1. The HTTP request method (e.g., "GET", "POST", etc.).
            // 2. The authority as declared by the HTTP "Host" request header field.
            // 3. The path and query components of the request resource URI.
            // 4. The protocol parameters excluding the "oauth_signature".
            // 5. Parameters included in the request entity-body if they comply 
            //    with restrictions defined in Section 3.4.1.3.

            return String.Join("&", new string[] {
                httpMethod.ToUpper(), 
                GetBaseStringUri(),
                GetParametersString()
            });
        }

        private string GetBaseStringUri()
        {
            // As per RFC5849, base string uri must conform to:
            // 1. The scheme and host MUST be in lowercase.
            // 2. The host and port values MUST match the content of 
            //    the HTTP request "Host" header field.
            // 3. The port MUST be included if it is not the default port 
            //    for the scheme, and MUST be excluded if it is the default.  
            //    Specifically, the port MUST be excluded when making an HTTP 
            //    request to port 80 or when making an HTTPS request to port 443.

            string url = String.Format("{0}://{1}", uri.Scheme, uri.Host);

            if (!IsRequestOnDefaultPort())
            {
                url += ":" + uri.Port;
            }

            return encoder.Encode(url + uri.AbsolutePath);
        }

        private bool IsRequestOnDefaultPort()
        {
            return (uri.Scheme == "http" && uri.Port == 80) ||
                   (uri.Scheme == "https" && uri.Port == 443);
        }

        private string GetParametersString()
        {
            // As per RFC5849, signature parameters must be sorted 
            // in their canonical form, e.i. they must not be encoded.

            List<BaseStringParameter> parameters = GetBaseStringParameters();
            parameters.Sort(new BaseStringParameterComparer());
            string[] values = Array.ConvertAll(parameters.ToArray(), i => i.ToString());
            return encoder.Encode(String.Join("&", values));
        }

        private List<BaseStringParameter> GetBaseStringParameters()
        {
            // As per RFC5849, parameters source are:
            // 1. The query component of the HTTP request URI as defined by [RFC3986], Section 3.4.
            // 2. The OAuth HTTP "Authorization" header field (Section 3.5.1) if present,
            //    excluding the "realm" parameter if present.
            // 3. Under some conditions, the HTTP request entity-body.

            List<BaseStringParameter> parameters = new List<BaseStringParameter>();

            parameters.Add(new BaseStringParameter(AuthorizationHeaderFields.VERSION, OAuthVersion.VERSION));
            parameters.Add(new BaseStringParameter(AuthorizationHeaderFields.NONCE, nonce.ToString()));
            parameters.Add(new BaseStringParameter(AuthorizationHeaderFields.TIMESTAMP, timestamp.ToString()));
            parameters.Add(new BaseStringParameter(AuthorizationHeaderFields.CONSUMER_KEY, credentials.Identifier));

            if (!String.IsNullOrEmpty(callback))
            {
                parameters.Add(new BaseStringParameter(AuthorizationHeaderFields.CALLBACK, callback));
            }

            if (!String.IsNullOrEmpty(verifier))
            {
                parameters.Add(new BaseStringParameter(AuthorizationHeaderFields.VERIFIER, verifier));
            }

            if (token != null)
            {
                parameters.Add(new BaseStringParameter(AuthorizationHeaderFields.TOKEN, token.Value));
            }

            parameters.AddRange(BaseStringParametersFromQueryString());
            parameters.Add(SignatureMethodParameter());

            return parameters;
        }

        private BaseStringParameter SignatureMethodParameter()
        {
            return new BaseStringParameter(AuthorizationHeaderFields.SIGNATURE_METHOD, signatureType);
        }

        private List<BaseStringParameter> BaseStringParametersFromQueryString()
        {
            QueryStringParser parser = new QueryStringParser().ParseQueryString(uri.Query);
            return ConvertQueryStringParametersToBaseStringParameters(parser.ParsedParameters);
        }

        private List<BaseStringParameter> ConvertQueryStringParametersToBaseStringParameters(NameValueCollection queryParameters)
        {
            List<BaseStringParameter> parameters = new List<BaseStringParameter>();

            foreach (string parameterName in queryParameters.AllKeys)
            {
                if (IsUsableQueryParameter(parameterName))
                {
                    string parameterValue = queryParameters[parameterName];
                    parameters.Add(new BaseStringParameter(parameterName, parameterValue));
                }
            }

            return parameters;
        }

        private bool IsUsableQueryParameter(string parameterName)
        {
            return !(String.IsNullOrEmpty(parameterName) || parameterName.StartsWith(AuthorizationHeaderFields.PREFIX));
        }
    }
}