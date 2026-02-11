using CrownATTime.Client.Pages;
using CrownATTime.Server.Models;
using CrownATTime.Server.Models.ATTime;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CrownATTime.Client
{
    public class TeamsChatService
    {
        private readonly Uri baseUri;
        private readonly HttpClient httpClient;
        private readonly NavigationManager navigationManager;
        private readonly JsonSerializerOptions jsonOptions;
        public TeamsChatService(
            NavigationManager navigationManager,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;

            this.baseUri = new Uri($"{navigationManager.BaseUri}");

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task SendTeamsChannelMessage(CrownATTime.Server.Models.TeamsMessageRequest message)
        {
            //var uri = new Uri(baseUri, $"Teams/SendChannelMessage");
            var uri = new Uri("https://automation.ce-technology.com/webhook/b8ae1dac-fade-405c-ab85-d533193aac17");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            var json = JsonSerializer.Serialize(message, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                throw new Exception($"Error Sending Teams Channel Message.  {content}");
            }
        }
    }  

}
