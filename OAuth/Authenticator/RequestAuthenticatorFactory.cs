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

using System.Security.Cryptography;
using OAuth.Base;

namespace OAuth.Authenticator
{
    /// <summary>
    /// OAuth request authenticator factory.
    /// </summary>
    public class RequestAuthenticatorFactory
    {
        /// <summary>
        /// Creates a PLAINTEXT request authenticator.
        /// </summary>
        /// <param name="credentials">Client credentials</param>
        /// <param name="token">Access token</param>
        /// <returns>PLAINTEXT request authenticator</returns>
        public static RequestAuthenticator GetPlainTextAuthenticator(ClientCredentials credentials, AccessToken token)
        {
            return new PlainTextRequestAuthenticator(credentials, token);
        }

        /// <summary>
        /// Creates an HAMC-SHA1 request authenticator.
        /// </summary>
        /// <param name="credentials">Client credentials</param>
        /// <param name="token">Access token</param>
        /// <returns>HAMC-SHA1 request authenticator</returns>
        public static RequestAuthenticator GetHmacSha1Authenticator(ClientCredentials credentials, AccessToken token)
        {
            return new HmacSha1RequestAuthenticator(credentials, token);
        }

        /// <summary>
        /// Creates a RSA-SHA1 request authenticator.
        /// </summary>
        /// <param name="credentials">Client credentials</param>
        /// <param name="token">Access token</param>
        /// <param name="key">Private key</param>
        /// <returns>RSA-SHA1 request authenticator</returns>
        public static RequestAuthenticator GetRsaSha1Authenticator(ClientCredentials credentials, AccessToken token, RSAParameters key)
        {
            return new RsaSha1RequestAuthenticator(credentials, token, key);
        }
    }
}
