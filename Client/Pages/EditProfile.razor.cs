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
    public partial class EditProfile
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


        protected override async Task OnInitializedAsync()
        {
            var result = await ATTimeService.GetResourceCaches(filter: $"Email eq '{Security.User.Email}'");
            resourceCache = result.Value.FirstOrDefault();
        }
        protected bool errorVisible;
        protected CrownATTime.Server.Models.ATTime.ResourceCache resourceCache;

        [Inject]
        protected SecurityService Security { get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.EmailTemplate> emailTemplates;

        protected int emailTemplatesCount;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> timeEntryTemplates;

        protected int timeEntryTemplatesCount;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> teamsMessageTemplates;

        protected int teamsMessageTemplatesCount;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.NoteTemplate> noteTemplates;

        protected int noteTemplatesCount;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> aiPromptConfigurations;

        protected int aiPromptConfigurationsCount;



        protected async Task FormSubmit()
        {
            try
            {
                await ATTimeService.UpdateResourceCache(resourceCache.Id, resourceCache);
                DialogService.Close(resourceCache);
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


        protected async Task emailTemplatesLoadData(LoadDataArgs args)
        {
            try
            {
                var defaultFilter = $"Active eq true and (ShareWithOthers eq true or contains(TemplateAssignedTo, '{resourceCache.Email}'))";
                var result = await ATTimeService.GetEmailTemplates(new Query { Top = args.Top, Skip = args.Skip, Filter = $"{defaultFilter} and contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", OrderBy = $"Title asc" });

                emailTemplates = result.Value.AsODataEnumerable();
                emailTemplatesCount = result.Count;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to load email entry templates.  {ex.Message}" });

            }
        }


        protected async Task timeEntryTemplatesLoadData(LoadDataArgs args)
        {
            try
            {
                var defaultFilter = $"Active eq true and (ShareWithOthers eq true or contains(TemplateAssignedTo, '{resourceCache.Email}'))";


                var result = await ATTimeService.GetTimeEntryTemplates(new Query { Top = args.Top, Skip = args.Skip, Filter = $"{defaultFilter} and contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", OrderBy = $"Title asc" });

                timeEntryTemplates = result.Value.AsODataEnumerable();
                timeEntryTemplatesCount = result.Count;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to load time entry templates.  {ex.Message}" });
            }
        }


        protected async Task teamsMessageTemplatesLoadData(LoadDataArgs args)
        {
            try
            {
                var defaultFilter = $"Active eq true and (ShareWithOthers eq true or contains(TemplateAssignedTo, '{resourceCache.Email}'))";


                var result = await ATTimeService.GetTeamsMessageTemplates(new Query { Top = args.Top, Skip = args.Skip, Filter = $"{defaultFilter} and contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", OrderBy = $"Title asc" });

                teamsMessageTemplates = result.Value.AsODataEnumerable();
                teamsMessageTemplatesCount = result.Count;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to load teams message templates.  {ex.Message}" });

            }
        }


        protected async Task noteTemplatesLoadData(LoadDataArgs args)
        {
            try
            {
                var defaultFilter = $"Active eq true";


                var result = await ATTimeService.GetNoteTemplates(new Query { Top = args.Top, Skip = args.Skip, Filter = $"{defaultFilter} and contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", OrderBy = $"Title asc" });

                noteTemplates = result.Value.AsODataEnumerable();
                noteTemplatesCount = result.Count;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to load note templates.  {ex.Message}" });

            }
        }


        protected async Task aiPromptConfigurationsLoadData(LoadDataArgs args)
        {
            try
            {
                var defaultFilter = $"Active eq true and (SharedWithEveryone eq true or contains(SharedWithUsers, '{resourceCache.Email}'))";


                var result = await ATTimeService.GetAiPromptConfigurations(new Query { Top = args.Top, Skip = args.Skip, Filter = $"{defaultFilter} and contains(Name, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", OrderBy = $"Name asc" });

                aiPromptConfigurations = result.Value.AsODataEnumerable();
                aiPromptConfigurationsCount = result.Count;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to load AI Prompt templates.  {ex.Message}" });

            }
        }


        
    }
}