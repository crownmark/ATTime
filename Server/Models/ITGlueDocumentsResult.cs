using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models
{
    public class ITGlueDocumentsResult
    {
        public Datum[] data { get; set; }
        public Meta meta { get; set; }
        public Links links { get; set; }

        public class Meta
        {
            public int currentpage { get; set; }
            public object nextpage { get; set; }
            public object prevpage { get; set; }
            public int totalpages { get; set; }
            public int totalcount { get; set; }
            public Filters filters { get; set; }
        }

        public class Filters
        {
        }

        public class Links
        {
            public string self { get; set; }
            public string first { get; set; }
            public object prev { get; set; }
            public object next { get; set; }
            public string last { get; set; }
        }

        public class Datum
        {
            public string id { get; set; }
            public string type { get; set; }
            public Attributes attributes { get; set; }
            public Relationships relationships { get; set; }
        }

        public class Attributes
        {
            [JsonPropertyName("organization-id")]
            public int OrganizationId { get; set; }

            [JsonPropertyName("organization-name")]
            public string OrganizationName { get; set; }

            [JsonPropertyName("resource-url")]
            public string ResourceUrl { get; set; }

            [JsonPropertyName("restricted")]
            public bool Restricted { get; set; }

            [JsonPropertyName("my-glue")]
            public bool MyGlue { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("created-at")]
            public DateTime? CreatedAt { get; set; }

            [JsonPropertyName("updated-at")]
            public DateTime? UpdatedAt { get; set; }

            [JsonPropertyName("public")]
            public bool Public { get; set; }

            [JsonPropertyName("document-folder-id")]
            public int? DocumentFolderId { get; set; }

            [JsonPropertyName("is-uploaded")]
            public bool IsUploaded { get; set; }

            [JsonPropertyName("user-id")]
            public int? UserId { get; set; }

            [JsonPropertyName("published-by")]
            public int? PublishedBy { get; set; }

            [JsonPropertyName("published-at")]
            public DateTime? PublishedAt { get; set; }

            [JsonPropertyName("archived")]
            public bool Archived { get; set; }

            [JsonPropertyName("content")]
            public Content[] Content { get; set; }

        }

        public class Content
        {
            public int document_id { get; set; }
            public int sort { get; set; }
            public int id { get; set; }
            public object ancestry { get; set; }
            public int resource_id { get; set; }
            public string resource_type { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public int lock_version { get; set; }
            public Resource resource { get; set; }
        }

        public class Resource
        {
            public string content { get; set; }
            public int id { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }

        public class Relationships
        {
        }
    }
}
