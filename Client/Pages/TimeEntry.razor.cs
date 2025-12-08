using CrownATTime.Client;
using CrownATTime.Server.Models;
using CrownATTime.Server.Models.ATTime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrownATTime.Client.Pages
{
    public partial class TimeEntry
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
        protected AutotaskTimeEntryService AutotaskTimeEntryService { get; set; }
        [Inject]
        protected AutotaskTicketService AutotaskTicketService { get; set; }

        [Inject]
        public ATTimeService ATTimeService { get; set; }

        protected CrownATTime.Server.Models.ATTime.TimeEntry timeEntryRecord { get; set; }
        protected TicketDtoResult ticket {  get; set; }
        protected ContactDtoResult contact {  get; set; }
        protected CompanyDtoResult company {  get; set; }
        protected ContractDtoResult contract {  get; set; }
        protected ResourceDtoResult resource {  get; set; }
        protected bool pageLoading { get; set; }
        [Parameter]
        public string TicketId { get; set; } 
        public string rocketshipUrl { get; set; }

        protected List<RoleDto> mappedRoles { get; set; } = new List<RoleDto>();
        protected List<BillingCodeDto> billingCodes { get; set; } = new List<BillingCodeDto>();
        protected List<ContractDto> contracts { get; set; } = new List<ContractDto>();

        protected List<TicketEntityFieldsDto.EntityField> ticketEntityFields { get; set; }
        protected string PriorityName { get; set; }
        protected string StatusName { get; set; }
        protected string ContractName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                pageLoading = true;
                var resourceResult = await AutotaskTimeEntryService.GetLoggedInResource(Security.User.Email);
                resource = resourceResult.Items.FirstOrDefault();
                var billingCodeItems = await AutotaskTimeEntryService.GetBillingCodes();
                billingCodes = billingCodeItems.Items;
                var roles = await AutotaskTimeEntryService.GetRoles();
                var serviceDeskRoles = await AutotaskTimeEntryService.GetServiceDeskRoles(resource.id);
                mappedRoles = AutotaskTimeEntryService.MapToServiceDeskRoles(roles.Items, serviceDeskRoles.Items, true);
                var fields = await AutotaskTicketService.GetTicketFields();
                ticketEntityFields = fields.Fields;
                
                ticket = await AutotaskTicketService.GetTicket(Convert.ToInt32(TicketId));
                var contractsResult = await AutotaskTicketService.GetTicketContracts(ticket.item.companyID);
                contracts = contractsResult.Items;
                contract = await AutotaskTicketService.GetContract(ticket.item.contractID);
                contact = await AutotaskTicketService.GetContact(Convert.ToInt32(ticket.item.contactID));
                company = await AutotaskTicketService.GetCompany(Convert.ToInt32(ticket.item.companyID));
                var statuses = ticketEntityFields.Where(x => x.Name == "status").FirstOrDefault().PicklistValues;
                var status = statuses.Where(x => x.Value == ticket.item.status.ToString()).FirstOrDefault();
                StatusName = status.Label;
                var priorities = ticketEntityFields.Where(x => x.Name == "priority").FirstOrDefault().PicklistValues;
                var priority = priorities.Where(x => x.Value == ticket.item.priority.ToString()).FirstOrDefault();
                PriorityName = priority.Label;
                var ticketLookupFields = await AutotaskTicketService.GetTicketFields();
                
                
                if (ticket == null)
                {
                    NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to find Ticket" });

                }
                else
                {
                    rocketshipUrl = $"https://r.giantrocketship.net/autotask-insight?isdarktheme=True&psaversion=2025.5.4.114022&resourceid={resource.id}&insightkey=DWWG6LLDX2O43RLX6R4CW554NN&vendorid=417&vendorsuppliedid=3d0ee874b9cf11f0bd9b0afe532016f9&entityid={ticket.item.id}&signature=4jO7Fi0PbtnaZtJpdF/xhD5Q92M=";


                    var timeOpenTimeEntries = await ATTimeService.GetTimeEntries(filter: $@"TicketId eq {TicketId} and ResourceID eq {resource.id} and IsCompleted eq false", orderby: null, top: 1);
                    if(timeOpenTimeEntries.Value.Count() > 0)
                    {
                        timeEntryRecord = timeOpenTimeEntries.Value.FirstOrDefault();
                    }
                    else
                    {
                        //Create a time entry record

                        timeEntryRecord = await ATTimeService.CreateTimeEntry(new Server.Models.ATTime.TimeEntry()
                        {
                            TicketId = Convert.ToInt32(TicketId),
                            ContractId = ticket.item.contractID,
                            BillingCodeId = Convert.ToInt32(ticket.item.billingCodeID),
                            RoleId = ticket.item.assignedResourceRoleID,
                            ResourceId = resource.id,
                            StartDateTime = DateTimeOffset.Now,
                            DateWorked = DateTimeOffset.Now,
                            TimeStampStatus = true,
                            
                            

                        });
                    }
                    pageLoading = false;

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Ticket.  Error: {ex.Message}" });
                pageLoading = false;

            }
        }

        protected async System.Threading.Tasks.Task TemplateForm0Submit(CrownATTime.Server.Models.ATTime.TimeEntry args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task SummaryNotesChange(System.String args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task InternalNotesChange(System.String args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task CallContactPhoneButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
        }

        protected async System.Threading.Tasks.Task CallContactMobileButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
        }

        protected async System.Threading.Tasks.Task BillingCodeIdChange(System.Object args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        
        protected async System.Threading.Tasks.Task RoleIdChange(System.Object args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        

        protected async System.Threading.Tasks.Task StartDateTimeChange(System.DateTime? args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task EndDateTimeChange(System.DateTime? args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task IsNonBillableChange(bool args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task ShowOnInvoiceChange(bool args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task ContractIdChange(System.Object args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        

        protected async System.Threading.Tasks.Task HoursWorkedChange(decimal? args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task OffsetHoursChange(decimal? args)
        {
            try
            {
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }
    }
}