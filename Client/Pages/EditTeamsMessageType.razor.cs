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
    public partial class EditTeamsMessageType
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
        public int TeamsMessageTypeId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            teamsMessageType = await ATTimeService.GetTeamsMessageTypeByTeamsMessageTypeId(teamsMessageTypeId:TeamsMessageTypeId);
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.TeamsMessageType teamsMessageType;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.UpdateTeamsMessageType(teamsMessageTypeId:TeamsMessageTypeId, teamsMessageType);
                DialogService.Close(teamsMessageType);
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
    }
}