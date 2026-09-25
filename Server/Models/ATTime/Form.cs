using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("Forms", Schema = "dbo")]
    public partial class Form
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        public bool Active { get; set; }

        [Required]
        public int FormCategoryId { get; set; }

        public FormCategory FormCategory { get; set; }

        [Required]
        [MaxLength(500)]
        public string FormUrl { get; set; }
    }
}