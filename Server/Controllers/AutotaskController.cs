namespace CrownATTime.Server.Controllers
{
    using CrownATTime.Server.Models;
    using CrownATTime.Server.Models.ATTime;
    using CrownATTime.Server.Services;
    using Microsoft.AspNetCore.Http.Json;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Logging;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    [ApiController]
    [Route("api/[controller]")]
    public class AutotaskController : ControllerBase
    {
        private readonly HttpClient _http;
        private CrownATTime.Server.Data.ATTimeContext context;

        public AutotaskController(IHttpClientFactory httpClientFactory, CrownATTime.Server.Data.ATTimeContext _context)
        {
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = new Uri("https://webservices5.autotask.net/atservicesrest/v1.0");
            _http.DefaultRequestHeaders.Add("UserName", "bmjnrji6mu6q57m@ce-technology.com");
            _http.DefaultRequestHeaders.Add("Secret", "0Js$n3*ZY@z4#5Bp1o~XmR7#@");
            _http.DefaultRequestHeaders.Add("ApiIntegrationCode", "GGNM7AWDIIZCZWTKY4SF4UGQEDE");
            context = _context;
        }

        [HttpGet("billingcodes/query")]
        public async Task<IActionResult> GetBillingCodes([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/BillingCodes/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching contacts: {ex.Message}");
            }

        }
        [HttpGet("billingcodes/sync")]
        public async Task<IActionResult> SyncBilingCodes([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/BillingCodes/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();
                var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<BillingCodeDto>>(content);
                var existingItems = context.BillingCodeCaches.ToList();
                var itemsToUpdate = new List<BillingCodeCache>();
                var itemsToCreate = new List<BillingCodeCache>();
                foreach (var item in converted.Items)
                {
                    var existingItem = existingItems.FirstOrDefault(x => x.Id == item.id);
                    if (existingItem != null)
                    {
                        existingItem.Id = item.id;
                        existingItem.Department = item.department;
                        existingItem.IsActive = item.isActive;
                        existingItem.AfterHoursWorkType = item.afterHoursWorkType;
                        existingItem.Name = item.name;
                        existingItem.UseType = item.useType;
                        existingItem.BillingCodeType = item.billingCodeType;

                    }
                    else
                    {
                        itemsToCreate.Add(new BillingCodeCache()
                        {
                            Id = item.id,
                            Department = item.department,
                            IsActive = item.isActive,
                            AfterHoursWorkType = item.afterHoursWorkType,
                            Name = item.name,
                            UseType = item.useType,
                            BillingCodeType = item.billingCodeType,

                        });

                    }
                }
                await context.AddRangeAsync(itemsToCreate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching Billing Codes: {ex.Message}");
            }

        }

        [HttpGet("contracts/query")]
        public async Task<IActionResult> GetContracts([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/Contracts/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching contacts: {ex.Message}");
            }

        }
        [HttpGet("contracts/sync")]
        public async Task<IActionResult> SyncContracts([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/Contracts/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();
                var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<ContractDto>>(content);
                var existingItems = context.ContractCaches.ToList();
                var itemsToUpdate = new List<ContractCache>();
                var itemsToCreate = new List<ContractCache>();
                foreach (var item in converted.Items)
                {
                    var existingItem = existingItems.FirstOrDefault(x => x.Id == item.Id);
                    if (existingItem != null)
                    {
                        existingItem.Id = item.Id;
                        existingItem.Status = item.Status;
                        existingItem.IsDefaultContract = item.IsDefaultContract;
                        existingItem.ContractName = item.ContractName;
                        existingItem.BillingPreference = item.BillingPreference;
                        existingItem.CompanyId = Convert.ToInt32(item.CompanyID);
                        
                    }
                    else
                    {
                        itemsToCreate.Add(new ContractCache() 
                        { 
                            Id = item.Id,
                            BillingPreference = item.BillingPreference,
                            Status = item.Status,
                            ContractName = item.ContractName,
                            CompanyId = Convert.ToInt32(item.CompanyID),
                            IsDefaultContract = item.IsDefaultContract,
                        });

                    }
                }
                await context.AddRangeAsync( itemsToCreate );
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching contacts: {ex.Message}");
            }

        }

        [HttpGet("contractexclusionbillingcodes/query")]
        public async Task<IActionResult> GetContractExclusionBillingCode([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/ContractExclusionBillingCodes/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching contacts: {ex.Message}");
            }

        }

        [HttpGet("contacts/query")]
        public async Task<IActionResult> GetContacts([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/Contacts/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching contacts: {ex.Message}");
            }

        }

        // GET: api/autotask/tickets/123
        [HttpGet("contacts/{id:long}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var response = await _http.GetAsync($"v1.0/Contacts/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Content(content, "application/json");
            }

            return StatusCode((int)response.StatusCode, content);
        }

        // GET: api/autotask/tickets/123
        [HttpGet("contracts/{id:long}")]
        public async Task<IActionResult> GetContractById(int id)
        {
            var response = await _http.GetAsync($"v1.0/Contracts/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Content(content, "application/json");
            }

            return StatusCode((int)response.StatusCode, content);
        }
        // GET: api/autotask/tickets/123
        [HttpGet("companies/{id:long}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var response = await _http.GetAsync($"v1.0/Companies/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Content(content, "application/json");
            }

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpGet("companies/query")]
        public async Task<IActionResult> GetCompanies([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/Companies/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching companies: {ex.Message}");
            }

        }

        [HttpGet("resources/query")]
        public async Task<IActionResult> GetResources([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/Resources/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching resources: {ex.Message}");
            }

        }

        [HttpGet("resources/sync")]
        public async Task<IActionResult> SyncResources([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/Resources/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();
                var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<ResourceDtoResult>>(content);
                var existingItems = context.ResourceCaches.ToList();
                var itemsToUpdate = new List<ResourceCache>();
                var itemsToCreate = new List<ResourceCache>();
                foreach (var item in converted.Items)
                {
                    var existingItem = existingItems.FirstOrDefault(x => x.Id == item.id);
                    if (existingItem != null)
                    {
                        existingItem.Id = item.id;
                        existingItem.OfficeExtension = item.officeExtension;
                        existingItem.IsActive = item.isActive;
                        existingItem.OfficePhone = item.officePhone;
                        existingItem.Email = item.email;
                        existingItem.FirstName = item.firstName;
                        existingItem.LastName = item.lastName;
                        existingItem.ResourceType = item.resourceType;
                        existingItem.UserName = item.userName;
                    }
                    else
                    {
                        itemsToCreate.Add(new ResourceCache()
                        {
                            Id = item.id,
                            OfficePhone = item.officePhone,
                            OfficeExtension = item.officeExtension,
                            IsActive = item.isActive,
                            Email = item.email,
                            FirstName = item.firstName, 
                            LastName = item.lastName,
                            ResourceType = item.resourceType,
                            UserName = item.userName,

                        });

                    }
                }
                await context.AddRangeAsync(itemsToCreate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching Roles: {ex.Message}");
            }

        }

        [HttpGet("roles/query")]
        public async Task<IActionResult> GetRoles([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/Roles/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching contacts: {ex.Message}");
            }

        }

        [HttpGet("roles/sync")]
        public async Task<IActionResult> SyncRoles([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/Roles/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();
                var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<RoleDto>>(content);
                var existingItems = context.RoleCaches.ToList();
                var itemsToUpdate = new List<RoleCache>();
                var itemsToCreate = new List<RoleCache>();
                foreach (var item in converted.Items)
                {
                    var existingItem = existingItems.FirstOrDefault(x => x.Id == item.id);
                    if (existingItem != null)
                    {
                        existingItem.Id = item.id;
                        existingItem.Name = item.name;
                        existingItem.IsActive = item.isActive;
                        existingItem.RoleType = item.roleType;
                    }
                    else
                    {
                        itemsToCreate.Add(new RoleCache()
                        {
                            Id = item.id,
                            Name = item.name,
                            IsActive = item.isActive,
                            RoleType = item.roleType,

                        });

                    }
                }
                await context.AddRangeAsync(itemsToCreate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching Roles: {ex.Message}");
            }

        }

        [HttpGet("servicedeskroles/query")]
        public async Task<IActionResult> GetServiceDeskRoles([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/ResourceServiceDeskRoles/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching contacts: {ex.Message}");
            }

        }

        [HttpGet("servicedeskroles/sync")]
        public async Task<IActionResult> SyncServiceDeskRoles([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/ResourceServiceDeskRoles/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();
                var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<SericeDeskRoleDto>>(content);
                var existingItems = context.ServiceDeskRoleCaches.ToList();
                var itemsToUpdate = new List<ServiceDeskRoleCache>();
                var itemsToCreate = new List<ServiceDeskRoleCache>();
                foreach (var item in converted.Items)
                {
                    var existingItem = existingItems.FirstOrDefault(x => x.Id == item.id);
                    if (existingItem != null)
                    {
                        existingItem.Id = item.id;
                        existingItem.IsActive = item.isActive;
                        existingItem.IsDefault = item.isDefault;
                        existingItem.ResourceId = item.resourceID;
                        existingItem.RoleId = item.roleID;
                    }
                    else
                    {
                        itemsToCreate.Add(new ServiceDeskRoleCache()
                        {
                            Id = item.id,
                            IsActive = item.isActive,
                            IsDefault = item.isDefault,
                            ResourceId = item.resourceID,
                            RoleId = item.roleID,

                        });

                    }
                }
                await context.AddRangeAsync(itemsToCreate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching Service Desk Roles: {ex.Message}");
            }

        }

        [HttpGet("tickets/fields")]
        public async Task<IActionResult> GetTicketFields()
        {
            var resp = await _http.GetAsync("v1.0/Tickets/entityInformation/fields");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpGet("tickets/fields/sync")]
        public async Task<IActionResult> SyncTicketPicklists()
        {
            try
            {
                
                var response = await _http.GetAsync($"v1.0/Tickets/entityInformation/fields");
                var content = await response.Content.ReadAsStringAsync();
                var converted = JsonSerializer.Deserialize<TicketEntityFieldsDto.EntityInformationFieldsResponse>(content);
                var existingItems = context.TicketEntityPicklistValueCaches.ToList();
                var itemsToUpdate = new List<TicketEntityPicklistValueCache>();
                var itemsToCreate = new List<TicketEntityPicklistValueCache>();
                foreach(var field in converted.Fields)
                {
                    if (field.PicklistValues != null)
                    {
                        foreach (var item in field.PicklistValues)
                        {
                            var existingItem = existingItems.FirstOrDefault(x => x.PicklistName == field.Name && x.Label == item.Label);
                            if (existingItem != null)
                            {
                                existingItem.PicklistName = field.Name;
                                existingItem.Label = item.Label;
                                existingItem.Value = item.Value;
                                existingItem.ValueInt = item.ValueInt.HasValue ? Convert.ToInt32(item.ValueInt) : null;

                            }
                            else
                            {
                                itemsToCreate.Add(new TicketEntityPicklistValueCache()
                                {
                                    PicklistName = field.Name,
                                    Label = item.Label,
                                    Value = item.Value,
                                    ValueInt = item.ValueInt,
                                });

                            }
                        }
                    }
                }
                
                await context.AddRangeAsync(itemsToCreate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching contacts: {ex.Message}");
            }

        }

        [HttpGet("userdefinefields")]
        public async Task<IActionResult> GetTicketUserDefineFields()
        {
            var resp = await _http.GetAsync("v1.0/Tickets/entityInformation/userDefinedFields");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }



        // GET: api/autotask/tickets/123
        [HttpGet("tickets/{id:long}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var response = await _http.GetAsync($"v1.0/Tickets/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Content(content, "application/json");
            }

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpPatch("tickets")]
        public async Task<IActionResult> UpdateTicket([FromBody] TicketUpdateDto ticket)
        {
            try
            {
                if (ticket.Id <= 0)
                {
                    return BadRequest("Missing or invalid ticket ID.");
                }

                var json = JsonSerializer.Serialize(ticket, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PatchAsync("v1.0/Tickets", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating ticket: {ex.Message}");
            }
        }

        // POST api/autotask/timeentries
        [HttpPost("tickets")]
        public async Task<IActionResult> CreateTicket([FromBody] TicketDto ticket)
        {
            try
            {
                var json = JsonSerializer.Serialize(ticket, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync("v1.0/Tickets", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating ticket: {ex.Message}");
            }
        }

        [HttpGet("timeentries/fields")]
        public async Task<IActionResult> GetTimeEntryFields()
        {
            var resp = await _http.GetAsync("v1.0/TimeEntries/entityInformation/fields");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // POST api/autotask/timeentries
        [HttpPost("timeentries")]
        public async Task<IActionResult> CreateTimeEntry([FromBody] TimeEntryCreateDto timeEntry)
        {
            try
            {
                var json = JsonSerializer.Serialize(timeEntry, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync("v1.0/TimeEntries", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating timeentry: {ex.Message}");
            }
        }

    }
}
