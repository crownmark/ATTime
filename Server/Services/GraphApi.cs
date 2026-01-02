using Azure.Identity;
using CrownATTime.Server.Models;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace CrownATTime.Server.Services
{
    public class GraphApi
    {
        private readonly GraphApiOptions _opts;
        private static readonly string[] _scopes = { "https://graph.microsoft.com/.default" };

        public GraphApi(IOptions<GraphApiOptions> options)
        {
            _opts = options.Value;
        }
        public GraphServiceClient CreateGraphClient()
        {
            
            // App-only tokens use ".default"
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var credential = new ClientSecretCredential(_opts.TenantId, _opts.ClientId, _opts.ClientSecret);

            return new GraphServiceClient(credential, scopes);
        }
    }
}
