using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("IngramLenovoScrapeSource", Schema = "dbo")]
    public partial class IngramLenovoScrapeSource
    {
        [Key]
        [Required]
        [MaxLength(150)]
        public string VendorPartNumber { get; set; }

        [Required]
        [MaxLength(2000)]
        public string SourceUrl { get; set; }

        [MaxLength(2000)]
        public string FamilyImageUrl { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? LastScrapeAttemptAtUtc { get; set; }

        public DateTime? LastScrapeSuccessAtUtc { get; set; }

        [MaxLength(1000)]
        public string LastScrapeError { get; set; }

        public int? LastSpecsExtracted { get; set; }

        public int? LastImagesExtracted { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }
    }
}