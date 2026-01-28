using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("AiPromptConfigurations", Schema = "dbo")]
    public partial class AiPromptConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AiPromptConfigurationId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string Description { get; set; }

        [Required]
        [MaxLength(255)]
        public string MenuName { get; set; }

        [MaxLength(8000)]
        public string SystemPrompt { get; set; }

        [MaxLength(8000)]
        public string UserPrompt { get; set; }

        public bool SharedWithEveryone { get; set; }

        [MaxLength(1000)]
        public string SharedWithUsers { get; set; }

        public bool Active { get; set; }

        public int? TimeGuardSectionsId { get; set; }

        public TimeGuardSection TimeGuardSection { get; set; }
    }
}