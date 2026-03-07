using CrownATTime.Server.Models;
using Microsoft.AspNetCore.Components;

namespace CrownATTime.Client
{
    public partial class ITGlueService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public ITGlueService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}api/");
        }

        public async Task<string> GetITGlueOrgIdByCompanyName(string companyName = default(string))
        {

            var uri = new Uri(baseUri, $"ITGlue/Organization/ByCompanyName/{Uri.EscapeDataString(companyName.Trim().Replace("'", "''"))}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await response.Content.ReadAsStringAsync();

        }

        public async Task<List<ITGlueDocumentAttributesResults>> GetITGlueDocumentsByOrganizationId(string orgId = default(string))
        {

            var uri = new Uri(baseUri, $"ITGlue/Organization/Documents/ByOrgId/{Uri.EscapeDataString(orgId.Trim().Replace("'", "''"))}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            return await Radzen.HttpResponseMessageExtensions.ReadAsync<List<ITGlueDocumentAttributesResults>>(response);

        }

        public async Task<List<ITGlueDocumentAttributesResults>> GetITGlueDocumentsByOrganizationAndFolderId(string orgId = default(string), string folderId = default(string))
        {

            var uri = new Uri(baseUri, $"ITGlue/Organization/Documents/ByOrgIdAndFolderId/{Uri.EscapeDataString(orgId.Trim().Replace("'", "''"))}/{Uri.EscapeDataString(folderId.Trim().Replace("'", "''"))}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            return await Radzen.HttpResponseMessageExtensions.ReadAsync<List<ITGlueDocumentAttributesResults>>(response);

        }

        public async Task<List<ITGluePasswordAttributeResults>> GetITGluePasswordsByOrganizationId(string orgId = default(string))
        {

            var uri = new Uri(baseUri, $"ITGlue/Organization/Passwords/ByOrgId/{Uri.EscapeDataString(orgId.Trim().Replace("'", "''"))}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            return await Radzen.HttpResponseMessageExtensions.ReadAsync<List<ITGluePasswordAttributeResults>>(response);

        }

        public async Task<ITGluePassword> GetITGluePasswordByPasswordId(string orgId = default(string), string passwordId = default(string))
        {

            var uri = new Uri(baseUri, $"ITGlue/Organization/Passwords/ByPasswordId/{Uri.EscapeDataString(orgId.Trim().Replace("'", "''"))}/{Uri.EscapeDataString(passwordId.Trim().Replace("'", "''"))}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ITGluePassword>(response);

        }
    }
}
