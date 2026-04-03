using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("WorkflowStepTypes", Schema = "dbo")]
    public partial class WorkflowStepType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowStepTypeId { get; set; }

        public bool Active { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public ICollection<WorkflowStep> WorkflowSteps { get; set; }
    }
}