namespace CrownATTime.Server.Models
{
    // Models/TimeEntryDto.cs
    using System;
    using System.Text.Json.Serialization;

    public class TimeEntryDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("ticketID")]
        public long? TicketId { get; set; }

        [JsonPropertyName("taskID")]
        public long? TaskId { get; set; }

        [JsonPropertyName("resourceID")]
        public int ResourceId { get; set; }

        [JsonPropertyName("roleID")]
        public int RoleId { get; set; }

        [JsonPropertyName("billingCodeID")]
        public int BillingCodeId { get; set; }

        [JsonPropertyName("hoursWorked")]
        public decimal HoursWorked { get; set; }

        [JsonPropertyName("hoursToBill")]
        public decimal? HoursToBill { get; set; }

        [JsonPropertyName("dateWorked")]
        public DateTime DateWorked { get; set; }

        [JsonPropertyName("startDateTime")]
        public DateTime? StartDateTime { get; set; }

        [JsonPropertyName("endDateTime")]
        public DateTime? EndDateTime { get; set; }

        [JsonPropertyName("summaryNotes")]
        public string? SummaryNotes { get; set; }

        [JsonPropertyName("internalNotes")]
        public string? InternalNotes { get; set; }

        [JsonPropertyName("isNonBillable")]
        public bool IsNonBillable { get; set; }
    }

    // For create/update you can re-use or make slimmer DTOs:
    public class TimeEntryCreateDto
    {
        public long? TicketId { get; set; }
        public long? TaskId { get; set; }
        public int ResourceId { get; set; }
        public int? ContractId { get; set; }
        public int RoleId { get; set; }
        public int BillingCodeId { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal? OffsetHours { get; set; }
        //public decimal? HoursToBill { get; set; }
        public DateTimeOffset DateWorked { get; set; }
        public DateTimeOffset? StartDateTime { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
        public string? SummaryNotes { get; set; }
        public string? InternalNotes { get; set; }
        public bool IsNonBillable { get; set; }
    }

    public class TimeEntryUpdateDto
    {
        public decimal? HoursWorked { get; set; }
        public decimal? HoursToBill { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string? SummaryNotes { get; set; }
        public string? InternalNotes { get; set; }
        public bool? IsNonBillable { get; set; }
    }

}
