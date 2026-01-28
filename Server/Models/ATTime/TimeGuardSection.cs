using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("TimeGuardSections", Schema = "dbo")]
    public partial class TimeGuardSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimeGuardSectionsId { get; set; }

        [Required]
        [MaxLength(255)]
        public string SectionName { get; set; }

        public bool Active { get; set; }

        public ICollection<AiPromptConfiguration> AiPromptConfigurations { get; set; }
    }
}