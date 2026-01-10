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
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [Inject]
        public ThreeCxClientService ThreeCxClientService { get; set; }

        protected CrownATTime.Server.Models.ATTime.TimeEntry timeEntryRecord { get; set; }
        protected TicketDtoResult ticket {  get; set; }
        protected ConfigurationItemResult configurationItem {  get; set; }
        protected ContactDtoResult contact {  get; set; }
        protected CompanyCache company {  get; set; }
        protected CompanyLocationDto companyLocation {  get; set; }
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
        protected string OtherNumber { get; set; }

        private System.Timers.Timer? _stopwatchTimer;
        private bool _isRunning;
        private bool saveAndCloseTicket = false;
        private DateTime _lastTickUtc;
        private string ElapsedFormatted => TimeSpan.FromMilliseconds((timeEntryRecord.DurationMs ?? 0)).ToString(@"hh\:mm\:ss");

        protected int accordionSelectedIndex { get; set; }
        private bool _openedAccordionOnce;
        protected bool isSaving {  get; set; }
        protected bool appendToResolution {  get; set; }

        protected string accountAlertTimeEntry {  get; set; }
        protected string accountAlertTicket {  get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> timeEntryTemplates;

        protected int timeEntryTemplatesCount;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_openedAccordionOnce && !pageLoading)
            {
                await Task.Delay(3000);
                _openedAccordionOnce = true;
                accordionSelectedIndex = -1;
                await InvokeAsync(StateHasChanged);
                accordionSelectedIndex = 1;
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
                if(ticket.item.contractID != null)
                {
                    contract = await ATTimeService.GetContractCacheById("", Convert.ToInt32(ticket.item.contractID));// await AutotaskTicketService.GetContract(ticket.item.contractID?? 0); //get from db
                }

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
                ticket = await AutotaskTicketService.GetTicket(ticket.item.id);
                timeEntryRecord.TicketTitle = ticket.item.title;
                contact = await AutotaskTicketService.GetContact(Convert.ToInt32(ticket.item.contactID));
                company = await ATTimeService.GetCompanyCacheById("", ticket.item.companyID);// await AutotaskTicketService.GetCompany(Convert.ToInt32(ticket.item.companyID));
                var accountAlerts = await AutotaskTicketService.GetAccountAlertsByCompanyId(company.Id);
                if (accountAlerts != null && accountAlerts.Items.Where(x => x.alertTypeID == 3).Any())
                {
                    accountAlertTimeEntry = accountAlerts.Items.FirstOrDefault(x => x.alertTypeID == 3).alertText;
                }
                configurationItem = await AutotaskTicketService.GetConfigurationItem(Convert.ToInt32(ticket.item.configurationItemID));
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
                timeEntryRecord.AccountName = company.CompanyName;
                timeEntryRecord.ContactName = contact.item == null ? "" : $"{contact.item.firstName} {contact.item.lastName}";
                timeEntryRecord.PriorityName = PriorityName;
                timeEntryRecord.StatusName = StatusName;
                timeEntryRecord.ResourceName = $"{resource.FirstName} {resource.LastName}";
                //timeEntryRecord.HoursWorked =
                //   Math.Max(
                //       Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2)
                //       + (timeEntryRecord.OffsetHours ?? 0),
                //       0
                //   );
                timeEntryRecord.HoursWorked =
                   Math.Max(
                       Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2), 0);
                timeEntryRecord.StartDateTime = CalculateStartFromDuration(DateTimeOffset.Now, timeEntryRecord.DurationMs.Value); //DateTimeOffset.Now;
                timeEntryRecord.EndDateTime = DateTimeOffset.Now;
                timeEntryRecord.DateWorked = DateTimeOffset.Now;
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                //NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved", Style = $"position: fixed; top: 20px;  left: 50%; transform: translateX(-50%);"  });
                if (ticket.item.companyLocationID.HasValue)
                {
                    companyLocation = await AutotaskTicketService.GetCompanyLocationByLocationId(Convert.ToInt32(ticket.item.companyLocationID.Value));
                }
                
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
                isSaving = true;
                if(timeEntryRecord.TimeStampStatus == true)
                {
                    //timer is still running
                    NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"The Timer is still running.  Please pause the timer before saving so it can calculate the hours worked." });

                }
                else
                {
                    //if ((timeEntryRecord.EndDateTime - timeEntryRecord.StartDateTime).Value.Duration() < TimeSpan.FromMinutes(1))
                    //{
                    //    timeEntryRecord.StartDateTime = timeEntryRecord.EndDateTime.Value.AddMinutes(-2);
                    //}
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
                        //OffsetHours = timeEntryRecord.OffsetHours,


                    };
                    var saveATTime = await AutotaskTimeEntryService.CreateTimeEntry(newATTimeEntry);
                    timeEntryRecord.IsCompleted = true;
                    timeEntryRecord.TimeStampStatus = false;
                    timeEntryRecord.AttimeEntryId = saveATTime.itemId;
                    await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                    ticket = await AutotaskTicketService.GetTicket(timeEntryRecord.TicketId);
                    if (appendToResolution)
                    {
                        ticket.item.resolution = appendToResolution ? $"{ticket.item.resolution}{Environment.NewLine}{timeEntryRecord.SummaryNotes}" : ticket.item.resolution;
                        await AutotaskTicketService.UpdateTicket(new TicketUpdateDto()
                        {
                            Id = ticket.item.id,
                            Status = ticket.item.status,
                            Resolution = ticket.item.resolution,
                        });
                    }


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
                isSaving = false;    
                    
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}" });
                isSaving = false;
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
            try
            {
                var calls = await ThreeCxClientService.MakeCall(contact.item.phone, resource.OfficeExtension);
                MonitorCallStatus(calls, contact.item.phone);

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to Make Call.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task CallContactMobileButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                var calls = await ThreeCxClientService.MakeCall(contact.item.mobilePhone, resource.OfficeExtension);
                MonitorCallStatus(calls, contact.item.mobilePhone);
                


            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to Make Call.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task CallOtherNumberButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                var calls = await ThreeCxClientService.MakeCall(OtherNumber, resource.OfficeExtension);
                MonitorCallStatus(calls, OtherNumber);

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to Make Call.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task OtherNumberChange(System.String args)
        {
            try
            {
                var calls = await ThreeCxClientService.MakeCall(OtherNumber, resource.OfficeExtension);
                MonitorCallStatus(calls, OtherNumber);

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to Make Call.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task CallCompanyPhoneButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                var calls = await ThreeCxClientService.MakeCall(company.Phone, resource.OfficeExtension);
                MonitorCallStatus(calls, contact.item.mobilePhone);



            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to Make Call.  Error: {ex.Message}" });

            }
        }

        protected async Task MonitorCallStatus(ThreeCxMakeCallResult.Calls calls, string phoneNumber)
        {
            try
            {
                var normalizedNumber = ThreeCxClientService.NormalizeUsPhone(phoneNumber);


                if (calls == null)
                {
                    // Call not found � handle gracefully
                    return;
                }
                //var call = calls.result;

                var callId = calls.result.callid;

                // Start fairly responsive, then back off so hours-long calls don't hammer the API.
                var delayMs = 5000;
                var maxDelayMs = 30000;
                var callStart = DateTime.Now;
                var callEnd = DateTime.Now;
                while (true)
                {
                    var status = await ThreeCxClientService.GetCallStatus(resource.OfficeExtension);

                    //var stillInCall =
                    //    status.participants.Where(p => p.callid == callId).Any() ?? true : false;

                    bool stillInCall;
                    if (status != null && status.participants.Any(x => x.callid == callId))
                    {
                        stillInCall = true;
                    }
                    else
                    {
                        stillInCall = false;
                    }

                    if (!stillInCall)
                    {
                        callEnd = DateTime.Now;
                        TimeSpan duration = callEnd - callStart;
                        int hours = (int)duration.TotalHours;
                        int minutes = duration.Minutes;
                        int seconds = duration.Seconds;

                        var timestamp = DateTime.Now.ToString("M/d/yyyy h:mm tt") + " - Outbound Call Details:" +
                            Environment.NewLine + $"Tech: {resource.FirstName} {resource.LastName} at extension {resource.OfficeExtension}" +
                            Environment.NewLine + $"Number Dialed: {normalizedNumber}" +
                            Environment.NewLine + $"Call Started: {callStart.ToString("M/d/yyyy h:mm tt")}" +
                            Environment.NewLine + $"Call Ended: {callEnd.ToString("M/d/yyyy h:mm tt")}" +
                            Environment.NewLine + $"Call Duration: {hours}h {minutes}m {seconds}s";

                        // If SummaryNotes is empty, set directly
                        if (string.IsNullOrWhiteSpace(timeEntryRecord.SummaryNotes))
                        {
                            timeEntryRecord.SummaryNotes = timestamp;
                            StateHasChanged();

                            await UpdateTicketValues();

                            return;
                        }

                        // Otherwise append as a new line
                        timeEntryRecord.SummaryNotes += Environment.NewLine + Environment.NewLine + timestamp;
                        StateHasChanged();

                        await UpdateTicketValues();

                        //add note here in autotask api

                        return; // call ended (no participants for this callId)

                    }

                    await Task.Delay(delayMs);

                    // backoff up to 30s
                    //delayMs = Math.Min(delayMs * 2, maxDelayMs);
                }
            }
            catch (Exception ex)
            {

            }
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

                            if (contractExclusion != null && contractExclusion.Items.Any())
                            {
                                

                                timeEntryRecord.IsNonBillable = false;
                                timeEntryRecord.ShowOnInvoice = true;
                                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Warning, Summary = $"Billable Notice", Detail = $"This work type is considered billable and not covered under the contract.", Duration = 10000 });

                            }
                            else
                            {
                                timeEntryRecord.IsNonBillable = true;
                                timeEntryRecord.ShowOnInvoice = false;
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
                timeEntryRecord.DurationMs = CalculateOffsetDurationMs(
                    timeEntryRecord.DurationMs.Value,
                    timeEntryRecord.OffsetHours.Value
                );
                //timeEntryRecord.HoursWorked = CalculateHoursWorked(
                //    timeEntryRecord.DurationMs.Value,
                //    timeEntryRecord.OffsetHours.Value
                //);
                //timeEntryRecord.HoursWorked =
                //    Math.Max(
                //        Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2)
                //        + (timeEntryRecord.OffsetHours ?? 0),
                //        0
                //    );
                //await Task.Delay(1);
                timeEntryRecord.OffsetHours = 0;

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

        public static long CalculateOffsetDurationMs(long durationMs, decimal offsetHours)
        {
            // Convert offset hours → milliseconds
            var offsetMs = (long)Math.Round(offsetHours * 60m * 60m * 1000m, MidpointRounding.AwayFromZero);

            var result = durationMs + offsetMs;

            // Never allow negative durations
            return Math.Max(0, result);
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
                return endDateTime;
            // throw new ArgumentException("DurationMs must be greater than zero.", nameof(durationMs));

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
                //timeEntryRecord.OffsetHours = 0;
                timeEntryRecord.TimeStampStatus = true;
                timeEntryRecord.StartDateTime = CalculateStartFromDuration(DateTimeOffset.Now, timeEntryRecord.DurationMs.Value); //DateTimeOffset.Now;
                timeEntryRecord.EndDateTime = DateTimeOffset.Now;
                timeEntryRecord.DateWorked = DateTimeOffset.Now;
                timeEntryRecord.OffsetHours = 0;

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
                StateHasChanged();
                _isRunning = false;
                // Timer can keep running; we just ignore ticks when not running.
                // (Or call _stopwatchTimer?.Stop(); if you prefer.)
                //timeEntryRecord.HoursWorked =
                //    Math.Max(
                //        Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2)
                //        - (timeEntryRecord.OffsetHours ?? 0),
                //        0
                //    );
                timeEntryRecord.TimeStampStatus = false;
                timeEntryRecord.StartDateTime = CalculateStartFromDuration(DateTimeOffset.Now, timeEntryRecord.DurationMs.Value); //DateTimeOffset.Now;
                timeEntryRecord.EndDateTime = DateTimeOffset.Now;
                timeEntryRecord.DateWorked = DateTimeOffset.Now;
                timeEntryRecord.OffsetHours = 0;
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

                //timeEntryRecord.HoursWorked =
                //    Math.Max(
                //        Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2)
                //        - (timeEntryRecord.OffsetHours ?? 0),
                //        0
                //    );
                timeEntryRecord.HoursWorked =
                    Math.Max(
                        Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2), 0);
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
                timeEntryRecord.OffsetHours = 0;
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


        protected async System.Threading.Tasks.Task DeviceButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"https://zinfandel.rmm.datto.com/device/{configurationItem.item.rmmDeviceID}");
            }
            catch (Exception ex)
            {

            }
        }


        protected async System.Threading.Tasks.Task WebRemoteButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"https://zinfandel.rmm.datto.com/web-remote/{configurationItem.item.rmmDeviceID}");
            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task AgentBrowserButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"https://zinfandel.centrastage.net/csm/device/startConnection/{configurationItem.item.rmmDeviceID}?connectionType=connect");
            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task SplashtopButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"https://zinfandel.centrastage.net/csm/device/startConnection/{configurationItem.item.rmmDeviceID}?connectionType=splashtopclient");
            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task ScreenConnectButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"{configurationItem.item.userDefinedFields.FirstOrDefault(x => x.name == "ScreenConnect Link").value}");
            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task SendEmailButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<NewEmail>($"New Email {ticket.item.ticketNumber} | {ticket.item.title}", new Dictionary<string, object>() { {"Ticket", ticket}, {"Contact", contact}, {"Resource", resource}, { "Company", company } } , new DialogOptions { Width = "800px", Draggable = true });
            await UpdateTicketValues();
            StateHasChanged();
            
        }

        protected async System.Threading.Tasks.Task AddNoteButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddNote>($"New Note {ticket.item.ticketNumber} | {ticket.item.title}", new Dictionary<string, object>() { {"Ticket", ticket}, {"Contact", contact}, {"Resource", resource}, {"Company", company} }, new DialogOptions { Width = "800px", Draggable = true });
        }


        protected async Task timeEntryTemplatesLoadData(LoadDataArgs args)
        {
            try
            {
                string defaultFilter = $"Active eq true and (ShareWithOthers eq true or TemplateAssignedTo eq '{Security.User.Email}')";
                var result = await ATTimeService.GetTimeEntryTemplates(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Title asc");

                timeEntryTemplates = result.Value.AsODataEnumerable();
                timeEntryTemplatesCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }

        protected async System.Threading.Tasks.Task TimeEntryTemplateChange(System.Object args)
        {
            try
            {
                if (Convert.ToInt32(args) != 0) 
                {
                    var template = await ATTimeService.GetTimeEntryTemplateByTimeEntryTemplateId("", Convert.ToInt32(args));
                    if (template != null)
                    {
                        timeEntryRecord.SummaryNotes = template.SummaryNotes;
                        timeEntryRecord.InternalNotes = template.InternalNotes;
                        timeEntryRecord.BillingCodeId = template.BillingCodeId;
                        ticket.item.status = template.TicketStatus.HasValue ? template.TicketStatus.Value : ticket.item.status;
                        appendToResolution = template.AppendToResolution;
                        await AutotaskTicketService.UpdateTicket(new TicketUpdateDto()
                        {
                            Id = ticket.item.id,
                            Status = ticket.item.status,
                        });
                        UpdateTicketValues();
                    }
                }
                
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to apply template" });

            }
        }

        protected async System.Threading.Tasks.Task DeleteTimeEntryButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                if(await DialogService.Confirm("Are you sure you want to delete this Time Entry?", "Delete Time Entry", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", ShowTitle = true, ShowClose = true }, null) == true)
                {
                    await ATTimeService.DeleteTimeEntry(timeEntryRecord.TimeEntryId);
                    await JSRuntime.InvokeVoidAsync(
                                "eval",
                                "window.open('', '_self'); window.close();"
                            );
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to delete time entry" });

            }
        }

        
    }
}