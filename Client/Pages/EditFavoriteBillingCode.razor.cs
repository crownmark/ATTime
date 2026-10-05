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
    public partial class EditFavoriteBillingCode
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
        public int FavoriteBillingCodeId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            favoriteBillingCode = await ATTimeService.GetFavoriteBillingCodeByFavoriteBillingCodeId(favoriteBillingCodeId:FavoriteBillingCodeId);
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.FavoriteBillingCode favoriteBillingCode;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.BillingCodeCache> billingCodeCachesForBillingCodeCacheId;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.ResourceCache> resourceCachesForResourceCacheId;


        protected int billingCodeCachesForBillingCodeCacheIdCount;
        protected CrownATTime.Server.Models.ATTime.BillingCodeCache billingCodeCachesForBillingCodeCacheIdValue;
        protected async Task billingCodeCachesForBillingCodeCacheIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetBillingCodeCaches(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Name, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                billingCodeCachesForBillingCodeCacheId = result.Value.AsODataEnumerable();
                billingCodeCachesForBillingCodeCacheIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load BillingCodeCache" });
            }
        }

        protected int resourceCachesForResourceCacheIdCount;
        protected CrownATTime.Server.Models.ATTime.ResourceCache resourceCachesForResourceCacheIdValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task resourceCachesForResourceCacheIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetResourceCaches(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(FullName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                resourceCachesForResourceCacheId = result.Value.AsODataEnumerable();
                resourceCachesForResourceCacheIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load ResourceCache" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.UpdateFavoriteBillingCode(favoriteBillingCodeId:FavoriteBillingCodeId, favoriteBillingCode);
                DialogService.Close(favoriteBillingCode);
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