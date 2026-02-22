using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("TimeEntryTemplates", Schema = "dbo")]
    public partial class TimeEntryTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimeEntryTemplateId { get; set; }

        public bool Active { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public int? TicketStatus { get; set; }

        [Required]
        public int BillingCodeId { get; set; }

        public BillingCodeCache BillingCodeCache { get; set; }

        [MaxLength(4000)]
        public string SummaryNotes { get; set; }

        [MaxLength(4000)]
        public string InternalNotes { get; set; }

        public bool AppendToResolution { get; set; }

        [MaxLength(50)]
        public string TemplateAssignedTo { get; set; }

        public bool ShareWithOthers { get; set; }

        public int? EmailTemplateId { get; set; }

        public EmailTemplate EmailTemplate { get; set; }

        public int? TeamsMessageTemplateId { get; set; }

        public TeamsMessageTemplate TeamsMessageTemplate { get; set; }

        public int? Minutes { get; set; }
    }
}