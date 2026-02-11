using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("TeamsMessageTypes", Schema = "dbo")]
    public partial class TeamsMessageType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamsMessageTypeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public bool Active { get; set; }

        public ICollection<TeamsMessageTemplate> TeamsMessageTemplates { get; set; }
    }
}