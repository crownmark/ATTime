namespace CrownATTime.Client
{
    using CrownATTime.Client.Pages;
    using CrownATTime.Server.Models.ATTime;
    using global::CrownATTime.Server.Models;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Configuration;
    using Radzen;
    using Radzen.Blazor.Rendering;
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using static CrownATTime.Server.Models.ThreeCxMakeCallResult;

    public partial class AutotaskService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;
        private readonly JsonSerializerOptions jsonOptions;
        private readonly ATTimeService _atTimeService;

        public AutotaskService(
            NavigationManager navigationManager,
            HttpClient httpClient,
            IConfiguration configuration,
            ATTimeService atTimeService)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;
            this._atTimeService = atTimeService;

            // This matches the server controller route: api/autotask/...
            this.baseUri = new Uri($"{navigationManager.BaseUri}api/autotask/");

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            _atTimeService = atTimeService;
        }

        public async Task<AutotaskItemsResponse<AccountAlertsDtoResult>> GetAccountAlertsByCompanyId(int id)
        {
            var uri = new Uri(baseUri, $"AccountAlerts/{id}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var convert = JsonSerializer.Deserialize<AutotaskItemsResponse<AccountAlertsDtoResult>>(content);
            }
            catch (Exception ex)
            {

            }
            return JsonSerializer.Deserialize<AutotaskItemsResponse<AccountAlertsDtoResult>>(content);
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
                    new { op = "notin", field = "emailAddress", value = new[] {"AAAAA@AAAAA.AAAAA" } },
                    //AAAAA@AAAAA.AAAAA
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

        public async Task<AutotaskItemsResponse<TicketAdditionalContactsDtoResult>> GetAdditionalContacts(int ticketId)
        {
            var uri = new Uri(baseUri, $"tickets/contacts/{ticketId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AutotaskItemsResponse<TicketAdditionalContactsDtoResult>>(content);
        }

        public async Task<AutotaskItemsResponse<TicketSecondaryResourcesDtoResult>> GetSecondaryResources(int ticketId)
        {
            var uri = new Uri(baseUri, $"tickets/resources/{ticketId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AutotaskItemsResponse<TicketSecondaryResourcesDtoResult>>(content);
        }

        public async Task<CompanyLocationDto> GetCompanyLocationByLocationId(int locationId)
        {
            var uri = new Uri(baseUri, $"companylocations/{locationId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CompanyLocationDto>(content);

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
                    //new { op = "eq", field = "isActive", value = true },
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
                //new { op = "eq", field = "status", value = 1 }
                new { op = "gt", field = "id", value = 0 }

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

        public async Task<ChecklistItemDtoResult> GetTicketChecklistItem(int TicketId, int Id)
        {
            var uri = new Uri(baseUri, $"TicketChecklistItem/{TicketId}/{Id}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);

            var content = await response.Content.ReadAsStringAsync();

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<ChecklistItemDtoResult>(response);


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

        public async Task<List<TicketChecklistItemResult>> GetAllTicketChecklistItems(int ticketId)
        {
            var filters = new List<object>
            {
                new { op = "eq", field = "ticketID", value = ticketId },
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

        public async Task<List<TicketChecklistItemResult>> GetAllTicketChecklistItemsCompletedToday(int ticketId)
        {
            var filters = new List<object>
            {
                new { op = "eq", field = "ticketID", value = ticketId },
                new { op = "gt", field = "completedDateTime", value = DateTime.Today },

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

        public async Task<AutotaskItemCreatedResult> DeleteChecklistItem(TicketChecklistItemResult checklistItem)
        {
            var uri = new Uri(baseUri, $"TicketChecklistItems");
            
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            var json = JsonSerializer.Serialize(checklistItem, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<AutotaskItemCreatedResult>(response);
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
        public async Task SyncTicketUdfFields()
        {

            var uri = new Uri(baseUri, $"userdefinefields/sync");

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

        public async Task<AutotaskItemsResponse<AttachmentDtoResult>> GetAttachmentsForTicket(long ticketId)
        {
            
            var uri = new Uri(baseUri, $"ticketattachments/{ticketId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<AttachmentDtoResult>>(content);
            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<AutotaskItemsResponse<AttachmentDtoResult>>(response);
        }

        public async Task<HttpResponseMessage> DeleteAttachment(AttachmentDtoResult attachment)
        {
            var uri = new Uri(baseUri, $"ticketattachments");
            
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            var json = JsonSerializer.Serialize(attachment, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async Task<AutotaskItemsResponse<ServiceCall>> GetServiceCallsForTicket(long ticketId)
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "ticketID", value = ticketId },
                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"ticketservicecalls/query?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<ServiceCall>>(content);
            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<AutotaskItemsResponse<ServiceCall>>(response);
        }

        #region Get time entries for ticket

        partial void OnGetTimeEntriesForTicket(HttpRequestMessage requestMessage);

        /// <summary>
        /// GET api/autotask/timeentries/byticket/{ticketId}
        /// </summary>
        public async Task<AutotaskItemsResponse<TimeEntryDto>> GetTimeEntriesForTicket(long ticketId)
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "ticketID", value = ticketId },
                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"tickettimeentries/query?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);

            var result = await Radzen.HttpResponseMessageExtensions
                .ReadAsync<AutotaskItemsResponse<TimeEntryDto>>(response);

            //return await Radzen.HttpResponseMessageExtensions
            //    .ReadAsync<AutotaskItemsResponse<TimeEntryDto>>(response);

            // ----------------------------------------------------
            // 🔥 Get distinct resource IDs from time entries
            // ----------------------------------------------------
            var resourceIds = result.Items
                .Select(x => x.ResourceId)
                .Distinct()
                .ToList();

            // ----------------------------------------------------
            // 🔥 Pull matching resources from your cache table
            // ----------------------------------------------------
            var resourcesResult = await _atTimeService.GetResourceCaches();
            var resources = resourcesResult.Value.Where(r => resourceIds.Contains(r.Id)).ToList();

            var resourceLookup = resources
                .ToDictionary(r => r.Id);

            // ----------------------------------------------------
            // 🔥 Map back onto DTO
            // ----------------------------------------------------
            foreach (var item in result.Items)
            {
                if (resourceLookup.TryGetValue(item.ResourceId, out var resource))
                {
                    item.ResourceName = resource.FirstName + " " + resource.LastName;
                    item.ResourceEmail = resource.Email;
                }
                item.isTimeEntry = true;
            }
            return result;
        }

        public async Task<AutotaskItemsResponse<NoteDto>> GetNotesForTicket(long ticketId)
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "ticketID", value = ticketId },
                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"ticketnotes/{ticketId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);

            var result = await Radzen.HttpResponseMessageExtensions
                .ReadAsync<AutotaskItemsResponse<NoteDto>>(response);

            //return await Radzen.HttpResponseMessageExtensions
            //    .ReadAsync<AutotaskItemsResponse<TimeEntryDto>>(response);

            // ----------------------------------------------------
            // 🔥 Get distinct resource IDs from time entries
            // ----------------------------------------------------
            var resourceIds = result.Items
                .Select(x => x.creatorResourceID)
                .Distinct()
                .ToList();

            // ----------------------------------------------------
            // 🔥 Pull matching resources from your cache table
            // ----------------------------------------------------
            var resourcesResult = await _atTimeService.GetResourceCaches();
            var resources = resourcesResult.Value.Where(r => resourceIds.Contains(r.Id)).ToList();

            var resourceLookup = resources
                .ToDictionary(r => r.Id);

            // ----------------------------------------------------
            // 🔥 Map back onto DTO
            // ----------------------------------------------------
            foreach (var item in result.Items)
            {
                if (resourceLookup.TryGetValue(item.creatorResourceID.Value, out var resource))
                {
                    item.ResourceName = resource.FirstName + " " + resource.LastName;
                    item.ResourceEmail = resource.Email;
                }
            }
            return result;
        }

        #endregion

        #region Get single time entry

        partial void OnGetTimeEntry(HttpRequestMessage requestMessage);

        /// <summary>
        /// GET api/autotask/timeentries/{timeEntryId}
        /// </summary>
        public async Task<CrownATTime.Server.Models.TimeEntryDto> GetTimeEntry(long timeEntryId)
        {
            var uri = new Uri(baseUri, $"timeentries/{timeEntryId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTimeEntry(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<CrownATTime.Server.Models.TimeEntryDto>(response);
        }

        #endregion

        #region Create time entry

        partial void OnCreateTimeEntry(HttpRequestMessage requestMessage);

        /// <summary>
        /// POST api/autotask/timeentries
        /// </summary>
        public async Task<CrownATTime.Server.Models.TimeEntryDtoCreatedResult> CreateTimeEntry(
            CrownATTime.Server.Models.TimeEntryCreateDto timeEntry)
        {
            var uri = new Uri(baseUri, $"timeentries");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            var json = JsonSerializer.Serialize(timeEntry, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            OnCreateTimeEntry(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Try to extract Autotask-style error message
                string message = result;

                try
                {
                    using var doc = JsonDocument.Parse(message);
                    if (doc.RootElement.TryGetProperty("errors", out var errors) &&
                        errors.ValueKind == JsonValueKind.Array &&
                        errors.GetArrayLength() > 0)
                    {
                        message = string.Join("; ", errors.EnumerateArray().Select(e => e.GetString()));
                    }
                }
                catch
                {
                    // fallback: raw body
                }

                throw new Exception($"Autotask time entry failed: {message}");
            }

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<CrownATTime.Server.Models.TimeEntryDtoCreatedResult>(response);
        }

        public async Task<CrownATTime.Server.Models.AutotaskItemCreatedResult> CreateChecklistItem(CrownATTime.Server.Models.ChecklistItemDto checklistItem)
        {
            var uri = new Uri(baseUri, $"TicketChecklistItems");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            var json = JsonSerializer.Serialize(checklistItem, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            OnCreateTimeEntry(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var result = await response.Content.ReadAsStringAsync();

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<CrownATTime.Server.Models.AutotaskItemCreatedResult>(response);
        }

        public async Task<CrownATTime.Server.Models.NoteDtoCreatedResult> CreateNote(CrownATTime.Server.Models.NoteDto note)
        {
            var uri = new Uri(baseUri, $"notes");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            var json = JsonSerializer.Serialize(note, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            OnCreateTimeEntry(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var result = await response.Content.ReadAsStringAsync();

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<CrownATTime.Server.Models.NoteDtoCreatedResult>(response);
        }

        public async Task<CrownATTime.Server.Models.AutotaskItemCreatedResult> CreateTicketAttachment(CrownATTime.Server.Models.AttachmentCreateDto attachment)
        {
            var uri = new Uri(baseUri, $"ticketattachments");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            var json = JsonSerializer.Serialize(attachment, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            OnCreateTimeEntry(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var result = await response.Content.ReadAsStringAsync();

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<CrownATTime.Server.Models.AutotaskItemCreatedResult>(response);
        }

        #endregion

        #region Update time entry

        partial void OnUpdateTimeEntry(HttpRequestMessage requestMessage);

        /// <summary>
        /// PATCH api/autotask/timeentries/{timeEntryId}
        /// </summary>
        public async Task<CrownATTime.Server.Models.TimeEntryCreateDto> UpdateTimeEntry(
            long timeEntryId,
            CrownATTime.Server.Models.TimeEntryUpdateDto timeEntry)
        {
            var uri = new Uri(baseUri, $"timeentries/{timeEntryId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            var json = JsonSerializer.Serialize(timeEntry, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            OnUpdateTimeEntry(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<CrownATTime.Server.Models.TimeEntryCreateDto>(response);
        }

        #endregion

        #region Delete time entry

        partial void OnDeleteTimeEntry(HttpRequestMessage requestMessage);

        /// <summary>
        /// DELETE api/autotask/timeentries/{timeEntryId}
        /// </summary>
        public async Task<HttpResponseMessage> DeleteTimeEntry(long timeEntryId)
        {
            var uri = new Uri(baseUri, $"timeentries/{timeEntryId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTimeEntry(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        #endregion

        public async Task<TimeEntryEntityFieldsDto.EntityInformationFieldsResponse> GetTimeEntryFields()
        {
            var uri = new Uri(baseUri, $"timeentries/fields");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<TimeEntryEntityFieldsDto.EntityInformationFieldsResponse>(response);
        }
        public async Task<AutotaskItemsResponse<ContractExclusionBillingCodeResult>?> GetContractExclusionsBillingCode(int contractID, int billingCodeID)
        {
            var filters = new List<object>
            {
                new { op = "eq", field = "contractID", value = contractID },
                new { op = "eq", field = "billingCodeID", value = billingCodeID },
            };

            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var encodedSearch = Uri.EscapeDataString(JsonSerializer.Serialize(searchObj));
            var uri = new Uri(baseUri, $"contractexclusionbillingcodes/query?search={encodedSearch}");

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
                return null;
            }

            // Try safe parse
            AutotaskItemsResponse<ContractExclusionBillingCodeResult>? result = null;
            try
            {
                return JsonSerializer.Deserialize<AutotaskItemsResponse<ContractExclusionBillingCodeResult>>(content);

            }
            catch
            {
                // If conversion fails, return null instead of throwing
                return null;
            }

            // If Autotask returned an empty items array
            if (result == null)
            {
                return null;
            }

            return result;
        }

        public async Task<AutotaskItemsResponse<BillingCodeDto>> GetBillingCodes()
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "isActive", value = true },
                    new { op = "eq", field = "useType", value = 1 },
                    //new { op = "eq", field = "billingCodeType", value = 0 },

                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"billingcodes/query?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<BillingCodeDto>>(content);
            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<AutotaskItemsResponse<BillingCodeDto>>(response);
        }
        public async Task SyncBillingCodes()
        {
            var filters = new List<object>
                {
                    //new { op = "eq", field = "isActive", value = true },
                    new { op = "eq", field = "useType", value = 1 },
                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"billingcodes/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                throw new Exception($"Error Syncing Contracts.  {content}");
            }
        }
        public async Task<ResourceDtoResultSingle> GetResourceById(int resourceId)
        {

            var uri = new Uri(baseUri, $"resources/{resourceId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var converted = JsonSerializer.Deserialize<ResourceDtoResultSingle>(content);
            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<ResourceDtoResultSingle>(response);
        }
        public async Task<AutotaskItemsResponse<ResourceDtoResult>> GetLoggedInResource(string email)
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "isActive", value = true },
                    new { op = "eq", field = "email", value = $"{email}" },
                    //new { op = "eq", field = "billingCodeType", value = 0 },

                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"resources/query?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<ResourceDtoResult>>(content);
            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<AutotaskItemsResponse<ResourceDtoResult>>(response);
        }
        public async Task SyncResources()
        {
            var filters = new List<object>
            {
                //new { op = "eq", field = "isActive", value = true }
                new { op = "gt", field = "id", value = 0 }
            };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"resources/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                throw new Exception($"Error Syncing Resources.  {content}");
            }
        }
        public async Task<AutotaskItemsResponse<RoleDto>> GetRoles()
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "isActive", value = true },
                    //new { op = "eq", field = "resourceID", value = 29682885 },
                    //new { op = "eq", field = "billingCodeType", value = 0 },

                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"roles/query?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<RoleDto>>(content);
            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<AutotaskItemsResponse<RoleDto>>(response);
        }

        public async Task SyncRoles()
        {
            var filters = new List<object>
                {
                //new { op = "eq", field = "isActive", value = true },
                //new { op = "eq", field = "isActive", value = true },

            };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"roles/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                throw new Exception($"Error Syncing Roles.  {content}");
            }
        }

        public async Task SyncServiceDeskRoles()
        {
            var filters = new List<object>
                {
                    //new { op = "eq", field = "isActive", value = true },
                    //new { op = "eq", field = "resourceID", value = resourceId },

                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"servicedeskroles/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                throw new Exception($"Error Syncing Service Desk Roles.  {content}");
            }
        }
        public async Task<AutotaskItemsResponse<SericeDeskRoleDto>> GetServiceDeskRoles(int resourceId)
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "isActive", value = true },
                    new { op = "eq", field = "resourceID", value = resourceId },

                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri(baseUri, $"servicedeskroles/query?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<SericeDeskRoleDto>>(content);
            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<AutotaskItemsResponse<SericeDeskRoleDto>>(response);
        }

        public static List<RoleCache> MapToServiceDeskRoles(IEnumerable<RoleCache> allRoles, IEnumerable<ServiceDeskRoleCache> serviceDeskRoles, bool onlyActive = true)
        {
            //If RoleId is a plain int
            var sdRoleIds = serviceDeskRoles
                .Select(s => s.RoleId)   // just use the int
                .Distinct()
                .ToHashSet();

            var roles = allRoles
                .Where(r => sdRoleIds.Contains(r.Id));

            if (onlyActive)
            {
                roles = roles.Where(r => r.IsActive);
            }

            return roles
                .OrderBy(r => r.Name)
                .ToList();
        }
    }
}
