using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using CrownATTime.Server.Models;

namespace CrownATTime.Client.Pages
{
    public partial class TimeEntries
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
        public ATTimeService ATTimeService { get; set; }

        [Inject]
        protected AutotaskService AutotaskService { get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntry> timeEntries;

        protected RadzenDataGrid<CrownATTime.Server.Models.ATTime.TimeEntry> grid0;
        protected bool MyTimeEntriesFilter { get; set; } = true;

        protected ResourceDtoResult resource {  get; set; }

        protected int count;
        protected bool gridLoading;

        protected string search = "";
        protected string defaultFilter = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {

                gridLoading = true;
                if(resource != null)
                {
                    if (MyTimeEntriesFilter)
                    {
                        defaultFilter = $"IsCompleted eq false and ResourceId eq {resource.id}";

                    }
                    else
                    {
                        defaultFilter = $"IsCompleted eq false";

                    }

                    var result = await ATTimeService.GetTimeEntries(filter: $@"{defaultFilter} and (contains(ResourceName,""{search}"") or contains(ContactName,""{search}"") or contains(TicketTitle,""{search}"") or contains(AccountName,""{search}"") or contains(PriorityName,""{search}"") or contains(StatusName,""{search}"") or contains(TicketNumber,""{search}"") or contains(SummaryNotes,""{search}"") or contains(InternalNotes,""{search}"")) and {(string.IsNullOrEmpty(args.Filter) ? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null);
                    timeEntries = result.Value.AsODataEnumerable();
                    count = result.Count;
                }
                
                gridLoading = false;
            }
            catch (System.Exception ex)
            {
                gridLoading = false;
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TimeEntries.  Error: {ex.Message}" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTimeEntry>("Add TimeEntry", null);
            await grid0.Reload();
        }

        protected async Task EditRow(CrownATTime.Server.Models.ATTime.TimeEntry args)
        {
            //await DialogService.OpenAsync<TimeEntry>("Time Entry", new Dictionary<string, object>() { {"TicketId", args.TicketId.ToString()} }, new DialogOptions { Width = "90%" });
            //await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CrownATTime.Server.Models.ATTime.TimeEntry timeEntry)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ATTimeService.DeleteTimeEntry(timeEntryId:timeEntry.TimeEntryId);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete TimeEntry"
                });
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var loggedinResource = await AutotaskService.GetLoggedInResource(Security.User.Email);
                resource = loggedinResource.Items.First();
                StateHasChanged();
                await grid0.Reload();
            }
            catch(Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to get Autotask User"
                });
            }
        }

        protected async System.Threading.Tasks.Task OpenButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {

        }

        protected async System.Threading.Tasks.Task RefreshButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await grid0.Reload();
        }

        protected async System.Threading.Tasks.Task MyTimeEntriesFilterChange(System.Boolean args)
        {
            await grid0.Reload();
        }
    }
}