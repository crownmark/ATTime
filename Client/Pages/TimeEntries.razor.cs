using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

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

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntry> timeEntries;

        protected RadzenDataGrid<CrownATTime.Server.Models.ATTime.TimeEntry> grid0;
        protected int count;
        protected bool gridLoading;

        protected string search = "";

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
                string defaultFilter = $"IsCompleted eq false";
                var result = await ATTimeService.GetTimeEntries(filter: $@"{defaultFilter} and (contains(SummaryNotes,""{search}"") or contains(InternalNotes,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                timeEntries = result.Value.AsODataEnumerable();
                count = result.Count;
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
            await DialogService.OpenAsync<EditTimeEntry>("Edit TimeEntry", new Dictionary<string, object> { {"TimeEntryId", args.TimeEntryId} });
            await grid0.Reload();
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
    }
}