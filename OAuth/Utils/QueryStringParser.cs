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

using System.Collections.Specialized;
using System;

namespace OAuth.Utils
{
    class QueryStringParser
    {
        private NameValueCollection parameters = new NameValueCollection();

        public QueryStringParser ParseQueryString(string queryString)
        {
            string[] queryParameters = SplitQueryParameters(queryString);

            foreach (string queryParameter in SplitQueryParameters(queryString))
            {
                AddQueryParmater(queryParameter);
            }

            return this;
        }

        public NameValueCollection ParsedParameters 
        {
            get
            {
                return parameters;
            }
        }

        private string[] SplitQueryParameters(string queryString)
        {
            return String.IsNullOrEmpty(queryString) ? 
                new string[] { } : queryString.Trim().TrimStart('?').Split('&');
        }

        private void AddQueryParmater(string queryParameter)
        {
            if (!String.IsNullOrEmpty(queryParameter))
            {
                string[] split = queryParameter.Split('=');

                if (split.Length > 1)
                {
                    parameters.Set(split[0], split[1]);
                }
                else
                {
                    parameters.Set(split[0], "");
                }
            }
        }
    }
}
