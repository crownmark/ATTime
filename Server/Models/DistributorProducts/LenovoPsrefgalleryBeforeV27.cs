using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("LenovoPSREFGalleryBeforeV27", Schema = "dbo")]
    public partial class LenovoPsrefgalleryBeforeV27
    {
        [Required]
        [MaxLength(80)]
        public string IngramPartNumber { get; set; }

        public string HtmlProductDetail { get; set; }

        public string ImagesJson { get; set; }

        [Required]
        public int ImageCount { get; set; }

        [Column("LastPSREFImageEdition")]
        public int? LastPsrefimageEdition { get; set; }

        [Column(TypeName="datetime2")]
        [Required]
        public DateTime BackedUpAtUtc { get; set; }
    }
}