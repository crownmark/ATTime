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
    public partial class AddNoteTemplate
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

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> ticketNoteEntityPicklistNoteTypeValueCaches;

        protected int ticketNoteEntityPicklistValueNoteTypeCachesCount;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> ticketNoteEntityPicklistPublishValueCaches;

        protected int ticketNoteEntityPicklistValuePublishCachesCount;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> ticketEntityPicklistValueCaches;

        protected int ticketEntityPicklistValueCachesCount;
        protected override async Task OnInitializedAsync()
        {
            noteTemplate = new CrownATTime.Server.Models.ATTime.NoteTemplate();
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.NoteTemplate noteTemplate;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.CreateNoteTemplate(noteTemplate);
                DialogService.Close(noteTemplate);
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

        protected async Task ticketNoteEntityPicklistValueCachesNoteTypeLoadData(LoadDataArgs args)
        {
            try
            {
                string defaultFilter = $"PicklistName eq 'noteType'";
                var result = await ATTimeService.GetTicketNoteEntityPicklistValueCaches(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Label, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Label asc");

                ticketNoteEntityPicklistNoteTypeValueCaches = result.Value.AsODataEnumerable();
                ticketNoteEntityPicklistValueNoteTypeCachesCount = result.Count;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to load Note Type Picklist. {ex.Message}" });
            }
        }

        protected async Task ticketNoteEntityPicklistValueCachesPublishLoadData(LoadDataArgs args)
        {
            try
            {
                string defaultFilter = $"PicklistName eq 'publish'";
                var result = await ATTimeService.GetTicketNoteEntityPicklistValueCaches(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Label, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Label asc");

                ticketNoteEntityPicklistPublishValueCaches = result.Value.AsODataEnumerable();
                ticketNoteEntityPicklistValuePublishCachesCount = result.Count;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to load Publish Picklist. {ex.Message}" });
            }
        }
        protected async Task ticketEntityPicklistValueCachesLoadData(LoadDataArgs args)
        {
            try
            {
                string defaultFilter = $"PicklistName eq 'status'";
                var result = await ATTimeService.GetTicketEntityPicklistValueCaches(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Label, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Label asc");

                ticketEntityPicklistValueCaches = result.Value.AsODataEnumerable();
                ticketEntityPicklistValueCachesCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }
    }
}