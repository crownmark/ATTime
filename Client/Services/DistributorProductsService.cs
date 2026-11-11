
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
    public partial class DistributorProductsService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public DistributorProductsService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/DistributorProducts/");
        }


        public async System.Threading.Tasks.Task ExportIngramLenovoFamilyImagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramlenovofamilyimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramlenovofamilyimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportIngramLenovoFamilyImagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramlenovofamilyimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramlenovofamilyimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetIngramLenovoFamilyImages(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>> GetIngramLenovoFamilyImages(Query query)
        {
            return await GetIngramLenovoFamilyImages(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>> GetIngramLenovoFamilyImages(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"IngramLenovoFamilyImages");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramLenovoFamilyImages(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>>(response);
        }

        partial void OnCreateIngramLenovoFamilyImage(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> CreateIngramLenovoFamilyImage(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage ingramLenovoFamilyImage = default(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage))
        {
            var uri = new Uri(baseUri, $"IngramLenovoFamilyImages");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramLenovoFamilyImage), Encoding.UTF8, "application/json");

            OnCreateIngramLenovoFamilyImage(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>(response);
        }

        partial void OnDeleteIngramLenovoFamilyImage(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteIngramLenovoFamilyImage(string machineType = default(string))
        {
            var uri = new Uri(baseUri, $"IngramLenovoFamilyImages('{Uri.EscapeDataString(machineType.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteIngramLenovoFamilyImage(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetIngramLenovoFamilyImageByMachineType(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> GetIngramLenovoFamilyImageByMachineType(string expand = default(string), string machineType = default(string))
        {
            var uri = new Uri(baseUri, $"IngramLenovoFamilyImages('{Uri.EscapeDataString(machineType.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramLenovoFamilyImageByMachineType(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>(response);
        }

        partial void OnUpdateIngramLenovoFamilyImage(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateIngramLenovoFamilyImage(string machineType = default(string), CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage ingramLenovoFamilyImage = default(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage))
        {
            var uri = new Uri(baseUri, $"IngramLenovoFamilyImages('{Uri.EscapeDataString(machineType.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramLenovoFamilyImage), Encoding.UTF8, "application/json");

            OnUpdateIngramLenovoFamilyImage(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportIngramLenovoScrapeSourcesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramlenovoscrapesources/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramlenovoscrapesources/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportIngramLenovoScrapeSourcesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramlenovoscrapesources/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramlenovoscrapesources/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetIngramLenovoScrapeSources(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>> GetIngramLenovoScrapeSources(Query query)
        {
            return await GetIngramLenovoScrapeSources(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>> GetIngramLenovoScrapeSources(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"IngramLenovoScrapeSources");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramLenovoScrapeSources(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>>(response);
        }

        partial void OnCreateIngramLenovoScrapeSource(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> CreateIngramLenovoScrapeSource(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource ingramLenovoScrapeSource = default(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource))
        {
            var uri = new Uri(baseUri, $"IngramLenovoScrapeSources");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramLenovoScrapeSource), Encoding.UTF8, "application/json");

            OnCreateIngramLenovoScrapeSource(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>(response);
        }

        partial void OnDeleteIngramLenovoScrapeSource(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteIngramLenovoScrapeSource(string vendorPartNumber = default(string))
        {
            var uri = new Uri(baseUri, $"IngramLenovoScrapeSources('{Uri.EscapeDataString(vendorPartNumber.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteIngramLenovoScrapeSource(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetIngramLenovoScrapeSourceByVendorPartNumber(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> GetIngramLenovoScrapeSourceByVendorPartNumber(string expand = default(string), string vendorPartNumber = default(string))
        {
            var uri = new Uri(baseUri, $"IngramLenovoScrapeSources('{Uri.EscapeDataString(vendorPartNumber.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramLenovoScrapeSourceByVendorPartNumber(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>(response);
        }

        partial void OnUpdateIngramLenovoScrapeSource(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateIngramLenovoScrapeSource(string vendorPartNumber = default(string), CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource ingramLenovoScrapeSource = default(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource))
        {
            var uri = new Uri(baseUri, $"IngramLenovoScrapeSources('{Uri.EscapeDataString(vendorPartNumber.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramLenovoScrapeSource), Encoding.UTF8, "application/json");

            OnUpdateIngramLenovoScrapeSource(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportIngramManufacturerContentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingrammanufacturercontents/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingrammanufacturercontents/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportIngramManufacturerContentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingrammanufacturercontents/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingrammanufacturercontents/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetIngramManufacturerContents(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>> GetIngramManufacturerContents(Query query)
        {
            return await GetIngramManufacturerContents(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>> GetIngramManufacturerContents(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"IngramManufacturerContents");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramManufacturerContents(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>>(response);
        }

        partial void OnCreateIngramManufacturerContent(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> CreateIngramManufacturerContent(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent ingramManufacturerContent = default(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent))
        {
            var uri = new Uri(baseUri, $"IngramManufacturerContents");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramManufacturerContent), Encoding.UTF8, "application/json");

            OnCreateIngramManufacturerContent(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>(response);
        }

        partial void OnDeleteIngramManufacturerContent(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteIngramManufacturerContent(string vendorName = default(string), string vendorPartNumber = default(string))
        {
            var uri = new Uri(baseUri, $"IngramManufacturerContents(VendorName='{Uri.EscapeDataString(vendorName.Trim().Replace("'", "''"))}',VendorPartNumber='{Uri.EscapeDataString(vendorPartNumber.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteIngramManufacturerContent(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetIngramManufacturerContentByVendorNameAndVendorPartNumber(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> GetIngramManufacturerContentByVendorNameAndVendorPartNumber(string expand = default(string), string vendorName = default(string), string vendorPartNumber = default(string))
        {
            var uri = new Uri(baseUri, $"IngramManufacturerContents(VendorName='{Uri.EscapeDataString(vendorName.Trim().Replace("'", "''"))}',VendorPartNumber='{Uri.EscapeDataString(vendorPartNumber.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramManufacturerContentByVendorNameAndVendorPartNumber(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>(response);
        }

        partial void OnUpdateIngramManufacturerContent(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateIngramManufacturerContent(string vendorName = default(string), string vendorPartNumber = default(string), CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent ingramManufacturerContent = default(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent))
        {
            var uri = new Uri(baseUri, $"IngramManufacturerContents(VendorName='{Uri.EscapeDataString(vendorName.Trim().Replace("'", "''"))}',VendorPartNumber='{Uri.EscapeDataString(vendorPartNumber.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramManufacturerContent), Encoding.UTF8, "application/json");

            OnUpdateIngramManufacturerContent(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportIngramProductsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproducts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproducts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportIngramProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproducts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproducts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetIngramProducts(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProduct>> GetIngramProducts(Query query)
        {
            return await GetIngramProducts(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProduct>> GetIngramProducts(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"IngramProducts");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramProducts(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProduct>>(response);
        }

        partial void OnCreateIngramProduct(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProduct> CreateIngramProduct(CrownATTime.Server.Models.DistributorProducts.IngramProduct ingramProduct = default(CrownATTime.Server.Models.DistributorProducts.IngramProduct))
        {
            var uri = new Uri(baseUri, $"IngramProducts");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramProduct), Encoding.UTF8, "application/json");

            OnCreateIngramProduct(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramProduct>(response);
        }

        partial void OnDeleteIngramProduct(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteIngramProduct(string ingramPartNumber = default(string))
        {
            var uri = new Uri(baseUri, $"IngramProducts('{Uri.EscapeDataString(ingramPartNumber.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteIngramProduct(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetIngramProductByIngramPartNumber(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProduct> GetIngramProductByIngramPartNumber(string expand = default(string), string ingramPartNumber = default(string))
        {
            var uri = new Uri(baseUri, $"IngramProducts('{Uri.EscapeDataString(ingramPartNumber.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramProductByIngramPartNumber(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramProduct>(response);
        }

        partial void OnUpdateIngramProduct(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateIngramProduct(string ingramPartNumber = default(string), CrownATTime.Server.Models.DistributorProducts.IngramProduct ingramProduct = default(CrownATTime.Server.Models.DistributorProducts.IngramProduct))
        {
            var uri = new Uri(baseUri, $"IngramProducts('{Uri.EscapeDataString(ingramPartNumber.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramProduct), Encoding.UTF8, "application/json");

            OnUpdateIngramProduct(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportIngramProductEnrichmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportIngramProductEnrichmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetIngramProductEnrichments(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>> GetIngramProductEnrichments(Query query)
        {
            return await GetIngramProductEnrichments(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>> GetIngramProductEnrichments(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"IngramProductEnrichments");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramProductEnrichments(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>>(response);
        }

        partial void OnCreateIngramProductEnrichment(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> CreateIngramProductEnrichment(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment ingramProductEnrichment = default(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment))
        {
            var uri = new Uri(baseUri, $"IngramProductEnrichments");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramProductEnrichment), Encoding.UTF8, "application/json");

            OnCreateIngramProductEnrichment(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>(response);
        }

        partial void OnDeleteIngramProductEnrichment(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteIngramProductEnrichment(string ingramPartNumber = default(string))
        {
            var uri = new Uri(baseUri, $"IngramProductEnrichments('{Uri.EscapeDataString(ingramPartNumber.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteIngramProductEnrichment(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetIngramProductEnrichmentByIngramPartNumber(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> GetIngramProductEnrichmentByIngramPartNumber(string expand = default(string), string ingramPartNumber = default(string))
        {
            var uri = new Uri(baseUri, $"IngramProductEnrichments('{Uri.EscapeDataString(ingramPartNumber.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramProductEnrichmentByIngramPartNumber(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>(response);
        }

        partial void OnUpdateIngramProductEnrichment(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateIngramProductEnrichment(string ingramPartNumber = default(string), CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment ingramProductEnrichment = default(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment))
        {
            var uri = new Uri(baseUri, $"IngramProductEnrichments('{Uri.EscapeDataString(ingramPartNumber.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramProductEnrichment), Encoding.UTF8, "application/json");

            OnUpdateIngramProductEnrichment(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportIngramSyncRulesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramsyncrules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramsyncrules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportIngramSyncRulesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramsyncrules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramsyncrules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetIngramSyncRules(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>> GetIngramSyncRules(Query query)
        {
            return await GetIngramSyncRules(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>> GetIngramSyncRules(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"IngramSyncRules");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramSyncRules(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>>(response);
        }

        partial void OnCreateIngramSyncRule(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> CreateIngramSyncRule(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule ingramSyncRule = default(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule))
        {
            var uri = new Uri(baseUri, $"IngramSyncRules");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramSyncRule), Encoding.UTF8, "application/json");

            OnCreateIngramSyncRule(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>(response);
        }

        partial void OnDeleteIngramSyncRule(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteIngramSyncRule(int ruleId = default(int))
        {
            var uri = new Uri(baseUri, $"IngramSyncRules({ruleId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteIngramSyncRule(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetIngramSyncRuleByRuleId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> GetIngramSyncRuleByRuleId(string expand = default(string), int ruleId = default(int))
        {
            var uri = new Uri(baseUri, $"IngramSyncRules({ruleId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramSyncRuleByRuleId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>(response);
        }

        partial void OnUpdateIngramSyncRule(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateIngramSyncRule(int ruleId = default(int), CrownATTime.Server.Models.DistributorProducts.IngramSyncRule ingramSyncRule = default(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule))
        {
            var uri = new Uri(baseUri, $"IngramSyncRules({ruleId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramSyncRule), Encoding.UTF8, "application/json");

            OnUpdateIngramSyncRule(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportIngramWarehouseInventoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramwarehouseinventories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramwarehouseinventories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportIngramWarehouseInventoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramwarehouseinventories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramwarehouseinventories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetIngramWarehouseInventories(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>> GetIngramWarehouseInventories(Query query)
        {
            return await GetIngramWarehouseInventories(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>> GetIngramWarehouseInventories(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"IngramWarehouseInventories");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramWarehouseInventories(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>>(response);
        }

        partial void OnCreateIngramWarehouseInventory(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> CreateIngramWarehouseInventory(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory ingramWarehouseInventory = default(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory))
        {
            var uri = new Uri(baseUri, $"IngramWarehouseInventories");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramWarehouseInventory), Encoding.UTF8, "application/json");

            OnCreateIngramWarehouseInventory(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>(response);
        }

        partial void OnDeleteIngramWarehouseInventory(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteIngramWarehouseInventory(string ingramPartNumber = default(string), string warehouseLocation = default(string))
        {
            var uri = new Uri(baseUri, $"IngramWarehouseInventories(IngramPartNumber='{Uri.EscapeDataString(ingramPartNumber.Trim().Replace("'", "''"))}',WarehouseLocation='{Uri.EscapeDataString(warehouseLocation.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteIngramWarehouseInventory(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetIngramWarehouseInventoryByIngramPartNumberAndWarehouseLocation(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> GetIngramWarehouseInventoryByIngramPartNumberAndWarehouseLocation(string expand = default(string), string ingramPartNumber = default(string), string warehouseLocation = default(string))
        {
            var uri = new Uri(baseUri, $"IngramWarehouseInventories(IngramPartNumber='{Uri.EscapeDataString(ingramPartNumber.Trim().Replace("'", "''"))}',WarehouseLocation='{Uri.EscapeDataString(warehouseLocation.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramWarehouseInventoryByIngramPartNumberAndWarehouseLocation(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>(response);
        }

        partial void OnUpdateIngramWarehouseInventory(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateIngramWarehouseInventory(string ingramPartNumber = default(string), string warehouseLocation = default(string), CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory ingramWarehouseInventory = default(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory))
        {
            var uri = new Uri(baseUri, $"IngramWarehouseInventories(IngramPartNumber='{Uri.EscapeDataString(ingramPartNumber.Trim().Replace("'", "''"))}',WarehouseLocation='{Uri.EscapeDataString(warehouseLocation.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(ingramWarehouseInventory), Encoding.UTF8, "application/json");

            OnUpdateIngramWarehouseInventory(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefeditionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefeditions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefeditions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefeditionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefeditions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefeditions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLenovoPsrefeditions(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>> GetLenovoPsrefeditions(Query query)
        {
            return await GetLenovoPsrefeditions(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>> GetLenovoPsrefeditions(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefeditions");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefeditions(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>>(response);
        }

        partial void OnCreateLenovoPsrefedition(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> CreateLenovoPsrefedition(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition lenovoPsrefedition = default(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefeditions");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(lenovoPsrefedition), Encoding.UTF8, "application/json");

            OnCreateLenovoPsrefedition(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>(response);
        }

        partial void OnDeleteLenovoPsrefedition(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteLenovoPsrefedition(long editionId = default(long))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefeditions({editionId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteLenovoPsrefedition(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetLenovoPsrefeditionByEditionId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> GetLenovoPsrefeditionByEditionId(string expand = default(string), long editionId = default(long))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefeditions({editionId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefeditionByEditionId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>(response);
        }

        partial void OnUpdateLenovoPsrefedition(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateLenovoPsrefedition(long editionId = default(long), CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition lenovoPsrefedition = default(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefeditions({editionId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(lenovoPsrefedition), Encoding.UTF8, "application/json");

            OnUpdateLenovoPsrefedition(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsreffamiliesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsreffamilies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsreffamilies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsreffamiliesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsreffamilies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsreffamilies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLenovoPsreffamilies(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>> GetLenovoPsreffamilies(Query query)
        {
            return await GetLenovoPsreffamilies(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>> GetLenovoPsreffamilies(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsreffamilies");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsreffamilies(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>>(response);
        }

        partial void OnCreateLenovoPsreffamily(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> CreateLenovoPsreffamily(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily lenovoPsreffamily = default(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily))
        {
            var uri = new Uri(baseUri, $"LenovoPsreffamilies");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(lenovoPsreffamily), Encoding.UTF8, "application/json");

            OnCreateLenovoPsreffamily(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>(response);
        }

        partial void OnDeleteLenovoPsreffamily(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteLenovoPsreffamily(long familyId = default(long))
        {
            var uri = new Uri(baseUri, $"LenovoPsreffamilies({familyId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteLenovoPsreffamily(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetLenovoPsreffamilyByFamilyId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> GetLenovoPsreffamilyByFamilyId(string expand = default(string), long familyId = default(long))
        {
            var uri = new Uri(baseUri, $"LenovoPsreffamilies({familyId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsreffamilyByFamilyId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>(response);
        }

        partial void OnUpdateLenovoPsreffamily(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateLenovoPsreffamily(long familyId = default(long), CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily lenovoPsreffamily = default(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily))
        {
            var uri = new Uri(baseUri, $"LenovoPsreffamilies({familyId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(lenovoPsreffamily), Encoding.UTF8, "application/json");

            OnUpdateLenovoPsreffamily(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsreffamilyImagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsreffamilyimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsreffamilyimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsreffamilyImagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsreffamilyimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsreffamilyimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLenovoPsreffamilyImages(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage>> GetLenovoPsreffamilyImages(Query query)
        {
            return await GetLenovoPsreffamilyImages(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage>> GetLenovoPsreffamilyImages(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsreffamilyImages");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsreffamilyImages(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage>>(response);
        }

        partial void OnCreateLenovoPsreffamilyImage(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> CreateLenovoPsreffamilyImage(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage lenovoPsreffamilyImage = default(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage))
        {
            var uri = new Uri(baseUri, $"LenovoPsreffamilyImages");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(lenovoPsreffamilyImage), Encoding.UTF8, "application/json");

            OnCreateLenovoPsreffamilyImage(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage>(response);
        }

        partial void OnDeleteLenovoPsreffamilyImage(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteLenovoPsreffamilyImage(long imageId = default(long))
        {
            var uri = new Uri(baseUri, $"LenovoPsreffamilyImages({imageId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteLenovoPsreffamilyImage(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetLenovoPsreffamilyImageByImageId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> GetLenovoPsreffamilyImageByImageId(string expand = default(string), long imageId = default(long))
        {
            var uri = new Uri(baseUri, $"LenovoPsreffamilyImages({imageId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsreffamilyImageByImageId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage>(response);
        }

        partial void OnUpdateLenovoPsreffamilyImage(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateLenovoPsreffamilyImage(long imageId = default(long), CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage lenovoPsreffamilyImage = default(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage))
        {
            var uri = new Uri(baseUri, $"LenovoPsreffamilyImages({imageId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(lenovoPsreffamilyImage), Encoding.UTF8, "application/json");

            OnUpdateLenovoPsreffamilyImage(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefgalleryBeforeV27SToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefgallerybeforev27s/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefgallerybeforev27s/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefgalleryBeforeV27SToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefgallerybeforev27s/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefgallerybeforev27s/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLenovoPsrefgalleryBeforeV27S(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27>> GetLenovoPsrefgalleryBeforeV27S(Query query)
        {
            return await GetLenovoPsrefgalleryBeforeV27S(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27>> GetLenovoPsrefgalleryBeforeV27S(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefgalleryBeforeV27S");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefgalleryBeforeV27S(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27>>(response);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefimageDataToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefimagedata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefimagedata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefimageDataToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefimagedata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefimagedata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLenovoPsrefimageData(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>> GetLenovoPsrefimageData(Query query)
        {
            return await GetLenovoPsrefimageData(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>> GetLenovoPsrefimageData(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefimageData");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefimageData(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>>(response);
        }

        partial void OnCreateLenovoPsrefimageDatum(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> CreateLenovoPsrefimageDatum(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum lenovoPsrefimageDatum = default(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefimageData");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(lenovoPsrefimageDatum), Encoding.UTF8, "application/json");

            OnCreateLenovoPsrefimageDatum(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>(response);
        }

        partial void OnDeleteLenovoPsrefimageDatum(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteLenovoPsrefimageDatum(long imageId = default(long))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefimageData({imageId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteLenovoPsrefimageDatum(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetLenovoPsrefimageDatumByImageId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> GetLenovoPsrefimageDatumByImageId(string expand = default(string), long imageId = default(long))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefimageData({imageId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefimageDatumByImageId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>(response);
        }

        partial void OnUpdateLenovoPsrefimageDatum(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateLenovoPsrefimageDatum(long imageId = default(long), CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum lenovoPsrefimageDatum = default(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefimageData({imageId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(lenovoPsrefimageDatum), Encoding.UTF8, "application/json");

            OnUpdateLenovoPsrefimageDatum(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefskuFamilyMapsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefskufamilymaps/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefskufamilymaps/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefskuFamilyMapsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefskufamilymaps/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefskufamilymaps/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLenovoPsrefskuFamilyMaps(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>> GetLenovoPsrefskuFamilyMaps(Query query)
        {
            return await GetLenovoPsrefskuFamilyMaps(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>> GetLenovoPsrefskuFamilyMaps(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefskuFamilyMaps");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefskuFamilyMaps(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>>(response);
        }

        partial void OnCreateLenovoPsrefskuFamilyMap(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> CreateLenovoPsrefskuFamilyMap(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap lenovoPsrefskuFamilyMap = default(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefskuFamilyMaps");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(lenovoPsrefskuFamilyMap), Encoding.UTF8, "application/json");

            OnCreateLenovoPsrefskuFamilyMap(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>(response);
        }

        partial void OnDeleteLenovoPsrefskuFamilyMap(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteLenovoPsrefskuFamilyMap(string vendorPartNumber = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefskuFamilyMaps('{Uri.EscapeDataString(vendorPartNumber.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteLenovoPsrefskuFamilyMap(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetLenovoPsrefskuFamilyMapByVendorPartNumber(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> GetLenovoPsrefskuFamilyMapByVendorPartNumber(string expand = default(string), string vendorPartNumber = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefskuFamilyMaps('{Uri.EscapeDataString(vendorPartNumber.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefskuFamilyMapByVendorPartNumber(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>(response);
        }

        partial void OnUpdateLenovoPsrefskuFamilyMap(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateLenovoPsrefskuFamilyMap(string vendorPartNumber = default(string), CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap lenovoPsrefskuFamilyMap = default(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefskuFamilyMaps('{Uri.EscapeDataString(vendorPartNumber.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(lenovoPsrefskuFamilyMap), Encoding.UTF8, "application/json");

            OnUpdateLenovoPsrefskuFamilyMap(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveProductsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproducts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproducts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproducts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproducts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStreakwaveProducts(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>> GetStreakwaveProducts(Query query)
        {
            return await GetStreakwaveProducts(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>> GetStreakwaveProducts(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveProducts");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStreakwaveProducts(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>>(response);
        }

        partial void OnCreateStreakwaveProduct(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> CreateStreakwaveProduct(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct streakwaveProduct = default(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct))
        {
            var uri = new Uri(baseUri, $"StreakwaveProducts");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(streakwaveProduct), Encoding.UTF8, "application/json");

            OnCreateStreakwaveProduct(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>(response);
        }

        partial void OnDeleteStreakwaveProduct(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStreakwaveProduct(string sku = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveProducts('{Uri.EscapeDataString(sku.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStreakwaveProduct(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStreakwaveProductBySku(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> GetStreakwaveProductBySku(string expand = default(string), string sku = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveProducts('{Uri.EscapeDataString(sku.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStreakwaveProductBySku(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>(response);
        }

        partial void OnUpdateStreakwaveProduct(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStreakwaveProduct(string sku = default(string), CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct streakwaveProduct = default(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct))
        {
            var uri = new Uri(baseUri, $"StreakwaveProducts('{Uri.EscapeDataString(sku.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(streakwaveProduct), Encoding.UTF8, "application/json");

            OnUpdateStreakwaveProduct(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveProductEnrichmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveProductEnrichmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStreakwaveProductEnrichments(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>> GetStreakwaveProductEnrichments(Query query)
        {
            return await GetStreakwaveProductEnrichments(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>> GetStreakwaveProductEnrichments(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveProductEnrichments");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStreakwaveProductEnrichments(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>>(response);
        }

        partial void OnCreateStreakwaveProductEnrichment(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> CreateStreakwaveProductEnrichment(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment streakwaveProductEnrichment = default(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment))
        {
            var uri = new Uri(baseUri, $"StreakwaveProductEnrichments");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(streakwaveProductEnrichment), Encoding.UTF8, "application/json");

            OnCreateStreakwaveProductEnrichment(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>(response);
        }

        partial void OnDeleteStreakwaveProductEnrichment(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStreakwaveProductEnrichment(string sku = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveProductEnrichments('{Uri.EscapeDataString(sku.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStreakwaveProductEnrichment(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStreakwaveProductEnrichmentBySku(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> GetStreakwaveProductEnrichmentBySku(string expand = default(string), string sku = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveProductEnrichments('{Uri.EscapeDataString(sku.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStreakwaveProductEnrichmentBySku(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>(response);
        }

        partial void OnUpdateStreakwaveProductEnrichment(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStreakwaveProductEnrichment(string sku = default(string), CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment streakwaveProductEnrichment = default(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment))
        {
            var uri = new Uri(baseUri, $"StreakwaveProductEnrichments('{Uri.EscapeDataString(sku.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(streakwaveProductEnrichment), Encoding.UTF8, "application/json");

            OnUpdateStreakwaveProductEnrichment(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveSyncRulesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwavesyncrules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwavesyncrules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveSyncRulesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwavesyncrules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwavesyncrules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStreakwaveSyncRules(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>> GetStreakwaveSyncRules(Query query)
        {
            return await GetStreakwaveSyncRules(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>> GetStreakwaveSyncRules(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveSyncRules");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStreakwaveSyncRules(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>>(response);
        }

        partial void OnCreateStreakwaveSyncRule(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> CreateStreakwaveSyncRule(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule streakwaveSyncRule = default(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule))
        {
            var uri = new Uri(baseUri, $"StreakwaveSyncRules");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(streakwaveSyncRule), Encoding.UTF8, "application/json");

            OnCreateStreakwaveSyncRule(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>(response);
        }

        partial void OnDeleteStreakwaveSyncRule(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStreakwaveSyncRule(int ruleId = default(int))
        {
            var uri = new Uri(baseUri, $"StreakwaveSyncRules({ruleId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStreakwaveSyncRule(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStreakwaveSyncRuleByRuleId(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> GetStreakwaveSyncRuleByRuleId(string expand = default(string), int ruleId = default(int))
        {
            var uri = new Uri(baseUri, $"StreakwaveSyncRules({ruleId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStreakwaveSyncRuleByRuleId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>(response);
        }

        partial void OnUpdateStreakwaveSyncRule(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStreakwaveSyncRule(int ruleId = default(int), CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule streakwaveSyncRule = default(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule))
        {
            var uri = new Uri(baseUri, $"StreakwaveSyncRules({ruleId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(streakwaveSyncRule), Encoding.UTF8, "application/json");

            OnUpdateStreakwaveSyncRule(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveWarehouseStocksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwavewarehousestocks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwavewarehousestocks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveWarehouseStocksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwavewarehousestocks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwavewarehousestocks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStreakwaveWarehouseStocks(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>> GetStreakwaveWarehouseStocks(Query query)
        {
            return await GetStreakwaveWarehouseStocks(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>> GetStreakwaveWarehouseStocks(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveWarehouseStocks");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStreakwaveWarehouseStocks(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>>(response);
        }

        partial void OnCreateStreakwaveWarehouseStock(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> CreateStreakwaveWarehouseStock(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock streakwaveWarehouseStock = default(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock))
        {
            var uri = new Uri(baseUri, $"StreakwaveWarehouseStocks");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(streakwaveWarehouseStock), Encoding.UTF8, "application/json");

            OnCreateStreakwaveWarehouseStock(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>(response);
        }

        partial void OnDeleteStreakwaveWarehouseStock(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStreakwaveWarehouseStock(string sku = default(string), string warehouseCode = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveWarehouseStocks(Sku='{Uri.EscapeDataString(sku.Trim().Replace("'", "''"))}',WarehouseCode='{Uri.EscapeDataString(warehouseCode.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStreakwaveWarehouseStock(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStreakwaveWarehouseStockBySkuAndWarehouseCode(HttpRequestMessage requestMessage);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> GetStreakwaveWarehouseStockBySkuAndWarehouseCode(string expand = default(string), string sku = default(string), string warehouseCode = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveWarehouseStocks(Sku='{Uri.EscapeDataString(sku.Trim().Replace("'", "''"))}',WarehouseCode='{Uri.EscapeDataString(warehouseCode.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStreakwaveWarehouseStockBySkuAndWarehouseCode(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>(response);
        }

        partial void OnUpdateStreakwaveWarehouseStock(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStreakwaveWarehouseStock(string sku = default(string), string warehouseCode = default(string), CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock streakwaveWarehouseStock = default(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock))
        {
            var uri = new Uri(baseUri, $"StreakwaveWarehouseStocks(Sku='{Uri.EscapeDataString(sku.Trim().Replace("'", "''"))}',WarehouseCode='{Uri.EscapeDataString(warehouseCode.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(streakwaveWarehouseStock), Encoding.UTF8, "application/json");

            OnUpdateStreakwaveWarehouseStock(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportIngramProductsForQuotingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductsforquotings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductsforquotings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportIngramProductsForQuotingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductsforquotings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductsforquotings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetIngramProductsForQuotings(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>> GetIngramProductsForQuotings(Query query)
        {
            return await GetIngramProductsForQuotings(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>> GetIngramProductsForQuotings(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"IngramProductsForQuotings");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramProductsForQuotings(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>>(response);
        }

        public async System.Threading.Tasks.Task ExportIngramProductsWithDetailsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductswithdetails/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductswithdetails/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportIngramProductsWithDetailsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductswithdetails/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductswithdetails/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetIngramProductsWithDetails(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>> GetIngramProductsWithDetails(Query query)
        {
            return await GetIngramProductsWithDetails(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>> GetIngramProductsWithDetails(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"IngramProductsWithDetails");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetIngramProductsWithDetails(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>>(response);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefapprovedSkuImagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefapprovedskuimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefapprovedskuimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefapprovedSkuImagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefapprovedskuimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefapprovedskuimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLenovoPsrefapprovedSkuImages(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefapprovedSkuImage>> GetLenovoPsrefapprovedSkuImages(Query query)
        {
            return await GetLenovoPsrefapprovedSkuImages(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefapprovedSkuImage>> GetLenovoPsrefapprovedSkuImages(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefapprovedSkuImages");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefapprovedSkuImages(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefapprovedSkuImage>>(response);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefcurrentFamiliesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefcurrentfamilies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefcurrentfamilies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefcurrentFamiliesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefcurrentfamilies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefcurrentfamilies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLenovoPsrefcurrentFamilies(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefcurrentFamily>> GetLenovoPsrefcurrentFamilies(Query query)
        {
            return await GetLenovoPsrefcurrentFamilies(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefcurrentFamily>> GetLenovoPsrefcurrentFamilies(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefcurrentFamilies");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefcurrentFamilies(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefcurrentFamily>>(response);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefimageStorageSummariesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefimagestoragesummaries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefimagestoragesummaries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefimageStorageSummariesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefimagestoragesummaries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefimagestoragesummaries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLenovoPsrefimageStorageSummaries(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageStorageSummary>> GetLenovoPsrefimageStorageSummaries(Query query)
        {
            return await GetLenovoPsrefimageStorageSummaries(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageStorageSummary>> GetLenovoPsrefimageStorageSummaries(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefimageStorageSummaries");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefimageStorageSummaries(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageStorageSummary>>(response);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefunmatchedSkusToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefunmatchedskus/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefunmatchedskus/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLenovoPsrefunmatchedSkusToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefunmatchedskus/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefunmatchedskus/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLenovoPsrefunmatchedSkus(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefunmatchedSku>> GetLenovoPsrefunmatchedSkus(Query query)
        {
            return await GetLenovoPsrefunmatchedSkus(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefunmatchedSku>> GetLenovoPsrefunmatchedSkus(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"LenovoPsrefunmatchedSkus");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLenovoPsrefunmatchedSkus(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefunmatchedSku>>(response);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveProductsForQuotingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductsforquotings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductsforquotings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveProductsForQuotingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductsforquotings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductsforquotings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStreakwaveProductsForQuotings(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>> GetStreakwaveProductsForQuotings(Query query)
        {
            return await GetStreakwaveProductsForQuotings(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>> GetStreakwaveProductsForQuotings(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveProductsForQuotings");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStreakwaveProductsForQuotings(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>>(response);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveProductsWithEnrichmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductswithenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductswithenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStreakwaveProductsWithEnrichmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductswithenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductswithenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStreakwaveProductsWithEnrichments(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>> GetStreakwaveProductsWithEnrichments(Query query)
        {
            return await GetStreakwaveProductsWithEnrichments(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>> GetStreakwaveProductsWithEnrichments(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"StreakwaveProductsWithEnrichments");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStreakwaveProductsWithEnrichments(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>>(response);
        }
    }
}