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
    public partial class EditIngramSyncRule
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
        public DistributorProductsService DistributorProductsService { get; set; }

        [Parameter]
        public int RuleId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ingramSyncRule = await DistributorProductsService.GetIngramSyncRuleByRuleId(ruleId:RuleId);
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.DistributorProducts.IngramSyncRule ingramSyncRule;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await DistributorProductsService.UpdateIngramSyncRule(ruleId:RuleId, ingramSyncRule);
                DialogService.Close(ingramSyncRule);
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