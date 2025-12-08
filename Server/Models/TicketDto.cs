namespace CrownATTime.Server.Models
{
    // Models/TicketDto.cs
    using System;
    using System.Text.Json.Serialization;

    public class TicketDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("ticketNumber")]
        public string TicketNumber { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("companyID")]
        public long CompanyId { get; set; }

        [JsonPropertyName("contactID")]
        public long? ContactId { get; set; }

        [JsonPropertyName("dueDateTime")]
        public DateTime? DueDateTime { get; set; }

        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; }
    }

}
