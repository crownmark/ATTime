using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models
{
    public class TicketUpdateDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; }
    }
}
