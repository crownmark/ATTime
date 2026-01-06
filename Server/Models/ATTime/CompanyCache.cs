using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("CompanyCache", Schema = "dbo")]
    public partial class CompanyCache
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("companyName")]
        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }

        [Column("phone")]
        [MaxLength(255)]
        public string Phone { get; set; }

        [Column("companyCategoryID")]
        [Required]
        public int CompanyCategoryId { get; set; }

        [Column("classification")]
        public int? Classification { get; set; }

        [Column("address1")]
        [MaxLength(255)]
        public string Address1 { get; set; }

        [Column("address2")]
        [MaxLength(255)]
        public string Address2 { get; set; }

        [Column("city")]
        [MaxLength(255)]
        public string City { get; set; }

        [Column("state")]
        [MaxLength(255)]
        public string State { get; set; }

        [Column("postalCode")]
        [MaxLength(255)]
        public string PostalCode { get; set; }
    }
}