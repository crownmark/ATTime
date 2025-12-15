using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("BillingCodeCache", Schema = "dbo")]
    public partial class BillingCodeCache
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int? AfterHoursWorkType { get; set; }

        [Required]
        public int BillingCodeType { get; set; }

        public int? Department { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int UseType { get; set; }
    }
}