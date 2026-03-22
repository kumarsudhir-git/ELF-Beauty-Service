using Brewery.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Brewery.Infrastructure.ExternalServices
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientService> _logger;

        public HttpClientService(HttpClient httpClient, ILogger<HttpClientService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // GENERIC GET
        public async Task<TResponse> GetAsync<TResponse>(string url)
        {
            try
            {
                _logger.LogInformation("GET Request to {Url}", url);

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("GET failed: {Error}", error);
                    throw new Exception($"GET request failed: {response.StatusCode}");
                }

                var result = await response.Content.ReadFromJsonAsync<TResponse>();

                return result!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GET request");
                throw;
            }
        }

        // GENERIC POST
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            try
            {
                _logger.LogInformation("POST Request to {Url}", url);

                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("POST failed: {Error}", error);
                    throw new Exception($"POST request failed: {response.StatusCode}");
                }

                var result = await response.Content.ReadFromJsonAsync<TResponse>();

                return result!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in POST request");
                throw;
            }
        }
    }
}
