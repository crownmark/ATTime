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

        protected IEnumerable<TimeEntryDto> timeEntries { get; set; }  
        protected int timeEntriesCount { get; set; }

        protected override async Task OnInitializedAsync()
        {
        }

        protected async System.Threading.Tasks.Task TimeEntriesDataGridLoadData(Radzen.LoadDataArgs args)
        {
            try
            {
                var result = await AutotaskService.GetTimeEntriesForTicket(Ticket.item.id);
                timeEntries = result;
                timeEntriesCount = timeEntries.Count();
            }
            catch (Exception ex)
            {

            }
        }
    }
}