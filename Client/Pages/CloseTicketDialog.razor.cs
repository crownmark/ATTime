using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System.ComponentModel.DataAnnotations;

namespace CrownATTime.Client.Pages
{
    public partial class CloseTicketDialog
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
        protected AutotaskService AutotaskService { get; set; }

        [Inject]
        public ATTimeService ATTimeService { get; set; }
        protected Server.Models.CloseTicket closeTicketRecord { get; set; } = new Server.Models.CloseTicket();
        [Parameter]
        public int TicketId { get; set; }
        [Parameter]
        public int TimeEntryId { get; set; }

        protected Server.Models.ATTime.TimeEntry timeEntryRecord { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                timeEntryRecord = await ATTimeService.GetTimeEntryByTimeEntryId("",TimeEntryId);
                
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Error Loading Time Entry Record.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task TemplateForm0Submit(Server.Models.CloseTicket args)
        {
            try
            {
                await AutotaskService.UpdateTicket(new Server.Models.TicketUpdateDto()
                {
                    Id = TicketId,
                    Status = 5
                });
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Ticket Completed" });
                DialogService.Close();
            }
            catch (Exception ex)
            {

                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Error Completing Ticket.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task Button0Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                // If SummaryNotes is empty, set directly
                if (string.IsNullOrWhiteSpace(closeTicketRecord.Resolution))
                {
                    closeTicketRecord.Resolution = timeEntryRecord.SummaryNotes;
                    return;
                }

                // Otherwise append as a new line
                closeTicketRecord.Resolution += Environment.NewLine + timeEntryRecord.SummaryNotes;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Error Inserting Time Entry Note.  Error: {ex.Message}" });

            }
        }
    }
}