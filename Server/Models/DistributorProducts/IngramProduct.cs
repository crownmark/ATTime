using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("IngramProduct", Schema = "dbo")]
    public partial class IngramProduct
    {
        [Key]
        [Required]
        [MaxLength(80)]
        public string IngramPartNumber { get; set; }

        [MaxLength(150)]
        public string VendorPartNumber { get; set; }

        [MaxLength(150)]
        public string VendorName { get; set; }

        public string Description { get; set; }

        [MaxLength(200)]
        public string CategoryName { get; set; }

        [MaxLength(200)]
        public string SubCategoryName { get; set; }

        [MaxLength(200)]
        public string ProductTypeName { get; set; }

        [MaxLength(30)]
        public string ApiType { get; set; }

        [Column("UPC")]
        [MaxLength(80)]
        public string Upc { get; set; }

        public bool IsDiscontinued { get; set; }

        public string RawJson { get; set; }

        public DateTime FirstSeenAtUtc { get; set; }

        public DateTime LastSeenAtUtc { get; set; }

        public DateTime LastUpdatedAtUtc { get; set; }

        public int? QuantityAvailable { get; set; }

        public bool? IsAvailable { get; set; }

        public bool? AcceptBackOrder { get; set; }

        public bool? ProductAuthorized { get; set; }

        public decimal? CustomerCost { get; set; }

        [MaxLength(10)]
        public string CurrencyCode { get; set; }

        public DateTime? LastStockAttemptAtUtc { get; set; }

        public DateTime? LastStockCheckAtUtc { get; set; }

        [MaxLength(500)]
        public string LastStockError { get; set; }

        [MaxLength(80)]
        public string SmartPricingTableLineItemId { get; set; }

        public ICollection<IngramProductEnrichment> IngramProductEnrichments { get; set; }

        public ICollection<IngramWarehouseInventory> IngramWarehouseInventories { get; set; }
    }
}