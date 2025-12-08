namespace CrownATTime.Server.Models
{
    using System.Text.Json.Serialization;
    public class AutotaskQuery
    {
        [JsonPropertyName("filter")]
        public List<AutotaskFilterCondition> Filter { get; set; } = new();
    }
    public class AutotaskFilterCondition
    {
        [JsonPropertyName("op")]
        public string Operator { get; set; } = "eq";

        [JsonPropertyName("field")]
        public string Field { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public object? Value { get; set; }
    }
}
