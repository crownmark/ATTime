
using System.Text.Json.Serialization;


namespace CrownATTime.Server.Models
{
    public partial class EmailMessage
    {
        public int TemplateId { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }

        public string QuoteLink { get; set; }
        public List<IFormFileModel> Attachments { get; set; }
        public List<EmailAddress> ReplyToList { get; set; }

        public partial class EmailAddress
        {
            public string Email { get; set; }
            public string DisplayName { get; set; }
        }

        public partial class IFormFileModel
        {
            [JsonPropertyName("contentDisposition")]
            public string ContentDisposition { get; set; }

            [JsonPropertyName("contentType")]
            public string ContentType { get; set; }

            [JsonPropertyName("headers")]
            public Headers Headers { get; set; }

            [JsonPropertyName("length")]
            public long Length { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("fileName")]
            public string FileName { get; set; }

            [JsonPropertyName("byteArray")]
            public byte[] ByteArray { get; set; }
        }

        public partial class Headers
        {
            [JsonPropertyName("Content-Disposition")]
            public List<string> ContentDisposition { get; set; }

            [JsonPropertyName("Content-Type")]
            public List<string> ContentType { get; set; }
        }
    }
}
