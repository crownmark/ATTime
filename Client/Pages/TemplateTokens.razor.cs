using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using CrownATTime.Server.Models;

namespace CrownATTime.Client.Pages
{
    public partial class TemplateTokens
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
        protected TemplateTokenDiscoveryService TemplateTokenDiscoveryService { get; set; }

        protected IReadOnlyList<TemplateTokenInfo> TemplateTokenInfos { get; set; } = new List<TemplateTokenInfo>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                TemplateTokenInfos = TemplateTokenDiscoveryService.GetAvailableTokens();
            }
            catch (Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task CopyTokenButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, TemplateTokenInfo selectedToken)
        {
            try
            {

                await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", selectedToken.Token);
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success", Detail = "Token Copied to Clipboard" });


            }
            catch (Exception ex)
            {

            }
        }
    }
}