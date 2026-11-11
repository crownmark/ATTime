using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("LenovoPSREFApprovedSkuImages", Schema = "dbo")]
    public partial class LenovoPsrefapprovedSkuImage
    {
        [Required]
        [MaxLength(64)]
        public string VendorPartNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string FamilyName { get; set; }

        [Required]
        public int CollectionVersion { get; set; }

        [Required]
        public int ImagePosition { get; set; }

        [MaxLength(2048)]
        public string ImageUrl { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }
    }
}