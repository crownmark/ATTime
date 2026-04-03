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
    public partial class WorkflowRules
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

        protected IEnumerable<CrownATTime.Server.Models.ATTime.WorkflowRule> workflowRules;

        protected RadzenDataGrid<CrownATTime.Server.Models.ATTime.WorkflowRule> grid0;
        protected int count;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetWorkflowRules(filter: $@"(contains(Title,""{search}"") or contains(TicketCreatedBy,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "CompanyCache,WorkflowTriggerType", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                workflowRules = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load WorkflowRules" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddWorkflowRule>("Add WorkflowRule", options: new DialogOptions { Resizable = false, Draggable = true });
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<CrownATTime.Server.Models.ATTime.WorkflowRule> args)
        {
            await DialogService.OpenAsync<EditWorkflowRule>("Edit WorkflowRule", new Dictionary<string, object> { {"WorkflowRuleId", args.Data.WorkflowRuleId} }, new DialogOptions { Resizable = false, Draggable = true });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CrownATTime.Server.Models.ATTime.WorkflowRule workflowRule)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ATTimeService.DeleteWorkflowRule(workflowRuleId:workflowRule.WorkflowRuleId);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete WorkflowRule"
                });
            }
        }

        protected async Task GridCopyButtonClick(MouseEventArgs args, CrownATTime.Server.Models.ATTime.WorkflowRule workflowRule)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to copy this record?") == true)
                {
                    int ruleId = workflowRule.WorkflowRuleId;
                    var copiedRule = workflowRule;
                    copiedRule.WorkflowRuleId = 0;
                    copiedRule.Title = $"{workflowRule.Title} (Copy)";
                    var copyresult = await ATTimeService.CreateWorkflowRule(copiedRule);
                    var steps = await ATTimeService.GetWorkflowSteps(filter: $"WorkflowRuleId eq {ruleId}");
                    foreach (var step in steps.Value.ToList())
                    {
                        step.WorkflowStepId = 0;
                        step.WorkflowRuleId = copyresult.WorkflowRuleId;
                        //step.Title = $"{step.Title} (Copy)";
                        await ATTimeService.CreateWorkflowStep(step);
                    }

                    if (copyresult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to copy WorkflowRule"
                });
            }
        }
        protected async Task WorkflowStepCopyButtonClick(MouseEventArgs args, CrownATTime.Server.Models.ATTime.WorkflowStep workflowStep)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to copy this record?") == true)
                {
                    var copiedStep = workflowStep;
                    copiedStep.WorkflowStepId = 0;
                    copiedStep.Title = $"{workflowStep.Title} (Copy)";
                    var copyresult = await ATTimeService.CreateWorkflowStep(copiedStep);

                    if (copyresult != null)
                    {
                        var ruleRecord = await ATTimeService.GetWorkflowRuleByWorkflowRuleId("", workflowStep.WorkflowRuleId);
                        await GetChildData(ruleRecord);
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to copy WorkflowRule"
                });
            }
        }

        protected CrownATTime.Server.Models.ATTime.WorkflowRule workflowRuleChild;
        protected async Task GetChildData(CrownATTime.Server.Models.ATTime.WorkflowRule args)
        {
            workflowRuleChild = args;
            var WorkflowStepsResult = await ATTimeService.GetWorkflowSteps(filter:$"WorkflowRuleId eq {args.WorkflowRuleId}", expand: "WorkflowRule,WorkflowStepType,EmailTemplate,NoteTemplate,TimeEntryTemplate,TeamsMessageTemplate", orderby: $"StepOrder");
            if (WorkflowStepsResult != null)
            {
                args.WorkflowSteps = WorkflowStepsResult.Value.ToList();
            }
        }
        protected CrownATTime.Server.Models.ATTime.WorkflowStep workflowStepWorkflowSteps;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.WorkflowRule> workflowRulesForWorkflowRuleIdWorkflowSteps;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.WorkflowStepType> workflowStepTypesForWorkflowStepTypeIdWorkflowSteps;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.EmailTemplate> emailTemplatesForEmailTemplateIdWorkflowSteps;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.NoteTemplate> noteTemplatesForNoteTemplateIdWorkflowSteps;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> timeEntryTemplatesForTimeEntryTemplateIdWorkflowSteps;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> teamsMessageTemplatesForTeamsMessageTemplateIdWorkflowSteps;

        protected int workflowRulesForWorkflowRuleIdWorkflowStepsCount;
        protected CrownATTime.Server.Models.ATTime.WorkflowRule workflowRulesForWorkflowRuleIdWorkflowStepsValue;
        protected async Task workflowRulesForWorkflowRuleIdWorkflowStepsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetWorkflowRules(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                workflowRulesForWorkflowRuleIdWorkflowSteps = result.Value.AsODataEnumerable();
                workflowRulesForWorkflowRuleIdWorkflowStepsCount = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load WorkflowRule" });
            }
        }

        protected int workflowStepTypesForWorkflowStepTypeIdWorkflowStepsCount;
        protected CrownATTime.Server.Models.ATTime.WorkflowStepType workflowStepTypesForWorkflowStepTypeIdWorkflowStepsValue;
        protected async Task workflowStepTypesForWorkflowStepTypeIdWorkflowStepsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetWorkflowStepTypes(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                workflowStepTypesForWorkflowStepTypeIdWorkflowSteps = result.Value.AsODataEnumerable();
                workflowStepTypesForWorkflowStepTypeIdWorkflowStepsCount = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load WorkflowStepType" });
            }
        }

        protected int emailTemplatesForEmailTemplateIdWorkflowStepsCount;
        protected CrownATTime.Server.Models.ATTime.EmailTemplate emailTemplatesForEmailTemplateIdWorkflowStepsValue;
        protected async Task emailTemplatesForEmailTemplateIdWorkflowStepsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetEmailTemplates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                emailTemplatesForEmailTemplateIdWorkflowSteps = result.Value.AsODataEnumerable();
                emailTemplatesForEmailTemplateIdWorkflowStepsCount = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load EmailTemplate" });
            }
        }

        protected int noteTemplatesForNoteTemplateIdWorkflowStepsCount;
        protected CrownATTime.Server.Models.ATTime.NoteTemplate noteTemplatesForNoteTemplateIdWorkflowStepsValue;
        protected async Task noteTemplatesForNoteTemplateIdWorkflowStepsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetNoteTemplates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                noteTemplatesForNoteTemplateIdWorkflowSteps = result.Value.AsODataEnumerable();
                noteTemplatesForNoteTemplateIdWorkflowStepsCount = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load NoteTemplate" });
            }
        }

        protected int timeEntryTemplatesForTimeEntryTemplateIdWorkflowStepsCount;
        protected CrownATTime.Server.Models.ATTime.TimeEntryTemplate timeEntryTemplatesForTimeEntryTemplateIdWorkflowStepsValue;
        protected async Task timeEntryTemplatesForTimeEntryTemplateIdWorkflowStepsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetTimeEntryTemplates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                timeEntryTemplatesForTimeEntryTemplateIdWorkflowSteps = result.Value.AsODataEnumerable();
                timeEntryTemplatesForTimeEntryTemplateIdWorkflowStepsCount = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TimeEntryTemplate" });
            }
        }

        protected int teamsMessageTemplatesForTeamsMessageTemplateIdWorkflowStepsCount;
        protected CrownATTime.Server.Models.ATTime.TeamsMessageTemplate teamsMessageTemplatesForTeamsMessageTemplateIdWorkflowStepsValue;
        protected async Task teamsMessageTemplatesForTeamsMessageTemplateIdWorkflowStepsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetTeamsMessageTemplates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                teamsMessageTemplatesForTeamsMessageTemplateIdWorkflowSteps = result.Value.AsODataEnumerable();
                teamsMessageTemplatesForTeamsMessageTemplateIdWorkflowStepsCount = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TeamsMessageTemplate" });
            }
        }

        protected RadzenDataGrid<CrownATTime.Server.Models.ATTime.WorkflowStep> WorkflowStepsDataGrid;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task WorkflowStepsAddButtonClick(MouseEventArgs args, CrownATTime.Server.Models.ATTime.WorkflowRule data)
        {

            var dialogResult = await DialogService.OpenAsync<AddWorkflowStep>("Add WorkflowSteps", new Dictionary<string, object> { {"WorkflowRuleId" , data.WorkflowRuleId} }, new DialogOptions { Resizable = false, Draggable = true });
            await GetChildData(data);
            await WorkflowStepsDataGrid.Reload();

        }

        protected async Task WorkflowStepsRowSelect(CrownATTime.Server.Models.ATTime.WorkflowStep args, CrownATTime.Server.Models.ATTime.WorkflowRule data)
        {
            var dialogResult = await DialogService.OpenAsync<EditWorkflowStep>("Edit WorkflowSteps", new Dictionary<string, object> { {"WorkflowStepId", args.WorkflowStepId} }, new DialogOptions { Resizable = false, Draggable = true });
            await GetChildData(data);
            await WorkflowStepsDataGrid.Reload();
        }

        protected async Task WorkflowStepsDeleteButtonClick(MouseEventArgs args, CrownATTime.Server.Models.ATTime.WorkflowStep workflowStep)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ATTimeService.DeleteWorkflowStep(workflowStepId:workflowStep.WorkflowStepId);

                    await GetChildData(workflowRuleChild);

                    if (deleteResult != null)
                    {
                        await WorkflowStepsDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete WorkflowStep"
                });
            }
        }
    }
}