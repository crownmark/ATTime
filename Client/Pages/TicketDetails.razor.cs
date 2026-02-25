using CrownATTime.Server.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static CrownATTime.Server.Models.EmailMessage;

namespace CrownATTime.Client.Pages
{
    public partial class TicketDetails
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
        protected AutotaskService AutotaskService { get; set; }

        [Parameter]
        public TicketDtoResult Ticket { get; set; }
        [Parameter]
        public string PriorityName { get; set; }
        [Parameter]
        public string StatusName { get; set; }
        [Parameter]
        public string PrimaryResource { get; set; }

        [Parameter]
        public int ResourceId { get; set; }

        protected List<TimeEntryDto> timeEntries { get; set; }  = new List<TimeEntryDto>();
        protected int timeEntriesCount { get; set; }

        protected bool gridLoading { get; set; }       

        protected RadzenDataGrid<TimeEntryDto> timeEntriesGrid;

        protected List<ServiceCall> serviceCalls { get; set; } = new List<ServiceCall>();
        protected int serviceCallsCount { get; set; }
        protected bool serviceCallsLoading { get; set; }

        protected RadzenDataGrid<ServiceCall> serviceCallsGrid;
        protected List<AttachmentDtoResult> attachments { get; set; } = new List<AttachmentDtoResult>();
        protected int attachmentsCount { get; set; }
        protected bool attachmentsLoading { get; set; }

        protected RadzenDataGrid<AttachmentDtoResult> attachmentsGrid;
        protected bool filterTimeEntries {  get; set; } = true;
        protected bool filterNotes { get; set; } = true;
        protected bool filterCommunication { get; set; } = true;
        protected string search = "";



        protected override async Task OnInitializedAsync()
        {
            
        }

