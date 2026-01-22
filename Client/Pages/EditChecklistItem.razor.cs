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
    public partial class EditChecklistItem
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

        

        protected CrownATTime.Server.Models.ChecklistItemDtoResult checklistItem { get; set; }
        [Parameter]
        public int TicketId { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected bool savingRecord { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                checklistItem = await AutotaskService.GetTicketChecklistItem(TicketId, Id);
                //checklistItem = new Server.Models.TicketChecklistItemResult()
                //{
                //    id = result.item.id,
                //    knowledgebaseArticleID = result.item.knowledgebaseArticleID,
                //    isCompleted = result.item.isCompleted,
                //    isImportant = result.item.isImportant,
                //    itemName = result.item.itemName,
                //    position = result.item.position,
                //    ticketID = result.item.ticketID,
                //};
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"{ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task TemplateForm0Submit(Server.Models.ChecklistItemDtoResult args)
        {
            try
            {
                savingRecord = true;
                await AutotaskService.UpdateTicketChecklistItem(new TicketChecklistItemResult()
                {
                    id = checklistItem.item.id,
                    isImportant = checklistItem.item.isImportant,
                    ticketID = checklistItem.item.ticketID,
                    itemName = checklistItem.item.itemName,
                    position = checklistItem.item.position,
                });
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