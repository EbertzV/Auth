using System.Net;

namespace Auth.Entities
{
    public sealed class AuthenticationResult
    {
        private AuthenticationResult(bool isAuthenticated, string token, HttpStatusCode statusCode)
        {
            IsAuthenticated = isAuthenticated;
            Token = token;
            StatusCode = statusCode;
        }

        public static AuthenticationResult NewFailure(HttpStatusCode statusCode)
            => new AuthenticationResult(false, string.Empty, statusCode);

        public static AuthenticationResult NewSuccess(string token)
            => new AuthenticationResult(true, token, HttpStatusCode.OK);

        public bool IsAuthenticated { get; }
        public string Token { get; }
        public HttpStatusCode StatusCode { get; }
    }
}
