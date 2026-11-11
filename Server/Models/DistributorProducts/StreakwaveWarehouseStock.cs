using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("StreakwaveWarehouseStock", Schema = "dbo")]
    public partial class StreakwaveWarehouseStock
    {
        [Key]
        [Column("SKU")]
        [Required]
        [MaxLength(150)]
        public string Sku { get; set; }

        public StreakwaveProduct StreakwaveProduct { get; set; }

        [Key]
        [Required]
        [MaxLength(160)]
        public string WarehouseCode { get; set; }

        public int? QuantityAvailable { get; set; }

        [Required]
        public DateTime LastCheckedAtUtc { get; set; }
    }
}