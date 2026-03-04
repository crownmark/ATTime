using CrownATTime.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using static CrownATTime.Server.Models.ITGlueDocumentsResult;

namespace CrownATTime.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ITGlueController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpClient _http;
        private readonly IServiceScopeFactory _scopeFactory;

        public ITGlueController()
        {
            _http = new HttpClient();
        }

        //GET /organizations/:organization_id/relationships/documents
        [HttpGet("Organization/Documents/ByOrgId/{orgId}")]
        public async Task<IActionResult> GetITGlueDocumentsByOrganizationId(string orgId)
        {
            try
            {
                const int pageSize = 100; // or 50, depending on IT Glue limits
                int pageNumber = 1;
                var documentsList = new List<ITGlueDocumentAttributesResults>();
                while (true)
                {
                    var url = $"https://api.itglue.com/organizations/{orgId}/relationships/documents?page[size]={pageSize}&page[number]={pageNumber}";

                    using var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("X-API-Key", "ITG.8b48231efe864f6b6ecd16b026dddd18.TzgHc7tB2trFmrR4dcYtrYsZ3pfuhFZmcTueD80fdpF-CHwroNQh-mdx7EL28w34");
                    request.Headers.Add("Accept", "application/json");

                    using var response = await _http.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        return StatusCode((int)response.StatusCode,
                            await response.Content.ReadAsStringAsync());
                    }
                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonSerializer.Deserialize<ITGlueDocumentsResult>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (result == null)
                        break;
                    foreach (var document in result.data)
                    {
                        documentsList.Add(new ITGlueDocumentAttributesResults()
                        {
                            Id = document.id,
                            Archived = document.attributes.Archived,
                            DocumentFolderId = document.attributes.DocumentFolderId,
                            Name = document.attributes.Name,
                            OrganizationId = document.attributes.OrganizationId,
                            OrganizationName = document.attributes.OrganizationName,
                            ResourceUrl = document.attributes.ResourceUrl,

                        });
                    }

                    // Stop when less than page size returned
                    if (result.links.next == null)
                        break;

                    // Go to next page
                    pageNumber++;
                }
                // No match found in any page
                return Ok(documentsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("Organization/ByCompanyName/{companyName}")]
        public async Task<IActionResult> GetITGlueOrganizationIdByCompanyNameAsync(string companyName)
        {
            try
            {
                const int pageSize = 100; // or 50, depending on IT Glue limits
                int pageNumber = 1;
                while (true)
                {
                    var url = $"https://api.itglue.com/organizations?page[size]={pageSize}&page[number]={pageNumber}";

                    using var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("X-API-Key", "ITG.8b48231efe864f6b6ecd16b026dddd18.TzgHc7tB2trFmrR4dcYtrYsZ3pfuhFZmcTueD80fdpF-CHwroNQh-mdx7EL28w34");
                    request.Headers.Add("Accept", "application/json");

                    using var response = await _http.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    using var json = JsonDocument.Parse(content);

                    // If there’s no "data" or it's empty, we’re done
                    if (!json.RootElement.TryGetProperty("data", out var dataArray) ||
                        dataArray.ValueKind != JsonValueKind.Array ||
                        dataArray.GetArrayLength() == 0)
                    {
                        break;
                    }

                    // Look for a match on this page
                    var match = dataArray
                        .EnumerateArray()
                        .FirstOrDefault(org =>
                        {
                            var attributes = org.GetProperty("attributes");
                            var name = attributes.GetProperty("name").GetString();
                            return name?.Equals(companyName, StringComparison.OrdinalIgnoreCase) == true;
                        });

                    if (match.ValueKind != JsonValueKind.Undefined)
                    {
                        return Ok(match.GetProperty("id").GetString());
                    }

                    // Go to next page
                    pageNumber++;

                }

                return Ok("");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("Organization/Passwords/ByOrgId/{organizationId}")]
        public async Task<IActionResult> GetITGlueOrganizationPasswordsAsync(string organizationId)
        {
            try
            {
                const int pageSize = 100; // or 50, depending on IT Glue limits
                int pageNumber = 1;
                var passwordList = new List<ITGluePasswordAttributeResults>();
                while (true)
                {
                    var url = $"https://api.itglue.com/organizations/{organizationId}/relationships/passwords?page[size]={pageSize}&page[number]={pageNumber}";

                    using var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("X-API-Key", "ITG.8b48231efe864f6b6ecd16b026dddd18.TzgHc7tB2trFmrR4dcYtrYsZ3pfuhFZmcTueD80fdpF-CHwroNQh-mdx7EL28w34");
                    request.Headers.Add("Accept", "application/json");

                    using var response = await _http.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        return StatusCode((int)response.StatusCode,
                            await response.Content.ReadAsStringAsync());
                    }
                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonSerializer.Deserialize<ITGluePasswordResults>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (result == null)
                        break;
                    foreach (var password in result.data)
                    {
                        passwordList.Add(new ITGluePasswordAttributeResults()
                        {
                            id = password.id,
                            type = password.type,
                            organizationid = password.attributes.organizationid,
                            organizationname = password.attributes.organizationname,
                            resourceurl = password.attributes.resourceurl,
                            restricted = password.attributes.restricted,
                            myglue = password.attributes.myglue,
                            name = password.attributes.name,
                            autofillselectors = password.attributes.autofillselectors,
                            username = password.attributes.username,
                            url = password.attributes.url,
                            notes = password.attributes.notes,
                            passwordupdatedat = password.attributes.passwordupdatedat,
                            updatedby = password.attributes.updatedby,
                            resourceid = password.attributes.resourceid,
                            resourcetype = password.attributes.resourcetype,
                            cachedresourcetypename = password.attributes.cachedresourcetypename,
                            cachedresourcename = password.attributes.cachedresourcename,
                            passwordcategoryid = password.attributes.passwordcategoryid,
                            passwordcategoryname = password.attributes.passwordcategoryname,
                            createdat = password.attributes.createdat,
                            updatedat = password.attributes.updatedat,
                        });
                    }

                    // Stop when less than page size returned
                    if (result.links.next == null)
                        break;

                    // Go to next page
                    pageNumber++;
                }
                // No match found in any page
                return Ok(passwordList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("Organization/Passwords/ByPasswordId/{organizationId}/{passwordId}")]
        public async Task<IActionResult> GetITGluePasswordByPasswordIdAsync(string organizationId, string passwordId)
        {
            try
            {
                var url = $"https://api.itglue.com/organizations/{organizationId}/relationships/passwords/{passwordId}";

                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("X-API-Key", "ITG.8b48231efe864f6b6ecd16b026dddd18.TzgHc7tB2trFmrR4dcYtrYsZ3pfuhFZmcTueD80fdpF-CHwroNQh-mdx7EL28w34");
                request.Headers.Add("Accept", "application/json");

                using var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode,
                        await response.Content.ReadAsStringAsync());
                }
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ITGluePassword>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

               
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
