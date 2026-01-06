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
    public partial class AddTimeEntryTemplate
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

        protected override async Task OnInitializedAsync()
        {
            timeEntryTemplate = new CrownATTime.Server.Models.ATTime.TimeEntryTemplate();
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.TimeEntryTemplate timeEntryTemplate;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.BillingCodeCache> billingCodeCachesForBillingCodeId;


        protected int billingCodeCachesForBillingCodeIdCount;
        protected CrownATTime.Server.Models.ATTime.BillingCodeCache billingCodeCachesForBillingCodeIdValue;

        [Inject]
        protected SecurityService Security { get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> ticketEntityPicklistValueCaches;

        protected int ticketEntityPicklistValueCachesCount;
        protected async Task billingCodeCachesForBillingCodeIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetBillingCodeCaches(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Name, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Name asc");
                billingCodeCachesForBillingCodeId = result.Value.AsODataEnumerable();
                billingCodeCachesForBillingCodeIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load BillingCodeCache" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.CreateTimeEntryTemplate(timeEntryTemplate);
                DialogService.Close(timeEntryTemplate);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected async Task ticketEntityPicklistValueCachesLoadData(LoadDataArgs args)
        {
            try
            {
                string defaultFilter = $"PicklistName eq 'status'";
                var result = await ATTimeService.GetTicketEntityPicklistValueCaches(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Label, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Label asc");

                ticketEntityPicklistValueCaches = result.Value.AsODataEnumerable();
                ticketEntityPicklistValueCachesCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }
    }
}