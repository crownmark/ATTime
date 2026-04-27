using CrownATTime.Client.CustomComponents.DialogManager;
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
        [Inject]
        protected DialogManager DialogManager { get; set; }

        protected List<TicketDtoResult.Item> ticketResults { get; set; } = new List<TicketDtoResult.Item>();
        protected bool myTicketsGridLoading {  get; set; }
        protected List<TicketDtoResult.Item> myTickets {  get; set; } = new List<TicketDtoResult.Item>();
        
        protected RadzenDataGrid<TicketDtoResult.Item> myTicketsGrid;

        protected string myTicketsSearch = "";
        protected bool openTickets { get; set; }
        protected int openTicketsCount { get; set; }
        protected bool newTickets {  get; set; }
        protected int newTicketsCount {  get; set; }
        protected bool overdueTickets {  get; set; }
        protected int overdueTicketsCount { get; set; }
        protected bool scheduledTodayTickets { get; set; } = true;
        protected int scheduledTodayTicketsCount { get; set; }
        protected bool waitingTickets {  get; set; }
        protected int waitingTicketsCount { get; set; }

        protected bool inMyCourtTickets {  get; set; }
        protected int inMyCourtTicketsCount { get; set; }

        protected bool unscheduledTickets {  get; set; }
        protected int unscheduledTicketsCount { get; set; }

        protected ResourceCache resource {  get; set; }
        protected List<TicketEntityPicklistValueCache> queues { get; set; }
        protected int? queueId { get; set; }

        protected RadzenDataGrid<CrownATTime.Server.Models.ATTime.TimeEntry> myTimeEntriesGrid;

        protected bool myTimeEntriesGridLoading {  get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntry> timeEntries;

        protected int timeEntriesCount;

        protected IEnumerable<CalendarEvent> calendarEvents = new List<CalendarEvent>();

        protected int calendarEventsCount;

        protected RadzenDataGrid<CalendarEvent> overdueEventsGrid;
        protected RadzenDataGrid<CalendarEvent> todaysEventsGrid;
        protected RadzenDataGrid<CalendarEvent> tomorrowsEventsGrid;
        protected RadzenDataGrid<CalendarEvent> thirdDayEventsGrid;
        protected bool calendarGridLoading { get; set; }

        
        protected override async Task OnInitializedAsync()
        {
            try
            {
                
                var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Email eq '{Security.User.Email}'");
                resource = resourceResult.Value.FirstOrDefault();
                var queueResult = await ATTimeService.GetTicketEntityPicklistValueCaches(filter: $"PicklistName eq 'queueID'", orderby: "Label");
                queues = queueResult.Value.ToList();
                //myTimeEntriesGridLoading = true;
                //myTicketsGridLoading = true;
                myTimeEntriesGrid.Reload();

                ReloadTicketsFromAutotask();
                
                //myTimeEntriesGridLoading = false;
                //myTicketsGridLoading = false;
                overdueEventsGrid.Reload();
                DialogManager.OnChange += StateHasChanged;


            }
            catch (Exception ex)
            {

            }
            
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                myTimeEntriesGridLoading = true;
                myTicketsGridLoading = true;

            }
        }

        protected async System.Threading.Tasks.Task CalendarDataGrid1LoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                calendarGridLoading = true;
                await LoadCalendarData();
                calendarGridLoading = false;

            }
            catch (Exception ex)
            {
                calendarGridLoading = false;

            }
        }
        protected async Task LoadCalendarData()
        {
            try
            {
                calendarEvents = await AutotaskService.GetCalendarEventsForResource(resource.Id);
                calendarEventsCount = calendarEvents.Count();

            }
            catch (Exception ex)
            {

            }
        }

        public void Dispose()
        {
            DialogManager.OnChange -= StateHasChanged;
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
                UpdateTicketCounts();
                await myTicketsGrid.Reload();
                myTicketsGridLoading = false;


            }
            catch (Exception ex)
            {
                myTicketsGridLoading = false;
            }
        }

        protected void UpdateTicketCounts()
        {
            // This method can be used to update any ticket count indicators on the UI based on the current filters
            // For example, you could set properties like OpenTicketsCount, NewTicketsCount, etc. here by applying the same filters to ticketResults
            openTicketsCount = ticketResults.Count(x => x.status != 5);
            newTicketsCount = ticketResults.Count(x => x.status != 5 && x.userDefinedFields.Where(x => x.name == "New Ticket" && x.value == "Yes").Any());
            overdueTicketsCount = ticketResults.Count(x => x.ServiceCallScheduledDate < DateTime.Now);
            //scheduledTodayTicketsCount = ticketResults.Count(x => x.ServiceCallScheduledDate >= DateTime.Today && x.ServiceCallScheduledDate < DateTime.Today.AddDays(1));
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            scheduledTodayTicketsCount = ticketResults.Count(x =>
            {
                var effectiveDate =
                    (x.ServiceCallScheduledDate >= today && x.ServiceCallScheduledDate < tomorrow)
                        ? x.ServiceCallScheduledDate
                        : (x.CompanyToDoScheduledDate >= today && x.CompanyToDoScheduledDate < tomorrow)
                            ? x.CompanyToDoScheduledDate
                            : (DateTime?)null;

                return effectiveDate != null;
            });
            waitingTicketsCount = ticketResults.Count(x =>
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
                return waitingStatuses.Contains(x.status);
            });
            inMyCourtTicketsCount = ticketResults.Count(x =>
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
                return !waitingStatuses.Contains(x.status);
            });
            unscheduledTicketsCount = ticketResults.Count(x => !x.OldestScheduledDate.HasValue);

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
                    var filteredTickets = ticketResults.Where(x => x.status != 5 && x.userDefinedFields.Where(x => x.name == "New Ticket" && x.value == "Yes").Any());
                    myTickets.AddRange(filteredTickets);
                }
                else if (overdueTickets)
                {
                    var filteredTickets = ticketResults.Where(x => x.OldestScheduledDate < DateTime.Now);
                    myTickets.AddRange(filteredTickets);
                }
                else if (scheduledTodayTickets)
                {
                    var filteredTickets = ticketResults.Where(x => x.OldestScheduledDate >= DateTime.Today && x.OldestScheduledDate < DateTime.Today.AddDays(1)).OrderBy(x => x.OldestScheduledDate);
                    myTickets.AddRange(filteredTickets);
                }
                else if(unscheduledTickets)
                {
                    var filteredTickets = ticketResults.Where(x => !x.OldestScheduledDate.HasValue);
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
                            (x.ticketNumber?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                              (x.priorityName?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                              (x.statusName?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                              (x.queueName?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false) ||
                              (x.companyName?.Contains(myTicketsSearch, StringComparison.OrdinalIgnoreCase) ?? false)

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
                await DialogService.OpenAsync<TicketDetails>("Ticket Details", new Dictionary<string, object>() { {"ResourceId", resource.Id}, {"Ticket", ticket}, {"PriorityName", args.priorityName.ToString()}, {"StatusName", args.statusName.ToString()}, {"PrimaryResource", $"{primaryResource.item.firstName} {primaryResource.item.lastName}" } }, new DialogOptions { Width = "1200px", CloseDialogOnOverlayClick = true });
            }
            catch (Exception ex)
            {
            }
        }

        

        protected async System.Threading.Tasks.Task TomorrowDataGrid2RowClick(Radzen.DataGridRowMouseEventArgs<CalendarEvent> args)
        {
        }

        protected async System.Threading.Tasks.Task Day3DataGrid3RowClick(Radzen.DataGridRowMouseEventArgs<CalendarEvent> args)
        {
        }

        protected async System.Threading.Tasks.Task TimeEntryDataGrid0RowClick(Radzen.DataGridRowMouseEventArgs<Server.Models.ATTime.TimeEntry> args)
        {
            
        }

        protected async System.Threading.Tasks.Task TimeEntryDataGrid0RowSelect(Server.Models.ATTime.TimeEntry args)
        {
            try
            {
                DialogManager.OpenOrFocus<TimeEntry>(
                    id: args.TicketId,
                    title: $"{args.TicketNumber} - {args.TicketTitle}",
                    type: "TimeEntry",
                    parameters: new Dictionary<string, object>
                    {
                        { "TicketId", args.TicketId.ToString() }
                    }
                );

                await JSRuntime.InvokeVoidAsync("scrollToTopBlazor");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task TomorrowDataGrid3RowSelect(CalendarEvent args)
        {
            try
            {
                if(args.TicketId != null)
                {
                    DialogManager.OpenOrFocus<TimeEntry>(
                    id: args.TicketId.Value,
                    title: $"{args.Title}",
                    type: "TimeEntry",
                    parameters: new Dictionary<string, object>
                    {
                        { "TicketId", args.TicketId.ToString() }
                    }
                );
                }
                await JSRuntime.InvokeVoidAsync("scrollToTopBlazor");


            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task Day3DataGrid4RowSelect(CalendarEvent args)
        {
            try
            {
                if (args.TicketId != null)
                {
                    DialogManager.OpenOrFocus<TimeEntry>(
                    id: args.TicketId.Value,
                    title: $"{args.Title}",
                    type: "TimeEntry",
                    parameters: new Dictionary<string, object>
                    {
                        { "TicketId", args.TicketId.ToString() }
                    }
                );
                }
                await JSRuntime.InvokeVoidAsync("scrollToTopBlazor");


            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Time Entry.  Error: {ex.Message}" });

            }
        }
        protected async System.Threading.Tasks.Task TodayDataGrid1RowSelect(CalendarEvent args)
        {
            try
            {
                if (args.TicketId != null)
                {
                    DialogManager.OpenOrFocus<TimeEntry>(
                    id: args.TicketId.Value,
                    title: $"{args.Title}",
                    type: "TimeEntry",
                    parameters: new Dictionary<string, object>
                    {
                        { "TicketId", args.TicketId.ToString() }
                    }
                );
                }
                await JSRuntime.InvokeVoidAsync("scrollToTopBlazor");


            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task TodayDataGrid2RowDeselect(CalendarEvent args)
        {
            
        }

        protected async System.Threading.Tasks.Task OverdueDataGrid2RowSelect(CalendarEvent args)
        {
            try
            {
                if (args.TicketId != null)
                {
                    DialogManager.OpenOrFocus<TimeEntry>(
                    id: args.TicketId.Value,
                    title: $"{args.Title}",
                    type: "TimeEntry",
                    parameters: new Dictionary<string, object>
                    {
                        { "TicketId", args.TicketId.ToString() }
                    }
                );
                }
                await JSRuntime.InvokeVoidAsync("scrollToTopBlazor");


            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task AllOpenTicketsChip2Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            openTickets = true;
            newTickets = false;
            overdueTickets = false;
            scheduledTodayTickets = false;
            waitingTickets = false;
            inMyCourtTickets = false;
            unscheduledTickets = false;

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
            unscheduledTickets = false;
            await myTicketsGrid.Reload();

        }
        protected async System.Threading.Tasks.Task UnScheduledTodayChip3Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            openTickets = false;
            newTickets = false;
            overdueTickets = false;
            scheduledTodayTickets = false;
            waitingTickets = false;
            inMyCourtTickets = false;
            unscheduledTickets = true;
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
            unscheduledTickets = false;

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
            unscheduledTickets = false;

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
            unscheduledTickets = false;

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
            unscheduledTickets = false;
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
                DialogManager.OpenOrFocus<TimeEntry>(
                    id: data.id,
                    title: $"{data.ticketNumber} - {data.title}",
                    type: "TimeEntry",
                    parameters: new Dictionary<string, object>
                    {
                        { "TicketId", data.id.ToString() }
                    }
                );
                await JSRuntime.InvokeVoidAsync("scrollToTopBlazor");

                //await DialogService.OpenAsync<TimeEntry>("Time Entry", new Dictionary<string, object>() { { "TicketId", data.id.ToString() } }, new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "90%" });

                //await myTimeEntriesGrid.Reload();
            }
            catch (Exception ex)
            {

            }
        }

        protected RenderFragment CreateComponent<T>(Dictionary<string, object>? parameters = null) where T : IComponent
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(T));

                if (parameters != null)
                {
                    var i = 1;
                    foreach (var parameter in parameters)
                    {
                        builder.AddAttribute(i++, parameter.Key, parameter.Value);
                    }
                }

                builder.CloseComponent();
            };
        }

        protected async System.Threading.Tasks.Task MinimizeWindowButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, AppDialog dialog)
        {
            DialogManager.Minimize(dialog.Id, dialog.Type);
            await myTimeEntriesGrid.Reload();

        }

        protected async System.Threading.Tasks.Task CloseWindowButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, AppDialog dialog)
        {
            DialogManager.Close(dialog.Id, dialog.Type);
            await myTimeEntriesGrid.Reload();

        }

        protected async System.Threading.Tasks.Task PopoutWindowButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, AppDialog dialog)
        {
            DialogManager.Close(dialog.Id, dialog.Type);
            await myTimeEntriesGrid.Reload();
            try
            {
                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"timeentry/{dialog.Id.ToString()}");
            }
            catch (Exception ex)
            {

            }

        }


        protected async System.Threading.Tasks.Task NewTicketButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"https://ww5.autotask.net/Autotask/AutotaskExtend/ExecuteCommand.aspx?Code=NewTicket");
            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task CalendarGridRefreshButton1Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await overdueEventsGrid.Reload();
        }

        protected async System.Threading.Tasks.Task CalendarRefreshButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Reload Calendar Data");

        }

        protected async System.Threading.Tasks.Task CalendarRefreshButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }
        protected async System.Threading.Tasks.Task AddTimeEntryButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Create a Time Entry");

        }

        protected async System.Threading.Tasks.Task AddTimeEntryButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }

        protected async System.Threading.Tasks.Task RefreshTimeEntriesButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Reload Time Entries");

        }

        protected async System.Threading.Tasks.Task RefreshTimeEntriesButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task RefreshTicketsButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Reload Tickets from Autotask");

        }

        protected async System.Threading.Tasks.Task RefreshTicketsButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task PopoutTimeEntryButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Pop Out Time Entry in a New Window");

        }

        protected async System.Threading.Tasks.Task PopoutTimeEntryButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        
    }
}