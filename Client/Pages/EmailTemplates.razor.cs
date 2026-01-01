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
    public partial class EmailTemplates
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

        protected IEnumerable<CrownATTime.Server.Models.ATTime.EmailTemplate> emailTemplates;

        protected RadzenDataGrid<CrownATTime.Server.Models.ATTime.EmailTemplate> grid0;
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
                var result = await ATTimeService.GetEmailTemplates(filter: $@"(contains(Title,""{search}"") or contains(EmailSubject,""{search}"") or contains(EmailBody,""{search}"") or contains(FromEmailAddress,""{search}"") or contains(TemplateAssignedTo,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                emailTemplates = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load EmailTemplates" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddEmailTemplate>("Add EmailTemplate", null);
            await grid0.Reload();
        }

        protected async Task EditRow(CrownATTime.Server.Models.ATTime.EmailTemplate args)
        {
            await DialogService.OpenAsync<EditEmailTemplate>("Edit EmailTemplate", new Dictionary<string, object> { {"EmailTemplateId", args.EmailTemplateId} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CrownATTime.Server.Models.ATTime.EmailTemplate emailTemplate)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ATTimeService.DeleteEmailTemplate(emailTemplateId:emailTemplate.EmailTemplateId);

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
                    Detail = $"Unable to delete EmailTemplate"
                });
            }
        }
    }
}