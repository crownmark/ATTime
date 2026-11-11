using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("IngramSyncRule", Schema = "dbo")]
    public partial class IngramSyncRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RuleId { get; set; }

        [Required]
        [MaxLength(150)]
        public string VendorName { get; set; }

        [Required]
        [MaxLength(200)]
        public string CategoryName { get; set; }

        [MaxLength(200)]
        public string SubCategoryName { get; set; }

        [MaxLength(200)]
        public string ProductTypeName { get; set; }

        [MaxLength(30)]
        public string ApiType { get; set; }

        [MaxLength(150)]
        public string Keyword { get; set; }

        public bool IsEnabled { get; set; }

        public int PageSize { get; set; }

        public int MaxPages { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}