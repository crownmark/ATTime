using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("WorkflowSteps", Schema = "dbo")]
    public partial class WorkflowStep
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowStepId { get; set; }

        [Required]
        public int WorkflowRuleId { get; set; }

        public WorkflowRule WorkflowRule { get; set; }

        public bool Active { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(4000)]
        public string StepAssignedTo { get; set; }

        [Required]
        public int WorkflowStepTypeId { get; set; }

        public WorkflowStepType WorkflowStepType { get; set; }

        public int? EmailTemplateId { get; set; }

        public EmailTemplate EmailTemplate { get; set; }

        public int? NoteTemplateId { get; set; }

        public NoteTemplate NoteTemplate { get; set; }

        public int? TimeEntryTemplateId { get; set; }

        public TimeEntryTemplate TimeEntryTemplate { get; set; }

        public int? TeamsMessageTemplateId { get; set; }

        public TeamsMessageTemplate TeamsMessageTemplate { get; set; }

        [MaxLength(255)]
        public string ConfirmationDialogTitle { get; set; }

        [MaxLength(4000)]
        public string ConfirmationDialogMessage { get; set; }

        [MaxLength(255)]
        public string NotificationDialogTitle { get; set; }

        [MaxLength(4000)]
        public string NotificationDialogMessage { get; set; }

        [Column("n8nWorkflowUrl")]
        [MaxLength(4000)]
        public string N8nWorkflowUrl { get; set; }

        [Column("n8nWorkflowMethod")]
        [MaxLength(50)]
        public string N8nWorkflowMethod { get; set; }

        [Column("n8nWorkflowNotificationType")]
        [MaxLength(50)]
        public string N8nWorkflowNotificationType { get; set; }

        [Column("n8nWorkflowNotificationTitle")]
        [MaxLength(255)]
        public string N8nWorkflowNotificationTitle { get; set; }

        [Column("n8nWorkflowNotificationWindowWidth")]
        [MaxLength(50)]
        public string N8nWorkflowNotificationWindowWidth { get; set; }

        public int StepOrder { get; set; }

        public bool ConfirmationDialogContinueOnNo { get; set; }
    }
}