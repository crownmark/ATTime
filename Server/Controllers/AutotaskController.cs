namespace CrownATTime.Server.Controllers
{
    using CrownATTime.Client;
    using CrownATTime.Server.Models;
    using CrownATTime.Server.Models.ATTime;
    using CrownATTime.Server.Services;
    using DocumentFormat.OpenXml.Office2010.Excel;
    using Microsoft.AspNetCore.Http.Json;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Graph.Models;
    using Microsoft.Graph.Models.Security;
    using Microsoft.IdentityModel.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using static CrownATTime.Server.Models.ITGlueDocumentsResult;

    [ApiController]
    [Route("api/[controller]")]
    public class AutotaskController : ControllerBase
    {
        private readonly HttpClient _http;
        private CrownATTime.Server.Data.ATTimeContext context;

        public AutotaskController(IHttpClientFactory httpClientFactory, IConfiguration configuration, CrownATTime.Server.Data.ATTimeContext _context)
        {
            var baseUrl = configuration["Autotask:BaseUrl"];
            var userName = configuration["Autotask:UserName"];
            var secret = configuration["Autotask:Secret"];
            var apiCode = configuration["Autotask:ApiIntegrationCode"];

            _http = httpClientFactory.CreateClient();
            
            _http.BaseAddress = new Uri(baseUrl!);
            _http.DefaultRequestHeaders.Add("UserName", userName);
            _http.DefaultRequestHeaders.Add("Secret", secret);
            _http.DefaultRequestHeaders.Add("ApiIntegrationCode", apiCode);

            context = _context;
        }

        [HttpGet("AccountAlerts/{id}")]
        public async Task<IActionResult> GetAccountAlertsByCompanyId(int id)
        {
            try
            {

                var response = await _http.GetAsync($"v1.0/Companies/{id}/Alerts");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching Account Alerts item: {ex.Message}");
            }

        }
        [HttpPost("ticketattachments")]
        public async Task<IActionResult> CreateAttacmentItem([FromBody] AttachmentCreateDto attachment)
        {
            try
            {
                var json = JsonSerializer.Serialize(attachment, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var request = new HttpRequestMessage(HttpMethod.Post, $"v1.0/Tickets/{attachment.ticketID}/Attachments");

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                // 🔑 Autotask impersonation header
                request.Headers.TryAddWithoutValidation("ImpersonationResourceId", attachment.attachedByResourceID.ToString());

                var response = await _http.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating attachment item: {ex.Message}");
            }
        }

        [HttpDelete("ticketattachments")]
        public async Task<IActionResult> DeleteAttachmentItem([FromBody] AttachmentDtoResult item)
        {
            try
            {
                if (item.id <= 0)
                {
                    return BadRequest("Missing or invalid ID.");
                }


                var response = await _http.DeleteAsync($"v1.0/Tickets/{item.ticketID}/Attachments/{item.id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting attachment item: {ex.Message}");
            }
        }

        [HttpGet("ticketattachments/{ticketId}")]
        public async Task<IActionResult> GetTicketAttachments(int ticketId)
        {
            try
            {

                var response = await _http.GetAsync($"v1.0/Tickets/{ticketId}/Attachments");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching Ticket Attachments: {ex.Message}");
            }
        }

        [HttpGet("appointments/query")]
        public async Task<IActionResult> GetAppointments([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/Appointments/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching appointments: {ex.Message}");
            }

        }
        [HttpGet("actionTypes/query")]
        public async Task<IActionResult> GetActionTypes([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/ActionTypes/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching entries: {ex.Message}");
            }

        }
        // POST api/autotask/timeentries
        [HttpPost("companyToDos")]
        public async Task<IActionResult> CreateTicket([FromBody] CompanyTodoDto todo)
        {
            try
            {
                var json = JsonSerializer.Serialize(todo, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync("v1.0/CompanyToDos", content);

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

        [HttpPatch("companyToDos")]
        public async Task<IActionResult> UpdateCompanyTodo([FromBody] CompanyToDoCreate todo)
        {
            try
            {
                if (todo.id <= 0)
                {
                    return BadRequest("Missing or invalid ticket ID.");
                }

                var json = JsonSerializer.Serialize(todo, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PatchAsync($"v1.0/Companies/{todo.companyID}/ToDos", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating todo: {ex.Message}");
            }
        }

        [HttpGet("companyToDos/query")]
        public async Task<IActionResult> GetCompanyToDos([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/CompanyToDos/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching entries: {ex.Message}");
            }

        }

        // POST api/autotask/timeentries
        [HttpPost("servicecalls")]
        public async Task<IActionResult> CreateServiceCall([FromBody] ServiceCallCreateDto serviceCall)
        {
            try
            {
                var json = JsonSerializer.Serialize(serviceCall, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync("v1.0/ServiceCalls", content);

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

        [HttpPost("servicecallTicket")]
        public async Task<IActionResult> CreateServiceCallTicket([FromBody] ServiceCallTicketCreate item)
        {
            try
            {
                var json = JsonSerializer.Serialize(item, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"v1.0/ServiceCalls/{item.serviceCallID}/Tickets", content);

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

        [HttpPost("servicecallTicketResource")]
        public async Task<IActionResult> CreateServiceCallResource([FromBody] ServiceCallTicketResourceCreate item)
        {
            try
            {
                var json = JsonSerializer.Serialize(item, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"v1.0/ServiceCalls/{item.serviceCallTicketID}/Resources", content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating service call resource: {ex.Message}");
            }
        }

        [HttpPatch("servicecalls")]
        public async Task<IActionResult> UpdateServiceCall([FromBody] ServiceCallCreateDto serviceCall)
        {
            try
            {
                if (serviceCall.id <= 0)
                {
                    return BadRequest("Missing or invalid ticket ID.");
                }
                //serviceCall.startDateTime.ToDateTimeTimeZone();
                //serviceCall.endDateTime.ToDateTimeTimeZone();
                var json = JsonSerializer.Serialize(serviceCall, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PatchAsync("v1.0/ServiceCalls", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating service call: {ex.Message}");
            }
        }

        [HttpGet("servicecalls/query")]
        public async Task<IActionResult> GetServiceCalls([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/ServiceCalls/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching time entries: {ex.Message}");
            }

        }

        [HttpGet("servicecallTicketResources/query")]
        public async Task<IActionResult> GetTicketResourceServiceCalls([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/ServiceCallTicketResources/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();
                var ticketResourceServiceCalls = JsonSerializer.Deserialize<AutotaskItemsResponse<ServiceCallTicketResource>>(content);


                //Get ServiceCallTicketResources

                var serviceCallTicketIds = ticketResourceServiceCalls.Items.Select(x => x.serviceCallTicketID).ToArray();


                var ServiceCallTicketFilter = new List<object>
                {
                    new { op = "in", field = "id", value = serviceCallTicketIds },
                };
                var scTicketResourceObject = new
                {
                    filter = ServiceCallTicketFilter,
                    MaxRecords = 500
                };

                var serviceCallTicketSearch = JsonSerializer.Serialize(scTicketResourceObject);
                var serviceCallTicketencodedSearch = Uri.EscapeDataString(serviceCallTicketSearch);
                var serviceCallTicketresponse = await _http.GetAsync($"v1.0/ServiceCallTickets/query?search={serviceCallTicketencodedSearch}");

                var serviceCallTicketcontent = await serviceCallTicketresponse.Content.ReadAsStringAsync();
                var serviceCallTickets = JsonSerializer.Deserialize<AutotaskItemsResponse<ServiceCallTicket>>(serviceCallTicketcontent);

                //Get Service Calls
                var serviceCallIds = serviceCallTickets.Items.Select(x => x.serviceCallID).ToArray();

                var ServiceCallFilter = new List<object>
                {
                    new { op = "in", field = "id", value = serviceCallIds },
                };
                var scObject = new
                {
                    filter = ServiceCallFilter,
                    MaxRecords = 500
                };

                var serviceCallSearch = JsonSerializer.Serialize(scObject);
                var serviceCallencodedSearch = Uri.EscapeDataString(serviceCallSearch);
                var serviceCallresponse = await _http.GetAsync($"v1.0/ServiceCalls/query?search={serviceCallencodedSearch}");

                var serviceCallcontent = await serviceCallTicketresponse.Content.ReadAsStringAsync();
                var serviceCalls = JsonSerializer.Deserialize<AutotaskItemsResponse<ServiceCall>>(serviceCallcontent);


                // ----------------------------------------------------
                // Build lookup: serviceCallID → ticketID
                // ----------------------------------------------------
                var lookupServiceCall = serviceCallTickets.Items
                    .GroupBy(x => x.serviceCallID)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ticketID).FirstOrDefault() // or ToList() if needed
                    );


                // ----------------------------------------------------
                // Merge TicketId into ServiceCalls
                // ----------------------------------------------------
                var resultItems = serviceCalls.Items.Where(x => x.isComplete == 0).Select(sc => new ServiceCall
                {
                    id = sc.id,
                    companyID = sc.companyID,
                    companyLocationID = sc.companyLocationID,
                    createDateTime = sc.createDateTime,
                    creatorResourceID = sc.creatorResourceID,
                    description = sc.description,
                    duration = sc.duration,
                    endDateTime = sc.endDateTime,
                    impersonatorCreatorResourceID = sc.impersonatorCreatorResourceID,
                    isComplete = sc.isComplete,
                    lastModifiedDateTime = sc.lastModifiedDateTime,
                    startDateTime = sc.startDateTime,
                    status = sc.status,

                    // copy other properties as needed...

                    ticketId = lookupServiceCall.ContainsKey(sc.id)
                        ? lookupServiceCall[sc.id]
                        : null,
                }).ToList();

                var result = new AutotaskItemsResponse<ServiceCall>
                {
                    Items = resultItems,
                    PageDetails = serviceCalls.PageDetails
                };


                return Content(JsonSerializer.Serialize(result), "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching service calls: {ex.Message}");
            }

        }

        [HttpGet("serviceCallTickets/query")]
        public async Task<IActionResult> GetServiceCallTickets([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/ServiceCallTickets/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();
                var ticketServiceCalls = JsonSerializer.Deserialize<AutotaskItemsResponse<ServiceCallTicket>>(content);

                // ----------------------------------------------------
                // Build lookup: serviceCallID → ticketID
                // ----------------------------------------------------
                var lookupServiceCall = ticketServiceCalls.Items
                    .GroupBy(x => x.serviceCallID)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ticketID).FirstOrDefault() // or ToList() if needed
                    );

                var serviceCallIds = ticketServiceCalls.Items.Select(x => x.serviceCallID).ToArray();

                //Get ServiceCallTicketResources
                
                var serviceCallTicketIds = ticketServiceCalls.Items.Select(x => x.id).ToArray();

                
                var ServiceCallTicketResourceFilter = new List<object>
                {
                    new { op = "in", field = "serviceCallTicketID", value = serviceCallTicketIds },
                };
                var scTicketResourceObject = new
                {
                    filter = ServiceCallTicketResourceFilter,
                    MaxRecords = 500
                };
                //Errors Here
                var serviceCallTicketResourcecurrentSearch = JsonSerializer.Serialize(scTicketResourceObject);
                var serviceCallTicketResourceencodedSearch = Uri.EscapeDataString(serviceCallTicketResourcecurrentSearch);
                var serviceCallTicketResourceresponse = await _http.GetAsync($"v1.0/ServiceCallTicketResources/query?search={serviceCallTicketResourceencodedSearch}");

                var serviceCallTicketResourcecontent = await serviceCallTicketResourceresponse.Content.ReadAsStringAsync();
                var serviceCallTicketResources = JsonSerializer.Deserialize<AutotaskItemsResponse<ServiceCallTicketResource>>(serviceCallTicketResourcecontent);

                // ----------------------------------------------------
                // Build lookup: serviceCallTicketId > ServiceCallId
                // ----------------------------------------------------
                var serviceCallTicketAndResourceLookup = new List<ServiceCallTicketAndResource>();


                
                foreach (var item in ticketServiceCalls.Items)
                {
                    serviceCallTicketAndResourceLookup.Add(new ServiceCallTicketAndResource()
                    {
                        ticketID = item.ticketID,
                        serviceCallID = item.serviceCallID,
                        serviceCallTicketID = item.id
                    });
                }
                foreach (var item in serviceCallTicketResources.Items)
                {
                    var serviceCallsList = serviceCallTicketAndResourceLookup.Where(x => x.serviceCallTicketID == item.serviceCallTicketID).FirstOrDefault();
                    if(serviceCallsList != null)
                    {
                        serviceCallsList.resourceID = item.resourceID;
                        serviceCallsList.serviceCallTicketResourceID = item.id;
                    }
                }

                var lookupServiceResource =
                    (from sct in ticketServiceCalls.Items
                     join sctr in serviceCallTicketResources.Items
                         on sct.id equals sctr.serviceCallTicketID
                     select new ServiceCallTicketAndResource
                     {
                         serviceCallTicketID = sct.id,
                         serviceCallID = sct.serviceCallID,
                         ticketID = sct.ticketID,

                         serviceCallTicketResourceID = sctr.id,
                         resourceID = sctr.resourceID
                     })
                    .ToList();

                

                //Get Service Calls
                var filters = new List<object>
                {
                    new { op = "in", field = "id", value = serviceCallIds },
                };
                var searchObj = new
                {
                    filter = filters,
                    MaxRecords = 500
                };

                var serviceCallcurrentSearch = JsonSerializer.Serialize(searchObj);
                var serviceCallencodedSearch = Uri.EscapeDataString(serviceCallcurrentSearch);
                var serviceCallresponse = await _http.GetAsync($"v1.0/ServiceCalls/query?search={serviceCallcurrentSearch}");

                var serviceCallcontent = await serviceCallresponse.Content.ReadAsStringAsync();
                var serviceCalls = JsonSerializer.Deserialize<AutotaskItemsResponse<ServiceCall>>(serviceCallcontent);

                // ----------------------------------------------------
                // Merge TicketId into ServiceCalls
                // ----------------------------------------------------
                var resultItems = serviceCalls.Items.Where(x => x.isComplete == 0).Select(sc => new ServiceCall
                {
                    id = sc.id,
                    companyID = sc.companyID,
                    companyLocationID = sc.companyLocationID,
                    createDateTime = sc.createDateTime,
                    creatorResourceID = sc.creatorResourceID,
                    description = sc.description,
                    duration = sc.duration,
                    endDateTime = sc.endDateTime,
                    impersonatorCreatorResourceID = sc.impersonatorCreatorResourceID,
                    isComplete = sc.isComplete,
                    lastModifiedDateTime = sc.lastModifiedDateTime,
                    startDateTime = sc.startDateTime,
                    status = sc.status,

                    // copy other properties as needed...

                    //ticketId = lookupServiceCall.ContainsKey(sc.id)
                    //    ? lookupServiceCall[sc.id]
                    //    : null,
                    ticketId = lookupServiceResource.Where(x => x.serviceCallID == sc.id).Any() ? lookupServiceResource.Where(x => x.serviceCallID == sc.id).FirstOrDefault().ticketID : null,
                    assignedToResourceId = lookupServiceResource.Where(x => x.serviceCallID == sc.id).Any() ? lookupServiceResource.Where(x => x.serviceCallID == sc.id).FirstOrDefault().resourceID : 0
                }).ToList();

                var result = new AutotaskItemsResponse<ServiceCall>
                {
                    Items = resultItems,
                    PageDetails = serviceCalls.PageDetails
                };


                return Content(JsonSerializer.Serialize(result), "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching service calls: {ex.Message}");
            }

        }

        [HttpGet("ticketnotes/{ticketId}")]
        public async Task<IActionResult> GetTicketNotes(int ticketId)
        {
            try
            {

                var response = await _http.GetAsync($"v1.0/Tickets/{ticketId}/Notes");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching Ticket Notes: {ex.Message}");
            }
        }

        [HttpGet("tickettimeentries/query")]
        public async Task<IActionResult> GetTimeEntries([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/TimeEntries/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching time entries: {ex.Message}");
            }

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
                        itemsToUpdate.Add(existingItem);

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
                context.UpdateRange(itemsToUpdate);
                await context.AddRangeAsync(itemsToCreate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching Billing Codes: {ex.Message}");
            }

        }
        [HttpGet("ConfigurationItems/{id}")]
        public async Task<IActionResult> GetConfigurationItemById(int id)
        {
            try
            {
                
                var response = await _http.GetAsync($"v1.0/ConfigurationItems/{id}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching configuration item: {ex.Message}");
            }

        }

        [HttpGet("TicketChecklistItem/{ticketId}/{id}")]
        public async Task<IActionResult> GetTicketChecklistItem(int ticketId, int id)
        {
            try
            {

                var response = await _http.GetAsync($"v1.0/Tickets/{ticketId}/ChecklistItems/{id}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching configuration item: {ex.Message}");
            }

        }
        [HttpGet("TicketChecklistItems/query")]
        public async Task<IActionResult> GetTicketChecklistItems([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/TicketChecklistItems/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching contacts: {ex.Message}");
            }

        }
        [HttpPatch("TicketChecklistItems")]
        public async Task<IActionResult> UpdateTicketChecklistItems([FromBody] TicketChecklistItemResult item)
        {
            try
            {
                if (item.id <= 0)
                {
                    return BadRequest("Missing or invalid ID.");
                }

                var json = JsonSerializer.Serialize(item, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var request = new HttpRequestMessage(HttpMethod.Patch, $"v1.0/Tickets/{item.ticketID}/ChecklistItems");
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating checklist item: {ex.Message}");
            }
        }

        [HttpPost("TicketChecklistItems")]
        public async Task<IActionResult> CreateChecklistItem([FromBody] ChecklistItemDto checklistItem)
        {
            try
            {
                var json = JsonSerializer.Serialize(checklistItem, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var request = new HttpRequestMessage(HttpMethod.Post, $"v1.0/Tickets/{checklistItem.ticketID}/ChecklistItems");

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating checklist item: {ex.Message}");
            }
        }

        [HttpDelete("TicketChecklistItems")]
        public async Task<IActionResult> DeleteTicketChecklistItem([FromBody] TicketChecklistItemResult item)
        {
            try
            {
                if (item.id <= 0)
                {
                    return BadRequest("Missing or invalid ID.");
                }

                
                var response = await _http.DeleteAsync($"v1.0/Tickets/{item.ticketID}/ChecklistItems/{item.id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(responseContent, "application/json");
                }

                return StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting checklist item: {ex.Message}");
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
                        existingItem.IsDefaultContract = item.IsDefaultContract.Value;
                        existingItem.ContractName = item.ContractName;
                        existingItem.BillingPreference = item.BillingPreference;
                        existingItem.CompanyId = Convert.ToInt32(item.CompanyID);
                        itemsToUpdate.Add(existingItem);
                        
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
                            IsDefaultContract = item.IsDefaultContract.Value,
                        });

                    }
                }
                context.UpdateRange(itemsToUpdate);
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

        [HttpGet("tickets/contacts/{id:long}")]
        public async Task<IActionResult> GetTicketAdditionalContactsByTicketId(int id)
        {
            var response = await _http.GetAsync($"v1.0/Tickets/{id}/AdditionalContacts");
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

        [HttpGet("companylocations/{id:long}")]
        public async Task<IActionResult> GetCompanyLocationById(int id)
        {
            var response = await _http.GetAsync($"v1.0/CompanyLocations/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Content(content, "application/json");
            }

            return StatusCode((int)response.StatusCode, content);
        }
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
        [HttpGet("companies/sync")]
        public async Task<IActionResult> SyncCompanies([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                // Start URL (relative to your HttpClient BaseAddress)
                var nextUrl = $"v1.0/Companies/query?search={encodedSearch}";

                var existingItems = context.CompanyCaches.ToList();
                var itemsToUpdate = new List<CompanyCache>();
                var itemsToCreate = new List<CompanyCache>();
                while (!string.IsNullOrWhiteSpace(nextUrl))
                {
                    var response = await _http.GetAsync(nextUrl);
                    var content = await response.Content.ReadAsStringAsync();
                    var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<CompanyDto>>(content);
                    
                    foreach (var item in converted.Items)
                    {
                        var existingItem = existingItems.FirstOrDefault(x => x.Id == item.id);
                        if (existingItem != null)
                        {
                            existingItem.CompanyCategoryId = item.companyCategoryID;
                            existingItem.CompanyName = item.companyName;
                            existingItem.Classification = item.classification;
                            existingItem.IsActive = item.isActive;
                            existingItem.Phone = item.phone;
                            existingItem.Address1 = item.address1;
                            existingItem.Address2 = item.address2;
                            existingItem.City = item.city;
                            existingItem.State = item.state;
                            existingItem.PostalCode = item.postalCode;
                            itemsToUpdate.Add(existingItem);
                        }
                        else
                        {
                            itemsToCreate.Add(new CompanyCache()
                            {
                                Id = item.id,
                                IsActive = item.isActive,
                                Classification = item.classification,
                                CompanyCategoryId = item.companyCategoryID,
                                CompanyName = item.companyName,
                                Phone = item.phone,
                                Address1 = item.address1,
                                Address2 = item.address2,
                                City = item.city,
                                State = item.state,
                                PostalCode = item.postalCode,

                            });

                        }
                    }
                    // Follow the server-provided next page URL
                    nextUrl = converted?.PageDetails?.NextPageUrl;


                }

                await context.AddRangeAsync(itemsToCreate);
                context.UpdateRange(itemsToUpdate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching Companies: {ex.Message}");
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
        [HttpGet("tickets/secondaryresources/query")]
        public async Task<IActionResult> GetTicketSecondaryResources([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/TicketSecondaryResources/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching resources: {ex.Message}");
            }
        }

        [HttpGet("tickets/resources/{id:long}")]
        public async Task<IActionResult> GetTicketSecondaryResourcesByTicketId(int id)
        {
            var response = await _http.GetAsync($"v1.0/Tickets/{id}/SecondaryResources");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Content(content, "application/json");
            }

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpGet("resources/{id:long}")]
        public async Task<IActionResult> GetResourceById(int id)
        {
            var response = await _http.GetAsync($"v1.0/Resources/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Content(content, "application/json");
            }

            return StatusCode((int)response.StatusCode, content);
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
                        existingItem.FullName = item.firstName + " " + item.lastName;
                        existingItem.ResourceType = item.resourceType;
                        existingItem.UserName = item.userName;
                        existingItem.LicenseType = item.licenseType;
                        itemsToUpdate.Add(existingItem);
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
                            FullName = item.firstName + "" + item.lastName,
                            ResourceType = item.resourceType,
                            UserName = item.userName,
                            LicenseType = item.licenseType,

                        });

                    }
                }
                await context.AddRangeAsync(itemsToCreate);
                context.UpdateRange(itemsToUpdate);
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

        [HttpGet("actiontypes/sync")]
        public async Task<IActionResult> SyncActionTypes([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/ActionTypes/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();
                var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<ActionTypesDtoResult>>(content);
                var existingItems = context.ActionTypesCaches.ToList();
                var itemsToUpdate = new List<ActionTypesCache>();
                var itemsToCreate = new List<ActionTypesCache>();
                foreach (var item in converted.Items)
                {
                    var existingItem = existingItems.FirstOrDefault(x => x.Id == item.id);
                    if (existingItem != null)
                    {
                        existingItem.Id = item.id;
                        existingItem.Name = item.name;
                        existingItem.IsActive = item.isActive;
                        existingItem.IsSystemActionType = item.isSystemActionType;
                        existingItem.View = item.view;

                        itemsToUpdate.Add(existingItem);
                    }
                    else
                    {
                        itemsToCreate.Add(new ActionTypesCache()
                        {
                            Id = item.id,
                            Name = item.name,
                            IsActive = item.isActive,
                            IsSystemActionType = item.isSystemActionType,
                            View = item.view,

                        });

                    }
                }
                await context.AddRangeAsync(itemsToCreate);
                context.UpdateRange(itemsToUpdate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching action types: {ex.Message}");
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
                        existingItem.RoleType = item.roleType.Value;
                        itemsToUpdate.Add(existingItem);
                    }
                    else
                    {
                        itemsToCreate.Add(new RoleCache()
                        {
                            Id = item.id,
                            Name = item.name,
                            IsActive = item.isActive,
                            RoleType = item.roleType.Value,

                        });

                    }
                }
                await context.AddRangeAsync(itemsToCreate);
                context.UpdateRange(itemsToUpdate);
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
                        itemsToUpdate.Add(existingItem);
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
                context.UpdateRange(itemsToUpdate);
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
                            var existingItem = existingItems.FirstOrDefault(x => x.PicklistName == field.Name && x.Label == item.Label && x.Value == item.Value);
                            if (existingItem != null)
                            {
                                existingItem.PicklistName = field.Name;
                                existingItem.Label = item.Label;
                                existingItem.Value = item.Value;
                                existingItem.ValueInt = item.ValueInt.HasValue ? Convert.ToInt32(item.ValueInt) : null;
                                itemsToUpdate.Add(existingItem);
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
                context.UpdateRange(itemsToUpdate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching ticket picklists: {ex.Message}");
            }

        }


        [HttpGet("userdefinefields")]
        public async Task<IActionResult> GetTicketUserDefineFields()
        {
            var resp = await _http.GetAsync("v1.0/Tickets/entityInformation/userDefinedFields");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpGet("userdefinefields/sync")]
        public async Task<IActionResult> SyncTicketUserDefineFields()
        {
            try
            {

                var response = await _http.GetAsync($"v1.0/Tickets/entityInformation/userDefinedFields");
                var content = await response.Content.ReadAsStringAsync();
                var converted = JsonSerializer.Deserialize<TicketUserDefinedFieldsDtoResult>(content);
                var existingItems = context.TicketNoteEntityPicklistValueCaches.ToList();
                var itemsToUpdate = new List<TicketUserDefinedFieldsDtoResult>();
                var itemsToCreate = new List<TicketUserDefinedFieldsDtoResult>();
                //foreach (var field in converted.Fields)
                //{
                //    if (field.PicklistValues != null)
                //    {
                //        foreach (var item in field.PicklistValues)
                //        {
                //            var existingItem = existingItems.FirstOrDefault(x => x.PicklistName == field.Name && x.Label == item.Label);
                //            if (existingItem != null)
                //            {
                //                existingItem.PicklistName = field.Name;
                //                existingItem.Label = item.Label;
                //                existingItem.Value = item.Value;
                //                existingItem.ValueInt = item.ValueInt.HasValue ? Convert.ToInt32(item.ValueInt) : null;

                //            }
                //            else
                //            {
                //                itemsToCreate.Add(new TicketNoteEntityPicklistValueCache()
                //                {
                //                    PicklistName = field.Name,
                //                    Label = item.Label,
                //                    Value = item.Value,
                //                    ValueInt = item.ValueInt,
                //                });

                //            }
                //        }
                //    }
                //}

                //await context.AddRangeAsync(itemsToCreate);
                //await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching ticket note picklists: {ex.Message}");
            }
        }

        [HttpGet("ticketNotes/fields/sync")]
        public async Task<IActionResult> SyncTicketNotesPicklists()
        {
            try
            {

                var response = await _http.GetAsync($"v1.0/TicketNotes/entityInformation/fields");
                var content = await response.Content.ReadAsStringAsync();
                var converted = JsonSerializer.Deserialize<TicketEntityFieldsDto.EntityInformationFieldsResponse>(content);
                var existingItems = context.TicketNoteEntityPicklistValueCaches.ToList();
                var itemsToUpdate = new List<TicketNoteEntityPicklistValueCache>();
                var itemsToCreate = new List<TicketNoteEntityPicklistValueCache>();
                foreach (var field in converted.Fields)
                {
                    if (field.PicklistValues != null)
                    {
                        foreach (var item in field.PicklistValues)
                        {
                            var existingItem = existingItems.FirstOrDefault(x => x.PicklistName == field.Name && x.Label == item.Label && x.Value == item.Value);
                            if (existingItem != null)
                            {
                                existingItem.PicklistName = field.Name;
                                existingItem.Label = item.Label;
                                existingItem.Value = item.Value;
                                existingItem.ValueInt = item.ValueInt.HasValue ? Convert.ToInt32(item.ValueInt) : null;
                                itemsToUpdate.Add(existingItem);
                            }
                            else
                            {
                                itemsToCreate.Add(new TicketNoteEntityPicklistValueCache()
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
                context.UpdateRange(itemsToUpdate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching ticket note picklists: {ex.Message}");
            }

        }

        [HttpPost("notes")]
        public async Task<IActionResult> CreateNote([FromBody] NoteDto note)
        {
            try
            {
                var json = JsonSerializer.Serialize(note, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var request = new HttpRequestMessage(HttpMethod.Post,$"v1.0/Tickets/{note.ticketID}/Notes");

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // 🔑 Autotask impersonation header
                request.Headers.TryAddWithoutValidation("ImpersonationResourceId", note.impersonatorCreatorResourceID.ToString());

                var response = await _http.SendAsync(request);
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


        [HttpGet("tickets/query")]
        public async Task<IActionResult> GetTickets([FromQuery] string search)
        {
            try
            {
                // Provide a default valid search if none is provided
                if (string.IsNullOrWhiteSpace(search))
                {
                    search = "{\"filter\":[{\"op\":\"gt\",\"field\":\"id\",\"value\":0}]}";
                }

                var encodedSearch = Uri.EscapeDataString(search);
                var response = await _http.GetAsync($"v1.0/Tickets/query?search={encodedSearch}");
                var content = await response.Content.ReadAsStringAsync();
                var ticketsResult = JsonSerializer.Deserialize<AutotaskItemsResponse<TicketDtoResult.Item>>(content);


                //Get secondary resources
                var ticketIds = ticketsResult.Items.Select(x => x.id).ToArray();


                var ServiceCallTicketResourceFilter = new List<object>
                {
                    new { op = "in", field = "ticketID", value = ticketIds },
                };
                var ticketSecondaryResourcesObject = new
                {
                    filter = ServiceCallTicketResourceFilter,
                    MaxRecords = 500
                };

                var ticketSecondaryResourcesSearch = JsonSerializer.Serialize(ticketSecondaryResourcesObject);
                var ticketSecondaryResourcesEncodedSearch = Uri.EscapeDataString(ticketSecondaryResourcesSearch);
                var ticketSecondaryResourcesResponse = await _http.GetAsync($"v1.0/TicketSecondaryResources/query?search={ticketSecondaryResourcesEncodedSearch}");

                var ticketSecondaryResourcesContent = await ticketSecondaryResourcesResponse.Content.ReadAsStringAsync();
                var ticketSecondaryResources = JsonSerializer.Deserialize<AutotaskItemsResponse<TicketSecondaryResourcesDtoResult>>(ticketSecondaryResourcesContent);

                foreach ( var item in ticketsResult.Items)
                {
                    var resourceIds = ticketSecondaryResources.Items
                    .Where(x => x.ticketID == item.id)
                    .Select(x => x.resourceID.ToString());

                    item.secondaryResources = string.Join(",", resourceIds);

                }
                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching tickes: {ex.Message}");
            }

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
                var request = new HttpRequestMessage(HttpMethod.Post, $"v1.0/TimeEntries/");

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // 🔑 Autotask impersonation header
                request.Headers.TryAddWithoutValidation("ImpersonationResourceId", timeEntry.ResourceId.ToString());

                var response = await _http.SendAsync(request);

                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                //var response = await _http.PostAsync("v1.0/TimeEntries", content);
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
