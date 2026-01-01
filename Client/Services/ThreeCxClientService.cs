namespace CrownATTime.Client
{
    using global::CrownATTime.Server.Models;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Configuration;
    using Radzen;
    using Radzen.Blazor.Rendering;
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public partial class ThreeCxClientService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;
        private readonly JsonSerializerOptions jsonOptions;

        public ThreeCxClientService(
            NavigationManager navigationManager,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;

            // This matches the server controller route: api/autotask/...
            this.baseUri = new Uri($"{navigationManager.BaseUri}api/ThreeCX/");

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }


        public static string NormalizeUsPhone(string? phoneNumber)
        {
            // Convert to string safely
            var rawInput = phoneNumber ?? string.Empty;

            // Strip all non-digit characters
            var digits = Regex.Replace(rawInput, @"\D", "");

            // Normalize assuming US numbers
            if (digits.Length == 10)
            {
                return "+1" + digits;
            }

            if (digits.Length == 11 && digits.StartsWith("1"))
            {
                return "+" + digits;
            }

            // Invalid or unsupported format
            return string.Empty;
        }
        public async Task<ThreeCxMakeCallResult.Calls> MakeCall(string phoneNumber, string extension)
        {
            var normalizedPhoneNumber = NormalizeUsPhone(phoneNumber);
            var uri = new Uri(baseUri, $"callcontrol/makecall");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            var callModel = new MakeCallModel()
            {
                Destination = normalizedPhoneNumber,
                Timeout = 0,
                Extension = extension
            };
            var json = JsonSerializer.Serialize(callModel, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var convert = JsonSerializer.Deserialize<ThreeCxMakeCallResult.Calls>(content);
            }
            catch (Exception ex)
            {

            }
            return JsonSerializer.Deserialize<ThreeCxMakeCallResult.Calls>(content);
        }

        public async Task<ThreeCxCallStatusResult.Calls> GetCallStatus(string extension)
        {
            //var normalizedPhoneNumber = NormalizeUsPhone(phoneNumber);
            var uri = new Uri(baseUri, $"callcontrol/{extension}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            //var callModel = new MakeCallModel()
            //{
            //    Destination = normalizedPhoneNumber,
            //    Timeout = 0,
            //    Extension = extension
            //};
            //var json = JsonSerializer.Serialize(callModel, jsonOptions);
            //httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var convert = JsonSerializer.Deserialize<ThreeCxCallStatusResult.Calls>(content);
            }
            catch (Exception ex)
            {

            }
            return JsonSerializer.Deserialize<ThreeCxCallStatusResult.Calls>(content);
        }
    }
}
