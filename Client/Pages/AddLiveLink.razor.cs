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
    public partial class AddLiveLink
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
        protected List<string> httpMethods { get; set; } = new List<string> { "GET", "POST", "PUT", "DELETE", "PATCH" };

        protected override async Task OnInitializedAsync()
        {
            liveLink = new CrownATTime.Server.Models.ATTime.LiveLink();
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.LiveLink liveLink;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.CreateLiveLink(liveLink);
                DialogService.Close(liveLink);
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