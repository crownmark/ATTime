namespace CrownATTime.Server.Models
{
    public class ServiceCallOutlookAppointment
    {
        public int ServiceCallId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Email { get; set; }
        public string Description { get; set; } 
    }
}
