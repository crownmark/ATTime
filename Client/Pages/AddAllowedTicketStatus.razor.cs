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
    public partial class AddAllowedTicketStatus
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
            allowedTicketStatus = new CrownATTime.Server.Models.ATTime.AllowedTicketStatus();
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.AllowedTicketStatus allowedTicketStatus;

        [Inject]
        protected SecurityService Security { get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> ticketEntityPicklistValueCaches;

        protected int ticketEntityPicklistValueCachesCount;

        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.CreateAllowedTicketStatus(allowedTicketStatus);
                DialogService.Close(allowedTicketStatus);
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
                var defaultFilter = $"PickListName eq 'status'";
                var result = await ATTimeService.GetTicketEntityPicklistValueCaches(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Label, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Label asc");

                ticketEntityPicklistValueCaches = result.Value.AsODataEnumerable();
                ticketEntityPicklistValueCachesCount = result.Count;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }
    }
}