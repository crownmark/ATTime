using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("LenovoPSREFImageData", Schema = "dbo")]
    public partial class LenovoPsrefimageDatum
    {
        [Key]
        [Required]
        public long ImageId { get; set; }

        public LenovoPsreffamilyImage LenovoPsreffamilyImage { get; set; }

        [Required]
        [MaxLength(60)]
        public string ContentType { get; set; }

        [Required]
        public byte[] ImageBytes { get; set; }

        [Required]
        public long ByteLength { get; set; }

        public DateTime StoredAtUtc { get; set; }

        public Guid AccessKey { get; set; }
    }
}