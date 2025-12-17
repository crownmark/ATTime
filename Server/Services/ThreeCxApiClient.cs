namespace CrownATTime.Server.Services
{
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using CrownATTime.Server.Models;
    using Microsoft.Extensions.Options;

    public sealed class ThreeCxApiClient
    {
        private readonly HttpClient _http;
        private readonly ThreeCxOptions _opt;

        // simple in-memory cache
        private string? _accessToken;
        private DateTimeOffset _expiresAtUtc;

        public ThreeCxApiClient(HttpClient http, IOptions<ThreeCxOptions> options)
        {
            _http = http;
            _opt = options.Value;
        }

        private sealed class TokenResponse
        {
            public string? access_token { get; set; }
            public int expires_in { get; set; } // seconds
            public string? token_type { get; set; }
            public string? scope { get; set; }
        }

        private async Task<string> GetAccessTokenAsync(CancellationToken ct)
        {
            // Refresh if missing or expiring within 60 seconds
            if (!string.IsNullOrWhiteSpace(_accessToken) && _expiresAtUtc > DateTimeOffset.UtcNow.AddSeconds(60))
                return _accessToken!;

            if (string.IsNullOrWhiteSpace(_opt.TokenUrl))
                throw new InvalidOperationException("3CX TokenUrl is not configured.");
            if (string.IsNullOrWhiteSpace(_opt.ClientId))
                throw new InvalidOperationException("3CX ClientId is not configured.");
            if (string.IsNullOrWhiteSpace(_opt.ClientSecret))
                throw new InvalidOperationException("3CX ClientSecret is not configured.");

            using var req = new HttpRequestMessage(HttpMethod.Post, _opt.TokenUrl);

            // "Authentication: Header" → send client creds in Authorization header (Basic)
            var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_opt.ClientId}:{_opt.ClientSecret}"));
            req.Headers.Authorization = new AuthenticationHeaderValue("Basic", basic);

            // Standard OAuth2 client_credentials body
            var pairs = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
        };

            if (!string.IsNullOrWhiteSpace(_opt.Scope))
                pairs.Add(new("scope", _opt.Scope!));

            req.Content = new FormUrlEncodedContent(pairs);

            using var resp = await _http.SendAsync(req, ct);
            var json = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException($"3CX token request failed ({(int)resp.StatusCode}): {json}");

            var token = JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (string.IsNullOrWhiteSpace(token?.access_token))
                throw new InvalidOperationException("3CX token response did not contain access_token.");

            _accessToken = token.access_token;
            _expiresAtUtc = DateTimeOffset.UtcNow.AddSeconds(Math.Max(30, token.expires_in)); // safe fallback

            return _accessToken!;
        }

        public async Task<HttpResponseMessage> SendTo3CxAsync(
            HttpMethod method,
            string absoluteUrl,
            HttpContent? content,
            CancellationToken ct)
        {
            var token = await GetAccessTokenAsync(ct);

            var req = new HttpRequestMessage(method, absoluteUrl);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (content != null)
                req.Content = content;

            // Don’t dispose the response here; controller will stream it back.
            return await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
        }
    }

}
