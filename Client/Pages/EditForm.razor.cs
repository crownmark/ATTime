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
    public partial class EditForm
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
        public int FormId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            form = await ATTimeService.GetFormByFormId(formId:FormId);
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.Form form;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.FormCategory> formCategoriesForFormCategoryId;


        protected int formCategoriesForFormCategoryIdCount;
        protected CrownATTime.Server.Models.ATTime.FormCategory formCategoriesForFormCategoryIdValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task formCategoriesForFormCategoryIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetFormCategories(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Title");
                formCategoriesForFormCategoryId = result.Value.AsODataEnumerable();
                formCategoriesForFormCategoryIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load FormCategory" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.UpdateForm(formId:FormId, form);
                DialogService.Close(form);
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