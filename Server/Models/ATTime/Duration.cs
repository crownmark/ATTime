using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("Durations", Schema = "dbo")]
    public partial class Duration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DurationId { get; set; }

        [Required]
        public int DurationTypeId { get; set; }

        public DurationType DurationType { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public int ValueInMinutes { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }
    }
}