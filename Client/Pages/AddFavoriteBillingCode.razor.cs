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
    public partial class AddFavoriteBillingCode
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

        protected override async Task OnInitializedAsync()
        {
            favoriteBillingCode = new CrownATTime.Server.Models.ATTime.FavoriteBillingCode();
            favoriteBillingCode.ResourceCacheId = ResourceCacheId;
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
                var result = await ATTimeService.GetBillingCodeCaches(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Name, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Name");
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
        [Parameter]
        public int ResourceCacheId { get; set; }
        protected async Task resourceCachesForResourceCacheIdLoadData(LoadDataArgs args)
        {
            try
            {
                string defaultFilter = $"ResourceCacheId eq {ResourceCacheId}";
                var result = await ATTimeService.GetResourceCaches(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(FullName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"FullName");
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
                await ATTimeService.CreateFavoriteBillingCode(favoriteBillingCode);
                DialogService.Close(favoriteBillingCode);
            }
            catch (Exception ex)
            {
                errorVisible = true;
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load ResourceCache.  {ex.Message}" });

            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}