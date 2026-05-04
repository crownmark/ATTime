using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("DurationTypes", Schema = "dbo")]
    public partial class DurationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DurationTypeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public bool Active { get; set; }

        public ICollection<Duration> Durations { get; set; }
    }
}