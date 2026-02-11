namespace CrownATTime.Server.Models
{
    public class TeamsMessageRequest
    {

        public string ChatId { get; set; }

        // ➜ Needed for channel posts
        public string TeamId { get; set; }
        public string ChannelId { get; set; }

        public string Message { get; set; }

        // "Philip:entraId,Mark:entraId"
        public string MentionsCsv { get; set; }

        public string AdaptiveCard { get; set; }
    }

    public class TeamsCreateChatRequest
    {
        public string Topic { get; set; }

        public List<string> UserEntraIds { get; set; }
    }

}
