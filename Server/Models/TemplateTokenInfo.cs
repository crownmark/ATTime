namespace CrownATTime.Server.Models
{
    public sealed class TemplateTokenInfo
    {
        public string Token { get; init; } = "";
        public string DisplayName { get; init; } = "";
        public string Source { get; init; } = ""; // Contact / Ticket / Resource
        public string PropertyPath { get; init; } = "";
        public Type PropertyType { get; init; } = typeof(string);
        public string? SampleValue { get; init; } // ✅ new
    }
}
