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
    public partial class EditTeamsMessageTemplate
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

        [Parameter]
        public int TeamsMessageTemplateId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            teamsMessageTemplate = await ATTimeService.GetTeamsMessageTemplateByTeamsMessageTemplateId(teamsMessageTemplateId:TeamsMessageTemplateId);
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.TeamsMessageTemplate teamsMessageTemplate;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TeamsMessageType> teamsMessageTypesForTeamsMessageTypeId;


        protected int teamsMessageTypesForTeamsMessageTypeIdCount;
        protected CrownATTime.Server.Models.ATTime.TeamsMessageType teamsMessageTypesForTeamsMessageTypeIdValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task teamsMessageTypesForTeamsMessageTypeIdLoadData(LoadDataArgs args)
        {
            try
            {
                string defaultFilter = $"Active eq true";
                var result = await ATTimeService.GetTeamsMessageTypes(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                teamsMessageTypesForTeamsMessageTypeId = result.Value.AsODataEnumerable();
                teamsMessageTypesForTeamsMessageTypeIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TeamsMessageType" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.UpdateTeamsMessageTemplate(teamsMessageTemplateId:TeamsMessageTemplateId, teamsMessageTemplate);
                DialogService.Close(teamsMessageTemplate);
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

        protected async System.Threading.Tasks.Task ViewTemplateTokensButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<TemplateTokens>("Template Tokens", null, new DialogOptions { Width = "915px", Draggable = true });

        }
    }
}