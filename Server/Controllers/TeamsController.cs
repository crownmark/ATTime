using CrownATTime.Server.Models;
using CrownATTime.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Net;
using System.Text.Json;

namespace CrownATTime.Server.Controllers
{
    [ApiController]
    public class TeamsController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IServiceScopeFactory scopeFactory;

        public TeamsController(
            IWebHostEnvironment environment,
            IServiceScopeFactory scopeFactory)
        {
            this.environment = environment;
            this.scopeFactory = scopeFactory;
        }

        [HttpPost("Teams/SendChannelMessage")]
        public async Task<ActionResult> SendChannelMessage([FromBody] TeamsMessageRequest item)
        {
            try
            {
                var GraphApiService =
                    (GraphApi)HttpContext.RequestServices.GetService(typeof(GraphApi));

                var graphClient = GraphApiService.CreateGraphClient();

                var message = TeamsMessageFactory.BuildGraphPayloadJson(item);

                // Deserialize to dynamic object
                var doc = JsonDocument.Parse(message);
                var root = doc.RootElement;

                // ----- Build ChatMessage -----

                var chatMessage = new Microsoft.Graph.Models.ChatMessage
                {
                    Body = new Microsoft.Graph.Models.ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = root.GetProperty("body")
                                      .GetProperty("content")
                                      .GetString()
                    }
                };

                // Mentions
                if (root.TryGetProperty("mentions", out var mentionsEl))
                {
                    chatMessage.Mentions = new List<ChatMessageMention>();

                    int index = 0;

                    foreach (var m in mentionsEl.EnumerateArray())
                    {
                        var emailOrId =
                            m.GetProperty("mentioned")
                             .GetProperty("user")
                             .GetProperty("id")
                             .GetString();

                        // 👇 THIS IS THE KEY PART
                        var userId = await GetUserIdByEmailAsync(emailOrId);

                        if (string.IsNullOrEmpty(userId))
                            continue;   // skip invalid users

                        chatMessage.Mentions.Add(new ChatMessageMention
                        {
                            Id = index,

                            MentionText =
                                m.GetProperty("mentionText").GetString(),

                            Mentioned = new ChatMessageMentionedIdentitySet
                            {
                                User = new Identity
                                {
                                    Id = userId   // <-- now real Entra ID
                                }
                            }
                        });

                        index++;
                    }
                }


                // Attachment (adaptive card)
                if (root.TryGetProperty("attachments", out var attEl))
                {
                    var first = attEl[0];

                    chatMessage.Attachments = new List<ChatMessageAttachment>
                    {
                        new ChatMessageAttachment
                        {
                            ContentType =
                                "application/vnd.microsoft.card.adaptive",

                            Content =
                                first.GetProperty("content")
                                     .GetRawText()
                        }
                    };
                }

               

                // ✅ CHANNEL SEND (the only real difference)
                await graphClient
                    .Teams[item.TeamId]
                    .Channels[item.ChannelId]
                    .Messages
                    .PostAsync(chatMessage);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Teams/GetUserByEmail")]

        public async Task<string?> GetUserIdByEmailAsync(string email)
        {
            var GraphApiService =
                    (GraphApi)HttpContext.RequestServices.GetService(typeof(GraphApi));

            var graphClient = GraphApiService.CreateGraphClient();
            var result = await graphClient.Users
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Filter =
                        $"mail eq '{email}'";
                });

            return result?.Value?.FirstOrDefault()?.Id;
        }


        [HttpPost("Teams/SendChatMessage")]
        public async Task<ActionResult> SendChatMessage([FromBody] TeamsMessageRequest item)
        {
            try
            {
                var GraphApiService =
                    (GraphApi)HttpContext.RequestServices.GetService(typeof(GraphApi));

                var graphClient = GraphApiService.CreateGraphClient();


                var message = TeamsMessageFactory.BuildGraphPayloadJson(item);

                // Deserialize to dynamic object
                var doc = JsonDocument.Parse(message);
                var root = doc.RootElement;

                // ----- Build ChatMessage -----

                var chatMessage = new ChatMessage
                {
                    Body = new Microsoft.Graph.Models.ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = root.GetProperty("body")
                                      .GetProperty("content")
                                      .GetString()
                    }
                };

                // Mentions
                if (root.TryGetProperty("mentions", out var mentionsEl))
                {
                    chatMessage.Mentions = new List<ChatMessageMention>();

                    foreach (var m in mentionsEl.EnumerateArray())
                    {
                        chatMessage.Mentions.Add(new ChatMessageMention
                        {
                            Id = m.GetProperty("id").GetInt32(),

                            MentionText =
                                m.GetProperty("mentionText").GetString(),

                            Mentioned = new Microsoft.Graph.Models.ChatMessageMentionedIdentitySet
                            {
                                User = new Identity
                                {
                                    Id = m.GetProperty("mentioned")
                                          .GetProperty("user")
                                          .GetProperty("id")
                                          .GetString()
                                }
                            }
                        });
                    }
                }

                // Attachment (adaptive card)
                if (root.TryGetProperty("attachments", out var attEl))
                {
                    var first = attEl[0];

                    chatMessage.Attachments = new List<ChatMessageAttachment>
                    {
                        new ChatMessageAttachment
                        {
                            ContentType =
                                "application/vnd.microsoft.card.adaptive",

                            Content =
                                first.GetProperty("content")
                                     .GetRawText()
                        }
                    };
                }

                // Send to existing chat
                await graphClient
                    .Chats[item.ChatId]
                    .Messages
                    .PostAsync(chatMessage);



                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Teams/CreateGroupChat")]
        public async Task<ActionResult<string>> CreateGroupChat(
            [FromBody] TeamsCreateChatRequest item)
        {
            try
            {
                var GraphApiService =
                    (GraphApi)HttpContext.RequestServices.GetService(typeof(GraphApi));

                var graphClient = GraphApiService.CreateGraphClient();

                var members = new List<ConversationMember>();

                foreach (var user in item.UserEntraIds)
                {
                    members.Add(new AadUserConversationMember
                    {
                        Roles = new List<string> { "owner" },
                        AdditionalData = new Dictionary<string, object>
                        {
                            {
                                "user@odata.bind",
                                $"https://graph.microsoft.com/v1.0/users/{user}"
                            }
                        }
                    });
                }

                var chat = new Chat
                {
                    ChatType = ChatType.Group,
                    Topic = item.Topic,
                    Members = members
                };

                var created =
                    await graphClient.Chats.PostAsync(chat);

                return Ok(created.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
