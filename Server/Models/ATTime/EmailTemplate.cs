using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("EmailTemplates", Schema = "dbo")]
    public partial class EmailTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailTemplateId { get; set; }

        public bool Active { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public string EmailSubject { get; set; }

        [Required]
        [MaxLength(4000)]
        public string EmailBody { get; set; }

        [MaxLength(255)]
        public string FromEmailAddress { get; set; }

        public bool SendAsTech { get; set; }

        [MaxLength(50)]
        public string TemplateAssignedTo { get; set; }

        public bool ShareWithOthers { get; set; }

        [Required]
        public int NoteType { get; set; }

        [Required]
        [MaxLength(255)]
        public string NoteTitle { get; set; }

        [Required]
        public int NotePublish { get; set; }

        public int? TicketStatus { get; set; }

        [MaxLength(255)]
        public string TeamsChannelEmail { get; set; }

        public bool NotifyTicketContact { get; set; }

        public bool NotifyTicketAdditionalContacts { get; set; }

        public bool NotifyTicketPrimaryResource { get; set; }

        public bool NotifyTicketSecondaryResources { get; set; }
    }
}