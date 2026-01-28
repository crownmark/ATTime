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
    public partial class AIGeneralAssistant
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

        
        [Parameter]
        public AiPromptConfiguration Prompt { get; set; }

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
            //if (firstRender)
            //{
            //    await basicChat.SendMessage($"{Prompt.UserPrompt} Message:{Environment.NewLine}{Note}");
                
            //}
            //else
            //{
            //}
        }
    }
}