        protected async System.Threading.Tasks.Task TimeEntriesDataGridLoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                gridLoading = true;
                timeEntries = new List<TimeEntryDto>();
                if (filterTimeEntries)
                {
                    var result = await AutotaskService.GetTimeEntriesForTicket(Ticket.item.id);

                    timeEntries = result.Items
                        .Where(x =>
                            (x.SummaryNotes?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (x.InternalNotes?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (x.ResourceName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (x.ResourceEmail?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)

                        )
                        .ToList();
                }
                if (filterNotes)
                {
                    var result = await AutotaskService.GetNotesForTicket(Ticket.item.id);
                    var notes = result.Items
                        .Where(x => !x.title.Contains("Customer") &&
                            (
                                (x.description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) || 
                                (x.title?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (x.ResourceName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (x.ResourceEmail?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                        )
                        .ToList();
                    foreach(var note in notes)
                    {
                        timeEntries.Add(new TimeEntryDto()
                        {
                            StartDateTime = note.createDateTime.Value,
                            ResourceId = note.creatorResourceID.Value,
                            ResourceEmail = note.ResourceEmail,
                            ResourceName = note.ResourceName,
                            SummaryNotes = note.publish == 1 ? note.title + Environment.NewLine + Environment.NewLine + note.description : null,
                            InternalNotes = note.publish == 2 ? note.title + Environment.NewLine + Environment.NewLine + note.description : null,
                            isNote = true     
                        });
                    }
                    //timeEntriesCount = timeEntries.Count();
                }
                if (filterCommunication)
                {
                    var result = await AutotaskService.GetNotesForTicket(Ticket.item.id);
                    var notes = result.Items
                        .Where(x => x.title.Contains("Customer") && 
                            (
                                (x.description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (x.title?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (x.ResourceName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (x.ResourceEmail?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                        )
                        .ToList();
                    foreach (var note in notes)
                    {
                        timeEntries.Add(new TimeEntryDto()
                        {
                            StartDateTime = note.createDateTime.Value,
                            ResourceId = note.creatorResourceID.Value,
                            ResourceEmail = note.ResourceEmail,
                            ResourceName = note.ResourceName,
                            SummaryNotes = note.publish == 1 ? note.title + Environment.NewLine + Environment.NewLine + note.description : null,
                            InternalNotes = note.publish == 2 ? note.title + Environment.NewLine + Environment.NewLine + note.description : null,
                            isNote = true
                        });
                    }
                    //timeEntriesCount = timeEntries.Count();
                }
                timeEntries = timeEntries.OrderByDescending(x => x.DateWorked).ToList();
                timeEntriesCount = timeEntries.Count();
                
                gridLoading = false;

            }
            catch (Exception ex)
            {
                gridLoading = false;
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to get activity.  Error: {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task RefreshTimeEntriesButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await timeEntriesGrid.Reload();
        }

        protected async System.Threading.Tasks.Task RefreshServiceCallsButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await serviceCallsGrid.Reload();
        }

        protected async System.Threading.Tasks.Task RefreshAttachmentsButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await attachmentsGrid.Reload();
        }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await timeEntriesGrid.GoToPage(0);

            await timeEntriesGrid.Reload();
        }

        protected async System.Threading.Tasks.Task SearchTimeEntriesButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await timeEntriesGrid.GoToPage(0);

            await timeEntriesGrid.Reload();
        }

        protected async System.Threading.Tasks.Task TimeEntriesSearchCheckBoxChange(bool args)
        {
            await timeEntriesGrid.GoToPage(0);

            await timeEntriesGrid.Reload();
        }

        protected async System.Threading.Tasks.Task NotesSearchCheckBoxChange(bool args)
        {
            await timeEntriesGrid.GoToPage(0);

            await timeEntriesGrid.Reload();
        }

        protected async System.Threading.Tasks.Task SearchTextBoxChange(System.String args)
        {
            await timeEntriesGrid.GoToPage(0);

            await timeEntriesGrid.Reload();
        }

        protected async System.Threading.Tasks.Task Tabs0Change(System.Int32 args)
        {
            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await attachmentsGrid.Reload();
                await timeEntriesGrid.Reload();
                await serviceCallsGrid.Reload();
            }
        }

        protected async System.Threading.Tasks.Task DataGrid1LoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                serviceCallsLoading = true;
                var results = await AutotaskService.GetServiceCallsForTicket(Ticket.item.id);
                serviceCalls = results.Items.OrderByDescending(x => x.startDateTime).ToList();
                serviceCallsCount = serviceCalls.Count();
                serviceCallsLoading = false;
            }
            catch (Exception ex)
            {
                serviceCallsLoading = false;

            }
        }

        protected async System.Threading.Tasks.Task DataGrid2LoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                attachmentsLoading = true;
                var results = await AutotaskService.GetAttachmentsForTicket(Ticket.item.id);
                attachments = results.Items.OrderByDescending(x => x.attachDate).ToList();
                attachmentsCount = serviceCalls.Count();
                attachmentsLoading = false;
            }
            catch (Exception ex)
            {
                attachmentsLoading = false;

            }
        }

        protected async System.Threading.Tasks.Task Upload0Complete(UploadCompleteEventArgs args)
        {
            var attachmentslist = new List<IFormFileModel>();
            attachmentslist.AddRange(JsonSerializer.Deserialize<List<IFormFileModel>>(args.RawResponse));
            foreach (var attachment in attachmentslist)
            {
                try
                {
                    await AutotaskService.CreateTicketAttachment(new AttachmentCreateDto()
                    {
                        attachDate = DateTime.Now,
                        attachedByContactID = null,
                        attachedByResourceID = ResourceId,
                        id = 0,
                        attachmentType = "FILE_ATTACHMENT",
                        contentType = attachment.ContentType,
                        publish = 1,
                        ticketID = Ticket.item.id,
                        title = attachment.FileName,
                        fullPath = attachment.FileName,
                        data = Convert.ToBase64String(attachment.ByteArray)
                    });
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{ex.Message}, Make sure the file is less than 10MB.", Duration = 5000 });

                }

            }
            await attachmentsGrid.Reload();
            attachmentsLoading = false;

        }
        protected async System.Threading.Tasks.Task Upload0Error(UploadErrorEventArgs args)
        {
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{args.Message}", Duration = 5000 });
        }

        protected async System.Threading.Tasks.Task Upload0Progress(UploadProgressArgs args)
        {
            attachmentsLoading = true;
        }

        protected async System.Threading.Tasks.Task DeleteAttachmentButton2Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args, AttachmentDtoResult data)
        {
            try
            {
                await AutotaskService.DeleteAttachment(data);
                await attachmentsGrid.Reload();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{ex.Message}", Duration = 5000 });

            }
        }
    }
}