using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("TimeEntries", Schema = "dbo")]
    public partial class TimeEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimeEntryId { get; set; }

        [Column("ATTimeEntryId")]
        public int? AttimeEntryId { get; set; }

        [Column("TicketID")]
        [Required]
        public int TicketId { get; set; }

        [MaxLength(50)]
        public string TicketNumber { get; set; }

        [Column("ResourceID")]
        [Required]
        public int ResourceId { get; set; }

        [Column("RoleID")]
        [Required]
        public int RoleId { get; set; }

        [Column("BillingCodeID")]
        [Required]
        public int BillingCodeId { get; set; }

        [Column("ContractID")]
        public int? ContractId { get; set; }

        public DateTimeOffset? DateWorked { get; set; }

        public DateTimeOffset? StartDateTime { get; set; }

        public DateTimeOffset? EndDateTime { get; set; }

        public decimal? HoursWorked { get; set; }

        public bool IsNonBillable { get; set; }

        public decimal? OffsetHours { get; set; }

        public bool ShowOnInvoice { get; set; }

        public string SummaryNotes { get; set; }

        public string InternalNotes { get; set; }

        public bool IsCompleted { get; set; }

        public bool TimeStampStatus { get; set; }

        public long? DurationMs { get; set; }
    }
}