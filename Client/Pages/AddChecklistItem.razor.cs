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
    public partial class AddChecklistItem
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
        protected SecurityService Security { get; set; }

        [Inject]
        public ATTimeService ATTimeService { get; set; }

        [Inject]
        public AutotaskService AutotaskService { get; set; }

        
        protected CrownATTime.Server.Models.ChecklistItemDto newChecklistItem { get; set; } = new Server.Models.ChecklistItemDto();
        [Parameter]
        public int TicketId { get; set; }

        protected bool savingRecord { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                newChecklistItem.ticketID = TicketId;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"{ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task TemplateForm0Submit(Server.Models.ChecklistItemDto args)
        {
            try
            {
                savingRecord = true;
                await AutotaskService.CreateChecklistItem(newChecklistItem);
                DialogService.Close();
            }
            catch (Exception ex)
            {
                savingRecord = false;

                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"{ex.Message}" });

            }
        }

        
    }
}