using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("ResourceCache", Schema = "dbo")]
    public partial class ResourceCache
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(50)]
        public string OfficeExtension { get; set; }

        [MaxLength(50)]
        public string OfficePhone { get; set; }

        [MaxLength(255)]
        public string ResourceType { get; set; }

        [MaxLength(255)]
        public string UserName { get; set; }

        public int? LicenseType { get; set; }

        public bool ChecklistItemsCollapsed { get; set; }

        public bool EmailNotesCollapsed { get; set; }

        public bool CompanyDetailsCollapsed { get; set; }

        [Column("AIChatCollapsed")]
        public bool AichatCollapsed { get; set; }

        public bool ContactDetailsCollapsed { get; set; }

        public bool RocketshipCollapsed { get; set; }

        public bool DeviceDetailsCollapsed { get; set; }

        public bool HideTimeDetails { get; set; }

        public int? DefaultEmailTemplate { get; set; }

        public int? DefaultTimeEntryTemplate { get; set; }

        public int? DefaultTeamsMessageTemplate { get; set; }

        public int? DefaultNoteTemplate { get; set; }

        [Column("DefaultAITemplate")]
        public int? DefaultAitemplate { get; set; }
    }
}