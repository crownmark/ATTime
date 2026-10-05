using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("FavoriteBillingCodes", Schema = "dbo")]
    public partial class FavoriteBillingCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FavoriteBillingCodeId { get; set; }

        [Required]
        public int BillingCodeCacheId { get; set; }

        public BillingCodeCache BillingCodeCache { get; set; }

        [Required]
        public int ResourceCacheId { get; set; }

        public ResourceCache ResourceCache { get; set; }

        public int SortOrder { get; set; }
    }
}