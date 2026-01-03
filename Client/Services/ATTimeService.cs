
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

        public async System.Threading.Tasks.Task ExportCompanyCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/companycaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/companycaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCompanyCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/companycaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/companycaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCompanyCaches(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.CompanyCache>> GetCompanyCaches(Query query)
        {
            return await GetCompanyCaches(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.CompanyCache>> GetCompanyCaches(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"CompanyCaches");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCompanyCaches(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.CompanyCache>>(response);
        }

        partial void OnCreateCompanyCache(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.CompanyCache> CreateCompanyCache(CrownATTime.Server.Models.ATTime.CompanyCache companyCache = default(CrownATTime.Server.Models.ATTime.CompanyCache))
        {
            var uri = new Uri(baseUri, $"CompanyCaches");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(companyCache), Encoding.UTF8, "application/json");

            OnCreateCompanyCache(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.CompanyCache>(response);
        }

        partial void OnDeleteCompanyCache(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCompanyCache(int id = default(int))
        {
            var uri = new Uri(baseUri, $"CompanyCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCompanyCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCompanyCacheById(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.CompanyCache> GetCompanyCacheById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"CompanyCaches({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCompanyCacheById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.CompanyCache>(response);
        }

        partial void OnUpdateCompanyCache(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCompanyCache(int id = default(int), CrownATTime.Server.Models.ATTime.CompanyCache companyCache = default(CrownATTime.Server.Models.ATTime.CompanyCache))
        {
            var uri = new Uri(baseUri, $"CompanyCaches({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(companyCache), Encoding.UTF8, "application/json");

            OnUpdateCompanyCache(httpRequestMessage);

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

        public async System.Threading.Tasks.Task ExportEmailTemplatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/emailtemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/emailtemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportEmailTemplatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/emailtemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/emailtemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetEmailTemplates(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.EmailTemplate>> GetEmailTemplates(Query query)
        {
            return await GetEmailTemplates(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.EmailTemplate>> GetEmailTemplates(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"EmailTemplates");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetEmailTemplates(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.EmailTemplate>>(response);
        }

        partial void OnCreateEmailTemplate(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.EmailTemplate> CreateEmailTemplate(CrownATTime.Server.Models.ATTime.EmailTemplate emailTemplate = default(CrownATTime.Server.Models.ATTime.EmailTemplate))
        {
            var uri = new Uri(baseUri, $"EmailTemplates");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(emailTemplate), Encoding.UTF8, "application/json");

            OnCreateEmailTemplate(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.EmailTemplate>(response);
        }

        partial void OnDeleteEmailTemplate(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteEmailTemplate(int emailTemplateId = default(int))
        {
            var uri = new Uri(baseUri, $"EmailTemplates({emailTemplateId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteEmailTemplate(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetEmailTemplateByEmailTemplateId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.EmailTemplate> GetEmailTemplateByEmailTemplateId(string expand = default(string), int emailTemplateId = default(int))
        {
            var uri = new Uri(baseUri, $"EmailTemplates({emailTemplateId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetEmailTemplateByEmailTemplateId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.EmailTemplate>(response);
        }

        partial void OnUpdateEmailTemplate(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateEmailTemplate(int emailTemplateId = default(int), CrownATTime.Server.Models.ATTime.EmailTemplate emailTemplate = default(CrownATTime.Server.Models.ATTime.EmailTemplate))
        {
            var uri = new Uri(baseUri, $"EmailTemplates({emailTemplateId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(emailTemplate), Encoding.UTF8, "application/json");

            OnUpdateEmailTemplate(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportNoteTemplatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/notetemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/notetemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportNoteTemplatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/notetemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/notetemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetNoteTemplates(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.NoteTemplate>> GetNoteTemplates(Query query)
        {
            return await GetNoteTemplates(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.NoteTemplate>> GetNoteTemplates(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"NoteTemplates");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetNoteTemplates(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.NoteTemplate>>(response);
        }

        partial void OnCreateNoteTemplate(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.NoteTemplate> CreateNoteTemplate(CrownATTime.Server.Models.ATTime.NoteTemplate noteTemplate = default(CrownATTime.Server.Models.ATTime.NoteTemplate))
        {
            var uri = new Uri(baseUri, $"NoteTemplates");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(noteTemplate), Encoding.UTF8, "application/json");

            OnCreateNoteTemplate(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.NoteTemplate>(response);
        }

        partial void OnDeleteNoteTemplate(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteNoteTemplate(int noteTemplateId = default(int))
        {
            var uri = new Uri(baseUri, $"NoteTemplates({noteTemplateId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteNoteTemplate(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetNoteTemplateByNoteTemplateId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.NoteTemplate> GetNoteTemplateByNoteTemplateId(string expand = default(string), int noteTemplateId = default(int))
        {
            var uri = new Uri(baseUri, $"NoteTemplates({noteTemplateId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetNoteTemplateByNoteTemplateId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.NoteTemplate>(response);
        }

        partial void OnUpdateNoteTemplate(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateNoteTemplate(int noteTemplateId = default(int), CrownATTime.Server.Models.ATTime.NoteTemplate noteTemplate = default(CrownATTime.Server.Models.ATTime.NoteTemplate))
        {
            var uri = new Uri(baseUri, $"NoteTemplates({noteTemplateId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(noteTemplate), Encoding.UTF8, "application/json");

            OnUpdateNoteTemplate(httpRequestMessage);

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

        public async System.Threading.Tasks.Task ExportTicketNoteEntityPicklistValueCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/ticketnoteentitypicklistvaluecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/ticketnoteentitypicklistvaluecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTicketNoteEntityPicklistValueCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/ticketnoteentitypicklistvaluecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/ticketnoteentitypicklistvaluecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTicketNoteEntityPicklistValueCaches(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache>> GetTicketNoteEntityPicklistValueCaches(Query query)
        {
            return await GetTicketNoteEntityPicklistValueCaches(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache>> GetTicketNoteEntityPicklistValueCaches(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"TicketNoteEntityPicklistValueCaches");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicketNoteEntityPicklistValueCaches(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache>>(response);
        }

        partial void OnCreateTicketNoteEntityPicklistValueCache(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> CreateTicketNoteEntityPicklistValueCache(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache ticketNoteEntityPicklistValueCache = default(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache))
        {
            var uri = new Uri(baseUri, $"TicketNoteEntityPicklistValueCaches");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ticketNoteEntityPicklistValueCache), Encoding.UTF8, "application/json");

            OnCreateTicketNoteEntityPicklistValueCache(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache>(response);
        }

        partial void OnDeleteTicketNoteEntityPicklistValueCache(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTicketNoteEntityPicklistValueCache(int ticketNoteEntityPicklistValueId = default(int))
        {
            var uri = new Uri(baseUri, $"TicketNoteEntityPicklistValueCaches({ticketNoteEntityPicklistValueId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTicketNoteEntityPicklistValueCache(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTicketNoteEntityPicklistValueCacheByTicketNoteEntityPicklistValueId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> GetTicketNoteEntityPicklistValueCacheByTicketNoteEntityPicklistValueId(string expand = default(string), int ticketNoteEntityPicklistValueId = default(int))
        {
            var uri = new Uri(baseUri, $"TicketNoteEntityPicklistValueCaches({ticketNoteEntityPicklistValueId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTicketNoteEntityPicklistValueCacheByTicketNoteEntityPicklistValueId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache>(response);
        }

        partial void OnUpdateTicketNoteEntityPicklistValueCache(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTicketNoteEntityPicklistValueCache(int ticketNoteEntityPicklistValueId = default(int), CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache ticketNoteEntityPicklistValueCache = default(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache))
        {
            var uri = new Uri(baseUri, $"TicketNoteEntityPicklistValueCaches({ticketNoteEntityPicklistValueId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ticketNoteEntityPicklistValueCache), Encoding.UTF8, "application/json");

            OnUpdateTicketNoteEntityPicklistValueCache(httpRequestMessage);

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

        public async System.Threading.Tasks.Task ExportTimeEntryTemplatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeentrytemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeentrytemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTimeEntryTemplatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeentrytemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeentrytemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTimeEntryTemplates(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>> GetTimeEntryTemplates(Query query)
        {
            return await GetTimeEntryTemplates(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>> GetTimeEntryTemplates(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"TimeEntryTemplates");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTimeEntryTemplates(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>>(response);
        }

        partial void OnCreateTimeEntryTemplate(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> CreateTimeEntryTemplate(CrownATTime.Server.Models.ATTime.TimeEntryTemplate timeEntryTemplate = default(CrownATTime.Server.Models.ATTime.TimeEntryTemplate))
        {
            var uri = new Uri(baseUri, $"TimeEntryTemplates");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(timeEntryTemplate), Encoding.UTF8, "application/json");

            OnCreateTimeEntryTemplate(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>(response);
        }

        partial void OnDeleteTimeEntryTemplate(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTimeEntryTemplate(int timeEntryTemplateId = default(int))
        {
            var uri = new Uri(baseUri, $"TimeEntryTemplates({timeEntryTemplateId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTimeEntryTemplate(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTimeEntryTemplateByTimeEntryTemplateId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> GetTimeEntryTemplateByTimeEntryTemplateId(string expand = default(string), int timeEntryTemplateId = default(int))
        {
            var uri = new Uri(baseUri, $"TimeEntryTemplates({timeEntryTemplateId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTimeEntryTemplateByTimeEntryTemplateId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>(response);
        }

        partial void OnUpdateTimeEntryTemplate(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTimeEntryTemplate(int timeEntryTemplateId = default(int), CrownATTime.Server.Models.ATTime.TimeEntryTemplate timeEntryTemplate = default(CrownATTime.Server.Models.ATTime.TimeEntryTemplate))
        {
            var uri = new Uri(baseUri, $"TimeEntryTemplates({timeEntryTemplateId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(timeEntryTemplate), Encoding.UTF8, "application/json");

            OnUpdateTimeEntryTemplate(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}