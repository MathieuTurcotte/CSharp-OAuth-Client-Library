﻿#region Copyright
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
using System.Net;
using System.Security.Cryptography;
using NUnit.Framework;
using OAuth.Authenticator;
using OAuth.Base;

namespace OAuth.IntegrationTests
{
    [TestFixture]
    class TermIe
    {
        private const string CLIENT_IDENTIFIER = "key";
        private const string CLIENT_SHARED_SECRET = "secret";

        private const string NEGOTIATION_TOKEN = "requestkey";
        private const string NEGOTIATION_TOKEN_SECRET = "requestsecret";

        private const string ACCESS_TOKEN = "accesskey";
        private const string ACCESS_TOKEN_SECRET = "accesssecret";

        private ClientCredentials credentials = new ClientCredentials(CLIENT_IDENTIFIER, CLIENT_SHARED_SECRET);

        private AccessToken accessToken = new AccessToken(ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
        private NegotiationToken negotiationToken = new NegotiationToken(NEGOTIATION_TOKEN, NEGOTIATION_TOKEN_SECRET);

        [Test]
        public void RetrieveRequestToken()
        {
            Uri uri = new Uri("http://term.ie/oauth/example/request_token.php");
            NegotiationTokenRequest request = NegotiationTokenRequest.Create(uri, credentials);

            NegotiationToken token = request.GetToken();

            Assert.That(token.Value, Is.Not.Empty);
            Assert.That(token.Secret, Is.Not.Empty);
        }

        [Test]
        public void RetrieveAccessToken()
        {
            Uri uri = new Uri("http://term.ie/oauth/example/access_token.php");
            AccessTokenRequest request = AccessTokenRequest.Create(uri, credentials, negotiationToken);

            AccessToken token = request.GetToken();

            Assert.That(token.Value, Is.Not.Empty);
            Assert.That(token.Secret, Is.Not.Empty);
        }

        [Test]
        public void HmacSha1AuthenticatedRequestParametersShouldBeEchoed()
        {
            RequestAuthenticator authenticator = RequestAuthenticatorFactory.GetHmacSha1Authenticator(credentials, accessToken);

            WebRequest req = WebRequest.Create("http://term.ie/oauth/example/echo_api.php?method=foo&bar=baz");

            authenticator.SignRequest(req);

            string response = Helpers.ReadResponse(req);

            Assert.That(response, Contains.Substring("method=foo&bar=baz"));
        }

        [Test]
        public void RsaSha1AuthenticatedRequestParametersShouldBeEchoed()
        {
            RSAParameters parameters = new RSAParameters();

            // PKCS #1 (A.1.2) Modulus.
            parameters.Modulus = new byte[] {
                0xb4, 0x62, 0x30, 0xb0, 0x21, 0xf6, 0x28, 0xa6, 0xba, 0xbf, 0x15, 0x03, 0xba, 0x95, 0xbd, 
                0xda, 0xb1, 0x7a, 0xf1, 0x2e, 0x52, 0x45, 0xb8, 0x2b, 0xed, 0x8a, 0x74, 0xc5, 0xe6, 0x9d, 
                0x06, 0xc6, 0xf4, 0x06, 0xbb, 0x93, 0xb7, 0x81, 0x8c, 0xad, 0x52, 0xfb, 0x42, 0xd8, 0x99, 
                0x58, 0xcf, 0x2a, 0x52, 0x46, 0x35, 0x71, 0xc3, 0x1a, 0xec, 0xb9, 0x17, 0x0f, 0xdd, 0xee, 
                0xb8, 0xd5, 0x27, 0x40, 0x4b, 0x07, 0xeb, 0x9b, 0x3c, 0xaf, 0x22, 0x03, 0xf4, 0xf0, 0xde, 
                0x12, 0xd0, 0x81, 0x73, 0x11, 0x44, 0x64, 0x57, 0x5c, 0x29, 0xfc, 0x8a, 0x47, 0xee, 0x41, 
                0xf8, 0xd4, 0x4b, 0x5b, 0x99, 0x49, 0xab, 0x5d, 0x2c, 0x1f, 0x35, 0x9b, 0x27, 0x41, 0x11, 
                0x39, 0x49, 0x84, 0x8e, 0x86, 0x1f, 0x8b, 0x70, 0xad, 0xb0, 0xc9, 0x09, 0x1d, 0x81, 0xc3, 
                0x2f, 0xd7, 0x59, 0x78, 0x2a, 0x80, 0x64, 0x73
            };

            // PKCS #1 (A.1.2) Public exponent.
            parameters.Exponent = new byte[] { 0x01, 0x00, 0x01 };

            // PKCS #1 (A.1.2) Private exponent.
            parameters.D = new byte[] {
                0x58, 0x59, 0x5b, 0x65, 0x79, 0x4c, 0xda, 0xbe, 0x46, 0xeb, 0x3e, 0x3c, 0xb4, 0x4f, 0x91,
                0x4c, 0xa2, 0xef, 0x07, 0x5f, 0xdb, 0xb6, 0x00, 0x2d, 0xab, 0xcb, 0xcb, 0xc3, 0xfe, 0x5e,
                0xdc, 0xa9, 0xe7, 0x6d, 0xc0, 0xc3, 0xe9, 0xf6, 0x4e, 0xd3, 0xb9, 0xb8, 0x0d, 0x16, 0x8f,
                0x8d, 0x1a, 0xf2, 0xac, 0x97, 0x6c, 0xa7, 0xca, 0x9a, 0xce, 0x65, 0x1d, 0x71, 0x8d, 0x0e,
                0xd6, 0x82, 0xb8, 0x15, 0x07, 0xe9, 0xa6, 0xb9, 0x27, 0x0e, 0x33, 0xfe, 0x47, 0x55, 0xc7,
                0x85, 0xb8, 0x64, 0x43, 0x85, 0x84, 0xc2, 0x39, 0xb1, 0x3e, 0xcb, 0x59, 0x31, 0x46, 0x5a,
                0x0d, 0x61, 0x79, 0xf1, 0xdd, 0xe8, 0xb5, 0xcd, 0xb7, 0x98, 0x2c, 0xf0, 0x87, 0x76, 0x1c,
                0x51, 0x61, 0x6e, 0xd3, 0xd2, 0xa7, 0x32, 0x9c, 0xd9, 0xb0, 0xae, 0x7d, 0x39, 0xfc, 0xa3,
                0xb6, 0x44, 0xb8, 0xe0, 0x40, 0xd6, 0xa5, 0xb1
            };

            // PKCS #1 (A.1.2) Prime 1.
            parameters.P = new byte[] {
                0xd7, 0x80, 0xdd, 0x0e, 0x0d, 0xba, 0x0a, 0xb2, 0xe1, 0x00, 0x54, 0x08, 0x40, 0xb4, 0xf5, 
                0x2a, 0xb2, 0x95, 0x3d, 0x8a, 0xf4, 0x5a, 0xeb, 0x22, 0xda, 0x8b, 0xd9, 0x02, 0x9c, 0xf6, 
                0xfd, 0x19, 0xcc, 0xed, 0xae, 0xc2, 0x59, 0x42, 0x6e, 0x0d, 0x1d, 0xc2, 0x95, 0x6e, 0x60, 
                0xf6, 0x17, 0x62, 0xf8, 0xa4, 0x34, 0x71, 0xe0, 0xf9, 0xa0, 0x96, 0x17, 0xe5, 0x57, 0xb1, 
                0xbf, 0x6b, 0xa4, 0xfb
            };

            // PKCS #1 (A.1.2) Prime 2.
            parameters.Q = new byte[] {
                0xd6, 0x47, 0xd4, 0xd3, 0xe0, 0xe4, 0x5d, 0xce, 0x88, 0xfe, 0x35, 0xc7, 0x27, 0xaa, 0x43, 
                0x2c, 0x0e, 0x6c, 0xed, 0xd0, 0x47, 0xcd, 0x68, 0x03, 0xb0, 0x1c, 0xe9, 0x95, 0x27, 0x64, 
                0x55, 0x06, 0xca, 0x4f, 0x73, 0x1b, 0x79, 0x88, 0xde, 0x07, 0x23, 0xb1, 0xcd, 0x78, 0x6e, 
                0x94, 0xe5, 0x8b, 0x05, 0x05, 0xbf, 0xbf, 0x19, 0x96, 0xe0, 0x7c, 0x14, 0xf7, 0x92, 0x93, 
                0x46, 0x58, 0xf4, 0xe9
            };

            // PKCS #1 (A.1.2) Exponent 1.
            parameters.DP = new byte[] {
                0x48, 0xfa, 0x01, 0x61, 0x8d, 0xf2, 0x6f, 0x47, 0x0d, 0xfc, 0x9f, 0x78, 0x3f, 0xf9, 0x47,
                0x80, 0x93, 0x03, 0x08, 0xd9, 0x32, 0x50, 0x4b, 0x89, 0xfc, 0xfa, 0x18, 0x9d, 0xd2, 0xeb,
                0xac, 0xdf, 0xfc, 0xce, 0x8c, 0x3c, 0x92, 0x1f, 0x75, 0xc7, 0x09, 0x49, 0xe8, 0x72, 0x7d,
                0x71, 0x38, 0x90, 0x32, 0x64, 0xe0, 0xc1, 0xa3, 0x8e, 0xc4, 0xfb, 0xae, 0xd1, 0xe2, 0x35,
                0x75, 0xfe, 0x0c, 0xdb
            };

            // PKCS #1 (A.1.2) Exponent 2.
            parameters.DQ = new byte[] {
                0x92, 0x52, 0x5a, 0xb2, 0x94, 0x4f, 0x5c, 0xff, 0x3b, 0xec, 0xdb, 0x2c, 0x33, 0x99, 0xc0, 
                0x64, 0xc5, 0x34, 0xfc, 0xef, 0xcd, 0x18, 0x26, 0x7e, 0xde, 0x33, 0xe0, 0x0d, 0x49, 0xe8, 
                0xe9, 0x66, 0xc9, 0x9f, 0x97, 0x2a, 0x9b, 0xc3, 0x2a, 0x5a, 0x15, 0xb5, 0xc4, 0x69, 0x08, 
                0x9a, 0x04, 0x64, 0xf9, 0xf9, 0x03, 0x06, 0xab, 0xa2, 0xab, 0x88, 0x0f, 0x89, 0x3f, 0x3d, 
                0xf2, 0x3b, 0xac, 0x81
            };

            // PKCS #1 (A.1.2) Coefficient.
            parameters.InverseQ = new byte[] {
                0x77, 0x96, 0x6b, 0xaf, 0xa4, 0x3c, 0x00, 0xef, 0xf4, 0x8a, 0xca, 0xff, 0xdd, 0xa6, 0xba,
                0x3b, 0xa3, 0x4b, 0x43, 0x12, 0x12, 0x2c, 0xa7, 0x0f, 0x0e, 0x4d, 0x8d, 0x39, 0xc7, 0x00,
                0x7d, 0x2f, 0xf1, 0x3a, 0xd7, 0xce, 0xcf, 0x8b, 0x09, 0x60, 0xcd, 0xfe, 0x06, 0xce, 0x08,
                0xa3, 0xdf, 0x9d, 0x64, 0x64, 0x83, 0x08, 0x0e, 0x78, 0x2f, 0x0d, 0x37, 0x78, 0x74, 0xcd,
                0x42, 0x08, 0x52, 0xd0
            };

            RequestAuthenticator authenticator = RequestAuthenticatorFactory.GetRsaSha1Authenticator(credentials, accessToken, parameters);

            WebRequest req = WebRequest.Create("http://term.ie/oauth/example/echo_api.php?method=foo&bar=baz");

            authenticator.SignRequest(req);

            string response = Helpers.ReadResponse(req);

            Assert.That(response, Contains.Substring("method=foo&bar=baz"));
        }

        [Test]
        public void PlainTextAuthenticatedRequestParametersShouldBeEchoed()
        {
            RequestAuthenticator authenticator = RequestAuthenticatorFactory.GetPlainTextAuthenticator(credentials, accessToken);

            WebRequest req = WebRequest.Create("http://term.ie/oauth/example/echo_api.php?method=foo&bar=baz");

            authenticator.SignRequest(req);

            string response = Helpers.ReadResponse(req);

            Assert.That(response, Contains.Substring("method=foo&bar=baz"));
        }
    }
}
