using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using CrownATTime.Server.Models.ATTime;

namespace CrownATTime.Client.Pages
{
    public partial class AddWorkflowRule
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

        protected List<TicketEntityPicklistValueCache> statuses { get; set; }
        protected List<TicketEntityPicklistValueCache> priorities { get; set; }
        protected List<TicketEntityPicklistValueCache> queues { get; set; }
        protected List<TicketEntityPicklistValueCache> ticketCategories { get; set; }
        protected List<TicketEntityPicklistValueCache> issueTypes { get; set; }
        protected List<TicketEntityPicklistValueCache> subIssueTypes { get; set; }
        protected override async Task OnInitializedAsync()
        {
            workflowRule = new CrownATTime.Server.Models.ATTime.WorkflowRule();
            try
            {
                var result = await ATTimeService.GetTicketEntityPicklistValueCaches();
                statuses = result.Value.Where(x => x.PicklistName == "status").OrderBy(x => x.Label).ToList();
                priorities = result.Value.Where(x => x.PicklistName == "priority").OrderBy(x => x.Label).ToList();
                queues = result.Value.Where(x => x.PicklistName == "queueID").OrderBy(x => x.Label).ToList();
                ticketCategories = result.Value.Where(x => x.PicklistName == "ticketCategory").OrderBy(x => x.Label).ToList();
                issueTypes = result.Value.Where(x => x.PicklistName == "issueType").OrderBy(x => x.Label).ToList();
                subIssueTypes = result.Value.Where(x => x.PicklistName == "subIssueType").OrderBy(x => x.Label).ToList();
                
            }
            catch (Exception ex)
            {
               
            }
            
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.WorkflowRule workflowRule;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.CompanyCache> companyCachesForCompanyId;


        protected int companyCachesForCompanyIdCount;
        protected CrownATTime.Server.Models.ATTime.CompanyCache companyCachesForCompanyIdValue;

        [Inject]
        protected SecurityService Security { get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.WorkflowTriggerType> workflowTriggerTypes;

        protected int workflowTriggerTypesCount;
        protected async Task companyCachesForCompanyIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetCompanyCaches(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(CompanyName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"CompanyName");
                companyCachesForCompanyId = result.Value.AsODataEnumerable();
                companyCachesForCompanyIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load CompanyCache" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.CreateWorkflowRule(workflowRule);
                DialogService.Close(workflowRule);
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


        protected async Task workflowTriggerTypesLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetWorkflowTriggerTypes(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, \"{(!string.IsNullOrEmpty(args.Filter) ? args.Filter: "")}\")", orderby: args.OrderBy);

                workflowTriggerTypes = result.Value.AsODataEnumerable();
                workflowTriggerTypesCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }
    }
}