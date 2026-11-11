using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("IngramLenovoFamilyImage", Schema = "dbo")]
    public partial class IngramLenovoFamilyImage
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public string MachineType { get; set; }

        [Required]
        [MaxLength(2000)]
        public string ImageUrl { get; set; }

        [MaxLength(2000)]
        public string ReferencePageUrl { get; set; }

        public bool IsApproved { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime UpdatedAtUtc { get; set; }
    }
}