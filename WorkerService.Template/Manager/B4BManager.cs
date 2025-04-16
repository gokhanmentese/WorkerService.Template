using WorkerService.Template.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkerService.Template.Manager
{
    public interface IB4BService
    {
        Task<B4BResponse> Send(List<Order> orders);
    }

    public class B4BManager : IB4BService
    {
        private readonly B4BSettings _b4BSettings;
        private readonly IntegrationLoginRequest _loginRequest;
        private readonly ILogger<B4BManager> _logger;

        public B4BManager(B4BSettings b4BSettings, ILogger<B4BManager> logger)
        {
            _b4BSettings = b4BSettings;
            _logger = logger;

            _loginRequest = new IntegrationLoginRequest
            {
                UserCode = _b4BSettings.UserCode,
                Password = _b4BSettings.Password
            };
        }

        public async Task<B4BResponse> Send(List<Order> orders)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, _b4BSettings.ApiURL + "UpdateIntegrationProducts");

            var b4bRequest = new B4BRequest();
            b4bRequest.Orders = orders;

            var accessToken = await GetToken(_loginRequest);

            b4bRequest.SecretKey = accessToken.SecretKey;
            b4bRequest.IntegrationLoginRequest = _loginRequest;

            var serialized = JsonSerializer.Serialize(b4bRequest);

            var content = new StringContent(serialized, null, "application/json");

            request.Content = content;

            _logger.LogInformation(serialized);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var bResponse = JsonSerializer.Deserialize<B4BResponse>(result);

            return bResponse;
        }


        private async Task<B4BToken> GetToken(IntegrationLoginRequest loginRequest)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, _b4BSettings.ApiURL + "EryazIntegrationLogin");

            var content = new StringContent(JsonSerializer.Serialize(loginRequest), null, "application/json");

            request.Content = content;
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var bResponse = JsonSerializer.Deserialize<B4BToken>(result);

            return bResponse;

        }
    }
}
