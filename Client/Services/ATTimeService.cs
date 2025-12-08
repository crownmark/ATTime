
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
    }
}