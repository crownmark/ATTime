using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models
{
    public class AutotaskItemsResponse<T>
    {
        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new();

        [JsonPropertyName("pageDetails")]
        public PageDetails PageDetails { get; set; } = new();
    }

    public class PageDetails
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("requestCount")]
        public int RequestCount { get; set; }

        [JsonPropertyName("prevPageUrl")]
        public string? PrevPageUrl { get; set; }

        [JsonPropertyName("nextPageUrl")]
        public string? NextPageUrl { get; set; }
    }
}
