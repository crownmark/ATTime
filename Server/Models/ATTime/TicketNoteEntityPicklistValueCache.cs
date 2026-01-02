using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("TicketNoteEntityPicklistValueCache", Schema = "dbo")]
    public partial class TicketNoteEntityPicklistValueCache
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketNoteEntityPicklistValueId { get; set; }

        [MaxLength(255)]
        public string Value { get; set; }

        [Required]
        [MaxLength(255)]
        public string Label { get; set; }

        public int? ValueInt { get; set; }

        [Required]
        [MaxLength(255)]
        public string PicklistName { get; set; }
    }
}