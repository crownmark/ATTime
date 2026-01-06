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
    public partial class TimeEntryTemplates
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

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> timeEntryTemplates;

        protected RadzenDataGrid<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> grid0;
        protected int count;

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
                var result = await ATTimeService.GetTimeEntryTemplates(filter: $@"(contains(Title,""{search}"") or contains(SummaryNotes,""{search}"") or contains(InternalNotes,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "BillingCodeCache", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                timeEntryTemplates = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TimeEntryTemplates" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTimeEntryTemplate>("Add TimeEntryTemplate", null);
            await grid0.Reload();
        }

        protected async Task EditRow(CrownATTime.Server.Models.ATTime.TimeEntryTemplate args)
        {
            await DialogService.OpenAsync<EditTimeEntryTemplate>("Edit TimeEntryTemplate", new Dictionary<string, object> { {"TimeEntryTemplateId", args.TimeEntryTemplateId} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CrownATTime.Server.Models.ATTime.TimeEntryTemplate timeEntryTemplate)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ATTimeService.DeleteTimeEntryTemplate(timeEntryTemplateId:timeEntryTemplate.TimeEntryTemplateId);

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
                    Detail = $"Unable to delete TimeEntryTemplate"
                });
            }
        }
    }
}