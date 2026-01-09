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
    public partial class ChecklistItems
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
        protected AutotaskTicketService AutotaskTicketService { get; set; }

        [Parameter]
        public int TicketId { get; set; }

        [Parameter]
        public int ResourceId { get; set; }

        protected List<TicketChecklistItemResult> TicketChecklistItemResult { get; set; }
        protected RadzenDataGrid<TicketChecklistItemResult> grid0 { get; set; }

        protected bool gridLoading { get; set; }


        protected async System.Threading.Tasks.Task DataGrid0LoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                gridLoading = true;
                TicketChecklistItemResult = await AutotaskTicketService.GetOpenTicketChecklistItems(TicketId);
                TicketChecklistItemResult = TicketChecklistItemResult.OrderBy(x => x.position).ToList();
                gridLoading = false;

            }
            catch (Exception ex)
            {
                gridLoading = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Checklist Items.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task CompleteChecklistItemButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, TicketChecklistItemResult item)
        {
            try
            {
                item.isBusy = true;
                item.isCompleted = true;
                item.completedDateTime = DateTime.Now;
                item.completedByResourceID = ResourceId;
                var response = await AutotaskTicketService.UpdateTicketChecklistItem(item);
                await grid0.Reload();
            }
            catch (Exception ex)
            {
                item.isBusy = true;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to Complete Checklist Item.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task GridRefreshButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await grid0.Reload();
        }

        protected async System.Threading.Tasks.Task AddChecklistItemButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddChecklistItem>("Add Checklist Item", new Dictionary<string, object>() { {"TicketId", TicketId} }, new DialogOptions { Draggable = true });
            await grid0.Reload();

        }

        protected async System.Threading.Tasks.Task DeleteButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, TicketChecklistItemResult item)
        {
            try
            {
                item.isBusy = true;
                await AutotaskTicketService.DeleteChecklistItem(item);
                await grid0.Reload();

            }
            catch (Exception ex)
            {
                item.isBusy = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to Delete Checklist Item.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task DataGrid0RowSelect(Server.Models.TicketChecklistItemResult args)
        {
            await DialogService.OpenAsync<EditChecklistItem>("Edit Checklist Item", new Dictionary<string, object>() { { "TicketId", args.ticketID }, { "Id", args.id } }, new DialogOptions { Draggable = true });
            await grid0.Reload();

        }
    }
}