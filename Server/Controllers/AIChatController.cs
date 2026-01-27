namespace CrownATTime.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http.Headers;
    using System.Text;

    [ApiController]
    [Route("api/[controller]")]
    public class AIChatController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public AIChatController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpPost]
        public async Task Proxy()
        {
            var endpoint = _config["Ai:Endpoint"]!;
            var apiKey = _config["Ai:ApiKey"]!;
            var apiKeyHeader = _config["Ai:ApiKeyHeader"] ?? "Authorization";

            using var client = _httpClientFactory.CreateClient();

            // Read incoming body (Radzen sends OpenAI-compatible payload)
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            using var req = new HttpRequestMessage(HttpMethod.Post, endpoint);
            req.Content = new StringContent(body, Encoding.UTF8, "application/json");

            // Set auth header
            if (apiKeyHeader.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            else
                req.Headers.TryAddWithoutValidation(apiKeyHeader, apiKey);

            // Stream back as-is
            using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, HttpContext.RequestAborted);

            Response.StatusCode = (int)resp.StatusCode;
            foreach (var h in resp.Headers)
                Response.Headers[h.Key] = h.Value.ToArray();
            foreach (var h in resp.Content.Headers)
                Response.Headers[h.Key] = h.Value.ToArray();

            // Important for streaming
            Response.Headers.Remove("transfer-encoding");

            await using var respStream = await resp.Content.ReadAsStreamAsync(HttpContext.RequestAborted);
            await respStream.CopyToAsync(Response.Body, HttpContext.RequestAborted);
        }
    }

}
