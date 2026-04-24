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
    public partial class AddWorkflowStep
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
        protected List<string> httpMethods { get; set; } = new List<string> { "OPENURL", "GET", "POST", "PUT", "DELETE", "PATCH" };
        protected List<string> notificationTypes { get; set; } = new List<string> { "Notification", "Alert", "Confirmation" };


        protected override async Task OnInitializedAsync()
        {
        }
        protected async System.Threading.Tasks.Task ViewTemplateTokensButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<TemplateTokens>("Template Tokens", null, new DialogOptions { Width = "915px", Draggable = true });

        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.WorkflowStep workflowStep;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.WorkflowRule> workflowRulesForWorkflowRuleId;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.WorkflowStepType> workflowStepTypesForWorkflowStepTypeId;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.EmailTemplate> emailTemplatesForEmailTemplateId;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.NoteTemplate> noteTemplatesForNoteTemplateId;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> timeEntryTemplatesForTimeEntryTemplateId;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> teamsMessageTemplatesForTeamsMessageTemplateId;


        protected int workflowRulesForWorkflowRuleIdCount;
        protected CrownATTime.Server.Models.ATTime.WorkflowRule workflowRulesForWorkflowRuleIdValue;
        protected async Task workflowRulesForWorkflowRuleIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetWorkflowRules(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                workflowRulesForWorkflowRuleId = result.Value.AsODataEnumerable();
                workflowRulesForWorkflowRuleIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load WorkflowRule" });
            }
        }

        protected int workflowStepTypesForWorkflowStepTypeIdCount;
        protected CrownATTime.Server.Models.ATTime.WorkflowStepType workflowStepTypesForWorkflowStepTypeIdValue;
        protected async Task workflowStepTypesForWorkflowStepTypeIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetWorkflowStepTypes(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"Active eq true and contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                workflowStepTypesForWorkflowStepTypeId = result.Value.AsODataEnumerable();
                workflowStepTypesForWorkflowStepTypeIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load WorkflowStepType" });
            }
        }

        protected int emailTemplatesForEmailTemplateIdCount;
        protected CrownATTime.Server.Models.ATTime.EmailTemplate emailTemplatesForEmailTemplateIdValue;
        protected async Task emailTemplatesForEmailTemplateIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetEmailTemplates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                emailTemplatesForEmailTemplateId = result.Value.AsODataEnumerable();
                emailTemplatesForEmailTemplateIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load EmailTemplate" });
            }
        }

        protected int noteTemplatesForNoteTemplateIdCount;
        protected CrownATTime.Server.Models.ATTime.NoteTemplate noteTemplatesForNoteTemplateIdValue;
        protected async Task noteTemplatesForNoteTemplateIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetNoteTemplates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                noteTemplatesForNoteTemplateId = result.Value.AsODataEnumerable();
                noteTemplatesForNoteTemplateIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load NoteTemplate" });
            }
        }

        protected int timeEntryTemplatesForTimeEntryTemplateIdCount;
        protected CrownATTime.Server.Models.ATTime.TimeEntryTemplate timeEntryTemplatesForTimeEntryTemplateIdValue;
        protected async Task timeEntryTemplatesForTimeEntryTemplateIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetTimeEntryTemplates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                timeEntryTemplatesForTimeEntryTemplateId = result.Value.AsODataEnumerable();
                timeEntryTemplatesForTimeEntryTemplateIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TimeEntryTemplate" });
            }
        }

        protected int teamsMessageTemplatesForTeamsMessageTemplateIdCount;
        protected CrownATTime.Server.Models.ATTime.TeamsMessageTemplate teamsMessageTemplatesForTeamsMessageTemplateIdValue;
        protected async Task teamsMessageTemplatesForTeamsMessageTemplateIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetTeamsMessageTemplates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                teamsMessageTemplatesForTeamsMessageTemplateId = result.Value.AsODataEnumerable();
                teamsMessageTemplatesForTeamsMessageTemplateIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TeamsMessageTemplate" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.CreateWorkflowStep(workflowStep);
                DialogService.Close(workflowStep);
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





        bool hasEmailTemplateIdValue;

        [Parameter]
        public int? EmailTemplateId { get; set; }

        bool hasNoteTemplateIdValue;

        [Parameter]
        public int? NoteTemplateId { get; set; }

        bool hasTeamsMessageTemplateIdValue;

        [Parameter]
        public int? TeamsMessageTemplateId { get; set; }

        bool hasTimeEntryTemplateIdValue;

        [Parameter]
        public int? TimeEntryTemplateId { get; set; }

        bool hasWorkflowRuleIdValue;

        [Parameter]
        public int WorkflowRuleId { get; set; }

        bool hasWorkflowStepTypeIdValue;

        [Parameter]
        public int WorkflowStepTypeId { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> ticketEntityPicklistValueCaches;

        protected int ticketEntityPicklistValueCachesCount;
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            workflowStep = new CrownATTime.Server.Models.ATTime.WorkflowStep();

            hasEmailTemplateIdValue = parameters.TryGetValue<int?>("EmailTemplateId", out var hasEmailTemplateIdResult);

            if (hasEmailTemplateIdValue)
            {
                workflowStep.EmailTemplateId = hasEmailTemplateIdResult;
            }

            hasNoteTemplateIdValue = parameters.TryGetValue<int?>("NoteTemplateId", out var hasNoteTemplateIdResult);

            if (hasNoteTemplateIdValue)
            {
                workflowStep.NoteTemplateId = hasNoteTemplateIdResult;
            }

            hasTeamsMessageTemplateIdValue = parameters.TryGetValue<int?>("TeamsMessageTemplateId", out var hasTeamsMessageTemplateIdResult);

            if (hasTeamsMessageTemplateIdValue)
            {
                workflowStep.TeamsMessageTemplateId = hasTeamsMessageTemplateIdResult;
            }

            hasTimeEntryTemplateIdValue = parameters.TryGetValue<int?>("TimeEntryTemplateId", out var hasTimeEntryTemplateIdResult);

            if (hasTimeEntryTemplateIdValue)
            {
                workflowStep.TimeEntryTemplateId = hasTimeEntryTemplateIdResult;
            }

            hasWorkflowRuleIdValue = parameters.TryGetValue<int>("WorkflowRuleId", out var hasWorkflowRuleIdResult);

            if (hasWorkflowRuleIdValue)
            {
                workflowStep.WorkflowRuleId = hasWorkflowRuleIdResult;
            }

            hasWorkflowStepTypeIdValue = parameters.TryGetValue<int>("WorkflowStepTypeId", out var hasWorkflowStepTypeIdResult);

            if (hasWorkflowStepTypeIdValue)
            {
                workflowStep.WorkflowStepTypeId = hasWorkflowStepTypeIdResult;
            }
            await base.SetParametersAsync(parameters);
        }


        protected async Task ticketEntityPicklistValueCachesLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetTicketEntityPicklistValueCaches(new Query { Top = args.Top, Skip = args.Skip, Filter = $"PicklistName eq 'status' and contains(Label, \"{(!string.IsNullOrEmpty(args.Filter) ? args.Filter: "")}\")", OrderBy = "Label" });

                ticketEntityPicklistValueCaches = result.Value.AsODataEnumerable();
                ticketEntityPicklistValueCachesCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }
    }
}