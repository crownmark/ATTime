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
    public partial class AutotaskSettings
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

        [Inject]
        protected AutotaskTimeEntryService AutotaskTimeEntryService { get; set; }

        protected bool syncingContacts { get; set; }
        protected bool syncingTicketFields { get; set; }
        protected bool syncingBillingCodes { get; set; }
        protected bool syncingRoles { get; set; }
        protected bool syncingResources { get; set; }
        protected bool syncingServiceDeskRoles { get; set; }

        protected async System.Threading.Tasks.Task CredentialsButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
        }

        protected async System.Threading.Tasks.Task BillingCodesButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                syncingBillingCodes = true;
                await AutotaskTimeEntryService.SyncBillingCodes();
                syncingBillingCodes = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Billing Codes Synced" });

            }
            catch (Exception ex)
            {
                syncingBillingCodes = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task ContractsButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                syncingContacts = true;
                await AutotaskTicketService.SyncContracts();
                syncingContacts = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Contracts Synced" });

            }
            catch (Exception ex)
            {
                syncingContacts = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task ResourcesButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                syncingResources = true;
                await AutotaskTimeEntryService.SyncResources();
                syncingResources = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Resources Synced" });

            }
            catch (Exception ex)
            {
                syncingResources = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task ServiceDeskRolesButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                syncingServiceDeskRoles = true;
                await AutotaskTimeEntryService.SyncServiceDeskRoles();
                syncingServiceDeskRoles = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Service Desk Roles Synced" });

            }
            catch (Exception ex)
            {
                syncingServiceDeskRoles = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task RolesButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                syncingRoles = true;
                await AutotaskTimeEntryService.SyncRoles();
                syncingRoles = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Roles Synced" });

            }
            catch (Exception ex)
            {
                syncingRoles = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task TicketPicklistsButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                syncingTicketFields = true;
                await AutotaskTicketService.SyncTicketFields();
                syncingTicketFields = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Ticket Picklists Synced" });

            }
            catch (Exception ex)
            {
                syncingTicketFields = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{ex.Message}" });

            }
        }
    }
}