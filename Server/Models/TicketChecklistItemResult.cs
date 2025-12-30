using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models
{
    public class TicketChecklistItemResult
    {
        public int? completedByResourceID { get; set; }
        public DateTime? completedDateTime { get; set; }

        public int id { get; set; }
        public bool isCompleted { get; set; }
        public bool isImportant { get; set; }
        public string itemName { get; set; }
        public int? knowledgebaseArticleID { get; set; }
        public int? position { get; set; }
        public int ticketID { get; set; }
        [JsonIgnore]
        public bool isBusy { get; set; }

    }
}
