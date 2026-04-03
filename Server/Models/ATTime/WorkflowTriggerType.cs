using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("WorkflowTriggerTypes", Schema = "dbo")]
    public partial class WorkflowTriggerType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowTriggerTypeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public bool Active { get; set; }

        public ICollection<WorkflowRule> WorkflowRules { get; set; }
    }
}