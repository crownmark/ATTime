using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("ClickEventActions", Schema = "dbo")]
    public partial class ClickEventAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClickEventActionId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public bool Active { get; set; }
    }
}