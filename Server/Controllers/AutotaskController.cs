namespace CrownATTime.Server.Controllers
{
    using CrownATTime.Server.Models;
    using CrownATTime.Server.Services;
    using Microsoft.AspNetCore.Http.Json;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;
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

        [HttpGet("tickets/fields")]
        public async Task<IActionResult> GetTicketFields()
        {
            var resp = await _http.GetAsync("v1.0/Tickets/entityInformation/fields");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
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

        [HttpPut]
        public async Task<IActionResult> UpdateTicket([FromBody] TicketDto ticket)
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
                var response = await _http.PutAsync("v1.0/Tickets", content);
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
