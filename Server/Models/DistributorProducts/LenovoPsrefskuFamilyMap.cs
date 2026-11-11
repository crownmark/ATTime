using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("LenovoPSREFSkuFamilyMap", Schema = "dbo")]
    public partial class LenovoPsrefskuFamilyMap
    {
        [Key]
        [Required]
        [MaxLength(64)]
        public string VendorPartNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string FamilyName { get; set; }

        [MaxLength(40)]
        public string MatchMethod { get; set; }

        [MaxLength(20)]
        public string ApprovalStatus { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public DateTime UpdatedAtUtc { get; set; }
    }
}