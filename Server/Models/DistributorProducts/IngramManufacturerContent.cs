using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("IngramManufacturerContent", Schema = "dbo")]
    public partial class IngramManufacturerContent
    {
        [Key]
        [Required]
        [MaxLength(150)]
        public string VendorName { get; set; }

        [Key]
        [Required]
        [MaxLength(150)]
        public string VendorPartNumber { get; set; }

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

        [MaxLength(150)]
        public string SourceName { get; set; }

        [MaxLength(2000)]
        public string SourceUrl { get; set; }

        public bool IsApproved { get; set; }

        public DateTime LastUpdatedAtUtc { get; set; }
    }
}