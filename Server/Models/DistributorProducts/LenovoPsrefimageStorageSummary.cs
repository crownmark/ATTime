using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("LenovoPSREFImageStorageSummary", Schema = "dbo")]
    public partial class LenovoPsrefimageStorageSummary
    {
        [Required]
        public int CollectionVersion { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; }

        public int? StoredImages { get; set; }

        public long? StoredBytes { get; set; }
    }
}