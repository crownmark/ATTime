using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("StreakwaveProductEnrichment", Schema = "dbo")]
    public partial class StreakwaveProductEnrichment
    {
        [Key]
        [Column("SKU")]
        [Required]
        [MaxLength(150)]
        public string Sku { get; set; }

        public StreakwaveProduct StreakwaveProduct { get; set; }

        [MaxLength(500)]
        public string ProductTitle { get; set; }

        public string ProductOverview { get; set; }

        public string FeaturesJson { get; set; }

        public string SpecificationsJson { get; set; }

        public string ImagesJson { get; set; }

        public string HtmlProductDetail { get; set; }

        [MaxLength(1000)]
        public string SourceUrl { get; set; }

        [MaxLength(30)]
        public string ScrapeStatus { get; set; }

        public DateTime? LastScrapeAttemptAtUtc { get; set; }

        public DateTime? LastScrapeSuccessAtUtc { get; set; }

        [MaxLength(2000)]
        public string LastScrapeError { get; set; }

        [MaxLength(30)]
        public string HtmlVersion { get; set; }

        public DateTime UpdatedAtUtc { get; set; }
    }
}