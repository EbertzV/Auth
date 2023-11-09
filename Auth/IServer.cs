
using Auth.Entities;
using Auth.Services;
using System.Runtime.InteropServices;

namespace Auth
{
    [Guid("08C8572F-1872-4B6A-A901-198DE8872002")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    public interface IServer
    {
        Task<string> Authenticate(string serviceUrl, string username, string password);
        Task<bool> Evaluate(string serviceUrl, string token);
    }

    [Guid("8CB239EB-0B1B-4C8A-8C88-73052D418A63")]
    [ComVisible(true)]
    public class Server : IServer
    {
        public async Task<string> Authenticate(string serviceUrl, string username, string password)
        {
            var authService = new AuthenticationService(serviceUrl);
            var user = new User(username, password);

            var result = await authService.TryAuthenticate(user);

            if (result.IsAuthenticated)
                return result.Token;
            else
                return string.Empty;
        }

        public async Task<bool> Evaluate(string serviceUrl, string token)
        {
            var sessionService = new EvaluationService(serviceUrl);
            var sessResult = await sessionService.EvaluateSessionFromToken(token);
            return sessResult.Valid;
        }
    }
}
