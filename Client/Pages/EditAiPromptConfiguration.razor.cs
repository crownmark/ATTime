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
    public partial class EditAiPromptConfiguration
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
        public int AiPromptConfigurationId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            aiPromptConfiguration = await ATTimeService.GetAiPromptConfigurationByAiPromptConfigurationId(aiPromptConfigurationId:AiPromptConfigurationId);
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.AiPromptConfiguration aiPromptConfiguration;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TimeGuardSection> timeGuardSectionsForTimeGuardSectionsId;


        protected int timeGuardSectionsForTimeGuardSectionsIdCount;
        protected CrownATTime.Server.Models.ATTime.TimeGuardSection timeGuardSectionsForTimeGuardSectionsIdValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task timeGuardSectionsForTimeGuardSectionsIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ATTimeService.GetTimeGuardSections(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(SectionName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                timeGuardSectionsForTimeGuardSectionsId = result.Value.AsODataEnumerable();
                timeGuardSectionsForTimeGuardSectionsIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TimeGuardSection" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.UpdateAiPromptConfiguration(aiPromptConfigurationId:AiPromptConfigurationId, aiPromptConfiguration);
                DialogService.Close(aiPromptConfiguration);
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