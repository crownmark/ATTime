namespace CrownATTime.Server.Models
{
    public class CalendarEvent
    {
        public long AppointmentId { get; set; }                 // supports both int + long
        public string EventType { get; set; }        // "ServiceCall" or "Appointment"

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int ResourceId { get; set; }
        public int CompanyId { get; set; }
        public int CreatorResourceId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        //Company Todo Specific
        public int? CompanyToDoId { get; set; }

        // ServiceCall-specific
        public int? ServiceCallId { get; set; }
        public int? TicketId { get; set; }
        public int? Status { get; set; }
        public bool? IsComplete { get; set; }

        // UI helpers
        public string Color { get; set; }
        public bool IsAllDay { get; set; }
        //public string DurationFormatted => (End - Start).ToString(@"hh\:mm\:ss");
        public string DurationFormatted
        {
            get
            {
                var duration = End - Start;

                if (duration.TotalMinutes < 1)
                    return "<1min";

                var hours = (int)duration.TotalHours;
                var minutes = duration.Minutes;

                if (hours > 0 && minutes > 0)
                    return $"{hours}hr {minutes}min";

                if (hours > 0)
                    return $"{hours}hr";

                return $"{minutes}min";
            }
        }
        public string TicketNumber { get; set; }
        public string TicketTitle { get; set; }
        public string CustomerName { get; set; }

    }
}
