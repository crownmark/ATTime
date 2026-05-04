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
    public partial class AddDuration
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
            duration = new CrownATTime.Server.Models.ATTime.Duration();
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.Duration duration;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.DurationType> durationTypesForDurationTypeId;


        protected int durationTypesForDurationTypeIdCount;
        protected CrownATTime.Server.Models.ATTime.DurationType durationTypesForDurationTypeIdValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task durationTypesForDurationTypeIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetDurationTypes(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                durationTypesForDurationTypeId = result.Value.AsODataEnumerable();
                durationTypesForDurationTypeIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load DurationType" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.CreateDuration(duration);
                DialogService.Close(duration);
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