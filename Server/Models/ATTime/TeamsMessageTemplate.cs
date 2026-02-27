using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("TeamsMessageTemplates", Schema = "dbo")]
    public partial class TeamsMessageTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamsMessageTemplateId { get; set; }

        [Required]
        public int TeamsMessageTypeId { get; set; }

        public TeamsMessageType TeamsMessageType { get; set; }

        [MaxLength(255)]
        public string TeamId { get; set; }

        [MaxLength(255)]
        public string ChannelId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(4000)]
        public string Message { get; set; }

        [MaxLength(50)]
        public string TemplateAssignedTo { get; set; }

        public bool ShareWithOthers { get; set; }

        [MaxLength(4000)]
        public string AdaptiveCardTemplate { get; set; }

        public bool Active { get; set; }

        public ICollection<ResourceCache> ResourceCaches { get; set; }

        public ICollection<TimeEntryTemplate> TimeEntryTemplates { get; set; }
    }
}