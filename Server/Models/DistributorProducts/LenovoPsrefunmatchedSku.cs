using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("LenovoPSREFUnmatchedSKUs", Schema = "dbo")]
    public partial class LenovoPsrefunmatchedSku
    {
        [MaxLength(150)]
        public string VendorPartNumber { get; set; }

        [MaxLength(500)]
        public string ProductTitle { get; set; }

        public string Description { get; set; }

        [MaxLength(200)]
        public string CategoryName { get; set; }
    }
}