using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("AllowedTicketStatuses", Schema = "dbo")]
    public partial class AllowedTicketStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AllowedTicketStatusId { get; set; }

        [Required]
        public int TicketStatusId { get; set; }
    }
}