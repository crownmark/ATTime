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
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static CrownATTime.Server.Models.MicrosoftEmailAttachments;

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
        protected CrownATTime.Client.AutotaskService AutotaskService { get; set; }

        

        protected IEnumerable<CrownATTime.Server.Models.ATTime.EmailTemplate> emailTemplates;

        protected int emailTemplatesCount;

        protected IEnumerable<CrownATTime.Server.Models.ATTime.ResourceCache> resourceCaches;

        protected int resourceCachesCount;

        protected IEnumerable<ContactDtoResult.Item> contacts;
        protected IEnumerable<string> crownTeamEmails {  get; set; } = new List<string>();
        protected IEnumerable<string> contactEmails {  get; set; } = new List<string>();

        protected bool ticketContact {  get; set; }
        protected bool msTeamsEmail { get; set; }

        protected bool ticketContacts {  get; set; }
        protected bool primaryResource {  get; set; }
        protected bool secondaryResources {  get; set; }
        protected bool sendingEmail {  get; set; }
        protected bool quoteTemplate {  get; set; }
        protected string additionalEmail {  get; set; }


        [Parameter] 
        public TicketDtoResult Ticket {  get; set; }

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

        protected EmailTemplate selectedTemplate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await AutotaskService.GetContacts(Ticket.item.companyID);
                contacts = results.Items
                    .Where(c => !string.IsNullOrWhiteSpace(c.emailAddress))
                    .GroupBy(c => c.emailAddress.Trim(), StringComparer.OrdinalIgnoreCase)
                    .Select(g => g.First())
                    .ToList();
                if(Ticket.item.userDefinedFields.Any(x => x.name == "Quoter Quote Link"))
                {
                    var quoteUdf = Ticket.item.userDefinedFields.FirstOrDefault(x => x.name == "Quoter Quote Link");
                    emailMessage.QuoteLink = quoteUdf.value;
                }
                var checklistResults = await AutotaskService.GetAllTicketChecklistItemsCompletedToday(Ticket.item.id);
                if (checklistResults.Any())
                {
                    ChecklistItems = checklistResults;
                }

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

                selectedTemplate = template;
                if (selectedTemplate.Title.Contains("Quote"))
                {
                    quoteTemplate = true;
                }
                else
                {
                    quoteTemplate = false;
                }
                if (!string.IsNullOrEmpty(selectedTemplate.TeamsChannelEmail))
                {
                    additionalEmail = selectedTemplate.TeamsChannelEmail;
                    msTeamsEmail = true;
                }

                if (selectedTemplate.NotifyTicketContact)
                {
                    ticketContact = true;
                }
                if (selectedTemplate.NotifyTicketAdditionalContacts)
                {
                    ticketContacts = true;
                }
                if (selectedTemplate.NotifyTicketPrimaryResource)
                {
                    primaryResource = true;
                }
                if (selectedTemplate.NotifyTicketSecondaryResources)
                {
                    secondaryResources = true;
                }

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

                // Apply template + render tokens
                emailMessage.From = template.SendAsTech ? Security.User.Email : "support@ce-technology.com";// template.FromEmailAddress;

                // Update Checlist Items Token with checklist items
                emailMessage.Body = EmailService.ReplaceChecklistItemsToken(template.EmailBody ?? string.Empty, ChecklistItems);

                emailMessage.Subject = EmailService.Render(template.EmailSubject ?? string.Empty, ctx);
                emailMessage.Body = EmailService.Render(emailMessage.Body ?? string.Empty, ctx);
                StateHasChanged();
                
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
                sendingEmail = true;

                // Update QuoteLink Token to Ticket UDF Value
                emailMessage.Body = EmailService.ReplaceQuoteLinkToken(emailMessage.Body, emailMessage);

                // Convert [EmailMessage.Body} Token to plain text
                emailMessage.Body = EmailService.ReplaceEmailBodyTokenOnSubmit(emailMessage);

                
                
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
                AddCsv(additionalEmail);

                if (ticketContact && Ticket.item.contactID != null)
                {
                    var ticketContactDto = await AutotaskService.GetContact(Convert.ToInt32(Ticket.item.contactID));

                    // Autotask contacts typically expose emailAddress
                    var email = ticketContactDto?.item.emailAddress;

                    if (!string.IsNullOrWhiteSpace(email))
                        emailSet.Add(email.Trim());
                }

                if (primaryResource && Ticket.item.assignedResourceID != null)
                {
                    var primaryResourceDto = await AutotaskService.GetResourceById(Convert.ToInt32(Ticket.item.assignedResourceID));

                    // resources often expose email
                    var email = primaryResourceDto?.item.email;

                    if (!string.IsNullOrWhiteSpace(email))
                        emailSet.Add(email.Trim());
                }

                if (ticketContacts)
                {
                    var ticketAdditionalContacts = await AutotaskService.GetAdditionalContacts(Ticket.item.id);
                    if(ticketAdditionalContacts != null)
                    {
                        foreach (var contact in ticketAdditionalContacts.Items)
                        {
                            var ticketContact = await AutotaskService.GetContact(contact.contactID);
                            AddCsv(ticketContact.item.emailAddress);
                        }
                    }
                    
                }

                if (secondaryResources)
                {
                    var ticketSecondaryResources = await AutotaskService.GetSecondaryResources(Ticket.item.id);
                    if (ticketSecondaryResources != null)
                    {
                        foreach (var secondaryResource in ticketSecondaryResources.Items)
                        {
                            var ticketSecondaryResource = await AutotaskService.GetResourceById(secondaryResource.resourceID);
                            AddCsv(ticketSecondaryResource.item.email);
                        }
                    }

                }


                var allRecipients = string.Join(",",emailSet.Select(e => e.Trim()));
                emailMessage.To = allRecipients;
                if (string.IsNullOrEmpty(emailMessage.To))
                {
                    sendingEmail = false;
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
                        impersonatorCreatorResourceID = Resource.Id,
                        description = $"{EmailService.BuildEmailNoteDescription(emailMessage.From,emailMessage.To, emailMessage.CC, emailMessage.Subject, emailMessage.Body)}",
                        //impersonatorCreatorResourceID = Resource.Id,
                        noteType = 204,
                        publish = 1,
                        ticketID = Ticket.item.id,
                        title = "Email To Customer Communication",
                    };
                    await AutotaskService.CreateNote(newNote);

                    //Update Ticket Status
                    if (selectedTemplate != null && selectedTemplate.TicketStatus.HasValue)
                    {
                        //update ticket status
                        var ticket = new TicketUpdateDto()
                        {
                            Id = Ticket.item.id,
                            Status = selectedTemplate.TicketStatus.Value,                            
                        };
                        await AutotaskService.UpdateTicket(ticket);
                    }
                    sendingEmail = false;

                    //Close Dialog
                    DialogService.Close();
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Error sending email.  {ex.Message}" });
                sendingEmail = false;
            }
        }

        protected async System.Threading.Tasks.Task CancelButtonClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            DialogService.Close();
        }

        protected async System.Threading.Tasks.Task notifyMSTeamsChannelCheckBoxChange(bool args)
        {
            additionalEmail = string.IsNullOrWhiteSpace(additionalEmail) ? selectedTemplate.TeamsChannelEmail : $"{selectedTemplate.TeamsChannelEmail},{additionalEmail}";
        }

        protected async System.Threading.Tasks.Task QuoteLinkChange(System.String args)
        {
            try
            {
                if (!string.IsNullOrEmpty(args))
                {
                    Ticket = await AutotaskService.GetTicket(Ticket.item.id);
                    var listUdf = new List<TicketUpdateDto.Userdefinedfield>();
                    var updateUdf = new TicketUpdateDto.Userdefinedfield()
                    {
                        name = "Quoter Quote Link",
                        value = args.ToString()
                    };
                    listUdf.Add(updateUdf);
                    var updateTicket = new TicketUpdateDto();
                    updateTicket.Status = Ticket.item.status;
                    updateTicket.Id = Ticket.item.id;
                    updateTicket.userDefinedFields = listUdf.ToArray();
                    await AutotaskService.UpdateTicket(updateTicket);
                        
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Error updating Ticket Quoter Quote Link.  {ex.Message}" });

            }
        }
    }
}