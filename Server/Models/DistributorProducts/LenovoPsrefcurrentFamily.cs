using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.DistributorProducts
{
    [Table("LenovoPSREFCurrentFamily", Schema = "dbo")]
    public partial class LenovoPsrefcurrentFamily
    {
        [Required]
        public long FamilyId { get; set; }

        [Required]
        [MaxLength(255)]
        public string FamilyName { get; set; }

        public DateTime? FamilyUpdated { get; set; }

        [Required]
        public int StartPdfPage { get; set; }

        [Required]
        public int EndPdfPage { get; set; }

        public string PlatformSpecifications { get; set; }

        [Required]
        public int CollectionVersion { get; set; }

        public DateTime? PublicationDate { get; set; }

        [MaxLength(2048)]
        public string SourcePdfUrl { get; set; }
    }
}