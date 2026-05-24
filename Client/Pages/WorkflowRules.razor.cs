using CrownATTime.Server.Models.ATTime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        AddWorkflowStep draggedStepItem;

        RadzenTree workflowStepsTree {  get; set; }
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
                if (await DialogService.Confirm("Are you sure you want to delete this workflow rule and all of its workflow steps?") == true)
                {
                    var stepsResult = await ATTimeService.GetWorkflowSteps(
                        filter: $"WorkflowRuleId eq {workflowRule.WorkflowRuleId}");

                    var steps = stepsResult?.Value?.ToList()
                        ?? new List<CrownATTime.Server.Models.ATTime.WorkflowStep>();

                    foreach (var rootStep in steps
                        .Where(x => x.ParentWorkflowStepId == null)
                        .ToList())
                    {
                        await DeleteWorkflowStepAndChildren(rootStep.WorkflowStepId, steps);
                    }

                    foreach (var orphanStep in steps.ToList())
                    {
                        try
                        {
                            await ATTimeService.DeleteWorkflowStep(workflowStepId: orphanStep.WorkflowStepId);
                        }
                        catch
                        {
                            // Already deleted or not found.
                        }
                    }

                    var deleteResult = await ATTimeService.DeleteWorkflowRule(
                        workflowRuleId: workflowRule.WorkflowRuleId);

                    if (deleteResult != null)
                    {
                        workflowRuleChild = null;

                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = $"Unable to delete WorkflowRule. {ex.Message}"
                });
            }
        }

        private async Task DeleteWorkflowStepAndChildren(int workflowStepId, List<CrownATTime.Server.Models.ATTime.WorkflowStep> allSteps)
        {
            var childSteps = allSteps
                .Where(x => x.ParentWorkflowStepId == workflowStepId)
                .ToList();

            foreach (var child in childSteps)
            {
                await DeleteWorkflowStepAndChildren(child.WorkflowStepId, allSteps);
            }

            await ATTimeService.DeleteWorkflowStep(workflowStepId: workflowStepId);

            allSteps.RemoveAll(x => x.WorkflowStepId == workflowStepId);
        }

        protected async Task GridCopyButtonClick(MouseEventArgs args, CrownATTime.Server.Models.ATTime.WorkflowRule workflowRule)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to copy this record?") == true)
                {
                    DialogService.OpenAsync("", ds =>
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
                                    columnContent.AddContent(0, $"Copying Workflow Rule...");
                                }));
                                rowContent.CloseComponent();
                            }));

                            dialogContent.CloseComponent();
                        };
                        return content;
                    }, new DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto", CloseDialogOnEsc = false });

                    var originalRuleId = workflowRule.WorkflowRuleId;

                    // Create a clean copy of the rule.
                    // Do NOT use: var copiedRule = workflowRule;
                    //var copiedRule = new CrownATTime.Server.Models.ATTime.WorkflowRule
                    //{
                    //    Title = $"{workflowRule.Title} (Copy)",
                    //    Active = workflowRule.Active,
                    //    RuleOrder = workflowRule.RuleOrder,

                    //    WorkflowTriggerTypeId = workflowRule.WorkflowTriggerTypeId,

                    //    // Add any other WorkflowRule fields you need copied here.
                    //    // Example:
                    //    // Description = workflowRule.Description,
                    //    // EntityType = workflowRule.EntityType,
                    //    // ConditionsJson = workflowRule.ConditionsJson,
                    //};

                    var copiedRule = workflowRule;
                    copiedRule.WorkflowRuleId = 0;
                    copiedRule.Title = $"{copiedRule.Title} - COPY";

                    var copyresult = await ATTimeService.CreateWorkflowRule(copiedRule);

                    if (copyresult == null)
                    {
                        NotificationService.Notify(new NotificationMessage
                        {
                            Severity = NotificationSeverity.Error,
                            Summary = "Error",
                            Detail = "Unable to create copied WorkflowRule"
                        });

                        return;
                    }

                    var stepsResult = await ATTimeService.GetWorkflowSteps(
                        filter: $"WorkflowRuleId eq {originalRuleId}",
                        expand: "WorkflowStepType,EmailTemplate,NoteTemplate,TimeEntryTemplate,TeamsMessageTemplate",
                        orderby: "ParentWorkflowStepId, BranchResult desc, StepOrder");

                    var originalSteps = stepsResult?.Value?.ToList()
                        ?? new List<CrownATTime.Server.Models.ATTime.WorkflowStep>();

                    // old step id -> newly created step
                    var copiedStepMap = new Dictionary<int, CrownATTime.Server.Models.ATTime.WorkflowStep>();

                    // ------------------------------------------------------------
                    // PASS 1:
                    // Create every step under the new WorkflowRule.
                    // Temporarily set ParentWorkflowStepId = null.
                    // ------------------------------------------------------------
                    foreach (var originalStep in originalSteps.OrderBy(x => x.ParentWorkflowStepId.HasValue).ThenBy(x => x.StepOrder))
                    {
                        var newStep = CopyWorkflowStepForNewRule(
                            originalStep,
                            copyresult.WorkflowRuleId);

                        // Important:
                        // Do not set the parent yet because the new parent id may not exist yet.
                        newStep.ParentWorkflowStepId = null;

                        var createdStep = await ATTimeService.CreateWorkflowStep(newStep);

                        if (createdStep != null)
                        {
                            copiedStepMap[originalStep.WorkflowStepId] = createdStep;
                        }
                    }

                    // ------------------------------------------------------------
                    // PASS 2:
                    // Remap parent ids.
                    // Old ParentWorkflowStepId -> New ParentWorkflowStepId
                    // ------------------------------------------------------------
                    foreach (var originalStep in originalSteps.Where(x => x.ParentWorkflowStepId.HasValue))
                    {
                        if (!copiedStepMap.TryGetValue(originalStep.WorkflowStepId, out var copiedChildStep))
                        {
                            continue;
                        }

                        if (!copiedStepMap.TryGetValue(originalStep.ParentWorkflowStepId.Value, out var copiedParentStep))
                        {
                            continue;
                        }

                        copiedChildStep.ParentWorkflowStepId = copiedParentStep.WorkflowStepId;

                        // Keep the original branch side.
                        copiedChildStep.BranchResult = originalStep.BranchResult;

                        await ATTimeService.UpdateWorkflowStep(
                            workflowStepId: copiedChildStep.WorkflowStepId,
                            workflowStep: copiedChildStep);
                    }

                    await grid0.Reload();

                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Copied",
                        Detail = "WorkflowRule and branch steps copied successfully"
                    });

                    DialogService.Close();
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = $"Unable to copy WorkflowRule. {ex.Message}"
                });
                DialogService.Close();

            }
        }

        protected CrownATTime.Server.Models.ATTime.WorkflowStep CopyWorkflowStepForNewRule(CrownATTime.Server.Models.ATTime.WorkflowStep step, int newWorkflowRuleId)
        {
            return new CrownATTime.Server.Models.ATTime.WorkflowStep
            {
                WorkflowRuleId = newWorkflowRuleId,

                Active = step.Active,
                StepOrder = step.StepOrder,
                Title = step.Title,

                WorkflowStepTypeId = step.WorkflowStepTypeId,
                StepAssignedTo = step.StepAssignedTo,

                ParentWorkflowStepId = step.ParentWorkflowStepId,
                IsBranch = step.IsBranch,
                BranchResult = step.BranchResult,

                EmailTemplateId = step.EmailTemplateId,
                NoteTemplateId = step.NoteTemplateId,
                TimeEntryTemplateId = step.TimeEntryTemplateId,
                TeamsMessageTemplateId = step.TeamsMessageTemplateId,

                ConfirmationDialogTitle = step.ConfirmationDialogTitle,
                ConfirmationDialogMessage = step.ConfirmationDialogMessage,
                ConfirmationDialogContinueOnNo = step.ConfirmationDialogContinueOnNo,

                NotificationDialogTitle = step.NotificationDialogTitle,
                NotificationDialogMessage = step.NotificationDialogMessage,

                N8nWorkflowUrl = step.N8nWorkflowUrl,
                N8nWorkflowMethod = step.N8nWorkflowMethod,

                TicketStatusId = step.TicketStatusId,

                TicketUdfName = step.TicketUdfName,
                TicketUdfValue = step.TicketUdfValue,

                TicketUdfName1 = step.TicketUdfName1,
                TicketUdfValue1 = step.TicketUdfValue1,

                TicketUdfName2 = step.TicketUdfName2,
                TicketUdfValue2 = step.TicketUdfValue2,

                TicketUdfName3 = step.TicketUdfName3,
                TicketUdfValue3 = step.TicketUdfValue3
            };
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
            var WorkflowStepsResult = await ATTimeService.GetWorkflowSteps(filter: $"WorkflowRuleId eq {args.WorkflowRuleId}", expand: "WorkflowRule,WorkflowStepType,EmailTemplate,NoteTemplate,TimeEntryTemplate,TeamsMessageTemplate", orderby: $"ParentWorkflowStepId, BranchResult desc, StepOrder");
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
            await workflowStepsTree.Reload();
            StateHasChanged();
            //await WorkflowStepsDataGrid.Reload();

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

        protected async System.Threading.Tasks.Task Tree0Change(Radzen.TreeEventArgs args)
        {
            if (args.Value is WorkflowStep step)
            {
                var dialogResult = await DialogService.OpenAsync<EditWorkflowStep>("Edit WorkflowSteps", new Dictionary<string, object> { { "WorkflowStepId", step.WorkflowStepId } }, new DialogOptions { Resizable = false, Draggable = true });
                await GetChildData(workflowRuleChild);
                await workflowStepsTree.Reload();
                StateHasChanged();
            }
        }

        protected async System.Threading.Tasks.Task Tree0ItemContextMenu(Radzen.TreeItemContextMenuEventArgs args)
        {
            try
            {
                ContextMenuService.Open(args,
                    new List<ContextMenuItem> {
                        new ContextMenuItem(){ Text = "Copy Step", Value = 1, Icon = "content_copy" },
                        new ContextMenuItem(){ Text = "Delete Step", Value = 2, Icon = "delete" },
                        },
                    async (e) =>
                    {
                        if(e.Text == "Copy Step")
                        {
                            try
                            {
                                if (args.Value is WorkflowStep step)
                                {
                                    if (await DialogService.Confirm("Are you sure you want to copy this record?") == true)
                                    {
                                        var copiedStep = step;
                                        copiedStep.WorkflowStepId = 0;
                                        copiedStep.Title = $"{step.Title} (Copy)";
                                        var copyresult = await ATTimeService.CreateWorkflowStep(copiedStep);

                                        if (copyresult != null)
                                        {
                                            var ruleRecord = await ATTimeService.GetWorkflowRuleByWorkflowRuleId("", step.WorkflowRuleId);
                                            await GetChildData(ruleRecord);
                                            await workflowStepsTree.Reload();
                                            StateHasChanged();
                                        }
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
                        if(e.Text == "Delete Step")
                        {
                            if (args.Value is WorkflowStep step)
                            {
                                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                                {
                                    try
                                    {
                                        var deleteResult = await ATTimeService.DeleteWorkflowStep(workflowStepId: step.WorkflowStepId);

                                        await GetChildData(workflowRuleChild);

                                        if (deleteResult != null)
                                        {
                                            await workflowStepsTree.Reload();
                                            StateHasChanged();
                                        }
                                    }
                                    catch (Exception ex)
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
                    }
                 );
                
            }
            catch (System.Exception ex)
            {
                
            }
        }

        protected IEnumerable<WorkflowStep> GetRootWorkflowSteps()
        {
            return workflowRuleChild?.WorkflowSteps?
                .Where(x => x.ParentWorkflowStepId == null)
                .OrderBy(x => x.StepOrder)
                ?? Enumerable.Empty<WorkflowStep>();
        }

        protected IEnumerable<WorkflowStep> GetChildWorkflowSteps(WorkflowStep parentStep, bool branchResult)
        {
            return workflowRuleChild?.WorkflowSteps?
                .Where(x =>
                    x.ParentWorkflowStepId == parentStep.WorkflowStepId &&
                    x.BranchResult == branchResult)
                .OrderBy(x => x.StepOrder)
                ?? Enumerable.Empty<WorkflowStep>();
        }

        protected string GetWorkflowStepTreeText(WorkflowStep step)
        {
            var stepTypeTitle = step.WorkflowStepType?.Title ?? "No Step Type";
            var title = string.IsNullOrWhiteSpace(step.Title) ? "Untitled Step" : step.Title;

            return $"{step.StepOrder} - ({stepTypeTitle}) - {title}";
        }

        private RenderFragment RenderWorkflowStep(WorkflowStep step) => builder =>
        {
            var sequence = 0;

            builder.OpenComponent<RadzenTreeItem>(sequence++);
            builder.AddAttribute(sequence++, "Text", GetWorkflowStepTreeText(step));
            builder.AddAttribute(sequence++, "Expanded", true);
            builder.AddAttribute(sequence++, "Value", step);

            if (step.IsBranch)
            {
                builder.AddAttribute(sequence++, "ChildContent", (RenderFragment)(childBuilder =>
                {
                    var childSequence = 0;

                    // TRUE branch folder
                    childBuilder.OpenComponent<RadzenTreeItem>(childSequence++);
                    childBuilder.AddAttribute(childSequence++, "Text", "True");
                    childBuilder.AddAttribute(childSequence++, "Expanded", true);
                    childBuilder.AddAttribute(childSequence++, "ChildContent", (RenderFragment)(trueBranchBuilder =>
                    {
                        var trueSequence = 0;

                        foreach (var trueChild in GetChildWorkflowSteps(step, true))
                        {
                            trueBranchBuilder.AddContent(trueSequence++, RenderWorkflowStep(trueChild));
                        }
                    }));
                    childBuilder.CloseComponent();

                    // FALSE branch folder
                    childBuilder.OpenComponent<RadzenTreeItem>(childSequence++);
                    childBuilder.AddAttribute(childSequence++, "Text", "False");
                    childBuilder.AddAttribute(childSequence++, "Expanded", true);
                    childBuilder.AddAttribute(childSequence++, "ChildContent", (RenderFragment)(falseBranchBuilder =>
                    {
                        var falseSequence = 0;

                        foreach (var falseChild in GetChildWorkflowSteps(step, false))
                        {
                            falseBranchBuilder.AddContent(falseSequence++, RenderWorkflowStep(falseChild));
                        }
                    }));
                    childBuilder.CloseComponent();
                }));
            }

            builder.CloseComponent();
        };

    }
}