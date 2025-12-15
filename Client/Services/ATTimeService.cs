
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace CrownATTime.Client
{
    public partial class ATTimeService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public ATTimeService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/ATTime/");
        }


        public async System.Threading.Tasks.Task ExportBillingCodeCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/billingcodecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/billingcodecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportBillingCodeCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/billingcodecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/billingcodecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetBillingCodeCaches(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.BillingCodeCache>> GetBillingCodeCaches(Query query)
        {
            return await GetBillingCodeCaches(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.BillingCodeCache>> GetBillingCodeCaches(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"BillingCodeCaches");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBillingCodeCaches(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.BillingCodeCache>>(response);
        }

        partial void OnCreateBillingCodeCache(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.BillingCodeCache> CreateBillingCodeCache(CrownATTime.Server.Models.ATTime.BillingCodeCache billingCodeCache = default(CrownATTime.Server.Models.ATTime.BillingCodeCache))
        {
            var uri = new Uri(baseUri, $"BillingCodeCaches");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(billingCodeCache), Encoding.UTF8, "application/json");

            OnCreateBillingCodeCache(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.BillingCodeCache>(response);
        }

        partial void OnDeleteBillingCodeCache(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteBillingCodeCache(int id = default(int))
        {
            var uri = new Uri(baseUri, $"BillingCodeCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteBillingCodeCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetBillingCodeCacheById(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.BillingCodeCache> GetBillingCodeCacheById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"BillingCodeCaches({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBillingCodeCacheById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.BillingCodeCache>(response);
        }

        partial void OnUpdateBillingCodeCache(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateBillingCodeCache(int id = default(int), CrownATTime.Server.Models.ATTime.BillingCodeCache billingCodeCache = default(CrownATTime.Server.Models.ATTime.BillingCodeCache))
        {
            var uri = new Uri(baseUri, $"BillingCodeCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(billingCodeCache), Encoding.UTF8, "application/json");

            OnUpdateBillingCodeCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportContractCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/contractcaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/contractcaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportContractCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/contractcaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/contractcaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetContractCaches(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.ContractCache>> GetContractCaches(Query query)
        {
            return await GetContractCaches(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.ContractCache>> GetContractCaches(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"ContractCaches");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetContractCaches(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.ContractCache>>(response);
        }

        partial void OnCreateContractCache(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.ContractCache> CreateContractCache(CrownATTime.Server.Models.ATTime.ContractCache contractCache = default(CrownATTime.Server.Models.ATTime.ContractCache))
        {
            var uri = new Uri(baseUri, $"ContractCaches");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(contractCache), Encoding.UTF8, "application/json");

            OnCreateContractCache(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.ContractCache>(response);
        }

        partial void OnDeleteContractCache(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteContractCache(int id = default(int))
        {
            var uri = new Uri(baseUri, $"ContractCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteContractCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetContractCacheById(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.ContractCache> GetContractCacheById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"ContractCaches({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetContractCacheById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.ContractCache>(response);
        }

        partial void OnUpdateContractCache(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateContractCache(int id = default(int), CrownATTime.Server.Models.ATTime.ContractCache contractCache = default(CrownATTime.Server.Models.ATTime.ContractCache))
        {
            var uri = new Uri(baseUri, $"ContractCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(contractCache), Encoding.UTF8, "application/json");

            OnUpdateContractCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportResourceCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/resourcecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/resourcecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportResourceCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/resourcecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/resourcecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetResourceCaches(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.ResourceCache>> GetResourceCaches(Query query)
        {
            return await GetResourceCaches(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.ResourceCache>> GetResourceCaches(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"ResourceCaches");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetResourceCaches(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.ResourceCache>>(response);
        }

        partial void OnCreateResourceCache(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.ResourceCache> CreateResourceCache(CrownATTime.Server.Models.ATTime.ResourceCache resourceCache = default(CrownATTime.Server.Models.ATTime.ResourceCache))
        {
            var uri = new Uri(baseUri, $"ResourceCaches");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(resourceCache), Encoding.UTF8, "application/json");

            OnCreateResourceCache(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.ResourceCache>(response);
        }

        partial void OnDeleteResourceCache(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteResourceCache(int id = default(int))
        {
            var uri = new Uri(baseUri, $"ResourceCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteResourceCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetResourceCacheById(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.ResourceCache> GetResourceCacheById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"ResourceCaches({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetResourceCacheById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.ResourceCache>(response);
        }

        partial void OnUpdateResourceCache(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateResourceCache(int id = default(int), CrownATTime.Server.Models.ATTime.ResourceCache resourceCache = default(CrownATTime.Server.Models.ATTime.ResourceCache))
        {
            var uri = new Uri(baseUri, $"ResourceCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(resourceCache), Encoding.UTF8, "application/json");

            OnUpdateResourceCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportRoleCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/rolecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/rolecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportRoleCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/rolecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/rolecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetRoleCaches(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.RoleCache>> GetRoleCaches(Query query)
        {
            return await GetRoleCaches(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.RoleCache>> GetRoleCaches(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"RoleCaches");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetRoleCaches(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.RoleCache>>(response);
        }

        partial void OnCreateRoleCache(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.RoleCache> CreateRoleCache(CrownATTime.Server.Models.ATTime.RoleCache roleCache = default(CrownATTime.Server.Models.ATTime.RoleCache))
        {
            var uri = new Uri(baseUri, $"RoleCaches");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(roleCache), Encoding.UTF8, "application/json");

            OnCreateRoleCache(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.RoleCache>(response);
        }

        partial void OnDeleteRoleCache(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteRoleCache(int id = default(int))
        {
            var uri = new Uri(baseUri, $"RoleCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteRoleCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetRoleCacheById(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.RoleCache> GetRoleCacheById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"RoleCaches({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetRoleCacheById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.RoleCache>(response);
        }

        partial void OnUpdateRoleCache(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateRoleCache(int id = default(int), CrownATTime.Server.Models.ATTime.RoleCache roleCache = default(CrownATTime.Server.Models.ATTime.RoleCache))
        {
            var uri = new Uri(baseUri, $"RoleCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(roleCache), Encoding.UTF8, "application/json");

            OnUpdateRoleCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportTicketEntityPicklistValueCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/ticketentitypicklistvaluecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/ticketentitypicklistvaluecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTicketEntityPicklistValueCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/ticketentitypicklistvaluecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/ticketentitypicklistvaluecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTicketEntityPicklistValueCaches(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache>> GetTicketEntityPicklistValueCaches(Query query)
        {
            return await GetTicketEntityPicklistValueCaches(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache>> GetTicketEntityPicklistValueCaches(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"TicketEntityPicklistValueCaches");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicketEntityPicklistValueCaches(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache>>(response);
        }

        partial void OnCreateTicketEntityPicklistValueCache(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> CreateTicketEntityPicklistValueCache(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache ticketEntityPicklistValueCache = default(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache))
        {
            var uri = new Uri(baseUri, $"TicketEntityPicklistValueCaches");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ticketEntityPicklistValueCache), Encoding.UTF8, "application/json");

            OnCreateTicketEntityPicklistValueCache(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache>(response);
        }

        partial void OnDeleteTicketEntityPicklistValueCache(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTicketEntityPicklistValueCache(int ticketEntityPicklistValueId = default(int))
        {
            var uri = new Uri(baseUri, $"TicketEntityPicklistValueCaches({ticketEntityPicklistValueId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTicketEntityPicklistValueCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTicketEntityPicklistValueCacheByTicketEntityPicklistValueId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> GetTicketEntityPicklistValueCacheByTicketEntityPicklistValueId(string expand = default(string), int ticketEntityPicklistValueId = default(int))
        {
            var uri = new Uri(baseUri, $"TicketEntityPicklistValueCaches({ticketEntityPicklistValueId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicketEntityPicklistValueCacheByTicketEntityPicklistValueId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache>(response);
        }

        partial void OnUpdateTicketEntityPicklistValueCache(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTicketEntityPicklistValueCache(int ticketEntityPicklistValueId = default(int), CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache ticketEntityPicklistValueCache = default(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache))
        {
            var uri = new Uri(baseUri, $"TicketEntityPicklistValueCaches({ticketEntityPicklistValueId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ticketEntityPicklistValueCache), Encoding.UTF8, "application/json");

            OnUpdateTicketEntityPicklistValueCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportTimeEntriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeentries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeentries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTimeEntriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeentries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeentries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTimeEntries(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TimeEntry>> GetTimeEntries(Query query)
        {
            return await GetTimeEntries(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TimeEntry>> GetTimeEntries(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"TimeEntries");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTimeEntries(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TimeEntry>>(response);
        }

        partial void OnCreateTimeEntry(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> CreateTimeEntry(CrownATTime.Server.Models.ATTime.TimeEntry timeEntry = default(CrownATTime.Server.Models.ATTime.TimeEntry))
        {
            var uri = new Uri(baseUri, $"TimeEntries");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(timeEntry), Encoding.UTF8, "application/json");

            OnCreateTimeEntry(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.TimeEntry>(response);
        }

        partial void OnDeleteTimeEntry(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTimeEntry(int timeEntryId = default(int))
        {
            var uri = new Uri(baseUri, $"TimeEntries({timeEntryId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTimeEntry(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTimeEntryByTimeEntryId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> GetTimeEntryByTimeEntryId(string expand = default(string), int timeEntryId = default(int))
        {
            var uri = new Uri(baseUri, $"TimeEntries({timeEntryId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTimeEntryByTimeEntryId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.TimeEntry>(response);
        }

        partial void OnUpdateTimeEntry(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTimeEntry(int timeEntryId = default(int), CrownATTime.Server.Models.ATTime.TimeEntry timeEntry = default(CrownATTime.Server.Models.ATTime.TimeEntry))
        {
            var uri = new Uri(baseUri, $"TimeEntries({timeEntryId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(timeEntry), Encoding.UTF8, "application/json");

            OnUpdateTimeEntry(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportServiceDeskRoleCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/servicedeskrolecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/servicedeskrolecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportServiceDeskRoleCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/servicedeskrolecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/servicedeskrolecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetServiceDeskRoleCaches(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache>> GetServiceDeskRoleCaches(Query query)
        {
            return await GetServiceDeskRoleCaches(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache>> GetServiceDeskRoleCaches(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"ServiceDeskRoleCaches");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetServiceDeskRoleCaches(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache>>(response);
        }

        partial void OnCreateServiceDeskRoleCache(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> CreateServiceDeskRoleCache(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache serviceDeskRoleCache = default(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache))
        {
            var uri = new Uri(baseUri, $"ServiceDeskRoleCaches");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(serviceDeskRoleCache), Encoding.UTF8, "application/json");

            OnCreateServiceDeskRoleCache(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache>(response);
        }

        partial void OnDeleteServiceDeskRoleCache(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteServiceDeskRoleCache(int id = default(int))
        {
            var uri = new Uri(baseUri, $"ServiceDeskRoleCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteServiceDeskRoleCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetServiceDeskRoleCacheById(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> GetServiceDeskRoleCacheById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"ServiceDeskRoleCaches({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetServiceDeskRoleCacheById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache>(response);
        }

        partial void OnUpdateServiceDeskRoleCache(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateServiceDeskRoleCache(int id = default(int), CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache serviceDeskRoleCache = default(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache))
        {
            var uri = new Uri(baseUri, $"ServiceDeskRoleCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(serviceDeskRoleCache), Encoding.UTF8, "application/json");

            OnUpdateServiceDeskRoleCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}