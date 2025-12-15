using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("RoleCache", Schema = "dbo")]
    public partial class RoleCache
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int RoleType { get; set; }
    }
}