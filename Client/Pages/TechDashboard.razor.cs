using CrownATTime.Server.Models;
using CrownATTime.Server.Models.ATTime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrownATTime.Client.Pages
{
    public partial class TechDashboard
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected AutotaskService AutotaskService { get; set; }

        [Inject]
        protected ATTimeService ATTimeService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        protected List<TicketDtoResult.Item> ticketResults { get; set; } = new List<TicketDtoResult.Item>();
        protected bool myTicketsGridLoading {  get; set; }
        protected List<TicketDtoResult.Item> myTickets {  get; set; } = new List<TicketDtoResult.Item>();
        
        protected RadzenDataGrid<TicketDtoResult.Item> myTicketsGrid;

        protected string myTicketsSearch = "";
        protected bool openTickets { get; set; } = true;
        protected bool newTickets {  get; set; }
        protected bool overdueTickets {  get; set; }
        protected bool scheduledTodayTickets {  get; set; }
        protected bool waitingTickets {  get; set; }
        protected bool inMyCourtTickets {  get; set; }

        
        public class CalendarItemSample
        {
            public int Id { get; set; }
            public DateTime Start { get; set; }
            public int DurationMinutes { get; set; }
            public string Title { get; set; }
            public string Type { get; set; } // Meeting, Ticket, Internal
            public string TicketNumber { get; set; }
        }
        public List<CalendarItemSample> CalendarItems = new()
        {
            // TODAY
            new CalendarItemSample
            {
                Id = 1,
                Start = DateTime.Today.AddHours(9),
                DurationMinutes = 15,
                Title = "Daily Dispatch Review",
                Type = "Meeting"
            },
            new CalendarItemSample
            {
                Id = 2,
                Start = DateTime.Today.AddHours(11),
                DurationMinutes = 60,
                Title = "Printer Mapping Issue",
                Type = "Onsite Support",
                TicketNumber = "T20260412.0010"
            },
            new CalendarItemSample
            {
                Id = 3,
                Start = DateTime.Today.AddHours(14.5),
                DurationMinutes = 45,
                Title = "Backup Verification Alert Review",
                Type = "Flexible Support",
                TicketNumber = "T20260411.0098"
            },

            // TOMORROW
            new CalendarItemSample
            {
                Id = 4,
                Start = DateTime.Today.AddDays(1).AddHours(8.5),
                DurationMinutes = 60,
                Title = "Firewall Rule Review",
                Type = "Meeting"
            },
            new CalendarItemSample
            {
                Id = 5,
                Start = DateTime.Today.AddDays(1).AddHours(10),
                DurationMinutes = 60,
                Title = "New User Setup Validation",
                Type = "Remote Support",
                TicketNumber = "T20260412.0014"
            },

            // DAY 3
            new CalendarItemSample
            {
                Id = 6,
                Start = DateTime.Today.AddDays(2).AddHours(9.5),
                DurationMinutes = 60,
                Title = "Wi-Fi Troubleshooting",
                Type = "Onsite Support",
                TicketNumber = "T20260410.0082"
            }
        };

        protected ResourceCache resource {  get; set; }
        protected List<TicketEntityPicklistValueCache> queues { get; set; }
        protected int? queueId { get; set; }

        protected RadzenDataGrid<CrownATTime.Server.Models.ATTime.TimeEntry> myTimeEntriesGrid;

        protected bool myTimeEntriesGridLoading {  get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntry> timeEntries;

        protected int timeEntriesCount;

        
        protected override async Task OnInitializedAsync()
        {
            try
            {
                
                var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Email eq '{Security.User.Email}'");
                resource = resourceResult.Value.FirstOrDefault();
                ReloadTicketsFromAutotask();
                var queueResult = await ATTimeService.GetTicketEntityPicklistValueCaches(filter: $"PicklistName eq 'queueID'", orderby: "Label");
                queues = queueResult.Value.ToList();
                await myTimeEntriesGrid.Reload();

            }
            catch (Exception ex)
            {

            }
            
        }

        protected async System.Threading.Tasks.Task ReloadTicketsFromAutotask()
        {
            try
            {
                myTicketsGridLoading = true;
                ticketResults.Clear();
                var results = await AutotaskService.GetTicketsForResourceId(resource.Id);
                foreach(var item in results.Items)
                {
                    ticketResults.Add(item);
                }

                await myTicketsGrid.Reload();
                myTicketsGridLoading = false;


            }
            catch (Exception ex)
            {
                myTicketsGridLoading = false;
            }
        }
        protected async System.Threading.Tasks.Task TicketsDataGrid1LoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                myTickets = new List<TicketDtoResult.Item>();
                
                
                if (openTickets)
                {
                    var filteredTickets = ticketResults.Where(x => x.status != 5);
                    myTickets.AddRange(filteredTickets);

                }
                else if (newTickets)
                {
                    var filteredTickets = ticketResults.Where(x => x.status != 5);
                    myTickets.AddRange(filteredTickets);
                }
                else if (overdueTickets)
                {
                    var filteredTickets = ticketResults.Where(x => x.dueDateTime < DateTime.Now);
                    myTickets.AddRange(filteredTickets);
                }
                else if (scheduledTodayTickets)
                {
                    var filteredTickets = ticketResults.Where(x => x.dueDateTime >= DateTime.Today && x.dueDateTime < DateTime.Today.AddDays(1));
                    myTickets.AddRange(filteredTickets);
                }
                else if (waitingTickets)
                {
                    var waitingStatuses = new HashSet<int>
                    {
                        7,
                        9,
                        12,
                        33,
                        34,
                        39
                    };
                    var filteredTickets = ticketResults.Where(x => waitingStatuses.Contains(x.status));
                    myTickets.AddRange(filteredTickets);
                }
                else if (inMyCourtTickets)
                {
                    var waitingStatuses = new HashSet<int>
                    {
                        5,
                        7,
                        9,
                        12,
                        33,
                        34,
                        39
                    };
                    var filteredTickets = ticketResults.Where(x => !waitingStatuses.Contains(x.status));
                    myTickets.AddRange(filteredTickets);
                }
                else
                {
                    myTickets.AddRange(ticketResults);
                }
                if (queueId.HasValue)
                {
                    myTickets = myTickets.Where(x => x.queueID == queueId.Value).ToList();
                }
                if (!string.IsNullOrEmpty(myTicketsSearch))
                {
                    myTickets = myTickets.Where(x =>
                            (x.title?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (x.ticketNumber?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false)
                        //(x.assignedResourceID?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        //(x.status?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        //(x.status?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        //(x.priority?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false)

                        ).ToList();
                    //myTickets.AddRange(filteredTickets);
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task TicketsDataGrid0RowSelect(TicketDtoResult.Item args)
        {
            try
            {
                var ticket = new TicketDtoResult()
                {
                    item = args
                };
                var primaryResource = await AutotaskService.GetResourceById(args.assignedResourceID.Value);
                await DialogService.OpenAsync<TicketDetails>("Ticket Details", new Dictionary<string, object>() { {"ResourceId", resource.Id}, {"Ticket", ticket}, {"PriorityName", args.priority.ToString()}, {"StatusName", args.status.ToString()}, {"PrimaryResource", $"{primaryResource.item.firstName} {primaryResource.item.lastName}" } }, new DialogOptions { Width = "1200px", CloseDialogOnOverlayClick = true });
            }
            catch (Exception ex)
            {
            }
        }

        protected async System.Threading.Tasks.Task TodayDataGrid1RowSelect(Pages.TechDashboard.CalendarItemSample args)
        {
        }

        protected async System.Threading.Tasks.Task TomorrowDataGrid2RowClick(Radzen.DataGridRowMouseEventArgs<Pages.TechDashboard.CalendarItemSample> args)
        {
        }

        protected async System.Threading.Tasks.Task Day3DataGrid3RowClick(Radzen.DataGridRowMouseEventArgs<Pages.TechDashboard.CalendarItemSample> args)
        {
        }

        protected async System.Threading.Tasks.Task TimeEntryDataGrid0RowClick(Radzen.DataGridRowMouseEventArgs<Server.Models.ATTime.TimeEntry> args)
        {
            
        }

        protected async System.Threading.Tasks.Task TimeEntryDataGrid0RowSelect(Server.Models.ATTime.TimeEntry args)
        {
            try
            {
                await DialogService.OpenAsync<TimeEntry>("Time Entry", new Dictionary<string, object>() { { "TicketId", args.TicketId.ToString() } }, new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "90%" });
                await myTimeEntriesGrid.Reload();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task TomorrowDataGrid3RowSelect(Pages.TechDashboard.CalendarItemSample args)
        {
        }

        protected async System.Threading.Tasks.Task Day3DataGrid4RowSelect(Pages.TechDashboard.CalendarItemSample args)
        {
        }

        protected async System.Threading.Tasks.Task TodayDataGrid2RowDeselect(Pages.TechDashboard.CalendarItemSample args)
        {
        }

        protected async System.Threading.Tasks.Task OverdueDataGrid2RowSelect(Pages.TechDashboard.CalendarItemSample args)
        {
        }

        protected async System.Threading.Tasks.Task AllOpenTicketsChip2Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            openTickets = true;
            newTickets = false;
            overdueTickets = false;
            scheduledTodayTickets = false;
            waitingTickets = false;
            inMyCourtTickets = false;
            await myTicketsGrid.Reload();
        }

        protected async System.Threading.Tasks.Task ScheduledTodayChip3Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            openTickets = false;
            newTickets = false;
            overdueTickets = false;
            scheduledTodayTickets = true;
            waitingTickets = false;
            inMyCourtTickets = false;

            await myTicketsGrid.Reload();

        }

        protected async System.Threading.Tasks.Task NewTicketsChip4Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            openTickets = false;
            newTickets = true;
            overdueTickets = false;
            scheduledTodayTickets = false;
            waitingTickets = false;
            inMyCourtTickets = false;

            await myTicketsGrid.Reload();

        }

        protected async System.Threading.Tasks.Task WaitingTicketsChip5Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            openTickets = false;
            newTickets = false;
            overdueTickets = false;
            scheduledTodayTickets = false;
            waitingTickets = true;
            inMyCourtTickets = false;

            await myTicketsGrid.Reload();

        }

        protected async System.Threading.Tasks.Task OverdueChip6Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            openTickets = false;
            newTickets = false;
            overdueTickets = true;
            scheduledTodayTickets = false;
            waitingTickets = false;
            inMyCourtTickets = false;

            await myTicketsGrid.Reload();

        }

        protected async System.Threading.Tasks.Task InMyCourtChip3Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            openTickets = false;
            newTickets = false;
            overdueTickets = false;
            scheduledTodayTickets = false;
            waitingTickets = false;
            inMyCourtTickets = true;

            await myTicketsGrid.Reload();
        }

        protected async System.Threading.Tasks.Task TicketSearchTextBox0Change(System.String args)
        {
            await myTicketsGrid.Reload();
        }

        protected async System.Threading.Tasks.Task QueuesChange(System.Object args)
        {
            await myTicketsGrid.Reload();

        }

        protected async System.Threading.Tasks.Task ReloadTicketsButton1Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await ReloadTicketsFromAutotask();
        }

        protected async System.Threading.Tasks.Task TimeEntriesGridRefreshButton1Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await myTimeEntriesGrid.Reload();
        }

        protected async System.Threading.Tasks.Task TimeEntriesDataGrid0LoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                myTimeEntriesGridLoading = true;
                var results = await ATTimeService.GetTimeEntries(filter: $"IsCompleted eq false and ResourceId eq {resource.Id}", orderby: "StartDateTime");
                timeEntries = results.Value.AsODataEnumerable();
                myTimeEntriesGridLoading = false;

            }
            catch (Exception ex)
            {
                myTimeEntriesGridLoading = false;

            }
        }

        protected async System.Threading.Tasks.Task AddTimeButton3Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, Server.Models.TicketDtoResult.Item data)
        {
            try
            {
                await DialogService.OpenAsync<TimeEntry>("Time Entry", new Dictionary<string, object>() { { "TicketId", data.id.ToString() } }, new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "90%" });

                await myTimeEntriesGrid.Reload();
            }
            catch (Exception ex)
            {

            }
        }
    }
}