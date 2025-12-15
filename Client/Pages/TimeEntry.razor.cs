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
        protected ContractCache contract {  get; set; }
        protected ResourceCache resource {  get; set; }
        protected bool pageLoading { get; set; }
        [Parameter]
        public string TicketId { get; set; } 
        public string rocketshipUrl { get; set; }

        protected List<RoleCache> mappedRoles { get; set; } = new List<RoleCache>();
        protected List<BillingCodeCache> billingCodes { get; set; } = new List<BillingCodeCache>();
        protected List<ContractCache> contracts { get; set; } = new List<ContractCache>();

        //protected List<TicketEntityFieldsDto.EntityField> ticketEntityFields { get; set; }
        protected List<TicketEntityPicklistValueCache> ticketStatuses { get; set; } = new List<TicketEntityPicklistValueCache>();
        protected string PriorityName { get; set; }
        protected string StatusName { get; set; }
        protected string ContractName { get; set; }

        private System.Timers.Timer? _stopwatchTimer;
        private bool _isRunning;
        private bool saveAndCloseTicket = false;
        private DateTime _lastTickUtc;
        private string ElapsedFormatted => TimeSpan.FromMilliseconds((timeEntryRecord.DurationMs ?? 0)).ToString(@"hh\:mm\:ss");

        protected int accordionSelectedIndex { get; set; }
        private bool _openedAccordionOnce;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_openedAccordionOnce && !pageLoading)
            {
                await Task.Delay(3000);
                _openedAccordionOnce = true;
                accordionSelectedIndex = -1;
                await InvokeAsync(StateHasChanged);
                accordionSelectedIndex = 0;
                await InvokeAsync(StateHasChanged);
            }
        }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                pageLoading = true;


                var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Email eq '{Security.User.Email}'");// await AutotaskTimeEntryService.GetLoggedInResource(Security.User.Email); //cache in db
                //var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Email eq 'jordan@ce-technology.com'");// await AutotaskTimeEntryService.GetLoggedInResource(Security.User.Email); //cache in db
                resource = resourceResult.Value.FirstOrDefault();
                var billingCodeItems = await ATTimeService.GetBillingCodeCaches();// await AutotaskTimeEntryService.GetBillingCodes(); //cache in db
                billingCodes = billingCodeItems.Value.ToList();
                var roles = await ATTimeService.GetRoleCaches();// await AutotaskTimeEntryService.GetRoles(); //cache in db
                var serviceDeskRoles = await ATTimeService.GetServiceDeskRoleCaches(filter: $"ResourceId eq {resource.Id} and IsActive eq true");// await AutotaskTimeEntryService.GetServiceDeskRoles(resource.id);
                mappedRoles = AutotaskTimeEntryService.MapToServiceDeskRoles(roles.Value.ToList(), serviceDeskRoles.Value.ToList(), true); //get from db
                var fields = await AutotaskTicketService.GetTicketFields(); //cache in db
                //ticketEntityFields = fields.Fields;
                
                ticket = await AutotaskTicketService.GetTicket(Convert.ToInt32(TicketId));
                var contractsResult = await ATTimeService.GetContractCaches(filter: $"CompanyId eq {ticket.item.companyID} and Status eq 1");// await AutotaskTicketService.GetTicketContracts(ticket.item.companyID); //cache in db
                contracts = contractsResult.Value.ToList();
                contract = await ATTimeService.GetContractCacheById("", Convert.ToInt32(ticket.item.contractID));// await AutotaskTicketService.GetContract(ticket.item.contractID?? 0); //get from db


                if (ticket == null)
                {
                    NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to find Ticket" });

                }
                else
                {
                    rocketshipUrl = $"https://r.giantrocketship.net/autotask-insight?isdarktheme=True&psaversion=2025.5.4.114022&resourceid={resource.Id}&vendorid=417&vendorsuppliedid=3d0ee874b9cf11f0bd9b0afe532016f9&entityid={ticket.item.id}&signature=4jO7Fi0PbtnaZtJpdF/xhD5Q92M=";


                    var timeOpenTimeEntries = await ATTimeService.GetTimeEntries(filter: $@"TicketId eq {TicketId} and ResourceID eq {resource.Id} and IsCompleted eq false", orderby: null, top: 1);
                    if(timeOpenTimeEntries.Value.Count() > 0)
                    {
                        timeEntryRecord = timeOpenTimeEntries.Value.FirstOrDefault();
                        // If it's null, treat it as 0
                        timeEntryRecord.DurationMs ??= 0;
                    }
                    else
                    {
                        
                        //Create a time entry record
                        var newTimeEntry = new Server.Models.ATTime.TimeEntry()
                        {
                            TicketId = Convert.ToInt32(TicketId),
                            TicketNumber = ticket.item.ticketNumber,
                            ContractId = ticket.item.contractID,
                            BillingCodeId = Convert.ToInt32(ticket.item.billingCodeID),
                            RoleId = Convert.ToInt32(ticket.item.assignedResourceRoleID),
                            ResourceId = resource.Id,
                            StartDateTime = DateTimeOffset.Now,
                            EndDateTime = DateTimeOffset.Now,
                            DateWorked = DateTimeOffset.Now,
                            TimeStampStatus = true,
                            DurationMs = 0,
                            TicketTitle = ticket.item.title,
                            

                        };
                        var selectedBillingCode = billingCodes.Where(x => x.Id == ticket.item.billingCodeID).FirstOrDefault();

                        if (selectedBillingCode != null)
                        {
                            
                            if (selectedBillingCode.BillingCodeType == 2)
                            {
                                newTimeEntry.IsNonBillable = true;
                                newTimeEntry.ShowOnInvoice = false;

                            }
                            else
                            {
                                if (ticket.item.contractID.HasValue)
                                {
                                    var contractExclusion = await AutotaskTimeEntryService.GetContractExclusionsBillingCode(Convert.ToInt32(newTimeEntry.ContractId), selectedBillingCode.Id);//cache in db

                                    if (contractExclusion != null)
                                    {
                                        newTimeEntry.IsNonBillable = true;
                                        newTimeEntry.ShowOnInvoice = false;
                                    }
                                    else
                                    {
                                        newTimeEntry.IsNonBillable = false;
                                        newTimeEntry.ShowOnInvoice = true;
                                        NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Warning, Summary = $"Billable Notice", Detail = $"This work type is considered billable and not covered under the contract.", Duration = 10000 });

                                    }
                                }
                                else
                                {
                                    newTimeEntry.IsNonBillable = false;
                                    newTimeEntry.ShowOnInvoice = true;
                                    NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Warning, Summary = $"No Contract Set", Detail = $"This work type is considered billable and no contract has been set on the ticket.", Duration = 10000 });

                                }


                            }
                        }
                        timeEntryRecord = await ATTimeService.CreateTimeEntry(newTimeEntry);
                    }
                    UpdateTicketValues();
                    accordionSelectedIndex = 0;

                    pageLoading = false;

                    // Create the timer that will update ElapsedMS
                    _stopwatchTimer = new System.Timers.Timer(250); // 250ms tick
                    _stopwatchTimer.AutoReset = true;
                    _stopwatchTimer.Elapsed += OnStopwatchElapsed;
                    if (timeEntryRecord.TimeStampStatus)
                    {
                        _isRunning = true;
                        _lastTickUtc = DateTime.UtcNow;
                        _stopwatchTimer?.Start();
                    }
                    
                    StateHasChanged();

                }
            }
            catch (Exception ex)
            {

                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Ticket.  Error: {ex.Message}" });
                pageLoading = false;
                StateHasChanged();

            }
        }

        
        protected async System.Threading.Tasks.Task UpdateTicketValues()
        {
            try
            {
                timeEntryRecord.TicketTitle = ticket.item.title;
                contact = await AutotaskTicketService.GetContact(Convert.ToInt32(ticket.item.contactID));
                company = await AutotaskTicketService.GetCompany(Convert.ToInt32(ticket.item.companyID));
                var picklistValues = await ATTimeService.GetTicketEntityPicklistValueCaches();
                var picklistValuesList = picklistValues.Value.ToList();
                var statuses = picklistValuesList.Where(x => x.PicklistName == "status");
                //var statuses = ticketEntityFields.Where(x => x.Name == "status").FirstOrDefault().PicklistValues;
                var allowedIds = new HashSet<int> { 1, 7, 8, 10, 12, 23, 29, 27, 32, 33, 34, 46, 47 }; // example status IDs

                var filtered = statuses
                    .Where(s => s.ValueInt.HasValue && allowedIds.Contains(s.ValueInt.Value)).OrderBy(x => x.Label)
                    .ToList();

                ticketStatuses = filtered;
                var status = statuses.Where(x => x.Value == ticket.item.status.ToString()).FirstOrDefault();
                StatusName = status.Label;
                var priorities = picklistValuesList.Where(x => x.PicklistName == "priority");
                var priority = priorities.Where(x => x.Value == ticket.item.priority.ToString()).FirstOrDefault();
                PriorityName = priority.Label;
                //var ticketLookupFields = await AutotaskTicketService.GetTicketFields();
                timeEntryRecord.AccountName = company.item.companyName;
                timeEntryRecord.ContactName = $"{contact.item.firstName} {contact.item.lastName}";
                timeEntryRecord.PriorityName = PriorityName;
                timeEntryRecord.StatusName = StatusName;
                timeEntryRecord.ResourceName = $"{resource.FirstName} {resource.LastName}";
                timeEntryRecord.HoursWorked =
                   Math.Max(
                       Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2)
                       - (timeEntryRecord.OffsetHours ?? 0),
                       0
                   );
                timeEntryRecord.StartDateTime = CalculateStartFromDuration(DateTimeOffset.Now, timeEntryRecord.DurationMs.Value); //DateTimeOffset.Now;
                timeEntryRecord.EndDateTime = DateTimeOffset.Now;
                timeEntryRecord.DateWorked = DateTimeOffset.Now;
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });

                StateHasChanged();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Error Updating Ticket Related Information.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task TemplateForm0Submit(CrownATTime.Server.Models.ATTime.TimeEntry args)
        {
            try
            {
                if(timeEntryRecord.TimeStampStatus == true)
                {
                    //timer is still running
                    NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"The Timer is still running.  Please pause the timer before saving so it can calculate the hours worked." });

                }
                else
                {
                    var newATTimeEntry = new TimeEntryCreateDto()
                    {
                        DateWorked = timeEntryRecord.DateWorked.Value,
                        BillingCodeId = timeEntryRecord.BillingCodeId.Value,
                        StartDateTime = timeEntryRecord.StartDateTime,
                        EndDateTime = timeEntryRecord.EndDateTime,
                        HoursWorked = timeEntryRecord.HoursWorked.Value,
                        InternalNotes = timeEntryRecord.InternalNotes,
                        IsNonBillable = timeEntryRecord.IsNonBillable,
                        ResourceId = timeEntryRecord.ResourceId,
                        RoleId = timeEntryRecord.RoleId.Value,
                        SummaryNotes = timeEntryRecord.SummaryNotes,
                        TicketId = timeEntryRecord.TicketId,
                        ContractId = timeEntryRecord.ContractId,
                        OffsetHours = timeEntryRecord.OffsetHours,


                    };
                    var saveATTime = await AutotaskTimeEntryService.CreateTimeEntry(newATTimeEntry);
                    timeEntryRecord.IsCompleted = true;
                    timeEntryRecord.TimeStampStatus = false;
                    timeEntryRecord.AttimeEntryId = saveATTime.itemId;
                    await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                    ticket = await AutotaskTicketService.GetTicket(timeEntryRecord.TicketId);


                    NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });
                    if (saveAndCloseTicket)
                    {
                        if (await DialogService.Confirm("Are you sure you want to close this ticket?", "Close Ticket Confirmation", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" }) == true)
                        {
                            await DialogService.OpenAsync<CloseTicketDialog>($"Close Ticket Dialog | {ticket.item.title}", new Dictionary<string, object>() { { "TicketId", timeEntryRecord.TicketId }, { "TimeEntryId", timeEntryRecord.TimeEntryId } }, new DialogOptions { Width = "800px", Resizable = true, Draggable = true });
                            await JSRuntime.InvokeVoidAsync(
                                "eval",
                                "window.open('', '_self'); window.close();"
                            );
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync(
                                "eval",
                                "window.open('', '_self'); window.close();"
                            );
                        }


                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync(
                                                "eval",
                                                "window.open('', '_self'); window.close();"
                                            );
                    }
                }
                    
                    
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
                await UpdateTicketValues();

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
                await UpdateTicketValues();

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

        protected async System.Threading.Tasks.Task BillingCodeIdChange(int args)
        {
            try
            {
                var selectedBillingCode = billingCodes.Where(x => x.Id == timeEntryRecord.BillingCodeId.Value).FirstOrDefault();

                if (selectedBillingCode != null)
                {

                    if (selectedBillingCode.BillingCodeType == 2)
                    {
                        timeEntryRecord.IsNonBillable = true;
                        timeEntryRecord.ShowOnInvoice = false;

                    }
                    else
                    {
                        if (timeEntryRecord.ContractId.HasValue)
                        {
                            var contractExclusion = await AutotaskTimeEntryService.GetContractExclusionsBillingCode(Convert.ToInt32(timeEntryRecord.ContractId.Value), selectedBillingCode.Id);

                            if (contractExclusion != null)
                            {
                                timeEntryRecord.IsNonBillable = true;
                                timeEntryRecord.ShowOnInvoice = false;
                            }
                            else
                            {
                                timeEntryRecord.IsNonBillable = false;
                                timeEntryRecord.ShowOnInvoice = true;
                                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Warning, Summary = $"Billable Notice", Detail = $"This work type is considered billable and not covered under the contract.", Duration = 10000 });

                            }
                        }
                        else
                        {
                            timeEntryRecord.IsNonBillable = false;
                            timeEntryRecord.ShowOnInvoice = true;
                            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Warning, Summary = $"No Contract Set", Detail = $"This work type is considered billable and no contract has been set on the ticket.", Duration = 10000 });

                        }


                    }
                }
                await UpdateTicketValues();

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
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved" });
                await UpdateTicketValues();

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
                await UpdateTicketValues();

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
                await UpdateTicketValues();

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
                await UpdateTicketValues();

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
                await UpdateTicketValues();

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
                var selectedBillingCode = billingCodes.Where(x => x.Id == timeEntryRecord.BillingCodeId.Value).FirstOrDefault();

                if (selectedBillingCode != null)
                {

                    if (selectedBillingCode.BillingCodeType == 2)
                    {
                        timeEntryRecord.IsNonBillable = true;
                        timeEntryRecord.ShowOnInvoice = false;

                    }
                    else
                    {
                        if (timeEntryRecord.ContractId.HasValue)
                        {
                            var contractExclusion = await AutotaskTimeEntryService.GetContractExclusionsBillingCode(Convert.ToInt32(timeEntryRecord.ContractId.Value), selectedBillingCode.Id);

                            if (contractExclusion != null)
                            {
                                timeEntryRecord.IsNonBillable = true;
                                timeEntryRecord.ShowOnInvoice = false;
                            }
                            else
                            {
                                timeEntryRecord.IsNonBillable = false;
                                timeEntryRecord.ShowOnInvoice = true;
                                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Warning, Summary = $"Billable Notice", Detail = $"This work type is considered billable and not covered under the contract.", Duration = 10000 });

                            }
                        }
                        else
                        {
                            timeEntryRecord.IsNonBillable = false;
                            timeEntryRecord.ShowOnInvoice = true;
                            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Warning, Summary = $"No Contract Set", Detail = $"This work type is considered billable and no contract has been set on the ticket.", Duration = 10000 });

                        }


                    }
                }
                await UpdateTicketValues();

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
                timeEntryRecord.HoursWorked =
                    Math.Max(
                        Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2)
                        + (timeEntryRecord.OffsetHours ?? 0),
                        0
                    );
                await UpdateTicketValues();

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
                timeEntryRecord.HoursWorked = CalculateHoursWorked(
                    timeEntryRecord.DurationMs.Value,
                    timeEntryRecord.OffsetHours.Value
                );
                //timeEntryRecord.HoursWorked =
                //    Math.Max(
                //        Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2)
                //        + (timeEntryRecord.OffsetHours ?? 0),
                //        0
                //    );
                await UpdateTicketValues();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }
        public static decimal CalculateHoursWorked(long durationMs, decimal offsetHours)
        {
            if (durationMs <= 0)
                throw new ArgumentException("DurationMs must be greater than zero.", nameof(durationMs));

            // Convert duration to hours
            var durationHours = Math.Round(
                (decimal)durationMs / 1000m / 60m / 60m,
                2
            );

            // Actual hours worked = duration + offset
            var hoursWorked = durationHours + offsetHours;

            // Business rules:
            // Must be > 0
            if (hoursWorked <= 0)
                throw new InvalidOperationException(
                    $"HoursWorked must be greater than zero. Duration={durationHours}, Offset={offsetHours}, Result={hoursWorked}."
                );

            // Must not exceed Autotask's limit
            if (hoursWorked > 24m)
                throw new InvalidOperationException(
                    $"HoursWorked cannot exceed 24 hours. Duration={durationHours}, Offset={offsetHours}, Result={hoursWorked}."
                );

            return Math.Round(hoursWorked, 2);
        }


        /// <summary>
        /// Returns the allowed range for OffsetHours for a given DurationMs.
        /// </summary>
        public static (decimal MinOffset, decimal MaxOffset) GetAllowedOffsetRange(long durationMs)
        {
            if (durationMs <= 0)
                throw new ArgumentException("DurationMs must be greater than zero.", nameof(durationMs));

            var durationHours = Math.Round(
                (decimal)durationMs / 1000m / 60m / 60m,
                2
            );

            // HoursWorked = durationHours + offset
            // Constraints:
            //   durationHours + offset > 0   => offset > -durationHours
            //   durationHours + offset <= 24 => offset <= 24 - durationHours

            var minOffset = -durationHours + 0.01m; // smallest offset that still yields > 0
            var maxOffset = 24m - durationHours;    // largest offset that yields <= 24

            return (minOffset, maxOffset);
        }

        public static DateTimeOffset CalculateStartFromDuration(DateTimeOffset endDateTime, long durationMs)
        {
            if (durationMs <= 0)
                throw new ArgumentException("DurationMs must be greater than zero.", nameof(durationMs));

            var duration = TimeSpan.FromMilliseconds(durationMs);

            // This gives you a start time such that:
            // endDateTime - startDateTime == duration
            return endDateTime - duration;
        }

        protected async System.Threading.Tasks.Task PlayButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                if (_isRunning)
                    return;

                _isRunning = true;
                _lastTickUtc = DateTime.UtcNow;
                _stopwatchTimer?.Start();

                timeEntryRecord.TimeStampStatus = true;
                timeEntryRecord.StartDateTime = CalculateStartFromDuration(DateTimeOffset.Now, timeEntryRecord.DurationMs.Value); //DateTimeOffset.Now;
                timeEntryRecord.EndDateTime = DateTimeOffset.Now;
                timeEntryRecord.DateWorked = DateTimeOffset.Now;

                //if (!timeEntryRecord.StartDateTime.HasValue)
                //{
                //    timeEntryRecord.StartDateTime = CalculateStartFromDuration(DateTimeOffset.Now, timeEntryRecord.DurationMs.Value); //DateTimeOffset.Now;
                //}
                await UpdateTicketValues();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }

        }

        protected async System.Threading.Tasks.Task PauseButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                accordionSelectedIndex = 0;
                StateHasChanged();
                _isRunning = false;
                // Timer can keep running; we just ignore ticks when not running.
                // (Or call _stopwatchTimer?.Stop(); if you prefer.)
                timeEntryRecord.HoursWorked =
                    Math.Max(
                        Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2)
                        - (timeEntryRecord.OffsetHours ?? 0),
                        0
                    );
                timeEntryRecord.TimeStampStatus = false;
                timeEntryRecord.StartDateTime = CalculateStartFromDuration(DateTimeOffset.Now, timeEntryRecord.DurationMs.Value); //DateTimeOffset.Now;
                timeEntryRecord.EndDateTime = DateTimeOffset.Now;
                timeEntryRecord.DateWorked = DateTimeOffset.Now;
                await UpdateTicketValues();


            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task StopButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                _isRunning = false;

                timeEntryRecord.TimeStampStatus = false;
                timeEntryRecord.EndDateTime = DateTimeOffset.Now;
                timeEntryRecord.StartDateTime = CalculateStartFromDuration(DateTimeOffset.Now, timeEntryRecord.DurationMs.Value); //DateTimeOffset.Now;
                timeEntryRecord.DateWorked = DateTimeOffset.Now;

                timeEntryRecord.HoursWorked =
                    Math.Max(
                        Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2)
                        - (timeEntryRecord.OffsetHours ?? 0),
                        0
                    );
                await UpdateTicketValues();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task ClearTimerButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                _isRunning = false;
                timeEntryRecord.DurationMs = 0;
                timeEntryRecord.HoursWorked = 0;
                timeEntryRecord.StartDateTime = DateTimeOffset.Now;
                timeEntryRecord.EndDateTime = DateTimeOffset.Now;
                timeEntryRecord.DateWorked = DateTimeOffset.Now;

                StateHasChanged();

                timeEntryRecord.TimeStampStatus = false;
                await UpdateTicketValues();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });

            }
        }

        private void OnStopwatchElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_isRunning)
                return;

            var now = DateTime.UtcNow;
            var diff = now - _lastTickUtc;
            _lastTickUtc = now;

            var current = timeEntryRecord.DurationMs ?? 0;
            timeEntryRecord.DurationMs = current + (long)diff.TotalMilliseconds;

            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            if (_stopwatchTimer is not null)
            {
                _stopwatchTimer.Elapsed -= OnStopwatchElapsed;
                _stopwatchTimer.Dispose();
            }
        }


        protected async System.Threading.Tasks.Task statusChange(System.Object args)
        {
            try
            {
                var ticketUpdate = new TicketUpdateDto()
                {
                    Id = ticket.item.id,
                    Status = ticket.item.status,
                };
                await AutotaskTicketService.UpdateTicket(ticketUpdate);
                await UpdateTicketValues();


            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to Update Ticket Status.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task InsertTimeSummaryNotesButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            var timestamp = DateTime.Now.ToString("M/d/yyyy h:mm tt") + " - ";

            // If SummaryNotes is empty, set directly
            if (string.IsNullOrWhiteSpace(timeEntryRecord.SummaryNotes))
            {
                timeEntryRecord.SummaryNotes = timestamp;
                await UpdateTicketValues();

                return;
            }

            // Otherwise append as a new line
            timeEntryRecord.SummaryNotes += Environment.NewLine + timestamp;
            await UpdateTicketValues();

        }

        protected async System.Threading.Tasks.Task InsertTimeInternalNotesButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            var timestamp = DateTime.Now.ToString("M/d/yyyy h:mm tt") + " - ";

            // If SummaryNotes is empty, set directly
            if (string.IsNullOrWhiteSpace(timeEntryRecord.InternalNotes))
            {
                timeEntryRecord.InternalNotes = timestamp;
                await UpdateTicketValues();

                return;
            }

            // Otherwise append as a new line
            timeEntryRecord.InternalNotes += Environment.NewLine + timestamp;
            await UpdateTicketValues();

        }

        protected async System.Threading.Tasks.Task SaveAndCloseTicketButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            saveAndCloseTicket = true;
        }
    }
}