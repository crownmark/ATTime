using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("StreakwaveProductsForQuoting", Schema = "dbo")]
    public partial class StreakwaveProductsForQuoting
    {
        [Column("SKU")]
        [Required]
        [MaxLength(150)]
        public string Sku { get; set; }

        [MaxLength(160)]
        public string ManufacturerPartNumber { get; set; }

        [MaxLength(160)]
        public string ManufacturerName { get; set; }

        [MaxLength(200)]
        public string CategoryName { get; set; }

        [MaxLength(200)]
        public string SubCategoryName { get; set; }

        [MaxLength(200)]
        public string ProductTypeName { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [Column("UPC")]
        [MaxLength(80)]
        public string Upc { get; set; }

        [Column("ProductURL")]
        [MaxLength(1000)]
        public string ProductUrl { get; set; }

        public decimal? CustomerCost { get; set; }

        [Column("MSRP")]
        public decimal? Msrp { get; set; }

        [MaxLength(10)]
        public string CurrencyCode { get; set; }

        public int? QuantityAvailable { get; set; }

        public bool? IsAvailable { get; set; }

        public bool IsDiscontinued { get; set; }

        public string RawCatalogJson { get; set; }

        [Required]
        public DateTime LastCatalogSeenAtUtc { get; set; }

        public DateTime? LastStockAttemptAtUtc { get; set; }

        public DateTime? LastStockCheckAtUtc { get; set; }

        [MaxLength(1000)]
        public string LastStockError { get; set; }

        [Required]
        public DateTime CreatedAtUtc { get; set; }

        [Required]
        public DateTime UpdatedAtUtc { get; set; }

        [Column("MAP")]
        public decimal? Map { get; set; }

        [Column("HasMAP")]
        public bool? HasMap { get; set; }

        public bool? IsRestricted { get; set; }

        [MaxLength(100)]
        public string ProductCondition { get; set; }

        public int? LeadTimeDays { get; set; }

        public DateTime? FeedGeneratedAtUtc { get; set; }

        public DateTime? LastSourceUpdatedAtUtc { get; set; }

        [Column("ImageURL")]
        [MaxLength(1000)]
        public string ImageUrl { get; set; }

        [Column("DatasheetURL")]
        [MaxLength(1000)]
        public string DatasheetUrl { get; set; }

        [MaxLength(80)]
        public string SmartPricingTableLineItemId { get; set; }
    }
}