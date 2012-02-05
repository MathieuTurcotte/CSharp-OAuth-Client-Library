using System;
using System.Net;

namespace OAuth.Tests.Helpers
{
    class TestWebRequest : WebRequest
    {
        private Uri requestUri;

        public override string Method { get; set; }
        public override Uri RequestUri { get { return requestUri; } }
        public override WebHeaderCollection Headers { get; set; }

        public TestWebRequest(string uri)
        {
            Method = "GET";
            requestUri = new Uri(uri);
            Headers = new WebHeaderCollection();
        }
    }
}
