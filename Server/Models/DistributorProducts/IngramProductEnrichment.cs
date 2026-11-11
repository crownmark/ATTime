using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("IngramProductEnrichment", Schema = "dbo")]
    public partial class IngramProductEnrichment
    {
        [Key]
        [Required]
        [MaxLength(80)]
        public string IngramPartNumber { get; set; }

        public IngramProduct IngramProduct { get; set; }

        public string HtmlProductDetail { get; set; }

        [MaxLength(500)]
        public string ProductTitle { get; set; }

        public string MarketingDescription { get; set; }

        public string FeaturesJson { get; set; }

        public string SpecificationsJson { get; set; }

        public string ImagesJson { get; set; }

        [MaxLength(2000)]
        public string ManufacturerProductUrl { get; set; }

        [MaxLength(2000)]
        public string DatasheetUrl { get; set; }

        [MaxLength(120)]
        public string ContentSource { get; set; }

        public bool HasApprovedManufacturerContent { get; set; }

        public int ImageCount { get; set; }

        public string LastDetailsJson { get; set; }

        public DateTime? LastEnrichmentAttemptAtUtc { get; set; }

        public DateTime? LastEnrichmentSuccessAtUtc { get; set; }

        [MaxLength(1000)]
        public string LastEnrichmentError { get; set; }

        [Column("LastPSREFImageEdition")]
        public int? LastPsrefimageEdition { get; set; }
    }
}