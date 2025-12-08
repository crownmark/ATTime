using CrownATTime.Server.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CrownATTime.Server.Services
{
    public abstract class AutotaskApiClient
    {
        protected readonly HttpClient _http;
        protected readonly AutotaskOptions _options;
        protected readonly JsonSerializerOptions _jsonOptions;

        protected AutotaskApiClient(HttpClient http, IOptions<AutotaskOptions> options)
        {
            _http = http;
            _options = options.Value;

            if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            {
                throw new InvalidOperationException("Autotask BaseUrl is not configured.");
            }

            // IMPORTANT: Use your zone-specific URL here, NOT the generic /autotask.net one.
            // Example: https://webservices5.autotask.net/atservicesrest/v1.0/
            if (_http.BaseAddress == null)
            {
                _http.BaseAddress = new Uri(_options.BaseUrl);
            }

            _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Builds an HttpRequestMessage with the required Autotask REST headers.
        /// </summary>
        protected HttpRequestMessage CreateRequest(HttpMethod method, string relativeUrl, HttpContent? content = null)
        {
            var request = new HttpRequestMessage(method, relativeUrl);

            // Auth headers – MUST match docs
            request.Headers.Remove("ApiIntegrationCode");
            request.Headers.Remove("UserName");
            request.Headers.Remove("Secret");

            request.Headers.Add("ApiIntegrationCode", _options.ApiIntegrationCode);
            request.Headers.Add("UserName", _options.UserName);
            request.Headers.Add("Secret", _options.Secret);

            // Content-Type is shown as a required header in their examples
            // and Autotask’s Postman collection.
            request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
            request.Headers.Accept.ParseAdd("application/json");

            if (content != null)
            {
                request.Content = content;
            }

            return request;
        }

        protected async Task<T> ReadAsAsync<T>(HttpResponseMessage response, CancellationToken ct)
        {
            // If we’re still unauthorized, we want the raw body for debugging.
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                throw new InvalidOperationException(
                    $"Autotask returned {(int)response.StatusCode} {response.ReasonPhrase}. Body: {body}");
            }

            var stream = await response.Content.ReadAsStreamAsync(ct);
            var result = await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions, ct);

            if (result == null)
            {
                throw new InvalidOperationException("Autotask API returned an empty response.");
            }

            return result;
        }
    }
}
