namespace CrownATTime.Server.Models
{
    public class GraphApiOptions
    {
        public string TenantId { get; set; } = default!;
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
        public string BaseUrl { get; set; } = "https://graph.microsoft.com/v1.0";
    }
}
