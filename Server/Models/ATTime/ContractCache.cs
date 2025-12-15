using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("ContractCache", Schema = "dbo")]
    public partial class ContractCache
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int BillingPreference { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ContractName { get; set; }

        public bool IsDefaultContract { get; set; }

        [Required]
        public int Status { get; set; }
    }
}