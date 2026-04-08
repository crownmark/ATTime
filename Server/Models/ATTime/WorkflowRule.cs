using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("WorkflowRules", Schema = "dbo")]
    public partial class WorkflowRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowRuleId { get; set; }

        public bool Active { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string TicketCreatedBy { get; set; }

        public int? CompanyId { get; set; }

        public CompanyCache CompanyCache { get; set; }

        public int? StatusId { get; set; }

        public int? PriorityId { get; set; }

        public int? QueueId { get; set; }

        public int? TicketCategoryId { get; set; }

        public int? IssueTypeId { get; set; }

        public int? SubIssueTypeId { get; set; }

        public decimal? TimeEntryHoursWorkedGreaterThan { get; set; }

        public decimal? TimeEntryHoursWorkedLessThan { get; set; }

        public int RuleOrder { get; set; }

        [Required]
        public int WorkflowTriggerTypeId { get; set; }

        public WorkflowTriggerType WorkflowTriggerType { get; set; }

        [MaxLength(255)]
        public string Udf1Name { get; set; }

        [MaxLength(255)]
        public string Udf1Value { get; set; }

        [MaxLength(255)]
        public string Udf2Name { get; set; }

        [MaxLength(255)]
        public string Udf2Value { get; set; }

        [MaxLength(255)]
        public string Udf3Name { get; set; }

        [MaxLength(255)]
        public string Udf3Value { get; set; }

        [MaxLength(255)]
        public string TimeEntryCreatedBy { get; set; }

        [MaxLength(255)]
        public string TicketAssignedTo { get; set; }

        public ICollection<WorkflowStep> WorkflowSteps { get; set; }
    }
}