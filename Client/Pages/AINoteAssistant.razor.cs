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
    public partial class AINoteAssistant
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
        [Parameter]
        public string Note { get; set; }

        protected RadzenAIChat basicChat = new RadzenAIChat();

        protected override async Task OnInitializedAsync()
        {
            //await basicChat.SendMessage($"Please clean this for my client facing time entry:  {Note}");
        }

        protected async System.Threading.Tasks.Task AIChat0MessageAdded(Radzen.Blazor.ChatMessage args)
        {

        }

        protected async System.Threading.Tasks.Task AIChat0MessageSent(System.String args)
        {
        }

        protected async System.Threading.Tasks.Task AIChat0ResponseReceived(System.String args)
        {
        }

        protected async System.Threading.Tasks.Task AIChat0ChatCleared()
        {
        }

       

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await basicChat.SendMessage($"This is a time entry in our ticketing system.  I would like this formated clearly so if the client see's this it's professional and they understand. The output will need to be in plain text and must be in paragraph form with paragraph breaks to look nice and clean. I only want the ouput with no reply and followup prompt.  Current Ticket Time Entry:  {Note}");

            }
            else
            {
            }
        }
    }
}