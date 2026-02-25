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

        protected List<TimeEntryDto> timeEntries { get; set; }  = new List<TimeEntryDto>();
        protected int timeEntriesCount { get; set; }

        protected bool gridLoading { get; set; }

        protected List<ServiceCall> serviceCalls { get; set; } = new List<ServiceCall>();
        protected int serviceCallsCount { get; set; }

        protected bool serviceCallsLoading { get; set; }
        protected RadzenDataGrid<ServiceCall> serviceCallsGrid;

        protected RadzenDataGrid<TimeEntryDto> timeEntriesGrid;
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
    }
}