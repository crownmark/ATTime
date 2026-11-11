using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("IngramWarehouseInventory", Schema = "dbo")]
    public partial class IngramWarehouseInventory
    {
        [Key]
        [Required]
        [MaxLength(80)]
        public string IngramPartNumber { get; set; }

        public IngramProduct IngramProduct { get; set; }

        [Key]
        [Required]
        [MaxLength(200)]
        public string WarehouseLocation { get; set; }

        public int? QuantityAvailable { get; set; }

        public int? QuantityBackordered { get; set; }

        public DateTime? QuantityBackorderedEta { get; set; }

        public DateTime LastCheckedAtUtc { get; set; }
    }
}