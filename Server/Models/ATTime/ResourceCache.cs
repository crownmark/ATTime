using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models.ATTime
{
    [Table("ResourceCache", Schema = "dbo")]
    public partial class ResourceCache
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(50)]
        public string OfficeExtension { get; set; }

        [MaxLength(50)]
        public string OfficePhone { get; set; }

        [MaxLength(255)]
        public string ResourceType { get; set; }

        [MaxLength(255)]
        public string UserName { get; set; }

        public int? LicenseType { get; set; }

        public bool ChecklistItemsCollapsed { get; set; }

        public bool EmailNotesCollapsed { get; set; }

        public bool CompanyDetailsCollapsed { get; set; }

        [Column("AIChatCollapsed")]
        public bool AichatCollapsed { get; set; }

        public bool ContactDetailsCollapsed { get; set; }

        public bool TimeZestCollapsed { get; set; }

        public bool RocketshipCollapsed { get; set; }

        public bool DeviceDetailsCollapsed { get; set; }

        public bool HideTimeDetails { get; set; }

        public int? DefaultEmailTemplate { get; set; }

        public EmailTemplate EmailTemplate { get; set; }

        public int? DefaultTimeEntryTemplate { get; set; }

        public TimeEntryTemplate TimeEntryTemplate { get; set; }

        public int? DefaultTeamsMessageTemplate { get; set; }

        public TeamsMessageTemplate TeamsMessageTemplate { get; set; }

        public int? DefaultNoteTemplate { get; set; }

        public NoteTemplate NoteTemplate { get; set; }

        [Column("DefaultAITemplate")]
        public int? DefaultAitemplate { get; set; }

        public AiPromptConfiguration AiPromptConfiguration { get; set; }

        [Column("ITGluePasswordsCollapsed")]
        public bool ItgluePasswordsCollapsed { get; set; }

        [Column("ITGlueDocumentsCollapsed")]
        public bool ItglueDocumentsCollapsed { get; set; }

        [Column("ITGlueFlexibleAssetsCollapsed")]
        public bool ItglueFlexibleAssetsCollapsed { get; set; }

        [Column("ITGlueConfigurationsCollapsed")]
        public bool ItglueConfigurationsCollapsed { get; set; }

        public bool LiveLinksCollapsed { get; set; }

        public int? TicketRowClickEventActionId { get; set; }

        public int? CalendarSlotClickEventActionId { get; set; }

        public int? CalendarAgendaRowClickEventActionId { get; set; }

        public bool NextActionsCollapsed { get; set; }

        public bool AutoRefreshTicketGrid { get; set; }

        public int AutoRefreshTicketGridMinutes { get; set; }

        public bool AutoRefreshTimeEntryGrid { get; set; }

        public int AutoRefreshTimeEntryGridMinutes { get; set; }

        public bool AutoRefreshCalendar { get; set; }

        public int AutoRefreshCalendarMinutes { get; set; }

        public bool AutoRefreshAgendaGrid { get; set; }

        public int AutoRefreshAgendaGridMinutes { get; set; }

        public int AutoRefreshCalendarNotificationMinutes { get; set; }

        public int? CalendarNotificationEventTypeId { get; set; }

        public CalendarNotificationEventType CalendarNotificationEventType { get; set; }

        public bool CalendarNotificationTargetSound { get; set; }

        public bool CalendarNotificationTargetTeams { get; set; }

        public bool CalendarNotificationTargetTimeGuardDialog { get; set; }
    }
}