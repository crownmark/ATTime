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
using System.Net.Mail;
using System.Threading.Tasks;

namespace CrownATTime.Client.Pages
{
    public partial class NewTeamsMessage
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
        protected TemplateTokenDiscoveryService TemplateTokenDiscoveryService { get; set; }

        [Inject]
        protected CrownATTime.Client.ATTimeService ATTimeService { get; set; }

        [Inject]
        protected CrownATTime.Client.AutotaskService AutotaskService { get; set; }

        [Inject]
        protected CrownATTime.Client.TeamsChatService TeamsChatService { get; set; }

        [Inject]
        protected CrownATTime.Client.EmailService EmailService { get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.ResourceCache> resourceCaches;

        protected int resourceCachesCount;

        protected IEnumerable<string> crownTeamEmails { get; set; } = new List<string>();


        protected Server.Models.TeamsMessageRequest teamsMessage { get; set; } = new Server.Models.TeamsMessageRequest();

        protected IEnumerable<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> teamsMessageTemplates;

        protected int teamsMessageTemplatesCount;

        protected int selectedTemplate {  get; set; }

        [Parameter]
        public TicketDtoResult Ticket { get; set; }

        [Parameter]
        public ContactDtoResult Contact { get; set; }

        [Parameter]
        public ResourceCache Resource { get; set; }

        [Parameter]
        public CompanyCache Company { get; set; }

        [Parameter]
        public List<TicketChecklistItemResult> ChecklistItems { get; set; } = new List<TicketChecklistItemResult>();

        [Parameter]
        public ResourceCache TicketResource { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                
            }
            catch (Exception ex)
            {

            }
            
        }

        protected async System.Threading.Tasks.Task TemplateForm0Submit(Server.Models.TeamsMessageRequest args)
        {
            try
            {
                //convert crownTeamEmails into csv
                string csv = string.Join(",", crownTeamEmails);
                teamsMessage.MentionsCsv = csv;

                // Load picklists
                var picklistResult = await ATTimeService.GetTicketEntityPicklistValueCaches();
                var picklistRows = picklistResult?.Value ?? new List<TicketEntityPicklistValueCache>();
                var picklists = EmailService.BuildPicklistMaps(picklistRows);

                var ctx = new TemplateContext
                {
                    Contact = Contact?.item,
                    Ticket = Ticket?.item,
                    Resource = Resource,
                    TicketResource = TicketResource,
                    Company = Company,
                    Picklists = picklists
                };
                teamsMessage.AdaptiveCard = EmailService.Render(teamsMessage.AdaptiveCard ?? string.Empty, ctx);

                // Convert [TeamsMessage.Body} Token to plain text
                teamsMessage.AdaptiveCard = EmailService.ReplaceTeamsMessageBodyTokenOnSubmit(teamsMessage.Message, teamsMessage.AdaptiveCard);

                await TeamsChatService.SendTeamsChannelMessage(args);
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success", Detail = $"Teams Channel Message Sent" });

                DialogService.Close();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Error sending teams message: {ex.Message}" });

            }
        }

        protected async Task resourceCachesLoadData(LoadDataArgs args)
        {
            try
            {
                var defaultFilter = $"IsActive eq true and (LicenseType eq 1 or LicenseType eq 3) and Email ne null and Email ne ''";

                var result = await ATTimeService.GetResourceCaches(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Email, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Email asc");

                resourceCaches = result.Value.AsODataEnumerable();
                resourceCachesCount = result.Count;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to Resources.  Error: {ex.Message}" });
            }
        }


        protected async Task teamsMessageTemplatesLoadData(LoadDataArgs args)
        {
            try
            {
                string defaultFilter = $"ShareWithOthers eq true or contains(TemplateAssignedTo, '{Security.User.Email}')";
                var result = await ATTimeService.GetTeamsMessageTemplates(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Title asc");

                teamsMessageTemplates = result.Value.AsODataEnumerable();
                teamsMessageTemplatesCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }

        protected async System.Threading.Tasks.Task TemplateDropDown0Change(System.Object args)
        {
            try
            {
                var template = await ATTimeService.GetTeamsMessageTemplateByTeamsMessageTemplateId("", selectedTemplate);
                teamsMessage.ChannelId = template.ChannelId;
                teamsMessage.TeamId = template.TeamId;
                teamsMessage.Message = template.Message;
                teamsMessage.AdaptiveCard = template.AdaptiveCardTemplate;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Error getting templat: {ex.Message}" });

            }
        }
    }
}