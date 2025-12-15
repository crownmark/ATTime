using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("ServiceDeskRoleCache", Schema = "dbo")]
    public partial class ServiceDeskRoleCache
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsDefault { get; set; }

        [Required]
        public int ResourceId { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}