using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("LenovoPSREFEdition", Schema = "dbo")]
    public partial class LenovoPsrefedition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EditionId { get; set; }

        [Required]
        public int CollectionVersion { get; set; }

        public DateTime? PublicationDate { get; set; }

        [MaxLength(2048)]
        public string SourcePdfUrl { get; set; }

        [Required]
        [MaxLength(64)]
        public string PdfSha256 { get; set; }

        [Required]
        public int PageCount { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }

        public DateTime ImportedAtUtc { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        public ICollection<LenovoPsreffamily> LenovoPsreffamilies { get; set; }
    }
}