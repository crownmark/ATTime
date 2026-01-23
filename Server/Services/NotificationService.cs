using CrownATTime.Client.Pages;
using CrownATTime.Server.Models;
using CrownATTime.Server.Models.ATTime;
using FluentScheduler;
using Microsoft.Graph.IdentityGovernance.LifecycleWorkflows.DeletedItems.Workflows.Item.MicrosoftGraphIdentityGovernanceActivateWithScope;
using System.Net.Http;
using System.Text.Json;

namespace CrownATTime.Server.Services
{
    public class NotificationService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor httpContextAccessor;
        private Schedule schedule;
        public NotificationService(IServiceScopeFactory scopeFactory, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.scopeFactory = scopeFactory;
            this._env = env;
            this.httpContextAccessor = httpContextAccessor;

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
                ? "https://localhost:5001/"   // adjust to your dev URL
                : "https://timeguard.crown.software/"; // adjust to your prod URL
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            schedule = new Schedule(
                    () => Console.WriteLine("Scheduled for every day at 4:30pm"),
                    run => run.EveryWeekday().At(16, 30)
                );
            schedule.JobStarted += Schedule_JobStarted;
            schedule.JobEnded += Schedule_JobEnded;
            schedule.Start();
        }

        private void Schedule_JobEnded(object sender, JobEndedEventArgs e)
        {

        }

        private void Schedule_JobStarted(object sender, JobStartedEventArgs e)
        {
            SendEndOfDayOpenTimeEntriesEmail();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            schedule.Stop();
        }

        public void Dispose()
        {

        }

        private async void SendEndOfDayOpenTimeEntriesEmail()
        {
            try
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var baseUrl = GetBaseUrl();

                    using var http = new HttpClient { BaseAddress = new Uri(baseUrl) };
                    
                    var atTimeService = scope.ServiceProvider.GetRequiredService<ATTimeService>();
                    var openTimeEntries = await atTimeService.GetTimeEntries(new Radzen.Query() { Filter = $"i => i.IsCompleted == false", OrderBy = $"TimeEntryId" });
                    var groups = openTimeEntries.GroupBy(x => x.ResourceId).ToList();
                    foreach (var techGroup in groups)
                    {
                        //get at resource
                        var resourceId = techGroup.Key;
                        var resource = await atTimeService.GetResourceCacheById(resourceId);
                        var techEntries = techGroup.OrderBy(x => x.AttimeEntryId).ToList();
                        if (!string.IsNullOrEmpty(resource?.Email))
                        {
                            // 2) Build HTML rows
                            var rowsHtml = BuildRowsHtml(techEntries);

                            // 3) Inject into your email template
                            var html = EmailTemplates.OpenTimeEntriesHtml
                                .Replace("{{Rows}}", rowsHtml);

                            //send email
                            var msg = new CrownATTime.Server.Models.EmailMessage
                            {
                                To = resource.Email,
                                From = "mark@ce-technology.com",
                                Subject = "Open time entries (end of day)",
                                Body = html,
                                // Add CC/BCC/ReplyTo/From if your model supports it
                            };

                            var response = await http.PostAsJsonAsync(
                                "Email/SendEmailFromSupport",
                                msg,
                                new JsonSerializerOptions(JsonSerializerDefaults.Web)
);

                            if (!response.IsSuccessStatusCode)
                            {
                                var error = await response.Content.ReadAsStringAsync();
                                throw new Exception($"Email send failed: {response.StatusCode} - {error}");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static string BuildRowsHtml(IEnumerable<TimeEntry> entries)
        {
            var sb = new System.Text.StringBuilder();

            foreach (var te in entries)
            {
                sb.AppendLine($@"
<tr>
  <td style=""padding:10px; border-bottom:1px solid #e5e7eb;"">
    <a href=""{HtmlEncode($"https://timeguard.crown.software/timeentry/{te.TicketId}")}"" style=""color:#2563eb; text-decoration:none;"">Open</a>
  </td>
  <td style=""padding:10px; border-bottom:1px solid #e5e7eb; white-space:nowrap;"">{HtmlEncode(te.TimeStampStatus ? "Running" : "Stopped")}</td>
  <td style=""padding:10px; border-bottom:1px solid #e5e7eb; white-space:nowrap;"">{HtmlEncode(te.TicketNumber)}</td>
  <td style=""padding:10px; border-bottom:1px solid #e5e7eb;"">{HtmlEncode(te.TicketTitle)}</td>
  <td style=""padding:10px; border-bottom:1px solid #e5e7eb;"">{HtmlEncode(te.AccountName)}</td>
  <td style=""padding:10px; border-bottom:1px solid #e5e7eb; white-space:nowrap;"">{HtmlEncode(te.DateWorked.Value.ToString("yyyy-MM-dd"))}</td>
</tr>");
            }

            return sb.ToString();
        }

        private static string HtmlEncode(string? value) =>
            System.Net.WebUtility.HtmlEncode(value ?? string.Empty);

        public static class EmailTemplates
        {
            public static string OpenTimeEntriesHtml => @"
<!doctype html>
<html>
<head>
  <meta charset=""utf-8"" />
</head>
<body style=""margin:0; padding:0; background:#f6f7f9; font-family: Arial, Helvetica, sans-serif; color:#111;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background:#f6f7f9; padding:24px 0;"">
    <tr>
      <td align=""center"">
        <table width=""700"" cellpadding=""0"" cellspacing=""0"" style=""background:#ffffff; border:1px solid #e5e7eb; border-radius:12px;"">
          <tr>
            <td style=""padding:18px 22px; background:#111827; color:#ffffff;"">
              <strong>End of Day Check</strong><br />
              <span style=""font-size:13px;"">Your open time entries</span>
            </td>
          </tr>

          <tr>
            <td style=""padding:16px 22px;"">
              <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""border-collapse:collapse;"">
                <tr>
                  <th align=""left"">Open</th>
                  <th align=""left"">Timer</th>
                  <th align=""left"">Ticket #</th>
                  <th align=""left"">Title</th>
                  <th align=""left"">Account</th>
                  <th align=""left"">Date</th>
                </tr>

                {{Rows}}

              </table>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
        }


    }
}
