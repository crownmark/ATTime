using Humanizer.Localisation;
using System.Net.Http;
using System.Text.Json;

namespace CrownATTime.Server.Services
{
    public class AutotaskSyncService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string baseUri;
        private readonly HttpClient httpClient;
        public AutotaskSyncService(IServiceScopeFactory scopeFactory, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.scopeFactory = scopeFactory;
            this._env = env;
            this.httpContextAccessor = httpContextAccessor;
            this.baseUri = GetBaseUrl();
            httpClient = new HttpClient();

        }

        private string GetBaseUrl()
        {
            var req = httpContextAccessor.HttpContext?.Request;

            // If we're in a web request (common in ASP.NET Core), build it dynamically
            if (req != null)
                return $"{req.Scheme}://{req.Host}{req.PathBase}/";

            // Fallback when there is no active request (background job / startup)
            // Pick sane defaults by environment
            return _env.IsDevelopment()
                ? "https://localhost:5001/api/autotask/"   // adjust to your dev URL
                : "https://timeguard.crown.software/api/autotask/"; // adjust to your prod URL
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Autotask Sync Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                // Next top of the hour
                var nextRun = new DateTime(
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    0,
                    0
                ).AddHours(4);

                var delay = nextRun - now;

                Console.WriteLine($"Next run scheduled for {nextRun}");

                try
                {
                    await Task.Delay(delay, stoppingToken);

                    if (!stoppingToken.IsCancellationRequested)
                    {
                        await RunHourlyJob();
                    }
                }
                catch (TaskCanceledException)
                {
                    // Expected when shutting down
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"AutotaskSyncService error: {ex}");
                }
            }
        }
        private async Task RunHourlyJob()
        {
            Console.WriteLine($"Running hourly job at {DateTime.Now}");
            await SyncBillingCodes();
            await SyncCompanies();
            await SyncContracts();
            await SyncResources();
            await SyncRoles();
            await SyncServiceDeskRoles();
            await SyncTicketFields();
            await SyncTicketNoteFields();
            await SyncTicketUdfFields();

            // your logic here

        }

        public async Task SyncBillingCodes()
        {
            var filters = new List<object>
                {
                    //new { op = "eq", field = "isActive", value = true },
                    new { op = "eq", field = "useType", value = 1 },
                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri($"{baseUri}billingcodes/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                Console.WriteLine($"AutotaskSyncService SyncBillingCodes error: {content}");

            }
        }

        public async Task SyncCompanies()
        {
            var filters = new List<object>
                {
                    //new { op = "eq", field = "isActive", value = true },
                    new { op = "noteq", field = "id", value = 0},
                };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri($"{baseUri}companies/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                Console.WriteLine($"AutotaskSyncService SyncCompanies error: {content}");

            }
        }
        public async Task SyncContracts()
        {
            var filters = new List<object>
                {
                //new { op = "eq", field = "status", value = 1 }
                new { op = "gt", field = "id", value = 0 }

            };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri($"{baseUri}contracts/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            //var converted = JsonSerializer.Deserialize<AutotaskItemsResponse<ContractDto>>(content);
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                Console.WriteLine($"AutotaskSyncService SyncContracts error: {content}");

            }
        }
        public async Task SyncResources()
        {
            var filters = new List<object>
            {
                //new { op = "eq", field = "isActive", value = true }
                new { op = "gt", field = "id", value = 0 }
            };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri($"{ baseUri }resources /sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                Console.WriteLine($"AutotaskSyncService SyncResources error: {content}");

            }
        }

        public async Task SyncRoles()
        {
            var filters = new List<object>
            {
                //new { op = "eq", field = "isActive", value = true },
                //new { op = "eq", field = "isActive", value = true },

            };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri($"{baseUri}roles/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                Console.WriteLine($"AutotaskSyncService SyncRoles error: {content}");
            }
        }

        public async Task SyncServiceDeskRoles()
        {
            var filters = new List<object>
            {
                //new { op = "eq", field = "isActive", value = true },
                //new { op = "eq", field = "resourceID", value = resourceId },

            };
            var searchObj = new
            {
                filter = filters,
                MaxRecords = 500
            };

            var currentSearch = JsonSerializer.Serialize(searchObj);
            var encodedSearch = Uri.EscapeDataString(currentSearch);
            var uri = new Uri($"{baseUri}servicedeskroles/sync?search={encodedSearch}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                Console.WriteLine($"AutotaskSyncService SyncServiceDeskRoles error: {content}");
            }
        }

        public async Task SyncTicketFields()
        {

            var uri = new Uri($"{baseUri}tickets/fields/sync");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                Console.WriteLine($"AutotaskSyncService SyncTicketFields error: {content}");
            }
        }
        public async Task SyncTicketUdfFields()
        {

            var uri = new Uri($"{baseUri}userdefinefields/sync");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                Console.WriteLine($"AutotaskSyncService SyncTicketUdfFields error: {content}");
            }
        }
        public async Task SyncTicketNoteFields()
        {

            var uri = new Uri($"{baseUri}ticketNotes/fields/sync");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                Console.WriteLine($"AutotaskSyncService SyncTicketNoteFields error: {content}");
            }
        }
    }
}
