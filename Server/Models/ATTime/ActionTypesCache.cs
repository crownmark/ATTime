using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("ActionTypesCache", Schema = "dbo")]
    public partial class ActionTypesCache
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsSystemActionType { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        public int View { get; set; }
    }
}