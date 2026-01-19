using CrownATTime.Server.Models;
using CrownATTime.Server.Models.ATTime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrownATTime.Client.Pages
{
    public partial class AddNote
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
        public ATTimeService ATTimeService { get; set; }

        [Inject]
        public AutotaskService AutotaskService { get; set; }


        protected CrownATTime.Server.Models.NewNote newNote { get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> ticketNoteEntityPicklistNoteTypeValueCaches;

        protected int ticketNoteEntityPicklistValueNoteTypeCachesCount;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> ticketNoteEntityPicklistPublishValueCaches;

        protected int ticketNoteEntityPicklistValuePublishCachesCount;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> ticketEntityPicklistValueCaches;

        protected int ticketEntityPicklistValueCachesCount;

        protected bool errorVisible;
        protected NewNote note;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.NoteTemplate> noteTemplates;

        protected int noteTemplatesCount;

        [Parameter]
        public TicketDtoResult Ticket { get; set; }

        [Parameter]
        public ContactDtoResult Contact { get; set; }

        [Parameter]
        public ResourceCache Resource { get; set; }

        [Parameter]
        public CompanyCache Company { get; set; }

        protected NoteTemplate selectedTemplate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            note = new NewNote();
            note.TicketId = Ticket.item.id;
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


        protected async Task noteTemplatesLoadData(LoadDataArgs args)
        {
            try
            {
                string defaultFilter = $"Active eq true";
                var result = await ATTimeService.GetNoteTemplates(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Title asc");

                noteTemplates = result.Value.AsODataEnumerable();
                noteTemplatesCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }

        protected async System.Threading.Tasks.Task TemplateForm0Submit(Server.Models.NewNote args)
        {
            try
            {
                await AutotaskService.CreateNote(new NoteDto()
                {
                    lastActivityDate = DateTime.Now,
                    createDateTime = DateTime.Now,
                    impersonatorCreatorResourceID = Resource.Id,
                    description = note.Description,
                    noteType = note.NoteType,
                    publish = note.NotePublish,
                    ticketID = Ticket.item.id,
                    title = note.Title,
                });
                if (note.TicketStatus != null)
                {
                    var ticketUpdate = new TicketUpdateDto()
                    {
                        Id = note.TicketId,
                        Status = Convert.ToInt32(note.TicketStatus)
                    };
                    await AutotaskService.UpdateTicket(ticketUpdate);
                }
                DialogService.Close(note);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to Create Note" });

            }
        }

        protected async System.Threading.Tasks.Task CancelButton0Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            DialogService.Close();
        }

        protected async System.Threading.Tasks.Task NoteTemplateIdChange(System.Object args)
        {
            try
            {
                selectedTemplate = await ATTimeService.GetNoteTemplateByNoteTemplateId("", note.NoteTemplateId);
                if (selectedTemplate != null)
                {
                    note.Title = selectedTemplate.Title;
                    note.TicketStatus = selectedTemplate.TicketStatus;
                    note.NotePublish = selectedTemplate.NotePublish;
                    note.Description = selectedTemplate.NoteDescription;
                    note.NoteType = selectedTemplate.NoteType;
                }
                
            }
            catch (Exception ex)
            {
                //NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to Create Note" });

            }
        }
    }
}