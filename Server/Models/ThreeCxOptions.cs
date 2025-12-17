namespace CrownATTime.Server.Models
{
    public sealed class ThreeCxOptions
    {
        public string TokenUrl { get; set; } = "";
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
        public string? Scope { get; set; }
    }

}
