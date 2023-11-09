using Auth.Entities;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Net;


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
                RequestUri = new Uri($"{_endpointUrl}Login")
            };

            message.Content = new StringContent(JsonConvert.SerializeObject(user, new JsonSerializerSettings{ }), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.SendAsync(message);

            if (result.StatusCode != HttpStatusCode.OK)
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
