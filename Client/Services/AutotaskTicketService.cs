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
    using System.Threading.Tasks;


    public partial class AutotaskTicketService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;
        private readonly JsonSerializerOptions jsonOptions;

        public AutotaskTicketService(
            NavigationManager navigationManager,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;

            // This matches the server controller route: api/autotask/...
            this.baseUri = new Uri($"{navigationManager.BaseUri}api/autotask/");

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Partial hook so you can add headers / logging if needed.
        /// </summary>
        partial void OnGetTicket(HttpRequestMessage requestMessage);

        /// <summary>
        /// Get a single Autotask Ticket by ID via your server API.
        /// GET api/autotask/tickets/{ticketId}
        /// </summary>
        public async Task<TicketDtoResult> GetTicket(long ticketId)
        {
            var uri = new Uri(baseUri, $"tickets/{ticketId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var convert = JsonSerializer.Deserialize<TicketDtoResult>(content);
            }
            catch (Exception ex)
            {

            }
            return JsonSerializer.Deserialize<TicketDtoResult>(content);
        }

        public async Task<CrownATTime.Server.Models.TicketUpdateDto> UpdateTicket(CrownATTime.Server.Models.TicketUpdateDto ticket)
        {
            var uri = new Uri(baseUri, $"tickets");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            var json = JsonSerializer.Serialize(ticket, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<CrownATTime.Server.Models.TicketUpdateDto>(response);
        }
        public async Task<ContactDtoResult> GetContact(long contactId)
        {
            var uri = new Uri(baseUri, $"contacts/{contactId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ContactDtoResult>(content);
        }

        public async Task<AutotaskItemsResponse<ContactDtoResult.Item>> GetContacts(int companyId)
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "isActive", value = 1 },
                    new { op = "eq", field = "companyID", value = companyId },
                    //new { op = "ne", field = "emailAddress", value = "" },
                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"contacts/query?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AutotaskItemsResponse<ContactDtoResult.Item>>(content);
        }

        public async Task<CompanyDtoResult> GetCompany(long companyId)
        {
            var uri = new Uri(baseUri, $"companies/{companyId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CompanyDtoResult>(content);

        }
        public async Task SyncCompanies()
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "isActive", value = true },
                    new { op = "noteq", field = "id", value = 0},
                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"companies/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                throw new Exception($"Error Syncing Companies.  {content}");
            }
        }
        public async Task<ConfigurationItemResult> GetConfigurationItem(int configurationId)
        {
            var uri = new Uri(baseUri, $"ConfigurationItems/{configurationId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ConfigurationItemResult>(content);

        }

        public async Task<ContractDtoResult> GetContract(long ticketId)
        {
            var uri = new Uri(baseUri, $"contracts/{ticketId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ContractDtoResult>(content);
        }

        public async Task SyncContracts()
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "status", value = 1 }
                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"contracts/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            //var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<ContractDto>>(content);
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                throw new Exception($"Error Syncing Contracts.  {content}");
            }
        }

        public async Task<List<TicketChecklistItemResult>> GetOpenTicketChecklistItems(int ticketId)
        {
            var filters = new List<object>
            {
                new { op = "eq", field = "ticketID", value = ticketId },
                new { op = "eq", field = "isCompleted", value = false },
            };

            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var encodedSearch = Uri.EscapeDataString(JsonSerializer.Serialize(searchObj));
            var uri = new Uri(baseUri, $"TicketChecklistItems/query?search={encodedSearch}");

            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            using var response = await httpClient.SendAsync(request);

            // Throw only on true HTTP failure
            if (!response.IsSuccessStatusCode)
            {
                // Optional: log content for debugging before throwing
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Autotask API call failed ({(int)response.StatusCode}): {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();

            // EMPTY OR NOTHING FOUND → Autotask returns: { "items": [] }
            if (string.IsNullOrWhiteSpace(content) || content.Trim() == "{}")
            {
                return new List<TicketChecklistItemResult>();

            }

            // Try safe parse
            List<TicketChecklistItemResult> result = null;
            try
            {
                var items = JsonSerializer.Deserialize<AutotaskItemsResponse<TicketChecklistItemResult>>(content);
                result = items.Items.ToList();
            }
            catch
            {
                // If conversion fails, return null instead of throwing
                return new List<TicketChecklistItemResult>();
            }

            // If Autotask returned an empty items array
            if (result == null)
            {
                return new List<TicketChecklistItemResult>();

            }

            return result;
        }

        public async Task<TicketChecklistItemResult> UpdateTicketChecklistItem(TicketChecklistItemResult checklistItem)
        {
            var uri = new Uri(baseUri, $"TicketChecklistItems");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            var json = JsonSerializer.Serialize(checklistItem, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<TicketChecklistItemResult>(response);
        }

        /// <summary>
        /// (Optional) If you later add a server endpoint like:
        /// GET api/autotask/tickets/bycompany/{companyId}
        /// you can add a matching client method here.
        /// </summary>
        partial void OnGetTicketsForCompany(HttpRequestMessage requestMessage);

        public async Task<System.Collections.Generic.List<TicketDto>> GetTicketsForCompany(long companyId)
        {
            var uri = new Uri(baseUri, $"tickets/bycompany/{companyId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicketsForCompany(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<System.Collections.Generic.List<TicketDto>>(response);
        }

        public async Task<TicketEntityFieldsDto.EntityInformationFieldsResponse> GetTicketFields()
        {
            var uri = new Uri(baseUri, $"tickets/fields");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<TicketEntityFieldsDto.EntityInformationFieldsResponse>(response);
        }
        public async Task SyncTicketFields()
        {
            
            var uri = new Uri(baseUri, $"tickets/fields/sync");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                throw new Exception($"Error Syncing Ticket Picklists.  {content}");
            }
        }

        public async Task SyncTicketNoteFields()
        {

            var uri = new Uri(baseUri, $"ticketNotes/fields/sync");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                throw new Exception($"Error Syncing Ticket Notes Picklists.  {content}");
            }
        }

        public async Task<AutotaskItemsResponse<ContractDto>> GetTicketContracts(long companyId)
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "companyID", value = companyId },
                    new { op = "eq", field = "status", value = 1 }
                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"contracts/query?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            //var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<ContractDto>>(content);
            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<AutotaskItemsResponse<ContractDto>>(response);
        }


    }
}
