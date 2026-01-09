using CrownATTime.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using System.Net.Mail;

namespace CrownATTime.Server.Controllers
{
    public class EmailController : Controller
    {
        private CrownATTime.Server.Data.ATTimeContext context;
        private readonly IWebHostEnvironment environment;
        string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
        string graphBaseUrl = "https://graph.microsoft.com/v1.0";
        private readonly IServiceScopeFactory scopeFactory;

        public EmailController(IWebHostEnvironment environment, CrownATTime.Server.Data.ATTimeContext context, IServiceScopeFactory scopeFactory)
        {
            this.context = context;
            this.environment = environment;
            this.scopeFactory = scopeFactory;
        }
        [HttpPost("Email/SendEmailFromSupport")]
        public async Task<ActionResult> SendEmailFromSupport([FromBody] CrownATTime.Server.Models.EmailMessage item)
        {

            var CrownCrmDb = (ATTimeService)HttpContext.RequestServices.GetService(typeof(ATTimeService));

            try
            {
                var attachmentsList = new List<Microsoft.Graph.Models.Attachment>();
                if (item.Attachments != null)
                {
                    foreach (var attachment in item.Attachments)
                    {
                        MemoryStream memoryStream = new MemoryStream(attachment.ByteArray);
                        attachmentsList.Add(new FileAttachment()
                        {
                            OdataType = "#microsoft.graph.fileAttachment",
                            Name = attachment.FileName,
                            ContentBytes = attachment.ByteArray,
                            ContentType = "application/octet-stream",
                        });
                    }
                }
                var replyToList = new List<Recipient>();
                //if (!string.IsNullOrEmpty(item.From))
                //{
                //    if(item.From != "support@ce-technology.com")
                //    {
                //        replyToList.Add(new Recipient()
                //        {
                //            EmailAddress = new EmailAddress
                //            {
                //                Address = item.From,
                //            }
                //        });
                //    }
                //    else
                //    {
                        
                //    }
                //    replyToList.Add(new Recipient()
                //    {
                //        EmailAddress = new EmailAddress
                //        {
                //            Address = "support@ce-technology.com",
                //        }
                //    });
                //}
                //else
                //{
                //    replyToList.Add(new Recipient()
                //    {
                //        EmailAddress = new EmailAddress
                //        {
                //            Address = "support@ce-technology.com",
                //        }
                //    });
                //}

                // Allways reply to support only
                replyToList.Add(new Recipient()
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = "support@ce-technology.com",
                    }
                });

                var emailAddresses = item.To.Split(',');
                var emailToList = new List<Recipient>();
                if (emailAddresses.Any())
                {
                    try
                    {
                        foreach (var email in emailAddresses)
                        {
                            if (!string.IsNullOrEmpty(email))
                            {
                                emailToList.Add(new Recipient()
                                {
                                    EmailAddress = new EmailAddress
                                    {
                                        Address = email.TrimEnd()
                                    }
                                });
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                var message = new Message
                {
                    Subject = item.Subject,
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = item.Body
                    },                    
                    ReplyTo = replyToList,
                    ToRecipients = emailToList,
                    Attachments = attachmentsList
                };

                var requestBody = new SendMailPostRequestBody
                {
                    Message = message,
                    SaveToSentItems = true
                };
                var GraphApiService = (GraphApi)HttpContext.RequestServices.GetService(typeof(GraphApi));
                var graphClient = GraphApiService.CreateGraphClient();
                await graphClient.Users[item.From].SendMail.PostAsync(requestBody);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        
    }
}
