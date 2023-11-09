using Auth.Entities;
using System.Net;
using System.Text.Json;

namespace Auth.Services
{
    public sealed class AuthenticationService
    {
        private readonly string _endpointUrl;

        public AuthenticationService(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
        }

        public async Task<AuthenticationResult> TryAuthenticate(User user)
        {
            var client = new HttpClient();

            var message = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_endpointUrl)
            };

            message.Headers.Clear();
            message.Content = new StringContent(JsonSerializer.Serialize(user));

            var result = await client.SendAsync(message);

            if(result.StatusCode != HttpStatusCode.OK)
                return AuthenticationResult.NewFailure(result.StatusCode);

            using var stream = await result.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            var authResult = await reader.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(authResult))
                return AuthenticationResult.NewFailure(result.StatusCode);
            return AuthenticationResult.NewSuccess(authResult);
        }
    }
}
