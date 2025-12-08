namespace CrownATTime.Client
{
    using CrownATTime.Server.Models;
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
        public async Task<CrownATTime.Server.Models.TimeEntryDto> CreateTimeEntry(
            CrownATTime.Server.Models.TimeEntryCreateDto timeEntry)
        {
            var uri = new Uri(baseUri, $"timeentries");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            var json = JsonSerializer.Serialize(timeEntry, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            OnCreateTimeEntry(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions
                .ReadAsync<CrownATTime.Server.Models.TimeEntryDto>(response);
        }

        #endregion

        #region Update time entry

        partial void OnUpdateTimeEntry(HttpRequestMessage requestMessage);

        /// <summary>
        /// PATCH api/autotask/timeentries/{timeEntryId}
        /// </summary>
        public async Task<CrownATTime.Server.Models.TimeEntryDto> UpdateTimeEntry(
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
                .ReadAsync<CrownATTime.Server.Models.TimeEntryDto>(response);
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

        public async Task<AutotaskItemsResponse<BillingCodeDto>> GetBillingCodes()
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "isActive", value = true },
                    new { op = "eq", field = "useType", value = 1 },
                    new { op = "eq", field = "billingCodeType", value = 0 },

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
        public async Task<AutotaskItemsResponse<SericeDeskRoleDto>> GetServiceDeskRoles(int resourceId)
        {
            var filters = new List<object>
                {
                    new { op = "eq", field = "isActive", value = true },
                    new { op = "eq", field = "resourceID", value = resourceId },
                    //new { op = "eq", field = "billingCodeType", value = 0 },

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

        public static List<RoleDto> MapToServiceDeskRoles(IEnumerable<RoleDto> allRoles, IEnumerable<SericeDeskRoleDto> serviceDeskRoles, bool onlyActive = true)
        {
            //If RoleId is a plain int
            var sdRoleIds = serviceDeskRoles
                .Select(s => s.roleID)   // just use the int
                .Distinct()
                .ToHashSet();

            var roles = allRoles
                .Where(r => sdRoleIds.Contains(r.id));

            if (onlyActive)
            {
                roles = roles.Where(r => r.isActive);
            }

            return roles
                .OrderBy(r => r.name)
                .ToList();
        }

    }


}

