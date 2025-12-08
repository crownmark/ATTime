namespace CrownATTime.Server.Models
{
    public class AutotaskOptions
    {
        public const string SectionName = "Autotask";

        /// <summary>
        /// Example: https://webservices5.autotask.net/atservicesrest/v1.0/
        /// or your own proxy base URL.
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;
        /// <summary>
        /// REST API tracking identifier (ApiIntegrationCode).
        /// </summary>
        public string ApiIntegrationCode { get; set; } = string.Empty;

        /// <summary>
        /// API User (Key) - UserName header.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// API User Secret (password).
        /// </summary>
        public string Secret { get; set; } = string.Empty;
    }
}
