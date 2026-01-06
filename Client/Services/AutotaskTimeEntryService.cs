namespace CrownATTime.Client
{
    using CrownATTime.Server.Models;
    using CrownATTime.Server.Models.ATTime;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Configuration;
    using Radzen;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;


    public partial class AutotaskTimeEntryService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;
        private readonly JsonSerializerOptions jsonOptions;

        public AutotaskTimeEntryService(
            NavigationManager navigationManager,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;

            // Matches your server controller route: api/autotask/...
            this.baseUri = new Uri($"{navigationManager.BaseUri}api/autotask/");

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        #region Get time entries for ticket

        partial void OnGetTimeEntriesForTicket(HttpRequestMessage requestMessage);

        /// <summary>
        /// GET api/autotask/timeentries/byticket/{ticketId}
        /// </summary>
        public async Task<List<CrownATTime.Server.Models.TimeEntryDto>> GetTimeEntriesForTicket(long ticketId)
        {
            var uri = new Uri(baseUri, $"timeentries/byticket/{ticketId}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTimeEntriesForTicket(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<List<CrownATTime.Server.Models.TimeEntryDto>>(response);
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

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<CrownATTime.Server.Models.TimeEntryDtoCreatedResult>(response);
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
                    new { op = "eq", field = "isActive", value = true },
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
                    new { op = "eq", field = "isActive", value = true }
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
                    new { op = "eq", field = "isActive", value = true },

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
                    new { op = "eq", field = "isActive", value = true },
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

