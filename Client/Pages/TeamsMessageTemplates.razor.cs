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
    public partial class TeamsMessageTemplates
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

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> teamsMessageTemplates;

        protected RadzenDataGrid<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> grid0;
        protected int count;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

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
                var result = await ATTimeService.GetTeamsMessageTemplates(filter: $@"(contains(TeamId,""{search}"") or contains(ChannelId,""{search}"") or contains(Title,""{search}"") or contains(Message,""{search}"") or contains(TemplateAssignedTo,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "TeamsMessageType", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                teamsMessageTemplates = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TeamsMessageTemplates" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTeamsMessageTemplate>("Add TeamsMessageTemplate", options: new DialogOptions { Resizable = false, Draggable = false });
            await grid0.Reload();
        }

        protected async Task EditRow(CrownATTime.Server.Models.ATTime.TeamsMessageTemplate args)
        {
            await DialogService.OpenAsync<EditTeamsMessageTemplate>("Edit TeamsMessageTemplate", new Dictionary<string, object> { {"TeamsMessageTemplateId", args.TeamsMessageTemplateId} }, new DialogOptions { Resizable = false, Draggable = false });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CrownATTime.Server.Models.ATTime.TeamsMessageTemplate teamsMessageTemplate)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ATTimeService.DeleteTeamsMessageTemplate(teamsMessageTemplateId:teamsMessageTemplate.TeamsMessageTemplateId);

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
                    Detail = $"Unable to delete TeamsMessageTemplate"
                });
            }
        }
    }
}