using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("LenovoPSREFFamily", Schema = "dbo")]
    public partial class LenovoPsreffamily
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FamilyId { get; set; }

        [Required]
        public long EditionId { get; set; }

        public LenovoPsrefedition LenovoPsrefedition { get; set; }

        [Required]
        [MaxLength(255)]
        public string FamilyName { get; set; }

        public DateTime? FamilyUpdated { get; set; }

        [Required]
        public int StartPdfPage { get; set; }

        [Required]
        public int EndPdfPage { get; set; }

        public string PlatformSpecifications { get; set; }

        public DateTime ExtractedAtUtc { get; set; }

        public ICollection<LenovoPsreffamilyImage> LenovoPsreffamilyImages { get; set; }
    }
}