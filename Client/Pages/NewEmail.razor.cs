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
using CrownATTime.Server.Models.ATTime;

namespace CrownATTime.Client.Pages
{
    public partial class NewEmail
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
        protected EmailService EmailService { get; set; }

        protected EmailMessage emailMessage { get; set; } = new EmailMessage();

        [Inject]
        protected CrownATTime.Client.ATTimeService ATTimeService { get; set; }

        [Inject]
        protected CrownATTime.Client.AutotaskTicketService AutotaskTicketService { get; set; }

        [Inject]
        protected CrownATTime.Client.AutotaskTimeEntryService AutotaskTimeEntryService { get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.EmailTemplate> emailTemplates;

        protected int emailTemplatesCount;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.ResourceCache> resourceCaches;

        protected int resourceCachesCount;

        protected IEnumerable<ContactDtoResult.Item> contacts;


        protected IEnumerable<string> crownTeamEmails {  get; set; } = new List<string>();
        protected IEnumerable<string> contactEmails {  get; set; } = new List<string>();

        protected bool ticketContact {  get; set; }
        protected bool ticketContacts {  get; set; }
        protected bool primaryResource {  get; set; }
        protected bool primaryResources {  get; set; }


        [Parameter] 
        public TicketDtoResult Ticket {  get; set; }

        [Parameter]
        public ContactDtoResult Contact { get; set; }

        [Parameter]
        public ResourceCache Resource { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await AutotaskTicketService.GetContacts(Ticket.item.companyID);
                contacts = results.Items;

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load contacts" });
            }
        }

        protected async Task emailTemplatesLoadData(LoadDataArgs args)
        {
            try
            {
                var defaultFilter = $"Active eq true and (ShareWithOthers eq true or TemplateAssignedTo eq '{Resource.Email}')";
                var result = await ATTimeService.GetEmailTemplates(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"{defaultFilter} and contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"Title asc");

                emailTemplates = result.Value.AsODataEnumerable();
                emailTemplatesCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
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


        //protected async System.Threading.Tasks.Task TemplateIdChange(System.Object args)
        //{
        //    try
        //    {
        //        var template = await ATTimeService.GetEmailTemplateByEmailTemplateId("", emailMessage.TemplateId);
        //        var picklistRows = await ATTimeService.GetTicketEntityPicklistValueCaches();

        //        var picklists = EmailService.BuildPicklistMaps(picklistRows.Value.ToList());
        //        var ctx = new TemplateContext()
        //        {
        //            Contact = Contact?.item,
        //            Ticket = Ticket.item,
        //            Resource = Resource,
        //            Picklists = picklists

        //        };
        //        if (template != null)
        //        {
        //            emailMessage.From = template.FromEmailAddress;
        //            emailMessage.Subject = EmailService.Render(template.EmailSubject, ctx);
        //            emailMessage.Body = EmailService.Render(template.EmailBody, ctx);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to apply template" });

        //    }
        //}

        protected async System.Threading.Tasks.Task TemplateIdChange(System.Object args)
        {
            try
            {
                if (emailMessage?.TemplateId == null)
                    return;

                // Load template
                var template = await ATTimeService.GetEmailTemplateByEmailTemplateId("", emailMessage.TemplateId);
                if (template == null)
                    return;

                // Load picklists
                var picklistResult = await ATTimeService.GetTicketEntityPicklistValueCaches();
                var picklistRows = picklistResult?.Value ?? new List<TicketEntityPicklistValueCache>();

                // OPTIONAL: filter to what Render() will translate
                // (If your Render translates only Ticket.status + Ticket.priority, keep the cache small)
                //picklistRows = picklistRows
                //    .Where(x =>
                //        x.PicklistName.Equals("Ticket.status", StringComparison.OrdinalIgnoreCase) ||
                //        x.PicklistName.Equals("Ticket.priority", StringComparison.OrdinalIgnoreCase))
                //    .ToList();

                var picklists = EmailService.BuildPicklistMaps(picklistRows);

                var ctx = new TemplateContext
                {
                    Contact = Contact?.item,
                    Ticket = Ticket?.item,   
                    Resource = Resource,     
                    Picklists = picklists
                };

                // Apply template + render tokens
                emailMessage.From = template.FromEmailAddress;

                emailMessage.Subject = EmailService.Render(template.EmailSubject ?? string.Empty, ctx);
                emailMessage.Body = EmailService.Render(template.EmailBody ?? string.Empty, ctx);
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "Unable to apply template"
                });
            }
        }




        protected async System.Threading.Tasks.Task TemplateForm0Submit(Server.Models.EmailMessage args)
        {
            try
            {
                // Collect emails here (case-insensitive, deduped)
                var emailSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                void AddCsv(string? csv)
                {
                    if (string.IsNullOrWhiteSpace(csv)) return;

                    foreach (var e in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                        emailSet.Add(e);
                }

                var customerEmailsCsv = EmailService.ToEmailCsv(contactEmails);
                var crownEmailsCsv = EmailService.ToEmailCsv(crownTeamEmails);

                AddCsv(customerEmailsCsv);
                AddCsv(crownEmailsCsv);

                if (ticketContact)
                {
                    var ticketContactDto = await AutotaskTicketService.GetContact(Convert.ToInt32(Ticket.item.contactID));

                    // Autotask contacts typically expose emailAddress
                    var email = ticketContactDto?.item.emailAddress;

                    if (!string.IsNullOrWhiteSpace(email))
                        emailSet.Add(email.Trim());
                }

                if (primaryResource)
                {
                    var primaryResourceDto = await AutotaskTimeEntryService.GetResourceById(Convert.ToInt32(Ticket.item.assignedResourceID));

                    // resources often expose email
                    var email = primaryResourceDto?.item.email;

                    if (!string.IsNullOrWhiteSpace(email))
                        emailSet.Add(email.Trim());
                }

                var allRecipients = string.Join(",",emailSet.Select(e => e.Trim()));
                emailMessage.To = allRecipients;
                if (string.IsNullOrEmpty(emailMessage.To))
                {
                    NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "No email addresses set" });
                }
                else
                {
                    await EmailService.SendEmail(emailMessage, Resource);
                    NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success", Detail = $"Email Sent" });
                    //Create Note
                    var newNote = new NoteDto()
                    {
                        lastActivityDate = DateTime.Now,
                        createDateTime = DateTime.Now,
                        creatorResourceID = Resource.Id,
                        description = $"{EmailService.BuildEmailNoteDescription(emailMessage.From,emailMessage.To, emailMessage.CC, emailMessage.Subject, emailMessage.Body)}",
                        //impersonatorCreatorResourceID = Resource.Id,
                        noteType = 204,
                        publish = 1,
                        ticketID = Ticket.item.id,
                        title = "Email To Customer Communication",
                    };
                    await AutotaskTimeEntryService.CreateNote(newNote);
                    //Close Dialog
                    DialogService.Close();
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Error sending email.  {ex.Message}" });

            }
        }

        protected async System.Threading.Tasks.Task CancelButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            DialogService.Close();
        }
    }
}