using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("LenovoPSREFFamilyImage", Schema = "dbo")]
    public partial class LenovoPsreffamilyImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ImageId { get; set; }

        [Required]
        public long FamilyId { get; set; }

        public LenovoPsreffamily LenovoPsreffamily { get; set; }

        [Required]
        public int ImagePosition { get; set; }

        [MaxLength(2048)]
        public string ImageUrl { get; set; }

        [MaxLength(512)]
        public string BlobName { get; set; }

        [Required]
        [MaxLength(64)]
        public string ImageSha256 { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? SourcePdfPage { get; set; }

        public bool IsApproved { get; set; }

        public ICollection<LenovoPsrefimageDatum> LenovoPsrefimageData { get; set; }
    }
}