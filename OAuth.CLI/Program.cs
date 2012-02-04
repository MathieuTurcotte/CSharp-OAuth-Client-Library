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
using System.IO;
using System.Net;
using OAuth.Base;

namespace OAuth.CLI
{
    class Program
    {
        private const string CLIENT_IDENTIFIER = "w0n8mks7y64f82m";
        private const string CLIENT_SHARED_SECRET = "7t53l3mh0gq122l";

        private const string REQUEST_TOKEN_END_POINT = "https://api.dropbox.com/1/oauth/request_token";
        private const string AUTHORIZE_END_POINT = "https://www.dropbox.com/1/oauth/authorize";
        private const string ACCESS_TOKEN_END_POINT = "https://api.dropbox.com/1/oauth/access_token";

        static void Main(string[] args)
        {
            Uri requestTokenEndPoint = new Uri(REQUEST_TOKEN_END_POINT);
            Uri authorizeEndPoint = new Uri(AUTHORIZE_END_POINT);
            Uri accessTokenEndPoint = new Uri(ACCESS_TOKEN_END_POINT);

            ClientCredentials credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);

            try
            {
                NegotiationTokenRequest negotiationTokenRequest = NegotiationTokenRequest.Create(requestTokenEndPoint, credentials);
                NegotiationToken negotiationToken = negotiationTokenRequest.GetToken();

                Console.WriteLine("Negotiation Token: " + negotiationToken.Value);
                Console.WriteLine("Negotiation Token Secret: " + negotiationToken.Secret);

                Uri authorizationUri = AuthorizationUri.Create(authorizeEndPoint, negotiationToken);

                Console.WriteLine(authorizationUri);
                Console.ReadLine(); // Wait for user authorization.

                AccessTokenRequest accessTokenRequest = AccessTokenRequest.Create(accessTokenEndPoint, credentials, negotiationToken);
                AccessToken accessToken = accessTokenRequest.GetToken();

                Console.WriteLine("Access Token: " + accessToken.Value);
                Console.WriteLine("Access Token Secret: " + accessToken.Secret);
            }
            catch (WebException ex)
            {
                Console.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                Environment.Exit(0);
            }
        }
    }
}