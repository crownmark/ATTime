using CrownATTime.Client;
using CrownATTime.Server.Models;
using CrownATTime.Server.Models.ATTime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;
using static CrownATTime.Server.Models.ITGlueDocumentsResult;
using static System.Net.WebRequestMethods;
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
        protected AutotaskService AutotaskService { get; set; }
        

        [Inject]
        public ATTimeService ATTimeService { get; set; }
        [Inject]
        public ITGlueService ITGlueService { get; set; }

        [Inject]
        public ThreeCxClientService ThreeCxClientService { get; set; }

        [Inject]
        public AiScenarioRunnerService AiScenarioRunnerService { get; set; }

        protected CrownATTime.Server.Models.ATTime.TimeEntry timeEntryRecord { get; set; }
        protected TicketDtoResult ticket {  get; set; }
        protected ConfigurationItemResult configurationItem {  get; set; }
        protected ContactDtoResult contact {  get; set; }
        protected CompanyCache company {  get; set; }
        protected CompanyLocationDto companyLocation {  get; set; }
        protected ContractCache contract {  get; set; }
        protected ResourceCache resource {  get; set; }
        protected ResourceCache ticketResource {  get; set; }
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

        protected List<TicketChecklistItemResult> TicketChecklistItemResult { get; set; }
        protected int checklistItemsCount { get; set; }
        protected RadzenDataGrid<TicketChecklistItemResult> grid0 { get; set; }
        protected bool gridLoading { get; set; }
        protected IEnumerable<AiPromptConfiguration> promptConfigurations {  get; set; }

        protected AiPromptConfiguration generalAiPromptConfiguration { get; set; } = new AiPromptConfiguration();

        protected bool summaryNotesAiBusy { get; set; }
        protected bool interalNotesAiBusy { get; set; }
        protected bool timeEntryAiBusy { get; set; }
        protected bool IsAiExpanded { get; set; }

        protected bool ChecklistItemsCollapsed { get; set; } = true;
        protected bool EmailNotesCollapsed { get; set; } = true;
        protected bool AIChatCollapsed { get; set; } = true;
        protected bool CompanyDetailsCollapsed { get; set; } = true;
        protected bool ContactDetailsCollapsed { get; set; } = true;
        protected bool RocketshipCollapsed { get; set; } = true;
        protected bool DeviceDetailsCollapsed { get; set; } = true;
        protected bool ItgluePasswordsCollapsed { get; set; } = true;
        protected bool ItglueDocumentsCollapsed { get; set; } = true;
        protected bool ItglueFlexibleAssetsCollapsed { get; set; } = true;
        protected bool ItglueConfigurationsCollapsed { get; set; } = true;
        protected bool LiveLinksCollapsed { get; set; } = true;

        protected List<ITGluePasswordAttributeResults> passwords { get; set; } = new List<ITGluePasswordAttributeResults>();
        protected int passwordsCount { get; set; }
        protected bool passwordsGridLoading { get; set; }
        protected RadzenDataGrid<ITGluePasswordAttributeResults> passwordsGrid { get; set; }

        protected string passwordSearch = "";

        protected List<ITGlueDocumentAttributesResults> documents { get; set; } = new List<ITGlueDocumentAttributesResults>();
        protected int documentsCount { get; set; }
        protected bool documentsGridLoading { get; set; }
        protected RadzenDataGrid<ITGlueDocumentAttributesResults> documentsGrid { get; set; }

        protected List<LiveLink> liveLinks { get; set; } = new List<LiveLink>();

        protected string documentsSearch = "";



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

                var aiPrompts = await ATTimeService.GetAiPromptConfigurations(filter: $"Active eq true and (SharedWithEveryone eq true or contains(SharedWithUsers, '{Security.User.Email}'))", orderby: $"MenuName", expand: $"TimeGuardSection");
                promptConfigurations = aiPrompts.Value.ToList();
                try
                {
                    if (resource.DefaultAitemplate.HasValue)
                    {
                        generalAiPromptConfiguration = promptConfigurations.Where(x => x.AiPromptConfigurationId == resource.DefaultAitemplate.Value).FirstOrDefault();
                    }
                    else
                    {
                        generalAiPromptConfiguration = promptConfigurations.Where(x => x.Name == "General AI Chat Prompt").FirstOrDefault();

                    }
                }
                catch (Exception ex)
                {

                }
                var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Email eq '{Security.User.Email}'");// await AutotaskService.GetLoggedInResource(Security.User.Email); //cache in db
                //var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Email eq 'jordan@ce-technology.com'");// await AutotaskService.GetLoggedInResource(Security.User.Email); //cache in db
                resource = resourceResult.Value.FirstOrDefault();
                if (resource != null)
                {
                    ChecklistItemsCollapsed = resource.ChecklistItemsCollapsed;
                    AIChatCollapsed = resource.AichatCollapsed;
                    CompanyDetailsCollapsed = resource.CompanyDetailsCollapsed;
                    ContactDetailsCollapsed = resource.ContactDetailsCollapsed;
                    DeviceDetailsCollapsed = resource.DeviceDetailsCollapsed;
                    EmailNotesCollapsed = resource.EmailNotesCollapsed;
                    RocketshipCollapsed = resource.RocketshipCollapsed;
                    ItglueDocumentsCollapsed = resource.ItglueDocumentsCollapsed;
                    ItgluePasswordsCollapsed = resource.ItgluePasswordsCollapsed;
                    ItglueFlexibleAssetsCollapsed = resource.ItglueFlexibleAssetsCollapsed;
                    ItglueConfigurationsCollapsed = resource.ItglueConfigurationsCollapsed;
                    LiveLinksCollapsed = resource.LiveLinksCollapsed;
                }
                var liveLinksResult = await ATTimeService.GetLiveLinks(filter: $"(contains(AssignedTo, '{resource.Email}') and Active eq true) or (ShareWithOthers eq true and Active eq true)");
                liveLinks = liveLinksResult.Value.ToList();
                var billingCodeItems = await ATTimeService.GetBillingCodeCaches(filter: $"IsActive eq true");// await AutotaskService.GetBillingCodes(); //cache in db
                billingCodes = billingCodeItems.Value.ToList();
                var roles = await ATTimeService.GetRoleCaches(filter: $"IsActive eq true");// await AutotaskService.GetRoles(); //cache in db
                var serviceDeskRoles = await ATTimeService.GetServiceDeskRoleCaches(filter: $"ResourceId eq {resource.Id} and IsActive eq true");// await AutotaskService.GetServiceDeskRoles(resource.id);
                mappedRoles = AutotaskService.MapToServiceDeskRoles(roles.Value.ToList(), serviceDeskRoles.Value.ToList(), true); //get from db
                var fields = await AutotaskService.GetTicketFields(); //cache in db
                //ticketEntityFields = fields.Fields;
                
                ticket = await AutotaskService.GetTicket(Convert.ToInt32(TicketId));
                var contractsResult = await ATTimeService.GetContractCaches(filter: $"CompanyId eq {ticket.item.companyID} and Status eq 1");// await AutotaskService.GetTicketContracts(ticket.item.companyID); //cache in db
                contracts = contractsResult.Value.ToList();
                if(ticket.item.contractID != null)
                {
                    contract = await ATTimeService.GetContractCacheById("", Convert.ToInt32(ticket.item.contractID));// await AutotaskService.GetContract(ticket.item.contractID?? 0); //get from db
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
                                    var contractExclusion = await AutotaskService.GetContractExclusionsBillingCode(Convert.ToInt32(newTimeEntry.ContractId), selectedBillingCode.Id);//cache in db

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
                    if (resource.DefaultTimeEntryTemplate.HasValue)
                    {
                        TimeEntryTemplateChange(resource.DefaultTimeEntryTemplate.Value);
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

        

        protected async System.Threading.Tasks.Task ProcessWorkflows(int workflowTriggerTypeId) 
        {
            try
            {
                string Escape(string s) => s.Replace("'", "''");

                string BuildUdfFilter(string nameField, string valueField, TicketDtoResult.Userdefinedfield[] userDefinedFields)
                {
                    var matches = userDefinedFields?
                        .Where(x => !string.IsNullOrWhiteSpace(x.name) && !string.IsNullOrWhiteSpace(x.value))
                        .Select(x => $"({nameField} eq '{Escape(x.name.Trim())}' and {valueField} eq '{Escape(x.value.Trim())}')")
                        .ToList() ?? new List<string>();

                    var slotUnused =
                        $"((({nameField} eq null) or ({nameField} eq '')) and (({valueField} eq null) or ({valueField} eq '')))";

                    if (!matches.Any())
                        return slotUnused;

                    return $"({slotUnused} or ({string.Join(" or ", matches)}))";
                }

                var udf1Filter = BuildUdfFilter("Udf1Name", "Udf1Value", ticket.item.userDefinedFields);
                var udf2Filter = BuildUdfFilter("Udf2Name", "Udf2Value", ticket.item.userDefinedFields);
                var udf3Filter = BuildUdfFilter("Udf3Name", "Udf3Value", ticket.item.userDefinedFields);

                var filter = string.Join(" and ", new[]
                {
                    $"(TicketCreatedBy eq '{ticket.item.createdByContactID}' or TicketCreatedBy eq null)",
                    $"(CompanyId eq {ticket.item.companyID} or CompanyId eq null)",
                    $"(StatusId eq {ticket.item.status} or StatusId eq null)",
                    $"(PriorityId eq {ticket.item.priority} or PriorityId eq null)",
                    $"(QueueId eq {ticket.item.queueID} or QueueId eq null)",
                    $"(TicketCategoryId eq {ticket.item.ticketCategory} or TicketCategoryId eq null)",
                    $"({(timeEntryRecord.HoursWorked ?? 0.0m)} ge TimeEntryHoursWorkedGreaterThan or TimeEntryHoursWorkedGreaterThan eq null)",
                    $"({(timeEntryRecord.HoursWorked ?? 0.0m)} le TimeEntryHoursWorkedLessThan or TimeEntryHoursWorkedLessThan eq null)",
                    (ticket.item.issueType != null
                        ? $"(IssueTypeId eq {ticket.item.issueType} or IssueTypeId eq null)"
                        : "(IssueTypeId eq null)"),
                    (ticket.item.subIssueType != null
                        ? $"(SubIssueTypeId eq {ticket.item.subIssueType} or SubIssueTypeId eq null)"
                        : "(SubIssueTypeId eq null)"),
                    udf1Filter,
                    udf2Filter,
                    udf3Filter,
                    $"(TimeEntryCreatedBy eq '{timeEntryRecord.ResourceId}' or TimeEntryCreatedBy eq null)",
                    $"(TicketAssignedTo eq '{ticket.item.assignedResourceID}' or TicketAssignedTo eq null)"
                });
                var workflowResult = await ATTimeService.GetWorkflowRules(filter: $"Active eq true and WorkflowTriggerTypeId eq {workflowTriggerTypeId} and {filter}", expand: "WorkflowSteps", orderby: $"RuleOrder");
                var workflowsList = workflowResult.Value.ToList();
                foreach (var workflow in workflowsList)
                {
                    foreach (var step in workflow.WorkflowSteps.Where(x => x.Active == true).OrderBy(x => x.StepOrder))
                    {
                        if (step.WorkflowStepTypeId == 1 && (string.IsNullOrEmpty(step.StepAssignedTo) || step.StepAssignedTo.Contains(Security.User.Email))) //email
                        {
                            try
                            {
                                await DialogService.OpenAsync<NewEmail>($"New Email {ticket.item.ticketNumber} | {ticket.item.title}", new Dictionary<string, object>() { { "Ticket", ticket }, { "Contact", contact }, { "Resource", resource }, { "Company", company }, { "TicketResource", ticketResource }, { "TimeEntry", timeEntryRecord }, { "EmailTemplateId", step.EmailTemplateId } }, new DialogOptions { Width = "800px", Draggable = true });
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else if (step.WorkflowStepTypeId == 2 && (string.IsNullOrEmpty(step.StepAssignedTo) || step.StepAssignedTo.Contains(Security.User.Email))) //note
                        {
                            try
                            {
                                await DialogService.OpenAsync<AddNote>($"New Note {ticket.item.ticketNumber} | {ticket.item.title}", new Dictionary<string, object>() { { "Ticket", ticket }, { "Contact", contact }, { "Resource", resource }, { "Company", company }, { "NoteTemplateId", step.NoteTemplateId } }, new DialogOptions { Width = "800px", Draggable = true });

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else if (step.WorkflowStepTypeId == 3 && (string.IsNullOrEmpty(step.StepAssignedTo) || step.StepAssignedTo.Contains(Security.User.Email))) //teams message
                        {
                            try
                            {
                                await DialogService.OpenAsync<NewTeamsMessage>($"New Teams Message {ticket.item.ticketNumber} | {ticket.item.title}", new Dictionary<string, object>() { { "Ticket", ticket }, { "Contact", contact }, { "Resource", resource }, { "Company", company }, { "TicketResource", ticketResource }, { "TeamsMessageTemplateId", step.TeamsMessageTemplateId } }, new DialogOptions { Width = "800px", Draggable = true });

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else if (step.WorkflowStepTypeId == 4 && (string.IsNullOrEmpty(step.StepAssignedTo) || step.StepAssignedTo.Contains(Security.User.Email))) //time entry template
                        {
                            try
                            {
                                await TimeEntryTemplateChange(step.TimeEntryTemplateId.Value);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (step.WorkflowStepTypeId == 5 && (string.IsNullOrEmpty(step.StepAssignedTo) || step.StepAssignedTo.Contains(Security.User.Email))) //confirmation dialog
                        {
                            var confirmed = await DialogService.Confirm(step.ConfirmationDialogMessage, step.ConfirmationDialogTitle, new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
                            if (!confirmed.HasValue || !confirmed.Value)
                            {
                                //user cancelled, stop processing workflow steps
                                if (step.ConfirmationDialogContinueOnNo)
                                {

                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else if (step.WorkflowStepTypeId == 6 && (string.IsNullOrEmpty(step.StepAssignedTo) || step.StepAssignedTo.Contains(Security.User.Email))) //notification dialog
                        {
                            await DialogService.Alert(step.NotificationDialogMessage, step.NotificationDialogTitle, new AlertOptions() { OkButtonText = "OK" });
                        }
                        else if (step.WorkflowStepTypeId == 7 && (string.IsNullOrEmpty(step.StepAssignedTo) || step.StepAssignedTo.Contains(Security.User.Email))) //n8n workflow
                        {
                            try
                            {
                                // Load picklists
                                var picklistResult = await ATTimeService.GetTicketEntityPicklistValueCaches();
                                var picklistRows = picklistResult?.Value ?? new List<TicketEntityPicklistValueCache>();

                                var picklists = EmailService.BuildPicklistMaps(picklistRows);

                                var ctx = new TemplateContext
                                {
                                    Contact = contact?.item,
                                    Ticket = ticket?.item,
                                    Resource = resource,
                                    TicketResource = ticketResource,
                                    Company = company,
                                    Picklists = picklists
                                };
                                step.N8nWorkflowUrl = EmailService.Render(step.N8nWorkflowUrl ?? string.Empty, ctx);

                                if (step.N8nWorkflowMethod == "GET")
                                {
                                    await RequestUrl(step.N8nWorkflowUrl, RequestMode.HttpGet, step);

                                }
                                else if (step.N8nWorkflowMethod == "POST")
                                {
                                    var json = JsonSerializer.Serialize(new
                                    {
                                        Ticket = ticket,
                                        TimeEntry = timeEntryRecord
                                    });
                                    await RequestUrl(step.N8nWorkflowUrl, RequestMode.HttpPostJson, json, step);

                                }
                                else if (step.N8nWorkflowMethod == "OPENURL")
                                {
                                    await RequestUrl(step.N8nWorkflowUrl, RequestMode.BrowserOpen, step);
                                }
                                else
                                {
                                    await RequestUrl(step.N8nWorkflowUrl, RequestMode.BrowserOpen, step);

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            
                        }
                        else if (step.WorkflowStepTypeId == 8 && (string.IsNullOrEmpty(step.StepAssignedTo) || step.StepAssignedTo.Contains(Security.User.Email))) //Datto RMM Job
                        {
                        }
                        else
                        {

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Workflow Error: {ex.Message} | {ex.StackTrace.ToString()}" });

            }
        }
        
        protected async System.Threading.Tasks.Task UpdateTicketValues()
        {
            try
            {
                await ProcessWorkflows(1); //time entry update

                timeEntryRecord.HoursWorked =
                  Math.Max(
                      Math.Round((timeEntryRecord.DurationMs.GetValueOrDefault() / 3_600_000m), 2), 0);
                timeEntryRecord.StartDateTime = CalculateStartFromDuration(DateTimeOffset.Now, timeEntryRecord.DurationMs.Value); //DateTimeOffset.Now;
                timeEntryRecord.EndDateTime = DateTimeOffset.Now;
                timeEntryRecord.DateWorked = DateTimeOffset.Now;
                ticket = await AutotaskService.GetTicket(ticket.item.id);
                timeEntryRecord.TicketTitle = ticket.item.title;
                contact = await AutotaskService.GetContact(Convert.ToInt32(ticket.item.contactID));
                company = await ATTimeService.GetCompanyCacheById("", ticket.item.companyID);
                if(ticket.item.assignedResourceID != null)
                {
                    var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Id eq {ticket.item.assignedResourceID}");
                    if (resourceResult.Value.Any())
                    {
                        ticketResource = resourceResult.Value.FirstOrDefault();
                    }
                }
                
                var accountAlerts = await AutotaskService.GetAccountAlertsByCompanyId(company.Id);
                if (accountAlerts != null && accountAlerts.Items.Where(x => x.alertTypeID == 3).Any())
                {
                    accountAlertTimeEntry = accountAlerts.Items.FirstOrDefault(x => x.alertTypeID == 3).alertText;
                }
                configurationItem = await AutotaskService.GetConfigurationItem(Convert.ToInt32(ticket.item.configurationItemID));
                var picklistValues = await ATTimeService.GetTicketEntityPicklistValueCaches();
                var picklistValuesList = picklistValues.Value.ToList();
                var statuses = picklistValuesList.Where(x => x.PicklistName == "status");
                //move to db for easier maintenance
               // var allowedIds = new HashSet<int> { 1, 7, 8, 10, 12, 23, 29, 27, 30, 32, 33, 34, 46, 47, 57 }; // example status IDs
                var allowedIdsResult = await ATTimeService.GetAllowedTicketStatuses();
                var allowedIds = allowedIdsResult.Value.Select(x => x.TicketStatusId).ToHashSet();
                var filtered = statuses
                    .Where(s => s.ValueInt.HasValue && allowedIds.Contains(s.ValueInt.Value)).OrderBy(x => x.Label)
                    .ToList();

                ticketStatuses = filtered;
                var status = statuses.Where(x => x.Value == ticket.item.status.ToString()).FirstOrDefault();
                StatusName = status.Label;
                var priorities = picklistValuesList.Where(x => x.PicklistName == "priority");
                var priority = priorities.Where(x => x.Value == ticket.item.priority.ToString()).FirstOrDefault();
                PriorityName = priority.Label;
                //var ticketLookupFields = await AutotaskService.GetTicketFields();
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
               
                await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                //NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Success", Detail = $"Time Entry Saved", Style = $"position: fixed; top: 20px;  left: 50%; transform: translateX(-50%);"  });
                if (ticket.item.companyLocationID.HasValue)
                {
                    companyLocation = await AutotaskService.GetCompanyLocationByLocationId(Convert.ToInt32(ticket.item.companyLocationID.Value));
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
                    await ProcessWorkflows(2); //time entry completing

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
                        ShowOnInvoice = timeEntryRecord.ShowOnInvoice,
                        //OffsetHours = timeEntryRecord.OffsetHours,


                    };
                    var saveATTime = await AutotaskService.CreateTimeEntry(newATTimeEntry);
                    timeEntryRecord.IsCompleted = true;
                    timeEntryRecord.TimeStampStatus = false;
                    timeEntryRecord.AttimeEntryId = saveATTime.itemId;
                    await ATTimeService.UpdateTimeEntry(timeEntryRecord.TimeEntryId, timeEntryRecord);
                    ticket = await AutotaskService.GetTicket(timeEntryRecord.TicketId);
                    if (appendToResolution)
                    {
                        ticket.item.resolution = appendToResolution ? $"{ticket.item.resolution}{Environment.NewLine}{timeEntryRecord.SummaryNotes}" : ticket.item.resolution;
                        await AutotaskService.UpdateTicket(new TicketUpdateDto()
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
                            await ProcessWorkflows(3); //ticket completing

                            await DialogService.OpenAsync<CloseTicketDialog>($"Close Ticket Dialog | {ticket.item.title}", new Dictionary<string, object>() { { "TicketId", timeEntryRecord.TicketId }, { "TimeEntryId", timeEntryRecord.TimeEntryId }, {"Ticket", ticket } }, new DialogOptions { Width = "800px", Resizable = true, Draggable = true });
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
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to save Time Entry.  Error: {ex.Message}", Duration = 5000 });
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
                MonitorCallStatus(calls, company.Phone);



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
                            var contractExclusion = await AutotaskService.GetContractExclusionsBillingCode(Convert.ToInt32(timeEntryRecord.ContractId.Value), selectedBillingCode.Id);

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
                            var contractExclusion = await AutotaskService.GetContractExclusionsBillingCode(Convert.ToInt32(timeEntryRecord.ContractId.Value), selectedBillingCode.Id);

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

        //decimal OffsetHours { get; set; }   // 🔴 Single source of truth

        //int Hours
        //{
        //    get => (int)Math.Floor(OffsetHours);
        //}

        //int Minutes
        //{
        //    get => (int)Math.Round((OffsetHours - Hours) * 60);
        //}

        //void OnHoursChanged(int value)
        //{
        //    OffsetHours = value + (Minutes / 60m);
        //}

        //void OnMinutesChanged(int value)
        //{
        //    OffsetHours = Hours + (value / 60m);            
        //}

        protected async System.Threading.Tasks.Task TimerOffsetButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            timeEntryRecord.DurationMs = CalculateOffsetDurationMs(
                    timeEntryRecord.DurationMs.Value,
                    OffsetHours
                );
            OffsetHours = 0;
            await UpdateTicketValues();
        }

        decimal OffsetHours { get; set; } = 0m; // single source of truth

        int TotalMinutes => (int)Math.Round(OffsetHours * 60m);

        // Signed hours in [-24..24]
        int Hours => ClampInt((int)Math.Truncate(TotalMinutes / 60m), -24, 24);

        // Signed minutes in [-59..59]
        int Minutes
        {
            get
            {
                var h = (int)Math.Truncate(TotalMinutes / 60m);
                var m = TotalMinutes - (h * 60);
                return ClampInt(m, -59, 59);
            }
            
        }
        protected bool isAiExpanded { get; set; }

        void OnHoursChanged(int newHours)
        {
            newHours = ClampInt(newHours, -24, 24);
            SetFromParts(newHours, Minutes);
        }

        void OnMinutesChanged(int newMinutes)
        {
            newMinutes = ClampInt(newMinutes, -59, 59);
            SetFromParts(Hours, newMinutes);
        }

        void SetFromParts(int hours, int minutes)
        {
            // Combine
            var total = (hours * 60) + minutes;

            // Clamp overall to [-24:00, +24:00]
            total = ClampInt(total, -24 * 60, 24 * 60);

            // Normalize so minutes always ends up in [-59..59] with matching hour
            var normHours = (int)Math.Truncate(total / 60m);
            var normMins = total - (normHours * 60);

            // Final safety clamp
            normHours = ClampInt(normHours, -24, 24);
            normMins = ClampInt(normMins, -59, 59);

            OffsetHours = ((normHours * 60) + normMins) / 60m;
        }

        static int ClampInt(int value, int min, int max)
            => value < min ? min : (value > max ? max : value);


        protected async System.Threading.Tasks.Task OffsetHoursChange(decimal? args)
        {
            try
            {
                
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
                await AutotaskService.UpdateTicket(ticketUpdate);
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
        protected async System.Threading.Tasks.Task ScreenConnectButtonBackstageClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"{configurationItem.item.userDefinedFields.FirstOrDefault(x => x.name == "ScreenConnect Link").value}WithOptions");
            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task SendEmailButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<NewEmail>($"New Email {ticket.item.ticketNumber} | {ticket.item.title}", new Dictionary<string, object>() { {"Ticket", ticket}, {"Contact", contact}, {"Resource", resource}, { "Company", company }, { "TicketResource", ticketResource }, { "TimeEntry", timeEntryRecord }, { "EmailTemplateId", null } } , new DialogOptions { Width = "800px", Draggable = true });
            await UpdateTicketValues();
            StateHasChanged();
            
        }

        protected async System.Threading.Tasks.Task AddNoteButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddNote>($"New Note {ticket.item.ticketNumber} | {ticket.item.title}", new Dictionary<string, object>() { {"Ticket", ticket}, {"Contact", contact}, {"Resource", resource}, {"Company", company}, { "NoteTemplateId", null } }, new DialogOptions { Width = "800px", Draggable = true });
        }


        protected async Task timeEntryTemplatesLoadData(LoadDataArgs args)
        {
            try
            {
                string defaultFilter = $"Active eq true and (ShareWithOthers eq true or contains(TemplateAssignedTo,'{Security.User.Email}'))";
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
                        if (template.Minutes.HasValue)
                        {
                            OnMinutesChanged(template.Minutes.Value);
                            await TimerOffsetButtonClick(new MouseEventArgs());
                        }
                        StateHasChanged();

                        await AutotaskService.UpdateTicket(new TicketUpdateDto()
                        {
                            Id = ticket.item.id,
                            Status = ticket.item.status,
                        });
                        if(template.EmailTemplateId.HasValue)
                        {
                            await DialogService.OpenAsync<NewEmail>($"New Email {ticket.item.ticketNumber} | {ticket.item.title}", new Dictionary<string, object>() { {"Ticket", ticket}, {"Contact", contact}, {"Resource", resource}, { "Company", company }, { "TicketResource", ticketResource }, { "TimeEntry", timeEntryRecord }, { "EmailTemplateId", template.EmailTemplateId } } , new DialogOptions { Width = "800px", Draggable = true });
                        }
                        if (template.TeamsMessageTemplateId.HasValue)
                        {
                            await DialogService.OpenAsync<NewTeamsMessage>($"New Teams Message {ticket.item.ticketNumber} | {ticket.item.title}", new Dictionary<string, object>() { { "Ticket", ticket }, { "Contact", contact }, { "Resource", resource }, { "Company", company }, { "TicketResource", ticketResource }, { "TeamsMessageTemplateId", template.TeamsMessageTemplateId } }, new DialogOptions { Width = "800px", Draggable = true });
                        }
                        StateHasChanged();

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

        

        

        protected async System.Threading.Tasks.Task PlayButton0MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Start/Resume Timer", new TooltipOptions() { Duration = null });
        }

        protected async System.Threading.Tasks.Task PlayButton0MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }

        protected async System.Threading.Tasks.Task PauseButton1MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Pause Timer", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task PauseButton1MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task ClearButton2MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Clear Timer", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task ClearButton2MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task UpdateTimerButton6MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Update Timer", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task UpdateTimerButton6MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task DeleteTimeEntryButton8MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Delete Time Entry", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task DeleteTimeEntryButton8MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }



        protected async System.Threading.Tasks.Task SummaryNoteButton4MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Insert Time Stamp in Summary Notes", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task SummaryNoteButton4MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task InternalNotesButton5MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Insert Time Stamp in Internal Notes", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task InternalNotesButton5MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task CompleteTimeEntryButton6MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Complete Time Entry", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task CompleteTimeEntryButton6MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task CloseTicketButton7MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Complete Time Entry & Close Ticket", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task CloseTicketButton7MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task ContactLink1MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Open Contact in Autotask", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task ContactLink1MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task CompanyLink2MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Open Company in Autotask", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task CompanyLink2MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        protected async System.Threading.Tasks.Task TicketLink0MouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Open Ticket in Autotask", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task TicketLink0MouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();

        }

        

        protected async System.Threading.Tasks.Task ViewTicketDescriptionButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            DialogService.OpenSide<TicketDetails>("Ticket Details", new Dictionary<string, object>() { { "Ticket", ticket }, { "PriorityName", PriorityName }, { "StatusName", StatusName }, { "PrimaryResource", ticketResource?.FullName }, { "ResourceId", resource.Id } }, new SideDialogOptions() { Position = DialogPosition.Right, ShowMask = false, Resizable = true, CloseDialogOnOverlayClick = true, Width = "800px" });
            //DialogService.Open<TicketDetails>("Ticket Details", new Dictionary<string, object>() { { "Ticket", ticket }, { "PriorityName", PriorityName }, { "StatusName", StatusName }, { "PrimaryResource", ticketResource?.FullName } }, new DialogOptions() {Draggable = true, Resizable = true,  CloseDialogOnOverlayClick = true, Width = "800px" });

        }

        protected async System.Threading.Tasks.Task TicketDetailsButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Open Ticket Details in Side Panel", new TooltipOptions() { Duration = null });
            
        }

        protected async System.Threading.Tasks.Task TicketDetailsButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }
        

        protected async System.Threading.Tasks.Task DataGrid0LoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                gridLoading = true;
                TicketChecklistItemResult = await AutotaskService.GetOpenTicketChecklistItems(ticket.item.id);
                TicketChecklistItemResult = TicketChecklistItemResult.OrderBy(x => x.position).ToList();
                checklistItemsCount = TicketChecklistItemResult.Count;
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
                item.completedByResourceID = resource.Id;
                var response = await AutotaskService.UpdateTicketChecklistItem(item);
                var timestamp = DateTime.Now.ToString("M/d/yyyy h:mm tt") + " - ";

                // If SummaryNotes is empty, set directly
                if (string.IsNullOrWhiteSpace(timeEntryRecord.SummaryNotes))
                {
                    timeEntryRecord.SummaryNotes = timestamp + $"Completed: {item.itemName}";
                }
                else
                {
                    // Otherwise append as a new line
                    timeEntryRecord.SummaryNotes += Environment.NewLine + timestamp + $"Completed: {item.itemName}";
                }
                await grid0.Reload();
                UpdateTicketValues();

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
            await DialogService.OpenAsync<AddChecklistItem>("Add Checklist Item", new Dictionary<string, object>() { { "TicketId", ticket.item.id } }, new DialogOptions { Draggable = true });
            await grid0.Reload();

        }

        protected async System.Threading.Tasks.Task DeleteButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, TicketChecklistItemResult item)
        {
            try
            {
                item.isBusy = true;
                await AutotaskService.DeleteChecklistItem(item);
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

        protected async System.Threading.Tasks.Task AISummaryNotesButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            try
            {
                var prompt = await ATTimeService.GetAiPromptConfigurations();

                var aiResponse = await AiScenarioRunnerService.RunAsync(prompt.Value.FirstOrDefault(), timeEntryRecord.SummaryNotes);
                if (!string.IsNullOrEmpty(aiResponse)) 
                {
                    timeEntryRecord.SummaryNotes = aiResponse;
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"AI Summary Error", Detail = $"{ex.Message}" });

            }
            //DialogService.OpenSide<AINoteAssistant>("AI Note Assistant", new Dictionary<string, object>() { {"Note", timeEntryRecord.SummaryNotes} }, new SideDialogOptions() { ShowClose = true, ShowTitle = true, CloseDialogOnOverlayClick = true, Width = "500px"});
        }

        protected async System.Threading.Tasks.Task AISummaryNotesButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Summarize Note (Client Facing)", new TooltipOptions() { Duration = null });

        }

        protected async System.Threading.Tasks.Task AISummaryNotesButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }

        protected async System.Threading.Tasks.Task AISummaryNotesSplitButtonClick(Radzen.Blazor.RadzenSplitButtonItem args)
        {
            try
            {
                if (args != null)
                {
                    summaryNotesAiBusy = true;
                    var prompt = await ATTimeService.GetAiPromptConfigurationByAiPromptConfigurationId("TimeGuardSection", Convert.ToInt32(args.Value));

                    var aiResponse = await AiScenarioRunnerService.RunAsync(prompt, timeEntryRecord.SummaryNotes);
                    if (!string.IsNullOrEmpty(aiResponse))
                    {
                        timeEntryRecord.SummaryNotes = aiResponse;
                    }
                    summaryNotesAiBusy = false;

                }

            }
            catch (Exception ex)
            {
                summaryNotesAiBusy = false;

                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"AI Error", Detail = $"{ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task AIInternalNotesSplitButtonClick(Radzen.Blazor.RadzenSplitButtonItem args)
        {
            try
            {
                if(args != null)
                {
                    interalNotesAiBusy = true;  
                    var prompt = await ATTimeService.GetAiPromptConfigurationByAiPromptConfigurationId("TimeGuardSection", Convert.ToInt32(args.Value));

                    var aiResponse = await AiScenarioRunnerService.RunAsync(prompt, timeEntryRecord.InternalNotes);
                    if (!string.IsNullOrEmpty(aiResponse))
                    {
                        timeEntryRecord.InternalNotes = aiResponse;
                    }
                    interalNotesAiBusy = false;

                }
            }
            catch (Exception ex)
            {
                interalNotesAiBusy = false;

                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"AI Error", Detail = $"{ex.Message}" });

            }
        }

        

        protected async System.Threading.Tasks.Task AddTeamsMessageButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<NewTeamsMessage>("New Teams Message", new Dictionary<string, object>() { { "Ticket", ticket }, { "Contact", contact }, { "Resource", resource }, { "Company", company }, { "TicketResource", ticketResource }, { "TeamsMessageTemplateId", null } }, new DialogOptions { Width = "800px" });
        }

        protected async System.Threading.Tasks.Task AiPopoutButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            if(IsAiExpanded)
            {
                TooltipService.Open(args, "Restore AI assistant", new TooltipOptions() { Duration = null });

            }
            else
            {
                TooltipService.Open(args, "Pop out AI assistant", new TooltipOptions() { Duration = null });

            }

        }

        protected async System.Threading.Tasks.Task AiPopoutButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }

        protected async System.Threading.Tasks.Task PasswordSearchTextBoxChange(System.String args)
        {
            await passwordsGrid.GoToPage(0);

            await passwordsGrid.Reload();
        }
        protected async System.Threading.Tasks.Task PasswordsDataGridLoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                passwordsGridLoading = true;
                var itGlueOrg = await ITGlueService.GetITGlueOrgIdByCompanyName(company.CompanyName);
                passwords = await ITGlueService.GetITGluePasswordsByOrganizationId(itGlueOrg);
                passwords = passwords
                    .Where(x =>
                        (x.name?.Contains(passwordSearch, StringComparison.OrdinalIgnoreCase) == true) ||
                        (x.notes?.Contains(passwordSearch, StringComparison.OrdinalIgnoreCase) == true))
                    .OrderBy(x => x.name)
                    .ToList();
                passwordsGridLoading = false;
            }
            catch (Exception ex)
            {
                passwordsGridLoading = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load passwords.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task DocumentsSearchTextBoxChange(System.String args)
        {
            await documentsGrid.GoToPage(0);

            await documentsGrid.Reload();
        }
        protected async System.Threading.Tasks.Task DocumentsDataGridLoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                documentsGridLoading = true;
                var itGlueOrg = await ITGlueService.GetITGlueOrgIdByCompanyName(company.CompanyName);
                documents = await ITGlueService.GetITGlueDocumentsByOrganizationId(itGlueOrg);
                documents = documents
                    .Where(x =>
                        (x.Name?.Contains(documentsSearch, StringComparison.OrdinalIgnoreCase) == true))
                    .OrderBy(x => x.Name)
                    .ToList();
                var crownitGlueOrg = await ITGlueService.GetITGlueOrgIdByCompanyName("Crown Enterprises");
                var kbdocuments = await ITGlueService.GetITGlueDocumentsByOrganizationAndFolderId(crownitGlueOrg, "3152508");
                kbdocuments = kbdocuments
                    .Where(x =>
                        (x.Name?.Contains(documentsSearch, StringComparison.OrdinalIgnoreCase) == true))
                    .OrderBy(x => x.Name)
                    .ToList();
                documents.AddRange(kbdocuments);

                documentsGridLoading = false;
            }
            catch (Exception ex)
            {
                documentsGridLoading = false;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load documents.  Error: {ex.Message}" });

            }
        }
        protected async System.Threading.Tasks.Task OpenITGlueDocumentButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, ITGlueDocumentAttributesResults data)
        {
            try
            {
                //var orgId = await ITGlueService.GetITGlueOrgIdByCompanyName(company.CompanyName);

                //var doc = await ITGlueService.GetITGluePasswordByPasswordId(orgId, data.id);
                //var folderId = data.DocumentFolderId == null ? data.OrganizationId : data.DocumentFolderId;
                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"{data.ResourceUrl}");

            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task ViewDocumentInITGlueButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "View Document in ITGlue", new TooltipOptions() { Duration = null });
        }

        protected async System.Threading.Tasks.Task ViewDocumentInITGlueButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }

        protected async System.Threading.Tasks.Task RefreshDocumentsDataGridButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await documentsGrid.Reload();
        }

        protected async System.Threading.Tasks.Task CopyUserNameButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, ITGluePasswordAttributeResults data)
        {
            try
            {

                await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", data.username);
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success", Detail = "Username Copied to Clipboard" });
            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task CopyPasswordButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, ITGluePasswordAttributeResults data)
        {
            try
            {
                var orgId = await ITGlueService.GetITGlueOrgIdByCompanyName(company.CompanyName);

                var password = await ITGlueService.GetITGluePasswordByPasswordId(orgId, data.id);
                await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", password.data.attributes.password);
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success", Detail = "Password Copied to Clipboard" });

            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task OpenPasswordButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, ITGluePasswordAttributeResults data)
        {
            try
            {

                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"{data.url}");

            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task OpenITGluePasswordButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, ITGluePasswordAttributeResults data)
        {
            try
            {
                var orgId = await ITGlueService.GetITGlueOrgIdByCompanyName(company.CompanyName);

                var password = await ITGlueService.GetITGluePasswordByPasswordId(orgId, data.id);
                await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"https://crown.itglue.com/{orgId}/passwords/{password.data.id}");

            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task RefreshPasswordsDataGridButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await passwordsGrid.Reload();
        }

        protected async System.Threading.Tasks.Task ViewInITGlueButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "View Password in ITGlue", new TooltipOptions() { Duration = null });
        }

        protected async System.Threading.Tasks.Task ViewInITGlueButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }

        protected async System.Threading.Tasks.Task CopyUsernameButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Copy Username", new TooltipOptions() { Duration = null });
        }

        protected async System.Threading.Tasks.Task CopyUsernameButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }

        protected async System.Threading.Tasks.Task CopyPasswordButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Copy Password", new TooltipOptions() { Duration = null });
        }

        protected async System.Threading.Tasks.Task CopyPasswordButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }
        protected async System.Threading.Tasks.Task OpenURLButtonMouseEnter(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Open(args, "Open URL", new TooltipOptions() { Duration = null });
        }

        protected async System.Threading.Tasks.Task OpenURLButtonMouseLeave(Microsoft.AspNetCore.Components.ElementReference args)
        {
            TooltipService.Close();
        }


        protected async System.Threading.Tasks.Task LiveLinkSplitButtonClick(Radzen.Blazor.RadzenSplitButtonItem args)
        {
            try
            {
                if(args != null)
                {
                    var liveLink = await ATTimeService.GetLiveLinkByLiveLinkId("", Convert.ToInt32(args.Value));
                    if(liveLink != null)
                    {
                        // Load picklists
                        var picklistResult = await ATTimeService.GetTicketEntityPicklistValueCaches();
                        var picklistRows = picklistResult?.Value ?? new List<TicketEntityPicklistValueCache>();

                        var picklists = EmailService.BuildPicklistMaps(picklistRows);

                        var ctx = new TemplateContext
                        {
                            Contact = contact?.item,
                            Ticket = ticket?.item,
                            Resource = resource,
                            TicketResource = ticketResource,
                            Company = company,
                            Picklists = picklists
                        };
                        liveLink.Url = EmailService.Render(liveLink.Url ?? string.Empty, ctx);

                        //await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"{liveLink.Url}");
                        // Example: perform an HTTP GET to the rendered URL (replace with RequestMode.BrowserOpen or RequestMode.HttpPostJson as needed)
                        if(liveLink.HttpMethod == "GET")
                        {
                            if (liveLink.RequiresConfirmationToRun)
                            {
                                if(await DialogService.Confirm("Are you sure you want to run this live link?", $"Confirmation: {liveLink.Title}", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", ShowTitle = true, ShowClose = false }, null) == true)
                                {
                                    var json = JsonSerializer.Serialize(ticket);
                                    await RequestUrl(liveLink.Url, RequestMode.HttpGet, json, null, liveLink);

                                }

                            }
                            else
                            {
                                var json = JsonSerializer.Serialize(ticket);
                                await RequestUrl(liveLink.Url, RequestMode.HttpGet, json, null, liveLink);

                            }
                        }
                        else if(liveLink.HttpMethod == "POST")
                        {
                            if (liveLink.RequiresConfirmationToRun)
                            {
                                if (await DialogService.Confirm("Are you sure you want to run this live link?", $"Confirmation: {liveLink.Title}", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", ShowTitle = true, ShowClose = false }, null) == true)
                                {
                                    var json = JsonSerializer.Serialize(ticket);
                                    await RequestUrl(liveLink.Url, RequestMode.HttpPostJson, json, null, liveLink);

                                }
                            }
                            else
                            {
                                var json = JsonSerializer.Serialize(ticket);
                                await RequestUrl(liveLink.Url, RequestMode.HttpPostJson, json, null, liveLink);
                            }
                                

                        }
                        else if (liveLink.HttpMethod == "OPENURL")
                        {
                            if (liveLink.RequiresConfirmationToRun)
                            {
                                if (await DialogService.Confirm("Are you sure you want to run this live link?", "Live Link Confirmation", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", ShowTitle = true, ShowClose = false }, null) == true)
                                {
                                    await RequestUrl(liveLink.Url, RequestMode.BrowserOpen, null);

                                }
                            }
                            else
                            {
                                await RequestUrl(liveLink.Url, RequestMode.BrowserOpen, null);

                            }

                        }
                        else 
                        {
                            if (liveLink.RequiresConfirmationToRun)
                            {
                                if (await DialogService.Confirm("Are you sure you want to run this live link?", "Live Link Confirmation", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", ShowTitle = true, ShowClose = false }, null) == true)
                                {
                                    await RequestUrl(liveLink.Url, RequestMode.BrowserOpen);

                                }
                            }                                
                            else
                            {
                                await RequestUrl(liveLink.Url, RequestMode.BrowserOpen);

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        // New: mode for how to handle the URL
        protected enum RequestMode
        {
            BrowserOpen,
            HttpGet,
            HttpPostJson
        }

        // New: helper to either open the URL in a browser or perform HTTP requests
        protected async Task RequestUrl(string url, RequestMode mode = RequestMode.BrowserOpen, object payload = null, WorkflowStep step = null, LiveLink liveLink = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "URL is empty." });
                    return;
                }
                var _httpClient = new HttpClient();

                switch (mode)
                {
                    case RequestMode.BrowserOpen:
                        await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), url);
                        break;

                    case RequestMode.HttpGet:
                        BusyDialog("Performing Task.  Please Wait...");
                        // Note: CORS must be allowed by the remote server for WASM HttpClient.
                        var getResponse = await _httpClient.GetAsync(url);
                        DialogService.Close();
                        if (!getResponse.IsSuccessStatusCode)
                        {
                            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = $"Error: {step?.N8nWorkflowNotificationTitle}", Detail = $"{(int)getResponse.StatusCode} {getResponse.ReasonPhrase}" });
                            return;
                        }
                        var getBody = await getResponse.Content.ReadAsStringAsync();
                        if (step != null && step.N8nWorkflowNotificationType == "Notification")
                        {
                            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = $"{step?.N8nWorkflowNotificationTitle}", Detail = $"{getResponse}" });

                        }
                        else if (step != null && step.N8nWorkflowNotificationType == "Alert")
                        {
                            //await DialogService.Alert((RenderFragment)(builder =>
                            //        builder.AddMarkupContent(0, getBody)
                            //    ), $"{step?.N8nWorkflowNotificationTitle}", null, null);

                            await DialogService.OpenAsync(
                                $"{step?.N8nWorkflowNotificationTitle}",
                                ds => builder =>
                                {
                                    builder.OpenElement(0, "div");
                                    builder.AddMarkupContent(1, getBody);
                                    builder.CloseElement();
                                }, new DialogOptions { ShowTitle = true, ShowClose = true, Width = $"{step.N8nWorkflowNotificationWindowWidth}" },
                                null
                            );

                        }
                        else if (step != null && step.N8nWorkflowNotificationType == "Confirmation")
                        {
                            await DialogService.Confirm((RenderFragment)(builder =>
                                    builder.AddMarkupContent(0, getBody)
                                ), $"{step?.N8nWorkflowNotificationTitle}", null, null);

                        }
                        else
                        {
                            if(liveLink != null)
                            {
                                if (!string.IsNullOrEmpty(liveLink.DialogWindowWidth))
                                {
                                    await DialogService.OpenAsync(
                                        $"{liveLink?.Title}",
                                        ds => builder =>
                                        {
                                            builder.OpenElement(0, "div");
                                            builder.AddMarkupContent(1, getBody);
                                            builder.CloseElement();
                                        }, new DialogOptions { ShowTitle = true, ShowClose = true, Width = $"{liveLink.DialogWindowWidth}" },
                                        null
                                    );
                                }
                                else
                                {
                                    NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = $"Success: {liveLink?.Title}", Detail = $"{(int)getResponse.StatusCode} {getResponse.ReasonPhrase}" });

                                }
                            }
                            else
                            {
                                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = $"Success:", Detail = $"{(int)getResponse.StatusCode} {getResponse.ReasonPhrase}" });

                            }

                        }
                        break;

                    case RequestMode.HttpPostJson:
                        BusyDialog("Performing Task.  Please Wait...");

                        var postResponse = await _httpClient.PostAsJsonAsync(url, payload ?? new { });
                        DialogService.Close();

                        if (!postResponse.IsSuccessStatusCode)
                        {
                            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = $"Error: {step?.N8nWorkflowNotificationTitle}", Detail = $"{(int)postResponse.StatusCode} {postResponse.ReasonPhrase}" });
                            return;
                        }
                        var postBody = await postResponse.Content.ReadAsStringAsync();
                        if (step != null && step.N8nWorkflowNotificationType == "Notification")
                        {
                            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = $"{step?.N8nWorkflowNotificationTitle}", Detail = $"{postBody}" });

                        }
                        else if (step != null && step.N8nWorkflowNotificationType == "Alert")
                        {
                            //await DialogService.Alert((RenderFragment)(builder =>
                            //        builder.AddMarkupContent(0, postBody)
                            //    ), $"{step?.N8nWorkflowNotificationTitle}", null, null);

                            
                            await DialogService.OpenAsync(
                                $"{step?.N8nWorkflowNotificationTitle}",
                                ds => builder =>
                                {
                                    builder.OpenElement(0, "div");
                                    builder.AddMarkupContent(1, postBody);
                                    builder.CloseElement();
                                }, new DialogOptions { ShowTitle = true, ShowClose = true, Width = $"{step.N8nWorkflowNotificationWindowWidth}" },
                                null
                            );

                        }
                        else if (step != null && step.N8nWorkflowNotificationType == "Confirmation")
                        {
                            await DialogService.Confirm((RenderFragment)(builder =>
                                    builder.AddMarkupContent(0, postBody)
                                ),$"{step?.N8nWorkflowNotificationTitle}", null, null);

                        }
                        else
                        {
                            if (liveLink != null)
                            {
                                if (!string.IsNullOrEmpty(liveLink.DialogWindowWidth))
                                {
                                    await DialogService.OpenAsync(
                                        $"{liveLink?.Title}",
                                        ds => builder =>
                                        {
                                            builder.OpenElement(0, "div");
                                            builder.AddMarkupContent(1, postBody);
                                            builder.CloseElement();
                                        }, new DialogOptions { ShowTitle = true, ShowClose = true, Width = $"{liveLink.DialogWindowWidth}" },
                                        null
                                    );
                                }
                                else
                                {
                                    NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = $"Success: {liveLink?.Title}", Detail = $"{(int)postResponse.StatusCode} {postResponse.ReasonPhrase}" });

                                }
                            }
                            else
                            {
                                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = $"Success:", Detail = $"{(int)postResponse.StatusCode} {postResponse.ReasonPhrase}" });

                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Request error", Detail = ex.Message });
                DialogService.Close();

            }
        }
        // Busy dialog from string
        async Task BusyDialog(string message)
        {
            await DialogService.OpenAsync("", ds =>
            {
                RenderFragment content = dialogContent =>
                {
                    dialogContent.OpenComponent<RadzenRow>(0);
                    dialogContent.AddComponentParameter(1, nameof(RadzenRow.ChildContent), (RenderFragment)(rowContent =>
                    {
                        rowContent.OpenComponent<RadzenColumn>(0);
                        rowContent.AddComponentParameter(1, nameof(RadzenColumn.Size), 12);
                        rowContent.AddComponentParameter(2, nameof(RadzenRow.ChildContent), (RenderFragment)(columnContent =>
                        {
                            columnContent.AddContent(0, message);
                        }));
                        rowContent.CloseComponent();
                    }));

                    dialogContent.CloseComponent();
                };
                return content;
            }, new DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto", CloseDialogOnEsc = false });
        }
    }
}