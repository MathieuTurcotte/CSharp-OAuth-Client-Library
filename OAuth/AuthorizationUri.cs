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
using OAuth.Internal;

namespace OAuth
{
    public class AuthorizationUri
    {
        public static Uri Create(string authorize, NegotiationToken negotiationToken)
        {
            return Create(new Uri(authorize), negotiationToken);
        }

        public static Uri Create(Uri authorize, NegotiationToken negotiationToken)
        {
            UriBuilder builder = new UriBuilder(authorize);

            if (QueryStringContainsParameters(builder.Query))
            {
                builder.Query = builder.Query.Substring(1) + "&" + OAuthTokenParameter(negotiationToken);
            }
            else
            {
                builder.Query = OAuthTokenParameter(negotiationToken);
            }

            return builder.Uri;
        }

        private static bool QueryStringContainsParameters(string queryString)
        {
            return queryString != null && queryString.Length > 1;
        }

        private static string OAuthTokenParameter(NegotiationToken negotiationToken)
        {
            return AuthorizationHeaderFields.TOKEN + "=" + negotiationToken.Value;
        }
    }
}
