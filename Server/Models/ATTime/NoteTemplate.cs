using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("NoteTemplates", Schema = "dbo")]
    public partial class NoteTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteTemplateId { get; set; }

        public bool Active { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public int NoteType { get; set; }

        [Required]
        [MaxLength(255)]
        public string NoteTitle { get; set; }

        [Required]
        public int NotePublish { get; set; }

        public int? TicketStatus { get; set; }

        [MaxLength(4000)]
        public string NoteDescription { get; set; }
    }
}