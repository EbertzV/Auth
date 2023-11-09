using Auth.Entities;
using Auth.Services.DTO;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Auth.Services
{
    public sealed class EvaluationService
    {
        private readonly string _serviceUrl;
        public EvaluationService(string serviceUrl)
        {
                _serviceUrl = serviceUrl;
        }

        public async Task<SessionResult> EvaluateSessionFromToken(string token)
        {
            var client = new HttpClient();

            var message = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_serviceUrl}Session")
            };

            message.Headers.Clear();
            message.Headers.Add("accept", "application/json");

            message.Content = new StringContent(JsonConvert.SerializeObject(new { token }, new JsonSerializerSettings { }), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(message);

            if (result.StatusCode != HttpStatusCode.OK)
                return SessionResult.NewInvalid();

            using var stream = await result.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(stream);
            var response = await streamReader.ReadToEndAsync();
            if(string.IsNullOrWhiteSpace(response)) 
                return SessionResult.NewInvalid();

            try
            {
                var dto = JsonConvert.DeserializeObject<SessionResponse>(response);
                if (dto.Valid)
                    return SessionResult.NewValid();
                else
                    return SessionResult.NewInvalid();

            } catch (Exception ex)
            {
                return SessionResult.NewInvalid();
            }
        }
    }
}
