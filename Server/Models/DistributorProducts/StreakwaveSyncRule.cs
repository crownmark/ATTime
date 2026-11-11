using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("StreakwaveSyncRule", Schema = "dbo")]
    public partial class StreakwaveSyncRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RuleId { get; set; }

        [MaxLength(160)]
        public string ManufacturerName { get; set; }

        [MaxLength(200)]
        public string CategoryName { get; set; }

        [MaxLength(200)]
        public string SubCategoryName { get; set; }

        [MaxLength(200)]
        public string ProductTypeName { get; set; }

        [MaxLength(200)]
        public string Keyword { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}