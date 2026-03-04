namespace CrownATTime.Server.Models
{
    public class ITGlueDocumentAttributesResults
    {
        public string Id { get; set; }
        public int OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public string ResourceUrl { get; set; }

        public bool Restricted { get; set; }

        public bool MyGlue { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool Public { get; set; }

        public int? DocumentFolderId { get; set; }

        public bool IsUploaded { get; set; }

        public int? UserId { get; set; }

        public int? PublishedBy { get; set; }

        public DateTime? PublishedAt { get; set; }
        public bool Archived { get; set; }
    }
}
