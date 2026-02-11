namespace CrownATTime.Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.Json;

    public static class TeamsMessageFactory
    {
        public static string BuildGraphPayloadJson(TeamsMessageRequest req)
        {
            var (html, mentions) = BuildMentions(req.MentionsCsv);

            var bodyText = string.IsNullOrWhiteSpace(html)
                ? WebUtility.HtmlEncode(req.Message)
                : $"{html} — {WebUtility.HtmlEncode(req.Message)}";

            var payload = new
            {
                body = new
                {
                    contentType = "html",
                    content = bodyText
                },

                mentions = mentions,

                attachments = new[]
                {
                new
                {
                    contentType = "application/vnd.microsoft.card.adaptive",
                    content = req.AdaptiveCard
                }
            }
            };

            return JsonSerializer.Serialize(payload);
        }

        private static (string Html, List<object> Mentions) BuildMentions(string input)
        {
            var mentions = new List<object>();

            if (string.IsNullOrWhiteSpace(input))
                return (string.Empty, mentions);

            var parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

            int index = 0;
            var seen = new HashSet<string>();
            var html = "";

            foreach (var part in parts)
            {
                var pair = part.Contains(":")
                    ? part.Split(':', 2)
                    : part.Split('|', 2);

                if (pair.Length != 2)
                    continue;

                var name = WebUtility.HtmlEncode(pair[0].Trim());
                var userId = pair[1].Trim();

                if (!seen.Add(userId))
                    continue;

                html += $"<at id=\"{index}\">{name}</at> ";

                mentions.Add(new
                {
                    id = index,
                    mentionText = name,
                    mentioned = new
                    {
                        user = new { id = userId }
                    }
                });

                index++;
            }

            return (html.Trim(), mentions);
        }
    }
}
