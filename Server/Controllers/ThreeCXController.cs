namespace CrownATTime.Server.Controllers
{
    using CrownATTime.Server.Models;
    using CrownATTime.Server.Services;
    using Microsoft.AspNetCore.Http.Json;
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Policy;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using static CrownATTime.Client.ThreeCxClientService;
    using static System.Net.WebRequestMethods;

    [ApiController]
    [Route("api/[controller]")]
    public sealed class ThreeCxController : ControllerBase
    {
        private readonly HttpClient _http;
        private CrownATTime.Server.Data.ATTimeContext context;
        private readonly ThreeCxApiClient _client;
        // Simple in-memory token cache
        private static string? _accessToken;
        private static DateTimeOffset _expiresUtc;

        // Move these to secrets/config later
        private const string PBX_BASE = "https://crownenterprises.ca.3cx.us";
        private const string CLIENT_ID = "timeguard";
        private const string CLIENT_SECRET = "pbmXE8qX3FWd7Dfh8p8f8fuf3cAXTa80";
        public ThreeCxController(IHttpClientFactory httpClientFactory, CrownATTime.Server.Data.ATTimeContext _context, ThreeCxApiClient client)
        {
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = new Uri(PBX_BASE);
            _client = client;
            //_http.DefaultRequestHeaders.Add("UserName", "bmjnrji6mu6q57m@ce-technology.com");
            //_http.DefaultRequestHeaders.Add("Secret", "0Js$n3*ZY@z4#5Bp1o~XmR7#@");
            //_http.DefaultRequestHeaders.Add("ApiIntegrationCode", "GGNM7AWDIIZCZWTKY4SF4UGQEDE");
            context = _context;
        }
        private async Task<string> GetAccessTokenAsync()
        {
            // Return cached token if still valid (30s buffer)
            if (!string.IsNullOrWhiteSpace(_accessToken) &&
                DateTimeOffset.UtcNow < _expiresUtc.AddSeconds(-30))
            {
                return _accessToken!;
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, "/connect/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = CLIENT_ID,
                    ["client_secret"] = CLIENT_SECRET,
                    ["grant_type"] = "client_credentials"
                })
            };

            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(
                    $"3CX OAuth token request failed: {(int)response.StatusCode}\n{json}");

            var token = JsonSerializer.Deserialize<ThreeCxTokenResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Invalid token response");

            _accessToken = token.access_token;
            _expiresUtc = DateTimeOffset.UtcNow.AddSeconds(token.expires_in);

            return _accessToken!;
        }


        [HttpPost("callcontrol/makecall")]
        public async Task<IActionResult> MakeCall([FromBody] MakeCallModel call)
        {
            try
            {
                var token = await GetAccessTokenAsync();

                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"/callcontrol/{call.Extension}/makecall");

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                request.Content = new StringContent(
                    JsonSerializer.Serialize(new
                    {
                        destination = call.Destination,
                        timeout = call.Timeout
                    }),
                    Encoding.UTF8,
                    "application/json");

                using var response = await _http.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                return StatusCode((int)response.StatusCode, body);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating ticket: {ex.Message}");
            }
        }

        [HttpGet("callcontrol/{extension}")]
        public async Task<IActionResult> GetCallStatus(string extension)
        {
            try
            {
                var token = await GetAccessTokenAsync();

                using var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"/callcontrol/{extension}");

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                //request.Content = new StringContent(
                //    JsonSerializer.Serialize(new
                //    {
                //        destination = call.Destination,
                //        timeout = call.Timeout
                //    }),
                //    Encoding.UTF8,
                //    "application/json");

                using var response = await _http.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                return StatusCode((int)response.StatusCode, body);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating ticket: {ex.Message}");
            }
        }
        public sealed class ThreeCxTokenResponse
        {
            public string token_type { get; set; } = "Bearer";
            public int expires_in { get; set; }          // seconds
            public string access_token { get; set; } = "";
            public string? refresh_token { get; set; }
        }

    }

}
