using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace CrownATTime.Server.Services
{
    public class GraphApi
    {
        static string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
        static string baseUrl = "https://graph.microsoft.com/v1.0";

        public static GraphServiceClient CreateGraphClient()
        {
            string tenantId = "b9e807ee-7ce7-421d-8be8-5a5a3a241e86";
            string clientId = "b45aeab4-3833-4142-b409-d128b99e68f3";
            string clientSecret = "R3s8Q~rrvBAdVgAwtcLgMYvTX1Bkht6QIN33Sdvl";
            // App-only tokens use ".default"
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            return new GraphServiceClient(credential, scopes);
        }
    }
}
