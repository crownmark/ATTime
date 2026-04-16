namespace CrownATTime.Client.Helpers
{
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Components;
    public static class TextHelpers
    {
        public static MarkupString Linkify(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new MarkupString(string.Empty);

            // 1. Convert URLs first
            var urlPattern = @"(https?:\/\/[^\s]+)";
            var result = Regex.Replace(text, urlPattern, match =>
            {
                var url = match.Value;

                // Detect Teams message links
                var isTeamsLink = url.Contains("teams.microsoft.com/l/message");

                var label = isTeamsLink
                    ? "Open Teams Message"
                    : url; // default to showing the URL itself

                return $"<a href=\"{url}\" target=\"_blank\" rel=\"noopener noreferrer\">{label}</a>";
            });

            // 2. Convert Ticket Numbers
            var ticketPattern = @"\bT\d{8}\.\d{4}\b";

            result = Regex.Replace(result, ticketPattern, match =>
            {
                var ticket = match.Value;

                var ticketUrl = $"https://ww5.autotask.net/Autotask/AutotaskExtend/ExecuteCommand.aspx?Code=OpenTicketDetail&TicketNumber={ticket}";

                return $"<a href=\"{ticketUrl}\" target=\"_blank\">{ticket}</a>";
            });

            // 3. Preserve line breaks
            result = result.Replace("\n", "<br>");

            return new MarkupString(result);
        }
    }
}
